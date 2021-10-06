using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Controls;

namespace Design
{
	public class PanelManagerDesigner : ParentControlDesigner
	{
		private DesignerVerbCollection m_verbs = new DesignerVerbCollection();

		private IDesignerHost m_DesignerHost;

		private ISelectionService m_SelectionService;

		private PanelManager HostControl => (PanelManager)Control;

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (m_verbs.Count == 2)
				{
					m_verbs[1].Enabled = HostControl.ManagedPanels.Count > 0;
				}
				return m_verbs;
			}
		}

		public IDesignerHost DesignerHost
		{
			get
			{
				if (m_DesignerHost == null)
				{
					m_DesignerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
				}
				return m_DesignerHost;
			}
		}

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

		public PanelManagerDesigner()
		{
			DesignerVerb designerVerb = new DesignerVerb("Add MangedPanel", OnAddPanel);
			DesignerVerb designerVerb2 = new DesignerVerb("Remove ManagedPanel", OnRemovePanel);
			m_verbs.AddRange(new DesignerVerb[2] { designerVerb, designerVerb2 });
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
		}

		private void OnAddPanel(object sender, EventArgs e)
		{
			Control.ControlCollection oldValue = HostControl.Controls;
			RaiseComponentChanging(TypeDescriptor.GetProperties(HostControl)["ManagedPanels"]);
			ManagedPanel managedPanel = (ManagedPanel)DesignerHost.CreateComponent(typeof(ManagedPanel));
			managedPanel.Text = managedPanel.Name;
			HostControl.ManagedPanels.Add(managedPanel);
			RaiseComponentChanged(TypeDescriptor.GetProperties(HostControl)["ManagedPanels"], oldValue, HostControl.ManagedPanels);
			HostControl.SelectedPanel = managedPanel;
			SetVerbs();
		}

		private void OnRemovePanel(object sender, EventArgs e)
		{
			Control.ControlCollection oldValue = HostControl.Controls;
			if (HostControl.SelectedIndex >= 0)
			{
				RaiseComponentChanging(TypeDescriptor.GetProperties(HostControl)["TabPages"]);
				DesignerHost.DestroyComponent((ManagedPanel)HostControl.ManagedPanels[HostControl.SelectedIndex]);
				RaiseComponentChanged(TypeDescriptor.GetProperties(HostControl)["ManagedPanels"], oldValue, HostControl.ManagedPanels);
				SelectionService.SetSelectedComponents(new IComponent[1] { HostControl }, SelectionTypes.Auto);
				SetVerbs();
			}
		}

		private void SetVerbs()
		{
			Verbs[1].Enabled = HostControl.ManagedPanels.Count == 1;
		}

		protected override void PostFilterProperties(IDictionary properties)
		{
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("Text");
			base.PostFilterProperties(properties);
		}

		public override void OnSetComponentDefaults()
		{
			HostControl.ManagedPanels.Add((ManagedPanel)DesignerHost.CreateComponent(typeof(ManagedPanel)));
			HostControl.ManagedPanels.Add((ManagedPanel)DesignerHost.CreateComponent(typeof(ManagedPanel)));
			PanelManager panelManager = (PanelManager)Control;
			panelManager.ManagedPanels[0].Text = panelManager.ManagedPanels[0].Name;
			panelManager.ManagedPanels[1].Text = panelManager.ManagedPanels[1].Name;
			HostControl.SelectedIndex = 0;
		}
	}
}
