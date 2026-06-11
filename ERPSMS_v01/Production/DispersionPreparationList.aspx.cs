using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using System.Data;
using BusinessLogic.AccountManagement;

namespace ERPSMS_v01.Production
{
    public partial class DispersionPreparationList :  ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)        {
            
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GetUserRights();
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());

                BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, SFGInspectionUrl(Resources.PageURL.RawMaterialInspectionTest.ToString()), currentUser.SBUID, currentUser.CurrentDeptPK);
                if (usrRights.Rights != null && usrRights.Rights.Count > 0)
                    hdfQcStatus.Value = "1";
                else
                    hdfQcStatus.Value = "0";
               
                //For get page Info
                DataTable dtPageInfo = BusinessLogic.CommonManagement.CommonBL.GetPageInfo(SFGInspectionUrl(Resources.PageURL.RawMaterialInspectionTest.ToString()));
                int pageId = (dtPageInfo.Rows.Count > 0 ? Convert.ToInt32(dtPageInfo.Rows[0]["PAG_PK"]) : 0);//For get page ID
                hdfQcPath.Value = (dtPageInfo.Rows.Count > 0 ?  dtPageInfo.Rows[0]["PAG_SERVER"].ToString() : string.Empty);//For get Path
                //For Get QC Department  
                DataTable dtDept = BusinessLogic.CommonManagement.CommonBL.GetPageDept(pageId, currentUser.PKUser);
                hdfQcDept.Value = (dtDept.Rows.Count > 0 ? dtDept.Rows[0]["DPT_PK"].ToString() : "0");
            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = "/Production/DispersionPreparation.aspx"; 
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;

        }

        /// <summary>
        /// Semi finished good Inspection URL set TYPE parameter as 2
        /// </summary>
        /// <param name="path">RawMateril Page Url</param>
        /// <returns></returns>
        private string SFGInspectionUrl(string path)
        {
            return path += "?TYPE=2";
        }

        /// <summary>
        /// Get User right for CANCEL action
        /// </summary>
        private void GetUserRights()
        {
            string path = "/Production/DispersionPreparationList.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "CANCEL" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        hdfCancelRight.Value = "1";
                        break;
                    }
                }
            }
        }
    }
}