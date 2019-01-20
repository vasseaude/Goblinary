<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AbilityDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.AbilityDetailsControls" %>
<asp:GridView ID="BonusGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Feat Name (Rank)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featRankHyperLink1" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>'></asp:HyperLink> (<asp:Label runat="server" ID="rank" Text='<%# Eval("Rank") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("Feat.FeatType.Name").ToString()) %>' Text='<%# Eval("Feat.FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ability Bonuses" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankAbilityBonusesListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="RankAbilityBonus" runat="server" Text='<%# Eval("Bonus") %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Bonus per 1m XP" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("AbilityBonusPerXP") %>' />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:GridView ID="RequirementGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Feat Name (Rank)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featRankHyperLink2" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>'></asp:HyperLink> (<asp:Label runat="server" ID="rank" Text='<%# Eval("Rank") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("Feat.FeatType.Name").ToString()) %>' Text='<%# Eval("Feat.FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Ability Requirements" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankAbilityRequirementsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="RankAbilityRequirement" runat="server" Text='<%# Eval("Requirement") %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>