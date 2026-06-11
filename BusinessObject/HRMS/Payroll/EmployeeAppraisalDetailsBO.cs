using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.HRMS.Payroll
{
   public class EmployeeAppraisalDetailsBO
    {
        [Serializable]
        [XmlRoot("Root")]
        public class EmployeeAppraisalHeader
        {
            [XmlElement("EIH_PK")]
            public int EIH_PK { get; set; }
            [XmlElement("EIH_DATE")]
            public DateTime EIH_DATE { get; set; }
            [XmlElement("EIH_EMPLOYEE")]
            public int EIH_EMPLOYEE { get; set; }
            [XmlElement("EIH_EMPLOYEE_TEXT")]
            public string EIH_EMPLOYEE_TEXT { get; set; }
            [XmlElement("EIH_NO")]
            public string EIH_NO { get; set; }
            [XmlElement("EIH_TRN_NAME")]
            public string EIH_TRN_NAME { get; set; }
            [XmlElement("EIH_TYPE")]
            public int EIH_TYPE { get; set; }
            [XmlElement("EIH_EFFECT_DATE")]
            public DateTime EIH_EFFECT_DATE { get; set; }
            [XmlElement("EIH_DEPT")]
            public int EIH_DEPT { get; set; }
            [XmlElement("EIH_COMPANY")]
            public int EIH_COMPANY { get; set; }
            [XmlElement("USER_PK")]
            public int USER_PK { get; set; }
            [XmlElement("BIZUNIT")]
            public int BIZUNIT { get; set; }
            [XmlElement("LAST_MOD_DT")]
            public DateTime LAST_MOD_DT { get; set; }
            [XmlElement("EIH_EMP_DESGN")]
            public string   EIH_EMP_DESGN { get; set; }
            [XmlElement("EIH_EMP_DESGN_TEXT")]
            public string EIH_EMP_DESGN_TEXT { get; set; }
            [XmlElement("EIH_EMP_DEPT")]
            public string EIH_EMP_DEPT { get; set; }
            [XmlElement("EIH_EMP_DEPT_TEXT")]
            public string EIH_EMP_DEPT_TEXT { get; set; }
            [XmlElement("EIH_DESC")]
            public string EIH_DESC { get; set; }
            [XmlElement("EIH_REF_NO")]
            public string EIH_REF_NO { get; set; }
            [XmlElement("EIH_PREV_APPR_DATE")]
            public string EIH_PREV_APPR_DATE { get; set; }
            [XmlElement("EIH_PREV_DESGN")]
            public string EIH_PREV_DESGN { get; set; }
            [XmlElement("EIH_PREV_DESGN_TEXT")]
            public string EIH_PREV_DESGN_TEXT { get; set; }
            [XmlElement("EIH_PREV_DEPT")]
            public string EIH_PREV_DEPT { get; set; }
            [XmlElement("EIH_PREV_DEPT_TEXT")]
            public string EIH_PREV_DEPT_TEXT { get; set; }
            [XmlElement("EIH_REF_DATE")]
            public string EIH_REF_DATE { get; set; }
            [XmlElement("AST_DOC_MODE")]
            public int AST_DOC_MODE { get; set; }
            [XmlElement("WKF_FLAG")]
            public int WKF_FLAG { get; set; }
            [XmlElement("ATL_ACTION")]
            public string ATL_ACTION { get; set; }
            [XmlElement("EIH_STATUS")]
            public int EIH_STATUS { get; set; }
            [XmlElement("PTM_PAYRL_START")]
            public int PTM_PAYRL_START { get; set; }
            
            
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

            [XmlElement("Details")]
            public List<BusinessObject.HRMS.Employee.EmpSalaryDetails> EmployeeAppraisalDtl { get; set; }
           // public List<EmployeeAppraisalDetails> EmployeeAppraisalDtl { get; set; }
        }

        [Serializable]
        public class EmployeeAppraisalDetails
        {
            [Serializable]
            public class EmpSalaryDetails
            {
                [XmlElement("EID_PK")]
                public DateTime EID_PK { get; set; }
                [XmlElement("EID_APPRAISAL_HDR")]
                public int EID_APPRAISAL_HDR { get; set; }
                [XmlElement("EID_PAY_ELEMENT")]
                public int EID_PAY_ELEMENT { get; set; }
                [XmlElement("EID_FORMULA")]
                public string EID_FORMULA { get; set; }
                [XmlElement("EID_REMARKS")]
                public string EID_REMARKS { get; set; }
                [XmlElement("EID_MIN_AMT")]
                public double EID_MIN_AMT { get; set; }
                [XmlElement("EID_MAX_AMT")]
                public double EID_MAX_AMT { get; set; }
                [XmlElement("EID_OLD_VALUE")]
                public double EID_OLD_VALUE { get; set; }
                [XmlElement("EID_NEW_VALUE")]
                public double EID_NEW_VALUE { get; set; }
                [XmlElement("EID_PAY_ELEMENT_DTL")]
                public int EID_PAY_ELEMENT_DTL { get; set; }
                [XmlElement("EID_IS_DELETE")]
                public int EID_IS_DELETE { get; set; }
                [XmlElement("EID_SL_NO")]
                public string EID_SL_NO { get; set; }


            }
        }
    }
   
}
