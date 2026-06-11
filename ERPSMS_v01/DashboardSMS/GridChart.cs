using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using System.Web.UI.HtmlControls;
using GTIService.Dashboard;
using gAssetsData;

namespace ERPSMS_v01.DashboardSMS
{
    public class GridChart : System.Web.UI.TemplateControl, ERPSMS_v01.DashboardSMS.IDashboard
    {
        public GridChart()
        {
            ChartElement = new GridView();
            Pager = LoadControl(Resources.Report.PagerUserControl) as UserControls.PagerControl;
            InitializeComponent();
            ChartElement.AutoGenerateColumns = true;
            ChartElement.ShowHeader = true;
            ChartElement.EmptyDataRowStyle.CssClass = "emptytable";
            ChartElement.ShowHeaderWhenEmpty = false;
            this.PageSize = 10;
            //To hide columns not required
            ChartElement.RowDataBound += new GridViewRowEventHandler(
                (obj, e) =>
                {
                    int maxIndex = e.Row.Cells.Count;
                    for (int i = this.Series; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                        //e.Row.Cells[i].ToolTip = e.Row.Cells[i].Text;
                        //e.Row.Cells[i].Text = CommonFunctions.GetShortString("", 5);
                    }
                });
        }
        /// <summary>
        /// Xml fetch delegate
        /// </summary>
        /// <param name="svc"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public delegate string GetMethod(string svc, string method, int size, int index);
        /// <summary>
        /// Xml fetch Method
        /// </summary>
        public GetMethod GetData;

        #region Public Properties

        /// <summary>
        /// The GridView Control
        /// </summary>
        public GridView ChartElement
        {
            get;
            set;
        }
        /// <summary>
        /// Chart Xml String
        /// </summary>
        public string DataXml
        {
            get;
            set;
        }
        /// <summary>
        /// Chart Data Collection
        /// </summary>
        public List<GridItem> DataItem
        {
            get;
            set;
        }
        /// <summary>
        /// Chart Titles collection
        /// </summary>
        public List<string> ChartTitles
        {
            get;
            set;
        }
        /// <summary>
        /// Main Css Class of the Chart
        /// </summary>
        public string StyleCss
        {
            set { ChartElement.CssClass = value; }
        }
        /// <summary>
        /// Chart SubType
        /// </summary>
        public string ChartSubType
        {
            get;
            set;
        }
        /// <summary>
        /// No. of series
        /// </summary>
        public int Series
        {
            get;
            set;
        }
        /// <summary>
        /// Tooltip for the Chart
        /// </summary>
        public string ToolTip
        {
            get
            {
                return ChartElement.ToolTip;
            }
            set
            {
                ChartElement.ToolTip = value;
            }
        }
        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize
        {
            get
            {
                return ChartElement.PageSize;
            }
            set
            {
                ChartElement.PageSize = value;
            }
        }
        /// <summary>
        /// Current Page
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return Pager.CurrentPage;
            }
            set
            {
                Pager.CurrentPage = value;
            }
        }
        /// <summary>
        /// Current Page
        /// </summary>
        public int TotalPages
        {
            get
            {
                return Pager.TotalPages;
            }
            set
            {
                Pager.TotalPages = value;
            }
        }
        /// <summary>
        /// Grid Pager Control
        /// </summary>
        public PagerControl Pager
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates whether for Popup
        /// </summary>
        public bool IsPopup
        {
            get;
            set;
        }
        /// <summary>
        /// Grid Size
        /// </summary>
        public GridSizeEnum GridSize
        {
            get;
            set;
        }
        /// <summary>
        /// Service Name
        /// </summary>
        public string Service
        {
            get
            {
                return Convert.ToString(ViewState["Service"]);
            }
            set
            {
                ViewState["Service"] = value;
            }
        }
        /// <summary>
        /// Service Method
        /// </summary>
        public string Method
        {
            get
            {
                return Convert.ToString(ViewState["Method"]);
            }
            set
            {
                ViewState["Method"] = value;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// To set basic properties of Chart
        /// </summary>
        /// <param name="dsbDashletMstObj"></param>
        public void SetChart(gAssetsData.DsbDashletMst dsbDashletMstObj)
        {
            this.Series = dsbDashletMstObj.DashletSeries;
            if (IsPopup)
                this.Pager.ID = "pgrPop" + dsbDashletMstObj.DashletPk.ToString();
            else
                this.Pager.ID = "pgr" + dsbDashletMstObj.DashletPk.ToString();
        }
        /// <summary>
        /// To bind Xml data to chart
        /// </summary>
        /// <param name="xml"></param>
        public void BindDataSource(string xmlData)
        {
            GridData data;
            data = null;
            data = GridData.XmlDeserialize(xmlData);
            Pager.TotalPages = (data.TotalPages == 0) ? 1 : (data.TotalPages <= this.PageSize) ? 1 : (data.TotalPages % this.PageSize) == 0 ? (data.TotalPages / this.PageSize) : (data.TotalPages / this.PageSize) + 1;

            BindDataSource(data);
            Pager.Visible = true;
            Pager.BindPager();
        }
        /// <summary>
        /// To bind Chart Data object to Chart
        /// </summary>
        /// <param name="data"></param>
        public void BindDataSource(GridData data)
        {
            GridViewRow hdGrid;
            if (data != null && data.GridItems != null && data.GridItems.Count > 0)
            {
                this.Series = this.Series == 6 || this.Series > data.SeriesCount ? data.SeriesCount : this.Series;
                this.DataItem = data.GridItems;
                this.ChartTitles = data.ChartTitles;

                ChartElement.DataSource = data.GridItems;
                ChartElement.DataBind();
                hdGrid = ChartElement.HeaderRow;
                // Create series object
                if (hdGrid != null && hdGrid.Cells != null)
                {
                    for (int i = 0; i < this.Series; i++)
                    {
                        if (i < data.ChartTitles.Count && i < hdGrid.Cells.Count)
                        {
                            hdGrid.Cells[i].Text = data.ChartTitles[i];
                        }
                    }
                }
            }
            else
            {
                this.Series = 0;
            }
        }
        /// <summary>
        /// To set a single property of Chart
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetProperty(string property, string value)
        {
            switch (property.Trim().ToLower())
            {
                case DshProperties.ToolTip:
                    this.ToolTip = value;
                    break;

                case DshProperties.StyleCss:
                    this.StyleCss = value;
                    break;
            }
        }
        /// <summary>
        /// To set properties of Chart
        /// </summary>
        /// <param name="dsbDshProprtyMpgObj"></param>
        public void SetProperty(List<gAssetsData.DsbDshProprtyMpg> dsbDshProprtyMpgObj)
        {
            foreach (DsbDshProprtyMpg prop in dsbDshProprtyMpgObj)
            {

                switch ((ChartPropertiesEnum)prop.DshPrptPropertyPk)
                {
                    case ChartPropertiesEnum.GridToolTip:
                        this.ToolTip = prop.DshPrptValue;
                        break;

                    case ChartPropertiesEnum.GridStyleName:
                        this.StyleCss = prop.DshPrptValue;
                        break;
                }
            }
        }
        /// <summary>
        /// To add chart to the specific container
        /// </summary>
        /// <param name="container"></param>
        public void PrepareChart(System.Web.UI.HtmlControls.HtmlGenericControl container)
        {
            HtmlTable tblGrid;
            HtmlTableRow trGrid;
            HtmlTableCell tdGrid;
            //HtmlGenericControl divGrid;


            //divGrid = new HtmlGenericControl("div");
            //divGrid.Attributes.Add("class", "gridwrap");
            //divGrid.Controls.Add(this.ChartElement);

            tblGrid = new HtmlTable();
            tblGrid.Attributes.Add("class", "dash-container");

            trGrid = new HtmlTableRow();
            tdGrid = new HtmlTableCell("td");
            tdGrid.Attributes.Add("class", "dash-container");

            tdGrid.Controls.Add(this.ChartElement);
            trGrid.Controls.Add(tdGrid);
            tblGrid.Rows.Add(trGrid);

            trGrid = new HtmlTableRow();
            tdGrid = new HtmlTableCell("td");
            tdGrid.Attributes.Add("class", "dash-container");

            tdGrid.Controls.Add(this.Pager);
            trGrid.Controls.Add(tdGrid);
            tblGrid.Rows.Add(trGrid);

            container.Controls.Add(tblGrid);
        }
        /// <summary>
        /// To set the height and width of the chart
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void SetDimension(int height, int width)
        {
            ChartElement.Width = new Unit(width, UnitType.Pixel);
            ChartElement.Height = new Unit(height, UnitType.Pixel);
        }

        #endregion

        #region Pager Methods + Init

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Pager.FirstPage += new FirstPageEventHandler(this.FirstPage);
            this.Pager.PreviousPage += new PreviousPageEventHandler(this.PreviousPage);
            this.Pager.NextPage += new NextPageEventHandler(this.NextPage);
            this.Pager.LastPage += new LastPageEventHandler(this.LastPage);
            this.Init += new EventHandler(this.Page_Init);
        }


        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            Pager.CurrentPage = 1;
        }


        protected void FirstPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage > 1)
            {
                Pager.CurrentPage = 1;
                if (GetData != null)
                {
                    this.BindDataSource(GetData(this.Service, this.Method, this.PageSize, Pager.CurrentPage));
                    EnableDisableButtons(e.TotalPages);
                }

            }
        }

        protected void PreviousPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage > 1)
            {
                Pager.CurrentPage--;
                if (GetData != null)
                {
                    this.BindDataSource(GetData(this.Service, this.Method, this.PageSize, Pager.CurrentPage));
                    EnableDisableButtons(e.TotalPages);
                }
            }
        }

        protected void NextPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage <= e.TotalPages)
            {
                Pager.CurrentPage++;
                if (GetData != null)
                {
                    this.BindDataSource(GetData(this.Service, this.Method, this.PageSize, Pager.CurrentPage));
                    EnableDisableButtons(e.TotalPages);
                }
            }
        }

        protected void LastPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage <= e.TotalPages)
            {
                Pager.CurrentPage = e.TotalPages;
                if (GetData != null)
                {
                    this.BindDataSource(GetData(this.Service, this.Method, this.PageSize, Pager.CurrentPage));
                    EnableDisableButtons(e.TotalPages);
                }
            }
        }

        private void EnableDisableButtons(int iTotalPages)
        {
            Pager.PreviousButtonEnabled = (Pager.CurrentPage == 1) ? false : true;
            Pager.NextButtonEnabled = (Pager.CurrentPage < iTotalPages) ? true : false;
        }

        #endregion
    }
}