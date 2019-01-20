<%@ Page Title="Recipes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecipeList.aspx.cs" Inherits="Goblinary.Website.RecipeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server" id="recipeTitle">Recipes</h1>
                <h2 runat="server" id="recipeTitle2"></h2>
            </hgroup>
            <small>Select a <b>Recipe Type</b> to narrow results: </small>
            <asp:DropDownList ID="recipeTypeList" runat="server" CssClass="typeDropDown" AutoPostBack="True" OnSelectedIndexChanged="recipeTypeList_SelectedIndexChanged" DataSourceID="recipeTypeListSource" DataTextField="DisplayName" DataValueField="Name" OnPreRender="recipeTypeList_PreRender">
            </asp:DropDownList> 
            <br />
            <small><b>Filter</b> on all columns at once: </small><input class="search" type="search" data-column="all">
            <br />
            <small>Reset all column filters, including Master Filter (above): </small><button type="button" class="reset">Reset All Filters</button><br />
            <div class="columnSelectorWrapper">
                <input id="colSelect1" type="checkbox" class="hidden">
                <label class="columnSelectorButton" for="colSelect1">Show/Hide Columns:</label>
                <div id="columnSelector" class="columnSelector"></div>
            </div>
            <asp:ObjectDataSource ID="recipeTypeListSource" runat="server" SelectMethod="SelectRecipeTypes" TypeName="Goblinary.Website.TypeLists.EntityTypeList">
            </asp:ObjectDataSource>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" class="noPad">
        <asp:Label runat="server" ID="MobileWarningLabel" CssClass="mobileWarning"><small><b>Mobile Users:</b> you may need to scroll left/right on this list</small></asp:Label>
    </div>
    <button type="button" class="ShowHideButton">Show/Hide Filtering Instructions</button> <span><small>(Hint: Use SHIFT key to sort multiple columns.)</small></span>
    <div runat="server" class="noPad NotesDiv" id="notes"></div>
    <div runat="server" id="RecipeListDiv" class="GridViewDiv"></div>
</asp:Content>