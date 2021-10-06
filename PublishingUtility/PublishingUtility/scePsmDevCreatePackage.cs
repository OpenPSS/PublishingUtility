using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal delegate int scePsmDevCreatePackage([MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string dirForPack);
}
