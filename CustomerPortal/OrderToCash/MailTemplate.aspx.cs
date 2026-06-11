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
using ERPManager;
using System.Xml.Linq;

#endregion
namespace CustomerPortal.OrderToCash
{
    public partial class MailTemplate : ERP.Store.UI.MyBasePage
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

        /// <summary>
        /// Application Code
        /// </summary>
        private string ApplicationCode
        {
            get
            {
                return this.ViewState[ViewstateStrings.ApplicationCode] == null ? string.Empty : Convert.ToString(this.ViewState[ViewstateStrings.ApplicationCode]);

            }
            set
            {
                this.ViewState[ViewstateStrings.ApplicationCode] = value;
            }
        }


        #endregion
        User currentUser;
        private ActionsEnum commonActions;
        DataTable dtCustomers;
        DataTable dtCustomerMails;
        MailTemplateBO mailTemplateObj;
        DataSet dsPageData;
        DataSet dsMailTemplate;
        DataTable dtTemplateDetails;
        XmlDocument xmlDoc;
        private TemplateParameterBO objTemplateparams;
        private List<TemplateParameterBO> objTemplateparamsList;
        bool mailSelecte = false;
        private ServiceUtility serviceUtilityObj;
        private bool IsPageChanged = false;
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
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    PageSize = Convert.ToInt32(grdMailTemplate.PageSize);                 
                    GetFieldValues(ControlsEnum.TEMPLATELIST);
                    SetFieldValues(ControlsEnum.TEMPLATELIST);
                    //uclPaging.TotalPages = TotalPages;
                    //uclPaging.CurrentPage = 1;
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
                    #region TEMPLATELIST/DETAILS
                    case ControlsEnum.TEMPLATEDETAILS:
                    case ControlsEnum.TEMPLATELIST:
                         serviceUtilityObj = new ServiceUtility();
                        if (!IsPageChanged)
                        {
                            PageIndex = "1";
                            uclPaging.CurrentPage = 1;
                        }
                        serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        serviceUtilityObj.TotalRecords = 0;
                        short Status = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        if (CurrPK > 0)
                            Status = Convert.ToInt16(DbActiveStatus.HASPK);
                        dsMailTemplate = MailerBL.GetMailTemplate(CurrPK, txtTmplName.Text.TrimStart().TrimEnd(), Status, currentUser.SBUID, 1, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);
                        serviceUtilityObj.TotalRecords = dsMailTemplate.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsMailTemplate.Tables[0].Rows[0]["REC_COUNT"].ToString()) : 0;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;                       

                        break;
                    #endregion                  
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
                foreach (GridViewRow grdrow in grdMailTemplate.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        HiddenField hdfTemplatePk = (HiddenField)grdrow.FindControl("hdfTemplatePk");
                        int curPk = 0;
                        int.TryParse(hdfTemplatePk.Value, out curPk);
                        CurrPK = curPk;
                        GetFieldValues(ControlsEnum.TEMPLATEDETAILS);
                        SetFieldValues(ControlsEnum.TEMPLATEDETAILS);
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
               
                case ControlsEnum.TEMPLATELIST:
                    BindGrid(ControlsEnum.TEMPLATELIST);                 
                    break;
                case ControlsEnum.TEMPLATEPARAMETERS:
                    BindGrid(ControlsEnum.TEMPLATEPARAMETERS);
                    break;        
                case ControlsEnum.TEMPLATEDETAILS:
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
                    #region Cancel/Clear
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.CLEAR:
                        ClearForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.TEMPLATELIST);
                        SetFieldValues(ControlsEnum.TEMPLATELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion                    
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.TEMPLATELIST);
                        SetFieldValues(ControlsEnum.TEMPLATELIST);
                        //EntryStatus = EntryStatus.LISTMODE;
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
                            mailTemplateObj = (MailTemplateBO)SetUIValuesToObject(ActionsEnum.SAVE);
                            if (ApplicationCode == ApplicationType.OUTDUE || ApplicationCode == ApplicationType.SALFRCST)
                            {
                                string[] strParms = mailTemplateObj.TML_TEMPLATE.Split('{');
                                if ((strParms.Length) < grdParameters.Rows.Count)//including subject parameter
                                {
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_Parameter_count").ToString(), grdParameters.Rows.Count);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                               + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                            }
                            if (mailTemplateObj != null)
                            {

                                result = MailerBL.SaveMailTemplate(mailTemplateObj, LastModifiedTime);                                
                                if (result > 0) // Success ! re-initialize the page
                                {
                                    ResetForm();
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailTemplate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text+""
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
                                        litErrorMsg.Text = Resources.PageNameRes.MailTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.MailTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailTemplate);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                          
                            }
                        }

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
                    #region Save Details
                    case ActionsEnum.SAVE:
                        mailTemplateObj = new MailTemplateBO();
                        mailTemplateObj.ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        mailTemplateObj.CRTD_BY = currentUser.PKUser;
                        mailTemplateObj.TML_FROM = txtMailFrom.Text;
                        mailTemplateObj.TML_IS_EDIT = 1;
                        mailTemplateObj.TML_MOD_BY = currentUser.PKUser;
                        mailTemplateObj.TML_NAME = txtSubject.Text;
                        mailTemplateObj.LAST_MOD_DT=LastModifiedTime;
                        mailTemplateObj.TML_NAME2 = HttpUtility.HtmlEncode(txtTemplateName.Text);
                        mailTemplateObj.TML_PK = CurrPK;
                        mailTemplateObj.TML_TEMPLATE = changeIndex(txtContent.Text.Replace("\n", "<br />"), -1);
                        mailTemplateObj.TML_TO_CC = txtCc.Text;
                        mailTemplateObj.TML_TO_BCC = txtBcc.Text;                    
                        returnObj = mailTemplateObj;
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
                if (dsMailTemplate != null && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    dtTemplateDetails = dsMailTemplate.Tables[0];
                    ApplicationCode = dtTemplateDetails.Rows[0]["APT_CODE"].ToString();
                    txtSubject.Text = dtTemplateDetails.Rows[0]["TML_NAME"].ToString();
                    txtContent.Text = changeIndex(dtTemplateDetails.Rows[0]["TML_TEMPLATE"].ToString().Replace("<br />", "\n"), 1);
                    txtMailFrom.Text = dtTemplateDetails.Rows[0]["TML_FROM"].ToString();
                    txtCc.Text = dtTemplateDetails.Rows[0]["TML_TO_CC"].ToString();
                    txtBcc.Text = dtTemplateDetails.Rows[0]["TML_TO_BCC"].ToString();
                    txtTemplateName.Text = dtTemplateDetails.Rows[0]["TML_NAME2"].ToString();
                    LastModifiedTime = dtTemplateDetails.Rows[0]["TML_MOD_DT"] == null ? DateTime.Now : Convert.ToDateTime(dtTemplateDetails.Rows[0]["TML_MOD_DT"].ToString());
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);

                    string xml = Convert.ToString(dsMailTemplate.Tables[0].Rows[0]["TEMPLATE_DETAIL"]);
                    XDocument xdoc = XDocument.Parse(xml);
                    xdoc.Declaration = null;
                    xml = xdoc.ToString();
                    objTemplateparams = new TemplateParameterBO();
                    objTemplateparams = (TemplateParameterBO)GTIService.CommonFunctions.DeserializeObject(xml, objTemplateparams);
                    SetFieldValues(ControlsEnum.TEMPLATEPARAMETERS);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string changeIndex(string str, int type)
        {
            if (ApplicationCode == ApplicationType.OUTDUE || ApplicationCode == ApplicationType.SALFRCST)
            {
                string result = string.Empty;
                string[] strSplit = str.Split('{');
                result = strSplit[0];
                for (int i = 1; i < strSplit.Length; i++)
                {
                    result += "{" + (type > 0 ? (Int32.Parse(strSplit[i].Substring(0, 1)) + 1).ToString() : (Int32.Parse(strSplit[i].Substring(0, 1)) - 1).ToString()) + strSplit[i].Substring(1, strSplit[i].Length - 1);

                }
                return result;
            }
            else
                return str;
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
        ///  Method for Bind Grid
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.TEMPLATELIST:
                    if (dsMailTemplate != null && dsMailTemplate.Tables[0].Rows.Count > 0)
                    {
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdMailTemplate.PageIndex = Convert.ToInt32(PageIndex);
                        grdMailTemplate.DataSource = dsMailTemplate.Tables[0];
                        grdMailTemplate.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();                      
                    }                 
                    break;
                case ControlsEnum.TEMPLATEPARAMETERS:
                    if (objTemplateparams != null && objTemplateparams.Detail.Count > 0)
                    {
                        grdParameters.DataSource = objTemplateparams.Detail.ToList();
                        grdParameters.DataBind();                        
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
                    lblLastModifiedHDR.Text = string.Empty;
                    EntryStatus = EntryStatus.LISTMODE;
                    txtContent.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    txtBcc.Text = string.Empty;
                    txtCc.Text = string.Empty;
                    txtMailFrom.Text = string.Empty;
                    txtTmplName.Text = string.Empty;
                    CurrPK = 0;
                    grdParameters.DataSource = null;
                    grdParameters.DataBind();
                    break;               
            }
        }

       
        /// <summary>
        /// Reset Form
        /// </summary>
        private void ResetForm()
        {
            ClearForm(ControlsEnum.CLEARALL);           
            GetFieldValues(ControlsEnum.TEMPLATELIST);
            SetFieldValues(ControlsEnum.TEMPLATELIST);
            EntryStatus = EntryStatus.LISTMODE;           
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

                IsPageChanged = true;
                PageIndex = uclPaging.CurrentPage.ToString();                
                GetFieldValues(ControlsEnum.TEMPLATELIST);
                SetFieldValues(ControlsEnum.TEMPLATELIST);
                EntryStatus = EntryStatus.LISTMODE;               
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
            MAILATTACHMENT,
            TEMPLATELIST,
            TEMPLATEDETAILS,
            TEMPLATEPARAMETERS
        }

        private enum MailStatus
        { 
          DRAFT=4,
          SEND=0
        }
        #endregion
    }
}