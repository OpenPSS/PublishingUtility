using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal delegate int scePsmDevPickFileFromPackage([MarshalAs(UnmanagedType.LPStr)] string outName, [MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string inName);
}
