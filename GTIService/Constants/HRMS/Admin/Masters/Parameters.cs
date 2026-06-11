using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Admin.Masters
{
    public class Parameters
    {
        public const string P_LTM_PK = "P_LTM_PK";
        public const string P_LTM_CODE = "P_LTM_CODE";
        public const string P_LTM_NAME = "P_LTM_NAME";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_PAGE_NUM = "P_PAGE_NUM";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string P_LTM_ACCURAL = "P_LTM_ACCURAL";
        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public const string P_XML = "P_XML";
        public const string P_RET_VAL = "P_RET_VAL";
        public const string P_SORT_BY = "P_SORT_BY";
        public const string P_FLD_NAME = "P_FLD_NAME";
        public const string P_VALUE = "P_VALUE";
        public const string P_PAGE_NO="P_PAGE_NO";
        public const string P_LTM_CREDIT = "P_LTM_CREDIT";
        public const string P_EMP_PK = "P_EMP_PK";
        public const string P_LTM_CARRY_FWD = "P_LTM_CARRY_FWD";
        public const string P_ELV_LEAVE_TYPE = "P_ELV_LEAVE_TYPE";
        public const string P_ELV_EMPLOYEE = "P_ELV_EMPLOYEE";
        public const string P_EMP_LEAVE_TYPE = "P_EMP_LEAVE_TYPE";
   
        // Leave Template
        public const string P_LTE_PK = "P_LTE_PK";
        public const string P_LTE_CODE = "P_LTE_CODE";
        public const string P_LTE_NAME = "P_LTE_NAME";
        public const string P_LTE_FORMULA = "P_LTE_FORMULA";
        public const string P_LTE_DEPT = "P_LTE_DEPT";
        public const string P_LTE_COMPANY = "P_LTE_COMPANY";
        public const string P_USER_PK = "P_USER_PK";        
        
        //OT Template
        public const string P_OTE_PK = "P_OTE_PK";
        public const string P_OTE_NAME = "P_OTE_NAME";
        public const string P_OTE_CODE = "P_OTE_CODE";
        public const string P_EMT_PK = "P_EMT_PK";

        #region PayElements Master
        public const string P_PEL_PK = "P_PEL_PK";
        public const string P_PEL_CODE = "P_PEL_CODE";
        public const string P_PEL_NAME = "P_PEL_NAME";
        public const string P_PEL_CODE_LL = "P_PEL_CODE_LL";
        public const string P_PEL_NAME_LL = "P_PEL_NAME_LL";

        public const string P_PEL_CLASS = "P_PEL_CLASS"; 
        public const string P_PEL_EFFECT_FROM = "P_PEL_EFFECT_FROM";
        public const string P_PEL_EFFECT_TO = "P_PEL_EFFECT_TO";
        public const string P_PEL_PARENT = "P_PEL_PARENT";
        public const string P_PEL_IN_PAY_SLIP = "P_PEL_IN_PAY_SLIP"; 
        public const string P_PEL_RECURRING = "P_PEL_RECURRING";
        public const string P_PEL_IN_CTC = "P_PEL_IN_CTC";
        public const string P_PEL_IS_EDITABLE = "P_PEL_IS_EDITABLE";
        public const string P_PEL_IN_GROSS = "P_PEL_IN_GROSS";
        public const string P_PEL_TAXABLE = "P_PEL_TAXABLE";
        public const string P_PEL_ACCOUNT = "P_PEL_ACCOUNT";  
        public const string P_PAY_EL_LASTMOD_DATE = "P_LAST_MOD_DT";
        public const string P_PARENT_PEL_PK = "P_PARENT_PEL_PK";
        public const string P_IS_LOAN_ADV = "P_IS_LOAN_ADV";
        public const string P_PEL_FORMULA_CODE = "P_PEL_FORMULA_CODE";
        public const string P_PEL_IS_DEDUCTION = "P_PEL_IS_DEDUCTION";
        public const string P_PEL_CALC_MODE = "P_PEL_CALC_MODE";
        public const string P_PEL_IN_SALARY = "P_PEL_IN_SALARY";
        public const string P_PEL_DESC = "P_PEL_DESC";
        public const string PEL_IS_FORMULA_EDIT = "PEL_IS_FORMULA_EDIT";
        public const string P_PEL_ROUND_OFF = "P_PEL_ROUND_OFF";
        public const string P_STS_PK = "P_STS_PK";

        public const string P_PEL_SEQUENCE = "P_PEL_SEQUENCE";
        public const string P_PEL_SHOW_IN_REPORT = "P_PEL_SHOW_IN_REPORT";
        public const string P_PEL_SHOW_IN_REPORT1 = "P_PEL_SHOW_IN_REPORT1";
        public const string P_PEL_SHOW_IN_EMPMAST = "P_PEL_SHOW_IN_EMPMAST";

        #endregion

        public const string P_STE_PK = "P_STE_PK";
        public const string P_STE_CODE = "P_STE_CODE";
        public const string P_STE_PAYROLL_TYPE = "P_STE_PAYROLL_TYPE";

        public const string P_ESH_ACTIVE = "P_ESH_ACTIVE";
        public const string P_STE_NAME = "P_STE_NAME";
        public const string STRINGEMPTY = "";
        public const string P_empPK = "P_empPK";

        #region PayElements Master
        public const string P_PTM_PK = "@P_PTM_PK";
        public const string V_PTM_PK = "V_PTM_PK";
        public const string P_IS_DEDUCTION = "P_IS_DEDUCTION";        
        #endregion

        #region Slab Definition
        public const string P_PHS_PK = "P_PHS_PK";
        public const string P_PHS_PAY_ELEMENT = "P_PHS_PAY_ELEMENT";
        public const string P_VAL_PK = "P_VAL_PK";
        public const string P_CODE = "P_CODE";
        public const string P_NAME = "P_NAME";
        public const string P_EFFECT_FROM = "P_EFFECT_FROM";
        public const string P_EFFECT_TO = "P_EFFECT_TO";
        public const string P_PAY_ELEMENT = "P_PAY_ELEMENT";
        public const string P_PEV_PK = "P_PEV_PK";
        #endregion

        #region Holiday Master
        public const string P_HDR_PK = "P_HDR_PK";
        public const string P_HDR_CAPTION = "P_HDR_CAPTION";
        public const string P_CFG_PK = "P_CFG_PK";
        public const string HDR_PK = "HDR_PK";

        #endregion

        #region Designation Master
        public const string P_dsgPK = "P_dsgPK";
        public const string P_dsgCode = "P_dsgCode";
        public const string P_dsgName = "P_dsgName";
        public const string P_TEXT = "P_TEXT";
        public const string P_dsgjobCategory = "P_dsgjobCategory";
        public const string P_dsgjobGrade = "P_dsgjobGrade";
        #endregion

        #region Bonus Type Master
        public const string P_BON_PK = "P_BON_PK";
        public const string P_BON_CODE = "P_BON_CODE";
        public const string P_BON_NAME =  "P_BON_NAME";
        #endregion

        #region Salary Report Template
        public const string P_SRT_PK = "P_SRT_PK";
        public const string P_SRT_CODE = "P_SRT_CODE";
        public const string P_SRT_NAME = "P_SRT_NAME";
        #endregion




    }
}
