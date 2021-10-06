using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility
{
	public class RatingFormResult : Form
	{
		private IContainer components;

		private Button buttonFinish;

		private TextBox textBox1;

		public RatingFormResult()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			Program._RatingData.TextResultRating = Program._RatingData.Result();
			textBox1.Text = Program._RatingData.TextResultRating;
		}

		private void buttonFinish_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.RatingFormResult));
			buttonFinish = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			SuspendLayout();
			resources.ApplyResources(buttonFinish, "buttonFinish");
			buttonFinish.Name = "buttonFinish";
			buttonFinish.UseVisualStyleBackColor = true;
			buttonFinish.Click += new System.EventHandler(buttonFinish_Click);
			resources.ApplyResources(textBox1, "textBox1");
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(textBox1);
			base.Controls.Add(buttonFinish);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormResult";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
