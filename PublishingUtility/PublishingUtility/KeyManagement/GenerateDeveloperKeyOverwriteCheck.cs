using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class GenerateDeveloperKeyOverwriteCheck : Form
	{
		private IContainer components;

		private Label label1;

		private Button buttonOK;

		private Button buttonCancel;

		public GenerateDeveloperKeyOverwriteCheck()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.GenerateDeveloperKeyOverwriteCheck));
			label1 = new System.Windows.Forms.Label();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(52, 27);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(181, 24);
			label1.TabIndex = 0;
			label1.Text = "Publisher Key has arleady existed.\r\nDo you want to overwrite it?";
			buttonOK.Location = new System.Drawing.Point(48, 70);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(161, 70);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(284, 116);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "GenerateDeveloperKeyOverwriteCheck";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Overwrite Check";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
