using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class ImportDeviceSeed01 : Form
	{
		private IContainer components;

		public ImportDeviceSeed01()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
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
			SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(284, 262);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ImportDeviceSeed01";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "ImportDeviceSeed01";
			ResumeLayout(false);
		}
	}
}
