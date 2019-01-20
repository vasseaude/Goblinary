namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class Feat
	{
		public Feat()
		{
			this.Effects = new List<FeatEffect>();
			this.Ranks = new List<FeatRank>();
			this.RankEffects = new List<FeatRankEffect>();
			this.RankKeywords = new List<FeatRankKeyword>();
		}

		[Key]
		public string Name { get; set; }
		[Key]
		public string Role_Name { get; set; }
		public string Advancement_Name { get; set; }

		[ForeignKey("Role_Name")]
		public virtual Role Role { get; set; }
		[ForeignKey("Advancement_Name")]
		public virtual Advancement Advancement { get; set; }

		[InverseProperty("Feat")]
		public virtual List<FeatEffect> Effects { get; private set; }
		[InverseProperty("Feat")]
		public virtual List<FeatRank> Ranks { get; private set; }
		[InverseProperty("Feat")]
		public virtual List<FeatRankEffect> RankEffects { get; private set; }
		[InverseProperty("Feat")]
		public virtual List<FeatRankKeyword> RankKeywords { get; private set; }
	}

	public abstract class ActiveFeat : Feat
	{
		[Required]
		public decimal? DamageFactor { get; set; }
		[Required]
		public decimal? AttackSeconds { get; set; }
		[Required]
		public int? StaminaCost { get; set; }
		[Required]
		public bool? IsMelee { get; set; }
		public decimal? Range { get; set; }
		[Required]
		public string WeaponCategory_Name { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public WeaponCategory WeaponCategory { get; set; }
	}

	public abstract class StandardAttack : ActiveFeat
	{
		[Required]
		public string AttackForm_Name { get; set; }
		[Required]
		public decimal? CooldownSeconds { get; set; }
		public string SpecificWeapon_Name { get; set; }

		[ForeignKey("AttackForm_Name")]
		public virtual AttackForm Form { get; set; }
		[ForeignKey("SpecificWeapon_Name")]
		public virtual WeaponType SpecificWeapon { get; set; }
	}

	public abstract class AttackFeat : StandardAttack
	{
	}

	public class PhysicalAttack : AttackFeat
	{
	}

	public class Cantrip : AttackFeat
	{
		[Required]
		public string School_Name { get; set; }

		[ForeignKey("School_Name")]
		public virtual School School { get; set; }
	}

	public class Orison : AttackFeat
	{
	}

	public class Utility : StandardAttack
	{
	}

	public abstract class PowerAttack : ActiveFeat
	{
		[Required]
		public int? PowerCost { get; set; }
		[Required]
		public int? Level { get; set; }
		[Required]
		public bool? HasEndOfCombatCooldown { get; set; }
		[Required]
		public string AttackBonus_Name { get; set; }

		[ForeignKey("AttackBonus_Name")]
		public virtual AttackBonus AttackBonus { get; set; }
	}

	public abstract class Expendable : PowerAttack
	{
	}

	public class TrophyCharmManeuver : Expendable
	{
	}

	public class RogueKitManeuver : Expendable
	{
	}

	public class SpellbookSpell : Expendable
	{
	}

	public class HolySymbolSpell : Expendable
	{
	}

	public class HoldoutWeaponManeuver : Expendable
	{
	}

	public class ToolkitManeuver : Expendable
	{
	}

	public class Consumable : PowerAttack
	{
	}

	public class ChanneledFeat : Feat
	{
		[Required]
		public string Channel_Name { get; set; }

		[ForeignKey("Channel_Name")]
		public virtual EffectChannel Channel { get; set; }
	}

	public class PassiveFeat : ChanneledFeat
	{
	}

	public class DefensiveFeat : PassiveFeat
	{
	}

	public class ReactiveFeat : PassiveFeat
	{
	}

	public class ArmorFeat : PassiveFeat
	{
	}

	public class RoleFeature : PassiveFeat
	{
	}

	public class Upgrade : ChanneledFeat
	{
	}

	public class RacialBenefit : Upgrade
	{
	}

	public class Skill : Upgrade
	{
	}

	public class CraftSkill : Skill
	{
	}

	public class CraftingSkill : CraftSkill
	{
	}

	public class RefiningSkill : CraftSkill
	{
	}

	public class GatheringSkill : Skill
	{
	}

	public class KnowledgeSkill : Skill
	{
	}

	public class Bonus : Upgrade
	{
	}

	public class AttackBonus : Bonus
	{
	}

	public class BaseAttackBonus : AttackBonus
	{
	}

	public class DefenseBonus : Bonus
	{
	}

	public abstract class Proficiency : Upgrade
	{
	}

	public class ArmorProficiency : Proficiency
	{
	}

	public class ImplementProficiency : Proficiency
	{
	}

	public class WeaponProficiency : Proficiency
	{
	}
}
