using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class ChoicesForPublisherKey : Form
	{
		private IContainer components;

		private Button buttonGenerate;

		private Button buttonImport;

		private Button buttonCancel;

		private Label label1;

		public ChoicesForPublisherKey()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void ChoicesForPublisherKey_Load(object sender, EventArgs e)
		{
		}

		private void buttonGenerate_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
		}

		private void buttonImport_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.ChoicesForPublisherKey));
			buttonGenerate = new System.Windows.Forms.Button();
			buttonImport = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			resources.ApplyResources(buttonGenerate, "buttonGenerate");
			buttonGenerate.Name = "buttonGenerate";
			buttonGenerate.UseVisualStyleBackColor = true;
			buttonGenerate.Click += new System.EventHandler(buttonGenerate_Click);
			resources.ApplyResources(buttonImport, "buttonImport");
			buttonImport.Name = "buttonImport";
			buttonImport.UseVisualStyleBackColor = true;
			buttonImport.Click += new System.EventHandler(buttonImport_Click);
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(label1);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonImport);
			base.Controls.Add(buttonGenerate);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ChoicesForPublisherKey";
			base.ShowIcon = false;
			base.Load += new System.EventHandler(ChoicesForPublisherKey_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
