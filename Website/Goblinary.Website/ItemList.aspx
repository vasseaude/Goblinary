<%@ Page Title="Items" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemList.aspx.cs" Inherits="Goblinary.Website.ItemList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server" id="itemTitle">Items</h1>
                <h2 runat="server" id="itemTitle2"></h2>
            </hgroup>
            <small>Select a <b>Item Type</b> to narrow results: </small>
            <asp:DropDownList ID="itemTypeList" runat="server" CssClass="typeDropDown" AutoPostBack="True" OnSelectedIndexChanged="itemTypeList_SelectedIndexChanged" DataSourceID="itemTypeListSource" DataTextField="DisplayName" DataValueField="Name" OnPreRender="itemTypeList_PreRender">
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
            <small><b>Note:</b> more columns of data are available for various types of items.</small>
            <asp:ObjectDataSource ID="itemTypeListSource" runat="server" SelectMethod="SelectItemTypes" TypeName="Goblinary.Website.TypeLists.EntityTypeList">
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
    <div runat="server" id="ItemListDiv" class="GridViewDiv"></div>
</asp:Content>