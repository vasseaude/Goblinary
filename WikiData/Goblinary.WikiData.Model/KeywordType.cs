namespace Goblinary.WikiData.Model
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Reflection;

	public class KeywordType
	{
		public static Type GetType<T>(string typeName)
		{
			var assembly = Assembly.GetAssembly(typeof(T));
			var type = assembly.GetType(string.Format("{0}.{1}", typeof(T).Namespace, typeName));
			if (type == null)
			{
				throw new Exception(string.Format("Type not found: {0}", typeName));
			}
			return type;
		}

		[Key]
		public string Name { get; set; }
		[Required]
		public string SourceFeatTypeName { get; set; }
		public string MatchingFeatTypeName { get; set; }
		public string MatchingItemTypeName { get; set; }

		private Type _sourceFeatType;
		[NotMapped]
		public Type SourceFeatType
		{
			get
			{
			    if (_sourceFeatType != null || SourceFeatTypeName == null) return _sourceFeatType;
			    _sourceFeatType = GetType<Feat>(SourceFeatTypeName);
			    return _sourceFeatType;
			}
		}

		private Type _matchingFeatType;
		[NotMapped]
		public Type MatchingFeatType
		{
			get
			{
			    if (_matchingFeatType != null || MatchingFeatTypeName == null) return _matchingFeatType;
			    _matchingFeatType = GetType<Feat>(MatchingFeatTypeName);
			    return _matchingFeatType;
			}
		}

		private Type _matchingItemType;
		[NotMapped]
		public Type MatchingItemType
		{
			get
			{
			    if (_matchingItemType != null || MatchingItemTypeName == null) return _matchingItemType;
			    _matchingItemType = GetType<Feat>(MatchingItemTypeName);
			    return _matchingItemType;
			}
		}
	}
}
