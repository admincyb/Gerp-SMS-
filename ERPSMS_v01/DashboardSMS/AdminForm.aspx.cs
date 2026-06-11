using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GTIService.Constants.Common;
using BusinessObject.AccountManagement;
using GTIService;
using gAssetsData;
using gAssetsManager;
using CustomControls;
using ERPSMS_v01.UserControls;

namespace ERPSMS_v01.DashboardSMS
{
    public partial class AdminForm : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

        BusinessObject.User CurrentUser;

        private DateTime PripertyLastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.PripertyLastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.PripertyLastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.PripertyLastModifiedTime] = value;
            }
        }
        /// <summary>
        /// Current Chart Type
        /// </summary>
        private short CurrChartType
        {
            get
            {
                return Convert.ToInt16(this.ViewState[ViewstateStrings.CurrChartType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrChartType] = value;
            }
        }
        /// <summary>
        /// Current Page
        /// </summary>
        private int CurrPage
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPage]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPage] = value;
            }
        }
        /// <summary>
        /// Current Row
        /// </summary>
        private int CurrRow
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrRow]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrRow] = value;
            }
        }
        /// <summary>
        /// Current Cell
        /// </summary>
        private int CurrCell
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrCell]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrCell] = value;
            }
        }
        /// <summary>
        /// Current Param
        /// </summary>
        private int CurrParam
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrParam]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrParam] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }
        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
            }
        }
        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        #endregion
        private ActionsEnum commonActions;

        //page related class objects
        private DsbPageMst dsbPageMstObj;
        private DsbRowMst dsbRowMstObj;
        private DsbDashletMst dsbDashletMstObj;
        private DsbDashletPropertyCfg dsbDashletPropertyCfgObj;
        private DsbDshProprtyMpg dsbDshProprtyMpgObj;
        private DsbFilterParameterMst dsbFilterParameterMstObj;
        private DsbChartTypeCfg dsbChartTypeCfgObj;
        private DsbChtSubTypeCfg dsbChtSubTypeCfgObj;
        private ServiceUtility serviceUtilityObj;

        //List for binding details to controls
        private List<DsbPageMst> dsbPageMstLst;
        private List<DsbRowMst> dsbRowMstLst;
        private List<DsbDashletMst> dsbDashletMstLst;
        private List<DsbDashletPropertyCfg> dsbDashletPropertyCfgLst;
        private List<DsbDshProprtyMpg> dsbDshProprtyMpgLst;
        private List<DsbFilterParameterMst> dsbFilterParameterMstLst;
        private List<DsbChartTypeCfg> dsbChartTypeCfgLst;
        private List<DsbChtSubTypeCfg> dsbChtSubTypeCfgLst;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            PageActionHandler();
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            DashBoardAdminServiceClient dashBoardAdminServiceClient;
            dashBoardAdminServiceClient = null;
            try
            {
                dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                switch (type)
                {
                    //Used Case to swich using Controls Enum in this Name Space
                    case ControlsEnum.DEFAULT:
                        dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdPageHierarchy.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.DsbPagePK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortAscending : SortDirection;
                        dsbPageMstObj.PagePk = CurrPage;
                        dsbPageMstObj.PageBizUnit = Convert.ToInt16(CurrentUser.SBUID);
                        dsbPageMstObj.PageDept = CurrentUser.CurrentDeptPK;
                        dsbPageMstLst = dashBoardAdminServiceClient.GetDsbPageMst(dsbPageMstObj, serviceUtilityObj);
                        serviceUtilityObj = dashBoardAdminServiceClient.GetDsbPageMstCount(dsbPageMstObj, serviceUtilityObj);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;

                    case ControlsEnum.PAGE:
                        dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbPageMstObj.PagePk = CurrPage;
                        dsbPageMstObj.PageBizUnit = Convert.ToInt16(CurrentUser.SBUID);
                        dsbPageMstObj.PageDept = CurrentUser.CurrentDeptPK;
                        dsbPageMstLst = dashBoardAdminServiceClient.GetDsbPageMst(dsbPageMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.ROW:
                        dsbRowMstObj = dashBoardAdminServiceClient.GetInitilizedDsbRowMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbRowMstObj.RowPagePk = CurrPage;
                        dsbRowMstObj.RowPk = CurrRow;
                        dsbRowMstLst = dashBoardAdminServiceClient.GetDsbRowMst(dsbRowMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.CELL:
                        dsbDashletMstObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbDashletMstObj.DashletRowPk = CurrRow;
                        dsbDashletMstObj.DashletPk = CurrCell;
                        dsbDashletMstLst = dashBoardAdminServiceClient.GetDsbDashletMst(dsbDashletMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.CHARTPROPS:
                        dsbDashletPropertyCfgObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletPropertyCfg();
                        dsbDshProprtyMpgObj = dashBoardAdminServiceClient.GetInitilizedDsbDshProprtyMpg();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DashletPropertyName;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbDashletPropertyCfgObj.PropertyChtType = CurrChartType;
                        dsbDashletPropertyCfgObj.PropertyPk = 0;
                        dsbDshProprtyMpgObj.DshPrptDashletPk = CurrCell;
                        dsbDashletPropertyCfgObj.DsbDshProprtyMpgs = new List<gAssetsData.DsbDshProprtyMpg>();
                        dsbDashletPropertyCfgObj.DsbDshProprtyMpgs.Add(dsbDshProprtyMpgObj);
                        dsbDashletPropertyCfgLst = dashBoardAdminServiceClient.GetDsbDashletPropertyCfg(dsbDashletPropertyCfgObj, serviceUtilityObj);

                        break;

                    case ControlsEnum.CELLPROPS:
                        //dsbDshProprtyMpgObj = dashBoardAdminServiceClient.GetInitilizedDsbDshProprtyMpg();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbDshProprtyMpgObj.DshPrptDashletPk = CurrCell;
                        //dsbDshProprtyMpgLst = dashBoardAdminServiceClient.GetDsbDshProprtyMpg(dsbDshProprtyMpgObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.CELLFILTER:
                        dsbFilterParameterMstObj = dashBoardAdminServiceClient.GetInitilizedDsbFilterParameterMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DashletParameterPK;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbFilterParameterMstObj.ParamDashletPk = CurrCell;
                        dsbFilterParameterMstObj.ParamPk = CurrParam;
                        dsbFilterParameterMstLst = dashBoardAdminServiceClient.GetDsbFilterParameterMst(dsbFilterParameterMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.CHARTTYPE:
                        dsbChartTypeCfgObj = dashBoardAdminServiceClient.GetInitilizedDsbChartTypeCfg();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.ChartTypeName;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbChartTypeCfgObj.ctpPK = 0;
                        dsbChartTypeCfgLst = dashBoardAdminServiceClient.GetDsbChartTypeCfg(dsbChartTypeCfgObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.CHTSUBTYPE:
                        dsbChtSubTypeCfgObj = dashBoardAdminServiceClient.GetInitilizedChtSubTypeCfg();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.ChtSubTypeName;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbChtSubTypeCfgObj.cstChartType = Convert.ToInt16(ddlChartType.SelectedValue);
                        dsbChtSubTypeCfgObj.cstPK = 0;
                        dsbChtSubTypeCfgLst = dashBoardAdminServiceClient.GetChtSubTypeCfg(dsbChtSubTypeCfgObj, serviceUtilityObj);
                        break;
                }
                dashBoardAdminServiceClient.Close();
            }
            catch (Exception ex)
            {
                dashBoardAdminServiceClient.Abort();
                throw ex;
            }
            finally
            {
                dsbPageMstObj = null;
                dsbRowMstObj = null;
                dsbDashletMstObj = null;
                dsbChartTypeCfgObj = null;
                dsbChtSubTypeCfgObj = null;
                serviceUtilityObj = null;
                dashBoardAdminServiceClient = null;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    //Used Fill the UI Values From Object
                    case ControlsEnum.PAGE:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid(ControlsEnum.PAGE);
                        break;
                    case ControlsEnum.ROW:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.CELL:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.CHARTPROPS:
                        BindGrid(ControlsEnum.CHARTPROPS);
                        break;
                    case ControlsEnum.CELLPROPS:
                        GetUIValuesFromObject(ControlsEnum.CELLPROPS);
                        break;
                    case ControlsEnum.CELLFILTER:
                        BindGrid(ControlsEnum.CELLFILTER);
                        break;
                    case ControlsEnum.SELECTFILTER:
                        GetUIValuesFromObject(ControlsEnum.SELECTFILTER);
                        break;
                    case ControlsEnum.CHARTTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.CHTSUBTYPE:
                        BindDropDown(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>       
        private object SetUIValuesToObject(ControlsEnum controlType)
        {
            object retn;
            TextBox txt;
            retn = null;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.PAGE:
                        dsbPageMstObj.PagePk = CurrPage;
                        dsbPageMstObj.PageTitle = HttpUtility.HtmlEncode(txtPageTitle.Text.Trim());
                        dsbPageMstObj.PageDescription = HttpUtility.HtmlEncode(txtPageDescription.Text.Trim());
                        dsbPageMstObj.PageRowCount = Convert.ToInt32(ddlNoOfRows.SelectedValue);

                        dsbPageMstObj.PageDept = CurrentUser.CurrentDeptPK;
                        dsbPageMstObj.PageBizUnit = Convert.ToInt16(CurrentUser.SBUID);
                        dsbPageMstObj.PageCrtdBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbPageMstObj.PageCrtdOn = System.DateTime.Now;
                        dsbPageMstObj.PageModBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbPageMstObj.PageModOn = LastModifiedTime;
                        retn = dsbPageMstObj;
                        break;

                    case ControlsEnum.ROW:
                        dsbRowMstObj.RowPagePk = CurrPage;
                        dsbRowMstObj.RowPk = CurrRow;
                        dsbRowMstObj.RowTitle = HttpUtility.HtmlEncode(txtRowTitle.Text.Trim());
                        dsbRowMstObj.RowDescription = HttpUtility.HtmlEncode(txtRowDesc.Text.Trim());
                        dsbRowMstObj.RowLayout = Convert.ToInt16(ddlRowLayout.SelectedValue);
                        dsbRowMstObj.RowCrtdBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbRowMstObj.RowCrtdOn = System.DateTime.Now;
                        dsbRowMstObj.RowModBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbRowMstObj.RowModOn = LastModifiedTime;
                        retn = dsbRowMstObj;
                        break;

                    case ControlsEnum.CELL:
                        dsbDashletMstObj.DashletRowPk = CurrRow;
                        dsbDashletMstObj.DashletPk = CurrCell;
                        dsbDashletMstObj.DashletTitle = HttpUtility.HtmlEncode(txtDashletTitle.Text.Trim());
                        dsbDashletMstObj.DashletSourceIsService = chkUseService.Checked;
                        dsbDashletMstObj.DashletDataSource = HttpUtility.HtmlEncode(txtDataSource.Text.Trim());
                        dsbDashletMstObj.DashletMethod = HttpUtility.HtmlEncode(txtServiceMethod.Text.Trim());
                        dsbDashletMstObj.DashletChartType = Convert.ToInt16(ddlChartType.SelectedValue);
                        dsbDashletMstObj.DashletChtSubType = Convert.ToInt16(ddlChartSubType.SelectedValue);
                        dsbDashletMstObj.DashletSeries = Convert.ToInt32(ddlNoOfSeries.SelectedValue);
                        dsbDashletMstObj.DashletReportUrl = HttpUtility.HtmlEncode(txtReportUrl.Text.Trim());
                        dsbDashletMstObj.DashletCrtdBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbDashletMstObj.DashletCrtdOn = System.DateTime.Now;
                        dsbDashletMstObj.DashletModBy = Convert.ToInt16(CurrentUser.PKUser);
                        dsbDashletMstObj.DashletModOn = LastModifiedTime;
                        retn = dsbDashletMstObj;
                        break;

                    case ControlsEnum.CELLPROPS:
                        dsbDshProprtyMpgLst = new List<DsbDshProprtyMpg>();
                        dsbDashletMstObj.DashletPropertyModOn = PripertyLastModifiedTime;
                        foreach (GridViewRow gvr in grdDashletProperties.Rows)
                        {
                            txt = gvr.Cells[2].FindControl("txtPropertyValue") as TextBox;
                            if (txt != null && txt.Text.Trim() != string.Empty)
                            {
                                //dsbDshProprtyMpgObj.DshPrptDashletPk = CurrCell;
                                //dsbDshProprtyMpgObj.DshPrptPropertyPk = Convert.ToInt32(grdDashletProperties.DataKeys[gvr.RowIndex].Values[0]);
                                //dsbDshProprtyMpgObj.DshPrptValue = HttpUtility.HtmlEncode(txt.Text.Trim());
                                dsbDshProprtyMpgLst.Add(new DsbDshProprtyMpg()
                                {
                                    DshPrptDashletPk = CurrCell,
                                    DshPrptPropertyPk = Convert.ToInt32(grdDashletProperties.DataKeys[gvr.RowIndex].Values[0]),
                                    DshPrptValue = HttpUtility.HtmlEncode(txt.Text.Trim()),
                                    DsbDashletMst = dsbDashletMstObj
                                });
                            }
                        }
                        if (dsbDshProprtyMpgLst.Count == 0)
                        {
                            dsbDshProprtyMpgLst.Add(new DsbDshProprtyMpg()
                            {
                                DshPrptDashletPk = CurrCell,
                                DshPrptPropertyPk = -1,
                                DshPrptValue = string.Empty,
                                DsbDashletMst = dsbDashletMstObj
                            });
                        }
                        retn = dsbDshProprtyMpgLst;
                        break;

                    case ControlsEnum.CELLFILTER:
                        dsbFilterParameterMstObj.ParamDashletPk = CurrCell;
                        dsbFilterParameterMstObj.ParamPk = CurrParam;
                        dsbFilterParameterMstObj.ParamName = HttpUtility.HtmlEncode(txtParameterName.Text.Trim());
                        dsbFilterParameterMstObj.ParamSourceIsService = chkParameterUseService.Checked;
                        dsbFilterParameterMstObj.ParamDatasource = HttpUtility.HtmlEncode(txtParameterDatasource.Text.Trim());
                        dsbFilterParameterMstObj.ParamMethod = HttpUtility.HtmlEncode(txtParameterMethod.Text.Trim());
                        dsbFilterParameterMstObj.ParamLabel = HttpUtility.HtmlEncode(txtParameterLabel.Text.Trim());
                        dsbFilterParameterMstObj.ParamDescription = HttpUtility.HtmlEncode(txtParameterDescription.Text.Trim());
                        retn = dsbFilterParameterMstObj;
                        break;
                }
                return retn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsbPageMstObj = null;
                dsbRowMstObj = null;
                dsbDashletMstObj = null;
                dsbDshProprtyMpgObj = null;
                dsbDshProprtyMpgLst = null;
                dsbFilterParameterMstObj = null;
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            TextBox txt;
            try
            {
                switch (controlType)
                {
                    //assigning the UI controls with the corresponding value from the MstLst
                    case ControlsEnum.PAGE:
                        if (dsbPageMstLst != null && dsbPageMstLst.Count() > 0)
                        {
                            CurrPage = dsbPageMstLst[0].PagePk;
                            txtPageTitle.Text = HttpUtility.HtmlDecode(dsbPageMstLst[0].PageTitle);
                            txtPageDescription.Text = HttpUtility.HtmlDecode(dsbPageMstLst[0].PageDescription);
                            ddlNoOfRows.SelectedValue = dsbPageMstLst[0].PageRowCount.ToString();
                            LastModifiedTime = dsbPageMstLst[0].PageModOn;
                            lblLastModifiedPageHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            PageModifiedDatePnl.Visible = true;
                        }
                        //Concurrency Page Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbPage);
                            ResetForm(ControlsEnum.PAGE);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            PageModifiedDatePnl.Visible = false;
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;
                    case ControlsEnum.ROW:
                        if (dsbRowMstLst != null && dsbRowMstLst.Count() > 0)
                        {
                            CurrRow = dsbRowMstLst[0].RowPk;
                            CurrPage = dsbRowMstLst[0].RowPagePk;
                            lblRowPage.Text = HttpUtility.HtmlDecode(dsbRowMstLst[0].DsbPageMst.PageTitle);
                            txtRowTitle.Text = HttpUtility.HtmlDecode(dsbRowMstLst[0].RowTitle);
                            txtRowDesc.Text = HttpUtility.HtmlDecode(dsbRowMstLst[0].RowDescription);
                            ddlRowLayout.SelectedValue = dsbRowMstLst[0].RowLayout.ToString();
                            LastModifiedTime = dsbRowMstLst[0].RowModOn;
                            lblLastModifiedRowHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            RowModifiedDatePnl.Visible = true;
                        }
                        //Concurrency Row Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbRow);
                            ResetForm(ControlsEnum.ROW);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            RowModifiedDatePnl.Visible = false;
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;
                    case ControlsEnum.CELL:
                        if (dsbDashletMstLst != null && dsbDashletMstLst.Count() > 0)
                        {
                            CurrCell = dsbDashletMstLst[0].DashletPk;
                            CurrRow = dsbDashletMstLst[0].DashletRowPk;
                            CurrChartType = dsbDashletMstLst[0].DsbChtSubTypeCfg.cstChartType;
                            lblCellRow.Text = HttpUtility.HtmlDecode(dsbDashletMstLst[0].DsbRowMst.RowTitle);
                            lblCellPage.Text = HttpUtility.HtmlDecode(dsbDashletMstLst[0].DsbRowMst.DsbPageMst.PageTitle);
                            txtDashletTitle.Text = HttpUtility.HtmlDecode(dsbDashletMstLst[0].DashletTitle);
                            chkUseService.Checked = dsbDashletMstLst[0].DashletSourceIsService;
                            txtDataSource.Text = HttpUtility.HtmlDecode(dsbDashletMstLst[0].DashletDataSource);
                            txtServiceMethod.Text = HttpUtility.HtmlDecode(dsbDashletMstLst[0].DashletMethod);
                            ddlChartType.SelectedValue = dsbDashletMstLst[0].DsbChtSubTypeCfg.cstChartType.ToString();
                            GetFieldValues(ControlsEnum.CHTSUBTYPE);
                            SetFieldValues(ControlsEnum.CHTSUBTYPE);
                            ddlChartSubType.SelectedValue = dsbDashletMstLst[0].DashletChtSubType.ToString();
                            ddlNoOfSeries.SelectedValue = dsbDashletMstLst[0].DashletSeries.ToString();
                            txtReportUrl.Text = HttpUtility.HtmlEncode(dsbDashletMstLst[0].DashletReportUrl);
                            LastModifiedTime = dsbDashletMstLst[0].DashletModOn;
                            lblLastModifiedDashletHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            DashletModifiedDatePnl.Visible = true;
                            PripertyLastModifiedTime = dsbDashletMstLst[0].DashletPropertyModOn;
                            lblLastModifiedPropertyHDR.Text = Resources.Report.LastModifiedOn + PripertyLastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            PropertyModifiedDatePanel.Visible = true;
                        }
                        //Concurrency Dashlet Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbCell);
                            ResetForm(ControlsEnum.CELL);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            DashletModifiedDatePnl.Visible = false;
                            PropertyModifiedDatePanel.Visible = false;
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;

                    case ControlsEnum.CELLPROPS:
                        if (dsbDshProprtyMpgLst != null && dsbDshProprtyMpgLst.Count() > 0)
                        {
                            foreach (GridViewRow gvr in grdDashletProperties.Rows)
                            {
                                foreach (DsbDshProprtyMpg mpg in dsbDshProprtyMpgLst)
                                {
                                    if (Convert.ToInt32(grdDashletProperties.DataKeys[gvr.RowIndex].Values[0]) == mpg.DshPrptPropertyPk)
                                    {
                                        txt = gvr.Cells[2].FindControl("txtPropertyValue") as TextBox;
                                        if (txt != null && mpg.DshPrptValue.Trim() != string.Empty)
                                            txt.Text = mpg.DshPrptValue;
                                    }
                                }
                            }
                        }
                        //Concurrency Properties Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.Controls.DsbCell);
                            CurrRow = 0;
                            ResetForm(ControlsEnum.CELLPROPS);
                            GetFieldValues(ControlsEnum.CELL);
                            SetFieldValues(ControlsEnum.CELL);
                            txtDashletTitle.Focus();
                            SetTab(TabsEnum.CELLBASIC);
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;

                    case ControlsEnum.SELECTFILTER:
                        if (dsbFilterParameterMstLst != null && dsbFilterParameterMstLst.Count() > 0)
                        {
                            CurrParam = dsbFilterParameterMstLst[0].ParamPk;
                            CurrCell = dsbFilterParameterMstLst[0].ParamDashletPk;
                            txtParameterName.Text = HttpUtility.HtmlDecode(dsbFilterParameterMstLst[0].ParamName);
                            txtParameterLabel.Text = HttpUtility.HtmlDecode(dsbFilterParameterMstLst[0].ParamLabel);
                            chkParameterUseService.Checked = dsbFilterParameterMstLst[0].ParamSourceIsService;
                            txtParameterDatasource.Text = HttpUtility.HtmlDecode(dsbFilterParameterMstLst[0].ParamDatasource);
                            txtParameterMethod.Text = HttpUtility.HtmlDecode(dsbFilterParameterMstLst[0].ParamMethod);
                            txtParameterDescription.Text = HttpUtility.HtmlDecode(dsbFilterParameterMstLst[0].ParamDescription);
                            btnDeleteParams.Visible = true;
                        }
                        //Concurrency Parameter Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.Controls.DsbFilterParams);
                            ResetForm(ControlsEnum.CELLFILTER);
                            GetFieldValues(ControlsEnum.CELLFILTER);
                            SetFieldValues(ControlsEnum.CELLFILTER);
                            txtParameterName.Focus();
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.PAGE:
                        if (dsbPageMstLst != null)
                        {
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdPageHierarchy.DataSource = dsbPageMstLst;
                            grdPageHierarchy.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        break;

                    case ControlsEnum.CHARTPROPS:
                        if (dsbDashletPropertyCfgLst != null)
                        {
                            grdDashletProperties.DataSource = dsbDashletPropertyCfgLst;
                            grdDashletProperties.DataBind();
                        }
                        break;

                    case ControlsEnum.CELLFILTER:
                        if (dsbFilterParameterMstLst != null)
                        {
                            grdParameter.DataSource = dsbFilterParameterMstLst;
                            grdParameter.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Bind DropDowns
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.CHARTTYPE:
                        ddlChartType.Items.Clear();
                        if (dsbChartTypeCfgLst != null && dsbChartTypeCfgLst.Count() > 0)
                        {
                            ddlChartType.DataSource = dsbChartTypeCfgLst;
                            ddlChartType.DataTextField = Resources.BindValues.ChartTypeName;
                            ddlChartType.DataValueField = Resources.BindValues.ChartTypePK;
                            ddlChartType.DataBind();
                        }
                        ddlChartType.Items.Insert(0, new ListItem(Resources.BindValues.Select, CommonConstants.SELECTVAL));
                        ddlChartType.SelectedIndex = 0;
                        break;

                    case ControlsEnum.CHTSUBTYPE:
                        ddlChartSubType.Items.Clear();
                        if (dsbChtSubTypeCfgLst != null && dsbChtSubTypeCfgLst.Count() > 0)
                        {
                            ddlChartSubType.DataSource = dsbChtSubTypeCfgLst;
                            ddlChartSubType.DataTextField = Resources.BindValues.ChtSubTypeName;
                            ddlChartSubType.DataValueField = Resources.BindValues.ChtSubTypePK;
                            ddlChartSubType.DataBind();
                        }
                        ddlChartSubType.Items.Insert(0, new ListItem(Resources.BindValues.Select, CommonConstants.SELECTVAL));
                        ddlChartSubType.SelectedIndex = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(object sender)
        {
            GridViewRow gvr;
            ExtGridView egrd;
            GridView grd;
            try
            {
                switch (commonActions)
                {
                    case ActionsEnum.DSBSELECTPAGE:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            CurrPage = Convert.ToInt32(grdPageHierarchy.DataKeys[gvr.RowIndex].Values[0]);
                            GetFieldValues(ControlsEnum.PAGE);
                            SetFieldValues(ControlsEnum.PAGE);
                            txtPageTitle.Focus();
                        }
                        break;

                    case ActionsEnum.DSBSELECTROW:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            egrd = gvr.Parent.Parent as ExtGridView;
                            if (egrd != null)
                            {
                                CurrRow = Convert.ToInt32(egrd.DataKeys[gvr.RowIndex].Values[0]);
                                CurrPage = 0;
                                GetFieldValues(ControlsEnum.ROW);
                                SetFieldValues(ControlsEnum.ROW);
                                txtRowTitle.Focus();
                            }
                        }
                        break;

                    case ActionsEnum.DSBSELECTCELL:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.Parent.Parent as GridView;
                            if (grd != null)
                            {
                                CurrCell = Convert.ToInt32(grd.DataKeys[gvr.RowIndex].Values[0]);
                                CurrRow = 0;
                                GetFieldValues(ControlsEnum.CELL);
                                SetFieldValues(ControlsEnum.CELL);
                                txtDashletTitle.Focus();
                            }
                        }
                        break;

                    case ActionsEnum.DSBSELECTPARAMS:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            CurrParam = Convert.ToInt32(grdParameter.DataKeys[gvr.RowIndex].Values[0]);
                            GetFieldValues(ControlsEnum.CELLFILTER);
                            SetFieldValues(ControlsEnum.SELECTFILTER);
                            txtParameterName.Focus();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PAGE:
                    CurrPage = 0;
                    CurrRow = 0;
                    CurrCell = 0;
                    txtPageTitle.Text = string.Empty;
                    txtPageDescription.Text = string.Empty;
                    ddlNoOfRows.SelectedIndex = 0;
                    lblLastModifiedPageHDR.Text = string.Empty;
                    PageModifiedDatePnl.Visible = false;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    break;

                case ControlsEnum.ROW:
                    CurrPage = 0;
                    CurrRow = 0;
                    CurrCell = 0;
                    lblRowPage.Text = string.Empty;
                    lblRowPage.ToolTip = string.Empty;
                    txtRowTitle.Text = string.Empty;
                    txtRowDesc.Text = string.Empty;
                    ddlRowLayout.SelectedIndex = 0;
                    lblLastModifiedRowHDR.Text = string.Empty;
                    RowModifiedDatePnl.Visible = false;
                    break;

                case ControlsEnum.CELL:
                    CurrPage = 0;
                    CurrRow = 0;
                    CurrCell = 0;
                    lblCellPage.Text = string.Empty;
                    lblCellPage.ToolTip = string.Empty;
                    lblCellRow.Text = string.Empty;
                    lblCellRow.ToolTip = string.Empty;
                    txtDashletTitle.Text = string.Empty;
                    chkUseService.Checked = true;
                    txtDataSource.Text = string.Empty;
                    txtServiceMethod.Text = string.Empty;
                    ddlChartType.SelectedIndex = 0;
                    ddlChartSubType.Items.Clear();
                    ddlNoOfSeries.SelectedIndex = 0;
                    txtReportUrl.Text = string.Empty;
                    lblLastModifiedDashletHDR.Text = string.Empty;
                    DashletModifiedDatePnl.Visible = false;
                    lblLastModifiedPropertyHDR.Text = string.Empty;
                    PropertyModifiedDatePanel.Visible = false;

                    grdDashletProperties.DataSource = null;
                    grdDashletProperties.DataBind();

                    grdParameter.DataSource = null;
                    grdParameter.DataBind();
                    txtParameterName.Text = string.Empty;
                    txtParameterLabel.Text = string.Empty;
                    txtParameterDatasource.Text = string.Empty;
                    txtParameterMethod.Text = string.Empty;
                    txtParameterDescription.Text = string.Empty;
                    break;

                case ControlsEnum.CELLPROPS:
                    grdDashletProperties.DataSource = null;
                    grdDashletProperties.DataBind();
                    break;

                case ControlsEnum.CELLFILTER:
                    CurrParam = 0;
                    grdParameter.DataSource = null;
                    grdParameter.DataBind();
                    txtParameterName.Text = string.Empty;
                    txtParameterLabel.Text = string.Empty;
                    chkParameterUseService.Checked = true;
                    txtParameterDatasource.Text = string.Empty;
                    txtParameterMethod.Text = string.Empty;
                    txtParameterDescription.Text = string.Empty;
                    break;
            }

        }
        /// <summary>
        /// Used to set the Visibility of Dashlet Tab selected
        /// </summary>
        /// <param name="tab"></param>
        private void SetTab(TabsEnum tab)
        {
            try
            {
                switch (tab)
                {
                    case TabsEnum.NEWCELL:
                        lbnBasicDetails.Visible = true;
                        lbnBasicDetails.CssClass = Resources.Report.TabActive;
                        trCellBasicDetails.Visible = true;
                        lbnChartProps.Visible = false;
                        trCellProps.Visible = false;
                        lbnFilterParams.Visible = false;
                        trCellFilter.Visible = false;
                        break;
                    case TabsEnum.CELLBASIC:
                        lbnBasicDetails.Visible = true;
                        lbnBasicDetails.CssClass = Resources.Report.TabActive;
                        trCellBasicDetails.Visible = true;
                        lbnChartProps.Visible = true;
                        lbnChartProps.CssClass = Resources.Report.TabInactive;
                        trCellProps.Visible = false;
                        lbnFilterParams.Visible = true;
                        lbnFilterParams.CssClass = Resources.Report.TabInactive;
                        trCellFilter.Visible = false;
                        break;
                    case TabsEnum.CELLPROPS:
                        lbnBasicDetails.Visible = true;
                        lbnBasicDetails.CssClass = Resources.Report.TabInactive;
                        trCellBasicDetails.Visible = false;
                        lbnChartProps.Visible = true;
                        lbnChartProps.CssClass = Resources.Report.TabActive;
                        trCellProps.Visible = true;
                        lbnFilterParams.Visible = true;
                        lbnFilterParams.CssClass = Resources.Report.TabInactive;
                        trCellFilter.Visible = false;
                        break;
                    case TabsEnum.FILTERPARAMS:
                        lbnBasicDetails.Visible = true;
                        lbnBasicDetails.CssClass = Resources.Report.TabInactive;
                        trCellBasicDetails.Visible = false;
                        lbnChartProps.Visible = true;
                        lbnChartProps.CssClass = Resources.Report.TabInactive;
                        trCellProps.Visible = false;
                        lbnFilterParams.Visible = true;
                        lbnFilterParams.CssClass = Resources.Report.TabActive;
                        trCellFilter.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Used for binding Dashlet Property Value in Textbox
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string GetPropertyValue(object obj)
        {
            List<gAssetsData.DsbDshProprtyMpg> dsbDashletPropertyCfgList;
            try
            {
                if (obj != null)
                {
                    dsbDashletPropertyCfgList = (List<gAssetsData.DsbDshProprtyMpg>)obj;
                    return dsbDashletPropertyCfgList == null || dsbDashletPropertyCfgList.Count == 0 ? string.Empty :
                        dsbDashletPropertyCfgList[0].DshPrptValue;
                }
                else return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            DashBoardAdminServiceClient dashBoardAdminServiceClient;
            int result;
            GridViewRow gvr;
            ExtGridView grd;
            dashBoardAdminServiceClient = null;
            result = 0;
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
                    commonActions = ActionsEnum.DSBCHTTYPESELECTED;
                }
                switch (commonActions)
                {
                    #region SAVE PAGE
                    // Do Action for , when click save page button
                    case ActionsEnum.DSBSAVEPAGE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            dsbPageMstLst = new List<DsbPageMst>();
                            dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                            dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                            dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                            dsbPageMstObj = (DsbPageMst)SetUIValuesToObject(ControlsEnum.PAGE);
                            dsbPageMstLst.Add(dsbPageMstObj);
                            result = dashBoardAdminServiceClient.SaveDsbPageMst(dsbPageMstLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                dashBoardAdminServiceClient.Close();
                                ResetForm(ControlsEnum.PAGE);
                                SortBy = Resources.DataFieldRes.DsbPagePK;
                                SortDirection = Resources.Report.SortDescending;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnSavePage.Focus();
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbPage);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region New Page
                    case ActionsEnum.DSBADDPAGE:
                        PageModifiedDatePnl.Visible = false;
                        this.txtPageTitle.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPagePopup", "ShowAddPagePopup();", true);
                        break;
                    #endregion

                    #region Cancel Page
                    case ActionsEnum.DSBCANCELPAGE:
                        ResetForm(ControlsEnum.PAGE);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnSavePage.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        aupdpnlDashboardAdmin.Update();
                        break;
                    #endregion

                    #region Select Page
                    case ActionsEnum.DSBSELECTPAGE:
                        SetUIEditView(sender);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPagePopup", "ShowAddPagePopup();", true);
                        break;
                    #endregion

                    #region Delete Page
                    case ActionsEnum.DSBDELETEPAGE:
                        // Delete Page By PK - Return 1 is Success , 0- Fail                                               
                        dsbPageMstLst = new List<DsbPageMst>();
                        dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                        dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                        dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                        dsbPageMstObj.PagePk = CurrPage;
                        dsbPageMstObj.PageModOn = LastModifiedTime;
                        dsbPageMstLst.Add(dsbPageMstObj);
                        result = dashBoardAdminServiceClient.DeleteDsbPageMst(dsbPageMstLst);
                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            dashBoardAdminServiceClient.Close();
                            ResetForm(ControlsEnum.PAGE);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            btnSavePage.Focus();
                            litErrorMsg.Text = Resources.Report.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbPage);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region Save Row
                    // Do Action for , when click save row button
                    case ActionsEnum.DSBSAVEROW:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            dsbRowMstLst = new List<DsbRowMst>();
                            dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                            dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                            dsbRowMstObj = dashBoardAdminServiceClient.GetInitilizedDsbRowMst();
                            dsbRowMstObj = (DsbRowMst)SetUIValuesToObject(ControlsEnum.ROW);
                            dsbRowMstLst.Add(dsbRowMstObj);
                            result = dashBoardAdminServiceClient.SaveDsbRowMst(dsbRowMstLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                dashBoardAdminServiceClient.Close();
                                ResetForm(ControlsEnum.ROW);
                                SortBy = Resources.DataFieldRes.DsbPagePK;
                                SortDirection = Resources.Report.SortDescending;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnSavePage.Focus();
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbRow);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region New Row
                    case ActionsEnum.DSBADDROW:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            CurrPage = Convert.ToInt32(grdPageHierarchy.DataKeys[gvr.RowIndex].Values[0]);
                            GetFieldValues(ControlsEnum.PAGE);
                            if (dsbPageMstLst != null && dsbPageMstLst.Count > 0)
                            {
                                lblRowPage.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dsbPageMstLst[0].PageTitle), 25);
                                lblRowPage.ToolTip = HttpUtility.HtmlDecode(dsbPageMstLst[0].PageTitle);
                            }
                            RowModifiedDatePnl.Visible = false;
                            this.txtRowTitle.Focus();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowRowPopup", "ClearSetRowPopup(true);", true);
                        }
                        break;
                    #endregion

                    #region Cancel Row
                    case ActionsEnum.DSBCANCELROW:
                        ResetForm(ControlsEnum.ROW);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnSavePage.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region Select Row
                    case ActionsEnum.DSBSELECTROW:
                        SetUIEditView(sender);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowRowPopup", "ShowAddRowPopup();", true);
                        break;
                    #endregion

                    #region Delete Row
                    case ActionsEnum.DSBDELETEROW:
                        // Delete Row By PK - Return 1 is Success , 0- Fail                                               
                        dsbRowMstLst = new List<DsbRowMst>();
                        dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                        dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                        dsbRowMstObj = dashBoardAdminServiceClient.GetInitilizedDsbRowMst();
                        dsbRowMstObj.RowPk = CurrRow;
                        dsbRowMstObj.RowModOn = LastModifiedTime;
                        dsbRowMstLst.Add(dsbRowMstObj);
                        result = dashBoardAdminServiceClient.DeleteDsbRowMst(dsbRowMstLst);
                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            dashBoardAdminServiceClient.Close();
                            ResetForm(ControlsEnum.ROW);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            btnSavePage.Focus();
                            litErrorMsg.Text = Resources.Report.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbRow);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region Select Chart Type
                    case ActionsEnum.DSBCHTTYPESELECTED:
                        if (ddlChartType.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            GetFieldValues(ControlsEnum.CHTSUBTYPE);
                            SetFieldValues(ControlsEnum.CHTSUBTYPE);
                            ddlChartSubType.Focus();
                        }
                        else
                        {
                            ddlChartSubType.Items.Clear();
                            ddlChartSubType.DataSource = null;
                            ddlChartSubType.DataBind();
                            ddlChartType.Focus();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Save Cell
                    case ActionsEnum.DSBSAVECELL:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            dsbDashletMstLst = new List<DsbDashletMst>();
                            dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                            dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                            dsbDashletMstObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletMst();
                            dsbDashletMstObj = (DsbDashletMst)SetUIValuesToObject(ControlsEnum.CELL);
                            dsbDashletMstLst.Add(dsbDashletMstObj);
                            result = dashBoardAdminServiceClient.SaveDsbDashletMst(dsbDashletMstLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                dashBoardAdminServiceClient.Close();
                                ResetForm(ControlsEnum.CELL);
                                SortBy = Resources.DataFieldRes.DsbPagePK;
                                SortDirection = Resources.Report.SortDescending;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                CurrCell = result;
                                CurrRow = 0;
                                GetFieldValues(ControlsEnum.CELL);
                                SetFieldValues(ControlsEnum.CELL);
                                txtDashletTitle.Focus();
                                SetTab(TabsEnum.CELLBASIC);
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbCell);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region New Cell
                    case ActionsEnum.DSBADDCELL:
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.Parent.Parent as ExtGridView;
                            CurrPage = 0;
                            CurrRow = Convert.ToInt32(grd.DataKeys[gvr.RowIndex].Values[0]);
                            GetFieldValues(ControlsEnum.ROW);
                            if (dsbRowMstLst != null && dsbRowMstLst.Count > 0)
                            {
                                lblCellRow.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dsbRowMstLst[0].RowTitle), 25);
                                lblCellRow.ToolTip = HttpUtility.HtmlDecode(dsbRowMstLst[0].RowTitle);
                                lblCellPage.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dsbRowMstLst[0].DsbPageMst.PageTitle), 25);
                                lblCellPage.ToolTip = HttpUtility.HtmlDecode(dsbRowMstLst[0].DsbPageMst.PageTitle);
                            }
                            DashletModifiedDatePnl.Visible = false;
                            this.txtDashletTitle.Focus();
                            SetTab(TabsEnum.NEWCELL);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ClearSetDashletPopup(true);", true);
                        }
                        break;
                    #endregion

                    #region Cancel Cell
                    case ActionsEnum.DSBCANCELCELL:
                        ResetForm(ControlsEnum.CELL);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnSavePage.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region Select Cell
                    case ActionsEnum.DSBSELECTCELL:
                        SetUIEditView(sender);
                        SetTab(TabsEnum.CELLBASIC);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Delete Cell
                    case ActionsEnum.DSBDELETECELL:
                        // Delete Cell By PK - Return 1 is Success , 0- Fail                                               
                        dsbDashletMstLst = new List<DsbDashletMst>();
                        dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                        dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                        dsbDashletMstObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletMst();
                        dsbDashletMstObj.DashletPk = CurrCell;
                        dsbDashletMstObj.DashletModOn = LastModifiedTime;
                        dsbDashletMstLst.Add(dsbDashletMstObj);
                        result = dashBoardAdminServiceClient.DeleteDsbDashletMst(dsbDashletMstLst);
                        if (result > 0)
                        {
                            dashBoardAdminServiceClient.Close();
                            ResetForm(ControlsEnum.CELL);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            btnSavePage.Focus();
                            litErrorMsg.Text = Resources.Report.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbCell);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region Save Dashlet Properties
                    case ActionsEnum.DSBSAVECELLPROPS:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                            dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                            dsbDshProprtyMpgObj = dashBoardAdminServiceClient.GetInitilizedDsbDshProprtyMpg();
                            dsbDashletMstObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletMst();
                            dsbDshProprtyMpgLst = (List<DsbDshProprtyMpg>)SetUIValuesToObject(ControlsEnum.CELLPROPS);
                            result = dashBoardAdminServiceClient.SaveDsbDshProprtyMpg(dsbDshProprtyMpgLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                dashBoardAdminServiceClient.Close();
                                ResetForm(ControlsEnum.CELLPROPS);
                                GetFieldValues(ControlsEnum.CELL);
                                SetFieldValues(ControlsEnum.CELL);
                                GetFieldValues(ControlsEnum.CHARTPROPS);
                                SetFieldValues(ControlsEnum.CHARTPROPS);
                                //GetFieldValues(ControlsEnum.CELLPROPS);
                                //SetFieldValues(ControlsEnum.CELLPROPS);
                                btnSaveCellProps.Focus();
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbProperties);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Cancel Dashlet Properties
                    case ActionsEnum.DSBCANCELCELLPROPS:
                        ResetForm(ControlsEnum.CELLPROPS);
                        CurrRow = 0;
                        GetFieldValues(ControlsEnum.CELL);
                        SetFieldValues(ControlsEnum.CELL);
                        txtDashletTitle.Focus();
                        SetTab(TabsEnum.CELLBASIC);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Save Fliter Parameters
                    case ActionsEnum.DSBADDPARAMS:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            dsbFilterParameterMstLst = new List<DsbFilterParameterMst>();
                            dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                            dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                            dsbFilterParameterMstObj = dashBoardAdminServiceClient.GetInitilizedDsbFilterParameterMst();
                            dsbFilterParameterMstObj = (DsbFilterParameterMst)SetUIValuesToObject(ControlsEnum.CELLFILTER);
                            dsbFilterParameterMstLst.Add(dsbFilterParameterMstObj);
                            result = dashBoardAdminServiceClient.SaveDsbFilterParameterMst(dsbFilterParameterMstLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {

                                dashBoardAdminServiceClient.Close();
                                ResetForm(ControlsEnum.CELLFILTER);
                                GetFieldValues(ControlsEnum.CELLFILTER);
                                SetFieldValues(ControlsEnum.CELLFILTER);
                                btnDeleteParams.Visible = false;
                                txtParameterName.Focus();
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbFilterParams);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Cancel Filter Parameters
                    case ActionsEnum.DSBCANCELPARAMS:
                        ResetForm(ControlsEnum.CELLFILTER);
                        GetFieldValues(ControlsEnum.CELLFILTER);
                        SetFieldValues(ControlsEnum.CELLFILTER);
                        btnDeleteParams.Visible = false;
                        txtParameterName.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Select Filter Parameters
                    case ActionsEnum.DSBSELECTPARAMS:
                        SetUIEditView(sender);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Delete Filter Parameters
                    case ActionsEnum.DSBDELETEPARAMS:
                        // Delete Parameter By PK - Return 1 is Success , 0- Fail                                               
                        dsbFilterParameterMstLst = new List<DsbFilterParameterMst>();
                        dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                        dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                        dsbFilterParameterMstObj = dashBoardAdminServiceClient.GetInitilizedDsbFilterParameterMst();
                        dsbFilterParameterMstObj.ParamPk = CurrParam;
                        dsbFilterParameterMstLst.Add(dsbFilterParameterMstObj);
                        result = dashBoardAdminServiceClient.DeleteDsbFilterParameterMst(dsbFilterParameterMstLst);
                        if (result > 0)
                        {
                            dashBoardAdminServiceClient.Close();
                            ResetForm(ControlsEnum.CELLFILTER);
                            GetFieldValues(ControlsEnum.CELLFILTER);
                            SetFieldValues(ControlsEnum.CELLFILTER);
                            btnDeleteParams.Visible = false;
                            txtParameterName.Focus();
                            litErrorMsg.Text = Resources.Report.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DsbFilterParams);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region Dashlet Basic Details Tab
                    case ActionsEnum.DSBCELLBASIC:
                        CurrRow = 0;
                        GetFieldValues(ControlsEnum.CELL);
                        SetFieldValues(ControlsEnum.CELL);
                        txtDashletTitle.Focus();
                        SetTab(TabsEnum.CELLBASIC);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Dashlet Properties Tab
                    case ActionsEnum.DSBCELLPROPERTIES:
                        GetFieldValues(ControlsEnum.CELL);
                        SetFieldValues(ControlsEnum.CELL);
                        GetFieldValues(ControlsEnum.CHARTPROPS);
                        SetFieldValues(ControlsEnum.CHARTPROPS);
                        //GetFieldValues(ControlsEnum.CELLPROPS);
                        //SetFieldValues(ControlsEnum.CELLPROPS);
                        btnSaveCellProps.Focus();
                        SetTab(TabsEnum.CELLPROPS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion

                    #region Dashlet Filter Parameters Tab
                    case ActionsEnum.DSBCELLFILTERPARAMS:
                        ResetForm(ControlsEnum.CELLFILTER);
                        GetFieldValues(ControlsEnum.CELLFILTER);
                        SetFieldValues(ControlsEnum.CELLFILTER);
                        txtParameterName.Focus();
                        SetTab(TabsEnum.FILTERPARAMS);
                        btnDeleteParams.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (commonActions == ActionsEnum.DSBSAVEPAGE || commonActions == ActionsEnum.DSBDELETEPAGE)
                {
                    dashBoardAdminServiceClient.Abort();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPagePopup", "ShowAddPagePopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.PageNameRes.DsbPage + " " + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else if (commonActions == ActionsEnum.DSBSAVEROW || commonActions == ActionsEnum.DSBDELETEROW)
                {
                    dashBoardAdminServiceClient.Abort();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowRowPopup", "ShowAddRowPopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.PageNameRes.DsbRow + " " + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else if (commonActions == ActionsEnum.DSBSAVECELL || commonActions == ActionsEnum.DSBDELETECELL)
                {
                    dashBoardAdminServiceClient.Abort();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.PageNameRes.DsbCell + " " + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else if (commonActions == ActionsEnum.DSBSAVECELLPROPS)
                {
                    dashBoardAdminServiceClient.Abort();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.PageNameRes.DsbProperties + " " + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else if (commonActions == ActionsEnum.DSBADDPARAMS || commonActions == ActionsEnum.DSBDELETEPARAMS)
                {
                    dashBoardAdminServiceClient.Abort();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.PageNameRes.DsbFilterParams + " " + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else if (commonActions == ActionsEnum.DSBCELLBASIC || commonActions == ActionsEnum.DSBCELLPROPERTIES || commonActions == ActionsEnum.DSBCELLFILTERPARAMS)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCellPopup", "ShowAddDashletPopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
            finally
            {
                dsbPageMstLst = null;
                dsbPageMstObj = null;
                dsbRowMstLst = null;
                dsbRowMstObj = null;
                dsbDashletMstLst = null;
                dsbDashletMstObj = null;
                dsbDshProprtyMpgLst = null;
                dsbDshProprtyMpgObj = null;
                dsbFilterParameterMstLst = null;
                dsbFilterParameterMstObj = null;
                dashBoardAdminServiceClient = null;
            }
        }
        /// <summary>
        /// Method used to Handle the row created event of GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            GridView grd;
            ExtGridView grdHierarchy;
            string senderId;
            try
            {
                senderId = (sender as ExtGridView).ID;
                if (senderId == "grdPageHierarchy")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        grdHierarchy = e.Row.FindControl("grdRowHierarchy") as ExtGridView;
                        dsbPageMstObj = (DsbPageMst)e.Row.DataItem;
                        if (dsbPageMstObj != null && dsbPageMstObj.DsbRowMsts != null)
                        {
                            grdHierarchy.DataSource = dsbPageMstObj.DsbRowMsts;
                        }
                    }
                }
                else if (senderId == "grdRowHierarchy")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        grd = e.Row.FindControl("grdDashlet") as GridView;
                        dsbRowMstObj = (DsbRowMst)e.Row.DataItem;
                        if (dsbRowMstObj != null && dsbRowMstObj.DsbDashletMsts != null)
                        {
                            grd.DataSource = dsbRowMstObj.DsbDashletMsts;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (SortBy == e.SortExpression)
                {
                    //Toggle the sort expression
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new FirstPageEventHandler(this.FirstPage);
            this.uclPaging.PreviousPage += new PreviousPageEventHandler(this.PreviousPage);
            this.uclPaging.NextPage += new NextPageEventHandler(this.NextPage);
            this.uclPaging.LastPage += new LastPageEventHandler(this.LastPage);
            this.Init += new EventHandler(this.Page_Init);
        }

        protected void FirstPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage > 1)
            {
                uclPaging.CurrentPage = 1;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void PreviousPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage > 1)
            {
                uclPaging.CurrentPage--;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void NextPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage <= e.TotalPages)
            {
                uclPaging.CurrentPage++;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void LastPage(object sender, DataNavigatorEventArgs e)
        {
            if (e.CurrentPage <= e.TotalPages)
            {
                uclPaging.CurrentPage = e.TotalPages;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            this.Page.Title = this.GetLocalResourceObject("PageTitle").ToString();
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
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.CHARTTYPE);
                    SetFieldValues(ControlsEnum.CHARTTYPE);
                    btnSavePage.Focus();
                    PageIndex = "1";
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    this.Page.Title = this.GetLocalResourceObject("PageTitle").ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        /// <summary>
        /// Controls Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            PAGE,
            ROW,
            CELL,
            CHARTPROPS,
            CELLPROPS,
            CELLFILTER,
            SELECTFILTER,
            CHARTTYPE,
            CHTSUBTYPE,
        }
        #endregion

        #region Dashlet Tabs Enum
        /// <summary>
        /// Dashlet Tabs Enum
        /// </summary>
        private enum TabsEnum
        {
            NEWCELL,
            CELLBASIC,
            CELLPROPS,
            FILTERPARAMS
        }
        #endregion
    }
}