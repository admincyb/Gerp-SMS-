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
    public partial class usrRawMaterialConsumptionFilter : MISFilterBase
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
                if (CheckListSearchControl.GetCheckedItems() != null && CheckListSearchControl.GetCheckedItems().Any())
                {
                    prms = new GTIService.Dashboard.ReportParameterName();
                    prms.Values = new List<ReportParameterValues>();
                    prms.ParamName = "DPT_PK";
                    foreach (ListItem item in CheckListSearchControl.GetCheckedItems())
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
                    GetFieldValues(ControlsEnum.TRANSACTIONMODE);
                    SetFieldValues(ControlsEnum.TRANSACTIONMODE);
                    GetFieldValues(ControlsEnum.PLANT);
                    SetFieldValues(ControlsEnum.PLANT);
                    GetFieldValues(ControlsEnum.FINYEAR);
                    GetFieldValues(ControlsEnum.FINYEARDATE);
                    SetFieldValues(ControlsEnum.FINYEARDATE);
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
                    else if (((DropDownList)sender).ID == "ddlTransMode")
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
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGEDRES
                    case ActionsEnum.SELECTEDINDEXCHANGEDRES:
                        GetFieldValues(ControlsEnum.TRANSACTIONNO);
                        SetFieldValues(ControlsEnum.TRANSACTIONNO);
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
                    case ControlsEnum.TRANSACTIONNO:
                        dtResult = GenerateReportBL.GetTransactionNoByMode(Convert.ToInt32(ddlTransMode.SelectedValue),FromDate.Text,ToDate.Text);
                        break;
                    case ControlsEnum.TRANSACTIONMODE:
                        dtResult = GenerateReportBL.GetTrasactionMode();
                        break;
                    case ControlsEnum.PLANT:
                        dtResult = GenerateReportBL.GetPlant();
                        break;
                    //case ControlsEnum.STORES:
                    //    dtResult = GenerateReportBL.GetDeptStores(currentUser.SBUID);

                    //    break;
                    //case ControlsEnum.CLASSIFICATION:
                    //    dtResult = GenerateReportBL.GetClassificationByCategory(Convert.ToInt32(ddlItemCategory.SelectedValue));
                    //    break;
                    case ControlsEnum.FINYEAR:
                        dtResult = GenerateReportBL.GetFinYear(currentUser.SBUID);
                        break;
                    case ControlsEnum.FINYEARDATE:
                        dtResult = GenerateReportBL.GetDateByFinYear(Convert.ToInt32(dtResult.Rows[0][0]));
                        break;
                        //case ControlsEnum.CURRENTFINYEAR:
                        //    dtResult = GenerateReportBL.GetCurrentFinYear(currentUser.SBUID);
                        //    break;
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
                    case ControlsEnum.TRANSACTIONNO:
                        var chkTranNo = dtResult.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(Resources.DataFieldRes.PK),
                            Value = row.Field<string>(Resources.DataFieldRes.Value),
                        }).ToList();
                        CheckListSearchControl.ListData = CommonFunctions.HtmlDecode(chkTranNo, "Value");
                        CheckListSearchControl.BindData();
                        break;
                    case ControlsEnum.TRANSACTIONMODE:
                        ddlTransMode.DataSource = dtResult;
                        ddlTransMode.DataTextField = "Value";
                        ddlTransMode.DataValueField = "PK";
                        ddlTransMode.DataBind();
                        ddlTransMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.PLANT:
                        ddlPlant.DataSource = dtResult;
                        ddlPlant.DataTextField = "Value";
                        ddlPlant.DataValueField = "PK";
                        ddlPlant.DataBind();
                        ddlPlant.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.FINYEARDATE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            FromDate.Text = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            ToDate.Text = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);
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
            FINYEAR,
            FINYEARDATE,
            TRANSACTIONMODE,
            PLANT,
            TRANSACTIONNO,

        }
        #endregion
    }
}