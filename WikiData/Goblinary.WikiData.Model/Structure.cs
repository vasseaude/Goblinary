namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public abstract class Structure
	{
		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseType_Name { get; set; }
		[Required]
		public string StructureType_Name { get; set; }
		public string ConstructionData { get; set; }

		[ForeignKey("BaseType_Name, StructureType_Name")]
		public EntityType StructureType { get; set; }

		[NotMapped]
		public virtual Item Kit { get { return null; } }
		[NotMapped]
		public string KitName { get { return this.Kit != null ? this.Kit.Name : null; } }
	}

	public class Camp : Structure
	{
		public Camp()
		{
			this.Upgrades = new List<CampUpgrade>();
			this.CampKits = new List<CampKit>();
		}

		[Required]
		public string Description { get; set; }
		[Required]
		public string DisplayName { get; set; }
		[Required]
		public decimal? Encumbrance { get; set; }
		[Required]
		public int? Quality { get; set; }
		[Required]
		public int? Tier { get; set; }
		[Required]
		public string Category { get; set; }
		[Required]
		public string HousingData { get; set; }
		[Required]
		public string HouseEntityDefn { get; set; }
		[Required]
		public int? Cooldown { get; set; }
		[Required]
		public bool? NoLoot { get; set; }
		[Required]
		public bool? Upgradable { get; set; }
		public string AccountRedeem_Name { get; set; }

		[InverseProperty("Camp")]
		public virtual List<CampUpgrade> Upgrades { get; set; }
		[InverseProperty("Camp")]
		public virtual List<CampKit> CampKits { get; set; }

		[NotMapped]
		public CampKit CampKit { get { return this.CampKits.FirstOrDefault(); } }
		public override Item Kit { get { return this.CampKit; } }
	}

	public class CampUpgrade
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Required]
		public int? PowerChannelDurationSeconds { get; set; }
		[Required]
		public int? PowerRegenerationSeconds { get; set; }
		[Required]
		public int? PowerCooldownMinutes { get; set; }
		[Required]
		public int? BuildingDurationMinutes { get; set; }
		[Required]
		public string ConstructionData { get; set; }
		[Required]
		public string BaseType_Name { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Camp Camp { get; set; }
	}

	public class Holding : Structure
	{
		public Holding()
		{
			this.Upgrades = new List<HoldingUpgrade>();
			this.HoldingKits = new List<HoldingKit>();
		}

		[InverseProperty("Holding")]
		public virtual List<HoldingUpgrade> Upgrades { get; set; }
		[InverseProperty("Holding")]
		public virtual List<HoldingKit> HoldingKits { get; set; }

		[NotMapped]
		public HoldingKit HoldingKit { get { return this.HoldingKits.FirstOrDefault(); } }
		public override Item Kit { get { return this.HoldingKit; } }
	}

	public class HoldingUpgrade
	{
		public HoldingUpgrade()
		{
			this.TrainerLevels = new List<HoldingUpgradeTrainerLevel>();
			this.BulkResourceBonuses = new List<HoldingUpgradeBulkResourceBonus>();
			this.BulkResourceRequirements = new List<HoldingUpgradeBulkResourceRequirement>();
		}

		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Required]
		public int? InfluenceCost { get; set; }
		public string CraftingFacilityFeat_Name { get; set; }
		public int? CraftingFacilityQuality { get; set; }
		[Required]
		public int? PvPPeakGuards { get; set; }
		[Required]
		public int? NonPvPPeakGuards { get; set; }
		[Required]
		public int? GuardSurgeSize { get; set; }
		[Required]
		public decimal? GuardRespawnsPerMinute { get; set; }
		[Required]
		public int? PvPGuardRespawns { get; set; }
		[Required]
		public int? NonPvPGuardRespawns { get; set; }
		[Required]
		public int? NonPvPRespawnFill { get; set; }
		[Required]
		public decimal? MinPvPTime { get; set; }
		[Required]
		public string GuardEntityNames { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Holding Holding { get; set; }
		[ForeignKey("CraftingFacilityFeat_Name")]
		public virtual Feat CraftingFacilityFeat { get; set; }

		[InverseProperty("HoldingUpgrade")]
		public virtual List<HoldingUpgradeTrainerLevel> TrainerLevels { get; private set; }
		[InverseProperty("HoldingUpgrade")]
		public virtual List<HoldingUpgradeBulkResourceBonus> BulkResourceBonuses { get; set; }
		[InverseProperty("HoldingUpgrade")]
		public virtual List<HoldingUpgradeBulkResourceRequirement> BulkResourceRequirements { get; set; }
	}

	public class HoldingUpgradeTrainerLevel
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? TrainerNo { get; set; }
		[Required]
		public string Trainer_Name { get; set; }
		[Required]
		public int? Level { get; set; }

		[ForeignKey("Structure_Name, Upgrade")]
		public virtual HoldingUpgrade HoldingUpgrade { get; set; }
		[ForeignKey("Trainer_Name")]
		public virtual Trainer Trainer { get; set; }
	}

	[Table("HoldingUpgradeBulkResourceBonuses")]
	public class HoldingUpgradeBulkResourceBonus
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? BonusNo { get; set; }
		[Required]
		public string BulkResource_Name { get; set; }
		[Required]
		public int? Bonus { get; set; }

		[ForeignKey("Structure_Name, Upgrade")]
		public virtual HoldingUpgrade HoldingUpgrade { get; set; }
		[ForeignKey("BulkResource_Name")]
		public virtual BulkResource BulkResource { get; set; }
	}

	public class HoldingUpgradeBulkResourceRequirement
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Required]
		public string BulkResource_Name { get; set; }
		[Required]
		public int? Requirement { get; set; }

		[ForeignKey("Structure_Name, Upgrade")]
		public virtual HoldingUpgrade HoldingUpgrade { get; set; }
		[ForeignKey("BulkResource_Name")]
		public virtual BulkResource BulkResource { get; set; }
	}

	public class Outpost : Structure
	{
		public Outpost()
		{
			this.WorkerFeats = new List<OutpostWorkerFeat>();
			this.OutpostKits = new List<OutpostKit>();
			this.BulkResources = new List<OutpostBulkResource>();
			this.Upgrades = new List<OutpostUpgrade>();
		}

		[InverseProperty("Outpost")]
		public virtual List<OutpostUpgrade> Upgrades { get; set; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostKit> OutpostKits { get; set; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostBulkResource> BulkResources { get; private set; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostWorkerFeat> WorkerFeats { get; private set; }

		[NotMapped]
		public OutpostKit OutpostKit { get { return this.OutpostKits.FirstOrDefault(); } }
		public override Item Kit { get { return this.OutpostKit; } }
	}

	public class OutpostBulkResource
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public string BulkResource_Name { get; set; }
		[Key, Column(Order = 3)]
		public string BulkRating_Name { get; set; }
		[Required]
		public int? Percentage { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Outpost Outpost { get; set; }
		[ForeignKey("BulkResource_Name")]
		public virtual BulkResource BulkResource { get; set; }
		[ForeignKey("BulkRating_Name")]
		public virtual BulkRating BulkRating { get; set; }
	}

	public class OutpostWorkerFeat
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public string WorkerFeat_Name { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Outpost Outpost { get; set; }
		[ForeignKey("WorkerFeat_Name")]
		public virtual Feat WorkerFeat { get; set; }
	}

	public class OutpostUpgrade
	{
		[Key, Column(Order = 1)]
		public string Structure_Name { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Required]
		public int? EffortBonus { get; set; }
		[Required]
		public int? InfluenceCost { get; set; }
		[Required]
		public int? PvPPeakGuards { get; set; }
		[Required]
		public int? GuardSurgeSize { get; set; }
		[Required]
		public decimal? GuardRespawnsPerMinute { get; set; }
		[Required]
		public int? PvPGuardRespawns { get; set; }
		[Required]
		public decimal? MinPvPTime { get; set; }
		[Required]
		public string GuardEntityNames { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Outpost Outpost { get; set; }
	}
}
