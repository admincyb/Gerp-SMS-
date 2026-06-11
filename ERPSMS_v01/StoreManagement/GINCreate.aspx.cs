using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogic.CommonManagement;
using BusinessObject.CommonManagement;
using System.Web.UI;
using GTIService.Constants.Store;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class GINCreate : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        int applicationType = 1;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // action for workflow save
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                // assign user details to objkect
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                if (Request.QueryString["TYPE"] != null)
                    hdfType.Value = Request.QueryString["TYPE"].ToString();
                // Fill initial date in page load
                FillInitialData();

                APT_CODE.Value = ApplicationType.GIN;
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

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillGINDetails(int pk)
        {

            BusinessObject.StoreManagement.GoodsIssueNote goodsIssueNote = null;
            // check Pk have value !=0
            if (pk != 0)
            {
                // Get GIN details and assign to hiddenfiled
                GINList.Value = BusinessLogic.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteDetails(pk);
                // Create object for File
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                AST_DOC_MODE.Value = "0";

            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                    {
                        hdfTransactionData.Value = dt.Rows[0]["appPK"].ToString();
                        FillGINDetails(Convert.ToInt32(dt.Rows[0]["appPK"]));
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
            BusinessObject.StoreManagement.GoodsIssueNote goodsIssueNote = null;
            // Create object for GoodsIssueNote
            goodsIssueNote = new BusinessObject.StoreManagement.GoodsIssueNote();
            goodsIssueNote.GINList = new List<BusinessObject.StoreManagement.GoodsIssueNoteList>();
            GINList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(goodsIssueNote);
            // Create object for User
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // Create object for File
            BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
            file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
            FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
            // Fill GIN NO 
            FillPurchseRequestNo(objUser);
            AST_DOC_MODE.Value = GetDOCMODE();

        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.GIN, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// get configuration value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("GINDOEDays", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DoeDifrDays.Value = dt.Rows[0]["ACF_DATA"].ToString();
                IsReqDoeValidation.Value = dt.Rows[0]["ACF_VALUE"].ToString();

            }
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0"; 
        }

        /// <summary>
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
            FillProcessId(true);
            ConfigurationSettings();
            int refId = 0;
            int appId = 0;
            int prefID = 0;
            // Check Querystring have PK Value
            if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.PKVALUE] != null)
            {
                // Fill GIN Details in Edit Mode
                FillGINDetails(Convert.ToInt32(Request.QueryString[GTIService.Constants.StockTransfer.Fields.PKVALUE].ToString()));
            }
            // Check Querystring no REFID
            else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] == null)
            {
                // Fill GIN Details
                FillGINDetails(0);
            }
            // Check Querystring have REFID
            else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] != null)
            {
                // Assign RefID 
                ucrWrkf.RefID = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                refId = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                // get Application details by RefID
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        // Assign AppID
                        appId = Convert.ToInt32((dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID] == DBNull.Value) ? 0 : dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID]);
                    }
                }
                // Fill GIN Details By appID
                FillGINDetails(appId);
                // Fill workflow details
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
            }
            if (Request.QueryString["PRefID"] != null)
            {
                IsPrefID.Value = "1";
                prefID = int.Parse(Request.QueryString["PRefID"]);
                hdfRefID.Value = prefID.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(prefID);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        string appID = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                        GRH_PK.Value = appID;
                        DataTable dtStore = BusinessLogic.StoreManagement.GoodsInspectionNote.GetGrnStore(Convert.ToInt32(appID));
                        if (dtStore != null && dtStore.Rows.Count > 0)
                            hdfGrnStore.Value = dtStore.Rows[0]["GRH_DEPT"].ToString();
                        else
                            hdfGrnStore.Value = "0";
                    }
                }
                //
            }
            // Check Querystring have Status Value 
            if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.STATUS] != null)
            {
                // Set Hide Workflow USer control section visible as false
                ucrWrkf.ViewType = 0;
                // 
                if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] != null)
                {
                    ucrWrkf.RefID = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnSubmit.Visible = false;
                }
                else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.STATUS] == GTIService.Constants.StockTransfer.Fields.VALUETWO)
                {
                    int procId = GetGINProcessID();
                    WorkFlowDetails wrkFlowDtls = CommonBL.GetWorkflowDetails(int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.PKVALUE].ToString()), procId);
                    if (wrkFlowDtls != null)
                    {
                        ucrWrkf.RefID = wrkFlowDtls.ReferenceID;
                        ucrWrkf.ViewType = Request.QueryString[GTIService.Constants.StockTransfer.Fields.STATUS].ToString() == GTIService.Constants.StockTransfer.Fields.VALUETWO ? 0 : 0;

                    }
                    else
                    {
                        ucrWrkf.RefID = 0;
                        ucrWrkf.ViewType = 0;
                    }
                    ucrWrkf.FillWorkFlowDetails(true);
                    btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }


                btnSave.Visible = false;

            }
            else
            {
                // Check Querystring have Flag Value 
                if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.FLAG] != null)
                {
                    // set workflow action section set as hide
                    ucrWrkf.ViewType = 0;
                    btnSave.Visible = false;
                }
                else
                {
                    // set workflow action section set as show
                    ucrWrkf.ViewType = 1;
                }

                ucrWrkf.Visible = true;
                // FIll Process Details
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
        /// Method to get GIN Processs ID
        /// </summary>
        /// <returns></returns>
        private int GetGINProcessID()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            // set Path
            if (System.Configuration.ConfigurationManager.AppSettings[GTIService.Constants.StockTransfer.Fields.VIRTUALDIRECTORY].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings[GTIService.Constants.StockTransfer.Fields.VIRTUALDIRECTORY].ToLower(), GTIService.Constants.StockTransfer.Fields.STRINGEMPTY);
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
                return int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());

            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Method to Fill Purchase Request Number
        /// </summary>
        /// <param name="objUser"></param>
        private void FillPurchseRequestNo(BusinessObject.User objUser)
        {
            //string ginNo;
            //// Get GIn Format , by GINNO
            //DataTable dtGRNNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, GTIService.Constants.StockTransfer.Fields.GIN_NO, 0);
            //if (dtGRNNoFormat.Rows.Count > 0)
            //    ginNo = dtGRNNoFormat.Rows[0][GTIService.Constants.StockTransfer.Fields.DEFAULTVALUE].ToString();
            //else
            //    ginNo = GTIService.Constants.Common.CommonConstant.GINNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(ginNo, GTIService.Constants.StockTransfer.Fields.NUMBERFORMATEXPR);
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        ginNo = ginNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace(GTIService.Constants.StockTransfer.Fields.HASHVALUE, string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        // Create GINNo
            //        ginNo = ginNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.GoodsInspectionNote.GetGINO());
            //    }

            //}
            // Assign ginNo, to label
            GIH_NO.Text = Resources.Messages.DocGenerationNew;
            // Assign Date
            if (Request.QueryString["PRefID"] == null)//From Inbox show the GRN Date as GINDate By Default.           
                GIH_DATE.Text = DateTime.Now.ToString(GTIService.Constants.StockTransfer.Fields.DATEFORMAT);
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
            // Check App id, or Details Saved Success
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? GTIService.Constants.StockTransfer.Fields.ZEROVALUE : hdfAppID.Value) > 0)
            {
                // Assign ApID
                ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                // Do Workflow and assign refid,
                refId = ucrWrkf.DoWorkFlow();
            }
            // check refid > 0, then ssave success and redirect to listing page
            if (refId > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'>ShowWorkflowSaveMsg();</script>", false);
            }


        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId(bool SetProcessID = false)
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[GTIService.Constants.StockTransfer.Fields.VIRTUALDIRECTORY].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings[GTIService.Constants.StockTransfer.Fields.VIRTUALDIRECTORY].ToLower(), GTIService.Constants.StockTransfer.Fields.STRINGEMPTY);
            else
                path = Request.Url.AbsolutePath.ToLower();

            path += "?TYPE=" + hdfType.Value;
            base.WkfPageUrl = path;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
            }
            else
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    // pass proc Id to wrkflw user control and fill action details 
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString();
                    ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PAGEPK].ToString();
                    // Fill workflow details
                    ucrWrkf.FillWorkFlowDetails(true);
                }
        }
        //===========================##### END Code For WorkFolw ###### =======================================

    }
}