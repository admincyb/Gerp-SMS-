using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Admin.Masters
{
    public class SalaryTemplateBO
    {
        [Serializable]
        public class PayElement
        {
            public int PelPk { get; set; }
            public string PelCode { get; set; }
            public string PelName { get; set; }
            public string PelFormulaCode { get; set; }
            public bool PelDeduction { get; set; }
            public int PelCalcMode { get; set; }
        }


        [Serializable]
        [XmlRoot("Detail")]
        public class SalaryTemplateDetail
        {
            public int SlNo { get; set; }
            [XmlElement("STS_PK")]
            public int Pk { get; set; }
            [XmlElement("PEL_CLASS")]
            public int PayClassificationPk { get; set; }
            [XmlElement("STS_PAY_ELEMENT")]
            public int PayElementPk { get; set; }
            [XmlElement("STS_PAY_ELEMENT_TEXT")]
            public string PayElementName { get; set; }

            [XmlElement("STS_VALUE_TEXT")]
            public string PayElementDisplay { get; set; }
            [XmlElement("STS_CALC_VALUE")]
            public string PayElementValue { get; set; }
            [XmlElement("PEL_IS_DEDUCTION")]
            public bool PayElementDeduction { get; set; }
            [XmlElement("STS_CALC_MODE")]
            public int PayCalculationMode { get; set; }
            public string PayCalculationModeText { get; set; }
            [XmlElement("STS_SL_NO")]
            public int STS_SL_NO { get; set; }
            [XmlElement("STS_MIN_AMT")]
            public decimal MinAmount { get; set; }
            [XmlElement("STS_MAX_AMT")]
            public decimal MaxAmount { get; set; }
            public int IsEdited { get; set; }
        }


        [Serializable]
        [XmlRoot("Root")]
        public class SalaryTemplate
        {
            [XmlElement("STE_PK")]
            public int STE_PK { get; set; }
            [XmlElement("STE_CODE")]
            public string STE_CODE { get; set; }
            [XmlElement("STE_NAME")]
            public string STE_NAME { get; set; }
            [XmlElement("STE_DESC")]
            public string STE_DESC { get; set; }
            [XmlElement("STE_DEPT")]
            public int STE_DEPT { get; set; }
            [XmlElement("STE_COMPANY")]
            public int STE_COMPANY { get; set; }
            [XmlElement("STE_PAYROLL_TYPE")]
            public int STE_PAYROLL_TYPE { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("ACTIVE")]
            public int ACTIVE { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("Detail")]
            public List<SalaryTemplateDetail> SalaryTemplateDetails { get; set; }

        }

        [Serializable]
        [XmlRoot("Root")]
        public class EmpSalaryTemplate
        {
            [XmlElement("STE_PK")]
            public int STE_PK { get; set; }
            [XmlElement("STS_PK")]
            public int STS_PK { get; set; }
            [XmlElement("STS_CALC_VALUE")]
            public string STS_CALC_VALUE { get; set; }
            [XmlElement("INSERT_NEW")]
            public int INSERT_NEW { get; set; }
            [XmlElement("IS_DELETE")]
            public int IS_DELETE { get; set; }
            [XmlElement("BIZUNIT_PK")]
            public int BIZUNIT_PK { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("Detail")]
            public List<EmpTemplateDetails> EmpTemplateDetails { get; set; }

        }

        [Serializable]
        [XmlRoot("Detail")]
        public class EmpTemplateDetails
        {
            [XmlElement("STS_CALC_VALUE_TEXT")]
            public string STS_CALC_VALUE_TEXT { get; set; }
            [XmlElement("STS_CALC_VALUE")]
            public string STS_CALC_VALUE { get; set; }
        }
    }
}
