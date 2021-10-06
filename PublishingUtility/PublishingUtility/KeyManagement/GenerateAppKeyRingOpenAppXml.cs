using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.KeyManagement
{
	public class GenerateAppKeyRingOpenAppXml : Form
	{
		private IContainer components;

		private Label label1;

		private Label label2;

		private Label label3;

		private TextBox textBoxAppXml;

		private TextBox textBoxAppID;

		private Button buttonOpenAppXml;

		private Button buttonOk;

		private Button buttonCancel;

		public GenerateAppKeyRingOpenAppXml()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxAppXml.Text))
			{
				MessageBox.Show("Please input app.xml.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				base.DialogResult = DialogResult.None;
			}
			else if (!File.Exists(textBoxAppXml.Text))
			{
				MessageBox.Show($"Can't find {textBoxAppXml.Text}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				base.DialogResult = DialogResult.None;
			}
			else if (string.IsNullOrEmpty(Program.appConfigData.AppID))
			{
				MessageBox.Show("Application ID is invalid.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.DialogResult = DialogResult.None;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonOpenAppXml_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Resources.openFileDialogTitle_Text;
			openFileDialog.Filter = Resources.openSaveFileDialogFilter_Text;
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "XML";
			openFileDialog.AddExtension = true;
			if (!string.IsNullOrEmpty(Program.appConfigData.PathAppXml))
			{
				openFileDialog.InitialDirectory = Program.appConfigData.PathAppXml;
			}
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Program.appConfigData.PathAppXml = Path.GetDirectoryName(openFileDialog.FileName);
			try
			{
				string fileName = openFileDialog.FileName;
				if (Utility.GetApplicationID(fileName, out var applicationID))
				{
					Program.appConfigData.AppID = applicationID;
					textBoxAppXml.Text = openFileDialog.FileName;
					textBoxAppID.Text = applicationID;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Get Application IDPublishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.KeyManagement.GenerateAppKeyRingOpenAppXml));
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			textBoxAppXml = new System.Windows.Forms.TextBox();
			textBoxAppID = new System.Windows.Forms.TextBox();
			buttonOpenAppXml = new System.Windows.Forms.Button();
			buttonOk = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			resources.ApplyResources(textBoxAppXml, "textBoxAppXml");
			textBoxAppXml.Name = "textBoxAppXml";
			resources.ApplyResources(textBoxAppID, "textBoxAppID");
			textBoxAppID.Name = "textBoxAppID";
			resources.ApplyResources(buttonOpenAppXml, "buttonOpenAppXml");
			buttonOpenAppXml.Name = "buttonOpenAppXml";
			buttonOpenAppXml.UseVisualStyleBackColor = true;
			buttonOpenAppXml.Click += new System.EventHandler(buttonOpenAppXml_Click);
			resources.ApplyResources(buttonOk, "buttonOk");
			buttonOk.Name = "buttonOk";
			buttonOk.UseVisualStyleBackColor = true;
			buttonOk.Click += new System.EventHandler(buttonOk_Click);
			resources.ApplyResources(buttonCancel, "buttonCancel");
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOk;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOk);
			base.Controls.Add(buttonOpenAppXml);
			base.Controls.Add(textBoxAppID);
			base.Controls.Add(textBoxAppXml);
			base.Controls.Add(label3);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "GenerateAppKeyRingOpenAppXml";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
