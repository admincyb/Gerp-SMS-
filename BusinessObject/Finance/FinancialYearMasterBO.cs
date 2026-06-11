using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
   public   class FinancialYearMasterBO
    {
        [XmlElement("FYR_PK")]
        public int FYR_PK { get; set; }
        [XmlElement("FYR_NAME")]
        public string FYR_NAME { get; set; }        
        [XmlElement("FYR_DESC")]
        public string FYR_DESC { get; set; }        
        [XmlElement("FYR_DATE_FROM")]
        public string FYR_DATE_FROM { get; set; } 
        [XmlElement("FYR_DATE_TO")]
        public string FYR_DATE_TO { get; set; }
        [XmlElement("FYR_ACTIVE")]
        public int FYR_ACTIVE { get; set; }
        [XmlElement("FYR_STATUS")]
        public int FYR_STATUS { get; set; }
        [XmlElement("FYR_DEPT")]
        public int FYR_DEPT { get; set; }
    }
    public class Field
    {
        public const string FYR_PK = "FYR_PK";
        public const string FYR_NAME = "FYR_NAME";
        public const string FYR_DESC = "FYR_DESC";
        public const string FYR_DATE_FROM = "FYR_DATE_FROM";
        public const string FYR_DATE_TO = "FYR_DATE_TO";
        public const string FYR_ACTIVE = "FYR_ACTIVE";
        public const string FYR_STATUS = "FYR_STATUS";
        public const string FYR_DEPT = "FYR_DEPT";
    }
}
