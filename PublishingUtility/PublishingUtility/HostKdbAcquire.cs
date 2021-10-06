using System;
using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal static class HostKdbAcquire
	{
		private const string PATH_DLL32 = "..\\lib\\host_kdbg_acquirer32.dll";

		private const string PATH_DLL64 = "..\\lib\\host_kdbg_acquirer64.dll";

		public static scePsmDrmGetHostKdbgInit _scePsmDrmGetHostKdbgInit;

		public static scePsmDrmGetHostKdbg _scePsmDrmGetHostKdbg;

		public static scePsmDrmGetHostKdbgTerm _scePsmDrmGetHostKdbgTerm;

		public static void Initialize()
		{
			if (IntPtr.Size == 4)
			{
				_scePsmDrmGetHostKdbgInit = scePsmDrmGetHostKdbgInit32;
				_scePsmDrmGetHostKdbg = scePsmDrmGetHostKdbg32;
				_scePsmDrmGetHostKdbgTerm = scePsmDrmGetHostKdbgTerm32;
			}
			else
			{
				_scePsmDrmGetHostKdbgInit = scePsmDrmGetHostKdbgInit64;
				_scePsmDrmGetHostKdbg = scePsmDrmGetHostKdbg64;
				_scePsmDrmGetHostKdbgTerm = scePsmDrmGetHostKdbgTerm64;
			}
		}

		[DllImport("..\\lib\\host_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbgInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbgInit32(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\host_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbg32(string signInID, string password, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr hostKdbg, IntPtr expiration);

		[DllImport("..\\lib\\host_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbgTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbgTerm32();

		[DllImport("..\\lib\\host_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbgInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbgInit64(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\host_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbg64(string signInID, string password, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr hostKdbg, IntPtr expiration);

		[DllImport("..\\lib\\host_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetHostKdbgTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetHostKdbgTerm64();
	}
}
