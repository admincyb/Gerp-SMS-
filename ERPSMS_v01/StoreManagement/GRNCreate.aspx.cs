using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using BusinessObject.CommonManagement;
using BusinessLogic.CommonManagement;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class GRNCreate : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                APT_CODE.Value = ApplicationType.GRN;
                hdfAppType.Value = ApplicationType.PO;
                hdfAppSubType.Value = string.Empty;
                FillInitialData();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
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
        /// get configuration value
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("GRNAdditionalQty", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                orderPercentage.Value = dt.Rows[0]["ACF_DATA"].ToString();
            }

            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();

            DataTable dtPOPrint = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("GRN PO PRINT", string.Empty, currentUser.SBUID);
            if (dtPOPrint != null && dtPOPrint.Rows.Count > 0)
            {
                hfPOPrint.Value = dtPOPrint.Rows[0]["ACF_VALUE"].ToString();
            }
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0"; 
        }


        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillGoodsReceiptDetails(int pk)
        {
            BusinessObject.StoreManagement.GoodsReceiptNote goodsReceiptNote = null;
            if (pk != 0)
            {
                GRNList.Value = BusinessLogic.StoreManagement.GoodsReceiptNote.GetGoodsReceiptNoteDetails(pk);
                // File Uploader start
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                // End FileUploader
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                        FillGoodsReceiptDetails(Convert.ToInt32(dt.Rows[0]["appPK"]));
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
            BusinessObject.StoreManagement.GoodsReceiptNote goodsReceiptNote = null;
            goodsReceiptNote = new BusinessObject.StoreManagement.GoodsReceiptNote();
            goodsReceiptNote.GRNList = new List<BusinessObject.StoreManagement.GoodsReceiptNoteList>();
            GRNList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(goodsReceiptNote);
            // File Uploader start
            BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
            file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
            FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
            // End FileUploader
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FillGoodsReceiptNo(objUser);
            AST_DOC_MODE.Value = GetDOCMODE();
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.GRN, 0, DateTime.Now);
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
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
            FillProcessId(true);
            int refId = 0;
            int prefID = 0;
            int appId = 0;
            ConfigurationSettings();
            if (Request.QueryString["PK"] != null)
            {
                FillGoodsReceiptDetails(Convert.ToInt32(Request.QueryString["PK"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillGoodsReceiptDetails(0);
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                hdfRefID.Value = refId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                FillGoodsReceiptDetails(appId);
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
                        PO_PK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                    }
                }
                //
            }
            if (Request.QueryString["Status"] != null)
            {
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null)
                {
                    ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnSubmit.Visible = false;
                }
                else if (Request.QueryString["Status"] == "2")
                {
                    int procId = GetGRNProcessID();
                    WorkFlowDetails wrkFlowDtls = CommonBL.GetWorkflowDetails(int.Parse(Request.QueryString["PK"].ToString()), procId);
                    if (wrkFlowDtls != null)
                    {
                        ucrWrkf.RefID = wrkFlowDtls.ReferenceID;
                        ucrWrkf.ViewType = Request.QueryString["Status"].ToString() == "2" ? 0 : 0;

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
                if (Request.QueryString["Flag"] != null)
                {
                    ucrWrkf.ViewType = 0;
                    btnSave.Visible = false;
                }
                else
                {
                    ucrWrkf.ViewType = 1;
                }
                ucrWrkf.Visible = true;
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
        /// Method to get GRN Processs ID
        /// </summary>
        /// <returns></returns>
        private int GetGRNProcessID()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            //DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            DataTable dtProcess;
            if (Session[SessionStrings.CurDept] != null)
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[SessionStrings.CurDept]));
            else
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                return int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());

            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillGoodsReceiptNo(BusinessObject.User objUser)
        {
            //string grnNo;
            //DataTable dtGRNNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "GRNNO", 0);
            //if (dtGRNNoFormat.Rows.Count > 0)
            //    grnNo = dtGRNNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    grnNo = GTIService.Constants.Common.CommonConstant.GRNNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(grnNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        grnNo = grnNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        grnNo = grnNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRN());
            //    }

            //}
            GRH_NO.Text = Resources.Messages.DocGenerationNew;
            //GRH_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

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
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? "0" : hdfAppID.Value) > 0)
            {
                ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                refId = ucrWrkf.DoWorkFlow();
            }
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
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess;
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
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
                    ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                    // Fill workflow details
                    ucrWrkf.FillWorkFlowDetails(true);
                }
        }

    }
}