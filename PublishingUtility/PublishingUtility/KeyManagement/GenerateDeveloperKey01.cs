using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.KeyManagement
{
	public class GenerateDeveloperKey01 : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Label label1;

		private Label label2;

		private TextBox textBoxKeyName;

		private Button buttonOK;

		private Button buttonCancel;

		public GenerateDeveloperKey01()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxKeyName.Text))
			{
				MessageBox.Show(Utility.TextLanguage("Please input key name.", "鍵名を入力してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (textBoxKeyName.Text.Length > 31)
			{
				MessageBox.Show(string.Format(Resources.enterWithinX_Text, 31), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Regex regex = new Regex("^[a-zA-Z0-9_-]+$");
			if (!regex.IsMatch(textBoxKeyName.Text))
			{
				MessageBox.Show(Resources.onlyUseXCharactor_Text, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Program.appConfigData.PublisherKeyNameTmp = textBoxKeyName.Text;
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void GenerateDeveloperKey01_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (sender == this)
			{
				_ = bNextButton;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.GenerateDeveloperKey01));
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			textBoxKeyName = new System.Windows.Forms.TextBox();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(textBoxKeyName, "textBoxKeyName");
			textBoxKeyName.Name = "textBoxKeyName";
			resources.ApplyResources(buttonOK, "buttonOK");
			buttonOK.Name = "buttonOK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(textBoxKeyName);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "GenerateDeveloperKey01";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GenerateDeveloperKey01_FormClosing);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
