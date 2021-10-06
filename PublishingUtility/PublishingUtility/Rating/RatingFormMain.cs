using System;
using System.ComponentModel;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility.Rating
{
	public class RatingFormMain : Form
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

		private GroupBox groupBox6;

		private RadioButton radioButton06No;

		private RadioButton radioButton06Yes;

		private Label label6;

		private GroupBox groupBox7;

		private RadioButton radioButton07No;

		private RadioButton radioButton07Yes;

		private Label label7;

		private GroupBox groupBox8;

		private RadioButton radioButton08No;

		private RadioButton radioButton08Yes;

		private Label label8;

		private GroupBox groupBox9;

		private RadioButton radioButton09No;

		private RadioButton radioButton09Yes;

		private Label label9;

		private Label label10;

		public RatingFormMain()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.IsMainScaryElements01)
			{
				radioButton01Yes.Checked = true;
				radioButton01No.Checked = false;
			}
			else
			{
				radioButton01Yes.Checked = false;
				radioButton01No.Checked = true;
			}
			if (Program._RatingData.IsMainLanguage01)
			{
				radioButton02Yes.Checked = true;
				radioButton02No.Checked = false;
			}
			else
			{
				radioButton02Yes.Checked = false;
				radioButton02No.Checked = true;
			}
			if (Program._RatingData.IsMainViolence01)
			{
				radioButton03Yes.Checked = true;
				radioButton03No.Checked = false;
			}
			else
			{
				radioButton03Yes.Checked = false;
				radioButton03No.Checked = true;
			}
			if (Program._RatingData.IsMainCrime01)
			{
				radioButton04Yes.Checked = true;
				radioButton04No.Checked = false;
			}
			else
			{
				radioButton04Yes.Checked = false;
				radioButton04No.Checked = true;
			}
			if (Program._RatingData.IsMainGambling01)
			{
				radioButton05Yes.Checked = true;
				radioButton05No.Checked = false;
			}
			else
			{
				radioButton05Yes.Checked = false;
				radioButton05No.Checked = true;
			}
			if (Program._RatingData.IsMainDiscrimination01)
			{
				radioButton06Yes.Checked = true;
				radioButton06No.Checked = false;
			}
			else
			{
				radioButton06Yes.Checked = false;
				radioButton06No.Checked = true;
			}
			if (Program._RatingData.IsMainOnline01)
			{
				radioButton07Yes.Checked = true;
				radioButton07No.Checked = false;
			}
			else
			{
				radioButton07Yes.Checked = false;
				radioButton07No.Checked = true;
			}
			if (Program._RatingData.IsMainDrugs01)
			{
				radioButton08Yes.Checked = true;
				radioButton08No.Checked = false;
			}
			else
			{
				radioButton08Yes.Checked = false;
				radioButton08No.Checked = true;
			}
			if (Program._RatingData.IsMainSex01)
			{
				radioButton09Yes.Checked = true;
				radioButton09No.Checked = false;
			}
			else
			{
				radioButton09Yes.Checked = false;
				radioButton09No.Checked = true;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01Yes.Checked)
			{
				Program._RatingData.IsMainScaryElements01 = true;
			}
			else
			{
				Program._RatingData.IsMainScaryElements01 = false;
			}
			if (radioButton02Yes.Checked)
			{
				Program._RatingData.IsMainLanguage01 = true;
			}
			else
			{
				Program._RatingData.IsMainLanguage01 = false;
			}
			if (radioButton03Yes.Checked)
			{
				Program._RatingData.IsMainViolence01 = true;
			}
			else
			{
				Program._RatingData.IsMainViolence01 = false;
			}
			if (radioButton04Yes.Checked)
			{
				Program._RatingData.IsMainCrime01 = true;
			}
			else
			{
				Program._RatingData.IsMainCrime01 = false;
			}
			if (radioButton05Yes.Checked)
			{
				Program._RatingData.IsMainGambling01 = true;
			}
			else
			{
				Program._RatingData.IsMainGambling01 = false;
			}
			if (radioButton06Yes.Checked)
			{
				Program._RatingData.IsMainDiscrimination01 = true;
			}
			else
			{
				Program._RatingData.IsMainDiscrimination01 = false;
			}
			if (radioButton07Yes.Checked)
			{
				Program._RatingData.IsMainOnline01 = true;
			}
			else
			{
				Program._RatingData.IsMainOnline01 = false;
			}
			if (radioButton08Yes.Checked)
			{
				Program._RatingData.IsMainDrugs01 = true;
			}
			else
			{
				Program._RatingData.IsMainDrugs01 = false;
			}
			if (radioButton09Yes.Checked)
			{
				Program._RatingData.IsMainSex01 = true;
			}
			else
			{
				Program._RatingData.IsMainSex01 = false;
			}
		}

		private void radioButton01Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton01No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton02Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton02No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton03Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton03No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton04Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton04No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton05Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton05No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton06Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton06No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton07Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton07No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton08Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton08No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton09Yes_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void radioButton09No_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void RatingFormMain_FormClosing(object sender, FormClosingEventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormMain));
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
			radioButton05No = new System.Windows.Forms.RadioButton();
			radioButton05Yes = new System.Windows.Forms.RadioButton();
			label5 = new System.Windows.Forms.Label();
			groupBox6 = new System.Windows.Forms.GroupBox();
			label10 = new System.Windows.Forms.Label();
			radioButton06No = new System.Windows.Forms.RadioButton();
			radioButton06Yes = new System.Windows.Forms.RadioButton();
			label6 = new System.Windows.Forms.Label();
			groupBox7 = new System.Windows.Forms.GroupBox();
			radioButton07No = new System.Windows.Forms.RadioButton();
			radioButton07Yes = new System.Windows.Forms.RadioButton();
			label7 = new System.Windows.Forms.Label();
			groupBox8 = new System.Windows.Forms.GroupBox();
			radioButton08No = new System.Windows.Forms.RadioButton();
			radioButton08Yes = new System.Windows.Forms.RadioButton();
			label8 = new System.Windows.Forms.Label();
			groupBox9 = new System.Windows.Forms.GroupBox();
			radioButton09No = new System.Windows.Forms.RadioButton();
			radioButton09Yes = new System.Windows.Forms.RadioButton();
			label9 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox3.SuspendLayout();
			groupBox4.SuspendLayout();
			groupBox5.SuspendLayout();
			groupBox6.SuspendLayout();
			groupBox7.SuspendLayout();
			groupBox8.SuspendLayout();
			groupBox9.SuspendLayout();
			SuspendLayout();
			buttonNext.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
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
			radioButton01No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton01No.Name = "radioButton01No";
			radioButton01No.TabStop = true;
			radioButton01No.UseVisualStyleBackColor = true;
			radioButton01No.CheckedChanged += new System.EventHandler(radioButton01No_CheckedChanged);
			resources.ApplyResources(radioButton01Yes, "radioButton01Yes");
			radioButton01Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton01Yes.Name = "radioButton01Yes";
			radioButton01Yes.TabStop = true;
			radioButton01Yes.UseVisualStyleBackColor = true;
			radioButton01Yes.CheckedChanged += new System.EventHandler(radioButton01Yes_CheckedChanged);
			resources.ApplyResources(label1, "label1");
			label1.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label1.Name = "label1";
			groupBox2.Controls.Add(radioButton02No);
			groupBox2.Controls.Add(radioButton02Yes);
			groupBox2.Controls.Add(label2);
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			resources.ApplyResources(radioButton02No, "radioButton02No");
			radioButton02No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton02No.Name = "radioButton02No";
			radioButton02No.TabStop = true;
			radioButton02No.UseVisualStyleBackColor = true;
			radioButton02No.CheckedChanged += new System.EventHandler(radioButton02No_CheckedChanged);
			resources.ApplyResources(radioButton02Yes, "radioButton02Yes");
			radioButton02Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton02Yes.Name = "radioButton02Yes";
			radioButton02Yes.TabStop = true;
			radioButton02Yes.UseVisualStyleBackColor = true;
			radioButton02Yes.CheckedChanged += new System.EventHandler(radioButton02Yes_CheckedChanged);
			resources.ApplyResources(label2, "label2");
			label2.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label2.Name = "label2";
			groupBox3.Controls.Add(radioButton03No);
			groupBox3.Controls.Add(radioButton03Yes);
			groupBox3.Controls.Add(label3);
			resources.ApplyResources(groupBox3, "groupBox3");
			groupBox3.Name = "groupBox3";
			groupBox3.TabStop = false;
			resources.ApplyResources(radioButton03No, "radioButton03No");
			radioButton03No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton03No.Name = "radioButton03No";
			radioButton03No.TabStop = true;
			radioButton03No.UseVisualStyleBackColor = true;
			radioButton03No.CheckedChanged += new System.EventHandler(radioButton03No_CheckedChanged);
			resources.ApplyResources(radioButton03Yes, "radioButton03Yes");
			radioButton03Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton03Yes.Name = "radioButton03Yes";
			radioButton03Yes.TabStop = true;
			radioButton03Yes.UseVisualStyleBackColor = true;
			radioButton03Yes.CheckedChanged += new System.EventHandler(radioButton03Yes_CheckedChanged);
			resources.ApplyResources(label3, "label3");
			label3.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label3.Name = "label3";
			groupBox4.Controls.Add(radioButton04No);
			groupBox4.Controls.Add(radioButton04Yes);
			groupBox4.Controls.Add(label4);
			resources.ApplyResources(groupBox4, "groupBox4");
			groupBox4.Name = "groupBox4";
			groupBox4.TabStop = false;
			resources.ApplyResources(radioButton04No, "radioButton04No");
			radioButton04No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton04No.Name = "radioButton04No";
			radioButton04No.TabStop = true;
			radioButton04No.UseVisualStyleBackColor = true;
			radioButton04No.CheckedChanged += new System.EventHandler(radioButton04No_CheckedChanged);
			resources.ApplyResources(radioButton04Yes, "radioButton04Yes");
			radioButton04Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton04Yes.Name = "radioButton04Yes";
			radioButton04Yes.TabStop = true;
			radioButton04Yes.UseVisualStyleBackColor = true;
			radioButton04Yes.CheckedChanged += new System.EventHandler(radioButton04Yes_CheckedChanged);
			resources.ApplyResources(label4, "label4");
			label4.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label4.Name = "label4";
			groupBox5.Controls.Add(radioButton05No);
			groupBox5.Controls.Add(radioButton05Yes);
			groupBox5.Controls.Add(label5);
			resources.ApplyResources(groupBox5, "groupBox5");
			groupBox5.Name = "groupBox5";
			groupBox5.TabStop = false;
			resources.ApplyResources(radioButton05No, "radioButton05No");
			radioButton05No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton05No.Name = "radioButton05No";
			radioButton05No.TabStop = true;
			radioButton05No.UseVisualStyleBackColor = true;
			radioButton05No.CheckedChanged += new System.EventHandler(radioButton05No_CheckedChanged);
			resources.ApplyResources(radioButton05Yes, "radioButton05Yes");
			radioButton05Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton05Yes.Name = "radioButton05Yes";
			radioButton05Yes.TabStop = true;
			radioButton05Yes.UseVisualStyleBackColor = true;
			radioButton05Yes.CheckedChanged += new System.EventHandler(radioButton05Yes_CheckedChanged);
			resources.ApplyResources(label5, "label5");
			label5.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label5.Name = "label5";
			groupBox6.Controls.Add(label10);
			groupBox6.Controls.Add(radioButton06No);
			groupBox6.Controls.Add(radioButton06Yes);
			groupBox6.Controls.Add(label6);
			resources.ApplyResources(groupBox6, "groupBox6");
			groupBox6.Name = "groupBox6";
			groupBox6.TabStop = false;
			resources.ApplyResources(label10, "label10");
			label10.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label10.Name = "label10";
			resources.ApplyResources(radioButton06No, "radioButton06No");
			radioButton06No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton06No.Name = "radioButton06No";
			radioButton06No.TabStop = true;
			radioButton06No.UseVisualStyleBackColor = true;
			radioButton06No.CheckedChanged += new System.EventHandler(radioButton06No_CheckedChanged);
			resources.ApplyResources(radioButton06Yes, "radioButton06Yes");
			radioButton06Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton06Yes.Name = "radioButton06Yes";
			radioButton06Yes.TabStop = true;
			radioButton06Yes.UseVisualStyleBackColor = true;
			radioButton06Yes.CheckedChanged += new System.EventHandler(radioButton06Yes_CheckedChanged);
			resources.ApplyResources(label6, "label6");
			label6.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label6.Name = "label6";
			groupBox7.Controls.Add(radioButton07No);
			groupBox7.Controls.Add(radioButton07Yes);
			groupBox7.Controls.Add(label7);
			resources.ApplyResources(groupBox7, "groupBox7");
			groupBox7.Name = "groupBox7";
			groupBox7.TabStop = false;
			resources.ApplyResources(radioButton07No, "radioButton07No");
			radioButton07No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton07No.Name = "radioButton07No";
			radioButton07No.TabStop = true;
			radioButton07No.UseVisualStyleBackColor = true;
			radioButton07No.CheckedChanged += new System.EventHandler(radioButton07No_CheckedChanged);
			resources.ApplyResources(radioButton07Yes, "radioButton07Yes");
			radioButton07Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton07Yes.Name = "radioButton07Yes";
			radioButton07Yes.TabStop = true;
			radioButton07Yes.UseVisualStyleBackColor = true;
			radioButton07Yes.CheckedChanged += new System.EventHandler(radioButton07Yes_CheckedChanged);
			resources.ApplyResources(label7, "label7");
			label7.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label7.Name = "label7";
			groupBox8.Controls.Add(radioButton08No);
			groupBox8.Controls.Add(radioButton08Yes);
			groupBox8.Controls.Add(label8);
			resources.ApplyResources(groupBox8, "groupBox8");
			groupBox8.Name = "groupBox8";
			groupBox8.TabStop = false;
			resources.ApplyResources(radioButton08No, "radioButton08No");
			radioButton08No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton08No.Name = "radioButton08No";
			radioButton08No.TabStop = true;
			radioButton08No.UseVisualStyleBackColor = true;
			radioButton08No.CheckedChanged += new System.EventHandler(radioButton08No_CheckedChanged);
			resources.ApplyResources(radioButton08Yes, "radioButton08Yes");
			radioButton08Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton08Yes.Name = "radioButton08Yes";
			radioButton08Yes.TabStop = true;
			radioButton08Yes.UseVisualStyleBackColor = true;
			radioButton08Yes.CheckedChanged += new System.EventHandler(radioButton08Yes_CheckedChanged);
			resources.ApplyResources(label8, "label8");
			label8.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label8.Name = "label8";
			groupBox9.Controls.Add(radioButton09No);
			groupBox9.Controls.Add(radioButton09Yes);
			groupBox9.Controls.Add(label9);
			resources.ApplyResources(groupBox9, "groupBox9");
			groupBox9.Name = "groupBox9";
			groupBox9.TabStop = false;
			resources.ApplyResources(radioButton09No, "radioButton09No");
			radioButton09No.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton09No.Name = "radioButton09No";
			radioButton09No.TabStop = true;
			radioButton09No.UseVisualStyleBackColor = true;
			radioButton09No.CheckedChanged += new System.EventHandler(radioButton09No_CheckedChanged);
			resources.ApplyResources(radioButton09Yes, "radioButton09Yes");
			radioButton09Yes.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			radioButton09Yes.Name = "radioButton09Yes";
			radioButton09Yes.TabStop = true;
			radioButton09Yes.UseVisualStyleBackColor = true;
			radioButton09Yes.CheckedChanged += new System.EventHandler(radioButton09Yes_CheckedChanged);
			resources.ApplyResources(label9, "label9");
			label9.ImageKey = PublishingUtility.Properties.Resources.updateSubmitText;
			label9.Name = "label9";
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox9);
			base.Controls.Add(groupBox8);
			base.Controls.Add(groupBox7);
			base.Controls.Add(groupBox6);
			base.Controls.Add(groupBox5);
			base.Controls.Add(groupBox4);
			base.Controls.Add(groupBox3);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormMain";
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormMain_FormClosing);
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
			groupBox6.ResumeLayout(false);
			groupBox6.PerformLayout();
			groupBox7.ResumeLayout(false);
			groupBox7.PerformLayout();
			groupBox8.ResumeLayout(false);
			groupBox8.PerformLayout();
			groupBox9.ResumeLayout(false);
			groupBox9.PerformLayout();
			ResumeLayout(false);
		}
	}
}
