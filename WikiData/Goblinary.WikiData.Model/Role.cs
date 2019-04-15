namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Role
	{
		public Role()
		{
			Feats = new List<Feat>();
			Armors = new List<Armor>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Role")]
		public List<Feat> Feats { get; }
		[InverseProperty("MainRole")]
		public List<Armor> Armors { get; }

		public static Func<Role, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
