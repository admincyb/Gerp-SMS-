using System.Collections.Generic;
using System.Runtime.Serialization;
using System;


namespace ERPManager
{
    public class ServiceUtility
    {
        /// <summary>
        /// Current Listing Page Index
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Listing Page Size
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Sort Listing By
        /// </summary>
        public string SortBy { get; set; }
        /// <summary>
        /// Then Listing By
        /// </summary>
        public string ThenBy { get; set; }
        /// <summary>
        /// Listing Sort Direction
        /// <value>asc</value>
        /// <value>desc</value>
        /// </summary>
        public string SortDirection { get; set; }
        /// <summary>
        /// Count of Total Records after filter
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Field to filter
        /// </summary>
        public string FilterBy { get; set; }
        /// <summary>
        /// Filter value
        /// </summary>
        public string FilterValue { get; set; }
        /// <summary>
        /// Field to Search Field
        /// </summary>
        public string SearchBy { get; set; }
        /// <summary>
        /// Filter location
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Field to Search VoucherNo
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Field to search Invoice Type
        /// </summary>
        public int InvoiceType { get; set; }
        /// <summary>
        /// Field to filter Date
        /// </summary>
        public DateTime? FilterDate { get; set; }
        /// <summary>
        /// Field to filter To Date
        /// </summary>
        public DateTime? FilterToDate { get; set; }
        /// <summary>
        /// Subproperties of ServiceUtility
        /// </summary>
        public List<ServiceUtility> EntityProperties { get; set; }
        /// <summary>
        /// Entities to Include
        /// </summary>
        public List<LoadEntities> LoadEntities { get; set; }

        //Documentation
        public LoadDMSEntities LoadDMSEntities { get; set; }
        /// <summary>
        /// Used for enabling advance filter in specific cases
        /// </summary>
        public bool NeedAdvanceFilter { get; set; }
        /// <summary>
        /// Field to filter with user mapping
        /// </summary>
        public int User { get; set; }

        public int ?BizUnit { get; set; }
        /// <summary>
        /// Any filterations, specifically against SBU
        /// </summary>
        public bool IsSBUSpecific { get; set; }
    }

    [DataContract]
    public enum LoadDMSEntities
    {
        List,
        Detail
    }
    /// <summary>
    /// Entity Incude Enumeration
    /// </summary>
    [DataContract]
    public enum LoadEntities
    {
        [EnumMember]
        CountriesMst,
        [EnumMember]
        CurrenciesMst,
        [EnumMember]
        CurrenciesMst1,
        [EnumMember]
        AirportMst,
        [EnumMember]
        VendorMst,
        [EnumMember]
        GuaranteeTrx,
        [EnumMember]
        BankMst,
        [EnumMember]
        ChargeBasis,
        [EnumMember]
        ChargeBasisType,
        [EnumMember]
        ElementMst,
        [EnumMember]
        ElementMst2,
        [EnumMember]
        ConstantsTable,
        [EnumMember]
        ContractBasisDetails,
        [EnumMember]
        ContractDetails,
        [EnumMember]
        ContractElement,
        [EnumMember]
        ContractSlabDetails,
        [EnumMember]
        ContractsHeader,
        [EnumMember]
        ContractHeaderAirport,
        [EnumMember]
        ContractHeaderVendor,
        [EnumMember]
        TaxesConstCfg,
        [EnumMember]
        TaxesMst,
        [EnumMember]
        TaxGroupMst,
        [EnumMember]
        UOMMst,
        [EnumMember]
        ConstantsTable1,
        [EnumMember]
        ControlsUI,
        [EnumMember]
        ControlsConfig,
        [EnumMember]
        SlabBasis,
        [EnumMember]
        SlabBasisControlsConfig,
        [EnumMember]
        SlabBasisControlsUI,
        [EnumMember]
        SlabBasisControlsChargeBasis,
        [EnumMember]
        AirportMst1,
        [EnumMember]
        AirportMst2,
        [EnumMember]
        TaxGroupMap,
        [EnumMember]
        TaxGroupAirportMap,
        [EnumMember]
        QueriesCfg,
        [EnumMember]
        FlightDelayCodeMst,
        [EnumMember]
        FuelRegionMst,
        [EnumMember]
        AirportMst3,
        [EnumMember]
        ROUTES_MST,
        [EnumMember]
        SECTORS_MST,
        [EnumMember]
        AD_FLIGHTS_MST,
        [EnumMember]
        AirportMst_3,
        [EnumMember]
        ShiftMst

    }

    /// <summary>
    /// ApplicationSubType enum for generating Trx No.
    /// </summary>
    [DataContract]
    public enum ApplicationSubType
    {
        [EnumMember]
        INVOICE = 11,
        [EnumMember]
        SALEADVINVOICEEXPORT = 12,
        [EnumMember]
        SALEADVINVOICEPROFORMA = 13,
        [EnumMember]
        PAYMENT = 3,
        [EnumMember]
        BANKGUARANTEE,
        [EnumMember]
        SELFINVOICE = 4,
        [EnumMember]
        RECEIPT = 5,
        [EnumMember]
        CREDITNOTE = 6,
        [EnumMember]
        DEBITNOTE = 7,
        [EnumMember]
        JOURNALIZE = 7,
        [EnumMember]
        DELIVERYORDER = 8,
        [EnumMember]
        OFFICIALRECEIPT = 1,
        [EnumMember]
        ADVANCEINVOICE = 1,
    }
    /// <summary>
    /// ApplicationSubType enum for generating Trx No.
    /// </summary>
    [DataContract]
    public enum PageType
    {
        [EnumMember]
        CUSTOMER = 1,
        [EnumMember]
        SALE = 2,
        [EnumMember]
        CUSTOMERUSER = 3,
        [EnumMember]
        SHIPPING = 4,
        INVOICE = 5
    }
    /// <summary>
    /// DropDownList Item DataType
    /// </summary>
    [Serializable]
    [DataContract]
    public class DDLMaster
    {
        public int? PK { get; set; }
        public string Value { get; set; }

    }
    /// <summary>
    /// CheckListMaster Item DataType
    /// </summary>
    [DataContract]
    public class CheckListData
    {
        public int? PK { get; set; }
        public string Value { get; set; }

    }

    [DataContract]
    public class TreeBinder
    {
        public int? PK { get; set; }
        public string Value { get; set; }
        public int? Parent { get; set; }

    }

    [DataContract]
    public class TextMaster
    {
        public int? PK { get; set; }
        public decimal Value { get; set; }

    }
    [DataContract]
    public enum VendorRoles
    {
        OEMManufacturer = 1,
        Dealer,
        ServiceProvider,
        Financier,
        Lessor,
        Lessee,
        SubContractor,
        ShippinngCompany,
        CartonManufacture,
        Transporter,
        BoxManufacture
    }
}
