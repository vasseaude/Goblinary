namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Common;

	public class WeaponCategory
	{
		public WeaponCategory()
		{
			this.GearTypes = new List<GearType>();
			this.WeaponTypes = new List<WeaponType>();
			this.ActiveFeats = new List<ActiveFeat>();
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
			this.GearTypes = new List<GearType>();
			this.WeaponTypes = new List<WeaponType>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("AttackBonus")]
		public List<GearType> GearTypes { get; set; }
		[InverseProperty("AttackBonus")]
		public List<WeaponType> WeaponTypes { get; set; }
	}
}
