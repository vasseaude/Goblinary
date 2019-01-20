<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChanneledFeat.ascx.cs" Inherits="Goblinary.Website.Controls.Feat.ChanneledFeat" %>
<asp:GridView ID="FeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featTypeHyperLink" runat="server" NavigateUrl='<%# "~/FeatList?type=" + HttpUtility.UrlEncode(Eval("FeatType.Name").ToString()) %>' Text='<%# Eval("FeatType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Role_Name" HeaderText="Role" SortExpression="" />
        <asp:BoundField DataField="Channel_Name" HeaderText="Channel" SortExpression="" />
        <asp:TemplateField HeaderText="Keywords (Rank)" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="FeatKeywordsListView">
                    <ItemTemplate>
                            <p runat="server" class="noPad">
                                <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Keyword_Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Keyword_Name") %>'></asp:HyperLink> <asp:Label ID="Keyword" runat="server" Text='<%# "(" + Eval("Feat_Rank") + ")" %>' />
                            </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>