using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGetCertInfo(IntPtr devPkcs12, int devPkcs12Size, IntPtr certCommonName, IntPtr certCreateTime);
}
