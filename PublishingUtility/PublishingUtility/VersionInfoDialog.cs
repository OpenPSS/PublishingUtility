using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public class VersionInfoDialog : Form
	{
		private PictureBox pictureBox1;

		private Label labelCopyright;

		private Label labelVersion;

		private Label labelVersionX;

		private Label labelCopyrightX;

		private Container components;

		public VersionInfoDialog()
		{
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.VersionInfoDialog));
			pictureBox1 = new System.Windows.Forms.PictureBox();
			labelCopyright = new System.Windows.Forms.Label();
			labelVersion = new System.Windows.Forms.Label();
			labelVersionX = new System.Windows.Forms.Label();
			labelCopyrightX = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			resources.ApplyResources(pictureBox1, "pictureBox1");
			pictureBox1.Image = PublishingUtility.Properties.Resources.about_bg;
			pictureBox1.Name = "pictureBox1";
			pictureBox1.TabStop = false;
			labelCopyright.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(labelCopyright, "labelCopyright");
			labelCopyright.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			labelCopyright.Name = "labelCopyright";
			labelVersion.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(labelVersion, "labelVersion");
			labelVersion.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			labelVersion.Name = "labelVersion";
			labelVersionX.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(labelVersionX, "labelVersionX");
			labelVersionX.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			labelVersionX.Name = "labelVersionX";
			labelCopyrightX.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(labelCopyrightX, "labelCopyrightX");
			labelCopyrightX.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			labelCopyrightX.Name = "labelCopyrightX";
			resources.ApplyResources(this, "$this");
			base.Controls.Add(labelCopyrightX);
			base.Controls.Add(labelVersionX);
			base.Controls.Add(labelCopyright);
			base.Controls.Add(labelVersion);
			base.Controls.Add(pictureBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "VersionInfoDialog";
			base.ShowInTaskbar = false;
			base.Load += new System.EventHandler(VerInfoDialog_Load);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		private void VerInfoDialog_Load(object sender, EventArgs e)
		{
			string productVersion = Application.ProductVersion;
			string productName = Application.ProductName;
			_ = Application.CompanyName;
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string text = "-";
			object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), inherit: false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				text = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
			}
			object[] customAttributes2 = entryAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), inherit: false);
			if (customAttributes2 != null && customAttributes2.Length > 0)
			{
				_ = ((AssemblyDescriptionAttribute)customAttributes2[0]).Description;
			}
			string executablePath = Application.ExecutablePath;
			FileInfo fileInfo = new FileInfo(executablePath);
			DateTime lastWriteTime = fileInfo.LastWriteTime;
			Text = string.Format(Resources.versionDialogTitle_Text, productName);
			labelVersion.Text = productVersion + "   " + lastWriteTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss");
			labelCopyright.Text = text;
			pictureBox1.Controls.Add(labelVersionX);
			pictureBox1.Controls.Add(labelVersion);
			pictureBox1.Controls.Add(labelCopyrightX);
			pictureBox1.Controls.Add(labelCopyright);
		}
	}
}
