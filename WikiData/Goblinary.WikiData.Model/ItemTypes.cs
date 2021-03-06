﻿namespace Goblinary.WikiData.Model
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class GearType
	{
		public GearType()
		{
			Gear = new List<Gear>();
		}

		[Key]
		public string Name { get; set; }
		public string WeaponCategoryName { get; set; }
		public string AttackBonusName { get; set; }

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
			Weapons = new List<Weapon>();
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string WeaponCategoryName { get; set; }
		[Required]
		public string AttackBonusName { get; set; }

		[ForeignKey("WeaponCategory_Name")]
		public virtual WeaponCategory WeaponCategory { get; set; }
		[ForeignKey("AttackBonus_Name")]
		public virtual AttackBonus AttackBonus { get; set; }

		[InverseProperty("WeaponType")]
		public virtual List<Weapon> Weapons { get; set; }
	}
}
