/// <reference path="../../../jquery/jquery-1.5.min.js" />
/// <reference path="../../../GrandScriptUtils.js" />


///#region------ Global Variable Declaration ---------------
var TaxID;
//#endregion

///#region------ Configuration Settings ---------------
var TaxSettings = {
    //Url
    GetTaxParameters: "TaxSettings.do?Action=GetTaxParameters",
    SaveTaxParameters: "TaxSettings.do?Action=SaveTaxDetails",
    GetTaxDetails: "TaxSettings.do?Action=GetTaxDetails",
    TaxAutoComplete :"TaxSettings.do?Action=GetSearchValue",
    //Commands
    Saved: "SAVED",
    Edit: "EDIT",
    Delete:"DELETE",
    InvalidExpression: "INVALIDEXPRESSION",
    DeleteMessageCommand:"DELETE",
    //MESSAGE
    Title : "Information",
    InvalidExpression: "Invalid Expression",
    SavedSuccessfully: "Tax setting saved sucessfully",
    ActionFailedMessage: "Action Failed",
    DeleteConfirmationMessage: "",
    ConfirmationMessage:"Do you want to delete",
    UpdatedSuccessfully :"Tax Settings updated successfuly",

    //Fields
    TaxPK: "TAX_PK",
    TAX_Type :"TAX_Type",
    TAX_DISC_FROM_DT :"TAX_DISC_FROM_DT",
    TAX_EXT_FROM_DT :"TAX_EXT_FROM_DT",
    TAX_EXT_TO_DATE:"TAX_EXT_TO_DATE",
    TAX_HEAD:"TAX_HEAD",
    TAX_FORMULA: "TAX_FORMULA",
    TAX_FROM_DT :"TAX_FROM_DT",
    TAX_TO_DT:"TAX_TO_DT"



}
//#endregion

///#region------ Initialization Section ---------------

$(document).ready(function () {
    PageInit();
});

function PageInit() {
    ///<summary>Function Initialize page details </summary>
    $("[id$=imbSave]").hide();
    $("[id$=imbAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("#DIVTAX_DISC").hide();
    $("#DIVTAX_EXT").hide();
    //Filling tax paremeters    
    FillParmeters();
    //GrandScriptUtils.DatePicker("DISC_FROM", false, false);
    GrandScriptUtils.AddDateRange("TAX_FROM_DT", "taxfromdt", "TAX_TO_DT", "taxtodt", false, false);
    //GrandScriptUtils.AddDateRange("EXT_FROM", "hdfextfrom", "EXT_TO", "hdfextto", false, false);
    $("[id$=EXT_FROM]").val("");
    $("[id$=hdfextfrom]").val("");
    $("[id$=EXT_TO]").val("");
    $("[id$=hdfextto]").val("");
    $("[id$=DISC_FROM]").val("");

    $("[id$=BIZUNIT]").val($("[id$=SBU]").val());
    $("[id$=UserID]").val($("[id$=UserPk]").val());
    $(".checkbx").hide();
    BindGrid();
    //setting search type.
    SetSearchType();
    //initializing search.
    SearchInit();
    return false;
}

function FillParmeters(paramid) {
    ///<summary>Function used to fill Paremeter details for tax </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=TAX_PARM]").attr("id");
    //Fill Parameters Details to the paramaeter DropDown, Name as Text, PK as Value
    $.get(TaxSettings.GetTaxParameters, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });

}

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        BindGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
    }
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", TaxSettings.TaxAutoComplete, "SearchType",false,"SBU");
}


function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///#endregion

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    //    if (srchVal)
    //        srchV = srchVal;
    //    else
    //        srchV = $("[id$=SearchType]").val() == "0" ? "0" : $("[id$=SearchType]").val() + " Like '%" + $("[id$=SearchValue]").val() + "%'";
    //    srchV = GrandScriptUtils.FixURLEncoding(srchV);
    //    var ajaxUrl = MaterialMaster.MaterialBindGridURL + srchV;

    var ajaxUrl = TaxSettings.GetTaxDetails +"&Status="+$("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("select[id$=SBU]").val();
    $("#grdTaxDetails").removeAttr("ajaxurl");
    $("#grdTaxDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTaxDetails");
    GrandGrid.MakeGrid($("#grdTaxDetails"));
    return false;
}

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    $("[id$=imbSave]").show();
    $("[id$=imbAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("input[id$=TAX_HEAD]").focus();
    $(".checkbx").hide();
    return false;
}



//#endregion

///#region------ Core Section ---------------

function TaxDetails(controlID) {
    ///<summary>Function used to fill Paremeter details for tax </summary>
    /// <param name="controlID"  type="Object">
    ///Control which causes the event to raise
    /// </param>
    var controlID = $(controlID).attr("id");
    if (controlID.indexOf('TAX_DISC') != -1) {
        AddBetweenDateRange("taxfromdt", "taxtodt", "DISC_FROM", "taxdisFrm", false, false);
        $("#DIVTAX_DISC").show();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TAX_EXT]").attr("checked", false);
        $("input[id$=DISC_FROM]").val("");
       
    }
    else if (controlID.indexOf('TAX_EXT') !=-1){
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").show();
        $("input[id$=TAX_DISC]").attr("checked", false);
        $("input[id$=EXT_FROM]").val("");
        $("input[id$=EXT_TO]").val("");
        AddAfterDateRange("taxtodt","EXT_FROM","hdfextfrom","EXT_TO","hdfextto" , false,false);
    }
    return false;
}

function MakeFormula() {
    ///<summary>Function used to Fill Formula corresponding to the paramtere selected </summary>
    
    var tempstr = $("input[id$=TAX_Formula]").val();
    if ($("select[id$=TAX_PARM]").val() != "0") {
        tempstr = tempstr + $("select[id$=TAX_PARM]").val();
        $("input[id$=TAX_Formula]").val(tempstr);
    }
    return false;
}

function SavePage() {
    Addvalidation()
    if ($(document.forms[0]).valid()) {
        var jSonString = GrandScriptUtils.FormToJsonString("divData");
        $.post(TaxSettings.SaveTaxParameters, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                //GrandScriptUtils.ShowModal(MaterialMaster.MaterialCodeExistsMessage, MaterialMaster.SaveCommand);
            }
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(TaxSettings.SavedSuccessfully, TaxSettings.Title, TaxSettings.Saved, false);
                ResetPage();
                BindGrid();
            }
            else if (data == -5) {
                GrandScriptUtils.ShowModal(TaxSettings.InvalidExpression, TaxSettings.Title, TaxSettings.InvalidExpression, false);

            }
            else {
                GrandScriptUtils.ShowModal(TaxSettings.ActionFailedMessage);
                ResetPage();
            }

        });
        return false;
    }
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req
        case TaxSettings.InvalidExpression:
            $("[id$=TAX_Formula]").focus();
            break;
        //Commend When calling
        case TaxSettings.SavedSuccessfully:
            PageInit();
            break;
    }
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler Catch all the grid events in this function </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    switch (command.toString()) {
        // To Delete Details    
        case TaxSettings.Delete:
            TaxID = GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TaxPK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(TaxSettings.DeleteConfirmationMessage, TaxSettings.ConfirmationMessage, TaxSettings.DeleteMessageCommand, true);
            break;

        // To Edit Details             
        case TaxSettings.Edit:
            FillDetails(tr);
            break;
        // Default Handler   
        default:
            alert(TaxSettings.DefaultAction);
            break;
    }
    return false;

}

function FillDetails(tr) {

    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param> 
    AddNew();
    var tableId = $(tr).parents("table:first").attr("id");
    var Taxtype = GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_Type, tableId);
    $("input[id$=TaxType]").val(Taxtype);
    $(".checkbx").show();
    $("input[id$=TAX_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TaxPK, tableId));
    $("input[id$=TAX_HEAD]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_HEAD, tableId));
    $("input[id$=TAX_Formula]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FORMULA, tableId));
    $("input[id$=TAX_FROM_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId));
    $("input[id$=taxfromdt]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId)));
    $("input[id$=TAX_TO_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId));
    $("input[id$=taxtodt]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId)));
    if (Taxtype == "0") {
        $("input[id$=TAX_EXT]").attr("checked", false);
        $("input[id$=TAX_DISC]").attr("checked", false);
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_TO_DT, tableId)));        
        
    }
    else if (Taxtype == "1") {
        $("input[id$=TAX_EXT]").attr("checked", false);
        $("input[id$=TAX_DISC]").attr("checked", true);
        $("input[id$=DISC_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_DISC_FROM_DT, tableId));
        $("#DIVTAX_DISC").show();
        $("#DIVTAX_EXT").hide();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_FROM_DT, tableId)));

        AddBetweenDateRange("TaxDate", "taxtodt", "DISC_FROM", "taxdisFrm", false, false);
    }
    else if (Taxtype == "2") {
        $("input[id$=TAX_EXT]").attr("checked", true);
        $("input[id$=TAX_DISC]").attr("checked", false);
        $("input[id$=EXT_FROM]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_FROM_DT, tableId));
        $("input[id$=EXT_TO]").val(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId));
        $("#DIVTAX_DISC").hide();
        $("#DIVTAX_EXT").show();
        $("input[id$=TaxDate]").val(GrandScriptUtils.ConvertDateFormat(GrandGrid.Utilities.GetColumnValue(tr, TaxSettings.TAX_EXT_TO_DATE, tableId)));
        AddAfterDateRange("TaxDate", "EXT_TO", "hdfextto", false, false, false, false);
    }
   


}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    Removevalidation();
    $("[id$=imbSave]").hide();
    $("[id$=imbAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("#DIVTAX_DISC").hide();
    $("#DIVTAX_EXT").hide();
    $("input[id$=TAX_EXT]").attr("checked", false);
    $("input[id$=TAX_DISC]").attr("checked", false);

    $(document.forms[0]).find("input:not(input[type=submit],input[type=button])").each(function () {
        var idval = $(this).attr("id");
        if (!Checkstatus(idval)) {
            $(this).val("");
        }
    });
    $("select[id$=TAX_PARM]").val("0");
    $("input[id$=TAX_PK]").val("0");
    //setting search type.
    $("select[id$=SearchType]").val("0");
    SetSearchType();
    //initializing search.
    SearchInit();
    return false;

}

function Checkstatus(controlID) {
    //<summary>function Used to Check the status befor clearing the input</summary>
    //Reseting all input controls in the page
    if (controlID.search("UserID") != -1) {
        return true;
    }
    if (controlID.search("TAX_PK") != -1) {
        return true;
    }
    if (controlID.search("BIZUNIT") != -1) {
        return true;
    }
    if (controlID.search("UserPK") != -1) {
        return true;
    }
    return false;
}

//#endregion

///#region------ Validations ---------------
function Addvalidation() {
    $("input[id$=TAX_HEAD]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Enter Tax Head" }
    });
    $("input[id$=TAX_Formula]").rules("add", {
        required: true,
        maxlength: 500,
        messages: { required: "Enter Tax Formula" }
    });
    
    $("input[id$=TAX_TO_DT]").rules("add", {
        required: true,
        //date: true,
        maxlength: 200,
        messages: { required: "Enter Tax To Date" }
        //messages: { date: "Enter a valid Tax To Date" }
    });

    $("input[id$=TAX_FROM_DT]").rules("add", {
        required: true,
        //date:true,
        maxlength: 200,
        messages: { required: "Enter Tax From Date" }
        //messages: { date: "Enter a valid Tax From Date" }
    });
}

function Removevalidation() {
    $(document.forms[0]).validate().resetForm();
    $("input[id$=TAX_HEAD]").rules("remove");
    $("input[id$=TAX_Formula]").rules("remove");
    $("input[id$=TAX_TO_DT]").rules("remove");
    $("input[id$=TAX_FROM_DT]").rules("remove");
}

//#endregion


function AddAfterDateRange(hdfTo, fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate) {
    ///<summary>
    ///     Used for From Date and To Date datepicker
    ///</summary>
    /// <param name="fromDate" optional="true" type="String">
    ///     The input id of the fromdate datepicker
    /// </param>
    /// <param name="hdnFrmDate" optional="true" type="String">
    ///     The hidden field id used for the set min date of the todate datepicker
    /// </param>
    /// <param name="toDate" optional="true" type="String">
    ///     The input id of the todate datepicker
    /// </param>
    /// <param name="hdnToDate" optional="true" type="String">
    ///      The hidden field id used for the set max date of the fromdate datepicker
    /// </param>
    /// <param name="format" optional="true" type="String">
    ///      Format of the datepicker
    /// </param>
    /// <param name="restrictfromDate" optional="true" type="bool">
    ///      true used for set the it will not allow to select the  the current before date
    /// </param>
    if (!format)
        format = "dd-M-yy";
    $('input[id$=' + fromDate + ']').datepicker({
        dateFormat: format,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
        },
        altField: $("[id$=" + hdnFrmDate + "]"),
        altFormat: "mm/dd/yy" //,
        //defaultDate: GrandScriptUtils.FillDate(fromDate, hdnFrmDate)
    });
    $('input[id$=' + toDate + ']').datepicker({
        dateFormat: format,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdnToDate + "]").val()));
        },
        altField: $("[id$=" + hdnToDate + "]"),
        altFormat: "mm/dd/yy"

    });
    $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdfTo + "]").val()));
    $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date());
    if (restrictfromDate)
        $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());


}

function AddBetweenDateRange(hdnFrmDate, hdfToDate, btwDate, hdnBtwDate, format, restrictfromDate) {
    ///<summary>
    ///     Used for From Date and To Date datepicker
    ///</summary>
    /// <param name="hdnFrmDate" optional="true" type="String">
    ///     The hidden field id used for get From Date 
    /// </param>
    /// <param name="hdfToDate" optional="true" type="String">
    ///     The hidden field id used for get To Date 
    /// </param>
    /// <param name="btwDate" optional="true" type="String">
    ///     The input id of the Between datepicker
    /// </param>
    /// <param name="hdnbtwDate" optional="true" type="String">
    ///      The hidden field id used for the set Between Date 
    /// </param>
    /// <param name="format" optional="true" type="String">
    ///      Format of the datepicker
    /// </param>
    /// <param name="restrictfromDate" optional="true" type="bool">
    ///      true used for set the it will not allow to select the  the current before date
    /// </param>
    if (!format)
        format = "dd-M-yy";
    $('input[id$=' + btwDate + ']').datepicker({
        dateFormat: format,
        changeMonth: true,
        changeYear: true,
        altField: $("[id$=" + hdnBtwDate + "]"),
        altFormat: "mm/dd/yy"

    });
    $('input[id$=' + btwDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
    $('input[id$=' + btwDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdfToDate + "]").val()));
    if (restrictfromDate)
        $('input[id$=' + btwDate + ']').datepicker("option", "minDate", new Date());
}
