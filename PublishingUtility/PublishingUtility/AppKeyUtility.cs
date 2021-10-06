using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class AppKeyUtility
	{
		private static string m_applicationID;

		private static string m_deviceID;

		private static string m_psnID;

		private static string m_password;

		private static long m_accountID_psmp;

		private static DeviceStatus m_deviceStatus;

		public static bool GererateAppKeyRingAll(string appID, string psnID, string password)
		{
			Console.WriteLine("GererateAppKeyRing()");
			if (!GenerateAppDevKey(appID, psnID, password))
			{
				Console.WriteLine("Failed to GenerateAppDevKey()");
				return false;
			}
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			if (Program.appConfigData.listDevice.Count == 0)
			{
				MessageBox.Show(Utility.TextLanguage("Device seed doesn't exist.", "デバイスシードが存在しません。"), "Gererate App Key Ring - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			if (!GenerateAppExeKeys(appID, psnID, password))
			{
				Console.WriteLine("Failed to Generate App Exe Key.");
				return false;
			}
			if (!GenerateAppKeyRing(appID))
			{
				return false;
			}
			Program.appConfigData.GetAppDevKey();
			return true;
		}

		public static bool GenerateAppKeyRing(string appID)
		{
			AppKeyRing appKeyRing = new AppKeyRing();
			List<string> appExeKeyList = null;
			if (appKeyRing.GenerateKeyRing(appID, out appExeKeyList))
			{
				string text = "";
				foreach (string item in appExeKeyList)
				{
					text = text + "- " + item + "\n";
				}
				MessageBox.Show(Utility.TextLanguage(string.Format("Succeeded to generate App Key Ring of {0}.\nApp Key Ring of {0} includes app keys of following devices:\n{1}", appID, text), string.Format("{0}の鍵束を作成しました。\n{0}の鍵束には次のデバイスのアプリ鍵が含まれます。\n{1}", appID, text)), "Generate App Key Ring - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.None);
				return true;
			}
			MessageBox.Show(string.Format(Resources.failedToGenerate_Text, Utility.TextLanguage("App Key Ring", "鍵束")), Utility.TextLanguage("Generate App Key Ring", "アプリ鍵束の作成"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}

		public static bool GenerateAppDevKey(string appID, string psnID, string password)
		{
			bool result = false;
			m_applicationID = appID;
			m_psnID = psnID;
			m_password = password;
			ProgressDialog progressDialog = new ProgressDialog("Generate App Dev Key", doWorkGenerateAppDevKey, 100);
			DialogResult dialogResult = progressDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				result = true;
			}
			progressDialog.Dispose();
			return result;
		}

		private static void doWorkGenerateAppDevKey(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			if (!PublisherKeyUtility.LoadPublisherKeyData(out var byteArray))
			{
				e.Cancel = true;
				return;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(1024);
			IntPtr intPtr2 = Marshal.AllocHGlobal(32);
			byte[] array = new byte[1024];
			byte[] array2 = new byte[32];
			backgroundWorker.ReportProgress(20, "Initialize...");
			HostKdbAcquire._scePsmDrmGetHostKdbgInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			backgroundWorker.ReportProgress(80, "Generating...");
			ScePsmDrmStatus scePsmDrmStatus = HostKdbAcquire._scePsmDrmGetHostKdbg(m_psnID, m_password, m_applicationID, byteArray, byteArray.Length, intPtr, intPtr2);
			HostKdbAcquire._scePsmDrmGetHostKdbgTerm();
			Console.WriteLine(scePsmDrmStatus.ToString());
			if (scePsmDrmStatus != 0)
			{
				MessageBox.Show(Utility.TextLanguage("Failed to generate App Dev Key.\n\n", "アプリ開発鍵の作成に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Generate App Dev Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
				e.Cancel = true;
				return;
			}
			Marshal.Copy(intPtr, array, 0, 1024);
			Marshal.Copy(intPtr2, array2, 0, array2.Length);
			Utility.ByteArrayToString(array2);
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			backgroundWorker.ReportProgress(90, "Generating Key file...");
			string path = Utility.UserAppDataPath + "\\HostAppsKey\\";
			if (m_applicationID == "*")
			{
				if (!Utility.SaveFileByteArray(path, "+asterisk+.khapp", array))
				{
					e.Cancel = true;
				}
			}
			else if (!Utility.SaveFileByteArray(path, m_applicationID + ".khapp", array))
			{
				e.Cancel = true;
			}
		}

		public static bool GenerateAppExeKey(string deviceID, string applicationID, string psnID, string password)
		{
			bool result = false;
			m_applicationID = applicationID;
			m_deviceID = deviceID;
			m_psnID = psnID;
			m_password = password;
			ProgressDialog progressDialog = new ProgressDialog("Generate App Key", doWorkGenerateAppExeKey, 100);
			DialogResult dialogResult = progressDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				result = true;
			}
			progressDialog.Dispose();
			return result;
		}

		private static void doWorkGenerateAppExeKey(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			if (!PublisherKeyUtility.LoadPublisherKeyData(out var byteArray))
			{
				e.Cancel = true;
				return;
			}
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			DeviceStatus deviceStatus = new DeviceStatus(DeviceSeedUtility.GetDeviceSeedPathFile(m_deviceID));
			if (!deviceStatus.IsCorrect)
			{
				MessageBox.Show($"Failed to generate App Exe Key\n({m_applicationID} - {m_deviceID}).\n\n", "new Device Status - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
				return;
			}
			backgroundWorker.ReportProgress(90, $"Generating App Key - {m_deviceID}...");
			IntPtr intPtr = Marshal.AllocHGlobal(512);
			IntPtr intPtr2 = Marshal.AllocHGlobal(1072);
			IntPtr intPtr3 = Marshal.AllocHGlobal(32);
			ScePsmDrmStatus scePsmDrmStatus = TargetKdbgAcquirer._scePsmDrmGetTargetKdbg(m_psnID, m_password, deviceStatus.C1, m_applicationID, byteArray, byteArray.Length, intPtr, intPtr2, intPtr3);
			if (scePsmDrmStatus != 0)
			{
				TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
				MessageBox.Show(Utility.TextLanguage($"Failed to generate App Exe Key\n({m_applicationID} - {m_deviceID}).\n\n", $"アプリ実行鍵({m_applicationID} - {m_deviceID})\nの作成に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Generate App Exe Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
				return;
			}
			string text = ((!(m_applicationID == "*")) ? (Program.PATH_DIR_TARGET_APPS_KEY + m_applicationID + "\\") : (Program.PATH_DIR_TARGET_APPS_KEY + "+asterisk+\\"));
			AppExeKey appExeKey = new AppExeKey();
			appExeKey.Nickname = deviceStatus.DeviceID;
			Marshal.Copy(intPtr3, appExeKey.Expiration, 0, appExeKey.Expiration.Length);
			Marshal.Copy(intPtr2, appExeKey.EncTargetKdb, 0, appExeKey.EncTargetKdb.Length);
			Marshal.Copy(intPtr, appExeKey.R1, 0, appExeKey.R1.Length);
			appExeKey.OpenConsoleID = deviceStatus.OpenConsoleID;
			appExeKey.Save(text + m_deviceID + ".ktapp");
			Console.WriteLine("Expiration=" + appExeKey.GetExpirationString());
			Console.WriteLine("Succeeded to generate " + m_deviceID + ".ktapp");
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
		}

		public static bool GenerateAppExeKeys(string applicationID, string psnID, string password)
		{
			Console.WriteLine("Generate App Exe Keys");
			bool result = false;
			m_applicationID = applicationID;
			m_psnID = psnID;
			m_password = password;
			ProgressDialog progressDialog = new ProgressDialog("Generate App Key", doWorkGenerateAppExeKeys, 100);
			DialogResult dialogResult = progressDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				result = true;
			}
			progressDialog.Dispose();
			return result;
		}

		private static void doWorkGenerateAppExeKeys(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			if (Program.appConfigData.listDevice.Count == 0)
			{
				MessageBox.Show(Utility.TextLanguage("Cannot find device.\n", "デバイスが見つかりません。\n"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
				return;
			}
			if (!PublisherKeyUtility.LoadPublisherKeyData(out var byteArray))
			{
				e.Cancel = true;
				return;
			}
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			int num = 60 / Program.appConfigData.listDevice.Count;
			int num2 = 0;
			foreach (DeviceStatus item in Program.appConfigData.listDevice)
			{
				backgroundWorker.ReportProgress(30 + num2 * num, $"Generating App Exe Key - {item.Nickname}...");
				num2++;
				IntPtr intPtr = Marshal.AllocHGlobal(512);
				IntPtr intPtr2 = Marshal.AllocHGlobal(1072);
				IntPtr intPtr3 = Marshal.AllocHGlobal(32);
				ScePsmDrmStatus scePsmDrmStatus = TargetKdbgAcquirer._scePsmDrmGetTargetKdbg(m_psnID, m_password, item.C1, m_applicationID, byteArray, byteArray.Length, intPtr, intPtr2, intPtr3);
				if (scePsmDrmStatus != 0)
				{
					TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
					MessageBox.Show(Utility.TextLanguage("Failed to generate App Exe Key.\n", "アプリ実行鍵の作成に失敗しました。\n") + $"({m_applicationID} - {item.Nickname} )\n\n" + DrmError.GetErrorMessage(scePsmDrmStatus), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					e.Cancel = true;
					return;
				}
				string text = ((!(m_applicationID == "*")) ? (Utility.UserAppDataPath + "\\TargetAppsKey\\" + m_applicationID + "\\") : (Utility.UserAppDataPath + "\\TargetAppsKey\\+asterisk+\\"));
				AppExeKey appExeKey = new AppExeKey();
				appExeKey.Nickname = item.Nickname;
				Marshal.Copy(intPtr3, appExeKey.Expiration, 0, appExeKey.Expiration.Length);
				Marshal.Copy(intPtr2, appExeKey.EncTargetKdb, 0, appExeKey.EncTargetKdb.Length);
				Marshal.Copy(intPtr, appExeKey.R1, 0, appExeKey.R1.Length);
				appExeKey.OpenConsoleID = item.OpenConsoleID;
				appExeKey.Save(text + item.DeviceID + ".ktapp");
				Console.WriteLine("Expiration=" + appExeKey.GetExpirationString());
				Console.WriteLine("Succeeded to generate " + item.DeviceID + ".ktapp");
			}
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
		}

		public static bool GenerateAppExeKeyQA(string appID, string psnID, string password, long accountID_psmp, DeviceStatus deviceStatus)
		{
			Console.WriteLine("Generate App Exe Key");
			bool result = false;
			m_applicationID = appID;
			m_psnID = psnID;
			m_password = password;
			m_accountID_psmp = accountID_psmp;
			m_deviceStatus = deviceStatus;
			ProgressDialog progressDialog = new ProgressDialog("Generate App Key (QA)", doWorkGenerateAppExeKeyQA, 100);
			DialogResult dialogResult = progressDialog.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				result = true;
			}
			progressDialog.Dispose();
			return result;
		}

		private static void doWorkGenerateAppExeKeyQA(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			if (!PublisherKeyUtility.LoadPublisherKeyQaData(out var byteArray))
			{
				e.Cancel = true;
				return;
			}
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			backgroundWorker.ReportProgress(85, $"Generating App Key - {m_deviceStatus.Nickname}...");
			IntPtr intPtr = Marshal.AllocHGlobal(512);
			IntPtr intPtr2 = Marshal.AllocHGlobal(1072);
			IntPtr intPtr3 = Marshal.AllocHGlobal(32);
			ScePsmDrmStatus scePsmDrmStatus = TargetKdbgAcquirer._scePsmDrmGetTitleQaTargetKdbg(m_psnID, m_password, m_deviceStatus.C1, m_applicationID, byteArray, byteArray.Length, m_accountID_psmp, intPtr, intPtr2, intPtr3);
			if (scePsmDrmStatus != 0)
			{
				TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
				MessageBox.Show(Utility.TextLanguage("Failed to generate App Exe Key.\n", "アプリ実行鍵の作成に失敗しました。\n") + $"({m_applicationID} - {m_deviceStatus.Nickname} )\n\n" + DrmError.GetErrorMessage(scePsmDrmStatus), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
				return;
			}
			string text = ((!(m_applicationID == "*")) ? (Utility.UserAppDataPath + "\\TargetAppsKey\\" + m_applicationID + "\\") : (Utility.UserAppDataPath + "\\TargetAppsKey\\+asterisk+\\"));
			AppExeKey appExeKey = new AppExeKey();
			appExeKey.Nickname = m_deviceStatus.Nickname;
			Marshal.Copy(intPtr3, appExeKey.Expiration, 0, appExeKey.Expiration.Length);
			Marshal.Copy(intPtr2, appExeKey.EncTargetKdb, 0, appExeKey.EncTargetKdb.Length);
			Marshal.Copy(intPtr, appExeKey.R1, 0, appExeKey.R1.Length);
			appExeKey.OpenConsoleID = m_deviceStatus.OpenConsoleID;
			appExeKey.Save(text + m_deviceStatus.DeviceID + ".ktapp");
			Console.WriteLine("Succeeded to generate " + m_deviceStatus.DeviceID + ".ktapp");
			TargetKdbgAcquirer._scePsmDrmGetTargetKdbgTerm();
		}

		public static bool ExistAppExeKey()
		{
			string pATH_DIR_TARGET_APPS_KEY = Program.PATH_DIR_TARGET_APPS_KEY;
			if (!Directory.Exists(pATH_DIR_TARGET_APPS_KEY))
			{
				return false;
			}
			string[] files = Directory.GetFiles(pATH_DIR_TARGET_APPS_KEY, "*.ktapp", SearchOption.AllDirectories);
			if (files.Length == 0)
			{
				return false;
			}
			return true;
		}

		public static bool ExistAppKey(string applicationID)
		{
			if (!ExistAppDevKey(applicationID))
			{
				return false;
			}
			if (!ExistAppExeKey(applicationID))
			{
				return false;
			}
			return true;
		}

		public static bool ExistAppKeyRing()
		{
			string pATH_DIR_TARGET_APPS_KEY = Program.PATH_DIR_TARGET_APPS_KEY;
			if (!Directory.Exists(pATH_DIR_TARGET_APPS_KEY))
			{
				return false;
			}
			string[] files = Directory.GetFiles(pATH_DIR_TARGET_APPS_KEY, "*.krng", SearchOption.AllDirectories);
			if (files.Length == 0)
			{
				return false;
			}
			return true;
		}

		public static string GetAppDevKeyPathFile(string applicationID)
		{
			return Utility.UserAppDataPath + "\\HostAppsKey\\" + applicationID + ".khapp";
		}

		public static bool ExistAppDevKey(string applicationID)
		{
			return File.Exists(GetAppDevKeyPathFile(applicationID));
		}

		public static DateTime GetTimeStampAppDevKey(string applicationID)
		{
			return File.GetLastWriteTime(GetAppDevKeyPathFile(applicationID));
		}

		public static string GetAppExeKeyPathFile(string applicationID, string deviceName)
		{
			return Utility.UserAppDataPath + "\\TargetAppsKey\\" + applicationID + "\\" + deviceName + ".ktapp";
		}

		public static bool ExistAppExeKey(string applicationID, string deviceName)
		{
			return File.Exists(GetAppExeKeyPathFile(applicationID, deviceName));
		}

		public static bool ExistAppExeKey(string applicationID)
		{
			if (!Directory.Exists(Program.PATH_DIR_TARGET_APPS_KEY))
			{
				return false;
			}
			string[] files = Directory.GetFiles(Program.PATH_DIR_TARGET_APPS_KEY + applicationID, "*.ktapp", SearchOption.AllDirectories);
			if (files.Length == 0)
			{
				return false;
			}
			return true;
		}

		public static DateTime GetTimeStampAppExeKey(string applicationID, string deviceName)
		{
			return File.GetLastWriteTime(GetAppExeKeyPathFile(applicationID, deviceName));
		}
	}
}
