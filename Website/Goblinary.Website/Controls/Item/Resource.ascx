<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resource.ascx.cs" Inherits="Goblinary.Website.Controls.Item.Resource" %>
<asp:GridView ID="ItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />
        <asp:BoundField DataField="Variety_Name" HeaderText="Variety" SortExpression="" />
        <asp:BoundField DataField="ResourceType_Name" HeaderText="Resource Type" SortExpression="" />
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
<asp:ListView runat="server" ID="ItemListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Tier:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Tier") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Encumbrance:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Encumbrance") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Variety:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Variety_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Resource Type:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("ResourceType_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Counts as stocks:" CssClass="detailsListField"></asp:Label>
            <asp:ListView runat="server" ID="StockItemStocksListView">
                <LayoutTemplate>
                    <ul class="detailsList"><ul runat="server" id="itemPlaceholder" class="detailsList"></ul></ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server" class="detailsListItem">
                        <asp:HyperLink runat="server" NavigateUrl='<%# "~/StockDetails?stock=" + HttpUtility.UrlEncode(Eval("Stock_Name").ToString()) %>' Text='<%# Eval("Stock_Name") %>' CssClass="detailsListDataSmallList" />
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Recipe(s) that require this item as an ingredient:" CssClass="detailsListField"></asp:Label>
            <asp:ListView runat="server" ID="StockItemRelatedRecipesListView">
                <LayoutTemplate>
                    <ul class="detailsList"><ul runat="server" id="itemPlaceholder" class="detailsList"></ul></ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server" class="detailsListItem">
                        <asp:HyperLink runat="server" NavigateUrl='<%# "~/RecipeDetails?recipe=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>' CssClass="detailsListDataSmallList"></asp:HyperLink>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
    </ItemTemplate>
</asp:ListView>
<asp:GridView ID="ItemBonusesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
    </Columns>
</asp:GridView>