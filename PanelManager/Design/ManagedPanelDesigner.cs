using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Controls;

namespace Design
{
	public class ManagedPanelDesigner : ScrollableControlDesigner
	{
		private DesignerVerbCollection m_verbs = new DesignerVerbCollection();

		private ISelectionService m_SelectionService;

		private ManagedPanel HostControl => (ManagedPanel)Control;

		public ISelectionService SelectionService
		{
			get
			{
				if (m_SelectionService == null)
				{
					m_SelectionService = (ISelectionService)GetService(typeof(ISelectionService));
				}
				return m_SelectionService;
			}
		}

		public override SelectionRules SelectionRules => SelectionRules.Visible;

		public override DesignerVerbCollection Verbs => m_verbs;

		public ManagedPanelDesigner()
		{
			DesignerVerb value = new DesignerVerb("Select PanelManager", OnSelectManager);
			m_verbs.Add(value);
		}

		private void OnSelectManager(object sender, EventArgs e)
		{
			if (HostControl.Parent != null)
			{
				SelectionService.SetSelectedComponents(new Component[1] { HostControl.Parent });
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			Color color = ((!((double)Control.BackColor.GetBrightness() >= 0.5)) ? Color.White : ControlPaint.Dark(Control.BackColor));
			Pen pen = new Pen(color);
			Rectangle clientRectangle = Control.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			pen.DashStyle = DashStyle.Dash;
			pe.Graphics.DrawRectangle(pen, clientRectangle);
			pen.Dispose();
		}

		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("Anchor");
			properties.Remove("TabStop");
			properties.Remove("TabIndex");
			base.PostFilterProperties(properties);
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control.Visible = true;
		}
	}
}
