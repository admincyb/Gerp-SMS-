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
using ERPManager;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StoreAccept : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private DataTable dtCompany;
        #region Properties
        public int MenuType
        {
            get { return this.ViewState["MenuType"] == null ? 0 : (int)(this.ViewState["MenuType"]); }
            set { this.ViewState["MenuType"] = value; }
        }

         private int Type
        {
            get
            {
                return Convert.ToInt32(this.ViewState["Type"]);
            }
            set
            {
                this.ViewState["Type"] = value;
            }
        }
        #endregion
        #endregion

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
                //Breadcrumb WO
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1")
                {
                    if (this.GetLocalResourceObject("BreadcrumbWO") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbWO").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("WOTitle").ToString();
                    }
                }
                else
                {
                    if (this.GetLocalResourceObject("Breadcrumb") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("Title").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (Request.QueryString["Type"] != null)
                MenuType = Convert.ToInt32(Request.QueryString["Type"]);
            if (!IsPostBack)
            {
                ConfigurationSettings();
                if (Request.QueryString["Type"] == "1")
                {
                    DivChkPendingWO.Visible = true;
                }
                else
                {
                    DivChkPendingWO.Visible = false;
                }


                    FillProcessId();
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                FillInitialData();
                APT_CODE.Value = ApplicationType.MI;
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                hdfAppType.Value = ApplicationType.MI;
                hdfAppSubType.Value = string.Empty;

                if (lblMINo.Text == "[NEW]")
                {
                    btnSave.Visible = false;
                    btnPrint.Visible = false;
                }
                else
                {
                    btnPrint.Visible = true;
                    btnSave.Visible = true;
                }

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

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            if (Request.QueryString["Type"] != null)
            {
                if (Request.QueryString["Type"] == "1")
                {
                    SearchType.Items.Insert(1, new ListItem(Resources.BindValues.WihNo, "WIH_NO"));
                }
                else
                {
                    SearchType.Items.Insert(1, new ListItem(Resources.BindValues.SRNo, "MRH_NO"));
                }
            }
            else
                SearchType.Items.Insert(1, new ListItem(Resources.BindValues.SRNo, "MRH_NO"));

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfAllowExcessIssue.Value = GetGlobalResourceObject("ConfigurationsRes", "AllowExcessIssue").ToString();
        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillMaterialIssue(int pk)
        {
            BusinessObject.StoreManagement.MaterialIssue materialIssue = null;
            if (pk != 0)
            {
                MIList.Value = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialIssueDetails(pk);
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
            BusinessObject.StoreManagement.MaterialIssue materialIssue = null;
            materialIssue = new BusinessObject.StoreManagement.MaterialIssue();
            materialIssue.MIList = new List<BusinessObject.StoreManagement.MaterialIssueList>();
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
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.MI, 0, DateTime.Now);
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
                        DataTable dtStore = BusinessLogic.StoreManagement.MaterialIssue.GetRequestingStore(Convert.ToInt32(appID));
                        if (dtStore != null && dtStore.Rows.Count > 0)
                            hdfSRSStore.Value = dtStore.Rows[0]["MRH_DEPT"].ToString();
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
            MIHNO.Value = "";

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
            if (Request.QueryString["Type"] != null)
            {
                path += "?Type=" + Request.QueryString["Type"].ToString();
                hdfMenuType.Value = Request.QueryString["Type"].ToString();
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

         enum MICategoryTypeEnum
        {
            WO = 1
        }

        //===========================##### END Code For WorkFolw ###### =======================================

    }
}