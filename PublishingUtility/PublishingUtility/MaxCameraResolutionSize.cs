using System.ComponentModel;

namespace PublishingUtility
{
	[TypeConverter(typeof(MaxCameraResolutionSizeDisplayNameConverter))]
	public enum MaxCameraResolutionSize
	{
		_800x600,
		_2048x1536
	}
}
