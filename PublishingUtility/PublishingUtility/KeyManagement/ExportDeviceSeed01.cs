using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.KeyManagement
{
	public class ExportDeviceSeed01 : Form
	{
		private DeviceStatus device;

		private string newNickname;

		private IContainer components;

		private Button buttonOK;

		private Label label1;

		private TextBox textBoxDeviceID;

		private Label label2;

		private Label label3;

		private TextBox textBoxNickname;

		private Button buttonCancel;

		public ExportDeviceSeed01()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		public ExportDeviceSeed01(DeviceStatus device)
			: this()
		{
			this.device = device;
			Text = Utility.TextLanguage($"Export Device Seed", $"デバイスシードのエクスポート");
			textBoxDeviceID.Text = device.deviceType.ToString() + " : " + device.DeviceID;
			textBoxNickname.Text = device.Nickname;
		}

		public ExportDeviceSeed01(DeviceStatus device, int num, int count)
			: this()
		{
			this.device = device;
			Text = Utility.TextLanguage($"Export Device Seed {num}/{count}", $"デバイスシードのエクスポート {num}/{count}");
			textBoxDeviceID.Text = device.deviceType.ToString() + " : " + device.DeviceID;
			textBoxNickname.Text = device.Nickname;
		}

		private void ExportDeviceSeed01_Load(object sender, EventArgs e)
		{
			base.ActiveControl = textBoxNickname;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (DeviceStatus.IsDeviceNicknameValid(textBoxNickname.Text))
			{
				newNickname = textBoxNickname.Text;
				if (Save())
				{
					base.DialogResult = DialogResult.OK;
				}
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		public bool Save()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Export Device Seed";
			saveFileDialog.Filter = "Device seed files|*.seed|All files|*.*";
			saveFileDialog.FileName = device.DeviceID + ".seed";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.OverwritePrompt = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				string nickname = device.Nickname;
				device.Nickname = newNickname;
				device.Save(saveFileDialog.FileName);
				if (nickname != newNickname)
				{
					string pathFile = Program.PATH_DIR_DEVICE_SEED + device.DeviceID + ".seed";
					device.Save(pathFile);
				}
				Program.MainForm.RefreshGui();
				return true;
			}
			return false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.ExportDeviceSeed01));
			buttonOK = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			textBoxDeviceID = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			textBoxNickname = new System.Windows.Forms.TextBox();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			resources.ApplyResources(buttonOK, "buttonOK");
			buttonOK.Name = "buttonOK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			resources.ApplyResources(textBoxDeviceID, "textBoxDeviceID");
			textBoxDeviceID.Name = "textBoxDeviceID";
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			resources.ApplyResources(textBoxNickname, "textBoxNickname");
			textBoxNickname.Name = "textBoxNickname";
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(buttonCancel);
			base.Controls.Add(textBoxNickname);
			base.Controls.Add(label3);
			base.Controls.Add(label2);
			base.Controls.Add(textBoxDeviceID);
			base.Controls.Add(label1);
			base.Controls.Add(buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ExportDeviceSeed01";
			base.Load += new System.EventHandler(ExportDeviceSeed01_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
