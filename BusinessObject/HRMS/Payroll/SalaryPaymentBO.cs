using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class SalaryPaymentBO
    {
    }
    [Serializable]
    [XmlRoot("Root")]
    public class SalaryPaymentHeader : ISmsBO, IMailBO
    {
        [XmlElement("PSH_PK")]
        public int PSH_PK { get; set; }
        [XmlElement("PSH_NO")]
        public string PSH_NO { get; set; }
        [XmlElement("PSH_DATE")]
        public DateTime PSH_DATE { get; set; }
        [XmlElement("PSH_REMARKS")]
        public string PSH_REMARKS { get; set; }
        [XmlElement("PSH_ACTIVE")]
        public int PSH_ACTIVE { get; set; }
        [XmlElement("PSH_COMPANY")]
        public int PSH_COMPANY { get; set; }
        [XmlElement("PSH_BIZUNIT")]
        public int PSH_BIZUNIT { get; set; }
        [XmlElement("PSH_DEPT")]
        public int PSH_DEPT { get; set; }
        [XmlElement("LAST_MOD_DT")]
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("USER_PK")]
        public int USER_PK { get; set; }
        [XmlElement("WKF_FLAG")]
        public int WKF_FLAG { get; set; }
        [XmlElement("PSH_DEL_STATUS")]
        public int PSH_DEL_STATUS { get; set; }
        [XmlElement("PSH_STATUS")]
        public int PSH_STATUS { get; set; }

        public int PSH_CURRENCY { get; set; }
        public string PSH_CURRENCY_CODE_TEXT { get; set; }
        public string PSH_CURRENCY_NAME_TEXT { get; set; }
        public int PSH_BASE_CURR { get; set; }
        public double PSH_EXCHG_RATE { get; set; }

        [XmlElement("WKF_REFERENCE")]
        public int WKF_REFERENCE { get; set; }
        [XmlElement("WKF_APPLICATION")]
        public int WKF_APPLICATION { get; set; }
        [XmlElement("WKF_PROCESS")]
        public int WKF_PROCESS { get; set; }
        [XmlElement("WKF_TASK")]
        public int WKF_TASK { get; set; }
        [XmlElement("WKF_TASK_ACTION")]
        public int WKF_TASK_ACTION { get; set; }
        [XmlElement("WKF_COMMENTS")]
        public string WKF_COMMENTS { get; set; }
        [XmlElement("WKF_TRX_FLAG")]
        public int WKF_TRX_FLAG { get; set; }

        [XmlElement("PSH_HAS_JRNL_ENTRY")]
        public int PSH_HAS_JRNL_ENTRY { get; set; }

        [XmlElement("ModeDetails")]
        public List<SalaryPaymentModeDetails> SalaryPaymentModeDtl { get; set; }

        [XmlElement("SMS")]
        public SmsList SmsList { get; set; }
        [XmlElement("EMAIL")]
        public MailList MailList { get; set; }
    }

    [Serializable]
    public class SalaryPaymentModeDetails
    {
        [XmlElement("ROW_NO")]
        public int ROW_NO { get; set; }
        [XmlElement("PSP_PK")]
        public int PSP_PK { get; set; }
        [XmlElement("PSP_PSH_PK")]
        public int PSP_PSH_PK { get; set; }
        [XmlElement("PSP_MODE")]
        public int PSP_MODE { get; set; }
        [XmlElement("PSP_MODE_TEXT")]
        public string PSP_MODE_TEXT { get; set; }
        [XmlElement("PSP_BANK")]
        public string PSP_BANK { get; set; }
        [XmlElement("PSP_BANK_TEXT")]
        public string PSP_BANK_TEXT { get; set; }
        [XmlElement("PSP_INSTR_NO")]
        public string PSP_INSTR_NO { get; set; }
        [XmlElement("PSP_DATE")]
        public string PSP_DATE { get; set; }
        [XmlElement("PSP_REMARK")]
        public string PSP_REMARK { get; set; }
        [XmlElement("PSP_AMOUNT")]
        public decimal PSP_AMOUNT { get; set; }
        [XmlElement("Details")]
        public List<SalaryPaymentDetails> SalaryPaymentDtl { get; set; }
    }

    [Serializable]
    public class SalaryPaymentDetails
    {
        [XmlElement("ROW_NO")]
        public int ROW_NO { get; set; }
        [XmlElement("PSL_PK")]
        public int PSL_PK { get; set; }
        [XmlElement("PSL_PSH_PK")]
        public int PSL_PSH_PK { get; set; }
        [XmlElement("PSL_PSP_PK")]
        public int PSL_PSP_PK { get; set; }
        [XmlElement("PSL_PAYROLL_MONTH")]
        public string PSL_PAYROLL_MONTH { get; set; }
        [XmlElement("PSL_FROM_DATE")]
        public string PSL_FROM_DATE { get; set; }
        [XmlElement("PSL_TO_DATE")]
        public string PSL_TO_DATE { get; set; }
        [XmlElement("PSL_EPS_PK")]
        public string PSL_EPS_PK { get; set; }
        [XmlElement("PSL_EMPLOYEE")]
        public int PSL_EMPLOYEE { get; set; }
        [XmlElement("PSL_EMPLOYEE_TEXT")]
        public string PSL_EMPLOYEE_TEXT { get; set; }
        [XmlElement("PSL_NET_SAL")]
        public decimal PSL_NET_SAL { get; set; }
        [XmlElement("PSL_BANK")]
        public string PSL_BANK { get; set; }
        [XmlElement("PSL_BANK_TEXT")]
        public string PSL_BANK_TEXT { get; set; }
        [XmlElement("PSL_ACCOUNT_NO")]
        public string PSL_ACCOUNT_NO { get; set; }
        [XmlElement("PSL_BANK_IFSC")]
        public string PSL_BANK_IFSC { get; set; }
        [XmlElement("IS_DELETED")]
        public int IS_DELETED { get; set; }
        [XmlElement("empDesignationText")]
        public string empDesignationText { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class SalaryPaymentHeader_PopUp
    {
        [XmlElement("Details")]
        public List<SalaryPaymentDetails> SalaryEmployee_PopUPDtl { get; set; }
    }

    public class SalaryEmployee_PopUP
    {
        public int ROW_NO { get; set; }
        public int PSL_PK { get; set; }
        public int PSL_PSH_PK { get; set; }
        public string PSL_PAYROLL_MONTH { get; set; }
        public string PSL_FROM_DATE { get; set; }
        public string PSL_TO_DATE { get; set; }
        public string PSL_EPS_PK { get; set; }
        public int PSL_EMPLOYEE { get; set; }
        public string PSL_EMPLOYEE_TEXT { get; set; }
        public decimal PSL_NET_SAL { get; set; }
        public string PSL_BANK { get; set; }
        public string PSL_BANK_TEXT { get; set; }
        public string PSL_ACCOUNT_NO { get; set; }
        public string PSL_BANK_IFSC { get; set; }
        public int IS_DELETED { get; set; }
        public string empDesignationText { get; set; }
        
    }

}
