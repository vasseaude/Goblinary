namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class WikiDataSet : IWikiDataSet
	{
		public bool IsLoaded { get; set; }
		public LookupDataSet lookupDataSet { get; set; }
		public WorkDataSet workDataSet { get; set; }

		public void Import()
		{
			if (!this.lookupDataSet.IsLoaded)
				this.lookupDataSet.XRead();
			if (!this.workDataSet.IsLoaded)
				this.workDataSet.XRead();
			this.Clear();
			this.IsLoaded = false;
			this.ImportEffectTerms();
			this.ImportFeats();
			this.IsLoaded = true;
			this.WriteXml(this.XGetFileName());
		}

		private void ImportEffectTerms()
		{
			string pattern = @"^(?<pre>.*)\[(?<type>.*)\](?<post>.*)$";
			foreach (var source in this.workDataSet.EffectTerms)
			{
				EffectTermsRow effectTerm = this.EffectTerms.NewEffectTermsRow();
				effectTerm.EffectTerm = source.Term;
				effectTerm.EffectType = source.EffectType;
				effectTerm.Description = source.Description;
				if (!source.IsMathSpecificsNull())
					effectTerm.MathSpecifics = source.MathSpecifics;
				if (!source.IsChannelNull())
					effectTerm.Channel = source.Channel;
				this.EffectTerms.AddEffectTermsRow(effectTerm);

				if (Regex.IsMatch(effectTerm.EffectTerm, pattern))
				{
					string type = Regex.Replace(effectTerm.EffectTerm, pattern, "${type}");
					foreach (var lookup in
						from el in this.lookupDataSet.EffectLookups
						where el.Type == type
						select el)
					{
						string effectName = Regex.Replace(effectTerm.EffectTerm, pattern, "${pre}" + lookup.Item + "${post}");
						this.CreateEffectsRow(effectName, effectTerm.EffectTerm);
					}
				}
				else if (effectTerm.EffectTerm.StartsWith("Blast"))
				{
					foreach (var lookup in
						from el in this.lookupDataSet.EffectLookups
						where el.Type == effectTerm.EffectTerm
						select el)
					{
						this.CreateEffectsRow(lookup.Item, effectTerm.EffectTerm);
					}
				}
				else
				{
					this.CreateEffectsRow(effectTerm.EffectTerm, effectTerm.EffectTerm);
				}
			}
		}

		private void CreateEffectsRow(string name, string effectTerm)
		{
			EffectsRow effectsRow = this.Effects.NewEffectsRow();
			effectsRow.Name = name;
			if (effectTerm != null)
				effectsRow.EffectTerm = effectTerm;
			this.Effects.AddEffectsRow(effectsRow);
		}

		private void ImportFeats()
		{
			#region Feat Query

			var featQuery =
				 (
					from ar in this.workDataSet.AdvancementRanks
					from a in this.workDataSet.Advancements
						.Where(x => x.SlotName == ar.SlotName)
						.DefaultIfEmpty()
					from afn in this.lookupDataSet.AltFeatNames
						.Where(x => x.SlotName == a.SlotName)
						.DefaultIfEmpty()
					from fr in this.workDataSet.FeatRanks
						.Where(x => x.FeatName == (afn != null ? afn.FeatName : a.SlotName) && (x.Rank == ar.Rank || x.Rank == "-1"))
						.DefaultIfEmpty()
					orderby afn != null ? afn.FeatName : a.SlotName, int.Parse(ar.Rank)
					select new FeatData
					{
						FeatName = afn != null ? afn.FeatName : a.SlotName,
						FeatType = fr != null && !fr.IsTypeNull() ? fr.Type : !ar.AdvancementsRow.IsTypeNull() ? ar.AdvancementsRow.Type : null,
						Worksheet = a.Worksheet,
						AdvancementRanksRow = ar,
						FeatRanksRow = fr
					}
				).Union(
					from fr in this.workDataSet.FeatRanks
					from afn in this.lookupDataSet.AltFeatNames
						.Where(x => x.FeatName == fr.FeatName)
						.DefaultIfEmpty()
					from a in this.workDataSet.Advancements
						.Where(x => x.SlotName == (afn != null ? afn.SlotName : fr.FeatName))
						.DefaultIfEmpty()
					where a == null
					orderby fr.FeatName, int.Parse(fr.Rank)
					select new FeatData
					{
						FeatName = fr.FeatName,
						FeatType = fr.Type,
						Worksheet = fr.Worksheet,
						FeatRanksRow = fr
					}
				);

			#endregion Feat Query

			FeatsRow feat = null;
			List<string> keywordList = null;
			List<string> priorKeywordList = null;
			Dictionary<string, string> missingFeats = new Dictionary<string, string>();

			foreach (FeatData source in featQuery)
			{
				Console.WriteLine("Building Feat '{0}' Rank {1}.", source.FeatName, source.AdvancementRanksRow != null ? source.AdvancementRanksRow.Rank : source.FeatRanksRow.Rank);
				if (source.FeatType == null)
				{
					missingFeats[source.FeatName] = source.Worksheet;
					continue;
				}
				if (feat == null || feat.Name != source.FeatName)
				{
					keywordList = new List<string>();
					feat = this.CreateFeat(source, keywordList);
					priorKeywordList = new List<string>();
				}
				if (source.AdvancementRanksRow != null)
				{
					FeatRanksRow featRank = this.CreateFeatRank(feat, source);
					this.AddFeatRankKeywords(featRank, source, keywordList, priorKeywordList);
				}
			}
			foreach (string missingFeat in missingFeats.Keys)
			{
				Console.WriteLine("Missing Feat '{0} ({1})'.", missingFeat, missingFeats[missingFeat]);
			}
		}

		private FeatsRow CreateFeat(FeatData source, List<string> keywordList)
		{
			FeatsRow row = this.Feats.NewFeatsRow();
			row.Name = source.FeatName;
			row.Type = source.FeatType;
			row.Role = source.FeatRanksRow != null && !source.FeatRanksRow.IsRoleNull() ? source.FeatRanksRow.Role : "General";
			this.Feats.AddFeatsRow(row);
			FeatData.FeatTypes featType = (FeatData.FeatTypes)Enum.Parse(typeof(FeatData.FeatTypes), source.FeatType.Replace(" ", ""));
			if (FeatData.HasCategory(featType, FeatData.FeatCategories.ActiveFeat))
			{
				row.DamageFactor = decimal.Parse(source.FeatRanksRow.DamageFactor);
				row.AttackSeconds = decimal.Parse(source.FeatRanksRow.AttackSeconds);
				row.StaminaCost = int.Parse(source.FeatRanksRow.StaminaCost);
				row.Range = source.FeatRanksRow.Range;
				row.WeaponCategory = source.FeatRanksRow.WeaponCategory;
				if (!source.FeatRanksRow.IsStandardEffectsNull())
					this.AddFeatEffects(row, "Standard", source.FeatRanksRow.StandardEffects);
				if (!source.FeatRanksRow.IsRestrictionEffectsNull())
					this.AddFeatEffects(row, "Restriction", source.FeatRanksRow.RestrictionEffects);
				if (!source.FeatRanksRow.IsConditionalEffectsNull())
					this.AddFeatEffects(row, "Conditional", source.FeatRanksRow.ConditionalEffects);
				for (int i = 1; i <= 9; i++)
				{
					if (source.FeatRanksRow["Keyword" + i.ToString()] != DBNull.Value)
						keywordList.Add(source.FeatRanksRow["Keyword" + i.ToString()].ToString().Replace(", ", ","));
					else
						break;
				}
				if (FeatData.HasCategory(featType, FeatData.FeatCategories.Attack))
				{
					row.CooldownSeconds = decimal.Parse(source.FeatRanksRow.CooldownSeconds);
					row.WeaponForm = source.FeatRanksRow.Form.Trim();
					if (!source.FeatRanksRow.IsSpecificWeaponNull())
						row.SpecificWeapon = source.FeatRanksRow.SpecificWeapon;
					if (featType == FeatData.FeatTypes.Cantrip)
						row.School = source.FeatRanksRow.School;
				}
				else if (FeatData.HasCategory(featType, FeatData.FeatCategories.PowerAttack))
				{
					row.PowerCost = int.Parse(source.FeatRanksRow.PowerCost);
					row.Level = int.Parse(source.FeatRanksRow.Level);
					row.EndOfCombatCooldown = source.FeatRanksRow.HasCooldown.ToLower() == "yes";
					row.AttackBonus = source.FeatRanksRow.AttackBonus + " Attack Bonus";
				}
			}
			else if (FeatData.HasCategory(featType, FeatData.FeatCategories.PassiveFeat))
			{
				row.Channel = source.FeatRanksRow.Channel;
				if (!source.FeatRanksRow.IsKeywordEffectsNull())
					this.AddFeatEffects(row, "PerKeyword", source.FeatRanksRow.KeywordEffects);
			}
			return row;
		}

		private FeatRanksRow CreateFeatRank(FeatsRow feat, FeatData source)
		{
			FeatRanksRow row = this.FeatRanks.NewFeatRanksRow();
			row.FeatName = feat.Name;
			row.Rank = int.Parse(source.AdvancementRanksRow.Rank);
			row.ExpCost = int.Parse(source.AdvancementRanksRow.ExpCost);
			this.FeatRanks.AddFeatRanksRow(row);
			if (source.FeatRanksRow != null && !source.FeatRanksRow.IsEffectNull())
				this.AddFeatRankEffects(row, source.FeatRanksRow.Effect);
			if (!source.AdvancementRanksRow.IsAbilityBonusNull())
				this.AddFeatRankFacts(row, FactData.FeatRankFactTypes.AbilityBonus, source.AdvancementRanksRow.AbilityBonus);
			if (!source.AdvancementRanksRow.IsAbilityReqNull())
				this.AddFeatRankFacts(row, FactData.FeatRankFactTypes.AbilityRequirement, source.AdvancementRanksRow.AbilityReq);
			// KLUDGE Capitalize Light Blade Expert properly
			if (!source.AdvancementRanksRow.IsAchievementReqNull())
				this.AddFeatRankFacts(row, FactData.FeatRankFactTypes.AchievementRequirement, source.AdvancementRanksRow.AchievementReq.Replace("Light blade Expert", "Light Blade Expert"));
			if (!source.AdvancementRanksRow.IsCategoryReqNull())
				this.AddFeatRankFacts(row, FactData.FeatRankFactTypes.CategoryRequirement, source.AdvancementRanksRow.CategoryReq);
			if (!source.AdvancementRanksRow.IsFeatReqNull())
				this.AddFeatRankFacts(row, FactData.FeatRankFactTypes.FeatRequirement, source.AdvancementRanksRow.FeatReq);
			return row;
		}

		private void AddFeatEffects(FeatsRow feat, string effectType, string effectsText)
		{
			int effectNo = 1;
			foreach (var effects in EffectParser.Parse(effectsText))
			{
				FeatEffectsRow row = this.FeatEffects.NewFeatEffectsRow();
				row.FeatName = feat.Name;
				row.EffectType = effectType;
				row.EffectNo = effectNo++;
				row.EffectDescription = effects["EffectDescription"];
				this.AddEffectDescription(effects);
				this.FeatEffects.AddFeatEffectsRow(row);
			}
		}

		private void AddFeatRankEffects(FeatRanksRow featRank, string effectsText)
		{
			int effectNo = 1;
			foreach (var effects in EffectParser.Parse(effectsText))
			{
				FeatRankEffectsRow row = this.FeatRankEffects.NewFeatRankEffectsRow();
				row.FeatName = featRank.FeatName;
				row.Rank = featRank.Rank;
				row.EffectNo = effectNo++;
				row.EffectDescription = effects["EffectDescription"];
				this.AddEffectDescription(effects);
				this.FeatRankEffects.AddFeatRankEffectsRow(row);
			}
		}

		private void AddEffectDescription(Dictionary<string, string> effects)
		{
			EffectDescriptionsRow row = this.EffectDescriptions.FindByEffectDescription(effects["EffectDescription"]);
			if (row == null)
			{
				row = this.EffectDescriptions.NewEffectDescriptionsRow();
				foreach (string key in effects.Keys)
				{
					row[key] = effects[key];
				}
				this.VerifyEffect(row.EffectName);
				this.EffectDescriptions.AddEffectDescriptionsRow(row);
			}
		}

		private void VerifyEffect(string name)
		{
			var list = (
				from e in this.Effects
				where e.Name == name
				select e).ToList();
			if (list.Count == 0)
			{
				this.CreateEffectsRow(name, null);
			}
		}

		private void AddFeatRankFacts(FeatRanksRow featRank, FactData.FeatRankFactTypes factType, string factsText)
		{
			foreach (FactData source in FactData.GetFacts(factsText))
			{
				FeatRankFactsRow row = this.FeatRankFacts.NewFeatRankFactsRow();
				row.FeatName = featRank.FeatName;
				row.Rank = featRank.Rank;
				row.FactType = factType.ToString();
				row.FactNo = source.FactNo;
				row.OptionNo = source.OptionNo;
				switch (factType)
				{
					case FactData.FeatRankFactTypes.AbilityBonus:
						row.BonusAbility = source.Name;
						row.BonusAbilityValue = decimal.Parse(source.Value);
						break;
					case FactData.FeatRankFactTypes.AbilityRequirement:
						row.RequiredAbility = source.Name;
						row.RequiredAbilityValue = int.Parse(source.Value);
						break;
					case FactData.FeatRankFactTypes.AchievementRequirement:
						row.RequiredAchievement = source.Name;
						row.RequiredAchievementRank = int.Parse(source.Value);
						break;
					case FactData.FeatRankFactTypes.CategoryRequirement:
						row.RequiredCategory = source.Name;
						row.RequiredCategoryValue = int.Parse(source.Value);
						break;
					case FactData.FeatRankFactTypes.FeatRequirement:
						row.RequiredFeat = source.Name;
						row.RequiredFeatRank = int.Parse(source.Value);
						break;
				}
				this.FeatRankFacts.AddFeatRankFactsRow(row);
			}
		}

		private void AddFeatRankKeywords(FeatRanksRow featRank, FeatData source, List<string> keywordList, List<string> priorKeywordList)
		{
			List<string> keywords = null;
			bool addPriorKeywords = false;
			if (keywordList.Count >= featRank.Rank)
				keywords = new List<string>(keywordList[featRank.Rank - 1].Split(','));
			else if (source.FeatRanksRow != null && !source.FeatRanksRow.IsKeywordsNull())
			{
				addPriorKeywords = true;
				keywords = new List<string>(source.FeatRanksRow.Keywords.Replace(", ", ",").Split(','));
				foreach (string priorKeyword in priorKeywordList)
					keywords.Remove(priorKeyword);
			}
			if (keywords != null)
			{
				int keywordNo = 1;
				foreach (string keyword in keywords)
				{
					FeatRankKeywordsRow row = this.FeatRankKeywords.NewFeatRankKeywordsRow();
					row.FeatName = featRank.FeatName;
					row.Rank = featRank.Rank;
					row.KeywordNo = keywordNo++;
					row.Keyword = keyword;
					if (addPriorKeywords)
						priorKeywordList.Add(keyword);
					this.FeatRankKeywords.AddFeatRankKeywordsRow(row);
				}
			}
		}
	}
}
