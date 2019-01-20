namespace Goblinary.Website.CustomNode
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;


    /// <summary>
    /// CustomTreeNode Extends TreeNode class to add the functionality of applying CSS 
    /// and adding Attributes to a node
    /// </summary>
    public class CustomTreeNode : TreeNode
    {
        public CustomTreeNode() : base() { }
        #region Private Declarations
        /// <summary>
        /// used to store Node Attributes
        /// </summary>
        private NameValueCollection _Attributes = new NameValueCollection();

        /// <summary>
        ///used to store the CSS Class applied to a node. 
        /// </summary>
        private string _cssClass;
        #endregion

        #region Public Properties
        /// <summary>
        /// Property used to set the CSS Class applied to a Node
        /// </summary>
        public string cssClass
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }

        }
        /// <summary>
        /// Property used to add Attributes to a node
        /// </summary>
        public NameValueCollection Attributes
        {
            get
            {
                return this._Attributes;
            }
            set
            {
                this._Attributes = value;
            }
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// add additional rendering to the node
        /// Add a div Tag and a class Attribute
        /// </summary>
        /// <param name="writer">represents the output stream used to write content to a Web page</param>
        protected override void RenderPreText(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.RenderPreText(writer);
        }
        /// <summary>
        /// add additional rendering to the node
        /// End Tag
        /// </summary>
        /// <param name="writer">represents the output stream used to write content to a Web page</param>
        protected override void RenderPostText(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            base.RenderPostText(writer);
        }
        #endregion


    }
}