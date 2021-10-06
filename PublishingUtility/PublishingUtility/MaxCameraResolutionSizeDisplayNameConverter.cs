using System;
using System.Collections.Generic;

namespace PublishingUtility
{
	public class MaxCameraResolutionSizeDisplayNameConverter : EnumDisplayNameConverter
	{
		public MaxCameraResolutionSizeDisplayNameConverter(Type type)
			: base(type)
		{
		}

		protected override void BuildTable(Dictionary<object, string> table)
		{
			table.Add(MaxCameraResolutionSize._800x600, "800x600");
			table.Add(MaxCameraResolutionSize._2048x1536, "2048x1536");
		}
	}
}
