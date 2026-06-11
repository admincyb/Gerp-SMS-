using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.UserControls
{
    //public deletgates
    public delegate void ActionHandler(object sender, DataNavigatorEventArgs e);

    public partial class PgerControlNew : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillPager();

            }
        }
        public void BindPager()
        {
            if (this.TotalPages > 1)
            {
                this.NextButtonEnabled = true;
                this.LastButtonEnabled = true;
                this.FillPager();
            }
            else
            {
                this.Visible = false;
                this.TotalPages = 1;
            }

        }
        public void FillPager()
        {
            // Put user code to initialize the page here
            int iPages = this.TotalPages;

            ddPage.Items.Clear();

            if (iPages > 0)
            {
                for (int i = 1; i <= iPages; i++)
                    ddPage.Items.Add(i.ToString());
            }

            SetDropDownPageNumber(this.CurrentPage);
        }

        //public events
        public event ActionHandler FirstPage;
        public event ActionHandler LastPage;
        public event ActionHandler PreviousPage;
        public event ActionHandler NextPage;
        public event ActionHandler PageChanged;

        protected void OnPageChangedButton(object sender, EventArgs e)
        {
            DataNavigatorEventArgs args = new DataNavigatorEventArgs();
            args.CurrentPage = int.Parse(ddPage.SelectedItem.Text);
            args.TotalPages = int.Parse(lblTotalPages.Text);
            args.Action = NavigationEnum.PAGECHANGE;

            OnPageChanged(args);
            SetDropDownPageNumber(args.CurrentPage);
        }

        protected virtual void OnPageChanged(DataNavigatorEventArgs args)
        {
            if (PageChanged != null)
            {
                // Invoke the delegates.
                PageChanged(this, args);
            }
        }

        protected void OnPreviousPageButton(object sender, ImageClickEventArgs e)
        {
            DataNavigatorEventArgs args = new DataNavigatorEventArgs();
            args.CurrentPage = int.Parse(lblCurrentPage.Text);
            args.TotalPages = int.Parse(lblTotalPages.Text);
            args.Action = NavigationEnum.PREVIOUS;
            OnPreviousPage(args);
            SetDropDownPageNumber(args.CurrentPage - 1);
        }

        protected virtual void OnPreviousPage(DataNavigatorEventArgs args)
        {
            if (PreviousPage != null)
            {
                // Invoke the delegates.
                PreviousPage(this, args);
            }
        }

        protected void OnNextPageButton(object sender, ImageClickEventArgs e)
        {
            DataNavigatorEventArgs args = new DataNavigatorEventArgs();
            args.CurrentPage = int.Parse(lblCurrentPage.Text);
            args.TotalPages = int.Parse(lblTotalPages.Text);
            args.Action = NavigationEnum.NEXT;
            OnNextPage(args);
            SetDropDownPageNumber(args.CurrentPage + 1);
        }

        protected virtual void OnNextPage(DataNavigatorEventArgs args)
        {
            if (NextPage != null)
            {
                // Invoke the delegates.
                NextPage(this, args);
            }
        }

        protected void OnFirstPageButton(object sender, ImageClickEventArgs e)
        {
            DataNavigatorEventArgs args = new DataNavigatorEventArgs();
            args.CurrentPage = int.Parse(lblCurrentPage.Text);
            args.TotalPages = int.Parse(lblTotalPages.Text);
            args.Action = NavigationEnum.FIRST;
            OnFirstPage(args);
            SetDropDownPageNumber(1);
        }

        protected virtual void OnFirstPage(DataNavigatorEventArgs args)
        {
            if (FirstPage != null)
            {
                // Invoke the delegates.
                FirstPage(this, args);
            }
        }

        protected void OnLastPageButton(object sender, ImageClickEventArgs e)
        {
            DataNavigatorEventArgs args = new DataNavigatorEventArgs();
            args.CurrentPage = int.Parse(lblCurrentPage.Text);
            args.TotalPages = int.Parse(lblTotalPages.Text);
            args.Action = NavigationEnum.LAST;
            OnLastPage(args);
            SetDropDownPageNumber(args.TotalPages);
        }

        protected virtual void OnLastPage(DataNavigatorEventArgs args)
        {
            if (LastPage != null)
            {
                // Invoke the delegates.
                LastPage(this, args);
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrevious.Click += new System.Web.UI.ImageClickEventHandler(this.OnPreviousPageButton);
            this.btnNext.Click += new System.Web.UI.ImageClickEventHandler(this.OnNextPageButton);
            this.btnFirst.Click += new System.Web.UI.ImageClickEventHandler(this.OnFirstPageButton);
            this.btnLast.Click += new System.Web.UI.ImageClickEventHandler(this.OnLastPageButton);
            this.ddPage.SelectedIndexChanged += new EventHandler(this.OnPageChangedButton);

        }
        #endregion

        #region Get/Set Properties
        public int CurrentPage
        {
            get { return int.Parse(lblCurrentPage.Text); }
            set { lblCurrentPage.Text = Convert.ToString(value); }
        }

        public int TotalPages
        {
            get { return Convert.ToInt32((lblTotalPages.Text.Equals(String.Empty) ? "0" : lblTotalPages.Text)); }
            set { lblTotalPages.Text = Convert.ToString(value); }
        }

        public bool NextButtonEnabled
        {
            get { return btnNext.Enabled; }
            set { btnNext.Enabled = value; }
        }

        public string NextButtonImageUrl
        {
            get { return btnNext.ImageUrl; }
            set { btnNext.ImageUrl = value; }
        }

        public bool PreviousButtonEnabled
        {
            get { return btnPrevious.Enabled; }
            set { btnPrevious.Enabled = value; }
        }

        public string PreviousButtonImageUrl
        {
            get { return btnPrevious.ImageUrl; }
            set { btnPrevious.ImageUrl = value; }
        }

        public bool FirstButtonEnabled
        {
            get { return btnFirst.Enabled; }
            set { btnFirst.Enabled = value; }
        }

        public string FirstButtonImageUrl
        {
            get { return btnFirst.ImageUrl; }
            set { btnFirst.ImageUrl = value; }
        }

        public bool LastButtonEnabled
        {
            get { return btnLast.Enabled; }
            set { btnLast.Enabled = value; }
        }

        public string LastButtonImageUrl
        {
            get { return btnLast.ImageUrl; }
            set { btnLast.ImageUrl = value; }
        }
        #endregion

        public void SetDropDownPageNumber(int iCurrentPage)
        {
            if (iCurrentPage > this.TotalPages)
            {
                this.CurrentPage = this.TotalPages;
                iCurrentPage = this.TotalPages;
            }
            else if (iCurrentPage <= 0)
            {
                this.CurrentPage = 1;
                iCurrentPage = 1;
            }
            if (ddPage.Items.Count > 0)
                // since SelectedIndex is 0-based, we have to
                // take the current page number and minus 1
                ddPage.SelectedIndex = iCurrentPage - 1;
        }
    }
  

    public enum NavigationEnum
    {
        PAGECHANGE,
        FIRST,
        LAST,
        NEXT,
        PREVIOUS

    }
}