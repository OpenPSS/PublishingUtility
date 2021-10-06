using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility.Dialog
{
	public class Progress : Form
	{
		private object mArgument;

		private object mResult;

		private Exception mException;

		private IContainer components;

		private Button btn;

		private Label txt;

		public ProgressBar bar;

		private BackgroundWorker bgw;

		private Label per;

		public object Result => mResult;

		public Exception Error => mException;

		public BackgroundWorker BackgroundWorker => bgw;

		public Progress()
		{
			InitializeComponent();
		}

		public Progress(DoWorkEventHandler dowork, string text, object argument)
		{
			InitializeComponent();
			mArgument = argument;
			bar.Visible = (((int)argument != 0) ? true : false);
			bar.Minimum = 0;
			bar.Maximum = (int)argument;
			bar.Value = 0;
			txt.Text = text;
			bgw.WorkerReportsProgress = true;
			bgw.WorkerSupportsCancellation = true;
			base.Shown += DialogShown;
			btn.Click += btn_Click;
			bgw.DoWork += dowork;
			bgw.ProgressChanged += bgw_Changed;
			bgw.RunWorkerCompleted += bgw_Completed;
		}

		private void DialogShown(object sender, EventArgs e)
		{
			bgw.RunWorkerAsync(mArgument);
		}

		private void btn_Click(object sender, EventArgs e)
		{
			btn.Enabled = false;
			bgw.CancelAsync();
		}

		private void bgw_Changed(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage < bar.Minimum)
			{
				bar.Value = bar.Minimum;
			}
			else if (bar.Maximum < e.ProgressPercentage)
			{
				bar.Value = bar.Maximum;
			}
			else
			{
				bar.Value = e.ProgressPercentage;
			}
			int num = (int)(100f / (float)bar.Maximum * (float)bar.Value);
			per.Text = num + " %";
		}

		private void bgw_Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				mException = e.Error;
				base.DialogResult = DialogResult.Abort;
			}
			else if (e.Cancelled)
			{
				base.DialogResult = DialogResult.Cancel;
			}
			else
			{
				mResult = e.Result;
				if (e.Result != null)
				{
					base.DialogResult = DialogResult.Abort;
				}
				else
				{
					base.DialogResult = DialogResult.OK;
				}
			}
			Close();
		}

		private void Progress_Load(object sender, EventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Dialog.Progress));
			bar = new System.Windows.Forms.ProgressBar();
			btn = new System.Windows.Forms.Button();
			txt = new System.Windows.Forms.Label();
			bgw = new System.ComponentModel.BackgroundWorker();
			per = new System.Windows.Forms.Label();
			SuspendLayout();
			bar.Location = new System.Drawing.Point(12, 98);
			bar.Name = "bar";
			bar.Size = new System.Drawing.Size(260, 23);
			bar.Step = 1;
			bar.TabIndex = 0;
			btn.Location = new System.Drawing.Point(197, 127);
			btn.Name = "btn";
			btn.Size = new System.Drawing.Size(75, 23);
			btn.TabIndex = 1;
			btn.Text = "&Cancel";
			btn.UseVisualStyleBackColor = true;
			txt.Location = new System.Drawing.Point(12, 9);
			txt.Name = "txt";
			txt.Size = new System.Drawing.Size(260, 65);
			txt.TabIndex = 2;
			txt.Text = "...";
			per.Location = new System.Drawing.Point(12, 74);
			per.Name = "per";
			per.Size = new System.Drawing.Size(260, 21);
			per.TabIndex = 2;
			per.Text = "...";
			per.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(284, 162);
			base.Controls.Add(per);
			base.Controls.Add(txt);
			base.Controls.Add(btn);
			base.Controls.Add(bar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Progress";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Publishing Utility";
			base.Load += new System.EventHandler(Progress_Load);
			ResumeLayout(false);
		}
	}
}
