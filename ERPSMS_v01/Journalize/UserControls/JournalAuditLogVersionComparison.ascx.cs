using BusinessObject.AccountManagement;
using BusinessObject.Journalize;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Journalize.UserControls
{
    public partial class JournalAuditLogVersionComparison : System.Web.UI.UserControl
    {
        public bool CheckDifference { get; set; } = false;
        private double totalDr = 0, totalCr = 0, totalCostCenterDr = 0, totalCostCenterCr = 0, totalCostCenterDrPrev = 0, totalCostCenterCrPrev = 0, totalDrPrev = 0, totalCrPrev = 0;
        private ActionsEnum commonActions;
        private string WeightFormat
        {
            get
            {
                return (string)this.ViewState["WeightFormat"];
            }
            set
            {
                this.ViewState["WeightFormat"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            PageActionHandler();
        }
        protected void ActionHandler(object sender, EventArgs e)
        {

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region COSTCENTER
                    case ActionsEnum.COSTCENTER:
                        GridViewRow grvEditRow = (((Button)sender).Parent.Parent as GridViewRow);
                        int Type = Convert.ToInt32(((HiddenField)grvEditRow.FindControl("hdfType")).Value);
                        int Version = Convert.ToInt32(((HiddenField)grvEditRow.FindControl("hdfFTR_AUDIT_VERSION")).Value);
                        if (Type == 1)//1->current debit,2->current credit,3->previous debit,4->previous credit
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divDebitCostCenter]','Cost Center Break - Up','450','350');", true);
                        }
                        else if (Type == 2)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCreditCostCenter]','Cost Center Break - Up','450','350');", true);

                        }
                        else if (Type == 3)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divDebitCostCenter1]','Cost Center Break - Up','450','350');", true);
                        }
                        else if (Type == 4)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCreditCostCenter1]','Cost Center Break - Up','450','350');", true);

                        }

                        break;
                        #endregion

                }
            }

            catch (Exception ex)
            {
                string Error = CommonFunctions.ProcessException(ex);
                if (Error == "547")
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.Messages.CannotDelete + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
                }

            }
            finally
            {
            }
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
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            WeightFormat = "#" + currencysep + "#0.";
            for (int i = 0; i < 2; i++)
            {
                WeightFormat += "0";
            }
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
            grdCreditDtls.Columns[4].HeaderText = grdDebitDtls.Columns[4].HeaderText = "Amt(" + lblCurrency.Text + ")";
            grdDebitDtls.DataSource = finTrxHeaderList[0].DTL.Where(m => m.FTR_DR_AMT_TC != 0).ToList();//finTrxDtlList;
            grdDebitDtls.DataBind();
            grdCreditDtls.DataSource = finTrxHeaderList[0].DTL.Where(m => m.FTR_CR_AMT_TC != 0).ToList();//finTrxDtlList;
            grdCreditDtls.DataBind();

            if (FromPosting == 1)
            {
                TrNarrattion.Visible = true;
                lblNarration.Text = finTrxHeaderList[0].FTH_NARRATION.ToString();
                if (finTrxHeaderList[0].FTH_NARRATION_ISMOD == 1 && CheckDifference == true)
                    lblNarration.ForeColor = System.Drawing.Color.Red;
                //grdVoucher.Columns[2].Visible = false;
                //grdVoucher.Columns[3].Visible = true;
            }
            else
            {
                //grdVoucher.Columns[2].Visible = true;
                //grdVoucher.Columns[3].Visible = false;
            }
            //if (CheckDifference == true)
            //{

            foreach (GridViewRow row in grdDebitDtls.Rows)
            {
                HiddenField hdfFTR_ACCOUNT_ISMOD = (HiddenField)row.FindControl("hdfFTR_ACCOUNT_ISMOD");
                HiddenField hdfFTR_PAYMENT_MODE_ISMOD = (HiddenField)row.FindControl("hdfFTR_PAYMENT_MODE_ISMOD");
                HiddenField hdfFTR_NARRATION_ISMOD = (HiddenField)row.FindControl("hdfFTR_NARRATION_ISMOD");
                HiddenField hdfFTR_CR_AMT_TC_ISMOD = (HiddenField)row.FindControl("hdfFTR_CR_AMT_TC_ISMOD");
                HiddenField hdfFTR_DR_AMT_BC_ISMOD = (HiddenField)row.FindControl("hdfFTR_DR_AMT_BC_ISMOD");
                HiddenField hdfFTR_TYPE_PK_ISMOD = (HiddenField)row.FindControl("hdfFTR_TYPE_PK_ISMOD");
                HiddenField hdfFTR_COST_BIT = (HiddenField)row.FindControl("hdfFTR_COST_BIT");
                Label lblAccountCode = (Label)row.FindControl("lblAccountCode");
                Button btnCostcenter = (Button)row.FindControl("btnCostcenter");
                //Button btnCostcenter1 = (Button)row.FindControl("btnCostcenter1");
                //Label lblAccountName = (Label)row.FindControl("lblAccountName");
                //Label lblMode = (Label)row.FindControl("lblMode");
                Label lblNarration = (Label)row.FindControl("lblNarration");
                Label lblDebit = (Label)row.FindControl("lblDebit");
                //Label lblCredit = (Label)row.FindControl("lblCredit");
                Label lblSubAccount = (Label)row.FindControl("lblSubAccount");
                if (hdfFTR_COST_BIT.Value == "1")
                {
                    List<CostCenter> objCostCenterlist = finTrxHeaderList[0].DTL[0].CostCenter.ToList();

                    btnCostcenter.Visible = true;
                    grdDebitCostCenter.DataSource = objCostCenterlist;
                    grdDebitCostCenter.DataBind();

                }
                if (hdfFTR_TYPE_PK_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblSubAccount.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfFTR_ACCOUNT_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblAccountCode.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfFTR_NARRATION_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblNarration.ForeColor = System.Drawing.Color.Red;
                }
                //if (hdfFTR_CR_AMT_TC_ISMOD.Value == "1")
                //{
                //    lblCredit.ForeColor = System.Drawing.Color.Red;
                //}
                if (hdfFTR_DR_AMT_BC_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblDebit.ForeColor = System.Drawing.Color.Red;
                }
            }
            foreach (GridViewRow row in grdCreditDtls.Rows)
            {
                HiddenField hdfFTR_ACCOUNT_ISMOD = (HiddenField)row.FindControl("hdfFTR_ACCOUNT_ISMOD");
                HiddenField hdfFTR_PAYMENT_MODE_ISMOD = (HiddenField)row.FindControl("hdfFTR_PAYMENT_MODE_ISMOD");
                HiddenField hdfFTR_NARRATION_ISMOD = (HiddenField)row.FindControl("hdfFTR_NARRATION_ISMOD");
                HiddenField hdfFTR_CR_AMT_TC_ISMOD = (HiddenField)row.FindControl("hdfFTR_CR_AMT_TC_ISMOD");
                HiddenField hdfFTR_DR_AMT_BC_ISMOD = (HiddenField)row.FindControl("hdfFTR_DR_AMT_BC_ISMOD");
                HiddenField hdfFTR_TYPE_PK_ISMOD = (HiddenField)row.FindControl("hdfFTR_TYPE_PK_ISMOD");
                HiddenField hdfFTR_COST_BIT = (HiddenField)row.FindControl("hdfFTR_COST_BIT");

                Label lblAccountCode = (Label)row.FindControl("lblAccountCode");
                Button btnCostcenter = (Button)row.FindControl("btnCostcenter");
                //Button btnCostcenter1 = (Button)row.FindControl("btnCostcenter1");

                //Label lblAccountName = (Label)row.FindControl("lblAccountName");
                //Label lblMode = (Label)row.FindControl("lblMode");
                Label lblNarration = (Label)row.FindControl("lblNarration");
                //Label lblDebit = (Label)row.FindControl("lblDebit");
                Label lblCredit = (Label)row.FindControl("lblCredit");
                Label lblSubAccount = (Label)row.FindControl("lblSubAccount");
                if (hdfFTR_COST_BIT.Value == "1")
                {
                    List<CostCenter> objCostCenterlist = finTrxHeaderList[0].DTL[1].CostCenter.ToList();

                    btnCostcenter.Visible = true;
                    grdCreditCostCenter.DataSource = objCostCenterlist;
                    grdCreditCostCenter.DataBind();

                }
                if (hdfFTR_TYPE_PK_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblSubAccount.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfFTR_ACCOUNT_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblAccountCode.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfFTR_NARRATION_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblNarration.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfFTR_CR_AMT_TC_ISMOD.Value == "1" && CheckDifference == true)
                {
                    lblCredit.ForeColor = System.Drawing.Color.Red;
                }
                //if (hdfFTR_DR_AMT_BC_ISMOD.Value == "1")
                //{
                //    lblDebit.ForeColor = System.Drawing.Color.Red;
                //}
            }
            //}
        }
        public void FillPreviousDetails(List<FinTrxHeader> finTrxHeaderList, int Version, int HeaderListCount, string Activity, int FromPosting)
        {
            DivPreviousHead.Visible = tblprevoiusHead.Visible = tbltemplatePrevious.Visible = true;
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            WeightFormat = "#" + currencysep + "#0.";
            for (int i = 0; i < 2; i++)
            {
                WeightFormat += "0";
            }
            lblVersionPrev.Text = finTrxHeaderList[0].FTH_AUDIT_VERSION.ToString();
            lblActivityPrev.Text = Activity;
            lblUsernamePrev.Text = finTrxHeaderList[0].FTH_MOD_BY_TEXT.ToString();
            lblDateTimePrev.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_DATE_TM).ToString("dd-MMM-yy HH:MM");
            lblVoucherDatePrev.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_DATE).ToString("dd-MMM-yyyy");
            lblVoucherNoPre.Text = finTrxHeaderList[0].FTH_VOUCHER_NO.ToString();
            lblRefNoPrev.Text = finTrxHeaderList[0].FTH_REF_NO.ToString();
            lblRefDatePrev.Text = Convert.ToDateTime(finTrxHeaderList[0].FTH_REF_DATE).ToString("dd-MMM-yyyy");
            lblCurrencyPrev.Text = finTrxHeaderList[0].FTH_TRX_CURR_TEXT.ToString();
            lblExchangeRatePrev.Text = finTrxHeaderList[0].FTH_EXCHG_RATE.ToString();
            lblToPrev.Text = finTrxHeaderList[0].FTH_PARTY_NAME.ToString();
            lblRemarksPrev.Text = finTrxHeaderList[0].FTH_REMARKS.ToString();
            List<FinTrxDetail> finTrxDtlList = finTrxHeaderList[0].DTL;
            grdCreditDtlsPrev.Columns[4].HeaderText = grdDebitDtlsPrev.Columns[4].HeaderText = "Amt(" + lblCurrency.Text + ")";
            grdDebitDtlsPrev.DataSource = finTrxHeaderList[0].DTL.Where(m => m.FTR_DR_AMT_TC != 0).ToList();//finTrxDtlList;
            grdDebitDtlsPrev.DataBind();
            grdCreditDtlsPrev.DataSource = finTrxHeaderList[0].DTL.Where(m => m.FTR_CR_AMT_TC != 0).ToList();//finTrxDtlList;
            grdCreditDtlsPrev.DataBind();
            lblNarrationPrev.Text = finTrxHeaderList[0].FTH_NARRATION.ToString();

            foreach (GridViewRow row in grdDebitDtlsPrev.Rows)
            {
                HiddenField hdfFTR_ACCOUNT_ISMOD = (HiddenField)row.FindControl("hdfFTR_ACCOUNT_ISMOD");
                HiddenField hdfFTR_PAYMENT_MODE_ISMOD = (HiddenField)row.FindControl("hdfFTR_PAYMENT_MODE_ISMOD");
                HiddenField hdfFTR_NARRATION_ISMOD = (HiddenField)row.FindControl("hdfFTR_NARRATION_ISMOD");
                HiddenField hdfFTR_CR_AMT_TC_ISMOD = (HiddenField)row.FindControl("hdfFTR_CR_AMT_TC_ISMOD");
                HiddenField hdfFTR_DR_AMT_BC_ISMOD = (HiddenField)row.FindControl("hdfFTR_DR_AMT_BC_ISMOD");
                HiddenField hdfFTR_TYPE_PK_ISMOD = (HiddenField)row.FindControl("hdfFTR_TYPE_PK_ISMOD");
                HiddenField hdfFTR_COST_BIT = (HiddenField)row.FindControl("hdfFTR_COST_BIT");
                Label lblAccountCode = (Label)row.FindControl("lblAccountCode");
                Button btnCostcenter = (Button)row.FindControl("btnCostcenter");
                Label lblNarration = (Label)row.FindControl("lblNarration");
                Label lblDebit = (Label)row.FindControl("lblDebit");
                Label lblSubAccount = (Label)row.FindControl("lblSubAccount");
                if (hdfFTR_COST_BIT.Value == "1")
                {
                    List<CostCenter> objCostCenterlist = finTrxHeaderList[0].DTL[0].CostCenter.ToList();

                    btnCostcenter.Visible = true;
                    grdDebitCostCenterPrev.DataSource = objCostCenterlist;
                    grdDebitCostCenterPrev.DataBind();

                }
            }
            foreach (GridViewRow row in grdCreditDtlsPrev.Rows)
            {
                HiddenField hdfFTR_ACCOUNT_ISMOD = (HiddenField)row.FindControl("hdfFTR_ACCOUNT_ISMOD");
                HiddenField hdfFTR_PAYMENT_MODE_ISMOD = (HiddenField)row.FindControl("hdfFTR_PAYMENT_MODE_ISMOD");
                HiddenField hdfFTR_NARRATION_ISMOD = (HiddenField)row.FindControl("hdfFTR_NARRATION_ISMOD");
                HiddenField hdfFTR_CR_AMT_TC_ISMOD = (HiddenField)row.FindControl("hdfFTR_CR_AMT_TC_ISMOD");
                HiddenField hdfFTR_DR_AMT_BC_ISMOD = (HiddenField)row.FindControl("hdfFTR_DR_AMT_BC_ISMOD");
                HiddenField hdfFTR_TYPE_PK_ISMOD = (HiddenField)row.FindControl("hdfFTR_TYPE_PK_ISMOD");
                HiddenField hdfFTR_COST_BIT = (HiddenField)row.FindControl("hdfFTR_COST_BIT");

                Label lblAccountCode = (Label)row.FindControl("lblAccountCode");
                Button btnCostcenter = (Button)row.FindControl("btnCostcenter");
                Label lblNarration = (Label)row.FindControl("lblNarration");
                Label lblCredit = (Label)row.FindControl("lblCredit");
                Label lblSubAccount = (Label)row.FindControl("lblSubAccount");
                if (hdfFTR_COST_BIT.Value == "1")
                {
                    List<CostCenter> objCostCenterlist = finTrxHeaderList[0].DTL[1].CostCenter.ToList();

                    btnCostcenter.Visible = true;
                    grdCreditCostCenterPrev.DataSource = objCostCenterlist;
                    grdCreditCostCenterPrev.DataBind();

                }
            }
        }
        #endregion
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdDebitDtls")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblDebit = e.Row.FindControl("lblDebit") as Label;
                        totalDr = totalDr + Convert.ToDouble(lblDebit.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblDrTotal = (Label)e.Row.FindControl("lblDrTotal");
                        lblDrTotal.Text = GetFormattedWeightwithComma(totalDr);
                    }
                }
                if (((GridView)sender).ID == "grdCreditDtls")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblCredit = e.Row.FindControl("lblCredit") as Label;
                        totalCr = totalCr + Convert.ToDouble(lblCredit.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblCrTotal = (Label)e.Row.FindControl("lblCrTotal");
                        lblCrTotal.Text = GetFormattedWeightwithComma(totalCr);
                    }
                }
                if (((GridView)sender).ID == "grdDebitDtlsPrev")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblDebit = e.Row.FindControl("lblDebit") as Label;
                        totalDrPrev = totalDrPrev + Convert.ToDouble(lblDebit.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblDrTotal = (Label)e.Row.FindControl("lblDrTotal");
                        lblDrTotal.Text = GetFormattedWeightwithComma(totalDrPrev);
                    }
                }
                if (((GridView)sender).ID == "grdCreditDtlsPrev")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblCredit = e.Row.FindControl("lblCredit") as Label;
                        totalCrPrev = totalCrPrev + Convert.ToDouble(lblCredit.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblCrTotal = (Label)e.Row.FindControl("lblCrTotal");
                        lblCrTotal.Text = GetFormattedWeightwithComma(totalCrPrev);
                    }
                }
                if (((GridView)sender).ID == "grdDebitCostCenter")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
                        Label lblAmountDt = e.Row.FindControl("lblAmountDt") as Label;
                        HiddenField hdfFTR_FTD_AMT_BC_ISMOD = e.Row.FindControl("hdfFTR_FTD_AMT_BC_ISMOD") as HiddenField;
                        HiddenField hdfFTR_CNM_CODE_ISMOD = e.Row.FindControl("hdfFTR_CNM_CODE_ISMOD") as HiddenField;
                        Label lblCostCenter = e.Row.FindControl("lblCostCenter") as Label;
                        if (hdfFTR_CNM_CODE_ISMOD.Value == "1" && CheckDifference == true)
                            lblCostCenter.ForeColor = System.Drawing.Color.Red;
                        if (hdfFTR_FTD_AMT_BC_ISMOD.Value == "1" && CheckDifference == true)
                            lblAmountDt.ForeColor = System.Drawing.Color.Red;
                        
                        totalCostCenterDr = totalCostCenterDr + Convert.ToDouble(lblAmountDt.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                        lblTotal.Text = GetFormattedWeightwithComma(totalCostCenterDr);
                    }
                }
                if (((GridView)sender).ID == "grdCreditCostCenter")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblAmountCr = e.Row.FindControl("lblAmountCr") as Label;
                        HiddenField hdfFTR_FTD_AMT_BC_ISMOD = e.Row.FindControl("hdfFTR_FTD_AMT_BC_ISMOD") as HiddenField;
                        HiddenField hdfFTR_CNM_CODE_ISMOD = e.Row.FindControl("hdfFTR_CNM_CODE_ISMOD") as HiddenField;
                        Label lblCostCenter = e.Row.FindControl("lblCostCenter") as Label;
                        if (hdfFTR_CNM_CODE_ISMOD.Value == "1" && CheckDifference == true)
                            lblCostCenter.ForeColor = System.Drawing.Color.Red;
                        if (hdfFTR_FTD_AMT_BC_ISMOD.Value == "1" && CheckDifference == true)
                            lblAmountCr.ForeColor = System.Drawing.Color.Red;
                        totalCostCenterCr = totalCostCenterCr + Convert.ToDouble(lblAmountCr.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                        lblTotal.Text = GetFormattedWeightwithComma(totalCostCenterCr);
                    }
                }
                if (((GridView)sender).ID == "grdDebitCostCenterPrev")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblAmountDt = e.Row.FindControl("lblAmountDt") as Label;
                        totalCostCenterDrPrev = totalCostCenterDrPrev + Convert.ToDouble(lblAmountDt.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                        lblTotal.Text = GetFormattedWeightwithComma(totalCostCenterDrPrev);
                    }
                }
                if (((GridView)sender).ID == "grdCreditCostCenterPrev")
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblAmountCr = e.Row.FindControl("lblAmountCr") as Label;
                        totalCostCenterCrPrev = totalCostCenterCrPrev + Convert.ToDouble(lblAmountCr.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                        lblTotal.Text = GetFormattedWeightwithComma(totalCostCenterCrPrev);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        public string GetFormattedWeightwithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (num != 0)
                return num.ToString(WeightFormat);
            else
                return num.ToString();
        }
    }

}