/// <reference path="../../../jquery/jquery-1.5.min.js" />
/// <reference path="../../../GrandScriptUtils.js" />


///#region------ Global Variable Declaration ---------------
var UserID;
var UserCategory;
//#endregion

///#region------ Configuration Settings ---------------
var UsersListSettings = {
    //Url
    //GetTaxParameters: "ProjectSite.do?Action=GetTaxParameters",
    GetUsersList: "UserManagement.do?Action=GetUsersList",
    GetUsersTypeList: "CommonManagement.do?Action=GetAppConfig&CfgValue=USER LEVEL TYPE&SBUID=",
    DeleteUser: "UserManagement.do?Action=DeleteUser&UserID=",
    UsersListAutoComplete: "UserManagement.do?Action=GetSearchValue&AUTOSEARCH=1",
    MappingPage: "UserManagement.aspx",
    //NewUser: "CreateNewUser.aspx",
    NewUser: "../../GeneralAdmin/CreateNewUser.aspx",
    GetUserRoles: "UserManagement.do?Action=GetUserRoles",
    ACTION: "ACTION",
    // Command Section
    SaveCommand: "SAVE",
    EDIT: "EDIT",
    DELETE: "DELETE",
    MAPPING: "MAPPING",
    ActivceCommand: "Active",
    NormalUser: "0",
    Customer: "1",
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
    CantEditCustomerUser: "Translate(CantEditCustomerUser)",
    CustomerUser: "Translate(CustomerPortalUser)",
    MessageBoxTitle: "Translate(Information)",
    NoUserRoleMapping: "Translate(NoUserRoleMapping)",

    //Fields
    PK: "usrPK",
    Category: "empCategory",
    UserStatus: "usrStatus",
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
});

function PageInit() {
    ///<summary>Function Initialize page details </summary>
    //  $("[id$=imbSave]").hide();
    //  $("[id$=imbAdd]").show();
    ///  $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("[id$=LOC_BIZUNIT]").val($("[id$=BizUnitPk]").val());
    $("[id$=UserID]").val($("[id$=UserPk]").val());

    //setting search type.
    SetSearchType();
    //initializing search.
    FillUserType(0);
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

function FillUserType(itemPK) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=ddlUserType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(UsersListSettings.GetUsersTypeList + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, itemPK);
        BindGrid();
        SearchInit();
    });
}



function SearchInit() {
    ///<summary>To handle auto complete</summary>
    BindGrid();
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", UsersListSettings.UsersListAutoComplete + ($("[id$=hdfUserType]").val() == "1" ? ("&UserType=" + $("[id$=ddlUserType]").val()) : ""), "SearchType", false, "SBU");
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}
///#endregion

function BindGrid(srchVal) {
    ///<summary>To handle bind grid corr. to the search type and search value</summary>
    var srchV = "";
    if ($("input[id$=chkIsActive]").is(':checked')) {
        $("[id$=STATUS]").val("1");
    }
    else {
        $("[id$=STATUS]").val("0");
    }
    var BizUnit = 0;
    if ($("[id$=hdfSBUSpecificUser]").val() == "1")
        BizUnit = $("[id$=BizUnitPk]").val();
    var ajaxUrl = UsersListSettings.GetUsersList + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + BizUnit + "&IsActive=" + $("[id$=STATUS]").val() + ($("[id$=hdfUserType]").val() == "1" ? ("&UserType=" + $("[id$=ddlUserType]").val()) : "");
    $("#grdUsersList").removeAttr("ajaxurl");
    $("#grdUsersList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdUsersList");
    GrandGrid.MakeGrid($("#grdUsersList"));
    return false;
}

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    //    $("[id$=imbSave]").show();
    //    $("[id$=imbAdd]").hide();
    //    $("[id$=divData]").show();
    //    $("[id$=divListing]").hide();
    window.location = UsersListSettings.NewUser;
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
    $("textarea[id$=LOC_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.DESCRIPTION, tableId));
    $("input[id$=LOC_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.SITENAME, tableId));
    $("input[id$=LOC_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.PK, tableId));
    $("input[id$=LOC_MOD_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, ProjectSiteSettings.MODDATE, tableId));
}

function ClearPage() {
    $("input[id$=LOC_CODE]").val("");
    $("textarea[id$=LOC_DESC]").val("");
    $("input[id$=LOC_NAME]").val("");
    $("input[id$=LOC_PK]").val("0");
    $("input[id$=LOC_MOD_DT]").val("");
}
function SavePage() {
    AddValidations();
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {
        $.post(ProjectSiteSettings.SaveProjectDetails, jSonString, function (result) {
            if (parseInt(result) > 0) {
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ProjectSiteSaveMessage, ProjectSiteSettings.MessageBoxTitle, ProjectSiteSettings.SaveCommand);
                BindGrid();
                ResetPage();
            }
            else if (parseInt(result) == -2)
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ActionFailedMessage, ProjectSiteSettings.MessageBoxTitle);
            else {
                GrandScriptUtils.ShowModal(ProjectSiteSettings.ActionFailedMessage, ProjectSiteSettings.MessageBoxTitle);
                ResetPage();
            }
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
        case UsersListSettings.InvalidExpression:
            $("[id$=TAX_Formula]").focus();
            break;
        //Commend When calling      
        case UsersListSettings.SavedSuccessfully:
            PageInit();
            break;
        case UsersListSettings.DeleteMessageCommand:
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
        case UsersListSettings.DELETE:
            UserID = GrandGrid.Utilities.GetColumnValue(tr, UsersListSettings.PK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(UsersListSettings.DeleteConfirmationMessage, UsersListSettings.ConfirmationMessage, UsersListSettings.DeleteMessageCommand, true);
            break;
        // To Mapp User Role  
        case UsersListSettings.ACTION:
            UserCategory = GrandGrid.Utilities.GetColumnValue(tr, UsersListSettings.Category, $(tr).parent().attr("id"));
           
            if (UserCategory != UsersListSettings.Customer) {
                UserID = GrandGrid.Utilities.GetColumnValue(tr, UsersListSettings.PK, $(tr).parent().attr("id"));
                window.location = UsersListSettings.MappingPage + "?UserID=" + UserID;
            }
            else {
                GrandScriptUtils.ShowModal(UsersListSettings.CantEditCustomerUser, UsersListSettings.MessageBoxTitle);
            }
            break;

        // To Edit Details   
        case UsersListSettings.EDIT:
            UserCategory = GrandGrid.Utilities.GetColumnValue(tr, UsersListSettings.Category, $(tr).parent().attr("id"));
            if (UserCategory != UsersListSettings.Customer) {
                UserID = GrandGrid.Utilities.GetColumnValue(tr, UsersListSettings.PK, $(tr).parent().attr("id"));
                window.location = UsersListSettings.NewUser + "?UserID=" + UserID;
            }
            else {
                GrandScriptUtils.ShowModal(UsersListSettings.CantEditCustomerUser, UsersListSettings.MessageBoxTitle);
            }
            break;

        default:
            alert(UsersListSettings.DefaultAction);
            break;
    }
    return false;
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(UsersListSettings.DeleteUser + UserID, function (data) {
        if (parseInt(data) == 1) {
            BindGrid();
            msgtxt = UsersListSettings.DeletedSuccessfully;
        }
        else if (parseInt(data) == 0) {
            msgtxt = UsersListSettings.AlreadyAssigned;
        }
        else {
            msgtxt = UsersListSettings.ActionFailedMessage;
        }
        GrandScriptUtils.ShowModal(msgtxt, UsersListSettings.Title);
    });
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    // Removevalidation();
    // $("[id$=imbSave]").hide();
    // $("[id$=imbAdd]").show();
    // $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("select[id$=SearchType]").val("0");
    $("select[id$=ddlUserType]").val("0");
    SetSearchType();
    //initializing search.
    SearchInit();
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
function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdUsersList") {
        $("#grdUsersList tr:has(td)").each(function (index) {
            var UserStatus = GrandGrid.Utilities.GetColumnValue($(this), UsersListSettings.UserStatus, grdID);
            if (UserStatus == 1) {
                $(this).find("td:last input[id$=imbusermapping]").show();
            }
            else if (UserStatus == 0) {
                $(this).find("td:last input[id$=imbusermapping]").hide();
            }

            UserCategory = GrandGrid.Utilities.GetColumnValue($(this), UsersListSettings.Category, grdID);
            if (UserCategory == UsersListSettings.Customer) {
                var userTypeIndex = GrandGrid.Utilities.GetColumnIndex($(this), "usrIsSysUserText", grdID);
                if (userTypeIndex != null) {
                    $(this).find("td:eq(" + userTypeIndex + ")").html(UsersListSettings.CustomerUser);
                }
            }
            var userIsPublicIndex = GrandGrid.Utilities.GetColumnIndex($(this), "usrIsPublic", grdID);
            if (userIsPublicIndex != null) {
                var IsPublic = GrandGrid.Utilities.GetColumnValue($(this), "usrIsPublic", grdID);
                $(this).find("td:eq(" + userIsPublicIndex + ")").html(IsPublic == "1" ? "True" : "False");
            }

            //**************************
            var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "usrName", grdID);
            var userPK = GrandGrid.Utilities.GetColumnValue($(this), "usrPK", grdID);
            var userName = GrandGrid.Utilities.GetColumnValue($(this), "usrName", grdID);
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:ViewUserRoles('" + userPK + "');\" > " + userName + " </a>  ");
            }
            //****************************
        });
    }
}

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

function ViewUserRoles(usrPK) {
    ///<summary>function used to get item's purchase request</summary>

    var ajaxurl = UsersListSettings.GetUserRoles + "&UserPk=" + usrPK;
    $.get(ajaxurl, function (data) {
        if (data.length > 0) {
            $("[id$=lblUserName]").html(data[0].USR_NAME);
            GrandGrid.MakeGrid($("#grdUserRoles"), 0, data);
            $("#divRoleDetails").dialog("open");
            $("#divRoleDetails").dialog(
            {
                width: 830,
                height: 500,
                title: "Translate(UserRolesTitle)"
            });
        }
        else {
            GrandGrid.MakeGrid($("#grdUserRoles"), 0, new Array());
            GrandScriptUtils.ShowModal(UsersListSettings.NoUserRoleMapping, UsersListSettings.MessageBoxTitle, UsersListSettings.MAPPING);
            return false;
        }
    });
}
