using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ERP.Utilities.Dashboard;
using ERPData;
using System.Web;

namespace ERP.Dashboard
{
    /// <summary>
    /// .Net Chart Class
    /// </summary>
    public class MSChart : IDashboard
    {
        public MSChart()
        {
            ChartElement = new Chart();
            ChartElement.ChartAreas.Add(new ChartArea(ChartAreaName));            
            ChartElement.Legends.Add(new Legend(LegendName));
            ChartElement.Legends[LegendName].Docking = Docking.Top;
        }

        #region Public Properties
        /// <summary>
        /// The Chart Control
        /// </summary>
        public Chart ChartElement
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
        public List<ChartItem> DataItem
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
        /// Chart SubType
        /// </summary>
        public ChartsEnum ChartType
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
        /// Whether single series or multiple
        /// </summary>
        public bool HasMultipleSeries
        {
            get
            {
                return Series > 0;
            }
        }
        /// <summary>
        /// Backgroud color of the Chart
        /// </summary>
        public string BackColor
        {
            set
            {
                ChartElement.ChartAreas[ChartAreaName].BackColor = ColorTranslator.FromHtml(value);
            }
        }
        /// <summary>
        /// Main Css Class of the Chart
        /// </summary>
        public string StyleCss
        {
            set { ChartElement.CssClass = value; }
        }
        /// <summary>
        /// Is Chart 3D
        /// </summary>
        public bool Enable3D
        {
            set { ChartElement.ChartAreas[ChartAreaName].Area3DStyle.Enable3D = value; }
        }
        /// <summary>
        /// To show Legends
        /// </summary>
        public bool EnableLegend
        {
            set { ChartElement.Legends[LegendName].Enabled = value; }
        }
        /// <summary>
        /// To show end point values of the X-Scale
        /// </summary>
        public bool IsXEndLabelVisible
        {
            set { ChartElement.ChartAreas[ChartAreaName].AxisX.LabelStyle.IsEndLabelVisible = value; }
        }
        /// <summary>
        /// To show scale values as Labels
        /// </summary>
        public bool IsValueShownAsLabel
        {
            get;
            set;
        }
        /// <summary>
        /// To enable X-axis margin
        /// </summary>
        public bool IsXMarginVisible
        {
            set
            {
                ChartElement.ChartAreas[ChartAreaName].AxisX.IsMarginVisible = value;
            }
        }
        /// <summary>
        /// To show X-axis marker lines
        /// </summary>
        public bool ShowMarkerLines
        {
            get;
            set;
        }
        /// <summary>
        /// Position of Legend(top, bottom, left, right)
        /// </summary>
        public string LegendDocking
        {
            set
            {
                Docking lgtDocking;
                if (Enum.TryParse(value, true, out lgtDocking))
                {
                    ChartElement.Legends[LegendName].Docking = lgtDocking;
                }
            }
        }
        /// <summary>
        /// Alignment of Legend
        /// </summary>
        public string LegendAlignment
        {
            set
            {
                StringAlignment lgtAlignment;
                if (Enum.TryParse(value, true, out lgtAlignment))
                {
                    ChartElement.Legends[LegendName].Alignment = lgtAlignment;
                }
            }
        }
        /// <summary>
        /// The Style effect for the Chart
        /// For Bar, Column,Pie and Doughnut
        /// </summary>
        public string DrawingStyle
        {
            get;
            set;
        }
        /// <summary>
        /// Width of the Bar
        /// For Bar and Column
        /// </summary>
        public double PointWidth
        {
            get;
            set;
        }
        /// <summary>
        /// Position of the Label
        /// For Bar, Column and Pie
        /// </summary>
        public string LabelStyle
        {
            get;
            set;
        }
        /// <summary>
        /// To Hide X Data Label
        /// </summary>
        public bool HideXLabel
        {
            set
            {
                ChartElement.ChartAreas[ChartAreaName].AxisX.LabelStyle.Enabled = !value;
            }
        }
        /// <summary>
        /// Pie Chart IsExplode
        /// </summary>
        public bool IsExplode
        {
            get;
            set;
        }

        #endregion

        private const string ChartAreaName = "MSChartArea";
        private const string LegendName = "MSChartLegend";

        #region Public Methods
        /// <summary>
        /// To set basic properties of Chart
        /// </summary>
        /// <param name="dsbDashletMstObj"></param>
        public void SetChart(DsbDashletMst dsbDashletMstObj)
        {
            if (dsbDashletMstObj != null)
            {
                this.Series = dsbDashletMstObj.DashletSeries;
                ChartType = (ChartsEnum)dsbDashletMstObj.DsbChtSubTypeCfg.cstChartType;
            }
        }
        /// <summary>
        /// To bind Xml data to chart
        /// </summary>
        /// <param name="xmlData"></param>
        public void BindDataSource(string xmlData)
        {
            ChartData data;
            data = null;
            data = ChartData.XmlDeserialize(xmlData);
            BindDataSource(data);
        }
        /// <summary>
        /// HTML Decode all string column of string field in a Data Table
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static ChartData HtmlDecodeDataTable(ChartData data)
        {
            foreach (ChartItem drow in data.ChartItems)
            {
                if (drow.x != null)
                    if (drow.x.GetType() == typeof(System.String))
                        drow.x = System.Web.HttpUtility.HtmlDecode(drow.x.ToString());
               

            }

            return data;

        }
        /// <summary>
        /// To bind Chart Data object to Chart
        /// </summary>
        /// <param name="data"></param>
        public void BindDataSource(ChartData data)
        {
            Series srs;
            string srsName;

            data = HtmlDecodeDataTable(data);
            //AxisAnnotation annotationX;
            if (data != null && data.ChartItems != null && data.ChartItems.Count > 0)
            {
                this.Series = this.Series == 6 || this.Series > data.SeriesCount ? data.SeriesCount : this.Series;
                this.DataItem = data.ChartItems;
                this.ChartTitles = data.ChartTitles;

                if (ChartType == ChartsEnum.AREA)
                {
                    ChartElement.ChartAreas[ChartAreaName].AxisX.Title = string.IsNullOrEmpty(data.XTitle) ? Resources.ErpRes.XTitleDefault : data.XTitle;
                    ChartElement.ChartAreas[ChartAreaName].AxisY.Title = "Faults";
                    ChartElement.ChartAreas[ChartAreaName].AxisX.IsMarginVisible = false;
                    ChartElement.ChartAreas[ChartAreaName].AxisX.MajorGrid.Enabled = false;
                    ChartElement.ChartAreas[ChartAreaName].AxisY.LineColor = Color.White;
                    ChartElement.ChartAreas[ChartAreaName].AxisX.LineColor = Color.Gray;
                    ChartElement.ChartAreas[ChartAreaName].ShadowColor = Color.Transparent;
                    ChartElement.ChartAreas[ChartAreaName].BackColor = Color.White;
                    ChartElement.BackColor = Color.White;
                    ChartElement.BackSecondaryColor = Color.White;
                }
                else
                {
                    ChartElement.ChartAreas[ChartAreaName].AxisX.Title = string.IsNullOrEmpty(data.XTitle) ? Resources.ErpRes.XTitleDefault : data.XTitle;
                    ChartElement.ChartAreas[ChartAreaName].AxisY.Title = string.IsNullOrEmpty(data.YTitle) ? Resources.ErpRes.YTitleDefault : data.YTitle;
                }

                // Create series object
                for (int i = 0; i < this.Series; i++)
                {
                    srsName = "S" + i.ToString();
                    srs = new Series(srsName);
                    if (data.ChartTitles != null && data.ChartTitles.Count > i && ChartType != ChartsEnum.PIE)
                    {
                        if (ChartType == ChartsEnum.AREA)
                        {
                            srs.LegendText = data.ChartTitles[i + 1];                          
                        }
                        else
                            srs.LegendText = data.ChartTitles[i];
                    }
                    SetSeriesType(srs, i);
                    if (ChartType == ChartsEnum.PIE)
                    {
                        srs.Points.DataBindY(this.DataItem, "y" + (i == 0 ? string.Empty : i.ToString()));
                        for (int j = 0; j < this.DataItem.Count; j++)
                        {
                            if (srs.Points.Count > j)
                            {
                                srs.Points[j].LegendText = this.DataItem[j].x;
                                if (string.IsNullOrEmpty(this.DataItem[j].Description))
                                {
                                    srs.Points[j].ToolTip = this.DataItem[j].x + "," + this.DataItem[j].y;
                                }
                                else 
                                {
                                    srs.Points[j].ToolTip = this.DataItem[j].Description + "," + this.DataItem[j].y;
                                }
                               
                                if(this.IsExplode)
                                    srs.Points[j]["Exploded"] = "True";
                            }
                        } 
                        srs.Label = "#VALY";                        
                    }
                    //for Area chart
                    else if (ChartType == ChartsEnum.AREA)
                    {
                        if (i == 0)
                        {
                            srs.Points.DataBindY(this.DataItem, "y" + (i == 0 ? string.Empty : i.ToString()));
                            for (int j = 0; j < this.DataItem.Count; j++)
                            {
                                if (srs.Points.Count > j)
                                {
                                    srs.Points[j].LegendText = this.DataItem[j].x;
                                    srs.Points[j].AxisLabel = Convert.ToDateTime(this.DataItem[j].x).ToString("MMM-dd"); 
                                    if (string.IsNullOrEmpty(this.DataItem[j].Description))
                                    {
                                        srs.Points[j].ToolTip = " Date :" + Convert.ToDateTime(this.DataItem[j].x).ToString("dd-MMM-yyyy") + " - New Faults :" + this.DataItem[j].y + " - Closed Faults :" + this.DataItem[j].y1;
                                    }
                                    else
                                    {
                                        srs.Points[j].ToolTip = " Date :" + Convert.ToDateTime(this.DataItem[j].x).ToString("dd-MMM-yyyy") + " - New Faults :" + this.DataItem[j].y + " - Closed Faults :" + this.DataItem[j].y1;
                                    }
                                }
                            }

                            srs.Color = System.Drawing.Color.FromArgb(127, Color.FromArgb(219, 247, 255));
                            srs.BorderColor = System.Drawing.Color.FromArgb(128, 218, 255);
                        }
                        else
                        {
                            srs.Points.DataBindY(this.DataItem, "y" + (i == 0 ? string.Empty : i.ToString()));
                            for (int j = 0; j < this.DataItem.Count; j++)
                            {
                                if (srs.Points.Count > j)
                                {
                                    srs.Points[j].LegendText = this.DataItem[j].x;
                                    srs.Points[j].AxisLabel = Convert.ToDateTime(this.DataItem[j].x).ToString("MMM-dd");
                                    if (string.IsNullOrEmpty(this.DataItem[j].Description))
                                    {
                                        srs.Points[j].ToolTip = " Date :" + Convert.ToDateTime(this.DataItem[j].x).ToString("dd-MMM-yyyy") + " - New Faults :" + this.DataItem[j].y + " - Closed Faults :" + this.DataItem[j].y1;
                                    }
                                    else
                                    {
                                        srs.Points[j].ToolTip = " Date :" + Convert.ToDateTime(this.DataItem[j].x).ToString("dd-MMM-yyyy") + " - New Faults :" + this.DataItem[j].y + " - Closed Faults :" + this.DataItem[j].y1;
                                    }
                                }
                            }

                            srs.Color = System.Drawing.Color.FromArgb(127, Color.FromArgb(231, 246,203));
                            srs.BorderColor = System.Drawing.Color.FromArgb(208, 243, 153);
                        }
                    }
                    else
                    {
                        srs.Points.DataBind(this.DataItem, "x", "y" + (i == 0 ? string.Empty : i.ToString()), null);
                        srs.ToolTip = "#VALX,#VALY";
                    }
                   
                    ChartElement.Series.Add(srs);
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
            bool isValue;
            double point;
            switch (property.Trim().ToLower())
            {
                case MSChartProperties.BackColor:
                    this.BackColor = value;
                    break;
                case MSChartProperties.StyleCss:
                    this.StyleCss = value;
                    break;
                case MSChartProperties.Enable3D:
                    this.Enable3D = Convert.ToBoolean(value);
                    break;
                case MSChartProperties.EnableLegend:
                    if (bool.TryParse(value, out isValue))
                        this.EnableLegend = isValue;
                    break;
                case MSChartProperties.IsXEndLabelVisible:
                    if (bool.TryParse(value, out isValue))
                        this.IsXEndLabelVisible = isValue;
                    break;
                case MSChartProperties.IsValueShownAsLabel:
                    if (bool.TryParse(value, out isValue))
                        this.IsValueShownAsLabel = isValue;
                    break;
                case MSChartProperties.IsXMarginVisible:
                    if (bool.TryParse(value, out isValue))
                        this.IsXMarginVisible = isValue;
                    break;
                case MSChartProperties.ShowMarkerLines:
                    if (bool.TryParse(value, out isValue))
                        this.ShowMarkerLines = isValue;
                    break;
                case MSChartProperties.LegendDocking:
                    this.LegendDocking = value;
                    break;
                case MSChartProperties.LegendAlignment:
                    this.LegendAlignment = value;
                    break;
                case MSChartProperties.DrawingStyle:
                    this.DrawingStyle = value;
                    break;
                case MSChartProperties.PointWidth:
                    if (double.TryParse(value, out point))
                        this.PointWidth = point;
                    break;
                case MSChartProperties.LabelStyle:
                    this.LabelStyle = value;
                    break;
            }
        }
        /// <summary>
        /// To set properties of Chart
        /// </summary>
        /// <param name="dsbDshProprtyMpgObj"></param>
        public void SetProperty(List<DsbDshProprtyMpg> dsbDshProprtyMpgObj)
        {
            foreach (DsbDshProprtyMpg prop in dsbDshProprtyMpgObj)
            {
                bool isValue;
                double point;
                if (prop.DsbDashletPropertyCfg != null)
                {
                    switch (prop.DsbDashletPropertyCfg.PropertyName.Trim().ToLower())
                    {
                        case MSChartProperties.BackColor:
                            this.BackColor = prop.DshPrptValue;
                            break;
                        case MSChartProperties.StyleCss:
                            this.StyleCss = prop.DshPrptValue;
                            break;
                        case MSChartProperties.Enable3D:
                            this.Enable3D = Convert.ToBoolean(prop.DshPrptValue);
                            break;
                        case MSChartProperties.EnableLegend:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.EnableLegend = isValue;
                            break;
                        case MSChartProperties.IsXEndLabelVisible:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.IsXEndLabelVisible = isValue;
                            break;
                        case MSChartProperties.IsValueShownAsLabel:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.IsValueShownAsLabel = isValue;
                            break;
                        case MSChartProperties.IsXMarginVisible:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.IsXMarginVisible = isValue;
                            break;
                        case MSChartProperties.ShowMarkerLines:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.ShowMarkerLines = isValue;
                            break;
                        case MSChartProperties.LegendDocking:
                            this.LegendDocking = prop.DshPrptValue;
                            break;
                        case MSChartProperties.LegendAlignment:
                            this.LegendAlignment = prop.DshPrptValue;
                            break;
                        case MSChartProperties.DrawingStyle:
                            this.DrawingStyle = prop.DshPrptValue;
                            break;
                        case MSChartProperties.PointWidth:
                            if (double.TryParse(prop.DshPrptValue, out point))
                                this.PointWidth = point;
                            break;
                        case MSChartProperties.LabelStyle:
                            this.LabelStyle = prop.DshPrptValue;
                            break;
                        case MSChartProperties.IsExplode:
                            if (bool.TryParse(prop.DshPrptValue, out isValue))
                                this.IsExplode = isValue;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// To add chart to the specific container
        /// </summary>
        /// <param name="container"></param>
        public void PrepareChart(HtmlGenericControl container)
        {
            HtmlGenericControl divEmptyChart;
            HtmlGenericControl bold;
            Label lbl;
            HtmlTextWriterStyle stl;
            stl = HtmlTextWriterStyle.TextAlign;
            container.Style.Add(stl, "center");
            if (this.Series > 0)
                container.Controls.Add(this.ChartElement);
            else
            {
                divEmptyChart = new HtmlGenericControl("div");
                divEmptyChart.Style.Add(System.Web.UI.HtmlTextWriterStyle.Height, this.ChartElement.Height.ToString());
                divEmptyChart.Style.Add(System.Web.UI.HtmlTextWriterStyle.Width, this.ChartElement.Width.ToString());
                bold = new HtmlGenericControl("b");
                lbl = new Label();
                lbl.Text = Resources.ErpRes.Msg_EmptyGrid;
                lbl.Style.Add(System.Web.UI.HtmlTextWriterStyle.FontWeight, "bold");
                bold.Controls.Add(lbl);
                divEmptyChart.Controls.Add(bold);
                container.Controls.Add(divEmptyChart);
            }
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

        #region Private Methods
        /// <summary>
        /// Set Series ChartType
        /// </summary>
        /// <param name="srs"></param>
        /// <param name="index"></param>
        private void SetSeriesType(Series srs, int index)
        {
            BarColDrawingEnum bcDrawingStyle;
            PieDrawingEnum pieDrawingStyle;
            BarColLabelEnum bcLabelStyle;
            PieLabelEnum pieLabelStyle;
            AreaDrawingEnum areaDrawingStyle;
            //Set Common Properties
            srs.IsValueShownAsLabel = this.IsValueShownAsLabel;
            srs["ShowMarkerLines"] = this.ShowMarkerLines.ToString();

            switch (ChartType)
            {
                case ChartsEnum.BAR:
                    srs.ChartType = SeriesChartType.Column;

                    if (!Enum.TryParse(DrawingStyle, true, out bcDrawingStyle))
                        bcDrawingStyle = BarColDrawingEnum.Cylinder;
                    srs["DrawingStyle"] = bcDrawingStyle.ToString();

                    if (this.PointWidth > 0)
                        srs["PointWidth"] = this.PointWidth.ToString();

                    if (!Enum.TryParse(DrawingStyle, true, out bcLabelStyle))
                        bcLabelStyle = BarColLabelEnum.Top;
                    srs["BarLabelStyle"] = bcLabelStyle.ToString();
                    break;
                case ChartsEnum.COMPOSITE:
                    if (index == 0)
                    {
                        srs.ChartType = SeriesChartType.Column;

                        if (!Enum.TryParse(DrawingStyle, true, out bcDrawingStyle))
                            bcDrawingStyle = BarColDrawingEnum.Cylinder;
                        srs["DrawingStyle"] = bcDrawingStyle.ToString();

                        if (this.PointWidth > 0)
                            srs["PointWidth"] = this.PointWidth.ToString();

                        if (!Enum.TryParse(DrawingStyle, true, out bcLabelStyle))
                            bcLabelStyle = BarColLabelEnum.Top;
                        srs["BarLabelStyle"] = bcLabelStyle.ToString();
                    }
                    else
                        srs.ChartType = SeriesChartType.Line;
                    break;
                case ChartsEnum.LINE:
                    srs.ChartType = SeriesChartType.Line;
                    break;
                case ChartsEnum.MERGED:
                    srs.ChartType = SeriesChartType.Column;

                    if (!Enum.TryParse(DrawingStyle, true, out bcDrawingStyle))
                        bcDrawingStyle = BarColDrawingEnum.Cylinder;
                    srs["DrawingStyle"] = bcDrawingStyle.ToString();

                    if (this.PointWidth > 0)
                        srs["PointWidth"] = this.PointWidth.ToString();

                    if (!Enum.TryParse(DrawingStyle, true, out bcLabelStyle))
                        bcLabelStyle = BarColLabelEnum.Top;
                    srs["BarLabelStyle"] = bcLabelStyle.ToString();
                    break;
                case ChartsEnum.MULTILINE:
                    srs.ChartType = SeriesChartType.Line;
                    break;
                case ChartsEnum.PIE:
                    srs.ChartType = SeriesChartType.Pie;

                    if (!Enum.TryParse(DrawingStyle, true, out pieDrawingStyle))
                        pieDrawingStyle = PieDrawingEnum.SoftEdge;
                    srs["DrawingStyle"] = pieDrawingStyle.ToString();

                    if (!Enum.TryParse(DrawingStyle, true, out pieLabelStyle))
                        pieLabelStyle = PieLabelEnum.Inside;
                    srs["PieLabelStyle"] = pieLabelStyle.ToString();
                    break;
                case ChartsEnum.STACKED:
                    srs.ChartType = SeriesChartType.StackedColumn;

                    if (!Enum.TryParse(DrawingStyle, true, out bcDrawingStyle))
                        bcDrawingStyle = BarColDrawingEnum.Cylinder;
                    srs["DrawingStyle"] = bcDrawingStyle.ToString();

                    if (this.PointWidth > 0)
                        srs["PointWidth"] = this.PointWidth.ToString();

                    if (!Enum.TryParse(DrawingStyle, true, out bcLabelStyle))
                        bcLabelStyle = BarColLabelEnum.Top;
                    srs["BarLabelStyle"] = bcLabelStyle.ToString();
                    break;
                case ChartsEnum.AREA:
                   srs.ChartType = SeriesChartType.Area; 
                    break;
                default:
                    srs.ChartType = SeriesChartType.Line;
                    break;
            }
        }

        #endregion
    }
    /// <summary>
    /// Bar/Column DrawingStyle Enum
    /// </summary>
    public enum BarColDrawingEnum
    {
        Default,
        Emboss,
        Cylinder,
        Wedge,
        LightToDark
    }
    /// <summary>
    /// Pie DrawingStyle Enum
    /// </summary>
    public enum PieDrawingEnum
    {
        Default,
        SoftEdge,
        Concave
    }
    /// <summary>
    /// Bar/Column LabelStyle Enum
    /// </summary>
    public enum BarColLabelEnum
    {
        Top,
        Bottom,
        Center,
        Left,
        Right
    }

    /// <summary>
    /// Area DrawingStyle Enum
    /// </summary>
    public enum AreaDrawingEnum
    {
        Default,
        MarkerStyle,
        MarkerColor
    }
    /// <summary>
    /// Pie LabelStyle Enum
    /// </summary>
    public enum PieLabelEnum
    {
        Inside,
        Outside,
        Disabled
    }

    public class EmptyTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            Label lbl = new Label();
            lbl.Text = Resources.ErpRes.Msg_EmptyGrid;
            container.Controls.Add(lbl);
        }
    }
}