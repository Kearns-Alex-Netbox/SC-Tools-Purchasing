'-----------------------------------------------------------------------------------------------------------------------------------------
' Module: MessageboxQuantity.vb
'
' Description: Custom message window that askes for a quantity and then returns it.
'
' Special Keys:
'   enter = confirms the quantity and trys to return it. Will not work if there is not a positive number.
'-----------------------------------------------------------------------------------------------------------------------------------------

Public Class MessageboxQuantity

	Private Sub MessageboxQuantity_Load() Handles MyBase.Load
		CenterToParent()
		KeyPreview = True
	End Sub

	Private Sub B_Confirm_Click() Handles B_Confirm.Click
		Try
			Dim quantity As Integer = TB_Quantity.Text

			DialogResult = DialogResult.Yes
			Close()
		Catch ex As Exception
			MsgBox("Please put in a positive number.")
		End Try
	End Sub

	Private Sub B_Cancel_Click() Handles B_Cancel.Click
		DialogResult = DialogResult.Cancel
		Close()
	End Sub

	Private Sub MyBase_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
		'Call the confirm Function when we press the Enter key.
		If e.KeyCode.Equals(Keys.Enter) Then
			Call B_Confirm_Click()
		End If
	End Sub

End Class