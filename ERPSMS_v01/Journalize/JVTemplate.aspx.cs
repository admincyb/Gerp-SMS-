#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using System.Data;
using System.Reflection;
using BusinessObject;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using System.Xml;
using BusinessObject.CommonManagement;
using CustomControls;
using BusinessLogic.CommonManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Journalize;
using BusinessLogic.Jouralize;

#endregion

namespace ERPSMS_v01.Journalize
{
    public partial class JVTemplate : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

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
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
       
        /// <summary>
        /// Item PK
        /// </summary>
        private int ItemPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemPK] = value;
            }
        }

        /// <summary>
        /// SelectedPK PK-- Used to keep the selected pk from a grid
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        /// <summary>
        /// To maintain Page Size
        /// </summary>
        private int PageSize
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageSize] == null ? 0 : (int)this.ViewState[ViewstateStrings.PageSize];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
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
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
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
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }
        /// <summary>
        /// To maintain the sort expression in viewstate
        /// </summary>
        private string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }
            set
            {
                this.ViewState["SortExpression"] = value;
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

        /// <summary>
        /// To maintain the From Date in viewstate
        /// </summary>
        private DateTime FromDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.FromDate] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.FromDate];

            }
            set
            {
                this.ViewState[ViewstateStrings.FromDate] = value;
            }
        }

        /// <summary>
        /// To maintain the To Date in viewstate
        /// </summary>
        private DateTime ToDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.ToDate] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.ToDate];

            }
            set
            {
                this.ViewState[ViewstateStrings.ToDate] = value;
            }
        }

        #endregion
        User currentUser;
        private ActionsEnum commonActions;
        DataSet dsPageData;
        DataTable dtType;
        DataSet dsAccountDtls;
        XmlDocument xmlDoc;
        JVTemplateBO jvTemplateObj;

        private static List<JVTemplateDetailsList> jvTemplateDetailslist;
        private static int slno = 0;
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
                    slno = 0;
                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);

                    EntryStatus = EntryStatus.LISTMODE;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    PageSize = Convert.ToInt32(grdJVList.PageSize);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                   
                    //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }
        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }


        /// <summary>
        /// Get the User Rights, Checks Page Level Rights, 
        /// Hides sections in which user don't have access rights
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
            {
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        int status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int Stype = Convert.ToInt32(ddlSType.SelectedValue);
                        string name = txtTName.Text.Trim();
                        dsPageData = JournalizeBL.GetVoucherTemplateList(CurrPK, status, Stype, name);
                        break;
                    #endregion
                    case ControlsEnum.TYPE:
                        dtType = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.VoucherTemplateType,1, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.ACCOUNTDETAILS:
                        dsAccountDtls = BusinessLogic.Jouralize.JournalizeBL.GetAccountDetails(hdfAccount.Value != string.Empty ? Convert.ToInt32(hdfAccount.Value) : 0, 2);
                        break;
                    case ControlsEnum.TMPDTLS:
                        dsPageData = JournalizeBL.GetVoucherTemplateDetails(CurrPK);
                        break;
                }
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
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        /// <param name="mode"></param>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdJVList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        HiddenField hdfTemplatePK;
                        hdfTemplatePK = (HiddenField)grdrow.FindControl("hdfTemplatePK");
                        CurrPK = hdfTemplatePK != null ? !string.IsNullOrEmpty(hdfTemplatePK.Value) ? Convert.ToInt32(hdfTemplatePK.Value) : -1 : -1;
                        GetFieldValues(ControlsEnum.TMPDTLS);
                        SetFieldValues(ControlsEnum.TMPDTLS);
                        txtName.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;

                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                //EntryStatus = EntryStatus.LISTMODE;
            }
            catch
            {
                throw;
            }
        }
        private void SetFieldValues(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.DEFAULT:
                    BindGrid(ControlsEnum.DEFAULT);
                    //btnEdit.Visible = false;
                    //btnView.Visible = true;
                    break;
                case ControlsEnum.TYPE:
                    BindDropdown(ControlsEnum.TYPE);
                    break;
                case ControlsEnum.ACCOUNTDETAILS:
                    
                    break;
                case ControlsEnum.TMPDTLS:
                    GetUIValuesFromObject();
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            long result;
            result = 0;
            GridViewRow gvr;
            GridView grd;
            string arg;
            string saveXml;
            string action;
            DataRow[] drr;

            GridViewRow gvrow;
            GridViewRow previousRow;
            HiddenField hdfCSeq ;
            HiddenField hdfPSeq;
            HiddenField hdfCSlNo;
            HiddenField hdfPSlNo;
            JVTemplateDetailsList crowitem;
            JVTemplateDetailsList prowitem;

            DropDownList ddlWkfAction;
            try
            {
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    case ActionsEnum.ADD:

                        if (hdfSNO.Value != string.Empty)
                        {
                            JVTemplateDetailsList row = jvTemplateDetailslist.SingleOrDefault(c => c.SLNO == Convert.ToInt32(hdfSNO.Value));
                            jvTemplateDetailslist.Remove(row);
                        }
                        GetFieldValues(ControlsEnum.ACCOUNTDETAILS);
                        JVTemplateDetailsList item = new JVTemplateDetailsList();
                        item.VLD_ACCOUNT =Convert.ToInt32(hdfAccount.Value);
                        if(dsAccountDtls!=null && dsAccountDtls.Tables[0].Rows.Count>0)
                        {
                            item.COA_CODE =dsAccountDtls.Tables[0].Rows[0]["COA_CODE"].ToString();
                            item.COA_NAME =dsAccountDtls.Tables[0].Rows[0]["COA_NAME"].ToString();
                        }
                        item.VLD_MODE=Convert.ToInt32(ddlMode.SelectedValue);
                        item.VLD_PK = hdfVLDPK.Value != string.Empty ? Convert.ToInt32(hdfVLDPK.Value) : 0;
                        item.VLD_SEQUENCE = hdfSeq.Value != string.Empty ? Convert.ToInt16(hdfSeq.Value) : (short)(slno + 1);
                        item.VLD_REF_TYPE = hdfRType.Value;
                        item.VLD_REF_TYPE_PK = hdfRefPK.Value != string.Empty ? Convert.ToInt32(hdfRefPK.Value) : 0;
                        item.SLNO=slno;
                        jvTemplateDetailslist.Add(item);
                        slno ++;
                        BindGrid(ControlsEnum.TEMPLATESDETAILS);
                        ClearForm(ControlsEnum.TMPDTLS);
                        break;
                    #region New
                    case ActionsEnum.NEW:
                        slno = 0;
                        jvTemplateDetailslist = new List<JVTemplateDetailsList>();
                        ClearForm(ControlsEnum.CLEARALL);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        slno = 0;
                        jvTemplateDetailslist = new List<JVTemplateDetailsList>();
                        SetUIEditView(ActionsEnum.VIEW);
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                        slno = 0;
                        jvTemplateDetailslist = new List<JVTemplateDetailsList>();
                        SetUIEditView(ActionsEnum.EDIT);
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ClearForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                   
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Clear
                    case ActionsEnum.CLEAR:
                        ClearForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = JournalizeBL.DeleteVoucherTemplate(CurrPK);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                
                                ResetForm();
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.JournalizeTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
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
                                    litErrorMsg.Text = Resources.PageNameRes.JournalizeTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.JournalizeTemplate + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.JournalizeTemplate + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.JournalizeTemplate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Save
                    case ActionsEnum.SAVE:
                        //Save Details
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            if (jvTemplateDetailslist.Count > 0)
                            {
                                jvTemplateObj = (JVTemplateBO)SetUIValuesToObject(ActionsEnum.SAVE);
                                xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(jvTemplateObj);
                                result = JournalizeBL.SaveVoucherTemplate(xmlDoc.InnerXml);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    ResetForm();
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.JournalizeTemplate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
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
                                        litErrorMsg.Text = Resources.PageNameRes.JournalizeTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.JournalizeTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.JournalizeTemplate);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Templates").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }


                            
                             

                        }

                        break;
                    #endregion

                    #region DELETEGRID
                    case ActionsEnum.DELETEGRID:
                        GridViewRow grdrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfSlNo = (HiddenField)grdrow.FindControl("hdfSlNo");
                        JVTemplateDetailsList rowitem=  jvTemplateDetailslist.SingleOrDefault(c=>c.SLNO == Convert.ToInt32(hdfSlNo.Value));
                        jvTemplateDetailslist.Remove(rowitem);
                        BindGrid(ControlsEnum.TEMPLATESDETAILS);
                        break;
                    #endregion

                    #region EDITGRID
                    case ActionsEnum.EDITGRID:
                        ClearForm(ControlsEnum.TMPDTLS);
                        GridViewRow grdeditrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        SetItemDetails(grdeditrow);
                        break;
                    #endregion

                    #region UPGRID
                    case ActionsEnum.UPGRID:
                        GridViewRow grduprow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        previousRow = grdJvItems.Rows[grduprow.RowIndex - 1];
                        hdfCSeq = (HiddenField)grduprow.FindControl("hdfSequence");
                        hdfPSeq = (HiddenField)previousRow.FindControl("hdfSequence");
                        hdfCSlNo = (HiddenField)grduprow.FindControl("hdfSlNo");
                        hdfPSlNo = (HiddenField)previousRow.FindControl("hdfSlNo");
                        crowitem = jvTemplateDetailslist.SingleOrDefault(c => c.SLNO == Convert.ToInt32(hdfCSlNo.Value));
                        prowitem = jvTemplateDetailslist.SingleOrDefault(c => c.SLNO == Convert.ToInt32(hdfPSlNo.Value));
                        if (crowitem != null && prowitem !=null)
                        {
                            crowitem.VLD_SEQUENCE = Convert.ToInt16(hdfPSeq.Value);
                            prowitem.VLD_SEQUENCE = Convert.ToInt16(hdfCSeq.Value);
                        }
                        BindGrid(ControlsEnum.TEMPLATESDETAILS);
                        ClearForm(ControlsEnum.TMPDTLS);
                        break;
                    #endregion

                    #region DOWNGRID
                    case ActionsEnum.DOWNGRID:
                        GridViewRow grddownrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        previousRow = grdJvItems.Rows[grddownrow.RowIndex + 1];
                        hdfCSeq = (HiddenField)grddownrow.FindControl("hdfSequence");
                        hdfPSeq = (HiddenField)previousRow.FindControl("hdfSequence");
                        hdfCSlNo = (HiddenField)grddownrow.FindControl("hdfSlNo");
                        hdfPSlNo = (HiddenField)previousRow.FindControl("hdfSlNo");
                        crowitem = jvTemplateDetailslist.SingleOrDefault(c => c.SLNO == Convert.ToInt32(hdfCSlNo.Value));
                        prowitem = jvTemplateDetailslist.SingleOrDefault(c => c.SLNO == Convert.ToInt32(hdfPSlNo.Value));
                        if (crowitem != null && prowitem !=null)
                        {
                            crowitem.VLD_SEQUENCE = Convert.ToInt16(hdfPSeq.Value);
                            prowitem.VLD_SEQUENCE = Convert.ToInt16(hdfCSeq.Value);
                        }
                        BindGrid(ControlsEnum.TEMPLATESDETAILS);
                        ClearForm(ControlsEnum.TMPDTLS);
                        break;
                    #endregion




                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// Handling Grid events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            string arg;
            ExtGridViewRow gvr;
            GridView grd;
            DropDownList ddlCurrency;
            try
            {
                //if ((sender as GridView).ID == "grdSelectedCusBrands")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow)
                //    {

                //        if (dsCustomerRate != null && dsCustomerRate.Tables[1].Rows.Count > 0)
                //        {
                //            HiddenField hdfItem = e.Row.FindControl("hdfItemPK") as HiddenField;
                //            var results = from myRow in dsCustomerRate.Tables[1].AsEnumerable()
                //                          where myRow.Field<int>("ITM_PK") == Convert.ToInt32(hdfItem.Value)
                //                          select new
                //                          {
                //                              CUS_PK = myRow.Field<int>("CUS_PK"),
                //                              CIM_PK = myRow.Field<int>("CIM_PK"),
                //                              ITM_PK = myRow.Field<int>("ITM_PK"),
                //                              CUS_CODE = myRow.Field<string>("CUS_CODE"),
                //                              CUS_NAME = myRow.Field<string>("CUS_NAME"),
                //                              CIM_BRAND_CODE = myRow.Field<string>("CIM_BRAND_CODE"),
                //                              CIM_BRAND_TEXT = myRow.Field<string>("CIM_BRAND_TEXT"),
                //                              APS_TEXT = myRow.Field<string>("APS_TEXT"),
                //                              BRD_RATE = myRow.Field<double?>("BRD_RATE"),
                //                              CUR_PK = myRow.Field<int?>("CUR_PK")
                //                          };
                //            gvr = e.Row as ExtGridViewRow;
                //            if (gvr != null)
                //            {
                //                grd = gvr.FindControl("grdSelectdCustomers") as GridView;
                //                grd.DataSource = results;
                //                grd.DataBind();
                //                gvr.ShowExpand = true;
                //            }
                //        }
                //    }
                //}
                //if ((sender as GridView).ID == "grdJvItems")
                // {
                //     if (e.Row.RowType == DataControlRowType.DataRow)
                //     {
                //         if (dsCustomerRate != null && dsCustomerRate.Tables[1].Rows.Count > 0)
                //         {
                //             HiddenField hdfCurrPK = e.Row.FindControl("hdfCurrPK") as HiddenField;
                //             ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                //             //ddlCurrency.DataSource = CommonBL.GetCurrencyList(currentUser.SBUID);
                //             ddlCurrency.DataSource = Currency;
                //             ddlCurrency.DataTextField = "CUR_CODE";
                //             ddlCurrency.DataValueField = "CUR_PK";
                //             ddlCurrency.ToolTip = "CUR_NAME";
                //             ddlCurrency.DataBind();
                //             ddlCurrency.SelectedValue = hdfCurrPK.Value;

                //         }
                //     }
                // }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            if ((sender as GridView).ID == "grdJVList")
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;

                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            else if ((sender as GridView).ID == "grdJvItems")
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
            }
        }

        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
        }

        #endregion

        #region Helper Methods

        private void SetItemDetails(GridViewRow grdItemRow)
        {

            HiddenField hdfSlNo = (HiddenField)grdItemRow.FindControl("hdfSlNo");
            HiddenField hdfDetailsPK = (HiddenField)grdItemRow.FindControl("hdfDetailsPK");
            HiddenField hdfRefType = (HiddenField)grdItemRow.FindControl("hdfRefType");
            HiddenField hdfRefTypePK = (HiddenField)grdItemRow.FindControl("hdfRefTypePK");
            HiddenField hdfSequence = (HiddenField)grdItemRow.FindControl("hdfSequence");
            HiddenField hdfGAccount = (HiddenField)grdItemRow.FindControl("hdfGAccount");
            HiddenField hdfMode = (HiddenField)grdItemRow.FindControl("hdfMode");

            Label lblVAccountName = (Label)grdItemRow.FindControl("lblVAccountName");
            Label lblVAccountCode = (Label)grdItemRow.FindControl("lblVAccountCode");

            hdfVLDPK.Value = hdfDetailsPK.Value;
            hdfSeq.Value = hdfSequence.Value;
            hdfRType.Value = hdfRefType.Value;
            hdfRefPK.Value = hdfRefTypePK.Value;
            hdfSNO.Value = hdfSlNo.Value;
            hdfAccount.Value = hdfGAccount.Value;
            txtAccount.Text = lblVAccountCode.Text + " - " + lblVAccountName.Text;
            ddlMode.SelectedValue = hdfMode.Value;
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            string value;
            value = string.Empty;
            bool hasUIValue = false;
            GridView grd;
            try
            {
                switch (mode)
                {
                    #region Save Details
                    case ActionsEnum.SAVE:

                        jvTemplateObj = new JVTemplateBO();
                        jvTemplateObj.VLH_PK = CurrPK;
                        jvTemplateObj.VLH_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                        jvTemplateObj.VLH_NAME = HttpUtility.HtmlEncode(txtName.Text.Trim());
                        jvTemplateObj.VLH_DESC = string.Empty;
                        jvTemplateObj.ACTIVE = chkActive.Checked == true ? 1 : 0;
                        jvTemplateObj.USER_PK = currentUser.PKUser;
                        jvTemplateObj.LAST_MOD_DT = LastModifiedTime.ToString();
                        List<JVTemplateDetails> detailsList = new List<JVTemplateDetails>();
                        foreach (JVTemplateDetailsList item in jvTemplateDetailslist)
                        {
                            JVTemplateDetails objJVTemplateDetails = new JVTemplateDetails();
                            objJVTemplateDetails.VLD_PK=item.VLD_PK;
                            objJVTemplateDetails.VLD_MODE =item.VLD_MODE;
                            objJVTemplateDetails.VLD_ACCOUNT =item.VLD_ACCOUNT;
                            objJVTemplateDetails.VLD_SEQUENCE = item.VLD_SEQUENCE;
                            objJVTemplateDetails.VLD_REF_TYPE = item.VLD_REF_TYPE;
                            objJVTemplateDetails.VLD_REF_TYPE_PK = item.VLD_REF_TYPE_PK;
                            detailsList.Add(objJVTemplateDetails);
                        }
                        jvTemplateObj.Detail = detailsList;
                        returnObj = jvTemplateObj;
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
                if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["VLH_NAME"].ToString());
                    ddlType.SelectedValue = dsPageData.Tables[0].Rows[0]["VLH_TYPE"].ToString();
                    LastModifiedTime = Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["VLH_MOD_ON"].ToString());
                    chkActive.Checked = dsPageData.Tables[0].Rows[0]["VLH_ACTIVE"].ToString() == "1" ? true : false;
                    int i=0;
                    foreach (DataRow row in dsPageData.Tables[0].Rows)
                    {
                        JVTemplateDetailsList item = new JVTemplateDetailsList();
                        item.COA_CODE = dsPageData.Tables[0].Rows[i]["COA_CODE"].ToString();
                        item.COA_NAME = dsPageData.Tables[0].Rows[i]["COA_NAME"].ToString();
                        item.VLD_PK = Convert.ToInt32(dsPageData.Tables[0].Rows[i]["VLD_PK"].ToString());
                        item.VLD_MODE = Convert.ToInt32(dsPageData.Tables[0].Rows[i]["VLD_MODE"].ToString());
                        item.VLD_ACCOUNT = Convert.ToInt32(dsPageData.Tables[0].Rows[i]["VLD_ACCOUNT"].ToString());
                        item.VLD_SEQUENCE = Convert.ToInt16(dsPageData.Tables[0].Rows[i]["VLD_SEQUENCE"].ToString());
                        item.SLNO = i;
                        jvTemplateDetailslist.Add(item);
                        i++;
                    }
                    slno = i;
                    BindGrid(ControlsEnum.TEMPLATESDETAILS);
                }
                //dtCustomerMails = dsPageData.Tables[1];
                //CustomerMails = dtCustomerMails;
                //mailSelecte = true;
                //SetFieldValues(ControlsEnum.CUSTOMEREMAIL);
                //txtSubject.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CMH_SUBJECT"].ToString());
                //txtContent.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CMH_CONTENT"].ToString());
                //LastModifiedTime = Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["CMH_MOD_DT"].ToString());
                //lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind Dropdown List
        /// </summary>
        /// <param name="type"></param>
        private void BindDropdown(ControlsEnum type)
        {
            switch (type)
            {
                #region Type
                case ControlsEnum.TYPE:
                ddlType.Items.Clear();
                ddlSType.Items.Clear();
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    ddlType.DataSource = dtType;
                    ddlType.DataTextField = Resources.DataFieldRes.ConstName;
                    ddlType.DataValueField = Resources.DataFieldRes.ConstPK;
                    ddlType.DataBind();
                }
                ddlType.Items.Insert(0, new ListItem(GTIService.Constants.Common.CommonConstants.SELECTTEXT, ERP.Utilities.CommonConstants.SELECTVAL));

                if (dtType != null && dtType.Rows.Count > 0)
                {
                    ddlSType.DataSource = dtType;
                    ddlSType.DataTextField = Resources.DataFieldRes.ConstName;
                    ddlSType.DataValueField = Resources.DataFieldRes.ConstPK;
                    ddlSType.DataBind();
                }
                ddlSType.Items.Insert(0, new ListItem(GTIService.Constants.Common.CommonConstants.SELECTTEXT, ERP.Utilities.CommonConstants.SELECTVAL));

               break;
                #endregion

            }
        }
        /// <summary>
        ///  Method for Bind Grid
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.DEFAULT:
                    if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                    {
                        if (dsPageData.Tables[0].Rows.Count > 0)
                            TotalPages = Convert.ToInt32(dsPageData.Tables[0].Rows.Count.ToString()) > grdJVList.PageSize ? Convert.ToInt32(dsPageData.Tables[0].Rows.Count.ToString()) / grdJVList.PageSize : 0;
                        else
                            TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        PageIndex = PageIndex == null ? "0" : PageIndex;
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdJVList.PageIndex = Convert.ToInt32(PageIndex);
                        DataView dv = new DataView(dsPageData.Tables[0]);
                        if (SortBy != null)
                        {
                            dv.Sort = SortBy + " " + SortDirection;
                        }
                        grdJVList.DataSource = dv;
                        //grdJVList.DataSource = dsPageData.Tables[0];
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                    }
                    else
                    {
                        PageIndex = null;
                        TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.Visible = false;
                        uclPaging.BindPager();
                        grdJVList.DataSource = null; ;
                    }
                    grdJVList.DataBind();
                    break;
                case ControlsEnum.TEMPLATESDETAILS:
                    jvTemplateDetailslist = jvTemplateDetailslist.OrderByDescending(c => c.VLD_MODE).ThenBy(c => c.VLD_SEQUENCE).ToList();
                    grdJvItems.DataSource = jvTemplateDetailslist;
                    grdJvItems.DataBind();
                    if (grdJvItems.Rows.Count > 0)
                    {
                        GridViewRow FirstRow = grdJvItems.Rows[0];
                        ImageButton btnUp = (ImageButton)FirstRow.FindControl("btnGUP");
                        btnUp.Visible = false;
                        GridViewRow LastRow = grdJvItems.Rows[grdJvItems.Rows.Count - 1];
                        ImageButton btnDown = (ImageButton)LastRow.FindControl("btnGDown");
                        btnDown.Visible = false;
                    }
                    break;
            }
        }
        private void ClearForm(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CLEARALL:
                    LastModifiedTime = System.DateTime.Now;
                    EntryStatus = EntryStatus.LISTMODE;
                    txtName.Text = string.Empty;
                    ddlType.SelectedValue = CommonConstants.SELECTVAL;
                    txtAccount.Text = string.Empty;
                    hdfAccount.Value = "0";
                    chkActive.Checked = true;
                    hdfVLDPK.Value =string.Empty ;
                    hdfSeq.Value = string.Empty;
                    hdfRType.Value = string.Empty;
                    hdfRefPK.Value = string.Empty;
                    hdfSNO.Value = string.Empty;
                    slno = 0;
                    jvTemplateDetailslist = new List<JVTemplateDetailsList>();
                    BindGrid(ControlsEnum.TEMPLATESDETAILS);
                    ddlSType.SelectedValue = CommonConstants.SELECTVAL;
                    ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
                    txtTName.Text = string.Empty;
                    break;
                case ControlsEnum.DEFAULT:
                    //LastModifiedTime = System.DateTime.Now;
                    //lblLastModifiedHDR.Text = string.Empty;
                    //txtContent.Text = string.Empty;
                    //txtSubject.Text = string.Empty;
                    //txtTo.Text = string.Empty;
                    //CurrPK = 0;
                    //ClearTree();
                    EntryStatus = EntryStatus.LISTMODE;
                    break;
                case ControlsEnum.TMPDTLS:
                    txtAccount.Text = string.Empty;
                    hdfAccount.Value = "0";
                    ddlMode.SelectedValue = "0";
                    hdfSNO.Value = string.Empty;
                    hdfVLDPK.Value = string.Empty;
                    hdfSeq.Value = string.Empty;
                    hdfRType.Value = string.Empty;
                    hdfRefPK.Value = string.Empty;
                    break;
            }
        }
        /// <summary>
        /// Reset Form
        /// </summary>
        private void ResetForm()
        {
            ClearForm(ControlsEnum.CLEARALL);
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;
            CurrPK = 0;
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
            string s = "";
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


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
                        // Assign the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assign the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;

                }


                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

        }
        /// <summary>
        /// Method used to enable and Disable Page Navigation Controls
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
            try
            {
                string breadCrumb;
                breadCrumb = this.GetLocalResourceObject("BreadcrumbCreation").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EntryMode", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);

                }
                //else if (EntryStatus == EntryStatus.NEWMODE)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                //}
                if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EntryMode", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(4);});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ListMode", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                    breadCrumb = this.GetLocalResourceObject("BreadcrumbListing").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            
                }
                else if (EntryStatus == EntryStatus.LISTDRAFTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ListMode", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                lblBreadCrum.Text = breadCrumb;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {

        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            CLEARALL,
            TYPE,
            ACCOUNTDETAILS,
            TEMPLATESDETAILS,
            TMPDTLS
        }
        
        #endregion
    }
}