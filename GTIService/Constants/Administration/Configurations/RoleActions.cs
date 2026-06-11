using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Administration.Configurations
{
    public class RoleActions
    {
        #region SPS
        public const string SP_GET = "SPAD_ROLES_MST_GET_KV";
        public const string SP_GETTREE = "SPAD_ACTIONS_TREE";
        public const string SP_SAVE = "SPADM_USER_GROUP_ACTION_MAP_SAVE";
        public const string SP_GETTREEXML = "SPADM_USER_GROUP_ACTION_GET_TREE";
        #endregion

        #region Fields
        public const string F_PK = "Pmgp";
        public const string F_CODE = "ROL_Code";
        public const string F_CODE_NAME = "PmgNme";
        public const string F_NAME = "ROL_Name";
        public const string F_DESCRIPTION = "ROL_Desc";
        public const string F_ROLELEVEL = "ROL_Level";
        public const string F_ROLEPARENTCODE = "ROL_Parent_Code";
        public const string F_ROLPARENT = "ROL_Parent";
        public const string F_ROLEACTIVE = "ROL_Active";
        public const string F_ROLEBIZUNIT = "ROL_BizUnit";
        public const string F_LASTMODDT = "LAST_MOD_DT";
        public const string DROPDOWNROLES = "ddlRoles";

        // [ACT_Page],[ACT_Section],[ACT_Action],[ACT_PK],
        public const string F_ACT_PAGE = "ACT_Page";
        public const string F_ACT_SECTION = "ACT_Section";
        public const string F_ACT_ACTION = "ACT_Action";
        public const string F_ACT_PK = "ACT_PK";

        public const string F_ROLE_FLAG = "ROLE_FLAG";
        public const string F_PAGE_FLAG = "PAGE_FLAG";
        public const string F_SECTION_FLAG = "SECTION_FLAG";
        #endregion

        #region Parameters
        public const string PK = "P_USER_GROUP";
        public const string PARENT = "P_ROL_Parent";
        public const string CODE = "P_ROL_Code";
        public const string NAME = "P_ROL_Name";
        public const string DESCRIPTION = "P_ROL_Desc";
        public const string ROLELEVEL = "P_ROL_Level";
        public const string P_XML = "P_GAM_XML";
        public const string P_ACTIVEPAGE = "P_PAGE";

        public const string P_RAM_Role = "P_GAM_USER_GROUP";

        #endregion
    }
}
