<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EffectDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.EffectDetailsControls" %>
<asp:GridView ID="FeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
<%--        <asp:BoundField DataField="Role_Name" HeaderText="Role" SortExpression="" />--%>
        <asp:TemplateField HeaderText="Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("FeatType.Name").ToString()) %>' Text='<%# Eval("FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Effects" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder ID="FeatEffectsPlaceholder" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>
<%--        <asp:TemplateField HeaderText="Keywords (Rank)" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="FeatKeywordsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink> <asp:Label ID="Keyword" runat="server" Text='<%# "(" + Eval("Feat_Rank") + ")" %>' />
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
</asp:GridView>
<asp:GridView ID="FeatsGridView3" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Feat Name (Rank)" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Feat_Name").ToString()) %>' Text='<%# Eval("Feat_Name") %>'></asp:HyperLink> (<asp:Label runat="server" ID="rank" Text='<%# Eval("Rank") %>'></asp:Label>)
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Effects This Rank" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder id="FeatRankEffectsPlaceholder" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>
<%--        <asp:TemplateField HeaderText="Effects" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder ID="FeatEffectsPlaceholder3" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
</asp:GridView>
<asp:ListView runat="server" ID="EffectListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Effect Term:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Term") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Effect Type:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("EffectType_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Channel:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Channel_Name") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Math Specifics:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("MathSpecifics") %>' CssClass="detailsListData"></asp:Label>
        </li>
    </ItemTemplate>
</asp:ListView>