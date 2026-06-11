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
    public partial class OnlineParameterControlSheet : MISFilterBase
    {
        #region Variables
        private BusinessObject.User currentUser;
        private bool bBindData = false;
        private ActionsEnum commonActions;

        private DataTable dtResult;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "usrInitUserComponents", "$(document).ready(function(){InitUserComponents();});", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
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
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "PTH_DATE";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = PTH_DATE.Text.Trim()
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            #region Param2
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "LNE_PK";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txtLNE_PK.SelectedValue
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            #region Param3
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "PTH_PRODUCT";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txtPTH_PRODUCT.SelectedValue
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            //#region Param4
            //prms = new GTIService.Dashboard.ReportParameterName();
            //prms.Values = new List<ReportParameterValues>();
            //prms.ParamName = "TXT_PARAM4";
            //prms.Values.Add(
            //                new ReportParameterValues()
            //                {
            //                    Value = ddl2.SelectedValue
            //                });
            //tempReportParams.Parameters.Add(prms);
            //#endregion

            return tempReportParams;
        }

        private void PageActionHandler()
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
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
                    #region DATECHANGED
                    case ActionsEnum.DATECHANGED:
                        GetFieldValues(ControlsEnum.DATECHANGED);
                        SetFieldValues(ControlsEnum.DATECHANGED);
                        break;
                    #endregion

                    #region LINEPKCHANGED
                    case ActionsEnum.LINEPKCHANGED:
                        GetFieldValues(ControlsEnum.LINEPKCHANGED);
                        SetFieldValues(ControlsEnum.LINEPKCHANGED);
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
            switch (controlType)
            {
                case ControlsEnum.DATECHANGED:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetLineDetailsforOnlineParameter(Convert.ToDateTime(PTH_DATE.Text));

                    txtLNE_PK.DataSource = dtResult;
                    txtLNE_PK.DataTextField = "Value";
                    txtLNE_PK.DataValueField = "pk";
                    txtLNE_PK.DataBind();
                    txtLNE_PK.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    txtPTH_PRODUCT.Items.Clear();

                    break;

                case ControlsEnum.LINEPKCHANGED:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetLineProductDetailsforOnlineParameter(Convert.ToDateTime(PTH_DATE.Text), txtLNE_PK.SelectedValue);
                    txtPTH_PRODUCT.Items.Clear();
                    txtPTH_PRODUCT.DataSource = dtResult;
                    txtPTH_PRODUCT.DataTextField = "Value";
                    txtPTH_PRODUCT.DataValueField = "pk";
                    txtPTH_PRODUCT.DataBind();
                    txtPTH_PRODUCT.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            //switch (controlType)
            //{

            //}
        }
        #endregion

        protected void PTH_DATE_TextChanged(EventArgs e)
        {
            dtResult = BusinessLogic.CommonManagement.CommonBL.GetLineDetailsforOnlineParameter(Convert.ToDateTime(PTH_DATE.Text));
            txtLNE_PK.DataSource = dtResult;
            txtLNE_PK.DataTextField = "PK";
            txtLNE_PK.DataValueField = "Value";
            txtLNE_PK.DataBind();
            txtLNE_PK.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

        }

        public void HandleTextBoxTextChanged(object sender, EventArgs e)
        {
            this.PTH_DATE_TextChanged(EventArgs.Empty);
        }
    }
}