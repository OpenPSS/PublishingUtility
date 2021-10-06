using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PublishingUtility
{
	public static class Package
	{
		public static int CreatePackage(string srcDir, string dstPsk)
		{
			int num = PsmDeviceFuncBinding.CreatePackage(dstPsk, srcDir);
			if (num < 0)
			{
				Console.WriteLine($"Failed to create package: error {num}");
				Environment.Exit(1);
			}
			return 0;
		}

		public static int SignPackage(string srcPkg, string dstPkg, string titleId)
		{
			string path = Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12";
			byte[] array = File.ReadAllBytes(path);
			if (dstPkg != null && File.Exists(dstPkg))
			{
				File.Delete(dstPkg);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			SceSagErrorCode sceSagErrorCode = ((IntPtr.Size == 8) ? SubmissionArchiveGenerator64.Generate(intPtr, array.Length, titleId, srcPkg, dstPkg) : SubmissionArchiveGenerator32.Generate(intPtr, array.Length, titleId, srcPkg, dstPkg));
			if (sceSagErrorCode != 0)
			{
				Console.WriteLine($"Submission Archive Generator returns \"{sceSagErrorCode.ToString()}\"");
				MessageBox.Show("ERROR: SubmissionSign()\n" + $"Submission Archive Generator returns \"{sceSagErrorCode.ToString()}\"" + ".", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(1);
				return -1;
			}
			return 0;
		}

		public static string ReadTitleIdentifierFromPackage(string package)
		{
			string tempFileName = Path.GetTempFileName();
			int num = PsmDeviceFuncBinding.PickFileFromPackage(tempFileName, package, "Metadata/app.xml");
			if (num < 0)
			{
				return "";
			}
			XDocument xDocument = Xml.LoadFromFile(tempFileName);
			Metadata metadata = new Metadata();
			metadata.SelfFilePath = tempFileName;
			metadata.LateValidation = true;
			Metadata metadata2 = metadata;
			XElement root = xDocument.Element("application");
			Xml.ReadFromXml(metadata2, root);
			string projectName = metadata2.ProjectName;
			File.Delete(tempFileName);
			return projectName;
		}
	}
}
