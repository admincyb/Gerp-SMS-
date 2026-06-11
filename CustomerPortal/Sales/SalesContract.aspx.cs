using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Sales;
using ERP.Utilities;
using ERPData;
using ERPService;
using ERPManager;
using System.IO;
using System.Configuration;
using BusinessLogic.CommonManagement;
using ERPSMS_v01.Reports;
using ERPSMS_v01;


namespace CustomerPortal.Sales
{
    public partial class SalesContract : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Is Quotation Contract
        /// </summary>
        private bool IsQuotationContract
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsQuotationContract] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsQuotationContract]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsQuotationContract] = value;
            }
        }
        /// <summary>
        /// Is Customer User
        /// </summary>
        private bool IsCustomerUser
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomerUser] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomerUser]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomerUser] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool IsTaxInSBU
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxInSBU] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxInSBU].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxInSBU] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsTaxForOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherCharge] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherCharge].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherCharge] = value;
            }
        }
        private bool IsDeliveryTermsCheck
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDeliveryTermsCheck] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDeliveryTermsCheck].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDeliveryTermsCheck] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool ShipdateUpdation
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShipdateUpdation] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShipdateUpdation].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.ShipdateUpdation] = value;
            }
        }
        /// <summary>
        /// Is carton
        /// </summary>
        private bool Iscarton
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscarton] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscarton]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscarton] = value;
            }
        }

        /// <summary>
        /// Is Same brand
        /// </summary>
        private bool IsBrand
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsBrand] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsBrand]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsBrand] = value;
            }
        }


        /// <summary>
        /// Is Same Materail
        /// </summary>
        private bool IsMaterail
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsMaterail] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsMaterail]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsMaterail] = value;
            }
        }



        /// <summary>
        /// To Disable Item Tax
        /// </summary>
        private int EnableItemTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemTax] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.DisableItemTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemTax] = value;
            }
        }

        /// <summary>
        /// To set custom tax config value
        /// </summary>
        private bool IsCustomTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomTaxEnabled] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomTaxEnabled] = value;
            }
        }
        /// <summary>
        /// To Disable Item Discount
        /// </summary>
        private int EnableItemDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemDiscount] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.DisableItemDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemDiscount] = value;
            }
        }

        private int ShowLotNo
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowLotNo"]);
            }
            set
            {
                this.ViewState["ShowLotNo"] = value;
            }
        }
        private int CreditLimitRuleBase
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CreditLimitRuleBase"]);
            }
            set
            {
                this.ViewState["CreditLimitRuleBase"] = value;
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

        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
            }
        }

        private int ShowLotSize
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowLotSize"]);
            }
            set
            {
                this.ViewState["ShowLotSize"] = value;
            }
        }
        private int ShowSCAdditionalPackDtls
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowSCAdditionalPackDtls"]);
            }
            set
            {
                this.ViewState["ShowSCAdditionalPackDtls"] = value;
            }
        }

        /// <summary>
        ///  Text of the Selected Tax
        /// </summary>
        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
            }
        }

        /// <summary>
        ///  Customer Countr Code
        /// </summary>
        private string CusCurrencyCode
        {
            get
            {
                return (string)this.ViewState["CusCurrencyCode"];
            }
            set
            {
                this.ViewState["CusCurrencyCode"] = value;
            }
        }
        /// <summary>
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
            }
        }
        /// <summary>
        /// isWarnedCBM
        /// </summary>
        private int isWarnedCBM
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.isWarnedCBM]);
            }
            set
            {
                this.ViewState[ViewstateStrings.isWarnedCBM] = value;
            }
        }
        /// <summary>
        /// Serail No. of the Contract Item
        /// </summary>
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }

        private int CurrDocSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] = value;
            }
        }

        /// <summary>
        /// Show or Hide To Port ddl 
        /// </summary>
        private bool IsToPortDdlShow
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsToPortDdlShow] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsToPortDdlShow].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsToPortDdlShow] = value;
            }
        }

        private int IsEnableTypeFilter
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.IsEnableTypeFilter] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.IsEnableTypeFilter];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.IsEnableTypeFilter] = value;
            }
        }
        /// <summary>
        /// To identify whether Header/Detail Tax
        /// </summary>
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }
        /// <summary>
        /// Whether the Tqax is in edit mode
        /// </summary>
        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrQuotationPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrQuotationPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrQuotationPK] = value;
            }
        }

        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CrmQuotationPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CrmQuotationPK"]);
            }
            set
            {
                this.ViewState["CrmQuotationPK"] = value;
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
        /// To maintain keep Carton Decimal
        /// </summary>
        private int CartonDecimal
        {
            get
            {
                return this.ViewState[ViewstateStrings.CartonDecimal] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CartonDecimal].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.CartonDecimal] = value;
            }
        }

        /// <summary>
        /// To maintain Bank PK in viewstate
        /// </summary>
        private int BankPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.BankPk] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.BankPk].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.BankPk] = value;
            }
        }
        private int ShowCaseMarkCheckAllItem
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowCaseMarkCheckAllItem"]);
            }
            set
            {
                this.ViewState["ShowCaseMarkCheckAllItem"] = value;
            }
        }
        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        //private SaleContractBO SaleOrderHeaderSession
        //{
        //    get
        //    {
        //        return (SaleContractBO)Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession];
        //    }
        //    set
        //    {
        //        if (Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession] != null)
        //            Session.Remove(ERP.Utilities.SessionStrings.SaleOrderHeaderSession);
        //        Session.Add(ERP.Utilities.SessionStrings.SaleOrderHeaderSession, value);
        //    }
        //}
        private SaleContractBO SaleOrderHeaderSession
        {
            get
            {
                return (SaleContractBO)ViewState[ERP.Utilities.ViewstateStrings.SaleOrderHeaderSession];
                //return (SaleContractBO)Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.ViewstateStrings.SaleOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.ViewstateStrings.SaleOrderHeaderSession);
                ViewState.Add(ERP.Utilities.ViewstateStrings.SaleOrderHeaderSession, value);
            }
        }


        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private SaleContractBO TempSaleOrderHeaderSession
        {
            get
            {
                return (SaleContractBO)ViewState[ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession);
                ViewState.Add(ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession, value);
            }
        }
        /// <summary>
        /// To maintain selected SC Detail PK
        /// </summary>
        private int SelectedDtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedDtlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedDtlPK] = value;
            }
        }
        /// <summary>
        /// To maintain the selected Brand PK
        /// for Detail Tax
        /// </summary>
        private int SelectedCusItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedCusItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCusItemPK] = value;
            }
        }
        /// <summary>
        /// To maintain the selected Product PK
        /// for Detail Tax
        /// </summary>
        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
            }
        }

        private List<SaleOrderTaxHdr> tempsaleOrderTaxHdrList
        {
            get
            {
                return (List<SaleOrderTaxHdr>)Session[ERP.Utilities.ViewstateStrings.tempsaleOrderTaxHdrList];
            }
            set
            {
                Session[ERP.Utilities.ViewstateStrings.tempsaleOrderTaxHdrList] = value;
            }

        }

        private List<BusinessObject.Sales.SaleOrderUploads> SOUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.SOUploadList] == null ? null : (List<BusinessObject.Sales.SaleOrderUploads>)ViewState[ViewstateStrings.SOUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.SOUploadList] = value;
            }
        }

        private List<BusinessObject.Sales.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileSODetailsList] == null ? null : (List<BusinessObject.Sales.FileDetails>)Session[ERP.Utilities.SessionStrings.FileSODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileSODetailsList] = value;
            }
        }

        private int LotNoFirstIndex
        {
            get
            {
                return Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCLotNoGenerateFirstIndex"));
            }
        }

        private int LotNoLastIndex
        {
            get
            {
                return Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCLotNoGenerateLastIndex"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool SendMailWithAttachment
        {
            get
            {
                return this.ViewState[ViewstateStrings.SendMailWithAttachment] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.SendMailWithAttachment].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.SendMailWithAttachment] = value;
            }
        }


        private ItemRefBO SaleOrderItemRefHdr
        {
            get
            {
                return ViewState[ViewstateStrings.SaleOrderItemRef] == null ? null : (ItemRefBO)ViewState[ViewstateStrings.SaleOrderItemRef];
            }
            set
            {
                ViewState[ViewstateStrings.SaleOrderItemRef] = value;
            }
        }
        private bool IsAmend
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAmend] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsAmend].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAmend] = value;
            }
        }


        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //To fetch DataSet/DataTable from DB
        private DataSet dsPageData;
        DataSet dsSaleOrderTaxDetails;
        DataTable dtSaleOrderTaxDetails;
        DataTable dtPageData;
        DataTable dtToPort;
        DataTable dtStrapping;
        DataTable dtSalesCost;
        DataTable dtCompany = new DataTable();
        private DataTable dtCustomTaxSet;

        //page related Entity/BO Object
        private SaleContractBO saleOrderHeaderObj;

        private SaleContractDetailsBO saleOrderDetailsObj;
        List<SaleContractDetailsBO> saleOrderDetailsList;
        SaleContractDetailsBO currLine;//For Line Movement
        SaleContractDetailsBO nextLine;//For Line Movement

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        List<SaleOrderTaxHdr> contractTaxHdrList;
        SaleContractDetailsBO contractDetails;

        private ProductDtlBO ProductDtlBOObj;
        private ProductDtls ProductDtlDtlObj;
        List<ProductDtls> ProductDtlsList;
        //List<SaleOrderTaxHdr> tempsaleOrderTaxHdrList;



        //Service, Service Parameter Objects
        private ServiceUtility serviceUtilityObj;
        private CommonService commonServiceObj;
        SaleOrderUploads soUploadObj;

        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;

        private ItemRefBO saleOrderItemRefObj;
        SalesCostBO objSCcost;
        //Global Private Variables used to maintaion data across methods in the same postback
        private int copyContractPK;
        private int notifyPartyPK;
        private int consigneePK;
        private int agentPK;
        private int fromPortPK;
        private int originOfGoodsPK;
        private int bankDetailPK;
        private double exchangeRate;
        private int soTypePK;
        private int soSubTypePK;
        private int custPK;
        private int addressPK;


        private int addressActive;
        private int transhipmentPK;
        private int shipByPK;
        private int deliveryTermPK;
        private int shipmentTermPK;
        private int paymentTermPK;
        private int specialCausePK;
        private int inspectionPK;
        private int exportDocPK;
        private int custprodPK;
        private int packspecPK;
        DateTime bookingDate;
        private int artWorkPK;
        private bool isItemBind;
        private string refID;
        private string inboxFlag;
        private int processPK;
        private int isCart;
        private int SodPk;

        private string clientCode;

        private bool IsLotNoIOReview = false;

        //to maintain the Logged in User instance
        private BusinessObject.User currentUser;

        //used for Tax Formula calculation
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations
            = {
                  (a1, a2) => a1 - a2,
                  (a1, a2) => a1 + a2,
                  (a1, a2) => a1 / a2,
                  (a1, a2) => a1 * a2,
                  (a1, a2) => Math.Pow(a1, a2)
              };
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
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string prefID;
            int cusPK;
            int referenceID;
            int preferenceID;
            int appId;
            string artWorkDesc;
            DateDefaultEnum defaultDate;
            int defaultAddMonths;

            referenceID = 0;
            preferenceID = 0;
            appId = 0;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                hdfIsPostback.Value = "1";
                if (!IsPostBack)
                {
                    this.DataBind();
                    CreditLimitRuleBase = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "CreditCheckRuleBase").ToString());
                    ShowLotNo = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowLotNo"));
                    ShowLotSize = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowLotSize"));
                    ShowSCAdditionalPackDtls = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowSCAdditionalPackDetails"));
                    hdfCBPriceDecimals.Value = GetGlobalResourceObject("ConfigurationsRes", "CBPriceDecimalDigits").ToString();
                    pnlAlert.Visible = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowSCAlert")));
                    hdfIsAdditionalDetailsVisible.Value= GetGlobalResourceObject("ConfigurationsRes", "ExpandAdditionalDetails").ToString();
                    hdfIsShowProductRequired.Value = GetGlobalResourceObject("ConfigurationsRes", "IsProductRequired").ToString();
                    ShowCaseMarkCheckAllItem= Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowCaseMarkApplyAllItem"));
                    hdfContractDateChange.Value = GetLocalResourceObject("ContractDateChange").ToString();
                    if (ChkCaseMarkApplyAllItem.Checked)  
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                    saleOrderDetailsList.ForEach(lst => lst.SOD_CASE_MARK = saleOrderDetailsObj.SOD_CASE_MARK);
                                            }
                                        }
                                    }
                    if (hdfIsShowProductRequired.Value == "1")
                    {
                        rdbProduct.Visible = true;
                        lblrbtProduct.Visible = true;
                    }
                    else
                    {
                        rdbProduct.Visible = false;
                        lblrbtProduct.Visible = false;
                    }

                    if (GetGlobalResourceObject("ConfigurationsRes", "SCCaseMarkShow").ToString() == "1")
                    {
                        txtCaseMark.Visible = true;
                        lblCaseMark.Visible = true;
                        // lblItemDiscount.Visible = txtDiscount.Visible = imgDiscount.Visible = EnableItemDiscount > 0 ? true : false;
                    }
                    else
                    {
                        txtCaseMark.Visible = false;
                        lblCaseMark.Visible = false;
                    }


                    if (Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowSOCalcButton"))))
                    {
                        imbCalcQty.Visible = true;
                        txtBrandUOM.CssClass = "input-normal lbl-16-7perc";
                    }
                    else
                    {
                        imbCalcQty.Visible = false;
                        txtBrandUOM.CssClass = "input-normal input-small-b";
                    }
                    hdfIsPostback.Value = "0";
                    ConfigurationSettings();
                    txtBrand.Enabled = true;
                    hdfMailAttachmentName.Value = string.Empty;
                    hdfVersion.Value = "1";
                    //set of hidden fields used to format Quantity, Amount, Rate
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfDecimalFormat.Value = "#0.";
                    hdfDecimalVal.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithComma.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }

                    hdfWeightFormat.Value = "#0.";
                    int weightDecimalDigits = (Session[ERP.Utilities.SessionStrings.WeightDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.WeightDecimalDigit]));
                    for (int i = 0; i < weightDecimalDigits; i++)
                    {
                        hdfWeightFormat.Value += "0";
                    }

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfRateDigits.Value = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]).ToString();
                    hdfExchangeRateDigits.Value = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]).ToString();
                    // hdfWeightFormat.Value = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.WeightDecimalDigit]).ToString();

                    FileDetailsList = null;
                    SOUploadList = null;

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SOD_SL_NO";
                    grdItemDetails.DataKeyNames = itemkeyarray;

                    string[] itemkeyarrayUpload;
                    itemkeyarrayUpload = new string[1];
                    itemkeyarrayUpload[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarrayUpload;

                    if (Request.QueryString[QueryStrings.PageType] != null &&
                        Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                    {
                        IsAmend = true;
                        imbHdrRef.Visible = true;
                    }
                    //fetch and fill Company
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    if (ShowSCAdditionalPackDtls == 1)
                    {
                        divAdditionalDetails.Visible = true;
                        divAdditionalPackDtlsSearch.Visible = true;
                        GetFieldValues(ControlsEnum.STRAPPINGCOLOR);
                        SetFieldValues(ControlsEnum.STRAPPINGCOLOR);
                        GetFieldValues(ControlsEnum.STANDARD);
                        SetFieldValues(ControlsEnum.STANDARD);
                    }

                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                        if (GetGlobalResourceObject("ConfigurationsRes", "IsCompanyDisable").ToString() == "1")
                        {
                            ddlCompany.Enabled = false;
                        }
                    }
                    txtCurrency.Enabled = false;
                    btnRevision.Visible = false;

                    //set the Quotation PK and other page specific sessions to null
                    Session[ERP.Utilities.SessionStrings.QuotationHeader] = null;
                    SaleOrderHeaderSession = null;
                    TempSaleOrderHeaderSession = null;

                    //fetch and fill the Terms Popup
                    GetFieldValues(ControlsEnum.CONTRACTTERMS);
                    ltrTerms.Text = (dtPageData != null && dtPageData.Rows.Count > 0) ? dtPageData.Rows[0]["CON_DESC"].ToString() : string.Empty;

                    //hide/ show detail tax/ discount
                    GetFieldValues(ControlsEnum.TAXSETTINGS);

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPageData.Rows)
                        {
                            if (row["ACF_DATA"].ToString().Equals("DISCOUNT"))
                            {
                                EnableItemDiscount = Convert.ToInt32(row["ACF_VALUE"]);
                            }
                            else if (row["ACF_DATA"].ToString().Equals("TAX"))
                            {
                                EnableItemTax = Convert.ToInt32(row["ACF_VALUE"]);
                            }
                        }
                    }
                    lblItemDiscount.Visible = txtDiscount.Visible = imgDiscount.Visible = EnableItemDiscount > 0 ? true : false;
                    lblItemTax.Visible = txtTax.Visible = imgTax.Visible = EnableItemTax > 0 ? true : false;
                    imgHdrDiscount.Visible = EnableItemDiscount != 1 ? true : false;
                    imgHdrTax.Visible = EnableItemTax != 1 ? true : false;
                    ddlToPort.Visible = false;
                    txtToPort.Visible = true;

                    //Set SC Date as current Date
                    txtSaleOrderDate.Text = txtPODate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);

                    //ddlCompany.Focus();
                    //check if user is Customer
                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (int.TryParse(dtPageData.Rows[0]["CUS_PK"].ToString(), out cusPK) && cusPK > 0)
                        {
                            //set Customer and disable
                            custPK = cusPK;
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CUS_NAME"].ToString());
                            hdfCustomer.Value = cusPK.ToString();
                            txtCustomer.Enabled = false;
                            IsCustomerUser = true;

                            //feth and fill Customer Details like address, ship to port, currency
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                string addr = string.Empty;
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                }
                                txtBuyerAddress.Text = addr;

                                hdfCusAddress.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                hdfCusCountry.Value = dsPageData.Tables[0].Rows[0]["CUS_COUNTRY"].ToString();
                                hdfCusCountryText.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                hdfCusZip.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ZIP"].ToString());
                                hdfCusPhone.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_PHONE"].ToString());
                                hdfCusMobile.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_MOBILE"].ToString());
                                hdfCusFax.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_FAX"].ToString());
                                hdfCusEmail.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_EMAIL"].ToString());

                                txtPortofDischarge.Text = txtToPort.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SHIP_TO_PORT"].ToString());
                                if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value) && ddlSaleOrderType.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString()) != null)
                                    ddlSaleOrderType.SelectedValue = dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString();

                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value))
                                {
                                    soTypePK = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"]);
                                }
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());
                            }
                        }
                    }

                    //Fill Process and get WorkFlow RefID and fetch Application ID
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
                    }
                    //If Has PrefID (from Inbox) i.e. if coming from a different Process workflow to the current workflow
                    //if comes from Inbox with Quotation/Sale Order RefID
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
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        preferenceID = int.Parse(prefID);
                        appId = GetApplicationID(preferenceID);
                        if (hdfIsCrmCustomer.Value == "1")
                            CrmQuotationPK = appId;
                        else
                            CurrQuotationPK = appId;
                        Session[ERP.Utilities.SessionStrings.PRefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    //Sale Contract Entry Section, if has SC / SO / Quotation PK
                    if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null || Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    {
                        if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null)
                            CurrQuotationPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.QUOTATIONPK]);
                        if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                            CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);
                    }
                    
                    if (hdfIsCrmCustomer.Value == "1")
                        divCrmQuote.Visible = true;
                    else
                        divCrmQuote.Visible = false;
                    
                    if (CurrPK > 0 || CurrQuotationPK > 0)
                    {
                        //New SC from SO/Quotation
                        if (CurrPK == 0)
                        {
                            //if customer User, fill Exchange Rate, Previous Customer Contract Details
                            if (custPK > 0)
                            {
                                if (hdfCurrency.Value != string.Empty && hdfCurrency.Value != CommonConstants.SELECT_VALUE_ZERO)
                                {
                                    GetFieldValues(ControlsEnum.EXCHANGERATE);
                                    if (exchangeRate > 0)
                                    {
                                        hdfExchangeRate.Value = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                                        txtExchangeRate.Text = hdfExchangeRate.Value;
                                    }
                                    else
                                    {
                                        txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                        hdfCurrency.Value = "0";
                                        txtExchangeRate.Text = string.Empty;
                                    }

                                    //Not allowed to edit exchange rate while currency same as base currency                           
                                    if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))  //soTypePK == ((byte)SalesInvoiceType.Domestic) || 
                                    {
                                        txtExchangeRate.Enabled = false;
                                    }
                                    else
                                    {
                                        txtExchangeRate.Enabled = true;
                                    }
                                }
                                GetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                                SetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                            }
                            hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                            pnlPrintSO.Visible = false;
                        }
                        //Fetch Sale Contract by SC/So/Quotation PK
                        GetFieldValues(ControlsEnum.SALEORDER);
                        if (saleOrderHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','Quotation.aspx');", true);
                        }
                        if (saleOrderHeaderObj != null)
                            CurrPK = saleOrderHeaderObj.SOH_PK;

                        if (Session[ERP.Utilities.SessionStrings.SaleOrderMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.SaleOrderMode];
                        }
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        //Get RefID by AppID
                        if (CurrPK > 0)
                        {
                            if (referenceID == 0)
                            {
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            }
                        }
                        else if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                        {
                            //Assign SC Header ArtWork Description
                            saleOrderHeaderObj.SaleContractDetails.ForEach(dtl =>
                            {
                                artWorkDesc = string.Empty;
                                if (!string.IsNullOrEmpty(dtl.SOD_ART_WORK))
                                {
                                    artWorkDesc = dtl.PC_ART_WORK;
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(dtl.IB_ART_WORK) ? string.Empty : ", ", dtl.IB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(dtl.IC_ART_WORK) ? string.Empty : ", ", dtl.IC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(dtl.ZB_ART_WORK) ? string.Empty : ", ", dtl.ZB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(dtl.MC_ART_WORK) ? string.Empty : ", ", dtl.MC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(dtl.SC_ART_WORK) ? string.Empty : ", ", dtl.SC_ART_WORK);
                                    dtl.SOD_ART_WORK_DESC = artWorkDesc;
                                }
                            });
                        }
                        //Assign SC Sessions to maintain data
                        SaleOrderHeaderSession = saleOrderHeaderObj;
                        TempSaleOrderHeaderSession = saleOrderHeaderObj;

                        SetFieldValues(ControlsEnum.SALEORDERHEADER);

                        //GetFieldValues(ControlsEnum.TOPORT);
                        //SetFieldValues(ControlsEnum.TOPORT);
                        //if (saleOrderHeaderObj.SOH_CURRENCY_TEXT == "INR" && IsToPortDdlShow == true)
                        //{
                        //    ddlToPort.SelectedValue = saleOrderHeaderObj.SOH_TO_PORT_PK;
                        //    ddlToPort.Visible = true;
                        //    txtToPort.Visible = false;
                        //}
                        //else
                        //{
                        //    ddlToPort.Visible = false;
                        //    txtToPort.Visible = true;
                        //}

                        isItemBind = true;
                        txtDtlRemark2.Text = string.Empty;
                    }
                    //Create New SC
                    else
                    {
                        lblSaleOrderNo.Text = Resources.Messages.DocGenerationNew;
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;

                        //Set Booking Date from Resource 
                        defaultDate = DateDefaultEnum.CurrentDate;
                        defaultAddMonths = 0;
                        Enum.TryParse(GetLocalResourceObject("DefaultDayBooking").ToString(), out defaultDate);
                        int.TryParse(GetLocalResourceObject("DefaultAddMonthsBooking").ToString(), out defaultAddMonths);
                        txtBookingDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ? defaultAddMonths + 1 : defaultAddMonths)
                            .AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                            defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);

                        //Set Required by Date from Resource 
                        defaultDate = DateDefaultEnum.CurrentDate;
                        defaultAddMonths = 0;
                        Enum.TryParse(GetLocalResourceObject("DefaultDayReqdBy").ToString(), out defaultDate);
                        int.TryParse(GetLocalResourceObject("DefaultAddMonthsReqdBy").ToString(), out defaultAddMonths);
                        txtSpecReqByDate.Text = txtReqByDate.Text = txtShipmentDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ?
                            defaultAddMonths + 1 : defaultAddMonths).AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                            defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);


                        if (Session[ERP.Utilities.SessionStrings.SaleOrderMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.SaleOrderMode];
                        }
                        else
                            EntryStatus = EntryStatus.NEWMODE;
                        //if Copy Contract, then fetch the selected Contract and fill Details
                        if (Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK] != null)
                        {
                            copyContractPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK]);
                            GetFieldValues(ControlsEnum.COPYCONTRACT);
                            SetFieldValues(ControlsEnum.COPYCONTRACT);
                            isItemBind = true;
                        }
                        else
                        {
                            //if user is Customer, the fill Customer details
                            if (custPK > 0)
                            {
                                if (hdfCurrency.Value != string.Empty && hdfCurrency.Value != CommonConstants.SELECT_VALUE_ZERO)
                                {
                                    GetFieldValues(ControlsEnum.EXCHANGERATE);
                                    if (exchangeRate > 0)
                                    {
                                        hdfExchangeRate.Value = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                                        txtExchangeRate.Text = hdfExchangeRate.Value;
                                    }
                                    else
                                    {
                                        txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                        hdfCurrency.Value = "0";
                                    }
                                    //Not allowed to edit exchange rate while currency same as base currency                           
                                    if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))  //soTypePK == ((byte)SalesInvoiceType.Domestic) || 
                                    {
                                        txtExchangeRate.Enabled = false;
                                    }
                                    else
                                    {
                                        txtExchangeRate.Enabled = true;
                                    }
                                }
                                GetFieldValues(ControlsEnum.NOTIFYPARTY);
                                SetFieldValues(ControlsEnum.NOTIFYPARTY);
                                GetFieldValues(ControlsEnum.CONSIGNEE);
                                SetFieldValues(ControlsEnum.CONSIGNEE);
                                GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                                SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                                //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                                //SetFieldValues(ControlsEnum.DELIVERYTERMS);
                                GetFieldValues(ControlsEnum.SHIPMENTTERMS);
                                SetFieldValues(ControlsEnum.SHIPMENTTERMS);
                                GetFieldValues(ControlsEnum.PAYMENTTERMS);
                                SetFieldValues(ControlsEnum.PAYMENTTERMS);
                                GetFieldValues(ControlsEnum.SPECIALCAUSE);
                                SetFieldValues(ControlsEnum.SPECIALCAUSE);
                                GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                                SetFieldValues(ControlsEnum.CUSTOMERADDRESS);

                                GetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                                SetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                            }
                            SaleOrderHeaderSession = new SaleContractBO()
                            {
                                TaxHdr = new List<SaleOrderTaxHdr>(),
                                SaleContractDetails = new List<SaleContractDetailsBO>()
                            };
                            TempSaleOrderHeaderSession = new SaleContractBO()
                            {
                                TaxHdr = new List<SaleOrderTaxHdr>(),
                                SaleContractDetails = new List<SaleContractDetailsBO>()
                            };

                            //fill DropDowns and assign default values
                            GetFieldValues(ControlsEnum.SOTYPE);
                            SetFieldValues(ControlsEnum.SOTYPE);

                            // 18-08-2017 If need to list all Consignee & Notify party
                            //GetFieldValues(ControlsEnum.CONSIGNEE); 
                            //SetFieldValues(ControlsEnum.CONSIGNEE);
                            //GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            //SetFieldValues(ControlsEnum.NOTIFYPARTY);

                            //if (dtPageData != null && dtPageData.Rows.Count > 0
                            //     && ddlSaleOrderType.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                            //    ddlSaleOrderType.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();

                            GetFieldValues(ControlsEnum.SOSUBTYPE);
                            SetFieldValues(ControlsEnum.SOSUBTYPE);

                            GetFieldValues(ControlsEnum.SHIPBY);
                            SetFieldValues(ControlsEnum.SHIPBY);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlShipBy.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlShipBy.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            //GetFieldValues(ControlsEnum.FROMPORT);
                            //SetFieldValues(ControlsEnum.FROMPORT);
                            //if (dtPageData != null && dtPageData.Rows.Count > 0
                            //     && ddlFromPort.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                            //    ddlFromPort.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlTranshipment.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlTranshipment.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            SetFieldValues(ControlsEnum.BANKDETAILS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlBankDetails.Items.FindByValue(dtPageData.Rows[0]["CBM_PK"].ToString()) != null)
                                ddlBankDetails.SelectedValue = dtPageData.Rows[0]["CBM_PK"].ToString();

                            GetFieldValues(ControlsEnum.INSPECTION);
                            SetFieldValues(ControlsEnum.INSPECTION);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlInspection.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                                ddlInspection.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();

                            GetFieldValues(ControlsEnum.EXPORTDOC);
                            SetFieldValues(ControlsEnum.EXPORTDOC);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlExportDoc.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                                ddlExportDoc.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();

                            GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlOriginofGoods.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlOriginofGoods.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            txtShipping.Text = ((double)0).ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = ((double)0).ToString(hdfCurrencyFormat.Value);
                            txtDtlRemark2.Text = string.Empty;
                        }

                        if (CrmQuotationPK > 0)
                        {
                            #region Validate Customer Exists
                            DataTable dtValidate = BusinessLogic.Sales.QuotationBL.ValidateCRMCustomer(CrmQuotationPK);
                            if(dtValidate!=null && dtValidate.Rows.Count > 0)
                            {
                                if (dtValidate.Rows[0][0].ToString() == "0")
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_CrmCustomer").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                }
                            }
                            #endregion
                            GetFieldValues(ControlsEnum.CRMCUSTOMER);
                            SetFieldValues(ControlsEnum.CRMCUSTOMER);
                            GetFieldValues(ControlsEnum.QUOTATION);
                            SetFieldValues(ControlsEnum.QUOTATION);
                            txtQuotation.Enabled = false;
                            txtCustomer.Enabled = false;
                        }
                    }


                    if (GetGlobalResourceObject("ConfigurationsRes", "SCApproxCtnBoxPriceVisible").ToString() == "1")
                    {
                        lblctnRate.Visible = true;
                        txtctnRate.Visible = true;
                    }
                    else
                    {
                        lblctnRate.Visible = false;
                        txtctnRate.Visible = false;
                    }

                    AST_DOC_MODE.Value = "0";
                    if (lblSaleOrderNo.Text.Trim().Equals(string.Empty) || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                    {
                        AST_DOC_MODE.Value = GetDOCMODE();
                    }
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                        ucrWrkf.ViewType = 1;
                    else
                        ucrWrkf.ViewType = 0;

                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                    ddlCompany.Focus();
                    rdbBrand.Checked = true;
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
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.SALEORDER:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(CurrQuotationPK, CurrPK);
                        break;
                    case ControlsEnum.COPYCONTRACT:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(0, copyContractPK);
                        break;
                    case ControlsEnum.CUSTOMERCONTRACT:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(0, 0, custPK);
                        break;
                    case ControlsEnum.FROMPORT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.FromPort, 1, currentUser.SBUID);
                        break;

                    case ControlsEnum.SOSUBTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.SaleContract, Convert.ToInt32(ddlSaleOrderType.SelectedValue) == 1 ? 2 : (Convert.ToInt32(ddlSaleOrderType.SelectedValue) == 2 ? 3 : 0), 1, currentUser.SBUID);
                        break;

                    case ControlsEnum.ORIGINOFGOODS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.OriginOfGoods, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SOTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    case ControlsEnum.CBMCONFIG:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.CBM, string.Empty, currentUser.SBUID);
                        break;
                    case ControlsEnum.BANKDETAILS:
                        //dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID, (int)CashBankType.Bank);                        
                        dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(BankPk, (int)DbActiveStatus.ACTIVE, currentUser.SBUID, (int)CashBankType.Bank);
                        break;
                    case ControlsEnum.NOTIFYPARTY:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(notifyPartyPK, custPK, notifyPartyPK > 0 ? 2 : 1, (int)CustomerAddressType.NotifyParty).Tables[0];
                        break;
                    case ControlsEnum.CONSIGNEE:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(consigneePK, custPK, consigneePK > 0 ? 2 : 1, (int)CustomerAddressType.Consignee).Tables[0];
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(agentPK, custPK, agentPK > 0 ? 2 : 1, (int)CustomerAddressType.ShippingAgent).Tables[0];
                        break;
                    case ControlsEnum.AGENT:
                        string xmlDoc;
                        ProductDtlBOObj = (ProductDtlBO)SetUIValuesToObject(ControlsEnum.AGENT);
                        if (ProductDtlBOObj != null && ProductDtlBOObj.ItemsList != null)
                        {
                            xmlDoc = CommonFunctions.XmlSerialize<ProductDtlBO>(ProductDtlBOObj);
                            dtPageData = BusinessLogic.Sales.CustomerProduct.GetcommissionAgent(xmlDoc, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID).Tables[0];
                        }
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.Transhipment, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPBY:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipBy, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, deliveryTermPK > 0 ? 2 : 1); //Bug : 6020
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, 1);
                        break;
                    case ControlsEnum.DELIVERYTERMSBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1); //Bug : 6020
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, 1);
                        break;
                    case ControlsEnum.PAYMENTTERMSBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, specialCausePK > 0 ? 2 : 1); //Bug : 6020
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, 1);
                        break;
                    case ControlsEnum.SPECIALCAUSEBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.CUSTOMER:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, string.Empty, currentUser.SBUID, 2);
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtSaleOrderDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            exchangeRate = string.IsNullOrEmpty(dsExchangeRate.Tables[0].Rows[0][0].ToString()) ? -1 :
                                Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                            //txtExchangeRate.Text = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            exchangeRate = -1;
                            //txtExchangeRate.Text = "";
                        }
                        break;
                    case ControlsEnum.CUSTOMERPRODUCT:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetBrandDetails(custprodPK);
                        //dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(custprodPK, productPK, custPK, string.Empty, string.Empty, currentUser.SBUID, custprodPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.PACKINGSPEC:
                        dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(artWorkPK, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, 0, custprodPK, string.Empty, true);
                        break;
                    case ControlsEnum.PACKINGSPECDETAILS:
                        dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(artWorkPK, artWorkPK > 0 ? Convert.ToInt32(DbActiveStatus.HASPK)
                            : Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, 0, custprodPK, string.Empty, true);
                        break;
                    case ControlsEnum.BRANDRATE:
                        dtPageData = BusinessLogic.BrandRates.BrandRatesBL.GetBrandRate(custprodPK, 0, 0, bookingDate);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(addressPK, custPK, addressActive == 2 ? 2 : 1, (int)CustomerAddressType.ShippingAddress).Tables[0];
                        break;
                    //case ControlsEnum.ARTWORK:
                    //    dtGrid = BusinessLogic.Sales.CustomerProduct.GetArtWork(artPK, custprodPK, artPK > 0 ? 2 : 1, string.Empty);
                    //    break;
                    case ControlsEnum.INSPECTION:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO INSP TYPE");
                        break;
                    case ControlsEnum.EXPORTDOC:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO EXP DOC");
                        break;
                    case ControlsEnum.CONTRACTTERMS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.SaleContract, 1, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsSaleOrderTaxDetails = BusinessLogic.Sales.QuotationBL.GetQuotationTaxDetails(TaxPK, 0, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                            if (dsSaleOrderTaxDetails != null && dsSaleOrderTaxDetails.Tables.Count > 0)
                            {
                                dtSaleOrderTaxDetails = dsSaleOrderTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            //dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL);

                            if ((int)TaxType.Shipping == category)//shipping is not under sales or Purchase
                            {
                                string CatXML = CommonFunctions.GetOtherChargeCatXML(GetLocalResourceObject("OtherChargeCategoryValues").ToString());
                                if (CatXML != null || CatXML != string.Empty)
                                {
                                    category = 0;
                                }
                                dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL, 1,0,0,CatXML);

                            }
                            else
                            {
                                if ((int)TaxType.Discount != category)
                                {
                                    dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL, 1, 0, 1);
                                }
                                else
                                {
                                    dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0);
                                }
                            }
                        }
                        break;
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetRevisionHistory(CurrPK);
                        break;
                    case ControlsEnum.AUTOTAXPOPUP:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetCustomerSupplyTax(1, currentUser.SBUID, Convert.ToDateTime(txtSaleOrderDate.Text), Convert.ToInt16(hdfCustomer.Value), 0);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            IsHeaderTax = true;
                        }
                        break;
                    case ControlsEnum.AUTOTAXPOPUPITEMWISE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetCustomerSupplyTax(1, currentUser.SBUID, Convert.ToDateTime(txtSaleOrderDate.Text), null, Convert.ToInt32(hdfBrand.Value));
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            IsHeaderTax = false;
                        }
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);

                        break;
                    #endregion
                    case ControlsEnum.CUSTOMTAXSETTINGS:
                        IsCustomTaxEnabled = true;
                        dtCustomTaxSet = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CUSTOM TAX SETTINGS", "TAX REQUIRED");
                        if (dtCustomTaxSet != null && dtCustomTaxSet.Rows.Count > 0)
                        {
                            int cfgval = Convert.ToInt32(dtCustomTaxSet.Rows[0]["ACF_VALUE"]);
                            if (cfgval == 0)
                            {
                                IsCustomTaxEnabled = false;
                            }
                        }
                        break;
                    case ControlsEnum.CLIENTCODE:
                        commonServiceObj = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_SETTING = "CLIENT CODE";
                        admAppConfigMstList = commonServiceObj.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        clientCode = admAppConfigMstList.FirstOrDefault().ACF_DATA;
                        break;
                    #region CONTAINERTYPEDETAILS
                    case ControlsEnum.CONTAINERTYPEDETAILS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, 6, 1, currentUser.SBUID);
                        break;
                    #endregion

                    case ControlsEnum.STRAPPINGCOLOR:
                        dtStrapping = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Packing, 9, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.STANDARD:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Packing, 10, 1, currentUser.SBUID);
                        break;
                    #region TOPORT
                    case ControlsEnum.TOPORT:
                        dtToPort = BusinessLogic.CommonManagement.CommonBL.GetPortDetails(null, 0, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID,
                                  Convert.ToInt16(ddlSaleOrderType.SelectedValue), 0, 1, 0, 0);
                        break;
                    #endregion

                    #region ITEM REF DTL
                    case ControlsEnum.ITEMREFDTL:
                        saleOrderItemRefObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractItemRef(CurrPK, SodPk);
                        if (SodPk == 0)
                            SaleOrderItemRefHdr = saleOrderItemRefObj.DeepClone();
                        break;
                    #endregion

                    #region SALESCOST
                    case ControlsEnum.SALESCOST:
                        xmlDoc = CommonFunctions.XmlSerialize<SalesCostBO>(objSCcost);
                        dtSalesCost = BusinessLogic.Sales.SaleOrderBL.GetSalesCostDetails(xmlDoc);
                        break;
                    #endregion

                    case ControlsEnum.PACKSPECSELECTED:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetPackSepecDetails(packspecPK, Convert.ToInt32(hdfIsProductRequired.Value));
                        break;

                    case ControlsEnum.SHIPMENTTERMS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipmentTerms, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPMENTTERMSBYPK:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(shipmentTermPK, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipmentTerms, 2, currentUser.SBUID);
                        break;
                    case ControlsEnum.QUOTATION:
                    case ControlsEnum.CRMCUSTOMER:
                        dtPageData = BusinessLogic.Sales.QuotationBL.GetCrmQuoationDetails(CrmQuotationPK);
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
                    #region CBMCONFIG
                    case ControlsEnum.CBMCONFIG:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfTotalCBM.Value = dtPageData.Rows[0]["ACF_DATA"].ToString();
                        }

                        break;
                    #endregion

                    #region SALEORDERHEADER
                    case ControlsEnum.SALEORDERHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion

                    #region   COPYCONTRACT
                    case ControlsEnum.COPYCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            saleOrderHeaderObj.SOH_PK = 0;
                            saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                                 || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                                ? string.Empty : lblSaleOrderNo.Text.Trim();
                            saleOrderHeaderObj.SOH_DATE = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtSaleOrderDate.Text.Trim();
                            saleOrderHeaderObj.SOH_REFERENCE = null;
                            saleOrderHeaderObj.SOH_REFERENCE_DATE = null;
                            saleOrderHeaderObj.FileList = null;
                            if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                saleOrderHeaderObj.SOH_BOOKING_DATE = txtBookingDate.Text;
                            if (!string.IsNullOrEmpty(txtShipmentDate.Text))
                                saleOrderHeaderObj.SOH_DELIVERY_DATE = txtShipmentDate.Text;
                            if (saleOrderHeaderObj.SaleContractDetails != null)
                            {
                                saleOrderHeaderObj.SaleContractDetails.ForEach(dtl =>
                                {
                                    dtl.SOD_PK = 0;
                                    dtl.SOD_REQUIRED_BY = txtReqByDate.Text;
                                    dtl.SOD_LOT_NO = string.Empty;  // No need to create lot no/size while copying SC
                                    dtl.SOD_LOT_SIZE = string.Empty;
                                    dtl.TaxDtl.ForEach(dtx =>
                                    {
                                        dtx.SLT_PK = 0;
                                    });
                                });
                                saleOrderHeaderObj.TaxHdr.ForEach(tax =>
                                {
                                    tax.SLT_PK = 0;
                                });
                            }
                            SaleOrderHeaderSession = saleOrderHeaderObj;
                            TempSaleOrderHeaderSession = saleOrderHeaderObj;
                        }
                        GetUIValuesFromObject(controlType);
                        #region Remove Inactive brands and rest the tax,discount,other charges etc
                        if (SaleOrderHeaderSession != null)
                        {
                            SaleOrderHeaderSession.SaleContractDetails.RemoveAll(r => r.SOD_CUST_ITEM_ACTIVE == 0 && r.SOD_IS_PACK_MAT == 3); //for packing material SOD_CUST_ITEM_ACTIVE=0 so entry is romoved from the list
                            SetDetailTax(SaleOrderHeaderSession);
                            SetSubTotal();
                            SetHdrTax();
                            saleOrderHeaderObj = SaleOrderHeaderSession;
                        }
                        #endregion
                        break;
                    #endregion

                    case ControlsEnum.CUSTOMERCONTRACT:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SALEORDERDETAIL:

                        if (rdbPackingSpec.Checked == true)
                        {
                            BindGrid(controlType);

                            lblTotalCBM.Text = lblTotalCBM.ToolTip = saleOrderHeaderObj.SaleContractDetails.Sum(itm => itm.APS_TOTAL_PCS <= 0 ? 0.0 :
                             Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM, 4)).ToString("N4");
                        }
                        else
                        {
                            if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                            {
                                txtCustomer.Enabled = (!IsCustomerUser) && saleOrderHeaderObj.SaleContractDetails.Count == 0;
                                BindGrid(controlType);
                                //lblTotalCBM.Text = lblTotalCBM.ToolTip = saleOrderHeaderObj.SaleContractDetails.Sum(itm =>
                                //    Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM).ToString("N4");

                                lblTotalCBM.Text = lblTotalCBM.ToolTip = saleOrderHeaderObj.SaleContractDetails.Sum(itm => itm.APS_TOTAL_PCS <= 0 ? 0.0 :
                                  Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM, 4)).ToString("N4");

                                lblGrossWeight.Text = lblGrossWeight.ToolTip = saleOrderHeaderObj.SOH_QTY_GROSS_WT.ToString();

                                //lblTotalCBM.Text = lblTotalCBM.ToolTip = saleOrderHeaderObj.SaleContractDetails.Sum(itm =>
                                //   (Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS)) * Convert.ToDouble(itm.CBM.ToString("N4"))).ToString("N4");
                                if (Convert.ToInt16(hdfAgtSaves.Value) > 0)
                                {
                                    ddlAgentN.SelectedIndex = ddlAgentN.Items.IndexOf(ddlAgentN.Items.FindByValue(hdfAgtSaves.Value));
                                }
                            }
                        }
                        break;
                    case ControlsEnum.PACKINGSPEC:
                        BindDropDownList(ControlsEnum.PACKINGSPEC);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            BindDropDownList(controlType);
                            txtShippingAddress.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                            hdfShpName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                            if (addressPK > 0 && dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addressPK = Convert.ToInt32(ddlCustAddress.SelectedValue);
                                addressActive = 2;
                                GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                                string addr;
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfShpName.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfShpCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                hdfShpAddress.Value = addr;
                                hdfShpAddress.Value = txtShippingAddress.Text = addr;
                            }
                        }
                        break;
                    case ControlsEnum.BANKDETAILS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.FROMPORT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.ORIGINOFGOODS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.CONSIGNEE:
                        BindDropDownList(controlType);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfCNECountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();
                            hdfCNECountryText.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                            hdfCNEEmail.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_EMAIL"].ToString());
                            hdfCNEFax.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_FAX"].ToString());
                            hdfCNEMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                            hdfCNEPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                            hdfCNEZip.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ZIP"].ToString());
                        }
                        break;
                    case ControlsEnum.NOTIFYPARTY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.AGENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.TOPORT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SOSUBTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHIPBY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        BindDropDownList(controlType);
                        txtDeliveryTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        BindDropDownList(controlType);
                        txtPaymentTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        BindDropDownList(controlType);
                        txtSpecialCause.Text = string.Empty;
                        break;
                    case ControlsEnum.INSPECTION:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.EXPORTDOC:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(ControlsEnum.TAXPOPUPGRID);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDownList(ControlsEnum.COMPANY);
                        break;

                    case ControlsEnum.AUTOTAXPOPUP:
                    case ControlsEnum.AUTOTAXPOPUPITEMWISE:

                        SaleOrderTaxHdr saleOrderTaxHdrObj;
                        List<SaleOrderTaxHdr> saleOrderTaxHdrList;
                        saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                        saleOrderHeaderObj = TempSaleOrderHeaderSession;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfisItemHaveTax.Value = "1";//Item Have Tax
                            if ((!IsHeaderTax || EnableItemTax != 0) && hdfBrand.Value != string.Empty && Convert.ToInt32(hdfBrand.Value) > 0)
                            {
                                saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;
                                hdfIsBrandYes.Value = "1"; IsBrand = true;
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                if (Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableCustomerTaxforSChdrDtl")) != 1)
                                    saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl == null ? new List<SaleOrderTaxHdr>() : saleOrderDetailsObj.TaxDtl.ToList();
                                else
                                    saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                hdfIsBrandYes.Value = "0"; IsBrand = false;
                                if (CurrSlNo > 0)
                                {
                                    saleOrderHeaderObj.SaleContractDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = null;
                                }
                            }
                            for (int i = 0; i < dtPageData.Rows.Count; i++)
                            {
                                saleOrderTaxHdrObj = new SaleOrderTaxHdr();
                                saleOrderTaxHdrObj.SLT_NAME = dtPageData.Rows[i]["CMT_TAX_TEXT"].ToString();
                                saleOrderTaxHdrObj.SLT_TAX = Convert.ToInt16(dtPageData.Rows[i]["TAX_PK"]);
                                //saleOrderTaxHdrObj.SLT_DIS_PERC = 0;
                                saleOrderTaxHdrObj.SLT_TAX_AMT = 0;
                                saleOrderTaxHdrObj.SLT_TAX_CATEGORY = Convert.ToInt16(dtPageData.Rows[i]["TAX_CATEGORY"]);
                                saleOrderTaxHdrObj.SLT_TAX_FORMULA = dtPageData.Rows[i]["TAX_FORMULA"].ToString();
                                saleOrderTaxHdrObj.SLT_TAX_TEXT = dtPageData.Rows[i]["TAX_HEAD"].ToString();
                                if (EnableItemTax == 2)
                                    saleOrderTaxHdrObj.SLT_HAS_OTHER_CHARGE = 1;

                                if (IsHeaderTax && EnableItemTax == 0)
                                {
                                    saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                }
                                else
                                {
                                    saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);

                                    if (hdfBrand.Value != string.Empty)
                                    {
                                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                                        SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                                        SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                                        if (CurrSlNo > 0)
                                        {
                                            saleOrderHeaderObj.SaleContractDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = saleOrderTaxHdrList;
                                        }
                                        else
                                        {
                                            saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
            && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                        }
                                    }
                                }
                            }
                            //saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                            if (IsHeaderTax && EnableItemTax != 1)
                            {
                                saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;

                                txtHdrTax.Text = "0.00";


                            }
                            else
                            {
                                txtTax.Text = "0.00";
                                txtDiscount.Text = "0.00";
                            }
                            TempSaleOrderHeaderSession = saleOrderHeaderObj;
                            // SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            //saleOrderHeaderObj = SaleOrderHeaderSession;

                        }
                        else
                        {
                            hdfisItemHaveTax.Value = "0";//ItemHave no Tax

                            if (IsHeaderTax)
                            {
                                saleOrderHeaderObj = TempSaleOrderHeaderSession;
                                saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                            }
                            else
                            {
                                saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;
                                hdfIsBrandYes.Value = "1"; IsBrand = true;
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                hdfIsBrandYes.Value = "0"; IsBrand = false;
                                saleOrderHeaderObj = TempSaleOrderHeaderSession;
                                if (CurrSlNo > 0)
                                {
                                    if (saleOrderHeaderObj.SaleContractDetails.Count(p => p.SOD_SL_NO == CurrSlNo) > 0)
                                        saleOrderTaxHdrList = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = null;
                                    else
                                        saleOrderTaxHdrList = null;
                                    txtTax.Text = "0.00";
                                    txtDiscount.Text = "0.00";
                                }
                                else
                                {
                                    saleOrderTaxHdrList = null;// saleOrderHeaderObj.SaleContractDetails.Where(rfq => rfq.SOD_PK == SelectedDtlPK                                        && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).ToList().TaxDtl = null;

                                }
                            }
                        }
                        break;

                    case ControlsEnum.UPLOADEDFILES:
                        if (saleOrderHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.STRAPPINGCOLOR:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.STANDARD:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.ITEMREFDTL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SALESCOST:
                        BindGrid(ControlsEnum.SALESCOST);
                        break;
                    case ControlsEnum.SHIPMENTTERMS:
                        BindDropDownList(controlType);
                        txtDeliveryTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.CRMCUSTOMER:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtCustomer.Text = dtPageData.Rows[0]["CQH_CUS_NAME"].ToString();
                            hdfCustomer.Value = dtPageData.Rows[0]["CQH_CUS_PK"].ToString();
                            ActionHandler(btnCustSelected, EventArgs.Empty);
                        }
                        break;
                    case ControlsEnum.QUOTATION:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtQuotation.Text = dtPageData.Rows[0]["CQH_NO"].ToString();
                            hdfQuotationPK.Value = dtPageData.Rows[0]["CQH_PK"].ToString();
                        }
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
            string artWorkDesc;
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //Label lblSubTotal;
            try
            {
                switch (controlType)
                {
                    #region SaleContract Header
                    case ControlsEnum.SALEORDERHEADER:
                        if (SaleOrderHeaderSession != null)
                        {

                            saleOrderHeaderObj = SaleOrderHeaderSession;
                            saleOrderHeaderObj.SOH_PK = CurrPK;
                            if (hdfCreditCheckContinue.Value == "1")
                                saleOrderHeaderObj.SOH_CREDIT_CHECK = 0; //If creditnlimit checking done in rule base and submit user confirmed then SP level checking not required
                            else
                                saleOrderHeaderObj.SOH_CREDIT_CHECK = 1;//If creditnlimit checking done in rule base and submit user not confirmed then SP level checking required

                            if (CurrPK == 0)
                                saleOrderHeaderObj.SOH_STATUS = 0;
                            saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                                 || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                                ? string.Empty : lblSaleOrderNo.Text.Trim();
                            saleOrderHeaderObj.SOH_DATE = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtSaleOrderDate.Text.Trim();
                            saleOrderHeaderObj.SOH_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                            saleOrderHeaderObj.SOH_CUSTOMER_NAME = HttpUtility.HtmlEncode(txtCustomer.Text);

                            //  if (txtBuyerAddress.Text.Trim() != string.Empty)
                            saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS = HttpUtility.HtmlEncode(txtBuyerAddress.Text); //HttpUtility.HtmlEncode(hdfCusAddress.Value);
                            if (hdfCusCountry.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY = hdfCusCountry.Value.Trim();
                            if (hdfCusCountryText.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfCusCountryText.Value);
                            if (hdfCusEmail.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_EMAIL = HttpUtility.HtmlEncode(hdfCusEmail.Value);
                            if (hdfCusFax.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_FAX = HttpUtility.HtmlEncode(hdfCusFax.Value);
                            if (hdfCusMobile.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_MOBILE = HttpUtility.HtmlEncode(hdfCusMobile.Value);
                            if (hdfCusPhone.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_PHONE = HttpUtility.HtmlEncode(hdfCusPhone.Value);
                            if (hdfCusZip.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_ZIP = HttpUtility.HtmlEncode(hdfCusZip.Value);

                            if (saleOrderHeaderObj.SOH_QUOTATION > 0)
                            {
                                // if (!string.IsNullOrEmpty(txtRefNo.Text)) //When copy a sale contract(created through Quotation or Sale Order),this codition saves copied refno and date
                                saleOrderHeaderObj.SOH_REF_NO = HttpUtility.HtmlEncode(txtRefNo.Text);
                                //if (!string.IsNullOrEmpty(txtRefDate.Text))
                                saleOrderHeaderObj.SOH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text) ? string.Empty : txtRefDate.Text;
                            }

                            saleOrderHeaderObj.QuotationPK = hdfQuotationPK.Value == string.Empty ? 0 : Convert.ToInt32(hdfQuotationPK.Value);

                            if (Request.QueryString[QueryStrings.PageType] != null && Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                            {
                                saleOrderHeaderObj.SOH_AMEND_DATE = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_AMEND_DATE = string.IsNullOrEmpty(txtAmendDate.Text) ? string.Empty : txtAmendDate.Text;
                            }


                            if (!string.IsNullOrEmpty(txtPONo.Text))
                                saleOrderHeaderObj.SOH_REFERENCE = HttpUtility.HtmlEncode(txtPONo.Text);
                            if (!string.IsNullOrEmpty(txtPODate.Text))
                                saleOrderHeaderObj.SOH_REFERENCE_DATE = txtPODate.Text;
                            if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                saleOrderHeaderObj.SOH_BOOKING_DATE = txtBookingDate.Text;
                            saleOrderHeaderObj.SOH_TYPE = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                            if (hdfEnableSubType.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                saleOrderHeaderObj.SOH_SUB_TYPE = Convert.ToInt32(ddlSaleOrderSubtype.SelectedValue);
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_SUB_TYPE = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                            }

                            saleOrderHeaderObj.SOH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                            saleOrderHeaderObj.SOH_CURRENCY_BC = currentUser.BaseCurrency;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            saleOrderHeaderObj.SOH_CURRENCY_RATE = string.IsNullOrEmpty(txtExchangeRate.Text) ? 1 : Convert.ToDouble(txtExchangeRate.Text);//string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);

                            if (ddlShipBy.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SHIP_BY = ddlShipBy.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_SHIP_BY = string.Empty;
                            }
                            if (!string.IsNullOrEmpty(txtShipmentDate.Text))
                                saleOrderHeaderObj.SOH_DELIVERY_DATE = txtShipmentDate.Text;
                            if (!string.IsNullOrEmpty(txtShipmentDateText.Text))
                                saleOrderHeaderObj.SOH_SHIPMENT_DESC = txtShipmentDateText.Text;

                            //FROM PORT - TO PORT
                            //For Demo-IN, To Port is made dropdown for Buyers with currency code 'INR' : Bug-3544
                            saleOrderHeaderObj.SOH_FROM_PORT = hdfFromPortID.Value.ToString();
                            if (IsToPortDdlShow == true && (saleOrderHeaderObj.SOH_CURRENCY_TEXT == "INR" || CusCurrencyCode == "INR"))
                            {
                                if (ddlToPort.SelectedValue != CommonConstants.SELECTVAL)
                                {
                                    saleOrderHeaderObj.SOH_TO_PORT_PK = ddlToPort.SelectedValue;
                                    saleOrderHeaderObj.SOH_TO_PORT = ddlToPort.SelectedItem.Text;
                                }
                                else
                                {
                                    saleOrderHeaderObj.SOH_TO_PORT_PK = saleOrderHeaderObj.SOH_TO_PORT = string.Empty;
                                }
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_TO_PORT_PK = hdfToPortID.Value.ToString();
                                saleOrderHeaderObj.SOH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text);
                            }
                            //For Demo-IN, To Port is made dropdown for Buyers with currency code 'INR' : Bug-3544

                            if (ddlTranshipment.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_TRANSHIPMENT = ddlTranshipment.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_TRANSHIPMENT = string.Empty;
                            }

                            saleOrderHeaderObj.SOH_FINAL_DESTINATION = HttpUtility.HtmlEncode(txtPortofDischarge.Text);

                            if (ddlConsigneeDetails.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_CONSIGNEE = ddlConsigneeDetails.SelectedValue;
                                if (hdfCNEName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_NAME = ddlConsigneeDetails.SelectedItem.Text;
                                if (hdfCNEAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS = hdfCNEAddress.Value;
                                if (hdfCNECountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY = hdfCNECountry.Value.Trim();
                                if (hdfCNECountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfCNECountryText.Value);
                                if (hdfCNEEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL = HttpUtility.HtmlEncode(hdfCNEEmail.Value);
                                if (hdfCNEFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_FAX = HttpUtility.HtmlEncode(hdfCNEFax.Value);
                                if (hdfCNEMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE = HttpUtility.HtmlEncode(hdfCNEMobile.Value);
                                if (hdfCNEPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_PHONE = hdfCNEPhone.Value;
                                if (hdfCNEZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_ZIP = HttpUtility.HtmlEncode(hdfCNEZip.Value);
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_CONSIGNEE = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_NAME = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_FAX = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_PHONE = string.Empty;
                                saleOrderHeaderObj.SOH_CONSIGNEE_ZIP = string.Empty;
                            }

                            if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SHIPPING_TO = ddlCustAddress.SelectedValue;
                                if (hdfShpName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_NAME = ddlCustAddress.SelectedItem == null ? hdfShpName.Value : HttpUtility.HtmlEncode(ddlCustAddress.SelectedItem.Text);// hdfShpName.Value;
                                if (txtShippingAddress.Text.Trim() != string.Empty)//hdfShpAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_ADDRESS = txtShippingAddress.Text;//hdfShpAddress.Value;
                                if (hdfShpCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_COUNTRY = hdfShpCountry.Value.Trim();
                                if (hdfShpCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT = hdfShpCountryText.Value;
                                if (hdfShpEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_EMAIL = hdfShpEmail.Value;
                                if (hdfShpFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_FAX = hdfShpFax.Value;
                                if (hdfShpMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_MOBILE = hdfShpMobile.Value;
                                if (hdfShpPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_PHONE = hdfShpPhone.Value;
                                if (hdfShpZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_ZIP = hdfShpZip.Value;
                            }
                            else
                            {
                                custPK = saleOrderHeaderObj.SOH_CUSTOMER;
                                GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    saleOrderHeaderObj.SOH_SHIPPING_NAME = dtPageData.Rows[0]["CAD_NAME"].ToString();
                                    saleOrderHeaderObj.SOH_SHIPPING_ADDRESS = dtPageData.Rows[0]["CAD_ADDRESS"].ToString();
                                }
                                saleOrderHeaderObj.SOH_SHIPPING_TO = string.Empty;

                                saleOrderHeaderObj.SOH_SHIPPING_COUNTRY = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_EMAIL = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_FAX = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_MOBILE = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_PHONE = string.Empty;
                                saleOrderHeaderObj.SOH_SHIPPING_ZIP = string.Empty;
                            }

                            if (ddlNotifyParty.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY = ddlNotifyParty.SelectedValue;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME = ddlNotifyParty.SelectedItem.Text; //hdfNPName.Value;
                                //if (hdfNPName.Value.Trim() != string.Empty)
                                //    saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME = hdfNPName.Value;
                                if (hdfNPAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS = hdfNPAddress.Value;
                                if (hdfNPCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY = hdfNPCountry.Value.Trim();
                                if (hdfNPCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT = hdfNPCountryText.Value;
                                if (hdfNPEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL = hdfNPEmail.Value;
                                if (hdfNPFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX = hdfNPFax.Value;
                                if (hdfNPMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE = hdfNPMobile.Value;
                                if (hdfNPPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE = hdfNPPhone.Value;
                                if (hdfNPZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP = hdfNPZip.Value;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE = string.Empty;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP = string.Empty;
                            }

                            if (ddlAgent.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SHIP_AGENT = ddlAgent.SelectedValue;
                                if (hdfAgentName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_NAME = hdfAgentName.Value;
                                if (hdfAgentAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS = hdfAgentAddress.Value;
                                if (hdfAgentCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY = hdfAgentCountry.Value.Trim();
                                if (hdfAgentCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT = hdfAgentCountryText.Value;
                                if (hdfAgentEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL = hdfAgentEmail.Value;
                                if (hdfAgentFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_FAX = hdfAgentFax.Value;
                                if (hdfAgentMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE = hdfAgentMobile.Value;
                                if (hdfAgentPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE = hdfAgentPhone.Value;
                                if (hdfAgentZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP = hdfAgentZip.Value;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_SHIP_AGENT = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_NAME = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_FAX = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE = string.Empty;
                                saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP = string.Empty;
                            }

                            saleOrderHeaderObj.SOH_SHIP_INT_TO = HttpUtility.HtmlEncode(txtShppingIntimationto.Text);
                            saleOrderHeaderObj.SOH_FAX = HttpUtility.HtmlEncode(txtShppingIntimationtoFax.Text);
                            saleOrderHeaderObj.SOH_CONTAINER_SIZE = HttpUtility.HtmlEncode(txtContainerSize.Text);

                            //saleOrderHeaderObj.SOH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                            saleOrderHeaderObj.SOH_SHIPMENT_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                            //if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                            //{
                            //    saleOrderHeaderObj.SOH_DEL_TERM = ddlDeliveryTerms.SelectedValue;
                            //}
                            //else
                            //{
                            saleOrderHeaderObj.SOH_DEL_TERM = string.Empty;
                            //}

                            if (ddlShipmentTerms.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SHIPMENT_TERM = Convert.ToInt32(ddlShipmentTerms.SelectedValue);

                            saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                            if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_PAYMENT_TERM = string.Empty;
                            }

                            saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text);
                            if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SPECIAL_TERM = ddlSpecialCause.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_SPECIAL_TERM = string.Empty;
                            }

                            if (ddlBankDetails.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_BANK = ddlBankDetails.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_BANK = string.Empty;
                            }
                            saleOrderHeaderObj.SOH_NEED_ADV_PYMT = chkNeedAdvPay.Checked;
                            if (ddlAgentN.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_AGENT = ddlAgentN.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_AGENT = string.Empty;
                            }

                            if (ddlInspection.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_INSP_TYPE = ddlInspection.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_INSP_TYPE = string.Empty;
                            }
                            if (ddlExportDoc.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_EXP_DOC = ddlExportDoc.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_EXP_DOC = string.Empty;
                            }
                            saleOrderHeaderObj.SOH_PACKING_INSTRN = HttpUtility.HtmlEncode(txtPackingInstruction.Text);
                            if (ddlOriginofGoods.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_ORG_GOODS = ddlOriginofGoods.SelectedValue;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_ORG_GOODS = string.Empty;
                            }
                            saleOrderHeaderObj.SOH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);

                            //lblSubTotal = (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            //saleOrderHeaderObj.SOH_TOTAL_AMT = lblSubTotal == null ? 0 : string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? 0 : Convert.ToDecimal(lblSubTotal.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_TAX = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            saleOrderHeaderObj.SOH_NET_AMOUNT = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                            saleOrderHeaderObj.SOH_NET_AMOUNT_BC = saleOrderHeaderObj.SOH_NET_AMOUNT * saleOrderHeaderObj.SOH_CURRENCY_RATE;

                            saleOrderHeaderObj.SOH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            saleOrderHeaderObj.SOH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            saleOrderHeaderObj.ACTIVE = saleOrderHeaderObj.SOH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            saleOrderHeaderObj.USER_PK = currentUser.PKUser;
                            saleOrderHeaderObj.LAST_MOD_DT = LastModifiedTime;
                            saleOrderHeaderObj.APT_CODE = ApplicationType.SO;
                            saleOrderHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            saleOrderHeaderObj.WKF_FLAG = 0;
                            saleOrderHeaderObj.WKF_PROCESS = ProcessID;
                            saleOrderHeaderObj.SOH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            if (commonActions == ActionsEnum.SAVE)
                            {
                                saleOrderHeaderObj.WKF_FLAG = 0;
                            }
                            else if (commonActions == ActionsEnum.WRKFSUBMIT)
                            {
                                saleOrderHeaderObj.WKF_FLAG = 1;
                            }

                            //if amendment, then save Sale Contract History
                            if (Request.QueryString[QueryStrings.PageType] != null &&
                                Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                            {
                                saleOrderHeaderObj.SOH_IS_AMEND = 1;
                            }

                            //Attach Documents
                            saleOrderHeaderObj.FileList = SOUploadList;

                            if (ShowSCAdditionalPackDtls == 1)
                            {
                                if (ddlStandard.Items.Count > 0 && ddlStandard.SelectedValue != CommonConstants.SELECTVAL)
                                    saleOrderHeaderObj.SOH_STANDARD = ddlStandard.SelectedValue;
                                saleOrderHeaderObj.SOH_INNER = txtInner.Text;
                                saleOrderHeaderObj.SOH_CARTON = txtCarton.Text;
                                saleOrderHeaderObj.SOH_NETTING = chkNetting.Checked == true ? 1 : 0;
                                saleOrderHeaderObj.SOH_INSULATION = chkInsulation.Checked == true ? 1 : 0;
                                saleOrderHeaderObj.SOH_ADDL_PACKAGE = chkAddlPackaging.Checked == true ? 1 : 0;
                                saleOrderHeaderObj.SOH_PRE_SHIPMENT = chkPreShipment.Checked == true ? 1 : 0;
                                saleOrderHeaderObj.SOH_PROTEIN_TEST = chkProteinTest.Checked == true ? 1 : 0;
                            }

                            if (hdfCusPoNumber.Value == "1")
                            {
                                saleOrderHeaderObj.SOH_CUS_PO_FLAG = 0;
                            }
                            else
                            {
                                saleOrderHeaderObj.SOH_CUS_PO_FLAG = 1;
                            }

                            retObject = saleOrderHeaderObj;
                        }
                        break;
                    #endregion

                    #region Packing Materail Sale Contract Details
                    case ControlsEnum.PACKINGMATERAILSALEORDERDETAIL:
                        saleOrderDetailsObj = saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                        if (CurrSlNo != 0 && saleOrderDetailsObj != null)
                        {
                            saleOrderDetailsObj = saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                            //if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_CUST_ITEM == Convert.ToInt32(hdfPrdPackSpec.Value)).Count() == 0))
                            //if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_CUST_ITEM == Convert.ToInt32(hdfPrdPackSpec.Value)).Count() == 0))
                            //{
                            if (saleOrderDetailsObj != null)
                            {
                                //saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                //saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);


                                //saleOrderDetailsObj.CIM_PACKING_SPEC_NAME = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPrdPackSpec.Value);
                                //saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(txtSpecCode.Text);
                                saleOrderDetailsObj.SOD_ITEM_CATEGORY = hdfPrdCategory.Value;
                                saleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT = txtPrdCategory.Text;
                                saleOrderDetailsObj.SOD_PACK_TYPE_TEXT = txtPrdType.Text;
                                saleOrderDetailsObj.SOD_PACK_TYPE_VALUE = Convert.ToInt32(hdfPrdType.Value);


                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfPrdPackSpec.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtSpecCode.Text);
                                saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtspecqty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfspecuom.Value);
                                saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(hdfspecuomcode.Value);
                                //saleOrderDetailsObj.SOD_UOM_IS_PCS = Convert.ToInt32(HdfIsPcs.Value);
                                saleOrderDetailsObj.SOD_SALE_QTY = Math.Round(Convert.ToDouble(txtspecqty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                //saleOrderDetailsObj.SOD_SALE_UOM = Convert.ToInt32(hdfspecsaleuom.Value);
                                //saleOrderDetailsObj.SOD_SALE_UOM_CONV = (string.IsNullOrEmpty(hdfBrandUOMConversion.Value) || hdfBrandUOMConversion.Value == "0") ? 1 : Convert.ToDouble(hdfBrandUOMConversion.Value);
                                //saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                saleOrderDetailsObj.SOD_SALE_UOM_TEXT = HttpUtility.HtmlEncode(hdfspecuomcode.Value);
                                saleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtspecprice.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                saleOrderDetailsObj.SOD_DISCOUNT = Math.Round(string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                //if (GetGlobalResourceObject("ConfigurationsRes", "SCApproxCtnBoxPriceVisible").ToString() == "1")
                                //{
                                //    saleOrderDetailsObj.SOD_CARTON_RATE = txtctnRate.Text;
                                //}
                                //else
                                //{
                                //    saleOrderDetailsObj.SOD_CARTON_RATE = hdfCtnRate.Value;//txtctnRate.Text;
                                //}

                                saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfPrdPackSpecPK.Value);
                                //saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                saleOrderDetailsObj.SOD_ITEM_TEXT = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(hdfapstotalpcs.Value);
                                //saleOrderDetailsObj.APS_IB_PCS = Convert.ToDouble(hdfTotalPcsInBox.Value);
                                if (!string.IsNullOrEmpty(txtSpecReqByDate.Text))
                                    saleOrderDetailsObj.SOD_REQUIRED_BY = txtSpecReqByDate.Text;
                                saleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtspecamount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_NET_AMOUNT = Math.Round(Convert.ToDouble(txtspecamount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                saleOrderDetailsObj.SOD_TAX = Math.Round(string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);



                                saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtspecremark.Text);

                                if (rdbPackingSpec.Checked == true)
                                {
                                    saleOrderDetailsObj.SOD_IS_PACK_MAT = 1;
                                }
                                else if (rdbProduct.Checked == true)
                                {
                                    saleOrderDetailsObj.SOD_IS_PACK_MAT = 2;
                                }
                                //if (EnableItemTax > 0) //If line Item tax enabled need to save tax details
                                //    if (TempSaleOrderHeaderSession != null && TempSaleOrderHeaderSession.SaleContractDetails != null && TempSaleOrderHeaderSession.SaleContractDetails.Count > 0)
                                //        if (TempSaleOrderHeaderSession.SaleContractDetails.Count(s => s.SOD_SL_NO == CurrSlNo) > 0)
                                //            tempsaleOrderTaxHdrList = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(s => s.SOD_SL_NO == CurrSlNo).TaxDtl;
                                //if (tempsaleOrderTaxHdrList != null)
                                //{
                                //    saleOrderDetailsObj.TaxDtl = tempsaleOrderTaxHdrList;
                                //    tempsaleOrderTaxHdrList = null;
                                //}
                                //if (saleOrderDetailsObj.TaxDtl != null)
                                //{
                                //    if (saleOrderDetailsObj.TaxDtl.Count > 0)
                                //    {
                                //        foreach (var taxitem in saleOrderDetailsObj.TaxDtl)
                                //        { taxitem.SLT_SL_NO = saleOrderDetailsObj.SOD_SL_NO; }
                                //    }
                                //}

                                //if (ShowSCAdditionalPackDtls == 1)
                                //{
                                //    if (ddlstrappingColor.Items.Count > 0 && ddlstrappingColor.SelectedValue != CommonConstants.SELECTVAL)
                                //        saleOrderDetailsObj.SOD_STRAPPING_COLOUR = ddlstrappingColor.SelectedValue;
                                //    saleOrderDetailsObj.SOD_STRAPPING = chkStrapping.Checked == true ? "1" : "0";
                                //    saleOrderDetailsObj.SOD_LAYERING = chkLayering.Checked == true ? "1" : "0";
                                //    saleOrderDetailsObj.SOD_NO_LAYERS = txtNoofLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtNoofLayers.Text);
                                //    saleOrderDetailsObj.SOD_PIECES_LAYER = txtPcsLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtPcsLayers.Text);

                                //    if (txtMfgDate.Text != string.Empty)
                                //        saleOrderDetailsObj.SOD_MFG_DATE = (DateTime.Parse(txtMfgDate.Text)).ToString();
                                //    if (txtExpiryDate.Text != string.Empty)
                                //        saleOrderDetailsObj.SOD_EXP_DATE = (DateTime.Parse(txtExpiryDate.Text)).ToString();
                                //}
                            }
                            //// }
                            // else
                            // {
                            //     //IsBrand = true;
                            //     //Iscarton = true;
                            //     //GetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                            //     //if (dtPageData != null && dtPageData.Rows.Count > 0)
                            //     //{
                            //     //    tempsaleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                            //     //    tempsaleOrderTaxHdrList = SaleOrderHeaderSession.SaleContractDetails.LastOrDefault().TaxDtl;
                            //     //    saleOrderDetailsList.RemoveAt(saleOrderDetailsList.Count - 1);
                            //     //}
                            //     //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDeleteConfirming", "ShowDeleteConfirming();", true);
                            //     //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPackinMaterailCheckConfirming", "$(document).ready(function(){ShowPackinMaterailCheckConfirming();});", true);
                            //     //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                            //     //    + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                            //     //    + "','" + Resources.ErpRes.Information + "');", true);
                            //     return retObject;// Err_Same_Brand_Cont
                            // }

                        }
                        else
                        {
                            int m_slno = 1;
                            if (saleOrderDetailsList == null || saleOrderDetailsList.Count == 0)
                            {
                                saleOrderDetailsList = new List<SaleContractDetailsBO>();
                                m_slno = 1;
                            }
                            else
                            {
                                m_slno = saleOrderDetailsList.Max(itm => itm.SOD_SL_NO);
                                m_slno++;
                            }

                            //if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)).Count() == 0) || (IsBrand == true && (hdfIsBrandYes.Value == "1")))
                            if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_PACKING_SPEC == Convert.ToInt32(hdfPrdPackSpec.Value)).Count() == 0))
                            {
                                //IsBrand = false;
                                //Iscarton = false;
                                saleOrderDetailsObj = new SaleContractDetailsBO();
                                saleOrderDetailsObj.SOD_SL_NO = m_slno;
                                CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                                //saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);


                                //saleOrderDetailsObj.CIM_PACKING_SPEC_NAME = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPrdPackSpec.Value);
                                //saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(txtSpecCode.Text);
                                saleOrderDetailsObj.SOD_ITEM_CATEGORY = hdfPrdCategory.Value;
                                saleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT = txtPrdCategory.Text;

                                saleOrderDetailsObj.SOD_PACK_TYPE_TEXT = txtPrdType.Text;
                                saleOrderDetailsObj.SOD_PACK_TYPE_VALUE = Convert.ToInt32(hdfPrdType.Value);

                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtPrdPackSpec.Text);
                                //saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfPrdPackSpec.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtSpecCode.Text);
                                saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtspecqty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfspecuom.Value);
                                saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(hdfspecuomcode.Value);
                                //saleOrderDetailsObj.SOD_UOM_IS_PCS = Convert.ToInt32(HdfIsPcs.Value);
                                saleOrderDetailsObj.SOD_SALE_QTY = Math.Round(Convert.ToDouble(txtspecqty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                //saleOrderDetailsObj.SOD_SALE_UOM = Convert.ToInt32(hdfspecsaleuom.Value);
                                //saleOrderDetailsObj.SOD_SALE_UOM_CONV = (string.IsNullOrEmpty(hdfBrandUOMConversion.Value) || hdfBrandUOMConversion.Value == "0") ? 1 : Convert.ToDouble(hdfBrandUOMConversion.Value);
                                //saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                saleOrderDetailsObj.SOD_SALE_UOM_TEXT = HttpUtility.HtmlEncode(hdfspecuomcode.Value);
                                saleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtspecprice.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                saleOrderDetailsObj.SOD_DISCOUNT = Math.Round(string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                //if (GetGlobalResourceObject("ConfigurationsRes", "SCApproxCtnBoxPriceVisible").ToString() == "1")
                                //{
                                //    saleOrderDetailsObj.SOD_CARTON_RATE = txtctnRate.Text;
                                //}
                                //else
                                //{
                                //    saleOrderDetailsObj.SOD_CARTON_RATE = hdfCtnRate.Value;//txtctnRate.Text;
                                //}

                                saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfPrdPackSpecPK.Value);
                                //saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                //saleOrderDetailsObj.SOD_ITEM_TEXT = hdfProductName.Value;
                                saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(hdfapstotalpcs.Value);
                                //saleOrderDetailsObj.APS_IB_PCS = Convert.ToDouble(hdfTotalPcsInBox.Value);
                                if (!string.IsNullOrEmpty(txtSpecReqByDate.Text))
                                    saleOrderDetailsObj.SOD_REQUIRED_BY = txtSpecReqByDate.Text;
                                saleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtspecamount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_TAX = Math.Round(string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);



                                saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtspecremark.Text);

                                if (rdbPackingSpec.Checked == true)
                                {
                                    saleOrderDetailsObj.SOD_IS_PACK_MAT = 1;
                                }
                                else if (rdbProduct.Checked == true)
                                {
                                    saleOrderDetailsObj.SOD_IS_PACK_MAT = 2;
                                }
                                //if (EnableItemTax > 0) //If line Item tax enabled need to save tax details
                                //    if (TempSaleOrderHeaderSession != null && TempSaleOrderHeaderSession.SaleContractDetails != null && TempSaleOrderHeaderSession.SaleContractDetails.Count > 0)
                                //        if (TempSaleOrderHeaderSession.SaleContractDetails.Count(s => s.SOD_SL_NO == CurrSlNo) > 0)
                                //            tempsaleOrderTaxHdrList = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(s => s.SOD_SL_NO == CurrSlNo).TaxDtl;
                                //if (tempsaleOrderTaxHdrList != null)
                                //{
                                //    saleOrderDetailsObj.TaxDtl = tempsaleOrderTaxHdrList;
                                //    tempsaleOrderTaxHdrList = null;
                                //}
                                //if (saleOrderDetailsObj.TaxDtl != null)
                                //{
                                //    if (saleOrderDetailsObj.TaxDtl.Count > 0)
                                //    {
                                //        foreach (var taxitem in saleOrderDetailsObj.TaxDtl)
                                //        { taxitem.SLT_SL_NO = saleOrderDetailsObj.SOD_SL_NO; }
                                //    }
                                //}

                                //if (ShowSCAdditionalPackDtls == 1)
                                //{
                                //    if (ddlstrappingColor.Items.Count > 0 && ddlstrappingColor.SelectedValue != CommonConstants.SELECTVAL)
                                //        saleOrderDetailsObj.SOD_STRAPPING_COLOUR = ddlstrappingColor.SelectedValue;
                                //    saleOrderDetailsObj.SOD_STRAPPING = chkStrapping.Checked == true ? "1" : "0";
                                //    saleOrderDetailsObj.SOD_LAYERING = chkLayering.Checked == true ? "1" : "0";
                                //    saleOrderDetailsObj.SOD_NO_LAYERS = txtNoofLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtNoofLayers.Text);
                                //    saleOrderDetailsObj.SOD_PIECES_LAYER = txtPcsLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtPcsLayers.Text);

                                //    if (txtMfgDate.Text != string.Empty)
                                //        saleOrderDetailsObj.SOD_MFG_DATE = (DateTime.Parse(txtMfgDate.Text)).ToString();
                                //    if (txtExpiryDate.Text != string.Empty)
                                //        saleOrderDetailsObj.SOD_EXP_DATE = (DateTime.Parse(txtExpiryDate.Text)).ToString();
                                //}

                                saleOrderDetailsList.Add(saleOrderDetailsObj);
                            }
                            else
                            {
                                //IsBrand = true;
                                //Iscarton = true;
                                //GetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                                //if (dtPageData != null && dtPageData.Rows.Count > 0)
                                //{
                                //    tempsaleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                //    tempsaleOrderTaxHdrList = SaleOrderHeaderSession.SaleContractDetails.LastOrDefault().TaxDtl;
                                //    saleOrderDetailsList.RemoveAt(saleOrderDetailsList.Count - 1);
                                //}
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDeleteConfirming", "ShowDeleteConfirming();", true);
                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPackinMaterailCheckConfirming", "$(document).ready(function(){ShowPackinMaterailCheckConfirming();});", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                                //    + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                                //    + "','" + Resources.ErpRes.Information + "');", true);
                                return retObject;// Err_Same_Brand_Cont
                            }
                        }
                        retObject = saleOrderDetailsList;
                        break;
                    #endregion


                    #region Sale Contract Details
                    case ControlsEnum.SALEORDERDETAIL:
                        saleOrderDetailsObj = saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                        int Seqno = 1;
                        if (CurrSlNo != 0 && saleOrderDetailsObj != null)
                        {
                            saleOrderDetailsObj = saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                            if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)).Count() == 0) || (IsBrand == true && (hdfIsBrandYes.Value == "1")))
                            {
                                IsBrand = false;
                                Iscarton = false;
                                if (saleOrderDetailsObj != null)
                                {
                                    saleOrderDetailsObj.SOD_PK = Convert.ToInt32(hdfDetailPK.Value);
                                    saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                    saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtBrand.Text);
                                    saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);
                                    saleOrderDetailsObj.CIM_PACKING_SPEC_NAME = HttpUtility.HtmlEncode(txtPacking.Text);
                                    //saleOrderDetailsObj. = HttpUtility.HtmlEncode(txtPacking.Text);
                                    saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPackingSpec.Value);
                                    saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(hdfPackingText.Value);
                                    saleOrderDetailsObj.CBM = Convert.ToDouble(hdfCBM.Value);
                                    if (ddlArtWork.SelectedValue != CommonConstants.SELECTVAL)
                                    {
                                        saleOrderDetailsObj.SOD_ART_WORK = ddlArtWork.SelectedValue;

                                        saleOrderDetailsObj.PC_ART_WORK = lnkArtWorkPC.InnerText;
                                        saleOrderDetailsObj.PC_DOC_PATH = hdfArtWorkPC.Value;
                                        saleOrderDetailsObj.IB_ART_WORK = lnkArtWorkIB.InnerText;
                                        saleOrderDetailsObj.IB_DOC_PATH = hdfArtWorkIB.Value;
                                        saleOrderDetailsObj.IC_ART_WORK = lnkArtWorkIC.InnerText;
                                        saleOrderDetailsObj.IC_DOC_PATH = hdfArtWorkIC.Value;
                                        saleOrderDetailsObj.ZB_ART_WORK = lnkArtWorkZB.InnerText;
                                        saleOrderDetailsObj.ZB_DOC_PATH = hdfArtWorkZB.Value;
                                        saleOrderDetailsObj.MC_ART_WORK = lnkArtWorkMC.InnerText;
                                        saleOrderDetailsObj.MC_DOC_PATH = hdfArtWorkMC.Value;
                                        saleOrderDetailsObj.SC_ART_WORK = lnkArtWorkSC.InnerText;
                                        saleOrderDetailsObj.SC_DOC_PATH = hdfArtWorkSC.Value;

                                        artWorkDesc = string.Empty;
                                        artWorkDesc = saleOrderDetailsObj.PC_ART_WORK;
                                        artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IB_ART_WORK);
                                        artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IC_ART_WORK);
                                        artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.ZB_ART_WORK);
                                        artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.MC_ART_WORK);
                                        artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.SC_ART_WORK);
                                        saleOrderDetailsObj.SOD_ART_WORK_DESC = artWorkDesc;
                                    }
                                    else
                                    {
                                        saleOrderDetailsObj.SOD_ART_WORK = null;
                                        saleOrderDetailsObj.PC_ART_WORK = null;
                                        saleOrderDetailsObj.PC_DOC_PATH = null;
                                        saleOrderDetailsObj.IB_ART_WORK = null;
                                        saleOrderDetailsObj.IB_DOC_PATH = null;
                                        saleOrderDetailsObj.IC_ART_WORK = null;
                                        saleOrderDetailsObj.IC_DOC_PATH = null;
                                        saleOrderDetailsObj.ZB_ART_WORK = null;
                                        saleOrderDetailsObj.ZB_DOC_PATH = null;
                                        saleOrderDetailsObj.MC_ART_WORK = null;
                                        saleOrderDetailsObj.MC_DOC_PATH = null;
                                        saleOrderDetailsObj.SC_ART_WORK = null;
                                        saleOrderDetailsObj.SC_DOC_PATH = null;
                                        saleOrderDetailsObj.SOD_ART_WORK_DESC = null;
                                    }
                                    saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfUOM.Value);
                                    saleOrderDetailsObj.SOD_SALE_QTY = Math.Round(Convert.ToDouble(txtBrandQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    saleOrderDetailsObj.SOD_SALE_UOM = Convert.ToInt32(hdfBrandUOM.Value);
                                    saleOrderDetailsObj.SOD_SALE_UOM_CONV = (string.IsNullOrEmpty(hdfBrandUOMConversion.Value) || hdfBrandUOMConversion.Value == "0") ? 1 : Convert.ToDouble(hdfBrandUOMConversion.Value);
                                    saleOrderDetailsObj.SOD_SALE_UOM_TEXT = HttpUtility.HtmlEncode(txtBrandUOM.Text);
                                    saleOrderDetailsObj.SOD_UOM_IS_PCS = Convert.ToInt32(HdfIsPcs.Value);
                                    saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                    saleOrderDetailsObj.SOD_RATE = string.IsNullOrEmpty(txtRate.Text) ? 0 : Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                    saleOrderDetailsObj.SOD_DISCOUNT = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Math.Round(Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    if ((chkLotNoApplyAllItem.Checked) && (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "1") && (txtLotNo.Text != string.Empty))
                                    {
                                        int sno, temp = 0;
                                        if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                                        {
                                            saleOrderDetailsList.ForEach(sl =>
                                            {
                                                string lotno = sl.SOD_LOT_NO;
                                                if (sl.SOD_SL_NO != CurrSlNo)
                                                    if (lotno.Contains("-"))
                                                    {
                                                        var lastno = lotno.Substring(lotno.LastIndexOf('-') + 1);
                                                        sno = Convert.ToInt32(lastno);
                                                        if (temp < Convert.ToInt32(sno))
                                                        {
                                                            Seqno = sno + 1;
                                                            temp = sno;
                                                        }
                                                        if (sno == 1 && Seqno == 1)
                                                        {
                                                            Seqno++;
                                                        }
                                                    }
                                                    else
                                                    {

                                                    }
                                            });

                                        }

                                        saleOrderDetailsObj.SOD_LOT_NO = txtLotNo.Visible ? HttpUtility.HtmlEncode(txtLotNo.Text) + "-" + Seqno : string.Empty;
                                    }
                                    else
                                    {
                                        saleOrderDetailsObj.SOD_LOT_NO = txtLotNo.Visible ? HttpUtility.HtmlEncode(txtLotNo.Text) : string.Empty;
                                    }
                                    if (GetGlobalResourceObject("ConfigurationsRes", "SCApproxCtnBoxPriceVisible").ToString() == "1")
                                    {
                                        saleOrderDetailsObj.SOD_CARTON_RATE = txtctnRate.Text;
                                    }
                                    else
                                    {
                                        saleOrderDetailsObj.SOD_CARTON_RATE = hdfCtnRate.Value;//txtctnRate.Text;
                                    }

                                    saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfProduct.Value);
                                    saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                    saleOrderDetailsObj.SOD_ITEM_TEXT = hdfProductName.Value;
                                    saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(txtTotalPiecesCtn.Text);
                                    saleOrderDetailsObj.APS_IB_PCS = Convert.ToDouble(hdfTotalPcsInBox.Value);
                                    if (!string.IsNullOrEmpty(txtReqByDate.Text))
                                        saleOrderDetailsObj.SOD_REQUIRED_BY = txtReqByDate.Text;
                                    saleOrderDetailsObj.SOD_AMOUNT = string.IsNullOrEmpty(txtAmount.Text) ? 0 : Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    saleOrderDetailsObj.SOD_TAX = string.IsNullOrEmpty(txtTax.Text) ? 0 : Math.Round(Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    saleOrderDetailsObj.SOD_LOT_SIZE = txtLotSize.Visible ? HttpUtility.HtmlEncode(txtLotSize.Text) : string.Empty;
                                    saleOrderDetailsObj.SOD_CASE_MARK = HttpUtility.HtmlEncode(txtCaseMark.Text);

                                    saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                    saleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);
                                    saleOrderDetailsObj.SOD_HSN_CODE = string.IsNullOrEmpty(hdfHSNNo.Value) ? 0 : Convert.ToInt32(hdfHSNNo.Value);
                                    saleOrderDetailsObj.SOD_HSN_CODE_TEXT = string.IsNullOrEmpty(txtHSNNo.Text) ? "" : txtHSNNo.Text;
                                    if(saleOrderDetailsObj.SOD_HSN_CODE_TEXT.Trim() == "Select/Type")
                                    {
                                        saleOrderDetailsObj.SOD_HSN_CODE_TEXT = "";
                                    }
                                   
                                    if (EnableItemTax > 0) //If line Item tax enabled need to save tax details
                                    {
                                        if (TempSaleOrderHeaderSession != null && TempSaleOrderHeaderSession.SaleContractDetails != null && TempSaleOrderHeaderSession.SaleContractDetails.Count > 0)
                                            if (TempSaleOrderHeaderSession.SaleContractDetails.Count(s => s.SOD_SL_NO == CurrSlNo) > 0)
                                                tempsaleOrderTaxHdrList = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(s => s.SOD_SL_NO == CurrSlNo).TaxDtl;

                                        if (tempsaleOrderTaxHdrList != null)
                                        {
                                            saleOrderDetailsObj.TaxDtl = tempsaleOrderTaxHdrList;
                                            tempsaleOrderTaxHdrList = null;
                                        }
                                    }
                                    if (saleOrderDetailsObj.TaxDtl != null)
                                    {
                                        if (saleOrderDetailsObj.TaxDtl.Count > 0)
                                        {
                                            foreach (var taxitem in saleOrderDetailsObj.TaxDtl)
                                            { taxitem.SLT_SL_NO = saleOrderDetailsObj.SOD_SL_NO; }
                                        }
                                    }
                                    if (chkLotNoApplyAllItem.Checked)  //Generate auto and custom lot number for all brands 
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                if (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "0")
                                                    saleOrderDetailsList.ForEach(lst => lst.SOD_LOT_NO = saleOrderDetailsObj.SOD_LOT_NO);
                                            }
                                        }
                                    }
                                  
                                    if (chkShipdateUpdation.Checked && ShipdateUpdation)  //update ship date for all brands 
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_REQUIRED_BY = saleOrderDetailsObj.SOD_REQUIRED_BY);
                                            }
                                        }
                                    }
                                    if (chkAddtlRemarkToAll.Checked)  //update additional remark for all brands 
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS2 = saleOrderDetailsObj.SOD_REMARKS2);
                                            }
                                        }
                                    }

                                    if (chkRemarkToAll.Checked)  //update remark for all brands 
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS = saleOrderDetailsObj.SOD_REMARKS);
                                            }
                                        }
                                    }
                                     if (ChkCaseMarkApplyAllItem.Checked&& ShowCaseMarkCheckAllItem==1)
                                        {
                                            if (saleOrderDetailsList != null)
                                            {
                                                if (saleOrderDetailsList.Count > 0)
                                                    saleOrderDetailsList.ForEach(lst => lst.SOD_CASE_MARK = saleOrderDetailsObj.SOD_CASE_MARK);
                                            }
                                        }
                                 
                                    if (ShowSCAdditionalPackDtls == 1)
                                    {
                                        //if (ChkMfgDateApplyAllItem.Checked)
                                        //{
                                        //    if (saleOrderDetailsList != null)
                                        //    {
                                        //        if (saleOrderDetailsList.Count > 0)
                                        //        {
                                        //            saleOrderDetailsList.ForEach(lst => lst.SOD_MFG_DATE = saleOrderDetailsObj.SOD_MFG_DATE);
                                        //        }
                                        //    }
                                        //}

                                        //if (ChkExpDateApplyAllItem.Checked)
                                        //{
                                        //    if (saleOrderDetailsList != null)
                                        //    {
                                        //        if (saleOrderDetailsList.Count > 0)
                                        //        {
                                        //            saleOrderDetailsList.ForEach(lst => lst.SOD_EXP_DATE = saleOrderDetailsObj.SOD_EXP_DATE);
                                        //        }
                                        //    }
                                        //}
                                        if (ddlstrappingColor.Items.Count > 0 && ddlstrappingColor.SelectedValue != CommonConstants.SELECTVAL)
                                            saleOrderDetailsObj.SOD_STRAPPING_COLOUR = ddlstrappingColor.SelectedValue;
                                        saleOrderDetailsObj.SOD_STRAPPING = chkStrapping.Checked == true ? "1" : "0";
                                        saleOrderDetailsObj.SOD_LAYERING = chkLayering.Checked == true ? "1" : "0";
                                        saleOrderDetailsObj.SOD_NO_LAYERS = txtNoofLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtNoofLayers.Text);
                                        saleOrderDetailsObj.SOD_PIECES_LAYER = txtPcsLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtPcsLayers.Text);

                                        if (hdfInitMfgDate.Value == "1")
                                        {
                                            if (txtMfgDate.Text != string.Empty)
                                            {
                                                saleOrderDetailsObj.SOD_MFG_DATE = (DateTime.Parse(txtMfgDate.Text)).ToString();
                                            }
                                            else
                                            {
                                                saleOrderDetailsObj.SOD_MFG_DATE = "";
                                            }
                                            if (txtExpiryDate.Text != string.Empty)
                                            {
                                                saleOrderDetailsObj.SOD_EXP_DATE = (DateTime.Parse(txtExpiryDate.Text)).ToString();
                                            }
                                            else
                                            {
                                                saleOrderDetailsObj.SOD_EXP_DATE = "";
                                            }
                                        }
                                        else
                                        {
                                            if (txtMfgDate.Text != string.Empty)
                                            {
                                                saleOrderDetailsObj.SOD_MFG_DATE = txtMfgDate.Text.ToString();
                                            }
                                            else
                                            {
                                                saleOrderDetailsObj.SOD_MFG_DATE = "";
                                            }
                                            if (txtExpiryDate.Text != string.Empty)
                                            {
                                                saleOrderDetailsObj.SOD_EXP_DATE = txtExpiryDate.Text.ToString();
                                            }
                                            else
                                            {
                                                saleOrderDetailsObj.SOD_EXP_DATE = "";
                                            }
                                        }
                                        if (ChkMfgDateApplyAllItem.Checked)
                                        {
                                            if (saleOrderDetailsList != null)
                                            {
                                                if (saleOrderDetailsList.Count > 0)
                                                {
                                                    saleOrderDetailsList.ForEach(lst => lst.SOD_MFG_DATE = saleOrderDetailsObj.SOD_MFG_DATE);
                                                }
                                            }
                                        }

                                        if (ChkExpDateApplyAllItem.Checked)
                                        {
                                            if (saleOrderDetailsList != null)
                                            {
                                                if (saleOrderDetailsList.Count > 0)
                                                {
                                                    saleOrderDetailsList.ForEach(lst => lst.SOD_EXP_DATE = saleOrderDetailsObj.SOD_EXP_DATE);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                IsBrand = true;
                                Iscarton = true;
                                GetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    tempsaleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                    tempsaleOrderTaxHdrList = SaleOrderHeaderSession.SaleContractDetails.LastOrDefault().TaxDtl;
                                    saleOrderDetailsList.RemoveAt(saleOrderDetailsList.Count - 1);
                                }
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDeleteConfirming", "ShowDeleteConfirming();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowBrandCheckConfirming", "$(document).ready(function(){ShowBrandCheckConfirming();});", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                                //    + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                                //    + "','" + Resources.ErpRes.Information + "');", true);
                                return retObject;// Err_Same_Brand_Cont
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (saleOrderDetailsList == null || saleOrderDetailsList.Count == 0)
                            {
                                saleOrderDetailsList = new List<SaleContractDetailsBO>();
                                slno = 1;
                            }
                            else
                            {
                                slno = saleOrderDetailsList.Max(itm => itm.SOD_SL_NO);
                                slno++;
                            }

                            if ((saleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)).Count() == 0) || (IsBrand == true && (hdfIsBrandYes.Value == "1")))
                            {
                                IsBrand = false;
                                Iscarton = false;
                                saleOrderDetailsObj = new SaleContractDetailsBO();
                                saleOrderDetailsObj.SOD_SL_NO = slno;
                                CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                                saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtBrand.Text);
                                saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);
                                saleOrderDetailsObj.CIM_PACKING_SPEC_NAME = HttpUtility.HtmlEncode(txtPacking.Text);
                                saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPackingSpec.Value);
                                saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(hdfPackingText.Value);
                                saleOrderDetailsObj.CBM = Convert.ToDouble(hdfCBM.Value);
                                if (ddlArtWork.SelectedValue != CommonConstants.SELECTVAL)
                                {
                                    saleOrderDetailsObj.SOD_ART_WORK = ddlArtWork.SelectedValue;

                                    saleOrderDetailsObj.PC_ART_WORK = lnkArtWorkPC.InnerText;
                                    saleOrderDetailsObj.PC_DOC_PATH = hdfArtWorkPC.Value;
                                    saleOrderDetailsObj.IB_ART_WORK = lnkArtWorkIB.InnerText;
                                    saleOrderDetailsObj.IB_DOC_PATH = hdfArtWorkIB.Value;
                                    saleOrderDetailsObj.IC_ART_WORK = lnkArtWorkIC.InnerText;
                                    saleOrderDetailsObj.IC_DOC_PATH = hdfArtWorkIC.Value;
                                    saleOrderDetailsObj.ZB_ART_WORK = lnkArtWorkZB.InnerText;
                                    saleOrderDetailsObj.ZB_DOC_PATH = hdfArtWorkZB.Value;
                                    saleOrderDetailsObj.MC_ART_WORK = lnkArtWorkMC.InnerText;
                                    saleOrderDetailsObj.MC_DOC_PATH = hdfArtWorkMC.Value;
                                    saleOrderDetailsObj.SC_ART_WORK = lnkArtWorkSC.InnerText;
                                    saleOrderDetailsObj.SC_DOC_PATH = hdfArtWorkSC.Value;

                                    artWorkDesc = string.Empty;
                                    artWorkDesc = saleOrderDetailsObj.PC_ART_WORK;
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.ZB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.MC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.SC_ART_WORK);
                                    saleOrderDetailsObj.SOD_ART_WORK_DESC = artWorkDesc;
                                }
                                saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfUOM.Value);
                                saleOrderDetailsObj.SOD_UOM_IS_PCS = Convert.ToInt32(HdfIsPcs.Value);
                                saleOrderDetailsObj.SOD_SALE_QTY = Math.Round(Convert.ToDouble(txtBrandQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_SALE_UOM = Convert.ToInt32(hdfBrandUOM.Value);
                                saleOrderDetailsObj.SOD_SALE_UOM_CONV = (string.IsNullOrEmpty(hdfBrandUOMConversion.Value) || hdfBrandUOMConversion.Value == "0") ? 1 : Convert.ToDouble(hdfBrandUOMConversion.Value);
                                saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                saleOrderDetailsObj.SOD_SALE_UOM_TEXT = HttpUtility.HtmlEncode(txtBrandUOM.Text);
                                saleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                saleOrderDetailsObj.SOD_DISCOUNT = Math.Round(string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                if ((chkLotNoApplyAllItem.Checked) && (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "1") && (txtLotNo.Text != string.Empty))
                                {
                                    if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                                    {
                                        //string lotno = saleOrderDetailsList.Max(itm => itm.SOD_LOT_NO);
                                        //if (lotno.Contains("-"))
                                        //{
                                        //    var lastno = lotno.Substring(lotno.LastIndexOf('-') + 1);
                                        //    //string[] result = lotno.Split('-');
                                        //    //lotno = result[1];
                                        //    Seqno = Convert.ToInt32(lastno) + 1;
                                        //}
                                        int sno, temp = 0;
                                        if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                                        {
                                            saleOrderDetailsList.ForEach(sl =>
                                            {
                                                string lotno = sl.SOD_LOT_NO;
                                                if (sl.SOD_SL_NO != CurrSlNo)
                                                    if (lotno.Contains("-"))
                                                    {
                                                        var lastno = lotno.Substring(lotno.LastIndexOf('-') + 1);
                                                        sno = Convert.ToInt32(lastno);
                                                        if (temp < Convert.ToInt32(sno))
                                                        {
                                                            Seqno = sno + 1;
                                                            temp = sno;
                                                        }
                                                        if (sno == 1 && Seqno == 1)
                                                        {
                                                            Seqno++;
                                                        }
                                                    }
                                                    else
                                                    {

                                                    }
                                            });

                                        }

                                    }
                                    saleOrderDetailsObj.SOD_LOT_NO = txtLotNo.Visible ? HttpUtility.HtmlEncode(txtLotNo.Text) + "-" + Seqno : string.Empty;
                                }
                                else
                                {
                                    saleOrderDetailsObj.SOD_LOT_NO = txtLotNo.Visible ? HttpUtility.HtmlEncode(txtLotNo.Text) : string.Empty;
                                }
                                if (GetGlobalResourceObject("ConfigurationsRes", "SCApproxCtnBoxPriceVisible").ToString() == "1")
                                {
                                    saleOrderDetailsObj.SOD_CARTON_RATE = txtctnRate.Text;
                                }
                                else
                                {
                                    saleOrderDetailsObj.SOD_CARTON_RATE = hdfCtnRate.Value;//txtctnRate.Text;
                                }

                                saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfProduct.Value);
                                saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                saleOrderDetailsObj.SOD_ITEM_TEXT = hdfProductName.Value;
                                saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(txtTotalPiecesCtn.Text);
                                saleOrderDetailsObj.APS_IB_PCS = Convert.ToDouble(hdfTotalPcsInBox.Value);
                                if (!string.IsNullOrEmpty(txtReqByDate.Text))
                                    saleOrderDetailsObj.SOD_REQUIRED_BY = txtReqByDate.Text;
                                saleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_TAX = Math.Round(string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_LOT_SIZE = txtLotSize.Visible ? HttpUtility.HtmlEncode(txtLotSize.Text) : string.Empty;
                                saleOrderDetailsObj.SOD_CASE_MARK = HttpUtility.HtmlEncode(txtCaseMark.Text);
                                saleOrderDetailsObj.SOD_MFG_DATE = txtMfgDate.Text.ToString(); 
                                saleOrderDetailsObj.SOD_EXP_DATE = txtExpiryDate.Text.ToString();
                                saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                saleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);
                                saleOrderDetailsObj.SOD_HSN_CODE= string.IsNullOrEmpty(hdfHSNNo.Value) ? 0 : Convert.ToInt32(hdfHSNNo.Value);
                                saleOrderDetailsObj.SOD_HSN_CODE_TEXT = string.IsNullOrEmpty(txtHSNNo.Text) ? "" : txtHSNNo.Text;
                                if (saleOrderDetailsObj.SOD_HSN_CODE_TEXT.Trim() == "Select/Type")
                                {
                                    saleOrderDetailsObj.SOD_HSN_CODE_TEXT = "";
                                }
                                if (EnableItemTax > 0) //If line Item tax enabled need to save tax details
                                    if (TempSaleOrderHeaderSession != null && TempSaleOrderHeaderSession.SaleContractDetails != null && TempSaleOrderHeaderSession.SaleContractDetails.Count > 0)
                                        if (TempSaleOrderHeaderSession.SaleContractDetails.Count(s => s.SOD_SL_NO == CurrSlNo) > 0)
                                            tempsaleOrderTaxHdrList = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(s => s.SOD_SL_NO == CurrSlNo).TaxDtl;
                                if (tempsaleOrderTaxHdrList != null)
                                {
                                    saleOrderDetailsObj.TaxDtl = tempsaleOrderTaxHdrList;
                                    tempsaleOrderTaxHdrList = null;
                                }
                                if (saleOrderDetailsObj.TaxDtl != null)
                                {
                                    if (saleOrderDetailsObj.TaxDtl.Count > 0)
                                    {
                                        foreach (var taxitem in saleOrderDetailsObj.TaxDtl)
                                        { taxitem.SLT_SL_NO = saleOrderDetailsObj.SOD_SL_NO; }
                                    }
                                }
                                if (chkLotNoApplyAllItem.Checked)  //Generate auto and custom lot number for all brands 
                                {
                                    if (saleOrderDetailsList != null)
                                    {
                                        if (saleOrderDetailsList.Count > 0)
                                        {
                                            if (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "0")
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_LOT_NO = saleOrderDetailsObj.SOD_LOT_NO);

                                        }
                                    }
                                }

                                if (chkShipdateUpdation.Checked && ShipdateUpdation)  //update ship date for all brands 
                                {
                                    if (saleOrderDetailsList != null)
                                    {
                                        if (saleOrderDetailsList.Count > 0)
                                        {
                                            saleOrderDetailsList.ForEach(lst => lst.SOD_REQUIRED_BY = saleOrderDetailsObj.SOD_REQUIRED_BY);
                                        }
                                    }
                                }
                                if (chkAddtlRemarkToAll.Checked)  //update additional remarks for all brands 
                                {
                                    if (saleOrderDetailsList != null)
                                    {
                                        if (saleOrderDetailsList.Count > 0)
                                        {
                                            saleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS2 = saleOrderDetailsObj.SOD_REMARKS2);
                                        }
                                    }
                                }
                                if (chkRemarkToAll.Checked)  //update remark for all brands 
                                {
                                    if (saleOrderDetailsList != null)
                                    {
                                        if (saleOrderDetailsList.Count > 0)
                                        {
                                            saleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS = saleOrderDetailsObj.SOD_REMARKS);
                                        }
                                    }
                                }
                                if (ChkCaseMarkApplyAllItem.Checked && ShowCaseMarkCheckAllItem == 1)
                                {
                                    if (saleOrderDetailsList != null)
                                    {
                                        if (saleOrderDetailsList.Count > 0)
                                            saleOrderDetailsList.ForEach(lst => lst.SOD_CASE_MARK = saleOrderDetailsObj.SOD_CASE_MARK);
                                    }
                                }

                                if (ShowSCAdditionalPackDtls == 1)
                                {
                                    if (ChkMfgDateApplyAllItem.Checked)
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_MFG_DATE = saleOrderDetailsObj.SOD_MFG_DATE);
                                            }
                                        }
                                    }

                                    if (ChkExpDateApplyAllItem.Checked)
                                    {
                                        if (saleOrderDetailsList != null)
                                        {
                                            if (saleOrderDetailsList.Count > 0)
                                            {
                                                saleOrderDetailsList.ForEach(lst => lst.SOD_EXP_DATE = saleOrderDetailsObj.SOD_EXP_DATE);
                                            }
                                        }
                                    }
                                    if (ddlstrappingColor.Items.Count > 0 && ddlstrappingColor.SelectedValue != CommonConstants.SELECTVAL)
                                        saleOrderDetailsObj.SOD_STRAPPING_COLOUR = ddlstrappingColor.SelectedValue;
                                    saleOrderDetailsObj.SOD_STRAPPING = chkStrapping.Checked == true ? "1" : "0";
                                    saleOrderDetailsObj.SOD_LAYERING = chkLayering.Checked == true ? "1" : "0";
                                    saleOrderDetailsObj.SOD_NO_LAYERS = txtNoofLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtNoofLayers.Text);
                                    saleOrderDetailsObj.SOD_PIECES_LAYER = txtPcsLayers.Text == string.Empty ? 0 : Convert.ToDouble(txtPcsLayers.Text);

                                    if (hdfInitMfgDate.Value == "1")
                                    {
                                        if (txtMfgDate.Text != string.Empty)
                                        {
                                            saleOrderDetailsObj.SOD_MFG_DATE = (DateTime.Parse(txtMfgDate.Text)).ToString();
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj.SOD_MFG_DATE = "";
                                        }
                                        if (txtExpiryDate.Text != string.Empty)
                                        {
                                            saleOrderDetailsObj.SOD_EXP_DATE = (DateTime.Parse(txtExpiryDate.Text)).ToString();
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj.SOD_EXP_DATE = "";
                                        }
                                    }
                                    else
                                    {
                                        if (txtMfgDate.Text != string.Empty)
                                        {
                                            saleOrderDetailsObj.SOD_MFG_DATE = txtMfgDate.Text.ToString();
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj.SOD_MFG_DATE = "";
                                        }
                                        if (txtExpiryDate.Text != string.Empty)
                                        {
                                            saleOrderDetailsObj.SOD_EXP_DATE = txtExpiryDate.Text.ToString();
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj.SOD_EXP_DATE = "";
                                        }
                                    }
                                }

                                saleOrderDetailsList.Add(saleOrderDetailsObj);
                            }
                            else
                            {
                                IsBrand = true;
                                Iscarton = true;
                                GetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    tempsaleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                    tempsaleOrderTaxHdrList = SaleOrderHeaderSession.SaleContractDetails.LastOrDefault().TaxDtl;
                                    saleOrderDetailsList.RemoveAt(saleOrderDetailsList.Count - 1);
                                }
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDeleteConfirming", "ShowDeleteConfirming();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowBrandCheckConfirming", "$(document).ready(function(){ShowBrandCheckConfirming();});", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                                //    + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                                //    + "','" + Resources.ErpRes.Information + "');", true);
                                return retObject;// Err_Same_Brand_Cont

                            }
                        }

                        retObject = saleOrderDetailsList;
                        break;
                    #endregion

                    #region AGENT
                    case ControlsEnum.AGENT:
                        ProductDtlBOObj = new ProductDtlBO();
                        ProductDtls ProductDtlsObj;
                        List<ProductDtls> ProductDtlsList;
                        ProductDtlsList = new List<ProductDtls>();
                        if (grdItemDetails.Rows.Count > 0 && grdItemDetails != null)
                        {
                            foreach (GridViewRow grdrow in grdItemDetails.Rows)
                            {
                                HiddenField hdfCusItemPK;
                                ProductDtlsObj = new ProductDtls();

                                hdfCusItemPK = (HiddenField)grdrow.FindControl("hdfCusItemPK");

                                ProductDtlsObj.CIM_PK = Convert.ToInt32(hdfCusItemPK.Value);
                                ProductDtlsList.Add(ProductDtlsObj);
                            }
                        }
                        ProductDtlBOObj.ItemsList = ProductDtlsList;
                        return ProductDtlBOObj;
                        break;
                    #endregion
                    #region SALESCOST
                    case ControlsEnum.SALESCOST:
                        List<SalesCostPatams> lstSParams = new List<SalesCostPatams>();
                        SalesCostPatams objSParams = new SalesCostPatams();
                        objSParams.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                        objSParams.SOD_BRAND_QTY = txtBrandQuantity.Text != string.Empty ? Convert.ToDouble(txtBrandQuantity.Text) : 0;
                        objSParams.SOD_QTY = txtQty.Text != string.Empty ? Convert.ToDouble(txtQty.Text) : 0;
                        objSParams.SOD_AMOUNT = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0;
                        objSParams.SOD_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objSParams.SOD_SALE_RATE = !string.IsNullOrEmpty(txtRate.Text) ? Convert.ToDouble(txtRate.Text) : 0;
                        objSParams.SOD_BRAND_UOM = !string.IsNullOrEmpty(hdfBrandUOM.Value) ? Convert.ToInt32(hdfBrandUOM.Value) : 0;
                        objSParams.SOD_BRAND_UOM_TEXT = txtBrandUOM.Text;
                        lstSParams.Add(objSParams);
                        retObject = lstSParams;
                        break;
                    #endregion
                    #region ALL SALES COST
                    case ControlsEnum.ALLSALESCOST:
                        List<SalesCostPatams> lstAParams = new List<SalesCostPatams>();
                        foreach (GridViewRow grdRow in grdItemDetails.Rows)
                        {
                            HiddenField hdfCusItemPK = ((HiddenField)grdRow.FindControl("hdfCusItemPK"));
                            Label lblQuantity = ((Label)grdRow.FindControl("lblQuantity"));
                            HiddenField hdfUoM = ((HiddenField)grdRow.FindControl("hdfUoM"));
                            Label lblItemAmount = ((Label)grdRow.FindControl("lblItemAmount"));
                            Label lblItemRate = ((Label)grdRow.FindControl("lblItemRate"));
                            Label lblBrandQuantity = ((Label)grdRow.FindControl("lblBrandQuantity"));
                            Label lblUOM = ((Label)grdRow.FindControl("lblUOM"));

                            SalesCostPatams objAParams = new SalesCostPatams();
                            objAParams.SOD_CUST_ITEM = !string.IsNullOrEmpty(hdfCusItemPK.Value) ? Convert.ToInt32(hdfCusItemPK.Value) : 0;
                            objAParams.SOD_BRAND_QTY = lblBrandQuantity.Text != string.Empty ? Convert.ToDouble(lblBrandQuantity.Text) : 0;
                            objAParams.SOD_QTY = lblQuantity.Text != string.Empty ? Convert.ToDouble(lblQuantity.Text.Replace(",", "")) : 0;
                            objAParams.SOD_AMOUNT = lblItemAmount.Text != string.Empty ? Convert.ToDouble(lblItemAmount.Text.Replace(",", "")) : 0;
                            objAParams.SOD_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                            objAParams.SOD_SALE_RATE = !string.IsNullOrEmpty(lblItemRate.Text) ? Convert.ToDouble(lblItemRate.Text) : 0;
                            objAParams.SOD_BRAND_UOM = !string.IsNullOrEmpty(hdfUoM.Value) ? Convert.ToInt32(hdfUoM.Value) : 0;
                            objAParams.SOD_BRAND_UOM_TEXT = lblUOM.Text;
                            lstAParams.Add(objAParams);
                        }

                        retObject = lstAParams;
                        break;
                        #endregion

                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
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
            string addr;
            DateDefaultEnum defaultDate;
            int defaultAddMonths;
            try
            {
                switch (controlType)
                {
                    #region Contract
                    case ControlsEnum.SALEORDERHEADER:
                    case ControlsEnum.COPYCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            if (controlType == ControlsEnum.COPYCONTRACT)
                            {
                                hdfIsCopy.Value = "1";
                            }
                            custPK = saleOrderHeaderObj.SOH_CUSTOMER;
                            hdfCustomer.Value = saleOrderHeaderObj.SOH_CUSTOMER.ToString();
                            txtCustomer.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_TEXT);

                            hdfVersion.Value = saleOrderHeaderObj.SOH_VERSION.ToString();
                            hdfMailAttachmentName.Value = saleOrderHeaderObj.SOH_ADD_NAME;
                            txtBuyerAddress.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS);//addr;

                            hdfCusAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS);
                            hdfCusCountry.Value = saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY;
                            hdfCusCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT);
                            hdfCusZip.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ZIP);
                            hdfCusPhone.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_PHONE);
                            hdfCusMobile.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_MOBILE);
                            hdfCusFax.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_FAX);
                            hdfCusEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_EMAIL);

                            if (controlType == ControlsEnum.SALEORDERHEADER)
                            {
                                CurrPK = saleOrderHeaderObj.SOH_PK;
                                if (CurrQuotationPK > 0)
                                {
                                    lblSaleOrderNo.Text = Resources.Messages.DocGenerationNew;
                                }
                                else
                                {
                                    lblSaleOrderNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_NO) ?
                                          ERP.Utilities.CommonFunctions.GetShortString(Resources.Messages.DocGenerationNew + " - " + HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REFERENCE), 32)
                                        : saleOrderHeaderObj.SOH_NO;
                                    lblSaleOrderNo.ToolTip = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_NO) ?
                                          Resources.Messages.DocGenerationNew + " - " + HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REFERENCE)
                                        : saleOrderHeaderObj.SOH_NO;

                                    //lblSaleOrderNo.CssClass = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_NO) ? "" : "medium";
                                }

                                txtSaleOrderDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_DATE) ?
                                    DateTime.Now.ToString(Resources.ErpRes.DateFormat) : saleOrderHeaderObj.SOH_DATE;

                                //lblCustPOSCNo.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REFERENCE);
                                txtPONo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REFERENCE) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REFERENCE);
                                txtPODate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REFERENCE_DATE) ? string.Empty : saleOrderHeaderObj.SOH_REFERENCE_DATE;

                                hdfIsCancelled.Value = saleOrderHeaderObj.SOH_DEL_STATUS.ToString();

                                if (Request.QueryString[QueryStrings.PageType] != null && Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                                {
                                    divAmendDate.Visible = true;
                                    txtAmendDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_AMEND_DATE) ? DateTime.Now.ToString(Resources.ErpRes.DateFormat) : saleOrderHeaderObj.SOH_AMEND_DATE;
                                }
                                else
                                {
                                    divAmendDate.Visible = false;
                                    txtAmendDate.Text = string.Empty;

                                    if (saleOrderHeaderObj.SOH_VERSION > 1)
                                    {
                                        if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_AMEND_DATE))
                                        {
                                            divAmendDate.Visible = true;
                                            txtAmendDate.Text = saleOrderHeaderObj.SOH_AMEND_DATE;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BOOKING_DATE))
                                    txtBookingDate.Text = saleOrderHeaderObj.SOH_BOOKING_DATE;
                                else
                                {
                                    defaultDate = DateDefaultEnum.CurrentDate;
                                    defaultAddMonths = 0;
                                    Enum.TryParse(GetLocalResourceObject("DefaultDayBooking").ToString(), out defaultDate);
                                    int.TryParse(GetLocalResourceObject("DefaultAddMonthsBooking").ToString(), out defaultAddMonths);
                                    txtBookingDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ? defaultAddMonths + 1 : defaultAddMonths)
                                        .AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                                        defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
                                }
                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DELIVERY_DATE))
                                    txtShipmentDate.Text = saleOrderHeaderObj.SOH_DELIVERY_DATE;
                                else
                                {
                                    defaultDate = DateDefaultEnum.CurrentDate;
                                    defaultAddMonths = 0;
                                    Enum.TryParse(GetLocalResourceObject("DefaultDayReqdBy").ToString(), out defaultDate);
                                    int.TryParse(GetLocalResourceObject("DefaultAddMonthsReqdBy").ToString(), out defaultAddMonths);
                                    txtShipmentDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ?
                                        defaultAddMonths + 1 : defaultAddMonths).AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                                        defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
                                }
                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPMENT_DESC))
                                    txtShipmentDateText.Text = saleOrderHeaderObj.SOH_SHIPMENT_DESC;
                            }

                            soTypePK = saleOrderHeaderObj.SOH_TYPE;
                            GetFieldValues(ControlsEnum.SOTYPE);
                            SetFieldValues(ControlsEnum.SOTYPE);

                            soSubTypePK = saleOrderHeaderObj.SOH_SUB_TYPE;
                            GetFieldValues(ControlsEnum.SOSUBTYPE);
                            SetFieldValues(ControlsEnum.SOSUBTYPE);


                            GetFieldValues(ControlsEnum.CBMCONFIG);
                            SetFieldValues(ControlsEnum.CBMCONFIG);

                            hdfCurrency.Value = saleOrderHeaderObj.SOH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CURRENCY_TEXT);
                            txtExchangeRate.Text = saleOrderHeaderObj.SOH_CURRENCY_RATE.ToString(hdfExchangeRateFormat.Value);
                            hdfExchangeRate.Value = saleOrderHeaderObj.SOH_CURRENCY_RATE.ToString(hdfExchangeRateFormat.Value);
                            //While saving SC with no exchange rate,then exchange rate will save as 1
                            //while editing SC no need to get exchange rate from master(dis:Manoj sir)
                            //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            //if (exchangeRate > 0)
                            //{
                            //    hdfExchangeRate.Value = exchangeRate.ToString();
                            //    if (string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CURRENCY_RATE.ToString()))
                            //    {
                            //        txtExchangeRate.Text = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                            //    }
                            //    else
                            //    {
                            //        txtExchangeRate.Text = saleOrderHeaderObj.SOH_CURRENCY_RATE.ToString(hdfExchangeRateFormat.Value);
                            //    }
                            //}
                            //else
                            //{
                            //    //txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                            //    //hdfCurrency.Value = "0";
                            //    txtExchangeRate.Text = string.Empty;
                            //}


                            //Not allowed to edit exchange rate while currency same as base currency                           
                            if (currentUser.BaseCurrency == saleOrderHeaderObj.SOH_CURRENCY)  //saleOrderHeaderObj.SOH_TYPE == ((byte)SalesInvoiceType.Domestic) || 
                            {
                                txtExchangeRate.Enabled = false;
                            }
                            else
                            {
                                txtExchangeRate.Enabled = true;
                            }

                            if (saleOrderHeaderObj.SOH_QUOTATION > 0)
                            {
                                trQuotationReference.Visible = true;
                                //*******For Resolving Bug ID:  16122***************
                                //txtRefNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_NO) ? string.Empty : saleOrderHeaderObj.SOH_REF_NO;
                                //txtRefDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_DATE) ? string.Empty : saleOrderHeaderObj.SOH_REF_DATE;
                                if (Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK] != null)
                                {
                                    txtRefNo.Text = string.Empty; txtRefNo.Enabled = true; txtRefNo.CssClass = "input-small";
                                    txtRefDate.Text = string.Empty; txtRefDate.Enabled = true; txtRefDate.CssClass = "input-small";
                                }
                                else
                                {
                                    txtRefNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_NO) ? string.Empty : saleOrderHeaderObj.SOH_REF_NO; txtRefNo.Enabled = true; txtRefNo.CssClass = "input-small";
                                    txtRefDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_DATE) ? string.Empty : saleOrderHeaderObj.SOH_REF_DATE; txtRefDate.Enabled = true; txtRefDate.CssClass = "input-small";
                                }
                                //***************************************************
                                IsQuotationContract = true;
                            }
                            else
                            {
                                trQuotationReference.Visible = false;
                                txtRefNo.Enabled = false; txtRefNo.CssClass = "input-disabled";
                                txtRefDate.Enabled = false; txtRefDate.CssClass = "input-small input-disabled";
                            }

                            if (saleOrderHeaderObj.QuotationPK > 0)
                            {
                                txtQuotation.Text = saleOrderHeaderObj.QuotationNo;
                                hdfQuotationPK.Value = saleOrderHeaderObj.QuotationPK.ToString();
                            }

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY))
                                shipByPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_BY);
                            GetFieldValues(ControlsEnum.SHIPBY);
                            SetFieldValues(ControlsEnum.SHIPBY);

                            //if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT))
                            //    fromPortPK = Convert.ToInt32(saleOrderHeaderObj.SOH_FROM_PORT);
                            //GetFieldValues(ControlsEnum.FROMPORT);
                            //SetFieldValues(ControlsEnum.FROMPORT);

                            //FROM PORT - TO PORT
                            hdfFromPortID.Value = saleOrderHeaderObj.SOH_FROM_PORT == null ? "0" : saleOrderHeaderObj.SOH_FROM_PORT.ToString();
                            txtFromPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FROM_PORT_TEXT);

                            //For Demo-IN, To Port is made dropdown for Buyers with currency code 'INR' : Bug-35443
                            GetFieldValues(ControlsEnum.TOPORT);
                            SetFieldValues(ControlsEnum.TOPORT);
                            if (saleOrderHeaderObj.SOH_CURRENCY_TEXT == "INR" && IsToPortDdlShow == true)
                            {
                                ddlToPort.SelectedValue = saleOrderHeaderObj.SOH_TO_PORT_PK;
                                ddlToPort.Visible = true;
                                txtToPort.Visible = false;
                            }
                            else
                            {
                                txtToPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);

                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TO_PORT_PK))
                                    hdfToPortID.Value = saleOrderHeaderObj.SOH_TO_PORT_PK;
                                else
                                    hdfToPortID.Value = CommonConstants.SELECT_VALUE_ZERO;

                                txtToPort.Visible = true;
                                ddlToPort.Visible = false;
                            }
                            //For Demo-IN, To Port is made dropdown for Buyers with currency code 'INR' : Bug-3544
                            //FROM PORT - TO PORT

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT))
                                transhipmentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_TRANSHIPMENT);
                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);

                            //Consignee
                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE))
                                consigneePK = Convert.ToInt32(saleOrderHeaderObj.SOH_CONSIGNEE);
                            SetFieldValues(ControlsEnum.CONSIGNEE);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_NAME))
                                hdfCNEName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY))
                                hdfCNECountry.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY);
                            txtConsigneeDetails.Text = hdfCNEAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT))
                                hdfCNECountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL))
                                hdfCNEEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_FAX))
                                hdfCNEFax.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_FAX);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE))
                                hdfCNEMobile.Value = saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_PHONE))
                                hdfCNEPhone.Value = saleOrderHeaderObj.SOH_CONSIGNEE_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP))
                                hdfCNEZip.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_TO))
                                addressPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPPING_TO);
                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_NAME))
                                hdfShpName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY))
                                hdfShpCountry.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY;

                            txtShippingAddress.Text = hdfShpAddress.Value = saleOrderHeaderObj.SOH_SHIPPING_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT))
                                hdfShpCountryText.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_EMAIL))
                                hdfShpEmail.Value = saleOrderHeaderObj.SOH_SHIPPING_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_FAX))
                                hdfShpFax.Value = saleOrderHeaderObj.SOH_SHIPPING_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_MOBILE))
                                hdfShpMobile.Value = saleOrderHeaderObj.SOH_SHIPPING_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_PHONE))
                                hdfShpPhone.Value = saleOrderHeaderObj.SOH_SHIPPING_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_ZIP))
                                hdfShpZip.Value = saleOrderHeaderObj.SOH_SHIPPING_ZIP;

                            //Notify Party                           
                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY))
                                notifyPartyPK = Convert.ToInt32(saleOrderHeaderObj.SOH_NOTIFY_PARTY);
                            SetFieldValues(ControlsEnum.NOTIFYPARTY);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME))
                            {
                                hdfNPName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME);
                            }
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY))
                            {
                                hdfNPCountry.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY;
                            }
                            txtNotifyParty.Text = hdfNPAddress.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT))
                                hdfNPCountryText.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL))
                                hdfNPEmail.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX))
                                hdfNPFax.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE))
                                hdfNPMobile.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE))
                                hdfNPPhone.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP))
                                hdfNPZip.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT))
                                agentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_AGENT);
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME))
                                hdfAgentName.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_NAME;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY))
                                hdfAgentCountry.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY;
                            hdfAgentAddress.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT))
                                hdfAgentCountryText.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL))
                                hdfAgentEmail.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_FAX))
                                hdfAgentFax.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE))
                                hdfAgentMobile.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE))
                                hdfAgentPhone.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP))
                                hdfAgentZip.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP;

                            txtShppingIntimationto.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_INT_TO);
                            txtShppingIntimationtoFax.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FAX) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FAX);
                            txtContainerSize.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONTAINER_SIZE) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONTAINER_SIZE);

                            //GetFieldValues(ControlsEnum.DELIVERYTERMS); //Bug:6020	
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_DEL_TERM);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                                shipmentTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPMENT_TERM);
                            //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            //SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.SHIPMENTTERMS);
                            SetFieldValues(ControlsEnum.SHIPMENTTERMS);
                            //txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPMENT_TERM_TEXT);
                            ddlShipmentTerms.SelectedValue = saleOrderHeaderObj.SOH_SHIPMENT_TERM.ToString();

                            //GetFieldValues(ControlsEnum.PAYMENTTERMS); //Bug:6020	
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_PAYMENT_TERM);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT));

                            //GetFieldValues(ControlsEnum.SPECIALCAUSE); //Bug:6020	
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(saleOrderHeaderObj.SOH_SPECIAL_TERM);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);


                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK))
                                BankPk = bankDetailPK = Convert.ToInt32(saleOrderHeaderObj.SOH_BANK);
                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            SetFieldValues(ControlsEnum.BANKDETAILS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_INSP_TYPE))
                                inspectionPK = Convert.ToInt32(saleOrderHeaderObj.SOH_INSP_TYPE);
                            GetFieldValues(ControlsEnum.INSPECTION);
                            SetFieldValues(ControlsEnum.INSPECTION);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_EXP_DOC))
                                exportDocPK = Convert.ToInt32(saleOrderHeaderObj.SOH_EXP_DOC);
                            GetFieldValues(ControlsEnum.EXPORTDOC);
                            SetFieldValues(ControlsEnum.EXPORTDOC);
                            txtPackingInstruction.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PACKING_INSTRN);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_ORG_GOODS))
                                originOfGoodsPK = Convert.ToInt32(saleOrderHeaderObj.SOH_ORG_GOODS);
                            GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (CurrPK == 0)
                            {
                                EntryStatus = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.NEWMODE : EntryStatus;
                                if (dtPageData != null && dtPageData.Rows.Count > 0
                                && ddlOriginofGoods.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                    ddlOriginofGoods.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();
                                btnRevision.Visible = false;
                                if (controlType == ControlsEnum.COPYCONTRACT)
                                {
                                    chkNeedAdvPay.Checked = saleOrderHeaderObj.SOH_NEED_ADV_PYMT;
                                    txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FINAL_DESTINATION);
                                }
                                else
                                    txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);
                            }
                            else
                            {
                                chkNeedAdvPay.Checked = saleOrderHeaderObj.SOH_NEED_ADV_PYMT;
                                txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FINAL_DESTINATION);
                                ModifiedDatePnl.Visible = true;

                                LastModifiedTime = saleOrderHeaderObj.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                                if (saleOrderHeaderObj.SOH_VERSION > 1)
                                {
                                    btnRevision.Visible = true;
                                }
                                else
                                {
                                    btnRevision.Visible = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_AGENT))
                            {
                                hdfAgtSaves.Value = saleOrderHeaderObj.SOH_AGENT;
                            }
                            else
                            {
                                hdfAgtSaves.Value = "0";
                            }
                            txtRemarks.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REMARKS);

                            txtHdrDiscount.Text = saleOrderHeaderObj.SOH_TOTAL_DISCOUNT.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = saleOrderHeaderObj.SOH_TOTAL_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrTotal.Text = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);

                            txtHdrDiscount.ToolTip = saleOrderHeaderObj.SOH_TOTAL_DISCOUNT.ToString("c");
                            txtHdrTax.ToolTip = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString("c");
                            txtShipping.ToolTip = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE.ToString("c");
                            txtPriceAdj.ToolTip = saleOrderHeaderObj.SOH_TOTAL_ADJUST.ToString("c");
                            txtHdrTotal.ToolTip = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString("c");
                            ddlCompany.SelectedValue = saleOrderHeaderObj.SOH_COMPANY.ToString();
                            if (saleOrderHeaderObj.SOH_STATUS == (int)WorkFlowStatusEnum.Reviewed || saleOrderHeaderObj.SOH_STATUS == (int)WorkFlowStatusEnum.SendBackForApprove)
                            {
                                if (ucrWrkf.ViewType == 0)
                                {
                                    vrfInspection.Enabled = false;
                                    vrfExportDoc.Enabled = false;
                                }
                            }
                            else
                            {
                                vrfInspection.Enabled = false;
                                vrfExportDoc.Enabled = false;
                            }
                            //if (controlType != ControlsEnum.COPYCONTRACT)
                            //{
                            //    if ((saleOrderHeaderObj.SOH_STATUS == (int)SCStatusEnum.IOgenerate) || (saleOrderHeaderObj.SOH_STATUS == (int)SCStatusEnum.AcceptedMinfo && saleOrderHeaderObj.SOH_NEED_ADV_PYMT == false) || (saleOrderHeaderObj.SOH_STATUS == (int)SCStatusEnum.Accepted) || (saleOrderHeaderObj.SOH_STATUS == (int)SCStatusEnum.FinApproved))
                            //    {
                            //        ddlAgentN.Visible = true; lblAgentN.Visible = true;
                            //    }
                            //    else
                            //    {
                            //        ddlAgentN.Visible = false; lblAgentN.Visible = false;
                            //    }
                            //}

                            if (ShowSCAdditionalPackDtls == 1)
                            {
                                chkNetting.Checked = saleOrderHeaderObj.SOH_NETTING == 0 ? false : true;
                                chkInsulation.Checked = saleOrderHeaderObj.SOH_INSULATION == 0 ? false : true;
                                chkProteinTest.Checked = saleOrderHeaderObj.SOH_PROTEIN_TEST == 0 ? false : true;
                                chkAddlPackaging.Checked = saleOrderHeaderObj.SOH_ADDL_PACKAGE == 0 ? false : true;
                                chkPreShipment.Checked = saleOrderHeaderObj.SOH_PRE_SHIPMENT == 0 ? false : true;
                                txtInner.Text = saleOrderHeaderObj.SOH_INNER;
                                txtCarton.Text = saleOrderHeaderObj.SOH_CARTON;
                                ddlStandard.SelectedIndex = ddlStandard.Items.IndexOf(ddlStandard.Items.FindByValue(saleOrderHeaderObj.SOH_STANDARD));
                            }

                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        }
                        break;
                    #endregion
                    #region Customer Contract
                    case ControlsEnum.CUSTOMERCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            //------ Customer Contract Details ------
                            //Set default Ship By
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY)
                                && ddlShipBy.Items.FindByValue(saleOrderHeaderObj.SOH_SHIP_BY) != null)
                                ddlShipBy.SelectedValue = saleOrderHeaderObj.SOH_SHIP_BY;
                            //Set default From Port
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT))
                            {
                                hdfFromPortID.Value = saleOrderHeaderObj.SOH_FROM_PORT;
                                txtFromPort.Text = saleOrderHeaderObj.SOH_FROM_PORT_TEXT;
                            }

                            //Set default Transhipment
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT)
                                && ddlTranshipment.Items.FindByValue(saleOrderHeaderObj.SOH_TRANSHIPMENT) != null)
                                ddlTranshipment.SelectedValue = saleOrderHeaderObj.SOH_TRANSHIPMENT;

                            txtShppingIntimationto.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_INT_TO);
                            txtShppingIntimationtoFax.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FAX);
                            txtContainerSize.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONTAINER_SIZE);

                            //Adv payment
                            //chkNeedAdvPay.Checked = saleOrderHeaderObj.SOH_NEED_ADV_PYMT;

                            //------ Bind Address and Details ------
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE)
                                && ddlConsigneeDetails.Items.FindByValue(saleOrderHeaderObj.SOH_CONSIGNEE) != null)
                                ddlConsigneeDetails.SelectedValue = saleOrderHeaderObj.SOH_CONSIGNEE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_NAME))
                                hdfCNEName.Value = saleOrderHeaderObj.SOH_CONSIGNEE_NAME;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY))
                                hdfCNECountry.Value = saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY;
                            txtConsigneeDetails.Text = hdfCNEAddress.Value = saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT))
                                hdfCNECountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL))
                                hdfCNEEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_FAX))
                                hdfCNEFax.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_FAX);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE))
                                hdfCNEMobile.Value = saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_PHONE))
                                hdfCNEPhone.Value = saleOrderHeaderObj.SOH_CONSIGNEE_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP))
                                hdfCNEZip.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP);


                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY)
                                && ddlNotifyParty.Items.FindByValue(saleOrderHeaderObj.SOH_NOTIFY_PARTY) != null)
                                ddlNotifyParty.SelectedValue = saleOrderHeaderObj.SOH_NOTIFY_PARTY;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME))
                                hdfNPName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY))
                                hdfNPCountry.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY;
                            txtNotifyParty.Text = hdfNPAddress.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT))
                                hdfNPCountryText.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL))
                                hdfNPEmail.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX))
                                hdfNPFax.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE))
                                hdfNPMobile.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE))
                                hdfNPPhone.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP))
                                hdfNPZip.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP;
                            //sarath
                            //if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_TO)
                            //    && ddlCustAddress.Items.FindByValue(saleOrderHeaderObj.SOH_SHIPPING_TO) != null)
                            //    ddlCustAddress.SelectedValue = saleOrderHeaderObj.SOH_SHIPPING_TO;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_NAME))
                                hdfShpName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY))
                                hdfShpCountry.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY;
                            //hdfShpAddress.Value = saleOrderHeaderObj.SOH_SHIPPING_ADDRESS;
                            txtShippingAddress.Text = hdfShpAddress.Value = saleOrderHeaderObj.SOH_SHIPPING_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT))
                                hdfShpCountryText.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_EMAIL))
                                hdfShpEmail.Value = saleOrderHeaderObj.SOH_SHIPPING_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_FAX))
                                hdfShpFax.Value = saleOrderHeaderObj.SOH_SHIPPING_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_MOBILE))
                                hdfShpMobile.Value = saleOrderHeaderObj.SOH_SHIPPING_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_PHONE))
                                hdfShpPhone.Value = saleOrderHeaderObj.SOH_SHIPPING_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_ZIP))
                                hdfShpZip.Value = saleOrderHeaderObj.SOH_SHIPPING_ZIP;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT)
                                && ddlAgent.Items.FindByValue(saleOrderHeaderObj.SOH_SHIP_AGENT) != null)
                                ddlAgent.SelectedValue = saleOrderHeaderObj.SOH_SHIP_AGENT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME))
                                hdfAgentName.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_NAME;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY))
                                hdfAgentCountry.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY;
                            hdfAgentAddress.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT))
                                hdfAgentCountryText.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL))
                                hdfAgentEmail.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_FAX))
                                hdfAgentFax.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE))
                                hdfAgentMobile.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE))
                                hdfAgentPhone.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP))
                                hdfAgentZip.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP;

                            //if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM)
                            //    && ddlDeliveryTerms.Items.FindByValue(saleOrderHeaderObj.SOH_DEL_TERM) != null)
                            //    ddlDeliveryTerms.SelectedValue = saleOrderHeaderObj.SOH_DEL_TERM;

                            if (Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPMENT_TERM) > 0
                                && ddlShipmentTerms.Items.FindByValue(saleOrderHeaderObj.SOH_SHIPMENT_TERM.ToString()) != null)
                                ddlShipmentTerms.SelectedValue = saleOrderHeaderObj.SOH_SHIPMENT_TERM.ToString();

                            //txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPMENT_TERM_TEXT);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM)
                                && ddlPaymentTerms.Items.FindByValue(saleOrderHeaderObj.SOH_PAYMENT_TERM) != null)
                                ddlPaymentTerms.SelectedValue = saleOrderHeaderObj.SOH_PAYMENT_TERM;
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT));

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM)
                                && ddlSpecialCause.Items.FindByValue(saleOrderHeaderObj.SOH_SPECIAL_TERM) != null)
                                ddlSpecialCause.SelectedValue = saleOrderHeaderObj.SOH_SPECIAL_TERM;
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);

                            //Set default bank

                            BankPk = !string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK) ? Convert.ToInt32(saleOrderHeaderObj.SOH_BANK) : 0;
                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            SetFieldValues(ControlsEnum.BANKDETAILS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK)
                                && ddlBankDetails.Items.FindByValue(saleOrderHeaderObj.SOH_BANK) != null)
                                ddlBankDetails.SelectedValue = saleOrderHeaderObj.SOH_BANK;

                            //Set default inspection
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_INSP_TYPE)
                                && ddlInspection.Items.FindByValue(saleOrderHeaderObj.SOH_INSP_TYPE) != null)
                                ddlInspection.SelectedValue = saleOrderHeaderObj.SOH_INSP_TYPE;
                            //Set default Export docs
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_EXP_DOC)
                                && ddlExportDoc.Items.FindByValue(saleOrderHeaderObj.SOH_EXP_DOC) != null)
                                ddlExportDoc.SelectedValue = saleOrderHeaderObj.SOH_EXP_DOC;

                            txtPackingInstruction.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PACKING_INSTRN);
                            //set default origin of goods
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_ORG_GOODS)
                                && ddlOriginofGoods.Items.FindByValue(saleOrderHeaderObj.SOH_ORG_GOODS) != null)
                                ddlOriginofGoods.SelectedValue = saleOrderHeaderObj.SOH_ORG_GOODS;

                            txtRemarks.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REMARKS);
                        }
                        break;
                    #endregion

                    #region Line Packing Material Item Details
                    case ControlsEnum.SELECTEDPACKINGMATERAILITEM:
                        if (saleOrderDetailsObj != null)
                        {
                            rdbBrand.Checked = false;
                            if (saleOrderDetailsObj.SOD_IS_PACK_MAT == 1)
                            {
                                rdbPackingSpec.Checked = true;
                                rdbProduct.Checked = false;
                            }
                            else if (saleOrderDetailsObj.SOD_IS_PACK_MAT == 2)
                            {
                                rdbProduct.Checked = true;
                                rdbPackingSpec.Checked = false;
                            }
                            hdfPrdPackSpecPK.Value = saleOrderDetailsObj.SOD_ITEM.ToString();
                            //saleOrderDetailsObj.SOD_SALE_UOM_TEXT = saleOrderDetailsObj.SOD_UOM_TEXT;
                            txtspecqty.Text = GetFormattedNumber(saleOrderDetailsObj.SOD_SALE_QTY);
                            txtSpecCode.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_CUST_ITEM_CODE);
                            hdfPrdPackSpec.Value = saleOrderDetailsObj.SOD_PACKING_SPEC.ToString();
                            txtPrdPackSpec.Text = saleOrderDetailsObj.SOD_CUST_ITEM_TEXT.ToString();
                            txtspecprice.Text = GetFormattedNumber(saleOrderDetailsObj.SOD_RATE);
                            txtDiscount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_DISCOUNT);
                            txtspecamount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_AMOUNT);
                            txtTax.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_TAX);
                            txtspecremark.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_REMARKS);
                            txtSpecReqByDate.Text = saleOrderDetailsObj.SOD_REQUIRED_BY;
                            hdfSpecReqByDate.Value = Convert.ToDateTime(saleOrderDetailsObj.SOD_REQUIRED_BY).ToString();
                            hdfspecuom.Value = saleOrderDetailsObj.SOD_UOM.ToString();
                            hdfspecuomcode.Value = saleOrderDetailsObj.SOD_UOM_TEXT;// saleOrderDetailsObj.SOD_SALE_UOM_TEXT.ToString();
                            hdfapstotalpcs.Value = saleOrderDetailsObj.APS_TOTAL_PCS.ToString();
                            txtPrdCategory.Text = saleOrderDetailsObj.SOD_ITEM_CATEGORY.ToString();

                            hdfPrdType.Value = saleOrderDetailsObj.SOD_PACK_TYPE_VALUE.ToString();
                            txtPrdType.Text = saleOrderDetailsObj.SOD_PACK_TYPE_TEXT; //.ToString();

                            SPECQTY.Text = saleOrderDetailsObj.SOD_UOM_TEXT; //saleOrderDetailsObj.SOD_SALE_UOM_TEXT.ToString();
                            hdfPrdCategory.Value = saleOrderDetailsObj.SOD_ITEM_CATEGORY.ToString();
                            txtPrdCategory.Text = saleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT.ToString();

                        }
                        break;
                    #endregion


                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (saleOrderDetailsObj != null)
                        {
                            rdbPackingSpec.Checked = false;
                            rdbBrand.Checked = true;
                            ResetForm(ControlsEnum.PACKINGSPEC);
                            custprodPK = saleOrderDetailsObj.SOD_CUST_ITEM;
                            hdfDetailPK.Value = saleOrderDetailsObj.SOD_PK.ToString();
                            CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                            hdfBrand.Value = saleOrderDetailsObj.SOD_CUST_ITEM.ToString();
                            txtBrand.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_CUST_ITEM_TEXT);
                            txtBrandCode.Text = saleOrderDetailsObj.SOD_CUST_ITEM_CODE;
                            txtPacking.Text = saleOrderDetailsObj.CIM_PACKING_SPEC_NAME;
                            txtPacking.ToolTip = saleOrderDetailsObj.CIM_PACKING_SPEC_NAME;
                            hdfPackingSpec.Value = saleOrderDetailsObj.SOD_PACKING_SPEC.ToString();
                            hdfPackingText.Value = saleOrderDetailsObj.PACKING_TEXT.ToString();
                            hdfCBM.Value = saleOrderDetailsObj.CBM.ToString();
                            artWorkPK = string.IsNullOrEmpty(saleOrderDetailsObj.SOD_ART_WORK) ? 0 : Convert.ToInt32(saleOrderDetailsObj.SOD_ART_WORK);
                            GetFieldValues(ControlsEnum.PACKINGSPEC);

                            SetFieldValues(ControlsEnum.PACKINGSPEC);

                            if (!string.IsNullOrEmpty(saleOrderDetailsObj.SOD_ART_WORK))
                            {
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.PC_ART_WORK))
                                {
                                    lnkArtWorkPC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.PC_DOC_PATH))
                                        lnkArtWorkPC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkPC.Disabled = false;
                                        lnkArtWorkPC.HRef = saleOrderDetailsObj.PC_DOC_PATH;
                                    }
                                    lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.PC_ART_WORK);
                                    hdfArtWorkPC.Value = saleOrderDetailsObj.PC_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK))
                                {
                                    lnkArtWorkIB.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.IB_DOC_PATH))
                                        lnkArtWorkIB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIB.Disabled = false;
                                        lnkArtWorkIB.HRef = saleOrderDetailsObj.IB_DOC_PATH;
                                    }
                                    lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.IB_ART_WORK);
                                    hdfArtWorkIB.Value = saleOrderDetailsObj.IB_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK))
                                {
                                    lnkArtWorkIC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.IC_DOC_PATH))
                                        lnkArtWorkIC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIC.Disabled = false;
                                        lnkArtWorkIC.HRef = saleOrderDetailsObj.IC_DOC_PATH;
                                    }
                                    lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.IC_ART_WORK);
                                    hdfArtWorkIC.Value = saleOrderDetailsObj.IC_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK))
                                {
                                    lnkArtWorkZB.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.ZB_DOC_PATH))
                                        lnkArtWorkZB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkZB.Disabled = false;
                                        lnkArtWorkZB.HRef = saleOrderDetailsObj.ZB_DOC_PATH;
                                    }
                                    lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.ZB_ART_WORK);
                                    hdfArtWorkZB.Value = saleOrderDetailsObj.ZB_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK))
                                {
                                    lnkArtWorkMC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.MC_DOC_PATH))
                                        lnkArtWorkMC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkMC.Disabled = false;
                                        lnkArtWorkMC.HRef = saleOrderDetailsObj.MC_DOC_PATH;
                                    }
                                    lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.MC_ART_WORK);
                                    hdfArtWorkMC.Value = saleOrderDetailsObj.MC_DOC_PATH;
                                }

                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK))
                                {
                                    lnkArtWorkSC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.SC_DOC_PATH))
                                        lnkArtWorkSC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkSC.Disabled = false;
                                        lnkArtWorkSC.HRef = saleOrderDetailsObj.SC_DOC_PATH;
                                    }
                                    lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.SC_ART_WORK);
                                    hdfArtWorkSC.Value = saleOrderDetailsObj.SC_DOC_PATH;
                                }
                            }
                            txtQty.Text = GetFormattedNumber(saleOrderDetailsObj.SOD_QTY);
                            txtBrandQuantity.Text = hdfQtyTemp.Value = GetFormattedNumber(saleOrderDetailsObj.SOD_SALE_QTY);
                            hdfBrandUOM.Value = saleOrderDetailsObj.SOD_SALE_UOM.ToString();
                            hdfBrandUOMConversion.Value = saleOrderDetailsObj.SOD_SALE_UOM_CONV.ToString();
                            txtBrandUOM.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_SALE_UOM_TEXT);
                            HdfIsPcs.Value = saleOrderDetailsObj.SOD_UOM_IS_PCS.ToString();
                            if (HdfIsPcs.Value == "1")
                            {
                                txtctnRate.Enabled = true;
                                txtctnRate.CssClass = "medium select-half";
                            }
                            else
                            {
                                txtctnRate.Enabled = false;
                                txtctnRate.CssClass += " input-disabled";
                            }
                            txtBrandRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_SALE_UOM_TEXT);
                            hdfQtyDespatched.Value = saleOrderDetailsObj.SOD_QTY_DISPATCHED.ToString();
                            hdfQtyInvoiced.Value = saleOrderDetailsObj.SOD_QTY_INVOICED.ToString();
                            hdfUOM.Value = saleOrderDetailsObj.SOD_UOM.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_UOM_TEXT);
                            txtRate.Text = GetFormattedRate(saleOrderDetailsObj.SOD_RATE);
                            txtDiscount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_DISCOUNT);
                            txtLotNo.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_LOT_NO);
                            if(saleOrderDetailsObj.SOD_HSN_CODE_TEXT != null)
                            {
                                txtHSNNo.Text = saleOrderDetailsObj.SOD_HSN_CODE_TEXT.ToString();
                                hdfHSNNo.Value = saleOrderDetailsObj.SOD_HSN_CODE.ToString();
                            }

                            hdfProduct.Value = saleOrderDetailsObj.SOD_ITEM.ToString();
                            txtProduct.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_ITEM_CODE);
                            hdfProductName.Value = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_ITEM_TEXT);
                            txtTotalPiecesCtn.Text = hdfPcsperCtn.Value = Math.Floor(saleOrderDetailsObj.APS_TOTAL_PCS > 1 ? saleOrderDetailsObj.APS_TOTAL_PCS : 1).ToString();
                            hdfTotalPcsInBox.Value = Math.Floor(saleOrderDetailsObj.APS_IB_PCS > 0 ? saleOrderDetailsObj.APS_IB_PCS : 0).ToString();
                            hdfSaleUOMPcs.Value = saleOrderDetailsObj.SOD_SALE_UOM_CONV.ToString();
                            txtReqByDate.Text = saleOrderDetailsObj.SOD_REQUIRED_BY;
                            hdfReqByDate.Value = Convert.ToDateTime(saleOrderDetailsObj.SOD_REQUIRED_BY).ToString();
                            txtAmount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_AMOUNT);
                            txtTax.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_TAX);
                            txtLotSize.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_LOT_SIZE);
                            txtCaseMark.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_CASE_MARK);



                            hdfCtnRate.Value = txtctnRate.Text = saleOrderDetailsObj.SOD_CARTON_RATE;


                            //if (saleOrderDetailsObj.APS_IB_PCS > 0)
                            //{
                            //    hdfCtnRate.Value = txtctnRate.Text = string.IsNullOrEmpty(saleOrderDetailsObj.SOD_CARTON_RATE) || saleOrderDetailsObj.SOD_CARTON_RATE == "0"
                            //        ? Convert.ToString(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(hdfTotalPcsInBox.Value)) + " /Box \n" + Convert.ToString(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtTotalPiecesCtn.Text)) + " /Ctn "
                            //        : saleOrderDetailsObj.SOD_CARTON_RATE;
                            //}
                            //else
                            //{
                            //    hdfCtnRate.Value = txtctnRate.Text = string.IsNullOrEmpty(saleOrderDetailsObj.SOD_CARTON_RATE) || saleOrderDetailsObj.SOD_CARTON_RATE == "0"
                            //        ? Convert.ToString(Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtTotalPiecesCtn.Text)) + " /Ctn "
                            //        : saleOrderDetailsObj.SOD_CARTON_RATE;
                            //}

                            txtDtlRemark.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_REMARKS);
                            txtDtlRemark2.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_REMARKS2);

                            if (ShowSCAdditionalPackDtls == 1)
                            {
                                ddlstrappingColor.SelectedIndex = ddlstrappingColor.Items.IndexOf(ddlstrappingColor.Items.FindByValue(saleOrderDetailsObj.SOD_STRAPPING_COLOUR));
                                chkStrapping.Checked = saleOrderDetailsObj.SOD_STRAPPING == "1" ? true : false;
                                chkLayering.Checked = saleOrderDetailsObj.SOD_LAYERING == "1" ? true : false;
                                txtNoofLayers.Text = saleOrderDetailsObj.SOD_NO_LAYERS.ToString();
                                txtPcsLayers.Text = saleOrderDetailsObj.SOD_PIECES_LAYER.ToString();

                                if (hdfInitMfgDate.Value == "1")
                                {
                                    txtMfgDate.Text = !string.IsNullOrEmpty(saleOrderDetailsObj.SOD_MFG_DATE) ? (Convert.ToDateTime(saleOrderDetailsObj.SOD_MFG_DATE)).ToString(Resources.Constants.HRMSDateFormatShort) : string.Empty;
                                    txtExpiryDate.Text = !string.IsNullOrEmpty(saleOrderDetailsObj.SOD_EXP_DATE) ? (Convert.ToDateTime(saleOrderDetailsObj.SOD_EXP_DATE)).ToString(Resources.Constants.HRMSDateFormatShort) : string.Empty;
                                }
                                else
                                {
                                    txtMfgDate.Text = saleOrderDetailsObj.SOD_MFG_DATE;
                                    txtExpiryDate.Text = saleOrderDetailsObj.SOD_EXP_DATE;
                                }
                            }
                        }
                        break;
                    #endregion
                    # region  FILE_UPLOAD
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = saleOrderHeaderObj.SOH_PK;
                        SOUploadList = saleOrderHeaderObj.FileList;
                        break;
                    case ControlsEnum.SELECTEDDOC:
                        if (soUploadObj != null)
                        {
                            CurrDocSlNo = soUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = soUploadObj.DOC_NAME;
                            anchorFile.HRef = soUploadObj.DOC_PATH;
                            anchorFile.Attributes.Remove("onclick");
                            anchorFile.Style.Add("cursor", "pointer");
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrDocSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Style.Add("cursor", "default");
                            }
                        }
                        break;
                        #endregion
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
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.PACKINGSPEC:
                        ddlArtWork.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlArtWork.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "PIM_ART_WORK");
                            ddlArtWork.DataTextField = "PIM_ART_WORK";
                            ddlArtWork.DataValueField = "PIM_PK";
                            ddlArtWork.DataBind();
                        }
                        ddlArtWork.Items.Insert(0, new ListItem(Resources.ErpRes.New, CommonConstants.SELECTVAL));
                        if (artWorkPK > 0 && ddlArtWork.Items.FindByValue(artWorkPK.ToString()) != null)
                            ddlArtWork.SelectedValue = artWorkPK.ToString();
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        ddlCustAddress.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlCustAddress.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlCustAddress.DataTextField = "CAD_NAME";
                            ddlCustAddress.DataValueField = "CAD_PK";
                            ddlCustAddress.DataBind();
                            if (addressPK == 0)
                            {
                                ddlCustAddress.SelectedItem.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                            }
                        }
                        ddlCustAddress.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (addressPK > 0 && ddlCustAddress.Items.FindByValue(addressPK.ToString()) != null)
                            ddlCustAddress.SelectedValue = addressPK.ToString();
                        break;
                    case ControlsEnum.NOTIFYPARTY:
                        ddlNotifyParty.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlNotifyParty.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlNotifyParty.DataTextField = "CAD_NAME";
                            ddlNotifyParty.DataValueField = "CAD_PK";
                            ddlNotifyParty.DataBind();
                        }
                        ddlNotifyParty.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (notifyPartyPK > 0 && ddlNotifyParty.Items.FindByValue(notifyPartyPK.ToString()) != null)
                            ddlNotifyParty.SelectedValue = notifyPartyPK.ToString();
                        break;
                    case ControlsEnum.CONSIGNEE:
                        ddlConsigneeDetails.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlConsigneeDetails.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlConsigneeDetails.DataTextField = "CAD_NAME";
                            ddlConsigneeDetails.DataValueField = "CAD_PK";
                            ddlConsigneeDetails.DataBind();
                        }
                        ddlConsigneeDetails.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (consigneePK > 0 && ddlConsigneeDetails.Items.FindByValue(consigneePK.ToString()) != null)
                            ddlConsigneeDetails.SelectedValue = consigneePK.ToString();
                        break;
                    case ControlsEnum.FROMPORT:
                        //ddlFromPort.Items.Clear();
                        //if (dtPageData != null)
                        //{
                        //    ddlFromPort.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                        //    ddlFromPort.DataTextField = "CON_NAME";
                        //    ddlFromPort.DataValueField = "CON_PK";
                        //    ddlFromPort.DataBind();
                        //}
                        ////ddlFromPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //if (fromPortPK > 0 && ddlFromPort.Items.FindByValue(fromPortPK.ToString()) != null)
                        //    ddlFromPort.SelectedValue = fromPortPK.ToString();
                        break;
                    case ControlsEnum.ORIGINOFGOODS:
                        ddlOriginofGoods.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlOriginofGoods.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlOriginofGoods.DataTextField = "CON_NAME";
                            ddlOriginofGoods.DataValueField = "CON_PK";
                            ddlOriginofGoods.DataBind();
                        }
                        ddlOriginofGoods.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (originOfGoodsPK > 0 && ddlOriginofGoods.Items.FindByValue(originOfGoodsPK.ToString()) != null)
                            ddlOriginofGoods.SelectedValue = originOfGoodsPK.ToString();
                        break;
                    case ControlsEnum.BANKDETAILS:
                        ddlBankDetails.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlBankDetails.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CBM_NAME");
                            ddlBankDetails.DataTextField = "CBM_NAME";
                            ddlBankDetails.DataValueField = "CBM_PK";
                            ddlBankDetails.DataBind();
                        }
                        ddlBankDetails.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (bankDetailPK > 0 && ddlBankDetails.Items.FindByValue(bankDetailPK.ToString()) != null)
                            ddlBankDetails.SelectedValue = bankDetailPK.ToString();
                        break;
                    case ControlsEnum.SOTYPE:
                        ddlSaleOrderType.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSaleOrderType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlSaleOrderType.DataTextField = "CFG_DATA";
                            ddlSaleOrderType.DataValueField = "CFG_VALUE";
                            ddlSaleOrderType.DataBind();
                        }
                        ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (soTypePK > 0 && ddlSaleOrderType.Items.FindByValue(soTypePK.ToString()) != null)
                            ddlSaleOrderType.SelectedValue = soTypePK.ToString();
                        break;
                    case ControlsEnum.SOSUBTYPE:
                        ddlSaleOrderSubtype.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSaleOrderSubtype.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_DATA");
                            ddlSaleOrderSubtype.DataTextField = "CON_NAME";
                            ddlSaleOrderSubtype.DataValueField = "CON_PK";
                            ddlSaleOrderSubtype.DataBind();
                        }
                        if (soSubTypePK > 0 && ddlSaleOrderSubtype.Items.FindByValue(soSubTypePK.ToString()) != null)
                            ddlSaleOrderSubtype.SelectedValue = soSubTypePK.ToString();

                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        ddlAgent.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlAgent.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlAgent.DataTextField = "CAD_NAME";
                            ddlAgent.DataValueField = "CAD_PK";
                            ddlAgent.DataBind();
                        }
                        ddlAgent.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (agentPK > 0 && ddlAgent.Items.FindByValue(agentPK.ToString()) != null)
                            ddlAgent.SelectedValue = agentPK.ToString();
                        break;
                    case ControlsEnum.AGENT:
                        ddlAgentN.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlAgentN.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "ACD_AGENT_NAME");
                            ddlAgentN.DataTextField = "ACD_AGENT_NAME";
                            ddlAgentN.DataValueField = "ACD_AGENT";
                            ddlAgentN.DataBind();
                            vrfagent.Enabled = true;
                        }
                        else
                        {
                            vrfagent.Enabled = false;
                        }
                       //if (dtPageData == null || dtPageData.Rows.Count == 0)
                        if(CurrPK>0)
                        {
                            if(ddlAgentN.Items.Count>0)
                            {
                                ddlAgentN.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                            }

                        }



                        break;
                    case ControlsEnum.TOPORT:
                        ddlToPort.Items.Clear();
                        if (dtToPort != null)
                        {
                            ddlToPort.DataSource = CommonFunctions.HtmlDecodeDataTable(dtToPort, "PRM_NAME");
                            ddlToPort.DataTextField = "PRM_NAME";
                            ddlToPort.DataValueField = "PRM_PK";
                            ddlToPort.DataBind();
                        }
                        ddlToPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (IsToPortDdlShow == true)
                        {
                            ddlToPort.Visible = true;
                            txtToPort.Visible = false;
                        }
                        else
                        {
                            ddlToPort.Visible = false;
                            txtToPort.Visible = true;
                        }
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        ddlTranshipment.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlTranshipment.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlTranshipment.DataTextField = "CON_NAME";
                            ddlTranshipment.DataValueField = "CON_PK";
                            ddlTranshipment.DataBind();
                        }
                        ddlTranshipment.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (transhipmentPK > 0 && ddlTranshipment.Items.FindByValue(transhipmentPK.ToString()) != null)
                            ddlTranshipment.SelectedValue = transhipmentPK.ToString();
                        break;
                    case ControlsEnum.SHIPBY:
                        ddlShipBy.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlShipBy.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlShipBy.DataTextField = "CON_NAME";
                            ddlShipBy.DataValueField = "CON_PK";
                            ddlShipBy.DataBind();
                        }
                        ddlShipBy.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (shipByPK > 0 && ddlShipBy.Items.FindByValue(shipByPK.ToString()) != null)
                            ddlShipBy.SelectedValue = shipByPK.ToString();
                        break;
                    //case ControlsEnum.DELIVERYTERMS:
                    //    ddlDeliveryTerms.Items.Clear();
                    //    if (dtPageData != null)
                    //    {
                    //        ddlDeliveryTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                    //        ddlDeliveryTerms.DataTextField = "TCH_NAME";
                    //        ddlDeliveryTerms.DataValueField = "TCH_PK";
                    //        ddlDeliveryTerms.DataBind();
                    //    }
                    //    ddlDeliveryTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //    if (deliveryTermPK > 0 && ddlDeliveryTerms.Items.FindByValue(deliveryTermPK.ToString()) != null)
                    //        ddlDeliveryTerms.SelectedValue = deliveryTermPK.ToString();
                    //    break;
                    case ControlsEnum.SHIPMENTTERMS:
                        ddlShipmentTerms.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlShipmentTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlShipmentTerms.DataTextField = "CON_NAME";
                            ddlShipmentTerms.DataValueField = "CON_PK";
                            ddlShipmentTerms.DataBind();
                        }
                        ddlShipmentTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (shipmentTermPK > 0 && ddlShipmentTerms.Items.FindByValue(shipmentTermPK.ToString()) != null)
                            ddlShipmentTerms.SelectedValue = shipmentTermPK.ToString();
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        ddlPaymentTerms.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlPaymentTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                            ddlPaymentTerms.DataTextField = "TCH_NAME";
                            ddlPaymentTerms.DataValueField = "TCH_PK";
                            ddlPaymentTerms.DataBind();
                        }
                        ddlPaymentTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (paymentTermPK > 0 && ddlPaymentTerms.Items.FindByValue(paymentTermPK.ToString()) != null)
                            ddlPaymentTerms.SelectedValue = paymentTermPK.ToString();
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        ddlSpecialCause.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSpecialCause.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                            ddlSpecialCause.DataTextField = "TCH_NAME";
                            ddlSpecialCause.DataValueField = "TCH_PK";
                            ddlSpecialCause.DataBind();
                        }
                        ddlSpecialCause.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (specialCausePK > 0 && ddlSpecialCause.Items.FindByValue(specialCausePK.ToString()) != null)
                            ddlSpecialCause.SelectedValue = specialCausePK.ToString();
                        break;
                    case ControlsEnum.INSPECTION:
                        ddlInspection.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlInspection.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlInspection.DataTextField = "CFG_DATA";
                            ddlInspection.DataValueField = "CFG_VALUE";
                            ddlInspection.DataBind();
                        }
                        ddlInspection.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (inspectionPK > 0 && ddlInspection.Items.FindByValue(inspectionPK.ToString()) != null)
                            ddlInspection.SelectedValue = inspectionPK.ToString();
                        break;
                    case ControlsEnum.EXPORTDOC:
                        ddlExportDoc.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlExportDoc.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlExportDoc.DataTextField = "CFG_DATA";
                            ddlExportDoc.DataValueField = "CFG_VALUE";
                            ddlExportDoc.DataBind();
                        }
                        ddlExportDoc.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (exportDocPK > 0 && ddlExportDoc.Items.FindByValue(exportDocPK.ToString()) != null)
                            ddlExportDoc.SelectedValue = exportDocPK.ToString();
                        break;
                    case ControlsEnum.TAXTYPES:
                        //Bind Tax dropdown
                        ddlPopupTaxType.Items.Clear();
                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count > 0)
                        {
                            ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSaleOrderTaxDetails, "TAX_DISP_NAME");
                            ddlPopupTaxType.DataTextField = "TAX_DISP_NAME";
                            ddlPopupTaxType.DataValueField = "TAX_PK";
                            ddlPopupTaxType.DataBind();
                        }

                        if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                            ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));

                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }

                        //if (dtCompany.Rows.Count > 0)
                        //{
                        //    ddlCompany.DataSource = dtCompany;

                        //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(dtCompany);,Resources.DataFieldRes.CompanySpecs);
                        //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        //    ddlCompany.DataBind();

                        //}


                        break;
                    #endregion
                    case ControlsEnum.STRAPPINGCOLOR:
                        ddlstrappingColor.Items.Clear();
                        ddlStrappingColorView.Items.Clear();
                        if (dtStrapping != null)
                        {
                            ddlstrappingColor.DataSource = CommonFunctions.HtmlDecodeDataTable(dtStrapping, "CON_NAME");
                            ddlstrappingColor.DataTextField = "CON_NAME";
                            ddlstrappingColor.DataValueField = "CON_PK";
                            ddlstrappingColor.DataBind();

                            ddlStrappingColorView.DataSource = CommonFunctions.HtmlDecodeDataTable(dtStrapping, "CON_NAME");
                            ddlStrappingColorView.DataTextField = "CON_NAME";
                            ddlStrappingColorView.DataValueField = "CON_PK";
                            ddlStrappingColorView.DataBind();

                        }
                        ddlstrappingColor.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlStrappingColorView.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.STANDARD:
                        ddlStandard.Items.Clear();
                        if (dtStrapping != null)
                        {
                            ddlStandard.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlStandard.DataTextField = "CON_NAME";
                            ddlStandard.DataValueField = "CON_PK";
                            ddlStandard.DataBind();
                        }
                        ddlStandard.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;

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
                    case ControlsEnum.SALEORDERDETAIL:
                        if (rdbPackingSpec.Checked == true)
                        {
                            if (saleOrderHeaderObj != null)
                            {
                                saleOrderDetailsList = new List<SaleContractDetailsBO>();
                                saleOrderDetailsList = saleOrderHeaderObj.SaleContractDetails;
                                if (saleOrderDetailsList != null)
                                {
                                    grdItemDetails.DataSource = saleOrderDetailsList;
                                    grdItemDetails.DataBind();
                                    if (GetGlobalResourceObject("ConfigurationsRes", "ShowHSNNo").ToString() == "1")
                                    {
                                        grdItemDetails.Columns[19].Visible = true;
                                    }
                                    else
                                    {
                                        grdItemDetails.Columns[19].Visible = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (saleOrderHeaderObj != null)
                            {
                                saleOrderDetailsList = new List<SaleContractDetailsBO>();
                                saleOrderDetailsList = saleOrderHeaderObj.SaleContractDetails;
                                if (saleOrderDetailsList != null)
                                {
                                    grdItemDetails.DataSource = saleOrderDetailsList;
                                    grdItemDetails.DataBind();
                                    if (GetGlobalResourceObject("ConfigurationsRes", "ShowHSNNo").ToString() == "1")
                                    {
                                        grdItemDetails.Columns[19].Visible = true;
                                    }
                                    else
                                    {
                                        grdItemDetails.Columns[19].Visible = false;
                                    }
                                }
                                GetFieldValues(ControlsEnum.AGENT);
                                SetFieldValues(ControlsEnum.AGENT);
                            }
                        }
                        break;
                    case ControlsEnum.REBINDSALEORDER:
                        if (SaleOrderHeaderSession != null)
                        {
                            if (SaleOrderHeaderSession.SaleContractDetails != null)
                            {
                                grdItemDetails.DataSource = SaleOrderHeaderSession.SaleContractDetails;
                                grdItemDetails.DataBind();
                                if (GetGlobalResourceObject("ConfigurationsRes", "ShowHSNNo").ToString() == "1")
                                {
                                    grdItemDetails.Columns[19].Visible = true;
                                }
                                else
                                {
                                    grdItemDetails.Columns[19].Visible = false;
                                }
                            }
                        }
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            string[] CatPKS = GetLocalResourceObject("OtherChargeCategoryValues").ToString().Split(',');// 2-> Shipment, 5->Duties, 6-> Surchages;
                            if (CatPKS.Contains(hdfTaxCategory.Value))
                            {                               
                                contractTaxHdrList = TempSaleOrderHeaderSession.TaxHdr == null ? new List<SaleOrderTaxHdr>() :
                                  TempSaleOrderHeaderSession.TaxHdr.Where(tax => CatPKS.Contains(tax.SLT_TAX_CATEGORY.ToString())).ToList();
                            }
                            else
                            {
                                contractTaxHdrList = TempSaleOrderHeaderSession.TaxHdr == null ? new List<SaleOrderTaxHdr>() :
                                    TempSaleOrderHeaderSession.TaxHdr.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        else
                        {
                            if (CurrSlNo > 0)
                            {
                                contractDetails = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                    TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);

                            }
                            else
                            {
                                contractDetails = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                    TempSaleOrderHeaderSession.SaleContractDetails.LastOrDefault(dtl => dtl.SOD_PK == SelectedDtlPK
                                    && dtl.SOD_CUST_ITEM == SelectedCusItemPK && dtl.SOD_ITEM == SelectedItemPK);
                            }

                            if (contractDetails != null)
                            {
                                contractTaxHdrList = contractDetails.TaxDtl == null ? new List<SaleOrderTaxHdr>() :
                                    contractDetails.TaxDtl.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                contractTaxHdrList = new List<SaleOrderTaxHdr>();
                        }
                        if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString())
                        {
                            grdTaxDetails.Columns[2].Visible = true; // show the discount percentage column when the popup is against Discount
                        }
                        else
                        {
                            grdTaxDetails.Columns[2].Visible = false;// hide the discount percentage column when the popup is not against Discount
                        }
                        grdTaxDetails.DataSource = contractTaxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        grdRevisionHistory.DataSource = dsPageData.Tables[0];
                        grdRevisionHistory.DataBind();
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = SOUploadList;
                        grdUploads.DataBind();
                        break;
                    case ControlsEnum.ITEMREFDTL:
                        if (saleOrderItemRefObj != null && saleOrderItemRefObj.RefDetails != null && saleOrderItemRefObj.RefDetails.Count > 0)
                            grdItemRefDtl.DataSource = saleOrderItemRefObj.RefDetails;
                        else
                            grdItemRefDtl.DataSource = null;
                        grdItemRefDtl.DataBind();
                        #region Set Column visibility
                        if (SodPk == 0)
                        {
                            grdItemRefDtl.Columns[0].Visible = true;
                            grdItemRefDtl.Columns[1].Visible = false;
                            grdItemRefDtl.Columns[2].Visible = false;
                            grdItemRefDtl.Columns[3].Visible = false;
                        }
                        else
                        {
                            grdItemRefDtl.Columns[0].Visible = false;
                            grdItemRefDtl.Columns[1].Visible = true;
                            grdItemRefDtl.Columns[2].Visible = true;
                            grdItemRefDtl.Columns[3].Visible = true;
                        }

                        #endregion
                        break;
                    case ControlsEnum.SALESCOST:
                        grdSalesCost.DataSource = dtSalesCost;
                        grdSalesCost.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Validate for invalid entry in Autocomplete fields
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>Validation Status</returns>
        private bool ValidateForm(ActionsEnum mode)
        {
            bool flag;
            flag = true;
            switch (mode)
            {
                case ActionsEnum.SAVE:
                case ActionsEnum.WRKFSUBMIT:

                    break;
            }
            return flag;
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.SALEORDERHEADER:
                    CurrPK = 0;
                    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrQuotationPK;
                    CurrQuotationPK = 0;
                    dvPerc.Visible = false;
                    break;
                case ControlsEnum.SALEORDERDETAIL:
                    if (rdbPackingSpec.Checked == true)
                    {
                        TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                        CurrSlNo = 0;
                        ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                    }
                    else
                    {
                        TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                        CurrSlNo = 0;
                        hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                        hdfBrand.Value = string.Empty;
                        txtBrand.Text = string.Empty;
                        ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                        dvPerc.Visible = false;
                    }
                    break;
                case ControlsEnum.CLEARPACKMATERIALITEM:
                    txtspecamount.Text = string.Empty;
                    txtSpecCode.Text = string.Empty;
                    txtspecprice.Text = string.Empty;
                    txtspecqty.Text = string.Empty;
                    txtspecremark.Text = string.Empty;
                    hdfspecamount.Value = string.Empty;
                    hdfspeccode.Value = string.Empty;
                    hdfspecprice.Value = string.Empty;
                    hdfspecqty.Value = string.Empty;
                    txtPrdCategory.Text = string.Empty;
                    txtPrdPackSpec.Text = string.Empty;
                    txtPrdType.Text = string.Empty;
                    hdfPrdCategory.Value = string.Empty;
                    hdfPrdPackSpec.Value = string.Empty;
                    hdfPrdType.Value = string.Empty;
                    hdfSpecReqByDate.Value = string.Empty;
                    hdfReqByDate.Value = string.Empty;
                    hdfspecuomcode.Value = string.Empty;
                    hdfspecuom.Value = string.Empty;
                    hdfapstotalpcs.Value = string.Empty;
                    hdfspecsaleuom.Value = string.Empty;
                    break;
                case ControlsEnum.CUSTOMERCONTRACT:
                    TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                    ResetDdl(ddlShipBy);
                    //    ResetDdl(ddlFromPort);
                    ResetDdl(ddlTranshipment);

                    txtShppingIntimationto.Text = string.Empty;
                    txtShppingIntimationtoFax.Text = string.Empty;
                    txtContainerSize.Text = string.Empty;

                    ClearDdl(ControlsEnum.CONSIGNEE);
                    hdfCNEName.Value = hdfCNECountry.Value = txtConsigneeDetails.Text = hdfCNEAddress.Value = hdfCNECountryText.Value =
                        hdfCNEEmail.Value = hdfCNEFax.Value = hdfCNEMobile.Value = hdfCNEPhone.Value = hdfCNEZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.NOTIFYPARTY);
                    hdfNPName.Value = hdfNPCountry.Value = txtNotifyParty.Text = hdfNPAddress.Value = hdfNPCountryText.Value =
                        hdfNPEmail.Value = hdfNPFax.Value = hdfNPMobile.Value = hdfNPPhone.Value = hdfNPZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.CUSTOMERADDRESS);
                    hdfShpName.Value = hdfShpCountry.Value = txtShippingAddress.Text = hdfShpAddress.Value = hdfShpCountryText.Value =
                        hdfShpEmail.Value = hdfShpFax.Value = hdfShpMobile.Value = hdfShpPhone.Value = hdfShpZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.SHIPPINGAGENT);
                    hdfAgentName.Value = hdfAgentCountry.Value = hdfAgentAddress.Value = hdfAgentCountryText.Value = hdfAgentEmail.Value =
                        hdfAgentFax.Value = hdfAgentMobile.Value = hdfAgentPhone.Value = hdfAgentZip.Value = string.Empty;

                    //ClearDdl(ControlsEnum.DELIVERYTERMS);
                    ClearDdl(ControlsEnum.SHIPMENTTERMS);
                    txtDeliveryTerms.Text = string.Empty;
                    ClearDdl(ControlsEnum.PAYMENTTERMS);
                    txtPaymentTerms.Text = string.Empty;
                    ClearDdl(ControlsEnum.SPECIALCAUSE);
                    txtSpecialCause.Text = string.Empty;

                    ResetDdl(ddlBankDetails);
                    ResetDdl(ddlInspection);
                    ResetDdl(ddlExportDoc);
                    txtPackingInstruction.Text = string.Empty;
                    ResetDdl(ddlOriginofGoods);
                    break;
                case ControlsEnum.CUSTOMER:
                    txtBuyerAddress.Text = hdfCusAddress.Value = hdfCusCountry.Value = hdfCusCountryText.Value = hdfCusZip.Value =
                                    hdfCusPhone.Value = hdfCusMobile.Value = hdfCusFax.Value = hdfCusEmail.Value = string.Empty;
                    hdfCurrency.Value = txtCurrency.Text = txtPortofDischarge.Text = txtToPort.Text = hdfExchangeRate.Value = txtFromPort.Text = string.Empty;
                    hdfToPortID.Value = CommonConstants.SELECT_VALUE_ZERO;
                    ResetDdl(ddlToPort);
                    break;
                case ControlsEnum.QUOTATION:
                    txtQuotation.Text = string.Empty;
                    hdfQuotationPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    break;
                case ControlsEnum.CUSTOMERPRODUCT:
                    if (rdbPackingSpec.Checked == true || rdbProduct.Checked == true)
                    {
                        txtspecamount.Text = string.Empty;
                        txtSpecCode.Text = string.Empty;
                        txtspecprice.Text = string.Empty;
                        txtspecqty.Text = string.Empty;
                        txtspecremark.Text = string.Empty;
                        hdfspecamount.Value = string.Empty;
                        hdfspeccode.Value = string.Empty;
                        hdfspecprice.Value = string.Empty;
                        hdfspecqty.Value = string.Empty;
                        txtPrdCategory.Text = string.Empty;
                        txtPrdPackSpec.Text = string.Empty;
                        txtPrdType.Text = string.Empty;
                        hdfPrdCategory.Value = string.Empty;
                        hdfPrdPackSpec.Value = string.Empty;
                        hdfPrdType.Value = string.Empty;
                        hdfSpecReqByDate.Value = string.Empty;
                        hdfReqByDate.Value = string.Empty;
                        hdfspecuomcode.Value = string.Empty;
                        hdfspecuom.Value = string.Empty;
                        hdfapstotalpcs.Value = string.Empty;
                        hdfspecsaleuom.Value = string.Empty;
                        SPECQTY.Text = string.Empty;
                        hdfspecqty.Value = string.Empty;
                        txtHSNNo.Text = string.Empty;
                        hdfHSNNo.Value = string.Empty;
                    }
                    else
                    {
                        txtBrandCode.Text = string.Empty;
                        txtPacking.Text = string.Empty;
                        txtPacking.ToolTip = string.Empty;
                        hdfPackingSpec.Value = string.Empty;
                        hdfPackingText.Value = string.Empty;
                        ClearDdl(ControlsEnum.PACKINGSPEC);
                        ResetForm(ControlsEnum.PACKINGSPEC);
                        txtBrandQuantity.Text = txtQty.Text = hdfQtyTemp.Value = string.Empty;
                        hdfQtyDespatched.Value = string.Empty;
                        hdfQtyInvoiced.Value = string.Empty;
                        hdfUOM.Value = string.Empty;
                        HdfIsPcs.Value = "0";
                        hdfBrandUOM.Value = string.Empty;
                        hdfBrandUOMConversion.Value = string.Empty;
                        txtUOM.Text = string.Empty;
                        txtRate.Text = string.Empty;
                        txtDiscount.Text = string.Empty;
                        txtHSNNo.Text = string.Empty;
                        hdfHSNNo.Value = string.Empty;
                        if (!chkLotNoApplyAllItem.Checked)
                        {
                            txtLotNo.Text = string.Empty;
                        }
                        if(ShowCaseMarkCheckAllItem==1)
                        {
                            if (!ChkCaseMarkApplyAllItem.Checked)
                                txtCaseMark.Text = string.Empty;
                        }
                        else
                        {
                            txtCaseMark.Text = string.Empty;

                        }
                        txtBrandRateUOM.Text = txtBrandUOM.Text = string.Empty;
                        txtBrand.Enabled = true;

                        hdfProduct.Value = string.Empty;
                        txtProduct.Text = string.Empty;
                        hdfProductName.Value = string.Empty;
                        txtTotalPiecesCtn.Text = string.Empty;
                        txtAmount.Text = string.Empty;
                        txtctnRate.Text = string.Empty;
                        hdfCtnRate.Value = string.Empty;
                        hdfTotalPcsInBox.Value = string.Empty;

                        txtTax.Text = string.Empty;
                        txtDiscount.Text = string.Empty;
                        txtLotSize.Text = string.Empty;
                        //txtCaseMark.Text = string.Empty;
                        txtDtlRemark.Text = string.Empty;

                        if (ShowSCAdditionalPackDtls == 1)
                        {
                            if (ddlstrappingColor.Items.Count > 0)
                                ddlstrappingColor.SelectedIndex = 0;
                            chkStrapping.Checked = false;
                            chkLayering.Checked = false;
                            txtNoofLayers.Text = string.Empty;
                            txtPcsLayers.Text = string.Empty;
                            //txtMfgDate.Text = string.Empty;
                            //txtExpiryDate.Text = string.Empty;
                            if (!ChkMfgDateApplyAllItem.Checked)
                            {
                                txtMfgDate.Text = string.Empty;
                            }
                            if (!ChkExpDateApplyAllItem.Checked)
                            {
                                txtExpiryDate.Text = string.Empty;
                            }
                        }
                    }
                    //txtDtlRemark2.Text = string.Empty;
                    break;

                case ControlsEnum.PRODUCT:
                    txtspecamount.Text = string.Empty;
                    txtSpecCode.Text = string.Empty;
                    txtspecprice.Text = string.Empty;
                    txtspecqty.Text = string.Empty;
                    txtspecremark.Text = string.Empty;
                    hdfspecamount.Value = string.Empty;
                    hdfspeccode.Value = string.Empty;
                    hdfspecprice.Value = string.Empty;
                    hdfspecqty.Value = string.Empty;
                    txtPrdCategory.Text = string.Empty;
                    txtPrdPackSpec.Text = string.Empty;
                    txtPrdType.Text = string.Empty;
                    hdfPrdCategory.Value = string.Empty;
                    hdfPrdPackSpec.Value = string.Empty;
                    hdfPrdType.Value = string.Empty;
                    hdfSpecReqByDate.Value = string.Empty;
                    hdfReqByDate.Value = string.Empty;
                    hdfspecuomcode.Value = string.Empty;
                    hdfspecuom.Value = string.Empty;
                    hdfapstotalpcs.Value = string.Empty;
                    hdfspecsaleuom.Value = string.Empty;
                    SPECQTY.Text = string.Empty;
                    hdfspecqty.Value = string.Empty;
                    break;

                case ControlsEnum.PACKINGSPEC:
                    hdfCBM.Value = CommonConstants.SELECT_VALUE_ZERO;
                    lnkArtWorkPC.HRef = "#";
                    lnkArtWorkIB.HRef = "#";
                    lnkArtWorkIC.HRef = "#";
                    lnkArtWorkZB.HRef = "#";
                    lnkArtWorkMC.HRef = "#";
                    lnkArtWorkSC.HRef = "#";

                    lnkArtWorkPC.Visible = false;
                    lnkArtWorkIB.Visible = false;
                    lnkArtWorkIC.Visible = false;
                    lnkArtWorkZB.Visible = false;
                    lnkArtWorkMC.Visible = false;
                    lnkArtWorkSC.Visible = false;

                    lnkArtWorkPC.Disabled = true;
                    lnkArtWorkIB.Disabled = true;
                    lnkArtWorkIC.Disabled = true;
                    lnkArtWorkZB.Disabled = true;
                    lnkArtWorkMC.Disabled = true;
                    lnkArtWorkSC.Disabled = true;

                    hdfArtWorkPC.Value = string.Empty;
                    hdfArtWorkIB.Value = string.Empty;
                    hdfArtWorkIC.Value = string.Empty;
                    hdfArtWorkZB.Value = string.Empty;
                    hdfArtWorkMC.Value = string.Empty;
                    hdfArtWorkSC.Value = string.Empty;

                    lnkArtWorkPC.InnerHtml = string.Empty;
                    lnkArtWorkIB.InnerHtml = string.Empty;
                    lnkArtWorkIC.InnerHtml = string.Empty;
                    lnkArtWorkZB.InnerHtml = string.Empty;
                    lnkArtWorkMC.InnerHtml = string.Empty;
                    lnkArtWorkSC.InnerHtml = string.Empty;
                    break;
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    SelectedDtlPK = 0;
                    SelectedItemPK = 0;
                    SelectedCusItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    dvPerc.Visible = false;
                    break;

                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrDocSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ControlsEnum.QTYPOPUP:
                    txtPopUpTotalPcsCtn.Text = string.Empty;
                    txtPopUpPcsperUnit.Text = string.Empty;
                    txtPopUpOrderQty.Text = string.Empty;
                    txtPopUpQty.Text = string.Empty;
                    break;
                case ControlsEnum.TAXCHECKBOX:
                    chkSubTotal.Checked = false;
                    chkDiscount.Checked = false;
                    chkOtherCharges.Checked = true;
                    break;
            }
        }
        /// <summary>
        /// Reset Ddl Selected Value
        /// </summary>
        /// <param name="ddl"></param>
        private void ResetDdl(DropDownList ddl)
        {
            if (ddl != null)
            {
                ddl.ClearSelection();
                if (ddl.Items.Count > 1)
                    ddl.Items[1].Selected = true;
            }
        }
        /// <summary>
        /// Clear Ddl Values
        /// </summary>
        /// <param name="control"></param>
        private void ClearDdl(ControlsEnum control)
        {
            dtPageData = null;
            BindDropDownList(control);
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            AppTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.SO, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        private void AutoGenerateLotNo()
        {
            if (SaleOrderHeaderSession != null && !string.IsNullOrEmpty(SaleOrderHeaderSession.SOH_NO) && SaleOrderHeaderSession.SaleContractDetails != null)
            {
                GetFieldValues(ControlsEnum.CLIENTCODE);//For getting client id
                if (admAppConfigMstList != null)
                {
                    SaleOrderHeaderSession.SaleContractDetails.ForEach(
                        itm =>
                        {
                            if (string.IsNullOrEmpty(itm.SOD_LOT_NO))
                            {
                                if (clientCode == ClientCode.TMD.ToString())//There is no LOT_NO for Thai med ;As per syed sir 11/04/2015
                                {
                                    itm.SOD_LOT_NO = "";
                                }
                                if (clientCode == ClientCode.MMT.ToString())
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "AutogenerateLotNo").ToString() == "1") // Generate LOT No. based on the configuration entry
                                    {
                                        itm.SOD_LOT_NO = itm.SOD_ITEM_CODE.Length < 2 ? "" : (itm.SOD_ITEM_CODE.Substring(0, 2) + itm.ISD_NAT_SUF.ToString()
                                                        + SaleOrderHeaderSession.SOH_NO.Substring(LotNoFirstIndex, LotNoLastIndex)
                                                        + Convert.ToDateTime(SaleOrderHeaderSession.SOH_BOOKING_DATE).ToString("MMyy"));
                                        if (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "1")
                                        {
                                            itm.SOD_LOT_NO = itm.SOD_LOT_NO + "-" + itm.SOD_SL_NO;
                                        }
                                    }
                                }
                                else
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "AutogenerateLotNo").ToString() == "1") // Generate LOT No. based on the configuration entry
                                    {
                                        itm.SOD_LOT_NO = itm.SOD_ITEM_CODE.Length < 2 ? "" : (itm.SOD_ITEM_CODE.Substring(0, (itm.SOD_ITEM_CODE.Length > 7 ? 7 : itm.SOD_ITEM_CODE.Length)) + "-"
                                                    + Convert.ToDateTime(SaleOrderHeaderSession.SOH_BOOKING_DATE).ToString("MMyy") + "-"
                                                    + SaleOrderHeaderSession.SOH_NO.Substring(LotNoFirstIndex, LotNoLastIndex));
                                        if (GetGlobalResourceObject("ConfigurationsRes", "EnableSCLotnoSequence").ToString() == "1")
                                        {
                                            itm.SOD_LOT_NO = itm.SOD_LOT_NO + "-" + itm.SOD_SL_NO;
                                        }
                                    }
                                }
                            }
                        });
                }
                saleOrderHeaderObj = SaleOrderHeaderSession;
                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
            }
        }
        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
                while (tokenIndex < tokens.Count)
                {
                    string token = tokens[tokenIndex];
                    if (token == "(")
                    {
                        string subExpr = getSubExpression(tokens, ref tokenIndex);
                        operandStack.Push(StringToFormula(subExpr));
                        continue;
                    }
                    if (token == ")")
                    {
                        throw new ArgumentException("Mis-matched parentheses in expression");
                    }
                    //If this is an operator  
                    if (Array.IndexOf(_operators, token) >= 0)
                    {
                        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                        }
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operandStack.Push(double.Parse(token));
                    }
                    tokenIndex += 1;
                }

                while (operatorStack.Count > 0)
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                return operandStack.Pop();
            }
            catch
            {
                return 0;
            }
        }
        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }
        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
        private void SetDetailTax(SaleContractBO saleContractHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;
            if (rdbPackingSpec.Checked == true || rdbProduct.Checked == true)
            {
                if (double.TryParse(txtspecprice.Text, out quantity) && double.TryParse(txtspecqty.Text, out rate))
                {
                    if (txtAmount != null)
                    {
                        txtAmount.Text = GetFormattedCurrency(rate * quantity);

                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                        //SelectedCusItemPK = string.IsNullOrEmpty(hdfPrdPackSpec.Value) ? 0 : Convert.ToInt32(hdfPrdPackSpec.Value);
                        SelectedCusItemPK = 0;
                        SelectedItemPK = string.IsNullOrEmpty(hdfPrdPackSpec.Value) ? 0 : Convert.ToInt32(hdfPrdPackSpec.Value);
                        SetItemTax(saleContractHdr);
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
            else
            {
                if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtBrandQuantity.Text, out rate))
                {
                    if (txtAmount != null)
                    {
                        txtAmount.Text = GetFormattedCurrency(rate * quantity);

                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                        SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                        SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                        SetItemTax(saleContractHdr);
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
        }
        private bool SetItemTax(SaleContractBO saleContractHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            Double.TryParse(txtAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (saleContractHdr != null)
                {
                    saleOrderHeaderObj = saleContractHdr;
                    if (CurrSlNo > 0)
                    {

                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                            && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK && crt.SOD_SL_NO == CurrSlNo);
                    }
                    else
                    {
                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                            && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                    }
                    if (saleOrderDetailsObj != null)
                    {
                        if (saleOrderDetailsObj.TaxDtl != null)
                        {
                            var discDetail = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (SaleOrderTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (saleOrderDetailsObj.TaxDtl != null)
                        {
                            var taxDetail = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (SaleOrderTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            itmTax = saleOrderDetailsObj.TaxDtl.ToList().Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        saleOrderDetailsObj.SOD_AMOUNT = amount;
                        saleOrderDetailsObj.SOD_DISCOUNT = EnableItemDiscount == 0 ? 0 : discount;
                        saleOrderDetailsObj.SOD_TAX = EnableItemTax == 0 ? 0 : itmTax;
                        saleOrderDetailsObj.SOD_NET_AMOUNT = (amount - (EnableItemDiscount == 0 ? 0 : discount) + (EnableItemTax == 0 ? 0 : itmTax));
                        txtTotal.Text = saleOrderDetailsObj.SOD_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                        //TempSaleOrderHeaderSession = saleOrderHeaderObj;
                    }
                }
                return true;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                return false;
            }
        }
        private void SetSubTotal()
        {
            Label lblSubTotalFooter;
            SaleOrderHeaderSession.SOH_TOTAL_AMT = Convert.ToDouble(SaleOrderHeaderSession.SaleContractDetails.Sum(dtl => dtl.SOD_NET_AMOUNT));
            hdfSubTotal.Value = SaleOrderHeaderSession.SOH_TOTAL_AMT.ToString(hdfCurrencyFormat.Value);
            if (grdItemDetails.FooterRow != null)
            {
                lblSubTotalFooter = grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label;
                if (lblSubTotalFooter != null)
                {
                    lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = SaleOrderHeaderSession.SOH_TOTAL_AMT.ToString(hdfCurrencyFormat.Value);
                }
            }
        }
        private bool SetHdrTax()
        {
            double amount;
            double discount;
            double shipping;
            double adjust;
            amount = 0;
            shipping = 0;
            adjust = 0;

            if (SaleOrderHeaderSession != null)
            {
                saleOrderHeaderObj = SaleOrderHeaderSession;
                amount = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT);

                #region Discount
                discount = 0;
                if (saleOrderHeaderObj.TaxHdr != null)
                {
                    var discHeader = saleOrderHeaderObj.TaxHdr.Where(hdr => hdr.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (SaleOrderTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    discount = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                saleOrderHeaderObj.SOH_TOTAL_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;
                #endregion
                #region Shipping Charge
                if (saleOrderHeaderObj.TaxHdr != null)
                {
                    string[] CatPKS = GetLocalResourceObject("OtherChargeCategoryValues").ToString().Split(',');// 2-> Shipment, 5->Duties, 6-> Surchages;
                    //var shippingHeader = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Shipping));
                    var shippingHeader = saleOrderHeaderObj.TaxHdr.Where(quotation =>CatPKS.Contains( quotation.SLT_TAX_CATEGORY.ToString()));
                    foreach (SaleOrderTaxHdr taxHdrObj in shippingHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula) && taxFormula != "0")
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    shipping = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = saleOrderHeaderObj.TaxHdr.Where(quotation =>CatPKS.Contains(quotation.SLT_TAX_CATEGORY.ToString())).Sum(quotation => quotation.SLT_TAX_AMT);
                    //shipping = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                txtShipping.Text = txtShipping.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = shipping;
                #endregion

                #region Tax
                if (EnableItemTax != 2)
                {
                    //Add Other Charges Based on configuration
                    if (IsTaxForOtherCharge)
                    {
                        amount += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                    }
                }

                if (saleOrderHeaderObj.TaxHdr != null)
                {
                    var taxHeader = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (SaleOrderTaxHdr taxHdrObj in taxHeader)
                    {
                        if (EnableItemTax == 2)
                        {
                            amount = 0;
                            if (taxHdrObj.SLT_HAS_SUB_TOTAL == 1)
                                amount += Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT);
                            if (taxHdrObj.SLT_HAS_DISCOUNT == 1)
                                amount = txtHdrDiscount.Text != string.Empty ? amount - Convert.ToDouble(txtHdrDiscount.Text) : amount;
                            if (taxHdrObj.SLT_HAS_OTHER_CHARGE == 1)
                                amount += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                        }
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            //Commented for RBPL rounding issue
                            //taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }

                    if (EnableItemTax != 1)
                        saleOrderHeaderObj.SOH_TOTAL_TAX = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT);
                    else
                        saleOrderHeaderObj.SOH_TOTAL_TAX = 0;
                }
                #endregion

                txtHdrTax.Text = txtHdrTax.ToolTip = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString(hdfCurrencyFormat.Value);
                //double.TryParse(txtShipping.Text, out shipping);

                double.TryParse(txtPriceAdj.Text, out adjust);
                saleOrderHeaderObj.SOH_TOTAL_ADJUST = adjust;
                saleOrderHeaderObj.SOH_NET_AMOUNT = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT) - saleOrderHeaderObj.SOH_TOTAL_DISCOUNT + saleOrderHeaderObj.SOH_TOTAL_TAX
                    + saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE + saleOrderHeaderObj.SOH_TOTAL_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                SaleOrderHeaderSession = saleOrderHeaderObj;
                TempSaleOrderHeaderSession = saleOrderHeaderObj;
                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotal", "CalculateTotal();", true);
            }
            return true;
        }
        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return taxAmt;
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedNumberWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithComma.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
        }
        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {
            if (GetGlobalResourceObject("ConfigurationsRes", "SCCaseMarkShow").ToString() == "1")
            {
                grdItemDetails.Columns[18].Visible = true;
            }
            else
            {
                grdItemDetails.Columns[18].Visible = false;
            }

            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "TAX");
            if (dt != null && dt.Rows.Count > 0)
                IsTaxInSBU = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;

            //Set Visibility of ddlSaleOrderSubtype.  No need for the field in IGCL.          
            DataTable dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALE CONTRACT", "EnableSubType");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                hdfEnableSubType.Value = dtConfig.Rows[0]["ACF_VALUE"].ToString();
            }
            IsTaxForOtherCharge = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            ShipdateUpdation = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShipdateUpdation")));
            SendMailWithAttachment = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SendMailWithAttachmnt_SC")));
            #region Carton decimal settings
            DataTable dtCartonConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CARTON SETTINGS", "CARTON DECIMAL");
            if (dtCartonConfig != null && dtCartonConfig.Rows.Count > 0)
            {
                CartonDecimal = Convert.ToInt32(dtCartonConfig.Rows[0]["ACF_VALUE"].ToString());
            }
            #endregion

            //show or hide To Port Ddl based on Configuraion - For Demo IN 
            IsToPortDdlShow = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsToPortDdlShow")));
            IsEnableTypeFilter = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "IsEnableTypeFilter"));
            hdfSaleOrderType.Value = IsEnableTypeFilter.ToString();

            IsDeliveryTermsCheck = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsDeliveryTermsCheck")));
            hdfInitMfgDate.Value = GetGlobalResourceObject("ConfigurationsRes", "InitMfgDate").ToString();
            hdfIsCrmCustomer.Value = GetGlobalResourceObject("ConfigurationsRes", "IsCrmEnabled").ToString();
        }
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
        }

        private string GetUploadSavePath()
        {
            string configuredPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
            if (string.IsNullOrEmpty(configuredPath))
            {
                string uploadFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Upload");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                return uploadFolder + Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(configuredPath))
                Directory.CreateDirectory(configuredPath);

            return configuredPath.EndsWith("\\") || configuredPath.EndsWith("/")
                ? configuredPath
                : configuredPath + Path.DirectorySeparatorChar;
        }

        private void SaveUploadedFile(SaleOrderUploads uploadObj)
        {
            if (uploadObj == null || string.IsNullOrEmpty(uploadObj.AttachmentFileName) || !fupUpload.HasFile)
                return;

            string filePath = Path.Combine(GetUploadSavePath(), uploadObj.AttachmentFileName);
            fupUpload.PostedFile.SaveAs(filePath);
        }

        private void EnsureUploadedFilesSaved()
        {
            if (SOUploadList == null || SOUploadList.Count == 0)
                return;

            foreach (SaleOrderUploads obj in SOUploadList)
            {
                if (obj == null || string.IsNullOrEmpty(obj.AttachmentFileName))
                    continue;

                string filePath = Path.Combine(GetUploadSavePath(), obj.AttachmentFileName);
                if (File.Exists(filePath))
                    continue;

                if (FileDetailsList != null)
                {
                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                    if (fileDetailsObj != null && fileDetailsObj.SoFile != null)
                    {
                        try
                        {
                            fileDetailsObj.SoFile.SaveAs(filePath);
                        }
                        catch (ObjectDisposedException)
                        {
                            // The uploaded request stream is no longer available. New uploads are saved immediately.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check Total CBM Exist the limit and show appropriate Message
        /// </summary>
        /// //SaveSubmitflag=> 0 - AddItem ,1 - Save, 2-WrkflowSubmit,3-Delete
        /// <returns></returns>
        private bool IsTotalCBMExceeds(int SaveSubmitDeleteflag, string totalCBM)
        {
            bool IsCBMExceeds = false;
            bool ShowCBMMsg = true;
            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ZERO && SaveSubmitDeleteflag == 2)//Submit button do not need CBM alert msg
            {
                ShowCBMMsg = false;
            }
            if (hdfIsSaveSubmit.Value != CommonConstants.SELECTVAL && ShowCBMMsg)// When 'Cancel & Submit' button clicks CBM message is displaying; which is not required
            {
                #region IsTotalCBMExceeds
                string Msg = "";
                #region Fetching Container Type details and Sorting into CBM Ascending order
                GetFieldValues(ControlsEnum.CONTAINERTYPEDETAILS);
                DataTable dt2 = dtPageData.Clone();
                dt2.Columns["CON_DATA"].DataType = Type.GetType("System.Double");//Convert  dataType string to Double
                foreach (DataRow dr in dtPageData.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["CON_DATA"].ToString()))
                    {
                        dt2.ImportRow(dr);
                    }
                }
                dt2.AcceptChanges();
                DataView dv = dt2.DefaultView;
                dv.Sort = "CON_DATA ASC";
                DataTable dtSortedContTypeDet = dv.ToTable();
                #endregion
                #region Setting Alert Message w.r.to CBM
                for (int i = 0; i < dtSortedContTypeDet.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dtSortedContTypeDet.Rows[i]["CON_DATA"].ToString()))
                    {
                        if (i + 1 >= dtSortedContTypeDet.Rows.Count)
                        {
                            if (Convert.ToDouble(totalCBM) > Convert.ToDouble(dtSortedContTypeDet.Rows[i]["CON_DATA"]))
                            {
                                Msg = GetLocalResourceObject("Msg_CBM_Confirm").ToString() + dtSortedContTypeDet.Rows[i]["CON_NAME"] + " container capacity.Do you want to continue?";
                                IsCBMExceeds = true;
                                hdfOldCBMLimitPK.Value = hdfNewCBMLimitPK.Value;
                                hdfNewCBMLimitPK.Value = dtSortedContTypeDet.Rows[i]["CON_PK"].ToString();
                            }
                        }
                        else
                        {
                            if (Convert.ToDouble(totalCBM) > Convert.ToDouble(dtSortedContTypeDet.Rows[i]["CON_DATA"]) && Convert.ToDouble(totalCBM) <= Convert.ToDouble(dtSortedContTypeDet.Rows[i + 1]["CON_DATA"]))
                            {
                                Msg = GetLocalResourceObject("Msg_CBM_Confirm").ToString() + dtSortedContTypeDet.Rows[i]["CON_NAME"] + " container capacity.Do you want to continue?";
                                IsCBMExceeds = true;
                                hdfOldCBMLimitPK.Value = hdfNewCBMLimitPK.Value;
                                hdfNewCBMLimitPK.Value = dtSortedContTypeDet.Rows[i]["CON_PK"].ToString();
                            }
                        }
                    }
                }
                #endregion
                if (!IsCBMExceeds)//If CBM value is not exceeds then reset the hiddenfield CBMLimit Pk
                {
                    hdfOldCBMLimitPK.Value = "0";
                    hdfNewCBMLimitPK.Value = "0";
                }
                if (SaveSubmitDeleteflag == 1)//Save.
                {
                    if (hdfIsWrkflwClose.Value == "1") { isWarnedCBM = 0; }
                    if (!IsCBMExceeds || (isWarnedCBM == 1))
                    {
                        IsCBMExceeds = false;
                    }
                    else
                    {
                        isWarnedCBM = 1;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCBMCheckConfirming", "$(document).ready(function(){ShowCBMCheckConfirming('" + Msg + "',1);});", true);
                    }
                }
                else if (SaveSubmitDeleteflag == 2)//WrkflowSubmit
                {
                    if (hdfIsWrkflwClose.Value == "1") { isWarnedCBM = 0; }
                    if (!IsCBMExceeds || (isWarnedCBM == 2))
                    {
                        IsCBMExceeds = false;
                    }
                    else
                    {
                        isWarnedCBM = 2;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCBMCheckConfirming", "$(document).ready(function(){ShowCBMCheckConfirming('" + Msg + "',2);});", true);
                    }
                }
                else
                {
                    if (!IsCBMExceeds || (hdfOldCBMLimitPK.Value == hdfNewCBMLimitPK.Value))
                    {
                        IsCBMExceeds = false;
                    }
                    else
                    {
                        if (SaveSubmitDeleteflag == 0)//Add Item
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCBMCheckConfirming", "$(document).ready(function(){ShowCBMCheckConfirming('" + Msg + "',0);});", true);
                        }
                    }
                }
                #endregion
            }
            return IsCBMExceeds;
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
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            path = path + (Request.Url.Query.IndexOf('&') > 0 ? Request.Url.Query.Substring(0, Request.Url.Query.IndexOf('&')).ToLower()
                : Request.Url.Query.ToLower());

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ProcessID = ucrWrkf.ProcessID;
            }

        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }

        #endregion
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                #region For Line Movement
                int SlNo, lineIndex;
                #endregion
                int? result;
                string action;
                DropDownList ddlWkfAction;
                string addr;

                double qty;
                double rate;
                Label lblSubTotal;
                SaleOrderTaxHdr tempSaleOrderTaxHdrObj = null;
                SaleOrderTaxHdr saleOrderTaxHdrObj;
                List<SaleOrderTaxHdr> saleOrderTaxHdrList;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                bool isValidDiscAm = true;
                int brandArtPK;
                FileInfo tempFileInfoObj;
                int selectedItemPK;
                int retRefID = 0;
                string savePath = string.Empty;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSaleOrderType")
                    {
                        commonActions = ActionsEnum.SOSUBTYPE;
                    }
                    else if (((DropDownList)sender).ID == "ddlArtWork")
                    {
                        commonActions = ActionsEnum.ARTWORKSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlNotifyParty")
                    {
                        commonActions = ActionsEnum.NOTIFYPARTYSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlConsigneeDetails")
                    {
                        commonActions = ActionsEnum.CONSIGNEESELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlAgent")
                    {
                        commonActions = ActionsEnum.AGENTSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlCustAddress")
                    {
                        commonActions = ActionsEnum.ADDRESSSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlDeliveryTerms")
                    {
                        commonActions = ActionsEnum.DELTERMSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlShipmentTerms")
                    {
                        commonActions = ActionsEnum.SHIPMENTTERMSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlPaymentTerms")
                    {
                        commonActions = ActionsEnum.PAYTERMSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlSpecialCause")
                    {
                        commonActions = ActionsEnum.SPECAUSESELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtRate")
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECKEDCHANGED;
                }
                if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rdbPackingSpec")
                    {
                        //PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        commonActions = ActionsEnum.CHANGETYPE;
                        //SearchType = (int)SearchTypeEnum.Customer;
                    }
                    else if (((RadioButton)sender).ID == "rdbProduct")
                    {
                        //PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        commonActions = ActionsEnum.PRODUCT;
                        //SearchType = (int)SearchTypeEnum.Customer;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.CHANGETYPE:

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "ShowHidePackingMaterialDetails();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "ShowHideItemDetails();", true);
                        ResetForm(ControlsEnum.PRODUCT);
                        lbl_Prd_PackSpeck.Visible = true;
                        lbl_product.Visible = false;
                        break;

                    case ActionsEnum.PRODUCT:
                        lbl_Prd_PackSpeck.Visible = false;
                        lbl_product.Visible = true;
                        ResetForm(ControlsEnum.PRODUCT);
                        break;

                    #region Order priority Up and Down
                    case ActionsEnum.MOVEUP:
                        saleOrderDetailsList = SaleOrderHeaderSession.SaleContractDetails;
                        SlNo = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        currLine = saleOrderDetailsList.Where(rl => rl.SOD_SL_NO == SlNo).First();
                        lineIndex = saleOrderDetailsList.FindIndex(rl => rl.SOD_SL_NO == SlNo);
                        nextLine = lineIndex == 0 ? null : saleOrderDetailsList.ElementAt(lineIndex - 1);
                        if (nextLine != null)
                        {
                            int currentlineNumber = currLine.SOD_SL_NO;
                            currLine.SOD_SL_NO = nextLine.SOD_SL_NO;
                            nextLine.SOD_SL_NO = currentlineNumber;
                        }
                        saleOrderDetailsList = saleOrderDetailsList.OrderBy(rl => rl.SOD_SL_NO).ToList();
                        SaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                        BindGrid(ControlsEnum.REBINDSALEORDER);
                        break;
                    case ActionsEnum.MOVEDOWN:
                        saleOrderDetailsList = SaleOrderHeaderSession.SaleContractDetails;
                        SlNo = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        currLine = saleOrderDetailsList.Where(rl => rl.SOD_SL_NO == SlNo).First();
                        lineIndex = saleOrderDetailsList.FindIndex(rl => rl.SOD_SL_NO == SlNo);
                        nextLine = lineIndex == (saleOrderDetailsList.Count - 1) ? null : saleOrderDetailsList.ElementAt(lineIndex + 1);
                        if (nextLine != null)
                        {
                            int currentlineNumber = currLine.SOD_SL_NO;
                            currLine.SOD_SL_NO = nextLine.SOD_SL_NO;
                            nextLine.SOD_SL_NO = currentlineNumber;
                        }
                        saleOrderDetailsList = saleOrderDetailsList.OrderBy(rl => rl.SOD_SL_NO).ToList();
                        SaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                        BindGrid(ControlsEnum.REBINDSALEORDER);
                        break;
                    #endregion

                    #region LineitemTax in Amount Change
                    case ActionsEnum.TOOLTIP:

                        if (hdfErrorMsgType.Value == "1")//qtyDespatched > currentQty
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Qty_Despatch").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (hdfErrorMsgType.Value == "2")//qtyInvoiced > currentQty
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Qty_Invoice").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        if ((string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value)) > 0)
                        {
                            //if (hdfisItemHaveTax.Value.ToString() == "1")
                            //{
                            if (TempSaleOrderHeaderSession.SaleContractDetails.Any(tm => tm.TaxDtl != null && tm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)))
                            {
                                double amountItemwise;
                                amountItemwise = Convert.ToDouble(txtAmount.Text);
                                saleOrderHeaderObj = TempSaleOrderHeaderSession;
                                SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                                SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                                if (CurrSlNo > 0)
                                {
                                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                }
                                else
                                {
                                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                                }
                                if (saleOrderHeaderObj.SaleContractDetails.Where(itm => itm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)).Count() == 1)
                                {
                                    if (saleOrderDetailsObj != null)
                                    {
                                        //CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                                    }
                                }
                                if (saleOrderDetailsObj.TaxDtl != null)
                                {
                                    var taxDetail = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                                    foreach (SaleOrderTaxHdr taxHdrObj in taxDetail)
                                    {
                                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                        if (!string.IsNullOrEmpty(taxFormula))
                                        {
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amountItemwise.ToString());
                                            //Commented for RBPL rounding issue
                                            //taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                    }
                                    if (CurrSlNo <= 0)
                                    {
                                        saleOrderHeaderObj.SaleContractDetails.LastOrDefault().SOD_TAX = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT);
                                    }
                                    //If disc Available ?
                                    if (saleOrderDetailsObj.TaxDtl != null)
                                    {
                                        var discDetail = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == (int)TaxType.Discount);
                                        foreach (SaleOrderTaxHdr taxHdrObj in discDetail)
                                        {
                                            string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                            if (!string.IsNullOrEmpty(taxFormula))
                                            {
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", amountItemwise.ToString());
                                                taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }
                                            else if (taxHdrObj.SLT_DISC_PERC > 0)
                                            {
                                                double Amount = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0;
                                                taxHdrObj.SLT_TAX_AMT = Math.Round((Amount * taxHdrObj.SLT_DISC_PERC) / 100, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }
                                        }
                                        //discount = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                                    }
                                    //netAmount = amount - discount;



                                }
                                txtTax.Text = txtTax.ToolTip = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT).ToString();
                                txtDiscount.Text = txtDiscount.ToolTip = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT).ToString();

                                ResetForm(ControlsEnum.TAXPOPUPGRID);
                                IsHeaderTax = false;
                                IsEditMode = false;
                            }
                            //}
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Brand").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }


                        if (hdfFocusPdctAdd.Value == "1")
                        {
                            txtRate.Focus(); //txtReqByDate.Focus();
                        }
                        else if (hdfFocusPdctAdd.Value == "2")
                        {
                            txtLotNo.Focus();
                            if (!txtLotNo.Visible)
                                ddlArtWork.Focus(); // txtDtlRemark.Focus();

                            //txtctnRate.Focus();
                        }
                        //txtctnRate.Focus();

                        break;
                    #endregion
                    #region Customer Selected
                    case ActionsEnum.CUSTOMERSELECTED:

                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            ResetForm(ControlsEnum.QUOTATION);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                }
                                txtBuyerAddress.Text = addr;

                                hdfCusAddress.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                hdfCusCountry.Value = dsPageData.Tables[0].Rows[0]["CUS_COUNTRY"].ToString();
                                hdfCusCountryText.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                hdfCusZip.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ZIP"].ToString());
                                hdfCusPhone.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_PHONE"].ToString());
                                hdfCusMobile.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_MOBILE"].ToString());
                                hdfCusFax.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_FAX"].ToString());
                                hdfCusEmail.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_EMAIL"].ToString());

                                CusCurrencyCode = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString());
                                txtPortofDischarge.Text = txtToPort.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SHIP_TO_PORT"].ToString());
                                hdfToPortID.Value = CommonConstants.SELECT_VALUE_ZERO;
                                if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value) && ddlSaleOrderType.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString()) != null)
                                    ddlSaleOrderType.SelectedValue = dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString();
                                GetFieldValues(ControlsEnum.SOSUBTYPE);
                                SetFieldValues(ControlsEnum.SOSUBTYPE);


                                if (Convert.ToByte(dsPageData.Tables[0].Rows[0]["CUS_ADV_REQUIRED"]) == 0)
                                { chkNeedAdvPay.Checked = true; }
                                else
                                { chkNeedAdvPay.Checked = false; }
                                chkNeedAdvPay.Checked = Convert.ToBoolean(dsPageData.Tables[0].Rows[0]["CUS_ADV_REQUIRED"]);



                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                if (exchangeRate > 0)
                                {
                                    hdfExchangeRate.Value = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                                    txtExchangeRate.Text = hdfExchangeRate.Value;
                                }
                                else
                                {
                                    //txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                    //hdfCurrency.Value = "0";
                                    txtExchangeRate.Text = string.Empty;
                                }

                                //Not allowed to edit exchange rate while currency same as base currency                           
                                if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value))
                                {
                                    if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value)) //Convert.ToInt32(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"]) == ((byte)SalesInvoiceType.Domestic) ||
                                    {
                                        txtExchangeRate.Enabled = false;
                                    }
                                    else
                                    {
                                        txtExchangeRate.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                ResetForm(ControlsEnum.CUSTOMER);
                            }
                            ResetForm(ControlsEnum.CUSTOMERCONTRACT);

                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            SetFieldValues(ControlsEnum.NOTIFYPARTY);
                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            SetFieldValues(ControlsEnum.CONSIGNEE);
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            //SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.SHIPMENTTERMS);
                            SetFieldValues(ControlsEnum.SHIPMENTTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);

                            GetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                            SetFieldValues(ControlsEnum.CUSTOMERCONTRACT);

                            GetFieldValues(ControlsEnum.CBMCONFIG);
                            SetFieldValues(ControlsEnum.CBMCONFIG);

                            //txtPONo.Focus();
                            ddlSaleOrderType.Focus();

                            //To Port
                            GetFieldValues(ControlsEnum.TOPORT);
                            SetFieldValues(ControlsEnum.TOPORT);

                            if (IsToPortDdlShow == true && CusCurrencyCode == "INR")
                            {
                                ddlToPort.Visible = true;
                                txtToPort.Visible = false;
                            }
                            else
                            {
                                txtToPort.Visible = true;
                                ddlToPort.Visible = false;
                            }
                        }
                        else
                        {
                            txtCustomer.Focus();
                            ResetForm(ControlsEnum.CUSTOMER);
                            ResetForm(ControlsEnum.CUSTOMERCONTRACT);
                        }
                        SaleOrderHeaderSession.SaleContractDetails = new List<SaleContractDetailsBO>();
                        saleOrderHeaderObj = SaleOrderHeaderSession;
                        //SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                        if (EnableItemTax != 1 && hdfCustomer.Value != string.Empty && Convert.ToInt32(hdfCustomer.Value) > 0)
                        {
                            //Fill tax pop default
                            SaleOrderHeaderSession.TaxHdr = new List<SaleOrderTaxHdr>();
                            GetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                        }
                        isItemBind = true;
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        if (rdbPackingSpec.Checked == true)
                        {
                            //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(0);", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePackingMaterialDetails", "ShowHidePackingMaterialDetails();", true);
                        }
                        break;
                    #endregion
                    #region Exchange Rate
                    case ActionsEnum.EXCHANGERATE:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        if (exchangeRate > 0)
                        {
                            hdfExchangeRate.Value = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                            txtExchangeRate.Text = hdfExchangeRate.Value;
                        }
                        else
                        {
                            txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                            hdfCurrency.Value = "0";
                            txtExchangeRate.Text = string.Empty;
                            litErrorMsg.Text = GetLocalResourceObject("Err_ExchangeRate").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        //if (saleOrderHeaderObj != null)
                        //{
                        //    //Not allowed to edit exchange rate while currency same as base currency                           
                        //    if (saleOrderHeaderObj.SOH_TYPE == ((byte)SalesInvoiceType.Domestic) || currentUser.BaseCurrency == saleOrderHeaderObj.SOH_CURRENCY)
                        //    {
                        //        txtExchangeRate.Enabled = false;
                        //    }
                        //    else
                        //    {
                        //        txtExchangeRate.Enabled = true;
                        //    }
                        //}
                        break;
                    #endregion
                    #region Brand Change
                    case ActionsEnum.PRODUCTSELECTED:
                        ResetForm(ControlsEnum.PACKINGSPEC);
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                        }
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty)
                        {
                            custprodPK = Convert.ToInt32(hdfBrand.Value);
                            GetFieldValues(ControlsEnum.CUSTOMERPRODUCT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                txtBrandCode.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CIM_BRAND_CODE"].ToString());
                                txtPacking.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CIM_PACKING_SPEC_NAME"].ToString());
                                txtPacking.ToolTip = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CIM_PACKING_SPEC_NAME"].ToString());
                                hdfPackingSpec.Value = dtPageData.Rows[0]["APS_PK"].ToString();
                                hdfPackingText.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PACKING_TEXT"].ToString());

                                if (GetGlobalResourceObject("ConfigurationsRes", "ShowSCRemarksValue").ToString() == "1")
                                {
                                    string Itm_Ref1 = dtPageData.Rows[0]["ITM_REF1"].ToString() == string.Empty ? "" : GetLocalResourceObject("510KNo").ToString() + " " + dtPageData.Rows[0]["ITM_REF1"].ToString();
                                    string MDLNo = dtPageData.Rows[0]["ITM_REF2"].ToString() == string.Empty ? "" : GetLocalResourceObject("MDLNo").ToString() + " " + dtPageData.Rows[0]["ITM_REF2"].ToString();
                                    txtDtlRemark.Text = Itm_Ref1 + (Itm_Ref1 == string.Empty && MDLNo == string.Empty ? " " : ", ") + MDLNo;
                                }

                                if (!dtPageData.Rows[0]["CIM_SALE_UOM"].Equals(DBNull.Value))
                                {
                                    hdfBrandUOM.Value = dtPageData.Rows[0]["CIM_SALE_UOM"].ToString();
                                }
                                if (!dtPageData.Rows[0]["SOD_SALE_UOM_TEXT"].Equals(DBNull.Value))
                                {
                                    txtBrandUOM.Text = dtPageData.Rows[0]["SOD_SALE_UOM_TEXT"].ToString();
                                    txtBrandRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + dtPageData.Rows[0]["SOD_SALE_UOM_TEXT"].ToString();
                                }
                                HdfIsPcs.Value = dtPageData.Rows[0]["CIM_UOM_IS_PCS"].ToString();
                                if (HdfIsPcs.Value == "1")
                                {
                                    txtctnRate.Enabled = true;
                                    txtctnRate.CssClass = "medium select-half";
                                }
                                else
                                {
                                    txtctnRate.Enabled = false;
                                    txtctnRate.CssClass += " input-disabled";
                                }
                                if (!dtPageData.Rows[0]["CIM_SALE_UOM_CONV"].Equals(DBNull.Value))
                                {
                                    hdfBrandUOMConversion.Value = dtPageData.Rows[0]["CIM_SALE_UOM_CONV"].ToString();
                                }

                                //hdfArtWork.Value = dtPageData.Rows[0]["PIM_PK"].ToString();
                                //chkNewArtWork.Checked = string.IsNullOrEmpty(hdfArtWork.Value);
                                hdfCBM.Value = dtPageData.Rows[0]["CBM"].Equals(DBNull.Value) ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["CBM"].ToString();
                                if (!dtPageData.Rows[0]["PIM_PK"].Equals(DBNull.Value))
                                {
                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["PC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkPC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["PC_DOC_PATH"].ToString()))
                                            lnkArtWorkPC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkPC.Disabled = false;
                                            lnkArtWorkPC.HRef = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PC_ART_WORK"].ToString());
                                        hdfArtWorkPC.Value = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkPC.HRef = "#";
                                        lnkArtWorkPC.Visible = false;
                                        lnkArtWorkPC.Disabled = true;
                                        hdfArtWorkPC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IB_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkIB.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["IB_DOC_PATH"].ToString()))
                                            lnkArtWorkIB.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkIB.Disabled = false;
                                            lnkArtWorkIB.HRef = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IB_ART_WORK"].ToString());
                                        hdfArtWorkIB.Value = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkIB.HRef = "#";
                                        lnkArtWorkIB.Visible = false;
                                        lnkArtWorkIB.Disabled = true;
                                        hdfArtWorkIB.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkIC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["IC_DOC_PATH"].ToString()))
                                            lnkArtWorkIC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkIC.Disabled = false;
                                            lnkArtWorkIC.HRef = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IC_ART_WORK"].ToString());
                                        hdfArtWorkIC.Value = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkIC.HRef = "#";
                                        lnkArtWorkIC.Visible = false;
                                        lnkArtWorkIC.Disabled = true;
                                        hdfArtWorkIC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkZB.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_DOC_PATH"].ToString()))
                                            lnkArtWorkZB.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkZB.Disabled = false;
                                            lnkArtWorkZB.HRef = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ZB_ART_WORK"].ToString());
                                        hdfArtWorkZB.Value = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkZB.HRef = "#";
                                        lnkArtWorkZB.Visible = false;
                                        lnkArtWorkZB.Disabled = true;
                                        hdfArtWorkZB.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["MC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkMC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["MC_DOC_PATH"].ToString()))
                                            lnkArtWorkMC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkMC.Disabled = false;
                                            lnkArtWorkMC.HRef = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["MC_ART_WORK"].ToString());
                                        hdfArtWorkMC.Value = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();

                                    }
                                    else
                                    {
                                        lnkArtWorkMC.HRef = "#";
                                        lnkArtWorkMC.Visible = false;
                                        lnkArtWorkMC.Disabled = true;
                                        hdfArtWorkMC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["SC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkSC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["SC_DOC_PATH"].ToString()))
                                            lnkArtWorkSC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkSC.Disabled = false;
                                            lnkArtWorkSC.HRef = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["SC_ART_WORK"].ToString());
                                        hdfArtWorkSC.Value = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkSC.HRef = "#";
                                        lnkArtWorkSC.Visible = false;
                                        lnkArtWorkSC.Disabled = true;
                                        hdfArtWorkSC.Value = string.Empty;
                                    }
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.PACKINGSPEC);
                                }

                                txtBrandQuantity.Text = txtQty.Text = hdfQtyTemp.Value = GetFormattedNumber(dtPageData.Rows[0]["CIM_LAST_ORDR_QTY"].Equals(DBNull.Value) ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["CIM_LAST_ORDR_QTY"].ToString());
                                hdfUOM.Value = dtPageData.Rows[0]["UOM_PK"].ToString();
                                txtUOM.Text = dtPageData.Rows[0]["UOM_CODE"].ToString();
                                hdfProduct.Value = dtPageData.Rows[0]["ITM_PK"].ToString();
                                txtProduct.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_CODE"].ToString());
                                hdfProductName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_NAME"].ToString());
                                double totalPieces = Convert.ToDouble(dtPageData.Rows[0]["APS_TOTAL_PCS"].Equals(DBNull.Value)
                                    ? CommonConstants.SELECT_VALUE_ONE : dtPageData.Rows[0]["APS_TOTAL_PCS"].ToString());
                                totalPieces = totalPieces > 1 ? totalPieces : 1;
                                txtTotalPiecesCtn.Text = Math.Floor(totalPieces).ToString();
                                hdfPcsperCtn.Value = Math.Floor(totalPieces).ToString();
                                double totalPcsInBox = Convert.ToDouble(dtPageData.Rows[0]["APS_IB_PCS"].Equals(DBNull.Value)
                                    ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["APS_IB_PCS"].ToString());
                                totalPcsInBox = totalPcsInBox > 0 ? totalPcsInBox : 0;
                                hdfTotalPcsInBox.Value = Math.Floor(totalPieces).ToString();
                                if (dtPageData.Rows[0]["CIM_SALE_UOM_CONV"] != null && dtPageData.Rows[0]["CIM_SALE_UOM_CONV"].ToString() != string.Empty)
                                    hdfSaleUOMPcs.Value = dtPageData.Rows[0]["CIM_SALE_UOM_CONV"].ToString();

                                brandArtPK = dtPageData.Rows[0]["PIM_PK"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dtPageData.Rows[0]["PIM_PK"]);

                                if (ShowSCAdditionalPackDtls == 1)
                                {
                                    ddlstrappingColor.SelectedIndex = ddlstrappingColor.Items.IndexOf(ddlstrappingColor.Items.FindByValue(dtPageData.Rows[0]["CIM_STRAPPING_COLOUR"].ToString()));
                                    chkStrapping.Checked = Convert.ToBoolean(dtPageData.Rows[0]["CIM_STRAPPING"].ToString());
                                    chkLayering.Checked = Convert.ToBoolean(dtPageData.Rows[0]["CIM_LAYERING"].ToString());
                                    txtNoofLayers.Text = dtPageData.Rows[0]["CIM_NO_LAYERS"].ToString();
                                    txtPcsLayers.Text = dtPageData.Rows[0]["CIM_PIECES_LAYER"].ToString();
                                }

                                GetFieldValues(ControlsEnum.PACKINGSPEC);
                                artWorkPK = brandArtPK;
                                SetFieldValues(ControlsEnum.PACKINGSPEC);



                                if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                {
                                    custprodPK = Convert.ToInt32(hdfBrand.Value);
                                    bookingDate = Convert.ToDateTime(txtBookingDate.Text);
                                    GetFieldValues(ControlsEnum.BRANDRATE);
                                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                                    {
                                        txtRate.Text = GetFormattedRate(dtPageData.Rows[0]["BRD_RATE"].ToString());
                                        if (double.TryParse(txtBrandQuantity.Text, out qty) && double.TryParse(txtRate.Text, out rate))
                                        {
                                            txtAmount.Text = GetFormattedCurrency(qty * rate);
                                        }
                                        else
                                            txtAmount.Text = GetFormattedCurrency(0);
                                    }
                                    else
                                    {
                                        txtRate.Text = GetFormattedRate(0);
                                        txtAmount.Text = GetFormattedCurrency(0);
                                    }
                                }
                                else
                                {
                                    txtRate.Text = GetFormattedRate(0);
                                    txtAmount.Text = GetFormattedCurrency(0);
                                }
                            }
                            else
                                ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                        }
                        else
                            ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                        //Fill tax pop default                        
                        GetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                        if (EnableItemTax == 1 && (dtPageData == null || dtPageData.Rows.Count == 0) && Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableCustomerTaxforSChdrDtl")) == 1)
                        {
                            txtTax.Text = txtDiscount.Text = string.Empty;
                            GetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                        }
                        SetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);

                        //include in list
                        saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;

                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                        SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                        SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                        if (CurrSlNo > 0)
                        {
                            saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                        }
                        else
                        {
                            if (hdfisItemHaveTax.Value.ToString() != "0")
                            {
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                       TempSaleOrderHeaderSession.SaleContractDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                       && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                            }
                        }
                        if (saleOrderDetailsObj == null)
                        {
                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            {
                                IsBrand = true; hdfIsBrandYes.Value = "1";
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                IsBrand = false; hdfIsBrandYes.Value = "0";
                            }
                        }
                        txtBrand.Focus(); //txtBrandQuantity.Focus(); //txtQty.Focus();
                        break;
                    #endregion
                    # region Packing Materail Selected
                    case ActionsEnum.PACKSPECSELECTED:
                        //if (hdfPackingSpec.Value != CommonConstants.SELECT_VALUE_ZERO && hdfPackingSpec.Value != string.Empty)
                        //if (hdfPrdCategory.Value != CommonConstants.SELECT_VALUE_ZERO && hdfPackingSpec.Value != string.Empty && hdfPrdType.Value != CommonConstants.SELECT_VALUE_ZERO)
                        //{
                        packspecPK = Convert.ToInt32(hdfPrdPackSpec.Value);
                        //packspecPK = 3;
                        GetFieldValues(ControlsEnum.PACKSPECSELECTED);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfPrdPackSpecPK.Value = dtPageData.Rows[0]["ITM_PK"].ToString();
                            txtSpecCode.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_CODE"].ToString());
                            hdfspeccode.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_CODE"].ToString());
                            txtspecqty.Text = GetFormattedCurrency(0);
                            txtspecamount.Text = GetFormattedCurrency(0);
                            txtspecprice.Text = GetFormattedCurrency(0);
                            hdfspecuom.Value = dtPageData.Rows[0]["ITM_UOM"].ToString();
                            hdfspecsaleuom.Value = dtPageData.Rows[0]["UOM_CODE"].ToString();
                            hdfspecuomcode.Value = dtPageData.Rows[0]["UOM_CODE"].ToString();
                            SPECQTY.Text = dtPageData.Rows[0]["UOM_CODE"].ToString();
                            hdfapstotalpcs.Value = "0";
                            // saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;
                            if (rdbPackingSpec.Checked == true)
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(0);", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePackingMaterialDetails", "ShowHidePackingMaterialDetails();", true);
                            }
                        }
                        //}
                        break;
                    #endregion

                    #region Booking Date Change
                    case ActionsEnum.BOOKINGDATECHANGE:
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty
                            && !string.IsNullOrEmpty(txtBookingDate.Text))
                        {
                            custprodPK = Convert.ToInt32(hdfBrand.Value);
                            bookingDate = Convert.ToDateTime(txtBookingDate.Text);
                            GetFieldValues(ControlsEnum.BRANDRATE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                txtRate.Text = GetFormattedRate(dtPageData.Rows[0]["BRD_RATE"].ToString());
                                if (double.TryParse(txtBrandQuantity.Text, out qty) && double.TryParse(txtRate.Text, out rate))
                                {
                                    txtAmount.Text = GetFormattedCurrency(qty * rate);
                                }
                                else
                                    txtAmount.Text = GetFormattedCurrency(0);
                            }
                            else
                            {
                                txtRate.Text = GetFormattedRate(0);
                                txtAmount.Text = GetFormattedCurrency(0);
                            }
                        }
                        else
                        {
                            txtRate.Text = GetFormattedRate(0);
                            txtAmount.Text = GetFormattedCurrency(0);
                        }
                        break;
                    #endregion
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            hdfIsLotNoIOReview.Value = "1";
                            hdfIsMsgIOReview.Value = "1";
                            if (txtAmount.Text.Length <= 15)
                            {
                                //Check If the Item have CBM.If not show warning Popup (Continue or Not)
                                if (Convert.ToDouble(hdfCBM.Value) == 0 && hdfIsContinueCBM.Value != "1")
                                {
                                    // hdfIsContinueCBM.Value = "0";
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CBMConfirm", "$(document).ready(function(){ShowCBMConfirm();});", true);

                                }
                                else
                                {

                                    if (((Math.Ceiling((Convert.ToDouble(txtQty.Text)) % (Convert.ToDouble(txtTotalPiecesCtn.Text))) == 0) ||
                                        (Iscarton == true && (hdfIscartYes.Value == "1"))))//Didn't want confirmation, when carton is fullfill with pcs
                                    //if (((Math.Ceiling((Convert.ToDouble(txtBrandQuantity.Text)) % (Convert.ToDouble(txtTotalPiecesCtn.Text))) == 0) || (Iscarton == true && (hdfIscartYes.Value == "1"))))
                                    {
                                        //hdfIsContinueCBM.Value = "0";
                                        Iscarton = false;
                                        // SaleOrderHeaderSession = TempSaleOrderHeaderSession;
                                        saleOrderDetailsList = SaleOrderHeaderSession.SaleContractDetails;
                                        saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                        string totalCBM = saleOrderDetailsList.Sum(itm => (itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM).ToString("N4");
                                        if (!IsTotalCBMExceeds(0, totalCBM))
                                        {
                                            IsBrand = false;
                                            Iscarton = false;
                                            saleOrderDetailsList = saleOrderDetailsList.Where(p => p.SOD_QTY > 0).ToList();
                                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                                            {
                                                SaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                                SetDetailTax(SaleOrderHeaderSession);
                                                SetSubTotal();
                                                SetHdrTax();
                                                saleOrderHeaderObj = SaleOrderHeaderSession;
                                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                                // isItemBind = true;
                                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                                                hdfIsContinueCBM.Value = "0";
                                                chkAddtlRemarkToAll.Checked = false;
                                                chkRemarkToAll.Checked = false;
                                                int i = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCAdditionalRemarksClear"));
                                                if (i == 0)
                                                {
                                                    txtDtlRemark2.Text = string.Empty;
                                                }
                                            }
                                            if (rdbBrand.Checked == true)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "ShowBrandDetails();", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(1);", true);
                                            }
                                        }
                                        else
                                        {
                                            IsBrand = true;
                                            Iscarton = true;
                                        }
                                    }
                                    else
                                    {
                                        Iscarton = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirming", "$(document).ready(function(){ShowCartCheckConfirming();});", true);
                                    }
                                }
                                // hdfIsContinueCBM.Value = "0";
                            }
                            else
                            {

                                litErrorMsg.Text = Resources.Messages.Pleaseenternomorethan0characters + " Eg : 100000000000.00";

                                litErrorMsg.Text = string.Format(litErrorMsg.Text, 15);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);


                            }
                        }
                        break;
                    #endregion

                    #region Add Packing Spec Item
                    case ActionsEnum.ADDPACKSPECITEM:
                        if (txtspecamount.Text.Length <= 15)
                        {
                            saleOrderDetailsList = SaleOrderHeaderSession.SaleContractDetails;
                            saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.PACKINGMATERAILSALEORDERDETAIL);
                            saleOrderDetailsList = saleOrderDetailsList.Where(p => p.SOD_SALE_QTY > 0).ToList();
                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            {
                                SaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                SetDetailTax(SaleOrderHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                saleOrderHeaderObj = SaleOrderHeaderSession;
                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                            }
                            if (rdbPackingSpec.Checked == true)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "ShowHidePackingMaterialDetails();", true);
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(0);", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.Pleaseenternomorethan0characters + " Eg : 100000000000.00";
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, 15);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }

                        break;
                    #endregion
                    #region Remove Item
                    case ActionsEnum.REMOVEITEM:
                        if (SaleOrderHeaderSession.SaleContractDetails != null && SaleOrderHeaderSession.SaleContractDetails.Count > 0)
                        {
                            double qtyDesp = 0;
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                if (Convert.ToInt16(hdfIsCopy.Value) == 0)
                                    qtyDesp = SaleOrderHeaderSession.SaleContractDetails.Where(row => CurrSlNo == row.SOD_SL_NO).ToList()[0].SOD_QTY_DISPATCHED;
                                if (qtyDesp <= 0)
                                {
                                    SaleOrderHeaderSession.SaleContractDetails = SaleOrderHeaderSession.SaleContractDetails.Where(row => CurrSlNo != row.SOD_SL_NO).ToList();
                                    SetDetailTax(SaleOrderHeaderSession);
                                    SetSubTotal();
                                    SetHdrTax();
                                    saleOrderHeaderObj = SaleOrderHeaderSession;
                                    //SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                    isItemBind = true;
                                    hdfIsMsgIOReview.Value = "1";
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Qty_DespatchAmd").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                    return;

                                }
                            }
                        }
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        try
                        {
                            string TotalCBM = saleOrderHeaderObj.SaleContractDetails.Sum(itm =>
                                            Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM, 4)).ToString("N4");
                            IsTotalCBMExceeds(3, TotalCBM);
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion
                    #region Edit Item
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        txtBrand.Enabled = false;
                        if (SaleOrderHeaderSession.SaleContractDetails != null && SaleOrderHeaderSession.SaleContractDetails.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                saleOrderDetailsObj = SaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                if (saleOrderDetailsObj.SOD_IS_PACK_MAT == 1)
                                {
                                    lbl_Prd_PackSpeck.Visible = true;
                                    lbl_product.Visible = false;
                                    ResetForm(ControlsEnum.CLEARPACKMATERIALITEM);
                                    GetUIValuesFromObject(ControlsEnum.SELECTEDPACKINGMATERAILITEM);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePackingMaterialDetails", "ShowHidePackingMaterialDetails();", true);
                                }
                                else if (saleOrderDetailsObj.SOD_IS_PACK_MAT == 2)
                                {
                                    lbl_Prd_PackSpeck.Visible = false;
                                    lbl_product.Visible = true;
                                    hdfIsProductRequired.Value = "1";
                                    ResetForm(ControlsEnum.CLEARPACKMATERIALITEM);
                                    GetUIValuesFromObject(ControlsEnum.SELECTEDPACKINGMATERAILITEM);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePackingMaterialDetails", "ShowHidePackingMaterialDetails();", true);
                                }
                                else
                                {
                                    GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowBrandDetails", "ShowBrandDetails();", true);
                                }
                            }
                        }
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        txtBrand.Enabled = false;
                        ddlArtWork.Focus();
                        //if (saleOrderDetailsObj.SOD_IS_PACK_MAT == 1)
                        //{

                        //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(0);", true);
                        //}
                        //else
                        //{

                        //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideItemDetails", "ShowHideItemDetails(1);", true);
                        //}
                        break;
                    #endregion
                    #region Clear Item
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        chkAddtlRemarkToAll.Checked = false;
                        chkRemarkToAll.Checked = false;
                        break;
                    #endregion
                    #region Clear Packing Materail Item
                    case ActionsEnum.CLEARPACKMATERIALITEM:
                        ResetForm(ControlsEnum.CLEARPACKMATERIALITEM);
                        break;
                    #endregion

                    #region Atrwork Selected
                    case ActionsEnum.ARTWORKSELECTED:
                        ResetForm(ControlsEnum.PACKINGSPEC);
                        artWorkPK = Convert.ToInt32(ddlArtWork.SelectedValue);
                        if (artWorkPK > 0)
                        {
                            GetFieldValues(ControlsEnum.PACKINGSPECDETAILS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                hdfCBM.Value = dtPageData.Rows[0]["CBM"].Equals(DBNull.Value) ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["CBM"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["PC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkPC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["PC_DOC_PATH"].ToString()))
                                        lnkArtWorkPC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkPC.Disabled = false;
                                        lnkArtWorkPC.HRef = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PC_ART_WORK"].ToString());
                                    hdfArtWorkPC.Value = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IB_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkIB.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["IB_DOC_PATH"].ToString()))
                                        lnkArtWorkIB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIB.Disabled = false;
                                        lnkArtWorkIB.HRef = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IB_ART_WORK"].ToString());
                                    hdfArtWorkIB.Value = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkIC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["IC_DOC_PATH"].ToString()))
                                        lnkArtWorkIC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIC.Disabled = false;
                                        lnkArtWorkIC.HRef = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IC_ART_WORK"].ToString());
                                    hdfArtWorkIC.Value = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkZB.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_DOC_PATH"].ToString()))
                                        lnkArtWorkZB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkZB.Disabled = false;
                                        lnkArtWorkZB.HRef = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ZB_ART_WORK"].ToString());
                                    hdfArtWorkZB.Value = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["MC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkMC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["MC_DOC_PATH"].ToString()))
                                        lnkArtWorkMC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkMC.Disabled = false;
                                        lnkArtWorkMC.HRef = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["MC_ART_WORK"].ToString());
                                    hdfArtWorkMC.Value = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();

                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["SC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkSC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["SC_DOC_PATH"].ToString()))
                                        lnkArtWorkSC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkSC.Disabled = false;
                                        lnkArtWorkSC.HRef = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["SC_ART_WORK"].ToString());
                                    hdfArtWorkSC.Value = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                }
                            }
                            else
                                ResetForm(ControlsEnum.PACKINGSPEC);
                        }
                        else
                            ResetForm(ControlsEnum.PACKINGSPEC);
                        break;
                    #endregion
                    #region Changing SO SUB TYPE
                    case ActionsEnum.SOSUBTYPE:
                        GetFieldValues(ControlsEnum.SOSUBTYPE);
                        SetFieldValues(ControlsEnum.SOSUBTYPE);
                        GetFieldValues(ControlsEnum.TOPORT);
                        SetFieldValues(ControlsEnum.TOPORT);
                        break;
                    #endregion
                    #region Notify Party
                    case ActionsEnum.NOTIFYPARTYSELECTED:
                        notifyPartyPK = Convert.ToInt32(ddlNotifyParty.SelectedValue);
                        if (notifyPartyPK > 0)
                        {
                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfNPName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfNPCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                hdfNPAddress.Value = txtNotifyParty.Text = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfNPCountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfNPEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfNPFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfNPMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfNPPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfNPZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                            }
                            else
                            {
                                txtNotifyParty.Text = string.Empty;
                                hdfNPName.Value = string.Empty;
                                hdfNPAddress.Value = string.Empty;
                                hdfNPCountry.Value = string.Empty;
                                hdfNPCountryText.Value = string.Empty;
                                hdfNPEmail.Value = string.Empty;
                                hdfNPFax.Value = string.Empty;
                                hdfNPMobile.Value = string.Empty;
                                hdfNPPhone.Value = string.Empty;
                                hdfNPZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtNotifyParty.Text = string.Empty;
                            hdfNPName.Value = string.Empty;
                            hdfNPAddress.Value = string.Empty;
                            hdfNPCountry.Value = string.Empty;
                            hdfNPCountryText.Value = string.Empty;
                            hdfNPEmail.Value = string.Empty;
                            hdfNPFax.Value = string.Empty;
                            hdfNPMobile.Value = string.Empty;
                            hdfNPPhone.Value = string.Empty;
                            hdfNPZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Consignee
                    case ActionsEnum.CONSIGNEESELECTED:
                        consigneePK = Convert.ToInt32(ddlConsigneeDetails.SelectedValue);
                        if (consigneePK > 0)
                        {
                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfCNEName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfCNECountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                hdfCNEAddress.Value = txtConsigneeDetails.Text = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfCNECountryText.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfCNEEmail.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_EMAIL"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfCNEFax.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_FAX"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfCNEMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfCNEPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfCNEZip.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_ZIP"].ToString());
                            }
                            else
                            {
                                txtConsigneeDetails.Text = string.Empty;
                                hdfCNEName.Value = string.Empty;
                                hdfCNEAddress.Value = string.Empty;
                                hdfCNECountry.Value = string.Empty;
                                hdfCNECountryText.Value = string.Empty;
                                hdfCNEEmail.Value = string.Empty;
                                hdfCNEFax.Value = string.Empty;
                                hdfCNEMobile.Value = string.Empty;
                                hdfCNEPhone.Value = string.Empty;
                                hdfCNEZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtConsigneeDetails.Text = string.Empty;
                            hdfCNEName.Value = string.Empty;
                            hdfCNEAddress.Value = string.Empty;
                            hdfCNECountry.Value = string.Empty;
                            hdfCNECountryText.Value = string.Empty;
                            hdfCNEEmail.Value = string.Empty;
                            hdfCNEFax.Value = string.Empty;
                            hdfCNEMobile.Value = string.Empty;
                            hdfCNEPhone.Value = string.Empty;
                            hdfCNEZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Agent
                    case ActionsEnum.AGENTSELECTED:
                        agentPK = Convert.ToInt32(ddlAgent.SelectedValue);
                        if (agentPK > 0)
                        {
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfAgentName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfAgentCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                hdfAgentAddress.Value = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfAgentCountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfAgentEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfAgentFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfAgentMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfAgentPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfAgentZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                            }
                            else
                            {
                                hdfAgentName.Value = string.Empty;
                                hdfAgentAddress.Value = string.Empty;
                                hdfAgentCountry.Value = string.Empty;
                                hdfAgentCountryText.Value = string.Empty;
                                hdfAgentEmail.Value = string.Empty;
                                hdfAgentFax.Value = string.Empty;
                                hdfAgentMobile.Value = string.Empty;
                                hdfAgentPhone.Value = string.Empty;
                                hdfAgentZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            hdfAgentName.Value = string.Empty;
                            hdfAgentAddress.Value = string.Empty;
                            hdfAgentCountry.Value = string.Empty;
                            hdfAgentCountryText.Value = string.Empty;
                            hdfAgentEmail.Value = string.Empty;
                            hdfAgentFax.Value = string.Empty;
                            hdfAgentMobile.Value = string.Empty;
                            hdfAgentPhone.Value = string.Empty;
                            hdfAgentZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Shipping Address
                    case ActionsEnum.ADDRESSSELECTED:
                        if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            addressPK = Convert.ToInt32(ddlCustAddress.SelectedValue);
                            addressActive = 2;
                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                        }
                        if (addressPK > 0 && dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            addr = string.Empty;
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                hdfShpName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                hdfShpCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                            }
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                            }
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                            }
                            else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                            }
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                            }
                            hdfShpAddress.Value = addr;
                            hdfShpAddress.Value = txtShippingAddress.Text = addr;

                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                hdfShpCountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                hdfShpEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                hdfShpFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                hdfShpMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                hdfShpPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                hdfShpZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                        }
                        else
                        {

                            hdfShpAddress.Value = txtShippingAddress.Text = hdfShpName.Value = hdfShpCountry.Value = hdfShpCountryText.Value =
                                hdfShpEmail.Value = hdfShpFax.Value = hdfShpMobile.Value = hdfShpPhone.Value = hdfShpZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Delivery Terms
                    //case ActionsEnum.DELTERMSELECTED:
                    //    if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                    //    {
                    //        if (IsDeliveryTermsCheck == false)
                    //        {
                    //            lblDelMan.Visible = false;
                    //        }

                    //        deliveryTermPK = Convert.ToInt32(ddlDeliveryTerms.SelectedValue);
                    //        GetFieldValues(ControlsEnum.DELIVERYTERMSBYPK);
                    //    }
                    //    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    //    {
                    //        txtDeliveryTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                    //        if (IsDeliveryTermsCheck == false)
                    //        {
                    //            lblDelMan.Visible = false;
                    //        }
                    //    }
                    //    else
                    //        txtDeliveryTerms.Text = string.Empty;


                    //    break;
                    #endregion
                    #region SHIPMENTTERMSELECTED
                    case ActionsEnum.SHIPMENTTERMSELECTED:
                        if (ddlShipmentTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            //lblDelMan.Visible = false;
                            shipmentTermPK = Convert.ToInt32(ddlShipmentTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.SHIPMENTTERMSBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CON_DESC"].ToString());
                            //lblDelMan.Visible = false;
                        }
                        else
                            txtDeliveryTerms.Text = string.Empty;
                        break;
                    #endregion
                    #region Payment Terms
                    case ActionsEnum.PAYTERMSELECTED:
                        if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            paymentTermPK = Convert.ToInt32(ddlPaymentTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.PAYMENTTERMSBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString()));
                        }
                        else
                            txtPaymentTerms.Text = string.Empty;
                        break;
                    #endregion
                    #region Special Cause
                    case ActionsEnum.SPECAUSESELECTED:
                        if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            specialCausePK = Convert.ToInt32(ddlSpecialCause.SelectedValue);
                            GetFieldValues(ControlsEnum.SPECIALCAUSEBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtSpecialCause.Text = string.Empty;
                        break;
                    #endregion
                    #region Autogenerate Lot No
                    case ActionsEnum.AUTOGENERATELOTNO:
                        AutoGenerateLotNo();
                        break;
                    #endregion
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (SaleOrderHeaderSession.SaleContractDetails == null || SaleOrderHeaderSession.SaleContractDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }

                        else if (IsTotalCBMExceeds(1, lblTotalCBM.Text))
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCBMCheckConfirming", "$(document).ready(function(){ShowCBMCheckConfirming('" + Msg + "');});", true);
                        }
                        else
                        {
                            //if (IsDeliveryTermsCheck == false)
                            //{
                            //    if ((ddlDeliveryTerms.SelectedValue == CommonConstants.SELECTVAL) && (txtDeliveryTerms.Text == string.Empty))
                            //    {
                            //        lblDelMan.Visible = true;
                            //        txtDeliveryTerms.Focus();
                            //        ddlDeliveryTerms.Focus();
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DeliveryTermsVal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            //        return;
                            //    }
                            //}

                            if ((ddlShipmentTerms.SelectedValue == CommonConstants.SELECTVAL) && (txtDeliveryTerms.Text == string.Empty))
                            {
                                //lblDelMan.Visible = true;
                                txtDeliveryTerms.Focus();
                                ddlShipmentTerms.Focus();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ShipmentTermsVal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }

                            saleOrderHeaderObj = new SaleContractBO();
                            saleOrderHeaderObj = (SaleContractBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);

                            if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                            {
                                saleOrderHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                string xmlDoc = CommonFunctions.XmlSerialize<SaleContractBO>(saleOrderHeaderObj);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
                                // save Process Control inspection details
                                string retMsg = string.Empty;
                                result = BusinessLogic.Sales.SaleOrderBL.SaveSaleOrderWkfDetails(xmlDoc, out retRefID, out retMsg);
                                if (result > 0) // Success !  redirect to listing page
                                {

                                    #region Attachment Details
                                    //Document Attach details
                                    EnsureUploadedFilesSaved();
                                    #endregion

                                    // Show Save Message and redired to listing page
                                    //string routeURL = "RFQResponse.aspx";
                                    //GetFieldValues(ControlsEnum.SALEORDER);
                                    CurrPK = (int)result;
                                    GetFieldValues(ControlsEnum.SALEORDER);
                                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_DraftSave_Success").ToString();//Msg_Save_Success
                                    object[] args = new object[2];
                                    args[0] = GetLocalResourceObject("SalesOrder").ToString();
                                    //args[0] = Resources.PageNameRes.SalesOrder;
                                    //  args[1] = saleOrderHeaderObj.SOH_NO;
                                    if (saleOrderHeaderObj.SOH_STATUS != 0 && hdfIsMsgIOReview.Value == "1")
                                    {
                                        hdfIsMsgIOReview.Value = "0";
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=" + "&IOType=1" + "&IOReview=" + hdfIsLotNoIOReview.Value) + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing + "?Dep=" + currentUser.CurrentDeptPK.ToString()) + "');", true);
                                    }
                                }
                                else if (result == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Save_Err_Ref + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == -2)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_CustPoNo_Exists").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == -6)//Total outstanding amount exceeds the credit limit set for the customer.	
                                {
                                    if (CreditLimitRuleBase == 1)
                                    {
                                        string ErrMsg = string.Format(GetLocalResourceObject("CreditLimitExceedErr").ToString(), retMsg);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "CreditCheckContinueConfirm('" + ErrMsg + "');", true);
                                    }
                                    else
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_OutstandingAmt_Exceeds").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.SaleOrderBL.DeleteSaleContractDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                //litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString();
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("SalesOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing + "?Dep=" + currentUser.CurrentDeptPK.ToString()) + "');", true);
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
                                    litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("SalesOrder").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Inactive
                    case ActionsEnum.INACTIVE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.SaleOrderBL.DeleteSaleContractDetails(CurrPK, LastModifiedTime, HttpUtility.HtmlEncode(txtReason.Text));
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("SalesOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing) + "');", true);
                            }
                            else
                            {
                                switch (result)
                                {
                                    case (int)DbDeleteStatus.SQLERROR:
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    case (int)DbDeleteStatus.CONCURRENCY:
                                        litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing) + "');", true);
                                        break;
                                    case (int)DbDeleteStatus.REFERRED:
                                        litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    case (int)DbDeleteStatus.DELETECONCURRENCY:
                                        litErrorMsg.Text = GetLocalResourceObject("SalesOrder").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing) + "');", true);
                                        break;
                                    default:
                                        if (result == (int)DbDeleteStatusSC.InvoiceCreated)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Invoiced_Created").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatusSC.ShippingCreated)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Shipping_Created").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("SalesOrder").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        break;
                                }
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "closedeletepopup", "$(document).ready(function(){closeDeletePopup();});", true);
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT popup
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup

                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region DELETEANDSUMBIT popup
                    case ActionsEnum.DELETEANDSUMBIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT Popup
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WorkFlow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideWkfSubmit", "ClosePopup();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (SaleOrderHeaderSession.SaleContractDetails == null || SaleOrderHeaderSession.SaleContractDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (IsTotalCBMExceeds(2, lblTotalCBM.Text))
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCBMCheckConfirming", "$(document).ready(function(){ShowCBMCheckConfirming('" + Msg + "');});", true);
                        }
                        else
                        {

                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                //if (IsDeliveryTermsCheck == false)
                                //{
                                //    if ((ddlDeliveryTerms.SelectedValue == CommonConstants.SELECTVAL) && (txtDeliveryTerms.Text == string.Empty))
                                //    {
                                //        lblDelMan.Visible = true;
                                //        txtDeliveryTerms.Focus();
                                //        ddlDeliveryTerms.Focus();
                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DeliveryTermsVal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                //        return;
                                //    }
                                //}

                                if ((ddlShipmentTerms.SelectedValue == CommonConstants.SELECTVAL) && (txtDeliveryTerms.Text == string.Empty))
                                {
                                    //lblDelMan.Visible = true;
                                    txtDeliveryTerms.Focus();
                                    ddlShipmentTerms.Focus();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ShipmentTermsVal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }

                                saleOrderHeaderObj = new SaleContractBO();
                                saleOrderHeaderObj = (SaleContractBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);
                                if (Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                                {

                                    if (saleOrderHeaderObj.SOH_ADV_INVOICED > 0)
                                    {
                                        if (saleOrderHeaderObj.SOH_ADV_INVOICED > saleOrderHeaderObj.SOH_NET_AMOUNT)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Net_Amount_Amend").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                }

                                if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                                {
                                    SaveTransaction(saleOrderHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender);
                                }

                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL ||
                                (Request.QueryString[QueryStrings.PageType] != null &&
                                Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.Sales.SaleOrderBL.SaleContractCancelCheck(CurrPK))
                                {
                                    // ucrWrkf.ApplicationID = CurrPK;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Cancel").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);

                        }
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINTSO:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE="), false);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);

                        break;
                    case ActionsEnum.PRINTIO:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE="), false);

                        if (hdfIsMsgIOReview.Value == "1")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "IOReviewforLotNo", "$(document).ready(function(){IOReview();});", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=" + "&IOType=1" + "&IOReview=" + hdfIsLotNoIOReview.Value) + "');", true);
                        }

                        break;
                    #endregion
                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            dvPerc.Visible = false;
                            saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);

                            if (CurrSlNo > 0)
                            {
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                            }
                            else
                            {
                                if (hdfisItemHaveTax.Value.ToString() != "0")
                                {
                                    saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                           TempSaleOrderHeaderSession.SaleContractDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                           && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                                }
                            }
                            if (saleOrderDetailsObj == null)
                            {
                                IsBrand = true; hdfIsBrandYes.Value = "1";
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                IsBrand = false; hdfIsBrandYes.Value = "0";
                            }

                            //if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            //{
                            if (saleOrderDetailsObj != null)// && saleOrderDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (TempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;

                                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupAmount.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DISCDETAILS
                    case ActionsEnum.DISCDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            dvPerc.Visible = false;
                            saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);



                            if (CurrSlNo > 0)
                            {
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                            }
                            //else
                            //{

                            //    saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                            //            TempSaleOrderHeaderSession.SaleContractDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                            //            && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                            //}


                            if (saleOrderDetailsObj == null)
                            {
                                IsBrand = true; hdfIsBrandYes.Value = "1";
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                IsBrand = false; hdfIsBrandYes.Value = "0";
                            }
                            if (saleOrderDetailsObj.SOD_AMOUNT <= 0)
                            {
                                saleOrderDetailsObj.SOD_AMOUNT = double.Parse(txtAmount.Text);
                            }
                            if (saleOrderDetailsObj != null)//&& saleOrderDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (TempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupAmount.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','645','300');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        if (hdfCustomer.Value == null || hdfCustomer.Value == "0")
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Customer").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                            hdfTaxFormula.Value = string.Empty;
                            if (SaleOrderHeaderSession != null)
                            {
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                                IsHeaderTax = true;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.TAXTYPES);
                                SetFieldValues(ControlsEnum.TAXTYPES);
                                double taxable = 0;
                                lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                                if (EnableItemTax != 2)
                                {
                                    taxable = Convert.ToDouble(SaleOrderHeaderSession.SOH_TOTAL_AMT) - SaleOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                    //Add Other Charges Based on configuration
                                    if (IsTaxForOtherCharge)
                                    {
                                        taxable += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                                    }
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.TAXCHECKBOX);
                                    if (chkSubTotal.Checked)
                                        taxable += lblSubTotal.Text != string.Empty ? Convert.ToDouble(lblSubTotal.Text) : 0;
                                    if (chkDiscount.Checked)
                                        taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                                    if (chkOtherCharges.Checked)
                                        taxable += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                                }

                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                if (EnableItemTax == 2)
                                    divTaxApplicableAmount.Attributes.Add("style", "display:block;");
                                else
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                                //}
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                //}
                            }
                        }
                        break;

                    #endregion
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (SaleOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.SALEORDERDETAIL);
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(lblSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','645','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SHIPPINGHEADER
                    case ActionsEnum.SHIPPINGHEADER:
                        dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (SaleOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.SALEORDERDETAIL);
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                double taxable = Convert.ToDouble(SaleOrderHeaderSession.SOH_TOTAL_AMT) - SaleOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    //txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupOther.Text = Resources.Report.Freight;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("Shipping").ToString() + "','645','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        if (IsHeaderTax)
                        {
                            SaleOrderHeaderSession = TempSaleOrderHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
                        }
                        else
                        {
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                            if (CurrSlNo > 0)
                            {
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                            }
                            else
                            {
                                saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                                && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                            }

                            if (saleOrderDetailsObj != null)
                            {
                                CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                            }
                            SetDetailTax(TempSaleOrderHeaderSession);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        //SelectedDtlPK
                        //SelectedCusItemPK
                        //SelectedItemPK


                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempSaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = TempSaleOrderHeaderSession;
                            tempSaleOrderTaxHdrObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                        saleOrderHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                        saleOrderHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.SLT_NAME == txtPopupOther.Text.Trim() && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                if (CurrSlNo > 0)
                                {
                                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                }
                                else
                                {
                                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                     saleOrderHeaderObj.SaleContractDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                     && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                                }



                                if (saleOrderDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                            saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                            saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == txtPopupOther.Text.Trim() && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempSaleOrderTaxHdrObj == null)
                            {
                                saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();

                                saleOrderTaxHdrObj = new SaleOrderTaxHdr();
                                try
                                {
                                    saleOrderTaxHdrObj.SLT_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    saleOrderTaxHdrObj.SLT_SO_DTL = SelectedDtlPK;
                                    saleOrderTaxHdrObj.SLT_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        saleOrderTaxHdrObj.SLT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    saleOrderTaxHdrObj.SLT_DISC_PERC = txtTaxPerc.Text == "" || Convert.ToDouble(txtTaxPerc.Text) < 0 ? 0 : Convert.ToDouble(txtTaxPerc.Text);
                                    saleOrderTaxHdrObj.SLT_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    saleOrderTaxHdrObj.SLT_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    saleOrderTaxHdrObj.SLT_PK = 0;
                                    //quotationTaxHdrObj.SLT_TAX_CATEGORY_TEXT = "Tax";
                                    saleOrderTaxHdrObj.SLT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    //saleOrderTaxHdrObj.SLT_TYPE = 1;
                                    saleOrderTaxHdrObj.SLT_TYPE = Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0 ? 1 : 2;//1 => Defined Tax, 2=> Custom Tax.
                                    saleOrderTaxHdrObj.SLT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (saleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Tax && EnableItemTax == 2)
                                        {
                                            saleOrderTaxHdrObj.SLT_HAS_SUB_TOTAL = chkSubTotal.Checked ? 1 : 0;
                                            saleOrderTaxHdrObj.SLT_HAS_DISCOUNT = chkDiscount.Checked ? 1 : 0;
                                            saleOrderTaxHdrObj.SLT_HAS_OTHER_CHARGE = chkOtherCharges.Checked ? 1 : 0;
                                        }
                                        if (saleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT);
                                            currentTotal = saleOrderHeaderObj.TaxHdr == null ? 0 :
                                                saleOrderHeaderObj.TaxHdr.Where(htx => htx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(saleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = saleOrderTaxHdrObj.SLT_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                if (Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                                                {
                                                    if (saleOrderHeaderObj.SOH_AMT_INVOICED > 0)
                                                    {
                                                        if (saleOrderHeaderObj.SOH_TOTAL_DISCOUNT > (currentTotal + taxAmt))
                                                        {
                                                            isValidDiscAm = false;
                                                        }
                                                    }
                                                }
                                                if (isValidDiscAm == true)
                                                {
                                                    saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                                    saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                    saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                                }
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }

                                        }
                                        else
                                        {
                                            saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                            saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                            saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        if (CurrSlNo > 0)
                                        {
                                            saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                                saleOrderHeaderObj.SaleContractDetails.LastOrDefault(item => item.SOD_PK == SelectedDtlPK
                                                && item.SOD_CUST_ITEM == SelectedCusItemPK && item.SOD_ITEM == SelectedItemPK);

                                        }
                                        if (saleOrderDetailsObj != null)
                                        {
                                            if (saleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = saleOrderDetailsObj.SOD_AMOUNT;
                                                currentTotal = saleOrderDetailsObj.TaxDtl == null ? 0 :
                                                    saleOrderDetailsObj.TaxDtl.Where(dtx => dtx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(saleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = saleOrderTaxHdrObj.SLT_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl == null ? new List<SaleOrderTaxHdr>() : saleOrderDetailsObj.TaxDtl.ToList();
                                                    saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                    if (CurrSlNo > 0)
                                                    {
                                                        saleOrderHeaderObj.SaleContractDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = saleOrderTaxHdrList;
                                                    }
                                                    else
                                                    {
                                                        saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                            && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;

                                                    }
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl == null ? new List<SaleOrderTaxHdr>() : saleOrderDetailsObj.TaxDtl.ToList();
                                                saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                if (CurrSlNo > 0)
                                                {
                                                    saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = saleOrderTaxHdrList;
                                                }
                                                else
                                                {
                                                    saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                        && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                                }
                                            }
                                        }
                                    }
                                    TempSaleOrderHeaderSession = saleOrderHeaderObj;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                }
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtPopupAmount.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','645','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("Shipping").ToString() + "','645','300');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);

                        if (errorTaxAdd)
                        {
                            if (hdfTaxCategory.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "2")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Shipping_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Disc_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        if (!isValidDiscAm)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount_Amend").ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (TempSaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = TempSaleOrderHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                            saleOrderHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                                saleOrderHeaderObj.TaxHdr.LastOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempSaleOrderTaxHdrObj != null)
                                    {
                                        saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                        saleOrderTaxHdrList.Remove(tempSaleOrderTaxHdrObj);
                                        saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                    }
                                }
                                else
                                {
                                    if (CurrSlNo > 0)
                                    {
                                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                           saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                    }
                                    else
                                    {
                                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                         saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                         && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK);

                                    }



                                    if (saleOrderDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                                saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                                    saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        if (CurrSlNo > 0)
                                        {
                                            saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                        }
                                        else
                                        {
                                            saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK);
                                        }

                                        if (saleOrderDetailsObj != null && saleOrderDetailsObj.TaxDtl != null)
                                        {
                                            saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl.ToList();
                                            saleOrderTaxHdrList.Remove(tempSaleOrderTaxHdrObj);
                                            if (CurrSlNo > 0)
                                            {
                                                saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = saleOrderTaxHdrList;
                                            }
                                            else
                                            {
                                                saleOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                    && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                            }
                                        }
                                    }
                                }

                                TempSaleOrderHeaderSession = saleOrderHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','645','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','645','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            dvPerc.Visible = false;
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxCategory.Value = dtSaleOrderTaxDetails.Rows[0]["TAX_CATEGORY"].ToString();
                                if (taxFormula == "0")
                                {
                                    hdfTaxFormula.Value = "0";
                                    SelectedTaxText = HttpUtility.HtmlEncode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    hdfTaxFormula.Value = taxFormula;
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                    txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                    SelectedTaxText = HttpUtility.HtmlEncode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;

                                }

                            }
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            txtPopupAmount.Text = string.Empty;
                            SelectedTaxText = Resources.Report.Custom;
                            txtPopupOther.Text = string.Empty;
                            txtPopupAmount.Enabled = true;
                            txtPopupOther.Enabled = true;
                            txtPopupOther.Text = Resources.Controls.Discount;
                            if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString())
                                dvPerc.Visible = true;
                            else
                                dvPerc.Visible = false;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','645','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','645','300');", true);
                        break;
                    #endregion
                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        SetDetailTax(TempSaleOrderHeaderSession);
                        //SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.SALEORDERHEADER);
                        //if (string.IsNullOrEmpty(lblSaleOrderNo.Text))
                        //{
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.QuotationListing), false);
                        //}
                        //else
                        //{
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing), false);

                        //}
                        if (hdfPreviousUrl.Value.Contains("Inbox.aspx"))
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing + "?Dep=" + currentUser.CurrentDeptPK.ToString()), false);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_GoBack", "GoBack();", true);
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing + "?Dep=" + currentUser.CurrentDeptPK.ToString()), false);
                            //Nobody have no Idea about GoBack() and It contains error code so I redirected it to listing Page

                        }
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.SO;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblSaleOrderNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtSaleOrderDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = txtCustomer.Text.Trim();
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region Revision History
                    case ActionsEnum.REVISIONHISTORY:
                        GetFieldValues(ControlsEnum.REVISIONHISTORY);
                        SetFieldValues(ControlsEnum.REVISIONHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divRevisionHistory]','" + GetLocalResourceObject("RevisionHistory").ToString() + "','400','300');", true);
                        break;
                    #endregion
                    #region ATTACHDOCS
                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (!string.IsNullOrEmpty(fupUpload.PostedFile.FileName))
                            {
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                if (!IsValidExtension(tempFileInfoObj.Extension))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    if (CurrDocSlNo != 0)
                                    {
                                        if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                        {
                                            soUploadObj = SOUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrDocSlNo);
                                            if (soUploadObj != null)
                                            {
                                                if (FileDetailsList == null)
                                                {
                                                    FileDetailsList = new List<FileDetails>();
                                                }
                                                if (fupUpload.HasFile)
                                                {

                                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    soUploadObj.AttachmentFileName = attachmentFileName;
                                                    soUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    soUploadObj.DOC_NAME = fupUpload.FileName;
                                                    soUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        soUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        soUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;

                                                    }
                                                    SaveUploadedFile(soUploadObj);
                                                    FileDetailsList = FileDetailsList.Where(aa => aa.SlNo != CurrDocSlNo).ToList();
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (fupUpload.HasFile)
                                        {

                                            int slno = 1;
                                            if (SOUploadList == null || SOUploadList.Count == 0)
                                            {
                                                SOUploadList = new List<BusinessObject.Sales.SaleOrderUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = SOUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }

                                            soUploadObj = new SaleOrderUploads();
                                            soUploadObj.DOC_PK = 0;
                                            soUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            soUploadObj.AttachmentFileName = attachmentFileName;
                                            soUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            soUploadObj.DOC_NAME = fupUpload.FileName;
                                            soUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                soUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                soUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }

                                            soUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            SaveUploadedFile(soUploadObj);
                                            FileDetailsList = FileDetailsList.Where(aa => aa.SlNo != slno).ToList();
                                            SOUploadList.Add(soUploadObj);

                                        }
                                    }
                                    //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                    //ResetForm(ControlsEnum.ADDITEM);
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotal", "CalculateTotal();", true);

                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (SOUploadList != null && SOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                SOUploadList = SOUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                if (FileDetailsList != null)
                                    FileDetailsList = FileDetailsList.Where(attdoc => selectedItemPK != attdoc.SlNo).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (SOUploadList != null && SOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                soUploadObj = SOUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        SaleContractDetailsBO ObjTempData = new SaleContractDetailsBO();
                        if (SaleOrderHeaderSession.SaleContractDetails != null && SaleOrderHeaderSession.SaleContractDetails.Count > 0)
                        {
                            int RowCurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (RowCurrSlNo > 0)
                            {
                                ObjTempData = SaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => RowCurrSlNo == row.SOD_SL_NO);
                            }
                        }
                        chkstrapView.Checked = ObjTempData.SOD_STRAPPING == "1" ? true : false;
                        chkLayerView.Checked = ObjTempData.SOD_LAYERING == "1" ? true : false;
                        ddlStrappingColorView.SelectedIndex = ddlStrappingColorView.Items.IndexOf(ddlStrappingColorView.Items.FindByValue(ObjTempData.SOD_STRAPPING_COLOUR));
                        txtNoofLrsView.Text = ObjTempData.SOD_NO_LAYERS.ToString();
                        txtPcsView.Text = ObjTempData.SOD_PIECES_LAYER.ToString();
                        txtMfgDtView.Text = !string.IsNullOrEmpty(ObjTempData.SOD_MFG_DATE) ? (Convert.ToDateTime(ObjTempData.SOD_MFG_DATE)).ToString(Resources.Constants.HRMSDateFormatShort) : string.Empty;
                        txtExpiryView.Text = !string.IsNullOrEmpty(ObjTempData.SOD_EXP_DATE) ? (Convert.ToDateTime(ObjTempData.SOD_EXP_DATE)).ToString(Resources.Constants.HRMSDateFormatShort) : string.Empty;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divViewAdditionalPackDtls]','" + GetLocalResourceObject("ViewAddPackDtls").ToString() + "','900','150');", true);

                        break;
                    #endregion
                    #region ITEM REF DTL
                    case ActionsEnum.ITEMREFDTL:
                        SodPk = 0;
                        int.TryParse(((ImageButton)sender).CommandArgument, out SodPk);
                        GetFieldValues(ControlsEnum.ITEMREFDTL);
                        SetFieldValues(ControlsEnum.ITEMREFDTL);
                        SetReferedItems();
                        if (SodPk == 0)
                        {
                            divPopupBrand.Visible = false;
                            if (saleOrderItemRefObj == null || saleOrderItemRefObj.RefDetails == null || saleOrderItemRefObj.RefDetails.Count == 0)
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_NoRefFound").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            divPopupBrand.Visible = true;
                            GridViewRow grvRow = (((ImageButton)sender).Parent.Parent as GridViewRow);
                            if (grvRow != null)
                            {
                                Label lblBrandName = (Label)grvRow.FindControl("lblBrandName");
                                lblPopupBrndName.Text = ERP.Utilities.CommonFunctions.GetShortString(lblBrandName.Text, 70);
                                lblPopupBrndName.ToolTip = lblBrandName.Text;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemRefDtl]','" + GetLocalResourceObject("ItemReferenceDetails").ToString() + "','" + GetLocalResourceObject("RefPopupWidth").ToString() + "','" + GetLocalResourceObject("RefPopupHeight").ToString() + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SALES COST
                    case ActionsEnum.SHOWSALESCOST:
                        if (((ImageButton)sender).ID == "imbShowSCdtlcost")
                        {
                            if (!string.IsNullOrEmpty(hdfBrand.Value) && Convert.ToInt32(hdfBrand.Value) > 0)
                            {
                                objSCcost = new SalesCostBO();
                                objSCcost.SOH_DATE = Convert.ToDateTime(txtSaleOrderDate.Text);
                                objSCcost.BIZUNIT_PK = currentUser.CurrentSBUPK;
                                objSCcost.SalesCostPatams = (List<SalesCostPatams>)SetUIValuesToObject(ControlsEnum.SALESCOST);
                                GetFieldValues(ControlsEnum.SALESCOST);
                                SetFieldValues(ControlsEnum.SALESCOST);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('[id$=divSaleCostDtls]','" + GetLocalResourceObject("SalesCostPopUpHeader").ToString() + "','1000');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Brand").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        else
                        {
                            objSCcost = new SalesCostBO();
                            objSCcost.SOH_DATE = Convert.ToDateTime(txtSaleOrderDate.Text);
                            objSCcost.BIZUNIT_PK = currentUser.CurrentSBUPK;
                            objSCcost.SalesCostPatams = (List<SalesCostPatams>)SetUIValuesToObject(ControlsEnum.ALLSALESCOST);
                            GetFieldValues(ControlsEnum.SALESCOST);
                            SetFieldValues(ControlsEnum.SALESCOST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('[id$=divSaleCostDtls]','" + GetLocalResourceObject("SalesCostPopUpHeader").ToString() + "','1000');", true);
                        }
                        break;
                    #endregion
                    #region CALCULATE QTY
                    case ActionsEnum.SHOWPOPUP:
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty)
                        {
                            ResetForm(ControlsEnum.QTYPOPUP);
                            txtPopUpTotalPcsCtn.Text = GetFormattedNumber(hdfPcsperCtn.Value);
                            txtPopUpPcsperUnit.Text = GetFormattedNumber(hdfSaleUOMPcs.Value);
                            txtPopUpQty.Text = txtBrandQuantity.Text != string.Empty ? GetFormattedNumber(txtBrandQuantity.Text) : "0";
                            double OrderQty = 0;
                            if (Convert.ToDouble(hdfPcsperCtn.Value) > 0)
                                OrderQty = (Convert.ToDouble(hdfSaleUOMPcs.Value) * Convert.ToDouble(txtBrandQuantity.Text)) / Convert.ToDouble(hdfPcsperCtn.Value);
                            else
                                txtPopUpOrderQty.Text = "0.00";
                            txtPopUpOrderQty.Text = GetFormattedNumber(OrderQty);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('[id$=divQuantityCalculation]','" + GetLocalResourceObject("QunatityPopUpCaption").ToString() + "','500');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Brand").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region QUANTITY APPLY
                    case ActionsEnum.QUANTITYAPPLY:
                        txtBrandQuantity.Text = txtPopUpQty.Text != string.Empty ? GetFormattedNumber(txtPopUpQty.Text) : "0.00";
                        ResetForm(ControlsEnum.QTYPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAmountPopUpApply", "ClosePopup();CalculateAmountPopUpApply();", true);
                        break;
                    #endregion
                    #region TAXPOPUP CHECK CHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        if (((CheckBox)sender).ID == "chkSubTotal" || ((CheckBox)sender).ID == "chkDiscount" || ((CheckBox)sender).ID == "chkOtherCharges")
                        {
                            hdfTaxFormula.Value = string.Empty;
                            double taxable = 0;
                            if (chkSubTotal.Checked)
                            {
                                lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                                taxable += lblSubTotal.Text != string.Empty ? Convert.ToDouble(lblSubTotal.Text) : 0;
                            }
                            if (chkDiscount.Checked)
                            {
                                taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                            }
                            if (chkOtherCharges.Checked)
                            {
                                taxable += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                            }
                            txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    TaxPK = 0;
                                    if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                    {
                                        string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                        hdfTaxFormula.Value = taxFormula;
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                        txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                        SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    }
                                }
                                else
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Text = string.Empty;
                                }
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }

                            //SetHdrTax();
                            //SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            if (EnableItemTax == 2)
                                divTaxApplicableAmount.Attributes.Add("style", "display:block;");

                            else
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                        }
                        break;
                    #endregion
                    case ActionsEnum.QUOTATIONCHANGE:
                        CrmQuotationPK = hdfQuotationPK.Value == "" ? 0 : Convert.ToInt32(hdfQuotationPK.Value);
                        GetFieldValues(ControlsEnum.CRMCUSTOMER);
                        SetFieldValues(ControlsEnum.CRMCUSTOMER);
                        GetFieldValues(ControlsEnum.QUOTATION);
                        SetFieldValues(ControlsEnum.QUOTATION);
                        break;
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

        private void SetReferedItems(GridViewRow grRow = null)
        {
            int sodtlpk = 0;
            HiddenField hdfSODPK;
            ImageButton imbDtlRef;
            if (grRow != null)
            {
                hdfSODPK = (HiddenField)grRow.FindControl("hdfSODPK");
                imbDtlRef = (ImageButton)grRow.FindControl("imbDtlRef");
                int.TryParse(hdfSODPK.Value, out sodtlpk);
                if (SaleOrderItemRefHdr != null && SaleOrderItemRefHdr.RefDetails != null && SaleOrderItemRefHdr.RefDetails.Count > 0 && sodtlpk > 0)
                {
                    if (SaleOrderItemRefHdr.RefDetails.Where(r => r.SOD_PK == sodtlpk).Count() > 0)
                    {
                        grRow.Attributes.Add("class", "custm-grd");
                        imbDtlRef.Visible = true;
                    }
                }
            }
            else
            {
                foreach (GridViewRow grvRow in grdItemDetails.Rows)
                {
                    hdfSODPK = (HiddenField)grvRow.FindControl("hdfSODPK");
                    imbDtlRef = (ImageButton)grvRow.FindControl("imbDtlRef");
                    int.TryParse(hdfSODPK.Value, out sodtlpk);
                    if (SaleOrderItemRefHdr != null && SaleOrderItemRefHdr.RefDetails != null && SaleOrderItemRefHdr.RefDetails.Count > 0 && sodtlpk > 0)
                    {
                        if (SaleOrderItemRefHdr.RefDetails.Where(r => r.SOD_PK == sodtlpk).Count() > 0)
                        {
                            grvRow.Attributes.Add("class", "custm-grd");
                            imbDtlRef.Visible = true;
                        }
                    }
                }
            }

        }

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(SaleContractBO objSaleContract, int workflowFlag, object sender)
        {
            string retMsg = string.Empty;
            int retRfID = 0;
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objSaleContract == null)
                objSaleContract = new SaleContractBO();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objSaleContract.USER_PK = wkfDetails.UserPK;
            objSaleContract.WKF_APPLICATION = CurrPK;
            objSaleContract.WKF_COMMENTS = wkfDetails.Comments;
            objSaleContract.WKF_TRX_FLAG = workflowFlag;
            objSaleContract.WKF_PROCESS = wkfDetails.ProcessID;
            objSaleContract.WKF_REFERENCE = wkfDetails.ReferenceID;
            objSaleContract.WKF_TASK = wkfDetails.TaskID;
            objSaleContract.WKF_TASK_ACTION = wkfDetails.ActionID;
            //objSaleContract.WKF_MAIL_ATTACH = 0;
            action = wkfDetails.ActionText;
            #endregion

            string xmlDoc = CommonFunctions.XmlSerialize<SaleContractBO>(objSaleContract);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
                                                                                          // save Process Control inspection details

            result = BusinessLogic.Sales.SaleOrderBL.SaveSaleOrderWkfDetails(xmlDoc, out retRfID, out retMsg);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                CurrPK = (int)result;
                if (Request.QueryString[QueryStrings.PageType] != SCWorkFlowType.Amend && SendMailWithAttachment && CurrPK > 0)
                {
                    int version = 0;
                    CurrQuotationPK = 0;
                    GetFieldValues(ControlsEnum.SALEORDER);
                    if (saleOrderHeaderObj.SOH_STATUS == 2)//Final Stage
                    {
                        GenerateReport objGenRpt = new GenerateReport();

                        //Mail report generation in case of SO approval
                        objGenRpt.APPSUBTYPE = 0;
                        objGenRpt.TransactionPk = CurrPK;
                        objGenRpt.Version = version + 1;
                        objGenRpt.DocumentName = string.IsNullOrEmpty(hdfMailAttachmentName.Value) ? (saleOrderHeaderObj.SOH_NO + "-" + ApplicationType.SO + ".pdf") : hdfMailAttachmentName.Value;
                        objGenRpt.APPTYPE = ApplicationType.SO;
                        objGenRpt.SaveReportForMail();

                        // Mail report generation in case of IO generation   --GTID170
                        objGenRpt.APPSUBTYPE = 0;
                        objGenRpt.TransactionPk = CurrPK;
                        objGenRpt.Version = version + 1;
                        objGenRpt.DocumentName = string.IsNullOrEmpty(hdfMailAttachmentName.Value) ? (saleOrderHeaderObj.SOH_NO + "-" + ApplicationType.IO + ".pdf") : hdfMailAttachmentName.Value;
                        objGenRpt.APPTYPE = ApplicationType.IO;
                        objGenRpt.IOType = 0;
                        objGenRpt.SaveReportForMail();

                        objSaleContract.P_TYPE = Convert.ToInt32(SCWorkFlowType.SC);
                        objSaleContract.WKF_MAIL_ATTACH = 1;
                        BusinessLogic.Sales.SaleOrderBL.SaleOrderMailSave(CurrPK, retRfID, Convert.ToInt32(SCWorkFlowType.SC));
                    }
                }
                else if (Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                {
                    if (SendMailWithAttachment && CurrPK > 0)
                    {
                        GetFieldValues(ControlsEnum.SALEORDER);
                        int ScVersion = 1;
                        int.TryParse(hdfVersion.Value, out ScVersion);
                        GenerateReport objGenRpt = new GenerateReport();

                        //Mail report generation in case of SO amendment
                        objGenRpt.APPSUBTYPE = 0;
                        objGenRpt.TransactionPk = CurrPK;
                        objGenRpt.Version = ScVersion + 1;
                        objGenRpt.DocumentName = saleOrderHeaderObj.SOH_NO + "-" + ApplicationType.SO + ".pdf";//  Guid.NewGuid().ToString() + ".pdf";
                        objGenRpt.APPTYPE = ApplicationType.SO;
                        objGenRpt.SaveReportForMail();

                        //Mail report generation in case of SO amendment for IO  --GTID170
                        objGenRpt.APPSUBTYPE = 0;
                        objGenRpt.TransactionPk = CurrPK;
                        objGenRpt.Version = ScVersion + 1;
                        objGenRpt.DocumentName = saleOrderHeaderObj.SOH_NO + "-" + ApplicationType.IO + ".pdf"; //Guid.NewGuid().ToString() + ".pdf";
                        objGenRpt.APPTYPE = ApplicationType.IO;
                        objGenRpt.IOType = 0;
                        objGenRpt.SaveReportForMail();

                        objSaleContract.P_TYPE = Convert.ToInt32(SCWorkFlowType.Amend);
                        objSaleContract.WKF_MAIL_ATTACH = 1;
                        BusinessLogic.Sales.SaleOrderBL.SaleOrderMailSave(CurrPK, retRfID, Convert.ToInt32(SCWorkFlowType.Amend));
                    }
                }
                CurrPK = (int)result;
                #region Attachment Details
                //Document Attach details
                EnsureUploadedFilesSaved();
                #endregion

                Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                //ucrWrkf.ApplicationID = result.Value;

                #region After Workflow
                string saleContractNo = string.Empty;
                if (string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                    || lblSaleOrderNo.Text.Contains(Resources.Messages.DocGenerationNew) || (saleOrderHeaderObj != null ? saleOrderHeaderObj.SOH_STATUS == 0 : true))
                {
                    //CurrPK = ucrWrkf.ApplicationID;
                    GetFieldValues(ControlsEnum.SALEORDER);
                    saleContractNo = saleOrderHeaderObj.SOH_NO;
                }
                else
                {
                    if (Request.QueryString[QueryStrings.PageType] != null &&
                        Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                    {
                        //CurrPK = (int)result;
                        GetFieldValues(ControlsEnum.SALEORDER);
                        saleContractNo = saleOrderHeaderObj.SOH_NO;
                    }
                    else
                    {
                        saleContractNo = lblSaleOrderNo.Text.Trim();
                    }
                }
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL ||
                        (Request.QueryString[QueryStrings.PageType] != null &&
                         Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel
                        ))
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Cancel_Success").ToString();
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                object[] args = new object[2];
                args[0] = GetLocalResourceObject("SalesOrder").ToString();
                //args[0] = Resources.PageNameRes.SalesOrder;
                args[1] = saleContractNo;
                //args[1] = saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                //    || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                //    ? saleOrderHeaderObj.SOH_NO : lblSaleOrderNo.Text.Trim();
                #region Inbox or Listing Page Redirection
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }

                else if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ConfigurationManager.AppSettings["ClientResourceSuffix"] == "BWH" && (saleOrderHeaderObj != null ? saleOrderHeaderObj.SOH_STATUS > 1 : true))
                {
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);

                }

                else
                {
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing + "?Dep=" + currentUser.CurrentDeptPK.ToString()) + "');", true);
                }

                #endregion
                #endregion

            }
            else if (result == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + Resources.ErrorMessages.Msg_Save_Err_Ref + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }
            else if (result == -2)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_CustPoNo_Exists").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }
            else if (result == -6)//Total outstanding amount exceeds the credit limit set for the customer.	
            {
                if (CreditLimitRuleBase == 1)
                {
                    string ErrMsg = string.Format(GetLocalResourceObject("CreditLimitExceedErr").ToString(), retMsg);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "CreditCheckContinueConfirm('" + ErrMsg + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_OutstandingAmt_Exceeds").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
            else if (result == -7)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Err_Net_Amount_Amend").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }

            else if (result == -44)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_SCAmendValidation").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }

            else if (result == -72)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowCustPONoExistConfirm('" + (sender as Button).ID + "');", true);
            }
            else if (result == -77)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Conv_PO_In_Another_Place").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }
            else if (result == -201)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("TransNo_Not_Generated").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                return;
            }

        }
        #endregion
        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            SaleContractDetailsBO scItem;

            Label lblItemCartonsOrBags;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkPC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkIB;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkIC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkZB;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkMC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkSC;
            Label lblNewArtWork;

            Label lblItemTotalQty;
            Label lblItemTotalCarton;
            Label lblItemTotalAmount;
            Label lblDiscountTotal;
            Label lblTaxTotal;
            Label lblSubTotalFooter;
            //Label lblBrandItemTotalQty;

            try
            {
                if ((sender as GridView).ID == "grdItemDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        e.Row.Cells[9].Visible = EnableItemDiscount > 0 ? true : false;
                        e.Row.Cells[10].Visible = EnableItemTax > 0 ? true : false;
                        e.Row.Cells[8].Visible = EnableItemDiscount > 0 ? true : false || EnableItemTax > 0 ? true : false;
                        e.Row.Cells[12].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[13].Visible = Convert.ToBoolean(ShowLotSize);

                        lblItemCartonsOrBags = e.Row.FindControl("lblItemCartonsOrBags") as Label;
                        lnkLstArtWorkPC = e.Row.FindControl("lnkLstArtWorkPC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkIB = e.Row.FindControl("lnkLstArtWorkIB") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkIC = e.Row.FindControl("lnkLstArtWorkIC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkZB = e.Row.FindControl("lnkLstArtWorkZB") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkMC = e.Row.FindControl("lnkLstArtWorkMC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkSC = e.Row.FindControl("lnkLstArtWorkSC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lblNewArtWork = e.Row.FindControl("lblNewArtWork") as Label;

                        if (ShowSCAdditionalPackDtls == 1)
                            (e.Row.FindControl("btnViewItem") as ImageButton).Visible = true;
                        else
                            (e.Row.FindControl("btnViewItem") as ImageButton).Visible = false;

                        scItem = (SaleContractDetailsBO)e.Row.DataItem;
                        if (scItem != null)
                        {

                            if (lblItemCartonsOrBags != null)
                            {
                                //lblItemCartonsOrBags.Text = Math.Ceiling(scItem.SOD_QTY / scItem.APS_TOTAL_PCS).ToString();
                                if (scItem.SOD_IS_PACK_MAT != 1 && scItem.SOD_IS_PACK_MAT != 2)
                                {
                                    if (CartonDecimal > 0)
                                        lblItemCartonsOrBags.Text = GetFormattedNumberWithComma(Math.Round((scItem.SOD_QTY / scItem.APS_TOTAL_PCS), CartonDecimal));
                                    else
                                        lblItemCartonsOrBags.Text = string.Format("{0:n0}", Math.Ceiling(scItem.SOD_QTY / scItem.APS_TOTAL_PCS));
                                }
                                else
                                {
                                    lblItemCartonsOrBags.Text = "-";
                                }

                                lblItemCartonsOrBags.ToolTip = HttpUtility.HtmlDecode(scItem.PACKING_TEXT);
                            }
                            if (string.IsNullOrEmpty(scItem.SOD_ART_WORK))
                            {
                                lblNewArtWork.Visible = true;
                            }
                            else
                            {
                                lblNewArtWork.Visible = false;
                                if (lnkLstArtWorkPC != null)
                                {
                                    lnkLstArtWorkPC.Visible = !string.IsNullOrEmpty(scItem.PC_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.PC_DOC_PATH))
                                        lnkLstArtWorkPC.Disabled = true;
                                    else
                                        lnkLstArtWorkPC.HRef = scItem.PC_DOC_PATH;
                                    lnkLstArtWorkPC.InnerText = lnkLstArtWorkPC.Title = scItem.PC_ART_WORK;

                                }
                                if (lnkLstArtWorkIB != null)
                                {
                                    lnkLstArtWorkIB.Visible = !string.IsNullOrEmpty(scItem.IB_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.IB_DOC_PATH))
                                        lnkLstArtWorkIB.Disabled = true;
                                    else
                                        lnkLstArtWorkIB.HRef = scItem.IB_DOC_PATH;
                                    lnkLstArtWorkIB.InnerText = lnkLstArtWorkIB.Title = scItem.IB_ART_WORK;
                                }
                                if (lnkLstArtWorkIC != null)
                                {
                                    lnkLstArtWorkIC.Visible = !string.IsNullOrEmpty(scItem.IC_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.IC_DOC_PATH))
                                        lnkLstArtWorkIC.Disabled = true;
                                    else
                                        lnkLstArtWorkIC.HRef = scItem.IC_DOC_PATH;
                                    lnkLstArtWorkIC.InnerText = lnkLstArtWorkIC.Title = scItem.IC_ART_WORK;
                                }
                                if (lnkLstArtWorkZB != null)
                                {
                                    lnkLstArtWorkZB.Visible = !string.IsNullOrEmpty(scItem.ZB_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.ZB_DOC_PATH))
                                        lnkLstArtWorkZB.Disabled = true;
                                    else
                                        lnkLstArtWorkZB.HRef = scItem.ZB_DOC_PATH;
                                    lnkLstArtWorkZB.InnerText = lnkLstArtWorkZB.Title = scItem.ZB_ART_WORK;
                                }
                                if (lnkLstArtWorkMC != null)
                                {
                                    lnkLstArtWorkMC.Visible = !string.IsNullOrEmpty(scItem.MC_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.MC_DOC_PATH))
                                    {
                                        lnkLstArtWorkMC.Disabled = true;
                                        lnkLstArtWorkMC.HRef = scItem.MC_DOC_PATH;
                                    }
                                    else
                                    { lnkLstArtWorkMC.HRef = scItem.MC_DOC_PATH; }
                                    lnkLstArtWorkMC.InnerText = lnkLstArtWorkMC.Title = scItem.MC_ART_WORK;
                                }
                                if (lnkLstArtWorkSC != null)
                                {
                                    lnkLstArtWorkSC.Visible = !string.IsNullOrEmpty(scItem.SC_ART_WORK);
                                    if (string.IsNullOrEmpty(scItem.SC_DOC_PATH))
                                        lnkLstArtWorkSC.Disabled = true;
                                    else
                                        lnkLstArtWorkSC.HRef = scItem.SC_DOC_PATH;
                                    lnkLstArtWorkSC.InnerText = lnkLstArtWorkSC.Title = scItem.SC_ART_WORK;
                                }
                            }
                            if (Math.Ceiling(scItem.SOD_QTY % scItem.APS_TOTAL_PCS) != 0)
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("PartialCartonColor").ToString());
                            }
                            if (IsAmend)
                                SetReferedItems(e.Row);

                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer && saleOrderDetailsList != null)
                    {
                        e.Row.Cells[9].Visible = EnableItemDiscount > 0 ? true : false;
                        e.Row.Cells[10].Visible = EnableItemTax > 0 ? true : false;
                        e.Row.Cells[8].Visible = EnableItemDiscount > 0 ? true : false || EnableItemTax > 0 ? true : false;
                        e.Row.Cells[12].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[13].Visible = Convert.ToBoolean(ShowLotSize);

                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalCarton = e.Row.FindControl("lblItemTotalCarton") as Label;
                        lblItemTotalAmount = e.Row.FindControl("lblItemTotalAmount") as Label;
                        lblDiscountTotal = e.Row.FindControl("lblDiscountTotal") as Label;
                        lblTaxTotal = e.Row.FindControl("lblTaxTotal") as Label;
                        lblSubTotalFooter = e.Row.FindControl("lblSubTotalFooter") as Label;
                        //lblBrandItemTotalQty = e.Row.FindControl("lblBrandItemTotalQty") as Label;

                        if (lblItemTotalQty != null)
                        {
                            lblItemTotalQty.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(saleOrderDetailsList.Sum(itm => itm.SOD_QTY));
                        }
                        //if (lblBrandItemTotalQty != null)
                        //{
                        //    lblBrandItemTotalQty.Text = lblBrandItemTotalQty.ToolTip = GetFormattedNumber(saleOrderDetailsList.Sum(itm => itm.SOD_SALE_QTY));
                        //}
                        if (lblItemTotalCarton != null)
                        {
                            //lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = saleOrderDetailsList.Sum(itm =>
                            //    Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS)).ToString();
                            if (saleOrderDetailsList.Sum(itm => itm.APS_TOTAL_PCS) > 0)
                            {
                                if (CartonDecimal > 0)
                                    lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = GetFormattedNumberWithComma(Math.Ceiling(Math.Round(saleOrderDetailsList.Sum(itm => itm.SOD_IS_PACK_MAT == 1 ? 0 : Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS), CartonDecimal)), CartonDecimal)));
                                else
                                    lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = string.Format("{0:n0}", saleOrderDetailsList.Sum(itm => itm.SOD_IS_PACK_MAT == 1 ? 0 : Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS)));
                            }
                            else
                            {
                                lblItemTotalCarton.Text = "-";
                            }
                        }
                        if (lblItemTotalAmount != null)
                        {
                            lblItemTotalAmount.Text = lblItemTotalAmount.ToolTip = GetFormattedCurrency(saleOrderDetailsList.Sum(itm => itm.SOD_AMOUNT));
                        }
                        if (lblDiscountTotal != null)
                        {
                            lblDiscountTotal.Text = lblDiscountTotal.ToolTip = GetFormattedCurrency(saleOrderDetailsList.Sum(itm => itm.SOD_DISCOUNT));
                        }
                        if (lblTaxTotal != null)
                        {
                            lblTaxTotal.Text = lblTaxTotal.ToolTip = GetFormattedCurrency(saleOrderDetailsList.Sum(itm => itm.SOD_TAX));
                        }
                        if (lblSubTotalFooter != null)
                        {
                            lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = string.Format("{0:c}", Convert.ToDecimal(saleOrderDetailsList.Sum(itm => itm.SOD_NET_AMOUNT).ToString(hdfCurrencyFormat.Value)));
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[9].Visible = EnableItemDiscount > 0 ? true : false;
                        e.Row.Cells[10].Visible = EnableItemTax > 0 ? true : false;
                        e.Row.Cells[8].Visible = EnableItemDiscount > 0 ? true : false || EnableItemTax > 0 ? true : false;
                        // e.Row.Cells[5].Text = GetLocalResourceObject("Quantity").ToString() + "(" + saleOrderDetailsList[0].SOD_UOM_TEXT + ")";
                        e.Row.Cells[12].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[13].Visible = Convert.ToBoolean(ShowLotSize);
                    }
                }
                else if ((sender as GridView).ID == "grdRevisionHistory")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //(e.Row.FindControl("lnkRevisionPrint") as LinkButton).PostBackUrl = "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "CEH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "CEH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.CQTN + "&APPSUBTYPE=";
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("OnClick", "javascript:return OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "SOH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "SOH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.SO + "&APPSUBTYPE=" + "');");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("href", "javascript:void(0);");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Text = DataBinder.Eval(e.Row.DataItem, "SOH_NO").ToString();
                    }
                }
                if (((GridView)sender).ID == "grdUploads")
                {
                    int slno;
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
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

            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnInActive.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintSO.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAlert.PreRender += new EventHandler(btnAction_PreRender);
            //this.imbAutogenerateLotNo.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            this.btnClearItem.PreRender += new EventHandler(btnAction_PreRender);
            this.btnApply.PreRender += new EventHandler(btnAction_PreRender);
            this.imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintIO.PreRender += new EventHandler(btnAction_PreRender);
            this.imbShowSCdtlcost.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnInActive.Load += new EventHandler(btnAction_Load);
            this.btnPrintSO.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnAlert.Load += new EventHandler(btnAction_Load);
            //this.imbAutogenerateLotNo.Load += new EventHandler(btnAction_Load);
            this.btnAddItem.Load += new EventHandler(btnAction_Load);
            this.btnClearItem.Load += new EventHandler(btnAction_Load);
            this.btnApply.Load += new EventHandler(btnAction_Load);
            this.imgPopupAdd.Load += new EventHandler(btnAction_Load);
            this.btnPrintIO.Load += new EventHandler(btnAction_Load);
            this.imbShowSCdtlcost.Load += new EventHandler(btnAction_Load);
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
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
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
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
                if (IsQuotationContract)
                {
                    //ImageButton btnEditItem;
                    ImageButton btnRemoveItem;
                    ImageButton imbTaxRemove;

                    foreach (GridViewRow gvr in grdItemDetails.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            //btnEditItem = gvr.FindControl("btnEditItem") as ImageButton;
                            btnRemoveItem = gvr.FindControl("btnRemoveItem") as ImageButton;
                            //btnEditItem.Visible = false;
                            btnRemoveItem.Visible = false;
                        }
                    }
                    //foreach (GridViewRow gvr in grdTaxDetails.Rows)
                    //{
                    //    if (gvr.RowType == DataControlRowType.DataRow)
                    //    {
                    //        imbTaxRemove = gvr.FindControl("imbTaxRemove") as ImageButton;
                    //        //imbTaxRemove.Visible = false;
                    //    }
                    //}

                    //btnAddItem.Visible = false;
                    //btnClearItem.Visible = false;
                    //btnApply.Visible = false;
                    //imgPopupAdd.Visible = false;
                    txtBookingDate.Enabled = false;
                    txtShipping.Enabled = false;
                    txtPriceAdj.Enabled = false;
                }
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSpec", "$(document).ready(function(){ShowSpec(0);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);

                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //Visibility Checking of ddlSaleOrderSubtype           
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaleOrderSubtype", "$(document).ready(function () { ShowHideSOSubtype();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            if (isItemBind)
            {
                if (IsValidButton("AUTOGENERATELOTNO", "PageAction_Entry"))
                    AutoGenerateLotNo();
                else
                    SetFieldValues(ControlsEnum.SALEORDERDETAIL);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            BANKDETAILS,
            SALEORDER,
            SALEORDERHEADER,
            SALEORDERDETAIL,
            PACKINGMATERAILSALEORDERDETAIL,
            SALEORDERDETAILPACKSPEC,
            FROMPORT,
            ORIGINOFGOODS,
            NOTIFYPARTY,
            CONSIGNEE,
            SOTYPE,
            SHIPPINGAGENT,
            TRANSHIPMENT,
            SHIPBY,
            DELIVERYTERMS,
            PAYMENTTERMS,
            SPECIALCAUSE,
            USERCUSTOMER,
            CUSTOMER,
            //ARTWORK,
            INSPECTION,
            EXPORTDOC,
            CONTRACTTERMS,
            TAXPOPUPGRID,
            EXCHANGERATE,
            CUSTOMERADDRESS,
            CUSTOMERCONTRACT,
            COPYCONTRACT,
            BRANDRATE,
            CUSTOMERPRODUCT,
            SELECTEDITEM,
            SELECTEDPACKINGMATERAILITEM,
            TAXTYPES,
            TAXSETTINGS,
            PACKINGSPEC,
            PRODUCT,
            REVISIONHISTORY,
            COMPANY,
            CBMCONFIG,
            SOSUBTYPE,
            AUTOTAXPOPUP,
            AUTOTAXPOPUPITEMWISE,
            AGENT,
            CUSTOMTAXSETTINGS,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            REBINDSALEORDER,
            CLIENTCODE,
            CONTAINERTYPEDETAILS,
            DELIVERYTERMSBYPK,
            PAYMENTTERMSBYPK,
            SPECIALCAUSEBYPK,
            PACKINGSPECDETAILS,
            STRAPPINGCOLOR,
            STANDARD,
            TOPORT,
            ITEMREFDTL,
            SALESCOST,
            ALLSALESCOST,
            QTYPOPUP,
            TAXCHECKBOX,
            PACKSPECSELECTED,
            CLEARPACKMATERIALITEM,
            SHIPMENTTERMS,
            SHIPMENTTERMSBYPK,
            QUOTATION,
            CRMCUSTOMER
        }
        private enum WorkFlowStatusEnum
        {
            Reviewed = 4,
            SendBackForApprove = 11
        }
        private enum SCStatusEnum
        {
            IOgenerate = 2,
            Accepted = 5,
            AcceptedMinfo = 12,
            FinApproved = 20

        }
        #endregion
    }
}
