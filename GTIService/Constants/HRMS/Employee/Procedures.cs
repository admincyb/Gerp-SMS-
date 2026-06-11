using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Employee
{
    public class Procedures
    {
        public const string SAVE_EMPLOYEEBASICINFO = "SPHRM_EMP_EMPLOYEE_SAVE";
        public const string GET_EMPDOCLIST = "SPHRM_EMP_DOC_DTL_GET_LIST";
        public const string GET_AUTOCOMPLETENATINALITY = "SPADM_COUNTRY_MST_GET_KV";
        public const string GET_EMPLOYEE_DESIGNATION = "SPEmpDesignationGetKV";
        public const string GET_CURRENCY = "SPADM_CURRENCY_MST_GET_KV";
        public const string GET_BASE_CURRENCY = "SPADM_APP_CONFIG_MST_GET";
        public const string GET_COMMONDROPDOWNLIST = "SPHRM_CONST_MST_GET_KV";
        public const string GET_AUTOCOMPLETEMPLOYEE = "SpWkfEmployeeMstGetKV";
        public const string DELETE_EMPLOYEE = "SPHRM_EMP_EMPLOYEE_DELETE";
        public const string SPADM_STATE_MST_AUTO = "SPADM_STATE_MST_AUTO";
        public const string SPHRM_PAYROLL_EMP_GET = "SPHRM_PAYROLL_EMP_GET";
        public const string GET_EMPLOYEE_DESIGNATIONBYJOB = "SPEmpDesignationJobLevelGetKV";
        public const string SPHRM_EMP_ATTENDANCE_AUTO = "SPHRM_EMP_ATTENDANCE_AUTO";

        //Employee List
        public const string GET_EmployeeList = "SPHRM_EMP_EMPLOYEE_GET_LIST";
        public const string GET_EmployeeBYID = "SPHRM_EMP_EMPLOYEE_GET";
        public const string GET_EmployeeHeaderList = "SPHRM_EMP_EMPLOYEE_GET_KV";

        public const string GET_EMPLOYEE_DOC_DETAILS = "SPHRM_EMP_DOC_DTL_GET";
        public const string SAVE_EMPLOYEE_DOC = "SPHRM_EMP_DOC_DTL_SAVE";
        public const string GET_CONFIG_MST_GET_KV = "SPADM_CONFIG_MST_GET_KV";

        public const string GET_EMPDOCLOGLIST = "SPHRM_EMP_DOC_TRX_DTL_GET_LIST";
        public const string SPHRM_EMP_STATUS_SAVE = "SPHRM_EMP_STATUS_SAVE";
        public const string SPHRM_EMP_STATUS_GET_KV = "SPHRM_EMP_STATUS_GET_KV";
        public const string SPHRM_EMP_STATUS_GET = "SPHRM_EMP_STATUS_GET";
        public const string SPHRM_EMP_BRANCH_LOG_SAVE = "SPHRM_EMP_BRANCH_LOG_SAVE";
        public const string SPHRM_EMP_BRANCH_GET = "SPHRM_EMP_BRANCH_GET";
        public const string SPHRM_EMP_DOC_DTL_DELETE = "SPHRM_EMP_DOC_DTL_DELETE";
        public const string SPHRM_EMP_DEPT_LOG_GET = "SPHRM_EMP_DEPT_LOG_GET";
        public const string SPHRM_EMP_DEPT_LOG_SAVE = "SPHRM_EMP_DEPT_LOG_SAVE";
        public const string SPHRM_EMP_DESIG_LOG_SAVE = "SPHRM_EMP_DESIG_LOG_SAVE";
        public const string SPHRM_EMP_DESIG_LOG_GET = "SPHRM_EMP_DESIG_LOG_GET";

        //Employee Qualification
        public const string SAVE_EMPLOYEE_QUALIFICATION = "SPHRM_EMP_QUALIFICATION_DTL_SAVE";
        public const string GET_EMPLOYEE_QUALIFICATION_LIST = "SPHRM_EMP_QUALIFICATION_DTL_GET_LIST";
        public const string GET_EMPLOYEE_QUALIFICATION_LIST_BY_QUALIFICATIONID = "SPHRM_EMP_QUALIFICATION_DTL_GET";
        public const string DELETE_EMPLOYEE_QUALIFICATION_QUALIFICATIONID = "SPHRM_EMP_QUALIFICATION_DTL_DELETE";

        //Employee Experience
        public const string SAVE_EMPLOYEE_EXPERIENCE = "SPHRM_EMP_EXPERIENCE_DTL_SAVE";
        public const string GET_EMPLOYEE_EXPERIENCE_LIST = "SPHRM_EMP_EXPERIENCE_DTL_GET_LIST";
        public const string GET_EMPLOYEE_EXPERIENCE_LIST_EXPERIENCE_ID = "SPHRM_EMP_EXPERIENCE_DTL_GET";
        public const string DELETE_EMPLOYEE_EXPERIENNCE_BYEXPID = "SPHRM_EMP_EXPERIENCE_DTL_DELETE";

        public const string SHHRM_EMP_DOC_DTL_GET_LIST = "SPHRM_EMP_DOC_DTL_GET_LIST";
        public const string SPHRM_EMP_DOC_TRX_DTL_SAVE = "SPHRM_EMP_DOC_TRX_DTL_SAVE";    
    
        //Employee Skills
        public const string GET_SKILL_CATEGORY = "SPHRM_EMP_SKILL_CATEGORY_GET";  //Get Skills Category
        public const string GET_SKILLS_BY_CATEGORYPK = "SPHRM_EMP_SKILL_DTL_GET";  //Get Skills Details By Category Id
        public const string GET_SKILL_LEVELS = "SPADM_CONFIG_MST_GET_KV";  //Get Skills Levels
        public const string SAVE_EMPLOYEE_SKILL = "SPHRM_EMP_SKILL_DTL_SAVE";  //Get Skills Levels

        //Pay Details        
        public const string SPHRM_EMP_PAY_DTL_GET_XML = "SPHRM_EMP_PAY_DTL_GET_XML";  
        public const string SPHRM_EMP_PAY_DTL_SAVE = "SPHRM_EMP_PAY_DTL_SAVE";
        public const string SPHRM_EMP_PAY_DTL_DELETE = "SPHRM_EMP_PAY_DTL_DELETE";
        public const string SPFIN_CASH_BANK_MST_GET_KV = "SPFIN_CASH_BANK_MST_GET_KV";
        public const string SPHRM_PAYROLL_TYPE_USER_MAP_GET = "SPHRM_PAYROLL_TYPE_USER_MAP_GET";

        //Salary Details
        public const string SPHRM_EMP_PAY_ELEMENT_DTL_GET_XML = "SPHRM_EMP_PAY_ELEMENT_DTL_GET_XML";
        public const string SPHRM_EMP_PAY_ELEMENT_DTL_SAVE = "SPHRM_EMP_PAY_ELEMENT_DTL_SAVE";
        public const string SPHRM_PAYROLL_TYPE_GET_KV = "SPHRM_PAYROLL_TYPE_GET_KV";
        public const string SPHRM_EMP_SALARY_DELETE = "SPHRM_EMP_SALARY_DELETE";
        //public const string 

        //Salary Revision
        // public const string SPHRM_EMP_SALARY_HISTORY_GET = "SPHRM_EMP_SALARY_HISTORY_GET";
        public const string SPHRM_EMP_PAY_ELEMENT_HISTORY_GET = "SPHRM_EMP_PAY_ELEMENT_HISTORY_GET";

        //Employee Leave Type
        public const string SPHRM_EMP_LEAVE_TYPE_DTL_GET_KV = "SPHRM_EMP_LEAVE_TYPE_DTL_GET_KV";
        public const string SPHRM_EMP_LEAVE_TYPE_DTL_SAVE = "SPHRM_EMP_LEAVE_TYPE_DTL_SAVE";
        public const string SPHRM_EMP_LEAVE_CREDIT_GET = "SPHRM_EMP_LEAVE_CREDIT_GET";

        //Department with code Autocomplete
        public const string SPHRM_EMP_DEPT_GET_KV = "SPHRM_EMP_DEPT_GET_KV";

        // Employee Performance
        public const string SPHRM_EMP_PERFORMANCE_DTL_SAVE="SPHRM_EMP_PERFORMANCE_DTL_SAVE";
        public const string SPADM_CONST_GRP_GET_KV="SPADM_CONST_GRP_GET_KV";
        public const string SPHRM_CONST_MST_GET_KV = "SPHRM_CONST_MST_GET_KV";
        public const string SPHRM_EMP_PERFORMANCE_DTL_GET_LIST = "SPHRM_EMP_PERFORMANCE_DTL_GET_LIST";
        public const string SPHRM_EMP_PERFORMANCE_DTL_GET="SPHRM_EMP_PERFORMANCE_DTL_GET";
        public const string SPHRM_EMP_PERFORMANCE_DTL_DELETE = "SPHRM_EMP_PERFORMANCE_DTL_DELETE";


        //Employee Training
        public const string SPHRM_EMP_TRAINING_GET_LIST="SPHRM_EMP_TRAINING_GET_LIST";
        public const string SPHRM_EMP_TRAINING_GET_XML="SPHRM_EMP_TRAINING_GET_XML";
        public const string SPHRM_EMP_TRAINING_SAVE="SPHRM_EMP_TRAINING_SAVE";
        public const string SPHRM_EMP_TRAINING_DELETE="SPHRM_EMP_TRAINING_DELETE";
        public const string SPHRM_EMP_TRAINING_DTL_GET = "SPHRM_EMP_TRAINING_DTL_GET";
        public const string SPHRM_EMP_TRAINING_EMP_GET = "SPHRM_EMP_TRAINING_EMP_GET";
        public const string SPHRM_EMP_TRAINING_OUTPUT_RPT = "SPHRM_EMP_TRAINING_OUTPUT_RPT";
        public const string SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_GET = "SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_GET";
        public const string SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_SAVE = "SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_SAVE";

        //Employee Transfer
        public const string SPHRM_EMP_TRANSFER_WKF_SAVE = "SPHRM_EMP_TRANSFER_WKF_SAVE";
        public const string SPHRM_EMP_TRANSFER_GET_LIST = "SPHRM_EMP_TRANSFER_GET_LIST";
        public const string SPHRM_EMP_TRANSFER_GET_XML = "SPHRM_EMP_TRANSFER_GET_XML";
        public const string SPHRM_EMP_TRANSFER_DELETE = "SPHRM_EMP_TRANSFER_DELETE";
        public const string SPHRM_EMP_TRANSFER_AUTO = "SPHRM_EMP_TRANSFER_AUTO";
        public const string SPHRM_EMP_TRANSFER_OUTPUT_RPT = "SPHRM_EMP_TRANSFER_OUTPUT_RPT";
    }
}
