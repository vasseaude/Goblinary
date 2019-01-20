<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryListControls.ascx.cs" Inherits="Goblinary.Website.Controls.CategoryListControls" %>
<asp:GridView ID="CategoriesGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Category Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/CategoryDetails?category=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>