using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public class DeviceStatus
	{
		private const int MAGIC_NUMBER = 1145394000;

		public DeviceType deviceType;

		public Guid DeviceGUID;

		public string DeviceID = "no_name";

		public string strAppExeEpiration = "-";

		private string nickname = "no_nickname";

		public bool IsCorrect = true;

		public bool HasSeed;

		public AppExeKeyStatus appExeKeyStatus;

		public bool IsUsbConnected;

		public bool IsDevAssistantOn;

		private int magicNumber;

		private int versionNumber;

		private byte[] byteDeviceID = new byte[32];

		private byte[] byteDeviceNickname = new byte[32];

		private byte[] byteC1 = new byte[1024];

		private byte[] byteOpenConsoleID = new byte[128];

		public string DeviceTypeID => string.Concat(deviceType, ":", DeviceID);

		public string Name2 => deviceType.ToString() + ":" + Nickname;

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
				for (int i = 0; i < byteDeviceNickname.Length; i++)
				{
					byteDeviceNickname[i] = 0;
				}
				Buffer.BlockCopy(bytes, 0, byteDeviceNickname, 0, bytes.Length);
			}
		}

		public DeviceSeedStatus deviceSeedStatus
		{
			get
			{
				if (!HasSeed)
				{
					return DeviceSeedStatus.None;
				}
				return DeviceSeedStatus.Seed;
			}
		}

		public byte[] C1 => byteC1;

		public byte[] OpenConsoleID => byteOpenConsoleID;

		public DeviceStatus()
		{
		}

		public DeviceStatus(ScePsmDevice scePsmDevice)
		{
			deviceType = (DeviceType)scePsmDevice.type;
			DeviceGUID = scePsmDevice.guid;
			DeviceID = Utility.SafeConvertCharArrayToString(scePsmDevice.deviceID);
			Nickname = DeviceID;
			IsUsbConnected = true;
			IsDevAssistantOn = ((scePsmDevice.online != 0) ? true : false);
			HasSeed = false;
			appExeKeyStatus = AppExeKeyStatus.None;
		}

		public DeviceStatus(string pathFile)
		{
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
				MessageBox.Show(ex.ToString(), "Publishing Utility - DeviceStatus()", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				IsCorrect = false;
				return;
			}
			if (array.Length != 1228)
			{
				DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage($"File size of {pathFile} is invalid.\nfile size={array.Length}\n\nDo you want to delete this file?", $"{pathFile} はファイルサイズが不正です。\nファイルサイズ={array.Length}\n\nこのファイルを削除しますか?"), "Publishing Utility - Load Device Seed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					File.Delete(pathFile);
				}
				IsCorrect = false;
				return;
			}
			magicNumber = BitConverter.ToInt32(array, 0);
			if (magicNumber != 1145394000)
			{
				DialogResult dialogResult2 = MessageBox.Show(Utility.TextLanguage(pathFile + " is not seed file.\n\nDo you want to delete this file?", pathFile + " は不正なデバイスシードです。.\n\nこのファイルを削除しますか？"), "Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult2 == DialogResult.Yes)
				{
					File.Delete(pathFile);
				}
				IsCorrect = false;
			}
			else
			{
				versionNumber = BitConverter.ToInt32(array, 4);
				deviceType = (DeviceType)BitConverter.ToInt32(array, 8);
				Buffer.BlockCopy(array, 12, byteDeviceID, 0, 32);
				DeviceID = Utility.ByteArrayToString(byteDeviceID);
				Buffer.BlockCopy(array, 44, byteDeviceNickname, 0, 32);
				Nickname = Utility.ByteArrayToString(byteDeviceNickname);
				Buffer.BlockCopy(array, 76, byteC1, 0, 1024);
				Buffer.BlockCopy(array, 1100, byteOpenConsoleID, 0, 128);
				HasSeed = true;
			}
		}

		public void Save(string pathFile)
		{
			FileStream fileStream = new FileStream(pathFile, FileMode.Create, FileAccess.Write);
			fileStream.Write(BitConverter.GetBytes(magicNumber), 0, 4);
			fileStream.Write(BitConverter.GetBytes(versionNumber), 0, 4);
			int value = (int)deviceType;
			fileStream.Write(BitConverter.GetBytes(value), 0, 4);
			fileStream.Write(byteDeviceID, 0, byteDeviceID.Length);
			fileStream.Write(byteDeviceNickname, 0, byteDeviceNickname.Length);
			fileStream.Write(byteC1, 0, byteC1.Length);
			fileStream.Write(byteOpenConsoleID, 0, byteOpenConsoleID.Length);
			fileStream.Close();
		}

		public void SaveC1(string pathFile)
		{
			string path = Path.GetDirectoryName(pathFile) + "\\";
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathFile);
			Utility.SaveFileByteArray(path, fileNameWithoutExtension + ".c1", byteC1);
		}

		public static bool IsDeviceNicknameValid(string nickname)
		{
			if (nickname.Length > 31)
			{
				MessageBox.Show(string.Format(Resources.enterWithinX_Text, 31), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			Regex regex = new Regex("^[a-zA-Z0-9_-]+$");
			if (!regex.IsMatch(nickname))
			{
				MessageBox.Show(Resources.onlyUseXCharactor_Text, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			return true;
		}

		public void Dump()
		{
			Console.WriteLine("{0},{1},{2},{3}", deviceType, DeviceGUID, DeviceID, Nickname, HasSeed, appExeKeyStatus);
		}
	}
}
