using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.Admin.Masters
{
    public sealed class Fields
    {
        public const string ADM_CFG_TEXT = "CFG_DATA";
        public const string ADM_CFG_VALUE = "CFG_VALUE";

        public const string ROW_NO = "ROW_NO";
        public const string LTM_PK = "LTM_PK";
        public const string LTM_CODE = "LTM_CODE";
        public const string LTM_NAME = "LTM_NAME";
        public const string PTM_PRC_MODE = "PTM_PRC_MODE";
        public const string PTM_PRC_MODE_TEXT = "PTM_PRC_MODE_TEXT";
        public const string LTM_DESC = "LTM_DESC";
        public const string LTM_ACCURAL = "LTM_ACCURAL";
        public const string LTM_ACCURAL_TEXT = "LTM_ACCURAL_TEXT";
        public const string LTM_PAID = "LTM_PAID";
        public const string LTM_LIMIT = "LTM_LIMIT";
        public const string LTM_CARRY_FWD_LIMIT = "LTM_CARRY_FWD_LIMIT";
        public const string LTM_ENCASH = "LTM_ENCASH";
        public const string LTM_CREDIT = "LTM_CREDIT";
        public const string LTM_CARRY_FWD = "LTM_CARRY_FWD";
        public const string LTM_RATE = "LTM_RATE";
        public const string LTM_FACTOR = "LTM_FACTOR";
        public const string LTM_ACTIVE = "LTM_ACTIVE";
        public const string LTM_STATUS = "LTM_STATUS";
        public const string LAST_MOD_DT = "LAST_MOD_DT";
        public const string TOTAL_ROW_COUNT = "TOTAL_ROW_COUNT";
        public const string ELV_LIMIT = "ELV_LIMIT";

        // Employee Type
        public const string LTE_PK = "LTE_PK";
        public const string LTE_NAME = "LTE_NAME";
        public const string OTE_PK = "OTE_PK";
        public const string OTE_NAME = "OTE_NAME";
        public const string EMT_PK = "EMT_PK";
        public const string EMT_NAME = "EMT_NAME";



        #region PayElements Master
        public const string F_PEL_PK = "PEL_PK";
        public const string F_PEL_CODE = "PEL_CODE";
        public const string F_COA_PK = "COA_PK";
        public const string F_COA_NAME_TEXT = "COA_NAME_TEXT";
        public const string F_PEL_NAME = "PEL_NAME";
        public const string F_PEL_CLASS = "PEL_CLASS";
        public const string F_PEL_EFFECT_FROM = "PEL_EFFECT_FROM";
        public const string F_PEL_EFFECT_TO = "PEL_EFFECT_TO";
        public const string F_PEL_PARENT = "PEL_PARENT";
        public const string F_PEL_IN_PAY_SLIP = "PEL_IN_PAY_SLIP";
        public const string F_PEL_RECURRING = "PEL_RECURRING";
        public const string F_PEL_IN_CTC = "PEL_IN_CTC";
        public const string F_PEL_IN_GROSS = "PEL_IN_GROSS";
        public const string F_PEL_IS_EDITABLE = "PEL_IS_EDITABLE";
        public const string F_PEL_TAXABLE = "PEL_TAXABLE";
        public const string F_PEL_ACTIVE = "PEL_ACTIVE";
        public const string F_PEL_ACCOUNT = "PEL_ACCOUNT";
        public const string F_PEL_MOD_DT = "PEL_MOD_DT";
        public const string F_PEL_IS_DEDUCTION = "PEL_IS_DEDUCTION";
        public const string F_PEL_FORMULA_CODE = "PEL_FORMULA_CODE";
        public const string F_PEL_CALC_MODE = "PEL_CALC_MODE";
        public const string F_PEL_USD_IN_FMLA = "PEL_USD_IN_FMLA";
        public const string F_PEL_ACCOUNT_TEXT = "PEL_ACCOUNT_TEXT";
        public const string F_PEL_IN_SALARY = "PEL_IN_SALARY";
        public const string F_PEL_FORMULA_BODY = "PEL_FORMULA_BODY";
        public const string F_VAL_NAME = "VAL_NAME";
        public const string F_VAL_PK = "VAL_PK";
        public const string F_PEL_IS_USED = "PEL_IS_USED";
        public const string F_PEL_CODE_LL = "PEL_CODE_LL";
        public const string F_PEL_NAME_LL = "PEL_NAME_LL";
        public const string F_PEL_DESC = "PEL_DESC";
        public const string F_PEL_IS_FORMULA_EDIT = "PEL_IS_FORMULA_EDITABLE";
        public const string F_PEL_ROUND_OFF = "PEL_ROUND_OFF";
        public const string F_PEL_SEQUENCE = "PEL_SEQUENCE";
        public const string F_PEL_SHOW_IN_REPORT = "PEL_SHOW_IN_REPORT";
        public const string F_PEL_SHOW_IN_REPORT1 = "PEL_SHOW_IN_REPORT1";
        public const string F_PEL_SHOW_IN_EMPMAST = "PEL_SHOW_IN_EMPMAST";
        #endregion

        public const string VALUE_PERC = "%";


        #region PayrollTypeMaster

        public const string PTM_PK = " PTM_PK";
        public const string PTM_CODE = "PTM_CODE";
        public const string PTM_NAME = "PTM_NAME";
        public const string PTM_DESC = "PTM_DESC";
        public const string PTM_ACTIVE = "PTM_ACTIVE";
        public const string PTM_PAYRL_START = "PTM_PAYRL_START";
        public const string PTM_OFFSET = "PTM_OFFSET";
        public const string PTM_MOD_DT="PTM_MOD_DT";
        #endregion

        #region Designation
        public const string dsgPK = "dsgPK";
        public const string dsgCode = "dsgCode";
        public const string dsgName = "dsgName";
        public const string dsgDesc = "dsgDesc";
        public const string dsgActive = "dsgActive";
        public const string dsgModOn = "dsgModOn";
        public const string dsgJobLevel = "dsgJobLevel";
        public const string dsgJobCategory = "dsgJobCategory";  
        #endregion

        #region Bonus Type Master
        public const string BON_PK = "BON_PK";
        public const string BON_CODE = "BON_CODE";
        public const string BON_NAME = "BON_NAME";
        public const string BON_DESC = "BON_DESC";
        public const string BON_ACTIVE = "BON_ACTIVE";
        public const string BON_MOD_ON = "BON_MOD_ON";
        public const string BON_FORMULA = "BON_FORMULA";
        public const string BON_FORMULA_TEXT = "BON_FORMULA_TEXT";
        public const string LTM_AUTO_CREDIT = "LTM_AUTO_CREDIT";        
        #endregion

    }
}
