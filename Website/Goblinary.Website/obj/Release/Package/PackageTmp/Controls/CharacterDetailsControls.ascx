<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CharacterDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.CharacterDetailsControls" %>
<asp:GridView ID="AbilitiesGridView" runat="server" CssClass="abilityGrid" AutoGenerateColumns="False" AlternatingRowStyle-CssClass="abilityGridAltRowStyle" RowStyle-CssClass="abilityGridRowStyle" HeaderStyle-CssClass="abilityGridHeader">
    <Columns>
        <asp:TemplateField HeaderText="Ability" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/AbilityDetails?ability=" + HttpUtility.UrlEncode(Eval("Ability_Name").ToString()) %>' Text='<%# Eval("Ability_Name") %>' />
                </span>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Trained Score" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("TrainedScore") %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Wishlist Score" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("WishListScore") %>' />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:GridView ID="FeatGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
		<asp:TemplateField HeaderText="Feat Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("SampleFeat_Name").ToString()) %>' Text='<%# Eval("AdvancementFeat_Name") %>' />
                </span>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Feat Type" SortExpression="">
			<ItemTemplate>
				<span runat="server" style="white-space: nowrap;">
					<asp:HyperLink runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("FeatType_Name").ToString()) %>' Text='<%# Eval("FeatType_Name") %>' />
				</span>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Trained Rank" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("TrainedRank") %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Trained XP Cost" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("TrainedExpCost") %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="WishList Rank" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("WishListRank") %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="WishList XP Cost" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("WishListExpCost") %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="WishList XP Deficit" SortExpression="">
			<ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("WishListDeficit") %>' />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:GridView ID="EarnedAchievementGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Achievement Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/AchievementDetails?achievement=" + HttpUtility.UrlEncode(Eval("Achievement_Name").ToString()) %>' Text='<%# Eval("Achievement_Name") %>' />
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rank" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("EarnedRank") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:GridView ID="WishListAchievementGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Achievement Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/AchievementDetails?achievement=" + HttpUtility.UrlEncode(Eval("Achievement_Name").ToString()) %>' Text='<%# Eval("Achievement_Name") %>' />
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rank" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("WishListRank") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>