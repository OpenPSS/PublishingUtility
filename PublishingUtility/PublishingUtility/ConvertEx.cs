using System.Globalization;

namespace PublishingUtility
{
	public class ConvertEx
	{
		public static int? ToInt32(string strSrc)
		{
			if (!int.TryParse(strSrc, NumberStyles.Any, NumberFormatInfo.CurrentInfo, out var result))
			{
				return null;
			}
			return result;
		}
	}
}
