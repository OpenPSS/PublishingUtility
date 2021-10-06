using System;
using System.IO;
using System.Windows.Forms;

namespace PublishingUtility
{
	internal static class Program
	{
		public const string FILENAME_APP_CONFIG_XML = "AppConfig.xml";

		public const string TitleName = "Publishing Utility";

		public const string FILENAME_USER_APP_CONFIG = "AppConfig.xml";

		public const string FILENAME_KDEV = "kdev.p12";

		public const string FILENAME_KDEV_QA = "kdevQa.p12";

		public const string DIR_PUBLISHER_KEY = "\\PublisherKey\\";

		public const string DIR_HOST_APPS_KEY = "\\HostAppsKey\\";

		public const string DIR_DEVICE_SEED = "\\DeviceSeed\\";

		public const string DIR_TARGET_APPS_KEY = "\\TargetAppsKey\\";

		public const string BASENAME_ASTERRISK = "+asterisk+";

		public const string APP_ID_DEFAULT = "*";

		public static AppConfigData appConfigData;

		public static SceRatingData _RatingData;

		private static MainForm mainForm;

		public static bool CheckDeviceConnection = true;

		public static MainForm MainForm => mainForm;

		public static string PATH_FILE_PUBLISHER_KEY => Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12";

		public static string PATH_FILE_PUBLISHER_KEY_QA => Utility.UserAppDataPath + "\\PublisherKey\\kdevQa.p12";

		public static string PATH_DIR_DEVICE_SEED => Utility.UserAppDataPath + "\\DeviceSeed\\";

		public static string PATH_DIR_HOST_APPS_KEY => Utility.UserAppDataPath + "\\HostAppsKey\\";

		public static string PATH_DIR_TARGET_APPS_KEY => Utility.UserAppDataPath + "\\TargetAppsKey\\";

		public static string PATH_FILE_APP_CONFIG_XML => Utility.UserAppDataPath + "\\AppConfig.xml";

		public static string GetAppDevKey(string applicationID)
		{
			if (applicationID == "*")
			{
				applicationID = "+asterisk+";
			}
			return PATH_DIR_HOST_APPS_KEY + "\\" + applicationID + ".khapp";
		}

		public static string GetAppExeKey(string applicationID, string deviceID)
		{
			return PATH_DIR_TARGET_APPS_KEY + applicationID + "\\" + deviceID + ".ktapp";
		}

		[STAThread]
		private static int Main(string[] args)
		{
			int result = 0;
			if (!AppConfigData.Load(out appConfigData))
			{
				appConfigData = new AppConfigData();
			}
			PsmDeviceFuncBinding.Initialize();
			KPubGenerator.Initialize();
			HostKdbAcquire.Initialize();
			TargetKdbgAcquirer.Initialize();
			appConfigData.Initialize();
			_RatingData = new SceRatingData();
			_RatingData.Reset();
			if (args.Length == 0 || (args.Length == 1 && File.Exists(args[0]) && Path.GetExtension(args[0]) == ".xml") || (args.Length == 1 && args[0].Trim().Equals("--open_keymanagement_panel")))
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(defaultValue: false);
				if (args.Length == 1 && args[0].Trim().Equals("--open_keymanagement_panel"))
				{
					mainForm = new MainForm(1);
				}
				else
				{
					mainForm = new MainForm();
				}
				if (appConfigData.WindowSize.Width >= 100 && appConfigData.WindowSize.Height >= 100)
				{
					mainForm.Size = appConfigData.WindowSize;
				}
				if (args.Length == 1 && File.Exists(args[0]) && Path.GetExtension(args[0]) == ".xml")
				{
					mainForm.LoadMeta(args[0]);
				}
				Application.Run(mainForm);
			}
			else
			{
				Utility.AttachConsole();
				Console.WriteLine("");
				if (args.Length == 5 && (args[0].Trim().Equals("--package") || args[0].Trim().Equals("-p")))
				{
					Console.WriteLine("Packaging");
					result = Command.CreateMasterPackage(args[1], args[2], args[3], args[4]);
				}
				else if (args.Length == 3 && (args[0].Trim().Equals("--compile") || args[0].Trim().Equals("-c")))
				{
					Console.WriteLine("Compiling \"{0}\" into \"{1}\"", args[1], args[2]);
					result = Command.CompileMetaData(args[1], args[2]);
				}
				else if ((args.Length == 1 || args.Length == 2) && (args[0].Trim().Equals("--submit") || args[0].Trim().Equals("-s")))
				{
					string text = ((args.Length == 2) ? args[1] : "");
					Console.WriteLine("Submit \"{0}\"", text);
					result = Command.Submit(text);
				}
				else if (args.Length == 3 && args[0].Trim().Equals("--updatekey"))
				{
					Console.WriteLine($"--updatekey {args[1]}  {args[2]}");
					KeyUtility.KeyCheckResult keyCheckResult = KeyUtility.UpdateKey2(args[1], args[2]);
					appConfigData.Save();
					switch (keyCheckResult)
					{
					case KeyUtility.KeyCheckResult.Valid_Update:
						result = 0;
						break;
					case KeyUtility.KeyCheckResult.Valid_NoUpdate:
						result = 1;
						break;
					default:
						Console.WriteLine("Failed to update key.");
						result = -1;
						break;
					}
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--generate_publisherkey"))
				{
					if (PublisherKeyUtility.GeneratePublishKey(PublisherKeyType.Developer))
					{
						result = 0;
					}
					else
					{
						Console.WriteLine("Failed to --generate_publisherkey.");
						result = -1;
					}
					appConfigData.Save();
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--check_publisherkey"))
				{
					if (PublisherKeyUtility.CheckPublisherKey())
					{
						result = 0;
					}
					else
					{
						Console.WriteLine("Failed to --check_publisherkey.");
						result = -1;
					}
					appConfigData.Save();
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--import_publisherkey"))
				{
					if (PublisherKeyUtility.ImportPublisherKeyWithCheck())
					{
						result = 0;
					}
					else
					{
						Console.WriteLine("Failed to --import_publisherkey.");
						result = -1;
					}
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--export_publisherkey"))
				{
					if (PublisherKeyUtility.ExportPublisherKey())
					{
						result = 0;
					}
					else
					{
						Console.WriteLine("Failed to --export_publisherkey.");
						result = -1;
					}
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--check_status_of_publisher_license"))
				{
					MessageBox.Show("--check_status_of_publisher_license");
					result = 0;
				}
				else if (args.Length == 1 && args[0].Trim().Equals("--delete_all_key"))
				{
					KeyUtility.DeleteAllDeviceSeedAndAppKey2();
				}
				else
				{
					string text2 = "";
					Console.Write("Invalid arguments: ");
					foreach (string text3 in args)
					{
						Console.Write("{0} ", text3);
						text2 += text3;
					}
					Console.WriteLine("");
					MessageBox.Show("ERROR: Invalid arguments = " + text2, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					result = -1;
				}
				Utility.DetachConsole();
			}
			return result;
		}
	}
}
