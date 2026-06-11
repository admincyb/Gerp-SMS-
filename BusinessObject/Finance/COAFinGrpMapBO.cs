using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Web;
namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class COAFinGrpMapBO
    {
        [XmlElement("RTC_PK")]
        public int RTC_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("Detail")]
        public List<COAFinGrpMapDetailBO> COAFinGrpMapDetailBO { get; set; }
    }
    [Serializable]
    public class COAFinGrpMapDetailBO
    {
        [XmlElement("COA_PK")]
        public int COA_PK { get; set; }
    }

    //FIN_COA_COST_CENTER_MPG
    [Serializable]
    [XmlRoot("Root")]
    public class CostCenterMpg
    {
        [XmlElement("FCM_COA_PK")]
        public int FCM_COA_PK { get; set; }
        [XmlElement("CC_PERCNT_IS_REQD")]
        public string CC_PERCNT_IS_REQD { get; set; }
        [XmlElement("Details")]
        public List<DetailsBO> DetailsList { get; set; }
    }
    [Serializable]
    public class DetailsBO
    {
        [XmlElement("FCM_PK")]
        public int FCM_PK { get; set; }
        [XmlElement("FCM_CNM_PK")]
        public int FCM_CNM_PK { get; set; }
        [XmlElement("FCM_VALUE")]
        public float FCM_VALUE { get; set; }
    }

}
