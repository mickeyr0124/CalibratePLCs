Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Text

'<ComClass(PLCRoutines.ClassId, PLCRoutines.InterfaceId, PLCRoutines.EventsId)>
Public Class PLCRoutines

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    '    Public Const ClassId As String = "3e828a13-c00d-4622-be0c-ae685de45571"
    '    Public Const InterfaceId As String = "9c880d32-5e04-43fc-84d2-e0b002089f58"
    '    Public Const EventsId As String = "8d1b49f4-f0d7-4975-a115-b96c1bf9b2d3"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    'communicate with PLCs

    'returns the data from the PLC
    Public Structure PLCDataStruct
        Public Flow As Single
        Public SuctionPressure As Single
        Public DischargePressure As Single
        Public Temperature As Single

        Public ValvePosition As Single

        Public TC1 As Single
        Public TC2 As Single
        Public TC3 As Single
        Public TC4 As Single

        Public AI1 As Single
        Public AI2 As Single
        Public AI3 As Single
        Public AI4 As Single

        Public PCoef As Single
        Public ICoef As Single
        Public DCoef As Single

        Public SetPoint As Single
        Public InHg As Single

    End Structure

    ' true if the network interface can be initialized using the selected protocol
    Private mNetworkOk As Boolean
    Public ReadOnly Property NetworkOK() As Boolean
        Get
            Return mNetworkOk
        End Get
    End Property

    ' number of Host Ethernet devices found on the network
    Private mDeviceCount As Integer
    Public ReadOnly Property DeviceCount() As Integer
        Get
            Return mDeviceCount
        End Get
    End Property

    ' maximum number of devices you want to allow
    Private mMaxDevices As Int16 = 100
    Public Property MaxDevices As Int16
        Get
            Return mMaxDevices
        End Get
        Set(ByVal value As Int16)
            mMaxDevices = value
        End Set
    End Property
    'structure returned when searching network
    Structure PLCStruct
        Public PLCDeviceNo As Integer
        Public PLCType As String
        Public PLCMAC As String
        Public PLCIPAddress As String
        Public PLCNumber As String
        Public PLCName As String
    End Structure

    'list of messages back from search, etc
    Public PLCList As List(Of PLCStruct) = New List(Of PLCStruct)

    ' Global variables
    '
    ' return code from the SDK API calls
    Dim Rc As Integer

    ' Ethernet protocol transport
    Dim TP As New HEITransport

    'pointer to HEITransport structure
    'this structure will be located in the unmanaged heap
    Dim pTransportStructure As IntPtr

    ' array of Host Ethernet devices
    Dim aDevices(MaxDevices) As HEIDevice

    ' set to true if any Host Ethernet device is already open
    Dim DeviceOpen As Boolean

    ' this is the device the user selected from the list
    Public tDevice As Int16

    'this is the PLC number that is connected
    Public PLCDevice As String

    ' this is the type of device the user selcted
    Dim tDeviceType As String

    ' detail line that gets displayed in the listbox
    Dim DetailLine As String

    Dim ascii As New ASCIIEncoding

    '------------------------ Routines ------------------------
    Public Function NetworkInit(ByRef ErrorList As List(Of String)) As Boolean

        ' if the network interface has already been opened, close it
        '
        If NetworkOK = True Then
            Rc = HEICloseTransport(pTransportStructure)
            Rc = HEIClose()
        Else
            TP.Initialize()

            For x As Integer = 0 To MaxDevices - 1
                aDevices(x).Initialize()
            Next
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Initialize the Ethernet Driver
        '
        Rc = HEIOpen(HEIAPIVersion)
        If Rc <> 0 Then
            ErrorList.Add("Error " & Hex(Rc) & " trying to initialize the Ethernet driver")
        Else
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Initiaizize the Winsock protocol transport
            '
            TP.Transport = HEIT_WINSOCK

            'use ip addresses
            TP.Protocol = HEIP_IP

            ' The HEITransport structure has now been configured with the transport (HEIT_WINSOCK) and protocol (HEIP_IP or HEIP_IPX).
            ' The HEITransport structure must now be moved to the unmanaged heap, which is where the HEI32_3 dll resides.
            ' If the HEITransport structure is left in the managed heap, it will be moved as the .net garbage collector destroys unneeded variables.
            ' The DLL maintains pointers to the HEITransport structure in the aDevices array.  
            ' If the HEITransport structure is left in the managed heap, when the structure is relocated, the DLL pointers will no longer point to 
            ' the structure and the DLL will work improperly.  Note that any pointers maintained by managed code will be automatically readjusted by
            ' the garbage collector as variables are moved.  The garbage collector cannot adjust pointers in unmanaged memory.  
            ' In order to prevent the garbage collector from moving the structure, the structure will be moved to the unmanaged heap.   

            'Creat a memory buffer in the unmanaged heap and obtain a pointer to it.  
            pTransportStructure = Marshal.AllocHGlobal(Marshal.SizeOf(TP))

            'Copy the TP structure to the buffer and destroy the original structure in managed memory
            Marshal.StructureToPtr(TP, pTransportStructure, True)

            Rc = HEIOpenTransport(pTransportStructure, HEIAPIVersion, 0)

            If Rc <> 0 Then
                ErrorList.Add("Error " & Hex(Rc) & " trying to initialize the Winsock transport")
            Else
                mNetworkOk = True
            End If

        End If

        Return NetworkOK

    End Function

    Public Function ScanNetwork(ByRef plclist As List(Of PLCStruct), ByRef ErrorList As List(Of String)) As Boolean

        Dim List As List(Of String) = New List(Of String)
        Dim PLCst As PLCStruct = New PLCStruct

        mDeviceCount = MaxDevices

        ' The default timeout value for the Query functions is 600ms. You can call
        ' HEISetQueryTimeout() with a new Timeout value (in ms) to extend
        ' the timeout period.

        ' Do the Query
        '
        Rc = HEIQueryDevices(pTransportStructure, aDevices, mDeviceCount, HEIAPIVersion)
        Dim x As Integer
        Dim IPError, DDError, IDError As Boolean
        Dim dtBuffer(31) As Byte
        Dim dtSize As Short
        Dim tBuffer(255) As Byte
        Dim RSize As Short
        If Rc <> 0 Then
            ErrorList.Add("Error " & Hex(Rc) & " trying to query network")
        Else
            If DeviceCount > 0 Then
                ' Error Flags
                '
                DDError = False
                IPError = False
                IDError = False

                DetailLine = ""

                For x = 0 To DeviceCount - 1
                    ' Now lets display some relevant information about each device found
                    '
                    ' Open the device
                    '
                    Rc = HEIOpenDevice(pTransportStructure, aDevices(x), HEIAPIVersion, LongDevTimeout, DefDevRetrys, False)
                    If Rc <> 0 Then
                        DetailLine = "Error: " & Hex(Rc) & " " & ShowHEIErrorText(Rc)
                        ErrorList.Add(DetailLine)
                    Else
                        PLCst.PLCDeviceNo = x
                        PLCst.PLCMAC = Trim(GetMACFromBuffer(aDevices(x).Address))
                        ' check to see if user wants to filter out non-ecom devices
                        '
                        If DeviceIsECOM(aDevices(x)) = True Then

                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' First: the Module Type
                            ' See if the device has DT_TYPE_STRING data. This is a relatively new feature
                            ' so devices with old firmware won't have this. You'll need to use the "old"
                            ' method of manually constructing the device name.
                            '
                            Array.Clear(dtBuffer, 0, dtBuffer.Length)
                            dtSize = 32

                            Rc = HEIReadSetupData(aDevices(x), DT_TYPE_STRING, dtBuffer(0), dtSize)
                            If Rc = 0 Then
                                DetailLine = ascii.GetString(dtBuffer, 0, Array.IndexOf(dtBuffer, Byte.Parse("0"))).PadRight(16)
                                PLCst.PLCType = Trim(DetailLine)
                            Else
                                List.Add("Error obtaining MAC Address")
                            End If

                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' Second: the MAC address
                            ' it's in bytes 30 through 35 of the HEIDevice structure
                            '
                            'DetailLine = DetailLine & GetMACFromBuffer(aDevices(x).Address)

                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' Third: the IP address
                            ' (remove leading spaces)
                            '
                            RSize = 4

                            Rc = HEIReadSetupData(aDevices(x), DT_IP_ADDRESS, tBuffer(0), RSize)
                            If Rc <> 0 Then
                                IPError = True
                                DetailLine = "Error obtaining IP Address"
                                ErrorList.Add(DetailLine)
                            Else
                                PLCst.PLCIPAddress = Trim(Str(tBuffer(0)) & "." & Str(tBuffer(1)) & "." & Str(tBuffer(2)) & "." & Str(tBuffer(3)))
                            End If

                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' Fourth: the Module ID
                            '
                            Array.Clear(tBuffer, 0, tBuffer.Length)
                            RSize = 4

                            Rc = HEIReadSetupData(aDevices(x), DT_NODE_NUMBER, tBuffer(0), RSize)
                            If Rc <> 0 Then
                                IDError = True
                                ErrorList.Add("Error obtaining PLC ID")

                            Else
                                PLCst.PLCNumber = Trim(Str(tBuffer(0)).PadRight(5))
                            End If

                            ' Display the Node Name
                            '
                            System.Array.Clear(tBuffer, 0, tBuffer.Length)
                            RSize = 256

                            Rc = HEIReadSetupData(aDevices(x), DT_NODE_NAME, tBuffer(0), RSize)
                            If Rc <> 0 Then
                                ErrorList.Add("Error reading Module Name")

                            Else
                                Dim PLCTemp As String
                                PLCTemp = Trim(ascii.GetString(tBuffer, 0, Array.IndexOf(tBuffer, Byte.Parse("0"))))
                                PLCTemp = PLCTemp.Replace(" ", "")
                                For j As Integer = 1 To Len(PLCTemp)
                                    If IsNumeric(Mid(PLCTemp, j, 1)) Then
                                        PLCTemp = PLCTemp.Insert(j - 1, " ")
                                        Exit For
                                    End If
                                Next j

                                PLCst.PLCName = PLCTemp
                            End If

                            ' alert me if any errors occurred trying to read from the device
                            ''
                            If DDError = True Or IPError = True Or IDError = True Then
                                DetailLine = "   ! Error(s) occurred while reading device(s)"
                                DDError = False
                                IPError = False
                                IDError = False
                                ErrorList.Add(DetailLine)
                            End If
                        End If

                        plclist.Add(PLCst)

                        ' close this device
                        '
                        Rc = HEICloseDevice(aDevices(x))
                        If Rc <> 0 Then
                            ErrorList.Add("Error " & Hex(Rc) & " trying to close this device")
                        End If
                    End If
                Next x

            End If
        End If

        'sort list on plc name
        plclist.Sort(Function(a, b) a.PLCNumber.CompareTo(b.PLCNumber))
        Return True
    End Function

    Public Function Disconnect(ErrorList As List(Of String)) As Boolean

        ' Close the open device
        '
        Rc = HEICloseDevice(aDevices(tDevice))
        If Rc <> 0 Then
            ErrorList.Add("Error " & Hex(Rc) & " trying to close device" & Str(tDevice))
        Else
            DeviceOpen = False

            tDeviceType = ""
        End If
    End Function

    Public Function Connect(ErrorList As List(Of String)) As Boolean

        ' tdevice is the device the user has selected
        '
        DeviceOpen = False

        Disconnect(ErrorList)

        ' Open the device
        '
        Rc = HEIOpenDevice(pTransportStructure, aDevices(tDevice), HEIAPIVersion, DefDevTimeout, DefDevRetrys, False)
        Dim RSDBuffer(31) As Byte
        Dim RSDSize As Int16
        Dim RSDError As Boolean
        Dim VISize As Int16
        Dim tBuffer(255) As Byte
        Dim RSize As Int16

        Dim VIBuffer As VersionInfoDef = Nothing
        VIBuffer.Initialize()
        Dim pVIBuffer As IntPtr

        If Rc <> 0 Then
            ErrorList.Add("Error " & Hex(Rc) & " trying to open the device")

        Else
            DeviceOpen = True

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Display the Device Type
            '

            System.Array.Clear(RSDBuffer, 0, RSDBuffer.Length)
            RSDSize = 32

            Rc = HEIReadSetupData(aDevices(tDevice), DT_TYPE_STRING, RSDBuffer(0), RSDSize)
            If Rc = 0 Then
                ErrorList.Add("Device : " & ascii.GetString(RSDBuffer, 0, Array.IndexOf(RSDBuffer, Byte.Parse("0"))))
            Else
                RSDError = False
                tDeviceType = BuildTypeString(aDevices(tDevice), RSDError)
                If RSDError = False Then
                    ErrorList.Add("Device :" & tDevice)
                End If
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Display the boot loader version and the firmware version
            '
            VISize = 50

            'Creat a memory buffer in the unmanaged heap and obtain a pointer to it.  
            pVIBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(VIBuffer))

            'Copy the VIBuffer structure to the buffer 
            Marshal.StructureToPtr(VIBuffer, pVIBuffer, True)

            'pass the pointer to the function to obtain version information
            Rc = HEIReadVersionInfo(aDevices(tDevice), pVIBuffer, VISize)

            'copy the memory buffer back to the VIBuffer structure located in managed heap
            VIBuffer = Marshal.PtrToStructure(pVIBuffer, GetType(VersionInfoDef))

            'free the memory buffer in the unmanaged heap
            Marshal.FreeHGlobal(pVIBuffer)

            If Rc <> 0 Then
                ErrorList.Add("Error " & Hex(Rc) & " trying to read the version information")
            Else
                If VIBuffer.SizeofVersionInfo <> 0 Then
                    'DisplayList.Items.Add("Booter : " &
                    'Format(VIBuffer.BootVersion.MajorVersion, "0") & "." &
                    'Format(VIBuffer.BootVersion.MinorVersion, "0") & "." &
                    'Format(VIBuffer.BootVersion.BuildVersion, "000"))
                    'DisplayList.Items.Add("Version: " &
                    'Format(VIBuffer.OSVersion.MajorVersion, "0") & "." &
                    'Format(VIBuffer.OSVersion.MinorVersion, "0") & "." &
                    'Format(VIBuffer.OSVersion.BuildVersion, "000"))
                End If
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Display the Module ID (Node Number)
            '
            System.Array.Clear(tBuffer, 0, tBuffer.Length)
            RSize = 4

            Rc = HEIReadSetupData(aDevices(tDevice), DT_NODE_NUMBER, tBuffer(0), RSize)
            If Rc <> 0 Then
                ErrorList.Add("Error reading Module ID:")

            Else


            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Display the Node Name
            '
            System.Array.Clear(tBuffer, 0, tBuffer.Length)
            RSize = 256

            Rc = HEIReadSetupData(aDevices(tDevice), DT_NODE_NAME, tBuffer(0), RSize)
            If Rc <> 0 Then
                ErrorList.Add("Error reading Module Name")

            Else
                PLCDevice = ascii.GetString(tBuffer, 0, Array.IndexOf(tBuffer, Byte.Parse("0")))

            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Display the Module Description
            '
            System.Array.Clear(tBuffer, 0, tBuffer.Length)
            RSize = 256

            Rc = HEIReadSetupData(aDevices(tDevice), DT_DESCRIPTION, tBuffer(0), RSize)
            If Rc <> 0 Then
                ErrorList.Add("Error reading Module Description")

            Else
                'ErrorList.Add("Desc   : " & ascii.GetString(tBuffer, 0, Array.IndexOf(tBuffer, Byte.Parse("0"))))

            End If


        End If

        Return DeviceOpen
    End Function
    Public Function WriteData(DataAddressIn As String, DataLength As Int16, ByRef PLCData As String, ErrorList As List(Of String)) As Boolean
        Dim DataType As Byte = &H31S
        Dim bwrite As Boolean = True

        Dim DataAddress As Int16
        'convert from octal address 
        Dim j As Integer = 0
        For k As Integer = 1 To Len(DataAddressIn)
            j = j + Val(Mid$(DataAddressIn, Len(DataAddressIn) - k + 1, 1)) * (8 ^ (k - 1))
        Next k

        DataAddress = j + 1

        Dim ByteBuffer(255) As Byte

        System.Array.Clear(ByteBuffer, 0, ByteBuffer.Length)

        '       If bwrite = True Then

        Dim s As String
        Dim U() As Byte


        s = "0000" & (CInt(PLCData))
        s = s.Substring(s.Length - 4, 4)
        s = s.Substring(s.Length - 2, 2) & s.Substring(0, 2)
        U = System.Text.Encoding.ASCII.GetBytes(s.ToUpper)


        Dim i As Integer
        For i = 0 To (Len(s) - 1)
            ByteBuffer(i) = U(i)
        Next i

        Dim DL As Integer = HexConvert(ByteBuffer, 2)

        Rc = HEICCMRequest(aDevices(tDevice), bwrite, DataType, DataAddress, DL, ByteBuffer(0))

        If Rc <> 0 Then
            ErrorList.Add("Error " & Hex(Rc) & " (" & ShowHEIErrorText(Rc) & ") performing writing data")

        Else

        End If

    End Function
    Private Function HexConvert(ByRef Buffer() As Byte, Count As Integer) As Integer
        Dim i As Integer

        'convert each character code
        For i = 0 To (Count * 2) - 1

            'have to manually process HEX character digits
            If (Buffer(i) > 64) And (Buffer(i) < 71) Then
                Select Case Buffer(i)
                    Case 65 'A
                        Buffer(i) = 10
                    Case 66 'B
                        Buffer(i) = 11
                    Case 67 'C
                        Buffer(i) = 12
                    Case 68 'D
                        Buffer(i) = 13
                    Case 69 'E
                        Buffer(i) = 14
                    Case 70 'F
                        Buffer(i) = 15
                End Select

            Else
                'numeric digits are much easier
                Buffer(i) = Buffer(i)

            End If

        Next i

        'Now pack two HEX characters into a byte
        Dim Z As Integer
        Dim zz As Integer
        Z = 0
        For i = 0 To (Count * 2) - 1 Step 2
            zz = (Val(Chr(Buffer(i))) * 16) + Val(Chr(Buffer(i + 1)))
            Buffer(Z) = zz
            Z = Z + 1
        Next i

        'Now clear the remainder of the byte array - just to be neat and complete
        For i = Z To (Count * 2) - 1
            Buffer(i) = 0
        Next i

        HexConvert = zz

    End Function

    Function ConvertToReal(DataAddressIn As String) As Single
        Dim sFloat As Single
        Dim lngNum As Long
        Dim blnSign As Boolean
        Dim I As Integer
        Dim bWrite As Single = 0

        Dim DataType As Byte = &H31S

        Dim DataAddress As Int16
        'convert from octal address 
        Dim j As Integer = 0
        For k As Integer = 1 To Len(DataAddressIn)
            j = j + Val(Mid$(DataAddressIn, Len(DataAddressIn) - k + 1, 1)) * (8 ^ (k - 1))
        Next k

        DataAddress = j + 1

        Dim ByteBuffer(255) As Byte

        System.Array.Clear(ByteBuffer, 0, ByteBuffer.Length)

        Dim DataLength As Int16 = 4

        Rc = HEICCMRequest(aDevices(tDevice), bWrite, DataType, DataAddress, DataLength, ByteBuffer(0))

        lngNum = 0

        If ByteBuffer(3) > 127 Then
            ByteBuffer(3) = ByteBuffer(3) - 128
            blnSign = True
        Else
            blnSign = False
        End If

        For I = 0 To 3
            lngNum = lngNum + (ByteBuffer(I) * 256 ^ I)
        Next I

        sFloat = ConvertLongToSingle(ByteBuffer)

        If blnSign Then
            sFloat = -sFloat
        End If

        ConvertToReal = sFloat

    End Function
    'Function ConvertToLong(ByteBuffer() As Byte) As Long
    Private Function ConvertToLong(DataAddressIn As String) As Long
        Dim I As Integer
        Dim S As String

        Dim bWrite As Single = 0

        Dim DataType As Byte = &H31S

        Dim DataAddress As Int16

        'convert from octal address 
        Dim j As Integer = 0
        For k As Integer = 1 To Len(DataAddressIn)
            j = j + Val(Mid$(DataAddressIn, Len(DataAddressIn) - k + 1, 1)) * (8 ^ (k - 1))
        Next k

        DataAddress = j + 1

        Dim ByteBuffer(255) As Byte

        System.Array.Clear(ByteBuffer, 0, ByteBuffer.Length)

        Dim DataLength As Int16 = 2

        Rc = HEICCMRequest(aDevices(tDevice), bWrite, DataType, DataAddress, DataLength, ByteBuffer(0))
        Dim l, h As String

        If Rc <> 0 Then
            '            ErrorList.Add("Error " & Hex(Rc) & " (" & ShowHEIErrorText(Rc) & ") performing CCM Request")

        Else
            DetailLine = ""

            If bWrite = False Then
                For I = 0 To DataLength - 1
                    l = Hex(ByteBuffer(I)).PadLeft(2, "0")
                    h = Hex(ByteBuffer(I + 1)).PadLeft(2, "0")
                    DetailLine = DetailLine & h & l & " "
                    I += 1
                Next I
                ConvertToLong = DetailLine
            Else
                '                ErrorList.Add("Successfully wrote" & Str("2 WORD(s) of data"))

            End If

        End If

    End Function
    Private Function ConvertLongToSingle(ByVal buf() As Byte) As Single
        'long is ieee representation of floating point
        'convert to string

        Return (BitConverter.ToSingle(buf, 0))

    End Function
    Public Function GetPLCData() As PLCDataStruct
        'gets the data and stores it in the structure

        Dim PLCData As PLCDataStruct = New PLCDataStruct

        PLCData.Flow = ConvertToReal("4050")
        PLCData.SuctionPressure = Convert.ToSingle(ConvertToReal("4052") / 10)
        PLCData.DischargePressure = ConvertToReal("4054")
        PLCData.Temperature = ConvertToReal("4056")

        PLCData.ValvePosition = Convert.ToSingle(ConvertToLong("2004"))

        PLCData.TC1 = Convert.ToSingle(ConvertToLong("2200") / 10)
        PLCData.TC2 = Convert.ToSingle(ConvertToLong("2202") / 10)
        PLCData.TC4 = Convert.ToSingle(ConvertToLong("2204") / 10)
        PLCData.TC2 = Convert.ToSingle(ConvertToLong("2206") / 10)

        PLCData.AI1 = ConvertToReal("4060").ToString
        PLCData.AI2 = ConvertToReal("4062").ToString
        PLCData.AI3 = ConvertToReal("4064").ToString
        PLCData.AI4 = ConvertToReal("4066").ToString

        PLCData.PCoef = Convert.ToSingle(ConvertToLong("2510") / 100)
        PLCData.ICoef = Convert.ToSingle(ConvertToLong("2511") / 100)
        PLCData.DCoef = Convert.ToSingle(ConvertToLong("2512") / 100)

        PLCData.SetPoint = Convert.ToSingle(ConvertToLong("4035"))
        PLCData.InHg = Convert.ToSingle(ConvertToLong("1460") / 100)

        Return PLCData
    End Function

    Private Function DeviceIsECOM(ByRef Device As HEIDevice) As Boolean

        Dim DDBuffer As New DeviceDef
        DDBuffer.Initialize()
        Dim DDSize As Integer
        DDSize = 42
        Dim eRc As Integer

        eRc = HEIReadDeviceDef(Device, DDBuffer.device(0), DDSize)
        If eRc = 0 And DDBuffer.device(2) = MT_ECOM Then
            DeviceIsECOM = True
        Else
            DeviceIsECOM = False
        End If
    End Function

    Private Function GetMACFromBuffer(ByRef Buffer() As Byte) As String

        Dim y As Int16, t As String

        For y = 30 To 35
            If Buffer(y) = 0 Then
                t = t + "00 "
            Else
                'make sure each piece of the address has two digits by adding a leading 0 to
                'single digit values
                If Buffer(y) < 16 Then
                    t = t + "0" + Hex(Buffer(y)) + " "
                Else
                    t = t + Hex(Buffer(y)) + " "
                End If
            End If
        Next y

        GetMACFromBuffer = t

    End Function

    Private Function ShowHEIErrorText(ByRef ErrorCode As Integer) As String

        Select Case ErrorCode
            Case 32769
                ShowHEIErrorText = "HEI Version Mismatch"

            Case 32771
                ShowHEIErrorText = "Invalid Device"

            Case 32772
                ShowHEIErrorText = "Data Buffer Too Small"

            Case 32774
                ShowHEIErrorText = "Timeout Error"

            Case 32775
                ShowHEIErrorText = "Unsupported Protocol"

            Case 32776
                ShowHEIErrorText = "IP Address not Initialized"

            Case 32778
                ShowHEIErrorText = "IPX Transport Not Initialized"

            Case 32779
                ShowHEIErrorText = "Error Opening IPX Socket"

            Case 32784
                ShowHEIErrorText = "Invalid Request"

            Case 32787
                ShowHEIErrorText = "Data Too Large"

            Case 41079
                ShowHEIErrorText = "Invalid Data"

            Case Else
                ShowHEIErrorText = "unknown"

        End Select

    End Function
    Private Function BuildTypeString(ByRef Device As HEIDevice, ByRef DDError As Boolean) As String

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' First
        ' Read the Device Definition Data
        '
        Dim DDBuffer As New DeviceDef
        DDBuffer.Initialize()

        Dim DDSize As Integer
        Dim t As String = ""
        Dim eRc As Integer


        DDSize = 42

        eRc = HEIReadDeviceDef(Device, DDBuffer.device(0), DDSize)
        If eRc <> 0 Then
            DDError = True

        Else
            ' device byte 0 determines the Module Family
            '
            ' device byte 2 determines the Module Type, MSB determines media type
            '
            Select Case DDBuffer.device(2)
                Case MT_EBC
                    Select Case DDBuffer.device(0)
                        Case MF_005
                            t = "H0-"
                        Case MF_205
                            t = "H2-"
                        Case MF_305
                            t = "H3-"
                        Case MF_405
                            t = "H4-"
                        Case MF_TERM
                            t = "T1H-"
                    End Select

                    If (DDBuffer.device(2) And &H80) = &H80 Then
                        t = t + "EBC-F"
                    Else
                        t = t + "EBC"
                    End If

                Case MT_ECOM
                    Select Case DDBuffer.device(0)
                        Case MF_005
                            t = "H0-"
                        Case MF_205
                            t = "H2-"
                        Case MF_305
                            t = "H3-"
                        Case MF_405
                            t = "H4-"
                    End Select

                    If (DDBuffer.device(2) And &H81) = &H81 Then
                        t = t + "ECOM-F"
                    Else
                        t = t + "ECOM"
                    End If

                Case MT_WPLC
                    Select Case DDBuffer.device(0)
                        Case MF_005
                            t = "H0-"
                        Case MF_205
                            t = "H2-"
                        Case MF_305
                            t = "H3-"
                        Case MF_405
                            t = "H4-"
                    End Select

                    t = t + "WPLC"

                Case MT_DRIVE
                    Select Case DDBuffer.device(0)
                        Case MF_100_SERIES
                            t = "HA-EDRV2"
                        Case MF_J300
                            t = "HA-EDRV3"
                        Case MF_300_Series
                            t = "HA-EDRV"
                        Case MF_GS
                            t = "GS-EDRV"
                    End Select

                Case MT_ERMA
                    Select Case DDBuffer.device(0)
                        Case MF_005
                            t = "H0-"
                        Case MF_205
                            t = "H2-"
                        Case MF_305
                            t = "H3-"
                        Case MF_405
                            t = "H4-"
                    End Select

                    If (DDBuffer.device(2) And &H84) = &H84 Then
                        t = t + "ERM-F"
                    Else
                        t = t + "ERM"
                    End If

                Case MT_CTRIO
                    t = t + "CTRIO"

                Case MT_AVG_DISP
                    t = t + "EZTOUCH"

                Case MT_PBC
                    t = t + "PBC"

                Case MT_PBCC
                    t = t + "PBCC"

                Case MT_UNK
                    t = t + "UNKNOWN"
            End Select

        End If

        If DDError = True Then
            BuildTypeString = " ??????????? "
        Else
            BuildTypeString = t
        End If

    End Function

    Public Function SortPLCList(PLCList As List(Of PLCStruct)) As List(Of PLCStruct)
        PLCList.Sort(Function(x As PLCStruct, y As PLCStruct)
                         If x.PLCName Is Nothing And y.PLCName Is Nothing Then
                             Return 0
                         ElseIf x.PLCName Is Nothing Then
                             Return -1
                         ElseIf y.PLCName Is Nothing Then
                             Return 1
                         Else
                             Return x.PLCName.CompareTo(y.PLCName)
                         End If
                     End Function)

        Return PLCList
    End Function

End Class


