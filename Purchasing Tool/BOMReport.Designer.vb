<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BOMReport
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
		Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.BOM_DGV = New System.Windows.Forms.DataGridView()
		Me.Condense_CheckBox = New System.Windows.Forms.CheckBox()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.ReferenceDesignator_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Value_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Process_CheckBox = New System.Windows.Forms.CheckBox()
		Me.Vendor_CheckBox = New System.Windows.Forms.CheckBox()
		Me.ItemNumber_CheckBox = New System.Windows.Forms.CheckBox()
		Me.MPN_CheckBox = New System.Windows.Forms.CheckBox()
		Me.BAS_CheckBox = New System.Windows.Forms.CheckBox()
		Me.SMT_CheckBox = New System.Windows.Forms.CheckBox()
		Me.PostAssembly_CheckBox = New System.Windows.Forms.CheckBox()
		Me.PCBboard_CheckBox = New System.Windows.Forms.CheckBox()
		Me.HandFlow_CheckBox = New System.Windows.Forms.CheckBox()
		Me.GenerateReport_Button = New System.Windows.Forms.Button()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.Excel_Button = New System.Windows.Forms.Button()
		Me.Boards_ComboBox = New System.Windows.Forms.ComboBox()
		Me.Close_Button = New System.Windows.Forms.Button()
		CType(Me.BOM_DGV, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox2.SuspendLayout()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Font = New System.Drawing.Font("Consolas", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(12, 11)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(99, 19)
		Me.Label3.TabIndex = 46
		Me.Label3.Text = "BOM Report"
		'
		'BOM_DGV
		'
		Me.BOM_DGV.AllowUserToAddRows = False
		Me.BOM_DGV.AllowUserToDeleteRows = False
		Me.BOM_DGV.AllowUserToOrderColumns = True
		DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
		Me.BOM_DGV.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
		Me.BOM_DGV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.BOM_DGV.BackgroundColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.BOM_DGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
		Me.BOM_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.BOM_DGV.DefaultCellStyle = DataGridViewCellStyle3
		Me.BOM_DGV.Location = New System.Drawing.Point(226, 50)
		Me.BOM_DGV.Name = "BOM_DGV"
		DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
		Me.BOM_DGV.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
		Me.BOM_DGV.Size = New System.Drawing.Size(496, 409)
		Me.BOM_DGV.TabIndex = 45
		'
		'Condense_CheckBox
		'
		Me.Condense_CheckBox.AutoSize = True
		Me.Condense_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Condense_CheckBox.Location = New System.Drawing.Point(226, 18)
		Me.Condense_CheckBox.Name = "Condense_CheckBox"
		Me.Condense_CheckBox.Size = New System.Drawing.Size(101, 24)
		Me.Condense_CheckBox.TabIndex = 41
		Me.Condense_CheckBox.Text = "Condense"
		Me.Condense_CheckBox.UseVisualStyleBackColor = True
		'
		'GroupBox2
		'
		Me.GroupBox2.Controls.Add(Me.ReferenceDesignator_CheckBox)
		Me.GroupBox2.Controls.Add(Me.Value_CheckBox)
		Me.GroupBox2.Controls.Add(Me.Process_CheckBox)
		Me.GroupBox2.Controls.Add(Me.Vendor_CheckBox)
		Me.GroupBox2.Controls.Add(Me.ItemNumber_CheckBox)
		Me.GroupBox2.Controls.Add(Me.MPN_CheckBox)
		Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox2.Location = New System.Drawing.Point(16, 84)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(204, 205)
		Me.GroupBox2.TabIndex = 39
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Catagory"
		'
		'ReferenceDesignator_CheckBox
		'
		Me.ReferenceDesignator_CheckBox.AutoSize = True
		Me.ReferenceDesignator_CheckBox.Checked = True
		Me.ReferenceDesignator_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ReferenceDesignator_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ReferenceDesignator_CheckBox.Location = New System.Drawing.Point(6, 19)
		Me.ReferenceDesignator_CheckBox.Name = "ReferenceDesignator_CheckBox"
		Me.ReferenceDesignator_CheckBox.Size = New System.Drawing.Size(185, 24)
		Me.ReferenceDesignator_CheckBox.TabIndex = 0
		Me.ReferenceDesignator_CheckBox.Text = "Reference Designator"
		Me.ReferenceDesignator_CheckBox.UseVisualStyleBackColor = True
		'
		'Value_CheckBox
		'
		Me.Value_CheckBox.AutoSize = True
		Me.Value_CheckBox.Checked = True
		Me.Value_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.Value_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Value_CheckBox.Location = New System.Drawing.Point(6, 79)
		Me.Value_CheckBox.Name = "Value_CheckBox"
		Me.Value_CheckBox.Size = New System.Drawing.Size(69, 24)
		Me.Value_CheckBox.TabIndex = 2
		Me.Value_CheckBox.Text = "Value"
		Me.Value_CheckBox.UseVisualStyleBackColor = True
		'
		'Process_CheckBox
		'
		Me.Process_CheckBox.AutoSize = True
		Me.Process_CheckBox.Checked = True
		Me.Process_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.Process_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Process_CheckBox.Location = New System.Drawing.Point(6, 169)
		Me.Process_CheckBox.Name = "Process_CheckBox"
		Me.Process_CheckBox.Size = New System.Drawing.Size(85, 24)
		Me.Process_CheckBox.TabIndex = 5
		Me.Process_CheckBox.Text = "Process"
		Me.Process_CheckBox.UseVisualStyleBackColor = True
		'
		'Vendor_CheckBox
		'
		Me.Vendor_CheckBox.AutoSize = True
		Me.Vendor_CheckBox.Checked = True
		Me.Vendor_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.Vendor_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Vendor_CheckBox.Location = New System.Drawing.Point(6, 109)
		Me.Vendor_CheckBox.Name = "Vendor_CheckBox"
		Me.Vendor_CheckBox.Size = New System.Drawing.Size(80, 24)
		Me.Vendor_CheckBox.TabIndex = 3
		Me.Vendor_CheckBox.Text = "Vendor"
		Me.Vendor_CheckBox.UseVisualStyleBackColor = True
		'
		'ItemNumber_CheckBox
		'
		Me.ItemNumber_CheckBox.AutoSize = True
		Me.ItemNumber_CheckBox.Checked = True
		Me.ItemNumber_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.ItemNumber_CheckBox.Enabled = False
		Me.ItemNumber_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ItemNumber_CheckBox.Location = New System.Drawing.Point(6, 49)
		Me.ItemNumber_CheckBox.Name = "ItemNumber_CheckBox"
		Me.ItemNumber_CheckBox.Size = New System.Drawing.Size(120, 24)
		Me.ItemNumber_CheckBox.TabIndex = 1
		Me.ItemNumber_CheckBox.Text = "Item Number"
		Me.ItemNumber_CheckBox.UseVisualStyleBackColor = True
		'
		'MPN_CheckBox
		'
		Me.MPN_CheckBox.AutoSize = True
		Me.MPN_CheckBox.Checked = True
		Me.MPN_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.MPN_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MPN_CheckBox.Location = New System.Drawing.Point(6, 139)
		Me.MPN_CheckBox.Name = "MPN_CheckBox"
		Me.MPN_CheckBox.Size = New System.Drawing.Size(62, 24)
		Me.MPN_CheckBox.TabIndex = 4
		Me.MPN_CheckBox.Text = "MPN"
		Me.MPN_CheckBox.UseVisualStyleBackColor = True
		'
		'BAS_CheckBox
		'
		Me.BAS_CheckBox.AutoSize = True
		Me.BAS_CheckBox.Checked = True
		Me.BAS_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.BAS_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BAS_CheckBox.Location = New System.Drawing.Point(6, 139)
		Me.BAS_CheckBox.Name = "BAS_CheckBox"
		Me.BAS_CheckBox.Size = New System.Drawing.Size(61, 24)
		Me.BAS_CheckBox.TabIndex = 4
		Me.BAS_CheckBox.Text = "BAS"
		Me.BAS_CheckBox.UseVisualStyleBackColor = True
		'
		'SMT_CheckBox
		'
		Me.SMT_CheckBox.AutoSize = True
		Me.SMT_CheckBox.Checked = True
		Me.SMT_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.SMT_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SMT_CheckBox.Location = New System.Drawing.Point(6, 109)
		Me.SMT_CheckBox.Name = "SMT_CheckBox"
		Me.SMT_CheckBox.Size = New System.Drawing.Size(61, 24)
		Me.SMT_CheckBox.TabIndex = 3
		Me.SMT_CheckBox.Text = "SMT"
		Me.SMT_CheckBox.UseVisualStyleBackColor = True
		'
		'PostAssembly_CheckBox
		'
		Me.PostAssembly_CheckBox.AutoSize = True
		Me.PostAssembly_CheckBox.Checked = True
		Me.PostAssembly_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.PostAssembly_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PostAssembly_CheckBox.Location = New System.Drawing.Point(6, 79)
		Me.PostAssembly_CheckBox.Name = "PostAssembly_CheckBox"
		Me.PostAssembly_CheckBox.Size = New System.Drawing.Size(162, 24)
		Me.PostAssembly_CheckBox.TabIndex = 2
		Me.PostAssembly_CheckBox.Text = "POST ASSEMBLY"
		Me.PostAssembly_CheckBox.UseVisualStyleBackColor = True
		'
		'PCBboard_CheckBox
		'
		Me.PCBboard_CheckBox.AutoSize = True
		Me.PCBboard_CheckBox.Checked = True
		Me.PCBboard_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.PCBboard_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PCBboard_CheckBox.Location = New System.Drawing.Point(6, 49)
		Me.PCBboard_CheckBox.Name = "PCBboard_CheckBox"
		Me.PCBboard_CheckBox.Size = New System.Drawing.Size(122, 24)
		Me.PCBboard_CheckBox.TabIndex = 1
		Me.PCBboard_CheckBox.Text = "PCB BOARD"
		Me.PCBboard_CheckBox.UseVisualStyleBackColor = True
		'
		'HandFlow_CheckBox
		'
		Me.HandFlow_CheckBox.AutoSize = True
		Me.HandFlow_CheckBox.Checked = True
		Me.HandFlow_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked
		Me.HandFlow_CheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.HandFlow_CheckBox.Location = New System.Drawing.Point(6, 19)
		Me.HandFlow_CheckBox.Name = "HandFlow_CheckBox"
		Me.HandFlow_CheckBox.Size = New System.Drawing.Size(124, 24)
		Me.HandFlow_CheckBox.TabIndex = 0
		Me.HandFlow_CheckBox.Text = "HAND FLOW"
		Me.HandFlow_CheckBox.UseVisualStyleBackColor = True
		'
		'GenerateReport_Button
		'
		Me.GenerateReport_Button.AutoSize = True
		Me.GenerateReport_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.GenerateReport_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.GenerateReport_Button.Location = New System.Drawing.Point(333, 14)
		Me.GenerateReport_Button.Name = "GenerateReport_Button"
		Me.GenerateReport_Button.Size = New System.Drawing.Size(140, 30)
		Me.GenerateReport_Button.TabIndex = 42
		Me.GenerateReport_Button.Text = "Generate Report"
		Me.GenerateReport_Button.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.BAS_CheckBox)
		Me.GroupBox1.Controls.Add(Me.SMT_CheckBox)
		Me.GroupBox1.Controls.Add(Me.PostAssembly_CheckBox)
		Me.GroupBox1.Controls.Add(Me.PCBboard_CheckBox)
		Me.GroupBox1.Controls.Add(Me.HandFlow_CheckBox)
		Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.GroupBox1.Location = New System.Drawing.Point(16, 295)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(204, 166)
		Me.GroupBox1.TabIndex = 40
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "Process"
		'
		'Excel_Button
		'
		Me.Excel_Button.AutoSize = True
		Me.Excel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Excel_Button.Enabled = False
		Me.Excel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Excel_Button.Location = New System.Drawing.Point(479, 14)
		Me.Excel_Button.Name = "Excel_Button"
		Me.Excel_Button.Size = New System.Drawing.Size(109, 30)
		Me.Excel_Button.TabIndex = 43
		Me.Excel_Button.Text = "Create Excel"
		Me.Excel_Button.UseVisualStyleBackColor = True
		'
		'Boards_ComboBox
		'
		Me.Boards_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.Boards_ComboBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Boards_ComboBox.FormattingEnabled = True
		Me.Boards_ComboBox.Location = New System.Drawing.Point(12, 50)
		Me.Boards_ComboBox.Name = "Boards_ComboBox"
		Me.Boards_ComboBox.Size = New System.Drawing.Size(208, 28)
		Me.Boards_ComboBox.TabIndex = 38
		'
		'Close_Button
		'
		Me.Close_Button.AutoSize = True
		Me.Close_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Close_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Close_Button.Location = New System.Drawing.Point(594, 14)
		Me.Close_Button.Name = "Close_Button"
		Me.Close_Button.Size = New System.Drawing.Size(59, 30)
		Me.Close_Button.TabIndex = 44
		Me.Close_Button.Text = "Close"
		Me.Close_Button.UseVisualStyleBackColor = True
		'
		'BOMReport
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(734, 472)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.BOM_DGV)
		Me.Controls.Add(Me.Condense_CheckBox)
		Me.Controls.Add(Me.GroupBox2)
		Me.Controls.Add(Me.GenerateReport_Button)
		Me.Controls.Add(Me.GroupBox1)
		Me.Controls.Add(Me.Excel_Button)
		Me.Controls.Add(Me.Boards_ComboBox)
		Me.Controls.Add(Me.Close_Button)
		Me.Name = "BOMReport"
		Me.Text = "BOM Report"
		CType(Me.BOM_DGV, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents Label3 As Label
	Friend WithEvents BOM_DGV As DataGridView
	Friend WithEvents Condense_CheckBox As CheckBox
	Friend WithEvents GroupBox2 As GroupBox
	Friend WithEvents ReferenceDesignator_CheckBox As CheckBox
	Friend WithEvents Value_CheckBox As CheckBox
	Friend WithEvents Process_CheckBox As CheckBox
	Friend WithEvents Vendor_CheckBox As CheckBox
	Friend WithEvents ItemNumber_CheckBox As CheckBox
	Friend WithEvents MPN_CheckBox As CheckBox
	Friend WithEvents BAS_CheckBox As CheckBox
	Friend WithEvents SMT_CheckBox As CheckBox
	Friend WithEvents PostAssembly_CheckBox As CheckBox
	Friend WithEvents PCBboard_CheckBox As CheckBox
	Friend WithEvents HandFlow_CheckBox As CheckBox
	Friend WithEvents GenerateReport_Button As Button
	Friend WithEvents GroupBox1 As GroupBox
	Friend WithEvents Excel_Button As Button
	Friend WithEvents Boards_ComboBox As ComboBox
	Friend WithEvents Close_Button As Button
End Class
