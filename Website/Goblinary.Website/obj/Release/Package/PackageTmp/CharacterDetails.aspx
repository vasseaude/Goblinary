<%@ Page Title="Character Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CharacterDetails.aspx.cs" Inherits="Goblinary.Website.CharacterDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		function ClipBoard() {
			holdtext.innerText = copytext.innerText; Copied = holdtext.createTextRange().execCommand("Copy");
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Character:</h1>
                <h1 runat="server" id="characterNameTitle"></h1>
            </hgroup>
			<div runat="server" id="ShareStatusDiv" class="statsDiv vtop" />
            <div runat="server" id="AbilityGridDiv" class="abilityGridDiv inlineGrid vtop" />
            <div runat="server" id="StatsDiv" class="statsDiv inlineGrid vtop" />
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" style="overflow-x: auto;">
        <div runat="server" id="FeatsDiv" class="sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Feats:</h4></div>
        <div runat="server" id="AchievementDiv" class="sideBySideGridDiv inlineGrid vtop"><h4 class="noPad">Achievements:</h4>Not available yet.</div>
    </div>
	<div runat="server" id="CharacterFootnoteDiv" />
</asp:Content>
