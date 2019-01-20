using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Web.Profile;
using System.Web.Security;

using Goblinary.Common;
using Goblinary.Website.Controls;
using Goblinary.CharacterData.Model;
using Goblinary.CharacterData.SqlServer;
using Goblinary.WikiData.Model;

namespace Goblinary.Website.Account
{
    public partial class CharacterList : System.Web.UI.Page
    {
        private string providerUserKey;
        private ProfileBase userProfile;
        private int profileCharacterID;
        private List<Character> characters;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MetaDescription += " Character list/manage.";
            if (User.Identity.IsAuthenticated)
            {
                using (CharacterDataContext characterDataContext = new CharacterDataContext())
                {
                    this.providerUserKey = Membership.GetUser().ProviderUserKey.ToString();
                    this.userProfile = ProfileBase.Create(Membership.GetUser().UserName); // get profile
                    this.profileCharacterID = (int)userProfile.GetPropertyValue("SelectedCharacterID"); // get character ID from profile

                    this.characters = (
                            from c in characterDataContext.Characters
                            where c.User_ID == providerUserKey
                            select c
                        ).ToList();
                    foreach (Character character in characters)
                    {
                        Panel characterListPanel = (Panel)Page.LoadControl("~/Controls/CharacterListControls.ascx").FindControl("CharacterListPanel");
                        characterListPanel.ID += character.ID.ToString();

                        Label characterNameLabel = (Label)characterListPanel.FindControl("CharacterNameLabel");
                        characterNameLabel.ID += character.ID.ToString();
                        characterNameLabel.Text = String.Format("Character Name: <b>{0}</b>", character.Name);

                        var trainedFeatRanks = (
                                from cfr in character.FeatRanks
                                where cfr.TrainedRank > 0
                                select cfr
                            );
                        Label characterTrainedFeatsLabel = (Label)characterListPanel.FindControl("CharacterTrainedFeatsLabel");
                        characterTrainedFeatsLabel.ID += character.ID.ToString();
                        characterTrainedFeatsLabel.Text = String.Format("Trained feats: <b>{0}</b>", trainedFeatRanks.Count().ToString());

                        TextBox characterNameTextBox = (TextBox)characterListPanel.FindControl("CharacterNameTextBox");
                        characterNameTextBox.ID += character.ID.ToString();
                        characterNameTextBox.Text = character.Name;

                        Button characterSelect = (Button)characterListPanel.FindControl("CharacterSelect");
                        characterSelect.ID += character.ID.ToString();
                        characterSelect.Command += CharacterSelect_Click;
                        characterSelect.CommandName = character.ID.ToString();

                        Button characterNameSubmit = (Button)characterListPanel.FindControl("CharacterNameSubmit");
                        characterNameSubmit.ID += character.ID.ToString();
						characterNameSubmit.Attributes.Add("onclick", "ShowProgress();");
                        characterListPanel.DefaultButton += character.ID;
                        characterNameSubmit.Command += CharacterNameSubmit_Click;
                        characterNameSubmit.CommandName = character.ID.ToString();
                        characterNameSubmit.CommandArgument = characterNameTextBox.Text;

                        Button characterDelete = (Button)characterListPanel.FindControl("CharacterDelete");
                        characterDelete.ID += character.ID.ToString();
                        characterDelete.Command += CharacterDelete_Click;
                        characterDelete.CommandName = character.ID.ToString();
                        characterDelete.OnClientClick = String.Format("ConfirmDelete(\"{0}\");", character.Name);

                        Button characterDuplicate = (Button)characterListPanel.FindControl("CharacterDuplicate");
                        characterDuplicate.ID += character.ID.ToString();
                        characterDuplicate.Command += CharacterDuplicate_Click;
                        characterDuplicate.CommandName = character.ID.ToString();

						Button characterDetails = (Button)characterListPanel.FindControl("CharacterDetails");
						characterDetails.ID += character.ID.ToString();
						characterDetails.Command += CharacterDetails_Click;
						characterDetails.CommandName = character.ID.ToString();

						RadioButtonList characterShareStatusRadio = (RadioButtonList)characterListPanel.FindControl("CharacterShareStatusRadio");
						characterShareStatusRadio.ID = character.ID.ToString() + "|c";
						characterShareStatusRadio.AutoPostBack = true;
						characterShareStatusRadio.Attributes.Add("onclick", "ShowProgress();");
						characterShareStatusRadio.TextChanged += characterShareStatusRadio_TextChanged;
						characterShareStatusRadio.Text = character.ShareStatus;

                        if (profileCharacterID == character.ID)
                        {
                            characterListPanel.CssClass += " currentCharacterListPanel";
                            characterSelect.Enabled = false;
                            characterSelect.Text = "(Selected)";
                        }

                        CharacterListDiv.Controls.Add(characterListPanel);
                    }

                    Panel createNewCharacterPanel = (Panel)Page.LoadControl("~/Controls/CharacterListControls.ascx").FindControl("CreateNewCharacterPanel");

                    TextBox newCharacterNameTextBox = (TextBox)createNewCharacterPanel.FindControl("NewCharacterNameTextBox");

                    Button newCharacterNameSubmit = (Button)createNewCharacterPanel.FindControl("NewCharacterNameSubmit");
                    newCharacterNameSubmit.Command += CharacterCreateSubmit_Click;

                    CharacterListDiv.Controls.Add(createNewCharacterPanel);
                }
            }
			CharacterFootnoteDiv.Controls.Add(Page.LoadControl("~/Controls/CharacterFootnoteControls.ascx").FindControl("CharacterFootnote"));
        }

        protected void CharacterSelect_Click(object sender, EventArgs e)
        {
            Button characterNameSubmit = (Button)sender;
            int characterID = Convert.ToInt32(characterNameSubmit.CommandName);
            this.userProfile.SetPropertyValue("SelectedCharacterID", characterID);
            this.userProfile.Save();
            Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
            Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
        }

        protected void CharacterNameSubmit_Click(object sender, EventArgs e)
        {
            using (CharacterDataContext characterDataContext = new CharacterDataContext())
            {
                Button characterNameSubmit = (Button)sender;
                int characterID = Convert.ToInt32(characterNameSubmit.CommandName);
                var character = (
						from c in this.characters
						where c.ID == characterID
						select c
                    ).FirstOrDefault(); // get current character based on the CommandName in the button

                string newCharacterName = ((TextBox)characterNameSubmit.Parent.FindControl("CharacterNameTextBox" + character.ID.ToString())).Text;

                character.Name = newCharacterName;
                characterDataContext.Entry(character).State = EntityState.Modified;
                characterDataContext.SaveChanges();
            }
            Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
            Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
        }

        protected void CharacterDelete_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                using (CharacterDataContext characterDataContext = new CharacterDataContext())
                {
                    var character = (
                            from c in this.characters
                            where c.ID == int.Parse(((Button)sender).CommandName)
                            select c
                        ).FirstOrDefault(); // get current character based on the CommandName in the button
                    characterDataContext.Characters.Attach(character);
                    foreach (CharacterFeatRank cfr in character.FeatRanks.ToList())
                    {
                        characterDataContext.Entry(cfr).State = EntityState.Deleted; // clear all FeatRanks
                    }
                    foreach (CharacterAchievementRank car in character.AchievementRanks.ToList())
                    {
                        characterDataContext.Entry(car).State = EntityState.Deleted; // clear all AchievementRanks
                    }
                    characterDataContext.Entry(character).State = EntityState.Deleted; // delete the character
                    characterDataContext.SaveChanges();

                    if (this.profileCharacterID == character.ID) // if we just deleted the currently-selected character
                    {
                        var nextSelectedCharacter = (
                            from c in this.characters
                            where c.ID != character.ID
                            select c
                            ).FirstOrDefault(); // should return the first character belonging to this user that is NOT the one we just deleted
                        if (nextSelectedCharacter != null)
                        {
                            this.userProfile.SetPropertyValue("SelectedCharacterID", nextSelectedCharacter.ID); // select the next character in their list
                        }
                        else // if no other characters, create a new one!
                        {
                            Character newCharacter = new Character();
							newCharacter.User_ID = this.providerUserKey;
							newCharacter.ShareSeed = Guid.NewGuid().ToString("N").Substring(1, 16);
							newCharacter.ShareStatus = "Private";
                            newCharacter.Name = "Default";
                            characterDataContext.Entry(newCharacter).State = EntityState.Added;
                            characterDataContext.SaveChanges();
                            this.userProfile.SetPropertyValue("SelectedCharacterID", newCharacter.ID);
                        }
                        this.userProfile.Save(); // either way, need to save the profile
                    }
                }
                Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
                Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
            }
        }

        protected void CharacterDuplicate_Click(object sender, EventArgs e)
        {
            using (CharacterDataContext characterDataContext = new CharacterDataContext())
            {
                var oldCharacter = (
						from c in this.characters
						where c.ID == int.Parse(((Button)sender).CommandName)
						select c
                    ).FirstOrDefault(); // get current character based on the CommandName in the button
				characterDataContext.Characters.Attach(oldCharacter);

				Character newCharacter = new Character();
				characterDataContext.Characters.Add(newCharacter);
				characterDataContext.Entry(newCharacter).CurrentValues.SetValues(characterDataContext.Entry(oldCharacter).CurrentValues); // copy all the properies
				newCharacter.ShareSeed = Guid.NewGuid().ToString("N").Substring(1, 16);
				newCharacter.ShareStatus = "Private";
				newCharacter.Name = oldCharacter.Name + " (Copy)";
				newCharacter.FeatRanks.AddRange(
						(
							from cfr in oldCharacter.FeatRanks
							select new CharacterFeatRank
							{
								Feat_Name = cfr.Feat_Name,
								TrainedRank = cfr.TrainedRank,
								WishListRank = cfr.WishListRank
							}
						).ToList()
					);
				newCharacter.AchievementRanks.AddRange(
						(
							from car in oldCharacter.AchievementRanks
							select new CharacterAchievementRank
							{
								Achievement_Name = car.Achievement_Name,
								EarnedRank = car.EarnedRank,
								WishListRank = car.WishListRank
							}
						)
					);
				characterDataContext.SaveChanges();
            }
            Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
            Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
        }

		protected void CharacterDetails_Click(object sender, EventArgs e)
		{
			Response.Redirect(String.Format("/CharacterDetails?character={0}", ((Button)sender).CommandName));
		}

        protected void CharacterCreateSubmit_Click(object sender, EventArgs e)
        {
            using (CharacterDataContext characterDataContext = new CharacterDataContext())
            {
                string newCharacterName = ((TextBox)((Button)sender).Parent.FindControl("NewCharacterNameTextBox")).Text; // sender=button, parent=button's panel, find textbox in panel
                

                var userProfile = ProfileBase.Create(Membership.GetUser().UserName); // get profile
                string providerUserKey = Membership.GetUser().ProviderUserKey.ToString();
                Character character = new Character();
                character.User_ID = providerUserKey;
				character.ShareSeed = Guid.NewGuid().ToString("N").Substring(1, 16);
				character.ShareStatus = "Private";
                character.Name = String.IsNullOrEmpty(newCharacterName) ? "New" : newCharacterName;
                characterDataContext.Entry(character).State = EntityState.Added;
                characterDataContext.SaveChanges();

                //userProfile.SetPropertyValue("SelectedCharacterID", character.ID); // and create the character
                //userProfile.Save(); // save the profile
            }
            Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
            Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
        }

		protected void characterShareStatusRadio_TextChanged(object sender, EventArgs e)
		{
			using (CharacterDataContext characterDataContext = new CharacterDataContext())
			{
				RadioButtonList characterShareStatusRadio = (RadioButtonList)sender;
				int characterID = Convert.ToInt32(characterShareStatusRadio.ID.Split('|').First());
				var character = (
						from c in this.characters
						where c.ID == characterID
						select c
					).FirstOrDefault(); // get current character based on the checkbox ID in the button

				character.ShareStatus = characterShareStatusRadio.Text;
				characterDataContext.Entry(character).State = EntityState.Modified;
				characterDataContext.SaveChanges();
			}
			Session["ViewState"] = null; // clear the user ViewState before refreshing and finishing postback (important!)
			Response.Redirect(Request.RawUrl); // force a reload of the page - forces the list to update
		}
    }
}