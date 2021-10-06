using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility.Dialog
{
	public class WaitDialog : Form
	{
		private IContainer components;

		private Label Message;

		public WaitDialog()
		{
			InitializeComponent();
			base.DialogResult = DialogResult.OK;
		}

		public void SetMessage(string message)
		{
			Message.Text = message;
			Refresh();
			Application.DoEvents();
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
			Message = new System.Windows.Forms.Label();
			SuspendLayout();
			Message.Location = new System.Drawing.Point(12, 9);
			Message.Name = "Message";
			Message.Size = new System.Drawing.Size(280, 64);
			Message.TabIndex = 0;
			Message.Text = "...";
			Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(304, 82);
			base.ControlBox = false;
			base.Controls.Add(Message);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "WaitDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Publishing Utility";
			ResumeLayout(false);
		}
	}
}
