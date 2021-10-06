using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Controls;
using PublishingUtility.Dialog;
using PublishingUtility.KeyManagement;
using PublishingUtility.Properties;
using PublishingUtility.Rating;

namespace PublishingUtility
{
	public class MainForm : Form
	{
		public class WorkerCheckDeviceConnection
		{
			public delegate void DeviceListDelegate(int num);

			private DeviceListDelegate deviceDelegate;

			private volatile bool _shouldStop;

			public void DoWork()
			{
				while (!_shouldStop)
				{
					Thread.Sleep(10);
					if (Program.MainForm == null || !Program.CheckDeviceConnection || (Program.MainForm.GetActiveCategory() != 1 && Program.MainForm.GetActiveCategory() != 2))
					{
						continue;
					}
					int listDevices = PsmDeviceFuncBinding.GetListDevices(out Program.appConfigData.onlineDevices);
					try
					{
						if (deviceDelegate == null)
						{
							deviceDelegate = Program.MainForm.UpdateDeviceList;
						}
						Program.MainForm.Invoke(deviceDelegate, listDevices);
						Thread.Sleep(2000);
					}
					catch (Exception value)
					{
						Console.WriteLine(value);
					}
				}
				Console.WriteLine("thread: FuncCheckDeviceConnection end.");
			}

			public void RequestStop()
			{
				_shouldStop = true;
			}
		}

		private Metadata metadata;

		private ArrayList purchaseSubPanels = new ArrayList();

		private Locale oldLocale = Locale.en_GB;

		private bool isMetaChanged;

		private object checkedUiLangMenuItem;

		private bool kmEdittingNickname;

		private bool kmUpdatingDeviceList;

		private Bitmap keyBitmap = new Bitmap(Resources.KeyIcon);

		private Bitmap androidBitmap = new Bitmap(Resources.DeviceAndroid);

		private Bitmap vitaBitmap = new Bitmap(Resources.DeviceVita);

		private Bitmap vitaOffBitmap = new Bitmap(Resources.DeviceVitaOff);

		private static Bitmap nothingStBitmap = new Bitmap(Resources.NothingIcon);

		private static Bitmap seedStBitmap = new Bitmap(Resources.SeedIcon);

		private static Bitmap keyStBitmap = new Bitmap(Resources.KeyIcon);

		private static Bitmap expiredKeyStBitmap = new Bitmap(Resources.KeyIcon_Expire);

		private static Color cateHighlightFillColor = Color.FromArgb(225, 243, 250);

		private static Color cateHighlightLineColor = Color.FromArgb(108, 132, 151);

		private Bitmap[] seedKeyImages = new Bitmap[4] { nothingStBitmap, seedStBitmap, keyStBitmap, expiredKeyStBitmap };

		private volatile ScePsmApplication[] appsArray = new ScePsmApplication[100];

		private volatile int appsNum;

		private Thread listAppsThread;

		private Thread logThread;

		private AbstractExecutor logModule;

		private Guid paSelectedGuid;

		private bool paUpdatingDeviceList;

		private WorkerCheckDeviceConnection workerObjectCheckDeviceConnection;

		private Thread workerThreadCheckDeviceConnection;

		private bool IsFormInitialized;

		public static readonly object[] objPEGIRating = new object[5] { "3", "7", "12", "16", "18" };

		public static readonly string EVERYONE_3 = "EVERYONE";

		public static readonly string EVERYONE_10 = "EVERYONE 10+";

		public static readonly string TEEN_13 = "TEEN";

		public static readonly string MATURE_17 = "MATURE 17+";

		public static readonly string ADULTS_ONLY_18 = "ADULTS ONLY 18+";

		public static Dictionary<string, int> dicESRBRating = new Dictionary<string, int>
		{
			{ EVERYONE_3, 3 },
			{ EVERYONE_10, 10 },
			{ TEEN_13, 13 },
			{ MATURE_17, 17 },
			{ ADULTS_ONLY_18, 18 }
		};

		private bool skipChangeCell;

		private IContainer components;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem loadToolStripMenuItem;

		private ToolStripMenuItem saveAsToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem aboutToolStripMenuItem;

		private ToolStripMenuItem saveToolStripMenuItem;

		private ToolStripMenuItem newToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem preferenceToolStripMenuItem;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripMenuItem documentToolStripMenuItem;

		private ErrorProvider errorProvider1;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem defaultToolStripMenuItem;

		private ToolStripMenuItem japaneseToolStripMenuItem;

		private ToolStripMenuItem englishToolStripMenuItem;

		private ToolTip toolTip1;

		private ContextMenuStrip contextMenuStrip1;

		private ToolStripMenuItem toolStripMenuItem1;

		private ToolStripMenuItem proxySettingToolStripMenuItem;

		private PanelManager panelManager1;

		private Button cate3PacageAppsButton;

		private Button cate2KeyManageButton;

		private Button cate1MetadataButton;

		private Panel panelKeyManagement;

		private GroupBox groupBox5;

		private Label labelDeviceList_AppKey;

		private DataGridView dataGridApplicationID;

		private Button buttonExportDeviceSeed;

		private Button buttonImportDeviceSeed;

		private Button buttonExportAppKeyRing;

		private Button buttonImportAppKeyRing;

		private Button buttonGererateAppKey;

		private GroupBox groupBox4;

		private Label label12;

		private TextBox textBoxDateOfGenerateKey;

		private Button buttonImportPublisherKey;

		private Button buttonExportPublisherKey;

		private TextBox textBoxDeveloperKey;

		private Label label4;

		private Button buttonGenerateKey;

		private Panel panelPackageApp;

		private Label appListLabel;

		private Label deviceListLabel;

		private TabControl appTabControl;

		private TabPage ttyTab;

		private TextBox consoleTextBox;

		private TabPage extractTab;

		private TextBox extractLogTextBox;

		private Panel dropBoxPanel;

		private Label dropBoxLabel;

		private Button killAppButton;

		private Button launchAppButton;

		private Button uninstallAppButton;

		private Button installAppButton;

		private DataGridView appsDataGridView;

		private DataGridView deviceDataGridView;

		private DataGridViewImageColumn PaDeviceType;

		private DataGridViewTextBoxColumn PaDeviceName;

		private DataGridViewTextBoxColumn PaGUID;

		private Panel panelMetadata;

		private Button saveAppXmlButton;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private PropertyGrid propertyGrid1;

		private TabPage tabPage2;

		private Panel nameDescriptionPanel;

		private Label appNameLabel;

		private Label nameDescriptionLabel;

		private Label titleLabel;

		private DataGridView appNameDataGridView;

		private DataGridViewTextBoxColumn dataGridView1Column1;

		private DataGridViewTextBoxColumn dataGridView1Column2;

		private DataGridViewTextBoxColumn dataGridView1Column3;

		private TabPage tabPage4;

		private TextBox textBoxCommonRating;

		private TextBox textBoxParentalLockLevel;

		private Label label3;

		private GroupBox groupBox3;

		private TextBox textBoxRatingResult;

		private Label label10;

		private Button buttonStartRatingCheck;

		private GroupBox groupBox2;

		private Label label9;

		private Label label6;

		private TextBox textBoxEsrbCode;

		private Label label7;

		private LinkLabel linkLabel_EsrbUrl;

		private ComboBox dropDownListEsrbRating;

		private GroupBox groupBox1;

		private Label label11;

		private CheckBox checkBoxGambling;

		private PictureBox pictureBox9;

		private CheckBox checkBoxSex;

		private CheckBox checkBoxLanguage;

		private PictureBox pictureBox4;

		private PictureBox pictureBox2;

		private CheckBox checkBoxScaryElements;

		private CheckBox checkBoxDrugs;

		private PictureBox pictureBox8;

		private PictureBox pictureBox7;

		private Label label8;

		private Label label5;

		private Label label2;

		private PictureBox pictureBox1;

		private CheckBox checkBoxDiscrimination;

		private CheckBox checkBoxViolence;

		private CheckBox checkBoxOnline;

		private PictureBox pictureBox5;

		private PictureBox pictureBox3;

		private TextBox TextBoxPEGINumber;

		private ComboBox DropDownListAgeRatingLogo;

		private LinkLabel PEGI_Link;

		private Label label1;

		private TabPage tabPage3;

		private TextBox productLabelText;

		private Label productLabel;

		private Panel purchasePanel;

		private Button addProductButton;

		private Label supportLocalLabel;

		private CheckedListBox localeCheckedListBox;

		private ManagedPanel managedPanel1;

		private ManagedPanel managedPanel2;

		private ManagedPanel managedPanel3;

		private ContextMenuStrip contextMenuStripDevices;

		private ToolStripMenuItem toolStripMenuItemDeleteDeviceSeed;

		private ContextMenuStrip contextMenuStripApplicationID;

		private ToolStripMenuItem toolStripMenuItemDeleteApplicationID;

		private DataGridView dataGridDeviceList;

		private Label label14;

		private TabPage tabIcon;

		private Button buttonOpenFileSplash;

		private TextBox textBoxSplash;

		private Label label18;

		private Button buttonOpenFileIcon128;

		private TextBox textBoxIcon128;

		private Label label17;

		private Button buttonOpenFileIcon256;

		private TextBox textBoxIcon256;

		private Label label16;

		private Button buttonOpenFileIcon512;

		private TextBox textBoxIcon512;

		private Label label15;

		private Button buttonGenerateImage256;

		private Button buttonGenerateImage128;

		private DataGridViewTextBoxColumn PaAppName;

		private DataGridViewImageColumn PaAppKey;

		private DataGridViewTextBoxColumn PaAppSize;

		private ToolStripMenuItem submitToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator4;

		private CheckBox checkBoxEnableLog;

		private Label labelQAPublisherKey;

		private Button buttonInstallAppQA;

		private Button buttonRegisterAppXml;

		private Button buttonGenerateDeviceSeed;

		private Button buttonCheckPublisherKey;

		private Button buttonRemoveAppXml;

		private ToolStripMenuItem toolStripMenuItemDeleteAppKey;

		private Button buttonUpdateDeviceSeedAndAppKey;

		private DataGridViewImageColumn KmDeviceType;

		private DataGridViewTextBoxColumn KmDeviceName;

		private DataGridViewTextBoxColumn KmConnection;

		private DataGridViewImageColumn KmSeedKeyState;

		private DataGridViewImageColumn KmSeed;

		private DataGridViewImageColumn KmAppKey;

		private DataGridViewTextBoxColumn KmExpirationDate;

		private DataGridViewTextBoxColumn KmDeviceID;

		private ToolStripMenuItem deleteAllDeviceSeedAndAppKeyToolStripMenuItem;

		private DataGridViewTextBoxColumn KmAppId;

		public Metadata Metadata => metadata;

		public bool IsMetaChanged
		{
			get
			{
				return isMetaChanged;
			}
			set
			{
				isMetaChanged = value;
				Text = WindowTitleText(isMetaChanged);
			}
		}

		private string WindowTitleText(bool isChanged)
		{
			string text = "";
			string text2 = "";
			if (Program.appConfigData.EnvServer != "np")
			{
				text2 = text2 + " '" + Program.appConfigData.EnvServer + "'";
			}
			text = Application.ProductName + text2;
			if (!string.IsNullOrEmpty(metadata.SelfFilePath))
			{
				text = text + " : \"" + Path.GetFileName(metadata.SelfFilePath) + "\"";
			}
			if (isChanged)
			{
				text += " *";
			}
			return text;
		}

		public MainForm(int panelIndex = 0)
		{
			metadata = new Metadata();
			InitializeComponent();
			InitalizeMenu();
			InitializeToolTip();
			cate1MetadataButton.BackColor = cateHighlightFillColor;
			cate2KeyManageButton.BackColor = (cate3PacageAppsButton.BackColor = SystemColors.Control);
			cate1MetadataButton.FlatAppearance.BorderSize = 1;
			dropBoxLabel.Text = Resources.dropHereToExtract_Text;
			propertyGrid1.SelectedObject = metadata;
			androidBitmap.Tag = "android";
			vitaBitmap.Tag = "vitaOn";
			vitaOffBitmap.Tag = "vitaOff";
			seedKeyImages[0].Tag = "Nothing have been started";
			seedKeyImages[1].Tag = "Got Device Seed";
			seedKeyImages[2].Tag = "App Key has been Ready";
			seedKeyImages[3].Tag = "App Key has been Expired";
			killAppButton.Enabled = false;
			InitializeAppNameGrid();
			InitializeLocaleCheckList();
			InitializeRatingCheckTab();
			if (Program.appConfigData.PcLocale != "ja-JP")
			{
				Font font = new Font("Verdana", 8f);
				propertyGrid1.Font = font;
				localeCheckedListBox.Font = font;
				textBoxIcon512.Font = font;
				textBoxIcon256.Font = font;
				textBoxIcon128.Font = font;
				textBoxSplash.Font = font;
				localeCheckedListBox.Height = 88;
			}
			ResizeDescriptionArea(ref propertyGrid1, 80);
			workerObjectCheckDeviceConnection = new WorkerCheckDeviceConnection();
			workerThreadCheckDeviceConnection = new Thread(workerObjectCheckDeviceConnection.DoWork);
			workerThreadCheckDeviceConnection.Start();
			PsmDeviceFuncBinding.SetAdbExePath(PsmSdk.AdbExe);
			ActivateCategory(panelIndex);
			IsMetaChanged = false;
			IsFormInitialized = true;
			buttonInstallAppQA.Hide();
		}

		private void InitalizeMenu()
		{
			string pcLocale = Program.appConfigData.PcLocale;
			defaultToolStripMenuItem.Checked = string.IsNullOrEmpty(pcLocale) || pcLocale == "Default";
			englishToolStripMenuItem.Checked = pcLocale == "en-US";
			japaneseToolStripMenuItem.Checked = pcLocale == "ja-JP";
			if (defaultToolStripMenuItem.Checked)
			{
				checkedUiLangMenuItem = defaultToolStripMenuItem;
			}
			if (englishToolStripMenuItem.Checked)
			{
				checkedUiLangMenuItem = englishToolStripMenuItem;
			}
			if (japaneseToolStripMenuItem.Checked)
			{
				checkedUiLangMenuItem = japaneseToolStripMenuItem;
			}
		}

		private void InitializeAppNameGrid()
		{
			appNameDataGridView.Rows.Clear();
			foreach (object value in Enum.GetValues(typeof(Locale)))
			{
				string[] values = new string[3]
				{
					Metadata.LocaleToString((Locale)value),
					"",
					""
				};
				appNameDataGridView.Rows.Add(values);
			}
			appNameDataGridView.ColumnHeadersHeight += 5;
			appNameDataGridView.Height = appNameDataGridView.ColumnHeadersHeight + 2 + appNameDataGridView.Rows.Count * appNameDataGridView.RowTemplate.Height;
		}

		private void InitializeLocaleCheckList()
		{
			localeCheckedListBox.Items.Clear();
			foreach (object value in Enum.GetValues(typeof(Locale)))
			{
				localeCheckedListBox.Items.Add(Metadata.LocaleToString((Locale)value));
			}
		}

		private void InitializeToolTip()
		{
			toolTip1.SetToolTip(productLabelText, Resources.toolTipProductLabel_Text);
			toolTip1.SetToolTip(addProductButton, Resources.toolTipProductAddButton_Text);
		}

		public void RefreshGui()
		{
			propertyGrid1.Refresh();
			for (int i = 0; i < appNameDataGridView.RowCount; i++)
			{
				appNameDataGridView[1, i].Value = ((metadata.AppLongNames[(Locale)i] != null) ? (metadata.AppLongNames[(Locale)i] as string) : "");
				appNameDataGridView[2, i].Value = ((metadata.AppShortNames[(Locale)i] != null) ? (metadata.AppShortNames[(Locale)i] as string) : "");
			}
			for (int j = 0; j < localeCheckedListBox.Items.Count; j++)
			{
				localeCheckedListBox.SetItemCheckState(j, CheckState.Unchecked);
			}
			textBoxCommonRating.Text = metadata.CommonRating;
			textBoxParentalLockLevel.Text = ((metadata.ParentalLockLevel == "99") ? "(Can't publish.)" : metadata.ParentalLockLevel);
			TextBoxPEGINumber.Text = metadata.PegiCode;
			textBoxEsrbCode.Text = metadata.EsrbCode;
			if (Program._RatingData.IsCheckedOffSceRating)
			{
				textBoxRatingResult.Text = Program._RatingData.OutputResultText();
			}
			else
			{
				textBoxRatingResult.Text = "(Not Checked)";
			}
			checkBoxDrugs.Checked = metadata.IsPegiDrugs;
			checkBoxLanguage.Checked = metadata.IsPegiLanguage;
			checkBoxSex.Checked = metadata.IsPegiSex;
			checkBoxScaryElements.Checked = metadata.IsPegiScaryElements;
			checkBoxOnline.Checked = metadata.IsPegiOnline;
			checkBoxDiscrimination.Checked = metadata.IsPegiDiscrimination;
			checkBoxViolence.Checked = metadata.IsPegiViolence;
			checkBoxGambling.Checked = metadata.IsPegiGambling;
			DropDownListAgeRatingLogo.SelectedItem = metadata.PegiAgeRatingText;
			dropDownListEsrbRating.SelectedItem = metadata.EsrbRatingText;
			RefreshGuiIconTab();
			RefreshKeyManagementPanel();
			foreach (Control purchaseSubPanel in purchaseSubPanels)
			{
				purchaseSubPanel.Dispose();
			}
			purchaseSubPanels.Clear();
			foreach (Product product3 in metadata.ProductList)
			{
				foreach (DictionaryEntry name in product3.Names)
				{
					localeCheckedListBox.SetItemCheckState((int)name.Key, CheckState.Checked);
				}
			}
			purchasePanel.SuspendLayout();
			foreach (Product product4 in metadata.ProductList)
			{
				AddPurchaseTable(product4.Label);
			}
			purchasePanel.ResumeLayout();
		}

		public int GetActiveCategory()
		{
			int result = 0;
			int.TryParse(panelManager1.SelectedPanel.Tag.ToString(), out result);
			return result;
		}

		public void ActivateCategory(int categoryIdx)
		{
			ManagedPanel[] array = new ManagedPanel[3] { managedPanel1, managedPanel2, managedPanel3 };
			panelManager1.SelectedPanel = array[categoryIdx];
		}

		private bool ResizeDescriptionArea(ref PropertyGrid grid, int height)
		{
			try
			{
				foreach (Control control in grid.Controls)
				{
					if (control.GetType().Name == "DocComment")
					{
						FieldInfo field = control.GetType().BaseType.GetField("userSized", BindingFlags.Instance | BindingFlags.NonPublic);
						field.SetValue(control, true);
						control.Height = height;
					}
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void ResetMeta()
		{
			base.ActiveControl = saveAppXmlButton;
			if (ConfirmDeleteMeta(Resources.newPromptMsgBoxBody_Text, Resources.newPromptMsgBoxTitle_Text))
			{
				metadata.Reset();
				Program._RatingData.Reset();
				RefreshGui();
				productLabelText.Text = "";
				IsMetaChanged = false;
			}
		}

		public void LoadMeta()
		{
			if (ConfirmDeleteMeta(Resources.loadPromptMsgBoxBody_Text, Resources.loadPromptMsgBoxTitle_Text))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Title = Resources.openFileDialogTitle_Text;
				openFileDialog.Filter = Resources.openSaveFileDialogFilter_Text;
				openFileDialog.FilterIndex = 0;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.CheckFileExists = true;
				openFileDialog.CheckPathExists = true;
				openFileDialog.DereferenceLinks = true;
				openFileDialog.DefaultExt = "XML";
				openFileDialog.AddExtension = true;
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					LoadMeta(openFileDialog.FileName);
				}
			}
		}

		public void LoadMeta(string file)
		{
			XDocument xDocument = Xml.LoadFromFile(file);
			if (xDocument == null)
			{
				return;
			}
			XElement xElement = xDocument.Element("application");
			if (xElement != null)
			{
				metadata.Reset();
				metadata.SelfFilePath = file;
				Program._RatingData.Reset();
				if (!Xml.ReadFromXml(metadata, xElement))
				{
					metadata.Reset();
					Program._RatingData.Reset();
				}
				RefreshGui();
				IsMetaChanged = false;
			}
		}

		public void SaveAsMeta()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = Resources.saveFileDialogTitle_Text;
			if (metadata != null && !string.IsNullOrEmpty(Path.GetFileName(metadata.SelfFilePath)))
			{
				saveFileDialog.FileName = Path.GetFileName(metadata.SelfFilePath);
			}
			else
			{
				saveFileDialog.FileName = "app.xml";
			}
			saveFileDialog.Filter = Resources.openSaveFileDialogFilter_Text;
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.OverwritePrompt = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				metadata.SelfFilePath = saveFileDialog.FileName;
				SaveMeta();
				RefreshGui();
			}
		}

		public void SaveMeta()
		{
			if (metadata.SelfFilePath != null)
			{
				Xml.WriteToXml(metadata, out var root);
				Xml.SaveIntoFile(metadata.SelfFilePath, root);
				IsMetaChanged = false;
			}
			else
			{
				SaveAsMeta();
			}
		}

		public bool ConfirmDeleteMeta(string msg, string title)
		{
			if (IsMetaChanged)
			{
				switch (MessageBox.Show(msg, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
				case DialogResult.Yes:
					SaveMeta();
					return true;
				case DialogResult.No:
					IsMetaChanged = false;
					return true;
				default:
					return false;
				}
			}
			return true;
		}

		public bool ExitingApplication()
		{
			return ConfirmDeleteMeta(string.Format(Resources.exitPromptMsgBoxBody_Text, Application.ProductName), Resources.exitPromptMsgBoxTitle_Text);
		}

		private void buttonOrMenuItem_Click(object sender, EventArgs e)
		{
			if (newToolStripMenuItem == sender)
			{
				ResetMeta();
			}
			else if (loadToolStripMenuItem == sender)
			{
				LoadMeta();
			}
			else if (saveToolStripMenuItem == sender || saveAppXmlButton == sender)
			{
				SaveMeta();
			}
			else if (saveAsToolStripMenuItem == sender)
			{
				SaveAsMeta();
			}
			else if (submitToolStripMenuItem == sender)
			{
				Command.Submit();
			}
			else if (proxySettingToolStripMenuItem == sender)
			{
				Form form = new ProxySetting();
				form.Show();
			}
			else if (exitToolStripMenuItem == sender)
			{
				if (ExitingApplication())
				{
					Application.Exit();
				}
			}
			else if (documentToolStripMenuItem == sender)
			{
				Utility.LinkToSDKDocument();
			}
			else if (aboutToolStripMenuItem == sender)
			{
				VersionInfoDialog versionInfoDialog = new VersionInfoDialog();
				versionInfoDialog.ShowDialog(this);
			}
			else
			{
				MessageBox.Show("Not Implemented Yet", "[ToDo] We need to implement this!");
			}
		}

		private void localeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			defaultToolStripMenuItem.Checked = sender == defaultToolStripMenuItem;
			englishToolStripMenuItem.Checked = sender == englishToolStripMenuItem;
			japaneseToolStripMenuItem.Checked = sender == japaneseToolStripMenuItem;
			if (sender != checkedUiLangMenuItem)
			{
				Program.appConfigData.PcLocale = (sender as ToolStripItem).Tag.ToString();
				DialogResult dialogResult = MessageBox.Show(Resources.restartFormBody_Text, Resources.restartFormTitle_Text, MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
				if (dialogResult == DialogResult.Yes)
				{
					Application.Restart();
				}
			}
			checkedUiLangMenuItem = sender;
		}

		private void categorySwitchButton_Click(object sender, EventArgs e)
		{
			int num = int.Parse(((Button)sender).Tag.ToString());
			Color color = cateHighlightFillColor;
			Color color2 = cateHighlightLineColor;
			Color control = SystemColors.Control;
			Color control2 = SystemColors.Control;
			cate1MetadataButton.BackColor = ((num == 0) ? color : control);
			cate2KeyManageButton.BackColor = ((num == 1) ? color : control);
			cate3PacageAppsButton.BackColor = ((num == 2) ? color : control);
			cate1MetadataButton.FlatAppearance.BorderColor = ((num == 0) ? color2 : control2);
			cate2KeyManageButton.FlatAppearance.BorderColor = ((num == 1) ? color2 : control2);
			cate3PacageAppsButton.FlatAppearance.BorderColor = ((num == 2) ? color2 : control2);
			cate1MetadataButton.FlatAppearance.BorderSize = ((num == 0) ? 1 : 0);
			cate2KeyManageButton.FlatAppearance.BorderSize = ((num == 1) ? 1 : 0);
			cate3PacageAppsButton.FlatAppearance.BorderSize = ((num == 2) ? 1 : 0);
			ActivateCategory(num);
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] array2 = array;
			foreach (string path in array2)
			{
				if (!File.Exists(path))
				{
					e.Effect = DragDropEffects.None;
					return;
				}
			}
			e.Effect = DragDropEffects.Copy;
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (Path.GetExtension(text) == ".xml")
				{
					if (ConfirmDeleteMeta(Resources.loadPromptMsgBoxBody_Text, Resources.loadPromptMsgBoxTitle_Text))
					{
						LoadMeta(text);
					}
					break;
				}
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !ExitingApplication();
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Console.WriteLine("MainForm_FormClosed()");
			StopLog();
			Program.appConfigData.Save();
			workerObjectCheckDeviceConnection.RequestStop();
			workerThreadCheckDeviceConnection.Join(1000);
		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (e.ChangedItem.Value != e.OldValue)
			{
				IsMetaChanged = true;
			}
		}

		private void appNameDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!skipChangeCell)
			{
				if (e.RowIndex < 0)
				{
					return;
				}
				DataGridView dataGridView = sender as DataGridView;
				DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
				DataGridViewCell dataGridViewCell = dataGridView[e.ColumnIndex, e.RowIndex];
				string text = (dataGridViewRow.Cells[e.ColumnIndex].Value as string).Trim();
				Locale locale = Metadata.StringToLocale(dataGridViewRow.Cells[0].Value as string);
				Hashtable hashtable;
				bool flag;
				if (e.ColumnIndex == 1)
				{
					hashtable = metadata.AppLongNames;
					flag = !metadata.IsAppLongNamesValid(text);
				}
				else
				{
					hashtable = metadata.AppShortNames;
					flag = !metadata.IsAppShortNamesValid(text);
				}
				if (flag && text.Length >= 2)
				{
					MessageBox.Show(Resources.nameByteOverMsgBoxBody_Text, Resources.nameByteOverMsgBoxTitle_Text);
					string text2 = hashtable[locale] as string;
					skipChangeCell = true;
					dataGridViewCell.Value = text2 ?? "";
				}
				else if (text != dataGridViewRow.Cells[e.ColumnIndex].Value as string)
				{
					skipChangeCell = true;
					dataGridViewCell.Value = text;
				}
				else
				{
					if (dataGridViewRow.Cells[e.ColumnIndex].Value != null && text.Length != 0)
					{
						hashtable[locale] = text;
					}
					else
					{
						hashtable.Remove((Locale)e.RowIndex);
					}
					IsMetaChanged = true;
				}
			}
			else
			{
				skipChangeCell = false;
			}
		}

		private void tabPage2_Enter(object sender, EventArgs e)
		{
			if (metadata.DefaultLocale != oldLocale)
			{
				appNameDataGridView[0, (int)oldLocale].Style.Font = new Font("MS UI Gothic", 9f, FontStyle.Regular);
				appNameDataGridView[0, (int)metadata.DefaultLocale].Style.Font = new Font("MS UI Gothic", 9f, FontStyle.Bold);
				appNameDataGridView[1, (int)metadata.DefaultLocale].Selected = true;
			}
			oldLocale = metadata.DefaultLocale;
		}

		private void dataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Modifiers & Keys.Control) != Keys.Control)
			{
				return;
			}
			DataGridView dataGridView = sender as DataGridView;
			if ((e.KeyData & Keys.KeyCode) == Keys.C)
			{
				Clipboard.SetDataObject(dataGridView.CurrentCell.Value.ToString());
			}
			if ((e.KeyData & Keys.KeyCode) == Keys.V && dataGridView.CurrentCell.ColumnIndex != 0)
			{
				int maxInputLength = (dataGridView.CurrentCell.OwningColumn as DataGridViewTextBoxColumn).MaxInputLength;
				int length = Clipboard.GetText().Length;
				if (length <= maxInputLength)
				{
					dataGridView.CurrentCell.Value = Clipboard.GetText();
					return;
				}
				dataGridView.CurrentCell.Value = Clipboard.GetText().Substring(0, maxInputLength);
				MessageBox.Show(string.Format(Resources.stringTooLongMsgBoxBody_Text, length, maxInputLength), Resources.stringTooLongMsgBoxTitle_Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void appNameDataGridView_CurrentCellChanged(object sender, EventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			switch (dataGridView.CurrentCellAddress.X)
			{
			case 0:
				appNameLabel.Text = "Locale";
				nameDescriptionLabel.Text = "";
				break;
			case 1:
				appNameLabel.Text = "Long Name";
				nameDescriptionLabel.Text = Resources.descriptionAppLongNames;
				break;
			case 2:
				appNameLabel.Text = "Short Name";
				nameDescriptionLabel.Text = Resources.descriptionAppShortNames;
				break;
			}
		}

		private void RefreshGuiIconTab()
		{
			textBoxIcon512.Text = Metadata.Icon512x512;
			textBoxIcon256.Text = Metadata.Icon256x256;
			textBoxIcon128.Text = Metadata.Icon128x128;
			textBoxSplash.Text = Metadata.Splash;
			if (!string.IsNullOrWhiteSpace(textBoxIcon512.Text))
			{
				buttonGenerateImage256.Enabled = true;
				buttonGenerateImage128.Enabled = true;
			}
			else
			{
				buttonGenerateImage256.Enabled = false;
				buttonGenerateImage128.Enabled = false;
			}
		}

		private void buttonOpenFileIcon512_Click(object sender, EventArgs e)
		{
			if (OpenIconImage(out var pathFile))
			{
				Metadata.Icon512x512 = pathFile;
			}
			RefreshGuiIconTab();
		}

		private void buttonOpenFileIcon256_Click(object sender, EventArgs e)
		{
			if (OpenIconImage(out var pathFile))
			{
				Metadata.Icon256x256 = pathFile;
			}
			RefreshGuiIconTab();
		}

		private void buttonOpenFileIcon128_Click(object sender, EventArgs e)
		{
			if (OpenIconImage(out var pathFile))
			{
				Metadata.Icon128x128 = pathFile;
			}
			RefreshGuiIconTab();
		}

		private void buttonOpenFileSplash_Click(object sender, EventArgs e)
		{
			if (OpenIconImage(out var pathFile))
			{
				Metadata.Splash = pathFile;
			}
			RefreshGuiIconTab();
		}

		private void buttonGenerateImage256_Click(object sender, EventArgs e)
		{
			int size = 256;
			string text = Utility.MakeFullPath(Metadata.SelfFileDir, Metadata.Icon512x512);
			if (!File.Exists(text))
			{
				MessageBox.Show($"Can't find {text}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			if (SaveFileDialogIconImage(fileNameWithoutExtension + "_" + size, out var pathFile))
			{
				if (!Utility.IsExpectFormatPng(text, new Size(512, 512), PixelFormat.Format32bppArgb))
				{
					MessageBox.Show($"{text} is invalid format.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (text == pathFile)
				{
					MessageBox.Show(string.Format(Resources.sameFilename_Text, text), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				Utility.ScaleImageFile(text, pathFile, size);
				Metadata.Icon256x256 = pathFile;
				RefreshGuiIconTab();
			}
		}

		private void buttonGenerateImage128_Click(object sender, EventArgs e)
		{
			int size = 128;
			string text = Utility.MakeFullPath(Metadata.SelfFileDir, Metadata.Icon512x512);
			if (!File.Exists(text))
			{
				MessageBox.Show($"Can't find {text}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			if (SaveFileDialogIconImage(fileNameWithoutExtension + "_" + size, out var pathFile))
			{
				if (!Utility.IsExpectFormatPng(text, new Size(512, 512), PixelFormat.Format32bppArgb))
				{
					MessageBox.Show($"{text} is invalid format.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (text == pathFile)
				{
					MessageBox.Show(string.Format(Resources.sameFilename_Text, text), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				Utility.ScaleImageFile(text, pathFile, size);
				Metadata.Icon128x128 = pathFile;
				RefreshGuiIconTab();
			}
		}

		private void textBoxIcon512_Validated(object sender, EventArgs e)
		{
			Metadata.Icon512x512 = textBoxIcon512.Text;
			textBoxIcon512.Text = Metadata.Icon512x512;
		}

		private void textBoxIcon256_Validated(object sender, EventArgs e)
		{
			Metadata.Icon256x256 = textBoxIcon256.Text;
			textBoxIcon256.Text = Metadata.Icon256x256;
		}

		private void textBoxIcon128_Validated(object sender, EventArgs e)
		{
			Metadata.Icon128x128 = textBoxIcon128.Text;
			textBoxIcon128.Text = Metadata.Icon128x128;
		}

		private void textBoxSplash_Validated(object sender, EventArgs e)
		{
			Metadata.Splash = textBoxSplash.Text;
			textBoxSplash.Text = Metadata.Splash;
		}

		private bool OpenIconImage(out string pathFile)
		{
			pathFile = "";
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Open icon image";
			openFileDialog.Filter = "PNG file|*.png|All files|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "png";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				pathFile = openFileDialog.FileName;
				return true;
			}
			return false;
		}

		private bool SaveFileDialogIconImage(string filename, out string pathFile)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = Resources.saveFileDialogTitle_Text;
			saveFileDialog.Filter = "PNG files|*.png|All files|*.*";
			saveFileDialog.FileName = filename;
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.OverwritePrompt = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				pathFile = saveFileDialog.FileName;
				return true;
			}
			pathFile = "";
			return false;
		}

		private void GenerateQAPublisherKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			PublisherKeyUtility.GeneratePublishKey(PublisherKeyType.SWQA);
			RefreshGui();
			RestartCheckDeviceConnection();
		}

		private void PEGI_Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://psm-rating.pegi.eu/Games/Submit");
		}

		private void linkLabel_EsrbUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://autoratingtool.esrb.org/ARTclient/PlayStationMobile.aspx");
		}

		private void InitializeRatingCheckTab()
		{
			DropDownListAgeRatingLogo.Items.AddRange(objPEGIRating);
			DropDownListAgeRatingLogo.SelectedIndex = 0;
			foreach (string key in dicESRBRating.Keys)
			{
				dropDownListEsrbRating.Items.Add(key);
			}
			dropDownListEsrbRating.SelectedIndex = 0;
			textBoxEsrbCode.Text = "";
			TextBoxPEGINumber.Text = "";
		}

		private void buttonStartRatingCheck_Click(object sender, EventArgs e)
		{
			Program._RatingData.Reset();
			RatingFormMain ratingFormMain = new RatingFormMain();
			DialogResult dialogResult = ratingFormMain.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				RefreshGui();
				return;
			}
			Form form;
			if (Program._RatingData.IsMainLanguage01)
			{
				form = new RatingFormLanguage();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			if (Program._RatingData.IsMainViolence01)
			{
				form = new RatingFormViolence();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
				form = new RatingFormViolence02();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			if (Program._RatingData.IsMainCrime01)
			{
				form = new RatingFormCrime();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			if (Program._RatingData.IsMainGambling01)
			{
				form = new RatingFormGambling();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			if (Program._RatingData.IsMainDrugs01)
			{
				form = new RatingFormDrugs();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			if (Program._RatingData.IsMainSex01)
			{
				form = new RatingFormSex();
				dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RefreshGui();
					return;
				}
			}
			form = new RatingFormOnline();
			dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				RefreshGui();
				return;
			}
			RatingFormResult ratingFormResult = new RatingFormResult();
			dialogResult = ratingFormResult.ShowDialog();
			Console.WriteLine("result=" + dialogResult);
			textBoxRatingResult.Text = Program._RatingData.TextResultRating;
			Program._RatingData.IsCheckedOffSceRating = true;
			IsMetaChanged = true;
			RefreshGui();
		}

		private void DropDownListAgeRatingLogo_SelectedIndexChanged(object sender, EventArgs e)
		{
			metadata.PegiAgeRatingText = DropDownListAgeRatingLogo.Text;
			IsMetaChanged = true;
			RefreshGui();
		}

		private void TextBoxPEGINumber_Validated(object sender, EventArgs e)
		{
			metadata.PegiCode = TextBoxPEGINumber.Text;
			IsMetaChanged = true;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			metadata.EsrbRatingText = dropDownListEsrbRating.Text;
			metadata.EsrbRatingNumber = dicESRBRating[dropDownListEsrbRating.Text];
			IsMetaChanged = true;
			RefreshGui();
		}

		private void textBoxEsrbCode_Validated(object sender, EventArgs e)
		{
			metadata.EsrbCode = textBoxEsrbCode.Text;
			IsMetaChanged = true;
		}

		private void checkBoxDrugs_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiDrugs = checkBoxDrugs.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxLanguage_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiLanguage = checkBoxLanguage.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxSex_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiSex = checkBoxSex.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxScaryElements_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiScaryElements = checkBoxScaryElements.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxOnline_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiOnline = checkBoxOnline.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxDiscrimination_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiDiscrimination = checkBoxDiscrimination.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxViolence_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiViolence = checkBoxViolence.Checked;
			IsMetaChanged = true;
		}

		private void checkBoxGambling_CheckedChanged(object sender, EventArgs e)
		{
			metadata.IsPegiGambling = checkBoxGambling.Checked;
			IsMetaChanged = true;
		}

		private void localeCheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAddProductButton();
			ResizePurchaseTable();
		}

		private void productLabelText_Validating(object sender, CancelEventArgs e)
		{
			if (metadata.IsProductLabelValid(productLabelText.Text) || productLabelText.Text.Length == 0)
			{
				errorProvider1.SetError((TextBox)sender, null);
			}
			else
			{
				errorProvider1.SetError((TextBox)sender, Resources.productFormatAlart_Text);
			}
		}

		private void productLabelText_TextChanged(object sender, EventArgs e)
		{
			if (metadata.IsProductLabelValid(productLabelText.Text) || productLabelText.Text.Length == 0)
			{
				errorProvider1.SetError((TextBox)sender, null);
			}
			UpdateAddProductButton();
		}

		private void productLabelText_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyData & Keys.KeyCode) == Keys.Return && addProductButton.Enabled)
			{
				addProductButton.PerformClick();
			}
		}

		private void addProductButton_Click(object sender, EventArgs e)
		{
			if (IsProductNameUsed(productLabelText.Text))
			{
				MessageBox.Show("このラベル名は既に使われています", "ラベルの重複");
				return;
			}
			if (metadata.ProductList.Count >= 20)
			{
				MessageBox.Show(Resources.cannotAdd21thProductMsgBoxBody_Text, Resources.cannotAdd21thProductMsgBoxTitle_Text);
				return;
			}
			metadata.AddProduct(new Product(productLabelText.Text));
			bool scroll = true;
			AddPurchaseTable(productLabelText.Text, scroll);
		}

		private void purchaseDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				return;
			}
			DataGridView dataGridView = sender as DataGridView;
			DataGridViewRow dataGridViewRow = dataGridView.Rows[e.RowIndex];
			int num = 0;
			foreach (Panel purchaseSubPanel in purchaseSubPanels)
			{
				DataGridView dataGridView2 = purchaseSubPanel.Controls[0] as DataGridView;
				if (dataGridView2 == dataGridView)
				{
					Product product = metadata.ProductList[num] as Product;
					Hashtable names = product.Names;
					Locale locale = Metadata.StringToLocale(dataGridViewRow.Cells[0].Value as string);
					if (dataGridViewRow.Cells[1].Value != null)
					{
						names[locale] = dataGridViewRow.Cells[1].Value as string;
					}
					else
					{
						names.Remove(locale);
					}
					IsMetaChanged = true;
				}
				num++;
			}
		}

		private void checkConsumable_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			Product product = (sender as Control).Parent.Tag as Product;
			product.Consumable = checkBox.Checked;
			IsMetaChanged = true;
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			if (toolStripMenuItem == null)
			{
				return;
			}
			ContextMenuStrip contextMenuStrip = toolStripMenuItem.Owner as ContextMenuStrip;
			if (contextMenuStrip == null)
			{
				return;
			}
			Panel panel = contextMenuStrip.SourceControl as Panel;
			if (panel != null)
			{
				DialogResult dialogResult = MessageBox.Show(Resources.removeProductBody_Text, Resources.removeProductTitle_Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
				int num = purchaseSubPanels.IndexOf(panel);
				if (dialogResult == DialogResult.Yes && num != -1)
				{
					purchaseSubPanels.RemoveAt(num);
					metadata.ProductList.RemoveAt(num);
					panel.Dispose();
					ResizePurchaseTable();
					IsMetaChanged = true;
				}
			}
		}

		private bool IsProductNameUsed(string rhs)
		{
			foreach (Product product in metadata.ProductList)
			{
				if (product.Label == rhs)
				{
					return true;
				}
			}
			return false;
		}

		private void UpdateAddProductButton()
		{
			addProductButton.Enabled = metadata.IsProductLabelValid(productLabelText.Text) && !IsProductNameUsed(productLabelText.Text) && localeCheckedListBox.CheckedItems.Count > 0;
		}

		private void AddPurchaseTable(string labelname, bool scroll = false)
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			Panel panel = new Panel();
			DataGridView dataGridView = new DataGridView();
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			Product product = (Product)(panel.Tag = metadata.ProductList[purchaseSubPanels.Count] as Product);
			Label label = new Label();
			label.Text = labelname;
			label.Location = new Point(3, 0);
			label.Width = 70;
			CheckBox checkBox = new CheckBox();
			checkBox.Name = "consumableCheckBox";
			checkBox.Text = Resources.consumableCheckBox_Text;
			checkBox.Location = new Point(purchasePanel.ClientRectangle.Width - checkBox.Width, -5);
			checkBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			checkBox.Checked = product.Consumable;
			checkBox.CheckedChanged += checkConsumable_CheckedChanged;
			dataGridViewCellStyle.Font = new Font("MS UI Gothic", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridView.AllowUserToAddRows = false;
			dataGridView.AllowUserToDeleteRows = false;
			dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView.ColumnHeadersVisible = false;
			dataGridView.Columns.AddRange(dataGridViewTextBoxColumn, dataGridViewTextBoxColumn2);
			dataGridView.GridColor = Color.FromArgb(224, 224, 224);
			dataGridView.Location = new Point(0, 15);
			dataGridView.MultiSelect = false;
			dataGridView.Name = "purchaseDataGridViewX";
			dataGridView.RowHeadersVisible = false;
			dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle;
			dataGridView.RowTemplate.Height = 16;
			dataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
			dataGridView.ScrollBars = ScrollBars.Horizontal;
			dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			int num = purchasePanel.ClientRectangle.Width;
			int num2 = dataGridView.RowTemplate.Height * localeCheckedListBox.CheckedItems.Count + 3;
			dataGridView.Size = new Size(num, num2);
			dataGridView.TabIndex = 0;
			dataGridView.CellValueChanged += purchaseDataGridView_CellValueChanged;
			dataGridView.KeyDown += dataGridView_KeyDown;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.MenuHighlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
			dataGridViewTextBoxColumn.Frozen = true;
			dataGridViewTextBoxColumn.HeaderText = "Locale";
			dataGridViewTextBoxColumn.Name = "dataGridView1ColumnX1";
			dataGridViewTextBoxColumn.ReadOnly = true;
			dataGridViewTextBoxColumn.Resizable = DataGridViewTriState.False;
			dataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridViewTextBoxColumn.Width = 70;
			dataGridViewCellStyle3.BackColor = SystemColors.Window;
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.WindowText;
			dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle3;
			dataGridViewTextBoxColumn2.HeaderText = "String";
			dataGridViewTextBoxColumn2.MaxInputLength = 64;
			dataGridViewTextBoxColumn2.Name = "dataGridView1ColumnX2";
			dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.True;
			dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
			foreach (object value in Enum.GetValues(typeof(Locale)))
			{
				string text = Metadata.LocaleToString((Locale)value);
				string text2 = ((product != null) ? (product.Names[(Locale)value] as string) : "");
				string[] values = new string[2] { text, text2 };
				if (localeCheckedListBox.GetItemChecked((int)(Locale)value))
				{
					dataGridView.Rows.Add(values);
				}
			}
			int num3 = purchaseSubPanels.Count * (num2 + 25);
			panel.Size = new Size(num, num2 + 16);
			panel.Location = new Point(0, num3 - purchasePanel.VerticalScroll.Value);
			panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			panel.Controls.Add(dataGridView);
			panel.Controls.Add(label);
			panel.Controls.Add(checkBox);
			panel.ContextMenuStrip = contextMenuStrip1;
			purchasePanel.Controls.Add(panel);
			purchaseSubPanels.Add(panel);
			UpdateAddProductButton();
			if (scroll)
			{
				purchasePanel.ScrollControlIntoView(panel);
			}
		}

		private void ResizePurchaseTable()
		{
			int num = 0;
			int num2 = purchasePanel.ClientRectangle.Width;
			purchasePanel.SuspendLayout();
			foreach (Panel purchaseSubPanel in purchaseSubPanels)
			{
				DataGridView dataGridView = purchaseSubPanel.Controls[0] as DataGridView;
				_ = purchaseSubPanel.Controls[1];
				int num3 = dataGridView.RowTemplate.Height * localeCheckedListBox.CheckedItems.Count + 3;
				dataGridView.Size = new Size(num2, num3);
				int num4 = num * (num3 + 25);
				purchaseSubPanel.Location = new Point(0, num4 - purchasePanel.VerticalScroll.Value);
				purchaseSubPanel.Size = new Size(num2, num3 + 16);
				dataGridView.Rows.Clear();
				foreach (object value in Enum.GetValues(typeof(Locale)))
				{
					string text = Metadata.LocaleToString((Locale)value);
					Product product = metadata.ProductList[num] as Product;
					Hashtable names = product.Names;
					string[] values = new string[2]
					{
						text,
						names[value] as string
					};
					if (localeCheckedListBox.GetItemChecked((int)(Locale)value))
					{
						dataGridView.Rows.Add(values);
					}
				}
				num++;
			}
			purchasePanel.ResumeLayout();
		}

		private void RefreshKeyManagementPanel()
		{
			textBoxDeveloperKey.Text = Program.appConfigData.PublisherKeyName;
			textBoxDateOfGenerateKey.Text = Program.appConfigData.CreateTimeOfPublisherKey;
			RefreshDataGridApplicationID();
			RefreshDataGridDeviceList();
			if (PublisherKeyUtility.ExistPublisherKey())
			{
				buttonExportPublisherKey.Enabled = true;
				buttonGererateAppKey.Enabled = true;
			}
			else
			{
				buttonExportPublisherKey.Enabled = false;
				buttonGererateAppKey.Enabled = false;
			}
			if (Directory.Exists(Program.PATH_DIR_TARGET_APPS_KEY))
			{
				if (Directory.GetDirectories(Program.PATH_DIR_TARGET_APPS_KEY).Length > 0)
				{
					buttonRemoveAppXml.Enabled = true;
					buttonUpdateDeviceSeedAndAppKey.Enabled = true;
					buttonGererateAppKey.Enabled = true;
				}
				else
				{
					buttonRemoveAppXml.Enabled = false;
					buttonUpdateDeviceSeedAndAppKey.Enabled = false;
					buttonGererateAppKey.Enabled = false;
				}
			}
		}

		private void RefreshDataGridApplicationID()
		{
			Console.WriteLine("RefreshDataGridApplicationID()");
			dataGridApplicationID.Rows.Clear();
			if (Directory.Exists(Program.PATH_DIR_TARGET_APPS_KEY))
			{
				string[] directories = Directory.GetDirectories(Program.PATH_DIR_TARGET_APPS_KEY);
				string[] array = directories;
				foreach (string path in array)
				{
					string fileName = Path.GetFileName(path);
					if (fileName == "+asterisk+")
					{
						dataGridApplicationID.Rows.Add("*");
					}
					else
					{
						dataGridApplicationID.Rows.Add(fileName);
					}
				}
				if (directories.Length > 0)
				{
					toolStripMenuItemDeleteApplicationID.Enabled = true;
				}
				else
				{
					toolStripMenuItemDeleteApplicationID.Enabled = false;
				}
			}
			else
			{
				toolStripMenuItemDeleteApplicationID.Enabled = false;
			}
		}

		private void dataGridApplicationID_SelectionChanged(object sender, EventArgs e)
		{
			Console.WriteLine("dataGridApplicationID_SelectionChanged()");
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell != null)
			{
				string text = currentCell.Value.ToString();
				string text2 = "";
				text2 = ((!(text == "*")) ? text : "+asterisk+");
				if (AppKeyUtility.ExistAppKey(text2))
				{
					buttonExportAppKeyRing.Enabled = true;
				}
				else
				{
					buttonExportAppKeyRing.Enabled = false;
				}
				labelDeviceList_AppKey.Text = Utility.TextLanguage($"Device List && App Key of {text}", $"デバイスリスト と {text} のアプリ鍵");
				RefreshDataGridDeviceList();
			}
		}

		private void RefreshDataGridDeviceList()
		{
			Console.WriteLine("RefreshDataGridDeviceList()");
			int num = -1;
			if (dataGridDeviceList.SelectedRows.Count > 0)
			{
				num = dataGridDeviceList.SelectedRows[0].Index;
			}
			kmUpdatingDeviceList = true;
			dataGridDeviceList.Rows.Clear();
			string text = "";
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell != null)
			{
				text = currentCell.Value.ToString();
				string text2 = "";
				if (dataGridDeviceList.CurrentRow != null)
				{
					currentCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
					if (currentCell == null)
					{
						return;
					}
					text2 = currentCell.Value.ToString();
				}
				Console.WriteLine("applicationID=" + text);
				Console.WriteLine("deviceID=" + text2);
				string text3 = "";
				text3 = ((!(text == "*")) ? text : "+asterisk+");
				string path = Program.PATH_DIR_TARGET_APPS_KEY + "\\" + text3 + "\\";
				if (!Directory.Exists(path))
				{
					Console.WriteLine(text3 + "に対応するフォルダーがない。");
					foreach (DeviceStatus item in Program.appConfigData.listDevice)
					{
						item.appExeKeyStatus = AppExeKeyStatus.None;
					}
					return;
				}
				Console.WriteLine(text3 + "に対応するフォルダーはある。");
				foreach (DeviceStatus item2 in Program.appConfigData.listDevice)
				{
					Console.WriteLine("device=" + item2.DeviceID);
					item2.appExeKeyStatus = AppExeKeyStatus.None;
					item2.strAppExeEpiration = "-";
					string[] files = Directory.GetFiles(path);
					int num2 = 0;
					while (true)
					{
						if (num2 < files.Length)
						{
							string text4 = files[num2];
							if (item2.DeviceID == Path.GetFileNameWithoutExtension(text4) || item2.Nickname == Path.GetFileNameWithoutExtension(text4))
							{
								Console.WriteLine("->あり");
								item2.appExeKeyStatus = AppExeKeyStatus.OK;
								AppExeKey appExeKey = new AppExeKey();
								if (appExeKey.Load(text4))
								{
									item2.strAppExeEpiration = Utility.ConvertLocaleDateTime(appExeKey.GetExpirationString());
									item2.appExeKeyStatus = appExeKey.GetValidationStatus();
								}
								break;
							}
							num2++;
							continue;
						}
						Console.WriteLine("->なし");
						break;
					}
				}
			}
			int num3 = 0;
			foreach (DeviceStatus item3 in Program.appConfigData.listDevice)
			{
				if (!item3.HasSeed && !item3.IsUsbConnected)
				{
					continue;
				}
				Bitmap bitmap = ((item3.deviceType == DeviceType.Android) ? androidBitmap : (item3.IsDevAssistantOn ? vitaBitmap : vitaOffBitmap));
				Bitmap bitmap2 = seedKeyImages[(int)item3.deviceSeedStatus];
				Bitmap bitmap3;
				if (item3.appExeKeyStatus == AppExeKeyStatus.OK)
				{
					bitmap3 = ((!Program.appConfigData.IsValidPublisherKey) ? expiredKeyStBitmap : keyStBitmap);
					num3++;
				}
				else if (item3.appExeKeyStatus == AppExeKeyStatus.Expired || item3.appExeKeyStatus == AppExeKeyStatus.OlderThanDeviceSeed || item3.appExeKeyStatus == AppExeKeyStatus.OlderThanPublisherKey)
				{
					bitmap3 = expiredKeyStBitmap;
					num3++;
				}
				else
				{
					bitmap3 = nothingStBitmap;
				}
				string text5 = "-";
				if (item3.IsUsbConnected)
				{
					if (item3.deviceType == DeviceType.Android)
					{
						text5 = "OK";
					}
					else if (item3.deviceType == DeviceType.PS_Vita)
					{
						text5 = ((!item3.IsDevAssistantOn) ? "Not Ready" : "OK");
					}
				}
				else
				{
					text5 = "-";
				}
				int index = dataGridDeviceList.Rows.Add(bitmap, item3.Nickname, text5, bitmap3, item3.HasSeed ? seedStBitmap : nothingStBitmap, bitmap3, item3.strAppExeEpiration, item3.DeviceID);
				if (item3.appExeKeyStatus == AppExeKeyStatus.Expired)
				{
					dataGridDeviceList.Rows[index].Cells["KmExpirationDate"].Style.ForeColor = Color.Red;
					dataGridDeviceList.Rows[index].Cells["KmExpirationDate"].Style.SelectionForeColor = Color.Red;
				}
				dataGridDeviceList.Rows[index].Cells["KmSeedKeyState"].ToolTipText = bitmap2.Tag.ToString();
				if (item3.DeviceID != item3.Nickname)
				{
					dataGridDeviceList.Rows[index].Cells["KmDeviceName"].ToolTipText = $"Device ID = \"{item3.DeviceID}\"";
				}
				else
				{
					dataGridDeviceList.Rows[index].Cells["KmDeviceName"].ToolTipText = $"Device ID = \"{item3.DeviceID}\"";
				}
				if (!item3.IsUsbConnected)
				{
					string text8 = (dataGridDeviceList.Rows[index].Cells["KmDeviceType"].ToolTipText = (dataGridDeviceList.Rows[index].Cells["KmConnection"].ToolTipText = Utility.TextLanguage($"The device {item3.Name2} is not connected.", $"デバイス {item3.Name2} は接続されていません。")));
				}
				else if (item3.deviceType == DeviceType.Android)
				{
					string text11 = (dataGridDeviceList.Rows[index].Cells["KmDeviceType"].ToolTipText = (dataGridDeviceList.Rows[index].Cells["KmConnection"].ToolTipText = Utility.TextLanguage($"Communication with the device {item3.Name2} is enabled.", $"デバイス {item3.Name2} と通信可能です。")));
				}
				else if (item3.deviceType == DeviceType.PS_Vita)
				{
					if (!item3.IsDevAssistantOn)
					{
						string text14 = (dataGridDeviceList.Rows[index].Cells["KmDeviceType"].ToolTipText = (dataGridDeviceList.Rows[index].Cells["KmConnection"].ToolTipText = Utility.TextLanguage($"DevAssistant of the device {item3.Name2} is not running. \nTo communicate with this device, start DevAssistant (PSM Dev) on Vita.", $"デバイス {item3.Name2} の DevAssistant が起動していません。\nこのデバイスと通信するにはVita内のDevAssistant(PSM Dev)を起動してください。")));
					}
					else
					{
						string text17 = (dataGridDeviceList.Rows[index].Cells["KmDeviceType"].ToolTipText = (dataGridDeviceList.Rows[index].Cells["KmConnection"].ToolTipText = Utility.TextLanguage($"Communication with the device {item3.Name2} is enabled.", $"デバイス {item3.Name2} と通信可能です。")));
					}
				}
				if (item3.HasSeed)
				{
					dataGridDeviceList.Rows[index].Cells["KmSeed"].ToolTipText = Utility.TextLanguage($"The Device Seed is created from the device {item3.Name2}. \nThe Device Seed is used to create the App Key.", $"デバイス {item3.Name2} から作成されたデバイスシードです。\nデバイスシードはアプリ鍵の作成に利用されます。");
				}
				else
				{
					dataGridDeviceList.Rows[index].Cells["KmSeed"].ToolTipText = Utility.TextLanguage($"The Device Seed of device {item3.Name2} has not been created. \nThe Device Seed is used to create the App Key.", $"デバイス {item3.Name2} のデバイスシードは作成されていません。\nデバイスシードはアプリ鍵の作成に利用されます。");
				}
				if (item3.appExeKeyStatus == AppExeKeyStatus.OK && !string.IsNullOrEmpty(text))
				{
					dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage($"PSM app of Application ID {item3.Name2} can be executed on device {text}.", $"デバイス {item3.Name2} で \nApplication ID が {text} のPSMアプリが実行可能です。");
				}
				else if (item3.appExeKeyStatus == AppExeKeyStatus.Expired)
				{
					dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage("The App Key has expired. \nPlease update the App Key.", "アプリ鍵の有効期限が切れています。\nアプリ鍵を更新してください。");
				}
				else if (item3.appExeKeyStatus == AppExeKeyStatus.OlderThanPublisherKey)
				{
					dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage("This App Key is invalid because the Publisher Key has been updated. \nPlease update the App Key.", "パブリッシャ鍵が更新されたため、このアプリ鍵は無効になりました。\nアプリ鍵を更新してください。");
				}
				else if (item3.appExeKeyStatus == AppExeKeyStatus.OlderThanDeviceSeed)
				{
					dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage("This App Key is invalid because the Device Seed has been updated. \nPlease update the App Key.", "デバイスシードが更新されたため、このアプリ鍵は無効になりました。\nアプリ鍵を更新してください。");
				}
				else if (item3.appExeKeyStatus == AppExeKeyStatus.None)
				{
					if (!string.IsNullOrEmpty(text))
					{
						dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage(string.Format("An App Key has not been created. \nThis App Key must be created in order to execute the PSM App of Application ID {1} on this device.", item3.Name2, text), string.Format("アプリ鍵は作成されていません。\nこのデバイスでApplication ID {1}のPSMアプリを実行するためには、このアプリ鍵を作成する必要があります。", item3.Name2, text));
					}
					else
					{
						dataGridDeviceList.Rows[index].Cells["KmAppKey"].ToolTipText = Utility.TextLanguage($"An App Key has not been created.", $"アプリ鍵は作成されていません。");
					}
				}
			}
			if (num != -1 && dataGridDeviceList.RowCount > num)
			{
				dataGridDeviceList[0, num].Selected = true;
			}
			kmUpdatingDeviceList = false;
			if (Program.appConfigData.GetCountOfDeviceSeedorConnectedDevice() > 0)
			{
				buttonExportDeviceSeed.Enabled = true;
			}
			else
			{
				buttonExportDeviceSeed.Enabled = false;
			}
			if (Program.appConfigData.GetCountOfDeviceSeed() > 0)
			{
				toolStripMenuItemDeleteDeviceSeed.Enabled = true;
			}
			else
			{
				toolStripMenuItemDeleteDeviceSeed.Enabled = false;
			}
			if (num3 > 0)
			{
				toolStripMenuItemDeleteAppKey.Enabled = true;
			}
			else
			{
				toolStripMenuItemDeleteAppKey.Enabled = false;
			}
			if (Program.appConfigData.GetCountOfDeviceSeed() == 0 && num3 == 0)
			{
				deleteAllDeviceSeedAndAppKeyToolStripMenuItem.Enabled = false;
			}
			else
			{
				deleteAllDeviceSeedAndAppKeyToolStripMenuItem.Enabled = true;
			}
		}

		internal void UpdateDeviceList(int num)
		{
			if (kmEdittingNickname)
			{
				return;
			}
			try
			{
				Program.appConfigData.UpdateDeviceList(Program.appConfigData.onlineDevices, num);
				RefreshDataGridDeviceList();
				if (paSelectedGuid == default(Guid) && deviceDataGridView.SelectedRows.Count > 0)
				{
					paUpdatingDeviceList = false;
					deviceAppsDataGridView_SelectionChanged(deviceDataGridView, new EventArgs());
				}
				paUpdatingDeviceList = true;
				bool flag = false;
				if (deviceDataGridView.SelectedRows.Count > 0)
				{
					paSelectedGuid = (Guid)deviceDataGridView.SelectedRows[0].Cells["PaGUID"].Value;
					flag = (Bitmap)deviceDataGridView.SelectedRows[0].Cells["PaDeviceType"].Value == vitaOffBitmap;
				}
				deviceDataGridView.Rows.Clear();
				foreach (DeviceStatus item in Program.appConfigData.listDevice)
				{
					if (!item.IsUsbConnected)
					{
						continue;
					}
					Bitmap bitmap = ((item.deviceType == DeviceType.Android) ? androidBitmap : (item.IsDevAssistantOn ? vitaBitmap : vitaOffBitmap));
					int index = deviceDataGridView.Rows.Add(bitmap, item.Nickname, item.DeviceGUID);
					if (item.DeviceGUID == paSelectedGuid)
					{
						deviceDataGridView.CurrentCell = deviceDataGridView.Rows[index].Cells[0];
						if (flag && item.deviceType == DeviceType.PS_Vita && item.IsDevAssistantOn)
						{
							ListAppsBG(item.DeviceGUID);
						}
					}
				}
				paUpdatingDeviceList = false;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private void buttonGeneratePublisherKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			PublisherKeyUtility.GeneratePublishKey(PublisherKeyType.Developer);
			RefreshKeyManagementPanel();
			RestartCheckDeviceConnection();
		}

		private void buttonCheckPublisherKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			PublisherKeyUtility.CheckPublisherKey();
			RestartCheckDeviceConnection();
		}

		private void buttonExportPublisherKey_Click(object sender, EventArgs e)
		{
			PublisherKeyUtility.ExportPublisherKey();
			RefreshKeyManagementPanel();
		}

		private void buttonImportPublisherKey_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonImportPublisherKey_Click()");
			StopCheckDeviceConnection();
			PublisherKeyUtility.ImportPublisherKeyWithCheck();
			RefreshKeyManagementPanel();
		}

		private void buttonRegisterApplicationID_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonRegisterAppXml_Click()");
			StopCheckDeviceConnection();
			Program.appConfigData.mainForm = this;
			Form form = new GenerateAppKeyRingOpenAppXml();
			DialogResult dialogResult = form.ShowDialog();
			if (dialogResult != DialogResult.OK)
			{
				RestartCheckDeviceConnection();
				return;
			}
			Directory.CreateDirectory(string.Concat(str1: (!(Program.appConfigData.AppID == "*")) ? Program.appConfigData.AppID : "+asterisk+", str0: Program.PATH_DIR_TARGET_APPS_KEY));
			RefreshKeyManagementPanel();
			RestartCheckDeviceConnection();
		}

		private void buttonRemoveApplicationID_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonRemoveAppXml_Click()");
			StopCheckDeviceConnection();
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Select Application ID", "Application IDを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			string text = currentCell.Value.ToString();
			string text2 = ((!(text == "*")) ? text : "+asterisk+");
			DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage($"Do you want to delete {text} from Application ID list?", $"{text} を Application IDリストから削除しますか？"), "Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.No)
			{
				RestartCheckDeviceConnection();
				return;
			}
			try
			{
				if (Directory.Exists(Program.PATH_DIR_TARGET_APPS_KEY + text2))
				{
					Directory.Delete(Program.PATH_DIR_TARGET_APPS_KEY + text2, recursive: true);
				}
				if (File.Exists(Program.PATH_DIR_HOST_APPS_KEY + text2 + ".khapp"))
				{
					File.Delete(Program.PATH_DIR_HOST_APPS_KEY + text2 + ".khapp");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			RefreshKeyManagementPanel();
			RestartCheckDeviceConnection();
		}

		private void buttonUpdateDeviceSeedAndAppKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			Console.WriteLine("buttonUpdateKey_Click()");
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Select Application ID", "Application IDを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			if (dataGridDeviceList.CurrentRow == null)
			{
				MessageBox.Show(Utility.TextLanguage("To update an App Key, connect the device to a PC with a USB cable or import the Device Seed.", "アプリ鍵を更新するには、デバイスをUSBケーブルでPCに接続する、もしくはデバイスシードをインポートしてください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			if (dataGridViewCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Please select device.", "デバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			string text = currentCell.Value.ToString();
			string text2 = dataGridViewCell.Value.ToString();
			DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage($"Do you want to update Device Seed and App Key ({text2} - {text})?", $"デバイスシードとアプリ鍵 ({text2} - {text}) を更新しますか？"), "Publishing Utility", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (dialogResult == DialogResult.No)
			{
				RestartCheckDeviceConnection();
				return;
			}
			if (KeyUtility.UpdateKey2(text2, text) == KeyUtility.KeyCheckResult.Valid_NoUpdate)
			{
				MessageBox.Show(Utility.TextLanguage($"App Key ({text2} - {text}) exists.", $"アプリ鍵 ({text2} - {text}) は存在します。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			RefreshDataGridDeviceList();
			RestartCheckDeviceConnection();
		}

		private void buttonGenerateDeviceSeed_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonGenerateDeviceSeed_Click()");
			StopCheckDeviceConnection();
			if (dataGridDeviceList.CurrentRow == null)
			{
				MessageBox.Show(Utility.TextLanguage("Please select device.", "デバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			if (dataGridViewCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Please select device.", "デバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			string text = dataGridViewCell.Value.ToString();
			if (DeviceSeedUtility.GenerateDeviceSeed(text))
			{
				MessageBox.Show(Utility.TextLanguage($"Succeeded to generate Device Seed of {text}.", $"デバイスシード({text}) の作成に成功しました。"), "Generate Device Seed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show(Utility.TextLanguage($"Failed to generate Device Seed of {text}.", $"{text} のデバイスシードの作成に失敗しました。"), "Generate Device Seed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			RestartCheckDeviceConnection();
		}

		private void buttonExportDeviceSeed_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonExportDeviceSeed_Click()");
			StopCheckDeviceConnection();
			if (dataGridDeviceList.CurrentRow == null || dataGridDeviceList.CurrentRow.Cells["KmDeviceID"] == null)
			{
				MessageBox.Show(Utility.TextLanguage("Device is not selected.\nPlease select device in list box.", "デバイスが選択されていません。\nデバイスリストでデバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			string text = dataGridViewCell.Value.ToString();
			DeviceStatus deviceStatus = null;
			for (int i = 0; i < Program.appConfigData.listDevice.Count; i++)
			{
				if (Program.appConfigData.listDevice[i].DeviceID == text)
				{
					deviceStatus = Program.appConfigData.listDevice[i];
				}
			}
			if (deviceStatus == null)
			{
				RestartCheckDeviceConnection();
				return;
			}
			if (!deviceStatus.HasSeed)
			{
				DialogResult dialogResult = MessageBox.Show(Utility.TextLanguage($"The Device Seed of this device {deviceStatus.DeviceTypeID} has not been created. \nDo you want to create one?", $"このデバイス ({deviceStatus.DeviceTypeID}) のデバイスシードが作成されていません。\n作成しますか？"), "Export Device Seed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dialogResult != DialogResult.Yes)
				{
					RestartCheckDeviceConnection();
					return;
				}
				if (!DeviceSeedUtility.GenerateDeviceSeed(text))
				{
					RestartCheckDeviceConnection();
					return;
				}
				Program.appConfigData.GetDeviceInfoInAppDataFolder();
			}
			Form form = new ExportDeviceSeed01(deviceStatus);
			form.ShowDialog();
			dataGridApplicationID_SelectionChanged(null, null);
			RestartCheckDeviceConnection();
		}

		private void buttonImportDeviceSeed_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Resources.openSeedFileDialogTitle_Text;
			openFileDialog.Filter = Resources.openSeedFileDialogFilter_Text;
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "seed";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				DeviceStatus deviceStatus = new DeviceStatus(openFileDialog.FileName);
				if (deviceStatus.IsCorrect)
				{
					string fileName = Path.GetFileName(openFileDialog.FileName);
					File.Copy(openFileDialog.FileName, Utility.UserAppDataPath + "\\DeviceSeed\\" + fileName, overwrite: true);
					MessageBox.Show("Succeed to import seed file.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					Program.appConfigData.GetDeviceInfoInAppDataFolder();
					RefreshKeyManagementPanel();
				}
			}
		}

		private void buttonGererateAppKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Select Application ID in Application ID list.", "Application IDリスト内でApplication IDを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			if (dataGridDeviceList.CurrentRow == null)
			{
				MessageBox.Show(Utility.TextLanguage("To create an App Key, connect the device to a PC with a USB cable or import the Device Seed.", "アプリ鍵を作成するには、デバイスをUSBケーブルでPCに接続する、もしくはデバイスシードをインポートしてください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			if (dataGridViewCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Please select device.", "デバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			string text = currentCell.Value.ToString();
			string text2 = dataGridViewCell.Value.ToString();
			if (!File.Exists(Program.PATH_DIR_DEVICE_SEED + text2 + ".seed"))
			{
				MessageBox.Show(Utility.TextLanguage($"The Device Seed of device {text2} has not been created. \nBefore creating an App Key, a Device Seed must be created.", $"デバイス {text2} のデバイスシードが作成されていません。\nアプリ鍵を作成する前に、デバイスシードを作成する必要があります。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
			}
			else if (!KeyUtility.SignInSENID_PasswordDialog())
			{
				RestartCheckDeviceConnection();
			}
			else if (!AppKeyUtility.GenerateAppDevKey(text, Program.appConfigData.PsnID, Program.appConfigData.Password))
			{
				RestartCheckDeviceConnection();
			}
			else if (!AppKeyUtility.GenerateAppExeKey(text2, text, Program.appConfigData.PsnID, Program.appConfigData.Password))
			{
				RestartCheckDeviceConnection();
			}
			else
			{
				RefreshKeyManagementPanel();
				RestartCheckDeviceConnection();
			}
		}

		private void buttonExportAppKeyRing_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Select Application ID", "Application IDを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			string text = currentCell.Value.ToString();
			string text2 = "";
			text2 = ((!(text == "*")) ? text : "+asterisk+");
			if (!AppKeyUtility.ExistAppKey(text2))
			{
				MessageBox.Show(Utility.TextLanguage($"Because an App Key of {text2} does not exist, it cannot be exported. \nBefore exporting the key ring, an App Key must be created.", $"{text2} のアプリ鍵が存在しないため、エクスポートできません。\n鍵束をエクスポートする前に、アプリ鍵を作成する必要があります。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				RestartCheckDeviceConnection();
				return;
			}
			Program.appConfigData.AppID = text2;
			if (!string.IsNullOrEmpty(Program.appConfigData.AppID))
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.Title = "Export App Key Ring of '" + Program.appConfigData.AppID + "'";
				saveFileDialog.Filter = "App Key Ring|*.krng|All files|*.*";
				saveFileDialog.FileName = Program.appConfigData.AppID + ".krng";
				saveFileDialog.FilterIndex = 0;
				saveFileDialog.RestoreDirectory = true;
				saveFileDialog.CheckPathExists = true;
				saveFileDialog.OverwritePrompt = true;
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					if (!AppKeyUtility.GenerateAppKeyRing(text2))
					{
						RestartCheckDeviceConnection();
						return;
					}
					string text3 = Program.PATH_DIR_TARGET_APPS_KEY + Program.appConfigData.AppID + "\\" + Program.appConfigData.AppID + ".krng";
					if (File.Exists(text3))
					{
						File.Copy(text3, saveFileDialog.FileName, overwrite: true);
					}
					else
					{
						MessageBox.Show($"Cannot find {text3}.");
					}
				}
			}
			RestartCheckDeviceConnection();
		}

		private void buttonImportAppKeyRing_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonImportAppKeyRing_Click()");
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Import App Key Ring";
			openFileDialog.Filter = "App Key Ring file|*.krng|All files|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "krng";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				AppKeyRing appKeyRing = new AppKeyRing();
				if (appKeyRing.LoadFile(openFileDialog.FileName))
				{
					appKeyRing.GenerateFileofAppDevExeKey();
				}
			}
			Program.appConfigData.GetAppDevKey();
			RefreshKeyManagementPanel();
		}

		private void toolStripMenuItemDeleteAppKeyRing_Click(object sender, EventArgs e)
		{
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				return;
			}
			Console.WriteLine(currentCell.Value.ToString());
			string text = currentCell.Value.ToString();
			if (text == "*")
			{
				text = "+asterisk+";
			}
			if (!string.IsNullOrEmpty(text))
			{
				string path = Utility.UserAppDataPath + "\\HostAppsKey\\" + text + ".khapp";
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				string path2 = Utility.UserAppDataPath + "\\TargetAppsKey\\\\" + text + "\\";
				if (Directory.Exists(path2))
				{
					Directory.Delete(path2, recursive: true);
				}
			}
			Program.appConfigData.GetAppDevKey();
			RefreshKeyManagementPanel();
		}

		private void toolStripMenuItemRenameDeviceSeed_Click(object sender, EventArgs e)
		{
			if (dataGridDeviceList.CurrentRow != null && dataGridDeviceList.CurrentRow.Cells["KmDeviceName"] != null)
			{
				dataGridDeviceList.CurrentCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceName"];
				dataGridDeviceList.BeginEdit(selectAll: true);
			}
		}

		private void toolStripMenuItemDeleteDeviceSeed_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			if (dataGridDeviceList.CurrentRow == null || dataGridDeviceList.CurrentRow.Cells["KmDeviceName"] == null)
			{
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			if (dataGridViewCell == null)
			{
				MessageBox.Show(Utility.TextLanguage("Please select device.", "デバイスを選択してください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string text = dataGridViewCell.Value.ToString();
			DeviceStatus deviceStatus = null;
			foreach (DeviceStatus item in Program.appConfigData.listDevice)
			{
				if (item.DeviceID == text)
				{
					Console.WriteLine("find " + text);
					if (!item.HasSeed)
					{
						MessageBox.Show(item.Nickname + " doesn't have Device Seed.", "Delete Device Seed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						break;
					}
					Console.WriteLine("Try to delete " + text);
					string path = Program.PATH_DIR_DEVICE_SEED + item.DeviceID + ".seed";
					if (File.Exists(path))
					{
						File.Delete(path);
						deviceStatus = item;
						Console.WriteLine("delete " + text);
						break;
					}
					MessageBox.Show("Cannot find Device Seed '" + item.DeviceID + "'.", "Delete Device Seed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			if (deviceStatus != null)
			{
				Program.appConfigData.listDevice.Remove(deviceStatus);
			}
			RefreshDataGridDeviceList();
			RestartCheckDeviceConnection();
		}

		private void toolStripMenuItemDeleteAppKey_Click(object sender, EventArgs e)
		{
			Console.WriteLine("toolStripMenuItemDeleteAppKey_Click()");
			StopCheckDeviceConnection();
			if (dataGridDeviceList.CurrentRow == null || dataGridDeviceList.CurrentRow.Cells["KmDeviceName"] == null)
			{
				RestartCheckDeviceConnection();
				return;
			}
			DataGridViewCell dataGridViewCell = dataGridDeviceList.CurrentRow.Cells["KmDeviceID"];
			string text = dataGridViewCell.Value.ToString();
			Console.WriteLine("deviceID=" + text);
			DataGridViewCell currentCell = dataGridApplicationID.CurrentCell;
			if (currentCell == null)
			{
				RestartCheckDeviceConnection();
				return;
			}
			string text2 = currentCell.Value.ToString();
			Console.WriteLine("applicationID=" + text2);
			KeyUtility.DeleteAppExeKey(text, text2);
			KeyUtility.DeleteAppDevKey(text2);
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			Program.appConfigData.GetDeviceInfoConnectedPC();
			RefreshDataGridDeviceList();
			RestartCheckDeviceConnection();
		}

		private void ImportQAPublisherKey_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Import QA Publisher Key (SWQA)";
			openFileDialog.Filter = "Publisher key files|*.p12|All files|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "p12";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string text = Utility.UserAppDataPath + "\\PublisherKey\\";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string destFileName = text + "kdevQa.p12";
				File.Copy(openFileDialog.FileName, destFileName, overwrite: true);
				if (PublisherKeyUtility.GetPublisherKeyInfo(Program.PATH_FILE_PUBLISHER_KEY_QA, out var strCertCommonName, out var strCertCreateTime))
				{
					Program.appConfigData.QAPublisherKeyName = strCertCommonName;
					Program.appConfigData.CreateTimeOfQAPublisherKey = Utility.ConvertLocaleDateTime(strCertCreateTime);
					Program.appConfigData.dtCreateTimeOfQAPublisherKey = DateTime.Parse(strCertCreateTime);
					MessageBox.Show($"Succeed to import QA Publisher Key.\n\nPublisherKeyName = {strCertCommonName}\nCreateTimeOfPublisherKey = {Program.appConfigData.CreateTimeOfQAPublisherKey}\n", "Import QA Publisher Key", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					MessageBox.Show("Failed to get QA Publisher Key Info.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Program.appConfigData.QAPublisherKeyName = "";
					Program.appConfigData.CreateTimeOfQAPublisherKey = "";
				}
			}
			RefreshGui();
			RestartCheckDeviceConnection();
		}

		private void ExportQAPublisherKey_Click(object sender, EventArgs e)
		{
			if (!File.Exists(Program.PATH_FILE_PUBLISHER_KEY_QA))
			{
				MessageBox.Show("Can't find QA Publisher key", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string pATH_FILE_PUBLISHER_KEY_QA = Program.PATH_FILE_PUBLISHER_KEY_QA;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = "Export QA Publisher Key";
			saveFileDialog.Filter = "Publisher key files|*.p12|All files|*.*";
			saveFileDialog.FileName = "kdevQa.p12";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.OverwritePrompt = true;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				File.Copy(pATH_FILE_PUBLISHER_KEY_QA, saveFileDialog.FileName, overwrite: true);
			}
		}

		private void deleteAllDeviceSeedAndAppKeyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			KeyUtility.DeleteAllDeviceSeedAndAppKey2();
			Program.appConfigData.GetDeviceInfoInAppDataFolder();
			Program.appConfigData.GetDeviceInfoConnectedPC();
			RefreshKeyManagementPanel();
		}

		private void dataGridDeviceList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			kmEdittingNickname = true;
		}

		private void dataGridDeviceList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			kmEdittingNickname = false;
		}

		private void dataGridDeviceList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!IsFormInitialized)
			{
				return;
			}
			foreach (DeviceStatus item in Program.appConfigData.listDevice)
			{
				if (!(item.DeviceID == dataGridDeviceList.CurrentRow.Cells["KmDeviceID"].Value.ToString()))
				{
					continue;
				}
				if (!item.HasSeed && (string)dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value != item.Nickname)
				{
					MessageBox.Show(item.Nickname + Utility.TextLanguage("\nSince a Device Seed is not created, changing name is not possible.", "\nデバイスシードが作成されていないため、変更できません。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value = item.Nickname;
					break;
				}
				string text = "";
				if (dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value == null)
				{
					dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value = item.Nickname;
					break;
				}
				text = dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value.ToString();
				if (!Utility.IsSafeCharactor(text))
				{
					MessageBox.Show(Resources.onlyUseXCharactor_Text, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value = item.Nickname;
					break;
				}
				if (text.Length > 31)
				{
					MessageBox.Show(string.Format(Resources.enterWithinX_Text, 31), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value = item.Nickname;
					break;
				}
				item.Nickname = text;
				string text2 = Program.PATH_DIR_DEVICE_SEED + item.DeviceID + ".seed";
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				item.Save(text2);
				break;
			}
		}

		private void dataGridDeviceList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			if (!IsFormInitialized || kmUpdatingDeviceList)
			{
				return;
			}
			foreach (DeviceStatus item in Program.appConfigData.listDevice)
			{
				if (item.DeviceID == dataGridDeviceList.CurrentRow.Cells["KmDeviceID"].Value.ToString())
				{
					string nickname = dataGridDeviceList.CurrentRow.Cells["KmDeviceName"].Value.ToString();
					if (!DeviceStatus.IsDeviceNicknameValid(nickname))
					{
						e.Cancel = true;
						break;
					}
				}
			}
		}

		private void dataGridDeviceList_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		private void StopCheckDeviceConnection()
		{
			Program.CheckDeviceConnection = false;
		}

		private void RestartCheckDeviceConnection()
		{
			Program.CheckDeviceConnection = true;
		}

		private void installAppButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Resources.openPackageFileDialogTitle_Text;
			openFileDialog.Filter = Resources.openPackageFileDialogFilter_Text;
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "PSMP";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string fileName = openFileDialog.FileName;
			if (!Utility.CheckFileMagicNumber(fileName, 1095586640) && !Utility.CheckFileMagicNumber(fileName, 67324752))
			{
				MessageBox.Show(Utility.TextLanguage("This file is not psmp/psdp format.", "このファイルはpsmp/psdp形式ではありません。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			InstallPakageAppName installPakageAppName = new InstallPakageAppName(Path.GetFileNameWithoutExtension(fileName));
			if (installPakageAppName.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
			_ = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag;
			string path = Utility.UserAppDataPath + "\\PublisherKey\\kdev.p12";
			if (!File.Exists(path))
			{
				MessageBox.Show("Can't find Publisher Key.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			byte[] bsKdev = File.ReadAllBytes(path);
			string text = "*";
			string text2 = "+asterisk+";
			if (Path.GetExtension(fileName) != ".psdp")
			{
				try
				{
					text = (text2 = Package.ReadTitleIdentifierFromPackage(fileName));
					if (string.IsNullOrEmpty(text))
					{
						return;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			if (text == "*")
			{
				text2 = "+asterisk+";
			}
			StopCheckDeviceConnection();
			long num = 0L;
			num = KeyUtility.ReadAccountIdFromKdevP12(bsKdev);
			Console.WriteLine("Check ExistAppExeKey() " + text);
			bool flag = false;
			int num2 = PsmDeviceFuncBinding.ExistAppExeKey(guid, num, text, Program.appConfigData.EnvServer);
			if (num2 != 1 || flag)
			{
				string text3 = deviceDataGridView.CurrentRow.Cells["PaDeviceName"].Value.ToString();
				string text4 = Utility.UserAppDataPath + "\\TargetAppsKey\\" + text2 + "\\" + text3 + ".ktapp";
				if (!File.Exists(text4))
				{
					MessageBox.Show(Utility.TextLanguage($"Can't find App Key of {text4}.\nPlease generate or import App Key '{text}' with the Publisher Key used to create the PSMP file.", $"アプリ鍵\n{text4}がみつかりません。\nこのpsdp, psmpファイルを作成したときに使用したパブリッシャ鍵をインポートし、'{text}' のアプリ鍵を作成もしくはインポートしてください。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					RestartCheckDeviceConnection();
					return;
				}
				num2 = PsmDeviceFuncBinding.SetAppExeKey(guid, text4);
				if (num2 < 0)
				{
					RestartCheckDeviceConnection();
					return;
				}
			}
			string text5 = installPakageAppName.appNameTextBox.Text;
			if (PsmDeviceFuncBinding.Install(guid, fileName, text5) == 0)
			{
				ListAppsBlock(guid);
				MessageBox.Show(string.Format(Resources.installAppsDialogBody_Text, text5), Resources.installAppsDialogTitle_Text, MessageBoxButtons.OK, MessageBoxIcon.None);
			}
			RestartCheckDeviceConnection();
		}

		private void buttonInstallAppQA_Click(object sender, EventArgs e)
		{
			Console.WriteLine("buttonInstallAppQA_Click()");
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = Resources.openPackageFileDialogTitle_Text;
			openFileDialog.Filter = "PSM Master Package(*.psmp)|*.psmp|All files(*.*)|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "PSMP";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			string fileName = openFileDialog.FileName;
			if (!Utility.CheckFileMagicNumber(fileName, 1095586640))
			{
				MessageBox.Show(Utility.TextLanguage("This file is not psmp format.", "このファイルはpsmp形式ではありません。"), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			InstallPakageAppName installPakageAppName = new InstallPakageAppName(Path.GetFileNameWithoutExtension(fileName));
			if (installPakageAppName.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
			_ = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag;
			string text = "*";
			string text2 = "+asterisk+";
			if (Path.GetExtension(fileName) != ".psdp")
			{
				try
				{
					text = (text2 = Package.ReadTitleIdentifierFromPackage(fileName));
					if (string.IsNullOrEmpty(text))
					{
						return;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			if (text == "*")
			{
				text2 = "+asterisk+";
			}
			StopCheckDeviceConnection();
			Console.WriteLine("Check ExistAppExeKey() " + text);
			string text3 = Utility.UserAppDataPath + "\\PublisherKey\\kdevQa.p12";
			if (!File.Exists(text3))
			{
				MessageBox.Show("Can't find Publisher Key QA.\n" + text3, "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			byte[] bsKdev = File.ReadAllBytes(text3);
			long num = KeyUtility.ReadAccountIdFromKdevP12(bsKdev);
			long accountID = 0L;
			if (!Utility.GetAccountIDFromPSMP(fileName, out accountID))
			{
				return;
			}
			if (accountID == -1)
			{
				MessageBox.Show($"You cannot install this {fileName} because AccountID is -1.)\n\nPSMP file need to be built in more than PSM SDK 1.00.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			MessageBox.Show($" guid={guid}\n\n AccountID_psmp={accountID}, (0x{accountID:x})\n AccountID_PublisherKeyQa={num}, (0x{num:x})\n\n TitleIdentifier={text}\n EnvServer={Program.appConfigData.EnvServer}\n", "psmp and Publisher Key Info (SWQA).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			int num2 = PsmDeviceFuncBinding.ExistAppExeKey(guid, accountID, text, Program.appConfigData.EnvServer);
			string text4 = deviceDataGridView.CurrentRow.Cells["PaDeviceName"].Value.ToString();
			string filename = Utility.UserAppDataPath + "\\TargetAppsKey\\" + text2 + "\\" + text4 + ".ktapp";
			string text5 = Utility.UserAppDataPath + "\\DeviceSeed\\" + text4 + ".seed";
			DeviceSeedUtility.GenerateDeviceSeedofOnlineDevices(regenerateAll: true);
			DeviceStatus deviceStatus = null;
			if (File.Exists(text5))
			{
				deviceStatus = new DeviceStatus(text5);
				if (!deviceStatus.IsCorrect)
				{
					MessageBox.Show($"{deviceStatus.Nickname} is invalid.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					RestartCheckDeviceConnection();
					return;
				}
				MessageBox.Show(Utility.TextLanguage($"Start to generate and install \nPSMP App Key ({text2} - {text4})\nto device.", $"psmp用アプリ鍵 ({text2} - {text4})\nを作成して、デバイスにインストールします。"), "Publishing Utility(Internal)");
				Form form = new SignInSENID_Password(" - Install App (QA)");
				DialogResult dialogResult = form.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					RestartCheckDeviceConnection();
					return;
				}
				if (!AppKeyUtility.GenerateAppExeKeyQA(text, Program.appConfigData.PsnID, Program.appConfigData.Password, accountID, deviceStatus))
				{
					MessageBox.Show(Utility.TextLanguage($"Failed to generate App Key {text}.", $"アプリ鍵 {text}\nの作成に失敗しました。"), "Publishing Utility(Internal)", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					RestartCheckDeviceConnection();
					return;
				}
				num2 = PsmDeviceFuncBinding.SetAppExeKey(guid, filename);
				if (num2 < 0)
				{
					RestartCheckDeviceConnection();
					return;
				}
				string text6 = installPakageAppName.appNameTextBox.Text;
				if (PsmDeviceFuncBinding.Install(guid, fileName, text6) == 0)
				{
					ListAppsBlock(guid);
					MessageBox.Show(string.Format(Resources.installAppsDialogBody_Text, text6), Resources.installAppsDialogTitle_Text, MessageBoxButtons.OK, MessageBoxIcon.None);
				}
				RestartCheckDeviceConnection();
			}
			else
			{
				MessageBox.Show($"Cannot find {text5}.", "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				RestartCheckDeviceConnection();
			}
		}

		private void uninstallAppButton_Click(object sender, EventArgs e)
		{
			if (Program.appConfigData.GetNumOfOnlineDevice() == 0)
			{
				MessageBox.Show(Utility.TextLanguage("Can't delete app because the device is not connected to the PC.", "デバイスがPCに接続されていないので、削除できません。"), "Uninstall PSM App - Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			UninstallAppsConfirmationDialog uninstallAppsConfirmationDialog = new UninstallAppsConfirmationDialog();
			uninstallAppsConfirmationDialog.uninstallAppsListBox.Items.Clear();
			DataGridViewSelectedRowCollection selectedRows = appsDataGridView.SelectedRows;
			foreach (DataGridViewRow item2 in selectedRows)
			{
				string item = item2.Cells["PaAppName"].Value as string;
				uninstallAppsConfirmationDialog.uninstallAppsListBox.Items.Add(item);
			}
			if (uninstallAppsConfirmationDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
			_ = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag;
			int num = 0;
			foreach (DataGridViewRow item3 in selectedRows)
			{
				string appId = item3.Cells["PaAppName"].Value as string;
				PsmDeviceFuncBinding.Uninstall(guid, appId);
				num++;
			}
			if (num != 0)
			{
				ListAppsBlock(guid);
				MessageBox.Show(Resources.uninstallAppsDialogBody_Text, Resources.uninstallAppsDialogTitle_Text, MessageBoxButtons.OK, MessageBoxIcon.None);
			}
		}

		private void launchAppButton_Click(object sender, EventArgs e)
		{
			string appId = appsDataGridView["PaAppName", appsDataGridView.CurrentRow.Index].Value as string;
			Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
			_ = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag;
			PsmDeviceFuncBinding.ClearLog(guid);
			consoleTextBox.Clear();
			PsmDeviceFuncBinding.Launch(guid, appId, debug: false, profile: false, keepnet: true, logwaiting: false, "");
		}

		private void killAppButton_Click(object sender, EventArgs e)
		{
			Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
			PsmDeviceFuncBinding.Kill(guid);
		}

		private void deviceAppsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView dataGridView = sender as DataGridView;
			if (dataGridView == deviceDataGridView)
			{
				bool flag = false;
				if (dataGridView.SelectedRows.Count >= 1)
				{
					foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
					{
						string text = (selectedRow.Cells["PaDeviceType"].Value as Bitmap).Tag as string;
						if (text != "vitaOff")
						{
							flag = true;
							break;
						}
					}
					Button button = installAppButton;
					bool enabled = (killAppButton.Enabled = flag);
					button.Enabled = enabled;
				}
			}
			else if (dataGridView == appsDataGridView && deviceDataGridView.CurrentRow != null)
			{
				string text2 = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag as string;
				bool flag3 = text2 != "vitaOff";
				launchAppButton.Enabled = flag3 && dataGridView.SelectedRows.Count == 1;
				uninstallAppButton.Enabled = flag3 && dataGridView.SelectedRows.Count >= 1;
			}
			if (dataGridView != deviceDataGridView || paUpdatingDeviceList)
			{
				return;
			}
			if (dataGridView.SelectedRows.Count == 1)
			{
				Guid guid = new Guid(deviceDataGridView.CurrentRow.Cells["PaGUID"].Value.ToString());
				if (!(paSelectedGuid != guid))
				{
					return;
				}
				paSelectedGuid = guid;
				string text3 = (deviceDataGridView.CurrentRow.Cells["PaDeviceType"].Value as Bitmap).Tag as string;
				if (text3 != "vitaOff")
				{
					ListAppsBG(guid);
					if (checkBoxEnableLog.Checked)
					{
						GetLogBG(guid);
						consoleTextBox.Clear();
					}
				}
				else
				{
					appsDataGridView.Rows.Clear();
				}
			}
			else
			{
				appsDataGridView.Rows.Clear();
			}
		}

		internal int ListApps(Guid guid)
		{
			int num = PsmDeviceFuncBinding.ListApplications(guid, appsArray);
			if (num >= 0)
			{
				appsNum = num;
			}
			return num;
		}

		private void ListAppsBlock(Guid guid)
		{
			int num = ListApps(guid);
			if (num >= 0)
			{
				UpdateAppsInfoInList();
			}
		}

		private void ListAppsBG(Guid guid)
		{
			appsDataGridView.Rows.Clear();
			if (listAppsThread != null)
			{
				listAppsThread.Abort();
			}
			AbstractExecutor abstractExecutor = new ListAppsThread(this, guid);
			listAppsThread = new Thread(abstractExecutor.Execute);
			listAppsThread.IsBackground = true;
			listAppsThread.Name = "ListAppsThread";
			listAppsThread.Start();
		}

		internal void UpdateAppsInfoInList()
		{
			appsDataGridView.Rows.Clear();
			for (int i = 0; i < appsNum; i++)
			{
				string text = Utility.SafeConvertCharArrayToString(appsArray[i].name);
				string text2 = "";
				if (appsArray[i].size == 0)
				{
					text2 = "N/A";
				}
				else if (appsArray[i].size > 1048576)
				{
					float num = (float)appsArray[i].size / 1048576f;
					text2 = $"{num:f1} MiB";
				}
				else if (appsArray[i].size > 1024)
				{
					float num2 = (float)appsArray[i].size / 1024f;
					text2 = $"{num2:f1} KiB";
				}
				else
				{
					text2 = $"{appsArray[i].size} Bytes";
				}
				appsDataGridView.Rows.Add(text, keyBitmap, text2);
			}
		}

		private void GetLogBG(Guid guid)
		{
			StopLog();
			logModule = new OutputLogThread(this, guid);
			logThread = new Thread(logModule.Execute);
			logThread.IsBackground = true;
			logThread.Name = "logThread";
			logThread.Start();
		}

		private void StopLog()
		{
			if (logModule != null)
			{
				((OutputLogThread)logModule).Terminate();
				logModule = null;
			}
			if (logThread != null)
			{
				logThread.Abort();
				logThread = null;
			}
		}

		private void dropBoxPanel_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void dropBoxPanel_DragDrop(object sender, DragEventArgs e)
		{
			string text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
			_ = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: false);
			extractLogTextBox.Clear();
			string text2 = text;
			string extension = Path.GetExtension(text2);
			if (extension != ".psmp" && extension != ".psdp")
			{
				TextBox textBox = extractLogTextBox;
				textBox.Text = textBox.Text + "File extension is not \".psmp\" or \".psdp\"" + Environment.NewLine;
				TextBox textBox2 = extractLogTextBox;
				textBox2.Text = textBox2.Text + $"Input file    : \"{text2}\"" + Environment.NewLine;
				return;
			}
			TextBox textBox3 = extractLogTextBox;
			textBox3.Text = textBox3.Text + "Extracting psmp package" + Environment.NewLine;
			TextBox textBox4 = extractLogTextBox;
			textBox4.Text = textBox4.Text + $"Input file    : \"{text2}\"" + Environment.NewLine;
			string text3 = text2 + ".extracted";
			TextBox textBox5 = extractLogTextBox;
			textBox5.Text = textBox5.Text + $"Extract dir   : \"{text3}\"" + Environment.NewLine;
			int num = PsmDeviceFuncBinding.ExtractPackage(text3, text2);
			if (num >= 0)
			{
				TextBox textBox6 = extractLogTextBox;
				textBox6.Text = textBox6.Text + $" ** Done ** " + Environment.NewLine;
			}
		}

		private void appsDataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			if (e.Column == appsDataGridView.Columns["PaAppSize"])
			{
				string text = ((e.CellValue1 == null) ? "" : e.CellValue1.ToString());
				string text2 = ((e.CellValue2 == null) ? "" : e.CellValue2.ToString());
				int num = text[text.Length - 3] - text2[text2.Length - 3];
				if (num != 0)
				{
					e.SortResult = num;
				}
				else
				{
					float num2 = float.Parse(text.Substring(0, text.Length - 4));
					float num3 = float.Parse(text2.Substring(0, text2.Length - 4));
					e.SortResult = (int)((num2 - num3) * 1000f);
				}
				e.Handled = true;
			}
		}

		public void ConsoleCallback(string msg)
		{
			consoleTextBox.AppendText(msg + Environment.NewLine);
		}

		private void appXmlSchemaCheck_Click(object sender, EventArgs e)
		{
			StopCheckDeviceConnection();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "app.xml Schema Check";
			openFileDialog.Filter = "Publisher key files|*.xml|All files|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.CheckFileExists = true;
			openFileDialog.CheckPathExists = true;
			openFileDialog.DereferenceLinks = true;
			openFileDialog.DefaultExt = "xml";
			openFileDialog.AddExtension = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK && Xml.LoadFromFile(openFileDialog.FileName, validate: true) != null)
			{
				MessageBox.Show($"{openFileDialog.FileName} is OK.");
			}
			RefreshGui();
			RestartCheckDeviceConnection();
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PublishingUtility.MainForm));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			submitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			preferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			proxySettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			deleteAllDeviceSeedAndAppKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			documentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			errorProvider1 = new System.Windows.Forms.ErrorProvider(components);
			productLabelText = new System.Windows.Forms.TextBox();
			contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			buttonExportDeviceSeed = new System.Windows.Forms.Button();
			buttonImportDeviceSeed = new System.Windows.Forms.Button();
			buttonExportAppKeyRing = new System.Windows.Forms.Button();
			buttonImportAppKeyRing = new System.Windows.Forms.Button();
			buttonGererateAppKey = new System.Windows.Forms.Button();
			buttonImportPublisherKey = new System.Windows.Forms.Button();
			buttonExportPublisherKey = new System.Windows.Forms.Button();
			buttonGenerateKey = new System.Windows.Forms.Button();
			killAppButton = new System.Windows.Forms.Button();
			launchAppButton = new System.Windows.Forms.Button();
			uninstallAppButton = new System.Windows.Forms.Button();
			installAppButton = new System.Windows.Forms.Button();
			textBoxSplash = new System.Windows.Forms.TextBox();
			textBoxIcon128 = new System.Windows.Forms.TextBox();
			textBoxIcon256 = new System.Windows.Forms.TextBox();
			textBoxIcon512 = new System.Windows.Forms.TextBox();
			cate1MetadataButton = new System.Windows.Forms.Button();
			cate2KeyManageButton = new System.Windows.Forms.Button();
			cate3PacageAppsButton = new System.Windows.Forms.Button();
			panelManager1 = new Controls.PanelManager();
			managedPanel1 = new Controls.ManagedPanel();
			panelMetadata = new System.Windows.Forms.Panel();
			saveAppXmlButton = new System.Windows.Forms.Button();
			tabControl1 = new System.Windows.Forms.TabControl();
			tabPage1 = new System.Windows.Forms.TabPage();
			propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			tabPage2 = new System.Windows.Forms.TabPage();
			nameDescriptionPanel = new System.Windows.Forms.Panel();
			appNameLabel = new System.Windows.Forms.Label();
			nameDescriptionLabel = new System.Windows.Forms.Label();
			titleLabel = new System.Windows.Forms.Label();
			appNameDataGridView = new System.Windows.Forms.DataGridView();
			dataGridView1Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridView1Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridView1Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			tabIcon = new System.Windows.Forms.TabPage();
			buttonGenerateImage128 = new System.Windows.Forms.Button();
			buttonGenerateImage256 = new System.Windows.Forms.Button();
			buttonOpenFileSplash = new System.Windows.Forms.Button();
			label18 = new System.Windows.Forms.Label();
			buttonOpenFileIcon128 = new System.Windows.Forms.Button();
			label17 = new System.Windows.Forms.Label();
			buttonOpenFileIcon256 = new System.Windows.Forms.Button();
			label16 = new System.Windows.Forms.Label();
			buttonOpenFileIcon512 = new System.Windows.Forms.Button();
			label15 = new System.Windows.Forms.Label();
			tabPage4 = new System.Windows.Forms.TabPage();
			textBoxCommonRating = new System.Windows.Forms.TextBox();
			textBoxParentalLockLevel = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			textBoxRatingResult = new System.Windows.Forms.TextBox();
			label10 = new System.Windows.Forms.Label();
			buttonStartRatingCheck = new System.Windows.Forms.Button();
			groupBox2 = new System.Windows.Forms.GroupBox();
			label9 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			textBoxEsrbCode = new System.Windows.Forms.TextBox();
			label7 = new System.Windows.Forms.Label();
			linkLabel_EsrbUrl = new System.Windows.Forms.LinkLabel();
			dropDownListEsrbRating = new System.Windows.Forms.ComboBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			label11 = new System.Windows.Forms.Label();
			checkBoxGambling = new System.Windows.Forms.CheckBox();
			pictureBox9 = new System.Windows.Forms.PictureBox();
			checkBoxSex = new System.Windows.Forms.CheckBox();
			checkBoxLanguage = new System.Windows.Forms.CheckBox();
			pictureBox4 = new System.Windows.Forms.PictureBox();
			pictureBox2 = new System.Windows.Forms.PictureBox();
			checkBoxScaryElements = new System.Windows.Forms.CheckBox();
			checkBoxDrugs = new System.Windows.Forms.CheckBox();
			pictureBox8 = new System.Windows.Forms.PictureBox();
			pictureBox7 = new System.Windows.Forms.PictureBox();
			label8 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			checkBoxDiscrimination = new System.Windows.Forms.CheckBox();
			checkBoxViolence = new System.Windows.Forms.CheckBox();
			checkBoxOnline = new System.Windows.Forms.CheckBox();
			pictureBox5 = new System.Windows.Forms.PictureBox();
			pictureBox3 = new System.Windows.Forms.PictureBox();
			TextBoxPEGINumber = new System.Windows.Forms.TextBox();
			DropDownListAgeRatingLogo = new System.Windows.Forms.ComboBox();
			PEGI_Link = new System.Windows.Forms.LinkLabel();
			label1 = new System.Windows.Forms.Label();
			tabPage3 = new System.Windows.Forms.TabPage();
			productLabel = new System.Windows.Forms.Label();
			purchasePanel = new System.Windows.Forms.Panel();
			addProductButton = new System.Windows.Forms.Button();
			supportLocalLabel = new System.Windows.Forms.Label();
			localeCheckedListBox = new System.Windows.Forms.CheckedListBox();
			managedPanel2 = new Controls.ManagedPanel();
			panelKeyManagement = new System.Windows.Forms.Panel();
			groupBox5 = new System.Windows.Forms.GroupBox();
			buttonUpdateDeviceSeedAndAppKey = new System.Windows.Forms.Button();
			buttonRemoveAppXml = new System.Windows.Forms.Button();
			buttonRegisterAppXml = new System.Windows.Forms.Button();
			buttonGenerateDeviceSeed = new System.Windows.Forms.Button();
			label14 = new System.Windows.Forms.Label();
			dataGridDeviceList = new System.Windows.Forms.DataGridView();
			KmDeviceType = new System.Windows.Forms.DataGridViewImageColumn();
			KmDeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			KmConnection = new System.Windows.Forms.DataGridViewTextBoxColumn();
			KmSeedKeyState = new System.Windows.Forms.DataGridViewImageColumn();
			KmSeed = new System.Windows.Forms.DataGridViewImageColumn();
			KmAppKey = new System.Windows.Forms.DataGridViewImageColumn();
			KmExpirationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			KmDeviceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			contextMenuStripDevices = new System.Windows.Forms.ContextMenuStrip(components);
			toolStripMenuItemDeleteDeviceSeed = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItemDeleteAppKey = new System.Windows.Forms.ToolStripMenuItem();
			labelDeviceList_AppKey = new System.Windows.Forms.Label();
			dataGridApplicationID = new System.Windows.Forms.DataGridView();
			KmAppId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			contextMenuStripApplicationID = new System.Windows.Forms.ContextMenuStrip(components);
			toolStripMenuItemDeleteApplicationID = new System.Windows.Forms.ToolStripMenuItem();
			groupBox4 = new System.Windows.Forms.GroupBox();
			buttonCheckPublisherKey = new System.Windows.Forms.Button();
			label12 = new System.Windows.Forms.Label();
			textBoxDateOfGenerateKey = new System.Windows.Forms.TextBox();
			textBoxDeveloperKey = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			managedPanel3 = new Controls.ManagedPanel();
			panelPackageApp = new System.Windows.Forms.Panel();
			buttonInstallAppQA = new System.Windows.Forms.Button();
			labelQAPublisherKey = new System.Windows.Forms.Label();
			appListLabel = new System.Windows.Forms.Label();
			deviceListLabel = new System.Windows.Forms.Label();
			appTabControl = new System.Windows.Forms.TabControl();
			ttyTab = new System.Windows.Forms.TabPage();
			checkBoxEnableLog = new System.Windows.Forms.CheckBox();
			consoleTextBox = new System.Windows.Forms.TextBox();
			extractTab = new System.Windows.Forms.TabPage();
			extractLogTextBox = new System.Windows.Forms.TextBox();
			dropBoxPanel = new System.Windows.Forms.Panel();
			dropBoxLabel = new System.Windows.Forms.Label();
			appsDataGridView = new System.Windows.Forms.DataGridView();
			PaAppName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			PaAppKey = new System.Windows.Forms.DataGridViewImageColumn();
			PaAppSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			deviceDataGridView = new System.Windows.Forms.DataGridView();
			PaDeviceType = new System.Windows.Forms.DataGridViewImageColumn();
			PaDeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			PaGUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
			contextMenuStrip1.SuspendLayout();
			panelManager1.SuspendLayout();
			managedPanel1.SuspendLayout();
			panelMetadata.SuspendLayout();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			tabPage2.SuspendLayout();
			nameDescriptionPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)appNameDataGridView).BeginInit();
			tabIcon.SuspendLayout();
			tabPage4.SuspendLayout();
			groupBox3.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
			tabPage3.SuspendLayout();
			managedPanel2.SuspendLayout();
			panelKeyManagement.SuspendLayout();
			groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridDeviceList).BeginInit();
			contextMenuStripDevices.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridApplicationID).BeginInit();
			contextMenuStripApplicationID.SuspendLayout();
			groupBox4.SuspendLayout();
			managedPanel3.SuspendLayout();
			panelPackageApp.SuspendLayout();
			appTabControl.SuspendLayout();
			ttyTab.SuspendLayout();
			extractTab.SuspendLayout();
			dropBoxPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)appsDataGridView).BeginInit();
			((System.ComponentModel.ISupportInitialize)deviceDataGridView).BeginInit();
			SuspendLayout();
			resources.ApplyResources(menuStrip1, "menuStrip1");
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { fileToolStripMenuItem, helpToolStripMenuItem });
			menuStrip1.Name = "menuStrip1";
			fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
			{
				newToolStripMenuItem, loadToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, submitToolStripMenuItem, toolStripSeparator4, preferenceToolStripMenuItem, proxySettingToolStripMenuItem, deleteAllDeviceSeedAndAppKeyToolStripMenuItem,
				exitToolStripMenuItem
			});
			resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem");
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			newToolStripMenuItem.Name = "newToolStripMenuItem";
			resources.ApplyResources(newToolStripMenuItem, "newToolStripMenuItem");
			newToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			resources.ApplyResources(loadToolStripMenuItem, "loadToolStripMenuItem");
			loadToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			resources.ApplyResources(saveToolStripMenuItem, "saveToolStripMenuItem");
			saveToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			resources.ApplyResources(saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
			saveAsToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
			submitToolStripMenuItem.Name = "submitToolStripMenuItem";
			resources.ApplyResources(submitToolStripMenuItem, "submitToolStripMenuItem");
			submitToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(toolStripSeparator4, "toolStripSeparator4");
			preferenceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { defaultToolStripMenuItem, englishToolStripMenuItem, japaneseToolStripMenuItem });
			preferenceToolStripMenuItem.Name = "preferenceToolStripMenuItem";
			resources.ApplyResources(preferenceToolStripMenuItem, "preferenceToolStripMenuItem");
			defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			resources.ApplyResources(defaultToolStripMenuItem, "defaultToolStripMenuItem");
			defaultToolStripMenuItem.Tag = "Default";
			defaultToolStripMenuItem.Click += new System.EventHandler(localeToolStripMenuItem_Click);
			englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			resources.ApplyResources(englishToolStripMenuItem, "englishToolStripMenuItem");
			englishToolStripMenuItem.Tag = "en-US";
			englishToolStripMenuItem.Click += new System.EventHandler(localeToolStripMenuItem_Click);
			japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
			resources.ApplyResources(japaneseToolStripMenuItem, "japaneseToolStripMenuItem");
			japaneseToolStripMenuItem.Tag = "ja-JP";
			japaneseToolStripMenuItem.Click += new System.EventHandler(localeToolStripMenuItem_Click);
			proxySettingToolStripMenuItem.Name = "proxySettingToolStripMenuItem";
			resources.ApplyResources(proxySettingToolStripMenuItem, "proxySettingToolStripMenuItem");
			proxySettingToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			deleteAllDeviceSeedAndAppKeyToolStripMenuItem.Name = "deleteAllDeviceSeedAndAppKeyToolStripMenuItem";
			resources.ApplyResources(deleteAllDeviceSeedAndAppKeyToolStripMenuItem, "deleteAllDeviceSeedAndAppKeyToolStripMenuItem");
			deleteAllDeviceSeedAndAppKeyToolStripMenuItem.Click += new System.EventHandler(deleteAllDeviceSeedAndAppKeyToolStripMenuItem_Click);
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
			exitToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { documentToolStripMenuItem, toolStripSeparator2, aboutToolStripMenuItem });
			resources.ApplyResources(helpToolStripMenuItem, "helpToolStripMenuItem");
			helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			documentToolStripMenuItem.Name = "documentToolStripMenuItem";
			resources.ApplyResources(documentToolStripMenuItem, "documentToolStripMenuItem");
			documentToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
			aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			resources.ApplyResources(aboutToolStripMenuItem, "aboutToolStripMenuItem");
			aboutToolStripMenuItem.Click += new System.EventHandler(buttonOrMenuItem_Click);
			errorProvider1.ContainerControl = this;
			resources.ApplyResources(productLabelText, "productLabelText");
			productLabelText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			errorProvider1.SetIconPadding(productLabelText, (int)resources.GetObject("productLabelText.IconPadding"));
			productLabelText.Name = "productLabelText";
			productLabelText.TextChanged += new System.EventHandler(productLabelText_TextChanged);
			productLabelText.KeyDown += new System.Windows.Forms.KeyEventHandler(productLabelText_KeyDown);
			productLabelText.Validating += new System.ComponentModel.CancelEventHandler(productLabelText_Validating);
			contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { toolStripMenuItem1 });
			contextMenuStrip1.Name = "contextMenuStrip1";
			resources.ApplyResources(contextMenuStrip1, "contextMenuStrip1");
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
			toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
			resources.ApplyResources(buttonExportDeviceSeed, "buttonExportDeviceSeed");
			buttonExportDeviceSeed.Image = PublishingUtility.Properties.Resources.SeedExport;
			buttonExportDeviceSeed.Name = "buttonExportDeviceSeed";
			toolTip1.SetToolTip(buttonExportDeviceSeed, resources.GetString("buttonExportDeviceSeed.ToolTip"));
			buttonExportDeviceSeed.UseVisualStyleBackColor = true;
			buttonExportDeviceSeed.Click += new System.EventHandler(buttonExportDeviceSeed_Click);
			resources.ApplyResources(buttonImportDeviceSeed, "buttonImportDeviceSeed");
			buttonImportDeviceSeed.Image = PublishingUtility.Properties.Resources.SeedImport;
			buttonImportDeviceSeed.Name = "buttonImportDeviceSeed";
			toolTip1.SetToolTip(buttonImportDeviceSeed, resources.GetString("buttonImportDeviceSeed.ToolTip"));
			buttonImportDeviceSeed.UseVisualStyleBackColor = true;
			buttonImportDeviceSeed.Click += new System.EventHandler(buttonImportDeviceSeed_Click);
			resources.ApplyResources(buttonExportAppKeyRing, "buttonExportAppKeyRing");
			buttonExportAppKeyRing.Image = PublishingUtility.Properties.Resources.KeyRingExport;
			buttonExportAppKeyRing.Name = "buttonExportAppKeyRing";
			toolTip1.SetToolTip(buttonExportAppKeyRing, resources.GetString("buttonExportAppKeyRing.ToolTip"));
			buttonExportAppKeyRing.UseVisualStyleBackColor = true;
			buttonExportAppKeyRing.Click += new System.EventHandler(buttonExportAppKeyRing_Click);
			resources.ApplyResources(buttonImportAppKeyRing, "buttonImportAppKeyRing");
			buttonImportAppKeyRing.Image = PublishingUtility.Properties.Resources.KeyRingImport;
			buttonImportAppKeyRing.Name = "buttonImportAppKeyRing";
			toolTip1.SetToolTip(buttonImportAppKeyRing, resources.GetString("buttonImportAppKeyRing.ToolTip"));
			buttonImportAppKeyRing.UseVisualStyleBackColor = true;
			buttonImportAppKeyRing.Click += new System.EventHandler(buttonImportAppKeyRing_Click);
			resources.ApplyResources(buttonGererateAppKey, "buttonGererateAppKey");
			buttonGererateAppKey.Image = PublishingUtility.Properties.Resources.AppKeyAdd;
			buttonGererateAppKey.Name = "buttonGererateAppKey";
			toolTip1.SetToolTip(buttonGererateAppKey, resources.GetString("buttonGererateAppKey.ToolTip"));
			buttonGererateAppKey.UseVisualStyleBackColor = true;
			buttonGererateAppKey.Click += new System.EventHandler(buttonGererateAppKey_Click);
			buttonImportPublisherKey.Image = PublishingUtility.Properties.Resources.PublisherKeyImport;
			resources.ApplyResources(buttonImportPublisherKey, "buttonImportPublisherKey");
			buttonImportPublisherKey.Name = "buttonImportPublisherKey";
			toolTip1.SetToolTip(buttonImportPublisherKey, resources.GetString("buttonImportPublisherKey.ToolTip"));
			buttonImportPublisherKey.UseVisualStyleBackColor = true;
			buttonImportPublisherKey.Click += new System.EventHandler(buttonImportPublisherKey_Click);
			buttonExportPublisherKey.Image = PublishingUtility.Properties.Resources.PublisherKeyExport;
			resources.ApplyResources(buttonExportPublisherKey, "buttonExportPublisherKey");
			buttonExportPublisherKey.Name = "buttonExportPublisherKey";
			toolTip1.SetToolTip(buttonExportPublisherKey, resources.GetString("buttonExportPublisherKey.ToolTip"));
			buttonExportPublisherKey.UseVisualStyleBackColor = true;
			buttonExportPublisherKey.Click += new System.EventHandler(buttonExportPublisherKey_Click);
			buttonGenerateKey.Image = PublishingUtility.Properties.Resources.PublisherKeyAdd;
			resources.ApplyResources(buttonGenerateKey, "buttonGenerateKey");
			buttonGenerateKey.Name = "buttonGenerateKey";
			toolTip1.SetToolTip(buttonGenerateKey, resources.GetString("buttonGenerateKey.ToolTip"));
			buttonGenerateKey.UseVisualStyleBackColor = true;
			buttonGenerateKey.Click += new System.EventHandler(buttonGeneratePublisherKey_Click);
			resources.ApplyResources(killAppButton, "killAppButton");
			killAppButton.Image = PublishingUtility.Properties.Resources.StopApp;
			killAppButton.Name = "killAppButton";
			toolTip1.SetToolTip(killAppButton, resources.GetString("killAppButton.ToolTip"));
			killAppButton.UseVisualStyleBackColor = true;
			killAppButton.Click += new System.EventHandler(killAppButton_Click);
			resources.ApplyResources(launchAppButton, "launchAppButton");
			launchAppButton.Image = PublishingUtility.Properties.Resources.PlayApp;
			launchAppButton.Name = "launchAppButton";
			toolTip1.SetToolTip(launchAppButton, resources.GetString("launchAppButton.ToolTip"));
			launchAppButton.UseVisualStyleBackColor = true;
			launchAppButton.Click += new System.EventHandler(launchAppButton_Click);
			resources.ApplyResources(uninstallAppButton, "uninstallAppButton");
			uninstallAppButton.Image = PublishingUtility.Properties.Resources.RemoveApp;
			uninstallAppButton.Name = "uninstallAppButton";
			toolTip1.SetToolTip(uninstallAppButton, resources.GetString("uninstallAppButton.ToolTip"));
			uninstallAppButton.UseVisualStyleBackColor = true;
			uninstallAppButton.Click += new System.EventHandler(uninstallAppButton_Click);
			resources.ApplyResources(installAppButton, "installAppButton");
			installAppButton.Image = PublishingUtility.Properties.Resources.InstallApp;
			installAppButton.Name = "installAppButton";
			toolTip1.SetToolTip(installAppButton, resources.GetString("installAppButton.ToolTip"));
			installAppButton.UseVisualStyleBackColor = true;
			installAppButton.Click += new System.EventHandler(installAppButton_Click);
			resources.ApplyResources(textBoxSplash, "textBoxSplash");
			textBoxSplash.Name = "textBoxSplash";
			toolTip1.SetToolTip(textBoxSplash, resources.GetString("textBoxSplash.ToolTip"));
			textBoxSplash.Validated += new System.EventHandler(textBoxSplash_Validated);
			resources.ApplyResources(textBoxIcon128, "textBoxIcon128");
			textBoxIcon128.Name = "textBoxIcon128";
			toolTip1.SetToolTip(textBoxIcon128, resources.GetString("textBoxIcon128.ToolTip"));
			textBoxIcon128.Validated += new System.EventHandler(textBoxIcon128_Validated);
			resources.ApplyResources(textBoxIcon256, "textBoxIcon256");
			textBoxIcon256.Name = "textBoxIcon256";
			toolTip1.SetToolTip(textBoxIcon256, resources.GetString("textBoxIcon256.ToolTip"));
			textBoxIcon256.Validated += new System.EventHandler(textBoxIcon256_Validated);
			resources.ApplyResources(textBoxIcon512, "textBoxIcon512");
			textBoxIcon512.Name = "textBoxIcon512";
			toolTip1.SetToolTip(textBoxIcon512, resources.GetString("textBoxIcon512.ToolTip"));
			textBoxIcon512.Validated += new System.EventHandler(textBoxIcon512_Validated);
			cate1MetadataButton.BackColor = System.Drawing.SystemColors.Control;
			cate1MetadataButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
			resources.ApplyResources(cate1MetadataButton, "cate1MetadataButton");
			cate1MetadataButton.Image = PublishingUtility.Properties.Resources.MetadataIcon;
			cate1MetadataButton.Name = "cate1MetadataButton";
			cate1MetadataButton.Tag = "0";
			cate1MetadataButton.UseVisualStyleBackColor = false;
			cate1MetadataButton.Click += new System.EventHandler(categorySwitchButton_Click);
			cate2KeyManageButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
			cate2KeyManageButton.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(cate2KeyManageButton, "cate2KeyManageButton");
			cate2KeyManageButton.Image = PublishingUtility.Properties.Resources.KeyManagementIcon;
			cate2KeyManageButton.Name = "cate2KeyManageButton";
			cate2KeyManageButton.Tag = "1";
			cate2KeyManageButton.UseVisualStyleBackColor = true;
			cate2KeyManageButton.Click += new System.EventHandler(categorySwitchButton_Click);
			cate3PacageAppsButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
			cate3PacageAppsButton.FlatAppearance.BorderSize = 0;
			resources.ApplyResources(cate3PacageAppsButton, "cate3PacageAppsButton");
			cate3PacageAppsButton.Image = PublishingUtility.Properties.Resources.Package_AppIcon;
			cate3PacageAppsButton.Name = "cate3PacageAppsButton";
			cate3PacageAppsButton.Tag = "2";
			cate3PacageAppsButton.UseVisualStyleBackColor = true;
			cate3PacageAppsButton.Click += new System.EventHandler(categorySwitchButton_Click);
			resources.ApplyResources(panelManager1, "panelManager1");
			panelManager1.Controls.Add(managedPanel1);
			panelManager1.Controls.Add(managedPanel2);
			panelManager1.Controls.Add(managedPanel3);
			panelManager1.Name = "panelManager1";
			panelManager1.SelectedIndex = 1;
			panelManager1.SelectedPanel = managedPanel2;
			managedPanel1.Controls.Add(panelMetadata);
			resources.ApplyResources(managedPanel1, "managedPanel1");
			managedPanel1.Name = "managedPanel1";
			managedPanel1.Tag = "0";
			resources.ApplyResources(panelMetadata, "panelMetadata");
			panelMetadata.Controls.Add(saveAppXmlButton);
			panelMetadata.Controls.Add(tabControl1);
			panelMetadata.Name = "panelMetadata";
			resources.ApplyResources(saveAppXmlButton, "saveAppXmlButton");
			saveAppXmlButton.Name = "saveAppXmlButton";
			saveAppXmlButton.UseVisualStyleBackColor = true;
			saveAppXmlButton.Click += new System.EventHandler(buttonOrMenuItem_Click);
			resources.ApplyResources(tabControl1, "tabControl1");
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Controls.Add(tabIcon);
			tabControl1.Controls.Add(tabPage4);
			tabControl1.Controls.Add(tabPage3);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
			tabPage1.Controls.Add(propertyGrid1);
			resources.ApplyResources(tabPage1, "tabPage1");
			tabPage1.Name = "tabPage1";
			resources.ApplyResources(propertyGrid1, "propertyGrid1");
			propertyGrid1.LineColor = System.Drawing.Color.FromArgb(224, 224, 224);
			propertyGrid1.Name = "propertyGrid1";
			propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
			propertyGrid1.ToolbarVisible = false;
			propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
			resources.ApplyResources(tabPage2, "tabPage2");
			tabPage2.Controls.Add(nameDescriptionPanel);
			tabPage2.Controls.Add(titleLabel);
			tabPage2.Controls.Add(appNameDataGridView);
			tabPage2.Name = "tabPage2";
			tabPage2.UseVisualStyleBackColor = true;
			tabPage2.Enter += new System.EventHandler(tabPage2_Enter);
			nameDescriptionPanel.Controls.Add(appNameLabel);
			nameDescriptionPanel.Controls.Add(nameDescriptionLabel);
			resources.ApplyResources(nameDescriptionPanel, "nameDescriptionPanel");
			nameDescriptionPanel.Name = "nameDescriptionPanel";
			resources.ApplyResources(appNameLabel, "appNameLabel");
			appNameLabel.BackColor = System.Drawing.SystemColors.Menu;
			appNameLabel.Name = "appNameLabel";
			nameDescriptionLabel.AutoEllipsis = true;
			nameDescriptionLabel.BackColor = System.Drawing.SystemColors.Menu;
			nameDescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(nameDescriptionLabel, "nameDescriptionLabel");
			nameDescriptionLabel.Name = "nameDescriptionLabel";
			resources.ApplyResources(titleLabel, "titleLabel");
			titleLabel.Name = "titleLabel";
			appNameDataGridView.AllowUserToAddRows = false;
			appNameDataGridView.AllowUserToDeleteRows = false;
			resources.ApplyResources(appNameDataGridView, "appNameDataGridView");
			appNameDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			appNameDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			appNameDataGridView.Columns.AddRange(dataGridView1Column1, dataGridView1Column2, dataGridView1Column3);
			dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle.Font = new System.Drawing.Font("MS UI Gothic", 9f);
			dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle.NullValue = "----";
			dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			appNameDataGridView.DefaultCellStyle = dataGridViewCellStyle;
			appNameDataGridView.GridColor = System.Drawing.Color.FromArgb(224, 224, 224);
			appNameDataGridView.MultiSelect = false;
			appNameDataGridView.Name = "appNameDataGridView";
			appNameDataGridView.RowHeadersVisible = false;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			appNameDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
			appNameDataGridView.RowTemplate.Height = 16;
			appNameDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			appNameDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			appNameDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(appNameDataGridView_CellValueChanged);
			appNameDataGridView.CurrentCellChanged += new System.EventHandler(appNameDataGridView_CurrentCellChanged);
			appNameDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(dataGridView_KeyDown);
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridView1Column1.DefaultCellStyle = dataGridViewCellStyle3;
			dataGridView1Column1.Frozen = true;
			resources.ApplyResources(dataGridView1Column1, "dataGridView1Column1");
			dataGridView1Column1.Name = "dataGridView1Column1";
			dataGridView1Column1.ReadOnly = true;
			dataGridView1Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			dataGridView1Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridView1Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridView1Column2.DefaultCellStyle = dataGridViewCellStyle4;
			resources.ApplyResources(dataGridView1Column2, "dataGridView1Column2");
			dataGridView1Column2.MaxInputLength = 64;
			dataGridView1Column2.Name = "dataGridView1Column2";
			dataGridView1Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			dataGridView1Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			dataGridView1Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridView1Column3.DefaultCellStyle = dataGridViewCellStyle5;
			dataGridView1Column3.FillWeight = 60f;
			resources.ApplyResources(dataGridView1Column3, "dataGridView1Column3");
			dataGridView1Column3.MaxInputLength = 16;
			dataGridView1Column3.Name = "dataGridView1Column3";
			dataGridView1Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			dataGridView1Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			tabIcon.Controls.Add(buttonGenerateImage128);
			tabIcon.Controls.Add(buttonGenerateImage256);
			tabIcon.Controls.Add(buttonOpenFileSplash);
			tabIcon.Controls.Add(textBoxSplash);
			tabIcon.Controls.Add(label18);
			tabIcon.Controls.Add(buttonOpenFileIcon128);
			tabIcon.Controls.Add(textBoxIcon128);
			tabIcon.Controls.Add(label17);
			tabIcon.Controls.Add(buttonOpenFileIcon256);
			tabIcon.Controls.Add(textBoxIcon256);
			tabIcon.Controls.Add(label16);
			tabIcon.Controls.Add(buttonOpenFileIcon512);
			tabIcon.Controls.Add(textBoxIcon512);
			tabIcon.Controls.Add(label15);
			resources.ApplyResources(tabIcon, "tabIcon");
			tabIcon.Name = "tabIcon";
			tabIcon.UseVisualStyleBackColor = true;
			resources.ApplyResources(buttonGenerateImage128, "buttonGenerateImage128");
			buttonGenerateImage128.Name = "buttonGenerateImage128";
			buttonGenerateImage128.UseVisualStyleBackColor = true;
			buttonGenerateImage128.Click += new System.EventHandler(buttonGenerateImage128_Click);
			resources.ApplyResources(buttonGenerateImage256, "buttonGenerateImage256");
			buttonGenerateImage256.Name = "buttonGenerateImage256";
			buttonGenerateImage256.UseVisualStyleBackColor = true;
			buttonGenerateImage256.Click += new System.EventHandler(buttonGenerateImage256_Click);
			resources.ApplyResources(buttonOpenFileSplash, "buttonOpenFileSplash");
			buttonOpenFileSplash.Name = "buttonOpenFileSplash";
			buttonOpenFileSplash.UseVisualStyleBackColor = true;
			buttonOpenFileSplash.Click += new System.EventHandler(buttonOpenFileSplash_Click);
			resources.ApplyResources(label18, "label18");
			label18.Name = "label18";
			resources.ApplyResources(buttonOpenFileIcon128, "buttonOpenFileIcon128");
			buttonOpenFileIcon128.Name = "buttonOpenFileIcon128";
			buttonOpenFileIcon128.UseVisualStyleBackColor = true;
			buttonOpenFileIcon128.Click += new System.EventHandler(buttonOpenFileIcon128_Click);
			resources.ApplyResources(label17, "label17");
			label17.Name = "label17";
			resources.ApplyResources(buttonOpenFileIcon256, "buttonOpenFileIcon256");
			buttonOpenFileIcon256.Name = "buttonOpenFileIcon256";
			buttonOpenFileIcon256.UseVisualStyleBackColor = true;
			buttonOpenFileIcon256.Click += new System.EventHandler(buttonOpenFileIcon256_Click);
			resources.ApplyResources(label16, "label16");
			label16.Name = "label16";
			resources.ApplyResources(buttonOpenFileIcon512, "buttonOpenFileIcon512");
			buttonOpenFileIcon512.Name = "buttonOpenFileIcon512";
			buttonOpenFileIcon512.UseVisualStyleBackColor = true;
			buttonOpenFileIcon512.Click += new System.EventHandler(buttonOpenFileIcon512_Click);
			resources.ApplyResources(label15, "label15");
			label15.Name = "label15";
			resources.ApplyResources(tabPage4, "tabPage4");
			tabPage4.Controls.Add(textBoxCommonRating);
			tabPage4.Controls.Add(textBoxParentalLockLevel);
			tabPage4.Controls.Add(label3);
			tabPage4.Controls.Add(groupBox3);
			tabPage4.Controls.Add(groupBox2);
			tabPage4.Controls.Add(groupBox1);
			tabPage4.Controls.Add(label1);
			tabPage4.Name = "tabPage4";
			tabPage4.UseVisualStyleBackColor = true;
			resources.ApplyResources(textBoxCommonRating, "textBoxCommonRating");
			textBoxCommonRating.Name = "textBoxCommonRating";
			resources.ApplyResources(textBoxParentalLockLevel, "textBoxParentalLockLevel");
			textBoxParentalLockLevel.Name = "textBoxParentalLockLevel";
			resources.ApplyResources(label3, "label3");
			label3.Name = "label3";
			resources.ApplyResources(groupBox3, "groupBox3");
			groupBox3.Controls.Add(textBoxRatingResult);
			groupBox3.Controls.Add(label10);
			groupBox3.Controls.Add(buttonStartRatingCheck);
			groupBox3.Name = "groupBox3";
			groupBox3.TabStop = false;
			resources.ApplyResources(textBoxRatingResult, "textBoxRatingResult");
			textBoxRatingResult.Name = "textBoxRatingResult";
			resources.ApplyResources(label10, "label10");
			label10.Name = "label10";
			resources.ApplyResources(buttonStartRatingCheck, "buttonStartRatingCheck");
			buttonStartRatingCheck.Name = "buttonStartRatingCheck";
			buttonStartRatingCheck.UseVisualStyleBackColor = true;
			buttonStartRatingCheck.Click += new System.EventHandler(buttonStartRatingCheck_Click);
			resources.ApplyResources(groupBox2, "groupBox2");
			groupBox2.Controls.Add(label9);
			groupBox2.Controls.Add(label6);
			groupBox2.Controls.Add(textBoxEsrbCode);
			groupBox2.Controls.Add(label7);
			groupBox2.Controls.Add(linkLabel_EsrbUrl);
			groupBox2.Controls.Add(dropDownListEsrbRating);
			groupBox2.Name = "groupBox2";
			groupBox2.TabStop = false;
			resources.ApplyResources(label9, "label9");
			label9.Name = "label9";
			resources.ApplyResources(label6, "label6");
			label6.Name = "label6";
			resources.ApplyResources(textBoxEsrbCode, "textBoxEsrbCode");
			textBoxEsrbCode.Name = "textBoxEsrbCode";
			textBoxEsrbCode.Validated += new System.EventHandler(textBoxEsrbCode_Validated);
			resources.ApplyResources(label7, "label7");
			label7.Name = "label7";
			resources.ApplyResources(linkLabel_EsrbUrl, "linkLabel_EsrbUrl");
			linkLabel_EsrbUrl.LinkVisited = true;
			linkLabel_EsrbUrl.Name = "linkLabel_EsrbUrl";
			linkLabel_EsrbUrl.TabStop = true;
			linkLabel_EsrbUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_EsrbUrl_LinkClicked);
			dropDownListEsrbRating.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			dropDownListEsrbRating.FormattingEnabled = true;
			resources.ApplyResources(dropDownListEsrbRating, "dropDownListEsrbRating");
			dropDownListEsrbRating.Name = "dropDownListEsrbRating";
			dropDownListEsrbRating.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
			resources.ApplyResources(groupBox1, "groupBox1");
			groupBox1.Controls.Add(label11);
			groupBox1.Controls.Add(checkBoxGambling);
			groupBox1.Controls.Add(pictureBox9);
			groupBox1.Controls.Add(checkBoxSex);
			groupBox1.Controls.Add(checkBoxLanguage);
			groupBox1.Controls.Add(pictureBox4);
			groupBox1.Controls.Add(pictureBox2);
			groupBox1.Controls.Add(checkBoxScaryElements);
			groupBox1.Controls.Add(checkBoxDrugs);
			groupBox1.Controls.Add(pictureBox8);
			groupBox1.Controls.Add(pictureBox7);
			groupBox1.Controls.Add(label8);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(pictureBox1);
			groupBox1.Controls.Add(checkBoxDiscrimination);
			groupBox1.Controls.Add(checkBoxViolence);
			groupBox1.Controls.Add(checkBoxOnline);
			groupBox1.Controls.Add(pictureBox5);
			groupBox1.Controls.Add(pictureBox3);
			groupBox1.Controls.Add(TextBoxPEGINumber);
			groupBox1.Controls.Add(DropDownListAgeRatingLogo);
			groupBox1.Controls.Add(PEGI_Link);
			groupBox1.Name = "groupBox1";
			groupBox1.TabStop = false;
			resources.ApplyResources(label11, "label11");
			label11.Name = "label11";
			resources.ApplyResources(checkBoxGambling, "checkBoxGambling");
			checkBoxGambling.Name = "checkBoxGambling";
			checkBoxGambling.UseVisualStyleBackColor = true;
			checkBoxGambling.Click += new System.EventHandler(checkBoxGambling_CheckedChanged);
			resources.ApplyResources(pictureBox9, "pictureBox9");
			pictureBox9.Name = "pictureBox9";
			pictureBox9.TabStop = false;
			resources.ApplyResources(checkBoxSex, "checkBoxSex");
			checkBoxSex.Name = "checkBoxSex";
			checkBoxSex.UseVisualStyleBackColor = true;
			checkBoxSex.Click += new System.EventHandler(checkBoxSex_CheckedChanged);
			resources.ApplyResources(checkBoxLanguage, "checkBoxLanguage");
			checkBoxLanguage.Name = "checkBoxLanguage";
			checkBoxLanguage.UseVisualStyleBackColor = true;
			checkBoxLanguage.Click += new System.EventHandler(checkBoxLanguage_CheckedChanged);
			resources.ApplyResources(pictureBox4, "pictureBox4");
			pictureBox4.Name = "pictureBox4";
			pictureBox4.TabStop = false;
			resources.ApplyResources(pictureBox2, "pictureBox2");
			pictureBox2.Name = "pictureBox2";
			pictureBox2.TabStop = false;
			resources.ApplyResources(checkBoxScaryElements, "checkBoxScaryElements");
			checkBoxScaryElements.Name = "checkBoxScaryElements";
			checkBoxScaryElements.UseVisualStyleBackColor = true;
			checkBoxScaryElements.Click += new System.EventHandler(checkBoxScaryElements_CheckedChanged);
			resources.ApplyResources(checkBoxDrugs, "checkBoxDrugs");
			checkBoxDrugs.Name = "checkBoxDrugs";
			checkBoxDrugs.UseVisualStyleBackColor = true;
			checkBoxDrugs.Click += new System.EventHandler(checkBoxDrugs_CheckedChanged);
			resources.ApplyResources(pictureBox8, "pictureBox8");
			pictureBox8.Name = "pictureBox8";
			pictureBox8.TabStop = false;
			resources.ApplyResources(pictureBox7, "pictureBox7");
			pictureBox7.Name = "pictureBox7";
			pictureBox7.TabStop = false;
			resources.ApplyResources(label8, "label8");
			label8.Name = "label8";
			resources.ApplyResources(label5, "label5");
			label5.Name = "label5";
			resources.ApplyResources(label2, "label2");
			label2.Name = "label2";
			resources.ApplyResources(pictureBox1, "pictureBox1");
			pictureBox1.Name = "pictureBox1";
			pictureBox1.TabStop = false;
			resources.ApplyResources(checkBoxDiscrimination, "checkBoxDiscrimination");
			checkBoxDiscrimination.Name = "checkBoxDiscrimination";
			checkBoxDiscrimination.UseVisualStyleBackColor = true;
			checkBoxDiscrimination.Click += new System.EventHandler(checkBoxDiscrimination_CheckedChanged);
			resources.ApplyResources(checkBoxViolence, "checkBoxViolence");
			checkBoxViolence.Name = "checkBoxViolence";
			checkBoxViolence.UseVisualStyleBackColor = true;
			checkBoxViolence.Click += new System.EventHandler(checkBoxViolence_CheckedChanged);
			resources.ApplyResources(checkBoxOnline, "checkBoxOnline");
			checkBoxOnline.Name = "checkBoxOnline";
			checkBoxOnline.UseVisualStyleBackColor = true;
			checkBoxOnline.Click += new System.EventHandler(checkBoxOnline_CheckedChanged);
			resources.ApplyResources(pictureBox5, "pictureBox5");
			pictureBox5.Name = "pictureBox5";
			pictureBox5.TabStop = false;
			resources.ApplyResources(pictureBox3, "pictureBox3");
			pictureBox3.Name = "pictureBox3";
			pictureBox3.TabStop = false;
			resources.ApplyResources(TextBoxPEGINumber, "TextBoxPEGINumber");
			TextBoxPEGINumber.Name = "TextBoxPEGINumber";
			TextBoxPEGINumber.Validated += new System.EventHandler(TextBoxPEGINumber_Validated);
			DropDownListAgeRatingLogo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			DropDownListAgeRatingLogo.FormattingEnabled = true;
			resources.ApplyResources(DropDownListAgeRatingLogo, "DropDownListAgeRatingLogo");
			DropDownListAgeRatingLogo.Name = "DropDownListAgeRatingLogo";
			DropDownListAgeRatingLogo.SelectedIndexChanged += new System.EventHandler(DropDownListAgeRatingLogo_SelectedIndexChanged);
			resources.ApplyResources(PEGI_Link, "PEGI_Link");
			PEGI_Link.LinkVisited = true;
			PEGI_Link.Name = "PEGI_Link";
			PEGI_Link.TabStop = true;
			PEGI_Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(PEGI_Link_LinkClicked);
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			tabPage3.Controls.Add(productLabelText);
			tabPage3.Controls.Add(productLabel);
			tabPage3.Controls.Add(purchasePanel);
			tabPage3.Controls.Add(addProductButton);
			tabPage3.Controls.Add(supportLocalLabel);
			tabPage3.Controls.Add(localeCheckedListBox);
			resources.ApplyResources(tabPage3, "tabPage3");
			tabPage3.Name = "tabPage3";
			tabPage3.UseVisualStyleBackColor = true;
			resources.ApplyResources(productLabel, "productLabel");
			productLabel.Name = "productLabel";
			resources.ApplyResources(purchasePanel, "purchasePanel");
			purchasePanel.Name = "purchasePanel";
			resources.ApplyResources(addProductButton, "addProductButton");
			addProductButton.Name = "addProductButton";
			addProductButton.UseVisualStyleBackColor = true;
			addProductButton.Click += new System.EventHandler(addProductButton_Click);
			resources.ApplyResources(supportLocalLabel, "supportLocalLabel");
			supportLocalLabel.Name = "supportLocalLabel";
			resources.ApplyResources(localeCheckedListBox, "localeCheckedListBox");
			localeCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
			localeCheckedListBox.CheckOnClick = true;
			localeCheckedListBox.FormattingEnabled = true;
			localeCheckedListBox.Items.AddRange(new object[19]
			{
				resources.GetString("localeCheckedListBox.Items"),
				resources.GetString("localeCheckedListBox.Items1"),
				resources.GetString("localeCheckedListBox.Items2"),
				resources.GetString("localeCheckedListBox.Items3"),
				resources.GetString("localeCheckedListBox.Items4"),
				resources.GetString("localeCheckedListBox.Items5"),
				resources.GetString("localeCheckedListBox.Items6"),
				resources.GetString("localeCheckedListBox.Items7"),
				resources.GetString("localeCheckedListBox.Items8"),
				resources.GetString("localeCheckedListBox.Items9"),
				resources.GetString("localeCheckedListBox.Items10"),
				resources.GetString("localeCheckedListBox.Items11"),
				resources.GetString("localeCheckedListBox.Items12"),
				resources.GetString("localeCheckedListBox.Items13"),
				resources.GetString("localeCheckedListBox.Items14"),
				resources.GetString("localeCheckedListBox.Items15"),
				resources.GetString("localeCheckedListBox.Items16"),
				resources.GetString("localeCheckedListBox.Items17"),
				resources.GetString("localeCheckedListBox.Items18")
			});
			localeCheckedListBox.MultiColumn = true;
			localeCheckedListBox.Name = "localeCheckedListBox";
			localeCheckedListBox.SelectedIndexChanged += new System.EventHandler(localeCheckedListBox1_SelectedIndexChanged);
			managedPanel2.Controls.Add(panelKeyManagement);
			resources.ApplyResources(managedPanel2, "managedPanel2");
			managedPanel2.Name = "managedPanel2";
			managedPanel2.Tag = "1";
			resources.ApplyResources(panelKeyManagement, "panelKeyManagement");
			panelKeyManagement.Controls.Add(groupBox5);
			panelKeyManagement.Controls.Add(groupBox4);
			panelKeyManagement.Name = "panelKeyManagement";
			resources.ApplyResources(groupBox5, "groupBox5");
			groupBox5.Controls.Add(buttonUpdateDeviceSeedAndAppKey);
			groupBox5.Controls.Add(buttonRemoveAppXml);
			groupBox5.Controls.Add(buttonRegisterAppXml);
			groupBox5.Controls.Add(buttonGenerateDeviceSeed);
			groupBox5.Controls.Add(label14);
			groupBox5.Controls.Add(dataGridDeviceList);
			groupBox5.Controls.Add(labelDeviceList_AppKey);
			groupBox5.Controls.Add(dataGridApplicationID);
			groupBox5.Controls.Add(buttonExportDeviceSeed);
			groupBox5.Controls.Add(buttonImportDeviceSeed);
			groupBox5.Controls.Add(buttonExportAppKeyRing);
			groupBox5.Controls.Add(buttonImportAppKeyRing);
			groupBox5.Controls.Add(buttonGererateAppKey);
			groupBox5.Name = "groupBox5";
			groupBox5.TabStop = false;
			resources.ApplyResources(buttonUpdateDeviceSeedAndAppKey, "buttonUpdateDeviceSeedAndAppKey");
			buttonUpdateDeviceSeedAndAppKey.Image = PublishingUtility.Properties.Resources.UpdateDeviceSeedAppKey;
			buttonUpdateDeviceSeedAndAppKey.Name = "buttonUpdateDeviceSeedAndAppKey";
			toolTip1.SetToolTip(buttonUpdateDeviceSeedAndAppKey, resources.GetString("buttonUpdateDeviceSeedAndAppKey.ToolTip"));
			buttonUpdateDeviceSeedAndAppKey.UseVisualStyleBackColor = true;
			buttonUpdateDeviceSeedAndAppKey.Click += new System.EventHandler(buttonUpdateDeviceSeedAndAppKey_Click);
			resources.ApplyResources(buttonRemoveAppXml, "buttonRemoveAppXml");
			buttonRemoveAppXml.Image = PublishingUtility.Properties.Resources.RemoveApp;
			buttonRemoveAppXml.Name = "buttonRemoveAppXml";
			toolTip1.SetToolTip(buttonRemoveAppXml, resources.GetString("buttonRemoveAppXml.ToolTip"));
			buttonRemoveAppXml.UseVisualStyleBackColor = true;
			buttonRemoveAppXml.Click += new System.EventHandler(buttonRemoveApplicationID_Click);
			resources.ApplyResources(buttonRegisterAppXml, "buttonRegisterAppXml");
			buttonRegisterAppXml.Image = PublishingUtility.Properties.Resources.InstallApp;
			buttonRegisterAppXml.Name = "buttonRegisterAppXml";
			toolTip1.SetToolTip(buttonRegisterAppXml, resources.GetString("buttonRegisterAppXml.ToolTip"));
			buttonRegisterAppXml.UseVisualStyleBackColor = true;
			buttonRegisterAppXml.Click += new System.EventHandler(buttonRegisterApplicationID_Click);
			resources.ApplyResources(buttonGenerateDeviceSeed, "buttonGenerateDeviceSeed");
			buttonGenerateDeviceSeed.Image = PublishingUtility.Properties.Resources.SeedGenerate;
			buttonGenerateDeviceSeed.Name = "buttonGenerateDeviceSeed";
			toolTip1.SetToolTip(buttonGenerateDeviceSeed, resources.GetString("buttonGenerateDeviceSeed.ToolTip"));
			buttonGenerateDeviceSeed.UseVisualStyleBackColor = true;
			buttonGenerateDeviceSeed.Click += new System.EventHandler(buttonGenerateDeviceSeed_Click);
			resources.ApplyResources(label14, "label14");
			label14.Name = "label14";
			dataGridDeviceList.AllowUserToAddRows = false;
			dataGridDeviceList.AllowUserToDeleteRows = false;
			dataGridDeviceList.AllowUserToResizeRows = false;
			resources.ApplyResources(dataGridDeviceList, "dataGridDeviceList");
			dataGridDeviceList.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			dataGridDeviceList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			dataGridDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridDeviceList.Columns.AddRange(KmDeviceType, KmDeviceName, KmConnection, KmSeedKeyState, KmSeed, KmAppKey, KmExpirationDate, KmDeviceID);
			dataGridDeviceList.ContextMenuStrip = contextMenuStripDevices;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			dataGridDeviceList.DefaultCellStyle = dataGridViewCellStyle7;
			dataGridDeviceList.GridColor = System.Drawing.SystemColors.ActiveBorder;
			dataGridDeviceList.MultiSelect = false;
			dataGridDeviceList.Name = "dataGridDeviceList";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			dataGridDeviceList.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
			dataGridDeviceList.RowHeadersVisible = false;
			dataGridDeviceList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridDeviceList.RowTemplate.Height = 21;
			dataGridDeviceList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dataGridDeviceList.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(dataGridDeviceList_CellBeginEdit);
			dataGridDeviceList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridDeviceList_CellContentClick);
			dataGridDeviceList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridDeviceList_CellEndEdit);
			dataGridDeviceList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(dataGridDeviceList_CellValidating);
			dataGridDeviceList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(dataGridDeviceList_CellValueChanged);
			resources.ApplyResources(KmDeviceType, "KmDeviceType");
			KmDeviceType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			KmDeviceType.Name = "KmDeviceType";
			KmDeviceType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			resources.ApplyResources(KmDeviceName, "KmDeviceName");
			KmDeviceName.Name = "KmDeviceName";
			KmDeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			resources.ApplyResources(KmConnection, "KmConnection");
			KmConnection.Name = "KmConnection";
			KmConnection.ReadOnly = true;
			KmConnection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			resources.ApplyResources(KmSeedKeyState, "KmSeedKeyState");
			KmSeedKeyState.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			KmSeedKeyState.Name = "KmSeedKeyState";
			KmSeedKeyState.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			resources.ApplyResources(KmSeed, "KmSeed");
			KmSeed.Name = "KmSeed";
			KmSeed.ReadOnly = true;
			KmSeed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			resources.ApplyResources(KmAppKey, "KmAppKey");
			KmAppKey.Name = "KmAppKey";
			KmAppKey.ReadOnly = true;
			KmAppKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			KmExpirationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(KmExpirationDate, "KmExpirationDate");
			KmExpirationDate.Name = "KmExpirationDate";
			KmExpirationDate.ReadOnly = true;
			KmExpirationDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			resources.ApplyResources(KmDeviceID, "KmDeviceID");
			KmDeviceID.Name = "KmDeviceID";
			KmDeviceID.ReadOnly = true;
			KmDeviceID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			contextMenuStripDevices.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { toolStripMenuItemDeleteDeviceSeed, toolStripMenuItemDeleteAppKey });
			contextMenuStripDevices.Name = "contextMenuStrip3";
			resources.ApplyResources(contextMenuStripDevices, "contextMenuStripDevices");
			toolStripMenuItemDeleteDeviceSeed.Name = "toolStripMenuItemDeleteDeviceSeed";
			resources.ApplyResources(toolStripMenuItemDeleteDeviceSeed, "toolStripMenuItemDeleteDeviceSeed");
			toolStripMenuItemDeleteDeviceSeed.Click += new System.EventHandler(toolStripMenuItemDeleteDeviceSeed_Click);
			toolStripMenuItemDeleteAppKey.Name = "toolStripMenuItemDeleteAppKey";
			resources.ApplyResources(toolStripMenuItemDeleteAppKey, "toolStripMenuItemDeleteAppKey");
			toolStripMenuItemDeleteAppKey.Click += new System.EventHandler(toolStripMenuItemDeleteAppKey_Click);
			resources.ApplyResources(labelDeviceList_AppKey, "labelDeviceList_AppKey");
			labelDeviceList_AppKey.Name = "labelDeviceList_AppKey";
			dataGridApplicationID.AllowUserToAddRows = false;
			dataGridApplicationID.AllowUserToDeleteRows = false;
			dataGridApplicationID.AllowUserToResizeRows = false;
			resources.ApplyResources(dataGridApplicationID, "dataGridApplicationID");
			dataGridApplicationID.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			dataGridApplicationID.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			dataGridApplicationID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridApplicationID.Columns.AddRange(KmAppId);
			dataGridApplicationID.ContextMenuStrip = contextMenuStripApplicationID;
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			dataGridApplicationID.DefaultCellStyle = dataGridViewCellStyle10;
			dataGridApplicationID.GridColor = System.Drawing.SystemColors.ActiveBorder;
			dataGridApplicationID.MultiSelect = false;
			dataGridApplicationID.Name = "dataGridApplicationID";
			dataGridApplicationID.ReadOnly = true;
			dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle11.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			dataGridApplicationID.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
			dataGridApplicationID.RowHeadersVisible = false;
			dataGridApplicationID.RowTemplate.Height = 21;
			dataGridApplicationID.SelectionChanged += new System.EventHandler(dataGridApplicationID_SelectionChanged);
			KmAppId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			resources.ApplyResources(KmAppId, "KmAppId");
			KmAppId.Name = "KmAppId";
			KmAppId.ReadOnly = true;
			KmAppId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			contextMenuStripApplicationID.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { toolStripMenuItemDeleteApplicationID });
			contextMenuStripApplicationID.Name = "contextMenuStrip2";
			resources.ApplyResources(contextMenuStripApplicationID, "contextMenuStripApplicationID");
			toolStripMenuItemDeleteApplicationID.Name = "toolStripMenuItemDeleteApplicationID";
			resources.ApplyResources(toolStripMenuItemDeleteApplicationID, "toolStripMenuItemDeleteApplicationID");
			toolStripMenuItemDeleteApplicationID.Click += new System.EventHandler(toolStripMenuItemDeleteAppKeyRing_Click);
			resources.ApplyResources(groupBox4, "groupBox4");
			groupBox4.Controls.Add(buttonCheckPublisherKey);
			groupBox4.Controls.Add(label12);
			groupBox4.Controls.Add(textBoxDateOfGenerateKey);
			groupBox4.Controls.Add(buttonImportPublisherKey);
			groupBox4.Controls.Add(buttonExportPublisherKey);
			groupBox4.Controls.Add(textBoxDeveloperKey);
			groupBox4.Controls.Add(label4);
			groupBox4.Controls.Add(buttonGenerateKey);
			groupBox4.Name = "groupBox4";
			groupBox4.TabStop = false;
			buttonCheckPublisherKey.Image = PublishingUtility.Properties.Resources.PublisherKeyCheck;
			resources.ApplyResources(buttonCheckPublisherKey, "buttonCheckPublisherKey");
			buttonCheckPublisherKey.Name = "buttonCheckPublisherKey";
			toolTip1.SetToolTip(buttonCheckPublisherKey, resources.GetString("buttonCheckPublisherKey.ToolTip"));
			buttonCheckPublisherKey.UseVisualStyleBackColor = true;
			buttonCheckPublisherKey.Click += new System.EventHandler(buttonCheckPublisherKey_Click);
			resources.ApplyResources(label12, "label12");
			label12.Name = "label12";
			resources.ApplyResources(textBoxDateOfGenerateKey, "textBoxDateOfGenerateKey");
			textBoxDateOfGenerateKey.Name = "textBoxDateOfGenerateKey";
			resources.ApplyResources(textBoxDeveloperKey, "textBoxDeveloperKey");
			textBoxDeveloperKey.Name = "textBoxDeveloperKey";
			resources.ApplyResources(label4, "label4");
			label4.Name = "label4";
			managedPanel3.Controls.Add(panelPackageApp);
			resources.ApplyResources(managedPanel3, "managedPanel3");
			managedPanel3.Name = "managedPanel3";
			managedPanel3.Tag = "2";
			resources.ApplyResources(panelPackageApp, "panelPackageApp");
			panelPackageApp.Controls.Add(buttonInstallAppQA);
			panelPackageApp.Controls.Add(labelQAPublisherKey);
			panelPackageApp.Controls.Add(appListLabel);
			panelPackageApp.Controls.Add(deviceListLabel);
			panelPackageApp.Controls.Add(appTabControl);
			panelPackageApp.Controls.Add(killAppButton);
			panelPackageApp.Controls.Add(launchAppButton);
			panelPackageApp.Controls.Add(uninstallAppButton);
			panelPackageApp.Controls.Add(installAppButton);
			panelPackageApp.Controls.Add(appsDataGridView);
			panelPackageApp.Controls.Add(deviceDataGridView);
			panelPackageApp.Name = "panelPackageApp";
			resources.ApplyResources(buttonInstallAppQA, "buttonInstallAppQA");
			buttonInstallAppQA.ForeColor = System.Drawing.Color.DimGray;
			buttonInstallAppQA.Image = PublishingUtility.Properties.Resources.InstallApp;
			buttonInstallAppQA.Name = "buttonInstallAppQA";
			toolTip1.SetToolTip(buttonInstallAppQA, resources.GetString("buttonInstallAppQA.ToolTip"));
			buttonInstallAppQA.UseVisualStyleBackColor = true;
			buttonInstallAppQA.Click += new System.EventHandler(buttonInstallAppQA_Click);
			resources.ApplyResources(labelQAPublisherKey, "labelQAPublisherKey");
			labelQAPublisherKey.Name = "labelQAPublisherKey";
			resources.ApplyResources(appListLabel, "appListLabel");
			appListLabel.Name = "appListLabel";
			resources.ApplyResources(deviceListLabel, "deviceListLabel");
			deviceListLabel.Name = "deviceListLabel";
			resources.ApplyResources(appTabControl, "appTabControl");
			appTabControl.Controls.Add(ttyTab);
			appTabControl.Controls.Add(extractTab);
			appTabControl.Name = "appTabControl";
			appTabControl.SelectedIndex = 0;
			ttyTab.Controls.Add(checkBoxEnableLog);
			ttyTab.Controls.Add(consoleTextBox);
			resources.ApplyResources(ttyTab, "ttyTab");
			ttyTab.Name = "ttyTab";
			ttyTab.UseVisualStyleBackColor = true;
			resources.ApplyResources(checkBoxEnableLog, "checkBoxEnableLog");
			checkBoxEnableLog.Checked = true;
			checkBoxEnableLog.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxEnableLog.Name = "checkBoxEnableLog";
			checkBoxEnableLog.UseVisualStyleBackColor = true;
			resources.ApplyResources(consoleTextBox, "consoleTextBox");
			consoleTextBox.BackColor = System.Drawing.SystemColors.Control;
			consoleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			consoleTextBox.HideSelection = false;
			consoleTextBox.Name = "consoleTextBox";
			consoleTextBox.ReadOnly = true;
			extractTab.Controls.Add(extractLogTextBox);
			extractTab.Controls.Add(dropBoxPanel);
			resources.ApplyResources(extractTab, "extractTab");
			extractTab.Name = "extractTab";
			extractTab.UseVisualStyleBackColor = true;
			resources.ApplyResources(extractLogTextBox, "extractLogTextBox");
			extractLogTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			extractLogTextBox.Cursor = System.Windows.Forms.Cursors.Default;
			extractLogTextBox.Name = "extractLogTextBox";
			extractLogTextBox.ReadOnly = true;
			dropBoxPanel.AllowDrop = true;
			resources.ApplyResources(dropBoxPanel, "dropBoxPanel");
			dropBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dropBoxPanel.Controls.Add(dropBoxLabel);
			dropBoxPanel.Name = "dropBoxPanel";
			dropBoxPanel.DragDrop += new System.Windows.Forms.DragEventHandler(dropBoxPanel_DragDrop);
			dropBoxPanel.DragEnter += new System.Windows.Forms.DragEventHandler(dropBoxPanel_DragEnter);
			resources.ApplyResources(dropBoxLabel, "dropBoxLabel");
			dropBoxLabel.ForeColor = System.Drawing.SystemColors.GrayText;
			dropBoxLabel.Name = "dropBoxLabel";
			appsDataGridView.AllowUserToAddRows = false;
			appsDataGridView.AllowUserToDeleteRows = false;
			appsDataGridView.AllowUserToResizeRows = false;
			resources.ApplyResources(appsDataGridView, "appsDataGridView");
			appsDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle12.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			appsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
			appsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			appsDataGridView.Columns.AddRange(PaAppName, PaAppKey, PaAppSize);
			dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle13.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			appsDataGridView.DefaultCellStyle = dataGridViewCellStyle13;
			appsDataGridView.GridColor = System.Drawing.SystemColors.ActiveBorder;
			appsDataGridView.Name = "appsDataGridView";
			dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle14.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			appsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle14;
			appsDataGridView.RowHeadersVisible = false;
			appsDataGridView.RowTemplate.Height = 21;
			appsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			appsDataGridView.SelectionChanged += new System.EventHandler(deviceAppsDataGridView_SelectionChanged);
			appsDataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(appsDataGridView_SortCompare);
			PaAppName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(PaAppName, "PaAppName");
			PaAppName.Name = "PaAppName";
			PaAppName.ReadOnly = true;
			PaAppName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			PaAppKey.FillWeight = 60f;
			resources.ApplyResources(PaAppKey, "PaAppKey");
			PaAppKey.Name = "PaAppKey";
			PaAppKey.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			PaAppKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			PaAppSize.FillWeight = 60f;
			resources.ApplyResources(PaAppSize, "PaAppSize");
			PaAppSize.Name = "PaAppSize";
			PaAppSize.ReadOnly = true;
			PaAppSize.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			deviceDataGridView.AllowUserToAddRows = false;
			deviceDataGridView.AllowUserToDeleteRows = false;
			deviceDataGridView.AllowUserToResizeRows = false;
			deviceDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle15.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			deviceDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
			deviceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			deviceDataGridView.Columns.AddRange(PaDeviceType, PaDeviceName, PaGUID);
			dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle16.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			deviceDataGridView.DefaultCellStyle = dataGridViewCellStyle16;
			deviceDataGridView.GridColor = System.Drawing.SystemColors.ActiveBorder;
			resources.ApplyResources(deviceDataGridView, "deviceDataGridView");
			deviceDataGridView.Name = "deviceDataGridView";
			dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle17.Font = new System.Drawing.Font("MS UI Gothic", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			deviceDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
			deviceDataGridView.RowHeadersVisible = false;
			deviceDataGridView.RowTemplate.Height = 21;
			deviceDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			deviceDataGridView.SelectionChanged += new System.EventHandler(deviceAppsDataGridView_SelectionChanged);
			resources.ApplyResources(PaDeviceType, "PaDeviceType");
			PaDeviceType.Image = PublishingUtility.Properties.Resources.DeviceAndroid;
			PaDeviceType.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			PaDeviceType.Name = "PaDeviceType";
			PaDeviceType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			PaDeviceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(PaDeviceName, "PaDeviceName");
			PaDeviceName.Name = "PaDeviceName";
			PaDeviceName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			PaDeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			resources.ApplyResources(PaGUID, "PaGUID");
			PaGUID.Name = "PaGUID";
			AllowDrop = true;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(cate3PacageAppsButton);
			base.Controls.Add(cate2KeyManageButton);
			base.Controls.Add(cate1MetadataButton);
			base.Controls.Add(panelManager1);
			base.Controls.Add(menuStrip1);
			base.MainMenuStrip = menuStrip1;
			base.Name = "MainForm";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MainForm_FormClosed);
			base.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
			base.DragEnter += new System.Windows.Forms.DragEventHandler(MainForm_DragEnter);
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
			contextMenuStrip1.ResumeLayout(false);
			panelManager1.ResumeLayout(false);
			managedPanel1.ResumeLayout(false);
			panelMetadata.ResumeLayout(false);
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage2.ResumeLayout(false);
			tabPage2.PerformLayout();
			nameDescriptionPanel.ResumeLayout(false);
			nameDescriptionPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)appNameDataGridView).EndInit();
			tabIcon.ResumeLayout(false);
			tabIcon.PerformLayout();
			tabPage4.ResumeLayout(false);
			tabPage4.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
			tabPage3.ResumeLayout(false);
			tabPage3.PerformLayout();
			managedPanel2.ResumeLayout(false);
			panelKeyManagement.ResumeLayout(false);
			groupBox5.ResumeLayout(false);
			groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridDeviceList).EndInit();
			contextMenuStripDevices.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dataGridApplicationID).EndInit();
			contextMenuStripApplicationID.ResumeLayout(false);
			groupBox4.ResumeLayout(false);
			groupBox4.PerformLayout();
			managedPanel3.ResumeLayout(false);
			panelPackageApp.ResumeLayout(false);
			panelPackageApp.PerformLayout();
			appTabControl.ResumeLayout(false);
			ttyTab.ResumeLayout(false);
			ttyTab.PerformLayout();
			extractTab.ResumeLayout(false);
			extractTab.PerformLayout();
			dropBoxPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)appsDataGridView).EndInit();
			((System.ComponentModel.ISupportInitialize)deviceDataGridView).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
