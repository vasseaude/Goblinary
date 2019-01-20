using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;

using Goblinary.WikiData.Model;
using Goblinary.WikiData.SqlServer;

namespace Goblinary.Website
{
    public partial class KeywordDetails : System.Web.UI.Page
    {
		internal static string GetLink(Keyword keyword)
		{
			return string.Format("<a href=\"/KeywordDetails?keyword={0}&type={1}\">{0}</a>", keyword.Name, keyword.KeywordType_Name);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            string QS_Keyword_Name = HttpUtility.UrlDecode(Request.QueryString["keyword"]);
            string QS_Type = HttpUtility.UrlDecode(Request.QueryString["type"]);
            Page.MetaDescription += String.Format(" Keyword details for {0} ({1}).", QS_Keyword_Name, QS_Type);
            Page.Title = String.Format("{0} ({1}) | Keyword", QS_Keyword_Name, QS_Type);
            keywordTitle.InnerHtml = QS_Keyword_Name;
            keywordTitle2.InnerHtml = String.Format("({0})", QS_Type); // set the 2nd header

            Label noFeatsLabel1 = new Label();
            noFeatsLabel1.Text = "No feats to show in this list.";
            Label noFeatsLabel2 = new Label();
            noFeatsLabel2.Text = "No feats to show in this list.";
            Label noItemsLabel = new Label();
            noItemsLabel.Text = "No items to show in this list.";

            using (WikiDataContext context = new WikiDataContext())
            {
                var keyword = (
                        from k in context.Set<Keyword>()
                        where k.Name == QS_Keyword_Name && k.KeywordType_Name == QS_Type
                        select k
                    ).First();

                Label KeywordNotesLabel = new Label();
                KeywordNotesLabel.CssClass = "descriptionLabel";

                if (keyword.Notes != null)
                {
                    KeywordNotesLabel.Text = keyword.Notes;
                    KeywordBlock.Controls.Add(KeywordNotesLabel);
                }
                else
                {
                    KeywordNotesLabel.Text = "Goblinworks has not provided notes for this keyword.";
                    KeywordBlock.Controls.Add(KeywordNotesLabel);
                }

                ListView keywordListView = (ListView)Page.LoadControl("~/Controls/KeywordDetailsControls.ascx").FindControl("KeywordListView");
                keywordListView.DataSource = new List<Keyword>() { keyword };
                keywordListView.DataBind();
                KeywordBlock.Controls.Add(keywordListView);

                if (keyword.SourceFeats != null && keyword.SourceFeats.Count() > 0)
                {
                    GridView gridControl1 = (GridView)Page.LoadControl("~/Controls/KeywordDetailsControls.ascx").FindControl("SourceFeatsGridView");
                    gridControl1.ID = "SourceFeatsGridView";
                    gridControl1.DataSource = keyword.SourceFeats;
                    gridControl1.RowDataBound += new GridViewRowEventHandler(gridControl1_RowDataBound);
                    gridControl1.DataBind();
                    if (gridControl1.Rows.Count > 0) { gridControl1.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl1.Attributes.Add("tableName", "a");
                    SourceFeatsBlock.Controls.Add(gridControl1);
                }
                else
                {
                    SourceFeatsBlock.Controls.Add(noFeatsLabel1);
                }

                if (keyword.MatchingFeats != null && keyword.MatchingFeats.Count() > 0)
                {
                    MatchingItemsBlock.Visible = false;

                    GridView gridControl2 = (GridView)Page.LoadControl("~/Controls/KeywordDetailsControls.ascx").FindControl("MatchingFeatsGridView");
                    gridControl2.ID = "MatchingFeatsGridView";
                    gridControl2.DataSource = keyword.MatchingFeats;
                    gridControl2.RowDataBound += new GridViewRowEventHandler(gridControl2_RowDataBound);
                    gridControl2.DataBind();
                    if (gridControl2.Rows.Count > 0) { gridControl2.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl2.Attributes.Add("tableName", "b");
                    MatchingFeatsBlock.Controls.Add(gridControl2);
                }
                else if (keyword.MatchingItems != null && keyword.MatchingItems.Count() > 0)
                {
                    MatchingFeatsBlock.Visible = false;
                    RightSideBreak.Visible = false;

                    GridView gridControl3 = (GridView)Page.LoadControl("~/Controls/KeywordDetailsControls.ascx").FindControl("MatchingItemsGridView");
                    gridControl3.ID = "MatchingItemsGridView";
                    gridControl3.DataSource = keyword.MatchingItems;
                    gridControl3.RowDataBound += new GridViewRowEventHandler(gridControl3_RowDataBound);
                    gridControl3.DataBind();
                    if (gridControl3.Rows.Count > 0) { gridControl3.HeaderRow.TableSection = TableRowSection.TableHeader; }
                    gridControl3.Attributes.Add("tableName", "c");
                    MatchingItemsBlock.Controls.Add(gridControl3);
                }
                else
                {
                    MatchingFeatsBlock.Controls.Add(noFeatsLabel2);
                    MatchingItemsBlock.Controls.Add(noItemsLabel);
                }
            }
            // insert search notes
            Table notesTable = (Table)Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
            notes.Controls.Add(notesTable);
        }
        protected void gridControl1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView sourceFeatsKeywordsListView = (ListView)e.Row.FindControl("SourceFeatsKeywordsListView");
                if (sourceFeatsKeywordsListView != null)
                {
                    sourceFeatsKeywordsListView.DataSource = DataBinder.Eval(e.Row.DataItem, "Keywords");
                    sourceFeatsKeywordsListView.DataBind();
                }
            }
        }
        protected void gridControl2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView matchingFeatsKeywordsListView = (ListView)e.Row.FindControl("MatchingFeatsKeywordsListView");
                if (matchingFeatsKeywordsListView != null)
                {
                    matchingFeatsKeywordsListView.DataSource = DataBinder.Eval(e.Row.DataItem, "Keywords");
                    matchingFeatsKeywordsListView.DataBind();
                }
            }
        }
        protected void gridControl3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListView matchingItemsKeywordsListView = (ListView)e.Row.FindControl("MatchingItemsKeywordsListView");
                if (matchingItemsKeywordsListView != null)
                {
                    matchingItemsKeywordsListView.DataSource = DataBinder.Eval(e.Row.DataItem, "Keywords");
                    matchingItemsKeywordsListView.DataBind();
                }
            }
        }
    }
}