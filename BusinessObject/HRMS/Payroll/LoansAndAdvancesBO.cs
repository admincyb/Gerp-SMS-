using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class LoansAndAdvancesBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmpLoanHeader
    {
        public int ELM_PK { get; set; }
        public string ELM_NO { get; set; }
        public int ELM_EMPLOYEE { get; set; }       
        public int ELM_PAY_ELEMENT { get; set; }
        public string ELM_APPLY_DATE { get; set; }
        public string ELM_APPROVED_DATE { get; set; }
        public string ELM_EFFECT_DATE { get; set; }
        public double ELM_PRINCIPAL_AMT { get; set; }
        public int ELM_INST_COUNT { get; set; }
        public double ELM_INST_AMT { get; set; }
        public double? ELM_ROI { get; set; }
        public byte ELM_STATUS { get; set; }
        public byte ACTIVE { get; set; }
        public int ELM_DEPT { get; set; }
        public int BIZUNIT_PK { get; set; }
        public string ELM_DESC { get; set; }
        //public int ELM_COMPANY { get; set; }
        public int USER_PK { get; set; }
        public string ELM_EMPLOYEE_TEXT { get; set; }
        public string ELM_EMP_TEXT { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public int WKF_FLAG { get; set; }
        public int ELM_CURRENCY { get; set; }
        public string ELM_CURRENCY_CODE_TEXT { get; set; }
        public string ELM_CURRENCY_NAME_TEXT { get; set; }
        public int ELM_BASE_CURR { get; set; }
        public double ELM_EXCHG_RATE { get; set; }
        public int ELM_COMPANY { get; set; }
        [XmlElement("SlabDetails")]
        public List<EmpLoanDetails> LoanSlabDetails { get; set; }



        
    }

    [Serializable]
    public class EmpLoanDetails
    {
        public int SlNo { get; set; }
        public int ELS_PK { get; set; }
        public int ELS_LOAN_MST { get; set; }
        public DateTime ELS_INST_DATE { get; set; }
        public double ELS_INST_AMT { get; set; }
        public double ELS_PAID_AMT{get; set;}
    }
}
