'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: CostofProduct.vb
'
' Description: Adds up the total cost of each item times its quantity to display the total number of components and how much it will cost
'		to build the product selected from the source level. Items that are not found will be flagged for the user to see.
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient

Public Class CostofProduct

	Dim DataTable_Cost As DataTable
	Dim QBItemList As DataTable
	Dim QBBOMList As DataTable

	Dim ds As New DataSet

	Dim totalItemCost As Decimal = 0.0
	Dim totalQuantity As Integer = 0
	Dim missingComponent As Boolean = False

	Dim formLoaded As Boolean = False

	Private Sub CostofProduct_Load() Handles MyBase.Load
		'Populate drop-down.
		GetPrefixDropDownItems(CB_SourceLevel)
		CB_SourceLevel.SelectedIndex = CB_SourceLevel.FindString(PREFIX_FGS)

		KeyPreview = True
		formLoaded = True
		CB_SourceLevel_SelectedValueChanged()
	End Sub

	Private Sub Cost_Button_Click() Handles Cost_Button.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					GenerateReport()
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				GenerateReport()
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub GenerateReport()
		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		QBItemList = New DataTable
		Dim myCmd As New SqlCommand("SELECT * FROM " & TABLE_QB_ITEMS, myConn)
		QBItemList.Load(myCmd.ExecuteReader())

		QBBOMList = New DataTable
		myCmd.CommandText = "SELECT * FROM " & TABLE_QBBOM
		QBBOMList.Load(myCmd.ExecuteReader())

		SetupTable()
		totalItemCost = 0.0
		totalQuantity = 0
		missingComponent = False

		Dim drs() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_PREFIX & "] = '" & CB_SourceLevel.Text & "' AND [" & DB_HEADER_ITEM_NUMBER & "] LIKE '%" & CB_Products.Text & "'")

		If drs.Length <> 0 Then
			DataTable_Cost.Rows.Add(drs(0)(DB_HEADER_ITEM_PREFIX) & ":" & drs(0)(DB_HEADER_ITEM_NUMBER), Nothing, Nothing, Nothing)
			ListComponentsAndPrice("[" & DB_HEADER_NAME_PREFIX & "] = '" & CB_SourceLevel.Text & "' AND [" & DB_HEADER_NAME & "] = '" & CB_Products.Text & "'", 1)
		Else
			DataTable_Cost.Rows.Add("The Selected Product is not found as an item in the QB Items List.")
		End If

		ds = New DataSet
		ds.Tables.Add(DataTable_Cost)

		Cost_DGV.DataSource = Nothing
		Cost_DGV.DataSource = ds.Tables(0)
		Cost_DGV.AutoResizeColumns(DataGridViewAutoSizeColumnMode.AllCells)

		'Check to see if our flag is raised. If so, show the text box.
		If missingComponent = True Then
			L_Missing.Visible = True
		Else
			L_Missing.Visible = False
		End If

		L_Quantity.Text = "Total Items: " & totalQuantity
		L_TotalCost.Text = "Item Cost: $" & totalItemCost
		Excel_Button.Enabled = True
	End Sub

	Private Sub SetupTable()
		DataTable_Cost = New DataTable
		DataTable_Cost.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_Cost.Columns.Add(DB_HEADER_COST, GetType(String))
		DataTable_Cost.Columns.Add(DB_HEADER_QUANTITY, GetType(String))
		DataTable_Cost.Columns.Add(HEADER_TOTAL_COST, GetType(String))
	End Sub

	Private Sub ListComponentsAndPrice(ByRef query As String, ByRef level As Integer)
		Dim indent As String = ""

		'Format to show level differences.
		If level <> 0 Then
			For number As Integer = 1 To level
				indent = indent & "    "
			Next
		End If

		Dim drs() As DataRow = QBBOMList.Select(query, "[" & DB_HEADER_ITEM_NUMBER & "] ASC")

		For Each dr As DataRow In drs
			If dr(DB_HEADER_TYPE).contains("Assembly") = True Then
				DataTable_Cost.Rows.Add(indent & dr(DB_HEADER_ITEM_PREFIX) & ":" & dr(DB_HEADER_ITEM_NUMBER), Nothing, Nothing, Nothing)

				Dim assemblyName As String = dr(DB_HEADER_ITEM_NUMBER).ToString.Substring(dr(DB_HEADER_ITEM_NUMBER).ToString.IndexOf("-") + 1)

				ListComponentsAndPrice("[" & DB_HEADER_NAME_PREFIX & "] = '" & dr(DB_HEADER_ITEM_PREFIX) & "' AND [" & DB_HEADER_NAME & "] = '" & assemblyName & "'", level + 1)
			Else
				Dim itemdrs() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dr(DB_HEADER_ITEM_NUMBER) & "'")

				If itemdrs.Length <> 0 Then
					Dim totalCost As Decimal = 0.0

					totalCost = itemdrs(0)(DB_HEADER_COST) * dr(DB_HEADER_QUANTITY)

					DataTable_Cost.Rows.Add(indent & itemdrs(0)(DB_HEADER_ITEM_PREFIX) & ":" & itemdrs(0)(DB_HEADER_ITEM_NUMBER), itemdrs(0)(DB_HEADER_COST), dr(DB_HEADER_QUANTITY), totalCost)
					totalQuantity += dr(DB_HEADER_QUANTITY)
					totalItemCost += totalCost
				Else
					Dim totalCost As Decimal = dr(DB_HEADER_COST) * dr(DB_HEADER_QUANTITY)
					DataTable_Cost.Rows.Add(indent & "**" & dr(DB_HEADER_ITEM_PREFIX) & ":" & dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_COST), dr(DB_HEADER_QUANTITY), totalCost)
					missingComponent = True
				End If
			End If
		Next
	End Sub

	Private Sub Excel_Button_Click() Handles Excel_Button.Click
		Dim report As New GenerateReport()
		report.GenerateCostReport(CB_Products.Text, ds, totalQuantity, totalItemCost)
	End Sub

	Private Sub Close_Button_Click() Handles Close_Button.Click
		Close()
	End Sub

	Private Sub CB_Products_SelectedValueChanged() Handles CB_Products.SelectedValueChanged
		Excel_Button.Enabled = False
	End Sub

	Private Sub CB_SourceLevel_SelectedValueChanged() Handles CB_SourceLevel.SelectedValueChanged
		If formLoaded = True Then
			Excel_Button.Enabled = False
			GetProducts()
		End If
	End Sub

	Private Sub GetProducts()
		'Populate Board drop-down.
		CB_Products.Items.Clear()

		Dim ProductNames As New DataTable()
		Dim myCmd As New SqlCommand("SELECT Distinct([" & DB_HEADER_NAME & "]) FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME_PREFIX & "] = '" & CB_SourceLevel.Text & "' ORDER BY [" & DB_HEADER_NAME & "]", myConn)

		ProductNames.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In ProductNames.Rows
			CB_Products.Items.Add(dr(DB_HEADER_NAME))
		Next

		If CB_Products.Items.Count <> 0 Then
			CB_Products.SelectedIndex = 0
		End If

		CB_Products.DropDownHeight = 200
	End Sub

	Private Sub GetPrefixDropDownItems(ByRef box As ComboBox)
		Dim ProductPrefixes As New DataTable()

		Dim myCmd As New SqlCommand("SELECT Distinct([" & DB_HEADER_NAME_PREFIX & "]) FROM " & TABLE_QBBOM, myConn)

		ProductPrefixes.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In ProductPrefixes.Rows
			box.Items.Add(dr(DB_HEADER_NAME_PREFIX))
		Next
	End Sub

	Private Sub Cost_DGV_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles Cost_DGV.RowPostPaint
		Using b As SolidBrush = New SolidBrush(Cost_DGV.RowHeadersDefaultCellStyle.ForeColor)
			e.Graphics.DrawString(e.RowIndex + 1, Cost_DGV.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
		End Using
	End Sub

End Class