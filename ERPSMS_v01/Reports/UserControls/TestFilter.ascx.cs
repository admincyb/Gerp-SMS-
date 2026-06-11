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
    public partial class TestFilter : MISFilterBase
    {
        #region Variables
        private BusinessObject.User currentUser;
        private bool bBindData = false;
        private ActionsEnum commonActions;
        private DataTable dtResult;
        #endregion

        #region Event
        
        #endregion

        #region Properties

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
            //prms = new GTIService.Dashboard.ReportParameterName();
            //prms.Values = new List<ReportParameterValues>();
            //prms.ParamName = "TXT_PARAM1";
            //prms.Values.Add(
            //                new ReportParameterValues()
            //                {
            //                    Value = txt1.Text.Trim()
            //                });
            //tempReportParams.Parameters.Add(prms);
            #endregion

            #region Param2
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "TXT_PARAM2";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txt2.Text.Trim()
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            #region Param3
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "TXT_PARAM3";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = ddl1.SelectedValue
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            #region Param4
            prms = new GTIService.Dashboard.ReportParameterName();
            prms.Values = new List<ReportParameterValues>();
            prms.ParamName = "TXT_PARAM4";
            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = ddl2.SelectedValue
                            });
            tempReportParams.Parameters.Add(prms);
            #endregion

            return tempReportParams;
        }

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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "usrInitUserComponents", "$(document).ready(function(){InitUserComponents();});", true);
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

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DataBind()
        {
            GetFieldValues(ControlsEnum.Dropdown);
            SetFieldValues(ControlsEnum.Dropdown);
        }

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddl1")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.BOMITEMTYPE);
                        SetFieldValues(ControlsEnum.BOMITEMTYPE);
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
                case ControlsEnum.BOMITEMTYPE:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "WO ITEM TYPE");
                    break;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.BOMITEMTYPE:
                    ddl2.DataSource = dtResult;
                    ddl2.DataTextField = "CFG_DATA";
                    ddl2.DataValueField = "CFG_VALUE";
                    ddl2.DataBind();
                    ddl2.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion

        #region Enums
        enum ControlsEnum
        {
            Dropdown,
            BOMITEMTYPE
        }
        #endregion
    }
}