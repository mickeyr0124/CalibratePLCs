Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices
Module PLCInterface

    Private Declare Sub CopyMemorySingleToInteger Lib "kernel32" Alias "RtlMoveMemory" (ByRef Destination As Single, ByRef Source As Integer, ByVal Length As Integer)

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' HEI API Defines
    '
    Const HEIAPIVersion As Byte = 3
    Const HEIAPIVersionString As String = "3"
    Const HEIP_HOST As Integer = 1
    Const HEIP_IPX As Integer = 2
    Const HEIP_IP As Integer = 3
    Const HEIT_HOST As Integer = 1
    Const HEIT_IPX As Integer = 2
    Const HEIT_WINSOCK As Integer = 4

    Const DefDevTimeout As Integer = 50 ' value in milliseconds
    Const LongDevTimeout As Integer = DefDevTimeout + 200 ' value in milliseconds
    Const DefDevRetrys As Byte = 3
    Const DefDevUseAddressedBroadcast As Boolean = False

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Error Messages
    '
    Const HEIE_NULL As Integer = 0 ' No error
    Const HEIE_FIRST_ERROR As Integer = &H8000 ' Number for first error
    Const HEIE_LAST_ERROR As Integer = &HFFFF ' Number for last error
    Const HEIE_NOT_IMPLEMENTED As Integer = &H8000 ' Function not implemented
    Const HEIE_VER_MISMATCH As Integer = &H8001 ' Version passed to function is not correct for the library
    Const HEIE_UNSUPPORTED_TRANSPORT As Integer = &H8002 ' Supplied transport is not supported
    Const HEIE_INVALID_DEVICE As Integer = &H8003 ' Supplied device is not valid
    Const HEIE_BUFFER_TOO_SMALL As Integer = &H8004 ' Supplied buffer is too small
    Const HEIE_ZERO_BYTES_RECEIVED As Integer = &H8005 ' Zero packets returned in the packet
    Const HEIE_TIMEOUT As Integer = &H8006 ' Timeout error
    Const HEIE_UNSUPPORTED_PROTOCOL As Integer = &H8007 ' Supplied protocol not supported
    Const HEIE_IP_ADDRESS_NOT_INITIALIZED As Integer = &H8008 ' The device's IP address has not been assigned
    Const HEIE_NULL_TRANSPORT As Integer = &H8009 ' No Transport specified
    Const HEIE_IPX_NOT_INITIALIZED As Integer = &H800A ' IPX Transport not installed
    Const HEIE_IPX_OPEN_SOCKET As Integer = &H800B ' Error opening IPX socket
    Const HEIE_NO_PACKET_DRIVER As Integer = &H800C ' No packet driver found
    Const HEIE_CRC_MISMATCH As Integer = &H800D ' CRC did not match
    Const HEIE_ALLOCATION_ERROR As Integer = &H800E ' Memory allocation failed
    Const HEIE_NO_IPX_CACHE As Integer = &H800F ' No cache has been allocated for IPX
    Const HEIE_INVALID_REQUEST As Integer = &H8010 ' Invalid CCM Request
    Const HEIE_NO_RESPONSE As Integer = &H8011 ' No response was available/requested
    Const HEIE_INVALID_RESPONSE As Integer = &H8012 ' Invalid format response was received
    Const HEIE_DATA_TOO_LARGE As Integer = &H8013 ' Given data is too large
    Const HEIE_LOAD_PROC_ERROR As Integer = &H8014 ' Error loading procedures
    Const HEIE_NOT_LOADED As Integer = &H8015 ' Attempted command before succesfull OpenTransport()
    Const HEIE_LOAD_ERROR As Integer = &H9000 ' Error on loading

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' If PASCAL_HEIOpenTransport()fails for WinSock, it will return one of the following errors:
    '
    ' &h8014    Error getting address from WinSock.DLL
    ' &h8100    System was out of memory, or executable file was corrupt, or sharing or network-protection error
    ' &h8101    Unused
    ' &h8102    file was not found
    ' &h8103    Path was not found
    ' &h8104    Unused
    ' &h8105    Attempt was made to dynamically link to a task, or there was a sharing or network-protection error
    ' &h8106    Library required separate data segments for each task
    ' &h8107    Unused
    ' &h8108    There was insufficient memory to start the application
    ' &h8109    Unused
    ' &h810A    Windows version was incorrect
    ' &h810B    Executable file was invalid, it's either not a Windows application or there was an error in the .EXE image.
    ' &h810C    Application was designed for a different operating system
    ' &h810D    Application was designed for MS-DOS 4.0
    ' &h810E    Type of executable was unknown
    ' &h810F    Attempt was made to load a real-mode application (developed for an earlier version of Windows )
    ' &h8110    Attempt was made to load a second instance of an application file containing multiple data segments that were not marked read-only
    ' &h8113    Attempt was made to load a compressed executable file. The file must be decompressed before it can be loaded
    ' &h8114    Dynamic-link library (DLL) file was invalid. One of the DLLs required to run this application was corrupt
    ' &h8115    Application requires Microsoft Windows 32-bit extensions
    '

    Const HEIE_INVALID_SLOT_NUMBER As Integer = 118
    Const HEIE_INVALID_DATA As Integer = 119
    Const HEIE_CHANNEL_FAILURE As Integer = 121 ' Analog I/O, broken transmitter alarm, for modules with
    '  one error bit for all channels
    Const HEIE_UNUSED_CHANNELS_EXIST As Integer = 122 ' Analog I/O, module jumpers set to disable some channels
    Const HEIE_INVALID_BASE_NUMBER As Integer = 135
    Const HEIE_INVALID_MODULE_TYPE As Integer = 136
    Const HEIE_INVALID_OFFSET As Integer = 137
    Const HEIE_BROKEN_TRANSMITTER As Integer = 139
    Const HEIE_INVALID_ADDRESS As Integer = 140
    Const HEIE_CHANNEL_FAILURE_MULTI As Integer = 142 ' Analog I/O, broken transmitter alarm, for modules with
    '  an error bit for each channel, e.g. F2-04RTD
    Const HEIE_SERIAL_SETUP_ERROR As Integer = 143
    Const HEIE_MODULE_ERROR As Integer = 152
    Const HEIE_MODULE_NOT_RESPONDING As Integer = 153 ' T1H-EBC, Hot-Swap, module removed
    Const HEIE_BASE_CHANGED As Integer = 154 ' T1H-EBC, Hot-Swap, module added
    Const HEIE_PARITY_ERROR As Integer = 156
    Const HEIE_FRAMING_ERROR As Integer = 157
    Const HEIE_OVER_RUN_ERROR As Integer = 158
    Const HEIE_BUFFER_OVERFLOW As Integer = 159
    Const HEIE_DRIVE_TRIP As Integer = 162 ' HA-EDRV2, Drive has tripped

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Terminator Hot-Swap Defines, Used in HEIRescanBase()
    '
    'Const RESCAN_LEAVE_IMAGE_RAM As Integer = 0 ' Don't clear the image RAM
    'Const RESCAN_CLEAR_IMAGE_RAM As Integer = 1 ' Clear the image RAM

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Warnings
    '
    Const HEIW_FIRST_WARNING As Integer = &H4000 ' Number for First Warning
    Const HEIW_LAST_WARNING As Integer = &H4FFF ' Number for Last Warning
    Const HEIW_RETRY As Integer = &H4000 ' One or More Retrys have occurred

    ' These are masks for values returned from HEIReadIO and/or HEIWriteIO and indicate that some
    ' error/warning/info/internal condition exists for some module in the base ( it could be an I/O
    ' module or it could be the ethernet module. The function HEIReadModuleStatus can then be used
    ' to retrieve the actual conditions. Note that more than one of the conditions can exist at any time.
    '
    Const MASK_DEVICE_ERROR As Integer = &H1000
    Const MASK_DEVICE_WARNING As Integer = &H2000
    Const MASK_DEVICE_INFO As Integer = &H4000

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Data Types
    '
    Const DT_IP_ADDRESS As Integer = &H10 ' 4-byte IP Address
    Const DT_SERIAL_SETTINGS As Integer = &H11 ' 8 Byte Serial Setup
    Const DT_RXWX_SETTINGS As Integer = &H15 ' 128 Byte setings for RXWX configuration
    Const DT_NODE_NAME As Integer = &H16 ' 256-byte Node Name
    Const DT_BASE_DEF As Integer = &H17 ' 512 Byte Base Definition (405 HEIWriteBaseDef)
    Const DT_NODE_NUMBER As Integer = &H20 ' 4-byte Node Number
    Const DT_MODULE_SETUP As Integer = &H24 ' 64-byte data from FLASH (see ModuleSetup structure)
    Const DT_DESCRIPTION As Integer = &H26 ' 256-byte Node Description
    Const DT_SUBNET_MASK As Integer = &H30 ' 4-byte Subnet Mask
    Const DT_TYPE_STRING As Integer = &H33 ' 32 byte string that contains the part number
    Const DT_LINK_MONITOR As Integer = &H8006 ' 256 Byte Link monitor setup

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Data Formats
    '
    Const DF_BIT_IN As Byte = &H3
    Const DF_BIT_OUT As Byte = &H4
    Const DF_WORD_IN As Byte = &H5
    Const DF_WORD_OUT As Byte = &H6
    Const DF_DWORD_IN As Byte = &H8
    Const DF_DWORD_OUT As Byte = &H9
    Const DF_BYTE_IN As Byte = &H10
    Const DF_BYTE_OUT As Byte = &H11
    Const DF_DOUBLE_IN As Byte = &H12
    Const DF_DOUBLE_OUT As Byte = &H13
    Const DF_FLOAT_IN As Byte = &H14
    Const DF_FLOAT_OUT As Byte = &H15
    Const DF_CONFIG As Byte = &H16

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Module Types
    '
    Const MT_EBC As Integer = 0 ' Ethernet Base Controller
    Const MT_ECOM As Integer = 1 ' Ethernet Communications Module
    Const MT_WPLC As Integer = 2 ' WinPLC
    Const MT_DRIVE As Integer = 3 ' Ethernet Drive Interface
    Const MT_ERMA As Integer = 4 ' Ethernet Remote Master
    Const MT_CTRIO As Integer = 5 ' Counter Interface
    Const MT_AVG_DISP As Integer = 6 ' AVG Display Adapter
    Const MT_PBC As Integer = 7 ' Profibus Slave controller
    Const MT_PBCC As Integer = 8 ' Profibus Slave Communications Module (PSCM)
    Const MT_UNK As Integer = &HFF ' Unknown

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Module Family defines for MT_EBC, MT_ECOM, MT_EDRV, MT_ERMA and MT_WPLC
    '
    Const MF_005 As Integer = 0 ' DL05 Series
    Const MF_205 As Integer = 2 ' DL205 Series
    Const MF_305 As Integer = 3 ' DL305 Series
    Const MF_405 As Integer = 4 ' DL405 Series
    Const MF_TERM As Integer = 10 ' Terminator I/O Series

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Module Family defines for MT_DRIVE
    '
    Const MF_100_SERIES As Integer = 1 ' Hitachi L100 and SJ100
    Const MF_J300 As Integer = 2 ' Hitachi J300
    Const MF_300_Series As Integer = 3 ' Hitachi SJ300
    Const MF_GS As Integer = 4 ' AutomationDirect GSx

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Module Family defines for MT_AVG_DISP
    '
    Const MF_EZ_TOUCH As Integer = 1 ' AVG EZ-Touch Ethernet Adapter

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Structure Definitions
    '
    Structure VersionInfoDef
        'SizeofVersionInfo As Byte
        'BootVersion As VersionDef
        'OSVersion As VersionDef
        'NumOSExtensions As Byte
        'OSExt(9) As VersionDef
        '<VBFixedArray(51)> Dim Version() As Byte
        <VBFixedArray(51), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=52)> Public Version() As Byte

        Public Sub Initialize()
            ReDim Version(51)
            For i As Integer = 0 To 51
                Version(i) = 0
            Next
        End Sub
    End Structure

    Structure DeviceDef
        'PLCFamily As Byte               ' 1= HA, 2= 205, 3= 305, 4= 405, 10= Terminator
        'Unused1 As Byte
        'ModuleType As Byte              ' 0= EBC, 1= ECOM, &H80= Fiber, 3= Drive, 4= ERM, 6= AVG
        'StatusCode As Byte
        'EthernetAddress(5) As Byte      ' Hardware Ethernet Address
        'RamSize As Integer              ' in K-byte increments
        'FlashSize As Integer            ' in K-byte increments
        'DipSettings As Byte             ' setting of the 8 element dip switch
        'MediaType As Byte               ' 0= 10Base-T,1= 10Base-F
        'EPFCount As Long                ' Early Power Fail Count - 405 only
        'Status As Byte                  '  Status:1 = Run Relay Status
        '                                '  Status:2 = Battery Low Indicator
        '                                '  Status:6 = unused bits
        'BattRamSize As Integer          ' size (in K-bytes) of the battery-backed RAM
        'ExtraDIPs As Byte               ' extra DIP switches on Terminator I/O
        'ModelNumber As Byte             ' used by WinPLC
        'EtherSpeed As Byte              ' 0 = 10Mbit, 1 = 100Mbit
        'PLDRev(1) As Byte               ' PLD revision number
        'unused(14) As Byte
        '<VBFixedArray(41)> Dim Device() As Byte ' 42-byte byte array (VB packs on 4-byte boundaries)
        <VBFixedArray(41), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=42)> Public Device() As Byte

        Public Sub Initialize()
            ReDim Device(41)
            For i As Integer = 0 To 41
                Device(i) = 0
            Next
        End Sub
    End Structure

     '
    Structure Encryption
        Dim Algorithm As Byte ' Algorithm to use for encryption: 0= No encryption, 1= private key encryption
        '<VBFixedArray(2)> Dim Unused1() As Byte ' Reserved
        '<VBFixedArray(59)> Dim Key() As Byte ' Encryption key (null terminated)

        <VBFixedArray(2), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=3)> Public Unused1() As Byte
        <VBFixedArray(59), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=60)> Public Key() As Byte

        Public Sub Initialize()
            ReDim Unused1(2)
            ReDim Key(59)
            For i As Integer = 0 To 2
                Unused1(i) = 3
            Next
            For i As Integer = 0 To 59
                Key(i) = 0
            Next
        End Sub
    End Structure

    Structure EnetAddress
        'union
        '{
        '   ' Use this for HEIP_HOST protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_UNSPEC == 0
        '       char Nodenum[6];            ' Ethernet network address
        '       unsigned short LANNum;      ' Lana number
        '   }  AddressHost;
        '
        '   ' Use this for HEIP_IPX protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_IPX == 6
        '       char Netnum[4];             ' Network Number
        '       char Nodenum[6];            ' Ethernet network address
        '       unsigned short Socket;      ' Socket Number == 0x7070
        '   } AddressIPX;
        '
        '   ' Use this for HEIP_IP protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_INET == 2
        '       unsigned short Port;        ' Port number == 0x7070
        '       union
        '       {
        '           struct { unsigned char b1, b2, b3, b4; } bAddr;  ' Byte Addressing
        '           struct { unsigned short w1, w2; } wAddr;         ' Word addressing
        '           unsigned long lAddr;                             ' DWord addressing
        '       } AddressingType;
        '       char Zero[8];               ' initialize to zeros
        '   } AddressIP;
        '
        '   struct
        '   {
        '       Byte    Commport;
        '       Byte    ByteSize;
        '       Byte    Parity;
        '       Byte    StopBits;
        '       Long    Baud;
        '       Long    *hLocal;
        '   } AddressSerial;
        '   BYTE Raw[20];
        '}Address
        '
        '<VBFixedArray(19)> Dim Address() As Byte
        <VBFixedArray(19), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=20)> Public Address() As Byte

        Public Sub Initialize()
            ReDim Address(19)
            For i As Integer = 0 To 19
                Address(i) = 0
            Next
        End Sub
    End Structure

    Structure HEITransport
        Dim Transport As Short
        Dim Protocol As Short
        Dim Encrypt As Encryption
        Dim SourceAddress As EnetAddress
        '<VBFixedArray(47)> Dim Reserved() As Byte
        <VBFixedArray(47), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=48)> Public Reserved() As Byte

        Public Sub Initialize()
            Encrypt.Initialize()
            SourceAddress.Initialize()
            ReDim Reserved(47)
            For i As Integer = 0 To 47
                Reserved(i) = 0
            Next
        End Sub
    End Structure

    Structure HEIDevice
        'union
        '{
        '   ' Use this for HEIP_HOST protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_UNSPEC == 0
        '       char Nodenum[6];            ' Ethernet network address
        '       unsigned short LANNum;      ' Lana number
        '   }  AddressHost;
        '
        '   ' Use this for HEIP_IPX protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_IPX == 6
        '       char Netnum[4];             ' Network Number
        '       char Nodenum[6];            ' Ethernet network address
        '       unsigned short Socket;      ' Socket Number == 0x7070
        '   } AddressIPX;
        '
        '   ' Use this for HEIP_IP protocol addressing
        '   struct
        '   {
        '       short Family;               ' AF_INET == 2
        '       unsigned short Port;        ' Port number == 0x7070
        '       union
        '       {
        '           struct { unsigned char b1, b2, b3, b4; } bAddr;  ' Byte Addressing
        '           struct { unsigned short w1, w2; } wAddr;         ' Word addressing
        '           unsigned long lAddr;                             ' DWord addressing
        '       } AddressingType;
        '       char Zero[8];               ' initialize to zeros
        '   } AddressIP;
        '
        '   struct
        '   {
        '       Byte    Commport;
        '       Byte    ByteSize;
        '       Byte    Parity;
        '       Byte    StopBits;
        '       Long    Baud;
        '       Long    *hLocal;
        '   } AddressSerial;
        '   BYTE Raw[20];
        '}Address
        '
        'wParam As Integer
        'dw_Param As Long
        'Timeout As Integer             ' Timeout value in ms
        'Retrys As Integer              ' Number of times to retry a transaction
        'EnetAddress(5) As Byte         ' Address placed here in HEIQueryDevice(s)
        'RetryCount As Integer          ' number of retrys that have occurred
        'BadCRCCount As Integer         ' number of packets received with a bad CRC
        'LatePacketCount As Integer     ' number of packets received after a timeout
        'ParallelPackets As Boolean     ' If set TRUE, (after HEIOpenDevice), will enable application
        ' to send multiple HEIReadIO, HEIWriteIO, HEICCMRequest or
        ' HEIKseqRequest transactions to different devices before waiting
        ' for any reponses. The application will then need to implement
        ' it's own timeout/retry mechanism while waiting on responses.
        ' Use HEIGetResponse() to see if responses have arrived.
        ' NOTE: do not send multiple requests to a single device.
        '
        'Internal Use Only -- Do Not Touch !!--
        'UseAddressedBroadcast As Boolean
        'UseBroadcast As Boolean
        'dwParam As Long
        'DataOffset As Integer
        'pTransport As Long
        'SizeOfData As Long
        'pData As Long
        'pBuffer As Long
        'LastAppVal As Integer
        'Byte UseProxy
        'Byte ProxyBase
        'Byte ProxySlot
        'Byte ProxyDevNum
        'Bytes Reserved(43)
        '<VBFixedArray(125)> Dim Address() As Byte ' 126-byte byte array (VB packs on 4-byte boundaries)
        <VBFixedArray(125), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=126)> Public Address() As Byte
        Public Sub Initialize()
            ReDim Address(125)
            For i As Integer = 0 To 125
                Address(i) = 0
            Next
        End Sub
    End Structure

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Host Ethernet APIs
    '
    Private Declare Function PASCAL_HEIOpen Lib "hei_pas" (ByVal HEIAPIVersion As Integer) As Integer

    Private Declare Function PASCAL_HEIClose Lib "hei_pas" () As Integer

    Private Declare Function PASCAL_HEIOpenTransport Lib "hei_pas" (ByRef pTransport As HEITransport, ByVal HEIAPIVersion As Integer, ByVal EnetAdress As Integer) As Integer

    Private Declare Function PASCAL_HEICloseTransport Lib "hei_pas" (ByRef pTransport As HEITransport) As Integer

    Private Declare Function PASCAL_HEIOpenDevice Lib "hei_pas" (ByRef pTransport As HEITransport, ByRef pDevice As HEIDevice, ByVal HEIAPIVersion As Integer, ByVal Timeout As Integer, ByVal Retrys As Integer, ByVal UseAddressedBroadcast As Boolean) As Integer

    Private Declare Function PASCAL_HEICloseDevice Lib "hei_pas" (ByRef pDevice As HEIDevice) As Integer

    Private Declare Function PASCAL_HEIQueryDevices Lib "hei_pas" (ByRef pTransport As HEITransport, ByRef pDevice As HEIDevice, ByRef pNumDevices As Integer, ByVal HEIAPIVersion As Integer) As Integer

    Private Declare Function PASCAL_HEIQueryDeviceData Lib "hei_pas" (ByRef pTransport As HEITransport, ByRef pDevice As HEIDevice, ByRef pNumDevices As Integer, ByVal HEIAPIVersion As Integer, ByVal DataType As Integer, ByRef pData As Byte, ByVal SizeOFData As Integer) As Integer

    Private Declare Function PASCAL_HEIReadSetupData Lib "hei_pas" (ByRef pDevice As HEIDevice, ByVal SetupType As Integer, ByRef pData As Byte, ByRef pSizeOfData As Integer) As Integer

    Private Declare Function PASCAL_HEIWriteSetupData Lib "hei_pas" (ByRef pDevice As HEIDevice, ByVal SetupType As Integer, ByRef pData As Byte, ByVal SizeOFData As Integer) As Integer

    Private Declare Function PASCAL_HEIReadDeviceDef Lib "hei_pas" (ByRef pDevice As HEIDevice, ByRef pModuleDefInfo As DeviceDef, ByVal pSizeOfModuleDefInfo As Integer) As Integer

    Private Declare Function PASCAL_HEIReadVersionInfo Lib "hei_pas" (ByRef pDevice As HEIDevice, ByRef pVersionInfo As VersionInfoDef, ByRef pVersionInfoSize As Integer) As Integer

    Private Declare Function PASCAL_HEIReadBaseDef Lib "hei_pas" (ByRef pDevice As HEIDevice, ByRef pData As Byte, ByRef pDataSize As Integer) As Integer

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' ECOM Specific APIs
    '
    Private Declare Function PASCAL_HEICCMRequest Lib "hei_pas" (ByRef pDevice As HEIDevice, ByVal bWrite As Integer, ByVal DataType As Byte, ByVal Address As Integer, ByVal pDataLen As Integer, ByRef pData As Byte) As Integer

    Private Declare Function PASCAL_HEIKSEQRequest Lib "hei_pas" (ByRef pDevice As HEIDevice, ByVal DataLen As Integer, ByRef pData As Byte, ByRef pReturnDataLen As Integer) As Integer


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Global variables
    '
    ' return code from the SDK API calls
    Public Rc As Integer

    ' Ethernet protocol transport
    Public TP As HEITransport

    ' true if the network interface can be initialized using the selected protocol
    Public NetworkOK As Boolean

    ' maximum number of devices you want to allow
    Const MAXDEVICES As Integer = 100

    ' array of Host Ethernet devices
    Public aDevices(MAXDEVICES) As HEIDevice

    ' number of Host Ethernet devices found on the network
    Public DeviceCount As Integer

    ' set to true if any Host Ethernet device is already open
    Public DeviceOpen As Boolean

    ' this is the device the user selected from the list
    Public tDevice As Integer

    ' this is the type of device the user selcted
    Public tDeviceType As String

    ' detail line that gets displayed in the listbox
    Public DetailLine As String

    Public bWrite As Integer
    Public DataType As Byte
    Public DataAddress As Integer
    Public DataLength As Integer
    Public ByteBuffer(255) As Byte
    Public CCM_CommandRead As Boolean
    Public CCM_CommandWrite As Boolean
    Public CCM_DataType31 As Boolean

    Public IPAddress(MAXDEVICES) As String
    Public NodeNumber(MAXDEVICES) As String
    Public Description(MAXDEVICES) As String

    Public Enum PLCStates
        NetworkFound = 1
        PLCsFound = 2
        PLCConnected = 3
    End Enum
    Public PLCState As PLCStates


    Function NetWorkInitialize() As Integer
        'return rc from pascal calls
        '  0 says ok

        ' if the network interface has already been opened, close it
        '
        If NetworkOK = True Then
            Rc = PASCAL_HEICloseTransport(TP)
            Rc = PASCAL_HEIClose()
        End If


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Initialize the Ethernet Driver
        '
        Rc = PASCAL_HEIOpen(HEIAPIVersion)
        If Rc <> 0 Then
            NetWorkInitialize = Rc 'return error code
            Exit Function
        Else
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Initiaizize the Winsock protocol transport
            '

            TP.Initialize()

            For i As Integer = 0 To MAXDEVICES - 1
                aDevices(i).Initialize()
            Next i

            TP.Transport = HEIT_WINSOCK

            TP.Protocol = HEIP_IP

            Rc = PASCAL_HEIOpenTransport(TP, HEIAPIVersion, 0)

            NetWorkInitialize = Rc

        End If

    End Function

    Function NetWorkScan() As String

        DeviceCount = MAXDEVICES

        ' Do the Query
        '
        Rc = PASCAL_HEIQueryDevices(TP, aDevices(0), DeviceCount, HEIAPIVersion)

        Dim X As Integer
        Dim tBuffer(256) As Byte
        Dim RSize As Integer

        If Rc <> 0 Then
            NetWorkScan = CStr(Rc) 'return error code
        Else
            ' If the PASCAL_HEIQuery call was successful, DeviceCount will contain the
            ' number of Host Ethernet Devices on the network
            '
            NetWorkScan = CStr(Rc)

            If DeviceCount > 0 Then

                ' Now lets display get relevant information about each device found
                '
                For X = 0 To DeviceCount - 1

                    ' Open the device
                    '
                    Rc = PASCAL_HEIOpenDevice(TP, aDevices(X), HEIAPIVersion, LongDevTimeout, DefDevRetrys, False)
                    If Rc <> 0 Then
                        NetWorkScan = CStr(Rc)
                        Exit Function

                    Else


                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' Display the IP address (remove leading spaces)
                        '
                        RSize = 4

                        Rc = PASCAL_HEIReadSetupData(aDevices(X), DT_IP_ADDRESS, tBuffer(0), RSize)
                        If Rc <> 0 Then
                            NetWorkScan = CStr(Rc)
                            Exit Function
                        Else
                            IPAddress(X) = GetIPFromBuffer(tBuffer)
                        End If

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' Fourth
                        ' Display the Module ID
                        '
                        'UPGRADE_NOTE: Erase was upgraded to System.Array.Clear. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
                        System.Array.Clear(tBuffer, 0, tBuffer.Length)
                        RSize = 4

                        Rc = PASCAL_HEIReadSetupData(aDevices(X), DT_NODE_NUMBER, tBuffer(0), RSize)
                        If Rc <> 0 Then
                            NetWorkScan = CStr(Rc)
                            Exit Function
                        Else
                            NodeNumber(X) = CStr(GetNodeNumberFromBuffer(tBuffer))

                        End If

                        'UPGRADE_NOTE: Erase was upgraded to System.Array.Clear. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
                        System.Array.Clear(tBuffer, 0, tBuffer.Length)
                        RSize = 256

                        Rc = PASCAL_HEIReadSetupData(aDevices(X), DT_DESCRIPTION, tBuffer(0), RSize)
                        If Rc <> 0 Then
                            NetWorkScan = CStr(Rc)
                            Exit Function
                        Else
                            Description(X) = GetNodeDescFromBuffer(tBuffer, RSize)
                        End If


                        ' close this device
                        '
                        Rc = PASCAL_HEICloseDevice(aDevices(X))
                        If Rc <> 0 Then
                            NetWorkScan = CStr(Rc)
                        End If
                    End If
                Next X
            End If
        End If

    End Function
    ' This function will format the Node Description retrieved from DT_NODE_DESCRIPTION
    '
    Private Function GetNodeDescFromBuffer(ByRef Buffer() As Byte, ByRef Size As Integer) As String

        Dim i As Integer = 0
        Dim DescText As String = ""

        For i = 0 To Size - 1
            If Chr(Buffer(i)) = vbCr Then
                Exit For
            End If
            DescText = DescText + Chr(Buffer(i))
        Next

        GetNodeDescFromBuffer = DescText

    End Function
    ' This function will format the IP address retrieved from DT_IP_ADDRESS
    '
    Private Function GetIPFromBuffer(ByRef Buffer() As Byte) As String

        Dim y As Integer = 0
        Dim t As String = ""

        For y = 0 To 3

            'make a printable version of each octet, add leading zeros to make all the octest three digits in length
            t = t + Format(StrConv(CStr(Buffer(y)), 1, Len(Buffer(y))), "000")

            ' place a decimal point between the octets
            If y < 3 Then
                t = t + "."
            Else
                t = t + " "
            End If

        Next y

        GetIPFromBuffer = t

    End Function

    ' This function will format the Node Number retrieved from DT_NODE_NUMBER
    '
    Private Function GetNodeNumberFromBuffer(ByRef Buffer() As Byte) As Integer

        Dim tNN As Integer

        ' the Node Number can be a 32bit number ( 4 bytes)
        '
        tNN = Buffer(0) + (Buffer(1) * 256) + (Buffer(2) * 65536.0#) + (Buffer(3) * 16777216.0#)

        GetNodeNumberFromBuffer = tNN

    End Function

    '***********************************************************************
    ' Since the PASCAL_HEIxxxx calls require a byte buffer, convert the user
    ' entered strings to byte arrays
    '
    Function StringToByteArray(ByVal inString As String, ByRef Buffer() As Byte) As Integer
        Dim i As Integer = 0
        Dim u() As Byte

        'Make sure all alpha characters are uppercase
        u = System.Text.UnicodeEncoding.Unicode.GetBytes(StrConv(inString, VbStrConv.Uppercase))

        'skip over the Unicode byte
        For i = 0 To (Len(inString) - 1)
            Buffer(i) = u(i * 2)
        Next i

        StringToByteArray = i

    End Function

    '***********************************************************************
    ' Swap successive entries in a byte array
    '
    Function ByteSwap(ByRef Buffer() As Byte, ByRef Count As Integer) As Integer
        Dim i As Integer = 0
        Dim temp As Byte

        For i = 0 To Count - 1 Step 2
            temp = Buffer(i)
            Buffer(i) = Buffer(i + 1)
            Buffer(i + 1) = temp
        Next i

        ByteSwap = i

    End Function

    '************************************************************************
    ' Convert a byte array of character codes to a packed array of characters
    '
    Function HexConvert(ByRef Buffer() As Byte, ByRef Count As Integer) As Integer
        Dim i As Integer = 0

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
                Buffer(i) = Microsoft.VisualBasic.AscW(Chr(Buffer(i)))

            End If

        Next i

        'Now pack two HEX characters into a byte
        Dim Z As Integer
        Z = 0
        For i = 0 To (Count * 2) - 1 Step 2
            Buffer(Z) = (Buffer(i) * 16) + Buffer(i + 1)
            Z = Z + 1
        Next i

        'Now clear the remainder of the byte array - just to be neat and complete
        For i = Z To (Count * 2) - 1
            Buffer(i) = 0
        Next i

        HexConvert = Z

    End Function

    '***********************************************************************
    ' Brute force method of converting a 4 character string to a HEX number
    '
    Function StringToHexInt(ByRef inData As String) As Integer

        Dim i, j As Integer
        Dim t(4) As Byte

        'convert from octal

        j = 0
        For i = 1 To Len(inData)
            j = j + Val(Mid(inData, Len(inData) - i + 1, 1)) * (8 ^ (i - 1))
        Next i

        StringToHexInt = j + 1
        Exit Function

        inData = Hex(j + 1)

        i = StringToByteArray(inData, t)

        'convert each character code
        For i = 0 To (Len(inData) - 1)

            'have to manually process HEX characters digits
            If (t(i) > 64) And (t(i) < 71) Then
                Select Case t(i)
                    Case 65 'A
                        t(i) = 10
                    Case 66 'B
                        t(i) = 11
                    Case 67 'C
                        t(i) = 12
                    Case 68 'D
                        t(i) = 13
                    Case 69 'E
                        t(i) = 14
                    Case 70 'F
                        t(i) = 15
                End Select

            Else
                'numeric digits are much easier
                t(i) = Microsoft.VisualBasic.AscW(Chr(t(i)))

            End If
        Next i

        Select Case Len(inData)
            Case 0
                StringToHexInt = 0
            Case 1
                StringToHexInt = t(0)
            Case 2
                StringToHexInt = (t(0) * 16) + t(1)
            Case 3
                StringToHexInt = (t(0) * 256) + (t(1) * 16) + t(2)
            Case 4
                StringToHexInt = (t(0) * 4096) + (t(1) * 256) + (t(2) * 16) + t(3)
        End Select

    End Function

    Sub DisconnectFromPLC()
        Rc = PASCAL_HEICloseTransport(TP)
        Rc = PASCAL_HEIClose()
    End Sub
    Function ConnectToPLC(ByRef DeviceNo As Integer) As String
        ' Open the device
        '
        Rc = PASCAL_HEIOpenDevice(TP, aDevices(DeviceNo), HEIAPIVersion, DefDevTimeout, DefDevRetrys, False)
        If Rc <> 0 Then
            DeviceOpen = False
        Else
            DeviceOpen = True
        End If
        ConnectToPLC = CStr(Rc)

    End Function

    Function DisconnectPLC() As String
        Rc = PASCAL_HEICloseDevice(aDevices(tDevice))
        DisconnectPLC = CStr(Rc)
    End Function
    Function GetData() As String
        GetData = CStr(PASCAL_HEICCMRequest(aDevices(tDevice), bWrite, DataType, DataAddress, DataLength, ByteBuffer(0)))
    End Function
    'Function ConvertToReal(ByteBuffer() As Byte) As Single
    Function ConvertToReal(ByRef Address As String) As Single
        Dim sFloat As Single
        Dim lngNum As Integer
        Dim blnSign As Boolean
        Dim i As Integer

        DataType = &H31
        DataLength = 4
        DataAddress = StringToHexInt(Address)
        Rc = CInt(GetData())
        lngNum = 0

        If ByteBuffer(3) > 127 Then
            ByteBuffer(3) = ByteBuffer(3) - 128
            blnSign = True
        End If
        For i = 0 To 3
            lngNum = lngNum + (ByteBuffer(i) * 256 ^ i)
        Next i

        CopyMemorySingleToInteger(sFloat, lngNum, 4)

        Dim sfloat1 As Single
        sfloat1 = 1.0 * lngNum

        If sFloat <> sfloat1 Then
            MsgBox("Not Equal")
        End If

        If blnSign Then
            sFloat = -sFloat
        End If

        ConvertToReal = sFloat

    End Function
    'Function ConvertToLong(ByteBuffer() As Byte) As Long
    Function ConvertToLong(ByRef Address As String) As Integer
        Dim i As Integer
        Dim s As String = ""

        DataType = &H31
        DataLength = 2
        DataAddress = StringToHexInt(Address)
        Rc = CInt(GetData())

        Rc = ByteSwap(ByteBuffer, 2)

        For i = 0 To DataLength - 1
            s = s & VB6.Format(Hex(ByteBuffer(i)), "00")
        Next i
        ConvertToLong = Val(s)
    End Function
    Public Function FromHex(ByRef HexString As String) As Integer
        Dim i As Integer

        For i = 1 To Len(HexString)
            FromHex = FromHex + Val(Mid(HexString, Len(HexString) - i + 1, 1)) * 16 ^ (i - 1)
        Next i
    End Function
End Module