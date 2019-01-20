namespace Goblinary.Website.Controls
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Data;
	using System.Data.Common;
	using System.Reflection;

	using Goblinary.Common;
	using Goblinary.WikiData.Model;

    public partial class FeatEffects : System.Web.UI.UserControl
    {
		public static int GetEffectTypeSortOrder(string effectType)
		{
			int sortOrder = 1;
			switch (effectType)
			{
				case "Restriction":
					sortOrder = 2;
					break;
				case "Conditional":
					sortOrder = 3;
					break;
			}
			return sortOrder;
		}

		public static string GetFeatEffectDescription(EffectDescription effectDescription, string effectType)
		{
			return string.Format(effectDescription.FormattedDescription,
				string.Format("<a class=\"{2}_EffectCSS\" href=\"/EffectDetails?effect={0}\">{1}</a>", HttpUtility.UrlEncode(effectDescription.Effect_Name), effectDescription.Effect_Name, effectType),
				GetCondition(effectDescription),
				"<br/>&nbsp;&nbsp;&nbsp;&nbsp;",
				effectType == "PerKeyword" ? " per Keyword" : "");
		}

		private static string GetCondition(EffectDescription effectDescription)
		{
			if (effectDescription.Condition == null)
			{
				return null;
			}
			else if (effectDescription.Condition is EffectCondition)
			{
				return string.Format(
					"<a href=\"/EffectDetails?effect={0}\">{1}</a>",
					HttpUtility.UrlEncode(((EffectCondition)effectDescription.Condition).Effect_Name),
					((EffectCondition)effectDescription.Condition).Effect_Name);
			}
			else
			{
				return effectDescription.Condition_Name;
			}
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}