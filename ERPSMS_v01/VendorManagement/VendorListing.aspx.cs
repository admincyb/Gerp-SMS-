using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


namespace ERPSMS_v01.VendorManagement
{
    public partial class VendorListing : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                FillInitialData();
            }
        }

        /// <summary>
        /// Function Used to fill Initial Rendering of the Screen
        /// </summary>
        /// <param name=""></param>        
        private void FillInitialData()
        {
           //getting the user roles to restrict actions
            
           UserRoles.Value = objUser.Roles;
           btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
           hdnFinalApprovar.Value = ConfigurationManager.AppSettings["FinalApprovar"].ToString();
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = "/VendorManagement/VendorMaster.aspx"; ;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;

        }


    }
}