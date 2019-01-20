<%@ Page Title="Release Notes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReleaseNotes.aspx.cs" Inherits="Goblinary.Website.ReleaseNotes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>Release Notes</h2>
            </hgroup>
            <h4 class="noPad">Current:</h4>
			<p>
				<b>Release 2.0.6 (9/21/15):</b> Added a "spreadsheet modified" date to the website footer - now we don't need to create a new release
				every time we update the database with the latest spreadsheet data.
			</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h4 class="noPad">Past:</h4>
	<p>
		<b>Release 2.0.5 (8/17/15):</b> Security update to Character Builder. Instead of a boolean "Public" flag, each character now has
		a three-way option for Public/Shared/Private. Characters that were previously marked "Public" are still public, and characters that were
		"Private" are still private. Updated Character List to show this status and allow it to be changed. Updated Character Details page
		to reflect this status and provide a link to share. Added a Password Reset page. Upon entering your username and email, a new password
		will be emailed to you. Added link on Login page to the Password Reset page. Updated password requirements and touched-up the Register
		page a little. Fixed a bug where Ability Score Bonus for Wish List were not calculated properly when the Trained rank was set to zero.
	</p>
	<p>
		<b>Release 2.0.4 (8/10/15):</b> Updated database with the latest spreadsheet from 8/7/15, which includes the EE10.1a
		changes. Also added a few effects parser tweaks to prepare for the upcoming Goblinary 2.1 changes. Added a new test environment:
		<asp:HyperLink runat="server" NavigateUrl="http://test.goblinary.com">test.goblinary.com</asp:HyperLink>!
		We'll be using this new site as a public beta test of sorts, where we'll be trying out new features and the latest changes to
		the database - especially when major changes happen to the Goblinworks spreadsheets. You will be able to login to both sites
		using the same username/password as the live/Release website, however <u>at this time, character data will not be replicated
		cross-site</u> - meaning you'll have to create new characters on the test website for now. We'll look into adding a "copy
		character to test" feature at a later date.		
	</p>
	<p>
		<b>Release 2.0.3 (8/5/15):</b> Phase II of the character builder! Added "Public/Private" status option for each character - defaulted to "Private"
		for existing chracters and all newly created characters. Updated Character Details page to accept querystring with character number - it will now
		show details for any character, provided it is either a Public character OR it is one of your own characters (note: attempts to view characters that
		do not meet these criteria will simply fail). Updated the "Change" button to read "List/Manage" instead. Added "Public" checkbox to Character List
		page. Added Public/Private status to Character Details page. Added link to Character Details page that copies current URL to clipboard (for
		convenience/sharing). Added Details button to each character on Character List page. Updated privacy notice to mention available Public status.
		Updated WishList Ability Scores and Total WishList XP - they will calculate based on the greater of each trained feat rank or each wished feat rank;
		in other words, the WishList scores/XP will never be lower than the trained scores/XP. Thanks for the suggestion, Dazyk!
	</p>
	<p>
		<b>Release 2.0.2 (8/3/15):</b> Moved some javascript around to deduplicate code and simplify things a bit. Fixed a bug where some browsers would not
		download the new logo and some modified javascript files until forcing a refresh of the website (fixes some column width issues).
	</p>
	<p>
		<b>Release 2.0.1 (8/2/15):</b> Moved some columns around on the Feat Details page to prepare for an upcoming change to the way grids are generated. Fixed
		some various typos around the website.
	</p>
    <p>
        <b>Release 2.0 (8/1/15):</b> We're back! After countless hours and many late nights programming, we're ready for the new
		Goblinary 2.0!
		<br />
		- <b>Structures</b> have been added to the database, and have been tied into the Items and Feats.
		<br />
		- <b>New logo</b>, designed by Pixie (<i><asp:HyperLink runat="server" NavigateUrl="http://darkangelpixie.deviantart.com/">darkangelpixie</asp:HyperLink></i>),
		plus a significant facelift to the entire website's style - including support for larger screens (now stretches 100%).
		<br />
		- <b>Trainer Info</b> have been added to feats, and we'll likely be adding a complete Trainers section in a future release.
		<br />
		- We are pleased to announce the first iteration of our <b>Character Builder</b>! Create an account and login to begin.
		Character details and character list are accessible	from the account/character info box at the top right of the site.
		Train individual feat ranks by checking boxes while	viewing individual feats. We'll be expanding on this in future
		releases.
		<br />
		- <b>And a whole lot more!</b> Changed page titles to little-endian; most significant part of page title is shown first on
		browser tabs. Updated expendable types to match Goblinworks nomenclature. Removed some old code for a Tree Generator we didn't
		use for Recipes. Added new page for Release Notes and changed our nomenclature to use "Releases" instead of "Versions". Implemented
		a new Effects parser - should help with some unsightly bugs in various Effects around the website. We think we've fixed the
		problem where letting the website go stale (letting it sit idle for ~30min) would yield an Internal Server Error (500) upon using
		the	Master Search - if it's still a problem, please let us know. Fixed the unfriendly Item Type displayed on the Item Details page.
		Added a Quantity Produced to each Recipe node on the Recipe Details page. Fixed sorting on Achievement Requirements on the
		Feat Details page. Updated the Recipe Tree to be more mobile-friendly when dealing with nodes with long names. Improved
		layout for grids that are side-by-side, such as the Keyword Details page. Fixed a bug where an erroneous "No feats to show
		in this list" was showing. Added some style to distinguish inherent keywords from the others. Added "Ability Score Bonus
		per XP Cost" to Feat Ranks shown on Ability Details page. Removed the Master Search from the front
		page, and updated the one at the top of the screen - it is now also the default input when loading the home page. Some
		updates to the Login, Register, and Manage pages for accounts, now that they're actually being used. Added better sorting
		for items on the Item Details page. Fixed a bug where an erroneous "per Keyword" would show up on certain Feat Ranks.
		Fixed a mostly-invisible bug where some items had broken URLs in the +4 and +5 upgrades, despite no text being in the grid.
		Fixed the Meta description for Achievement Details pages. Fixed some malformed URLs for Weapon Kill Achievements. Fixed a
		large number of broken URLs due to inappropriate trailing spaces. Fixed various bugs due to cAsE inSensItiviTy. Fixed a bug
		where the filtering instructions were shown momentarily before being hidden on every page load - it is now hidden until
		shown. Fixed numerous bugs with the new features we've implemented, and countless more as we stumbled upon them while
		creating this new release.
    </p>
    <p>
        <b>Release 1.2.1 (4/16/15):</b> Fixed bugs on ItemList for Armor type and Implement type that were causing internal errors loading each.
            Uploaded latest data from spreadsheets (from 4/15/15), minus the Camps/Holdings/Outposts data (coming soon).
    </p>
    <p>
        <b>Release 1.2 (4/14/15):</b> We've completely changed the way the website interacts with its database/model. With this change, we
         had to re-write hundreds of lines of code scattered throughout nearly every file in the website, but it was all worth it.
         The website should not experience any more slow-downs like we had been seeing in v1.1, plus this change has enabled us to do some
         more fancy things throughout the website. In other news...
         <b>StockList & StockDetails</b> pages have been added. <b>RecipeDetails</b> page now has details about each recipe, along with
         a nifty ingredients tree. Dropdown for Achievement types has been added to the AchievementList page. Various obsolete things removed
         from the data model. Fixed a bug where the Player Killer achievement page gave a 500 Internal Server Error (thanks, Red!). Fixed a bug
         where Stock Items did not show their Stocks on the ItemList and ItemDetails pages. Fixed a bug where Ability Bonuses were being
         rounded to two decimal places, where it should be 3 places like it is in the spreadsheets (thanks, Tuoweit!). FeatList page now shows
         friendly Feat types in the dropdown, in the title, in the meta information, and in the header.
         New stuff from the latest spreadsheets added: Coin costs (for Feats), "Feat Advancements" & related feats, Holdout Weapon Expendables
         & Toolkit Expendables, Holdings & Outposts, and related recipes. Formatted the Effects slightly different such that conditional
         statements are broken into two lines.
    </p>
    <p>
        <b>Release 1.1 (3/18/15):</b> <b>Items and Recipes</b> have been loaded into the database! Unfortunately, the RecipeDetails page is not fully built
         yet - this is coming in v1.2. Otherwise the Items information is pretty much ready to go. KeywordDetails page now splits source
         feats and matching feats, and also shows matching items now. Fixed a bug where Keywords were not linked properly from the Master
         Search page (did not pass the Type variable). Fixed a bug where
         Personality was showing up twice. <b>New feature:</b> filters now automatically update the hashtag in the URL - allowing you to save
         a bookmark/URL that contains all the filters for the page you are currently viewing. Upon returning to this bookmark/URL, the filters
         will auto-populate. Caveats: may not work with all browsers (particularly mobile device browsers), and it stores the filters based
         on <i>column number</i>. This means that if the column order changes, or columns are add/removed, your bookmarked filters might not
         work as expected.
    </p>
    <p>
        <b>Release 1.0 (3/12/15):</b> <b>Release to public!</b> Updated text on home page. Updated footer. Fixed a large number of Category hyperlinks
         that search crawlers were having trouble with. Added distinction between keywords with same name - links to keywords now send
         the keyword type. Improved formatting of the all the DetailsViews on FeatDetails pages. Switched to ListViews for all the other
         Details pages.
    </p>
    <p>
        <b>Alpha 0.9:</b> Added Ability Scores to AbilityList page. Added Achievement Categories to CategoryList page. Added Keywords to
         database, added them to KeywordsList page. Removed obsolete references to CategoryPoints. Added AbilityDetails page. Updated
         MasterSearch to include Categories, Abilities and Keywords. Fixed bug where Bleeding Attack and a few other Reactives were
         missing ranks. Added some details to the EffectDetails page.
    </p>
    <p>
        <b>Alpha 0.8:</b> Fixed where MasterSearch was not returning effects from Feat Ranks. Added KeywordDetails page. Added EffectList
         page. "Finished" the AchievementList page. Added placeholder pages KeywordList, AbilityList, and CategoryList - all coming in v0.9.
    </p>
    <p>
        <b>Alpha 0.7:</b> Added CategoryDetails Page. Fix for old URLs that pointed to the AchievementDetails page that were
         using the real name of achievement, instead of the FriendlyName. Added a page for MetaAchievements, even though they don't
         exist yet. All details pages now handle %2B's in URL (happens when some search engines crawl the website and translate the
         '+' in the URLs). Added CategoryBonus to AchievementList. Details page for Feat Achievements have more info.
    </p>
</asp:Content>
