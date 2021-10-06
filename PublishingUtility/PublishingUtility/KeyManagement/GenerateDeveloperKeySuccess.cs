using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class GenerateDeveloperKeySuccess : Form
	{
		private IContainer components;

		private Button buttonOK;

		private Label label1;

		public GenerateDeveloperKeySuccess()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.GenerateDeveloperKeySuccess));
			buttonOK = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			resources.ApplyResources(buttonOK, "buttonOK");
			buttonOK.Name = "buttonOK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			base.AcceptButton = buttonOK;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(label1);
			base.Controls.Add(buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "GenerateDeveloperKeySuccess";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
