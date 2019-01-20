<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Item.ascx.cs" Inherits="Goblinary.Website.Controls.Item.Item" %>

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
                    <asp:HyperLink ID="itemTypeHyperLink" runat="server" NavigateUrl='<%# "~/ItemList?type=" + HttpUtility.UrlEncode(Eval("ItemType.Name").ToString()) %>' Text='<%# Eval("ItemType.DisplayName") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />
    </Columns>
</asp:GridView>