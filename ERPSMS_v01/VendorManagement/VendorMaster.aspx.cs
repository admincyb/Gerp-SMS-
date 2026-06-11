using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using WorkflowCore;
using System.Data;
using BusinessLogic.CommonManagement;
using BusinessObject.CommonManagement;
using BusinessObject.Common;
using ERPService;
using ERPData;

namespace ERPSMS_v01.VendorManagement
{
    public partial class VendorMaster : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                hdfBaseCurrency.Value = currentUser.BaseCurrency.ToString();
                FillInitialData();
                ConfigurationSettings();
                APT_CODE.Value = ApplicationType.VND;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ERP.Utilities.CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            AST_DOC_MODE.Value = GetDOCMODE();
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            hdfIncludeFG.Value = GetGlobalResourceObject("ConfigurationsRes", "IncludeFG").ToString(); 
            VENCODE_AUTO.Value = GetGlobalResourceObject("ConfigurationsRes", "VenCodeAutoGen").ToString();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }

            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsLineitemTax.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                hdfIsLineitemDiscount.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }

            if (Convert.ToInt32(VENCODE_AUTO.Value) == 1)
            {
                VEN_CODE.Enabled = false;
                VEN_CODE.CssClass += " input-disabled"; 
            }

            hdfResetCurrencyOnType.Value = GetGlobalResourceObject("ConfigurationsRes", "ResetCurrencyOtherThanLocal").ToString();//Bug ID:35014:The currency changing functionality on changing type is not required for EKK.
            hdfIsGstEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableGST").ToString();
            hdfIsCurrencyWithTypeValidnReqd.Value = GetGlobalResourceObject("ConfigurationsRes", "CurrencyWithTypeValidationRequired").ToString();
            hdfIsShowESINo.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowESINo").ToString();
            hdfIsShowPFNo.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowPFNo").ToString();
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.VND, 0, DateTime.Now);
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
        /// Function Used to fill Initial Rendering of the Screen
        /// </summary>
        /// <param name=""></param>        
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;
            BizUnitPk.Value = currentUser.SBUID.ToString();
            if (Request.QueryString["VendorID"] != null)
            {
                FillVendorDetails(Convert.ToInt32(Request.QueryString["VendorID"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillVendorDetails(0);
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
                FillVendorDetails(appId);
                ucrWrkf.FillWorkFlowDetails(true);
                TaskID.Value = ucrWrkf.TaskID.ToString();
                //btnSave.Visible = false;
            }
            if (Request.QueryString["Status"] != null)
            {
                if (Request.QueryString["Status"].ToString() == "1")
                {
                    BankSave.Visible = BankClear.Visible = false;
                    hdfViewMode.Value = 1.ToString();
                }
                else { hdfViewMode.Value = 0.ToString(); }
                ucrWrkf.ViewType = 0;
                if (Request.QueryString["RefID"] != null)
                {
                    //btnSubmit.Visible = false;
                    ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                    ucrWrkf.FillWorkFlowDetails(true);
                    TaskID.Value = ucrWrkf.TaskID.ToString();
                    Button btnWkfSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    btnWkfSubmit.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (Request.QueryString["VendorID"] != null)
                {
                    int procId = GetVendorRegProcessID();
                    WorkFlowDetails wrkFlowDtls = CommonBL.GetWorkflowDetails(int.Parse(Request.QueryString["VendorID"].ToString()), procId);
                    if (wrkFlowDtls != null)
                    {
                        ucrWrkf.RefID = wrkFlowDtls.ReferenceID;
                        ucrWrkf.ViewType = Request.QueryString["Status"].ToString() == "2" ? 0 : 0;
                        //btnSubmit.Visible = Request.QueryString["Status"].ToString() == "2" ? false : true;

                    }
                    else
                    {
                        ucrWrkf.RefID = 0;
                        ucrWrkf.ViewType = 0;
                    }
                    ucrWrkf.FillWorkFlowDetails(true);
                    //btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.Visible = false;
                }

            }
            else
            {

                ucrWrkf.Visible = true;
                // For Completed Task
                if (Request.QueryString["Flag"] != null)
                {
                    ucrWrkf.ViewType = 0;
                }
                else
                {
                    ucrWrkf.ViewType = 1;
                }
                FillProcessId();
            }


            // For Completed task


        }

        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillVendorDetails(int vendorID)
        {




            if (vendorID != 0)
            {

                VendorDetails.Value = BusinessLogic.VendorManagement.VendorRegistration.GetVendorDetails(vendorID);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                hdfVendorPK.Value = vendorID.ToString();
            }
            else
            {
                BusinessObject.VendorManagement.Vendor vendorObject = new BusinessObject.VendorManagement.Vendor();
                vendorObject.AddressBookDetails = new List<BusinessObject.VendorManagement.VendorAddressBook> { };
                vendorObject.MaterialDetails = new List<BusinessObject.VendorManagement.MaterialDetails> { };
                vendorObject.TermsDetails = new List<BusinessObject.VendorManagement.VendorTerms> { };
                //NewSamples Start
                vendorObject.MaterialSamples = new List<BusinessObject.VendorManagement.MaterialSamples> { };
                //New End
                //Assigning initialized VendorObject to hidden field (VendorDetails)
                VendorDetails.Value = Newtonsoft.Json.JsonConvert.SerializeObject(vendorObject);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
            }
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
                Response.Redirect("VendorListing.aspx?No=" + hdfAppID.Value);
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
                TaskID.Value = ucrWrkf.TaskID.ToString();
            }
        }


        private int GetVendorRegProcessID()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
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
        //===========================##### END Code For WorkFolw ###### =======================================



    }
}
