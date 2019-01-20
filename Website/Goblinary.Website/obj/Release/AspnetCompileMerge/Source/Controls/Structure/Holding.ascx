<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Holding.ascx.cs" Inherits="Goblinary.Website.Controls.Structure.Holding" %>
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
    </Columns>
</asp:GridView>
<asp:GridView ID="StructureUpgradeGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false">
	<Columns>
        <asp:TemplateField HeaderText="Upgrade" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Structure_Name") + " +" + Eval("Upgrade") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Influcence Cost" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("InfluenceCost") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Facility (Quality)" SortExpression="">
            <ItemTemplate>
				<asp:Label runat="server" Text='<%# Eval("FacilityFeat") %>' />
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField HeaderText="Trainer Levels" SortExpression="">
			<ItemTemplate>
				<asp:ListView runat="server" ID="TrainerLevelsListView">
					<ItemTemplate>
						<p runat="server" class="noPad">
							<asp:Label runat="server" Text='<%# Eval("Trainer_Name") + " " + Eval("Level") %>' />
						</p>
					</ItemTemplate>
				</asp:ListView>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Bulk Resource Bonuses" SortExpression="">
			<ItemTemplate>
				<asp:ListView runat="server" ID="BulkResourceBonusesListView">
					<ItemTemplate>
						<p runat="server" class="noPad">
							<asp:Label runat="server" Text='<%# Eval("BulkResource_Name") + " (" + Eval("Bonus") + ")" %>' />
						</p>
					</ItemTemplate>
				</asp:ListView>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Bulk Resource Requirements" SortExpression="">
			<ItemTemplate>
				<asp:ListView runat="server" ID="BulkResourceRequirementsListView">
					<ItemTemplate>
						<p runat="server" class="noPad">
							<asp:Label runat="server" Text='<%# Eval("BulkResource_Name") + " (" + Eval("Requirement") + ")" %>' />
						</p>
					</ItemTemplate>
				</asp:ListView>
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
            <asp:Label runat="server" Text="Kit:" CssClass="detailsListField"></asp:Label>
            <asp:Label runat="server" Text='<%# Eval("KitName") %>' CssClass="detailsListData"></asp:Label>
        </li>
    </ItemTemplate>
</asp:ListView>