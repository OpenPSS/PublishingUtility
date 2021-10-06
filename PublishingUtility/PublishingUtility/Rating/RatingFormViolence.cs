using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormViolence : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Button buttonNext;

		private GroupBox groupBox01;

		private GroupBox groupBox01_01;

		private RadioButton radioButton01_01No;

		private RadioButton radioButton01_01Yes;

		private Label label4;

		private RadioButton radioButton01No;

		private RadioButton radioButton01Yes;

		private Label label2;

		private GroupBox groupBox02;

		private RadioButton radioButton02No;

		private RadioButton radioButton02Yes;

		private Label label1;

		private GroupBox groupBox03;

		private GroupBox groupBox03_01;

		private RadioButton radioButton03_01No;

		private RadioButton radioButton03_01Yes;

		private Label label5;

		private RadioButton radioButton03No;

		private RadioButton radioButton03Yes;

		private Label label3;

		private GroupBox groupBox04;

		private GroupBox groupBox04_03;

		private RadioButton radioButton04_03No;

		private RadioButton radioButton04_03Yes;

		private Label label9;

		private GroupBox groupBox04_02;

		private RadioButton radioButton04_02No;

		private RadioButton radioButton04_02Yes;

		private Label label8;

		private GroupBox groupBox04_01;

		private RadioButton radioButton04_01No;

		private RadioButton radioButton04_01Yes;

		private Label label6;

		private RadioButton radioButton04No;

		private RadioButton radioButton04Yes;

		private Label label7;

		public RatingFormViolence()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.IsViolenceQ01)
			{
				radioButton01Yes.Checked = true;
				radioButton01No.Checked = false;
			}
			else
			{
				radioButton01Yes.Checked = false;
				radioButton01No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ01_01)
			{
				radioButton01_01Yes.Checked = true;
				radioButton01_01No.Checked = false;
			}
			else
			{
				radioButton01_01Yes.Checked = false;
				radioButton01_01No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ02)
			{
				radioButton02Yes.Checked = true;
				radioButton02No.Checked = false;
			}
			else
			{
				radioButton02Yes.Checked = false;
				radioButton02No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ03)
			{
				radioButton03Yes.Checked = true;
				radioButton03No.Checked = false;
			}
			else
			{
				radioButton03Yes.Checked = false;
				radioButton03No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ03_01)
			{
				radioButton03_01Yes.Checked = true;
				radioButton03_01No.Checked = false;
			}
			else
			{
				radioButton03_01Yes.Checked = false;
				radioButton03_01No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ04)
			{
				radioButton04Yes.Checked = true;
				radioButton04No.Checked = false;
			}
			else
			{
				radioButton04Yes.Checked = false;
				radioButton04No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ04_01)
			{
				radioButton04_01Yes.Checked = true;
				radioButton04_01No.Checked = false;
			}
			else
			{
				radioButton04_01Yes.Checked = false;
				radioButton04_01No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ04_02)
			{
				radioButton04_02Yes.Checked = true;
				radioButton04_02No.Checked = false;
			}
			else
			{
				radioButton04_02Yes.Checked = false;
				radioButton04_02No.Checked = true;
			}
			if (Program._RatingData.IsViolenceQ04_03)
			{
				radioButton04_03Yes.Checked = true;
				radioButton04_03No.Checked = false;
			}
			else
			{
				radioButton04_03Yes.Checked = false;
				radioButton04_03No.Checked = true;
			}
			if (radioButton01Yes.Checked)
			{
				groupBox01_01.Enabled = true;
			}
			else
			{
				groupBox01_01.Enabled = false;
			}
			if (radioButton03Yes.Checked)
			{
				groupBox03_01.Enabled = true;
			}
			else
			{
				groupBox03_01.Enabled = false;
			}
			if (radioButton04Yes.Checked)
			{
				groupBox04_01.Enabled = true;
				groupBox04_02.Enabled = true;
				groupBox04_03.Enabled = true;
			}
			else
			{
				groupBox04_01.Enabled = false;
				groupBox04_02.Enabled = false;
				groupBox04_03.Enabled = false;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01Yes.Checked)
			{
				Program._RatingData.IsViolenceQ01 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ01 = false;
			}
			if (radioButton01_01Yes.Checked)
			{
				Program._RatingData.IsViolenceQ01_01 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ01_01 = false;
			}
			if (radioButton02Yes.Checked)
			{
				Program._RatingData.IsViolenceQ02 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ02 = false;
			}
			if (radioButton03Yes.Checked)
			{
				Program._RatingData.IsViolenceQ03 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ03 = false;
			}
			if (radioButton03_01Yes.Checked)
			{
				Program._RatingData.IsViolenceQ03_01 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ03_01 = false;
			}
			if (radioButton04Yes.Checked)
			{
				Program._RatingData.IsViolenceQ04 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ04 = false;
			}
			if (radioButton04_01Yes.Checked)
			{
				Program._RatingData.IsViolenceQ04_01 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ04_01 = false;
			}
			if (radioButton04_02Yes.Checked)
			{
				Program._RatingData.IsViolenceQ04_02 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ04_02 = false;
			}
			if (radioButton04_03Yes.Checked)
			{
				Program._RatingData.IsViolenceQ04_03 = true;
			}
			else
			{
				Program._RatingData.IsViolenceQ04_03 = false;
			}
		}

		private void RatingFormViolence_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (sender == this && !bNextButton)
			{
				SceRatingData.CheckCloseWindow(this, e);
			}
		}

		private void radioButton01Yes_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton01Yes.Checked)
			{
				groupBox01_01.Enabled = true;
			}
			else
			{
				groupBox01_01.Enabled = false;
			}
		}

		private void radioButton01No_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton01Yes.Checked)
			{
				groupBox01_01.Enabled = true;
			}
			else
			{
				groupBox01_01.Enabled = false;
			}
		}

		private void radioButton03Yes_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton03Yes.Checked)
			{
				groupBox03_01.Enabled = true;
			}
			else
			{
				groupBox03_01.Enabled = false;
			}
		}

		private void radioButton03No_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton03Yes.Checked)
			{
				groupBox03_01.Enabled = true;
			}
			else
			{
				groupBox03_01.Enabled = false;
			}
		}

		private void radioButton04Yes_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton04Yes.Checked)
			{
				groupBox04_01.Enabled = true;
				groupBox04_02.Enabled = true;
				groupBox04_03.Enabled = true;
			}
			else
			{
				groupBox04_01.Enabled = false;
				groupBox04_02.Enabled = false;
				groupBox04_03.Enabled = false;
			}
		}

		private void radioButton04No_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton04Yes.Checked)
			{
				groupBox04_01.Enabled = true;
				groupBox04_02.Enabled = true;
				groupBox04_03.Enabled = true;
			}
			else
			{
				groupBox04_01.Enabled = false;
				groupBox04_02.Enabled = false;
				groupBox04_03.Enabled = false;
			}
		}

		private void groupBox01_Enter(object sender, EventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormViolence));
			buttonNext = new System.Windows.Forms.Button();
			groupBox01 = new System.Windows.Forms.GroupBox();
			groupBox01_01 = new System.Windows.Forms.GroupBox();
			radioButton01_01No = new System.Windows.Forms.RadioButton();
			radioButton01_01Yes = new System.Windows.Forms.RadioButton();
			label4 = new System.Windows.Forms.Label();
			radioButton01No = new System.Windows.Forms.RadioButton();
			radioButton01Yes = new System.Windows.Forms.RadioButton();
			label2 = new System.Windows.Forms.Label();
			groupBox02 = new System.Windows.Forms.GroupBox();
			radioButton02No = new System.Windows.Forms.RadioButton();
			radioButton02Yes = new System.Windows.Forms.RadioButton();
			label1 = new System.Windows.Forms.Label();
			groupBox03 = new System.Windows.Forms.GroupBox();
			groupBox03_01 = new System.Windows.Forms.GroupBox();
			radioButton03_01No = new System.Windows.Forms.RadioButton();
			radioButton03_01Yes = new System.Windows.Forms.RadioButton();
			label5 = new System.Windows.Forms.Label();
			radioButton03No = new System.Windows.Forms.RadioButton();
			radioButton03Yes = new System.Windows.Forms.RadioButton();
			label3 = new System.Windows.Forms.Label();
			groupBox04 = new System.Windows.Forms.GroupBox();
			groupBox04_03 = new System.Windows.Forms.GroupBox();
			radioButton04_03No = new System.Windows.Forms.RadioButton();
			radioButton04_03Yes = new System.Windows.Forms.RadioButton();
			label9 = new System.Windows.Forms.Label();
			groupBox04_02 = new System.Windows.Forms.GroupBox();
			radioButton04_02No = new System.Windows.Forms.RadioButton();
			radioButton04_02Yes = new System.Windows.Forms.RadioButton();
			label8 = new System.Windows.Forms.Label();
			groupBox04_01 = new System.Windows.Forms.GroupBox();
			radioButton04_01No = new System.Windows.Forms.RadioButton();
			radioButton04_01Yes = new System.Windows.Forms.RadioButton();
			label6 = new System.Windows.Forms.Label();
			radioButton04No = new System.Windows.Forms.RadioButton();
			radioButton04Yes = new System.Windows.Forms.RadioButton();
			label7 = new System.Windows.Forms.Label();
			groupBox01.SuspendLayout();
			groupBox01_01.SuspendLayout();
			groupBox02.SuspendLayout();
			groupBox03.SuspendLayout();
			groupBox03_01.SuspendLayout();
			groupBox04.SuspendLayout();
			groupBox04_03.SuspendLayout();
			groupBox04_02.SuspendLayout();
			groupBox04_01.SuspendLayout();
			SuspendLayout();
			resources.ApplyResources(buttonNext, "buttonNext");
			buttonNext.Name = "buttonNext";
			buttonNext.UseVisualStyleBackColor = true;
			buttonNext.Click += new System.EventHandler(buttonNext_Click);
			groupBox01.Controls.Add(groupBox01_01);
			groupBox01.Controls.Add(radioButton01No);
			groupBox01.Controls.Add(radioButton01Yes);
			groupBox01.Controls.Add(label2);
			resources.ApplyResources(groupBox01, "groupBox01");
			groupBox01.Name = "groupBox01";
			groupBox01.TabStop = false;
			groupBox01.Enter += new System.EventHandler(groupBox01_Enter);
			groupBox01_01.Controls.Add(radioButton01_01No);
			groupBox01_01.Controls.Add(radioButton01_01Yes);
			groupBox01_01.Controls.Add(label4);
			resources.ApplyResources(groupBox01_01, "groupBox01_01");
			groupBox01_01.Name = "groupBox01_01";
			groupBox01_01.TabStop = false;
			resources.ApplyResources(radioButton01_01No, "radioButton01_01No");
			radioButton01_01No.Name = "radioButton01_01No";
			radioButton01_01No.TabStop = true;
			radioButton01_01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton01_01Yes, "radioButton01_01Yes");
			radioButton01_01Yes.Name = "radioButton01_01Yes";
			radioButton01_01Yes.TabStop = true;
			radioButton01_01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label4, "label4");
			label4.Name = "label4";
			resources.ApplyResources(radioButton01No, "radioButton01No");
			radioButton01No.Name = "radioButton01No";
			radioButton01No.TabStop = true;
			radioButton01No.UseVisualStyleBackColor = true;
			radioButton01No.CheckedChanged += new System.EventHandler(radioButton01No_CheckedChanged);
			resources.ApplyResources(radioButton01Yes, "radioButton01Yes");
			radioButton01Yes.Name = "radioButton01Yes";
			radioButton01Yes.TabStop = true;
			radioButton01Yes.UseVisualStyleBackColor = true;
			radioButton01Yes.CheckedChanged += new System.EventHandler(radioButton01Yes_CheckedChanged);
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
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
			groupBox03.Controls.Add(groupBox03_01);
			groupBox03.Controls.Add(radioButton03No);
			groupBox03.Controls.Add(radioButton03Yes);
			groupBox03.Controls.Add(label3);
			resources.ApplyResources(groupBox03, "groupBox03");
			groupBox03.Name = "groupBox03";
			groupBox03.TabStop = false;
			groupBox03_01.Controls.Add(radioButton03_01No);
			groupBox03_01.Controls.Add(radioButton03_01Yes);
			groupBox03_01.Controls.Add(label5);
			resources.ApplyResources(groupBox03_01, "groupBox03_01");
			groupBox03_01.Name = "groupBox03_01";
			groupBox03_01.TabStop = false;
			resources.ApplyResources(radioButton03_01No, "radioButton03_01No");
			radioButton03_01No.Name = "radioButton03_01No";
			radioButton03_01No.TabStop = true;
			radioButton03_01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton03_01Yes, "radioButton03_01Yes");
			radioButton03_01Yes.Name = "radioButton03_01Yes";
			radioButton03_01Yes.TabStop = true;
			radioButton03_01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			resources.ApplyResources(radioButton03No, "radioButton03No");
			radioButton03No.Name = "radioButton03No";
			radioButton03No.TabStop = true;
			radioButton03No.UseVisualStyleBackColor = true;
			radioButton03No.CheckedChanged += new System.EventHandler(radioButton03No_CheckedChanged);
			resources.ApplyResources(radioButton03Yes, "radioButton03Yes");
			radioButton03Yes.Name = "radioButton03Yes";
			radioButton03Yes.TabStop = true;
			radioButton03Yes.UseVisualStyleBackColor = true;
			radioButton03Yes.CheckedChanged += new System.EventHandler(radioButton03Yes_CheckedChanged);
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			groupBox04.Controls.Add(groupBox04_03);
			groupBox04.Controls.Add(groupBox04_02);
			groupBox04.Controls.Add(groupBox04_01);
			groupBox04.Controls.Add(radioButton04No);
			groupBox04.Controls.Add(radioButton04Yes);
			groupBox04.Controls.Add(label7);
			resources.ApplyResources(groupBox04, "groupBox04");
			groupBox04.Name = "groupBox04";
			groupBox04.TabStop = false;
			groupBox04_03.Controls.Add(radioButton04_03No);
			groupBox04_03.Controls.Add(radioButton04_03Yes);
			groupBox04_03.Controls.Add(label9);
			resources.ApplyResources(groupBox04_03, "groupBox04_03");
			groupBox04_03.Name = "groupBox04_03";
			groupBox04_03.TabStop = false;
			resources.ApplyResources(radioButton04_03No, "radioButton04_03No");
			radioButton04_03No.Name = "radioButton04_03No";
			radioButton04_03No.TabStop = true;
			radioButton04_03No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton04_03Yes, "radioButton04_03Yes");
			radioButton04_03Yes.Name = "radioButton04_03Yes";
			radioButton04_03Yes.TabStop = true;
			radioButton04_03Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label9, "label9");
			label9.Name = "label9";
			groupBox04_02.Controls.Add(radioButton04_02No);
			groupBox04_02.Controls.Add(radioButton04_02Yes);
			groupBox04_02.Controls.Add(label8);
			resources.ApplyResources(groupBox04_02, "groupBox04_02");
			groupBox04_02.Name = "groupBox04_02";
			groupBox04_02.TabStop = false;
			resources.ApplyResources(radioButton04_02No, "radioButton04_02No");
			radioButton04_02No.Name = "radioButton04_02No";
			radioButton04_02No.TabStop = true;
			radioButton04_02No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton04_02Yes, "radioButton04_02Yes");
			radioButton04_02Yes.Name = "radioButton04_02Yes";
			radioButton04_02Yes.TabStop = true;
			radioButton04_02Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label8, "label8");
			label8.Name = "label8";
			groupBox04_01.Controls.Add(radioButton04_01No);
			groupBox04_01.Controls.Add(radioButton04_01Yes);
			groupBox04_01.Controls.Add(label6);
			resources.ApplyResources(groupBox04_01, "groupBox04_01");
			groupBox04_01.Name = "groupBox04_01";
			groupBox04_01.TabStop = false;
			resources.ApplyResources(radioButton04_01No, "radioButton04_01No");
			radioButton04_01No.Name = "radioButton04_01No";
			radioButton04_01No.TabStop = true;
			radioButton04_01No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton04_01Yes, "radioButton04_01Yes");
			radioButton04_01Yes.Name = "radioButton04_01Yes";
			radioButton04_01Yes.TabStop = true;
			radioButton04_01Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label6, "label6");
			label6.Name = "label6";
			resources.ApplyResources(radioButton04No, "radioButton04No");
			radioButton04No.Name = "radioButton04No";
			radioButton04No.TabStop = true;
			radioButton04No.UseVisualStyleBackColor = true;
			radioButton04No.CheckedChanged += new System.EventHandler(radioButton04No_CheckedChanged);
			resources.ApplyResources(radioButton04Yes, "radioButton04Yes");
			radioButton04Yes.Name = "radioButton04Yes";
			radioButton04Yes.TabStop = true;
			radioButton04Yes.UseVisualStyleBackColor = true;
			radioButton04Yes.CheckedChanged += new System.EventHandler(radioButton04Yes_CheckedChanged);
			resources.ApplyResources(label7, "label7");
			label7.Name = "label7";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox04);
			base.Controls.Add(groupBox03);
			base.Controls.Add(groupBox02);
			base.Controls.Add(groupBox01);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormViolence";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormViolence_FormClosing);
			groupBox01.ResumeLayout(false);
			groupBox01.PerformLayout();
			groupBox01_01.ResumeLayout(false);
			groupBox01_01.PerformLayout();
			groupBox02.ResumeLayout(false);
			groupBox02.PerformLayout();
			groupBox03.ResumeLayout(false);
			groupBox03.PerformLayout();
			groupBox03_01.ResumeLayout(false);
			groupBox03_01.PerformLayout();
			groupBox04.ResumeLayout(false);
			groupBox04.PerformLayout();
			groupBox04_03.ResumeLayout(false);
			groupBox04_03.PerformLayout();
			groupBox04_02.ResumeLayout(false);
			groupBox04_02.PerformLayout();
			groupBox04_01.ResumeLayout(false);
			groupBox04_01.PerformLayout();
			ResumeLayout(false);
		}
	}
}
