using System.ComponentModel;

namespace PublishingUtility
{
	[TypeConverter(typeof(GenreDisplayNameConverter))]
	public enum Genre
	{
		None = 0,
		Game_Action = 0x1000000,
		Game_Adventure = 16842752,
		Game_Arcade = 16908288,
		Game_BrainTraining = 16973824,
		Game_Casual = 17039360,
		Game_Educational = 17104896,
		Game_Family = 17170432,
		Game_Fighting = 17235968,
		Game_Fitness = 17301504,
		Game_Horror = 17367040,
		Game_MusicRhythm = 17432576,
		Game_Party = 17498112,
		Game_Puzzle = 17563648,
		Game_Racing = 17629184,
		Game_RPG = 17694720,
		Game_Shooter = 17760256,
		Game_Simulation = 17825792,
		Game_Sports = 17891328,
		Game_Strategy = 17956864,
		Game_Unique = 18022400,
		App_Audio = 0x2000000,
		App_Books = 33619968,
		App_Business = 33685504,
		App_Catalogs = 33751040,
		App_Comics = 33816576,
		App_Communication = 33882112,
		App_Education = 33947648,
		App_Entertainment = 34013184,
		App_Finance = 34078720,
		App_HealthAndFitness = 34144256,
		App_Lifestyle = 34209792,
		App_Magazines = 34275328,
		App_Medical = 34340864,
		App_Music = 34406400,
		App_News = 34471936,
		App_Personalization = 34537472,
		App_Photo = 34603008,
		App_Productivity = 34668544,
		App_SocialNetworking = 34734080,
		App_Sports = 34799616,
		App_Travel = 34865152,
		App_Utilities = 34930688,
		App_Video = 34996224,
		App_Weather = 35061760
	}
}
