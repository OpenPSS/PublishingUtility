using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGetTitleQaTargetKdbg(string signInID, string password, byte[] c1, string titleIdentifier, byte[] qaPkcs12, int qaPkcs12Size, long devAccountId, IntPtr pR1, IntPtr peTargetKdbg, IntPtr pExpiration);
}
