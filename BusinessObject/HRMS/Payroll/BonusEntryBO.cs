using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class BonusEntryBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class BonusEntryHeader
    {
        [XmlElement("BOH_PK")]
        public int BOH_PK { get; set; }
        [XmlElement("BOH_NO")]
        public string BOH_NO { get; set; }
        [XmlElement("BOH_DATE")]
        public DateTime BOH_DATE { get; set; }
        [XmlElement("BOH_BONUS_TYPE")]
        public int BOH_BONUS_TYPE { get; set; }
        [XmlElement("BOH_BONUS_TYPE_TEXT")]
        public string BOH_BONUS_TYPE_TEXT { get; set; }
        [XmlElement("BOH_REMARK")]
        public string BOH_REMARK { get; set; }
        [XmlElement("BOH_COMPANY")]
        public int BOH_COMPANY { get; set; }
        [XmlElement("BOH_BIZUNIT")]
        public int BOH_BIZUNIT { get; set; }
        [XmlElement("BOH_DEPT")]
        public int BOH_DEPT { get; set; }
        [XmlElement("BOH_MOD_DT")]
        public DateTime BOH_MOD_DT { get; set; }
        [XmlElement("BOH_STATUS	")]
        public int BOH_STATUS { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("BOH_TOTAL_AMT")]
        public decimal BOH_TOTAL_AMT { get; set; }

        public int BOH_DEL_STATUS { get; set; }
        public int BOH_CURRENCY { get; set; }
        public string BOH_CURRENCY_TEXT { get; set; }
        public string BOH_BASE_CURR_TEXT { get; set; }
        public int BOH_BASE_CURR { get; set; }
        public double BOH_EXCHG_RATE { get; set; }

        [XmlElement("Details")]
        public List<BonusEntryDetails> BonusEntryDtl { get; set; }
    }

    [Serializable]
    public class BonusEntryDetails
    {
        [XmlElement("ROW_NO")]
        public int ROW_NO { get; set; }
        [XmlElement("BOD_PK")]
        public int BOD_PK { get; set; }
        [XmlElement("BOD_BOH_PK")]
        public int BOD_BOH_PK { get; set; }
        [XmlElement("BOD_EMPLOYEE")]
        public int BOD_EMPLOYEE { get; set; }
        [XmlElement("BOD_AMOUNT")]
        public decimal BOD_AMOUNT { get; set; }
        [XmlElement("BOD_ACTIVE")]
        public int BOD_ACTIVE { get; set; }
        [XmlElement("empName_txt")]
        public string empName_txt { get; set; }
        [XmlElement("empBranchText")]
        public string empBranchText { get; set; }
        [XmlElement("empDepartmentText")]
        public string empDepartmentText { get; set; }
        [XmlElement("EPD_EMP_TYPE_TEXT")]
        public string EPD_EMP_TYPE_TEXT { get; set; }
        [XmlElement("empDesignationText")]
        public string empDesignationText { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmployeeBonusHeader_PopUp
    {
        [XmlElement("Details")]
        public List<EmployeeBonus_PopUP> EmployeeBonus_PopUPDtl { get; set; }
    }

    public class EmployeeBonus_PopUP
    {
        public int ROW_NO { get; set; }
        public int ETL_PK { get; set; }
        public int ETL_EMP_PK { get; set; }
        public string empName_txt { get; set; }
        public string empBranchText { get; set; }
        public string empDepartmentText { get; set; }
        public string empDesignationText { get; set; }
    }
}
