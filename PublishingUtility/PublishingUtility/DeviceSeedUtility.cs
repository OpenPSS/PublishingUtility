using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PublishingUtility.KeyManagement;

namespace PublishingUtility
{
	public class DeviceSeedUtility
	{
		private static Guid m_guid;

		private static string m_outputPathFilename;

		private static string m_deviceName;

		public static void GenerateDeviceSeedofOnlineDevices(bool regenerateAll)
		{
			Console.WriteLine("GenerateDeviceSeed");
			ScePsmDevice[] devices;
			int listDevices = PsmDeviceFuncBinding.GetListDevices(out devices);
			string text = Utility.UserAppDataPath + "\\DeviceSeed\\";
			for (int i = 0; i < listDevices; i++)
			{
				if (!regenerateAll)
				{
					foreach (DeviceStatus item in Program.appConfigData.listDevice)
					{
						if (!(item.DeviceGUID == devices[i].guid) || !item.HasSeed)
						{
							continue;
						}
						Console.WriteLine("Skip to Generate Device seed of {0}.", item.DeviceGUID);
						goto IL_0129;
					}
				}
				DeviceType type = (DeviceType)devices[i].type;
				string text2 = Utility.SafeConvertCharArrayToString(devices[i].deviceID);
				Console.WriteLine("Try to connect device:{0}...", text2);
				Console.WriteLine("Generate Device Seed now...");
				string text3 = text2 + ".seed";
				if (GenerateDeviceSeed(devices[i].guid, text + text3, type.ToString() + ":" + text2))
				{
					Console.WriteLine("Succeeded to generate Device Seed of {0}.", text3);
				}
				IL_0129:;
			}
		}

		private static bool GenerateDeviceSeed(Guid guid, string outputPathFilename, string deviceName)
		{
			m_guid = guid;
			m_outputPathFilename = outputPathFilename;
			m_deviceName = deviceName;
			ProgressDialog progressDialog = new ProgressDialog("Generate Device Seed ", doWorkGenerateDeviceSeed, 100);
			DialogResult dialogResult = progressDialog.ShowDialog();
			bool result = ((dialogResult == DialogResult.OK) ? true : false);
			progressDialog.Dispose();
			return result;
		}

		public static bool GenerateDeviceSeed(string deviceID)
		{
			string pATH_DIR_DEVICE_SEED = Program.PATH_DIR_DEVICE_SEED;
			bool result = false;
			ScePsmDevice[] devices;
			int listDevices = PsmDeviceFuncBinding.GetListDevices(out devices);
			if (listDevices <= 0)
			{
				MessageBox.Show(Utility.TextLanguage("The device is not connected to a PC. \nTo create a Device Seed, the device must be connected to a PC by a USB cable.", "デバイスがPCに接続されていません。\nデバイスシードを作成するには、デバイスをUSBケーブルでPCに接続しておく必要があります。"), "Generate Device Seed - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return result;
			}
			for (int i = 0; i < listDevices; i++)
			{
				DeviceType type = (DeviceType)devices[i].type;
				if (Utility.SafeConvertCharArrayToString(devices[i].deviceID) == deviceID)
				{
					Console.WriteLine("Try to connect device:{0}...", deviceID);
					Console.WriteLine("Generate Device Seed now...");
					string text = deviceID + ".seed";
					if (GenerateDeviceSeed(devices[i].guid, pATH_DIR_DEVICE_SEED + text, type.ToString() + ":" + deviceID))
					{
						Console.WriteLine("Succeeded to generate Device Seed of {0}.", deviceID);
						return true;
					}
					Console.WriteLine("Failed to generate Device Seed of {0}.", deviceID);
					return false;
				}
			}
			MessageBox.Show(Utility.TextLanguage($"The device {deviceID} is not connected to a PC. \nTo create a Device Seed, the device must be connected to a PC by a USB cable.", $"デバイス {deviceID} はPCに接続されていません。\nデバイスシードを作成するには、デバイスをUSBケーブルでPCに接続しておく必要があります。"), "Generate Device Seed - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}

		private static void doWorkGenerateDeviceSeed(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(70, $"Generating Device Seed of {m_deviceName}...");
			int deviceSeed = PsmDeviceFuncBinding.GetDeviceSeed(m_guid, m_outputPathFilename);
			Console.WriteLine(string.Format("GetDeviceSeed retValue={0}, 0x{0:x}", deviceSeed));
			if (deviceSeed != 0)
			{
				e.Cancel = true;
			}
			backgroundWorker.ReportProgress(100, "Generating....");
			Thread.Sleep(1000);
		}

		public static void ExportDeviceSeed()
		{
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			Program.appConfigData.GetDeviceInfoConnectedPC();
			for (int i = 0; i < Program.appConfigData.listDevice.Count; i++)
			{
				if (Program.appConfigData.listDevice[i].HasSeed)
				{
					Form form = new ExportDeviceSeed01(Program.appConfigData.listDevice[i], i + 1, Program.appConfigData.listDevice.Count);
					DialogResult dialogResult = form.ShowDialog();
					if (dialogResult != DialogResult.OK && dialogResult != DialogResult.Ignore)
					{
						_ = 3;
						break;
					}
				}
			}
		}

		public static string GetDeviceSeedPathFile(string deviceID)
		{
			return Utility.UserAppDataPath + "\\DeviceSeed\\" + deviceID + ".seed";
		}

		public static bool ExistDeviceSeed(string deviceID)
		{
			return File.Exists(GetDeviceSeedPathFile(deviceID));
		}

		public static DateTime GetTimeStamp(string deviceID)
		{
			return File.GetLastWriteTime(GetDeviceSeedPathFile(deviceID));
		}

		public static void GenerateDeviceSeedofOnlineDevices()
		{
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			Program.appConfigData.GetDeviceInfoConnectedPC();
			List<DeviceStatus> noSeedDeviceList = Program.appConfigData.GetNoSeedDeviceList();
			if (noSeedDeviceList.Count > 0)
			{
				GenerateDeviceSeedofOnlineDevices(regenerateAll: false);
			}
		}
	}
}
