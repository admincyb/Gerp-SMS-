using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using ERPData;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject.Common;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERPManager;
using System.Threading;

namespace ERPSMS_v01.Inventory
{
    public partial class MRissue : ERP.Store.UI.MyBasePage
    {
        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private DataTable dtCompany;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);            
            if (!IsPostBack)
            {
                ConfigurationSettings();
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";

                divRequestedBy.Visible = false;
                if (Request.QueryString["TYPE"] != null && Convert.ToInt32(Request.QueryString["TYPE"]) > 0)
                {
                    hdfMenuType.Value = Request.QueryString["TYPE"];
                    if (hdfMenuType.Value == "9")
                        divRequestedBy.Visible = true;
                }

                ICH_BASE_CURR.Value = currentUser.BaseCurrency.ToString();
                FillInitialData();
                APT_CODE.Value = ApplicationType.MTI;
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                //Department Session Expired
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

        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfStockTypeValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableMRStockitemValidation").ToString();
            hdfHasJournalEntryImport.Value = GetGlobalResourceObject("ConfigurationsRes", "HasJournalEntryImport").ToString();
            hdfShowInvestor.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor").ToString();
        }

        public void AssignLocalBreadCrumb()
        {
            try
            {
                string _breadCrumb = string.Empty;
                string breadCrumb = string.Empty;
                if (GetGlobalResourceObject("ConfigurationsRes", "HasPlantwiseBreadcrumb").ToString() == "1")
                {
                    string plant = string.Empty;
                    if (currentUser.CurrentDept.Split('-').ElementAtOrDefault(1) != null)
                        plant = currentUser.CurrentDept.Split('-')[1].ToString().Trim();

                    if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() == "9")
                        _breadCrumb = string.Format(this.GetLocalResourceObject("BreadcrumbP2P_PName").ToString(), plant);
                    else
                        _breadCrumb = string.Format(this.GetLocalResourceObject("Breadcrumb").ToString(), plant);

                    breadCrumb = _breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    lblBreadCrum.Text = breadCrumb;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillMaterialIssue(int pk)
        {
            BusinessObject.StoreManagement.MRIssue materialIssue = null;
            if (pk != 0)
            {
                MIList.Value = BusinessLogic.StoreManagement.MaterialIssue.GetMRIssueDetails(pk);
                //string tt = MIList.Value.Substring(MIList.Value.IndexOf("MIH_COMPANY:")+1);
                //tt = tt.Substring(0,tt.IndexOf(',') + 1);

                AST_DOC_MODE.Value = "0";
            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                        FillMaterialIssue(Convert.ToInt32(dt.Rows[0]["appPK"]));
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
            BusinessObject.StoreManagement.MRIssue materialIssue = null;
            materialIssue = new BusinessObject.StoreManagement.MRIssue();
            materialIssue.MIList = new List<BusinessObject.StoreManagement.MRIssueList>();
            MIList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(materialIssue);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FillMINo(objUser);
            AST_DOC_MODE.Value = GetDOCMODE();
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.MTI, 0, DateTime.Now);
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
            //start
            int refId = 0;
            int appId = 0;
            int prefID;
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            if (Request.QueryString["MIPK"] != null)
            {
                FillMaterialIssue(Convert.ToInt32(Request.QueryString["MIPK"].ToString()));
            }

            else if (Request.QueryString["RefID"] == null)
            {
                FillMaterialIssue(0);
            }
            else if (Request.QueryString["RefID"] != null)
            {
                hdfRefID.Value = refId.ToString();
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                //DataTable dtApplication = wrkfService.GetApplicationID(refId);
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                    }
                }
                FillMaterialIssue(appId);
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
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
            //end
            //if (Request.QueryString["MIPK"] != null)
            //{
            //    FillMaterialIssue(Convert.ToInt32(Request.QueryString["MIPK"].ToString()));
            //}
            //else
            //{
            //    FillMaterialIssue(0);


            //}
            if (Request.QueryString["PRefID"] != null)
            {
                int prefId = int.Parse(Request.QueryString["PRefID"]);
                hdfRefID.Value = prefId.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(prefId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        string appID = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                        hdfApplicationID.Value = appID;
                        DataTable dtStore = BusinessLogic.StoreManagement.MaterialIssue.GetMRStore(Convert.ToInt32(appID));
                        if (dtStore != null && dtStore.Rows.Count > 0)
                        {
                            hdfSRSStore.Value = dtStore.Rows[0]["PRH_DEPT"].ToString();
                            hdfMRType.Value = dtStore.Rows[0]["PRH_TRX_TYPE"].ToString() == string.Empty ? "0" : dtStore.Rows[0]["PRH_TRX_TYPE"].ToString();
                        }
                        else
                            hdfSRSStore.Value = "0";
                    }
                }
            }

            //Modified on Aug-16-2017          --Sruthy H
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));

            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }

            // Get Batch Allow Flag
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("INVENTORY SETTINGS", "EnableStockBatch", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfEnableBatch.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            else
            {
                hdfEnableBatch.Value = "0";
            }
            if (Request.QueryString["IsModify"] != null)
            {
                ucrWrkf.ViewType = 0;
                btnSave.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillMINo(BusinessObject.User objUser)
        {
            //string miNo;
            //DataTable dtMINoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "MINO", 0);
            //if (dtMINoFormat.Rows.Count > 0)
            //    miNo = dtMINoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    miNo = GTIService.Constants.Common.CommonConstant.MINNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(miNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        miNo = miNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        miNo = miNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.MaterialIssue.GetMINO());
            //    }

            //}
            lblMINo.Text = Resources.Messages.DocGenerationNew;
            ICH_NO.Value = "";

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
            if (Convert.ToInt32(hdfAppID.Value == string.Empty ? "0" : hdfAppID.Value) > 0)
            {
                ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
                refId = ucrWrkf.DoWorkFlow();
            }

            if (refId > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'>$(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
            }


        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            if ((hdfMenuType.Value == "" ? 0 : Convert.ToInt32(hdfMenuType.Value)) > 0)
                path += "?TYPE=" + hdfMenuType.Value;

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
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
                ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                // Fill workflow details
                ucrWrkf.FillWorkFlowDetails(true);
            }
        }


        //===========================##### END Code For WorkFolw ###### =======================================   

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        Journalize();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + ERP.Utilities.CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {


            }
        }
        #endregion

        #region JOURNALIZE
        private void Journalize()
        {
            if (MIList.Value != string.Empty)
                FillMaterialIssue(Convert.ToInt32(Request.QueryString["MIPK"].ToString()));
            BusinessObject.StoreManagement.MRIssue obj = (BusinessObject.StoreManagement.MRIssue)Newtonsoft.Json.JsonConvert.DeserializeObject(MIList.Value);

            Session[ERP.Utilities.SessionStrings.CrDrType] = null;
            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
            Session[ERP.Utilities.SessionStrings.JournalMode] = null;

            //Journalize New sessions start
            Session[ERP.Utilities.SessionStrings.DrControls] = null;
            Session[ERP.Utilities.SessionStrings.CrControls] = null;
            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
            Session[ERP.Utilities.SessionStrings.AccountType] = null;
            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
            //Journalize New sessions End

          /*  ucrJournalize.TransactionType = ApplicationType.MIJ;
            Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.MIJ;
            ucrJournalize.TransactionPK = Convert.ToInt32(Request.QueryString["MIPK"].ToString());
            Session[ERP.Utilities.SessionStrings.TransactionPK] = Convert.ToInt32(Request.QueryString["MIPK"].ToString());
            ucrJournalize.JournalizePK = 0;
            Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
            Session[ERP.Utilities.SessionStrings.TransactionNo] = invoiceHeaderObj.IVH_NO;
            Session[ERP.Utilities.SessionStrings.TransactionDate] = invoiceHeaderObj.IVH_DATE;
            Session[ERP.Utilities.SessionStrings.TransactionCurrency] = invoiceHeaderObj.IVH_CURRENCY;
            Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
            Session[ERP.Utilities.SessionStrings.AccountPayablePK] = invoiceHeaderObj.IVH_VENDOR;
            Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.MIJ;
            ucrWrkf.WrkfSubmit -= ActionHandler;
            ucrWrkf.Reset();
            ucrWrkf.ViewType = 1;
            FillProcessID(2);
            GetFieldValues(ControlsEnum.FINHEADER);
            //EntryStatus = EntryStatus.ENTRYMODE;
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
            {
                ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                base.WkfRefID = ucrWrkf.RefID;
            }
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
            {
                ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                ucrWrkf.ViewType = 1;
                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
            }
            else
            {
                ucrWrkf.ViewType = 0;
                //EntryStatus = EntryStatus.VIEWMODE;
                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
            }
            ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;


            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;

            ucrWrkf.ViewAction();

            HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
            hdfExchangeRateJV.Value = "";

            TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
            txtJournalExchangeRate.Text = "";

            TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
            txtNarration.Text = "";

            TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
            WrkfComments.Text = "";

            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
            hdfJournalizeWorkFlow.Value = "1";
            int numberGenerationSubType;
            if ((byte)POGroup == (byte)POInvoiceGroup.Services)
            {
                numberGenerationSubType = 0;// (int)AppSubTypeCNPurchase.NONSTOCK;
            }
            else
            {
                numberGenerationSubType = string.IsNullOrEmpty(hdfPOItemType.Value) ? (int)AppSubTypeCNPurchase.STOCK : Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ?
                    (int)AppSubTypeCNPurchase.NONSTOCK : (int)AppSubTypeCNPurchase.STOCK;
            }

            ucrJournalize.TypeForNumberGenaration = numberGenerationSubType.ToString();//((int)AppSubTypeCNPurchase.STOCK).ToString();
            ucrJournalize.CallUserControl();
            Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Purchase_Invoice_Journal").ToString();

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

            */
        }
        #endregion
    }
}