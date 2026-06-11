/// <reference path="../../../jquery/jquery-1.5.min.js" />
/// <reference path="../../../GrandScriptUtils.js" />


///#region------ Global Variable Declaration ---------------
var ProjectID;
//#endregion

///#region------ Configuration Settings ---------------
var ProjectSiteSettings = {
    //Url
    //GetTaxParameters: "ProjectSite.do?Action=GetTaxParameters",
    SaveProjectDetails: "ProjectSite.do?Action=SaveProjectSiteDetails",
    DeleteProjectSite: "ProjectSite.do?Action=DeleteProjectSite&ProjectID=",
    GetProjectDetails: "ProjectSite.do?Action=GetProjectSiteList",
    ProjectSiteAutoComplete: "ProjectSite.do?Action=GetSearchValue",
    GetTaxCategory: "ProjectSite.do?Action=GetTaxCategory",
    // Command Section
    SaveCommand: "SAVE",
    EDIT: "EDIT",
    DELETE: "DELETE",
    ActivceCommand: "Active",
   
    InvalidExpression: "INVALIDEXPRESSION",
    DeleteMessageCommand: "DELETE",
    //MESSAGE
    Title: "Information",
    MessageBoxTitle: "Translate(Information)",
    ProjectSiteSaveMessage: "Translate(ProjectSiteSavedSuccesfully)",
    ProjectCodeValidation: "Translate(EnterProjectSiteCode)",
    ProjectNameValidation: "Translate(EnterProjectSiteName)",
    ProjectSiteIDExistsMessage: "Translate(ProjectSiteIDExists)",
    InvalidExpression: "Translate(InvalidExpression)",
    SavedSuccessfully: "Translate(TaxSaved)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    UpdatedSuccessfully: "Translate(TaxUpdate)",
    DeletedSuccessfully: "Translate(ProjectSiteDeletedSuccessfully)",
    AlreadyAssigned: "Translate(TaxMasterAlreadyAssigned)",

    //Fields
    PK: "LOC_PK",
    SITEID: "LOC_CODE",
    SITENAME: "LOC_NAME",
    DESCRIPTION: "LOC_DESC",
    ACTIVE: "LOC_ACTIVE",
    MODDATE: "LOC_MOD_DT"
   

}
//#endregion

///#region------ Initialization Section ---------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageInit();
});

function PageInit() {
    ///<summary>Function Initialize page details </summary>
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=LOC_BIZUNIT]").val($("[id$=BizUnitPk]").val());
    $("[id$=UserID]").val($("[id$=UserPk]").val());
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    BindGrid();
   //setting search type.
    SetSearchType();
   //initializing search.
    SearchInit();
    return false;
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
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", ProjectSiteSettings.ProjectSiteAutoComplete, "SearchType", false, "SBU");
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///#endregion

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    var ajaxUrl = ProjectSiteSettings.GetProjectDetails + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val();
    //var ajaxUrl = ProjectSiteSettings.GetProjectDetails + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() ;
    $("#grdProjectSite").removeAttr("ajaxurl");
    $("#grdProjectSite").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdProjectSite");
    GrandGrid.MakeGrid($("#grdProjectSite"));
    return false;
}

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    $("[id$=btnSave]").show();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    return false;
}

//#endregion

///#region------ Core Section ---------------

function FillDetails(tr) {

    ///<summary>// Fill material  Details for edit</summary>
    /// <param name="tr"  type="object">
    ///      edited row
    /// </param> 
    AddNew();
    var tableId = $(tr).parents("table:first").attr("id");
    $("input[id$=LOC_CODE]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.SITEID, tableId));
    $("input[id$=LOC_CODE]").attr("disabled", true);
    $("input[id$=LOC_CODE]").addClass("input-disabled");
    $("textarea[id$=LOC_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.DESCRIPTION, tableId));
    $("input[id$=LOC_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.SITENAME, tableId));
    $("input[id$=LOC_NAME]").attr("disabled", true);
    $("input[id$=LOC_NAME]").addClass("input-disabled");
    $("input[id$=LOC_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.PK, tableId));
    $("input[id$=LOC_MOD_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.MODDATE, tableId)); 
}

function ClearPage() {
    $("input[id$=LOC_CODE]").val("");
    $("input[id$=LOC_CODE]").removeClass("input-disabled");
    $("input[id$=LOC_CODE]").attr("disabled", false);
    $("textarea[id$=LOC_DESC]").val("");
    $("input[id$=LOC_NAME]").removeClass("input-disabled");
    $("input[id$=LOC_NAME]").attr("disabled", false);
    $("input[id$=LOC_NAME]").val("");
    $("input[id$=LOC_PK]").val("0");
    $("input[id$=LOC_MOD_DT]").val(""); 
}
function SavePage() {
    AddValidations();
    $("input[id$=LOC_CODE]").attr("disabled", false);
    $("input[id$=LOC_NAME]").attr("disabled", false);
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {
        $("#updateProgress").show();
        $.post(ProjectSiteSettings.SaveProjectDetails, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ProjectSiteSaveMessage, ProjectSiteSettings.MessageBoxTitle, ProjectSiteSettings.SaveCommand);
                BindGrid();
                ResetPage();
            }
            else if (parseInt(result) == -2)
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ProjectSiteIDExistsMessage, ProjectSiteSettings.MessageBoxTitle);
            else {
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ActionFailedMessage, ProjectSiteSettings.MessageBoxTitle);
                ResetPage();
            }
            ClosePopup();
        });
    }
    return false;
}


function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req  
        case ProjectSiteSettings.InvalidExpression:
            $("[id$=TAX_Formula]").focus();
            break;
        //Commend When calling  
        case ProjectSiteSettings.SavedSuccessfully:
            PageInit();
            break;
        case ProjectSiteSettings.DeleteMessageCommand:
            DeleteDetails();
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
        case ProjectSiteSettings.DELETE:
            ProjectID  = GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.PK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(ProjectSiteSettings.DeleteConfirmationMessage, ProjectSiteSettings.ConfirmationMessage, ProjectSiteSettings.DeleteMessageCommand, true);
            break;

        // To Edit Details               
        case ProjectSiteSettings.EDIT:
            FillDetails(tr);
            break;

        default:
            alert(ProjectSiteSettings.DefaultAction);
            break;
    }
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(ProjectSiteSettings.DeleteProjectSite + ProjectID , function (data) {
        if (parseInt(data) == 1) {
            BindGrid();
            msgtxt = ProjectSiteSettings.DeletedSuccessfully;
        }
        else if (parseInt(data) == 0) {
            msgtxt = ProjectSiteSettings.AlreadyAssigned;
        }
        else {
            msgtxt = ProjectSiteSettings.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, ProjectSiteSettings.Title);
    });
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    Removevalidation();
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("select[id$=SearchType]").val("0");
    SetSearchType();
    //initializing search.
    SearchInit();
    ClearPage();
    return false;
}

//#endregion

///#region------ Validations ---------------
function AddValidations() {
    Removevalidation();
    $("input[id$=LOC_CODE]").rules("add", {
        required: true,
        maxlength: 200,
        messages: { required: ProjectSiteSettings.ProjectCodeValidation }
    });

    $("input[id$=LOC_NAME]").rules("add", {
        required: true,
        maxlength: 500,
        messages: { required: ProjectSiteSettings.ProjectNameValidation }
    });
   
}

function Removevalidation() {
   // $(document.forms[0]).validate().resetForm();
    $("input[id$=LOC_NAME]").rules("remove");
    $("input[id$=LOC_CODE]").rules("remove");
}

//#endregion


function AddAfterDateRange(hdfTo, fromDate, hdnFrmDate, format, restrictfromDate) {
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
        altField: $("[id$=" + hdnFrmDate + "]"),
        altFormat: "mm/dd/yy"

    });
    $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdfTo + "]").val()));
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
