using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;


namespace BusinessObject.Stock
{
    public class StockBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class StockHDR
        {
            [XmlElement("BIT_PK")]
            public int BIT_PK { get; set; }
            [XmlElement("BIT_DATE")]
            public DateTime BIT_DATE { get; set; }
            [XmlElement("BIT_STATUS")]
            public int BIT_STATUS { get; set; }
            [XmlElement("BIT_DESC")]
            public string BIT_DESC { get; set; }
            [XmlElement("BIT_IS_OPEN")]
            public int BIT_IS_OPEN { get; set; }

            [XmlElement("BIT_COMPANY")]
            public string BIT_COMPANY { get; set; }
            [XmlElement("BIT_DEPT")]
            public int BIT_DEPT { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("ACTIVE")]
            public int ACTIVE { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; } 
        }
    }
}
