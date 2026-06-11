using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class OtherAdditionDeductionBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class AdditionDeductionHeader
    {
        public int OAH_PK { get; set; }
        public string OAH_NO { get; set; }
        public string OAH_DATE_FROM { get; set; }
        public string OAH_DATE_TO { get; set; }
        public int OAH_CLASS { get; set; }
        public int OAH_PAY_ELEMENT { get; set; }
        public string OAH_DESC { get; set; }
        public byte OAH_STATUS { get; set; }
        public byte OAH_ACTIVE { get; set; }
        public int OAH_COMPANY { get; set; }
        public int OAH_BIZUNIT { get; set; }
        public int OAH_DEPT { get; set; }
        public int USER_PK { get; set; }
        public int OAH_PAYROLL_DTL { get; set; }
        public string OAH_DISP_NAME { get; set; }
        public int WKF_FLAG { get; set; }
        public int OAH_CURRENCY { get; set; }
        public string OAH_CURRENCY_CODE_TEXT { get; set; }
        public string OAH_CURRENCY_NAME_TEXT { get; set; }
        public int OAH_BASE_CURR { get; set; }
        public double OAH_EXCHG_RATE { get; set; }
             
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Details")]
        public List<AdditionDeductionDetails> EmpAddDedDtl { get; set; }
    }

    [Serializable]
    public class AdditionDeductionDetails
    {
        public int OAD_PK { get; set; }
        public int SlNo { get; set; }
        public int OAD_HDR_PK { get; set; }
        public DateTime OAD_DATE { get; set; }
        public int OAD_EMPLOYEE { get; set; }
        public string OAD_EMPLOYEE_NAME { get; set; }
        public double OAD_AMOUNT { get; set; }
        public string OAD_REMARKS { get; set; }
        public byte OAD_ACTIVE { get; set; }
        public int EmpBranch { get; set; }
        public string EmpBranch_Text { get; set; }
        public string EMP_TYPE { get; set; }
        public string EMP_TYPE_TEXT { get; set; }
        public int OAD_PAYROLL_DTL { get; set; }
    }


    #region Import
    [Serializable]
    [XmlRoot("Root")]
    public class AddDedImportHeader
    {
        [XmlElement("OAH_PK")]
        public int OAH_PK { get; set; }
        [XmlElement("OAH_NO")]
        public string OAH_NO { get; set; }
        [XmlElement("OAH_DATE_FROM")]
        public string OAH_DATE_FROM { get; set; }
        [XmlElement("OAH_DATE_TO")]
        public string OAH_DATE_TO { get; set; }
        [XmlElement("OAH_CLASS")]
        public int OAH_CLASS { get; set; }
        [XmlElement("OAH_PAY_ELEMENT")]
        public int OAH_PAY_ELEMENT { get; set; }
        [XmlElement("OAH_DESC")]
        public string OAH_DESC { get; set; }
        [XmlElement("OAH_STATUS")]
        public byte OAH_STATUS { get; set; }
        [XmlElement("OAH_ACTIVE")]
        public byte OAH_ACTIVE { get; set; }
        [XmlElement("OAH_COMPANY")]
        public int OAH_COMPANY { get; set; }
        [XmlElement("OAH_BIZUNIT")]
        public int OAH_BIZUNIT { get; set; }
        [XmlElement("OAH_DEPT")]
        public int OAH_DEPT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("OAH_PAYROLL_DTL")]
        public int OAH_PAYROLL_DTL { get; set; }
        [XmlElement("OAH_DISP_NAME")]
        public string OAH_DISP_NAME { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("OAH_CURRENCY")]
        public int OAH_CURRENCY { get; set; }
        [XmlElement("OAH_CURRENCY_CODE_TEXT")]
        public string OAH_CURRENCY_CODE_TEXT { get; set; }
        [XmlElement("OAH_CURRENCY_NAME_TEXT")]
        public string OAH_CURRENCY_NAME_TEXT { get; set; }
        [XmlElement("OAH_BASE_CURR")]
        public int OAH_BASE_CURR { get; set; }
        [XmlElement("OAH_EXCHG_RATE")]
        public double OAH_EXCHG_RATE { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Details")]
        public List<AddDedImportDetails> EmpAddDedDetails { get; set; }
    }

    [Serializable]
    public class AddDedImportDetails
    {
        [XmlElement("OAD_PK")]
        public int OAD_PK { get; set; }
        [XmlElement("SlNo")]
        public int SlNo { get; set; }
        [XmlElement("OAD_HDR_PK")]
        public int OAD_HDR_PK { get; set; }
        [XmlElement("OAD_DATE")]
        public DateTime OAD_DATE { get; set; }
        [XmlElement("OAD_EMPLOYEE_CODE")]
        public string OAD_EMPLOYEE_CODE { get; set; }
        [XmlElement("OAD_EMPLOYEE")]
        public int OAD_EMPLOYEE { get; set; }
        [XmlElement("OAD_EMPLOYEE_NAME")]
        public string OAD_EMPLOYEE_NAME { get; set; }
        [XmlElement("OAD_AMOUNT")]
        public double OAD_AMOUNT { get; set; }
        [XmlElement("OAD_REMARKS")]
        public string OAD_REMARKS { get; set; }
        [XmlElement("OAD_ACTIVE")]
        public byte OAD_ACTIVE { get; set; }
        [XmlElement("EmpBranch")]
        public int EmpBranch { get; set; }
        [XmlElement("EmpBranch_Text")]
        public string EmpBranch_Text { get; set; }
        [XmlElement("EMP_TYPE")]
        public string EMP_TYPE { get; set; }
        [XmlElement("OAD_PAYROLL_DTL")]
        public int OAD_PAYROLL_DTL { get; set; }
    } 
    #endregion
}
