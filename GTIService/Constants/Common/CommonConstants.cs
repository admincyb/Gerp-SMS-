using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Common
{
    public class CommonConstants
    {
        public const string USER = "P_USER";
        public const string USERPK = "P_USER_PK";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string DEPARTMENT = "P_DEPT";
        public const string MODULE = "P_MODULE";
        public const string COSTCENTER = "P_COST_CENTER";
        public const string TIMEZONE = "P_TMZ_PK";
        public const string CREATEDBY = "P_Crtd_By";
        public const string RETURNVAL = "P_RET_VAL";
        public const string ACTIVESTATUS = "P_Active";
        public const string SELECTVAL = "-1";
        public const string SELECTTEXT = "--Select--";
        public const string DATEFORMAT = "dd/MM/yyyy";
        public const string LASTMODDATE = "P_LAST_MOD_DT";
        public const string LASTMODDATETIME = "LAST_MOD_DT";
        public const string PROP_LASTMODDATETIME = "LastModifiedDateTime";
        public const string SELECT_VALUE_ONE = "1";
        public const string SELECT_VALUE_ZERO = "0";
        public const string SELECTSBUTEXT = "Select SBU";

        public const string SELECT_VALUE_EMPTY = " ";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_PAGE_NUM = "P_PAGE_NUM";
        public const string P_PAGENO = "P_PAGE_NO";
        public const string P_PAGESIZE = "P_PAGE_SIZE";
        public const string PAGESIZE = "10";
        public const string SORT_ASC = "ASC";
        public const string SORT_DESC = "DESC";
        public const string FIRST_PAGE = "0";
        public const string LAST_MODIFIED_DATETIME_LABEL = "lblLastModifiedDateTime";
        public const string ALL = "ALL";
        public const string ALLVAL = "0";
        public const string ADDIMAGEURL = "~/Images/ERP-Blue/Buttons/gbudget-btn-bookmark.png";
        public const string REMOVEIMAGEURL = "~/Images/ERP-Blue/Buttons/gbudget-btn-rbookmark.png";
        public const string ADDIMAGETOOLTIP = "Add this page to favourites";
        public const string REMOVEIMAGETOOLTIP = "Remove this page from Favourites";

        public const string PROCESSCONTROLTYPE_TEXT = "Process Control Type";

        public const string P_SEARCH_FIELD = "P_SER_NAME";
        public const string P_SEARCH_VALUE = "P_SER_VAL";
        public const string P_FIELDS = "P_FIELDS";
        public const string P_SEARCH_FROM_DATE = "P_FROM_DT";
        public const string P_SEARCH_TO_DATE = "P_TO_DT";
        public const string ZERO = "0";
        public const string SPACE = " ";
        public const string SEARCH_MENU = "P_MNU_NAME";
        public const string P_MODULE = "P_MODULE";

        public const string P_Refrerence = "PRefrerence";
        public const string P_PageUrl = "PpageUrl";
        public const string P_Process = "Pprocess";
        public const string P_UserPK = "PusrPK";
        public const string P_CustomerPK = "P_CUS_PK";
        public const string P_REFID = "P_REFID"; 
        public const string P_ItemPK = "P_ITM_PK"; 
        public const string LastModDateFormat = "yyyy-MM-dd H:mm:ss";

        public const string P_UserInoxType = "PusrInboxType";
        public const string P_RetVal = "PretVal";

        public const string DLC_PK = "P_DLC_PK";
        public const string DLC_USER = "P_DLC_USER";

        public static string P_FROM_DATE = "P_FROM_DATE";
        public static string P_TO_DATE = "P_TO_DATE";

        public static string P_REPORT_GRP = "P_RPT_GROUP";

        //Menu Edit
        public static string P_MNS_PK = "P_MNS_PK";
        public static string P_MNG_PK = "P_MNG_PK";
        public static string P_MNU_PK = "P_MNU_PK";
        public static string P_MNS_NAME = "P_MNS_NAME";
        public static string P_MNS_NAME2 = "P_MNS_NAME2";
        public static string P_MNG_NAME = "P_MNG_NAME";
        public static string P_MNG_NAME2 = "P_MNG_NAME2";
        public static string P_MNU_NAME = "P_MNU_NAME";
        public static string P_MNU_NAME2 = "P_MNU_NAME2";
        public static string V_LAST_MOD_DT = "V_LAST_MOD_DT";
        public static string P_MNG_SECTION = "P_MNG_SECTION";
        public static string P_MNU_GROUP = "P_MNU_GROUP";

        public const string CMP_DISPLAY_CODE = "CMP_DISPLAY_CODE";
    }

    public class ViewstateStrings
    {
        public const string EntryState = "EntryState";
        public const string CurrPK = "CurrPK";
        public const string CurrStatus = "CurrStatus";
        public const string RetVal = "RetVal";
        public const string PageIndex = "PageIndex";
        public const string TotalPages = "TotalPages";
        public const string SortBy = "SortBy";
        public const string SortDirection = "SortDirection";
        public const string LastModifiedTime = "LastModifiedTime";
        public const string FilterBy = "FilterBy";
        public const string FilterValue = "FilterValue";
        public const string LastModifiedSPETime = "LastModifiedSPETime";
        public const string LastModifiedSPCTime = "LastModifiedSPCTime";
        public const string FilePath = "FilePath";
        public const string FileName = "FileName";
        public const string LogoFilePath = "LogoFilePath";
        public const string LogoFileName = "LogoFileName";
        public const string UserType = "UserType";
        public const string UserLimitSetting = "UserLimitSetting";


        public const string CurrMode = "CurrMode";
        //Exchange Rate
        public const string CurrencyFromPK = "CurrencyFromPK";
        public const string CurrencyToPK = "CurrencyToPK";

        //Asset Maintanence Details

        public const string AmhAssetPK = "AmhAssetPK";
        // For misellenious

        public const string BuyCondPK = "BuyCondPK";
        public const string DepreciationPK = "DepreciationPK";
        public const string AmiAssetPK = "AmiAssetPK";

        public const string OutwardNotePK = "OutwardNotePK";

        // For Knowledge base TypePK
        public const string KbPK = "KbPK";
        public const string KbTypePK = "KbTypePK";
        public const string AkbAssetPK = "AkbAssetPK";

        // For Country pages
        public const string CurrencyPK = "CurrencyPK";

        // For vendor Pages
        public const string VendorPK = "VendorPK";
        public const string VendorCode = "VendorCode";
        public const string VendorName = "VendorName";

        //For checklist
        public const string ChecklistPK = "ChecklistPK";
        public const string ChecklistCode = "ChecklistCode";
        public const string ChecklistName = "ChecklistName";

        //Type PK for checklist
        public const string TypePK = "TypePK";

        //Parent PK for Heirarchical Items
        public const string CurrParent = "CurrParent";
        //For Resource Category
        public const string ResourceCategory = "ResourceCategory";

        //For Asset Category
        public const string AssetCategory = "AssetCategory";

        //For Uom page
        public const string BasisPK = "BasisPK";
        //For  Employee
        public const string CountryPK = "CountryPK";
        //For Project info
        public const string ProjectTypPK = "ProjectTypPK";

        //For Asset Basic Info
        public const string AssetCode = "AssetCode";
        public const string GridType = "GridType";
        public const string AssetCondnPK = "AssetCondnPK";
        public const string LocationName = "LocationName";
        public const string CustodianName = "CustodianName";
        public const string OwnerName = "OwnerName";

        //For parts&subparts
        public const string AssetParentPK = "AssetParentPK";
        public const string SaveMode = "SaveMode";
        public const string CategoryPK = "CategoryPK";
        public const string AttachmentFileName = "AttachmentFileName";
        public const string ReUseCondnPK = "ReUseCondnPK";
        public const string IntransitAssetCount = "IntransitAssetCount";

        //For Alerts page and Attachments page
        public const string Module = "Module";
        public const string ModuleRef = "ModuleRef";
        public const string Sequence = "Sequence";
        public const string UserPK = "UserPK";
        public const string DeptPK = "DeptPK";
        public const string BizUnitPK = "BizUnitPK";
        public const string ModuleRefCode = "ModuleRefCode";
        public const string ModuleRefName = "ModuleRefName";
        public const string ModuleRefCodeText = "ModuleRefCodeText";
        public const string ModuleRefNameText = "ModuleRefNameText";
       


        //For Asset Statutory Details Page
        public const string ResponsibilityPK = "ResponsibilityPK";
        public const string SatTypePK = "SatTypePK";
        public const string AssetPK = "AssetPK";

        //For Asset Warranty Details Page
        public const string WatTypePK = "WatTypePK";
        public const string ServiceProvidorPK = "ServiceProvidorPK";
        public const string AutoGeneratedCode = "AutoGeneratedCode";

        //For Asset Search
        public const string AssociateAsset = "AssociateAsset";
        public const string IsPopup = "IsPopup";

        public const string CurrentTab = "CurrentTab";
        public const string MainTabEnum = "MainTabEnum";

        public const string AssetMntHdrPK = "AssetMntHdrPK";
        public const string ResourceTypePK = "ResourceTypePK";

        public const string AssetAcvtyHdrPK = "AssetAcvtyHdrPK";
        public const string AssetName = "AssetName";

        //For Asset Maintenance Checklist
        public const string SortByFrom = "SortByFrom";
        public const string SortDirectionFrom = "SortDirectionFrom";
        public const string SortByTo = "SortByTo";
        public const string SortDirectionTo = "SortDirectionTo";
        public const string MntChecklistPks = "MntChecklistPks";

        //For Item Master
        public const string MaterialTypePK = "MaterialTypePK";

        //For Project
        public const string ProjectPK = "ProjectPK";
        public const string ProjectCode = "ProjectCode";
        public const string ProjectName = "ProjectName";

        //For Project Activity
        public const string RecurrencePK = "RecurrencePK";
        public const string FreequencyPK = "FreequencyPK";

        //For Dashboard
        public const string CurrPage = "CurrPage";
        public const string CurrRow = "CurrRow";
        public const string CurrCell = "CurrCell";
        public const string CurrChartType = "CurrChartType";
        public const string CurrParam = "CurrParam";
        public const string ParamsTotalPages = "ParamsTotalPages";
        public const string ParamsSortBy = "ParamsSortBy";
        public const string ParamsSortDirection = "ParamsSortDirection";
        public const string ParamsPageIndex = "ParamsPageIndex";
        public const string PripertyLastModifiedTime = "PripertyLastModifiedTime";
        public const string CurrFilter = "CurrFilter";
        public const string ZoomPK = "ZoomPK";

        //For Transfer Schedule
        public const string TransferType = "TransferType";
        public const string OutwardPK = "OutwardPK";
        public const string SavedAssetPK = "SavedAssetPK";
        public const string AssetTypePK = "AssetTypePK";
        public const string ScheduleStatus = "ScheduleStatus";

        // Schedule & Assignment
        public const string PlanPageIndex = "PlanPageIndex";
        public const string ActivityTypePK = "ActivityTypePK";
        public const string EmpFromDate = "EmpFromDate";
        public const string EmpToDate = "EmpToDate";
        public const string EmpRecuurToDate = "EmpRecuurToDate";
        public const string EmpRecuurFromDate = "EmpRecuurFromDate";
        public const string ActivityFromDate = "ActivityFromDate";
        public const string ActivityToDate = "ActivityToDate";
        public const string ActivityRecuurToDate = "ActivityRecuurToDate";
        public const string ActivityRecuurFromDate = "ActivityRecuurFromDate";
        public const string FromDateActivity = "FromDateActivity";
        public const string ToDateActivity = "ToDateActivity";
        public const string FromDateAsset = "FromDateAsset";
        public const string ToDateAsset = "ToDateAsset";
        public const string ActivityHdrPK = "ActivityHdrPK";
        public const string SchAssetTypePK = "AssetTypePK";
        public const string ActivityPK = "ActivityPK";
        public const string ActivityRecurrencePK = "ActivityRecurrencePK";
        public const string AssetEmployeePK = "AssetEmployeePK";
        public const string SaaPK = "saaPK";
        public const string AssetActivityHdrPK = "AssetActivityHdrPK";
        public const string AssetRecurrencePK = "AssetRecurrencePK";
        public const string ProjectTaskPlanTotalPages = "ProjectTaskPlanTotalPages";

        public const string SelectedAdmReccurRuleMstList = "SelectedAdmReccurRuleMstList";
        public const string TransferAssets = "TransferAssets";

        //For Assets Due
        public const string DisplayType = "DisplayType";
        public const string LocationPK = "LocationPK";


        public static string InwardNotePK = "InwardNotePK";
        public static string DsFlag = "DsFlag";

        public static string LastModifiedTimeDtl = "LastModifiedTimeDtl";
        public static string DsdPK = "DsdPK";

        //For Utilization Report
        public static string FilterParams = "FilterParams";
        public static string CurrentAction = "CurrentAction";

        //For Outward note
        public static string AsoTrfScheduleHdrPrint = "AsoTrfScheduleHdrPrint";
        public static string OutwardNoteTime = "OutwardNoteTime";

    }

    public class RequestParameters
    {
        public const string SearchValue = "SearchValue";
        public const string SearchType = "SearchType";
        public const string Make = "Make";
        public const string Type = "Type";
        public const string Project = "Project";
        public const string Group = "Group";
    }

    public class PageLevelStrings
    {
        public const string StartTime = " 00:00:00 AM";
        public const string EndTime = " 11:59:59 PM";
        public const string Err_EnterAssetFromDate = "Err_EnterAssetFromDate";
        public const string Err_EnterAssetToDate = "Err_EnterAssetToDate";
        public const string Err_EnterAssetFromTime = "Err_EnterAssetFromTime";
        public const string Err_EnterAssetToTime = "Err_EnterAssetToTime";
        public const string Err_InvalidDuration = "Err_InvalidDuration";
        public const string Err_SelectOneAsset = "Err_SelectOneAsset";
        public const string Err_SelectOneEmployee = "Err_SelectOneEmployee";

        public const string Err_MaintenanceType = "Err_MaintenanceType";
        public const string Err_YearofManufact = "Err_YearofManufact";
        public const string Err_PurchaseDate = "Err_PurchaseDate";
        public const string AssetTypeCode = "AssetTypeCode";
        public const string AssetTypeName = "AssetTypeName";

        public const string VendorCode = "VendorCode";
        public const string VendorName = "VendorName";

        public const string Err_Class = "Err_Class";
        public const string Err_Type = "Err_Type";
        public const string Err_GreaterThanToday = "Err_GreaterThanToday";
        public const string Err_Dealer = "Err_Dealer";
        public const string Err_Make = "Err_Make";
        public const string Err_Model = "Err_Model";
        public const string Err_Department = "Err_Department";
        public const string Err_Owner = "Err_Owner";
        public const string Err_Custodian = "Err_Custodian";
        public const string Err_Location = "Err_Location";
        public const string Err_Project = "Err_Project";
        public const string Err_Reason = "Err_Reason";

        public const string Err_Responsibility = "Err_Project";
        public const string Err_SP = "Err_SP";

        public const string Dealer = "Dealer";
        public const string Make = "OEM/Make";
        public const string DeAssociate = "DeAssociate";
        public const string PartsSubparts = "PartsSubparts";
        public const string grdListing = "grdListing";
        public const string grdParts = "grdParts";
        public const string grdDeassociatedParts = "grdDeassociatedParts";
        public const string Err_Reorder_Less = "Err_Reorder_Less";
        public const string Err_MinStock_Less = "Err_MinStock_Less";
        public const string LineBreak = "<br/>";

        public const short VendorDealer = 2;
        public const short VendorMake = 1;

        // Cost and other Info
        public const string Err_InspectionGrt = "Err_InspectionGrt";

        public const string Err_EffectiveUseGrt = "Err_EffectiveUseGrt";

        //KB Details

        public const string KBCode = "KBCode";
        public const string KBName = "KBName";
        public const string Err_KBType = "Err_KBType";

        public const string Err_ValueParameter = "Err_ValueParameter";
        public const string Err_UOM = "Err_UOM";
        public const string ParameterName = "ParameterName";

        public const string tabactive = "tab-active";
        public const string tabinactive = "tab-inactive";


        // breadcrumb Attachment or alert
        public const string BreadcrumbStatutoryDetail = "BreadcrumbStatutoryDetail";
        public const string BreadcrumbKBDetail = "BreadcrumbKBDetail";
        public const string BreadcrumbWarrantyDetail = "BreadcrumbWarrantyDetail";
        public const string BreadcrumbMntDetail = "BreadcrumbMntDetail";

        //For Maintenance costing
        public const string Err_Role = "Err_Role";
        public const string Err_DurationUom = "Err_DurationUom";
        public const string Err_Category = "Err_Category";
        public const string Err_Item = "Err_Item";
        public const string Err_QuantityUom = "Err_QuantityUom";
        public const string Err_Tool = "Err_Tool";
        public const string Err_Quantity_Zero = "Err_Quantity_Zero";
        public const string Err_Nos_Zero = "Err_Nos_Zero";
        public const string Err_Duration_Zero = "Err_Duration_Zero";


        public const string lblCode = "lblCode";
        public const string lblName = "lblName";

        //For Maintenance Checklist
        public const string chkSelectFrom = "chkSelectFrom";
        public const string chkSelectTo = "chkSelectTo";

        // For 3 radio buttons in Asset maintenance costing 
        public const string rbtSelectHR = "rbtSelectHR";
        public const string rbtSelectM = "rbtSelectM";
        public const string rbtSelectTM = "rbtSelectTM";

        //For Vendor Contract
        public const string Err_Currency = "Err_Currency";
        public const string Err_Uom = "Err_Uom";
        public const string VirtualDirectory = "VirtualDirectory";

        //For Asset Activity
        public const string ActCode = "ActCode";
        public const string ErrorOBUOM = "Err_OBUOM";
        public const string ErrorDUUOM = "Err_DUUOM";
        public const string ErrorOQUOM = "Err_OQUOM";
        public const string ErrorActivityType = "Err_ActivityType";

        public const string ErrorChecklist = "Err_Checklist";

        //For Project Activity

        public const string ReccModule = "tai";

        public const string ReccFreqModule = "amf";        
    }
}
