<%@ Page Title="Ability Scores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbilityList.aspx.cs" Inherits="Goblinary.Website.AbilityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Ability Scores</h1>
            </hgroup>
            <p class="noPad">Here's a list of all of the Ability Scores.</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="AbilityListDiv" class="GridViewDiv"></div>
</asp:Content>