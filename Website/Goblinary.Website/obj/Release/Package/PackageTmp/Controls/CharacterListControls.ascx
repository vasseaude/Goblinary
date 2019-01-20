<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CharacterListControls.ascx.cs" Inherits="Goblinary.Website.Controls.CharacterListControls" %>
<asp:Panel ID="CharacterListPanel" runat="server" DefaultButton="CharacterNameSubmit" CssClass="characterListPanel">
    <asp:Button ID="CharacterSelect" runat="server" Text="Select" CssClass="" UseSubmitBehavior="false" />
    <br />
    <asp:Label ID="CharacterNameLabel" runat="server" /><br />
    <asp:Label ID="CharacterTrainedFeatsLabel" runat="server" /><br />
	<asp:RadioButtonList ID="CharacterShareStatusRadio" runat="server" RepeatDirection="Horizontal" CssClass="shareStatusRadioList">
		<asp:ListItem>Private</asp:ListItem>
		<asp:ListItem>Shared</asp:ListItem>
		<asp:ListItem>Public</asp:ListItem>
	</asp:RadioButtonList>
    <hr />
    <p class="noPad">Character operations:</p>
    <asp:Button ID="CharacterDuplicate" runat="server" Text="Duplicate" CssClass="" UseSubmitBehavior="false" />
    <asp:Button ID="CharacterDelete" runat="server" Text="Delete" CssClass="" UseSubmitBehavior="false" />
	<asp:Button ID="CharacterDetails" runat="server" Text="Details" CssClass="" UseSubmitBehavior="false" />
    <hr />
    <p class="noPad">Change character name:</p>
    <asp:TextBox ID="CharacterNameTextBox" runat="server" CssClass="" />
    <asp:Button ID="CharacterNameSubmit" runat="server" Text="Update Name" CssClass="" UseSubmitBehavior="false" /><br />
</asp:Panel>
<asp:Panel ID="CreateNewCharacterPanel" runat="server" DefaultButton="NewCharacterNameSubmit" CssClass="characterListPanel">
    <p class="noPad">Create new character:</p>
    <asp:TextBox ID="NewCharacterNameTextBox" runat="server" CssClass="" />
    <asp:Button ID="NewCharacterNameSubmit" runat="server" Text="Create" CssClass="" UseSubmitBehavior="false" />
</asp:Panel>