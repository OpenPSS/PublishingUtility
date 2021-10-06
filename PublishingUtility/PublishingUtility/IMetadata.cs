using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PublishingUtility
{
	[DefaultProperty("ProjectName")]
	public abstract class IMetadata
	{
		[Browsable(false)]
		public int EsrbRatingNumber;

		[DisplayName("Managed Heap")]
		[SRDescription("descriptionManagedHeap")]
		[Category("1.Development")]
		public abstract int ManagedHeap { get; set; }

		[DisplayName("Resource Heap")]
		[Category("1.Development")]
		[SRDescription("descriptionResourceHeap")]
		public abstract int ResourceHeap { get; set; }

		[Category("1.Development")]
		[SRDescription("descriptionMaxScreenSize")]
		[DisplayName("Max Screen Size")]
		public abstract MaxScreenSize MaxSize { get; set; }

		[SRDescription("descriptionMaxCameraResolutionSize")]
		[Category("1.Development")]
		[DisplayName("Max Camera Resolution Size")]
		public abstract MaxCameraResolutionSize CameraResolutionSize { get; set; }

		[SRDescription("descriptionDeviceGamePad")]
		[Category("1.Development")]
		[DisplayName("GamePad")]
		public abstract bool DeviceGamePad { get; set; }

		[Category("1.Development")]
		[SRDescription("descriptionDeviceTouch")]
		[DisplayName("Touch")]
		public abstract bool DeviceTouch { get; set; }

		[Category("1.Development")]
		[SRDescription("descriptionDeviceMotion")]
		[DisplayName("Motion")]
		public abstract bool DeviceMotion { get; set; }

		[Category("1.Development")]
		[SRDescription("descriptionDeviceLocation")]
		[DisplayName("Location")]
		public abstract bool DeviceLocation { get; set; }

		[SRDescription("descriptionDeviceCamera")]
		[Category("1.Development")]
		[DisplayName("Camera")]
		public abstract bool DeviceCamera { get; set; }

		[Category("1.Development")]
		[SRDescription("descriptionDevicePSVitaTV")]
		[DisplayName("PS Vita TV")]
		public abstract bool DevicePSVitaTV { get; set; }

		[SRDescription("descriptionProjectName")]
		[DisplayName("Application ID")]
		[Category("2.Application")]
		public abstract string ProjectName { get; set; }

		[SRDescription("descriptionVersion")]
		[DisplayName("Version")]
		[Category("2.Application")]
		public abstract string AppVersion { get; set; }

		[DisplayName("Runtime Version")]
		[SRDescription("descriptionRuntimeVersion")]
		[Category("2.Application")]
		[ReadOnly(true)]
		public abstract string RuntimeVersion { get; set; }

		[Browsable(false)]
		public abstract string SdkVersion { get; set; }

		[SRDescription("descriptionAppLongNames")]
		[Browsable(false)]
		[Category("2.Application")]
		public abstract Hashtable AppLongNames { get; set; }

		[Browsable(false)]
		[Category("2.Application")]
		[SRDescription("descriptionAppShortNames")]
		public abstract Hashtable AppShortNames { get; set; }

		[SRDescription("descriptionDefaultLocale")]
		[Category("2.Application")]
		[DisplayName("Default Locale")]
		public abstract Locale DefaultLocale { get; set; }

		[Browsable(false)]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[SRDescription("descriptionIcon512x512")]
		[DisplayName("Icon 512x512")]
		[Category("3.Image")]
		public abstract string Icon512x512 { get; set; }

		[Browsable(false)]
		[SRDescription("descriptionIcon256x256")]
		[DisplayName("Icon 256x256")]
		[Category("3.Image")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public abstract string Icon256x256 { get; set; }

		[DisplayName("Icon 128x128")]
		[SRDescription("descriptionIcon128x128")]
		[Browsable(false)]
		[Category("3.Image")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public abstract string Icon128x128 { get; set; }

		[SRDescription("descriptionSplash")]
		[Browsable(false)]
		[DisplayName("Splash")]
		[Category("3.Image")]
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		public abstract string Splash { get; set; }

		[Category("3.Genre")]
		[SRDescription("descriptionGenre1")]
		[DisplayName("Primary Genre")]
		public abstract Genre Genre1 { get; set; }

		[Category("3.Genre")]
		[DisplayName("Secondary Genre")]
		[SRDescription("descriptionGenre2")]
		public abstract Genre Genre2 { get; set; }

		[Browsable(false)]
		public abstract string DeveloperName { get; set; }

		[SRDescription("descriptionDeveloperWebSite")]
		[DisplayName("Website")]
		[DefaultValue("*")]
		[Category("4.Developer")]
		public abstract string DeveloperWebSite { get; set; }

		[DisplayName("Copyright Short")]
		[SRDescription("descriptionCopyrightShort")]
		[Category("4.Developer")]
		public abstract string CopyrightShort { get; set; }

		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[SRDescription("descriptionCopyright")]
		[DisplayName("Copyright")]
		[Category("4.Developer")]
		public abstract string Copyright { get; set; }

		[DisplayName("Scoreboards")]
		[SRDescription("descriptionScoreboards")]
		[Browsable(false)]
		[Category("5.PSN(SM)")]
		public abstract bool UseScoreboards { get; set; }

		[Browsable(false)]
		public int HighestAgeLimit
		{
			get
			{
				int esrbRatingNumber = EsrbRatingNumber;
				int ratingAge = Program._RatingData.RatingAge;
				int val = int.Parse(PegiAgeRatingText);
				return Math.Max(Math.Max(esrbRatingNumber, ratingAge), val);
			}
		}

		[Browsable(false)]
		public string PegiAgeRatingText { get; set; }

		[Browsable(false)]
		public string PegiCode { get; set; }

		[Browsable(false)]
		public string EsrbRatingText { get; set; }

		[Browsable(false)]
		public string EsrbCode { get; set; }

		[Browsable(false)]
		public bool IsPegiDrugs { get; set; }

		[Browsable(false)]
		public bool IsPegiLanguage { get; set; }

		[Browsable(false)]
		public bool IsPegiSex { get; set; }

		[Browsable(false)]
		public bool IsPegiScaryElements { get; set; }

		[Browsable(false)]
		public bool IsPegiOnline { get; set; }

		[Browsable(false)]
		public bool IsPegiDiscrimination { get; set; }

		[Browsable(false)]
		public bool IsPegiViolence { get; set; }

		[Browsable(false)]
		public bool IsPegiCrime { get; set; }

		[Browsable(false)]
		public bool IsPegiGambling { get; set; }
	}
}
