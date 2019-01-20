<%@ Page Title="Keyword" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeywordDetails.aspx.cs" Inherits="Goblinary.Website.KeywordDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server">Keyword:</h1>
                <h1 runat="server" id="keywordTitle"></h1>
                <h2 runat="server" id="keywordTitle2"></h2>
            </hgroup>
            <div runat="server" id="KeywordBlock"></div>
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
                <td>
                    <div runat="server" id="SourceFeatsBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Feats that provide this keyword:</h4></div>
                </td>
                <td>
                    <div runat="server" id="MatchingFeatsBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Feats that match this keyword:</h4></div>
                    <br runat="server" id="RightSideBreak" />
                    <div runat="server" id="MatchingItemsBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Items that match this keyword:</h4></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>