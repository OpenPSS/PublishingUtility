using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace PublishingUtility
{
	internal class Xml
	{
		private const string PEGI_SCARYELEMENTS = "Fear";

		private const string PEGI_LANGUAGE = "Language";

		private const string PEGI_VIOLENCE = "Violence";

		private const string PEGI_CRIME = "Crime";

		private const string PEGI_GAMBLING = "Gambling";

		private const string PEGI_DISCRIMINATION = "Discrimination";

		private const string PEGI_ONLINE = "Online";

		private const string PEGI_DRUGS = "Drugs";

		private const string PEGI_SEX = "Sex";

		private static bool success = true;

		public static void SaveIntoFile(string fileName, XElement root)
		{
			Console.WriteLine("==Save()==");
			XDocument xDocument = new XDocument(root);
			UTF8Encoding encoding = new UTF8Encoding();
			StreamWriter streamWriter = new StreamWriter(fileName, append: false, encoding);
			xDocument.Save(streamWriter);
			streamWriter.Close();
			streamWriter.Dispose();
		}

		public static XDocument LoadFromFile(string fileName, bool validate = false, bool errorExit = true)
		{
			try
			{
				if (validate)
				{
					XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
					success = true;
					using (Stream input = typeof(Program).Assembly.GetManifestResourceStream("PublishingUtility.app.xsd"))
					{
						xmlSchemaSet.Add("", XmlReader.Create(input));
					}
					XDocument xDocument = XDocument.Load(fileName);
					if (errorExit)
					{
						xDocument.Validate(xmlSchemaSet, ValidationCallBackWithErrorExit);
					}
					else
					{
						xDocument.Validate(xmlSchemaSet, ValidationCallBack);
					}
					if (!success)
					{
						return null;
					}
					return xDocument;
				}
				return XDocument.Load(fileName);
			}
			catch (XmlSchemaException ex)
			{
				MessageBox.Show(Utility.TextLanguage("The exception occurred on schema check.", "app.xmlのスキーマチェックで例外が発生しました。") + ex.Message, "Publishing Utility - Xml Schema Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			catch (Exception ex2)
			{
				MessageBox.Show("Tried to open invalid XML file.\n" + ex2.Message, "Publishing Utility - Xml Schema Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return null;
		}

		private static void ValidationCallBackWithErrorExit(object sender, ValidationEventArgs args)
		{
			if (args.Severity == XmlSeverityType.Error)
			{
				MessageBox.Show(Utility.TextLanguage("The schema of app.xml was invalid.\n\nPlease re-edit app.xml by Publishing Utility and save it.\n\nError Information:\n", "app.xmlのスキーマが不正です。\n\nPublishing Utilityでapp.xmlを再編集し、セーブしなおしてください。\n\nエラー情報:\n") + args.Message, "Publishing Utility - Xml Schema Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
			}
			if (args.Severity == XmlSeverityType.Warning)
			{
				MessageBox.Show("Warning: " + args.Message, Utility.TextLanguage("app.xml Validation Check", "app.xml のスキーマチェック"));
			}
		}

		private static void ValidationCallBack(object sender, ValidationEventArgs args)
		{
			if (args.Severity == XmlSeverityType.Error)
			{
				success = false;
				MessageBox.Show(Utility.TextLanguage("The schema of app.xml was invalid.\n\nPlease re-edit app.xml by Publishing Utility and save it.\n\nError Information:\n", "app.xmlのスキーマが不正です。\n\nPublishing Utilityでapp.xmlを再編集し、セーブしなおしてください。\n\nエラー情報:\n") + args.Message, "Publishing Utility - Xml Schema Check", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (args.Severity == XmlSeverityType.Warning)
			{
				success = false;
				MessageBox.Show("Warning: " + args.Message, Utility.TextLanguage("app.xml Validation Check", "app.xml のスキーマチェック"));
			}
		}

		public static void WriteToXml(Metadata metadata, out XElement root, bool isFixedPath = false)
		{
			if (!string.IsNullOrEmpty(metadata.SdkVersion))
			{
				root = new XElement("application", new XAttribute("project_name", metadata.ProjectName), new XAttribute("version", metadata.AppVersion), new XAttribute("runtime_version", metadata.RuntimeVersion), new XAttribute("sdk_version", metadata.SdkVersion), new XAttribute("default_locale", Metadata.LocaleToString(metadata.DefaultLocale)));
			}
			else
			{
				root = new XElement("application", new XAttribute("project_name", metadata.ProjectName), new XAttribute("version", metadata.AppVersion), new XAttribute("runtime_version", metadata.RuntimeVersion), new XAttribute("default_locale", Metadata.LocaleToString(metadata.DefaultLocale)));
			}
			XElement content = new XElement("app_xml_format", new XAttribute("version", "1.00.00"), new XAttribute("sdk_type", "PSM SDK"));
			root.Add(content);
			XElement xElement = new XElement("name");
			root.Add(xElement);
			foreach (Locale value5 in Enum.GetValues(typeof(Locale)))
			{
				string value = (string)metadata.AppLongNames[value5];
				if (!string.IsNullOrEmpty(value))
				{
					xElement.Add(new XElement("localized_item", new XAttribute("locale", Metadata.LocaleToString(value5)), new XAttribute("value", value)));
				}
			}
			XElement xElement2 = new XElement("short_name");
			root.Add(xElement2);
			foreach (Locale value6 in Enum.GetValues(typeof(Locale)))
			{
				string value2 = (string)metadata.AppShortNames[value6];
				if (!string.IsNullOrEmpty(value2))
				{
					xElement2.Add(new XElement("localized_item", new XAttribute("locale", Metadata.LocaleToString(value6)), new XAttribute("value", value2)));
				}
			}
			root.Add(new XElement("parental_control", new XAttribute("lock_level", metadata.ParentalLockLevel)));
			XElement xElement3 = new XElement("rating", new XAttribute("type", "PEGIEX"), new XAttribute("value", metadata.PegiAgeRatingText), new XAttribute("age", metadata.PegiAgeRatingText), new XAttribute("code", metadata.PegiCode));
			if (metadata.IsPegiDrugs)
			{
				xElement3.Add(new XElement("descriptor", "Drugs"));
			}
			if (metadata.IsPegiLanguage)
			{
				xElement3.Add(new XElement("descriptor", "Language"));
			}
			if (metadata.IsPegiSex)
			{
				xElement3.Add(new XElement("descriptor", "Sex"));
			}
			if (metadata.IsPegiScaryElements)
			{
				xElement3.Add(new XElement("descriptor", "Fear"));
			}
			if (metadata.IsPegiOnline)
			{
				xElement3.Add(new XElement("descriptor", "Online"));
			}
			if (metadata.IsPegiDiscrimination)
			{
				xElement3.Add(new XElement("descriptor", "Discrimination"));
			}
			if (metadata.IsPegiViolence)
			{
				xElement3.Add(new XElement("descriptor", "Violence"));
			}
			if (metadata.IsPegiCrime)
			{
				xElement3.Add(new XElement("descriptor", "Crime"));
			}
			if (metadata.IsPegiGambling)
			{
				xElement3.Add(new XElement("descriptor", "Gambling"));
			}
			XElement xElement4 = null;
			if (Program._RatingData.IsCheckedOffSceRating)
			{
				xElement4 = new XElement("rating", new XAttribute("type", "SELF"), new XAttribute("value", Program._RatingData.RatingAge.ToString()), new XAttribute("age", Program._RatingData.RatingAge.ToString()));
				if (Program._RatingData.IsCsScaryElements)
				{
					xElement4.Add(new XElement("descriptor", "Fear"));
				}
				if (Program._RatingData.IsCsLanguage)
				{
					xElement4.Add(new XElement("descriptor", "Language"));
				}
				if (Program._RatingData.IsCsViolence)
				{
					xElement4.Add(new XElement("descriptor", "Violence"));
				}
				if (Program._RatingData.IsCsGambling)
				{
					xElement4.Add(new XElement("descriptor", "Gambling"));
				}
				if (Program._RatingData.IsCsDiscrimination)
				{
					xElement4.Add(new XElement("descriptor", "Discrimination"));
				}
				if (Program._RatingData.IsCsOnline)
				{
					xElement4.Add(new XElement("descriptor", "Online"));
				}
				if (Program._RatingData.IsCsDrugs)
				{
					xElement4.Add(new XElement("descriptor", "Drugs"));
				}
				if (Program._RatingData.IsCsSex)
				{
					xElement4.Add(new XElement("descriptor", "Sex"));
				}
			}
			XElement xElement5 = new XElement("online_features", new XAttribute("chat", Program._RatingData.IsCsOnlineChat), new XAttribute("personal_info", Program._RatingData.IsCsOnlinePersonalInfo), new XAttribute("user_location", Program._RatingData.IsCsOnlineUsersLocation), new XAttribute("exchange_content", Program._RatingData.IsCsOnlineExchangeContent));
			if (Program._RatingData.IsCsOnlineMinimumAge)
			{
				xElement5.Add(new XAttribute("minimum_age", Program._RatingData.CsOnlineMinimumAge.ToString()));
			}
			if ((metadata.Genre1 & Genre.App_Audio) != 0)
			{
				root.Add(new XElement("rating_list", new XAttribute("highest_age_limit", metadata.HighestAgeLimit.ToString()), new XAttribute("has_online_features", Program._RatingData.IsCsOnlineFeatures), xElement5, xElement4));
			}
			else
			{
				root.Add(new XElement("rating_list", new XAttribute("highest_age_limit", metadata.HighestAgeLimit.ToString()), new XAttribute("has_online_features", Program._RatingData.IsCsOnlineFeatures), xElement5, new XElement("rating", new XAttribute("type", "ESRB"), new XAttribute("value", metadata.EsrbRatingNumber.ToString()), new XAttribute("age", metadata.EsrbRatingNumber.ToString()), new XAttribute("code", metadata.EsrbCode)), xElement3, xElement4));
			}
			root.Add(new XElement("images", string.IsNullOrEmpty(metadata.Icon128x128) ? null : new XAttribute("icon_128x128", isFixedPath ? metadata.FixedIcon128Path : metadata.Icon128x128), string.IsNullOrEmpty(metadata.Icon256x256) ? null : new XAttribute("icon_256x256", isFixedPath ? metadata.FixedIcon256Path : metadata.Icon256x256), string.IsNullOrEmpty(metadata.Icon512x512) ? null : new XAttribute("icon_512x512", isFixedPath ? metadata.FixedIcon512Path : metadata.Icon512x512), string.IsNullOrEmpty(metadata.Splash) ? null : new XAttribute("splash_854x480", isFixedPath ? metadata.FixedSplashPath : metadata.Splash)));
			XElement xElement6 = new XElement("genre_list");
			root.Add(xElement6);
			if (metadata.Genre1 != 0)
			{
				xElement6.Add(new XElement("genre", new XAttribute("value", Metadata.GenreToXmlString(metadata.Genre1))));
			}
			if (metadata.Genre2 != 0)
			{
				xElement6.Add(new XElement("genre", new XAttribute("value", Metadata.GenreToXmlString(metadata.Genre2))));
			}
			root.Add(new XElement("developer", new XElement("name", new XAttribute("value", metadata.DeveloperName))));
			root.Add(new XElement("website", new XAttribute("href", metadata.DeveloperWebSite)));
			root.Add(new XElement("copyright", new XAttribute("author", metadata.CopyrightShort), string.IsNullOrEmpty(metadata.Copyright) ? null : new XAttribute("text", isFixedPath ? metadata.FixedCopyrightPath : metadata.Copyright)));
			XElement xElement7 = new XElement("product_list");
			foreach (Product product in metadata.ProductList)
			{
				if (product.Names.Count == 0)
				{
					continue;
				}
				string value3 = (product.Consumable ? "consumable" : "normal");
				XElement xElement8 = new XElement("product", new XAttribute("label", product.Label), new XAttribute("type", value3));
				XElement xElement9 = new XElement("name");
				xElement8.Add(xElement9);
				xElement7.Add(xElement8);
				foreach (Locale value7 in Enum.GetValues(typeof(Locale)))
				{
					string value4 = (string)product.Names[value7];
					if (!string.IsNullOrEmpty(value4))
					{
						xElement9.Add(new XElement("localized_item", new XAttribute("locale", Metadata.LocaleToString(value7)), new XAttribute("value", value4)));
					}
				}
			}
			root.Add(new XElement("purchase", xElement7));
			root.Add(new XElement("runtime_config", new XElement("memory", new XAttribute("managed_heap_size", metadata.ManagedHeap), new XAttribute("resource_heap_size", metadata.ResourceHeap)), new XElement("display", new XAttribute("max_screen_size", Metadata.MaxScreenSizeToString(metadata.MaxSize))), new XElement("camera", new XAttribute("max_capture_resolution", Metadata.MaxCameraResolutionSizeToString(metadata.CameraResolutionSize)))));
			XElement xElement10 = new XElement("feature_list");
			if (metadata.DeviceGamePad)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "GamePad")));
			}
			if (metadata.DeviceTouch)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "Touch")));
			}
			if (metadata.DeviceMotion)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "Motion")));
			}
			if (metadata.DeviceLocation)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "Location")));
			}
			if (metadata.DeviceCamera)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "Camera")));
			}
			if (metadata.DevicePSVitaTV)
			{
				xElement10.Add(new XElement("feature", new XAttribute("value", "PSVitaTV")));
			}
			xElement10.Add(new XElement("feature", new XAttribute("value", "##SDKType:PSM SDK")));
			root.Add(xElement10);
			XElement xElement11 = new XElement("psn_service_list");
			if (metadata.UseScoreboards)
			{
				xElement11.Add(new XElement("psn_service", new XAttribute("value", "Scoreboards")));
			}
			root.Add(xElement11);
		}

		public static bool ReadFromXml(Metadata metadata, XElement root)
		{
			try
			{
				XElement xElement = root;
				if (xElement != null)
				{
					foreach (XAttribute item in xElement.Attributes())
					{
						if (item.Name == "project_name")
						{
							metadata.ProjectName = item.Value;
						}
						else if (item.Name == "version")
						{
							metadata.AppVersion = item.Value;
						}
						else if (!(item.Name == "runtime_version") && item.Name == "default_locale")
						{
							metadata.DefaultLocale = Metadata.StringToLocale(item.Value);
						}
					}
				}
				xElement = root.Element("name");
				if (xElement != null)
				{
					foreach (XElement item2 in xElement.Elements("localized_item"))
					{
						metadata.AppLongNames[Metadata.StringToLocale(item2.Attribute("locale").Value)] = item2.Attribute("value").Value;
					}
				}
				xElement = root.Element("short_name");
				if (xElement != null)
				{
					foreach (XElement item3 in xElement.Elements("localized_item"))
					{
						metadata.AppShortNames[Metadata.StringToLocale(item3.Attribute("locale").Value)] = item3.Attribute("value").Value;
					}
				}
				xElement = root.Element("rating_list");
				metadata.IsPegiDrugs = false;
				metadata.IsPegiLanguage = false;
				metadata.IsPegiSex = false;
				metadata.IsPegiScaryElements = false;
				metadata.IsPegiOnline = false;
				metadata.IsPegiDiscrimination = false;
				metadata.IsPegiViolence = false;
				metadata.IsPegiCrime = false;
				metadata.IsPegiGambling = false;
				Program._RatingData.IsCheckedOffSceRating = false;
				if (xElement != null)
				{
					XAttribute xAttribute = xElement.Attribute("has_online_features");
					if (xAttribute != null && xAttribute.Value == "true")
					{
						Program._RatingData.IsCsOnlineFeatures = true;
					}
					else
					{
						Program._RatingData.IsCsOnlineFeatures = false;
					}
					XElement xElement2 = xElement.Element("online_features");
					if (xElement2 != null)
					{
						if (xElement2.Attribute("chat") != null && xElement2.Attribute("chat").Value == "true")
						{
							Program._RatingData.IsCsOnlineChat = true;
						}
						else
						{
							Program._RatingData.IsCsOnlineChat = false;
						}
						if (xElement2.Attribute("personal_info") != null && xElement2.Attribute("personal_info").Value == "true")
						{
							Program._RatingData.IsCsOnlinePersonalInfo = true;
						}
						else
						{
							Program._RatingData.IsCsOnlinePersonalInfo = false;
						}
						if (xElement2.Attribute("user_location") != null && xElement2.Attribute("user_location").Value == "true")
						{
							Program._RatingData.IsCsOnlineUsersLocation = true;
						}
						else
						{
							Program._RatingData.IsCsOnlineUsersLocation = false;
						}
						if (xElement2.Attribute("exchange_content") != null && xElement2.Attribute("exchange_content").Value == "true")
						{
							Program._RatingData.IsCsOnlineExchangeContent = true;
						}
						else
						{
							Program._RatingData.IsCsOnlineUsersLocation = false;
						}
						if (xElement2.Attribute("minimum_age") != null)
						{
							Program._RatingData.IsCsOnlineMinimumAge = true;
							Program._RatingData.CsOnlineMinimumAge = int.Parse(xElement2.Attribute("minimum_age").Value);
						}
						else
						{
							Program._RatingData.IsCsOnlineMinimumAge = false;
							Program._RatingData.CsOnlineMinimumAge = 0;
						}
					}
					IEnumerable<XElement> enumerable = xElement.Elements("rating");
					foreach (XElement item4 in enumerable)
					{
						if (item4.Attribute("type").Value == "PEGIEX")
						{
							metadata.PegiCode = item4.Attribute("code").Value;
							metadata.PegiAgeRatingText = item4.Attribute("value").Value;
							IEnumerable<XElement> enumerable2 = item4.Elements("descriptor");
							foreach (XElement item5 in enumerable2)
							{
								switch (item5.Value.ToString())
								{
								case "Drugs":
									metadata.IsPegiDrugs = true;
									break;
								case "Language":
									metadata.IsPegiLanguage = true;
									break;
								case "Sex":
									metadata.IsPegiSex = true;
									break;
								case "Fear":
									metadata.IsPegiScaryElements = true;
									break;
								case "Online":
									metadata.IsPegiOnline = true;
									break;
								case "Discrimination":
									metadata.IsPegiDiscrimination = true;
									break;
								case "Violence":
									metadata.IsPegiViolence = true;
									break;
								case "Crime":
									metadata.IsPegiCrime = true;
									break;
								case "Gambling":
									metadata.IsPegiGambling = true;
									break;
								default:
									MessageBox.Show("ERROR: unknown element value=" + item5.Value.ToString() + ".", "app.xml descriptor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
									break;
								}
							}
						}
						if (item4.Attribute("type").Value == "SELF")
						{
							Program._RatingData.IsCheckedOffSceRating = true;
							Program._RatingData.RatingAge = int.Parse(item4.Attribute("value").Value);
							IEnumerable<XElement> enumerable3 = item4.Elements("descriptor");
							foreach (XElement item6 in enumerable3)
							{
								switch (item6.Value.ToString())
								{
								case "Fear":
									Program._RatingData.IsCsScaryElements = true;
									break;
								case "Language":
									Program._RatingData.IsCsLanguage = true;
									break;
								case "Violence":
									Program._RatingData.IsCsViolence = true;
									break;
								case "Gambling":
									Program._RatingData.IsCsGambling = true;
									break;
								case "Discrimination":
									Program._RatingData.IsCsDiscrimination = true;
									break;
								case "Online":
									Program._RatingData.IsCsOnline = true;
									break;
								case "Drugs":
									Program._RatingData.IsCsDrugs = true;
									break;
								case "Sex":
									Program._RatingData.IsCsSex = true;
									break;
								}
							}
						}
						if (item4.Attribute("type").Value == "ESRB")
						{
							metadata.EsrbCode = item4.Attribute("code").Value;
							switch (item4.Attribute("value").Value)
							{
							case "3":
								metadata.EsrbRatingText = MainForm.EVERYONE_3;
								metadata.EsrbRatingNumber = 3;
								break;
							case "6":
								metadata.EsrbRatingText = MainForm.EVERYONE_3;
								metadata.EsrbRatingNumber = 3;
								break;
							case "10":
								metadata.EsrbRatingText = MainForm.EVERYONE_10;
								metadata.EsrbRatingNumber = 10;
								break;
							case "13":
								metadata.EsrbRatingText = MainForm.TEEN_13;
								metadata.EsrbRatingNumber = 13;
								break;
							case "17":
								metadata.EsrbRatingText = MainForm.MATURE_17;
								metadata.EsrbRatingNumber = 17;
								break;
							case "18":
								metadata.EsrbRatingText = MainForm.ADULTS_ONLY_18;
								metadata.EsrbRatingNumber = 18;
								break;
							default:
								metadata.EsrbRatingText = MainForm.EVERYONE_3;
								metadata.EsrbRatingNumber = 3;
								MessageBox.Show("Default value in ESRB Rating.", "Publishing Utility");
								break;
							}
						}
					}
				}
				xElement = root.Element("images");
				if (xElement != null)
				{
					foreach (XAttribute item7 in xElement.Attributes())
					{
						if (item7.Name == "icon_128x128")
						{
							metadata.Icon128x128 = item7.Value;
						}
						else if (item7.Name == "icon_256x256")
						{
							metadata.Icon256x256 = item7.Value;
						}
						else if (item7.Name == "icon_512x512")
						{
							metadata.Icon512x512 = item7.Value;
						}
						else if (item7.Name == "splash_854x480")
						{
							metadata.Splash = item7.Value;
						}
					}
				}
				xElement = root.Element("genre_list");
				if (xElement != null)
				{
					int num = 0;
					foreach (XElement item8 in xElement.Elements("genre"))
					{
						XAttribute xAttribute2 = item8.Attribute("value");
						if (xAttribute2 != null)
						{
							switch (num)
							{
							case 0:
								metadata.Genre1 = Metadata.XmlStringToGenre(xAttribute2.Value);
								num++;
								continue;
							case 1:
								metadata.Genre2 = Metadata.XmlStringToGenre(xAttribute2.Value);
								num++;
								continue;
							}
							break;
						}
					}
				}
				xElement = root.Element("developer");
				if (xElement != null)
				{
					foreach (XElement item9 in xElement.Elements("name"))
					{
						XAttribute xAttribute3 = item9.Attribute("value");
						if (xAttribute3 != null)
						{
							metadata.DeveloperName = xAttribute3.Value;
						}
					}
				}
				xElement = root.Element("website");
				if (xElement != null)
				{
					XAttribute xAttribute4 = xElement.Attribute("href");
					if (xAttribute4 != null)
					{
						metadata.DeveloperWebSite = xAttribute4.Value;
					}
				}
				xElement = root.Element("copyright");
				if (xElement != null)
				{
					XAttribute xAttribute5 = xElement.Attribute("author");
					if (xAttribute5 != null)
					{
						metadata.CopyrightShort = xAttribute5.Value;
					}
					xAttribute5 = xElement.Attribute("text");
					if (xAttribute5 != null)
					{
						metadata.Copyright = xAttribute5.Value;
					}
				}
				xElement = root.Element("purchase");
				if (xElement != null)
				{
					XElement xElement3 = xElement.Element("product_list");
					if (xElement3 != null)
					{
						foreach (XElement item10 in xElement3.Elements("product"))
						{
							XAttribute xAttribute6 = item10.Attribute("label");
							int num2 = 0;
							if (xAttribute6 == null)
							{
								continue;
							}
							num2 = metadata.AddProduct(new Product(xAttribute6.Value));
							Product product = metadata.ProductList[num2] as Product;
							XAttribute xAttribute7 = item10.Attribute("type");
							if (xAttribute7 != null && xAttribute7.Value == "consumable")
							{
								product.Consumable = true;
							}
							XElement xElement4 = item10.Element("name");
							foreach (XElement item11 in xElement4.Elements("localized_item"))
							{
								product.Names[Metadata.StringToLocale(item11.Attribute("locale").Value)] = item11.Attribute("value").Value;
							}
						}
					}
				}
				xElement = root.Element("runtime_config");
				if (xElement != null)
				{
					XElement xElement3 = xElement.Element("memory");
					if (xElement3 != null)
					{
						XAttribute xAttribute8 = xElement3.Attribute("managed_heap_size");
						int result;
						if (xAttribute8 != null)
						{
							metadata.ResourceHeap = 1024;
							if (int.TryParse(xAttribute8.Value, out result))
							{
								metadata.ManagedHeap = result;
							}
						}
						xAttribute8 = xElement3.Attribute("resource_heap_size");
						if (xAttribute8 != null && int.TryParse(xAttribute8.Value, out result))
						{
							metadata.ResourceHeap = result;
						}
					}
					xElement3 = xElement.Element("display");
					if (xElement3 != null)
					{
						XAttribute xAttribute9 = xElement3.Attribute("max_screen_size");
						if (xAttribute9 != null)
						{
							metadata.MaxSize = Metadata.StringToMaxScreenSize(xAttribute9.Value);
						}
					}
					xElement3 = xElement.Element("camera");
					if (xElement3 != null)
					{
						XAttribute xAttribute10 = xElement3.Attribute("max_capture_resolution");
						if (xAttribute10 != null)
						{
							metadata.CameraResolutionSize = Metadata.StringToMaxCameraResolutionSize(xAttribute10.Value);
						}
					}
				}
				metadata.DeviceGamePad = false;
				metadata.DeviceTouch = false;
				metadata.DeviceMotion = false;
				metadata.DeviceLocation = false;
				metadata.DeviceCamera = false;
				metadata.DevicePSVitaTV = false;
				xElement = root.Element("feature_list");
				if (xElement != null)
				{
					foreach (XElement item12 in xElement.Elements("feature"))
					{
						XAttribute xAttribute11 = item12.Attribute("value");
						if (xAttribute11 != null)
						{
							if (xAttribute11.Value == "GamePad")
							{
								metadata.DeviceGamePad = true;
							}
							if (xAttribute11.Value == "Touch")
							{
								metadata.DeviceTouch = true;
							}
							if (xAttribute11.Value == "Motion")
							{
								metadata.DeviceMotion = true;
							}
							if (xAttribute11.Value == "Location")
							{
								metadata.DeviceLocation = true;
							}
							if (xAttribute11.Value == "Camera")
							{
								metadata.DeviceCamera = true;
							}
							if (xAttribute11.Value == "PSVitaTV")
							{
								metadata.DevicePSVitaTV = true;
							}
						}
					}
				}
				metadata.UseScoreboards = false;
				xElement = root.Element("psn_service_list");
				if (xElement != null)
				{
					foreach (XElement item13 in xElement.Elements("psn_service"))
					{
						XAttribute xAttribute12 = item13.Attribute("value");
						if (xAttribute12 != null && xAttribute12.Value == "Scoreboards")
						{
							metadata.UseScoreboards = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Utility.TextLanguage($"Failed to load Metadata.\n\nIllegal data file.\n\n{ex.Message}", $"Metadataのロードに失敗しました。\n\n不正なデータファイルです。\n\n{ex.Message}"), "Load Metadata - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}

		public static string ConvertToSdkVersion(string unityOriginalRuntimeVersion)
		{
			string[] array = unityOriginalRuntimeVersion.Split('.');
			if (array.Length != 4)
			{
				MessageBox.Show("unity_original_runtime_version is invalid format.", "Publishing Utility - Read app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return "0.00.00";
			}
			return $"{int.Parse(array[0])}.{int.Parse(array[1]):d2}.{int.Parse(array[2]):d2}";
		}

		public static string ConvertToRuntimeVersion(string unityOriginalRuntimeVersion)
		{
			string[] array = unityOriginalRuntimeVersion.Split('.');
			if (array.Length != 4)
			{
				MessageBox.Show("runtime_version is invalid format.", "Publishing Utility - Read app.xml", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return "0.00";
			}
			return $"{int.Parse(array[0])}.{int.Parse(array[1]):d2}";
		}
	}
}
