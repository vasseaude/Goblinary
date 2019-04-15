namespace Goblinary.WikiData
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using Common;
    using WikiData;
    using Model;
    using SqlServer;
    using System.Data.Entity.Validation;

    public class ModelBuilder
	{
		private class FactData
		{
			public static List<FactData> GetFacts(string factsText)
			{
				var facts = new List<FactData>();
				var factNo = 1;
				foreach (var fact in factsText.Replace("personality", "Personality").Replace(" or ", "|").Replace(", ", ",").Split(','))
					if (fact != "")
					{
						var optionNo = 1;
					    facts.AddRange(from option in fact.Split('|') where option != "" select option.Split('=') into parts select new FactData(factNo, optionNo++, parts[0].Trim(), parts[1].Trim()));
					    factNo++;
					}
				return facts;
			}

			public static List<FactData> GetFacts(WorkDataSet.AchievementRanksRow source, IEnumerable<string> categories)
			{
			    var factNo = 1;
				var optionNo = 1;
			    return (from category in categories where source[category].ToString() != "" select new FactData(factNo++, optionNo, category, source[category].ToString())).ToList();
			}

			private FactData() { }

			private FactData(int factNo, int optionNo, string name, string value)
			{
				FactNo = factNo;
				OptionNo = optionNo;
				Name = name;
				Value = value;
			}

			public int FactNo { get; private set; }
			public int OptionNo { get; private set; }
			public string Name { get; private set; }
			public string Value { get; private set; }
		}

		public ModelBuilder(LookupDataSet lookupDataSet, WorkDataSet workDataSet)
		{
			this.lookupDataSet = lookupDataSet;
			this.workDataSet = workDataSet;
			assembly = Assembly.GetAssembly(typeof(Feat));
			categories = new List<string> { "Adventure", "Arcane", "Crafting", "Divine", "Martial", "Social", "Subterfuge" };
		}

		private LookupDataSet lookupDataSet;
		private WorkDataSet workDataSet;
		private Assembly assembly;
		private List<string> categories;
		private Dictionary<string, EffectDescription> effectDescriptions = new Dictionary<string, EffectDescription>();
		private Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
		private Dictionary<Type, Dictionary<string, string>> factTypes = new Dictionary<Type, Dictionary<string, string>>();

		private Dictionary<Type, object> EntityLists = new Dictionary<Type, object>();

		public void Build()
		{
			BuildLookups();
			BuildEntityTypes();
			BuildRoles();
			BuildKeywordTypes();
			BuildItemTypes();
			BuildStocks();
			BuildEffects();
			BuildConditions();
			BuildKeywords();
			BuildFeats();
			BuildAchievements();
			BuildItems();
			BuildRecipes();
			BuildFeatRankTrainerLevels();
			BuildHexes();
			BuildStructures();

			ValidateForeignKeys();

			SaveData<BulkRating>();
			SaveData<BulkResource>();
			SaveData<AdvancementFeat>();
			SaveData<EntityType>();
			SaveData<EntityTypeMapping>();
			SaveData<Ability>();
			SaveData<Role>();
			SaveData<EffectTerm>();
			SaveData<Effect>();
			SaveData<Condition>();
			SaveData<EffectDescription>();
			SaveData<KeywordType>();
			SaveData<Keyword>();
			SaveData<WeaponCategory>();
			SaveData<AttackBonus>();
			SaveData<WeaponType>();
			SaveData<GearType>();
			SaveData<Feat>();
			SaveData<AchievementGroup>();
			SaveData<Achievement>();
			SaveData<FeatEffect>();
			SaveData<FeatRank>();
			SaveData<Trainer>();
			SaveData<FeatRankTrainerLevel>();
			SaveData<AchievementRank>();
			SaveData<FeatRankEffect>();
			SaveData<FeatRankKeyword>();
			SaveData<FeatRankAbilityBonus>();
			SaveData<FeatRankAbilityRequirement>();
			SaveData<FeatRankAchievementRequirement>();
			SaveData<FeatRankCategoryRequirement>();
			SaveData<FeatRankFeatRequirement>();
			SaveData<AchievementRankCategoryBonus>();
			SaveData<AchievementRankFeatRequirement>();
			SaveData<AchievementRankFlagRequirement>();
			SaveData<Stock>();
			SaveData<Camp>();
			SaveData<Holding>();
			SaveData<Outpost>();
			SaveData<CampUpgrade>();
			SaveData<Item>();
			SaveData<StockItemStock>();
			SaveData<RecipeOutputItem>();
			SaveData<RecipeOutputItemUpgrade>();
			SaveData<RecipeOutputItemUpgradeKeyword>();
			SaveData<Recipe>();
			SaveData<RecipeIngredient>();
			SaveData<Hex>();
			SaveData<HexBulkRating>();
			SaveData<HoldingUpgrade>();
			SaveData<HoldingUpgradeBulkResourceBonus>();
			SaveData<HoldingUpgradeBulkResourceRequirement>();
			SaveData<HoldingUpgradeTrainerLevel>();
			SaveData<OutpostUpgrade>();
			SaveData<OutpostBulkResource>();
			SaveData<OutpostWorkerFeat>();
		}

		private void ValidateForeignKeys()
		{
			foreach (var req in (
					from frar in GetEntityList<FeatRankAchievementRequirement>()
					from a in GetEntityList<Achievement>()
					where string.Equals(a.Name, frar.AchievementName, StringComparison.CurrentCultureIgnoreCase)
						&& a.Name != frar.AchievementName
					select new
					{
						Req = frar,
					    a.Name
					}
				))
			{
				req.Req.AchievementName = req.Name;
			}
			foreach (var req in (
					from frfr in GetEntityList<FeatRankFeatRequirement>()
					from f in GetEntityList<Feat>()
					where string.Equals(f.Name, frfr.RequiredFeat_Name, StringComparison.CurrentCultureIgnoreCase)
						&& f.Name != frfr.RequiredFeat_Name
					select new
					{
						Req = frfr,
					    f.Name
					}
				))
			{
				req.Req.RequiredFeat_Name = req.Name;
			}
		}

		private void SaveData<T>() where T : class
		{
			Console.WriteLine("Saving {0} data", typeof(T).Name);
			using (var context = new WikiDataContext())
			{
				var list = GetEntityList<T>();
				context.Set<T>().AddRange(list);
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						context.SaveChanges();
						transaction.Commit();
					}
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    catch (Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		#region Build Methods

		private void BuildLookups()
		{
			var abilityList = GetEntityList<Ability>();
			var attackBonusList = GetEntityList<AttackBonus>();
			var weaponCategoryList = GetEntityList<WeaponCategory>();
			var bulkRatings = GetEntityList<BulkRating>();
			var bulkResources = GetEntityList<BulkResource>();
			var conditions = GetEntityList<Condition>();

			foreach (var lookup in lookupDataSet.Lookups)
			{
				switch (lookup.Type)
				{
					case "Ability":
						abilityList.Add(new Ability { Name = lookup.Name });
						break;
					case "AttackBonus":
						attackBonusList.Add(new AttackBonus { Name = lookup.Name });
						break;
					case "WeaponCategory":
						weaponCategoryList.Add(new WeaponCategory { Name = lookup.Name });
						break;
					case "BulkRating":
						bulkRatings.Add(new BulkRating { Name = lookup.Name });
						break;
					case "BulkResource":
						bulkResources.Add(new BulkResource { Name = lookup.Name });
						break;
					case "Condition":
						conditions.Add(new Condition { Name = lookup.Name });
						break;
                    case "DevelopmentIndex":
                        //TODO add this
                        break;
                    default:
						throw new Exception(string.Format("Unexpected Lookup Type: {0}", lookup.Type));
				}
			}
		}

		private void BuildEntityTypes()
		{
			var entityTypes = GetEntityList<EntityType>();
			var mappings = GetEntityList<EntityTypeMapping>();

			foreach (var entityType in lookupDataSet.EntityTypes)
			{
				entityTypes.Add(
					new EntityType
					{
						BaseTypeName = entityType.BaseType,
						Name = entityType.EntityType,
						ParentTypeName = entityType.IsParentTypeNull() ? null : entityType.ParentType,
						Modifier = entityType.Modifier,
						DisplayName = entityType.DisplayName
					});
			    if (entityType.Modifier != "Final") continue;
			    var parent = entityType.EntityType;
			    while (parent != null)
			    {
			        mappings.Add(new EntityTypeMapping
			        {
			            BaseTypeName = entityType.BaseType,
			            ChildTypeName = entityType.EntityType,
			            ParentTypeName = parent
			        });
			        parent = (
			            from ft in lookupDataSet.EntityTypes
			            where ft.EntityType == parent
			            select ft.IsParentTypeNull() ? null : ft.ParentType).First();
			    }
			}
		}

		private void BuildRoles()
		{
			var roles = GetEntityList<Role>();
		    roles.AddRange((from fr in workDataSet.FeatRanks where !fr.IsRoleNull() select fr.Role).Union(from i in workDataSet.Items where !i.IsMainRoleNull() select i.MainRole)
		        .Distinct()
		        .Select(role => new Role
		        {
		            Name = role
		        }));
		}

		private void BuildKeywordTypes()
		{
			var keywordTypeList = GetEntityList<KeywordType>();
		    keywordTypeList.AddRange(lookupDataSet.KeywordTypes.Select(keywordType => new KeywordType
		    {
		        Name = keywordType.KeywordType,
		        SourceFeatTypeName = keywordType.SourceFeatType,
		        MatchingFeatTypeName = keywordType.IsMatchingFeatTypeNull() ? null : keywordType.MatchingFeatType,
		        MatchingItemTypeName = keywordType.IsMatchingItemTypeNull() ? null : keywordType.MatchingItemType
		    }));
		}

		private void BuildItemTypes()
		{
			var weaponTypeList = GetEntityList<WeaponType>();
		    weaponTypeList.AddRange(lookupDataSet.WeaponTypes.Select(weaponType => new WeaponType
		    {
		        Name = weaponType.WeaponType,
		        WeaponCategoryName = weaponType.WeaponCategory,
		        AttackBonusName = weaponType.AttackBonus
		    }));

		    var gearTypeList = GetEntityList<GearType>();
		    gearTypeList.AddRange(lookupDataSet.GearTypes.Where(x => x.ItemType == "Gear")
		        .Select(gearType => new GearType
		        {
		            Name = gearType.GearType,
		            WeaponCategoryName = gearType.IsWeaponCategoryNull() ? null : gearType.WeaponCategory,
		            AttackBonusName = gearType.IsAttackBonusNull() ? null : gearType.AttackBonus
		        }));
		}

		private void BuildStocks()
		{
			var stockList = GetEntityList<Stock>();
		    stockList.AddRange(lookupDataSet.Stocks.Select(stock => new Stock {Name = stock.Stock}));
		    stockList.Add(new Stock { Name = "Scrap Paper" });

			var stockItemStockList = GetEntityList<StockItemStock>();
		    stockItemStockList.AddRange((from si in lookupDataSet.StockIngredients
		        from rt in lookupDataSet.ResourceTypes.Where(x => x.ResourceType == si.Ingredient)
		            .DefaultIfEmpty()
		        from r in lookupDataSet.Resources.Where(x => rt != null && x.ResourceType == rt.ResourceType)
		            .DefaultIfEmpty()
		        select new
		        {
		            si.Stock,
		            Item = r != null ? r.Resource : si.Ingredient
		        }).Select(stockIngredient => new StockItemStock
		    {
		        StockName = stockIngredient.Stock,
		        StockItemName = stockIngredient.Item
		    }));
		}

		private void BuildEffects()
		{
			var pattern = @"^(?<pre>.*)\[(?<type>.*)\](?<post>.*)$";
			var effectTermList = GetEntityList<EffectTerm>();
			var effectList = GetEntityList<Effect>();
			foreach (var effectTerm in workDataSet.EffectTerms)
			{
				effectTermList.Add(new EffectTerm
				{
					Term = effectTerm.Term,
					EffectTypeName = effectTerm.EffectType,
					Description = effectTerm.Description,
					MathSpecifics = effectTerm.IsMathSpecificsNull() ? "" : effectTerm.MathSpecifics,
					ChannelName = effectTerm.IsChannelNull() ? "" : effectTerm.Channel
				});
				if (Regex.IsMatch(effectTerm.Term, pattern))
				{
				    var type = Regex.Replace(effectTerm.Term, pattern, "${type}");
				    effectList.AddRange((from el in lookupDataSet.EffectLookups where el.Type == type select el).Select(lookup => Regex.Replace(effectTerm.Term, pattern, "${pre}" + lookup.Item + "${post}")).Select(effectName => CreateEffect(effectName, effectTerm.Term)));
				}
				else if (effectTerm.Term.StartsWith("Blast"))
				{
				    effectList.AddRange((from el in lookupDataSet.EffectLookups where el.Type == effectTerm.Term select el).Select(lookup => CreateEffect(lookup.Item, effectTerm.Term)));
				}
				else
				{
					effectList.Add(CreateEffect(effectTerm.Term, effectTerm.Term));
				}
			}
		}

		private void BuildConditions()
		{
			var conditionList = GetEntityList<Condition>();
			var effectList = GetEntityList<Effect>();
		    conditionList.AddRange(effectList.Distinct().Select(effect => new EffectCondition {Name = effect.Name, EffectName = effect.Name}).Cast<Condition>());
		}

		private void BuildKeywords()
		{
			var keywordList = GetEntityList<Keyword>();
		    keywordList.AddRange(workDataSet.Keywords.Select(keyword => new Keyword
		    {
		        KeywordTypeName = keyword.Gear,
		        Name = keyword.Keyword,
		        ValueName = keyword.Value,
		        Notes = keyword.Notes
		    }));
		}

		private void BuildFeats()
		{
			var featList = GetEntityList<Feat>();
			var featEffectList = GetEntityList<FeatEffect>();
			var featRankList = GetEntityList<FeatRank>();
			var featRankEffectList = GetEntityList<FeatRankEffect>();
			var featRankAbilityBonusList = GetEntityList<FeatRankAbilityBonus>();
			var featRankAbilityRequirementList = GetEntityList<FeatRankAbilityRequirement>();
			var featRankAchievementRequirementList = GetEntityList<FeatRankAchievementRequirement>();
			var featRankCategoryRequirementList = GetEntityList<FeatRankCategoryRequirement>();
			var featRankFeatRequirementList = GetEntityList<FeatRankFeatRequirement>();
			var featRankKeywordList = GetEntityList<FeatRankKeyword>();

			Feat feat = null;
			string[] keywordList = null;
			string keywordTypeName = null;
			List<string> priorKeywords = null;
			var missingKeywords = new List<string>();

			// Kludge: Fix CrossBow
			foreach (var source in
					from ar in workDataSet.AdvancementRanks
					where ar.SlotName.Contains("CrossBow")
					select ar
				)
			{
				source.SlotName = source.SlotName.Replace("CrossBow", "Crossbow");
			}
			foreach (var source in
					from a in workDataSet.Advancements
					where a.SlotName.Contains("CrossBow")
					select a
				)
			{
				source.SlotName = source.SlotName.Replace("CrossBow", "Crossbow");
			}
			foreach (var source in
					from fr in workDataSet.FeatRanks
					where !fr.IsAdvancementLinkNull() && fr.AdvancementLink.Contains("CrossBow")
					select fr
				)
			{
				source.FeatName = source.FeatName.Replace("CrossBow", "Crossbow");
				source.AdvancementLink = source.AdvancementLink.Replace("CrossBow", "Crossbow");
			}
			// End Kludge

			#region Feat Query

			var featQuery =
				 (
					from ar in workDataSet.AdvancementRanks
					from a in workDataSet.Advancements
						.Where(x => x.SlotName == ar.SlotName)
						.DefaultIfEmpty()
					from fr in workDataSet.FeatRanks
						.Where(x => !x.IsAdvancementLinkNull() && x.AdvancementLink == a.SlotName && (x.Rank == ar.Rank || x.Rank == "-1"))
						.DefaultIfEmpty()
					orderby fr != null ? fr.FeatName : a.SlotName,
						int.Parse(ar.Rank)
					select new FeatData
					{
						FeatName = fr != null ? fr.FeatName : a.SlotName,
						FeatType = (fr != null && !fr.IsTypeNull()) ? fr.Type : (!ar.AdvancementsRow.IsTypeNull() ? ar.AdvancementsRow.Type : null),
						Worksheet = a.Worksheet,
						AdvancementRanksRow = ar,
						FeatRanksRow = fr
					}
				).Union(
					from fr in workDataSet.FeatRanks
					from a in workDataSet.Advancements
						.Where(x => !fr.IsAdvancementLinkNull() && x.SlotName == fr.AdvancementLink)
						.DefaultIfEmpty()
					where a == null
					orderby fr.FeatName,
						int.Parse(fr.Rank)
					select new FeatData
					{
						FeatName = fr.FeatName,
						FeatType = fr.Type,
						Worksheet = fr.Worksheet,
						FeatRanksRow = fr
					}
				);

			#endregion Feat Query

			workDataSet.MissingFeats.Clear();
			foreach (var source in featQuery)
			{
				Console.WriteLine("Building Feat '{0}' Rank {1}.", source.FeatName, source.AdvancementRanksRow != null ? source.AdvancementRanksRow.Rank : source.FeatRanksRow.Rank);
				if (source.FeatType == "Upgrade" && source.AdvancementRanksRow?.AdvancementsRow != null && !source.AdvancementRanksRow.AdvancementsRow.IsTypeNull() && source.AdvancementRanksRow.AdvancementsRow.Type == "ProficiencyFeat")
				{
					source.FeatType = "ProficiencyFeat";
				}
				if (source.FeatType == null)
				{
					if (!missingKeywords.Contains(source.FeatName))
					{
						workDataSet.MissingFeats.AddMissingFeatsRow(source.FeatName, source.Worksheet);
						missingKeywords.Add(source.FeatName);
					}
					continue;
				}
				if (feat == null || feat.Name != source.FeatName)
				{
					#region Build Feat Headers

					keywordList = null;
					priorKeywords = new List<string>();
					feat = CreateEntity<Feat>(source.FeatType);
					feat.Name = source.FeatName;
					feat.BaseTypeName = "Feat";
					feat.FeatTypeName = source.FeatType;
					feat.RoleName = source.FeatRanksRow != null && !source.FeatRanksRow.IsRoleNull() ? source.FeatRanksRow.Role : "General";
					if (source.FeatRanksRow != null && !source.FeatRanksRow.IsAdvancementLinkNull())
					{
						feat.AdvancementFeatName = source.FeatRanksRow.AdvancementLink;
					}
					else if (source.FeatRanksRow == null && source.FeatType != "Consumable" && source.AdvancementRanksRow != null)
					{
						feat.AdvancementFeatName = source.AdvancementRanksRow.SlotName;
					}
					if (feat is ActiveFeat activeFeat)
					{
					    activeFeat.DamageFactor = decimal.Parse(source.FeatRanksRow.DamageFactor);
						activeFeat.AttackSeconds = decimal.Parse(source.FeatRanksRow.AttackSeconds);
						activeFeat.StaminaCost = int.Parse(source.FeatRanksRow.StaminaCost);
						activeFeat.Range = source.FeatRanksRow.Range;
						activeFeat.WeaponCategoryName = source.FeatRanksRow.WeaponCategory;
						if (!source.FeatRanksRow.IsStandardEffectsNull())
							featEffectList.AddRange(GetFeatEffects(activeFeat, "Standard", source.FeatRanksRow.StandardEffects));
						if (!source.FeatRanksRow.IsRestrictionEffectsNull())
							featEffectList.AddRange(GetFeatEffects(activeFeat, "Restriction", source.FeatRanksRow.RestrictionEffects));
						if (!source.FeatRanksRow.IsConditionalEffectsNull())
							featEffectList.AddRange(GetFeatEffects(activeFeat, "Conditional", source.FeatRanksRow.ConditionalEffects));
						var tempKeywordList = new List<string>();
						for (var i = 1; i <= 9; i++)
						{
							if (source.FeatRanksRow["Keyword" + i.ToString()] != DBNull.Value)
								tempKeywordList.Add(source.FeatRanksRow["Keyword" + i.ToString()].ToString().Replace(", ", ","));
							else
								break;
						}
						keywordList = tempKeywordList.ToArray();
						if (activeFeat is StandardAttack standardAttack)
						{
							if (standardAttack is Attack)
							{
								keywordTypeName = "Weapon";
							}
							else
							{
								keywordTypeName = "Gear";
							}
						    standardAttack.CooldownSeconds = decimal.Parse(source.FeatRanksRow.CooldownSeconds);
							standardAttack.WeaponFormName = source.FeatRanksRow.Form.Trim();
							if (!source.FeatRanksRow.IsSpecificWeaponNull())
							{
								standardAttack.SpecificWeaponName = source.FeatRanksRow.SpecificWeapon;
							}
							if (standardAttack is Cantrip)
							{
								((Cantrip)standardAttack).SchoolName = source.FeatRanksRow.School;
							}
						}
						else if (activeFeat is PowerAttack)
						{
							if (activeFeat is Expendable)
							{
								keywordTypeName = "Implement";
							}
							var powerAttackFeat = (PowerAttack)activeFeat;
							powerAttackFeat.PowerCost = int.Parse(source.FeatRanksRow.PowerCost);
							powerAttackFeat.Level = int.Parse(source.FeatRanksRow.Level);
							powerAttackFeat.HasEndOfCombatCooldown = source.FeatRanksRow.HasCooldown.ToLower() == "yes";
							powerAttackFeat.AttackBonusName = source.FeatRanksRow.AttackBonus;
						}
					}
					else if (feat is ArmorFeat)
					{
						keywordTypeName = "Armor";
						if (!source.FeatRanksRow.IsKeywordEffectsNull())
						{
							featEffectList.AddRange(GetFeatEffects((ArmorFeat)feat, "PerKeyword", source.FeatRanksRow.KeywordEffects));
						}
					}
					else if (feat is Feature)
					{
						keywordTypeName = "Implement";
					}
					if (feat is ChanneledFeat)
					{
						((ChanneledFeat)feat).ChannelName = source.FeatRanksRow.Channel;
					}
					featList.Add(feat);

					#endregion Build Feat Headers
				}
				if (source.AdvancementRanksRow != null)
				{
					#region Build Feat Ranks

					var featRank = new FeatRank();
					featRank.FeatName = feat.Name;
					featRank.Rank = int.Parse(source.AdvancementRanksRow.Rank);
					featRank.ExpCost = int.Parse(source.AdvancementRanksRow.ExpCost);
					featRank.CoinCost = int.Parse(source.AdvancementRanksRow.CoinCost);
					List<string> keywords = null;
					var addPriorKeywords = false;
					if (source.FeatRanksRow != null && !source.FeatRanksRow.IsEffectNull())
						featRankEffectList.AddRange(GetFeatRankEffects(featRank, source.FeatRanksRow.Effect));
					if (!source.AdvancementRanksRow.IsAbilityBonusNull())
						featRankAbilityBonusList.AddRange(GetFeatRankFacts<FeatRankAbilityBonus>(featRank, source.AdvancementRanksRow.AbilityBonus));
					if (!source.AdvancementRanksRow.IsAbilityReqNull())
						featRankAbilityRequirementList.AddRange(GetFeatRankFacts<FeatRankAbilityRequirement>(featRank, source.AdvancementRanksRow.AbilityReq));
					if (!source.AdvancementRanksRow.IsAchievementReqNull())
						featRankAchievementRequirementList.AddRange(GetFeatRankFacts<FeatRankAchievementRequirement>(featRank, source.AdvancementRanksRow.AchievementReq));
					if (!source.AdvancementRanksRow.IsCategoryReqNull())
						featRankCategoryRequirementList.AddRange(GetFeatRankFacts<FeatRankCategoryRequirement>(featRank, source.AdvancementRanksRow.CategoryReq));
					if (!source.AdvancementRanksRow.IsFeatReqNull())
						featRankFeatRequirementList.AddRange(GetFeatRankFacts<FeatRankFeatRequirement>(featRank, source.AdvancementRanksRow.FeatReq));
					if (keywordList != null)
					{
						if (feat is Expendable)
						{
							keywords = new List<string>(keywordList);
						}
						else
						{
							keywords = new List<string>(keywordList[featRank.Rank.Value - 1].Split(','));
						}
					}
					else if (source.FeatRanksRow != null && !source.FeatRanksRow.IsKeywordsNull())
					{
						addPriorKeywords = true;
						keywords = new List<string>(source.FeatRanksRow.Keywords.Replace(", ", ",").Split(','));
						foreach (var priorKeyword in priorKeywords)
							keywords.Remove(priorKeyword);
					}
					if (keywords != null)
					{
						var keywordNo = 1;
						foreach (var keyword in keywords)
						{
							featRankKeywordList.Add(
								new FeatRankKeyword
								{
									FeatName = featRank.FeatName,
									Feat_Rank = featRank.Rank,
									KeywordNo = keywordNo++,
									KeywordTypeName = keywordTypeName,
									KeywordName = keyword
								});
							if (addPriorKeywords)
								priorKeywords.Add(keyword);
						}
					}
					featRankList.Add(featRank);

					#endregion Build Feat Ranks
				}
			}

			var advancementFeatList = GetEntityList<AdvancementFeat>();
		    advancementFeatList.AddRange((from f in featList where f.AdvancementFeatName != null select f.AdvancementFeatName).Distinct().Select(advancementFeat => new AdvancementFeat {Name = advancementFeat}));
		}

		private void BuildAchievements()
		{
			var achievementList = GetEntityList<Achievement>();
			var achievementRankList = GetEntityList<AchievementRank>();
			var achievementRankCategoryBonusList = GetEntityList<AchievementRankCategoryBonus>();
			var achievementRankFeatRequirementList = GetEntityList<AchievementRankFeatRequirement>();
			var achievementRankFlagRequirementList = GetEntityList<AchievementRankFlagRequirement>();

			Achievement achievement = null;
			foreach (var source in
					from al in workDataSet.AchievementRanks
					orderby al.SlotName, int.Parse(al.Rank)
					select al
				)
			{
				Console.WriteLine("Building Achievement '{0}' Rank {1}.", source.SlotName, source.Rank);
				if (achievement == null || achievement.Name != source.SlotName)
				{
					achievement = CreateEntity<Achievement>(source.Type);
					achievement.Name = source.SlotName;
					achievement.BaseTypeName = "Achievement";
					achievement.AchievementTypeName = source.Type;
					achievement.AchievementGroupName = source.DisplayName.XRegexReplace(@"^(?<FriendlyName>[^\(0-9]*).*$", "${FriendlyName}").Trim();
					achievementList.Add(achievement);
				}
				var achievementRank = CreateEntity<AchievementRank>(source.Type + "Rank");
				achievementRank.AchievementName = achievement.Name;
				achievementRank.Rank = int.Parse(source.Rank);
				achievementRank.DisplayName = source.DisplayName;
				achievementRank.InfluenceGain = source.InfluenceGain;
				if (!source.IsDescriptionNull())
					achievementRank.Description = source.Description;
				if (achievementRank is IKeywordAchievementRank)
				{
					if (!source.IsKeywordNull())
					{
						((IKeywordAchievementRank)achievementRank).Keyword = source.Keyword;
					}
					if (achievementRank is WeaponKillAchievementRank)
					{
						((IKeywordAchievementRank)achievementRank).Keyword = source.DisplayName.XRegexReplace(@"^(?<WeaponProficiency>.*) Expert [0-9]*$", "${WeaponProficiency}", RegexReplaceEmptyResultBehaviors.ThrowError); 
					}
				}
				if (achievementRank is CounterAchievementRank)
				{
					((CounterAchievementRank)achievementRank).CounterName = source.Designation;
					((CounterAchievementRank)achievementRank).Value = int.Parse(source.CounterValue);
				}
				if (achievementRank is FlagAchievementRank)
				{
					((FlagAchievementRank)achievementRank).FlagName = source.Designation;
				}
				if (achievementRank is CategoryBonusAchievementRank)
				{
					achievementRankCategoryBonusList.AddRange(GetAchievementRankFacts<AchievementRankCategoryBonus>(achievementRank, FactData.GetFacts(source, categories)));
				}
				if (achievementRank is CraftAchievementRank)
				{
					((CraftAchievementRank)achievementRank).FeatName = source.CraftingFeat;
					((CraftAchievementRank)achievementRank).Tier = int.Parse(source.CraftingTier);
					((CraftAchievementRank)achievementRank).RarityName = source.CraftingRarity;
					((CraftAchievementRank)achievementRank).Upgrade = int.Parse(source.CraftingPlus);
				}
				if (!source.IsFeatReqListNull())
				{
					achievementRankFeatRequirementList.AddRange(GetAchievementRankFacts<AchievementRankFeatRequirement>(achievementRank, FactData.GetFacts(source.FeatReqList)));
				}
				if (!source.IsFlagReqListNull())
				{
					achievementRankFlagRequirementList.AddRange(GetAchievementRankFacts<AchievementRankFlagRequirement>(achievementRank, FactData.GetFacts(source.FlagReqList)));
				}
				achievementRankList.Add(achievementRank);
			}

			var achievementGroupList = GetEntityList<AchievementGroup>();
		    achievementGroupList.AddRange((from ar in workDataSet.AchievementRanks
		            from a in achievementList
		            where a.Name == ar.SlotName
		            select new
		            {
		                Name = a.AchievementGroupName,
		                AchievementType_Name = ar.Type
		            }).Distinct()
		        .ToList()
		        .Select(achievementGroupName => new AchievementGroup
		        {
		            Name = achievementGroupName.Name,
		            BaseTypeName = "Achievement",
		            AchievementTypeName = achievementGroupName.AchievementType_Name
		        }));
		}

		private void BuildItems()
		{
			var itemList = GetEntityList<Item>();

			#region Build Recipe Items

			string itemName = null;
			foreach (var source in
				from r in workDataSet.Recipes
				from i in workDataSet.Items
					.Where(x => x.Name == r.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError))
					.DefaultIfEmpty()
				from gt in lookupDataSet.GearTypes
					.Where(x => i != null && i.Type == "Gear" && x.GearType == i.Slot)
					.DefaultIfEmpty()
				orderby r.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError)
				select new
				{
					GearType = gt,
					Item = i,
				    r.Type,
					Name = r.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError),
					Category = r.IsCategoryNull() ? null : r.Category,
					Variety = r.IsVarietyNull() ? "Essence" : r.Variety, // Divine Charms are the only known Output from Crafting Recipes that count as Components and they're of Variety "Essence"
					Tier = int.Parse(r.Tier),
					Encumbrance = i != null ? (decimal?)decimal.Parse(i.Encumbrance) : null,
					Description = i != null && !i.IsDescriptionNull() ? i.Description : null,
					ArmorType = i != null && !i.IsWeightNull() ? i.Weight : null,
					MainRole = i != null && !i.IsMainRoleNull() ? i.MainRole : null,
					InherentKeywords = i != null && !i.IsInherentKeywordsNull() ? new List<string>(i.InherentKeywords.Replace(", ", ",").Split(',')) : new List<string>(),
					UpgradeKeywords = i != null && !i.IsUpgradeKeywordsNull() ? new List<string>(i.UpgradeKeywords.XRegexReplace(@"^(?<upgradeKeywords>.*\(\+3\)).*$", "${upgradeKeywords}").Replace(", ", ",").Split(',')) : new List<string>()
				})
			{
				if (itemName == source.Name)
				{
					continue;
				}
				itemName = source.Name;
				Console.WriteLine("Building Item '{0}'.", source.Name);
				var type = "ConsumableItem";
				if (source.GearType != null)
				{
					type = source.GearType.ItemType;
				}
				else if (source.Item != null)
				{
					type = source.Item.Type;
				}
				else if (source.Category != null && Regex.IsMatch(source.Category, "^(Ammo|Ammo Container|Weapon Coating|Mule)$"))
				{
					type = source.Category.XRegexReplaceSimple(" ", "");
				}
				else if (source.Category != null && Regex.IsMatch(source.Category, "^(Camp|Holding|Outpost)$"))
				{
					type = source.Category.Trim() + "Kit";
				}
				else if (source.Category != null && Regex.IsMatch(source.Category, "^(Head)$"))
				{
					type = "Gear";
				}
				else if ((source.Category != null && source.Category == "Resource") || source.Type == "RefiningRecipe")
				{
					type = "Component";
				}
				if (type == "ConsumableItem")
				{
					var consumableFeat = (
							from f in GetEntityList<Feat>()
							where f.Name == itemName
							select f
						).FirstOrDefault();
					if (consumableFeat == null)
					{
						continue;
					}
				}
				var item = CreateEntity<Item>(type);
				item.Name = source.Name;
				item.BaseType_Name = "Item";
				item.ItemType_Name = type;
				item.Tier = source.Tier;
				item.Encumbrance = source.Encumbrance;
				item.Description = source.Description;
				if (item is Equipment)
				{
					((Equipment)item).ItemCategoryName = source.Category;
					switch (item.ItemType_Name)
					{
						case "Armor":
							((Armor)item).ArmorTypeName = source.ArmorType;
							((Armor)item).MainRoleName = source.MainRole;
							break;
						case "Weapon":
							((Weapon)item).WeaponTypeName = source.Description.XRegexReplace("^Tier [1-3] (?<WeaponType>[^;]*);.*$", "${WeaponType}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Gear":
							((Gear)item).GearTypeName = source.GearType != null ? source.GearType.GearType : source.Category;
							break;
						case "Implement":
							((Implement)item).ImplementTypeName = source.GearType.GearType;
							break;
						case "AmmoContainer":
							((AmmoContainer)item).AmmoContainerTypeName = source.Name.XRegexReplace(@"^.* (?<Type>Charge Gem|Bullet Pouch|Quiver)$", "${Type}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Ammo":
							((Ammo)item).AmmoTypeName = source.Name.XRegexReplace("^.* (?<AmmoType>Charge|Arrow)$", "${AmmoType}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Mule":
							break;
						case "CampKit":
							((CampKit)item).CampName = item.Name;
							break;
						case "HoldingKit":
							((HoldingKit)item).HoldingName = item.Name.Replace(" Kit", "");
							break;
						case "OutpostKit":
							((OutpostKit)item).OutpostName = item.Name.Replace(" Kit", "");
							break;
						case "WeaponCoating":
							((WeaponCoating)item).WeaponCoatingTypeName = source.Name.XRegexReplace("^(Apprentice's|Journeyman's|Master's|Cold Iron|Silver|Adamantine) (?<Type>.*)$", "${Type}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "ConsumableItem":
							((ConsumableItem)item).ConsumableName = source.Name;
							break;
						case "Component":
							break;
						default:
							throw new Exception(string.Format("Unexpected Equipment Type: {0}", item.ItemType_Name));
					}
				}
				else if (item is Component)
				{
					((Component)item).VarietyName = source.Variety;
				}
				else
				{
					throw new Exception(string.Format("Unexpected Item Type: {0}", item.GetType().Name));
				}
				itemList.Add(item);
				CreateRecipeOutputItem(item, source.InherentKeywords, source.UpgradeKeywords);
			}

			#endregion Build Recipe Items

			#region Build Consumable Items

			foreach (var source in
				from fr in workDataSet.FeatRanks
				where fr.Type == "Consumable" && fr.WeaponCategory == "Token"
				select new
				{
					Consumable = fr
				})
			{
				Console.WriteLine("Building Token: {0}", source.Consumable.FeatName);
				var item = new ConsumableItem
				{
					Name = source.Consumable.FeatName,
					BaseType_Name = "Item",
					ItemType_Name = "ConsumableItem",
					Tier = int.Parse(source.Consumable.Level) / 3,
					ItemCategoryName = source.Consumable.WeaponCategory,
					ConsumableName = source.Consumable.FeatName
				};
				itemList.Add(item);
			}

			#endregion Build Consumable Items

			#region Build Salvages

			foreach (var source in
				from s in lookupDataSet.Salvages
				select new
				{
					Salvage = s
				})
			{
				Console.WriteLine("Building Salvage '{0}'.", source.Salvage.Salvage);
				var item = new Salvage
				{
					Name = source.Salvage.Salvage,
					BaseType_Name = "Item",
					ItemType_Name = "Salvage",
					Tier = int.Parse(source.Salvage.Tier),
					Encumbrance = decimal.Parse(source.Salvage.Encumbrance),
					VarietyName = source.Salvage.Variety
				};
				itemList.Add(item);
			}

			#endregion Build Salvages

			#region Build Resources

			foreach (var source in
				from r in lookupDataSet.Resources
				from rt in lookupDataSet.ResourceTypes
				where rt.ResourceType == r.ResourceType
				select new
				{
					Resource = r,
					ResourceType = rt
				})
			{
				Console.WriteLine("Building Resource '{0}'.", source.Resource.Resource);
				var item = new Resource
				{
					Name = source.Resource.Resource,
					BaseType_Name = "Item",
					ItemType_Name = "Resource",
					Tier = int.Parse(source.ResourceType.Tier),
					Encumbrance = decimal.Parse(source.Resource.Encumbrance),
					VarietyName = source.ResourceType.Variety,
					ResourceTypeName = source.ResourceType.ResourceType
				};
				itemList.Add(item);
			}

			#endregion Build Resources
		}

		private void BuildRecipes()
		{
			#region Build Rank 0 for Crafting and Refining Feats

			var featRankList = GetEntityList<FeatRank>();
		    featRankList.AddRange((from r in workDataSet.Recipes
		            where r.FeatRank == "0"
		            select new
		            {
		                r.FeatName
		            }).Distinct()
		        .Select(source => new FeatRank
		        {
		            FeatName = source.FeatName,
		            Rank = 0,
		            ExpCost = 0,
		            CoinCost = 0
		        }));

		    #endregion Build Rank 0 for Crafting and Refining Feats

			var recipeList = GetEntityList<Recipe>();
			var recipeIngredientList = GetEntityList<RecipeIngredient>();

			foreach (var source in
				from r in workDataSet.Recipes
				select new
				{
					Recipe = r
				})
			{
				var recipe = CreateEntity<Recipe>(source.Recipe.Type);
				recipe.Name = source.Recipe.Name;
				recipe.BaseTypeName = "Recipe";
				recipe.RecipeTypeName = source.Recipe.Type;
				recipe.FeatName = source.Recipe.FeatName;
				recipe.Feat_Rank = int.Parse(source.Recipe.FeatRank);
				recipe.Tier = int.Parse(source.Recipe.Tier);
				recipe.OutputItemName = source.Recipe.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError);
				recipe.QtyProduced = int.Parse(source.Recipe.Qty);
				recipe.BaseCraftingSeconds = int.Parse(source.Recipe.BaseCraftingSeconds);
				recipe.AchievementTypeName = source.Recipe.AchievementType;
				if (recipe is RefiningRecipe)
				{
					recipe.Quality = int.Parse(source.Recipe.Quality);
					((RefiningRecipe)recipe).Upgrade = int.Parse(source.Recipe.Upgrade);
				}
				else if (recipe is CraftingRecipe)
				{
					recipe.Quality = int.Parse(source.Recipe.BaseQuality);
				}
				else
				{
					throw new Exception(string.Format("Unexpected Recipe Type: '{0}'.", recipe.GetType().Name));
				}
				recipeList.Add(recipe);

				for (var i = 1; i <= 4; i++)
				{
					if (source.Recipe["Ingredient" + i.ToString()] == DBNull.Value)
					{
						break;
					}
					var recipeIngredient = CreateEntity<RecipeIngredient>(source.Recipe.Type + "Ingredient");
					recipeIngredient.RecipeName = recipe.Name;
					recipeIngredient.IngredientNo = i;
					recipeIngredient.Quantity = int.Parse(source.Recipe["Qty" + i.ToString()].ToString());
					if (recipeIngredient is RefiningRecipeIngredient)
					{
						((RefiningRecipeIngredient)recipeIngredient).StockName = source.Recipe["Ingredient" + i.ToString()].ToString();
					}
					else if (recipeIngredient is CraftingRecipeIngredient)
					{
						((CraftingRecipeIngredient)recipeIngredient).ComponentName = source.Recipe["Ingredient" + i.ToString()].ToString();
					}
					else
					{
						throw new Exception(string.Format("Unexpected Recipe Type: '{0}'.", recipe.GetType().Name));
					}
					recipeIngredientList.Add(recipeIngredient);
				}
			}
		}

		private void BuildFeatRankTrainerLevels()
		{
			var levels = GetEntityList<FeatRankTrainerLevel>();
			levels.AddRange((
					from l in workDataSet.FeatRankTrainerLevels
					from a in GetEntityList<AdvancementFeat>()
						.Where(x => x.Name.ToLower() == l.Feat.ToLower())
						.DefaultIfEmpty()
					select new FeatRankTrainerLevel
					{
						Feat_Name = a.Name,
						Feat_Rank = l.Rank,
						Trainer_Name = l.Trainer,
						Level = l.Level
					}
				).ToList()
			);

			GetEntityList<Trainer>().AddRange((
					from tn in (
							from l in levels
							select l.Trainer_Name
						).Distinct()
					select new Trainer
					{
						Name = tn
					}
				).Distinct().ToList()
			);
		}

		private void BuildHexes()
		{
			var ratings = new List<string> { "Stone", "Fish", "Crops", "Wood", "Game", "Herds", "Ore" };

			var hexes = GetEntityList<Hex>();
			var hexBulkRatings = GetEntityList<HexBulkRating>();

			foreach (var source in workDataSet.Hexes)
			{
				var hex = new Hex
				{
					Longitude = int.Parse(source.Longitude),
					Latitude = int.Parse(source.Latitude),
					RegionName = source.Region,
					TerrainTypeName = source.TerrainType,
					HexTypeName = source.HexType
				};
				hexes.Add(hex);
				for (var i = 0; i <= 6; i++)
				{
					hexBulkRatings.Add(
						new HexBulkRating
						{
							Hex_Longitude = hex.Longitude,
							Hex_Latitude = hex.Latitude,
							BulkRating_Name = ratings[i],
							Rating = int.Parse(source[i + 7].ToString())
						});
				}
			}
		}

		private void BuildStructures()
		{
			var camps = GetEntityList<Camp>();
		    camps.AddRange(workDataSet.Camps.Select(source => new Camp
		    {
		        BaseTypeName = "Structure",
		        StructureTypeName = "Camp",
		        Name = source.Name,
		        Description = source.Description,
		        DisplayName = source.DisplayName,
		        Encumbrance = decimal.Parse(source.Encumbrance),
		        Quality = int.Parse(source.Quality),
		        Tier = int.Parse(source.Tier),
		        Category = source.Category,
		        HousingData = source.HousingData,
		        HouseEntityDefn = source.HouseEntityDefn,
		        Cooldown = int.Parse(source.Cooldown),
		        NoLoot = !source.IsNoLootNull() && bool.Parse(source.NoLoot),
		        Upgradable = source.Upgradable == "Y",
		        AccountRedeemName = source.IsAccountRedeemNull() ? null : source.AccountRedeem
		    }));

		    var pattern = @"^(?<Name>.*) \+(?<Upgrade>[0-9]+)$";
			
			// Kludge: Remove bogus row
			workDataSet.CampUpgrades.Rows[0].Delete();
			// End Kludge
			
			var campUpgrades = GetEntityList<CampUpgrade>();
		    campUpgrades.AddRange(workDataSet.CampUpgrades.Select(source => new CampUpgrade
		    {
		        StructureName = source.Name.XRegexReplace(pattern, "${Name}"),
		        Upgrade = int.Parse(source.Name.XRegexReplace(pattern, "${Upgrade}")),
		        PowerChannelDurationSeconds = int.Parse(source.PowerChannelDuration),
		        PowerRegenerationSeconds = int.Parse(source.PowerRegeneration),
		        PowerCooldownMinutes = int.Parse(source.PowerCooldown),
		        BuildingDurationMinutes = int.Parse(source.BuildingDuration),
		        ConstructionData = source.ConstructionData,
		        BaseTypeName = source.BaseType
		    }));

		    pattern = @"^(?<Name>.*) (?<Quality>[0-9]+)$";
			var trainerPattern = @"^(?<Trainer>.*)=(?<Level>[0-9]+)$";
			var resourcePattern = @"^(?<Resource>.*) \+(?<Bonus>[0-9]+)$";
			var upkeepPattern = @"^(?<Resource>.*) (?<Requirement>[0-9]+)$";

			// Kludge: Remove bogus row
			workDataSet.HoldingUpgrades.Rows[0].Delete();
			// End Kludge

			var holdings = GetEntityList<Holding>();
			var holdingUpgrades = GetEntityList<HoldingUpgrade>();
			var holdingBonuses = GetEntityList<HoldingUpgradeBulkResourceBonus>();
			var holdingReqs = GetEntityList<HoldingUpgradeBulkResourceRequirement>();
			var holdingTrainers = GetEntityList<HoldingUpgradeTrainerLevel>();
			Holding holding = null;
			foreach (var source in workDataSet.HoldingUpgrades)
			{
				if (holding == null || holding.Name != source.Name)
				{
					holding = new Holding { Name = source.Name, BaseTypeName = "Structure", StructureTypeName = "Holding" };
					holdings.Add(holding);
				}

				var holdingUpgrade = new HoldingUpgrade
				{
					CraftingFacilityFeatName = source.IsCraftingFacilityNull() ? null : source.CraftingFacility.XRegexReplace(pattern, "${Name}"),
					CraftingFacilityQuality = source.IsCraftingFacilityNull() ? (int?)null : int.Parse(source.CraftingFacility.XRegexReplace(pattern, "${Quality}")),
					InfluenceCost = int.Parse(source.InfluenceCost),
					StructureName = source.Name,
					Upgrade = int.Parse(source.Upgrade),
					PvPPeakGuards = int.Parse(source.PvPPeakGuards),
					NonPvPPeakGuards = int.Parse(source.NonPvPPeakGuards),
					GuardSurgeSize = int.Parse(source.GuardSurgeSize),
					GuardRespawnsPerMinute = decimal.Parse(source.GuardRespawnsPerMinute),
					PvPGuardRespawns = int.Parse(source.PvPGuardRespawns),
					NonPvPGuardRespawns = int.Parse(source.NonPvPGuardRespawns),
					NonPvPRespawnFill = int.Parse(source.NonPvPRespawnFill),
					MinPvPTime = decimal.Parse(source.MinPvPTime),
					GuardEntityNames = source.GuardEntityNames
				};
				holdingUpgrades.Add(holdingUpgrade);

				var trainerNo = 1;
			    holdingTrainers.AddRange(source.Training.Replace(", ", ",")
			        .Trim()
			        .Split(',')
			        .Select(training => new HoldingUpgradeTrainerLevel
			        {
			            StructureName = holdingUpgrade.StructureName,
			            Upgrade = holdingUpgrade.Upgrade,
			            TrainerNo = trainerNo++,
			            TrainerName = training.XRegexReplace(trainerPattern, "${Trainer}"),
			            Level = int.Parse(training.XRegexReplace(trainerPattern, "${Level}"))
			        }));

			    var bonusNo = 1;
				if (!source.IsResourceBonusNull())
				{
				    holdingBonuses.AddRange(source.ResourceBonus.Replace(", ", ",")
				        .Trim()
				        .Split(',')
				        .Select(resource => new HoldingUpgradeBulkResourceBonus
				        {
				            StructureName = holdingUpgrade.StructureName,
				            Upgrade = holdingUpgrade.Upgrade,
				            BonusNo = bonusNo++,
				            BulkResourceName = resource.XRegexReplace(resourcePattern, "${Resource}"),
				            Bonus = int.Parse(resource.XRegexReplace(resourcePattern, "${Bonus}"))
				        }));
				}

				var reqNo = 1;
			    holdingReqs.AddRange(source.Upkeep.Replace(", ", ",")
			        .Trim()
			        .Split(',')
			        .Select(req => new HoldingUpgradeBulkResourceRequirement
			        {
			            StructureName = holdingUpgrade.StructureName,
			            Upgrade = holdingUpgrade.Upgrade,
			            RequirementNo = reqNo++,
			            BulkResourceName = req.XRegexReplace(upkeepPattern, "${Resource}"),
			            Requirement = int.Parse(req.XRegexReplace(upkeepPattern, "${Requirement}"))
			        }));
			}

			var genPattern = @"^(?<Percentage>[0-9]+)\% (?<Resource>.*) from (?<Rating>.*)$";

			// Kludge: Remove bogus row
			workDataSet.OutpostUpgrades.Rows[0].Delete();
			// End Kludge

			var outposts = GetEntityList<Outpost>();
			var outpostResources = GetEntityList<OutpostBulkResource>();
			var outpostWorkers = GetEntityList<OutpostWorkerFeat>();
			foreach (var source in workDataSet.Outposts)
			{
				var outpost = new Outpost
				{
					Name = source.Name,
					BaseTypeName = "Structure",
					StructureTypeName = "Outpost",
					ConstructionData = source.ConstructionData
				};
				outposts.Add(outpost);

			    outpostResources.AddRange(source.ResourceGeneration.Replace(", ", ",")
			        .Trim()
			        .Split(',')
			        .Select(gen => new OutpostBulkResource
			        {
			            StructureName = outpost.Name,
			            BulkRatingName = gen.XRegexReplace(genPattern, "${Rating}"),
			            BulkResourceName = gen.XRegexReplace(genPattern, "${Resource}"),
			            Percentage = int.Parse(gen.XRegexReplace(genPattern, "${Percentage}"))
			        }));

			    outpostWorkers.AddRange(source.WorkerSkills.Replace(", ", ",")
			        .Trim()
			        .Split(',')
			        .Select(worker => new OutpostWorkerFeat
			        {
			            StructureName = outpost.Name,
			            WorkerFeatName = worker
			        }));
			}

			var outpostUpgrades = GetEntityList<OutpostUpgrade>();
		    outpostUpgrades.AddRange(workDataSet.OutpostUpgrades.Select(source => new OutpostUpgrade
		    {
		        StructureName = source.Name,
		        Upgrade = int.Parse(source.Upgrade),
		        EffortBonus = int.Parse(source.EffortBonus),
		        InfluenceCost = int.Parse(source.InfluenceCost),
		        PvPPeakGuards = int.Parse(source.PvPPeakGuards),
		        GuardSurgeSize = int.Parse(source.GuardSurgeSize),
		        GuardRespawnsPerMinute = decimal.Parse(source.GuardRespawnsPerMinute),
		        PvPGuardRespawns = int.Parse(source.PvPGuardRespawns),
		        MinPvPTime = decimal.Parse(source.MinPvPTime),
		        GuardEntityNames = source.GuardEntityNames
		    }));
		}

		#endregion Build Methods

		#region Helper Methods

		private List<T> GetEntityList<T>()
		{
			if (!EntityLists.ContainsKey(typeof(T)))
				EntityLists[typeof(T)] = new List<T>();
			return (List<T>)EntityLists[typeof(T)];
		}

		private T CreateEntity<T>(string typeName) where T : class
		{
			var baseType = typeof(T);
			var type = assembly.GetType(string.Format("{0}.{1}", baseType.Namespace, typeName));
			if (type == null)
				throw new Exception(string.Format("Unknown Type '{0}'.", typeName));
			var entity = Activator.CreateInstance(type) as T;
			if (entity == null)
				throw new Exception(string.Format("Type '{0}' cannot be cast as Type '{1}'.", typeName, baseType.Name));
			return entity;
		}

		private Effect CreateEffect(string name, string effectTerm)
		{
			var effect = new Effect
			{
				Name = name,
				EffectTermTerm = effectTerm
			};
			effects[effect.Name] = effect;
			return effect;
		}

		private List<T> GetEffects<T>(string effectsText, string effectType, Action<T> action) where T : IEffectReference, new()
		{
			var effects = new List<T>();
			var effectList = GetEntityList<Effect>();
			var effectDescriptionList = GetEntityList<EffectDescription>();

			var effectNo = 1;
			var pattern = @"^(?<Entry>[^,\(]*(\([^\)]*\)[^,]*)?)(, *(?<Remainder>.*))?$";
			var entryPattern
				= @"^"
					+ @"(?<EffectName>(?:(?! \+| \-| [0-9]+| \(| to | on | if | with | per ).)+)"
					+ @"(?: +(?<Magnitude>(?:\+|\-)?[0-9]+%?))?"
					+ @"(?: ?\("
						+ @"(?:(?<Duration>(?<DurationValue>[0-9]+) (?<DurationUnits>Rounds?|Seconds?)))?"
						+ @"(?:, )?"
						+ @"(?:(?<Chance>(?<ChanceValue>[0-9]+)(?<ChanceUnits>% Chance)))?"
						+ @"(?:(?<Distance>(?<DistanceValue>\-?[0-9]+) (?<DistanceUnits>Meters?)))?"
					+ @"\))?"
					+ @"(?: +(?<Target>to +(?<TargetIdentifier>Self|All( Targets)?)))?"
					+ @"(?: +(?<Discriminator>with|if (?<DiscriminatorTarget>Attacker|Target) (?:has|is)|per|on) (?<Condition>(?:(?! to ).)+)(?: (?<ConditionTarget>to Self))?)?"
				+ @"$"
				;
			var entryRegex = new Regex(entryPattern, RegexOptions.IgnoreCase);
			var remainder = effectsText;
			while (remainder != "")
			{
				var entry = remainder.XRegexReplace(pattern, "${Entry}", RegexReplaceEmptyResultBehaviors.ThrowError);
				EffectDescription effectDescription;
				if (!effectDescriptions.ContainsKey(entry))
				{
					effectDescription = new EffectDescription();
					effectDescription.Text = entry;
					effectDescription.EffectName = entry.XRegexReplace(entryRegex, "${EffectName}", RegexReplaceEmptyResultBehaviors.ThrowError);
					if (!this.effects.Keys.Contains(effectDescription.EffectName))
					{
						effectList.Add(CreateEffect(effectDescription.EffectName, null));
					}
					effectDescription.Magnitude = entry.XRegexReplace(entryRegex, "${Magnitude}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Duration = entry.XRegexReplace(entryRegex, "${Duration}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Chance = entry.XRegexReplace(entryRegex, "${Chance}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Distance = entry.XRegexReplace(entryRegex, "${Distance}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Target = entry.XRegexReplace(entryRegex, "${Target}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Discriminator = entry.XRegexReplace(entryRegex, "${Discriminator}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.ConditionName = entry.XRegexReplace(entryRegex, "${Condition}", RegexReplaceEmptyResultBehaviors.Ignore);
					if (effectDescription.ConditionName == "")
					{
						effectDescription.ConditionName = null;
					}
					effectDescription.ConditionTarget = entry.XRegexReplace(entryRegex, "${ConditionTarget}", RegexReplaceEmptyResultBehaviors.Ignore);

					// Build FormattedDescription
					// {0} = Hyperlink to EffectDetails by Effect_Name
					// {1} = Hyperlink to ??? by Condition
					// {2} = Newline indention before Discriminator
					var sb = new StringBuilder();
					sb.Append("{0}");
					if (!string.IsNullOrEmpty(effectDescription.Magnitude))
					{
						sb.AppendFormat(" {0}", effectDescription.Magnitude);
					}
					if (!(string.IsNullOrEmpty(effectDescription.Duration) && string.IsNullOrEmpty(effectDescription.Chance) && string.IsNullOrEmpty(effectDescription.Distance)))
					{
						sb.Append(" (");
						var pad = "";
						if (!string.IsNullOrEmpty(effectDescription.Duration))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Duration);
							pad = ", ";
						}
						if (!string.IsNullOrEmpty(effectDescription.Chance))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Chance);
						}
						if (!string.IsNullOrEmpty(effectDescription.Distance))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Distance);
							pad = ", ";
						}
						sb.Append(")");
					}
					if (!string.IsNullOrEmpty(effectDescription.Target))
					{
						sb.AppendFormat(" {0}", effectDescription.Target);
					}
					if (!string.IsNullOrEmpty(effectDescription.Discriminator))
					{
						sb.Append("{2}");
						sb.AppendFormat("{0}", effectDescription.Discriminator);
					}
					if (!string.IsNullOrEmpty(effectDescription.ConditionName))
					{
						sb.Append(" {1}");
						//sb.AppendFormat(" {0}", effectDescription.Condition);
					}
					if (!string.IsNullOrEmpty(effectDescription.ConditionTarget))
					{
						sb.AppendFormat(" {0}", effectDescription.ConditionTarget);
					}
					effectDescription.FormattedDescription = sb.ToString();

					effectDescriptions[entry] = effectDescription;
					effectDescriptionList.Add(effectDescription);
				}
				else
				{
					effectDescription = effectDescriptions[entry];
				}

			    var effect = new T
			    {
			        EffectDescriptionText = effectDescription.Text,
			        EffectNo = effectNo++
			    };
			    action(effect);
				effects.Add(effect);
				remainder = remainder.XRegexReplace(pattern, "${Remainder}");
			}
			return effects;
		}

		private List<FeatEffect> GetFeatEffects(Feat feat, string effectType, string effectsText)
		{
		    return GetEffects<FeatEffect>(effectsText, effectType, (x) =>
		        {
		            x.FeatName = feat.Name;
		            x.EffectType = effectType;
		        })
		        .ToList();
		}

		private List<FeatRankEffect> GetFeatRankEffects(FeatRank featRank, string effectsText)
		{
		    return GetEffects<FeatRankEffect>(effectsText, null, (x) =>
		        {
		            x.FeatName = featRank.FeatName;
		            x.Feat_Rank = featRank.Rank;
		        })
		        .ToList();
		}

		private List<T> GetFeatRankFacts<T>(FeatRank featRank, string factsText) where T : IFeatRankFact, new()
		{
			var facts = new List<T>();
			foreach (var factData in FactData.GetFacts(factsText))
			{
				var fact = new T();
				fact.FeatName = featRank.FeatName;
				fact.Feat_Rank = featRank.Rank;
				fact.OptionNo = factData.OptionNo;
				fact.FactName = factData.Name;
				if (fact is IFeatRankBonus)
				{
					((IFeatRankBonus)fact).BonusNo = factData.FactNo;
					((IFeatRankBonus)fact).Bonus = decimal.Parse(factData.Value);
				}
				else if (fact is IFeatRankRequirement)
				{
					((IFeatRankRequirement)fact).RequirementNo = factData.FactNo;
					((IFeatRankRequirement)fact).Value = int.Parse(factData.Value);
				}
				else
				{
					throw new Exception(string.Format("Unexpected Fact Type '{0}'.", fact.GetType().Name));
				}
				facts.Add(fact);
			}
			return facts;
		}

		private List<T> GetAchievementRankFacts<T>(AchievementRank achievementRank, List<FactData> factsText) where T : IAchievementRankFact, new()
		{
			var facts = new List<T>();
			foreach (var factData in factsText)
			{
			    var fact = new T
			    {
			        AchievementName = achievementRank.AchievementName,
			        Achievement_Rank = achievementRank.Rank
			    };
			    if (fact is IAchievementRankCategoryBonus)
				{
					((IAchievementRankCategoryBonus)fact).BonusNo = factData.FactNo;
					((IAchievementRankCategoryBonus)fact).CategoryName = factData.Name;
					((IAchievementRankCategoryBonus)fact).Bonus = int.Parse(factData.Value);
				}
				else if (fact is IAchievementRankRequirement)
				{
					((IAchievementRankRequirement)fact).RequirementNo = factData.FactNo;
					((IAchievementRankRequirement)fact).OptionNo = factData.OptionNo;
					if (fact is IAchievementRankFeatRequirement)
					{
						((IAchievementRankFeatRequirement)fact).FeatName = factData.Name;
						((IAchievementRankFeatRequirement)fact).Feat_Rank = int.Parse(factData.Value);
					}
					else if (fact is IAchievementRankFlagRequirement)
					{
						((IAchievementRankFlagRequirement)fact).FlagName = factData.Name;
					}
				}
				else
				{
					throw new Exception(string.Format("Unexpected Fact Type '{0}'.", fact.GetType().Name));
				}
				facts.Add(fact);
			}
			return facts;
		}

		private void CreateRecipeOutputItem(Item item, List<string> inherentKeywords, List<string> upgradeKeywords)
		{
			var recipeOutputItemList = GetEntityList<RecipeOutputItem>();
			var recipeOutputItemUpgradeList = GetEntityList<RecipeOutputItemUpgrade>();
			var recipeOutputItemUpgradeKeywordList = GetEntityList<RecipeOutputItemUpgradeKeyword>();

			recipeOutputItemList.Add(new RecipeOutputItem
			{
				ItemName = item.Name
			});

			for (var upgrade = 0; upgrade <= 5; upgrade++)
			{
				recipeOutputItemUpgradeList.Add(new RecipeOutputItemUpgrade
				{
					ItemName = item.Name,
					Upgrade = upgrade
				});
				var keywordNo = 1;
				if (upgrade == 0)
				{
				    recipeOutputItemUpgradeKeywordList.AddRange(inherentKeywords.Select(keyword => new RecipeOutputItemUpgradeKeyword
				    {
				        ItemName = item.Name,
				        Upgrade = upgrade,
				        KeywordKindName = "Inherent",
				        KeywordNo = keywordNo++,
				        KeywordTypeName = item.ItemType_Name,
				        KeywordName = keyword
				    }));
				}
				if (upgradeKeywords.Count > upgrade)
				{
					recipeOutputItemUpgradeKeywordList.Add(new RecipeOutputItemUpgradeKeyword
					{
						ItemName = item.Name,
						Upgrade = upgrade,
						KeywordKindName = "Upgrade",
						KeywordNo = 1,
						KeywordTypeName = item.ItemType_Name,
						KeywordName = upgradeKeywords[upgrade].XRegexReplace(@"^(?<Keyword>.*) \(\+[0-3]\).*$", "${Keyword}", RegexReplaceEmptyResultBehaviors.ThrowError)
					});
				}
			}
		}

		#endregion Helper Methods
	}
}
