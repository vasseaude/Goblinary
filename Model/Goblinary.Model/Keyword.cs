namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Keyword
	{
		[Key, Column(Order = 1)]
		public string KeywordType_Name { get; set; }
		[Key, Column(Order = 2)]
		public string Name { get; set; }

		[ForeignKey("KeywordType_Name")]
		public virtual KeywordType KeywordType { get; set; }
	}
}
