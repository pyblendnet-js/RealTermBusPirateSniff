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
<h2>Usage</h2>
<p>Just run the executable found the debug directory with an optional path for the captured data.
  The default is to save it to the executable directory as bp.txt.
  The program will open an instance of RealTerm (assuming it has been registered as an ActiveX control as per the installation process) and will attempt to open the bus pirated at Com9.
  You can add a second argument to the commandline to direct it to a different Com port.</p>
  <p> e.g. <b><i>RealTermBusPirateSniff.exe C:\Users\home\Documents\bp1.txt 4</i></b><p>
  <p>If all goes well, the Real Term application will establish contact with the bus pirate, renegotiate baud rate from
    115200 up to 800000 baud and then prepare the bus pirate for SPI sniffing.  Though it should be safe to do otherwise, I do not power the master and slave system until this stage is reached (I also use 3K9 resistors in the all the connected lines to attempt to protect the devices from output mismatches).<p>
    <p>Once the system is ready to sniff, press any key to commence sniffing and once more to stop.</p>
    <p>The program should then restore to Bus Pirate to it's original state and close the program.</p>
    <p>You will notice that the data is prefixed with hex 01, but otherwise follows the raw hex protocol with 0xHB to start a packet, 0xHD to end a packet, and 0xHC to start each Master and Slave data pairs.<p>
</body>
</html>
