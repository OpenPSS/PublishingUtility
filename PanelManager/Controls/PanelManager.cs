using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Design;
using Editors;
using TypeConverters;

namespace Controls
{
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("SelectedPanel")]
	[Designer(typeof(PanelManagerDesigner))]
	public class PanelManager : Control
	{
		private Container components;

		private ManagedPanel m_SelectedPanel;

		private ManagedPanel oldSelection;

		[Editor(typeof(ManagedPanelCollectionEditor), typeof(UITypeEditor))]
		public ControlCollection ManagedPanels => base.Controls;

		[TypeConverter(typeof(SelectedPanelConverter))]
		public ManagedPanel SelectedPanel
		{
			get
			{
				return m_SelectedPanel;
			}
			set
			{
				if (m_SelectedPanel != value)
				{
					m_SelectedPanel = value;
					OnSelectedPanelChanged(EventArgs.Empty);
				}
			}
		}

		[Browsable(false)]
		public int SelectedIndex
		{
			get
			{
				return ManagedPanels.IndexOf(SelectedPanel);
			}
			set
			{
				if (value == -1)
				{
					SelectedPanel = null;
				}
				else
				{
					SelectedPanel = (ManagedPanel)ManagedPanels[value];
				}
			}
		}

		protected override Size DefaultSize => new Size(200, 100);

		public event EventHandler SelectedIndexChanged;

		public PanelManager()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		protected void OnSelectedPanelChanged(EventArgs e)
		{
			if (oldSelection != null)
			{
				oldSelection.Visible = false;
			}
			if (m_SelectedPanel != null)
			{
				m_SelectedPanel.Visible = true;
			}
			bool flag = false;
			flag = ((m_SelectedPanel != null) ? (!m_SelectedPanel.Equals(oldSelection)) : (oldSelection != null));
			if (flag && base.Created && this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, EventArgs.Empty);
			}
			oldSelection = m_SelectedPanel;
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			if (!(e.Control is ManagedPanel))
			{
				throw new ArgumentException("Only Controls.ManagedPanels can be added to a Controls.PanelManger.");
			}
			if (SelectedPanel != null)
			{
				SelectedPanel.Visible = false;
			}
			SelectedPanel = (ManagedPanel)e.Control;
			e.Control.Visible = true;
			e.Control.Size = base.Size;
			base.OnControlAdded(e);
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			if (e.Control is ManagedPanel)
			{
				if (ManagedPanels.Count > 0)
				{
					SelectedIndex = 0;
				}
				else
				{
					SelectedPanel = null;
				}
			}
			base.OnControlRemoved(e);
		}
	}
}
