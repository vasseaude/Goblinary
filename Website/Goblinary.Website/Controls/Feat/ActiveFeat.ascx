<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveFeat.ascx.cs" Inherits="Goblinary.Website.Controls.Feat.ActiveFeat" %>
<asp:GridView ID="FeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="False">
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
        <%--keywords--%>
        <asp:BoundField DataField="WeaponCategory_Name" HeaderText="Weapon Category" SortExpression="" />
        <asp:BoundField DataField="Range" HeaderText="Range" SortExpression="" />
        <asp:BoundField DataField="AttackSeconds" HeaderText="Atk" SortExpression="" DataFormatString="{0:F1}" />
        <asp:BoundField DataField="DamageFactor" HeaderText="DF" SortExpression="" DataFormatString="{0:F2}" />
        <asp:BoundField DataField="StaminaCost" HeaderText="St" SortExpression="" />
        <asp:BoundField DataField="DamageFactorPerAttackSeconds" HeaderText="<u>DF</u><br />Atk" SortExpression="" DataFormatString="{0:F2}" HtmlEncode="false" />
        <asp:BoundField DataField="StaminaCostPerDamageFactor" HeaderText="<u>Stam</u><br />DF" SortExpression="" DataFormatString="{0:F2}" HtmlEncode="false"  />
        <asp:TemplateField HeaderText="Effects" SortExpression="">
            <ItemTemplate>
                <asp:PlaceHolder id="FeatEffectsPlaceholder" runat="server"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:TemplateField>
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