<%@ Page Title="Recipe" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecipeDetails.aspx.cs" Inherits="Goblinary.Website.RecipeDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server">Recipe:</h1>
                <h1 runat="server" id="recipeTitle"></h1>
            </hgroup>
            <div runat="server" id="RecipeBlock" class="detailsViewDiv"></div>
            <div runat="server" id="DropDownBlock" class="noPad" visible="True">
                Select Upgrade to auto-explode ingredients tree: 
                <asp:DropDownList runat="server" ID="upgradeDropDownList" CssClass="typeDropDown" AutoPostBack="true" OnSelectedIndexChanged="upgradeDropDownList_SelectedIndexChanged" OnPreRender="upgradeDropDownList_PreRender" DataTextField="Text" DataValueField="Value"></asp:DropDownList>
                <br />
                <small>Note: currently, no recipes exist for +4 or +5 components</small>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" class="RecipeTreeViewDiv">
        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" CssClass="RecipeTreeView" LineImagesFolder="~/TreeLineImages" NodeIndent="32" />
    </div>
</asp:Content>
