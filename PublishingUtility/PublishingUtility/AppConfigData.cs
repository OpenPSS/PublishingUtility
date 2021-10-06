using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public class AppConfigData
	{
		public enum ProxySetting
		{
			SystemDefault,
			UseProxy,
			NotUseProxy
		}

		public int Version = 11000;

		public Size WindowSize;

		public string PcLocale = "";

		[XmlIgnore]
		public string EnvServer = "np";

		public string PathAppXml = "";

		[XmlIgnore]
		public string AppID;

		public string PsnID;

		[XmlIgnore]
		public string Password;

		public string Ep;

		[XmlIgnore]
		public Form mainForm;

		public ProxySetting proxySetting;

		public string saveProxyAddress;

		public int savePort;

		[XmlIgnore]
		public string ProxyAddress;

		[XmlIgnore]
		public int Port;

		[XmlIgnore]
		public string PublisherKeyName;

		[XmlIgnore]
		public string CreateTimeOfPublisherKey;

		[XmlIgnore]
		public DateTime dtCreateTimeOfPublisherKey;

		[XmlIgnore]
		public bool IsValidPublisherKey = true;

		public bool irp;

		[XmlIgnore]
		public string QAPublisherKeyName;

		[XmlIgnore]
		public string CreateTimeOfQAPublisherKey;

		[XmlIgnore]
		public DateTime dtCreateTimeOfQAPublisherKey;

		[XmlIgnore]
		public string PublisherKeyNameTmp;

		[XmlIgnore]
		public ScePsmDevice[] onlineDevices;

		[XmlIgnore]
		public List<string> listAppDevKey = new List<string>();

		[XmlIgnore]
		public List<DeviceStatus> listDevice = new List<DeviceStatus>();

		public string tmpFolderPath = "";

		public string tmpFolderPublisherKey = "";

		public string AssemblyVersionDateTime
		{
			get
			{
				string executablePath = Application.ExecutablePath;
				FileInfo fileInfo = new FileInfo(executablePath);
				return string.Concat(str2: fileInfo.LastWriteTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"), str0: Application.ProductVersion, str1: " - ");
			}
		}

		public AppConfigData()
		{
			GetAppDevKey();
			GetDeviceInfoInAppDataFolder();
		}

		public void Save()
		{
			if (!PublisherKeyUtility.ExistPublisherKey())
			{
				PublisherKeyName = "";
				CreateTimeOfPublisherKey = "";
			}
			if (Program.MainForm != null)
			{
				WindowSize = Program.MainForm.Size;
			}
			string userAppDataPath = Utility.UserAppDataPath;
			if (!Directory.Exists(userAppDataPath))
			{
				Directory.CreateDirectory(userAppDataPath);
			}
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfigData));
				using FileStream stream = new FileStream(userAppDataPath + "\\AppConfig.xml", FileMode.Create);
				xmlSerializer.Serialize(stream, this);
			}
			catch (IOException)
			{
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message, "Save AppConfig - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		public void SetProxyData()
		{
			if (proxySetting == ProxySetting.NotUseProxy)
			{
				ProxyAddress = "";
				Port = 0;
			}
			else if (proxySetting == ProxySetting.SystemDefault)
			{
				Utility.GetDefaultProxy(out var proxyAddress, out var port);
				ProxyAddress = proxyAddress;
				Port = port;
			}
			else
			{
				ProxyAddress = saveProxyAddress;
				Port = savePort;
			}
		}

		public static bool Load(out AppConfigData data)
		{
			data = null;
			string text = Utility.UserAppDataPath + "\\AppConfig.xml";
			try
			{
				if (File.Exists(text))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppConfigData));
					using FileStream stream = new FileStream(text, FileMode.Open);
					try
					{
						data = (AppConfigData)xmlSerializer.Deserialize(stream);
					}
					catch (Exception ex)
					{
						MessageBox.Show("AppConfig.xml is invalid.\n\n" + ex.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return false;
					}
					data.SetProxyData();
					Console.WriteLine("EnvServer=" + data.EnvServer);
					return true;
				}
				Console.WriteLine(text + " does not exist.");
			}
			catch (Exception ex2)
			{
				MessageBox.Show("Failed to load AppConfig.xml.\n\n" + ex2.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return false;
		}

		public void Initialize()
		{
			InitializePublisherKeyInfo();
			InitializeLocale();
		}

		private void InitializePublisherKeyInfo()
		{
			if (PublisherKeyUtility.ExistPublisherKey() && PublisherKeyUtility.GetPublisherKeyInfo(Program.PATH_FILE_PUBLISHER_KEY, out var strCertCommonName, out var strCertCreateTime))
			{
				PublisherKeyName = strCertCommonName;
				CreateTimeOfPublisherKey = Utility.ConvertLocaleDateTime(strCertCreateTime);
				dtCreateTimeOfPublisherKey = DateTime.Parse(strCertCreateTime);
			}
			else
			{
				PublisherKeyName = "";
				CreateTimeOfPublisherKey = "";
			}
		}

		private void InitializeQAPublisherKeyInfo()
		{
			if (File.Exists(Program.PATH_FILE_PUBLISHER_KEY_QA) && PublisherKeyUtility.GetPublisherKeyInfo(Program.PATH_FILE_PUBLISHER_KEY_QA, out var strCertCommonName, out var strCertCreateTime))
			{
				QAPublisherKeyName = strCertCommonName;
				CreateTimeOfQAPublisherKey = Utility.ConvertLocaleDateTime(strCertCreateTime);
				dtCreateTimeOfQAPublisherKey = DateTime.Parse(strCertCreateTime);
			}
			else
			{
				QAPublisherKeyName = "";
				CreateTimeOfQAPublisherKey = "";
			}
		}

		private void InitializeLocale()
		{
			string pcLocale = PcLocale;
			if (string.IsNullOrEmpty(pcLocale) || pcLocale == "Default")
			{
				PcLocale = CultureInfo.CurrentCulture.Name;
			}
			else if (pcLocale == "ja-JP")
			{
				PcLocale = "ja-JP";
				Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
				Resources.Culture = CultureInfo.GetCultureInfo("ja-JP");
			}
			else
			{
				PcLocale = "en-US";
				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
				Resources.Culture = CultureInfo.GetCultureInfo("en-US");
			}
		}

		public void GetAppDevKey()
		{
			listAppDevKey.Clear();
			string path = Utility.UserAppDataPath + "\\HostAppsKey\\";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string[] files = Directory.GetFiles(path, "*.khapp");
			foreach (string path2 in files)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path2);
				Console.WriteLine(fileNameWithoutExtension);
				listAppDevKey.Add(fileNameWithoutExtension);
			}
		}

		public void GetDeviceInfoInAppDataFolder()
		{
			listDevice.Clear();
			string pATH_DIR_DEVICE_SEED = Program.PATH_DIR_DEVICE_SEED;
			if (!Directory.Exists(pATH_DIR_DEVICE_SEED))
			{
				Directory.CreateDirectory(pATH_DIR_DEVICE_SEED);
			}
			string[] files = Directory.GetFiles(pATH_DIR_DEVICE_SEED, "*.seed");
			foreach (string pathFile in files)
			{
				DeviceStatus deviceStatus = new DeviceStatus(pathFile);
				if (deviceStatus.IsCorrect)
				{
					listDevice.Add(deviceStatus);
				}
			}
		}

		public void GetDeviceInfoConnectedPC()
		{
			ScePsmDevice[] devices;
			int listDevices = PsmDeviceFuncBinding.GetListDevices(out devices);
			UpdateDeviceList(devices, listDevices);
		}

		public void UpdateDeviceList(ScePsmDevice[] m_devices, int num)
		{
			foreach (DeviceStatus item in listDevice)
			{
				item.IsUsbConnected = false;
			}
			for (int i = 0; i < num; i++)
			{
				string deviceID = Utility.SafeConvertCharArrayToString(m_devices[i].deviceID);
				DeviceStatus deviceFromList = GetDeviceFromList(deviceID);
				if (deviceFromList != null)
				{
					deviceFromList.deviceType = (DeviceType)m_devices[i].type;
					deviceFromList.IsUsbConnected = true;
					deviceFromList.IsDevAssistantOn = ((m_devices[i].online != 0) ? true : false);
					deviceFromList.DeviceGUID = m_devices[i].guid;
				}
				else
				{
					Program.appConfigData.AddDeviceList(m_devices[i]);
				}
			}
		}

		public List<DeviceStatus> GetOnlineDeviceList()
		{
			List<DeviceStatus> list = new List<DeviceStatus>();
			foreach (DeviceStatus item in listDevice)
			{
				if (item.IsUsbConnected)
				{
					list.Add(item);
				}
			}
			return list;
		}

		public int GetNumOfOnlineDevice()
		{
			int num = 0;
			foreach (DeviceStatus item in listDevice)
			{
				if (item.IsUsbConnected)
				{
					num++;
				}
			}
			return num;
		}

		public List<DeviceStatus> GetNoSeedDeviceList()
		{
			List<DeviceStatus> list = new List<DeviceStatus>();
			foreach (DeviceStatus item in listDevice)
			{
				if (item.IsUsbConnected && !item.HasSeed)
				{
					list.Add(item);
				}
			}
			return list;
		}

		public int GetCountOfDeviceSeedorConnectedDevice()
		{
			int num = 0;
			foreach (DeviceStatus item in listDevice)
			{
				if (item.HasSeed || item.IsUsbConnected)
				{
					num++;
				}
			}
			return num;
		}

		public int GetCountOfDeviceSeed()
		{
			int num = 0;
			foreach (DeviceStatus item in listDevice)
			{
				if (item.HasSeed)
				{
					num++;
				}
			}
			return num;
		}

		private DeviceStatus GetDeviceFromList(string deviceID)
		{
			foreach (DeviceStatus item in listDevice)
			{
				if (item.DeviceID == deviceID)
				{
					return item;
				}
			}
			return null;
		}

		private void AddDeviceList(ScePsmDevice scePsmDevice)
		{
			DeviceStatus item = new DeviceStatus(scePsmDevice);
			listDevice.Add(item);
		}
	}
}
