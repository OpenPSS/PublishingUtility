using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal delegate int scePsmDevExtractPackage([MarshalAs(UnmanagedType.LPStr)] string dirExtract, [MarshalAs(UnmanagedType.LPStr)] string packageFile);
}
