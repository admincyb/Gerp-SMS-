using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.HRMS
{
    public class Employee
    {

        #region Parametrs

        //Parameters for Country and Nationality
        public const string CNT_NAME = "P_CNT_NAME";
        public const string CNT_ACTIVE = "CNT_ACTIVE ";
        public const string P_CNT_NATIONALITY = "P_CNT_NATIONALITY ";
        public const string P_SORT_BY = "P_SORT_BY";

        //Parameters for Employee
        public const string P_empName = "P_empName";
        public const string P_empPK = "P_empPK";
        public const string P_RET_VAL = "P_RET_VAL";
        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";

        public const string P_EMP_TYPE = "P_empEmploymentType";
        public const string P_EMP_BRANCH = "P_EmpBranch";
        // Parameters for Employee Leave Master
        public const string P_ELH_EMPLOYEE = "P_ELH_EMPLOYEE";
        public const string P_ELH_DATE = "P_ELH_DATE";
        public const string P_EOH_DATE_FROM = "P_EOH_DATE_FROM";
        public const string P_EOH_DATE_TO = "P_EOH_DATE_TO";


        //Common DropDown Populate Param

        public const string P_CON_PK = "P_CON_PK";
        public const string P_CON_PARENT = "P_CON_PARENT";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_GROUP_TYPE_VALUE = "P_GROUP_TYPE_VALUE";
        public const string P_GROUP_VALUE = "P_GROUP_VALUE";
        public const string P_TEXT_VALUE = "P_TEXT_VALUE";
        public const string P_dsgName = "P_dsgName";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_empActive = "P_empActive";
        public const string P_SEARCH_VAL = "P_SEARCH_VAL";

        public const string P_ACTIVE_HEADER = "P_ACTIVE";
        public const string P_Is_Payroll = "P_Is_Payroll";
        public const string P_dsgJobLevel = "P_dsgJobLevel";
        public const string P_dsgJobCategory = "P_dsgJobCategory";



        //Fill Designatiom
        public const string P_dsgPK = "P_dsgPK";

        //QUALIFICATION LIST
        public const string P_EMPLOYEE = "P_EMPLOYEE";
        public const string P_EQD_PK = "P_EQD_PK";
        //EXPERIENCE 
        public const string P_EED_PK = "P_EED_PK";

        //CURRENCY
        public const string P_CURRENCY_ACT = "CUR_ACTIVE";
        public const string P_CURRENCY_PK = "CUR_PK";

        // Base Currency
        public const string P_ACF_SETTING = "P_ACF_SETTING";

        //    Employee Leave Master 
        public const string P_EOH_PK = "P_EOH_PK";


        public const string P_ELM_EMPLOYEE = "P_ELM_EMPLOYEE";

        //payroll Process
        public const string P_EPS_PK = "P_EPS_PK";
        public const string EPH_PK = "EPH_PK";

        //Attendace report
        public const string P_EAR_PK = "P_EAR_PK";

        //Addition Deduction Report
        public const string HDR_PK = "HDR_PK";
        public const string P_EMP_PK = "P_EMP_PK";


        //Employee Performance
        public const string P_CNG_PK = "P_CNG_PK";
        public const string P_CNG_GRP_TYPE = "P_CNG_GRP_TYPE";
        public const string P_CGT_VALUE = "P_CGT_VALUE";
        public const string P_CNG_BIZUNIT = "P_CNG_BIZUNIT";
        public const string P_CNG_PARENT = "P_CNG_PARENT";
        public const string P_CNG_SPL_COND = "P_CNG_SPL_COND";
        public const string P_EPD_PK = "P_EPD_PK";
        #endregion

        public const string P_VALUE = "P_VALUE";
        public const string STT_COUNTRY = "STT_COUNTRY";
    }
}
