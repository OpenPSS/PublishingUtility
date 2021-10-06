using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PublishingUtility.Rating
{
	public class RatingFormGambling : Form
	{
		private bool bNextButton;

		private IContainer components;

		private Button buttonNext;

		private GroupBox groupBox2;

		private RadioButton radioButton03;

		private RadioButton radioButton02;

		private RadioButton radioButton01;

		private Label label10;

		public RatingFormGambling()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
			if (Program._RatingData.gambleLevel == SceRatingData.GambleLevel.Level01)
			{
				radioButton01.Checked = true;
				radioButton02.Checked = false;
				radioButton03.Checked = false;
			}
			else if (Program._RatingData.gambleLevel == SceRatingData.GambleLevel.Level02)
			{
				radioButton01.Checked = false;
				radioButton02.Checked = true;
				radioButton03.Checked = false;
			}
			else
			{
				radioButton01.Checked = false;
				radioButton02.Checked = false;
				radioButton03.Checked = true;
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			bNextButton = true;
			if (radioButton01.Checked)
			{
				Program._RatingData.gambleLevel = SceRatingData.GambleLevel.Level01;
			}
			else if (radioButton02.Checked)
			{
				Program._RatingData.gambleLevel = SceRatingData.GambleLevel.Level02;
			}
			else
			{
				Program._RatingData.gambleLevel = SceRatingData.GambleLevel.Level03;
			}
		}

		private void RatingFormGambling_FormClosing(object sender, FormClosingEventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.Rating.RatingFormGambling));
			buttonNext = new System.Windows.Forms.Button();
			groupBox2 = new System.Windows.Forms.GroupBox();
			label10 = new System.Windows.Forms.Label();
			radioButton03 = new System.Windows.Forms.RadioButton();
			radioButton02 = new System.Windows.Forms.RadioButton();
			radioButton01 = new System.Windows.Forms.RadioButton();
			groupBox2.SuspendLayout();
			SuspendLayout();
			resources.ApplyResources(buttonNext, "buttonNext");
			buttonNext.Name = "buttonNext";
			buttonNext.UseVisualStyleBackColor = true;
			buttonNext.Click += new System.EventHandler(buttonNext_Click);
			groupBox2.Controls.Add(label10);
			groupBox2.Controls.Add(radioButton03);
			groupBox2.Controls.Add(radioButton02);
			groupBox2.Controls.Add(radioButton01);
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			resources.ApplyResources(radioButton03, "radioButton03");
			radioButton03.Name = "radioButton03";
			radioButton03.TabStop = true;
			radioButton03.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton02, "radioButton02");
			radioButton02.Name = "radioButton02";
			radioButton02.TabStop = true;
			radioButton02.UseVisualStyleBackColor = true;
			resources.ApplyResources(radioButton01, "radioButton01");
			radioButton01.Name = "radioButton01";
			radioButton01.TabStop = true;
			radioButton01.UseVisualStyleBackColor = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox2);
			base.Controls.Add(buttonNext);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "RatingFormGambling";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(RatingFormGambling_FormClosing);
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
		}
	}
}
