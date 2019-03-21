<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPLC
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    'Public WithEvents txtFlow As System.Windows.Forms.TextBox
    'Public WithEvents txtCh1 As System.Windows.Forms.TextBox
    'Public WithEvents cmdExit As System.Windows.Forms.Button
    'Public WithEvents Timer1 As System.Windows.Forms.Timer
    'Public WithEvents txtCh4 As System.Windows.Forms.TextBox
    'Public WithEvents txtCh3 As System.Windows.Forms.TextBox
    'Public WithEvents txtCh2 As System.Windows.Forms.TextBox
    'Public WithEvents txtTemperature As System.Windows.Forms.TextBox
    'Public WithEvents txtDischarge As System.Windows.Forms.TextBox
    'Public WithEvents txtSuction As System.Windows.Forms.TextBox
    'Public WithEvents cmbPLCLoop As System.Windows.Forms.ComboBox
    'Public WithEvents lblVersion As System.Windows.Forms.Label
    Public WithEvents Label10 As System.Windows.Forms.Label
    Public WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents Label1 As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmbPLCLoop = New System.Windows.Forms.ComboBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.txtFlow = New System.Windows.Forms.TextBox()
        Me.txtSuction = New System.Windows.Forms.TextBox()
        Me.txtDischarge = New System.Windows.Forms.TextBox()
        Me.txtTemperature = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtCh4 = New System.Windows.Forms.TextBox()
        Me.txtCh3 = New System.Windows.Forms.TextBox()
        Me.txtCh2 = New System.Windows.Forms.TextBox()
        Me.txtCh1 = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.btnAveRaw = New System.Windows.Forms.Button()
        Me.pnlDamping = New System.Windows.Forms.Panel()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.lblAveRaw = New System.Windows.Forms.Label()
        Me.pnlDamping.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbPLCLoop
        '
        Me.cmbPLCLoop.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPLCLoop.FormattingEnabled = True
        Me.cmbPLCLoop.Location = New System.Drawing.Point(362, 120)
        Me.cmbPLCLoop.Name = "cmbPLCLoop"
        Me.cmbPLCLoop.Size = New System.Drawing.Size(200, 33)
        Me.cmbPLCLoop.TabIndex = 1
        '
        'cmdExit
        '
        Me.cmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.Location = New System.Drawing.Point(414, 753)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(92, 29)
        Me.cmdExit.TabIndex = 2
        Me.cmdExit.Text = "Exit"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'txtFlow
        '
        Me.txtFlow.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFlow.Location = New System.Drawing.Point(212, 208)
        Me.txtFlow.Name = "txtFlow"
        Me.txtFlow.Size = New System.Drawing.Size(183, 53)
        Me.txtFlow.TabIndex = 3
        Me.txtFlow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSuction
        '
        Me.txtSuction.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSuction.Location = New System.Drawing.Point(212, 275)
        Me.txtSuction.Name = "txtSuction"
        Me.txtSuction.Size = New System.Drawing.Size(183, 53)
        Me.txtSuction.TabIndex = 4
        Me.txtSuction.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDischarge
        '
        Me.txtDischarge.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDischarge.Location = New System.Drawing.Point(212, 338)
        Me.txtDischarge.Name = "txtDischarge"
        Me.txtDischarge.Size = New System.Drawing.Size(183, 53)
        Me.txtDischarge.TabIndex = 5
        Me.txtDischarge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTemperature
        '
        Me.txtTemperature.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTemperature.Location = New System.Drawing.Point(212, 401)
        Me.txtTemperature.Name = "txtTemperature"
        Me.txtTemperature.Size = New System.Drawing.Size(183, 53)
        Me.txtTemperature.TabIndex = 6
        Me.txtTemperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(140, 224)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 29)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Flow"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(113, 291)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(93, 29)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "Suction"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(84, 354)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(122, 29)
        Me.Label13.TabIndex = 9
        Me.Label13.Text = "Discharge"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(53, 417)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(153, 29)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "Temperature"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(537, 417)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(62, 29)
        Me.Label15.TabIndex = 18
        Me.Label15.Text = "Ch 4"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(537, 354)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 29)
        Me.Label16.TabIndex = 17
        Me.Label16.Text = "Ch 3"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(537, 291)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(62, 29)
        Me.Label17.TabIndex = 16
        Me.Label17.Text = "Ch 2"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(537, 224)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(62, 29)
        Me.Label18.TabIndex = 15
        Me.Label18.Text = "Ch 1"
        '
        'txtCh4
        '
        Me.txtCh4.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCh4.Location = New System.Drawing.Point(605, 401)
        Me.txtCh4.Name = "txtCh4"
        Me.txtCh4.Size = New System.Drawing.Size(183, 53)
        Me.txtCh4.TabIndex = 14
        Me.txtCh4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCh3
        '
        Me.txtCh3.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCh3.Location = New System.Drawing.Point(605, 342)
        Me.txtCh3.Name = "txtCh3"
        Me.txtCh3.Size = New System.Drawing.Size(183, 53)
        Me.txtCh3.TabIndex = 13
        Me.txtCh3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCh2
        '
        Me.txtCh2.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCh2.Location = New System.Drawing.Point(605, 279)
        Me.txtCh2.Name = "txtCh2"
        Me.txtCh2.Size = New System.Drawing.Size(183, 53)
        Me.txtCh2.TabIndex = 12
        Me.txtCh2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCh1
        '
        Me.txtCh1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCh1.Location = New System.Drawing.Point(605, 212)
        Me.txtCh1.Name = "txtCh1"
        Me.txtCh1.Size = New System.Drawing.Size(183, 53)
        Me.txtCh1.TabIndex = 11
        Me.txtCh1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Timer1
        '
        Me.Timer1.Interval = 250
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(433, 34)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(0, 25)
        Me.lblVersion.TabIndex = 19
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(238, 4)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(448, 55)
        Me.Label19.TabIndex = 20
        Me.Label19.Text = "Get Data From PLC"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(396, 85)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(133, 29)
        Me.Label20.TabIndex = 21
        Me.Label20.Text = "Select PLC"
        '
        'btnAveRaw
        '
        Me.btnAveRaw.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAveRaw.Location = New System.Drawing.Point(51, 630)
        Me.btnAveRaw.Name = "btnAveRaw"
        Me.btnAveRaw.Size = New System.Drawing.Size(147, 77)
        Me.btnAveRaw.TabIndex = 22
        Me.btnAveRaw.Text = "Raw Data"
        Me.btnAveRaw.UseVisualStyleBackColor = True
        '
        'pnlDamping
        '
        Me.pnlDamping.Controls.Add(Me.Label23)
        Me.pnlDamping.Controls.Add(Me.Label22)
        Me.pnlDamping.Controls.Add(Me.Label21)
        Me.pnlDamping.Controls.Add(Me.TrackBar1)
        Me.pnlDamping.Location = New System.Drawing.Point(315, 602)
        Me.pnlDamping.Name = "pnlDamping"
        Me.pnlDamping.Size = New System.Drawing.Size(563, 133)
        Me.pnlDamping.TabIndex = 27
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(460, 63)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(87, 29)
        Me.Label23.TabIndex = 30
        Me.Label23.Text = "Lighter"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(16, 63)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(96, 29)
        Me.Label22.TabIndex = 29
        Me.Label22.Text = "Heavier"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(229, 13)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(110, 29)
        Me.Label21.TabIndex = 28
        Me.Label21.Text = "Damping"
        '
        'TrackBar1
        '
        Me.TrackBar1.Location = New System.Drawing.Point(127, 45)
        Me.TrackBar1.Maximum = 9
        Me.TrackBar1.Minimum = 1
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(314, 64)
        Me.TrackBar1.TabIndex = 27
        Me.TrackBar1.Value = 1
        '
        'lblAveRaw
        '
        Me.lblAveRaw.AutoSize = True
        Me.lblAveRaw.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAveRaw.ForeColor = System.Drawing.Color.Red
        Me.lblAveRaw.Location = New System.Drawing.Point(199, 485)
        Me.lblAveRaw.Name = "lblAveRaw"
        Me.lblAveRaw.Size = New System.Drawing.Size(527, 52)
        Me.lblAveRaw.TabIndex = 28
        Me.lblAveRaw.Text = "Displaying Averaged Data"
        Me.lblAveRaw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmPLC
        '
        Me.ClientSize = New System.Drawing.Size(924, 804)
        Me.Controls.Add(Me.lblAveRaw)
        Me.Controls.Add(Me.pnlDamping)
        Me.Controls.Add(Me.btnAveRaw)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtCh4)
        Me.Controls.Add(Me.txtCh3)
        Me.Controls.Add(Me.txtCh2)
        Me.Controls.Add(Me.txtCh1)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtTemperature)
        Me.Controls.Add(Me.txtDischarge)
        Me.Controls.Add(Me.txtSuction)
        Me.Controls.Add(Me.txtFlow)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmbPLCLoop)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmPLC"
        Me.pnlDamping.ResumeLayout(False)
        Me.pnlDamping.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbPLCLoop As System.Windows.Forms.ComboBox
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents txtFlow As System.Windows.Forms.TextBox
    Friend WithEvents txtSuction As System.Windows.Forms.TextBox
    Friend WithEvents txtDischarge As System.Windows.Forms.TextBox
    Friend WithEvents txtTemperature As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtCh4 As System.Windows.Forms.TextBox
    Friend WithEvents txtCh3 As System.Windows.Forms.TextBox
    Friend WithEvents txtCh2 As System.Windows.Forms.TextBox
    Friend WithEvents txtCh1 As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As Label
    Friend WithEvents btnAveRaw As Button
    Friend WithEvents pnlDamping As Panel
    Friend WithEvents Label23 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents lblAveRaw As Label
#End Region
End Class