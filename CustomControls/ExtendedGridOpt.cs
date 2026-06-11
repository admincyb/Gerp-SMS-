using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace CustomControls
{
    public class ExtGridExpandingEventArgs : EventArgs
    {
        public int RowNumber { get; set; }
        public Dictionary<string, string> KeyValues { get; set; }
        public object CurrentRow { get; set; }
    }

    /// <summary>
    /// ExtGridViewRow extendes the standard GridView row to render the contents from the last cell as an expandible cell
    /// </summary>
    public class ExtGridViewRowOpt : GridViewRow
    {
        private TableCell _expCell;
        private HtmlInputHidden _ihExp;
        private HtmlAnchor _ctlExpand;
        private Boolean _showExpand;
        private bool _isExpanded;

        /// <summary>
        /// Gets or sets a value which specifies if the expand boutton should be displayed or not for the current row.
        /// </summary>
        public Boolean ShowExpand
        {
            get
            {
                return (bool)(ViewState[this.Parent.ClientID + this.RowIndex + "_Expanded"] ?? _showExpand);
            }
            set
            {
                ViewState[this.Parent.ClientID + this.RowIndex + "_Expanded"] = value;
                _showExpand = value;
            }
        }

        /// <summary>
        /// Constructor for ExtGridViewRow
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dataItemIndex"></param>
        /// <param name="rowType"></param>
        /// <param name="rowState"></param>
        public ExtGridViewRowOpt(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
            : base(rowIndex, dataItemIndex, rowType, rowState)
        {
        }


        /// <summary>
        /// Overrides GridViewRow.OnInit to perform custom initialization of the row.
        /// </summary>
        /// <param name="e">event args</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (RowType == DataControlRowType.Header)
            {
                _expCell = new TableHeaderCell();
            }
            else if (RowType == DataControlRowType.DataRow)
            {
                _expCell = new TableCell();

                _ctlExpand = new HtmlAnchor();
                //_ctlExpand.HRef = "#";
                _ctlExpand.Attributes["onclick"] = "TglRow(this);";

                _ihExp = new HtmlInputHidden();
                _ihExp.ID = "e" + this.DataItemIndex.ToString();

                _expCell.Controls.Add(_ctlExpand);
                _expCell.Controls.Add(_ihExp);
            }


            if (_expCell != null)
            {
                _expCell.Width = Unit.Pixel(20);

                Cells.AddAt(0, _expCell);
            }

        }


        /// <summary>
        /// Overrides GridViewRow.Render to perform custom rendering of the row.
        /// </summary>
        /// <param name="writer">the HtmlTextWrite object in which the row is rendered</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                base.Render(writer);
                return;
            }

            TableCell c = Cells[Cells.Count - 1];

            if (RowType == DataControlRowType.DataRow)
            {
                //if (_showExpand)
                //{
                ExtGridViewOpt grid = this.Parent.Parent as ExtGridViewOpt;

                if (_ihExp.Value == String.Empty)
                {
                    _ctlExpand.InnerHtml = grid.ExpandButtonText;
                    _ctlExpand.Attributes["class"] = grid.ExpandButtonCssClass;
                }
                else
                {
                    _ctlExpand.InnerHtml = grid.CollapseButtonText;
                    _ctlExpand.Attributes["class"] = grid.CollapseButtonCssClass;
                }

                c.Visible = false;
                //if (this.ShowExpand) //if the row is expanded set the attribute 
                //{
                //    this.Attributes.Add("ShowExpand", "true");
                //}
                base.Render(writer);

                c.Visible = true;
                c.ColumnSpan = GetVisibleCellsCount() - 1;
                c.BackColor = BackColor;

                if (_ihExp.Value == String.Empty)
                {
                    writer.Write("<tr style='display:none'>");
                }
                else
                {
                    writer.Write("<tr>");
                }

                c.RenderControl(writer);

                writer.Write("</tr>");


                if (RowIndex == grid.Rows.Count - 1)
                {
                    if (
                            (grid.BottomPagerRow == null && grid.FooterRow == null) ||
                            (
                                (grid.BottomPagerRow != null && grid.BottomPagerRow.Visible == false) &&
                                grid.FooterRow != null && grid.FooterRow.Visible == false
                            )
                        )
                    {
                        writer.Write("<tr><td colspan=");
                        writer.Write(c.ColumnSpan);
                        writer.Write("></td></tr>");
                    }
                }
                //}
                //else
                //{
                //    _ctlExpand.Visible = _ihExp.Visible = false;
                //    c.Visible = false;
                //    base.Render(writer);
                //}

            }
            else if (RowType == DataControlRowType.Header)
            {
                c.Visible = false;
                base.Render(writer);
            }
            else
            {
                base.Render(writer);
            }
        }

        /// <summary>
        /// Helper method which obtains the visible cells count in the current row.
        /// </summary>
        /// <returns></returns>
        private Int32 GetVisibleCellsCount()
        {
            Int32 ret = 0;

            foreach (TableCell c in Cells)
            {
                if (c.Visible) ret++;
            }

            return ret;
        }
    }



    public delegate void ExtGridExpandingEventHandler(Object sender, ExtGridExpandingEventArgs args);
    /// <summary>
    /// ExtGridView implements the expandible GridView behaviour.
    /// </summary>
    public class ExtGridViewOpt : GridView, IPostBackDataHandler
    {
        private String _expandButtonCssClass;
        private String _collapseButtonCssClass;
        private String _expandButtonText = "+";
        private String _collapseButtonText = "-";
        private string _keyFields = "";


        private int _rowNumber;






        public event ExtGridExpandingEventHandler RowExpanding;




        /// <summary>
        /// Sets or gets the CSS class which is applied on the expand button control.
        /// </summary>
        [Category("Styles")]
        public String ExpandButtonCssClass
        {
            get { return _expandButtonCssClass; }
            set { _expandButtonCssClass = value; }
        }


        /// <summary>
        /// Sets or gets the CSS class which is applied on the collapse button control.
        /// </summary>
        [Category("Styles")]
        public String CollapseButtonCssClass
        {
            get { return _collapseButtonCssClass; }
            set { _collapseButtonCssClass = value; }
        }

        /// <summary>
        /// Sets or gets the expand button's text.
        /// </summary>
        [Category("Appearance")]
        public String ExpandButtonText
        {
            get { return _expandButtonText; }
            set { _expandButtonText = value; }
        }

        /// <summary>
        /// Sets or gets the collapse button's text.
        /// </summary>
        [Category("Appearance")]
        public String CollapseButtonText
        {
            get { return _collapseButtonText; }
            set { _collapseButtonText = value; }
        }


        /// <summary>
        /// Sets or gets the expand button's text.
        /// </summary>
        [Category("Appearance")]
        public String KeyFields
        {
            get { return _keyFields; }
            set { _keyFields = value; }
        }

        public override object DataSource
        {
            get
            {
                return ViewState[this.ClientID + "DataSource"] ?? base.DataSource;
                //  base.DataSource;
            }
            set
            {
                ViewState[this.ClientID + "DataSource"] = value;
                base.DataSource = value;
            }
        }


        /// <summary>
        /// Overrides GridView.OnInit to perform custom initialization of the control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            #region register script
            String script =
@"function TglRow(ctl)
{
	var row = ctl.parentNode.parentNode;
	var tbl = row.parentNode;
	var crow = tbl.rows[row.rowIndex + 1];
	var ihExp = ctl.parentNode.getElementsByTagName('input').item(0);

	tbl = tbl.parentNode;

	var expandClass = tbl.attributes.getNamedItem('expandClass').value;
	var collapseClass = tbl.attributes.getNamedItem('collapseClass').value;
	var expandText = tbl.attributes.getNamedItem('expandText').value;
	var collapseText = tbl.attributes.getNamedItem('collapseText').value;
	
	if (crow.style.display == 'none')
	{
		crow.style.display = '';
		ctl.innerHTML = collapseText;
		ctl.className = collapseClass;
		ihExp.value = '1';

     if (typeof AfterGridExpand == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterGridExpand(row);
                }
	}
	else
	{
		crow.style.display = 'none';
		ctl.innerHTML = expandText;
		ctl.className = expandClass;
		ihExp.value = '';
	}
}

";

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "ExtGridOpt", script, true);


            #endregion

        }

        /// <summary>
        /// Overrides GridView.CreateRow to create the custom rows in the grid (ExtGridViewRow).
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dataSourceIndex"></param>
        /// <param name="rowType"></param>
        /// <param name="rowState"></param>
        /// <returns></returns>
        protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            return new ExtGridViewRowOpt(rowIndex, dataSourceIndex, rowType, rowState);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.RegisterRequiresPostBack(this);
            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.Attributes["expandClass"] = _expandButtonCssClass;
            this.Attributes["collapseClass"] = _collapseButtonCssClass;
            this.Attributes["expandText"] = _expandButtonText;
            this.Attributes["collapseText"] = _collapseButtonText;

            base.Render(writer);
        }


        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            string _target = postCollection["__EVENTTARGET"];

            string _arg = postCollection["__EVENTARGUMENT"];

            if (_target == this.ClientID)
            {
                _rowNumber = string.IsNullOrEmpty(_arg) ? -1 : Convert.ToInt32(_arg);
                Page.RegisterRequiresRaiseEvent(this);

                return (true);

            }

            else
            {

                return (false);

            }


        }

        protected virtual void OnRowExpanding(ExtGridExpandingEventArgs e)
        {
            if (RowExpanding != null)
            {
                RowExpanding(this, e);
            }
        }

        public void RaisePostDataChangedEvent()
        {
            ExtGridExpandingEventArgs arg = new ExtGridExpandingEventArgs();
            arg.RowNumber = _rowNumber;
            OnRowExpanding(arg);

        }
    }
}
