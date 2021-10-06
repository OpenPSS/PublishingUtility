using System.ComponentModel;

namespace PublishingUtility
{
	[TypeConverter(typeof(LocaleDisplayNameConverter))]
	public enum Locale
	{
		en_US,
		en_GB,
		ja_JP,
		fr_FR,
		es_ES,
		de_DE,
		it_IT,
		nl_NL,
		pt_PT,
		pt_BR,
		ru_RU,
		ko_KR,
		zh_Hans,
		zh_Hant,
		fi_FI,
		sv_SE,
		da_DK,
		nb_NO,
		pl_PL
	}
}
