Imports System.Data.OleDb

Public Class GetPLCAddresses
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(PumpConnectionString As String)
        MyBase.New()
        mPumpConnectionString = PumpConnectionString
    End Sub

    Private mPumpConnectionString As String
    Public Property PumpConnectionString() As String
        Get
            Return mPumpConnectionString
        End Get
        Set(ByVal value As String)
            mPumpConnectionString = value
        End Set
    End Property

    Private mAddressArray As String(,)
    Public ReadOnly Property AddressArray() As String(,)
        Get
            Return mAddressArray
        End Get
    End Property

    Private mArrayPLCsNumber As Integer
    Public ReadOnly Property ArrayPLCsNumber As Integer
        Get
            Return mArrayPLCsNumber
        End Get
    End Property

    Public Sub GetAddresses()
        Dim cnPump As OleDb.OleDbConnection = New OleDb.OleDbConnection
        cnPump.ConnectionString = mPumpConnectionString
        cnPump.Open()

        Dim SQLString As String = "SELECT Description, PLCAddressInfo FROM PLCAddresses"
        Dim da As New OleDbDataAdapter(SQLString, cnPump)
        Dim dt As New DataTable
        da.Fill(dt)

        Dim AddressArray As String(,) = Nothing
        ReDim AddressArray(1, dt.Rows.Count - 1)
        For i As Integer = 0 To dt.Rows.Count - 1
            AddressArray(0, i) = dt.Rows(i)("Description")
            AddressArray(1, i) = dt.Rows(i)("PLCAddressInfo")
        Next

        mAddressArray = AddressArray
        mArrayPLCsNumber = mAddressArray.GetUpperBound(1)

    End Sub

End Class


