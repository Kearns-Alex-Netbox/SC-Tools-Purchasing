<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CriticalBuildPath
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
		Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.AllVendors_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.BuildProducts_Button = New System.Windows.Forms.Button()
		Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
		Me.OrderSoon_Label = New System.Windows.Forms.Label()
		Me.Database_Label = New System.Windows.Forms.Label()
		Me.OutofAssembly_Label = New System.Windows.Forms.Label()
		Me.StopLevel_TextBox = New System.Windows.Forms.TextBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.SourceLevel_ComboBox = New System.Windows.Forms.ComboBox()
		Me.Add_Button = New System.Windows.Forms.Button()
		Me.Clear_Button = New System.Windows.Forms.Button()
		Me.Remove_Button = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.BuildProducts_ListBox = New System.Windows.Forms.ListBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Products_ListBox = New System.Windows.Forms.ListBox()
		Me.UseInventory_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Results_DGV = New System.Windows.Forms.DataGridView()
		Me.GenerateReport_Button = New System.Windows.Forms.Button()
		Me.Excel_Button = New System.Windows.Forms.Button()
		Me.Close_Button = New System.Windows.Forms.Button()
		CType(Me.Results_DGV, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'AllVendors_CheckBox
		'
		Me.AllVendors_CheckBox.AutoSize = True
		Me.AllVendors_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AllVendors_CheckBox.Location = New System.Drawing.Point(450, 17)
		Me.AllVendors_CheckBox.Name = "AllVendors_CheckBox"
		Me.AllVendors_CheckBox.Size = New System.Drawing.Size(109, 24)
		Me.AllVendors_CheckBox.TabIndex = 68
		Me.AllVendors_CheckBox.Text = "All Vendors"
		Me.AllVendors_CheckBox.UseVisualStyleBackColor = True
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Font = New System.Drawing.Font("Consolas", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(12, 10)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(180, 19)
		Me.Label3.TabIndex = 67
		Me.Label3.Text = "Critical Build Path"
		'
		'BuildProducts_Button
		'
		Me.BuildProducts_Button.AutoSize = True
		Me.BuildProducts_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.BuildProducts_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.BuildProducts_Button.Location = New System.Drawing.Point(114, 571)
		Me.BuildProducts_Button.Name = "BuildProducts_Button"
		Me.BuildProducts_Button.Size = New System.Drawing.Size(121, 30)
		Me.BuildProducts_Button.TabIndex = 66
		Me.BuildProducts_Button.Text = "Build Products"
		Me.BuildProducts_Button.UseVisualStyleBackColor = True
		'
		'ProgressBar1
		'
		Me.ProgressBar1.Location = New System.Drawing.Point(588, 17)
		Me.ProgressBar1.Name = "ProgressBar1"
		Me.ProgressBar1.Size = New System.Drawing.Size(186, 23)
		Me.ProgressBar1.TabIndex = 65
		Me.ProgressBar1.Visible = False
		'
		'OrderSoon_Label
		'
		Me.OrderSoon_Label.AutoSize = True
		Me.OrderSoon_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.OrderSoon_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OrderSoon_Label.Location = New System.Drawing.Point(226, 604)
		Me.OrderSoon_Label.Name = "OrderSoon_Label"
		Me.OrderSoon_Label.Size = New System.Drawing.Size(64, 20)
		Me.OrderSoon_Label.TabIndex = 64
		Me.OrderSoon_Label.Text = "Product"
		'
		'Database_Label
		'
		Me.Database_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.Database_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Database_Label.Location = New System.Drawing.Point(12, 604)
		Me.Database_Label.Name = "Database_Label"
		Me.Database_Label.Size = New System.Drawing.Size(125, 20)
		Me.Database_Label.TabIndex = 62
		Me.Database_Label.Text = "Not in Database"
		'
		'OutofAssembly_Label
		'
		Me.OutofAssembly_Label.AutoSize = True
		Me.OutofAssembly_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.OutofAssembly_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OutofAssembly_Label.Location = New System.Drawing.Point(143, 604)
		Me.OutofAssembly_Label.Name = "OutofAssembly_Label"
		Me.OutofAssembly_Label.Size = New System.Drawing.Size(77, 20)
		Me.OutofAssembly_Label.TabIndex = 63
		Me.OutofAssembly_Label.Text = "Assembly"
		'
		'StopLevel_TextBox
		'
		Me.StopLevel_TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.StopLevel_TextBox.Location = New System.Drawing.Point(131, 81)
		Me.StopLevel_TextBox.Name = "StopLevel_TextBox"
		Me.StopLevel_TextBox.Size = New System.Drawing.Size(31, 26)
		Me.StopLevel_TextBox.TabIndex = 52
		Me.StopLevel_TextBox.Text = "1"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(12, 84)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(94, 20)
		Me.Label4.TabIndex = 59
		Me.Label4.Text = "Stop Level"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(12, 50)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(113, 20)
		Me.Label1.TabIndex = 58
		Me.Label1.Text = "Source Level"
		'
		'SourceLevel_ComboBox
		'
		Me.SourceLevel_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.SourceLevel_ComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SourceLevel_ComboBox.FormattingEnabled = True
		Me.SourceLevel_ComboBox.Location = New System.Drawing.Point(131, 47)
		Me.SourceLevel_ComboBox.Name = "SourceLevel_ComboBox"
		Me.SourceLevel_ComboBox.Size = New System.Drawing.Size(181, 28)
		Me.SourceLevel_ComboBox.TabIndex = 51
		'
		'Add_Button
		'
		Me.Add_Button.AutoSize = True
		Me.Add_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Add_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Add_Button.Location = New System.Drawing.Point(16, 325)
		Me.Add_Button.Name = "Add_Button"
		Me.Add_Button.Size = New System.Drawing.Size(48, 30)
		Me.Add_Button.TabIndex = 54
		Me.Add_Button.Text = "Add"
		Me.Add_Button.UseVisualStyleBackColor = True
		'
		'Clear_Button
		'
		Me.Clear_Button.AutoSize = True
		Me.Clear_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Clear_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Clear_Button.Location = New System.Drawing.Point(255, 571)
		Me.Clear_Button.Name = "Clear_Button"
		Me.Clear_Button.Size = New System.Drawing.Size(56, 30)
		Me.Clear_Button.TabIndex = 57
		Me.Clear_Button.Text = "Clear"
		Me.Clear_Button.UseVisualStyleBackColor = True
		'
		'Remove_Button
		'
		Me.Remove_Button.AutoSize = True
		Me.Remove_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Remove_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Remove_Button.Location = New System.Drawing.Point(16, 571)
		Me.Remove_Button.Name = "Remove_Button"
		Me.Remove_Button.Size = New System.Drawing.Size(78, 30)
		Me.Remove_Button.TabIndex = 56
		Me.Remove_Button.Text = "Remove"
		Me.Remove_Button.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(12, 358)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(146, 20)
		Me.Label2.TabIndex = 61
		Me.Label2.Text = "Products to Build"
		'
		'BuildProducts_ListBox
		'
		Me.BuildProducts_ListBox.AllowDrop = True
		Me.BuildProducts_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BuildProducts_ListBox.FormattingEnabled = True
		Me.BuildProducts_ListBox.HorizontalScrollbar = True
		Me.BuildProducts_ListBox.ItemHeight = 20
		Me.BuildProducts_ListBox.Location = New System.Drawing.Point(16, 381)
		Me.BuildProducts_ListBox.Name = "BuildProducts_ListBox"
		Me.BuildProducts_ListBox.ScrollAlwaysVisible = True
		Me.BuildProducts_ListBox.Size = New System.Drawing.Size(296, 184)
		Me.BuildProducts_ListBox.TabIndex = 55
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.Location = New System.Drawing.Point(12, 112)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(230, 20)
		Me.Label5.TabIndex = 60
		Me.Label5.Text = "Products from Source Level"
		'
		'Products_ListBox
		'
		Me.Products_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Products_ListBox.FormattingEnabled = True
		Me.Products_ListBox.HorizontalScrollbar = True
		Me.Products_ListBox.ItemHeight = 20
		Me.Products_ListBox.Location = New System.Drawing.Point(16, 135)
		Me.Products_ListBox.Name = "Products_ListBox"
		Me.Products_ListBox.ScrollAlwaysVisible = True
		Me.Products_ListBox.Size = New System.Drawing.Size(296, 184)
		Me.Products_ListBox.TabIndex = 53
		'
		'UseInventory_CheckBox
		'
		Me.UseInventory_CheckBox.AutoSize = True
		Me.UseInventory_CheckBox.Checked = True
		Me.UseInventory_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.UseInventory_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.UseInventory_CheckBox.Location = New System.Drawing.Point(318, 17)
		Me.UseInventory_CheckBox.Name = "UseInventory_CheckBox"
		Me.UseInventory_CheckBox.Size = New System.Drawing.Size(126, 24)
		Me.UseInventory_CheckBox.TabIndex = 50
		Me.UseInventory_CheckBox.Text = "Use Inventory"
		Me.UseInventory_CheckBox.UseVisualStyleBackColor = True
		'
		'Results_DGV
		'
		Me.Results_DGV.AllowUserToAddRows = False
		Me.Results_DGV.AllowUserToDeleteRows = False
		DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
		Me.Results_DGV.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
		Me.Results_DGV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Results_DGV.BackgroundColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Results_DGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
		Me.Results_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Results_DGV.DefaultCellStyle = DataGridViewCellStyle3
		Me.Results_DGV.Location = New System.Drawing.Point(318, 47)
		Me.Results_DGV.Name = "Results_DGV"
		Me.Results_DGV.Size = New System.Drawing.Size(782, 579)
		Me.Results_DGV.TabIndex = 49
		'
		'GenerateReport_Button
		'
		Me.GenerateReport_Button.AutoSize = True
		Me.GenerateReport_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.GenerateReport_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.GenerateReport_Button.Location = New System.Drawing.Point(780, 13)
		Me.GenerateReport_Button.Name = "GenerateReport_Button"
		Me.GenerateReport_Button.Size = New System.Drawing.Size(140, 30)
		Me.GenerateReport_Button.TabIndex = 46
		Me.GenerateReport_Button.Text = "Generate Report"
		Me.GenerateReport_Button.UseVisualStyleBackColor = True
		'
		'Excel_Button
		'
		Me.Excel_Button.AutoSize = True
		Me.Excel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Excel_Button.Enabled = False
		Me.Excel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Excel_Button.Location = New System.Drawing.Point(926, 13)
		Me.Excel_Button.Name = "Excel_Button"
		Me.Excel_Button.Size = New System.Drawing.Size(109, 30)
		Me.Excel_Button.TabIndex = 47
		Me.Excel_Button.Text = "Create Excel"
		Me.Excel_Button.UseVisualStyleBackColor = True
		'
		'Close_Button
		'
		Me.Close_Button.AutoSize = True
		Me.Close_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Close_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Close_Button.Location = New System.Drawing.Point(1041, 13)
		Me.Close_Button.Name = "Close_Button"
		Me.Close_Button.Size = New System.Drawing.Size(59, 30)
		Me.Close_Button.TabIndex = 48
		Me.Close_Button.Text = "Close"
		Me.Close_Button.UseVisualStyleBackColor = True
		'
		'CriticalBuildPath
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1112, 637)
		Me.Controls.Add(Me.AllVendors_CheckBox)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.BuildProducts_Button)
		Me.Controls.Add(Me.ProgressBar1)
		Me.Controls.Add(Me.OrderSoon_Label)
		Me.Controls.Add(Me.Database_Label)
		Me.Controls.Add(Me.OutofAssembly_Label)
		Me.Controls.Add(Me.StopLevel_TextBox)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.SourceLevel_ComboBox)
		Me.Controls.Add(Me.Add_Button)
		Me.Controls.Add(Me.Clear_Button)
		Me.Controls.Add(Me.Remove_Button)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.BuildProducts_ListBox)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.Products_ListBox)
		Me.Controls.Add(Me.UseInventory_CheckBox)
		Me.Controls.Add(Me.Results_DGV)
		Me.Controls.Add(Me.GenerateReport_Button)
		Me.Controls.Add(Me.Excel_Button)
		Me.Controls.Add(Me.Close_Button)
		Me.Name = "CriticalBuildPath"
		Me.Text = "Critical Build Path"
		CType(Me.Results_DGV, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents AllVendors_CheckBox As CheckBox
	Friend WithEvents Label3 As Label
	Friend WithEvents BuildProducts_Button As Button
	Friend WithEvents ProgressBar1 As ProgressBar
	Friend WithEvents OrderSoon_Label As Label
	Friend WithEvents Database_Label As Label
	Friend WithEvents OutofAssembly_Label As Label
	Friend WithEvents StopLevel_TextBox As TextBox
	Friend WithEvents Label4 As Label
	Friend WithEvents Label1 As Label
	Friend WithEvents SourceLevel_ComboBox As ComboBox
	Friend WithEvents Add_Button As Button
	Friend WithEvents Clear_Button As Button
	Friend WithEvents Remove_Button As Button
	Friend WithEvents Label2 As Label
	Friend WithEvents BuildProducts_ListBox As ListBox
	Friend WithEvents Label5 As Label
	Friend WithEvents Products_ListBox As ListBox
	Friend WithEvents UseInventory_CheckBox As CheckBox
	Friend WithEvents Results_DGV As DataGridView
	Friend WithEvents GenerateReport_Button As Button
	Friend WithEvents Excel_Button As Button
	Friend WithEvents Close_Button As Button
End Class
