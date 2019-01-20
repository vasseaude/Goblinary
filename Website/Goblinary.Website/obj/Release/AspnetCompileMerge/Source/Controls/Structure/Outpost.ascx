<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Outpost.ascx.cs" Inherits="Goblinary.Website.Controls.Structure.Outpost" %>
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
        <asp:TemplateField HeaderText="Effort Bonus" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("EffortBonus") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Influence Cost" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("InfluenceCost") %>' />
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
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Bulk Resources Produced:" CssClass="detailsListField"></asp:Label>
            <asp:ListView runat="server" ID="BulkResourcesListView">
                <LayoutTemplate>
                    <ul class="detailsList"><ul runat="server" id="itemPlaceholder" class="detailsList"></ul></ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server" class="detailsListItem">
                        <asp:Label runat="server" Text='<%# Eval("Percentage") + "% " + Eval("BulkResource_Name") + " from " + Eval("BulkRating_Name") %>' />
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
        <li runat="server" class="detailsListItem">
            <asp:Label runat="server" Text="Worker Feats:" CssClass="detailsListField"></asp:Label>
            <asp:ListView runat="server" ID="WorkerFeatsListView">
                <LayoutTemplate>
                    <ul class="detailsList"><ul runat="server" id="itemPlaceholder" class="detailsList"></ul></ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li runat="server" class="detailsListItem">
                        <asp:Label runat="server" Text='<%# Eval("WorkerFeat") %>' />
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
    </ItemTemplate>
</asp:ListView>