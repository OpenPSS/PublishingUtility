using System;
using System.Collections.Generic;

namespace PublishingUtility
{
	public class LocaleDisplayNameConverter : EnumDisplayNameConverter
	{
		public LocaleDisplayNameConverter(Type type)
			: base(type)
		{
		}

		protected override void BuildTable(Dictionary<object, string> table)
		{
			table.Add(Locale.ja_JP, "ja-JP");
			table.Add(Locale.en_US, "en-US");
			table.Add(Locale.en_GB, "en-GB");
			table.Add(Locale.fr_FR, "fr-FR");
			table.Add(Locale.es_ES, "es-ES");
			table.Add(Locale.de_DE, "de-DE");
			table.Add(Locale.it_IT, "it-IT");
			table.Add(Locale.nl_NL, "nl-NL");
			table.Add(Locale.pt_PT, "pt-PT");
			table.Add(Locale.pt_BR, "pt-BR");
			table.Add(Locale.ru_RU, "ru-RU");
			table.Add(Locale.ko_KR, "ko-KR");
			table.Add(Locale.zh_Hans, "zh-Hans");
			table.Add(Locale.zh_Hant, "zh-Hant");
			table.Add(Locale.fi_FI, "fi-FI");
			table.Add(Locale.sv_SE, "sv-SE");
			table.Add(Locale.da_DK, "da-DK");
			table.Add(Locale.nb_NO, "nb-NO");
			table.Add(Locale.pl_PL, "pl-PL");
		}
	}
}
