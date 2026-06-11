using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject
{
    [Serializable]
    [XmlRoot("Root")]
    public class SaleForecastBO
    {

        [XmlElement("Details")]
        public List<ForecastDetails> Details { get; set; }
       
    }
    [Serializable]
    public class ForecastDetails
    {
        [XmlElement("FMD_FORECAST_DATE")]
        public DateTime  FMD_FORECAST_DATE { get; set; }
        [XmlElement("FMD_CUSTOMER")]
        public int FMD_CUSTOMER { get; set; }
        [XmlElement("FMD_MAIL_DATE")]
        public string FMD_MAIL_DATE { get; set; }
        [XmlElement("FMD_RPT_BIZUNIT")]
        public int FMD_RPT_BIZUNIT { get; set; }
        [XmlElement("FMD_RPT_CRTD_BY")]
        public int FMD_RPT_CRTD_BY { get; set; }
        [XmlElement("FMD_RPT_CRTD_DT")]
        public string FMD_RPT_CRTD_DT { get; set; }
    }

    

}
