Imports Realterm
Imports System.Threading
Imports System.IO

Module Module1
    Dim rt As New RealtermIntf

    'Sub send(ByVal b As Byte)

    Sub Main(ByVal args As String())
        If args.Count > 0 Then
            Console.WriteLine("Sniff data to:" & args(0))
        End If
        Dim strPath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
        Console.WriteLine("Current directory:" & strPath)
        Thread.Sleep(1000)  'give time for RealTerm to open
        rt.Caption = "Bus Pirate Sniffer"
        rt.baud = 115200
        If args.Count > 1 Then
            rt.Port = Integer.Parse(args(1))
        Else
            rt.Port = 9
        End If
        Console.Write("Opening Port " & rt.Port)
        'Try
        rt.PortOpen = True
        Thread.Sleep(100)
        rt.PutString(vbCr) ' & vbLf)
        Console.WriteLine(" opened")
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.PutString("b" & vbCr) ' & vbLf)
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.PutString("10" & vbCr) ' & vbLf)
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        Dim N As Integer = 4
        Dim baud = 4000000 / (N + 1)
        Console.WriteLine("Attempting to go to " & baud.ToString())
        rt.PutString(N.ToString() & vbCr) ' & vbLf)   '4000000 / (N + 1) where is N > 1
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.baud = baud
        Thread.Sleep(100)
        rt.PutString(" " & vbCr)
        Console.WriteLine("Char count =" & rt.CharCount)
        For i As Integer = 0 To 19
            Thread.Sleep(100)
            rt.PutChar(0)
        Next
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.PutChar(1)  'spi raw mode
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.PutChar(&H82)
        Thread.Sleep(100)
        Console.WriteLine("Char count =" & rt.CharCount)
        rt.CaptureAsHex = True
        rt.CaptureDirect = True
        rt.CaptureEnd = 100000
        rt.CaptureEndUnits = 0  'bytes
        If args.Count > 0 Then
            rt.CaptureFile = args(0)
        Else
            rt.CaptureFile = Path.Combine(strPath, "bp.txt")
        End If
        Console.WriteLine("Capture hex to " & rt.CaptureFile)
        Console.WriteLine("Press any key to commence sniffing")
        Console.ReadKey()
        'wait for key press
        Console.WriteLine("Press any key to stop sniffing")
        rt.StartCapture()
        rt.PutChar(&HE)  'spi sniffing
        Thread.Sleep(5000)
        Console.WriteLine("Char Count:" & rt.CharCount)
        Console.ReadKey()
        rt.StopCapture()
        Console.WriteLine("Capture stopped")
        'Catch e As Exception
        '    Console.WriteLine(" error" & vbLf & e.Message)
        'End Try
        Console.WriteLine("Press any key to exit")
        Console.ReadKey()
        If rt.PortOpen Then
            rt.PutChar(0) 'exit sniff mode
            Thread.Sleep(100)
            rt.PutChar(15)  'should reset bit pirate
            rt.Close()
        End If
    End Sub

    ' Bus Pirate SPI modes
    '0  = 00000000 - Enter raw bitbang mode, reset to raw bitbang mode
    '1  = 00000001 – SPI mode/rawSPI version string (SPI1)
    '2  = 00000010 – CS low (0)
    '3  = 00000011 – CS high (1)
    '13 = 00001101 - Sniff all SPI traffic
    '
    '14 = 00001110 - Sniff when CS low
    '15 = 00001111 - Sniff when CS high  (Seems to have been changed to reset)
    '0001xxxx – Bulk SPI transfer, send 1-16 bytes (0=1byte!)
    '0010xxxx – Low 4 bits of byte + single byte write/read
    '0011xxxx – High 4 bits of byte
    '0100wxyz – Configure peripherals, w=power, x=pullups, y=AUX, z=CS
    '01010000 – Read peripherals
    '
    '01100xxx – Set SPI speed, 30, 125, 250khz; 1, 2, 2.6, 4, 8MHz
    '01110000 – Read SPI speed
    '1000wxyz – SPI config, w=output type, x=idle, y=clock edge, z=sample
    '10010000 – Read SPI config
    '
    'Or it could be a new protocol:
    '# 00000001 – SPI mode/rawSPI version string (SPI1)
    '# 00000010 – CS low (0)
    '# 00000011 – CS high (1)
    '# 0001xxxx – Bulk SPI transfer, send 1-16 bytes (0=1byte!)
    '# 0010xxxx – Low 4 bits of byte + single byte write/read
    '# 0011xxxx – High 4 bits of byte
    '# 0100wxyz – Configure peripherals w=power, x=pullups, y=AUX, z=CS
    '# 0101wxyz – read peripherals w=power, x=pullups, y=AUX, z=CS
    '# 01100xxx – Set SPI speed,
    '# 01110xxx – Read SPI speed,
    '# 1000wxyz – SPI config, w=output type, x=idle, y=clock edge, z=sample
    '# 1001wxyz – read SPI config, w=output type, x=idle, y=clock edge, z=sample
    '
    'Plus there is possible:
    '
    '* 00000000 //Enter binmode, show version string “BBIO1″.
    '* 00000001 //enter rawSPI mode, responds “SPI1″.
    '* …. //more protocols
    '* 00001111 //reset Bus Pirate (returns to user terminal)
    '* 010xxxxx //set pins as input(1) or output(0), responds with read.
    '* 1xxxxxxx //set pins high (1) or low (0), responds with read.

    'From BusPirate.SPIsniffer.v0.3 code:
    '  Once BBIO mode is reached, enter SPI mode by sending 0x01,  then 0x82 to setup SPI for positive clk pulses,
    '  then start sniffer by sending 0x0E
End Module
