using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;
using System.Web.Security;

using Goblinary.Common;
using Goblinary.Website.Controls;
using Goblinary.CharacterData.Model;
using Goblinary.CharacterData.SqlServer;
using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;

namespace Goblinary.Website
{
	public partial class CharacterDetails : System.Web.UI.Page
	{
		private Character character;
		private string providerUserKey;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (User.Identity.IsAuthenticated)
			{
				this.providerUserKey = Membership.GetUser().ProviderUserKey.ToString();
			}
			using (CharacterDataContext characterDataContext = new CharacterDataContext())
			{
				using (WikiDataContext wikiDataContext = new WikiDataContext())
				{
					if (this.Request.QueryString["character"] != null)
					{
						string characterQuerystring = this.Request.QueryString["character"];
						string seedQuerystring = this.Request.QueryString["seed"];
						int characterTry;
						if (int.TryParse(characterQuerystring, out characterTry))
						{
							this.character = (
									from c in characterDataContext.Characters
										.Include("FeatRanks")
										.Include("AchievementRanks")
									where c.ID == characterTry && (c.ShareStatus == "Public" || (c.ShareStatus == "Shared" && c.ShareSeed == seedQuerystring) || c.User_ID == this.providerUserKey)
									select c
								).FirstOrDefault();
						}
					}
					if (this.character != null) // make sure we actually have a character before proceeding
					{
						Page.MetaDescription += String.Format(" Character details for {0}.", this.character.Name);
						Page.Title = this.character.Name + " | Character";
						characterNameTitle.InnerHtml = this.character.Name; // set the main header

						var characterFeatRanks = (
								from cfr in this.character.FeatRanks
								from f in wikiDataContext.Feats
								where f.AdvancementFeat_Name == cfr.Feat_Name
								orderby f.Name
								group f by cfr into g
								from fr in g.FirstOrDefault().Ranks
								where fr.Rank <= g.Key.TrainedRank || fr.Rank <= g.Key.WishListRank
								group fr by new
								{
									FeatType_Name = g.FirstOrDefault().FeatType.DisplayName,
									AdvancementFeat_Name = g.Key.Feat_Name,
									g.Key.TrainedRank,
									g.Key.WishListRank,
									SampleFeat_Name = g.FirstOrDefault().Name
								} into gr
								let TrainedExpCost = gr.Where(x => x.Rank <= gr.Key.TrainedRank).Sum(x => x.ExpCost)
								let WishListExpCost = gr.Where(x => x.Rank <= (gr.Key.TrainedRank ?? 0) || x.Rank <= gr.Key.WishListRank).Sum(x => x.ExpCost)
								let WishListDeficit = gr.Where(x => x.Rank > (gr.Key.TrainedRank ?? 0) && x.Rank <= gr.Key.WishListRank).Sum(x => x.ExpCost)
								select new
								{
									gr.Key.FeatType_Name,
									gr.Key.AdvancementFeat_Name,
									TrainedRank = gr.Key.TrainedRank > 0 ? gr.Key.TrainedRank : null,
									WishListRank = gr.Key.WishListRank > 0 ? gr.Key.WishListRank : null,
									gr.Key.SampleFeat_Name,
									TrainedExpCost = TrainedExpCost > 0 ? TrainedExpCost : null,
									WishListExpCost = WishListExpCost > 0 ? WishListExpCost : null,
									WishListDeficit = WishListDeficit > 0 ? WishListDeficit : null
								}
							).ToList();

						var characterAbilityScores = (
								from cfr in this.character.FeatRanks
								from f in wikiDataContext.Feats
								where f.AdvancementFeat_Name == cfr.Feat_Name
								orderby f.Name
								group f by cfr into g
								from fr in g.FirstOrDefault().Ranks
								where fr.Rank <= g.Key.TrainedRank || fr.Rank <= g.Key.WishListRank
								from a in wikiDataContext.Set<Ability>()
								from ab in fr.AbilityBonuses
									.Where(x => x.Ability_Name == a.Name)
									.DefaultIfEmpty()
								group new
								{
									g.Key.TrainedRank,
									g.Key.WishListRank,
									fr.Rank,
									Bonus = ab != null ? ab.Bonus : 0
								}
								by new
								{
									Ability_Name = a.Name
								}
									into gab
									select new
									{
										gab.Key.Ability_Name,
										TrainedScore = 10 + gab.Where(x => x.Rank <= x.TrainedRank).Sum(x => x.Bonus),
										WishListScore = 10 + gab.Where(x => x.Rank <= x.TrainedRank).Sum(x => x.Bonus)
											+ gab.Where(x => x.Rank > (x.TrainedRank ?? 0) && x.Rank <= x.WishListRank).Sum(x => x.Bonus),
										WishListDeficit = gab.Where(x => x.Rank > (x.TrainedRank ?? 0) && x.Rank <= x.WishListRank).Sum(x => x.Bonus)
									}
							).ToList();

						GridView abilitiesGridView = (GridView)Page.LoadControl("~/Controls/CharacterDetailsControls.ascx").FindControl("AbilitiesGridView");
						abilitiesGridView.DataSource = characterAbilityScores;
						abilitiesGridView.DataBind();
						AbilityGridDiv.Controls.Add(abilitiesGridView);

						if (character.ShareStatus == "Public")
						{
							if (character.User_ID == this.providerUserKey)
							{
								string characterURL = String.Format("{0}?character={1}", Request.Url.GetLeftPart(UriPartial.Path), character.ID);
								this.ShareStatusDiv.Controls.Add(new Label { Text = "This is a <b>Public</b> character. Anyone with this character's number can view it. Use " });
								this.ShareStatusDiv.Controls.Add(new HyperLink { Text = "this link", NavigateUrl = characterURL });
								this.ShareStatusDiv.Controls.Add(new Label { Text = " to share it." });
							}
							else
							{
								this.ShareStatusDiv.Controls.Add(new Label { Text = "You are viewing a <b>Public</b> character." });
							}
						}
						else if (character.ShareStatus == "Shared")
						{
							if (character.User_ID == this.providerUserKey)
							{
								string characterURL = String.Format("{0}?character={1}&seed={2}", Request.Url.GetLeftPart(UriPartial.Path), character.ID, character.ShareSeed);
								this.ShareStatusDiv.Controls.Add(new Label { Text = "This is a <b>Shared</b> character. Anyone with this character's number & seed can view it. Use " });
								this.ShareStatusDiv.Controls.Add(new HyperLink { Text = "this link", NavigateUrl = characterURL });
								this.ShareStatusDiv.Controls.Add(new Label { Text = " (includes seed) to share it." });
							}
							else
							{
								this.ShareStatusDiv.Controls.Add(new Label { Text = "You are viewing a <b>Shared</b> character." });
							}
						}
						else
						{
							this.ShareStatusDiv.Controls.Add(new Label { Text = "This is a <b>Private</b> character. Only you can view it." });
						}

						Label xpLabel = new Label();
						xpLabel.Text = String.Format("<b>Total Spent XP:</b> {0}", characterFeatRanks.Sum(x => x.TrainedExpCost));
						this.StatsDiv.Controls.Add(xpLabel);
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));

						Label wishListXPLabel = new Label();
						wishListXPLabel.Text = string.Format("<b>Total WishList XP:</b> {0}", characterFeatRanks.Sum(x => x.WishListExpCost));
						this.StatsDiv.Controls.Add(wishListXPLabel);
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));

						Label wishListXPDeficitLabel = new Label();
						wishListXPDeficitLabel.Text = string.Format("<b>Total WishList XP Deficit:</b> {0}", characterFeatRanks.Sum(x => x.WishListDeficit));
						this.StatsDiv.Controls.Add(wishListXPDeficitLabel);
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));

						Label featsTrainedLabel = new Label();
						featsTrainedLabel.Text = String.Format("<b>Total Number of Feats Trained:</b> {0}", characterFeatRanks.Where(x => x.TrainedRank > 0).Count());
						this.StatsDiv.Controls.Add(featsTrainedLabel);
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));

						Label featsWishedLabel = new Label();
						featsWishedLabel.Text = String.Format("<b>Total Number of Feats Wished:</b> {0}", characterFeatRanks.Where(x => x.WishListRank > 0).Count());
						this.StatsDiv.Controls.Add(featsWishedLabel);
						this.StatsDiv.Controls.Add(new LiteralControl("<br/>"));

						GridView featGridView = (GridView)Page.LoadControl("~/Controls/CharacterDetailsControls.ascx").FindControl("FeatGridView");
						if (characterFeatRanks.Count > 0)
						{
							featGridView.DataSource = characterFeatRanks;
							featGridView.DataBind();
							featGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
							featGridView.Attributes.Add("tableName", "a");
							FeatsDiv.Controls.Add(featGridView);
						}

						GridView earnedAchievementGridView = (GridView)Page.LoadControl("~/Controls/CharacterDetailsControls.ascx").FindControl("EarnedAchievementGridView");
						var earnedAchievementRanks = (
								from car in this.character.AchievementRanks
								where car.EarnedRank != null && car.EarnedRank > 0
								select new
								{
									Achievement_Name = car.Achievement_Name,
									EarnedRank = car.EarnedRank
								}
								).ToList();
						if (earnedAchievementRanks.Count() > 0)
						{
							earnedAchievementGridView.DataSource = earnedAchievementRanks;
							earnedAchievementGridView.DataBind();
							earnedAchievementGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
							earnedAchievementGridView.Attributes.Add("tableName", "c");
							AchievementDiv.Controls.Add(earnedAchievementGridView);
						}

						GridView wishListAchievementGridView = (GridView)Page.LoadControl("~/Controls/CharacterDetailsControls.ascx").FindControl("WishListAchievementGridView");
						var wishedAchievementRanks = (
								from car in this.character.AchievementRanks
								where car.WishListRank != null && car.WishListRank > 0
								select new
								{
									Achievement_Name = car.Achievement_Name,
									WishListRank = car.WishListRank
								}
							).ToList();
						if (wishedAchievementRanks.Count() > 0)
						{
							wishListAchievementGridView.DataSource = wishedAchievementRanks;
							wishListAchievementGridView.DataBind();
							wishListAchievementGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
							wishListAchievementGridView.Attributes.Add("tableName", "d");
							AchievementDiv.Controls.Add(wishListAchievementGridView);
						}
					}
					else // we didn't get a character - likely because the character is no longer public, or user provided a malformed querystring
					{
						characterNameTitle.InnerHtml = "Not Found!"; // set the main header
						AbilityGridDiv.Controls.Add(new Label { Text = "Error: Character does not exist or is not public." });
					}
				}
			}
			CharacterFootnoteDiv.Controls.Add(Page.LoadControl("~/Controls/CharacterFootnoteControls.ascx").FindControl("CharacterFootnote"));
		}
	}
}