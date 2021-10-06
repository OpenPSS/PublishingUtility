using System;

namespace PublishingUtility
{
	internal delegate ScePsmDrmStatus scePsmDrmGetHostKdbg(string signInID, string password, string titleIdentifier, byte[] devPkcs12, int devPkcs12Size, IntPtr hostKdbg, IntPtr expiration);
}
