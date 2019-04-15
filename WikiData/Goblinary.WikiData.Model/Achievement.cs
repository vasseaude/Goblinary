namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Common;

	public abstract class Achievement : IEntity
	{
        public Achievement() => CreateRanks();

        protected virtual void CreateRanks()
		{
			CreateRanks<AchievementRank>(new List<AchievementRank>());
		}

		protected virtual void CreateRanks<T>(IList<T> ranks)
			where T : AchievementRank
		{
			Ranks = new VariantList<AchievementRank, T>(ranks);
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseTypeName { get; set; }
		[Required]
		public string AchievementTypeName { get; set; }
		[Required]
		public string AchievementGroupName { get; set; }

		[ForeignKey("BaseType_Name, AchievementType_Name")]
		public virtual EntityType AchievementType { get; set; }
		[ForeignKey("AchievementGroup_Name")]
		public virtual AchievementGroup AchievementGroup { get; set; }

		[InverseProperty("Achievement")]
		public virtual IList<AchievementRank> Ranks { get; set; }

		public static Func<Achievement, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}

	public abstract class CounterAchievement : Achievement { }

	public class InteractionAchievement : CounterAchievement { }

	public class NPCKillAchievement : CounterAchievement { }

	public class WeaponKillAchievement : CounterAchievement { }

	public class PlayerKillAchievement : CounterAchievement { }

	public abstract class FlagAchievement : Achievement { }

	public class SettlementLocationAchievement : FlagAchievement { }

	public class SpecialLocationAchievement : FlagAchievement { }

	public abstract class CraftAchievement : Achievement
	{
		protected override void CreateRanks()
		{
			base.CreateRanks<CraftAchievementRank>(new List<CraftAchievementRank>());
		}
	}

	public class RefiningAchievement : CraftAchievement { }

	public class CraftingAchievement : CraftAchievement { }

	public class FeatAchievement : Achievement { }
}
