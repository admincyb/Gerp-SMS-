/// <reference path="../../../jquery/jquery-1.5.min.js" />
/// <reference path="../../../GrandScriptUtils.js" />

///#region------ Global Variable Declaration ---------------
var UserID;
//#endregion

///#region------ Configuration Settings ---------------
var CreateUsersSettings = {
    //Url
    //GetTaxParameters: "ProjectSite.do?Action=GetTaxParameters",
    GetUsersList: "UserManagement.do?Action=GetUsersList",
    DeleteUser: "UserManagement.do?Action=DeleteUser&UserID=",
    UsersListAutoComplete: "UserManagement.do?Action=GetSearchValue",
    MappingPage: "UserManagement.aspx",
    NewUser: "CreateNewUser.aspx",
    ACTION: "ACTION",
    // Command Section
    SaveCommand: "SAVE",
    EDIT: "EDIT",
    DELETE: "DELETE",
    MAPPING: "MAPPING",
    ActivceCommand: "Active",

    InvalidExpression: "INVALIDEXPRESSION",
    DeleteMessageCommand: "DELETE",
    //MESSAGE
    Title: "Information",
    //    ProjectSiteSaveMessage: "Translate(ProjectSiteSavedSuccesfully)",
    //    ProjectCodeValidation: "Translate(EnterProjectSiteCode)",
    //    ProjectNameValidation: "Translate(EnterProjectSiteName)",
    //    ProjectSiteIDExistsMessage: "Translate(ProjectSiteIDExists)",
    //    InvalidExpression: "Translate(InvalidExpression)",
    //    SavedSuccessfully: "Translate(TaxSaved)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    DeleteConfirmationMessage: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    // UpdatedSuccessfully: "Translate(TaxUpdate)",
    DeletedSuccessfully: "Translate(UserDtlDeletedSuccessfully)",
    AlreadyAssigned: "Translate(UserAlreadyAssigned)",

    //Fields
    PK: "usrPK",
    //    SITEID: "LOC_CODE",
    //    SITENAME: "LOC_NAME",
    //    DESCRIPTION: "LOC_DESC",
    //    ACTIVE: "LOC_ACTIVE",
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
    ShowHideDelete();
});

function PageInit() {
    ///<summary>Function Initialize page details </summary>
    return false;
}

///#region---- Auto Complete Section ----
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    // Removevalidation();
    // $("[id$=imbSave]").hide();
    // $("[id$=imbAdd]").show();
    // $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("select[id$=SearchType]").val("0");
    //initializing search.
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

function ValidateNow(validationGroup) {
    //<summary>method Check validations in a page and </summary>
    var isValid = true;
    if (typeof (Page_ClientValidate) == 'function') {
        CheckValidationDuplicate(validationGroup);
        Page_ClientValidate(validationGroup);
    }

    if (!Page_IsValid) {
        $("[id$=litErrorMsg]").hide();

        if (typeof (ShowErrorMessage) == 'function') {
            ShowErrorMessage($("#diverror").html());
        }

        isValid = false;  //Page is invalid -- stop right here
    }

    if (!ValidateFile()) {
        isValid = false;
    }

    return isValid;
}

//calls parent page alert message 
function AlertMessage(msg, title, command) {
    //<summary>method Show Alert Message</summary>
    Util.ShowModalDialog(msg, title, command);
}

//redirct to command argument
function ModalOk(command) {
    //<summary>method do action on Modal Ok Button Event</summary>
    if (command != null) {
        location.href = command;
    }
}
var Util = {
    ShowModalDialog: function (content, title, command, showCancel) {
        //<summary>method to show alert message</summary>
        if (showCancel) {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                        if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                            ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                        }
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        else {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                beforeClose: function () {
                    if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                        ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                    }
                },
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    }
}

var validationArrayGroup;
function CheckValidationDuplicate(valGroup) {
    validationArrayGroup = new Array();
    //Traversing from bottom through all the validation controls in the page

    for (var i = Page_Validators.length - 1; i >= 0; i--) {
        if (typeof (Page_Validators[i].validationGroup) == "string") {
            if (valGroup == Page_Validators[i].validationGroup) {
                //checks if the control is already in the validation array
                if (!CheckValidationExists(Page_Validators[i].id)) {
                    //insert new conrol to the Array of present validations
                    validationArrayGroup.push(Page_Validators[i].id);
                }
                //remove if control is already in Array of present validations
                else {
                    Page_Validators.splice(i, 1);
                }
            }
            //remove control if not in group
            else {
                Page_Validators.splice(i, 1);
            }
        }
    }
}

//For checking if validation control in Array of present validations
function CheckValidationExists(id) {
    for (var i in validationArrayGroup) {
        if (validationArrayGroup[i] == id) {
            return true;
        }
    }
    return false;
}

var validFilesTypes = ["bmp", "gif", "png", "jpg", "jpeg"];
function ValidateFile() {
    var isValidFile = false;
    var path = $("[id$='fudSignature']").val();
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

    if (path != "") {
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }
    }
    else
        isValidFile = true;

    if (!isValidFile) {
        $("[id$=litErrorMsg]").show();
        $("[id$=litErrorMsg]").html("<ul><li>Invalid File. Please upload a valid image file</li></ul>");
        ShowErrorMessage($("#diverror").html());
    }

    return isValidFile;
}

function ShowHideDelete() {
    if ($("[id$='lblSignatureName']").text() == "")
        $("[id$='btnDeleteSignature']").hide();
    else
        $("[id$='btnDeleteSignature']").show();
}