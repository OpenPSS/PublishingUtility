using System;
using System.Collections.Generic;

namespace PublishingUtility
{
	public class MaxScreenSizeDisplayNameConverter : EnumDisplayNameConverter
	{
		public MaxScreenSizeDisplayNameConverter(Type type)
			: base(type)
		{
		}

		protected override void BuildTable(Dictionary<object, string> table)
		{
			table.Add(MaxScreenSize._1280x800, "1280x800");
			table.Add(MaxScreenSize._1920x1200, "1920x1200");
		}
	}
}
