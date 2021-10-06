using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PublishingUtility.KeyManagement;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class PublisherKeyUtility
	{
		private static PublisherKeyType m_publisherKeyType;

		private static string publisherKeynameOnServer;

		private static DateTime dateTimePublisherKeyOnServer = default(DateTime);

		public static bool GetPubisherKeyInfoOnServer(out string keyname, out DateTime dateTime)
		{
			bool flag = false;
			keyname = "";
			dateTime = default(DateTime);
			Form form = new SignInSENID_Password();
			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				return false;
			}
			string caption = "Get Publisher Key Info";
			ProgressDialog progressDialog = new ProgressDialog(caption, doWorkGetPublisherKeyInfo, 100);
			DialogResult dialogResult2 = progressDialog.ShowDialog();
			if (dialogResult2 == DialogResult.OK)
			{
				keyname = publisherKeynameOnServer;
				dateTime = dateTimePublisherKeyOnServer;
				flag = true;
			}
			else
			{
				flag = false;
			}
			progressDialog.Dispose();
			return flag;
		}

		private static void doWorkGetPublisherKeyInfo(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			Console.WriteLine("AssemblyVersionDateTime=" + Program.appConfigData.AssemblyVersionDateTime);
			KPubGenerator._scePsmDrmGenerateKpubInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			IntPtr devPkcs = IntPtr.Zero;
			byte[] array = new byte[65];
			byte[] array2 = new byte[29];
			array[0] = 0;
			array2[0] = 0;
			byte[] array3 = new byte[65];
			byte[] array4 = new byte[29];
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			IntPtr intPtr2 = Marshal.AllocHGlobal(array2.Length);
			backgroundWorker.ReportProgress(50, "Connect to Server...");
			ScePsmDrmStatus scePsmDrmStatus = ScePsmDrmStatus.SCE_OK;
			scePsmDrmStatus = KPubGenerator._scePsmDrmGenerateKpub(Program.appConfigData.PsnID, Program.appConfigData.Password, "TmpKeyName", ScePsmDrmKpubUploadState.NEW_REGIST, out devPkcs, out var _, intPtr, intPtr2);
			if (scePsmDrmStatus == ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_KPUB_ALREADY_REGISTERED)
			{
				backgroundWorker.ReportProgress(60, "Overwrite Check.");
				Marshal.Copy(intPtr, array3, 0, 65);
				Marshal.Copy(intPtr2, array4, 0, 29);
				string text = Utility.ByteArrayToString(array3);
				string s = Utility.ByteArrayToString(array4);
				publisherKeynameOnServer = text;
				dateTimePublisherKeyOnServer = DateTime.Parse(s);
			}
			else
			{
				MessageBox.Show(Utility.TextLanguage("Failed to get Publisher Key information.\n\n", "SCEサーバー上のパブリッシャ鍵の情報取得に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus) + "\n\n" + Utility.GetProxySettingText(), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
			}
			KPubGenerator._scePsmDrmReleaseDevPkcs12(devPkcs);
			KPubGenerator._scePsmDrmGenerateKpubTerm();
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			Marshal.FreeHGlobal(devPkcs);
		}

		public static bool GeneratePublishKey(PublisherKeyType publisherKeyType)
		{
			bool result = false;
			m_publisherKeyType = publisherKeyType;
			Form form = new GenerateDeveloperKey01();
			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				return false;
			}
			form = new SignInSENID_Password(" - Generate Publisher Key");
			dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				return false;
			}
			string caption = "Generate Publisher Key";
			ProgressDialog progressDialog = new ProgressDialog(caption, doWorkGeneratePublisherKey, 100);
			DialogResult dialogResult2 = progressDialog.ShowDialog();
			if (dialogResult2 == DialogResult.OK)
			{
				MessageBox.Show(string.Format(Resources.succeedToGenerate_Text, "Publisher Key"), "Generate Publisher Key - Publishing Utility");
				result = true;
			}
			progressDialog.Dispose();
			return result;
		}

		private static void doWorkGeneratePublisherKey(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
			backgroundWorker.ReportProgress(10, "Initialize...");
			Console.WriteLine("AssemblyVersionDateTime=" + Program.appConfigData.AssemblyVersionDateTime);
			KPubGenerator._scePsmDrmGenerateKpubInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			IntPtr devPkcs;
			IntPtr devPkcs2 = (devPkcs = IntPtr.Zero);
			byte[] array = new byte[65];
			byte[] array2 = new byte[29];
			array[0] = 0;
			array2[0] = 0;
			byte[] array3 = new byte[65];
			byte[] array4 = new byte[29];
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			IntPtr intPtr2 = Marshal.AllocHGlobal(array2.Length);
			backgroundWorker.ReportProgress(50, "Connect to Server...");
			ScePsmDrmStatus scePsmDrmStatus = ScePsmDrmStatus.SCE_OK;
			scePsmDrmStatus = ((m_publisherKeyType != 0) ? KPubGenerator._scePsmDrmGenerateQaKpub(Program.appConfigData.PsnID, Program.appConfigData.Password, Program.appConfigData.PublisherKeyNameTmp, ScePsmDrmKpubUploadState.NEW_REGIST, out devPkcs2, out var devPkcs12Size, intPtr, intPtr2) : KPubGenerator._scePsmDrmGenerateKpub(Program.appConfigData.PsnID, Program.appConfigData.Password, Program.appConfigData.PublisherKeyNameTmp, ScePsmDrmKpubUploadState.NEW_REGIST, out devPkcs2, out devPkcs12Size, intPtr, intPtr2));
			if (scePsmDrmStatus == ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_KPUB_ALREADY_REGISTERED)
			{
				backgroundWorker.ReportProgress(60, "Overwrite Check.");
				Marshal.Copy(intPtr, array3, 0, 65);
				Marshal.Copy(intPtr2, array4, 0, 29);
				string arg = Utility.ByteArrayToString(array3);
				string strDateTime = Utility.ByteArrayToString(array4);
				string existPublisherKey_Text = Resources.existPublisherKey_Text;
				string text = ((!(Program.appConfigData.PcLocale == "ja-JP")) ? $"\n\n Key name : {arg}\n date : {Utility.ConvertLocaleDateTime(strDateTime)}\n\n" : $"\n\n 鍵名 : {arg}\n 作成日時 : {Utility.ConvertLocaleDateTime(strDateTime)}\n\n");
				string overwritePublisherKey_Text = Resources.overwritePublisherKey_Text;
				DialogResult dialogResult = MessageBox.Show(existPublisherKey_Text + text + overwritePublisherKey_Text, "Overwrite Check - Publishing Utility", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
				if (dialogResult != DialogResult.OK)
				{
					e.Cancel = true;
					KPubGenerator._scePsmDrmReleaseDevPkcs12(devPkcs2);
					KPubGenerator._scePsmDrmGenerateKpubTerm();
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
					Marshal.FreeHGlobal(devPkcs2);
					return;
				}
				backgroundWorker.ReportProgress(70, "Overwrite Publisher Key...");
				scePsmDrmStatus = ((m_publisherKeyType != 0) ? KPubGenerator._scePsmDrmGenerateQaKpub(Program.appConfigData.PsnID, Program.appConfigData.Password, Program.appConfigData.PublisherKeyNameTmp, ScePsmDrmKpubUploadState.OVERWRITE, out devPkcs, out devPkcs12Size, intPtr, intPtr2) : KPubGenerator._scePsmDrmGenerateKpub(Program.appConfigData.PsnID, Program.appConfigData.Password, Program.appConfigData.PublisherKeyNameTmp, ScePsmDrmKpubUploadState.OVERWRITE, out devPkcs, out devPkcs12Size, intPtr, intPtr2));
			}
			if (scePsmDrmStatus == ScePsmDrmStatus.SCE_OK)
			{
				backgroundWorker.ReportProgress(90, "Saving Publisher Key...");
				Console.WriteLine("Succeeded to generate kpub.");
				Marshal.Copy(intPtr, array3, 0, 65);
				Marshal.Copy(intPtr2, array4, 0, 29);
				string arg = Utility.ByteArrayToString(array3);
				string strDateTime = Utility.ByteArrayToString(array4);
				Program.appConfigData.PublisherKeyName = arg;
				Program.appConfigData.CreateTimeOfPublisherKey = Utility.ConvertLocaleDateTime(strDateTime);
				Program.appConfigData.dtCreateTimeOfPublisherKey = DateTime.Parse(strDateTime);
				string path = Utility.UserAppDataPath + "\\PublisherKey\\";
				byte[] array5 = new byte[devPkcs12Size];
				Marshal.Copy(devPkcs, array5, 0, devPkcs12Size);
				if (m_publisherKeyType == PublisherKeyType.Developer)
				{
					Utility.SaveFileByteArray(path, "kdev.p12", array5);
				}
				else
				{
					Utility.SaveFileByteArray(path, "kdevQa.p12", array5);
				}
				Program.appConfigData.PublisherKeyName = Program.appConfigData.PublisherKeyNameTmp;
				Program.appConfigData.Save();
				KPubGenerator._scePsmDrmReleaseDevPkcs12(devPkcs);
			}
			else
			{
				MessageBox.Show(Utility.TextLanguage("Failed to generate Publisher key.\n\n", "パブリッシャ鍵の生成に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Generate Publisher Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				e.Cancel = true;
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			KPubGenerator._scePsmDrmGenerateKpubTerm();
		}

		public static bool LoadPublisherKeyData(out byte[] byteArray)
		{
			return LoadPublisherKeyData(Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12", out byteArray);
		}

		public static bool LoadPublisherKeyQaData(out byte[] byteArray)
		{
			return LoadPublisherKeyData(Utility.UserAppDataPath + "\\PublisherKey\\kdevQa.p12", out byteArray);
		}

		public static bool LoadPublisherKeyData(string pathFile, out byte[] byteArray)
		{
			byteArray = null;
			try
			{
				FileStream fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
				byteArray = new byte[fileStream.Length];
				fileStream.Read(byteArray, 0, byteArray.Length);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Load Publisher Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}

		public static bool GetPublisherKeyInfo(string pathFile, out string strCertCommonName, out string strCertCreateTime)
		{
			strCertCommonName = "-";
			strCertCreateTime = "-";
			ScePsmDrmStatus scePsmDrmStatus = KPubGenerator._scePsmDrmGenerateKpubInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			if (scePsmDrmStatus != 0)
			{
				MessageBox.Show(Utility.TextLanguage("Failed to get Publisher Key information.\n\n", "パブリッシャ鍵の情報取得に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Get Publisher Key Info - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			byte[] array = new byte[65];
			byte[] array2 = new byte[32];
			array[0] = 0;
			array2[0] = 0;
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			IntPtr intPtr2 = Marshal.AllocHGlobal(array2.Length);
			if (!LoadPublisherKeyData(pathFile, out var byteArray))
			{
				MessageBox.Show(Utility.TextLanguage("Failed to open Publisher Key.", "パブリッシャ鍵のオープンに失敗しました。"), "Get Publisher Key Info - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			IntPtr intPtr3 = Marshal.AllocHGlobal(byteArray.Length);
			Marshal.Copy(byteArray, 0, intPtr3, byteArray.Length);
			scePsmDrmStatus = KPubGenerator._scePsmDrmGetCertInfo(intPtr3, byteArray.Length, intPtr, intPtr2);
			bool flag = false;
			if (scePsmDrmStatus != 0)
			{
				DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage("Failed to get Publisher Key information.\n\n", "パブリッシャ鍵の情報取得に失敗しました。\n\n") + pathFile + Utility.TextLanguage(" is invalid.\n\n", "は不正なファイルです。\n\n") + Utility.TextLanguage($"Do you want to delete {pathFile}?", $"ファイル{pathFile}を削除しますか？"), "Read Publisher Key - Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
				if (dialogResult == DialogResult.Yes)
				{
					string path = Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12";
					File.Delete(path);
				}
				flag = false;
			}
			else
			{
				Marshal.Copy(intPtr, array, 0, 65);
				Marshal.Copy(intPtr2, array2, 0, 32);
				strCertCommonName = Utility.ByteArrayToString(array);
				strCertCreateTime = Utility.ByteArrayToString(array2);
				flag = true;
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			KPubGenerator._scePsmDrmGenerateKpubTerm();
			return flag;
		}

		public static bool GetPublisherKeyInfoOnLocalPC(out string keyname, out DateTime dateTime)
		{
			dateTime = default(DateTime);
			keyname = "";
			if (GetPublisherKeyInfo(Program.PATH_FILE_PUBLISHER_KEY, out var strCertCommonName, out var strCertCreateTime))
			{
				keyname = strCertCommonName;
				dateTime = DateTime.Parse(strCertCreateTime);
				return true;
			}
			return false;
		}

		public static bool GetAccountId(out long accountID)
		{
			accountID = 0L;
			bool flag = false;
			ScePsmDrmStatus scePsmDrmStatus = KPubGenerator._scePsmDrmGenerateKpubInit(Program.appConfigData.AssemblyVersionDateTime, Program.appConfigData.EnvServer, string.IsNullOrEmpty(Program.appConfigData.ProxyAddress) ? null : Program.appConfigData.ProxyAddress, Program.appConfigData.Port);
			if (scePsmDrmStatus != 0)
			{
				MessageBox.Show(Utility.TextLanguage($"Failed to get AccountID.\n\n", $"AccountIDの取得に失敗しました。\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Get AccountIdPublishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			byte[] array = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			if (!LoadPublisherKeyData(out var byteArray))
			{
				MessageBox.Show(Utility.TextLanguage("Failed to open Publisher Key.", "パブリッシャ鍵のオープンに失敗しました。"), "Get AccountIdPublishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			else
			{
				IntPtr intPtr2 = Marshal.AllocHGlobal(byteArray.Length);
				Marshal.Copy(byteArray, 0, intPtr2, byteArray.Length);
				scePsmDrmStatus = KPubGenerator._scePsmDrmGetAccountId(intPtr2, byteArray.Length, intPtr);
				if (scePsmDrmStatus != 0)
				{
					MessageBox.Show(Utility.TextLanguage($"Failed to get AccountID.\n\n", $"AccountIDの取得に失敗しました。.\n\n") + DrmError.GetErrorMessage(scePsmDrmStatus), "Get Account Id - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					flag = false;
				}
				else
				{
					Marshal.Copy(intPtr, array, 0, 8);
					accountID = BitConverter.ToInt64(array, 0);
					Console.WriteLine("accountID=" + accountID);
					flag = true;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			KPubGenerator._scePsmDrmGenerateKpubTerm();
			return flag;
		}

		public static bool ExistPublisherKey()
		{
			return File.Exists(Program.PATH_FILE_PUBLISHER_KEY);
		}

		public static bool CheckPublisherKey(bool showMessageBox = true)
		{
			if (!ExistPublisherKey())
			{
				MessageBox.Show(Utility.TextLanguage("There is no Publisher Key. \nGenerate or import the Publisher Key.", "パブリッシャ鍵がありません。\nパブリッシャ鍵を作成もしくはインポートしてください。"), "Check Publisher Key - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			if (GetPubisherKeyInfoOnServer(out var keyname, out var dateTime))
			{
				if (GetPublisherKeyInfoOnLocalPC(out var keyname2, out var dateTime2))
				{
					string text = Utility.TextLanguage($"Publisher Key on local PC  \n  keyname: {keyname2}\n  date: {Utility.ConvertLocaleDateTime(dateTime2)}\n\n" + $"Publisher Key on SCE server\n  keyname: {keyname}\n  date: {Utility.ConvertLocaleDateTime(dateTime)}\n", $"ローカルPCのパブリッシャ鍵\n  鍵名: {keyname2}\n  作成日時: {Utility.ConvertLocaleDateTime(dateTime2)}\n\n" + $"サーバー上のパブリッシャ鍵\n  鍵名: {keyname}\n  作成日時: {Utility.ConvertLocaleDateTime(dateTime)}\n");
					if (keyname2 == keyname && dateTime2 == dateTime)
					{
						if (showMessageBox)
						{
							MessageBox.Show(Utility.TextLanguage("\nThis Publisher Key is valid.\n\n", "\nこのパブリッシャ鍵は有効です。\n\n") + text, Utility.TextLanguage("Check Publisher Key - ", "パブリッシャ鍵のチェック - ") + "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
						return true;
					}
					MessageBox.Show(Utility.TextLanguage($"\nThis Publisher Key '{keyname2}' is invalid.\nRecreate the Publisher Key or import the Publisher Key '{keyname}'.\n\n", $"\nこのパブリッシャ鍵 '{keyname2}' は無効です。\nパブリッシャ鍵を再作成する、もしくはパブリッシャ鍵 '{keyname}' をインポートしてください。\n\n") + text, Utility.TextLanguage("Check Publisher Key - ", "パブリッシャ鍵のチェック - ") + "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				MessageBox.Show(Utility.TextLanguage("Failed to obtain information on the Publisher Key placed on the local PC.", "ローカルPCに配置してあるパブリッシャ鍵の情報取得に失敗しました。"), Utility.TextLanguage("Check Publisher Key - ", "パブリッシャ鍵のチェック - ") + "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return false;
		}

		public static bool ExportPublisherKey()
		{
			if (!ExistPublisherKey())
			{
				MessageBox.Show("Can't find Publisher key", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			string sourceFileName = Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12";
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Export Publisher Key";
			saveFileDialog.Filter = "Publisher key files|*.p12|All files|*.*";
			saveFileDialog.FileName = "kdev.p12";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.OverwritePrompt = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				File.Copy(sourceFileName, saveFileDialog.FileName, overwrite: true);
				return true;
			}
			return false;
		}

		public static bool ImportPublisherKey()
		{
			bool result = false;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Import Publisher Key";
			openFileDialog.Filter = "Publisher key files|*.p12|All files|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "p12";
			openFileDialog.AddExtension = true;
			if (!string.IsNullOrEmpty(Program.appConfigData.tmpFolderPublisherKey) && Directory.Exists(Program.appConfigData.tmpFolderPublisherKey))
			{
				openFileDialog.InitialDirectory = Program.appConfigData.tmpFolderPublisherKey;
			}
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string text = Utility.UserAppDataPath + "\\PublisherKey\\";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string destFileName = text + "kdev.p12";
				File.Copy(openFileDialog.FileName, destFileName, overwrite: true);
				if (GetPublisherKeyInfo(Program.PATH_FILE_PUBLISHER_KEY, out var strCertCommonName, out var strCertCreateTime))
				{
					Program.appConfigData.PublisherKeyName = strCertCommonName;
					Program.appConfigData.CreateTimeOfPublisherKey = Utility.ConvertLocaleDateTime(strCertCreateTime);
					Program.appConfigData.dtCreateTimeOfPublisherKey = DateTime.Parse(strCertCreateTime);
					MessageBox.Show(Utility.TextLanguage("Publisher Key is imported .", "パブリッシャ鍵をインポートしました。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
					Program.appConfigData.tmpFolderPublisherKey = Path.GetDirectoryName(openFileDialog.FileName);
					result = true;
				}
				else
				{
					MessageBox.Show("Failed to get Publisher Key Info.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Program.appConfigData.PublisherKeyName = "";
					Program.appConfigData.CreateTimeOfPublisherKey = "";
				}
			}
			return result;
		}

		public static bool ImportPublisherKeyWithCheck()
		{
			bool flag = true;
			if (flag = ImportPublisherKey())
			{
				DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage("Do you want to confirm if this Publisher Key is valid or not?", "このパブリッシャ鍵が有効か確認しますか？"), "Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
				if (dialogResult == DialogResult.Yes)
				{
					flag = CheckPublisherKey();
				}
			}
			return flag;
		}
	}
}
