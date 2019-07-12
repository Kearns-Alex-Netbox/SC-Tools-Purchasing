'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: Printing.vb
'
' Description: Takes a list of Item Numbers and prints either labels or cover sheets for them.
'
' RESTRICTIONS: The label template needs to have text fields and barcode fields equal to the ammount that the label supports. The fields 
'	should be names As: [TEXT # ], [BARCODE #] where '#' represents what number item it is starting at 1 and incrementing by 1s. The label
'	template that the user selects NEEDS To be In this format.
'		
' Buttons:
'	Print Label = Prints the list of Items to the selected DYMO label printer.
'	Print Cover = Prints the list of Items to the selected printer.
'
' Special Keys:
'   Delete = If we have an item highlighted in our parts listbox, then we remove it from the listbox.
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports DYMO.Label.Framework        'V:8.3.1.1332
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop

Public Class Printing

	Dim StockList As List(Of String)
	Dim labelPath As String
	Const MAX_LINE_CHARACTERS As Integer = 60
	Const MAX_LINES As Integer = 8

	Public Sub New(ByRef list As List(Of String))
		InitializeComponent()

		'Assign the passed in list to our own list.
		StockList = list
	End Sub

	Private Sub Printing_Load() Handles MyBase.Load

		labelPath = My.Settings.LabelPath

		For Each file As String In IO.Directory.GetFiles(labelPath, "*.label")
			Label_ComboBox.Items.Add(IO.Path.GetFileNameWithoutExtension(file))
		Next

		If Label_ComboBox.Items.Count <> 0 Then
			Label_ComboBox.SelectedIndex = 0
		End If

		Label_ComboBox.DropDownHeight = 200

		'Begin loading up the label printers that are connect to the PC.
		Dim labelPrinters() = Nothing
		Try
			labelPrinters = Framework.GetPrinters.ToArray
		Catch ex As Exception
			MsgBox("DYMO Drivers/printers are not installed on this computer.")
			PrintLabel_Button.Enabled = False
			Label_ComboBox.Enabled = False
		End Try
		'Dim labelPrinters() As IPrinter = Framework.GetPrinters.ToArray

		'Add our stock Items to our list.
		For Each stockItem In StockList
			'Filter out any BAS or FGS items.
			If stockItem.Contains(PREFIX_BAS & ":") Or stockItem.Contains(PREFIX_BIS & ":") Or stockItem.Contains(PREFIX_DAS & ":") Or stockItem.Contains(PREFIX_FGS & ":") Or stockItem.Contains(PREFIX_SMA & ":") Then
				Continue For
			End If
			ItemNumbers_ListBox.Items.Add(stockItem)
		Next

		KeyPreview = True
	End Sub

	Private Sub Add_Button_Click() Handles Add_Button.Click
		Dim itemNumber As String = AddStockNumber_TextBox.Text
		Dim hasPrefix As Boolean = False
		Dim itemPrefix As String = ""

		Dim myCmd As New SqlCommand("", myConn)
		Dim resultTable = New DataTable

		If itemNumber.Contains(":") = True Then
			itemPrefix = itemNumber.Substring(0, itemNumber.IndexOf(":"))
			itemNumber = itemNumber.Substring(itemNumber.IndexOf(":") + 1)
			hasPrefix = True
		End If

		If hasPrefix = False Then
			myCmd.CommandText = "SELECT * FROM QB_Items WHERE [Item Number] = '" & itemNumber & "'"
		Else
			myCmd.CommandText = "SELECT * FROM QB_Items WHERE [Item Prefix] = '" & itemPrefix & "' AND [Item Number] = '" & itemNumber & "'"
		End If

		resultTable.Load(myCmd.ExecuteReader)

		If resultTable.Rows.Count = 0 Then
			myCmd.CommandText = "SELECT * FROM QB_Items WHERE ([MPN] = '" & itemNumber & "' OR [MPN 2] = '" & itemNumber & "' OR [MPN 3] = '" & itemNumber & "')"
			resultTable = New DataTable
			resultTable.Load(myCmd.ExecuteReader)

			If resultTable.Rows.Count = 0 Then
				MsgBox("The item Number [" & AddStockNumber_TextBox.Text & "] Does not exist in the QB Database.")
				Return
			End If
		End If

		If resultTable.Rows.Count <> 1 Then
			If hasPrefix = False Then
				MsgBox("The item Number [" & AddStockNumber_TextBox.Text & "] has more than one item like it in the QB Database." & vbNewLine &
						"Please try to refine your search by adding a prefix [ie. 'CON:'].")
			Else
				MsgBox("The item Number [" & AddStockNumber_TextBox.Text & "] has more than one item like it in the QB Database." & vbNewLine &
						"This issue must be address and resolved before a label or cover can be made.")
			End If
			Return
		End If

		itemNumber = resultTable.Rows(0)(DB_HEADER_ITEM_PREFIX) & ":" & resultTable.Rows(0)(DB_HEADER_ITEM_NUMBER)

		'Add to our Build List
		ItemNumbers_ListBox.Items.Add(itemNumber)
		'Clear the Textbox.
		AddStockNumber_TextBox.Text = ""
	End Sub

	Private Sub Remove_Button_Click() Handles Remove_Button.Click
		If ItemNumbers_ListBox.SelectedItems.Count <> 0 Then
			ItemNumbers_ListBox.Items.Remove(ItemNumbers_ListBox.SelectedItem)
		End If
	End Sub

	Private Sub Close_Button_Click() Handles Close_Button.Click
		Close()
	End Sub

	Private Sub PrintLabel_Button_Click() Handles PrintLabel_Button.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					PrintLabel()
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				PrintLabel()
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub PrintLabel()
		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		InvalidItemNumbers_ListBox.Items.Clear()

		'Create a temp list for items that did not get added
		Dim invalidStockItems As New List(Of String)

		'Set Printer
		Dim p As New PrintDialog
		p.UseEXDialog = True
		If p.ShowDialog = Windows.Forms.DialogResult.OK Then

			Dim QBItemList = New DataTable
			Dim myCmd = New SqlCommand("SELECT * FROM " & TABLE_QB_ITEMS, myConn)
			QBItemList.Load(myCmd.ExecuteReader())

			Dim Label As ILabel
			Label = Framework.Open(labelPath & "\" & Label_ComboBox.Text & ".label")
			Dim labelNumber As Integer = 1

			'Check to see that we have the correct field and start at one.
			'If not, then let the user know and exit.
			If Label.ObjectNames.Contains("TEXT " & labelNumber) = False Then
				MsgBox("This label file does Not have an object defined as 'TEXT 1'. Double check the label format.")
				Return
			End If

			For Each stockLabel In ItemNumbers_ListBox.Items
				'Set up the variables that we are going to use.
				Dim search As String = stockLabel.substring(stockLabel.indexOf(":") + 1)

				Dim stockPrefix As String = ""
				Dim stockNumber As String = ""
				Dim manufacturer1 As String = ""
				Dim partNumber1 As String = ""
				Dim manufacturer2 As String = ""
				Dim partNumber2 As String = ""
				Dim manufacturer3 As String = ""
				Dim partNumber3 As String = ""
				Dim description As String = ""

				'Check/get information for the stock item
				Dim dr() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & search & "'")

				If dr.Length <> 0 Then
					stockPrefix = dr(0)(DB_HEADER_ITEM_PREFIX)
					stockNumber = dr(0)(DB_HEADER_ITEM_NUMBER)
					manufacturer1 = dr(0)(DB_HEADER_VENDOR)
					partNumber1 = dr(0)(DB_HEADER_MPN)
					manufacturer2 = dr(0)(DB_HEADER_VENDOR2)
					partNumber2 = dr(0)(DB_HEADER_MPN2)
					manufacturer3 = dr(0)(DB_HEADER_VENDOR3)
					partNumber3 = dr(0)(DB_HEADER_MPN3)
					description = dr(0)(DB_HEADER_DESCRIPTION)
				Else
					'The stockNumber does not exist in the database. Move on to the next item.
					invalidStockItems.Add(stockLabel)
					Continue For
				End If

				If Label.ObjectNames.Contains("TEXT " & labelNumber) Then
					Label.SetObjectText("TEXT " & labelNumber, stockPrefix & ":" & stockNumber & vbNewLine &
										manufacturer1 & ", " & partNumber1 & vbNewLine &
										manufacturer2 & ", " & partNumber2 & vbNewLine &
										manufacturer3 & ", " & partNumber3)

					Label.SetObjectText("BARCODE " & labelNumber, stockNumber)

					If Label.ObjectNames.Contains("DESCRIPTION " & labelNumber) Then
						Dim alteredDescription As String = WordWrap(description)

						Dim descriptionArray() As String = Split(alteredDescription, "|")
						Dim totalLines As Integer = MAX_LINES

						If descriptionArray.Length() <= MAX_LINES Then
							totalLines = descriptionArray.Length()
						End If

						For line = 1 To totalLines
							Label.SetObjectText("DESCRIPTION " & line, descriptionArray(line - 1))
						Next

					End If

					labelNumber += 1
				End If

				'Determine if we need to print now or not by checking to see if the next number is found or not.
				If Label.ObjectNames.Contains("TEXT " & labelNumber) = False Then
					'If we do not find the next number up, we have reached the end of our objects and need to print off and reset back to one.
					Label.Print(p.PrinterSettings.PrinterName)
					labelNumber = 1
				End If
			Next

			'Check to see if we did not finish printing the current label.
			If labelNumber <> 1 Then
				'If we are, we want to clear out any text in the label so we do not print duplicates.
				While Label.ObjectNames.Contains("TEXT " & labelNumber)
					Label.SetObjectText("TEXT " & labelNumber, "")
					Label.SetObjectText("BARCODE " & labelNumber, "")
					labelNumber += 1
				End While

				'Print our the labels and reset our number back to 1.
				Label.Print(p.PrinterSettings.PrinterName)
				labelNumber = 1
			End If

			'Check to see if we had any invalid stock items. If we do, add them to the list and let the user know.
			If invalidStockItems.Count <> 0 Then
				For Each stockItem In invalidStockItems
					InvalidItemNumbers_ListBox.Items.Add(stockItem)
				Next
				InvalidItemNumbers_ListBox.Items.Add("")
				InvalidItemNumbers_ListBox.Items.Add("This items are not found in the database.")
				InvalidItemNumbers_ListBox.Items.Add("All other stock items are good to go.")
			Else
				InvalidItemNumbers_ListBox.Items.Add("Everything is good to go.")
			End If
		End If
	End Sub

	Private Sub PrintCover_Button_Click() Handles PrintCover_Button.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					PrintCover()
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				PrintCover()
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub PrintCover()
		Try


			Dim message As String = ""
			If sqlapi.CheckDirtyBit(message) = True Then
				MsgBox(message)
				Return
			End If

			InvalidItemNumbers_ListBox.Items.Clear()
			'Create a temp list for items that did not get added
			Dim invalidStockItems As New List(Of String)

			' Get the Word application object.
			Dim word_app As Word._Application = New Word.ApplicationClass()

			'Set Printer
			Dim p As New PrintDialog
			p.UseEXDialog = True
			If p.ShowDialog = Windows.Forms.DialogResult.OK Then
				word_app.WordBasic.FilePrintSetup(Printer:=p.PrinterSettings.PrinterName, DoNotSetAsSysDefault:=1)
				word_app.ActivePrinter = p.PrinterSettings.PrinterName

				' Make Word visible (optional).
				word_app.Visible = False

				' Create the Word document.
				Dim word_doc As Word._Document = word_app.Documents.Add()

				Dim para As Word.Paragraph = word_doc.Paragraphs.Add()
				Dim pageNumber As Integer = 1

				Dim QBItemList = New DataTable
				Dim myCmd = New SqlCommand("SELECT * FROM " & TABLE_QB_ITEMS, myConn)
				QBItemList.Load(myCmd.ExecuteReader())

				For Each stockLabel In ItemNumbers_ListBox.Items
					'Set up the variables that we are going to use.
					Dim search As String = stockLabel.substring(stockLabel.indexOf(":") + 1)

					Dim stockPrefix As String = ""
					Dim stockNumber As String = ""
					Dim manufacturer1 As String = ""
					Dim partNumber1 As String = ""
					Dim manufacturer2 As String = ""
					Dim partNumber2 As String = ""
					Dim manufacturer3 As String = ""
					Dim partNumber3 As String = ""
					Dim description As String = ""

					'Check/get information for the stock item
					Dim dr() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & search & "'")

					If dr.Length <> 0 Then
						stockPrefix = dr(0)(DB_HEADER_ITEM_PREFIX)
						stockNumber = dr(0)(DB_HEADER_ITEM_NUMBER)
						manufacturer1 = dr(0)(DB_HEADER_VENDOR)
						partNumber1 = dr(0)(DB_HEADER_MPN)
						manufacturer2 = dr(0)(DB_HEADER_VENDOR2)
						partNumber2 = dr(0)(DB_HEADER_MPN2)
						manufacturer3 = dr(0)(DB_HEADER_VENDOR3)
						partNumber3 = dr(0)(DB_HEADER_MPN3)
						description = dr(0)(DB_HEADER_DESCRIPTION)

						If pageNumber <> 1 Then
							para.Range.InsertBreak(Word.WdBreakType.wdPageBreak)
						End If

						' Our stock number
						para.Range.Text = stockPrefix & ":" & stockNumber
						para.Range.Font.Name = "Calibri"
						para.Range.Font.Size = 26
						para.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter
						para.Range.InsertParagraphAfter()
						para.Range.InsertParagraphAfter()

						' Our description of our stock number
						para.Range.Text = description
						para.Range.InsertParagraphAfter()
						para.Range.InsertParagraphAfter()

						' Manufacturer 1 information
						If partNumber1.Length = 0 Then
							para.Range.Text = "No Manufacturer 1"
						Else
							para.Range.Text = manufacturer1 & ", " & partNumber1
						End If
						para.Range.InsertParagraphAfter()

						' Manufacturer 2 information
						If partNumber2.Length = 0 Then
							para.Range.Text = "No Manufacturer 2"
						Else
							para.Range.Text = manufacturer2 & ", " & partNumber2
						End If
						para.Range.InsertParagraphAfter()

						' Manufacturer 2 information
						If partNumber3.Length = 0 Then
							para.Range.Text = "No Manufacturer 3"
						Else
							para.Range.Text = manufacturer3 & ", " & partNumber3
						End If
						para.Range.InsertParagraphAfter()

						'set this variable to not = 1 so the first check in the beginning allows us to insert a page
						'   break allowing us to print off all of the cover sheets that we need.
						pageNumber = 2
					Else
						'The stockNumber does not exist in the database. Move on to the next item.
						invalidStockItems.Add(stockLabel)
						Continue For
					End If
				Next

				Try
					word_app.PrintOut(Background:=False)
				Catch ex As Exception
					MsgBox(ex.Message)
				End Try

				' Let the program sleep for a while so the printer can get the document to print out.
				'Threading.Thread.Sleep(1500)

				' Close.
				Dim save_changes As Object = False
				word_doc.Close(save_changes)
				word_app.Quit(save_changes)

				'Release 
				word_doc = Nothing
				word_app = Nothing

				'Check to see if we had any invalid stock items. if we do, add them to the list and let the user know.
				If invalidStockItems.Count <> 0 Then
					For Each stockItem In invalidStockItems
						InvalidItemNumbers_ListBox.Items.Add(stockItem)
					Next
					InvalidItemNumbers_ListBox.Items.Add("")
					InvalidItemNumbers_ListBox.Items.Add("This items are not found in the database.")
					InvalidItemNumbers_ListBox.Items.Add("All other stock items are good to go.")
				Else
					InvalidItemNumbers_ListBox.Items.Add("Everything is good to go.")
				End If

			End If
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub

	Private Sub MyBase_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
		If e.KeyCode.Equals(Keys.Delete) Then
			Call Remove_Button_Click()
		End If
	End Sub

	Private Function WordWrap(ByVal description As String) As String
		' This is our starting position within the text body wherewe start a new line
		Dim startingPosition As Integer

		' This is the ending position within the text body where we end a new line
		Dim endingPosition As Integer

		' This is used for the substring for the length of the line we need to pull.
		'    This has to be used because you can’t always break apart strings for
		'    word wrapping at exactly 86 characters.  You have to account for not
		'    breaking words in half, so you have to break apart at spaces.  You’ll
		'    see this below
		Dim lineLength As Integer = MAX_LINE_CHARACTERS

		' This is the line that we will be writing to the file.
		Dim alteredDescription As String = ""

		' Start looping through the text of the textbox until we reach the end.
		While startingPosition < description.Length
			' This locates the ending position of a line identified by a CRLF.
			endingPosition = description.IndexOf(Environment.NewLine, startingPosition)

			If endingPosition = -1 Then
				endingPosition = description.Length
			End If

			' This tells us that the complete line, from start to CRLF, is less than
			'    60 characters, so no word wrapping needs to be handled.
			If endingPosition - startingPosition < MAX_LINE_CHARACTERS Then
				' We are not at the end of the file, but we have a suitable line
				'    to write that doesn’t need word wrapping.  Get the line and
				'    set the start position of the next line to the end position of the
				'    current line + 1.
				alteredDescription = alteredDescription & description.Substring(startingPosition, endingPosition - startingPosition) & "|"
				startingPosition = endingPosition + 1
			Else
				' THIS IS WHERE WORD WRAPPING IS HANDLED
				' This continues to word wrap for every 60 characters in the line
				While lineLength + startingPosition <= endingPosition
					' This backtracks in the line in order to find a suitable place to word wrap,
					' in this case, a SPACE
					While description.Substring(startingPosition + lineLength - 1, 1) <> Chr(32)
						lineLength -= 1
					End While

					' Get the line.
					alteredDescription = alteredDescription & description.Substring(startingPosition, lineLength) & "|"

					' If we had to backtrack, we can’t add 1 to the count.  This
					' prevents us from cutting off the first letter of the next word.
					If lineLength < MAX_LINE_CHARACTERS Then
						startingPosition += lineLength
					Else
						startingPosition += lineLength + 1
					End If

					' Reset the lineLength back to the default.
					lineLength = MAX_LINE_CHARACTERS
				End While

				' This is the end of the lines we had to word wrap.  This writes
				' the last line that got word wrapped to the file.
				alteredDescription = alteredDescription & description.Substring(startingPosition, endingPosition - startingPosition)

				' Set the start position of the next line to the end position of the
				'    current line + 1.
				startingPosition = endingPosition + 1
			End If
		End While
		Return alteredDescription
	End Function

End Class