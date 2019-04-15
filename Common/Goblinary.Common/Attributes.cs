namespace Goblinary.Common
{
	using System;


	public enum PresentationTypes
	{
		Property,
		Wrapper,
	}

	[AttributeUsage(AttributeTargets.All)]
	public class PresentationAttribute : Attribute
	{
		public PresentationAttribute(PresentationTypes presentationType)
		{
			PresentationType = presentationType;
			DisplayOrder = int.MaxValue;
		}

		public PresentationAttribute()
			: this(PresentationTypes.Property)
		{
		}

		public PresentationTypes PresentationType { get; }
		public string DisplayName { get; set; }
		public int DisplayOrder { get; set; }
	}
}
