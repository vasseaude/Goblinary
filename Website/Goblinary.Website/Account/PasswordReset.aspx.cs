using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Profile;
using System.Configuration;

namespace Goblinary.Website
{
	public partial class PasswordReset : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void validateUserEmail(object sender, LoginCancelEventArgs e)
		{
			TextBox emailAddressTextBox = ((TextBox)PasswordRecovery.UserNameTemplateContainer.FindControl("EmailAddressTextBox"));
			Literal errorLiteral = ((Literal)PasswordRecovery.UserNameTemplateContainer.FindControl("ErrorLiteral"));
			MembershipUser mu = Membership.GetUser(PasswordRecovery.UserName);
			if (mu != null) // The username exists
			{
				if (!mu.Email.Equals(emailAddressTextBox.Text)) // Their email matches
				{
					e.Cancel = true;
					errorLiteral.Text = "Email address not valid for this user.";
				}
			}
			else
			{
				e.Cancel = true;
				errorLiteral.Text = "No such user found.";
			}
		}
	}
}