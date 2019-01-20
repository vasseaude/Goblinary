<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockListControls.ascx.cs" Inherits="Goblinary.Website.Controls.StockListControls" %>
<asp:GridView ID="StocksGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Stock Name" SortExpression="">
            <ItemTemplate>
                <span runat="server" style="white-space: nowrap;">
                    <asp:HyperLink ID="stockHyperLink" runat="server" NavigateUrl='<%# "~/StockDetails?stock=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </span>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>