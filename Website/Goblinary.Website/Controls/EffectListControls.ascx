<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EffectListControls.ascx.cs" Inherits="Goblinary.Website.Controls.EffectListControls" %>
<asp:GridView ID="EffectsGridView" runat="server" CssClass="tablesorter custom-popup" AutoGenerateColumns="false" >
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="">
            <ItemTemplate>
                <asp:HyperLink ID="featHyperLink" runat="server" NavigateUrl='<%# "~/EffectDetails?effect=" + HttpUtility.UrlEncode(Eval("Name").ToString()) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="EffectTerm.Term" HeaderText="Effect Term" SortExpression="" />
        <asp:BoundField DataField="EffectTerm.EffectType_Name" HeaderText="Effect Type" SortExpression="" />
        <asp:BoundField DataField="EffectTerm.Channel_Name" HeaderText="Channel" SortExpression="" />
        <asp:BoundField DataField="EffectTerm.MathSpecifics" HeaderText="Math Specifics" SortExpression="" ItemStyle-CssClass="wrap" />
        <asp:BoundField DataField="EffectTerm.Description" HeaderText="Description" SortExpression="" ItemStyle-CssClass="wrap" />
    </Columns>
</asp:GridView>