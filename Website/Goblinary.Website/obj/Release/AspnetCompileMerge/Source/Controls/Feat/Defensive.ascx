<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Defensive.ascx.cs" Inherits="Goblinary.Website.Controls.Feat.Defensive" %>
<asp:DetailsView ID="FeatDetailsView" runat="server" AutoGenerateRows="false" CssClass="detailsView">
    <RowStyle CssClass="detailsViewRowStyle" /><AlternatingRowStyle CssClass="detailsViewAltRowStyle" /><FieldHeaderStyle CssClass="detailsViewLeft" /><HeaderStyle CssClass="detailsViewHeader" />
    <HeaderTemplate>Available Feat Details:</HeaderTemplate>
    <Fields>
        <asp:BoundField DataField="Role_Name" HeaderText="Role:" SortExpression="" />
        <asp:BoundField DataField="Channel_Name" HeaderText="Channel:" SortExpression="" />
        <asp:BoundField DataField="AdvancementFeat_Name" HeaderText="Advancement Feat:" SortExpression="" />
        <asp:TemplateField HeaderText="Related Feats (Type):" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RelatedFeatsListView">
                    <ItemTemplate>
                            <p runat="server" class="noPad">
                                <asp:HyperLink ID="RelatedFeatHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") + " (" + Eval("FeatType.DisplayName") + ")" %>'></asp:HyperLink>
                            </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>
<asp:GridView ID="FeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <%--rank--%>
        <asp:BoundField DataField="Role_Name" HeaderText="Role" SortExpression="" />
        <asp:BoundField DataField="Channel_Name" HeaderText="Channel" SortExpression="" />
<%--        <asp:TemplateField HeaderText="Effects" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder id="FeatEffectsPlaceholder" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>--%>
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