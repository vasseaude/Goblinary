<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mule.ascx.cs" Inherits="Goblinary.Website.Controls.Item.Mule" %>
<asp:GridView ID="ItemsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="Item Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemHyperLink" runat="server" NavigateUrl='<%# "~/ItemDetails?item=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Item Type" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="itemTypeHyperLink" runat="server" NavigateUrl='<%# "~/ItemList?type=" + HttpUtility.UrlEncode(Eval("ItemType_Name").ToString()) %>' Text='<%# Eval("ItemType_Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="" />
        <asp:BoundField DataField="Encumbrance" HeaderText="Encumbrance" SortExpression="" />
        <asp:BoundField DataField="ItemCategory_Name" HeaderText="Item Category" SortExpression="" />
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
			<asp:Label runat="server" Text="Recipe(s) that make this item:" CssClass="detailsListField"></asp:Label>
			<asp:ListView runat="server" ID="ItemRecipesListView">
				<LayoutTemplate>
					<ul class="detailsList">
						<ul runat="server" id="itemPlaceholder" class="detailsList"></ul>
					</ul>
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
					<asp:Label runat="server" Text='<%# Eval("ItemName") %>' />
					+<asp:Label runat="server" Text='<%# Eval("Upgrade") %>' />
				</p>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
