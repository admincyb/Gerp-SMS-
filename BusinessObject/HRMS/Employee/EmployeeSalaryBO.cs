using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Employee
{
    public class EmployeeSalaryBO
    {
    }

    [Serializable]
    [XmlRoot("Root")]
    public class EmpTemplateHeader
    {
        [XmlElement("empPK")]
        public int EMP_PK { get; set; }
        [XmlElement("empSalaryTemp")]
        public int EMP_SALARY_TEMP { get; set; }
        [XmlElement("empBasicPay")]
        public double EMP_BASIC_PAY { get; set; } 
        [XmlElement("empDesignationText")]
        public string EMP_DESIGNATION_TEXT { get; set; }
        [XmlElement("empDept_Text")]
        public string EMP_DEPT_TEXT { get; set; }
        [XmlElement("empEffectiveFrom")]
        public DateTime EMP_EFFECTIVE_FROM { get; set; }
        [XmlElement("empPayRollType")]
        public int EMP_PAYROLL_TYPE { get; set; }
        [XmlElement("empNetSalary")]
        public double empNetSalary { get; set; }
        [XmlElement("empCTC")]
        public double empCTC { get; set; }
        [XmlElement("empGrossSalary")]
        public double empGrossSalary { get; set; }
        [XmlElement("PTM_PAYRL_START")]
        public int PTM_PAYRL_START { get; set; }
        

        [XmlElement("empDesignation")]
        public int empDesignation { get; set; }
        [XmlElement("empDept")]
        public int empDept { get; set; }
        [XmlElement("EIH_PREV_APPR_DATE")]
        public string EIH_PREV_APPR_DATE { get; set; }

        
        public int STE_PK { get; set; }
        public string STE_CODE { get; set; }
        public string STE_NAME { get; set; }
        public byte STE_ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        [XmlElement("Detail")]
        public List<EmpSalaryDetails> SalaryDtl { get; set; }
    }

    [Serializable]
    public class EmpSalaryDetails
    {
        public int EDP_PK { get; set; }
        public int STS_PAY_ELEMENT { get; set; }
        public string STS_PAY_ELEMENT_TEXT { get; set; }        
        public string STS_FORMULA_CODE { get; set; }
        //public string STS_CALC_VALUE_TEXT { get; set; }//public string STS_FORMULA_TEXT { get; set; } 
        public string STS_VALUE_TEXT { get; set; }
        public string STS_CALC_VALUE { get; set; }
        public double STS_VALUE { get; set; }
        public double STS_VALUE_OLD { get; set; }
        public int STS_CALC_MODE { get; set; }
        public byte PEL_IS_DEDUCTION { get; set; }
        public int STS_SL_NO { get; set; }
        public int STS_PK { get; set; }
        public int STS_HAS_PAYROLL { get; set; }
        public byte PEL_IN_SALARY { get; set; }
        public decimal STS_MIN_AMT { get; set; }
        public decimal STS_MAX_AMT { get; set; }
        public int PEL_IS_FORMULA_EDITABLE { get; set; }
        public byte PEL_IN_CTC { get; set; }
        public byte PEL_IN_GROSS { get; set; }

        public byte PEL_SHOW_IN_EMPMAST { get; set; }
        public int EID_IS_DELETE { get; set; }
        public int EID_PK { get; set; }
        public int EID_APPRAISAL_HDR { get; set; }
    }
}
