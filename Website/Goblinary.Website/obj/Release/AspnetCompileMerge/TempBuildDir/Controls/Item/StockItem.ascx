<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockItem.ascx.cs" Inherits="Goblinary.Website.Controls.Item.StockItem" %>
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
        <asp:TemplateField HeaderText="Stocks" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="StockItemStocksListView">
                    <ItemTemplate>
                            <p runat="server" class="noPad">
                                <asp:HyperLink runat="server" NavigateUrl='<%# "~/StockDetails?stock=" + HttpUtility.UrlEncode(Eval("Stock_Name").ToString()) %>' Text='<%# Eval("Stock_Name") %>' />
                            </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>