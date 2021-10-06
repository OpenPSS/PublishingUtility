using System.Windows.Forms;

namespace PublishingUtility
{
	public class SceRatingData
	{
		public enum GambleLevel
		{
			Level01,
			Level02,
			Level03
		}

		public const string SCE_SCARYELEMENTS = "Fear";

		public const string SCE_LANGUAGE = "Strong Language";

		public const string SCE_VIOLENCE = "Violence/Crime";

		public const string SCE_CRIME = "Violence/Crime";

		public const string SCE_GAMBLING = "Gambling";

		public const string SCE_DISCRIMINATION = "Discrimination";

		public const string SCE_ONLINE = "Online";

		public const string SCE_DRUGS = "Drugs/Alcohol/Tobacco";

		public const string SCE_SEX = "Sex/Romance";

		public const string SCE_CS_SCARYELEMENTS = "Fear";

		public const string SCE_CS_LANGUAGE = "Language";

		public const string SCE_CS_VIOLENCE = "Violence";

		public const string SCE_CS_CRIME = "Crime";

		public const string SCE_CS_GAMBLING = "Gambling";

		public const string SCE_CS_DISCRIMINATION = "Discrimination";

		public const string SCE_CS_ONLINE = "Online";

		public const string SCE_CS_DRUGS = "Drugs";

		public const string SCE_CS_SEX = "Sex";

		public const string SCE_ONLIE_CHAT = "Chat Functionality";

		public const string SCE_ONLIE_PERSONAL_INFO = "Personal Information";

		public const string SCE_ONLIE_USERS_LOCATION = "User's Location";

		public const string SCE_ONLIE_EXCHANGE_CONTENT = "Create/Exchange Content";

		public const string SCE_ONLIE_MINIMUM_AGE = "Minimum Age";

		public string TextResultRating = "";

		public int RatingAge;

		public static readonly string[,] ratingText = new string[14, 2]
		{
			{ "Fear", "恐怖を感じさせる表現" },
			{ "Strong Language", "言語表現" },
			{ "Violence/Crime", "暴力表現" },
			{ "Violence/Crime", "反社会的表現" },
			{ "Gambling", "ギャンブル" },
			{ "Discrimination", "差別的表現" },
			{ "Online", "オンライン機能" },
			{ "Drugs/Alcohol/Tobacco", "麻薬等薬物/アルコール/タバコ" },
			{ "Sex/Romance", "性表現" },
			{ "Chat Functionality", "チャット機能" },
			{ "Personal Information", "個人情報" },
			{ "User's Location", "位置情報" },
			{ "Create/Exchange Content", "コンテンツの交換" },
			{ "Minimum Age", "最低年齢制限" }
		};

		public bool IsCsScaryElements;

		public bool IsCsLanguage;

		public bool IsCsViolence;

		public bool IsCsGambling;

		public bool IsCsDiscrimination;

		public bool IsCsOnline;

		public bool IsCsDrugs;

		public bool IsCsSex;

		public bool IsCsOnlineChat;

		public bool IsCsOnlinePersonalInfo;

		public bool IsCsOnlineUsersLocation;

		public bool IsCsOnlineExchangeContent;

		public bool IsCsOnlineMinimumAge;

		public int CsOnlineMinimumAge;

		public bool IsCsOnlineFeatures;

		public bool IsCheckedOffSceRating;

		public bool IsMainScaryElements01;

		public bool IsMainLanguage01;

		public bool IsMainViolence01;

		public bool IsMainCrime01;

		public bool IsMainGambling01;

		public bool IsMainDiscrimination01;

		public bool IsMainOnline01;

		public bool IsMainDrugs01;

		public bool IsMainSex01;

		public bool IsLanguageQ01;

		public bool IsLanguageQ02;

		public bool IsViolenceQ01;

		public bool IsViolenceQ01_01;

		public bool IsViolenceQ02;

		public bool IsViolenceQ03;

		public bool IsViolenceQ03_01;

		public bool IsViolenceQ04;

		public bool IsViolenceQ04_01;

		public bool IsViolenceQ04_02;

		public bool IsViolenceQ04_03;

		public bool IsViolenceQ05;

		public bool IsViolenceQ05_01;

		public bool IsViolenceQ05_02;

		public bool IsViolenceQ05_03;

		public bool IsViolenceQ05_04;

		public bool IsViolenceQ05_05;

		public bool IsViolenceQ05_06;

		public bool IsViolenceQ05_07;

		public bool IsCrimeQ01;

		public bool IsCrimeQ02;

		public bool IsDrugsQ01;

		public bool IsDrugsQ02;

		public bool IsDrugsQ03;

		public bool IsSexQ01;

		public bool IsSexQ02;

		public bool IsSexQ03;

		public bool IsSexQ04;

		public bool IsOnlineQ01;

		public bool IsOnlineQ02;

		public bool IsOnlineQ03;

		public bool IsOnlineQ04;

		public bool IsOnlineQ05;

		public GambleLevel gambleLevel;

		public void Reset()
		{
			RatingAge = 3;
			IsCheckedOffSceRating = false;
			IsMainSex01 = (IsMainDrugs01 = (IsMainOnline01 = (IsMainDiscrimination01 = (IsMainGambling01 = (IsMainCrime01 = (IsMainViolence01 = (IsMainLanguage01 = (IsMainScaryElements01 = false))))))));
			bool flag = (IsCrimeQ02 = (IsCrimeQ01 = (IsViolenceQ05_07 = (IsViolenceQ05_06 = (IsViolenceQ05_05 = (IsViolenceQ05_04 = (IsViolenceQ05_03 = (IsViolenceQ05_02 = (IsViolenceQ05_01 = (IsViolenceQ05 = (IsViolenceQ04_03 = (IsViolenceQ04_02 = (IsViolenceQ04_01 = (IsViolenceQ04 = (IsViolenceQ03_01 = (IsViolenceQ03 = (IsViolenceQ02 = (IsViolenceQ01_01 = (IsViolenceQ01 = (IsLanguageQ02 = (IsLanguageQ01 = false)))))))))))))))))))));
			gambleLevel = GambleLevel.Level01;
			IsDrugsQ01 = flag;
			IsDrugsQ02 = flag;
			IsDrugsQ03 = flag;
			IsSexQ01 = flag;
			IsSexQ02 = flag;
			IsSexQ03 = flag;
			IsSexQ04 = flag;
			IsOnlineQ01 = flag;
			IsOnlineQ02 = flag;
			IsOnlineQ03 = flag;
			IsOnlineQ04 = flag;
			IsOnlineQ05 = flag;
		}

		private void SetRatingMax(int age)
		{
			if (age > RatingAge)
			{
				RatingAge = age;
			}
		}

		public string Result()
		{
			RatingAge = 3;
			IsCsScaryElements = false;
			IsCsLanguage = false;
			IsCsViolence = false;
			IsCsGambling = false;
			IsCsDiscrimination = false;
			IsCsOnline = false;
			IsCsDrugs = false;
			IsCsSex = false;
			IsCsOnlineChat = false;
			IsCsOnlinePersonalInfo = false;
			IsCsOnlineUsersLocation = false;
			IsCsOnlineExchangeContent = false;
			IsCsOnlineMinimumAge = false;
			IsCsOnlineFeatures = false;
			if (IsMainScaryElements01)
			{
				SetRatingMax(7);
				IsCsScaryElements = true;
			}
			if (IsMainLanguage01)
			{
				if (IsLanguageQ01)
				{
					SetRatingMax(12);
					IsCsLanguage = true;
				}
				if (IsLanguageQ02)
				{
					SetRatingMax(16);
					IsCsLanguage = true;
				}
			}
			if (IsMainViolence01)
			{
				if (IsViolenceQ01)
				{
					IsCsViolence = true;
					if (IsViolenceQ01_01)
					{
						SetRatingMax(7);
					}
					else
					{
						SetRatingMax(3);
					}
				}
				if (IsViolenceQ02)
				{
					SetRatingMax(7);
					IsCsViolence = true;
				}
				if (IsViolenceQ03)
				{
					IsCsViolence = true;
					if (IsViolenceQ03_01)
					{
						SetRatingMax(16);
					}
					else
					{
						SetRatingMax(12);
					}
				}
				if (IsViolenceQ04)
				{
					IsCsViolence = true;
					if (IsViolenceQ04_01)
					{
						SetRatingMax(7);
					}
					if (IsViolenceQ04_02)
					{
						SetRatingMax(7);
					}
					if (IsViolenceQ04_03)
					{
						SetRatingMax(12);
					}
				}
				if (IsViolenceQ05)
				{
					IsCsViolence = true;
					if (IsViolenceQ05_01)
					{
						SetRatingMax(12);
					}
					if (IsViolenceQ05_02)
					{
						SetRatingMax(12);
					}
					if (IsViolenceQ05_03)
					{
						SetRatingMax(16);
					}
					if (IsViolenceQ05_04)
					{
						SetRatingMax(18);
					}
					if (IsViolenceQ05_05)
					{
						SetRatingMax(18);
					}
					if (IsViolenceQ05_06)
					{
						SetRatingMax(18);
					}
					if (IsViolenceQ05_07)
					{
						SetRatingMax(18);
					}
				}
			}
			if (IsMainCrime01)
			{
				if (IsCrimeQ01)
				{
					SetRatingMax(16);
					IsCsViolence = true;
				}
				if (IsCrimeQ02)
				{
					SetRatingMax(18);
					IsCsViolence = true;
				}
			}
			if (IsMainGambling01)
			{
				IsCsGambling = true;
				if (gambleLevel == GambleLevel.Level01)
				{
					SetRatingMax(12);
					IsCsGambling = true;
				}
				else if (gambleLevel == GambleLevel.Level02)
				{
					SetRatingMax(16);
					IsCsGambling = true;
				}
				else
				{
					SetRatingMax(18);
					IsCsGambling = true;
				}
			}
			if (IsMainDiscrimination01)
			{
				SetRatingMax(99);
				IsCsDiscrimination = true;
			}
			if (IsMainOnline01)
			{
				SetRatingMax(3);
				IsCsOnline = true;
			}
			if (IsMainDrugs01)
			{
				if (IsDrugsQ01)
				{
					SetRatingMax(16);
					IsCsDrugs = true;
				}
				if (IsDrugsQ02)
				{
					SetRatingMax(16);
					IsCsDrugs = true;
				}
				if (IsDrugsQ03)
				{
					SetRatingMax(18);
					IsCsDrugs = true;
				}
			}
			if (IsMainSex01)
			{
				if (IsSexQ01)
				{
					SetRatingMax(3);
					IsCsSex = true;
				}
				if (IsSexQ02)
				{
					SetRatingMax(12);
					IsCsSex = true;
				}
				if (IsSexQ03)
				{
					SetRatingMax(16);
					IsCsSex = true;
				}
				if (IsSexQ04)
				{
					SetRatingMax(18);
					IsCsSex = true;
				}
			}
			if (IsOnlineQ01)
			{
				IsCsOnlineChat = true;
			}
			if (IsOnlineQ02)
			{
				IsCsOnlinePersonalInfo = true;
			}
			if (IsOnlineQ03)
			{
				IsCsOnlineUsersLocation = true;
			}
			if (IsOnlineQ04)
			{
				IsCsOnlineExchangeContent = true;
			}
			if (IsOnlineQ05)
			{
				IsCsOnlineMinimumAge = true;
			}
			if (!IsCsOnlineChat && !IsCsOnlinePersonalInfo && !IsCsOnlineUsersLocation && !IsCsOnlineExchangeContent && !IsCsOnlineMinimumAge)
			{
				IsCsOnlineFeatures = false;
			}
			else
			{
				IsCsOnlineFeatures = true;
			}
			return OutputResultText();
		}

		public string OutputResultText()
		{
			int num = ((Program.appConfigData.PcLocale == "ja-JP") ? 1 : 0);
			string text = "";
			text = ((RatingAge != 99) ? (text + "Rating=" + RatingAge + "\r\n") : (text + ((Program.appConfigData.PcLocale == "ja-JP") ? "このPSMアプリはPSMアプリケーション開発ガイドラインに違反しています。\r\n" : "This PSM application violates the PSM Application Development Guidelines.\r\n")));
			if (IsCsScaryElements)
			{
				text = text + ratingText[0, num] + ", ";
			}
			if (IsCsLanguage)
			{
				text = text + ratingText[1, num] + ", ";
			}
			if (IsCsViolence)
			{
				text = text + ratingText[2, num] + ", ";
			}
			if (IsCsGambling)
			{
				text = text + ratingText[4, num] + ", ";
			}
			if (IsCsDiscrimination)
			{
				text = text + ratingText[5, num] + ", ";
			}
			if (IsCsOnline)
			{
				text = text + ratingText[6, num] + ", ";
			}
			if (IsCsDrugs)
			{
				text = text + ratingText[7, num] + ", ";
			}
			if (IsCsSex)
			{
				text = text + ratingText[8, num] + ", ";
			}
			text += ((num == 0) ? "\r\n[Online Features]\r\n" : "\r\n[オンライン要素] ");
			text = (IsCsOnlineFeatures ? (text + ((num == 0) ? "Yes\r\n" : "あり\r\n")) : (text + ((num == 0) ? "none\r\n" : "なし\r\n")));
			if (IsCsOnlineChat)
			{
				text = text + ratingText[9, num] + ", ";
			}
			if (IsCsOnlinePersonalInfo)
			{
				text = text + ratingText[10, num] + ", ";
			}
			if (IsCsOnlineUsersLocation)
			{
				text = text + ratingText[11, num] + ", ";
			}
			if (IsCsOnlineExchangeContent)
			{
				text = text + ratingText[12, num] + ", ";
			}
			if (IsCsOnlineMinimumAge)
			{
				text = text + ratingText[13, num] + "=" + CsOnlineMinimumAge;
			}
			return text;
		}

		public static void CheckCloseWindow(Form form, FormClosingEventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show((Program.appConfigData.PcLocale == "ja-JP") ? "レーティングチェックを中断しますか？" : "Do you want to cancel Rating Check?", (Program.appConfigData.PcLocale == "ja-JP") ? "レーティングチェック" : "Rating Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (dialogResult == DialogResult.Yes)
			{
				e.Cancel = false;
				form.DialogResult = DialogResult.Cancel;
			}
			else
			{
				e.Cancel = true;
			}
		}
	}
}
