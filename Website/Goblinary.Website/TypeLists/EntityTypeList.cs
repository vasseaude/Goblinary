namespace Goblinary.Website.TypeLists
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;

	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public class EntityTypeList
	{
		private static Dictionary<string, List<EntityTypeNode>> nodes = new Dictionary<string,List<EntityTypeNode>>();

		private static List<EntityTypeNode> Select(string baseType)
		{
			if (!EntityTypeList.nodes.ContainsKey(baseType))
			{
				EntityTypeList.nodes[baseType] = new List<EntityTypeNode>();
				using (WikiDataContext context = new WikiDataContext())
				{
					var entityType = (
							from et in context.Set<EntityType>()
							where et.BaseType_Name == baseType && et.ParentType_Name == null
							select et
						).FirstOrDefault();
					new EntityTypeNode(EntityTypeList.nodes[baseType], entityType, null);
				}
			}
			return EntityTypeList.nodes[baseType];
		}

		public static List<EntityTypeNode> SelectFeatTypes()
		{
			return EntityTypeList.Select("Feat");
		}

		public static List<EntityTypeNode> SelectAchievementTypes()
		{
			return EntityTypeList.Select("Achievement");
		}

		public static List<EntityTypeNode> SelectItemTypes()
		{
			return EntityTypeList.Select("Item");
		}

		public static List<EntityTypeNode> SelectRecipeTypes()
		{
			return EntityTypeList.Select("Recipe");
		}

		public static List<EntityTypeNode> SelectStructureTypes()
		{
			return EntityTypeList.Select("Structure");
		}
	}

	public class EntityTypeNode
	{
		public EntityTypeNode(List<EntityTypeNode> nodeList, EntityType entityType, EntityTypeNode parentNode)
		{
			this.nodeList = nodeList;
			this.entityType = entityType;
			this.parentNode = parentNode;
			this.nodeList.Add(this);

			foreach (EntityType childType in
				from ct in this.entityType.ChildTypes
				orderby ct.Name
				select ct)
			{
				new EntityTypeNode(this.nodeList, childType, this);
			}
		}

		private List<EntityTypeNode> nodeList;
		private EntityType entityType;
		private EntityTypeNode parentNode;

		private int Depth
		{
			get
			{
				return this.parentNode == null ? 0 : (this.parentNode.Depth + 1);
			}
		}

		public string Name
		{
			get
			{
				return this.entityType.Name;
			}
		}

		public string DisplayName
		{
			get
			{
				return string.Format("{0}{1}{2}",
					string.Concat(Enumerable.Repeat("\u00A0\u00A0\u00A0", this.Depth > 0 ? this.Depth - 1 : this.Depth)),
					this.Depth > 0 ? "\u21B3 " : "",
					this.entityType.DisplayName);
			}
		}
	}
}