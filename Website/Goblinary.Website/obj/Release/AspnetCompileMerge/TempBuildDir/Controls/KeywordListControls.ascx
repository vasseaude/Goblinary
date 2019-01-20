<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KeywordListControls.ascx.cs" Inherits="Goblinary.Website.Controls.KeywordListControls" %>
<asp:GridView ID="KeywordsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="keywordHyperLink" runat="server" NavigateUrl='<%# "~/KeywordDetails?keyword=" + HttpUtility.UrlEncode(Eval("Name").ToString()) + "&type=" + HttpUtility.UrlEncode(Eval("KeywordType_Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="KeywordType_Name" HeaderText="Keyword Type" SortExpression="" />
        <asp:BoundField DataField="Value_Name" HeaderText="Value" SortExpression="" />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="" />
    </Columns>
</asp:GridView>