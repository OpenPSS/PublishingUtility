using System;
using System.Globalization;

namespace PublishingUtility
{
	[AttributeUsage(AttributeTargets.All)]
	public class EnumDisplayNameAttribute : Attribute
	{
		private string name;

		public string Name => GetName(CultureInfo.CurrentCulture);

		public EnumDisplayNameAttribute(string name)
		{
			this.name = name;
		}

		public virtual string GetName(CultureInfo culture)
		{
			return name;
		}
	}
}
