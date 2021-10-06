using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using PublishingUtility.Dialog;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class SubmitCheckMeta
	{
		private byte[] mTicket;

		private AppXmlInfo mappXmlInfo;

		private string mXmlFile;

		public DialogResult Exec(byte[] Ticket, string XmlFile, AppXmlInfo appXmlInfo, string Text, ref string Result)
		{
			DialogResult dialogResult = DialogResult.None;
			mTicket = Ticket;
			mappXmlInfo = appXmlInfo;
			mXmlFile = XmlFile;
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
				string text = $"https://sdk.{Program.appConfigData.EnvServer}.psm.playstation.net/submission/check_metadata";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
				byte[] cacert = Resources.cacert;
				X509Certificate value = new X509Certificate(cacert);
				httpWebRequest.ClientCertificates.Add(value);
				ServicePointManager.ServerCertificateValidationCallback = OnRemoteCertificateValidationCallback;
				string text2 = Environment.TickCount.ToString();
				string s = "--" + text2 + "\r\nContent-Disposition: form-data; name=\"ticket\"; filename=\"ticket.bin\"\r\nContent-Type: application/octet-stream\r\n\r\n";
				string s2 = "\r\n--" + text2 + "\r\nContent-Disposition: form-data; name=\"meta\"; filename=\"app.xml\"\r\nContent-Type: text/xml\r\n\r\n";
				string s3 = "\r\n--" + text2 + "--\r\n";
				UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				byte[] bytes = uTF8Encoding.GetBytes(s);
				byte[] bytes2 = uTF8Encoding.GetBytes(s2);
				byte[] bytes3 = uTF8Encoding.GetBytes(s3);
				byte[] array = mTicket;
				byte[] bytes4 = uTF8Encoding.GetBytes(mXmlFile);
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
				string text3 = response.Headers["X-PSM-Status"];
				string text4 = text3.Substring(0, 2);
				if (text4 != "OK")
				{
					e.Result = Submit.GetErrorStr("Check metadata error...\n", text3.Substring(4), mappXmlInfo);
					return;
				}
				Stream responseStream = response.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, uTF8Encoding);
				streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
			}
			catch (Exception ex)
			{
				e.Result = ex.ToString();
			}
		}

		private bool OnRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}
	}
}
