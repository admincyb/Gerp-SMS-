using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StoreAuditing : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private DataTable dtCompany;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // action for workflow submit
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                // Assign User Details 
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Fill initial data in page load event
                FillInitialData();
                ConfigurationSettings();
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillStoreAudit(int pk)
        {
            // Assign StoreAudit as Null
            BusinessObject.StoreManagement.StoreAudit storeAudit = null;
            if (pk != 0)
            {
                // check pk !=0, then get store audit details and assign to hidden filed
                ItemList.Value = BusinessLogic.StoreManagement.StoreAudit.GetStoreAuditDetails(pk);
            }
            else
            {
                // Create object for StoreAudit, StoreMaterialDtls, DamageDetailsDtls, objUser
                storeAudit = new BusinessObject.StoreManagement.StoreAudit();
                storeAudit.ItemList = new List<BusinessObject.StoreManagement.StoreMaterialDtls>();
                storeAudit.DamageStock = new List<BusinessObject.StoreManagement.DamageDetailsDtls>();
                ItemList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(storeAudit);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Set Store Audit Number
                FillStoreAuditNo(objUser);
                // Assign Date
                SAHTXT_DATE.Text = System.DateTime.Now.ToString(GTIService.Constants.StockTransfer.Fields.DATEFORMAT);               
                SAH_DATE.Value = System.DateTime.Now.ToString(GTIService.Constants.StockTransfer.Fields.DATEFORMAT);
                
            }
        }

        /// <summary>
        /// Funtion used fill Fill Initial Data
        /// </summary>
        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;
            // Chekc Query string have PK value
            if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.PKVALUE] != null)
            {
                // Then fill store audit details by PK
                FillStoreAudit(Convert.ToInt32(Request.QueryString[GTIService.Constants.StockTransfer.Fields.PKVALUE].ToString()));
            }
                // Check querystring have no RefID, then sset pk as 0 and fill store audit details
            else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] == null)
            {
                FillStoreAudit(0);
            }
                // check query string have RefiD
            else if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] != null)
            {
                // Assign RefID
                ucrWrkf.RefID = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                refId = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                // Get Application details by RefID
                DataTable dtApplication = wrkfService.GetApplicationID(refId);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        // Assign APPlication ID
                        appId = Convert.ToInt32((dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID] == DBNull.Value) ? 0 : dtApplication.Rows[0][GTIService.Constants.StockTransfer.Fields.APPID]);
                    }
                }
                // Fill Store Audit Details by AppID
                FillStoreAudit(appId);
                // Fill Workflow Details
                ucrWrkf.FillWorkFlowDetails(true);
                // Hide Save Button
                btnSave.Visible = false;
            }
            // Check querystring have Status 
            if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.STATUS] != null)
            {
                // Set Worklflow Action section visible as false
                ucrWrkf.ViewType = 0;
                // Check querystring have RefID
                if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE] != null)
                {
                    //btnSubmit.Visible = false;
                    // Assign RefID
                    ucrWrkf.RefID = int.Parse(Request.QueryString[GTIService.Constants.StockTransfer.Fields.REFIDVALUE]);
                    // Fill WorkFlow Details
                    ucrWrkf.FillWorkFlowDetails(true);
                    Button btnWkfSubmit = (Button)ucrWrkf.FindControl("btnSubmitWrkf");
                    // Hide Submit Button
                    btnWkfSubmit.Visible = false;
                }
                else
                {
                    // set user control visible as fasle
                    ucrWrkf.Visible = false;
                }
                // set save button visibility as false
                btnSave.Visible = false;
            }
            else
            {
                // Check querystring have falge value
                if (Request.QueryString[GTIService.Constants.StockTransfer.Fields.FLAG] != null)
                {
                    // set workflow control action visible as false
                    ucrWrkf.ViewType = 0;
                    // hide save button
                    btnSave.Visible = false;
                }
                else
                {
                    // set usercontrol action section visible as true
                    ucrWrkf.ViewType = 1;
                }
               // fill usercontrol visible as true
                ucrWrkf.Visible = true;
                // fill processID
                FillProcessId();
            }
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
        }

        /// <summary>
        /// fill Store Audit Number
        /// </summary>
        /// <param name="objUser"></param>
        private void FillStoreAuditNo(BusinessObject.User objUser)
        {
            //string sANo;
            //// Get Atore Audit Number Format
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "SANO", 0);
            //if (dtSrsNoFormat.Rows.Count > 0)
            //    sANo = dtSrsNoFormat.Rows[0][GTIService.Constants.StockTransfer.Fields.DEFAULTVALUE].ToString();
            //else
            //    sANo = GTIService.Constants.Common.CommonConstant.SANUMBERFORMAT;
            //// Set store audit number with format
            //MatchCollection matchcol = Regex.Matches(sANo, GTIService.Constants.StockTransfer.Fields.NUMBERFORMATEXPR);
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        sANo = sANo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace(GTIService.Constants.StockTransfer.Fields.HASHVALUE, string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        sANo = sANo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.StoreAudit.GetStoreAuditNO());
            //    }

            //}
            // Assign Store Audit Number
            SAHTXT_NO.Text = Resources.Messages.DocGenerationNew;
           // SAH_NO.Value = Resources.Messages.DocGenerationNew;
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
            // Set AppId to user Control Property
            ucrWrkf.ApplicationID = Convert.ToInt32(Convert.ToInt32(hdfAppID.Value));
            // Get refid after DoWorkflow Action
            int refId = ucrWrkf.DoWorkFlow();

            // Check RefID >0 , then save success and Redirect to listing page
            if (refId > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "wrkfMsg", "<script type='text/javascript'> $(document).ready(function () { ShowWorkflowSaveMsg(); });</script>", false);
            }

        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId()
        {
            // if process id not passing, initialize processid and assign page id to master page hidden fileds
            string path = string.Empty;
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
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString();
                ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PAGEPK].ToString();
                // Fill workflow details
                ucrWrkf.FillWorkFlowDetails(true);
            }
        }
        //===========================##### END Code For WorkFolw ###### =======================================

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {                  
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

            //For autocomplete search min length
            AutoStartValue.Value = GetGlobalResourceObject("ConfigurationsRes", "AutoCompleteLimit").ToString();

        }
       
    }
}