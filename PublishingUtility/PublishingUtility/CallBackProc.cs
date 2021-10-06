using System.Runtime.InteropServices;

namespace PublishingUtility
{
	internal delegate void CallBackProc([MarshalAs(UnmanagedType.LPStr)] string msg);
}
