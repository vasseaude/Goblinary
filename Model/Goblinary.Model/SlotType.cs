namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class SlotType
	{
		public SlotType()
		{
			this.Slots = new List<Slot>();
		}

		[Key]
		public string Name { get; set; }
		public string Parent_Name { get; set; }

		[ForeignKey("Parent_Name")]
		public virtual SlotType Parent { get; set; }

		[InverseProperty("SlotType")]
		public virtual List<Slot> Slots { get; private set; }
	}
}
