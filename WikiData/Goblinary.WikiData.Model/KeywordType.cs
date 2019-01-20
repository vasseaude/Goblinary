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

	public class KeywordType
	{
		public static Type GetType<T>(string typeName)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(T));
			Type type = assembly.GetType(string.Format("{0}.{1}", typeof(T).Namespace, typeName));
			if (type == null)
			{
				throw new Exception(string.Format("Type not found: {0}", typeName));
			}
			return type;
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string SourceFeatType_Name { get; set; }
		public string MatchingFeatType_Name { get; set; }
		public string MatchingItemType_Name { get; set; }

		private Type sourceFeatType;
		[NotMapped]
		public Type SourceFeatType
		{
			get
			{
				if (this.sourceFeatType == null && this.SourceFeatType_Name != null)
				{
					this.sourceFeatType = KeywordType.GetType<Feat>(this.SourceFeatType_Name);
				}
				return this.sourceFeatType;
			}
		}

		private Type matchingFeatType;
		[NotMapped]
		public Type MatchingFeatType
		{
			get
			{
				if (this.matchingFeatType == null && this.MatchingFeatType_Name != null)
				{
					this.matchingFeatType = KeywordType.GetType<Feat>(this.MatchingFeatType_Name);
				}
				return this.matchingFeatType;
			}
		}

		private Type matchingItemType;
		[NotMapped]
		public Type MatchingItemType
		{
			get
			{
				if (this.matchingItemType == null && this.MatchingItemType_Name != null)
				{
					this.matchingItemType = KeywordType.GetType<Feat>(this.MatchingItemType_Name);
				}
				return this.matchingItemType;
			}
		}
	}
}
