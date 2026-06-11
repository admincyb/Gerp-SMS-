using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
    public class BulkAppraisalBO
    {

        [Serializable]
        [XmlRoot("Root")]
        public class SalaryBulkAppraisalHeader
        {
            [XmlElement("EBH_PK")]
            public int EBH_PK { get; set; }
            [XmlElement("EBH_NO")]
            public string EBH_NO { get; set; }
            [XmlElement("EBH_DATE")]
            public DateTime EBH_DATE { get; set; }
            [XmlElement("EBH_EFFECT_DATE")]
            public DateTime EBH_EFFECT_DATE { get; set; }
            [XmlElement("EBH_TYPE")]
            public int EBH_TYPE { get; set; }

            [XmlElement("EBH_DEPT")]
            public int EBH_DEPT { get; set; }
            [XmlElement("EBH_COMPANY")]
            public int EBH_COMPANY { get; set; }
            [XmlElement("EBH_BIZUNIT")]
            public int EBH_BIZUNIT { get; set; }
            [XmlElement("EBH_REF_NO")]
            public string EBH_REF_NO { get; set; }
            [XmlElement("EBH_REF_DATE")]
            public string EBH_REF_DATE { get; set; }
            [XmlElement("EBH_REMARKS")]
            public string EBH_REMARKS { get; set; }
            [XmlElement("EBH_DESC")]
            public string EBH_DESC { get; set; }
            [XmlElement("EBH_DEL_STATUS")]
            public int EBH_DEL_STATUS { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }

            [XmlElement("EBH_EFCT_IS_FLAG")]
            public int EBH_EFCT_IS_FLAG { get; set; }

            [XmlElement("WKF_FLAG")]
            public int WKF_FLAG { get; set; }
            [XmlElement("EIH_STATUS")]
            public int EIH_STATUS { get; set; }

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

            [XmlElement("EmpDetails")]
            public List<SalaryBulkEmpDetails> AppraisalEmpDetails { get; set; }
            [XmlElement("ElementDetails")]
            public List<SalaryBulkPayAppraisalDetails> AppraisalPayDetails { get; set; }
        }

        [Serializable]
        public class SalaryBulkEmpDetails
        {
            [XmlElement("BED_PK")]
            public int BED_PK { get; set; }
            [XmlElement("BED_EBH_PK")]
            public int BED_EBH_PK { get; set; }
            [XmlElement("BED_EMPLOYEE")]
            public int BED_EMPLOYEE { get; set; }
            [XmlElement("BED_EMPLOYEE_TEXT")]
            public string BED_EMPLOYEE_TEXT { get; set; }
            [XmlElement("BED_EMP_DEPT")]
            public int BED_EMP_DEPT { get; set; }
            [XmlElement("BED_EMP_DEPT_TEXT")]
            public string BED_EMP_DEPT_TEXT { get; set; }
            [XmlElement("BED_EMP_DESGN")]
            public int BED_EMP_DESGN { get; set; }
            [XmlElement("BED_EMP_DESGN_TEXT")]
            public string BED_EMP_DESGN_TEXT { get; set; }
            [XmlElement("BED_EMP_PREV_APPR_DATE")]
            public string BED_EMP_PREV_APPR_DATE { get; set; }


            public string BED_EMP_DOJ { get; set; }
            //public int BED_EMP_BRANCH { get; set; }
            public string BED_EMP_BRANCH_TEXT { get; set; }
            public int BED_EMP_BIZUNIT { get; set; }
        }

        [Serializable]
        public class SalaryBulkPayAppraisalDetails
        {
            [XmlElement("EBD_PK")]
            public int EBD_PK { get; set; }
            [XmlElement("EBD_PAY_ELEMENT")]
            public int EBD_PAY_ELEMENT { get; set; }
            [XmlElement("EBD_EBH_PK")]
            public int EBD_EBH_PK { get; set; }

            [XmlElement("EBD_CALC_MODE")]
            public int EBD_CALC_MODE { get; set; }


            [XmlElement("EBD_PAY_ELEMENT_TEXT")]
            public string EBD_PAY_ELEMENT_TEXT { get; set; }
            [XmlElement("EBD_CALC_MODE_TEXT")]
            public string EBD_CALC_MODE_TEXT { get; set; }
            [XmlElement("EBD_TYPE")]
            public string EBD_TYPE { get; set; }
            [XmlElement("EBD_TYPE_TEXT")]
            public string EBD_TYPE_TEXT { get; set; }

            [XmlElement("EBD_INC_DECR")]
            public string EBD_INC_DECR { get; set; }
            [XmlElement("EBD_INC_DECR_TEXT")]
            public string EBD_INC_DECR_TEXT { get; set; }

            [XmlElement("EBD_VALUE")]
            public double EBD_VALUE { get; set; }

            [XmlElement("EBD_DEL_STATUS")]
            public int EBD_DEL_STATUS { get; set; }
            [XmlElement("EBD_IS_NEW")]
            public int EBD_IS_NEW { get; set; }


            public string PEL_NAME { get; set; }
            public int PEL_IS_DEDUCTION { get; set; }
            public int PEL_IN_SALARY { get; set; }
            public string STS_FORMULA_CODE { get; set; }
            public int STS_SL_NO { get; set; }
        }



        [Serializable]
        [XmlRoot("Root")]
        public class EmpAppHeader
        {
            public int empBizUnit { get; set; }
            [XmlElement("EmpDetails")]
            public List<SalaryBulkEmpDetails> EmpInfoDtl { get; set; }
           [XmlElement("ElementDetails")]
            public List<SalaryBulkPayAppraisalDetails> EmpPayElementDtl { get; set; }
        }
    }



  
}
