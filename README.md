<html>
<body>
<h1>RealTerm BusPirate Sniff</h1>
Applies faster baud rate to real term and launches bus pirate SPI sniffer.
<h2>Introduction</h2>
<p>The <a href="http://dangerousprototypes.com/docs/Bus_Pirate">Bus Pirate</a> has at least three other out-of-the-box methods of sniffing SPI data from a Master and Slave communication:<p>
<ul>
  <li>The Bus Pirate GUI</li>
  <li><a href=http://dangerousprototypes.com/docs/Bus_Pirate_binary_SPI_sniffer_utility>
      Bus Pirate SPI Sniffer Utility</a></li>
  <li>Third party tools such as the Sump Logic Analyzer Client - an example <a href="">here</a>.</li>
</ul>
<p>I believe the fastest of these may be the sniffer utility.
   I used BusPirate.SPIsniffer.v0.3 application with a version 3.0 Spark Fun Bus Pirate Clone to sniff the SPI data between a microcontroller and a BK2421 tranceiver slave device from an MJX RC helicopter.
   This was fine to grab the initialisation sequence but the 115200 baud FTDI USB link to the laptop could not keep up long after that.  I was able to interpret the hex code grabbed using my own parser - see my GitHub project at <a href="https://github.com/pyblendnet-js/parseSPI"/>ParseSPI</a>.</p>
<p>For higher speed comms, I discovered the solution had been found on this post:<a href="http://dangerousprototypes.com/forum/viewtopic.php?f=4&t=6765&p=59413&hilit=SPIsniffer#p59413">Using Bus Pirate with SPI Sniffer</a>.  Armed with this, the <a href="http://dangerousprototypes.com/docs/SPI_(binary)">Bus Pirate SPI API</a> and the <a href="http://realterm.sourceforge.net/">Real Term</a> help file, iwas able to develop this small console program in visual basic (VS2010) to automate the process of receiving the sniffed data at 800000 baud.</p>
<h2>Installation</h2>
<p>I will never include executables in my github repositories so if you find one there - beware.</p>  
<p>This project was compiled with Visual Studio 2010 but there should be no reason why it cannot be compiled with Visual Studio Community 2013.
  Once you have installed Visual Studio, open the .sln project file and rebuild.
  Install <a href="https://sourceforge.net/p/realterm">RealTerm</a> from
   <a href="http://realterm.sourceforge.net/index.html#downloads_Download">here</a> with all the recommended service registrations 
   (I don't understand how these work but they allow RealTerm to be included as an activeX component in the visual studio project).
  The installer I used was named <i>Realterm_2.0.0.70_Signed_Wrapper_setup.exe</i></p> 
<h2>Usage</h2>
<p>The compiled executable can found in the debug directory and can be run with an optional path for the captured data followed by an optional virtual com port number.</p>
<p> e.g. <b><i>RealTermBusPirateSniff.exe C:\Users\home\Documents\bp1.txt 4</i></b><p>
<p>The default is to save it to the executable directory as bp.txt.</p>
<p>The default com port is COM9, but this is just the port that bus pirate was given when I first used it.</p>
  After some issues with a faulty USB cable, the port changed to COM5 as could be seen in the Device Manager.</p>
  The program will open an instance of RealTerm (assuming it has been registered as an ActiveX control as per the installation process) and will attempt to open the bus pirated at Com9 (or your selected Com port).
<p>If all goes well, the Real Term application will establish contact with the bus pirate, renegotiate baud rate from
    115200 up to 800000 baud and then prepare the bus pirate for SPI sniffing.</p>
<p>For sniffing the master and slave systems using the Bus Pirate, I have 3K9 resistors on the inputs to the Bus Pirate.</p>	 
<p>Even with this protection, and though it should be safe to do otherwise anyhow,
	I do not power the master and slave system until this stage is reached.</p>.
<p>Note:I also use 3K9 resistors in the all the connected lines to attempt to protect the devices from output mismatches.<p>
<p>At this stage it is possible to make further configeration changes directly in the RealTerm gui if required.
   Otherwise return to the RealTermBusPirateSniff console.</p>
    <p>Once the system is ready to sniff, press any key to commence sniffing and once more to stop.</p>
    <p>The program should then restore to Bus Pirate to it's original state and close the program.</p>
	<p>If things go belly up and the Bus Pirate is left in the higher baud rate mode, running my program again will still reach a position ready for capture.</p>
    <p>You will notice that the data is prefixed with hex 01, but otherwise follows the raw hex protocol with 0xHB to start a packet, 0xHD to end a packet, and 0xHC to start each Master and Slave data pairs.<p>
	<p>I have not found a way to tell how much data that RealTerm has downloaded until I stop the capture - if anyone knows please let me know.</p>
</body>
</html>
