namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class Achievement
	{
		public Achievement()
		{
			this.AchievementLevels = new List<AchievementLevel>();
			this.Facts = new List<AchievementLevelFact>();
		}

		[Key]
		public string Name { get; set; }
		public string Description { get; set; }
		[Required]
		public bool? AlwaysGainsInfluence { get; set; }

		[InverseProperty("Achievement")]
		public virtual List<AchievementLevel> AchievementLevels { get; set; }
		[InverseProperty("Achievement")]
		public virtual List<AchievementLevelFact> Facts { get; set; }
	}

	public abstract class AchievementSub<T> : Achievement
		where T : AchievementLevel
	{

		public IEnumerable<T> Levels
		{
			get
			{
				return this.AchievementLevels.OfType<T>();
			}
		}
	}

	public class RoleAchievement : AchievementSub<AchievementLevel>
	{
		[Required]
		public string Role_Name { get; set; }

		[ForeignKey("")]
		public virtual Role Role { get; set; }
	}

	public abstract class CounterAchievement : AchievementSub<CounterAchievementLevel>
	{
	}

	public class InteractionAchievement : CounterAchievement
	{
	}

	public class NPCKillAchievement : CounterAchievement
	{
	}

	public class PlayerKillAchievement : CounterAchievement
	{
	}

	public class WeaponKillAchievement : CounterAchievement
	{
	}

	public abstract class LocationAchievement : AchievementSub<AchievementLevel>
	{
	}

	public class SettlementLocationAchievement : LocationAchievement
	{
	}

	public class SpecialLocationAchievement : LocationAchievement
	{
	}

	public abstract class CraftAchievement : AchievementSub<CraftAchievementLevel>
	{
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string Rarity { get; set; }
	}

	public class CraftingAchievement : CraftAchievement
	{
		[Required]
		public string CraftingSkill_Name { get; set; }

		[ForeignKey("CraftingSkill_Name")]
		public virtual CraftingSkill CraftingSkill { get; set; }
	}

	public class RefiningAchievement : CraftAchievement
	{
		[Required]
		public string RefiningSkill_Name { get; set; }

		[ForeignKey("RefiningSkill_Name")]
		public virtual RefiningSkill RefiningSkill { get; set; }
	}
}
