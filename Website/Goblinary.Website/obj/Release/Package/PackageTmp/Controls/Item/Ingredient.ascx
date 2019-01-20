<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ingredient.ascx.cs" Inherits="Goblinary.Website.Controls.Item.Ingredient" %>
<asp:GridView ID="ItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Type" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemTypeHyperLink" runat="server" NavigateUrl='<%# "~/ItemList?type=" + HttpUtility.UrlEncode(Eval("ItemType_Name").ToString()) %>' Text='<%# Eval("ItemType_Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />
        <asp:BoundField DataField="Variety_Name" HeaderText="Variety" SortExpression="" />
    </Columns>
</asp:GridView>