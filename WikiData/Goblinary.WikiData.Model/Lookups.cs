namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class WeaponCategory
	{
		public WeaponCategory()
		{
			GearTypes = new List<GearType>();
			WeaponTypes = new List<WeaponType>();
			ActiveFeats = new List<ActiveFeat>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("WeaponCategory")]
		public List<GearType> GearTypes { get; set; }
		[InverseProperty("WeaponCategory")]
		public List<WeaponType> WeaponTypes { get; set; }
		[InverseProperty("WeaponCategory")]
		public List<ActiveFeat> ActiveFeats { get; set; }
	}

	[Table("AttackBonuses")]
	public class AttackBonus
	{
		public AttackBonus()
		{
			GearTypes = new List<GearType>();
			WeaponTypes = new List<WeaponType>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("AttackBonus")]
		public List<GearType> GearTypes { get; set; }
		[InverseProperty("AttackBonus")]
		public List<WeaponType> WeaponTypes { get; set; }
	}
}
