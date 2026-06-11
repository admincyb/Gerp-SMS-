using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using ERPData;

namespace ERP.Dashboard
{
    #region Dashboard Interface
    /// <summary>
    /// Dashboard Interface( To be implemented to create a chart)
    /// </summary>
    public interface IDashboard
    {
        #region Properties

        //string DataXml { get; set; }
        //List<ChartItem> DataItem { get; set; }
        //List<GaugeItem> GaugeDataItem { get; set; }
        //List<string> ChartTitles { get; set; }
        //List<string> LegendTexts { get; set; }
        //string Title { get; set; }
        //string StyleCss { set; }
        //string ChartSubType { get; set; }
        //int Series { get; set; }
        //bool HasMultipleSeries { get; }
        //string DataPointColor { get; set; }
        //string ToolTip { get; set; }
        //double Transparency { get; set; }
        //string ThemeName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// To set basic properties of Chart
        /// </summary>
        /// <param name="dsbDashletMstObj"></param>
        void SetChart(DsbDashletMst dsbDashletMstObj);
        /// <summary>
        /// To bind Xml data to chart
        /// </summary>
        /// <param name="xmlData"></param>
        void BindDataSource(string xmlData);
        ///// <summary>
        ///// To bind Chart Data object to Chart
        ///// </summary>
        ///// <param name="data"></param>
        //void BindDataSource(ChartData data);
        /// <summary>
        /// To set a single property of Chart
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        void SetProperty(string property, string value);
        /// <summary>
        /// To set properties of Chart
        /// </summary>
        /// <param name="dsbDshProprtyMpgObj"></param>
        void SetProperty(List<DsbDshProprtyMpg> dsbDshProprtyMpgObj);
        /// <summary>
        /// To add chart to the specific container
        /// </summary>
        /// <param name="container"></param>
        void PrepareChart(HtmlGenericControl container);
        /// <summary>
        /// To set the height and width of the chart
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        void SetDimension(int height, int width);

        #endregion
    }

    #endregion
}
