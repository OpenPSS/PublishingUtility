using System;
using System.ComponentModel;

namespace PublishingUtility
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
	public class SRDescriptionAttribute : DescriptionAttribute
	{
		private bool _localized;

		public override string Description
		{
			get
			{
				if (!_localized)
				{
					_localized = true;
					base.DescriptionValue = SR.GetString(base.DescriptionValue);
				}
				return base.Description;
			}
		}

		public SRDescriptionAttribute(string text)
			: base(text)
		{
			_localized = false;
		}
	}
}
