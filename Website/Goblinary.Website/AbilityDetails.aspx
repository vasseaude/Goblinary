<%@ Page Title="Ability Score" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbilityDetails.aspx.cs" Inherits="Goblinary.Website.AbilityDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server">Ability Score:</h1>
                <h1 runat="server" id="abilityTitle"></h1>
            </hgroup>
            <div runat="server" id="AbilityBlock"></div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" class="noPad">
        <asp:Label runat="server" ID="MobileWarningLabel" CssClass="mobileWarning"><small><b>Mobile Users:</b> you may need to scroll left/right on this list</small></asp:Label>
    </div>
    <button type="button" class="ShowHideButton">Show/Hide Filtering Instructions</button> <span><small>(Hint: Use SHIFT key to sort multiple columns.)</small></span>
    <div runat="server" class="noPad NotesDiv" id="notes"></div>    
    <div runat="server" class="noPad xOverflow widthHundredPercent">
        <table class="sideBySideGridTable">
            <tr>
                <td><div runat="server" id="RankBonusBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Feat ranks granting this ability score:</h4></div></td>
                <td><div runat="server" id="RankRequirementBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Feat ranks requiring this ability score:</h4></div></td>
            </tr>
        </table>
    </div>
</asp:Content>