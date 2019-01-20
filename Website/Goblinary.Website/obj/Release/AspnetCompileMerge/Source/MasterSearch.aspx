<%@ Page Title="Master Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MasterSearch.aspx.cs" Inherits="Goblinary.Website.MasterSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %></h1>
            </hgroup>
            <asp:Label ID="searchLabel" runat="server" Text=""></asp:Label><br />
<%--            <small><b>Filter</b> on all columns at once: </small><input class="search" type="search" data-column="all">
            <br />
            <small>Reset all column filters, including Master Filter (above): </small><button type="button" class="reset">Reset All Filters</button><br />--%>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="MasterSearchGridDiv" class="GridViewDiv"><asp:Label runat="server" ID="MobileWarningLabel" CssClass="mobileWarning"><small>Mobile Users: you may need to scroll left/right on this list</small></asp:Label>
    </div>
</asp:Content>