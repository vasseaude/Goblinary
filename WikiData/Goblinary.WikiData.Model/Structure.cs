namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public abstract class Structure
	{
		[Key]
		public string Name { get; set; }
		[Required]
		public string BaseTypeName { get; set; }
		[Required]
		public string StructureTypeName { get; set; }
		public string ConstructionData { get; set; }

		[ForeignKey("BaseType_Name, StructureType_Name")]
		public EntityType StructureType { get; set; }

		[NotMapped]
		public virtual Item Kit => null;

	    [NotMapped]
		public string KitName => Kit?.Name;
	}

	public class Camp : Structure
	{
		public Camp()
		{
			Upgrades = new List<CampUpgrade>();
			CampKits = new List<CampKit>();
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
		public string AccountRedeemName { get; set; }

		[InverseProperty("Camp")]
		public virtual List<CampUpgrade> Upgrades { get; set; }
		[InverseProperty("Camp")]
		public virtual List<CampKit> CampKits { get; set; }

		[NotMapped]
		public CampKit CampKit => CampKits.FirstOrDefault();

	    public override Item Kit => CampKit;
	}

	public class CampUpgrade
	{
		[Key, Column(Order = 1)]
		public string StructureName { get; set; }
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
		public string BaseTypeName { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Camp Camp { get; set; }
	}

	public class Holding : Structure
	{
		public Holding()
		{
			Upgrades = new List<HoldingUpgrade>();
			HoldingKits = new List<HoldingKit>();
		}

		[InverseProperty("Holding")]
		public virtual List<HoldingUpgrade> Upgrades { get; set; }
		[InverseProperty("Holding")]
		public virtual List<HoldingKit> HoldingKits { get; set; }

		[NotMapped]
		public HoldingKit HoldingKit => HoldingKits.FirstOrDefault();

	    public override Item Kit => HoldingKit;
	}

	public class HoldingUpgrade
	{
		public HoldingUpgrade()
		{
			TrainerLevels = new List<HoldingUpgradeTrainerLevel>();
			BulkResourceBonuses = new List<HoldingUpgradeBulkResourceBonus>();
			BulkResourceRequirements = new List<HoldingUpgradeBulkResourceRequirement>();
		}

		[Key, Column(Order = 1)]
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Required]
		public int? InfluenceCost { get; set; }
		public string CraftingFacilityFeatName { get; set; }
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
		public virtual List<HoldingUpgradeTrainerLevel> TrainerLevels { get; }
		[InverseProperty("HoldingUpgrade")]
		public virtual List<HoldingUpgradeBulkResourceBonus> BulkResourceBonuses { get; set; }
		[InverseProperty("HoldingUpgrade")]
		public virtual List<HoldingUpgradeBulkResourceRequirement> BulkResourceRequirements { get; set; }
	}

	public class HoldingUpgradeTrainerLevel
	{
		[Key, Column(Order = 1)]
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? TrainerNo { get; set; }
		[Required]
		public string TrainerName { get; set; }
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
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? BonusNo { get; set; }
		[Required]
		public string BulkResourceName { get; set; }
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
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public int? Upgrade { get; set; }
		[Key, Column(Order = 3)]
		public int? RequirementNo { get; set; }
		[Required]
		public string BulkResourceName { get; set; }
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
			WorkerFeats = new List<OutpostWorkerFeat>();
			OutpostKits = new List<OutpostKit>();
			BulkResources = new List<OutpostBulkResource>();
			Upgrades = new List<OutpostUpgrade>();
		}

		[InverseProperty("Outpost")]
		public virtual List<OutpostUpgrade> Upgrades { get; set; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostKit> OutpostKits { get; set; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostBulkResource> BulkResources { get; }
		[InverseProperty("Outpost")]
		public virtual List<OutpostWorkerFeat> WorkerFeats { get; }

		[NotMapped]
		public OutpostKit OutpostKit => OutpostKits.FirstOrDefault();

	    public override Item Kit => OutpostKit;
	}

	public class OutpostBulkResource
	{
		[Key, Column(Order = 1)]
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public string BulkResourceName { get; set; }
		[Key, Column(Order = 3)]
		public string BulkRatingName { get; set; }
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
		public string StructureName { get; set; }
		[Key, Column(Order = 2)]
		public string WorkerFeatName { get; set; }

		[ForeignKey("Structure_Name")]
		public virtual Outpost Outpost { get; set; }
		[ForeignKey("WorkerFeat_Name")]
		public virtual Feat WorkerFeat { get; set; }
	}

	public class OutpostUpgrade
	{
		[Key, Column(Order = 1)]
		public string StructureName { get; set; }
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
