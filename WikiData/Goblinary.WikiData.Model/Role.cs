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

	using Goblinary.Common;

	public class Role
	{
		public Role()
		{
			this.Feats = new List<Feat>();
			this.Armors = new List<Armor>();
		}

		[Key]
		public string Name { get; set; }

		[InverseProperty("Role")]
		public List<Feat> Feats { get; private set; }
		[InverseProperty("MainRole")]
		public List<Armor> Armors { get; private set; }

		public static Func<Role, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}
}
