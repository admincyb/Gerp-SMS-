using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using BusinessObject.Common;

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialReturn : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
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
            }
        }

        private void FillInitialData()
        {
            int refId = 0;
            int appId = 0;

            if (Request.QueryString["ConsumptionID"] != null)
            {
                FillRequisitionData(Convert.ToInt32(Request.QueryString["ConsumptionID"].ToString()));
            }
            else if (Request.QueryString["RefID"] == null)
            {
                FillRequisitionData(0);
            }

        }




        //}
        private void FillRequisitionData(int consumptionID)
        {
            if (consumptionID != 0)
            {
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = BusinessLogic.StoreManagement.MaterialConsumption.GetConsumptionDetails(consumptionID);
            }
            else
            {
                BusinessObject.StoreManagement.MaterialConsumptionCreation ConsumptionObject = new BusinessObject.StoreManagement.MaterialConsumptionCreation();
                ConsumptionObject.ConsumptionDetailsList = new List<BusinessObject.StoreManagement.MaterialConsumptionDetail> { };
                //Assigning initialized Requisitionobject to hidden field (EvalDetailsList)
                ConsumptionDtl.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ConsumptionObject);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillSRSNumber(objUser);
            }


        }
        /// <summary>
        ///Methord used to Fill SRSno Related data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="User object"></param>
        private void FillSRSNumber(BusinessObject.User objUser)
        {
            string srsNo;
            DataTable dtSrsNoFormat = BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValue(objUser.SBUID, 0, "CNSNO", 0);
            if (dtSrsNoFormat.Rows.Count > 0)
                srsNo = dtSrsNoFormat.Rows[0]["DFT_VALUE"].ToString();
            else
                srsNo = GTIService.Constants.Common.CommonConstant.SRSNUMBERFORMAT;
            MatchCollection matchcol = Regex.Matches(srsNo, @"\#[\w]+\#");
            for (int i = 0; i < matchcol.Count; i++)
            {
                if (i == 0)
                {
                    srsNo = srsNo.Replace(matchcol[i].ToString(), System.DateTime.Now.ToString(matchcol[i].ToString().Replace("#", string.Empty)));
                }
                else if (i == 1)
                {
                    srsNo = srsNo.Replace(matchcol[i].ToString(), BusinessLogic.StoreManagement.MaterialConsumption.GetICHNo());
                }

            }
            ICH_NO.Value = srsNo;
            lblMaterilaConsumptionNo.Text = srsNo;

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
        }


      

    }
}