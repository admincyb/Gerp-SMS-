using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Employee
{
    public sealed class Parameters
    {
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_Active = "P_Active";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_EMPPK = "P_EMPLOYEE";
        public const string P_EMP_DOC_PK = "P_EDD_PK";
        public const string P_XML = "P_XML";
        public const string P_CFG_PK = "P_CFG_PK";
        public const string P_CFG_TYPE = "P_CFG_TYPE";
        public const string P_EMPLOYEE_PK = "P_empPK";
        public const string P_EMPNAME = "P_empName";
        public const string P_EMP_PK = "P_EMP_PK";
        public const string P_EmpBranch = "P_EmpBranch";
        public const string P_EPD_EMP_TYPE = "P_EPD_EMP_TYPE";
        public const string P_EMP_EMPLOYMENT_Type = "P_empEmploymentType";
        public const string P_EMP_COMPANY = "P_empCompany";
        public const string P_empDepartment = "P_empDepartment";
        public const string P_EPD_PAY_MODE = "P_EPD_PAY_MODE";
        public const string P_EPD_CURRENCY = "P_EPD_CURRENCY";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";
        public const string P_PAYROLL_TYPE = "P_PAYROLL_TYPE";
        public const string P_EMS_CANCEL_STATUS = "P_EMS_CANCEL_STATUS";


        //Filter Document Sp Parameters
        public const string P_EMPCODE = "P_EMP_CODE";
        public const string P_EMP_NAME = "P_EMP_NAME";
        public const string P_EMPNATIONALITY = "P_EMP_NATIONALITY";
        public const string P_EDD_EXPIRES_IN = "P_EDD_EXPIRES_IN";
        public const string P_EMPCOMPANY = "P_EMP_COMPANY";
        public const string P_EDD_IS_CHECKED_IN = "P_EDD_IS_CHECKED_IN";
        public const string P_EDD_IS_CHECKED_FOR = "P_EDD_IS_CHECKED_FOR";
        public const string P_EDD_DOC_TYPE = "P_EDD_DOC_TYPE";
        public const string P_EDD_DOC_NO = "P_EDD_DOC_NO";
        public const string P_EMPLOYEE = "P_EMPLOYEE";
        public const string P_EDD_EXPIRES_ON = "P_EDD_EXPIRES_ON";

        public const string P_SORT_BY = "P_SORT_BY";
        public const string P_SORT_DIR = "P_SORT_DIR";

        //Filter Employee List Sp Parameters
        public const string P_SearchempDesignation = "P_empDesignation";
        public const string P_SearchempDepartment = "P_empDepartment";
        public const string P_SearchempCompany = "P_empCompany";
        public const string P_SearchempCode = "P_empCode";
        public const string P_SearchempName = "P_empName";
        public const string P_SearchempNationality = "P_empNationality";
        public const string P_SearchempCurStatus = "P_empCurStatus";
        public const string P_SearchempActive = "P_empActive";
        public const string P_empPassportNo = "P_empPassportNo";
        public const string P_empPermitNo = "P_empPermitNo";
        public const string P_empBranch = "P_empBranch";

        public const string P_GROUP_TYPE_VALUE = "P_GROUP_TYPE_VALUE";
        public const string P_GROUP_VALUE = "P_GROUP_VALUE";

        //Employee Skills
        public const string ACTIVE = "P_ACTIVE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string CON_PK = "P_CON_PK";
        public const string EMPLOYEE = "P_EMPLOYEE";
        public const string ESD_SKILL_CATEGORY = "P_ESD_SKILL_CATEGORY";
        public const string CFG_PK = "P_CFG_PK";
        public const string CFG_TYPE = "P_CFG_TYPE";
        public const string P_CFG_SPL_COND = "P_CFG_SPL_COND";

        public const string ESD_FLAG = "P_ESD_FLAG";
        public const string P_EMPCATEGORY = "P_empCategory";
        public const string P_EMPTEAMID = "P_EmpTeamId";
        public const string P_EMPCOSTCENTERID = "P_EmpCostcenterId";

        public static string P_EMS_PK = "P_EMS_PK";
        public static string P_EMS_EMPLOYEE = "P_EMS_EMPLOYEE";
        public static string P_EMS_DATE = "P_EMS_DATE";
        public static string P_EMS_STATUS = "P_EMS_STATUS";
        public static string P_EMS_REASON = "P_EMS_REASON";
        public static string P_USER_PK = "P_USER_PK";
        public static string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public static string P_RET_VAL = "P_RET_VAL";
        public static string P_EMS_EMP_PK = "P_EMS_EMP_PK";
        public static string P_EMB_PK = "P_EMB_PK";
        public static string P_EMB_EMPLOYEE = "P_EMB_EMPLOYEE";
        public static string P_EMB_DATE = "P_EMB_DATE";
        public static string P_EMB_BRANCH = "P_EMB_BRANCH";
        public static string P_EMB_REASON = "P_EMB_REASON";
        public static string P_EMB_EMP_PK = "P_EMB_EMP_PK";
        public const string P_EMP_TYPE_PK = "P_EMP_TYPE_PK";

        public const string P_EDL_PK = "P_EDL_PK";
        public const string P_EDL_DATE = "P_EDL_DATE";
        public const string P_EDL_FROM_DEPT = "P_EDL_FROM_DEPT";
        public const string P_EDL_TO_DEPT = "P_EDL_TO_DEPT";
        public const string P_EDL_REASON = "P_EDL_REASON";
        public static string P_EDL_EMPLOYEE = "P_EDL_EMPLOYEE";

        public const string P_DSL_PK = "P_DSL_PK";
        public const string P_DSL_DATE = "P_DSL_DATE";
        public const string P_DSL_FROM_DESIG = "P_DSL_FROM_DESIG";
        public const string P_DSL_TO_DESIG = "P_DSL_TO_DESIG";
        public const string P_DSL_REASON = "P_DSL_REASON";
        public const string P_DSL_EMPLOYEE = "P_DSL_EMPLOYEE";

        public const string P_dsgPK = "P_dsgPK";
                   

        //Pay Details
        public const string P_EPD_PK = "P_EPD_PK";
        public const string P_CBM_PK = "P_CBM_PK";
        public const string P_CBM_TYPE = "P_CBM_TYPE";
        public const string P_CBM_ACC_TYPE = "P_CBM_ACC_TYPE";

        public const string P_EPD_EMPLOYEE = "P_EPD_EMPLOYEE";

        //Employee Salary
        public const string P_TO_DATE = "P_TO_DATE";
        public const string P_PTM_PK = "P_PTM_PK";
        public const string P_EDP_EMPLOYEE = "P_EDP_EMPLOYEE";
        public const string P_TR_DATE = "P_TR_DATE";

        //Employee Leave Type
        public const string P_ELV_PK = "P_ELV_PK";
        public const string P_ELV_EMPLOYEE = "P_ELV_EMPLOYEE";
        public const string P_LEAVE_TYPE = "P_LEAVE_TYPE";

        public const string P_PTM_CODE = "P_PTM_CODE";
        public const string P_PTM_NAME = "P_PTM_NAME";

        public const string P_PTM_PRC_MODE = "P_PTM_PRC_MODE";
        public const string P_EMB_FROM_BRANCH = "P_EMB_FROM_BRANCH";
        public const string P_EMS_FROM_STATUS = "P_EMS_FROM_STATUS";

        //Designation Master
        public const string P_dsgCode = "P_dsgCode";
        public const string P_dsgName = "P_dsgName";
        public const string P_empDesignation = "P_empDesignation";
        public const string P_empPayRollType = "P_empPayRollType";


        // Employee Training
        public const string P_ETA_PK = "P_ETA_PK";
        public const string HDR_PK = "HDR_PK";
        public const string P_TRN_MONTH_FROM = "P_TRN_MONTH_FROM";
        public const string P_TRN_MONTH_TO = "P_TRN_MONTH_TO";
        public const string P_ETA_TOPIC = "P_ETA_TOPIC";
        public const string P_ETY_EMP_PK = "P_ETY_EMP_PK";
        public const string P_ETY_PK = "P_ETY_PK";
        public const string P_ETY_EMPLOYEE = "P_ETY_EMPLOYEE";
        public const string P_ETY_DATE = "P_ETY_DATE";
        public const string P_ETY_FROM_EMP_TYPE = "P_ETY_FROM_EMP_TYPE";
        public const string P_ETY_EMP_TYPE = "P_ETY_EMP_TYPE";
        public const string P_ETY_REASON = "P_ETY_REASON";
        public const string P_EXPIRED_ON = "P_EXPIRED_ON";
        public const string P_MAIL_BEFORE = "P_MAIL_BEFORE";
        public const string P_IS_CONTRACT = "P_IS_CONTRACT";
        public const string P_FROM_DATE = "P_FROM_DATE";

        //Employee Transfer
        public const string P_EFH_FROM = "P_EFH_FROM";
        public const string P_EFH_TO = "P_EFH_TO";
        public const string P_EFH_PK = "P_EFH_PK";
        public const string P_EFH_STATUS = "P_EFH_STATUS";
        public const string P_EFHPK = "EFH_PK";

    }
}
