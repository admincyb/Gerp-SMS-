using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.CommonManagement;
using BusinessObject.CommonManagement;
using GTIService.Constants.StockTransfer;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StockTransfer : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private int prefID = 0;
        private DataTable dtCompany;
        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            // set action control set as visible
            ucrWrkf.ViewType = 1;
            // Assign current user details

            // Action for workflow submit
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                // fill indital date to pageload
                FillInitialData();
                // Assign login user details to hidden field
                SFH_BIZUNIT.Value = currentUser.SBUID.ToString();
                SFH_SUBMITTED_BY.Value = currentUser.PKUser.ToString();
                SFH_SUBMITTED_DATE.Value = currentUser.PKUser.ToString();
                SFH_APPROVED_BY.Value = currentUser.PKUser.ToString();
                SFH_APPROVED_DATE.Value = DateTime.Now.ToString();
                SFH_CRTD_BY.Value = currentUser.PKUser.ToString();
                SFH_CRTD_DT.Value = DateTime.Now.ToString();
                SFH_MOD_BY.Value = currentUser.PKUser.ToString();
                SFH_MOD_DT.Value = DateTime.Now.ToString();
                APT_CODE.Value = ApplicationType.STR;
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                string redirectURL = "../login.aspx";
                if (!base.HasPageRight())
                {
                    Session.Abandon();
                    System.Web.Security.FormsAuthentication.SignOut();
                    if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                    {
                        redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                }
            }
        }
        #endregion

        #region HelperMethods
        /// <summary>
        /// Method to fill Storetansfer Number
        /// </summary>
        /// <param name="objUser"></param>
        private void FillStoreTansferNo(BusinessObject.User objUser)
        {
            //string stNo;
            //// get stock transfer format
            //DataTable dtSTNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, Fields.STOCKTRANSFERNO, 0);
            //// set stock tranfer number
            //if (dtSTNoFormat.Rows.Count > 0)
            //    stNo = dtSTNoFormat.Rows[0][Fields.DEFAULTVALUE].ToString();
            //else
            //    stNo = GTIService.Constants.Common.CommonConstant.STOCKTRANSFERNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(stNo, Fields.NUMBERFORMATEXPR);
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        stNo = stNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace(Fields.HASHVALUE, string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        stNo = stNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.StockTransferBL.GetStockTransfer());
            //    }

            //}
            // Show Store Transfer Number
            SFH_NO.Text = "[NEW]";
            if (Request.QueryString["PRefID"] == null)//From Inbox show the GRN Date as SADate By Default. 
                SFH_DATE.Text = DateTime.Now.ToString(Fields.DATEFORMAT);

        }

        private void ConfigurationSettings()
        {
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0"; 
        }

        /// <summary>
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
            FillProcessId(true);
            int refId = 0;
            int appId = 0;
            ConfigurationSettings();
            // check querystirng have PK Value
            if (Request.QueryString[Fields.PKVALUE] != null)
            {
                // Fill stock tranfer details by sslected pK
                FillStockTransferDetails(Convert.ToInt32(Request.QueryString[Fields.PKVALUE].ToString()));
            }
            // check querystirng have RefID Value
            else if (Request.QueryString[Fields.REFIDVALUE] == null)
            {
                // Fill stock tranfer details by 0 PK
                FillStockTransferDetails(0);
            }
            // check querystirng have RefID Value
            else if (Request.QueryString[Fields.REFIDVALUE] != null)
            {
                // assign refid to usercontrol property
                ucrWrkf.RefID = int.Parse(Request.QueryString[Fields.REFIDVALUE]);
                // Get An assign RefiD
                refId = int.Parse(Request.QueryString[Fields.REFIDVALUE]);
                hdfRefID.Value = refId.ToString();

                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                // Get Applixcation details by RefID
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        // Assign appID from Datatable
                        appId = Convert.ToInt32((dtApplication.Rows[0][Fields.APPID] == DBNull.Value) ? 0 : dtApplication.Rows[0][Fields.APPID]);
                    }
                }
                // FFIll stock Tranfer Details by AppID
                FillStockTransferDetails(appId);
                // Fill Workflow Details
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
            }
            if (Request.QueryString["PRefID"] != null)
            {
                IsPrefID.Value = "1";
                prefID = int.Parse(Request.QueryString["PRefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(prefID);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        string appID = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                        GinPk.Value = appID;
                        DataTable dtStore = BusinessLogic.StoreManagement.GoodsInspectionNote.GetGinStore(Convert.ToInt32(appID));
                        if (dtStore != null && dtStore.Rows.Count > 0)
                            hdfGinStore.Value = dtStore.Rows[0]["GIH_DEPT"].ToString();
                        else
                            hdfGinStore.Value = "0";
                    }
                }
                //
            }

            // check querystirng have Status Value, Thrn Action is view
            if (Request.QueryString[Fields.STATUS] != null)
            {
                // Set Wrkflow Action Details section visible as false
                ucrWrkf.ViewType = 0;
                // check querystirng have RefID Value,
                if (Request.QueryString[Fields.REFIDVALUE] != null)
                {
                    ucrWrkf.RefID = int.Parse(Request.QueryString[Fields.REFIDVALUE]);
                    // Fill Workflow Details
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnSubmit.Visible = false;
                }
                // check querystirng have Status Value =2
                else if (Request.QueryString[Fields.STATUS] == Fields.VALUETWO)
                {
                    // Get Process ID 
                    int procId = GetStockTransferProcessID();
                    // Get Workflow Details
                    WorkFlowDetails wrkFlowDtls = CommonBL.GetWorkflowDetails(int.Parse(Request.QueryString[Fields.PKVALUE].ToString()), procId);
                    // Check Workflow details have value
                    if (wrkFlowDtls != null)
                    {
                        // Assign RefID
                        ucrWrkf.RefID = wrkFlowDtls.ReferenceID;
                        // Assign Workflow View Type
                        ucrWrkf.ViewType = Request.QueryString[Fields.STATUS].ToString() == Fields.VALUETWO ? 0 : 0;

                    }
                    else
                    {
                        ucrWrkf.RefID = 0;
                        ucrWrkf.ViewType = 0;
                    }
                    // FIll Workflow Details
                    ucrWrkf.FillWorkFlowDetails(true);
                    btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }

                // Hide Save Button
                btnSave.Visible = false;

            }
            else
            {
                // check querystirng have Flag Value 
                if (Request.QueryString[Fields.FLAG] != null)
                {
                    // Set Workflow Action Section is Hide
                    ucrWrkf.ViewType = 0;
                    btnSave.Visible = false;
                }
                else
                {
                    // Set Workflow Action Section is Show
                    ucrWrkf.ViewType = 1;
                }

                ucrWrkf.Visible = true;
                // Fill Process details
                FillProcessId();
            }
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfCompany.Value = hdfSBUCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
            if (Request.QueryString["IsModify"] != null)
            {
                btnSave.Visible = true;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.STR, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }


        private void FillStockTransferDetails(int pk)
        {

            BusinessObject.StoreManagement.StockTransferBO stockTransfer = null;
            if (pk != 0)
            {
                // get and assign stock tranfer details to hiddenfiled
                StockTransferList.Value = BusinessLogic.StoreManagement.StockTransferBL.GetStockTransferDetails(pk);
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    hdfRefID.Value = Request.QueryString["PRefID"].ToString();
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                    {
                        FillStockTransferDetails(Convert.ToInt32(dt.Rows[0]["appPK"]));
                        hdfTransactionPK.Value = dt.Rows[0]["appPK"].ToString();
                    }
                    else
                    {
                        SetPrimaryInfo();
                    }
                }
                else
                {
                    SetPrimaryInfo();
                }

            }
        }

        private void SetPrimaryInfo()
        {
            BusinessObject.StoreManagement.StockTransferBO stockTransfer = null;
            // create object
            stockTransfer = new BusinessObject.StoreManagement.StockTransferBO();
            stockTransfer.GINList = new List<BusinessObject.StoreManagement.GINItemsDetails>();
            stockTransfer.POList = new List<BusinessObject.StoreManagement.POsTransferQty>();
            stockTransfer.PRList = new List<BusinessObject.StoreManagement.PRsTransferQty>();
            stockTransfer.AllocatedAdditionalList = new List<BusinessObject.StoreManagement.AllocatedQtyList>();
            stockTransfer.GINPKList = new List<BusinessObject.StoreManagement.GINPKList>();
            stockTransfer.POPKList = new List<BusinessObject.StoreManagement.POPKList>();
            StockTransferList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(stockTransfer);
            FillStoreTansferNo(currentUser);
            AST_DOC_MODE.Value = GetDOCMODE();
        }
        //===========================##### Add Code For WorkFolw , Update Code In Fill Initial Data ###### =======================================
        /// <summary>
        /// Method to Fire event when click submit button in a Workflow, user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WrkfSubmit(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Save Details From Work Flow
                Save();
            }
        }

        /// <summary>
        /// Method to do Action For Work Flow
        /// </summary>
        private void Save()
        {
            int refId = 0;
            // Check type is new entry
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? Fields.ZEROVALUE : hdfAppID.Value) > 0)
            {
                // Assign Appid
                ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                // Fill Workflow Details
                refId = ucrWrkf.DoWorkFlow();
            }
            // id Workflow sucess
            if (refId > 0)
            {
                // Redirect to login Page
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'> $(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
            }

        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId(bool SetProcessID = false)
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[Fields.VIRTUALDIRECTORY].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings[Fields.VIRTUALDIRECTORY].ToLower(), Fields.STRINGEMPTY);
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess = null;
            if (Session[SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
            }
            else
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    // pass proc Id to wrkflw user control and fill action details 
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][Fields.PROCESSPK].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][Fields.PROCESSPK].ToString();
                    ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0][Fields.PAGEPK].ToString();
                    // Fill workflow details
                    ucrWrkf.FillWorkFlowDetails(true);
                }
        }

        private int GetStockTransferProcessID()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[Fields.VIRTUALDIRECTORY].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings[Fields.VIRTUALDIRECTORY].ToLower(), Fields.STRINGEMPTY);
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess;
            if (Session[SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                return int.Parse(dtProcess.Rows[0][Fields.PROCESSPK].ToString());

            }
            else
            {
                return 0;
            }
        }
        //===========================##### END Code For WorkFolw ###### =======================================


        #endregion
    }
}