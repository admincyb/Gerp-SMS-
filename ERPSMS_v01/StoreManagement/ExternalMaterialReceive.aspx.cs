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


namespace ERPSMS_v01.StoreManagement
{
    public partial class ExternalMaterialReceive : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        public string ReceivingStore = string.Empty;
        public string MaterialReceiptNo = string.Empty;
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
                hdfSbuCurrency.Value = currentUser.BaseCurrency.ToString();
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                FillInitialData();
                ConfigurationSettings();
                // ICH_DEPT.Value = currentUser.CurrentDeptPK.ToString();
                if (Convert.ToInt16(transactionType.Value) == 4)
                {
                    APT_CODE.Value = ApplicationType.OS;
                }
                else
                {
                    APT_CODE.Value = ApplicationType.EMR;
                }                
                GetWeightedAverage();
                //Comma Separation for Quantity & Amount Based on Configuration(Table)
                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                hdfCurrentDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
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
            //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value));
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                hdfSelCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
            }
        }
        //}
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
                MaterialIssueObject.MaterialIssueDetailsList = new List<BusinessObject.StoreManagement.ExternalMaterialIssueDetails>();
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = Newtonsoft.Json.JsonConvert.SerializeObject(MaterialIssueObject);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillSRSNumber(objUser);
                AST_DOC_MODE.Value = GetDOCMODE();
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.EMR, 0, DateTime.Now);
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
            //DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "ERCNO", 0);
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
            //        srsNo = srsNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetICHNo(currentUser.SBUID, 2));
            //    }

            //}
            ICH_NO.Value = "";
            lblMaterilaConsumptionNo.Text = Resources.Messages.DocGenerationNew;

        }
        private void FillDatasFromCreditNote(int crdrPK)
        {
            if (crdrPK != 0)
            {
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetDetailsFromCRDR(crdrPK);
                AST_DOC_MODE.Value = GetDOCMODE();
            }
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
            //For some clients  the category Semi Finished Goods should not be listed in category ddl
            hdnShowSFGCategory.Value = (GetGlobalResourceObject("ConfigurationsRes", "ShowSFGCategory")).ToString();
            AutoStartValue.Value = GetGlobalResourceObject("ConfigurationsRes", "AutoCompleteLimit").ToString();
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0"; 
        }

        private void GetWeightedAverage()
        {
            hdfWeightedAverage.Value = "0";
            DataTable dtWeightedAverage = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetWeightedAverage(1, "INVENTORY SETTINGS", "EnableWA", currentUser.SBUID);
            if (dtWeightedAverage.Rows.Count>0)
            {
                if (dtWeightedAverage.Rows[0].ItemArray[3].ToString()=="1" )
                {
                    hdfWeightedAverage.Value = "1";
                }
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
                case "2":
                    ReceivingStore = Resources.Controls.ReceivingStore;
                    MaterialReceiptNo = Resources.Controls.MaterialReceiptNo;
                    break;
                case "4":
                    ReceivingStore = Resources.Controls.Store;
                    MaterialReceiptNo = Resources.Controls.OpeningStockNo;
                    break;
            }
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
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "4")
                {
                    if (this.GetLocalResourceObject("BreadcrumbOpeningStock") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbOpeningStock").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("OpeningStockTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}