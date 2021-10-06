using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormViolence02 : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Button buttonNext;

		private GroupBox groupBox05;

		private GroupBox groupBox05_03;

		private RadioButton radioButton05_03No;

		private RadioButton radioButton05_03Yes;

		private Label label9;

		private GroupBox groupBox05_02;

		private RadioButton radioButton05_02No;

		private RadioButton radioButton05_02Yes;

		private Label label8;

		private GroupBox groupBox05_01;

		private RadioButton radioButton05_01No;

		private RadioButton radioButton05_01Yes;

		private Label label6;

		private RadioButton radioButton05No;

		private RadioButton radioButton05Yes;

		private Label label7;

		private GroupBox groupBox05_05;

		private RadioButton radioButton05_05No;

		private RadioButton radioButton05_05Yes;

		private Label label2;

		private GroupBox groupBox05_04;

		private RadioButton radioButton05_04No;

		private RadioButton radioButton05_04Yes;

		private Label label1;

		private GroupBox groupBox05_07;

		private RadioButton radioButton05_07No;

		private RadioButton radioButton05_07Yes;

		private Label label4;

		private GroupBox groupBox05_06;

		private RadioButton radioButton05_06No;

		private RadioButton radioButton05_06Yes;

		private Label label3;

		private Label label5;

		private Label label10;

		private Label label11;

		private Label label12;

		public RatingFormViolence02()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.IsViolenceQ05)
			{
				radioButton05Yes.Checked = true;
				radioButton05No.Checked = false;
			}
			else
			{
				radioButton05Yes.Checked = false;
				radioButton05No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_01)
			{
				radioButton05_01Yes.Checked = true;
				radioButton05_01No.Checked = false;
			}
			else
			{
				radioButton05_01Yes.Checked = false;
				radioButton05_01No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_02)
			{
				radioButton05_02Yes.Checked = true;
				radioButton05_02No.Checked = false;
			}
			else
			{
				radioButton05_02Yes.Checked = false;
				radioButton05_02No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_03)
			{
				radioButton05_03Yes.Checked = true;
				radioButton05_03No.Checked = false;
			}
			else
			{
				radioButton05_03Yes.Checked = false;
				radioButton05_03No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_04)
			{
				radioButton05_04Yes.Checked = true;
				radioButton05_04No.Checked = false;
			}
			else
			{
				radioButton05_04Yes.Checked = false;
				radioButton05_04No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_05)
			{
				radioButton05_05Yes.Checked = true;
				radioButton05_05No.Checked = false;
			}
			else
			{
				radioButton05_05Yes.Checked = false;
				radioButton05_05No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_06)
			{
				radioButton05_06Yes.Checked = true;
				radioButton05_06No.Checked = false;
			}
			else
			{
				radioButton05_06Yes.Checked = false;
				radioButton05_06No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ05_07)
			{
				radioButton05_07Yes.Checked = true;
				radioButton05_07No.Checked = false;
			}
			else
			{
				radioButton05_07Yes.Checked = false;
				radioButton05_07No.Checked = true;
			}
			if (radioButton05Yes.Checked)
			{
				groupBox05_01.Enabled = true;
				groupBox05_02.Enabled = true;
				groupBox05_03.Enabled = true;
				groupBox05_04.Enabled = true;
				groupBox05_05.Enabled = true;
				groupBox05_06.Enabled = true;
				groupBox05_07.Enabled = true;
			}
			else
			{
				groupBox05_01.Enabled = false;
				groupBox05_02.Enabled = false;
				groupBox05_03.Enabled = false;
				groupBox05_04.Enabled = false;
				groupBox05_05.Enabled = false;
				groupBox05_06.Enabled = false;
				groupBox05_07.Enabled = false;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton05Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05 = false;
			}
			if (radioButton05_01Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_01 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_01 = false;
			}
			if (radioButton05_02Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_02 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_02 = false;
			}
			if (radioButton05_03Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_03 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_03 = false;
			}
			if (radioButton05_04Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_04 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_04 = false;
			}
			if (radioButton05_05Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_05 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_05 = false;
			}
			if (radioButton05_06Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_06 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_06 = false;
			}
			if (radioButton05_07Yes.Checked)
			{
				Program._RatingData.IsViolenceQ05_07 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ05_07 = false;
			}
		}

		private void RatingFormViolence02_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (sender == this && !bNextButton)
			{
				SceRatingData.CheckCloseWindow(this, e);
			}
		}

		private void radioButton05Yes_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton05Yes.Checked)
			{
				groupBox05_01.Enabled = true;
				groupBox05_02.Enabled = true;
				groupBox05_03.Enabled = true;
				groupBox05_04.Enabled = true;
				groupBox05_05.Enabled = true;
				groupBox05_06.Enabled = true;
				groupBox05_07.Enabled = true;
			}
			else
			{
				groupBox05_01.Enabled = false;
				groupBox05_02.Enabled = false;
				groupBox05_03.Enabled = false;
				groupBox05_04.Enabled = false;
				groupBox05_05.Enabled = false;
				groupBox05_06.Enabled = false;
				groupBox05_07.Enabled = false;
			}
		}

		private void radioButton05No_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton05Yes.Checked)
			{
				groupBox05_01.Enabled = true;
				groupBox05_02.Enabled = true;
				groupBox05_03.Enabled = true;
				groupBox05_04.Enabled = true;
				groupBox05_05.Enabled = true;
				groupBox05_06.Enabled = true;
				groupBox05_07.Enabled = true;
			}
			else
			{
				groupBox05_01.Enabled = false;
				groupBox05_02.Enabled = false;
				groupBox05_03.Enabled = false;
				groupBox05_04.Enabled = false;
				groupBox05_05.Enabled = false;
				groupBox05_06.Enabled = false;
				groupBox05_07.Enabled = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormViolence02));
			buttonNext = new System.Windows.Forms.Button();
			groupBox05 = new System.Windows.Forms.GroupBox();
			groupBox05_07 = new System.Windows.Forms.GroupBox();
			label5 = new System.Windows.Forms.Label();
			radioButton05_07No = new System.Windows.Forms.RadioButton();
			radioButton05_07Yes = new System.Windows.Forms.RadioButton();
			label4 = new System.Windows.Forms.Label();
			groupBox05_06 = new System.Windows.Forms.GroupBox();
			label10 = new System.Windows.Forms.Label();
			radioButton05_06No = new System.Windows.Forms.RadioButton();
			radioButton05_06Yes = new System.Windows.Forms.RadioButton();
			label3 = new System.Windows.Forms.Label();
			groupBox05_05 = new System.Windows.Forms.GroupBox();
			label11 = new System.Windows.Forms.Label();
			radioButton05_05No = new System.Windows.Forms.RadioButton();
			radioButton05_05Yes = new System.Windows.Forms.RadioButton();
			label2 = new System.Windows.Forms.Label();
			groupBox05_04 = new System.Windows.Forms.GroupBox();
			label12 = new System.Windows.Forms.Label();
			radioButton05_04No = new System.Windows.Forms.RadioButton();
			radioButton05_04Yes = new System.Windows.Forms.RadioButton();
			label1 = new System.Windows.Forms.Label();
			groupBox05_03 = new System.Windows.Forms.GroupBox();
			radioButton05_03No = new System.Windows.Forms.RadioButton();
			radioButton05_03Yes = new System.Windows.Forms.RadioButton();
			label9 = new System.Windows.Forms.Label();
			groupBox05_02 = new System.Windows.Forms.GroupBox();
			radioButton05_02No = new System.Windows.Forms.RadioButton();
			radioButton05_02Yes = new System.Windows.Forms.RadioButton();
			label8 = new System.Windows.Forms.Label();
			groupBox05_01 = new System.Windows.Forms.GroupBox();
			radioButton05_01No = new System.Windows.Forms.RadioButton();
			radioButton05_01Yes = new System.Windows.Forms.RadioButton();
			label6 = new System.Windows.Forms.Label();
			radioButton05No = new System.Windows.Forms.RadioButton();
			radioButton05Yes = new System.Windows.Forms.RadioButton();
			label7 = new System.Windows.Forms.Label();
			groupBox05.SuspendLayout();
			groupBox05_07.SuspendLayout();
			groupBox05_06.SuspendLayout();
			groupBox05_05.SuspendLayout();
			groupBox05_04.SuspendLayout();
			groupBox05_03.SuspendLayout();
			groupBox05_02.SuspendLayout();
			groupBox05_01.SuspendLayout();
			SuspendLayout();
			resources.ApplyResources(buttonNext, "buttonNext");
			buttonNext.Name = "buttonNext";
			buttonNext.UseVisualStyleBackColor = true;
			buttonNext.Click += new System.EventHandler(buttonNext_Click);
			groupBox05.Controls.Add(groupBox05_07);
			groupBox05.Controls.Add(groupBox05_06);
			groupBox05.Controls.Add(groupBox05_05);
			groupBox05.Controls.Add(groupBox05_04);
			groupBox05.Controls.Add(groupBox05_03);
			groupBox05.Controls.Add(groupBox05_02);
			groupBox05.Controls.Add(groupBox05_01);
			groupBox05.Controls.Add(radioButton05No);
			groupBox05.Controls.Add(radioButton05Yes);
			groupBox05.Controls.Add(label7);
			resources.ApplyResources(groupBox05, "groupBox05");
			groupBox05.Name = "groupBox05";
			groupBox05.TabStop = false;
			groupBox05_07.Controls.Add(label5);
			groupBox05_07.Controls.Add(radioButton05_07No);
			groupBox05_07.Controls.Add(radioButton05_07Yes);
			groupBox05_07.Controls.Add(label4);
			resources.ApplyResources(groupBox05_07, "groupBox05_07");
			groupBox05_07.Name = "groupBox05_07";
			groupBox05_07.TabStop = false;
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			resources.ApplyResources(radioButton05_07No, "radioButton05_07No");
			radioButton05_07No.Name = "radioButton05_07No";
			radioButton05_07No.TabStop = true;
			radioButton05_07No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_07Yes, "radioButton05_07Yes");
			radioButton05_07Yes.Name = "radioButton05_07Yes";
			radioButton05_07Yes.TabStop = true;
			radioButton05_07Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label4, "label4");
			label4.Name = "label4";
			groupBox05_06.Controls.Add(label10);
			groupBox05_06.Controls.Add(radioButton05_06No);
			groupBox05_06.Controls.Add(radioButton05_06Yes);
			groupBox05_06.Controls.Add(label3);
			resources.ApplyResources(groupBox05_06, "groupBox05_06");
			groupBox05_06.Name = "groupBox05_06";
			groupBox05_06.TabStop = false;
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			resources.ApplyResources(radioButton05_06No, "radioButton05_06No");
			radioButton05_06No.Name = "radioButton05_06No";
			radioButton05_06No.TabStop = true;
			radioButton05_06No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_06Yes, "radioButton05_06Yes");
			radioButton05_06Yes.Name = "radioButton05_06Yes";
			radioButton05_06Yes.TabStop = true;
			radioButton05_06Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			groupBox05_05.Controls.Add(label11);
			groupBox05_05.Controls.Add(radioButton05_05No);
			groupBox05_05.Controls.Add(radioButton05_05Yes);
			groupBox05_05.Controls.Add(label2);
			resources.ApplyResources(groupBox05_05, "groupBox05_05");
			groupBox05_05.Name = "groupBox05_05";
			groupBox05_05.TabStop = false;
			resources.ApplyResources(label11, "label11");
			label11.Name = "label11";
			resources.ApplyResources(radioButton05_05No, "radioButton05_05No");
			radioButton05_05No.Name = "radioButton05_05No";
			radioButton05_05No.TabStop = true;
			radioButton05_05No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_05Yes, "radioButton05_05Yes");
			radioButton05_05Yes.Name = "radioButton05_05Yes";
			radioButton05_05Yes.TabStop = true;
			radioButton05_05Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			groupBox05_04.Controls.Add(label12);
			groupBox05_04.Controls.Add(radioButton05_04No);
			groupBox05_04.Controls.Add(radioButton05_04Yes);
			groupBox05_04.Controls.Add(label1);
			resources.ApplyResources(groupBox05_04, "groupBox05_04");
			groupBox05_04.Name = "groupBox05_04";
			groupBox05_04.TabStop = false;
			resources.ApplyResources(label12, "label12");
			label12.Name = "label12";
			resources.ApplyResources(radioButton05_04No, "radioButton05_04No");
			radioButton05_04No.Name = "radioButton05_04No";
			radioButton05_04No.TabStop = true;
			radioButton05_04No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_04Yes, "radioButton05_04Yes");
			radioButton05_04Yes.Name = "radioButton05_04Yes";
			radioButton05_04Yes.TabStop = true;
			radioButton05_04Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			groupBox05_03.Controls.Add(radioButton05_03No);
			groupBox05_03.Controls.Add(radioButton05_03Yes);
			groupBox05_03.Controls.Add(label9);
			resources.ApplyResources(groupBox05_03, "groupBox05_03");
			groupBox05_03.Name = "groupBox05_03";
			groupBox05_03.TabStop = false;
			resources.ApplyResources(radioButton05_03No, "radioButton05_03No");
			radioButton05_03No.Name = "radioButton05_03No";
			radioButton05_03No.TabStop = true;
			radioButton05_03No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_03Yes, "radioButton05_03Yes");
			radioButton05_03Yes.Name = "radioButton05_03Yes";
			radioButton05_03Yes.TabStop = true;
			radioButton05_03Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label9, "label9");
			label9.Name = "label9";
			groupBox05_02.Controls.Add(radioButton05_02No);
			groupBox05_02.Controls.Add(radioButton05_02Yes);
			groupBox05_02.Controls.Add(label8);
			resources.ApplyResources(groupBox05_02, "groupBox05_02");
			groupBox05_02.Name = "groupBox05_02";
			groupBox05_02.TabStop = false;
			resources.ApplyResources(radioButton05_02No, "radioButton05_02No");
			radioButton05_02No.Name = "radioButton05_02No";
			radioButton05_02No.TabStop = true;
			radioButton05_02No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_02Yes, "radioButton05_02Yes");
			radioButton05_02Yes.Name = "radioButton05_02Yes";
			radioButton05_02Yes.TabStop = true;
			radioButton05_02Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label8, "label8");
			label8.Name = "label8";
			groupBox05_01.Controls.Add(radioButton05_01No);
			groupBox05_01.Controls.Add(radioButton05_01Yes);
			groupBox05_01.Controls.Add(label6);
			resources.ApplyResources(groupBox05_01, "groupBox05_01");
			groupBox05_01.Name = "groupBox05_01";
			groupBox05_01.TabStop = false;
			resources.ApplyResources(radioButton05_01No, "radioButton05_01No");
			radioButton05_01No.Name = "radioButton05_01No";
			radioButton05_01No.TabStop = true;
			radioButton05_01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton05_01Yes, "radioButton05_01Yes");
			radioButton05_01Yes.Name = "radioButton05_01Yes";
			radioButton05_01Yes.TabStop = true;
			radioButton05_01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label6, "label6");
			label6.Name = "label6";
			resources.ApplyResources(radioButton05No, "radioButton05No");
			radioButton05No.Name = "radioButton05No";
			radioButton05No.TabStop = true;
			radioButton05No.UseVisualStyleBackColor = true;
			radioButton05No.CheckedChanged += new System.EventHandler(radioButton05No_CheckedChanged);
			resources.ApplyResources(radioButton05Yes, "radioButton05Yes");
			radioButton05Yes.Name = "radioButton05Yes";
			radioButton05Yes.TabStop = true;
			radioButton05Yes.UseVisualStyleBackColor = true;
			radioButton05Yes.CheckedChanged += new System.EventHandler(radioButton05Yes_CheckedChanged);
			resources.ApplyResources(label7, "label7");
			label7.Name = "label7";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox05);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormViolence02";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormViolence02_FormClosing);
			groupBox05.ResumeLayout(false);
			groupBox05.PerformLayout();
			groupBox05_07.ResumeLayout(false);
			groupBox05_07.PerformLayout();
			groupBox05_06.ResumeLayout(false);
			groupBox05_06.PerformLayout();
			groupBox05_05.ResumeLayout(false);
			groupBox05_05.PerformLayout();
			groupBox05_04.ResumeLayout(false);
			groupBox05_04.PerformLayout();
			groupBox05_03.ResumeLayout(false);
			groupBox05_03.PerformLayout();
			groupBox05_02.ResumeLayout(false);
			groupBox05_02.PerformLayout();
			groupBox05_01.ResumeLayout(false);
			groupBox05_01.PerformLayout();
			ResumeLayout(false);
		}
	}
}
