using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.Dialog
{
	public class InstallPakageAppName : Form
	{
		private IContainer components;

		private Label label1;

		private Label label2;

		private Button buttonOk;

		private Button buttonCancel;

		internal TextBox appNameTextBox;

		public InstallPakageAppName(string text)
		{
			InitializeComponent();
			label1.Text = Resources.installAppsDialogLabel1_Text;
			label2.Text = Resources.installAppsDialogLabel2_Text;
			appNameTextBox.Text = text;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Dialog.InstallPakageAppName));
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			appNameTextBox = new System.Windows.Forms.TextBox();
			buttonOk = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(30, 21);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 12);
			label1.TabIndex = 0;
			label1.Text = "label1";
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(30, 66);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(35, 12);
			label2.TabIndex = 1;
			label2.Text = "label2";
			appNameTextBox.Location = new System.Drawing.Point(84, 63);
			appNameTextBox.Name = "appNameTextBox";
			appNameTextBox.Size = new System.Drawing.Size(230, 19);
			appNameTextBox.TabIndex = 2;
			buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			buttonOk.Location = new System.Drawing.Point(71, 105);
			buttonOk.Name = "buttonOk";
			buttonOk.Size = new System.Drawing.Size(75, 23);
			buttonOk.TabIndex = 3;
			buttonOk.Text = "&OK";
			buttonOk.UseVisualStyleBackColor = true;
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(213, 105);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 4;
			buttonCancel.Text = "&Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = buttonOk;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(359, 159);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOk);
			base.Controls.Add(appNameTextBox);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "InstallPakageAppName";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Application Name";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
