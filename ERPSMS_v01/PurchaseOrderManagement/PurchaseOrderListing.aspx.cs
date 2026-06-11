using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessLogic.AccountManagement;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERP.Utilities;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderListing : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        BusinessObject.User objUser;
        #endregion               
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            ucTrxComments.AfterCommentControlEvent += new EventHandler(CommentControlHandler);
            //if (!IsPostBack)
            //{
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[BusinessObject.Common.SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                hdnShortCloseGroup.Value = ConfigurationManager.AppSettings["ShortCloseGroup"].ToString();
                hdnRoleID.Value = objUser.Roles;
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                hdfAppType.Value = ApplicationType.PO;
                hdfAppSubType.Value = string.Empty;
                GetUserRights();
                ConfigurationSettings();
           // }
        } 
        #endregion
        #region Functions
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            //string path = "/PurchaseOrderManagement/PurchaseOrderGenerate.aspx";
            string path = "purchaseordermanagement/purchaseordergenerate.aspx?TYPE=1";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;
        }

        private void GetUserRights()
        {
            string path = "/PurchaseOrderManagement/PurchaseOrderListing.aspx?TYPE=2";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "SHORTCLOSURE" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        hdnClosePO.Value = "1";
                        break;
                    }
                }
            }
        }

        private void ShowCommentPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTrxComments]','" + Resources.Captions.Comments + "','" + Resources.Constants.TrxComments_Width + "','" + Resources.Constants.TrxComments_Height + "');", true);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {            
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }
        #endregion
        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {               
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));              
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region COMMENTS
                    case ActionsEnum.COMMENTS:
                        ucTrxComments.TrxNo = hdfCmntTrxNo.Value;
                        ucTrxComments.TrxPk = Convert.ToInt32(hdfCmntTrxPk.Value);
                        ucTrxComments.AppType = ApplicationType.PO;
                        ucTrxComments.InitializeControl();
                        ShowCommentPopup();
                        break;
                    #endregion                  
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        
        #endregion
        #region Comment User Control Event Handler
        protected void CommentControlHandler(object sender, EventArgs e)
        {
            ShowCommentPopup();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetPage", "$(document).ready(function(){ResetPage();});", true);
        } 
        #endregion
    }
}