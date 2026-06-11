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
using BusinessLogic.BrandRates;
using BusinessObject.BrandRate;
using System.Xml;
using BusinessObject.CommonManagement;
using CustomControls;
using BusinessLogic.CommonManagement;
using ERPSMS_v01.UserControls;
using System.Threading;
using ERPManager;

#endregion

namespace CustomerPortal.OrderToCash
{
    public partial class BrandRates : ERP.Store.UI.WorkFlowBasePage
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 5 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageIndex] == null ? "1" : (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
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
        /// Brand PK
        /// </summary>
        private int BrandPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.BrandPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.BrandPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BrandPK] = value;
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
        /// To keep selected products PK
        /// </summary>
        private ItemBO SelectedProducts
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedProducts] == null ? null : (ItemBO)this.ViewState[ViewstateStrings.SelectedProducts];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedProducts] = value;
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
        /// Keep Search Type
        /// </summary>
        private int SearchType
        {
            get
            {
                return this.ViewState["SearchType"] == null ? 2 : Convert.ToInt32(this.ViewState["SearchType"]);
            }
            set
            {
                this.ViewState["SearchType"] = value;
            }
        }

        /// <summary>
        /// To keep selected customers PK
        /// </summary>
        private CustomerBO SelectedCustomers
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedCustomers] == null ? null : (CustomerBO)this.ViewState[ViewstateStrings.SelectedCustomers];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomers] = value;
            }
        }

        /// <summary>
        /// Selected Custoer PK
        /// </summary>
        private List<CustomerListBO> SelCustomerList
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerList] == null ? null : (List<CustomerListBO>)this.ViewState[ViewstateStrings.CustomerList];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerList] = value;
            }
        }

        /// <summary>
        /// Selected Product List
        /// </summary>
        private List<ItemsListBO> SelItemList
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemList] == null ? null : (List<ItemsListBO>)this.ViewState[ViewstateStrings.ItemList];
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemList] = value;
            }

        }
        /// <summary>
        /// Keep Current Action
        /// </summary>
        private ActionsEnum CurrentAction
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrentAction] == null ? ActionsEnum.DEFAULT : (ActionsEnum)this.ViewState[ViewstateStrings.CurrentAction];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrentAction] = value;
            }

        }

        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }

        /// <summary>
        /// To keep selected packing Spec PK
        /// </summary>
        private  List<PackingSpecListBO> SelectedPackingSpecs
        {
            get
            {
                return this.ViewState["SelectedPackingSpecs"] == null ? null : ( List<PackingSpecListBO>)this.ViewState["SelectedPackingSpecs"];
            }
            set
            {
                this.ViewState["SelectedPackingSpecs"] = value;
            }
        }
        #endregion
        User currentUser;
        private ActionsEnum commonActions;
        private string refID;
        private string prefID;
        private int referenceID;
        private int processPK;
        private int appId;
        int brandPK;
        private string inboxFlag;
        DataTable dtCustomers;
        DataTable dtCurrency;
        DataTable dtProducts;
        DataTable dtProductDtl;
        DataTable dtCustomerDtl;
        DataSet dsCustomerRate;
        DataTable dtBrandDetails;
        DataSet dsRateHistory;
        XmlDocument xmlDoc;
        DataSet dsPageData;

        ItemBO selectedItemsObj;
        CustomerBO selectedCustomersObj;
        CustomerRateBO customerRateObj;
        CustomerRateCopyBO customerRateCopyObj;
        bool validPage = false;
        User CurrentUser;
        private string searchType = string.Empty;
        private DataTable dtUserData;
        private DataTable dtSpecialCategory;
        private DataTable dtPageData;
        List<DDLMaster> chkUsers;
        int disableDetailsFlag = 0;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            int preferenceID;
            int processID;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    disableDetailsFlag = 1;
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    SetFieldValues(ControlsEnum.USERCUSTOMER);
                    GetFieldValues(ControlsEnum.CUSTOMERPROPERTIES);
                    SetFieldValues(ControlsEnum.CUSTOMERPROPERTIES);

                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.CURRENCY);
                    BindGrid(ControlsEnum.DEFAULT);
                    hdfCurrentUserSbu.Value = CurrentUser.SBUID.ToString();
                    FillProcessID();
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                                   ? string.IsNullOrEmpty(prefID)
                                       ? 0
                                       : int.Parse(prefID)
                                   : int.Parse(refID);

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSet.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            CurrPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                        Session[ERP.Utilities.SessionStrings.PRefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }


                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                        }
                    }
                    CurrentAction = ActionsEnum.DEFAULT;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.SET);
                    SetFieldValues(ControlsEnum.APPLY);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    #region BindCurrency
                    // DataTable dtCurrencyList = CommonBL.GetCurrencyListByBtzuUnit(currentUser.SBUID);
                    ddlCurrencyCustomerListPopUp.DataSource = dtCurrency; // dtCurrencyList;
                    ddlCurrencyCustomerListPopUp.DataTextField = "CUR_CODE";
                    ddlCurrencyCustomerListPopUp.DataValueField = "CUR_PK";
                    //ddlCurrency.ToolTip = "CUR_NAME";
                    ddlCurrencyCustomerListPopUp.DataBind();
                    ddlCurrencyCustomerListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    #endregion BindCurrency

                    #region ProductPropertiesDropDownBind
                    // currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE)
                    object val = Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode());

                    int ii = Convert.ToInt32(val);
                    int jj = Convert.ToInt32(Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode()));

                    int groupType = Convert.ToInt32(Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode()));
                    DataTable dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Type, ProductProperties.Type.GetTypeCode())));
                    ddlTypeProductListPopUp.DataValueField = "CON_PK";
                    ddlTypeProductListPopUp.DataTextField = "CON_NAME";
                    ddlTypeProductListPopUp.DataSource = dtProductProperties;
                    ddlTypeProductListPopUp.DataBind();
                    ddlTypeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlTypeProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Thickness, ProductProperties.Thickness.GetTypeCode())));
                    ddlThicknessProductListPopUp.DataValueField = "CON_PK";
                    ddlThicknessProductListPopUp.DataTextField = "CON_NAME";
                    ddlThicknessProductListPopUp.DataSource = dtProductProperties;
                    ddlThicknessProductListPopUp.DataBind();
                    ddlThicknessProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlThicknessProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Category, ProductProperties.Category.GetTypeCode())));
                    ddlCategoryProductListPopUp.DataValueField = "CON_PK";
                    ddlCategoryProductListPopUp.DataTextField = "CON_NAME";
                    ddlCategoryProductListPopUp.DataSource = dtProductProperties;
                    ddlCategoryProductListPopUp.DataBind();
                    ddlCategoryProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlCategoryProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Surface, ProductProperties.Surface.GetTypeCode())));
                    ddlSurfaceProductListPopUp.DataValueField = "CON_PK";
                    ddlSurfaceProductListPopUp.DataTextField = "CON_NAME";
                    ddlSurfaceProductListPopUp.DataSource = dtProductProperties;
                    ddlSurfaceProductListPopUp.DataBind();
                    ddlSurfaceProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlSurfaceProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Shade, ProductProperties.Shade.GetTypeCode())));
                    ddlShadeProductListPopUp.DataValueField = "CON_PK";
                    ddlShadeProductListPopUp.DataTextField = "CON_NAME";
                    ddlShadeProductListPopUp.DataSource = dtProductProperties;
                    ddlShadeProductListPopUp.DataBind();
                    ddlShadeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlShadeProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Classification, ProductProperties.Classification.GetTypeCode())));
                    ddlClassificationProductListPopUp.DataValueField = "CON_PK";
                    ddlClassificationProductListPopUp.DataTextField = "CON_NAME";
                    ddlClassificationProductListPopUp.DataSource = dtProductProperties;
                    ddlClassificationProductListPopUp.DataBind();
                    ddlClassificationProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlClassificationProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Size, ProductProperties.Size.GetTypeCode())));
                    ddlSizeProductListPopUp.DataValueField = "CON_PK";
                    ddlSizeProductListPopUp.DataTextField = "CON_NAME";
                    ddlSizeProductListPopUp.DataSource = dtProductProperties;
                    ddlSizeProductListPopUp.DataBind();
                    ddlSizeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlSizeProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Length, ProductProperties.Length.GetTypeCode())));
                    ddlLengthProductListPopUp.DataValueField = "CON_PK";
                    ddlLengthProductListPopUp.DataTextField = "CON_NAME";
                    ddlLengthProductListPopUp.DataSource = dtProductProperties;
                    ddlLengthProductListPopUp.DataBind();
                    ddlLengthProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlLengthProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Chlorination, ProductProperties.Chlorination.GetTypeCode())));
                    ddlChlorinationProductListPopUp.DataValueField = "CON_PK";
                    ddlChlorinationProductListPopUp.DataTextField = "CON_NAME";
                    ddlChlorinationProductListPopUp.DataSource = dtProductProperties;
                    ddlChlorinationProductListPopUp.DataBind();
                    ddlChlorinationProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlChlorinationProductListPopUp.SelectedIndex = 0;
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Side, ProductProperties.Side.GetTypeCode())));
                    ddlSideProductListPopUp.DataValueField = "CON_PK";
                    ddlSideProductListPopUp.DataTextField = "CON_NAME";
                    ddlSideProductListPopUp.DataSource = dtProductProperties;
                    ddlSideProductListPopUp.DataBind();
                    ddlSideProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlSideProductListPopUp.SelectedIndex = 0;

                    //Additional Spec
                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec05, ProductProperties.Side.GetTypeCode())));
                    ddlAdnlSpec05ProductListPopUp.DataValueField = "CON_PK";
                    ddlAdnlSpec05ProductListPopUp.DataTextField = GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlAdnlSpec05ProductListPopUp.DataSource = dtProductProperties;
                    ddlAdnlSpec05ProductListPopUp.DataBind();
                    ddlAdnlSpec05ProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlAdnlSpec05ProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec06, ProductProperties.Side.GetTypeCode())));
                    ddlAdnlSpec06ProductListPopUp.DataValueField = "CON_PK";
                    ddlAdnlSpec06ProductListPopUp.DataTextField = GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlAdnlSpec06ProductListPopUp.DataSource = dtProductProperties;
                    ddlAdnlSpec06ProductListPopUp.DataBind();
                    ddlAdnlSpec06ProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlAdnlSpec06ProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec07, ProductProperties.Side.GetTypeCode())));
                    ddlAdnlSpec07ProductListPopUp.DataValueField = "CON_PK";
                    ddlAdnlSpec07ProductListPopUp.DataTextField = GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlAdnlSpec07ProductListPopUp.DataSource = dtProductProperties;
                    ddlAdnlSpec07ProductListPopUp.DataBind();
                    ddlAdnlSpec07ProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlAdnlSpec07ProductListPopUp.SelectedIndex = 0;
                    //End
                    #endregion ProductPropertiesDropDownBind

                    GetFieldValues(ControlsEnum.PACKINGSPECS);
                    SetFieldValues(ControlsEnum.PACKINGSPECS);
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
            string fromDate;
            string toDate;
            try
            {
                switch (type)
                {
                    #region Old Brand
                    case ControlsEnum.OLDBRAND:
                        customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.OLDBRAND);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                        dsPageData = BrandRatesBL.GetCustomerLatestRate(xmlDoc.InnerXml);
                        break;
                    #endregion
                    #region Default
                    case ControlsEnum.DEFAULT:
                        if (CurrPK > 0)
                        {
                            customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.DEFAULT);
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                            dsPageData = BrandRatesBL.GetCustomerLatestRate(xmlDoc.InnerXml, disableDetailsFlag);
                        }
                        else
                            dsPageData = BrandRatesBL.GetCustomerLatestRate(string.Empty, disableDetailsFlag);
                        break;
                    #endregion

                    case ControlsEnum.CURRENCY:
                        dtCurrency = CommonBL.GetCurrencyListByBtzuUnit(currentUser.SBUID); //CommonBL.GetCurrencyList(currentUser.SBUID);
                        Currency = dtCurrency;
                        break;
                    case ControlsEnum.CUSTOMERS:
                        int specialCatId = 0;
                        int.TryParse(ddlCustSpecialCategory.SelectedValue, out specialCatId);
                        dtCustomers = BrandRatesBL.GetCustomerList(currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), specialCatId);
                        break;
                    case ControlsEnum.PRODUCTS:
                        dtProducts = BrandRatesBL.GetProductList(currentUser.SBUID, Convert.ToInt32(ERP.Utilities.Constants.DA.ProductCategory.FinishedGoods), Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE));
                        break;
                    case ControlsEnum.PRODUCTDTL:
                        selectedItemsObj = (ItemBO)SetUIValuesToObject(commonActions);
                        if (selectedItemsObj.ItemsList.Count > 0)
                        {
                            SelItemList = selectedItemsObj.ItemsList;
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(selectedItemsObj);
                            dtProductDtl = BrandRatesBL.GetProductDetail(xmlDoc.InnerXml);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectProduct").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ControlsEnum.CUSTOMERDTL:
                        selectedCustomersObj = (CustomerBO)SetUIValuesToObject(commonActions);
                        if (selectedCustomersObj.CustomerList.Count > 0)
                        {
                            SelCustomerList = selectedCustomersObj.CustomerList;
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(selectedCustomersObj);
                            dtCustomerDtl = BrandRatesBL.GetCustomerDetail(xmlDoc.InnerXml);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectCustomer").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ControlsEnum.APPLY:
                        customerRateObj = (CustomerRateBO)SetUIValuesToObject(commonActions);
                        if (customerRateObj.CustomerList.Count > 0 || customerRateObj.ItemsList.Count > 0)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                            dsCustomerRate = BrandRatesBL.GetCustomerRate(xmlDoc.InnerXml);
                            CustomerRates = dsCustomerRate;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectPrdCus").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ControlsEnum.SET:
                        fromDate =txtFromDate.Text; //"01-" + txtCalender.Text;
                        //need to check valid date
                        DateTime t;
                        if (DateTime.TryParse(fromDate, out t))
                        {
                            int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(fromDate)).Year, (Convert.ToDateTime(fromDate)).Month);
                            toDate = string.IsNullOrEmpty(txtToDate.Text) ? fromDate : txtToDate.Text;//totaldays.ToString() + "-" + txtCalender.Text;
                            dtBrandDetails = BrandRatesBL.GetBratndDetails(0, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE));
                            FromDate = Convert.ToDateTime(fromDate);
                            ToDate = Convert.ToDateTime(toDate);
                            validPage = true;
                        }
                        else
                        {
                            validPage = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectValidMonth").ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        }
                        break;

                    case ControlsEnum.RATEHISTORY:
                        dsRateHistory = BrandRatesBL.GetBrabdRateHistory(brandPK);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        dtUserData = new DataTable();
                        dtUserData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    #region CUSTOMERPROPERTIES
                    case ControlsEnum.CUSTOMERPROPERTIES:
                        dtSpecialCategory = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, (int)ConstGroup.SpecialCategory,ConstGroupType.CustomerProperties, 0, 1, currentUser.SBUID);                       
                        break;
                    #endregion
                    #region PACKINGSPECS
                    case ControlsEnum.PACKINGSPECS:
                        string sortby = string.Empty;
                        sortby = GetGlobalResourceObject("ConfigurationsRes", "PackingSpecSortBy").ToString();
                        dsPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID,
                           string.Empty, string.Empty, string.Empty,sortby);
                        if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                        {
                            dtPageData = dsPageData.Tables[0];
                            chkUsers = dtPageData.AsEnumerable().Select(row => new DDLMaster
                            {
                                PK = row.Field<int?>(Resources.DataFieldRes.PackingMstPK),
                                Value = row.Field<string>(Resources.DataFieldRes.PackingSpecs),
                            }).ToList();

                        }
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

        private void SetFieldValues(ControlsEnum type)
        {
            DataRow[] drr;
            switch (type)
            {
                case ControlsEnum.DEFAULT:
                    dtBrandDetails = dsPageData.Tables[0];


                    dsPageData.Tables.Remove(dsPageData.Tables[0]);
                    dsCustomerRate = dsPageData;
                    CustomerRates = dsCustomerRate;
                    //For filling product and cutomer ddl
                    if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                    {
                        hdfbrandRatePK.Value = dtBrandDetails.Rows[0]["BRH_PK"].ToString();
                        hdfBrandRateSbu.Value = dtBrandDetails.Rows[0]["BRH_BIZUNIT"].ToString();

                    }


                    break;
                case ControlsEnum.PRODUCTS:
                    BindTree(ControlsEnum.PRODUCTS);
                    break;
                case ControlsEnum.CUSTOMERS:
                    BindTree(ControlsEnum.CUSTOMERS);
                    break;
                case ControlsEnum.PRODUCTDTL:
                    BindGrid(type);
                    break;
                case ControlsEnum.CUSTOMERDTL:
                    BindGrid(type);
                    break;
                case ControlsEnum.APPLY:
                    BindGrid(ControlsEnum.APPLY);
                    if (dsCustomerRate != null && dsCustomerRate.Tables.Count > 0 && dsCustomerRate.Tables[0].Rows.Count > 0 && EntryStatus != EntryStatus.VIEWMODE)
                    {
                        // btnSave.Visible = true;
                        // SetRdbVisibility(true);
                        SetButtonEnable(false);
                    }
                    break;
                case ControlsEnum.SET:
                    GetUIValuesFromObject(type);
                    break;
                #region Rate History
                case ControlsEnum.RATEHISTORY:
                    GetUIValuesFromObject(type);
                    break;
                #endregion
                case ControlsEnum.RATEAPPLY:
                    dsCustomerRate = CustomerRates;
                    drr = dsCustomerRate.Tables[1].Select("HDR_PK=" + ItemPK);
                    foreach (var row in drr)
                    {
                        if (row["CUR_PK"].ToString() == ddlCurrency.SelectedValue.ToString())
                        {
                            row["BRD_RATE"] = txtNewRateApply.Text;
                            //   row["CUR_PK"] = ddlCurrency.SelectedValue.ToString();
                            //row["BRD_RATE"] = decimal.Parse(txtNewRateApply.Text)
                            row.AcceptChanges();
                        }
                    }
                    dsCustomerRate.Tables[1].AcceptChanges();
                    CustomerRates = dsCustomerRate;
                    BindGrid(ControlsEnum.APPLY);
                    txtNewRateApply.Text = "";
                    break;
                case ControlsEnum.USERCUSTOMER:
                    if (dtUserData.Rows.Count > 0)
                    {
                        ddlPrint.Enabled = false;
                        txtCustomer.Enabled = false;
                        txtCustomer.Text = dtUserData.Rows[0]["CUS_NAME"].ToString();
                        hdfCustomer.Value = dtUserData.Rows[0]["CUS_PK"].ToString();
                    }
                    else
                    {
                        ddlPrint.Enabled = true;
                        txtCustomer.Enabled = true;
                    }
                    break;
                case ControlsEnum.CUSTOMERPROPERTIES:
                    BindDropdown(ControlsEnum.CUSTOMERPROPERTIES);
                    break;
                #region PACKINGSPECS
                case ControlsEnum.PACKINGSPECS:
                    if (chkUsers != null && chkUsers.Count > 0)
                    {
                        chklstPackingSpec.ListData = CommonFunctions.HtmlDecode(chkUsers, "Value");
                        chklstPackingSpec.BindData();
                    }
                    else
                    {
                        chklstPackingSpec.ClearData();
                    }
                    break; 
                #endregion
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
            int leafCount = 0;
            string brandDate = "";
            int checkCount = 0;
            //chkExpandAll.Checked = false;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rdbCustomer")
                    {
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        commonActions = ActionsEnum.CHANGETYPE;
                        SearchType = (int)SearchTypeEnum.Customer;
                    }
                    else
                        if (((RadioButton)sender).ID == "rdbProduct")
                        {
                            PageIndex = CommonConstants.SELECT_VALUE_ONE;
                            commonActions = ActionsEnum.CHANGETYPE;
                            SearchType = (int)SearchTypeEnum.Product;
                        }
                }
                switch (commonActions)
                {
                    #region Copy Brnad
                    case ActionsEnum.COPY:
                        //BrandPK = CurrPK;
                        //ClearForm(ControlsEnum.COPY);
                        //SetButtonEnable(false);
                        //BindWorkflow();
                        //// SetRdbVisibility(true);
                        customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.SAVE);
                        if (customerRateObj != null && customerRateObj.DetailsList.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowbrndCopy", "ShowContainerDiv('[id$=divCopyBrand]','" + GetLocalResourceObject("CopyBrandrate").ToString() + "','350','260');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgSetBrandRate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        }
                        break;
                    #endregion
                    #region Radio change
                    case ActionsEnum.CHANGETYPE:
                        if (lblStatus.Text == GetLocalResourceObject("New").ToString() && BrandPK > 0)
                        {
                            GetFieldValues(ControlsEnum.OLDBRAND);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            BindGrid(ControlsEnum.APPLY);
                        }
                        else
                            if (lblStatus.Text == GetLocalResourceObject("New").ToString() || SelectedCustomers != null || SelectedProducts != null)
                            {
                                GetFieldValues(ControlsEnum.APPLY);
                                SetFieldValues(ControlsEnum.APPLY);

                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                BindGrid(ControlsEnum.APPLY);
                            }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                        break;
                    #endregion
                    #region Gridvew Events
                    case ActionsEnum.REMOVEITEM:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfItem = gvr.FindControl("hdfItemPK") as HiddenField;
                            dsCustomerRate = CustomerRates;
                            DeleteTabelRow(Convert.ToInt32(arg));
                            SetFieldValues(ControlsEnum.APPLY);

                        }
                        break;
                    #endregion
                    #region General
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BrandRatesBL.DeleteRecord(CurrPK);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                ClearForm(ControlsEnum.CLEARALL);
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
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
                                    litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
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
                            if (SetMonth())
                            {
                                #region Save Details
                                customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.SAVE);
                                if (customerRateObj != null && customerRateObj.DetailsList.Count > 0)
                                {
                                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                                    result = BrandRatesBL.SaveBrandDetails(xmlDoc.InnerXml);
                                    CurrPK = result;
                                    if (result >= 0) // Success ! re-initialize the page
                                    {
                                        EntryStatus = EntryStatus.ENTRYMODE;
                                        btnDelete.Visible = true;
                                        CurrentAction = ActionsEnum.SAVE;
                                        GetFieldValues(ControlsEnum.SET);
                                        SetFieldValues(ControlsEnum.SET);
                                        hdfbrandRatePK.Value = CurrPK.ToString();
                                        CurrentAction = ActionsEnum.APPLY; // Shihab
                                        //lblStatus.Text = GetLocalResourceObject("DraftSave").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
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
                                            litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgSetBrandRate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                                #endregion
                            }
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectValidMonth").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Save Copy
                    case ActionsEnum.SAVECOPY:
                        //Save Details
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            if (SetCopyMonth())
                            {
                                #region Save Copy Details
                                customerRateCopyObj = (CustomerRateCopyBO)SetUIValuesToObject(ActionsEnum.SAVECOPY);
                                if (customerRateCopyObj != null)
                                {
                                    result = BrandRatesBL.CopyBrandDetails(customerRateCopyObj);
                                    if (result > 0) // Success ! re-initialize the page
                                    {
                                        CurrPK = result;
                                        EntryStatus = EntryStatus.ENTRYMODE;
                                        btnDelete.Visible = true;
                                        CurrentAction = ActionsEnum.SAVE;
                                       // txtCalender.Text = txtCopyMonth.Text;
                                        GetFieldValues(ControlsEnum.SET);
                                        SetFieldValues(ControlsEnum.SET);
                                        txtCopyFrom.Text = string.Empty;
                                        txtCopyTo.Text = string.Empty;
                                        hdfbrandRatePK.Value = CurrPK.ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
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
                                            litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgSetBrandRate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                                #endregion
                            }
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectValidMonth").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            if (SetMonth())
                            {
                                #region Submit details
                                ucrWrkf.ApplicationID = 0;
                               // brandDate = txtCalender.Text;
                                customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.SAVE);
                                if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                                {
                                    if (customerRateObj != null && customerRateObj.DetailsList.Count > 0)
                                    {
                                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                                        result = BrandRatesBL.SaveBrandDetails(xmlDoc.InnerXml);
                                        if (result >= 0) // Success ! re-initialize the page
                                        {

                                            ClearForm(ControlsEnum.CLEARALL);
                                            ucrWrkf.ApplicationID = result;
                                            //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                            ////Do WorkFlow if WorkFlow has Actions
                                            //if (ddlWkfAction.Items.Count > 0)
                                            //{
                                            //    action = ddlWkfAction.SelectedItem.ToString();
                                            //    result = ucrWrkf.DoWorkFlow();
                                            //    CurrPK = 0;
                                            //    //Show Save success message and reset Contract Entry
                                            //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                            //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            //       + "','" + Resources.ErpRes.Information + "');", true);
                                            //}
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
                                                litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgSetBrandRate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }

                                }
                                else
                                    ucrWrkf.ApplicationID = CurrPK;

                                if (customerRateObj != null && customerRateObj.DetailsList.Count > 0)
                                {
                                    if (ucrWrkf.ApplicationID > 0)
                                    {
                                        ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                        //Do WorkFlow if WorkFlow has Actions
                                        if (ddlWkfAction.Items.Count > 0)
                                        {
                                            action = ddlWkfAction.SelectedItem.ToString();
                                            result = ucrWrkf.DoWorkFlow();


                                            CurrPK = 0;
                                            if (result > 0)
                                            {
                                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                                {
                                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                       + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                                }
                                                else
                                                {
                                                  //  txtCalender.Text = brandDate;
                                                    hdfbrandRatePK.Value = CurrPK.ToString();
                                                    GetFieldValues(ControlsEnum.SET);
                                                    SetFieldValues(ControlsEnum.SET);
                                                    //Show Save success message and reset Contract Entry
                                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                       + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //Trx not saved
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgSetBrandRate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }


                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                                #endregion
                            }
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectValidMonth").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #endregion
                    #region POPUP
                    #region Show Customers/ Products
                    case ActionsEnum.SHOWCUSTOMER:
                        //if (trvCustomers.Nodes.Count == 0)
                        //{
                        ddlCustSpecialCategory.SelectedIndex = 0;
                        GetFieldValues(ControlsEnum.CUSTOMERS);
                        BindTree(ControlsEnum.CUSTOMERS);
                        foreach (TreeNode node in trvCustomers.Nodes)
                        {
                            node.Checked = false;
                        }
                        //}
                        //else
                        //{

                        //// Code for Check, Checkboxes in Treeview for already Selected Customers: Begins
                        //if (SelCustomerList != null)
                        //{
                        //    checkCount = 0;
                        //    TreeNode root = null;
                        //    foreach (CustomerListBO item in SelCustomerList)
                        //        foreach (TreeNode node in trvCustomers.Nodes)
                        //        {
                        //            root = node;
                        //            root.Checked = false;
                        //            foreach (TreeNode child1 in node.ChildNodes)
                        //            {
                        //                if (child1.Value == item.cusPK.ToString())
                        //                {
                        //                    child1.Checked = true;
                        //                    checkCount++;
                        //                }
                        //            }
                        //        }
                        //    if (checkCount == root.ChildNodes.Count)
                        //    {
                        //        root.Checked = true;
                        //    }
                        //}
                        //else
                        //{
                        //    foreach (TreeNode node in trvCustomers.Nodes)
                        //    {
                        //        node.Checked = false;
                        //    }

                        //}
                        //// Code for Check, Checkboxes in Treeview for already Selected Customers: Ends

                        //  }
                        ddlCurrencyCustomerListPopUp.SelectedIndex = 0;                        
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSearchCustomers]','" + GetLocalResourceObject("CustomerList").ToString() + "','500','330');", true);
                        break;
                    case ActionsEnum.SHOWPRODUCTS:
                        ddlTypeProductListPopUp.SelectedIndex = ddlThicknessProductListPopUp.SelectedIndex = ddlCategoryProductListPopUp.SelectedIndex = 0;
                        ddlSurfaceProductListPopUp.SelectedIndex = ddlShadeProductListPopUp.SelectedIndex = ddlClassificationProductListPopUp.SelectedIndex = 0;
                        ddlSizeProductListPopUp.SelectedIndex = ddlLengthProductListPopUp.SelectedIndex = ddlChlorinationProductListPopUp.SelectedIndex = 0;
                        ddlSideProductListPopUp.SelectedIndex = 0;
                        GetFieldValues(ControlsEnum.PRODUCTS);
                        BindTree(ControlsEnum.PRODUCTS);
                        foreach (TreeNode node in trvProducts.Nodes)
                        {
                            node.Checked = false;
                        }
                        ////// Code for Check, Checkboxes in Treeview for already Selected Products: Begins
                        //if (trvProducts.Nodes.Count == 0)
                        //{
                        //    //GetFieldValues(ControlsEnum.PRODUCTS);
                        //    //BindTree(ControlsEnum.PRODUCTS);
                        //}
                        //else
                        //{
                        //    if (SelItemList != null)
                        //    {
                        //        checkCount = 0;
                        //        TreeNode root = null;
                        //        foreach (ItemsListBO item in SelItemList)
                        //            foreach (TreeNode node in trvProducts.Nodes)
                        //            {
                        //                root = node;
                        //                root.Checked = false;
                        //                foreach (TreeNode child1 in node.ChildNodes)
                        //                {
                        //                    if (child1.Value == item.itemPK.ToString())
                        //                    {
                        //                        child1.Checked = true;
                        //                        checkCount++;
                        //                    }
                        //                }
                        //            }
                        //        if (checkCount == root.ChildNodes.Count)
                        //        {
                        //            root.Checked = true;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        foreach (TreeNode node in trvProducts.Nodes)
                        //        {
                        //            node.Checked = false;
                        //        }
                        //    }
                        //}
                        ////// Code for Check, Checkboxes in Treeview for already Selected Products: Ends

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divSearchProducts]','" + GetLocalResourceObject("ProductList").ToString() + "','750','400');", true);
                        break;
                    #endregion
                    #region Rate History
                    case ActionsEnum.RATEHISTORY:
                        arg = ((ImageButton)sender).CommandArgument;
                        brandPK = Convert.ToInt32(arg);
                        GetFieldValues(ControlsEnum.RATEHISTORY);
                        SetFieldValues(ControlsEnum.RATEHISTORY);

                        break;
                    #endregion
                    #region Set Rate
                    case ActionsEnum.SETRATE:
                        SetCurrentValueToTable();
                        arg = ((ImageButton)sender).CommandArgument;
                        ItemPK = Convert.ToInt32(arg);
                        dsCustomerRate = CustomerRates;
                        drr = dsCustomerRate.Tables[0].Select("HDR_PK=" + ItemPK);
                        if (drr != null)
                        {
                            switch (rdbCustomer.Checked)
                            {
                                case true:
                                    lbnProductCode.Text = GetLocalResourceObject("CustomerCode").ToString();
                                    lbnProductDesc.Text = GetLocalResourceObject("CustomerName").ToString();
                                    lblProductCode.Text = drr[0]["HDR_CODE"].ToString();
                                    lblProductDesc.Text = string.IsNullOrEmpty(drr[0]["HDR_NAME"].ToString()) ? "&nbsp;" : drr[0]["HDR_NAME"].ToString();
                                    break;
                                case false:
                                    lbnProductCode.Text = GetLocalResourceObject("ProductCode").ToString();
                                    lbnProductDesc.Text = GetLocalResourceObject("ProductDescription").ToString();
                                    lblProductCode.Text = drr[0]["HDR_CODE"].ToString();
                                    lblProductDesc.Text = string.IsNullOrEmpty(drr[0]["HDR_DESC"].ToString()) ? "&nbsp;" : drr[0]["HDR_DESC"].ToString();
                                    break;
                            }

                            BindDropdown(ControlsEnum.CURRENCY);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSett]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                        break;
                    #endregion

                    #endregion
                    #region Apply
                    case ActionsEnum.RATEAPPLY:
                        SetFieldValues(ControlsEnum.RATEAPPLY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                        break;
                    case ActionsEnum.PRODUCTAPPLY:
                        GetFieldValues(ControlsEnum.PRODUCTDTL);
                        SetFieldValues(ControlsEnum.PRODUCTDTL);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup2", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CUSTOMERAPPLY:
                        GetFieldValues(ControlsEnum.CUSTOMERDTL);
                        SetFieldValues(ControlsEnum.CUSTOMERDTL);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    case ActionsEnum.APPLY:
                        CurrentAction = ActionsEnum.APPLY;
                        GetFieldValues(ControlsEnum.APPLY);
                        SetFieldValues(ControlsEnum.APPLY);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.PRODUCTCANCEL:
                        ClearTree(ControlsEnum.PRODUCTS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CUSTOMERCANCEL:
                        ClearTree(ControlsEnum.CUSTOMERS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;
                    #endregion
                    #region Set
                    case ActionsEnum.SET:
                        clearPaging();
                        rdbProduct.Checked = false;
                        rdbCustomer.Checked = true;
                        SearchType = (int)SearchTypeEnum.Customer;
                        CurrentAction = ActionsEnum.SET;
                        GetFieldValues(ControlsEnum.SET);
                        SetFieldValues(ControlsEnum.SET);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                       // SetFocus(txtCalender);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ClearForm(ControlsEnum.DEFAULT);

                        break;
                    #endregion
                    #region View & Print
                    case ActionsEnum.VIEW:
                        if (lblStatus.Text != GetLocalResourceObject("New").ToString())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();InitCustomer();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + GetLocalResourceObject("BrandRates").ToString() + "','400','200');", true);
                            ddlPrint.Focus();
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.NoBrandRate;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    + "','" + Resources.ErpRes.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }

                        break;
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        if (ddlPrint.SelectedValue == "1")
                        {
                            if (txtCustomer.Text != string.Empty && txtCustomer.Text != "All" && hdfCustomer.Value != string.Empty && hdfCustomer.Value != "0")
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&CusID=" + hdfCustomer.Value + "&ForMoth=" + "01-" + txtCalender.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&CusID=" + hdfCustomer.Value + "&ForMoth=" + txtFromDate.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + "01-" + txtCalender.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + txtFromDate.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                            }
                        }
                        else if (ddlPrint.SelectedValue == "2")
                        {
                            if (txtProduct.Text != string.Empty && txtProduct.Text != "All" && hdfProduct.Value != string.Empty && hdfProduct.Value != "0")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ItemID=" + hdfProduct.Value + "&ForMoth=" + txtFromDate.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + txtFromDate.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + txtFromDate.Text.Trim() + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                        }

                        break;
                    #endregion

                    #region CUSTOMERLISTPOPUPOK
                    case ActionsEnum.CUSTOMERLISTPOPUPOK:
                        int currencyId = Convert.ToInt32(ddlCurrencyCustomerListPopUp.SelectedValue);
                        int specialCatId = 0;
                        int.TryParse(ddlCustSpecialCategory.SelectedValue, out specialCatId);                        
                        if (currencyId>0)
                        {  
                            DataTable dtCustomers = BrandRatesBL.GetCustomerListByCurrency(currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), currencyId, specialCatId);
                            TreeNode child;
                            TreeNode root1;
                            trvCustomers.Nodes.Clear();
                            root1 = new TreeNode(GetLocalResourceObject("AllCustomers").ToString(), "0");
                            root1.NavigateUrl = "javascript:return false;";
                            root1.ShowCheckBox = true;
                            trvCustomers.Nodes.Add(root1);
                            if (dtCustomers != null)
                            {
                                foreach (DataRow row in dtCustomers.Rows)
                                {
                                    child = new TreeNode(row["CUS_NAME"].ToString(), row["CUS_PK"].ToString());
                                    child.ShowCheckBox = true;
                                    child.NavigateUrl = "javascript:return false;";
                                    child.ToolTip = row["CUS_NAME"].ToString();
                                    root1.ChildNodes.Add(child);
                                }
                                root1.ExpandAll();
                            } 
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.CUSTOMERS);
                            BindTree(ControlsEnum.CUSTOMERS);
                            //}
                            //else
                            //{
                            if (SelCustomerList != null)
                            {
                                checkCount = 0;
                                TreeNode root = null;
                                foreach (CustomerListBO item in SelCustomerList)
                                    foreach (TreeNode node in trvCustomers.Nodes)
                                    {
                                        root = node;
                                        root.Checked = false;
                                        foreach (TreeNode child1 in node.ChildNodes)
                                        {
                                            if (child1.Value == item.cusPK.ToString())
                                            {
                                                child1.Checked = true;
                                                checkCount++;
                                            }
                                        }
                                    }
                                if (checkCount == root.ChildNodes.Count)
                                {
                                    root.Checked = true;
                                }
                            }
                            else
                            {
                                foreach (TreeNode node in trvCustomers.Nodes)
                                {
                                    node.Checked = false;
                                }

                            }
                        }

                        //trvCustomers.Nodes[0].Checked = false;
                        //Instead Rebind The Treeview With 'dtCustomers'
                        //foreach (TreeNode node in trvCustomers.Nodes[0].ChildNodes)
                        //{
                        //    node.Checked = false;
                        //}
                        //foreach (DataRow drItem in dtCustomers.Rows)
                        //{
                        //    foreach (TreeNode trNode in trvCustomers.Nodes[0].ChildNodes)
                        //    {
                        //        if (trNode.Value==drItem.ItemArray[0].ToString())
                        //        {
                        //            trNode.Checked = true;
                        //        }
                        //    }
                        //}
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSearchCustomers]','" + GetLocalResourceObject("CustomerList").ToString() + "','500','330');", true);
                        break;
                    #endregion

                    #region PRODUCTLISTPOPUPOK
                    case ActionsEnum.PRODUCTLISTPOPUPOK:
                        int typeId = Convert.ToInt32(ddlTypeProductListPopUp.SelectedValue);
                        int thicknessId = Convert.ToInt32(ddlThicknessProductListPopUp.SelectedValue);
                        int categoryId = Convert.ToInt32(ddlCategoryProductListPopUp.SelectedValue);
                        int surfaceId = Convert.ToInt32(ddlSurfaceProductListPopUp.SelectedValue);
                        int shadeId = Convert.ToInt32(ddlShadeProductListPopUp.SelectedValue);
                        int classificationId = Convert.ToInt32(ddlClassificationProductListPopUp.SelectedValue);
                        int sizeId = Convert.ToInt32(ddlSizeProductListPopUp.SelectedValue);
                        int lengthId = Convert.ToInt32(ddlLengthProductListPopUp.SelectedValue);
                        int chlorinationId = Convert.ToInt32(ddlChlorinationProductListPopUp.SelectedValue);
                        int sideId = Convert.ToInt32(ddlSideProductListPopUp.SelectedValue);
                        int adnlSpec05 = Convert.ToInt32(ddlAdnlSpec05ProductListPopUp.SelectedValue);
                        int adnlSpec06 = Convert.ToInt32(ddlAdnlSpec06ProductListPopUp.SelectedValue);
                        int adnlSpec07 = Convert.ToInt32(ddlAdnlSpec07ProductListPopUp.SelectedValue);
                        DataTable dtProductsByProperty = BrandRatesBL.GetProductListByProperties(currentUser.SBUID, Convert.ToInt32(ERP.Utilities.Constants.DA.ProductCategory.FinishedGoods), Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE),
                            typeId, thicknessId, categoryId, surfaceId, shadeId, classificationId, sizeId, lengthId, chlorinationId, sideId, adnlSpec05, adnlSpec06, adnlSpec07);
                        TreeNode childProduct;
                        TreeNode rootProduct;
                        trvProducts.Nodes.Clear();
                        if (dtProductsByProperty.Rows.Count > 0)
                        {
                            tblEmptyRecord.Visible = false;
                            rootProduct = new TreeNode(GetLocalResourceObject("AllProducts").ToString(), "0");
                            rootProduct.NavigateUrl = "javascript:return false;";
                            rootProduct.ShowCheckBox = true;
                            trvProducts.Nodes.Add(rootProduct);
                            if (dtProductsByProperty != null)
                            {
                                foreach (DataRow row in dtProductsByProperty.Rows)
                                {
                                    childProduct = new TreeNode(row["ITM_CODE"].ToString() + "  -  " + row["ITM_NAME"].ToString(), row["ITM_PK"].ToString());
                                    childProduct.ShowCheckBox = true;
                                    childProduct.NavigateUrl = "javascript:return false;";
                                    childProduct.ToolTip = row["ITM_NAME"].ToString();
                                    rootProduct.ChildNodes.Add(childProduct);
                                }
                                rootProduct.ExpandAll();
                            }
                        }
                        else
                            tblEmptyRecord.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divSearchProducts]','" + GetLocalResourceObject("ProductList").ToString() + "','750','400');", true);
                        break;
                    #endregion

                    #region SHOWGRIDHEADERPOPUP
                    case ActionsEnum.SHOWGRIDHEADERPOPUP:
                        ddlGridHeaderPopUp.DataSource = new ListItem[]
                        {
                            new ListItem { Text="Value", Value="Value"},
                            new ListItem{Text="Percentage", Value="Percentage"}
                        };
                        ddlGridHeaderPopUp.DataBind();
                        txtNewRateGridHeaderPopUp.Text = "";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divGridHeaderPopUp]','" + GetLocalResourceObject("RateSettings").ToString() + "','300','110');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divGridHeaderPopUp]','" + GetLocalResourceObject("ProductList").ToString() + "','300','110');", true);
                        break;
                    case ActionsEnum.GRIDHEADERPOPUPAPPLY:
                        string type = ddlGridHeaderPopUp.SelectedValue;
                        decimal value = Convert.ToDecimal(txtNewRateGridHeaderPopUp.Text.Trim());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "changeRateValueSHOWGRIDHEADERPOPUP('" + type + "'," + value + ");", true);
                        break;
                    case ActionsEnum.GRIDHEADERPOPUPCANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                        break;

                    #endregion SHOWGRIDHEADERPOPUP

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
                if ((sender as GridView).ID == "grdSelectedCusBrands")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        if (dsCustomerRate != null && dsCustomerRate.Tables[1].Rows.Count > 0)
                        {
                            HiddenField hdfItem = e.Row.FindControl("hdfItemPK") as HiddenField;
                            var results = from myRow in dsCustomerRate.Tables[1].AsEnumerable()
                                          where myRow.Field<int>("HDR_PK") == Convert.ToInt32(hdfItem.Value)
                                          select new
                                               {
                                                   HDR_PK = myRow.Field<int>("HDR_PK"),
                                                   CIM_PK = myRow.Field<int>("CIM_PK"),
                                                   DTL_CODE = myRow.Field<string>("DTL_CODE"),
                                                   DTL_TEXT = myRow.Field<string>("DTL_TEXT"),
                                                   CIM_BRAND_NAME = myRow.Field<string>("CIM_BRAND_NAME"),
                                                   CIM_BRAND_TEXT = myRow.Field<string>("CIM_BRAND_TEXT"),
                                                   APS_TEXT = myRow.Field<string>("APS_TEXT"),
                                                   BRD_SALE_UOM_TEXT = myRow.Field<string>("BRD_SALE_UOM_TEXT"),
                                                   BRD_RATE = myRow.Field<decimal?>("BRD_RATE"),
                                                   CUR_PK = myRow.Field<int?>("CUR_PK")
                                               };

                            gvr = e.Row as ExtGridViewRow;
                            if (gvr != null)
                            {
                                grd = gvr.FindControl("grdSelectdCustomers") as GridView;
                                if (SearchType == (int)SearchTypeEnum.Customer)
                                {
                                    grd.Columns[0].HeaderText = GetLocalResourceObject("ProductCode").ToString();
                                }
                                else
                                {
                                    grd.Columns[0].HeaderText = GetLocalResourceObject("CustomerCode").ToString();

                                }
                                grd.DataSource = results;
                                grd.DataBind();
                                gvr.ShowExpand = true;

                            }
                        }
                        //else
                        //{
                        //    gvr = e.Row as ExtGridViewRow;
                        //    if (gvr != null)
                        //    {
                        //        grd = gvr.FindControl("grdSelectdCustomers") as GridView;
                        //        grd.DataSource = null;
                        //        grd.DataBind();
                        //    }
                        //}
                    }
                }
                else if ((sender as GridView).ID == "grdSelectdCustomers")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (dsCustomerRate != null && dsCustomerRate.Tables[1].Rows.Count > 0)
                        {
                            HiddenField hdfCurrPK = e.Row.FindControl("hdfCurrPK") as HiddenField;
                            ////////////////////////////////////////////////
                            //ddlCurrency = e.Row.FindControl("ddlCurrency") as DropDownList;
                            //ddlCurrency.DataSource = CommonBL.GetCurrencyList(currentUser.SBUID);
                            //ddlCurrency.DataSource = Currency;
                            //ddlCurrency.DataTextField = "CUR_CODE";
                            //ddlCurrency.DataValueField = "CUR_PK";
                            ////ddlCurrency.ToolTip = "CUR_NAME";
                            //ddlCurrency.DataBind();
                            //ddlCurrency.SelectedValue = hdfCurrPK.Value;
                            Label lblGridCurrency = e.Row.FindControl("lblGridCurrency") as Label;
                            lblGridCurrency.Text = Currency.AsEnumerable()
                                .Where(c => c.Field<int>("CUR_PK") == Convert.ToInt32(hdfCurrPK.Value))
                                .Select(s => s.Field<string>("CUR_CODE"))
                                .FirstOrDefault();


                            TextBox txtGrdRate = e.Row.FindControl("txtNewRate") as TextBox;
                            if (txtGrdRate.Text != string.Empty)
                            {
                                if (double.Parse(txtGrdRate.Text) <= 0)
                                {
                                    txtGrdRate.CssClass = "small-a numeric yellow-inbg";
                                }
                            }
                            else
                                txtGrdRate.CssClass = "small-a numeric palered-inbg";


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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

            //GetFieldValues(ControlsEnum.DEFAULT);
            //SetFieldValues(ControlsEnum.DEFAULT);
        }

        #endregion
        #endregion
        #region Helper Methods

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);

        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
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
            CustomerRateCopyBO objCopyRate;
            try
            {
                switch (mode)
                {
                    #region Old Brand
                    case ActionsEnum.OLDBRAND:
                        customerRateObj = new CustomerRateBO();
                        customerRateObj.BrhPK = BrandPK;
                        customerRateObj.ListType = SearchType;
                        returnObj = customerRateObj;
                        break;
                    #endregion
                    #region Default
                    case ActionsEnum.DEFAULT:
                        customerRateObj = new CustomerRateBO();
                        customerRateObj.BrhPK = CurrPK;
                        customerRateObj.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        customerRateObj.ListType = SearchType;
                        customerRateObj.BizUnit = currentUser.SBUID;
                        customerRateObj.PageSize = grdSelectedCusBrands.PageSize;
                        customerRateObj.PageIndex = Convert.ToInt32(PageIndex);
                        returnObj = customerRateObj;
                        break;
                    #endregion
                    #region Save Details
                    case ActionsEnum.SAVE:
                        customerRateObj = new CustomerRateBO();
                        customerRateObj.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        customerRateObj.LastModDate = LastModifiedTime.ToString();
                        customerRateObj.ListType = SearchType;
                        customerRateObj.UserPK = currentUser.PKUser.ToString();
                        customerRateObj.BizUnit = currentUser.SBUID;
                        customerRateObj.BrhPK = CurrPK;
                        customerRateObj.CustomerList = SelectedCustomers == null ? new List<CustomerListBO>() : SelectedCustomers.CustomerList;
                        customerRateObj.ItemsList = SelectedProducts == null ? new List<ItemsListBO>() : SelectedProducts.ItemsList;
                        customerRateObj.PackingSpecList = SelectedPackingSpecs == null ? new List<PackingSpecListBO>() : SelectedPackingSpecs;
                        customerRateObj.DateFrom = FromDate.ToString();
                        customerRateObj.DateTo = ToDate.ToString();
                        customerRateObj.DetailsList = new List<DetailsBO>();
                        List<DetailsBO> detailsList = new List<DetailsBO>();
                        DetailsBO objDetail;
                        foreach (ExtGridViewRow gvr in grdSelectedCusBrands.Rows)
                        {
                            grd = (GridView)gvr.FindControl("grdSelectdCustomers") as GridView;
                            foreach (GridViewRow inRow in grd.Rows)
                            {
                                if (!String.IsNullOrEmpty((inRow.FindControl("txtNewRate") as TextBox).Text))
                                {
                                    objDetail = new DetailsBO();
                                    objDetail.BrandPK = Convert.ToInt32((inRow.FindControl("hdfCustomerItem") as HiddenField).Value);
                                    //////////////////////////////////////
                                    //objDetail.CurrencyPK = Convert.ToInt32((inRow.FindControl("ddlCurrency") as DropDownList).SelectedValue);
                                    objDetail.CurrencyPK = Convert.ToInt32((inRow.FindControl("hdfCurrPK") as HiddenField).Value);
                                    /////////////////////////////////////objDetail.CurrencyPK = Convert.ToInt32((inRow.FindControl("hdfCurrPK") as HiddenField).Value);
                                    objDetail.Rate = Convert.ToDouble((inRow.FindControl("txtNewRate") as TextBox).Text);
                                    objDetail.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                                    detailsList.Add(objDetail);
                                }
                            }
                        }
                        customerRateObj.DetailsList = detailsList;
                        returnObj = customerRateObj;

                        break;
                    #endregion
                    #region Get Selected Products PK
                    case ActionsEnum.PRODUCTAPPLY:
                        selectedItemsObj = new ItemBO();
                        selectedItemsObj.ItemsList = new List<ItemsListBO>();
                        List<ItemsListBO> items = new List<ItemsListBO>();
                        foreach (TreeNode node in trvProducts.Nodes)
                        {
                            foreach (TreeNode child1 in node.ChildNodes)
                            {
                                if (child1.Checked)
                                {
                                    ItemsListBO objItem = new ItemsListBO();
                                    objItem.itemPK = Convert.ToInt32(child1.Value);
                                    items.Add(objItem);
                                }
                            }
                        }
                        selectedItemsObj.ItemsList = items;
                        selectedItemsObj.Active = Convert.ToInt32(DbActiveStatus.HASPK);
                        SelectedProducts = selectedItemsObj;
                        returnObj = selectedItemsObj;
                        break;
                    #endregion
                    #region Get Selected Customers PK
                    case ActionsEnum.CUSTOMERAPPLY:
                        selectedCustomersObj = new CustomerBO();
                        selectedCustomersObj.CustomerList = new List<CustomerListBO>();
                        List<CustomerListBO> customerList = new List<CustomerListBO>();
                        foreach (TreeNode node in trvCustomers.Nodes)
                        {
                            foreach (TreeNode child1 in node.ChildNodes)
                            {
                                if (child1.Checked)
                                {
                                    CustomerListBO objcustomer = new CustomerListBO();
                                    objcustomer.cusPK = Convert.ToInt32(child1.Value);
                                    customerList.Add(objcustomer);
                                }

                            }
                        }
                        selectedCustomersObj.CustomerList = customerList;
                        selectedCustomersObj.Active = Convert.ToInt32(DbActiveStatus.HASPK);
                        SelectedCustomers = selectedCustomersObj;
                        returnObj = selectedCustomersObj;
                        break;
                    #endregion
                    #region Apply
                    case ActionsEnum.APPLY:
                    case ActionsEnum.CHANGETYPE:
                        customerRateObj = new CustomerRateBO();
                        customerRateObj.BrhPK = CurrPK;
                        customerRateObj.ListType = SearchType;
                        customerRateObj.BizUnit = currentUser.SBUID;
                        customerRateObj.PageSize = grdSelectedCusBrands.PageSize;
                        customerRateObj.PageIndex = Convert.ToInt32(PageIndex);
                        customerRateObj.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        customerRateObj.CustomerList = SelectedCustomers == null ? new List<CustomerListBO>() : SelectedCustomers.CustomerList;
                        customerRateObj.ItemsList = SelectedProducts == null ? new List<ItemsListBO>() : SelectedProducts.ItemsList;

                        //Packing Spec
                        List<PackingSpecListBO> selPackingSpecs = new List<PackingSpecListBO>();
                        foreach (ListItem item in chklstPackingSpec.GetCheckedItems())
                        {
                            selPackingSpecs.Add(new PackingSpecListBO { APS_PK = Convert.ToInt32(item.Value) });
                        }
                        SelectedPackingSpecs = selPackingSpecs;
                        customerRateObj.PackingSpecList = SelectedPackingSpecs == null ? new List<PackingSpecListBO>() : SelectedPackingSpecs;
                        //End Packig Specs
                        returnObj = customerRateObj;
                        break;
                    #endregion
                    #region Save Copy
                    case ActionsEnum.SAVECOPY:
                        objCopyRate = new CustomerRateCopyBO();
                        objCopyRate.BrhPK = CurrPK;
                        objCopyRate.BizUnit = currentUser.SBUID;
                        objCopyRate.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objCopyRate.DateFrom = FromDate.ToString();
                        objCopyRate.DateTo = ToDate.ToString();
                        objCopyRate.UserPK = currentUser.PKUser.ToString();
                        returnObj = objCopyRate;
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
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            string fromDate;
            string toDate;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SET:
                        lblLastModifiedHDR.Text = string.Empty;
                        hdfselectedPks.Value = string.Empty;
                        chkExpandAll.Checked = false;
                        if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                        {
                            LastModifiedTime = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_MOD_DT"].ToString());
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            CurrPK = Convert.ToInt32(dtBrandDetails.Rows[0]["BRH_PK"].ToString());
                            lblStatus.Text = dtBrandDetails.Rows[0]["BRH_STATUS_text"].ToString();
                            SetStatus((WkfStatus)Convert.ToInt32(dtBrandDetails.Rows[0]["BRH_STATUS"]));
                            if (txtFromDate.Text.Trim() == string.Empty)
                            {
                                txtFromDate.Text = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString()).ToString(Resources.Constants.DateFormatShort);
                                hdfFromDate.Value = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString()).ToString(Resources.Constants.DateFormatShort);
                                txtToDate.Text = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString()).ToString(Resources.Constants.DateFormatShort);
                                hdfToDate.Value = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString()).ToString(Resources.Constants.DateFormatShort);
                                FromDate = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString());
                                ToDate = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString());
                            }
                            
                            hdfBrandRateSbu.Value = dtBrandDetails.Rows[0]["BRH_BIZUNIT"].ToString();

                        }
                        else
                        {
                            SetStatus(WkfStatus.NEW);
                            tblFilter.Visible = false;
                            ClearForm(ControlsEnum.DEFAULT);
                            CurrPK = 0;
                        }
                        BindWorkflow();

                        //For Edit Mode the current brand Rate
                        if (CurrPK > 0 && CurrentAction != ActionsEnum.SAVE)
                        {
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.APPLY);
                        }
                        // EntryStatus = EntryStatus.NEWMODE;
                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowFilter", "ShowFilter();", true);
                        break;
                    case ControlsEnum.RATEHISTORY:
                        if (dsRateHistory != null && dsRateHistory.Tables.Count > 1)
                        {
                            if (dsRateHistory.Tables[0].Rows.Count > 0)
                            {
                                lblHBrand.Text = dsRateHistory.Tables[0].Rows[0]["CIM_BRAND_NAME"].ToString() == "" ? "&nbsp;" : dsRateHistory.Tables[0].Rows[0]["CIM_BRAND_NAME"].ToString();
                                lblHBrand.ToolTip = dsRateHistory.Tables[0].Rows[0]["CIM_BRAND_NAME"].ToString();

                                lblHCustomer.Text = dsRateHistory.Tables[0].Rows[0]["CIM_CUS_NAME"].ToString() == "" ? "&nbsp;" : dsRateHistory.Tables[0].Rows[0]["CIM_CUS_NAME"].ToString();
                                lblHCustomer.ToolTip = dsRateHistory.Tables[0].Rows[0]["CIM_CUS_CODE"].ToString();

                                lblHProduct.Text = dsRateHistory.Tables[0].Rows[0]["CIM_ITEM_CODE"].ToString() == "" ? "&nbsp;" : dsRateHistory.Tables[0].Rows[0]["CIM_ITEM_CODE"].ToString();
                                lblHProduct.ToolTip = dsRateHistory.Tables[0].Rows[0]["CIM_ITEM_TEXT"].ToString();

                                grdRateHistory.DataSource = dsRateHistory.Tables[1];
                                grdRateHistory.DataBind();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDivWkf('[id$=divHistory]','" + GetLocalResourceObject("RateHistory").ToString() + "','550');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MSGNoRecordFound").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
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
        /// Bind Workflow
        /// </summary>
        private void BindWorkflow()
        {
            base.WkfRefID = 0;
            if (CurrPK != 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                ucrWrkf.FillWorkFlowDetails();
                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == true)
                {
                    ucrWrkf.ViewType = 0;
                    ucrWrkf.ViewAction();
                    EntryStatus = EntryStatus.ENTRYMODE;
                    // btnSave.Visible = true;
                }
                else
                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false)
                    {
                        ucrWrkf.ViewType = 1;
                        EntryStatus = EntryStatus.ENTRYMODE;
                    }
                    else
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        //  btnSave.Visible=false;
                    }
            }
            else
            {
                FillProcessID();
                ucrWrkf.RefID = 0;
                ucrWrkf.ApplicationID = 0;
                ucrWrkf.ViewType = 1;
                ucrWrkf.FillWorkFlowDetails();
                EntryStatus = EntryStatus.NEWMODE;
            }

        }
        /// <summary>
        /// Delete Table Row
        /// </summary>
        /// <param name="pk"></param>
        private void DeleteTabelRow(int pk)
        {
            GridView grd;
            int BrandPK;
            int CurrencyPK;
            decimal Rate;
            DataRow[] drr;
            dsCustomerRate = CustomerRates;
            //For update current grid values to table
            foreach (ExtGridViewRow gvr in grdSelectedCusBrands.Rows)
            {
                grd = (GridView)gvr.FindControl("grdSelectdCustomers") as GridView;
                foreach (GridViewRow inRow in grd.Rows)
                {
                    BrandPK = Convert.ToInt32((inRow.FindControl("hdfCustomerItem") as HiddenField).Value);
                    /////////////////////////
                    //CurrencyPK = Convert.ToInt32((inRow.FindControl("ddlCurrency") as DropDownList).SelectedValue);
                    CurrencyPK = Convert.ToInt32((inRow.FindControl("hdfCurrPK") as HiddenField).Value);

                    drr = dsCustomerRate.Tables[1].Select("CIM_PK=" + BrandPK);
                    if ((inRow.FindControl("txtNewRate") as TextBox).Text != "")
                    {
                        Rate = decimal.Parse((inRow.FindControl("txtNewRate") as TextBox).Text);
                        drr[0]["BRD_RATE"] = Rate;
                    }
                    drr[0]["CUR_PK"] = CurrencyPK;
                    drr[0].AcceptChanges();
                }
            }
            drr = dsCustomerRate.Tables[1].Select("CIM_PK=" + pk);
            foreach (var row in drr)
            {
                row.Delete();
            }
            dsCustomerRate.Tables[1].AcceptChanges();
            CustomerRates = dsCustomerRate;
        }
        /// <summary>
        /// Set Current Values To Table
        /// </summary>
        private void SetCurrentValueToTable()
        {
            GridView grd;
            int BrandPK;
            int CurrencyPK;
            decimal Rate;
            DataRow[] drr;
            dsCustomerRate = CustomerRates;
            //For update current grid values to table
            foreach (ExtGridViewRow gvr in grdSelectedCusBrands.Rows)
            {
                grd = (GridView)gvr.FindControl("grdSelectdCustomers") as GridView;
                foreach (GridViewRow inRow in grd.Rows)
                {
                    BrandPK = Convert.ToInt32((inRow.FindControl("hdfCustomerItem") as HiddenField).Value);
                    /////////////////////
                    // CurrencyPK = Convert.ToInt32((inRow.FindControl("ddlCurrency") as DropDownList).SelectedValue);
                    CurrencyPK = Convert.ToInt32((inRow.FindControl("hdfCurrPK") as HiddenField).Value);
                    drr = dsCustomerRate.Tables[1].Select("CIM_PK=" + BrandPK);
                    if ((inRow.FindControl("txtNewRate") as TextBox).Text != "")
                    {
                        Rate = decimal.Parse((inRow.FindControl("txtNewRate") as TextBox).Text);
                        drr[0]["BRD_RATE"] = Rate;
                    }
                    drr[0]["CUR_PK"] = CurrencyPK;
                    drr[0].AcceptChanges();
                }
            }
            CustomerRates = dsCustomerRate;
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
                    root.NavigateUrl = "javascript:return false;";
                    root.ShowCheckBox = true;
                    trvCustomers.Nodes.Add(root);
                    if (dtCustomers != null)
                    {
                        foreach (DataRow row in dtCustomers.Rows)
                        {
                            child = new TreeNode(HttpUtility.HtmlDecode(row["CUS_NAME"].ToString()), row["CUS_PK"].ToString());
                            child.ShowCheckBox = true;
                            child.NavigateUrl = "javascript:return false;";
                            child.ToolTip = HttpUtility.HtmlDecode(row["CUS_NAME"].ToString());
                            root.ChildNodes.Add(child);
                        }
                        root.ExpandAll();
                    }
                    break;
                #endregion
                #region Products
                case ControlsEnum.PRODUCTS:
                    trvProducts.Nodes.Clear();
                    root = new TreeNode(GetLocalResourceObject("AllProducts").ToString(), "0");
                    root.ShowCheckBox = true;
                    root.NavigateUrl = "javascript:return false;";
                    trvProducts.Nodes.Add(root);
                    if (dtProducts != null)
                    {
                        foreach (DataRow row in dtProducts.Rows)
                        {
                            child = new TreeNode(row["ITM_CODE"].ToString() + "  -  " + row["ITM_NAME"].ToString(), row["ITM_PK"].ToString());
                            child.ShowCheckBox = true;
                            child.NavigateUrl = "javascript:return false;";
                            child.ToolTip = row["ITM_NAME"].ToString();
                            root.ChildNodes.Add(child);
                        }
                        root.ExpandAll();
                    }
                    break;
                #endregion
            }

        }
        /// <summary>
        /// Enable disable buttons
        /// </summary>
        /// <param name="value"></param>
        private void SetButtonEnable(bool value)
        {
            btnApply.Enabled = value;
            btnShowProducts.Enabled = value;
            btnShowCustomer.Enabled = value;

            switch (value)
            {
                case true:
                    btnApply.CssClass = "BTNenable-submit";
                    btnShowProducts.CssClass = "IMAGEenable-popup";
                    btnShowCustomer.CssClass = "IMAGEenable-popup";
                    break;
                case false:
                    btnApply.CssClass = "BTNdisable-submit";
                    btnShowProducts.CssClass = "IMAGEdisable-popup";
                    btnShowCustomer.CssClass = "IMAGEdisable-popup";
                    break;
            }



        }
        /// <summary>
        /// Set Workflow status to label
        /// </summary>
        /// <param name="status"></param>
        private void SetStatus(WkfStatus status)
        {
            switch (status)
            {
                case WkfStatus.DRAFT:
                    //btnDelete.Visible = true;
                    break;
                case WkfStatus.SUBMIT:
                    //btnDelete.Visible = false;
                    break;
                case WkfStatus.APPROVED:
                    //btnDelete.Visible = false;
                    break;
                default:
                    lblStatus.Text = GetLocalResourceObject("New").ToString();
                    // SetRdbVisibility(false);
                    // btnDelete.Visible = false;
                    break;
            }
        }
        private void BindDropdown(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CURRENCY:
                    ddlCurrency.DataSource = Currency;
                    ddlCurrency.DataTextField = "CUR_CODE";
                    ddlCurrency.DataValueField = "CUR_PK";
                    //ddlCurrency.ToolTip = "CUR_NAME";
                    ddlCurrency.DataBind();
                    break;
                case ControlsEnum.CUSTOMERPROPERTIES:
                    ddlCustSpecialCategory.Items.Clear();
                    if (dtSpecialCategory != null)
                    {
                        ddlCustSpecialCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSpecialCategory, "CON_NAME");
                        ddlCustSpecialCategory.DataTextField = "CON_NAME";
                        ddlCustSpecialCategory.DataValueField = "CON_PK";
                        ddlCustSpecialCategory.DataBind();
                    }
                    ddlCustSpecialCategory.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
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
                    grdCustomers.DataSource = null;
                    grdCustomers.DataBind();

                    grdProducts.DataSource = null;
                    grdProducts.DataBind();

                    grdSelectedCusBrands.DataSource = null;
                    grdSelectedCusBrands.DataBind();
                    clearPaging();
                    break;
                case ControlsEnum.PRODUCTDTL:
                    if (dtProductDtl != null && dtProductDtl.Rows.Count > 0)
                        grdProducts.DataSource = dtProductDtl;
                    else
                        grdCustomers.DataSource = null;
                    grdProducts.DataBind();
                    break;
                case ControlsEnum.CUSTOMERDTL:
                    if (dtCustomerDtl != null && dtCustomerDtl.Rows.Count > 0)
                        grdCustomers.DataSource = dtCustomerDtl;
                    else
                        grdCustomers.DataSource = null;
                    grdCustomers.DataBind();
                    break;
                case ControlsEnum.APPLY:

                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    if (SearchType == (int)SearchTypeEnum.Customer)
                    {
                        grdSelectedCusBrands.Columns[0].HeaderText = GetLocalResourceObject("CustomerCode").ToString();
                        grdSelectedCusBrands.Columns[1].HeaderText = GetLocalResourceObject("CustomerName").ToString();
                    }
                    else
                    {
                        grdSelectedCusBrands.Columns[0].HeaderText = GetLocalResourceObject("ProductCode").ToString();
                        grdSelectedCusBrands.Columns[1].HeaderText = GetLocalResourceObject("ProductDescription").ToString();
                    }

                    if (dsCustomerRate != null && dsCustomerRate.Tables.Count>0 && dsCustomerRate.Tables[0].Rows.Count > 0)
                    {
                        tblFilter.Visible = true;
                        decimal pages = Convert.ToDecimal(Convert.ToDecimal(dsCustomerRate.Tables[0].Rows[0]["ROW_COUNT"].ToString()) / Convert.ToDecimal(grdSelectedCusBrands.PageSize.ToString()));
                        TotalPages = Convert.ToInt32(Math.Ceiling(pages));
                        uclPaging.TotalPages = TotalPages;
                        grdSelectedCusBrands.DataSource = dsCustomerRate.Tables[0];
                    }
                    else
                    {
                        tblFilter.Visible = false;
                        grdSelectedCusBrands.DataSource = null;
                    }
                    grdSelectedCusBrands.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ExpandSelected", "ExpandSelected();", true);
                    break;
            }
        }

        /// <summary>
        /// Set Radio button visibility
        /// </summary>
        /// <param name="val"></param>
        private void SetRdbVisibility1(bool val)
        {
            rdbCustomer.Visible = val;
            rdbProduct.Visible = val;
        }

        private void clearPaging()
        {
            TotalPages = 1;
            PageIndex = "1";
            uclPaging.TotalPages = TotalPages;
            uclPaging.BindPager();
            uclPaging.Visible = false;

        }
        /// <summary>
        /// Clear the form
        /// </summary>
        /// <param name="type"></param>
        private void ClearForm(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.COPY:
                    CurrPK = 0;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    txtFromDate.Focus();
                    SetStatus(WkfStatus.NEW);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                    break;
                case ControlsEnum.CLEARALL:
                    LastModifiedTime = System.DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    SelectedProducts = null;
                    ToDate = System.DateTime.Now;
                    FromDate = System.DateTime.Now;
                    BrkPK = 0;
                    SelectedCustomers = null;
                    txtFromDate.Text = System.DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = System.DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = System.DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = System.DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    EntryStatus = EntryStatus.LISTMODE;
                    lblStatus.Text = string.Empty;
                    ClearTree(ControlsEnum.PRODUCTS);
                    ClearTree(ControlsEnum.CUSTOMERS);
                    CustomerRates = null;
                    btnDelete.Visible = false;
                    CurrPK = 0;
                    BrandPK = 0;
                    validPage = false;
                    SelCustomerList = null;
                    SelItemList = null;
                    tblFilter.Visible = false;
                    hdfselectedPks.Value = string.Empty;
                    // SetRdbVisibility(false);
                    BindGrid(ControlsEnum.DEFAULT);
                    CurrentAction = ActionsEnum.DEFAULT;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                    disableDetailsFlag = 0;
                    break;
                case ControlsEnum.DEFAULT:
                    SelectedProducts = null;
                    CustomerRates = null;
                    SelectedCustomers = null;
                    SelCustomerList = null;
                    SelItemList = null;
                    tblFilter.Visible = false;
                    ClearTree(ControlsEnum.PRODUCTS);
                    ClearTree(ControlsEnum.CUSTOMERS);
                    hdfselectedPks.Value = string.Empty;
                    SetButtonEnable(true);
                    // SetRdbVisibility(false);
                    BindGrid(ControlsEnum.DEFAULT);
                    chklstPackingSpec.ClearData();
                    GetFieldValues(ControlsEnum.PACKINGSPECS);
                    SetFieldValues(ControlsEnum.PACKINGSPECS);
                    break;
            }

            // btnSave.Visible = false;
        }

        /// <summary>
        /// Set From And To Date
        /// </summary>
        /// <returns></returns>
        private bool SetMonth()
        {
            string fromDate = txtFromDate.Text;
            string toDate = string.Empty;
            bool result = false;
            DateTime t;
            if (DateTime.TryParse(fromDate, out t))
            {
                //int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(fromDate)).Year, (Convert.ToDateTime(fromDate)).Month);
                toDate = string.IsNullOrEmpty(txtToDate.Text) ? fromDate : txtToDate.Text; //totaldays.ToString() + "-" + txtCalender.Text;
                FromDate = Convert.ToDateTime(fromDate);
                ToDate = Convert.ToDateTime(toDate);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Set From And To Date for Copy Month 
        /// </summary>
        /// <returns></returns>
        private bool SetCopyMonth()
        {
            string fromDate =txtCopyFrom.Text;
            string toDate = string.Empty;
            bool result = false;
            DateTime t;
            if (DateTime.TryParse(fromDate, out t))
            {
               // int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(fromDate)).Year, (Convert.ToDateTime(fromDate)).Month);
                toDate = txtCopyTo.Text;
                FromDate = Convert.ToDateTime(fromDate);
                ToDate = Convert.ToDateTime(toDate);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Clear Tree
        /// </summary>
        private void ClearTree(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.PRODUCTS:
                    //Clear Products
                    foreach (TreeNode node in trvProducts.Nodes)
                    {
                        foreach (TreeNode child1 in node.ChildNodes)
                        {
                            child1.Checked = false;
                        }
                    }
                    break;
                case ControlsEnum.CUSTOMERS:
                    //Clear Customer
                    foreach (TreeNode node in trvCustomers.Nodes)
                    {
                        foreach (TreeNode child1 in node.ChildNodes)
                        {
                            child1.Checked = false;
                        }
                    }
                    break;

            }



        }
        /// <summary>
        /// Method for creating datatable from list
        /// </summary>
        /// <returns></returns>      
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others          will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        #endregion
        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            //path = Resources.PageURL.ActivityWkfURL;
            path = "/OrderToCash/BrandRates.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                //ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                //hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();

                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
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
            string s = "";
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCopy.PreRender += new EventHandler(btnAction_PreRender);

            this.btnCopy.Load += new EventHandler(btnAction_Load);
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);

        }
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
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

                switch (CurrentAction)
                {
                    case ActionsEnum.APPLY:
                        commonActions = ActionsEnum.APPLY;
                        GetFieldValues(ControlsEnum.APPLY);
                        SetFieldValues(ControlsEnum.APPLY);
                        break;
                    default:
                        commonActions = ActionsEnum.DEFAULT;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.SET);
                        SetFieldValues(ControlsEnum.APPLY);
                        break;
                }
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(4);});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if(CurrPK > 0)
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetBtnVisibilityForDiffSBU", "$(document).ready(function(){SetBtnVisibilityForDiffSBU();});", true);
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
        /// Search Type
        /// </summary>
        enum SearchTypeEnum
        {
            Product = 1,
            Customer
        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            PRODUCTS,
            CUSTOMERS,
            PRODUCTDTL,
            CUSTOMERDTL,
            APPLY,
            SET,
            CLEARALL,
            CURRENCY,
            RATEAPPLY,
            RATEHISTORY,
            USERCUSTOMER,
            COPY,
            OLDBRAND,
            CUSTOMERPROPERTIES,
            PACKINGSPECS
        }
        public enum WkfStatus
        {
            NEW = -1,
            DRAFT = 0,
            SUBMIT = 1,
            REJECT = 3,
            PENDING = 4,
            CLOSED = 5,
            APPROVED = 2
        }

        #endregion
        
    }
}

