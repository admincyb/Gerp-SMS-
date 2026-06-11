using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Inventory;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Inventory;
using BusinessLogic.Inventory;
using System.Data;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class PlanningGroupsMaster : ERP.Store.UI.MyBasePage//: System.Web.UI.Page
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
        /// Current PK
        /// </summary>
        private Byte CurrGrpType
        {
            get
            {
                return Convert.ToByte(this.ViewState[ViewstateStrings.CurrGrpType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrGrpType] = value;
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
        private BusinessObject.User currentUser;
        DataTable ProductItems;
        private object returnObject;
        private PlanningBO objPlanningHeader;
        private DataTable PlanningItemGroupList;
        private DataSet PlanningItemEdit;
        private string PlanningGroupCode;
        private string PlanningGroupName;
        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
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
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE).ToString();
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
                        int PageSize=Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        string FilterBy = ddlFilterBy.SelectedItem.Value;
                        if (FilterBy == "1" && txtFilterPlan.Text != "Select/Type")
                            PlanningGroupCode = txtFilterPlan.Text;
                        else if (FilterBy == "2" && txtFilterPlan.Text != "Select/Type")
                            PlanningGroupName = txtFilterPlan.Text;
                        PlanningItemGroupList = ProductsBL.GetPlanningItemGroupList(PageIndex, PageSize, CurrPK, PlanningGroupCode, PlanningGroupName);
                        break;
                    case ControlsEnum.ITEM:
                        ProductItems = ProductsBL.GetProductItems(CurrPK);
                        break;
                    case ControlsEnum.EDIT:
                        PlanningItemEdit = ProductsBL.GetPlanningItemEdit(CurrPK);
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
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.ITEM:
                        BindTreeView();
                        break;
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject();
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

        private void BindPlanningGroupTreeView()
        {
            try
            {
                TreeNode child;
                TreeNode root;
                trvItem.Nodes.Clear();
                root = new TreeNode(GetLocalResourceObject("AllItems").ToString(), "0");
                trvItem.Nodes.Add(root);
                root.Collapse();
                if (PlanningItemEdit != null && PlanningItemEdit.Tables[1].Columns.Count > 0)
                {
                    foreach (DataRow item in PlanningItemEdit.Tables[1].Rows)
                    {
                        child = new TreeNode();
                        child.ShowCheckBox = true;
                        child.SelectAction = TreeNodeSelectAction.Select;
                        child.Checked = Convert.ToBoolean(item[PlanningGroupPdt.IS_MAPPED]);
                        child.Text = item["ITM_NAME"].ToString();
                        child.Value = item["ITM_PK"].ToString();
                        root.ChildNodes.Add(child);

                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Method to bind the Item Tree
        /// </summary>
        private void BindTreeView()
        {
            try
            {
                TreeNode child;
                TreeNode root;
                trvItem.Nodes.Clear();
                root = new TreeNode(GetLocalResourceObject("AllItems").ToString(), "0");
                trvItem.Nodes.Add(root);
                root.Collapse();

                if (ProductItems != null && ProductItems.Columns.Count > 0)
                {
                    foreach (DataRow item in ProductItems.Rows)
                    {
                        child = new TreeNode();
                        child.ShowCheckBox = true;
                        child.Text = item["ITM_NAME"].ToString();
                        child.Value = item["ITM_PK"].ToString();
                        root.ChildNodes.Add(child);
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        /// <summary>
        // Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private PlanningBO SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                objPlanningHeader = new PlanningBO();
                objPlanningHeader.PIG_PK = CurrPK;
                objPlanningHeader.PIG_CODE = HttpUtility.HtmlEncode(txtGroupCode.Text.Trim());
                objPlanningHeader.PIG_NAME = HttpUtility.HtmlEncode(txtGroupName.Text.Trim());
                objPlanningHeader.PIG_DESC = HttpUtility.HtmlEncode(txtGroupDesc.Text.Trim());

                objPlanningHeader.LAST_MOD_DT = LastModifiedTime.ToString();
                objPlanningHeader.ACTIVE = Convert.ToByte(ddlStatus.SelectedValue).ToString();
                objPlanningHeader.BIZUNIT = currentUser.SBUID.ToString();
                objPlanningHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                objPlanningHeader.DetailList = new List<PlanningDetailsBO>();
                PlanningDetailsBO objDet;
                foreach (TreeNode root in trvItem.Nodes)
                {
                    foreach (TreeNode child in root.ChildNodes)
                    {
                        if (child.Checked)
                        {
                            objDet = new PlanningDetailsBO();
                            objDet.ITM_PK = Convert.ToInt32(child.Value);
                            objPlanningHeader.DetailList.Add(objDet);
                        } 
                    }
                }
                return objPlanningHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                returnObject = null;
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                EntryStatus = EntryStatus.EDITMODE;
                CurrPK = Convert.ToInt32(PlanningItemEdit.Tables[0].Rows[0]["PIG_PK"]);
                txtGroupCode.Text = HttpUtility.HtmlDecode(PlanningItemEdit.Tables[0].Rows[0]["PIG_CODE"].ToString());
                txtGroupName.Text = HttpUtility.HtmlDecode(PlanningItemEdit.Tables[0].Rows[0]["PIG_NAME"].ToString());
                ddlStatus.SelectedValue = PlanningItemEdit.Tables[0].Rows[0]["PIG_ACTIVE"].ToString();
                txtGroupDesc.Text = HttpUtility.HtmlDecode(PlanningItemEdit.Tables[0].Rows[0]["PIG_DESC"].ToString());
                LastModifiedTime = Convert.ToDateTime(PlanningItemEdit.Tables[0].Rows[0]["PIG_MOD_DT"].ToString());
                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                BindPlanningGroupTreeView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                    uclPaging.Visible = false;
                    if (PlanningItemGroupList != null && PlanningItemGroupList.Rows.Count > 0)
                    {
                        int rowCount = 0;
                        rowCount = Convert.ToInt32(PlanningItemGroupList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                          (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                          (rowCount / this.PageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdPlanningGroupMst.DataSource = PlanningItemGroupList;
                        grdPlanningGroupMst.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                    }
                    else
                    {
                        grdPlanningGroupMst.DataSource = null;
                        grdPlanningGroupMst.DataBind();
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Status Dropdown
        /// </summary>
        public void BindStatusDropDown()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlStatus.Items.Insert(1, new ListItem(Resources.Controls.Active, ((int)RecordStatus.ACTIVE).ToString()));
            ddlStatus.Items.Insert(2, new ListItem(Resources.Controls.InActive, ((int)RecordStatus.INACTIVE).ToString()));
            ddlStatus.SelectedValue = "1";
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdPlanningGroupMst.Rows)
                {
                    CurrPK = 0;
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPlanningPk")).Value);
                        GetFieldValues(ControlsEnum.EDIT);
                        SetFieldValues(ControlsEnum.EDIT);
                        txtGroupCode.Focus();
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
            ddlFilterBy.SelectedIndex = 0;
            txtFilterPlan.Text = string.Empty;
            txtSearchBy.Text = string.Empty;
            txtGroupCode.Text = string.Empty;
            txtGroupName.Text = string.Empty;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            txtGroupDesc.Text = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
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
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
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
                            returnObject = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<PlanningBO>(objPlanningHeader);
                            result = ProductsBL.SavePlanningGroup(xmlDoc);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PlanningGroup);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                            else
                            {
                                if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.PENDINGEXIST)
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Already_Exist;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.ALREADYDELETED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " +
                                      GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PlanningGroup);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        BindStatusDropDown();
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        ModifiedDatePnl.Visible = false;
                        this.txtGroupCode.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        //btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
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
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        result = ProductsBL.DeleteInvItemPlanningMst(CurrPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PlanningGroup);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        //delete reference error
                        else
                        {
                            if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PlanningGroup + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == -1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PlanningGroup);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("ProductGroupDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Code)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Row data bound Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblProductGroupActive = e.Row.FindControl("lblProductGroupActive") as Label;
                //lblProductGroupActive.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.INVProductGroupStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
                //lblProductGroupActive.ToolTip = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.INVProductGroupStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
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
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
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
                        // Decrement the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Increment the last page index.
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages, "uclPaging");
                //EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
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
            string docTitle = string.Empty;

            //string breadCrumPage = string.Empty;
            string breadCrumPage = "Planning Groups";
            string breadCrumMode = "Creation";
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

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
                breadCrumMode = "Listing";
            }
            breadCrumb = string.Format(this.GetLocalResourceObject("BreadcrumbCustom").ToString(), breadCrumPage, breadCrumMode);
            breadCrumb = breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            lblBreadCrum.Text = breadCrumb;
            this.Title = docTitle;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            ITEM,
            EDIT
        }
        #endregion

        public class PlanningGroupPdt
        {
            public const string IS_MAPPED = "IS_MAPPED";
        }
    }
}