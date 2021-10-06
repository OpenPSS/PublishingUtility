using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.Dialog
{
	public class UninstallAppsConfirmationDialog : Form
	{
		private IContainer components;

		private Label label1;

		private Button button2;

		private Button button1;

		internal ListBox uninstallAppsListBox;

		public UninstallAppsConfirmationDialog()
		{
			InitializeComponent();
			label1.Text = Resources.uninstallAppsConfirmationBody_Text;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Dialog.UninstallAppsConfirmationDialog));
			uninstallAppsListBox = new System.Windows.Forms.ListBox();
			label1 = new System.Windows.Forms.Label();
			button2 = new System.Windows.Forms.Button();
			button1 = new System.Windows.Forms.Button();
			SuspendLayout();
			uninstallAppsListBox.FormattingEnabled = true;
			uninstallAppsListBox.ItemHeight = 12;
			uninstallAppsListBox.Items.AddRange(new object[1] { "Display Apps to uninstall here.." });
			uninstallAppsListBox.Location = new System.Drawing.Point(12, 21);
			uninstallAppsListBox.Name = "uninstallAppsListBox";
			uninstallAppsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
			uninstallAppsListBox.Size = new System.Drawing.Size(315, 100);
			uninstallAppsListBox.TabIndex = 0;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(10, 133);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 12);
			label1.TabIndex = 1;
			label1.Text = "label1";
			button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			button2.Location = new System.Drawing.Point(194, 167);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(75, 23);
			button2.TabIndex = 6;
			button2.Text = "&Cancel";
			button2.UseVisualStyleBackColor = true;
			button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			button1.Location = new System.Drawing.Point(69, 167);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 5;
			button1.Text = "&Uninstall";
			button1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(339, 210);
			base.Controls.Add(button2);
			base.Controls.Add(button1);
			base.Controls.Add(label1);
			base.Controls.Add(uninstallAppsListBox);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "UninstallAppsConfirmationDialog";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Uninstall Apps";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
