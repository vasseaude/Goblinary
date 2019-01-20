<%@ Page Title="Character List/Manage" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CharacterList.aspx.cs" Inherits="Goblinary.Website.Account.CharacterList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function ConfirmDelete(characterName) {
            var returnInput = document.createElement("INPUT");
            returnInput.type = "hidden";
            returnInput.name = "confirm_value";
            if (confirm("Are you sure you want to DELETE \"" + characterName + "\"?")) {
                returnInput.value = "Yes";
            } else {
                returnInput.value = "No";
            }
            document.forms[0].appendChild(returnInput);
        }
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
                <h1>Character List/Manage</h1>
            </hgroup>
            <p class="noPad">Manage your characters or select a different character to view/train.</p>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="CharacterListDiv" class="GridViewDiv" />
	<div runat="server" id="CharacterFootnoteDiv" />
</asp:Content>
