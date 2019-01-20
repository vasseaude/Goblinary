<%@ Page Title="Effects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EffectList.aspx.cs" Inherits="Goblinary.Website.EffectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Effects</h1>
            </hgroup>
            <p class="noPad">Here's a list of all of the Effects.</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <button type="button" class="ShowHideButton">Show/Hide Filtering Instructions</button> <span><small>(Hint: Use SHIFT key to sort multiple columns.)</small></span>
    <div runat="server" class="noPad NotesDiv" id="notes"></div>
    <div runat="server" id="EffectListDiv" class="GridViewDiv">
    </div>
</asp:Content>