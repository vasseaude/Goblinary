<%@ Page Title="Structures" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StructureList.aspx.cs" Inherits="Goblinary.Website.StructureList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Structures</h1>
            </hgroup>
            <p class="noPad">Here's a list of all of the Structures.</p>
            <small>Select a <b>Structure Type</b> to narrow results: </small>
            <asp:DropDownList ID="structureTypeList" runat="server" CssClass="typeDropDown" AutoPostBack="True" OnSelectedIndexChanged="structureTypeList_SelectedIndexChanged" DataSourceID="structureTypeListSource" DataTextField="DisplayName" DataValueField="Name" OnPreRender="structureTypeList_PreRender">
            </asp:DropDownList>
            <br />
            <small><b>Note:</b> more columns of data are available for various types of feats.</small>
            <asp:ObjectDataSource ID="structureTypeListSource" runat="server" SelectMethod="SelectStructureTypes" TypeName="Goblinary.Website.TypeLists.EntityTypeList">
            </asp:ObjectDataSource>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="StructureListDiv" class="GridViewDiv"></div>
</asp:Content>