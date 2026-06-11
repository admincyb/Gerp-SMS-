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

    public partial class LogVersionComparision : System.Web.UI.Page
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
                FromPosting= Convert.ToInt32(Request.QueryString["FromPosting"]);
                //FinTrxLogBO objFinLogBO = new FinTrxLogBO();
                //BusinessObject.Journalize.FinTrxHeader objFinTrxHeader = new FinTrxHeader();
                //FinTrxDetail objFinTrxDetail = new FinTrxDetail();

                //List<FinTrxHeader> finTrxHeaderList = new List<FinTrxHeader>();
                //objFinTrxHeader.FTH_PK = 267183;
                //finTrxHeaderList.Add(objFinTrxHeader);
                //objFinTrxHeader.FTH_PK = 267183;
                //finTrxHeaderList.Add(objFinTrxHeader);
                //List<FinTrxDetail> finTrxDetailsList = new List<FinTrxDetail>();
                //objFinTrxDetail.FTR_PK = 1240162;
                //finTrxDetailsList.Add(objFinTrxDetail);
                //objFinTrxDetail.FTR_PK = 1240163;
                //finTrxDetailsList.Add(objFinTrxDetail);

                //objFinTrxHeader.DTL = finTrxDetailsList;

                //objFinLogBO.TrxLog = finTrxHeaderList;

                //string xmlDoc = CommonFunctions.XmlSerialize<FinTrxLogBO>(objFinLogBO);
                //objFinLogBO = CommonFunctions.XmlDeserialize<FinTrxLogBO>(xmlDoc);
                GetFieldValues();
                SetFieldValues();
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
            objFinTrxHeaderListFirst = objFinTrxLogBO.TrxLog.Where(m => m.SL_NO== 1).ToList();//objFinTrxLogBO.TrxLog;
            objFinTrxHeaderListSecond = objFinTrxLogBO.TrxLog.Where(m => m.SL_NO== 2).ToList();//objFinTrxLogBO.TrxLog;
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues()
        {
            //GetUIValuesFromObject();
            //BindGrid();
            ucrFirstVersion.FillDetails(objFinTrxHeaderListFirst, Version, objFinTrxLogBO.TrxLog.Count, objFinTrxHeaderListFirst[0].FTH_ACTIVITY, FromPosting);
            if (objFinTrxHeaderListSecond.Count > 0)
            {
                ucrSecondVersion.FillDetails(objFinTrxHeaderListSecond, Version, objFinTrxLogBO.TrxLog.Count, objFinTrxHeaderListFirst[0].FTH_ACTIVITY, FromPosting);
                ucrSecondVersion.Visible= DivPreviousHead.Visible = true;
            }
            else
                ucrSecondVersion.Visible = DivPreviousHead.Visible= false;
        }
        #endregion
        #region Helper Methods
        private void GetUIValuesFromObject()
        {
            //if (dtPrevVersionHdr!=null)
            //{
            //    lblPrevVersion.Text = dtPrevVersionHdr.Rows[0]["FTH_AUDIT_VERSION"].ToString();
            //    lblPrevActivity.Text = dtPrevVersionHdr.Rows[0]["FTH_ACTIVITY"].ToString();
            //    lblPrevUsername.Text = dtPrevVersionHdr.Rows[0]["FTh_MOD_BY_TEXT"].ToString();
            //    lblPrevDateTime.Text = dtPrevVersionHdr.Rows[0]["FTH_DATE_TM"].ToString();
            //}
            //lblCurrVersion.Text = dtCurrVersionHdr.Rows[0]["FTH_AUDIT_VERSION"].ToString();
            //lblCurrActivity.Text = dtCurrVersionHdr.Rows[0]["FTH_ACTIVITY"].ToString();
            //lblCurrUsername.Text = dtCurrVersionHdr.Rows[0]["FTh_MOD_BY_TEXT"].ToString();
            //lblCurrDateTime.Text = dtCurrVersionHdr.Rows[0]["FTH_DATE_TM"].ToString();
        }
        public void BindGrid()
        {
        }
        #endregion
        //#region Enum
        //#region ControlEnum
        //public enum ControlsEnum
        //{
        //    COMPAREVERSIONS
        //}
        //#endregion
        //#endregion
    }
}