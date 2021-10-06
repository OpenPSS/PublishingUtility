using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormOnline : Form
	{
		private bool bNextButton;

		public static readonly object[] objOnlineMinimumAge = new object[21]
		{
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20"
		};

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

		private GroupBox groupBox3;

		private RadioButton radioButton03No;

		private RadioButton radioButton03Yes;

		private Label label3;

		private GroupBox groupBox4;

		private RadioButton radioButton04No;

		private RadioButton radioButton04Yes;

		private Label label4;

		private GroupBox groupBox5;

		private RadioButton radioButton05No;

		private RadioButton radioButton05Yes;

		private Label label5;

		private Label label6;

		private ComboBox comboBoxMinimumAge;

		public RatingFormOnline()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			comboBoxMinimumAge.Items.AddRange(objOnlineMinimumAge);
			comboBoxMinimumAge.SelectedIndex = 0;
			if (Program._RatingData.IsOnlineQ01)
			{
				radioButton01Yes.Checked = true;
				radioButton01No.Checked = false;
			}
			else
			{
				radioButton01Yes.Checked = false;
				radioButton01No.Checked = true;
			}
			if (Program._RatingData.IsOnlineQ02)
			{
				radioButton02Yes.Checked = true;
				radioButton02No.Checked = false;
			}
			else
			{
				radioButton02Yes.Checked = false;
				radioButton02No.Checked = true;
			}
			if (Program._RatingData.IsOnlineQ03)
			{
				radioButton03Yes.Checked = true;
				radioButton03No.Checked = false;
			}
			else
			{
				radioButton03Yes.Checked = false;
				radioButton03No.Checked = true;
			}
			if (Program._RatingData.IsOnlineQ04)
			{
				radioButton04Yes.Checked = true;
				radioButton04No.Checked = false;
			}
			else
			{
				radioButton04Yes.Checked = false;
				radioButton04No.Checked = true;
			}
			if (Program._RatingData.IsOnlineQ05)
			{
				radioButton05Yes.Checked = true;
				radioButton05No.Checked = false;
				comboBoxMinimumAge.Enabled = true;
			}
			else
			{
				radioButton05Yes.Checked = false;
				radioButton05No.Checked = true;
				comboBoxMinimumAge.Enabled = false;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01Yes.Checked)
			{
				Program._RatingData.IsOnlineQ01 = true;
			}
			else
			{
				Program._RatingData.IsOnlineQ01 = false;
			}
			if (radioButton02Yes.Checked)
			{
				Program._RatingData.IsOnlineQ02 = true;
			}
			else
			{
				Program._RatingData.IsOnlineQ02 = false;
			}
			if (radioButton03Yes.Checked)
			{
				Program._RatingData.IsOnlineQ03 = true;
			}
			else
			{
				Program._RatingData.IsOnlineQ03 = false;
			}
			if (radioButton04Yes.Checked)
			{
				Program._RatingData.IsOnlineQ04 = true;
			}
			else
			{
				Program._RatingData.IsOnlineQ04 = false;
			}
			if (radioButton05Yes.Checked)
			{
				Program._RatingData.IsOnlineQ05 = true;
				Program._RatingData.CsOnlineMinimumAge = comboBoxMinimumAge.SelectedIndex;
			}
			else
			{
				Program._RatingData.IsOnlineQ05 = false;
				Program._RatingData.CsOnlineMinimumAge = 0;
			}
		}

		private void RatingFormOnline_FormClosing(object sender, FormClosingEventArgs e)
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
				comboBoxMinimumAge.Enabled = true;
			}
		}

		private void radioButton05No_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton05No.Checked)
			{
				comboBoxMinimumAge.Enabled = false;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormOnline));
			buttonNext = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			radioButton01No = new System.Windows.Forms.RadioButton();
			radioButton01Yes = new System.Windows.Forms.RadioButton();
			label1 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			radioButton02No = new System.Windows.Forms.RadioButton();
			radioButton02Yes = new System.Windows.Forms.RadioButton();
			label2 = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			radioButton03No = new System.Windows.Forms.RadioButton();
			radioButton03Yes = new System.Windows.Forms.RadioButton();
			label3 = new System.Windows.Forms.Label();
			groupBox4 = new System.Windows.Forms.GroupBox();
			radioButton04No = new System.Windows.Forms.RadioButton();
			radioButton04Yes = new System.Windows.Forms.RadioButton();
			label4 = new System.Windows.Forms.Label();
			groupBox5 = new System.Windows.Forms.GroupBox();
			comboBoxMinimumAge = new System.Windows.Forms.ComboBox();
			label6 = new System.Windows.Forms.Label();
			radioButton05No = new System.Windows.Forms.RadioButton();
			radioButton05Yes = new System.Windows.Forms.RadioButton();
			label5 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox3.SuspendLayout();
			groupBox4.SuspendLayout();
			groupBox5.SuspendLayout();
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
			groupBox2.Controls.Add(radioButton02No);
			groupBox2.Controls.Add(radioButton02Yes);
			groupBox2.Controls.Add(label2);
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
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
			groupBox3.Controls.Add(radioButton03No);
			groupBox3.Controls.Add(radioButton03Yes);
			groupBox3.Controls.Add(label3);
			resources.ApplyResources(groupBox3, "groupBox3");
			groupBox3.Name = "groupBox3";
			groupBox3.TabStop = false;
			resources.ApplyResources(radioButton03No, "radioButton03No");
			radioButton03No.Name = "radioButton03No";
			radioButton03No.TabStop = true;
			radioButton03No.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton03Yes, "radioButton03Yes");
			radioButton03Yes.Name = "radioButton03Yes";
			radioButton03Yes.TabStop = true;
			radioButton03Yes.UseVisualStyleBackColor = true;
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			groupBox4.Controls.Add(radioButton04No);
			groupBox4.Controls.Add(radioButton04Yes);
			groupBox4.Controls.Add(label4);
			resources.ApplyResources(groupBox4, "groupBox4");
			groupBox4.Name = "groupBox4";
			groupBox4.TabStop = false;
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
			groupBox5.Controls.Add(comboBoxMinimumAge);
			groupBox5.Controls.Add(label6);
			groupBox5.Controls.Add(radioButton05No);
			groupBox5.Controls.Add(radioButton05Yes);
			groupBox5.Controls.Add(label5);
			resources.ApplyResources(groupBox5, "groupBox5");
			groupBox5.Name = "groupBox5";
			groupBox5.TabStop = false;
			comboBoxMinimumAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxMinimumAge.FormattingEnabled = true;
			resources.ApplyResources(comboBoxMinimumAge, "comboBoxMinimumAge");
			comboBoxMinimumAge.Name = "comboBoxMinimumAge";
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
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox5);
			base.Controls.Add(groupBox4);
			base.Controls.Add(groupBox3);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormOnline";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormOnline_FormClosing);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			groupBox4.ResumeLayout(false);
			groupBox4.PerformLayout();
			groupBox5.ResumeLayout(false);
			groupBox5.PerformLayout();
			ResumeLayout(false);
		}
	}
}
