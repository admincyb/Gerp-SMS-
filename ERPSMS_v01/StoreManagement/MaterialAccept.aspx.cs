using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERPService;
using ERPData;
using System.Threading;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialAccept : ERP.Store.UI.MyBasePage
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
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                ConfigurationSettings();
                hdfMIPk.Value = 0.ToString();
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GetUserRights();
                FillInitialData();
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                hdfAppType.Value = ApplicationType.STA;
                hdfAppSubType.Value = string.Empty;
                if (MAH_NO.Text == "[NEW]")
                {
                    btnSave.Visible = false;
                    btnPrint.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    btnPrint.Visible = true;
                }

            }
        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }

        /// <summary>
        /// Funtion used fill purchase request details
        /// </summary>
        /// <param name="pk"></param>
        private void FillMaterialAccept(int pk)
        {
            BusinessObject.StoreManagement.MaterialAccept materialAccept = null;
            if (pk != 0)
            {
                MaterialList.Value = BusinessLogic.StoreManagement.MaterialAccept.GetMaterialAccept(pk);
                AST_DOC_MODE.Value = "0";
            }
            else
            {
                if (Request.QueryString["PRefID"] != null)
                {
                    IsPrefID.Value = "1";
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(Request.QueryString["PRefID"]));
                    if (dt.Rows.Count > 0)
                        FillMaterialAccept(Convert.ToInt32(dt.Rows[0]["appPK"]));
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
            BusinessObject.StoreManagement.MaterialAccept materialAccept = null;
            materialAccept = new BusinessObject.StoreManagement.MaterialAccept();
            materialAccept.MaterialList = new List<BusinessObject.StoreManagement.MaterialAcceptList>();
            MaterialList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(materialAccept);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FillMaterialAccpetNo(objUser);
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
            int prefID = 0;
            hdfIsGoToInbox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString();
            if (Request.QueryString["PK"] != null)
            {
                FillMaterialAccept(Convert.ToInt32(Request.QueryString["PK"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillMaterialAccept(0);
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
                FillMaterialAccept(appId);
                ucrWrkf.FillWorkFlowDetails(true);
                btnSave.Visible = false;
            }
            if (Request.QueryString["PRefID"] != null)
            {
                prefID = int.Parse(Request.QueryString["PRefID"]);
                hdfRefID.Value = prefID.ToString();
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtApplication = wrkfService.GetApplicationID(prefID);
                if (dtApplication != null)
                {
                    if (dtApplication.Rows.Count > 0)
                    {
                        string appID = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                        hdfMIPk.Value = appID;
                        DataTable dtStore = BusinessLogic.StoreManagement.MaterialAccept.GetAcceptStore(Convert.ToInt32(appID));
                        if (dtStore != null && dtStore.Rows.Count > 0)
                        {
                            hdfAcceptStore.Value = dtStore.Rows[0]["MIH_DEPT_TO"].ToString();
                            hdfCompany.Value = dtStore.Rows[0]["MIH_COMPANY"].ToString();
                        }
                        else
                        {
                            hdfAcceptStore.Value = "0";
                            hdfCompany.Value = "0";
                        }
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

            // Modified on Aug-16-2017       --Sruthy H
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));

            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
            //end

            //if (Request.QueryString["PK"] != null)
            //{
            //    FillMaterialAccept(Convert.ToInt32(Request.QueryString["PK"].ToString()));
            //}
            //else
            //{
            //    FillMaterialAccept(0);
            //}
            //if (Request.QueryString["Status"] != null)
            //{
            //    btnSave.Visible = false;
            //    btnSaveandSubmit.Visible = false;
            //    AddToList.Visible = false;
            //    MAH_STATUS.Value = Request.QueryString["Status"].ToString();
            //}
            if (Request.QueryString["IsModify"] != null && hdfIsModify.Value == "1")
            {
                btnSave.Visible = true;
                ucrWrkf.ViewType = 0;
            }
        }


        private void GetUserRights()
        {            
            BusinessLogic.CommonManagement.CommonBL userAuth = new BusinessLogic.CommonManagement.CommonBL();
            UserRightsBO usrRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, Resources.PageURL.MaterialAcceptURL, currentUser.CurrentDeptPK, 0);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true)
                    {
                        hdfIsModify.Value = "1";
                        break;
                    }
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillMaterialAccpetNo(BusinessObject.User objUser)
        {
            //string matrAccpetNo;
            //DataTable dtGRNNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "MANO", 0);
            //if (dtGRNNoFormat.Rows.Count > 0)
            //    matrAccpetNo = dtGRNNoFormat.Rows[0]["DFT_VALUE"].ToString();
            //else
            //    matrAccpetNo = GTIService.Constants.Common.CommonConstant.MANUMBERFORMAT;
            //MatchCollection matchcol = Regex.Matches(matrAccpetNo, @"\#[\w]+\#");
            //for (int i = 0; i < matchcol.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        matrAccpetNo = matrAccpetNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
            //    }
            //    else if (i == 1)
            //    {
            //        matrAccpetNo = matrAccpetNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.MaterialAccept.GetMANO());
            //    }

            //}
            MAH_NO.Text = Resources.Messages.DocGenerationNew;
            //MAH_DATE.Text = DateTime.Now.ToString("dd-MMM-yyyy");
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