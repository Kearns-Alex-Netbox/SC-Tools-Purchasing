<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BuildProducts
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
		Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.AllVendors_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.CriticalBuild_Button = New System.Windows.Forms.Button()
		Me.AnualUsage_Button = New System.Windows.Forms.Button()
		Me.StopLevel_TextBox = New System.Windows.Forms.TextBox()
		Me.OrderReport_Button = New System.Windows.Forms.Button()
		Me.ShowAll_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Database_Label = New System.Windows.Forms.Label()
		Me.OrderSoon_Label = New System.Windows.Forms.Label()
		Me.OutofAssembly_Label = New System.Windows.Forms.Label()
		Me.OutofStock_Label = New System.Windows.Forms.Label()
		Me.UseInventory_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.SourceLevel_ComboBox = New System.Windows.Forms.ComboBox()
		Me.Add_Button = New System.Windows.Forms.Button()
		Me.Clear_Button = New System.Windows.Forms.Button()
		Me.Remove_Button = New System.Windows.Forms.Button()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.BuildProducts_ListBox = New System.Windows.Forms.ListBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Products_ListBox = New System.Windows.Forms.ListBox()
		Me.GenerateReport_Button = New System.Windows.Forms.Button()
		Me.Excel_Button = New System.Windows.Forms.Button()
		Me.Close_Button = New System.Windows.Forms.Button()
		Me.Results_DGV = New System.Windows.Forms.DataGridView()
		CType(Me.Results_DGV, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'AllVendors_CheckBox
		'
		Me.AllVendors_CheckBox.AutoSize = True
		Me.AllVendors_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AllVendors_CheckBox.Location = New System.Drawing.Point(545, 18)
		Me.AllVendors_CheckBox.Name = "AllVendors_CheckBox"
		Me.AllVendors_CheckBox.Size = New System.Drawing.Size(109, 24)
		Me.AllVendors_CheckBox.TabIndex = 71
		Me.AllVendors_CheckBox.Text = "All Vendors"
		Me.AllVendors_CheckBox.UseVisualStyleBackColor = True
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Font = New System.Drawing.Font("Consolas", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.Location = New System.Drawing.Point(12, 11)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(135, 19)
		Me.Label5.TabIndex = 70
		Me.Label5.Text = "Build Products"
		'
		'CriticalBuild_Button
		'
		Me.CriticalBuild_Button.AutoSize = True
		Me.CriticalBuild_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.CriticalBuild_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.CriticalBuild_Button.Location = New System.Drawing.Point(122, 574)
		Me.CriticalBuild_Button.Name = "CriticalBuild_Button"
		Me.CriticalBuild_Button.Size = New System.Drawing.Size(105, 30)
		Me.CriticalBuild_Button.TabIndex = 69
		Me.CriticalBuild_Button.Text = "Critical Build"
		Me.CriticalBuild_Button.UseVisualStyleBackColor = True
		'
		'AnualUsage_Button
		'
		Me.AnualUsage_Button.AutoSize = True
		Me.AnualUsage_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.AnualUsage_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.AnualUsage_Button.Location = New System.Drawing.Point(981, 14)
		Me.AnualUsage_Button.Name = "AnualUsage_Button"
		Me.AnualUsage_Button.Size = New System.Drawing.Size(120, 30)
		Me.AnualUsage_Button.TabIndex = 68
		Me.AnualUsage_Button.Text = "Annual Usage"
		Me.AnualUsage_Button.UseVisualStyleBackColor = True
		'
		'StopLevel_TextBox
		'
		Me.StopLevel_TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.StopLevel_TextBox.Location = New System.Drawing.Point(131, 84)
		Me.StopLevel_TextBox.Name = "StopLevel_TextBox"
		Me.StopLevel_TextBox.Size = New System.Drawing.Size(31, 26)
		Me.StopLevel_TextBox.TabIndex = 47
		Me.StopLevel_TextBox.Text = "1"
		'
		'OrderReport_Button
		'
		Me.OrderReport_Button.AutoSize = True
		Me.OrderReport_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.OrderReport_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.OrderReport_Button.Location = New System.Drawing.Point(863, 14)
		Me.OrderReport_Button.Name = "OrderReport_Button"
		Me.OrderReport_Button.Size = New System.Drawing.Size(112, 30)
		Me.OrderReport_Button.TabIndex = 56
		Me.OrderReport_Button.Text = "Order Report"
		Me.OrderReport_Button.UseVisualStyleBackColor = True
		'
		'ShowAll_CheckBox
		'
		Me.ShowAll_CheckBox.AutoSize = True
		Me.ShowAll_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ShowAll_CheckBox.Location = New System.Drawing.Point(450, 18)
		Me.ShowAll_CheckBox.Name = "ShowAll_CheckBox"
		Me.ShowAll_CheckBox.Size = New System.Drawing.Size(89, 24)
		Me.ShowAll_CheckBox.TabIndex = 54
		Me.ShowAll_CheckBox.Text = "Show All"
		Me.ShowAll_CheckBox.UseVisualStyleBackColor = True
		'
		'Database_Label
		'
		Me.Database_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.Database_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Database_Label.Location = New System.Drawing.Point(116, 607)
		Me.Database_Label.Name = "Database_Label"
		Me.Database_Label.Size = New System.Drawing.Size(125, 20)
		Me.Database_Label.TabIndex = 66
		Me.Database_Label.Text = "Not in Database"
		'
		'OrderSoon_Label
		'
		Me.OrderSoon_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.OrderSoon_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OrderSoon_Label.Location = New System.Drawing.Point(12, 632)
		Me.OrderSoon_Label.Name = "OrderSoon_Label"
		Me.OrderSoon_Label.Size = New System.Drawing.Size(98, 20)
		Me.OrderSoon_Label.TabIndex = 65
		Me.OrderSoon_Label.Text = "Order Soon"
		'
		'OutofAssembly_Label
		'
		Me.OutofAssembly_Label.AutoSize = True
		Me.OutofAssembly_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.OutofAssembly_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OutofAssembly_Label.Location = New System.Drawing.Point(116, 632)
		Me.OutofAssembly_Label.Name = "OutofAssembly_Label"
		Me.OutofAssembly_Label.Size = New System.Drawing.Size(125, 20)
		Me.OutofAssembly_Label.TabIndex = 67
		Me.OutofAssembly_Label.Text = "Out of Assembly"
		'
		'OutofStock_Label
		'
		Me.OutofStock_Label.AutoSize = True
		Me.OutofStock_Label.BackColor = System.Drawing.SystemColors.ActiveCaption
		Me.OutofStock_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.OutofStock_Label.Location = New System.Drawing.Point(12, 607)
		Me.OutofStock_Label.Name = "OutofStock_Label"
		Me.OutofStock_Label.Size = New System.Drawing.Size(98, 20)
		Me.OutofStock_Label.TabIndex = 64
		Me.OutofStock_Label.Text = "Out of Stock"
		'
		'UseInventory_CheckBox
		'
		Me.UseInventory_CheckBox.AutoSize = True
		Me.UseInventory_CheckBox.Checked = True
		Me.UseInventory_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.UseInventory_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.UseInventory_CheckBox.Location = New System.Drawing.Point(318, 18)
		Me.UseInventory_CheckBox.Name = "UseInventory_CheckBox"
		Me.UseInventory_CheckBox.Size = New System.Drawing.Size(126, 24)
		Me.UseInventory_CheckBox.TabIndex = 53
		Me.UseInventory_CheckBox.Text = "Use Inventory"
		Me.UseInventory_CheckBox.UseVisualStyleBackColor = True
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(12, 87)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(94, 20)
		Me.Label4.TabIndex = 61
		Me.Label4.Text = "Stop Level"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(12, 53)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(113, 20)
		Me.Label3.TabIndex = 60
		Me.Label3.Text = "Source Level"
		'
		'SourceLevel_ComboBox
		'
		Me.SourceLevel_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.SourceLevel_ComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SourceLevel_ComboBox.FormattingEnabled = True
		Me.SourceLevel_ComboBox.Location = New System.Drawing.Point(131, 50)
		Me.SourceLevel_ComboBox.Name = "SourceLevel_ComboBox"
		Me.SourceLevel_ComboBox.Size = New System.Drawing.Size(181, 28)
		Me.SourceLevel_ComboBox.TabIndex = 46
		'
		'Add_Button
		'
		Me.Add_Button.AutoSize = True
		Me.Add_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Add_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Add_Button.Location = New System.Drawing.Point(16, 328)
		Me.Add_Button.Name = "Add_Button"
		Me.Add_Button.Size = New System.Drawing.Size(48, 30)
		Me.Add_Button.TabIndex = 49
		Me.Add_Button.Text = "Add"
		Me.Add_Button.UseVisualStyleBackColor = True
		'
		'Clear_Button
		'
		Me.Clear_Button.AutoSize = True
		Me.Clear_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Clear_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Clear_Button.Location = New System.Drawing.Point(255, 574)
		Me.Clear_Button.Name = "Clear_Button"
		Me.Clear_Button.Size = New System.Drawing.Size(56, 30)
		Me.Clear_Button.TabIndex = 52
		Me.Clear_Button.Text = "Clear"
		Me.Clear_Button.UseVisualStyleBackColor = True
		'
		'Remove_Button
		'
		Me.Remove_Button.AutoSize = True
		Me.Remove_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Remove_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Remove_Button.Location = New System.Drawing.Point(16, 574)
		Me.Remove_Button.Name = "Remove_Button"
		Me.Remove_Button.Size = New System.Drawing.Size(78, 30)
		Me.Remove_Button.TabIndex = 51
		Me.Remove_Button.Text = "Remove"
		Me.Remove_Button.UseVisualStyleBackColor = True
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(12, 361)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(146, 20)
		Me.Label2.TabIndex = 63
		Me.Label2.Text = "Products to Build"
		'
		'BuildProducts_ListBox
		'
		Me.BuildProducts_ListBox.AllowDrop = True
		Me.BuildProducts_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BuildProducts_ListBox.FormattingEnabled = True
		Me.BuildProducts_ListBox.HorizontalScrollbar = True
		Me.BuildProducts_ListBox.ItemHeight = 20
		Me.BuildProducts_ListBox.Location = New System.Drawing.Point(16, 384)
		Me.BuildProducts_ListBox.Name = "BuildProducts_ListBox"
		Me.BuildProducts_ListBox.ScrollAlwaysVisible = True
		Me.BuildProducts_ListBox.Size = New System.Drawing.Size(296, 184)
		Me.BuildProducts_ListBox.TabIndex = 50
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(12, 115)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(230, 20)
		Me.Label1.TabIndex = 62
		Me.Label1.Text = "Products from Source Level"
		'
		'Products_ListBox
		'
		Me.Products_ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Products_ListBox.FormattingEnabled = True
		Me.Products_ListBox.HorizontalScrollbar = True
		Me.Products_ListBox.ItemHeight = 20
		Me.Products_ListBox.Location = New System.Drawing.Point(16, 138)
		Me.Products_ListBox.Name = "Products_ListBox"
		Me.Products_ListBox.ScrollAlwaysVisible = True
		Me.Products_ListBox.Size = New System.Drawing.Size(296, 184)
		Me.Products_ListBox.TabIndex = 48
		'
		'GenerateReport_Button
		'
		Me.GenerateReport_Button.AutoSize = True
		Me.GenerateReport_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.GenerateReport_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.GenerateReport_Button.Location = New System.Drawing.Point(717, 14)
		Me.GenerateReport_Button.Name = "GenerateReport_Button"
		Me.GenerateReport_Button.Size = New System.Drawing.Size(140, 30)
		Me.GenerateReport_Button.TabIndex = 55
		Me.GenerateReport_Button.Text = "Generate Report"
		Me.GenerateReport_Button.UseVisualStyleBackColor = True
		'
		'Excel_Button
		'
		Me.Excel_Button.AutoSize = True
		Me.Excel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Excel_Button.Enabled = False
		Me.Excel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Excel_Button.Location = New System.Drawing.Point(1107, 14)
		Me.Excel_Button.Name = "Excel_Button"
		Me.Excel_Button.Size = New System.Drawing.Size(109, 30)
		Me.Excel_Button.TabIndex = 57
		Me.Excel_Button.Text = "Create Excel"
		Me.Excel_Button.UseVisualStyleBackColor = True
		'
		'Close_Button
		'
		Me.Close_Button.AutoSize = True
		Me.Close_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Close_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Close_Button.Location = New System.Drawing.Point(1222, 14)
		Me.Close_Button.Name = "Close_Button"
		Me.Close_Button.Size = New System.Drawing.Size(59, 30)
		Me.Close_Button.TabIndex = 58
		Me.Close_Button.Text = "Close"
		Me.Close_Button.UseVisualStyleBackColor = True
		'
		'Results_DGV
		'
		Me.Results_DGV.AllowUserToAddRows = False
		Me.Results_DGV.AllowUserToDeleteRows = False
		DataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
		Me.Results_DGV.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
		Me.Results_DGV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Results_DGV.BackgroundColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Results_DGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle8
		Me.Results_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle9.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Results_DGV.DefaultCellStyle = DataGridViewCellStyle9
		Me.Results_DGV.Location = New System.Drawing.Point(318, 50)
		Me.Results_DGV.Name = "Results_DGV"
		Me.Results_DGV.Size = New System.Drawing.Size(963, 602)
		Me.Results_DGV.TabIndex = 59
		'
		'BuildProducts
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1293, 662)
		Me.Controls.Add(Me.AllVendors_CheckBox)
		Me.Controls.Add(Me.Label5)
		Me.Controls.Add(Me.CriticalBuild_Button)
		Me.Controls.Add(Me.AnualUsage_Button)
		Me.Controls.Add(Me.StopLevel_TextBox)
		Me.Controls.Add(Me.OrderReport_Button)
		Me.Controls.Add(Me.ShowAll_CheckBox)
		Me.Controls.Add(Me.Database_Label)
		Me.Controls.Add(Me.OrderSoon_Label)
		Me.Controls.Add(Me.OutofAssembly_Label)
		Me.Controls.Add(Me.OutofStock_Label)
		Me.Controls.Add(Me.UseInventory_CheckBox)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.SourceLevel_ComboBox)
		Me.Controls.Add(Me.Add_Button)
		Me.Controls.Add(Me.Clear_Button)
		Me.Controls.Add(Me.Remove_Button)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.BuildProducts_ListBox)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.Products_ListBox)
		Me.Controls.Add(Me.GenerateReport_Button)
		Me.Controls.Add(Me.Excel_Button)
		Me.Controls.Add(Me.Close_Button)
		Me.Controls.Add(Me.Results_DGV)
		Me.Name = "BuildProducts"
		Me.Text = "Build Products"
		CType(Me.Results_DGV, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents AllVendors_CheckBox As CheckBox
	Friend WithEvents Label5 As Label
	Friend WithEvents CriticalBuild_Button As Button
	Friend WithEvents AnualUsage_Button As Button
	Friend WithEvents StopLevel_TextBox As TextBox
	Friend WithEvents OrderReport_Button As Button
	Friend WithEvents ShowAll_CheckBox As CheckBox
	Friend WithEvents Database_Label As Label
	Friend WithEvents OrderSoon_Label As Label
	Friend WithEvents OutofAssembly_Label As Label
	Friend WithEvents OutofStock_Label As Label
	Friend WithEvents UseInventory_CheckBox As CheckBox
	Friend WithEvents Label4 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents SourceLevel_ComboBox As ComboBox
	Friend WithEvents Add_Button As Button
	Friend WithEvents Clear_Button As Button
	Friend WithEvents Remove_Button As Button
	Friend WithEvents Label2 As Label
	Friend WithEvents BuildProducts_ListBox As ListBox
	Friend WithEvents Label1 As Label
	Friend WithEvents Products_ListBox As ListBox
	Friend WithEvents GenerateReport_Button As Button
	Friend WithEvents Excel_Button As Button
	Friend WithEvents Close_Button As Button
	Friend WithEvents Results_DGV As DataGridView
End Class
