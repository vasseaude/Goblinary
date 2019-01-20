namespace Goblinary.ModelBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class WikiData : IWikiData
	{
		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode { get { return XmlWriteMode.IgnoreSchema; } }
		public XmlReadMode ReadMode { get { return XmlReadMode.IgnoreSchema; } }
		public LookupData LookupData { get; set; }
		public SourceData SourceData { get; set; }
		public StockData StockData { get; set; }

		public void Import()
		{
			if (!this.LookupData.IsLoaded)
			{
				this.LookupData.XRead();
			}
			if (!this.SourceData.IsLoaded)
			{
				this.SourceData.XRead();
			}
			if (!this.StockData.IsLoaded)
			{
				this.StockData.XRead();
			}
			this.Clear();
			this.IsLoaded = false;
			try
			{
				this.ImportAll();
			}
			catch (Exception ex)
			{
				var errors = this.XGetErrors();
				throw ex;
			}
			this.IsLoaded = true;
			this.XSave();
		}

		private void ImportAll()
		{
			this.ImportLookups();
			this.ImportAdvancements();
			this.ImportFeats();
			this.ImportAchievements();
			this.ImportAdvancementRankFacts();
			this.ImportAchievementLevelFacts();

			// TODO:  You just finished making ActiveFeatRankKeywords and ArmorFeatRankKeywords.  Need to load Effects and Keywords for Feat types.  Need to make sure it works in the Model, too.
		}

		private void ImportLookups()
		{
			foreach (var lookup in this.LookupData.Lookups)
			{
				switch (lookup.Type)
				{
					case "Ability":
						this.Abilities.AddAbilitiesRow(lookup.Name);
						break;
					case "AchievementCategory":
						this.AchievementCategories.AddAchievementCategoriesRow(lookup.Name);
						break;
					case "AttackForm":
						this.AttackForms.AddAttackFormsRow(lookup.Name);
						break;
					case "EffectChannel":
						this.EffectChannels.AddEffectChannelsRow(lookup.Name);
						break;
					case "Role":
						this.Roles.AddRolesRow(lookup.Name);
						break;
					case "School":
						this.Schools.AddSchoolsRow(lookup.Name);
						break;
					case "SpecificWeapon":
						this.SpecificWeapons.AddSpecificWeaponsRow(lookup.Name);
						break;
					case "WeaponCategory":
						this.WeaponCategories.AddWeaponCategoriesRow(lookup.Name);
						break;
				}
			}
		}

		private void ImportAdvancements()
		{
			foreach (var advancement in
				from w in this.LookupData.SourceWorksheets
				where w.Action == "Advancement"
				from DataTable t in this.SourceData.Tables
				where t.TableName == w.WorksheetName
				from DataRow r in t.Rows
				select new
				{
					Worksheet = w,
					DataTable = t,
					Row = r
				})
			{
				AdvancementsRow advancementsRow = this.Advancements.AddAdvancementsRow(advancement.Row["SlotName"].ToString());
				int rank = 0;
				int columnsPerLevel = 7;
				while (++rank * columnsPerLevel + 1 <= advancement.DataTable.Columns.Count)
				{
					int offset = 1 + (rank - 1) * columnsPerLevel;
					AdvancementRanksRow ranksRow = this.AdvancementRanks.NewAdvancementRanksRow();
					ranksRow.Advancement_Name = advancementsRow.Name;
					ranksRow.Rank = rank;
					if (advancement.Row[offset] == DBNull.Value)
					{
						break;
					}
					ranksRow.ExpCost = int.Parse(advancement.Row[offset].ToString());
					ranksRow.CoinCost = int.Parse(advancement.Row[offset + 1].ToString());
					this.AdvancementRanks.AddAdvancementRanksRow(ranksRow);
					this.AdvancementRankFactStrings.AddAdvancementRankFactStringsRow(ranksRow.Advancement_Name, ranksRow.Rank, "CategoryRequirement", advancement.Row[offset + 2].ToString());
					this.AdvancementRankFactStrings.AddAdvancementRankFactStringsRow(ranksRow.Advancement_Name, ranksRow.Rank, "AdvancementRequirement", advancement.Row[offset + 3].ToString());
					this.AdvancementRankFactStrings.AddAdvancementRankFactStringsRow(ranksRow.Advancement_Name, ranksRow.Rank, "AchievementRequirement", advancement.Row[offset + 4].ToString());
					this.AdvancementRankFactStrings.AddAdvancementRankFactStringsRow(ranksRow.Advancement_Name, ranksRow.Rank, "AbilityRequirement", advancement.Row[offset + 5].ToString());
					this.AdvancementRankFactStrings.AddAdvancementRankFactStringsRow(ranksRow.Advancement_Name, ranksRow.Rank, "AbilityBonus", advancement.Row[offset + 6].ToString());
				}
			}
			foreach (var rank in this.LookupData.AdvancementRanks)
			{
				var advRow = this.Advancements.AddAdvancementsRow(rank.Advancement_Name);
				this.AdvancementRanks.AddAdvancementRanksRow(advRow, int.Parse(rank.Rank), int.Parse(rank.ExpCost), int.Parse(rank.CoinCost));
			}
		}

		private void ImportFeats()
		{
			FeatsRow featsRow = null;
			foreach (var feat in
				from w in this.LookupData.SourceWorksheets
				where w.Action == "Feat"
				from DataTable t in this.SourceData.Tables
				where t.TableName == w.WorksheetName
				from DataRow r in t.Rows
				from l in this.LookupData.FeatTypeLookups
					.Where(x => x.Feat_Name == r["featName"].ToString())
					.DefaultIfEmpty()
				orderby w.Type
				select new
				{
					Worksheet = w,
					DataTable = t,
					Row = r,
					Lookup = l
				})
			{
				string name = feat.Row["featName"].ToString();
				if (featsRow == null || featsRow.Name != name)
				{
					#region Add Feats Row

					featsRow = this.Feats.NewFeatsRow();
					featsRow.Name = feat.Row["featName"].ToString();
					if (feat.Worksheet.Type2 == "Upgrade")
					{
						featsRow.FeatType = feat.Lookup.FeatType;
					}
					else
					{
						featsRow.FeatType = feat.Worksheet.Type2;
					}
					if (feat.Worksheet.Type == "PowerAttack")
					{
						featsRow.Role_Name = "General";
					}
					else
					{
						featsRow.Role_Name = feat.Row["role"].ToString();
					}
					if (feat.Worksheet.Type2 != "Consumable")
					{
						featsRow.Advancement_Name = feat.Row["advancementLink"].ToString();
					}
					this.Feats.AddFeatsRow(featsRow);

					if (feat.Worksheet.Type == "ChanneledFeat")
					{
						ChanneledFeatsRow chanRow = this.ChanneledFeats.NewChanneledFeatsRow();
						chanRow.Feat_Name = featsRow.Name;
						chanRow.Channel_Name = feat.Row["channel"].ToString();
						this.ChanneledFeats.AddChanneledFeatsRow(chanRow);

						if (featsRow.FeatType == "ArmorFeat")
						{
							this.ArmorFeats.AddArmorFeatsRow(chanRow);
						}
						else if (featsRow.FeatType == "AttackBonus")
						{
							this.AttackBonuses.AddAttackBonusesRow(chanRow);
						}
						else if (featsRow.FeatType == "CraftingSkill")
						{
							this.CraftingSkills.AddCraftingSkillsRow(chanRow);
						}
						else if (featsRow.FeatType == "RefiningSkill")
						{
							this.RefiningSkills.AddRefiningSkillsRow(chanRow);
						}
					}
					else
					{
						ActiveFeatsRow actRow = this.ActiveFeats.NewActiveFeatsRow();
						actRow.Feat_Name = featsRow.Name;
						actRow.DamageFactor = decimal.Parse(feat.Row["damageFactor"].ToString());
						actRow.AttackSeconds = decimal.Parse(feat.Row["attackSeconds"].ToString());
						actRow.StaminaCost = int.Parse(feat.Row["staminaCost"].ToString());
						string rangeColumnName = "range";
						if (feat.Worksheet.Type == "PowerAttack")
						{
							rangeColumnName = "featRange";
						}
						actRow.IsMelee = feat.Row[rangeColumnName].ToString() == "Melee";
						if (!actRow.IsMelee)
						{
							actRow.Range = decimal.Parse(feat.Row[rangeColumnName].ToString().XRegexReplace(@"^(?<Range>[0-9\.]+)m$", "${Range}", RegexReplaceEmptyResultBehaviors.ThrowError));
						}
						actRow.WeaponCategory_Name = feat.Row["weaponCategory"].ToString();
						this.ActiveFeats.AddActiveFeatsRow(actRow);

						if (feat.Worksheet.Type == "PowerAttack")
						{
							this.PowerAttacks.AddPowerAttacksRow(
								actRow,
								int.Parse(feat.Row["powerCost"].ToString()),
								int.Parse(feat.Row["level"].ToString()),
								feat.Row["endOfCombatCooldown"].ToString().ToLower() == "yes",
								this.AttackBonuses.FindByFeat_Name(feat.Row["attackBonus"].ToString() + " Attack Bonus"));
						}
						else
						{
							var stdRow = this.StandardAttacks.AddStandardAttacksRow(
								actRow,
								this.AttackForms.FindByName(feat.Row["form"].ToString()),
								decimal.Parse(feat.Row["cooldownSeconds"].ToString()),
								this.SpecificWeapons.FindByName(feat.Row["specificWeapon"].ToString()));

							if (featsRow.FeatType == "Cantrip")
							{
								this.Cantrips.AddCantripsRow(
									stdRow,
									this.Schools.FindByName(feat.Row["school"].ToString()));
							}
						}
					}

					#endregion Add Feats Row
				}
			}
		}

		private void ImportAchievements()
		{
			AchievementsRow achRow = null;
			foreach (var achievement in
				from w in this.LookupData.SourceWorksheets
				where w.Action == "Achievement"
				from DataTable t in this.SourceData.Tables
				where t.TableName == w.WorksheetName
				from DataRow r in t.Rows
				select new
				{
					Worksheet = w,
					DataTable = t,
					Row = r
				})
			{
				string name = achievement.Row["slotname"].ToString();
				if (name == "TestHill")
				{
					continue;
				}
				if (achRow == null || achRow.Name != name)
				{
					#region Add Achievements Row

					achRow = this.Achievements.NewAchievementsRow();
					achRow.Name = name;
					achRow.AchievementType = achievement.Worksheet.Type;
					switch (achievement.Worksheet.Type)
					{
						case "InteractionAchievement":
						case "NPCKillAchievement":
						case "PlayerKillAchievement":
						case "WeaponKillAchievement":
							achRow.DisplayName = achievement.Row["displayname"].ToString().XRegexReplace(@"^(?<SlotName>.*?)( [0-9]+)?$", "${SlotName}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "RoleAchievement":
							achRow.DisplayName = achievement.Row["displayname"].ToString().XRegexReplace(@"^(?<SlotName>.*?) Level [0-9]+?$", "${SlotName}", RegexReplaceEmptyResultBehaviors.ThrowError);
							break;
						case "SettlementLocationAchievement":
						case "SpecialLocationAchievement":
							achRow.DisplayName = achievement.Row["displayname"].ToString();
							break;
						case "CraftingAchievement":
						case "RefiningAchievement":
							achRow.DisplayName = string.Format("{0} (Tier {1}, {2})", achievement.Row["figurefeat"], achievement.Row["figuretier"], achievement.Row["figurerarity"]);
							break;
						default:
							throw new Exception("Unexpected Achievement Type: " + achievement.Worksheet.Type);
					}
					string influenceGain = achievement.Row["influencegain"].ToString().ToLower();
					switch (influenceGain)
					{
						case "always":
							achRow.AlwaysGainsInfluence = true;
							break;
						case "once":
							achRow.AlwaysGainsInfluence = false;
							break;
						default:
							throw new Exception("Unexpected Influence Gain: " + influenceGain);
					}
					switch (achievement.Worksheet.Type)
					{
						case "RoleAchievement":
							achRow.Description = string.Format("Purchase {0}-approriate Advancement Feats.", achievement.Row["slotname"]);
							break;
						case "InteractionAchievement":
						case "PlayerKillAchievement":
						case "WeaponKillAchievement":
						case "SettlementLocationAchievement":
							achRow.Description = achievement.Row["description"].ToString();
							break;
						case "SpecialLocationAchievement":
							achRow.Description = string.Format("Visit the location, {0}.", name);
							break;
						case "NPCKillAchievement":
							achRow.Description = string.Format("Kill {0} NPCs.", achRow.DisplayName.XRegexReplace("^(?<Race>.*) Slayer$", "${Race}", RegexReplaceEmptyResultBehaviors.ThrowError));
							break;
						case "CraftingAchievement":
						case "RefiningAchievement":
							achRow.Description = string.Format("Craft items with {0} that are Tier {1} and {2}.", achievement.Row["figurefeat"], achievement.Row["figuretier"], achievement.Row["figurerarity"]);
							break;
						default:
							throw new Exception("Unexpected Achievement Type: " + achievement.Worksheet.Type);
					}
					this.Achievements.AddAchievementsRow(achRow);

					if (achRow.AchievementType == "RoleAchievement")
					{
						this.RoleAchievements.AddRoleAchievementsRow(
							achRow,
							this.Roles.FindByName(achievement.Row["slotName"].ToString()));
					}
					else if (achRow.AchievementType == "CraftingAchievement" || achRow.AchievementType == "RefiningAchievement")
					{
						var craftRow = this.CraftAchievements.AddCraftAchievementsRow(
							achRow,
							int.Parse(achievement.Row["figuretier"].ToString()),
							achievement.Row["figurerarity"].ToString());
						if (achRow.AchievementType == "CraftingAchievement")
						{
							this.CraftingAchievements.AddCraftingAchievementsRow(
								craftRow,
								this.CraftingSkills.FindByFeat_Name(achievement.Row["figurefeat"].ToString()));
						}
						else
						{
							this.RefiningAchievements.AddRefiningAchievementsRow(
								craftRow,
								this.RefiningSkills.FindByFeat_Name(achievement.Row["figurefeat"].ToString()));
						}
					}

					#endregion Add Achievements Row
				}

				AchievementLevelsRow levelsRow = this.AchievementLevels.NewAchievementLevelsRow();
				levelsRow.Achievement_Name = achRow.Name;
				levelsRow.Level = int.Parse(achievement.Row["level"].ToString());
				levelsRow.DisplayName = achievement.Row["displayname"].ToString();
				levelsRow.Description = achievement.Row["description"].ToString();
				levelsRow.AchievementLevelType = achievement.Worksheet.Type2;
				this.AchievementLevels.AddAchievementLevelsRow(levelsRow);

				if (levelsRow.AchievementLevelType == "CounterAchievementLevel")
				{
					this.CounterAchievementLevels.AddCounterAchievementLevelsRow(
						levelsRow.Achievement_Name,
						levelsRow.Level,
						int.Parse(achievement.Row["countervalue"].ToString()));
				}
				else if (levelsRow.AchievementLevelType == "CraftAchievementLevel")
				{
					this.CraftAchievementLevels.AddCraftAchievementLevelsRow(
						levelsRow.Achievement_Name,
						levelsRow.Level,
						int.Parse(achievement.Row["plus"].ToString()));
				}

				if (achRow.AchievementType == "RoleAchievement")
				{
					this.AchievementLevelFactStrings.AddAchievementLevelFactStringsRow(
						levelsRow.Achievement_Name,
						levelsRow.Level,
						"AdvancementRequirement",
						achievement.Row["featReqList"].ToString());
				}
				else
				{
					this.AchievementLevelFactStrings.AddAchievementLevelFactStringsRow(
						levelsRow.Achievement_Name,
						levelsRow.Level,
						"CategoryBonus",
						FactData.GetFactString(achievement.Row,from c in this.AchievementCategories select c.Name));
				}
			}
		}

		private void ImportAdvancementRankFacts()
		{
			foreach (var rank in
				from fs in this.AdvancementRankFactStrings
				from f in FactData.GetFacts(fs.FactString)
				from cat in this.AchievementCategories
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				from adv in this.Advancements
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				from ach in this.Achievements
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				from abil in this.Abilities
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				select new
				{
					fs.Advancement_Name,
					fs.Advancement_Rank,
					fs.FactType,
					f.FactNo,
					f.OptionNo,
					f.Name,
					f.Value,
					CatRow = cat,
					AdvRow = adv,
					AchRow = ach,
					AbilRow = abil
				})
			{
				this.AdvancementRankFacts.AddAdvancementRankFactsRow(
					rank.Advancement_Name,
					rank.Advancement_Rank,
					rank.FactType,
					rank.FactNo,
					rank.OptionNo);
				switch (rank.FactType)
				{
					case "CategoryRequirement":
						this.AdvRankCatReqs.AddAdvRankCatReqsRow(
							rank.Advancement_Name,
							rank.Advancement_Rank,
							rank.FactType, rank.FactNo,
							rank.OptionNo, rank.CatRow,
							int.Parse(rank.Value));
						break;
					case "AdvancementRequirement":
						this.AdvRankAdvReqs.AddAdvRankAdvReqsRow(
							rank.Advancement_Name,
							rank.Advancement_Rank,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.AdvRow.Name,
							int.Parse(rank.Value));
						break;
					case "AchievementRequirement":
						this.AdvRankAchReqs.AddAdvRankAchReqsRow(
							rank.Advancement_Name,
							rank.Advancement_Rank,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.AchRow.Name,
							int.Parse(rank.Value));
						break;
					case "AbilityRequirement":
						this.AdvRankAbilReqs.AddAdvRankAbilReqsRow(
							rank.Advancement_Name,
							rank.Advancement_Rank,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.AbilRow,
							int.Parse(rank.Value));
						break;
					case "AbilityBonus":
						this.AdvRankAbilBonuses.AddAdvRankAbilBonusesRow(
							rank.Advancement_Name,
							rank.Advancement_Rank,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.AbilRow,
							decimal.Parse(rank.Value));
						break;
					default:
						throw new Exception("Unexpected FactType: " + rank.FactType);
				}
			}
		}

		private void ImportAchievementLevelFacts()
		{
			foreach (var rank in
				from fs in this.AchievementLevelFactStrings
				from f in FactData.GetFacts(fs.FactString)
				from cat in this.AchievementCategories
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				from adv in this.Advancements
					.Where(x => x.Name.ToLower() == f.Name.ToLower())
					.DefaultIfEmpty()
				select new
				{
					fs.Achievement_Name,
					fs.Achievement_Level,
					fs.FactType,
					f.FactNo,
					f.OptionNo,
					f.Name,
					f.Value,
					CatRow = cat,
					AdvRow = adv
				})
			{
				this.AchievementLevelFacts.AddAchievementLevelFactsRow(
					rank.Achievement_Name,
					rank.Achievement_Level,
					rank.FactType,
					rank.FactNo,
					rank.OptionNo);
				switch (rank.FactType)
				{
					case "AdvancementRequirement":
						this.AchLevelAdvReqs.AddAchLevelAdvReqsRow(
							rank.Achievement_Name,
							rank.Achievement_Level,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.AdvRow.Name,
							int.Parse(rank.Value));
						break;
					case "CategoryBonus":
						this.AchLevelCatBonuses.AddAchLevelCatBonusesRow(
							rank.Achievement_Name,
							rank.Achievement_Level,
							rank.FactType,
							rank.FactNo,
							rank.OptionNo,
							rank.CatRow,
							int.Parse(rank.Value));
						break;
					default:
						throw new Exception("Unexpected FactType: " + rank.FactType);
				}
			}
		}

		private void ImportFeatFacts()
		{
		}

		private void ImportFeatRankFacts()
		{
		}
	}
}