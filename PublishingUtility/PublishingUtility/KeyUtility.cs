using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PublishingUtility.KeyManagement;

namespace PublishingUtility
{
	public class KeyUtility
	{
		public enum KeyCheckResult
		{
			Valid_NoUpdate,
			Valid_Update,
			Invalid
		}

		public static bool SignInSENID_PasswordDialog()
		{
			Form form = new SignInSENID_Password();
			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				return false;
			}
			return true;
		}

		public static long ReadAccountIdFromKdevP12(byte[] bsKdev12)
		{
			long[] array = new long[1];
			IntPtr intPtr = Marshal.AllocHGlobal(bsKdev12.Length);
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)));
			Marshal.Copy(bsKdev12, 0, intPtr, bsKdev12.Length);
			KPubGenerator._scePsmDrmGenerateKpubInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			KPubGenerator._scePsmDrmGetAccountId(intPtr, bsKdev12.Length, intPtr2);
			KPubGenerator._scePsmDrmGenerateKpubTerm();
			Marshal.Copy(intPtr2, array, 0, 1);
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			return array[0];
		}

		public static DateTime GetDateTimeOfInstallDevAssistant(string deviceID)
		{
			return new DateTime(2013, 3, 1);
		}

		public static KeyCheckResult UpdateKey2(string deviceID, string applicationID, bool IsRegenerateAppKey = false)
		{
			KeyCheckResult keyCheckResult = UpdateKey(deviceID, applicationID, IsRegenerateAppKey);
			switch (keyCheckResult)
			{
			case KeyCheckResult.Valid_Update:
				MessageBox.Show(Utility.TextLanguage($"Succeeded to generate App Key ({deviceID} - {applicationID}).", $"アプリ鍵 ({deviceID} - {applicationID}) の作成に成功しました。"), "Update Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				break;
			default:
				Console.WriteLine("Failed to generate key.");
				break;
			case KeyCheckResult.Valid_NoUpdate:
				break;
			}
			return keyCheckResult;
		}

		public static KeyCheckResult UpdateKey(string deviceID, string applicationID, bool IsRegenerateAppKey = false)
		{
			KeyCheckResult result = KeyCheckResult.Valid_NoUpdate;
			bool flag = false;
			if (string.IsNullOrEmpty(deviceID) || string.IsNullOrEmpty(applicationID))
			{
				MessageBox.Show("deviceID or applicationID is Null or Empty.", "Update Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return KeyCheckResult.Invalid;
			}
			string text = ((!(applicationID == "*")) ? applicationID : "+asterisk+");
			Console.WriteLine("applicationID=" + applicationID);
			Console.WriteLine("SEN ID=" + Program.appConfigData.PsnID);
			Console.WriteLine("\nA) Check if Publisher Key exists.");
			if (PublisherKeyUtility.ExistPublisherKey())
			{
				Console.WriteLine(" ->OK.");
				if (!IsRegenerateAppKey)
				{
					Console.WriteLine("\nSkip Check Publisher Key.");
				}
				else
				{
					Console.WriteLine("\nA2) Check if Publisher Key is valid.");
					if (!PublisherKeyUtility.CheckPublisherKey(showMessageBox: false))
					{
						Console.WriteLine(" ->invalid.");
						return KeyCheckResult.Invalid;
					}
					flag = true;
					Console.WriteLine(" ->OK.");
				}
				Console.WriteLine($"\nD) Check if {deviceID}'s Device Seed exist.");
				if (DeviceSeedUtility.ExistDeviceSeed(deviceID))
				{
					Console.WriteLine(" ->OK");
					if (!IsRegenerateAppKey)
					{
						goto IL_0168;
					}
					Console.WriteLine(" ->Regenerate Device Seed.");
				}
				else
				{
					Console.WriteLine(" ->" + deviceID + "'s Device Seed does NOT exist.");
				}
				Console.WriteLine($"\nF) Generate {deviceID}'s Device Seed.");
				if (DeviceSeedUtility.GenerateDeviceSeed(deviceID))
				{
					result = KeyCheckResult.Valid_Update;
					goto IL_0168;
				}
				Console.WriteLine("ERROR: Failed to generate Device Seed.");
				return KeyCheckResult.Invalid;
			}
			MessageBox.Show(Utility.TextLanguage(string.Format("There is no Publisher Key.\n\nGenerate or import Publisher Key by key management panel of {0}.", "Publishing Utility"), string.Format("パブリッシャ鍵がありません。\n\n{0}の鍵管理画面でパブリッシャ鍵を作成、もしくはインポートしてください。", "Publishing Utility")), "Update Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			return KeyCheckResult.Invalid;
			IL_0373:
			Console.WriteLine("Succeeded.");
			return result;
			IL_0168:
			Console.WriteLine($"\nG) Check if App Dev Key {text} exist.");
			if (IsRegenerateAppKey)
			{
				Console.WriteLine(" ->Regenerated App Dev Key " + text + ".");
			}
			else if (AppKeyUtility.ExistAppDevKey(text))
			{
				Console.WriteLine(" ->OK");
				Console.WriteLine($"\nH) Compare the timestamp of the {text}'s App Dev Key and Publisher Key.");
				if (!PublisherKeyUtility.GetPublisherKeyInfoOnLocalPC(out var _, out var dateTime))
				{
					Console.WriteLine("Failed to get Publisher Key information in local PC.");
					MessageBox.Show("Failed to get Publisher Key information in local PC.", "UpdateKey - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return KeyCheckResult.Invalid;
				}
				if (AppKeyUtility.GetTimeStampAppDevKey(text) > dateTime)
				{
					Console.WriteLine(" ->OK");
					goto IL_0283;
				}
				Console.WriteLine(" ->App Dev Key is invalid.");
			}
			else
			{
				Console.WriteLine(" ->" + text + "'s App Dev Key does NOT exist.");
			}
			Console.WriteLine("\nI) Generate App Dev Key.");
			if (!flag)
			{
				Form form = new SignInSENID_Password(" - Generate Publisher Key");
				DialogResult dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					return KeyCheckResult.Invalid;
				}
				flag = true;
			}
			if (AppKeyUtility.GenerateAppDevKey(applicationID, Program.appConfigData.PsnID, Program.appConfigData.Password))
			{
				Console.WriteLine(" ->OK");
				result = KeyCheckResult.Valid_Update;
				goto IL_0283;
			}
			Console.WriteLine("ERROR: Failed to generate App Dev Key.");
			return KeyCheckResult.Invalid;
			IL_0283:
			Console.WriteLine(string.Format("\nJ) Check if {0}'s App Exe Key exist.", text + "/" + deviceID));
			if (AppKeyUtility.ExistAppExeKey(text, deviceID))
			{
				Console.WriteLine(" ->OK");
				Console.WriteLine($"\nK) Check if timestamp of the {applicationID}'s App Exe Key is valid.");
				if (IsValidAppExeKey(text, deviceID))
				{
					goto IL_0373;
				}
			}
			else
			{
				Console.WriteLine(" ->" + applicationID + "/" + deviceID + "'s App Exe Key does NOT exist.");
			}
			Console.WriteLine(string.Format("\nL) Generate {0}'s App Exe Key.", applicationID + "/" + deviceID));
			if (!flag)
			{
				Form form = new SignInSENID_Password();
				DialogResult dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					return KeyCheckResult.Invalid;
				}
				flag = true;
			}
			if (AppKeyUtility.GenerateAppExeKey(deviceID, applicationID, Program.appConfigData.PsnID, Program.appConfigData.Password))
			{
				result = KeyCheckResult.Valid_Update;
				Console.WriteLine(" ->OK");
				goto IL_0373;
			}
			Console.WriteLine("ERROR: Failed to generate App Exe Key.");
			return KeyCheckResult.Invalid;
		}

		public static bool DeleteAllDeviceSeedAndAppKey2()
		{
			bool result = false;
			DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage("Do you want to delete all App Keys and Device Seeds?", "デバイスシードとアプリ鍵を全て削除しますか？"), "Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.Yes)
			{
				result = DeleteAllDeviceSeedAndAppKey();
			}
			return result;
		}

		public static bool DeleteAllDeviceSeedAndAppKey()
		{
			bool result = true;
			try
			{
				if (Directory.Exists(Program.PATH_DIR_DEVICE_SEED))
				{
					Directory.Delete(Program.PATH_DIR_DEVICE_SEED, recursive: true);
				}
				if (Directory.Exists(Program.PATH_DIR_HOST_APPS_KEY))
				{
					Directory.Delete(Program.PATH_DIR_HOST_APPS_KEY, recursive: true);
				}
				if (Directory.Exists(Program.PATH_DIR_TARGET_APPS_KEY))
				{
					string[] directories = Directory.GetDirectories(Program.PATH_DIR_TARGET_APPS_KEY);
					string[] array = directories;
					foreach (string path in array)
					{
						string[] files = Directory.GetFiles(path);
						foreach (string path2 in files)
						{
							File.Delete(path2);
						}
					}
				}
				MessageBox.Show(Utility.TextLanguage("Succeeded to delete all Device Seed and App Key.", "デバイスシードとアプリ鍵を全て削除しました。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Delete All Device Seed And App Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
		}

		public static bool DeleteAppExeKey(string deviceID, string applicationID)
		{
			if (applicationID == "*")
			{
				applicationID = "+asterisk+";
			}
			string text = Program.PATH_DIR_TARGET_APPS_KEY + applicationID + "\\" + deviceID + ".ktapp";
			if (File.Exists(text))
			{
				File.Delete(text);
				Console.WriteLine("Delete " + text);
				return true;
			}
			MessageBox.Show("Cannot find " + text, "Delete App Exe Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}

		public static bool DeleteAppDevKey(string applicationID)
		{
			if (applicationID == "*")
			{
				applicationID = "+asterisk+";
			}
			string text = Program.PATH_DIR_HOST_APPS_KEY + "\\" + applicationID + ".khapp";
			if (File.Exists(text))
			{
				File.Delete(text);
				Console.WriteLine("Delete " + text);
				return true;
			}
			MessageBox.Show("Cannot find " + text, "Delete App Dev Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}

		public static bool IsValidAppExeKey(string applicationID, string deviceID)
		{
			if (applicationID == "*")
			{
				applicationID = "+asterisk+";
			}
			AppExeKey appExeKey = new AppExeKey();
			if (appExeKey.Load(Program.GetAppExeKey(applicationID, deviceID)))
			{
				Console.WriteLine("Expiration={0}", Utility.ConvertLocaleDateTime(appExeKey.GetExpirationString()));
				AppExeKeyStatus validationStatus = appExeKey.GetValidationStatus();
				Console.WriteLine(" ->" + validationStatus);
				if (validationStatus == AppExeKeyStatus.OK)
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
