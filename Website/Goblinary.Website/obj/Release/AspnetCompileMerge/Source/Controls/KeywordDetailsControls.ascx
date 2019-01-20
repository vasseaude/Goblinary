<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KeywordDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.KeywordDetailsControls" %>
<asp:GridView ID="SourceFeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Feat Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Feat Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("FeatType.Name").ToString()) %>' Text='<%# Eval("FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Keywords (Rank)" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="SourceFeatsKeywordsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink> <asp:Label ID="Keyword" runat="server" Text='<%# "(" + Eval("FeatRank.Rank") + ")" %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:GridView ID="MatchingFeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Feat Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Feat Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("FeatType.Name").ToString()) %>' Text='<%# Eval("FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Keywords (Rank)" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="MatchingFeatsKeywordsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink> <asp:Label ID="Keyword" runat="server" Text='<%# "(" + Eval("FeatRank.Rank") + ")" %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:GridView ID="MatchingItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
		<asp:TemplateField HeaderText="Tier" SortExpression="">
			<ItemTemplate>
				<p runat="server" class="noPad">
					<asp:Label ID="TierLabel" runat="server" Text='<%# Eval("Tier") %>' />
				</p>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/ItemList?type=" + HttpUtility.UrlEncode(Eval("ItemType_Name").ToString()) %>' Text='<%# Eval("ItemType_Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Keywords (Upgrade)" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="MatchingItemsKeywordsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="keywordHyperLink" runat="server" cssClass='<%# Eval("KeywordKind_Name") + "_ItemKeywordCSS " + Eval("Keyword.Value_Name") + "_ItemKeywordCSS" %>' NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink> <asp:Label ID="Keyword" runat="server" cssClass='<%# Eval("KeywordKind_Name") + "_ItemKeywordUpgradeCSS " + Eval("Keyword.Value_Name") + "_ItemKeywordUpgradeCSS" %>' Text='<%# "(+" + Eval("Upgrade") + ")" %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ListView runat="server" ID="KeywordListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Keyword Value:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Value_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
    </ItemTemplate>
</asp:ListView>