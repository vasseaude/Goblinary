namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IEffectReference
	{
		int? EffectNo { get; set; }
		string EffectDescription_Text { get; set; }
	}
}
