<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.StockDetailsControls" %>
<asp:GridView ID="ItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemNameHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="itemTypeHyperLink" runat="server" NavigateUrl='<%# "~/ItemList?type=" + HttpUtility.UrlEncode(Eval("ItemType.Name").ToString()) %>' Text='<%# Eval("ItemType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tier" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label ID="TierLabel1" runat="server" Text='<%# Eval("Tier") %>' />
                </p>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Encumbrance" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label ID="EncumbranceLabel" runat="server" Text='<%# Eval("Encumbrance") %>' />
                </p>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Variety" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label ID="VarietyLabel" runat="server" Text='<%# Eval("Variety_Name") %>' />
                </p>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Stocks" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="StocksListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="StocksHyperLink" runat="server" NavigateUrl='<%# "~/StockDetails?stock=" + HttpUtility.UrlEncode(Eval("Stock_Name").ToString()) %>' Text='<%# Eval("Stock_Name") %>'></asp:HyperLink>
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:GridView ID="RecipesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Recipe Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="RecipeNameHyperLink" runat="server" NavigateUrl='<%# "~/RecipeDetails?recipe=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Feat Name (Rank)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="FeatHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>'></asp:HyperLink> (<asp:Label runat="server" ID="RankLabel" Text='<%# Eval("Feat_Rank") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tier" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label ID="TierLabel2" runat="server" Text='<%# Eval("Tier") %>' />
                </p>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>