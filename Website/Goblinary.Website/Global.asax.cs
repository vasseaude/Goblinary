namespace Goblinary.Website
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;
	using System.Web.Optimization;
	using System.Web.Routing;
	using System.Web.Security;

	using Goblinary.Website;
	using Goblinary.WikiData.Model;

	public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			Ability.ToStringMethod = x => AbilityDetails.GetLink(x);
			Feat.ToStringMethod = x => FeatDetails.GetLink(x);
			FeatEffect.ToStringMethod = x => FeatDetails.GetFeatEffect(x);
			FeatRankAbilityBonus.ToStringMethod = x => FeatDetails.GetFeatRankAbilityBonus(x);
			FeatRankAbilityRequirement.ToStringMethod = x => FeatDetails.GetFeatRankAbilityRequirement(x);
			FeatRankAchievementRequirement.ToStringMethod = x => FeatDetails.GetFeatRankAchievementRequirement(x);
			FeatRankCategoryRequirement.ToStringMethod = x => FeatDetails.GetFeatRankCategoryRequirement(x);
			FeatRankFeatRequirement.ToStringMethod = x => FeatDetails.GetFeatRankFeatRequirement(x);
			FeatRankEffect.ToStringMethod = x => FeatDetails.GetFeatRankEffect(x);
			FeatRankKeyword.ToStringMethod = x => FeatDetails.GetFeatRankKeyword(x);
		}

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
