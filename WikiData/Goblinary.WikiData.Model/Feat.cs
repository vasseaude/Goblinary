namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public abstract class Feat
	{
		public Feat()
		{
			this.Ranks = new EntityList<FeatRank>();
			this.Effects = new EntityList<FeatEffect>();
			this.AchievementRanks = new EntityList<CraftAchievementRank>();
			this.HoldingUpgrades = new EntityList<HoldingUpgrade>();
			this.OutpostWorkerFeats = new EntityList<OutpostWorkerFeat>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseType_Name { get; set; }
		[Required]
		public string FeatType_Name { get; set; }
		[Required]
		[DisplayName("Role")]
		public string Role_Name { get; set; }
		[DisplayName("Advancement Feat")]
		public string AdvancementFeat_Name { get; set; }

		[ForeignKey("BaseType_Name, FeatType_Name")]
		public virtual EntityType FeatType { get; set; }
		[ForeignKey("Role_Name")]
		public virtual Role Role { get; set; }
		[ForeignKey("AdvancementFeat_Name")]
		public virtual AdvancementFeat AdvancementFeat { get; set; }

		[InverseProperty("Feat")]
		public virtual EntityList<FeatRank> Ranks { get; private set; }
		[InverseProperty("Feat")]
		public virtual EntityList<FeatEffect> Effects { get; private set; }
		[InverseProperty("Feat")]
		public virtual EntityList<CraftAchievementRank> AchievementRanks { get; private set; }
		[InverseProperty("CraftingFacilityFeat")]
		public virtual EntityList<HoldingUpgrade> HoldingUpgrades { get; private set; }
		[InverseProperty("WorkerFeat")]
		public virtual EntityList<OutpostWorkerFeat> OutpostWorkerFeats { get; private set; }

		private List<FeatRankKeyword> keywords;
		[NotMapped]
		public List<FeatRankKeyword> Keywords
		{
			get
			{
				if (this.keywords == null)
				{
					this.keywords = (
							from r in this.Ranks
							from k in r.Keywords
							orderby k.Feat_Rank, k.Keyword_Name
							select k
						).ToList();
				}
				return this.keywords;
			}
		}

		public static Func<Feat, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
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
		public string WeaponCategory_Name { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public virtual WeaponCategory WeaponCategory { get; set; }

		[NotMapped]
		public decimal? StaminaCostPerDamageFactor
		{
			get
			{
				return this.DamageFactor.HasValue && this.DamageFactor > 0 ? this.StaminaCost / this.DamageFactor : null;
			}
		}

		[NotMapped]
		public decimal? DamageFactorPerAttackSeconds
		{
			get
			{
				return this.AttackSeconds.HasValue && this.AttackSeconds > 0 ? this.DamageFactor / this.AttackSeconds : null;
			}
		}

		[NotMapped]
		public decimal? NetStaminaCost
		{
			get
			{
				return this.StaminaCost - this.AttackSeconds * 10;
			}
		}
	}

	public abstract class StandardAttack : ActiveFeat
	{
		[Required]
		public decimal? CooldownSeconds { get; set; }
		[Required]
		public string WeaponForm_Name { get; set; }
		public string SpecificWeapon_Name { get; set; }

		[ForeignKey("SpecificWeapon_Name")]
		public virtual WeaponType SpecificWeapon { get; set; }

		[NotMapped]
		public decimal? DamageFactorPerCooldownSeconds
		{
			get
			{
				return this.CooldownSeconds.HasValue && this.CooldownSeconds > 0 ? this.DamageFactor / this.CooldownSeconds : null;
			}
		}
	}

	public abstract class Attack : StandardAttack { }

	public class Cantrip : Attack
	{
		[Required]
		public string School_Name { get; set; }
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
		public string AttackBonus_Name { get; set; }
	}

	public abstract class Expendable : PowerAttack
	{
		[NotMapped]
		public int? ExpCost
		{
			get
			{
				return this.Ranks[0].ExpCost;
			}
		}

		[NotMapped]
		public List<FeatRankAbilityBonus> AbilityBonuses
		{
			get
			{
				return this.Ranks[0].AbilityBonuses;
			}
		}

		[NotMapped]
		public List<FeatRankAbilityRequirement> AbilityRequirements
		{
			get
			{
				return this.Ranks[0].AbilityRequirements;
			}
		}
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
		public string Channel_Name { get; set; }
	}

	public abstract class PassiveFeat : ChanneledFeat { }

	public class Feature : PassiveFeat { }

	public class ArmorFeat : PassiveFeat { }

	public class Defensive : PassiveFeat { }

	public class Reactive : PassiveFeat { }

	public class Upgrade : ChanneledFeat { }

	public class ProficiencyFeat : Feat { }
}
