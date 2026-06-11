using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Admin.Masters
{
    public class Procedures
    {
        #region Leave type
        public const string SPHRM_LEAVE_TYPE_MST_SAVE = "SPHRM_LEAVE_TYPE_MST_SAVE";
        public const string SPHRM_LEAVE_TYPE_MST_GET_KV = "SPHRM_LEAVE_TYPE_MST_GET_KV";
        public const string SPHRM_LEAVE_TYPE_MST_DELETE = "SPHRM_LEAVE_TYPE_MST_DELETE";
        public const string SPHRM_LEAVE_TYPE_MST_ACTIVATE = "SPHRM_LEAVE_TYPE_MST_ACTIVATE";
        #endregion
        #region Employee Leave Master
        public const string SPHRM_EMP_OB_LEAVE_DELETE = "SPHRM_EMP_OB_LEAVE_DELETE";
        public const string SPHRM_EMP_OB_LEAVE_GET_XML = "SPHRM_EMP_OB_LEAVE_GET_XML";
        public const string SPHRM_EMP_OB_LEAVE_SAVE = "SPHRM_EMP_OB_LEAVE_SAVE";
        public const string SPHRM_EMP_OB_LEAVE_GET_LIST = "SPHRM_EMP_OB_LEAVE_GET_LIST";
        public const string SPHRM_EMPLOYEE_LEAVE_TYPE_GET = "SPHRM_EMPLOYEE_LEAVE_TYPE_GET";
        public const string SPHRM_OB_LEAVE_EMPLOYEE_GET = "SPHRM_OB_LEAVE_EMPLOYEE_GET";
        #endregion

        public const string SPADM_CONFIG_MST_GET_KV = "SPADM_CONFIG_MST_GET_KV";
        #region Employee Type
        public const string SPHRM_EMP_TYPE_GET_XML = "SPHRM_EMP_TYPE_GET_XML";
        public const string SPHRM_EMP_TYPE_SAVE = "SPHRM_EMP_TYPE_SAVE";
        public const string SPHRM_EMP_TYPE_GET_LIST = "SPHRM_EMP_TYPE_GET_LIST";
        public const string SPHRM_EMP_TYPE_DELETE = "SPHRM_EMP_TYPE_DELETE";
        public const string SPHRM_EMP_TYPE_GET_KV = "SPHRM_EMP_TYPE_GET_KV";
        public const string SPHRM_EMP_TYPE_MST_ACTIVATE = "SPHRM_EMP_TYPE_MST_ACTIVATE";
        #endregion
        public const string SPHRM_LEAVE_TEMP_GET_KV = "SPHRM_LEAVE_TEMP_GET_KV";
        public const string SPHRM_OT_TEMP_GET_KV = "SPHRM_OT_TEMP_GET_KV";
        #region PayElements Master
        public const string SPHRM_PAY_ELEMENT_GET_KV = "SPHRM_PAY_ELEMENT_GET_KV";
        public const string SPHRM_PAY_ELEMENT_MST_SAVE = "SPHRM_PAY_ELEMENT_MST_SAVE";
        public const string SPHRM_PAY_ELEMENT_DELETE = "SPHRM_PAY_ELEMENT_DELETE";
        public const string SPHRM_PAY_ELEMENT_GET_LIST = "SPHRM_PAY_ELEMENT_GET_LIST";
        public const string SPHRM_PAY_ELEMENT_ATUO = "SPHRM_PAY_ELEMENT_ATUO";
        public const string SPHRM_PAY_ELEMENT_MST_ACTIVATE = "SPHRM_PAY_ELEMENT_MST_ACTIVATE";
        public const string SPHRM_PAY_ELEMENT_VALUE_PK_GET = "SPHRM_PAY_ELEMENT_VALUE_PK_GET";
        public const string SPHRM_FORMULA_ELEMENT_GET_KV = "SPHRM_FORMULA_ELEMENT_GET_KV";
        #endregion
        public const string SPHRM_SALARY_TEMP_GET_KV = "SPHRM_SALARY_TEMP_GET_KV";
        public const string SPHRM_EMP_TYPE_WORK_HRS_GET = "SPHRM_EMP_TYPE_WORK_HRS_GET";
        // Salary Template
        public const string SPHRM_SALARY_TEMP_SAVE = "SPHRM_SALARY_TEMP_SAVE";
        public const string SPHRM_SALARY_TEMP_GET_LIST = "SPHRM_SALARY_TEMP_GET_LIST";
        public const string SPHRM_SALARY_TEMP_GET_XML = "SPHRM_SALARY_TEMP_GET_XML";
        public const string SPHRM_SALARY_TEMP_DELETE = "SPHRM_SALARY_TEMP_DELETE";
        public const string SPHRM_SALARY_TEMP_ACTIVATE = "SPHRM_SALARY_TEMP_ACTIVATE";
        public const string SPHRM_SALARY_TEMP_DTL_EMP_GET = "SPHRM_SALARY_TEMP_DTL_EMP_GET";
        public const string SPHRM_SALARY_TEMP_DTL_UPDATE = "SPHRM_SALARY_TEMP_DTL_UPDATE";  
        #region Leave Template
        public const string SPHRM_LEAVE_TEMP_SAVE = "SPHRM_LEAVE_TEMP_SAVE";
        public const string SPHRM_LEAVE_TEMP_GET_LIST = "SPHRM_LEAVE_TEMP_GET_LIST";
        public const string SPHRM_LEAVE_TEMP_DELETE = "SPHRM_LEAVE_TEMP_DELETE";
        public const string SPHRM_LEAVE_TEMP_GET_XML = "SPHRM_LEAVE_TEMP_GET_XML";
        public const string SPHRM_LEAVE_TEMP_ACTIVATE = "SPHRM_LEAVE_TEMP_ACTIVATE";
        #endregion
        #region OT Template
        public const string SPHRM_OT_TEMP_SAVE = "SPHRM_OT_TEMP_SAVE";
        public const string SPHRM_OT_TEMP_GET_LIST = "SPHRM_OT_TEMP_GET_LIST";
        public const string SPHRM_OT_TEMP_GET_XML = "SPHRM_OT_TEMP_GET_XML";
        public const string SPHRM_OT_TEMP_DELETE = "SPHRM_OT_TEMP_DELETE";
        public const string SPHRM_OT_TEMP_ACTIVATE="SPHRM_OT_TEMP_ACTIVATE";
        #endregion
        #region Payroll Type Master
        public const string SPHRM_PAYROLL_TYPE_MST_ACTIVATE="SPHRM_PAYROLL_TYPE_MST_ACTIVATE";
        public const string SPHRM_PAYROLL_TYPE_GET_KV = "SPHRM_PAYROLL_TYPE_GET_KV";
        public const string SPHRM_PAYROLL_TYPE_MST_SAVE = "SPHRM_PAYROLL_TYPE_MST_SAVE";
        public const string SPHRM_PAYROLL_TYPE_MST_DELETE = "SPHRM_PAYROLL_TYPE_MST_DELETE";
        public const string SPHRM_PAYROLL_TYPE_GET_XML = "SPHRM_PAYROLL_TYPE_GET_XML";
        #endregion        
        #region Slab Definitions
        public const string SPHRM_PAY_ELEMENT_SLAB_HDR_SAVE = "SPHRM_PAY_ELEMENT_SLAB_HDR_SAVE";
        public const string SPHRM_PAY_ELEMENT_SLAB_GET_LIST = "SPHRM_PAY_ELEMENT_SLAB_GET_LIST";
        public const string SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV = "SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV";
        public const string SPHRM_PAY_ELEMENT_SLAB_DELETE = "SPHRM_PAY_ELEMENT_SLAB_DELETE";
        public const string SPHRM_PAY_ELEMENT_SLAB_ACTIVATE = "SPHRM_PAY_ELEMENT_SLAB_ACTIVATE";
        #endregion

        #region Holiday Master
        public const string SPHRM_HOLIDAY_HDR_SAVE = "SPHRM_HOLIDAY_HDR_SAVE";
        public const string SPHRM_HOLIDAY_GET_KV = "SPHRM_HOLIDAY_GET_KV";
        public const string SPHRM_HOLIDAY_GET_LIST = "SPHRM_HOLIDAY_GET_LIST";
        public const string SPHRM_HOLIDAY_OUTPUT_RPT = "SPHRM_HOLIDAY_OUTPUT_RPT";
        public const string SPHRM_HOLIDAY_DELETE = "SPHRM_HOLIDAY_DELETE";
        public const string SPHRM_HOLIDAY_TYPE_GET = "SPHRM_HOLIDAY_TYPE_GET";
        #endregion

        #region Designation Master
        public const string SPHRM_EmpDesignation_GET_KV = "SPHRM_EmpDesignation_GET_KV";
        public const string SPHRM_EmpDesignation_SAVE = "SPHRM_EmpDesignation_SAVE";
        public const string SPHRM_EmpDesignation_DELETE = "SPHRM_EmpDesignation_DELETE";
        public const string SPHRM_EmpDesignation_ACTIVATE = "SPHRM_EmpDesignation_ACTIVATE";
        public const string SPHRM_EmpDesignation_GET_XML = "SPHRM_EmpDesignation_GET_XML";    
        #endregion

        #region Bonus Type Master
        public const string SPHRM_BONUS_TYPE_SAVE = "SPHRM_BONUS_TYPE_SAVE";
        public const string SPHRM_BONUS_TYPE_GET_KV = "SPHRM_BONUS_TYPE_GET_KV";
        public const string SPHRM_BONUS_TYPE_DELETE = "SPHRM_BONUS_TYPE_DELETE";
        public const string SPHRM_BONUS_TYPE_ACTIVATE = "SPHRM_BONUS_TYPE_ACTIVATE";
        #endregion

        #region Salary Report Template
        public const string SPHRM_SAL_RPT_TMP_HDR_SAVE = "SPHRM_SAL_RPT_TMP_HDR_SAVE";
        public const string SPHRM_SAL_RPT_TMP_HDR_GET_KV = "SPHRM_SAL_RPT_TMP_HDR_GET_LIST";
        public const string SPHRM_SAL_RPT_TMP_HDR_GET_XML = "SPHRM_SAL_RPT_TMP_HDR_GET_XML";
        public const string SPHRM_SAL_RPT_TMP_HDR_DELETE = "SPHRM_SAL_RPT_TMP_HDR_DELETE";
        #endregion

    }
}
