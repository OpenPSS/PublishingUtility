using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PublishingUtility.Dialog
{
	public class TextInputForm : Form
	{
		private IContainer components;

		private TextBox textBox1;

		private Label label1;

		private Button buttonOK;

		private Button buttonCancel;

		public string Label
		{
			set
			{
				label1.Text = value;
			}
		}

		public TextBox TextBox => textBox1;

		public TextInputForm()
		{
			InitializeComponent();
			base.StartPosition = FormStartPosition.CenterScreen;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
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
			textBox1 = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			textBox1.Location = new System.Drawing.Point(14, 24);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(260, 19);
			textBox1.TabIndex = 0;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 12);
			label1.TabIndex = 1;
			label1.Text = "label1";
			buttonOK.Location = new System.Drawing.Point(64, 49);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 2;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(145, 49);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 3;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(284, 86);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(label1);
			base.Controls.Add(textBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "TextInputForm";
			Text = "TextInputForm";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
