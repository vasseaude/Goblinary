<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeatDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.FeatDetailsControls" %>
<asp:GridView ID="FeatRanksGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Trained" SortExpression="">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Wish List" SortExpression="">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rank" SortExpression="">
            <ItemTemplate>
                <asp:Label ID="RankLabel" runat="server" Text='<%# Eval("Rank") %>' />
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Trainer Levels" SortExpression="">
			<ItemTemplate>
				<asp:ListView runat="server" ID="TrainerLevelsListView">
					<ItemTemplate>
						<p runat="server" class="noPad">
							<asp:Label ID="TrainerLevelsLabel" runat="server" Text='<%# Eval("Trainer") + " " + Eval("Level") %>' />
						</p>
					</ItemTemplate>
				</asp:ListView>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="XP Cost" SortExpression="">
			<ItemTemplate>
				<p runat="server" class="noPad">
					<asp:Label ID="ExpCostLabel" runat="server" Text='<%# Eval("ExpCost") %>' />
				</p>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Coin Cost" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label ID="CoinCostLabel" runat="server" Text='<%# Eval("CoinCost") %>' />
                </p>
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
        <asp:TemplateField HeaderText="Achievement Requirements" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankAchievementRequirementsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="RankAchievementRequirement" runat="server" Text='<%# Eval("Requirement") %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Category Requirements" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankCategoryRequirementsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:Label ID="RankCategoryRequirement" runat="server" Text='<%# Eval("Requirement") %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
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
        <asp:TemplateField HeaderText="Rank Effects" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder id="FeatRankEffectsPlaceholder" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Keywords" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="FeatKeywordsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink>
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>