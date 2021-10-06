using System;
using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal static class KPubGenerator
	{
		private const string PATH_DLL32 = "..\\lib\\kpub_generator32.dll";

		private const string PATH_DLL64 = "..\\lib\\kpub_generator64.dll";

		public static scePsmDrmGenerateKpubInit _scePsmDrmGenerateKpubInit;

		public static scePsmDrmGenerateKpub _scePsmDrmGenerateKpub;

		public static scePsmDrmGenerateQaKpub _scePsmDrmGenerateQaKpub;

		public static scePsmDrmGetCertInfo _scePsmDrmGetCertInfo;

		public static scePsmDrmGetAccountId _scePsmDrmGetAccountId;

		public static scePsmDrmReleaseDevPkcs12 _scePsmDrmReleaseDevPkcs12;

		public static scePsmDrmGenerateKpubTerm _scePsmDrmGenerateKpubTerm;

		public static void Initialize()
		{
			if (IntPtr.Size == 4)
			{
				_scePsmDrmGenerateKpubInit = scePsmDrmGenerateKpubInit32;
				_scePsmDrmGenerateKpub = scePsmDrmGenerateKpub32;
				_scePsmDrmGenerateQaKpub = scePsmDrmGenerateQaKpub32;
				_scePsmDrmGetCertInfo = scePsmDrmGetCertInfo32;
				_scePsmDrmGetAccountId = scePsmDrmGetAccountId32;
				_scePsmDrmReleaseDevPkcs12 = scePsmDrmReleaseDevPkcs12_32;
				_scePsmDrmGenerateKpubTerm = scePsmDrmGenerateKpubTerm32;
			}
			else
			{
				_scePsmDrmGenerateKpubInit = scePsmDrmGenerateKpubInit64;
				_scePsmDrmGenerateKpub = scePsmDrmGenerateKpub64;
				_scePsmDrmGenerateQaKpub = scePsmDrmGenerateQaKpub64;
				_scePsmDrmGetCertInfo = scePsmDrmGetCertInfo64;
				_scePsmDrmGetAccountId = scePsmDrmGetAccountId64;
				_scePsmDrmReleaseDevPkcs12 = scePsmDrmReleaseDevPkcs12_64;
				_scePsmDrmGenerateKpubTerm = scePsmDrmGenerateKpubTerm64;
			}
		}

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpubInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpubInit32(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpub", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpub32(string signInID, string password, string commonName, ScePsmDrmKpubUploadState uploadState, out IntPtr devPkcs12, out int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateQaKpub", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateQaKpub32(string signInID, string password, string commonName, ScePsmDrmKpubUploadState uploadState, out IntPtr devPkcs12, out int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetCertInfo", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetCertInfo32(IntPtr devPkcs12, int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetAccountId", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetAccountId32(IntPtr devPkcs12, int devPkcs12Size, IntPtr accountId);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmReleaseDevPkcs12", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmReleaseDevPkcs12_32(IntPtr devPkcs12);

		[DllImport("..\\lib\\kpub_generator32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpubTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpubTerm32();

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpubInit", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpubInit64(string pu_ver, string env, string proxyServer, int proxyPort);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpub", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpub64(string signInID, string password, string commonName, ScePsmDrmKpubUploadState uploadState, out IntPtr devPkcs12, out int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateQaKpub", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateQaKpub64(string signInID, string password, string commonName, ScePsmDrmKpubUploadState uploadState, out IntPtr devPkcs12, out int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetCertInfo", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetCertInfo64(IntPtr devPkcs12, int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGetAccountId", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGetAccountId64(IntPtr devPkcs12, int devPkcs12Size, IntPtr accountId);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmReleaseDevPkcs12", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmReleaseDevPkcs12_64(IntPtr devPkcs12);

		[DllImport("..\\lib\\kpub_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmDrmGenerateKpubTerm", ThrowOnUnmappableChar = true)]
		private static extern ScePsmDrmStatus scePsmDrmGenerateKpubTerm64();
	}
}
