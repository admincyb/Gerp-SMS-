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
    public partial class SemiFinishedGoodsList : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdfpageURL.Value = "/Production/SemiFinishedGoods.aspx";
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GetUserRights();
               // btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
            }
        }

          /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (this.GetLocalResourceObject("BreadcrumbNew") != null && currentUser != null)
            {
                string breadCrumb;
                breadCrumb = currentUser.CurrentDept + this.GetLocalResourceObject("BreadcrumbNew").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                lblbreadCrumNew.Text = breadCrumb;
            }

        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = "/Production/SemiFinishedGoods.aspx";
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
            string path = "/Production/SemiFinishedGoods.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.CurrentSBUPK, currentUser.CurrentDeptPK);
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