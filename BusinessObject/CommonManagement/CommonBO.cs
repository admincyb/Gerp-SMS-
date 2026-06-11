using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.CommonManagement
{
    [Serializable]
    public class PageBO
    {
        public int PagePK
        { get; set; }
        public string PageName
        { get; set; }
        public List<SectionBO> Sections
        { get; set; }

        public PageBO()
        {
            Sections = new List<SectionBO>();
        }
    }
    #region Workflow Sequence Configuration Save
    [Serializable]
    [XmlRoot("Root")]
    public class WKFSequenceConfig
    {
        [XmlElement("ProcessPK")]
        public int ProcessPK { get; set; }
        [XmlElement("UserPK")]
        public int UserPK { get; set; }
        [XmlElement("DeptPK")]
        public int DeptPK { get; set; }
        [XmlElement("LastModDate")]
        public DateTime LastModDate { get; set; }
        [XmlElement("TaskDetails")]
        public List<TaskDetails> lstTaskDetails;
    }
    [Serializable]
    public class TaskDetails
    {
        [XmlElement("tskPK")]
        public int tskPK { get; set; }
        [XmlElement("tskName")]
        public string tskName { get; set; }
        [XmlElement("tskOrderSeq")]
        public string tskOrderSeq { get; set; }
        [XmlElement("SeqDetail")]
        public List<SeqDetail> lstSeqDetail;
    }
    [Serializable]
    public class SeqDetail
    {
        [XmlElement("wsqPK")]
        public int wsqPK { get; set; }
        [XmlElement("wsqTaskActionSeq")]
        public int wsqTaskActionSeq { get; set; }
        [XmlElement("wsqTaskAction")]
        public int wsqTaskAction { get; set; }
        [XmlElement("wsqTaskActionText")]
        public string wsqTaskActionText { get; set; }
        [XmlElement("wsqNextActionFlag")]
        public int wsqNextActionFlag { get; set; }
        [XmlElement("wsqType")]
        public int wsqType { get; set; }
        [XmlElement("SeqActionDetails")]
        public List<SequenceTaskActionList> lstSeqActionDetails;
    }


    [Serializable]
    [XmlRoot("Root")]
    public class SequenceTaskActionBO
    {
        [XmlElement("SeqActionDetails")]
        public List<SequenceTaskActionList> SeqActionDetailsList { get; set; }
    }
    [Serializable]
    public class SequenceTaskActionList
    {
        public int TaskPK { get; set; }
        public int TaskActionPK { get; set; }

        [XmlElement("sqaType")]
        public int sqaType { get; set; }
        [XmlElement("sqaSequence")]
        public int sqaSequence { get; set; }
        [XmlElement("sqaTaskAction")]
        public int sqaTaskAction { get; set; }
        [XmlElement("sqaTaskActionText")]
        public string sqaTaskActionText { get; set; }
        [XmlElement("sqaIsMapped")]
        public int sqaIsMapped { get; set; }
        [XmlElement("sqaIsDefault")]
        public int sqaIsDefault { get; set; }
    }
    #endregion
    [Serializable]
    [XmlRoot("ROOT")]
    public class PlantBO
    {
        [XmlElement("PLT_PK")]
        public int PLT_PK { get; set; }
        [XmlElement("ACTIVE")]
        public int ACTIVE { get; set; }
        [XmlElement("BIZUNIT")]
        public int BIZUNIT { get; set; }
    }
    [Serializable]
    [XmlRoot("ROOT")]
    public class SelectedItems
    {
        [XmlElement("PO")]
        public List<POSelect> lstPO { get; set; }
    }
    public class POSelect
    {
        [XmlElement("POH_PK")]
        public long PoPK { get; set; }
    }


    [Serializable]
    public class AlertSettingBO
    {
        public string PageURL
        { get; set; }
        public string TypeCode
        { get; set; }
        public int TypePK
        { get; set; }
        public string TypeRef
        { get; set; }
        public DateTime TrxDate
        { get; set; }
        public string TypeText
        { get; set; }
        public string TypePartyName
        { get; set; }

    }

    [Serializable]
    public class SectionBO
    {
        public string SectionName
        { get; set; }
        public List<ActionsBO> Actions
        { get; set; }
        public SectionBO()
        { Actions = new List<ActionsBO>(); }
    }

    [Serializable]
    public class ActionsBO
    {
        public string ActionName
        { get; set; }
    }

    public enum DbActiveStatus
    {
        INACTIVE = 0,
        ACTIVE = 1,
        HASPK = 2,
        ALL = 3
    }
    public enum DbRequiredStatus
    {
        Required = 1,
        NotRequired = 0
    }

    public enum DbSaveStatus
    {
        REFERRED = 0,
        OLDCODEEXIST = 0,
        SAVED = 1,
        SQLERROR = -1,
        CODEEXIST = -2,
        CONCURRENCY = -3,
        DATEOVERLAP = -4,
        GRNEXCESS = -47,
        ALREADYDELETED = -5,
        REFNOEXIST = -6,
        INCORRECT = -7,
        ALREADYCREATED = -8,
        PENDINGEXIST = -9,
        INVNOEXISTS = -23,
        AMOUNTEXCEEDS = -50,
        NEGATIVESAL = -31,
        SHIFTEXIST = -30,
        NAMEEXIST = -32,
        PRODUCTOVERLAP = -33,
        //Brand Imports
        CHECKPRINTER = -10, // To Check the printer name
        CHECKMCPRINTER = -11, // To Check the mc_printer name
        CHECKZBPRINTER = -12, // To Check the zb_printer name
        CHECKPOUCHPRINTER = -13, // To Check the pouch_printer name
        CHECKBOXPRINTER = -14, // To Check the box_printer name
        CHECKPACKINGTYPE = -15, // To Check the packing_type_name
        CHECKQCANDTOTAL = -16, // To Check equality of per piece and total value in the list
        CHECKPRODUCTEXIST = -17, // To Check the Proudct Exists on not
        CHECKCUSTOMEREXIST = -18, // To Check the Customer Exists or Not
        CHECKBRANDREPEAT = -19, // To Check if brand name is repeating
        CHECKCURRENCYMASTER = -20, // To check the currecny with customer master
        CHECKCURRENCYCODE = -21,  // To Check the currrency code
        CHECKBRANDNAMEEXIST = -22,  // To Check if brand name already exists,
        ALREADYEXIST = -23, // To Check Unique Insert based on a business rule
        AMOUNTEXCEEDED = -24, // Depreciation Amount Exceeded
        INVALIDBATCH = -41, // Selected batches are not in sequence  
        STOCKVALUECHECK = -35, // stock issue is requested stock is greater than present stock
        TOTALPCSZERO = -118, // To Check if total pcs are zero
        PACKCODEZERO = -119, // To Check if PackCode is zero
        PACKCODEVALIDATION = -120, // To Check if Pack Code validation
        PACKCODEPIECESCOUNT = -121, // To Check if pieces count according to Pack Code
        AQLNOTEXIST = -123, // To Check if AQL Is exist or not
        EXEEDLIMIT = -40, // To check the userlimit exeed
        ITEMEXISTINLOADINGPLAN = -42,
        ITEMEXISTINDO = -43,
        CHECKSCQUANTITY = -44,
        CHECKMIEXIST = -45,//Material Issue not exist against Work order
        DUPLICATERECORDS = -25,
        RECORDNOTFOUND = -70,
        EMPWITHDIFFPROCESSMODE = -71,
        INVOICEEXISTASDRAFT = -28,//Invoice entry exists as draft
        FINYEARNOTEXIST = -112,//Financial year not entered
        CHECKHCPRINTER = -151, // To Check the hc_printer name
        CHECKPACKINGSPEC = -152, // To Check the packing_spec
        CHECKBRANDCODEEXIST = -153,  // To Check if brand code already exists,
        CHECKBRANDCODEORBRANDNAMEEXIST = -154,  // To Check if brand code or brand name exists,
        ALREADYREFERRED = -155,  // Alredy referred in another place
        CONVERSIONUSING = -101,
        LICENSEEECEEDS = -36,
        CHECKEXCHANGERATE = -48,
        CHECKCOA = -65,
        CHECKCS = -66,
        CHECKPLANT=-67,
        CHECKFINYEAR = -68,
        CHECKMONTH = -69,
        BUDGETEXIST = -70,

    }
    /// <summary>
    /// To indicate User Module Index
    /// </summary>
    public enum UserModuleIndex
    {
        Total = 0,
        Sip,
        Account,
        Production,
        HR,
        Asset,
        Construction
    }
    public enum DbDeleteStatus
    {
        REFERRED = 0,
        DELETED = 1,
        SQLERROR = -1,
        CONCURRENCY = -3,
        DELETECONCURRENCY = -5,
        ALREADYEXIST = -2
    }

    public enum DBActiveInactiveStatus
    {
        SUCCESS = 1,
        SQLERROR = -1,
        DELETED = -2,
        CONCURRENCY = -3,
        DELETECONCURRENCY = -5
    }
    public enum DbStatus
    {
        DRAFTED = 0,
        APPROVED = 2
    }
    public enum PageStatus
    {
        ENABLED = 1,
        DISABLED
    }
    public enum ClientCode
    {
        TMD = 1,
        IGCL,
        MMT,
        WARM
    }
    public class AutoCompleteBO
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public long LongKey { get; set; }
    }
    public class AutoCompleteComboBoxBO
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public long LongKey { get; set; }
        public string Code { get; set; }
    }

    public class AutoCompletePairedBO
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string PairText { get; set; }
    }

    public enum RecordStatus
    {
        INACTIVE = 0,
        ACTIVE = 1
    }

    public enum PurchaseType
    {
        Import = 1,
        Local
    }

    /// <summary>
    /// Represetns Moths
    /// </summary>
    public class Month
    {
        public int MValue
        { get; set; }
        public string MText
        { get; set; }
    }

    /// <summary>
    /// Represetns Page Sections
    /// </summary>
    public enum SectionsEnum
    {
        EntrySection,
        ListingSection
    }

    public class WorkFlowDetails
    {
        public int ReferenceID { get; set; }
        public int ProcessID { get; set; }
        public int TaskID { get; set; }
        public int Action { get; set; }
        public int IsClosed { get; set; }
        public int Type { get; set; }
    }

    public class MenuType
    {
        public int PK { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Application Type Enum
    /// </summary>
    public enum AppType
    {
        PurchaseInvoice = 10001,
        Payment = 10002,
        SalesInvoice = 10003,
        Receipt = 10004,
        CreditNote = 10005,
        DebitNote = 10006,
        StoreRequisitionSlip = 2001,
        PurchaseRequest = 2002,
        RequestForQuote = 2003,
        PurchaseOrder = 2004,
        GoodsReceiptNote = 2005,
        GoodsInspectionNote = 2006,
        StockTransfer = 2007,
        MaterialIssue = 2008,
        MaterialReceipt = 2009,
        ExternalMaterialIssue = 2010,
        ExternalMaterialReceipt = 2011,
        StoreIssue = 2012,
        StoreReceipt = 2013,
        StoreAudit = 2014,
        StoreAdjustment = 2015,
        MaterialConsumption = 2016,
        SalesQuotation = 2017,
        SalesOrder = 2018,
        FundRequestSlip = 4001,
        PurchaseInvoiceJournal = 10101,
        SupplierPaymentJournal = 10102
    }
    public class DocumentType
    {
        public const string PDF = "PDF";
        public const string EXCEL = "EXCEL";
        public const string WORD = "WORD";

    }

    /// <summary>
    /// Application Type class
    /// </summary>
    public class ApplicationType
    {
        public const string LST_VCH = "LST_VCH";
        public const string SRS = "SRS";
        public const string SR = "SR";
        public const string SOA = "SOA";
        public const string SRA = "SRA";
        public const string PR = "PR";
        public const string MTR = "MTR";
        public const string MTI = "MTI";
        public const string OPLN = "OPLN";//Order Planning

        public const string PRT = "PRT";
        public const string RFQ = "RFQ";
        public const string PO = "PO";
        public const string POG = "POG";
        public const string POP = "POP";
        public const string POTR = "POTR";
        public const string POPG = "POPG";
        public const string DO = "DO";
        public const string CID = "CID";
        public const string SALEO = "SALEO";
        public const string GRN = "GRN";
        public const string GIN = "GIN";
        public const string STR = "STR";
        public const string DSA = "DSA";
        public const string MI = "MI";
        public const string MA = "MA";
        public const string VNDEVAL = "VNDEVAL";
        public const string MR = "MR";
        public const string IO = "IO";
        public const string EMI = "EMI";
        public const string EMR = "EMR";
        public const string MRT = "MRT";
        public const string SRI = "SRI";
        public const string SRR = "SRR";
        public const string SRD = "SRD";
        public const string SRJ = "SRJ";
        public const string MC = "MC";
        public const string SQ = "SQ";
        public const string SO = "SO";
        public const string SOD = "SOD";
        public const string FRS = "FRS";
        public const string ACI = "ACI";
        public const string ACIJ = "ACIJ";
        public const string SHCID = "SHCID";
        //For Advance Sale Invoice
        public const string SAD = "SAD";
        public const string ES = "ES";
        public const string ESJ = "ESJ";
        // 

        //For Miscellaneous
        public const string MSI = "MSI";
        public const string MIDO = "MIDO";
        public const string MSIR = "MSIR";
        public const string MSIJ = "MSIJ";
        public const string MSIRJ = "MSIRJ";
        //For Payment Goods
        public const string VP = "VP";
        //For Payment Service
        public const string SIP = "SIP";
        //For Payment Expense
        public const string EIP = "EIP";
        //For Payment Agent Inv
        public const string AIP = "AIP";

        //For PO Invoice Goods
        public const string PI = "PI";
        //For PO Invoice Import
        public const string PII = "PII";
        //For PO Invoice Service
        public const string PSI = "PSI";
        //For PO Invoice Expense
        public const string EI = "EI";

        public const string SI = "SI";
        public const string SIM = "SIM";
        public const string SIMSF = "SIMSF";
        public const string SIMSS = "SIMSS";
        public const string SID = "SID";
        public const string SIE = "SIE";
        public const string SIC = "SIC";
        public const string FCHR = "FCHR";
        public const string CR = "CR";
        public const string CRT = "CRT";
        public const string VCN = "VCN";
        public const string VDN = "VDN";
        public const string CN = "CN";
        public const string DN = "DN";
        public const string AP = "AP";
        public const string ADP = "ADP";
        public const string AR = "AR";
        public const string ADR = "ADR";
        public const string CRTJ = "CRTJ";
        public const string MSIRTJ = "MSIRTJ";
        //For Journal
        public const string PIJ = "PIJ";
        public const string PSIJ = "PSIJ";
        public const string EIJ = "EIJ";
        public const string FCHRJ = "FCHRJ";//FC Revert

        //for Addition/Deduction
        public const string EMPADDDED = "EMPADDDED";

        //for Stock Transfer Accept
        public const string STA = "STA";

        //for Employee Training
        public const string ETR = "ETR";

        public const string VPJ = "VPJ";
        public const string SIPJ = "SIPJ";
        public const string EIPJ = "EIPJ";
        public const string AIPJ = "AIPJ";

        public const string SIJ = "SIJ";
        public const string SPJ = "SPJ";
        public const string VCNJ = "VCNJ";
        public const string VDNJ = "VDNJ";
        public const string CNJ = "CNJ";
        public const string DNJ = "DNJ";
        public const string VND = "VND";
        public const string COA = "COA";
        public const string QAC = "RFQCMP";
        public const string SAS = "SAS";
        public const string PSAS = "PSAS";
        public const string CRJ = "CRJ";
        public const string PDCCJ = "PDCCJ";
        public const string PPCCJ = "PPCCJ";
        public const string RCBJ = "RCBJ";
        public const string PCBJ = "PCBJ";
        public const string DPVCJ = "DPVCJ";
        public const string DPBJ = "DPBJ";
        public const string DRVJ = "DRVJ";
        public const string DRVCJ = "DRVCJ";
        public const string DRBJ = "DRBJ";

        public const string PPCCTJ = "PPCCTJ";
        public const string PCBTJ = "PCBTJ";

        //Compound Usage - Summary MIS Report PDF
        public const string COMR = "COMR";

        //Compound Yield Cost Report
        public const string CDC = "CDC";

        //Cost Center Wise Ledger Details
        public const string COL = "COL";

        public const string SFG = "SFG";
        public const string SFGCD = "SFGCD";
        public const string SFGBL = "SFGBL";

        public const string SFGD = "SFGD";
        public const string SFGCDSP = "SFGCDSP";
        public const string SFGBDSP = "SFGBDSP";

        public const string SCWO = "SCWO";//Sub Cont Work Order
        //For Vouchers

        public const string CTVJ = "CTVJ";//Contra Entry Voucher
        public const string JV = "JV";
        public const string PCS = "PCS";
        public const string BNR = "BNR";
        public const string BNP = "BNP";
        public const string CSP = "CSP";
        public const string TB = "TB";
        public const string AS = "AS";
        public const string GLFIN = "GLFIN";
        public const string VTYPE = "VTYPE";
        public const string VTYPESIJ = "VTYPESIJ";
        public const string VTYPEPSIJ = "VTYPEPSIJ";
        public const string VTYPEVPJ = "VTYPEVPJ";
        public const string VTYPECRJ = "VTYPECRJ";
        public const string VTYPEMSIRJ = "VTYPEMSIRJ";
        public const string VTYPEPCVJ = "VTYPEPCVJ";
        public const string VTYPEDPVJ = "VTYPEDPVJ";
        public const string VTYPEDRVJ = "VTYPEDRVJ";
        public const string VTYPEMIJ = "VTYPEMIJ";
        public const string VTYPEEIJ = "VTYPEEIJ";
        public const string VTYPEMSIJ = "VTYPEMSIJ";
        public const string VTYPECNJPI = "VTYPECNJPI";
        public const string VTYPECNJSI = "VTYPECNJSI";
        public const string VTYPEDNJSI = "VTYPEDNJSI";
        public const string VTYPEDNJPI = "VTYPEDNJPI";
        public const string VTYPEDPRJ = "VTYPEDPRJ";
        public const string VTYPECWIPJ = "VTYPECWIPJ";

        public const string CENQ = "CENQ";
        public const string CDOR = "CDOR";
        public const string CQTN = "CQTN";
        public const string BRC = "BRC";

        public const string OBV = "OBV";
        public const string DPVJ = "DPVJ";
        public const string PCVJ = "PCVJ";
        public const string PCRVJ = "PCRVJ";
        // For Comercial invoice
        public const string CI = "CI";
        public const string TI = "TI";


        //For ContainerEvaluation
        public const string CNTEVAL = "CNTEVAL";
        //For ContainerInspection
        public const string CNTINSP = "CNTINSP";


        //For Store Requistion
        public const string OTRM = "OTRM";

        //For ShiftReport
        public const string SFTRPT = "SFTRPT";

        //ShippingPlan
        public const string SPLN = "SPLN";
        public const string LPLN = "SPLN(LPLN)";

        public const string SPRR = "SPLN"; //Shipping Plan Receipt Report

        public const string CBR = "CBR";

        public const string SALFRCST = "SALFRCST";
        public const string OUTDUE = "OUTDUE";

        public const string GeneralMail = "GENMAIL";
        public const string MailQueue = "MAILQ";

        public const string SPLNPM = "SPLNPM";
        //Cash Flow
        public const string CFLW = "CFLW";

        // Finance- Vat Sale Export
        public const string VSE = "VSE";

        // Finance- GST Reports
        public const string GST = "GST";

        //Finance- GST Return
        public const string GSTR = "GSTR";

        //Monthly Production
        public const string FMP = "FMP";

        public const string EMP = "EMP";

        //Compound Preparation
        public const string CMP = "CMP";

        //Dispersion Preparation
        public const string DISP = "DISP";

        public const string BOM = "BOM";

        //Depreciation Preparation
        public const string DPR = "DPR";
        public const string DPRJ = "DPRJ";
        public const string RDPRJ = "RDPRJ";

        //Asset Disposal
        public const string ASD = "ASD";
        public const string ASDJ = "ASDJ";

        //Year End Voucher
        public const string PIJYE = "PIJYE";
        public const string VPJYE = "VPJYE";
        public const string SIJYE = "SIJYE";
        public const string CRJYE = "CRJYE";
        public const string EIJYE = "EIJYE";
        public const string PSIJYE = "PSIJYE";
        public const string EIPJYE = "EIPJYE";
        public const string SIPJYE = "SIPJYE";
        public const string MSIJYE = "MSIJYE";
        public const string MSIRJYE = "MSIRJYE";
        public const string FCHRJYE = "FCHRJYE";
        public const string CNSJYE = "CNSJYE";
        public const string DNSJYE = "DNSJYE";
        public const string CNPJYE = "CNPJYE";
        public const string DNPJYE = "DNPJYE";

        public const string YE = "YE";
        public const string BD = "BD";
        public const string BDJ = "BDJ";//Application Type for Bad Debt journal
        public const string CLST = "CLST";

        public const string YCV = "YCV";
        public const string CLSTJ = "CLSTJ";
        public const string OS = "OS";//Opening Stock

        public const string REPORTCONFIG = "REPORTCONFIG";   // Reports
        public const string PAYRL = "PAYRL";
        public const string PAYRLJ = "PAYRLJ";
        public const string PAYRL1 = "PAYRL1";
        public const string HLDM = "HLDM"; //for HolidayList report
        public const string SALPYMT = "SALPYMT"; //for Salary payment report
        public const string SALPYMTJ = "SALPYMTJ"; //for Salary payment Voucher
        public const string SALSLP = "SALSLP"; //for Salary slip report
        public const string EAP = "EAP";      // Employee Appraisal Details
        public const string EBA = "EBA";      // Bulk Appraisal Details


        public const string ITC = "ITC"; //For Incometax Computation report
        public const string EMPATTND = "EMPATTND"; //For Attendance Report
        public const string EMPATTNDDTLS = "EMPATTNDDTLS"; //For Attendance Details Report
        public const string STRLOMS = "STRLOMS"; //For Store Location Master
        public const string BSRC = "BSRC"; //For Store Reconciliation Report
        public const string ETF = "ETF"; //Employee Transfer

        public const string DOD = "DOD"; //Delivery Order Direct
        public const string DSI = "DSI"; // Direct Sales Invoice 
        public const string DSID = "DSID"; // Direct Sales Invoice Trading
        public const string DSIJ = "DSIJ"; // Direct sales invoice voucher       
        public const string EIT = "EIT"; //For Expense Invoice Trading
        public const string EITJ = "EITJ"; //For Expense Invoice Trading Voucher

        public const string POT = "POT";	//Purchase Order Trading
        public const string MSIT = "MSIT";  //Misc Invoice Trading
        public const string MSITJ = "MSITJ";//Misc Invoice Trading Voucher

        public const string TPI = "TPI";//Purchase Invoice Trading
        public const string TPSI = "TPSI";//Purchase Service Invoice Trading
        public const string TPIJ = "TPIJ";//Purchase Invoice Trading Journal
        public const string TPSIJ = "TPSIJ";//Purchase Service Invoice Trading Journal

        public const string CNT = "CNT";//Credit Note Trading
        public const string DNT = "DNT";//Debit Note Trading
        public const string CNTJ = "CNTJ";//Credit Note Trading Journal
        public const string DNTJ = "DNTJ";//Debit Note Trading Journal

        public const string VPT = "VPT";//Payment Goods Trading
        public const string EIPT = "EIPT";//Expense Invoice Payment Trading
        public const string SIPT = "SIPT";//Service Invoice Payment Trading
        public const string AIPT = "AIPT";//Agent Invoice Payment Trading

        public const string VPTJ = "VPTJ";//Payment Goods Trading Journal
        public const string EIPTJ = "EIPTJ";//Expense Invoice Payment Trading Journal
        public const string SIPTJ = "SIPTJ";//Service Invoice Payment Trading Journal 
        public const string AIPTJ = "AIPTJ";//Agent Invoice Payment Trading Journal
        public const string PDCCTJ = "PDCCTJ";//PDC clear voucher Trading(Receipt)
        public const string RCBTJ = "RCBTJ";//Cheque return voucher Trading(Receipt)
        public const string MIJ = "MIJ";//Material Issue Journal

        public const string FRD = "FRD";//Fund Request (Department wise)

        public const string RAB = "RAB";//For RA Bill
        public const string WO = "WO";//For Work Order
        public const string SUBWOD = "SUBWOD";//For Work Order
        public const string SWO = "SWO";//For Sub Contract
        public const string BOQ = "BOQ";//For BOQ
        public const string MPG = "MPG";//For Monthly Programme
        public const string MBSO = "MBSO";//For MBook Entry
        public const string CBL = "CBL";//For Client bill
        public const string DE = "DE";//For Daily Expenditure
        public const string WRA = "WRA"; //Rate Analysis
        public const string WBG = "WBG"; //Budget
        public const string VCT = "VCT"; //For variable cost
        public const string IFC = "IFC"; //Issue Factor
        public const string COSTING = "COSTING"; //Costing Report
        public const string PROJECT = "PROJECT"; //Project Dropdown
        public const string DO2 = "DO2"; //SI Packing List Report

        public const string BINCARDISSUEWO = "BCIWO";
        public const string CARTONISSUEWO = "BCPIWO";
        public const string DONOTE = "DONOTE";
        public const string MCR = "MCR";
        public const string PRJ = "PRJ";//For Work Order
        public const string LOCATION = "LOCATION";
        public const string CWIP = "CWIP";
        public const string CWIPJ = "CWIPJ";//Journal

        public const string FSMT = "FSMT";

        public const string RMI = "RMI";
        public const string RMIPM = "RMIPM";
    }
    public class TransactionType
    {
        public const string CANCELATION = "CANCELATION";
    }
    public enum AccountSubType
    {
        AP = 1,
        AR,
        Purchase,
        Sales,
        VatBuy,
        Freight,
        Discount,
        Adjustments,
        General,
        Cash,
        Bank,
        ADP,
        ADR,
        VatSale,
        WHT,
        GLAdjS,
        PDC,
        SalesReturn,
        Inventory,
        PPC,
        PurchaseReturn,
        Expense,
        BankCharge,
        Income,
        GLAdjP
    }
    public enum AppSubTypePOInvoice
    {
        INVOICE = 0,
        ADVINVOICE = 11
    }
    public enum AppSubTypeSOInvoice
    {
        INVOICE = 0,
        ADVINVOICE = 11
    }
    public enum AppSubTypeDebitCreditNote
    {
        VENDOR = 1,
        CUSTOMER = 2,
        EXPORT_COMMERICAL = 21,
        DOMESTIC = 22
    }
    public enum AppSubTypeCNSales
    {
        EXPORT_COMMERICAL = 21,
        DOMESTIC = 22
    }
    public enum AppSubTypeDNSales
    {
        EXPORT_COMMERICAL = 21,
        DOMESTIC = 22
    }
    public enum AppSubTypeCNPurchase
    {
        STOCK = 11,
        NONSTOCK = 12
    }
    public enum AppSubTypeDNPurchase
    {
        STOCK = 11,
        NONSTOCK = 12
    }


    /// <summary>
    /// Application Sub Type class
    /// </summary>
    public class AppSubType
    {
        public const string SRS = "SRS";
        public const string PR = "PR";
        public const string RFQ = "RFQ";
        public const string PO = "PO";
        public const string GRN = "GRN";
        public const string GIN = "GIN";
        public const string STR = "STR";
        public const string MI = "MI";
        public const string MR = "MR";
        public const string EMI = "EMI";
        public const string EMR = "EMR";
        public const string SRI = "SRI";
        public const string SRR = "SRR";
        public const string SRD = "SRD";
        public const string SRJ = "SRJ";
        public const string MC = "MC";
        public const string SQ = "SQ";
        public const string SO = "SO";
        public const string FRS = "FRS";
        public const string VP = "VP";
        public const string PI = "PI";
        public const string SI = "SI";
        public const string CR = "CR";
        public const string CN = "CN";
        public const string DN = "DN";
        public const string PIJ = "PIJ";
        public const string SPJ = "SPJ";
        public const string SPLNLP = "1";
        public const string SPLNEF = "2";
        public const string PAYPREPCSRPT = "4";
    }

    /// <summary>
    /// Dynamic Form List
    /// </summary>
    public class FormType
    {
        public const string CUS = "CUS";
        public const string FTH = "FTH";
    }

    /// <summary>
    /// Dynamic Tab List
    /// </summary>
    public class TabType
    {
        public const string CLST = "CLST";
        public const string CUS = "CUS";
        public const string CCD = "CCD";
        public const string CRD = "CRD";
        public const string CBD = "CBD";
        public const string CIM = "CIM";
        public const string TCH = "TCH";
        public const string PI = "PI";
        public const string SP = "SP";
    }

    /// <summary>
    /// Represetns ADM_CONST_GRP_TYPE
    /// </summary>
    public enum ConstGroupType
    {
        Product = 1,
        Packing = 11,
        Shipment = 13,
        CheckList = 14,
        ContainerType = 74,
        SaleContract = 15,
        PackingType = 78,
        ReceiptAdjustments = 17,
        VoucherTemplateType = 18,
        HISCode = 13,
        WHTFormNo = 19,
        // TaskGroup=21,
        TaskCategory = 22,
        EDocSetUp = 36,
        Classification = 7,
        HRMSSetup = 21,
        VendorDocType = 42,
        CustomerProperties = 9
    }

    /// <summary>
    /// Represetns ADM_CONST_GRP_TYPE
    /// </summary>
    public enum ConstGroup
    {
        HIScode = 11,
        WHTFormnoval = 1,
        SpecialCategory = 36
    }

    public enum Adjustments
    {
        Receipt = 1,
        Payment
    }

    /// <summary>
    /// Tax Types
    /// </summary>
    public enum TaxType
    {
        Tax = 1,
        Shipping,
        Discount,
        Duties = 5,
        Surcharges = 6
    }
    /// <summary>
    /// Tax Types
    /// </summary>
    public enum From_ERP
    {
        DO = 1
    }
    /// <summary>
    /// Shipment Type in Constant Table
    /// </summary>
    public enum ConstShipmentType
    {
        FromPort = 1,
        Transhipment,
        OriginOfGoods,
        ShipBy,
        BillOfLoading = 10,
        ShipmentTerms = 14
    }

    public enum SalesInvoiceType
    {
        Domestic = 1,
        Export,
        Proforma,
        Deemed = 4,
        InvoiceTermsMain = 5,
        InvoiceTerms1 = 6,
        InvoiceTerms2 = 7
    }
    public enum YearEndVoucherSearch
    {
        Other = 0,
        PartySelect = 1,
        VoucherSelect,
        TransactionSelect
    }
    /// <summary>
    /// Paking Type
    /// </summary>
    public enum PackingType
    {
        PakingMaterialType = 6,
        PackingItemType = 12

    }
    /// <summary>
    /// Customer Term Types
    /// </summary>
    public enum CustomerTermType
    {
        DeliveryTerms = 1,
        PaymentTerms = 2,
        SpecialCause = 3,
        InvoiceTerms = 5
    }

    public class CheckListTypes
    {
        public const string Shipment = "SP";
    }
    public enum CustomerAddressType
    {
        ShippingAddress = 1,
        ShippingAgent = 2,
        Consignee = 3,
        NotifyingParty = 4,
        NotifyParty = 6,
        HeadOfficeAddress = 4
    }

    public enum CashBankType
    {
        Bank = 2,
        Cash = 1
    }

    public enum Mode
    {
        Allocation = 2,
        Dispatch = 4,
        ShiftReport = 5,
        FormerAllocation = 6,
        SaleOrder = 7,
        Packing = 8

    }

    public enum Category
    {
        Product = 1,
        Former

    }

    public enum ProductCategory
    {
        Product = 2

    }

    public enum AllocationType
    {
        Initial = 1
    }

    public enum DOCMODE
    {
        Draft = 1,
        Submit = 0,
        Manual = 2
    }

    public enum EnqWorkFlowStatus
    {
        EnquiryDrafted = 0,
        EnquirySubmitted = 1,
        EnquiryApproved = 2,
        EnquiryRejected = 3,
        EnquiryReviewed = 4,
        EnquiryRequestedformoreInfo = 6,
        EnquiryMoreInfoSubmitted = 7,
        EnquiryRequestedtoreviewwithmoreInfo = 8,
        EnquiryReviewedwithmoreInfo = 9,
        QuotationDrafted = 10,
        QuotationSubmitted = 11,
        QuotationFinallyApproved = 12,
        QuotationRejected = 13,
        QuotationReviewed = 14,
        QuotationApproved = 15,
        QuotationRequestedformoreInfo = 16,
        QuotationMoreInfoSubmitted = 17,
        QuotationRequestedtoreviewwithmoreInfo = 18,
        QuotationReviewedwithmoreInfo = 19,
        QuotationRequestedformoreInfofromfinalapproval = 21,
        QuotationSubmittedtoFinalapprovalwithmoreinfo = 22
    }
    public enum ReportTemplate
    {
        All = 0,
        BalanceSheet = 1
    }
    public class CFGType
    {
        public const string Shipping = "SHIPPING STATUS";
        public const string ReportCurrency = "Currency Settings";
        public const string ItemWiseTaxSetting = "SALE ITEM WISE SETTINGS";
        public const string ExpenseInvUOM = "EXPENSE INVOICE UOM";
        public const string CBM = "CBM LIMIT";
    }

    public class PayemtMode
    {
        public const string Cheque = "2";
    }
    public enum DateDefaultEnum
    {
        CurrentDate = 0,
        FirstDate,
        LastDate
    }

    public enum DbDeleteStatusSC
    {
        InvoiceCreated = -11,
        ShippingCreated = -12
    }

    public enum TaxSubCategory
    {
        VATBuy = 1,
        VATSale = 2,
        WHT = 3
    }

    public enum AppSubTypePO
    {
        NONSTOCK = 11
    }

    public enum POGroup
    {
        Other = 0,
        Goods = 1,
        Services = 2,
        Workorder = 6
    }
    public enum AccSubType
    {
        All = 0,
        VatBuy = 5,
        WHT = 15,
        GainLossSales = 16,
        BankCharge = 23,
        GainLossPurchase = 26,
        PPC = 20,
        PDC = 17
    }
    public enum DOSubType
    {
        General = 0,
        CommercialInvoice,
        PackingList,
        ShippingInstructions,
        CertificateofOrigin,
        PostShipmentAdvice,
        DeliveryOrder,
        PackingList_2_LP,
        PackingList_LP,
    }
    public enum POInvoiceGroup
    {
        Goods = 1,
        Services = 2,
        Expense,
        AgtInvoice,
        ExpenseSettilement,
        WorkOrder
    }

    public enum POInvoiceCategory
    {
        Invoice = 1,
        Advanced,
    }


    public enum SalesInvoiceGroup
    {
        Goods = 1,
        Miscellaneous = 3
    }

    public enum SalesInvoiceCategory
    {
        Invoice = 1,
        Advanced,
    }

    public enum AlertType
    {
        System = 1
    }

    public class TaxFilterType
    {
        public const string SAL = "SAL";
        public const string PUR = "PUR";
    }

    public enum AppSubTypeVP
    {
        WHTCERTIFICATE = 2,
        PND54 = 3
    }

    public class SCWorkFlowType
    {
        public const string SC = "1";
        public const string Amend = "2";
        public const string Cancel = "3";
    }

    public enum POWorkflowType
    {
        PO = 1,
        AMEND = 2,
        CANCEL = 3
    }

    public enum PaymentModeEnum
    {
        Cash = 1,
        Cheque = 2,
        DD = 3,
        Bank = 4,
        General = 5
    }
    public enum TaxStatus
    {
        Exclude = 0,
        Include = 1,
        Only = 2
    }

    /// <summary>
    /// WHT category Enum
    /// </summary>
    public enum WHTCategoryEnum
    {
        WHT = 1,
        VATBUY = 2,
        VATSALE = 3
    }


    /// <summary>
    /// Vendor contact type Enum
    /// </summary>
    public enum VendorContactTypeEnum
    {
        HeadOffice = 4,
        Branch = 5
    }



    /// <summary>
    /// Vendor contact type Enum
    /// </summary>
    public enum AgCommInvoiceSettledTypeEnum
    {
        Settled = 1,
        NonSettled = 0
    }


    /// <summary>
    /// WHT Type Enum
    /// </summary>
    public enum WhtTypeEnum
    {
        DEFINEDTAX = 1,
        CUSTOMTAX = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TaxTypes
    {
        VATBuy = 2,
        VATBuyNotYetDue = 10
    }

    public enum LogAction
    {
        NEW = 1,
        UPDATE = 2,
        DELETE = 3,
        SUBMIT = 4,
        CANCEL = 5
    }


    public enum DebitCreditModeEnum
    {
        DEBIT = 1,
        CREDIT = 2

    }

    public enum ProductProperties
    {
        Type = 1,
        Thickness = 2,
        Category = 3,
        Surface = 4,
        Shade = 5,
        Classification = 6,
        Size = 7,
        Length = 8,
        Chlorination = 9,
        Side = 12,
        AdnlSpec05 = 16,//Colour Category
        AdnlSpec06 = 17,//Former Type
        AdnlSpec07 = 18,//Former Size
        Color = 5,
        PrintType = 14
    }

    public enum PrinterMode
    {
        DOTMATRIX,
        LASER
    }

    public enum GroupTypeConstantValue
    {
        YearEnd = 27
    }

    public enum GroupConstantValue
    {
        YearEndProcess = 1
    }

    public enum ITEMTYPE
    {
        StockItem = 1,
        OfficeConsumables = 2,
        ReturnableToolsMAC = 3,
        FG = 4
    }

    public enum Store
    {
        InventoryStore = 2,
        CompoundStore = 4
    }

    public enum ShippingSatus
    {
        CANCEL = 4
    }
    public enum POSubType
    {
        Service = 11
    }

    public enum MaritalStatus
    {
        Single = 1,
        Married = 2,
        Separated = 3,
        Divorced = 4,
        Widowed = 5
    }

    public enum PayElementCalcMode
    {
        FixedAmount = 0,
        Formula = 1,
        Slab = 2,
        Custom = 3
    }

    public enum PayrollProcessMode
    {
        Monthly = 1,
        Periodic = 2
    }

    public enum ProcessAction
    {
        Process = 0,
        Reprocess = 1
    }
    public enum LockingModule
    {
        SMS = 2,
        Finance = 10
    }

    public enum ItemCategory
    {
        Former = 6
    }

    public enum EmployeeCategory
    {
        SystemEmployee = 0,
        CustomerEmployee = 1,
        HRMSEmployee = 2
    }

    public enum PayClassifications
    {
        BasicPay = 1,
        Allowance = 11,
        Overtime = 21,
        Earnings = 51,
        StatutoryDeductions = 101,
        TaxDeductions = 111,
        Loans = 121,
        Advances = 131,
        Deductions = 151
    }

    public enum WorkingDayType
    {
        Fixed = 1,
        Calendardays = 2,
        Custom = 3
    }
    public enum AttendanceTemplateType
    {
        GENERAL = 1,
        IGCL = 2,
        IGCL_TXT = 3
    }
    public enum AttendanceEntryMode
    {
        Daily = 1,
        Cumulative = 2
    }
    public enum HrmsPayrollOffset
    {
        Previous = -1,
        Current = 0
    }

    public enum EmpTabEnum
    {
        EmpList = 1,
        EmpDetails = 2,
        Qualifications = 3,
        Experiance = 4,
        SkillsAndExpertise = 5,
        Documents = 6,
        PayDetails = 7,
        LeaveType = 8,
        Salary = 9,
        SalaryRevision = 10,
        ITDeclaration = 11,
        BEHAVIOUR = 12,
        TRAINING = 13
    }

    public enum PayrollTabEnum
    {
        PREPROCESSDATA = 1,
        SALARYPROCESS = 2
    }
    /// <summary>
    /// Workflow Transaction Flag
    /// </summary>
    public enum WorkflowTransactionFlag
    {
        SAVE = 0,
        SAVEANDSUBMIT = 1,
        SUBMIT = 2
    }

    /// <summary>
    /// Department Type Enum
    /// </summary>
    public enum DeptTypeEnum
    {
        FINANCE = 8,
        HRMS = 11
    }

    /// <summary>
    /// Dash board Type Enum
    /// </summary>
    public enum DashboardTypeEnum
    {
        MENU = 1,
        WIDGET = 2
    }
    /// <summary>
    /// Dash board Mode Enum
    /// </summary>
    public enum DashboardModeEnum
    {
        MENU = 2,
        WIDGET = 3
    }
    /// <summary>
    /// Dash board Type Enum
    /// </summary>
    public enum DashboardTilesEnum
    {
        TASK = 1,
        SMSINBOX = 4,
        EDOCS = 5,
        CIVIL = 6,
        HRMS = 7,
        PRODUCTION = 8,
        PACKING = 9,
        QA = 10,
        WIP = 11,
        WAREHOUSE = 12
    }

    /// <summary>
    /// SALARY APPRAISAL
    /// </summary>
    public enum VisbleStatusEnum
    {
        FALSE = 0,
        TRUE = 1
    }

    public enum NumericControlType
    {
        Currency,
        ExchangeRate,
        MiscRate,
        Number,
        NumberPurchase,
        NumberCompounding,
        NumberBin,
        NumberConstruction,
        Rate,
        RatePurchase,
        RateConstruction,
        AvgWeightPrd,
        WeightPrd,
        Weight,
        NumericInteger
    }

    /// <summary>
    /// Dash board Type Enum
    /// </summary>
    public enum ControlParentPage
    {
        EmpDoc = 1,
        EmployeeMaster = 2
    }
    public enum DbImportStatus
    {
        SQLERROR = -1,
        CONCURRENCY = -3,
        NOEXELROWS = -70,
        ALREADYEXISTS = -7,
        EMPLOYEENOTFOUND = -71,
    }

    public enum TrxType
    {
        SaleOrder = 1,
        DirectSaleOrder = 2
    }
    public enum VoucherVersions
    {
        None = -1
    }
    /// <summary>
    /// Print Mode
    /// </summary>
    public class PrintMode
    {
        public const string MSWord = "MSWORD";
        public const string MSExcel = "MSEXCEL";
        public const string Pdf = "PDF";
    }
    public enum Day
    {
        SUN = 1,
        MON = 2,
        TUE = 3,
        WED = 4,
        THU = 5,
        FRI = 6,
        SAT = 7

    }
    public enum OtherLeaveType
    {
        Yearly = 1,
        Monthly = 2,
        SpecialHolidayInMonth = 3
    }
}
