namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;

	using Goblinary.Common;

	internal class FactData
	{
		public enum FeatRankFactTypes
		{
			AbilityBonus,
			AbilityRequirement,
			AchievementRequirement,
			CategoryRequirement,
			FeatRequirement
		}

		public enum AchievementRankFactTypes
		{
			CategoryBonus,
			FeatRequirement,
			FlagRequirement
		}

		public static List<FactData> GetFacts(string factsText)
		{
			List<FactData> facts = new List<FactData>();
			int factNo = 1;
			foreach (string fact in factsText.Replace(" or ", "|").Replace(", ", ",").Split(','))
				if (fact != "")
				{
					int optionNo = 1;
					foreach (string option in fact.Split('|'))
					{
						if (option != "")
						{
							string[] parts = option.Split('=');
							facts.Add(new FactData(factNo, optionNo++, parts[0], parts[1]));
						}
					}
					factNo++;
				}
			return facts;
		}

		public static List<FactData> GetFacts(WorkDataSet.AchievementRanksRow source, IEnumerable<string> categories)
		{
			List<FactData> facts = new List<FactData>();
			int factNo = 1;
			int optionNo = 1;
			foreach (string category in categories)
				if (source[category].ToString() != "")
					facts.Add(new FactData(factNo++, optionNo, category, source[category].ToString()));
			return facts;
		}

		private FactData(int factNo, int optionNo, string name, string value)
		{
			this.FactNo = factNo;
			this.OptionNo = optionNo;
			this.Name = name;
			this.Value = value;
		}

		public int FactNo { get; private set; }
		public int OptionNo { get; private set; }
		public string Name { get; private set; }
		public string Value { get; private set; }
	}
}
