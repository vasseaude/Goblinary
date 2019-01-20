<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDetailsControls.ascx.cs" Inherits="Goblinary.Website.Controls.CategoryDetailsControls" %>
<asp:GridView ID="FeatsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
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
        <asp:BoundField DataField="Role_Name" HeaderText="Role" SortExpression="" />
    </Columns>
</asp:GridView>
<asp:GridView ID="AchievementsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Achievement Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="achievementHyperLink" runat="server" NavigateUrl='<%# "~/AchievementDetails?achievement=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Bind("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Achievement Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="achievementHyperLink" runat="server" NavigateUrl='<%# "~/AchievementList?type=" + HttpUtility.UrlEncode(Eval("AchievementType.Name").ToString()) %>' Text='<%# Bind("AchievementType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>