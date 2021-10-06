using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGetTargetKdbg(string signInID, string password, byte[] c1, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);
}
