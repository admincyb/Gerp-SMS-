

/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region ------- Global Variable -----
var desigID = 0;
///#endregion

//#region ------- Configuration Section -------
var DesignationMaster = {
    // URL
    GETDEPARTMENTTREEURL: "CommonManagement.do?Action=GetDepartmentDetails&SBUPk=",
    DESIGNATIONSAVEURL: "Designation.do?Action=SavePage",
    DESIGNATIONBINDGRIDURL: "Designation.do?Action=GetDesignationList&Status=",
    DESIGNATIONDELETEURL: "Designation.do?Action=DeleteDesignation&DesigID=",
    DESIGNATIONAUTOCOMPLETEURL: "Designation.do?Action=GetSearchValue&SBU=",
    DEPTDTLSURL: "CommonManagement.do?Action=GetDepartmentName&BizUnit=",
    // Constant
    SAVECMD: "Save",
    DELETECOMMAND: "DELETE",
    DELETE: "Delete",
    EDITCOMMAND: "EDIT",
    SELECTONE: "selectNone",
    TEXTZERO: "0",
    TEXTEMPTY: "",
    ROOT: "Root",
    DESIGPK: "DSG_PK",
    DESIGDEPT: "DSG_DEPT",
    DESIGNAME: "DSG_NAME",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMMSG: "Translate(Conformation)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    SELECTONEMSG: "Translate(Pleaseselectanoption)",
    SAVESUCCESS: "Translate(DesignationSavedSuccessfully)",
    DESIGNATIONALREADYEXISTS: "Translate(DesignationAlreadyExist)",
    ACTIONFAILED: "Translate(ActionFailedPleaseTryAgain)",
    PLSSELECTDEPT: "Translate(SelectDepartment)",
    ENTERDESIGMSG: "Translate(EnterDesignation)",
    CONFIRMATIONTITLE: "Translate(Confirmation)",
    SELECTDEPTMSG: "Translate(SelectDepartment)",
    CHOOSEDEPTTITLE: "Translate(ChooseDepartment)",
    DELETEDSUCCESS: "Translate(Designationdeletedsuccessfully)",
    DESIGNATIONASSIGNED: "Translate(DesignationAlreadyAssigned)"
}
//#endregion

///#region ------- Initialization Section ----------------

///<summary>For Adding rule to Select</summary>
$.validator.addMethod(DesignationMaster.SELECTONE, function (value, element) {
    return ($(element).val() != DesignationMaster.TEXTZERO);
}, DesignationMaster.SELECTONEMSG);

$(document).ready(function () {
    ///<summary>Document . Ready()</summary>
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //Page Initial condtions
    PageInit();
    //Modal popup and tree view default settings
    $("#divDeptPopUp").dialog({ autoOpen: false });

});

function PageInit() {
    ///<summary>Initial page condition</summary>
    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("select[id$=SearchType]").val(DesignationMaster.TEXTZERO);
    $("[id$=SearchValue]").val(DesignationMaster.TEXTEMPTY);
    // Fill Department Details To DropDown
    FillDeptDtls(0);
    //initializing search.
    SearchInit();
    SetSearchType()
    // Focus To Control
    $("[id$=SearchType]").focus();
    return false;
}

///#endregion

///#region ------- Core Section -------

function FillDeptDtls(deptID) {
    ///<summary>Function used Fill Departmetn Details</summary>
    var drpID = $("select[id$=DSG_DEPT]").attr("id");
    $.get(DesignationMaster.DEPTDTLSURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, deptID);
    });
}

function ShowDeaprtment() {
    ///<summary>Function used call the tree Data </summary>
    RemoveValidation();
    //Fill Department TreeView
    FillDepartmentTree();
    // Show Department PopUp
    GrandScriptUtils.ShowModalID("divDeptPopUp", DesignationMaster.CHOOSEDEPTTITLE, false, 400, 400, false);
    return false;
}

///#region---- Set Or Reset Form----

function AddNew() {
    ///<summary>Function To Show Data Entry Form </summary>
    // Remove Validation From Control
    RemoveValidation();
    $("[id$=btnSave]").show();
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    // Clear Controls value
    ClearDeatils();
    $("[id$=DSG_NAME]").focus();
    return false;
}

//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //Reseting all input controls in the page
//    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () {
//        var idval = $(this).attr("id");
//        //Avoid DSG_PK to get the value
//        if (idval.search("DSG_PK") != -1)
//            $(this).val(DesignationMaster.TEXTZERO);
//        //Avoid UserPk to get the value of log in user
//        else if (idval.search("UserPk") == -1)
//            $(this).val(DesignationMaster.TEXTEMPTY);
//    });
//    //Selecting the first value in all drop downs
//    $(document.forms[0]).find("select").each(function () {
//        idval = $(this).attr("id");
//        if (idval.search("SBU") == -1)
//            $(this).val($(this).find("option:eq(0)").val());
//    });
    //Reset validation
    //RemoveValidation();
    $(document.forms[0]).validate().resetForm();
    // Focus To SearchType When Reset
    $("[id$=SearchType]").focus();
    PageInit();
    return false;
}

///#endregion

///#region---- Auto Complete Section ----

function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(DesignationMaster.TEXTEMPTY);
    if (strname == DesignationMaster.TEXTZERO) {
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
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", DesignationMaster.DESIGNATIONAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

///#endregion

function FillDepartmentTree() {
    //<summary>function To Fill Department in tree view  </summary>
    SetTreeHeaderStructure("trvCategory", DesignationMaster.GETDEPARTMENTTREEURL + $("[id$=BizUnitPk]").val() + "&DeptParentID=", DesignationMaster.ROOT, false, false, DesignationMaster.TEXTZERO, false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view

}

function AddSelectedTree(liAdd) {
    ///<summary>Function Select a Department and Set To Department DropDown</summary>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    // Set Selcted Departmend From TreeView as in Dept DropDown
    $("select[id$=DSG_DEPT]").val(cagID);
    //closing modalbox after selected from treeview.
    $("#divDeptPopUp").dialog("destroy");
    $("#divDeptPopUp").dialog({ autoOpen: false });
    $("select[id$=DSG_DEPT]").focus();
}

function SavePage() {
    ///<summary>Function used to saving Designation Details  </summary>
    // Add validation To Controls
    AddValidations();
    // Check Form is valid or not
    if ($(document.forms[0]).valid()) {
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(DesignationMaster.DESIGNATIONSAVEURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            // Already Exists
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(DesignationMaster.DESIGNATIONALREADYEXISTS, DesignationMaster.INFORMATIONTITLE);
            }
            // Save Success
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(DesignationMaster.SAVESUCCESS, DesignationMaster.INFORMATIONTITLE,DesignationMaster.SAVECMD);

            }
            // Error
            else {
                GrandScriptUtils.ShowModal(DesignationMaster.ACTIONFAILED);
                ResetPage();
            }

        });
    }

    return false;
}

function FillDetails(tr) {
    ///<summary>Function To Fill Designation Details  </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>   
    // Set Page as UserEntry Mode
    AddNew();
    // Assign Value to the controls
    $("input[id$=DSG_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, DesignationMaster.DESIGNAME, $(tr).parent().attr("id")));

    $("input[id$=DSG_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, DesignationMaster.DESIGPK, $(tr).parent().attr("id")));

    FillDeptDtls(GrandGrid.Utilities.GetColumnValue(tr, DesignationMaster.DESIGDEPT, $(tr).parent().attr("id")));

}

function ClearDeatils() {
    ///<summary>Clear Designaion Details </summary>
    $("input[id$=DSG_NAME]").val(DesignationMaster.TEXTEMPTY);

    $("input[id$=DSG_PK]").val(DesignationMaster.TEXTZERO);

}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(DesignationMaster.DESIGNATIONDELETEURL + desigID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = DesignationMaster.DELETEDSUCCESS;
        else if (parseInt(data) == 0)
            msgtxt = DesignationMaster.DESIGNATIONASSIGNED;
        else
            msgtxt = DesignationMaster.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, DesignationMaster.INFORMATIONTITLE, DesignationMaster.SAVECMD);

    });
    return false;
}

function BindGrid(srchVal) {
    ///<summary>Bind Designaion Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    var ajaxUrl = DesignationMaster.DESIGNATIONBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnit=" + $("[id$=BizUnitPk]").val();
    $("#grdDesignation").removeAttr("ajaxurl")
    $("#grdDesignation").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDesignation");
    GrandGrid.MakeGrid($("#grdDesignation"));
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

///#region----Grid Handlers And Model Popup Ok Click----

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
        case DesignationMaster.DELETECOMMAND:
            desigID = GrandGrid.Utilities.GetColumnValue(tr, DesignationMaster.DESIGPK, $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(DesignationMaster.DELETECONFIRMMSG, DesignationMaster.CONFIRMATIONTITLE, DesignationMaster.DELETE, true);
            break;
        // To Edit Details              
        case DesignationMaster.EDITCOMMAND:
            FillDetails(tr);
            break;
        // Default Handler
        default:
            //alert(DesignationMaster.DEFAULTACTION);
            GrandScriptUtils.ShowModal(DesignationMaster.DEFAULTACTION, DesignationMaster.INFORMATIONTITLE);
            break;
    }
    return false;

}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///    
    /// </param>
    switch (command) {
        //comment req     
        case DesignationMaster.SAVECMD:
            PageInit();
            break;
        //Commend When calling   
        case DesignationMaster.DELETE:
            DeleteDetails();
            break;


    }
    return false;
}

///#endregion


///#endregion

///#region ------- Validations ----------------
function AddValidations() {

    ///<summary>function To Add Validations to Controls  </summary>
    $("input[id$=DSG_NAME]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: DesignationMaster.ENTERDESIGMSG }
    });
    $("select[id$=DSG_DEPT]").rules("add", {
        selectNone: true,
        messages: { selectNone: DesignationMaster.SELECTDEPTMSG }
    });
}

function RemoveValidation() {
    ///<summary>function To REMOVE  Validations </summary>
    $("select[id$=DSG_DEPT]").rules("remove");

    $("input[id$=DSG_NAME]").rules("remove");
}
///#endregion