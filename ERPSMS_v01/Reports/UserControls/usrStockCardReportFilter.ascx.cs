using BusinessLogic.ReportsManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPManager;
using GTIService.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Reports.UserControls
{
    public partial class usrStockCardReportFilter : MISFilterBase
    {
        #region Variables & Properties
        private BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        private DataTable dtResult;
        #region Properties
        private bool usrStockCardReportFilter_IsPostBack
        {
            get
            {
                return this.ViewState["usrStockCardReportFilter_IsPostBack"] == null ? false : (bool)this.ViewState["usrStockCardReportFilter_IsPostBack"];
            }
            set
            {
                this.ViewState["usrStockCardReportFilter_IsPostBack"] = value;
            }
        }
        #endregion
        #endregion

        public override ReportParameters GetReportParameters()
        {
            try
            {
                ReportParameterName prms;
                ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
                tempReportParams.BizUnit = currentUser.SBUID;
                tempReportParams.Dept = currentUser.CurrentDeptPK;
                tempReportParams.UserPK = currentUser.PKUser;
                tempReportParams.RptPK = Session[ERP.Utilities.SessionStrings.REPORTPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK]);
                tempReportParams.Currency = currentUser.BaseCurrency;
                tempReportParams.FromDate = FromDate.Text;
                tempReportParams.ToDate = ToDate.Text;
                tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();

                #region ITM_CATEGORY
                if (Convert.ToInt32(ddlItemCategory.SelectedValue) > 0)
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "ITM_CATEGORY";
                    prms.Values.Add(
                                    new ReportParameterValues()
                                    {
                                        Value = ddlItemCategory.SelectedValue
                                    });
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region ITM_PK
                if (CheckListSearchControlNew.GetCheckedItems() != null && CheckListSearchControlNew.GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "ITM_PK";
                    foreach (ListItem item in CheckListSearchControlNew.GetCheckedItems())
                    {
                        prms.Values.Add(
                                   new ReportParameterValues()
                                   {
                                       Value = item.Value
                                   });
                    }
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region DPT_PK
                if (CheckListSearchControl1.GetCheckedItems() != null && CheckListSearchControl1.GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "DPT_PK";
                    foreach (ListItem item in CheckListSearchControl1.GetCheckedItems())
                    {
                        prms.Values.Add(
                                   new ReportParameterValues()
                                   {
                                       Value = item.Value
                                   });
                    }
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region TRAN_ONLY
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "TRAN_ONLY";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = chkTransaction.Checked ? "1" : "0"
                                });
                tempReportParams.Parameters.Add(prms);
                #endregion

                #region IS_RETURN
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "IS_RETURN";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = chkExcludeMatReturn.Checked ? "1" : "0"
                                });
                tempReportParams.Parameters.Add(prms);
                #endregion

                #region IPD_CLASSIFICATION
                if (Convert.ToInt32(ddlClassification.SelectedValue) > 0)
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "IPD_CLASSIFICATION";
                    prms.Values.Add(
                                    new ReportParameterValues()
                                    {
                                        Value = ddlClassification.SelectedValue
                                    });
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                #region FIN_YEAR
                if (Convert.ToInt32(ddlFinYear.SelectedValue) > 0)
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "FIN_YEAR";
                    prms.Values.Add(
                                    new ReportParameterValues()
                                    {
                                        Value = ddlFinYear.SelectedValue
                                    });
                    tempReportParams.Parameters.Add(prms);
                }
                #endregion

                return tempReportParams;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region PageLevel Events

        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "usrInitializeComponents", "$(document).ready(function(){usrInitComponents();});", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                if (!usrStockCardReportFilter_IsPostBack)
                {
                    usrStockCardReportFilter_IsPostBack = true;

                    GetFieldValues(ControlsEnum.ITEMCATEGORY);
                    SetFieldValues(ControlsEnum.ITEMCATEGORY);
                    GetFieldValues(ControlsEnum.STORES);
                    SetFieldValues(ControlsEnum.STORES);
                    GetFieldValues(ControlsEnum.FINYEAR);
                    SetFieldValues(ControlsEnum.FINYEAR);
                    GetFieldValues(ControlsEnum.CURRENTFINYEAR);
                    SetFieldValues(ControlsEnum.CURRENTFINYEAR);
                    GetFieldValues(ControlsEnum.FINYEARDATE);
                    SetFieldValues(ControlsEnum.FINYEARDATE);
                    dtResult = null;
                    SetFieldValues(ControlsEnum.CLASSIFICATION);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlItemCategory")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlFinYear")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGEDRES;
                    }
                }
                switch (commonActions)
                {
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.ITEMS);
                        SetFieldValues(ControlsEnum.ITEMS);
                        GetFieldValues(ControlsEnum.CLASSIFICATION);
                        SetFieldValues(ControlsEnum.CLASSIFICATION);
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGEDRES
                    case ActionsEnum.SELECTEDINDEXCHANGEDRES:
                        GetFieldValues(ControlsEnum.FINYEARDATE);
                        SetFieldValues(ControlsEnum.FINYEARDATE);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ITEMCATEGORY:
                        dtResult = GenerateReportBL.GetItemCategory(currentUser.SBUID);
                        break;
                    case ControlsEnum.ITEMS:
                        dtResult = GenerateReportBL.GetItemsByCategory(Convert.ToInt32(ddlItemCategory.SelectedValue));
                        break;
                    case ControlsEnum.STORES:
                        dtResult = GenerateReportBL.GetDeptStores(currentUser.SBUID);
                        break;
                    case ControlsEnum.CLASSIFICATION:
                        dtResult = GenerateReportBL.GetClassificationByCategory(Convert.ToInt32(ddlItemCategory.SelectedValue));
                        break;
                    case ControlsEnum.FINYEAR:
                        dtResult = GenerateReportBL.GetFinYear(currentUser.SBUID);
                        break;
                    case ControlsEnum.FINYEARDATE:
                        dtResult = GenerateReportBL.GetDateByFinYear(Convert.ToInt32(ddlFinYear.SelectedValue));
                        break;
                    case ControlsEnum.CURRENTFINYEAR:
                        dtResult = GenerateReportBL.GetCurrentFinYear(currentUser.SBUID);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ITEMCATEGORY:
                        ddlItemCategory.DataSource = dtResult;
                        ddlItemCategory.DataTextField = "Value";
                        ddlItemCategory.DataValueField = "PK";
                        ddlItemCategory.DataBind();
                        ddlItemCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.ITEMS:
                        var chkItems = dtResult.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(Resources.DataFieldRes.PK),
                            Value = row.Field<string>(Resources.DataFieldRes.Value),
                        }).ToList();
                        CheckListSearchControlNew.ListData = CommonFunctions.HtmlDecode(chkItems, "Value");
                        CheckListSearchControlNew.BindData();
                        break;
                    case ControlsEnum.STORES:
                        var chkStores = dtResult.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(Resources.DataFieldRes.PK),
                            Value = row.Field<string>(Resources.DataFieldRes.Value),
                        }).ToList();
                        CheckListSearchControl1.ListData = CommonFunctions.HtmlDecode(chkStores, "Value");
                        CheckListSearchControl1.BindData();
                        break;
                    case ControlsEnum.CLASSIFICATION:
                        ddlClassification.DataSource = dtResult;
                        ddlClassification.DataTextField = "Value";
                        ddlClassification.DataValueField = "PK";
                        ddlClassification.DataBind();
                        ddlClassification.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.FINYEAR:
                        ddlFinYear.DataSource = dtResult;
                        ddlFinYear.DataTextField = "Value";
                        ddlFinYear.DataValueField = "PK";
                        ddlFinYear.DataBind();
                        ddlFinYear.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.FINYEARDATE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            FromDate.Text = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            ToDate.Text = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);

                            //rgfvFromDate.MinimumValue = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat2);
                            //rgfvFromDate.MaximumValue = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat2);

                            //rgfvToDate.MinimumValue = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat2);
                            //rgfvToDate.MaximumValue = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat2);

                            hdfFromDate.Value= Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            hdfToDate.Value = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);
                        }
                        break;
                    case ControlsEnum.CURRENTFINYEAR:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlFinYear.SelectedValue = dtResult.Rows[0]["FYR_PK"].ToString();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Enums
        enum ControlsEnum
        {
            ITEMCATEGORY,
            ITEMS,
            STORES,
            CLASSIFICATION,
            FINYEAR,
            FINYEARDATE,
            CURRENTFINYEAR
        }
        #endregion
    }
}