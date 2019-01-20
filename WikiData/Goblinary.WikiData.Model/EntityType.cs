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

	public class EntityType
	{
		public EntityType()
		{
			this.ChildTypes = new List<EntityType>();
			this.ChildMappings = new List<EntityTypeMapping>();
			this.ParentMappings = new List<EntityTypeMapping>();
		}

		[Key, Column(Order = 1)]
		public string BaseType_Name { get; set; }
		[Key, Column(Order = 2)]
		public string Name { get; set; }
		public string ParentType_Name { get; set; }
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
		public string BaseType_Name { get; set; }
		[Key, Column(Order = 2)]
		public string ParentType_Name { get; set; }
		[Key, Column(Order = 3)]
		public string ChildType_Name { get; set; }

		[ForeignKey("BaseType_Name, ParentType_Name")]
		public virtual EntityType ParentType { get; set; }
		[ForeignKey("BaseType_Name, ChildType_Name")]
		public virtual EntityType ChildType { get; set; }
	}
}
