using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.OrderPlanningBO;
using CustomControls;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic.OrderPlanning;

namespace CustomerPortal.OrderPlanning
{
    public partial class OrderPlanning : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        private User currentUser;
        //private OrderPlanBO objPlanDtls;
        private List<PendingOrderList> SelectedOrderList;
        private List<OrderDetails> lstOrders;
        private List<SizeDetails> lstSizeDetails;

        private DataTable dtList;
        private DataTable dtLine;
        private DataTable dtPage;
        private DataTable dtLineDtl;
        private DataTable dtSummary;
        private DataTable dtVersions;
        private DataTable dtLineWiseSummary;

        private bool IsBindLines = false;

        HiddenField hdfDecimalCoundMst;

        private int PlanGroupPK = 0;
        private int linePK = 0;
        private int FormerlinePK = 0;
        private int OPPageSize = 0;
        private int SummaryType = 0;
        private int SummaryTypePK = 0;
        double TotalPlannedPerc = 0;
        double TotalPlannedPcs = 0;
        double TotalProducedPerc = 0;
        double TotalProducedPcs = 0;
        double TotalBalancePerc = 0;
        double TotalBalancePcs = 0;

        double TotalSCapacity = 0;
        double TotalScenQtyPlan = 0;
        double TotalLineScenPrdHrs = 0;
        double TotalSPlannedPcs = 0;
        double TotalSPlannedPerc = 0;
        double TotalSPlannedBalance = 0;
        double TotalSProducedPcs = 0;
        double TotalSProducedPerc = 0;
        double TotalSProducedBalance = 0;

        #region Properties
        private int CurPK
        {
            get
            {
                return (int)this.ViewState["CurPK"];
            }
            set
            {
                this.ViewState["CurPK"] = value;
            }
        }
        /// <summary>
        /// To maintain Last Modified Date
        /// </summary>
        private string lastModDate
        {
            get
            {
                return (string)this.ViewState["lastModDate"];
            }
            set
            {
                this.ViewState["lastModDate"] = value;
            }
        }
        /// To maintain the WeightFormat in viewstate
        /// </summary>
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
        private string DecimalFormat
        {
            get
            {
                return (string)this.ViewState["DecimalFormat"];
            }
            set
            {
                this.ViewState["DecimalFormat"] = value;
            }

        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState["PageIndex"];
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }
        /// <summary>
        /// To maintain saved data
        /// </summary>
        private OrderPlanBO objPlanDtls
        {
            get
            {
                return (OrderPlanBO)this.ViewState["objPlanDtls"];
            }
            set
            {
                this.ViewState["objPlanDtls"] = value;
            }
        }
        /// <summary>
        /// To maintain data
        /// </summary>
        private LineBO objLineBO
        {
            get
            {
                return (LineBO)this.ViewState["objLineBO"];
            }
            set
            {
                this.ViewState["objLineBO"] = value;
            }
        }
        /// <summary>
        /// To maintain data
        /// </summary>
        private PLanLineBO objPlnLineBo
        {
            get
            {
                return (PLanLineBO)this.ViewState["objPlnLineBo"];
            }
            set
            {
                this.ViewState["objPlnLineBo"] = value;
            }
        }
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        private List<LineDetails> lstLineDetails
        {
            get
            {
                return (List<LineDetails>)this.ViewState["lstLineDetails"];
            }
            set
            {
                this.ViewState["lstLineDetails"] = value;
            }
        }

        private List<SODetails> lstSODetails
        {
            get
            {
                return (List<SODetails>)this.ViewState["lstSODetails"];
            }
            set
            {
                this.ViewState["lstSODetails"] = value;
            }
        }

        private int isRevise
        {
            get
            {
                return this.ViewState["isRevise"] == null ? 0 : Convert.ToInt32(this.ViewState["isRevise"]);
            }
            set
            {
                this.ViewState["isRevise"] = value;
            }
        }

        private int PlanLnePopUpStatus
        {
            get
            {
                return this.ViewState["PlanLnePopUpStatus"] == null ? 0 : Convert.ToInt32(this.ViewState["PlanLnePopUpStatus"]);
            }
            set
            {
                this.ViewState["PlanLnePopUpStatus"] = value;
            }
        }

        private int isFinalize
        {
            get
            {
                return this.ViewState["isFinalize"] == null ? 0 : Convert.ToInt32(this.ViewState["isFinalize"]);
            }
            set
            {
                this.ViewState["isFinalize"] = value;
            }
        }
        /// <summary>
        /// To maintain the sort order in viewstate
        /// </summary>
        private string SortOrder
        {
            get
            {
                return (string)this.ViewState["SortOrder"];
            }
            set
            {
                this.ViewState["SortOrder"] = value;
            }
        }
        /// <summary>
        /// To maintain the sort expression in viewstate
        /// </summary>
        private string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }
            set
            {
                this.ViewState["SortExpression"] = value;
            }
        }
        //To maintan row index
        private int ParentRowIndex
        {
            get
            {
                return (int)this.ViewState["ParentRowIndex"];
            }
            set
            {
                this.ViewState["ParentRowIndex"] = value;
            }
        }
        private int RwIndex
        {
            get
            {
                return (int)this.ViewState["RwIndex"];
            }
            set
            {
                this.ViewState["RwIndex"] = value;
            }
        }

        private int ScenarioHdrIndex
        {
            get
            {
                return (int)this.ViewState["ScenarioHdrIndex"];
            }
            set
            {
                this.ViewState["ScenarioHdrIndex"] = value;
            }
        }

        #endregion
        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            currentUser = (User)HttpContext.Current.User.Identity;
        }
        #endregion

        #region Page Level Event
        /// <summary>
        /// For Page Load Event, and fiill details as default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            ucrPO.ShowPendingOrder += new EventHandler(ActionHandler);
            //ucrLinewisePdtn.ReportPopUp += new EventHandler(ActionHandler);
            //ucrPO.AfterApply += new EventHandler(ucrPO_AfterApply);
            if (!IsPostBack)
            {
                #region Set Decimal Count
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                DecimalFormat = "#" + currencysep + "#";
                WeightFormat = "#" + currencysep + "#0.";
                hdfDecimalCoundMst = this.Page.Master.FindControl("hdfRateDecimal") as HiddenField;
                for (int i = 0; i < Convert.ToInt32(GetLocalResourceObject("PercDecimalDigit")); i++)
                {
                    WeightFormat += "0";
                }
                #endregion
                ddlFilterStatus.SelectedIndex = ddlFilterStatus.Items.IndexOf(ddlFilterStatus.Items.FindByValue("1"));
                ucrPO.isVisibleOrder = true;
                GetFieldValues(ControlsEnum.PLANLISTING);
                SetFieldValues(ControlsEnum.PLANLISTING);
                EntryStatus = EntryStatus.LISTMODE;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTabs", "ShowTabs('" + hdnTabListNew.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSummaryTabs", "ShowSummaryTabs('" + hdnSummaryTab.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTabs", "ShowTabs('" + hdnTabListNew.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSummaryTabs", "ShowSummaryTabs('" + hdnSummaryTab.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "NewModeBtnvisibility", "NewModeBtnvisibility();", true);
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTabs", "ShowTabs('" + hdnTabListNew.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSummaryTabs", "ShowSummaryTabs('" + hdnSummaryTab.Value + "');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ReviseMode", "ReviseMode(" + (Convert.ToInt32(lblVersion.Text) > 0 ? 1 : 0) + ");", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DisableHeader", "DisableHeader();", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HighlightFinalizedRow", "HighlightFinalizedRow();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComp", "InitComponents();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideChkCombinedLines", "ShowHideChkCombinedLines(" + GetLocalResourceObject("DefaultCompatibleLines").ToString().ToLower() + ");", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ActionHandlers
        #region ButtonActions
        /// <summary>
        /// For Button Click (Save/Cancel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr;
                GridView grd;
                string arg;
                RadioButton rbtn;
                int result = 0;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.CHANGE;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECK_CHANGE;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    //if (((RadioButton)sender).ID == "rbtSelect")
                    //{
                    commonActions = ActionsEnum.ITEMSELECTED; 
                    //}
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        CurPK = 0;
                        hdnTabListNew.Value = "1";
                        ucrPO.POPageIndex = "1";
                        ucrPO.SelectedList = null;
                        ucrPO.objALLPendingOrderLst = null;
                        ((HiddenField)ucrPO.FindControl("hdfIsAllPagsSelected")).Value = "0";
                        chkOptimizePlan.Checked = false;
                        chkOptimizeScenario.Checked = false;
                        ucrPO.PopulatePendingListGrid();
                        lnkPlanning.Enabled = true;
                       // tab4.Visible = true;//QucikPlan tab
                        tab3.Visible = false;//Summary tab
                        if (GetLocalResourceObject("PlanPeriodConfig").ToString() != string.Empty && Convert.ToInt32(GetLocalResourceObject("PlanPeriodConfig")) > 0)
                        {
                            DateTime date = DateTime.Now.AddMonths(Convert.ToInt32(GetLocalResourceObject("PlanPeriodConfig")));
                            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                            txtFromDate.Text = firstDayOfMonth.ToString(CommonConstants.DATEFORMAT); ;
                            txtToDate.Text = lastDayOfMonth.ToString(CommonConstants.DATEFORMAT);
                            txtPlanName.Text = string.Format(GetLocalResourceObject("NewModePlanName").ToString(), date.ToString("MMMM") + " " + date.Year);
                            txtPlanCode.Text = string.Format(GetLocalResourceObject("NewModePlanCode").ToString(), date.ToString("MM"), date.ToString("yy"));
                        }
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region TABS
                    case ActionsEnum.PENDINGORDERS:
                        lnkPlanning.Enabled = true;

                        if (CurPK == 0)
                        {
                           // tab4.Visible = true;//Quick Plan Tab
                            tab3.Visible = false;//Summary tab
                        }
                        else
                        {
                            tab4.Visible = false;//Quick Plan Tab
                            tab3.Visible = true;//Summary tab
                        }

                        hdnTabListNew.Value = "1";
                        break;
                    case ActionsEnum.PLANNING:
                        if ((objPlanDtls != null && objPlanDtls.Details != null && objPlanDtls.Details.Count > 0) || ucrPO.SelectedItemsCount() > 0)
                        {
                            hdnTabListNew.Value = "2";
                            tab4.Visible = false;//Quick Plan Tab
                            tab3.Visible = true;//Summary tab
                            if (ucrPO.SelectedItemsCount() > 0)
                            {
                                SelectOrdersForPlan();
                                GetFieldValues(ControlsEnum.PLANGROUPLINES);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_NoItemSelected").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    case ActionsEnum.SCENARIO:
                        if ((objPlanDtls != null && objPlanDtls.Details != null && objPlanDtls.Details.Count > 0) || ucrPO.SelectedItemsCount() > 0)
                        {
                            hdnTabListNew.Value = "4";
                            if (ucrPO.SelectedItemsCount() > 0)
                            {
                                lnkPlanning.Enabled = false;
                                SelectOrdersForScenario();
                                GetFieldValues(ControlsEnum.GROUPLINES);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_NoItemSelectedForscenario").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    case ActionsEnum.SUMMARY:
                        if (((LinkButton)sender).ID == "lnkSummary")
                        {
                            if (objPlanDtls != null && objPlanDtls.Details != null && objPlanDtls.Details.Count > 0)
                            {
                                hdnTabListNew.Value = "3";
                                //hdnSummaryTab.Value = "1";
                                GetFieldValues(ControlsEnum.LINEWISESUMMARY);
                                SetFieldValues(ControlsEnum.LINEWISESUMMARY);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_NoItemSelected").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        //else if (((LinkButton)sender).ID == "lnkSummary1")
                        //{
                        //    hdnSummaryTab.Value = "1";
                        //}
                        //else if (((LinkButton)sender).ID == "lnkSummary2")
                        //{
                        //    hdnSummaryTab.Value = "2";
                        //}
                        break;
                    #endregion

                    #region HIERACHICALGRID - SIZE
                    case ActionsEnum.PLANGROUPITEMS:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        CheckBox ChkPlanOrder = (CheckBox)gvr.FindControl("ChkPlanOrder") as CheckBox;
                        Label lblBalancetoPlan = (Label)gvr.FindControl("lblBalancetoPlan") as Label;
                        System.Web.UI.UserControl ucTextFormat = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan") as System.Web.UI.UserControl;
                        TextBox txtTotalCurrPlan = ucTextFormat.FindControl("txtFormattedAmount") as TextBox;

                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdSize") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                lstSizeDetails = null;
                            }
                            else
                            {
                                PlanGroupPK = Convert.ToInt32(arg);
                                lstSizeDetails = new List<SizeDetails>();
                                lstSizeDetails = objPlanDtls.Details.Where(dtl => dtl.PND_PLAN_GROUP == PlanGroupPK).SelectMany(x => x.SizeDetail).ToList();
                            }

                            grd.Visible = true;
                            if (lstSizeDetails != null && lstSizeDetails.Count > 0)
                            {
                                grd.DataSource = lstSizeDetails;
                                grd.DataBind();
                            }
                            double sizeSum = 0;
                            if (ChkPlanOrder.Checked)
                            {
                                txtTotalCurrPlan.RemoveCssClass("input-disabled");
                                txtTotalCurrPlan.Enabled = true;
                                foreach (GridViewRow gvrRow in grd.Rows)
                                {
                                    string percVal = "0";
                                    System.Web.UI.UserControl ucTextFormatDtl = (System.Web.UI.UserControl)gvrRow.FindControl("txtSizePlanNow");
                                    double OrderBal = !string.IsNullOrEmpty((gvrRow.FindControl("lblSBaltoPlan") as Label).Text) ? Convert.ToDouble((gvrRow.FindControl("lblSBaltoPlan") as Label).Text.Replace(",", "")) : 0;
                                    double TotalBal = !string.IsNullOrEmpty(lblBalancetoPlan.Text) ? Convert.ToDouble(lblBalancetoPlan.Text.Replace(",", "")) : 0;
                                    int GroupDtl = Convert.ToInt32((gvrRow.FindControl("hdfSizePlanGroup") as HiddenField).Value);
                                    int Size = Convert.ToInt32((gvrRow.FindControl("hdfSizeVal") as HiddenField).Value);
                                    ((ImageButton)gvrRow.FindControl("imbViewSC")).Visible = true;
                                    if (TotalBal > 0)
                                    {
                                        percVal = ((OrderBal * 100) / TotalBal).ToString();
                                        (gvrRow.FindControl("hdfProportionVal") as HiddenField).Value = percVal;
                                        if (txtTotalCurrPlan.Text == "0")
                                            (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text = "0";
                                        else
                                        {
                                            if (objPlanDtls.Details != null && objPlanDtls.Details.Count > 0 && objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == GroupDtl).PND_PLAN_QTY == 0)
                                                (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(((Convert.ToDouble(txtTotalCurrPlan.Text) * Convert.ToDouble(percVal)) / 100).ToString()) != string.Empty ? GetFormattedNumber(((Convert.ToDouble(txtTotalCurrPlan.Text) * Convert.ToDouble(percVal)) / 100).ToString()) : "0";
                                            else
                                                (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(lstSizeDetails.FirstOrDefault(x => x.SIZE_PLAN_GROUP == GroupDtl && x.PNS_SIZE == Size).SIZE_PLAN_QTY);
                                        }
                                    }

                                    sizeSum = sizeSum + ((ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text));
                                }


                                //for correct pcs count start 
                                double TotSum = !string.IsNullOrEmpty(txtTotalCurrPlan.Text) ? Convert.ToDouble(txtTotalCurrPlan.Text) : 0;

                                if (sizeSum < TotSum && grd.Rows.Count > 0)
                                {
                                    double diffQty = TotSum - sizeSum;
                                    System.Web.UI.UserControl ucTextFormatDtl = (grd.Rows[grd.Rows.Count - 1].FindControl("txtSizePlanNow") as System.Web.UI.UserControl);
                                    double plnqty = (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text);
                                    (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(plnqty + diffQty);
                                }
                                else if (sizeSum > TotSum && grd.Rows.Count > 0)
                                {
                                    double diffQty = sizeSum - TotSum;
                                    System.Web.UI.UserControl ucTextFormatDtl = (grd.Rows[grd.Rows.Count - 1].FindControl("txtSizePlanNow") as System.Web.UI.UserControl);
                                    double plnqty = (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text);
                                    (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(plnqty - diffQty);
                                }


                                //for correct pcs count end
                            }
                            else
                            {
                                foreach (GridViewRow gvrRow in grd.Rows)
                                {
                                    string percVal = "0";
                                    double OrderBal = !string.IsNullOrEmpty((gvrRow.FindControl("lblSBaltoPlan") as Label).Text) ? Convert.ToDouble((gvrRow.FindControl("lblSBaltoPlan") as Label).Text.Replace(",", "")) : 0;
                                    double TotalBal = !string.IsNullOrEmpty(lblBalancetoPlan.Text) ? Convert.ToDouble(lblBalancetoPlan.Text.Replace(",", "")) : 0;
                                    ((ImageButton)gvrRow.FindControl("imbViewSC")).Visible = false;
                                    if (TotalBal > 0)
                                    {
                                        percVal = ((OrderBal * 100) / TotalBal).ToString();
                                        (gvrRow.FindControl("hdfProportionVal") as HiddenField).Value = percVal;
                                    }
                                }
                            }
                        }
                        (gvr.FindControl("hdfIsExpandedGroupItem") as HiddenField).Value = "1";
                        break;
                    #endregion

                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        if (((RadioButton)sender).ID == "rbtSelect")
                        {
                            foreach (GridViewRow grdrow in grdPlanList.Rows)
                            {
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    CurPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPlanlistPK")).Value);
                                    lastModDate = ((HiddenField)grdrow.FindControl("hdfModedt")).Value;
                                    if (((HiddenField)grdrow.FindControl("hdfIsActive")).Value == "0")
                                        btnEdit.Visible = false;
                                    else
                                        btnEdit.Visible = true;
                                    break;
                                }
                            }
                        }
                        else if (((RadioButton)sender).ID == "rbtSelectLine")
                        {
                            gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfLinePK = (gvr.FindControl("hdfLinePK") as HiddenField);
                            FormerlinePK = (hdfLinePK.Value == string.Empty ? 0 : Convert.ToInt32(hdfLinePK.Value));
                            SetFieldValues(ControlsEnum.LINEFORMERDETAILS);
                            divFormerDtls.Visible = true;
                            ShowLineScenarioPopUp();
                        }
                        else if (((RadioButton)sender).ID == "rbtSelectPlanLine")
                        {
                            divPlanlnFormerDtls.Visible = true;
                            gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfLinePK = (gvr.FindControl("hdfLinePK") as HiddenField);
                            List<LineDetails> lneDtls = lstLineDetails.Where(x => x.PNL_LINE == Convert.ToInt32(hdfLinePK.Value)).ToList();
                            if (lneDtls != null && lneDtls[0].LineFormerDetail.Count > 0)
                                grdPlanlineFormer.DataSource = lneDtls[0].LineFormerDetail;
                            grdPlanlineFormer.DataBind();
                            ShowLinePopup(PlanLnePopUpStatus);
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                        if (CurPK > 0)
                        {
                            lnkPlanning.Enabled = true;
                            tab4.Visible = false;//Qucik Plan Tab
                            tab3.Visible = true;//Summary tab
                            hdnTabListNew.Value = "2";
                            ucrPO.OrderPlanPK = CurPK;
                            ucrPO.SelectedList = null;
                            ucrPO.objALLPendingOrderLst = null;
                            ((HiddenField)ucrPO.FindControl("hdfIsAllPagsSelected")).Value = "0";
                            ucrPO.POPageIndex = null;
                            ucrPO.PopulatePendingListGrid();
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            GetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                            SetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                            SummaryType = 1;
                            GetFieldValues(ControlsEnum.SUMMARYDETAILS);
                            SetFieldValues(ControlsEnum.SUMMARYGROUPLIST);
                            GetFieldValues(ControlsEnum.PLANGROUPLINES);
                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Row").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    #endregion

                    #region VIEW
                    case ActionsEnum.VIEW:
                        if (CurPK > 0)
                        {
                            hdnTabListNew.Value = "2";
                            ucrPO.OrderPlanPK = CurPK;
                            ucrPO.SelectedList = null;
                            ucrPO.PopulatePendingListGrid();
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            GetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                            SetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                            SummaryType = 1;
                            GetFieldValues(ControlsEnum.SUMMARYDETAILS);
                            SetFieldValues(ControlsEnum.SUMMARYGROUPLIST);
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Row").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurPK > 0)
                        {
                            DeleteOrderPlan(CurPK, lastModDate, 0);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_NoPlan").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    #endregion

                    #region CANCELSUBMIT
                    #region DELETE
                    case ActionsEnum.CANCELSUBMIT:
                        if (CurPK > 0)
                        {
                            DeleteOrderPlan(CurPK, lastModDate, 1);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_NoPlan").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    #endregion
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        lnkPlanning.Enabled = true;
                       // tab4.Visible = true;//quick plan Tab
                        tab3.Visible = true;//Summary tab
                        ResetPage();
                        ucrPO.POPageIndex = "1";
                        ucrPO.SelectedList = null;
                        ucrPO.objALLPendingOrderLst = null;
                        ucrPO.PopulatePendingListGrid();
                        GetFieldValues(ControlsEnum.PLANLISTING);
                        SetFieldValues(ControlsEnum.PLANLISTING);
                        break;
                    #endregion

                    #region SAVE/FINALIZE/REVISE
                    case ActionsEnum.SAVE:
                    case ActionsEnum.FINALIZE:
                    case ActionsEnum.REVISE:
                        if (!IsValid)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            if (commonActions == ActionsEnum.FINALIZE)
                                isFinalize = 1;
                            else if (commonActions == ActionsEnum.REVISE)
                                isRevise = 1;

                            if (objPlanDtls == null)
                                objPlanDtls = new OrderPlanBO();
                            if (CheckPlanNow() == 1)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_PlanNow").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                            }
                            //else if ((commonActions == ActionsEnum.FINALIZE || commonActions == ActionsEnum.REVISE) && CheckEmptyLineExists() == 1)
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_NoLineMapped").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                            //}
                            //else if ((commonActions == ActionsEnum.FINALIZE || commonActions == ActionsEnum.REVISE) && CheckLinePlannedQty() == 1)
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_PlanQty_LineQty_Mismatch").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                            //}
                            else
                            {
                                UpdateDetailList();
                                if (objPlanDtls.Details != null && objPlanDtls.Details.Count > 0)
                                {
                                    SavePlan(objPlanDtls, 0);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_NoItemSelected").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region ADD ACTION - LINE, SC POP UP
                    case ActionsEnum.ADD:
                        if (((ImageButton)sender).ID == "imbAddLine")
                        {
                            gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                            hdfLinePlanGroup.Value = ((HiddenField)gvr.FindControl("hdfPlanGroupPK")).Value;
                            hdfLinePlanGroupDtlPK.Value = ((HiddenField)gvr.FindControl("hdfPlanDtlPK")).Value;
                            hdfLinePlanGroupSlNO.Value = ((HiddenField)gvr.FindControl("hdfPlanDtlSLNO")).Value;
                            System.Web.UI.UserControl ucCurrPlan = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan");
                            TextBox txtTotal = ucCurrPlan.FindControl("txtFormattedAmount") as TextBox;
                            lblCurrPlanQty.Text = !string.IsNullOrEmpty(txtTotal.Text) ? GetFormattedNumber(txtTotal.Text) : "0";
                            lblLineProductGroup.Text = lblLineProductGroup.ToolTip = ((Label)gvr.FindControl("lblProductGroup")).ToolTip;
                            lblPlanNamePopup.Text = CommonFunctions.GetShortString(txtPlanName.Text, 35);
                            lblPlanNamePopup.ToolTip = txtPlanName.Text;
                            lblFromDatePopup.Text = txtFromDate.Text;
                            lblRequiredbyPopup.Text = Convert.ToDateTime(((Label)gvr.FindControl("lblRequiredby")).Text).ToShortDateString();
                            lblToDatePopup.Text = txtToDate.Text;
                            GetFieldValues(ControlsEnum.LINELIST);
                            SetFieldValues(ControlsEnum.LINELIST);
                            ResetLineDtl();
                            lstLineDetails = new List<LineDetails>();
                            lstLineDetails = objPlanDtls.Details.Where(dtl => dtl.PND_PK == Convert.ToInt32(hdfLinePlanGroupDtlPK.Value)).SelectMany(x => x.LineDetails).ToList();
                            ucLineQty.Text = (Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(lstLineDetails.Sum(x => x.PNL_PLAN_QTY))) < 0 ? "0" : (Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(lstLineDetails.Sum(x => x.PNL_PLAN_QTY))).ToString();
                            hdfLineRequiredQty.Value = txtTotal.Text;
                            SetFieldValues(ControlsEnum.LINEGRID);
                            divPlanlnFormerDtls.Visible = false;
                            PlanLnePopUpStatus = 0;
                            divLinePopUp.Attributes.Add("class", GetLocalResourceObject("Css_EditlinePopup").ToString());
                            ShowLinePopup(0);
                        }
                        else if (((ImageButton)sender).ID == "imbAddLineScenario")
                        {
                            divFormerDtls.Visible = false;
                            gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                            grdLineScenario.DataSource = objPlanDtls.Details[gvr.RowIndex].LineDetails;
                            grdLineScenario.DataBind();

                            System.Web.UI.UserControl ucCurrPlanQty = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan");
                            TextBox txtTotalQty = ucCurrPlanQty.FindControl("txtFormattedAmount") as TextBox;

                            lblPlanQtyScenario.Text = !string.IsNullOrEmpty(txtTotalQty.Text) ? GetFormattedNumber(txtTotalQty.Text) : "0";
                            lblPlanGrScenario.Text = lblPlanGrScenario.ToolTip = ((Label)gvr.FindControl("lblProductGroup")).ToolTip;
                            hdfLineProductGroup.Value = ((HiddenField)gvr.FindControl("hdfPlanGroupPK")).Value;
                            lblPlanReqByScenario.Text = Convert.ToDateTime(((Label)gvr.FindControl("lblRequiredby")).Text).ToShortDateString();

                            ShowLineScenarioPopUp();
                        }
                        else if (((ImageButton)sender).ID == "imbAddLineDtl")
                        {
                            divPlanlnFormerDtls.Visible = false;
                            if (lstLineDetails == null)
                                lstLineDetails = new List<LineDetails>();

                            ActionHandler(ddlLine, EventArgs.Empty);//for fill data against line

                            if (CompareLineDate() == 0)
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Error_LineDate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (ucLineSpeed.Text == "0")
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_valid_LineSpeed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (ucLineQty.Text == "0")
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_valid_LineQty").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (Convert.ToDouble(ucLineQty.Text) > Convert.ToDouble(lblCapacity.Text))
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_No_LineCapacity").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (Convert.ToDouble(ucLineQty.Text) > Convert.ToDouble(lblBalLineQty.Text))
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Total_lineCapacity_Exceeds").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (lstLineDetails.Where(x => x.PNL_LINE == Convert.ToInt32(ddlLine.SelectedValue) && (x.PNL_PK != Convert.ToInt32(hdfLineDtlPK.Value) || x.SLNO != Convert.ToInt32(hdfLineSLNO.Value))).Count() > 0)
                            {
                                ShowLinePopup(0);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Line_Exists").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else
                            {
                                LineDetails objLineDtl = new LineDetails();
                                DateTime TtempDate, tempTime;


                                if (Convert.ToInt32(hdfLineDtlPK.Value) == 0 && Convert.ToInt32(hdfLineSLNO.Value) == 0)
                                {
                                    objLineDtl.PNL_PK = Convert.ToInt32(hdfLineDtlPK.Value);
                                    objLineDtl.LNE_CODE = ddlLine.SelectedItem.Text;
                                    objLineDtl.PNL_LINE = Convert.ToInt32(ddlLine.SelectedValue);
                                    objLineDtl.PNL_PLAN_GROUP = Convert.ToInt32(hdfLinePlanGroup.Value);
                                    objLineDtl.PNL_PLAN_TRX_DTL = Convert.ToInt32(hdfLinePlanGroupDtlPK.Value);
                                    objLineDtl.LNE_CAPACITY = hdfdayCapacity.Value != string.Empty ? Convert.ToDouble(hdfdayCapacity.Value) : 0;
                                    objLineDtl.LNE_PLANT_NAME = lblPlant.Text;
                                    objLineDtl.PND_SL_NO = Convert.ToInt32(hdfLinePlanGroupSlNO.Value);
                                    objLineDtl.PNL_PLAN_QTY = Convert.ToDouble(ucLineQty.Text);
                                    objLineDtl.SLNO = lstLineDetails.Count + 1;
                                    objLineDtl.PRD_HRS = hdfProdHrs.Value == string.Empty ? 0 : Convert.ToInt32(hdfProdHrs.Value);
                                    objLineDtl.REPAIR_HRS = hdfProdHrs.Value == string.Empty ? 0 : Convert.ToDouble(hdfRepairHrs.Value);
                                    tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineFromTime.Text.Trim()));
                                    if (!DateTime.TryParse(txtLineFromDt.Text, out TtempDate))
                                        TtempDate = DateTime.Now;
                                    TtempDate = SetTime(TtempDate, tempTime);
                                    objLineDtl.PNL_FROM_DATE = TtempDate.ToString();
                                    tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineToTime.Text.Trim()));
                                    if (!DateTime.TryParse(txtLineToDt.Text, out TtempDate))
                                        TtempDate = DateTime.Now;
                                    TtempDate = SetTime(TtempDate, tempTime);
                                    objLineDtl.PNL_TO_DATE = TtempDate.ToString();
                                    objLineDtl.PNL_LNE_AVG_SPEED = Convert.ToDouble(ucLineSpeed.Text);

                                    if (objPlnLineBo != null && objPlnLineBo.Detail.Count > 0)
                                    {
                                        objLineDtl.LineFormerDetail = (from order in objPlnLineBo.Detail
                                                                       select new LineFormerDetail
                                                                       {
                                                                           PLD_SIZ_PK = order.PLD_SIZ_PK.ToString(),
                                                                           PLD_SIZ_TEXT = order.PLD_SIZ_TEXT,
                                                                           PLD_PRODUCTION_QTY = Math.Round(order.PLD_PRODUCTION_QTY),
                                                                           PLD_FORMER_QTY = Math.Round(order.PLD_FORMER_QTY),
                                                                           PNL_SL_NO = lstLineDetails.Count + 1,
                                                                           PND_SL_NO = Convert.ToInt32(hdfLinePlanGroupSlNO.Value)
                                                                       }).ToList();
                                    }
                                    lstLineDetails.Add(objLineDtl);
                                }
                                else
                                {
                                    if (Convert.ToInt32(hdfLineDtlPK.Value) > 0)
                                        objLineDtl = lstLineDetails.SingleOrDefault(x => x.PNL_PK == Convert.ToInt32(hdfLineDtlPK.Value));
                                    else
                                        objLineDtl = lstLineDetails.SingleOrDefault(x => x.SLNO == Convert.ToInt32(hdfLineSLNO.Value));
                                    objLineDtl.LNE_CODE = ddlLine.SelectedItem.Text;
                                    objLineDtl.PNL_LINE = Convert.ToInt32(ddlLine.SelectedValue);
                                    objLineDtl.LNE_CAPACITY = hdfdayCapacity.Value != string.Empty ? Convert.ToDouble(hdfdayCapacity.Value) : 0;
                                    objLineDtl.LNE_PLANT_NAME = lblPlant.Text;
                                    objLineDtl.PNL_PLAN_QTY = Convert.ToDouble(ucLineQty.Text);
                                    tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineFromTime.Text.Trim()));
                                    if (!DateTime.TryParse(txtLineFromDt.Text, out TtempDate))
                                        TtempDate = DateTime.Now;
                                    TtempDate = SetTime(TtempDate, tempTime);
                                    objLineDtl.PNL_FROM_DATE = TtempDate.ToString();
                                    tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineToTime.Text.Trim()));
                                    if (!DateTime.TryParse(txtLineToDt.Text, out TtempDate))
                                        TtempDate = DateTime.Now;
                                    TtempDate = SetTime(TtempDate, tempTime);
                                    objLineDtl.PNL_TO_DATE = TtempDate.ToString();
                                    objLineDtl.PNL_LNE_AVG_SPEED = Convert.ToDouble(ucLineSpeed.Text);

                                    if (objPlnLineBo != null && objPlnLineBo.Detail.Count > 0)
                                    {
                                        objLineDtl.LineFormerDetail = (from order in objPlnLineBo.Detail
                                                                       select new LineFormerDetail
                                                                       {
                                                                           PLD_SIZ_PK = order.PLD_SIZ_PK.ToString(),
                                                                           PLD_SIZ_TEXT = order.PLD_SIZ_TEXT,
                                                                           PLD_PRODUCTION_QTY = Math.Round(order.PLD_PRODUCTION_QTY),
                                                                           PLD_FORMER_QTY = Math.Round(order.PLD_FORMER_QTY),
                                                                           PND_SL_NO = Convert.ToInt32(hdfLinePlanGroupSlNO.Value)
                                                                       }).ToList();
                                    }
                                }

                                SetFieldValues(ControlsEnum.LINEGRID);
                                ResetLineDtl();
                                ucLineQty.Text = (Convert.ToDouble(hdfLineRequiredQty.Value) - lstLineDetails.Sum(x => x.PNL_PLAN_QTY)) < 0 ? "0" : (Convert.ToDouble(hdfLineRequiredQty.Value) - lstLineDetails.Sum(x => x.PNL_PLAN_QTY)).ToString();
                                ShowLinePopup(0);
                            }

                        }
                        else if (((ImageButton)sender).ID == "imbViewSC")
                        {
                            gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            GridViewRow gvrRow = ((ImageButton)sender).Parent.Parent.Parent.Parent.Parent.Parent as ExtGridViewRow;
                            RwIndex = gvr.RowIndex;
                            ParentRowIndex = gvrRow.RowIndex;
                            int GroupDtl = Convert.ToInt32((gvr.FindControl("hdfSizePlanGroup") as HiddenField).Value);
                            int Size = Convert.ToInt32((gvr.FindControl("hdfSizeVal") as HiddenField).Value);
                            HiddenField hdfIsApplied = (HiddenField)gvr.FindControl("hdfIsApplied");
                            lblSPlanName.Text = CommonFunctions.GetShortString(txtPlanName.Text, 35);
                            lblSPlanName.ToolTip = txtPlanName.Text;
                            lblSDFrom.Text = txtFromDate.Text;
                            lblSDTo.Text = txtToDate.Text;
                            lblGroupSize.Text = (gvr.FindControl("lblSizeText") as Label).Text;
                            hdfSCSize.Value = (gvr.FindControl("hdfSizeVal") as HiddenField).Value;
                            lblSPGroup.Text = (gvrRow.FindControl("lblProductGroup") as Label).Text;
                            hdfSPGroupVal.Value = (gvrRow.FindControl("hdfPlanGroupPK") as HiddenField).Value;
                            System.Web.UI.UserControl ucCurrPlan = (System.Web.UI.UserControl)gvr.FindControl("txtSizePlanNow");
                            TextBox txtTotal = ucCurrPlan.FindControl("txtFormattedAmount") as TextBox;
                            lblSPlanQty.Text = !string.IsNullOrEmpty(txtTotal.Text) ? GetFormattedNumber(txtTotal.Text) : "0";
                            lblSReqdBy.Text = Convert.ToDateTime(((Label)gvrRow.FindControl("lblRequiredby")).Text).ToShortDateString();
                            lstSizeDetails = objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == GroupDtl).SizeDetail;
                            lstSODetails = lstSizeDetails.Where(x => x.PNS_SIZE == Size).SelectMany(dtl => dtl.SODetails).ToList();
                            if (hdfIsApplied.Value == "0")
                            {
                                lstSODetails.ForEach(dtl =>
                                {
                                    if (lstSODetails.Sum(y => y.PNS_PLAN_QTY) != Convert.ToDouble(txtTotal.Text))
                                        dtl.PNS_PLAN_QTY = Math.Floor(Convert.ToDouble((Convert.ToDouble(lblSPlanQty.Text) * Convert.ToDouble(dtl.SOD_PERCENTAGE)) / 100));
                                });
                                //Code to adjust the last SC quantity to adjust sum to be equal to the total value
                                double plnqty = Convert.ToDouble(lstSODetails.Sum(x => x.PNS_PLAN_QTY));
                                if (plnqty != Convert.ToDouble(txtTotal.Text))
                                {
                                    double diffQty = Convert.ToDouble(txtTotal.Text) - plnqty;
                                    SODetails item = lstSODetails.LastOrDefault();
                                    item.PNS_PLAN_QTY = item.PNS_PLAN_QTY + diffQty;
                                }

                            }
                            SetFieldValues(ControlsEnum.SCDETAILS);
                            ShowSCPopUp();
                        }
                        break;
                    #endregion

                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        if (((DropDownList)sender).ID == "ddlLine")
                        {
                            if (ddlLine.SelectedValue != "-1")
                            {
                                linePK = Convert.ToInt32(ddlLine.SelectedValue);
                                GetFieldValues(ControlsEnum.LINEDETAILS);
                                lblPlant.Text = string.Empty;
                                lblCapacity.Text = string.Empty;
                                lblLinePlanned.Text = string.Empty;
                                lblBalLineQty.Text = string.Empty;
                                txtLineFromDt.Text = string.Empty;
                                txtLineFromTime.Text = string.Empty;
                                txtLineToDt.Text = string.Empty;
                                txtLineToTime.Text = string.Empty;
                                hdfProdHrs.Value = "0";
                                hdfRepairHrs.Value = "0";
                                if (objPlnLineBo != null)
                                {
                                    lblPlant.Text = objPlnLineBo.LNE_PLANT_NAME;
                                    hdfdayCapacity.Value = objPlnLineBo.LNE_CAPACITY.ToString();
                                    ucLineSpeed.Text = objPlnLineBo.AVG_SPEED.ToString();
                                    hdfHolderPos.Value = objPlnLineBo.LNE_TOT_FORMER_HOLDER_POS.ToString();
                                    txtLineFromDt.Text = !string.IsNullOrEmpty(objPlnLineBo.LNE_FORMER_FROM_DATE) ? Convert.ToDateTime(objPlnLineBo.LNE_FORMER_FROM_DATE).ToShortDateString() : string.Empty;
                                    txtLineFromTime.Text = !string.IsNullOrEmpty(objPlnLineBo.LNE_FORMER_FROM_DATE) ? Convert.ToDateTime(objPlnLineBo.LNE_FORMER_FROM_DATE).ToString(CommonConstants.TIMEFORMAT) : string.Empty;
                                    txtLineToDt.Text = !string.IsNullOrEmpty(objPlnLineBo.LNE_FORMER_TO_DATE) ? Convert.ToDateTime(objPlnLineBo.LNE_FORMER_TO_DATE).ToShortDateString() : string.Empty;
                                    txtLineToTime.Text = !string.IsNullOrEmpty(objPlnLineBo.LNE_FORMER_TO_DATE) ? Convert.ToDateTime(objPlnLineBo.LNE_FORMER_TO_DATE).ToString(CommonConstants.TIMEFORMAT) : string.Empty;
                                    double Capacity = Convert.ToDouble(hdfdayCapacity.Value);
                                    hdfProdHrs.Value = objPlnLineBo.PRD_HRS.ToString();
                                    hdfRepairHrs.Value = objPlnLineBo.REPAIR_HRS.ToString();
                                    //Capacity calculated from DB

                                    if (txtLineFromDt.Text != string.Empty && txtLineFromTime.Text != string.Empty && txtLineToDt.Text != string.Empty && txtLineToTime.Text != string.Empty)
                                    {
                                        TimeSpan DateDiff = Convert.ToDateTime(txtLineToDt.Text).Subtract(Convert.ToDateTime(txtLineFromDt.Text));
                                        lblCapacity.Text = ((DateDiff.Days + 1) * Capacity) > 0 ? GetFormattedNumber(Math.Ceiling((DateDiff.Days + 1) * Capacity)) : "0";
                                    }
                                    lblLinePlanned.Text = GetLineTotalQty() > 0 ? GetFormattedNumber(GetLineTotalQty()) : "0";
                                    if (lblCapacity.Text != string.Empty && lblLinePlanned.Text != string.Empty)
                                        lblBalLineQty.Text = (Convert.ToDouble(lblCapacity.Text) - Convert.ToDouble(lblLinePlanned.Text)) > 0 ? GetFormattedNumber(Convert.ToDouble(lblCapacity.Text) - Convert.ToDouble(lblLinePlanned.Text)) : "0";
                                }
                            }
                            else
                            {

                                lblPlant.Text = string.Empty;
                                lblCapacity.Text = string.Empty;
                                lblLinePlanned.Text = string.Empty;
                                lblBalLineQty.Text = string.Empty;
                                txtLineFromDt.Text = string.Empty;
                                txtLineFromTime.Text = string.Empty;
                                txtLineToDt.Text = string.Empty;
                                txtLineToTime.Text = string.Empty;
                                hdfProdHrs.Value = "0";
                                hdfRepairHrs.Value = "0";
                            }
                            ShowLinePopup(0);
                        }
                        break;
                    #endregion

                    #region EDIT_ACTION
                    case ActionsEnum.EDIT_ACTION:
                        if (((ImageButton)sender).ID == "imbLineEdit")
                        {
                            gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfDtlPK = ((HiddenField)gvr.FindControl("hdfDtlPK")) as HiddenField;
                            HiddenField hdfSLNO = ((HiddenField)gvr.FindControl("hdfSLNO")) as HiddenField;
                            LineDetails objLine = new LineDetails();
                            if (hdfDtlPK.Value != "0")
                                objLine = lstLineDetails.SingleOrDefault(x => x.PNL_PK == Convert.ToInt32(hdfDtlPK.Value));
                            else
                                objLine = lstLineDetails.SingleOrDefault(x => x.SLNO == Convert.ToInt32(hdfSLNO.Value));
                            linePK = objLine.PNL_LINE;
                            GetFieldValues(ControlsEnum.LINELIST);
                            SetFieldValues(ControlsEnum.LINELIST);
                            ddlLine.SelectedIndex = ddlLine.Items.IndexOf(ddlLine.Items.FindByValue(objLine.PNL_LINE.ToString()));
                            hdfLineDtlPK.Value = objLine.PNL_PK.ToString();
                            hdfLineSLNO.Value = objLine.SLNO.ToString();
                            lblPlant.Text = objLine.LNE_PLANT_NAME;
                            hdfdayCapacity.Value = objLine.LNE_CAPACITY.ToString();
                            double Capacity = objLine.LNE_CAPACITY.ToString() != string.Empty ? Convert.ToDouble(objLine.LNE_CAPACITY) : 0;

                            txtLineFromDt.Text = Convert.ToDateTime(objLine.PNL_FROM_DATE).ToString(CommonConstants.DATEFORMAT);
                            txtLineFromTime.Text = Convert.ToDateTime(objLine.PNL_FROM_DATE).ToString(CommonConstants.TIMEFORMAT);
                            txtLineToDt.Text = Convert.ToDateTime(objLine.PNL_TO_DATE).ToString(CommonConstants.DATEFORMAT);
                            txtLineToTime.Text = Convert.ToDateTime(objLine.PNL_TO_DATE).ToString(CommonConstants.TIMEFORMAT);

                            if (txtLineFromDt.Text != string.Empty && txtLineToDt.Text != string.Empty)
                            {
                                TimeSpan DateDiff = Convert.ToDateTime(txtLineToDt.Text).Subtract(Convert.ToDateTime(txtLineFromDt.Text));
                                lblCapacity.Text = ((DateDiff.Days + 1) * Capacity) > 0 ? GetFormattedNumber(Math.Ceiling((DateDiff.Days + 1) * Capacity)) : "0";
                            }

                            //lblCapacity.Text = objLine.LNE_CAPACITY != null && objLine.LNE_CAPACITY.ToString() != string.Empty ? GetFormattedNumber(objLine.LNE_CAPACITY.ToString()) : "0";
                            ucLineQty.Text = objLine.PNL_PLAN_QTY.ToString();
                            lblLinePlanned.Text = GetLineTotalQty(objLine.PND_SL_NO).ToString() != string.Empty ? GetFormattedNumber(GetLineTotalQty(objLine.PND_SL_NO).ToString()) : "0";
                            lblBalLineQty.Text = (Convert.ToDouble(lblCapacity.Text) - Convert.ToDouble(lblLinePlanned.Text)) > 0 ? GetFormattedNumber(Convert.ToDouble(lblCapacity.Text) - Convert.ToDouble(lblLinePlanned.Text)) : "0";


                            ucLineSpeed.Text = objLine.PNL_LNE_AVG_SPEED.ToString();
                            ShowLinePopup(0);
                        }
                        break;
                    #endregion

                    #region DELETE_ACTION
                    case ActionsEnum.DELETE_ACTION:
                        if (((ImageButton)sender).ID == "imbLineDelete")
                        {
                            divPlanlnFormerDtls.Visible = false;
                            gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfDtlPK = ((HiddenField)gvr.FindControl("hdfDtlPK")) as HiddenField;
                            HiddenField hdfSLNO = ((HiddenField)gvr.FindControl("hdfSLNO")) as HiddenField;
                            if (hdfDtlPK.Value != "0")
                                lstLineDetails.Remove(lstLineDetails.SingleOrDefault(x => x.PNL_PK == Convert.ToInt32(hdfDtlPK.Value)));
                            else
                                lstLineDetails.Remove(lstLineDetails.SingleOrDefault(x => x.SLNO == Convert.ToInt32(hdfSLNO.Value)));
                            SetFieldValues(ControlsEnum.LINEGRID);
                            ShowLinePopup(0);
                            ResetLineDtl();
                            ucLineQty.Text = (Convert.ToDouble(hdfLineRequiredQty.Value) - Convert.ToDouble(lstLineDetails.Sum(x => x.PNL_PLAN_QTY))).ToString();
                        }
                        else if (((ImageButton)sender).ID == "imbGroupDelete")
                        {
                            gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfPlanDtlPK = ((HiddenField)gvr.FindControl("hdfPlanDtlPK")) as HiddenField;
                            if (hdfPlanDtlPK.Value != "0")
                                objPlanDtls.Details.Remove(objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)));

                            HiddenField hdfPlanGroupPK = ((HiddenField)gvr.FindControl("hdfPlanGroupPK")) as HiddenField;
                            if (objLineBO != null && objLineBO.Detail != null)
                                objLineBO.Detail.Remove(objLineBO.Detail.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfPlanGroupPK.Value)));

                            GetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                            SetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                        }
                        else if (((ImageButton)sender).ID == "imbSODelete")
                        {
                            arg = ((ImageButton)sender).CommandArgument;
                            gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfOrderSlNo = ((HiddenField)gvr.FindControl("hdfOrderSlNo")) as HiddenField;
                            HiddenField hdfPlanGroupDtl = ((HiddenField)gvr.FindControl("hdfPlanGroupDtl")) as HiddenField;
                            foreach (GridViewRow gvrRw in grdOrderDtls.Rows)
                            {
                                HiddenField hdfOrderDtl = (HiddenField)gvrRw.FindControl("hdfOrderDtl") as HiddenField;
                                System.Web.UI.UserControl ucrNumericControlDtl = (System.Web.UI.UserControl)gvrRw.FindControl("txtDtlCurrPlan") as System.Web.UI.UserControl;
                                TextBox txtDtlCurrPlan = ucrNumericControlDtl.FindControl("txtFormattedAmount") as TextBox;
                                objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.ToList().ForEach(dtl =>
                                {
                                    dtl.PNS_PLAN_QTY = !string.IsNullOrEmpty(txtDtlCurrPlan.Text) ? Convert.ToDouble(txtDtlCurrPlan.Text) : 0;
                                });
                            }
                            SODetails objItem = lstSODetails.SingleOrDefault(dtl => dtl.PNS_PK == Convert.ToInt32(arg));
                            lstSODetails.Remove(objItem);

                            if (lstSODetails.Count > 0)
                                BindOrderGridAfterDelete(gvr, lstSODetails);
                            else
                                SetFieldValues(ControlsEnum.SCDETAILS);
                            ShowSCPopUp();
                            /*SODetails objItem = objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.SingleOrDefault(dtl => dtl.PNS_PK == Convert.ToInt32(arg));
                        objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.Remove(objItem);
                        if (objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.Count == 0)
                        {
                            SizeDetails objTempS = objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value));
                            objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.Remove(objTempS);
                        }
                        lstSODetails = new List<SODetails>();
                        if (objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.Count > 0)
                            lstSODetails = objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.ToList();

                        if (lstSODetails != null && lstSODetails.Count > 0)
                            objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanGroupDtl.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SIZE_PLAN_QTY = lstSODetails.Sum(x => x.PNS_PLAN_QTY);
                        else
                            objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanGroupDtl.Value)).PND_PLAN_QTY = 0;*/
                        }
                        break;
                    #endregion

                    #region APPLY
                    case ActionsEnum.APPLY:
                        if (((Button)sender).ID == "btnApplyLines")
                        {
                            if (hdfIscontYes.Value == "0" && Convert.ToDouble(hdfLineRequiredQty.Value) != lstLineDetails.Sum(x => x.PNL_PLAN_QTY))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "$(document).ready(function(){CheckLineQty();});", true);
                            }
                            else
                            {
                                objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfLinePlanGroupDtlPK.Value)).LineDetails = new List<LineDetails>();
                                objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfLinePlanGroupDtlPK.Value)).LineDetails = lstLineDetails;
                                foreach (GridViewRow gvrRow in grdGrouplist.Rows)
                                {
                                    HiddenField hdfPlanDtlPK = ((HiddenField)gvrRow.FindControl("hdfPlanDtlPK") as HiddenField);
                                    if (Convert.ToInt32(hdfPlanDtlPK.Value) == Convert.ToInt32(hdfLinePlanGroupDtlPK.Value))
                                    {
                                        ((LinkButton)gvrRow.FindControl("lnbLine") as LinkButton).Text = GetFormattedNumber(lstLineDetails.Sum(x => x.PNL_PLAN_QTY));
                                        if (Convert.ToInt32(lstLineDetails.Sum(x => x.PNL_PLAN_QTY)) > 0)
                                        {
                                            ((LinkButton)gvrRow.FindControl("lnbLine") as LinkButton).Enabled = true;
                                            ((LinkButton)gvrRow.FindControl("lnbLine") as LinkButton).CssClass = "text-underline";
                                        }
                                        else
                                        {
                                            ((LinkButton)gvrRow.FindControl("lnbLine") as LinkButton).Enabled = false;
                                            ((LinkButton)gvrRow.FindControl("lnbLine") as LinkButton).CssClass = "link-disabled";
                                        }
                                    }
                                }
                                ResetLinePopUp();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup", "ClosePopup();", true);
                            }
                        }
                        else if (((Button)sender).ID == "btnApplySCDtls")
                        {
                            if (hdfIsSCcontYes.Value == "0" && Convert.ToDouble(lblSPlanQty.Text) != Convert.ToDouble(hdfSCPopUpTotal.Value))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "$(document).ready(function(){CheckSCQty();});", true);
                            }
                            else
                            {
                                objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails = new List<SODetails>();
                                objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails = lstSODetails;
                                foreach (GridViewRow gvrRow in grdOrderDtls.Rows)
                                {
                                    System.Web.UI.UserControl ucCurrPlan = (System.Web.UI.UserControl)gvrRow.FindControl("txtDtlCurrPlan");
                                    TextBox txtTotal = ucCurrPlan.FindControl("txtFormattedAmount") as TextBox;
                                    objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails[gvrRow.RowIndex].PNS_PLAN_QTY = Convert.ToDouble(txtTotal.Text);

                                }

                                if (objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value)).SODetails.Count == 0)
                                {
                                    SizeDetails objTempS = objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.SingleOrDefault(dtl => dtl.PNS_SIZE == Convert.ToInt32(hdfSCSize.Value));
                                    objPlanDtls.Details.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfSPGroupVal.Value)).SizeDetail.Remove(objTempS);
                                }
                                System.Web.UI.UserControl ucGroupQty = (System.Web.UI.UserControl)grdGrouplist.Rows[ParentRowIndex].FindControl("txtTotalCurrPlan");
                                string GroupQty = (ucGroupQty.FindControl("txtFormattedAmount") as TextBox).Text;
                                GridView grdS = (GridView)grdGrouplist.Rows[ParentRowIndex].FindControl("grdSize");
                                System.Web.UI.UserControl ucSCQty = (System.Web.UI.UserControl)grdS.Rows[RwIndex].FindControl("txtSizePlanNow");
                                HiddenField hdfSizePlanGroup = (HiddenField)grdS.Rows[RwIndex].FindControl("hdfSizePlanGroup");
                                HiddenField hdfSizeVal = (HiddenField)grdS.Rows[RwIndex].FindControl("hdfSizeVal");
                                (ucGroupQty.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(Convert.ToDouble(GroupQty) - Convert.ToDouble((ucSCQty.FindControl("txtFormattedAmount") as TextBox).Text) + Convert.ToDouble(hdfSCPopUpTotal.Value));
                                (ucSCQty.FindControl("txtFormattedAmount") as TextBox).Text = GetFormattedNumber(hdfSCPopUpTotal.Value);
                                hdfIsSCcontYes.Value = "0";
                                ((HiddenField)grdS.Rows[RwIndex].FindControl("hdfIsApplied")).Value = "1";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                            }
                        }
                        else if (((Button)sender).ID == "btnMachineApply")
                        {

                            //objPlanDtls.Details[ScenarioHdrIndex].FilteredLines = new List<FilteredLines>();
                            //foreach (GridViewRow gvdtl in grdMachines.Rows)
                            //{
                            //    FilteredLines ObjFilteredLines = new FilteredLines();
                            //    CheckBox chk = (CheckBox)gvdtl.FindControl("chkMachine") as CheckBox;
                            //    HiddenField hdfLinePK = (HiddenField)gvdtl.FindControl("hdfLinePK") as HiddenField;
                            //    if (chk.Checked == true)
                            //    {
                            //        ObjFilteredLines.LNE_PK = Convert.ToInt32(hdfLinePK.Value);
                            //        ObjFilteredLines.PND_SL_NO = objPlanDtls.Details[ScenarioHdrIndex].PND_SL_NO;
                            //        objPlanDtls.Details[ScenarioHdrIndex].FilteredLines.Add(ObjFilteredLines);
                            //    }
                            //}
                            foreach (GridViewRow gvdtl in grdMachines.Rows)
                            {
                                CheckBox chk = (CheckBox)gvdtl.FindControl("chkMachine") as CheckBox;
                                if (chk.Checked)
                                    objLineBO.Detail[ScenarioHdrIndex].LineDetail[gvdtl.RowIndex].Ischecked = 1;
                                else
                                    objLineBO.Detail[ScenarioHdrIndex].LineDetail[gvdtl.RowIndex].Ischecked = 0;

                                if (chkCombinedLines.Checked)
                                {
                                    objPlanDtls.Details[ScenarioHdrIndex].LINE_COMBINED = 1;
                                }
                                else
                                {
                                    objPlanDtls.Details[ScenarioHdrIndex].LINE_COMBINED = 0;
                                    ((CheckBox)grdGrouplistScenario.HeaderRow.FindControl("chkAllCombinedLines")).Checked = false;
                                }
                            }


                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                            //ShowMachinePopUp();
                            //int checkCount = 0;
                            //if (chkCombinedLines.Checked == true)
                            //{
                            //    objPlanDtls.LINE_COMBINED = 1;
                            //    foreach (GridViewRow gvrMch in grdMachines.Rows)
                            //    {
                            //        CheckBox ChkMch = (CheckBox)gvrMch.FindControl("chkMachine") as CheckBox;
                            //        if (ChkMch.Checked)
                            //            checkCount = 1;
                            //    }
                            //    if (checkCount == 0)
                            //    {
                            //        ShowMachinePopUp();
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectMachine").ToString()) + "');", true);
                            //        break;
                            //    }
                            //}
                            //IsBindLines = true;
                        }
                        break;
                    #endregion

                    #region SHOW LINE POP UP/VERSION POP UP
                    case ActionsEnum.VIEW_ACTIONPOPUP:
                        if (sender is ImageButton)
                        {
                            if (((ImageButton)sender).ID == "imbShowVersions")
                            {
                                GetFieldValues(ControlsEnum.PLANVERSIONS);
                                SetFieldValues(ControlsEnum.PLANVERSIONS);
                                ShowPlanVersionPopUp();
                            }
                        }
                        else if (sender is LinkButton)
                        {
                            if (((LinkButton)sender).ID == "lnbLine")
                            {
                                divPlanlnFormerDtls.Visible = false;
                                PlanLnePopUpStatus = 1;
                                gvr = ((LinkButton)sender).Parent.Parent as ExtGridViewRow;
                                hdfLinePlanGroupDtlPK.Value = ((HiddenField)gvr.FindControl("hdfPlanDtlPK")).Value;
                                System.Web.UI.UserControl ucCurrPlanQty = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan");
                                TextBox txtTotalQty = ucCurrPlanQty.FindControl("txtFormattedAmount") as TextBox;
                                lblCurrPlanQty.Text = !string.IsNullOrEmpty(txtTotalQty.Text) ? GetFormattedNumber(txtTotalQty.Text) : "0";
                                lblLineProductGroup.Text = lblLineProductGroup.ToolTip = ((Label)gvr.FindControl("lblProductGroup")).ToolTip;
                                //lblGroupSize.Text = ((Label)gvr.FindControl("lblSize")).Text;
                                lblPlanNamePopup.Text = CommonFunctions.GetShortString(txtPlanName.Text, 35);
                                lblPlanNamePopup.ToolTip = txtPlanName.Text;
                                lblFromDatePopup.Text = txtFromDate.Text;
                                lblToDatePopup.Text = txtToDate.Text;
                                lblRequiredbyPopup.Text = Convert.ToDateTime(((Label)gvr.FindControl("lblRequiredby")).Text).ToShortDateString();
                                lstLineDetails = new List<LineDetails>();
                                lstLineDetails = objPlanDtls.Details.Where(dtl => dtl.PND_PK == Convert.ToInt32(hdfLinePlanGroupDtlPK.Value)).SelectMany(x => x.LineDetails).ToList();
                                SetFieldValues(ControlsEnum.LINEGRID);
                                divLinePopUp.Attributes.Add("class", GetLocalResourceObject("Css_ViewLinepopUp").ToString());
                                ShowLinePopup(1);
                            }
                        }
                        break;
                    #endregion

                    #region INTERNAL ORDER DETAILS
                    case ActionsEnum.DETAILS:
                        if (((LinkButton)sender).ID == "lnkOrderNo")
                        {
                            gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfSohPk = (HiddenField)gvr.FindControl("hdfSohPk") as HiddenField;
                            ShowSCPopUp();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + hdfSohPk.Value + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                        }
                        break;
                    #endregion

                    #region REVERT
                    case ActionsEnum.REVERT:
                        if (!IsValid)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            result = OrderPlanningBL.RevertOrderPlan(CurPK, currentUser.PKUser, lastModDate);
                            if (result > 0)
                            {
                                hdnTabListNew.Value = "2";
                                GetFieldValues(ControlsEnum.EDIT);
                                SetFieldValues(ControlsEnum.EDIT);
                                GetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                                SetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                                EntryStatus = EntryStatus.EDITMODE;
                            }
                            else
                            {
                                DbSaveStatus saveStatus = (DbSaveStatus)result;
                                switch (saveStatus)
                                {
                                    case DbSaveStatus.SQLERROR:
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Sql_Error, GetLocalResourceObject("OrderPlan").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                        break;
                                    case DbSaveStatus.CONCURRENCY:
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Save_Error_Concurrent, GetLocalResourceObject("OrderPlan").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                        break;
                                    case DbSaveStatus.ALREADYDELETED:
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Already_Deleted, GetLocalResourceObject("OrderPlan").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                        break;
                                    default:
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_SavError, GetLocalResourceObject("OrderPlan").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region ACTIVATE
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPlanlistPK")).Value);
                        lastModDate = ((HiddenField)gvr.FindControl("hdfModedt")).Value;
                        // check activated or not : Success - Return PK
                        result = OrderPlanningBL.UpdateOrderPlanStatus(CurPK, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.PKUser, lastModDate);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlsEnum.PLANLISTING);
                        SetFieldValues(ControlsEnum.PLANLISTING);
                        break;
                    #endregion

                    #region DEACTIVATE
                    // Do Action if click DeActivate Button
                    case ActionsEnum.DEACTIVATE:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPlanlistPK")).Value);
                        lastModDate = ((HiddenField)gvr.FindControl("hdfModedt")).Value;
                        // check activated or not : Success - Return PK
                        result = OrderPlanningBL.UpdateOrderPlanStatus(CurPK, Convert.ToInt32(DbActiveStatus.INACTIVE), currentUser.PKUser, lastModDate);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlsEnum.PLANLISTING);
                        SetFieldValues(ControlsEnum.PLANLISTING);
                        break;
                    #endregion

                    #region SEARCHFILTER
                    case ActionsEnum.SEARCHFILTER:
                        GetFieldValues(ControlsEnum.PLANLISTING);
                        SetFieldValues(ControlsEnum.PLANLISTING);
                        break;
                    #endregion

                    #region CLEARFILTER
                    case ActionsEnum.CLEARFILTER:
                        ClearSearch();
                        GetFieldValues(ControlsEnum.PLANLISTING);
                        SetFieldValues(ControlsEnum.PLANLISTING);
                        break;
                    #endregion

                    #region SUMMARY LINES
                    case ActionsEnum.SUMMARYLINES:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdLineSummary") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                dtSummary = null;
                            }
                            else
                            {
                                SummaryType = 2;
                                SummaryTypePK = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.SUMMARYDETAILS);
                            }
                            grd.Visible = true;
                            if (dtSummary != null && dtSummary.Rows.Count > 0)
                            {
                                grd.DataSource = dtSummary;
                                grd.DataBind();
                            }
                            (gvr.FindControl("hdfIsExpandedPlantItem") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion

                    #region SUMMARY GROUP DETAILS
                    case ActionsEnum.SUMMARYGROUP:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdPGroupSummary") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                dtSummary = null;
                            }
                            else
                            {
                                SummaryType = 3;
                                SummaryTypePK = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.SUMMARYDETAILS);
                            }
                            grd.Visible = true;
                            if (dtSummary != null && dtSummary.Rows.Count > 0)
                            {
                                grd.DataSource = dtSummary;
                                grd.DataBind();
                            }
                            (gvr.FindControl("hdfIsExpandedLineItem") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion

                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (CurPK > 0)
                        {
                            //Same as that of Version wise report - Changed on 16-03-2018
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + CurPK.ToString() + "&APPTYPE=" + ApplicationType.OPLN + "&APPSUBTYPE=0") + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + CurPK.ToString() + "&APPTYPE=" + ApplicationType.OPLN + "&APPSUBTYPE=0") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("PrintError").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError0", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;

                    case ActionsEnum.PRINTLIST:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPlanlistPK")).Value);
                        if (CurPK > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + CurPK.ToString() + "&APPTYPE=" + ApplicationType.OPLN + "&APPSUBTYPE=2&FilterDate=" + DateTime.Now) + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("PrintError").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError0", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;
                    #endregion

                    #region PRINT VERSIONS
                    case ActionsEnum.PRINTRECORD:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        LinkButton lnkVersionName = (LinkButton)gvr.FindControl("lnkVersionName");
                        if (CurPK > 0 && !string.IsNullOrEmpty(lnkVersionName.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + CurPK.ToString() + "&VersionNo=" + lnkVersionName.Text + "&APPTYPE=" + ApplicationType.OPLN + "&APPSUBTYPE=1") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("PrintError").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError0", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        ShowPlanVersionPopUp();
                        break;
                    #endregion

                    #region CHECKBOX SELECT
                    case ActionsEnum.CHECK_CHANGE:
                        if (((CheckBox)sender).ID == "ChkPlanAll")
                        {
                            CheckSelectAll();
                        }
                        else if (((CheckBox)sender).ID == "ChkPlanOrder")
                        {
                            CheckSelectItem(sender);
                        }
                        else if (((CheckBox)sender).ID == "chkAllCombinedLines")
                        {
                            CheckAllCombinedLines();
                        }
                        break;
                    #endregion

                    #region Scenario Details
                    case ActionsEnum.SCENARIOGROUPITEMS:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        //CheckBox ChkPlanOrder = (CheckBox)gvr.FindControl("ChkPlanOrder") as CheckBox;
                        Label lblBalancetoPlanScen = (Label)gvr.FindControl("lblBalancetoPlan") as Label;
                        System.Web.UI.UserControl txtTotalCurrPlanScen = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan") as System.Web.UI.UserControl;
                        TextBox txtFormattedAmountScen = txtTotalCurrPlanScen.FindControl("txtFormattedAmount") as TextBox;
                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdSizeScenario") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                lstSizeDetails = null;
                            }
                            else
                            {
                                PlanGroupPK = Convert.ToInt32(arg);
                                lstSizeDetails = new List<SizeDetails>();
                                lstSizeDetails = objPlanDtls.Details.Where(dtl => dtl.PND_PLAN_GROUP == PlanGroupPK).SelectMany(x => x.SizeDetail).ToList();
                            }
                            grd.Visible = true;
                            if (lstSizeDetails != null && lstSizeDetails.Count > 0)
                            {
                                grd.DataSource = lstSizeDetails;
                                grd.DataBind();
                            }
                            double sizeSum = 0;
                            foreach (GridViewRow gvrRow in grd.Rows)
                            {
                                string percVal = "0";
                                double OrderBal = !string.IsNullOrEmpty((gvrRow.FindControl("lblSBaltoPlan") as Label).Text) ? Convert.ToDouble((gvrRow.FindControl("lblSBaltoPlan") as Label).Text.Replace(",", "")) : 0;
                                double TotalBal = !string.IsNullOrEmpty(lblBalancetoPlanScen.Text) ? Convert.ToDouble(lblBalancetoPlanScen.Text.Replace(",", "")) : 0;

                                if (TotalBal > 0)
                                {
                                    percVal = ((OrderBal * 100) / TotalBal).ToString();
                                    (gvrRow.FindControl("hdfProportionVal") as HiddenField).Value = percVal;
                                    double val = txtFormattedAmountScen.Text == string.Empty ? 0 : Convert.ToDouble(txtFormattedAmountScen.Text);
                                    (gvrRow.FindControl("txtDtlPlanNow") as TextBox).Text = Math.Round((val / 100 * Convert.ToDouble(percVal))).ToString();

                                    sizeSum = Math.Round(sizeSum + (val / 100 * Convert.ToDouble(percVal)));
                                }
                            }

                            double TotSum = txtFormattedAmountScen.Text == string.Empty ? 0 : Convert.ToDouble(txtFormattedAmountScen.Text);
                            if (sizeSum < TotSum && grd.Rows.Count > 0)
                            {
                                double diffQty = TotSum - sizeSum;
                                double plnqty = (grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text);
                                (grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text = GetFormattedNumber(plnqty + diffQty);
                            }
                            else if (sizeSum > TotSum && grd.Rows.Count > 0)
                            {
                                double diffQty = sizeSum - TotSum;
                                double plnqty = (grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text);
                                (grd.Rows[grd.Rows.Count - 1].FindControl("txtDtlPlanNow") as TextBox).Text = GetFormattedNumber(plnqty - diffQty);
                            }
                        }
                        (gvr.FindControl("hdfIsExpandedGroupItem") as HiddenField).Value = "1";
                        break;
                    #endregion

                    #region Calculate
                    case ActionsEnum.CALCULATE:
                        if (((Button)sender).ID == "btnCalculate")
                        {
                            GetFieldValues(ControlsEnum.SCENARIODETAILS);
                            SetFieldValues(ControlsEnum.SCENARIODETAILS);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                        }
                        else if (((Button)sender).ID == "btnPlanCalculate")
                        {
                            if (CheckPlanNow() == 1)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_PlanNow").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                                break;
                            }
                            GetFieldValues(ControlsEnum.PLANCALCULATION);
                            SetFieldValues(ControlsEnum.PLANCALCULATION);
                        }
                        break;
                    #endregion   

                    #region Machines
                    case ActionsEnum.MACHINES:

                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        ScenarioHdrIndex = gvr.RowIndex;
                        HiddenField hdfPlnGrp = ((HiddenField)gvr.FindControl("hdfPlanGroupPK"));
                        chkCombinedLines.Checked = Convert.ToBoolean(objPlanDtls.Details[ScenarioHdrIndex].LINE_COMBINED);
                        grdMachines.DataSource = objLineBO.Detail.SingleOrDefault(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfPlnGrp.Value)).LineDetail;//objLineBO.Detail[ScenarioHdrIndex].LineDetail;
                        grdMachines.DataBind();

                        //chkAllActive.
                        int count = 0;
                        if (grdMachines.Rows.Count > 0)
                        {
                            CheckBox chkAllActive = (CheckBox)grdMachines.HeaderRow.FindControl("chkAllActive");
                            if (objLineBO.Detail[ScenarioHdrIndex].LineDetail.Count(x => x.Ischecked == 1) == objLineBO.Detail[ScenarioHdrIndex].LineDetail.Count)
                                count = 1;
                            chkAllActive.Checked = count == 0 ? false : true;
                        }

                        ShowMachinePopUp();
                        break;
                    #endregion   

                    #region Save Plan
                    case ActionsEnum.SAVEPLAN:
                        UpdateScenarioSaveList();
                        SavePlanScenario(objPlanDtls, 0);
                        break;
                    #endregion  

                    #region Search
                    case ActionsEnum.SEARCH:
                        ActionHandler(ddlLine, EventArgs.Empty);
                        break;
                        #endregion


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion



        #region GridEvents
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (sender is GridView)
                {
                    if (((GridView)sender).ID == "grdPlanList")
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            HiddenField hdfIsDefault = (HiddenField)e.Row.Cells[0].FindControl("hdfIsActive");
                            if (hdfIsDefault.Value == "1")
                            {
                                ImageButton imbInActive = (ImageButton)e.Row.Cells[1].FindControl("imbInActive");
                                imbInActive.Visible = true;
                            }
                            else
                            {
                                ImageButton imbActive = (ImageButton)e.Row.Cells[1].FindControl("imbActive");
                                imbActive.Visible = true;
                            }
                        }
                    }
                    else if (((GridView)sender).ID == "grdLines")
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            HiddenField hdfLineCapacity = (HiddenField)e.Row.Cells[0].FindControl("hdfLineCapacity");
                            Label lblLineCapacity = (Label)e.Row.Cells[0].FindControl("lblLineCapacity");
                            double Capacity = hdfLineCapacity.Value != string.Empty ? Convert.ToDouble(hdfLineCapacity.Value) : 0;
                            //TimeSpan DateDiff = Convert.ToDateTime(txtToDate.Text).Subtract(Convert.ToDateTime(txtFromDate.Text));
                            // string fromDate = txtLineFromDt.Text;
                            TimeSpan DateDiff;
                            //lblLineCapacity.Text = "0";


                            HiddenField hdfLineFromDate = (HiddenField)e.Row.FindControl("hdfLineFromDate");
                            HiddenField hdfLineToDate = (HiddenField)e.Row.FindControl("hdfLineToDate");

                            if (hdfLineFromDate.Value != string.Empty && hdfLineToDate.Value != string.Empty)
                            {
                                string fromDate = (Convert.ToDateTime(hdfLineFromDate.Value).ToString(CommonConstants.DATEFORMAT)).ToString();
                                string ToDate = (Convert.ToDateTime(hdfLineToDate.Value).ToString(CommonConstants.DATEFORMAT)).ToString();
                                DateDiff = Convert.ToDateTime(ToDate).Subtract(Convert.ToDateTime(fromDate));
                                lblLineCapacity.Text = ((DateDiff.Days + 1) * Capacity) > 0 ? GetFormattedNumber(Math.Ceiling((DateDiff.Days + 1) * Capacity)) : "0";
                            }
                            if (Convert.ToDateTime(Convert.ToDateTime(hdfLineToDate.Value).ToShortDateString()) > Convert.ToDateTime(txtToDate.Text))
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("LineScenarioExceedColor").ToString());

                        }
                    }
                    else if (((GridView)sender).ID == "grdLinewiseSummary")
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            Label lblSCapacity = (Label)e.Row.Cells[0].FindControl("lblSCapacity");
                            //Label lblSPlanned = (Label)e.Row.Cells[0].FindControl("lblSPlanned");
                            HiddenField hdfPlanned = (HiddenField)e.Row.Cells[0].FindControl("hdfPlanned");
                            //Label lblSPlannedPerc = (Label)e.Row.Cells[0].FindControl("lblSPlannedPerc");
                            //Label lblSPlannedBalance = (Label)e.Row.Cells[0].FindControl("lblSPlannedBalance");
                            HiddenField hdfQtyBal = (HiddenField)e.Row.Cells[0].FindControl("hdfQtyBal");
                            //Label lblSProduced = (Label)e.Row.Cells[0].FindControl("lblSProduced");
                            HiddenField hdfProdPcs = (HiddenField)e.Row.Cells[0].FindControl("hdfProdPcs");
                            //Label lblSProducedPerc = (Label)e.Row.Cells[0].FindControl("lblSProducedPerc");
                            Label lblSProducedBalance = (Label)e.Row.Cells[0].FindControl("lblSProducedBalance");

                            double SCapacity = double.Parse(ReplaceComma(lblSCapacity.Text));
                            double SPlannedPcs = double.Parse(ReplaceComma(hdfPlanned.Value));
                            //double SPlannedPerc = double.Parse(ReplaceComma(lblSPlannedPerc.Text));
                            double SPlannedBalance = double.Parse(ReplaceComma(hdfQtyBal.Value));
                            double SProduced = double.Parse(ReplaceComma(hdfProdPcs.Value));
                            //double SProducedPerc = double.Parse(ReplaceComma(lblSProducedPerc.Text));
                            double SProducedBalance = double.Parse(ReplaceComma(lblSProducedBalance.Text));

                            TotalSCapacity = TotalSCapacity + SCapacity;
                            TotalSPlannedPcs = TotalSPlannedPcs + SPlannedPcs;
                            //TotalSPlannedPerc = TotalSPlannedPerc + SPlannedPerc;
                            TotalSPlannedBalance = TotalSPlannedBalance + SPlannedBalance;
                            TotalSProducedPcs = TotalSProducedPcs + SProduced;
                            //TotalSProducedPerc = TotalSProducedPerc + SProducedPerc;
                            TotalSProducedBalance = TotalSProducedBalance + SProducedBalance;
                        }
                        if (e.Row.RowType == DataControlRowType.Footer)
                        {
                            ((Label)e.Row.Cells[0].FindControl("lblTotalCapacity")).Text = GetFormattedNumber(TotalSCapacity);
                            ((Label)e.Row.Cells[0].FindControl("lblTotalPlanned")).Text = GetFormattedNumber(TotalSPlannedPcs);
                            //((Label)e.Row.Cells[0].FindControl("lblTotalPlannedPerc")).Text = GetFormattedWeightwithComma((TotalSPlannedPcs * 100) / TotalSCapacity);
                            ((Label)e.Row.Cells[0].FindControl("lblTotalPlannedBalance")).Text = GetFormattedNumber(TotalSPlannedBalance);
                            ((Label)e.Row.Cells[0].FindControl("lblTotalProduced")).Text = GetFormattedNumber(TotalSProducedPcs);
                            //((Label)e.Row.Cells[0].FindControl("lblTotalProducedPerc")).Text = GetFormattedWeightwithComma((TotalSProducedPcs * 100) / TotalSCapacity);
                            ((Label)e.Row.Cells[0].FindControl("lblTotalProducedBalance")).Text = GetFormattedNumber(TotalSProducedBalance);
                        }

                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            e.Row.Cells[4].ColumnSpan = 2;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].ColumnSpan = 2;
                            e.Row.Cells[7].Visible = false;
                            e.Row.Cells[8].ColumnSpan = 2;
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[10].ColumnSpan = 2;
                            e.Row.Cells[11].Visible = false;
                        }
                    }
                    else if (((GridView)sender).ID == "grdLineScenario")
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            if (Convert.ToDateTime(Convert.ToDateTime(((HiddenField)e.Row.Cells[0].FindControl("hdfLineScenarioEndDate")).Value).ToShortDateString()) > Convert.ToDateTime(txtToDate.Text))
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("LineScenarioExceedColor").ToString());

                            if (((Label)e.Row.Cells[0].FindControl("lblDeadLineMet")).Text == "No")
                                e.Row.CssClass = "highlight";

                            Label lblLineQty = (Label)e.Row.Cells[0].FindControl("lblLineQty");
                            double LineQty = double.Parse(ReplaceComma(lblLineQty.Text));
                            TotalScenQtyPlan = TotalScenQtyPlan + LineQty;

                            Label ProdHrs = (Label)e.Row.Cells[0].FindControl("lblProdHrs");
                            double LineProdHrs = double.Parse(ReplaceComma(ProdHrs.Text));
                            TotalLineScenPrdHrs = TotalLineScenPrdHrs + LineProdHrs;
                        }
                        if (e.Row.RowType == DataControlRowType.Footer)
                        {
                            ((Label)e.Row.Cells[0].FindControl("lblTotalLineQty")).Text = GetFormattedNumber(TotalScenQtyPlan);
                            ((Label)e.Row.Cells[0].FindControl("lblTotalProdHrs")).Text = GetFormattedNumber(TotalLineScenPrdHrs);
                        }
                    }
                }
                else if (sender is ExtGridView)
                {
                    if (((ExtGridView)sender).ID == "grdSummaryPlant")
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            Label lblPlannedPerc = (Label)e.Row.Cells[0].FindControl("lblPlannedPerc");
                            Label lblPlannedPcs = (Label)e.Row.Cells[0].FindControl("lblPlannedPcs");
                            Label lblProducedPerc = (Label)e.Row.Cells[0].FindControl("lblProducedPerc");
                            Label lblProducedPcs = (Label)e.Row.Cells[0].FindControl("lblProducedPcs");
                            Label lblBalancePerc = (Label)e.Row.Cells[0].FindControl("lblBalancePerc");
                            Label lblBalancePcs = (Label)e.Row.Cells[0].FindControl("lblBalancePcs");
                            double PlannedPerc = double.Parse(ReplaceComma(lblPlannedPerc.Text));
                            double PlannedPcs = double.Parse(ReplaceComma(lblPlannedPcs.Text));
                            double ProducedPerc = double.Parse(ReplaceComma(lblProducedPerc.Text));
                            double ProducedPcs = double.Parse(ReplaceComma(lblProducedPcs.Text));
                            double BalancePerc = double.Parse(ReplaceComma(lblBalancePerc.Text));
                            double BalancePcs = double.Parse(ReplaceComma(lblBalancePcs.Text));

                            TotalPlannedPerc = TotalPlannedPerc + PlannedPerc;
                            TotalPlannedPcs = TotalPlannedPcs + PlannedPcs;
                            TotalProducedPerc = TotalProducedPerc + ProducedPerc;
                            TotalProducedPcs = TotalProducedPcs + ProducedPcs;
                            TotalBalancePerc = TotalBalancePerc + BalancePerc;
                            TotalBalancePcs = TotalBalancePcs + BalancePcs;
                        }
                        if (e.Row.RowType == DataControlRowType.Footer)
                        {
                            Label lblPlannedPercTotal = (Label)e.Row.Cells[0].FindControl("lblPlannedPercTotal");
                            Label lblPlannedPcsTotal = (Label)e.Row.Cells[0].FindControl("lblPlannedPcsTotal");
                            Label lblProducedPercTotal = (Label)e.Row.Cells[0].FindControl("lblProducedPercTotal");
                            Label lblProducedPcsTotal = (Label)e.Row.Cells[0].FindControl("lblProducedPcsTotal");
                            Label lblBalancePercTotal = (Label)e.Row.Cells[0].FindControl("lblBalancePercTotal");
                            Label lblBalancePcsTotal = (Label)e.Row.Cells[0].FindControl("lblBalancePcsTotal");

                            lblPlannedPercTotal.Text = GetFormattedWeightwithComma(TotalPlannedPerc);
                            lblPlannedPcsTotal.Text = GetFormattedNumber(TotalPlannedPcs);
                            lblProducedPercTotal.Text = GetFormattedWeightwithComma(TotalProducedPerc);
                            lblProducedPcsTotal.Text = GetFormattedNumber(TotalProducedPcs);
                            lblBalancePercTotal.Text = GetFormattedWeightwithComma(TotalBalancePerc);
                            lblBalancePcsTotal.Text = GetFormattedNumber(TotalBalancePcs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            if (((GridView)sender).ID == "grdPlanList")
            {
                // Set Grid Sort Order
                SortExpression = e.SortExpression;// Convert.ToString(fieldInfo.GetValue(0));
                if (SortOrder == CommonConstants.SORT_ASC)
                    SortOrder = CommonConstants.SORT_DESC;
                else
                    SortOrder = CommonConstants.SORT_ASC;
                GetFieldValues(ControlsEnum.PLANLISTING);
                SetFieldValues(ControlsEnum.PLANLISTING);
            }
        }
        #endregion
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum ControlType)
        {
            try
            {
                string xmlString = string.Empty;
                switch (ControlType)
                {
                    case ControlsEnum.PLANLISTING:
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        OPPageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        SortExpression = SortExpression == null ? "PNH_PK" : SortExpression;
                        SortOrder = SortOrder == null ? CommonConstants.SORT_DESC : SortOrder;
                        dtList = OrderPlanningBL.GetPlanList(currentUser.CurrentSBUPK, Convert.ToInt32(PageIndex), OPPageSize, Convert.ToInt32(hdfFilterPlanPK.Value), Convert.ToInt32(ddlFilterStatus.SelectedValue), txtFilterFrom.Text, txtFilterTo.Text, SortExpression, SortOrder);
                        break;
                    case ControlsEnum.EDIT:
                        objPlanDtls = OrderPlanningBL.GetOrderPlan(CurPK);
                        break;
                    case ControlsEnum.PRODUCTPLANGROUPLIST:
                        if (objPlanDtls.Details != null && objPlanDtls.Details.Count > 0)
                            lstOrders = objPlanDtls.Details.OrderBy(d => Convert.ToDateTime(d.PND_REQUIRED_DT)).ThenByDescending(x => x.PND_PLAN_QTY).ToList(); ;
                        break;
                    case ControlsEnum.LINELIST:
                        dtLine = OrderPlanningBL.GetLinestByProductGroup(linePK, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK, Convert.ToInt32(hdfLinePlanGroup.Value), Convert.ToInt32(hdfLinePlanGroupSize.Value));
                        break;
                    case ControlsEnum.LINEDETAILS:
                        //UpdateDetailList();  
                        List<OrderDetails> objDtls = new List<OrderDetails>();
                        objDtls = objPlanDtls.Details.Where(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfLinePlanGroup.Value)).ToList();
                        //objBO.Details = objDtls;
                        //if (objPlnLineBo == null)
                        //    objPlnLineBo = new PLanLineBO();
                        objPlnLineBo = new PLanLineBO();

                        objPlnLineBo.BIZUNIT = currentUser.CurrentSBUPK;
                        objPlnLineBo.AVG_SPEED = ucLineSpeed.Text == string.Empty ? 0 : Convert.ToDouble(ucLineSpeed.Text);
                        objPlnLineBo.ITEM_GP = Convert.ToInt32(hdfLinePlanGroup.Value);
                        objPlnLineBo.LNE_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objPlnLineBo.LNE_PK = Convert.ToInt32(ddlLine.SelectedValue);
                        objPlnLineBo.PND_PK = Convert.ToInt32(hdfLinePlanGroupDtlPK.Value);
                        objPlnLineBo.PNH_FROM_DT = lblFromDatePopup.Text;

                        DateTime StartDate, TtempDate, tempTime;
                        if (txtLineFromTime.Text != string.Empty && txtLineFromTime.Text != string.Empty)
                        {
                            tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineFromTime.Text.Trim()));
                            if (!DateTime.TryParse(txtLineFromDt.Text, out TtempDate))
                                TtempDate = DateTime.Now;
                            StartDate = SetTime(TtempDate, tempTime);
                            objPlnLineBo.PNL_FROM_DATE = StartDate.ToString();
                        }

                        objPlnLineBo.TOTAL_QTY = ucLineQty.Text == string.Empty ? 0 : Convert.ToDouble(ucLineQty.Text);

                        //objPlnLineBo.Detail = (PLanLineDetails)objDtls[0].SizeDetail.ToList();


                        if (!string.IsNullOrEmpty(ucLineQty.Text) && Convert.ToDouble(ucLineQty.Text) > 0)
                        {
                            double percVal = 0;
                            double TotalBal = objDtls[0].SOD_TOT_BAL_TO_PLAN;
                            objDtls[0].SizeDetail.ToList().ForEach(dtl =>
                            {
                                percVal = ((dtl.SOD_BAL_TO_PLAN * 100) / TotalBal);
                                dtl.SIZE_PLAN_QTY = Math.Round(((Convert.ToDouble(ucLineQty.Text) * Convert.ToDouble(percVal)) / 100));
                            });
                            objPlnLineBo.Detail = (from order in objDtls[0].SizeDetail
                                                   select new PLanLineDetails
                                                   {
                                                       PNS_PLAN_QTY = order.SIZE_PLAN_QTY,
                                                       PLD_SIZ_PK = order.PNS_SIZE,
                                                   }).ToList();

                            XmlDocument xmlDocPlnLne = CommonFunctions.ObjectTOXml(objPlnLineBo);
                            objPlnLineBo = OrderPlanningBL.GetLineDetails(xmlDocPlnLne.InnerXml);
                        }
                        break;
                    case ControlsEnum.SUMMARYDETAILS:
                        dtSummary = OrderPlanningBL.GetSummaryDetails(CurPK, SummaryType, SummaryTypePK);
                        break;
                    case ControlsEnum.PLANVERSIONS:
                        dtVersions = OrderPlanningBL.GetPlanVersions(CurPK, currentUser.CurrentSBUPK, Convert.ToInt32(DbActiveStatus.HASPK));
                        break;
                    case ControlsEnum.LINEWISESUMMARY:
                        dtLineWiseSummary = OrderPlanningBL.GetLineWiseSummary(CurPK);
                        break;
                    case ControlsEnum.MACHINEFILL:
                        dtPage = OrderPlanningBL.GetAllLine(currentUser.CurrentSBUPK, DeptPk: currentUser.CurrentDeptPK);
                        break;
                    case ControlsEnum.SCENARIODETAILS:
                        UpdateScenarioList();
                        UpdateLineSelection();
                        int ReturnVal = 0;
                        //To get lines even if all products are not mapped with lines pass 0 in this case
                        if (Convert.ToBoolean(GetLocalResourceObject("DefaultCompatibleLines")))
                            objPlanDtls.PLAN_MODE = 1;
                        objPlanDtls.PLAN_OPTIMIZE = chkOptimizeScenario.Checked ? 1 : 0;
                        if (chkOptimizeScenario.Checked == true)
                            objPlanDtls.PLAN_SPLIT = chkScenarioSplit.Checked ? 1 : 0;
                       // XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objPlanDtls);
                        xmlString = CommonFunctions.XmlSerialize<OrderPlanBO>(objPlanDtls);
                        string XmlReturn = OrderPlanningBL.GetScnarioDetails(xmlString, ref ReturnVal);
                        if (XmlReturn != string.Empty && ReturnVal > 0)
                        {
                            objPlanDtls = (OrderPlanBO)CommonFunctions.DeserializeObject(XmlReturn, new OrderPlanBO());
                        }
                        else
                        {
                            if (ReturnVal == -1)
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (ReturnVal == -11)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_MultipleGroupNotAllowed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    case ControlsEnum.GROUPLINES:
                        objLineBO = (LineBO)SetUIValuestoObject(ControlsEnum.GROUPLINES);
                        //XmlDocument LinexmlDoc = CommonFunctions.ObjectTOXml(objLineBO);
                        xmlString = CommonFunctions.XmlSerialize<LineBO>(objLineBO);
                        objLineBO = OrderPlanningBL.GetGroupLines(xmlString);
                        break;
                    case ControlsEnum.PLANGROUPLINES:
                        objLineBO = (LineBO)SetUIValuestoObject(ControlsEnum.PLANGROUPLINES);
                        xmlString = CommonFunctions.XmlSerialize<LineBO>(objLineBO);
                       // XmlDocument PlanLinexmlDoc = CommonFunctions.ObjectTOXml(objLineBO);
                        objLineBO = OrderPlanningBL.GetGroupLines(xmlString);
                        break;
                    case ControlsEnum.PLANCALCULATION:
                        UpdateDetailList();
                        UpdateLineSelection();
                        int Return = 0;
                        //To get lines even if all products are not mapped with lines pass 0 in this case
                        if (Convert.ToBoolean(GetLocalResourceObject("DefaultCompatibleLines")))
                            objPlanDtls.PLAN_MODE = 1;
                        objPlanDtls.PLAN_OPTIMIZE = chkOptimizePlan.Checked ? 1 : 0;
                        if (chkOptimizePlan.Checked == true)
                            objPlanDtls.PLAN_SPLIT = chkPlanSplit.Checked ? 1 : 0;
                        //XmlDocument xmlDocPlan = CommonFunctions.ObjectTOXml(objPlanDtls);
                        xmlString = CommonFunctions.XmlSerialize<OrderPlanBO>(objPlanDtls);

                        string XmlReturnPlan = OrderPlanningBL.GetScnarioDetails(xmlString, ref Return);
                        if (XmlReturnPlan != string.Empty && Return > 0)
                        {
                            objPlanDtls = (OrderPlanBO)CommonFunctions.DeserializeObject(XmlReturnPlan, new OrderPlanBO());
                        }
                        else
                        {
                            if (Return == -1)
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else if (Return == -11)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_MultipleGroupNotAllowed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum ControlType)
        {
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    case ControlsEnum.PLANLISTING:
                        BindGrid(ControlsEnum.PLANLISTING);
                        break;
                    case ControlsEnum.PRODUCTPLANGROUPLIST:
                        BindGrid(ControlsEnum.PRODUCTPLANGROUPLIST);
                        break;
                    case ControlsEnum.LINELIST:
                        BindDropDown(ControlsEnum.LINELIST);
                        break;
                    case ControlsEnum.LINEGRID:
                        BindGrid(ControlsEnum.LINEGRID);
                        break;
                    case ControlsEnum.SUMMARYGROUPLIST:
                        BindGrid(ControlsEnum.SUMMARYGROUPLIST);
                        break;
                    case ControlsEnum.PLANVERSIONS:
                        BindGrid(ControlsEnum.PLANVERSIONS);
                        break;
                    case ControlsEnum.LINEWISESUMMARY:
                        BindGrid(ControlsEnum.LINEWISESUMMARY);
                        break;
                    case ControlsEnum.SCDETAILS:
                        BindGrid(ControlsEnum.SCDETAILS);
                        break;
                    case ControlsEnum.SCENARIODETAILS:
                        BindGrid(ControlsEnum.SCENARIODETAILS);
                        break;
                    case ControlsEnum.MACHINEFILL:
                        BindGrid(ControlsEnum.MACHINEFILL);
                        break;
                    case ControlsEnum.LINEFORMERDETAILS:
                        BindGrid(ControlsEnum.LINEFORMERDETAILS);
                        break;
                    case ControlsEnum.PLANCALCULATION:
                        BindGrid(ControlsEnum.PLANCALCULATION);
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region HelperMethods
        #region Set UI Values to Object
        private Object SetUIValuestoObject(ControlsEnum ControlType)
        {
            Object retObject = null;
            switch (ControlType)
            {
                case ControlsEnum.PLANHEADER:
                    OrderPlanBO objPlanHeader = new OrderPlanBO();
                    objPlanHeader.PNH_PK = !string.IsNullOrEmpty(hdfPlanPK.Value) ? Convert.ToInt32(hdfPlanPK.Value) : 0;
                    objPlanHeader.PNH_NAME = txtPlanName.Text;
                    objPlanHeader.PNH_CODE = txtPlanCode.Text;
                    objPlanHeader.PNH_FROM_DT = Convert.ToDateTime(txtFromDate.Text).ToShortDateString();
                    objPlanHeader.PNH_TO_DT = Convert.ToDateTime(txtToDate.Text).ToShortDateString();
                    objPlanHeader.PNH_TAB = Convert.ToInt32(hdnTabListNew.Value);
                    objPlanHeader.BIH_DEPT = currentUser.CurrentDeptPK;
                    objPlanHeader.BIZUNIT_PK = currentUser.CurrentSBUPK;
                    objPlanHeader.USER_PK = currentUser.PKUser;
                    retObject = objPlanHeader;
                    break;
                case ControlsEnum.GROUPLINES:
                    LineBO objLineBO = new LineBO();

                    List<PlanGroupDetails> plngr = new List<PlanGroupDetails>();
                    foreach (OrderDetails groups in objPlanDtls.Details)
                    {
                        //objLineBO.Detail = (from sc in objPlanDtls.Details
                        //                    where sc.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP
                        //                    select new PlanGroupDetails
                        //                    {
                        //                        PND_PLAN_GROUP = sc.PND_PLAN_GROUP,
                        //                        PND_SL_NO = sc.PND_SL_NO,
                        //                        SODetail = sc.SizeDetail[0].SODetails
                        //                    }).ToList();  

                        PlanGroupDetails objplngr = new PlanGroupDetails();
                        objplngr.PND_PLAN_GROUP = groups.PND_PLAN_GROUP;
                        objplngr.PND_SL_NO = groups.PND_SL_NO;
                        //To get lines even if all products are not mapped with lines pass 1 here
                        if (Convert.ToBoolean(GetLocalResourceObject("DefaultCompatibleLines")))
                            objplngr.LINE_IS_COMPATIBLE = 1;
                        objplngr.SODetail = new List<PlanGroupSODetails>();
                        foreach (SizeDetails sizes in groups.SizeDetail)
                        {
                            foreach (SODetails sods in sizes.SODetails)
                            {
                                PlanGroupSODetails objpl = new PlanGroupSODetails();
                                objpl.PNS_SL_NO = groups.PND_SL_NO;
                                objpl.PNS_SO_DTL = sods.PNS_SO_DTL;
                                objplngr.SODetail.Add(objpl);
                            }
                        }
                        plngr.Add(objplngr);
                        objLineBO.Detail = plngr;
                    }
                    retObject = objLineBO;
                    break;
                case ControlsEnum.PLANGROUPLINES:
                    LineBO objPlanLineBO = new LineBO();
                    List<PlanGroupDetails> Planplngr = new List<PlanGroupDetails>();
                    if (objPlanDtls != null && objPlanDtls.Details != null)
                    {
                        foreach (OrderDetails dtls in objPlanDtls.Details)
                        {
                            PlanGroupDetails objplngr = new PlanGroupDetails();
                            objplngr.PND_PLAN_GROUP = dtls.PND_PLAN_GROUP;
                            objplngr.PND_SL_NO = dtls.PND_SL_NO;
                            objplngr.SODetail = new List<PlanGroupSODetails>();
                            foreach (SizeDetails sizes in dtls.SizeDetail)
                            {
                                foreach (SODetails sods in sizes.SODetails)
                                {
                                    PlanGroupSODetails objpl = new PlanGroupSODetails();
                                    objpl.PNS_SL_NO = sizes.PND_SL_NO;
                                    objpl.PNS_SO_DTL = sods.PNS_SO_DTL;
                                    objplngr.SODetail.Add(objpl);
                                }
                            }
                            Planplngr.Add(objplngr);
                            objPlanLineBO.Detail = Planplngr;
                        }
                    }
                    retObject = objPlanLineBO;
                    break;
            }
            return retObject;
        }
        #endregion

        #region Get UI Values from Object
        private void GetUIValuesFromObject(ControlsEnum ControlType)
        {
            switch (ControlType)
            {
                case ControlsEnum.EDIT:
                    if (objPlanDtls != null)
                    {
                        hdfPlanPK.Value = objPlanDtls.PNH_PK.ToString();
                        txtPlanName.Text = HttpUtility.HtmlDecode(objPlanDtls.PNH_NAME);
                        txtPlanCode.Text = HttpUtility.HtmlDecode(objPlanDtls.PNH_CODE);
                        txtFromDate.Text = Convert.ToDateTime(objPlanDtls.PNH_FROM_DT).ToString(CommonConstants.DATEFORMAT);
                        txtToDate.Text = Convert.ToDateTime(objPlanDtls.PNH_TO_DT).ToString(CommonConstants.DATEFORMAT);
                        lblVersion.Text = objPlanDtls.PNH_VERSION;
                        hdfAGradePerc.Value = objPlanDtls.ISD_AGRADE_PER.ToString();
                        lastModDate = objPlanDtls.LAST_MOD_DT;
                        hdfshowRevert.Value = objPlanDtls.PNH_IS_VER_MODIFIED.ToString();
                        imbShowVersions.Visible = Convert.ToInt32(objPlanDtls.PNH_VERSION) > 0 ? true : false;
                        btnCancelSubmit.Visible = Convert.ToInt32(objPlanDtls.PNH_VERSION) > 0 ? true : false;
                    }
                    break;
            }
        }
        #endregion

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.LINELIST:
                    if (dtLine != null && dtLine.Rows.Count > 0)
                    {
                        ddlLine.DataSource = CommonFunctions.HtmlDecodeDataTable(dtLine, BusinessObject.Constants.OrderPlanning.F_LNE_CODE);
                        ddlLine.DataTextField = BusinessObject.Constants.OrderPlanning.F_LNE_CODE;
                        ddlLine.DataValueField = BusinessObject.Constants.OrderPlanning.F_LNE_PK;
                        ddlLine.DataBind();
                    }
                    ddlLine.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion

        #region BindGrid
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PLANLISTING:
                    if (dtList != null && dtList.Rows.Count > 0)
                    {
                        if ((Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"]) >= 0))
                        {
                            uclPaging.Visible = true;
                            if ((Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"]) % OPPageSize) == 0)
                                uclPaging.TotalPages = Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"]) / OPPageSize;
                            else
                                uclPaging.TotalPages = (Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"]) / OPPageSize) + 1;
                        }
                        grdPlanList.DataSource = dtList;
                    }
                    else
                    {
                        grdPlanList.DataSource = null;
                        uclPaging.TotalPages = 0;
                    }

                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdPlanList.DataBind();
                    uclPaging.BindPager();
                    break;
                case ControlsEnum.PRODUCTPLANGROUPLIST:
                    if (lstOrders != null && lstOrders.Count > 0)
                    {
                        grdGrouplist.DataSource = lstOrders;
                        grdGrouplist.DataBind();
                    }
                    else
                    {
                        grdGrouplist.DataSource = null;
                        grdGrouplist.DataBind();
                    }
                    break;
                case ControlsEnum.LINEGRID:
                    if (lstLineDetails != null && lstLineDetails.Count > 0)
                    {
                        grdLines.DataSource = lstLineDetails;
                        grdLines.DataBind();
                    }
                    else
                    {
                        grdLines.DataSource = null;
                        grdLines.DataBind();
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateLineQtyTotal", "CalculateLineQtyTotal();", true);
                    break;
                //case ControlsEnum.SUMMARYGROUPLIST:
                //    if (dtSummary != null && dtSummary.Rows.Count > 0)
                //    {
                //        grdSummaryPlant.DataSource = dtSummary;
                //        grdSummaryPlant.DataBind();
                //    }
                //    else
                //    {
                //        grdSummaryPlant.DataSource = null;
                //        grdSummaryPlant.DataBind();
                //    }
                //    break;
                case ControlsEnum.PLANVERSIONS:
                    if (dtVersions != null && dtVersions.Rows.Count > 0)
                    {
                        grdVersions.DataSource = dtVersions;
                        grdVersions.DataBind();
                    }
                    else
                    {
                        grdVersions.DataSource = null;
                        grdVersions.DataBind();
                    }
                    break;
                case ControlsEnum.LINEWISESUMMARY:
                    if (dtLineWiseSummary != null && dtLineWiseSummary.Rows.Count > 0)
                    {
                        grdLinewiseSummary.DataSource = dtLineWiseSummary;
                        grdLinewiseSummary.DataBind();
                    }
                    else
                    {
                        grdLinewiseSummary.DataSource = null;
                        grdLinewiseSummary.DataBind();
                    }
                    break;
                case ControlsEnum.SCDETAILS:
                    if (lstSODetails != null && lstSODetails.Count > 0)
                    {
                        grdOrderDtls.DataSource = lstSODetails;
                        grdOrderDtls.DataBind();
                    }
                    else
                    {
                        grdOrderDtls.DataSource = null;
                        grdOrderDtls.DataBind();
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateSCPopUpTotal", "CalculateSCPopUpTotal();", true);
                    break;
                case ControlsEnum.SCENARIODETAILS:
                    if (objPlanDtls != null && objPlanDtls.Details != null)
                    {
                        grdGrouplistScenario.DataSource = objPlanDtls.Details;
                        grdGrouplistScenario.DataBind();
                    }
                    else
                    {
                        grdGrouplistScenario.DataSource = null;
                        grdGrouplistScenario.DataBind();
                    }
                    break;
                case ControlsEnum.MACHINEFILL:
                    grdMachines.DataSource = dtPage;
                    grdMachines.DataBind();
                    break;
                case ControlsEnum.LINEFORMERDETAILS:
                    grdFormerDetails.DataSource = null;
                    grdFormerDetails.DataBind();
                    bool Fillval = false;
                    List<OrderDetails> listTemp = new List<OrderDetails>();
                    listTemp = objPlanDtls.Details.Where(x => x.PND_PLAN_GROUP == Convert.ToInt32(hdfLineProductGroup.Value)).ToList();
                    foreach (OrderDetails dtls in listTemp)
                    {
                        if (dtls.LineDetails != null)
                        {
                            foreach (LineDetails lne in dtls.LineDetails)
                            {
                                if (lne.PNL_LINE == FormerlinePK)
                                {
                                    grdFormerDetails.DataSource = lne.LineFormerDetail;
                                    grdFormerDetails.DataBind();
                                    Fillval = true;
                                }
                            }
                        }
                        if (Fillval)
                            break;
                    }
                    break;
                case ControlsEnum.PLANCALCULATION:
                    if (objPlanDtls != null && objPlanDtls.Details != null)
                    {
                        grdGrouplist.DataSource = objPlanDtls.Details;
                        grdGrouplist.DataBind();
                    }
                    else
                    {
                        grdGrouplist.DataSource = null;
                        grdGrouplist.DataBind();
                    }
                    break;
            }
        }
        #endregion

        #region Update Details to object
        private void UpdateDetailList()
        {
            objPlanDtls.PNH_PK = !string.IsNullOrEmpty(hdfPlanPK.Value) ? Convert.ToInt32(hdfPlanPK.Value) : 0;
            objPlanDtls.PNH_NAME = txtPlanName.Text;
            objPlanDtls.PNH_CODE = txtPlanCode.Text;
            objPlanDtls.PNH_FROM_DT = Convert.ToDateTime(txtFromDate.Text).ToShortDateString();
            objPlanDtls.PNH_TO_DT = Convert.ToDateTime(txtToDate.Text).ToShortDateString();
            objPlanDtls.PNH_TAB = Convert.ToInt32(hdnTabListNew.Value);
            objPlanDtls.BIH_DEPT = currentUser.CurrentDeptPK;
            objPlanDtls.BIZUNIT_PK = currentUser.CurrentSBUPK;
            objPlanDtls.USER_PK = currentUser.PKUser;
            objPlanDtls.PNH_VERSION = !string.IsNullOrEmpty(lblVersion.Text) ? lblVersion.Text : "0";
            objPlanDtls.LAST_MOD_DT = lastModDate;
            if (isFinalize == 1)
            {
                objPlanDtls.PNH_IS_FINALIZED = 1;
                objPlanDtls.IS_REVISE = 1;
            }
            else
                objPlanDtls.IS_REVISE = isRevise;
            foreach (GridViewRow gvr in grdGrouplist.Rows)
            {
                CheckBox ChkPlanOrder = (CheckBox)gvr.FindControl("ChkPlanOrder") as CheckBox;
                HiddenField hdfPlanDtlPK = (HiddenField)gvr.FindControl("hdfPlanDtlPK") as HiddenField;
                System.Web.UI.UserControl ucrNumericControl = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan") as System.Web.UI.UserControl;
                TextBox txtTotalCurrPlan = ucrNumericControl.FindControl("txtFormattedAmount") as TextBox;
                Label lblRequiredby = (Label)gvr.FindControl("lblRequiredby") as Label;

                objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).PND_IS_CHECKED = ChkPlanOrder.Checked ? 1 : 0;
                objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).PND_PLAN_QTY = !string.IsNullOrEmpty(txtTotalCurrPlan.Text) ? Convert.ToDouble(txtTotalCurrPlan.Text) : 0;
                objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).PND_REQUIRED_DT = Convert.ToDateTime(lblRequiredby.Text).ToShortDateString();

                if (ChkPlanOrder.Checked == false)
                    objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).LineDetails = null;

                if ((gvr.FindControl("grdSize") as GridView).Rows.Count > 0)
                {
                    foreach (GridViewRow gvrRow in (gvr.FindControl("grdSize") as GridView).Rows)
                    {
                        HiddenField hdfSizePlanGroup = (HiddenField)gvrRow.FindControl("hdfSizePlanGroup");
                        HiddenField hdfSizeVal = (HiddenField)gvrRow.FindControl("hdfSizeVal");
                        System.Web.UI.UserControl ucrNumericControlDtl = (System.Web.UI.UserControl)gvrRow.FindControl("txtSizePlanNow");
                        TextBox txtSizePlanNow = ucrNumericControlDtl.FindControl("txtFormattedAmount") as TextBox;
                        HiddenField hdfIsApplied = (HiddenField)gvrRow.FindControl("hdfIsApplied");
                        objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).SizeDetail.Where(y => y.SIZE_PLAN_GROUP == Convert.ToInt32(hdfSizePlanGroup.Value) && y.PNS_SIZE == Convert.ToInt32(hdfSizeVal.Value)).ToList().ForEach(dtl =>
                        {
                            dtl.SIZE_PLAN_QTY = !string.IsNullOrEmpty(txtSizePlanNow.Text) ? Convert.ToDouble(txtSizePlanNow.Text) : 0;
                        });
                        lstSizeDetails = objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).SizeDetail.Where(y => y.SIZE_PLAN_GROUP == Convert.ToInt32(hdfSizePlanGroup.Value) && y.PNS_SIZE == Convert.ToInt32(hdfSizeVal.Value)).ToList();

                        double SoSum = 0;
                        if (!string.IsNullOrEmpty(txtSizePlanNow.Text) && Convert.ToDouble(txtSizePlanNow.Text) > 0)
                        {
                            foreach (SizeDetails items in lstSizeDetails)
                            {
                                if (hdfIsApplied.Value == "0")
                                {
                                    items.SODetails.ForEach(x =>
                                    {
                                        x.PNS_PLAN_QTY = Math.Round(Convert.ToDouble(GetFormattedNumber((items.SIZE_PLAN_QTY * x.SOD_PERCENTAGE) / 100)));
                                        SoSum = SoSum + x.PNS_PLAN_QTY;
                                    });
                                }
                                else
                                {
                                    items.SODetails.ForEach(x =>
                                    {
                                        SoSum = SoSum + x.PNS_PLAN_QTY;
                                    });
                                }
                            }
                        }

                        //for correct pcs count start 
                        double TotSum = !string.IsNullOrEmpty(txtSizePlanNow.Text) ? Convert.ToDouble(txtSizePlanNow.Text) : 0;

                        if (SoSum < TotSum)
                        {
                            double diffQty = TotSum - SoSum;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvrRow.RowIndex].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY + diffQty;
                        }
                        else if (SoSum > TotSum)
                        {
                            double diffQty = SoSum - TotSum;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvrRow.RowIndex].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY - diffQty;
                        }
                        //for correct pcs count end
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtTotalCurrPlan.Text) && Convert.ToDouble(txtTotalCurrPlan.Text) > 0)
                    {
                        string percVal = "0";
                        double TotalBal = objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).SOD_TOT_BAL_TO_PLAN;
                        objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).SizeDetail.ToList().ForEach(dtl =>
                        {
                            percVal = TotalBal > 0 ? ((dtl.SOD_BAL_TO_PLAN * 100) / TotalBal).ToString(): "1";
                            dtl.SIZE_PLAN_QTY = Math.Round(((Convert.ToDouble(txtTotalCurrPlan.Text) * Convert.ToDouble(percVal)) / 100));
                        });
                        lstSizeDetails = objPlanDtls.Details.SingleOrDefault(x => x.PND_PK == Convert.ToInt32(hdfPlanDtlPK.Value)).SizeDetail.ToList();


                        //for correct pcs count start

                        double TotSum = Convert.ToDouble(txtTotalCurrPlan.Text);
                        double sizeSum = lstSizeDetails.Sum(x => x.SIZE_PLAN_QTY);

                        if (sizeSum < TotSum)
                        {
                            double diffQty = TotSum - sizeSum;
                            SizeDetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail.LastOrDefault();
                            item.SIZE_PLAN_QTY = item.SIZE_PLAN_QTY + diffQty;
                        }
                        else if (sizeSum > TotSum)
                        {
                            double diffQty = sizeSum - TotSum;
                            SizeDetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail.LastOrDefault();
                            item.SIZE_PLAN_QTY = item.SIZE_PLAN_QTY - diffQty;
                        }

                        double LineSizeQty = 0;
                        int inde = 0;
                        foreach (SizeDetails items in lstSizeDetails)
                        {
                            LineSizeQty = items.SIZE_PLAN_QTY;
                            double Scsum = 0;
                            items.SODetails.ForEach(x =>
                            {
                                x.PNS_PLAN_QTY = Math.Round(Convert.ToDouble(GetFormattedNumber((items.SIZE_PLAN_QTY * x.SOD_PERCENTAGE) / 100)));
                                Scsum = Scsum + x.PNS_PLAN_QTY;
                            });

                            if (Scsum < LineSizeQty)
                            {
                                double diffQty = LineSizeQty - Scsum;
                                if (objPlanDtls.Details[gvr.RowIndex].SizeDetail.Count() > inde)
                                {
                                    SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[inde].SODetails.LastOrDefault();
                                    item.PNS_PLAN_QTY = item.PNS_PLAN_QTY + diffQty;
                                }
                            }
                            else if (Scsum > LineSizeQty)
                            {
                                double diffQty = Scsum - LineSizeQty;
                                if (objPlanDtls.Details[gvr.RowIndex].SizeDetail.Count() > inde)
                                {
                                    SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[inde].SODetails.LastOrDefault();
                                    item.PNS_PLAN_QTY = item.PNS_PLAN_QTY - diffQty;
                                }
                            }
                            inde++;
                        }

                        //for correct pcs count end
                    }
                }
            }
        }

        private void UpdateScenarioSaveList()
        {
            objPlanDtls.PNH_PK = 0;
            objPlanDtls.PNH_NAME = txtPlanName.Text;
            objPlanDtls.PNH_CODE = txtPlanCode.Text;
            objPlanDtls.PNH_FROM_DT = Convert.ToDateTime(txtFromDate.Text).ToShortDateString();
            objPlanDtls.PNH_TO_DT = Convert.ToDateTime(txtToDate.Text).ToShortDateString();
            objPlanDtls.PNH_TAB = Convert.ToInt32(hdnTabListNew.Value);
            objPlanDtls.BIH_DEPT = currentUser.CurrentDeptPK;
            objPlanDtls.BIZUNIT_PK = currentUser.CurrentSBUPK;
            objPlanDtls.USER_PK = currentUser.PKUser;
            objPlanDtls.PNH_VERSION = !string.IsNullOrEmpty(lblVersion.Text) ? lblVersion.Text : "0";
            objPlanDtls.LAST_MOD_DT = lastModDate;

            UpdateScenarioList();
        }

        private void UpdateScenarioList()
        {
            foreach (GridViewRow gvr in grdGrouplistScenario.Rows)
            {
                GridView gvrScen = (GridView)gvr.FindControl("grdSizeScenario") as GridView;
                GridView gv = (GridView)gvr.FindControl("grdSizeScenario") as GridView;
                HiddenField hdfPlanGroupPK = (HiddenField)gvr.FindControl("hdfPlanGroupPK") as HiddenField;
                HiddenField hdfSize = (HiddenField)gvr.FindControl("hdfSizeVal") as HiddenField;

                Label lblBalancetoPlan = (Label)gvr.FindControl("lblBalancetoPlan") as Label;

                System.Web.UI.UserControl ucTextFormatDtl = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan") as System.Web.UI.UserControl;
                objPlanDtls.Details[gvr.RowIndex].PND_PLAN_QTY = (ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text == string.Empty ? 0 : Convert.ToDouble((ucTextFormatDtl.FindControl("txtFormattedAmount") as TextBox).Text.Replace(",", ""));
                objPlanDtls.Details[gvr.RowIndex].PND_IS_CHECKED = 1;
                if (gv.Rows.Count > 0)
                {
                    double plnqty = 0;
                    foreach (GridViewRow gvdtl in gv.Rows)
                    {
                        TextBox txtDtlPlanNow = (TextBox)gvdtl.FindControl("txtDtlPlanNow") as TextBox;
                        objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY = txtDtlPlanNow.Text == string.Empty ? 0 : Convert.ToDouble(txtDtlPlanNow.Text.Replace(",", ""));
                        foreach (SODetails items in objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SODetails)
                        {
                            double Perc = 0;
                            double sizqty = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SOD_BAL_TO_PLAN;
                            double sodbaltoplan = items.SOD_BAL_TO_PLAN;
                            if (sodbaltoplan > 0)
                            {
                                Perc = ((sodbaltoplan * 100) / sizqty);
                                items.PNS_PLAN_QTY = Math.Round(((objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY / 100) * Perc));
                            }
                            plnqty = Convert.ToDouble(objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SODetails.Sum(x => x.PNS_PLAN_QTY));
                        }

                        // balancing so qty wr.to size qty
                        if (plnqty < objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY)
                        {
                            double diffQty = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY - plnqty;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY + diffQty;
                        }
                        else if (plnqty > objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY)
                        {
                            double diffQty = plnqty - objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SIZE_PLAN_QTY;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[gvdtl.RowIndex].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY - diffQty;
                        }
                    }
                }
                else
                {
                    double plnqty = 0;
                    double sizeSum = 0;



                    for (int i = 0; i <= objPlanDtls.Details[gvr.RowIndex].SizeDetail.Count - 1; i++)
                    {
                        double PlanNowHdr = (lblBalancetoPlan.Text == string.Empty ? 0 : Convert.ToDouble(lblBalancetoPlan.Text.Replace(",", "")));
                        double percVal = (objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SOD_BAL_TO_PLAN * 100) / PlanNowHdr;
                        objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY = Math.Round((objPlanDtls.Details[gvr.RowIndex].PND_PLAN_QTY / 100 * Convert.ToDouble(percVal)));
                        double Perc = 0;
                        double sizqty = objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SOD_BAL_TO_PLAN;

                        // balancing size qty wr.to main qty
                        if (i == objPlanDtls.Details[gvr.RowIndex].SizeDetail.Count - 1)
                        {
                            sizeSum = objPlanDtls.Details[gvr.RowIndex].SizeDetail.Sum(x => x.SIZE_PLAN_QTY);
                            double TotSum = objPlanDtls.Details[gvr.RowIndex].PND_PLAN_QTY;
                            if (sizeSum < TotSum)
                            {
                                double diffQty = TotSum - sizeSum;
                                SizeDetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail.LastOrDefault();
                                item.SIZE_PLAN_QTY = item.SIZE_PLAN_QTY + diffQty;
                            }
                            else if (sizeSum > TotSum)
                            {
                                double diffQty = sizeSum - TotSum;
                                SizeDetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail.LastOrDefault();
                                item.SIZE_PLAN_QTY = item.SIZE_PLAN_QTY - diffQty;
                            }
                        }

                        // balancing so qty wr.to size qty
                        foreach (SODetails soditems in objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SODetails)
                        {
                            double sodbaltoplan = soditems.SOD_BAL_TO_PLAN;
                            Perc = ((sodbaltoplan * 100) / sizqty);
                            soditems.PNS_PLAN_QTY = Math.Round(((objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY / 100) * Perc));

                        }

                        plnqty = Convert.ToDouble(objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SODetails.Sum(x => x.PNS_PLAN_QTY));
                        if (plnqty < objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY)
                        {
                            double diffQty = objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY - plnqty;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY + diffQty;
                        }
                        else if (plnqty > objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY)
                        {
                            double diffQty = plnqty - objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SIZE_PLAN_QTY;
                            SODetails item = objPlanDtls.Details[gvr.RowIndex].SizeDetail[i].SODetails.LastOrDefault();
                            item.PNS_PLAN_QTY = item.PNS_PLAN_QTY - diffQty;
                        }
                    }



                }
            }
        }

        private void UpdateLineSelection()
        {
            //objPlanDtls.Details[ScenarioHdrIndex].FilteredLines = new List<FilteredLines>();
            for (int i = 0; i <= objLineBO.Detail.Count - 1; i++)
            {
                List<FilteredLines> ListFilteredLines = new List<FilteredLines>();
                foreach (PlanGroupLineDetail lne in objLineBO.Detail[i].LineDetail)
                {
                    if (lne.Ischecked == 1)
                    {
                        FilteredLines ObjFilteredLines = new FilteredLines();
                        ObjFilteredLines.LNE_PK = lne.PNS_LNE_PK;
                        ObjFilteredLines.PND_SL_NO = objLineBO.Detail[i].PND_SL_NO;
                        ListFilteredLines.Add(ObjFilteredLines);
                    }
                }
                objPlanDtls.Details[i].FilteredLines = ListFilteredLines;
            }
        }
        #endregion

        #region Delete Order Plan
        private void DeleteOrderPlan(int PlanPK, string lastmodDate, int cancelflag)
        {
            int delVal = OrderPlanningBL.DeleteOrderPlan(PlanPK, lastmodDate, currentUser.PKUser, cancelflag);
            switch (delVal)
            {
                case (int)DbDeleteStatus.DELETED://successfully deleted.
                    if (cancelflag == 0)
                        litErrorMsg.Text = Resources.Messages.DeletedSuccessfully;
                    else
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Cancel_Success;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                    ResetPage();
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.PLANLISTING);
                    SetFieldValues(ControlsEnum.PLANLISTING);
                    break;
                case (int)DbDeleteStatus.CONCURRENCY:
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    break;
                case (int)DbDeleteStatus.DELETECONCURRENCY:
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    break;
                case (int)DbDeleteStatus.SQLERROR:
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    break;
                case (int)DbDeleteStatus.REFERRED:
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    break;
                default:
                    litErrorMsg.Text = this.GetLocalResourceObject("Err_Delete").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("OrderPlan").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    break;

            }
        }


        #endregion

        #region Number Formats
        /// <summary>
        /// Manage Decimal Points 
        /// </summary>
        /// <param name="number">Number For Formating</param>
        /// <returns></returns>
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (num != 0 && num.ToString(DecimalFormat) != string.Empty)
                return num.ToString(DecimalFormat);
            else
                return "0";

        }

        /// <summary>
        /// Manage Decimal Points with comma seperation
        /// </summary>
        /// <param name="number">Number For Formating</param>
        /// <returns></returns>
        public string GetFormattedWeightwithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (num != 0)
                return num.ToString(WeightFormat);
            else
                return num.ToString();
        }
        #endregion

        #region AFTER_APPLY
        //public void ucrPO_AfterApply(object sender, EventArgs e)
        public void SelectOrdersForPlan()
        {
            OrderPlanBO objPlan = new OrderPlanBO();
            SelectedOrderList = ucrPO.GetSelectedOrders();
            //To get distinct groups
            List<PendingOrderList> SelectedGroupList = (from order in SelectedOrderList
                                                        group order by new
                                                        {
                                                            order.ITM_PLAN_GROUP
                                                        } into g
                                                        select new PendingOrderList
                                                        {
                                                            ITM_PLAN_GROUP = g.Key.ITM_PLAN_GROUP
                                                        }).ToList();

            //Rearranging list for save format
            List<OrderDetails> lstOrderdtls = new List<OrderDetails>();
            int count = 1;
            foreach (PendingOrderList groups in SelectedGroupList)
            {
                OrderDetails objOrderDtls = new OrderDetails();
                objOrderDtls.PND_PK = 0;
                objOrderDtls.PND_PLAN_GROUP = groups.ITM_PLAN_GROUP;
                if (objOrderDtls.PND_IS_CHECKED == 1)
                    objOrderDtls.PND_PLAN_QTY = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Sum(x => x.SOD_BAL_TO_PLAN);
                else
                    objOrderDtls.PND_PLAN_QTY = 0;// groups.SOD_BAL_TO_PLAN;

                objOrderDtls.SOD_TOT_BAL_TO_PLAN = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Sum(x => x.SOD_BAL_TO_PLAN);
                objOrderDtls.PND_REQUIRED_DT = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Min(x => Convert.ToDateTime(x.SOD_REQUIRED_BY)).ToString();
                objOrderDtls.PND_SL_NO = count;
                objOrderDtls.ISD_AGRADE_PER = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Min(x => x.ISD_AGRADE_PER);
                objOrderDtls.SizeDetail = (from sc in SelectedOrderList
                                           where sc.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP
                                           orderby sc.ISD_SIZE_SEQUENCE
                                           group sc by new
                                           {
                                               sc.ISD_SIZE
                                           } into g
                                           select new SizeDetails
                                           {
                                               PNS_SIZE = g.Key.ISD_SIZE,
                                               PNS_SIZE_TEXT = g.FirstOrDefault().ISD_SIZE_TEXT,
                                               SIZE_PLAN_GROUP = g.FirstOrDefault().ITM_PLAN_GROUP,
                                               PND_SL_NO = count,
                                               SIZE_REQUIRED_DT = g.Min(x => x.SOD_REQUIRED_BY),
                                               SIZE_PLAN_QTY = g.Sum(x => x.SOD_BAL_TO_PLAN),
                                               SOD_BAL_TO_PLAN = g.Sum(x => x.SOD_BAL_TO_PLAN),
                                               SODetails = (from sz in SelectedOrderList
                                                            where sz.ITM_PLAN_GROUP == g.FirstOrDefault().ITM_PLAN_GROUP &&
                                                                     sz.ISD_SIZE == g.Key.ISD_SIZE
                                                            select new SODetails
                                                            {
                                                                PNS_PK = 0,
                                                                PND_SL_NO = count,
                                                                PNS_SO_DTL = sz.SOD_PK,
                                                                PNS_PLAN_GROUP = sz.ITM_PLAN_GROUP,
                                                                PNS_PLAN_QTY = (objOrderDtls.PND_IS_CHECKED == 1 ? sz.SOD_BAL_TO_PLAN : 0),//sz.SOD_BAL_TO_PLAN,
                                                                PNS_REQUIRED_DT = sz.SOD_REQUIRED_BY,
                                                                PNS_ITEM = sz.SOD_ITEM,
                                                                PNS_SIZE = sz.ISD_SIZE
                                                            }).ToList()
                                           }).ToList();
                lstOrderdtls.Add(objOrderDtls);
                count++;
            }

            if (CurPK == 0)
            {
                objPlan = (OrderPlanBO)SetUIValuestoObject(ControlsEnum.PLANHEADER);
                objPlan.Details = lstOrderdtls;
                SavePlan(objPlan, 1);
            }
            else
            {
                AddNewItemsinEditMode(lstOrderdtls);
                SavePlan(objPlanDtls, 1);
            }
        }
        #endregion

        public void SelectOrdersForScenario()
        {
            OrderPlanBO objPlan = new OrderPlanBO();
            SelectedOrderList = ucrPO.GetSelectedOrders();
            if (SelectedOrderList != null)
                SelectedOrderList = SelectedOrderList.OrderBy(x => Convert.ToDateTime(x.SOD_REQUIRED_BY)).ThenBy(s => s.ITM_PLAN_GROUP_TEXT).ThenBy(s => s.ISD_SIZE_SEQUENCE).ThenBy(s => s.ISD_SIZE_TEXT).ToList();
            //SelectedOrderList = SelectedOrderList.OrderBy(x => x.ITM_PLAN_GROUP_TEXT).ThenBy(s => s.ISD_SIZE_SEQUENCE).ToList();

            //To get distinct groups
            List<PendingOrderList> SelectedGroupList = (from order in SelectedOrderList
                                                        group order by new
                                                        {
                                                            order.ITM_PLAN_GROUP
                                                        } into g
                                                        select new PendingOrderList
                                                        {
                                                            ITM_PLAN_GROUP = g.Key.ITM_PLAN_GROUP
                                                        }).ToList();

            //Rearranging list for save format
            List<OrderDetails> lstOrderdtls = new List<OrderDetails>();
            int count = 1;
            foreach (PendingOrderList groups in SelectedGroupList)
            {

                PendingOrderList tempobjOrderDtls = null;
                tempobjOrderDtls = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).First();

                OrderDetails objOrderDtls = new OrderDetails();
                objOrderDtls.PND_PK = 0;
                objOrderDtls.PND_PLAN_GROUP = groups.ITM_PLAN_GROUP;
                objOrderDtls.PND_PLAN_GROUP_TEXT = tempobjOrderDtls.ITM_PLAN_GROUP_TEXT;
                objOrderDtls.SOD_TOT_BAL_TO_PLAN = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Sum(x => x.SOD_BAL_TO_PLAN);
                objOrderDtls.PND_PLAN_QTY = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Sum(x => x.SOD_BAL_TO_PLAN);
                objOrderDtls.PND_REQUIRED_DT = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Min(x => Convert.ToDateTime(x.SOD_REQUIRED_BY)).ToString();
                objOrderDtls.PND_SL_NO = count;
                objOrderDtls.ISD_AGRADE_PER = SelectedOrderList.Where(dtl => dtl.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP).ToList().Min(x => x.ISD_AGRADE_PER);
                objOrderDtls.PND_DDL_MET = string.Empty;
                objOrderDtls.SizeDetail = (from sc in SelectedOrderList
                                           where sc.ITM_PLAN_GROUP == groups.ITM_PLAN_GROUP
                                           orderby sc.ISD_SIZE_SEQUENCE
                                           group sc by new
                                           {
                                               sc.ISD_SIZE
                                           } into g
                                           select new SizeDetails
                                           {
                                               PNS_SIZE = g.Key.ISD_SIZE,
                                               PNS_SIZE_TEXT = g.FirstOrDefault().ISD_SIZE_TEXT,
                                               SIZE_PLAN_GROUP = g.FirstOrDefault().ITM_PLAN_GROUP,
                                               PND_SL_NO = count,
                                               SIZE_REQUIRED_DT = g.Min(x => x.SOD_REQUIRED_BY),
                                               SIZE_PLAN_QTY = 0,
                                               SOD_BAL_TO_PLAN = g.Sum(x => x.SOD_BAL_TO_PLAN),
                                               SOD_QTY_PLANNED = g.Sum(x => x.SOD_QTY_PLANNED),
                                               SODetails = (from sz in SelectedOrderList
                                                            where sz.ITM_PLAN_GROUP == g.FirstOrDefault().ITM_PLAN_GROUP &&
                                                                     sz.ISD_SIZE == g.Key.ISD_SIZE
                                                            select new SODetails
                                                            {
                                                                PNS_PK = 0,
                                                                PND_SL_NO = count,
                                                                PNS_SO_DTL = sz.SOD_PK,
                                                                PNS_PLAN_GROUP = sz.ITM_PLAN_GROUP,
                                                                PNS_PLAN_QTY = sz.SOD_BAL_TO_PLAN,
                                                                PNS_REQUIRED_DT = sz.SOD_REQUIRED_BY,
                                                                PNS_ITEM = sz.SOD_ITEM,
                                                                PNS_SIZE = sz.ISD_SIZE,
                                                                SOD_BAL_TO_PLAN = sz.SOD_BAL_TO_PLAN
                                                            }).ToList()
                                           }).ToList();
                lstOrderdtls.Add(objOrderDtls);
                count++;
            }
            List<OrderDetails> TemplstOrderdtls = lstOrderdtls.OrderBy(d => Convert.ToDateTime(d.PND_REQUIRED_DT)).ThenByDescending(x => x.PND_PLAN_QTY).ToList();
            objPlan = (OrderPlanBO)SetUIValuestoObject(ControlsEnum.PLANHEADER);
            objPlan.Details = TemplstOrderdtls;
            objPlanDtls = objPlan;
            grdGrouplistScenario.DataSource = TemplstOrderdtls;
            grdGrouplistScenario.DataBind();
        }

        #region SAVE AFTER SELECTION
        private void SavePlan(OrderPlanBO objOrderPlan, int NewModeSave)
        {
            int result = 0;
            if (objOrderPlan != null)
            {
                List<OrderDetails> TempObj = objOrderPlan.Details.Where(x => x.SizeDetail.Count > 0).ToList();
                objOrderPlan.Details = TempObj;

               //XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objOrderPlan);
                string xmlDoc = CommonFunctions.XmlSerialize<OrderPlanBO>(objOrderPlan);
                result = OrderPlanningBL.SaveOrderPlan(xmlDoc);
            }
            if (result > 0)
            {
                if (NewModeSave == 1)
                {
                    CurPK = result;
                    hdfPlanPK.Value = CurPK.ToString();
                    hdnTabListNew.Value = "2";
                    ucrPO.SelectedList = null;
                    ucrPO.POPageIndex = "1";
                    ucrPO.OrderPlanPK = CurPK;
                    ucrPO.objALLPendingOrderLst = null;
                    ((HiddenField)ucrPO.FindControl("hdfIsAllPagsSelected")).Value = "0";
                    ucrPO.PopulatePendingListGrid();
                    GetFieldValues(ControlsEnum.EDIT);
                    SetFieldValues(ControlsEnum.EDIT);
                    GetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                    SetFieldValues(ControlsEnum.PRODUCTPLANGROUPLIST);
                    SummaryType = 1;
                    GetFieldValues(ControlsEnum.SUMMARYDETAILS);
                    SetFieldValues(ControlsEnum.SUMMARYGROUPLIST);
                    EntryStatus = EntryStatus.EDITMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.PLANLISTING);
                    SetFieldValues(ControlsEnum.PLANLISTING);
                    if (isFinalize == 1)
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Finalize_Success").ToString(), txtFromDate.Text, txtToDate.Text);
                    else if (isRevise == 1)
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Revise_Success").ToString(), txtFromDate.Text, txtToDate.Text);
                    else
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Save_Success").ToString(), txtFromDate.Text, txtToDate.Text);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "', '" + Resources.Captions.Information + "');", true);
                    ResetPage();
                }
                hdnTabListNew.Value = "2";
            }
            else
            {
                DbSaveStatus saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {
                    case DbSaveStatus.SQLERROR:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Sql_Error, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.CODEEXIST:
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Plan_Code_Exists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    case DbSaveStatus.NAMEEXIST://32
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Plan_Name_Exists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    case DbSaveStatus.CONCURRENCY:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Save_Error_Concurrent, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.ALREADYDELETED:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Already_Deleted, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.PRODUCTOVERLAP://33
                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrPlanExistsonPeriod").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    default:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_SavError, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                }
                hdnTabListNew.Value = "1";
            }
        }

        private void SavePlanScenario(OrderPlanBO objOrderPlan, int NewModeSave)
        {
            int result = 0;
            if (objOrderPlan != null)
            {
                List<OrderDetails> TempObj = objOrderPlan.Details.Where(x => x.SizeDetail.Count > 0).ToList();
                objOrderPlan.Details = TempObj;

               // XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objOrderPlan);
                string xmlString = CommonFunctions.XmlSerialize<OrderPlanBO>(objOrderPlan);

                result = OrderPlanningBL.SaveOrderPlan(xmlString);
            }
            if (result > 0)
            {
                CurPK = result;
                ActionHandler(btnEdit, EventArgs.Empty);
                //EntryStatus = EntryStatus.LISTMODE;
                //GetFieldValues(ControlsEnum.PLANLISTING);
                //SetFieldValues(ControlsEnum.PLANLISTING); 
                litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Save_Success").ToString(), txtFromDate.Text, txtToDate.Text);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "', '" + Resources.Captions.Information + "');", true);
                //ResetPage(); 
            }
            else
            {
                DbSaveStatus saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {
                    case DbSaveStatus.SQLERROR:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Sql_Error, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.CODEEXIST:
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Plan_Code_Exists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    case DbSaveStatus.NAMEEXIST:
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Plan_Name_Exists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    case DbSaveStatus.CONCURRENCY:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Save_Error_Concurrent, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.ALREADYDELETED:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Already_Deleted, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.PRODUCTOVERLAP:
                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrPlanExistsonPeriod").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        objOrderPlan = null;
                        break;
                    default:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_SavError, GetLocalResourceObject("OrderPlan").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                }
            }
        }

        #endregion

        #region POPUP
        private void ShowLinePopup(int mode)
        {
            string caption = string.Empty;
            if (mode == 1)//for view mode
            {
                tblLineEntry.Visible = false;
                btnApplyLines.Visible = false;
                if (grdLines.Rows.Count > 0)
                {
                    foreach (GridViewRow grvR in grdLines.Rows)
                    {
                        ((ImageButton)grvR.FindControl("imbLineEdit")).Visible = false;
                        ((ImageButton)grvR.FindControl("imbLineDelete")).Visible = false;
                    }
                }
                caption = GetLocalResourceObject("Viewlines").ToString();
            }
            else
            {
                tblLineEntry.Visible = true;
                btnApplyLines.Visible = true;
                caption = GetLocalResourceObject("Addline").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateLineQtyTotal", "$(document).ready(function(){CalculateLineQtyTotal();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divLines]','" + caption + "','900','550');", true);
        }

        private void ShowPlanVersionPopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divVersions]','" + GetLocalResourceObject("ShowVersions").ToString() + "','500','400');", true);
        }

        private void ShowSCPopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSCDetails]','" + GetLocalResourceObject("ViewSCDetails").ToString() + "','900','400');", true);
        }
        private void ShowLineScenarioPopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divLinesScenario]','" + GetLocalResourceObject("ScenarioLineDtls").ToString() + "','900','500');", true);
        }
        private void ShowMachinePopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divMachines]','" + GetLocalResourceObject("Machines").ToString() + "','400','350');", true);
        }
        #endregion

        #region Reset
        private void ResetPage()
        {
            CurPK = 0;
            lastModDate = string.Empty;
            hdfPlanPK.Value = "0";
            hdfAGradePerc.Value = "0";
            ucrPO.OrderPlanPK = 0;
            ucrPO.objALLPendingOrderLst = null;
            ((HiddenField)ucrPO.FindControl("hdfIsAllPagsSelected")).Value = "0";
            objPlanDtls = null;
            lstOrders = null;
            lstSODetails = null;
            lstLineDetails = null;
            SelectedOrderList = null;
            ucrPO.SelectedList = null;
            txtPlanName.Text = string.Empty;
            txtPlanCode.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            lblVersion.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value= string.Empty;
            txtToDate.Text = string.Empty;
            hdfToDate.Value = string.Empty;

            PlanGroupPK = 0;
            isRevise = 0;
            isFinalize = 0;
            hdfshowRevert.Value = "0";
            PageIndex = "1";
            hdfShowHideFilter.Value = "0";
            SummaryType = 0;
            SummaryTypePK = 0;
            TotalPlannedPerc = 0;
            TotalPlannedPcs = 0;
            TotalProducedPerc = 0;
            TotalProducedPcs = 0;
            TotalBalancePerc = 0;
            TotalBalancePcs = 0;
            imbShowVersions.Visible = false;
            SortExpression = null;
            SortOrder = null;
            chkOptimizePlan.Checked = false;
            chkOptimizeScenario.Checked = false;
            ClearSearch();
        }
        private void ResetLineDtl()
        {
            ddlLine.SelectedIndex = 0;
            lblPlant.Text = string.Empty;
            hdfdayCapacity.Value = "0";
            lblCapacity.Text = string.Empty;
            ucLineQty.Text = string.Empty;
            hdfLineDtlPK.Value = "0";
            hdfLineSLNO.Value = "0";
            lblLinePlanned.Text = string.Empty;
            lblBalLineQty.Text = string.Empty;
            txtLineFromDt.Text = string.Empty;
            txtLineFromTime.Text = string.Empty;
            txtLineToDt.Text = string.Empty;
            txtLineToTime.Text = string.Empty;
            ucLineSpeed.Text = "0";
            hdfHolderPos.Value = "0";
        }

        private void ResetLinePopUp()
        {
            hdfIscontYes.Value = "0";
            hdfLineDtlPK.Value = "0";
            hdfLineSLNO.Value = "0";
            hdfLinePlanGroup.Value = "0";
            hdfLinePlanGroupSize.Value = "0";
            hdfLinePlanGroupDtlPK.Value = "0";
            hdfLinePlanGroupSlNO.Value = "0";
            hdfLineRequiredQty.Value = "0";
            lstLineDetails = null;
            lblCurrPlanQty.Text = string.Empty;
            lblLineProductGroup.Text = string.Empty;
            lblGroupSize.Text = string.Empty;
        }

        private void ClearSearch()
        {
            txtFilterPlanName.Text = "Select/Type";
            txtFilterPlanCode.Text = "Select/Type";
            hdfFilterPlanPK.Value = "0";
            txtFilterFrom.Text = string.Empty;
            hdfFilterFrom.Value = string.Empty;
            txtFilterTo.Text = string.Empty;
            hdfFilterTo.Value = string.Empty;
            ddlFilterStatus.SelectedIndex = ddlFilterStatus.Items.IndexOf(ddlFilterStatus.Items.FindByValue("1"));
            PageIndex = "1";
        }
        #endregion

        #region Checking for Line entry
        //To get line quantity of selected row
        private double GetLineTotalQty(int SlNO = -1)
        {
            double LineQty = 0;
            double LineCapacity = 0;
            if (SlNO >= 0)
            {
                foreach (OrderDetails items in objPlanDtls.Details)
                {
                    LineQty = LineQty + items.LineDetails.Where(x => x.PNL_LINE == Convert.ToInt32(ddlLine.SelectedValue) && x.PND_SL_NO != SlNO).Sum(dtl => dtl.PNL_PLAN_QTY);
                }
            }
            return LineQty;
        }

        //to check if row exists without line mapping
        private int CheckEmptyLineExists()
        {
            int emptyLine = 0;
            for (int i = 0; i < objPlanDtls.Details.Count; i++)
            {
                if (objPlanDtls.Details[i].PND_IS_CHECKED == 1 && objPlanDtls.Details[i].LineDetails.Count == 0)
                {
                    emptyLine = 1;
                    break;
                }
            }
            return emptyLine;
        }

        //To check if line qty and plan qty is matching
        private int CheckLinePlannedQty()
        {
            int retVal = 0;
            for (int i = 0; i < grdGrouplist.Rows.Count; i++)
            {
                CheckBox ChkPlanOrder = (CheckBox)grdGrouplist.Rows[i].FindControl("ChkPlanOrder");
                System.Web.UI.UserControl ucCurTotal = (System.Web.UI.UserControl)grdGrouplist.Rows[i].FindControl("txtTotalCurrPlan");
                TextBox txtTotalCurrPlan = (TextBox)ucCurTotal.FindControl("txtFormattedAmount");
                LinkButton lnbLine = (LinkButton)grdGrouplist.Rows[i].FindControl("lnbLine");
                if (ChkPlanOrder.Checked)
                {
                    if (Convert.ToDouble(txtTotalCurrPlan.Text) != Convert.ToDouble(lnbLine.Text))
                    {
                        lnbLine.AddCssClass("txt_Orange bold");
                    }
                    else
                    {
                        lnbLine.RemoveCssClass("txt_Orange bold");
                    }

                    if (Convert.ToDouble(txtTotalCurrPlan.Text) > Convert.ToDouble(lnbLine.Text))
                    {
                        retVal = 1;
                    }
                }
            }
            return retVal;
        }
        #endregion

        #region Check PlanNow
        private int CheckPlanNow()
        {
            int retVal = 0;
            for (int i = 0; i < grdGrouplist.Rows.Count; i++)
            {
                CheckBox ChkPlanOrder = (CheckBox)grdGrouplist.Rows[i].FindControl("ChkPlanOrder");
                System.Web.UI.UserControl ucCurTotal = (System.Web.UI.UserControl)grdGrouplist.Rows[i].FindControl("txtTotalCurrPlan");
                double PlanQty = !string.IsNullOrEmpty(((TextBox)ucCurTotal.FindControl("txtFormattedAmount")).Text) ? Convert.ToDouble(((TextBox)ucCurTotal.FindControl("txtFormattedAmount")).Text) : 0;
                if (ChkPlanOrder.Checked && (PlanQty == 0 || PlanQty < 0))
                    retVal = 1;
            }
            return retVal;
        }
        #endregion

        #region MERGE LIST IN EDIT MODE
        private void AddNewItemsinEditMode(List<OrderDetails> NewItemsList)
        {
            objPlanDtls.PNH_PK = !string.IsNullOrEmpty(hdfPlanPK.Value) ? Convert.ToInt32(hdfPlanPK.Value) : 0;
            objPlanDtls.PNH_NAME = txtPlanName.Text;
            objPlanDtls.PNH_CODE = txtPlanCode.Text;
            objPlanDtls.PNH_FROM_DT = Convert.ToDateTime(txtFromDate.Text).ToShortDateString();
            objPlanDtls.PNH_TO_DT = Convert.ToDateTime(txtToDate.Text).ToShortDateString();
            objPlanDtls.PNH_TAB = Convert.ToInt32(hdnTabListNew.Value);
            objPlanDtls.BIH_DEPT = currentUser.CurrentDeptPK;
            objPlanDtls.BIZUNIT_PK = currentUser.CurrentSBUPK;
            objPlanDtls.USER_PK = currentUser.PKUser;
            objPlanDtls.PNH_VERSION = !string.IsNullOrEmpty(lblVersion.Text) ? lblVersion.Text : "0";
            objPlanDtls.LAST_MOD_DT = lastModDate;
            if (isFinalize == 1)
                objPlanDtls.IS_REVISE = 1;
            else
                objPlanDtls.IS_REVISE = isRevise;
            foreach (OrderDetails Items in NewItemsList)
            {
                OrderDetails objT = objPlanDtls.Details.SingleOrDefault(dtl => dtl.PND_PLAN_GROUP == Items.PND_PLAN_GROUP);
                if (objT != null)
                {
                    Items.SizeDetail.ForEach(dtl =>
                    {
                        dtl.PND_SL_NO = objT.PND_SL_NO;
                        dtl.SODetails.ForEach(x => x.PND_SL_NO = objT.PND_SL_NO);
                    });
                    objT.SizeDetail.AddRange(Items.SizeDetail);
                    objT.PND_REQUIRED_DT = objT.SizeDetail.Min(x => x.SIZE_REQUIRED_DT);
                    objT.PND_PLAN_QTY = objT.PND_PLAN_QTY + Items.SizeDetail.Sum(x => x.SIZE_PLAN_QTY);
                }
                else
                {
                    OrderDetails objOrderDtls = new OrderDetails();
                    objOrderDtls.PND_PK = 0;
                    objOrderDtls.PND_PLAN_GROUP = Items.PND_PLAN_GROUP;
                    objOrderDtls.PND_PLAN_QTY = 0;
                    objOrderDtls.PND_REQUIRED_DT = Items.SizeDetail.Min(x => x.SIZE_REQUIRED_DT);
                    objPlanDtls.ISD_AGRADE_PER = Items.ISD_AGRADE_PER;
                    objOrderDtls.PND_SL_NO = objPlanDtls.Details.Count + 1;
                    Items.SizeDetail.ForEach(dtl =>
                    {
                        dtl.PND_SL_NO = objPlanDtls.Details.Count + 1;
                        dtl.SODetails.ForEach(x => x.PND_SL_NO = objPlanDtls.Details.Count + 1);
                    });
                    objOrderDtls.SizeDetail = Items.SizeDetail;
                    objPlanDtls.Details.Add(objOrderDtls);
                }
            }
        }
        #endregion

        #region Bind Order Grid After Delete
        private void BindOrderGridAfterDelete(GridViewRow gvr, List<SODetails> list)
        {
            if (gvr != null)
            {
                if (list != null && list.Count > 0)
                    grdOrderDtls.DataSource = list;
                else
                    grdOrderDtls.DataSource = null;
                grdOrderDtls.DataBind();
                foreach (GridViewRow gvrRw in grdOrderDtls.Rows)
                {
                    HiddenField hdfOrderDtl = ((HiddenField)gvrRw.FindControl("hdfOrderDtl")) as HiddenField;
                    double OrderBal = !string.IsNullOrEmpty((gvrRw.FindControl("lblOrderBalQty") as Label).Text) ? Convert.ToDouble((gvrRw.FindControl("lblOrderBalQty") as Label).Text.Replace(",", "")) : 0;
                    double TotalBal = 0;
                    TotalBal = TotalBal + OrderBal;
                    System.Web.UI.UserControl ucOrderQty = (System.Web.UI.UserControl)gvrRw.FindControl("txtDtlCurrPlan") as System.Web.UI.UserControl;
                    if (TotalBal > 0)
                    {
                        string percVal = ((OrderBal * 100) / TotalBal).ToString();
                        (gvrRw.FindControl("hdfSCProportionVal") as HiddenField).Value = percVal;
                        (ucOrderQty.FindControl("txtFormattedAmount") as TextBox).Text = list.SingleOrDefault(x => x.PNS_PK == Convert.ToInt32(hdfOrderDtl.Value)).PNS_PLAN_QTY.ToString();
                    }
                }
                lblSPlanQty.Text = GetFormattedNumber(list.Sum(x => x.PNS_PLAN_QTY)).ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateSCPopUpTotal", "CalculateSCPopUpTotal();", true);
            }
        }
        #endregion

        #region ReplaceComma
        /// <summary>
        /// Replace comma 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string ReplaceComma(string number)
        {
            return number.Replace(",", "");
        }
        #endregion

        #region CheckBox selection
        private void CheckSelectAll()
        {
            foreach (GridViewRow gvr in grdGrouplist.Rows)
            {
                CheckBox ChkAll = (CheckBox)grdGrouplist.HeaderRow.FindControl("ChkPlanAll");
                System.Web.UI.UserControl ucCurTotal = (System.Web.UI.UserControl)gvr.FindControl("txtTotalCurrPlan");
                Label lblBalancetoPlan = (Label)gvr.FindControl("lblBalancetoPlan");
                TextBox txtTotalCurrPlan = (TextBox)ucCurTotal.FindControl("txtFormattedAmount");
                ImageButton imbAddLine = (ImageButton)gvr.FindControl("imbAddLine");
                LinkButton lnbLine = (LinkButton)gvr.FindControl("lnbLine");
                if (ChkAll.Checked)
                {
                    if (lnbLine.Text == "0")
                    {
                        ((CheckBox)gvr.FindControl("ChkPlanOrder")).Checked = true;
                        txtTotalCurrPlan.Text = !string.IsNullOrEmpty(txtTotalCurrPlan.Text) ? GetFormattedNumber(lblBalancetoPlan.Text) : "0";
                        txtTotalCurrPlan.Enabled = true;
                        imbAddLine.Attributes.Add("display", "block");
                        gvr.Attributes.Add("class", "table-firstlevel Selection");
                        EnableDisablegrdPlanQty(gvr, 1, Convert.ToDouble(txtTotalCurrPlan.Text));
                    }
                }
                else
                {
                    if (lnbLine.Text == "0")
                    {
                        ((CheckBox)gvr.FindControl("ChkPlanOrder")).Checked = false;
                        txtTotalCurrPlan.Text = "0";
                        txtTotalCurrPlan.Enabled = false;
                        imbAddLine.Attributes.Add("display", "none");
                        gvr.Attributes.Add("class", "");
                        EnableDisablegrdPlanQty(gvr, 0, 0);
                    }
                }
            }
        }

        private void CheckSelectItem(object sender)
        {
            bool ischecked = true;
            GridViewRow gvrRow = ((CheckBox)sender).Parent.Parent as GridViewRow;
            System.Web.UI.UserControl ucCurTotal = (System.Web.UI.UserControl)gvrRow.FindControl("txtTotalCurrPlan");
            Label lblBalancetoPlan = (Label)gvrRow.FindControl("lblBalancetoPlan");
            TextBox txtTotalCurrPlan = (TextBox)ucCurTotal.FindControl("txtFormattedAmount");
            ImageButton imbAddLine = (ImageButton)gvrRow.FindControl("imbAddLine");
            LinkButton lnbLine = (LinkButton)gvrRow.FindControl("lnbLine");
            if (((CheckBox)gvrRow.FindControl("ChkPlanOrder")).Checked)
            {
                if (lnbLine.Text == "0")
                {
                    txtTotalCurrPlan.Text = !string.IsNullOrEmpty(txtTotalCurrPlan.Text) ? GetFormattedNumber(lblBalancetoPlan.Text) : "0";
                    txtTotalCurrPlan.Enabled = true;
                    imbAddLine.Attributes.Add("display", "block");
                    gvrRow.Attributes.Add("class", "table-firstlevel Selection");
                    EnableDisablegrdPlanQty(gvrRow, 1, Convert.ToDouble(txtTotalCurrPlan.Text));
                }
            }
            else
            {
                if (lnbLine.Text == "0")
                {
                    txtTotalCurrPlan.Text = "0";
                    txtTotalCurrPlan.Enabled = false;
                    imbAddLine.Attributes.Add("display", "none");
                    gvrRow.Attributes.Add("class", "");
                    EnableDisablegrdPlanQty(gvrRow, 0, 0);
                }
            }
            foreach (GridViewRow gvr in grdGrouplist.Rows)
            {
                if (((CheckBox)gvr.FindControl("ChkPlanOrder")).Checked == false)
                    ischecked = false;
            }
            ((CheckBox)grdGrouplist.HeaderRow.FindControl("ChkPlanAll")).Checked = ischecked;
        }

        private void CheckAllCombinedLines()
        {
            CheckBox chkbx = (CheckBox)grdGrouplistScenario.HeaderRow.FindControl("chkAllCombinedLines");
            for (int i = 0; i < objPlanDtls.Details.Count; i++)
            {
                if (chkbx.Checked)
                    objPlanDtls.Details[i].LINE_COMBINED = 1;
                else
                    objPlanDtls.Details[i].LINE_COMBINED = 0;
            }
        }

        private void EnableDisablegrdPlanQty(GridViewRow gvr, int Enable, double Balance)
        {
            foreach (GridViewRow gvrRw in (gvr.FindControl("grdSize") as GridView).Rows)
            {
                System.Web.UI.UserControl ucCurTotal = (System.Web.UI.UserControl)gvrRw.FindControl("txtSizePlanNow");
                TextBox txtDtlCurrPlan = (TextBox)ucCurTotal.FindControl("txtFormattedAmount");
                HiddenField hdfProportionVal = (HiddenField)gvrRw.FindControl("hdfProportionVal");
                if (Enable == 1)
                {
                    txtDtlCurrPlan.Text = GetFormattedNumber((Balance * Convert.ToDouble(hdfProportionVal.Value)) / 100).ToString() != string.Empty ? GetFormattedNumber(((Balance * Convert.ToDouble(hdfProportionVal.Value)) / 100).ToString()) : "0";
                    ((ImageButton)gvrRw.FindControl("imbViewSC")).Visible = true;
                }
                else
                {
                    txtDtlCurrPlan.Text = "0";
                    ((ImageButton)gvrRw.FindControl("imbViewSC")).Visible = false;
                }
            }
        }

        public static DateTime SetTime(DateTime currentDate, DateTime currentTime)
        {

            return new DateTime(
                currentDate.Year,
                currentDate.Month,
                currentDate.Day,
                currentTime.Hour,
                currentTime.Minute,
                currentTime.Second,
                currentTime.Millisecond,
                currentTime.Kind);
        }

        private int CompareLineDate()
        {
            DateTime StartDate, EndDate, TtempDate, tempTime;
            tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineFromTime.Text.Trim()));
            if (!DateTime.TryParse(txtLineFromDt.Text, out TtempDate))
                TtempDate = DateTime.Now;
            StartDate = SetTime(TtempDate, tempTime);
            tempTime = Convert.ToDateTime(HttpUtility.HtmlEncode(txtLineToTime.Text.Trim()));
            if (!DateTime.TryParse(txtLineToDt.Text, out TtempDate))
                TtempDate = DateTime.Now;
            EndDate = SetTime(TtempDate, tempTime);
            if (StartDate > EndDate)
                return 0;
            else
                return 1;
        }
        #endregion
        #endregion

        #region Pager Methods+Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///    // Add Common to All pages
        protected void Page_Init(object sender, System.EventArgs e)
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        // Add Common to All pages
        private void InitializeComponent()
        {

        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.PLANLISTING);
                SetFieldValues(ControlsEnum.PLANLISTING);
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }
        // Add Common to All pages
        protected void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }
        #endregion
    }
    public enum ControlsEnum
    {
        PENDINGORDERSLIST,
        PRODUCTPROPERTY,
        PROPERTY,
        ORDERNUMBER,
        PLANHEADER,
        PRODUCTPLANGROUP,
        EDIT,
        PRODUCTPLANGROUPLIST,
        SBU,
        PLANLISTING,
        CLEARLIST,
        PLANDETAILS,
        ORDERDETAILS,
        LINELIST,
        LINEDETAILS,
        LINEGRID,
        SIZE,
        CUSTOMER,
        ALLOCATIONDETAILS,
        SUMMARYDETAILS,
        SUMMARYPLANTLIST,
        SUMMARYLINELIST,
        SUMMARYGROUPLIST,
        PLANVERSIONS,
        RELEASESC,
        BINDETAILS,
        LINEWISESUMMARY,
        SCDETAILS,
        SCENARIODETAILS,
        MACHINEFILL,
        LINEFORMERDETAILS,
        GROUPLINES,
        PLANGROUPLINES,
        PLANCALCULATION,
        SELECTALLPAGEITEMS
    }
}