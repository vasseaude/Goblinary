<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterSearchGrid.ascx.cs" Inherits="Goblinary.Website.Controls.MasterSearchGrid" %>
<asp:GridView ID="MasterSearchGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" EmptyDataText="">
    <Columns>
        <asp:TemplateField HeaderText="Result Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="achievementHyperLink" runat="server" NavigateUrl='<%# Eval("ResultURL") %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Result Type" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="typeHyperLink" runat="server" NavigateUrl='<%# "~/" + Eval("ResultType") + "List" %>' Text='<%# Eval("ResultType") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ResultSubtype" HeaderText="Result Subtype" SortExpression="" HtmlEncode="false"/>
    </Columns>
</asp:GridView>