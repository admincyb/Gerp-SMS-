using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class PayrollProcessBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmpPayrollHeader
    {
       
        public int EPH_PK { get; set; }
        public string EPH_NO { get; set; }
        public int EPH_STATUS { get; set; }
        public string EPH_FROM_DATE { get; set; }
        public string EPH_TO_DATE { get; set; }
        public byte EPH_PRC_MODE { get; set; }
        public int EPH_PAYROLL_TYPE { get; set; }
        public string EPH_PAYROLL_TYPE_TEXT { get; set; }
        public string EPH_PRC_NAME { get; set; }
        public string EPH_DATE { get; set; }
        public int EPH_DEPT { get; set; }
        public int EPH_COMPANY { get; set; }
        public int BIZUNIT_PK { get; set; }
        public byte ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public byte IS_REPROCESS { get; set; }
        public DateTime EPH_PAYROLL_MONTH { get; set; }
        public int EPH_PROCESSED_COUNT { get; set; }
        public int EPH_FAILED_COUNT { get; set; }
        public int EPH_CURRENCY { get; set; }
        public string EPH_CURRENCY_TEXT { get; set; }
        public int EPH_BASE_CURR { get; set; }
        public double EPH_EXCHG_RATE { get; set; }
        public int WKF_FLAG { get; set; }
        public int EPH_HAS_JRNL_ENTRY { get; set; }    
        public int TOTAL_EMP_COUNT { get; set; }    
        public int TOTAL_PRC_COUNT { get; set; }    
        [XmlElement("Detail")]
        public List<EmpPayrollDetails> EmpPayrollDtl { get; set; }
        [XmlElement("Workdaysdtl")]
        public List<EmpWorkingDayDetails> EmpWorkingDaysDtl { get; set; }
        public int EPH_IS_FINAL_SETTLEMENT { get; set; }

        public string EPH_RESIGNATION_DATE { get; set; }



    }

    [Serializable]
    public class EmpPayrollDetails
    {
        public int EPS_PK { get; set; }
        public string EPS_TEXT { get; set; }
        public int EPS_PAYROLL_HDR { get; set; }
        public int EPS_EMPLOYEE { get; set; }
        public double EPS_GROSS_AMT { get; set; }
        public double EPS_NET_AMT { get; set; }
        public double EPS_DED_AMT { get; set; }
        public double EPS_ALW_AMT { get; set; }
        public double EPS_CTC_AMT { get; set; }
        public string EPS_MOD_DT { get; set; }
        public double EPS_WORK_DAYS { get; set; }
        public byte EPS_IS_LJ { get; set; }
        public string empDOJ { get; set; }
        public string PSL_PK { get; set; }
        [XmlElement("Detail")]
        public List<EmpPayrollPayDetails> EmpPayrollPayDtl { get; set; }

        public double EPS_GRATUITY { get; set; }
    }

    [Serializable]
    public class EmpWorkingDayDetails
    {
        public int EPW_PK { get; set; }
        public int EPW_LEAVE_TYPE { get; set; }
        public double EPW_WORK_DAYS { get; set; }
        public double EPW_HOLIDAYS { get; set; }
        public byte EPW_ACTIVE { get; set; }
        public int EPW_EMP_TYPE { get; set; }
        public string EMT_NAME { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmpPayrollPayHeader
    {
        public int EPS_PK { get; set; }
        public int EPS_PAYROLL_HDR { get; set; }
        public string EPH_PRC_NAME { get; set; }
        public string EPH_DATE { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empText { get; set; }
        public string empPK { get; set; }
        public string EPH_FROM_DATE { get; set; }
        public string EPH_TO_DATE { get; set; }
        public string EPD_PAY_MODE_TEXT { get; set; }
        public string EPD_CURR_TEXT { get; set; }
        public string EPD_PAY_BANK_TEXT { get; set; }
        public string EPD_PAY_BANK_BRANCH { get; set; }
        public string EPD_BANK_AC_NO { get; set; }
        public string EPD_BANK_AC_NAME { get; set; }
        public string EPD_PF_AC { get; set; }
        public string EPD_SOCSO_AC { get; set; }
        public string EPD_LOP { get; set; }
        public string EPD_WORK_DAYS { get; set; }
        public double EPS_GROSS_AMT { get; set; }
        public double EPS_NET_AMT { get; set; }
        public double EPS_DED_AMT { get; set; }
        public double EPS_ALW_AMT { get; set; }
        public double EPS_CTC_AMT { get; set; }
        public int USER_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public DateTime EPH_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<EmpPayrollPayDetails> EmpPayrollPayDtl { get; set; }
        [XmlElement("SalHistory")]
        public List<EmpSalHistoryDetails> EmpSalHistory { get; set; }
    }

    [Serializable]
    public class EmpPayrollPayDetails
    {
        public int EPP_PK { get; set; }
        public int EPP_SL_NO { get; set; }
        public int EPP_PAYROLL_DTL { get; set; }
        public int EPP_PAY_ELEMENT { get; set; }
        public string EPP_PAY_ELEMENT_TEXT { get; set; }
        public double EPP_ACT_PAY_AMT { get; set; }
        public double EPP_PAY_AMT { get; set; }
        public byte EPP_IS_DEDUCTION { get; set; }
        public int EPP_PAY_ELEMENT_EDITABLE { get; set; }
        public byte PEL_IN_SALARY { get; set; }
    }

    [Serializable]
    public class EmpSalHistoryDetails
    {
        public int EPS_PK { get; set; }
        public string EPH_PRC_NAME { get; set; }
        public DateTime EPH_FROM_DATE { get; set; }
        public DateTime EPH_TO_DATE { get; set; }
        public DateTime EPH_DATE { get; set; }
        public decimal EPS_GROSS_AMT { get; set; }
        public decimal EPS_NET_AMT { get; set; }
        public decimal EPS_DED_AMT { get; set; }
        public decimal EPS_ALW_AMT { get; set; }
        public DateTime EPH_PAYROLL_MONTH { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class PeriodWorkingDayHdr
    {
        public int EPW_PAYROLL_HDR { get; set; }
        public string EPW_PAYROLL_HDR_TEXT { get; set; }
        [XmlElement("Workdaysdtl")]
        public List<EmpWorkingDayDetails> EmpWorkingDaysDtl { get; set; }
    }


    [Serializable]
    [XmlRoot("Root")]
    public class FilterEmpDetails
    {
        public string EPH_PK { get; set; }
        public string EPH_FROM_DATE { get; set; }
        public string EPH_TO_DATE { get; set; }
        public string EMP_PK { get; set; }
        public string EMP_TYPE { get; set; }
        public string EMP_BRANCH { get; set; }
        public string EMP_PAYROLL_TYPE { get; set; }
        public string EPH_COMPANY { get; set; }
        public string empEmploymentType { get; set; }
        public string EPH_DEPT { get; set; }
        public string empDepartment { get; set; }
        public string empCompany { get; set; }
        public string EPH_CURRENCY { get; set; }

        public int EPH_IS_FINAL_SETTLEMENT { get; set; }


        public string EPH_RESIGNATION_DATE { get; set; }

        public string EPH_TASK1_BY { get; set; }
    }
    [XmlRoot("Root")]
    public class EmpPaySlipHeader
    {
        public int EPH_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<EmpPaySlipDetails> EmpPaySlipDetails { get; set; }
    }
    [Serializable]
    public class EmpPaySlipDetails
    {
        public string EPS_PK { get; set; }
    }

    [Serializable]
    [XmlRoot("Root")]
    public class PayrollPreprocessFilter
    {
        public string EMP_COMPANY { get; set; }
        public string EMP_PAYROLL_MONTH { get; set; }
        public string EMP_FROM_DATE { get; set; }
        public string EMP_TO_DATE { get; set; }
        public string EMP_PRC_MODE { get; set; }
        public string EMP_PAYROLL_TYPE { get; set; }
        public string EMP_TYPE { get; set; }
        public string EMP_EMPLOYMENT_TYPE { get; set; }
        public string EMP_DEPT { get; set; }
        public string EMP_BRANCH { get; set; }
        public int BIZUNIT_PK { get; set; }
        public string EMP_PK { get; set; }
    }
}
