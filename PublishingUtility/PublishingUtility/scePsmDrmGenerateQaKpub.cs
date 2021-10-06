using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGenerateQaKpub(string signInID, string password, string commonName, ScePsmDrmKpubUploadState uploadState, out IntPtr devPkcs12, out int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);
}
