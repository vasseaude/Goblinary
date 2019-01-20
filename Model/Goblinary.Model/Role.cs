namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Role
	{
		public Role()
		{
		}

		[Key]
		public string Name { get; set; }
	}
}
