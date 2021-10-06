using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using PublishingUtility.Dialog;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class SubmitGetMeta
	{
		private byte[] mTicket;

		private AppXmlInfo mappXmlInfo;

		private string mXmlParam;

		private bool mUseXml;

		private string developerName = "";

		private string actionInformationText = "";

		private string changedUnmodificationText;

		private bool updateBlocked;

		public string DeveloperName => developerName;

		public string ActionInformationText => actionInformationText;

		public string ChangedUnmodificationText => changedUnmodificationText;

		public bool UpdateBlocked => updateBlocked;

		public DialogResult ExecFromXml(byte[] Ticket, string XmlFile, AppXmlInfo appXmlInfo, string Text, ref string Result)
		{
			return Exec(Ticket, XmlFile, appXmlInfo, Text, ref Result, useXml: true);
		}

		public DialogResult ExecFromProjectName(byte[] Ticket, string projectName, string Text, ref string Result)
		{
			return Exec(Ticket, projectName, default(AppXmlInfo), Text, ref Result, useXml: false);
		}

		private DialogResult Exec(byte[] Ticket, string xmlParam, AppXmlInfo appXmlInfo, string Text, ref string Result, bool useXml)
		{
			DialogResult dialogResult = DialogResult.None;
			mTicket = Ticket;
			mappXmlInfo = appXmlInfo;
			mXmlParam = xmlParam;
			mUseXml = useXml;
			Progress progress = new Progress(Work, Text, 1);
			dialogResult = progress.ShowDialog();
			progress.Dispose();
			Result = ((progress.Error != null) ? progress.Error.ToString() : ((progress.Result != null) ? progress.Result.ToString() : ""));
			return dialogResult;
		}

		private void Work(object sender, DoWorkEventArgs e)
		{
			try
			{
				BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
				e.Cancel = false;
				string text = $"https://sdk.{Program.appConfigData.EnvServer}.psm.playstation.net/submission/get_metadata";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
				byte[] cacert = Resources.cacert;
				X509Certificate value = new X509Certificate(cacert);
				httpWebRequest.ClientCertificates.Add(value);
				ServicePointManager.ServerCertificateValidationCallback = OnRemoteCertificateValidationCallback;
				string text2 = Environment.TickCount.ToString();
				string s = "--" + text2 + "\r\nContent-Disposition: form-data; name=\"ticket\"; filename=\"ticket.bin\"\r\nContent-Type: application/octet-stream\r\n\r\n";
				string s2 = "\r\n--" + text2 + "\r\nContent-Disposition: form-data; name=\"project_name\"; filename=\"project_name.xml\"\r\nContent-Type: text/xml\r\n\r\n";
				string s3 = "\r\n--" + text2 + "--\r\n";
				UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				byte[] bytes = uTF8Encoding.GetBytes(s);
				byte[] bytes2 = uTF8Encoding.GetBytes(s2);
				byte[] bytes3 = uTF8Encoding.GetBytes(s3);
				byte[] array = mTicket;
				string text3 = null;
				byte[] bytes4;
				if (mUseXml)
				{
					text3 = mXmlParam;
					bytes4 = uTF8Encoding.GetBytes(text3);
				}
				else
				{
					string s4 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n" + $"<application project_name=\"{mXmlParam}\"/>\r\n";
					bytes4 = uTF8Encoding.GetBytes(s4);
				}
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "multipart/form-data; boundary=" + text2;
				httpWebRequest.ContentLength = bytes.Length + array.Length + (bytes2.Length + bytes4.Length) + bytes3.Length;
				httpWebRequest.Headers.Add("X-PSM-Version", "1.0");
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Write(array, 0, array.Length);
				requestStream.Write(bytes2, 0, bytes2.Length);
				requestStream.Write(bytes4, 0, bytes4.Length);
				requestStream.Write(bytes3, 0, bytes3.Length);
				requestStream.Close();
				backgroundWorker.ReportProgress(1);
				WebResponse response = httpWebRequest.GetResponse();
				if (response.ResponseUri.ToString() != text)
				{
					e.Result = "Response Uri = " + response.ResponseUri;
					return;
				}
				if (response.ContentType != "text/xml;charset=UTF-8")
				{
					e.Result = "Response content type = " + response.ContentType;
					return;
				}
				string text4 = response.Headers["X-PSM-Status"];
				string text5 = text4.Substring(0, 2);
				if (text5 != "OK")
				{
					e.Result = Submit.GetErrorStr("Get metadata error...\n", text4.Substring(4), mappXmlInfo);
					return;
				}
				Stream responseStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, uTF8Encoding);
				string s5 = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				StringReader stringReader = new StringReader(s5);
				XmlReader xmlReader = XmlReader.Create(stringReader);
				AppXmlInfo info = default(AppXmlInfo);
				Utility.AnalyzeAppXml(xmlReader, ref info);
				developerName = info.developerName;
				if (!mUseXml)
				{
					stringReader.Close();
					xmlReader.Close();
					return;
				}
				updateBlocked = false;
				if (info.appVersion == null || info.runtimeVersion == null)
				{
					actionInformationText = string.Format(Resources.firstSubmitText, info.projectName);
				}
				else
				{
					string information = "";
					if (CheckUpdateBlocked(info, mappXmlInfo, ref information))
					{
						actionInformationText = information;
						updateBlocked = true;
					}
					else
					{
						actionInformationText = string.Format(Resources.updateSubmitText, info.projectName, info.appVersion, mappXmlInfo.appVersion);
						changedUnmodificationText = CheckUnmodifiableInfo(info, mappXmlInfo);
					}
				}
				stringReader.Close();
				xmlReader.Close();
			}
			catch (Exception ex)
			{
				e.Result = ex.ToString();
			}
		}

		private bool CheckUpdateBlocked(AppXmlInfo oldInfo, AppXmlInfo newInfo, ref string information)
		{
			string text = (string.IsNullOrEmpty(oldInfo.appXmlFormat.sdkType) ? "PSM SDK" : oldInfo.appXmlFormat.sdkType);
			if (newInfo.appXmlFormat.sdkType.CompareTo(text) != 0)
			{
				if (newInfo.appXmlFormat.sdkType.CompareTo("PSM SDK") == 0)
				{
					information = string.Format(Resources.updateBlockedSubmitText_UnityToPsmSdk, oldInfo.projectName, text, newInfo.appXmlFormat.sdkType, "Unity for PSM");
				}
				else if (newInfo.appXmlFormat.sdkType.CompareTo("Unity for PSM") == 0)
				{
					information = string.Format(Resources.updateBlockedSubmitText_PsmSdkToUnity, oldInfo.projectName, text, newInfo.appXmlFormat.sdkType, "PSM SDK");
				}
				else
				{
					string arg = string.Format("提出する {0} が不一致 ( \"{1}\" → \"{2}\" )", "SDK タイプ", text, newInfo.appXmlFormat.sdkType);
					information = string.Format(Resources.updateBlockedSubmitText, oldInfo.projectName, arg);
				}
				return true;
			}
			if (oldInfo.sdkVersion.IndexOf("1.") == 0 && newInfo.sdkVersion.IndexOf("2.") == 0)
			{
				information = string.Format(Resources.updateBlockedSubmitText_1xTo2x, oldInfo.projectName, oldInfo.sdkVersion, newInfo.sdkVersion, "1.2X");
				return true;
			}
			if (oldInfo.sdkVersion.IndexOf("2.") == 0 && newInfo.sdkVersion.IndexOf("1.") == 0)
			{
				information = string.Format(Resources.updateBlockedSubmitText_2xTo1x, oldInfo.projectName, oldInfo.sdkVersion, newInfo.sdkVersion, "2.X");
				return true;
			}
			return false;
		}

		private string CheckUnmodifiableInfo(AppXmlInfo oldInfo, AppXmlInfo newInfo)
		{
			string text = Resources.changedUnmodificationBody_Text;
			bool flag = true;
			if (oldInfo.developerName != newInfo.developerName)
			{
				string text2 = text;
				text = text2 + "Developer Name : \"" + oldInfo.developerName + "\" -> \"" + newInfo.developerName + "\"\r\n";
				flag = false;
			}
			if (oldInfo.projectName != newInfo.projectName)
			{
				string text3 = text;
				text = text3 + "Project Name : \"" + oldInfo.projectName + "\" -> \"" + newInfo.projectName + "\"\r\n";
				flag = false;
			}
			foreach (string key in oldInfo.names.Keys)
			{
				if (newInfo.names.ContainsKey(key) && newInfo.names[key].value != oldInfo.names[key].value)
				{
					string text4 = text;
					text = text4 + "Application Name (" + key + ") : \"" + oldInfo.names[key].value + "\" -> \"" + newInfo.names[key].value + "\"\r\n";
					flag = false;
				}
			}
			foreach (string key2 in oldInfo.shortNames.Keys)
			{
				if (newInfo.shortNames.ContainsKey(key2) && newInfo.shortNames[key2].value != oldInfo.shortNames[key2].value)
				{
					string text5 = text;
					text = text5 + "Application Short Name(" + key2 + ") : \"" + oldInfo.shortNames[key2].value + "\" -> \"" + newInfo.shortNames[key2].value + "\"\r\n";
					flag = false;
				}
			}
			if (oldInfo.lockLevel != newInfo.lockLevel)
			{
				string text6 = text;
				text = text6 + "Parental Lock Lebel : \"" + oldInfo.lockLevel + "\" -> \"" + newInfo.lockLevel + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.highestAgeLimit != newInfo.ratingList.highestAgeLimit)
			{
				string text7 = text;
				text = text7 + "Highest Age Limit : \"" + oldInfo.ratingList.highestAgeLimit + "\" -> \"" + newInfo.ratingList.highestAgeLimit + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.hasOnlineFeatures != newInfo.ratingList.hasOnlineFeatures)
			{
				string text8 = text;
				text = text8 + "Has Online Features : \"" + oldInfo.ratingList.hasOnlineFeatures + "\" -> \"" + newInfo.ratingList.hasOnlineFeatures + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.onlineFeatures.chat != newInfo.ratingList.onlineFeatures.chat)
			{
				string text9 = text;
				text = text9 + "Online Features(chat) : \"" + oldInfo.ratingList.onlineFeatures.chat + "\" -> \"" + newInfo.ratingList.onlineFeatures.chat + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.onlineFeatures.personalInfo != newInfo.ratingList.onlineFeatures.personalInfo)
			{
				string text10 = text;
				text = text10 + "Online Features(Personal Info) : \"" + oldInfo.ratingList.onlineFeatures.personalInfo + "\" -> \"" + newInfo.ratingList.onlineFeatures.personalInfo + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.onlineFeatures.userLocation != newInfo.ratingList.onlineFeatures.userLocation)
			{
				string text11 = text;
				text = text11 + "Online Features(User Location) : \"" + oldInfo.ratingList.onlineFeatures.userLocation + "\" -> \"" + newInfo.ratingList.onlineFeatures.userLocation + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.onlineFeatures.exchangeContent != newInfo.ratingList.onlineFeatures.exchangeContent)
			{
				string text12 = text;
				text = text12 + "Online Features(Exchange Content) : \"" + oldInfo.ratingList.onlineFeatures.exchangeContent + "\" -> \"" + newInfo.ratingList.onlineFeatures.exchangeContent + "\"\r\n";
				flag = false;
			}
			if (oldInfo.ratingList.onlineFeatures.mininumAge != newInfo.ratingList.onlineFeatures.mininumAge)
			{
				string text13 = text;
				text = text13 + "Online Features(Mininum Age) : \"" + oldInfo.ratingList.onlineFeatures.mininumAge + "\" -> \"" + newInfo.ratingList.onlineFeatures.mininumAge + "\"\r\n";
				flag = false;
			}
			if (oldInfo.esrb.type != newInfo.esrb.type)
			{
				string text14 = text;
				text = text14 + "Rating(ESRB) Type : \"" + oldInfo.esrb.type + "\" -> \"" + newInfo.esrb.type + "\"\r\n";
				flag = false;
			}
			if (oldInfo.esrb.value != newInfo.esrb.value)
			{
				string text15 = text;
				text = text15 + "Rating(ESRB) Value : \"" + oldInfo.esrb.value + "\" -> \"" + newInfo.esrb.value + "\"\r\n";
				flag = false;
			}
			if (oldInfo.esrb.age != newInfo.esrb.age)
			{
				string text16 = text;
				text = text16 + "Rating(ESRB) Age : \"" + oldInfo.esrb.age + "\" -> \"" + newInfo.esrb.age + "\"\r\n";
				flag = false;
			}
			if (oldInfo.esrb.code != newInfo.esrb.code)
			{
				string text17 = text;
				text = text17 + "Rating(ESRB) Code : \"" + oldInfo.esrb.code + "\" -> \"" + newInfo.esrb.code + "\"\r\n";
				flag = false;
			}
			if (oldInfo.pegiex.type != newInfo.pegiex.type)
			{
				string text18 = text;
				text = text18 + "Rating(PEGIEX) Type : \"" + oldInfo.pegiex.type + "\" -> \"" + newInfo.pegiex.type + "\"\r\n";
				flag = false;
			}
			if (oldInfo.pegiex.value != newInfo.pegiex.value)
			{
				string text19 = text;
				text = text19 + "Rating(PEGIEX) Value : \"" + oldInfo.pegiex.value + "\" -> \"" + newInfo.pegiex.value + "\"\r\n";
				flag = false;
			}
			if (oldInfo.pegiex.age != newInfo.pegiex.age)
			{
				string text20 = text;
				text = text20 + "Rating(PEGIEX) Age : \"" + oldInfo.pegiex.age + "\" -> \"" + newInfo.pegiex.age + "\"\r\n";
				flag = false;
			}
			if (oldInfo.pegiex.code != newInfo.pegiex.code)
			{
				string text21 = text;
				text = text21 + "Rating(PEGIEX) Code : \"" + oldInfo.pegiex.code + "\" -> \"" + newInfo.pegiex.code + "\"\r\n";
				flag = false;
			}
			if (oldInfo.self.type != newInfo.self.type)
			{
				string text22 = text;
				text = text22 + "Rating(SELF) Type : \"" + oldInfo.self.type + "\" -> \"" + newInfo.self.type + "\"\r\n";
				flag = false;
			}
			if (oldInfo.self.value != newInfo.self.value)
			{
				string text23 = text;
				text = text23 + "Rating(SELF) Value : \"" + oldInfo.self.value + "\" -> \"" + newInfo.self.value + "\"\r\n";
				flag = false;
			}
			if (oldInfo.self.age != newInfo.self.age)
			{
				string text24 = text;
				text = text24 + "Rating(SELF) Age : \"" + oldInfo.self.age + "\" -> \"" + newInfo.self.age + "\"\r\n";
				flag = false;
			}
			if (oldInfo.self.code != newInfo.self.code)
			{
				string text25 = text;
				text = text25 + "Rating(SELF) Code : \"" + oldInfo.self.code + "\" -> \"" + newInfo.self.code + "\"\r\n";
				flag = false;
			}
			if (oldInfo.primaryGenre != newInfo.primaryGenre)
			{
				string text26 = text;
				text = text26 + "Primary Genre : \"" + oldInfo.primaryGenre + "\" -> \"" + newInfo.primaryGenre + "\"\r\n";
				flag = false;
			}
			foreach (string key3 in oldInfo.products.Keys)
			{
				AppXmlInfo.Product product = oldInfo.products[key3];
				if (!newInfo.products.ContainsKey(key3))
				{
					continue;
				}
				AppXmlInfo.Product product2 = newInfo.products[key3];
				if (product2.type == product.type)
				{
					foreach (string key4 in product.names.Keys)
					{
						if (product2.names.ContainsKey(key4) && product2.names[key4].value != product.names[key4].value)
						{
							string text2 = text;
							text = text2 + "Product Label(" + key4 + ") : \"" + product.names[key4].value + "\" -> \"" + product2.names[key4].value + "\"\r\n";
							flag = false;
						}
					}
				}
				else
				{
					string text2 = text;
					text = text2 + "Product Type(" + key3 + ") : \"" + product.type + "\" -> \"" + product2.type + "\"\r\n";
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return text;
		}

		private bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}
