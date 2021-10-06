using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PublishingUtility.Dialog;

namespace PublishingUtility
{
	internal static class PsmDeviceFuncBinding
	{
		private enum LSTDEV
		{
			GUID,
			TYPE,
			LINE,
			DVID
		}

		private enum LSTAPP
		{
			NAME,
			SIZE
		}

		private const int DEVICE_NUM = 8;

		private const int APPLICATION_NUM = 100;

		public const int TARGET_SIMULATOR = 0;

		public const int TARGET_ANDROID = 1;

		public const int TARGET_PS_VITA = 2;

		public const int SCE_PSM_DEVAGENT_OK = 0;

		private const string dll32 = "..\\lib\\psm_device32.dll";

		private const string dll64 = "..\\lib\\psm_device64.dll";

		private static string[] TARGET_NAME = new string[3] { "SIMULATOR     ", "ANDROID       ", "PS_VITA       " };

		private static scePsmDevCreatePackage _scePsmDevCreatePackage;

		private static scePsmDevExtractPackage _scePsmDevExtractPackage;

		private static scePsmDevPickFileFromPackage _scePsmDevPickFileFromPackage;

		private static ScePsmDevice[] mDevices = new ScePsmDevice[8];

		private static Hashtable mDevicesVerChk = new Hashtable();

		private static int mDeviceNum = 0;

		private static int mHandle = 0;

		private static int mConnectNum = -1;

		private static Mutex mInfoMutex = new Mutex();

		private static int androidVersion = 0;

		[DllImport("..\\lib\\psm_device32.dll", EntryPoint = "scePsmDevCreatePackage")]
		public static extern int scePsmDevCreatePackage32([MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string dirForPack);

		[DllImport("..\\lib\\psm_device32.dll", EntryPoint = "scePsmDevExtractPackage")]
		public static extern int scePsmDevExtractPackage32([MarshalAs(UnmanagedType.LPStr)] string dirExtract, [MarshalAs(UnmanagedType.LPStr)] string packageFile);

		[DllImport("..\\lib\\psm_device32.dll", EntryPoint = "scePsmDevPickFileFromPackage")]
		public static extern int scePsmDevPickFileFromPackage32([MarshalAs(UnmanagedType.LPStr)] string outName, [MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string inName);

		[DllImport("..\\lib\\psm_device64.dll", EntryPoint = "scePsmDevCreatePackage")]
		public static extern int scePsmDevCreatePackage64([MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string dirForPack);

		[DllImport("..\\lib\\psm_device64.dll", EntryPoint = "scePsmDevExtractPackage")]
		public static extern int scePsmDevExtractPackage64([MarshalAs(UnmanagedType.LPStr)] string dirExtract, [MarshalAs(UnmanagedType.LPStr)] string packageFile);

		[DllImport("..\\lib\\psm_device64.dll", EntryPoint = "scePsmDevPickFileFromPackage")]
		public static extern int scePsmDevPickFileFromPackage64([MarshalAs(UnmanagedType.LPStr)] string outName, [MarshalAs(UnmanagedType.LPStr)] string packageFile, [MarshalAs(UnmanagedType.LPStr)] string inName);

		public static void Initialize()
		{
			if (IntPtr.Size == 4)
			{
				_scePsmDevCreatePackage = scePsmDevCreatePackage32;
				_scePsmDevExtractPackage = scePsmDevExtractPackage32;
				_scePsmDevPickFileFromPackage = scePsmDevPickFileFromPackage32;
			}
			else
			{
				_scePsmDevCreatePackage = scePsmDevCreatePackage64;
				_scePsmDevExtractPackage = scePsmDevExtractPackage64;
				_scePsmDevPickFileFromPackage = scePsmDevPickFileFromPackage64;
			}
			try
			{
				string path = PsmSdk.Location + "\\tools\\psmdevassistant.ver";
				StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
				androidVersion = Convert.ToInt32(streamReader.ReadToEnd().Trim(), 10);
				streamReader.Close();
			}
			catch
			{
			}
		}

		private static Process CmdStart(string arg, bool waitFinish = true)
		{
			string fileName = PsmSdk.Location + "\\tools\\PsmDevice\\PsmDevice.exe";
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = fileName;
			processStartInfo.Arguments = arg;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardInput = true;
			Process process = Process.Start(processStartInfo);
			if (waitFinish)
			{
				process.WaitForExit();
			}
			return process;
		}

		private static void CmdEnd(Process p)
		{
			p.Close();
			p.Dispose();
		}

		private static Process AdbCmdStart(string arg, bool waitFinish = true)
		{
			string fileName = PsmSdk.Location + "\\tools\\adb\\adb.exe";
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = fileName;
			processStartInfo.Arguments = arg;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardInput = true;
			Process process = Process.Start(processStartInfo);
			if (waitFinish)
			{
				process.WaitForExit();
			}
			return process;
		}

		private static void AdbCmdEnd(Process p)
		{
			p.Close();
			p.Dispose();
		}

		public static int GetListDevices(out ScePsmDevice[] devices)
		{
			Console.WriteLine("*** ListDevices *** \n");
			int num = -1;
			Process process = CmdStart($"-list_devices");
			if (0 < process.ExitCode)
			{
				string text = process.StandardOutput.ReadLine();
				if (!string.IsNullOrEmpty(text))
				{
					int num2 = int.Parse(text);
					for (int i = 0; i < num2; i++)
					{
						string[] array = process.StandardOutput.ReadLine().Split('\t');
						mDevices[i].guid = new Guid(array[0]);
						if (array[1] == "Simulator")
						{
							mDevices[i].type = 0;
						}
						if (array[1] == "Android")
						{
							mDevices[i].type = 1;
						}
						if (array[1] == "Vita")
						{
							mDevices[i].type = 2;
						}
						mDevices[i].online = ((array[2] == "on") ? 1 : 0);
						mDevices[i].deviceID = array[3].ToCharArray();
					}
					devices = mDevices;
					mDeviceNum = num2;
					if (mDeviceNum > 0)
					{
						DrawDeviceList();
					}
					num = num2;
				}
				else
				{
					devices = mDevices;
					mDeviceNum = 0;
					num = -1;
				}
				CmdEnd(process);
				mInfoMutex.WaitOne();
				mDevicesVerChk.Clear();
				for (int j = 0; j < num; j++)
				{
					if (devices[j].type == 2 && devices[j].online == 1)
					{
						process = CmdStart($"-version {devices[j].guid}");
						mDevicesVerChk.Add(devices[j].guid, process.ExitCode);
						CmdEnd(process);
					}
				}
				if (num < 0)
				{
					PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Cannot get device list.", "デバイスリストが取得できません。"));
				}
				mInfoMutex.ReleaseMutex();
			}
			else
			{
				Console.WriteLine("It fails in \"{0}\" command.\n( {1} )", "-list_deviecs", process.ExitCode);
				CmdEnd(process);
				devices = mDevices;
				num = -1;
			}
			return num;
		}

		private static int CheckVersionErr(Guid guid)
		{
			int num = 0;
			mInfoMutex.WaitOne();
			if (mDevicesVerChk[guid] != null)
			{
				num = (int)mDevicesVerChk[guid];
			}
			if (num == -2147418100 || num == -2147418099)
			{
				string text = "";
				Process process = CmdStart($"-get_errstr {guid}");
				text = process.StandardOutput.ReadLine();
				CmdEnd(process);
				string text2 = "";
				text2 = ((num != -2147418100) ? Utility.TextLanguage("Host/Target versions mismatched. Target is OLDER.\nPlease update the PlayStation Mobile Development Assistant to {0}", "PlayStation Mobile SDK、または PSM Studio と\nPlayStation Mobile Development Assistant のバージョンが異なります。\nPlayStation Mobile Development Assistant を {0} に更新して下さい。") : Utility.TextLanguage("Host/Target versions mismatched. Target is NEWER.\nPlease update the PlayStation Mobile SDK or the PSM Studio to {0}", "PlayStation Mobile SDK、または PSM Studio と\nPlayStation Mobile Development Assistant のバージョンが異なります。\nPlayStation Mobile SDK、または PSM Studio を {0} に更新して下さい。"));
				MessageBox.Show(string.Format(text2, text), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			mInfoMutex.ReleaseMutex();
			return num;
		}

		private static void DrawDeviceList()
		{
			for (int i = 0; i < mDeviceNum; i++)
			{
				int type = mDevices[i].type;
				_ = mDevices[i].online;
				if (type != -1)
				{
					Utility.SafeConvertCharArrayToString(mDevices[i].deviceID);
				}
			}
		}

		public static int SetAdbExePath(string path)
		{
			Process p = CmdStart($"-set_adbexe_path \"{path}\"");
			CmdEnd(p);
			return 0;
		}

		public static int CreatePackage(string packageFile, string dirForPack)
		{
			int num = _scePsmDevCreatePackage(packageFile, dirForPack);
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Creation package failed.", "パッケージの作成に失敗しました。"));
			}
			return num;
		}

		public static int ExtractPackage(string dirExtract, string packageFile)
		{
			Encoding uTF = Encoding.UTF8;
			int byteCount = uTF.GetByteCount(packageFile);
			int byteCount2 = uTF.GetByteCount(dirExtract);
			if (byteCount >= 255)
			{
				MessageBox.Show("byteLength=" + byteCount + Utility.TextLanguage("\n\nThe length of the path and the file name is greater than 255 bytes.\nPlease shorten the length of the path and file.", "\n\nパス+ファイル名の長さが255バイトを超えています。\nファイルとパスの長さを短くしてください。") + "\n\n" + packageFile, "ExtractPackage()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return 1;
			}
			if (byteCount2 >= 255)
			{
				MessageBox.Show("byteLength=" + byteCount2 + Utility.TextLanguage("\n\nThe length of the path and the file name is greater than 255 bytes.\nPlease shorten the length of the path and file.", "\n\nパス+ファイル名の長さが255バイトを超えています。\nファイルとパスの長さを短くしてください。") + "\n\n" + dirExtract, "ExtractPackage()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return 1;
			}
			int num = _scePsmDevExtractPackage(dirExtract, packageFile);
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Extraction from package failed.", "パッケージの解凍に失敗しました。"));
			}
			return num;
		}

		public static int PickFileFromPackage(string outName, string packageFile, string inName)
		{
			int num = _scePsmDevPickFileFromPackage(outName, packageFile, inName);
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Extraction file from master package failed.", "マスターパッケージからのファイル抽出に失敗しました。"));
			}
			return num;
		}

		public static int Install(Guid guid, string packageFile, string appId)
		{
			int num = CheckVersionErr(guid);
			if (num == 0)
			{
				if (!ExistAssistant(guid))
				{
					num = InstallAssistant(guid);
				}
				if (num == 0)
				{
					Process process = CmdStart($"-install {guid} \"{packageFile}\" {appId}");
					num = process.ExitCode;
					CmdEnd(process);
				}
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Installing application failed.", "アプリケーションのインストールに失敗しました。") + PsmDeviceError.OptionList(num, guid, packageFile, appId));
			}
			return num;
		}

		public static int Uninstall(Guid guid, string appId)
		{
			int num = CheckVersionErr(guid);
			if (num == 0)
			{
				if (!ExistAssistant(guid))
				{
					num = InstallAssistant(guid);
				}
				if (num == 0)
				{
					Process process = CmdStart($"-uninstall {guid} {appId}");
					num = process.ExitCode;
					CmdEnd(process);
				}
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Uninstalling application failed.", "アプリケーションのアンインストールに失敗しました。") + PsmDeviceError.OptionList(num, guid, appId));
			}
			return num;
		}

		public static int Launch(Guid guid, string appId, bool debug, bool profile, bool keepnet, bool logwaiting, string arg)
		{
			int num = CheckVersionErr(guid);
			if (num == 0)
			{
				if (!ExistAssistant(guid))
				{
					num = InstallAssistant(guid);
				}
				if (num == 0)
				{
					Process process = CmdStart($"-launch {guid} {appId} {debug} {profile} {keepnet} {logwaiting} {arg}");
					num = process.ExitCode;
					CmdEnd(process);
				}
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Launching application failed.", "アプリケーションの起動に失敗しました。") + PsmDeviceError.OptionList(num, guid, appId, debug.ToString(), profile.ToString(), keepnet.ToString(), arg));
			}
			return num;
		}

		public static int Kill(Guid guid)
		{
			int num = CheckVersionErr(guid);
			if (num == 0)
			{
				if (!ExistAssistant(guid))
				{
					num = InstallAssistant(guid);
				}
				if (num == 0)
				{
					Process process = CmdStart($"-kill {guid}");
					num = process.ExitCode;
					CmdEnd(process);
				}
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Stopping application failed.", "アプリケーションの停止に失敗しました。") + PsmDeviceError.OptionList(num, guid));
			}
			return num;
		}

		public static int ListApplications(Guid guid, ScePsmApplication[] list)
		{
			int num = -1;
			Process process = CmdStart($"-list_apps {guid}");
			if (0 < process.ExitCode)
			{
				string text = process.StandardOutput.ReadLine();
				if (!string.IsNullOrEmpty(text))
				{
					int num2 = int.Parse(text);
					for (int i = 0; i < num2; i++)
					{
						string[] array = process.StandardOutput.ReadLine().Split('\t');
						list[i].name = array[0].ToCharArray();
						list[i].size = int.Parse(array[1]);
					}
				}
				num = process.ExitCode;
				CmdEnd(process);
				if (num < 0)
				{
					PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Cannot get application list.", "アプリケーションリストが取得できません。") + PsmDeviceError.OptionList(num, guid));
				}
			}
			else
			{
				Console.WriteLine("It fails in \"{0}\" command.\n( {1} )", "-list_apps", process.ExitCode);
				CmdEnd(process);
				num = -1;
			}
			return num;
		}

		public static int GetDeviceSeed(Guid guid, string outputFilename)
		{
			int num = 0;
			if (!ExistAssistant(guid))
			{
				num = InstallAssistant(guid);
			}
			if (num == 0)
			{
				Process process = CmdStart($"-get_device_seed {guid} \"{outputFilename}\"");
				num = process.ExitCode;
				CmdEnd(process);
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage($"Failed to generate Device Seed {Path.GetFileName(outputFilename)}. ", $"デバイスシード {Path.GetFileName(outputFilename)} の作成に失敗しました。") + PsmDeviceError.OptionList(num, guid, outputFilename));
			}
			return num;
		}

		public static int SetAppExeKey(Guid guid, string filename)
		{
			int num = 0;
			if (!ExistAssistant(guid))
			{
				num = InstallAssistant(guid);
			}
			if (num == 0)
			{
				Process process = CmdStart($"-set_appexekey {guid} \"{filename}\"");
				num = process.ExitCode;
				CmdEnd(process);
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Installing App Key failed.", "アプリ鍵のインストールに失敗しました。") + PsmDeviceError.OptionList(num, guid, filename));
			}
			return num;
		}

		public static int ExistAppExeKey(Guid guid, long accountId, string titleIdentifier, string env)
		{
			int num = 0;
			if (!ExistAssistant(guid))
			{
				num = InstallAssistant(guid);
			}
			if (num == 0)
			{
				Process process = CmdStart($"-exist_appexekey {guid} {accountId} {titleIdentifier} {env}");
				num = process.ExitCode;
				CmdEnd(process);
			}
			if (num < 0)
			{
				PsmDeviceError.PsmDeviceMessage((PsmDeviceStatus)num, Utility.TextLanguage("Existing App Key failed.", "アプリ鍵の確認に失敗しました。") + PsmDeviceError.OptionList(num, guid, accountId.ToString(), titleIdentifier, env));
			}
			return num;
		}

		public static int SetConsoleWrite(Guid guid)
		{
			int num = -1;
			Process process = CmdStart($"-set_console {guid}");
			num = process.ExitCode;
			CmdEnd(process);
			return num;
		}

		public static Process GetLog(Guid guid)
		{
			return CmdStart($"-get_log {guid}", waitFinish: false);
		}

		public static Process ClearLog(Guid guid)
		{
			return CmdStart($"-clear_log {guid}");
		}

		private static bool ExistAssistant(Guid guid)
		{
			bool result = false;
			ScePsmDevice deviceFromGuid = GetDeviceFromGuid(guid);
			if (deviceFromGuid.type == 1)
			{
				string text = new string(deviceFromGuid.deviceID);
				if (!string.IsNullOrEmpty(text))
				{
					bool flag = true;
					Process process = AdbCmdStart($"-s {text} shell am start -n com.playstation.psmdevassistant/.PsmDevCommand", waitFinish: false);
					string text2 = process.StandardOutput.ReadToEnd();
					AdbCmdEnd(process);
					int num = text2.LastIndexOf("does not exist");
					if (num != -1)
					{
						flag = false;
					}
					if (flag)
					{
						Process process2 = AdbCmdStart($"-s {text} shell dumpsys package", waitFinish: false);
						string text3 = process2.StandardOutput.ReadToEnd();
						AdbCmdEnd(process2);
						string text4 = "com.playstation.psmdevassistant";
						int num2 = text3.IndexOf("Package [" + text4 + "]");
						if (num2 != -1)
						{
							string text5 = "versionCode=";
							int num3 = text3.IndexOf(text5, num2);
							int num4 = text3.IndexOf('\r', num3);
							int num5 = num3 + text5.Length;
							string text6 = text3.Substring(num5, num4 - num5);
							char[] anyOf = new char[2] { ' ', '\t' };
							int num6 = text6.IndexOfAny(anyOf, 0);
							if (num6 != -1)
							{
								text6 = text6.Substring(0, num6);
							}
							if (androidVersion == int.Parse(text6))
							{
								result = true;
							}
						}
					}
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		private static int InstallAssistant(Guid guid)
		{
			int num = 0;
			ScePsmDevice deviceFromGuid = GetDeviceFromGuid(guid);
			if (deviceFromGuid.type == 1)
			{
				WaitDialog waitDialog = new WaitDialog();
				waitDialog.Show();
				waitDialog.SetMessage(Utility.TextLanguage("Please wait.\nInstall PlayStation Mobile Development Assistant to device.", "PlayStation Mobile Development Assistant を\nデバイスにインストールしています。\nしばらくお待ちください。"));
				string text = new string(deviceFromGuid.deviceID);
				if (!string.IsNullOrEmpty(text))
				{
					string arg = PsmSdk.Location + "\\tools\\psmdevassistant.apk";
					Process process = AdbCmdStart($"-s {text} install -r \"{arg}\"");
					string s = process.StandardOutput.ReadToEnd();
					AdbCmdEnd(process);
					bool flag = true;
					while (flag)
					{
						flag = false;
						StringReader stringReader = new StringReader(s);
						string text2;
						while ((text2 = stringReader.ReadLine()) != null)
						{
							if (text2.StartsWith("Success"))
							{
								num = 0;
								break;
							}
							if (text2.StartsWith("Failure"))
							{
								num = -2147418094;
								if (text2.Equals("Failure [INSTALL_FAILED_VERSION_DOWNGRADE]"))
								{
									process = AdbCmdStart($"-s {text} install -r -d \"{arg}\"");
									s = process.StandardOutput.ReadToEnd();
									AdbCmdEnd(process);
									flag = true;
								}
								break;
							}
						}
					}
					if (num == 0 && waitDialog.DialogResult != DialogResult.OK)
					{
						num = -2147418094;
					}
				}
				waitDialog.Close();
				waitDialog.Dispose();
			}
			return num;
		}

		private static ScePsmDevice GetDeviceFromGuid(Guid guid)
		{
			ScePsmDevice result = default(ScePsmDevice);
			for (int i = 0; i < mDeviceNum; i++)
			{
				if (mDevices[i].guid == guid)
				{
					return mDevices[i];
				}
			}
			return result;
		}
	}
}
