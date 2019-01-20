<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatEffects.ascx.cs" Inherits="Goblinary.Website.Controls.FeatEffects" %>
<asp:ListView runat="server" ID="FeatEffectsListView">
    <ItemTemplate>
		<asp:Label runat="server" CssClass='<%# Eval("EffectType") + "_EffectCSS" %>'><%# Eval("Description") %></asp:Label>
		<br />
    </ItemTemplate>
</asp:ListView>