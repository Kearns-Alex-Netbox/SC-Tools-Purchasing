<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MasterControl
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
		Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
		Me.CB_QB_Search2 = New System.Windows.Forms.ComboBox()
		Me.RB_QB_DescendingSort = New System.Windows.Forms.RadioButton()
		Me.B_QB_First = New System.Windows.Forms.Button()
		Me.B_QB_Last = New System.Windows.Forms.Button()
		Me.DGV_QB_Items = New System.Windows.Forms.DataGridView()
		Me.L_QB_Results = New System.Windows.Forms.Label()
		Me.TB_QB_Search = New System.Windows.Forms.TextBox()
		Me.B_QB_Previous = New System.Windows.Forms.Button()
		Me.RB_QB_AscendingSort = New System.Windows.Forms.RadioButton()
		Me.B_Close = New System.Windows.Forms.Button()
		Me.B_QB_Next = New System.Windows.Forms.Button()
		Me.CB_QB_Search = New System.Windows.Forms.ComboBox()
		Me.B_QB_Sort = New System.Windows.Forms.Button()
		Me.B_QB_ListAll = New System.Windows.Forms.Button()
		Me.CB_QB_Sort = New System.Windows.Forms.ComboBox()
		Me.B_QB_Search = New System.Windows.Forms.Button()
		Me.B_CreateExcel = New System.Windows.Forms.Button()
		Me.TabControl1 = New System.Windows.Forms.TabControl()
		Me.TP_QB_items = New System.Windows.Forms.TabPage()
		Me.TB_QB_Search2 = New System.Windows.Forms.TextBox()
		Me.CB_QB_Display = New System.Windows.Forms.ComboBox()
		Me.CB_QB_Operand2 = New System.Windows.Forms.ComboBox()
		Me.CB_QB_Operand1 = New System.Windows.Forms.ComboBox()
		CType(Me.DGV_QB_Items, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.TabControl1.SuspendLayout()
		Me.TP_QB_items.SuspendLayout()
		Me.SuspendLayout()
		'
		'CB_QB_Search2
		'
		Me.CB_QB_Search2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Search2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Search2.FormattingEnabled = True
		Me.CB_QB_Search2.Location = New System.Drawing.Point(6, 40)
		Me.CB_QB_Search2.Name = "CB_QB_Search2"
		Me.CB_QB_Search2.Size = New System.Drawing.Size(212, 28)
		Me.CB_QB_Search2.TabIndex = 2
		'
		'RB_QB_DescendingSort
		'
		Me.RB_QB_DescendingSort.AutoSize = True
		Me.RB_QB_DescendingSort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.RB_QB_DescendingSort.Location = New System.Drawing.Point(341, 75)
		Me.RB_QB_DescendingSort.Name = "RB_QB_DescendingSort"
		Me.RB_QB_DescendingSort.Size = New System.Drawing.Size(122, 24)
		Me.RB_QB_DescendingSort.TabIndex = 6
		Me.RB_QB_DescendingSort.TabStop = True
		Me.RB_QB_DescendingSort.Text = "Descending"
		Me.RB_QB_DescendingSort.UseVisualStyleBackColor = True
		'
		'B_QB_First
		'
		Me.B_QB_First.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.B_QB_First.AutoSize = True
		Me.B_QB_First.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_First.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_First.Location = New System.Drawing.Point(6, 528)
		Me.B_QB_First.Name = "B_QB_First"
		Me.B_QB_First.Size = New System.Drawing.Size(50, 30)
		Me.B_QB_First.TabIndex = 10
		Me.B_QB_First.Text = "First"
		Me.B_QB_First.UseVisualStyleBackColor = True
		'
		'B_QB_Last
		'
		Me.B_QB_Last.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.B_QB_Last.AutoSize = True
		Me.B_QB_Last.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_Last.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_Last.Location = New System.Drawing.Point(168, 528)
		Me.B_QB_Last.Name = "B_QB_Last"
		Me.B_QB_Last.Size = New System.Drawing.Size(50, 30)
		Me.B_QB_Last.TabIndex = 13
		Me.B_QB_Last.Text = "Last"
		Me.B_QB_Last.UseVisualStyleBackColor = True
		'
		'DGV_QB_Items
		'
		Me.DGV_QB_Items.AllowUserToAddRows = False
		Me.DGV_QB_Items.AllowUserToDeleteRows = False
		Me.DGV_QB_Items.AllowUserToOrderColumns = True
		DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
		Me.DGV_QB_Items.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
		Me.DGV_QB_Items.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.DGV_QB_Items.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
		Me.DGV_QB_Items.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
		Me.DGV_QB_Items.BackgroundColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
		DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
		DataGridViewCellStyle4.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
		DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
		DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
		DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
		Me.DGV_QB_Items.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
		Me.DGV_QB_Items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
		Me.DGV_QB_Items.Location = New System.Drawing.Point(6, 108)
		Me.DGV_QB_Items.Name = "DGV_QB_Items"
		Me.DGV_QB_Items.Size = New System.Drawing.Size(1050, 414)
		Me.DGV_QB_Items.TabIndex = 16
		'
		'L_QB_Results
		'
		Me.L_QB_Results.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.L_QB_Results.AutoSize = True
		Me.L_QB_Results.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.L_QB_Results.Location = New System.Drawing.Point(299, 533)
		Me.L_QB_Results.Name = "L_QB_Results"
		Me.L_QB_Results.Size = New System.Drawing.Size(151, 20)
		Me.L_QB_Results.TabIndex = 15
		Me.L_QB_Results.Text = "Number of results"
		'
		'TB_QB_Search
		'
		Me.TB_QB_Search.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TB_QB_Search.Location = New System.Drawing.Point(294, 7)
		Me.TB_QB_Search.Name = "TB_QB_Search"
		Me.TB_QB_Search.Size = New System.Drawing.Size(254, 26)
		Me.TB_QB_Search.TabIndex = 1
		'
		'B_QB_Previous
		'
		Me.B_QB_Previous.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.B_QB_Previous.AutoSize = True
		Me.B_QB_Previous.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_Previous.Enabled = False
		Me.B_QB_Previous.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_Previous.Location = New System.Drawing.Point(62, 528)
		Me.B_QB_Previous.Name = "B_QB_Previous"
		Me.B_QB_Previous.Size = New System.Drawing.Size(47, 30)
		Me.B_QB_Previous.TabIndex = 11
		Me.B_QB_Previous.Text = "<<--"
		Me.B_QB_Previous.UseVisualStyleBackColor = True
		'
		'RB_QB_AscendingSort
		'
		Me.RB_QB_AscendingSort.AutoSize = True
		Me.RB_QB_AscendingSort.Checked = True
		Me.RB_QB_AscendingSort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.RB_QB_AscendingSort.Location = New System.Drawing.Point(224, 75)
		Me.RB_QB_AscendingSort.Name = "RB_QB_AscendingSort"
		Me.RB_QB_AscendingSort.Size = New System.Drawing.Size(111, 24)
		Me.RB_QB_AscendingSort.TabIndex = 5
		Me.RB_QB_AscendingSort.TabStop = True
		Me.RB_QB_AscendingSort.Text = "Ascending"
		Me.RB_QB_AscendingSort.UseVisualStyleBackColor = True
		'
		'B_Close
		'
		Me.B_Close.AutoSize = True
		Me.B_Close.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_Close.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_Close.Location = New System.Drawing.Point(686, 7)
		Me.B_Close.Name = "B_Close"
		Me.B_Close.Size = New System.Drawing.Size(59, 30)
		Me.B_Close.TabIndex = 8
		Me.B_Close.Text = "Close"
		Me.B_Close.UseVisualStyleBackColor = True
		'
		'B_QB_Next
		'
		Me.B_QB_Next.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.B_QB_Next.AutoSize = True
		Me.B_QB_Next.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_Next.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_Next.Location = New System.Drawing.Point(115, 528)
		Me.B_QB_Next.Name = "B_QB_Next"
		Me.B_QB_Next.Size = New System.Drawing.Size(47, 30)
		Me.B_QB_Next.TabIndex = 12
		Me.B_QB_Next.Text = "-->>"
		Me.B_QB_Next.UseVisualStyleBackColor = True
		'
		'CB_QB_Search
		'
		Me.CB_QB_Search.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Search.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Search.FormattingEnabled = True
		Me.CB_QB_Search.Location = New System.Drawing.Point(6, 6)
		Me.CB_QB_Search.Name = "CB_QB_Search"
		Me.CB_QB_Search.Size = New System.Drawing.Size(212, 28)
		Me.CB_QB_Search.TabIndex = 0
		'
		'B_QB_Sort
		'
		Me.B_QB_Sort.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_Sort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_Sort.Location = New System.Drawing.Point(478, 73)
		Me.B_QB_Sort.Name = "B_QB_Sort"
		Me.B_QB_Sort.Size = New System.Drawing.Size(70, 30)
		Me.B_QB_Sort.TabIndex = 8
		Me.B_QB_Sort.Text = "Sort"
		Me.B_QB_Sort.UseVisualStyleBackColor = True
		'
		'B_QB_ListAll
		'
		Me.B_QB_ListAll.AutoSize = True
		Me.B_QB_ListAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_ListAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_ListAll.Location = New System.Drawing.Point(630, 22)
		Me.B_QB_ListAll.Name = "B_QB_ListAll"
		Me.B_QB_ListAll.Size = New System.Drawing.Size(65, 30)
		Me.B_QB_ListAll.TabIndex = 9
		Me.B_QB_ListAll.Text = "List All"
		Me.B_QB_ListAll.UseVisualStyleBackColor = True
		'
		'CB_QB_Sort
		'
		Me.CB_QB_Sort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Sort.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Sort.FormattingEnabled = True
		Me.CB_QB_Sort.Location = New System.Drawing.Point(6, 74)
		Me.CB_QB_Sort.Name = "CB_QB_Sort"
		Me.CB_QB_Sort.Size = New System.Drawing.Size(212, 28)
		Me.CB_QB_Sort.TabIndex = 4
		'
		'B_QB_Search
		'
		Me.B_QB_Search.AutoSize = True
		Me.B_QB_Search.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_QB_Search.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_QB_Search.Location = New System.Drawing.Point(554, 22)
		Me.B_QB_Search.Name = "B_QB_Search"
		Me.B_QB_Search.Size = New System.Drawing.Size(70, 30)
		Me.B_QB_Search.TabIndex = 7
		Me.B_QB_Search.Text = "Search"
		Me.B_QB_Search.UseVisualStyleBackColor = True
		'
		'B_CreateExcel
		'
		Me.B_CreateExcel.AutoSize = True
		Me.B_CreateExcel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.B_CreateExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.B_CreateExcel.Location = New System.Drawing.Point(572, 7)
		Me.B_CreateExcel.Name = "B_CreateExcel"
		Me.B_CreateExcel.Size = New System.Drawing.Size(109, 30)
		Me.B_CreateExcel.TabIndex = 7
		Me.B_CreateExcel.Text = "Create Excel"
		Me.B_CreateExcel.UseVisualStyleBackColor = True
		'
		'TabControl1
		'
		Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
		Me.TabControl1.Controls.Add(Me.TP_QB_items)
		Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TabControl1.Location = New System.Drawing.Point(14, 9)
		Me.TabControl1.Name = "TabControl1"
		Me.TabControl1.SelectedIndex = 0
		Me.TabControl1.Size = New System.Drawing.Size(1073, 605)
		Me.TabControl1.TabIndex = 6
		'
		'TP_QB_items
		'
		Me.TP_QB_items.BackColor = System.Drawing.Color.Transparent
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Operand2)
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Operand1)
		Me.TP_QB_items.Controls.Add(Me.TB_QB_Search2)
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Display)
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Search2)
		Me.TP_QB_items.Controls.Add(Me.RB_QB_DescendingSort)
		Me.TP_QB_items.Controls.Add(Me.B_QB_First)
		Me.TP_QB_items.Controls.Add(Me.B_QB_Last)
		Me.TP_QB_items.Controls.Add(Me.DGV_QB_Items)
		Me.TP_QB_items.Controls.Add(Me.L_QB_Results)
		Me.TP_QB_items.Controls.Add(Me.TB_QB_Search)
		Me.TP_QB_items.Controls.Add(Me.B_QB_Previous)
		Me.TP_QB_items.Controls.Add(Me.RB_QB_AscendingSort)
		Me.TP_QB_items.Controls.Add(Me.B_QB_Next)
		Me.TP_QB_items.Controls.Add(Me.B_QB_Search)
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Search)
		Me.TP_QB_items.Controls.Add(Me.B_QB_Sort)
		Me.TP_QB_items.Controls.Add(Me.B_QB_ListAll)
		Me.TP_QB_items.Controls.Add(Me.CB_QB_Sort)
		Me.TP_QB_items.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TP_QB_items.Location = New System.Drawing.Point(4, 32)
		Me.TP_QB_items.Name = "TP_QB_items"
		Me.TP_QB_items.Padding = New System.Windows.Forms.Padding(3)
		Me.TP_QB_items.Size = New System.Drawing.Size(1065, 569)
		Me.TP_QB_items.TabIndex = 0
		Me.TP_QB_items.Text = "QB Items"
		Me.TP_QB_items.UseVisualStyleBackColor = True
		'
		'TB_QB_Search2
		'
		Me.TB_QB_Search2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TB_QB_Search2.Location = New System.Drawing.Point(294, 41)
		Me.TB_QB_Search2.Name = "TB_QB_Search2"
		Me.TB_QB_Search2.Size = New System.Drawing.Size(254, 26)
		Me.TB_QB_Search2.TabIndex = 3
		'
		'CB_QB_Display
		'
		Me.CB_QB_Display.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.CB_QB_Display.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Display.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Display.FormattingEnabled = True
		Me.CB_QB_Display.Items.AddRange(New Object() {"100", "250", "500"})
		Me.CB_QB_Display.Location = New System.Drawing.Point(224, 528)
		Me.CB_QB_Display.Name = "CB_QB_Display"
		Me.CB_QB_Display.Size = New System.Drawing.Size(69, 28)
		Me.CB_QB_Display.TabIndex = 14
		'
		'CB_QB_Operand2
		'
		Me.CB_QB_Operand2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Operand2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Operand2.FormattingEnabled = True
		Me.CB_QB_Operand2.Items.AddRange(New Object() {"LIKE", "=", "<=", ">="})
		Me.CB_QB_Operand2.Location = New System.Drawing.Point(224, 40)
		Me.CB_QB_Operand2.Name = "CB_QB_Operand2"
		Me.CB_QB_Operand2.Size = New System.Drawing.Size(64, 28)
		Me.CB_QB_Operand2.TabIndex = 20
		'
		'CB_QB_Operand1
		'
		Me.CB_QB_Operand1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.CB_QB_Operand1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CB_QB_Operand1.FormattingEnabled = True
		Me.CB_QB_Operand1.Items.AddRange(New Object() {"LIKE", "=", "<=", ">="})
		Me.CB_QB_Operand1.Location = New System.Drawing.Point(224, 6)
		Me.CB_QB_Operand1.Name = "CB_QB_Operand1"
		Me.CB_QB_Operand1.Size = New System.Drawing.Size(64, 28)
		Me.CB_QB_Operand1.TabIndex = 19
		'
		'MasterControl
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1101, 620)
		Me.Controls.Add(Me.B_Close)
		Me.Controls.Add(Me.B_CreateExcel)
		Me.Controls.Add(Me.TabControl1)
		Me.Name = "MasterControl"
		Me.Text = "Master Control"
		CType(Me.DGV_QB_Items, System.ComponentModel.ISupportInitialize).EndInit()
		Me.TabControl1.ResumeLayout(False)
		Me.TP_QB_items.ResumeLayout(False)
		Me.TP_QB_items.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents CB_QB_Search2 As ComboBox
	Friend WithEvents RB_QB_DescendingSort As RadioButton
	Friend WithEvents B_QB_First As Button
	Friend WithEvents B_QB_Last As Button
	Friend WithEvents DGV_QB_Items As DataGridView
	Friend WithEvents L_QB_Results As Label
	Friend WithEvents TB_QB_Search As TextBox
	Friend WithEvents B_QB_Previous As Button
	Friend WithEvents RB_QB_AscendingSort As RadioButton
	Friend WithEvents B_Close As Button
	Friend WithEvents B_QB_Next As Button
	Friend WithEvents CB_QB_Search As ComboBox
	Friend WithEvents B_QB_Sort As Button
	Friend WithEvents B_QB_ListAll As Button
	Friend WithEvents CB_QB_Sort As ComboBox
	Friend WithEvents B_QB_Search As Button
	Friend WithEvents B_CreateExcel As Button
	Friend WithEvents TabControl1 As TabControl
	Friend WithEvents TP_QB_items As TabPage
	Friend WithEvents TB_QB_Search2 As TextBox
	Friend WithEvents CB_QB_Display As ComboBox
	Friend WithEvents CB_QB_Operand2 As ComboBox
	Friend WithEvents CB_QB_Operand1 As ComboBox
End Class
