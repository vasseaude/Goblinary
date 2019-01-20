<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefiningAchievement.ascx.cs" Inherits="Goblinary.Website.Controls.Achievement.RefiningAchievement" %>
<asp:DetailsView ID="AchievementDetailsView" runat="server" AutoGenerateRows="false" CssClass="detailsView">
    <Fields>
        <%--<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />--%>
    </Fields>
</asp:DetailsView>
<asp:GridView ID="AchievementRanksGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
		<asp:BoundField DataField="DisplayName" HeaderText="Achievement Rank" SortExpression="" />
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />
        <asp:TemplateField HeaderText="Feat Required" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featRequiredHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Bind("Feat_Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
		<asp:BoundField DataField="Rarity_Name" HeaderText="Rarity" SortExpression="" />
		<asp:BoundField DataField="Upgrade" HeaderText="Upgrade" SortExpression="" />
        <asp:TemplateField HeaderText="Category Points" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankCategoryPointsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="featRequiredLabel" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>