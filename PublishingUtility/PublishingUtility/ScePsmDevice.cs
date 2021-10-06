using System;
using System.Runtime.InteropServices;

namespace PublishingUtility
{
	public struct ScePsmDevice
	{
		public Guid guid;

		public int type;

		public int online;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] deviceID;
	}
}
