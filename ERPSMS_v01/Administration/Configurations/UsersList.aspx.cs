using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using System.Data;
using BusinessLogic.Administration.Configurations;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class UsersList : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                //if (objUser.PKUser == (int)UserRight.SuperAdmin)
                if(IsSuperAdminUser(objUser.PKUser))
                {
                    divUserType.Visible = true;
                    divUserModule.Visible = true;
                    hdfUserType.Value = ((int)UserRight.SuperAdmin).ToString();
                }

                #region Configuration SBUSpecificUser

                hdfSBUSpecificUser.Value = GetGlobalResourceObject("ConfigurationsRes", "SBUSpecificUser").ToString();

                #endregion
            }

        }
        private bool IsSuperAdminUser(int pkUser)
        {
            bool retVal = false;
            DataTable dtResult = UserManagementBL.SuperAdminMstGet(pkUser);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = dtResult.Rows[0]["usrIsSuperAdmin"].ToString() == "1" ? true : false;
            }
            return retVal;
        }
    }
}