namespace ERP.Utilities
{
    /// <summary>
    /// Constant string formats
    /// </summary>
    public class CommonConstants
    {
        public const string SELECT_ALL_VAL = "0";
        public const string SELECTVAL = "-1";
        public const string SELECTTEXT = "Select";
        public const string ALL = "All";
        public const string SELECT_VALUE_ZERO = "0";
        public const string SELECT_VALUE_ONE = "1";
        public const string DATEFORMAT = "dd-MMM-yyyy";

        public const string SHORT_DATEFORMAT = "dd-M-y";
        public const string TIMEFORMAT = "HH:mm";
        public const string ASSET = "1";
        public const string GENERAL = "1";
        public const string DEPRECIATIONINDEX = "2";
        public const string UPLOAD_FOLDER = "Upload\\BRAND";
        public const string EXELSHEETNAME = "Template$";//Sheet name for exel import
        public const string EXELSHEETNAME_IGCL = "time$";//Sheet name for exel import(IGCL)
        public const string EXELSHEETNAME_TRANSACTION = "Transactions$";//Sheet name for Transactions exel import(DIRECT PAYMENT)
        public const string MULTIBATCH = "Multiple Batches";
        public const string MULTIBATCH_VALUE = "-2";

        public const string RESETPASSWORD = "ResetPassword";
        public const string IS_RPT_SP = "SP";
        public const string F_AUTO_PK = "PK";
        public const string F_AUTO_VALUE = "VALUE";
        public const string SORT_ASC = "ASC";
        public const string SORT_DESC = "DESC";

        // For Packing Spec
        public const int IPPacking = 1;
        public const int OPPacking = 2;
        public const int PackingType = 5;
        public const int PouchPacking = 4;
        public const int PackingMaterialType = 6;

        //For Voucher Template Type
        public const int VoucherTemplate = 1;

        // For Product Properties
        public const int Type = 1;
        public const int Thickness = 2;
        public const int Category = 3;
        public const int Surface = 4;
        public const int Shade = 5;
        public const int Classification = 6;
        public const int Size = 7;
        public const int Length = 8;

        public const string ACTIVE = "1";
        public const string HASPK = "2";
        public const string PlanStage = "PLAN STAGE";

        public const bool CON_DEFAULT = true;

        // For Control Special condition
        public const string CTL_SPL_COND = "CL";

        public const int SequenceNumber = 1;

        public const string DEFAULT_STORE = "1";

        public const string CUS_ADDRESS_TYPE = "CUS ADDRESS TYPE";

        public const string HTML_SPACE = "&nbsp;";
        public const string HTML_NEW_LINE = "<br/>";


        public const string DEFECT_LIABILITY_PERIOD = "DEFECT LIABILITY PERIOD";

        #region Parameters
        public const string RETURNVALUE = "PRetVal";
        public const string USERPK = "P_USER_PK";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string DEPARTMENT = "P_DEPT";
        public const string MODULE = "P_MODULE";
        //public const string P_USRGRPFLAG = "P_FLAG";
        public const string P_USRGRPFLAG = "PFlag";
        public const string RETURNVAL = "P_RET_VAL";
        public const string LASTMODDATE = "P_LAST_MOD_DT";
        public const string LASTMODDATETIME = "LAST_MOD_DT";
        public const string CREATEDBY = "P_Crtd_By";
        public const string ACTIVESTATUS = "P_Active";

        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_PAGENO = "P_PAGE_NO";
        public const string P_PAGENUM = "P_PAGE_NUM";
        public const string P_PAGESIZE = "P_PAGE_SIZE";

        public const string P_FROM_DATE = "P_FROM_DATE";
        public const string P_TO_DATE = "P_TO_DATE";

        public const string P_SEARCH_FIELD = "P_SER_NAME";
        public const string P_SEARCH_VALUE = "P_SER_VAL";
        public const string P_PAGE_NO = "P_PAGE_NO";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string P_SORT_EXPRESSION = "P_SORT_BY";
        public const string P_SORT_DIRECTION = "P_SORT_DIR";

        public const string SEARCH_TYPE = "P_FLD_NAME";
        public const string SEARCH_VALUE = "P_VALUE";
        public const string SEARCH_MENU = "P_MNU_NAME";
        public const string SEARCH_KEY = "SEARCH_KEY";

        #endregion
        #region Fields

        public const string F_BIZUNIT = "DPT_BIZUNIT";
        public const string F_BIZUNITNAME = "BZU_NAME";
        public const string F_DEPARTMENT = "DPT_PK";
        public const string F_DEPARTMENTNAME = "DPT_NAME";

        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";

        public const string F_PROCESS = "PROCESS_PK";
        public const string F_PAGE = "PAG_PK";
        public const string F_APP_PK = "refApplication";
        public const string F_APP_PROCESS = "refProcess";


        #endregion
        #region SPs

        public const string SP_GETSBUDEPARTMENT = "SPADM_USER_DEPT_BIZUNIT_GET_KV";
        //public const string SP_GETUSERGROUPS = "ADM_USER_GROUP_KV";
        public const string SP_GETUSERGROUPS = "SpWkfUserGroupGetKV";
        public const string SP_GETPROCESSLIST = "SPWKF_USER_PROC_AUTO";

        #endregion
    }



    /// <summary>
    /// Report Type
    /// </summary>
    public class ReportType
    {
        public const string CrystalReport = "rpt";
        public const string RDLCReport = "rdlc";
        public const string HTMLReport = "xml";
        public const string TelerikReport = "trdp";
    }


    /// <summary>
    /// AppSettings Strings 
    /// </summary>
    public class ConfigStrings
    {
        public const string GcomsModule = "gComsModule";
        public const string GERPModule = "gERPModule";
    }

    /// <summary>
    /// Http Request Parameters
    /// </summary>
    public class RequestParameters
    {
        public const string SearchValue = "SearchValue";
        public const string SearchBy = "SearchBy";
        public const string SearchType = "SearchType";
        public const string ProcessPK = "ProcessPK";
        public const string Make = "Make";
        public const string Type = "Type";
        public const string BasisType = "BasisType";
        public const string FilterType = "FilterType";
        public const string Project = "Project";
        public const string Group = "Group";
        public const string CompoundPK = "CompoundPK";
        public const string VoucherType = "VoucherType";
        public const string AccType = "AccType";
        public static string RptID = "RptID";
        public static string ServiceType = "ServiceType";
        public static string CurrencyVendor = "CurrencyVendor";
        public static string ExcDate = "ExcDate";
        public static string CustomerID = "CustomerID";
        public static string Category = "Category";
        public static string IsProductRequired = "IsProductRequired";
        public static string BrandID = "BrandID";
        public static string itemPK = "itemPK";
        public static string OpParam = "OpParam";
        public static string UserLogin = "UserLogin";
        public static string Dep = "Dep";

        public static string Status = "Status";
        public static string VendorPk = "VendorPk";
        public static string ParentPk = "ParentPk";

        public const string FieldName = "FieldName";
        public const string Location = "Location";
        public const string HasQuery = "HasQuery";
        public const string Query = "Query";
        public const string RelCtrl = "RelCtrl";
        public const string RelCtrlValue = "RelCtrlValue";
        public const string QueryText = "QueryText";
        public const string SCID = "SCID";
        public const string PayElmntValue = "PayElmntValue";
        public const string ItemGrade = "ItemGrade";
        public const string EmpCategory = "EmpCategory";
        public const string EmpBranch = "EmpBranch";
        public const string EmpType = "EmpType";
        public const string EmploymentType = "EmploymentType";
        public const string EmpCompany = "EmpCompany";
        public const string EmpDept = "EmpDept";
        public const string BinSubType = "BinSubType";
        public const string LinkedProduct = "LinkedProduct";
        public const string EmpDesignation = "EmpDesignation";
        public const string BranchByUser = "BranchByUser";
        public const string PaymentMode = "PaymentMode";
        public const string ProcessMode = "ProcessMode";
        public const string EmpCurrency = "EmpCurrency";
        public const string Country = "Country";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string EmpPayrollType = "EmpPayrollType";
        public const string ItmCategory = "ItmCategory";
        public const string PackSpec = "PackSpec";
        public const string SubType = "SubType";
        public const string ItmCategoryPK = "ItmCategoryPK";
        public const string ItemPK = "ItemPK";
        public const string JobCategory = "JobCategory";
        public const string JobLevel = "JobLevel";
        public const string PAGE_URL = "PAGE_URL";
        public const string InvoicePk = "InvoicePk";
        public const string ModulePK = "ModulePK";
        public const string InvGroup = "InvGroup";
        public const string InvCategory = "InvCategory";
        public const string EnableWO = "EnableWO";
        public const string SIType = "SIType";
        public const string SaleFromPort = "SaleFromPort";
        public const string SaleToPort = "SaleToPort";
        public const string PurFromPort = "PurFromPort";
        public const string PurToPort = "PurToPort";
        public const string materialID = "materialID";
        public const string batchPK = "batchPK";
        public const string IssueAgainst = "IssueAgainst";
        public const string InvType = "InvType";
        public const string PrdCategoryPk = "PrdCategoryPk";
        public const string PrdTypePk = "PrdTypePk";
        public const string CusPk = "CusPk";
        public const string IsSBUVendor = "IsSBUVendor";
        public const string IsSBUCustomer = "IsSBUCustomer";
        public const string IsSBUBank = "IsSBUBank";
        public const string OperationPK = "OperationPK";
        public const string Role = "Role";
        public const string StatusVal = "StatusVal";
        public const string SBU = "sbu";
        public const string CompanyPK = "CompanyPK";
        public const string MenuType = "MenuType";
        public const string AccountPk = "P_COA_PK";
        public const string BudgetPk = "BudgetPk";

    }

    public class sortDirection
    {
        public const string Ascending = "Asc";
        public const string Descending = "Desc";

    }
    /// <summary>
    /// Viewstate Strings
    /// </summary>
    public class ViewstateStrings
    {

        public const string CurrTab = "CurrTab";
        public const string CurrGrpType = "CurrGrpType";
        public const string ReferanceID = "ReferanceID";

        //HRMS - Employee Skills
        public const string categoryId = "categoryId";

        //Vat Sale Export
        public const string vatSaleItemPK = "vatSaleItemPK";

        public const string InvType = "InvType";
        //Sale Order
        public const string dirState = "dirState";
        public const string SodPK = "SodPK";
        public const string ScPK = "ScPK";
        public const string ScDetailsPK = "ScDetailsPK";

        //
        public const string POTotalTaxAmount = "POTotalTaxAmount";
        public const string PrevTotalPOAmountDiscount = "PrevTotalPOAmountDiscount";
        public const string PrevSubTotalPOAmount = "PrevSubTotalPOAmount";
        public const string InvoiceActionState = "InvoiceActionState";
        public const string EntryState = "EntryState";
        public const string SaleOrderHeaderSession = "SaleOrderHeaderSession";
        public const string DirectSaleOrderHeaderSession = "DirectSaleOrderHeaderSession";
        public const string DirectSaleOrderDetailSession = "DirectSaleOrderDetailSession";
        public const string tempsaleOrderTaxHdrList = "tempsaleOrderTaxHdrList";
        public const string PreEntryState = "PreEntryState";
        public const string TransactionType = "TransactionType";
        public const string ReportCurr = "ReportCurr";
        public const string CurrPK = "CurrPK";
        public const string CurrDtlPk = "CurrDtlPk";
        public const string CurrDtlSlNo = "CurrDtlSlNo";
        public const string AmendPage = "AmendPage";
        public const string AmendAdd = "AmendAdd";
        public const string AmendPagination = "AmendPagination";
        public const string IsImport = "IsImport";      
        public const string IsAdd = "IsAdd";
        public const string dtBudgetDetails = "dtBudgetDetails";
        public const string IsSbu = "IsSbu";
        public const string EditPK = "EditPK";
        public const string IsCopySO = "IsCopySO";
        public const string AllocateCurrPK = "AllocateCurrPK";
        public const string JurCurrPK = "JurCurrPK";
        public const string ModulePK = "ModulePK";
        public const string UserPK = "UserPK";
        public const string UserGroupPK = "UserGroupPK";
        public const string ReportPK = "ReportPK";
        public const string BrandPK = "BrandPK";
        public const string CusPK = "CusPK";
        public const string ShippingPK = "ShippingPK";
        public const string PlantPK = "PlantPK";
        public const string CurrSlNo = "CurrSlNo";
        public const string CurrDocSlNo = "CurrDocSlNo";
        public const string CurrVenSlNo = "CurrVenSlNo";
        public const string Approved = "Approved";
        public const string ItemStatus = "ItemStatus";
        public const string Group = "Group";
        public const string Posted = "Posted";
        public const string DetailPK = "DetailPK";
        public const string TransactionPK = "TransactionPK";
        public const string JournalizePK = "JournalizePK";
        public const string Invoice = "Invoice";
        public const string FCReverse = "FCReverse";
        public const string AGTCommV = "AGTCommV";
        public const string FCRGroup = "FCRGroup";
        public const string RefType = "RefType";
        public const string SelectedVendors = "SelectedVendors";
        public const string SelectedCurrency = "SelectedCurrency";
        public const string SelectedInvoiceType = "SelectedInvoiceType";
        public const string SelectedPOGroup = "SelectedPOGroup";
        public const string SelectedType = "SelectedType";
        public const string SelectedPOPKs = "SelectedPOPKs";
        public const string POGroup = "POGroup";
        public const string PICategory = "PICategory";
        public const string IsPartyNo = "IsPartyNo";
        public const string CheckListData = "CheckListData";
        public const string SOGroup = "SOGroup";
        public const string SICategory = "SICategory";
        public const string SelectedCustomers = "SelectedCustomers";
        public const string CustomerList = "CustomerList";
        public const string ItemList = "ItemList";
        public const string CurrentAction = "CurrentAction";
        public const string SelectedCustomersForAdvInv = "SelectedCustomersForAdvInv";
        public const string SelectedCustomersForDO = "SelectedCustomersForDO";
        public const string SelectedCustomersType = "SelectedCustomersType";
        public const string ShippingSCPKs = "ShippingSCPKs";
        public const string CurrMpgPK = "CurrMpgPK";
        public const string CurrFtrPK = "CurrFtrPK";
        public const string VoucherType = "VoucherType";
        public const string AccountType = "AccountType";
        public const string SaveFlag = "SaveFlag";
        public const string ActivityHeader = "ActivityHeader";
        public const string ContractsHeader = "ContractsHeader";
        public const string PageIndex = "PageIndex";
        public const string fromType = "fromType";
        public const string TotalPages = "TotalPages";
        public const string PageSize = "PageSize";
        public const string PageNo = "PageNo";
        public const string RowCount = "RowCount";
        public const string BudgetStatus = "BudgetStatus";
        public const string SortBy = "SortBy";
        public const string SortByGrd = "SortByGrd";
        public const string ThenBy = "ThenBy";
        public const string SortByTax = "SortByTax";
        public const string SortDirection = "SortDirection";
        public const string ThenDirection = "ThenDirection";
        public const string LastModifiedTime = "LastModifiedTime";
        public const string ShowMaximumCharacters = "ShowMaximumCharacters";
        public const string FilterBy = "FilterBy";
        public const string FilterValue = "FilterValue";
        public const string RptID = "RptID";
        public const string PoId = "PoId";
        public const string PoDtlPK = "PoDtlPK";
        public const string SoId = "SoId";
        public const string DoId = "DoId";
        public const string SoDtlId = "SoDtlId";
        public const string DelQtySum = "DelQtySum";
        public const string VendorID = "VendorID";
        public const string CustomerID = "CustomerID";
        public const string SaleOrderType = "SaleOrderType";
        public const string InvoiceId = "InvoiceId";
        public const string TrxPK = "TrxPK";
        public const string RefPK = "RefPK";
        public const string AppType = "AppType";
        public const string DropDownID = "DropDownID";
        public static string CngVal = "CngVal";
        public static string Status = "Status";
        public static string CompanyPK = "CompanyPK";
        public static string CurrMode = "CurrMode";
        public static string SelectedPosCount = "SelectedPosCount";
        public static string SelectedSosCount = "SelectedSosCount";
        public static string SelectedInvoicesCount = "SelectedInvoicesCount";
        public static string SelectedInvoicesCrDrCount = "SelectedInvoicesCrDrCount";
        public static string PayNow = "PayNow";
        public static string Tax = "Tax";
        public static string InvoiceTax = "InvoiceTax";
        public static string AdjustmentAmount = "AdjustmentAmount";
        public static string LineItemTaxEnabled = "LineItemTaxEnabled";
        public static string IsCustomTaxEnabled = "IsCustomTaxEnabled";
        public static string AppliedInvPkList = "AppliedInvPkList";
        public static string IsDoModified = "IsDoModified";
        public static string ReloadInvoice = "ReloadInvoice";
        public static string IsDeleted = "IsDeleted";
        public static string RefreshInvoice = "RefreshInvoice";
        public static string IsAdvInvHasTax = "IsAdvInvHasTax";
        public static string IsShowEffRateInPOInvoice = "IsShowEffRateInPOInvoice";
        public static string IsCustomer = "IsCustomer";
        public static string PaymentModeDetailsList = "PaymentModeDetailsList";
        public static string PaymentModeRowIndex = "PaymentModeRowIndex";
        public static string CurrencyPk = "CurrencyPk";
        public static string IsPaymentModeAdded = "IsPaymentModeAdded";
        public static string PaymentCrdrList = "PaymentCrdrList";
        public static string ReceiptCrdrList = "ReceiptCrdrList";
        public static string ReceiptPk = "ReceiptPk";
        public const string SelectedInvoices = "SelectedInvoices";
        public const string SelectedPO = "SelectedPO";
        public static string IsCreditNoteApplied = "IsCreditNoteApplied";
        public static string IsDebitNoteApplied = "IsDebitNoteApplied";
        public static string admConfigMstListCrdrType = "admConfigMstListCrdrType";
        public static string finPaymentTrxMpgList = "finPaymentTrxMpgList";
        public static string PaymentInvDetList = "PaymentInvDetList";
        public static string finInvoiceHdrList = "finInvoiceHdrList";
        public static string DOCancelStatus = "DOCancelStatus";
        public static string ShowTaxForMiscInv = "ShowTaxForMiscInv";
        public static string SCSubTotalAmount = "SCSubTotalAmount";
        public static string SCTotalDiscountAmount = "SCTotalDiscountAmount";
        public static string AssetServiceRequestDetailList = "AssetServiceRequestDetailList";
        public static string AssetServiceOrderDetailList = "AssetServiceOrderDetailList";
        public static string AssetServiceOrderHeader = "AssetServiceOrderHeader";
        public const string BatchNoPK = "BatchNoPK";
        public const string IsToPortDdlShow = "IsToPortDdlShow";
        public const string IsEnableTypeFilter = "IsEnableTypeFilter";
        public static string DirectDeliveryOrderHeader = "DirectDeliveryOrderHeader";
        public const string SelectedPackingSpecs = "SelectedPackingSpecs";
        public const string GetConfigrate = "GetConfigrate";
        public const string SearchBy = "SearchBy";
        public const string SP_NAME = "SP_NAME";
        public const string AgentCMSelectType = "AgentCMSelectType";

        //For Tax group mapping
        public const string CountryPK = "CountryPK";
        public const string CurrTaxGroupPK = "CurrTaxGroupPK";
        public const string CurrTaxMapPK = "CurrTaxMapPK";
        public const string CurrAirportTaxMapPK = "CurrAirportTaxMapPK";
        public const string TaxGroupPK = "TaxGroupPK";
        public const string Currency = "Currency";
        public const string Category = "Category";

        public const string TempProductRateSession = "TempProductRateSession";
        public const string CustomerRateHeaderSession = "CustomerRateHeaderSession";
        public const string groupCount = "groupCount";

        //Dash Board
        public const string RegionPK = "RegionPK";
        public const string RoutPK = "RoutPK";
        public const string SectorPK = "SectorPK";

        //External Material Issue (Multiple)
        public const string EMIMultipleDetailsList = "EMIMultipleDetailsList";
        public const string CurrItemPK = "CurrItemPK";

        //For Brand Rates
        public const string SelectedProducts = "SelectedProducts";
        public const string CustomerRates = "CustomerRates";
        public const string ProductRates = "ProductRates";
        public const string CustomerMails = "CustomerMails";
        public const string CustomerEmailList = "CustomerEmailList";
        public const string BrkPK = "BrkPK";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string IsBrandItemInsert = "IsBrandItemInsert";
        public const string IsAgentCommissionFormula = "IsAgentCommissionFormula";
        public const string BrandItemInsert = "BrandItemInsert";

        //For Maintaining Contract Element PK
        public const string ContractElementPK = "ContractElementPK";
        public const string ParentPK = "ParentPK";
        public const string ParentBasisPK = "ParentBasisPK";
        public const string CurrFilter = "CurrFilter";
        public const string HasParentPK = "HasParentPK";

        //For Contract Popups
        public const string CurrElementPK = "CurrElementPK";
        public const string LastModifiedTimeElement = "LastModifiedTimeElement";
        public const string LastModifiedTimeBasis = "LastModifiedTimeBasis";
        public const string LastModifiedTimeSlab = "LastModifiedTimeSlab";
        public const string CurrBasisPK = "CurrBasisPK";
        public const string CurrSlabPK = "CurrSlabPK";
        public const string CurrDetailPK = "CurrDetailPK";
        public const string CurrSlabDetailPK = "CurrSlabDetailPK";
        public const string SlabBasisType = "SlabBasisType";
        public const string ControlType = "ControlType";
        public const string LastAction = "LastAction";
        public const string ContractSlabBasisMpgList = "ContractSlabBasisMpgList";
        public const string DefaultSlabBasis = "DefaultSlabBasis";
        public const string DefaultContractCurrency = "DefaultContractCurrency";

        // For vendor Pages
        public const string VendorPK = "VendorPK";
        public const string VendorCode = "VendorCode";
        public const string VendorName = "VendorName";
        public static string IsVendorSelected = "IsVendorSelected";
        public const string IsApply = "IsApply";

        //For Documentation
        public const string Module = "Module";
        public const string ModuleRef = "ModuleRef";
        public const string Sequence = "Sequence";
        public const string ModuleRefCode = "ModuleRefCode";
        public const string ModuleRefName = "ModuleRefName";
        public const string ModuleRefCodeText = "ModuleRefCodeText";
        public const string ModuleRefNameText = "ModuleRefNameText";
        public const string AttFolder = "AttFolder";

        //For Activity
        public static string ActivityPK = "ActivityPK";
        public static string StationPK = "StationPK";
        public static string CurrencyFormat = "CurrencyFormat";
        public static string HasDelay = "HasDelay";

        //For SGHA
        public static string SGHAGroup = "SGHAGroup";
        public static string SGHA_HDRPK = "SGHA_HDRPK";
        public static string ParentSequenceNo = "ParentSequenceNo";

        //For types
        public static string TypeGroup = "TypeGroup";
        public static string ItemPK = "ItemPK";
        public static string CurrentUserPK = "CurrentUserPK";
        public static string Level = "Level";

        //For flight delay code
        public static string Agency = "Agency";
        //For Charge Calculation
        public static string IsSaved = "IsSaved";

        //Bank
        public static string BankPK = "BankPK";

        //Activity Service Tab 
        public static string ServiceTypeVal = "ServiceTypeVal";
        public static string ServiceTabName = "ServiceTabName";

        //RFQ
        public static string RFQPK = "RFQPK";
        public const string DisableItemTax = "DisableItemTax";
        public const string DisableItemDiscount = "DisableItemDiscount";
        public const string TaxPK = "TaxPK";
        public const string isWarnedCBM = "isWarnedCBM";
        public static string SelectedResponsePK = "SelectedResponsePK";
        public static string IsEditMode = "IsEditMode";
        public static string IsHeaderTax = "IsHeaderTax";
        public static string SelectedTaxText = "SelectedTaxText";
        public static string RFQItemDetailParameters = "RFQItemDetailParameters";
        public static string TempRFQResponseHeader = "TempRFQResponseHeader";
        public static string xmlDocSO = "xmlDocSO";
        public static string SelectedItemPK = "SelectedItemPK";
        public static string SelectedPOPK = "SelectedPOPK";
        public static string SelectedDtlPK = "SelectedDtlPK";
        public static string TabCode = "TabCode";
        public static string SelectedPK = "SelectedPK";
        public static string ControlPK = "ControlPK";
        public static string EntityName = "EntityName";
        public static string RelatedControlID = "RelatedControlID";
        public static string ChildGridName = "ChildGridName";
        public static string SelectedParentGrid = "SelectedParentGrid";
        public static string SelectedParentPK = "SelectedParentPK";
        public static string DynamicTabName = "DynamicTabName";
        public static string DynamicTabDesc = "DynamicTabDesc";
        public static string CusCountryCode = "CusCountryCode";
        public static string AttachmentFileName = "AttachmentFileName";
        public static string TempQuotationHeader = "TempQuotationHeader";
        public static string SelectedCusItemPK = "SelectedCusItemPK";
        public static string SelectedQuotationPK = "SelectedQuotationPK";
        public static string CusPk = "CusPk";

        //CheckList
        public const string CheckListGroupPK = "CheckListGroupPK";
        public const string CheckListItemPK = "CheckListItemPK";
        public const string CheckListTypePK = "CheckListTypePK";

        //Delivery Order
        public static string DespatchID = "DespatchID";

        //Sale Order
        public const string IsCustomerUser = "IsCustomerUser";
        public const string Iscarton = "Iscarton";
        public const string IsTaxInSBU = "IsTaxInSBU";
        public const string IsTaxForOtherCharge = "IsTaxForOtherCharge";
        public const string IsMappedAccounts = "IsMappedAccounts";
        public const string IsDeliveryTermsEnable = "IsDeliveryTermEnable";
        public const string IsDeliveryTermsCheck = "IsDeliveryTermsCheck";
        public const string IsExportExcel = "IsExportExcel";
        public const string IsInvoiceTerms = "IsInvoiceTerms";
        public const string IsExcelPrint = "IsExcelPrint";
        public const string IsBrand = "IsBrand";
        public const string IsMaterail = "IsMaterail";
        public const string IsItem = "IsItem";
        public const string IsQuotationContract = "IsQuotationContract";
        public static string CurrQuotationPK = "CurrQuotationPK";
        public static string CurrSOPK = "CurrSOPK";
        public static string vndpk = "vndpk";
        public const string IsTaxForOtherChargeSales = "IsTaxForOtherChargeSales";
        public const string IsTaxForOtherChargePurchase = "IsTaxForOtherChargePurchase";
        public const string IsRepeatSIheader = "IsRepeatSIheader";
        public static string SelectedSosCountForDO = "SelectedSosCountForDO";
        public static string SelectedSosCountForAdvInv = "SelectedSosCountForAdvInv";
        public static string SelectedCurrencyForDO = "SelectedCurrencyForDO";
        public static string SelectedCurrencyForAdvInv = "SelectedCurrencyForAdvInv";
        public static string CurrencyForDO = "CurrencyForDO";
        public static string CurrencyForAdvInv = "CurrencyForAdvInv";
        public static string SoIdForDO = "SoIdForDO";
        public static string SoIdForAdvInv = "SoIdForAdvInv";
        public static string CustomerIDForDO = "CustomerIDForDO";
        public static string CustomerIDForAdvInv = "CustomerIDForAdvInv";
        public static string SOInvoicePK = "SOInvoicePK";
        public const string SOUploadList = "SOUploadList";
        public const string DSOUploadList = "DSOUploadList";
        public static string ShipdateUpdation = "ShipdateUpdation";
        public static string CartonDecimal = "CartonDecimal";
        public static string SOInvoiceUploadList = "SOInvoiceUploadList";
        public static string ContineInvoiceAmtGreaterThanSCAmt = "ContineInvoiceAmtGreaterThanSCAmt";
        public static string IsInvoiceGSTEnable = "IsInvoiceGSTEnable";
        public static string IsAgentInvoiceSCApproveList = "IsAgentInvoiceSCApproveList";
        public static string SIContainerDefaultVal = "SIContainerDefaultVal";

        //For Dashboard
        public const string SelectedSlno = "SelectedSlno";
        public const string CurrPage = "CurrPage";
        public const string CurrRow = "CurrRow";
        public const string RowIndex = "RowIndex";
        public const string ItemRowIndex = "ItemRowIndex";
        public const string CurrCell = "CurrCell";
        public const string CurrChartType = "CurrChartType";
        public const string CurrParam = "CurrParam";
        public const string ParamsTotalPages = "ParamsTotalPages";
        public const string ParamsSortBy = "ParamsSortBy";
        public const string ParamsSortDirection = "ParamsSortDirection";
        public const string ParamsPageIndex = "ParamsPageIndex";
        public const string PripertyLastModifiedTime = "PripertyLastModifiedTime";
        public const string ZoomPK = "ZoomPK";
        public const string IsRdlc = "IsRdlc";
        public const string CurrentPageSize = "CurrentPageSize";
        public const string CurrParent = "CurrParent";
        public const string fromDate = "fromDate";
        public const string toDate = "toDate";
        public const string paymentMpgPK = "paymentMpgPK";
        public const string invoicePK = "invoicePK";
        public const string PayNowAmount = "PayNowAmount";
        public const string paynowtax = "paynowtax";
        public const string ReceiptMpgPK = "ReceiptMpgPK";
        public const string AdjnNowAmount = "AdjnNowAmount";
        public const string TrxIndex = "TrxIndex";
        public const string DicDropdownValueMpg = "DicDropdownValueMpg";
        public const string TotalPagesFinishedGoods = "TotalPagesFinishedGoods";
        public const string TotalPagesSelectedOrder = "TotalPagesSelectedOrder";

        //OrderTracker
        public const string SohPK = "SohPK";
        public const string Product = "Product";
        public const string TotalAllocatedQty = "TotalAllocatedQty";
        public const string TotalPlannedQty = "TotalPlannedQty";
        public const string TotalDespatchedQty = "TotalDespatchedQty";
        public const string TotalPackedQty = "TotalPackedQty";
        public const string BalProduce = "BalProduce";
        public const string BalPlan = "BalPlan";
        public const string BalDispatch = "BalDispatch";
        public const string TotalPagesCustomersOrders = "TotalPagesCustomersOrders";
        public const string TotalPagesAllocated = "TotalPagesAllocated";
        public const string TotalPagesPlanned = "TotalPagesPlanned";
        public const string TotalPagesDespatched = "TotalPagesDespatched";

        //Shipping Plan- Loading Plan       
        public const string ShippingPlanPK = "ShippingPlanPK";
        public const string ShippingUploadType = "ShippingUploadType";
        public const string ShippingUploadList = "ShippingUploadList";
        public const string BLUploadList = "BLUploadList";
        public const string POUploadList = "POUploadList";
        public const string TabIndexDr = "TabIndexDr";
        public const string TabIndexCr = "TabIndexCr";
        public const string TypePK = "TypePK";
        public const string TypeCode = "TypeCode";
        public const string TypeRef = "TypeRef";
        public const string TypeText = "TypeText";
        public const string JournalizeRefPK = "JournalizeRefPK";
        public const string SelectedInvoicesCrDr = "SelectedInvoicesCrDr";
        public const string SelectedSalesInvoices = "SelectedSalesInvoices";
        public static string ReceiptAdjnList = "ReceiptAdjnList";
        public static string InvoiceSOSplitList = "InvoiceSOSplitList";
        public static string FinReceiptCusAllocationList = "FinReceiptCusAllocationList";
        public static string PaymentAdjnList = "PaymentAdjnList";
        public const string SCBrandPK = "SCBrandPK";
        public static string InvoicePOSplitList = "InvoicePOSplitList";
        public const string dsLoadingPlan = "dsLoadingPlan";
        public const string TrxDate = "TrxDate";
        public const string TypePartyName = "TypePartyName";
        public const string IsWkfCompleted = "IsWkfCompleted";
        public const string PackingPk = "PackingPk";
        public const string HasWkfPermission = "HasWkfPermission";
        public const string BOIStatus = "BOIStatus";
        public const string TrxStatus = "TrxStatus";

        //Expenses
        public const string ExpensePK = "ExpensePK";
        public const string Expense = "Expense";
        public const string VoucherTemplateMode = "VoucherTemplateMode";
        public const string VoucherTemplatePK = "VoucherTemplatePK";
        public const string PaymentCategory = "PaymentCategory";
        public const string IsBaseCurrency = "IsBaseCurrency";
        public const string JournalType = "JournalType";
        public const string GainLossCalculationMode = "GainLossCalculationMode";
        public const string BCEnable = "BCEnable";
        public const string IsNewDummy = "IsNewDummy";
        public const string ReturnURL = "ReturnURL";
        public const string TypeForNumberGenaration = "TypeForNumberGenaration";

        public const string VoucherApplicationType = "VoucherApplicationType";

        public const string CloseVoucherPopup = "CloseVoucherPopup";
        public const string VoucherDeleteStatus = "VoucherDeleteStatus";
        public const string IsDummyAdd = "IsDummyAdd";
        public const string PostBackFlag = "PostBackFlag";
        public const string SelectedPOTypes = "SelectedPOTypes";
        public const string IsVoucherDeleted = "IsVoucherDeleted";
        public const string CrDrMpgPK = "CrDrMpgPK";
        public const string Iscont = "Iscont";
        public const string IsOCded = "IsOCded";
        public const string IsOCEdit = "IsOCEdit";
        public const string IsBizUnitCur = "IsBizUnitCur";
        public const string SBUID = "SBUID";
        public const string CurrWthPK = "CurrWthPK";
        public const string DocAttachList = "DocAttachList";
        public const string DSAUploadList = "DSAUploadList";
        public const string EditTempExpenseHeaderSession = "EditTempExpenseHeaderSession";

        //Purchase Invoice - OtherCharge
        public const string POTotalOtherAmount = "POTotalOtherAmount";
        public const string POTotalInvOtherAmount = "POTotalInvOtherAmount";
        public const string POTotalBalanceOtherAmount = "POTotalBalanceOtherAmount";

        //Purchase Invoice - Deduction
        public const string DedTotalAllocateNowFooterSplit = "DedTotalAllocateNowFooterSplit";
        public const string OtherAmountFooterSplit = "OtherAmountFooterSplit";
        public const string TaxFooterSplit = "TaxFooterSplit";
        public static string AllocatedDiscount = "AllocatedDiscount";

        //Purchase Invoice - GRN
        public const string POTotalGRNQty = "POTotalGRNQty";
        public const string POTotalGRNInvdQty = "POTotalGRNInvdQty";
        public const string POTotalGRNBalance = "POTotalGRNBalance";

        //Miscellaneous Invoice - Issue Details
        public const string TotalIssueQty = "TotalIssueQty";
        public const string TotalIssueInvdQty = "TotalIssueInvdQty";
        public const string TotalIssueBalance = "TotalIssueBalance";

        //Finance - GST Return
        public static string RecentEndDate = "RecentEndDate";
        public static string IsAnyDraft = "IsAnyDraft";

        //BizUnitConfigValue
        public const string BizUnitConfigValue = "BizUnitConfigValue";

        //TaskTracker
        public const string IsSubtask = "IsSubtask";
        public const string CurrSqNo = "CurrSqNo";

        //Employee Inage
        public const string FilePath = "FilePath";
        public const string FileName = "FileName";
        public const string SelectedPkArray = "SelectedPkArray";
        public const string SearchMode = "SearchMode";
        public const string DocumentAction = "DocumentAction";
        public const string Datasource = "Datasource";

        //Agent Comm
        public static string SelectedCurrencyForInvComm = "SelectedCurrencyForInvComm";
        public static string SelectedCustomersForInvComm = "SelectedCustomersForInvComm";
        public static string SelectedSosCountForInvComm = "SelectedSosCountForInvComm";
        public static string CurrencyForInvComm = "CurrencyForInvComm";
        public static string CustomerIDForInvComm = "CustomerIDForInvComm";
        public static string SoIdForInvComm = "SoIdForInvComm";
        public static string TypeIDForInvComm = "TypeIDForInvComm";
        public static string InvNoForAgentCommn = "InvNoForAgentCommn";
        public static string SelectedTypeForInvComm = "SelectedTypeForInvComm";
        public static string CurrencyFormatString = "CurrencyFormatString";
        public static string PageProcessID = "PageProcessID";
        public static string ConnectionStringBuilder = "ConnectionStringBuilder";
        public static string BackUpToDirectory = "BackUpToDirectory";
        public static string BackUpDevice = "BackUpDevice";
        public static string BackUpPath = "BackUpPath";
        public static string ResoteFrom = "ResoteFrom";

        //BadDebits
        public const string BDApplicationType = "BDApplicationType";
        public static string drTableIndex = "drTableIndex";
        public static string crTableIndex = "crTableIndex";
        public static string BankPk = "BankPk";

        //Loans & Advances
        public const string empLoanDetailList = "empLoanDetailList";
        public const string TotalPrincipalAmt = "TotalPrincipalAmt";
        public const string InterestAmount = "InterestAmount";

        //Employee Salary Details
        public const string EmployeePK = "EmployeePK";
        public const string empSalaryDetailList = "empSalaryDetailList";
        public const string EmpSalaryHdr = "EmpSalaryHdr";
        public const string SelectedDeductPK = "SelectedDeductPK";
        public const string SelectedEarnPK = "SelectedEarnPK";
        public const string IsApplyEarnFormula = "IsApplyEarnFormula";
        public const string EmpSalaryAction = "EmpSalaryAction";
        public const string EarningData = "EarningData";
        public const string DeductionData = "DeductionData";
        public static string ShowCommandBar = "ShowCommandBar";
        public static string DisplayMode = "DisplayMode";
        public static string TempPk = "TempPk";
        public const string UserActionLog = "UserActionLog";
        public const string HasMappingPermission = "HasMappingPermission";
        public const string SendMailWithAttachment = "SendMailWithAttachment";
        public const string EffectTo = "EffectTo";
        public const string TemplateDetailPk = "TemplateDetailPk";
        public const string IsEmpAppraisal = "IsEmpAppraisal";
        public const string MonthlyStartDay = "MonthlyStartDay";

        // Payroll Process
        public const string CurrEmployeePayrollPK = "CurrEmployeePayrollPK";
        public const string empPayrollDetailList = "empPayrollDetailList";
        public const string EmpPaySlipHeaderSession = "EmpPaySlipHeaderSession";
        public const string EmployeePayrollPK = "EmployeePayrollPK";
        public const string empPayDtlList = "empPayDtlList";
        public const string workdayDetailList = "workdayDetailList";
        public const string prvRow = "prvRow";
        public const string curRow = "curRow";
        public const string ShowSalPartPayElement = "ShowSalPartPayElement";
        public const string PreprocessDisabled = "PreprocessDisabled";
        public const string PayrollPreprocessFilter = "PayrollPreprocessFilter";
        public const string NegativeSalExist = "NegativeSalExist";

        //Other Addition / Deduction
        public const string OtherDetailList = "OtherDetailList";
        public const string CurrEmpAddDedPK = "CurrEmpAddDedPK";
        public const string ApplicationCode = "ApplicationCode";
        public const string PackingSpecPk = "PackingSpecPk";
        public const string ShowAdjColumn = "ShowAdjColumn";
        public const string DisplayTab = "DisplayTab";
        public const string ProductGrade = "ProductGrade";
        public const string SubType = "SubType";
        public const string RelatedItems = "RelatedItems";
        public const string SubTypeMapList = "SubTypeMapList";
        public const string PackMatMapList = "PackMatMapList";
        public const string RelProductPk = "RelProductPk";
        public const string SubTypeProductPk = "SubTypeProductPk";
        public const string PackMatProductPk = "PackMatProductPk";
        public const string VenMaterialList = "VenMaterialList";
        public const string SelectedIssueTypePk = "SelectedIssueTypePk";
        public const string IsHdrFormula = "IsHdrFormula";
        public const string SlabDefinitionViewState = "SlabDefinitionViewState";
        public const string SlabDefinitionDetailList = "SlabDefinitionDetailList";
        public const string TemplateType = "TemplateType";
        public const string SalaryProcessed = "SalaryProcessed";
        public const string ShowExpenseCrDr = "ShowExpenseCrDr";
        public const string CurrHolidayDtPK = "CurrHolidayDtPK";
        public const string HolidayDetails = "HolidayDetails";
        public const string VersionPK = "VersionPK";

        //Dashboard Setup
        public const string CurrDashGroupPK = "CurrDashGroupPK";
        public const string ShowIncomeTax = "ShowIncomeTax";
        public const string LeaveDetailsList = "LeaveDetailsList";

        //Extra Days
        public const string ExtraDaysDetailsList = "ExtraDaysDetailsList";

        //Employee Basic Info
        public const string ShowESIRequired = "ShowESIRequired";
        public const string ShowPFRequired = "ShowPFRequired";
        public const string SalaryPaymentModeDetailList = "SalaryPaymentModeDetailList";
        public const string EmpSalaryDetailsPopupList = "EmpSalaryDetailsPopupList";
        public const string PopupViewMode = "PopupViewMode";
        public const string RowIndexPopup = "RowIndexPopup";
        public const string TotalEmployees = "TotalEmployees";
        public const string EmployeeLimits = "EmployeeLimits";
        public const string EmpResignedDate = "EmpResignedDate";
        public const string ShowAdditionalInfo = "ShowAdditionalInfo";
        public const string ShowCostCenterTeamInfo = "ShowCostCenterTeamInfo";
        public const string EmpPayDetailsSaveCheck = "EmpPayDetailsSaveCheck";


        //BONUS ENTRY
        public const string BonusEntryDetailList = "BonusEntryDetailList";

        //Employee Appraisal Details
        public const string EMPAppraisaDetailsList = "EMPAppraisaDetailsList";

        public const string tempDirectsaleOrderTaxHdrList = "tempDirectsaleOrderTaxHdrList";

        public const string MultiCurrencyEnabled = "MultiCurrencyEnabled";
        public const string JournalStatus = "JournalStatus";

        //User DashBoard
        public const string UsrControlType = "UsrControlType";

        // Send SMS
        public const string SendSMSDetails = "SendSMSDetails";

        // Leave Entry
        public const string CreditLeave = "CreditLeave";
        public const string LeaveDtPk = "LeaveDtPk";


        public const string AssetServiceOrderReceiptHeader = "AssetServiceOrderReceiptHeader";

        public const string StockInitPk = "StockInitPk";
        public const string SlNo = "SlNo";
        public const string CurrCmntPK = "CurrCmntPK";
        public const string CmntLastModifiedTime = "CmntLastModifiedTime";
        public const string TransactionPk = "TransactionPk";
        public const string ApplicationType = "ApplicationType";
        public const string ApplicationName = "ApplicationName";
        public const string TransactionNo = "TransactionNo";
        public const string EmpTransferDtls = "EmpTransferDtls";
        public const string PageIndexList = "PageIndexList";
        public const string ParentPage = "ParentPage";
        public const string EmpDocDetails = "EmpDocDetails";
        public const string DefaultDocType = "DefaultDocType";

        //Salary Report Template
        public const string GroupSequence = "GroupSequence";
        public const string GroupItemSequence = "GroupItemSequence";
        public const string RowEditMode = "RowEditMode";
        public const string TimeMaskValidationExp = "TimeMaskValidationExp";
        public const string TimeMask = "TimeMask";
        public const string AttendanceEntryGridViewList = "AttendanceEntryGridViewList";
        public const string AttendancePopupDetailsViewState = "AttendancePopupDetailsViewState";

        public const string SalaryBulkEmpDetail = "SalaryBulkEmpDetails";
        public const string SalaryBulkPayAppraisalDetails = "SalaryBulkPayAppraisalDetails";

        // Push Notification
        public const string PushNotificationDetails = "PushNotificationDetails";

        //Direct Sales Invoice
        public static string DirectTempSOInvoiceDtlSession = "DirectTempSOInvoiceDtlSession";
        public static string IsHeaderDiscountForTradingSale = "IsHeaderDiscountForTradingSale";
        public static string IsHeaderTaxForTradingSale = "IsHeaderTaxForTradingSale";
        public static string CustomerCategory = "CustomerCategory";

        public static string IsItemwiseDiscountForTradingSale = "IsItemwiseDiscountForTradingSale";
        public static string IsItemwiseTaxForTradingSale = "IsItemwiseTaxForTradingSale";
        public static string ControlId = "ControlId";
        public static string CostCenterList = "CostCenterList";
        public static string VoucherDetails = "VoucherDetails";
        public static string AccountPk = "AccountPk";
        public static string CCInvalidSplitControlList = "CCInvalidSplitControlList";

        //ExpenseInvoice Trading
        public static string IsHeaderDiscountForTradingPurchase = "IsHeaderDiscountForTradingPurchase";
        public static string IsHeaderTaxForTradingPurchase = "IsHeaderTaxForTradingPurchase";
        public static string IsItemwiseDiscountForTradingPurchase = "IsItemwiseDiscountForTradingPurchase";
        public static string IsItemwiseTaxForTradingPurchase = "IsItemwiseTaxForTradingPurchase";
        public static string Selected_InvoiceType = "Selected_InvoiceType";
        public static string Selected_Currency = "Selected_Currency";
        public static string Selected_Customers = "Selected_Customers";
        public static string InvoiceCategory = "InvoiceCategory";

        public static string DashboarHdr = "DashboarHdr";
        public static string Payments = "Payments";
        public static string VoucherVersion = "VoucherVersion";
        public static string ShowVersions = "ShowVersions";

        public static string EditDirectTempSOInvoiceHeaderSession = "EditDirectTempSOInvoiceHeaderSession";
        public static string UploadPath = "UploadPath";
        public static string HeaderTax = "HeaderTax";
        public static string HeaderTaxTemp = "HeaderTaxTemp";
        public static string CustomerType = "CustomerType";
        public static string SaleOrderItemRef = "SaleOrderItemRef";
        public static string IsAmend = "IsAmend";
        public static string FundRequisitionDeptDetailList = "FundRequisitionDeptDetailList";
        public static string FundRequisitionDeptUploadList = "FundRequisitionDeptUploadList";

        //WorkOrder
        public const string WorkOrderHeaderSession = "WorkOrderHeaderSession";
        public const string WOUploadList = "WOUploadList";
        public const string WorkOrderDetailList = "WorkOrderDetailList";

        //project
        public const string SaveAndContinue = "SaveAndContinue";
        public const string TypeCurrPK = "TypeCurrPK";
    }

    /// <summary>
    /// QueryStrings
    /// </summary>
    public class QueryStrings
    {
        #region QueryString Constants
        //For workflow        
        public const string PRefID = "PRefID";
        public const string RefID = "RefID";
        public const string FromExt = "FromExt";
        public const string PID = "PID";
        public const string CurDept = "CurDept";
        public const string ProcessDept = "ProcessDept";
        public const string GRefPK = "GRefPK";
        public const string PK = "PK";
        public const string GRefType = "GRefType";

        public const string Flag = "Flag";
        //For Dynamic Report 
        public const string RptID = "RptID";
        public const string GroupCount = "GroupCount";

        //For Journal
        public const string Type = "Type";
        public const string SType = "SType";

        public const string fromPK = "fromPK";

        public const string DONO = "DONO";
        public const string PageType = "TYPE";
        public const string ReportType = "REPORTTYPE";
        public const string ProjID = "ProjID";
        // For Page Navigation
        public const string JrnlPK = "JrnlPK";
        public const string JrnlType = "JrnlType";

        //For TrialBalance
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        //project
        public const string FromPage = "FromPage";
        #endregion

        public const string TabID = "TabID";
        public const string CustPk = "CustPk";
        public const string BrankPk = "BrankPk";
        public const string EMP_PK = "EMP_PK";

        public const string newsRequested = "NEWSPK";
        public const string RPT_PK = "REPORT";
    }

    /// <summary>
    /// Session Strings
    /// </summary>
    public class SessionStrings
    {
        //Common Sessions
        public static string SessionConfigData = "SessionConfigData";
        public static string RateDecimalDigit = "RateDecimalDigit";
        public static string WeightDecimalDigit = "WeightDecimalDigit";
        public static string RateDecimalDigitP2P = "RateDecimalDigitP2P";
        public static string ExchRateDecimalDigit = "ExchRateDecimalDigit";
        public static string NumberDecimalDigitsP2P = "NumberDecimalDigitsP2P";
        public static string NumberDecimalDigitsCompounding = "NumberDecimalDigitsCompounding";
        public static string MiscRateDecimalDigit = "MiscRateDecimalDigit";
        public static string NumberDecimalDigitBin = "NumberDecimalDigitBin";
        public static string POInvoiceType = "POInvoiceType";

        //Alert
        public static string PoAlert = "POAlert";
        //Bill of Loading
        public const string FileBLDetailsList = "FileBLDetailsList";
        public const string FilePODetailsList = "FilePODetailsList";
        //Sale Order
        public const string FilterCriteria = "FilterCriteria";
        public const string SessionPK = "SessionPK";
        public const string SelectedOrderItem = "SelectedOrderItem";
        public const string SelectedOrderInfo = "SelectedOrderInfo";
        public const string SelectedOrders = "SelectedOrders";
        public const string PurInvoices = "PurInvoices";

        public const string CustomerPK = "CustomerPK";
        public const string CurrentPK = "CurrentPK";
        public const string CurrentViewModePK = "CurrentViewModePK";
        public const string FileSODetailsList = "FileSODetailsList";
        //
        //Customer Order
        public const string CustomerOrderDetails = "CustomerOrderDetails";
        //
        public const string LicenceMessage = "LicenceMessage";
        public const string Vendor = "Vendor";
        public const string ActivityHeader = "ActivityHeader";
        public const string ContractsHeader = "ContractsHeader";
        public const string VendorMstMode = "VendorMstMode";
        public static string ContractPK = "ContractPK";
        public static string AttachmentSaveCompleted = "AttachmentSaveCompleted";
        public static string AttachmentList = "AttachmentList";
        public static string ModuleRef = "ModuleRef";
        public static string AttachmentViewMode = "AttachmentViewMode";
        public static string ViewMode = "ViewMode";
        public static string ActivityPK = "ActivityPK";
        public static string Activity = "Activity";
        public static string ActivityHdrMode = "ActivityHdrMode";
        public static string SGHA_HDR = "SGHA_HDR";
        public static string SGHA_DTL = "SGHA_DTL";
        public static string CurrGridView = "CurrGridView";
        public static string WkfUserMst = "WkfUserMst";
        public static string VendorPK = "VendorPK";
        public static string RefID = "RefID";
        public static string PRefID = "PRefID";
        public static string Type = "Type";
        public static string SubType = "SubType";
        public static string InboxFlag = "InboxFlag";
        public static string ITM_MST = "ITM_MST";
        public static string TYPES_MST = "TYPES_MST";
        public static string Routs = "Routs";
        public static string Legs = "Legs";
        public static string Flights = "Flights";
        public static string Region = "Region";
        public static string Country = "Country";
        public static string Airport = "Airport";
        public static string AircraftType = "AircraftType";
        public static string Vendors = "Vendors";
        public static string Parents = "Parents";
        public static string Elements = "Elements";
        public static string filterFieldsList = "filterFieldsList";
        public static string groupFilterFields = "groupFilterFields";
        public static string fieldFilterFields = "fieldFilterFields";
        public static string RptID = "RptID";
        public static string reportData = "reportData";
        public static string CurrencyCode = "CurrencyCode";
        public static string Periode = "Periode";

        public const string InvoiceMultiplePOPKs = "InvoiceMultiplePOPKs";
        public const string InvoiceMultiplePOPKsNew = "InvoiceMultiplePOPKsNew";
        public const string SelectedPOInfoLst = "SelectedPOInfoLst";
        public const string SelectedPos = "SelectedPos";
        public const string SelectedSos = "SelectedSos";
        public const string SelectedSosForAdvInv = "SelectedSosForAdvInv";
        public const string SelectedSosForDO = "SelectedSosForDO";
        public const string SelectedSaleOrders = "SelectedSaleOrders";
        public const string SelectedInvoices = "SelectedInvoices";
        public const string SelectedInvoicesInfoLst = "SelectedInvoicesInfoLst";
        public const string SelectedInvoicesCrDr = "SelectedInvoicesCrDr";
        public const string RemovedInvoicesCrDr = "RemovedInvoicesCrDr";
        public const string CrDrType = "CrDrType";
        public const string PurOrderHeaderList = "PurOrderHeaderList";
        public const string PurOrderHeaderMappingList = "PurOrderHeaderMappingList";
        public const string PoHeaderList = "PoHeaderList";
        public const string SalHeaderList = "SalHeaderList";
        public const string ProcessidDummy = "ProcessidDummy";
        public const string InvoiceMapList = "InvoiceMapList";
        public const string InvoiceCusMapList = "InvoiceCusMapList";
        public static string SelectedSalesInvoices = "SelectedSalesInvoices";
        public static string SelectedSalesInvoicesInfoLst = "SelectedSalesInvoicesInfoLst";
        public static string EditedInvoices = "EditedInvoices";
        public static string FinInvoiceVndHdrSelectedList = "FinInvoiceVndHdrSelectedList";
        public static string FinInvoiceCusHdrSelectedList = "FinInvoiceCusHdrSelectedList";
        public static string FinInvoiceCrDrSelectedList = "FinInvoiceCrDrSelectedList";
        public static string EditedPaymentDtls = "EditedPaymentDtls";
        public static string EditedSalesInvoices = "EditedSalesInvoices";
        public static string EditedReceiptDtls = "EditedReceiptDtls";
        public static string FlightDelayPK = "FlightDelayPK";
        public static string finInvoiceHdrList = "finInvoiceHdrList";
        public static string finPaymentTrxMpgList = "finPaymentTrxMpgList";
        public const string POList = "POList";

        public static string ContractSlabBasisMpgList = "ContractSlabBasisMpgList";

        public static string Bank = "Bank";
        public static string SELF_INVOICE_HDR = "SELF_INVOICE_HDR";
        public static string SPCAM_SELF_INVOICE_DTL_GET_LIST_Result = "SPCAM_SELF_INVOICE_DTL_GET_LIST_Result";
        public static string SERVICETAB_SELECTED_VALUE = "SERVICETAB_SELECTED_VALUE";
        public static string SERVICETAB_ACTIVETAB = "SERVICETAB_ACTIVETAB";
        public static string DYNAMICTAB_SELECTEDTAB_NAME = "DYNAMICTAB_SELECTEDTAB_NAME";
        public static string SERVICETAB_SELECTED_PK = "SERVICETAB_SELECTED_PK";
        public static string JOURNALIZETAB_SELECTED_PK = "JOURNALIZETAB_SELECTED_PK";
        public static string FINHEADERLIST = "FINHEADERLIST";
        public static string RemoveRowIndex = "RemoveRowIndex";
        public static string RemoveHdfIdDr = "RemoveHdfIdDr";
        public static string RemoveHdfIdCr = "RemoveHdfIdCr";
        public static string TransactionType = "TransactionType";
        public static string TrxPK = "TrxPK";
        public static string JournalHead = "JournalHead";
        public static string AccountPayable = "AccountPayable";
        public static string AccountPayablePK = "AccountPayablePK";
        public static string JournalMode = "JournalMode";
        public static string JournalType = "JournalType";
        public static string TransactionCurrency = "TransactionCurrency";
        public static string TransactionNo = "TransactionNo";
        public static string TransactionDate = "TransactionDate";
        public static string TransactionPK = "TransactionPK";
        public static string JournalizePK = "JournalizePK";
        public static string Transaction = "Transaction";
        public static string JOURNALIZETAB_SELECTED_QUERY = "JOURNALIZETAB_SELECTED_QUERY";
        public static string SubTypeCr = "SubTypeCr";
        public static string SubTypeDr = "SubTypeDr";
        public static string SubTypeCCr = "SubTypeCCr";
        public static string SubTypeDDr = "SubTypeDDr";
        public static string FTRPK = "FTRPK";
        public static string Employee = "Employee";
        public static string PaymentFlag = "PaymentFlag";
        public static string CrDrFlag = "CrDrFlag";

        //Agent Comm
        public static string SelectedSosForInvComm = "SelectedSosForInvComm";
        public static string SelectedSOListForInvComm = "SelectedSOListForInvComm";

        //For RFQ
        public static string PRSearchResult = "PRSearchResult";
        public static string PRSelected = "PRSelected";
        public static string RFQParameters = "RFQParameters";
        public static string RFQSelectedDetails = "RFQSelectedDetails";
        public static string RFQPK = "RFQPK";
        public static string RFQMODE = "RFQMODE";
        public static string MAILSTATUS = "MAILSTATUS";
        public static string RFQVendor = "RFQVendor";
        public static string RFQTaxDtlSplitSession = "RFQTaxDtlSplitSession";
        public static string RFQTaxHdrSplitSession = "RFQTaxHdrSplitSession";
        public static string RFQResponseHeader = "RFQResponseHeader";
        public static string TempRFQResponseHeader = "TempRFQResponseHeader";
        public static string RFQItemDetailParameters = "RFQItemDetailParameters";
        public static string CUSTOMERPK = "CUSTOMERPK";
        public static string CUSTOMERNAME = "CUSTOMERNAME";
        public static string DYNAMICTAB_SELECTED_CODE = "DYNAMICTAB_SELECTED_CODE";
        public static string ENQUIRYPK = "ENQUIRYPK";
        public static string QUOTATIONPK = "QUOTATIONPK";
        public static string SALEORDERPK = "SALEORDERPK";
        public static string SALEORDERDOPK = "SALEORDERDOPK";
        public static string SALEORDERCOPYPK = "SALEORDERCOPYPK";
        public static string SaleOrderMode = "SaleOrderMode";
        public static string QuotationHeader = "QuotationHeader";
        public static string SaleOrderHeaderSession = "SaleOrderHeaderSession";
        public static string TempSaleOrderHeaderSession = "TempSaleOrderHeaderSession";
        public static string DirectTempSaleOrderHeaderSession = "DirectTempSaleOrderHeaderSession";
        public static string EditDirectTempSaleOrderHeaderSession = "EditDirectTempSaleOrderHeaderSession";
        public static string SaleDespatchDtlList = "SaleDespatchDtlList";
        public static string SalDetailList = "SalDetailList";
        public static string REPORTPK = "REPORTPK";
        public static string CRReportData = "CRReportData";
        public static string CRReportParam = "CRReportParam";
        public static string CRReportDataFromExt = "CRReportDataFromExt";
        public static string CRReportParamFromExt = "CRReportParamFromExt";

        public const string DONO = "DONO";

        //FOR ContainerInspection
        public static string ContainerInspectionPK = "ContainerInspectionPK";
        public static string ContainerInspectionMode = "ContainerInspectionMode";

        //For Container Evaluation
        public static string ContainerEvaluationPK = "ContainerEvaluationPK";
        public static string ContainerEvaluationMode = "ContainerEvaluationMode";

        //For Sales Invoice
        public static string SOInvoiceHeaderSession = "SOInvoiceHeaderSession";
        public static string POInvoiceHeaderSession = "POInvoiceHeaderSession";
        public static string TempSOInvoiceHeaderSession = "TempSOInvoiceHeaderSession";
        public static string TempSOInvoiceDtlSession = "TempSOInvoiceDtlSession";
        public static string TempSOInvoiceHeaderSessionCustAll = "TempSOInvoiceHeaderSessionCustAll";
        public static string TempSOInvoiceHeaderTSessionCustAll = "TempSOInvoiceHeaderTSessionCustAll";
        public static string SoHeaderDOList = "SoHeaderDOList";
        public static string invoiceHeaderMulObj = "invoiceHeaderMulObj";
        public static string invoiceHdrMulDummyObj = "invoiceHdrMulDummyObj";
        public static string FileSOInvoiceDetailsList = "FileSOInvoiceDetailsList";
        public static string DirectSOInvoiceHeaderSession = "DirectSOInvoiceHeaderSession";
        public static string DirectTempSOInvoiceDtlSession = "DirectTempSOInvoiceDtlSession";
        public static string DirectTempSOInvoiceHeaderSession = "DirectTempSOInvoiceHeaderSession";
        public static string DirectTempSOInvoiceHeaderSessionCustAll = "DirectTempSOInvoiceHeaderSessionCustAll";
        public static string FileDirectSOInvoiceDetailsList = "FileDirectSOInvoiceDetailsList";
        public static string FininvoiceHeaderMulObj = "FininvoiceHeaderMulObj";

        //For Finance (Vat sales export)
        public static string VatSaleDetailsSession = "VatSaleDetailsSession";
        public static string VatSaleSession = "VatSaleSession";
        public static string TempVatSaleSession = "TempVatSaleSession";
        public static string tempVatSaleDetailsSession = "tempVatSaleDetailsSession";
        public static string VatSaleHeaderSession = "VatSaleHeaderSession";
        public static string tempVatSaleHeaderSession = "tempVatSaleHeaderSession";

        //Finance (GST Return)
        public static string GSTReturnHeaderSession = "GSTReturnHeaderSession";


        //Dashboard
        public static string CurrNodeType = "CurrNodeType";
        public static string CurrNodePK = "CurrNodePK";
        public static string CurrNodeText = "CurrNodeText";
        public static string AssetTreeViewBy = "AssetTreeViewBy";
        public static string AssetFilterText = "AssetFilterText";
        public static string MasterNodeType = "MasterNodeType";
        public static string AssetFilterValue = "AssetFilterValue";
        public static string DashboardAssetTypeText = "DashboardAssetTypeText";
        public static string TreeViewSelectedNodeText = "TreeViewSelectedNodeText";
        public static string DashboardAssetTypePK = "DashboardAssetTypePK";
        public static string AssetTreeFilterBy = "AssetTreeFilterBy";

        public static string PURCHASEORDERPK = "PURCHASEORDERPK";
        public static string CUSTOMERLOGINPK = "CUSTOMERLOGINPK";
        public static string Dashboard = "Dashboard";

        //ShiftReport
        public const string ShiftDetails = "ShiftDtl";
        public const string AllocatedOrderInfo = "AllocatedOrderInfo";

        public const string DespatchDetails = "DespatchDetails";
        public const string PackingDetails = "PackingDetails";
        public const string AccountType = "AccountType";
        public static string ControlInfo = "ControlInfo";
        public static string DrControls = "DrControls";
        public static string RemovedControls = "RemovedControls";
        public static string CrControls = "CrControls";
        public static string FinTrxPk = "FinTrxPk";

        //ShippingPlan
        public static string SelectedSosForSP = "SelectedSosForSP";
        public static string SHIPPINGPLANPK = "SHIPPINGPLANPK";
        public static string SHIPPINGUPLOADTYPE = "SHIPPINGUPLOADTYPE";
        public static string CONTAINERTYPE = "CONTAINERTYPE";
        public static string GONPK = "GONPK";
        public static string PID = "PID";
        public static string JournalRefId = "JournalRefId";

        public static string PackingPk = "PackingPk";
        public static string PackingMode = "PackingMode";

        //For Expenses
        public const string SelectedExpenses = "SelectedExpenses";
        public const string ExpenseHeaderSession = "ExpenseHeaderSession";
        public const string TempExpenseHeaderSession = "TempExpenseHeaderSession";
        public const string jvTemplateBOHeaderObj = "jvTemplateBOHeaderObj";
        public const string ExpensePayment = "ExpensePayment";
        public const string PaymentCategory = "PaymentCategory";

        //Payment
        public const string InvoicePOSplitList = "InvoicePOSplitList";
        public static string InvoiceSOSplitList = "InvoiceSOSplitList";
        public static string PaymentAdjnList = "PaymentAdjnList";
        public static string WHTTaxDetails = "WHTTaxDetails";
        public static string TempWHTTaxDetails = "TempWHTTaxDetails";
        public static string VATTaxDetails = "VATTaxDetails";
        public static string TempVATTaxDetails = "TempVATTaxDetails";
        public static string PaymentModeDetailsList = "PaymentModeDetailsList";
        public static string PaymentModeConfigMstList = "PaymentModeConfigMstList";


        public const string CrDrSplitList = "CrDrSplitList";

        // adm config
        public const string TempConfigMstDetails = "TempConfigMstDetails";
        //public static int CrDrSplitList { get; set; }

        //CrDrNoteCusttomer
        public const string FinCrDrNoteTaxHeader = "FinCrDrNoteTaxHeader";
        public const string FinCrDrNoteTaxHeaderTemp = "FinCrDrNoteTaxHeaderTemp";

        //Customer Registration Page
        public const string CrmCustTaxDetails = "CrmCustTaxDetails";
        public const string CrmCustTaxDetailsTemp = "CrmCustTaxDetailsTemp";

        //For Task Tracker
        public const string TaskInfoSession = "TaskInfoSession";
        public const string CurrSqNo = "CurrSqNo";

        public const string ActiveEmployeeTab = "ActiveEmployeeTab";

        //Agt comm
        public static string AgtCommHeaderSession = "AgtCommHeaderSession";

        //Finance -  Depreciation
        public static string SelectedDepreciation = "SelectedDepreciation";

        //eDoc
        public const string EDocPk = "EDocPk";
        //DirectStockAdmission
        public const string FileDSADetailsList = "FileDSADetailsList";

        public static string InvoicePK = "InvoicePK";

        public static string VoucherPk = "VoucherPk";

        public static string TransactionCancel = "TransactionCancel";
        public const string TempPk = "TempPk";

        public const string AutoCompleteRelatedControlValue = "AutoCompleteRelatedControlValue";
        public const string IsDashBoardUser = "IsDashBoardUser";

        public const string SelectedSosForAdvInvoice = "SelectedSosForAdvInvoice";
        public const string IssueListApplied = "IssueListApplied";

        //HRMS
        //Payroll
        public const string CurrEmployeePayrollPK = "CurrEmployeePayrollPK";
        public const string AssetServiceRequestHeaderSession = "AssetServiceRequestHeaderSession";
        public const string AttnDetailsList = "AttnDetailsList";
        public const string ReceiptHeaderSession = "ReceiptHeaderSession";
        public const string DebitCreditHeaderSession = "DebitCreditHeaderSession";
        public const string FileDetailsList = "FileDetailsList";
        public const string CostCenterParamsSession = "CostCenterParamsSession";

        //WorkOrder
        public static string TempWorkOrderHeaderSession = "TempWorkOrderHeaderSession";
        public const string FileWODetailsList = "FileWODetailsList";
        public const string WOIssuesXml = "WOIssuesXml";

        //project

        public const string IsWOAmend = "IsWOAmend";



    }
}
