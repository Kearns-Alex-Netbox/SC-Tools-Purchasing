<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Printing
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
		Me.Label2 = New System.Windows.Forms.Label()
		Me.InvalidItemNumbers_ListBox = New System.Windows.Forms.ListBox()
		Me.PrintCover_Button = New System.Windows.Forms.Button()
		Me.AddStockNumber_TextBox = New System.Windows.Forms.TextBox()
		Me.Add_Button = New System.Windows.Forms.Button()
		Me.Remove_Button = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.ItemNumbers_ListBox = New System.Windows.Forms.ListBox()
		Me.PrintLabel_Button = New System.Windows.Forms.Button()
		Me.Close_Button = New System.Windows.Forms.Button()
		Me.Label_ComboBox = New System.Windows.Forms.ComboBox()
		Me.SuspendLayout()
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(10, 62)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(134, 20)
		Me.Label2.TabIndex = 40
		Me.Label2.Text = "Add Item Number"
		'
		'InvalidItemNumbers_ListBox
		'
		Me.InvalidItemNumbers_ListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.InvalidItemNumbers_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.InvalidItemNumbers_ListBox.FormattingEnabled = True
		Me.InvalidItemNumbers_ListBox.ItemHeight = 20
		Me.InvalidItemNumbers_ListBox.Location = New System.Drawing.Point(312, 155)
		Me.InvalidItemNumbers_ListBox.Name = "InvalidItemNumbers_ListBox"
		Me.InvalidItemNumbers_ListBox.Size = New System.Drawing.Size(296, 304)
		Me.InvalidItemNumbers_ListBox.TabIndex = 39
		'
		'PrintCover_Button
		'
		Me.PrintCover_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.PrintCover_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PrintCover_Button.Location = New System.Drawing.Point(511, 117)
		Me.PrintCover_Button.Name = "PrintCover_Button"
		Me.PrintCover_Button.Size = New System.Drawing.Size(94, 30)
		Me.PrintCover_Button.TabIndex = 38
		Me.PrintCover_Button.Text = "Print Cover"
		Me.PrintCover_Button.UseVisualStyleBackColor = True
		'
		'AddStockNumber_TextBox
		'
		Me.AddStockNumber_TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AddStockNumber_TextBox.Location = New System.Drawing.Point(10, 85)
		Me.AddStockNumber_TextBox.Name = "AddStockNumber_TextBox"
		Me.AddStockNumber_TextBox.Size = New System.Drawing.Size(595, 26)
		Me.AddStockNumber_TextBox.TabIndex = 37
		'
		'Add_Button
		'
		Me.Add_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Add_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Add_Button.Location = New System.Drawing.Point(10, 117)
		Me.Add_Button.Name = "Add_Button"
		Me.Add_Button.Size = New System.Drawing.Size(72, 30)
		Me.Add_Button.TabIndex = 35
		Me.Add_Button.Text = "Add"
		Me.Add_Button.UseVisualStyleBackColor = True
		'
		'Remove_Button
		'
		Me.Remove_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Remove_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Remove_Button.Location = New System.Drawing.Point(88, 117)
		Me.Remove_Button.Name = "Remove_Button"
		Me.Remove_Button.Size = New System.Drawing.Size(94, 30)
		Me.Remove_Button.TabIndex = 36
		Me.Remove_Button.Text = "Remove"
		Me.Remove_Button.UseVisualStyleBackColor = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(10, 10)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(118, 20)
		Me.Label1.TabIndex = 32
		Me.Label1.Text = "Label Template"
		'
		'ItemNumbers_ListBox
		'
		Me.ItemNumbers_ListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.ItemNumbers_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ItemNumbers_ListBox.FormattingEnabled = True
		Me.ItemNumbers_ListBox.ItemHeight = 20
		Me.ItemNumbers_ListBox.Location = New System.Drawing.Point(10, 155)
		Me.ItemNumbers_ListBox.Name = "ItemNumbers_ListBox"
		Me.ItemNumbers_ListBox.Size = New System.Drawing.Size(296, 304)
		Me.ItemNumbers_ListBox.TabIndex = 31
		'
		'PrintLabel_Button
		'
		Me.PrintLabel_Button.AutoSize = True
		Me.PrintLabel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.PrintLabel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PrintLabel_Button.Location = New System.Drawing.Point(411, 117)
		Me.PrintLabel_Button.Name = "PrintLabel_Button"
		Me.PrintLabel_Button.Size = New System.Drawing.Size(94, 30)
		Me.PrintLabel_Button.TabIndex = 30
		Me.PrintLabel_Button.Text = "Print Label"
		Me.PrintLabel_Button.UseVisualStyleBackColor = True
		'
		'Close_Button
		'
		Me.Close_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Close_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Close_Button.Location = New System.Drawing.Point(511, 31)
		Me.Close_Button.Name = "Close_Button"
		Me.Close_Button.Size = New System.Drawing.Size(94, 30)
		Me.Close_Button.TabIndex = 29
		Me.Close_Button.Text = "Close"
		Me.Close_Button.UseVisualStyleBackColor = True
		'
		'Label_ComboBox
		'
		Me.Label_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.Label_ComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label_ComboBox.FormattingEnabled = True
		Me.Label_ComboBox.Location = New System.Drawing.Point(12, 33)
		Me.Label_ComboBox.Name = "Label_ComboBox"
		Me.Label_ComboBox.Size = New System.Drawing.Size(294, 28)
		Me.Label_ComboBox.TabIndex = 41
		'
		'Printing
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(619, 469)
		Me.Controls.Add(Me.Label_ComboBox)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.InvalidItemNumbers_ListBox)
		Me.Controls.Add(Me.PrintCover_Button)
		Me.Controls.Add(Me.AddStockNumber_TextBox)
		Me.Controls.Add(Me.Add_Button)
		Me.Controls.Add(Me.Remove_Button)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.ItemNumbers_ListBox)
		Me.Controls.Add(Me.PrintLabel_Button)
		Me.Controls.Add(Me.Close_Button)
		Me.Name = "Printing"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Printing Label"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents Label2 As Label
	Friend WithEvents InvalidItemNumbers_ListBox As ListBox
	Friend WithEvents PrintCover_Button As Button
	Friend WithEvents AddStockNumber_TextBox As TextBox
	Friend WithEvents Add_Button As Button
	Friend WithEvents Remove_Button As Button
	Friend WithEvents Label1 As Label
	Friend WithEvents ItemNumbers_ListBox As ListBox
	Friend WithEvents PrintLabel_Button As Button
	Friend WithEvents Close_Button As Button
	Friend WithEvents Label_ComboBox As ComboBox
End Class
