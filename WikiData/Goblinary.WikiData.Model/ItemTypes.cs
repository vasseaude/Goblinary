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

	public class GearType
	{
		public GearType()
		{
			this.Gear = new List<Gear>();
		}

		[Key]
		public string Name { get; set; }
		public string WeaponCategory_Name { get; set; }
		public string AttackBonus_Name { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public virtual WeaponCategory WeaponCategory { get; set; }
		[ForeignKey("AttackBonus_Name")]
		public virtual AttackBonus AttackBonus { get; set; }

		[InverseProperty("GearType")]
		public virtual List<Gear> Gear { get; set; }
	}

	public class WeaponType
	{
		public WeaponType()
		{
			this.Weapons = new List<Weapon>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string WeaponCategory_Name { get; set; }
		[Required]
		public string AttackBonus_Name { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public virtual WeaponCategory WeaponCategory { get; set; }
		[ForeignKey("AttackBonus_Name")]
		public virtual AttackBonus AttackBonus { get; set; }

		[InverseProperty("WeaponType")]
		public virtual List<Weapon> Weapons { get; set; }
	}
}
