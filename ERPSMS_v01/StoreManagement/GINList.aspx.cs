using System;
using System.Data;
using System.Web;
using BusinessObject.CommonManagement;
using ERPData;
using ERPService;
using System.Collections.Generic;
using BusinessLogic.AccountManagement;
using BusinessObject;
using BusinessObject.Common;

namespace ERPSMS_v01.StoreManagement
{
    public partial class GINList : ERP.Store.UI.MyBasePage
    {
        #region Private Variables
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;
        #endregion

        BusinessObject.User objUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // asssign User details to object
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                // set add button visiblity by initial task permission
                if (Request.QueryString["TYPE"] != null)
                    hdfType.Value = Request.QueryString["TYPE"].ToString();
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                hdfAppSubType.Value = string.Empty;
                SetConfigData();
                GetUserRights();
            }
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            // set entry screen path
            string path = GTIService.Constants.StockTransfer.Fields.GIN_Entry_Screen + "?TYPE=" + hdfType.Value;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by path and dept PK
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // assign procID
                procId = int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());
                hdfProcId.Value = procId.ToString();
            }
            // return ProcID
            return procId;

        }
        private void SetConfigData()
        {
            CommonService CommonServiceClient;
            CommonServiceClient = new CommonService();
            admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
            admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
            admAppConfigMstObj.ACF_PK = 111;
            admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("CFG_Setting").ToString();
            admAppConfigMstList = CommonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
            if (admAppConfigMstList.Count > 1)
            {
                hdfClient.Value = admAppConfigMstList[1].ACF_DATA.ToString();
                if (admAppConfigMstList[1].ACF_DATA == "EKK")
                    hdfAppType.Value = ApplicationType.GRN;
                else
                    hdfAppType.Value = ApplicationType.GIN;
            }
            else
                hdfAppType.Value = ApplicationType.GIN;

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                string NavUrl = hdfPrintUrl.Value;
                //Response.Redirect(NavUrl);

                Response.Redirect("../Reports/GenerateReport.aspx?ID=292&APPTYPE=GIN&APPSUBTYPE=", false);

            }
            catch (Exception ex)
            {
                //Process Exception and show error message
            }
            finally
            {
                //reset all objects
            }
        }
        private void GetUserRights()
        {
            string path = "/StoreManagement/GINCreate.aspx?TYPE=" + hdfType.Value.ToString();//"/StoreManagement/GINCreate.aspx?TYPE=1";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);          
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnModifyGIN.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnCancelGIN.Value = "1";
                    }
                }
            }
        }
    }
}