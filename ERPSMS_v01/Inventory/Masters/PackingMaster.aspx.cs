using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.Inventory;
using BusinessLogic.Inventory;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class PackingMaster : ERP.Store.UI.MyBasePage
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
        private List<ADM_CONST_MST> admConstMstList;
        private DataTable dtControlsData;
        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private DataSet dsPageData;
        private PackingMasterBO PackMasterBO;

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
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        SortBy = SortBy == null ? Resources.DataFieldRes.PackingMstPK : SortBy;
                        SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        dsPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(CurrPK, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID,
                            ddlFilterBy.SelectedItem.Text == Resources.Controls.PackingSpecification ? txtSearchBy.Text : string.Empty,
                            ddlFilterBy.SelectedItem.Text == Resources.Controls.PackingType ? txtSearchBy.Text : string.Empty,
                            ddlFilterBy.SelectedItem.Text == Resources.Controls.PackingCode ? txtSearchBy.Text : string.Empty);
                        Session[ERP.Utilities.SessionStrings.PackingPk] = null;
                        Session[ERP.Utilities.SessionStrings.PackingMode] = null;
                        break;
                    case ControlsEnum.PACKINGTYPE:
                        CommonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.Packing, ERP.Utilities.CommonConstants.PackingType, null);
                        break;
                    case ControlsEnum.PACKING:
                        dsPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(CurrPK, Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID, string.Empty, string.Empty,string.Empty);
                        break;
                    case ControlsEnum.CONTROLS:
                        dtControlsData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Packing, (int)PackingType.PakingMaterialType, 1, currentUser.SBUID);
                        break;
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
                    case ControlsEnum.PACKING:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.PACKINGTYPE:
                        BindPackTypeDropDown();
                        break;
                    case ControlsEnum.CONTROLS:
                        SetControls();
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
       
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                   // case ActionsEnum.WRKFSUBMIT:
                        PackMasterBO = new PackingMasterBO();

                        PackMasterBO.APS_PK = CurrPK;
                        PackMasterBO.APS_CODE = txtPackCode.Text;
                        PackMasterBO.APS_NAME = txtPackSpecName.Text;
                        PackMasterBO.APS_TYPE = Convert.ToInt32(ddlPackingType.SelectedItem.Value);
                        PackMasterBO.APS_PC_PCS = Convert.ToDouble(txtPouchPcs.Text==string.Empty ?"0":txtPouchPcs.Text);
                        PackMasterBO.APS_IB_PCS = Convert.ToDouble(txtInnerBox.Text==string.Empty?"0":txtInnerBox.Text);
                        PackMasterBO.APS_IC_PCS = Convert.ToDouble(txtInnerCarton.Text==string.Empty?"0":txtInnerCarton.Text);
                        PackMasterBO.APS_ZB_PCS = Convert.ToDouble(txtZipperBag.Text==string.Empty?"0":txtZipperBag.Text);
                        PackMasterBO.APS_MC_PCS = Convert.ToDouble(txtMasterCarton.Text==string.Empty?"0":txtMasterCarton.Text);
                        PackMasterBO.APS_SC_PCS = Convert.ToDouble(txtSack.Text==string.Empty?"0":txtSack.Text);
                        PackMasterBO.APS_POB_PCS = Convert.ToDouble(txtPlainOuterBag.Text == string.Empty ? "0" : txtPlainOuterBag.Text);
                        PackMasterBO.APS_PRB_PCS = Convert.ToDouble(txtPrintedOuterBag.Text == string.Empty ? "0" : txtPrintedOuterBag.Text);
                        PackMasterBO.APS_WLT_PCS = Convert.ToDouble(txtWallet.Text == string.Empty ? "0" : txtWallet.Text);
                        PackMasterBO.APS_TOTAL_PCS = Convert.ToDouble(txtTotalPcs.Text);
                        PackMasterBO.APS_DESC = txtPackingDesc.Text;
                        PackMasterBO.ACTIVE= Convert.ToInt16(DbActiveStatus.ACTIVE);
                        PackMasterBO.USER_PK = currentUser.PKUser;
                        PackMasterBO.BIZUNIT = currentUser.SBUID;
                        PackMasterBO.LAST_MOD_DT = LastModifiedTime;
                        returnObj = PackMasterBO;
                        break;
                    #endregion
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
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (dsPageData != null && dsPageData.Tables[0].Rows.Count> 0)
                {
                    CurrPK = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["APS_PK"].ToString());
                    txtPouchPcs.Text = dsPageData.Tables[0].Rows[0]["APS_PC_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_PC_PCS"].ToString();
                    txtPackCode.Text = dsPageData.Tables[0].Rows[0]["APS_CODE"].ToString();
                    txtPackCode.ToolTip = dsPageData.Tables[0].Rows[0]["APS_CODE"].ToString();
                    txtPackSpecName.Text = dsPageData.Tables[0].Rows[0]["APS_NAME"].ToString();
                    txtPackSpecName.ToolTip = dsPageData.Tables[0].Rows[0]["APS_NAME"].ToString();
                    txtInnerBox.Text = dsPageData.Tables[0].Rows[0]["APS_IB_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_IB_PCS"].ToString();
                    txtInnerCarton.Text = dsPageData.Tables[0].Rows[0]["APS_IC_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_IC_PCS"].ToString();
                    txtMasterCarton.Text = dsPageData.Tables[0].Rows[0]["APS_MC_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_MC_PCS"].ToString();
                    txtSack.Text = dsPageData.Tables[0].Rows[0]["APS_SC_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_SC_PCS"].ToString();
                    txtTotalPcs.Text = dsPageData.Tables[0].Rows[0]["APS_TOTAL_PCS"].ToString();
                    txtZipperBag.Text = dsPageData.Tables[0].Rows[0]["APS_ZB_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_ZB_PCS"].ToString();
                    txtPlainOuterBag.Text = dsPageData.Tables[0].Rows[0]["APS_POB_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_POB_PCS"].ToString();
                    txtWallet.Text = dsPageData.Tables[0].Rows[0]["APS_WLT_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_WLT_PCS"].ToString();
                    txtPrintedOuterBag.Text = dsPageData.Tables[0].Rows[0]["APS_PRB_PCS"].ToString() == "0" ? string.Empty : dsPageData.Tables[0].Rows[0]["APS_PRB_PCS"].ToString();
                    txtPackingDesc.Text = dsPageData.Tables[0].Rows[0]["APS_DESC"].ToString();
                    LastModifiedTime = Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["LAST_MOD_DT"].ToString());
                    ddlPackingType.SelectedValue = (dsPageData.Tables[0].Rows[0]["APS_TYPE"].ToString());
                    ddlPackingType.ToolTip = (dsPageData.Tables[0].Rows[0]["CON_NAME"].ToString());
                 
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                }
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    ModifiedDatePnl.Visible = false;
                    throw new Exception(litErrorMsg.Text);
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
                    case ControlsEnum.DEFAULT:
                        if (dsPageData != null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdPackingMst.PageIndex = Convert.ToInt32(PageIndex);
                            dsPageData.Tables[0].DefaultView.Sort = SortBy + " " + SortDirection;
                            grdPackingMst.DataSource = dsPageData.Tables[0].DefaultView;
                            grdPackingMst.DataBind();
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
        /// Method for Packing Type DropDown
        /// </summary>
        public void BindPackTypeDropDown()
        {
            ddlPackingType.Items.Clear();
            if (admConstMstList != null && admConstMstList.Count > 0)
            {
                ddlPackingType.DataSource = admConstMstList;
                ddlPackingType.DataTextField = Resources.DataFieldRes.ConstName;
                ddlPackingType.DataValueField = Resources.DataFieldRes.ConstPK;
                ddlPackingType.DataBind();
            }
            ddlPackingType.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdPackingMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdPackingMst.DataKeys[grdrow.RowIndex].Values[0]);
                        GetFieldValues(ControlsEnum.PACKINGTYPE);
                        SetFieldValues(ControlsEnum.PACKINGTYPE);
                        GetFieldValues(ControlsEnum.PACKING);
                        SetFieldValues(ControlsEnum.PACKING);
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
                        //Label lblPackSpecCode= grdrow.FindControl("lblPackSpecCode") as Label;
                        //lblPackingSpecCodeHdr.ToolTip = (lblPackSpecCode != null && !string.IsNullOrEmpty(lblPackSpecCode.ToolTip.Trim())) ?
                        //    lblPackSpecCode.ToolTip.Trim() : "[New]";
                        //lblPackingSpecCodeHdr.Text = CommonFunctions.GetShortString(lblPackingSpecCodeHdr.ToolTip, 25);
                        //Label TotalPieces = grdrow.FindControl("TotalPieces") as Label;
                        //lblPackingSpecTotalHdr.Text = lblPackingSpecTotalHdr.ToolTip = (TotalPieces != null && !string.IsNullOrEmpty(TotalPieces.ToolTip.Trim())) ?
                        //    TotalPieces.ToolTip.Trim() : "0";
                        txtPackCode.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                }

                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            txtSearchBy.Text = string.Empty;
            txtPackCode.Text = string.Empty;
            txtPackCode.ToolTip = string.Empty;
            ddlPackingType.SelectedIndex = 0;
            ddlPackingType.ToolTip = string.Empty;
            txtPouchPcs.Text = string.Empty;
            txtTotalPcs.Text = string.Empty;
            txtInnerBox.Text = string.Empty;
            txtInnerCarton.Text = string.Empty;
            txtMasterCarton.Text = string.Empty;
            txtPackSpecName.Text = string.Empty;
            txtPackSpecName.ToolTip = string.Empty;
            txtSack.Text = string.Empty;
            txtPackingDesc.Text = string.Empty;
            txtZipperBag.Text = string.Empty;
            txtPlainOuterBag.Text = txtWallet.Text = txtPrintedOuterBag.Text = string.Empty;
            PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
        }
        #endregion


           /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdPackingMst")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        ImageButton btnBrand = e.Row.FindControl("imbGenerateBrand") as ImageButton;
                        HiddenField hdfapshasbrandprd = e.Row.FindControl("hdfapshasbrandprd") as HiddenField;
                        if (GetGlobalResourceObject("ConfigurationsRes", "ShowPRDGenerateButton").ToString() == "1" && hdfapshasbrandprd.Value == "0")
                        {
                            btnBrand.Visible = true;
                        }
                        else
                        {
                            btnBrand.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


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
                GridViewRow gvr;
                int? result;
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
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.TOOLTIP;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            PackMasterBO = (PackingMasterBO)SetUIValuesToObject(commonActions);
                            if (PackMasterBO != null)
                            {
                                result = PackingMasterBL.SavePackingMaster(PackMasterBO);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    SortBy = Resources.DataFieldRes.PackingMstPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                                //Duplicate records
                                //Packing spec duplicates
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingMaster) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingMaster) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingMaster) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaster);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = BusinessLogic.Inventory.PackingMasterBL.DeletePackingMaster(CurrPK, LastModifiedTime);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingMaster) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaster + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingMaster) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaster);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        this.txtPackCode.Focus();
                        GetFieldValues(ControlsEnum.PACKINGTYPE);
                        SetFieldValues(ControlsEnum.PACKINGTYPE);
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
                        EntryStatus = EntryStatus.NEWMODE;
                        //lblPackingSpecCodeHdr.Text = lblPackingSpecCodeHdr.ToolTip = "[New]";
                        //lblPackingSpecTotalHdr.Text = lblPackingSpecTotalHdr.ToolTip = "0";
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(commonActions);

                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region PackingMapping
                    case ActionsEnum.PACKINGMAPPING:
                        //if (Session[ERP.Utilities.SessionStrings.PackingPk] != null)
                        //{
                        Response.Redirect(Resources.PageURL.PackingSpecMapping.ToString());
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    EntryStatus = EntryStatus.LISTMODE;
                        //}
                        break;
                    #endregion

                    #region ShowDetails
                    case ActionsEnum.SHOWDETAILS:
                        GridViewRow row = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        int PackingPK = Convert.ToInt32(((HiddenField)row.FindControl("hfPackingPK")).Value);
                        if (PackingPK != null)
                        {
                            Session[ERP.Utilities.SessionStrings.PackingPk] = PackingPK;
                        }

                        break;
                    case ActionsEnum.TOOLTIP:
                        ddlPackingType.ToolTip = ddlPackingType.SelectedItem.Text;
                        break;
                    #endregion

                    #region GENERATEBRAND
                    case ActionsEnum.GENERATEBRAND:
                         gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                         CurrPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hfPackingPK")).Value);
                         currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                         result = PackingMasterBL.BrandProductSpecSave(CurrPK, currentUser.PKUser);
                         if (result > 0)
                         {
                             //ResetForm();
                             GetFieldValues(ControlsEnum.DEFAULT);
                             SetFieldValues(ControlsEnum.DEFAULT);
                             litErrorMsg.Text = Resources.Messages.Msg_Generate_Brand_Success;
                             ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                         }
                         else if (result <= 0)
                         {
                             ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BrandProductSaveFailed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                         }
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                
            }
        }
        
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
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
                this.PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
        }

        
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        private void SetControls()
        {
            if (dtControlsData != null)
            {
                if (dtControlsData.Rows.Count > 0)
                {
                    DataView dv = dtControlsData.DefaultView;
                    dv.Sort = Resources.DataFieldRes.ConstValue + " " + Resources.Constants.DefaultSortDirection;
                    DataTable dtControlsDataSort = dv.ToTable();



                    lblPouchpcs.Text = dtControlsDataSort.Rows[0][Resources.DataFieldRes.ConstName].ToString();

                    if (dtControlsDataSort.Rows.Count > 1)
                    {
                        lblInnerBox.Text = dtControlsDataSort.Rows[1][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 2)
                    {
                        lblInnerCarton.Text = dtControlsDataSort.Rows[2][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 3)
                    {
                        lblZipperBag.Text = dtControlsDataSort.Rows[3][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 4)
                    {
                        lblMasterCarton.Text = dtControlsDataSort.Rows[4][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 5)
                    {
                        lblSack.Text = dtControlsDataSort.Rows[5][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 6)
                    {
                        lblWallet.Text = dtControlsDataSort.Rows[6][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 7)
                    {
                        lblPlainOuterBag.Text = dtControlsDataSort.Rows[7][Resources.DataFieldRes.ConstName].ToString();
                    }
                    if (dtControlsDataSort.Rows.Count > 8)
                    {
                        lblPrintedOuterBag.Text = dtControlsDataSort.Rows[8][Resources.DataFieldRes.ConstName].ToString();
                    }
                }
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
            string breadCrumb;
            breadCrumb = this.GetLocalResourceObject("BreadcrumbPackingCreation").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotalPcs", "CalcTotalPcs();", true);

            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                breadCrumb = this.GetLocalResourceObject("BreadcrumbPackingList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            }
            lblBreadCrum.Text = breadCrumb;
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
                    imgbtnPouchInformation.ToolTip = GetLocalResourceObject("NewPouchInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnInnerBoxInformation.ToolTip = GetLocalResourceObject("InnerBoxInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnInnerCartonInformation.ToolTip = GetLocalResourceObject("InnerCartonInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnZipperBagInformation.ToolTip = GetLocalResourceObject("ZipperBagInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnMasterCartonInformation.ToolTip = GetLocalResourceObject("MasterCartonInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnSackInformation.ToolTip = GetLocalResourceObject("SackBagInformation").ToString().Replace("##", Environment.NewLine);                    
                    imgbtnPlainOuterBagInformation.ToolTip = GetLocalResourceObject("PlainBagInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnWalletInformation.ToolTip = GetLocalResourceObject("WalletInformation").ToString().Replace("##", Environment.NewLine);
                    imgbtnPrintedOuterBag.ToolTip = GetLocalResourceObject("PrintedOuterBagInformation").ToString().Replace("##", Environment.NewLine);
                    
                    //////this.btnNew.Focus();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.PackingMstPK;
                    grdPackingMst.DataKeyNames = datakeyarray;
                    int PackingMode = (Session[ERP.Utilities.SessionStrings.PackingMode] != null) ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.PackingMode]) : 1;
                    if (PackingMode == 1)
                    {
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    else
                    {
                        CurrPK = (Session[ERP.Utilities.SessionStrings.PackingPk] != null) ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.PackingPk]) : 0;
                        if (CurrPK != 0)
                        {
                            GetFieldValues(ControlsEnum.PACKINGTYPE);
                            SetFieldValues(ControlsEnum.PACKINGTYPE);
                            GetFieldValues(ControlsEnum.PACKING);
                            SetFieldValues(ControlsEnum.PACKING);
                            GetFieldValues(ControlsEnum.CONTROLS);
                            SetFieldValues(ControlsEnum.CONTROLS);
                            txtPackCode.Focus();
                            ModifiedDatePnl.Visible = true;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                    }
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
            DEFAULT,
            PACKING,
            IPPACKING,
            OPPACKING,
            POUCHPACKING,
            PACKINGTYPE,
            CONTROLS
        }
        #endregion
    }
}