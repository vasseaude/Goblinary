<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AchievementListControls.ascx.cs" Inherits="Goblinary.Website.Controls.AchievementListControls" %>
<asp:GridView ID="AchievementsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="False" EmptyDataText="Something went wrong - couldn't get a list of achievements. That's bad.">
    <Columns>
        <asp:TemplateField HeaderText="Achievement Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="achievementHyperLink" runat="server" NavigateUrl='<%# "~/AchievementDetails?achievement=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Bind("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Achievement Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="achievementTypeHyperLink" runat="server" NavigateUrl='<%# "~/AchievementList?type=" + HttpUtility.UrlEncode(Eval("AchievementType.Name").ToString()) %>' Text='<%# Bind("AchievementType.DisplayName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Category" SortExpression="">
            <ItemTemplate>
                <asp:ListView runat="server" ID="RankCategoryPointsListView">
                    <ItemTemplate>
                        <p runat="server" class="noPad">
                            <asp:HyperLink ID="featRequiredHyperLink" runat="server" NavigateUrl='<%# "~/CategoryDetails?category=" + HttpUtility.UrlEncode(Eval("Category_Name").ToString()) %>' Text='<%# Bind("Category_Name") %>'></asp:HyperLink>
                        </p>
                    </ItemTemplate>
                </asp:ListView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
