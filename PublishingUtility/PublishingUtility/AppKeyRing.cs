using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PublishingUtility
{
	public class AppKeyRing
	{
		private const int MAGIG_NUMBER = 1380663632;

		private int magicNumberTmp;

		private int versionNumber;

		private string AppID;

		private int numOfAppExeKey;

		private byte[] byteAppDevKey;

		private List<byte[]> listAppExeKey = new List<byte[]>();

		public bool LoadFile(string pathFile)
		{
			byte[] array = Utility.LoadFileAsByteArray(pathFile);
			if (array == null)
			{
				throw new Exception("Can't open " + pathFile);
			}
			magicNumberTmp = BitConverter.ToInt32(array, 0);
			if (magicNumberTmp != 1380663632)
			{
				MessageBox.Show(Utility.TextLanguage(pathFile + " is not App Key Ring file.", pathFile + " はアプリ鍵束ではありません。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			byte[] array2 = new byte[32];
			Buffer.BlockCopy(array, 8, array2, 0, 32);
			AppID = Utility.ByteArrayToString(array2);
			byteAppDevKey = new byte[1024];
			Buffer.BlockCopy(array, 72, byteAppDevKey, 0, 1024);
			numOfAppExeKey = BitConverter.ToInt32(array, 1096);
			for (int i = 0; i < numOfAppExeKey; i++)
			{
				byte[] array3 = new byte[1784];
				Buffer.BlockCopy(array, 1100 + 1784 * i, array3, 0, 1784);
				listAppExeKey.Add(array3);
			}
			return true;
		}

		public void GenerateFileofAppDevExeKey()
		{
			string path = Utility.UserAppDataPath + "\\HostAppsKey\\";
			Utility.SaveFileByteArray(path, AppID + ".khapp", byteAppDevKey);
			string path2 = Utility.UserAppDataPath + "\\TargetAppsKey\\" + AppID + "\\";
			foreach (byte[] item in listAppExeKey)
			{
				string nicknameFromByteArray = AppExeKey.GetNicknameFromByteArray(item);
				Utility.SaveFileByteArray(path2, nicknameFromByteArray + ".ktapp", item);
			}
		}

		public bool GenerateKeyRing(string appID, out List<string> appExeKeyList)
		{
			appExeKeyList = new List<string>();
			versionNumber = 1;
			string text = ((!(appID == "*")) ? appID : "+asterisk+");
			string path = Program.PATH_DIR_TARGET_APPS_KEY + "\\" + text + "\\" + text + ".krng";
			string text2 = Program.PATH_DIR_HOST_APPS_KEY + "\\" + text + ".khapp";
			if (!File.Exists(text2))
			{
				MessageBox.Show($"Cannot find {text2}.", "Publishing Utility - App Key Ring", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			string text3 = Program.PATH_DIR_TARGET_APPS_KEY + "\\" + text + "\\";
			if (!Directory.Exists(text3))
			{
				MessageBox.Show($"Cannot find {text3}.", "Publishing Utility - App Key Ring", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			int num = 0;
			num = Directory.GetFiles(text3, "*.ktapp").Length;
			if (num == 0)
			{
				MessageBox.Show($"Cannot find {text}'s App Exe Key(*.ktapp).", "Publishing Utility - App Key Ring", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			fileStream.Write(BitConverter.GetBytes(1380663632), 0, 4);
			fileStream.Write(BitConverter.GetBytes(versionNumber), 0, 4);
			byte[] array = new byte[32];
			byte[] array2 = new byte[32];
			Encoding encoding = Encoding.GetEncoding("Shift_JIS");
			byte[] bytes = encoding.GetBytes(text);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
			fileStream.Write(array, 0, array.Length);
			fileStream.Write(array2, 0, array2.Length);
			bytes = Utility.LoadFileAsByteArray(text2);
			if (bytes == null)
			{
				MessageBox.Show($"Failed to load {text2}.", "Publishing Utility - App Key Ring", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				fileStream.Close();
				return false;
			}
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Write(BitConverter.GetBytes(num), 0, 4);
			string[] files = Directory.GetFiles(text3, "*.ktapp");
			foreach (string text4 in files)
			{
				bytes = Utility.LoadFileAsByteArray(text4);
				if (bytes == null)
				{
					MessageBox.Show($"Failed to load {text4}.", "Publishing Utility - App Key Ring", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					fileStream.Close();
					return false;
				}
				appExeKeyList.Add(Path.GetFileNameWithoutExtension(text4));
				fileStream.Write(bytes, 0, bytes.Length);
			}
			fileStream.Close();
			Console.WriteLine("Succeed to save {0} key ring.", text);
			return true;
		}
	}
}
