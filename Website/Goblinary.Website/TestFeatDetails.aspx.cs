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

	public partial class TestFeatDetails : System.Web.UI.Page
	{
		private class CustomFeatRank
		{
			public CustomFeatRank(FeatRank featRank, CharacterFeatRank characterFeatRank)
			{
				this.FeatRank = featRank;
				this.IsTrained = characterFeatRank != null && characterFeatRank.TrainedRank >= this.FeatRank.Rank;
				this.IsWishList = characterFeatRank != null && characterFeatRank.WishListRank >= this.FeatRank.Rank;
			}

			[Presentation(PresentationTypes.Property, DisplayName = "Trained?")]
			public bool IsTrained { get; set; }
			[Presentation(PresentationTypes.Property, DisplayName = "Wish List?")]
			public bool IsWishList { get; set; }
			[Presentation(PresentationTypes.Wrapper)]
			public FeatRank FeatRank { get; private set; }
		}

		private Feat feat;
		private CharacterFeatRank characterFeatRank = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				string featName = HttpUtility.UrlDecode(Request.QueryString["feat"]);
				using (WikiDataContext context = new WikiDataContext())
				{
					this.feat = (
						from f in context.Set<Feat>()
							.Include("FeatType")
							.Include("Role")
							.Include("AdvancementFeat.Feats")
							.Include("Ranks.AbilityBonuses.Ability")
							.Include("Ranks.AbilityRequirements.Ability")
							.Include("Ranks.AchievementRequirements.AchievementRank.Achievement")
							.Include("Ranks.CategoryRequirements")
							.Include("Ranks.FeatRequirements.RequiredFeatRank.Feat")
							.Include("Ranks.Effects.EffectDescription.Effect")
							.Include("Ranks.Keywords.Keyword")
							.Include("Effects.EffectDescription.Effect")
							.Include("AchievementRanks")
							.Include("HoldingUpgrades")
							.Include("OutpostWorkerFeats")
						where f.Name == featName
						select f).FirstOrDefault();
				}

				if (this.User.Identity.IsAuthenticated && Membership.GetUser() != null)
				{
					MembershipUser user = Membership.GetUser();
					if (user != null)
					{
						ProfileBase userProfile = ProfileBase.Create(user.UserName);
						string providerUserKey = user.ProviderUserKey.ToString();
						int profileCharacterID = (int)userProfile.GetPropertyValue("SelectedCharacterID");
						using (CharacterDataContext characterDataContext = new CharacterDataContext())
						{
							this.characterFeatRank = (
									from c in characterDataContext.Characters
									where c.User_ID == providerUserKey && c.ID == profileCharacterID
									from cfr in c.FeatRanks
									where cfr.Feat_Name == this.feat.AdvancementFeat_Name
									select cfr
								).FirstOrDefault();
						}
					}
				}

				#region Meta

				Page.MetaDescription += String.Format(" Feat details and ranks for {0} - a {1}.", feat.Name, feat.FeatType.DisplayName);
				Page.Title = feat.Name + " | Feat";
				featTitle.InnerHtml = feat.Name; // set the main header

				Label dd = new Label();
				// temporary until we can get a real description from the database
				dd.Text = String.Format("Goblinworks has not provided a description for type: <b>{0}</b>", FeatList.GetLink(feat.FeatType));
				FeatBlock.Controls.Add(dd);

				#endregion Meta

				//DetailsView detailsControl = (DetailsView)Page.LoadControl("~/Controls/Feat/" + feat.FeatType.Name + ".ascx").FindControl("FeatDetailsView");
				//detailsControl.DataSource = new List<Feat> { this.feat };
				//detailsControl.DataBind();
				//detailsControl.HeaderRow.TableSection = TableRowSection.TableHeader;
				//FeatBlock.Controls.Add(detailsControl);

				DetailsView testView = new DetailsView();
				testView.XGenerateFields(this.feat.GetType());
				testView.DataSource = new List<Feat> { this.feat };
				testView.XDataBind();
				this.FeatBlock.Controls.Add(testView);

				#region AdvancementFeat Notes

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

				#endregion AdvancementFeat Notes

				//if (feat.Effects.Count() > 0)
				//{
				//	ListView featEffectsListView = (ListView)Page.LoadControl("~/Controls/FeatEffects.ascx").FindControl("FeatEffectsListView");
				//	featEffectsListView.DataSource = feat.Effects;
				//	featEffectsListView.DataSource = (
				//			from fe in feat.Effects
				//			orderby FeatEffects.GetEffectTypeSortOrder(fe.EffectType)
				//			select new
				//			{
				//				EffectType = fe.EffectType,
				//				Description = FeatEffects.GetFeatEffectDescription(fe.EffectDescription, fe.EffectType)
				//			}
				//		).ToList();
				//	featEffectsListView.DataBind();
				//	PlaceHolder FeatEffectsPlaceholder = (PlaceHolder)detailsControl.FindControl("FeatEffectsPlaceholder");
				//	if (FeatEffectsPlaceholder != null)
				//	{
				//		FeatEffectsPlaceholder.Controls.Add(featEffectsListView);
				//	}
				//}

				//ListView relatedFeatsListView = (ListView)detailsControl.FindControl("RelatedFeatsListView");
				//if (relatedFeatsListView != null)
				//{
				//	relatedFeatsListView.DataSource = (
				//			from f in feat.AdvancementFeat.Feats
				//			where f.Name != feat.Name
				//			orderby f.Name
				//			select f
				//		);
				//	relatedFeatsListView.DataBind();
				//}

				if (feat.Ranks.Count() > 0)
				{
					GridView featRanksGrid = new GridView();
					featRanksGrid.XGenerateColumns(typeof(CustomFeatRank));
					featRanksGrid.DataSource = (
							from fr in feat.Ranks
							select new CustomFeatRank(fr, this.characterFeatRank)
						).ToList();
					featRanksGrid.XDataBind("a");
					this.RanksBlock.Controls.Add(featRanksGrid);
				}
				this.XLoadNotes(this.notes);
			}
		}
	}
}