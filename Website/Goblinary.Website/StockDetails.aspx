<%@ Page Title="Stock" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockDetails.aspx.cs" Inherits="Goblinary.Website.StockDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server">Stock:</h1>
                <h1 runat="server" id="stockTitle"></h1>
            </hgroup>
            <div runat="server" id="StockBlock"></div>
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
                <td><div runat="server" id="ItemsBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Items that count as this stock:</h4></div></td>
                <td><div runat="server" id="RecipesBlock" class="GridViewDiv sideBySideGridDiv inlineGrid vTop"><h4 class="noPad">Recipes requiring this stock:</h4></div></td>
            </tr>
        </table>
    </div>
</asp:Content>