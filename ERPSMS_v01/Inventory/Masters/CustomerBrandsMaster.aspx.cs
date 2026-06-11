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
using System.Text;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class CustomerBrandsMaster : System.Web.UI.Page
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
        //page related class objects      
        private CRM_CUST_ITEM_MAP admItemMapMstObj;
        private CustomerBrands  admCustomerBrandsMstObj;

        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<CRM_CUSTOMER_MST> admCustomersMstList;
        private List<INV_ITEM_MST> admProductsList;
        private List<CustomerBrands> admCustomerBrandsList;
        private List<CRM_CUST_ITEM_MAP> admItemMapMstList;
        private List<SPINV_ITEM_ATTRIBUTES_GET_LIST_Result> ProductAttributesList;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
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
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.CustomerBrandPK;
                    grdCustomerBrandsMst.DataKeyNames = datakeyarray;
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
            AdmCustomerBrandsService AdmCustomerBrandsServiceClient = null;
            try
            {
                AdmCustomerBrandsServiceClient = new AdmCustomerBrandsService();
                AdmCustomerBrandsServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCustomerBrandsServiceClient);
                admCustomerBrandsMstObj = ERP.Utilities.CommonFunctions.Initilize<CustomerBrands>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        admCustomersMstList = AdmCustomerBrandsServiceClient.GetCustomers();
                        admProductsList = AdmCustomerBrandsServiceClient.GetProducts(0);
                        break;
                    case ControlsEnum.CUSTOMERBRANDSDATA:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        admCustomerBrandsMstObj.CIM_PK = CurrPK;
                        admCustomerBrandsMstObj.CIM_ITEM = Convert.ToInt32(ddlSearchIGPLProductCode.SelectedValue);
                        admCustomerBrandsMstObj.CIM_CUSTOMER = Convert.ToInt32(ddlSearchCustomer.SelectedValue);
                        admCustomerBrandsList = AdmCustomerBrandsServiceClient.GetCustomerBrands(admCustomerBrandsMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.CUSTOMERBRANDSLIST:

                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdCustomerBrandsMst.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CustomerBrandPK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admCustomerBrandsMstObj.CIM_PK = CurrPK;
                        admCustomerBrandsMstObj.CIM_ITEM = Convert.ToInt32(ddlSearchIGPLProductCode.SelectedValue);
                        admCustomerBrandsMstObj.CIM_CUSTOMER = Convert.ToInt32(ddlSearchCustomer.SelectedValue);
                        admCustomerBrandsList = AdmCustomerBrandsServiceClient.GetCustomerBrands(admCustomerBrandsMstObj, serviceUtilityObj);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    case ControlsEnum.PRODUCTNAME:
                        admProductsList = AdmCustomerBrandsServiceClient.GetProducts(Convert.ToInt32(ddlIGPLProductCode.SelectedValue));
                        break;
                    case ControlsEnum.PRODUCTATTRIBUTES:
                        CommonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        ProductAttributesList = CommonServiceClient.GetProductAttributes(Convert.ToInt32(ddlIGPLProductCode.SelectedValue), (int)ConstGroupType.Product, currentUser.SBUID, (byte)DbActiveStatus.ACTIVE);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admItemMapMstObj = null;
                admCustomerBrandsMstObj = null;
                serviceUtilityObj = null;
                AdmCustomerBrandsServiceClient = null;
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
                    case ControlsEnum.CUSTOMERBRANDSLIST:
                        BindGrid();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindCustomerDropDown();
                        BindProdctCodeDropDown();
                        BindStatusDropDown();
                        break;
                    case ControlsEnum.CUSTOMERBRANDSDATA:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.PRODUCTNAME:
                        SetProductName();
                        break;
                    case ControlsEnum.PRODUCTATTRIBUTES:
                        GetProductAttributesList();
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
        private CRM_CUST_ITEM_MAP SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                admItemMapMstObj.CIM_PK = CurrPK;
                admItemMapMstObj.CIM_CUSTOMER = Convert.ToInt32(ddlCustomer.SelectedValue);
                admItemMapMstObj.CIM_BRAND_NAME = HttpUtility.HtmlEncode(txtBrandName.Text.Trim());
                admItemMapMstObj.CIM_BRAND_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text.Trim());
                admItemMapMstObj.CIM_ITEM = Convert.ToInt32(ddlIGPLProductCode.SelectedValue);
                admItemMapMstObj.CIM_ACTIVE = (byte)Convert.ToInt32(ddlStatus.SelectedValue);
                admItemMapMstObj.CIM_MOD_DT = LastModifiedTime;
                return admItemMapMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admItemMapMstObj = null;
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
                if (admCustomerBrandsList != null && admCustomerBrandsList.Count() > 0)
                {
                    CurrPK = admCustomerBrandsList[0].CIM_PK;
                    ddlCustomer.SelectedValue = admCustomerBrandsList[0].CIM_CUSTOMER.ToString();
                    ddlIGPLProductCode.SelectedValue = admCustomerBrandsList[0].CIM_ITEM.ToString();
                    txtBrandName.Text = HttpUtility.HtmlDecode( admCustomerBrandsList[0].CIM_BRAND_NAME);
                    txtBrandCode.Text = HttpUtility.HtmlDecode(admCustomerBrandsList[0].CIM_BRAND_CODE);
                    ddlStatus.SelectedValue = admCustomerBrandsList[0].CIM_ACTIVE.ToString();
                    txtIGPLProductName.Text = HttpUtility.HtmlDecode(admCustomerBrandsList[0].ITM_NAME);

                    ModifiedDatePnl.Visible = true;
                    LastModifiedTime = admCustomerBrandsList[0].CIM_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);

                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                    EntryStatus = EntryStatus.LISTMODE;
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
                ModifiedDatePnl.Visible = false;
                if (admCustomerBrandsList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdCustomerBrandsMst.DataSource = admCustomerBrandsList;
                    grdCustomerBrandsMst.DataBind();
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
        /// Method for set Product Name
        /// </summary>
        public void SetProductName()
        {
            if (admProductsList != null && admProductsList.Count > 0)
            {
                txtIGPLProductName.Text = admProductsList[0].ITM_NAME;
            }
        }

        /// <summary>
        /// Method for get Product Attributes
        /// </summary>
        public void GetProductAttributesList()
        {
            StringBuilder ProductAttributes;
            if (ProductAttributesList != null && ProductAttributesList.Count > 0)
            {
                ProductAttributes = new StringBuilder();
                 ProductAttributes.Append("<div class=\"fields-grpwrap color-grey grp-before pad-t10 color-white\"><div class=\"header\"><h1>"+ Resources.Controls.ProductAttributes +"</h1><div class=\"clear\"></div></div>");
                                    
                //ProductAttributes.Append("<div class=\"search-wrap-custom1\"><div id=\"divSearch\"><label>" + Resources.Controls.ProductAttributes + "</label></div><div class=\"clear\"></div></div>");
                ProductAttributes.Append("<div class=\"fields-group\"><table class=\"table-devide\" style='margin-top:10px;'>");
                //ProductAttributes.Append("<tr><td colspan='2'>" + Resources.Controls.ProductAttributes + "</td></tr>");
                for (int index = 0; index < ProductAttributesList.Count; index++)
                {
                    if (index < (ProductAttributesList.Count - 1))
                    {
                        ProductAttributes.Append("<tr>");
                        ProductAttributes.Append("<td><div class=\"div2col-S\"><label>" + ProductAttributesList[index].CNG_NAME + "</label><span>" + ProductAttributesList[index].CNG_VALUE_TEXT + "</span></div></td>");
                        ProductAttributes.Append("<td><div class=\"div2col-S\"><label>" + ProductAttributesList[index+1].CNG_NAME + "</label><span>" + ProductAttributesList[index+1].CNG_VALUE_TEXT + "</span></div></td>");
                        ProductAttributes.Append("</tr>");
                        index++;
                    }
                    else
                    {
                        ProductAttributes.Append("<tr><td><div class=\"div2col-S\"><label>" + ProductAttributesList[index].CNG_NAME + "</label><span>" + ProductAttributesList[index].CNG_VALUE_TEXT + "</span></div></td></tr>");
           
                    }
                }
                ProductAttributes.Append("</table></div>");
                ltlProductAttributes.Text =ProductAttributes.ToString();
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
        }

        /// <summary>
        /// Method for Customer DropDown
        /// </summary>
        public void BindCustomerDropDown()
        {
            ddlSearchCustomer.Items.Clear();
            ddlCustomer.Items.Clear();
            if (admCustomersMstList != null && admCustomersMstList.Count > 0)
            {
                ddlSearchCustomer.DataSource = admCustomersMstList;
                ddlSearchCustomer.DataTextField = Resources.DataFieldRes.CustomerName;
                ddlSearchCustomer.DataValueField = Resources.DataFieldRes.CusPK; 
                ddlSearchCustomer.DataBind();


                ddlCustomer.DataSource = admCustomersMstList;
                ddlCustomer.DataTextField = Resources.DataFieldRes.CustomerName;
                ddlCustomer.DataValueField = Resources.DataFieldRes.CusPK; 
                ddlCustomer.DataBind();
            }
            ddlSearchCustomer.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlCustomer.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Method for Product code DropDown
        /// </summary>
        public void BindProdctCodeDropDown()
        {
            ddlSearchIGPLProductCode.Items.Clear();
            ddlIGPLProductCode.Items.Clear();
            if (admProductsList != null && admProductsList.Count > 0)
            {
                ddlSearchIGPLProductCode.DataSource = admProductsList;
                ddlSearchIGPLProductCode.DataTextField = Resources.DataFieldRes.ItemCode;
                ddlSearchIGPLProductCode.DataValueField = Resources.DataFieldRes.ItemPK;
                ddlSearchIGPLProductCode.DataBind();

                ddlIGPLProductCode.DataSource = admProductsList;
                ddlIGPLProductCode.DataTextField = Resources.DataFieldRes.ItemCode;
                ddlIGPLProductCode.DataValueField = Resources.DataFieldRes.ItemPK;
                ddlIGPLProductCode.DataBind();
            }
            ddlSearchIGPLProductCode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlIGPLProductCode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdCustomerBrandsMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdCustomerBrandsMst.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get And Set the Location details
                        GetFieldValues(ControlsEnum.CUSTOMERBRANDSDATA);
                        SetFieldValues(ControlsEnum.CUSTOMERBRANDSDATA);
                        // Get and set the Product Attributes
                        GetFieldValues(ControlsEnum.PRODUCTATTRIBUTES);
                        SetFieldValues(ControlsEnum.PRODUCTATTRIBUTES);
                        if (Mode == ActionsEnum.VIEW || Mode == ActionsEnum.ACTIVATE)
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
            ddlCustomer.SelectedValue = CommonConstants.SELECTVAL;
            ddlIGPLProductCode.SelectedValue = CommonConstants.SELECTVAL;
            txtBrandName.Text = string.Empty;
            txtBrandCode.Text = string.Empty;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            txtIGPLProductName.Text = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;

        }
        #endregion

        #region ActionHandler

        // /// <summary>
        ///// Handling control events
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void DropDownActionHandler(object sender, EventArgs e)
        //{

        //}
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            AdmCustomerBrandsService AdmCustomerBrandsServiceClient;
            AdmCustomerBrandsServiceClient = null;
            string dropdownID=string.Empty;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    dropdownID = ((DropDownList)sender).ID;
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
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
                            admItemMapMstList = new List<CRM_CUST_ITEM_MAP>();
                            AdmCustomerBrandsServiceClient = new AdmCustomerBrandsService();
                            AdmCustomerBrandsServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCustomerBrandsServiceClient);
                            admItemMapMstObj = ERP.Utilities.CommonFunctions.Initilize<CRM_CUST_ITEM_MAP>();
                            admItemMapMstObj = SetUIValuesToObject();
                            admItemMapMstList.Add(admItemMapMstObj);
                            result = AdmCustomerBrandsServiceClient.SaveCustomerBrands(admItemMapMstList);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.CustomerBrandPK;
                                SortDirection = Resources.Report.SortDescending;
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CustomerBrands);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                ddlSearchIGPLProductCode.SelectedValue = admItemMapMstObj.CIM_ITEM.ToString();
                                ddlSearchCustomer.SelectedValue = admItemMapMstObj.CIM_CUSTOMER.ToString();
                                GetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                                SetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                                btnNew.Focus();
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                        SetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                        SetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        AdmCustomerBrandsServiceClient = new AdmCustomerBrandsService();
                        AdmCustomerBrandsServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCustomerBrandsServiceClient);
                        admItemMapMstList = new List<CRM_CUST_ITEM_MAP>();
                        admItemMapMstObj = ERP.Utilities.CommonFunctions.Initilize<CRM_CUST_ITEM_MAP>();
                        admItemMapMstObj.CIM_PK = CurrPK;
                        admItemMapMstList.Add(admItemMapMstObj);

                        result = AdmCustomerBrandsServiceClient.DeleteCustomerBrands(admItemMapMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                            SetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CustomerBrands);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
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

                    #region SelectIndexChanged
                    //Drop down select change
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        if ( dropdownID == "ddlIGPLProductCode" )
                        {
                            if ( Convert.ToInt32(ddlIGPLProductCode.SelectedValue) > 0 )
                            {
                                GetFieldValues(ControlsEnum.PRODUCTNAME);
                                SetFieldValues(ControlsEnum.PRODUCTNAME);
                                GetFieldValues(ControlsEnum.PRODUCTATTRIBUTES);
                                SetFieldValues(ControlsEnum.PRODUCTATTRIBUTES);
                            }
                        }
                        else if (dropdownID == "ddlSearchCustomer" || dropdownID == "ddlSearchIGPLProductCode")
                        {
                            PageIndex = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                            SetFieldValues(ControlsEnum.CUSTOMERBRANDSLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("CustomerBrandsDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.BrandCode)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                admItemMapMstObj = null;
                admItemMapMstList = null;
                AdmCustomerBrandsServiceClient = null;
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                Label lblItemActive = e.Row.FindControl("lblItemActive") as Label;
                lblItemActive.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CIMActive).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
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
            breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnableProductAttributes", "EnableDisableProductAttributes();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnablegridCustomerBrands", "EnableDisableSearch();", true);

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
            }
            lblBreadCrum.Text = breadCrumb;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            CUSTOMERBRANDSLIST,
            CUSTOMERBRANDSDATA,
            PRODUCTNAME,
            PRODUCTATTRIBUTES

        }
        #endregion

    }
}