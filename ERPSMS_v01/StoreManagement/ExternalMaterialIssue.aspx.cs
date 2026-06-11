using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using BusinessObject.Common;
using ERPData;
using ERPService;
using BusinessObject.CommonManagement;
using System.Threading;
using BusinessObject.AccountManagement;
using BusinessLogic.StoreManagement;

namespace ERPSMS_v01.StoreManagement
{
    public partial class ExternalMaterialIssue : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        public string IssuingAgainst = string.Empty;
        public string MaterialIssueNo = string.Empty;
        public string IssueTo = string.Empty;
        public string IssuingStore = string.Empty;        
        public string QtyIssued = string.Empty;     
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["TYPE"] != null)
                {
                    transactionType.Value = Request.QueryString["TYPE"].ToString();
                }
                SetResourse(transactionType.Value);
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                FillInitialData();
                ConfigurationSettings();
                // ICH_DEPT.Value = currentUser.CurrentDeptPK.ToString();
                if (Convert.ToInt16(transactionType.Value) == 3)
                {
                    APT_CODE.Value = ApplicationType.MRT;
                }
                else
                {
                    APT_CODE.Value = ApplicationType.EMI;
                }
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                base.WkfPageUrl = hdfPageURL.Value = GetGlobalResourceObject("PageURL", "EMIPageURL").ToString() + "?TYPE=" + transactionType.Value;

                //string redirectURL = "../login.aspx";
                //if (!base.HasPageRight())
                //{
                //    Session.Abandon();
                //    System.Web.Security.FormsAuthentication.SignOut();
                //    if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                //    {
                //        redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                //    }
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                //    return;
                //}                
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (transactionType.Value == "1")
                {
                    btnSaveSubmit.Visible = false;
                }
                if (transactionType.Value == "7")
                {
                    btnSaveandSubmit.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

         /// <summary>
         /// Set Label based on Type
         /// </summary>
         /// <param name="type"></param>
        private void SetResourse(string type)
        {
            switch (type)
            { 
                case "1":
                case "7":
                    IssuingAgainst = Resources.Controls.IssueAgainst;
                    MaterialIssueNo = Resources.Controls.MaterialIssueNo;
                    IssueTo = Resources.Controls.IssueTo;
                    QtyIssued = Resources.Controls.QtyIssued;
                    IssuingStore = Resources.Controls.IssuingStore;
                    break;
                case "3":
                    IssuingAgainst = Resources.Controls.ReturnAgainst;
                    MaterialIssueNo = Resources.Controls.MaterialReturnNo;
                    IssueTo = Resources.Controls.ReturnTo;
                    QtyIssued = Resources.Controls.QtyReturned;
                    IssuingStore = Resources.Controls.ReturningStore;                    
                    break;
            }
        }
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;

            if (Request.QueryString["IssueID"] != null)
            {
                FillRequisitionData(Convert.ToInt32(Request.QueryString["IssueID"].ToString()));
            }
            else if (Request.QueryString["CrDr"] != null)
            {
                FillDatasFromCreditNote(Convert.ToInt32(Request.QueryString["CrDr"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillRequisitionData(0);
            }

            if (Request.QueryString["RefID"] != null)
            {
                //hdfRefID.Value = refId.ToString();
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        appId = Convert.ToInt32((dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? 0 : dtApplication.Rows[0]["refApplication"]);
                        hdfAppID.Value = appId.ToString();
                    }
                }
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
                FillRequisitionData(appId);
            }
            //if (Request.QueryString["Flag"] != null)
            //{
            //    ucrWrkf.ViewType = 0;
            //    btnSave.Visible = false;
            //}
            //else
            //{
            //    ucrWrkf.ViewType = 1;
            //}
            FillProcessId();
            if (ucrWrkf.HasActions)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
            }

            ucrWrkf.Visible = true;

            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
        }

        private void FillProcessId()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "7")
            {
                path += "?Type=" + Request.QueryString["Type"].ToString();
                base.WkfPageUrl = path;
            }
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

        private void FillRequisitionData(int materialIssueID)
        {
            if (materialIssueID != 0)
            {
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetMaterialIssueDetails(materialIssueID);
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                BusinessObject.StoreManagement.ExternalMaterialIssue MaterialIssueObject = new BusinessObject.StoreManagement.ExternalMaterialIssue();
                MaterialIssueObject.MaterialIssueDetailsList=new List<BusinessObject.StoreManagement.ExternalMaterialIssueDetails>();
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = Newtonsoft.Json.JsonConvert.SerializeObject(MaterialIssueObject);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillSRSNumber(objUser);
                AST_DOC_MODE.Value = GetDOCMODE();
            }
        }

        private void FillDatasFromCreditNote(int crdrPK)
        {
            if (crdrPK != 0)
            {
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetDetailsFromCRDR(crdrPK);
                AST_DOC_MODE.Value = GetDOCMODE();
            }           
        }

        private void ConfigurationSettings0()
        {
           
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.EMI, 0, DateTime.Now);
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
        ///Methord used to Fill SRSno Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillSRSNumber(BusinessObject.User objUser)
        {
            //string srsNo;
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "EISNO", 0);
            //if (dtSrsNoFormat.Rows.Count > 0)
            //    srsNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    srsNo = GTIService.Constants.Common.CommonConstant.SRSNUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(srsNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetICHNo(currentUser.SBUID,1));
            //    }

            //}
            ICH_NO.Value = "";
            lblMaterilaConsumptionNo.Text = Resources.Messages.DocGenerationNew;

        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SRSStockValidation", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdnIsNeededStockValidation.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            //For autocomplete search min length
            AutoStartValue.Value = GetGlobalResourceObject("ConfigurationsRes", "AutoCompleteLimit").ToString();
            hdfEmptyEMIDate.Value = GetGlobalResourceObject("ConfigurationsRes", "EmptyEMIDate").ToString(); //For EMI Date set Null Or Default Date
            hdfLoadFromGRN.Value = GetGlobalResourceObject("ConfigurationsRes", "LoadGRNItemsInEMI").ToString();
            hdfGRNLimitTextLength.Value = GetGlobalResourceObject("ConfigurationsRes", "GRNLimitTextLength").ToString();
            if (hdfLoadFromGRN.Value == "1" && transactionType.Value == "1")
                divLoadFromGRN.Visible = true;
            //dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            //}
            // Get Negative Stock Allow Flag
            dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("INVENTORY SETTINGS", "EnableNegativeStk", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfAllowNegativeStock.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            // Get Batch Allow Flag
            dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("INVENTORY SETTINGS", "EnableStockBatch", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfEnableBatch.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            else
            {
                hdfEnableBatch.Value = "0";
            }
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfAddCommentMandValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "AddCommentMandValidation").ToString();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb Material Return
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "3")
                {
                    if (this.GetLocalResourceObject("BreadcrumbMaterialReturn") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbMaterialReturn").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("MaterialReturnTitle").ToString();
                    }
                }
                else if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "7")
                {
                    if (this.GetLocalResourceObject("BreadcrumbFormer") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbFormer").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("BreadcrumbFormer").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        
    }
}