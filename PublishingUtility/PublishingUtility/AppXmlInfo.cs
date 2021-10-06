using System.Collections.Generic;

namespace PublishingUtility
{
	public struct AppXmlInfo
	{
		public struct AppXmlFormat
		{
			public string version;

			public string sdkType;
		}

		public struct LocalizedItem
		{
			public string locale;

			public string value;
		}

		public struct OnlineFeatures
		{
			public string chat;

			public string personalInfo;

			public string userLocation;

			public string exchangeContent;

			public string mininumAge;
		}

		public struct RatingList
		{
			public string highestAgeLimit;

			public string hasOnlineFeatures;

			public OnlineFeatures onlineFeatures;
		}

		public struct Rating
		{
			public string type;

			public string value;

			public string age;

			public string code;
		}

		public struct Product
		{
			public string label;

			public string type;

			public Dictionary<string, LocalizedItem> names;
		}

		public struct PsnService
		{
			public bool scoreBoards;
		}

		public string appVersion;

		public string runtimeVersion;

		public string sdkVersion;

		public string projectName;

		public AppXmlFormat appXmlFormat;

		public Dictionary<string, LocalizedItem> names;

		public Dictionary<string, LocalizedItem> shortNames;

		public string lockLevel;

		public RatingList ratingList;

		public Rating esrb;

		public Rating pegiex;

		public Rating self;

		public string primaryGenre;

		public string developerName;

		public Dictionary<string, Product> products;

		public PsnService psnService;
	}
}
