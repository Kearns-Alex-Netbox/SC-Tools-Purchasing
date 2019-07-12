'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: GenerateReport.vb
'
' Description: Generates each report through Excel with special formatting.
'
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports Microsoft.Office.Interop
Imports System.Data.SqlClient

Public Class GenerateReport

	Public Sub GenerateImportReport(ByRef list As RichTextBox)
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			xlWorkSheet.PageSetup.CenterHeader = "Import Output Report   " & Date.Now

			For Each line In list.Lines
				xlWorkSheet.Cells(INDEX_row, 1) = line
				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub CompareAllReport(ByRef name As String,
								ByRef ALPHA_Missing As DataTable, ByRef ALPHA_Extra As DataTable,
								ByRef QB_Missing As DataTable, ByRef QB_Extra As DataTable,
								ByRef QB_Missing2 As DataTable, ByRef QB_Extra2 As DataTable,
								ByRef PCAD_Quantity As DataTable, ByRef QB_Quantity As DataTable,
								ByRef ALPHA_Quantity As DataTable, ByRef QB_Quantity2 As DataTable)
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			xlWorkBook = xlApp.Workbooks.Add(misValue)

			'----- SHEET 1 -----'

			xlWorkSheet = xlWorkBook.Sheets("sheet1")
			xlWorkSheet.Name = "PCAD-ALPHA"
			xlWorkSheet.PageSetup.CenterHeader = "Difference Report [PCAD/ALPHA] for: " & name & "   " & Date.Now.Date

			'ROW 1
			Dim INDEX_row As Integer = 1
			Dim INDEX_Column As Integer = 1

			'ROW 2
			INDEX_row += 1

			For Each header In ALPHA_Missing.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, 1) = "ALPHA Missing parts [In PCAD Not in ALPHA]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, 1), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			Dim nextColumn2 = INDEX_Column

			For Each header In ALPHA_Extra.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn2) = "ALPHA Extra parts [In ALPHA Not in PCAD]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn2), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			'ROW 3
			INDEX_row += 1
			INDEX_Column = 1

			For row = 0 To ALPHA_Missing.Rows.Count - 1
				For column = 0 To ALPHA_Missing.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = ALPHA_Missing(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = 1
				INDEX_row += 1
			Next

			INDEX_Column = nextColumn2
			INDEX_row = 3

			For row = 0 To ALPHA_Extra.Rows.Count - 1
				For column = 0 To ALPHA_Extra.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = ALPHA_Extra(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn2
				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1:A2").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			'----- SHEET 2 -----'

			xlWorkSheet = xlWorkBook.Sheets("sheet2")
			xlWorkSheet.Name = "PCAD-QB"
			xlWorkSheet.PageSetup.CenterHeader = "Difference Report [PCAD/QB] for: " & name & "   " & Date.Now.Date

			'ROW 1
			INDEX_row = 1
			INDEX_Column = 1

			'ROW 2
			INDEX_row += 1

			For Each header In QB_Missing.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, 1) = "QB Missing parts [In PCAD Not in QB]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, 1), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			nextColumn2 = INDEX_Column

			For Each header In QB_Extra.Columns
				xlWorkSheet.Cells(1, nextColumn2) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn2) = "QB Extra parts [In QB Not in PCAD]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn2), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			'ROW 3
			INDEX_row += 1
			INDEX_Column = 1

			For row = 0 To QB_Missing.Rows.Count - 1
				For column = 0 To QB_Missing.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Missing(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = 1
				INDEX_row += 1
			Next

			INDEX_Column = nextColumn2
			INDEX_row = 3

			For row = 0 To QB_Extra.Rows.Count - 1
				For column = 0 To QB_Extra.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Extra(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn2
				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1:A2").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			'----- SHEET 3 -----'

			xlWorkSheet = xlWorkBook.Sheets("sheet3")
			xlWorkSheet.Name = "ALPHA-QB"
			xlWorkSheet.PageSetup.CenterHeader = "Difference Report [ALPHA/QB] for: " & name & "   " & Date.Now.Date

			'ROW 1
			INDEX_row = 1
			INDEX_Column = 1

			'ROW 2
			INDEX_row += 1

			For Each header In QB_Missing2.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, 1) = "QB Missing parts [In ALPHA Not in QB]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, 1), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			nextColumn2 = INDEX_Column

			For Each header In QB_Extra2.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn2) = "QB Extra parts [In QB Not in ALPHA]"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn2), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			'ROW 3
			INDEX_row += 1
			INDEX_Column = 1

			For row = 0 To QB_Missing2.Rows.Count - 1
				For column = 0 To QB_Missing2.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Missing2(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = 1
				INDEX_row += 1
			Next

			INDEX_row = 3
			INDEX_Column = nextColumn2

			For row = 0 To QB_Extra2.Rows.Count - 1
				For column = 0 To QB_Extra2.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Extra2(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn2
				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1:A2").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			'----- SHEET 4 -----'

			xlWorkSheet = xlWorkBook.Sheets.Add(After:=xlWorkBook.Sheets(xlWorkBook.Sheets.Count))
			xlWorkSheet.Name = "Quantities"
			xlWorkSheet.PageSetup.CenterHeader = "Difference Report [Quantities] for: " & name & "   " & Date.Now.Date

			'ROW 1
			INDEX_row = 1
			INDEX_Column = 1

			'ROW 2
			INDEX_row += 1

			For Each header In PCAD_Quantity.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, 1) = "PCAD Against QB"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, 1), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			nextColumn2 = INDEX_Column

			For Each header In QB_Quantity.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn2) = "QB Against PCAD"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn2), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			Dim nextColumn3 = INDEX_Column

			For Each header In ALPHA_Quantity.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn3) = "ALPHA Against QB"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn3), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			INDEX_Column += 1
			Dim nextColumn4 = INDEX_Column

			For Each header In QB_Quantity2.Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_Column) = header.columnName
				INDEX_Column += 1
			Next

			xlWorkSheet.Cells(1, nextColumn4) = "QB Against ALPHA"
			xlWorkSheet.Range(xlWorkSheet.Cells(1, nextColumn4), xlWorkSheet.Cells(1, INDEX_Column - 1)).MergeCells = True

			'ROW 3
			INDEX_row += 1
			INDEX_Column = 1

			For row = 0 To PCAD_Quantity.Rows.Count - 1
				For column = 0 To PCAD_Quantity.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = PCAD_Quantity(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = 1
				INDEX_row += 1
			Next

			INDEX_row = 3
			INDEX_Column = nextColumn2

			For row = 0 To QB_Quantity.Rows.Count - 1
				For column = 0 To QB_Quantity.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Quantity(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn2
				INDEX_row += 1
			Next

			INDEX_row = 3
			INDEX_Column = nextColumn3

			For row = 0 To ALPHA_Quantity.Rows.Count - 1
				For column = 0 To ALPHA_Quantity.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = ALPHA_Quantity(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn3
				INDEX_row += 1
			Next

			INDEX_row = 3
			INDEX_Column = nextColumn4

			For row = 0 To QB_Quantity2.Rows.Count - 1
				For column = 0 To QB_Quantity2.Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_Column) = QB_Quantity2(row)(column)
					INDEX_Column += 1
				Next
				INDEX_Column = nextColumn4
				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1:A2").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			xlWorkBook.Sheets("PCAD-ALPHA").Select()

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub GenerateBOMReport(ByRef ds As DataSet, ByRef name As String, ByRef isCondensed As Boolean)
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			If isCondensed Then
				xlWorkSheet.PageSetup.CenterHeader = name & " Condensed BOM Report   " & Date.Now
			Else
				xlWorkSheet.PageSetup.CenterHeader = name & " Expanded BOM Report   " & Date.Now
			End If


			For Each dc As DataColumn In ds.Tables(0).Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = dc.ColumnName
				INDEX_column += 1
			Next

			'Advance to the next row and reset the column to 1.
			INDEX_row += 1
			INDEX_column = 1

			For Each dr As DataRow In ds.Tables(0).Rows
				For Each dc As DataColumn In ds.Tables(0).Columns
					xlWorkSheet.Cells(INDEX_row, INDEX_column) = dr(dc).ToString
					INDEX_column += 1
				Next
				INDEX_row += 1
				'Reset the Column index
				INDEX_column = 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub GenerateCostReport(ByRef name As String, ByRef ds As DataSet, ByRef totalQuantity As String, ByRef totalCost As String)
		Dim item As Integer = 1
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			xlWorkSheet.PageSetup.CenterHeader = name & " Cost Report   " & Date.Now

			xlWorkSheet.Cells(INDEX_row, 1) = "(**) Component not found in the database. Totals will not include these component(s)."
			xlWorkSheet.Range("A1:G1").MergeCells = True

			INDEX_row += 2

			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Item"
			INDEX_column += 1
			For Each dc As DataColumn In ds.Tables(0).Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = dc.ColumnName
				INDEX_column += 1
			Next

			INDEX_row += 1
			INDEX_column = 2

			For row = 0 To ds.Tables(0).Rows.Count - 1
				xlWorkSheet.Cells(INDEX_row, 1) = item
				For column = 0 To ds.Tables(0).Columns.Count - 1
					xlWorkSheet.Cells(INDEX_row, INDEX_column) = ds.Tables(0)(row)(column)
					INDEX_column += 1
				Next
				INDEX_column = 2
				INDEX_row += 1
				item += 1
			Next

			INDEX_row += 1
			xlWorkSheet.Cells(INDEX_row, 2) = "          Total"
			xlWorkSheet.Cells(INDEX_row, 4) = totalQuantity
			xlWorkSheet.Cells(INDEX_row, 5) = totalCost

			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("C1").EntireColumn.NumberFormat = "_($* #,##0.00#####_);_($* (#,##0.00#####);_($* ""-""??_);_(@_)"
			xlWorkSheet.Range("C1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight
			xlWorkSheet.Range("E1").EntireColumn.NumberFormat = "_($* #,##0.00#####_);_($* (#,##0.00#####);_($* ""-""??_);_(@_)"
			xlWorkSheet.Range("E1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight
			xlWorkSheet.Range("A4").EntireRow.Font.Bold = True

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub GenerateBuildProductsReport(ByRef boards As ListBox, ByRef ds As DataSet, ByRef inventory_chk As Boolean, ByRef ShowAll_chk As Boolean,
										   ByRef uniqueParts As Integer, ByRef levelKey As List(Of String))
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			Dim costIndex As Integer = 0
			Dim totalCostIndex As Integer = 0
			Dim levelIndex As Integer = 0

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			Dim Style_Out As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Part Out")
			Style_Out.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 151, 163))

			Dim Style_Order As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Order Soon")
			Style_Order.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 235, 156))

			Dim Style_NotFound As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Not Found")
			Style_NotFound.Interior.Color = ColorTranslator.ToOle(Color.Orange)

			Dim Style_AssemblyOut As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Assembly Out")
			Style_AssemblyOut.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 199, 206))

			xlWorkSheet.PageSetup.CenterHeader = "Build Products Report   " & Date.Now

			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Quantity"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Product"
			INDEX_row += 1

			'Display how many of each board we built for
			For Each board In boards.Items
				Dim info() As String = board.ToString.Split("|")
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = info(0)
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = info(1)
				INDEX_row += 1
			Next

			INDEX_row += 1
			'Show what conditions were used.
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Use Inventory"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			If inventory_chk = True Then
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Enabled"
			Else
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Disabled"
			End If

			INDEX_row += 1
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Show All"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			If ShowAll_chk = True Then
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Enabled"
			Else
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Disabled"
			End If
			INDEX_row += 2

			'Key to Levels
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Assembly"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Key"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1).Font.Bold = True
			INDEX_row += 1
			For Each entryKey As String In levelKey
				Dim keySplit() = entryKey.Split("|")
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = keySplit(0)
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = keySplit(1)
				INDEX_row += 1
			Next
			Dim endOfHeader As Integer = INDEX_row

			INDEX_row = 1
			INDEX_column = 4

#If obsolete = 1 Then
			'Tally of parts
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "VendorList"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "# of Items"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1).Font.Bold = True

			xlWorkSheet.Cells(INDEX_row + 1, INDEX_column) = "My Arrow"
			xlWorkSheet.Cells(INDEX_row + 2, INDEX_column) = "Arrow"
			xlWorkSheet.Cells(INDEX_row + 3, INDEX_column) = "Digikey"
			xlWorkSheet.Cells(INDEX_row + 4, INDEX_column) = "Newark"
			xlWorkSheet.Cells(INDEX_row + 5, INDEX_column) = "FAI"
			xlWorkSheet.Cells(INDEX_row + 6, INDEX_column) = "Mouser"
			xlWorkSheet.Cells(INDEX_row + 7, INDEX_column) = "Verical"
			xlWorkSheet.Cells(INDEX_row + 8, INDEX_column) = "In Stock"

			xlWorkSheet.Cells(INDEX_row + 10, INDEX_column) = "Total # of Items"
			xlWorkSheet.Cells(INDEX_row + 11, INDEX_column) = "Expected # of Items"
			xlWorkSheet.Cells(INDEX_row + 11, INDEX_column + 1) = uniqueParts

			xlWorkSheet.Cells(INDEX_row + 13, INDEX_column) = "Notes."

#end If
			If endOfHeader < INDEX_row + 14 Then
				endOfHeader = INDEX_row + 14
			End If

			INDEX_column = 1
			INDEX_row = endOfHeader + 2
			For Each dc As DataColumn In ds.Tables(0).Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = dc.ColumnName
				If dc.ColumnName = DB_HEADER_COST Then
					costIndex = INDEX_column
				ElseIf dc.ColumnName = HEADER_TOTAL_COST Then
					totalCostIndex = INDEX_column
				ElseIf dc.ColumnName = HEADER_LEVEL_KEY Then
					levelIndex = INDEX_column
				End If
				INDEX_column += 1
			Next

			Dim OutlineTopRow As Integer = INDEX_row
			Dim OutlineColumn As Integer = 0

			'These are extra headers for the excel sheet for Lou/Hunter. Add additional if needed.
#If obsolete = 1 Then
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Arrow Buy?"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Vendor"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 2) = "Pur QTY"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 3) = "Unit Price"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 4) = "Total Cost"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 5) = "Conf Buy"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 6) = "Exp Date"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 7) = "Comment"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 8) = "Order #"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 9) = "Received"
			OutlineColumn = INDEX_column + 9
#else
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 0) = "Vendor"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Order #"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 2) = "Exp Date"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 3) = "Pur QTY"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 4) = "Unit Price"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 5) = "Total Cost"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 6) = "Received"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 7) = "Initial"
			OutlineColumn = INDEX_column + 7
#End if

			xlWorkSheet.Range("A" & INDEX_row).EntireRow.Font.Bold = True
			INDEX_column = 1
			INDEX_row += 1

			For Each dr As DataRow In ds.Tables(0).Rows
				Dim index As Integer = 0
				'Add the row of the dataTable
				For Each dc As DataColumn In ds.Tables(0).Columns
					xlWorkSheet.Cells(INDEX_row, INDEX_column + index) = dr(index).ToString
					index += 1
				Next

				' Add any style that the row needs 
				If dr(HEADER_REMAINDER).ToString <> "" Then

					If dr(HEADER_QTY_AVAIL).ToString = NOT_IN_DATABASE Then
						'NOT IN DATABASE

						xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, INDEX_column), xlWorkSheet.Cells(INDEX_row, INDEX_column + index - 1)).Style = Style_NotFound
					ElseIf CInt(dr(HEADER_REMAINDER).ToString) < 0 Then
						'Less than 0 Remainder
						Try
							Dim myCmd As New SqlCommand("SELECT [" & DB_HEADER_TYPE & "] FROM " & TABLE_QB_ITEMS & " WHERE UPPER([" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]) = UPPER('" & dr(DB_HEADER_ITEM_NUMBER).trim & "')", myConn)
							Dim type As String = myCmd.ExecuteScalar

							If type = "Inventory Assembly" Then
								'Assembly
								xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, INDEX_column), xlWorkSheet.Cells(INDEX_row, INDEX_column + index - 1)).Style = Style_AssemblyOut
							Else
								'Inventory Part
								xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, INDEX_column), xlWorkSheet.Cells(INDEX_row, INDEX_column + index - 1)).Style = Style_Out
							End If
						Catch ex As Exception
							xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, INDEX_column), xlWorkSheet.Cells(INDEX_row, INDEX_column + index - 1)).Style = Style_Out
						End Try
					Else
						'Grabe the re-Order Quantity if we have one.
						Dim myCmd As New SqlCommand("SELECT [" & DB_HEADER_REORDER_QTY & "] FROM " & TABLE_QB_ITEMS & " WHERE UPPER([" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]) = UPPER('" & dr(DB_HEADER_ITEM_NUMBER).trim & "')", myConn)
						Dim roq As Integer = BuildProducts.DEFAULT_REORDER_QT

						Try
							roq = CInt(myCmd.ExecuteScalar)
							If roq = 0 Then
								roq = BuildProducts.DEFAULT_REORDER_QT
							End If
						Catch ex As Exception
							'Do nothing
						End Try

						If CInt(dr(HEADER_REMAINDER).ToString) <= roq Then
							'Less than the Re-Order Quantity
							xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, INDEX_column), xlWorkSheet.Cells(INDEX_row, INDEX_column + index - 1)).Style = Style_Order
						End If
					End If

					'Make all Assemblies stand out by making them bold
					Try
						Dim myCmd As New SqlCommand("SELECT [" & DB_HEADER_TYPE & "] FROM " & TABLE_QB_ITEMS & " WHERE UPPER([" & DB_HEADER_ITEM_PREFIX & "] + ':' + [" & DB_HEADER_ITEM_NUMBER & "]) = UPPER('" & dr(DB_HEADER_ITEM_NUMBER).trim & "')", myConn)
						Dim type As String = myCmd.ExecuteScalar

						If type = "Inventory Assembly" Then
							xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
						End If
					Catch ex As Exception
						'Do Nothing.
					End Try
				End If
				INDEX_row += 1
			Next

			Dim OutlineBottomRow As Integer = INDEX_row - 1


			Dim range As Excel.Range
			range = xlWorkSheet.UsedRange

			range.EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
			range.EntireColumn.AutoFit()
			range.EntireColumn.NumberFormat = "0"
			range.EntireColumn.NumberFormat = "General"

			'Cost
			xlWorkSheet.Cells(1, costIndex).EntireColumn.NumberFormat = "_($* #,##0.00000##_);_($* (#,##0.00000##);_($* ""-""??_);_(@_)"

			'Total Cost
			xlWorkSheet.Cells(1, totalCostIndex).EntireColumn.NumberFormat = "_($* #,##0.00000##_);_($* (#,##0.00000##);_($* ""-""??_);_(@_)"
			xlWorkSheet.Range("A1").EntireRow.Font.Bold = True

			'Level Key
			xlWorkSheet.Cells(1, levelIndex).EntireColumn.NumberFormat = "#0.0#"

			'Boarder
			range = xlWorkSheet.Range(xlWorkSheet.Cells(OutlineTopRow, 1), xlWorkSheet.Cells(OutlineBottomRow, OutlineColumn))
			Dim borders As Excel.Borders = range.Borders
			borders.LineStyle = Excel.XlLineStyle.xlContinuous

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub GenerateCriticalBuildReport(ByRef boards As ListBox, ByRef ds As DataSet, ByRef inventory_chk As Boolean, ByRef levelKey As List(Of String))
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			Dim costIndex As Integer = 0
			Dim totalCostIndex As Integer = 0
			Dim levelIndex As Integer = 0

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			Dim Style_Product As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Product Title")
			Style_Product.Interior.Color = ColorTranslator.ToOle(Color.LightGreen)

			Dim Style_NotFound As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Not Found")
			Style_NotFound.Interior.Color = ColorTranslator.ToOle(Color.Orange)

			Dim Style_Assembly As Excel.Style = xlWorkSheet.Application.ActiveWorkbook.Styles.Add("Assembly")
			Style_Assembly.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 199, 206))

			xlWorkSheet.PageSetup.CenterHeader = "Critical Build Path Report   " & Date.Now

			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Quantity"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Product"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1).Font.Bold = True
			INDEX_row += 1

			'Display how many of each board we built for
			For Each board In boards.Items
				Dim info() As String = board.ToString.Split("|")
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = info(0)
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = info(1)
				INDEX_row += 1
			Next

			INDEX_row += 1
			'Show what conditions were used.
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Use Inventory"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			If inventory_chk = True Then
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Enabled"
			Else
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Disabled"
			End If
			INDEX_row += 2

			'Key to Levels
			xlWorkSheet.Cells(INDEX_row, INDEX_column) = "Assembly"
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = "Key"
			xlWorkSheet.Cells(INDEX_row, INDEX_column).Font.Bold = True
			xlWorkSheet.Cells(INDEX_row, INDEX_column + 1).Font.Bold = True
			INDEX_row += 1
			For Each entryKey As String In levelKey
				Dim keySplit() = entryKey.Split("|")
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = entryKey
				xlWorkSheet.Cells(INDEX_row, INDEX_column + 1) = levelKey.IndexOf(entryKey)
				INDEX_row += 1
			Next

			INDEX_row += 1
			For Each dc As DataColumn In ds.Tables(0).Columns
				xlWorkSheet.Cells(INDEX_row, INDEX_column) = dc.ColumnName
				If dc.ColumnName = DB_HEADER_COST Then
					costIndex = INDEX_column
				ElseIf dc.ColumnName = HEADER_TOTAL_COST Then
					totalCostIndex = INDEX_column
				ElseIf dc.ColumnName = HEADER_LEVEL_KEY Then
					levelIndex = INDEX_column
				End If
				INDEX_column += 1
			Next

			xlWorkSheet.Range("A" & INDEX_row).EntireRow.Font.Bold = True

			INDEX_column = 1
			INDEX_row += 1

			For Each dr As DataRow In ds.Tables(0).Rows
				Dim index As Integer = 0
				'Add the row of the dataTable
				For Each dc As DataColumn In ds.Tables(0).Columns
					xlWorkSheet.Cells(INDEX_row, INDEX_column + index) = dr(index).ToString
					index += 1
				Next

				'Product Header.
				If String.IsNullOrEmpty(xlWorkSheet.Cells(INDEX_row, 1).Value) = False Then
					If xlWorkSheet.Cells(INDEX_row, 1).Value.contains("-") = True Then
						xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, 1), xlWorkSheet.Cells(INDEX_row, 1)).Style = Style_Product
					End If
				End If

				'Assemblies.
				If String.IsNullOrEmpty(xlWorkSheet.Cells(INDEX_row, 5).Value) = False Then
					If xlWorkSheet.Cells(INDEX_row, 5).Value.contains(PREFIX_BAS & "-") = True Then
						xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, 5), xlWorkSheet.Cells(INDEX_row, 5)).Style = Style_Assembly
					ElseIf xlWorkSheet.Cells(INDEX_row, 5).Value.contains(PREFIX_SMA & "-") = True Then
						xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, 5), xlWorkSheet.Cells(INDEX_row, 5)).Style = Style_Assembly
					End If
				End If

				'Not in Database.
				If String.IsNullOrEmpty(xlWorkSheet.Cells(INDEX_row, 6).Value) = False Then
					If xlWorkSheet.Cells(INDEX_row, 6).Value.contains("???") = True Then
						xlWorkSheet.Range(xlWorkSheet.Cells(INDEX_row, 10), xlWorkSheet.Cells(INDEX_row, 10)).Style = Style_NotFound
					End If
				End If

				INDEX_row += 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"

			'Cost
			xlWorkSheet.Cells(1, costIndex).EntireColumn.NumberFormat = "_($* #,##0.00#####_);_($* (#,##0.00#####);_($* ""-""??_);_(@_)"

			'Total Cost
			xlWorkSheet.Cells(1, totalCostIndex).EntireColumn.NumberFormat = "_($* #,##0.00#####_);_($* (#,##0.00#####);_($* ""-""??_);_(@_)"
			xlWorkSheet.Range("A1").EntireRow.Font.Bold = True

			'Level Key
			xlWorkSheet.Cells(1, levelIndex).EntireColumn.NumberFormat = "#0.0#"

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Public Sub GenerateQB_itemslistReport(ByRef ds As DataSet)
		Try
			Dim xlApp As New Excel.Application
			Dim xlWorkBook As Excel.Workbook
			Dim xlWorkSheet As Excel.Worksheet
			Dim misValue As Object = Reflection.Missing.Value
			Dim INDEX_row As Integer = 1
			Dim INDEX_column As Integer = 1

			xlWorkBook = xlApp.Workbooks.Add(misValue)
			xlWorkSheet = xlWorkBook.Sheets("sheet1")

			xlWorkSheet.PageSetup.CenterHeader = "QB Items Report   " & Date.Now

			For Each dc As DataColumn In ds.Tables(0).Columns
				xlWorkSheet.Cells(1, INDEX_column) = dc.ColumnName
				INDEX_column += 1
			Next

			INDEX_row += 1
			'Reset the Column index
			INDEX_column = 1

			For Each dr As DataRow In ds.Tables(0).Rows
				For Each dc As DataColumn In ds.Tables(0).Columns
					xlWorkSheet.Cells(INDEX_row, INDEX_column) = dr(dc).ToString
					INDEX_column += 1
				Next
				INDEX_row += 1
				'Reset the Column index
				INDEX_column = 1
			Next

			xlWorkSheet.Range("A1:X1").EntireColumn.AutoFit()
			xlWorkSheet.Range("A1:X1").EntireColumn.NumberFormat = "0"
			xlWorkSheet.Range("A1").EntireRow.Font.Bold = True
			xlWorkSheet.Range("A1:X1").EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft

			xlApp.DisplayAlerts = False
			xlApp.Visible = True

			releaseObject(xlWorkSheet)
			releaseObject(xlWorkBook)
			releaseObject(xlApp)
		Catch ex As Exception
			MsgBox("File was not written: " & ex.Message)
		End Try
	End Sub

	Private Sub releaseObject(ByVal obj As Object)
		Try
			Runtime.InteropServices.Marshal.ReleaseComObject(obj)
			obj = Nothing
		Catch ex As Exception
			obj = Nothing
		Finally
			GC.Collect()
		End Try
	End Sub

End Class
