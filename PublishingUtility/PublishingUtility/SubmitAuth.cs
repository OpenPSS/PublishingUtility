using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using PublishingUtility.Dialog;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class SubmitAuth
	{
		private DateTime mNowDate;

		private bool mEntitlementFlag;

		private bool bMiniTicket;

		private string mLoginID;

		private string mPassword;

		private string mDeviceID;

		private byte[] mTicket;

		public byte[] TicketData => mTicket;

		public DialogResult Exec(string LoginID, string Password, string DeviceID, string Text, ref string Result)
		{
			DialogResult dialogResult = DialogResult.None;
			mLoginID = LoginID;
			mPassword = Password;
			mDeviceID = DeviceID;
			Progress progress = new Progress(Work, Text, 2);
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
				string text = $"https://auth.{Program.appConfigData.EnvServer}.ac.playstation.net/np/auth";
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
				string text2 = string.Format("loginid={0}&password={1}&deviceid={2}&serviceid={3}&serviceentity=urn:service-entity:np&type=0", HttpUtility.UrlEncode(mLoginID), HttpUtility.UrlEncode(mPassword), mDeviceID, "IP9100-NPIA00024_00");
				if (bMiniTicket)
				{
					text2 += "&miniticket=1";
				}
				UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
				byte[] bytes = uTF8Encoding.GetBytes(text2);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = bytes.Length;
				httpWebRequest.Headers.Add("X-NP-Version", "3.0");
				backgroundWorker.ReportProgress(1);
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				backgroundWorker.ReportProgress(2);
				WebResponse response = httpWebRequest.GetResponse();
				if (response.ResponseUri.ToString() != text)
				{
					e.Result = "Response Uri = " + response.ResponseUri;
					return;
				}
				if (response.ContentType != "application/x-np-ticket")
				{
					e.Result = "Response content type = " + response.ContentType;
					return;
				}
				string text3 = response.Headers["X-NP-Status"];
				string text4 = text3.Substring(0, 2);
				if (text4 != "OK")
				{
					e.Result = Resources.illegalAccountInformation_Text + " ( " + text3.Substring(4) + " )";
					return;
				}
				Stream responseStream = response.GetResponseStream();
				mTicket = new byte[response.ContentLength];
				responseStream.Read(mTicket, 0, (int)response.ContentLength);
				responseStream.Close();
				mNowDate = DateTime.Now;
				MemoryStream memoryStream = new MemoryStream(mTicket);
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				uint num = ToUint32Data(binaryReader);
				switch (num & 0x70000000)
				{
				}
				AnalyzeI5TicketV3(num, binaryReader);
				binaryReader.Close();
				memoryStream.Close();
				if (!mEntitlementFlag)
				{
					e.Result = Resources.submissionErrorText_2002or2300;
				}
			}
			catch (Exception ex)
			{
				e.Result = ex.ToString();
			}
		}

		private void AnalyzeI5TicketV3(uint header, BinaryReader reader)
		{
			uint num = ToUint32Data(reader);
			uint num2 = ToUint32Data(reader);
			int[] array = new int[11]
			{
				20, 4, 8, 8, 8, 32, 4, 4, 24, 4,
				4
			};
			uint num3 = 11u;
			uint num4 = 0u;
			for (int i = 0; i < num3; i++)
			{
				num4 = ToUint32Data(reader);
				reader.ReadBytes(array[i]);
			}
			num4 = ToUint32Data(reader);
			uint category = (num4 & 0xF0000000u) >> 28;
			uint tagId = (num4 & 0xFFF0000) >> 16;
			uint num5 = num4 & 0xFFFFu;
			if (!IsEntitlementList(category, tagId))
			{
				do
				{
					num4 = ToUint32Data(reader);
					category = (num4 & 0xF0000000u) >> 28;
					tagId = (num4 & 0xFFF0000) >> 16;
					num5 = num4 & 0xFFFFu;
					ToBytesData(reader, (int)num5);
				}
				while (!IsEntitlementList(category, tagId));
			}
			int num6 = (int)num5;
			DateTime dateTime = new DateTime(1970, 1, 1);
			do
			{
				byte b = ToByteData(reader);
				num6--;
				uint num7 = (byte)(b & 0x1Fu);
				bool flag = (b & 0x20) >> 5 != 0;
				if (1 <= num7 && num7 <= 8)
				{
					ToStringData(reader, (int)num7);
				}
				else if (9 <= num7 && num7 <= 31)
				{
					ToStringData(reader, (int)num7);
				}
				else
				{
					ToStringData(reader, (int)num7);
				}
				num6 -= (int)num7;
				ulong num8 = ToUint64Data(reader);
				num6 -= 8;
				DateTime dateTime2 = dateTime.AddMilliseconds(num8);
				if (flag)
				{
					num8 = ToUint64Data(reader);
					num6 -= 8;
					DateTime dateTime3 = dateTime.AddMilliseconds(num8);
					if (dateTime2 <= mNowDate && mNowDate <= dateTime3)
					{
						mEntitlementFlag = true;
						break;
					}
					continue;
				}
				mEntitlementFlag = true;
				break;
			}
			while (num6 > 0);
		}

		private bool IsEntitlementList(uint category, uint tagId)
		{
			if (category == 3)
			{
				return tagId == 16;
			}
			return false;
		}

		private byte ToByteData(BinaryReader reader)
		{
			return reader.ReadByte();
		}

		private byte[] ToBytesData(BinaryReader reader, int length)
		{
			return reader.ReadBytes(length);
		}

		private string ToStringData(BinaryReader reader, int length)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
			return uTF8Encoding.GetString(reader.ReadBytes(length));
		}

		private uint ToUint32Data(BinaryReader reader)
		{
			byte[] array = reader.ReadBytes(4);
			Array.Reverse(array);
			return BitConverter.ToUInt32(array, 0);
		}

		private ulong ToUint64Data(BinaryReader reader)
		{
			byte[] array = reader.ReadBytes(8);
			Array.Reverse(array);
			return BitConverter.ToUInt64(array, 0);
		}

		private double ToDoubleData(BinaryReader reader)
		{
			byte[] array = reader.ReadBytes(8);
			Array.Reverse(array);
			return BitConverter.ToDouble(array, 0);
		}
	}
}
