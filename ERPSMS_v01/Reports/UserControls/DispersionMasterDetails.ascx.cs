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
    public partial class DispersionMasterDetails1 : MISFilterBase
    {
        #region Variables
        private BusinessObject.User currentUser;
        private bool bBindData = false;
        private ActionsEnum commonActions;

        private DataTable dtResult;
        #region Properties
        private bool DispersionMasterDetails_IsPostBack
        {
            get
            {
                return this.ViewState["DispersionMasterDetails_IsPostBack"] == null ? false : (bool)this.ViewState["DispersionMasterDetails_IsPostBack"];
            }
            set
            {
                this.ViewState["DispersionMasterDetails_IsPostBack"] = value;
            }
        }
        #endregion
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponentsDSP", "$(document).ready(function(){InitComponentsDSP();});", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void PageActionHandler()
        {
            try
            {
                if (!DispersionMasterDetails_IsPostBack)
                {
                    DispersionMasterDetails_IsPostBack = true;
                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);
                    GetFieldValues(ControlsEnum.PLANT);
                    SetFieldValues(ControlsEnum.PLANT);
                    GetFieldValues(ControlsEnum.STATUS);
                    SetFieldValues(ControlsEnum.STATUS);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.TYPE:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetDispersionTypes();
                    break;
                case ControlsEnum.PLANT:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(currentUser.CurrentSBUPK);
                    break;
                case ControlsEnum.STATUS:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetStatus();
                    break;


            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.TYPE:
                    ddlType.DataSource = dtResult;
                    ddlType.DataValueField = "PK";
                    ddlType.DataTextField = "Value";
                    ddlType.DataBind();
                    //ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.PLANT:
                    ddlPlant.DataSource = dtResult;
                    ddlPlant.DataValueField = "PK";
                    ddlPlant.DataTextField = "Value";
                    ddlPlant.DataBind();
                    ddlPlant.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.STATUS:
                    ddlStatus.DataSource = dtResult;
                    ddlStatus.DataTextField = "Value";
                    ddlStatus.DataValueField = "pk";
                    ddlStatus.DataBind();
                    ddlStatus.Items.Insert(0, new ListItem(Resources.ErpRes.All, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion
        public override ReportParameters GetReportParameters()
        {
            ReportParameterName prms;
            ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
            tempReportParams.BizUnit = currentUser.SBUID;
            tempReportParams.Dept = currentUser.CurrentDeptPK;
            tempReportParams.UserPK = currentUser.PKUser;
            tempReportParams.RptPK = Session[ERP.Utilities.SessionStrings.REPORTPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK]);
            tempReportParams.Currency = currentUser.BaseCurrency;
            tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();

            #region Param1
            if (hdfMasterCode.Value != "0" && hdfMasterCode.Value != "")
            {
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "DSP_PK";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = hdfMasterCode.Value
                                });
                tempReportParams.Parameters.Add(prms);
            }
            #endregion

            #region Param2
            if (Convert.ToInt32(ddlType.SelectedValue)>0)
            {
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "DSP_TYPE";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = ddlType.SelectedValue
                                });
                tempReportParams.Parameters.Add(prms);
            }
            #endregion

            #region Param3
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "STATUS";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = ddlStatus.SelectedValue
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion
            #region Param4
            if (Convert.ToInt32(ddlPlant.SelectedValue) > 0)
            {
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                prms.ParamName = "CMP_PK";
                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = ddlPlant.SelectedValue
                                });
                tempReportParams.Parameters.Add(prms);
            }
            #endregion
            return tempReportParams;
        }
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "txtLNE_PK")
                    {
                        commonActions = ActionsEnum.LINEPKCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECK_CHANGE;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {

                    if (((TextBox)sender).ID == "PTH_DATE")
                    {
                        commonActions = ActionsEnum.DATECHANGED;
                    }



                }
                switch (commonActions)
                {
                    //#region DATECHANGED
                    //case ActionsEnum.DATECHANGED:
                    //    GetFieldValues(ControlsEnum.DATECHANGED);
                    //    SetFieldValues(ControlsEnum.DATECHANGED);
                    //    break;
                    //#endregion

                    //#region LINEPKCHANGED
                    //case ActionsEnum.LINEPKCHANGED:
                    //    GetFieldValues(ControlsEnum.LINEPKCHANGED);
                    //    SetFieldValues(ControlsEnum.LINEPKCHANGED);
                    //    break;
                    //    #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}