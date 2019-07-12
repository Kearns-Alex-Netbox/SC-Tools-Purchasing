<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CostofProduct
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
		Me.Label2 = New System.Windows.Forms.Label()
		Me.L_TotalCost = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.CB_SourceLevel = New System.Windows.Forms.ComboBox()
		Me.Cost_DGV = New System.Windows.Forms.DataGridView()
		Me.L_Missing = New System.Windows.Forms.Label()
		Me.L_Quantity = New System.Windows.Forms.Label()
		Me.Excel_Button = New System.Windows.Forms.Button()
		Me.Cost_Button = New System.Windows.Forms.Button()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.CB_Products = New System.Windows.Forms.ComboBox()
		Me.Close_Button = New System.Windows.Forms.Button()
		CType(Me.Cost_DGV, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Consolas", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(12, 10)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(144, 19)
		Me.Label2.TabIndex = 49
		Me.Label2.Text = "Cost of Product"
		'
		'L_TotalCost
		'
		Me.L_TotalCost.AutoSize = True
		Me.L_TotalCost.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.L_TotalCost.Location = New System.Drawing.Point(12, 166)
		Me.L_TotalCost.Name = "L_TotalCost"
		Me.L_TotalCost.Size = New System.Drawing.Size(102, 24)
		Me.L_TotalCost.TabIndex = 47
		Me.L_TotalCost.Text = "Item Cost $"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(12, 50)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(113, 20)
		Me.Label3.TabIndex = 44
		Me.Label3.Text = "Source Level"
		'
		'CB_SourceLevel
		'
		Me.CB_SourceLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_SourceLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_SourceLevel.FormattingEnabled = True
		Me.CB_SourceLevel.Location = New System.Drawing.Point(131, 47)
		Me.CB_SourceLevel.Name = "CB_SourceLevel"
		Me.CB_SourceLevel.Size = New System.Drawing.Size(280, 28)
		Me.CB_SourceLevel.TabIndex = 38
		'
		'Cost_DGV
		'
		Me.Cost_DGV.AllowUserToAddRows = False
		Me.Cost_DGV.AllowUserToDeleteRows = False
		DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
		Me.Cost_DGV.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
		Me.Cost_DGV.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Cost_DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
		Me.Cost_DGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
		Me.Cost_DGV.BackgroundColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Cost_DGV.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
		Me.Cost_DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
		DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
		DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.Cost_DGV.DefaultCellStyle = DataGridViewCellStyle3
		Me.Cost_DGV.Location = New System.Drawing.Point(417, 47)
		Me.Cost_DGV.Name = "Cost_DGV"
		Me.Cost_DGV.Size = New System.Drawing.Size(446, 435)
		Me.Cost_DGV.TabIndex = 43
		'
		'L_Missing
		'
		Me.L_Missing.AutoSize = True
		Me.L_Missing.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.L_Missing.Location = New System.Drawing.Point(12, 223)
		Me.L_Missing.Name = "L_Missing"
		Me.L_Missing.Size = New System.Drawing.Size(308, 40)
		Me.L_Missing.TabIndex = 48
		Me.L_Missing.Text = "(**) Component not found in the database." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Totals will not include these componen" &
	"t(s)."
		Me.L_Missing.Visible = False
		'
		'L_Quantity
		'
		Me.L_Quantity.AutoSize = True
		Me.L_Quantity.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.L_Quantity.Location = New System.Drawing.Point(12, 122)
		Me.L_Quantity.Name = "L_Quantity"
		Me.L_Quantity.Size = New System.Drawing.Size(105, 24)
		Me.L_Quantity.TabIndex = 46
		Me.L_Quantity.Text = "Total Items:"
		'
		'Excel_Button
		'
		Me.Excel_Button.AutoSize = True
		Me.Excel_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Excel_Button.Enabled = False
		Me.Excel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Excel_Button.Location = New System.Drawing.Point(477, 13)
		Me.Excel_Button.Name = "Excel_Button"
		Me.Excel_Button.Size = New System.Drawing.Size(109, 30)
		Me.Excel_Button.TabIndex = 41
		Me.Excel_Button.Text = "Create Excel"
		Me.Excel_Button.UseVisualStyleBackColor = True
		'
		'Cost_Button
		'
		Me.Cost_Button.AutoSize = True
		Me.Cost_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Cost_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Cost_Button.Location = New System.Drawing.Point(417, 13)
		Me.Cost_Button.Name = "Cost_Button"
		Me.Cost_Button.Size = New System.Drawing.Size(52, 30)
		Me.Cost_Button.TabIndex = 40
		Me.Cost_Button.Text = "Cost"
		Me.Cost_Button.UseVisualStyleBackColor = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(54, 84)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(71, 20)
		Me.Label1.TabIndex = 45
		Me.Label1.Text = "Product"
		'
		'CB_Products
		'
		Me.CB_Products.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_Products.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_Products.FormattingEnabled = True
		Me.CB_Products.Location = New System.Drawing.Point(131, 81)
		Me.CB_Products.Name = "CB_Products"
		Me.CB_Products.Size = New System.Drawing.Size(280, 28)
		Me.CB_Products.TabIndex = 39
		'
		'Close_Button
		'
		Me.Close_Button.AutoSize = True
		Me.Close_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.Close_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
		Me.Close_Button.Location = New System.Drawing.Point(592, 13)
		Me.Close_Button.Name = "Close_Button"
		Me.Close_Button.Size = New System.Drawing.Size(59, 30)
		Me.Close_Button.TabIndex = 42
		Me.Close_Button.Text = "Close"
		Me.Close_Button.UseVisualStyleBackColor = True
		'
		'CostofProduct
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(875, 493)
		Me.Controls.Add(Me.Label2)
		Me.Controls.Add(Me.L_TotalCost)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.CB_SourceLevel)
		Me.Controls.Add(Me.Cost_DGV)
		Me.Controls.Add(Me.L_Missing)
		Me.Controls.Add(Me.L_Quantity)
		Me.Controls.Add(Me.Excel_Button)
		Me.Controls.Add(Me.Cost_Button)
		Me.Controls.Add(Me.Label1)
		Me.Controls.Add(Me.CB_Products)
		Me.Controls.Add(Me.Close_Button)
		Me.Name = "CostofProduct"
		Me.Text = "Cost of Product"
		CType(Me.Cost_DGV, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents Label2 As Label
	Friend WithEvents L_TotalCost As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents CB_SourceLevel As ComboBox
	Friend WithEvents Cost_DGV As DataGridView
	Friend WithEvents L_Missing As Label
	Friend WithEvents L_Quantity As Label
	Friend WithEvents Excel_Button As Button
	Friend WithEvents Cost_Button As Button
	Friend WithEvents Label1 As Label
	Friend WithEvents CB_Products As ComboBox
	Friend WithEvents Close_Button As Button
End Class
