/// <reference path="../../../GrandScriptUtils.js" />
/// <reference path="../../../JSLINQ/JSLINQ-vsdoc.js" />


///#region -------------- Global Variable -------
var currencyMasterID = 0;
var exchangeMasterID = 0;
var CurrencyJson = new Object();
var frmUnitID = 0;
var toUnitId = 0;
var ExchDec;
var currencyId;

///#endregion

//#region ------- Configuration Section --------
var CurrencyMaster = {

    AutoCompleteURL: "CurrencyManagement.do?Action=GetSearchValue&bizUnit=",
    FillExchangeRateDropdownURL: "CurrencyManagement.do?Action=GetExchangeType",
    SaveURL: "CurrencyManagement.do?Action=SavePage",
    SaveConversionURL: "CurrencyManagement.do?Action=SaveCurrencyConversion",
    SaveExchangeRateURL: "CurrencyManagement.do?Action=SaveExchangeType",
    BindGridURL: "CurrencyManagement.do?Action=GetCurrencyMasterList&Status=",
    BindGridExchangeURL: "CurrencyManagement.do?Action=GetExchangeTypeList",
    DeleteURL: "CurrencyManagement.do?Action=DeleteCurrencyMasterDtls&CurrencyMasterID=",
    DeleteDetailURL: "CurrencyManagement.do?Action=DeleteCurrencyConversionDtls&CurConversionID=",

    DeleteExchangeURL: "CurrencyManagement.do?Action=DeleteExchangeType&ExchangeMasterID=",
    FillCurrecyDropdown: "CurrencyManagement.do?Action=GetCurrencyForCombo&&bizUnit=",
    GetCurrencyDetails: "CurrencyManagement.do?Action=GetCurrencyDetails&CurrencyID=",
    GetCurrencyDetailsList: "CurrencyManagement.do?Action=GetCurrencyDetailsList&CurrencyID=",
    Inbox: "../AccountManagement/WorkflowInbox.aspx",
    //Constants
    TextZero: "0",
    SaveOk: "SAVED",
    DeleteCommand: "DELETE",
    EditCommand: "EDIT",
    EDITEXCHANGE: "edittype",
    DELETEEXCHANGE: "deletetype",
    DeleteConversion: "deleteconversion",
    DeleteMessageCommand: "DELETECURRENCY",
    DeleteComplete: "DeleteComplete",
    Param: "&MatCagID=",
    DELETE: "DELETE",

    CurrencyMasterID: "CUR_PK",
    CurrencyName: "CUR_NAME",
    CurrencyCode: "CUR_CODE",
    DisplayIn: "",
    Fraction: "CUR_FRACTION",
    Symbol: "CUR_SYMBOL",

    //Exchange DropDown
    ExchangeMasterID: "CUC_PK",
    ExchangeToPK: "CUC_TO",
    ExchangeToName: "CUC_TO_NAME",
    ExchangeFromName: "CUC_FROM_NAME",
    ExchangeToCode: "CUC_TO_CODE",
    ExchangeFromCode: "CUC_FROM_CODE",
    ExchangeRate: "CUC_CONV_FACT",

    //Messages
    MessageBoxTitle: "Translate(Information)",
    ConfirmationMessage: "Translate(Conformation)",
    CurrencySaveMessage: "Translate(CurrencyDetailsSavedSuccessfully)",
    TransacTionExist: "Translate(TransacTionExist)",
    DateRangeAlreadyExists: "Translate(DateRangeAlreadyExists)",
    CodeExistsMessage: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    CurrencyDeleteMessage: "Translate(CurrencyDetailsDeletedSuccessfully)",
    Used: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    ConversionAlreadyAdded: "Translate(ConversionAlreadyAdded)",
    NotAllowedEdit: "Translate(NotAllowedEdit)",
    InvalidDate: "Translate(InvalidDate)",

    //validation messages.
    EnterCurrencyName: "Translate(EnterCurrencyName)",
    EnterCurrencyCode: "Translate(EnterCurrencyCode)",
    EnterCurrencyDecimal: "Translate(EnterDecimal)",
    EnterFraction: "Translate(EnterFraction)",
    selectDisplayin: "Translate(selectDisplayin)",
    EnterSymbol: "Translate(EnterSymbol)",
    EnterRate: "Translate(EnterRate)",
    SelectToConversion: "Translate(SelectToConversion)",
    AddExchangeRate: "Translate(AddExchangeRate)",
    EnterFromCurrency: "Translate(EnterFromCurrency)",

    ConversionConfirmMsg: "Translate(ConversionConfirmMsg)"


}
//#endregion

///#region------- Initialization Section --------
//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != TankMaster.TextZero);
}, 'Translate(Pleaseselectanoption)');


$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    ExchDec = $("[id$='hdfExcRateDecimal']").val();

    //Page Initial condtions
    PageInit();


});

function PageInit() {
    ///<summary>Initial page condition</summary>
    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=btnAddNew]").show();
    $("[id$=divData]").hide();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("[id$=divListing]").show();
    $("select[id$=SearchType]").val("0");
    $("[id$=SearchValue]").val("");
    $("[ID$=imbAddLocation]").attr("title", CurrencyMaster.AddExchangeRate);
    $("#divExchange").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveExchangeValidations();
        }
    });

    DateInit();
    // Initialize/Load data to the view state
    CurrencyJson = $.parseJSON($("[id$=CurrencyDetails]").val());
    // CurrencyJson = $.parseJSON($("[id$=ConversionList]").val());
    $("#divData").data("CurrencyData", CurrencyJson);

    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    // FillCurrencyDropdown();
    BindGrid();
    return false;
}
///#endregion

///#region---- Set Or Reset Form----

function AddNew() {
    $("[id$=btnSave]").show();
    $("[id$=btnAddNew]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("[id$=CUR_CODE]").focus();

    return false;
}


function RedirectToInbox() {
    window.location = CurrencyMaster.Inbox;
    return false;
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {

    ClearForm();
    $(document.forms[0]).validate().resetForm();
    var ObjDisp = $("#divData").data("CurrencyData");
    var ajaxUrl = CurrencyMaster.GetCurrencyDetailsList + currencyId;
    $("#grdExchangeRate").removeAttr("ajaxUrl")
    $("#grdExchangeRate").attr("ajaxUrl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    GrandGrid.MakeGrid($("#grdExchangeRate"));

    //    ObjDisp.ConversionList = new Array();
    ////    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    //    if (ObjDisp.ConversionList != null) {
    //        GrandGrid.MakeGrid($("#grdExchangeRate"), 0, ObjDisp.ConversionList);
    //    }
    //    //$(document.forms[0]).validate().resetForm();
    //    $("#divData").data("CurrencyData", ObjDisp);
    PageInit();
    $("#divExchange").dialog("close");
    return false;
}

function ClearForm() {
    $("[id$=SearchType]").focus();
    $("[id$=CUR_PK]").val("0");
    $("[id$=CUR_NAME]").val("");
    $("[id$=CUR_DECIMAL]").val(1);
    $("[id$=CUR_CODE]").val("");
    $("[id$=CUR_FORMAT]").val("0");
    $("[id$=CUR_FRACTION]").val("");
    $("[id$=CUR_SYMBOL]").val("");
    $("[id$=CUC_PK]").val("");
    $("[id$=CUC_TO]").val("0");
    $("[id$=CUC_CONV_FACT]").val("");
    //$("[id$=CUC_FROM_DATE]").rules("remove");
    //$("[id$=CUC_TO_DATE]").rules("remove");
}
///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        //BindGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
    }
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", CurrencyMaster.AutoCompleteURL + $("[id$=BizUnitPk]").val(), "SearchType");
}
///#endregion

///#region----Grid Handlers And Model Popup Ok Click----

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    // RemoveValidations();
    switch (command.toString()) {
        // To Delete Details        
        case CurrencyMaster.DeleteCommand:
            currencyMasterID = GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.CurrencyMasterID, $(tr).parents("table:first").attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(CurrencyMaster.DeleteConfirmationMessage, CurrencyMaster.ConfirmationMessage, CurrencyMaster.DeleteMessageCommand, true);
            break;

        // To Edit Details                
        case CurrencyMaster.EditCommand:
            FillDetails(tr);
            break;
        // Default Handler      
        default:
            alert(CurrencyMaster.DefaultAction);
            break;
    }
    return false;

}
///<summary>Grid Handler Catches all grid events from vendor Type popup </summary>
function GridHandlerType(tr, command) {
    switch (command.toString().toLowerCase()) {

        case CurrencyMaster.EDITEXCHANGE:
            FillExchangeDetails(tr);
            return false;
            break;

        case CurrencyMaster.DELETEEXCHANGE:
            //DeleteConversionDetails(tr);
            toUnitId = GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeToPK, $(tr).parent().parent().attr("id"));
            exchangeMasterID = GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeMasterID, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(CurrencyMaster.DeleteConfirmationMessage, CurrencyMaster.ConfirmationMessage, CurrencyMaster.DeleteConversion, true);
            return false;
            break;

        default:
            GrandScriptUtils.ShowModal(CurrencyMaster.DefaultAction, CurrencyMaster.MessageBoxTitle);
            return false;
            break;


    }
    //return false;

}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {

    switch (command) {


        case CurrencyMaster.DeleteMessageCommand:
            DeleteDetails();
            break;
        case CurrencyMaster.DeleteConversion:
            DeleteConversionDetails();
            break;
        case CurrencyMaster.SaveOk:
            PageInit();
            break;
        case CurrencyMaster.DeleteComplete:
            BindGrid();
            break;



    }
    return false;
}
///#endregion

///#region---- Fetch Data To Populate In Controls
function FillDetails(tr) {

    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param>
    //Get OrderID From tr - For Pass this as QueryString
    currencyId = GrandGrid.Utilities.GetColumnValue(tr, "CUR_PK", $(tr).parent().parent().attr("id"));
    var objDisp = new Object();
    $.get(CurrencyMaster.GetCurrencyDetails + currencyId, function (data) {

        $("#divData").data("CurrencyData", data);
        FillCurrencyDetails();

    });



}

///Method to fill currency details 
function FillCurrencyDetails() {
    //var grdID = $(tr).parents("table:first").attr("id");
    debugger;
    CurrencyJson = $("#divData").data("CurrencyData");
    if (!($.isArray(CurrencyJson.ConversionList))) {
        var objArray = CurrencyJson.ConversionList;
        CurrencyJson.ConversionList = new Array();
        if (objArray != null) {
            CurrencyJson.ConversionList.push(objArray);
        }
    }

    $("input[id$=CUR_PK]").val(CurrencyJson.CUR_PK);
    $("input[id$=CUR_NAME]").val(CurrencyJson.CUR_NAME);
    $("input[id$=CUR_DECIMAL]").val(CurrencyJson.CUR_DECIMAL);
    $("input[id$=CUR_CODE]").val(CurrencyJson.CUR_CODE);
    $("input[id$=CUR_FRACTION]").val(CurrencyJson.CUR_FRACTION);
    $("select[id$=CUR_FORMAT]").val(CurrencyJson.CUR_FORMAT);
    $("input[id$=CUR_SYMBOL]").val(CurrencyJson.CUR_SYMBOL);


    var srchV = "";
    var ajaxUrl = CurrencyMaster.GetCurrencyDetailsList + currencyId;
    $("#grdExchangeRate").removeAttr("ajaxUrl")
    $("#grdExchangeRate").attr("ajaxUrl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    GrandGrid.MakeGrid($("#grdExchangeRate"));


    //    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    //    if (CurrencyJson.ConversionList != null && CurrencyJson.ConversionList.length > 0) {
    //        GrandGrid.MakeGrid($("#grdExchangeRate"), 0, CurrencyJson.ConversionList);
    //    }
    //changing mode to material lising
    AddNew();

}
function DateInit() {
    //<summary>function used to make datepicker</summary>

    GrandScriptUtils.DatePicker("CUC_FROM_DATE", false, false);
    GrandScriptUtils.DatePicker("CUC_TO_DATE", false, false);
}

function FillExchangeDetails(tr) {
    var Fromexchange = $("[id$=lblFromExchange]").html();
    var grdID = $(tr).parents("table:first").attr("id");
    if (Fromexchange == GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeFromCode, grdID)) {
        $("input[id$=CUC_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeMasterID, grdID));
        $("[id$=lblFromExchange]").html(GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeFromCode, grdID));
        $("select[id$=CUC_TO]").val(GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeToPK, grdID));
        //$("input[id$=CUC_CONV_FACT]").val(GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeRate, grdID));
        $("input[id$=CUC_CONV_FACT]").val(parseFloat(GrandGrid.Utilities.GetColumnValue(tr, "CUC_CONV_FACTOR", grdID)).toFixed(ExchDec));
        $("input[id$=EditConversion]").val(GrandGrid.Utilities.GetColumnValue(tr, CurrencyMaster.ExchangeMasterID, $(tr).parent().parent().attr("id")));
        $("[id$=CUC_FROM_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, "CUC_FROM_DATE", grdID));
        $("[id$=CUC_TO_DATE]").val(GrandGrid.Utilities.GetColumnValue(tr, "CUC_TO_DATE", grdID));
        $("[id$=CUC_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, "CUC_FROM", grdID));
        toUnitId = $("select[id$=CUC_TO]").val();
    }
    else {
        GrandScriptUtils.ShowModal(CurrencyMaster.NotAllowedEdit, CurrencyMaster.MessageBoxTitle);
    }
}
function FillExchangeType() {
    //<summary>function To Fill Exchange Type Details </summary>
    // Get id of the Exchange DropDown
    var drpID = $("select[id$=CUC_TO]").attr("id");
    //Fill Tank Type to the Category DropDown, Name as Text, PK as Value

    $.get(CurrencyMaster.FillExchangeRateDropdownURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}
function AddExchangeType() {
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        if (CurrencyJson.ConversionList != null && CurrencyJson.ConversionList.length > 0)
            $("#ExchangeDiv").show();

        else
            $("#ExchangeDiv").hide();
        
        $("#divExchange").dialog({ width: 700, height: 450, buttons: {} });
        $("#divExchange").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
        $("[id$=lblFromExchange]").html($("[id$=CUR_CODE]").val());
        $("[id$=CUC_FROM]").val($("[id$=CUR_PK]").val());
        $("[id$=CUC_TO]").focus();
        // BindGridExchangeType();
        ResetConversionDetails();
        FillCurrencyDropdown();
        $("[id$=CUC_CONV_FACT]").val("");
    }
    return false;
}
function ResetConversionDetails() {
    //<summary>Function to Reset Conversion Grid Controls Details </summary>
    var ajaxUrl = CurrencyMaster.GetCurrencyDetailsList + currencyId;
    $("#grdExchangeRate").removeAttr("ajaxUrl")
    $("#grdExchangeRate").attr("ajaxUrl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    GrandGrid.MakeGrid($("#grdExchangeRate"));

    //    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    //    if (CurrencyJson.ConversionList == null) {
    //        CurrencyJson.ConversionList = new Array();
    //    }
    //    GrandGrid.MakeGrid($("#grdExchangeRate"), 0, CurrencyJson.ConversionList);
}
function FillCurrencyDropdown() {
    //<summary>function To Fill Exchange Type Details </summary>
    // Get id of the Exchange DropDown
    var drpID = $("select[id$=CUC_TO]").attr("id");
    //Fill Tank Type to the Category DropDown, Name as Text, PK as Value
    $.get(CurrencyMaster.FillCurrecyDropdown + $("[id$=BizUnitPk]").val(), function (data) {
        var sample = JSLINQ(data).Where(function (item) { return item.Value != $("[id$=CUR_PK]").val() });

        GrandScriptUtils.FillDropDown(drpID, sample.items, true, true);
    });

}

///#endregion

///#region---- Data Management Section----


function DeleteDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(CurrencyMaster.DeleteURL + currencyMasterID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = CurrencyMaster.CurrencyDeleteMessage;
        else if (parseInt(data) == 0)
            msgtxt = CurrencyMaster.Used;
        else
            msgtxt = CurrencyMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, CurrencyMaster.MessageBoxTitle, CurrencyMaster.DeleteComplete);

    });
    return false;
}

function DeleteExchangeRateDetails(DetailID) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(CurrencyMaster.DeleteDetailURL + DetailID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = CurrencyMaster.CurrencyDeleteMessage;
        else if (parseInt(data) == 0)
            msgtxt = CurrencyMaster.Used;
        else if (parseInt(data) == -5) {//-5 Already transaction exists
            msgtxt = CurrencyMaster.TransacTionExist;
        }
        else
            msgtxt = CurrencyMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, CurrencyMaster.MessageBoxTitle, CurrencyMaster.DeleteComplete);
    });
    return false;
}

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = CurrencyMaster.BindGridURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val();
    $("#grdCurrency").removeAttr("ajaxurl")
    $("#grdCurrency").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdCurrency");
    GrandGrid.MakeGrid($("#grdCurrency"));
    return false;
}
function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///<summary>To handle bind grid for TankType Type</summary>
function BindGridExchangeType() {

    var ajaxUrl = CurrencyMaster.GetCurrencyDetailsList + currencyId;
    $("#grdExchangeRate").removeAttr("ajaxUrl")
    $("#grdExchangeRate").attr("ajaxUrl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    GrandGrid.MakeGrid($("#grdExchangeRate"));

    //    CurrencyJson = $("#divData").data("CurrencyData")
    //    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    //   GrandGrid.MakeGrid($("#grdExchangeRate"),0, CurrencyJson.ConversionList);
}
function SaveExchangeType() {
    ///<summary>Function used to saving materials  </summary>
    AddValidations(2);
    if ($(document.forms[0]).valid()) {

        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(CurrencyMaster.SaveExchangeRateURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(CurrencyMaster.CodeExistsMessage, CurrencyMaster.MessageBoxTitle);

            }
            else if (parseInt(data) > 0) {

                // GrandScriptUtils.ShowModal(TankMaster.taMaterialSaveMessage, TankMaster.MessageBoxTitle, TankMaster.SaveCommand);


            }
            else {
                GrandScriptUtils.ShowModal(CurrencyMaster.ActionFailedMessage);
                // ResetPage();
            }

        });
    }
    return false;
}

function SaveConversionDtls() {
    //$(document.forms[0]).validate().resetForm();
    RemoveExchangeValidations();
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        CurrencyJson = $("#divData").data("CurrencyData");
        var editConversion = $("input[id$=EditConversion]").val();
        var obj = new Object();
        var flag = true;
        var confirmflag = true;
        var toDate = ConvertDate($("input[id$=CUC_TO_DATE]").val());
        var fromDate = ConvertDate($("input[id$=CUC_FROM_DATE]").val());

        if (fromDate > toDate) {
            GrandScriptUtils.ShowModal(CurrencyMaster.InvalidDate, CurrencyMaster.MessageBoxTitle);
            return false;
        }

        if (parseInt(toUnitId) == 0) {
            for (var i in CurrencyJson.ConversionList) {
                var conToDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_TO_DATE);
                var conFromDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_FROM_DATE);
                if (CurrencyJson.ConversionList[i].CUC_TO == $("select[id$=CUC_TO]").val() && ((conFromDate <= fromDate && fromDate <= conToDate) || (conFromDate <= toDate && toDate <= conToDate) || (fromDate <= conFromDate && conFromDate <= toDate))) {
                    flag = false;
                    break;
                }
            }

            // Check Conversion From currency 
            for (var i in CurrencyJson.ConversionList) {
                var conToDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_TO_DATE);
                var conFromDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_FROM_DATE);
                if (CurrencyJson.ConversionList[i].CUC_FROM == $("select[id$=CUC_TO]").val() && ((conFromDate <= fromDate && fromDate <= conToDate) || (conFromDate <= toDate && toDate <= conToDate) || (fromDate <= conFromDate && conFromDate <= toDate))) {
                    confirmflag = false;
                    break;
                }
            }
        }
        else {
            // Check Conversion Already Added - For Updation
            for (var i in CurrencyJson.ConversionList) {
                if (CurrencyJson.ConversionList[i].CUC_PK != editConversion || CurrencyJson.ConversionList[i].CUC_TO != toUnitId) {
                    var conToDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_TO_DATE);
                    var conFromDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_FROM_DATE);
                    if (CurrencyJson.ConversionList[i].CUC_TO == $("select[id$=CUC_TO]").val() && ((conFromDate <= fromDate && fromDate <= conToDate) || (conFromDate <= toDate && toDate <= conToDate) || (fromDate <= conFromDate && conFromDate <= toDate))) {
                        flag = false;
                        break;
                    }
                }
                else if (CurrencyJson.ConversionList[i].CUC_TO == toUnitId) {
                    CurrencyJson.ConversionList[i].CUC_CONV_FACT = $("[id$=CUC_CONV_FACT]").val();
                    CurrencyJson.ConversionList[i].CUC_CONV_FACTOR = $("[id$=CUC_CONV_FACT]").val();
                    CurrencyJson.ConversionList[i].CUC_FROM_DATE = $("[id$=CUC_FROM_DATE]").val();
                    CurrencyJson.ConversionList[i].CUC_TO_DATE = $("[id$=CUC_TO_DATE]").val();
                }
            }
            // Check Conversion From currency - For Updation
            for (var i in CurrencyJson.ConversionList) {

                if (CurrencyJson.ConversionList[i].CUC_PK != editConversion || CurrencyJson.ConversionList[i].CUC_TO != toUnitId) {
                    var conToDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_TO_DATE);
                    var conFromDate = ConvertDate(CurrencyJson.ConversionList[i].CUC_FROM_DATE);
                    if (CurrencyJson.ConversionList[i].CUC_FROM == $("select[id$=CUC_TO]").val() && ((conFromDate <= fromDate && fromDate <= conToDate) || (conFromDate <= toDate && toDate <= conToDate) || (fromDate <= conFromDate && conFromDate <= toDate))) {
                        confirmflag = false;
                        break;
                    }
                }
            }

            // Check Details Is new Entry Or to update . If Update Get Details From CurrencyData and Assign to Obj, and Details Will Updated To Object
            for (var i in CurrencyJson.ConversionList) {

                if (parseInt(toUnitId) == CurrencyJson.ConversionList[i].CUC_TO)

                    obj = CurrencyJson.ConversionList[i];
            }

        }
        // Check Already Added Or Not
        if (flag) {
            if (confirmflag == false) {
                var msgTitle;
                var msg;
                msgTitle = CurrencyMaster.MessageBoxTitle;
                msg = CurrencyMaster.ConversionConfirmMsg;
                msg = msg.replace('#from#', $("select[id$=CUC_TO] :selected").text());
                msg = msg.replace('#to#', $("[id$=lblFromExchange]").html());
                $("#divConfirmation").html(msg).dialog({
                    modal: true,
                    height: 150,
                    width: 350,
                    title: msgTitle,
                    resizable: false,
                    buttons: {
                        OK: function (e) {
                            $(this).dialog("close");
                            SaveConversion();
                        },
                        Cancel: function (e) {
                            $(this).dialog("close");
                            return false;
                        }
                    }
                });
            }
            else {
                SaveConversion();
            }
        }
        else {

            GrandScriptUtils.ShowModal(CurrencyMaster.ConversionAlreadyAdded, CurrencyMaster.MessageBoxTitle);
        }

        return false;
    }


}

function SaveConversion() {
    CurrencyJson = $("#divData").data("CurrencyData");
    var obj = new Object();
    // Add One By one Details To Object

    obj.CUC_TO = $("[id$=CUC_TO]").val();
    obj.CUC_TO_NAME = $("select[id$=CUC_TO] :selected").text();
    obj.CUC_FROM_NAME = $("[id$=lblFromExchange]").html();
    obj.CUC_TO_CODE = $("select[id$=CUC_TO] :selected").text();
    obj.CUC_FROM_CODE = $("[id$=lblFromExchange]").html();
    obj.CUC_CONV_FACT = parseFloat($("[id$=CUC_CONV_FACT]").val());
    obj.CUC_CONV_FACTOR = parseFloat($("[id$=CUC_CONV_FACT]").val());
    obj.CUC_FROM_DATE = $("[id$=CUC_FROM_DATE]").val();
    obj.CUC_TO_DATE = $("[id$=CUC_TO_DATE]").val();
    obj.CUC_PK = $("[id$=CUC_PK]").val();
    CurrencyJson.ConversionList = new Array();
    CurrencyJson.ConversionList.push(obj);

    // Check Add Details - For New Entry
    if (parseInt(toUnitId) == 0) {
        // If Yes - get Length of the List and Assign Length+1 as the PK of New Entry
        //  CurrencyJson.ConversionList.length + 1;
        // $("[id$=CUC_PK]").val();
        //Push Object to List
        //        CurrencyJson.ConversionList = new Array();
        //        CurrencyJson.ConversionList.push(obj);
    }

    $("#divData").data("CurrencyData", CurrencyJson);
    SaveCurrencyConversion();
    CurrencyJson.ConversionList = new Array();
    $("#divData").data("CurrencyData", CurrencyJson);
    //     Bind Conversion Details Grid
    //    GrandGrid.MakeGrid($("#grdExchangeRate"), 0, CurrencyJson.ConversionList);
    //     GrandScriptUtils.ShowModal(UOMMaster.SAVECONVERSION, UOMMaster.INFORMATIONTITLE);
    //    if (CurrencyJson.ConversionList != null && CurrencyJson.ConversionList.length > 0)
    //        $("#ExchangeDiv").show();

    //    else
    //        $("#ExchangeDiv").hide();

}


///<summary>For delete the item in the grid - Conversion Details List</summary>
function DeleteConversionDetails() {
    DeleteExchangeRateDetails(exchangeMasterID);
    ShowExchangeRate();

    //    CurrencyJson = $("#divData").data("CurrencyData");
    //    // Delete Conversion Details - By MaintanceInfoID Using Loop
    //    for (var i in CurrencyJson.ConversionList) {
    //        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
    //        if (CurrencyJson.ConversionList[i].CUC_TO == toUnitId) {
    //            // Splice Details From List, Corresponding ToUnit
    //            CurrencyJson.ConversionList.splice(i, 1);
    //            break;
    //        }
    //    }

    //    //    var ajaxUrl = CurrencyMaster.GetCurrencyDetailsList + currencyId;
    //    //    $("#grdExchangeRate").removeAttr("ajaxUrl")
    //    //    $("#grdExchangeRate").attr("ajaxUrl", ajaxUrl);
    //    //    GrandGrid.Utilities.ResetGrid(true, "grdExchangeRate");
    //    //    GrandGrid.MakeGrid($("#grdExchangeRate"));

    //    $("#divData").data("CurrencyData", CurrencyJson);
    //    GrandGrid.MakeGrid($("#grdExchangeRate"), 0, CurrencyJson.ConversionList);
    //    if (CurrencyJson.ConversionList != null && CurrencyJson.ConversionList.length > 0)
    //        $("#ExchangeDiv").show();

    //    else
    //        $("#ExchangeDiv").hide();
    //    ClearExchangeDtls();

    return false;

}
///Method to clear exchange details
function ClearExchangeDtls() {

    $("select[id$=CUC_TO]").val("0");
    $("input[id$=CUC_CONV_FACT]").val("");
    $("input[id$=EditConversion]").val("0");
    $("[id$=CUC_FROM_DATE]").val("");
    $("[id$=CUC_TO_DATE]").val("");
    // $("[id$=CUC_FROM]").val("0");
    $("[id$=CUC_PK]").val("0");



    frmUnitID = 0;
    toUnitId = 0;
}

function DeleteExchangeTypeDetails(tr) {
    ///<summary>Function To Get delete and Delete categoryDetails, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(CurrencyMaster.DeleteExchangeURL + exchangeMasterID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = CurrencyMaster.MaterialDeleteMessage;
        else if (parseInt(data) == 0)
            msgtxt = CurrencyMaster.Used;
        else
            msgtxt = CurrencyMaster.ActionFailedMessage;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, CurrencyMaster.MessageBoxTitle, CurrencyMaster.DELETEEXCHANGE);

    });
    return false;
}
function SavePage() {
    ///<summary>Function used to saving materials  </summary>
    //    $("#divExchange").dialog("close");
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("CurrencyData");
        //Assigning the Material details to a hidden field by converting the object to string using Json Stringify Methord
        $("[id$=ConversionList]").val(JSON.stringify(ObjDisp.ConversionList));
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(CurrencyMaster.SaveURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            $("#updateProgress").hide();
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(CurrencyMaster.CodeExistsMessage, CurrencyMaster.MessageBoxTitle);
                $("input[id$=CUR_CODE]").focus();

            }
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(CurrencyMaster.CurrencySaveMessage, CurrencyMaster.MessageBoxTitle, CurrencyMaster.SaveOk);
                $("#divExchange").dialog("close");
            }
            else {
                GrandScriptUtils.ShowModal(CurrencyMaster.ActionFailedMessage);
                // ResetPage();
            }

        });
    }
    return false;
}
///#endregion

function SaveCurrencyConversion() {
    ///<summary>Function used to saving materials  </summary>
    // if ($(document.forms[0]).valid()) {
    var ObjDisp = $("#divData").data("CurrencyData");
    //Assigning the Material details to a hidden field by converting the object to string using Json Stringify Methord
    $("[id$=ConversionList]").val(JSON.stringify(ObjDisp.ConversionList));
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    $.post(CurrencyMaster.SaveConversionURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
        $("#updateProgress").hide();
        if (parseInt(data) == -5) {//-5 Already transaction exists
            GrandScriptUtils.ShowModal(CurrencyMaster.TransacTionExist, CurrencyMaster.MessageBoxTitle);
            $("input[id$=CUC_CONV_FACT]").focus();
        }
        else if (parseInt(data) == -4) {  //-4 Already exists in date range
            GrandScriptUtils.ShowModal(CurrencyMaster.DateRangeAlreadyExists, CurrencyMaster.MessageBoxTitle);
            $("input[id$=CUC_CONV_FACT]").focus();
        }
        else if (parseInt(data) == 0) {
            GrandScriptUtils.ShowModal(CurrencyMaster.CodeExistsMessage, CurrencyMaster.MessageBoxTitle);
        }
        else if (parseInt(data) > 0) {
            GrandScriptUtils.ShowModal(CurrencyMaster.CurrencySaveMessage, CurrencyMaster.MessageBoxTitle, CurrencyMaster.SaveOk);
            ShowExchangeRate();
            ClearExchangeDtls();
            $("#divExchange").dialog("close");
        }
        else {
            GrandScriptUtils.ShowModal(CurrencyMaster.ActionFailedMessage);
            // ResetPage();
        }

    });
    //}
    return false;
}

function ShowExchangeRate() {
    $("#ExchangeDiv").show();
    $("#divExchange").dialog({ width: 700, height: 450, buttons: {} });
    $("#divExchange").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    //    $("[id$=lblFromExchange]").html($("[id$=CUR_CODE]").val());
    //    $("[id$=CUC_FROM]").val($("[id$=CUR_PK]").val());
    //    $("[id$=CUC_TO]").focus();
    // BindGridExchangeType();
    ResetConversionDetails();
    FillCurrencyDropdown();
    $("[id$=CUC_CONV_FACT]").val("");
    return false;
}



///#region---------- Validations ----------------
function AddValidations(mode) {
    RemoveValidations();
    ///<summary>function To Validations </summary>
    if (mode == 1) {
        $("input[id$=CUR_NAME]").rules("add", {
            required: true,
            maxlength: 100,
            minlength: 3,
            messages: { required: CurrencyMaster.EnterCurrencyName }
        });
        $("input[id$=CUR_CODE]").rules("add", {
            required: true,
            minlength: 1,
            messages: { required: CurrencyMaster.EnterCurrencyCode }

        });

        $("input[id$=CUR_DECIMAL]").rules("add", {
            required: true,
            minlength: 1,
            messages: { required: CurrencyMaster.EnterCurrencyDecimal }

        });

        $("input[id$=CUR_FRACTION]").rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: CurrencyMaster.EnterFraction }

        });
        $("select[id$=CUR_FORMAT]").rules("add", {
            selectNone: true,
            messages: { selectNone: CurrencyMaster.selectDisplayin }
        });
        //        $("input[id$=CUR_SYMBOL]").rules("add", {
        //            required: true,
        //            messages: { required: CurrencyMaster.EnterSymbol }
        //        });


    }
    else
        if (mode == 2) {
            $("select[id$=CUC_TO]").rules("add", {
                selectNone: true,
                messages: { selectNone: CurrencyMaster.SelectToConversion }
            });
            $("input[id$=CUC_CONV_FACT]").rules("add", {
                required: true,
                maxlength: 10,
                number: true,
                DecimalDigits: ExchDec,
                CustomDecimal: true,
                messages: { required: CurrencyMaster.EnterRate, CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", ExchDec) }
            });
            $("input[id$=CUC_FROM_DATE]").rules("add", {
                date: true,
                required: true,
                messages: { required: "Translate(ReqRequredDate)" }
            });
            $("input[id$=CUC_TO_DATE]").rules("add", {
                date: true,
                required: true,
                messages: { required: "Translate(ReqRequredToDate)" }
            });

        }
        else if (mode == 3) {
            $("input[id$=CUR_CODE]").rules("add", {
                required: true,
                minlength: 1,
                messages: { required: CurrencyMaster.EnterCurrencyCode }

            });
        }
}
//<summary>function Remove Validation</summary>
function RemoveValidations() {
    $("[id$=CUR_NAME]").rules("remove");
    $("[id$=CUR_CODE]").rules("remove");
    $("[id$=CUR_DECIMAL]").rules("remove");
    $("[id$=CUR_FRACTION]").rules("remove");
    $("[id$=CUR_FORMAT]").rules("remove");
    //    $("[id$=CUR_SYMBOL]").rules("remove");


}
function RemoveExchangeValidations() {
    $("[id$=CUC_TO]").rules("remove");
    $("[id$=CUC_CONV_FACT]").rules("remove");
    $("[id$=CUC_FROM_DATE]").rules("remove");
    $("[id$=CUC_TO_DATE]").rules("remove");
}

//<summary>function Convert Date</summary>
function ConvertDate(str) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      n = months.length, re = /(\d{2})-([a-z]{3})-(\d{4})/i, matches;

    while (n--) { months[months[n]] = n; } // map month names to their index :)

    matches = str.match(re); // extract date parts from string

    return new Date(matches[3], months[matches[2]], matches[1]);
}
///#endregion

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>    
    if (grdID == "grdExchangeRate") {
        var colIndex = 0;
        var rate = 0;
        $("#grdExchangeRate tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CUC_CONV_FACT", grdID);
            rate = GrandGrid.Utilities.GetColumnValue($(this), "CUC_CONV_FACT", grdID);
            if (colIndex != null) {
                if (parseFloat(rate))
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(rate).toFixed(ExchDec));
            }
        });
    }
}





