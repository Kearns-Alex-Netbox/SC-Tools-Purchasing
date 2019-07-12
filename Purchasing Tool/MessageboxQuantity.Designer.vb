<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MessageboxQuantity
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.B_Confirm = New System.Windows.Forms.Button()
		Me.B_Cancel = New System.Windows.Forms.Button()
		Me.TB_Quantity = New System.Windows.Forms.TextBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'B_Confirm
		'
		Me.B_Confirm.AutoSize = True
		Me.B_Confirm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_Confirm.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.B_Confirm.Location = New System.Drawing.Point(12, 66)
		Me.B_Confirm.Name = "B_Confirm"
		Me.B_Confirm.Size = New System.Drawing.Size(74, 30)
		Me.B_Confirm.TabIndex = 6
		Me.B_Confirm.Text = "Confirm"
		Me.B_Confirm.UseVisualStyleBackColor = True
		'
		'B_Cancel
		'
		Me.B_Cancel.AutoSize = True
		Me.B_Cancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_Cancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.B_Cancel.Location = New System.Drawing.Point(92, 66)
		Me.B_Cancel.Name = "B_Cancel"
		Me.B_Cancel.Size = New System.Drawing.Size(68, 30)
		Me.B_Cancel.TabIndex = 7
		Me.B_Cancel.Text = "Cancel"
		Me.B_Cancel.UseVisualStyleBackColor = True
		'
		'TB_Quantity
		'
		Me.TB_Quantity.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TB_Quantity.Location = New System.Drawing.Point(12, 34)
		Me.TB_Quantity.Name = "TB_Quantity"
		Me.TB_Quantity.Size = New System.Drawing.Size(148, 26)
		Me.TB_Quantity.TabIndex = 5
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(52, 11)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(68, 20)
		Me.Label1.TabIndex = 4
		Me.Label1.Text = "Quantity"
		'
		'MessageboxQuantity
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(172, 106)
		Me.Controls.Add(Me.B_Confirm)
		Me.Controls.Add(Me.B_Cancel)
		Me.Controls.Add(Me.TB_Quantity)
		Me.Controls.Add(Me.Label1)
		Me.Name = "MessageboxQuantity"
		Me.Text = "Quantity"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents B_Confirm As Button
	Friend WithEvents B_Cancel As Button
	Friend WithEvents TB_Quantity As TextBox
	Friend WithEvents Label1 As Label
End Class
