<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TablesorterNotes.ascx.cs" Inherits="Goblinary.Website.Controls.TablesorterNotes" %>
<asp:Table CssClass="tablesorter-blue notes" ID="TablesorterNotesTable" runat="server">
    <asp:TableHeaderRow runat="server">
        <asp:TableHeaderCell>Filter Type</asp:TableHeaderCell>
        <asp:TableHeaderCell>Description and Examples</asp:TableHeaderCell>
    </asp:TableHeaderRow>
    <asp:TableRow runat="server">
        <asp:TableCell>text</asp:TableCell>
        <asp:TableCell><p class="noPad">Any text entered in the filter will <strong>match</strong> text found within the column.</p>
        <p class="noPad"><code>attack</code> (finds "Base Attack Bonus", "Ranged Attack Bonus", "Bleeding Attack", etc);</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>/\d/</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Add any <b>regex</b> to the query to use in the query ("mig" flags can be included <code>/\w/mig</code>)</p>
        <p class="noPad"><code>/b[aeiou]s/i</code> (finds "Base Attack Bonus", "Bestow Curse", etc);</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>&lt;</code> <code>&lt;=</code> <code>&gt;=</code> <code>&gt;</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Find alphabetical or numerical values less than or greater than or equal to the filtered query.</p>
        <p class="noPad"><code>&gt;= 10</code> (find values greater than or equal to 10)</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>!</code> or <code>!=</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Not operator, or not exactly match. Filters the column with content that does <strong>not</strong> match the query. Include an equal (<code>=</code>), single (<code>'</code>) or double quote (<code>"</code>) to exactly <em>not</em> match a filter.</p>
        <p class="noPad"><code>!ge</code> (hide rows with "General" in that column, but shows "Fighter", "Cleric", "Rogue"); <code>!"Abjurer"</code> (find content that does not exactly match "Abjurer")</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>"</code> or <code>=</code></asp:TableCell>
        <asp:TableCell><p class="noPad">To exactly match the search query, add a quote, apostrophe or equal sign to the beginning and/or end of the query</p>
        <p class="noPad"><code>Aid"</code> or <code>Aid=</code> (exactly match "Aid")</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>&nbsp;&&&nbsp;</code> or <code>&nbsp;AND&nbsp;</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Logical "and". Filter the column for content that matches text from either side of the operator.</p>
        <p class="noPad"><code>channel && harm</code> (matches a cell that contains both "channel" and "harm");</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>&nbsp;-&nbsp;</code> or <code>&nbsp;to&nbsp; </code></asp:TableCell>
        <asp:TableCell><p class="noPad">Find a range of values. Make sure there is a space before and after the dash (or the word "to").</p>
        <p class="noPad"><code>10 - 30</code> or <code>10 to 30</code> (match values between 10 and 30)</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>?</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Wildcard for a single, non-space character.</p>
        <p class="noPad"><code>a?i</code> (finds "Acid Arrow" and "Agile Feet", but <b>not</b> "Alchemist")</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>*</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Wildcard for zero or more non-space characters.</p>
        <p class="noPad"><code>a*i</code> (matches "Acid Arrow", "Agile Feet", and "Alchemist")</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>|</code> or <code>&nbsp;OR&nbsp; </code></asp:TableCell>
        <asp:TableCell><p class="noPad">Logical "or" (Vertical bar). Filter the column for content that matches text from either side of the bar.</p>
        <p class="noPad"><code>rogue|fighter</code> (matches either "Rogue Maneuver" or "Fighter Maneuver")</p></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell><code>~</code></asp:TableCell>
        <asp:TableCell><p class="noPad">Perform a fuzzy search (matches sequential characters) by adding a tilde to the beginning of the query.</p>
        <p class="noPad"><code>~baa</code> (matches "Balestra and "Bleeding Attack"), or <code>~paizo</code> (matches "Axe Specialization")</p></asp:TableCell>
    </asp:TableRow>
</asp:Table>