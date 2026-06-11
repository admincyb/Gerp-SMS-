using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using System.Data;

namespace ERPSMS_v01.UserControls
{
    public partial class TransactionComments : System.Web.UI.UserControl
    {
        #region Deligates
        public event EventHandler AfterCommentControlEvent;
        #endregion
        #region Variables
        private ActionsEnum commonActions;
        BusinessObject.User currentUser;
        TrxCommentBO objTrxComment;
        DataTable dtCommentsList;
        DataTable dtResult;
        #endregion
        #region Properties
        public string AppName
        {
            get { return Convert.ToString(this.ViewState[ViewstateStrings.ApplicationName]); }
            set
            { 
                this.ViewState[ViewstateStrings.ApplicationName] = value;
                lblTrx.Text = value;
            }
        }
        public string AppType
        {
            get { return Convert.ToString(this.ViewState[ViewstateStrings.ApplicationType]); }
            set { this.ViewState[ViewstateStrings.ApplicationType] = value; }
        }
        public int TrxPk
        {
            get { return this.ViewState[ViewstateStrings.TransactionPk] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.TransactionPk]); }
            set { this.ViewState[ViewstateStrings.TransactionPk] = value; }
        }
        public string TrxNo
        {
            get { return Convert.ToString(this.ViewState[ViewstateStrings.TransactionNo]); }
            set 
            { 
                this.ViewState[ViewstateStrings.TransactionNo] = value;
                lblTrxNo.Text = value;
            }
        }
        /// <summary>
        /// Current Comment PK
        /// </summary>
        private int CurrCmntPK
        {
            get { return this.ViewState[ViewstateStrings.CurrCmntPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrCmntPK]); }
            set { this.ViewState[ViewstateStrings.CurrCmntPK] = value; }
        }

        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        private DateTime CmntLastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.CmntLastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.CmntLastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.CmntLastModifiedTime] = value;
            }
        }
        /// <summary>
        /// To maintain the ViewType in viewstate
        /// </summary>
        public int ViewType
        {
            get { return this.ViewState["ViewType"] == null ? 1 : Convert.ToInt32(this.ViewState["ViewType"]); }
            set { this.ViewState["ViewType"] = value; }            
        }
        #endregion
        #region Page Events
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    InitializeControl();                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }

        public void InitializeControl()
        {
            ResetForm(ControlsEnum.SAVE);
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            ViewAction();
        }
        #endregion
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            TrxCommentBO objTrxCmnt = new TrxCommentBO();
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        objTrxCmnt.ACM_APP_TYPE = AppType;
                        objTrxCmnt.ACM_APP_TRX_PK = TrxPk;
                        objTrxCmnt.BIZUNIT = currentUser.SBUID;
                        objTrxCmnt.USER_PK = currentUser.PKUser;
                        if (!string.IsNullOrEmpty(AppType.Trim()))
                            dtCommentsList = BusinessLogic.CommonManagement.CommonBL.GetTransactionComments(objTrxCmnt);
                        break;
                    #endregion
                    #region COMMENT DETAILS
                    case ControlsEnum.COMMENTDETAILS:
                        objTrxCmnt.ACM_PK = CurrCmntPK;
                        objTrxCmnt.ACM_APP_TYPE = AppType;
                        objTrxCmnt.ACM_APP_TRX_PK = TrxPk;
                        objTrxCmnt.BIZUNIT = currentUser.SBUID;
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetTransactionComments(objTrxCmnt);
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
                    case ControlsEnum.COMMENTDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.LIST:
                        BindGrid(controlType);
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
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region SAVE
                    case ControlsEnum.SAVE:
                        TrxCommentBO objCmnt = new TrxCommentBO();
                        objCmnt.ACM_PK = CurrCmntPK;
                        objCmnt.ACM_APP_TRX_CODE = TrxNo;
                        objCmnt.ACM_APP_TRX_PK = TrxPk;
                        objCmnt.ACM_APP_TYPE = AppType;
                        objCmnt.ACM_COMMENT = HttpUtility.HtmlEncode(txtTrxComments.Text);
                        objCmnt.BIZUNIT = currentUser.SBUID;
                        objCmnt.LAST_MOD_DT = CmntLastModifiedTime;
                        objCmnt.USER_PK = currentUser.PKUser;
                        objCmnt.ACM_DATE = Convert.ToDateTime(txtTrxCmntDate.Text);
                        retObject = objCmnt;
                        break;
                    #endregion
                }
                return retObject;
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
                    case ControlsEnum.COMMENTDETAILS:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtTrxCmntDate.Text = Convert.ToDateTime(dtResult.Rows[0]["ACM_DATE"]).ToString(Resources.Constants.DateFormatShort);
                            txtTrxComments.Text = HttpUtility.HtmlDecode(Convert.ToString(dtResult.Rows[0]["ACM_COMMENT"]));
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default:
                    break;
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
                    case ControlsEnum.LIST:
                        if (dtCommentsList != null && dtCommentsList.Rows.Count > 0)
                            grdTrxCommentsList.DataSource = dtCommentsList;
                        else
                            grdTrxCommentsList.DataSource = null;
                        grdTrxCommentsList.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {

                case ControlsEnum.SAVE:
                    CurrCmntPK = 0;
                    txtTrxCmntDate.Text = DateTime.Today.ToString(Resources.Constants.DateFormatShort);
                    txtTrxComments.Text = string.Empty;
                    txtTrxComments.Focus();
                    break;
            }
        }
        /// <summary>
        /// Method to View Actions Control in User Controls
        /// </summary>
        public void ViewAction()
        {
            if (ViewType == 1)
            {
                SEC_UcrTransCommentsPanel.Visible = true;
            }
            else if (ViewType == 0)
            {
                SEC_UcrTransCommentsPanel.Visible = false;
            }
        }
        #endregion
        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int? result;
                result = 0;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GridViewRow grvRow;
                HiddenField hdfCurrCmntPk;
                HiddenField hdfLastModDate;
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
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        objTrxComment = new TrxCommentBO();
                        objTrxComment = (TrxCommentBO)SetUIValuesToObject(ControlsEnum.SAVE);
                        if (objTrxComment != null)
                        {                          
                            result = BusinessLogic.CommonManagement.CommonBL.SaveTransactionComments(objTrxComment);
                            if (result > 0)
                            {
                                ResetForm(ControlsEnum.SAVE);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                AfterCommentControlEvent(sender, e);
                                litErrorMsg.Text = Resources.Messages.Msg_Cmnt_Save_Success;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                
                                
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.Messages.Comments + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }

                        break;
                    #endregion
                    #region EDIT ITEM
                    case ActionsEnum.EDITITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfCurrCmntPk = (HiddenField)grvRow.FindControl("hdfCurrCmntPk");
                        hdfLastModDate = (HiddenField)grvRow.FindControl("hdfLastModDate");
                        Label lblgrdTrxCmntDate = (Label)grvRow.FindControl("lblgrdTrxCmntDate");
                        Label lblgrdTrxCmnt = (Label)grvRow.FindControl("lblgrdTrxCmnt");
                        CurrCmntPK = Convert.ToInt32(hdfCurrCmntPk.Value);
                        CmntLastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                        txtTrxCmntDate.Text = Convert.ToDateTime(lblgrdTrxCmntDate.Text).ToString(Resources.Constants.DateFormatShort);
                        txtTrxComments.Text = lblgrdTrxCmnt.ToolTip;
                        AfterCommentControlEvent(sender, e);
                        break;
                    #endregion
                    #region DELETEITEM
                    case ActionsEnum.DELETEITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfCurrCmntPk = (HiddenField)grvRow.FindControl("hdfCurrCmntPk");
                        hdfLastModDate = (HiddenField)grvRow.FindControl("hdfLastModDate");
                        CurrCmntPK = Convert.ToInt32(hdfCurrCmntPk.Value);
                        CmntLastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                        result = BusinessLogic.CommonManagement.CommonBL.DeleteTransactionComments(CurrCmntPK, CmntLastModifiedTime);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.SAVE);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            AfterCommentControlEvent(sender, e);                        
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Messages.Comments);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                           
                        }
                        else
                        {
                            AfterCommentControlEvent(sender, e);    
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.Messages.Comments + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.Messages.Comments + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.Messages.Comments + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Messages.Comments);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponentsCmnts", "$(document).ready(function(){InitComponentsCmnts();});", true);
        }

        #endregion
        #region --- For Grid Actions----

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {

            try
            {
                #region Grid Fixed Columns
                if ((sender as GridView).ID == "grdTrxCommentsList")
                {
                    //int isClosePO = Convert.ToInt32(hdnViewType.Value);
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton btngrdTrxEditItem = e.Row.FindControl("btngrdTrxEditItem") as ImageButton;
                        ImageButton btngrdTrxRemoveItem = e.Row.FindControl("btngrdTrxRemoveItem") as ImageButton;
                      
                        #region Show/Hide grdPOList image buttons w.r.to previlege
                        if (ViewType == 1)
                        {
                            btngrdTrxEditItem.Visible = true;
                            btngrdTrxRemoveItem.Visible = true;
                        }
                        else if (ViewType == 0)
                        {
                            btngrdTrxEditItem.Visible = false;
                            btngrdTrxRemoveItem.Visible = false;
                        }

                        #endregion
                    }
                }
                #endregion
                if (((GridView)sender).ID == "grdStockTransfer")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }       
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            SAVE,
            LIST,
            COMMENTDETAILS
        }


        #endregion
    }
}