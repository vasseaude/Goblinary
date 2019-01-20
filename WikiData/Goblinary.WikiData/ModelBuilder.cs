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

    using Goblinary.Common;
    using Goblinary.WikiData;
    using Goblinary.WikiData.Model;
    using Goblinary.WikiData.SqlServer;
    using System.Data.Entity.Validation;

    public class ModelBuilder
	{
		private class FactData
		{
			public static List<FactData> GetFacts(string factsText)
			{
				List<FactData> facts = new List<FactData>();
				int factNo = 1;
				foreach (string fact in factsText.Replace("personality", "Personality").Replace(" or ", "|").Replace(", ", ",").Split(','))
					if (fact != "")
					{
						int optionNo = 1;
						foreach (string option in fact.Split('|'))
						{
							if (option != "")
							{
								string[] parts = option.Split('=');
								facts.Add(new FactData(factNo, optionNo++, parts[0].Trim(), parts[1].Trim()));
							}
						}
						factNo++;
					}
				return facts;
			}

			public static List<FactData> GetFacts(WorkDataSet.AchievementRanksRow source, IEnumerable<string> categories)
			{
				List<FactData> facts = new List<FactData>();
				int factNo = 1;
				int optionNo = 1;
				foreach (string category in categories)
					if (source[category].ToString() != "")
						facts.Add(new FactData(factNo++, optionNo, category, source[category].ToString()));
				return facts;
			}

			private FactData() { }

			private FactData(int factNo, int optionNo, string name, string value)
			{
				this.FactNo = factNo;
				this.OptionNo = optionNo;
				this.Name = name;
				this.Value = value;
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
			this.assembly = Assembly.GetAssembly(typeof(Feat));
			this.categories = new List<string> { "Adventure", "Arcane", "Crafting", "Divine", "Martial", "Social", "Subterfuge" };
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
			this.BuildLookups();
			this.BuildEntityTypes();
			this.BuildRoles();
			this.BuildKeywordTypes();
			this.BuildItemTypes();
			this.BuildStocks();
			this.BuildEffects();
			this.BuildConditions();
			this.BuildKeywords();
			this.BuildFeats();
			this.BuildAchievements();
			this.BuildItems();
			this.BuildRecipes();
			this.BuildFeatRankTrainerLevels();
			this.BuildHexes();
			this.BuildStructures();

			this.ValidateForeignKeys();

			this.SaveData<BulkRating>();
			this.SaveData<BulkResource>();
			this.SaveData<AdvancementFeat>();
			this.SaveData<EntityType>();
			this.SaveData<EntityTypeMapping>();
			this.SaveData<Ability>();
			this.SaveData<Role>();
			this.SaveData<EffectTerm>();
			this.SaveData<Effect>();
			this.SaveData<Condition>();
			this.SaveData<EffectDescription>();
			this.SaveData<KeywordType>();
			this.SaveData<Keyword>();
			this.SaveData<WeaponCategory>();
			this.SaveData<AttackBonus>();
			this.SaveData<WeaponType>();
			this.SaveData<GearType>();
			this.SaveData<Feat>();
			this.SaveData<AchievementGroup>();
			this.SaveData<Achievement>();
			this.SaveData<FeatEffect>();
			this.SaveData<FeatRank>();
			this.SaveData<Trainer>();
			this.SaveData<FeatRankTrainerLevel>();
			this.SaveData<AchievementRank>();
			this.SaveData<FeatRankEffect>();
			this.SaveData<FeatRankKeyword>();
			this.SaveData<FeatRankAbilityBonus>();
			this.SaveData<FeatRankAbilityRequirement>();
			this.SaveData<FeatRankAchievementRequirement>();
			this.SaveData<FeatRankCategoryRequirement>();
			this.SaveData<FeatRankFeatRequirement>();
			this.SaveData<AchievementRankCategoryBonus>();
			this.SaveData<AchievementRankFeatRequirement>();
			this.SaveData<AchievementRankFlagRequirement>();
			this.SaveData<Stock>();
			this.SaveData<Camp>();
			this.SaveData<Holding>();
			this.SaveData<Outpost>();
			this.SaveData<CampUpgrade>();
			this.SaveData<Item>();
			this.SaveData<StockItemStock>();
			this.SaveData<RecipeOutputItem>();
			this.SaveData<RecipeOutputItemUpgrade>();
			this.SaveData<RecipeOutputItemUpgradeKeyword>();
			this.SaveData<Recipe>();
			this.SaveData<RecipeIngredient>();
			this.SaveData<Hex>();
			this.SaveData<HexBulkRating>();
			this.SaveData<HoldingUpgrade>();
			this.SaveData<HoldingUpgradeBulkResourceBonus>();
			this.SaveData<HoldingUpgradeBulkResourceRequirement>();
			this.SaveData<HoldingUpgradeTrainerLevel>();
			this.SaveData<OutpostUpgrade>();
			this.SaveData<OutpostBulkResource>();
			this.SaveData<OutpostWorkerFeat>();
		}

		private void ValidateForeignKeys()
		{
			foreach (var req in (
					from frar in this.GetEntityList<FeatRankAchievementRequirement>()
					from a in this.GetEntityList<Achievement>()
					where a.Name.ToLower() == frar.Achievement_Name.ToLower()
						&& a.Name != frar.Achievement_Name
					select new
					{
						Req = frar,
						Name = a.Name
					}
				))
			{
				req.Req.Achievement_Name = req.Name;
			}
			foreach (var req in (
					from frfr in this.GetEntityList<FeatRankFeatRequirement>()
					from f in this.GetEntityList<Feat>()
					where f.Name.ToLower() == frfr.RequiredFeat_Name.ToLower()
						&& f.Name != frfr.RequiredFeat_Name
					select new
					{
						Req = frfr,
						Name = f.Name
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
				var list = this.GetEntityList<T>();
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
                    catch (Exception ex)
					{
						transaction.Rollback();
						throw ex;
					}
				}
			}
		}

		#region Build Methods

		private void BuildLookups()
		{
			List<Ability> abilityList = this.GetEntityList<Ability>();
			List<AttackBonus> attackBonusList = this.GetEntityList<AttackBonus>();
			List<WeaponCategory> weaponCategoryList = this.GetEntityList<WeaponCategory>();
			List<BulkRating> bulkRatings = this.GetEntityList<BulkRating>();
			List<BulkResource> bulkResources = this.GetEntityList<BulkResource>();
			List<Condition> conditions = this.GetEntityList<Condition>();

			foreach (var lookup in this.lookupDataSet.Lookups)
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
					default:
						throw new Exception(string.Format("Unexpected Lookup Type: {0}", lookup.Type));
				}
			}
		}

		private void BuildEntityTypes()
		{
			List<EntityType> entityTypes = this.GetEntityList<EntityType>();
			List<EntityTypeMapping> mappings = this.GetEntityList<EntityTypeMapping>();

			foreach (var entityType in this.lookupDataSet.EntityTypes)
			{
				entityTypes.Add(
					new EntityType
					{
						BaseType_Name = entityType.BaseType,
						Name = entityType.EntityType,
						ParentType_Name = entityType.IsParentTypeNull() ? null : entityType.ParentType,
						Modifier = entityType.Modifier,
						DisplayName = entityType.DisplayName
					});
				if (entityType.Modifier == "Final")
				{
					string parent = entityType.EntityType;
					while (parent != null)
					{
						mappings.Add(new EntityTypeMapping
						{
							BaseType_Name = entityType.BaseType,
							ChildType_Name = entityType.EntityType,
							ParentType_Name = parent
						});
						parent = (
							from ft in this.lookupDataSet.EntityTypes
							where ft.EntityType == parent
							select ft.IsParentTypeNull() ? null : ft.ParentType).First();
					}
				}
			}
		}

		private void BuildRoles()
		{
			List<Role> roles = this.GetEntityList<Role>();

			foreach (var role in (
					from fr in this.workDataSet.FeatRanks
					where !fr.IsRoleNull()
					select fr.Role
				).Union(
					from i in this.workDataSet.Items
					where !i.IsMainRoleNull()
					select i.MainRole
				).Distinct())
			{
				roles.Add(new Role
				{
					Name = role
				});
			}
		}

		private void BuildKeywordTypes()
		{
			List<KeywordType> keywordTypeList = this.GetEntityList<KeywordType>();
			foreach (var keywordType in this.lookupDataSet.KeywordTypes)
			{
				keywordTypeList.Add(new KeywordType
				{
					Name = keywordType.KeywordType,
					SourceFeatType_Name = keywordType.SourceFeatType,
					MatchingFeatType_Name = keywordType.IsMatchingFeatTypeNull() ? null : keywordType.MatchingFeatType,
					MatchingItemType_Name = keywordType.IsMatchingItemTypeNull() ? null : keywordType.MatchingItemType
				});
			}
		}

		private void BuildItemTypes()
		{
			List<WeaponType> weaponTypeList = this.GetEntityList<WeaponType>();
			foreach (var weaponType in this.lookupDataSet.WeaponTypes)
			{
				weaponTypeList.Add(new WeaponType
				{
					Name = weaponType.WeaponType,
					WeaponCategory_Name = weaponType.WeaponCategory,
					AttackBonus_Name = weaponType.AttackBonus
				});
			}

			List<GearType> gearTypeList = this.GetEntityList<GearType>();
			foreach (var gearType in this.lookupDataSet.GearTypes.Where(x => x.ItemType == "Gear"))
			{
				gearTypeList.Add(new GearType
				{
					Name = gearType.GearType,
					WeaponCategory_Name = gearType.IsWeaponCategoryNull() ? null : gearType.WeaponCategory,
					AttackBonus_Name = gearType.IsAttackBonusNull() ? null : gearType.AttackBonus
				});
			}
		}

		private void BuildStocks()
		{
			List<Stock> stockList = this.GetEntityList<Stock>();
			foreach (var stock in this.lookupDataSet.Stocks)
			{
				stockList.Add(new Stock { Name = stock.Stock });
			}
			stockList.Add(new Stock { Name = "Scrap Paper" });

			List<StockItemStock> stockItemStockList = this.GetEntityList<StockItemStock>();
			foreach (var stockIngredient in
				from si in this.lookupDataSet.StockIngredients
				from rt in this.lookupDataSet.ResourceTypes
					.Where(x => x.ResourceType == si.Ingredient)
					.DefaultIfEmpty()
				from r in this.lookupDataSet.Resources
					.Where(x => rt != null && x.ResourceType == rt.ResourceType)
					.DefaultIfEmpty()
				select new
				{
					Stock = si.Stock,
					Item = r != null ? r.Resource : si.Ingredient
				})
			{
				stockItemStockList.Add(new StockItemStock
				{
					Stock_Name = stockIngredient.Stock,
					StockItem_Name = stockIngredient.Item
				});
			}
		}

		private void BuildEffects()
		{
			string pattern = @"^(?<pre>.*)\[(?<type>.*)\](?<post>.*)$";
			List<EffectTerm> effectTermList = this.GetEntityList<EffectTerm>();
			List<Effect> effectList = this.GetEntityList<Effect>();
			foreach (var effectTerm in this.workDataSet.EffectTerms)
			{
				effectTermList.Add(new EffectTerm
				{
					Term = effectTerm.Term,
					EffectType_Name = effectTerm.EffectType,
					Description = effectTerm.Description,
					MathSpecifics = effectTerm.IsMathSpecificsNull() ? "" : effectTerm.MathSpecifics,
					Channel_Name = effectTerm.IsChannelNull() ? "" : effectTerm.Channel
				});
				if (Regex.IsMatch(effectTerm.Term, pattern))
				{
					string type = Regex.Replace(effectTerm.Term, pattern, "${type}");
					foreach (var lookup in
						from el in this.lookupDataSet.EffectLookups
						where el.Type == type
						select el)
					{
						string effectName = Regex.Replace(effectTerm.Term, pattern, "${pre}" + lookup.Item + "${post}");
						effectList.Add(this.CreateEffect(effectName, effectTerm.Term));
					}
				}
				else if (effectTerm.Term.StartsWith("Blast"))
				{
					foreach (var lookup in
						from el in this.lookupDataSet.EffectLookups
						where el.Type == effectTerm.Term
						select el)
					{
						effectList.Add(this.CreateEffect(lookup.Item, effectTerm.Term));
					}
				}
				else
				{
					effectList.Add(this.CreateEffect(effectTerm.Term, effectTerm.Term));
				}
			}
		}

		private void BuildConditions()
		{
			List<Condition> conditionList = this.GetEntityList<Condition>();
			List<Effect> effectList = this.GetEntityList<Effect>();
			foreach (var effect in effectList.Distinct())
			{
				conditionList.Add(new EffectCondition { Name = effect.Name, Effect_Name = effect.Name });
			}
		}

		private void BuildKeywords()
		{
			List<Keyword> keywordList = this.GetEntityList<Keyword>();
			foreach (var keyword in this.workDataSet.Keywords)
			{
				keywordList.Add(new Keyword
				{
					KeywordType_Name = keyword.Gear,
					Name = keyword.Keyword,
					Value_Name = keyword.Value,
					Notes = keyword.Notes
				});
			}
		}

		private void BuildFeats()
		{
			List<Feat> featList = this.GetEntityList<Feat>();
			List<FeatEffect> featEffectList = this.GetEntityList<FeatEffect>();
			List<FeatRank> featRankList = this.GetEntityList<FeatRank>();
			List<FeatRankEffect> featRankEffectList = this.GetEntityList<FeatRankEffect>();
			List<FeatRankAbilityBonus> featRankAbilityBonusList = this.GetEntityList<FeatRankAbilityBonus>();
			List<FeatRankAbilityRequirement> featRankAbilityRequirementList = this.GetEntityList<FeatRankAbilityRequirement>();
			List<FeatRankAchievementRequirement> featRankAchievementRequirementList = this.GetEntityList<FeatRankAchievementRequirement>();
			List<FeatRankCategoryRequirement> featRankCategoryRequirementList = this.GetEntityList<FeatRankCategoryRequirement>();
			List<FeatRankFeatRequirement> featRankFeatRequirementList = this.GetEntityList<FeatRankFeatRequirement>();
			List<FeatRankKeyword> featRankKeywordList = this.GetEntityList<FeatRankKeyword>();

			Feat feat = null;
			string[] keywordList = null;
			string keywordTypeName = null;
			List<string> priorKeywords = null;
			List<string> missingKeywords = new List<string>();

			// Kludge: Fix CrossBow
			foreach (var source in
					from ar in this.workDataSet.AdvancementRanks
					where ar.SlotName.Contains("CrossBow")
					select ar
				)
			{
				source.SlotName = source.SlotName.Replace("CrossBow", "Crossbow");
			}
			foreach (var source in
					from a in this.workDataSet.Advancements
					where a.SlotName.Contains("CrossBow")
					select a
				)
			{
				source.SlotName = source.SlotName.Replace("CrossBow", "Crossbow");
			}
			foreach (var source in
					from fr in this.workDataSet.FeatRanks
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
					from ar in this.workDataSet.AdvancementRanks
					from a in this.workDataSet.Advancements
						.Where(x => x.SlotName == ar.SlotName)
						.DefaultIfEmpty()
					from fr in this.workDataSet.FeatRanks
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
					from fr in this.workDataSet.FeatRanks
					from a in this.workDataSet.Advancements
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

			this.workDataSet.MissingFeats.Clear();
			foreach (FeatData source in featQuery)
			{
				Console.WriteLine("Building Feat '{0}' Rank {1}.", source.FeatName, source.AdvancementRanksRow != null ? source.AdvancementRanksRow.Rank : source.FeatRanksRow.Rank);
				if (source.FeatType == "Upgrade"
					&& source.AdvancementRanksRow != null
					&& source.AdvancementRanksRow.AdvancementsRow != null
					&& !source.AdvancementRanksRow.AdvancementsRow.IsTypeNull()
					&& source.AdvancementRanksRow.AdvancementsRow.Type == "ProficiencyFeat")
				{
					source.FeatType = "ProficiencyFeat";
				}
				if (source.FeatType == null)
				{
					if (!missingKeywords.Contains(source.FeatName))
					{
						this.workDataSet.MissingFeats.AddMissingFeatsRow(source.FeatName, source.Worksheet);
						missingKeywords.Add(source.FeatName);
					}
					continue;
				}
				if (feat == null || feat.Name != source.FeatName)
				{
					#region Build Feat Headers

					keywordList = null;
					priorKeywords = new List<string>();
					feat = this.CreateEntity<Feat>(source.FeatType);
					feat.Name = source.FeatName;
					feat.BaseType_Name = "Feat";
					feat.FeatType_Name = source.FeatType;
					feat.Role_Name = source.FeatRanksRow != null && !source.FeatRanksRow.IsRoleNull() ? source.FeatRanksRow.Role : "General";
					if (source.FeatRanksRow != null && !source.FeatRanksRow.IsAdvancementLinkNull())
					{
						feat.AdvancementFeat_Name = source.FeatRanksRow.AdvancementLink;
					}
					else if (source.FeatRanksRow == null && source.FeatType != "Consumable" && source.AdvancementRanksRow != null)
					{
						feat.AdvancementFeat_Name = source.AdvancementRanksRow.SlotName;
					}
					if (feat is ActiveFeat)
					{
						ActiveFeat activeFeat = (ActiveFeat)feat;
						activeFeat.DamageFactor = decimal.Parse(source.FeatRanksRow.DamageFactor);
						activeFeat.AttackSeconds = decimal.Parse(source.FeatRanksRow.AttackSeconds);
						activeFeat.StaminaCost = int.Parse(source.FeatRanksRow.StaminaCost);
						activeFeat.Range = source.FeatRanksRow.Range;
						activeFeat.WeaponCategory_Name = source.FeatRanksRow.WeaponCategory;
						if (!source.FeatRanksRow.IsStandardEffectsNull())
							featEffectList.AddRange(this.GetFeatEffects(activeFeat, "Standard", source.FeatRanksRow.StandardEffects));
						if (!source.FeatRanksRow.IsRestrictionEffectsNull())
							featEffectList.AddRange(this.GetFeatEffects(activeFeat, "Restriction", source.FeatRanksRow.RestrictionEffects));
						if (!source.FeatRanksRow.IsConditionalEffectsNull())
							featEffectList.AddRange(this.GetFeatEffects(activeFeat, "Conditional", source.FeatRanksRow.ConditionalEffects));
						List<string> tempKeywordList = new List<string>();
						for (int i = 1; i <= 9; i++)
						{
							if (source.FeatRanksRow["Keyword" + i.ToString()] != DBNull.Value)
								tempKeywordList.Add(source.FeatRanksRow["Keyword" + i.ToString()].ToString().Replace(", ", ","));
							else
								break;
						}
						keywordList = tempKeywordList.ToArray();
						if (feat is StandardAttack)
						{
							if (feat is Attack)
							{
								keywordTypeName = "Weapon";
							}
							else
							{
								keywordTypeName = "Gear";
							}
							StandardAttack standardAttack = (StandardAttack)feat;
							standardAttack.CooldownSeconds = decimal.Parse(source.FeatRanksRow.CooldownSeconds);
							standardAttack.WeaponForm_Name = source.FeatRanksRow.Form.Trim();
							if (!source.FeatRanksRow.IsSpecificWeaponNull())
							{
								standardAttack.SpecificWeapon_Name = source.FeatRanksRow.SpecificWeapon;
							}
							if (feat is Cantrip)
							{
								((Cantrip)feat).School_Name = source.FeatRanksRow.School;
							}
						}
						else if (feat is PowerAttack)
						{
							if (feat is Expendable)
							{
								keywordTypeName = "Implement";
							}
							PowerAttack powerAttackFeat = (PowerAttack)feat;
							powerAttackFeat.PowerCost = int.Parse(source.FeatRanksRow.PowerCost);
							powerAttackFeat.Level = int.Parse(source.FeatRanksRow.Level);
							powerAttackFeat.HasEndOfCombatCooldown = source.FeatRanksRow.HasCooldown.ToLower() == "yes";
							powerAttackFeat.AttackBonus_Name = source.FeatRanksRow.AttackBonus;
						}
					}
					else if (feat is ArmorFeat)
					{
						keywordTypeName = "Armor";
						if (!source.FeatRanksRow.IsKeywordEffectsNull())
						{
							featEffectList.AddRange(this.GetFeatEffects((ArmorFeat)feat, "PerKeyword", source.FeatRanksRow.KeywordEffects));
						}
					}
					else if (feat is Feature)
					{
						keywordTypeName = "Implement";
					}
					if (feat is ChanneledFeat)
					{
						((ChanneledFeat)feat).Channel_Name = source.FeatRanksRow.Channel;
					}
					featList.Add(feat);

					#endregion Build Feat Headers
				}
				if (source.AdvancementRanksRow != null)
				{
					#region Build Feat Ranks

					FeatRank featRank = new FeatRank();
					featRank.Feat_Name = feat.Name;
					featRank.Rank = int.Parse(source.AdvancementRanksRow.Rank);
					featRank.ExpCost = int.Parse(source.AdvancementRanksRow.ExpCost);
					featRank.CoinCost = int.Parse(source.AdvancementRanksRow.CoinCost);
					List<string> keywords = null;
					bool addPriorKeywords = false;
					if (source.FeatRanksRow != null && !source.FeatRanksRow.IsEffectNull())
						featRankEffectList.AddRange(this.GetFeatRankEffects(featRank, source.FeatRanksRow.Effect));
					if (!source.AdvancementRanksRow.IsAbilityBonusNull())
						featRankAbilityBonusList.AddRange(this.GetFeatRankFacts<FeatRankAbilityBonus>(featRank, source.AdvancementRanksRow.AbilityBonus));
					if (!source.AdvancementRanksRow.IsAbilityReqNull())
						featRankAbilityRequirementList.AddRange(this.GetFeatRankFacts<FeatRankAbilityRequirement>(featRank, source.AdvancementRanksRow.AbilityReq));
					if (!source.AdvancementRanksRow.IsAchievementReqNull())
						featRankAchievementRequirementList.AddRange(this.GetFeatRankFacts<FeatRankAchievementRequirement>(featRank, source.AdvancementRanksRow.AchievementReq));
					if (!source.AdvancementRanksRow.IsCategoryReqNull())
						featRankCategoryRequirementList.AddRange(this.GetFeatRankFacts<FeatRankCategoryRequirement>(featRank, source.AdvancementRanksRow.CategoryReq));
					if (!source.AdvancementRanksRow.IsFeatReqNull())
						featRankFeatRequirementList.AddRange(this.GetFeatRankFacts<FeatRankFeatRequirement>(featRank, source.AdvancementRanksRow.FeatReq));
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
						foreach (string priorKeyword in priorKeywords)
							keywords.Remove(priorKeyword);
					}
					if (keywords != null)
					{
						int keywordNo = 1;
						foreach (string keyword in keywords)
						{
							featRankKeywordList.Add(
								new FeatRankKeyword
								{
									Feat_Name = featRank.Feat_Name,
									Feat_Rank = featRank.Rank,
									KeywordNo = keywordNo++,
									KeywordType_Name = keywordTypeName,
									Keyword_Name = keyword
								});
							if (addPriorKeywords)
								priorKeywords.Add(keyword);
						}
					}
					featRankList.Add(featRank);

					#endregion Build Feat Ranks
				}
			}

			List<AdvancementFeat> advancementFeatList = this.GetEntityList<AdvancementFeat>();
			foreach (var advancementFeat in (
					from f in featList
					where f.AdvancementFeat_Name != null
					select f.AdvancementFeat_Name
				).Distinct())
			{
				advancementFeatList.Add(new AdvancementFeat { Name = advancementFeat });
			}
		}

		private void BuildAchievements()
		{
			List<Achievement> achievementList = this.GetEntityList<Achievement>();
			List<AchievementRank> achievementRankList = this.GetEntityList<AchievementRank>();
			List<AchievementRankCategoryBonus> achievementRankCategoryBonusList = this.GetEntityList<AchievementRankCategoryBonus>();
			List<AchievementRankFeatRequirement> achievementRankFeatRequirementList = this.GetEntityList<AchievementRankFeatRequirement>();
			List<AchievementRankFlagRequirement> achievementRankFlagRequirementList = this.GetEntityList<AchievementRankFlagRequirement>();

			Achievement achievement = null;
			foreach (var source in
					from al in this.workDataSet.AchievementRanks
					orderby al.SlotName, int.Parse(al.Rank)
					select al
				)
			{
				Console.WriteLine("Building Achievement '{0}' Rank {1}.", source.SlotName, source.Rank);
				if (achievement == null || achievement.Name != source.SlotName)
				{
					achievement = this.CreateEntity<Achievement>(source.Type);
					achievement.Name = source.SlotName;
					achievement.BaseType_Name = "Achievement";
					achievement.AchievementType_Name = source.Type;
					achievement.AchievementGroup_Name = source.DisplayName.XRegexReplace(@"^(?<FriendlyName>[^\(0-9]*).*$", "${FriendlyName}").Trim();
					achievementList.Add(achievement);
				}
				AchievementRank achievementRank = this.CreateEntity<AchievementRank>(source.Type + "Rank");
				achievementRank.Achievement_Name = achievement.Name;
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
					((CounterAchievementRank)achievementRank).Counter_Name = source.Designation;
					((CounterAchievementRank)achievementRank).Value = int.Parse(source.CounterValue);
				}
				if (achievementRank is FlagAchievementRank)
				{
					((FlagAchievementRank)achievementRank).Flag_Name = source.Designation;
				}
				if (achievementRank is CategoryBonusAchievementRank)
				{
					achievementRankCategoryBonusList.AddRange(this.GetAchievementRankFacts<AchievementRankCategoryBonus>(achievementRank, FactData.GetFacts(source, this.categories)));
				}
				if (achievementRank is CraftAchievementRank)
				{
					((CraftAchievementRank)achievementRank).Feat_Name = source.CraftingFeat;
					((CraftAchievementRank)achievementRank).Tier = int.Parse(source.CraftingTier);
					((CraftAchievementRank)achievementRank).Rarity_Name = source.CraftingRarity;
					((CraftAchievementRank)achievementRank).Upgrade = int.Parse(source.CraftingPlus);
				}
				if (!source.IsFeatReqListNull())
				{
					achievementRankFeatRequirementList.AddRange(this.GetAchievementRankFacts<AchievementRankFeatRequirement>(achievementRank, FactData.GetFacts(source.FeatReqList)));
				}
				if (!source.IsFlagReqListNull())
				{
					achievementRankFlagRequirementList.AddRange(this.GetAchievementRankFacts<AchievementRankFlagRequirement>(achievementRank, FactData.GetFacts(source.FlagReqList)));
				}
				achievementRankList.Add(achievementRank);
			}

			List<AchievementGroup> achievementGroupList = this.GetEntityList<AchievementGroup>();
			foreach (var achievementGroupName in (
						from ar in this.workDataSet.AchievementRanks
						from a in achievementList
						where a.Name == ar.SlotName
						select new
						{
							Name = a.AchievementGroup_Name,
							AchievementType_Name = ar.Type
						}
					).Distinct().ToList()
				)
			{
				achievementGroupList.Add(new AchievementGroup
				{
					Name = achievementGroupName.Name,
					BaseType_Name = "Achievement",
					AchievementType_Name = achievementGroupName.AchievementType_Name
				});
			}
		}

		private void BuildItems()
		{
			List<Item> itemList = this.GetEntityList<Item>();

			#region Build Recipe Items

			string itemName = null;
			foreach (var source in
				from r in this.workDataSet.Recipes
				from i in this.workDataSet.Items
					.Where(x => x.Name == r.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError))
					.DefaultIfEmpty()
				from gt in this.lookupDataSet.GearTypes
					.Where(x => i != null && i.Type == "Gear" && x.GearType == i.Slot)
					.DefaultIfEmpty()
				orderby r.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError)
				select new
				{
					GearType = gt,
					Item = i,
					Type = r.Type,
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
				string type = "ConsumableItem";
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
							from f in this.GetEntityList<Feat>()
							where f.Name == itemName
							select f
						).FirstOrDefault();
					if (consumableFeat == null)
					{
						continue;
					}
				}
				Item item = this.CreateEntity<Item>(type);
				item.Name = source.Name;
				item.BaseType_Name = "Item";
				item.ItemType_Name = type;
				item.Tier = source.Tier;
				item.Encumbrance = source.Encumbrance;
				item.Description = source.Description;
				if (item is Equipment)
				{
					((Equipment)item).ItemCategory_Name = source.Category;
					switch (item.ItemType_Name)
					{
						case "Armor":
							((Armor)item).ArmorType_Name = source.ArmorType;
							((Armor)item).MainRole_Name = source.MainRole;
							break;
						case "Weapon":
							((Weapon)item).WeaponType_Name = source.Description.XRegexReplace("^Tier [1-3] (?<WeaponType>[^;]*);.*$", "${WeaponType}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Gear":
							((Gear)item).GearType_Name = source.GearType != null ? source.GearType.GearType : source.Category;
							break;
						case "Implement":
							((Implement)item).ImplementType_Name = source.GearType.GearType;
							break;
						case "AmmoContainer":
							((AmmoContainer)item).AmmoContainerType_Name = source.Name.XRegexReplace(@"^.* (?<Type>Charge Gem|Bullet Pouch|Quiver)$", "${Type}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Ammo":
							((Ammo)item).AmmoType_Name = source.Name.XRegexReplace("^.* (?<AmmoType>Charge|Arrow)$", "${AmmoType}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "Mule":
							break;
						case "CampKit":
							((CampKit)item).Camp_Name = item.Name;
							break;
						case "HoldingKit":
							((HoldingKit)item).Holding_Name = item.Name.Replace(" Kit", "");
							break;
						case "OutpostKit":
							((OutpostKit)item).Outpost_Name = item.Name.Replace(" Kit", "");
							break;
						case "WeaponCoating":
							((WeaponCoating)item).WeaponCoatingType_Name = source.Name.XRegexReplace("^(Apprentice's|Journeyman's|Master's|Cold Iron|Silver|Adamantine) (?<Type>.*)$", "${Type}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "ConsumableItem":
							((ConsumableItem)item).Consumable_Name = source.Name;
							break;
						case "Component":
							break;
						default:
							throw new Exception(string.Format("Unexpected Equipment Type: {0}", item.ItemType_Name));
					}
				}
				else if (item is Component)
				{
					((Component)item).Variety_Name = source.Variety;
				}
				else
				{
					throw new Exception(string.Format("Unexpected Item Type: {0}", item.GetType().Name));
				}
				itemList.Add(item);
				this.CreateRecipeOutputItem(item, source.InherentKeywords, source.UpgradeKeywords);
			}

			#endregion Build Recipe Items

			#region Build Consumable Items

			foreach (var source in
				from fr in this.workDataSet.FeatRanks
				where fr.Type == "Consumable" && fr.WeaponCategory == "Token"
				select new
				{
					Consumable = fr
				})
			{
				Console.WriteLine("Building Token: {0}", source.Consumable.FeatName);
				ConsumableItem item = new ConsumableItem
				{
					Name = source.Consumable.FeatName,
					BaseType_Name = "Item",
					ItemType_Name = "ConsumableItem",
					Tier = int.Parse(source.Consumable.Level) / 3,
					ItemCategory_Name = source.Consumable.WeaponCategory,
					Consumable_Name = source.Consumable.FeatName
				};
				itemList.Add(item);
			}

			#endregion Build Consumable Items

			#region Build Salvages

			foreach (var source in
				from s in this.lookupDataSet.Salvages
				select new
				{
					Salvage = s
				})
			{
				Console.WriteLine("Building Salvage '{0}'.", source.Salvage.Salvage);
				Salvage item = new Salvage
				{
					Name = source.Salvage.Salvage,
					BaseType_Name = "Item",
					ItemType_Name = "Salvage",
					Tier = int.Parse(source.Salvage.Tier),
					Encumbrance = decimal.Parse(source.Salvage.Encumbrance),
					Variety_Name = source.Salvage.Variety
				};
				itemList.Add(item);
			}

			#endregion Build Salvages

			#region Build Resources

			foreach (var source in
				from r in this.lookupDataSet.Resources
				from rt in this.lookupDataSet.ResourceTypes
				where rt.ResourceType == r.ResourceType
				select new
				{
					Resource = r,
					ResourceType = rt
				})
			{
				Console.WriteLine("Building Resource '{0}'.", source.Resource.Resource);
				Resource item = new Resource
				{
					Name = source.Resource.Resource,
					BaseType_Name = "Item",
					ItemType_Name = "Resource",
					Tier = int.Parse(source.ResourceType.Tier),
					Encumbrance = decimal.Parse(source.Resource.Encumbrance),
					Variety_Name = source.ResourceType.Variety,
					ResourceType_Name = source.ResourceType.ResourceType
				};
				itemList.Add(item);
			}

			#endregion Build Resources
		}

		private void BuildRecipes()
		{
			#region Build Rank 0 for Crafting and Refining Feats

			List<FeatRank> featRankList = this.GetEntityList<FeatRank>();

			foreach (var source in (
				from r in this.workDataSet.Recipes
				where r.FeatRank == "0"
				select new
				{
					FeatName = r.FeatName
				}).Distinct())
			{
				featRankList.Add(new FeatRank
				{
					Feat_Name = source.FeatName,
					Rank = 0,
					ExpCost = 0,
					CoinCost = 0
				});
			}

			#endregion Build Rank 0 for Crafting and Refining Feats

			List<Recipe> recipeList = this.GetEntityList<Recipe>();
			List<RecipeIngredient> recipeIngredientList = this.GetEntityList<RecipeIngredient>();

			foreach (var source in
				from r in this.workDataSet.Recipes
				select new
				{
					Recipe = r
				})
			{
				Recipe recipe = this.CreateEntity<Recipe>(source.Recipe.Type);
				recipe.Name = source.Recipe.Name;
				recipe.BaseType_Name = "Recipe";
				recipe.RecipeType_Name = source.Recipe.Type;
				recipe.Feat_Name = source.Recipe.FeatName;
				recipe.Feat_Rank = int.Parse(source.Recipe.FeatRank);
				recipe.Tier = int.Parse(source.Recipe.Tier);
				recipe.OutputItem_Name = source.Recipe.Output.XRegexReplace(@"^(?<OutputItem>.*?)( Utility \((Boot|Glove)\))?$", "${OutputItem}", RegexReplaceEmptyResultBehaviors.ThrowError);
				recipe.QtyProduced = int.Parse(source.Recipe.Qty);
				recipe.BaseCraftingSeconds = int.Parse(source.Recipe.BaseCraftingSeconds);
				recipe.AchievementType_Name = source.Recipe.AchievementType;
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

				for (int i = 1; i <= 4; i++)
				{
					if (source.Recipe["Ingredient" + i.ToString()] == DBNull.Value)
					{
						break;
					}
					RecipeIngredient recipeIngredient = this.CreateEntity<RecipeIngredient>(source.Recipe.Type + "Ingredient");
					recipeIngredient.Recipe_Name = recipe.Name;
					recipeIngredient.IngredientNo = i;
					recipeIngredient.Quantity = int.Parse(source.Recipe["Qty" + i.ToString()].ToString());
					if (recipeIngredient is RefiningRecipeIngredient)
					{
						((RefiningRecipeIngredient)recipeIngredient).Stock_Name = source.Recipe["Ingredient" + i.ToString()].ToString();
					}
					else if (recipeIngredient is CraftingRecipeIngredient)
					{
						((CraftingRecipeIngredient)recipeIngredient).Component_Name = source.Recipe["Ingredient" + i.ToString()].ToString();
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
			List<FeatRankTrainerLevel> levels = this.GetEntityList<FeatRankTrainerLevel>();
			levels.AddRange((
					from l in this.workDataSet.FeatRankTrainerLevels
					from a in this.GetEntityList<AdvancementFeat>()
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

			this.GetEntityList<Trainer>().AddRange((
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
			List<string> ratings = new List<string> { "Stone", "Fish", "Crops", "Wood", "Game", "Herds", "Ore" };

			List<Hex> hexes = this.GetEntityList<Hex>();
			List<HexBulkRating> hexBulkRatings = this.GetEntityList<HexBulkRating>();

			foreach (var source in this.workDataSet.Hexes)
			{
				Hex hex = new Hex
				{
					Longitude = int.Parse(source.Longitude),
					Latitude = int.Parse(source.Latitude),
					Region_Name = source.Region,
					TerrainType_Name = source.TerrainType,
					HexType_Name = source.HexType
				};
				hexes.Add(hex);
				for (int i = 0; i <= 6; i++)
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
			List<Camp> camps = this.GetEntityList<Camp>();
			foreach (var source in this.workDataSet.Camps)
			{
				Camp camp = new Camp
				{
					BaseType_Name = "Structure",
					StructureType_Name = "Camp",
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
					NoLoot = source.IsNoLootNull() ? false : bool.Parse(source.NoLoot),
					Upgradable = source.Upgradable == "Y",
					AccountRedeem_Name = source.IsAccountRedeemNull() ? null : source.AccountRedeem
				};
				camps.Add(camp);
			}

			string pattern = @"^(?<Name>.*) \+(?<Upgrade>[0-9]+)$";
			
			// Kludge: Remove bogus row
			this.workDataSet.CampUpgrades.Rows[0].Delete();
			// End Kludge
			
			List<CampUpgrade> campUpgrades = this.GetEntityList<CampUpgrade>();
			foreach (var source in this.workDataSet.CampUpgrades)
			{
				CampUpgrade campUpgrade = new CampUpgrade
				{
					Structure_Name = source.Name.XRegexReplace(pattern, "${Name}"),
					Upgrade = int.Parse(source.Name.XRegexReplace(pattern, "${Upgrade}")),
					PowerChannelDurationSeconds = int.Parse(source.PowerChannelDuration),
					PowerRegenerationSeconds = int.Parse(source.PowerRegeneration),
					PowerCooldownMinutes = int.Parse(source.PowerCooldown),
					BuildingDurationMinutes = int.Parse(source.BuildingDuration),
					ConstructionData = source.ConstructionData,
					BaseType_Name = source.BaseType
				};
				campUpgrades.Add(campUpgrade);
			}

			pattern = @"^(?<Name>.*) (?<Quality>[0-9]+)$";
			string trainerPattern = @"^(?<Trainer>.*)=(?<Level>[0-9]+)$";
			string resourcePattern = @"^(?<Resource>.*) \+(?<Bonus>[0-9]+)$";
			string upkeepPattern = @"^(?<Resource>.*) (?<Requirement>[0-9]+)$";

			// Kludge: Remove bogus row
			this.workDataSet.HoldingUpgrades.Rows[0].Delete();
			// End Kludge

			List<Holding> holdings = this.GetEntityList<Holding>();
			List<HoldingUpgrade> holdingUpgrades = this.GetEntityList<HoldingUpgrade>();
			List<HoldingUpgradeBulkResourceBonus> holdingBonuses = this.GetEntityList<HoldingUpgradeBulkResourceBonus>();
			List<HoldingUpgradeBulkResourceRequirement> holdingReqs = this.GetEntityList<HoldingUpgradeBulkResourceRequirement>();
			List<HoldingUpgradeTrainerLevel> holdingTrainers = this.GetEntityList<HoldingUpgradeTrainerLevel>();
			Holding holding = null;
			foreach (var source in this.workDataSet.HoldingUpgrades)
			{
				if (holding == null || holding.Name != source.Name)
				{
					holding = new Holding { Name = source.Name, BaseType_Name = "Structure", StructureType_Name = "Holding" };
					holdings.Add(holding);
				}

				HoldingUpgrade holdingUpgrade = new HoldingUpgrade
				{
					CraftingFacilityFeat_Name = source.IsCraftingFacilityNull() ? null : source.CraftingFacility.XRegexReplace(pattern, "${Name}"),
					CraftingFacilityQuality = source.IsCraftingFacilityNull() ? (int?)null : int.Parse(source.CraftingFacility.XRegexReplace(pattern, "${Quality}")),
					InfluenceCost = int.Parse(source.InfluenceCost),
					Structure_Name = source.Name,
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

				int trainerNo = 1;
				foreach (string training in source.Training.Replace(", ", ",").Trim().Split(','))
				{
					HoldingUpgradeTrainerLevel holdingTrainer = new HoldingUpgradeTrainerLevel
					{
						Structure_Name = holdingUpgrade.Structure_Name,
						Upgrade = holdingUpgrade.Upgrade,
						TrainerNo = trainerNo++,
						Trainer_Name = training.XRegexReplace(trainerPattern, "${Trainer}"),
						Level = int.Parse(training.XRegexReplace(trainerPattern, "${Level}"))
					};
					holdingTrainers.Add(holdingTrainer);
				}

				int bonusNo = 1;
				if (!source.IsResourceBonusNull())
				{
					foreach (string resource in source.ResourceBonus.Replace(", ", ",").Trim().Split(','))
					{
						HoldingUpgradeBulkResourceBonus holdingBonus = new HoldingUpgradeBulkResourceBonus
						{
							Structure_Name = holdingUpgrade.Structure_Name,
							Upgrade = holdingUpgrade.Upgrade,
							BonusNo = bonusNo++,
							BulkResource_Name = resource.XRegexReplace(resourcePattern, "${Resource}"),
							Bonus = int.Parse(resource.XRegexReplace(resourcePattern, "${Bonus}"))
						};
						holdingBonuses.Add(holdingBonus);
					}
				}

				int reqNo = 1;
				foreach (string req in source.Upkeep.Replace(", ", ",").Trim().Split(','))
				{
					HoldingUpgradeBulkResourceRequirement holdingReq = new HoldingUpgradeBulkResourceRequirement
					{
						Structure_Name = holdingUpgrade.Structure_Name,
						Upgrade = holdingUpgrade.Upgrade,
						RequirementNo = reqNo++,
						BulkResource_Name = req.XRegexReplace(upkeepPattern, "${Resource}"),
						Requirement = int.Parse(req.XRegexReplace(upkeepPattern, "${Requirement}"))
					};
					holdingReqs.Add(holdingReq);
				}
			}

			string genPattern = @"^(?<Percentage>[0-9]+)\% (?<Resource>.*) from (?<Rating>.*)$";

			// Kludge: Remove bogus row
			this.workDataSet.OutpostUpgrades.Rows[0].Delete();
			// End Kludge

			List<Outpost> outposts = this.GetEntityList<Outpost>();
			List<OutpostBulkResource> outpostResources = this.GetEntityList<OutpostBulkResource>();
			List<OutpostWorkerFeat> outpostWorkers = this.GetEntityList<OutpostWorkerFeat>();
			foreach (var source in this.workDataSet.Outposts)
			{
				Outpost outpost = new Outpost
				{
					Name = source.Name,
					BaseType_Name = "Structure",
					StructureType_Name = "Outpost",
					ConstructionData = source.ConstructionData
				};
				outposts.Add(outpost);

				foreach (var gen in source.ResourceGeneration.Replace(", ", ",").Trim().Split(','))
				{
					OutpostBulkResource outpostResource = new OutpostBulkResource
					{
						Structure_Name = outpost.Name,
						BulkRating_Name = gen.XRegexReplace(genPattern, "${Rating}"),
						BulkResource_Name = gen.XRegexReplace(genPattern, "${Resource}"),
						Percentage = int.Parse(gen.XRegexReplace(genPattern, "${Percentage}"))
					};
					outpostResources.Add(outpostResource);
				}

				foreach (var worker in source.WorkerSkills.Replace(", ", ",").Trim().Split(','))
				{
					OutpostWorkerFeat outpostWorker = new OutpostWorkerFeat
					{
						Structure_Name = outpost.Name,
						WorkerFeat_Name = worker
					};
					outpostWorkers.Add(outpostWorker);
				}
			}

			List<OutpostUpgrade> outpostUpgrades = this.GetEntityList<OutpostUpgrade>();
			foreach (var source in this.workDataSet.OutpostUpgrades)
			{
				OutpostUpgrade outpostUpgrade = new OutpostUpgrade
				{
					Structure_Name = source.Name,
					Upgrade = int.Parse(source.Upgrade),
					EffortBonus = int.Parse(source.EffortBonus),
					InfluenceCost = int.Parse(source.InfluenceCost),
					PvPPeakGuards = int.Parse(source.PvPPeakGuards),
					GuardSurgeSize = int.Parse(source.GuardSurgeSize),
					GuardRespawnsPerMinute = decimal.Parse(source.GuardRespawnsPerMinute),
					PvPGuardRespawns = int.Parse(source.PvPGuardRespawns),
					MinPvPTime = decimal.Parse(source.MinPvPTime),
					GuardEntityNames = source.GuardEntityNames
				};
				outpostUpgrades.Add(outpostUpgrade);
			}
		}

		#endregion Build Methods

		#region Helper Methods

		private List<T> GetEntityList<T>()
		{
			if (!this.EntityLists.ContainsKey(typeof(T)))
				this.EntityLists[typeof(T)] = new List<T>();
			return (List<T>)this.EntityLists[typeof(T)];
		}

		private T CreateEntity<T>(string typeName) where T : class
		{
			Type baseType = typeof(T);
			Type type = this.assembly.GetType(string.Format("{0}.{1}", baseType.Namespace, typeName));
			if (type == null)
				throw new Exception(string.Format("Unknown Type '{0}'.", typeName));
			T entity = Activator.CreateInstance(type) as T;
			if (entity == null)
				throw new Exception(string.Format("Type '{0}' cannot be cast as Type '{1}'.", typeName, baseType.Name));
			return entity;
		}

		private Effect CreateEffect(string name, string effectTerm)
		{
			Effect effect = new Effect
			{
				Name = name,
				EffectTerm_Term = effectTerm
			};
			this.effects[effect.Name] = effect;
			return effect;
		}

		private List<T> GetEffects<T>(string effectsText, string effectType, Action<T> action) where T : IEffectReference, new()
		{
			List<T> effects = new List<T>();
			List<Effect> effectList = this.GetEntityList<Effect>();
			List<EffectDescription> effectDescriptionList = this.GetEntityList<EffectDescription>();

			int effectNo = 1;
			string pattern = @"^(?<Entry>[^,\(]*(\([^\)]*\)[^,]*)?)(, *(?<Remainder>.*))?$";
			string entryPattern
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
			Regex entryRegex = new Regex(entryPattern, RegexOptions.IgnoreCase);
			string remainder = effectsText;
			while (remainder != "")
			{
				string entry = remainder.XRegexReplace(pattern, "${Entry}", RegexReplaceEmptyResultBehaviors.ThrowError);
				EffectDescription effectDescription;
				if (!this.effectDescriptions.ContainsKey(entry))
				{
					effectDescription = new EffectDescription();
					effectDescription.Text = entry;
					effectDescription.Effect_Name = entry.XRegexReplace(entryRegex, "${EffectName}", RegexReplaceEmptyResultBehaviors.ThrowError);
					if (!this.effects.Keys.Contains(effectDescription.Effect_Name))
					{
						effectList.Add(this.CreateEffect(effectDescription.Effect_Name, null));
					}
					effectDescription.Magnitude = entry.XRegexReplace(entryRegex, "${Magnitude}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Duration = entry.XRegexReplace(entryRegex, "${Duration}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Chance = entry.XRegexReplace(entryRegex, "${Chance}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Distance = entry.XRegexReplace(entryRegex, "${Distance}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Target = entry.XRegexReplace(entryRegex, "${Target}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Discriminator = entry.XRegexReplace(entryRegex, "${Discriminator}", RegexReplaceEmptyResultBehaviors.Ignore);
					effectDescription.Condition_Name = entry.XRegexReplace(entryRegex, "${Condition}", RegexReplaceEmptyResultBehaviors.Ignore);
					if (effectDescription.Condition_Name == "")
					{
						effectDescription.Condition_Name = null;
					}
					effectDescription.ConditionTarget = entry.XRegexReplace(entryRegex, "${ConditionTarget}", RegexReplaceEmptyResultBehaviors.Ignore);

					// Build FormattedDescription
					// {0} = Hyperlink to EffectDetails by Effect_Name
					// {1} = Hyperlink to ??? by Condition
					// {2} = Newline indention before Discriminator
					StringBuilder sb = new StringBuilder();
					sb.Append("{0}");
					if (!String.IsNullOrEmpty(effectDescription.Magnitude))
					{
						sb.AppendFormat(" {0}", effectDescription.Magnitude);
					}
					if (!(String.IsNullOrEmpty(effectDescription.Duration) && String.IsNullOrEmpty(effectDescription.Chance) && String.IsNullOrEmpty(effectDescription.Distance)))
					{
						sb.Append(" (");
						string pad = "";
						if (!String.IsNullOrEmpty(effectDescription.Duration))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Duration);
							pad = ", ";
						}
						if (!String.IsNullOrEmpty(effectDescription.Chance))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Chance);
						}
						if (!String.IsNullOrEmpty(effectDescription.Distance))
						{
							sb.AppendFormat("{0}{1}", pad, effectDescription.Distance);
							pad = ", ";
						}
						sb.Append(")");
					}
					if (!String.IsNullOrEmpty(effectDescription.Target))
					{
						sb.AppendFormat(" {0}", effectDescription.Target);
					}
					if (!String.IsNullOrEmpty(effectDescription.Discriminator))
					{
						sb.Append("{2}");
						sb.AppendFormat("{0}", effectDescription.Discriminator);
					}
					if (!String.IsNullOrEmpty(effectDescription.Condition_Name))
					{
						sb.Append(" {1}");
						//sb.AppendFormat(" {0}", effectDescription.Condition);
					}
					if (!String.IsNullOrEmpty(effectDescription.ConditionTarget))
					{
						sb.AppendFormat(" {0}", effectDescription.ConditionTarget);
					}
					effectDescription.FormattedDescription = sb.ToString();

					this.effectDescriptions[entry] = effectDescription;
					effectDescriptionList.Add(effectDescription);
				}
				else
				{
					effectDescription = this.effectDescriptions[entry];
				}

				T effect = new T();
				effect.EffectDescription_Text = effectDescription.Text;
				effect.EffectNo = effectNo++;
				action(effect);
				effects.Add(effect);
				remainder = remainder.XRegexReplace(pattern, "${Remainder}", RegexReplaceEmptyResultBehaviors.Ignore);
			}
			return effects;
		}

		private List<FeatEffect> GetFeatEffects(Feat feat, string effectType, string effectsText)
		{
			List<FeatEffect> effects = new List<FeatEffect>();
			foreach (FeatEffect effect in this.GetEffects<FeatEffect>(effectsText, effectType, (x) => { x.Feat_Name = feat.Name; x.EffectType = effectType; }))
				effects.Add(effect);
			return effects;
		}

		private List<FeatRankEffect> GetFeatRankEffects(FeatRank featRank, string effectsText)
		{
			List<FeatRankEffect> effects = new List<FeatRankEffect>();
			foreach (FeatRankEffect effect in this.GetEffects<FeatRankEffect>(effectsText, null, (x) => { x.Feat_Name = featRank.Feat_Name; x.Feat_Rank = featRank.Rank; }))
				effects.Add(effect);
			return effects;
		}

		private List<T> GetFeatRankFacts<T>(FeatRank featRank, string factsText) where T : IFeatRankFact, new()
		{
			List<T> facts = new List<T>();
			foreach (FactData factData in FactData.GetFacts(factsText))
			{
				T fact = new T();
				fact.Feat_Name = featRank.Feat_Name;
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
			List<T> facts = new List<T>();
			foreach (FactData factData in factsText)
			{
				T fact = new T();
				fact.Achievement_Name = achievementRank.Achievement_Name;
				fact.Achievement_Rank = achievementRank.Rank;
				if (fact is IAchievementRankCategoryBonus)
				{
					((IAchievementRankCategoryBonus)fact).BonusNo = factData.FactNo;
					((IAchievementRankCategoryBonus)fact).Category_Name = factData.Name;
					((IAchievementRankCategoryBonus)fact).Bonus = int.Parse(factData.Value);
				}
				else if (fact is IAchievementRankRequirement)
				{
					((IAchievementRankRequirement)fact).RequirementNo = factData.FactNo;
					((IAchievementRankRequirement)fact).OptionNo = factData.OptionNo;
					if (fact is IAchievementRankFeatRequirement)
					{
						((IAchievementRankFeatRequirement)fact).Feat_Name = factData.Name;
						((IAchievementRankFeatRequirement)fact).Feat_Rank = int.Parse(factData.Value);
					}
					else if (fact is IAchievementRankFlagRequirement)
					{
						((IAchievementRankFlagRequirement)fact).Flag_Name = factData.Name;
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
			List<RecipeOutputItem> recipeOutputItemList = this.GetEntityList<RecipeOutputItem>();
			List<RecipeOutputItemUpgrade> recipeOutputItemUpgradeList = this.GetEntityList<RecipeOutputItemUpgrade>();
			List<RecipeOutputItemUpgradeKeyword> recipeOutputItemUpgradeKeywordList = this.GetEntityList<RecipeOutputItemUpgradeKeyword>();

			recipeOutputItemList.Add(new RecipeOutputItem
			{
				Item_Name = item.Name
			});

			for (int upgrade = 0; upgrade <= 5; upgrade++)
			{
				recipeOutputItemUpgradeList.Add(new RecipeOutputItemUpgrade
				{
					Item_Name = item.Name,
					Upgrade = upgrade
				});
				int keywordNo = 1;
				if (upgrade == 0)
				{
					foreach (string keyword in inherentKeywords)
					{
						recipeOutputItemUpgradeKeywordList.Add(new RecipeOutputItemUpgradeKeyword
						{
							Item_Name = item.Name,
							Upgrade = upgrade,
							KeywordKind_Name = "Inherent",
							KeywordNo = keywordNo++,
							KeywordType_Name = item.ItemType_Name,
							Keyword_Name = keyword
						});
					}
				}
				if (upgradeKeywords.Count > upgrade)
				{
					recipeOutputItemUpgradeKeywordList.Add(new RecipeOutputItemUpgradeKeyword
					{
						Item_Name = item.Name,
						Upgrade = upgrade,
						KeywordKind_Name = "Upgrade",
						KeywordNo = 1,
						KeywordType_Name = item.ItemType_Name,
						Keyword_Name = upgradeKeywords[upgrade].XRegexReplace(@"^(?<Keyword>.*) \(\+[0-3]\).*$", "${Keyword}", RegexReplaceEmptyResultBehaviors.ThrowError)
					});
				}
			}
		}

		#endregion Helper Methods
	}
}
