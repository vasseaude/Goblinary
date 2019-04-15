namespace Goblinary.WikiData.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class EntityType
	{
		public EntityType()
		{
			ChildTypes = new List<EntityType>();
			ChildMappings = new List<EntityTypeMapping>();
			ParentMappings = new List<EntityTypeMapping>();
		}

		[Key, Column(Order = 1)]
		public string BaseTypeName { get; set; }
		[Key, Column(Order = 2)]
		public string Name { get; set; }
		public string ParentTypeName { get; set; }
		[Required]
		public string Modifier { get; set; }
		[Required]
		public string DisplayName { get; set; }

		[ForeignKey("BaseType_Name, ParentType_Name")]
		public virtual EntityType ParentType { get; set; }

		[InverseProperty("ParentType")]
		public virtual List<EntityType> ChildTypes { get; private set; }
		[InverseProperty("ParentType")]
		public virtual List<EntityTypeMapping> ChildMappings { get; private set; }
		[InverseProperty("ChildType")]
		public virtual List<EntityTypeMapping> ParentMappings { get; private set; }

		public static Func<EntityType, string> ToStringMethod { get; set; }
		public override string ToString()
		{
			return ToStringMethod != null ? ToStringMethod(this) : base.ToString();
		}
	}

	public class EntityTypeMapping
	{
		[Key, Column(Order = 1)]
		public string BaseTypeName { get; set; }
		[Key, Column(Order = 2)]
		public string ParentTypeName { get; set; }
		[Key, Column(Order = 3)]
		public string ChildTypeName { get; set; }

		[ForeignKey("BaseType_Name, ParentType_Name")]
		public virtual EntityType ParentType { get; set; }
		[ForeignKey("BaseType_Name, ChildType_Name")]
		public virtual EntityType ChildType { get; set; }
	}
}
