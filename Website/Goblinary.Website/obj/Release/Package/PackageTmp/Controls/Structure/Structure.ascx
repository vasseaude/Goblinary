<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Structure.ascx.cs" Inherits="Goblinary.Website.Controls.Structure.Structure" %>
<asp:GridView ID="StructuresGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Structure Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="structureHyperLink" runat="server" NavigateUrl='<%# "~/StructureDetails?structure=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Structure Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="structureTypeHyperLink" runat="server" NavigateUrl='<%# "~/StructureList?type=" + HttpUtility.UrlEncode(Eval("StructureType_Name").ToString()) %>' Text='<%# Eval("StructureType_Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Kit" SortExpression="">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("KitName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>