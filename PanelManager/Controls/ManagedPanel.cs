using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Design;

namespace Controls
{
	[ToolboxItem(false)]
	[Designer(typeof(ManagedPanelDesigner))]
	public class ManagedPanel : ScrollableControl
	{
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(typeof(DockStyle), "Fill")]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = DockStyle.Fill;
			}
		}

		[DefaultValue(typeof(AnchorStyles), "None")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override AnchorStyles Anchor
		{
			get
			{
				return AnchorStyles.None;
			}
			set
			{
				base.Anchor = AnchorStyles.None;
			}
		}

		public ManagedPanel()
		{
			base.Dock = DockStyle.Fill;
			SetStyle(ControlStyles.ResizeRedraw, value: true);
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);
			base.Location = Point.Empty;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			foreach (Control control in base.Controls)
			{
				control.Size = new Size(base.Width, base.Height - 10);
			}
			base.OnSizeChanged(e);
			if (base.Parent == null)
			{
				base.Size = Size.Empty;
			}
			else
			{
				base.Size = base.Parent.ClientSize;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			foreach (Control control in base.Controls)
			{
				control.Size = new Size(base.Width, base.Height - 10);
			}
			base.OnResize(e);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			if (!(base.Parent is PanelManager) && base.Parent != null)
			{
				throw new ArgumentException("Managed Panels may only be added to a Panel Manager.");
			}
			base.Size = base.Parent.Size;
			base.OnParentChanged(e);
		}
	}
}
