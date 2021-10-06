using System;
using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal static class TargetKdbgAcquirer
	{
		private const string PATH_DLL32 = "..\\lib\\target_kdbg_acquirer32.dll";

		private const string PATH_DLL64 = "..\\lib\\target_kdbg_acquirer64.dll";

		public static scePsmDrmGetTargetKdbgInit _scePsmDrmGetTargetKdbgInit;

		public static scePsmDrmGetTargetKdbg _scePsmDrmGetTargetKdbg;

		public static scePsmDrmGetTitleQaTargetKdbg _scePsmDrmGetTitleQaTargetKdbg;

		public static scePsmDrmGetTargetKdbgTerm _scePsmDrmGetTargetKdbgTerm;

		public static void Initialize()
		{
			if (IntPtr.Size == 4)
			{
				_scePsmDrmGetTargetKdbgInit = scePsmDrmGetTargetKdbgInit32;
				_scePsmDrmGetTargetKdbg = scePsmDrmGetTargetKdbg32;
				_scePsmDrmGetTitleQaTargetKdbg = scePsmDrmGetTitleQaTargetKdbg32;
				_scePsmDrmGetTargetKdbgTerm = scePsmDrmGetTargetKdbgTerm32;
			}
			else
			{
				_scePsmDrmGetTargetKdbgInit = scePsmDrmGetTargetKdbgInit64;
				_scePsmDrmGetTargetKdbg = scePsmDrmGetTargetKdbg64;
				_scePsmDrmGetTitleQaTargetKdbg = scePsmDrmGetTitleQaTargetKdbg64;
				_scePsmDrmGetTargetKdbgTerm = scePsmDrmGetTargetKdbgTerm64;
			}
		}

		[DllImport("..\\lib\\target_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbgInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbgInit32(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\target_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbg32(string signInID, string password, byte[] c1, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);

		[DllImport("..\\lib\\target_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTitleQaTargetKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTitleQaTargetKdbg32(string signInID, string password, byte[] c1, string titleIdentifier, byte[] qaPkcs12, int qaPkcs12Size, long devAccountId, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);

		[DllImport("..\\lib\\target_kdbg_acquirer32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbgTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbgTerm32();

		[DllImport("..\\lib\\target_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbgInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbgInit64(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\target_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbg64(string signInID, string password, byte[] c1, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);

		[DllImport("..\\lib\\target_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTitleQaTargetKdbg", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTitleQaTargetKdbg64(string signInID, string password, byte[] c1, string titleIdentifier, byte[] qaPkcs12, int qaPkcs12Size, long devAccountId, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);

		[DllImport("..\\lib\\target_kdbg_acquirer64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetTargetKdbgTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetTargetKdbgTerm64();
	}
}
