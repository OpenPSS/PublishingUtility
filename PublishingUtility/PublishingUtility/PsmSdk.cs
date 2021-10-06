using System;
using System.IO;
using Microsoft.Win32;

namespace PublishingUtility
{
	internal static class PsmSdk
	{
		public static string Location { get; private set; }

		public static string AdbExe => Path.Combine(Location, "tools", "adb", "adb.exe");

		public static string RuntimeVer => Path.Combine(Location, "tools", "runtime.ver");

		public static string SdkVersionFile => Path.Combine(Location, "tools", "sdk_version.txt");

		public static string DefaultCopyright => Path.Combine(Location, "tools", "PublishingUtility", "default_copyright.txt");

		static PsmSdk()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("SCE_PSM_SDK");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				Location = environmentVariable;
				return;
			}
			string text = CheckRegistry();
			if (!string.IsNullOrWhiteSpace(text))
			{
				Location = text;
			}
			else
			{
				Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "SCE", "PSM");
			}
		}

		private static string CheckRegistry()
		{
			object value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\SCE\\PSM", "InstallDirectory", null);
			if (value != null)
			{
				return value as string;
			}
			value = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Wow6432Node\\SCE\\PSM", "InstallDirectory", null);
			return value as string;
		}
	}
}
