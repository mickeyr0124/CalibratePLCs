Option Strict Off
Option Explicit On
Imports System.Collections.Generic

Friend Class frmPLC
    Inherits System.Windows.Forms.Form
    ' v 1.0.1 MHR 11/02/05
    ' Changed path from checps1 to checpsa
    ' v 1.0.2 NHR 8/1/2011
    ' Changed path to TEI-Main-01

    Dim FirstTime As Boolean
    'instantiate new class of plc communications
    Dim PLCCm As PLCRoutines = New PLCRoutines
    Dim PLCData As PLCRoutines.PLCDataStruct

    Dim PLCDict As Dictionary(Of String, String) = New Dictionary(Of String, String)


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

        FirstTime = True

        Timer1.Enabled = True

    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick
        'Static count As Integer
        'If FirstTime Then
        '    count = count + 1
        '    If count > 2 Then
        '        count = 2
        '        cmbPLCLoop.SelectedIndex = 0
        '        cmbPLCLoop_SelectedIndexChanged(cmbPLCLoop, New System.EventArgs())
        '        FirstTime = False
        '    End If
        'End If

        PLCData = PLCCm.GetPLCData()
        updateData(PLCData)

    End Sub
    Private Sub updateData(PLCData As PLCRoutines.PLCDataStruct)

        txtFlow.Text = Format(PLCData.Flow, "###0.0")
        txtSuction.Text = Format(Convert.ToSingle(PLCData.SuctionPressure), "###0.0")
        txtDischarge.Text = Format(PLCData.DischargePressure, "###0.0")
        txtTemperature.Text = Format(PLCData.Temperature, "###0.0")
        txtCh1.Text = Format(PLCData.AI1, "###0.0")
        txtCh2.Text = Format(PLCData.AI2, "###0.0")
        txtCh3.Text = Format(PLCData.AI3, "###0.0")
        txtCh4.Text = Format(PLCData.AI4, "###0.0")

    End Sub

End Class