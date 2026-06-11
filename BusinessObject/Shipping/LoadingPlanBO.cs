using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Shipping
{
    [Serializable]
    [XmlRoot("ROOT")]
    public class LoadingPlanBO
    {
        [XmlElement("LPH_PK")]
        public string LPH_PK { get; set; }
        [XmlElement("LPH_SHIPPING_PLAN")]
        public int LPH_SHIPPING_PLAN { get; set; }
        [XmlElement("LPH_DATE")]
        public string LPH_DATE { get; set; }
        [XmlElement("LPH_DESC")]
        public string LPH_DESC { get; set; }
        [XmlElement("LPH_DEPT")]
        public string LPH_DEPT { get; set; }
        [XmlElement("LPH_COMPANY")]
        public int LPH_COMPANY { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public string USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("APT_CODE")]
        public string APT_CODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }


        [XmlElement("DETAIL")]
        public List<DetailsBO> DetailsList { get; set; }
    }

    [Serializable]
    public class DetailsBO
    {
        [XmlElement("LPD_PK")]
        public string LPD_PK { get; set; }
        [XmlElement("LPD_ROW_FROM")]
        public int LPD_ROW_FROM { get; set; }
        [XmlElement("LPD_ROW_TO")]
        public int LPD_ROW_TO { get; set; }
        [XmlElement("LPD_TYPE")]
        public string LPD_TYPE { get; set; }
        [XmlElement("LPD_QTY_X")]
        public double LPD_QTY_X { get; set; }
        [XmlElement("LPD_QTY_Y")]
        public double LPD_QTY_Y { get; set; }
        [XmlElement("LPD_QTY")]
        public double LPD_QTY { get; set; }
        [XmlElement("LPD_LOT_NO")]
        public string LPD_LOT_NO { get; set; }
        [XmlElement("LPD_ACTIVE")]
        public int LPD_ACTIVE { get; set; }
        [XmlElement("LPD_MOD_BY")]
        public int LPD_MOD_BY { get; set; }
        [XmlElement("LPD_MOD_DT")]
        public string LPD_MOD_DT { get; set; }
        [XmlElement("LPD_CARTON_FROM")]
        public string LPD_CARTON_FROM { get; set; }
        [XmlElement("LPD_CARTON_TO")]
        public string LPD_CARTON_TO { get; set; }

        [XmlElement("LPD_SO_HDR")]
        public string LPD_SO_HDR { get; set; }
        
        [XmlElement("LPH_SO_HDR")]
        public string LPH_SO_HDR { get; set; }
        
        [XmlElement("LPH_SO_DTL")]
        public string LPH_SO_DTL { get; set; }
       
        [XmlElement("LPH_SC_DATE")]
        public string LPH_SC_DATE { get; set; }
        [XmlElement("LPH_BRAND")]
        public string LPH_BRAND { get; set; }
[XmlElement("LPH_BRAND_NAME")]
        public string LPH_BRAND_NAME { get; set; }
        [XmlElement("LPH_EXP_DATE")]
        public string LPH_EXP_DATE { get; set; }
        [XmlElement("LPH_NET_WT")]
        public string LPH_NET_WT { get; set; }
        [XmlElement("LPH_CTN_NET_WT")]
        public string LPH_CTN_NET_WT { get; set; }
        [XmlElement("LPH_GROSS_WT")]
        public string LPH_GROSS_WT { get; set; }
        [XmlElement("LPH_CTN_GROSS_WT")]
        public string LPH_CTN_GROSS_WT { get; set; }
        [XmlElement("LPH_IR_RADIATION_LOT_NO")]
        public string LPH_IR_RADIATION_LOT_NO { get; set; }

    }

}
