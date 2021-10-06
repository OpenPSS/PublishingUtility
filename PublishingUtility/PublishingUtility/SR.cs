using System.Globalization;
using System.Resources;

namespace PublishingUtility
{
	public sealed class SR
	{
		private ResourceManager _rm;

		private static SR _loader = null;

		private static object _lock = new object();

		private ResourceManager Resources => _rm;

		private SR()
		{
			_rm = new ResourceManager("PublishingUtility.Properties.Resources", GetType().Assembly);
		}

		private static SR GetLoader()
		{
			if (_loader == null)
			{
				lock (_lock)
				{
					if (_loader == null)
					{
						_loader = new SR();
					}
				}
			}
			return _loader;
		}

		public static string GetString(string name)
		{
			SR loader = GetLoader();
			string result = null;
			if (loader != null)
			{
				result = loader.Resources.GetString(name, null);
			}
			return result;
		}

		public static string GetString(CultureInfo culture, string name)
		{
			SR loader = GetLoader();
			string result = null;
			if (loader != null)
			{
				result = loader.Resources.GetString(name, culture);
			}
			return result;
		}
	}
}
