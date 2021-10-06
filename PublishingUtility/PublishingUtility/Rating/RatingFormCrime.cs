using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormCrime : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Button buttonNext;

		private GroupBox groupBox1;

		private RadioButton radioButton01No;

		private RadioButton radioButton01Yes;

		private Label label1;

		private GroupBox groupBox2;

		private RadioButton radioButton02No;

		private RadioButton radioButton02Yes;

		private Label label2;

		private Label label10;

		public RatingFormCrime()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.IsCrimeQ01)
			{
				radioButton01Yes.Checked = true;
				radioButton01No.Checked = false;
			}
			else
			{
				radioButton01Yes.Checked = false;
				radioButton01No.Checked = true;
			}
			if (Program._RatingData.IsCrimeQ02)
			{
				radioButton02Yes.Checked = true;
				radioButton02No.Checked = false;
			}
			else
			{
				radioButton02Yes.Checked = false;
				radioButton02No.Checked = true;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01Yes.Checked)
			{
				Program._RatingData.IsCrimeQ01 = true;
			}
			else
			{
				Program._RatingData.IsCrimeQ01 = false;
			}
			if (radioButton02Yes.Checked)
			{
				Program._RatingData.IsCrimeQ02 = true;
			}
			else
			{
				Program._RatingData.IsCrimeQ02 = false;
			}
		}

		private void RatingFormCrime_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (sender == this && !bNextButton)
			{
				SceRatingData.CheckCloseWindow(this, e);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormCrime));
			buttonNext = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			radioButton01No = new System.Windows.Forms.RadioButton();
			radioButton01Yes = new System.Windows.Forms.RadioButton();
			label1 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			label10 = new System.Windows.Forms.Label();
			radioButton02No = new System.Windows.Forms.RadioButton();
			radioButton02Yes = new System.Windows.Forms.RadioButton();
			label2 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			resources.ApplyResources(buttonNext, "buttonNext");
			buttonNext.Name = "buttonNext";
			buttonNext.UseVisualStyleBackColor = true;
			buttonNext.Click += new System.EventHandler(buttonNext_Click);
			groupBox1.Controls.Add(radioButton01No);
			groupBox1.Controls.Add(radioButton01Yes);
			groupBox1.Controls.Add(label1);
			resources.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			resources.ApplyResources(radioButton01No, "radioButton01No");
			radioButton01No.Name = "radioButton01No";
			radioButton01No.TabStop = true;
			radioButton01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton01Yes, "radioButton01Yes");
			radioButton01Yes.Name = "radioButton01Yes";
			radioButton01Yes.TabStop = true;
			radioButton01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			groupBox2.Controls.Add(label10);
			groupBox2.Controls.Add(radioButton02No);
			groupBox2.Controls.Add(radioButton02Yes);
			groupBox2.Controls.Add(label2);
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			resources.ApplyResources(radioButton02No, "radioButton02No");
			radioButton02No.Name = "radioButton02No";
			radioButton02No.TabStop = true;
			radioButton02No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton02Yes, "radioButton02Yes");
			radioButton02Yes.Name = "radioButton02Yes";
			radioButton02Yes.TabStop = true;
			radioButton02Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormCrime";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormCrime_FormClosing);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
		}
	}
}
