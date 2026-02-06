   //Creation de la classe Helper qui va faire le Marshalling vers l'api native
   //Pour l'envoi d'un fichier il faut du PJL pour décrire les commandes d'impressions
   
   var data = bytes array

   string pjl = "";
   pjl += ((char)27);
   pjl += "%-12345X@PJL\r\n";

   if (isRectoVerso)
   {
		pjl += "@PJL SET DUPLEX=ON\r\n";
		pjl += "@PJL SET BINDING=LONGEDGE\r\n";
	}
    else
    {
		pjl += "@PJL SET DUPLEX=OFF\r\n";
    }

    if (isCouleur)
    {
		pjl += "@PJL SET RENDERMODE=COLOR\r\n";
    }
    else
    {
		pjl += "@PJL SET RENDERMODE=GRAYSCALE\r\n";
    }

    pjl += "@PJL ENTER LANGUAGE = PDF\r\n";
    var ascii = Encoding.ASCII;
    byte[] command = ascii
					.GetBytes(pjl)
                    .ToArray();

    RawPrinterHelper.SendFileToPrinter(__prodPrinter, data, command, file);
	
	//le PJL doit avoir une balise de EOF   
	string eojPjl = "";
    eojPjl += ((char)27);
    eojPjl += "%-12345X";
   
   
   https://msdn.microsoft.com/fr-fr/library/windows/desktop/ff686812(v=vs.85).aspx
   
   // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
		
		      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DRIVER_INFO_8
        {
            public uint cVersion;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pEnvironment;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverPath;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDataFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pConfigFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pHelpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDependentFiles;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pMonitorName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDefaultDataType;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszzPreviousNames;
            FILETIME ftDriverDate;
            UInt64 dwlDriverVersion;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszMfgName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszOEMUrl;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszHardwareID;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszProvider;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszPrintProcessor;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszVendorSetup;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszzColorProfiles;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszInfPath;
            public uint dwPrinterDriverAttributes;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszzCoreDriverDependencies;
            FILETIME ftMinInboxDriverVerDate;
            UInt64 dwlMinInboxDriverVerVersion;
        }
		
		
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
		
		//Permet de récuperer s'il faut lancer le Flux en RAW ou XPS_PASS
		private static string GetPrinterDataType(IntPtr hPrinter)
        {
            IntPtr driverInfo = new IntPtr();
            driverInfo = IntPtr.Zero;
            int buf_len = 0;
            int IntPtrSize = Marshal.SizeOf(typeof(IntPtr));

            int a = GetPrinterDriver(hPrinter, "", 8, driverInfo, 0, out buf_len);

            driverInfo = Marshal.AllocHGlobal(buf_len);

            a = GetPrinterDriver(hPrinter, "", 8, driverInfo, buf_len, out buf_len);
            var info = (DRIVER_INFO_8)Marshal.PtrToStructure(driverInfo, typeof(DRIVER_INFO_8));

            for (int i = 0; i <= 24; i++)
            {
                if (i == 12 || i == 15 || i == 11 || i == 14)
                    continue;

                IntPtr ptr = Marshal.ReadIntPtr(driverInfo, IntPtrSize * i);
               // Console.WriteLine("DRIVER INFO {0}: {1}", i, Marshal.PtrToStringUni(ptr));
            }
            

            var value = (int)info.dwPrinterDriverAttributes;
            BitArray b = new BitArray(new int[] { value });
            bool[] bits = new bool[b.Count];
            b.CopyTo(bits, 0);

            if (bits[1])
                return "XPS_PASS";
            else
                return "RAW";
        }

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount, string docName)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = docName;
           
            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
					di.pDataType = GetPrinterDataType(hPrinter);
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }


        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, byte[] bytes, Int32 dwCount, string docName)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = docName;

            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);

            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);

            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, bytes.Length);

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
					di.pDataType = GetPrinterDataType(hPrinter);
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pUnmanagedBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }

            Marshal.FreeCoTaskMem(pUnmanagedBytes);

            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, byte[] data, byte[] pjlCommand, string filename)
        {
            string eojPjl = "";
            eojPjl += ((char)27);
            eojPjl += "%-12345X";

            byte[] eojBytes =
                Encoding.ASCII.GetBytes(eojPjl);

            var commandLength = pjlCommand.Length;
            var eojLength = eojBytes.Length;




            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(data.Length);



            byte[] rv = new byte[pjlCommand.Length + nLength + eojLength];

            System.Buffer.BlockCopy(pjlCommand, 0, rv, 0, pjlCommand.Length);
            System.Buffer.BlockCopy(data, 0, rv, pjlCommand.Length, data.Length);
            System.Buffer.BlockCopy(eojBytes, 0, rv, pjlCommand.Length + data.Length, eojBytes.Length);

            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength + commandLength + eojLength);

            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(rv, 0, pUnmanagedBytes, nLength + commandLength + eojLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength + commandLength + eojLength, filename);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;

        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName, byte[] pjlCommand)
        {

            string eojPjl = "";
            eojPjl += ((char)27);
            eojPjl += "%-12345X";

            byte[] eojBytes =
                Encoding.ASCII.GetBytes(eojPjl);

            var commandLength = pjlCommand.Length;
            var eojLength = eojBytes.Length;

            // Open the file.
            using (FileStream fs = new FileStream(szFileName, FileMode.Open))
            {
                var filename = Path.GetFileName(szFileName);
                // Create a BinaryReader on the file.
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // Dim an array of bytes big enough to hold the file's contents.
                    Byte[] bytes = new Byte[fs.Length];
                    bool bSuccess = false;
                    // Your unmanaged pointer.
                    IntPtr pUnmanagedBytes = new IntPtr(0);
                    int nLength;

                    nLength = Convert.ToInt32(fs.Length);
                    // Read the contents of the file into the array.
                    bytes = br.ReadBytes(nLength);

                    byte[] rv = new byte[pjlCommand.Length + nLength + eojLength];

                    System.Buffer.BlockCopy(pjlCommand, 0, rv, 0, pjlCommand.Length);
                    System.Buffer.BlockCopy(bytes, 0, rv, pjlCommand.Length, bytes.Length);
                    System.Buffer.BlockCopy(eojBytes, 0, rv, pjlCommand.Length + bytes.Length, eojBytes.Length);

                    // Allocate some unmanaged memory for those bytes.
                    pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength + commandLength + eojLength);



                    // Copy the managed byte array into the unmanaged array.
                    Marshal.Copy(rv, 0, pUnmanagedBytes, nLength + commandLength + eojLength);
                    // Send the unmanaged bytes to the printer.
                    bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength + commandLength + eojLength, filename);
                    // Free the unmanaged memory that you allocated earlier.
                    Marshal.FreeCoTaskMem(pUnmanagedBytes);
                    return bSuccess;
                }
            }
        }

    }