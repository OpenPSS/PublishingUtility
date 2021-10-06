using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormSex : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Button buttonNext;

		private GroupBox groupBox01;

		private RadioButton radioButton01No;

		private RadioButton radioButton01Yes;

		private Label label3;

		private GroupBox groupBox02;

		private RadioButton radioButton02No;

		private RadioButton radioButton02Yes;

		private Label label1;

		private GroupBox groupBox03;

		private RadioButton radioButton03No;

		private RadioButton radioButton03Yes;

		private Label label2;

		private GroupBox groupBox04;

		private RadioButton radioButton04No;

		private RadioButton radioButton04Yes;

		private Label label4;

		private Label label10;

		public RatingFormSex()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.IsSexQ01)
			{
				radioButton01Yes.Checked = true;
				radioButton01No.Checked = false;
			}
			else
			{
				radioButton01Yes.Checked = false;
				radioButton01No.Checked = true;
			}
			if (Program._RatingData.IsSexQ02)
			{
				radioButton02Yes.Checked = true;
				radioButton02No.Checked = false;
			}
			else
			{
				radioButton02Yes.Checked = false;
				radioButton02No.Checked = true;
			}
			if (Program._RatingData.IsSexQ03)
			{
				radioButton03Yes.Checked = true;
				radioButton03No.Checked = false;
			}
			else
			{
				radioButton03Yes.Checked = false;
				radioButton03No.Checked = true;
			}
			if (Program._RatingData.IsSexQ04)
			{
				radioButton04Yes.Checked = true;
				radioButton04No.Checked = false;
			}
			else
			{
				radioButton04Yes.Checked = false;
				radioButton04No.Checked = true;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01Yes.Checked)
			{
				Program._RatingData.IsSexQ01 = true;
			}
			else
			{
				Program._RatingData.IsSexQ01 = false;
			}
			if (radioButton02Yes.Checked)
			{
				Program._RatingData.IsSexQ02 = true;
			}
			else
			{
				Program._RatingData.IsSexQ02 = false;
			}
			if (radioButton03Yes.Checked)
			{
				Program._RatingData.IsSexQ03 = true;
			}
			else
			{
				Program._RatingData.IsSexQ03 = false;
			}
			if (radioButton04Yes.Checked)
			{
				Program._RatingData.IsSexQ04 = true;
			}
			else
			{
				Program._RatingData.IsSexQ04 = false;
			}
		}

		private void RatingFormSex_FormClosing(object sender, FormClosingEventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormSex));
			buttonNext = new System.Windows.Forms.Button();
			groupBox01 = new System.Windows.Forms.GroupBox();
			radioButton01No = new System.Windows.Forms.RadioButton();
			radioButton01Yes = new System.Windows.Forms.RadioButton();
			label3 = new System.Windows.Forms.Label();
			groupBox02 = new System.Windows.Forms.GroupBox();
			radioButton02No = new System.Windows.Forms.RadioButton();
			radioButton02Yes = new System.Windows.Forms.RadioButton();
			label1 = new System.Windows.Forms.Label();
			groupBox03 = new System.Windows.Forms.GroupBox();
			radioButton03No = new System.Windows.Forms.RadioButton();
			radioButton03Yes = new System.Windows.Forms.RadioButton();
			label2 = new System.Windows.Forms.Label();
			groupBox04 = new System.Windows.Forms.GroupBox();
			label10 = new System.Windows.Forms.Label();
			radioButton04No = new System.Windows.Forms.RadioButton();
			radioButton04Yes = new System.Windows.Forms.RadioButton();
			label4 = new System.Windows.Forms.Label();
			groupBox01.SuspendLayout();
			groupBox02.SuspendLayout();
			groupBox03.SuspendLayout();
			groupBox04.SuspendLayout();
			SuspendLayout();
			resources.ApplyResources(buttonNext, "buttonNext");
			buttonNext.Name = "buttonNext";
			buttonNext.UseVisualStyleBackColor = true;
			buttonNext.Click += new System.EventHandler(buttonNext_Click);
			groupBox01.Controls.Add(radioButton01No);
			groupBox01.Controls.Add(radioButton01Yes);
			groupBox01.Controls.Add(label3);
			resources.ApplyResources(groupBox01, "groupBox01");
			groupBox01.Name = "groupBox01";
			groupBox01.TabStop = false;
			resources.ApplyResources(radioButton01No, "radioButton01No");
			radioButton01No.Name = "radioButton01No";
			radioButton01No.TabStop = true;
			radioButton01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton01Yes, "radioButton01Yes");
			radioButton01Yes.Name = "radioButton01Yes";
			radioButton01Yes.TabStop = true;
			radioButton01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			groupBox02.Controls.Add(radioButton02No);
			groupBox02.Controls.Add(radioButton02Yes);
			groupBox02.Controls.Add(label1);
			resources.ApplyResources(groupBox02, "groupBox02");
			groupBox02.Name = "groupBox02";
			groupBox02.TabStop = false;
			resources.ApplyResources(radioButton02No, "radioButton02No");
			radioButton02No.Name = "radioButton02No";
			radioButton02No.TabStop = true;
			radioButton02No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton02Yes, "radioButton02Yes");
			radioButton02Yes.Name = "radioButton02Yes";
			radioButton02Yes.TabStop = true;
			radioButton02Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			groupBox03.Controls.Add(radioButton03No);
			groupBox03.Controls.Add(radioButton03Yes);
			groupBox03.Controls.Add(label2);
			resources.ApplyResources(groupBox03, "groupBox03");
			groupBox03.Name = "groupBox03";
			groupBox03.TabStop = false;
			resources.ApplyResources(radioButton03No, "radioButton03No");
			radioButton03No.Name = "radioButton03No";
			radioButton03No.TabStop = true;
			radioButton03No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton03Yes, "radioButton03Yes");
			radioButton03Yes.Name = "radioButton03Yes";
			radioButton03Yes.TabStop = true;
			radioButton03Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			groupBox04.Controls.Add(label10);
			groupBox04.Controls.Add(radioButton04No);
			groupBox04.Controls.Add(radioButton04Yes);
			groupBox04.Controls.Add(label4);
			resources.ApplyResources(groupBox04, "groupBox04");
			groupBox04.Name = "groupBox04";
			groupBox04.TabStop = false;
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			resources.ApplyResources(radioButton04No, "radioButton04No");
			radioButton04No.Name = "radioButton04No";
			radioButton04No.TabStop = true;
			radioButton04No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton04Yes, "radioButton04Yes");
			radioButton04Yes.Name = "radioButton04Yes";
			radioButton04Yes.TabStop = true;
			radioButton04Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label4, "label4");
			label4.Name = "label4";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox04);
			base.Controls.Add(groupBox03);
			base.Controls.Add(groupBox02);
			base.Controls.Add(groupBox01);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormSex";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormSex_FormClosing);
			groupBox01.ResumeLayout(false);
			groupBox01.PerformLayout();
			groupBox02.ResumeLayout(false);
			groupBox02.PerformLayout();
			groupBox03.ResumeLayout(false);
			groupBox03.PerformLayout();
			groupBox04.ResumeLayout(false);
			groupBox04.PerformLayout();
			ResumeLayout(false);
		}
	}
}
