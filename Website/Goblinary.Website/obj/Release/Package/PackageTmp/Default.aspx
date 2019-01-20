<%@ Page Title="Pathfinder Online Database" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Goblinary.Website._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>Welcome to the Goblinary - a Pathfinder Online database.</h2>
            </hgroup>
            <p class="noPad">
                <i>"All progress is experimental."<small> - John Jay Chapman</small></i><br /><br />
                Welcome to the reference website we've created for players of <a href="https://goblinworks.com/pathfinder-online/">Pathfinder Online</a>
                 to use on their daily adventures. We're welcoming you to share in the wealth of information that the game developers have provided
                 us - including feats, achievements, recipes, craftable items, and everything related to these - all in a format that's easy
                 to navigate and understand.<br />
            </p>
            <p class="noPad">
                <br />To get started, you can view complete lists of feats, achievements, effects, etc. by clicking the links along the top of
                 the page, or you can simply search for stuff using the Master Search at the top left of the page.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h4 class="noPad">Current Release Notes:</h4>
	<p>
		<b>Release 2.0.6 (9/21/15):</b> Added a "spreadsheet modified" date to the website footer - now we don't need to create a new release
		every time we update the database with the latest spreadsheet data.
	</p>
    <asp:HyperLink runat="server" NavigateUrl="~/ReleaseNotes">Older Release Notes...</asp:HyperLink>
</asp:Content>