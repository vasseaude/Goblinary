<%@ Page Title="Feat" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FeatDetails.aspx.cs" Inherits="Goblinary.Website.FeatDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="/Scripts/showHideNotesDiv.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <div class="loading"><h3>Saving character data...</h3><img src="/Images/spiffygif.gif" alt="" /></div>
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1 runat="server">Feat:</h1>
                <h1 runat="server" id="featTitle"></h1>
            </hgroup>
            <div runat="server" id="FeatBlock" class="detailsViewDiv"></div>
            <hr />
            <small><b>Filter</b> on all columns at once: </small><input class="search" type="search" data-column="all">
            <br />
            <small>Reset all column filters, including Master Filter (above): </small><button type="button" class="reset">Reset All Filters</button><br />
            <div class="columnSelectorWrapper">
                <input id="colSelect1" type="checkbox" class="hidden">
                <label class="columnSelectorButton" for="colSelect1">Show/Hide Columns:</label>
                <div id="columnSelector" class="columnSelector"></div>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" class="noPad">
        <asp:Label runat="server" ID="Label1" CssClass="mobileWarning"><small><b>Mobile Users:</b> you may need to scroll left/right on this list</small></asp:Label>
    </div>
    <button type="button" class="ShowHideButton">Show/Hide Filtering Instructions</button> <span><small>(Hint: Use SHIFT key to sort multiple columns.)</small></span>
    <div runat="server" class="noPad NotesDiv" id="notes"></div>
    <div runat="server" id="RanksBlock" class="GridViewDiv">
    </div>
</asp:Content>
