'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: MasterControl.vb
'
' Description: Lou's special Process Release utilities
'   Tab 1 QuickBooks: Items: List of QB items with search ability
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient

Public Class MasterControl

#Region "Tab 1: QB Items Variables"
	Dim QBitems_da As New SqlDataAdapter
	Dim QBitems_ds As New DataSet
	Dim QBitems_myCmd = New SqlCommand("", myConn)

	Dim QBitems_scrollValue As Integer
	Dim QBitems_Command As String = "SELECT [" & DB_HEADER_ITEM_PREFIX & "]" &
										", [" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "] AS '" & DB_HEADER_ITEM_NUMBER & "'" &
										", [" & DB_HEADER_TYPE & "]" &
										", [" & DB_HEADER_DESCRIPTION & "]" &
										", [" & DB_HEADER_VENDOR & "]" &
										", [" & DB_HEADER_MPN & "]" &
										", [" & DB_HEADER_QUANTITY & "]" &
										", [" & DB_HEADER_COST & "]" &
										", [" & DB_HEADER_LEAD_TIME & "]" &
										", [" & DB_HEADER_MIN_ORDER_QTY & "]" &
										", [" & DB_HEADER_REORDER_QTY & "]" &
										", [" & DB_HEADER_VENDOR2 & "]" &
										", [" & DB_HEADER_MPN2 & "]" &
										", [" & DB_HEADER_VENDOR3 & "]" &
										", [" & DB_HEADER_MPN3 & "]" &
										" FROM " & TABLE_QB_ITEMS
	Dim QBitems_countCommand As String = "SELECT COUNT(*) FROM " & TABLE_QB_ITEMS
	Dim QBitems_entriesToShow As Integer = 250
	Dim QBitems_numberOfRecords As Integer
	Dim QBitems_sort As String = ""
	Dim QBitems_searchCommand As String = ""
	Dim QBitems_searchCountCommand As String = ""
	Dim QBitems_Freeze As Integer = 1

	'Set up the Context menu
	Dim mnuCell As New ContextMenuStrip
#End Region

	Dim myCmd As New SqlCommand("", myConn)

	Private Sub MasterControl_Load() Handles MyBase.Load
		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		'Inherited Sub that will center the opening form to screen.
		CenterToParent()

		Dim result As String = ""

		Try
			'Tab Page 1 - QuickBooks
			sqlapi.GetNumberOfRecords(QBitems_myCmd, QBitems_countCommand, QBitems_numberOfRecords, result)
			L_QB_Results.Text = "Number of results: " & QBitems_numberOfRecords

			QBitems_myCmd.CommandText = QBitems_Command & " ORDER BY [" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]"
			QBitems_da = New SqlDataAdapter(QBitems_myCmd)
			QBitems_ds = New DataSet()

			sqlapi.RetriveData(QBitems_Freeze, QBitems_da, QBitems_ds, DGV_QB_Items, QBitems_scrollValue, QBitems_entriesToShow, QBitems_numberOfRecords,
							   B_QB_Next, B_QB_Last, B_QB_First, B_QB_Previous)

			'Get Drop Down Items.
			GetColumnDropDownItems(CB_QB_Sort, QBitems_ds)
			GetColumnDropDownItems(CB_QB_Search, QBitems_ds)
			GetColumnDropDownItems(CB_QB_Search2, QBitems_ds)

			CB_QB_Operand1.SelectedIndex = 0
			CB_QB_Operand2.SelectedIndex = 0

			'We need to add each menu strip to the menu and add its own handdler location
			With mnuCell
				Dim newMenu As New ToolStripMenuItem("Print", Nothing, AddressOf cMenu_Click)
				.Items.Add(newMenu)
			End With

			KeyPreview = True
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub

	Private Sub B_CreateExcel_Click() Handles B_CreateExcel.Click
		Dim report As New GenerateReport()

		'Depending on which tab is open will determine which report to create.
		Select Case TabControl1.SelectedTab.Name
			Case "TP_QB_items"
				Dim Temp_ds As New DataSet
				QBitems_da.Fill(Temp_ds, "TABLE")
				report.GenerateQB_itemslistReport(Temp_ds)
			Case Else
				MsgBox("Generating a report on this tab has not been coded yet.")
		End Select
	End Sub

	Private Sub B_Close_Click() Handles B_Close.Click
		Close()
	End Sub

	Private Sub GetColumnDropDownItems(ByRef cb As ComboBox, ByRef ds As DataSet)
		For Each dc As DataColumn In ds.Tables(0).Columns
			cb.Items.Add(dc.ColumnName)
		Next

		If cb.Items.Count <> 0 Then
			cb.SelectedIndex = 0
		End If

		cb.DropDownHeight = 200
	End Sub

#Region "Tab 1: QB Items"
	Private Sub B_QB_First_Click() Handles B_QB_First.Click
		sqlapi.FirstPage(QBitems_scrollValue, QBitems_ds, QBitems_da, QBitems_entriesToShow)
		B_QB_First.Enabled = False
		B_QB_Previous.Enabled = False
		B_QB_Next.Enabled = True
		B_QB_Last.Enabled = True
	End Sub

	Private Sub B_QB_Previous_Click() Handles B_QB_Previous.Click
		sqlapi.PreviousPage(QBitems_scrollValue, QBitems_entriesToShow, QBitems_ds, QBitems_da, B_QB_First, B_QB_Previous)
		B_QB_Next.Enabled = True
		B_QB_Last.Enabled = True
	End Sub

	Private Sub B_QB_Next_Click() Handles B_QB_Next.Click
		sqlapi.NextPage(QBitems_scrollValue, QBitems_entriesToShow, QBitems_numberOfRecords, QBitems_ds, QBitems_da, B_QB_Next, B_QB_Last)
		B_QB_First.Enabled = True
		B_QB_Previous.Enabled = True
	End Sub

	Private Sub B_QB_Last_Click() Handles B_QB_Last.Click
		sqlapi.LastPage(QBitems_scrollValue, QBitems_entriesToShow, QBitems_numberOfRecords, QBitems_ds, QBitems_da)
		B_QB_First.Enabled = True
		B_QB_Previous.Enabled = True
		B_QB_Next.Enabled = False
		B_QB_Last.Enabled = False
	End Sub

	Private Sub B_QB_ListAll_Click() Handles B_QB_ListAll.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				sqlapi.ListAll(1, QBitems_searchCommand, QBitems_searchCountCommand, QBitems_myCmd, QBitems_countCommand, QBitems_numberOfRecords, L_QB_Results,
							   QBitems_Command & " ORDER BY [" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]", QBitems_ds, QBitems_da, DGV_QB_Items, QBitems_scrollValue, QBitems_entriesToShow, B_QB_Next,
							   B_QB_Last, B_QB_First, B_QB_Previous)
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			sqlapi.ListAll(1, QBitems_searchCommand, QBitems_searchCountCommand, QBitems_myCmd, QBitems_countCommand, QBitems_numberOfRecords, L_QB_Results,
						   QBitems_Command & " ORDER BY [" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]", QBitems_ds, QBitems_da, DGV_QB_Items, QBitems_scrollValue, QBitems_entriesToShow, B_QB_Next,
						   B_QB_Last, B_QB_First, B_QB_Previous)
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub B_QB_Search_Click() Handles B_QB_Search.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				sqlapi.Search(QBitems_Freeze, TB_QB_Search, CB_QB_Operand1, QBitems_searchCommand, QBitems_Command, QBitems_searchCountCommand, QBitems_countCommand, TB_QB_Search2, CB_QB_Operand2,
							  CB_QB_Search, CB_QB_Search2, QBitems_myCmd, QBitems_ds, QBitems_da, DGV_QB_Items, QBitems_numberOfRecords, L_QB_Results, QBitems_scrollValue,
							  QBitems_entriesToShow, B_QB_Next, B_QB_Last, B_QB_First, B_QB_Previous, "[" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]")
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			sqlapi.Search(QBitems_Freeze, TB_QB_Search, CB_QB_Operand1, QBitems_searchCommand, QBitems_Command, QBitems_searchCountCommand, QBitems_countCommand, TB_QB_Search2, CB_QB_Operand2,
						  CB_QB_Search, CB_QB_Search2, QBitems_myCmd, QBitems_ds, QBitems_da, DGV_QB_Items, QBitems_numberOfRecords, L_QB_Results, QBitems_scrollValue,
						  QBitems_entriesToShow, B_QB_Next, B_QB_Last, B_QB_First, B_QB_Previous, "[" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]")
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub B_QB_Sort_Click() Handles B_QB_Sort.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				sqlapi.Sort(QBitems_Freeze, QBitems_searchCommand, QBitems_Command, CB_QB_Sort, RB_QB_AscendingSort, QBitems_myCmd, QBitems_ds, QBitems_da, DGV_QB_Items,
							QBitems_scrollValue, QBitems_entriesToShow, QBitems_numberOfRecords, B_QB_Next, B_QB_Last, B_QB_First, B_QB_Previous)
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			sqlapi.Sort(QBitems_Freeze, QBitems_searchCommand, QBitems_Command, CB_QB_Sort, RB_QB_AscendingSort, QBitems_myCmd, QBitems_ds, QBitems_da, DGV_QB_Items,
						QBitems_scrollValue, QBitems_entriesToShow, QBitems_numberOfRecords, B_QB_Next, B_QB_Last, B_QB_First, B_QB_Previous)
		End If
		Cursor = Cursors.Default

	End Sub

	Private Sub TB_QB_Search_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TB_QB_Search.KeyDown
		If e.KeyCode.Equals(Keys.Enter) Then
			Call B_QB_Search_Click()
		End If
	End Sub

	Private Sub TB_QB_Search2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TB_QB_Search2.KeyDown
		If e.KeyCode.Equals(Keys.Enter) Then
			Call B_QB_Search_Click()
		End If
	End Sub

	Private Sub CB_QB_Display_SelectedValueChanged() Handles CB_QB_Display.SelectedValueChanged
		QBitems_entriesToShow = CInt(CB_QB_Display.Text)
	End Sub

	Private Sub CB_QB_Search_Click() Handles CB_QB_Search.Click
		CB_QB_Search.SelectedIndex = 0
	End Sub

	Private Sub CB_QB_Search_DropDownClosed() Handles CB_QB_Search.DropDownClosed
		Dim newcmd As New SqlCommand("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" & TABLE_QB_ITEMS & "' AND COLUMN_NAME = '" & CB_QB_Search.Text & "'", myConn)

		Dim type As String = newcmd.ExecuteScalar

		If type = "decimal" Or type = "int" Then
			CB_QB_Operand1.SelectedIndex = 3
		Else
			CB_QB_Operand1.SelectedIndex = 0
		End If
	End Sub

	Private Sub CB_QB_Search2_DropDownClosed() Handles CB_QB_Search2.DropDownClosed
		Dim newcmd As New SqlCommand("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" & TABLE_QB_ITEMS & "' AND COLUMN_NAME = '" & CB_QB_Search2.Text & "'", myConn)

		Dim type As String = newcmd.ExecuteScalar

		If type = "decimal" Or type = "int" Then
			CB_QB_Operand2.SelectedIndex = 3
		Else
			CB_QB_Operand2.SelectedIndex = 0
		End If
	End Sub

	Private Sub CB_QB_Sort_Click() Handles CB_QB_Sort.Click
		CB_QB_Sort.SelectedIndex = 0
	End Sub

	Private Sub Results_DGV_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles DGV_QB_Items.MouseDown
		'Handles a right click on our grid to bring up our context menu.
		If e.Button = Windows.Forms.MouseButtons.Right Then
			Dim ht As DataGridView.HitTestInfo
			ht = DGV_QB_Items.HitTest(e.X, e.Y)
			If ht.Type = DataGridViewHitTestType.Cell Then
				DGV_QB_Items.ContextMenuStrip = mnuCell
			Else
				DGV_QB_Items.ContextMenuStrip = Nothing
			End If
		End If
	End Sub

	Private Sub cMenu_Click(ByVal sender As Object, ByVal e As EventArgs)
		'Using the sender.text, find out which menu item was selected.
		Select Case sender.text
			Case "Print"
				If My.Settings.LabelPath.Length = 0 Then
					MsgBox("Label Directory not set up yet. Taking you to Settings.")
					Dim DoSettings As New Settings()
					DoSettings.ShowDialog()
					Return
				End If
				'Add all of the selected row's stock numbers to a list to pass into the Printing Form.
				Dim stockList As New List(Of String)
				For Each row In DGV_QB_Items.SelectedCells
					stockList.Add(DGV_QB_Items.Rows(row.rowIndex).Cells(1).Value.ToString.Trim)
				Next

				Dim printing As New Printing(stockList)
				printing.Show()
		End Select
	End Sub

	Private Sub DGV_QB_Items_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles DGV_QB_Items.RowPostPaint
		'Go through each row of the DGV and add the row number to the row header.
		Using b As SolidBrush = New SolidBrush(DGV_QB_Items.RowHeadersDefaultCellStyle.ForeColor)
			e.Graphics.DrawString(e.RowIndex + 1 + QBitems_scrollValue, DGV_QB_Items.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
		End Using
	End Sub
#End Region

End Class