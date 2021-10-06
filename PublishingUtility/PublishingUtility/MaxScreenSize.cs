using System.ComponentModel;

namespace PublishingUtility
{
	[TypeConverter(typeof(MaxScreenSizeDisplayNameConverter))]
	public enum MaxScreenSize
	{
		_1280x800,
		_1920x1200
	}
}
