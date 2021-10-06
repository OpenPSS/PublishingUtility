using System.ComponentModel;
using Controls;

namespace TypeConverters
{
	public class SelectedPanelConverter : ReferenceConverter
	{
		public SelectedPanelConverter()
			: base(typeof(ManagedPanel))
		{
		}

		protected override bool IsValueAllowed(ITypeDescriptorContext context, object value)
		{
			if (context != null)
			{
				PanelManager panelManager = (PanelManager)context.Instance;
				return panelManager.ManagedPanels.Contains((ManagedPanel)value);
			}
			return false;
		}
	}
}
