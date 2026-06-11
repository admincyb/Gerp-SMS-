
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />


///#region ---------- Global Variable Declaration

var UOMJson = new Object();
var uomId = 0;
var uomTypeId = 0;
var conversionID = 0;

var frmUnitID = 0;
var toUnitId = 0;

///#endregion

///#region ---------- Configuration


var UOMMaster = {

    ENTERUOMTYPE: "Translate(PleaseProvideUOMType)",
    SELECTUOM: "Translate(PleaseSelectUnit)",
    SELECTUOMTYPE: "Translate(PleaseselectUOMType)",
    ENTERUOMCODE: "Translate(PleaseProvideUOMCode)",
    PROVIDENAME: "Translate(PleaseProvideName)",
    ENTERDIGITS: "Translate(PleaseProvideDecimal)",
    DEFAULTACTIONMSG: "Translate(DefaultActionneedstobeperformed)",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    SELECTONEMSG: "Translate(Pleaseselectanoption)",
    CONFORMATIONTITLE: "Translate(Confirmation)",
    UOMCONVERSION: "Translate(UOMConversion)",
    CHOOSEUOMCONVERSION: "Translate(ChooseUOMConversion)",
    ENTER2DIGITDECIMALMSG: "Translate(EnterTwoDigitDecimal)",
    UOMDELETESUCESSMSG: "Translate(UOMdeletedsuccessfully)",
    INFORMATIONTITLE: "Translate(Information)",
    UOMALREADYUSED: "Translate(CannotDeleteHaveReference)",
    CANNOTDELETEALREADYUSED: "Translate(CannotDeleteHaveReference)",
    ACTIONFIALEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    UOMDELETESUCCESS: "Translate(UOMTypedeletedsuccessfully)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    UOMTYPESAVESUCESSMSG: "Translate(UOMTYPESavedSuccessfully)",
    SAVESUCESSMSG: "Translate(UOMSavedSuccessfully)",
    UOMTYPEALREADYEXITS: "Translate(UOMtypealreadyexists)",
    ENTERCONVERSIONDETAILS: "Translate(EnterConversionDetails)",
    UOMCODEALREADYEXISTS: "Translate(UOMcodealreadyexists)",
    SAMECONVERSIONALREADYADDED: "Translate(SameConversionAlreadyAdded)",
    YOUSELECTESSAMEUNITTYPE: "Translate(YouSelectedSameUnitType)",
    SAVECONVERSION: "Translate(AddConversionSuccessfully)",
    CONVERSIONALREADYEXISTS: "Translate(ConversionAlreadyExists)",
    PROVIDEUOMTYPEMSG: "Translate(EnterUOMType)",
    ENTERVALUE: "Translate(EnterValue)",

    CREATEUOMTYPE: "Translate(CreateUOMType)",
    // URL
    SAVEUOMURL: "UOMManagement.do?Action=SaveUOM",
    DELETEUOMTYPEURL: "UOMManagement.do?Action=DeleteUOMType&UOMTypeId=",
    GETUOMDETAILSBYPKURL: "UOMManagement.do?Action=GetUOMDtls&uOMPK=",
    SAVEUOMTYPEURL: "UOMManagement.do?Action=SaveUOMType",
    SEARCHINITURL: "UOMManagement.do?Action=GetSearchValue&SBU=",
    FILLUOMTYPEURL: "UOMManagement.do?Action=GetUOMType&SBUPk=",
    FILLUOMURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=",
    FILLUOMDTLSURL: "UOMManagement.do?Action=GetUOMList&Status=",
    DELETEUOMTYPEURL: "UOMManagement.do?Action=DeleteUOMType&UOMTypeId=",
    DELETEUOMURL: "UOMManagement.do?Action=DeleteUOM&UOMId=",
    INBOX: "../../AccountManagement/WorkflowInbox.aspx",

    // COnstant
    TOUNIT: "UMC_TO",
    FROMUNIT: "UMC_FROM",
    CONVALUE: "UMC_CONV_FACT",
    CONVERSIONID: "UMC_PK",
    DELETECOVNCMD: "deleteconv",
    EDITCONVCMD: "editconv",
    VALUEZERO: "0",
    VALUEEMPTY: "",
    UOMTYPE: "Translate(UOMType)",
    SAVEUOM: "saveUOM",
    SAVECONV: "saveconv",
    DELETE: "delete",
    UOMPK: "UOM_PK",
    UMTPK: "UMT_PK",
    DELETETYPE: "deletetype",
    UOMSAVE: "uomsaved",
    UOMDELETED: "uomdeleted",
    UMTNAME: "UMT_NAME",
    SAVE: "save",
    SAVEBUTTON:"Save",
    UOMDELETE: "uomdeleted",
    SELECTONE: "selectNone",
    EDITTYPE: "edittype"
}

///#endregion

///#region ---------- Initialization Section

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    $.validator.addMethod(UOMMaster.SELECTONE, function (value, element) {
        return ($(element).val() != UOMMaster.VALUEZERO);
    }, UOMMaster.SELECTONEMSG);

    // validation For 2 Digit Decimal
    $.validator.addMethod("twodecimal", function (value) {
        return /^\d{1,3}(\.\d{0,3})?$/.test(value);
    }, UOMMaster.ENTER2DIGITDECIMALMSG);
    $("input[id$=UOM_CODE]").val("");
    $("input[id$=UOM_NAME]").val("");
    //Page Initial condtions
    PageInit();
    // Show Search Criteria
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });
    // Search Details By Button Click with Search Criteria
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
    // Show Conversion Button, Select UOM Type
    $("[id$=UOM_TYPE]").change(function () {

        ShowConversionDtls();
    });

    $("[id$=UOM_CODE]").keyup(function () {
        ShowConversionDtls();
    });
    // Initilize UOMJsn Object
    UOMJson = $.parseJSON($("[id$=ConversionList]").val());
    $("#divDatas").data("UOMData", UOMJson);

    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    UOMJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, UOMJson.ConversionList);

});

function PageInit() {
    ///<summary>Used for initial settings</summary>
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();


    $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });

    $("input[id$=UOM_PK]").val(UOMMaster.VALUEZERO);
    // Bind UOM Details In Grid For Listing
    BindGrid();
    // Fill UOM Type
    FillUOMType(0);
    // Set Search Init- For Autocomplete
    SearchInit();
    // Set Seatch Controls
    SetSearchType();
    // Hide PopUp Controls
    //$("#divAddUomType").dialog({ autoOpen: false });
    //$("#divAddConversion").dialog({ autoOpen: false });
    $("#divAddUomType").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveUOMTypePopUpValidation();
        }
    });
    $("#divAddConversion").dialog({
        autoOpen: false,
        height:950,
        width: 640,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveConversionPopUpValidation();
        }
    })
    $("[id$=SearchType]").focus();
    $("input[id$=BizUnit]").val($("[id$=BizUnitPk]").val());
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    return false;
}

///#endregion

///#region ---------- Core Section 

///#region----Auto Complete Section 

///<summary>To handle auto complete</summary>
function SearchInit() {
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", UOMMaster.SEARCHINITURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

///<summary>Function To Enable/Disable Selected Option For Search </summary>
function SetSearchType() {
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(UOMMaster.VALUEEMPTY);
    if (strname == UOMMaster.VALUEZERO) {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
    }
}

///#endregion

///#region----Fetch Data To Populate In Controls
function ShowConversionDtls() {
    ///<summary>Show Hide Conversion Details  settings</summary>
    if ($("[id$=UOM_TYPE]").val() != "0" && $("[id$=UOM_CODE]").val().length > 0) {
        ShowConversion();
        ResetConversionDetails();
    }
    else {

        $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
    }

}

///<summary>Used to fill UOM Type</summary>
function FillUOMType(uomTypeID) {
    // Get id of the UOM Type DropDown
    var drpID = $("select[id$=UOM_TYPE]").attr("id");
    $.get(UOMMaster.FILLUOMTYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, uomTypeID);
    });
}

///<summary>function to fill Dropdown with UOM</summary>
function FillUOM() {
    // Get id of the Unit DropDown
    //var drpID = $("select[id$=UMC_FROM]").attr("id");
    var drpIDTo = $("select[id$=UMC_TO]").attr("id");
    //$.get(UOMMaster.FILLUOMURL + $("select[id$=UOM_TYPE]").val() + "&UOMPK=" + $("input[id$=UOM_PK]").val() + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
     $.get(UOMMaster.FILLUOMURL + "0" + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        //GrandScriptUtils.FillDropDown(drpID, data, true, true);
        GrandScriptUtils.FillDropDown(drpIDTo, data, true, true);
    });
}

///#endregion

///#region----Set Or Reset Form

///<summary>Used to reset UOM Details</summary>
function ResetPage() {
    // Reset All Validations
    $(document.forms[0]).validate().resetForm();
    var idval = "";
    //Reseting all input controls in the page
//    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () {
//        idval = $(this).attr("id");
//        if (idval.search("UOM_PK") != -1)
//            $(this).val(UOMMaster.VALUEZERO);
//        else if (idval.search("UserPk") == -1)
//            $(this).val(UOMMaster.VALUEEMPTY);
//    });
//    $(document.forms[0]).find("select").each(function () {
//        idval = $(this).attr("id");
//        if (idval.search("SBU") == -1)
//            $(this).val($(this).find("option:eq(0)").val());
    //    });
    ClearForm();
   
    PageInit();
    // Reset and clear Conversion details grid and UOM Conversion List
    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    var ObjUOM = $("#divDatas").data("UOMData");
    if (ObjUOM != null) {
        ObjUOM.ConversionList = new Array();

    }
    // Bind grid With Null Data
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, ObjUOM.ConversionList);
    $("#divDatas").data("UOMData", ObjUOM);
    //$("#divAddConversion").dialog("close");
    //$("#divAddUomType").dialog("close");
    RemoveValidations();
    return false;
}
function ClearForm() {

    $("input[id$=UOM_PK]").val('0');
    $("input[id$=UOM_PK]").val('0');
    $("input[id$=UOM_DECIMAL]").val('');
    $("input[id$=UOM_CODE]").val('');
    $("input[id$=UOM_NAME]").val('');
    $("select[id$=UOM_TYPE]").val('0');

}

///<summary>Used to reset UOM Type popup</summary>
function ResetUOMType() {
    $("input[id$=UMT_NAME]").val(UOMMaster.VALUEEMPTY);
    $("input[id$=UMT_PK]").val(UOMMaster.VALUEZERO);
    $("[id$=lblstarUOM]").hide();
}

///<summary>Used to reset UOM Conversion popup</summary>
function ResetUOMConversion() {

    $("select[id$=UMC_TO]").val(UOMMaster.VALUEZERO);
    $("input[id$=UMC_CONV_FACT]").val(UOMMaster.VALUEEMPTY);
}

///#endregion

///#region----Bind Grid

///<summary>To handle bind grid corr. to the search type and search value</summary>
function BindGrid() {
    var ajaxUrl = UOMMaster.FILLUOMDTLSURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val() ;
    $("#grdUOMDetails").removeAttr("ajaxurl")
    $("#grdUOMDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdUOMDetails");
    GrandGrid.MakeGrid($("#grdUOMDetails"));
}

///<summary>To handle bind grid for UOM Type</summary>
function BindGridUOMType() {
    var ajaxUrl = "UOMManagement.do?Action=GetUOMTypeList" + "&bizUnit=" + $("[id$=BizUnitPk]").val();
    $("#grdUOMType").removeAttr("ajaxurl")
    $("#grdUOMType").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdUOMType");
    GrandGrid.MakeGrid($("#grdUOMType"));
}

///<summary>filling gridview after entering search value in search textbox</summary>
function AfterSelect() {
    BindGrid();
}

///#endregion

function RedirectToInbox() {
    window.location = UOMMaster.INBOX;
    return false;
}

///#region----Grid Handlers And Model Popup Ok Click

///<summary>Grid Handler Catches all grid events of UOM </summary>
function GridHandler(tr, command) {
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case UOMMaster.DELETE:
            uomId = GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.UOMPK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(UOMMaster.DoUWantToDelMsg, UOMMaster.CONFORMATIONTITLE, UOMMaster.DELETE, true);
            return false;
            break;

        case "edit":
            uomId = GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.UOMPK, $(tr).parent().attr("id"));
            //FillDetails(tr);
            FillDetails(uomId);
            return false;
            break;

        default:

            GrandScriptUtils.ShowModal(UOMMaster.DEFAULTACTIONMSG, UOMMaster.INFORMATIONTITLE);
            return false;
            break;

    }
    return false;
}

///<summary>Grid Handler Catches all grid events from UOM Type popup </summary>
function GridHandlerType(tr, command) {
    switch (command.toString().toLowerCase()) {

        case UOMMaster.EDITTYPE:
            FillUOMTypeDetails(tr);
            return false;
            break;

        case UOMMaster.DELETETYPE:
            uomTypeId = GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.UMTPK, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(UOMMaster.DoUWantToDelMsg, UOMMaster.CONFORMATIONTITLE, UOMMaster.DELETETYPE, true);
            return false;
            break;

        default:

            GrandScriptUtils.ShowModal(UOMMaster.DEFAULTACTIONMSG, UOMMaster.INFORMATIONTITLE);
            return false;
            break;


    }
    return false;

}

//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {

    switch (command) {

        case UOMMaster.DELETECOVNCMD:
            DeleteConversionDetails();
            break;
        case UOMMaster.DELETETYPE:
            DeleteUOMType();
            break;
        case UOMMaster.DELETE:
            DeleteDetails();
            break;
        case UOMMaster.UOMSAVE:
            ResetUOMType();
            ShowConversionDtls();
            $("[id$=divAddUomType]").dialog("close");
            $("select[id$=UOM_TYPE]").focus();
            break;
        case UOMMaster.UOMDELETED:
            BindGridUOMType();
            FillUOMType(0);
            break;
    }
    return false;
}

///#endregion

///#region----Data Management Section

//<summary>Function To Delete UOM Details</summary>
function DeleteDetails() {
    $.get(UOMMaster.DELETEUOMURL + uomId, function (data) {
        if (parseInt(data) > 0)
            GrandScriptUtils.ShowModal(UOMMaster.UOMDELETESUCESSMSG, UOMMaster.INFORMATIONTITLE);
        else if (parseInt(data) == 0 || parseInt(data) == -2)
            GrandScriptUtils.ShowModal(UOMMaster.UOMALREADYUSED, UOMMaster.INFORMATIONTITLE);
        else
            GrandScriptUtils.ShowModal(UOMMaster.ACTIONFIALEDMSG, UOMMaster.INFORMATIONTITLE);
        BindGrid();
    });
    return false;

}

//<summary>Function To Delete UOM Type</summary>
function DeleteUOMType() {
    $.get(UOMMaster.DELETEUOMTYPEURL + uomTypeId, function (data) {
        if (parseInt(data) > 0) {

            $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
            GrandScriptUtils.ShowModal(UOMMaster.UOMDELETESUCCESS, UOMMaster.INFORMATIONTITLE, UOMMaster.UOMDELETE);
        }
        else if (parseInt(data) == 0 ||  parseInt(data) == -2)
            GrandScriptUtils.ShowModal(UOMMaster.UOMALREADYUSED, UOMMaster.INFORMATIONTITLE);
        else
            GrandScriptUtils.ShowModal(UOMMaster.ACTIONFAILEDMSG, UOMMaster.INFORMATIONTITLE);

    });
    return false;

}

///<summary>Used to fill UOMType Details for editing</summary>
function FillDetails(uOMID) {
    $.get(UOMMaster.GETUOMDETAILSBYPKURL + uOMID, function (data) {
        $("#divDatas").data("UOMData", data);
        FillUOMDetails();
    });
}

///<summary>Used to fill UOMTYpe Details for editing</summary>
function FillUOMDetails() {
    AddNew();
    UOMJson = $("#divDatas").data("UOMData");
    $("input[id$=UOM_PK]").val(UOMJson.UOM_PK);
    $("input[id$=UOM_DECIMAL]").val(UOMJson.UOM_DECIMAL);
    $("input[id$=UOM_CODE]").val(UOMJson.UOM_CODE);
    $("input[id$=UOM_NAME]").val(UOMJson.UOM_NAME);
    $("select[id$=UOM_TYPE]").val(UOMJson.UOM_TYPE);
    // Check Array Have atleast One Record-Check Is Array or Not
    if (!($.isArray(UOMJson.ConversionList))) {

        if (UOMJson.ConversionList != undefined) {
            var objArray = UOMJson.ConversionList;
            UOMJson.ConversionList = new Array();
            UOMJson.ConversionList.push(objArray);
        }
        else {
            var objArray = UOMJson.ConversionList;
            UOMJson.ConversionList = new Array();
        }
    }
    if ($.isArray(UOMJson.ConversionList)) {
        GrandGrid.MakeGrid($("#grdConversionDtls"), 0, UOMJson.ConversionList);
    }
    ShowConversion();
    return false;

}

//<summary>Used to fill UOM Type Details for editing</summary>
function FillUOMTypeDetails(tr) {

    $("input[id$=UMT_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.UMTNAME, $(tr).parent().parent().attr("id")));
    $("input[id$=UMT_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.UMTPK, $(tr).parent().parent().attr("id")));
    return false;

}

//<summary>Used Save UOM Details</summary>
function SavePage() {
    //Add Validation for UOM Master Details by setting mode as 1
    AddValidations();
    UOMJson = $("#divDatas").data("UOMData");
    // Check Have The Conversion List have value
    //if (UOMJson.ConversionList.length > 0) {
    if ($(document.forms[0]).valid()) {

        $("[id$=ConversionList]").val(JSON.stringify(UOMJson.ConversionList));
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(UOMMaster.SAVEUOMURL, jSonString, function (data) {
            if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(UOMMaster.SAVESUCESSMSG, UOMMaster.INFORMATIONTITLE, UOMMaster.SAVE);
                ResetPage();
            }
            else if (parseInt(data) == -2)
                GrandScriptUtils.ShowModal(UOMMaster.CONVERSIONALREADYEXISTS, UOMMaster.INFORMATIONTITLE);
            else if (parseInt(data) == 0)
                GrandScriptUtils.ShowModal(UOMMaster.UOMCODEALREADYEXISTS, UOMMaster.INFORMATIONTITLE);
            else
                GrandScriptUtils.ShowModal(UOMMaster.ACTIONFAILEDMSG, UOMMaster.INFORMATIONTITLE);


        });

    }

    //}
    // else {
    //     GrandScriptUtils.ShowModal(UOMMaster.ENTERCONVERSIONDETAILS, UOMMaster.INFORMATIONTITLE);
    // }
    return false;

}

//<summary>Used Save UOM Type</summary>
function SaveUOMType() {
    //$(document.forms[0]).validate().resetForm();
   
    AddUOMTypePopUpValidation();
    var jSonString = GrandScriptUtils.FormToJsonString("divAddUomType");
    if ($(document.forms[0]).valid()) {
        
        $.post(UOMMaster.SAVEUOMTYPEURL, jSonString, function (data) {
            if (parseInt(data) > 0) {
                FillUOMType(data);
                //BindGridUOMType();
                GrandScriptUtils.ShowModal(UOMMaster.UOMTYPESAVESUCESSMSG, UOMMaster.INFORMATIONTITLE, UOMMaster.UOMSAVE);
            }
            else if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(UOMMaster.UOMTYPEALREADYEXITS, UOMMaster.INFORMATIONTITLE);
            }
            else {
                GrandScriptUtils.ShowModal(UOMMaster.ACTIONFAILEDMSG, UOMMaster.INFORMATIONTITLE);
            }

        });

        return false;
    }
    return false;
}

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    $(document.forms[0]).validate().resetForm();
    $("[id$=btnSave]").show();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("input[id$=UOM_DECIMAL]").val("2");
    $("[id$=UOM_TYPE]").focus();
    return false;
}

///<summary>Function To Allow UOM Conversion </summary>
function ShowConversion() {
    if ($("select[id$=UOM_TYPE]").val() != UOMMaster.VALUEZERO) {
        $("[id$=imbAddConversion]").css({ "display": "inline", "visibility": "visible" });
        //AddConversionPopUpValidation();
        ResetUOMConversion();
        FillUOM();
        $("[id$=UOMTypeFm]").text($("select[id$=UOM_TYPE] option:selected").text());
        $("[id$=UOMTypeTo]").text($("select[id$=UOM_TYPE] option:selected").text());
        $("[id$=UMC_FROM]").text($("[id$=UOM_CODE]").val());
    }
    else

        $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });

}

///<summary>function Show UOM type adding div</summary>
function AddUOMType() {
    RemoveValidations();
    ResetUOMType();
    BindGridUOMType();
    $("input[id$=btnAddUomType]").val(UOMMaster.SAVEBUTTON);
    $("#divAddUomType").dialog("open");
    $("#divAddUomType").dialog({ width: 500, height: 350, resizable: true, title :UOMMaster.CREATEUOMTYPE });
    $("#divAddUomType").css({ "min-height": "300", "margin-top": "25px" });

    setTimeout(function () { $("input[id$=UMT_NAME]").focus(); }, 10);

    RemoveUOMTypePopUpValidation();
    return false;
}

////<summary>function Show UOM conversion adding div</summary>
function AddConversion() {
    RemoveValidations();
    ClearConversionDtls();
    if (UOMJson.ConversionList.length > 0)
        $("#ConversionDiv").show();
        
    else
        $("#ConversionDiv").hide();
    $("input[id$=btnAddConv]").val(UOMMaster.SAVEBUTTON);
    $("#divAddConversion").dialog("open");
    $("#divAddConversion").dialog({ width:950, height: 500, resizable: false });
//    $("#divAddConversion").css({ "min-width": "300", "margin-top": "25px" });
    setTimeout(function () { $("select[id$=UMC_TO]").focus(); }, 10);
    RemoveConversionPopUpValidation();
    return false;
}

//Active Inactive Button Visibility and Delete button visibility depends on status.
function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type UOM Button</summary>
    var active = "false";
    $("#grdUOMType tr:has(td)").each(function () {
        active = GrandGrid.Utilities.GetColumnValue($(this), "UMT_DEFAULT", $(this).parents("table:first").attr("id"));
        if (active == "true") {
            $(this).find("input[type=image]:eq(1)").hide();
//            $(this).find("input[type=image]:eq(3)").hide();

        }
//        else
//            $(this).find("input[type=image]:eq(2)").hide();

    });
    $("#grdUOMDetails tr:has(td)").each(function () {
        active = GrandGrid.Utilities.GetColumnValue($(this), "UOM_DEFAULT", $(this).parents("table:first").attr("id"));
        if (active == "true"){
            $(this).find("input[type=image]:eq(1)").hide();
//            $(this).find("input[type=image]:eq(3)").hide();

        }
//        else
//            $(this).find("input[type=image]:eq(2)").hide();

    });
}

///#endregion

///#region ---------- Conversion Info Section

///#region ---------- Conversion Details Section - Add , Delete , Edit, Fill Details Operations
//<summary>function used to add Conversion  details </summary>
function SaveConversionDtls() {
    //$(document.forms[0]).validate().resetForm();
    AddConversionPopUpValidation();
    if ($(document.forms[0]).valid()) {
        UOMJson = $("#divDatas").data("UOMData");
        // Get ConversionID PK
        var editConversion = $("input[id$=EditConversion]").val();
        var obj = new Object();
        var flag = true;
        // Check Conversion Already Added - For new Entry
        if (parseInt(toUnitId) == 0) {
            for (var i in UOMJson.ConversionList) {

                if (UOMJson.ConversionList[i].UMC_TO == $("select[id$=UMC_TO]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            // Check Conversion Already Added - For Updation
            for (var i in UOMJson.ConversionList) {
                if (UOMJson.ConversionList[i].UMC_PK != editConversion || UOMJson.ConversionList[i].UMC_TO != toUnitId) {
                    if (UOMJson.ConversionList[i].UMC_TO == $("select[id$=UMC_TO]").val()) {
                        flag = false;
                        break;
                    }
                }
            }
            // Check Details Is new Entry Or to update . If Update Get Details From UOMJSON and Assign to Obj, and Details Will Updated To Object
            for (var i in UOMJson.ConversionList) {

                if (parseInt(toUnitId) == UOMJson.ConversionList[i].UMC_TO)

                    obj = UOMJson.ConversionList[i];
            }

        }

        // Check Already Added Or Not
        if (flag) {

            // Add One By one Details To Object
            obj.UMC_UOM_TYPE = parseInt($("select[id$=UOM_TYPE]").val());
            obj.UOMTYPEText = $("select[id$=UOM_TYPE] option:selected").text();
            obj.UMC_FROM = 0;
            obj.FromUnitText = $("[id$=UOM_CODE]").val();
            obj.UMC_TO = parseInt($("select[id$=UMC_TO]").val());
            obj.ToUnitText = $("select[id$=UMC_TO] :selected").text();
            obj.UMC_CONV_FACT = $("input[id$=UMC_CONV_FACT]").val();
            // Check Add Details - For New Entry
            if (parseInt(toUnitId) == 0) {
                // If Yes - get Length of the List and Assign Length+1 as the PK of New Entry
                obj.UMC_PK = 0; //  UOMJson.ConversionList.length + 1;
                //Push Object to List
                UOMJson.ConversionList.push(obj);
            }
            ClearConversionDtls();
            // Add Details To DivDatas
            $("#divDatas").data("UOMData", UOMJson);
            // Bind Conversion Details Grid
            GrandGrid.MakeGrid($("#grdConversionDtls"), 0, UOMJson.ConversionList);
            GrandScriptUtils.ShowModal(UOMMaster.SAVECONVERSION, UOMMaster.INFORMATIONTITLE);
            if (UOMJson.ConversionList.length > 0)
                $("#ConversionDiv").show();

            else
                $("#ConversionDiv").hide();
    
        }
        else {

            GrandScriptUtils.ShowModal(UOMMaster.SAMECONVERSIONALREADYADDED, UOMMaster.INFORMATIONTITLE);
        }
      
    }
    return false;

}

//<summary>Function to Clear Conversion Popup Controls Details </summary>
function ClearConversionDtls() {

    $("select[id$=UMC_TO]").val(UOMMaster.VALUEZERO);
    $("input[id$=UMC_CONV_FACT]").val(UOMMaster.VALUEEMPTY);
    $("input[id$=EditConversion]").val(UOMMaster.VALUEZERO);
    frmUnitID = 0;
    toUnitId = 0;
}

function ResetConversionDetails() {
    //<summary>Function to Reset Conversion Grid Controls Details </summary>
    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    UOMJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, UOMJson.ConversionList);
}

///<summary>Grid Handler Catch all the grid events in this function  - For Conversion Details</summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function ConversionGridHandler(tr, command) {
    // Remove Validations

    switch (command.toString().toLowerCase()) {
        case UOMMaster.DELETECOVNCMD:
            //conversionID = GrandGrid.Utilities.GetColumnValue(tr, "UMC_PK", "grdConversionDtls");

            toUnitId = GrandGrid.Utilities.GetColumnValue(tr, "UMC_TO", "grdConversionDtls");
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(UOMMaster.DoUWantToDelMsg, UOMMaster.CONFORMATIONTITLE, UOMMaster.DELETECOVNCMD, true);

            return false;
            break;
        case UOMMaster.EDITCONVCMD:
            FillConversionDetails(tr);
            return false;
            break;
        default:
            GrandScriptUtils.ShowModal(UOMMaster.DEFAULTACTIONMSG, UOMMaster.INFORMATIONTITLE);
            return false;
            break;
    }
}
///<summary>For delete the item in the grid - Conversion Details List</summary>
function DeleteConversionDetails() {
    var UOMJson = $("#divDatas").data("UOMData");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in UOMJson.ConversionList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (UOMJson.ConversionList[i].UMC_TO == toUnitId) {
            // Splice Details From List, Corresponding ToUnit
            UOMJson.ConversionList.splice(i, 1);
            break;
        }
    }

    $("#divDatas").data("UOMData", UOMJson);
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, UOMJson.ConversionList);
    if (UOMJson.ConversionList.length > 0)
        $("#ConversionDiv").show();

    else
        $("#ConversionDiv").hide();
    ClearConversionDtls();

}
///<summary>Used fill Details of Conversion Details for editing</summary>
/// <param name="tr"  type="Object">
/// Specific Container and its controls
/// </param>
function FillConversionDetails(tr) {
    ClearConversionDtls();
    $("select[id$=UMC_TO]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.TOUNIT, $(tr).parent().parent().attr("id")));
    //$("select[id$=UMC_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.FROMUNIT, $(tr).parents("table:eq(0)").attr("id")));
    $("input[id$=UMC_CONV_FACT]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.CONVALUE, $(tr).parents("table:eq(0)").attr("id")));
    $("input[id$=EditConversion]").val(GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.CONVERSIONID, $(tr).parent().parent().attr("id")));
    toUnitId = GrandGrid.Utilities.GetColumnValue(tr, UOMMaster.TOUNIT, $(tr).parents("table:eq(0)").attr("id"));
}

///#endregion

///#endregion
///#endregion

///#region ---------- Validation 

//<summary>function used to assign validation</summary>
function AddValidations() {
    $("select[id$=UOM_TYPE]").rules("add", {
        selectNone: true,
        messages: { selectNone: UOMMaster.SELECTUOMTYPE }
    });

    $("input[id$=UOM_CODE]").rules("add", {
        required: true,
        maxlength: 15,
        messages: { required: UOMMaster.ENTERUOMCODE }
    });

    $("input[id$=UOM_NAME]").rules("add", {
        required: true,
        maxlength: 50,
        messages: { required: UOMMaster.PROVIDENAME }
    });

    $("input[id$=UOM_DECIMAL]").rules("add", {
        required: true,
        digits: true,
        maxlength: 2,
        messages: { required: UOMMaster.ENTERDIGITS }
    });
}

//<summary>function used to assign validation To UOM Type POPupControl</summary>
function AddUOMTypePopUpValidation() {
    $("[id$=UMT_NAME]").rules("add", {
        required: true,
        maxlength: 50,
        messages: { required: UOMMaster.PROVIDEUOMTYPEMSG }
    });
}

//<summary>function used to assign To Conversion POPUP validation</summary>
function AddConversionPopUpValidation() {
    RemoveConversionPopUpValidation();
    $("select[id$=UMC_TO]").rules("add", {
        selectNone: true,
        messages: { selectNone: UOMMaster.SELECTUOM }
    });
    $("input[id$=UMC_CONV_FACT]").rules("add", {
        required: true,
        maxlength: 10,
        ThreeDecimal: true,
        messages: { required: UOMMaster.ENTERVALUE}
    });
}

function RemoveConversionPopUpValidation() {
    $("input[id$=UMC_CONV_FACT]").rules("remove");
    $("select[id$=UMC_TO]").rules("remove"); 
}
//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("select[id$=UOM_TYPE]").rules("remove");
  
    $("input[id$=UOM_CODE]").rules("remove");

    $("input[id$=UOM_NAME]").rules("remove");
 
    $("input[id$=UOM_DECIMAL]").rules("remove");
}

//<summary>function Conversion Popup Control Remove Validation</summary>
function RemoveConversionPopUpValidation() {
    $("select[id$=UMC_TO]").rules("remove");
    $("input[id$=UMC_CONV_FACT]").rules("remove");
}

//<summary>function Remove UOM Type Control Validation</summary>
function RemoveUOMTypePopUpValidation() {
    $("input[id$=UMT_NAME]").rules("remove");
}

///#endregion

///#region ---------- Utitlity 
function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}
///#endregion






