namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public abstract class Feat
	{
	    protected Feat()
		{
			Ranks = new EntityList<FeatRank>();
			Effects = new EntityList<FeatEffect>();
			AchievementRanks = new EntityList<CraftAchievementRank>();
			HoldingUpgrades = new EntityList<HoldingUpgrade>();
			OutpostWorkerFeats = new EntityList<OutpostWorkerFeat>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseTypeName { get; set; }
		[Required]
		public string FeatTypeName { get; set; }
		[Required]
		[DisplayName("Role")]
		public string RoleName { get; set; }
		[DisplayName("Advancement Feat")]
		public string AdvancementFeatName { get; set; }

		[ForeignKey("BaseType_Name, FeatType_Name")]
		public virtual EntityType FeatType { get; set; }
		[ForeignKey("Role_Name")]
		public virtual Role Role { get; set; }
		[ForeignKey("AdvancementFeat_Name")]
		public virtual AdvancementFeat AdvancementFeat { get; set; }

		[InverseProperty("Feat")]
		public virtual EntityList<FeatRank> Ranks { get; }
		[InverseProperty("Feat")]
		public virtual EntityList<FeatEffect> Effects { get; }
		[InverseProperty("Feat")]
		public virtual EntityList<CraftAchievementRank> AchievementRanks { get; }
		[InverseProperty("CraftingFacilityFeat")]
		public virtual EntityList<HoldingUpgrade> HoldingUpgrades { get; }
		[InverseProperty("WorkerFeat")]
		public virtual EntityList<OutpostWorkerFeat> OutpostWorkerFeats { get; }

		private List<FeatRankKeyword> _keywords;
		[NotMapped]
		public List<FeatRankKeyword> Keywords => _keywords ?? (_keywords = (
		                                             from r in Ranks
		                                             from k in r.Keywords
		                                             orderby k.Feat_Rank, k.KeywordName
		                                             select k
		                                         ).ToList());

	    public static Func<Feat, string> ToStringMethod { get; set; }
		public override string ToString() => ToStringMethod != null ? ToStringMethod(this) : base.ToString();
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
		public string Range { get; set; }
		[Required]
		public string WeaponCategoryName { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public virtual WeaponCategory WeaponCategory { get; set; }

		[NotMapped]
		public decimal? StaminaCostPerDamageFactor => DamageFactor.HasValue && DamageFactor > 0 ? StaminaCost / DamageFactor : null;

	    [NotMapped]
		public decimal? DamageFactorPerAttackSeconds => AttackSeconds.HasValue && AttackSeconds > 0 ? DamageFactor / AttackSeconds : null;

	    [NotMapped]
		public decimal? NetStaminaCost => StaminaCost - AttackSeconds * 10;
	}

	public abstract class StandardAttack : ActiveFeat
	{
		[Required]
		public decimal? CooldownSeconds { get; set; }
		[Required]
		public string WeaponFormName { get; set; }
		public string SpecificWeaponName { get; set; }

		[ForeignKey("SpecificWeapon_Name")]
		public virtual WeaponType SpecificWeapon { get; set; }

		[NotMapped]
		public decimal? DamageFactorPerCooldownSeconds => CooldownSeconds.HasValue && CooldownSeconds > 0 ? DamageFactor / CooldownSeconds : null;
	}

	public abstract class Attack : StandardAttack { }

	public class Cantrip : Attack
	{
		[Required]
		public string SchoolName { get; set; }
	}

	public class Orison : Attack { }

	public class PhysicalAttack : Attack { }

	public class Utility : StandardAttack { }

	public abstract class PowerAttack : ActiveFeat
	{
		[Required]
		public int? PowerCost { get; set; }
		[Required]
		public int? Level { get; set; }
		[Required]
		public bool? HasEndOfCombatCooldown { get; set; }
		[Required]
		public string AttackBonusName { get; set; }
	}

	public abstract class Expendable : PowerAttack
	{
		[NotMapped]
		public int? ExpCost => Ranks[0].ExpCost;

	    [NotMapped]
		public List<FeatRankAbilityBonus> AbilityBonuses => Ranks[0].AbilityBonuses;

	    [NotMapped]
		public List<FeatRankAbilityRequirement> AbilityRequirements => Ranks[0].AbilityRequirements;
	}

	public class TrophyCharmManeuver : Expendable { }

	public class RogueKitManeuver : Expendable { }

	public class HolySymbolSpell : Expendable { }

	public class SpellbookSpell : Expendable { }

	public class HoldoutWeaponManeuver : Expendable { }

	public class ToolkitManeuver : Expendable { }

	public class Consumable : PowerAttack { }

	public abstract class ChanneledFeat : Feat
	{
		[Required]
		[DisplayName("Channel")]
		public string ChannelName { get; set; }
	}

	public abstract class PassiveFeat : ChanneledFeat { }

	public class Feature : PassiveFeat { }

	public class ArmorFeat : PassiveFeat { }

	public class Defensive : PassiveFeat { }

	public class Reactive : PassiveFeat { }

	public class Upgrade : ChanneledFeat { }

	public class ProficiencyFeat : Feat { }
}
