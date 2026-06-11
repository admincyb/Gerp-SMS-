using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class SalaryReportTemplateBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class SalaryReportTemplateHeader
    {
        [XmlElement("SRT_PK")]
        public int HdrPK { get; set; }
        [XmlElement("SRT_CODE")]
        public string SRT_CODE { get; set; }
        [XmlElement("SRT_NAME")]
        public string SalRptTempName { get; set; }
        [XmlElement("SRT_DESC")]
        public string SalRptTempRemark { get; set; }
        [XmlElement("SRT_VALUE")]
        public int SalRptTempValue { get; set; }
        [XmlElement("SRT_ACTIVE")]
        public int Active { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<SalaryReportTemplateGroupDetails> SalaryReportTemplateGrpDtl { get; set; }
    }

    [Serializable]
    public class SalaryReportTemplateGroupDetails
    {
        [XmlElement("SGD_PK")]
        public int GroupPk { get; set; }
        [XmlElement("SGD_SRT_PK")]
        public int GroupHdrPK { get; set; }
        [XmlElement("SGD_NAME")]
        public string GroupName { get; set; }
        [XmlElement("SGD_DESC")]
        public string SGD_DESC { get; set; }
        [XmlElement("SGD_SEQUENCE")]
        public int GroupSequence { get; set; }
        [XmlElement("SGD_ACTIVE")]
        public int GroupActive { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("ItemDetail")]
        public List<SalaryReportTemplateItemDetails> SalaryReportTemplateItemDtl { get; set; }
    }

    [Serializable]
    public class SalaryReportTemplateItemDetails 
    {
        [XmlElement("TDL_PK")]
        public int TDL_PK { get; set; }
        [XmlElement("TDL_SGD_PK")]
        public int TDL_SGD_PK { get; set; }
        [XmlElement("TDL_PEL_PK")]
        public int PayElementPK { get; set; }
        [XmlElement("TDL_DISP_NAME")]
        public string PayElementDispName { get; set; }
        [XmlElement("TDL_SEQUENCE")]
        public int TDL_SEQUENCE { get; set; }
        [XmlElement("TDL_ACTIVE")]
        public int TDL_ACTIVE { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("PAY_ELEMENT_NAME")]
        public string PayElementName { get; set; }
    }
}
 