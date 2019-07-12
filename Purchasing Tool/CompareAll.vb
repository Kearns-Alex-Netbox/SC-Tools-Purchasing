'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: MasterControl.vb
'
' Description: Compares all of our BOM files from QB, PCAD, and ALPHA of the selected board to make sure they are all in agreement with
'		eachother. All disagreements are displayed in the tables.
'   PCAD to ALPHA Tab 1: Compares our PCAD BOM to our ALPHA BOM both ways. Checks Reference Designators and Item Number along with exsiting
'		inside our QB inventory. EXCEPTIONS: Only compares items that are part of the 'SMT' process.
'   PCAD to QB Tab 2: Compares our PCAD BOM to our QB BOM both ways. Covers both BAS and SMA assemblies in QB. Checks Item Number and Item
'		Prefix along with existing inside our QB inventory. EXCEPTIONS: Does not look at 'ZD' Reference Designators. Only checks 
'		'POST ASSEMBLIY' items that contain 'HRD' in it's name.
'   ALPHA to QB Tab 3: Compares our ALPHA BOM to our QB BOM both ways. Covers SMA assemblies. Checks Item Number. EXCEPTIONS: Only checks 
'		'SMT'/'SMA' items. Does not look at 'PCB' item.
'   Quantities Tab 4: Compares the number of times an item number is used in each BOMs and compares the number between all three different
'		sources.
'-----------------------------------------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient

Public Class CompareAll
	Dim QBitems As New DataTable

	Dim stamp As String = ""

	Dim DataTable_ALPHAmissingPCAD As New DataTable
	Dim DataTable_ALPHAextraPCAD As New DataTable

	Dim DataTable_QBmissingPCAD As New DataTable
	Dim DataTable_QBextraPCAD As New DataTable

	Dim DataTable_QBmissingALPHA As New DataTable
	Dim DataTable_QBextraALPHA As New DataTable

	Dim DataTable_PCAD_QB_quantity As New DataTable
	Dim DataTable_QB_PCAD_quantity As New DataTable
	Dim DataTable_ALPHA_QB_quantity As New DataTable
	Dim DataTable_QB_ALPHA_quantity As New DataTable

	Dim QBItemList As New DataTable

	Dim myCmd As New SqlCommand("", myConn)

	Private Sub CompareAll_Load() Handles MyBase.Load
		'Populate Board drop-down.
		GetBoardDropDownItems(Board_ComboBox)

		If Board_ComboBox.Items.Count <> 0 Then
			Board_ComboBox.SelectedIndex = 0
		End If

		Board_ComboBox.DropDownHeight = 200

		KeyPreview = True

		SetupDataTables()
		ResizeTables(TabPage1)
	End Sub

	Private Sub GenerateReport_Button_Click() Handles GenerateReport_Button.Click
		Cursor = Cursors.WaitCursor
		If LOGDATA = True Then
			Try
				If ChangeCheck(True) = True Then
					Compare()
				End If
			Catch ex As Exception
				UnhandledExceptionMessage(ex)
			End Try
		Else
			If ChangeCheck(True) = True Then
				Compare()
			End If
		End If
		Cursor = Cursors.Default
	End Sub

	Private Sub Compare()

		Dim message As String = ""
		If sqlapi.CheckDirtyBit(message) = True Then
			MsgBox(message)
			Return
		End If

		QBItemList = New DataTable
		myCmd.CommandText = "SELECT * FROM " & TABLE_QB_ITEMS
		QBItemList.Load(myCmd.ExecuteReader())

		'Clear all of our tables.
		DataTable_ALPHAmissingPCAD.Clear()
		DataTable_ALPHAextraPCAD.Clear()
		DataTable_QBmissingPCAD.Clear()
		DataTable_QBextraPCAD.Clear()
		DataTable_QBmissingALPHA.Clear()
		DataTable_QBextraALPHA.Clear()
		DataTable_PCAD_QB_quantity.Clear()
		DataTable_QB_PCAD_quantity.Clear()
		DataTable_ALPHA_QB_quantity.Clear()
		DataTable_QB_ALPHA_quantity.Clear()

		Dim board As String = Board_ComboBox.Text

		'Find out if our BOMs are the same.
		ComparePCADtoALPHA(board)

		ComparePCADtoQB(board)

		CompareALPHAtoQB(board)

		'Check to see if we did not add anything to the tables.
		'This means that we had no differences with the compares.
		If DataTable_ALPHAextraPCAD.Rows.Count = 0 Then
			DataTable_ALPHAextraPCAD.Rows.Add("There are no extra Components")
		End If
		If DataTable_ALPHAmissingPCAD.Rows.Count = 0 Then
			DataTable_ALPHAmissingPCAD.Rows.Add("There are no missing Components")
		End If
		If DataTable_QBextraPCAD.Rows.Count = 0 Then
			DataTable_QBextraPCAD.Rows.Add("There are no extra Components")
		End If
		If DataTable_QBmissingPCAD.Rows.Count = 0 Then
			DataTable_QBmissingPCAD.Rows.Add("There are no missing Components")
		End If
		If DataTable_PCAD_QB_quantity.Rows.Count = 0 Then
			DataTable_PCAD_QB_quantity.Rows.Add("There are no PCAD quantity disagreements")
		End If
		If DataTable_QB_PCAD_quantity.Rows.Count = 0 Then
			DataTable_QB_PCAD_quantity.Rows.Add("There are no QB quantity disagreements")
		End If
		If DataTable_QBmissingALPHA.Rows.Count = 0 Then
			DataTable_QBmissingALPHA.Rows.Add("There are no missing Components")
		End If
		If DataTable_QBextraALPHA.Rows.Count = 0 Then
			DataTable_QBextraALPHA.Rows.Add("There are no extra Components")
		End If
		If DataTable_ALPHA_QB_quantity.Rows.Count = 0 Then
			DataTable_ALPHA_QB_quantity.Rows.Add("There are no ALPHA quantity disagreements")
		End If
		If DataTable_QB_ALPHA_quantity.Rows.Count = 0 Then
			DataTable_QB_ALPHA_quantity.Rows.Add("There are no QB quantity disagreements")
		End If

		'Set our DGV sources.
		DGV_ALPHA_Missing_PCAD.DataSource = DataTable_ALPHAmissingPCAD
		DGV_ALPHA_Extra_PCAD.DataSource = DataTable_ALPHAextraPCAD

		DGV_QB_Missing_PCAD.DataSource = DataTable_QBmissingPCAD
		DGV_QB_Extra_PCAD.DataSource = DataTable_QBextraPCAD

		DGV_QB_Missing_ALPHA.DataSource = DataTable_QBmissingALPHA
		DGV_QB_Extra_ALPHA.DataSource = DataTable_QBextraALPHA

		DGV_PCAD_QB_Quantity.DataSource = DataTable_PCAD_QB_quantity
		DGV_QB_PCAD_Quantity.DataSource = DataTable_QB_PCAD_quantity
		DGV_ALPHA_QB_Quantity.DataSource = DataTable_ALPHA_QB_quantity
		DGV_QB_ALPHA_Quantity.DataSource = DataTable_QB_ALPHA_quantity

		Excel_Button.Enabled = True
	End Sub

	Private Sub ComparePCADtoALPHA(ByRef board As String)
		'First we need to check to see if we have an entry in the database for the board that we are comparing.
		If sqlapi.FindFile(myCmd, "SELECT [" & DB_HEADER_BOARD_NAME & "] FROM " & TABLE_ALPHABOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "'") = True Then
			Dim cmd As New SqlCommand("", myConn)

			cmd.CommandText = "SELECT * FROM " & TABLE_PCADBOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "' AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_SMT & "' AND [" & DB_HEADER_REF_DES & "] != '" & REFERENCE_DESIGNATOR_OPTION & "' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]"
			Dim PCAD_BOM As New DataTable()
			PCAD_BOM.Load(cmd.ExecuteReader())

			cmd.CommandText = "SELECT * FROM " & TABLE_ALPHABOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "' AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_SMT & "' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]"
			Dim ALPHA_BOM As New DataTable()
			ALPHA_BOM.Load(cmd.ExecuteReader())

			'------------------------------------'
			'Compare ALPHA with our PCAD record. '
			'------------------------------------'
			For Each dsRow As DataRow In PCAD_BOM.Rows
				'Check to see if the Stock Nubmer exists in the ALPHA BOM.
				Dim dr() As DataRow = ALPHA_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "' AND [" & DB_HEADER_REF_DES & "] = '" & dsRow(DB_HEADER_REF_DES) & "'")
				If dr.Length = 0 Then
					DataTable_ALPHAmissingPCAD.Rows.Add(dsRow(DB_HEADER_REF_DES), dsRow(DB_HEADER_ITEM_NUMBER), dsRow(DB_HEADER_VENDOR), dsRow(DB_HEADER_MPN))
				End If
			Next

			'------------------------------------'
			'Compare PCAD with our ALPHA record. '
			'------------------------------------'
			For Each dsRow As DataRow In ALPHA_BOM.Rows
				'Check to see if the Stock Number exists in the PCAD BOM.
				Dim dr() As DataRow = PCAD_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "' AND [" & DB_HEADER_REF_DES & "] = '" & dsRow(DB_HEADER_REF_DES) & "'")
				If dr.Length = 0 Then
					'Check to see if we have the item inside of QB.
					'Item Number is not in PCAD File. Check to see if the Item is in our QB List.
					Dim Item_Results() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
					If Item_Results.Length = 0 Then
						'Item Number is not in the database.
						DataTable_ALPHAextraPCAD.Rows.Add(dsRow(DB_HEADER_REF_DES), dsRow(DB_HEADER_ITEM_NUMBER), NOT_IN_DATABASE, NOT_IN_DATABASE)
					Else
						DataTable_ALPHAextraPCAD.Rows.Add(dsRow(DB_HEADER_REF_DES), dsRow(DB_HEADER_ITEM_NUMBER), Item_Results(0)(DB_HEADER_VENDOR), Item_Results(0)(DB_HEADER_MPN))
					End If
				End If
			Next
		Else
			DataTable_ALPHAmissingPCAD.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
			DataTable_ALPHAextraPCAD.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
		End If
	End Sub

	Private Sub ComparePCADtoQB(ByRef board As String)
		Dim canContinue As Boolean = True

		If sqlapi.FindFile(myCmd, "SELECT [" & DB_HEADER_NAME & "] FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] = '" & board & "'") = False Then
			canContinue = False
		End If

		'First we need to check to see if we have an entry in the database for the board that we are comparing.
		If canContinue = True Then

			'Create and add our query into our data set for comparison.
			myCmd.CommandText = ("SELECT * FROM " & TABLE_PCADBOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "' AND [" & DB_HEADER_PROCESS & "] != '" & PROCESS_NOTUSED & "' AND [" & DB_HEADER_REF_DES & "] NOT LIKE '" & REFERENCE_DESIGNATOR_OPTION & "%' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]")
			Dim PCAD_BOM As New DataTable()
			PCAD_BOM.Load(myCmd.ExecuteReader())

			Dim QB_BOM As New DataTable()
			myCmd.CommandText = ("SELECT * FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] = '" & board & "' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]")
			QB_BOM.Load(myCmd.ExecuteReader())

			'---------------------------------'
			'Compare QB with our PCAD record. '
			'---------------------------------'
			For Each dsRow As DataRow In PCAD_BOM.Rows

				'Check to see if we are a POST ASSEBLY part AND our prefix is not HRD.
				'This is inside our PCAD Table. The BAS file for QB contains the HRD but not the CON.
				If dsRow(DB_HEADER_PROCESS).contains(PROCESS_POSTASSEMBLY) = True Then
					If dsRow(DB_HEADER_ITEM_PREFIX).contains("HRD") = False Then
						Continue For
					End If
				End If

				'Check to see if the Stock Nubmer exists in the QB BOM.
				Dim dr() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "' AND [" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX) & "'")
				If dr.Length = 0 Then
					DataTable_QBmissingPCAD.Rows.Add(dsRow(DB_HEADER_REF_DES), dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_PREFIX) & ":" & dsRow(DB_HEADER_ITEM_NUMBER), dsRow(DB_HEADER_DESCRIPTION), dsRow(DB_HEADER_VENDOR), dsRow(DB_HEADER_MPN), dsRow(DB_HEADER_PROCESS))
				End If

				'Check our quantity differneces.
				Dim PCADrows() As DataRow = PCAD_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBrows() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBquantity As Integer = 0
				Try
					QBquantity = QBrows(0)(DB_HEADER_QUANTITY)
				Catch ex As Exception
					'Do nothing
				End Try

				If PCADrows.Length <> QBquantity Then
					If DataTable_PCAD_QB_quantity.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'").Length = 0 Then
						DataTable_PCAD_QB_quantity.Rows.Add(dsRow(DB_HEADER_ITEM_NUMBER), PCADrows.Length, QBquantity)
					End If
				End If
			Next

			'---------------------------------'
			'Compare PCAD with our QB record. '
			'---------------------------------'
			For Each dsRow As DataRow In QB_BOM.Rows
				If dsRow(DB_HEADER_ITEM_PREFIX).contains(PREFIX_SMA) = True Or dsRow(DB_HEADER_ITEM_PREFIX).contains(PREFIX_BIS) = True Then
					Continue For
				End If

				'Check to see if the Stock Nubmer exists in the QB BOM.
				Dim dr() As DataRow = PCAD_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "' AND [" & DB_HEADER_ITEM_PREFIX & "] = '" & dsRow(DB_HEADER_ITEM_PREFIX) & "'")
				If dr.Length = 0 Then
					'Get the process that it is attached to.
					Dim process As String = ""
					process = dsRow(DB_HEADER_PROCESS)

					'Check to see if we have the item inside of QB.
					'Item Number is not in PCAD File. Check to see if the Item is in our QB List.
					Dim Item_Results() As DataRow = QBItemList.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
					If Item_Results.Length = 0 Then
						'Item Number is not in the database.
						DataTable_QBextraPCAD.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_PREFIX) & ":" & dsRow(DB_HEADER_ITEM_NUMBER), NOT_IN_DATABASE, NOT_IN_DATABASE, NOT_IN_DATABASE, process)
					Else
						DataTable_QBextraPCAD.Rows.Add(dsRow(DB_HEADER_ITEM_PREFIX), dsRow(DB_HEADER_ITEM_PREFIX) & ":" & dsRow(DB_HEADER_ITEM_NUMBER), Item_Results(0)(DB_HEADER_DESCRIPTION), Item_Results(0)(DB_HEADER_VENDOR), Item_Results(0)(DB_HEADER_MPN), process)
					End If
				End If

				'Check our quantity differneces.
				Dim PCADquantity() As DataRow = PCAD_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBrows() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBquantity As Integer = 0
				Try
					QBquantity = QBrows(0)(DB_HEADER_QUANTITY)
				Catch ex As Exception
					'Do nothing
				End Try

				If PCADquantity.Length <> QBquantity Then
					If DataTable_QB_PCAD_quantity.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'").Length = 0 Then
						DataTable_QB_PCAD_quantity.Rows.Add(dsRow(DB_HEADER_ITEM_NUMBER), QBquantity, PCADquantity.Length)
					End If
				End If
			Next
		Else
			DataTable_QBmissingPCAD.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
			DataTable_QBextraPCAD.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
			DataTable_PCAD_QB_quantity.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
			DataTable_QB_PCAD_quantity.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
		End If
	End Sub

	Private Sub CompareALPHAtoQB(ByRef board As String)
		Dim haveAlphaBOM As Boolean = sqlapi.FindFile(myCmd, "SELECT [" & DB_HEADER_BOARD_NAME & "] FROM " & TABLE_ALPHABOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "'")
		Dim haveQBBOM As Boolean = sqlapi.FindFile(myCmd, "SELECT [" & DB_HEADER_NAME & "] FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] = '" & board & "'")

		'First we need to check to see if we have an entry in the database for the board that we are comparing.
		If haveAlphaBOM = True And haveQBBOM = True Then

			'Create and add our query into our data set for comparison.
			myCmd.CommandText = "SELECT * FROM " & TABLE_ALPHABOM & " WHERE [" & DB_HEADER_BOARD_NAME & "] = '" & board & "' AND [" & DB_HEADER_PROCESS & "] = '" & PROCESS_SMT & "' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]"
			Dim ALPHA_BOM As New DataTable()
			ALPHA_BOM.Load(myCmd.ExecuteReader())

			Dim QB_BOM As New DataTable()
			myCmd.CommandText = ("SELECT * FROM " & TABLE_QBBOM & " WHERE [" & DB_HEADER_NAME & "] LIKE '%" & board & "%' AND [" & DB_HEADER_NAME_PREFIX & "] = '" & PREFIX_SMA & "' ORDER BY [" & DB_HEADER_ITEM_NUMBER & "]")
			QB_BOM.Load(myCmd.ExecuteReader())

			'----------------------------------'
			'Compare QB with our ALPHA record. '
			'----------------------------------'
			For Each dsRow As DataRow In ALPHA_BOM.Rows
				'Check to see if the Stock Number exists in the PCAD BOM.
				Dim dr() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				If dr.Length = 0 Then
					DataTable_QBmissingALPHA.Rows.Add(dsRow(DB_HEADER_REF_DES), dsRow(DB_HEADER_ITEM_NUMBER))
				End If

				'Check our quantity differneces.
				Dim ALPHAquantity() As DataRow = ALPHA_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBrows() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBquantity As Integer = 0
				Try
					QBquantity = QBrows(0)(DB_HEADER_QUANTITY)
				Catch ex As Exception
					'Do nothing
				End Try

				If ALPHAquantity.Length <> QBquantity Then
					If DataTable_ALPHA_QB_quantity.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'").Length = 0 Then
						DataTable_ALPHA_QB_quantity.Rows.Add(dsRow(DB_HEADER_ITEM_NUMBER), ALPHAquantity.Length, QBquantity)
					End If
				End If
			Next

			'----------------------------------'
			'Compare ALPHA with our QB record. '
			'----------------------------------'
			For Each dsRow As DataRow In QB_BOM.Rows
				If dsRow(DB_HEADER_ITEM_PREFIX).contains(PREFIX_PCB) = True Then
					Continue For
				End If

				'Check to see if the Stock Nubmer exists in the QB BOM.
				Dim dr() As DataRow = ALPHA_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				If dr.Length = 0 Then
					DataTable_QBextraALPHA.Rows.Add(dsRow(DB_HEADER_ITEM_NUMBER))
				End If

				'Check our quantity differneces.
				Dim ALPHAquantity() As DataRow = ALPHA_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBrows() As DataRow = QB_BOM.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'")
				Dim QBquantity As Integer = 0
				Try
					QBquantity = QBrows(0)(DB_HEADER_QUANTITY)
				Catch ex As Exception
					'Do nothing
				End Try

				If ALPHAquantity.Length <> QBquantity Then
					If DataTable_QB_ALPHA_quantity.Select("[" & DB_HEADER_ITEM_NUMBER & "] = '" & dsRow(DB_HEADER_ITEM_NUMBER) & "'").Length = 0 Then
						DataTable_QB_ALPHA_quantity.Rows.Add(dsRow(DB_HEADER_ITEM_NUMBER), QBquantity, ALPHAquantity.Length)
					End If
				End If
			Next
		Else
			'Ckeck for which file is missing.
			If haveAlphaBOM = False Then
				DataTable_QBmissingALPHA.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
				DataTable_QBextraALPHA.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
				DataTable_ALPHA_QB_quantity.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
				DataTable_QB_ALPHA_quantity.Rows.Add("ERROR: MISSING [" & board & "] ALPHA FILE")
			End If
			If haveQBBOM = False Then
				DataTable_QBmissingALPHA.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
				DataTable_QBextraALPHA.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
				DataTable_ALPHA_QB_quantity.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
				DataTable_QB_ALPHA_quantity.Rows.Add("ERROR: MISSING [" & board & "] QB FILE")
			End If
		End If
	End Sub

	Private Sub SetupDataTables()
		'PCAD <-> ALPHA
		DataTable_ALPHAmissingPCAD.Columns.Add(DB_HEADER_REF_DES, GetType(String))
		DataTable_ALPHAmissingPCAD.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_ALPHAmissingPCAD.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		DataTable_ALPHAmissingPCAD.Columns.Add(DB_HEADER_MPN, GetType(String))

		DataTable_ALPHAextraPCAD.Columns.Add(DB_HEADER_REF_DES, GetType(String))
		DataTable_ALPHAextraPCAD.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_ALPHAextraPCAD.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		DataTable_ALPHAextraPCAD.Columns.Add(DB_HEADER_MPN, GetType(String))

		'PCAD <-> QB
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_REF_DES, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_ITEM_PREFIX, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_VALUE, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_MPN, GetType(String))
		DataTable_QBmissingPCAD.Columns.Add(DB_HEADER_PROCESS, GetType(String))

		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_ITEM_PREFIX, GetType(String))
		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_DESCRIPTION, GetType(String))
		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_VENDOR, GetType(String))
		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_MPN, GetType(String))
		DataTable_QBextraPCAD.Columns.Add(DB_HEADER_PROCESS, GetType(String))

		'ALPHA <-> QB
		DataTable_QBmissingALPHA.Columns.Add(DB_HEADER_REF_DES, GetType(String))
		DataTable_QBmissingALPHA.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))

		DataTable_QBextraALPHA.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))

		'PCAD quantity -> QB
		DataTable_PCAD_QB_quantity.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_PCAD_QB_quantity.Columns.Add(HEADER_QTY_PCAD, GetType(String))
		DataTable_PCAD_QB_quantity.Columns.Add(HEADER_QTY_QB, GetType(String))

		DataTable_QB_PCAD_quantity.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_QB_PCAD_quantity.Columns.Add(HEADER_QTY_QB, GetType(String))
		DataTable_QB_PCAD_quantity.Columns.Add(HEADER_QTY_PCAD, GetType(String))

		DataTable_ALPHA_QB_quantity.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_ALPHA_QB_quantity.Columns.Add(HEADER_QTY_ALPHA, GetType(String))
		DataTable_ALPHA_QB_quantity.Columns.Add(HEADER_QTY_QB, GetType(String))

		DataTable_QB_ALPHA_quantity.Columns.Add(DB_HEADER_ITEM_NUMBER, GetType(String))
		DataTable_QB_ALPHA_quantity.Columns.Add(HEADER_QTY_QB, GetType(String))
		DataTable_QB_ALPHA_quantity.Columns.Add(HEADER_QTY_ALPHA, GetType(String))
	End Sub

	Private Sub Close_Button_Click() Handles Close_Button.Click
		Close()
	End Sub

	Private Sub Excel_Button_Click() Handles Excel_Button.Click
		Dim report As New GenerateReport()
		report.CompareAllReport(Board_ComboBox.Text, DataTable_ALPHAmissingPCAD, DataTable_ALPHAextraPCAD,
								 DataTable_QBmissingPCAD, DataTable_QBextraPCAD, DataTable_QBmissingALPHA,
								 DataTable_QBextraALPHA, DataTable_PCAD_QB_quantity, DataTable_QB_PCAD_quantity,
								 DataTable_ALPHA_QB_quantity, DataTable_QB_ALPHA_quantity)
	End Sub

	Sub GetBoardDropDownItems(ByRef box As ComboBox)
		Dim BoardNames As New DataTable()
		myCmd.CommandText = "SELECT Distinct([" & DB_HEADER_BOARD_NAME & "]) FROM " & TABLE_PCADBOM & " ORDER BY [" & DB_HEADER_BOARD_NAME & "]"

		BoardNames.Load(myCmd.ExecuteReader)

		For Each dr As DataRow In BoardNames.Rows
			box.Items.Add(dr(DB_HEADER_BOARD_NAME))
		Next
	End Sub

	Private Sub Board_ComboBox_SelectedValueChanged() Handles Board_ComboBox.SelectedValueChanged
		Excel_Button.Enabled = False
	End Sub

	Private Sub CompareAll_Resize() Handles Me.Resize
		ResizeTables(TabControl1.SelectedTab)
	End Sub

	Public Sub ResizeTables(ByRef Tab As TabPage)
		'Math done to get all of the datagrids on each of the tabs centered. All hard-coded numbers are fine adjustment buffers.
		Dim topAndBottomPadding As Integer = 36      'Both top and bottom of the table get about 18 px of padding.
		Dim leftAndRightPadding As Integer = 16      'Both left and right of the table get about 8 px of padding.
		Dim labelYLocation As Integer = 3
		Dim DGVYLocation As Integer = 26

		Dim newWidth As Integer = Tab.Width / 2
		Dim newHeigth As Integer = Tab.Height / 2

		'Tab Page 1
		L_ALPHA_PCAD.Location = New Point(newWidth + 3, labelYLocation)
		DGV_ALPHA_Extra_PCAD.Location = New Point(newWidth, DGVYLocation)
		DGV_ALPHA_Extra_PCAD.Width = newWidth - leftAndRightPadding
		DGV_ALPHA_Extra_PCAD.Height = Tab.Height - topAndBottomPadding

		DGV_ALPHA_Missing_PCAD.Width = newWidth - leftAndRightPadding
		DGV_ALPHA_Missing_PCAD.Height = Tab.Height - topAndBottomPadding

		'Tab Page 2
		L_QB_PCAD.Location = New Point(newWidth + 3, labelYLocation)
		DGV_QB_Extra_PCAD.Location = New Point(newWidth, DGVYLocation)
		DGV_QB_Extra_PCAD.Width = newWidth - leftAndRightPadding
		DGV_QB_Extra_PCAD.Height = Tab.Height - topAndBottomPadding

		DGV_QB_Missing_PCAD.Width = newWidth - leftAndRightPadding
		DGV_QB_Missing_PCAD.Height = Tab.Height - topAndBottomPadding

		'Tab Page 3
		L_QB_ALPHA.Location = New Point(newWidth + 3, labelYLocation)
		DGV_QB_Extra_ALPHA.Location = New Point(newWidth, DGVYLocation)
		DGV_QB_Extra_ALPHA.Width = newWidth - leftAndRightPadding
		DGV_QB_Extra_ALPHA.Height = Tab.Height - topAndBottomPadding

		DGV_QB_Missing_ALPHA.Width = newWidth - leftAndRightPadding
		DGV_QB_Missing_ALPHA.Height = Tab.Height - topAndBottomPadding

		'Tab Page 4
		'Top Right table.
		L_QB_PCAD_Q.Location = New Point(newWidth + 3, labelYLocation)
		DGV_QB_PCAD_Quantity.Location = New Point(newWidth, DGVYLocation)

		'Bottom Left table.
		L_ALPHA_QB_Q.Location = New Point(L_ALPHA_QB_Q.Location.X, newHeigth)
		DGV_ALPHA_QB_Quantity.Location = New Point(10, L_ALPHA_QB_Q.Location.Y + DGVYLocation)

		'Bottom Right table.
		L_QB_ALPHA_Q.Location = New Point(newWidth + 3, newHeigth)
		DGV_QB_ALPHA_Quantity.Location = New Point(newWidth, L_QB_ALPHA_Q.Location.Y + DGVYLocation)

		DGV_PCAD_QB_Quantity.Width = newWidth - leftAndRightPadding
		DGV_QB_PCAD_Quantity.Width = DGV_PCAD_QB_Quantity.Width
		DGV_ALPHA_QB_Quantity.Width = DGV_PCAD_QB_Quantity.Width
		DGV_QB_ALPHA_Quantity.Width = DGV_PCAD_QB_Quantity.Width

		DGV_PCAD_QB_Quantity.Height = L_QB_ALPHA_Q.Location.Y - DGV_QB_Extra_ALPHA.Location.Y - 10
		DGV_QB_PCAD_Quantity.Height = DGV_PCAD_QB_Quantity.Height
		DGV_ALPHA_QB_Quantity.Height = DGV_PCAD_QB_Quantity.Height
		DGV_QB_ALPHA_Quantity.Height = DGV_PCAD_QB_Quantity.Height

		TabControl1.Refresh()
	End Sub

End Class