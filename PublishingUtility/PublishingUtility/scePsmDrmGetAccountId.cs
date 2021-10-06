using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGetAccountId(IntPtr devPkcs12, int devPkcs12Size, IntPtr accountId);
}
