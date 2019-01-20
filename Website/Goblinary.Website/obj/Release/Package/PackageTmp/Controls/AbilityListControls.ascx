<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbilityListControls.ascx.cs" Inherits="Goblinary.Website.Controls.AbilityListControls" %>
<asp:GridView ID="AbilitiesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Ability Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/AbilityDetails?ability=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>