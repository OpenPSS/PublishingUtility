using System.Runtime.InteropServices;

namespace PublishingUtility
{
	public struct ScePsmApplication
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] name;

		public int size;
	}
}
