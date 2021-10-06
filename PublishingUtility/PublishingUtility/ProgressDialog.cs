using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility
{
	public class ProgressDialog : Form
	{
		private object workerArgument;

		private object _result;

		private Exception _error;

		private IContainer components;

		private ProgressBar progressBar1;

		private Label messageLabel;

		private Button cancelAsyncButton;

		private BackgroundWorker backgroundWorker1;

		public object Result => _result;

		public Exception Error => _error;

		public BackgroundWorker BackgroundWorker => backgroundWorker1;

		public ProgressDialog(string caption, DoWorkEventHandler doWork, object argument)
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			workerArgument = argument;
			Text = caption;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			base.ControlBox = false;
			base.CancelButton = cancelAsyncButton;
			messageLabel.Text = "";
			progressBar1.Minimum = 0;
			progressBar1.Maximum = 100;
			progressBar1.Value = 0;
			cancelAsyncButton.Text = "Cancel";
			cancelAsyncButton.Enabled = true;
			backgroundWorker1.WorkerReportsProgress = true;
			backgroundWorker1.WorkerSupportsCancellation = true;
			base.Shown += ProgressDialog_Shown;
			cancelAsyncButton.Click += cancelAsyncButton_Click;
			backgroundWorker1.DoWork += doWork;
			backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
			backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
		}

		public ProgressDialog(string formTitle, DoWorkEventHandler doWorkHandler)
			: this(formTitle, doWorkHandler, null)
		{
		}

		private void ProgressDialog_Shown(object sender, EventArgs e)
		{
			backgroundWorker1.RunWorkerAsync(workerArgument);
		}

		private void cancelAsyncButton_Click(object sender, EventArgs e)
		{
			cancelAsyncButton.Enabled = false;
			backgroundWorker1.CancelAsync();
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage < progressBar1.Minimum)
			{
				progressBar1.Value = progressBar1.Minimum;
			}
			else if (progressBar1.Maximum < e.ProgressPercentage)
			{
				progressBar1.Value = progressBar1.Maximum;
			}
			else
			{
				progressBar1.Value = e.ProgressPercentage;
			}
			messageLabel.Text = (string)e.UserState;
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				MessageBox.Show(this, "Error \n\n" + e.Error.Message, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				_error = e.Error;
				base.DialogResult = DialogResult.Abort;
			}
			else if (e.Cancelled)
			{
				base.DialogResult = DialogResult.Cancel;
			}
			else
			{
				_result = e.Result;
				base.DialogResult = DialogResult.OK;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.ProgressDialog));
			progressBar1 = new System.Windows.Forms.ProgressBar();
			messageLabel = new System.Windows.Forms.Label();
			cancelAsyncButton = new System.Windows.Forms.Button();
			backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			SuspendLayout();
			progressBar1.Location = new System.Drawing.Point(12, 12);
			progressBar1.Name = "progressBar1";
			progressBar1.Size = new System.Drawing.Size(355, 23);
			progressBar1.TabIndex = 0;
			messageLabel.AutoSize = true;
			messageLabel.Location = new System.Drawing.Point(12, 46);
			messageLabel.Name = "messageLabel";
			messageLabel.Size = new System.Drawing.Size(35, 12);
			messageLabel.TabIndex = 1;
			messageLabel.Text = "label1";
			cancelAsyncButton.Location = new System.Drawing.Point(292, 62);
			cancelAsyncButton.Name = "cancelAsyncButton";
			cancelAsyncButton.Size = new System.Drawing.Size(75, 23);
			cancelAsyncButton.TabIndex = 2;
			cancelAsyncButton.Text = "button1";
			cancelAsyncButton.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(379, 97);
			base.Controls.Add(cancelAsyncButton);
			base.Controls.Add(messageLabel);
			base.Controls.Add(progressBar1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "ProgressDialog";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "ProgressDialog";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
