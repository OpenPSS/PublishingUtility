using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public class Metadata : IMetadata
	{
		private int managedHeap;

		private int resourceHeap;

		private string projectName;

		private string appVersion;

		private string runtimeVersion;

		private string sdkVersion;

		private Hashtable appLongNames = new Hashtable();

		private Hashtable appShortNames = new Hashtable();

		private Locale defaultLocale;

		private string icon512x512Full;

		private string icon256x256Full;

		private string icon128x128Full;

		private string splashFull;

		private Genre genre1;

		private Genre genre2;

		private string developerName;

		private string webSite;

		private string copyrightShort;

		private string copyrightFull;

		private ArrayList productList = new ArrayList();

		private static MaxScreenSizeDisplayNameConverter maxScreenSizeConverter = new MaxScreenSizeDisplayNameConverter(typeof(string));

		private static MaxCameraResolutionSizeDisplayNameConverter maxCameraResolutionSizeConverter = new MaxCameraResolutionSizeDisplayNameConverter(typeof(string));

		private static LocaleDisplayNameConverter localeConverter = new LocaleDisplayNameConverter(typeof(string));

		private static GenreDisplayNameConverter genreConverter = new GenreDisplayNameConverter(typeof(string));

		public override int ManagedHeap
		{
			get
			{
				return managedHeap;
			}
			set
			{
				managedHeap = Math.Min(Math.Max(value, 5120), 98304 - ResourceHeap);
			}
		}

		public override int ResourceHeap
		{
			get
			{
				return resourceHeap;
			}
			set
			{
				resourceHeap = Math.Min(Math.Max(value, 1024), 98304 - ManagedHeap);
			}
		}

		public override MaxScreenSize MaxSize { get; set; }

		public override MaxCameraResolutionSize CameraResolutionSize { get; set; }

		public override bool DeviceGamePad { get; set; }

		public override bool DeviceTouch { get; set; }

		public override bool DeviceMotion { get; set; }

		public override bool DeviceLocation { get; set; }

		public override bool DeviceCamera { get; set; }

		public override bool DevicePSVitaTV { get; set; }

		public override string ProjectName
		{
			get
			{
				return projectName;
			}
			set
			{
				if (LateValidation || IsProjectNameInDevelopmentValid(value))
				{
					projectName = value;
				}
			}
		}

		public override string AppVersion
		{
			get
			{
				return appVersion;
			}
			set
			{
				if (LateValidation || IsAppVersionValid(value))
				{
					appVersion = value;
				}
			}
		}

		public override string RuntimeVersion
		{
			get
			{
				return runtimeVersion;
			}
			set
			{
				if (LateValidation || IsRuntimeVersionValid(value))
				{
					runtimeVersion = value;
				}
			}
		}

		public override string SdkVersion
		{
			get
			{
				return sdkVersion;
			}
			set
			{
				if (IsSdkVersionValid(value) || LateValidation)
				{
					sdkVersion = value;
				}
			}
		}

		public override Hashtable AppLongNames
		{
			get
			{
				return appLongNames;
			}
			set
			{
				appLongNames = value;
			}
		}

		public override Hashtable AppShortNames
		{
			get
			{
				return appShortNames;
			}
			set
			{
				appShortNames = value;
			}
		}

		public override Locale DefaultLocale
		{
			get
			{
				return defaultLocale;
			}
			set
			{
				defaultLocale = value;
			}
		}

		public override string Icon512x512
		{
			get
			{
				return Utility.MakeRelativePath(SelfFileDir, icon512x512Full);
			}
			set
			{
				if (LateValidation || IsIcon512x512Valid(value))
				{
					icon512x512Full = Utility.MakeFullPath(SelfFileDir, value);
				}
				else if (string.IsNullOrWhiteSpace(value))
				{
					icon512x512Full = "";
				}
			}
		}

		public override string Icon256x256
		{
			get
			{
				return Utility.MakeRelativePath(SelfFileDir, icon256x256Full);
			}
			set
			{
				if (LateValidation || IsIcon256x256Valid(value))
				{
					icon256x256Full = Utility.MakeFullPath(SelfFileDir, value);
				}
				else if (string.IsNullOrWhiteSpace(value))
				{
					icon256x256Full = "";
				}
			}
		}

		public override string Icon128x128
		{
			get
			{
				return Utility.MakeRelativePath(SelfFileDir, icon128x128Full);
			}
			set
			{
				if (LateValidation || IsIcon128x128Valid(value))
				{
					icon128x128Full = Utility.MakeFullPath(SelfFileDir, value);
				}
				else if (string.IsNullOrWhiteSpace(value))
				{
					icon128x128Full = "";
				}
			}
		}

		public override string Splash
		{
			get
			{
				return Utility.MakeRelativePath(SelfFileDir, splashFull);
			}
			set
			{
				if (LateValidation || IsSplashValid(value))
				{
					splashFull = Utility.MakeFullPath(SelfFileDir, value);
				}
				else if (string.IsNullOrWhiteSpace(value))
				{
					splashFull = "";
				}
			}
		}

		public override Genre Genre1
		{
			get
			{
				return genre1;
			}
			set
			{
				int num = (int)genre2;
				if (((ulong)value & 0xFF000000uL) != (ulong)(num & 0xFF000000u) || value == (Genre)num)
				{
					genre2 = Genre.None;
				}
				genre1 = value;
			}
		}

		public override Genre Genre2
		{
			get
			{
				return genre2;
			}
			set
			{
				int num = (int)genre1;
				if (genre1 == Genre.None)
				{
					genre2 = Genre.None;
				}
				if (num != (int)value && (num & 0xFF000000u) == (long)((ulong)value & 0xFF000000uL))
				{
					genre2 = value;
				}
			}
		}

		public override string DeveloperName
		{
			get
			{
				return developerName;
			}
			set
			{
				if (LateValidation || IsDeveloperNameValid(value))
				{
					developerName = value;
				}
				if (string.IsNullOrWhiteSpace(value))
				{
					developerName = "";
				}
			}
		}

		public override string DeveloperWebSite
		{
			get
			{
				return webSite;
			}
			set
			{
				if (LateValidation || IsDeveloperWebSiteValid(value))
				{
					webSite = value.Trim();
				}
				if (string.IsNullOrWhiteSpace(value))
				{
					webSite = "";
				}
			}
		}

		public override string CopyrightShort
		{
			get
			{
				return copyrightShort;
			}
			set
			{
				if (LateValidation || IsCopyrightShortValid(value))
				{
					copyrightShort = value.Trim();
				}
				if (string.IsNullOrWhiteSpace(value))
				{
					copyrightShort = "";
				}
			}
		}

		public override string Copyright
		{
			get
			{
				return Utility.MakeRelativePath(SelfFileDir, copyrightFull);
			}
			set
			{
				if (LateValidation || IsCopyrightValid(value))
				{
					copyrightFull = Utility.MakeFullPath(SelfFileDir, value);
				}
				else if (string.IsNullOrWhiteSpace(value))
				{
					copyrightFull = "";
				}
			}
		}

		public override bool UseScoreboards { get; set; }

		[Browsable(false)]
		public bool LateValidation { get; set; }

		[Browsable(false)]
		public ArrayList ProductList
		{
			get
			{
				return productList;
			}
			set
			{
				productList = value;
			}
		}

		[Browsable(false)]
		public string SelfFilePath { get; set; }

		[Browsable(false)]
		public string SelfFileDir => Path.GetDirectoryName(SelfFilePath);

		[Browsable(false)]
		public string FixedIcon128Path => "image/icon128.png";

		[Browsable(false)]
		public string FixedIcon256Path => "image/icon256.png";

		[Browsable(false)]
		public string FixedIcon512Path => "image/icon512.png";

		[Browsable(false)]
		public string FixedSplashPath => "image/splash.png";

		[Browsable(false)]
		public string FixedCopyrightPath => "text/copyright.txt";

		[Browsable(false)]
		public string ParentalLockLevel
		{
			get
			{
				int num = 2;
				if (base.HighestAgeLimit <= 3)
				{
					num = 2;
				}
				else if (base.HighestAgeLimit <= 7)
				{
					num = 3;
				}
				else if (base.HighestAgeLimit <= 10)
				{
					num = 4;
				}
				else if (base.HighestAgeLimit <= 13)
				{
					num = 5;
				}
				else if (base.HighestAgeLimit <= 16)
				{
					num = 7;
				}
				else if (base.HighestAgeLimit <= 18)
				{
					num = 9;
				}
				else if (base.HighestAgeLimit == 99)
				{
					num = 99;
				}
				return num.ToString();
			}
		}

		[Browsable(false)]
		public string CommonRating
		{
			get
			{
				if (base.HighestAgeLimit == 99)
				{
					return "(Can't publish.)";
				}
				return base.HighestAgeLimit.ToString();
			}
		}

		public Metadata()
		{
			Reset();
		}

		public void Reset()
		{
			SelfFilePath = null;
			managedHeap = 32768;
			resourceHeap = 65536;
			MaxSize = MaxScreenSize._1280x800;
			CameraResolutionSize = MaxCameraResolutionSize._800x600;
			projectName = "*";
			appVersion = "1.00";
			runtimeVersion = Utility.ReadRuntimeVersionFile();
			sdkVersion = Utility.ReadSdkVersion();
			appLongNames.Clear();
			appShortNames.Clear();
			defaultLocale = Locale.en_US;
			icon512x512Full = "";
			icon256x256Full = "";
			icon128x128Full = "";
			splashFull = "";
			developerName = "";
			webSite = "";
			copyrightFull = "";
			UseScoreboards = false;
			DeviceGamePad = true;
			DeviceTouch = true;
			DeviceMotion = false;
			DeviceLocation = false;
			DeviceCamera = false;
			DevicePSVitaTV = false;
			Genre1 = Genre.None;
			Genre2 = Genre.None;
			CopyrightShort = "";
			productList.Clear();
			base.PegiAgeRatingText = "3";
			base.PegiCode = "";
			base.EsrbRatingText = MainForm.EVERYONE_3;
			EsrbRatingNumber = 3;
			base.EsrbCode = "";
			base.IsPegiDrugs = false;
			base.IsPegiLanguage = false;
			base.IsPegiSex = false;
			base.IsPegiScaryElements = false;
			base.IsPegiOnline = false;
			base.IsPegiDiscrimination = false;
			base.IsPegiViolence = false;
			base.IsPegiCrime = false;
			base.IsPegiGambling = false;
		}

		public static string MaxScreenSizeToString(MaxScreenSize rhs)
		{
			return maxScreenSizeConverter.ConvertTo(null, null, rhs, typeof(string)) as string;
		}

		public static MaxScreenSize StringToMaxScreenSize(string rhs)
		{
			return (MaxScreenSize)maxScreenSizeConverter.ConvertFrom(rhs);
		}

		public static string MaxCameraResolutionSizeToString(MaxCameraResolutionSize rhs)
		{
			return maxCameraResolutionSizeConverter.ConvertTo(null, null, rhs, typeof(string)) as string;
		}

		public static MaxCameraResolutionSize StringToMaxCameraResolutionSize(string rhs)
		{
			return (MaxCameraResolutionSize)maxCameraResolutionSizeConverter.ConvertFrom(rhs);
		}

		public static string LocaleToString(Locale rhs)
		{
			return localeConverter.ConvertTo(null, null, rhs, typeof(string)) as string;
		}

		public static Locale StringToLocale(string rhs)
		{
			return (Locale)localeConverter.ConvertFrom(rhs);
		}

		public static string GenreToString(Genre rhs)
		{
			return genreConverter.ConvertTo(null, null, rhs, typeof(string)) as string;
		}

		public static Genre StringToGenre(string rhs)
		{
			return (Genre)genreConverter.ConvertFrom(rhs);
		}

		public static string GenreToXmlString(Genre rhs)
		{
			return genreConverter.XmlConvertTo(rhs) as string;
		}

		public static Genre XmlStringToGenre(string rhs)
		{
			return (Genre)genreConverter.XmlConvertFrom(rhs);
		}

		public int AddProduct(object item, int idx = -1)
		{
			if (idx < 0)
			{
				return productList.Add(item);
			}
			if (idx >= productList.Count)
			{
				return -1;
			}
			productList.Insert(idx, item);
			return idx;
		}

		private bool IsFileExistsFullOrRelative(string rhs)
		{
			bool flag = File.Exists(rhs);
			if (!flag && SelfFilePath != null)
			{
				return File.Exists(Path.Combine(Path.GetDirectoryName(SelfFilePath), rhs));
			}
			return flag;
		}

		public static bool IsProjectNameValid(string rhs)
		{
			if (rhs.Length > 31)
			{
				return false;
			}
			if (rhs == "*")
			{
				return true;
			}
			Regex regex = new Regex("^[a-zA-Z0-9_-]+$");
			return regex.IsMatch(rhs);
		}

		public bool IsProjectNameInDevelopmentValid(string rhs)
		{
			if (rhs.Length > 31)
			{
				return false;
			}
			Regex regex = new Regex("^[a-zA-Z0-9_-]+$|^\\*$");
			return regex.IsMatch(rhs);
		}

		public bool IsAppVersionValid(string rhs)
		{
			Regex regex = new Regex("^[1-9][0-9]?\\.[0-9]{2}$");
			return regex.IsMatch(rhs);
		}

		public bool IsRuntimeVersionValid(string rhs)
		{
			Regex regex = new Regex("^[1-9]?[0-9]\\.[0-9]{2}$|^N/A$");
			return regex.IsMatch(rhs);
		}

		public static bool IsSdkVersionValid(string version)
		{
			version = version.TrimEnd();
			string text = "^[1-9]?[0-9]\\.[0-9][0-9]\\.[0-9][0-9]$";
			Regex regex = new Regex(text);
			if (regex.IsMatch(version))
			{
				return true;
			}
			MessageBox.Show("Invalid SDK Version format.\n-> " + version + "\n\nValid format is " + text + " .\n\nPlease check " + PsmSdk.SdkVersionFile + ".", "Publishing Utility - IsSdkVersionValid", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}

		public bool IsAppLongNamesValid(string rhs)
		{
			if (rhs != rhs.Trim())
			{
				return false;
			}
			if (rhs.Length < 2 || 64 < rhs.Length)
			{
				return false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(rhs);
			if (bytes.Length >= 128)
			{
				return false;
			}
			return true;
		}

		public bool IsAppShortNamesValid(string rhs)
		{
			if (rhs.Length < 2 || 16 < rhs.Length)
			{
				return false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(rhs);
			if (bytes.Length >= 52)
			{
				return false;
			}
			return true;
		}

		public bool IsDefaultLocaleValid(string rhs)
		{
			return localeConverter.IsValid(rhs);
		}

		public bool IsRatingLockLevelValid(string rhs)
		{
			if (int.TryParse(rhs, out var result) && 0 <= result && result <= 11)
			{
				return true;
			}
			return false;
		}

		public bool IsIcon512x512Valid(string rhs)
		{
			if (!IsFileExistsFullOrRelative(rhs))
			{
				if (!string.IsNullOrWhiteSpace(rhs))
				{
					MessageBox.Show($"Can't find {rhs}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			string imageFile = (Path.IsPathRooted(rhs) ? rhs : Path.Combine(SelfFileDir, rhs));
			return Utility.IsExpectFormatPng(imageFile, new Size(512, 512), PixelFormat.Format32bppArgb, alphaCheck: true);
		}

		public bool IsIcon256x256Valid(string rhs)
		{
			if (!IsFileExistsFullOrRelative(rhs))
			{
				if (!string.IsNullOrWhiteSpace(rhs))
				{
					MessageBox.Show($"Can't find {rhs}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			string imageFile = (Path.IsPathRooted(rhs) ? rhs : Path.Combine(SelfFileDir, rhs));
			return Utility.IsExpectFormatPng(imageFile, new Size(256, 256), PixelFormat.Format32bppArgb, alphaCheck: true);
		}

		public bool IsIcon128x128Valid(string rhs)
		{
			if (!IsFileExistsFullOrRelative(rhs))
			{
				if (!string.IsNullOrWhiteSpace(rhs))
				{
					MessageBox.Show($"Can't find {rhs}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			string imageFile = (Path.IsPathRooted(rhs) ? rhs : Path.Combine(SelfFileDir, rhs));
			return Utility.IsExpectFormatPng(imageFile, new Size(128, 128), PixelFormat.Format32bppArgb, alphaCheck: true);
		}

		public bool IsSplashValid(string rhs)
		{
			if (!IsFileExistsFullOrRelative(rhs))
			{
				if (!string.IsNullOrWhiteSpace(rhs))
				{
					MessageBox.Show($"Can't find {rhs}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}
			string file = (Path.IsPathRooted(rhs) ? rhs : Path.Combine(SelfFileDir, rhs));
			return Utility.IsExpectFormatPng(file, new Size(854, 480), PixelFormat.Format8bppIndexed);
		}

		public bool IsGenre1Valid(string rhs)
		{
			Genre genre = StringToGenre(rhs);
			return genre != Genre.None;
		}

		public bool IsGenre2Valid(string rhs)
		{
			return true;
		}

		public bool IsDeveloperNameValid(string rhs)
		{
			if (64 < rhs.Length)
			{
				return false;
			}
			Regex regex = new Regex("^[ -~]+$");
			return regex.IsMatch(rhs);
		}

		public bool IsDeveloperWebSiteValid(string rhs)
		{
			if (255 < rhs.Length)
			{
				return false;
			}
			Regex regex = new Regex("^http://.+$|^https://.+$");
			return regex.IsMatch(rhs);
		}

		public bool IsCopyrightValid(string rhs)
		{
			if (Path.GetExtension(rhs) == ".txt")
			{
				return IsFileExistsFullOrRelative(rhs);
			}
			return false;
		}

		public bool IsCopyrightShortValid(string rhs)
		{
			if (string.IsNullOrWhiteSpace(rhs))
			{
				return false;
			}
			if (255 < rhs.Length)
			{
				return false;
			}
			return true;
		}

		public bool IsProductLabelValid(string rhs)
		{
			if (rhs == "000000")
			{
				return false;
			}
			Regex regex = new Regex("^[A-Z0-9]{6}$");
			return regex.IsMatch(rhs);
		}

		public bool IsProductNameValid(string rhs)
		{
			if (64 < rhs.Length)
			{
				return false;
			}
			return true;
		}

		public static bool IsValidMetadataForPublish(Metadata meta, out string textCheckResult)
		{
			bool result = true;
			textCheckResult = "";
			string[] array = new string[10] { "ProjectName", "AppVersion", "RuntimeVersion", "Icon512x512", "Icon256x256", "Icon128x128", "Splash", "DeveloperWebSite", "Copyright", "CopyrightShort" };
			string[] array2 = array;
			foreach (string text in array2)
			{
				PropertyInfo property = typeof(IMetadata).GetProperty(text);
				MethodInfo method = typeof(Metadata).GetMethod("Is" + text + "Valid");
				CategoryAttribute categoryAttribute = (CategoryAttribute)Attribute.GetCustomAttribute(property, typeof(CategoryAttribute));
				DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)Attribute.GetCustomAttribute(property, typeof(DisplayNameAttribute));
				object[] array3 = new object[1] { property.GetValue(meta, null) };
				if (!(bool)method.Invoke(meta, array3))
				{
					result = false;
					textCheckResult += $"{categoryAttribute.Category} / {displayNameAttribute.DisplayName} : \"{array3[0]}\" is invalid.\n";
				}
			}
			if (meta.Genre1 == Genre.None)
			{
				result = false;
				textCheckResult += $"Primary Genre is not set.\n";
			}
			foreach (Locale value4 in Enum.GetValues(typeof(Locale)))
			{
				string text2 = (string)meta.AppLongNames[value4];
				if (!string.IsNullOrEmpty(text2) && !meta.IsAppLongNamesValid(text2))
				{
					result = false;
					textCheckResult += $"Apps Long Name ({LocaleToString(value4)}): \"{text2}\" is invalid.\n";
				}
			}
			foreach (Locale value5 in Enum.GetValues(typeof(Locale)))
			{
				string text3 = (string)meta.AppShortNames[value5];
				if (!string.IsNullOrEmpty(text3) && !meta.IsAppShortNamesValid(text3))
				{
					result = false;
					textCheckResult += $"Apps Short Name ({LocaleToString(value5)}): \"{text3}\" is invalid.\n";
				}
			}
			if (meta.ProductList.Count > 20)
			{
				result = false;
				textCheckResult += $"Product num is too much : num = {meta.ProductList.Count}.\n";
			}
			foreach (Product product3 in meta.ProductList)
			{
				if (!meta.IsProductLabelValid(product3.Label))
				{
					result = false;
					textCheckResult += $"Product Label: \"{product3.Label}\" is invalid.\n";
				}
				foreach (Locale value6 in Enum.GetValues(typeof(Locale)))
				{
					string text4 = (string)product3.Names[value6];
					if (!string.IsNullOrEmpty(text4) && !meta.IsProductNameValid(text4))
					{
						result = false;
						textCheckResult += $"Product Name {product3.Label} ({LocaleToString(value6)}): \"{text4}\" is invalid.\n";
					}
				}
			}
			Locale locale4 = meta.DefaultLocale;
			string value = (string)meta.AppLongNames[meta.DefaultLocale];
			if (string.IsNullOrEmpty(value))
			{
				result = false;
				textCheckResult += string.Format("Default Locale is {0}, but Apps Long Name ({0}) isn't defined.\n", LocaleToString(locale4));
			}
			string value2 = (string)meta.AppShortNames[meta.DefaultLocale];
			if (string.IsNullOrEmpty(value2))
			{
				result = false;
				textCheckResult += string.Format("Default Locale is {0}, but Apps Short Name ({0}) isn't defined.\n", LocaleToString(locale4));
			}
			foreach (Product product4 in meta.ProductList)
			{
				string value3 = (string)product4.Names[locale4];
				if (string.IsNullOrEmpty(value3))
				{
					result = false;
					textCheckResult += string.Format("Default Locale is {0}, but Product [{1}] ({0}) isn't defined.\n", LocaleToString(locale4), product4.Label);
				}
			}
			if (string.IsNullOrEmpty(meta.PegiCode) && (meta.Genre1 & Genre.Game_Action) != 0)
			{
				result = false;
				textCheckResult = textCheckResult + "\n" + Resources.notEnterPEGI_Text + "\n";
			}
			if (string.IsNullOrEmpty(meta.EsrbCode) && (meta.Genre1 & Genre.Game_Action) != 0)
			{
				result = false;
				textCheckResult = textCheckResult + "\n" + Resources.notEnterESRB_Text + "\n";
			}
			if ((meta.Genre1 & Genre.App_Audio) != 0 && !string.IsNullOrEmpty(meta.PegiCode))
			{
				result = false;
				textCheckResult += ((Program.appConfigData.PcLocale == "ja-JP") ? "\nPSMアプリのジャンルがゲームでない場合、PEGIのRegistered Numberに値を入力しないでください。\n" : "\nIf the genre of the PSM app is not a game, do not input a PEGI registered number value.\n");
			}
			if ((meta.Genre1 & Genre.App_Audio) != 0 && !string.IsNullOrEmpty(meta.EsrbCode))
			{
				result = false;
				textCheckResult += ((Program.appConfigData.PcLocale == "ja-JP") ? "\nPSMアプリのジャンルがゲームでない場合、ESRBのCertificate Codeに値を入力しないでください。\n" : "\nIf the genre of the PSM app is not a game, do not input a ESRB Certificate Code value.\n");
			}
			if (!Program._RatingData.IsCheckedOffSceRating)
			{
				result = false;
				textCheckResult = textCheckResult + "\n" + Resources.notEnterPSMRating_Text + "\n";
			}
			if (meta.IsPegiDiscrimination)
			{
				result = false;
				textCheckResult = textCheckResult + "\n" + ((Program.appConfigData.PcLocale == "ja-JP") ? "\nPEGI Express のレーティング結果で差別表現がチェックされています。\n弊社のビジネスポリシーにより、このPSMアプリはDevPortalにアップロードできません。\n" : "\nThe PEGI Express rating results check for depictions of discrimination. \nBecause of our business policies, this PSM App cannot be uploaded to DevPortal.\n");
			}
			if (meta.HighestAgeLimit.ToString() == "99")
			{
				result = false;
				textCheckResult += string.Format("\n" + Resources.cannotSellContentMsgBoxBody_Text);
			}
			if (meta.EsrbRatingNumber >= 18)
			{
				result = false;
				textCheckResult = textCheckResult + "\n" + ((Program.appConfigData.PcLocale == "ja-JP") ? "\nESRB Short Formのレーティング結果がAdult Onlyのため、弊社のビジネスポリシーにより、このPSMアプリはDevPortalにアップロードできません。\n" : "\nAs the rating result of ESRB Short Form is Adult Only, your PSM application cannot be uploaded to the DevPortal due to our  business policy.\n");
			}
			return result;
		}
	}
}
