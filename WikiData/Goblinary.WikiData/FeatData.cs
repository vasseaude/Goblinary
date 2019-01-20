namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;

	using Goblinary.Common;

	internal class FeatData
	{
		public enum FeatTypes
		{
			PhysicalAttack,
			Cantrip,
			Orison,
			Utility,
			FighterManeuver,
			RogueManeuver,
			ClericSpell,
			WizardSpell,
			Consumable,
			Defensive,
			Reactive,
			Feature,
			ArmorFeat,
			Upgrade,
			ProficiencyFeat
		}

		public enum FeatCategories
		{
			Feat,
			ActiveFeat,
			Attack,
			PowerAttack,
			PassiveFeat
		}

		static FeatData()
		{
			featTypeDictionary[FeatTypes.PhysicalAttack] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.Attack };
			featTypeDictionary[FeatTypes.Cantrip] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.Attack };
			featTypeDictionary[FeatTypes.Orison] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.Attack };
			featTypeDictionary[FeatTypes.Utility] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.Attack };
			featTypeDictionary[FeatTypes.FighterManeuver] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.PowerAttack };
			featTypeDictionary[FeatTypes.RogueManeuver] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.PowerAttack };
			featTypeDictionary[FeatTypes.ClericSpell] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.PowerAttack };
			featTypeDictionary[FeatTypes.WizardSpell] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.PowerAttack };
			featTypeDictionary[FeatTypes.Consumable] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.ActiveFeat, FeatCategories.PowerAttack };
			featTypeDictionary[FeatTypes.Defensive] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.PassiveFeat };
			featTypeDictionary[FeatTypes.Reactive] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.PassiveFeat };
			featTypeDictionary[FeatTypes.Feature] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.PassiveFeat };
			featTypeDictionary[FeatTypes.ArmorFeat] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.PassiveFeat };
			featTypeDictionary[FeatTypes.Upgrade] = new List<FeatCategories> { FeatCategories.Feat, FeatCategories.PassiveFeat };
			featTypeDictionary[FeatTypes.ProficiencyFeat] = new List<FeatCategories> { FeatCategories.Feat };
		}

		private static Dictionary<FeatTypes, List<FeatCategories>> featTypeDictionary = new Dictionary<FeatTypes, List<FeatCategories>>();

		public static bool HasCategory(FeatTypes featType, FeatCategories featCategory)
		{
			return featTypeDictionary[featType].Contains(featCategory);
		}

		public string FeatName = null;
		public string FeatType = null;
		public string Worksheet = null;
		public WorkDataSet.AdvancementRanksRow AdvancementRanksRow = null;
		public WorkDataSet.FeatRanksRow FeatRanksRow = null;
	}
}
