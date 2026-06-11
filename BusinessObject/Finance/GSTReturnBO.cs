using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BusinessObject.Finance
{
    [Serializable]
    [XmlRoot("Root")]
    public class GSTReturnHeader
    {
        [XmlElement("TGH_PK")]
        public int TGH_PK { get; set; }
        [XmlElement("TGH_NO")]
        public string TGH_NO { get; set; }
        [XmlElement("TGH_FROM_DATE")]
        public string TGH_FROM_DATE { get; set; }
        [XmlElement("TGH_TO_DATE")]
        public string TGH_TO_DATE { get; set; }
        [XmlElement("TGH_DUE_DATE")]
        public string TGH_DUE_DATE { get; set; }
        [XmlElement("TGH_STATUS")]
        public int TGH_STATUS { get; set; }
        [XmlElement("TGH_DEPT")]
        public int TGH_DEPT { get; set; }
        [XmlElement("TGH_COMPANY")]
        public int TGH_COMPANY { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BIZUNIT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public string LAST_MOD_DT { get; set; }
        [XmlElement("TGH_DEC_NAME")]
        public string TGH_DEC_NAME { get; set; }
        [XmlElement("TGH_DEC_ID_NEW")]
        public string TGH_DEC_ID_NEW { get; set; }
        [XmlElement("TGH_DEC_ID_OLD")]
        public string TGH_DEC_ID_OLD { get; set; }
        [XmlElement("TGH_DEC_PASSPORT_NO")]
        public string TGH_DEC_PASSPORT_NO { get; set; }
        [XmlElement("TGH_DEC_NATIONALITY")]
        public string TGH_DEC_NATIONALITY { get; set; }
        [XmlElement("TGH_DEC_DATE")]
        public string TGH_DEC_DATE { get; set; }
        [XmlElement("TGH_MOD_DT")]
        public string TGH_MOD_DT { get; set; }

        [XmlElement("APT_CODE")]
        public string AST_CODE { get; set; }
        [XmlElement("AST_DOC_MODE")]
        public int AST_DOC_MODE { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }

        [XmlElement("Detail")]
        public List<GSTReturnDetails> listGSTReturnDetails { get; set; }
        [XmlElement("BreakDetail")]
        public List<GSTReturnBreakDetail> listGSTReturnBreakDetail { get; set; }
    }
    [Serializable]
    public class GSTReturnDetails
    {
        [XmlElement("TGD_PK")]
        public int TGD_PK { get; set; }
        [XmlElement("TGD_TAX_GST")]
        public string TGD_TAX_GST { get; set; }
        [XmlElement("TGD_AMOUNT")]
        public string TGD_AMOUNT { get; set; }
    }
    [Serializable]
    public class GSTReturnBreakDetail
    {
        [XmlElement("TGS_PK")]
        public int TGS_PK { get; set; }
        [XmlElement("TGS_CODE")]
        public string TGS_CODE { get; set; }
        [XmlElement("TGS_AMOUNT")]
        public string TGS_AMOUNT { get; set; }
        [XmlElement("TGS_RATE")]
        public string TGS_RATE { get; set; }
    }
}
