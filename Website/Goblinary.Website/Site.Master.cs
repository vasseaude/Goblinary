using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Web.Profile;

using Goblinary.CharacterData.Model;
using Goblinary.CharacterData.SqlServer;

namespace Goblinary.Website
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
		public string siteTitle;
		private DateTime databaseModifiedDate;

		protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
#if PUBLIC_TEST
			this.Page.MetaDescription += "The Goblinary: PFO (Pathfinder Online) database, public test.";
#else
			this.Page.MetaDescription += "The Goblinary: PFO (Pathfinder Online) database.";
#endif
		}

        protected void Page_Load(object sender, EventArgs e)
        {
#if PUBLIC_TEST
			this.searchPanel.CssClass = "searchPanelPublicTest";
			this.loginPanel.CssClass = "loginPanelPublicTest";
			this.logoImage.ImageUrl = String.Format("/Images/goblinary-logo-test.gif?modified={0}", System.IO.File.GetLastWriteTime(Server.MapPath("/Images/goblinary-logo-test.gif")).ToString("MMddyyhhmmss"));
			this.siteTitle = "Goblinary Test";
			this.databaseModifiedDate = DateTime.Now; // Nihimon, this is where we need to get the date from the TEST database.
			this.DatabaseUpdatedLabel.Text = String.Format("Goblinary <b>Test</b> database currently loaded with data from a copy of the <a href=\"https://drive.google.com/folderview?id=0B0THpTRVitJ7clFtVi1lZWptekk&usp=sharing#list\">PFO Wiki spreadsheet</a> from <b>{0}</b>.", databaseModifiedDate.ToString("d"));
#else
			this.logoImage.ImageUrl = String.Format("/Images/goblinary-logo.gif?modified={0}", System.IO.File.GetLastWriteTime(Server.MapPath("/Images/goblinary-logo.gif")).ToString("MMddyyhhmmss"));
			this.siteTitle = "Goblinary";
			this.databaseModifiedDate = new DateTime(2015, 10, 12, 0, 0, 0); // Nihimon, this is where we need to get the date from the database.
			this.DatabaseUpdatedLabel.Text = String.Format("Goblinary database currently loaded with data from a copy of the <a href=\"https://drive.google.com/folderview?id=0B0THpTRVitJ7clFtVi1lZWptekk&usp=sharing#list\">PFO Wiki spreadsheet</a> from <b>{0}</b>.", databaseModifiedDate.ToString("d"));
#endif

			if (HttpContext.Current.User.Identity.IsAuthenticated && Membership.GetUser() != null)
            {
                using (CharacterDataContext characterDataContext = new CharacterDataContext()) // now we start looking at character entitites
                {
                    var userProfile = ProfileBase.Create(Membership.GetUser().UserName); // get profile
                    int profileCharacterID = (int)userProfile.GetPropertyValue("SelectedCharacterID"); // get character ID from profile
                    string providerUserKey = Membership.GetUser().ProviderUserKey.ToString();
                    Character character = (
                           from c in characterDataContext.Characters
                           where c.User_ID == providerUserKey && c.ID == profileCharacterID
                           select c
                       ).FirstOrDefault();
                    if (character == null) // no character found by that name? Let's create it.
                    {
                        character = new Character();
                        character.User_ID = providerUserKey;
						character.ShareSeed = Guid.NewGuid().ToString("N").Substring(1, 16);
						character.ShareStatus = "Private";
                        character.Name = "Default";
                        characterDataContext.Entry(character).State = EntityState.Added;
                        characterDataContext.SaveChanges();

                        userProfile.SetPropertyValue("SelectedCharacterID", character.ID); // and create the character
                        userProfile.Save(); // save the profile
                    }
                    (this.masterLoginView.FindControl("characterNameLabel") as Label).Text = character.Name; // Finally, we update the page header!
					(this.masterLoginView.FindControl("characterDetailsHyperLink") as HyperLink).NavigateUrl += "?character=" + character.ID.ToString();
                }
            }
        }

        protected void SearchSubmit_Click(object sender, EventArgs e)
        {
            //Page.Form.DefaultButton = SearchSubmit.UniqueID;
            Response.Redirect(string.Format("~/MasterSearch?q={0}", HttpUtility.UrlEncode(searchbox.Text)), true); 
        }

    }
}