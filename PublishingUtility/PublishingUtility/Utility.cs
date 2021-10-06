using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public static class Utility
	{
		internal class Win32
		{
			[DllImport("kernel32.dll")]
			public static extern bool AttachConsole(uint dwProcessId);

			[DllImport("kernel32.dll")]
			public static extern bool FreeConsole();
		}

		public static Dictionary<PixelFormat, string> dicPixelFormat = new Dictionary<PixelFormat, string>
		{
			{
				PixelFormat.Undefined,
				"Undefined"
			},
			{
				PixelFormat.Max,
				"Max"
			},
			{
				PixelFormat.Indexed,
				"Indexed"
			},
			{
				PixelFormat.Gdi,
				"GDI"
			},
			{
				PixelFormat.Format16bppRgb555,
				"Pixel Format=16bit/pixel  RGB=555"
			},
			{
				PixelFormat.Format16bppRgb565,
				"Pixel Format=16bit/pixel  RGB=565"
			},
			{
				PixelFormat.Format24bppRgb,
				"Pixel Format=24bit/pixel  RGB"
			},
			{
				PixelFormat.Format32bppRgb,
				"Pixel Format=32bit/pixel  RGB"
			},
			{
				PixelFormat.Format1bppIndexed,
				"Pixel Format=1bit/pixel  Indexed Color"
			},
			{
				PixelFormat.Format4bppIndexed,
				"Pixel Format=4bit/pixel  Indexed Color"
			},
			{
				PixelFormat.Format8bppIndexed,
				"Pixel Format=8bit/pixel  Indexed Color"
			},
			{
				PixelFormat.Alpha,
				"Alpha"
			},
			{
				PixelFormat.Format16bppArgb1555,
				"Pixel Format=16bit/pixel  ARGB 1555"
			},
			{
				PixelFormat.PAlpha,
				"PAlpha"
			},
			{
				PixelFormat.Format32bppPArgb,
				"Pixel Format=32bit/pixel PARGB"
			},
			{
				PixelFormat.Extended,
				"Extended"
			},
			{
				PixelFormat.Format16bppGrayScale,
				"Pixel Format=16bit/pixel  GrayScale"
			},
			{
				PixelFormat.Format48bppRgb,
				"Pixel Format=48bit/pixel  RGB"
			},
			{
				PixelFormat.Format64bppPArgb,
				"Pixel Format=64bit/pixel  PARGB"
			},
			{
				PixelFormat.Canonical,
				"Canonical"
			},
			{
				PixelFormat.Format32bppArgb,
				"Pixel Format=32bit/pixel  ARGB"
			},
			{
				PixelFormat.Format64bppArgb,
				"Pixel Format=64bit/pixel ARGB"
			}
		};

		private static byte[] entropy = new byte[4] { 113, 163, 17, 5 };

		public static string CommonAppDataPath => GetFileSystemPath(Environment.SpecialFolder.CommonApplicationData);

		public static string UserAppDataPath => GetFileSystemPath(Environment.SpecialFolder.ApplicationData);

		public static string LocalUserAppDataPath => GetFileSystemPath(Environment.SpecialFolder.LocalApplicationData);

		public static RegistryKey CommonAppDataRegistry => GetRegistryPath(Registry.LocalMachine);

		public static RegistryKey UserAppDataRegistry => GetRegistryPath(Registry.CurrentUser);

		public static void AnalyzeAppXml(XmlReader reader, ref AppXmlInfo info)
		{
			if (info.names == null)
			{
				info.names = new Dictionary<string, AppXmlInfo.LocalizedItem>();
			}
			if (info.shortNames == null)
			{
				info.shortNames = new Dictionary<string, AppXmlInfo.LocalizedItem>();
			}
			if (info.products == null)
			{
				info.products = new Dictionary<string, AppXmlInfo.Product>();
			}
			string[] array = new string[6];
			string key = null;
			for (int i = 0; i < 6; i++)
			{
				array[i] = null;
			}
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.LocalName)
					{
					case "application":
						if (reader.Depth == 0 && reader.HasAttributes)
						{
							info.projectName = reader.GetAttribute("project_name");
							info.appVersion = reader.GetAttribute("version");
							info.runtimeVersion = reader.GetAttribute("runtime_version");
							info.sdkVersion = reader.GetAttribute("sdk_version");
						}
						break;
					case "app_xml_format":
						if (reader.Depth == 1 && reader.HasAttributes)
						{
							info.appXmlFormat.version = reader.GetAttribute("version");
							info.appXmlFormat.sdkType = reader.GetAttribute("sdk_type");
						}
						break;
					case "localized_item":
						if (reader.Depth == 2 && array[1] == "name")
						{
							if (reader.HasAttributes)
							{
								AppXmlInfo.LocalizedItem value = default(AppXmlInfo.LocalizedItem);
								value.locale = reader.GetAttribute("locale");
								value.value = reader.GetAttribute("value");
								info.names.Add(value.locale, value);
							}
						}
						else if (reader.Depth == 2 && array[1] == "short_name")
						{
							if (reader.HasAttributes)
							{
								AppXmlInfo.LocalizedItem value2 = default(AppXmlInfo.LocalizedItem);
								value2.locale = reader.GetAttribute("locale");
								value2.value = reader.GetAttribute("value");
								info.shortNames.Add(value2.locale, value2);
							}
						}
						else if (reader.Depth == 5 && array[4] == "name" && reader.HasAttributes)
						{
							AppXmlInfo.LocalizedItem value3 = default(AppXmlInfo.LocalizedItem);
							value3.locale = reader.GetAttribute("locale");
							value3.value = reader.GetAttribute("value");
							info.products[key].names.Add(value3.locale, value3);
						}
						break;
					case "parental_control":
						if (reader.Depth == 1 && reader.HasAttributes)
						{
							info.lockLevel = reader.GetAttribute("lock_level");
						}
						break;
					case "rating_list":
						if (reader.Depth == 1 && reader.HasAttributes)
						{
							info.ratingList.highestAgeLimit = reader.GetAttribute("highest_age_limit");
							info.ratingList.hasOnlineFeatures = reader.GetAttribute("has_online_features");
						}
						break;
					case "online_features":
						if (reader.Depth == 2 && array[1] == "rating_list" && reader.HasAttributes)
						{
							info.ratingList.onlineFeatures.chat = reader.GetAttribute("chat");
							info.ratingList.onlineFeatures.personalInfo = reader.GetAttribute("personal_info");
							info.ratingList.onlineFeatures.userLocation = reader.GetAttribute("user_location");
							info.ratingList.onlineFeatures.exchangeContent = reader.GetAttribute("exchange_content");
							info.ratingList.onlineFeatures.mininumAge = reader.GetAttribute("minimum_age");
						}
						break;
					case "rating":
						if (reader.Depth == 2 && array[1] == "rating_list" && reader.HasAttributes)
						{
							string attribute = reader.GetAttribute("type");
							switch (attribute)
							{
							case "ESRB":
								info.esrb.type = attribute;
								info.esrb.value = reader.GetAttribute("value");
								info.esrb.age = reader.GetAttribute("age");
								info.esrb.code = reader.GetAttribute("code");
								break;
							case "PEGIEX":
								info.pegiex.type = attribute;
								info.pegiex.value = reader.GetAttribute("value");
								info.pegiex.age = reader.GetAttribute("age");
								info.pegiex.code = reader.GetAttribute("code");
								break;
							case "SELF":
								info.self.type = attribute;
								info.self.value = reader.GetAttribute("value");
								info.self.age = reader.GetAttribute("age");
								info.self.code = reader.GetAttribute("code");
								break;
							}
						}
						break;
					case "genre":
						if (reader.Depth == 2 && array[1] == "genre_list" && reader.HasAttributes && info.primaryGenre == null)
						{
							info.primaryGenre = reader.GetAttribute("value");
						}
						break;
					case "product":
						if (reader.Depth == 3 && array[2] == "product_list" && reader.HasAttributes)
						{
							AppXmlInfo.Product value4 = default(AppXmlInfo.Product);
							value4.names = new Dictionary<string, AppXmlInfo.LocalizedItem>();
							value4.label = reader.GetAttribute("label");
							value4.type = reader.GetAttribute("type");
							info.products.Add(value4.label, value4);
							key = value4.label;
						}
						break;
					case "name":
						if (reader.Depth == 2 && array[1] == "developer" && reader.HasAttributes)
						{
							info.developerName = reader.GetAttribute("value");
						}
						break;
					case "psn_service":
						if (reader.Depth == 2 && array[1] == "psn_service_list" && reader.HasAttributes && reader.GetAttribute("value") == "Scoreboards")
						{
							info.psnService.scoreBoards = true;
						}
						break;
					}
				}
				if (0 <= reader.Depth && reader.Depth <= 5)
				{
					array[reader.Depth] = reader.LocalName;
				}
			}
		}

		public static void LinkToSDKDocument()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("SCE_PSM_DATA");
			if (string.IsNullOrEmpty(environmentVariable))
			{
				MessageBox.Show((Program.appConfigData.PcLocale == "ja-JP") ? "環境変数 SCE_PSM_DATA が見つかりませんでした。" : "Cannot find Environment Variable SCE_PSM_DATA", "Document");
				return;
			}
			string text = environmentVariable + "/doc/";
			text = ((!(Program.appConfigData.PcLocale == "ja-JP")) ? (text + "en/publishing_utility_en.html") : (text + "ja/publishing_utility_ja.html"));
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show((Program.appConfigData.PcLocale == "ja-JP") ? (text + " が見つかりませんでした。") : ("Can not find " + text), "Document");
			}
		}

		public static string ReadRuntimeVersionFile()
		{
			string result = "N/A";
			try
			{
				result = File.ReadAllText(PsmSdk.RuntimeVer);
				return result;
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show(string.Format(Resources.fileNotFoundMsgBoxBody_Text, "$SCE_PSM_SDK\\tools\\runtime.ver"), Resources.fileNotFoundMsgBoxTitle_Text);
				return result;
			}
			catch (IOException)
			{
				MessageBox.Show(string.Format("IOException: " + Resources.fileNotFoundMsgBoxBody_Text, "$SCE_PSM_SDK\\tools\\runtime.ver"), Resources.fileNotFoundMsgBoxTitle_Text);
				return result;
			}
		}

		public static string ReadSdkVersion()
		{
			string text = "0.99.00";
			try
			{
				text = File.ReadAllText(PsmSdk.SdkVersionFile);
				if (!Metadata.IsSdkVersionValid(text))
				{
					text = "";
					return text;
				}
				return text;
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine(string.Format(Resources.fileNotFoundMsgBoxBody_Text, PsmSdk.SdkVersionFile), Resources.fileNotFoundMsgBoxTitle_Text);
				return text;
			}
			catch (IOException)
			{
				MessageBox.Show(string.Format("IOException: " + Resources.fileNotFoundMsgBoxBody_Text, PsmSdk.SdkVersionFile), Resources.fileNotFoundMsgBoxTitle_Text);
				return text;
			}
		}

		public static long GetDirectorySize(DirectoryInfo dirInfo)
		{
			long num = 0L;
			FileInfo[] files = dirInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				num += fileInfo.Length;
			}
			DirectoryInfo[] directories = dirInfo.GetDirectories();
			foreach (DirectoryInfo dirInfo2 in directories)
			{
				num += GetDirectorySize(dirInfo2);
			}
			return num;
		}

		public static string MakeRelativePath(string path1, string path2)
		{
			if (string.IsNullOrEmpty(path1) && Path.IsPathRooted(path2))
			{
				return path2;
			}
			if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
			{
				return "";
			}
			Uri uri = new Uri(path1 + "\\");
			Uri uri2 = new Uri(path2);
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string text = uri3.ToString().Replace("%20", " ");
			return text.Replace("/", "\\");
		}

		public static string MakeFullPath(string path1, string path2)
		{
			if (Path.IsPathRooted(path2))
			{
				return path2;
			}
			if (string.IsNullOrWhiteSpace(path2))
			{
				return "";
			}
			return Path.Combine(path1, path2);
		}

		public static void AttachConsole()
		{
			if (Win32.AttachConsole(uint.MaxValue))
			{
				StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput());
				streamWriter.AutoFlush = true;
				Console.SetOut(streamWriter);
			}
		}

		public static void DetachConsole()
		{
			Win32.FreeConsole();
		}

		public static Bitmap ExtractVistaIcon(Icon icoIcon)
		{
			Bitmap result = null;
			try
			{
				byte[] array = null;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					icoIcon.Save(memoryStream);
					array = memoryStream.ToArray();
				}
				int num = BitConverter.ToInt16(array, 4);
				for (int i = 0; i < num; i++)
				{
					int num2 = array[6 + 16 * i];
					int num3 = array[6 + 16 * i + 1];
					int num4 = BitConverter.ToInt16(array, 6 + 16 * i + 6);
					if (num2 == 0 && num3 == 0 && num4 == 32)
					{
						int count = BitConverter.ToInt32(array, 6 + 16 * i + 8);
						int index = BitConverter.ToInt32(array, 6 + 16 * i + 12);
						MemoryStream memoryStream2 = new MemoryStream();
						BinaryWriter binaryWriter = new BinaryWriter(memoryStream2);
						binaryWriter.Write(array, index, count);
						memoryStream2.Seek(0L, SeekOrigin.Begin);
						return new Bitmap(memoryStream2);
					}
				}
				return result;
			}
			catch
			{
				return null;
			}
		}

		public static bool IsExpectFormatPng(string file, Size size, PixelFormat format)
		{
			return IsExpectFormatPng(file, size, format, alphaCheck: false);
		}

		public static bool IsExpectFormatPng(string imageFile, Size validSize, PixelFormat validPixelFormat, bool alphaCheck)
		{
			string text = "";
			string text2 = "";
			Image image = null;
			bool result = true;
			text = Resources.invalidPngMsgBoxTitle_Text;
			try
			{
				image = Image.FromFile(imageFile);
				if (Path.GetExtension(imageFile) != ".png" || !image.RawFormat.Equals(ImageFormat.Png))
				{
					text2 = Resources.invalidPngMsgBoxBody2_Text + "\n" + imageFile;
					throw new Exception();
				}
				if (image.Size != validSize || image.PixelFormat != validPixelFormat)
				{
					text2 = string.Format(Resources.invalidPngMsgBoxBody1_Text, image.Width, image.Height, dicPixelFormat[image.PixelFormat]) + "\n\n" + string.Format(Resources.correctForamt_Text, validSize.Width, validSize.Height, dicPixelFormat[validPixelFormat]) + TextLanguage(", PNG format", ", PNG形式") + "\n\n" + imageFile;
					throw new Exception();
				}
				if (alphaCheck)
				{
					if (!CheckImageAlpha(imageFile))
					{
						text2 = Resources.notUseApha_Text + "\n\n" + imageFile;
						throw new Exception();
					}
					return result;
				}
				return result;
			}
			catch (OutOfMemoryException)
			{
				text2 = Resources.invalidPngMsgBoxBody2_Text + "\n" + imageFile;
				MessageBox.Show(text2, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			catch
			{
				MessageBox.Show(text2, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			finally
			{
				image?.Dispose();
			}
		}

		private static string GetFileSystemPath(Environment.SpecialFolder folder)
		{
			string text = $"{Environment.GetFolderPath(folder)}\\{Application.CompanyName}\\{Application.ProductName}";
			lock (typeof(Application))
			{
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
					return text;
				}
				return text;
			}
		}

		private static RegistryKey GetRegistryPath(RegistryKey key)
		{
			string arg = ((key != Registry.LocalMachine) ? "Software" : "SOFTWARE");
			string subkey = $"{arg}\\{Application.CompanyName}\\{Application.ProductName}";
			return key.CreateSubKey(subkey);
		}

		public static string ByteArrayToString(byte[] byteArray)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			for (int i = 0; i < byteArray.Length && byteArray[i] != 0; i++)
			{
				stringBuilder.Append((char)byteArray[i]);
			}
			return stringBuilder.ToString();
		}

		public static bool SaveFileByteArray(string path, string file, byte[] byteArray)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (!path.EndsWith("\\") && !path.EndsWith("/"))
			{
				path += "\\";
			}
			try
			{
				FileStream fileStream = new FileStream(path + file, FileMode.Create, FileAccess.Write);
				fileStream.Write(byteArray, 0, byteArray.Length);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + " in SaveFileByteArray()", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}

		public static byte[] LoadFileAsByteArray(string pathFile)
		{
			byte[] array = null;
			try
			{
				FileStream fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
				return array;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return null;
			}
		}

		public static string SafeConvertCharArrayToString(char[] charArray)
		{
			int length = charArray.Length;
			for (int i = 0; i < charArray.Length; i++)
			{
				if (charArray[i] == '\0')
				{
					length = i;
					break;
				}
			}
			return new string(charArray, 0, length);
		}

		public static string GetAssemblyVersion(string pathFile)
		{
			if (!File.Exists(pathFile))
			{
				MessageBox.Show("Can't find " + pathFile, "GetAssemblyVersion()");
				return "";
			}
			Assembly assembly = Assembly.LoadFrom(pathFile);
			AssemblyName name = assembly.GetName();
			return name.Version.ToString();
		}

		public static bool GetDefaultProxy(out string proxyAddress, out int port)
		{
			string uriString = "http://www.foo_test/";
			Uri uri = new Uri(uriString);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			Uri proxy = httpWebRequest.Proxy.GetProxy(uri);
			if (proxy == uri)
			{
				proxyAddress = "";
			}
			else
			{
				proxyAddress = proxy.Host;
			}
			port = proxy.Port;
			Console.WriteLine("proxyAddress=" + proxyAddress);
			Console.WriteLine("port=" + port);
			return true;
		}

		public static bool CheckImageAlpha(string pathFile)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = new Bitmap(pathFile);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Publishing Utility");
				return false;
			}
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			{
				MessageBox.Show($"Image file is invalid format.\nUse {PixelFormat.Format32bppArgb.ToString()} format.", "Publishing Utility");
				return false;
			}
			int width = bitmap.Width;
			int height = bitmap.Height;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = bitmap.GetPixel(j, i).ToArgb();
					if ((num & 0xFF000000u) != 4278190080u)
					{
						bitmap.Dispose();
						return false;
					}
				}
			}
			bitmap.Dispose();
			return true;
		}

		public static void ScaleImageFile(string pathFileInput, string pathFileOutput, int size)
		{
			Bitmap bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
			Graphics graphics = Graphics.FromImage(bitmap);
			Bitmap bitmap2 = new Bitmap(pathFileInput);
			Rectangle rect = new Rectangle(0, 0, size, size);
			graphics.DrawImage(bitmap2, rect);
			bitmap.Save(pathFileOutput, ImageFormat.Png);
			bitmap.Dispose();
			bitmap2.Dispose();
		}

		public static string ConvertLocaleDateTime(string strDateTime)
		{
			DateTime dateTime = DateTime.Parse(strDateTime);
			if (Program.appConfigData.PcLocale == "ja-JP")
			{
				return dateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
			}
			return dateTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
		}

		public static string ConvertLocaleDateTime(DateTime dateTime)
		{
			if (Program.appConfigData.PcLocale == "ja-JP")
			{
				return dateTime.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
			}
			return dateTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
		}

		public static bool IsSafeCharactor(string text)
		{
			Regex regex = new Regex("^[a-zA-Z0-9_-]+$");
			return regex.IsMatch(text);
		}

		public static string GetDeveloperName()
		{
			return Command.GetDeveloperName();
		}

		public static string GetDeveloperName(string projectName)
		{
			return Command.GetDeveloperName(projectName);
		}

		public static bool CheckFileMagicNumber(string pathFile, int magicNumber)
		{
			FileStream fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[4];
			int num = fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			if (num != 4)
			{
				return false;
			}
			if (BitConverter.ToInt32(array, 0) != magicNumber)
			{
				return false;
			}
			return true;
		}

		public static bool GetAccountIDFromPSMP(string pathFile, out long accountID)
		{
			accountID = 0L;
			FileStream fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[24];
			int num = fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			if (num != 24)
			{
				MessageBox.Show(pathFile + " is invalid PSMP file.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			int num2 = BitConverter.ToInt32(array, 0);
			if (num2 != 1095586640)
			{
				MessageBox.Show(pathFile + " is not PSMP file.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			BitConverter.ToInt32(array, 4);
			long num3 = BitConverter.ToInt64(array, 8);
			Console.WriteLine("size=" + num3);
			accountID = BitConverter.ToInt64(array, 16);
			accountID = ReverseEndian(accountID);
			return true;
		}

		public static long ReverseEndian(long val)
		{
			byte[] bytes = BitConverter.GetBytes(val);
			Array.Reverse(bytes);
			return BitConverter.ToInt64(bytes, 0);
		}

		public static string TextLanguage(string textEnglish, string textJapanese)
		{
			if (Program.appConfigData == null)
			{
				return textEnglish;
			}
			if (!(Program.appConfigData.PcLocale == "ja-JP"))
			{
				return textEnglish;
			}
			return textJapanese;
		}

		public static string ProtectText(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			byte[] inArray = ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(inArray);
		}

		public static string UnprotectText(string text)
		{
			try
			{
				byte[] encryptedData = Convert.FromBase64String(text);
				byte[] bytes = ProtectedData.Unprotect(encryptedData, entropy, DataProtectionScope.CurrentUser);
				return Encoding.UTF8.GetString(bytes);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				MessageBox.Show(ex.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return "";
		}

		public static string GetProxySettingText()
		{
			string result = "";
			string proxyAddress = "";
			int port = 0;
			switch (Program.appConfigData.proxySetting)
			{
			case AppConfigData.ProxySetting.NotUseProxy:
				result = "Not use proxy server";
				break;
			case AppConfigData.ProxySetting.SystemDefault:
				GetDefaultProxy(out proxyAddress, out port);
				result = $"Proxy address : {proxyAddress}\nPort number : {port}\n";
				break;
			case AppConfigData.ProxySetting.UseProxy:
				result = $"Proxy address : {Program.appConfigData.ProxyAddress}\nPort number : {Program.appConfigData.Port}\n";
				break;
			}
			return result;
		}

		public static bool GetApplicationID(string appXml, out string applicationID)
		{
			applicationID = "";
			try
			{
				XDocument xDocument = Xml.LoadFromFile(appXml);
				if (xDocument != null)
				{
					XElement xElement = xDocument.Element("application");
					if (xElement != null)
					{
						applicationID = xElement.Attribute("project_name").Value;
						if (!string.IsNullOrEmpty(applicationID))
						{
							if (Metadata.IsProjectNameValid(applicationID))
							{
								return true;
							}
							MessageBox.Show("Application ID is invalid.\n\n" + applicationID, "Get Application ID - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							return false;
						}
						MessageBox.Show("Cannot find 'project_name' tag.", "Get Application ID - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return false;
					}
					MessageBox.Show("Cannot find 'application' tag.", "Get Application ID - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Get Application ID - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return false;
		}
	}
}
