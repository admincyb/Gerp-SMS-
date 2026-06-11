using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;

namespace  ERP.Utilities.Dashboard
{
    #region Chart Data

    /// <summary>
    /// Chart Item Class
    /// </summary>
    public class ChartItem
    {
        public string x { get; set; }
        public double y { get; set; }
        public double y1 { get; set; }
        public double y2 { get; set; }
        public double y3 { get; set; }
        public double y4 { get; set; }
        public double y5 { get; set; }
        public double y6 { get; set; }
        public double y7 { get; set; }
        public double y8 { get; set; }
        public double y9 { get; set; }
        public double y10 { get; set; }
        string _Description;
        public string Description { 
            get
            {
                if(string.IsNullOrEmpty(_Description)) 
                    return string.Empty;
                return HttpUtility.HtmlDecode(_Description);
            }
            set
            {
                _Description=value;
            }
        }
    }
    /// <summary>
    /// Guage Item Class
    /// </summary>
    public class GaugeItem
    {
        public double min { get; set; }
        public double max { get; set; }
        public double value { get; set; }
    }

    /// <summary>
    /// Grid Item Class
    /// </summary>
    public class GridItem
    {
        public string x { get; set; }
        public string y { get; set; }
        public string y1 { get; set; }
        public string y2 { get; set; }
        public string y3 { get; set; }
        public string y4 { get; set; }
        public string y5 { get; set; }
        public string y6 { get; set; }
        public string y7 { get; set; }
        public string y8 { get; set; }
        public string y9 { get; set; }
        public string y10 { get; set; }
    }

    /// <summary>
    /// Chart Data Class (to Serialize and return)
    /// </summary>
    [XmlRoot("ChartData")]
    public class ChartData
    {
        public int TotalPages { get; set; }
        public int SeriesCount { get; set; }
        public string XTitle { get; set; }
        public string YTitle { get; set; }
        [XmlArray("ChartTitles")]
        [XmlArrayItem("ChartTitle")]
        public List<string> ChartTitles { get; set; }
        //[XmlArray("LegendTexts")]
        //[XmlArrayItem("LegendText")]
        //public List<string> LegendTexts { get; set; }
        [XmlArray("ChartItems")]
        public List<ChartItem> ChartItems { get; set; }
        [XmlArray("GaugeItems")]
        public List<GaugeItem> GaugeItems { get; set; }

        public string XmlSerialize()
        {
            XmlSerializer ser = new XmlSerializer(typeof(ChartData));

            StringWriter sw = new StringWriter();

            XmlTextWriter tw = new XmlTextWriter(sw);

            ser.Serialize(tw, this);

            string result = sw.ToString();
            return result;

        }

        public static ChartData XmlDeserialize(string xml)
        {
            //deserialize to object
            XmlSerializer ser = new XmlSerializer(typeof(ChartData));
            ChartData obj = (ChartData)ser.Deserialize(XmlReader.Create(new StringReader(xml)));
            return obj;
        }
    }

    /// <summary>
    /// GridData Data Class (to Serialize and return)
    /// </summary>
    [XmlRoot("ChartData")]
    public class GridData
    {
        public int TotalRows { get; set; }
        public int SeriesCount { get; set; }
        public string XTitle { get; set; }
        public string YTitle { get; set; }
        [XmlArray("ChartTitles")]
        [XmlArrayItem("ChartTitle")]
        public List<string> ChartTitles { get; set; }
        //[XmlArray("LegendTexts")]
        //[XmlArrayItem("LegendText")]
        //public List<string> LegendTexts { get; set; }
        [XmlArray("GaugeItems")]
        public List<GaugeItem> GaugeItems { get; set; }
        [XmlArray("ChartItems")]
        [XmlArrayItem("ChartItem")]
        public List<GridItem> GridItems { get; set; }

        public string XmlSerialize()
        {
            XmlSerializer ser = new XmlSerializer(typeof(GridData));

            StringWriter sw = new StringWriter();

            XmlTextWriter tw = new XmlTextWriter(sw);

            ser.Serialize(tw, this);

            string result = sw.ToString();
            return result;

        }

        public static GridData XmlDeserialize(string xml)
        {
            //deserialize to object
            XmlSerializer ser = new XmlSerializer(typeof(GridData));
            GridData obj = (GridData)ser.Deserialize(XmlReader.Create(new StringReader(xml)));
            return obj;
        }
    }

    #endregion

    #region Filter Parameter Data
    /// <summary>
    /// Filter Parameter Class( to serialize and send)
    /// </summary>
    public class FilterParameters
    {

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int BizUnit { get; set; }
        public int UserPK { get; set; }
        public int Dept { get; set; }
        public int CusPK { get; set; }
        [XmlArrayItem("Parameter")]
        public List<Parameter> Parameters { get; set; }
        /// <summary>
        /// To Serialize the FilterParameters class object
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize()
        {
            XmlSerializer ser;
            StringWriter sw;
            XmlTextWriter tw;
            string result;
            result = string.Empty;
            try
            {
                ser = new XmlSerializer(typeof(FilterParameters));
                sw = new StringWriter();
                tw = new XmlTextWriter(sw);
                ser.Serialize(tw, this);
                result = sw.ToString();
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// To Deserialize the FilterParameters class object
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static FilterParameters XmlDeserialize(string xml)
        {
            XmlSerializer ser;
            FilterParameters obj;
            obj = null;
            try
            {
                //deserialize to object
                ser = new XmlSerializer(typeof(FilterParameters));
                obj = (FilterParameters)ser.Deserialize(XmlReader.Create(new StringReader(xml)));

            }
            catch
            {

            }
            return obj;
        }
    }
    /// <summary>
    /// Filter Parameters Class( to serialize and return)
    /// </summary>
    public class Parameter
    {
        public String Name { get; set; }
        public String Label { get; set; }
        [XmlArray("Values")]
        [XmlArrayItem("Item")]
        public List<ParamItem> Values { get; set; }

        /// <summary>
        /// To Serialize the Parameters class object
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize()
        {
            XmlSerializer ser;
            StringWriter sw;
            XmlTextWriter tw;
            string result;
            result = string.Empty;
            try
            {
                ser = new XmlSerializer(typeof(Parameter));
                sw = new StringWriter();
                tw = new XmlTextWriter(sw);
                ser.Serialize(tw, this);
                result = sw.ToString();
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// To Deserialize the Parameters class object
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Parameter XmlDeserialize(string xml)
        {
            XmlSerializer ser;
            Parameter obj;
            obj = null;
            try
            {
                //deserialize to object
                ser = new XmlSerializer(typeof(Parameter));
                obj = (Parameter)ser.Deserialize(XmlReader.Create(new StringReader(xml)));

            }
            catch
            {

            }
            return obj;
        }
    }
    /// <summary>
    /// Filter Parameter Item Class
    /// </summary>
    public class ParamItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    #endregion
}
