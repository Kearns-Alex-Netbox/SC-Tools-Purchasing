'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: CriticalBuildPath.vb
'
' Description: Select the product(s) that you wish to build along with the quantity. Products can be selected based off of the source
'		source level. Displays to the user how many we can build before the next item runs out along with resulting quantities, prices,
'		
' Checkboxes:
'	Use Inventory = Use the current quantity of the product in the calculations or start with 0. [only applies to the top level product]
'	All Vendors = Include the 2nd and 3rd Vendors and their MPN in the report.
'
' Buttons:
'	Build Products = Take the current list of products that we want to build and open them inside the "BuildProducts" window to run the
'		build report with the exact same list and quantities.
'
' Special Keys:
'   Delete = If we have an item highlighted in our chosenBoards listbox, then we remove it from the listbox.
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient
Imports System.Threading

Public Class CriticalBuildPath

	Const TABLE_INVENTORY As String = "Inventory"
	Const TABLE_PRODUCT As String = "Product Data"

	'Variables used for our list of products to build. <quantity>|<product>
	Const VLINE_DILIMITER As String = "|"
	Const INDEX_QUANTITY As Integer = 0
	Const INDEX_PRODUCT As Integer = 1

	'Used to help describe where the part is found across all of the products.
	Dim levelKey As List(Of String)

	'Used for the toatl times we use a part. Helps us keep track of which part has been added.
	Dim qtyAddedCheck As List(Of String)

	Dim da As New SqlDataAdapter

	'Final results
	Dim final_DataTable As DataTable

	'What our current build shorts are.
	Dim shorts_DataTable As DataTable

	'What our last build shorts are.
	Dim Lastshorts_DataTable As DataTable

	'Inventory to pull from so we can adjust inventory without changing the database.
	Dim tempInventory_DataTable As DataTable

	Dim ds As New DataSet

	Dim InventoryDS As New DataSet
	Dim qbBOMDS As New DataSet
	Dim pcadBOMDS As New DataSet
	Dim cmd = New SqlCommand("", myConn)

	Dim buildingNumber As Integer = 0
	Dim lastCount As Integer = 0
	Dim newCurrentLevel As Integer = 0
	Dim stopLevel As Integer = 1
	Dim searchForPCADItems As Boolean = False
	Dim postAssemblyMessage As Boolean = False
	Dim newProductFirstBuild = False

	Dim formLoaded As Boolean = False

#Region "Thread related items"
	Dim report As New Thread(New ThreadStart(AddressOf GenerateReport))

	Dim progressMax As Integer = 0

	Private Delegate Sub UpdateDatagridDelegate()
	Private Sub UpdateDatagrid()
		If Results_DGV.InvokeRequired Then
			Results_DGV.Invoke(New UpdateDatagridDelegate(AddressOf UpdateDatagrid))
		Else
			'Set up our Data Grid View.
			Results_DGV.DataSource = Nothing
			Results_DGV.DataSource = ds.Tables(0)
			Results_DGV.AutoResizeColumns(DataGridViewAutoSizeColumnMode.AllCells)

			Results_DGV.ClearSelection()
			FormatDGV()
			Results_DGV.Columns(5).Frozen = True

			UpdateGenerateButton()
			UpdateExcelButton()
		End If
	End Sub

	Private Delegate Sub UpdateExcelButtonDelegate()
	Private Sub UpdateExcelButton()
		If Excel_Button.InvokeRequired Then
			Excel_Button.Invoke(New UpdateExcelButtonDelegate(AddressOf UpdateExcelButton))
		Else
			Excel_Button.Enabled = True
		End If
	End Sub

	Private Delegate Sub UpdateGenerateButtonDelegate()
	Private Sub UpdateGenerateButton()
		If GenerateReport_Button.InvokeRequired Then
			GenerateReport_Button.Invoke(New UpdateGenerateButtonDelegate(AddressOf UpdateGenerateButton))
		Else
			GenerateReport_Button.Enabled = True
		End If
	End Sub

	Private Delegate Sub UpdateProgressDelegate()
	Private Sub UpdateProgress()
		If ProgressBar1.InvokeRequired Then
			ProgressBar1.Invoke(New UpdateProgressDelegate(AddressOf UpdateProgress))
		Else
			ProgressBar1.Value = buildingNumber
		End If
	End Sub

	Private Delegate Sub VisableProgressDelegate()
	Private Sub VisableProgress()
		If ProgressBar1.InvokeRequired Then
			ProgressBar1.Invoke(New VisableProgressDelegate(AddressOf VisableProgress))
		Else
			If ProgressBar1.Visible = False Then
				ProgressBar1.Maximum = progressMax
				ProgressBar1.Visible = True
				ProgressBar1.Refresh()
			Else
				ProgressBar1.Visible = False
			End If
		End If
	End Sub
#End Region

	'Helps to see important Items in the report.
	Dim PRODUCT_COLOR As Color = Color.LightGreen
	Dim DATABASE_COLOR As Color = Color.Orange
	Dim ASSEMBLY_COLOR As Color = Color.FromArgb(255, 199, 206)

	Private Sub CriticalBuildPath_Load() Handles MyBase.Load
		'Set our visual guide.
		OrderSoon_Label.BackColor = PRODUCT_COLOR
		Database_Label.BackColor = DATABASE_COLOR
		OutofAssembly_Label.BackColor = ASSEMBLY_COLOR

		'Populate drop-down.
		GetPrefixDropDownItems(SourceLevel_ComboBox)
		SourceLevel_ComboBox.DropDownHeight = 200
		SourceLevel_ComboBox.SelectedIndex = SourceLevel_ComboBox.FindString(PREFIX_FGS)

		GetProducts()

		KeyPreview = True
		formLoaded = True
	End Sub

	Private Sub SetupTables()
		shorts_DataTable = New DataTable
		shorts_DataTable.Columns.Add(DB_HEADER_ITEM_PREFIX, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_MPN, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_VENDOR2, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_MPN2, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_VENDOR3, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_MPN3, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_QTY_AVAIL, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_TO_BUY, GetType(String))
		shorts_DataTable.Columns.Add(DB_HEADER_COST, GetType(Decimal))
		shorts_DataTable.Columns.Add(DB_HEADER_MIN_ORDER_QTY, GetType(Integer))
		shorts_DataTable.Columns.Add(DB_HEADER_LEAD_TIME, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_ASSEMBLY, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_LEVEL_KEY, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_PER_BOARD, GetType(String))
		shorts_DataTable.Columns.Add(HEADER_NEW, GetType(String))

		Lastshorts_DataTable = New DataTable
		Lastshorts_DataTable = shorts_DataTable.Clone()

		final_DataTable = New DataTable
		final_DataTable.Columns.Add(HEADER_ACTION, GetType(String))
		final_DataTable.Columns.Add(HEADER_QUANTITY_WANT, GetType(String))
		final_DataTable.Columns.Add(HEADER_ON_HAND, GetType(String))
		final_DataTable.Columns.Add(HEADER_TO_BUILD, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_ITEM_PREFIX, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_MPN, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_VENDOR2, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_MPN2, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_VENDOR3, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_MPN3, GetType(String))
		final_DataTable.Columns.Add(HEADER_PER_BOARD, GetType(String))
		final_DataTable.Columns.Add(HEADER_QTY_NEEDED, GetType(String))
		final_DataTable.Columns.Add(HEADER_QTY_AVAIL, GetType(String))
		final_DataTable.Columns.Add(HEADER_TO_BUY, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_COST, GetType(Decimal))
		final_DataTable.Columns.Add(HEADER_TOTAL_COST, GetType(Decimal))
		final_DataTable.Columns.Add(DB_HEADER_MIN_ORDER_QTY, GetType(String))
		final_DataTable.Columns.Add(DB_HEADER_LEAD_TIME, GetType(String))
		final_DataTable.Columns.Add(HEADER_LEVEL_KEY, GetType(String))
		final_DataTable.Columns.Add(HEADER_ASSEMBLY, GetType(String))

		tempInventory_DataTable = New DataTable
		tempInventory_DataTable.Columns.Add(DB_HEADER_ITEM_PREFIX, GetType(String))
		tempInventory_DataTable.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		tempInventory_DataTable.Columns.Add(HEADER_QTY_NEEDED, GetType(Integer))
		tempInventory_DataTable.Columns.Add("Last " & HEADER_QTY_NEEDED, GetType(Integer))
	End Sub

#Region "Dual Listbox Methods"

	Private Sub Products_ListBox_DoubleClick() Handles Products_ListBox.DoubleClick
		Call Add_Button_Click()
	End Sub

	Private Sub Add_Button_Click() Handles Add_Button.Click
		'Call a special form that will ask for a quantity.
		Dim doForm As New MessageboxQuantity()
		doForm.ShowDialog()

		If doForm.DialogResult = Windows.Forms.DialogResult.Yes Then
			'Grab the quantity that the user input into the window.
			Dim quantity As Integer = doForm.TB_Quantity.Text

			'Add to our Build List using whatever product is selected in our product list.
			'[ ### | <board>]
			If SourceLevel_ComboBox.Text = PREFIX_FGS = True Then
				BuildProducts_ListBox.Items.Add(quantity & VLINE_DILIMITER & Products_ListBox.SelectedItem)
			Else
				BuildProducts_ListBox.Items.Add(quantity & VLINE_DILIMITER & SourceLevel_ComboBox.Text & "-" & Products_ListBox.SelectedItem)
			End If

		Else
			'The user decided not to make a quantity change so return and do nothing.
			Return
		End If
		'We have changed the situation so disable the excel button so the user does not create under misinformation.
		Excel_Button.Enabled = False
	End Sub

	Private Sub Clear_Button_Click() Handles Clear_Button.Click
		'Clears the whole list of products that we want to build.
		BuildProducts_ListBox.Items.Clear()
		Results_DGV.DataSource = Nothing
		'We have changed the situation so disable the excel button so the user does not create under misinformation.
		Excel_Button.Enabled = False
	End Sub

	Private Sub Remove_Button_Click() Handles Remove_Button.Click
		'Removes the selected product from our list of products that we want to build.
		If BuildProducts_ListBox.SelectedItems.Count <> 0 Then
			BuildProducts_ListBox.Items.Remove(BuildProducts_ListBox.SelectedIndex)
			'We have changed the situation so disable the excel button so the user does not create under misinformation.
			Excel_Button.Enabled = False
		End If
	End Sub

	Private Sub BuildProducts_ListBox_DoubleClick() Handles BuildProducts_ListBox.DoubleClick
		'Handle changing the quantity of a product that has been doubled clicked.
		'Preform a check to see if we have selected an item in the list.
		If BuildProducts_ListBox.SelectedIndex <> -1 Then

			'Call a special form that will ask for a new quantity.
			Dim doForm As New MessageboxQuantity()
			doForm.ShowDialog()

			If doForm.DialogResult = Windows.Forms.DialogResult.Yes Then
				Dim quantity As Integer = doForm.TB_Quantity.Text

				'Parse out the old selected line [ ### | <board>]
				Dim info() As String = BuildProducts_ListBox.SelectedItem.ToString.Split(VLINE_DILIMITER)

				'Find selected line and replace with new information.
				Dim index As Integer = BuildProducts_ListBox.SelectedIndex
				BuildProducts_ListBox.Items(index) = quantity & VLINE_DILIMITER & info(INDEX_PRODUCT)
			Else
				Return
			End If
			'We have changed the situation so disable the excel button so the user does not create under misinformation.
			Excel_Button.Enabled = False
		End If
	End Sub

	Private Sub BuildProducts_ListBox_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles BuildProducts_ListBox.DragDrop
		'Allow the user to re-arange the order in which they would like to build their products.
		Dim point As Point = BuildProducts_ListBox.PointToClient(New Point(e.X, e.Y))
		Dim index As Integer = BuildProducts_ListBox.IndexFromPoint(point)
		If index < 0 Then
			index = BuildProducts_ListBox.Items.Count - 1
		End If

		Dim itemIndex As Integer = BuildProducts_ListBox.SelectedIndex

		Dim data As Object = BuildProducts_ListBox.Items(itemIndex)

		BuildProducts_ListBox.Items.RemoveAt(itemIndex)
		BuildProducts_ListBox.Items.Insert(index, data)
	End Sub

	Private Sub BuildProducts_ListBox_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles BuildProducts_ListBox.DragOver
		e.Effect = DragDropEffects.Move
	End Sub

	Private Sub BuildProducts_ListBox_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles BuildProducts_ListBox.MouseDown
		Try
			If e.Clicks = 2 Then
				Call BuildProducts_ListBox_DoubleClick()
			Else
				BuildProducts_ListBox.DoDragDrop(BuildProducts_ListBox.SelectedItem, DragDropEffects.Move)
			End If
		Catch ex As Exception
			' catching any acidental drags when no item is selected
		End Try
	End Sub

#End Region

	Private Sub GenerateReport_Button_Click() Handles GenerateReport_Button.Click
		'Check to see if we are in the middle of an import.
		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		'Make sure we are building at lease one board.
		If BuildProducts_ListBox.Items.Count = 0 Then
			MsgBox("Please build at least one Product.")
			Return
		End If

		'Check that the stop level has been filled out.
		If StopLevel_TextBox.Text.Length = 0 Then
			MsgBox("Please input a positive whole number for the stop level.")
			Return
		End If

		'Check that the stop level is a positive whole number
		Try
			stopLevel = CInt(StopLevel_TextBox.Text)

			If stopLevel < 0 Then
				MsgBox("Please input a positive whole number for the stop level.")
				Return
			End If
		Catch ex As Exception
			MsgBox("Please input a positive whole number for the stop level.")
			Return
		End Try

		Cursor = Cursors.WaitCursor
		GenerateReport_Button.Enabled = False
		Excel_Button.Enabled = False

		If LOGDATA = True Then
			If ChangeCheck(True) = True Then
				Try
					'Create our thread so we can show the progress bar updating.
					report = New Thread(New ThreadStart(AddressOf GenerateReport))
					report.IsBackground = True

					report.Start()
				Catch ex As Exception
					UnhandledExceptionMessage(ex)
				End Try
			Else
				GenerateReport_Button.Enabled = True
			End If
		Else
			If ChangeCheck(True) = True Then
				Try
					'Create our thread so we can show the progress bar updating.
					report = New Thread(New ThreadStart(AddressOf GenerateReport))
					report.IsBackground = True

					report.Start()
				Catch ex As Exception
					UnhandledExceptionMessage(ex)
				End Try
			Else
				GenerateReport_Button.Enabled = True
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub GenerateReport()
		'Reset our tables, lists, and inventory.
		SetupTables()
		newCurrentLevel = 0
		levelKey = New List(Of String)
		qtyAddedCheck = New List(Of String)
		InventoryDS = New DataSet

		PopulateTables()

		'Check to see if we want to see the extra vendor information
		If AllVendors_CheckBox.Checked = False Then
			final_DataTable.Columns.Remove(DB_HEADER_MPN2)
			final_DataTable.Columns.Remove(DB_HEADER_MPN3)
			final_DataTable.Columns.Remove(DB_HEADER_VENDOR2)
			final_DataTable.Columns.Remove(DB_HEADER_VENDOR3)
		End If

		ds = New DataSet()
		ds.Tables.Add(final_DataTable)

		UpdateDatagrid()
	End Sub

	Private Sub PopulateTables()
		'Fill up our temp inventory table first. When we build multiple products, we want to take away from these numbers.
		cmd.commandtext = "SELECT [" & DB_HEADER_ITEM_NUMBER & "], " &
				"[" & DB_HEADER_ITEM_PREFIX & "], " &
				"[" & DB_HEADER_TYPE & "], " &
				"[" & DB_HEADER_QUANTITY & "] AS '" & HEADER_QTY_AVAIL & "', " &
				"[" & DB_HEADER_COST & "], " &
				"[" & DB_HEADER_VENDOR & "], " &
				"[" & DB_HEADER_MPN & "], " &
				"[" & DB_HEADER_VENDOR2 & "], " &
				"[" & DB_HEADER_MPN2 & "], " &
				"[" & DB_HEADER_VENDOR3 & "], " &
				"[" & DB_HEADER_MPN3 & "], " &
				"[" & DB_HEADER_LEAD_TIME & "], " &
				"[" & DB_HEADER_MIN_ORDER_QTY & "], " &
				"[" & DB_HEADER_QUANTITY & "] AS '" & HEADER_QTY_ORIG & "' from " & TABLE_QB_ITEMS

		da = New SqlDataAdapter(cmd)
		InventoryDS = New DataSet()
		da.Fill(InventoryDS, TABLE_INVENTORY)
		Dim productCounter As Integer = 1

		'We need to loop through all of the items that we want to build.
		For Each build In BuildProducts_ListBox.Items
			'Set our variables to a default state. These will change depending on what assembly level we start on in each of our products.
			searchForPCADItems = False
			postAssemblyMessage = False
			Dim firstLine As Boolean = True

			'This is a flag to show that we are starting a new product and if we are building the first level we want to not subtract any of the inventory just yet.
			newProductFirstBuild = False
			If productCounter > 1 Then
				newProductFirstBuild = True
			End If
			productCounter += 1

			'clear our shorts list each time we start a bew product.
			shorts_DataTable.Rows.Clear()
			Lastshorts_DataTable.Rows.Clear()

			'Split our info from the listbox to get the quantity and the product.
			Dim info() As String = build.ToString.Split(VLINE_DILIMITER)

			final_DataTable.Rows.Add(info(INDEX_QUANTITY) & " - " & info(INDEX_PRODUCT))

			'Check to see if the item is a BAS. If so, we need to look at the PCAD BOM to find POST ASSEMBLY parts
			'for all BAS items that we find.
			If info(INDEX_PRODUCT).Contains(PREFIX_BAS) = True Then
				searchForPCADItems = True
			End If

			'Set our progressbar max to how many we are builing and make it visible.
			buildingNumber = 1
			progressMax = info(INDEX_QUANTITY)
			VisableProgress()

			'Check to see if we have this product as an item inside the inventory database.
			Dim productPrefix As String = ""
			Dim productName As String = ""
			If info(INDEX_PRODUCT).Contains(PREFIX_SMA) = False And info(INDEX_PRODUCT).Contains(PREFIX_BAS) = False And info(INDEX_PRODUCT).Contains(PREFIX_BIS) = False And info(INDEX_PRODUCT).Contains(PREFIX_DAS) = False Then
				productPrefix = PREFIX_FGS
				productName = info(INDEX_PRODUCT)
			Else
				productPrefix = info(INDEX_PRODUCT).Substring(0, info(INDEX_PRODUCT).IndexOf("-"))
				productName = info(INDEX_PRODUCT)
			End If

			Dim returnedRows() As DataRow = InventoryDS.Tables(TABLE_INVENTORY).Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & productPrefix & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & productName & "'")

			If returnedRows.Length <> 0 Then
				'We are in the database.

				'Set up the level key. If we have build this one before, then we need to use the same key.
				'otherwise we need to add a new key to the list.
				If levelKey.Contains(info(INDEX_PRODUCT)) = True Then
					newCurrentLevel = levelKey.IndexOf(info(INDEX_PRODUCT))
				Else
					levelKey.Add(info(INDEX_PRODUCT))
					newCurrentLevel = levelKey.IndexOf(info(INDEX_PRODUCT))
				End If

				'Check to see if we want to start at 0 inventory for the product that we are building.
				If UseInventory_CheckBox.Checked = False Then
					'If we are larger than 0 then that means that we are already building that product and need to keep using that number.
					'We do not want to keep resetting back to 0 if we are building the same product over and over.
					If returnedRows(0)(HEADER_QTY_ORIG) > 0 Then
						returnedRows(0)(HEADER_QTY_ORIG) = 0
						returnedRows(0)(HEADER_QTY_AVAIL) = 0
					End If
				End If

				'This should always pass as 'Can Ship Today' even if it is 0 because 0 - 0 is not < 0.
				For buildingNumber = 1 To (info(INDEX_QUANTITY)) Step 1

					'Grab the current quantity on hand for the build
					Dim quantityAvaliable As Integer = returnedRows(0)(HEADER_QTY_AVAIL)

					'Check to see if we originally have enough on hand for the build.
					If (quantityAvaliable - 1) < 0 Then
						'clear our shorts list each time we start
						shorts_DataTable.Rows.Clear()

						'Check to see if we are looking for POST ASSEBLY parts.
						If searchForPCADItems = True And returnedRows(0)(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_BAS) And stopLevel <> 0 And postAssemblyMessage = False Then
							CheckPostAssembly(returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)
							CheckAssembly(1, info(INDEX_PRODUCT))
						ElseIf stopLevel <> 0 Then
							CheckAssembly(1, info(INDEX_PRODUCT))
						End If

						'Check to see if this is the first row of the product that we are building.
						'Else check to see if the shorts table has a new record added to it. If there is no new record then we continue.
						If firstLine = True Then

							'Add the first line that tells us how many we can ship today total.
							'This is needed for each product that we want to build, even when it is 0. The user needs to know.
							'We subtract one from the building number because one before is what we have before we run into this issue.
							'Then we need to calculate how many we need to build to meet what we would like to ship. Here it should always be '0'.
							final_DataTable.Rows.Add("Can Ship Today", (buildingNumber - 1), CInt(returnedRows(0)(HEADER_QTY_ORIG)), CInt((buildingNumber - 1) - returnedRows(0)(HEADER_QTY_ORIG)))

							'Add any shorts that we have already to have an accurate report
							For Each row As DataRow In shorts_DataTable.Rows
								final_DataTable.Rows.Add(Nothing, Nothing, Nothing, row(HEADER_NEW), row(DB_HEADER_ITEM_PREFIX), row(DB_HEADER_ITEM_NUMBER), row(DB_HEADER_VENDOR), row(DB_HEADER_MPN), row(DB_HEADER_VENDOR2), row(DB_HEADER_MPN2), row(DB_HEADER_VENDOR3), row(DB_HEADER_MPN3), row(HEADER_PER_BOARD), "0", row(HEADER_QTY_AVAIL), "0", row(DB_HEADER_COST), "0.00", row(DB_HEADER_MIN_ORDER_QTY), row(DB_HEADER_LEAD_TIME), row(HEADER_LEVEL_KEY), row(HEADER_ASSEMBLY))
							Next row

							final_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Total Cost", Nothing, "0.00", Nothing, Nothing, Nothing, Nothing)
							final_DataTable.Rows.Add()

							'Set the count to the total rows in our shorts table so we can determine if we have a new component or not.
							lastCount = shorts_DataTable.Rows.Count

							'Move our current shorts over to the previous shorts table.
							Lastshorts_DataTable.Clear()
							Lastshorts_DataTable = shorts_DataTable.Copy

							firstLine = False

						ElseIf shorts_DataTable.Rows.Count <> lastCount Then
							'We have added a new item to our shorts list that will prevent us.
							'We subtract one from the building number because one before is what we have before we run into this issue.
							'Then we need to calculate how many we need to build to meet what we would like to ship.
							final_DataTable.Rows.Add("To Build Today", (buildingNumber - 1), CInt(returnedRows(0)(HEADER_QTY_ORIG)), CInt((buildingNumber - 1) - returnedRows(0)(HEADER_QTY_ORIG)))
							Dim total As Decimal = 0.0

							For Each row As DataRow In shorts_DataTable.Rows
								'See if we added it to our list of total quantity needed.
								Dim needed As String = ""
								Dim returnedRows2() As DataRow = tempInventory_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & row(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & row(DB_HEADER_ITEM_NUMBER).trim & "'")
								If returnedRows2.Count <> 0 Then
									'We have been added to the list and need to increase by the quantityNeeded.
									needed = returnedRows2(0)("Last " & HEADER_QTY_NEEDED)
								Else
									'This is a prat that is not found in the database
									needed = "???"
								End If

								'Calculate the number of parts that are needed.
								Dim numberToBuy As String = 0
								If needed <> "???" And row(HEADER_QTY_AVAIL) <> NOT_IN_DATABASE Then
									numberToBuy = needed - row(HEADER_QTY_AVAIL)
									If numberToBuy < 0 Then
										numberToBuy = 0
									End If
								End If

								'Try to calculate the total cost. Assemblies do not have a cost which is why we use a try/catch.
								Dim totalcost As Decimal = 0.0
								Try
									totalcost = (row(DB_HEADER_COST) * numberToBuy)
								Catch ex As Exception

								End Try

								final_DataTable.Rows.Add(Nothing, Nothing, Nothing, row(HEADER_NEW), row(DB_HEADER_ITEM_PREFIX), row(DB_HEADER_ITEM_NUMBER), row(DB_HEADER_VENDOR), row(DB_HEADER_MPN), row(DB_HEADER_VENDOR2), row(DB_HEADER_MPN2), row(DB_HEADER_VENDOR3), row(DB_HEADER_MPN3), row(HEADER_PER_BOARD), needed, row(HEADER_QTY_AVAIL), numberToBuy, row(DB_HEADER_COST), totalcost, row(DB_HEADER_MIN_ORDER_QTY), row(DB_HEADER_LEAD_TIME), row(HEADER_LEVEL_KEY), row(HEADER_ASSEMBLY))
								Try
									total += totalcost
								Catch ex As Exception
									'Do nothing because there is no price. Should only happen with assemblies.
								End Try
							Next row

							final_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Total Cost", Nothing, total, Nothing, Nothing, Nothing, Nothing)
							final_DataTable.Rows.Add()

							'Set the count to the total rows in our shorts table.
							lastCount = shorts_DataTable.Rows.Count

							'Move our current structure over to the previous structure.
							Lastshorts_DataTable.Clear()
							Lastshorts_DataTable = shorts_DataTable.Copy
						End If
					Else
						'Do Nothing because we have enough on hand to build to the number requested.
					End If

					'Before we move one to the next build, subtract one from our temp Data to simulate us using that inventory.
					returnedRows(0)(HEADER_QTY_AVAIL) -= 1

					UpdateProgress()
					newProductFirstBuild = False
				Next buildingNumber

				'Now we need to handle if we had enough for the whole build or not and add the final entry otherwise we would not get a full report.
				If firstLine = True Then

					shorts_DataTable.Rows.Clear()

					'Check to see if we are looking for POST ASSEBLY parts.
					If searchForPCADItems = True And returnedRows(0)(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_BAS) And stopLevel <> 0 And postAssemblyMessage = False Then
						CheckPostAssembly(returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)
						CheckAssembly(1, info(INDEX_PRODUCT))
					ElseIf stopLevel <> 0 Then
						CheckAssembly(1, info(INDEX_PRODUCT))
					End If

					final_DataTable.Rows.Add("Can Ship Today", info(INDEX_QUANTITY), CInt(returnedRows(0)(HEADER_QTY_ORIG)), (buildingNumber - 1) - returnedRows(0)(HEADER_QTY_ORIG))
					Dim total As Decimal = 0.0

					For Each row As DataRow In shorts_DataTable.Rows
						'See if we added it to our list of total quantity needed.
						Dim needed As String = ""
						Dim returnedRows2() As DataRow = tempInventory_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & row(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & row(DB_HEADER_ITEM_NUMBER).trim & "'")
						If returnedRows2.Count <> 0 Then
							'We have been added to the list and need to increase by the quantityNeeded.
							needed = returnedRows2(0)("Last " & HEADER_QTY_NEEDED)
						Else
							needed = "???"
						End If

						'Calculate the number of parts that are needed.
						Dim numberToBuy As String = 0
						If needed <> "???" And row(HEADER_QTY_AVAIL) <> NOT_IN_DATABASE Then
							numberToBuy = needed - row(HEADER_QTY_AVAIL)
							If numberToBuy < 0 Then
								numberToBuy = 0
							End If
						End If

						'Try to calculate the total cost. Assemblies do not have a cost which is why we use a try/catch.
						Dim totalcost As Decimal = 0.0
						Try
							totalcost = (row(DB_HEADER_COST) * numberToBuy)
						Catch ex As Exception

						End Try

						final_DataTable.Rows.Add(Nothing, Nothing, Nothing, row(HEADER_NEW), row(DB_HEADER_ITEM_PREFIX), row(DB_HEADER_ITEM_NUMBER), row(DB_HEADER_VENDOR), row(DB_HEADER_MPN), row(DB_HEADER_VENDOR2), row(DB_HEADER_MPN2), row(DB_HEADER_VENDOR3), row(DB_HEADER_MPN3), row(HEADER_PER_BOARD), needed, row(HEADER_QTY_AVAIL), numberToBuy, row(DB_HEADER_COST), totalcost, row(DB_HEADER_MIN_ORDER_QTY), row(DB_HEADER_LEAD_TIME), row(HEADER_LEVEL_KEY), row(HEADER_ASSEMBLY))
						Try
							total += totalcost
						Catch ex As Exception
							'Do nothing because there is no price.
						End Try
					Next row

					final_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Total Cost", Nothing, total, Nothing, Nothing, Nothing, Nothing)
					final_DataTable.Rows.Add()
				Else
					shorts_DataTable.Rows.Clear()

					'Check to see if we are looking for POST ASSEBLY parts.
					If searchForPCADItems = True And returnedRows(0)(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_BAS) And stopLevel <> 0 And postAssemblyMessage = False Then
						CheckPostAssembly(returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)
						CheckAssembly(1, info(INDEX_PRODUCT))
					ElseIf stopLevel <> 0 Then
						CheckAssembly(1, info(INDEX_PRODUCT))
					End If

					final_DataTable.Rows.Add("To Build Today", info(INDEX_QUANTITY), CInt(returnedRows(0)(HEADER_QTY_ORIG)), CInt((buildingNumber - 1) - returnedRows(0)(HEADER_QTY_ORIG)))
					Dim total As Decimal = 0.0

					For Each row As DataRow In shorts_DataTable.Rows
						'See if we added it to our list of total quantity needed.
						Dim needed As String = ""
						Dim returnedRows2() As DataRow = tempInventory_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & row(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & row(DB_HEADER_ITEM_NUMBER).trim & "'")
						If returnedRows2.Count <> 0 Then
							'We have been added to the list and need to increase by the quantityNeeded.
							needed = returnedRows2(0)("Last " & HEADER_QTY_NEEDED)
						Else
							needed = "???"
						End If

						'Calculate the number of parts that are needed.
						Dim numberToBuy As String = 0
						If needed <> "???" And row(HEADER_QTY_AVAIL) <> NOT_IN_DATABASE Then
							numberToBuy = needed - row(HEADER_QTY_AVAIL)
							If numberToBuy < 0 Then
								numberToBuy = 0
							End If
						End If

						'Try to calculate the total cost. Assemblies do not have a cost which is why we use a try/catch.
						Dim totalcost As Decimal = 0.0
						Try
							totalcost = (row(DB_HEADER_COST) * numberToBuy)
						Catch ex As Exception

						End Try

						final_DataTable.Rows.Add(Nothing, Nothing, Nothing, row(HEADER_NEW), row(DB_HEADER_ITEM_PREFIX), row(DB_HEADER_ITEM_NUMBER), row(DB_HEADER_VENDOR), row(DB_HEADER_MPN), row(DB_HEADER_VENDOR2), row(DB_HEADER_MPN2), row(DB_HEADER_VENDOR3), row(DB_HEADER_MPN3), row(HEADER_PER_BOARD), needed, row(HEADER_QTY_AVAIL), numberToBuy, row(DB_HEADER_COST), totalcost, row(DB_HEADER_MIN_ORDER_QTY), row(DB_HEADER_LEAD_TIME), row(HEADER_LEVEL_KEY), row(HEADER_ASSEMBLY))
						Try
							total += totalcost
						Catch ex As Exception
							'Do nothing because there is no price.
						End Try
					Next row

					final_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Total Cost", Nothing, total, Nothing, Nothing, Nothing, Nothing)
					final_DataTable.Rows.Add()
				End If
			Else
				'We are not in the database.
				'We should NEVER end up here because we are only allowed to select items from the drop-down which come from the Database itself.
				'Here just incase of a weird bug.
				final_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, info(INDEX_PRODUCT), "???", "???", Nothing, Nothing, Nothing, Nothing, Nothing, "???", NOT_IN_DATABASE, (info(INDEX_QUANTITY) * -1), Nothing, 0.0, Nothing, Nothing, newCurrentLevel, Nothing)
				final_DataTable.Rows.Add()
			End If

			'We now need to update our "On Hand" quantities for the next builds to use.
			'For Each row As DataRow In InventoryDS.Tables(TABLE_INVENTORY).Rows
			'	If row(HEADER_QTY_AVAIL) <> row(HEADER_QTY_ORIG) Then
			'		If row(HEADER_QTY_AVAIL) < 0 Then
			'			'If we are below 0, set the quantity to 0 because we cannot have a negitive inventory.
			'			row(HEADER_QTY_ORIG) = 0
			'		Else
			'			'Else, set what we have available to us as the new "original" quantity.
			'			row(HEADER_QTY_ORIG) = row(HEADER_QTY_AVAIL)
			'		End If
			'	End If
			'Next

			VisableProgress()

			If postAssemblyMessage = True Then
				'Let the user know that if we are working with a BAS and do not find any POST ASSEMBLY Parts. Could help find BOM errors.
				MsgBox("Product " & info(INDEX_PRODUCT) & " does not have any Post Assembly Parts")
			End If
		Next
	End Sub

	Private Sub CheckAssembly(ByRef level As Integer, ByRef queryName As String)
		Dim indent As String = ""

		'We use 2 so the first time we go into this check we do not add any space.
		For index As Integer = 2 To level
			indent = indent & "    "
		Next

		'Check to see if we have already added this table to our list of tables.
		If qbBOMDS.Tables.Contains(queryName.Trim) = False Then
			Dim productPrefix As String = ""
			Dim productName As String = ""
			If queryName.Contains(PREFIX_SMA) = False And queryName.Contains(PREFIX_BAS) = False And queryName.Contains(PREFIX_BIS) = False And queryName.Contains(PREFIX_DAS) = False Then
				productPrefix = PREFIX_FGS
				productName = queryName
			Else
				productPrefix = queryName.Substring(0, queryName.IndexOf("-"))
				productName = queryName.Substring(queryName.IndexOf("-") + 1)
			End If
			cmd.commandText = "IF EXISTS(SELECT * FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] = '" & productName & "' AND [" & DB_HEADER_NAME_PREFIX & "] = '" & productPrefix & "') " &
				"SELECT * FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] = '" & productName & "' AND [" & DB_HEADER_NAME_PREFIX & "] = '" & productPrefix & "' ORDER BY [" & DB_HEADER_TYPE & "] DESC ELSE SELECT '" & DOES_NOT_EXIST & "'"

			Dim myReader As SqlDataReader = cmd.ExecuteReader

			'Check to see if we are in the database before we try to add it to the list of tables.
			If myReader.Read() Then
				If myReader(0) <> DOES_NOT_EXIST Then
					'We are in the database.
					myReader.Close()

					da = New SqlDataAdapter(cmd)
					da.Fill(qbBOMDS, queryName)
				Else
					'We are not in the database.
					myReader.Close()

					shorts_DataTable.Rows.Add("???", indent & queryName, "???", "???", Nothing, Nothing, Nothing, Nothing, NOT_IN_DATABASE, 0, 0, 0, 0.00, Nothing, newCurrentLevel)

					Return
				End If
			End If
		End If

		'Go through each item that we get from our query.
		For Each dsRow As DataRow In qbBOMDS.Tables(queryName).Rows
			'Check to see if we are dealing with a Service Item. These items will always have a quantity of 0.

			'REM REM REM We will probably delete this since we have changed the way we were going to implement it.
			If dsRow(DB_HEADER_TYPE) = "Service" Then
				Continue For
			End If

			Dim returnedRows() As DataRow = InventoryDS.Tables(TABLE_INVENTORY).Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")

			If returnedRows.Length <> 0 Then
				'We are in the database.

				'Get the Assembly information: SMA vs. BAS vs. FGS
				Dim thisAssembly As String = dsRow(DB_HEADER_NAME_PREFIX)

				'Set up our level key.
				If levelKey.Contains(dsRow(DB_HEADER_NAME)) = True Then
					newCurrentLevel = levelKey.IndexOf(dsRow(DB_HEADER_NAME))
				Else
					levelKey.Add(dsRow(DB_HEADER_NAME))
					newCurrentLevel = levelKey.IndexOf(dsRow(DB_HEADER_NAME))
				End If

				'Get the Quantity that is needed to build the remaining quantity.
				Dim quantityNeeded As Integer = dsRow(DB_HEADER_QUANTITY)

				'Update to what we have in the table.
				Dim quantityToUse As Integer = returnedRows(0)(HEADER_QTY_AVAIL)
				Dim minOrderQty As Integer = returnedRows(0)(DB_HEADER_MIN_ORDER_QTY)

				'Check to see if we are dealing with an assembly to adjust our minOrderQty.
				If dsRow(DB_HEADER_TYPE).ToString.Contains("Assembly") = True Then
					minOrderQty = Nothing
				End If

				'Add the quantity needed to get the max that we need for the build.
				Dim returnedRows2() As DataRow = tempInventory_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER).trim & "'")
				If returnedRows2.Count <> 0 Then
					'We have been added to the list and need to increase by the quantityNeeded.
					If newProductFirstBuild = False Then
						returnedRows2(0)("Last " & HEADER_QTY_NEEDED) = returnedRows2(0)(HEADER_QTY_NEEDED)
						returnedRows2(0)(HEADER_QTY_NEEDED) += quantityNeeded
					End If

				Else
					tempInventory_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_NUMBER), quantityNeeded, 0)
				End If

				'Check to see if we have enough on hand to build.
				If newProductFirstBuild = False Then
					quantityToUse -= quantityNeeded
				End If


				If quantityToUse < 0 Then
					'Add the Part/assembly here so if it is an assembly, we know that this is the "physical" run out
					'First, we need to check the old shorts list and see if we have added it durring this run.
					Dim thisRundrs() As DataRow = shorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
					If thisRundrs.Length <> 0 Then
						'If we have found it inside this run list then we need to edit these old numbers to this new entry
						'Add the quantity needed.
						thisRundrs(0)(HEADER_TO_BUY) = (quantityToUse * -1)

						'Check to see if we have added this products quantity to the shorts list yet.
						If qtyAddedCheck.Contains(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName) = False Then
							thisRundrs(0)(HEADER_PER_BOARD) += quantityNeeded
							qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)
						End If

						'Combine the Levels that it is found on.
						If thisRundrs(0)(HEADER_LEVEL_KEY).ToString.Contains(newCurrentLevel) = False Then
							thisRundrs(0)(HEADER_LEVEL_KEY) = thisRundrs(0)(HEADER_LEVEL_KEY).ToString & ", " & newCurrentLevel
						End If

					Else
						'Else, we need to check to see if it was part of the last shorts list.
						Dim lastRundrs() As DataRow = Lastshorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
						If lastRundrs.Length <> 0 Then
							'we have found it inside of the last run so we need to edit these old numbers to this new entry.
							'Add the quantity needed.
							lastRundrs(0)(HEADER_TO_BUY) = (quantityToUse * -1)

							'Check to see if we have added this products quantity to the shorts list yet.
							If qtyAddedCheck.Contains(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName) = False Then
								lastRundrs(0)(HEADER_PER_BOARD) += quantityNeeded
								qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)
							End If

							'Combine the Levels that it is found on.
							If lastRundrs(0)(HEADER_LEVEL_KEY).ToString.Contains(newCurrentLevel) = False Then
								lastRundrs(0)(HEADER_LEVEL_KEY) = lastRundrs(0)(HEADER_LEVEL_KEY).ToString & ", " & newCurrentLevel
							End If

							'Add it to the current run.
							shorts_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), indent & dsRow(DB_HEADER_ITEM_NUMBER), returnedRows(0)(DB_HEADER_VENDOR), returnedRows(0)(DB_HEADER_MPN), returnedRows(0)(DB_HEADER_VENDOR2), returnedRows(0)(DB_HEADER_MPN2), returnedRows(0)(DB_HEADER_VENDOR3), returnedRows(0)(DB_HEADER_MPN3), CInt(returnedRows(0)(HEADER_QTY_ORIG)), (quantityToUse * -1), lastRundrs(0)(DB_HEADER_COST), returnedRows(0)(DB_HEADER_MIN_ORDER_QTY), returnedRows(0)(DB_HEADER_LEAD_TIME), thisAssembly, lastRundrs(0)(HEADER_LEVEL_KEY), lastRundrs(0)(HEADER_PER_BOARD))

						Else
							'If we do not find it in either list then it is a new part that we are adding this run.
							Dim price As Decimal = Nothing
							If dsRow(DB_HEADER_TYPE).ToString.Contains("Assembly") = False Then
								price = returnedRows(0)(DB_HEADER_COST)
							End If

							qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)

							'Add it to the current run
							shorts_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), indent & dsRow(DB_HEADER_ITEM_NUMBER), returnedRows(0)(DB_HEADER_VENDOR), returnedRows(0)(DB_HEADER_MPN), returnedRows(0)(DB_HEADER_VENDOR2), returnedRows(0)(DB_HEADER_MPN2), returnedRows(0)(DB_HEADER_VENDOR3), returnedRows(0)(DB_HEADER_MPN3), CInt(returnedRows(0)(HEADER_QTY_ORIG)), (quantityToUse * -1), price, returnedRows(0)(DB_HEADER_MIN_ORDER_QTY), returnedRows(0)(DB_HEADER_LEAD_TIME), thisAssembly, newCurrentLevel, quantityNeeded, "Next to Run Out")
						End If
					End If

					'Check to see if we are looking for POST ASSEBLY parts that need to be looked in PCAD.
					If quantityToUse <= 0 And searchForPCADItems = True And returnedRows(0)(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_BAS) And dsRow(DB_HEADER_TYPE).ToString.Contains("Assembly") = True And (level + 1) <= stopLevel And postAssemblyMessage = False Then
						'We need to check both post assembly parts and the lower levels.
						CheckPostAssembly(returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)
						CheckAssembly(level + 1, returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)

					ElseIf quantityToUse <= 0 And dsRow(DB_HEADER_TYPE).ToString.Contains("Assembly") = True And (level + 1) <= stopLevel Then
						'We only need to check the lower levels.
						CheckAssembly(level + 1, returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)

					ElseIf quantityToUse <= 0 And searchForPCADItems = True And returnedRows(0)(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_BAS) And postAssemblyMessage = False Then
						'We only need to check the post assembly parts.
						CheckPostAssembly(returnedRows(0)(DB_HEADER_ITEM_NUMBER).ToString)
					End If
				Else
					'Do Nothing because we have enough on hand to build to the number requested.
				End If

				'Before we move one to the next build, subtract one from our temp Data to simulate us using that inventory.
				returnedRows(0)(HEADER_QTY_AVAIL) -= quantityNeeded

			Else
				'We are not in the database.
				'We should not expect to get here if we follow the steps of this program for comparing. 
				'Added as a double check.

				Dim newdrs() As DataRow = shorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")

				If newdrs.Length = 0 Then
					'Add the new component to the list.
					shorts_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), indent & dsRow(DB_HEADER_ITEM_NUMBER), "???", "???", Nothing, Nothing, Nothing, Nothing, NOT_IN_DATABASE, 0, 0, 0, 0.00, Nothing, newCurrentLevel)
				End If
			End If
		Next
	End Sub

	Private Sub CheckPostAssembly(ByRef queryName As String)
		'Drop off the first 4 chars from the board. "123-"
		Dim boardName As String = queryName.Substring(4)

		Dim StockPrefix As String = ""
		Dim StockNumber As String = ""
		Dim thisAssembly As String = "POST"

		'Check to see if we are in our list of tables first.
		If pcadBOMDS.Tables.Contains(boardName) = False Then
			cmd.commandText = "IF EXISTS(SELECT DISTINCT [" & DB_HEADER_ITEM_NUMBER & "], [" & DB_HEADER_ITEM_PREFIX & "], Count([" & DB_HEADER_ITEM_NUMBER & "]) AS Count FROM " & TABLE_PCADBOM & " WHERE UPPER([" & DB_HEADER_BOARD_NAME & "]) = UPPER('" & boardName & "') AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_POSTASSEMBLY & "' Group BY [" & DB_HEADER_ITEM_NUMBER & "], [" & DB_HEADER_ITEM_PREFIX & "])" &
				"SELECT DISTINCT [" & DB_HEADER_ITEM_NUMBER & "], [" & DB_HEADER_ITEM_PREFIX & "], Count([" & DB_HEADER_ITEM_NUMBER & "]) AS Count FROM " & TABLE_PCADBOM & " WHERE UPPER([" & DB_HEADER_BOARD_NAME & "]) = UPPER('" & boardName & "') AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_POSTASSEMBLY & "' Group BY [" & DB_HEADER_ITEM_NUMBER & "], [" & DB_HEADER_ITEM_PREFIX & "] ELSE SELECT '" & DOES_NOT_EXIST & "'"

			Dim myReader As SqlDataReader = cmd.ExecuteReader

			'Check to see if we are in the database before we try to add it to the list of tables.
			If myReader.Read() Then
				If myReader(0) <> DOES_NOT_EXIST Then
					'We are in the database.
					myReader.Close()

					da = New SqlDataAdapter(cmd)
					da.Fill(pcadBOMDS, boardName)
				Else
					'We are not in the database.
					myReader.Close()
					postAssemblyMessage = True
					Return
				End If
			End If
		End If

		For Each dsRow As DataRow In pcadBOMDS.Tables(boardName).Rows
			'First we need to check to see if the items have been added to our master table.
			'if not, we need to add them.
			Dim returnedRows() As DataRow = InventoryDS.Tables(TABLE_INVENTORY).Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")

			If returnedRows.Length <> 0 Then
				'We are in the database.

				'Set up our level key.
				If levelKey.Contains(queryName) = True Then
					newCurrentLevel = levelKey.IndexOf(queryName)
				Else
					levelKey.Add(queryName)
					newCurrentLevel = levelKey.IndexOf(queryName)
				End If

				'Get the Quantity that is needed to build the remaining quantity.
				Dim quantityNeeded As Integer = dsRow("Count")

				'Update to what we have in the table.
				Dim quantityToUse As Integer = returnedRows(0)(HEADER_QTY_AVAIL)
				Dim minOrderQty As Integer = returnedRows(0)(DB_HEADER_MIN_ORDER_QTY)

				'Add the quantity needed to get the max that we need for the build.
				Dim returnedRows2() As DataRow = tempInventory_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER).trim & "'")
				If returnedRows2.Count <> 0 Then
					'We have been added to the list and need to increase by the quantityNeeded.
					If newProductFirstBuild = False Then
						returnedRows2(0)("Last " & HEADER_QTY_NEEDED) = returnedRows2(0)(HEADER_QTY_NEEDED)
						returnedRows2(0)(HEADER_QTY_NEEDED) += quantityNeeded
					End If

				Else
					tempInventory_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_NUMBER), quantityNeeded)
				End If

				'Check to see if we have enough on hand to build.
				quantityToUse -= quantityNeeded
				If quantityToUse < 0 Then

					'Add the Part/assembly here so if it is an assembly, all the parts appear below it.
					'First, we need to check the old shorts list and see if we have added it durring this run.
					Dim thisRundrs() As DataRow = shorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
					If thisRundrs.Length <> 0 Then
						'If we have found it inside this run list then we need to edit these old numbers to this new entry
						'Add the quantity needed.
						thisRundrs(0)(HEADER_TO_BUY) = (quantityToUse * -1)

						'Check to see if we have added this products quantity to the shorts list yet.
						If qtyAddedCheck.Contains(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName) = False Then
							thisRundrs(0)(HEADER_PER_BOARD) += quantityNeeded
							qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)
						End If

						'Combine the Levels that it is found on.
						If thisRundrs(0)(HEADER_LEVEL_KEY).ToString.Contains(newCurrentLevel) = False Then
							thisRundrs(0)(HEADER_LEVEL_KEY) = thisRundrs(0)(HEADER_LEVEL_KEY).ToString & ", " & newCurrentLevel
						End If

					Else
						'Else, we need to check to see if it was part of the last shorts list.
						Dim lastRundrs() As DataRow = Lastshorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
						If lastRundrs.Length <> 0 Then
							'we have found it inside of the last run so we need to edit these old numbers to this new entry.
							'Add the quantity needed.
							lastRundrs(0)(HEADER_TO_BUY) = (quantityToUse * -1)

							'Check to see if we have added this products quantity to the shorts list yet.
							If qtyAddedCheck.Contains(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName) = False Then
								lastRundrs(0)(HEADER_PER_BOARD) += quantityNeeded
								qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)
							End If

							'Combine the Levels that it is found on.
							If lastRundrs(0)(HEADER_LEVEL_KEY).ToString.Contains(newCurrentLevel) = False Then
								lastRundrs(0)(HEADER_LEVEL_KEY) = lastRundrs(0)(HEADER_LEVEL_KEY).ToString & ", " & newCurrentLevel
							End If

							'Add it to the current run.
							shorts_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_NUMBER), returnedRows(0)(DB_HEADER_VENDOR), returnedRows(0)(DB_HEADER_MPN), returnedRows(0)(DB_HEADER_VENDOR2), returnedRows(0)(DB_HEADER_MPN2), returnedRows(0)(DB_HEADER_VENDOR3), returnedRows(0)(DB_HEADER_MPN3), CInt(returnedRows(0)(HEADER_QTY_ORIG)), (quantityToUse * -1), lastRundrs(0)(DB_HEADER_COST), returnedRows(0)(DB_HEADER_MIN_ORDER_QTY), returnedRows(0)(DB_HEADER_LEAD_TIME), thisAssembly, lastRundrs(0)(HEADER_LEVEL_KEY), lastRundrs(0)(HEADER_PER_BOARD))

						Else
							'If we do not find it in either list then it is a new part that we are adding this run.
							Dim price As Decimal = returnedRows(0)(DB_HEADER_COST)

							qtyAddedCheck.Add(dsRow(DB_HEADER_ITEM_NUMBER) & "," & queryName)

							'Add it to the current run
							shorts_DataTable.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_NUMBER), returnedRows(0)(DB_HEADER_VENDOR), returnedRows(0)(DB_HEADER_MPN), returnedRows(0)(DB_HEADER_VENDOR2), returnedRows(0)(DB_HEADER_MPN2), returnedRows(0)(DB_HEADER_VENDOR3), returnedRows(0)(DB_HEADER_MPN3), CInt(returnedRows(0)(HEADER_QTY_ORIG)), (quantityToUse * -1), price, returnedRows(0)(DB_HEADER_MIN_ORDER_QTY), returnedRows(0)(DB_HEADER_LEAD_TIME), thisAssembly, newCurrentLevel, quantityNeeded, "Next to Run Out")
						End If
					End If
				Else
					'Do Nothing because we have enough on hand to build to the number requested.
				End If

				'Before we move one to the next build, subtract one from our temp Data to simulate us using that inventory.
				returnedRows(0)(HEADER_QTY_AVAIL) -= quantityNeeded

			Else
				'We are not in the database.
				Dim newdrs() As DataRow = shorts_DataTable.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX).trim & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")

				If newdrs.Length = 0 Then
					'Add the new component to the list.
					shorts_DataTable.Rows.Add("???", dsRow(DB_HEADER_ITEM_NUMBER), "???", "???", Nothing, Nothing, Nothing, Nothing, NOT_IN_DATABASE, 0, 0, 0, 0.00, Nothing, newCurrentLevel)
				End If
			End If
		Next
	End Sub

	Private Sub FormatDGV()
		'Go through the DGV and hilight the different alerts that we want the user to be able to see right away.
		For index = 0 To Results_DGV.Rows.Count - 1
			'Look for our product headers and highlight them.
			If IsDBNull(Results_DGV.Rows(index).Cells(HEADER_ACTION).Value) = False Then
				If Results_DGV.Rows(index).Cells(HEADER_ACTION).Value.contains("-") = True Then
					Results_DGV.Rows(index).Cells(HEADER_ACTION).Style.BackColor = PRODUCT_COLOR
				End If
			End If

			'Look for assemblies and highlight them.
			If IsDBNull(Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value) = False Then
				If Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value.contains("BAS-") = True Or Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value.contains("SMA-") = True Or Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value.contains("BIS-") = True Or Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value.contains("DAS-") = True Then
					Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Style.BackColor = ASSEMBLY_COLOR
				End If
			End If

			'Look for a "NOT IN DATABASE" item.
			If IsDBNull(Results_DGV.Rows(index).Cells(DB_HEADER_VENDOR).Value) = False Then
				If Results_DGV.Rows(index).Cells(DB_HEADER_VENDOR).Value.contains("???") = True Then
					Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Style.BackColor = DATABASE_COLOR
				End If
			End If
		Next
	End Sub

	Private Sub Excel_Button_Click() Handles Excel_Button.Click
		Dim report As New GenerateReport
		report.GenerateCriticalBuildReport(BuildProducts_ListBox, ds, UseInventory_CheckBox.Checked, levelKey)
	End Sub

	Private Sub Close_Button_Click() Handles Close_Button.Click
		Close()
	End Sub

	Private Sub isClosing() Handles MyBase.Closing
		report.Abort()
	End Sub

	Private Sub GetPrefixDropDownItems(ByRef box As ComboBox)
		Dim ProductPrefixes As New DataTable()

		Dim myCmd As New SqlCommand("SELECT Distinct([" & DB_HEADER_NAME_PREFIX & "]) FROM " & TABLE_QBBOM, myConn)

		ProductPrefixes.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In ProductPrefixes.Rows
			box.Items.Add(dr(DB_HEADER_NAME_PREFIX))
		Next
	End Sub

	Private Sub SourceLevel_ComboBox_SelectedValueChanged() Handles SourceLevel_ComboBox.SelectedValueChanged
		If formLoaded = True Then
			Excel_Button.Enabled = False
			GetProducts()
		End If
	End Sub

	Private Sub GetProducts()
		'Populate Board drop-down.
		Products_ListBox.Items.Clear()

		Dim ProductNames As New DataTable()
		Dim myCmd As New SqlCommand("SELECT Distinct([" & DB_HEADER_NAME & "]) FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME_PREFIX & "] = '" & SourceLevel_ComboBox.Text & "' ORDER BY [" & DB_HEADER_NAME & "]", myConn)

		ProductNames.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In ProductNames.Rows
			Products_ListBox.Items.Add(dr(DB_HEADER_NAME))
		Next
	End Sub

	Private Sub MyBase_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
		'If we press the Delete key, call our remove function.
		If e.KeyCode.Equals(Keys.Delete) Then
			Call Remove_Button_Click()
		End If
	End Sub

	Private Sub CheckBox_CheckedChanged() Handles UseInventory_CheckBox.CheckedChanged
		Excel_Button.Enabled = False
	End Sub

	Private Sub BuildProducts_Button_Click() Handles BuildProducts_Button.Click
		If MenuMain.OpenForm(BuildProducts) = False Then
			Return
		Else
			'Send the list and quantities over to build products.
			BuildProducts.transferCriticalBuild(BuildProducts_ListBox)
		End If
	End Sub

	Public Sub transferBuildProducts(ByRef list As ListBox)
		BuildProducts_ListBox.Items.Clear()
		Excel_Button.Enabled = False

		For Each item In list.Items
			'Update list with what we recived.
			BuildProducts_ListBox.Items.Add(item)
		Next
	End Sub

End Class