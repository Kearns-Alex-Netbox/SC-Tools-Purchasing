'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: BuildProducts.vb
'
' Description: Select the product(s) that you wish to build along with the quantity. Products can be selected based off of the source
'		source level. Multiple reports can be made based off of the checkboxes and the type of report that you would like to produce.
'
' Context Menu: Right-click menu used in the DGV.
'	Print = Brings up the "Printing" window that handles creating label(s) or coversheet(s) for the item(s) that are selected.
'
' Checkboxes:
'	Use Inventory = Use the current quantity of the product in the calculations or start with 0. [only applies to the top level product]
'	Show All = Show all the items [up to the stoping level] regardless of their status.
'	All Vendors = Include the 2nd and 3rd Vendors and their MPN in the report.
'
' Stop Level: The number of times to drill down an assembly until we do not drill down anymore. 1 and 2 are most commonly used.
'
' Buttons:
'	Generate Report = Generates the basic report that shows all assemblies and items with a tier effect to show the level they are
'		associated with. Uses all of the checkboxes.
'	Order Report = Generates a report that combines all of the like parts to get a grand total over the whole report without assemblies.
'		Uses all Of the checkboxes.
'	Annual Usage = Generates a report that combines all of the like parts and where ALL inventory quantites are 0 to show the true number
'		of parts that we use without assemblies. Commonly used to calculate what we use on a yearly average. Does not use 'Use Inventory'
'		checkbox.
'	Critical Build = Take the current list of products that we want to build and open them inside the "CriticalBuild" window to run the
'		critical build report with the exact same list and quantities.
'
' Status:
'	Out of Stock = We have a negitive (-) quantity after running the report. These items are need to be purchased to satisfy the report.
'	Out of Assembly = We have a negitive (-) quantity after running the report. These assemblies need to be built to astisfy the report.
'	Order Soon = We have a positive (+) quantity after running the report and based on the re-order quantity of the item, these parts
'		are in danger of running out soon and might need to be considered on being purchased. [Default set to 50]
'	Not in Database = the item was not found in the database.
'
' Special Keys:
'   delete = If we have an item highlighted in our chosenBoards listbox, then we remove it from the listbox.
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient

Public Class BuildProducts
	'Variables used for our list of products to build. <quantity>|<product>
	Const QUANTITY_INDEX As Integer = 0
	Const PRODUCT_INDEX As Integer = 1

	'Reports
	Public Const REPORT As Integer = 1
	Public Const ORDER As Integer = 2
	Public Const ANUAL As Integer = 3

	Public Const DEFAULT_REORDER_QT As Integer = 50

	'Variables used for our level system. #.#
	'The 1st # = the level this item is found on. Increases and decreases.
	'The 2nd # = unique, [increasing only] number for each different assembly type found.
	Const MAX_RUNNING_NUMBERS = 50                      'The max number of array index that we can have. Increase this number if needed.
	Dim RunningNumbers(MAX_RUNNING_NUMBERS) As Integer  'Array used to store the unique 2nd number in our level key system.
	Dim CurrentLevel As Integer = 0                     'Points to the array index and used as what level we are on.
	Dim levelKey As List(Of String)
	Dim uniqueParts As List(Of String)
	'This is used to grab the post assembly parts that NEED to be accounted for when we are building BAS products.
	Dim searchForPCADItems As Boolean = False

	'Styles used to help see results on the datagrid.
	Dim OUT_COLOR As Color = Color.FromArgb(255, 151, 163)
	Dim ORDER_COLOR As Color = Color.FromArgb(255, 235, 156)
	Dim DATABASE_COLOR As Color = Color.Orange
	Dim ASSEMBLY_COLOR As Color = Color.FromArgb(255, 199, 206)
	Dim GOOD_COLOR As Color = Color.LightGreen

	Dim master_DataTable As DataTable

	Dim result_DataTable As DataTable
	Dim result_da As New SqlDataAdapter
	Dim result_ds As New DataSet
	Dim result_Cmd = New SqlCommand("", myConn)

	'Determines how far down we drill into our assemblies.
	Dim stopLevel As Integer = 1

	'Determines the total cost to build.
	Dim masterCost As Decimal = 0.0

	'Flags
	Dim formLoaded As Boolean = False
	Dim wasAnualLast As Boolean = False

	'Set up the Context menu
	Dim mnuCell As New ContextMenuStrip

	'Tables to make the report run faster.
	Dim QBItemList As DataTable
	Dim QBBOMList As DataTable

	Private Sub BuildProducts_load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		'Set our visual guide.
		OutofStock_Label.BackColor = OUT_COLOR
		OrderSoon_Label.BackColor = ORDER_COLOR
		Database_Label.BackColor = DATABASE_COLOR
		OutofAssembly_Label.BackColor = ASSEMBLY_COLOR

		'Populate drop-down.
		GetPrefixDropDownItems(SourceLevel_ComboBox)
		SourceLevel_ComboBox.DropDownHeight = 200
		SourceLevel_ComboBox.SelectedIndex = SourceLevel_ComboBox.FindString(PREFIX_FGS)

		GetProducts()

		'We need to add each menu strip to the menu and add its own handdler location
		With mnuCell
			Dim newMenu As New ToolStripMenuItem("Print", Nothing, AddressOf cMenu_Click)
			.Items.Add(newMenu)
		End With

		KeyPreview = True
		formLoaded = True
	End Sub

	Private Sub SetupTable(ByRef buttonCaller As String)
		'Set up our results DataTable. This is what will be used on the screen.
		result_DataTable = New DataTable
		result_DataTable.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_MPN, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_VENDOR2, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_MPN2, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_VENDOR3, GetType(String))
		result_DataTable.Columns.Add(DB_HEADER_MPN3, GetType(String))
		result_DataTable.Columns.Add(HEADER_QTY_ORIG, GetType(String))
		result_DataTable.Columns.Add(HEADER_QTY_AVAIL, GetType(String))
		result_DataTable.Columns.Add(HEADER_QTY_NEEDED, GetType(String))
		result_DataTable.Columns.Add(HEADER_REMAINDER, GetType(Integer))
		result_DataTable.Columns.Add(DB_HEADER_COST, GetType(Decimal))
		result_DataTable.Columns.Add(HEADER_TO_BUY, GetType(String))
		result_DataTable.Columns.Add(HEADER_TOTAL_COST, GetType(Decimal))
		result_DataTable.Columns.Add(DB_HEADER_MIN_ORDER_QTY, GetType(Decimal))
		result_DataTable.Columns.Add(DB_HEADER_LEAD_TIME, GetType(Decimal))

		result_DataTable.Columns.Add(HEADER_LEVEL_KEY, GetType(String))
		result_DataTable.Columns.Add(HEADER_ASSEMBLY, GetType(String))

		'We only need a new master table if we are changing the conditions of the report we want.
		'Handled with textbox, list, and checkbox change
		'Used as a master list that we then copy and filter from depending on what reprot we run.
		If Excel_Button.Enabled = False Or buttonCaller = ANUAL Or wasAnualLast = True Then
			master_DataTable = New DataTable
			master_DataTable = result_DataTable.Clone
			master_DataTable.Columns.Add(HEADER_LEVEL, GetType(Integer))
			master_DataTable.Columns.Add(DB_HEADER_TYPE, GetType(String))
			master_DataTable.Columns.Add(DB_HEADER_REORDER_QTY, GetType(Integer))
		End If
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
			BuildProducts_ListBox.Items.RemoveAt(BuildProducts_ListBox.SelectedIndex)
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
				BuildProducts_ListBox.Items(index) = quantity & VLINE_DILIMITER & info(PRODUCT_INDEX)
			Else
				'The user decided not to make a quantity change so return and do nothing.
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
		'Create a general list of item numbers with indentations to show the relationship each part has and where it is located.
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					GenerateReport(REPORT)
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				GenerateReport(REPORT)
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub OrderReport_Button_Click() Handles OrderReport_Button.Click
		'Combining all like item numbers together to show how many we need to buy and the total cost over all of the products 
		' that we have selected to build.
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					GenerateReport(ORDER)
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				GenerateReport(ORDER)
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub AnualUsage_Button_Click() Handles AnualUsage_Button.Click
		'Create an anual report of what is on the build list. It will zero out all of the inventory to get how much money we spend.
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					GenerateReport(ANUAL)
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				GenerateReport(ANUAL)
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub GenerateReport(ByRef buttonCaller As Integer)
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

		'Make sure that we have a stop level and that it is a number.
		If StopLevel_TextBox.Text.Length = 0 Then
			MsgBox("Please input a whole number for the stop level.")
			Return
		End If
		Try
			stopLevel = CInt(StopLevel_TextBox.Text)
		Catch ex As Exception
			MsgBox("Please input a whole number for the stop level.")
			Return
		End Try

		SetupTable(buttonCaller)

		'Populate our helper tables.
		QBItemList = New DataTable
		Dim myCmd As New SqlCommand("SELECT * FROM " & TABLE_QB_ITEMS, myConn)
		QBItemList.Load(myCmd.ExecuteReader())

		QBBOMList = New DataTable
		myCmd.CommandText = "SELECT * FROM " & TABLE_QBBOM
		QBBOMList.Load(myCmd.ExecuteReader())

		'Check to see if we are in a situation that needs to rebuild the master table.
		If buttonCaller = ANUAL Then
			'Special Condition where we make EVERYTHING 0 to see the TOTAL parts we need for a given amount.
			Array.Clear(RunningNumbers, 0, MAX_RUNNING_NUMBERS)
			CurrentLevel = 0
			levelKey = New List(Of String)
			uniqueParts = New List(Of String)

			CreateMasterTable(True)
			wasAnualLast = True
		ElseIf wasAnualLast = True Then
			'Special Condition where we make EVERYTHING 0 to see the TOTAL parts we need for a given amount.
			Array.Clear(RunningNumbers, 0, MAX_RUNNING_NUMBERS)
			CurrentLevel = 0
			levelKey = New List(Of String)
			uniqueParts = New List(Of String)

			CreateMasterTable(False)
			wasAnualLast = False
		Else
			If Excel_Button.Enabled = False Then
				'We only disable the Excel button when we are creating a new report. That means we need to reset our level key.
				Array.Clear(RunningNumbers, 0, MAX_RUNNING_NUMBERS)
				CurrentLevel = 0
				levelKey = New List(Of String)
				uniqueParts = New List(Of String)

				CreateMasterTable(False)
			End If
		End If

		'Filter our master report to get what we want.
		If buttonCaller = REPORT Then
			FilterMasterTable(REPORT)
		Else
			'Special condition where we need to combine like items and have a total price.
			FilterMasterTable(ORDER)

			'Create our original table as a new table and copy the data over from the temp DataTable.
			Dim objDv As New DataView(result_DataTable)
			objDv.Sort = HEADER_REMAINDER & " ASC"

			result_DataTable = New DataTable
			result_DataTable = objDv.ToTable.Copy

			result_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Total Expenses", Nothing, Nothing, Nothing, masterCost, Nothing, Nothing, Nothing, Nothing)
		End If

		'Check to see if we want to remove our extra Vendors
		If AllVendors_CheckBox.Checked = False Then
			result_DataTable.Columns.Remove(DB_HEADER_MPN2)
			result_DataTable.Columns.Remove(DB_HEADER_MPN3)
			result_DataTable.Columns.Remove(DB_HEADER_VENDOR2)
			result_DataTable.Columns.Remove(DB_HEADER_VENDOR3)
		End If

		'Set up our data set.
		result_ds = New DataSet()
		result_ds.Tables.Add(result_DataTable)

		'Set up our Data Grid View.
		Results_DGV.DataSource = Nothing
		Results_DGV.DataSource = result_ds.Tables(0)
		Results_DGV.AutoResizeColumns(DataGridViewAutoSizeColumnMode.AllCells)
		FormatDGV()

		Results_DGV.ClearSelection()
		Results_DGV.Columns(0).Frozen = True
		Excel_Button.Enabled = True
	End Sub

	Private Sub CreateMasterTable(ByRef isAnualReport As Boolean)
		'Go through each of our products that we want to build.
		For Each build In BuildProducts_ListBox.Items
			searchForPCADItems = False

			'Split the information from our buld products list.
			Dim info() As String = build.ToString.Split(VLINE_DILIMITER)
			Dim productPrefix As String = ""
			Dim productName As String = ""

			'Depending on what level this item was added will determine how we get the name and prefix out of it.
			If info(PRODUCT_INDEX).Contains(PREFIX_SMA) = False And info(PRODUCT_INDEX).Contains(PREFIX_BAS) = False And info(PRODUCT_INDEX).Contains(PREFIX_BIS) = False And info(PRODUCT_INDEX).Contains(PREFIX_DAS) = False Then
				productPrefix = PREFIX_FGS
				productName = info(PRODUCT_INDEX)
			Else
				productPrefix = info(PRODUCT_INDEX).Substring(0, info(PRODUCT_INDEX).IndexOf("-"))
				productName = info(PRODUCT_INDEX).Substring(info(PRODUCT_INDEX).IndexOf("-") + 1)
			End If

			'Build next level query.
			Dim query As String = "[" & DB_HEADER_NAME & "] = '" & productName & "' AND [" & DB_HEADER_NAME_PREFIX & "] = '" & productPrefix & "'"

			'Check to see if the item is a BAS. If so, we need to look at the PCAD BOM to find POST ASSEMBLY parts
			'for all BAS items that we find.
			If info(PRODUCT_INDEX).Contains(PREFIX_BAS) = True Then
				searchForPCADItems = True
			End If

			'Grab the basic information of the item
			Dim Itemsdrs() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & info(PRODUCT_INDEX) & "'")

			If Itemsdrs.Length <> 0 Then
				'The item was found in the database.
				'Assign Values into Variables.
				Dim itemNumber As String = Itemsdrs(0)(DB_HEADER_ITEM_PREFIX) & ":" & Itemsdrs(0)(DB_HEADER_ITEM_NUMBER)
				Dim partNumber As String = Itemsdrs(0)(DB_HEADER_MPN)
				Dim vendor As String = Itemsdrs(0)(DB_HEADER_VENDOR)
				Dim quantityNeeded As Integer = info(QUANTITY_INDEX)
				Dim quantityAvaliable As Integer = Itemsdrs(0)(DB_HEADER_QUANTITY)
				Dim remainder_Value As Integer = 0

				'Check to see if we want to start at 0.
				If UseInventory_CheckBox.Checked = False Or isAnualReport = True Then
					quantityAvaliable = 0
				Else
					'See if we have already used this stock number previously in another build.
					Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & itemNumber & "%'")

					If drs.Length <> 0 Then
						'We have used it already so we need to use their remainder.
						Dim newQuantity As Integer = CInt(drs(0).Item(HEADER_REMAINDER))
						For Each row As DataRow In drs
							'We want to get the one that has the lowest Remainder.
							If CInt(row.Item(HEADER_REMAINDER)) < newQuantity Then
								newQuantity = CInt(row.Item(HEADER_REMAINDER))
							End If
						Next
						quantityAvaliable = newQuantity
					End If
				End If

				'Calculate the remainder.
				remainder_Value = quantityAvaliable - quantityNeeded

				Dim leftToBuild As Integer = 0

				'Check to see if we need to build or purcahse
				If remainder_Value < 0 Then
					'If we have a negitive number than we need to build or purchase.
					'Multiply by -1 because you cannot buy negative inventory.
					leftToBuild = remainder_Value * -1
				End If

				'This check is to prevent us from doubling the ammount of products that we need to build.
				'If we are at -130 and need to build 10, the REMAINDER should be -140 but what we need for this
				'build is only 10, not all 140.
				If quantityAvaliable <= 0 Then
					leftToBuild = quantityNeeded
				End If

				'Check to make sure that our array for our level key is big enough to support the next item before we add it.
				CurrentLevel += 1
				If CurrentLevel = MAX_RUNNING_NUMBERS Then
					MsgBox("Current MAX_RUNNING_NUMBERS has been reached. Array Index will be out of bounds. Edit code to allow a larger array.")
					Return
				End If
				RunningNumbers(CurrentLevel) += 1
				levelKey.Add(itemNumber & "|" & CurrentLevel & "." & RunningNumbers(CurrentLevel))

				'Add the basic information at the top of the section of the data table.
				'We use a 1 because this is a completed product only needs one. The 0 is used to show that it is the top level of items.
				master_DataTable.Rows.Add(itemNumber, vendor, partNumber, Nothing, Nothing, Nothing, Nothing, "1", quantityAvaliable, quantityNeeded, remainder_Value, Nothing, Nothing, Nothing, Nothing, Nothing, CurrentLevel & "." & RunningNumbers(CurrentLevel), "", 0, "Inventory Assembly", DEFAULT_REORDER_QT)

				'Check to see if we are looking for POST ASSEBLY parts.
				If searchForPCADItems = True And info(PRODUCT_INDEX).Contains(PREFIX_BAS) Then
					AddPostAssemblyParts(info(PRODUCT_INDEX), leftToBuild, isAnualReport)
				End If

				'leftToBuild should be 0 or <
				NextLevel(query, 1, leftToBuild, isAnualReport)

				'Add a blank row to seperate each item we want to build.
				'The 0 is used to show that it is the top level of items so it will always appear in the report to seperate the different builds.
				master_DataTable.Rows.Add()
				master_DataTable.Rows(master_DataTable.Rows.Count - 1)(HEADER_LEVEL) = 0
			Else
				'The product is not in our QB Items
				master_DataTable.Rows.Add(info(PRODUCT_INDEX), "???", "???", "???", "???", "???", "???", "1", NOT_IN_DATABASE, info(QUANTITY_INDEX), (info(QUANTITY_INDEX) * -1), Nothing, Nothing, Nothing, Nothing, Nothing, CurrentLevel & "." & RunningNumbers(CurrentLevel), "", 0, "?", DEFAULT_REORDER_QT)

				'Add a blank row to seperate each item we want to build.
				'The 0 is used to show that it is the top level of items so it will always appear in the report to seperate the different builds.
				master_DataTable.Rows.Add()
				master_DataTable.Rows(master_DataTable.Rows.Count - 1)(HEADER_LEVEL) = 0
			End If
		Next
	End Sub

	Private Sub NextLevel(ByRef query As String, ByRef level As Integer, ByRef quantityNeededToBuild As Integer, ByRef isAnualReport As Boolean)
		'Format to show level differences.
		Dim indent As String = ""
		If level <> 0 Then
			For number As Integer = 1 To level
				indent = indent & "    "
			Next
		End If

		'Get the list of items that are part of the BOM.
		Dim BOMdrs() As DataRow = QBBOMList.Select(query)

		'Go through each item in the BOM.
		For Each BOMdr As DataRow In BOMdrs
			'Get the Assembly information: SMA vs. BAS vs. FGS
			Dim thisAssembly As String = BOMdr(DB_HEADER_NAME_PREFIX)

			'Find the item inside our item list.
			Dim Itemdrs() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & BOMdr(DB_HEADER_ITEM_PREFIX) & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & BOMdr(DB_HEADER_ITEM_NUMBER) & "'")

			If Itemdrs.Length <> 0 Then
				'Get the type of item this is inside of the QB BOM
				'Assembly or Inv Part or Service
				Dim thisType As String = BOMdr(DB_HEADER_TYPE)

				'REM is here until we remove service items from the QB BOMs
				'Check to see if we are dealing with a Service Item. These items will always have a quantity of 0.
				If thisType = "Service" Then
					Continue For
				End If

				'Get the Quantity that is needed to build the remaining quantity.
				Dim quantityNeeded As Integer = BOMdr(DB_HEADER_QUANTITY) * quantityNeededToBuild

				'We use '0' to get the first row. There should only be one row returned.
				Dim quantityAvaliable As Integer = Itemdrs(0)(DB_HEADER_QUANTITY)
				Dim partNumber1 As String = Itemdrs(0)(DB_HEADER_MPN)
				Dim vendor1 As String = Itemdrs(0)(DB_HEADER_VENDOR)
				Dim partNumber2 As String = Itemdrs(0)(DB_HEADER_MPN2)
				Dim vendor2 As String = Itemdrs(0)(DB_HEADER_VENDOR2)
				Dim partNumber3 As String = Itemdrs(0)(DB_HEADER_MPN3)
				Dim vendor3 As String = Itemdrs(0)(DB_HEADER_VENDOR3)
				Dim cost_Value As Decimal = Itemdrs(0)(DB_HEADER_COST)
				Dim reorder_Quantity As Integer = Itemdrs(0)(DB_HEADER_REORDER_QTY)

				'Get the Lead Time from QB Inventory.
				Dim leadTime_Value As Decimal = Nothing
				Try
					leadTime_Value = CDec(Itemdrs(0)(DB_HEADER_LEAD_TIME))
				Catch ex As Exception

				End Try

				'Get min Order Qty
				'The minimum order quantity is 1 because that is the smallest number that we can order.
				Dim minOrderQty As Integer = 1
				Try
					minOrderQty = CInt(Itemdrs(0)(DB_HEADER_MIN_ORDER_QTY))
					If minOrderQty = 0 Then
						minOrderQty = 1
					End If
				Catch ex As Exception
					'Do nothing
				End Try

				'Calculate the available quantity
				'If this is an anual report then we need to set our avaliable to 0.
				If isAnualReport = True Then
					quantityAvaliable = 0
				Else
					'Try to see if we have already used this stock number previously so we can use the leftovers.
					Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER) & "%'")

					'We do not have to worry about NULLs here because this is inside the if statement for the part existing.
					If drs.Length <> 0 Then
						Dim newQuantity As Integer = drs(0).Item(HEADER_REMAINDER)
						For Each row As DataRow In drs
							'We want to get the one that has the lowest Remainder to use for our calculations.
							If CInt(row.Item(HEADER_REMAINDER)) < newQuantity Then
								newQuantity = CInt(row.Item(HEADER_REMAINDER))
							End If
						Next

						'Assign the lowest available quantity to our working variable.
						quantityAvaliable = newQuantity
					End If
				End If

				'Calculate the remainder.
				Dim remainder_Value As Integer = quantityAvaliable - quantityNeeded

				'check to see if we need to build or purcahse
				Dim leftToBuild As Integer = 0
				If remainder_Value < 0 Then
					'if we have a negitive number than we need to build or purchase
					leftToBuild = remainder_Value * -1
				Else
					leftToBuild = 0
				End If

				'Caclulate # to Buy
				'This number should be the same as the leftToBuild.
				'if it is 0 then we do not need any
				Dim numberToBuy = Nothing
				If 0 < leftToBuild Then
					numberToBuy = leftToBuild
				End If

				'Caclulate Total cost
				Dim totalCost = Nothing
				If numberToBuy <> Nothing Then
					totalCost = numberToBuy * cost_Value
				End If

				'Check to see if we are dealing with an assembly to adjust our LevelKey.
				'Also check to see if the levelKey array is still big enough.
				'This needs to be done before adding the assembly to the table because this new key needs to be associated with it.
				If BOMdr(DB_HEADER_TYPE).ToString.Contains("Assembly") Then
					CurrentLevel += 1
					If CurrentLevel = MAX_RUNNING_NUMBERS Then
						MsgBox("Current MAX_RUNNING_NUMBERS has been reached. Array Index will be out of bounds. Edit code to allow a larger array.")
						Return
					End If
					RunningNumbers(CurrentLevel) += 1
					levelKey.Add(BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER) & "|" & CurrentLevel & "." & RunningNumbers(CurrentLevel))

					'For assemblies, we cannot order them so the minOrderWty is set to Nothing for the data row entry.
					minOrderQty = Nothing
				Else
					'Check to see if we have added this part number to our list of unique parts. Add if not found.
					If uniqueParts.Contains(BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER)) = False Then
						uniqueParts.Add(BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER))
					End If
				End If

				'Add the information to the data table.
				master_DataTable.Rows.Add(indent & BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER), vendor1, partNumber1, vendor2, partNumber2, vendor3, partNumber3, BOMdr(DB_HEADER_QUANTITY), quantityAvaliable, quantityNeeded, remainder_Value, cost_Value, numberToBuy, totalCost, minOrderQty, leadTime_Value, CurrentLevel & "." & RunningNumbers(CurrentLevel), thisAssembly, level, thisType, reorder_Quantity)

				'Check to see if we are looking for POST ASSEBLY parts.
				If searchForPCADItems = True And BOMdr(DB_HEADER_ITEM_PREFIX) = PREFIX_BAS Then
					AddPostAssemblyParts(BOMdr(DB_HEADER_ITEM_NUMBER), leftToBuild, isAnualReport)
				End If

				'If we are dealing with an assembly, then we need to drill down futher and get the parts inside of it.
				If BOMdr(DB_HEADER_TYPE).ToString.Contains("Assembly") Then
					Dim productPrefix As String = ""
					Dim productName As String = ""
					If BOMdr(DB_HEADER_ITEM_PREFIX).ToString.Contains(PREFIX_FGS) = True Then
						productPrefix = BOMdr(DB_HEADER_ITEM_PREFIX)
						productName = BOMdr(DB_HEADER_ITEM_NUMBER)
					Else
						productPrefix = BOMdr(DB_HEADER_ITEM_NUMBER).Substring(0, BOMdr(DB_HEADER_ITEM_NUMBER).IndexOf("-"))
						productName = BOMdr(DB_HEADER_ITEM_NUMBER).Substring(BOMdr(DB_HEADER_ITEM_NUMBER).IndexOf("-") + 1)
					End If

					NextLevel("[" & DB_HEADER_NAME & "] = '" & productName & "' AND [" & DB_HEADER_NAME_PREFIX & "] = '" & productPrefix & "'", level + 1, leftToBuild, isAnualReport)
				End If
			Else
				'The stock number is in not the database.

				'Get the Quantity that is needed to build the remaining quantity.
				Dim quantityNeeded As Integer = BOMdr(DB_HEADER_QUANTITY) * quantityNeededToBuild

				'Calculate the remainder.
				'We use 0 because if the stock number is not in the database then we have 0 in stock.
				Dim remainder_Value As Integer = 0 - quantityNeeded

				'Try to see if we have already used this stock number previously.
				Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] Like '%" & BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER) & "%'")

				'We need to account for finding NULL values.
				If drs.Length > 1 Then
					'Put in the biggest number that we can start with incase the first row returns a NULL. There is no way we have 2 billions parts.
					Dim newQuantity As Integer = 2147483647
					For Each row As DataRow In drs
						If IsDBNull(row.Item(HEADER_REMAINDER)) = False Then
							'We want to get the one that has the lowest Remainder.
							If CInt(row.Item(HEADER_REMAINDER)) < newQuantity Then
								newQuantity = CInt(row.Item(HEADER_REMAINDER))
							End If
						End If
					Next
					'Use this newly found value and re-calculate the remainder_value.
					remainder_Value = newQuantity - quantityNeeded
				ElseIf drs.Length = 1 Then
					'We only have one duplicate row. Make sure it is not NULL.
					If IsDBNull(drs(0).Item(HEADER_REMAINDER)) = False Then
						remainder_Value = drs(0).Item(HEADER_REMAINDER) - quantityNeeded
					End If
				End If

				'Check to see if we have added this part number to our list of unique parts. Add if not found.
				If uniqueParts.Contains(BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER)) = False Then
					uniqueParts.Add(BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER))
				End If

				'Add the information to the data table.
				master_DataTable.Rows.Add(indent & BOMdr(DB_HEADER_ITEM_PREFIX) & ":" & BOMdr(DB_HEADER_ITEM_NUMBER), "???", "???", "???", "???", "???", "???", BOMdr(DB_HEADER_QUANTITY), NOT_IN_DATABASE, quantityNeeded, remainder_Value, Nothing, Nothing, Nothing, Nothing, Nothing, CurrentLevel & "." & RunningNumbers(CurrentLevel), thisAssembly, level, "?", DEFAULT_REORDER_QT)
			End If
		Next

		'We have drilled as far down as we can go so back up one level for our levelKey.
		CurrentLevel -= 1
	End Sub

	Private Sub AddPostAssemblyParts(ByRef board As String, ByRef quantityNeededToBuild As Integer, ByRef noInventory As Boolean)
		'Get rid of 'BAS-' so we can search for the board.
		Dim boardName As String = board.Substring(4)

		result_Cmd.commandText = "SELECT * FROM " & TABLE_PCADBOM & " WHERE UPPER([" & DB_HEADER_BOARD_NAME & "]) = UPPER('" & boardName & "') AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_POSTASSEMBLY & "'"

		Dim PCADList = New DataTable
		PCADList.Load(result_Cmd.ExecuteReader())

		Dim itemList As New List(Of String)

		'Go through all of the results. There should not be very many.
		For Each row As DataRow In PCADList.Rows
			Dim thisAssembly As String = "POST"
			Dim ItemNumber As String = row(DB_HEADER_ITEM_NUMBER)
			Dim ItemPrefix As String = row(DB_HEADER_ITEM_PREFIX)
			Dim wholeStockNumber As String = ItemPrefix & ":" & ItemNumber

			'Check to see if we have already added this item to the table. This prevents duplicates.
			If itemList.Contains(ItemNumber) = True Then
				Continue For
			Else
				itemList.Add(ItemNumber)
			End If

			'Get the count of the Item.
			Dim itemCount() As DataRow = PCADList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & row(DB_HEADER_ITEM_NUMBER) & "'")
			Dim originalQuantity As Integer = itemCount.Length

			'Calculate how many parts we need. This number should be 0 or <
			'0 means that we will not need to build/purcahse anything.
			Dim quantityNeeded As Integer = originalQuantity * quantityNeededToBuild

			'Get all of the information from QB Inventory.
			Dim Itemdrs() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & ItemPrefix & "' AND [" & DB_HEADER_ITEM_NUMBER & "] = '" & ItemNumber & "'")

			If Itemdrs.Length <> 0 Then
				'We use '0' to get the first row in the array.
				Dim quantityAvaliable As Integer = Itemdrs(0)(DB_HEADER_QUANTITY)
				Dim partNumber1 As String = Itemdrs(0)(DB_HEADER_MPN)
				Dim vendor1 As String = Itemdrs(0)(DB_HEADER_VENDOR)
				Dim partNumber2 As String = Itemdrs(0)(DB_HEADER_MPN2)
				Dim vendor2 As String = Itemdrs(0)(DB_HEADER_VENDOR2)
				Dim partNumber3 As String = Itemdrs(0)(DB_HEADER_MPN3)
				Dim vendor3 As String = Itemdrs(0)(DB_HEADER_VENDOR3)
				Dim cost_Value As Decimal = Itemdrs(0)(DB_HEADER_COST)
				Dim reorder_Quantity As Integer = Itemdrs(0)(DB_HEADER_REORDER_QTY)

				'Get the Lead Time from QB Inventory.
				Dim leadTime_Value As Decimal = Nothing
				Try
					leadTime_Value = CDec(Itemdrs(0)(DB_HEADER_LEAD_TIME))
				Catch ex As Exception

				End Try

				'Get min Order Qty
				'The minimum order quantity is 1 because that is the smallest number that we can order.
				Dim minOrderQty As Integer = 1
				Try
					minOrderQty = CInt(Itemdrs(0)(DB_HEADER_MIN_ORDER_QTY))
					If minOrderQty = 0 Then
						minOrderQty = 1
					End If
				Catch ex As Exception
					'Do nothing
				End Try

				'Calculate the Quantity Available
				If noInventory = True Then
					quantityAvaliable = 0
				End If

				'Try to see if we have already used this stock number previously.
				Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & wholeStockNumber & "%'")

				'We do not have to worry about NULLs here because this is inside the if statement for the part existing.
				If drs.Length <> 0 Then
					Dim newQuantity As Integer = CInt(drs(0).Item(HEADER_REMAINDER))
					For Each masterrow As DataRow In drs
						'We want to get the one that has the lowest Remainder so we can be accurate.
						If CInt(masterrow.Item(HEADER_REMAINDER)) < newQuantity Then
							newQuantity = CInt(masterrow.Item(HEADER_REMAINDER))
						End If
					Next
					quantityAvaliable = newQuantity
				End If

				'Calculate the remainder.
				Dim remainder_Value As Integer = quantityAvaliable - quantityNeeded

				'check to see if we need to build or purcahse
				Dim leftToBuild As Integer = 0
				If remainder_Value < 0 Then
					'if we have a negitive number than we need to build or purchase
					'Multiply by -1 because we cannot build a negitive number of items.
					leftToBuild = remainder_Value * -1
				End If

				'Caclulate # to Buy
				'This number should be the same as the leftToBuild.
				'if it is 0 then we do not need any
				Dim numberToBuy = Nothing
				If 0 < leftToBuild Then
					numberToBuy = leftToBuild
				End If

				'Caclulate Total cost
				Dim totalCost = Nothing
				If numberToBuy <> Nothing Then
					totalCost = numberToBuy * cost_Value
				End If

				'Check to see if we have added this part number to our list of unique parts. Add if not found.
				If uniqueParts.Contains(wholeStockNumber) = False Then
					uniqueParts.Add(wholeStockNumber)
				End If

				'Add all of the data to the master dataTable.
				master_DataTable.Rows.Add(ItemPrefix & ":" & ItemNumber, vendor1, partNumber1, vendor2, partNumber2, vendor3, partNumber3, originalQuantity, quantityAvaliable, quantityNeeded, remainder_Value, cost_Value, numberToBuy, totalCost, minOrderQty, leadTime_Value, CurrentLevel & "." & RunningNumbers(CurrentLevel), thisAssembly, 0, "Inv Part", reorder_Quantity)
			Else
				'The stock number is in not the database.

				'Calculate the remainder.
				'We use 0 because if the stock number is not in the database then we have 0 in stock.
				Dim remainder_Value As Integer = 0 - quantityNeeded

				'Try to see if we have already used this stock number previously.
				Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & ItemPrefix & ":" & ItemNumber & "%'")

				'We need to account for finding NULL values.
				If drs.Length > 1 Then
					Dim newQuantity As Integer = 0
					For Each masterrow As DataRow In drs
						If IsDBNull(masterrow.Item(HEADER_REMAINDER)) = False Then
							'We want to get the one that has the lowest Remainder.
							If CInt(masterrow.Item(HEADER_REMAINDER)) < newQuantity Then
								newQuantity = CInt(masterrow.Item(HEADER_REMAINDER))
							End If
						End If
					Next
					'Use this newly found value and re-calculate the remainder_value.
					remainder_Value = newQuantity - quantityNeeded
				ElseIf drs.Length = 1 Then
					'We only have one duplicate row. Make sure it is not NULL.
					If IsDBNull(drs(0).Item(HEADER_REMAINDER)) = False Then
						remainder_Value = drs(0).Item(HEADER_REMAINDER) - quantityNeeded
					End If
				End If

				'Check to see if we have added this part number to our list of unique parts. Add if not found.
				If uniqueParts.Contains(ItemPrefix & ":" & ItemNumber) = False Then
					uniqueParts.Add(ItemPrefix & ":" & ItemNumber)
				End If

				'Add the information to the data table.
				master_DataTable.Rows.Add(ItemPrefix & ":" & ItemNumber, "???", "???", "???", "???", "???", "???", originalQuantity, NOT_IN_DATABASE, quantityNeeded, remainder_Value, Nothing, Nothing, Nothing, Nothing, Nothing, CurrentLevel & "." & RunningNumbers(CurrentLevel), thisAssembly, 0, "?", DEFAULT_REORDER_QT)
			End If
		Next
	End Sub

	Private Sub FilterMasterTable(ByRef isOrderReport As Integer)
		'Create a Temp DataTable with the same structure as the results.
		'Used for our ORDER and ANUAL reports.
		Dim temp_DataTable = New DataTable
		temp_DataTable = result_DataTable.Clone()

		'Make sure the result table is cleared of all previous data.
		result_DataTable.Rows.Clear()
		Dim firstLine As Boolean = True
		Dim rownumber As Integer = 0
		Dim reportTotalPrice As Decimal = 0.0
		Dim productTotalPrice As Decimal = 0.0

		For Each dr As DataRow In master_DataTable.Rows
			If isOrderReport = REPORT Then
				'This is a REPORT report.
				'Check to see if we are dealing with a NULL Stock Number. If yes, then add the entire row and move onto the next row.
				'A blank row = seperator between builds.
				If IsDBNull(dr(DB_HEADER_ITEM_NUMBER)) Then
					'We have to add each value because the structure of the two tables are different.
					result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), "Total Cost", productTotalPrice, dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))
					result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), dr(HEADER_TO_BUY), dr(HEADER_TOTAL_COST), dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))
					firstLine = True
					reportTotalPrice += productTotalPrice
					productTotalPrice = 0.0
					rownumber += 1
					Continue For
				End If

				'This means that we have a part that is not in the database so just add it to the table to show the user.
				If dr(DB_HEADER_TYPE) = "?" Then
					result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), dr(HEADER_TO_BUY), dr(HEADER_TOTAL_COST), dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))
					rownumber += 1
					Continue For
				End If

				If dr(HEADER_LEVEL) <= stopLevel Then
					'This is within our stop level requirement.

					If ShowAll_CheckBox.Checked = True Then
						'This means that we want to show everything. Just need to calculate the cost.
						Dim totalcost As Decimal = Nothing

						If dr(DB_HEADER_TYPE).ToString.Contains("Assembly") Then
							totalcost = AssemblyPrice(dr(HEADER_LEVEL), rownumber)
						Else
							If IsDBNull(dr(HEADER_TOTAL_COST)) = False Then
								totalcost = dr(HEADER_TOTAL_COST)
								productTotalPrice += totalcost
							End If
						End If

						result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), dr(HEADER_TO_BUY), totalcost, dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))

					Else
						'This means that we only want the shorts.
						Dim totalcost As Decimal = Nothing

						If firstLine = True Then
							'This is the first line so we have to show it no matter what.

							If dr(DB_HEADER_TYPE).ToString.Contains("Assembly") Then
								totalcost = AssemblyPrice(dr(HEADER_LEVEL), rownumber)
							Else
								If IsDBNull(dr(HEADER_TOTAL_COST)) = False Then
									totalcost = dr(HEADER_TOTAL_COST)
									productTotalPrice += totalcost
								End If
							End If

							result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), dr(HEADER_TO_BUY), totalcost, dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))

							firstLine = False

						ElseIf dr(HEADER_REMAINDER) < 0 And dr(HEADER_QTY_NEEDED) > 0 Then
							If dr(DB_HEADER_TYPE).ToString.Contains("Assembly") Then
								totalcost = AssemblyPrice(dr(HEADER_LEVEL), rownumber)
							Else
								If IsDBNull(dr(HEADER_TOTAL_COST)) = False Then
									totalcost = dr(HEADER_TOTAL_COST)
									productTotalPrice += totalcost
								End If
							End If

							result_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), dr(HEADER_QTY_ORIG), dr(HEADER_QTY_AVAIL), dr(HEADER_QTY_NEEDED), dr(HEADER_REMAINDER), dr(DB_HEADER_COST), dr(HEADER_TO_BUY), totalcost, dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), dr(HEADER_LEVEL_KEY), dr(HEADER_ASSEMBLY))

						End If
					End If
				End If
			Else
				'This is an ORDER or ANUAL report.
				If IsDBNull(dr(DB_HEADER_ITEM_NUMBER)) Then
					rownumber += 1
					Continue For
				End If

				'Check to see if we are looking at an assembly. We do not want to add these in this type of report.
				If dr(DB_HEADER_TYPE) = "Inventory Assembly" Then
					rownumber += 1
					Continue For
				End If

				'Check to see if we have not added this part number to the new temp table.
				If temp_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dr(DB_HEADER_ITEM_NUMBER).trim & "'").Length = 0 Then

					'Grab every row that has a stock number that is similar.
					Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dr(DB_HEADER_ITEM_NUMBER).trim & "%'")

					'Add up the quantity of each row to get how many we need.
					Dim needed As Integer = 0
					Dim original As Integer = 0
					Dim levelkey As String = ""

					'Add up the original numbers that we need and the total number needed to build.
					'Add up the levelkey that this part is found on.
					For Each row As DataRow In drs
						needed += row(HEADER_QTY_NEEDED)
						original += row(HEADER_QTY_ORIG)
						levelkey = levelkey & row(HEADER_LEVEL_KEY) & ", "
					Next

					'Get rid of the last comma and space in the string
					levelkey = levelkey.Substring(0, levelkey.Length - 2)

					Dim remaining As Integer = 0

					'Check to see if we have a quantity for this item to calculate the remainder.
					If dr(HEADER_QTY_AVAIL) = NOT_IN_DATABASE Then
						remaining = 0 - needed
					Else
						remaining = dr(HEADER_QTY_AVAIL) - needed
					End If

					Dim numToBuy As Integer = Nothing
					If remaining < 0 Then
						numToBuy = remaining * -1
					End If

					'Add the new data to the temp table.
					temp_DataTable.Rows.Add(dr(DB_HEADER_ITEM_NUMBER).Trim, dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_VENDOR2), dr(DB_HEADER_MPN2), dr(DB_HEADER_VENDOR3), dr(DB_HEADER_MPN3), original, dr(HEADER_QTY_AVAIL), needed, remaining, dr(DB_HEADER_COST), numToBuy, dr(HEADER_TOTAL_COST), dr(DB_HEADER_MIN_ORDER_QTY), dr(DB_HEADER_LEAD_TIME), levelkey, dr(HEADER_ASSEMBLY))
				End If
			End If
			rownumber += 1
		Next

		If isOrderReport <> REPORT Then
			'We are dealing with an ORDER report. that means that we need the total price to complete this build order.
			masterCost = 0.0
			For Each dr2 As DataRow In temp_DataTable.Rows
				'Check for anything that is a BAS or SMA or FGS. We do not want these in this repot.
				If dr2(DB_HEADER_ITEM_NUMBER).contains(PREFIX_BAS & ":") Or dr2(DB_HEADER_ITEM_NUMBER).contains(PREFIX_BIS & ":") Or dr2(DB_HEADER_ITEM_NUMBER).contains(PREFIX_DAS & ":") Or dr2(DB_HEADER_ITEM_NUMBER).contains(PREFIX_SMA & ":") Or dr2(DB_HEADER_ITEM_NUMBER).contains(PREFIX_FGS & ":") Then
					Continue For
				End If

				'Get the type of item it is that we are working with.
				Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & dr2(DB_HEADER_ITEM_NUMBER) & "%'")
				Dim thisType As String = drs(0).Item(DB_HEADER_TYPE)

				'This means that we have a part that is not in the database so just add it to the table to show the user.
				If thisType = "?" Then
					result_DataTable.Rows.Add(dr2(DB_HEADER_ITEM_NUMBER), dr2(DB_HEADER_VENDOR), dr2(DB_HEADER_MPN), dr2(DB_HEADER_VENDOR2), dr2(DB_HEADER_MPN2), dr2(DB_HEADER_VENDOR3), dr2(DB_HEADER_MPN3), dr2(HEADER_QTY_ORIG), dr2(HEADER_QTY_AVAIL), dr2(HEADER_QTY_NEEDED), dr2(HEADER_REMAINDER), dr2(DB_HEADER_COST), dr2(HEADER_TO_BUY), dr2(HEADER_TOTAL_COST), dr2(DB_HEADER_MIN_ORDER_QTY), dr2(DB_HEADER_LEAD_TIME), dr2(HEADER_LEVEL_KEY), dr2(HEADER_ASSEMBLY))
					Continue For
				End If

				If ShowAll_CheckBox.Checked = True Then
					'Add warnings to this report.
					Dim totalCost As Decimal = Nothing

					If dr2(HEADER_REMAINDER) < 0 Then
						'Since we have less than 0 parts, we need to order which means we need a cost.
						Try
							totalCost = dr2(DB_HEADER_COST) * dr2(HEADER_TO_BUY)
							masterCost += totalCost
						Catch ex As Exception
							'The only time we should get here involves an item not being in the database and us being short that part.
							'Do nothing.
						End Try
					End If
					result_DataTable.Rows.Add(dr2(DB_HEADER_ITEM_NUMBER), dr2(DB_HEADER_VENDOR), dr2(DB_HEADER_MPN), dr2(DB_HEADER_VENDOR2), dr2(DB_HEADER_MPN2), dr2(DB_HEADER_VENDOR3), dr2(DB_HEADER_MPN3), dr2(HEADER_QTY_ORIG), dr2(HEADER_QTY_AVAIL), dr2(HEADER_QTY_NEEDED), dr2(HEADER_REMAINDER), dr2(DB_HEADER_COST), dr2(HEADER_TO_BUY), totalCost, dr2(DB_HEADER_MIN_ORDER_QTY), dr2(DB_HEADER_LEAD_TIME), dr2(HEADER_LEVEL_KEY), dr2(HEADER_ASSEMBLY))

				ElseIf dr2(HEADER_REMAINDER) < 0 Then
					'Since we have less than 0 parts, we need to order which means we need a cost.
					Dim totalCost As Decimal = 0.0
					Try
						totalCost = dr2(DB_HEADER_COST) * dr2(HEADER_TO_BUY)
						masterCost += totalCost
					Catch ex As Exception
						'The only time we should get here involves an item not being in the database and us being short that part.
						'Do nothing.
					End Try

					result_DataTable.Rows.Add(dr2(DB_HEADER_ITEM_NUMBER), dr2(DB_HEADER_VENDOR), dr2(DB_HEADER_MPN), dr2(DB_HEADER_VENDOR2), dr2(DB_HEADER_MPN2), dr2(DB_HEADER_VENDOR3), dr2(DB_HEADER_MPN3), dr2(HEADER_QTY_ORIG), dr2(HEADER_QTY_AVAIL), dr2(HEADER_QTY_NEEDED), dr2(HEADER_REMAINDER), dr2(DB_HEADER_COST), dr2(HEADER_TO_BUY), totalCost, dr2(DB_HEADER_MIN_ORDER_QTY), dr2(DB_HEADER_LEAD_TIME), dr2(HEADER_LEVEL_KEY), dr2(HEADER_ASSEMBLY))
				End If
			Next
		Else
			result_DataTable.Rows.Add(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "Grand Cost", reportTotalPrice, Nothing, Nothing, Nothing, Nothing)
		End If
	End Sub

	Private Function AssemblyPrice(ByRef rowlevel As Integer, ByRef rowNumber As Integer) As Decimal
		Dim total As Decimal = 0.0
		For index As Integer = rowNumber + 1 To master_DataTable.Rows.Count - 1
			'We want to get the total price to build this assembly.
			If master_DataTable.Rows(index)(HEADER_LEVEL) > rowlevel Then
				Try
					total += master_DataTable.Rows(index)(HEADER_TOTAL_COST)
				Catch ex As Exception

				End Try
			Else
				'Once we hit a row that is higher than the rowlevel, exit and continue.
				Exit For
			End If
		Next
		Return total
	End Function

	Private Sub Excel_Button_Click() Handles Excel_Button.Click
		Dim report As New GenerateReport
		report.GenerateBuildProductsReport(BuildProducts_ListBox, result_ds, UseInventory_CheckBox.Checked, ShowAll_CheckBox.Checked, uniqueParts.Count, levelKey)
	End Sub

	Private Sub Close_Button_Click() Handles Close_Button.Click
		Close()
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
			GetProducts()
		End If
	End Sub

	Private Sub GetProducts()
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

	Private Sub FormatDGV()
		Dim isFirstLineStocked As Boolean = True
		For index = 0 To Results_DGV.Rows.Count - 1
			'Check to see that the item number is not NULL
			If IsDBNull(Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value) = False Then

				Dim drs() As DataRow = master_DataTable.Select("[" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value & "%'")
				Dim thisType As String = drs(0).Item(DB_HEADER_TYPE)
				Dim roq As Integer = drs(0).Item(DB_HEADER_REORDER_QTY)

				'Check the remainder value of the item. If we are less then 0, then we need to mark it.
				If Results_DGV.Rows(index).Cells(HEADER_REMAINDER).Value < 0 Then
					If thisType.Contains("Assembly") Then
						Results_DGV.Rows(index).DefaultCellStyle.BackColor = ASSEMBLY_COLOR
					Else
						Results_DGV.Rows(index).DefaultCellStyle.BackColor = OUT_COLOR
					End If
					isFirstLineStocked = False

				ElseIf ShowAll_CheckBox.Checked = True Then
					If Results_DGV.Rows(index).Cells(HEADER_REMAINDER).Value <= roq Then
						'we are getting low on this part and need to consider ordering it.
						Results_DGV.Rows(index).DefaultCellStyle.BackColor = ORDER_COLOR
					End If

					If isFirstLineStocked = True Then
						For Each build As String In BuildProducts_ListBox.Items
							Dim info() As String = build.ToString.Split(VLINE_DILIMITER)
							If Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Value.trim.contains(info(PRODUCT_INDEX)) Then
								Results_DGV.Rows(index).DefaultCellStyle.BackColor = GOOD_COLOR
							End If
						Next
						isFirstLineStocked = False
					End If
				End If

				If thisType.Contains("Assembly") Then
					Results_DGV.Rows(index).Cells(DB_HEADER_ITEM_NUMBER).Style.Font = New Font(Results_DGV.DefaultCellStyle.Font, FontStyle.Bold)
				End If

				If Results_DGV.Rows(index).Cells(HEADER_QTY_AVAIL).Value = NOT_IN_DATABASE Then
					Results_DGV.Rows(index).DefaultCellStyle.BackColor = DATABASE_COLOR
				End If
			Else
				isFirstLineStocked = True
			End If
		Next
	End Sub

	Private Sub Results_DGV_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Results_DGV.MouseDown
		'Handles a right click on our grid to bring up our context menu.
		If e.Button = Windows.Forms.MouseButtons.Right Then
			Dim ht As DataGridView.HitTestInfo
			ht = Results_DGV.HitTest(e.X, e.Y)
			If ht.Type = DataGridViewHitTestType.Cell Then
				Results_DGV.ContextMenuStrip = mnuCell
			Else
				Results_DGV.ContextMenuStrip = Nothing
			End If
		End If
	End Sub

	Private Sub cMenu_Click(ByVal sender As Object, ByVal e As EventArgs)
		'Using the sender.text, find out which menu item was selected.
		Select Case sender.text
			Case "Print"
				'Add all of the selected row's stock numbers to a list to pass into the Printing Form.
				Dim stockList As New List(Of String)
				For Each row In Results_DGV.SelectedCells
					stockList.Add(Results_DGV.Rows(row.rowIndex).Cells(0).Value.ToString.Trim)
				Next

				Dim printing As New Printing(stockList)
				printing.Show()
		End Select
	End Sub

	Private Sub CriticalBuild_Button_Click() Handles CriticalBuild_Button.Click
		If MenuMain.OpenForm(CriticalBuildPath) = False Then
			Return
		Else
			'Send the list and quantities over to build products.
			CriticalBuildPath.transferBuildProducts(BuildProducts_ListBox)
		End If
	End Sub

	Public Sub transferCriticalBuild(ByRef list As ListBox)
		BuildProducts_ListBox.Items.Clear()
		Excel_Button.Enabled = False

		For Each item In list.Items
			'Update list with what we recived.
			BuildProducts_ListBox.Items.Add(item)
		Next
	End Sub

End Class