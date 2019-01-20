namespace Goblinary.Web
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	public class GridViewTemplate : ITemplate
	{
		public GridViewTemplate(DataControlRowType type, string colname, string colNameBinding, string ctlType)
		{
			templateType = type;
			columnName = colname;
			columnNameBinding = colNameBinding;
			controlType = ctlType;
		}

		private DataControlRowType templateType;
		private string columnName;
		private string columnNameBinding;
		private string controlType;

		public void InstantiateIn(System.Web.UI.Control container)
		{
			switch (templateType)
			{
				case DataControlRowType.Header:
					Literal lc = new Literal();
					lc.Text = columnName;
					container.Controls.Add(lc);
					break;
				case DataControlRowType.DataRow:
					if (controlType == "Label")
					{
						Label lb = new Label();
						lb.ID = "lb1";
						lb.DataBinding += new EventHandler(this.ctl_OnDataBinding);
						container.Controls.Add(lb);
					}
					else if (controlType == "TextBox")
					{
						TextBox tb = new TextBox();
						tb.ID = "tb1";
						tb.DataBinding += new EventHandler(this.ctl_OnDataBinding);
						container.Controls.Add(tb);
					}
					else if (controlType == "CheckBox")
					{
						CheckBox cb = new CheckBox();
						cb.ID = "cb1";
						cb.DataBinding += new EventHandler(this.ctl_OnDataBinding);
						container.Controls.Add(cb);
					}
					else if (controlType == "HyperLink")
					{
						HyperLink hl = new HyperLink();
						hl.ID = "hl1";
						hl.DataBinding += new EventHandler(this.ctl_OnDataBinding);
						container.Controls.Add(hl);
					}
					break;
				default:
					break;
			}
		}

		public void ctl_OnDataBinding(object sender, EventArgs e)
		{
			if (sender.GetType().Name == "Label")
			{
				Label lb = (Label)sender;
				GridViewRow container = (GridViewRow)lb.NamingContainer;
				lb.Text = ((DataRowView)container.DataItem)[columnNameBinding].ToString();
			}
			else if (sender.GetType().Name == "TextBox")
			{
				TextBox tb = (TextBox)sender;
				GridViewRow container = (GridViewRow)tb.NamingContainer;
				tb.Text = ((DataRowView)container.DataItem)[columnNameBinding].ToString();
			}
			else if (sender.GetType().Name == "CheckBox")
			{
				CheckBox cb = (CheckBox)sender;
				GridViewRow container = (GridViewRow)cb.NamingContainer;
				cb.Checked = Convert.ToBoolean(((DataRowView)container.DataItem)[columnNameBinding].ToString());
			}
			else if (sender.GetType().Name == "HyperLink")
			{
				HyperLink hl = (HyperLink)sender;
				GridViewRow container = (GridViewRow)hl.NamingContainer;
				hl.Text = ((DataRowView)container.DataItem)[columnNameBinding].ToString();
				hl.NavigateUrl = ((DataRowView)container.DataItem)[columnNameBinding].ToString();
			}
		}
	}
}
