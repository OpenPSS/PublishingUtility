using System;
using System.ComponentModel.Design;
using Controls;

namespace Editors
{
	public class ManagedPanelCollectionEditor : CollectionEditor
	{
		public ManagedPanelCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override Type CreateCollectionItemType()
		{
			return typeof(ManagedPanel);
		}
	}
}
