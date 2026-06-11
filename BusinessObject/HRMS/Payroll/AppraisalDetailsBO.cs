using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data;

namespace BusinessObject.HRMS.Payroll
{
    [Serializable]
    [XmlRoot("Root")]
    public sealed class AppraisalDetailsBO
    {
        [XmlElement("EIH_PK")]
        public int CurrPk { get; set; } 
        [XmlElement("EIH_DATE")]
        public string TransactDate { get; set; }
        [XmlElement("EIH_BRANCH")]
        public string Branch { get; set; }
        [XmlElement("EIH_EMPMNT_TYPE")]
        public string EmpType { get; set; }
        [XmlElement("EIH_EMPLOYEE")]
        public string Employee { get; set; } 
        [XmlElement("EIH_NO")]
        public string TransactNo { get; set; }
        [XmlElement("EIH_TRN_NAME")]
        public string TransactName { get; set; }
        [XmlElement("EIH_TYPE")]
        public string Type { get; set; }
        [XmlElement("EIH_EFFECT_DATE")]
        public string Effectdate { get; set; }  
        [XmlElement("EIH_DEPT")]
        public int DepartMent { get; set; }
        [XmlElement("EIH_COMPANY")]
        public string Company { get; set; }
        [XmlElement("BIZUNIT_PK")]
        public int BizUnit { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LastModDate { get; set; }
        [XmlElement("USER_PK")]
        public int UserPk { get; set; }
        [XmlElement("EIH_EMPLOYEE_TEXT")]
        public string EmployeeText { get; set; }
        [XmlElement("EIH_EMP_DEPT")]
        public string EmpDepartMent { get; set; }
        [XmlElement("EIH_EMP_DESGN")] 
        public string EmpDesignation { get; set; } 

        [XmlElement("Detail")]
        public List<Details> Details { get; set; }
    }

    [Serializable]
    [XmlRoot("Detail")]
    public sealed class  Details
    {
        [XmlElement("EID_PK")]
        public int DetPK { get; set; }
        [XmlElement("EID_PAY_ELEMENT")]
        public string IncrementOn { get; set; }
        [XmlElement("EID_FORMULA")]
        public string Formula { get; set; }
        [XmlElement("EID_VALUE")]
        public double Value { get; set; }
        [XmlElement("EID_REMARKS")]
        public string Remarks { get; set; }
        [XmlElement("EID_PAY_ELEMENT_TEXT")]
        public string IncrementOnText { get; set; }
        [XmlElement("EID_FORMULA_TEXT")]
        public string FormulaText { get; set; }
        [XmlElement("EID_MIN_AMT")]
        public decimal MinAmount { get; set; }
        [XmlElement("EID_MAX_AMT")]
        public decimal MaxAmount { get; set; }
        
    }
}
