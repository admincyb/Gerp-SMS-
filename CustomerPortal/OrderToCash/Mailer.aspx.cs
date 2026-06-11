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
using BusinessLogic.BrandRates;
using BusinessObject.Mailer;
using BusinessLogic.Mailer;
using ERPSMS_v01.UserControls;
using MailSendCore;
using System.Globalization;
using System.Configuration;

#endregion
namespace CustomerPortal.OrderToCash
{
    public partial class Mailer : ERP.Store.UI.MyBasePage
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
        /// Current PK
        /// </summary>
        private int CusPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CusPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CusPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CusPK] = value;
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

        /// <summary>
        /// Keep Customer Rate Dataset
        /// </summary>
        private DataSet CustomerRates
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerRates] == null ? null : (DataSet)this.ViewState[ViewstateStrings.CustomerRates];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerRates] = value;
            }

        }
        /// <summary>
        /// Keep Customer Mails
        /// </summary>
        private DataTable CustomerMails
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerMails] == null ? null : (DataTable)this.ViewState[ViewstateStrings.CustomerMails];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerMails] = value;
            }
        }
        /// <summary>
        /// Customer List
        /// </summary>
        private List<CustomerBO> CustomerEmailList
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerEmailList] == null ? null : (List<CustomerBO>)this.ViewState[ViewstateStrings.CustomerEmailList];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerEmailList] = value;
            }
          
        }
        /// <summary>
        /// Status
        /// </summary>
        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.Status]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        /// <summary>
        /// Keep Currency List in viewstate
        /// </summary>
        private DataTable Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? null : (DataTable)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }

        }
        /// <summary>
        /// Brand PK
        /// </summary>
        private int BrkPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.BrkPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.BrkPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BrkPK] = value;
            }
        }


        #endregion
        User currentUser;
        private ActionsEnum commonActions;
        DataTable dtCustomers;
        DataTable dtCustomerMails;
        MailDetailsBO mailDetailsObj;
        DataSet dsPageData;
        XmlDocument xmlDoc;
        bool mailSelecte = false;
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
                    EntryStatus = EntryStatus.LISTMODE;
                    Status = (int)MailStatus.SEND;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    PageSize = Convert.ToInt32(grdMailList.PageSize);
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
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
            int appTypePK = 0;
            try
            {
                switch (type)
                {
                    case ControlsEnum.TYPE:
                        BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "CRM MAIL TYPE");
                        break;
                    case ControlsEnum.MAILDTL:
                        dsPageData = MailerBL.GetMailQueue(CurrPK,currentUser.SBUID);
                        break;
                    #region Default
                    case ControlsEnum.DEFAULT:
                        appTypePK = hdfType.Value==string.Empty?0:Convert.ToInt32(hdfType.Value);
                        Status = (txtMailStatus.Text == "Select/Type" || hdfMailStatus.Value=="-1" || txtMailStatus.Text ==string.Empty) ? -1 : Convert.ToInt32(hdfMailStatus.Value);
                        dsPageData = MailerBL.GetMailQList(new BusinessObject.GridPrams()
                            {
                                PageSize = this.PageSize,
                                PageNumber = Convert.ToInt32(this.PageIndex),
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                            },
                            string.IsNullOrEmpty(hdfSendTo.Value.Trim()) ? 0 : Convert.ToInt32(hdfSendTo.Value), string.IsNullOrEmpty(hdfParty.Value.Trim()) ? 0 : Convert.ToInt32(hdfParty.Value), appTypePK, Status, txtSerachSubject.Text.Trim());
                        break;
                    #endregion
                    case ControlsEnum.CUSTOMERS:
                        dtCustomers = BrandRatesBL.GetCustomerList(currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE));
                        break;
                    case ControlsEnum.CUSTOMEREMAIL:
                        mailDetailsObj = (MailDetailsBO)SetUIValuesToObject(ActionsEnum.CUSTOMERAPPLY);
                        if (mailDetailsObj.Email.Count > 0)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXml(mailDetailsObj);
                            dtCustomerMails = MailerBL.GetCustomerMails(xmlDoc.InnerXml);
                            CustomerMails = dtCustomerMails;
                            mailSelecte = true;
                        }
                        else
                            mailSelecte = false;

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
                foreach (GridViewRow grdrow in grdMailList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                         switch (Status)
                        {
                            case (int)MailStatus.DRAFT:
                                EntryStatus= EntryStatus.ENTRYMODE;
                               // btnDelete.Visible = true;
                                imbAddContact.Visible = false;
                                break;
                            case (int)MailStatus.SEND:
                                EntryStatus = EntryStatus.VIEWMODE;
                                break;
                         }

                            HiddenField hdfCurrPK;
                            HiddenField hdfcusPK;
                            hdfCurrPK = (HiddenField)grdrow.FindControl("hdfCurPK");
                            hdfcusPK = (HiddenField)grdrow.FindControl("hdfCusPK");
                            CurrPK = hdfCurrPK != null ? !string.IsNullOrEmpty(hdfCurrPK.Value) ? Convert.ToInt32(hdfCurrPK.Value) : -1 : -1;
                            CusPK = hdfcusPK != null ? !string.IsNullOrEmpty(hdfcusPK.Value) ? Convert.ToInt32(hdfcusPK.Value) : -1 : -1;
                            GetFieldValues(ControlsEnum.MAILDTL);
                            switch (mode)
                            { 
                                case ActionsEnum.EDIT:
                                    SetFieldValues(ControlsEnum.MAILDTL);
                                    break;
                                case ActionsEnum.VIEW:
                                    SetFieldValues(ControlsEnum.VIEW);
                                    break;
                            }
                            
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
            string toAddress = "";
            switch (type)
            {
                case ControlsEnum.CUSTOMERS:
                    BindTree(ControlsEnum.CUSTOMERS);
                    break;
                case ControlsEnum.DEFAULT:
                    BindGrid(ControlsEnum.DEFAULT);
                    //btnEdit.Visible = false;
                    //btnView.Visible = true;
                    break;
                case ControlsEnum.CUSTOMEREMAIL:
                    if (mailSelecte)
                    {
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            if (dtCustomerMails != null && dtCustomerMails.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtCustomerMails.Rows)
                                {
                                    if (dr["CUS_PK"].ToString() == CusPK.ToString())
                                    {
                                        toAddress += dr["CUS_EMAIL"].ToString() + ",";
                                        break;
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (dtCustomerMails != null && dtCustomerMails.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtCustomerMails.Rows)
                                {
                                    toAddress += dr["CUS_EMAIL"].ToString() + ",";
                                }

                            }
                        }
                        if (toAddress.Length > 1)
                        {
                            toAddress = toAddress.Substring(0, toAddress.LastIndexOf(','));

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("noEmailID").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        txtTo.Text = toAddress;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup2", "ClosePopup();", true);
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("selectCustomer").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup2", "ClosePopup();", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCustomers]','" + GetLocalResourceObject("CustomerList").ToString() + "','400','300');", true);
                    
                    }
                    break;
                case ControlsEnum.MAILDTL:
                    if (dsPageData != null && dsPageData.Tables.Count>0)
                    {
                       
                            GetUIValuesFromObject();
                    }
                    break;
                case ControlsEnum.VIEW:
                    if (dsPageData != null && dsPageData.Tables.Count > 0)
                    {
                        if ( MailQSubType.EMPLOYEEMAIL ==Convert.ToString(dsPageData.Tables[0].Rows[0]["MLQ_APP_SUB_TYPE"]))
                        {
                            litErrorMsg.Text = GetLocalResourceObject("ErrEmployeeMail").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup2", "ClosePopup();", true);
                        }
                        else
                        {
                            lblMailSubject.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_SUBJECT"].ToString());
                            ltContent.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_CONTENT"].ToString());
                            lblToMail.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_TO"].ToString());

                            lblCc.Text = HttpUtility.HtmlDecode(Convert.ToString(dsPageData.Tables[0].Rows[0]["MLQ_TO_CC"]));
                            lblBcc.Text = HttpUtility.HtmlDecode(Convert.ToString(dsPageData.Tables[0].Rows[0]["MLQ_TO_BCC"]));

                            DateTime AttempTime = string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["MLQ_ATTEMPT_ON"].ToString()) ? DateTime.Now : Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["MLQ_ATTEMPT_ON"].ToString());
                            lblAttempTime.Text = AttempTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            lblMailSendStatus.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_STATUS_TEXT"].ToString());
                            SetFieldValues(ControlsEnum.MAILATTACHMENT);
                            EntryStatus = EntryStatus.LISTMODE;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPopMail", "ShowContainerDiv('[id$=divPopUp]','" + GetLocalResourceObject("Mail").ToString() + "','600','400');", true);
                        }
                    }
                    break;
                case ControlsEnum.MAILATTACHMENT:
                    BindGrid(ControlsEnum.MAILATTACHMENT);
                    //btnEdit.Visible = false;
                    //btnView.Visible = true;
                    break;
                //case ControlsEnum.PRODUCTDTL:
                //    BindGrid(type);
                //    break;
              
            
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
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;

            int result;
            result = 0;
            GridViewRow gvr;
            GridView grd;
            string arg;
            string saveXml;
            string action;
            DataRow[] drr;
            int applicationPK = 0;

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
                    #region New
                    case ActionsEnum.NEW:
                        CustomerMails = null;
                        ClearTree();
                        EntryStatus = EntryStatus.ENTRYMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(ActionsEnum.EDIT);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(ActionsEnum.VIEW);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ClearForm(ControlsEnum.DEFAULT);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CUSTOMERCANCEL:
                        ClearTree();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        //EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Customer Email
                    case ActionsEnum.CUSTOMEREMAIL:
                        GetFieldValues(ControlsEnum.CUSTOMEREMAIL);
                        SetFieldValues(ControlsEnum.CUSTOMEREMAIL);
                       
                        break;
                    #endregion
                    #region Get Graft Items
                    case ActionsEnum.DRAFTITEMS: 
                        Status = (int)MailStatus.DRAFT;
                        SetLinkStatus(commonActions);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTDRAFTMODE;
                        //btnEdit.Visible = true;
                        //btnView.Visible = false;
                        break;
                    #endregion
                    #region Get Send Items
                    case ActionsEnum.SENDITEMS:
                        Status = (int)MailStatus.SEND;
                        SetLinkStatus(commonActions);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        //btnEdit.Visible = false;
                        //btnView.Visible = true;
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
                            result = MailerBL.DeleteMail(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                ClearForm(ControlsEnum.CLEARALL);
                                EntryStatus = EntryStatus.LISTDRAFTMODE;
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
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
                                    litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
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
                            mailDetailsObj = (MailDetailsBO)SetUIValuesToObject(ActionsEnum.SAVE);
                            mailDetailsObj.CMH_STATUS = CommonConstants.SELECT_VALUE_ZERO;
                            if (mailDetailsObj != null)
                            {
                                dtCustomerMails = CustomerMails;
                                bool hasPK = dtCustomerMails.Columns.Contains("MLQ_PK");
                                foreach (DataRow dr in dtCustomerMails.Rows)
                                {
                                    applicationPK = hasPK == true ? Convert.ToInt32(dr["MLQ_PK"].ToString()) : 0;
                                    result = CommonFunctions.SaveMailQue(txtSubject.Text, txtContent.Text.Replace("\n", "<br />"), dr["CUS_EMAIL"].ToString(), applicationPK, currentUser.SBUID, currentUser.PKUser, ApplicationType.GeneralMail, 1, Convert.ToInt32(dr["CUS_PK"].ToString()),
                                        (int)MailStatus.DRAFT, 0, LastModifiedTime, string.Empty, string.Empty, ConfigurationManager.AppSettings["FrmMailCusMailer"]);
                                }
                                if (result > 0) // Success ! re-initialize the page
                                {
                                    ResetForm();
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
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
                                        litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                          
                            }
                        }

                        break;
                    #endregion
                    #region Re-Send
                    case ActionsEnum.RESEND:
                       result =MailSendManager.SaveMailDetails(0,CurrPK);
                       if (result > 0) // Success ! re-initialize the page
                       {
                           ResetForm();
                           //Show Save success message and reset Contract Entry
                           litErrorMsg.Text = Resources.ErrorMessages.Msg_Send_Success;
                           litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
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
                               litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.EditUsedByAnotherUser;
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                               + "','" + Resources.ErpRes.Information + "');", true);
                           }
                           else if (result == (int)DbSaveStatus.CODEEXIST)
                           {
                               litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                               + "','" + Resources.ErpRes.Information + "','" + "');", true);
                           }
                           else
                           {
                               litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                               litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                           }
                       }
                       ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup2", "ClosePopup();", true);
                            
                        break;
                    #endregion
                    #region Send
                    case ActionsEnum.SEND:
                        //Save Details
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            mailDetailsObj = (MailDetailsBO)SetUIValuesToObject(ActionsEnum.SEND);
                            mailDetailsObj.CMH_STATUS = CommonConstants.SELECT_VALUE_ONE;
                            if (mailDetailsObj != null)
                            {
                               dtCustomerMails = CustomerMails;
                               bool hasPK= dtCustomerMails.Columns.Contains("MLQ_PK");
                                foreach (DataRow dr in dtCustomerMails.Rows)
                                {
                                    applicationPK = hasPK == true ? Convert.ToInt32(dr["MLQ_PK"].ToString()) : 0;

                                    result = CommonFunctions.SaveMailQue(txtSubject.Text, txtContent.Text.Replace("\n", "<br />"), dr["CUS_EMAIL"].ToString(), applicationPK, currentUser.SBUID, currentUser.PKUser, ApplicationType.GeneralMail, 1,
                                        Convert.ToInt32(dr["CUS_PK"].ToString()), (int)MailStatus.SEND, 0, LastModifiedTime, txtCCAddress.Text, txtBCCAddress.Text, ConfigurationManager.AppSettings["FrmMailCusMailer"]);
                                }
                                if (result > 0) // Success ! re-initialize the page
                                {
                                    ResetForm();
                                    //Show Save success message and reset 
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
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
                                        litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }

                        }

                        break;
                    #endregion
                    #region POPUP
                    #region Show Customers
                    case ActionsEnum.SHOWCUSTOMER:
                        if (trvCustomers.Nodes.Count == 0)
                        {
                            GetFieldValues(ControlsEnum.CUSTOMERS);
                            BindTree(ControlsEnum.CUSTOMERS);
                        }
                        else 
                        {
                            
                           dtCustomerMails=CustomerMails;
                           if (dtCustomerMails != null && dtCustomerMails.AsEnumerable().First().Table.Columns.Contains("PARTY_PK"))
                            foreach (DataRow dr in dtCustomerMails.Rows)
                                foreach (TreeNode node in trvCustomers.Nodes)
                                {
                                    foreach (TreeNode child1 in node.ChildNodes)
                                    {
                                        if (child1.Value == dr["PARTY_PK"].ToString())
                                            child1.Checked = true;
                                    }
                                }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCustomers]','" + GetLocalResourceObject("CustomerList").ToString() + "','400','300');", true);
                        break;
                    #endregion
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
                //else if ((sender as GridView).ID == "grdSelectdCustomers")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow)
                //    {
                //        if (dsCustomerRate != null && dsCustomerRate.Tables[1].Rows.Count > 0)
                //        {
                //            HiddenField hdfCurrPK = e.Row.FindControl("hdfCurrPK") as HiddenField;
                //            ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                //            //ddlCurrency.DataSource = CommonBL.GetCurrencyList(currentUser.SBUID);
                //            ddlCurrency.DataSource = Currency;
                //            ddlCurrency.DataTextField = "CUR_CODE";
                //            ddlCurrency.DataValueField = "CUR_PK";
                //            ddlCurrency.ToolTip = "CUR_NAME";
                //            ddlCurrency.DataBind();
                //            ddlCurrency.SelectedValue = hdfCurrPK.Value;

                //        }
                //    }
                //}
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
            SortBy = e.SortExpression;
            if (SortDirection == Resources.Report.SortAscending)
                SortDirection = Resources.Report.SortDescending;
            else
                SortDirection = Resources.Report.SortAscending;
         
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
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
                    case ActionsEnum.CUSTOMERAPPLY:
                        mailDetailsObj = new MailDetailsBO();
                        mailDetailsObj.ACTIVE = Convert.ToInt32(DbActiveStatus.HASPK);
                        mailDetailsObj.BIZUNIT_PK = currentUser.SBUID;
                        mailDetailsObj.Email  = new List<CustomerBO>();
                        List<CustomerBO> EmailList = new List<CustomerBO>();
                        CustomerBO objCustomer;
                        foreach (TreeNode node in trvCustomers.Nodes)
                        {
                            foreach (TreeNode child1 in node.ChildNodes)
                            {
                                if (child1.Checked)
                                {
                                    objCustomer = new CustomerBO();
                                    objCustomer.CUS_PK = Convert.ToInt32(child1.Value);
                                    EmailList.Add(objCustomer);
                                }
                            }
                        }
                        mailDetailsObj.Email = EmailList;
                        returnObj = mailDetailsObj;
                        break;
                    #region Save Details
                    case ActionsEnum.SAVE:
                    case ActionsEnum.SEND:
                        mailDetailsObj = new MailDetailsBO();
                        mailDetailsObj.ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        mailDetailsObj.BIZUNIT_PK = currentUser.SBUID;
                        mailDetailsObj.CMH_DATE =System.DateTime.Now.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        mailDetailsObj.CMH_PK = CurrPK;
                        mailDetailsObj.CMH_SUBJECT = HttpUtility.HtmlEncode(txtSubject.Text);
                        mailDetailsObj.CMH_CONTENT = HttpUtility.HtmlEncode(txtContent.Text).Replace("\n", "<br />");
                        mailDetailsObj.LAST_MOD_DT=LastModifiedTime.ToString();
                        mailDetailsObj.USER_PK=currentUser.PKUser.ToString();
                        mailDetailsObj.Detail = new List<MailDetails>();
                        List<MailDetails> detailsList = new List<MailDetails>();
                        MailDetails objMailDetail;
                        foreach (TreeNode node in trvCustomers.Nodes)
                        {
                            foreach (TreeNode child1 in node.ChildNodes)
                            {
                                if (child1.Checked)
                                {
                                    objMailDetail= new MailDetails();
                                    objMailDetail.CMD_CM = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                                    objMailDetail.CMD_PK = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                                    objMailDetail.CMD_CUSTOMER = Convert.ToInt32(child1.Value);
                                    detailsList.Add(objMailDetail);
                                }
                            }
                        }
                        mailDetailsObj.Detail = detailsList;
                        returnObj = mailDetailsObj;

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
                if (trvCustomers.Nodes.Count == 0)
                {
                    GetFieldValues(ControlsEnum.CUSTOMERS);
                    BindTree(ControlsEnum.CUSTOMERS);
                }
                foreach (TreeNode node in trvCustomers.Nodes)
                {
                    foreach (TreeNode child1 in node.ChildNodes)
                    {
                            if (child1.Value == dsPageData.Tables[0].Rows[0]["PARTY_PK"].ToString())
                            {
                                child1.Checked = true;
                                break;
                            }
                    }
                }
                dtCustomerMails = dsPageData.Tables[0];
                CustomerMails = dtCustomerMails;
                mailSelecte = true;
              //  SetFieldValues(ControlsEnum.CUSTOMEREMAIL);
                txtSubject.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_SUBJECT"].ToString());
                txtContent.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_CONTENT"].ToString()).Replace("<br />","\n");
                txtTo.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["MLQ_TO"].ToString());
                LastModifiedTime = dsPageData.Tables[0].Rows[0]["MLQ_MOD_DT"]==null?DateTime.Now : Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["MLQ_MOD_DT"].ToString());
                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind Tree view
        /// </summary>
        /// <param name="type"></param>
        private void BindTree(ControlsEnum type)
        {
            TreeNode child;
            TreeNode root;
            switch (type)
            {
                #region Customer
                case ControlsEnum.CUSTOMERS:
                    trvCustomers.Nodes.Clear();
                    root = new TreeNode(GetLocalResourceObject("AllCustomers").ToString(), "0");
                    root.ShowCheckBox = true;
                    root.NavigateUrl = "javascript:return false;";
                    trvCustomers.Nodes.Add(root);
                    if (dtCustomers != null)
                    {
                        foreach (DataRow row in dtCustomers.Rows)
                        {
                            child = new TreeNode(row["CUS_NAME"].ToString(), row["CUS_PK"].ToString());
                            child.ShowCheckBox = true;
                            child.NavigateUrl = "javascript:return false;";
                            child.ToolTip = row["CUS_NAME"].ToString();
                            root.ChildNodes.Add(child);
                        }
                        root.ExpandAll();
                    }
                    break;
                #endregion
                #region Products
              //  case ControlsEnum.PRODUCTS:
                    //trvProducts.Nodes.Clear();
                    //root = new TreeNode(GetLocalResourceObject("AllProducts").ToString(), "0");
                    //root.ShowCheckBox = true;
                    //trvProducts.Nodes.Add(root);
                    //if (dtProducts != null)
                    //{
                    //    foreach (DataRow row in dtProducts.Rows)
                    //    {
                    //        child = new TreeNode(row["ITM_CODE"].ToString(), row["ITM_PK"].ToString());
                    //        child.ShowCheckBox = true;
                    //        child.ToolTip = row["ITM_NAME"].ToString();
                    //        root.ChildNodes.Add(child);
                    //    }
                    //    root.ExpandAll();
                    //}
                  //  break;
                #endregion
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
              
            }
        }
        /// <summary>
        /// Set Link CSS
        /// </summary>
        /// <param name="type"></param>
        private void SetLinkStatus(ActionsEnum type)
        {
            switch (type)
            {
                case ActionsEnum.DRAFTITEMS:
                        //lnkSendItems.CssClass ="mailsend-icon";
                        //lnkDraftItems.CssClass = "maildraft-icon-active";
                    break;
                case ActionsEnum.SENDITEMS:
                        //lnkSendItems.CssClass = "mailsend-icon-active";
                        //lnkDraftItems.CssClass = "maildraft-icon";
                        break;

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
                        {
                           // TotalPages = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROWS"].ToString()) > grdMailList.PageSize ? Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROWS"].ToString()) / grdMailList.PageSize : 0;
                            decimal pages = Convert.ToDecimal(Convert.ToDecimal(dsPageData.Tables[0].Rows[0]["TOTAL_ROWS"].ToString()) / Convert.ToDecimal(grdMailList.PageSize.ToString()));
                            TotalPages = Convert.ToInt32(Math.Ceiling(pages));
                        }
                        else
                            TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdMailList.PageIndex = Convert.ToInt32(PageIndex);
                        grdMailList.DataSource = dsPageData.Tables[0];
                        uclPaging.Visible = true;
                        uclPaging.BindPager();

                    }
                    else
                    {
                        PageIndex ="1";
                        TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.Visible = false;
                        uclPaging.BindPager();
                        grdMailList.DataSource = null; ;
                    }
                    grdMailList.DataBind();
                    break;
                case ControlsEnum.MAILATTACHMENT:
                    if (dsPageData != null && dsPageData.Tables.Count > 1 && dsPageData.Tables[1].Rows.Count > 0)
                    {
                        divAttachment.Visible = true;
                        grdMailAttachments.DataSource = dsPageData.Tables[1];                       
                    }
                    else
                    {
                        divAttachment.Visible = false;
                        grdMailAttachments.DataSource = null;                        
                    }
                    grdMailAttachments.DataBind();
                    break;
            }
        }
        private void ClearForm(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CLEARALL:
                    LastModifiedTime = System.DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    EntryStatus = EntryStatus.LISTMODE;
                    txtContent.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    txtTo.Text = string.Empty;
                    imbAddContact.Visible = true;
                    txtSerachSubject.Text = string.Empty;
                   // txtSearchSubject.Text = string.Empty;
                    txtParty.Text = string.Empty;
                    hdfParty.Value = "0";
                    txtSendTo.Text = string.Empty;
                    hdfSendTo.Value = "0";
                    txtType.Text = string.Empty;
                    hdfType.Value = "0";
                    txtMailStatus.Text = string.Empty;
                    hdfMailStatus.Value = "-1";
                   // txtSearchSubject.Text = string.Empty;
                    CurrPK = 0;
                    ClearTree();
                   // btnDelete.Visible = false;
                    CustomerMails = null;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    //"", "", "", ""
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    break;
                case ControlsEnum.DEFAULT:
                    LastModifiedTime = System.DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    if (Status == (int)MailStatus.DRAFT)
                        EntryStatus = EntryStatus.LISTDRAFTMODE;
                    else
                        EntryStatus = EntryStatus.LISTMODE;
                    txtContent.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    txtTo.Text = string.Empty;
                    CurrPK = 0;
                    ClearTree();
                  //  btnDelete.Visible = false;
                    break;
            }
        }

        /// <summary>
        /// Clear Tree
        /// </summary>
        private void ClearTree()
        {

            //Clear Customer
            foreach (TreeNode node in trvCustomers.Nodes)
            {
                foreach (TreeNode child1 in node.ChildNodes)
                {
                    child1.Checked = false;
                }
            }
        }
        /// <summary>
        /// Reset Form
        /// </summary>
        private void ResetForm()
        {
            ClearForm(ControlsEnum.CLEARALL);
            Status = (int)MailStatus.SEND; 
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;
            SetLinkStatus(ActionsEnum.SENDITEMS);
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
                }
                else if (EntryStatus == EntryStatus.LISTDRAFTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ListMode", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
        /// Mail Queue SubType
        /// </summary>
        /// 
        private static class MailQSubType
        {
            public const string EMPLOYEEMAIL = "7078";
        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            CLEARALL,
            CUSTOMERS,
            CUSTOMERSEDIT,
            CUSTOMEREMAIL,
            MAILDTL,
            TYPE,
            VIEW,
            MAILATTACHMENT
        }

        private enum MailStatus
        { 
          DRAFT=4,
          SEND=0
        }
        #endregion
    }
}