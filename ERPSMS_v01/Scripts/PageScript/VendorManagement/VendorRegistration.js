materialID/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region GlobalVariables
var vendorlID = 0;
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Vendor";
var tdset = "";
var addressID = 0;
var materialID = 0;
var sampleMaterialID = 0;
var tdset = "";
var tdsetSamples = "";
var bankID = 0;
var typeChanged = false;
var LocalAddressID = 0;
var Vendor_Name = "";

//var FileJson = new Object(); 
///#endregion

///#region Configuration Section
var vendorRegistration = {
    GetCountry: "CommonManagement.do?Action=GetCountryList",
    GetAccount: "CommonManagement.do?Action=GetAccount",
    GetVendorPOType: "VendorRegistration.do?Action=GetVendorPOType&BizUnit=",
    GetState: "CommonManagement.do?Action=GetStateList",
    GetTax: "TaxSettings.do?Action=GetTaxCategoryValue",
    GetCurrency: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    GetMaterialCategoryTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryWithoutSemiAndFinished&SBUPk=",
    GetVendorTerms: "VendorTermsManagement.do?Action=GetVenderTermsList",
    SaveVendorDetails: "VendorRegistration.do?Action=SaveVendorDetails",
    SaveTaxDiscount: "VendorRegistration.do?Action=TaxDiscountApply",
    GetVndrTaxDiscountDetails: "VendorRegistration.do?Action=GetTaxDiscountDetails&ItemPk=",
    GetVendorRoles: "VendorRegistration.do?Action=GetRole&SBU=",
    DeleteVenMaterial: "VendorRegistration.do?Action=DeleteVndMaterial&MaterialID=",
    GetVendorMaterials: "VendorRegistration.do?Action=GetVndMaterials&VendorID=",
    SaveMaterial: "VendorRegistration.do?Action=SaveVenMaterial",
    GetCategoryTaxDiscountDateBase: "TaxSettings.do?Action=GetActiveCategoryDateValue&CategoryPK=",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListWithoutSemiAndFinished&SBUPk=",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&AUTOSEARCH=1",
    //    GetMaterialCategoryListExceptFG
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialCodeNameByCategory&AUTOSEARCH=1&SBUPk=",
    GetMaterialUOM: "MaterialManagement.do?Action=GetMaterialUOM",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetCategoryTaxDiscount: "TaxSettings.do?Action=GetActiveCategoryValue&CategoryPK=",
    VendorListing: "Vendorlisting.aspx",
    Category: "&CategoryPK=1",
    SubCategory: "&SubCategoryPK=3",
    //New
    GetAddressTypeList: "VendorRegistration.do?Action=GetAddressTypeList&SBUPk=",
    //New End
    //NewMaterial Start
    GetRateHistory: "MaterialManagement.do?Action=GetRateHistory&SBUPk=",
    //New End

    SaveBank: "VendorRegistration.do?Action=SaveVenBank",
    SaveLocalAddress: "VendorRegistration.do?Action=SaveVenLocalAddress",
    GetVendorLocalAddress: "VendorRegistration.do?Action=GetVendorLocalAddress&VendorID=",
    GetVendorLocalAddressById: "VendorRegistration.do?Action=GetVendorLocalAddressById&VendorID=",
    GetAccountTypes: "VendorRegistration.do?Action=GetAccountTypesBank",
    GetVendorBanks: "VendorRegistration.do?Action=GetVndBanks&VendorID=",
    GetBankDetailsById: "VendorRegistration.do?Action=GetBankDetaislById&VendorID=",
    DeleteVenBank: "VendorRegistration.do?Action=DeleteVndBank&P_VBD_PK=",

    DeleteVenLocalAddress: "VendorRegistration.do?Action=DeleteVenLocalAddress&P_VNC_LC_PK=",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    VendorNameAutoCompleteURL: "VendorRegistration.do?Action=GetVendorAutoSearch&AUTOSEARCH=1&SBUPk=",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    GetVendorGSTType: "CommonManagement.do?Action=GetAppConfig&CfgValue=",

    //Constants
    INBOX: "INBOX",
    ViewCommand: "VIEW",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    HistoryCommand: "HISTORY",
    SaveCommand: "SAVE",
    CodeExist: "CODEEXIST",
    Param: "&MatCagID=",
    DeleteAddress: "DELETEADDRESS",
    DeleteMaterial: "DELETEMATERIAL",
    SEMIFINISHEDGOOD: "SEMI FINISHED GOOD",
    FINISHEDGOOD: "FINISHED GOOD",
    EDITLOCALADDRESS: "EDITLOCALADDRESS",
    DELETELOCALADDRESS: "DELETELOCALADDRESS",
    VIEWLOCALADDRESS: "VIEWLOCALADDRESS",
    //NewSamples Start
    DeleteSamples: "DELETESAMPLES",
    //New End
    TaxDelete: "TAXDELETE",
    TaxDeleteVendor: "TAXDELETEVENDOR",
    DeleteBank: "DELETEBANK",
    DeleteLcAddress:"DeleteLcAddress",

    //Fields
    AddressID: "AddressID",
    VNC_NAME: "VNC_NAME",
    VNC_ADDR1: "VNC_ADDR1",
    VNC_ADDR2: "VNC_ADDR2",
    VNC_EMAIL: "VNC_EMAIL",
    VNC_CITY: "VNC_CITY",
    VNC_DEFAULT: "VNC_DEFAULT",
    VNC_CNTRY: "VNC_CNTRY",
    VNC_STATE: "VNC_STATE",
    VNC_MOBIL: "VNC_MOBIL",
    VNC_FAX: "VNC_FAX",
    VNC_TAX_NO: "VNC_TAX_NO",
    VEN_IS_PO_EXIST:"VEN_IS_PO_EXIST",
    //New
    VNC_TYPE: "VNC_TYPE",
    VNC_TYPE_NAME: "VNC_TYPE_NAME",
    //New End
    VNC_CONT_NAME: "VNC_CONT_NAME",
    VNC_PHONE: "VNC_PHONE",
    VendorTermsID: "VET_PK",
    VendorTermsTitle: "VET_TITLE",
    VendorTermsType: "VET_TYPE",
    VendorMaterialID: "VendorMaterialID",
    MaterialType: "MaterialType",
    ITV_ITEM: "ITV_ITEM",
    MatreialITV_MOQ_UOM: "ITV_MOQ_UOM",
    ITV_CURRENCY: "ITV_CURRENCY",
    MaterialCode: "ITV_ITEM",
    MaterialNameVendor: "ITV_NAME",
    MaterialITV_PRICE: "ITV_PRICE",
    MaterialITV_MOQ: "ITV_MOQ",
    ToDate: "VIH_EFCT_TO",
    //NewMaterial Start
    VendorDiscount: "ITV_DISC_PERC",
    ITV_LEAD_TIME: "ITV_LEAD_TIME",
    //New End
    //NewMaterial start
    //VendorTAX: "ITV_TAX_PERC",
    //New End
    //NewSamples Start
    ISV_ITEM: "ISV_ITEM",
    MaterialTypeSamples: "MaterialTypeSamples",
    SampleMaterialCode: "ISV_ITEM",
    ISV_RECEIVED_DATE: "ISV_RECEIVED_DATE",
    ISV_QC_TEST: "ISV_QC_TEST",
    ISV_QC_VALUE: "ISV_QC_VALUE",
    ISV_REMARKS: "ISV_REMARKS",
    ISV_QUANTITY: "ISV_QUANTITY",
    ISV_REFERENCE: "ISV_REFERENCE",
    ISV_STATUS: "ISV_STATUS",
    ISV_STATUS_TEXT: "ISV_STATUS_TEXT",
    //New End

    //Fields for local address
    VNC_LC_TITTLE: "VNC_LC_TITTLE",
    VNC_LC_LASTNAME: "VNC_LC_LASTNAME",
    VNC_LC_ADDR2: "VNC_LC_ADDR2",
    VNC_LC_CNTRY: "VNC_LC_CNTRY",
    VNC_LC_TAX_NO: "VNC_LC_TAX_NO",
    VNC_LC_NAME: "VNC_LC_NAME",
    VNC_LC_ADDR1: "VNC_LC_ADDR1",
    VNC_LC_ADDR3: "VNC_LC_ADDR3",
    VNC_LC_POSTAL: "VNC_LC_POSTAL",
    VNC_LC_BRANCH: "VNC_LC_BRANCH",



    IVT_SL_NO: "IVT_SL_NO",
    IVT_TAX: "IVT_TAX",
    ITV_DISC_PERC: "ITV_DISC_PERC",
    ITV_TAX_PERC: "ITV_TAX_PERC",
    ITV_SL_NO: "ITV_SL_NO",
    CfgType: "PURCHASE TYPE",
    CfgGstType: "VENDOR GST TYPE",
    Local: "2",
    Import: "1",

    //Properties
    ItemTaxPK: 0,
    EditTax: 0,
    VendorTaxPK: 0,

    //Messages
    VendorCodeExist: "Translate(VendroCodeExist)",
    UsedMaterialInAnotherPlace: "Translate(UsedMaterialInAnotherPlace)",
    VendorSaveMessage: "Translate(VendorSavedMessage)",
    VendorSubmitMessage: "Translate(VendorSubmitMessage)",
    VendorSavedMessage1: "Translate(VendorSavedMessage1)",
    VendorSavedMessage2: "Translate(VendorSavedMessage2)",
    VendorSubmitMessage2: "Translate(VendorSubmitMessage2)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    Information: "Translate(Information)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    TypeAlreadyAdded: "Translate(TypeAlreadyAdded)"
}
///#endregion

var QtyDec, AmtDec, RateDec;

//#region initialization Section
$.validator.addMethod('selectNone', function (value, element) {
    ///<summary>
    ///Add additional validation for select
    ///</summary>
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$.validator.addMethod("TwoDecimal", function (value) {
    return /^\d{1,2}(\.\d{1,2})?$/.test(value);
}, "Max 2 numeric and 2 decimals allowed");

$(document).ready(function () {
    ///<summary>
    ///Document Ready function Aftre page initialization
    ///</summary>
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0" && $(element).val() != null);
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "" && $(element).val() != "Select/Type");
    }, "Translate(Pleaseselectanoption)");

    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimal']").val();
    var pageURL = window.document.URL;
    //    var virtualPath = $("[id$=hdfVirtualPath]").val();
    //    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    //    if (url.indexOf("?") != -1) {
    //       // GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "&AccType=1", "VEN_ACCOUNT", true, true, "ACCOUNTMST");
    //        GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "&AccType=1", "VEN_ACCOUNT", true, true, "ACCOUNTMST");
    //    }
    //    else {
    //        GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "?AccType=1", "VEN_ACCOUNT", true, true, "ACCOUNTMST");

    //    }
    PageInit(); //Page Initial condtions
});

//function SetAutoComplete() {
//    var pageURL = window.document.URL;
//    var virtualPath = $("[id$=hdfVirtualPath]").val();
//    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
//    if (url.indexOf("?") != -1) {
//        // GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "&AccType=1", "VEN_ACCOUNT", true, true, "ACCOUNTMST");
//        GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "&AccType=11", "VEN_ACCOUNT", true, true, "ACCOUNTMST");
//    }
//    else {
//        GrandScriptUtils.MakeAutoCompleteDDL("COA_TEXT", url + "?AccType=1", "VEN_ACCOUNT", true, true, "ACCOUNTMST");

//    }
//}
function PageInit() {
    ///<summary>
    ///Used For PageInit
    ///</summary>
    WindowExpand(true); //Hide Side Menu
    $('#VEN_ADDR3').hide();
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var qstrings = queryStr.split("&")
        for (var i = 0; i < qstrings.length; i++) {
            var pK = qstrings[i].split("=");
            if (pK[1] != "" && pK[0] == "Status" || (pK[1] != "" && pK[0] == "RefID")) {
                if (pK[0] == "RefID") {
                    // $("[id$=btnSave]").hide();
                    // $("[id$=imbSave]").hide();
                    // $("[id$=AddressSave]").hide();
                    //                    $("[id$=divdummyMaterial]").hide();
                    //                    $("[id$=divdummySamples]").hide();
                    $("[id$=divdummySamples]").show();
                    //New 07-01-2014
                    // $("[id$=btnBack]").hide();
                    // $("[id$=imbaddnew]").hide();
                    // $("[id$=imbSamplesAdd]").hide();

                }
                if (pK[1] == "1") {
                    $("[id$=btnSave]").hide();
                    $("[id$=imbSave]").hide();
                    $("[id$=ViewStatus]").val("1");
                    $("[id$=AddressSave]").hide();
                    $("[id$=btnSaveLocalAddress]").hide();
                    $("[id$=divdummyMaterial]").hide();
                    $("[id$=btnBack]").hide();
                    $("[id$=btnLocalAddressClear]").hide();
                    // $("[id$=imbaddnew]").hide(); 
                    //NewSamples Start
                    //                    $("[id$=divdummySamples]").hide();
                    $("[id$=imbSamplesAdd]").hide();

                    //New End
                    $("textarea[id='ctl00_MainContent_VEN_ADDR3']").remove();
                    $('#VEN_ADDR3').show();
                }
                if (pK[1] == "2") {
                    $("[id$=btnSave]").show();
                    $("[id$=imbSave]").hide();
                }
            }
        }
    }

    //    FillRoleTreeView(0)
    $("[id$=tabs]").tabs(); // Create Tabs
    vendorJson = $.parseJSON($("[id$=VendorDetails]").val());
    $("#divVendorData").data("VendorData", vendorJson);
    $("#divCategory").dialog({ autoOpen: false });
    FillAddressType(0);
    FillMaterialCategory();
    FillCategoryMaterials(0);
    FillMaterialSamplesCategory();
    DateInit();
    //FillTax(0);
    //    FillAccount(0);
    BindVendorTerms();
    if (vendorJson.VEN_PK == undefined || vendorJson.VEN_PK == 0) {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", true);
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdVendorAddressDetails"), 0, dummyObj);
        GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, dummyObj);
        //NewSamples Start
        GrandGrid.MakeGrid($("#grdMaterialSampleDetails"), 0, dummyObj);
        //New End
        FillDropDowns();
        FillRoleTreeView(0);
        $("[id$=VEN_CURRENCY_TEXT]").html("");
        $("[id$=VEN_CNTRY_TEXT]").html("");
        $("[id$=VEN_STATE_TEXT]").html("");
        $("[id$=VEN_GST_TYPE_TEXT]").html("");
        $("[id$=VEN_CURRENCY_TEXT]").hide("");
        $("[id$=VEN_CNTRY_TEXT]").hide("");
        $("[id$=VEN_STATE_TEXT]").hide("");
        $("[id$=VEN_GST_TYPE_TEXT]").hide("");
        $(".ddlSelect").show();
        if ($("[id$=VEN_ACTIVE1]").attr("checked") == "1") {
            $("[id$=divCommentsText]").hide();
            $("[id$=divComments]").hide();
            $("[id$=VEN_COMMENTS]").html('');
        }
        else {
            $("[id$=divCommentsText]").hide();
            $("[id$=divComments]").show();
        }
        if ($("[id$=VENCODE_AUTO]").val() == "1") {
            $("[id$=VEN_CODE]").val(vendorRegistration.DocGenerationNewValue);
        }
    }
    else {
        FillVendorDetails(vendorJson);
    }
    $("input[id$=UserID]").val($("input[id$=UserPk]").val());
    $("input[id$=VEN_NAME]").focus();
    $("select[id$=VEN_TYPE]").attr("disabled", true);
    if ($("[id$=ViewStatus]").val() == "1") {
        GrandScriptUtils.ChangeMode("FormData");
    }
    $("[id$=imbaddnew]").show();

    bindBankTab();
    BindGridBanks();
    BindGridLocalAddress();
    FillVendorNameAutoComplete();


    if ($("[id$=hdfIsGstEnabled]").val() == "0") {
        $("[id$=divGstTypeDdl]").hide();
        $("[id$=divGstTypeText]").hide();
    }
    //Show/Hide ESI no & PF No Based on config
    if ($("[id$=hdfIsShowESINo]").val() == "1") {
        $("[id$=divESINo").show();
    }
    if ($("[id$=hdfIsShowPFNo]").val() == "1") {
        $("[id$=divPFNo").show();
    }
    
    return false;
}

function FillVendorNameAutoComplete() {
    ///<summary>To handle auto complete</summary>   
    GrandScriptUtils.MakeAutoComplete("VEN_NAME", vendorRegistration.VendorNameAutoCompleteURL + $("[id$=BizUnitPk]").val() + "&SearchType=VEN_NAME", false);
}
function FillTax(taxID) {
    var drpID = $("select[id$=VEN_WHT_TAX]").attr("id");
    $.get(vendorRegistration.GetTax + vendorRegistration.Category + vendorRegistration.SubCategory, function (data) {
        if (taxID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, taxID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });

}
function FillCategoryMaterials(catgID) {
    ///<summary>Function Used to Fill material based on the category  </summary>
    $("[id$=MaterialItem]").val("");
    $("[id$=ITV_ITEM]").val(0);
    // 
    //GrandScriptUtils.MakeAutoCompleteLimitLen("MaterialItem", vendorRegistration.MaterialURL, "ITV_ITEM", true, false, "MaterialType", true, "Store", "", "", $("[id$=AutoStartValue]").val());

    GrandScriptUtils.MakeAutoCompleteLimitLen("MaterialItem", vendorRegistration.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + catgID, "ITV_ITEM", true, false, "MaterialType", true, "Store", "", "", $("[id$=AutoStartValue]").val());
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "MaterialItem") {
        FillMaterialDetails($("[id$=ITV_ITEM]").val());
    }
}

function FillRoleTreeView(venPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    var sbuPK = $("[id$=BizUnitPk]").val();
    SetTreeHeaderStructure("trvRoleMap", vendorRegistration.GetVendorRoles + sbuPK + "&venPK=" + venPk + "&MapParentID=0", "Role", true, false, "0", true);    // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

function GetSelectedMaterial() {
    //<summary>Function Used to get the all checked dept details </summary>
    var MaterialArray = new Array();
    var sbuPK = $("[id$=BizUnitPk]").val();
    var store = $("[id$=Store]").val();
    var userPK = $("[id$=UserPk]").val();
    var materialMapping
    var materialPK = 0;
    $("#trvMaterialMap").find("input[type=checkbox]:checked").each(function () {
        materialPK = $(this).attr("id");
        if ($(this).next().next("input[type=hidden]").val() == "true") {
            materialPK = materialPK.substr(materialPK.lastIndexOf("_") + 1, materialPK.length);
            MaterialArray.push({ IDM_ITEM: materialPK, IDM_DEPT: store, IDM_MOD_BY: userPK, IDM_BIZUNIT: sbuPK });
        }
    });
    return MaterialArray;
}

function GetSelectedRoles() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ObjVendorRole = new Array();
    var obj = new Object();
    $("#trvRoleMap").find("input[type=checkbox]:checked").each(function () {
        rolePK = $(this).attr("id");
        obj = new Object();
        rolePK = rolePK.substr(rolePK.lastIndexOf("_") + 1, rolePK.length);
        obj.VRM_ROLE = rolePK;
        ObjVendorRole.push(obj);
    });
    return ObjVendorRole;
}

function GetVendorTerms() {
    var ObjVendor = $("#divVendorData").data("VendorData");
    var obj = new Object();
    var flag = false;
    $("#grdTermsDetails").find("tr:has(td)").each(function () {
        var grdcbx = $(this).find("td:first input");
        var grdinput = $(this).find("td:last input");
        var TermID = GrandGrid.Utilities.GetColumnValue(this, vendorRegistration.VendorTermsID, $(this).parent().attr("id"));
        if ($(grdcbx).attr("checked")) {
            obj = new Object();
            flag = false;
            for (var i in ObjVendor.TermsDetails) {
                if (TermID == ObjVendor.TermsDetails[i].VTD_VENDER_TERM) {
                    flag = true;
                    obj = ObjVendor.TermsDetails[i];
                }
            }
            if (flag)
                obj.VTD_VALUE = $(grdinput).val();
            else {
                obj.VTD_VENDER_TERM = TermID;
                obj.VTD_VALUE = $(grdinput).val();
                ObjVendor.TermsDetails.push(obj);
            }
        }
        else {
            for (var i in ObjVendor.TermsDetails) {
                if (ObjVendor.TermsDetails[i].VTD_VENDER_TERM == TermID) {
                    ObjVendor.TermsDetails.splice(i, 1);
                    break;
                }
            }
        }
    });
    $("#divVendorData").data("VendorData", ObjVendor);
    return false;
}


//NewSample Start
function DateInit() {
    //<summary>function used to make datepicker</summary>
    GrandScriptUtils.DatePicker("ISV_RECEIVED_DATE", false, false);
    GrandScriptUtils.DatePicker("VEN_GST_NO_MOD_DT", false, false);
    GrandScriptUtils.DatePicker("VEN_DATE", false, false);
}
//New End

function MakeNumeric(event) {
    ///<summary>function used to make text box Numeric only</summary>
    /// <param name= "event"  type="Object">
    /// Object Used to get the Key Pressed
    /// </param>
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}

$("[id$='VEN_ACTIVE1']").live("change", function () {
    if ($("[id$=VEN_ACTIVE1]").attr("checked") == "1") {
        $("[id$=divCommentsText]").hide();
        $("[id$=divComments]").hide();
        $("[id$=VEN_COMMENTS]").html('');
    }
    else {
        $("[id$=divCommentsText]").hide();
        $("[id$=divComments]").show();
    }
});

function FillVendorDetails(vendorObject) {
    ///<summary>function used to fill vendor details corresponding to vendor id </summary>
    /// <param name="vendorObject"  type="Object">
    /// vendor object fetched corresponding to vendorID
    /// </param>
    var objArray;
    if (parseInt(vendorObject.VEN_STATUS) == 0 || parseInt(vendorObject.VEN_STATUS) == 6) {
        if ($("[id$=ViewStatus]").val() != "1") {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", false);
        }
    }
    else {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Vendor", false); 0
    }
    $("[id$=VEN_NAME]").val(vendorObject.VEN_NAME);
    //    if ($("[id$=VENCODE_AUTO]").val() == "1" && (vendorObject.VEN_CODE == "" || vendorObject.VEN_CODE == undefined)) {
    //        $("[id$=VEN_CODE]").val(vendorRegistration.DocGenerationNewValue);
    //    } 
    $("[id$=VEN_CODE]").val(vendorObject.VEN_CODE);
    $("[id$=VEN_CONT_NAME]").val(vendorObject.VEN_CONT_NAME);
    Vendor_Name = vendorObject.VEN_NAME;
    //disable PO existing customers
    if (vendorObject.VEN_IS_PO_EXIST == "1") {
        $("[id$=VEN_CODE]").attr("disabled", "disabled");
        $("[id$=VEN_NAME]").attr("disabled", "disabled");
        $("[id$=VEN_CODE]").addClass('input-disabled');
        $("[id$=VEN_NAME]").addClass('input-disabled');
    }
    else {
        $("[id$=VEN_CODE]").removeAttr("disabled");
        $("[id$=VEN_NAME]").removeAttr("disabled");
    }

    $("[id$=VEN_TIN]").val(vendorObject.VEN_TIN);
    $("[id$=VEN_PHONE]").val(vendorObject.VEN_PHONE);
    $("[id$=VEN_ADDR1]").val(vendorObject.VEN_ADDR1);
    $("[id$=VEN_PIN]").val(vendorObject.VEN_PIN);
    $("[id$=VEN_ADDR2]").val(vendorObject.VEN_ADDR2);
    $("[id$=VEN_EMAIL]").val(vendorObject.VEN_EMAIL);
    $("[id$=VEN_MOBIL]").val(vendorObject.VEN_MOBIL);
    $("[id$=VEN_CITY]").val(vendorObject.VEN_CITY);
    $("[id$=VEN_FAX]").val(vendorObject.VEN_FAX);
    $("[id$=VEN_NAME2]").val(vendorObject.VEN_NAME2);
    $("[id$=VEN_GST_NO]").val(vendorObject.VEN_GST_NO);
    $("[id$=VEN_ADDR3]").val(vendorObject.VEN_ADDR3);
    $('#VEN_ADDR3').html(vendorObject.VEN_ADDR3);
    $("[id$=VEN_COMMENTS]").val(vendorObject.VEN_COMMENTS);
    $("[id$=VEN_COMMENTSTEXT]").html(vendorObject.VEN_COMMENTS);
    $("[id$=VEN_REG_NO]").val(vendorObject.VEN_REG_NO);
    $("[id$=VEN_COMMISSION]").val(parseFloat(vendorObject.VEN_COMMISSION).toFixed(RateDec)); //val(vendorObject.VEN_COMMISSION);
    $("[id$=VEN_GST_NO_MOD_DT]").val('');
    if (vendorObject.VEN_GST_NO_MOD_DT) {  // != 'undefined')
        $("[id$=VEN_GST_NO_MOD_DT]").val(convertDate(vendorObject.VEN_GST_NO_MOD_DT));
    }
    $("[id$=VEN_DATE]").val('');
    if (vendorObject.VEN_DATE) {  // != 'undefined')
        $("[id$=VEN_DATE]").val(convertDate(vendorObject.VEN_DATE));
    }
    $("[id$=VEN_WEBSITE]").val(vendorObject.VEN_WEBSITE);
    $("[id$=VEN_PAN]").val(vendorObject.VEN_PAN);

    $("[id$=VEN_ESI_NO]").val(vendorObject.VEN_ESI_NO);
    $("[id$=VEN_PF_NO]").val(vendorObject.VEN_PF_NO);

    //new
    //$("[id$=VEN_SERVICE_PROVIDED]").val(vendorObject.VEN_SERVICE_PROVIDED);
    //    $("[id$=VEN_ANNUAL_SALES]").val(vendorObject.VEN_ANNUAL_SALES);
    //To prevent 0 to annual sale text, otherwise generate validation error occure
    var varCreditDays = parseFloat(vendorObject.VEN_CREDIT_DAYS).toFixed(3);
    varCreditDays = parseFloat(varCreditDays);
    if (varCreditDays != 0)
        $("[id$=VEN_CREDIT_DAYS]").val(varCreditDays);

    var varAnnualValue = parseFloat(vendorObject.VEN_ANNUAL_SALES).toFixed(3);
    varAnnualValue = parseFloat(varAnnualValue);
    if (varAnnualValue != 0)
        $("[id$=VEN_ANNUAL_SALES]").val(varAnnualValue);
    $("[id$=VEN_WAREHOUSE_DTL]").val(vendorObject.VEN_WAREHOUSE_DTL);
    $("[id$=VEN_MANAGER]").val(vendorObject.VEN_MANAGER);
    $("[id$=VEN_HAS_ISO]").val(vendorObject.VEN_HAS_ISO);
    if (vendorObject.VEN_HAS_ISO == "True" || vendorObject.VEN_HAS_ISO == "1") {
        $("[id$=VEN_HAS_ISO1]").attr("checked", true);
    } else {
        $("[id$=VEN_HAS_ISO1]").attr("checked", false);
    }

    $("[id$=VEN_PAY_FOR_VENDOR]").val(vendorObject.VEN_PAY_FOR_VENDOR);
    if (vendorObject.VEN_PAY_FOR_VENDOR == "True" || vendorObject.VEN_PAY_FOR_VENDOR == "1") {
        $("[id$=VEN_PAY_FOR_VENDOR1]").attr("checked", true);
        $("[id$=VEN_PAY_FOR_VENDOR_TEXT]").attr("checked", true);
    } else {
        $("[id$=VEN_PAY_FOR_VENDOR1]").attr("checked", false);
        $("[id$=VEN_PAY_FOR_VENDOR_TEXT]").attr("checked", false);
    }

    //Add VEN_ACTIVE
    $("[id$=VEN_ACTIVE]").val(vendorObject.VEN_ACTIVE);
    if (vendorObject.VEN_ACTIVE == "True" || vendorObject.VEN_ACTIVE == "1") {
        $("[id$=VEN_ACTIVE1]").attr("checked", true);
        $("[id$=VEN_ACTIVE]").attr("checked", true);

    } else {
        $("[id$=VEN_ACTIVE1]").attr("checked", false);
        $("[id$=VEN_ACTIVE]").attr("checked", false);
    }

    FillAccount(vendorObject.VEN_ACCOUNT);
    //    if (vendorObject.COA_TEXT != undefined) {
    //        $("[id$=COA_TEXT]").val(vendorObject.COA_TEXT);
    //        $("[id$=VEN_ACCOUNT]").val(vendorObject.VEN_ACCOUNT);
    //    }
    //    else {
    //        $("[id$=COA_TEXT]").val("Select/Type");
    //        $("[id$=VEN_ACCOUNT]").val("0");
    //    }
    FillAdvAccount(vendorObject.VEN_ADP_ACCOUNT);

    //Fill the country drop down and selecting the Country already added
    FillCountry("VENDOR", vendorObject.VEN_CNTRY);
    FillCountry("VBD_COUNTRY", vendorObject.VEN_CNTRY);
    $("[id$=VEN_TYPE]").val(vendorObject.VEN_TYPE);
    if ($("select[id$=VEN_CNTRY]").val() != "0")
        FillState("VENDOR", vendorObject.VEN_STATE, vendorObject.VEN_CNTRY);
    else
        $("select[id$=VEN_STATE]").find("option").remove();
    $("[id$=VEN_PK]").val(vendorObject.VEN_PK);
    FillType(vendorObject.VEN_PO_TYPE);
    FillGSTType(vendorObject.VEN_GST_TYPE);
    FillCurrency(vendorObject.VEN_CURRENCY);

    FillRoleTreeView(vendorObject.VEN_PK);
    FillCountry("ADDRESS");
    FillCountry("VNC_LC_CNTRY");
    FillTax(vendorObject.VEN_WHT_TAX);

    if (!($.isArray(vendorObject.AddressBookDetails))) {
        if (vendorObject.AddressBookDetails != undefined) {
            objArray = vendorObject.AddressBookDetails;
            vendorObject.AddressBookDetails = new Array();
            vendorObject.AddressBookDetails.push(objArray);
        }
        else {
            objArray = vendorObject.AddressBookDetails;
            vendorObject.AddressBookDetails = new Array();
        }
    }
    if (!($.isArray(vendorObject.MaterialDetails))) {

        if (vendorObject.MaterialDetails != undefined) {
            objArray = vendorObject.MaterialDetails;
            vendorObject.MaterialDetails = new Array();
            vendorObject.MaterialDetails.push(objArray);
        }
        else {
            objArray = vendorObject.MaterialDetails;
            vendorObject.MaterialDetails = new Array();
        }
    }
    if (!($.isArray(vendorObject.TaxHdr))) {

        if (vendorObject.TaxHdr != undefined) {
            objArray = vendorObject.TaxHdr;
            vendorObject.TaxHdr = new Array();
            vendorObject.TaxHdr.push(objArray);

            var ObjVendor = $("#divVendorData").data("VendorData");
            var ObjTax = new Object();
            ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
            ObjTax.USER_PK = ObjVendor.UserPk;
            ObjTax.ITV_PK = materialID;
            var category = parseInt($("[id$=hdfTaxCategory]").val());
            ObjTax.ITV_TAX_CATEGORY = category;
            ObjTax.TaxDetail = vendorObject.TaxHdr;

            $("#divTaxDataVendor").data("TaxObj", ObjTax);
            var v = JSON.stringify(ObjTax.TaxDetail);
            $("[id$=TaxHdr]").val(v);
            // Bind Vendor tax details to textbox 'TaxByVendor' 'IVT_TAX_TEXT'
            var taxText = '';
            for (i = 0; i < ObjTax.TaxDetail.length; i++) {
                taxText += ObjTax.TaxDetail[i].IVT_TAX_TEXT + ", ";
            }
            taxText = taxText.substring(0, taxText.length - 2);
            $("[id$=TaxByVendor]").attr('title', taxText);
            if (taxText.length > 35) {
                taxText = taxText.substring(0, 35);
                taxText += '...';
            }
            $("[id$=TaxByVendor]").text(taxText);
            if (taxText != '') {
                $("[id$=hdfIsVendorTax]").val('1');
            }
            GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, ObjTax.TaxDetail);
        }
        else {
            objArray = vendorObject.TaxHdr;
            vendorObject.TaxHdr = new Array();
            $("[id$=hdfIsVendorTax]").val('0');
            GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, new Array());
        }
    }
    else {
        var ObjVendor = $("#divVendorData").data("VendorData");
        var ObjTax = new Object();
        ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
        ObjTax.USER_PK = ObjVendor.UserPk;
        ObjTax.ITV_PK = materialID;
        var category = parseInt($("[id$=hdfTaxCategory]").val());
        ObjTax.ITV_TAX_CATEGORY = category;
        ObjTax.TaxDetail = vendorObject.TaxHdr;

        $("#divTaxDataVendor").data("TaxObj", ObjTax);
        var v = JSON.stringify(ObjTax.TaxDetail);
        $("[id$=TaxHdr]").val(v);

        // Bind Vendor tax details to textbox 'TaxByVendor' 'IVT_TAX_TEXT'
        var taxText = '';
        for (i = 0; i < ObjTax.TaxDetail.length; i++) {
            taxText += ObjTax.TaxDetail[i].IVT_TAX_TEXT + ", ";
        }
        taxText = taxText.substring(0, taxText.length - 2);
        $("[id$=TaxByVendor]").attr('title', taxText);
        if (taxText.length > 35) {
            taxText = taxText.substring(0, 35);
            taxText += '...';
        }
        $("[id$=TaxByVendor]").text(taxText);
        $("[id$=hdfIsVendorTax]").val('1')
    }
    var taxDetails = new Array();
    if (vendorObject.MaterialDetails.length > 0) {
        for (var itv in vendorObject.MaterialDetails) {
            if (!($.isArray(vendorObject.MaterialDetails[itv].TaxDtl))) {
                if (vendorObject.MaterialDetails[itv].TaxDtl != undefined) {
                    objArray = vendorObject.MaterialDetails[itv].TaxDtl;
                    vendorObject.MaterialDetails[itv].TaxDtl = new Array();
                    vendorObject.MaterialDetails[itv].TaxDtl.push(objArray);

                }
                else {
                    vendorObject.MaterialDetails[itv].TaxDtl = new Array();
                }
            }
            if (vendorObject.MaterialDetails[itv].TaxDtl.length > 0) {
                for (var txDtl in vendorObject.MaterialDetails[itv].TaxDtl)
                    taxDetails.push(vendorObject.MaterialDetails[itv].TaxDtl[txDtl]);

            }
        }

    }
    $("#divData").data("TaxDetails", taxDetails);
    if (!($.isArray(vendorObject.TermsDetails))) {
        if (vendorObject.TermsDetails != undefined) {
            objArray = vendorObject.TermsDetails;
            vendorObject.TermsDetails = new Array();
            vendorObject.TermsDetails.push(objArray);
        }
        else {
            objArray = vendorObject.TermsDetails;
            vendorObject.TermsDetails = new Array();
        }
    }
    //NewSamples Start
    if (!($.isArray(vendorObject.MaterialSamples))) {

        if (vendorObject.MaterialSamples != undefined) {
            objArray = vendorObject.MaterialSamples;
            vendorObject.MaterialSamples = new Array();
            vendorObject.MaterialSamples.push(objArray);
        }
        else {
            objArray = vendorObject.MaterialSamples;
            vendorObject.MaterialSamples = new Array();
        }
    }
    //New End
    if (!($.isArray(vendorObject.FILELIST))) {// Fill File Upload Details
        if (vendorObject.FILELIST != undefined) {
            objArray = vendorObject.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = vendorObject.FILELIST;
            FileJson.FILELIST = new Array();
        }
    }
    else {
        FileJson.FILELIST = vendorObject.FILELIST;
    }
    FillFileDetails();
    if ($("[id$=ViewStatus]").val() == "1") {
        $("[id$=VEN_STATE]").remove();
        ShowVendor();
        $("[id$=venCurrText]").show();
        $("[id$=venCurr]").hide();
        $("[id$=divTaxText]").show();
        $("[id$=divTaxddl]").hide();
        $("[id$=divTypeText]").show();
        $("[id$=divTypeddl]").hide();
        if ($("[id$=hdfIsGstEnabled]").val() == "1") {
            $("[id$=divGstTypeText]").show();
        }
        else {
            $("[id$=divGstTypeText]").hide();
        }
        $("[id$=divGstTypeDdl]").hide();
        $("[id$=divAccountText]").show();
        $("[id$=divAccountddl]").hide();
        if (vendorObject.VEN_ACTIVE == "True" || vendorObject.VEN_ACTIVE == "1") {
            $("[id$=divCommentsText]").hide();
            $("[id$=divComments]").hide();
            $("[id$=VEN_COMMENTS]").html('');
        }
        else {
            $("[id$=divCommentsText]").show();
            $("[id$=divComments]").hide();
        }

        $("[id$=VEN_CNTRY]").remove();
        $("[id$=VEN_CURRENCY_TEXT]").html(vendorObject.VEN_CURRENCY_TEXT);
        $("[id$=VEN_CNTRY_TEXT]").html(vendorObject.VEN_CNTRY_TEXT);
        $("[id$=VEN_STATE_TEXT]").html(vendorObject.VEN_STATE_TEXT);
        $("[id$=VEN_GST_TYPE_TEXT]").html(vendorObject.VEN_GST_TYPE_TEXT);
        $(".ddlSelect").hide();
    }
    else {
        $("[id$=divAccountText]").hide();
        $("[id$=divAccountddl]").show();
        $("[id$=divTypeText]").hide();
        $("[id$=divTypeddl]").show();
        $("[id$=divGstTypeText]").hide();
        if ($("[id$=hdfIsGstEnabled]").val() == "1") {
            $("[id$=divGstTypeDdl]").show();
        }
        else {
            $("[id$=divGstTypeDdl]").hide();
        }
        $("[id$=venCurrText]").hide();
        $("[id$=venCurr]").show();
        $("[id$=divTaxText]").hide();
        $("[id$=divTaxddl]").show();
        if (vendorObject.VEN_ACTIVE == "True" || vendorObject.VEN_ACTIVE == "1") {
            $("[id$=divCommentsText]").hide();
            $("[id$=divComments]").hide();
            $("[id$=VEN_COMMENTS]").html('');
        }
        else {
            $("[id$=divCommentsText]").hide();
            $("[id$=divComments]").show();
        }

        $("[id$=VEN_CURRENCY_TEXT]").html("");
        $("[id$=VEN_CNTRY_TEXT]").html("");
        $("[id$=VEN_STATE_TEXT]").html("");
        $("[id$=VEN_GST_TYPE_TEXT]").html("");
        $("[id$=VEN_CURRENCY_TEXT]").hide("");
        $("[id$=VEN_CNTRY_TEXT]").hide("");
        $("[id$=VEN_STATE_TEXT]").hide("");
        $("[id$=VEN_GST_TYPE_TEXT]").hide("");
        $(".ddlSelect").show();
    }
    if (vendorObject.COA_WHT_TAX_TEXT != undefined) {
        $("[id$=VEN_WHT_TAX_TEXT]").html(vendorObject.COA_WHT_TAX_TEXT);
    }
    GrandGrid.MakeGrid($("#grdVendorAddressDetails"), 0, vendorObject.AddressBookDetails);
    ClearAddress();
    // GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, vendorObject.MaterialDetails);
    ClearMaterials();
    //NewSamples Start
    GrandGrid.MakeGrid($("#grdMaterialSampleDetails"), 0, vendorObject.MaterialSamples);
    ClearSamples();

    BindGrid()
    //New End

    checkHideMaterialTaxPopUpImage();
    checkHideVendorTaxPopUpImage();
}

function BindGrid() {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = vendorRegistration.GetVendorMaterials + $("[id$=hdfVendorPK]").val()
    $("#grdVendorMaterialDetails").removeAttr("ajaxurl")
    $("#grdVendorMaterialDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorMaterialDetails");
    GrandGrid.MakeGrid($("#grdVendorMaterialDetails"));
    checkHideMaterialTaxPopUpImage();
    return false;
}

// Method to Change Item Currency as vendor Currency
function ChangeCurrency() {
    $("select[id$=ITV_CURRENCY]").val($("select[id$=VEN_CURRENCY]").val());
    $("select[id$=ITV_CURRENCY]").attr("disabled", true);
    UpdateMaterialMappingList();
}
// Method to Change Currency  with respect to Type
function ChangeType() {
    typeChanged = true;
    if ($("select[id$=VEN_PO_TYPE]").val() == "2" || $("[id$=hdfResetCurrencyOnType]").val() == "0") {//If Vendor Type is "Local" ,by default currency should be base currency.Bug ID:  35014 :The currency changing functionality on changing type is not required for EKK.
        if ($("[id$=VEN_PK]").val() == "0")//New Mode
        {
            FillCurrency($("[id$=hdfBaseCurrency]").val());
        }
        else {
            $("select[id$=VEN_CURRENCY]").val($("[id$=hdfBaseCurrency]").val());
            ChangeCurrency();
        }
    }
    else {
        if ($("[id$=VEN_PK]").val() == "0")//New Mode
        {
            FillCurrency();
        }
        else {
            $("select[id$=VEN_CURRENCY]").val("0");
            ChangeCurrency();
        }
    }
}

//If Vendor Type is "Local", then the currency should be base currency.If not - set a validation message to show for user do you want to continue or not.
function ShowDifferentCurrencyConfirm(command) {

    var msgTitle;
    var msg;
    msgTitle = "Translate(Information)";
    if ($("[id$=hdfResetCurrencyOnType]").val() == "0")//EKK
        msg = "Translate(DifferentCurrencyConfirmWithOutType)";
    else
        msg = "Translate(DifferentCurrencyConfirmMsg)";
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIscontYes]").val(1);
                $(this).dialog("close");
                SavePage(command);
            },
            Cancel: function (e) {
                $("[id$=hdfIscontYes]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

function ConfirmCurrencyTypeChange(command,type) {

    var msgTitle;
    var msg;
    msgTitle = "Translate(Information)";
    msg = "Translate(ConfirmCurrencyType)";
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfCurrncyChangeConfirm]").val(1);
                $(this).dialog("close");
                SavePage(command,type);
            },
            Cancel: function (e) {
                $("[id$=hdfCurrncyChangeConfirm]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}

/// Method to Update Material Mapping List Currency Based on Vendor Currency
function UpdateMaterialMappingList() {
    var ObjVendor = $("#divVendorData").data("VendorData");
    if (ObjVendor.MaterialDetails.length > 0) {
        for (var index in ObjVendor.MaterialDetails) {
            ObjVendor.MaterialDetails[index].ITV_CURRENCY = $("select[id$=ITV_CURRENCY]").val();
            ObjVendor.MaterialDetails[index].MaterialCurrencyText = $("select[id$=ITV_CURRENCY] option:selected").text();
        }
        $("#divVendorData").data("VendorData", ObjVendor);
        GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjVendor.MaterialDetails);
    }
}

//To Fill File Details To Grid
function FillFileDetails() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}

function FillDropDowns() {
    ///<summary>
    ///Used for FillDropDowns
    ///</summary>
    FillCountry();
    FillType(); //FillCurrency() was moved to function FillType()   
    FillGSTType();
    //FillCurrency();    
    FillTax(0);
    FillAccount(0);
    FillAdvAccount(0);

}

function FillType(SelectVal) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=VEN_PO_TYPE]").attr("id");
    $.get(vendorRegistration.GetVendorPOType + $("[id$=BizUnitPk]").val() + "&CfgType=" + vendorRegistration.CfgType, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
        $("[id$=VEN_PO_TYPE_TEXT]").html($("select[id$=VEN_PO_TYPE] option:selected").text());
        if ($("[id$=VEN_PK]").val() == "0") {
            ChangeType(); //FillCurrency() called inside this function (If Vendor Type is "Local" ,by default currency should be base currency)
        }
    });
}

function FillGSTType(SelectVal) {
    ///<summary>function used to Fill Vendor GST Type</summary>   
    var drpID = $("select[id$=VEN_GST_TYPE]").attr("id");
    $.get(vendorRegistration.GetVendorGSTType + vendorRegistration.CfgGstType, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, SelectVal);
        //$("[id$=VEN_GST_TYPE_TEXT]").html($("select[id$=VEN_GST_TYPE] option:selected").text());       
    });
}

function FillAccount(SelectVal) {
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=VEN_ACCOUNT]").attr("id");
    var url = vendorRegistration.GetAccount + "&Active=1" + "&SubType=1" + "&IsGroup=0" + "&IsIncludeAccCode=1";
    $.get(url, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true,true, SelectVal);
        $("[id$=VEN_ACCOUNT_TEXT]").html($("select[id$=VEN_ACCOUNT] option:selected").text());
    });
}

function FillAdvAccount(SelectVal) {
    //debugger;
    ///<summary>function used to Fill which dept department is raising po details</summary>
    // var queryString = "&BizUnit=" + PurchaseOrderConfig.BizUnitPk + "&Vendor=" + vendorPK 
    var drpID = $("select[id$=VEN_ADP_ACCOUNT]").attr("id");
    var url = vendorRegistration.GetAccount + "&Active=1" + "&SubType=12" + "&IsGroup=0" + "&IsIncludeAccCode=1";
    $.get(url, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, SelectVal);
        //$("[id$=VEN_ACCOUNT_TEXT]").html($("select[id$=VEN_ACCOUNT] option:selected").text());
    });
}
function FillCountry(controlID, countryID) {
    ///<summary>function used to fill vendor details corresponding to vendor id </summary>
    /// <param name="controlID"  type="Object">
    /// Determins which tab Address or vendor
    /// </param>
    /// <param name="countryID"  type="Object">
    /// Country Id to Select after filling Drop down
    /// </param>
    var drpID = $("select[id$=VEN_CNTRY]").attr("id");
    var AddressdrpID = $("select[id$=VNC_CNTRY]").attr("id");
    //var LocalAddressdrpID = $("select[id$=VNC_LC_CNTRY]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(vendorRegistration.GetCountry, function (data) {
        if (controlID == "ADDRESS") {
            GrandScriptUtils.FillDropDown(AddressdrpID, data, true, true, countryID);
        }
        else if (controlID == "VENDOR") {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, countryID);
        }
        else if (controlID == "VBD_COUNTRY") {
            var id = $("select[id$=VBD_COUNTRY]").attr("id");
            GrandScriptUtils.FillDropDown(id, data, true, true);
        }
        else if (controlID == "VNC_LC_CNTRY") {
            var id = $("select[id$=VNC_LC_CNTRY]").attr("id");
            GrandScriptUtils.FillDropDown(id, data, true, true);
        }
        else {
          
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
            GrandScriptUtils.FillDropDown(AddressdrpID, data, true, true);
            //GrandScriptUtils.FillDropDown(LocalAddressdrpID, data, true, true);
            var id = $("select[id$=VBD_COUNTRY]").attr("id");
            GrandScriptUtils.FillDropDown(id, data, true, true);
        }
    });
}

function FillState(controlID, stateID, countryID) {
    ////<summary>function used to fill State </summary>
    /// <param name="controlID"  type="Object">
    /// Determins which tab Address or vendor
    /// </param>
    /// <param name="stateID"  type="Object">
    /// State Id to Select after filling Drop down
    /// </param>
    /// <param name="countryID"  type="Object">
    /// Country Id to Fill State Corresponding to Country
    /// </param>
    var drpID;
    var ajaxurl;
    if (controlID == "VENDOR") {
        drpID = $("select[id$=VEN_STATE]").attr("id");
    }
    else if (controlID == "ADDRESS") {
        drpID = $("select[id$=VNC_STATE]").attr("id");
    }
    if (countryID)
        ajaxurl = vendorRegistration.GetState + "&CountryID=" + countryID;
    else
        ajaxurl = vendorRegistration.GetState + "&CountryID=" + $("select[id$=VEN_CNTRY]").val();

    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, stateID);
    });
    return false;
}

function FillCurrency(currencyID) {
    ///<summary>
    ///Used for FillCurrency
    ///</summary>
    // Get id of the Country DropDown
    var drpID = $("select[id$=VEN_CURRENCY]").attr("id");
    var drpAddressCurrency = $("select[id$=Currency]").attr("id");
    var drpmaterialCurrency = $("select[id$=ITV_CURRENCY]").attr("id");
    $.get(vendorRegistration.GetCurrency + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, currencyID);
        GrandScriptUtils.FillDropDown(drpAddressCurrency, data, true, true);
        GrandScriptUtils.FillDropDown(drpmaterialCurrency, data, true, true);
        if (typeChanged == true) {
            ChangeCurrency();
        }
    });
}



function TabChange(tabname) {
    if (tabname == "material") {
        $("select[id$=ITV_CURRENCY]").val($("select[id$=VEN_CURRENCY]").val());
        $("select[id$=ITV_CURRENCY]").attr("disabled", true);
        $("select[id$=MaterialType]").focus();
        $("[id$='ITV_MOQ']").val("");
        //$("[id$='ITV_MOQ']").val(parseFloat("0").toFixed(QtyDec));
        $("select[id$=MaterialType]").val("0");
        FillCategoryMaterials(0);
    }
}

function ResetPage() {
    $("[id$=hdfVendorPK]").val('0')
    window.location = vendorRegistration.VendorListing;
    return false;
}

function ShowVendor() {
    //<summary>Function Used to Show Vendor Panel </summary>
    $("#imgVendorHide").show();
    $("#imgVendorShow").hide();
    $("#VendorSelection").show();
}

function HideVendor() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgVendorHide").hide();
    $("#imgVendorShow").show();
    $("#VendorSelection").hide();
}
//#endregion

///#region Core Section
//#region *************************************************************AddressBook Management**********************************************************
function SaveAddress() {
    ///<summary>
    ///Used for SaveAddress
    ///</summary>
    AddValidations(2)
    if ($(document.forms[0]).valid()) {
        $("[id$=VNC_DEFAULT]").attr("disabled", false);
        var ObjVendor = $("#divVendorData").data("VendorData");
        var editAddress = $("input[id$=EditAddress]").val();
        var obj = new Object();
        if (editAddress != 0) {
            for (var i in ObjVendor.AddressBookDetails) {
                if (editAddress == ObjVendor.AddressBookDetails[i].AddressID)
                    obj = ObjVendor.AddressBookDetails[i];
            }
        }
        var guid = GrandScriptUtils.GenerateGuid();
        obj.VNC_NAME = $("[id$=VNC_NAME]").val();
        obj.VNC_CONT_NAME = $("[id$=VNC_CONT_NAME]").val();
        obj.VNC_ADDR1 = $("[id$=VNC_ADDR1]").val();
        obj.VNC_ADDR2 = $("[id$=VNC_ADDR2]").val();
        obj.VNC_EMAIL = $("[id$=VNC_EMAIL]").val();
        obj.VNC_PHONE = $("[id$=VNC_PHONE]").val();
        obj.VNC_DEFAULT = ($("[id$=VNC_DEFAULT]").attr("checked")) ? "True" : "False";
        obj.VNC_CITY = $("[id$=VNC_CITY]").val();
        obj.VNC_CNTRY = $("[id$=VNC_CNTRY]").val();
        obj.VNC_STATE = $("[id$=VNC_STATE]").val();
        obj.VNC_MOBIL = $("[id$=VNC_MOBIL]").val();
        obj.VNC_FAX = $("[id$=VNC_FAX]").val();
        obj.VNC_TAX_NO = $("[id$=VNC_TAX_NO]").val();
        obj.VNC_TYPE = $("[id$=VNC_TYPE]").val();
        obj.VNC_TYPE_TEXT = $("[id$=VNC_TYPE] option:selected").html() == "--Select--" ? "" : $("[id$=VNC_TYPE] option:selected").html();
        obj.VNC_NAME2 = $("[id$=VNC_NAME2]").val();
        obj.VNC_ADDR3 = $("[id$=VNC_ADDR3]").val();

        //TYPE_ID
        obj.VNC_TYPE_NAME = $("[id$=VNC_TYPE_NAME]").val();


        if (editAddress == 0) {
            obj.AddressID = guid;
            obj.VNC_PK = "0";
            ObjVendor.AddressBookDetails.push(obj);
        }
        $("#divVendorData").data("VendorData", ObjVendor);
        GrandGrid.MakeGrid($("#grdVendorAddressDetails"), 0, ObjVendor.AddressBookDetails);
        ClearAddress();
    }
    return false;
}

function GridAddressHandler(tr, command) {
    switch (command.toString().toUpperCase()) {
        case vendorRegistration.DeleteCommand:
            addressID = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.AddressID, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.DeleteAddress, true);
            //DeleteAddress(tr);
            return false;
            break;
        case vendorRegistration.EditCommand:
            FillAddress(tr);
            return false;
            break;
        case vendorRegistration.ViewCommand:
            FillAddress(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

function DeleteAddress(tr) {
    var ObjVendor = $("#divVendorData").data("VendorData");
    for (var i in ObjVendor.AddressBookDetails) {
        if (ObjVendor.AddressBookDetails[i].AddressID == addressID) {
            //Will delete the Address details
            ObjVendor.AddressBookDetails.splice(i, 1);
            break;
        }
    }
    $("#divVendorData").data("VendorData", ObjVendor);
    GrandGrid.MakeGrid($("#grdVendorAddressDetails"), 0, ObjVendor.AddressBookDetails);
    ClearAddress();
}

function FillAddress(tr) {
    ClearAddress();
    var tableID = $(tr).parents("table:first").attr("id");
    $("[id$=VNC_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_NAME, tableID));
    $("[id$=VNC_CONT_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_CONT_NAME, tableID));
    $("[id$=VNC_ADDR1]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_ADDR1, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_ADDR1, tableID));
    $("[id$=VNC_ADDR2]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_ADDR2, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_ADDR2, tableID));
    $("[id$=VNC_EMAIL]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_EMAIL, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_EMAIL, tableID));
    $("[id$=VNC_PHONE]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_PHONE, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_PHONE, tableID));
    $("[id$=VNC_TAX_NO]").val((GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TAX_NO, tableID) == "undefined" || GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TAX_NO, tableID) == "null") ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TAX_NO, tableID));
    $("[id$=VNC_ADDR3]").val(GrandGrid.Utilities.GetColumnValue(tr, "VNC_ADDR3", tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, "VNC_ADDR3", tableID));
    $("[id$=VNC_NAME2]").val((GrandGrid.Utilities.GetColumnValue(tr, "VNC_NAME2", tableID) == "null" || GrandGrid.Utilities.GetColumnValue(tr, "VNC_NAME2", tableID) == "undefined") ? "" : GrandGrid.Utilities.GetColumnValue(tr, "VNC_NAME2", tableID));
    //VNC_TYPE_NAME Adding
    $("[id$=VNC_TYPE_NAME]").val((GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TYPE_NAME, tableID) == "undefined" || GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TYPE_NAME, tableID) == "null") ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TYPE_NAME, tableID));

    $("[id$=VNC_DEFAULT]").attr("disabled", false);
    if (GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_DEFAULT, tableID) == "True" || GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_DEFAULT, tableID) == "1") {
        $("[id$=VNC_DEFAULT]").attr("disabled", false);
        $("[id$=VNC_DEFAULT]").attr("checked", true);
    }
    else {
        var ObjVendor = $("#divVendorData").data("VendorData");
        if (ObjVendor.AddressBookDetails.length > 0) {
            for (var i in ObjVendor.AddressBookDetails) {
                if (ObjVendor.AddressBookDetails[i].VNC_DEFAULT == "True" || ObjVendor.AddressBookDetails[i].VNC_DEFAULT == "1") {
                    $("[id$=VNC_DEFAULT]").attr("disabled", true);
                }
            }
        }
        else {
            $("[id$=VNC_DEFAULT]").attr("disabled", false);
        }
    }
    $("[id$=VNC_CITY]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_CITY, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_CITY, tableID));
    FillCountry("ADDRESS", GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_CNTRY, tableID));
    if (vendorRegistration.VNC_CNTRY != "0")
        FillState("ADDRESS", GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_STATE, tableID), GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_CNTRY, tableID));
    else
        $("[id$=VNC_STATE]").find("option").remove();
    //New
    FillAddressType(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_TYPE, tableID));
    //New End
    $("[id$=VNC_MOBIL]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_MOBIL, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_MOBIL, tableID));
    $("[id$=VNC_FAX]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_FAX, tableID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.VNC_FAX, tableID));
    $("input[id$=EditAddress]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.AddressID, tableID));
}

function ClearAddress() {
    $("[id$=VNC_NAME]").val('');
    $("[id$=VNC_CONT_NAME]").val('');
    $("[id$=VNC_ADDR1]").val('');
    $("[id$=VNC_ADDR2]").val('');
    $("[id$=VNC_EMAIL]").val('');
    $("[id$=VNC_PHONE]").val('');
    $("[id$=VNC_DEFAULT]").attr("checked", false);
    $("[id$=VNC_CITY]").val('');
    $("select[id$=VNC_CNTRY]").val('0');
    $("select[id$=VNC_STATE]").html('');
    $("[id$=VNC_MOBIL]").val('');
    $("[id$=VNC_TYPE]").val('');
    $("[id$=VNC_FAX]").val('');
    $("[id$=VNC_TAX_NO]").val('');
    $("[id$=VNC_NAME2]").val('');
    $("[id$=VNC_ADDR3]").val('');
    //18_08
    $("[id$=VNC_TYPE_NAME]").val('');

    $("select[id$=VNC_TYPE]").val('0');
    $("input[id$=EditAddress]").val('0');
    addressID = 0;
    var ObjVendor = $("#divVendorData").data("VendorData");
    if (ObjVendor.AddressBookDetails.length > 0) {
        for (var i in ObjVendor.AddressBookDetails) {
            if (ObjVendor.AddressBookDetails[i].VNC_DEFAULT == "True" || ObjVendor.AddressBookDetails[i].VNC_DEFAULT == "1") {
                $("[id$=VNC_DEFAULT]").attr("disabled", true);
            }
        }
    }
    else {
        $("[id$=VNC_DEFAULT]").attr("disabled", false);
        $("[id$=VNC_DEFAULT]").attr("checked", true);
    }
    return false;
}

//New
function FillAddressType(catgID) {
    //<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=VNC_TYPE]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(vendorRegistration.GetAddressTypeList + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, catgID);
    });


}
//new End
//#endregion

//#region*****************************************************MaterialManagement**************************************************************
function FillMaterialCategory() {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var drpID = $("select[id$=MaterialType]").attr("id");
    $.get(vendorRegistration.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val() + "&IncludeFG=" + $("[id$=hdfIncludeFG]").val(), function (data) {
        for (var i in data) {//checking data contains finished good or semifinished good.if exist removing that items from dropdown.
            if ((data[i].Text == vendorRegistration.SEMIFINISHEDGOOD) || (data[i].Text == vendorRegistration.FINISHEDGOOD)) {
                data.splice(i, 1);
            }
        }
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}



//NewSamples Start
function FillMaterialSamplesCategory() {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    var SamplesdrpID = $("select[id$=MaterialTypeSamples]").attr("id");
    $.get(vendorRegistration.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val() + "&IncludeFG=" + $("[id$=hdfIncludeFG]").val(), function (data) {
        for (var i in data) {//checking data contains finished good or semifinished good.if exist removing that items from dropdown.
            if ((data[i].Text == vendorRegistration.SEMIFINISHEDGOOD) || (data[i].Text == vendorRegistration.FINISHEDGOOD)) {
                data.splice(i, 1);
            }
        }
        GrandScriptUtils.FillDropDown(SamplesdrpID, data, true, true);
    });
}
//New End

function ShowCategory() {
    ///<summary>Function used call the tree Data For filling the Material Category </summary>
    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Category", false, "700", false, false);
    return false;
}

function FillUOM(categoryID, selectval) {
    ///<summary>function To Fill Uom Details </summary>
    /// <param name="categoryID"  type="string">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    /// <param name="selectval"  type="string">
    ///     Specific value to be selected.
    /// </param>
    if (categoryID != 0) {
        $("select[id$=ITV_MOQ_UOM]").removeData();
        var drpID = $("select[id$=ITV_MOQ_UOM]").attr("id");
        //need to correct after QC
        //        $.get(vendorRegistration.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val() + vendorRegistration.Param + categoryID, function (data) {
        $.get(vendorRegistration.FillMaterialUOMDropdownURL + $("[id$=BizUnitPk]").val(), function (data) {
            if (selectval) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true);
            }
        });
    }
    else
        $("select[id$=MaterialUOM]").find("option").remove();
}

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    $("select[id$=MaterialType]").val(cagID);
    //setting selected value for uom after selecting category from treeview.
    FillUOM(cagID, false)
    //Filling the material Drop down after selecting the material category
    FillCategoryMaterials(cagID);
    //closing modalbox after selected from treeview.
    $("#divCategory").dialog("destroy");
    $("#divCategory").dialog({ autoOpen: false });
}

function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (categoryID != "0") {
        FillUOM(categoryID, false);
        FillCategoryMaterials(categoryID);
    }
    else {
        ClearMaterials();
    }
}

//NewSamples Start
function FillCategorySamplesDetails(categoryID) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (categoryID != "0") {
        FillCategorySampleMaterials(categoryID);
    }
    else {
        ClearSamples();
    }
}
//New End

function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    SetTreeHeaderStructure("trvCategory", vendorRegistration.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + "&MatCagID=", "Root", false, false, "0", false);  // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

//function AddMaterialDetails() {
//    ///<summary>
//    ///Used for SaveAddress
//    ///</summary>
//    AddValidations(3)
//    if ($(document.forms[0]).valid()) {
//        var ObjVendor = $("#divVendorData").data("VendorData");
//        var editMaterial = $("input[id$=EditMaterial]").val();
//        var obj = new Object();
//        var flag = true;
//        if (parseInt(editMaterial) == 0) {
//            for (var i in ObjVendor.MaterialDetails) {
//                if (ObjVendor.MaterialDetails[i].ITV_ITEM == $("select[id$=ITV_ITEM]").val()) {
//                    flag = false;
//                    break;
//                }
//            }
//        }
//        else {
//            for (var i in ObjVendor.MaterialDetails) {
//                if (ObjVendor.MaterialDetails[i].ITV_ITEM == $("select[id$=ITV_ITEM]").val() && parseInt(editMaterial) != ObjVendor.MaterialDetails[i].ITV_ITEM) {
//                    flag = false;
//                    break;
//                }
//                if (parseInt(editMaterial) == ObjVendor.MaterialDetails[i].ITV_ITEM)
//                    obj = ObjVendor.MaterialDetails[i];
//            }
//        }TabChange//        if (flag) {
//            obj.MaterialTypeText = $("select[id$=MaterialType] option:selected").text();
//            obj.MaterialType = $("select[id$=MaterialType]").val();
//            obj.ITV_ITEM = $("select[id$=ITV_ITEM]").val();
//            obj.MaterialCode = $("select[id$=ITV_ITEM] option:selected").text();
//            obj.ITV_NAME = $("input[id$=ITV_NAME]").val();
//            obj.ITV_PRICE = parseFloat($("input[id$=ITV_PRICE]").val()).toFixed(RateDec);
//            obj.ITV_CURRENCY = $("select[id$=ITV_CURRENCY]").val();
//            obj.MaterialCurrencyText = $("select[id$=ITV_CURRENCY] option:selected").text();
//            obj.ITV_MOQ = parseFloat($("input[id$=ITV_MOQ]").val()).toFixed(QtyDec);
//            obj.ITV_MOQ_UOM = $("select[id$=ITV_MOQ_UOM]").val();
//            obj.UOMText = $("select[id$=ITV_MOQ_UOM] option:selected").text();
//            //NewMaterial Start
//            obj.ITV_DISC_PERC = 0;
//            obj.ITV_TAX_PERC = 0;
//            obj.ITV_LEAD_TIME = $("input[id$=ITV_LEAD_TIME]").val();
//            if (obj.ITV_SL_NO == null || obj.ITV_SL_NO == 0) {
//                var maxSlNo = JSLINQ(ObjVendor.MaterialDetails)
//                                    .Max(function (itm) { return itm.ITV_SL_NO; });
//                obj.ITV_SL_NO = maxSlNo == null || maxSlNo == 0 ? 1 : parseInt(maxSlNo) + 1;
//            }
//            //NewMaterial End
//            //NewMaterial start
//            //obj.ITV_TAX_PERC = $("input[id$=ITV_TAX_PERC]").val();
//            //New End
//            if (parseInt(editMaterial) == 0) {
//                obj.VendorITV_ITEM = 0;
//                ObjVendor.MaterialDetails.push(obj);
//            }
//            $("#divVendorData").data("VendorData", ObjVendor);
//            GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, ObjVendor.MaterialDetails);
//        }
//        else {
//            GrandScriptUtils.ShowModal("Material Already Added", "Information");
//        }
//        ClearMaterials();
//    }
//    return false;
//}


function AddMaterialDetails() {
    ///<summary>
    ///Used for SaveAddress
    ///</summary>
    if ($("[id$=hdfVendorPK]").val() == "0") {
        SavePage("Draft", "AddMaterial");
    }
    else
        AddMaterials();
    return false;
}

function AddMaterials() {

    AddValidations(3)
    if ($(document.forms[0]).valid()) {
        var obj = new Object();
        obj.P_ITV_PK = materialID;
        //        obj.P_ITV_ITEM = $("select[id$=ITV_ITEM]").val();
        obj.P_ITV_ITEM = $("[id$=ITV_ITEM]").val();
        obj.P_ITV_VENDOR = $("[id$=hdfVendorPK]").val();
        obj.P_ITV_NAME = $("input[id$=MaterialItem]").val(); //$("input[id$=ITV_NAME]").val();
        obj.P_ITV_PRICE = $("input[id$=ITV_PRICE]").val() == "" ? parseFloat("0.00").toFixed(RateDec) : parseFloat($("input[id$=ITV_PRICE]").val()).toFixed(RateDec);
        obj.P_ITV_CURRENCY = $("select[id$=ITV_CURRENCY]").val();
        //obj.P_ITV_MOQ = parseFloat($("input[id$=ITV_MOQ]").val()).toFixed(QtyDec);
        obj.P_ITV_MOQ = parseFloat($("input[id$=ITV_MOQ]").val() == "" ? parseFloat("0.00").toFixed(QtyDec) : $("input[id$=ITV_MOQ]").val()).toFixed(QtyDec);
        obj.P_ITV_MOQ_UOM = $("select[id$=ITV_MOQ_UOM]").val();
        obj.P_ITV_LEAD_TIME = $("input[id$=ITV_LEAD_TIME]").val();
        obj.P_ACTIVE = $("[id$=ddlActive]").val();
        var material = JSON.stringify(obj);
        $.post(vendorRegistration.SaveMaterial, material, function (data) {
            if (data) {
                if (parseInt(data) > 0) {
                    $("#divdummyMaterial").show();
                    $(tdset).insertAfter($("#materialInsert").find("tr:eq(0)"));
                    BindGrid();

                }
                else if (parseInt(data) == -61) {
                    GrandScriptUtils.ShowModal("Server busy, Try again", "Information");
                }
                else {
                    GrandScriptUtils.ShowModal("Material Already Added", "Information");
                }
            }
        });
        ClearMaterials();
    }
    return false;
}


//NewSamples Start
function AddSampleDetails() {
    ///<summary>
    ///Used for SaveAddress
    ///</summary>
    AddValidations(4)
    if ($(document.forms[0]).valid()) {
        var ObjVendor = $("#divVendorData").data("VendorData");
        var editSamples = $("input[id$=EditSamples]").val();
        var obj = new Object();
        var flag = true;
        if (parseInt(editSamples) == 0) {
            for (var i in ObjVendor.MaterialSamples) {
                if (ObjVendor.MaterialSamples[i].ISV_ITEM == $("[id$=ISV_ITEM]").val()) {
                    //                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjVendor.MaterialSamples) {
                if (ObjVendor.MaterialSamples[i].ISV_ITEM == $("[id$=ISV_ITEM]").val() && parseInt(editSamples) != ObjVendor.MaterialSamples[i].ISV_ITEM) {
                    flag = false;
                    break;
                }
                if (parseInt(editSamples) == ObjVendor.MaterialSamples[i].ISV_ITEM)
                    obj = ObjVendor.MaterialSamples[i];
            }
        }
        if (flag) {
            obj.MaterialTypeText = $("select[id$=MaterialTypeSamples] option:selected").text();
            obj.MaterialType = $("select[id$=MaterialTypeSamples]").val();
            obj.ISV_ITEM = $("[id$=ISV_ITEM]").val();
            obj.SampleMaterialCode = $("[id$=txtSampleItem]").val()
            obj.ISV_RECEIVED_DATE = $("[id$=ISV_RECEIVED_DATE]").val();
            obj.ISV_QC_TEST = $("input[id$=ISV_QC_TEST]").val();
            obj.ISV_QC_VALUE = $("input[id$=ISV_QC_VALUE]").val();
            obj.ISV_REMARKS = $("input[id$=ISV_REMARKS]").val();
            obj.ISV_QUANTITY = parseFloat($("input[id$=ISV_QUANTITY]").val()).toFixed(QtyDec);
            obj.ISV_REFERENCE = $("input[id$=ISV_REFERENCE]").val();
            obj.ISV_STATUS = ($("[id$=ISV_STATUS]").attr("checked")) ? "True" : "False";
            obj.ISV_STATUS_TEXT = ($("[id$=ISV_STATUS]").attr("checked")) ? "Accept" : "Reject";
            if (parseInt(editSamples) == 0) {
                obj.VendorISV_ITEM = 0;
                ObjVendor.MaterialSamples.push(obj);
            }
            $("#divVendorData").data("VendorData", ObjVendor);
            GrandGrid.MakeGrid($("#grdMaterialSampleDetails"), 0, ObjVendor.MaterialSamples);

        }
        else {
            GrandScriptUtils.ShowModal("Samples Already Added", "Information");
        }
        ClearSamples();
    }
    return false;
}
//New End

//function FillCategoryMaterials(categoryID, materialID) {
//    ///<summary>Function used Add the tree Data </summary>
//    /// <param name="categoryID"  type="object">
//    ///     Specific categoryid to fill corresponding Material
//    /// </param>
//    /// <param name="materialID"  type="object">
//    ///     Specific materialID to select the dropdown item after filling drop down
//    /// </param>
//    var drpID = $("select[id$=ITV_ITEM]").attr("id");
//    $.getJSON(vendorRegistration.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID, function (data) {
//        if (materialID) {
//            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
//        }
//        else {
//            GrandScriptUtils.FillDropDown(drpID, data, true, true);
//        }
//    });
//}

function FillCategorySampleMaterials(categoryID, sampleMaterialID) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="sampleMaterialID"  type="object">
    ///     Specific sampleMaterialID to select the dropdown item after filling drop down
    /// </param>
    //    var drpID = $("select[id$=ISV_ITEM]").attr("id");
    //    $.getJSON(vendorRegistration.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID, function (data) {
    //        if (sampleMaterialID) {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, true, sampleMaterialID);
    //        }
    //        else {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    //        }
    //    });

    $("[id$=txtSampleItem]").val("");
    $("[id$=ISV_ITEM]").val(0);
    GrandScriptUtils.MakeAutoCompleteLimitLen("txtSampleItem", vendorRegistration.MaterialURL, "ISV_ITEM", true, false, "MaterialTypeSamples", true, "Store", "", "", $("[id$=AutoStartValue]").val());
    if (sampleMaterialID)
        $("[id$=ISV_ITEM]").val(sampleMaterialID);
}

function GridMaterialAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case vendorRegistration.DeleteCommand: //GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_ITEM, tableID)
            materialID = GrandGrid.Utilities.GetColumnValue(tr, "ITV_PK", $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.DeleteMaterial, true);
            //DeleteMaterial(tr);
            return false;
            break;
        //NewMAterial Start                                    
        case vendorRegistration.HistoryCommand:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_ITEM, $(tr).parents("table:first").attr("id"));
            //$("[id$=VEN_PK]").val(vendorObject.VEN_PK);
            if ($("[id$=VEN_PK]").val() != "0") {
                var vendorPK = "0";
                var vendorPK = $("[id$=VEN_PK]").val(); //GetRateHistory: "MaterialManagement.do?Action=GetRateHistory&SBUPk=",
                ShowRateHistory(materialID, vendorPK);
            }
            return false;
            break;
        //New End                                    
        case vendorRegistration.EditCommand:
            materialID = GrandGrid.Utilities.GetColumnValue(tr, "ITV_PK", $(tr).parents("table:first").attr("id"));
            FillMaterial(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

//NewSamples Start
function GridSamplesAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case vendorRegistration.DeleteCommand:
            sampleMaterialID = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_ITEM, $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.DeleteSamples, true);
            return false;
            break;
        case vendorRegistration.EditCommand:
            FillSamples(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}
//New End

//function DeleteMaterial(tr) {
//    ///<summary>Function Used to delete the selected material from the gris </summary>
//    /// <param name="tr"  type="Object">
//    ///     Specific Container and its controls
//    /// </param>
//    var objVendor = $("#divVendorData").data("VendorData");
//    for (var i in objVendor.MaterialDetails) {
//        if (objVendor.MaterialDetails[i].ITV_ITEM == materialID) {
//            slNo = objVendor.MaterialDetails[i].ITV_SL_NO;
//            var TaxDetails = $("#divData").data("TaxDetails");
//            for (var j = 0; j < TaxDetails.length; j++) {
//                if (TaxDetails[j].IVT_SL_NO == slNo) {
//                    TaxDetails.splice(j, 1);
//                    --j;
//                }
//            }
//            //Will delete the Material details
//            objVendor.MaterialDetails.splice(i, 1);
//            $("#divData").data("TaxDetails", TaxDetails);
//            break;
//        }
//    }
//    $("#divVendorData").data("VendorData", objVendor);
//    if (objVendor.MaterialDetails.length == 0) {
//        $("#divdummyMaterial").show();
//        GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, objVendor.MaterialDetails);
//        $(tdset).insertAfter($("#materialInsert").find("tr:eq(0)"));
//    }
//    else {
//        GrandGrid.MakeGrid($("#grdVendorMaterialDetails"), 0, objVendor.MaterialDetails);
//    }
//    ClearMaterials();
//}

function DeleteMaterial(tr) {
    ///<summary>Function Used to delete the selected material from the gris </summary>
    $.get(vendorRegistration.DeleteVenMaterial + materialID, function (data) {
        if (parseInt(data) > 0) {
            $("#divdummyMaterial").show();
            $(tdset).insertAfter($("#materialInsert").find("tr:eq(0)"));
            BindGrid();
        }
        else if (parseInt(data) == -31) {
            GrandScriptUtils.ShowModal(vendorRegistration.UsedMaterialInAnotherPlace, vendorRegistration.Information);
        }

    });

    ClearMaterials();
}

//NewSamples Start
function DeleteSamples(tr) {
    ///<summary>Function Used to delete the selected material from the gris </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>

    var objVendor = $("#divVendorData").data("VendorData");
    for (var i in objVendor.MaterialSamples) {
        if (objVendor.MaterialSamples[i].ISV_ITEM == sampleMaterialID) {
            //Will delete the Material details
            objVendor.MaterialSamples.splice(i, 1);
            break;
        }
    }
    $("#divVendorData").data("VendorData", objVendor);
    if (objVendor.MaterialSamples.length == 0) {
        $("#divdummySamples").show();
        GrandGrid.MakeGrid($("#grdMaterialSampleDetails"), 0, objVendor.MaterialSamples);
        $(tdsetSamples).insertAfter($("#sampleInsert").find("tr:eq(0)"));
    }
    else {
        GrandGrid.MakeGrid($("#grdMaterialSampleDetails"), 0, objVendor.MaterialSamples);
    }
    ClearSamples();
}
//New End

function FillMaterial(tr) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls       
    /// </param>
    var tableID = $(tr).parents("table:first").attr("id");
    $("select[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialType, tableID));
    $("select[id$=ddlActive]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITV_ACTIVE", tableID));
    FillCategoryMaterials(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialType, tableID), GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_ITEM, tableID));
    //    $("input[id$=ITV_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialNameVendor, tableID));
    if (parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialITV_PRICE, tableID)) > 0)
        $("input[id$=ITV_PRICE]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialITV_PRICE, tableID)).toFixed(RateDec));
    $("select[id$=ITV_CURRENCY]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_CURRENCY, tableID));
    //$("input[id$=ITV_MOQ]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialITV_MOQ, tableID)).toFixed(QtyDec));

    $("input[id$=ITV_MOQ]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialITV_MOQ, tableID)) > 0 ? parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialITV_MOQ, tableID)).toFixed(QtyDec) : "");
    FillUOM(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialType, tableID), GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MatreialITV_MOQ_UOM, tableID))
    $("input[id$=EditMaterial]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_ITEM, tableID));
    $("input[id$=ITV_LEAD_TIME]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_LEAD_TIME, tableID));
    $("[id$=ITV_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ITV_ITEM, tableID));
    $("input[id$=MaterialItem]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialNameVendor, tableID));

}

//NewSamples Start
function FillSamples(tr) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls       
    /// </param>
    var tableID = $(tr).parents("table:first").attr("id");
    $("select[id$=MaterialTypeSamples]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialType, tableID));
    FillCategorySampleMaterials(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.MaterialType, tableID), GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_ITEM, tableID));
    $("input[id$=ISV_RECEIVED_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_RECEIVED_DATE, tableID));
    $("input[id$=ISV_QC_TEST]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_QC_TEST, tableID));
    $("input[id$=ISV_QC_VALUE]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_QC_VALUE, tableID));
    $("input[id$=ISV_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_REMARKS, tableID));

    $("input[id$=ISV_QUANTITY]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_QUANTITY, tableID)).toFixed(QtyDec));

    $("input[id$=ISV_REFERENCE]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_REFERENCE, tableID));
    $("input[id$=EditSamples]").val(GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_ITEM, tableID));
    if (GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_STATUS, tableID) == "True" || GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.ISV_STATUS, tableID) == "1") {
        $("[id$=ISV_STATUS]").attr("checked", true);
    } else {
        $("[id$=ISV_STATUS]").attr("checked", false);
    }
}
//New End

function ClearMaterials() {
    ///<summary>Function Used Clear material Details data Entry Section</summary>
    $("select[id$=MaterialType]").val("0");
    FillCategoryMaterials(0);
    // $("input[id$=ITV_NAME]").val("");
    $("input[id$=ITV_PRICE]").val("");
    $("select[id$=ITV_CURRENCY]").val($("select[id$=ITV_CURRENCY]").val());
    // $("input[id$=ITV_MOQ]").val(parseFloat("0").toFixed(QtyDec));
    $("input[id$=ITV_MOQ]").val("");
    $("input[id$=EditMaterial]").val("0");
    $("[id$=ITV_ITEM]").val("0")
    $("input[id$=MaterialItem]").val("");
    $("select[id$=ITV_MOQ_UOM]").html("");
    $("input[id$=ITV_LEAD_TIME]").val("0");
    $("select[id$=ddlActive]").val("1");
    materialID = 0;
}

//NewSamples  Start
function ClearSamples() {
    ///<summary>Function Used Clear material Details data Entry Section</summary>
    $("select[id$=MaterialTypeSamples]").val("0");
    //$("input[id$=ISV_RECEIVED_DATE]").val("");
    $("input[id$=ISV_QC_TEST]").val("");
    $("input[id$=ISV_QC_VALUE]").val("");
    $("input[id$=EditSamples]").val("0");
    $("input[id$=txtSampleItem]").val("");
    $("[id$=ISV_ITEM]").val(0);
    $("input[id$=ISV_REMARKS]").val("");
    $("input[id$=ISV_QUANTITY]").val(parseFloat("0").toFixed(QtyDec));
    $("input[id$=ISV_REFERENCE]").val("");
    $("input[id$=ISV_STATUS]").attr("checked", false);
    sampleMaterialID = 0;
    //    var ctrID = $("input[id$=ISV_RECEIVED_DATE]").attr("id");
    //    GrandScriptUtils.DatePicker(ctrID, false, false, true);
}
//New End

function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the selected material to controls to the controls in the tr </summary>
    /// <param name="materialID"  type="Object">
    ///  Specific Container and its controls       
    /// </param>
    var drpID = $("select[id$=ITV_MOQ_UOM]").attr("id");
    $.get(vendorRegistration.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        //$("input[id$=ITV_NAME]").val(data[0].ITM_NAME);
        $("select[id$=MaterialType]").val(data[0].ITM_CATEGORY);
        FillUOM(data[0].ITM_CATEGORY, data[0].ITM_UOM);
        //$("select[id$=ITV_MOQ_UOM]").val(data[0].ITM_UOM);


    });
}
//#endregion

//#region*****************************************************Terms Management*************************************************************
function BindVendorTerms() {
    var ajaxUrl = vendorRegistration.GetVendorTerms + "&BizUnitPk=" + $("[id$=BizUnitPk]").val();
    $("#grdTermsDetails").removeAttr("ajaxurl")
    $("#grdTermsDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTermsDetails");
    GrandGrid.MakeGrid($("#grdTermsDetails"));
    return false;
}

function AfterGridBind(gridID) {
    var colIndex = 0;
    if (gridID == $("#grdTermsDetails").attr("id")) {
        var count = 1;
        var ObjVendor = $("#divVendorData").data("VendorData");
        //$("#grdTermsDetails").find("th:eq(0)").css("width","10px");
        $("#grdTermsDetails").find("th:eq(4) span").text("Details");
        $("#grdTermsDetails").find("tr:has(td)").each(function () {
            var tableID = $(this).parents("table:first").attr("id");
            var TermsType = GrandGrid.Utilities.GetColumnValue(this, vendorRegistration.VendorTermsType, tableID)
            var TermsID = GrandGrid.Utilities.GetColumnValue(this, vendorRegistration.VendorTermsID, tableID)
            var grdcbx = $(this).find("td:first input");
            var grdinput = $(this).find("td:last input");
            if ($("[id$=ViewStatus]").val() == "1") {
                $(grdinput).attr("disabled", true);
                $(grdcbx).attr("disabled", true);
            }

            grdinput.attr("id", "inputype" + count)
            if (TermsType == "1") {
                //$(grdinput).removeAttr("onkeypress")
                GrandScriptUtils.DatePicker(grdinput.attr("id"), false, false);
                $(grdinput).val('');
                $(grdinput).attr("onkeydown", "javascript:return false;");
            }
            else if (TermsType == "3") {
                $(grdinput).attr("onkeydown", "javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);");
            }

            if (TermsType == "2") {
                $(grdinput).removeAttr("onkeypress");
            }
            //Loop through vendor terms and assigning the terms value while editing

            for (var i in ObjVendor.TermsDetails) {
                if (ObjVendor.TermsDetails.length >= 1) {
                    if (TermsID == ObjVendor.TermsDetails[i].VTD_VENDER_TERM) {
                        $(grdinput).val(ObjVendor.TermsDetails[i].VTD_VALUE);
                        $(grdcbx).attr("checked", true);
                    }
                }
            }
            count++;
        });
    }

    if (gridID == $("#grdVendorMaterialDetails").attr("id")) {
        var amt = 0;
        var qty = 0;
        var strVal = "";
        var itemCount = 0;
        var active = "Yes";

        if (tdset == "") {//tdset contains controls for add details.
            tdset = $("#materialInsert").find("tr:eq(1)");
        }
        //Mode iS view
        if ($("[id$=ViewStatus]").val() == "1") {
            $("#divdummyMaterial").hide();
            //Hiding the template field
            $("#grdVendorMaterialDetails").find("tr").each(function () {
                //                $(this).find("td:last,th:last").hide();
                $(this).find("td:last input[id$=imbEditMaterial]").hide();
                $(this).find("td:last input[id$=imbDeleteMaterial]").hide();
                $(this).find("td:last input[id$=imbrateHistory]").show();
            });
        }
        $("#divdummyMaterial").hide();
        $(tdset).insertBefore($("#grdVendorMaterialDetails").find("tr:eq(1)"));
        $("#grdVendorMaterialDetails").find("tr:has(td)").each(function () {
            itemCount = itemCount + 1;
            var ItemPK = GrandGrid.Utilities.GetColumnValue($(this), "ITV_PK", $(this).parents("table:first").attr("id"));
            ItemPK = parseInt(ItemPK) >= 0 ? ItemPK : 0;
            var slNo = GrandGrid.Utilities.GetColumnValue($(this), vendorRegistration.ITV_SL_NO, $(this).parents("table:first").attr("id"));
            slNo = parseInt(slNo) >= 0 ? slNo : 0;
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), vendorRegistration.ITV_DISC_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null && parseInt($("[id$='hdfIsLineitemDiscount']").val())> 0) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemDiscount('" + slNo + "','" + ItemPK + "');\" src=\"../Images/Classic/Icons/discount.png\" alt=\"Translate(Discounts)\" title=\"Translate(Discounts)\" style=\"cursor:pointer\" />");
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), vendorRegistration.ITV_TAX_PERC, $(this).parents("table:first").attr("id"));
            if (colIndex != null && parseInt($("[id$='hdfIsLineitemTax']").val()) > 0) {
                $(this).find("td:eq(" + colIndex + ")").html("<img onclick=\"javascript:AddLineItemTax('" + slNo + "','" + ItemPK + "');\" src=\"../Images/Classic/Icons/tax.png\"  alt=\"Translate(Taxes)\" title=\"Translate(Taxes)\" style=\"cursor:pointer\" purpose='MaterialTax' />");
                checkHideMaterialTaxPopUpImage();
            }
            else {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_PRICE", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                amt = GrandGrid.Utilities.GetColumnValue($(this), "ITV_PRICE", $(this).parents("table:first").attr("id"));
                strVal = amt == "" ? "" : parseFloat(amt).toFixed(RateDec);
                // strVal = amt == "" ? "" : amt;

                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_LEAD_TIME", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                var leadDays = GrandGrid.Utilities.GetColumnValue($(this), "ITV_LEAD_TIME", $(this).parents("table:first").attr("id"));
                strVal = leadDays == "" ? "" : leadDays;
                $(this).find("td:eq(" + colIndex + ")").html(strVal);
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_MOQ", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), "ITV_MOQ", $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                if (!isNaN(strVal))
                    $(this).find("td:eq(" + colIndex + ")").html(strVal);
                else
                    $(this).find("td:eq(" + colIndex + ")").html("");

            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                active = GrandGrid.Utilities.GetColumnValue($(this), "ITV_ACTIVE", $(this).parents("table:first").attr("id"));
                if (active == "1")
                    active = "Yes";
                else
                    active = "No";
                $(this).find("td:eq(" + colIndex + ")").html(active);
            }
        });
        if (itemCount == 0) {
            $("#divdummyMaterial").show();
        }

    }

    if (gridID == $("#grdRateDetails").attr("id")) {
        var toDate = "";
        $("#grdRateDetails").find("tr").each(function () {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), vendorRegistration.ToDate, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                toDate = GrandGrid.Utilities.GetColumnValue($(this), vendorRegistration.ToDate, $(this).parents("table:first").attr("id"));
                if (toDate == "null")
                    toDate = "Till Date";
                $(this).find("td:eq(" + colIndex + ")").html(toDate);
            }
        });
    }

    //NewSamples start
    if (gridID == $("#grdMaterialSampleDetails").attr("id")) {
        if (tdsetSamples == "") {//tdset contains controls for add details.
            tdsetSamples = $("#sampleInsert").find("tr:eq(1)");
        }
        //Mode iS view
        if ($("[id$=ViewStatus]").val() == "1") {
            //$("#divdummySamples").hide();
            //Hiding the template field
            // $("#grdPODetails th:last").hide();
            $("#grdMaterialSampleDetails").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();

            });
        }
        else {
            //$("#divdummySamples").hide();
            //$(tdsetSamples).insertBefore($("#grdMaterialSampleDetails").find("tr:eq(1)"));
        }
        GrandScriptUtils.DatePicker("ISV_RECEIVED_DATE", false, false);
    }
    //New End

    if (gridID == $("#grdVendorAddressDetails").attr("id")) {
        $("#grdVendorAddressDetails").find("tr").each(function () {
            if ($("[id$=ViewStatus]").val() == "1") {
                $(this).find("td:last input[id$=imbEdit]").hide();
                $(this).find("td:last input[id$=imbDelete]").hide();
                $(this).find("td:last input[id$=imbView]").show();
            }
            else {
                $(this).find("td:last input[id$=imbEdit]").show();
                $(this).find("td:last input[id$=imbDelete]").show();
                $(this).find("td:last input[id$=imbView]").hide();
            }
        });
    }

    if (gridID == $("#grdVendorLocalAddressDetails").attr("id")) {
        $("#grdVendorLocalAddressDetails").find("tr").each(function () {
            if ($("[id$=ViewStatus]").val() == "1") {
                $(this).find("td:last input[id$=ImbLocalAddrrEdit]").hide();
                $(this).find("td:last input[id$=ImbLocalAddrrDelete]").hide();
                $(this).find("td:last input[id$=ImbLocalAddrrView]").show();
            }
            else {
                $(this).find("td:last input[id$=ImbLocalAddrrEdit]").show();
                $(this).find("td:last input[id$=ImbLocalAddrrDelete]").show();
                $(this).find("td:last input[id$=ImbLocalAddrrView]").hide();
            }
        });
    }

    if (gridID == $("#grdVendorAddressDetails").attr("id")) {
        var addressType;
        var addressIndx = 0;
        var phoneNo;
        var phoneNoIndex = 0;
        var AddressBookType;
        var AddressBookTypeIndex = 0;
        var AddressBookContactPerson;
        var AddressBookContactPersonIndex = 0;


        $("#grdVendorAddressDetails").find("tr:has(td)").each(function () {
            addressIndx = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_ADDRESS_TYPE", gridID);
            addressType = GrandGrid.Utilities.GetColumnValue($(this), "VNC_DEFAULT", gridID);

            phoneNoIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_PHONE", gridID);
            phoneNo = GrandGrid.Utilities.GetColumnValue($(this), "VNC_PHONE", gridID);

            AddressBookTypeIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_TYPE_TEXT", gridID);
            AddressBookType = GrandGrid.Utilities.GetColumnValue($(this), "VNC_TYPE_TEXT", gridID);

            AddressBookContactPersonIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_CONT_NAME", gridID);
            AddressBookContactPerson = GrandGrid.Utilities.GetColumnValue($(this), "VNC_CONT_NAME", gridID);


            if (addressType == "1" || addressType == "True") {
                $(this).find("td:eq(" + addressIndx + ")").html("True");
            }
            else {
                $(this).find("td:eq(" + addressIndx + ")").html("False");
            }

            if (phoneNo == "null") {
                $(this).find("td:eq(" + phoneNoIndex + ")").html(" ");
            }

            if (AddressBookType == "undefined") {
                $(this).find("td:eq(" + AddressBookTypeIndex + ")").html(" ");
            }

            if (AddressBookContactPerson == "undefined" || AddressBookContactPerson == "null") {
                $(this).find("td:eq(" + AddressBookContactPersonIndex + ")").html(" ");
            }
        });
    }
    //    // grdVendorBankDetails Grid Bank Details
    if (gridID == $("#grdVendorBankDetails").attr("id")) {
        var ItemPK = GrandGrid.Utilities.GetColumnValue($(this), "VBD_PK", $(this).parents("table:first").attr("id"));
        ItemPK = parseInt(ItemPK) >= 0 ? ItemPK : 0;
        $("[id$=imbViewBankGrid]").each(function () {
            $(this).hide();
        });
        if ($("[id$=hdfViewMode]").val() == '1') {
            $('table[id=grdVendorBankDetails] input[type=image]').each(function () {
                $(this).hide();
            });
            $("[id$=imbViewBankGrid]").each(function () {
                $(this).show();
            });
        }
        $("#grdVendorBankDetails").find("tr:has(td)").each(function () {
            var countryIndx = GrandGrid.Utilities.GetColumnIndex($(this), "VBD_COUNTRY_NAME", gridID);
            var country = GrandGrid.Utilities.GetColumnValue($(this), "VBD_COUNTRY_NAME", gridID);
            if (country == "undefined" || country == "null") {
                $(this).find("td:eq(" + countryIndx + ")").html(" ");
            }
        });
    }

    /////////////////////////////////////////////////////////////////////////////////
    //if (gridID == $("#grdVendorLocalAddressDetails").attr("id")) {
    //    var ItemPK = GrandGrid.Utilities.GetColumnValue($(this), "VNC_LC_PK ", $(this).parents("table:first").attr("id"));
    //    ItemPK = parseInt(ItemPK) >= 0 ? ItemPK : 0;
    //    $("[id$=ImbLocalAddrrView]").each(function () {
    //        $(this).hide();
    //    });
    //    if ($("[id$=hdfViewMode]").val() == '1') {
    //        $('table[id=grdVendorLocalAddressDetails] input[type=image]').each(function () {
    //            $(this).hide();
    //        });
    //        $("[id$=ImbLocalAddrrView]").each(function () {
    //            $(this).show();
    //        });
    //    }
    //    $("#grdVendorLocalAddressDetails").find("tr:has(td)").each(function () {
    //        var countryIndx = GrandGrid.Utilities.GetColumnIndex($(this), "VNC_LC_CNTRY", gridID);
    //        var country = GrandGrid.Utilities.GetColumnValue($(this), "VNC_LC_CNTRY", gridID);
    //        if (country == "undefined" || country == "null") {
    //            $(this).find("td:eq(" + countryIndx + ")").html(" ");
    //        }
    //    });
   // }



}

//NewMaterial start
function ShowRateHistory(itemPK, vendorPK) {
    //<summary>function used to add the item tax details</summary>
    ///////////////
    ajaxurl = vendorRegistration.GetRateHistory + $("[id$=BizUnitPk]").val() + "&ItemPK=" + itemPK + "&VendorPK=" + vendorPK;
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(ajaxurl, function (data) {
        if (data.length > 0) {
            //            $("[id$=ItemCode]").text(data[0].VIH_ITEM_CODE);
            //            $("[id$=ItemName]").text(data[0].VIH_ITEM_NAME);
            $("[id$=ItemCode]").html(data[0].VIH_ITEM_CODE.length > 30 ? data[0].VIH_ITEM_CODE.substr(0, 30) + "..." : data[0].VIH_ITEM_CODE);
            $("[id$=ItemCode]").attr('title', data[0].VIH_ITEM_CODE);
            $("[id$=ItemName]").html(data[0].VIH_ITEM_NAME.length > 30 ? data[0].VIH_ITEM_NAME.substr(0, 30) + "..." : data[0].VIH_ITEM_NAME);
            $("[id$=ItemName]").attr('title', data[0].VIH_ITEM_NAME);
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, data);
            $("#divRateHistory").dialog(
        {
            width: 540,
            title: "Translate(RateHistory)"
        });
        }
        else {
            //            $("[id$=ItemCode]").text("");
            //            $("[id$=ItemName]").text("");
            $("[id$=ItemCode]").attr('title', "");
            $("[id$=ItemName]").attr('title', "");
            GrandGrid.MakeGrid($("#grdRateDetails"), 0, new Array());
            GrandScriptUtils.ShowModal("Translate(NoRateHistory)");
        }
    });
}
var VenNumber;
function SavePage(command, Type) {

    // Currency confirmation region (If Vendor Type is "Local", then the currency should be base currency.If not - show a Confirmation message "Do you want to continue ?".
    if ($("select[id$=VEN_PO_TYPE]").val() == "2" || $("[id$=hdfResetCurrencyOnType]").val() == "0") { //2=> "Local" .In the case of EKK,hdfResetCurrencyOnType.val()==0
        if ($("[id$=hdfIscontYes]").val() == "0") {
            if (parseInt($("select[id$=VEN_CURRENCY]").val()) != parseInt($("[id$=hdfBaseCurrency]").val())) {
                ShowDifferentCurrencyConfirm(command);
                return false;
            }
        }
    }
    //If vendor currency is base currency, and change PO type to Import, then need to confirm.
    if ($("[id$=hdfIsCurrencyWithTypeValidnReqd]").val() == "1") {
        if (parseInt($("select[id$=VEN_CURRENCY]").val()) == parseInt($("[id$=hdfBaseCurrency]").val())) {
            if ($("select[id$=VEN_PO_TYPE]").val() != vendorRegistration.Local && $("[id$=hdfCurrncyChangeConfirm]").val() == "0") {
                ConfirmCurrencyTypeChange(command,Type);
                return false;
            }
        }
    }
//    //If vendor currency is not base currency, and change PO type to Local, then need to confirm.
//    if (parseInt($("select[id$=VEN_CURRENCY]").val()) != parseInt($("[id$=hdfBaseCurrency]").val())) {
//        if ($("select[id$=VEN_PO_TYPE]").val() != vendorRegistration.Import && $("[id$=hdfIscontYes]").val() == "0") {
//            ShowDifferentCurrencyConfirm(vendorRegistration.Import);
//           // ConfirmCurrencyTypeChange()
//            return false;
//        }
//    }
    //End Currency confirmation

    $("[id$=imbSave]").hide();
    //    if ($("input[id$=COA_TEXT]").val().trim() == "" || $("input[id$=COA_TEXT]").val().toLowerCase() == "select/type") {
    //        $("input[id$=VEN_ACCOUNT]").val("0");
    //    }
    AddValidations(1);
    $("select[id$=VEN_TYPE]").attr("disabled", false);
    $("[id$=VEN_CODE]").removeAttr("disabled");
    if ($(document.forms[0]).valid()) {
        SetItemTaxDetails();
        GetVendorTerms();
        //  RemoveValidation();

        var ObjVendor = $("#divVendorData").data("VendorData");
        if ($("[id$=ActionID]").val() == "2") {
            ObjVendor.VEN_TYPE == "1";
        }
        $("[id$=AddressBookDetails]").val(JSON.stringify(ObjVendor.AddressBookDetails));
        $("[id$=MaterialDetails]").val(JSON.stringify(ObjVendor.MaterialDetails));
        $("[id$=TermsDetails]").val(JSON.stringify(ObjVendor.TermsDetails));
        $("[id$=MaterialSamples]").val(JSON.stringify(ObjVendor.MaterialSamples));
        // For File Upload---------------------------------------------------------------------
        var ObjFile = $("#divFileData").data("FileData");
        $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
        // For File Upload---------------------------------------------------------------------

        if (command != "Draft") {
            $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
        }
        else {
            $("[id$=ActionID]").val('0');
        }
        $("[id$=VEN_HAS_ISO]").val($("[id$=VEN_HAS_ISO1]").val() == "on" ? "true" : "false");
        $("[id$=VEN_PAY_FOR_VENDOR]").val($("[id$=VEN_PAY_FOR_VENDOR1]").val() == "on" ? "true" : "false");

        //Add VEN_ACTIVE
        var ActiveInactive;
        ActiveInactive = ($("[id$=VEN_ACTIVE1]").attr("checked")) ? "1" : "0";
        $("[id$=VEN_ACTIVE]").val(ActiveInactive);

        if (ActiveInactive == "1") {
            $("[id$=VEN_COMMENTS]").val('');
        }
        if ($("[id$=VEN_COMMISSION]").val() == "") {
            $("[id$=VEN_COMMISSION]").val("0.00");
        }

        $("[id$=VEN_NAME]").removeAttr("disabled");
        $("[id$=VEN_NAME]").removeClass('input-disabled');

        $("[id$=Roles]").val(JSON.stringify(GetSelectedRoles()));
        var Code = $("[id$=VEN_CODE]").val();
        VenNumber = ($("[id$=VEN_CODE]").val().replace('[', "").replace(']', ""));
        $("[id$=VEN_CODE]").val(VenNumber);
        var jSonString = GrandScriptUtils.FormToJsonString("FormData");
        $("[id$=VEN_CODE]").val(Code)

       




        //To Prevent Duplicate Submission
        if ($("[id$=SubmitFlag]").val() == "0")
            $("[id$=SubmitFlag]").val('1')
        else
            return false;

        $.post(vendorRegistration.SaveVendorDetails, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully


            if (parseInt(data[0]) == 0) {
                GrandScriptUtils.ShowModal(vendorRegistration.VendorCodeExist, vendorRegistration.Information, vendorRegistration.CodeExist);
                $("[id$=SubmitFlag]").val('0')
            }
          
            else if (parseInt(data[0]) > 0) {
                $("[id$=VEN_PK]").val(data[0]);
                if (command == "Draft") {
                    $("[id$=hdfVendorPK]").val(data[0])
                    if (Type == "AddMaterial") {
                        AddMaterials();
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (Type == "AddBank") {
                        AddBank();
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (Type == "AddLocalAddress") {
                        AddLocalAddress();
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else {
                        var SaveMessageWithVenCode;
                        if (data[1] == '') {
                            GrandScriptUtils.ShowModal(vendorRegistration.VendorSaveMessage, vendorRegistration.Information, vendorRegistration.SaveCommand);
                        }
                        else {
                            SaveMessageWithVenCode = vendorRegistration.VendorSavedMessage1 + " " + data[1] + " " + vendorRegistration.VendorSavedMessage2;
                            GrandScriptUtils.ShowModal(SaveMessageWithVenCode, vendorRegistration.Information, vendorRegistration.SaveCommand);
                        }
                    }
                }
                else {
                    $("[id$=hdfAppID]").val(data[0]);
                    SaveWorkFlow();
                    // GrandScriptUtils.ShowModal(vendorRegistration.VendorSaveMessage, vendorRegistration.Information, vendorRegistration.SaveCommand);
                }
                $("[id$=hdfVendorPK]").val(data[0])
                $("[id$=hdfVenCode]").val(data[1])
            }
            else {

                GrandScriptUtils.ShowModal(vendorRegistration.ActionFailedMessage);
                $("[id$=SubmitFlag]").val('0')
            }
        });
    }
    $("select[id$=VEN_TYPE]").attr("disabled", true);
    $("[id$=imbSave]").show();
    if ($("[id$=VENCODE_AUTO]").val() == "1")
        $("[id$=VEN_CODE]").attr("disabled", "disabled");

    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>Function used to show meassage</summary> 
    var SaveMessageWithVenCode = vendorRegistration.VendorSavedMessage1 + " " + $("[id$=hdfVenCode]").val() + " " + vendorRegistration.VendorSubmitMessage2;
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        GrandScriptUtils.ShowModal(SaveMessageWithVenCode, vendorRegistration.Information, vendorRegistration.INBOX);
    } else {
        GrandScriptUtils.ShowModal(SaveMessageWithVenCode, vendorRegistration.Information, vendorRegistration.SaveCommand);
    }
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        case vendorRegistration.SaveCommand: //comment req
            window.location = vendorRegistration.VendorListing;
            break;
        case vendorRegistration.CodeExist: //Commend When calling 
            break;
        case vendorRegistration.DeleteAddress:
            DeleteAddress();
            break;
        case vendorRegistration.DeleteMaterial:
            DeleteMaterial();
            break;
        //NewSamples                                    
        case vendorRegistration.DeleteSamples:
            DeleteSamples();
            break;
        //New End                          
        case vendorRegistration.TaxDelete:
            DeleteTaxDetails();
            break;
        case vendorRegistration.TaxDeleteVendor:
            deleteVendorTaxDetails();
            break;
        case vendorRegistration.DeleteBank:
            DeleteBank();
            break;
        case vendorRegistration.DeleteLcAddress:
            DeleteLocalAddress();
            break;
        case vendorRegistration.INBOX:
            window.location = vendorRegistration.InboxURL;
            break;
    }
    return false;
}
///#endregion

///#region validation Section
function AddValidations(mode) {
    ////<summary>function used validate each sections </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 vendor 2 address book 3 material 
    /// </param>
    RemoveValidation();
    if (mode == 1) {
        $("input[id$=VEN_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqVendorName)" }
        });
        if ($("[id$=VENCODE_AUTO]").val() == "0") {
            $("input[id$=VEN_CODE]").rules("add", {
                required: true,
                maxlength: 100,
                messages: { required: "Translate(ReqVendorCode)" }
            });
        }
        $("input[id$=VEN_DATE]").rules("add", {
            required: true,
            messages: { required: "Translate(ReqVendorDate)" }
        });
        $("select[id$=VEN_CURRENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectCurrency)" }
        });
        $("select[id$=VEN_ACCOUNT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectAccount)" }
        });
        $("select[id$=VEN_ADP_ACCOUNT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectAccount)" }
        });

        $("select[id$=VEN_PO_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectVendorPOType)" }
        });
        $("input[id$=VEN_EMAIL]").rules("add", {
            email: true,
            messages: { email: "Translate(RegEmail)" }
        });
        $("input[id$=VEN_ANNUAL_SALES]").rules("add", {

            ThreeDecimal: true,
            maxlength: 14,
            messages: { required: "Translate(ReqAnnuanSales)" }
        });

        $("input[id$=VEN_CREDIT_DAYS]").rules("add", {
            digits: true,
            range: [0, 999],
            messages: { digits: "Translate(ErrCreditDays)" }
        });
        $("select[id$=VEN_CNTRY]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ProvideCountry)" }
        });

    }
    else if (mode == 2) {
        $("input[id$=VNC_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqVendorTitle)" }
        });

        $("input[id$=VNC_EMAIL]").rules("add", {
            email: true,
            messages: { email: "Translate(RegEmail)" }
        });
    }
    else if (mode == 3) {
        $("select[id$=VEN_CURRENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectCurrency)" }
        });

        $("select[id$=MaterialType]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectCategory)" }
        });
        //        $("select[id$=ITV_ITEM]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectItemCode)" }
        //        });
        $("[id$=MaterialItem]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItem)" }
        });
        //        $("input[id$=ITV_NAME]").rules("add", {
        //            required: true,
        //            maxlength: 100,
        //            messages: { required: "Translate(ReqVendorMaterial)" }
        //        });

        $("input[id$=ITV_PRICE]").rules("add", {
            required: false,
            DecimalDigits: RateDec,
            CustomDecimal: true,
            maxlength: 14,
            messages: { required: "Translate(ReqStdPrice)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", RateDec) }
        });
        if ($("select[id$=VEN_CURRENCY]").lenght > 0)
            $("select[id$=ITV_CURRENCY]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Translate(ReqCurrency)" }
            });
        if ($("[id$='ITV_MOQ']").val() != '');
        $("input[id$=ITV_MOQ]").rules("add", {
            //           required: true,
            //           NonZero: true,
            DecimalDigits: QtyDec,
            CustomDecimalIncludeZero: true, //CustomDecimal: true,
            maxlength: 14,
            messages: { CustomDecimalIncludeZero: String.format("Translate(MsgValidDecimalNo)", QtyDec) }
        });
        //required: "Translate(ReqMOQ)",

        $("select[id$=ITV_MOQ_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseSelectUnit)" }
        });
        $("input[id$=ITV_LEAD_TIME]").rules("add", {
            required: true,
            digits: true,
            range: [0, 999],
            messages: { required: "Translate(LeadDays)" }
        });
    }
    //NewSamples
    else if (mode == 4) {
        $("select[id$=MaterialTypeSamples]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqMaterialType)" }
        });

        $("[id$=txtSampleItem]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(ReqItem)" }
        });

        $("input[id$=ISV_RECEIVED_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(ReqRcvdDate)" }
        });

        $("input[id$=ISV_QC_TEST]").rules("add", {
            required: true,
            messages: { required: "Translate(ReqQCTest)" }
        });

        $("input[id$=ISV_QC_VALUE]").rules("add", {
            required: true,
            messages: { required: "Translate(ReqQCValue)" }
        });

        $("input[id$=ISV_REMARKS]").rules("add", {
            required: true,
            messages: { required: "Translate(ReqRemarks)" }
        });

        $("input[id$=ISV_QUANTITY]").rules("add", {
            required: true,
            TwoDecimal: QtyDec == 2 ? true : false,
            ThreeDecimal: QtyDec == 3 ? true : false,
            maxlength: 14,
            messages: { required: "Translate(EnterQty)" }
        });

    }
    //New  End
    else if (mode == 5) {
        //        $("[id$=ChooseTax]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectType)" }
        //        });
    }
    else if (mode == 6) { // Bank Add
        $("input[id$=VBD_NAME]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqBankTitle)" }
        });
    }
    else if (mode == 7) {
        $("input[id$=VNC_LC_TITTLE]").rules("add", {
            required: true,
            maxlength: 200,
            messages: { required: "Translate(ReqVendorTitle)" }
        });
    }
}

function RemoveAllValidations() {
    ////<summary>function used Remove validation before Upload the File </summary>
    RemoveValidation();
}

function RemoveValidation() {
    ////<summary>function remove all added validations </summary>
    /// <param name="mode"  type="Object">
    /// Determins which session to validate if 1 vendor 2 address book 3 material 
    /// </param>
    // $("input[id$=COA_WHT_TEXT]").rules("remove"); 
    $("input[id$=VEN_NAME]").rules("remove");
    $("input[id$=MaterialItem]").rules("remove");
    $("input[id$=VEN_CONT_NAME]").rules("remove");
    $("input[id$=VEN_CODE]").rules("remove");
    $("input[id$=VEN_DATE]").rules("remove");
    $("input[id$=VEN_PHONE]").rules("remove");
    if ($("select[id$=VEN_CURRENCY]").lenght > 0)
        $("select[id$=VEN_CURRENCY]").rules("remove");
    $("input[id$=VNC_NAME]").rules("remove");
    $("input[id$=VNC_CONT_NAME]").rules("remove");
    $("input[id$=VNC_PHONE]").rules("remove");
    $("select[id$=MaterialType]").rules("remove");
    //    $("select[id$=ITV_ITEM]").rules("remove");
    // $("input[id$=ITV_NAME]").rules("remove");
    $("input[id$=ITV_PRICE]").rules("remove");
    $("select[id$=ITV_CURRENCY]").rules("remove");
    $("select[id$=VEN_PO_TYPE]").rules("remove"); 
    $("select[id$=VEN_ACCOUNT]").rules("remove");
    $("select[id$=VEN_ADP_ACCOUNT]").rules("remove");
    $("input[id$=ITV_MOQ]").rules("remove");
    $("select[id$=ITV_MOQ_UOM]").rules("remove");
    //NewMaterial start
    //    $("input[id$=ITV_DISC_PERC]").rules("remove");
    $("input[id$=ITV_LEAD_TIME]").rules("remove");
    //new End
    //NewMaterial start
    //$("input[id$=ITV_TAX_PERC]").rules("remove");
    //New End
    $("input[id$=VEN_ADDR1]").rules("remove");
    $("input[id$=VEN_TIN]").rules("remove");
    $("input[id$=VEN_ANNUAL_SALES]").rules("remove");

    //NewSamples Start
    $("select[id$=MaterialTypeSamples]").rules("remove");
    $("input[id$=txtSampleItem]").rules("remove");
    $("input[id$=ISV_RECEIVED_DATE]").rules("remove");
    $("input[id$=ISV_QC_TEST]").rules("remove");
    $("input[id$=ISV_QC_VALUE]").rules("remove");
    $("input[id$=ISV_REMARKS]").rules("remove");
    $("input[id$=ISV_QUANTITY]").rules("remove");
    //New End
    //$("[id$=ChooseTax]").rules("remove");
    //$("[id$=VBD_NAME]").rules("remove");
    $("input[id$=VBD_NAME]").rules("remove");
     $("input[id$=VNC_LC_TITTLE]").rules("remove");

}

function FillTaxDiscount(category) {
    ///<summary>function To Fill tax Details </summary>
    var drpID;
    drpID = $("[id$=ChooseTax]").attr("id");
    var reqString = vendorRegistration.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDue=0&ISPURCHASE=1";
    $.get(reqString, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function AddLineItemTax(slNo, ItemPK) {
    //<summary>function used to add the item tax details</summary>   
    $("[id$='hdfTaxPopUpByVendor']").val('0');
    $("#divTaxDataVendor").data("TaxObj", '');
    RemoveAllValidations();
    FillTaxDiscount(1);
    materialID = ItemPK;
    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });
    vendorRegistration.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(1);
    ClearPopUp(slNo, 1, ItemPK);
    ClearTaxDetails();
}

function AddLineItemDiscount(slNo, itemPK) {
    //<summary>function used to add the item discount details</summary>
    $("#divTaxDataVendor").data("TaxObj", '');
    RemoveAllValidations();
    materialID = itemPK;
    FillTaxDiscount(3);

    $("#divItemTax").dialog("open");
    $("#divItemTax").dialog(
    {
        width: 540,
        title: "Translate(DiscountDetails)"
    });
    vendorRegistration.ItemTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(3);
    ClearPopUp(slNo, 3, itemPK);
    ClearTaxDetails();
}

function ClearPopUp(slNo, type, itemPK) {
    //<summary>function used to bind the item tax/ discount details</summary>
    var TaxDetails = new Array();
    var ObjVendor = $("#divVendorData").data("VendorData");
    $.get(vendorRegistration.GetVndrTaxDiscountDetails + itemPK + "&Category=" + type, function (data) {
        if (data.TaxDetail) {
            var category = parseInt($("[id$=hdfTaxCategory]").val());
            data.BIZUNIT_PK = ObjVendor.BizUnitPk;
            data.USER_PK = ObjVendor.UserPk;
            data.ITV_PK = materialID;
            data.ITV_TAX_CATEGORY = category;
            $("#divTaxData").data("TaxObj", data);
            if (data.TaxDetail.length > 0) {
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, data.TaxDetail);
            }
            else {
                var TaxObj = new Object();
                TaxObj.IVT_SL_NO = data.TaxDetail.IVT_SL_NO;
                TaxObj.IVT_PK = data.TaxDetail.IVT_PK;
                TaxObj.IVT_TAX = data.TaxDetail.IVT_TAX;
                TaxObj.IVT_TAX_TEXT = data.TaxDetail.IVT_TAX_TEXT;
                TaxObj.IVT_TAX_CATEGORY = data.TaxDetail.IVT_TAX_CATEGORY;
                TaxDetails.push(TaxObj);
                data.TaxDetail = TaxDetails;
                $("#divTaxData").data("TaxObj", data);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, TaxDetails);
            }
        }
        else {
            GrandGrid.MakeGrid($("#grdTaxDetails"), 0, new Array());
            $("#divTaxData").data("TaxObj", new Array());
        }
    });

    // GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(type, itemPK));
}

//function GetTaxDiscountDetails(slNo, type) {
//    //<summary>function used to get the tax / discount details </summary>
//    var TaxDetails = $("#divData").data("TaxDetails");
//    var TaxArray = new Array();
//    for (var i in TaxDetails) {
//        if (TaxDetails[i].IVT_SL_NO == slNo && TaxDetails[i].IVT_TAX_CATEGORY == type) {
//            TaxArray.push(TaxDetails[i]);
//        }
//    }
//    return TaxArray;
//}

function GetTaxDiscountDetails(type, itemPK) {
    //<summary>function used to get the tax / discount details </summary>
    var TaxArray = new Array();
    $.get(vendorRegistration.GetVndrTaxDiscountDetails + itemPK + "&Category=" + type, function (data) {
        if (data.TaxDetail) {
            for (var i in data.TaxDetail) {
                TaxArray.push(TaxDetail[i]);
            }
        }
    });
    return TaxArray;

    for (var i in TaxDetails) {
        if (TaxDetails[i].IVT_SL_NO == slNo && TaxDetails[i].IVT_TAX_CATEGORY == type) {
            TaxArray.push(TaxDetails[i]);
        }
    }
    return TaxArray;
}



function ClearTaxDetails() {
    //<summary>function used to clear the tax/ discount details</summary>
    vendorRegistration.EditTax = 0;
    $("[id$=ChooseTax]").val("0");
}

function SaveTaxDiscount() {
    //<summary>function to save tax/discount details</summary>
    // var TaxDetails = $("#divData").data("TaxDetails");
    var ObjTaxDetails = $("#divTaxData").data("TaxObj");
    var TaxDetails = TaxDetails = new Array();
    if (ObjTaxDetails.TaxDetail) {
        if (ObjTaxDetails.TaxDetail.length > 0)
            for (var i in ObjTaxDetails.TaxDetail) {
                TaxDetails.push(ObjTaxDetails.TaxDetail[i]);
            }
        else {
            var TaxObj = new Object();
            TaxObj.IVT_SL_NO = ObjTaxDetails.TaxDetail.IVT_SL_NO;
            TaxObj.IVT_PK = ObjTaxDetails.TaxDetail.IVT_PK;
            TaxObj.IVT_TAX = ObjTaxDetails.TaxDetail.IVT_TAX;
            TaxObj.IVT_TAX_TEXT = ObjTaxDetails.TaxDetail.IVT_TAX_TEXT;
            TaxObj.IVT_TAX_CATEGORY = ObjTaxDetails.TaxDetail.IVT_TAX_CATEGORY;
            TaxDetails.push(TaxObj);
        }
    }
    //   TaxDetails = ObjTaxDetails.TaxDetail;


    var slNo = parseInt(vendorRegistration.ItemTaxPK);
    var itemTax = parseInt($("[id$=ChooseTax]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    AddValidations(5);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (vendorRegistration.EditTax == 0) {
                    if (TaxDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                    .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax && tax.IVT_TAX_CATEGORY == category; })
                                    .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        // TaxDetailsObj.IVT_PK = materialID;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(vendorRegistration.TypeAlreadyAdded, vendorRegistration.Information);
                    }
                }
                else {
                    if (vendorRegistration.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                        .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == vendorRegistration.EditTax; })
                                        .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.IVT_TAX = itemTax;
                            TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTax] :selected").text());
                            TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                          .Where(function (tax) { return tax.IVT_SL_NO == slNo && tax.IVT_TAX == itemTax; })
                          .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(vendorRegistration.TypeAlreadyAdded, vendorRegistration.Information);
                        }
                    }
                }

                var ObjVendor = $("#divVendorData").data("VendorData");

                var TaxArray = new Array();
                for (var i in TaxDetails) {
                    TaxArray.push(TaxDetails[i]);
                }
                var ObjTax = new Object();
                ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
                ObjTax.USER_PK = ObjVendor.UserPk;
                ObjTax.ITV_PK = materialID;
                ObjTax.ITV_TAX_CATEGORY = category;
                ObjTax.TaxDetail = TaxDetails;
                $("#divTaxData").data("TaxObj", ObjTax);
                //  var taxList = JSON.stringify(ObjTax);
                //    $("#divData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetails"), 0, ObjTax.TaxDetail);
            }
            ClearTaxDetails();
        }
    }
    return false;
}

function SaveApply() {
    //<summary>function used to clear the tax/ discount details</summary>
    var TaxDetails = $("#divData").data("TaxDetails");
    var ObjTax = $("#divTaxData").data("TaxObj");
    var taxList = JSON.stringify(ObjTax);
    $.post(vendorRegistration.SaveTaxDiscount, taxList, function (data) {
        if (data) {

        }
    });
    $("#divItemTax").dialog("close");
    $("[id$=hdfIsMaterialTax]").val('1');
    checkHideVendorTaxPopUpImage();
    return false;
}

function GridHandler(tr, command) {
    ////<summary>function used handle Grid Events </summary>
    switch (command.toString().toUpperCase()) {
        case vendorRegistration.TaxDelete:
            vendorRegistration.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.IVT_SL_NO, $(tr).parents("table:first").attr("id"));
            vendorRegistration.EditTax = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.IVT_TAX, $(tr).parents("table:first").attr("id"));
            var slNo = $("[id$=hdnSlNo]").val();
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.TaxDelete, true);
            break;
        case vendorRegistration.TaxDeleteVendor:
            vendorRegistration.ItemTaxPK = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.IVT_SL_NO, $(tr).parents("table:first").attr("id"));
            vendorRegistration.EditTax = GrandGrid.Utilities.GetColumnValue(tr, vendorRegistration.IVT_TAX, $(tr).parents("table:first").attr("id"));
            var slNo = $("[id$=hdnSlNo]").val();
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.TaxDeleteVendor, true);
            break;
    }
    return false;
}

function DeleteTaxDetails() {
    //<summary>function used to delete the tax details</summary>    
    //var TaxDetails = $("#divData").data("TaxDetails");
    var taxObj = $("#divTaxData").data("TaxObj");
    var TaxDetails = taxObj.TaxDetail;
    for (var i in TaxDetails) {
        if (vendorRegistration.EditTax > 0) {
            if (TaxDetails[i].IVT_SL_NO == vendorRegistration.ItemTaxPK && TaxDetails[i].IVT_TAX == vendorRegistration.EditTax) {
                TaxDetails.splice(i, 1);
            }
        }
    }
    //  $("#divData").data("TaxDetails", TaxDetails);
    taxObj.TaxDetail = TaxDetails;
    $("#divTaxData").data("TaxObj", taxObj);
    // GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(vendorRegistration.ItemTaxPK, $("[id$=hdfTaxCategory]").val(), true));
    GrandGrid.MakeGrid($("#grdTaxDetails"), 0, TaxDetails);

    vendorRegistration.EditTax = 0;
}

function SetItemTaxDetails() {
    //<summary>function used to get the item tax details</summary>
    var ObjVendor = $("#divVendorData").data("VendorData");
    var TaxDetails = $("#divData").data("TaxDetails");
    for (var itv in ObjVendor.MaterialDetails) {
        slNo = ObjVendor.MaterialDetails[itv].ITV_SL_NO;
        var TaxArray = new Array();
        for (var i in TaxDetails) {
            if (TaxDetails[i].IVT_SL_NO == slNo) {
                TaxArray.push(TaxDetails[i]);
            }
        }
        ObjVendor.MaterialDetails[itv].TaxDtl = TaxArray;
    }
}
///#endregion

function AddVendorItemTax(slNo, ItemPK) {
    //<summary>function used to add the item tax details</summary>    
    RemoveAllValidations();
    RemoveValidation();
    FillTaxDiscountVendor(1);
    $("[id$='hdfTaxPopUpByVendor']").val('1');
    //    if (!$("#divTaxDataVendor").data("TaxObj")) {
    //        $("#divTaxDataVendor").data("TaxObj", '');
    //    }    

    // materialID = ItemPK;
    $("#divVendorTax").dialog("open");
    $("#divVendorTax").dialog(
        {
            width: 540,
            title: "Translate(TaxDetails)"
        });

    vendorRegistration.VendorTaxPK = slNo;
    $("[id$=hdnSlNo]").val(slNo);
    $("[id$=hdfTaxCategory]").val(1);
    var category = parseInt($("[id$=hdfTaxCategory]").val());

    $("[id$=ChooseTaxVendor]").val("0"); // ClearTaxDetails();
    var t = $("[id$=TaxHdr]").val();
    var TaxArray = jQuery.parseJSON(t);
    if (!TaxArray) {
        TaxArray = new Array();
    }
    var ObjVendor = $("#divVendorData").data("VendorData");
    // var TaxArray = new Array();
    var ObjTax = new Object();
    ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
    ObjTax.USER_PK = ObjVendor.UserPk;
    ObjTax.ITV_PK = materialID;
    ObjTax.ITV_TAX_CATEGORY = category;
    ObjTax.TaxDetail = TaxArray;
    $("#divTaxDataVendor").data("TaxObj", ObjTax);
    ClearPopUpVendorTax(slNo, 1, ItemPK);
    if ($("[id$=ViewStatus]").val() == "1") {
        $("[id$=imbTaxDiscountSaveVendor]").hide();
        $("[id$=btnApplyVendorTax]").hide();
    }
    return false;
}

function FillTaxDiscountVendor(category) {
    ///<summary>function To Fill tax Details </summary>
    var drpID;
    drpID = $("[id$=ChooseTaxVendor]").attr("id");
    var reqString = vendorRegistration.GetCategoryTaxDiscountDateBase + category + "&Active=1" + "&TaxDue=0&ISPURCHASE=1";
    $.get(reqString, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function ClearPopUpVendorTax(slNo, type, itemPK) {
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, ObjTaxDetails.TaxDetail);
}

function AddTaxDiscountVendorToGrid() {
    //<summary>function to save tax/discount details for Vendor</summary>  
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    var TaxDetails = new Array();
    if (ObjTaxDetails.TaxDetail) {
        if (ObjTaxDetails.TaxDetail.length > 0)
            for (var i in ObjTaxDetails.TaxDetail) {
                TaxDetails.push(ObjTaxDetails.TaxDetail[i]);
            }

    }
    //   TaxDetails = ObjTaxDetails.TaxDetail;


    var slNo = parseInt(vendorRegistration.VendorTaxPK);
    var itemTax = parseInt($("[id$=ChooseTaxVendor]").val());
    var category = parseInt($("[id$=hdfTaxCategory]").val());
    // AddValidations(5);
    var TaxDetailsObj = null;
    if ($(document.forms[0]).valid()) {
        if (itemTax > 0) {
            if (slNo > 0) {
                if (vendorRegistration.EditTax == 0) {
                    if (TaxDetails.length > 0) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                    .Where(function (tax) { return tax.IVT_TAX == itemTax; })
                                    .FirstOrDefault(null);
                    }
                    if (TaxDetailsObj == null) {
                        TaxDetailsObj = new Object();
                        TaxDetailsObj.IVT_SL_NO = slNo;
                        TaxDetailsObj.IVT_PK = 0;
                        // TaxDetailsObj.IVT_PK = materialID;
                        TaxDetailsObj.IVT_TAX = parseInt(itemTax) > 0 ? itemTax : 0;
                        TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTaxVendor] :selected").text());
                        TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        TaxDetails.push(TaxDetailsObj);
                    }
                    else {
                        GrandScriptUtils.ShowModal(vendorRegistration.TypeAlreadyAdded, vendorRegistration.Information);
                    }
                }
                else {
                    if (vendorRegistration.EditTax == itemTax) {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                                        .Where(function (tax) { return tax.IVT_TAX == vendorRegistration.EditTax; })
                                        .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            TaxDetailsObj.IVT_TAX = itemTax;
                            TaxDetailsObj.IVT_TAX_TEXT = $.trim($("[id$=ChooseTaxVendor] :selected").text());
                            TaxDetailsObj.IVT_TAX_CATEGORY = category;
                        }
                    }
                    else {
                        TaxDetailsObj = JSLINQ(TaxDetails)
                          .Where(function (tax) { return tax.IVT_TAX == itemTax; })
                          .FirstOrDefault(null);
                        if (TaxDetailsObj != null) {
                            GrandScriptUtils.ShowModal(vendorRegistration.TypeAlreadyAdded, vendorRegistration.Information);
                        }
                    }
                }

                var ObjVendor = $("#divVendorData").data("VendorData");

                var TaxArray = new Array();
                for (var i in TaxDetails) {
                    TaxArray.push(TaxDetails[i]);
                }
                var ObjTax = new Object();
                ObjTax.BIZUNIT_PK = ObjVendor.BizUnitPk;
                ObjTax.USER_PK = ObjVendor.UserPk;
                ObjTax.ITV_PK = materialID;
                ObjTax.ITV_TAX_CATEGORY = category;
                ObjTax.TaxDetail = TaxDetails;
                $("#divTaxDataVendor").data("TaxObj", ObjTax);
                //  var taxList = JSON.stringify(ObjTax);
                //    $("#divData").data("TaxDetails", TaxDetails);
                GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, ObjTax.TaxDetail);
            }

            vendorRegistration.EditTax = 0;
            $("[id$=ChooseTaxVendor]").val("0");
        }
    }
    return false;
}

function SaveApplyVendorTax() {
    var ObjTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    var v = JSON.stringify(ObjTaxDetails.TaxDetail);
    $("[id$=TaxHdr]").val(v);
    $("#divVendorTax").dialog("close");
    $("[id$=hdfIsVendorTax]").val('1')
    checkHideMaterialTaxPopUpImage();
    var taxText = '';
    for (i = 0; i < ObjTaxDetails.TaxDetail.length; i++) {
        taxText += ObjTaxDetails.TaxDetail[i].IVT_TAX_TEXT + ", ";
    }
    taxText = taxText.substring(0, taxText.length - 2);
    $("[id$=TaxByVendor]").attr('title', taxText);
    if (taxText.length > 35) {
        taxText = taxText.substring(0, 35);
        taxText += '...';
    }
    $("[id$=TaxByVendor]").text(taxText);
}


////Bind PendingPO Grid Start
//function BindPendingPOGrid() {
//    ///<summary>To handle bind PendingPOGrid </summary>  
//    var materialID = $("[id$=MaterialPK]").val();
//    $.get(PurchaseRequestConfig.GetPendingPODetails + materialID, function (data) {
//        GrandGrid.Utilities.ResetGrid(true, "grdPendingPOList");
//        GrandGrid.MakeGrid($("#grdPendingPOList"), 1, data);
//    });

//}

function convertDate(date) {
    // 2014-09-21T00:00:00 to 01-Sep-2014
    var months = { '01': 'Jan', '02': 'Feb', '03': 'Mar', '04': 'Apr', '05': 'May', '06': 'Jun', '07': 'Jul', '08': 'Aug', '09': 'Sep', '10': 'Oct', '11': 'Nov', '12': 'Dec' };
    var splitArr = date.split('-'); // split based on '-'
    var yyyy = splitArr[0]; // Year
    var mm = months[splitArr[1]]; // convert month into lower
    var splitArr2 = splitArr[2].split('T');
    var dd = splitArr2[0];
    if (dd < 10) dd = dd; // add '0' if date is less then 10.
    return [dd, mm, yyyy].join('-'); // join value according to format.
}

function checkHideVendorTaxPopUpImage() {
    if ($("[id$=hdfIsMaterialTax]").val() == '1') {
        $("img[purpose='VendorTax']").hide();
    }
}
function checkHideMaterialTaxPopUpImage() {
    if ($("[id$=hdfIsVendorTax]").val() == '1') {
        $("img[purpose='MaterialTax']").hide();
    }

}
function deleteVendorTaxDetails() {
    //<summary>function used to delete the tax details</summary>
    var objTaxDetails = $("#divTaxDataVendor").data("TaxObj");
    for (var i in objTaxDetails.TaxDetail) {
        if (vendorRegistration.EditTax > 0) {
            if (objTaxDetails.TaxDetail[i].IVT_TAX == vendorRegistration.EditTax) {
                objTaxDetails.TaxDetail.splice(i, 1);
                $("#divTaxDataVendor").data("TaxObj", objTaxDetails);
                var TaxDetails1 = $("#divTaxDataVendor").data("TaxObj");
            }
        }
    }
    $("#divTaxDataVendor").data("TaxObj", objTaxDetails);
    //  GrandGrid.MakeGrid($("#grdTaxDetails"), 0, GetTaxDiscountDetails(vendorRegistration.ItemTaxPK, $("[id$=hdfTaxCategory]").val(), true));
    GrandGrid.MakeGrid($("#grdTaxDetailsVendor"), 0, objTaxDetails.TaxDetail);
    vendorRegistration.EditTax = 0;
}

// Bank Tab -- Begins

function bindBankTab() {
    // bindBankCountry();
    bindAccountTypes();

    $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');
    $("select[id$=VBD_ACCOUNT_TYPE]").change(function () {
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").val('')
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');

        var value = $("select[id$=VBD_ACCOUNT_TYPE] option:selected").val();
        if (value == '-1') {
            $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").removeAttr('disabled');
        }
    });
}

function bindAccountTypes() {
    $.get(vendorRegistration.GetAccountTypes, function (data) {
        var id = $("select[id$=VBD_ACCOUNT_TYPE]").attr("id");
        GrandScriptUtils.FillDropDown(id, data, true, true, false, false, true);
    });
}


function SaveLocalAddressDetails() {
    //debugger;

    if ($("[id$=hdfVendorPK]").val() == "0") {
        SavePage("Draft", "AddLocalAddress"); // Method not implimented yet
    }
    else {
        AddLocalAddress();
    }
    return false;
} 
function AddLocalAddress() {
    RemoveValidation();
    AddValidations(7);
    if ($(document.forms[0]).valid()) {

        var Obj = new Object();
        Obj.VNC_LC_PK = LocalAddressID;
        Obj.VNC_LC_VENDOR = $("[id$=hdfVendorPK]").val();
        Obj.VNC_LC_TITTLE = $("input[id$=VNC_LC_TITTLE]").val();
        Obj.VNC_LC_LASTNAME = $("input[id$=VNC_LC_LASTNAME]").val();
        Obj.VNC_LC_ADDR1 = $("input[id$=VNC_LC_ADDR1]").val();
        Obj.VNC_LC_ADDR2 = $("input[id$=VNC_LC_ADDR2]").val();
        Obj.VNC_LC_ADDR3 = $("input[id$=VNC_LC_ADDR3]").val();
        if ($("select[id$=VNC_LC_CNTRY]").val() == 0) {
            Obj.VNC_LC_CNTRY = '';
        }
        else {
            Obj.VNC_LC_CNTRY = $("select[id$=VNC_LC_CNTRY]").val();
        }
        Obj.VNC_LC_TAX_NO = $("input[id$=VNC_LC_TAX_NO]").val();
        Obj.VNC_LC_NAME = $("input[id$=VNC_LC_NAME]").val();
        Obj.VNC_LC_POSTAL = $("input[id$=VNC_LC_POSTAL]").val();
        Obj.VNC_LC_BRANCH = $("input[id$=VNC_LC_BRANCH]").val();

        var LocalAddress = JSON.stringify(Obj);
        $.post(vendorRegistration.SaveLocalAddress, LocalAddress, function (data) {
            if (data) {
                if (parseInt(data) > 0) {
                    BindGridLocalAddress();
                }
                else {
                    GrandScriptUtils.ShowModal("Information Not Saved", "Information");
                }
            }
        });
        ClearLocalAddress();
    }
    return false;
}

function BindGridLocalAddress() {
    //<summary>To handle bind grid Bank corr. to the VendorPk value</summary>
    //    var srchV = "";
    //    var ajaxUrl = vendorRegistration.GetVendorBanks + $("[id$=hdfVendorPK]").val()
    //    $("#grdVendorBankDetails").removeAttr("ajaxurl")
    //    $("#grdVendorBankDetails").attr("ajaxurl", ajaxUrl);
    //    GrandGrid.Utilities.ResetGrid(true, "grdVendorBankDetails");
    //    GrandGrid.MakeGrid($("#grdVendorBankDetails"));
   
    if ($("[id$=hdfVendorPK]").val() == undefined || $("[id$=hdfVendorPK]").val() == 0) {
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdVendorLocalAddressDetails"), 0, dummyObj);
    }
    else {
        $.get(vendorRegistration.GetVendorLocalAddress + $("[id$=hdfVendorPK]").val(), function (data) {
            GrandGrid.Utilities.ResetGrid(true, "grdVendorLocalAddressDetails");
            GrandGrid.MakeGrid($("#grdVendorLocalAddressDetails"), 1, data);
        });
    }
    return false;
}

function DeleteLocalAddress() {
    ///<summary>Function Used to delete the selected Bank from the Bank Grid and Database </summary>
    $.get(vendorRegistration.DeleteVenLocalAddress + LocalAddressID, function (data) {
        if (parseInt(data) > 0) {
            LocalAddressID = 0;
            //$("#divdummyMaterial").show();
            // $(tdset).insertAfter($("#materialInsert").find("tr:eq(0)"));
            BindGridLocalAddress();
        }

    });
    ClearLocalAddress();
}

function GridLocalAddressAction(tr, command) {

    switch (command.toString().toUpperCase()) {
        case vendorRegistration.DELETELOCALADDRESS:
            var vncLcPk = GrandGrid.Utilities.GetColumnValue(tr, "VNC_LC_PK", $(tr).parents("table:first").attr("id"));
            //  materialID = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.DeleteLcAddress, true);
            LocalAddressID = vncLcPk;
            //            DeleteBank(vbdPk);
            return false;
            break;
        case vendorRegistration.EDITLOCALADDRESS:
            var vncLcPk = GrandGrid.Utilities.GetColumnValue(tr, "VNC_LC_PK", $(tr).parents("table:first").attr("id"));
            FillLocalAddressDetailsFromGrid(vncLcPk);
            return false;
            break;
        case vendorRegistration.VIEWLOCALADDRESS:
            var vncLcPk = GrandGrid.Utilities.GetColumnValue(tr, "VNC_LC_PK", $(tr).parents("table:first").attr("id"));
            FillLocalAddressDetailsFromGrid(vncLcPk);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

function FillLocalAddressDetailsFromGrid(vncLcPk) {
    var vendorId = $("[id$=VEN_PK]").val();
    LocalAddressID = vncLcPk;
    $.get(vendorRegistration.GetVendorLocalAddressById + vendorId + "&P_VNC_LC_PK=" + vncLcPk, function (data) {

        var LocalAddDet = data[0];
        $("[id$=VNC_LC_TITTLE]").val(LocalAddDet.VNC_LC_TITTLE);
        $("[id$=VNC_LC_NAME]").val(LocalAddDet.VNC_LC_NAME);
        $("[id$=VNC_LC_LASTNAME]").val(LocalAddDet.VNC_LC_LASTNAME);
        $("[id$=VNC_LC_ADDR1]").val(LocalAddDet.VNC_LC_ADDR1);
        $("[id$=VNC_LC_ADDR2]").val(LocalAddDet.VNC_LC_ADDR2);
        $("[id$=VNC_LC_ADDR3]").val(LocalAddDet.VNC_LC_ADDR3);
        $("[id$=VNC_LC_TAX_NO]").val(LocalAddDet.VNC_LC_TAX_NO);
        $("[id$=VNC_LC_POSTAL]").val(LocalAddDet.VNC_LC_POSTAL);
        $("[id$=VNC_LC_BRANCH]").val(LocalAddDet.VNC_LC_BRANCH);

        if (LocalAddDet.VNC_LC_CNTRY == null) {
            $("[id$=VNC_LC_CNTRY]").val(0);
        }
        else {
            $("[id$=VNC_LC_CNTRY]").val(LocalAddDet.VNC_LC_CNTRY);
        }
    });
    return false;
}

function ClearLocalAddress() {
    LocalAddressID = 0;
    $("[id$=VNC_LC_TITTLE]").val('');
    $("[id$=VNC_LC_NAME]").val('');
    $("[id$=VNC_LC_LASTNAME]").val('');
    $("[id$=VNC_LC_ADDR1]").val('');
    $("[id$=VNC_LC_ADDR2]").val('');
    $("[id$=VNC_LC_ADDR3]").val('');
    $("[id$=VNC_LC_TAX_NO]").val('');
    $("[id$=VNC_LC_POSTAL]").val('');
    $("[id$=VNC_LC_BRANCH]").val('');
    $("[id$=VNC_LC_CNTRY]").val(0);
    RemoveValidation();
    return false;
}

function AddBankDetails() {
    ///<summary>
    ///Used for SaveAddress
    ///</summary>
    if ($("[id$=hdfVendorPK]").val() == "0") {
        SavePage("Draft", "AddBank"); // Method not implimented yet
    }
    else
        AddBank();

    return false;
}

function AddBank() {
    // $("input[id$=VBD_NAME]").removeClass("error");
    $("input[id$=VBD_ACCOUNT_NO]").removeClass("error");
    $("input[id$=VBD_ACCOUNT_NO_CNFM]").removeClass("error");
    RemoveValidation();
    AddValidations(6)
    if ($(document.forms[0]).valid()) {
        if ($("input[id$=VBD_ACCOUNT_NO]").val() != $("input[id$=VBD_ACCOUNT_NO_CNFM]").val()) {
            GrandScriptUtils.ShowModal("Translate(ValuesMisMatch)", "Information");
            $("input[id$=VBD_ACCOUNT_NO]").addClass("error");
            $("input[id$=VBD_ACCOUNT_NO_CNFM]").addClass("error");
            return false;
        }
        var obj = new Object();
        obj.VBD_PK = bankID;
        obj.VBD_VENDOR = $("[id$=hdfVendorPK]").val();
        obj.VBD_NAME = $("[id$=VBD_NAME]").val();
        obj.VBD_BRANCH = $("input[id$=VBD_BRANCH]").val();
        obj.VBD_CITY = $("input[id$=VBD_CITY]").val();
        if ($("select[id$=VBD_COUNTRY]").val() == 0) {
            obj.VBD_COUNTRY = '';
        }
        else {
            obj.VBD_COUNTRY = $("select[id$=VBD_COUNTRY]").val();
        }
        obj.VBD_PHONE = $("input[id$=VBD_PHONE]").val();
        obj.VBD_FAX = $("input[id$=VBD_FAX]").val();
        obj.VBD_IFSC_CODE = $("input[id$=VBD_IFSC_CODE]").val();
        obj.VBD_ACCOUNT_NO = $("input[id$=VBD_ACCOUNT_NO]").val();
        if ($("select[id$=VBD_ACCOUNT_TYPE]").val() == 0 || $("select[id$=VBD_ACCOUNT_TYPE]").val() == -1) {
            obj.VBD_ACCOUNT_TYPE = '';
        }
        else {
            obj.VBD_ACCOUNT_TYPE = $("select[id$=VBD_ACCOUNT_TYPE]").val();
        }
        obj.VBD_ACCOUNT_TYPE_OTHER = $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").val();
        obj.VBD_STATE_OTHER = $("input[id$=VBD_STATE_OTHER]").val();
        obj.VBD_ZIP = $("input[id$=VBD_ZIP]").val();
        obj.VBD_EMAIL = $("input[id$=VBD_EMAIL]").val();
        obj.VBD_SWIFT_CODE = $("input[id$=VBD_SWIFT_CODE]").val();
        obj.VBD_ACCOUNT_NO_CNFM = $("input[id$=VBD_ACCOUNT_NO_CNFM]").val();
        obj.VBD_CONTACT = $("input[id$=VBD_CONTACT]").val();
        obj.VBD_ACTIVE = 1;
        obj.VBD_MOD_BY = 0;
        obj.VBD_ADDRESS = $("textarea[id$=VBD_ADDRESS]").val();
        obj.VBD_MOBILE = $("input[id$=VBD_MOBILE]").val();

        var bank = JSON.stringify(obj);
        $.post(vendorRegistration.SaveBank, bank, function (data) {
            if (data) {
                if (parseInt(data) > 0) {
                    BindGridBanks();
                }
                else {
                    GrandScriptUtils.ShowModal("Information Not Saved", "Information");
                }
            }
        });
        clearBank();

    }
    // RemoveValidation();
    return false;
}

function BindGridBanks() {
    //<summary>To handle bind grid Bank corr. to the VendorPk value</summary>
    //    var srchV = "";
    //    var ajaxUrl = vendorRegistration.GetVendorBanks + $("[id$=hdfVendorPK]").val()
    //    $("#grdVendorBankDetails").removeAttr("ajaxurl")
    //    $("#grdVendorBankDetails").attr("ajaxurl", ajaxUrl);
    //    GrandGrid.Utilities.ResetGrid(true, "grdVendorBankDetails");
    //    GrandGrid.MakeGrid($("#grdVendorBankDetails"));
    //debugger;
    if ($("[id$=hdfVendorPK]").val() == undefined || $("[id$=hdfVendorPK]").val() == 0) {
        var dummyObj = new Object();
        GrandGrid.MakeGrid($("#grdVendorBankDetails"), 0, dummyObj);
    }
    else {
        $.get(vendorRegistration.GetVendorBanks + $("[id$=hdfVendorPK]").val(), function (data) {
            GrandGrid.Utilities.ResetGrid(true, "grdVendorBankDetails");
            GrandGrid.MakeGrid($("#grdVendorBankDetails"), 1, data);
        });
    }
    return false;
}

function GridBankAction(tr, command) {
    ///<summary>Grid Handler For Material Grid Catch all the material grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString().toUpperCase()) {
        case vendorRegistration.DeleteCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            //  materialID = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            GrandScriptUtils.ShowModal(vendorRegistration.DeleteConfirmationMessage, vendorRegistration.ConfirmationMessage, vendorRegistration.DeleteBank, true);
            bankID = vbdPk;
            //            DeleteBank(vbdPk);
            return false;
            break;
        case vendorRegistration.EditCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            FillBankDetailsFromGrid(vbdPk);
            return false;
            break;
        case vendorRegistration.ViewCommand:
            var vbdPk = GrandGrid.Utilities.GetColumnValue(tr, "VBD_PK", $(tr).parents("table:first").attr("id"));
            FillBankDetailsFromGrid(vbdPk);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

function FillBankDetailsFromGrid(vbdPk) {
    ///<summary>Function Used Fill the Bank Details corresponding to the vbdPk </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls       
    /// </param>
    var vendorId = $("[id$=VEN_PK]").val();
    bankID = vbdPk;
    $.get(vendorRegistration.GetBankDetailsById + vendorId + "&P_VBD_PK=" + vbdPk, function (data) {
        var bankDet = data[0];
        $("[id$=VBD_NAME]").val(bankDet.VBD_NAME);
        $("[id$=VBD_BRANCH]").val(bankDet.VBD_BRANCH);
        $("[id$=VBD_ADDRESS]").val(bankDet.VBD_ADDRESS);
        $("[id$=VBD_CITY]").val(bankDet.VBD_CITY);
        $("[id$=VBD_STATE_OTHER]").val(bankDet.VBD_STATE_OTHER);
        if (bankDet.VBD_COUNTRY == null) {
            $("[id$=VBD_COUNTRY]").val(0);
        }
        else {
            $("[id$=VBD_COUNTRY]").val(bankDet.VBD_COUNTRY);
        }
        // $("[id$=VBD_COUNTRY]").val(bankDet.VBD_COUNTRY);
        $("[id$=VBD_ZIP]").val(bankDet.VBD_ZIP);
        $("[id$=VBD_PHONE]").val(bankDet.VBD_PHONE);
        $("[id$=VBD_MOBILE]").val(bankDet.VBD_MOBILE);
        $("[id$=VBD_FAX]").val(bankDet.VBD_FAX);
        $("[id$=VBD_EMAIL]").val(bankDet.VBD_EMAIL);
        $("[id$=VBD_IFSC_CODE]").val(bankDet.VBD_IFSC_CODE);
        $("[id$=VBD_SWIFT_CODE]").val(bankDet.VBD_SWIFT_CODE);
        $("[id$=VBD_ACCOUNT_NO]").val(bankDet.VBD_ACCOUNT_NO);
        $("[id$=VBD_ACCOUNT_NO_CNFM]").val(bankDet.VBD_ACCOUNT_NO);
        $("[id$=VBD_ACCOUNT_TYPE]").val(bankDet.VBD_ACCOUNT_TYPE);
        if (bankDet.VBD_ACCOUNT_TYPE == null) {
            $("[id$=VBD_ACCOUNT_TYPE]").val(0);
        }
        else {
            $("[id$=VBD_ACCOUNT_TYPE]").val(bankDet.VBD_ACCOUNT_TYPE);
        }
        $("[id$=VBD_ACCOUNT_TYPE_OTHER]").val(bankDet.VBD_ACCOUNT_TYPE_OTHER);
        if (bankDet.VBD_ACCOUNT_TYPE_OTHER != '') {
            $("[id$=VBD_ACCOUNT_TYPE]").val(-1);
        }
        $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").attr('disabled', 'disabled');
        var value = $("select[id$=VBD_ACCOUNT_TYPE] option:selected").val();
        if (value == '-1') {
            $("input[id$=VBD_ACCOUNT_TYPE_OTHER]").removeAttr('disabled');
        }
        $("[id$=VBD_CONTACT]").val(bankDet.VBD_CONTACT);
    });
    return false;
}

function clearBank() {
    bankID = 0;
    $("[id$=VBD_NAME]").val('');
    $("[id$=VBD_BRANCH]").val('');
    $("[id$=VBD_ADDRESS]").val('');
    $("[id$=VBD_CITY]").val('');
    $("[id$=VBD_STATE_OTHER]").val('');
    $("[id$=VBD_COUNTRY]").val(0);
    $("[id$=VBD_ZIP]").val('');
    $("[id$=VBD_PHONE]").val('');
    $("[id$=VBD_MOBILE]").val('');
    $("[id$=VBD_FAX]").val('');
    $("[id$=VBD_EMAIL]").val('');
    $("[id$=VBD_IFSC_CODE]").val('');
    $("[id$=VBD_SWIFT_CODE]").val('');
    $("[id$=VBD_ACCOUNT_NO]").val('');
    $("[id$=ACCOUNT_NO_CNFM]").val('');
    $("[id$=VBD_ACCOUNT_TYPE]").val(0);
    $("[id$=VBD_ACCOUNT_TYPE_OTHER]").val('');
    $("[id$=VBD_CONTACT]").val('');
    RemoveValidation();
    //$("input[id$=VBD_NAME]").rules("remove");
    return false;
}

function DeleteBank() {
    ///<summary>Function Used to delete the selected Bank from the Bank Grid and Database </summary>
    $.get(vendorRegistration.DeleteVenBank + bankID, function (data) {
        if (parseInt(data) > 0) {
            bankID = 0;
            //$("#divdummyMaterial").show();
            // $(tdset).insertAfter($("#materialInsert").find("tr:eq(0)"));
            BindGridBanks();
        }

    });
    clearBank();
}
// Bank Tab -- Ends

function InitComponents() {
    $("[id$=VEN_COMMISSION]").ForceNumericOnly();
}
