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
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StoreRequisitionSlipCreation : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        DataTable dtCompany;
        #endregion       
        protected void Page_Load(object sender, EventArgs e)
        {
            ucrWrkf.WrkfSubmit += new EventHandler(WrkfSubmit);
            if (!IsPostBack)
            {
                if (Request.QueryString["Type"] != null) //for SBU Store request (Type=2)
                {
                    hdfType.Value = Request.QueryString["Type"];
                }
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                hdfItemTypes.Value = ((int)ITEMTYPE.StockItem).ToString();
                //hdfCompoundStore.Value = ((int)Store.CompoundStore).ToString();
                //hdfInvStore.Value = ((int)Store.InventoryStore).ToString();
                GetUserRights();
                FillInitialData();
                ConfigurationSettings();
                APT_CODE.Value = ApplicationType.SRS;
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                //Department Session Expired

                //string path = string.Empty;
                //if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                //    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                //else
                //    path = Request.Url.AbsolutePath.ToLower();
                //base.WkfPageUrl = path;// +"?TYPE=1";

                if (lblSRS.Text == "[NEW]")
                {
                    btnSave.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                }

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

        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;

            

            if (Request.QueryString["RequisitionID"] != null)
            {
                FillRequisitionData(Convert.ToInt32(Request.QueryString["RequisitionID"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillRequisitionData(0);
            }
            else if (Request.QueryString["RefID"] != null)
            {
                ucrWrkf.RefID = int.Parse(Request.QueryString["RefID"]);
                refId = int.Parse(Request.QueryString["RefID"]);
                hdfRefID.Value = refId.ToString();
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
                FillRequisitionData(appId);
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

            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }

            if (Request.QueryString["IsModify"] != null && hdnModifyMR.Value == "1")
            {
                btnSave.Visible = true;
            }


        }

        private void GetUserRights()
        {
            BusinessLogic.CommonManagement.CommonBL userAuth = new BusinessLogic.CommonManagement.CommonBL();
            string PageUrl = Resources.PageURL.StoreRequisitionSlipURL;
            if (hdfType.Value == "2")
            {
                PageUrl = PageUrl + "?Type=" + hdfType.Value;
            }
            UserRightsBO usrRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, PageUrl, currentUser.CurrentDeptPK, 0);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true)
                    {
                        hdnModifyMR.Value = "1";
                        break;
                    }                   
                }

            }

        }

        private void FillRequisitionData(int requisitionID)
        {
            if (requisitionID != 0)
            {
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                RequisitionDetailsList.Value = BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetRequisitionDetails(requisitionID);
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                BusinessObject.StoreManagement.StoreRequisitionSlipCreation RequisitionObject = new BusinessObject.StoreManagement.StoreRequisitionSlipCreation();
                RequisitionObject.RequisitionDetailsList = new List<BusinessObject.StoreManagement.StoreRequisitionSlipCreationDetail> { };
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                RequisitionDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(RequisitionObject);
                AST_DOC_MODE.Value = GetDOCMODE();
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillSRSNumber(objUser);
            }


        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.SRS, 0, DateTime.Now);
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
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "SRSNO", 0);
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
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSRSNo());
            //    }

            //}
            MRH_NO.Value = "";
            lblSRS.Text = Resources.Messages.DocGenerationNew;
            MRH_SUBMITTED_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");

        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SRSStockValidation", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdnIsNeededStockValidation.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            //For autocomplete search min length
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            //To get Inventory Department PK
            var invParam = new { active = true, parent = 0, type = 2, category = 0 };
            dt = BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetInvStoreDepartment(invParam.active, invParam.parent, invParam.type, invParam.category);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfInvStore.Value = dt.Rows[0][GTIService.Constants.Store.Parametes_Department.DPT_PK].ToString();
            }
            //To get Compound Department PK
            var cmpParam = new { active = true, parent = 2, type = 2, category = 2 };
            dt = BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetInvStoreDepartment(cmpParam.active, cmpParam.parent, cmpParam.type, cmpParam.category);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfCompoundStore.Value = dt.Rows[0][GTIService.Constants.Store.Parametes_Department.DPT_PK].ToString();
            }
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfMaterialWithStock.Value = GetGlobalResourceObject("ConfigurationsRes", "MaterialWithStock").ToString();
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

            if (hdfType.Value == "2")
            {
                path = path + "?Type=" + hdfType.Value;
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


        //===========================##### END Code For WorkFolw ###### =======================================




    }
}