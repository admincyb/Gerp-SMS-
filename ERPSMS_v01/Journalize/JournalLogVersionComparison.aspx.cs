using BusinessObject.Journalize;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Journalize
{
    public partial class JournalLogVersionComparison : System.Web.UI.Page
    {
        #region Variables and Properties
        private FinTrxLogBO objFinTrxLogBO;
        private List<FinTrxHeader> objFinTrxHeaderListFirst;
        private List<FinTrxHeader> objFinTrxHeaderListSecond;
        int CurrPK, Version, FromPosting;
        #endregion
        #region PageLevel Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                CurrPK = Convert.ToInt32(Request.QueryString["PK"]);
                Version = Convert.ToInt32(Request.QueryString["Version"]);
                FromPosting = Convert.ToInt32(Request.QueryString["FromPosting"]);
                if (!IsPostBack)
                {
                    GetFieldValues();
                    SetFieldValues();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues()
        {
            objFinTrxLogBO = BusinessLogic.CommonManagement.CommonBL.GetAuditLogComparision(CurrPK, Version);
            Session["objFinTrxLogBO"] = objFinTrxLogBO;
            objFinTrxHeaderListFirst = objFinTrxLogBO.TrxLog.Where(m => m.SL_NO == 1).ToList();
            objFinTrxHeaderListSecond = objFinTrxLogBO.TrxLog.Where(m => m.SL_NO == 2).ToList();
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues()
        {
            ucrFirstVersion.FillDetails(objFinTrxHeaderListFirst, Version, objFinTrxLogBO.TrxLog.Count, objFinTrxHeaderListFirst[0].FTH_ACTIVITY, FromPosting);
            if (objFinTrxHeaderListSecond.Count > 0)
            {
                ucrFirstVersion.FillPreviousDetails(objFinTrxHeaderListSecond, Version, objFinTrxLogBO.TrxLog.Count, objFinTrxHeaderListFirst[0].FTH_ACTIVITY, FromPosting);
            }
        }
        #endregion
        #region Helper Methods
        private void GetUIValuesFromObject()
        {
        }
        public void BindGrid()
        {
        }
        #endregion
        
    }
}