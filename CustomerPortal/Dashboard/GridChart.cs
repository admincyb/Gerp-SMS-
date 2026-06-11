using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ERPData;
using System.Xml;
using System.Web.UI.HtmlControls;
using ERP.Utilities.Dashboard;
using ERP.Utilities;
using System.Drawing;
using ERPSMS_v01.UserControls;
using ERP.Dashboard;
namespace ERP.Dashboard
{
    public class GridChart : System.Web.UI.TemplateControl, IDashboard
    {
        public GridChart()
        {
            ChartElement = new GridView();
            Pager = LoadControl(Resources.PageURL.PagerUserControl) as PgerControlNew;
            InitializeComponent();
            ChartElement.AutoGenerateColumns = true;
            ChartElement.ShowHeader = true;
            ChartElement.EmptyDataRowStyle.CssClass = DashboardConstants.EmptyStyle;
            ChartElement.EmptyDataTemplate = new EmptyTemplate();
            ChartElement.ShowHeaderWhenEmpty = false;
            this.PageSize = DashboardConstants.PageSize;
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
        public PgerControlNew Pager
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
        /// <summary>
        /// Font Name of the Grid
        /// </summary>
        public string FontName
        {
            get
            {
                return ChartElement.Font.Name;
            }

            set
            {
                ChartElement.Font.Name = value;
            }
        }
        /// <summary>
        /// Font Size of the Grid
        /// </summary>
        public double FontSize
        {
            get
            {
                return ChartElement.Font.Size.Unit.Value;
            }
            set
            {
                ChartElement.Font.Size = new FontUnit(value);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// To set basic properties of Chart
        /// </summary>
        /// <param name="dsbDashletMstObj"></param>
        public void SetChart(ERPData.DsbDashletMst dsbDashletMstObj)
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
            Pager.TotalPages = (data.TotalRows == 0) ? 1 : (data.TotalRows <= this.PageSize) ? 1 : (data.TotalRows % this.PageSize) == 0 ? (data.TotalRows / this.PageSize) : (data.TotalRows / this.PageSize) + 1;
            
            BindDataSource(data);
            Pager.Visible = true;
            Pager.BindPager();
        }
        /// <summary>
        /// HTML Decode all string column of string field in a Data Table
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static GridData HtmlDecodeDataTable(GridData data)
        {
            foreach (GridItem drow in data.GridItems)
            {
                if(drow.x!=null)
                    if (drow.x.GetType() == typeof(System.String))
                        drow.x = System.Web.HttpUtility.HtmlDecode(drow.x.ToString());
                if (drow.y != null)
                    if (drow.y.GetType() == typeof(System.String))
                        drow.y = System.Web.HttpUtility.HtmlDecode(drow.y.ToString());
                if (drow.y1 != null)
                    if (drow.y1.GetType() == typeof(System.String))
                        drow.y1 = System.Web.HttpUtility.HtmlDecode(drow.y1.ToString());
                if (drow.y2 != null)
                    if (drow.y2.GetType() == typeof(System.String))
                        drow.y2 = System.Web.HttpUtility.HtmlDecode(drow.y2.ToString());
                if (drow.y3 != null)
                    if (drow.y3.GetType() == typeof(System.String))
                        drow.y3 = System.Web.HttpUtility.HtmlDecode(drow.y3.ToString());

                if (drow.y4 != null)
                    if (drow.y4.GetType() == typeof(System.String))
                        drow.y4 = System.Web.HttpUtility.HtmlDecode(drow.y4.ToString());

                if (drow.y5 != null)
                    if (drow.y5.GetType() == typeof(System.String))
                        drow.y5 = System.Web.HttpUtility.HtmlDecode(drow.y5.ToString());
                if (drow.y6 != null)
                    if (drow.y6.GetType() == typeof(System.String))
                        drow.y6 = System.Web.HttpUtility.HtmlDecode(drow.y6.ToString());
                if (drow.y7 != null)
                    if (drow.y7.GetType() == typeof(System.String))
                        drow.y7 = System.Web.HttpUtility.HtmlDecode(drow.y7.ToString());
                if (drow.y8 != null)
                    if (drow.y8.GetType() == typeof(System.String))
                        drow.y8 = System.Web.HttpUtility.HtmlDecode(drow.y8.ToString());

                if (drow.y9 != null)
                    if (drow.y9.GetType() == typeof(System.String))
                        drow.y9 = System.Web.HttpUtility.HtmlDecode(drow.y9.ToString());
                if (drow.y10 != null)
                    if (drow.y10.GetType() == typeof(System.String))
                        drow.y10 = System.Web.HttpUtility.HtmlDecode(drow.y10.ToString());

               
               
            }
           
            return data;

        }

        /// <summary>
        /// To bind Chart Data object to Chart
        /// </summary>
        /// <param name="data"></param>
        
        public void BindDataSource(GridData data)
        {
            GridViewRow hdGrid;
            data = HtmlDecodeDataTable(data);
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
        public void SetProperty(List<ERPData.DsbDshProprtyMpg> dsbDshProprtyMpgObj)
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
            tblGrid.Attributes.Add("class", DashboardConstants.CotainerStyle);

            trGrid = new HtmlTableRow();
            tdGrid = new HtmlTableCell("td");
            tdGrid.Attributes.Add("class", DashboardConstants.CotainerStyle);

            tdGrid.Controls.Add(this.ChartElement);
            trGrid.Controls.Add(tdGrid);
            tblGrid.Rows.Add(trGrid);

            trGrid = new HtmlTableRow();
            tdGrid = new HtmlTableCell("td");
            tdGrid.Attributes.Add("class", DashboardConstants.CotainerStyle);

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
            this.Pager.FirstPage += new ActionHandler(this.ActionHandler);
            this.Pager.PreviousPage += new ActionHandler(this.ActionHandler);
            this.Pager.NextPage += new ActionHandler(this.ActionHandler);
            this.Pager.LastPage += new ActionHandler(this.ActionHandler);
            this.Pager.PageChanged += new ActionHandler(this.ActionHandler);
            Pager.CurrentPage = 1;
            Pager.TotalPages = 1;
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, ERPSMS_v01.UserControls.DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        Pager.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assign the current page index.
                        if (e.CurrentPage > 1)
                            Pager.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assign the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            Pager.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            Pager.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            Pager.CurrentPage--;
                        break;

                }
                if (GetData != null)
                {
                    this.BindDataSource(GetData(this.Service, this.Method, this.PageSize, Pager.CurrentPage));
                    EnableDisableButtons(e.TotalPages);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
           // Pager.FirstButtonEnabled = (Pager.CurrentPage == 1) ? false : true;

            // Should we disable the previous link?
            Pager.PreviousButtonEnabled = (Pager.CurrentPage == 1) ? false : true;

            // Should we enable the next link?
            Pager.NextButtonEnabled = (Pager.CurrentPage < iTotalPages) ? true : false;

            // Should we enable the last link?
           // Pager.LastButtonEnabled = (Pager.CurrentPage < iTotalPages) ? true : false;
        }

        #endregion
                
    }
}