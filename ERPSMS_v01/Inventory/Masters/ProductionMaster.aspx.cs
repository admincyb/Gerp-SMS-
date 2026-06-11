using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Inventory;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;

namespace ERPSMS_v01.Inventory.Masters
{
	public partial class ProductionMaster : System.Web.UI.Page
	{

        #region Variables and Properties
        #region Properties
        
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

        
        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private ADM_CONST_MST admConstMstObj;
        private ADM_CONST_GRP admConstGrpObj;
        private ServiceUtility serviceUtilityObj;

        private List<ADM_CONST_MST> admConstMstList;
        private List<ADM_CONST_GRP> admConstGrtList;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private int grpID;
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
                    hdnGrptype.Value = "1";

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.ConstPK;
                    grdProductionMst.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.GROUP);
                    SetFieldValues(ControlsEnum.GROUP);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);


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
            //AdmProductPropertiesService AdmProductPropertiesServiceClient = null;
            //try
            //{
            //    AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
            //    AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
            //    admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
            //    switch (type)
            //    {
            //        case ControlsEnum.DEFAULT:
            //            serviceUtilityObj = new ServiceUtility();
            //            serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
            //            serviceUtilityObj.PageSize = grdProductionMst.PageSize;
            //            serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(txtSearchBy.Text.Trim());
            //            serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ConstPK : SortBy;
            //            serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
            //            admConstMstObj.CON_PK = CurrPK;
            //            admConstMstObj.CON_GROUP= Convert.ToInt32(ddlGroup.SelectedValue);
            //            admConstMstList = AdmProductPropertiesServiceClient.GetProductProperties(admConstMstObj, serviceUtilityObj);
            //            TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
            //                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
            //                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
            //            break;
            //        case ControlsEnum.GROUP:
            //            admConstGrtList = AdmProductPropertiesServiceClient.GetConstGrpvalues(Convert.ToInt32(hdnGrptype.Value));
            //            break;
            //        case ControlsEnum.PRODUCTION:
            //            serviceUtilityObj = new ServiceUtility();
            //            serviceUtilityObj.CurrentPage = -1;
            //            serviceUtilityObj.PageSize = -1;
            //            admConstMstObj.CON_PK = CurrPK;
            //            admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
            //            admConstMstList = AdmProductPropertiesServiceClient.GetProductProperties(admConstMstObj, serviceUtilityObj);
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    admConstMstObj = null;
            //    serviceUtilityObj = null;
            //}
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
                    case ControlsEnum.PRODUCTION:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.GROUP:
                        BindGroupDropDown();
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
        private ADM_CONST_MST SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                admConstMstObj.CON_PK = CurrPK;
                admConstMstObj.CON_CODE = HttpUtility.HtmlEncode(txtProductionCode.Text.Trim());
                admConstMstObj.CON_NAME = HttpUtility.HtmlEncode(txtProductionName.Text.Trim());
                admConstMstObj.CON_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                admConstMstObj.CON_DESC = HttpUtility.HtmlEncode(txtProductionDesc.Text.Trim());
                admConstMstObj.CON_DEFAULT = true;
                admConstMstObj.CON_ACTIVE = (byte)1;
                admConstMstObj.CON_BIZUNIT = 1;
                admConstMstObj.CON_MOD_BY = 2;
                return admConstMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admConstMstObj = null;
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
                if (admConstMstList != null && admConstMstList.Count() > 0)
                {
                    CurrPK = admConstMstList[0].CON_PK;
                    txtProductionCode.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_CODE);
                    txtProductionName.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_NAME);
                    txtProductionDesc.Text = HttpUtility.HtmlDecode(admConstMstList[0].CON_DESC);
                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
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
        public void BindGrid()
        {
            try
            {
                if (admConstMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdProductionMst.DataSource = admConstMstList;
                    grdProductionMst.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Group DropDown
        /// </summary>
        public void BindGroupDropDown()
        {
            ddlGroup.Items.Clear();
            if (admConstGrtList != null && admConstGrtList.Count > 0)
            {
                ddlGroup.DataSource = admConstGrtList;
                ddlGroup.DataTextField = Resources.DataFieldRes.GroupName;
                ddlGroup.DataValueField = Resources.DataFieldRes.GroupPK;
                ddlGroup.DataBind();
            }
        }


        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            txtSearchBy.Text = string.Empty;
            txtProductionCode.Text = string.Empty;
            txtProductionName.Text = string.Empty;
            txtProductionDesc.Text = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
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
            //AdmProductPropertiesService AdmProductPropertiesServiceClient;
            //AdmProductPropertiesServiceClient = null;
            //try
            //{
            //    int result;
            //    if (sender.GetType().IsEquivalentTo(typeof(Button)))
            //    {
            //        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            //    }
            //    else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            //    {
            //        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            //    }
            //    switch (commonActions)
            //    {
                   

            //        #region New
            //        case ActionsEnum.SAVE:
            //              if (!IsValid)
            //            {
            //                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
            //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //            }
            //            else//valid
            //            {
            //                admConstMstList = new List<ADM_CONST_MST>();
            //                AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
            //                AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
            //                admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
            //                admConstMstObj = SetUIValuesToObject();
            //                admConstMstList.Add(admConstMstObj);
            //                result = AdmProductPropertiesServiceClient.SaveProductProperties(admConstMstList);
            //                if (result >= 0) // Success ! re-initialize the page
            //                {
            //                    SortBy = Resources.DataFieldRes.ConstPK;
            //                    SortDirection = Resources.Report.SortDescending;
            //                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
            //                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
            //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //                    ResetForm();
            //                    GetFieldValues(ControlsEnum.DEFAULT);
            //                    SetFieldValues(ControlsEnum.DEFAULT);
            //                    btnSave.Focus();
            //                }
            //            }
            //            break;
            //        #endregion

            //        #region Search
            //        case ActionsEnum.SEARCH:
            //            btnSearch.Focus();
            //            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            //            GetFieldValues(ControlsEnum.DEFAULT);
            //            SetFieldValues(ControlsEnum.DEFAULT);
            //            break;
            //        #endregion

            //        #region Cancel
            //        case ActionsEnum.CANCEL:
            //            ResetForm();
            //            GetFieldValues(ControlsEnum.DEFAULT);
            //            SetFieldValues(ControlsEnum.DEFAULT);
            //            this.btnSave.Focus();
            //            break;
            //        #endregion

                   

                   
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
            //    else
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
            //finally
            //{
            //    admConstMstObj = null;
            //    admConstMstList = null;
            //    AdmProductPropertiesServiceClient = null;
            //}
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        /// <summary>
        /// Method used to Handle all actions in the page with GridViewRowCommand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            //AdmProductPropertiesService AdmProductPropertiesServiceClient;
            //AdmProductPropertiesServiceClient = null;
            //try
            //{
            //    int result;
            //    int index = Convert.ToInt32(e.CommandArgument);
            //    CurrPK = (int)this.grdProductionMst.DataKeys[index][Resources.DataFieldRes.ConstPK];
            //    if (e.CommandName == "GRIDDELETE")
            //    {
            //        AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
            //        AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
            //        admConstMstList = new List<ADM_CONST_MST>();
            //        admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
            //        admConstMstObj.CON_PK = CurrPK;
            //        admConstMstList.Add(admConstMstObj);

            //        result = AdmProductPropertiesServiceClient.DeleteProductProperties(admConstMstList);
            //        if (result > 0)
            //        {
            //            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
            //            ResetForm();
            //            btnSave.Focus();
            //            GetFieldValues(ControlsEnum.DEFAULT);
            //            SetFieldValues(ControlsEnum.DEFAULT);
            //            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
            //            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //        }

            //    }
            //    else if (e.CommandName == "GRIDEDIT")
            //    {
            //        // Get And Set the Location details
            //        GetFieldValues(ControlsEnum.PRODUCTION);
            //        SetFieldValues(ControlsEnum.PRODUCTION);
            //        txtProductionCode.Focus();
            //    } 
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
        }



        /// <summary>
        /// Handling dropdown change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownActionHandler(object sender, EventArgs e)
        {

            ResetForm();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            this.btnSave.Focus();

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
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
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
                EnableDisableButtons(e.TotalPages);
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PRODUCTION,
            GROUP
        }
        #endregion
	

        
	}
}