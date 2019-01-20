namespace Goblinary.Common
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public enum PresentationTypes
	{
		Property,
		Wrapper,
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class PresentationAttribute : Attribute
	{
		public PresentationAttribute(PresentationTypes presentationType)
		{
			this.PresentationType = presentationType;
			this.DisplayOrder = int.MaxValue;
		}

		public PresentationAttribute()
			: this(PresentationTypes.Property)
		{
		}

		public PresentationTypes PresentationType { get; private set; }
		public string DisplayName { get; set; }
		public int DisplayOrder { get; set; }
	}
}
