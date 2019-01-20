<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecipeDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.RecipeDetailsControls" %>
<asp:ListView runat="server" ID="RecipeDetailsListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Feat (Rank):" CssClass="detailsListField"></asp:Label>
            <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>' CssClass="detailsListData"></asp:HyperLink> (<asp:Label runat="server" Text='<%# Eval("Feat_Rank") %>' CssClass="detailsListData"></asp:Label>)
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Tier:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Tier") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Output Item (Qty):" CssClass="detailsListField"></asp:Label>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("OutputItem_Name").ToString()) %>' Text='<%# Eval("OutputItem_Name") %>' CssClass="detailsListData"></asp:HyperLink> (<asp:Label runat="server" Text='<%# Eval("QtyProduced") %>' CssClass="detailsListData"></asp:Label>)
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Base Crafting Seconds:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("BaseCraftingSeconds") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Quality:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Quality") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Achievement Type:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("AchievementType_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
    </ItemTemplate>
</asp:ListView>