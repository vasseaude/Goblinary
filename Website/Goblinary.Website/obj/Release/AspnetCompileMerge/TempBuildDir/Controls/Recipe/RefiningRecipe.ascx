<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefiningRecipe.ascx.cs" Inherits="Goblinary.Website.Controls.Recipe.RefiningRecipe" %>
<asp:GridView ID="RecipesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Recipe Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/RecipeDetails?recipe=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Feat Name (Rank)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>'></asp:HyperLink> (<asp:Label runat="server" Text='<%# Eval("Feat_Rank") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:TemplateField HeaderText="Output Item (Qty)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("OutputItem_Name").ToString()) %>' Text='<%# Eval("OutputItem_Name") %>'></asp:HyperLink> (<asp:Label runat="server" Text='<%# Eval("QtyProduced") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="BaseCraftingSeconds" HeaderText="Base Crafting Seconds" SortExpression="" />
        <asp:BoundField DataField="Quality" HeaderText="Quality" SortExpression="" />
        <asp:BoundField DataField="AchievementType_Name" HeaderText="Achievement Type" SortExpression="" />
        <asp:BoundField DataField="Upgrade" HeaderText="Upgrade" SortExpression="" />
    </Columns>
</asp:GridView>