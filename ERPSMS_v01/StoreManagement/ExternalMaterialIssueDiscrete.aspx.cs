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

namespace ERPSMS_v01.StoreManagement
{
    public partial class ExternalMaterialIssueDiscrete : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User currentUser;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private DataTable dtCompany;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                FillInitialData();
                ConfigurationSettings();
                // ICH_DEPT.Value = currentUser.CurrentDeptPK.ToString();
                APT_CODE.Value = ApplicationType.EMI;
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
            else if (Request.QueryString["RefID"] == null)
            {
                FillRequisitionData(0);
            }

            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("AUTO COMPLETE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                AutoStartValue.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
        }
    }
}