<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatAchievement.ascx.cs" Inherits="Goblinary.Website.Controls.Achievement.FeatAchievement" %>
<asp:DetailsView ID="AchievementDetailsView" runat="server" AutoGenerateRows="false" CssClass="detailsView">
    <Fields>
        <%--<asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />--%>
    </Fields>
</asp:DetailsView>
<asp:GridView ID="AchievementRanksGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
		<asp:BoundField DataField="DisplayName" HeaderText="Achievement Rank" SortExpression="" />
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" ItemStyle-CssClass="wrap" />
        <asp:TemplateField HeaderText="Feat Requirements" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankFeatRequirementsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="RankFeatRequirement" runat="server" Text='<%# Eval("Requirement") %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>