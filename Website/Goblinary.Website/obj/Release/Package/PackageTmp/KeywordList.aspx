<%@ Page Title="Keywords" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeywordList.aspx.cs" Inherits="Goblinary.Website.KeywordList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Keywords</h1>
            </hgroup>
            <p class="noPad">Here's a list of all of the Keywords.</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="KeywordListDiv" class="GridViewDiv"></div>
</asp:Content>