using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	internal class Submit
	{
		public const string PU_SUBMIT_DEF_TICKET_VER = "3.0";

		public const string PU_SUBMIT_DEF_SERVICE_ID = "IP9100-NPIA00024_00";

		private const string DLGCAP = "Publishing Utility";

		public void Information(string text)
		{
			Message(text, MessageBoxIcon.Asterisk);
		}

		public void Error(string text)
		{
			Message(text, MessageBoxIcon.Hand);
		}

		public void Message(string text)
		{
			Message(text, MessageBoxIcon.None);
		}

		public void Message(string text, MessageBoxIcon icon)
		{
			MessageBox.Show(text, "Publishing Utility", MessageBoxButtons.OK, icon);
		}

		public static string GetErrorStr(string def, string rsn, AppXmlInfo appXmlInfo)
		{
			string text = "";
			switch (rsn)
			{
			case "reason=2002":
			case "reason=2300":
				return Resources.submissionErrorText_2002or2300;
			case "reason=2301":
				return Resources.submissionErrorText_2301;
			case "reason=2401":
				return string.Format(Resources.submissionErrorText_2401, appXmlInfo.runtimeVersion);
			case "reason=2402":
				return Resources.submissionErrorText_2402;
			case "reason=2403":
				return Resources.submissionErrorText_2403;
			case "reason=2404":
				return Resources.submissionErrorText_2404;
			case "reason=2405":
				return Resources.submissionErrorText_2405;
			case "reason=2406":
				return Resources.submissionErrorText_2406;
			case "reason=2407":
				return string.Format(Resources.submissionErrorText_2407, appXmlInfo.sdkVersion);
			case "reason=3115":
				return string.Format(Resources.submissionErrorText_3115, "...");
			default:
				return def + rsn;
			}
		}
	}
}
