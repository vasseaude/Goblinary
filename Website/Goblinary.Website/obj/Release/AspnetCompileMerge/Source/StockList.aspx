<%@ Page Title="Stocks" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockList.aspx.cs" Inherits="Goblinary.Website.StockList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Stocks</h1>
            </hgroup>
            <p class="noPad">Here's a list of all of the Stocks.</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="StockListDiv" class="GridViewDiv"></div>
</asp:Content>