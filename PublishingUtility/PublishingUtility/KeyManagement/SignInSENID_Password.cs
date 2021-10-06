using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class SignInSENID_Password : Form
	{
		private IContainer components;

		private Label label1;

		private Label label2;

		private TextBox textBoxPsnID;

		private TextBox textBoxPassword;

		private Button buttonOK;

		private Button buttonCancel;

		private CheckBox checkBoxRememberPassword;

		public SignInSENID_Password(string title)
			: this()
		{
			Text = Text + " " + title;
		}

		public SignInSENID_Password()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program.appConfigData.irp)
			{
				checkBoxRememberPassword.Checked = true;
			}
			else
			{
				checkBoxRememberPassword.Checked = false;
				Program.appConfigData.Password = "";
			}
			if (!string.IsNullOrEmpty(Program.appConfigData.PsnID))
			{
				textBoxPsnID.Text = Program.appConfigData.PsnID;
			}
			if (Program.appConfigData.irp && !string.IsNullOrEmpty(Program.appConfigData.Ep))
			{
				textBoxPassword.Text = Utility.UnprotectText(Program.appConfigData.Ep);
			}
		}

		private void GenerateDeveloperKeyPsnID_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Program.appConfigData.PsnID))
			{
				base.ActiveControl = textBoxPassword;
				if (!string.IsNullOrEmpty(Program.appConfigData.Ep))
				{
					base.ActiveControl = buttonOK;
				}
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxPsnID.Text))
			{
				MessageBox.Show("Sony Entertainment Network ID is not entered.");
				return;
			}
			if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
			{
				MessageBox.Show("Password is not entered.");
				return;
			}
			Program.appConfigData.PsnID = textBoxPsnID.Text;
			Program.appConfigData.Password = textBoxPassword.Text;
			if (checkBoxRememberPassword.Checked)
			{
				Program.appConfigData.Ep = Utility.ProtectText(textBoxPassword.Text);
			}
			else
			{
				Program.appConfigData.Ep = "";
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void GenerateDeveloperKey02_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void GenerateDeveloperKeyPsnID_Shown(object sender, EventArgs e)
		{
			Activate();
		}

		private void checkBoxRememberPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxRememberPassword.Checked)
			{
				Program.appConfigData.irp = true;
			}
			else
			{
				Program.appConfigData.irp = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.SignInSENID_Password));
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			textBoxPsnID = new System.Windows.Forms.TextBox();
			textBoxPassword = new System.Windows.Forms.TextBox();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			checkBoxRememberPassword = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(textBoxPsnID, "textBoxPsnID");
			textBoxPsnID.Name = "textBoxPsnID";
			resources.ApplyResources(textBoxPassword, "textBoxPassword");
			textBoxPassword.Name = "textBoxPassword";
			textBoxPassword.UseSystemPasswordChar = true;
			resources.ApplyResources(buttonOK, "buttonOK");
			buttonOK.Name = "buttonOK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			resources.ApplyResources(checkBoxRememberPassword, "checkBoxRememberPassword");
			checkBoxRememberPassword.Name = "checkBoxRememberPassword";
			checkBoxRememberPassword.UseVisualStyleBackColor = true;
			checkBoxRememberPassword.CheckedChanged += new System.EventHandler(checkBoxRememberPassword_CheckedChanged);
			base.AcceptButton = buttonOK;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.Controls.Add(checkBoxRememberPassword);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(textBoxPassword);
			base.Controls.Add(textBoxPsnID);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "SignInSENID_Password";
			base.TopMost = true;
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GenerateDeveloperKey02_FormClosing);
			base.Load += new System.EventHandler(GenerateDeveloperKeyPsnID_Load);
			base.Shown += new System.EventHandler(GenerateDeveloperKeyPsnID_Shown);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
