using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class MonthlyProductionHdr : WorkflowBO
    {
        [XmlElement("FPH_PK")]
        public int FPH_PK { get; set; }
        [XmlElement("FPH_NO")]
        public string FPH_NO { get; set; }
        [XmlElement("FPH_MONTH")]
        public string FPH_MONTH { get; set; }
        [XmlElement("FPH_TRX_DATE")]
        public string FPH_TRX_DATE { get; set; }
        [XmlElement("FPH_REMARKS")]
        public string FPH_REMARKS { get; set; }
        [XmlElement("FPH_DEPT")]
        public int FPH_DEPT { get; set; }
        [XmlElement("FPH_COMPANY")]
        public int FPH_COMPANY { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("Detail")]
        public List<MonthlyProductionDtl> MonthlyProductionList { get; set; }
    }

    [Serializable]
    public class MonthlyProductionDtl
    {
        [XmlElement("PLD_SL_NO")]
        public int PLD_SL_NO { get; set; }
        [XmlElement("FPD_PK")]
        public int FPD_PK { get; set; }
        [XmlElement("FPD_PLANT")]
        public int FPD_PLANT { get; set; }
        [XmlElement("FPD_PLANT_TEXT")]
        public string FPD_PLANT_TEXT { get; set; }
        [XmlElement("FPD_QUANTITY")]
        public double FPD_QUANTITY { get; set; }
        [XmlElement("FPD_REMARKS")]
        public string FPD_REMARKS { get; set; }
    }
}
