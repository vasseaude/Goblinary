namespace Goblinary.Website
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Common;
	using System.Data.Entity;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.Profile;
	using System.Web.Security;

	using Goblinary.Common;
	using Goblinary.CharacterData.Model;
	using Goblinary.CharacterData.SqlServer;
	using Goblinary.Web;
	using Goblinary.Website.Controls;
	using Goblinary.WikiData.Model;
	using Goblinary.WikiData.SqlServer;

	public partial class FeatDetails : System.Web.UI.Page
	{
		private class CustomFeatRank
		{
			public class CustomTrainerLevel
			{
				public string Trainer_Name { get; set; }
				public int? Level { get; set; }
			}

			public FeatRank FeatRank { get; set; }
			public bool IsTrained { get; set; }
            public bool IsWished { get; set; }
			public List<CustomTrainerLevel> TrainerLevels { get; set; }

			public string Feat_Name { get { return this.FeatRank.Feat_Name; } }
			public int? Rank { get { return this.FeatRank.Rank; } }
			public int? ExpCost { get { return this.FeatRank.ExpCost; } }
			public int? CoinCost { get { return this.FeatRank.CoinCost; } }
			public Feat Feat { get { return this.FeatRank.Feat; } }
			public List<FeatRankAbilityBonus> AbilityBonuses { get { return this.FeatRank.AbilityBonuses; } }
			public List<FeatRankAbilityRequirement> AbilityRequirements { get { return this.FeatRank.AbilityRequirements; } }
			public List<FeatRankAchievementRequirement> AchievementRequirements { get { return this.FeatRank.AchievementRequirements; } }
			public List<FeatRankCategoryRequirement> CategoryRequirements { get { return this.FeatRank.CategoryRequirements; } }
			public List<FeatRankFeatRequirement> FeatRequirements { get { return this.FeatRank.FeatRequirements; } }
			public List<FeatRankEffect> Effects { get { return this.FeatRank.Effects; } }
			public List<FeatRankKeyword> Keywords { get { return this.FeatRank.Keywords; } }
		}

		internal static string GetLink(Feat feat)
		{
			return string.Format("<a href=\"/FeatDetails?feat={0}\">{0}</a>", feat.Name);
		}

		internal static string GetFeatEffect(FeatEffect effect)
		{
			return string.Format(effect.EffectDescription.FormattedDescription,
				EffectDetails.GetLink(effect.EffectDescription.Effect, effect.EffectType),
				"",
				"<br/>&nbsp;&nbsp;&nbsp;&nbsp;");
		}

		internal static string GetFeatRankAbilityBonus(FeatRankAbilityBonus abilityBonus)
		{
			return string.Format("{0} {1}", AbilityDetails.GetLink(abilityBonus.Ability), abilityBonus.Bonus);
		}

		internal static string GetFeatRankAbilityRequirement(FeatRankAbilityRequirement abilityRequirement)
		{
			return string.Format("{0} {1}", AbilityDetails.GetLink(abilityRequirement.Ability), abilityRequirement.Value);
		}

		internal static string GetFeatRankAchievementRequirement(FeatRankAchievementRequirement achievementRequirement)
		{
			return string.Format("{0} {1}", AchievementDetails.GetLink(achievementRequirement.AchievementRank.Achievement), achievementRequirement.Achievement_Rank);
		}

		internal static string GetFeatRankCategoryRequirement(FeatRankCategoryRequirement categoryRequirement)
		{
			return string.Format("{0} {1}", CategoryDetails.GetLink(categoryRequirement.Category_Name), categoryRequirement.Value);
		}

		internal static string GetFeatRankFeatRequirement(FeatRankFeatRequirement featRequirement)
		{
			return string.Format("{0} {1}", FeatDetails.GetLink(featRequirement.RequiredFeatRank.Feat), featRequirement.RequiredFeat_Rank);
		}

		internal static string GetFeatRankEffect(FeatRankEffect effect)
		{
			return string.Format(effect.EffectDescription.FormattedDescription,
				EffectDetails.GetLink(effect.EffectDescription.Effect),
				"",
				"<br/>&nbsp;&nbsp;&nbsp;&nbsp;");
		}

		internal static string GetFeatRankKeyword(FeatRankKeyword keyword)
		{
			return KeywordDetails.GetLink(keyword.Keyword);
		}

		private string featName;
		private Feat feat;
		private string providerUserKey = "";
		private Character character = null;
		private CharacterFeatRank characterFeatRank = null;
        private int profileCharacterID;
        private ProfileBase userProfile;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.featName = HttpUtility.UrlDecode(Request.QueryString["feat"]);
			using (WikiDataContext context = new WikiDataContext())
			{
				using (CharacterDataContext characterDataContext = new CharacterDataContext())
				{
					var feats = (
						from f in context.Set<Feat>()
							.Include("Ranks.Effects")
							.Include("Ranks.Keywords.Keyword")
							.Include("Ranks.AbilityBonuses.Ability")
							.Include("Ranks.AbilityRequirements.Ability")
							.Include("Ranks.AchievementRequirements.AchievementRank.Achievement")
							.Include("Ranks.FeatRequirements.RequiredFeatRank.Feat")
						where f.Name == this.featName
						select f).ToList();
					feat = feats[0];

                    if (User.Identity.IsAuthenticated && Membership.GetUser() != null)
					{
						this.providerUserKey = Membership.GetUser().ProviderUserKey.ToString();
                        this.userProfile = ProfileBase.Create(Membership.GetUser().UserName); // get profile
                        this.profileCharacterID = (int)userProfile.GetPropertyValue("SelectedCharacterID"); // get character name from profile
						this.character = (
								from c in characterDataContext.Characters
									.Include("FeatRanks")
									.Include("AchievementRanks")
                                where c.User_ID == providerUserKey && c.ID == profileCharacterID
								select c
							).FirstOrDefault();
						this.characterFeatRank = (
								from cfr in this.character.FeatRanks
								where cfr.Feat_Name == feat.AdvancementFeat_Name
								select cfr
							).FirstOrDefault();
					}

					Page.MetaDescription += String.Format(" Feat details and ranks for {0} - a {1}.", feat.Name, feat.FeatType.DisplayName);
					Page.Title = feat.Name + " | Feat";
					featTitle.InnerHtml = feat.Name; // set the main header

					Label dd = new Label();
					// temporary until we can get a real description from the database
					dd.Text = String.Format("Goblinworks has not provided a description for type: <b><a href=\"/FeatList?type={0}\">{1}</a></b>", HttpUtility.UrlEncode(feat.FeatType.Name), feat.FeatType.DisplayName);
					FeatBlock.Controls.Add(dd);

					DetailsView detailsControl = (DetailsView)Page.LoadControl("~/Controls/Feat/" + feat.FeatType.Name + ".ascx").FindControl("FeatDetailsView");
					detailsControl.DataSource = feats;
					detailsControl.DataBind();
					detailsControl.HeaderRow.TableSection = TableRowSection.TableHeader;
					FeatBlock.Controls.Add(detailsControl);

					if (feat.AdvancementFeat != null) // Consumables don't have AdvancementFeat - check for null before proceeding.
					{
						if (feat.AdvancementFeat.Feats.Count() > 1)
						{
							Label advancementNotesLabel = new Label();
							advancementNotesLabel.Text = "Note:  All Related Feats are automatically acquired when the Advancement Feat is trained, meaning that Costs must only be paid once, and Bonuses are only earned once.";
							advancementNotesLabel.Style.Value = "background-color: #FFE0A3;";
							FeatBlock.Controls.Add(advancementNotesLabel);

                            Label trainerNotesLabel = new Label();
                            trainerNotesLabel.Text = "<br/>Note: This feat may be listed on trainer(s) by its Advancement Feat.";
                            trainerNotesLabel.Style.Value = "background-color: #F3D3FF;";
                            FeatBlock.Controls.Add(trainerNotesLabel);
						}
					}

					if (feat.Effects.Count() > 0)
					{
						ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
						featEffectsListView.DataSource = feat.Effects;
						featEffectsListView.DataSource = (
								from fe in feat.Effects
								orderby FeatEffects.GetEffectTypeSortOrder(fe.EffectType)
								select new
								{
									EffectType = fe.EffectType,
									Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, fe.EffectType)
								}
							).ToList();
						featEffectsListView.DataBind();
						PlaceHolder FeatEffectsPlaceholder = (PlaceHolder)detailsControl.FindControl("FeatEffectsPlaceholder");
						if (FeatEffectsPlaceholder != null)
						{
							FeatEffectsPlaceholder.Controls.Add(featEffectsListView);
						}
					}

					ListView relatedFeatsListView = (ListView)detailsControl.FindControl("RelatedFeatsListView");
					if (relatedFeatsListView != null)
					{
						relatedFeatsListView.DataSource = (
								from f in feat.AdvancementFeat.Feats
								where f.Name != feat.Name
								orderby f.Name
								select f
							);
						relatedFeatsListView.DataBind();
					}

					if (feat.Ranks.Count() > 0)
					{
						GridView gridControl = (GridView)Page.LoadControl("~/Controls/FeatDetailsControls.ascx").FindControl("FeatRanksGridView");
						gridControl.DataSource = (
								from fr in feat.Ranks
								select new CustomFeatRank
								{
									FeatRank = fr,
									IsTrained = this.characterFeatRank != null && this.characterFeatRank.TrainedRank >= fr.Rank,
									IsWished = this.characterFeatRank != null && this.characterFeatRank.WishListRank >= fr.Rank,
									TrainerLevels = (
											from frtl in fr.Feat.AdvancementFeat.FeatRankTrainerLevels
											where frtl.Feat_Rank >= fr.Rank
											group frtl by frtl.Trainer_Name into trainers
											select new CustomFeatRank.CustomTrainerLevel
											{
												Trainer_Name = trainers.Key,
												Level = trainers.Min(x => x.Level)
											}
										).ToList()
								}
							).ToList();
						gridControl.RowDataBound += new GridViewRowEventHandler(gridControl_RowDataBound);
						gridControl.DataBind();
						gridControl.HeaderRow.TableSection = TableRowSection.TableHeader;
						gridControl.Attributes.Add("tableName", "a");
						RanksBlock.Controls.Add(gridControl);
					}
				}
			}
			// insert search notes
			Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
			notes.Controls.Add(notesTable);
		}

		protected void gridControl_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				var currentFeatRank = (CustomFeatRank)e.Row.DataItem;

				if (User.Identity.IsAuthenticated)
				{
					CheckBox featRankTrainedCheckBox = new CheckBox();
					featRankTrainedCheckBox.AutoPostBack = true;
					featRankTrainedCheckBox.Attributes.Add("onclick", "ShowProgress();");
					featRankTrainedCheckBox.CheckedChanged += featRankTrainedCheckBox_CheckedChanged;
					featRankTrainedCheckBox.ID = currentFeatRank.Rank.ToString() + "|t";
					featRankTrainedCheckBox.Checked = currentFeatRank.IsTrained;
					e.Row.Cells[0].Controls.AddAt(0, featRankTrainedCheckBox);

					CheckBox featRankWishListCheckBox = new CheckBox();
					featRankWishListCheckBox.AutoPostBack = true;
					featRankWishListCheckBox.CheckedChanged += featRankWishListCheckBox_CheckedChanged;
					featRankWishListCheckBox.ID = currentFeatRank.Rank.ToString() + "|w";
					featRankWishListCheckBox.Checked = currentFeatRank.IsWished;
					e.Row.Cells[1].Controls.AddAt(0, featRankWishListCheckBox);
				}

				if (currentFeatRank.Feat.AdvancementFeat.Feats.Count() > 1)
				{
					((DataControlFieldCell)e.Row.FindControl("ExpCostLabel").Parent.Parent).Style.Value = "background-color: #FFE0A3;";
					((DataControlFieldCell)e.Row.FindControl("CoinCostLabel").Parent.Parent).Style.Value = "background-color: #FFE0A3;";
					((DataControlFieldCell)e.Row.FindControl("RankAbilityBonusesListView").Parent).Style.Value = "background-color: #FFE0A3;";
					((DataControlFieldCell)e.Row.FindControl("TrainerLevelsListView").Parent).Style.Value = "background-color: #F3D3FF;";
				}

				ListView trainerLevelsListView = (ListView)e.Row.FindControl("TrainerLevelsListView");
				trainerLevelsListView.DataSource = (
						from tl in currentFeatRank.TrainerLevels
						select new
						{
							Trainer = tl.Trainer_Name,
							Level = tl.Level
						}
					);
				trainerLevelsListView.DataBind();

				ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
				featEffectsListView.DataSource = (
						from fre in currentFeatRank.Effects
						select new
						{
							EffectType = "FeatRank",
							Description = FeatEffects.GetFeatEffectDescription(fre.EffectDescription, "FeatRank")
						}
					);
				featEffectsListView.DataBind();
				PlaceHolder featRankEffectsPlaceholder = (PlaceHolder)e.Row.FindControl("FeatRankEffectsPlaceholder");
				featRankEffectsPlaceholder.Controls.Add(featEffectsListView);

				ListView featKeywordsListView = (ListView)e.Row.FindControl("FeatKeywordsListView");
				featKeywordsListView.DataSource = currentFeatRank.Keywords;
				featKeywordsListView.DataBind();

				ListView rankAbilityBonusesListView = (ListView)e.Row.FindControl("RankAbilityBonusesListView");
				rankAbilityBonusesListView.DataSource = (
					from ab in currentFeatRank.AbilityBonuses
					orderby ab.BonusNo, ab.OptionNo
					select new
					{
						Bonus = String.Format("{0}<a href=\"/AbilityDetails?ability={1}\">{2}</a> {3}", (ab.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ab.Ability_Name), ab.Ability_Name, ab.Bonus)
					});
				rankAbilityBonusesListView.DataBind();

				ListView rankAbilityRequirementsListView = (ListView)e.Row.FindControl("RankAbilityRequirementsListView");
				rankAbilityRequirementsListView.DataSource = (
					from ar in currentFeatRank.AbilityRequirements
					orderby ar.RequirementNo, ar.OptionNo
					select new
					{
						Requirement = String.Format("{0}<a href=\"/AbilityDetails?ability={1}\">{2}</a> {3}", (ar.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ar.Ability_Name), ar.Ability_Name, ar.Value)
					});
				rankAbilityRequirementsListView.DataBind();

				if (currentFeatRank.AchievementRequirements.Count > 0)
				{
					ListView rankAchievementRequirementsListView = (ListView)e.Row.FindControl("RankAchievementRequirementsListView");
					rankAchievementRequirementsListView.DataSource = (
						from ar in currentFeatRank.AchievementRequirements
						where ar.AchievementRank != null
						orderby ar.RequirementNo, ar.OptionNo
						select new
						{
							Requirement = String.Format("{0}<a href=\"/AchievementDetails?achievement={1}\">{2}</a>{3}", (ar.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(ar.AchievementRank.Achievement.AchievementGroup.Name),
								ar.AchievementRank.Achievement is CraftAchievement ? ar.AchievementRank.DisplayName : ar.AchievementRank.Achievement.AchievementGroup_Name,
								ar.AchievementRank.Achievement is CraftAchievement ? "" : " " + ar.Achievement_Rank.ToString()
							)
						});
					rankAchievementRequirementsListView.DataBind();
				}

				ListView rankCategoryRequirementsListView = (ListView)e.Row.FindControl("RankCategoryRequirementsListView");
				rankCategoryRequirementsListView.DataSource = (
					from cr in currentFeatRank.CategoryRequirements
					orderby cr.RequirementNo, cr.OptionNo
					select new
					{
						Requirement = String.Format("{0}<a href=\"/CategoryDetails?category={1}\">{2}</a> {3}", (cr.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(cr.Category_Name), cr.Category_Name, cr.Value)
					});
				rankCategoryRequirementsListView.DataBind();

				ListView rankFeatRequirementsListView = (ListView)e.Row.FindControl("RankFeatRequirementsListView");
				rankFeatRequirementsListView.DataSource = (
					from fr in currentFeatRank.FeatRequirements
					orderby fr.RequirementNo, fr.OptionNo
					select new
					{
						Requirement = String.Format("{0}<a href=\"/FeatDetails?feat={1}\">{2}</a> {3}", (fr.OptionNo == 1 ? "" : " \u21B3 or "), HttpUtility.UrlEncode(fr.RequiredFeat_Name), fr.RequiredFeat_Name, fr.RequiredFeat_Rank)
					});
				rankFeatRequirementsListView.DataBind();
			}
		}

		protected void featRankTrainedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (User.Identity.IsAuthenticated)
			{
				CheckBox featRankTrainedCheckBox = (CheckBox)sender;
				int rank = Convert.ToInt32(featRankTrainedCheckBox.ID.Split('|').First());

				using (CharacterDataContext characterDataContext = new CharacterDataContext())
				{
					EntityState state = EntityState.Modified;
					if (this.characterFeatRank == null)
					{
						this.characterFeatRank = new CharacterFeatRank();
						this.characterFeatRank.Character_ID = this.character.ID;
						this.characterFeatRank.Feat_Name = this.feat.AdvancementFeat_Name;
						state = EntityState.Added;
					}
					this.characterFeatRank.TrainedRank = featRankTrainedCheckBox.Checked ? rank : rank - 1;
					//if (this.characterFeatRank.TrainedRank < this.minFeatRank)
					//{
					//    state = EntityState.Deleted;
					//}
					characterDataContext.Entry(this.characterFeatRank).State = state;
					characterDataContext.SaveChanges();
				}

				Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
				Response.Redirect(Request.RawUrl); // force a reload of the page - forces the RowDataBound event to fire again, and clears/checks boxes as needed
			}
		}

		protected void featRankWishListCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (User.Identity.IsAuthenticated)
			{
				CheckBox featRankWishListCheckBox = (CheckBox)sender;
				int rank = Convert.ToInt32(featRankWishListCheckBox.ID.Split('|').First());

				using (CharacterDataContext characterDataContext = new CharacterDataContext())
				{
					EntityState state = EntityState.Modified;
					if (this.characterFeatRank == null)
					{
						this.characterFeatRank = new CharacterFeatRank();
						this.characterFeatRank.Character_ID = this.character.ID;
						this.characterFeatRank.Feat_Name = this.feat.AdvancementFeat_Name;
						state = EntityState.Added;
					}
					this.characterFeatRank.WishListRank = featRankWishListCheckBox.Checked ? rank : rank - 1;
					//if (this.characterFeatRank.WishListRank < this.minFeatRank)
					//{
					//    state = EntityState.Deleted;
					//}
					characterDataContext.Entry(this.characterFeatRank).State = state;
					characterDataContext.SaveChanges();
				}

				Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
				Response.Redirect(Request.RawUrl); // force a reload of the page - forces the RowDataBound event to fire again, and clears/checks boxes as needed
			}
		}
	}
}