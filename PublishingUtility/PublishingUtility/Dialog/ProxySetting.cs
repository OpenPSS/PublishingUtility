using System;
using System.ComponentModel;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.Dialog
{
	public class ProxySetting : Form
	{
		private IContainer components;

		private RadioButton radioButtonSystemDefault;

		private RadioButton radioButtonUseProxy;

		private Button buttonOK;

		private Button buttonCancel;

		private Label label1;

		private Label label2;

		private TextBox textBoxProxyAddress;

		private TextBox textBoxPort;

		private RadioButton radioButtonNotUseProxy;

		public ProxySetting()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			textBoxProxyAddress.Text = Program.appConfigData.saveProxyAddress;
			if (Program.appConfigData.savePort != 0)
			{
				textBoxPort.Text = Program.appConfigData.savePort.ToString();
			}
			if (Program.appConfigData.proxySetting == AppConfigData.ProxySetting.NotUseProxy)
			{
				radioButtonNotUseProxy.Checked = true;
				radioButtonSystemDefault.Checked = false;
				radioButtonUseProxy.Checked = false;
				textBoxProxyAddress.Enabled = false;
				textBoxPort.Enabled = false;
			}
			else if (Program.appConfigData.proxySetting == AppConfigData.ProxySetting.SystemDefault)
			{
				radioButtonNotUseProxy.Checked = false;
				radioButtonSystemDefault.Checked = true;
				radioButtonUseProxy.Checked = false;
				textBoxProxyAddress.Enabled = false;
				textBoxPort.Enabled = false;
			}
			else
			{
				radioButtonNotUseProxy.Checked = false;
				radioButtonSystemDefault.Checked = false;
				radioButtonUseProxy.Checked = true;
				textBoxProxyAddress.Enabled = true;
				textBoxPort.Enabled = true;
			}
		}

		private void radioButtonNotUseProxy_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonNotUseProxy.Checked)
			{
				textBoxProxyAddress.Enabled = false;
				textBoxPort.Enabled = false;
			}
		}

		private void radioButtonSystemDefault_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonUseProxy.Checked)
			{
				textBoxProxyAddress.Enabled = true;
				textBoxPort.Enabled = true;
			}
			else
			{
				textBoxProxyAddress.Enabled = false;
				textBoxPort.Enabled = false;
			}
		}

		private void radioButtonUseProxy_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonUseProxy.Checked)
			{
				textBoxProxyAddress.Enabled = true;
				textBoxPort.Enabled = true;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (radioButtonNotUseProxy.Checked)
			{
				Program.appConfigData.proxySetting = AppConfigData.ProxySetting.NotUseProxy;
			}
			else if (radioButtonSystemDefault.Checked)
			{
				Program.appConfigData.proxySetting = AppConfigData.ProxySetting.SystemDefault;
			}
			else
			{
				Program.appConfigData.proxySetting = AppConfigData.ProxySetting.UseProxy;
			}
			Program.appConfigData.saveProxyAddress = textBoxProxyAddress.Text;
			Program.appConfigData.SetProxyData();
			if (Program.appConfigData.proxySetting == AppConfigData.ProxySetting.UseProxy)
			{
				int? num = ConvertEx.ToInt32(textBoxPort.Text);
				if (num.HasValue)
				{
					Program.appConfigData.savePort = num.Value;
					Program.appConfigData.Port = num.Value;
					Program.appConfigData.Save();
					Close();
				}
				else
				{
					MessageBox.Show(Utility.TextLanguage("Port number is invalid.", "ポート番号が不正です。"), Utility.TextLanguage("Port number", "ポート番号"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			else
			{
				Close();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Close();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Dialog.ProxySetting));
			radioButtonSystemDefault = new System.Windows.Forms.RadioButton();
			radioButtonUseProxy = new System.Windows.Forms.RadioButton();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			textBoxProxyAddress = new System.Windows.Forms.TextBox();
			textBoxPort = new System.Windows.Forms.TextBox();
			radioButtonNotUseProxy = new System.Windows.Forms.RadioButton();
			SuspendLayout();
			resources.ApplyResources(radioButtonSystemDefault, "radioButtonSystemDefault");
			radioButtonSystemDefault.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButtonSystemDefault.Name = "radioButtonSystemDefault";
			radioButtonSystemDefault.TabStop = true;
			radioButtonSystemDefault.UseVisualStyleBackColor = true;
			radioButtonSystemDefault.CheckedChanged += new System.EventHandler(radioButtonSystemDefault_CheckedChanged);
			resources.ApplyResources(radioButtonUseProxy, "radioButtonUseProxy");
			radioButtonUseProxy.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButtonUseProxy.Name = "radioButtonUseProxy";
			radioButtonUseProxy.TabStop = true;
			radioButtonUseProxy.UseVisualStyleBackColor = true;
			radioButtonUseProxy.CheckedChanged += new System.EventHandler(radioButtonUseProxy_CheckedChanged);
			buttonOK.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			resources.ApplyResources(buttonOK, "buttonOK");
			buttonOK.Name = "buttonOK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			resources.ApplyResources(label1, "label1");
			label1.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label1.Name = "label1";
			resources.ApplyResources(label2, "label2");
			label2.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label2.Name = "label2";
			resources.ApplyResources(textBoxProxyAddress, "textBoxProxyAddress");
			textBoxProxyAddress.Name = "textBoxProxyAddress";
			resources.ApplyResources(textBoxPort, "textBoxPort");
			textBoxPort.Name = "textBoxPort";
			resources.ApplyResources(radioButtonNotUseProxy, "radioButtonNotUseProxy");
			radioButtonNotUseProxy.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButtonNotUseProxy.Name = "radioButtonNotUseProxy";
			radioButtonNotUseProxy.TabStop = true;
			radioButtonNotUseProxy.UseVisualStyleBackColor = true;
			radioButtonNotUseProxy.CheckedChanged += new System.EventHandler(radioButtonNotUseProxy_CheckedChanged);
			base.AcceptButton = buttonOK;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.Controls.Add(radioButtonNotUseProxy);
			base.Controls.Add(textBoxPort);
			base.Controls.Add(textBoxProxyAddress);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(radioButtonUseProxy);
			base.Controls.Add(radioButtonSystemDefault);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ProxySetting";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
