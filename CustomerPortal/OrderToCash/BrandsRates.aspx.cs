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
    public partial class BrandsRates : ERP.Store.UI.MyBasePage
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
        /// Keep Category List in viewstate
        /// </summary>
        private DataTable Category
        {
            get
            {
                return this.ViewState[ViewstateStrings.Category] == null ? null : (DataTable)this.ViewState[ViewstateStrings.Category];
            }
            set
            {
                this.ViewState[ViewstateStrings.Category] = value;
            }

        }

        //<summary>
        //To maintain keep multiple rate of product
        //</summary>
        private List<ProductRateBO> TempProductRateSession
        {
            get
            {
                return ViewState[ViewstateStrings.TempProductRateSession] == null ? null : (List<ProductRateBO>)ViewState[ViewstateStrings.TempProductRateSession];
            }
            set
            {
                ViewState[ViewstateStrings.TempProductRateSession] = value;
            }
        }


        private CustomerRateNewBO CustomerRateHeaderSession
        {
            get
            {
                return ViewState[ViewstateStrings.CustomerRateHeaderSession] == null ? null : (CustomerRateNewBO)ViewState[ViewstateStrings.CustomerRateHeaderSession];
            }
            set
            {
                ViewState[ViewstateStrings.CustomerRateHeaderSession] = value;
            }
        }


        //private List<ProductDetailsBo> ProductDetailsSession
        //{
        //    get
        //    {
        //        return ViewState[ViewstateStrings.ProductDetailsSession] == null ? null : (List<ProductDetailsBo>)ViewState[ViewstateStrings.ProductDetailsSession];
        //    }
        //    set
        //    {
        //        ViewState[ViewstateStrings.ProductDetailsSession] = value;
        //    }
        //}


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
        /// Item PK
        /// </summary>
        private int CurrentUserPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrentUserPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrentUserPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrentUserPK] = value;
            }
        }


        /// <summary>
        /// To keep selected packing Spec PK
        /// </summary>
        private List<PackingSpecListBO> SelectedPackingSpecs
        {
            get
            {
                return this.ViewState["SelectedPackingSpecs"] == null ? null : (List<PackingSpecListBO>)this.ViewState["SelectedPackingSpecs"];
            }
            set
            {
                this.ViewState["SelectedPackingSpecs"] = value;
            }
        }

        /// <summary>
        /// To keep selected products PK
        /// </summary>
        private List<ProductsListBO> SelectedProduct
        {
            get
            {
                return this.ViewState["SelectedProducts"] == null ? null : (List<ProductsListBO>)this.ViewState["SelectedProducts"];
            }
            set
            {
                this.ViewState["SelectedProducts"] = value;
            }
        }

        /// <summary>
        /// To keep selected products PK
        /// </summary>
        private List<ItemsListBO> SelectedProductItem
        {
            get
            {
                return this.ViewState["SelectedProductItem"] == null ? null : (List<ItemsListBO>)this.ViewState["SelectedProductItem"];
            }
            set
            {
                this.ViewState["SelectedProductItem"] = value;
            }
        }


        /// <summary>
        /// To keep selected sub category PK
        /// </summary>
        private List<SubCategoryListBO> SelectedSubCategory
        {
            get
            {
                return this.ViewState["SelectedSubCategory"] == null ? null : (List<SubCategoryListBO>)this.ViewState["SelectedSubCategory"];
            }
            set
            {
                this.ViewState["SelectedSubCategory"] = value;
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
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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
        /// Current PK
        /// </summary>
        private int EditPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.EditPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.EditPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EditPK] = value;
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
        private DataSet ProductRates
        {
            get
            {
                return this.ViewState[ViewstateStrings.ProductRates] == null ? null : (DataSet)this.ViewState[ViewstateStrings.ProductRates];
            }
            set
            {
                this.ViewState[ViewstateStrings.ProductRates] = value;
            }

        }

        #endregion
        User currentUser;
        private ActionsEnum commonActions;
        ItemBO selectedItemsObj;
        CustomerBO selectedCustomersObj;
        CustomerRateBO customerRateObj;
        CustomerRateCopyBO customerRateCopyObj;
        CustomerProductFilterBO CustomerProductFilterObj;

        CustomerRateNewBO CustomerRateHeaderObj;
        ProductDetailsBo productDetailsObj;
        List<ProductDetailsBo> productDetailsList;
        ProductDetailsBo tempProductDetailsObj = null;
        ProductRateBO ProductRateObj;
        List<ProductRateBO> productRateList;
        ProductRateBO tempProductRateObj = null;
        XmlDocument xmlDoc;

        DataTable dtProductDtl;
        DataTable dtBrandDetails;
        DataSet dsProductRate;
        DataSet dsPageData;
        DataTable dtSubType;
        DataSet dsProductHistory;
        DataSet dsSubType;
        string arg;
        string saveXml;
        string action;
        DataRow[] drr;
        private DataTable dtUserData;
        private DataTable dtSpecialCategory;
        private DataTable dtPageData;
        DataTable dtCurrency;
        int disableDetailsFlag = 0;
        List<DDLMaster> chkUsers;
        List<CheckListData> chkList;

        #endregion

        #region Page Events


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
                    ClearForm(ControlsEnum.CLEARALL);

                    disableDetailsFlag = 1;
                    FillProcessID();
                    BindGrid(ControlsEnum.DEFAULT);
                    hdfCurrentUserSbu.Value = currentUser.SBUID.ToString();
                    // CurrentAction = ActionsEnum.DEFAULT;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.SET);
                    GetFieldValues(ControlsEnum.CUSTOMERPROPERTIES);
                    SetFieldValues(ControlsEnum.CUSTOMERPROPERTIES);

                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    #region ProductPropertiesDropDownBind
                    BindDropdown(ControlsEnum.DEFAULT);
                    #endregion

                    //GetFieldValues(ControlsEnum.PRODUCTS);

                    #region PackingSpec
                    GetFieldValues(ControlsEnum.PACKINGSPECS);
                    SetFieldValues(ControlsEnum.PACKINGSPECS);
                    #endregion

                    #region SubType
                    GetFieldValues(ControlsEnum.SUBTYPE);
                    SetFieldValues(ControlsEnum.SUBTYPE);
                    #endregion

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
            {
                return;
            }
            try
            {
                int? result;
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
                    #region Filter Product
                    case ActionsEnum.PRODUCTLISTPOPUPOK:
                        ClearForm(ControlsEnum.PRODUCTS);
                        GetFieldValues(ControlsEnum.PRODUCTS);
                        BindGrid(ControlsEnum.DEFAULT);
                        break;
                    #endregion

                    #region Apply
                    case ActionsEnum.PRODUCTAPPLY:
                        //ClearForm(ControlsEnum.PRODUCTS);
                        GetFieldValues(ControlsEnum.PRODUCTS);
                        CurrentAction = ActionsEnum.APPLY;
                        GetFieldValues(ControlsEnum.APPLY);
                        SetFieldValues(ControlsEnum.APPLY);
                        //GetFieldValues(ControlsEnum.PRODUCTDTL);
                        //SetFieldValues(ControlsEnum.PRODUCTDTL);
                        break;
                    //case ActionsEnum.APPLY:
                    //    // CurrentAction = ActionsEnum.APPLY;
                    //    GetFieldValues(ControlsEnum.APPLY);
                    //    SetFieldValues(ControlsEnum.APPLY);
                    //    break;
                    #endregion

                    #region Set individual Rate PopupShow
                    case ActionsEnum.SETRATE:
                        // SetCurrentValueToTable();
                        lbnProductCode.Visible = true;
                        lbnProductDesc.Visible = true;
                        lblProductCode.Visible = true;
                        lblProductDesc.Visible = true;
                        btnRateApply.Visible = true;
                        btnRateApplyAll.Visible = false;
                        arg = ((ImageButton)sender).CommandArgument;
                        EditPK = Convert.ToInt32(arg);
                        productDetailsList = new List<ProductDetailsBo>();
                        productDetailsList = CustomerRateHeaderSession.ProductDetailsList;
                        tempProductDetailsObj = productDetailsList == null ? null :
                        productDetailsList.SingleOrDefault(p => p.BPR_ITEM == EditPK);
                        if (tempProductDetailsObj != null)
                        {
                            lbnProductCode.Text = GetLocalResourceObject("ProductCode").ToString();
                            lbnProductDesc.Text = GetLocalResourceObject("ProductDescription").ToString();
                            lblProductCode.Text = tempProductDetailsObj.BPR_ITEM_CODE;
                            lblProductDesc.Text = string.IsNullOrEmpty(tempProductDetailsObj.BPR_ITEM_NAME.ToString()) ? "&nbsp;" : tempProductDetailsObj.BPR_ITEM_NAME.ToString();
                            BindDropdown(ControlsEnum.CURRENCY);
                            BindGrid(ControlsEnum.SETRATE);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSett]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                        break;
                    #endregion

                    #region Show Header Popup
                    case ActionsEnum.SHOWGRIDHEADERPOPUP:
                        lbnProductCode.Visible = false;
                        lbnProductDesc.Visible = false;
                        lblProductCode.Visible = false;
                        lblProductDesc.Visible = false;
                        btnRateApply.Visible = false;
                        btnRateApplyAll.Visible = true;
                        BindDropdown(ControlsEnum.CURRENCY);
                        //BindGrid(ControlsEnum.RATEADDALL);                       
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSett]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                        //BindDropdown(ControlsEnum.CURRENCYALL);
                        //BindDropdown(ControlsEnum.CUSTOMERPROPERTIESALL);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSettAllProduct]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                        break;
                    #endregion

                    #region Rate Set to all product
                    case ActionsEnum.RATEADDALL:
                        SetFieldValues(ControlsEnum.RATEADDALL);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                        //GetFieldValues(ControlsEnum.RATEADDALL);
                        //SetFieldValues(ControlsEnum.RATEADDALL);
                        //BindDropdown(ControlsEnum.CURRENCYALL);
                        //BindDropdown(ControlsEnum.CUSTOMERPROPERTIESALL);
                        break;
                    #endregion

                    #region Rate Add
                    case ActionsEnum.RATEADD:
                        GetFieldValues(ControlsEnum.RATEADD);
                        SetFieldValues(ControlsEnum.RATEADD);
                        BindDropdown(ControlsEnum.CURRENCY);
                        BindDropdown(ControlsEnum.CUSTOMERPROPERTIES);
                        ClearForm(ControlsEnum.RATEADD);
                        break;
                    #endregion

                    #region Rate Apply
                    case ActionsEnum.RATEAPPLY:
                        SetFieldValues(ControlsEnum.RATEAPPLY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                        break;
                    #endregion

                    #region Rate Remove
                    case ActionsEnum.REMOVERATE:
                        arg = ((ImageButton)sender).CommandArgument;
                        int rowNumber = Convert.ToInt32(arg);
                        if (TempProductRateSession != null)
                        {
                            productRateList = TempProductRateSession;
                            if (rowNumber > 0)
                            {
                                tempProductRateObj = productRateList == null ? null :
                                    productRateList.SingleOrDefault(p => p.RowNumber == rowNumber);
                            }
                            if (tempProductRateObj != null)
                            {
                                productRateList.Remove(tempProductRateObj);
                                TempProductRateSession = productRateList;
                            }
                        }
                        SetFieldValues(ControlsEnum.RATEADD);
                        break;
                    #endregion

                    #region RateHistory
                    case ActionsEnum.RATEHISTORY:
                        arg = ((ImageButton)sender).CommandArgument;
                        EditPK = Convert.ToInt32(arg);
                        productDetailsList = new List<ProductDetailsBo>();
                        productDetailsList = CustomerRateHeaderSession.ProductDetailsList;
                        tempProductDetailsObj = productDetailsList == null ? null :
                        productDetailsList.SingleOrDefault(p => p.BPR_ITEM == EditPK);
                        if (tempProductDetailsObj != null)
                        {
                            lbnHisProductCode.Text = GetLocalResourceObject("ProductCode").ToString();
                            lbnHisProductDesc.Text = GetLocalResourceObject("ProductDescription").ToString();
                            lblHisProductCode.Text = tempProductDetailsObj.BPR_ITEM_CODE;
                            lblHisProductDesc.Text = string.IsNullOrEmpty(tempProductDetailsObj.BPR_ITEM_NAME.ToString()) ? "&nbsp;" : tempProductDetailsObj.BPR_ITEM_NAME.ToString();
                        }
                        GetFieldValues(ControlsEnum.RATEHISTORY);
                        SetFieldValues(ControlsEnum.RATEHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateHistory]','" + GetLocalResourceObject("RateHistory").ToString() + "','550');", true);
                        // 
                        break;
                    #endregion

                    #region Save
                    case ActionsEnum.SAVE:
                        CustomerRateHeaderObj = new CustomerRateNewBO();
                        CustomerRateHeaderObj = (CustomerRateNewBO)SetUIValuesToObject(ActionsEnum.BRANDDETAILS);
                        if (CustomerRateHeaderObj != null && CustomerRateHeaderObj.ProductDetailsList != null)
                        {
                            string xmlDoc = CommonFunctions.XmlSerialize<CustomerRateNewBO>(CustomerRateHeaderObj);
                            result = BusinessLogic.BrandRates.BrandRatesBL.SaveBrandsRates(xmlDoc);
                            if (result > 0)
                            {
                                ClearForm(ControlsEnum.CLEARALL);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {

                            }
                        }
                        break;
                    #endregion

                    #region SaveSubmit
                    case ActionsEnum.SAVESUBMIT:
                        break;
                    #endregion

                    #region Cancel Region
                    case ActionsEnum.CANCEL:
                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);  
                        break;
                    #endregion

                    #region Clear All Data
                    case ActionsEnum.CLEAR:
                        ClearForm(ControlsEnum.DEFAULT);
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                object returnObj;
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

                    #region currency
                    case ControlsEnum.CURRENCY:
                        //  dtCurrency = CommonBL.GetCurrencyListByBtzuUnit(currentUser.SBUID); //CommonBL.GetCurrencyList(currentUser.SBUID);
                        dtCurrency = CommonBL.BrandRateGetCurrencyListByBtzuUnit(0, 1, currentUser.SBUID); //CommonBL.GetCurrencyList(currentUser.SBUID);
                        Currency = dtCurrency;
                        break;
                    #endregion

                    #region fill product
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
                    #endregion

                    #region apply popup
                    case ControlsEnum.APPLY:
                        customerRateObj = (CustomerRateBO)SetUIValuesToObject(ActionsEnum.APPLY);
                        if (customerRateObj.ItemsList.Count > 0)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(customerRateObj);
                            CustomerRateHeaderObj = new CustomerRateNewBO();
                            CustomerRateHeaderObj = BrandRatesBL.GetProductRate(xmlDoc.InnerXml);
                            CustomerRateHeaderSession = CustomerRateHeaderObj;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_SelectPrdCus").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region rate history
                    case ControlsEnum.RATEHISTORY:
                        dsProductHistory = new DataSet();
                        dsProductHistory = BrandRatesBL.GetBrandProductRateHistory(EditPK);
                        break;
                    #endregion

                    #region rate add individual popup
                    case ControlsEnum.RATEADD:
                        productRateList = new List<ProductRateBO>();
                        ProductRateObj = new ProductRateBO();
                        int rowNumber = 0;
                        ProductRateObj.CategoryName = HttpUtility.HtmlDecode(ddlCustSpecialCategory.SelectedItem.Text);
                        ProductRateObj.CategoryPK = Convert.ToInt32(ddlCustSpecialCategory.SelectedValue);
                        ProductRateObj.BandRate = Convert.ToDouble(txtNewRateApply.Text.Trim());
                        ProductRateObj.CurrencyName = HttpUtility.HtmlDecode(ddlCurrency.SelectedItem.Text);
                        ProductRateObj.CurrencyPK = Convert.ToInt32(ddlCurrency.SelectedValue);
                        productRateList.Add(ProductRateObj);
                        if (TempProductRateSession == null)
                        {
                            TempProductRateSession = new List<ProductRateBO>();
                        }
                        if (TempProductRateSession.Count > 0)
                        {
                            rowNumber = TempProductRateSession.Max(p => p.RowNumber) + 1;
                        }
                        else
                        {
                            rowNumber = 1;
                        }
                        ProductRateObj.RowNumber = rowNumber;
                        TempProductRateSession.Add(ProductRateObj);
                        break;
                    #endregion

                    #region rate popup all product
                    case ControlsEnum.RATEADDALL:
                        productRateList = new List<ProductRateBO>();
                        ProductRateObj = new ProductRateBO();
                        int rowNumber01 = 0;
                        ProductRateObj.CategoryName = HttpUtility.HtmlDecode(ddlCustSpecialCategory01.SelectedItem.Text);
                        ProductRateObj.CategoryPK = Convert.ToInt32(ddlCustSpecialCategory01.SelectedValue);
                        ProductRateObj.BandRate = Convert.ToDouble(txtNewRateApply01.Text.Trim());
                        ProductRateObj.CurrencyName = HttpUtility.HtmlDecode(ddlCurrency01.SelectedItem.Text);
                        ProductRateObj.CurrencyPK = Convert.ToInt32(ddlCurrency01.SelectedValue);
                        productRateList.Add(ProductRateObj);
                        if (TempProductRateSession == null)
                        {
                            TempProductRateSession = new List<ProductRateBO>();
                        }
                        if (TempProductRateSession.Count > 0)
                        {
                            rowNumber01 = TempProductRateSession.Max(p => p.RowNumber) + 1;
                        }
                        else
                        {
                            rowNumber01 = 1;
                        }
                        ProductRateObj.RowNumber = rowNumber01;
                        TempProductRateSession.Add(ProductRateObj);
                        break;
                    #endregion

                    #region bind product tree
                    case ControlsEnum.PRODUCTS:
                        if (trvProducts.Nodes.Count <= 0)
                        {
                            CustomerProductFilterObj = (CustomerProductFilterBO)SetUIValuesToObject(ActionsEnum.PRODUCTS);
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(CustomerProductFilterObj);
                            DataTable dtProductsByProperty = BrandRatesBL.FilterProductListByProperties(xmlDoc.InnerXml);
                            TreeNode childProduct;
                            TreeNode rootProduct;
                            trvProducts.Nodes.Clear();
                            if (dtProductsByProperty.Rows.Count > 0)
                            {
                                //tblEmptyRecord.Visible = false;
                                rootProduct = new TreeNode("All Brand Products", "0");  //GetLocalResourceObject("AllProducts").ToString()
                                rootProduct.NavigateUrl = "javascript:return false;";
                                rootProduct.ShowCheckBox = true;
                                trvProducts.Nodes.Add(rootProduct);
                                if (dtProductsByProperty != null)
                                {
                                    foreach (DataRow row in dtProductsByProperty.Rows)
                                    {
                                        //childProduct = new TreeNode(row["ITM_CODE"].ToString() + "  -  " + row["ITM_NAME"].ToString(), row["ITM_PK"].ToString());
                                        childProduct = new TreeNode(row["ITM_NAME"].ToString(), row["ITM_PK"].ToString());
                                        childProduct.ShowCheckBox = true;
                                        childProduct.NavigateUrl = "javascript:return false;";
                                        childProduct.ToolTip = row["ITM_NAME"].ToString();
                                        rootProduct.ChildNodes.Add(childProduct);
                                    }
                                    rootProduct.ExpandAll();
                                }
                            }
                        }
                        break;
                    #endregion

                    #region CUSTOMERPROPERTIES
                    case ControlsEnum.CUSTOMERPROPERTIES:
                        dtSpecialCategory = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, (int)ConstGroup.SpecialCategory, ConstGroupType.CustomerProperties, 0, 1, currentUser.SBUID);
                        Category = dtSpecialCategory;
                        break;
                    #endregion

                    #region PACKINGSPECS
                    case ControlsEnum.PACKINGSPECS:
                        string sortby = string.Empty;
                        sortby = GetGlobalResourceObject("ConfigurationsRes", "PackingSpecSortBy").ToString();
                        dsPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(0, Convert.ToInt16(DbActiveStatus.ACTIVE), CurrentUserPK,
                           string.Empty, string.Empty, string.Empty, sortby);
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

                    //#region SUBTYPE
                    //case ControlsEnum.SUBTYPE:
                    //    dtSubType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(CurrentUserPK, "PRODUCT SUB TYPE");
                    //    if (dtSubType.Rows.Count > 0)
                    //    {
                    //        chkUsers = new List<DDLMaster>();
                    //        chkUsers = dtSubType.AsEnumerable().Select(row => new DDLMaster
                    //            {
                    //                PK = row.Field<byte?>(Resources.DataFieldRes.cfgValue),
                    //                Value = row.Field<string>(Resources.DataFieldRes.cfgData),
                    //            }).ToList();
                    //    }
                    //    break;
                    //#endregion
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

        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        dtBrandDetails = dsPageData.Tables[0];
                        dsPageData.Tables.Remove(dsPageData.Tables[0]);
                        dsProductRate = dsPageData;
                        ProductRates = dsProductRate;
                        //For filling product and cutomer ddl
                        if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                        {
                            hdfbrandRatePK.Value = dtBrandDetails.Rows[0]["BRH_PK"].ToString();
                            hdfBrandRateSbu.Value = dtBrandDetails.Rows[0]["BRH_BIZUNIT"].ToString();
                        }
                        break;

                    case ControlsEnum.PRODUCTDTL:
                        BindGrid(type);
                        break;

                    case ControlsEnum.APPLY:
                        BindGrid(ControlsEnum.APPLY);
                        break;

                    case ControlsEnum.RATEADD:
                        BindGrid(ControlsEnum.RATEADD);
                        break;

                    //case ControlsEnum.RATEADDALL:
                    //    BindGrid(ControlsEnum.RATEADDALL);
                    //    break;

                    case ControlsEnum.RATEAPPLY:
                        if (TempProductRateSession != null)
                        {
                            productDetailsList = new List<ProductDetailsBo>();
                            productDetailsList = CustomerRateHeaderSession.ProductDetailsList;
                            tempProductDetailsObj = productDetailsList == null ? null :
                            productDetailsList.SingleOrDefault(p => p.BPR_ITEM == EditPK);
                            tempProductDetailsObj.ProductRateList = TempProductRateSession;
                            CustomerRateHeaderSession.ProductDetailsList = productDetailsList;
                            TempProductRateSession = null;
                            tempProductDetailsObj = null;
                        }
                        break;

                    case ControlsEnum.RATEADDALL:
                        if (TempProductRateSession != null)
                        {
                            productDetailsList = new List<ProductDetailsBo>();
                            productDetailsList = CustomerRateHeaderSession.ProductDetailsList;
                            foreach (var item in productDetailsList)
                            {
                                tempProductDetailsObj = item;
                                tempProductDetailsObj.ProductRateList = TempProductRateSession;
                            }
                            CustomerRateHeaderSession.ProductDetailsList = productDetailsList;
                            TempProductRateSession = null;
                            tempProductDetailsObj = null;
                        }

                        break;

                    case ControlsEnum.RATEHISTORY:
                        BindGrid(ControlsEnum.RATEHISTORY);
                        break;

                    case ControlsEnum.SET:
                        GetUIValuesFromObject(type);
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

                    //case ControlsEnum.SUBTYPE:
                    //    if (chkUsers != null && chkUsers.Count > 0)
                    //    {
                    //        chklstSubCategory.ListData = CommonFunctions.HtmlDecode(chkUsers, "Value");
                    //        chklstSubCategory.BindData();
                    //    }
                    //    else
                    //    {
                    //        chklstSubCategory.ClearData();
                    //    }
                    //    break;
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
            try
            {
                switch (mode)
                {

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

                    #region Brand Rate Details
                    case ActionsEnum.BRANDDETAILS:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        CustomerRateHeaderObj = CustomerRateHeaderSession;
                        //CustomerRateHeaderObj.UserPK = currentUser.SBUID.ToString();CurrentUserPK
                        CustomerRateHeaderObj.UserPK = CurrentUserPK.ToString();
                        CustomerRateHeaderObj.LastModDate = Convert.ToDateTime(hdfDate.Value);
                        returnObj = CustomerRateHeaderObj;
                        break;
                    #endregion

                    #region Apply
                    case ActionsEnum.APPLY:
                    case ActionsEnum.CHANGETYPE:
                        customerRateObj = new CustomerRateBO();
                        customerRateObj.BrhPK = CurrPK;
                        customerRateObj.ListType = (int)SearchTypeEnum.Product;
                        customerRateObj.BizUnit = Convert.ToInt32(CurrentUserPK);//currentUser.SBUID
                        customerRateObj.PageSize = grdSelectedCusBrands.PageSize;
                        customerRateObj.PageIndex = Convert.ToInt32(PageIndex);
                        customerRateObj.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        // customerRateObj.CustomerList = SelectedCustomers == null ? new List<CustomerListBO>() : SelectedCustomers.CustomerList;
                        customerRateObj.ItemsList = SelectedProducts == null ? new List<ItemsListBO>() : SelectedProducts.ItemsList;
                        //new section start
                        List<ItemsListBO> selProducts = new List<ItemsListBO>();
                        if (trvProducts.Nodes.Count > 0)
                        {
                            if (trvProducts.CheckedNodes.Count >0)                          
                            {
                                foreach (TreeNode node in trvProducts.Nodes)
                                {
                                    //foreach (TreeNode child1 in node.ChildNodes)
                                    foreach (TreeNode child1 in node.ChildNodes)
                                    {
                                        if (child1.Checked)
                                        {
                                            selProducts.Add(new ItemsListBO { itemPK = Convert.ToInt32(child1.Value) });
                                        }
                                    }
                                }

                                //foreach (TreeNode checkedchild in trvProducts.CheckedNodes)
                                //{
                                //    selProducts.Add(new ItemsListBO { itemPK = Convert.ToInt32(checkedchild.Value) });
                                //}
                            }
                            else
                            {
                                //foreach (TreeNode uncheckedchild in trvProducts.Nodes)
                                //{
                                foreach (TreeNode node in trvProducts.Nodes)
                                {
                                    foreach (TreeNode child1 in node.ChildNodes)
                                    {
                                        selProducts.Add(new ItemsListBO { itemPK = Convert.ToInt32(child1.Value) });
                                        child1.Checked = true;
                                        node.Checked = true;
                                    }
                                }

                                // foreach (TreeNode child1 in node.ChildNodes)
                                // selProducts.Add(new ItemsListBO { itemPK = Convert.ToInt32(uncheckedchild.Value) });
                                // }
                            }

                            //foreach (TreeNode node in trvProducts.CheckedNodes)
                            ////foreach (TreeNode child1 in node.ChildNodes)
                            //{
                            //    selProducts.Add(new ItemsListBO { itemPK = Convert.ToInt32(node.Value) });
                            //}
                        }
                        customerRateObj.ItemsList = selProducts == null ? new List<ItemsListBO>() : selProducts;
                        SelectedProductItem = selProducts;
                        customerRateObj.ItemsList = SelectedPackingSpecs == null ? new List<ItemsListBO>() : SelectedProductItem;
                        //new section end
                        ////Packing Spec
                        //List<PackingSpecListBO> selPackingSpecs = new List<PackingSpecListBO>();
                        //foreach (ListItem item in chklstPackingSpec.GetCheckedItems())
                        //{
                        //    selPackingSpecs.Add(new PackingSpecListBO { APS_PK = Convert.ToInt32(item.Value) });
                        //    // selPackingSpecs.Add(new PackingSpecListBO { APS_PK = Convert.ToInt32(hdfPackingSpec.Value) });
                        //}

                        //customerRateObj.PackingSpecList = selPackingSpecs == null ? new List<PackingSpecListBO>() : selPackingSpecs;

                        //SelectedPackingSpecs = selPackingSpecs;
                        //customerRateObj.PackingSpecList = SelectedPackingSpecs == null ? new List<PackingSpecListBO>() : SelectedPackingSpecs;
                        // End Packig Specs
                        returnObj = customerRateObj;
                        break;

                    case ActionsEnum.PRODUCTS:
                    case ActionsEnum.PRODUCTLISTPOPUPOK:
                        CustomerProductFilterObj = new CustomerProductFilterBO();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        //CustomerProductFilterObj.UserPK = currentUser.SBUID;CurrentUserPK
                        CustomerProductFilterObj.UserPK = CurrentUserPK;
                        // CustomerProductFilterObj.FinishedGoods = Convert.ToInt32(ERP.Utilities.Constants.DA.ProductCategory.FinishedGoods);
                        //CustomerProductFilterObj.FinishedGoods = 122;
                        CustomerProductFilterObj.SelectValue = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        CustomerProductFilterObj.Type = Convert.ToInt32(ddlTypeProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.Size = Convert.ToInt32(ddlSizeProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.ColorCategory = Convert.ToInt32(ddlColorCategoryProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.FormerType = Convert.ToInt32(ddlFormerTypeProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.FormerSize = Convert.ToInt32(ddlFormerSizeProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.Color = Convert.ToInt32(ddlColorProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.PrintType = Convert.ToInt32(ddlPrintProductListPopUp.SelectedValue);
                        CustomerProductFilterObj.SubCategoryType = Convert.ToInt32(ddlSubCategoryList.SelectedValue);


                        //SubCategory
                        //List<SubCategoryListBO> selSubCategoryList = new List<SubCategoryListBO>();
                        //foreach (ListItem item in chklstSubCategory.GetCheckedItems())
                        //{
                        //    selSubCategoryList.Add(new SubCategoryListBO { CFG_VALUE = Convert.ToByte(item.Value) });
                        //}
                        //CustomerProductFilterObj.SubCategoryList = selSubCategoryList == null ? new List<SubCategoryListBO>() : selSubCategoryList;
                        //SelectedSubCategory = selSubCategoryList;
                        //CustomerProductFilterObj.SubCategoryList = SelectedSubCategory == null ? new List<SubCategoryListBO>() : SelectedSubCategory;
                        //End SubCategory


                        //Packing Spec
                        List<PackingSpecListBO> selPackingSpecss = new List<PackingSpecListBO>();
                        foreach (ListItem item in chklstPackingSpec.GetCheckedItems())
                        {
                            selPackingSpecss.Add(new PackingSpecListBO { APS_PK = Convert.ToInt32(item.Value) });
                            // selPackingSpecs.Add(new PackingSpecListBO { APS_PK = Convert.ToInt32(hdfPackingSpec.Value) });
                        }

                        CustomerProductFilterObj.PackingSpecList = selPackingSpecss == null ? new List<PackingSpecListBO>() : selPackingSpecss;

                        SelectedPackingSpecs = selPackingSpecss;
                        CustomerProductFilterObj.PackingSpecList = SelectedPackingSpecs == null ? new List<PackingSpecListBO>() : SelectedPackingSpecs;
                        //End Packing Spec

                        //Products
                        //List<ProductsListBO> selProducts = new List<ProductsListBO>();
                        //foreach (TreeNode node in trvProducts.CheckedNodes)
                        //{
                        //    selProducts.Add(new ProductsListBO { PRD_PK = Convert.ToInt32(node.Value) });
                        //}
                        //CustomerProductFilterObj.ProductsList = SelectedProduct == null ? new List<ProductsListBO>() : selProducts;
                        //SelectedProduct = selProducts;
                        //CustomerProductFilterObj.ProductsList = SelectedProduct == null ? new List<ProductsListBO>() : SelectedProduct;
                        //End Products

                        returnObj = CustomerProductFilterObj;
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
                        //chkExpandAll.Checked = false;
                        if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                        {
                            LastModifiedTime = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_MOD_DT"].ToString());
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            CurrPK = Convert.ToInt32(dtBrandDetails.Rows[0]["BRH_PK"].ToString());
                            // lblStatus.Text = dtBrandDetails.Rows[0]["BRH_STATUS_text"].ToString();
                            SetStatus((WkfStatus)Convert.ToInt32(dtBrandDetails.Rows[0]["BRH_STATUS"]));
                            //if (txtFromDate.Text.Trim() == string.Empty)
                            //{
                            //txtFromDate.Text = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            //hdfFromDate.Value = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            // txtToDate.Text = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            // hdfToDate.Value = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            //FromDate = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_FROM"].ToString());
                            //ToDate = Convert.ToDateTime(dtBrandDetails.Rows[0]["BRH_DATE_TO"].ToString());
                            //}

                            hdfBrandRateSbu.Value = dtBrandDetails.Rows[0]["BRH_BIZUNIT"].ToString();

                        }
                        else
                        {
                            SetStatus(WkfStatus.NEW);
                            tblFilter.Visible = false;
                            ClearForm(ControlsEnum.DEFAULT);
                            CurrPK = 0;
                        }
                        // BindWorkflow();

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
                // default:
                // lblStatus.Text = GetLocalResourceObject("New").ToString();
                // SetRdbVisibility(false);
                // btnDelete.Visible = false;
                // break;
            }
        }

        /// <summary>
        ///  Method for Bind Grid
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {
                #region Default
                case ControlsEnum.DEFAULT:
                    grdSelectedCusBrands.DataSource = null;
                    grdSelectedCusBrands.DataBind();

                    grdRates.DataSource = null;
                    grdRates.DataBind();
                    clearPaging();

                    grdProductRateALL.DataSource = null;
                    grdProductRateALL.DataBind();
                    break;
                #endregion

                //case ControlsEnum.RATEADDALL:
                //    grdRates.DataSource = null;
                //    grdRates.DataBind();
                //    break;
                #region add rate to popu grid
                case ControlsEnum.SETRATE:
                    if (productDetailsList != null)
                    {
                        productRateList = tempProductDetailsObj.ProductRateList;
                        TempProductRateSession = productRateList;
                        if (productRateList != null)
                        {
                            grdRates.DataSource = productRateList;
                        }
                        else
                        {
                            grdRates.DataSource = null;
                        }
                        grdRates.DataBind();
                    }
                    break;
                #endregion

                #region rate add all product
                case ControlsEnum.RATEADD:
                    grdRates.DataSource = TempProductRateSession;
                    grdRates.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSett]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                    break;
                #endregion

                //case ControlsEnum.RATEADDALL:
                //    grdProductRateALL.DataSource = TempProductRateSession;
                //    grdProductRateALL.DataBind();
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDivWkf('[id$=divRateSettAllProduct]','" + GetLocalResourceObject("SetRate").ToString() + "','550');", true);
                //    break;

                #region Rate history 
                case ControlsEnum.RATEHISTORY:

                    if (dsProductHistory.Tables.Count > 0)
                    {
                        grdProductRateHistory.DataSource = dsProductHistory;
                    }
                    else
                    {
                        grdProductRateHistory.DataSource = null;
                    }
                    grdProductRateHistory.DataBind();
                    break;
                #endregion

                case ControlsEnum.APPLY:
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    //grdSelectedCusBrands.Columns[0].HeaderText = GetLocalResourceObject("ProductCode").ToString();
                    //grdSelectedCusBrands.Columns[1].HeaderText = GetLocalResourceObject("ProductDescription").ToString();

                    if (CustomerRateHeaderSession != null)
                    {
                        productDetailsList = new List<ProductDetailsBo>();
                        //productDetailsList.AddRange(productRateList.FirstOrDefault(p=>p.BandRat))
                        //productDetailsList.AddRange(productDetailsList.SingleOrDefault(p=>p.ProductRateList)));
                        productDetailsList = CustomerRateHeaderSession.ProductDetailsList;
                        var ROW = productDetailsList.FirstOrDefault();
                        int rowCount = ROW.ROW_COUNT;
                        if (productDetailsList != null)
                        {
                            tblFilter.Visible = true;
                            decimal pages = Convert.ToDecimal(Convert.ToDecimal(rowCount) / Convert.ToDecimal(grdSelectedCusBrands.PageSize.ToString()));
                            TotalPages = Convert.ToInt32(Math.Ceiling(pages));
                            uclPaging.TotalPages = TotalPages;
                            grdSelectedCusBrands.DataSource = productDetailsList;
                            // tempProductDetailsObj = null; tempProductDetailsObj.ROW_COUNT
                        }
                    }
                    else
                    {
                        tblFilter.Visible = false;
                        grdSelectedCusBrands.DataSource = null;
                    }
                    grdSelectedCusBrands.DataBind();

                    //grdSelectedCusBrands.DataBind();
                    //break;
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ExpandSelected", "ExpandSelected();", true);
            }
        }

        /// <summary>
        ///  Method for Bind DropDown
        /// </summary>
        private void BindDropdown(ControlsEnum type)
        {
            switch (type)
            {
                #region default
                case ControlsEnum.DEFAULT:
                    object val = Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode());

                    int ii = Convert.ToInt32(val);
                    int jj = Convert.ToInt32(Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode()));

                    int groupType = Convert.ToInt32(Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode()));
                    DataTable dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Type, ProductProperties.Type.GetTypeCode())));
                    ddlTypeProductListPopUp.DataValueField = "CON_PK";
                    ddlTypeProductListPopUp.DataTextField = "CON_NAME";
                    ddlTypeProductListPopUp.DataSource = dtProductProperties;
                    ddlTypeProductListPopUp.DataBind();
                    ddlTypeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlTypeProductListPopUp.SelectedIndex = 0;

                    dtSubType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(CurrentUserPK, "PRODUCT SUB TYPE");
                    ddlSubCategoryList.DataValueField = "CFG_VALUE";
                    ddlSubCategoryList.DataTextField = "CFG_DATA";
                    ddlSubCategoryList.DataSource = dtSubType;
                    ddlSubCategoryList.DataBind();
                    ddlSubCategoryList.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlSubCategoryList.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec05, ProductProperties.Side.GetTypeCode())));
                    ddlColorCategoryProductListPopUp.DataValueField = "CON_PK";
                    ddlColorCategoryProductListPopUp.DataTextField = "CON_NAME";// GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlColorCategoryProductListPopUp.DataSource = dtProductProperties;
                    ddlColorCategoryProductListPopUp.DataBind();
                    ddlColorCategoryProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlColorCategoryProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Size, ProductProperties.Size.GetTypeCode())));
                    ddlSizeProductListPopUp.DataValueField = "CON_PK";
                    ddlSizeProductListPopUp.DataTextField = "CON_NAME";
                    ddlSizeProductListPopUp.DataSource = dtProductProperties;
                    ddlSizeProductListPopUp.DataBind();
                    ddlSizeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlSizeProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec06, ProductProperties.Side.GetTypeCode())));
                    ddlFormerTypeProductListPopUp.DataValueField = "CON_PK";
                    ddlFormerTypeProductListPopUp.DataTextField = "CON_NAME";// GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlFormerTypeProductListPopUp.DataSource = dtProductProperties;
                    ddlFormerTypeProductListPopUp.DataBind();
                    ddlFormerTypeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlFormerTypeProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.AdnlSpec07, ProductProperties.Side.GetTypeCode())));
                    ddlFormerSizeProductListPopUp.DataValueField = "CON_PK";
                    ddlFormerSizeProductListPopUp.DataTextField = "CON_NAME";// GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlFormerSizeProductListPopUp.DataSource = dtProductProperties;
                    ddlFormerSizeProductListPopUp.DataBind();
                    ddlFormerSizeProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlFormerSizeProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Color, ProductProperties.Side.GetTypeCode())));
                    ddlColorProductListPopUp.DataValueField = "CON_PK";
                    ddlColorProductListPopUp.DataTextField = "CON_NAME";// GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlColorProductListPopUp.DataSource = dtProductProperties;
                    ddlColorProductListPopUp.DataBind();
                    ddlColorProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlColorProductListPopUp.SelectedIndex = 0;

                    dtProductProperties = new DataTable();
                    dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(CurrentUserPK, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.PrintType, ProductProperties.Side.GetTypeCode())));
                    ddlPrintProductListPopUp.DataValueField = "CON_PK";
                    ddlPrintProductListPopUp.DataTextField = "CON_NAME";// GetLocalResourceObject("CON_NAME").ToString();//"CON_NAME";
                    ddlPrintProductListPopUp.DataSource = dtProductProperties;
                    ddlPrintProductListPopUp.DataBind();
                    ddlPrintProductListPopUp.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
                    ddlPrintProductListPopUp.SelectedIndex = 0;
                    break;
                #endregion

                #region currency
                case ControlsEnum.CURRENCY:
                    ddlCurrency.DataSource = Currency;
                    ddlCurrency.DataTextField = "CUR_CODE";
                    ddlCurrency.DataValueField = "CUR_PK";
                    //ddlCurrency.ToolTip = "CUR_NAME";
                    ddlCurrency.DataBind();
                    break;
                #endregion

                #region customer properties
                case ControlsEnum.CUSTOMERPROPERTIES:
                    ddlCustSpecialCategory.Items.Clear();
                    if (Category != null)
                    {
                        ddlCustSpecialCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(Category, "CON_NAME");
                        ddlCustSpecialCategory.DataTextField = "CON_NAME";
                        ddlCustSpecialCategory.DataValueField = "CON_PK";
                        ddlCustSpecialCategory.DataBind();
                    }
                    // ddlCustSpecialCategory.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion

                //#region CurrencyAll
                //case ControlsEnum.CURRENCYALL:
                //    ddlCurrency01.DataSource = Currency;
                //    ddlCurrency01.DataTextField = "CUR_CODE";
                //    ddlCurrency01.DataValueField = "CUR_PK";
                //    ddlCurrency01.DataBind();
                //    break;
                //#endregion

                //case ControlsEnum.CUSTOMERPROPERTIESALL:
                //    ddlCustSpecialCategory01.Items.Clear();
                //    if (Category != null)
                //    {
                //        ddlCustSpecialCategory01.DataSource = CommonFunctions.HtmlDecodeDataTable(Category, "CON_NAME");
                //        ddlCustSpecialCategory01.DataTextField = "CON_NAME";
                //        ddlCustSpecialCategory01.DataValueField = "CON_PK";
                //        ddlCustSpecialCategory01.DataBind();
                //    }
                //    break;
            }
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
                #region Clear All
                case ControlsEnum.CLEARALL:
                    // txtPackingSpec.Text = Resources.ErpRes.AutoDefaultValue;
                    CustomerRateHeaderSession = null;
                    TempProductRateSession = null;
                    tempProductDetailsObj = null;
                    productDetailsList = null;
                    productDetailsObj = null;
                    productRateList = null;
                    ProductRateObj = null;
                    //chkShowHideProducts.Checked = false;
                    BindDropdown(ControlsEnum.DEFAULT);
                    BindGrid(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.PACKINGSPECS);
                    SetFieldValues(ControlsEnum.PACKINGSPECS);
                    GetFieldValues(ControlsEnum.SUBTYPE);
                    SetFieldValues(ControlsEnum.SUBTYPE);
                    trvProducts.Nodes.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "HideFilter();", true);
                    disableDetailsFlag = 0;
                    txtDate.Text = string.Empty;
                    break;
                #endregion

                #region RateAdd
                case ControlsEnum.RATEADD:
                    txtNewRateApply.Text = string.Empty;
                    break;
                #endregion

                #region products
                case ControlsEnum.PRODUCTS:
                    trvProducts.Nodes.Clear();
                    break;
                #endregion

                #region default
                case ControlsEnum.DEFAULT:
                    //trvProducts.no
                    CustomerRateHeaderSession = null;
                    TempProductRateSession = null;
                    tempProductDetailsObj = null;
                    productDetailsList = null;
                    productDetailsObj = null;
                    productRateList = null;
                    ProductRateObj = null;
                    //chkShowHideProducts.Checked = false;
                    BindDropdown(ControlsEnum.DEFAULT);
                    BindGrid(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.PACKINGSPECS);
                    SetFieldValues(ControlsEnum.PACKINGSPECS);
                    GetFieldValues(ControlsEnum.SUBTYPE);
                    SetFieldValues(ControlsEnum.SUBTYPE);
                    trvProducts.Nodes.Clear();
                    txtDate.Text = string.Empty;
                    break;
                #endregion
            }
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
            dsProductRate = ProductRates;
            //For update current grid values to table
            //foreach (GridView gvr in grdSelectedCusBrands.Rows)
            //{
            //  grd = (GridView)gvr.FindControl("grdSelectedCusBrands") as GridView;
            foreach (GridViewRow inRow in grdSelectedCusBrands.Rows)
            {
                // BrandPK = Convert.ToInt32((inRow.FindControl("hdfItemPK") as HiddenField).Value);
                EditPK = Convert.ToInt32((inRow.FindControl("hdfItemPK") as HiddenField).Value);
                /////////////////////
                // CurrencyPK = Convert.ToInt32((inRow.FindControl("ddlCurrency") as DropDownList).SelectedValue);
                //CurrencyPK = Convert.ToInt32((inRow.FindControl("hdfCurrPK") as HiddenField).Value);
                //drr = dsCustomerRate.Tables[1].Select("CIM_PK=" + BrandPK);
                //if ((inRow.FindControl("txtNewRate") as TextBox).Text != "")
                //{
                //    Rate = decimal.Parse((inRow.FindControl("txtNewRate") as TextBox).Text);
                //    drr[0]["BRD_RATE"] = Rate;
                //}
                //drr[0]["CUR_PK"] = CurrencyPK;
                //drr[0].AcceptChanges();
            }
            ProductRates = dsProductRate;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            //path = Resources.PageURL.ActivityWkfURL;
            //path = "/OrderToCash/BrandRates.aspx";
            //WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CurrentUserPK = currentUser.SBUID;
            //DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            //if (dtProcess != null && dtProcess.Rows.Count > 0)
            //{
            //    //ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
            //    //hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
            //}
        }

        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
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
                        CurrentAction = ActionsEnum.APPLY;
                        GetFieldValues(ControlsEnum.APPLY);
                        SetFieldValues(ControlsEnum.APPLY);


                        //commonActions = ActionsEnum.APPLY;
                        //GetFieldValues(ControlsEnum.APPLY);
                        //SetFieldValues(ControlsEnum.APPLY);
                        break;
                    default:
                        //commonActions = ActionsEnum.DEFAULT;
                        //GetFieldValues(ControlsEnum.DEFAULT);
                        //SetFieldValues(ControlsEnum.DEFAULT);
                        //SetFieldValues(ControlsEnum.SET);
                        //SetFieldValues(ControlsEnum.APPLY);
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

        #region Enum
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
            PACKINGSPECS,
            SUBTYPE,
            RATEADD,
            RATEADDALL,
            REMOVERATE,
            SETRATE,
            BRANDSRATEHEADER,
            CURRENCYALL,
            CUSTOMERPROPERTIESALL

        }

        /// <summary>
        /// Define workflow Enum
        /// </summary>
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

        /// <summary>
        /// Search Type
        /// </summary>
        enum SearchTypeEnum
        {
            Product = 1,
            Customer
        }
        #endregion
    }
}