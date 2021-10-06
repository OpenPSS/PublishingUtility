using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;
using PublishingUtility.KeyManagement;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal static class Command
	{
		private const string caption = "Compile app.xml";

		private const string RESERVED_DEV_PARAM_STRING = "__DEVPSM__";

		public static int CheckHasPublisherKey()
		{
			if (!PublisherKeyUtility.ExistPublisherKey())
			{
				MessageBox.Show(Resources.noPublisherKeyBody_Text, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			return 0;
		}

		public static int CheckMaxFileSize(string path)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(path);
			long directorySize = Utility.GetDirectorySize(dirInfo);
			if (directorySize > 1073741824)
			{
				MessageBox.Show(string.Format(Resources.fileSizeTooBigBody_Text, directorySize / 1048576, path.Replace("\\\\", "\\")), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			return 0;
		}

		public static int CreateIntermediateDir(string srcDir, string intermediateDir)
		{
			if (Directory.Exists(intermediateDir))
			{
				Directory.Delete(intermediateDir, recursive: true);
			}
			FileSystem.CopyDirectory(srcDir, intermediateDir);
			Directory.CreateDirectory(Path.Combine(intermediateDir, "Metadata"));
			return 0;
		}

		public static int MetaValidationCheck(string file)
		{
			XDocument xDocument = Xml.LoadFromFile(file);
			Metadata metadata = new Metadata();
			metadata.SelfFilePath = file;
			metadata.LateValidation = true;
			Metadata metadata2 = metadata;
			XElement root = xDocument.Element("application");
			Xml.ReadFromXml(metadata2, root);
			if (!Metadata.IsValidMetadataForPublish(metadata2, out var textCheckResult))
			{
				MessageBox.Show(textCheckResult, "Publishing Utility - app.xml check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			xDocument = Xml.LoadFromFile(file, validate: true);
			if (xDocument == null)
			{
				MessageBox.Show(Utility.TextLanguage($"Failed to load {file}.", $"{file}のロードに失敗しました。"), "Xml.LoadFromFile()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			return 0;
		}

		public static bool InsertDeveloperInfoToAppXml(string appXml)
		{
			string developerName = GetDeveloperName();
			if (string.IsNullOrEmpty(developerName))
			{
				MessageBox.Show(Utility.TextLanguage(string.Format("Failed to get developer information from {0} server.", (Program.appConfigData.EnvServer != "np") ? Program.appConfigData.EnvServer.ToString() : "SCE"), string.Format("{0}サーバーからの開発者情報の取得に失敗しました。", (Program.appConfigData.EnvServer != "np") ? Program.appConfigData.EnvServer.ToString() : "SCE")), "Insert Developer Info to AppXml - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			Console.WriteLine("developerName=" + developerName);
			XDocument xDocument = Xml.LoadFromFile(appXml, validate: true);
			if (xDocument == null)
			{
				Environment.Exit(1);
			}
			XElement xElement = xDocument.Element("application").Element("developer").Element("name");
			if (xElement != null)
			{
				xElement.Attribute("value").Value = developerName;
				Xml.SaveIntoFile(appXml, xDocument.Element("application"));
			}
			else
			{
				MessageBox.Show(Utility.TextLanguage("Failed to get <developer> element in app.xml", "app.xmlファイル内の<developer>要素の取得に失敗しました。"), "Publishing Utility - InsertDeveloperInfoToAppXml()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			return true;
		}

		public static int MetaConvertToServerPath(string srcFile, string dstFile, out Metadata meta)
		{
			meta = new Metadata
			{
				SelfFilePath = srcFile
			};
			if (srcFile == null || !File.Exists(srcFile))
			{
				Console.WriteLine(string.Format("Input file \"{0}\" doesn't exist.", srcFile ?? ""));
				MessageBox.Show("ERROR: MetaConvertToServerPath()\n" + string.Format("Input file \"{0}\" doesn't exist.", srcFile ?? "") + ".", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			XDocument xDocument = Xml.LoadFromFile(srcFile);
			XElement root = xDocument.Element("application");
			Xml.ReadFromXml(meta, root);
			bool isFixedPath = true;
			Xml.WriteToXml(meta, out root, isFixedPath);
			Xml.SaveIntoFile(dstFile, root);
			return 0;
		}

		public static int CompileMetaData(string srcFile, string dstFile)
		{
			if (string.IsNullOrEmpty(srcFile) || !File.Exists(srcFile))
			{
				MessageBox.Show(string.Format("Input file \"{0}\" doesn't exist.", srcFile ?? ""), "Compile app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return -1;
			}
			if (string.IsNullOrEmpty(dstFile))
			{
				Console.WriteLine("ERROR: dstFile is not assigned", "Compile app.xml");
				return -1;
			}
			string location = PsmSdk.Location;
			if (string.IsNullOrEmpty(location))
			{
				MessageBox.Show("ERROR: Environment value SCE_PSM_SDK is not defined.", "Compile app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return -1;
			}
			string text = location + "/tools/Python/python.exe";
			if (!File.Exists(text))
			{
				MessageBox.Show("ERROR: Cannot find " + text + ".", "Compile app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return -1;
			}
			string text2 = location + "/tools/PublishingUtility/cxml/appinfo/appinfo_compiler.py";
			if (!File.Exists(text2))
			{
				MessageBox.Show("ERROR: Cannot find " + text2 + ".", "Compile app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return -1;
			}
			string text3 = "\"" + text + "\"";
			string text4 = "\"" + text2 + "\" -o \"" + dstFile + "\" \"" + srcFile + "\"";
			Console.WriteLine("command+arguments=" + text3 + " " + text4);
			string processOutput = "";
			if (ChildProcessOutputRedirection.SortInputListText(text3, text4, ref processOutput) != 0)
			{
				MessageBox.Show(Utility.TextLanguage($"Failed to convert {srcFile} to {dstFile}.\n\nLog:{processOutput}", $"{srcFile} から {dstFile} への変換に失敗しました。.\n\nLog:{processOutput}"), "Compile app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return -1;
			}
			return 0;
		}

		public static int PlaceAssetsToPackageDirectory(string appxml, string dstDir)
		{
			if (!File.Exists(appxml))
			{
				Console.WriteLine(string.Format("Input file \"{0}\" doesn't exist!", appxml ?? ""));
				MessageBox.Show(Utility.TextLanguage(string.Format("Input file \"{0}\" doesn't exist!", appxml ?? "") + ".", string.Format("入力ファイル \"{0}\" が見つかりません。", appxml ?? "") + "."), "Publishing Utility - PackagePlaceAsset()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			try
			{
				Metadata metadata = new Metadata();
				metadata.SelfFilePath = appxml;
				Metadata metadata2 = metadata;
				XDocument xDocument = Xml.LoadFromFile(appxml);
				XElement root = xDocument.Element("application");
				Xml.ReadFromXml(metadata2, root);
				string[] array = new string[4]
				{
					Path.Combine(metadata2.SelfFileDir, metadata2.Icon512x512),
					Path.Combine(metadata2.SelfFileDir, metadata2.Icon256x256),
					Path.Combine(metadata2.SelfFileDir, metadata2.Icon128x128),
					Path.Combine(metadata2.SelfFileDir, metadata2.Splash)
				};
				string[] array2 = new string[4]
				{
					Path.Combine(dstDir, "Metadata", metadata2.FixedIcon512Path),
					Path.Combine(dstDir, "Metadata", metadata2.FixedIcon256Path),
					Path.Combine(dstDir, "Metadata", metadata2.FixedIcon128Path),
					Path.Combine(dstDir, "Metadata", metadata2.FixedSplashPath)
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (!File.Exists(array[i]))
					{
						Console.WriteLine(string.Format("Input file \"{0}\" doesn't exist!", array[i] ?? ""));
						return -2;
					}
					string directoryName = Path.GetDirectoryName(array2[i]);
					if (!Directory.Exists(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					if (!File.Exists(array2[i]) || !(File.GetLastWriteTime(array[i]) == File.GetLastWriteTime(array2[i])))
					{
						File.Copy(array[i], array2[i], overwrite: true);
					}
				}
				string path = Path.Combine(dstDir, "Metadata", metadata2.FixedCopyrightPath);
				string path2 = Path.Combine(metadata2.SelfFileDir, metadata2.Copyright);
				string defaultCopyright = PsmSdk.DefaultCopyright;
				string directoryName2 = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName2))
				{
					Directory.CreateDirectory(directoryName2);
				}
				StreamWriter streamWriter = new StreamWriter(path, append: false);
				StreamReader streamReader = new StreamReader(path2);
				StreamReader streamReader2 = new StreamReader(defaultCopyright);
				string value;
				while ((value = streamReader.ReadLine()) != null)
				{
					streamWriter.WriteLine(value);
				}
				streamWriter.WriteLine("");
				streamWriter.WriteLine("");
				while ((value = streamReader2.ReadLine()) != null)
				{
					streamWriter.WriteLine(value);
				}
				streamWriter.Close();
				streamReader.Close();
				streamReader2.Close();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex.Message);
				MessageBox.Show("Exception: " + ex.Message + ".", "Publishing Utility - PackagePlaceAsset()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			return 0;
		}

		public static int CreateMasterPackage(string appXml, string srcDir, string intermediateDir, string masterPackageFile)
		{
			string tempFileName = Path.GetTempFileName();
			CheckHasPublisherKey();
			CheckMaxFileSize(srcDir);
			CreateIntermediateDir(srcDir, intermediateDir);
			MetaValidationCheck(appXml);
			InsertDeveloperInfoToAppXml(appXml);
			string text = Path.Combine(intermediateDir, "Metadata\\app.xml");
			MetaConvertToServerPath(appXml, text, out var meta);
			PlaceAssetsToPackageDirectory(appXml, intermediateDir);
			if (CompileMetaData(text, intermediateDir + "\\Application\\app.info") != 0)
			{
				Environment.Exit(1);
			}
			Package.CreatePackage(intermediateDir, tempFileName);
			Package.SignPackage(tempFileName, masterPackageFile, meta.ProjectName);
			File.Delete(tempFileName);
			MessageBox.Show(Utility.TextLanguage("Succeeded to create a Master Package.\n\n" + masterPackageFile.Replace("\\\\", "\\"), "マスターパッケージの作成に成功しました。\n\n" + masterPackageFile.Replace("\\\\", "\\")), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			MessageBox.Show(Utility.TextLanguage("When submitting a Master package, SEN account and the Publisher Key must be the same as the one when the Master Package was created.\n\nAnd make sure the Publisher Key is not changed until this PSM App is distributed on PS Store.", "マスターパッケージを提出する時、SENアカウントとパブリッシャ鍵はマスターパッケージ作成時に利用したものと同じものである必要があります。\n\nまたPS StoreでこのPSMアプリの配信が開始されるまで、パブリッシャ鍵を更新しないよう注意してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return 0;
		}

		public static int Submit()
		{
			string text = null;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Resources.openPackageFileDialogTitle_Text;
			openFileDialog.Filter = Resources.openPsmPackageFileDialogFilter_Text;
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "PSMP";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return -1;
			}
			text = openFileDialog.FileName;
			return Submit(text);
		}

		public static int Submit(string masterPackageFile)
		{
			string info = "";
			return Submit(masterPackageFile, getInfoOnly: false, ref info);
		}

		public static string GetDeveloperName()
		{
			return GetDeveloperName("__DEVPSM__");
		}

		public static string GetDeveloperName(string projectName)
		{
			if (projectName == null || projectName == "")
			{
				return GetDeveloperName();
			}
			Form form = new SignInSENID_Password(" - Submittion");
			if (form.ShowDialog() != DialogResult.OK)
			{
				return null;
			}
			string psnID = Program.appConfigData.PsnID;
			string password = Program.appConfigData.Password;
			string deviceID = "";
			string accountBeingAttested_Text = Resources.accountBeingAttested_Text;
			string Result = "";
			SubmitAuth submitAuth = new SubmitAuth();
			if (submitAuth.Exec(psnID, password, deviceID, accountBeingAttested_Text, ref Result) != DialogResult.OK)
			{
				MessageBox.Show(Result, "Get Developer Name - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return null;
			}
			accountBeingAttested_Text = ((!(projectName == "__DEVPSM__")) ? ("Get Metadata...\n\"" + projectName + "\"") : "Get Metadata...\n");
			SubmitGetMeta submitGetMeta = new SubmitGetMeta();
			if (submitGetMeta.ExecFromProjectName(submitAuth.TicketData, projectName, accountBeingAttested_Text, ref Result) != DialogResult.OK)
			{
				MessageBox.Show(Result, "Get Developer Name - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return null;
			}
			return submitGetMeta.DeveloperName;
		}

		private static int Submit(string masterPackageFile, bool getInfoOnly, ref string info)
		{
			if (masterPackageFile == "")
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.FileName = "*.psmp";
				openFileDialog.Filter = "Master package file(*.psmp)|*.psmp|All File(*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.Title = "Select Master Package to Submit.";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					masterPackageFile = openFileDialog.FileName;
				}
			}
			string tempFileName = Path.GetTempFileName();
			if (masterPackageFile != "")
			{
				if (PsmDeviceFuncBinding.PickFileFromPackage(tempFileName, masterPackageFile, "Metadata/app.xml") < 0)
				{
					return -1;
				}
				Submit submit = new Submit();
				if (!File.Exists(masterPackageFile))
				{
					submit.Error("Package file not found.\n(" + masterPackageFile + ")");
				}
				else if (!File.Exists(tempFileName))
				{
					submit.Error("XML file not found.\n(" + tempFileName + ")");
				}
				else
				{
					AppXmlInfo info2 = default(AppXmlInfo);
					string text = File.ReadAllText(tempFileName);
					StringReader input = new StringReader(text);
					XmlReader reader = XmlReader.Create(input);
					Utility.AnalyzeAppXml(reader, ref info2);
					Form form = new SignInSENID_Password();
					DialogResult dialogResult = form.ShowDialog();
					if (dialogResult == DialogResult.OK)
					{
						string Result = "";
						string text2 = "";
						DialogResult dialogResult2 = DialogResult.None;
						SubmitAuth submitAuth = new SubmitAuth();
						int num = 1;
						while (num > 0)
						{
							switch (num)
							{
							case 1:
							{
								string psnID = Program.appConfigData.PsnID;
								string password = Program.appConfigData.Password;
								string deviceID = "";
								text2 = "Account is attested...";
								dialogResult2 = submitAuth.Exec(psnID, password, deviceID, text2, ref Result);
								if (dialogResult2 != DialogResult.OK)
								{
									submit.Error(Result);
									num = -1;
								}
								else
								{
									num++;
								}
								break;
							}
							case 2:
							{
								SubmitGetMeta submitGetMeta = new SubmitGetMeta();
								text2 = "Get Metadata...\n\"" + tempFileName + "\"";
								dialogResult2 = submitGetMeta.ExecFromXml(submitAuth.TicketData, text, info2, text2, ref Result);
								if (dialogResult2 != DialogResult.OK)
								{
									submit.Error(Result);
									num = -1;
									break;
								}
								if (getInfoOnly)
								{
									info = submitGetMeta.DeveloperName;
									num = -1;
									break;
								}
								if (submitGetMeta.UpdateBlocked)
								{
									submit.Error(submitGetMeta.ActionInformationText);
									return -1;
								}
								if (MessageBox.Show(submitGetMeta.ActionInformationText, "Publishing Utility", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
								{
									return -1;
								}
								if (submitGetMeta.ChangedUnmodificationText != null)
								{
									submit.Error(submitGetMeta.ChangedUnmodificationText);
									return -1;
								}
								num++;
								break;
							}
							case 3:
							{
								SubmitCheckMeta submitCheckMeta = new SubmitCheckMeta();
								text2 = "Check Metadata...\n\"" + tempFileName + "\"";
								dialogResult2 = submitCheckMeta.Exec(submitAuth.TicketData, text, info2, text2, ref Result);
								if (dialogResult2 != DialogResult.OK)
								{
									submit.Error(Result);
									num = -1;
								}
								else
								{
									num++;
								}
								break;
							}
							case 4:
							{
								FileInfo fileInfo = new FileInfo(masterPackageFile);
								int num2 = (int)fileInfo.Length;
								string text3 = $"{(float)num2 / 1048576f:f2}" + " MiB";
								SubmitUpload submitUpload = new SubmitUpload();
								text2 = "Uploading...\n\"" + masterPackageFile + "\" (" + text3 + ")";
								switch (submitUpload.Exec(submitAuth.TicketData, text, masterPackageFile, num2, info2, text2, ref Result))
								{
								case DialogResult.Cancel:
									num = -2;
									break;
								default:
									submit.Error(Result);
									num = -1;
									break;
								case DialogResult.OK:
									submit.Information("SubmitUpload was completed.\n\"" + masterPackageFile + "\" (" + text3 + ")");
									if (info2.psnService.scoreBoards)
									{
										MessageBox.Show(Utility.TextLanguage("This PSM App has scoreboard settings. \n\nDon't forget the settings that make the scoreboard public in PSM DevPortal.", "このPSMアプリではスコアボードの設定がなされています。\n\nPSM DevPortalでスコアボードを公開する設定を忘れずにおこなってください。"), Utility.TextLanguage("Publishing Utility - Submit Master Package", "Publishing Utility - マスターパッケージの提出"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
									}
									num++;
									break;
								}
								break;
							}
							default:
								num = 0;
								break;
							}
						}
					}
				}
			}
			File.Delete(tempFileName);
			return 0;
		}
	}
}
