using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public class AppExeKey
	{
		private const int FILE_SIZE = 1784;

		private const int MAGIC_NUMBER = 1414218064;

		private int magicNumber;

		private int versionNumber;

		private byte[] byteNickname = new byte[32];

		private byte[] byteEncTargetKdb = new byte[1072];

		private byte[] byteR1 = new byte[512];

		private byte[] byteOpenConsoleID = new byte[128];

		private byte[] byteExpiration = new byte[32];

		private string nickname = "no_nickname";

		public string DeviceID = "";

		public DateTime LastWriteTime;

		public string Nickname
		{
			get
			{
				return nickname;
			}
			set
			{
				nickname = value;
				Encoding encoding = Encoding.GetEncoding("Shift_JIS");
				byte[] bytes = encoding.GetBytes(nickname);
				for (int i = 0; i < byteNickname.Length; i++)
				{
					byteNickname[i] = 0;
				}
				Buffer.BlockCopy(bytes, 0, byteNickname, 0, bytes.Length);
			}
		}

		public byte[] EncTargetKdb
		{
			get
			{
				return byteEncTargetKdb;
			}
			set
			{
				Buffer.BlockCopy(value, 0, byteEncTargetKdb, 0, byteEncTargetKdb.Length);
			}
		}

		public byte[] R1
		{
			get
			{
				return byteR1;
			}
			set
			{
				Buffer.BlockCopy(value, 0, byteR1, 0, byteR1.Length);
			}
		}

		public byte[] OpenConsoleID
		{
			get
			{
				return byteOpenConsoleID;
			}
			set
			{
				Buffer.BlockCopy(value, 0, byteOpenConsoleID, 0, byteOpenConsoleID.Length);
			}
		}

		public byte[] Expiration
		{
			get
			{
				return byteExpiration;
			}
			set
			{
				Buffer.BlockCopy(value, 0, byteExpiration, 0, byteExpiration.Length);
			}
		}

		public DateTime GetExpirationDateTime()
		{
			return DateTime.Parse(GetExpirationString());
		}

		public string GetExpirationString()
		{
			return Utility.ByteArrayToString(byteExpiration);
		}

		public void Save(string pathFile)
		{
			if (!Directory.Exists(Path.GetDirectoryName(pathFile)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(pathFile));
			}
			magicNumber = 1414218064;
			versionNumber = 1;
			FileStream fileStream = new FileStream(pathFile, FileMode.Create, FileAccess.Write);
			fileStream.Write(BitConverter.GetBytes(magicNumber), 0, 4);
			fileStream.Write(BitConverter.GetBytes(versionNumber), 0, 4);
			fileStream.Write(byteNickname, 0, byteNickname.Length);
			fileStream.Write(byteExpiration, 0, byteExpiration.Length);
			fileStream.Write(byteEncTargetKdb, 0, byteEncTargetKdb.Length);
			fileStream.Write(byteR1, 0, byteR1.Length);
			fileStream.Write(byteOpenConsoleID, 0, byteOpenConsoleID.Length);
			fileStream.Close();
		}

		public bool Load(string pathFile)
		{
			DeviceID = Path.GetFileNameWithoutExtension(pathFile);
			LastWriteTime = File.GetLastWriteTime(pathFile);
			if (!File.Exists(pathFile))
			{
				MessageBox.Show($"Can't find {pathFile}.", "Publishing Utility");
				return false;
			}
			byte[] array;
			try
			{
				FileStream fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			if (array.Length != 1784)
			{
				MessageBox.Show(Utility.TextLanguage($"{pathFile} size is invalid.\nfile size={array.Length}. correct size = {1784}.", $"{pathFile} のファイルサイズが不正です。.\nファイルサイズ={array.Length}. 正しいファイルサイズ = {1784}."), "Publishing Utility - Load App Key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			magicNumber = BitConverter.ToInt32(array, 0);
			if (magicNumber != 1414218064)
			{
				MessageBox.Show(Utility.TextLanguage(pathFile + " is not App Key file.", pathFile + " はアプリ鍵ではありません。"), "Publishing Utility - Load App Key", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			versionNumber = BitConverter.ToInt32(array, 4);
			Buffer.BlockCopy(array, 8, byteNickname, 0, 32);
			Nickname = Utility.ByteArrayToString(byteNickname);
			Buffer.BlockCopy(array, 40, byteExpiration, 0, 32);
			Buffer.BlockCopy(array, 72, byteEncTargetKdb, 0, 1072);
			Buffer.BlockCopy(array, 1144, byteR1, 0, 512);
			Buffer.BlockCopy(array, 1656, byteOpenConsoleID, 0, 128);
			return true;
		}

		public AppExeKeyStatus GetValidationStatus()
		{
			if (GetExpirationDateTime() < DateTime.Now)
			{
				return AppExeKeyStatus.Expired;
			}
			if (DeviceSeedUtility.ExistDeviceSeed(DeviceID) && !CompareDeviceSeedByTimeStamp())
			{
				return AppExeKeyStatus.OlderThanDeviceSeed;
			}
			if (PublisherKeyUtility.ExistPublisherKey() && !ComparePublisherKeyByTimeStamp())
			{
				return AppExeKeyStatus.OlderThanPublisherKey;
			}
			return AppExeKeyStatus.OK;
		}

		public bool CompareDeviceSeedByTimeStamp()
		{
			if (string.IsNullOrEmpty(DeviceID))
			{
				MessageBox.Show("LastWriteTime or DeviceID are not set.", "Publishing Utility CompareDeviceSeedByTimeStamp", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			if (!DeviceSeedUtility.ExistDeviceSeed(DeviceID))
			{
				return false;
			}
			if (LastWriteTime >= DeviceSeedUtility.GetTimeStamp(DeviceID))
			{
				return true;
			}
			return false;
		}

		public bool ComparePublisherKeyByTimeStamp()
		{
			if (LastWriteTime >= Program.appConfigData.dtCreateTimeOfPublisherKey)
			{
				return true;
			}
			return false;
		}

		public bool IsOneMonthBeforeExpiration()
		{
			if (GetExpirationDateTime() < DateTime.Now + new TimeSpan(30, 0, 0, 0))
			{
				return true;
			}
			return false;
		}

		public static string GetNicknameFromByteArray(byte[] byteArray)
		{
			byte[] array = new byte[32];
			Buffer.BlockCopy(byteArray, 8, array, 0, 32);
			string text = Utility.ByteArrayToString(array);
			if (string.IsNullOrWhiteSpace(text))
			{
				MessageBox.Show(Resources.NicknameIsNotNamed_Text, "Publishing Utility - GetNicknameFromByteArray", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return text;
		}
	}
}
