using System;
using System.Collections.Generic;

namespace PublishingUtility
{
	public class GenreDisplayNameConverter : EnumDisplayNameConverter
	{
		private Dictionary<object, string> table2 = new Dictionary<object, string>();

		public GenreDisplayNameConverter(Type type)
			: base(type)
		{
		}

		protected override void BuildTable(Dictionary<object, string> table)
		{
			table.Add(Genre.None, "");
			table.Add(Genre.Game_Action, "Games - Action");
			table.Add(Genre.Game_Adventure, "Games - Adventure");
			table.Add(Genre.Game_Arcade, "Games - Arcade");
			table.Add(Genre.Game_BrainTraining, "Games - BrainTraining");
			table.Add(Genre.Game_Casual, "Games - Casual");
			table.Add(Genre.Game_Educational, "Games - Educational");
			table.Add(Genre.Game_Family, "Games - Family");
			table.Add(Genre.Game_Fighting, "Games - Fighting");
			table.Add(Genre.Game_Fitness, "Games - Fitness");
			table.Add(Genre.Game_Horror, "Games - Horror");
			table.Add(Genre.Game_MusicRhythm, "Games - Music Rhythm");
			table.Add(Genre.Game_Party, "Games - Party");
			table.Add(Genre.Game_Puzzle, "Games - Puzzle");
			table.Add(Genre.Game_Racing, "Games - Racing");
			table.Add(Genre.Game_RPG, "Games - Role-playing Games (RPG)");
			table.Add(Genre.Game_Shooter, "Games - Shooter");
			table.Add(Genre.Game_Simulation, "Games - Simulation");
			table.Add(Genre.Game_Sports, "Games - Sports");
			table.Add(Genre.Game_Strategy, "Games - Strategy");
			table.Add(Genre.Game_Unique, "Games - Unique");
			table.Add(Genre.App_Audio, "Apps - Audio");
			table.Add(Genre.App_Books, "Apps - Books");
			table.Add(Genre.App_Business, "Apps - Business");
			table.Add(Genre.App_Catalogs, "Apps - Catalogs");
			table.Add(Genre.App_Comics, "Apps - Comics");
			table.Add(Genre.App_Communication, "Apps - Communication");
			table.Add(Genre.App_Education, "Apps - Education");
			table.Add(Genre.App_Entertainment, "Apps - Entertainment");
			table.Add(Genre.App_Finance, "Apps - Finance");
			table.Add(Genre.App_HealthAndFitness, "Apps - Health & Fitness");
			table.Add(Genre.App_Lifestyle, "Apps - Lifestyle");
			table.Add(Genre.App_Magazines, "Apps - Magazines");
			table.Add(Genre.App_Medical, "Apps - Medical");
			table.Add(Genre.App_Music, "Apps - Music");
			table.Add(Genre.App_News, "Apps - News");
			table.Add(Genre.App_Personalization, "Apps - Personalization");
			table.Add(Genre.App_Photo, "Apps - Photo");
			table.Add(Genre.App_Productivity, "Apps - Productivity");
			table.Add(Genre.App_SocialNetworking, "Apps - Social Networking");
			table.Add(Genre.App_Sports, "Apps - Sports");
			table.Add(Genre.App_Travel, "Apps - Travel");
			table.Add(Genre.App_Utilities, "Apps - Utilities");
			table.Add(Genre.App_Video, "Apps - Video");
			table.Add(Genre.App_Weather, "Apps - Weather");
			table2.Add(Genre.None, "");
			table2.Add(Genre.Game_Action, "Games:Action");
			table2.Add(Genre.Game_Adventure, "Games:Adventure");
			table2.Add(Genre.Game_Arcade, "Games:Arcade");
			table2.Add(Genre.Game_BrainTraining, "Games:BrainTraining");
			table2.Add(Genre.Game_Casual, "Games:Casual");
			table2.Add(Genre.Game_Educational, "Games:Educational");
			table2.Add(Genre.Game_Family, "Games:Family");
			table2.Add(Genre.Game_Fighting, "Games:Fighting");
			table2.Add(Genre.Game_Fitness, "Games:Fitness");
			table2.Add(Genre.Game_Horror, "Games:Horror");
			table2.Add(Genre.Game_MusicRhythm, "Games:MusicRhythm");
			table2.Add(Genre.Game_Party, "Games:Party");
			table2.Add(Genre.Game_Puzzle, "Games:Puzzle");
			table2.Add(Genre.Game_Racing, "Games:Racing");
			table2.Add(Genre.Game_RPG, "Games:RPG");
			table2.Add(Genre.Game_Shooter, "Games:Shooter");
			table2.Add(Genre.Game_Simulation, "Games:Simulation");
			table2.Add(Genre.Game_Sports, "Games:Sports");
			table2.Add(Genre.Game_Strategy, "Games:Strategy");
			table2.Add(Genre.Game_Unique, "Games:Unique");
			table2.Add(Genre.App_Audio, "Apps:Audio");
			table2.Add(Genre.App_Books, "Apps:Books");
			table2.Add(Genre.App_Business, "Apps:Business");
			table2.Add(Genre.App_Catalogs, "Apps:Catalogs");
			table2.Add(Genre.App_Comics, "Apps:Comics");
			table2.Add(Genre.App_Communication, "Apps:Communication");
			table2.Add(Genre.App_Education, "Apps:Education");
			table2.Add(Genre.App_Entertainment, "Apps:Entertainment");
			table2.Add(Genre.App_Finance, "Apps:Finance");
			table2.Add(Genre.App_HealthAndFitness, "Apps:HealthAndFitness");
			table2.Add(Genre.App_Lifestyle, "Apps:Lifestyle");
			table2.Add(Genre.App_Magazines, "Apps:Magazines");
			table2.Add(Genre.App_Medical, "Apps:Medical");
			table2.Add(Genre.App_Music, "Apps:Music");
			table2.Add(Genre.App_News, "Apps:News");
			table2.Add(Genre.App_Personalization, "Apps:Personalization");
			table2.Add(Genre.App_Photo, "Apps:Photo");
			table2.Add(Genre.App_Productivity, "Apps:Productivity");
			table2.Add(Genre.App_SocialNetworking, "Apps:SocialNetworking");
			table2.Add(Genre.App_Sports, "Apps:Sports");
			table2.Add(Genre.App_Travel, "Apps:Travel");
			table2.Add(Genre.App_Utilities, "Apps:Utilities");
			table2.Add(Genre.App_Video, "Apps:Video");
			table2.Add(Genre.App_Weather, "Apps:Weather");
		}

		public object XmlConvertFrom(object value)
		{
			string text = value as string;
			Dictionary<object, string>.Enumerator enumerator = table2.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value == text)
				{
					return enumerator.Current.Key;
				}
			}
			return null;
		}

		public object XmlConvertTo(object value)
		{
			if (table2.TryGetValue(value, out var value2))
			{
				return value2;
			}
			return null;
		}
	}
}
