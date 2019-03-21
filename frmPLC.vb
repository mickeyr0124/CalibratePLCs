Option Strict Off
Option Explicit On
Imports System.Collections.Generic

Friend Class frmPLC
    Inherits System.Windows.Forms.Form
    ' v 1.0.1 MHR 11/02/05
    ' Changed path from checps1 to checpsa
    ' v 1.0.2 MHR 8/1/2011
    ' Changed path to TEI-Main-01
    ' v 1.0.3 MHR 3/21/2019
    ' Changed to vb.net and added averaging

    'instantiate new class of plc communications
    Dim PLCCm As PLCRoutines = New PLCRoutines
    Dim PLCData As PLCRoutines.PLCDataStruct

    Dim PLCDict As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Dim AveData As AverageData = New AverageData


    Private Sub cmbPLCLoop_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbPLCLoop.SelectedIndexChanged

        Dim ErrorList As List(Of String) = New List(Of String)
        'Change the PLC that we're looking at

        PLCCm.tDevice = Convert.ToInt16(PLCDict(cmbPLCLoop.SelectedItem))

        PLCCm.Connect(ErrorList)

        PLCData = PLCCm.GetPLCData()
        updateData(PLCData)

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        End
    End Sub

    Private Sub frmPLC_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        If CStr(My.Application.Info.Version.Major) <> "" Then
            lblVersion.Text = "Version " & CStr(My.Application.Info.Version.Major) & "." & My.Application.Info.Version.Minor
        End If


        'list of plcs available
        Dim PLCList As List(Of PLCRoutines.PLCStruct) = New List(Of PLCRoutines.PLCStruct)
        'error list to collect errors and report them
        Dim ErrorList As List(Of String) = New List(Of String)


        'initialize the PLC network
        If Not (PLCCm.NetworkInit(ErrorList)) Then
            Debug.Print("Errors found in Network Initialization")
            For i As Integer = 0 To ErrorList.Count - 1
                Debug.Print(ErrorList(i))
            Next
        End If

        Debug.Print(PLCCm.NetworkOK)

        'find all plcs in the network
        If Not PLCCm.ScanNetwork(PLCList, ErrorList) Then
            Debug.Print("Errors found in Network Initialization")
            For i As Integer = 0 To ErrorList.Count - 1
                Debug.Print(ErrorList(i))
            Next
        End If

        PLCCm.SortPLCList(PLCList)

        'make a dictionary to translate from cmbPLCLoop to PLCDevice
        'also add plcName to cmbPLCLoop
        For i As Integer = 0 To PLCList.Count - 1
            PLCDict.Add(PLCList(i).PLCName.ToString, PLCList(i).PLCDeviceNo.ToString)
            Me.cmbPLCLoop.Items.Add(PLCList(i).PLCName.ToString)
        Next


        cmbPLCLoop.SelectedIndex = 0
        cmbPLCLoop_SelectedIndexChanged(cmbPLCLoop, New System.EventArgs())

        AveData.Multiplier = 0
        Me.btnAveRaw_Click(eventSender, eventArgs)
        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick
        PLCData = PLCCm.GetPLCData()
        updateData(PLCData)
    End Sub
    Private Sub updateData(PLCData As PLCRoutines.PLCDataStruct)

        'calculate average/raw data
        ' if multiplier = 0, calc raw
        txtFlow.Text = AveData.CalcAveData(txtFlow.Text, PLCData.Flow)
        txtSuction.Text = AveData.CalcAveData(txtSuction.Text, PLCData.SuctionPressure)
        txtDischarge.Text = AveData.CalcAveData(txtDischarge.Text, PLCData.DischargePressure)
        txtTemperature.Text = AveData.CalcAveData(txtTemperature.Text, PLCData.Temperature)
        txtCh1.Text = AveData.CalcAveData(txtCh1.Text, PLCData.AI1)
        txtCh2.Text = AveData.CalcAveData(txtCh2.Text, PLCData.AI2)
        txtCh3.Text = AveData.CalcAveData(txtCh3.Text, PLCData.AI3)
        txtCh4.Text = AveData.CalcAveData(txtCh4.Text, PLCData.AI4)

    End Sub

    Private Sub btnAveRaw_Click(sender As Object, e As EventArgs) Handles btnAveRaw.Click
        If Me.btnAveRaw.Text = "Raw Data" Then
            Me.btnAveRaw.Text = "Average Data"
            pnlDamping.Visible = False
            Me.lblAveRaw.Text = "Displaying Raw Data"
            AveData.Multiplier = 0

        Else
            Me.btnAveRaw.Text = "Raw Data"
            pnlDamping.Visible = True
            Me.lblAveRaw.Text = "Displaying Averaged Data"
            Me.TrackBar1_Scroll(sender, e)
        End If

        'center the label horizontally
        Dim frm As Form = Me
        Dim fw As Int16 = frm.Width
        Dim lw As Int16 = Me.lblAveRaw.Width
        Me.lblAveRaw.Left = (fw - lw) / 2
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        AveData.Multiplier = TrackBar1.Value / 10
    End Sub
End Class