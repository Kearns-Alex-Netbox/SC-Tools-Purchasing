Imports System.Data.SqlClient
'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: BOMReport.vb
'
' Description: Generate the BOM report of the selected board
'
' Checkboxes:
'	Condense = Condense the report to show quantites of item numbers [easier to add BOM into QB]
'	Catagory = Checked fields are to be included in the BOM report [EXCEPTION: Reference Designators are removed on a condensed report]
'	Process = Checked processes that are to be included in the BOM report.
'-----------------------------------------------------------------------------------------------------------------------------------------

Public Class BOMReport

	Dim BOM_DataTable As New DataTable
	Dim BOM_da As New SqlDataAdapter
	Dim BOM_ds As New DataSet
	Dim BOM_Cmd = New SqlCommand("", myConn)

	Private Sub GenerateBOM_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		'Populate Board drop-down.
		GetBoardDropDownItems(Boards_ComboBox)

		If Boards_ComboBox.Items.Count <> 0 Then
			Boards_ComboBox.SelectedIndex = 0
		End If

		Boards_ComboBox.DropDownHeight = 200
	End Sub

	Private Sub GenerateReport_Button_Click() Handles GenerateReport_Button.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				Generate_Query()
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			Generate_Query()
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub Generate_Query()
		'Check to see if we are in the middle of an import.
		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		'Check to see that we have at least one process selected for our report.
		If HandFlow_CheckBox.Checked = False And PCBboard_CheckBox.Checked = False And PostAssembly_CheckBox.Checked = False And SMT_CheckBox.Checked = False And BAS_CheckBox.Checked = False Then
			MsgBox("Please select at least one item from the 'Process' Group.")
			Return
		End If

		'Set up the table.
		BOM_DataTable = New DataTable
		BOM_DataTable.Columns.Add(DB_HEADER_QUANTITY, GetType(Integer))
		BOM_DataTable.Columns.Add(DB_HEADER_REF_DES, GetType(String))
		BOM_DataTable.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		BOM_DataTable.Columns.Add(DB_HEADER_DESCRIPTION, GetType(String))
		BOM_DataTable.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		BOM_DataTable.Columns.Add(DB_HEADER_MPN, GetType(String))
		BOM_DataTable.Columns.Add(DB_HEADER_PROCESS, GetType(String))

		'Build up our query based on the Proccess checkboxs that are enabled.
		'Because this is inclusive, we have to use the 'OR' SQL keyword. 'firstProcess' boolean helps with when we need to use that string.
		Dim newQuery As New Text.StringBuilder("SELECT * FROM " & TABLE_PCADBOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & Boards_ComboBox.Text & "' AND (")
		Dim firstProcess As Boolean = True

		'HANDFLOW check
		If HandFlow_CheckBox.Checked = True Then
			newQuery.Append("[" & DB_HEADER_PROCESS & "] = '" & PROCESS_HANDFLOW & "'")
			firstProcess = False
		End If

		'PCB check
		If PCBboard_CheckBox.Checked = True Then
			If firstProcess = False Then
				newQuery.Append(" OR ")
			End If
			newQuery.Append("[" & DB_HEADER_PROCESS & "] = '" & PROCESS_PCBBOARD & "'")
			firstProcess = False
		End If

		'POST ASSEMBLY check
		If PostAssembly_CheckBox.Checked = True Then
			If firstProcess = False Then
				newQuery.Append(" OR ")
			End If
			newQuery.Append("[" & DB_HEADER_PROCESS & "] = '" & PROCESS_POSTASSEMBLY & "'")
			firstProcess = False
		End If

		'SMT check
		If SMT_CheckBox.Checked = True Then
			If firstProcess = False Then
				newQuery.Append(" OR ")
			End If
			newQuery.Append("[" & DB_HEADER_PROCESS & "] LIKE '" & PROCESS_SMT & "%'")
			firstProcess = False
		End If

		'BAS check
		If BAS_CheckBox.Checked = True Then
			If firstProcess = False Then
				newQuery.Append(" OR ")
			End If
			newQuery.Append("[" & DB_HEADER_PROCESS & "] = '" & PROCESS_BAS & "'")
			firstProcess = False
		End If

		'Determine the sorting order based on the Condense CheckBox.
		If Condense_CheckBox.Checked = True Then
			newQuery.Append(") ORDER BY [" & DB_HEADER_PROCESS & "] DESC, [" & DB_HEADER_ITEM_NUMBER & "] ASC")
		Else
			newQuery.Append(") ORDER BY [" & DB_HEADER_PROCESS & "] DESC, [" & DB_HEADER_REF_DES & "] ASC")
		End If

		Populate_DGV(newQuery.ToString)
	End Sub

	Private Sub Populate_DGV(ByRef query As String)
		Dim PCAD_BOM As New DataTable()

		'Fill up our DataTable based on if we are condensed or not.
		If Condense_CheckBox.Checked = True Then
			'This is creating a condensed report.
			Dim myCmd As New SqlCommand("SELECT [" & DB_HEADER_DESCRIPTION & "], [" & DB_HEADER_VENDOR & "], [" & DB_HEADER_MPN & "], [" & DB_HEADER_PROCESS & "], [" & DB_HEADER_ITEM_PREFIX & "], [" & DB_HEADER_ITEM_NUMBER & "], COUNT([" & DB_HEADER_ITEM_NUMBER & "]) as '" & DB_HEADER_QUANTITY & "' FROM " & TABLE_PCADBOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & Boards_ComboBox.Text & "' " &
										"GROUP BY [" & DB_HEADER_ITEM_NUMBER & "], [" & DB_HEADER_DESCRIPTION & "], [" & DB_HEADER_VENDOR & "], [" & DB_HEADER_MPN & "], [" & DB_HEADER_PROCESS & "], [" & DB_HEADER_ITEM_PREFIX & "] ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]", myConn)

			PCAD_BOM.Load(myCmd.ExecuteReader)

			For Each dr As DataRow In PCAD_BOM.Rows
				BOM_DataTable.Rows.Add(dr(DB_HEADER_QUANTITY), "", dr(DB_HEADER_ITEM_PREFIX) & ":" & dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_DESCRIPTION), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_PROCESS))
			Next
		Else
			'This is creating a full report.
			BOM_Cmd.commandText = query
			PCAD_BOM.Load(BOM_Cmd.ExecuteReader)

			For Each dr As DataRow In PCAD_BOM.Rows
				BOM_DataTable.Rows.Add(Nothing, dr(DB_HEADER_REF_DES), dr(DB_HEADER_ITEM_PREFIX) & ":" & dr(DB_HEADER_ITEM_NUMBER), dr(DB_HEADER_DESCRIPTION), dr(DB_HEADER_VENDOR), dr(DB_HEADER_MPN), dr(DB_HEADER_PROCESS))
			Next

			'We have to delete the Quantity column because this is not a condensed report.
			BOM_DataTable.Columns.Remove(DB_HEADER_QUANTITY)
		End If

		'Delete Columns according to the Catagory checkboxes.
		If ReferenceDesignator_CheckBox.Enabled = False Or ReferenceDesignator_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_REF_DES)
		End If
		If ItemNumber_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_ITEM_NUMBER)
		End If
		If Value_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_DESCRIPTION)
		End If
		If Vendor_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_VENDOR)
		End If
		If MPN_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_MPN)
		End If
		If Process_CheckBox.Checked = False Then
			BOM_DataTable.Columns.Remove(DB_HEADER_PROCESS)
		End If

		BOM_ds = New DataSet()
		BOM_ds.Tables.Add(BOM_DataTable)

		BOM_DGV.DataSource = Nothing
		BOM_DGV.DataSource = BOM_ds.Tables(0)
		BOM_DGV.AutoResizeColumns(DataGridViewAutoSizeColumnMode.AllCells)
		Excel_Button.Enabled = True
	End Sub

	Private Sub Excel_Button_Click() Handles Excel_Button.Click
		Dim report As New GenerateReport()
		report.GenerateBOMReport(BOM_ds, Boards_ComboBox.Text, Condense_CheckBox.Checked)
	End Sub

	Sub GetBoardDropDownItems(ByRef box As ComboBox)
		'Grab all of the board names.
		Dim BoardNames As New DataTable()
		Dim myCmd As New SqlCommand("SELECT Distinct([" & DB_HEADER_BOARD_NAME & "]) FROM " & TABLE_PCADBOM & " ORDER BY [" & DB_HEADER_BOARD_NAME & "]", myConn)

		BoardNames.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In BoardNames.Rows
			box.Items.Add(dr(DB_HEADER_BOARD_NAME))
		Next
	End Sub

	Private Sub Close_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close_Button.Click
		Close()
	End Sub

	Private Sub Condense_CheckBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Condense_CheckBox.CheckedChanged
		'If we are checking the box then disable our Reference Designator catagory
		'Else, enable our Reference Designator catagory.
		If Condense_CheckBox.Checked = True Then
			'If we are making a condense report, we cannot use the reference designators.
			ReferenceDesignator_CheckBox.Enabled = False
		Else
			ReferenceDesignator_CheckBox.Enabled = True
		End If

		'Disable the Excel button so we do not create under mis-information.
		Excel_Button.Enabled = False
	End Sub

	Private Sub Boards_ComboBox_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Boards_ComboBox.SelectedValueChanged
		'Disable the Excel button so we do not create under mis-information.
		Excel_Button.Enabled = False
	End Sub

	Private Sub BOM_DGV_RowPostPaint(ByVal sender As Object, ByVal e As DataGridViewRowPostPaintEventArgs) Handles BOM_DGV.RowPostPaint
		'Go through each row of the DGV and add the row number to the row header.
		Using b As SolidBrush = New SolidBrush(BOM_DGV.RowHeadersDefaultCellStyle.ForeColor)
			e.Graphics.DrawString(e.RowIndex + 1, BOM_DGV.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4)
		End Using
	End Sub

End Class