using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.HRMS.Employee;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using BusinessLogic.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using GTIService.Constants.HRMS.Admin.Masters;
using DataAccess.HRMS.Admin.Masters;

namespace HRMS.Admin.Masters
{
    public partial class PayElementsMaster : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
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

        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
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
        private DataTable dtResult;
        private BusinessObject.User currentUser;
        PayElementsMasterBO objPayElements;
        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            dtResult = null;
            try
            {
                switch (type)
                {
                    #region Pay Classification
                    case ControlsEnum.PAYCLASSIFICATION:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("ClassificationType").ToString());
                        break;
                    #endregion
                    #region Parent Element
                    case ControlsEnum.PARENTELEMENT:
                        dtResult = PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, CurrPK);
                        break;
                    #endregion
                    #region COA
                    //case ControlsEnum.COA:
                    //    dtResult = CommonBL.GetAccountType(0, Convert.ToInt32(GetLocalResourceObject("COAsubType")), 1);
                    //    break;
                    #endregion
                    #region Listing Page
                    case ControlsEnum.LIST:
                        BusinessObject.GridPrams gridParam;
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = (PageIndex == 0) ? 1 : PageIndex;
                        gridParam.PageSize = PageSize;                        
                        dtResult = PayElementsMasterBL.GetParentElementList(gridParam, currentUser.SBUID, txtPayElementCode.Text.Trim(), txtPayElementName.Text.Trim(),
                            txtPayElementClass.Text.Trim(), null,Convert.ToString(GetLocalResourceObject("SortOderBy")));
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        dtResult = PayElementsMasterBL.GetParentElement(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region Pay Type
                    case ControlsEnum.PAYTYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("PayElementType").ToString());
                        break;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Pay Classification
                    case ControlsEnum.PAYCLASSIFICATION:
                        BindDropDown(ControlsEnum.PAYCLASSIFICATION);
                        break;
                    #endregion
                    #region Parent Element
                    case ControlsEnum.PARENTELEMENT:
                        BindDropDown(ControlsEnum.PARENTELEMENT);
                        break;
                    #endregion
                    #region COA
                    //case ControlsEnum.COA:
                    //    BindDropDown(ControlsEnum.COA);
                    //    break;
                    #endregion
                    #region Listing Page
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    #endregion
                    #region Pay Type
                    case ControlsEnum.PAYTYPE:
                        BindDropDown(ControlsEnum.PAYTYPE);
                        break;
                    #endregion

                    case ControlsEnum.FORMULAPREFIX:
                        lblFormulaPrefix.Text = hdfFormulaPrefix.Value;
                        lblFormulaSufix.Text = new string(hdfFormulaPrefix.Value.ToCharArray().Reverse().ToArray());
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

        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("HRMS FORMULA PREFIX", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfFormulaPrefix.Value = dt.Rows[0]["ACF_DATA"].ToString();//To define the Payelement suffix & prefix constant for formula                         
            }
        }


        /// <summary>
        /// for checking radio button selected in main grid
        /// </summary>
        /// <param name="GridRowIndex"></param>
        /// <returns></returns>
        private bool IsRadioButtonSelected(ref int GridRowIndex)
        {
            RadioButton rbtn;
            foreach (GridViewRow grdrow in grdList.Rows)
            {
                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                if (rbtn.Checked)
                {
                    GridRowIndex = grdrow.RowIndex;
                    return true;
                }
            }
            return GridRowIndex == 0 ? false : true;
        }
        private object SetUIValuesToObject(ControlsEnum ControlType)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.SAVE:
                        objPayElements = new PayElementsMasterBO();
                        objPayElements.PayElmntPk = CurrPK;
                        //objPayElements.AccountCode = ddlCOA.SelectedValue == CommonConstants.SELECTVAL ? null : ddlCOA.SelectedValue;
                        if (string.IsNullOrEmpty(txtCOA.Text.Trim()) || txtCOA.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                            hdfCOA.Value = string.Empty;
                        objPayElements.AccountCode = string.IsNullOrEmpty(hdfCOA.Value) ? null : hdfCOA.Value;
                        objPayElements.CTC = chkPartofCTC.Checked == true ? 1 : 0;
                        objPayElements.IsEditable = chkEditable.Checked == true ? 1 : 0;
                        objPayElements.PartofGross = chkPartofGross.Checked == true ? 1 : 0;
                        objPayElements.EffectiveFrom = txtEffectiveFrom.Text == string.Empty ? null : txtEffectiveFrom.Text;
                        objPayElements.EffectiveTo = txtEffectiveTo.Text == string.Empty ? null : txtEffectiveTo.Text;
                        //objPayElements.isTaxable = chkTaxable.Checked == true ? 1 : 0;
                        objPayElements.Parent = ddlParentElement.SelectedValue == CommonConstants.SELECTVAL ? null : ddlParentElement.SelectedValue;
                        objPayElements.PayElmntClass = ddlClassification.SelectedValue == CommonConstants.SELECTVAL ? null : ddlClassification.SelectedValue;
                        objPayElements.PayElmntCode = txtCode.Text;
                        objPayElements.PayElmntName = txtElementDesp.Text;
                        objPayElements.P_PEL_CODE_LL = txtCodeLL.Text;
                        objPayElements.P_PEL_NAME_LL = txtElementDespLL.Text;
                        objPayElements.PaySlip = chkAppearsinPayslip.Checked == true ? 1 : 0;
                        //objPayElements.Recurring = chkRecurring.Checked == true ? 1 : 0;
                        objPayElements.Active = chkActive.Checked == true ? 1 : 0;
                        //objPayElements.isDeduct = chkIsDeduct.Checked == true ? 1 : 0;
                        objPayElements.IncludeInSalary = chkIncludeInSalary.Checked ? 1 : 0;
                        //objPayElements.FormulaCode = string.IsNullOrEmpty(txtFormula.Text) ? string.Empty : string.Format("{0}" + txtFormula.Text + "{0}", GetGlobalResourceObject("ConfigurationsRes", "PayElementFormulaConst").ToString());
                        //objPayElements.FormulaCode = string.IsNullOrEmpty(txtFormula.Text) ? string.Empty : lblFormulaPrefix.Text + txtFormula.Text + lblFormulaSufix.Text;
                        objPayElements.FormulaCode = txtFormula.Text.Trim();
                        objPayElements.UserPk = currentUser.PKUser;
                        objPayElements.BizUnit = currentUser.SBUID;
                        objPayElements.LastModDate = LastModifiedTime;
                        objPayElements.PayType = ddlType.SelectedValue == CommonConstants.SELECTVAL ? null : ddlType.SelectedValue;
                        objPayElements.PayElmntDesc = txtDescription.Text == string.Empty ? null : txtDescription.Text.HtmlEncode(); 
                        //Formula Editable field added
                        objPayElements.FormulaEditable = chkFormulaEditable.Checked == true ? 1 : 0;
                        objPayElements.RoundoffRequired = chkRoundoff.Checked == true ? 1 : 0;
                        objPayElements.Sequence = Convert.ToInt16(txtSeqnc.Text);
                        objPayElements.ShowReport = chkShwRpt.Checked == true ? 1 : 0;
                        objPayElements.ShowReport1 = chkShwRpt1.Checked == true ? 1 : 0;
                        objPayElements.ShowInEmp = chkShwEmpMasterSalTemp.Checked == true ? 1 : 0;
                        returnObj = objPayElements;
                        break;
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EDIT:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCode.Text = dtResult.Rows[0][Fields.F_PEL_CODE].ToString();
                            txtElementDesp.Text = dtResult.Rows[0][Fields.F_PEL_NAME].ToString();
                            txtCodeLL.Text =Convert.ToString( dtResult.Rows[0][Fields.F_PEL_CODE_LL]);
                            txtElementDespLL.Text = Convert.ToString( dtResult.Rows[0][Fields.F_PEL_NAME_LL]);
                            txtDescription.Text = dtResult.Rows[0][Fields.F_PEL_DESC].ToString().HtmlDecode();
                            ddlClassification.SelectedValue = dtResult.Rows[0][Fields.F_PEL_CLASS].ToString();
                            if (Convert.ToInt32(ddlClassification.SelectedValue) == (int)PayClassifications.BasicPay)
                            {
                                ddlType.Enabled = false;                                
                            }
                            else
                            {
                                ddlType.Enabled = true;
                            }
                            if (!string.IsNullOrEmpty(dtResult.Rows[0][Fields.F_PEL_PARENT].ToString()))
                                ddlParentElement.SelectedValue = dtResult.Rows[0][Fields.F_PEL_PARENT].ToString();
                            if (!string.IsNullOrEmpty(dtResult.Rows[0][Fields.F_PEL_EFFECT_FROM].ToString()))
                                txtEffectiveFrom.Text = Convert.ToDateTime((dtResult.Rows[0][Fields.F_PEL_EFFECT_FROM])).ToString(Resources.Constants.HRMSDateFormatShort);
                            if (!string.IsNullOrEmpty(dtResult.Rows[0][Fields.F_PEL_EFFECT_TO].ToString()))
                                txtEffectiveTo.Text = Convert.ToDateTime((dtResult.Rows[0][Fields.F_PEL_EFFECT_TO])).ToString(Resources.Constants.HRMSDateFormatShort);
                            if (!string.IsNullOrEmpty(dtResult.Rows[0][Fields.F_PEL_ACCOUNT].ToString()))
                            {
                                //ddlCOA.SelectedValue = dtResult.Rows[0][Fields.F_PEL_ACCOUNT].ToString();
                                hdfCOA.Value = dtResult.Rows[0][Fields.F_PEL_ACCOUNT].ToString();
                                txtCOA.Text = HttpUtility.HtmlDecode(dtResult.Rows[0][Fields.F_PEL_ACCOUNT_TEXT].ToString());
                            }
                            chkAppearsinPayslip.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IN_PAY_SLIP]);
                            //chkRecurring.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_RECURRING]);
                            chkPartofCTC.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IN_CTC]);
                            chkEditable.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IS_EDITABLE]);
                            chkPartofGross.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IN_GROSS]);
                            //chkTaxable.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_TAXABLE]);
                            chkActive.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_ACTIVE]);
                            //chkIsDeduct.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IS_DEDUCTION]);
                            chkIncludeInSalary.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IN_SALARY]);
                            txtFormula.Text = dtResult.Rows[0][Fields.F_PEL_FORMULA_BODY].ToString();      //RemoveFormulaConstant(dtResult.Rows[0][Fields.F_PEL_FORMULA_CODE].ToString());
                            txtFormula.Enabled = Convert.ToInt32(dtResult.Rows[0][Fields.F_PEL_USD_IN_FMLA]) == 1 ? false : true;
                            LastModifiedTime = Convert.ToDateTime(dtResult.Rows[0][Fields.F_PEL_MOD_DT].ToString());
                            ddlType.SelectedValue = dtResult.Rows[0][Fields.F_PEL_CALC_MODE].ToString();
                            if (Convert.ToInt32(dtResult.Rows[0][Fields.F_PEL_IS_USED]) > 0)
                                ddlClassification.Enabled = false;
                            else
                                ddlClassification.Enabled = true;
                            chkFormulaEditable.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_IS_FORMULA_EDIT]);
                            chkRoundoff.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_ROUND_OFF]);
                            txtSeqnc.Text = dtResult.Rows[0][Fields.F_PEL_SEQUENCE].ToString();
                            chkShwRpt.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_SHOW_IN_REPORT]);
                            chkShwRpt1.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_SHOW_IN_REPORT1]);
                            if (!string.IsNullOrEmpty(dtResult.Rows[0][Fields.F_PEL_SHOW_IN_EMPMAST].ToString()))
                                chkShwEmpMasterSalTemp.Checked = Convert.ToBoolean(dtResult.Rows[0][Fields.F_PEL_SHOW_IN_EMPMAST]);
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
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dtResult.Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string RemoveFormulaConstant(string formula)
        {
            string slimmed = string.Empty;
            var str = formula;
            // Number of characters to remove on each end
            //var n = GetGlobalResourceObject("ConfigurationsRes", "PayElementFormulaConst").ToString().Length;
            var n = hdfFormulaPrefix.Value.Length;
            if (str.Length > n * 2)
                slimmed = str.Substring(n, str.Length - (n * 2));
            else
                slimmed = string.Empty;
            return slimmed;
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// for bind drop downs
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Pay Classification
                case ControlsEnum.PAYCLASSIFICATION:
                    ddlClassification.Items.Clear();
                    ddlClassification.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlClassification.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                    ddlClassification.DataSource = dtResult;
                    ddlClassification.DataBind();
                    ddlClassification.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Parent Element
                case ControlsEnum.PARENTELEMENT:
                    ddlParentElement.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlParentElement.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, Fields.F_PEL_NAME);
                        ddlParentElement.DataTextField = Fields.F_PEL_NAME;
                        ddlParentElement.DataValueField = Fields.F_PEL_PK;
                        ddlParentElement.DataBind();
                    }
                    ddlParentElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region COA
                //case ControlsEnum.COA:
                //    ddlCOA.Items.Clear();
                //    ddlCOA.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.HRMS.Admin.Masters.Fields.F_COA_NAME_TEXT);
                //    ddlCOA.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_COA_NAME_TEXT;
                //    ddlCOA.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_COA_PK;
                //    ddlCOA.DataBind();
                //    ddlCOA.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                //    break;
                #endregion
                #region Pay Type
                case ControlsEnum.PAYTYPE:
                    ddlType.Items.Clear();
                    ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                    ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                    ddlType.DataBind();
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
            }
        }
        /// <summary>
        /// for clear controls
        /// </summary>
        /// <param name="controlType"></param>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CANCEL:
                    break;
                case ControlsEnum.CLEAR:
                    dtResult = null;
                    CurrPK = 0;
                    txtCode.Text = string.Empty;
                    txtCodeLL.Text = string.Empty;
                    txtElementDespLL.Text = string.Empty;
                    txtEffectiveFrom.Text = string.Empty;
                    txtEffectiveTo.Text = string.Empty;
                    txtElementDesp.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    if (ddlClassification.Items.Count > 0)
                        ddlClassification.SelectedIndex = 0;
                    //if (ddlCOA.Items.Count > 0)
                    //    ddlCOA.SelectedIndex = 0;
                    txtCOA.Text = hdfCOA.Value = string.Empty;
                    if (ddlType.Items.Count > 0)
                        ddlType.SelectedIndex = 0;
                    chkActive.Checked = false;
                    //chkIsDeduct.Checked = false;
                    chkIncludeInSalary.Checked = true;
                    txtFormula.Text = string.Empty;
                    chkAppearsinPayslip.Checked = false;
                    chkPartofCTC.Checked = false;
                    chkFormulaEditable.Checked = false;
                    chkEditable.Checked = false;
                    chkPartofGross.Checked = false;
                    //chkFormulaEditable.Checked = false;
                    //chkRecurring.Checked = false;
                    //chkTaxable.Checked = false;
                    txtSeqnc.Text = "1";
                    chkShwRpt.Checked = false;
                    chkShwRpt1.Checked = false;
                    chkShwEmpMasterSalTemp.Checked = false;
                    break;
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    CurrPK = 0;                  
                    txtPayElementClass.Text = string.Empty;
                    txtPayElementCode.Text = string.Empty;
                    txtPayElementName.Text = string.Empty;
                    //ddlFilterBy.ClearSelection();
                    break;
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
            try
            {
                int GridRowIndex = 0;
                int? result;
                GridViewRow gvrPayElement;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    //  commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlClassification")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                    // commonActions = ActionsEnum.TOOLTIP;  
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            objPayElements = (PayElementsMasterBO)SetUIValuesToObject(ControlsEnum.SAVE);
                            if (objPayElements != null)
                            {
                                result = PayElementsMasterBL.SavePayElements(objPayElements);
                                if (result >= 0)
                                {
                                    ResetForm(ControlsEnum.CLEAR);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    btnNew.Focus();
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PayElements").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    #region Error Messages
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("PayElements").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Name_Already_Exists, GetLocalResourceObject("PayElements").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Code_Already_Exists, GetLocalResourceObject("PayElements").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.INCORRECT)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_PayElementsKeyBaseExists").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_FormulaCodeExists").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PayElements").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        txtCode.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        SetFieldValues(ControlsEnum.FORMULAPREFIX);
                        ResetForm(ControlsEnum.CLEAR);
                        chkActive.Checked = true;
                        txtFormula.Enabled = true;
                        ddlClassification.Enabled = true;
                        //For get and set parent element ddl
                        GetFieldValues(ControlsEnum.PARENTELEMENT);
                        SetFieldValues(ControlsEnum.PARENTELEMENT);
                        chkShwRpt.Checked = chkShwRpt1.Checked = true;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        if (IsRadioButtonSelected(ref GridRowIndex))
                        {
                            txtCode.Focus();
                            SetFieldValues(ControlsEnum.FORMULAPREFIX);
                            CurrPK = Convert.ToInt32((grdList.Rows[GridRowIndex].FindControl("hdfPayElemPk") as HiddenField).Value);
                            //For get and set parent element ddl
                            GetFieldValues(ControlsEnum.PARENTELEMENT);
                            SetFieldValues(ControlsEnum.PARENTELEMENT);
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        result = PayElementsMasterBL.DeletePayElement(CurrPK, this.LastModifiedTime);
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("PayElements").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            #region Error Message
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("PayElements").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("PayElements").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("PayElements").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("PayElements").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;

                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region ACTIVATE
                    case ActionsEnum.ACTIVATE:
                        gvrPayElement = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = PayElementsMasterBL.UpdatePayElementStatus(Convert.ToInt32(((HiddenField)gvrPayElement.FindControl("hdfPayElemPk")).Value), (int)DbActiveStatus.ACTIVE, currentUser.PKUser, null);
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndex = 1;
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayElement.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region DEACTIVATE
                    case ActionsEnum.DEACTIVATE:
                        gvrPayElement = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = PayElementsMasterBL.UpdatePayElementStatus(Convert.ToInt32(((HiddenField)gvrPayElement.FindControl("hdfPayElemPk")).Value), (int)DbActiveStatus.INACTIVE, currentUser.PKUser, null);
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndex = 1;
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayElement.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        if (Convert.ToInt32(ddlClassification.SelectedValue) == (int)PayClassifications.BasicPay)
                        {
                            ddlType.Enabled = false;
                            ddlType.SelectedValue = ((int)PayElementCalcMode.FixedAmount).ToString();
                        }
                        else
                        {
                            ddlType.Enabled = true;
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex)
                                           + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {

            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            EntryStatus = EntryStatus.LISTMODE;
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
                this.PageIndex = 1;
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

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
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EnableDisableButtons(e.TotalPages, "uclPaging");
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            }
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
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
                    ConfigurationSettings();
                    PageIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.CurrentPage = PageIndex;
                    //For get and set listing grid
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    //For get and set pay classificatuion ddl
                    GetFieldValues(ControlsEnum.PAYCLASSIFICATION);
                    SetFieldValues(ControlsEnum.PAYCLASSIFICATION);
                    //For get and set parent COA ddl
                    //GetFieldValues(ControlsEnum.COA);
                    //SetFieldValues(ControlsEnum.COA);
                    //For get and set Pay Type
                    GetFieldValues(ControlsEnum.PAYTYPE);
                    SetFieldValues(ControlsEnum.PAYTYPE);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EDIT,
            LIST,
            CANCEL,
            CLEAR,
            PARENTELEMENT,
            COA,
            PAYCLASSIFICATION,
            SAVE,
            PAYTYPE,
            CLEARSEARCH,
            FORMULAPREFIX
        }
        #endregion
    }
}