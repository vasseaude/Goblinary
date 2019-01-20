namespace Goblinary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Slot
	{
		[Key]
		public string Name { get; set; }
		[Required]
		public string SlotType_Name { get; set; }
		[Required]
		public bool? CanBeSlotted { get; set; }

		[ForeignKey("SlotType_Name")]
		public virtual SlotType SlotType { get; set; }
	}
}
