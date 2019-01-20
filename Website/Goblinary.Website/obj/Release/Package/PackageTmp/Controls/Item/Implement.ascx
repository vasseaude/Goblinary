<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Implement.ascx.cs" Inherits="Goblinary.Website.Controls.Item.Implement" %>
<asp:GridView ID="ItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />
        <asp:TemplateField HeaderText="Implement Type" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("ImplementType_Name").ToString() + " Implement Proficiency") %>' Text='<%# Eval("ImplementType_Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ListView runat="server" ID="ItemListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Tier:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Tier") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Encumbrance:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Encumbrance") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Implement Type:" CssClass="detailsListField"></asp:Label>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "~/FeatDetails?feat=" + HttpUtility.UrlEncode(Eval("ImplementType_Name").ToString() + " Implement Proficiency") %>' Text='<%# Eval("ImplementType_Name") %>' CssClass="detailsListData"></asp:HyperLink>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Recipe(s) that make this item:" CssClass="detailsListField"></asp:Label>
            <asp:ListView runat="server" ID="ItemRecipesListView">
                <LayoutTemplate>
                    <ul class="detailsList"><ul runat="server" id="itemPlaceholder" class="detailsList"></ul></ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server" class="detailsListItem">
                        <asp:HyperLink runat="server" NavigateUrl='<%# "~/RecipeDetails?recipe=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>' CssClass="detailsListData"></asp:HyperLink>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
    </ItemTemplate>
</asp:ListView>
<asp:GridView ID="ItemBonusesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Upgrade" SortExpression="">
            <ItemTemplate>
                <p runat="server" class="noPad">
                    <asp:Label runat="server" Text='<%# Eval("ItemName") %>' /> +<asp:Label runat="server" Text='<%# Eval("Upgrade") %>' />
                </p>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>