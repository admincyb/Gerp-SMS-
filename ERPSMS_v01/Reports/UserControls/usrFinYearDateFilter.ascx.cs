using BusinessLogic.ReportsManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities;
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
    public partial class usrFinYearDateFilter : System.Web.UI.UserControl
    {
        #region Variables & Properties
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private ActionsEnum commonActions;
        #region Properties
        private bool usrFinYearDateFilter_IsPostBack
        {
            get
            {
                return this.ViewState["usrFinYearDateFilter_IsPostBack"] == null ? false : (bool)this.ViewState["usrFinYearDateFilter_IsPostBack"];
            }
            set
            {
                this.ViewState["usrFinYearDateFilter_IsPostBack"] = value;
            }
        }
        #endregion
        #endregion

        //public override ReportParameters GetReportParameters()
        //{
        //    try
        //    {
        //        ReportParameterName prms;
        //        ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
        //        return tempReportParams;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                if (!usrFinYearDateFilter_IsPostBack)
                {
                    usrFinYearDateFilter_IsPostBack = true;

                    GetFieldValues(ControlsEnum.FINYEAR);
                    SetFieldValues(ControlsEnum.FINYEAR);
                    GetFieldValues(ControlsEnum.CURRENTFINYEAR);
                    SetFieldValues(ControlsEnum.CURRENTFINYEAR);
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
                    if (((DropDownList)sender).ID == "ddlFinYear")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
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

        #region Get/Set Properties
        public string FromDate
        {
            get { return txtFromDate.Text.Trim(); }
            set { txtFromDate.Text = Convert.ToString(value); }
        }
        public string ToDate
        {
            get { return txtToDate.Text.Trim(); }
            set { txtToDate.Text = Convert.ToString(value); }
        }
        public string hdf_FromDate
        {
            get { return hdfFromDate.Value; }
            set { hdfFromDate.Value = Convert.ToString(value); }
        }
        public string hdf_ToDate
        {
            get { return hdfToDate.Value; }
            set { hdfToDate.Value = Convert.ToString(value); }
        }
        public string FinYear
        {
            get { return ddlFinYear.SelectedValue; }
            set { ddlFinYear.SelectedValue = Convert.ToString(value); }
        }
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
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
                            txtFromDate.Text = txtFrom.Text = Convert.ToDateTime(dtResult.Rows[0]["FORM_DT"]).ToString(Resources.ErpRes.DateFormat);
                            hdfFromDate.Value = Convert.ToDateTime(txtFromDate.Text).ToString(Resources.Constants.DateFormatShort);
                            txtToDate.Text = txtTo.Text = Convert.ToDateTime(dtResult.Rows[0]["TO_DT"]).ToString(Resources.ErpRes.DateFormat);
                            hdfToDate.Value = Convert.ToDateTime(txtToDate.Text).ToString(Resources.Constants.DateFormatShort);
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
            FINYEAR,
            FINYEARDATE,
            CURRENTFINYEAR
        }
        #endregion
    }
}