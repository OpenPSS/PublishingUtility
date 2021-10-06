using System;
using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal static class SubmissionArchiveGenerator64
	{
		private const string NATIVE_DLL = "..\\lib\\submission_archive_generator64.dll";

		[DllImport("..\\lib\\submission_archive_generator64.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "scePsmGenerateSubmissionArchive", ThrowOnUnmappableChar = true)]
		public static extern SceSagErrorCode Generate(IntPtr devPkcs12, int devPkcs12Size, string applicationId, string edataArchiveFileName, string submissionArchiveFileName);
	}
}
