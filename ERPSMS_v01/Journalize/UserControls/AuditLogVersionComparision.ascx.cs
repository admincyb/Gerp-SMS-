using BusinessObject.Journalize;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.UserControls
{
    public partial class AuditLogVersionComparision : System.Web.UI.UserControl
    {
        public bool CheckDifference { get; set; } = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
        }
        #endregion
        #region Helper Methods
        public void FillDetails(List<FinTrxHeader> finTrxHeaderList, int Version, int HeaderListCount, string Activity, int FromPosting)
        {
            if (finTrxHeaderList[0].SL_NO == 1 && HeaderListCount > 1)
                CheckDifference = true;
            lblVersion.Text = finTrxHeaderList[0].FTH_AUDIT_VERSION.ToString();
            lblActivity.Text = Activity;
            lblUsername.Text = finTrxHeaderList[0].FTH_MOD_BY_TEXT.ToString();
            lblDateTime.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_DATE_TM).ToString("dd-MMM-yy HH:MM");

            lblVoucherDate.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_DATE).ToString("dd-MMM-yyyy");

            if (finTrxHeaderList[0].FTH_DATE_ISMOD == 1 && CheckDifference == true)
                lblVoucherDate.ForeColor = System.Drawing.Color.Red;

            lblVoucherNo.Text = finTrxHeaderList[0].FTH_VOUCHER_NO.ToString();
            lblRefNo.Text = finTrxHeaderList[0].FTH_REF_NO.ToString();

            if (finTrxHeaderList[0].FTH_REF_NO_ISMOD == 1 && CheckDifference == true)
                lblRefNo.ForeColor = System.Drawing.Color.Red;

            lblRefDate.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_REF_DATE).ToString("dd-MMM-yyyy");

            if (finTrxHeaderList[0].FTH_REF_DATE_ISMOD == 1 && CheckDifference == true)
                lblRefDate.ForeColor = System.Drawing.Color.Red;

            lblCurrency.Text = finTrxHeaderList[0].FTH_TRX_CURR_TEXT.ToString();
            lblExchangeRate.Text = finTrxHeaderList[0].FTH_EXCHG_RATE.ToString();
            lblTo.Text = finTrxHeaderList[0].FTH_PARTY_NAME.ToString();

            if (finTrxHeaderList[0].FTH_PARTY_NAME_ISMOD == 1 && CheckDifference == true)
                lblTo.ForeColor = System.Drawing.Color.Red;

            lblRemarks.Text = finTrxHeaderList[0].FTH_REMARKS.ToString();

            if (finTrxHeaderList[0].FTH_REMARKS_ISMOD == 1 && CheckDifference == true)
                lblRemarks.ForeColor = System.Drawing.Color.Red;
            List<FinTrxDetail> finTrxDtlList = finTrxHeaderList[0].DTL;
            grdVoucher.DataSource = finTrxDtlList;
            grdVoucher.DataBind();

            if (FromPosting == 1)
            {
                TrNarrattion.Visible = true;
                lblNarration.Text = finTrxHeaderList[0].FTH_NARRATION.ToString();
                if (finTrxHeaderList[0].FTH_NARRATION_ISMOD == 1 && CheckDifference == true)
                    lblNarration.ForeColor = System.Drawing.Color.Red;
                grdVoucher.Columns[2].Visible = false;
                grdVoucher.Columns[3].Visible = true;
            }
            else
            {
                grdVoucher.Columns[2].Visible = true;
                grdVoucher.Columns[3].Visible = false;
            }
            if (CheckDifference == true)
            {
                foreach (GridViewRow row in grdVoucher.Rows)
                {
                    HiddenField hdfFTR_ACCOUNT_ISMOD = (HiddenField)row.FindControl("hdfFTR_ACCOUNT_ISMOD");
                    HiddenField hdfFTR_PAYMENT_MODE_ISMOD = (HiddenField)row.FindControl("hdfFTR_PAYMENT_MODE_ISMOD");
                    HiddenField hdfFTR_NARRATION_ISMOD = (HiddenField)row.FindControl("hdfFTR_NARRATION_ISMOD");
                    HiddenField hdfFTR_CR_AMT_TC_ISMOD = (HiddenField)row.FindControl("hdfFTR_CR_AMT_TC_ISMOD");
                    HiddenField hdfFTR_DR_AMT_BC_ISMOD = (HiddenField)row.FindControl("hdfFTR_DR_AMT_BC_ISMOD");
                    HiddenField hdfFTR_TYPE_PK_ISMOD = (HiddenField)row.FindControl("hdfFTR_TYPE_PK_ISMOD");
                    Label lblAccountCode = (Label)row.FindControl("lblAccountCode");
                    Label lblAccountName = (Label)row.FindControl("lblAccountName");
                    Label lblMode = (Label)row.FindControl("lblMode");
                    Label lblNarration = (Label)row.FindControl("lblNarration");
                    Label lblDebit = (Label)row.FindControl("lblDebit");
                    Label lblCredit = (Label)row.FindControl("lblCredit");
                    Label lblSubAccount = (Label)row.FindControl("lblSubAccount");
                        if (hdfFTR_TYPE_PK_ISMOD.Value == "1")
                        {
                            lblSubAccount.ForeColor = System.Drawing.Color.Red;
                        }
                        if (hdfFTR_ACCOUNT_ISMOD.Value == "1")
                        {
                            lblAccountCode.ForeColor = lblAccountName.ForeColor = System.Drawing.Color.Red;
                        }
                        if (hdfFTR_PAYMENT_MODE_ISMOD.Value == "1")
                        {
                            lblMode.ForeColor = System.Drawing.Color.Red;
                        }
                        if (hdfFTR_NARRATION_ISMOD.Value == "1")
                        {
                            lblNarration.ForeColor = System.Drawing.Color.Red;
                        }
                        if (hdfFTR_CR_AMT_TC_ISMOD.Value == "1")
                        {
                            lblCredit.ForeColor = System.Drawing.Color.Red;
                        }
                        if (hdfFTR_DR_AMT_BC_ISMOD.Value == "1")
                        {
                            lblDebit.ForeColor = System.Drawing.Color.Red;
                        }
                        //if (hdfFTR_TYPE_PK_ISMOD.Value == "1")
                        //{
                        //    lblSubAccount.ForeColor = System.Drawing.Color.Red;
                        //}
                    
                }
            }
        }
        #endregion
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdVoucher")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //Label lblSubAccount = e.Row.FindControl("lblSubAccount") as Label;
                        //if (!string.IsNullOrEmpty(lblSubAccount.Text))
                        //{
                        //    e.Row.Cells[3].BackColor = System.Drawing.ColorTranslator.FromHtml("#A2C4A5");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

    }
}