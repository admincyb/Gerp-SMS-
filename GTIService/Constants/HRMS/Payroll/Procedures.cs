using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Payroll
{
    public class Procedures
    {
        public const string SPHRM_EMP_ATTENDANCE_DTL_GET = "SPHRM_EMP_ATTENDANCE_DTL_GET";
        public const string SPHRM_EMP_ATTENDANCE_DTL_SAVE = "SPHRM_EMP_ATTENDANCE_DTL_SAVE";
        public const string SPHRM_EMP_ATTENDANCE_IMPORT_SAVE = "SPHRM_EMP_ATTENDANCE_IMPORT_SAVE";
        public const string SPHRM_EMP_WORK_HR_GET = "SPHRM_EMP_WORK_HR_GET";
        public const string SPHRM_FORMULA_VALIDATE = "SPHRM_FORMULA_VALIDATE";
        public const string SPHRM_EMP_ATTENDANCE_DELETE = "SPHRM_EMP_ATTENDANCE_DELETE";
        public const string SPHRM_EMP_ATTENDANCE_GET_LIST = "SPHRM_EMP_ATTENDANCE_GET_LIST";
        public const string SPHRM_EMP_ATTENDANCE_DTL_GET_XML = "SPHRM_EMP_ATTENDANCE_DTL_GET_XML";
        public const string SPHRM_EMP_ATTENDANCE_GET_XML = "SPHRM_EMP_ATTENDANCE_GET_XML";
        public const string SPHRM_EMP_ATTENDANCE_GEN_SAVE = "SPHRM_EMP_ATTENDANCE_GEN_SAVE";
        public const string SPHRM_EMP_ATTENDANCE_GEN_GET_XML = "SPHRM_EMP_ATTENDANCE_GEN_GET_XML";


        // Monthly Leave
        public const string SPHRM_EMP_EMP_LEAVE_DTL_GET_XML = "SPHRM_EMP_EMP_LEAVE_DTL_GET_XML";
        public const string SPHRM_EMP_LEAVE_DTL_SAVE = "SPHRM_EMP_LEAVE_DTL_SAVE";
        public const string SPHRM_EMP_LEAVE_DTL_GET_LIST = "SPHRM_EMP_LEAVE_DTL_GET_LIST";
        public const string SPHRM_YEAR_MST_GET_KV = "SPHRM_YEAR_MST_GET_KV";
        public const string SPHRM_EMP_LEAVE_HDR_GET_LIST = "SPHRM_EMP_LEAVE_HDR_GET_LIST";
        public const string SPHRM_EMP_LEAVE_DTL_GET_KV = "SPHRM_EMP_LEAVE_DTL_GET_KV";
        public const string SPHRM_EMP_LEAVE_DELETE = "SPHRM_EMP_LEAVE_DELETE";
        public const string SPHRM_EMP_LEAVE_DTL_DELETE = "SPHRM_EMP_LEAVE_DTL_DELETE";

        public const string SPHRM_EMP_LEAVE_SAVE = "SPHRM_EMP_LEAVE_SAVE";
        public const string SPHRM_EMP_LEAVE_DAY_DTL_DELETE = "SPHRM_EMP_LEAVE_DAY_DTL_DELETE";

        //Overtime Calculator
        public const string SPHRM_EMP_OT_DTL_GET_XML = "SPHRM_EMP_OT_DTL_GET_XML";
        public const string SPHRM_EMP_OT_DTL_SAVE = "SPHRM_EMP_OT_DTL_SAVE";
        public const string SPHRM_EMP_OT_DTL_GET_LIST = "SPHRM_EMP_OT_DTL_GET_LIST";
        public const string SPHRM_EMP_OT_DTL_GET = "SPHRM_EMP_OT_DTL_GET";
        public const string SPHRM_EMP_OT_HDR_DELETE = "SPHRM_EMP_OT_HDR_DELETE";
        public const string SPHRM_EMP_OT_HDR_AUTO = "SPHRM_EMP_OT_HDR_AUTO";

        //LoansAndAdvances
        public const string SPHRM_EMP_LOAN_GET_LIST = "SPHRM_EMP_LOAN_GET_LIST";
        public const string SPHRM_LOAN_TYPE_GET_KV = "SPHRM_LOAN_TYPE_GET_KV";
        public const string SPHRM_EMP_LOAN_SAVE = "SPHRM_EMP_LOAN_SAVE";
        public const string SPHRM_EMP_LOAN_GET_XML = "SPHRM_EMP_LOAN_GET_XML";
        public const string SPHRM_EMP_LOAN_HISTORY_GET = "SPHRM_EMP_LOAN_HISTORY_GET";
        public const string SPHRM_EMP_LOAN_DELETE = "SPHRM_EMP_LOAN_DELETE";

        //AppraisalDetails
        public const string SPHRM_EMP_APPRAISAL_GET_LIST = "SPHRM_EMP_APPRAISAL_GET_LIST";
        public const string SPHRM_EMP_APPRAISAL_GET_XML = "SPHRM_EMP_APPRAISAL_GET_XML";
        public const string SPHRM_EMP_APPRAISAL_SAVE = "SPHRM_EMP_APPRAISAL_SAVE";
        public const string SPHRM_EMP_APPRAISAL_DELETE = "SPHRM_EMP_APPRAISAL_DELETE";

        //Payroll Process
        public const string SPHRM_EMP_PAYROLL_DTL_GET_XML = "SPHRM_EMP_PAYROLL_DTL_GET_XML";
        public const string SPHRM_EMP_PAYROLL_SAVE = "SPHRM_EMP_PAYROLL_SAVE";
        public const string SPHRM_EMP_PAYROLL_GET_LIST = "SPHRM_EMP_PAYROLL_GET_LIST";
        public const string SPHRM_EMP_PAYROLL_PAY_DTL_GET_XML = "SPHRM_EMP_PAYROLL_PAY_DTL_GET_XML";
        public const string SPHRM_EMP_PAYROLL_PAY_DTL_SAVE = "SPHRM_EMP_PAYROLL_PAY_DTL_SAVE";
        public const string SPHRM_PAYROLL_TYPE_GET_KV = "SPHRM_PAYROLL_TYPE_GET_KV";
        public const string SPHRM_EMP_PAYROLL_WD_DTL_GET_KV = "SPHRM_EMP_PAYROLL_WD_DTL_GET_KV";
        public const string SPHRM_EMP_PAYROLL_HDR_DELETE = "SPHRM_EMP_PAYROLL_HDR_DELETE";
        public const string SPHRM_EMP_PAYROLL_DTL_DELETE = "SPHRM_EMP_PAYROLL_DTL_DELETE";
        public const string SPHRM_EMP_SALARY_SLIP_OUTPUT_RPT = "SPHRM_EMP_SALARY_SLIP_OUTPUT_RPT";
        public const string SPHRM_EMP_SAL_STMNT_OUTPUT_RPT = "SPHRM_EMP_SAL_STMNT_OUTPUT_RPT";
        public const string SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT = "SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT";
        public const string SPHRM_EMP_ATTENDANCE_IMPORT_OUTPUT_RPT = "SPHRM_EMP_ATTENDANCE_IMPORT_OUTPUT_RPT";
        public const string SPHRM_EMP_ATTENDANCE_OUTPUT_RPT = "SPHRM_EMP_ATTENDANCE_OUTPUT_RPT";
        public const string SPHRM_SAL_PRE_PROCESS_GET = "SPHRM_SAL_PRE_PROCESS_GET";
        public const string SPHRM_EMP_SAL_OUTPUT_RPT = "SPHRM_EMP_SAL_OUTPUT_RPT";
        public const string SPHRM_EMP_PAYROLL_GET_KV = "SPHRM_EMP_PAYROLL_GET_KV";
        public const string SPHRM_PAYROLL_PEND_EMP_GET = "SPHRM_PAYROLL_PEND_EMP_GET";
        public const string SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT_IGCL = "SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT_IGCL";
        public const string SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT_JTME = "SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT_JTME";

        // IncomeTax_01
        public const string SPHRM_HRM_INCOME_TAX_SAVE = "SPHRM_HRM_INCOME_TAX_SAVE";
        public const string SPHRM_INCOME_TAX_01_GET_KV = "SPHRM_INCOME_TAX_01_GET_KV";
        public const string SPHRM_EMP_INCOME_TAX_01_GET_XML = "SPHRM_EMP_INCOME_TAX_01_GET_XML";



        //Salary Payment
        public const string SPHRM_EMP_SAL_PAYMENT_GET_LIST = "SPHRM_EMP_SAL_PAYMENT_GET_LIST";
        public const string SPHRM_EMP_SAL_PAYMENT_GET_KV = "SPHRM_EMP_SAL_PAYMENT_GET_KV";
        public const string SPHRM_EMP_SAL_PAYMENT_DELETE = "SPHRM_EMP_SAL_PAYMENT_DELETE";
        public const string SPHRM_EMP_SAL_PAYMENT_SAVE = "SPHRM_EMP_SAL_PAYMENT_WKF_SAVE";
        public const string SPHRM_EMP_SAL_PAYMENT_DTL_GET = "SPHRM_EMP_SAL_PAYMENT_DTL_GET";
        public const string SPHRM_BANK_STMT_OUTPUT_RPT = "SPHRM_BANK_STMT_OUTPUT_RPT";
        public const string SPHRM_BANK_STMT_RPT = "SPHRM_BANK_STMT_RPT";
        public const string SPHRM_EMP_SAL_PAYMENT_MODE_DTL_DAT_GET = "SPHRM_EMP_SAL_PAYMENT_MODE_DTL_DAT_GET";
        public const string SPHRM_EMP_SAL_PAYMENT_AUTO = "SPHRM_EMP_SAL_PAYMENT_AUTO";

        //Employee Addition / Deduction
        public const string SPHRM_OTHER_ADD_DED_SAVE = "SPHRM_OTHER_ADD_DED_SAVE";
        public const string SPHRM_OTHER_ADD_DED_GET_LIST = "SPHRM_OTHER_ADD_DED_GET_LIST";
        public const string SPHRM_OTHER_ADD_DED_GET_KV = "SPHRM_OTHER_ADD_DED_GET_KV";
        public const string SPHRM_OTHER_ADD_DED_DELETE = "SPHRM_OTHER_ADD_DED_DELETE";
        public const string SPHRM_OTHER_ADD_DED_GET_XML = "SPHRM_OTHER_ADD_DED_GET_XML";
        public const string SPHRM_OTHER_ADD_DED_OUTPUT_RPT = "SPHRM_OTHER_ADD_DED_OUTPUT_RPT";
        


        //Functions
        public const string FNHRM_EMP_LEAVE_BAL_GET = "FNHRM_EMP_LEAVE_BAL_GET";
        public const string FNHRM_EMP_WORK_HR_GET = "FNHRM_EMP_WORK_HR_GET";

        //Attendance Popup
        public const string SPHRM_EMP_ATTENDANCE_IN_OUT_GET_KV = "SPHRM_EMP_ATTENDANCE_IN_OUT_GET_KV";
        public const string SPHRM_EMP_LEAVE_HDR_SAVE = "SPHRM_EMP_LEAVE_HDR_SAVE";



        // Extra Days
        public const string SPHRM_EMP_EXTRA_DAYS_GET_LIST = "SPHRM_EMP_EXTRA_DAYS_GET_LIST";
        public const string SPHRM_EMP_EXTRA_DAYS_SAVE="SPHRM_EMP_EXTRA_DAYS_SAVE";
        public const string SPHRM_EMP_EXTRA_DAYS_GET_XML = "SPHRM_EMP_EXTRA_DAYS_GET_XML";
        public const string SPHRM_EMP_EXTRA_DAYS_DELETE = "SPHRM_EMP_EXTRA_DAYS_DELETE";
        public const string SPHRM_EMP_ATTENDANCE_DTL_IMPORT_SAVE = "SPHRM_EMP_ATTENDANCE_DTL_IMPORT_SAVE";
        public const string SPHRM_EMP_EXTRA_DAYS_IMP_SAVE = "SPHRM_EMP_EXTRA_DAYS_IMP_SAVE";

        public const string SPHRM_OTHER_ADD_DED_AUTO = "SPHRM_OTHER_ADD_DED_AUTO";

        //Bonus Entry
        public const string SPHRM_EMP_BONUS_SAVE = "SPHRM_EMP_BONUS_SAVE";
        public const string SPHRM_BONUS_TYPE_GET_KV = "SPHRM_BONUS_TYPE_GET_KV";
        public const string SPHRM_EMP_BONUS_EMP_GET = "SPHRM_EMP_BONUS_EMP_GET";
        public const string SPHRM_EMP_BONUS_GET_LIST = "SPHRM_EMP_BONUS_GET_LIST";
        public const string SPHRM_EMP_BONUS_GET_XML = "SPHRM_EMP_BONUS_GET_XML";
        public const string SPHRM_EMP_BONUS_DELETE = "SPHRM_EMP_BONUS_DELETE";

       // HRMS Year End
        public const string SPHRM_YEAR_END_DATA = "SPHRM_YEAR_END_DATA";
        public const string SPHRM_YEAR_MST_GET_LIST = "SPHRM_YEAR_MST_GET_LIST";


        //Employee Salary Appraisal
        public const string SPHRM_EMP_APPRAISAL_NEW_SAVE = "SPHRM_EMP_APPRAISAL_NEW_SAVE";
        public const string SPSPHRM_EMP_APPRAISAL_NEW_WKF_SAVE = "SPSPHRM_EMP_APPRAISAL_NEW_WKF_SAVE";
        public const string SPHRM_EMP_APPRAISAL_NEW_GET_XML = "SPHRM_EMP_APPRAISAL_NEW_GET_XML";
        public const string SPHRM_EMP_APPRAISAL_NEW_GET_LIST = "SPHRM_EMP_APPRAISAL_NEW_GET_LIST";
        public const string SPHRM_EMP_APPRAISAL_NEW_DELETE = "SPHRM_EMP_APPRAISAL_NEW_DELETE";
        public const string SPHRM_EMP_ATTENDANCE_GEN_IMPORT_SAVE = "SPHRM_EMP_ATTENDANCE_GEN_IMPORT_SAVE";

        public const string SPHRM_OTHER_ADD_DED_IMPORT_SAVE = "SPHRM_OTHER_ADD_DED_IMPORT_SAVE";

        //Bulk Appraisal
        public const string SPHRM_BULK_APPRAISAL_EMP_GET = "SPHRM_BULK_APPRAISAL_EMP_GET";
        public const string SPHRM_BULK_APPRAISAL_SAVE = "SPHRM_BULK_APPRAISAL_SAVE";
        public const string SPHRM_BULK_APPRAISAL_GET_LIST = "SPHRM_BULK_APPRAISAL_GET_LIST";
        public const string SPHRM_BULK_APPRAISAL_GET_XML = "SPHRM_BULK_APPRAISAL_GET_XML";
        public const string SPHRM_BULK_APPRAISAL_DELETE="SPHRM_BULK_APPRAISAL_DELETE";
        public const string SPHRM_BULK_APPRAISAL_WKF_SAVE = "SPHRM_BULK_APPRAISAL_WKF_SAVE";

        //External Material Issue (Multiple)
        public const string SPINV_ITEM_EXT_ISS_RCV_GET = "SPINV_ITEM_EXT_ISS_RCV_GET";

        //Customer Brand
        public const string SPCRM_CUST_ITEM_MAP_BUL_DEL = "SPCRM_CUST_ITEM_MAP_BUL_DEL";

        //Other Leave Entry
        public const string SPHRM_EMP_OTHER_LEAVE_SAVE= "SPHRM_EMP_OTHER_LEAVE_SAVE";
        public const string SPHRM_EMP_OTHER_LEAVE_HDR_GET_LIST = "SPHRM_EMP_OTHER_LEAVE_HDR_GET_LIST";
        public const string SPHRM_EMP_OTHER_LEAVE_HDR_AUTO= "SPHRM_EMP_OTHER_LEAVE_HDR_AUTO";
        public const string SPHRM_EMP_OTHER_DTL_GET= "SPHRM_EMP_OTHER_DTL_GET";
        public const string SPHRM_EMP_OTHER_LEAVE_HDR_DELETE = "SPHRM_EMP_OTHER_LEAVE_HDR_DELETE";

        //full and final settlement
        public const string SPHRM_EMP_FINAL_SETTLEMNT_GET_LIST = "SPHRM_EMP_FINAL_SETTLEMNT_GET_LIST";
        public const string SPHRM_EMP_FINAL_SETTLEMENT_SAVE = "SPHRM_EMP_FINAL_SETTLEMENT_SAVE";
        //public const string SPHRM_EMP_PAYROLL_DTL_GET_XML = "SPHRM_EMP_PAYROLL_DTL_GET_XML";
    }

}
