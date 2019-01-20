<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Camp.ascx.cs" Inherits="Goblinary.Website.Controls.Structure.Camp" %>
<asp:GridView ID="StructuresGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Structure Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="structureHyperLink" runat="server" NavigateUrl='<%# "~/StructureDetails?structure=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Kit" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("KitName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="" />
        <%--<asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />--%>
        <%--<asp:BoundField DataField="Quality" HeaderText="Quality" SortExpression="" />--%>
        <%--<asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />--%>
        <%--<asp:BoundField DataField="Category" HeaderText="Category" SortExpression="" />--%>
        <%--<asp:BoundField DataField="HouseEntityDefn" HeaderText="House Entity Definition" SortExpression="" />--%>
        <asp:BoundField DataField="Cooldown" HeaderText="Cooldown" SortExpression="" />
        <%--<asp:BoundField DataField="NoLoot" HeaderText="No Loot" SortExpression="" />--%>
        <%--<asp:BoundField DataField="Upgradable" HeaderText="Upgradable" SortExpression="" />--%>
    </Columns>
</asp:GridView>
<asp:GridView ID="StructureUpgradeGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
	<Columns>
        <asp:TemplateField HeaderText="Upgrade" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Structure_Name") + " +" + Eval("Upgrade") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Power Regeneration" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("PowerRegenerationSeconds") %>' />
            </ItemTemplate>
        </asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:ListView runat="server" ID="StructureDetailsListView">
    <LayoutTemplate>
        <ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Description:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Description") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Kit:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("KitName") %>' CssClass="detailsListData"></asp:Label>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Cooldown:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("Cooldown") %>' CssClass="detailsListData"></asp:Label>
        </li>
<%--        <li runat="server" class="detailsListItem">
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
        </li>--%>
    </ItemTemplate>
</asp:ListView>