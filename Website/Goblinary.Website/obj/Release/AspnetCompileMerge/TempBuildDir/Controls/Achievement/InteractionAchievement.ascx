﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InteractionAchievement.ascx.cs" Inherits="Goblinary.Website.Controls.Achievement.InteractionAchievement" %>
<asp:DetailsView ID="AchievementDetailsView" runat="server" AutoGenerateRows="false" CssClass="detailsView">
    <Fields>
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />
    </Fields>
</asp:DetailsView>
<asp:GridView ID="AchievementRanksGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
		<asp:BoundField DataField="DisplayName" HeaderText="Achievement Rank" SortExpression="" />
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />
		<asp:BoundField DataField="InteractionKeyword_Name" HeaderText="Interaction Keyword" SortExpression="" />
		<asp:BoundField DataField="Value" HeaderText="How Many" SortExpression="" />
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