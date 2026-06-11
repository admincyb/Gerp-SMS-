

/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />

///#region ------- Global Variable -------
var subDeptID = 0;
///#endregion

//#region ------- Configuration Section -------
var SubDepartmentMaster = {
    // URL
    //GETSBULIST: "SBUConfiguration.do?Action=GetAllSBUList",
    GETCOUNTRY: "CommonManagement.do?Action=GetCountryList",
    GETSTATELIST: "CommonManagement.do?Action=GetStateList&CountryID=",
    GETCURRENCYLIST: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    GETDEPARTMENT: "CommonManagement.do?Action=GetParentDepartments&BizUnit=",
    GETDEPARTMENTCATEGORIES: "CommonManagement.do?Action=GetParentDepartmentCategoriesByID&BizUnit=",
    GetDepartmentTreeURL: "CommonManagement.do?Action=GetDepartmentDetails&SBUPk=",
    GETPROJECTAUTO: "CommonManagement.do?Action=GetProjectListAuto",
    GETCOMPANY: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",

    SUBDEPARTMENTBINDGRIDURL: "SubDepartment.do?Action=GetSubDeptDtlsList&Status=",

    SUBDEPTAUTOCOMPLETEURL: "SubDepartment.do?Action=GetSearchValue&SBU=",
    SUBDEPTDTLSSAVEURL: "SubDepartment.do?Action=SavePage",
    SUBDEPDELETEURL: "SubDepartment.do?Action=DeleteSubDeptDtls&SubDeptID=",
    SUBDEPTDTLSGRIDURL: "SubDepartment.do?Action=GetSubDeptDtlsList&Status=",
    GETSUBDEPTDTLSBYPKURL: "SubDepartment.do?Action=GetSubDeptDtlsByID&SubDeptID=",
    INBOX: "../../AccountManagement/WorkflowInbox.aspx",
    SUBDEPARTMENTMASTERLISTURL: "SubDepartmentMaster.aspx",

    // Constants
    SAVECMD: "Save",
    DELETECMD: "DELETE",
    DELETE: "Delete",
    EDITCMD: "EDIT",
    VIEWCMD: "VIEW",
    SELECTONE: "selectNone",
    SELECTMINUSONE: "selectMinusOne",
    TEXTZERO: "0",
    TEXTMINUSONE: "-1",
    TEXTEMPTY: "",
    ROOT: "Root",
    STATE: "STT_NAME",
    COUNTRY: "CNT_NAME",
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMATIONMSGTITLE: "Translate(Conformation)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    SELECTONEMSG: "Translate(Pleaseselectanoption)",
    SELECTSBUMSG: "Transleate(ProvideSBU)",
    SELECTDEPTMSG: "Translate(ProvideBaseDepartment)",
    SELECTCATEGORYMSG: "Translate(ProvideType)",
    PROVIDETITLEMSG: "Translate(ProvideBaseDepartment)",
    PROVIDECODE: "Translate(ProvideCode)",
    PROVIDEADDRESS1MSG: "Translate(ProvideAddressLine1)",
    PROVIDEADDRESS2MSG: "Translate(ProvideAddress2)",
    PROVIDEEMAILMSG: "Translate(ProvideEmail)",
    PROVIDEPHONEMSG: "Translate(ProvidePhone)",
    SELECTCOUNTRYMSG: "Translate(ProvideCountry)",
    SELECTCOMPANYMSG: "Translate(ProvidePlant)",
    SELECTSTATEMSG: "Translate(ProvideState)",
    PROVIDECITYMSG: "Translate(ProvideCity)",
    PROVIDECURRENCYMSG: "Translate(ProvideCurrency)",
    DELETEDSUCCESS: "Translate(SubDeptdeletedsuccessfully)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    SAVESUCCESS: "Translate(SubDepartmentSavedSuccessfully)",
    CONFIRMATIONTITLE: "Translate(Confirmation)",
    ACTIONFAILED: "Translate(ActionFailedPleaseTryAgain)",

    // Messages
    SUBDEPTCODEEXISTSMSG: "Translate(SubDepartmentCodeAlreadyExists)",
    SUBDEPTNAMEEXISTSMSG: "Translate(SubDeptNameAlreadyExists)",
    DEPTNAMEEXISTSMSG: "Translate(SubDepartmentNameAlreadyExists)",
    SUBDEPTASSIGNED: "Translate(CannotDeleteHaveReference)",
    DEPARTMENTDETAILS: "Translate(ChooseDepartment)",

    //Fields
    DPT_ACTIVE: "DPT_ACTIVE"

}
//#endregion

///#region-------- Initialization Section ----------------

//For Adding rule to Select
$.validator.addMethod(SubDepartmentMaster.SELECTONE, function (value, element) {
    return ($(element).val() != SubDepartmentMaster.TEXTZERO);
}, SubDepartmentMaster.SELECTONEMSG);
$.validator.addMethod(SubDepartmentMaster.SELECTMINUSONE, function (value, element) {
    return ($(element).val() != SubDepartmentMaster.TEXTMINUSONE);
}, SubDepartmentMaster.SELECTONEMSG);
$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //Page Initial condtions
    PageInit();
    $("#divDeptPopUp").dialog({ autoOpen: false });
    // Fill Departments
    FillDepartment(0);
    FillCountry(0);
    FillCompany(0);
    FillCurrency(0);
    FillState(0, 0);
    FillDepartmentCategories("", 0);

    //CountryAuto();
    //StateAuto();
});
//initial page condition
function PageInit() {
    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("select[id$=DPT_PARENT]").attr('disabled', true);
    BindGrid();
    //initializing search.
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    if ($("[id$=hdfShowProject]").val() == 1) {
        $("[id$=divProjectAuto]").show();
    }
    else {
        $("[id$=divProjectAuto]").hide();
    }
    FillProjectAutoComplete();
    return false;
}

///#endregion

///#region ------- Core Section -------


function FillDepartment(deptID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=DPT_PARENT]").attr("id");
    $.get(SubDepartmentMaster.GETDEPARTMENT + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, deptID);
    });
}
function FillDepartmentCategories(deptID, deptCategoryID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=DPT_CATEGORY]").attr("id");
    $.get(SubDepartmentMaster.GETDEPARTMENTCATEGORIES + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + deptID, function (data) {
        GrandScriptUtils.FillDropDownWithSelect(drpID, data, true, true, deptCategoryID);
    });
}

function FillCountry(countryID) {
    //<summary>Function Used to fill all Country</summary>
    var drpID = $("[id$=DPT_CNTRY]").attr("id");
    $.get(SubDepartmentMaster.GETCOUNTRY, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, countryID);
    });
}

function FillCompany(companyID) {
    //<summary>Function Used to fill all Country</summary>
    var drpID = $("[id$=DPT_COMPANY]").attr("id");
    $.get(SubDepartmentMaster.GETCOMPANY + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, companyID);
    });
}

function FillState(stateID, countryID) {
    // alert(stateID, countryID);
    //<summary>Function Used to fill all State Under Selected Country</summary>
    var drpID = $("[id$=DPT_STATE]").attr("id");
    $.get(SubDepartmentMaster.GETSTATELIST + countryID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, stateID);
    });

}

function FillStateList() {
    //<summary>Function Used to fill all State Under Selected Country</summary>
    FillState(0, $("select[id$=DPT_CNTRY]").val());

}
function FillDepartmentCategoryList() {
    //<summary>Function Used to fill all State Under Selected departement</summary>

    //FillDepartmentCategories($("select[id$=DPT_PARENT] option:selected").text(), 0);
    FillDepartmentCategories($("select[id$=DPT_PARENT] option:selected").val(), 0);
}

function FillCurrency(curID) {
    //<summary>Function Used to fill all Currency</summary>
    var drpID = $("[id$=DPT_CURR]").attr("id");
    $.get(SubDepartmentMaster.GETCURRENCYLIST + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, curID);
    });
}

function FillProjectAutoComplete() {
    GrandScriptUtils.MakeAutoComplete("txtProject", SubDepartmentMaster.GETPROJECTAUTO, "DPT_PROJECT", true, false, false, true);
}

/// Used to disable Autocomplete
function DisableAuto(extender, hfield) {
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
    $(extender).autocomplete("option", "disabled", true);
    $(extender).attr("disabled", true);
}

function SavePage() {
    ///<summary>Function used to saving Designation Details  </summary>
    AddValidations();
    if ($(document.forms[0]).valid()) {
        $("[id$=DPT_PARENT]").attr("disabled", false);
        $("input[id$=DPT_CODE]").attr("disabled", false);
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(SubDepartmentMaster.SUBDEPTDTLSSAVEURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(SubDepartmentMaster.SUBDEPTCODEEXISTSMSG, SubDepartmentMaster.INFORMATIONTITLE);

            }

            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(SubDepartmentMaster.SAVESUCCESS, SubDepartmentMaster.INFORMATIONTITLE, SubDepartmentMaster.SAVECMD);
                ClearForm();

            }
            else if (parseInt(data) == -2) {
                GrandScriptUtils.ShowModal(SubDepartmentMaster.SUBDEPTNAMEEXISTSMSG, SubDepartmentMaster.INFORMATIONTITLE);

            }
            else {
                GrandScriptUtils.ShowModal(SubDepartmentMaster.ACTIONFAILED);

            }

        });
    }

    return false;
}

function FillDetails(subDeptID) {
    ///<summary>Function To Fill Department Details  </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    $.get(SubDepartmentMaster.GETSUBDEPTDTLSBYPKURL + subDeptID, function (data) {

        //        $("[id$=SBU]").val(data.Table[0].SBU);
        //        $("select[id$=SBU]").val(data.Table[0].SBU);
        //        $("[id$=BizUnitPk]").val(data.Table[0].SBU);
        $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
        FillDepartment(data.Table[0].DPT_PARENT);
        //$("select[id$=DPT_PARENT]").val();
        $("input[id$=DPT_NAME]").val(data.Table[0].DPT_NAME);
        $("input[id$=DPT_CODE]").val(data.Table[0].DPT_CODE);
        //$("input[id$=DPT_ADDR1]").val(data.Table[0].DPT_ADDR1);
        $("[id$=DPT_ADDR1]").val(data.Table[0].DPT_ADDR1);
        //$("input[id$=DPT_ADDR2]").val(data.Table[0].DPT_ADDR2);
        $("[id$=DPT_ADDR2]").val(data.Table[0].DPT_ADDR2);
        $("input[id$=DPT_EMAIL]").val(data.Table[0].DPT_EMAIL);
        $("input[id$=DPT_PHONE]").val(data.Table[0].DPT_PHONE);
        //Fill And Select Country
        FillCountry(data.Table[0].DPT_CNTRY);
        //Fill And Select Country
        FillCompany(data.Table[0].DPT_COMPANY);
        //Fill And Select State
        FillState(data.Table[0].DPT_STATE, data.Table[0].DPT_CNTRY);
        $("input[id$=DPT_CITY]").val(data.Table[0].DPT_CITY);

        $("input[id$=DPT_ZIP]").val(data.Table[0].DPT_ZIP);
        $("input[id$=DPT_MOBILE]").val(data.Table[0].DPT_MOBILE);
        $("input[id$=DPT_GST_NO]").val(data.Table[0].DPT_GST_NO);

        //Fill And Select Currency
        FillCurrency(data.Table[0].DPT_CURR);
        $("input[id$=DPT_PK]").val(data.Table[0].DPT_PK);
        // Check Selected Dept have subDept or not, if default, disabled the control
        if (data.Table[0].HAS_CHILD == "false") {

            $("[id$=imbViewDept]").css({ "display": "inline-block", "visibility": "visible" });
            //$("[id$=DPT_PARENT]").attr("disabled", false);
        }
        else {
            $("[id$=imbViewDept]").css({ "display": "none", "visibility": "hidden" });
            $("[id$=DPT_PARENT]").attr("disabled", true);
        }
        // Check Selected Dept is Default or not, if default, disabled the control
        if (data.Table[0].DPT_DEFAULT == false) {
            $("input[id$=DPT_CODE]").attr("disabled", false);
        }
        else {
            $("input[id$=DPT_CODE]").attr("disabled", true);
        }

        if (data.Table[0].DPT_ACTIVE == true) {
            $("input[id$=DPT_ACTIVE]").attr("checked", true);
        }
        else {
            $("input[id$=DPT_ACTIVE]").attr("checked", false);
        }
        if (data.Table[0].DPT_IS_STOCK == "1") {
            $("input[id$=DPT_IS_STOCK ]").attr("checked", true);
        }
        else {
            $("input[id$=DPT_IS_STOCK ]").attr("checked", false);
        }

        //Fill Subdeaprtement
        // FillDepartmentCategories(data.Table[0].DPT_PARENT_TEXT, data.Table[0].DPT_CATEGORY);
        FillDepartmentCategories(data.Table[0].DPT_PARENT, data.Table[0].DPT_CATEGORY);

        $("input[id$=txtProject]").val(data.Table[0].DPT_PROJECT_TEXT);
        $("[id$=DPT_PROJECT]").val(data.Table[0].DPT_PROJECT);

        // alert(data.Table[0].DPT_PROJECT);
        if (parseInt(data.Table[0].DPT_PROJECT) > 0) {
            DisableAuto($("[id$=txtProject]"), $("[id$=DPT_PROJECT]"));
        }
        else {
            FillProjectAutoComplete();
        }
    });

}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(SubDepartmentMaster.SUBDEPDELETEURL + subDeptID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = SubDepartmentMaster.DELETEDSUCCESS;
        else if (parseInt(data) == 0)
            msgtxt = SubDepartmentMaster.SUBDEPTASSIGNED;
        else
            msgtxt = SubDepartmentMaster.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, SubDepartmentMaster.INFORMATIONTITLE, SubDepartmentMaster.SAVECMD);

    });
    return false;
}

function BindGrid(srchVal) {
    ///<summary>Bind Designaion Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    var isTick = 0;
    if ($("[id$=chkActiveFilter]").attr('checked') == true) {
        isTick = 1;
    }
    else {
        isTick = 0;
    }
    var ajaxUrl = SubDepartmentMaster.SUBDEPARTMENTBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val() + "&ActiveStatus=" + isTick;
    $("#grdSubDepartment").removeAttr("ajaxurl")
    $("#grdSubDepartment").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdSubDepartment");
    GrandGrid.MakeGrid($("#grdSubDepartment"));
    return false;

}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

function RedirectToInbox() {
    window.location = SubDepartmentMaster.INBOX;
    return false;
}

function ShowDeaprtment() {
    ///<summary>Function used call the tree Data </summary>
    // RemoveValidation();
    //Fill Department TreeView
    FillDepartmentTree();
    // Show Department PopUp
    GrandScriptUtils.ShowModalID("divDeptPopUp", SubDepartmentMaster.DEPARTMENTDETAILS, false, 400, 400, false);
    return false;
}

function FillDepartmentTree() {
    //<summary>function To Fill Department in tree view  </summary>
    SetTreeHeaderStructure("trvCategory", SubDepartmentMaster.GetDepartmentTreeURL + $("[id$=BizUnitPk]").val() + "&deptParentID=", SubDepartmentMaster.ROOT, false, false, "0", false);
    MakeMultiTree(); // call the function to bind tree view

}

function AddSelectedTree(liAdd) {
    ///<summary>Function Select a Department and Set To Department DropDown</summary>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category
    // Set Selcted Departmend From TreeView as in Dept DropDown
    $("select[id$=DPT_PARENT]").val(cagID);
    //closing modalbox after selected from treeview.
    $("#divDeptPopUp").dialog("destroy");
    $("#divDeptPopUp").dialog({ autoOpen: false });
    $("select[id$=DPT_PARENT]").focus();
    FillDepartmentCategories(cagID, -1);
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
        case SubDepartmentMaster.DELETECMD:
            subDeptID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(SubDepartmentMaster.DELETECONFIRMMSG, SubDepartmentMaster.CONFIRMATIONTITLE, SubDepartmentMaster.DELETE, true);
            break;
        // To Edit Details                
        case SubDepartmentMaster.EDITCMD:
            subDeptID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            FillDetails(subDeptID);
            AddNew(1);
            break;
        case SubDepartmentMaster.VIEWCMD:
            subDeptID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            FillDetails(subDeptID);
            AddNew(0);
            break;
        // Default Handler      
        default:
            GrandScriptUtils.ShowModal(SubDepartmentMaster.DEFAULTACTION, DesignationMaster.InFormationTitle);
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
        case SubDepartmentMaster.SAVECMD:
            PageInit();
            ResetPage();
            break;
        //Commend When calling     
        case SubDepartmentMaster.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type </summary>
    var colIndex = 0;
    var colData = "";
    $("#grdSubDepartment tr:has(td)").each(function () {
        var DefaultStatus = GrandGrid.Utilities.GetColumnValue($(this), "DPT_DEFAULT", $(this).parents("table:first").attr("id"));
        // Hide Edit And Delete Button, if status=true
        if (DefaultStatus == "true") {
            $(this).find("td:last input[id$=imbEditMast]").hide();
            $(this).find("td:last input[id$=imbDeleteMast]").hide();
        }
        // Show Edit And Delete Button, if status=false
        else {
            $(this).find("td:last input[id$=imbEditMast]").show();
            $(this).find("td:last input[id$=imbDeleteMast]").show();
        }

        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), SubDepartmentMaster.STATE, $(this).parents("table:first").attr("id"));
        if (colIndex != null) {
            colData = GrandGrid.Utilities.GetColumnValue($(this), SubDepartmentMaster.STATE, $(this).parents("table:first").attr("id"));
            if (colData == "null")
                colData = "-";
            $(this).find("td:eq(" + colIndex + ")").html(colData);
        }

        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), SubDepartmentMaster.COUNTRY, $(this).parents("table:first").attr("id"));
        if (colIndex != null) {
            colData = GrandGrid.Utilities.GetColumnValue($(this), SubDepartmentMaster.COUNTRY, $(this).parents("table:first").attr("id"));
            if (colData == "null")
                colData = "-";
            $(this).find("td:eq(" + colIndex + ")").html(colData);
        }

    });

}

///#endregion

///#region---- Set Or Reset Form----

function AddNew(status) {
    ///<summary>Function To Show Data Entry Form </summary>
    RemoveValidation();
    if (status == 1) {
        $("[id$=btnSave]").show();
        $("input[id$=DPT_ACTIVE]").attr("checked", true);
    }
    else {
        $("[id$=btnSave]").hide();
    }
    $("[id$=btnAdd]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();


    $("[id$=imbViewDept]").css({ "display": "inline", "visibility": "visible" });

    //$("[id$=DPT_PARENT]").attr("disabled", false);
    if (status == 0) {
        FillDepartment(0);
    }

    $("[id$=DPT_CODE]").focus();
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    //    var idval = "";
    //    $(document.forms[0]).find("input:not([id=__VIEWSTATE])").each(function () {
    //        idval = $(this).attr("id");
    //        if (idval.search("DPT_PK") != -1)
    //            $(this).val(SubDepartmentMaster.TEXTZERO);

    //        //Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1)
    //            $(this).val(SubDepartmentMaster.TEXTEMPTY);
    //    });
    //   // Selecting the first value in all drop downs
    //    $(document.forms[0]).find("select").each(function () {
    //        idval = $(this).attr("id");
    //        if (idval.search("SBU") == -1)
    //            $(this).val($(this).find("option:eq(0)").val());
    //    });
    //    RemoveValidation();
    ClearForm();
    //    var t = $("[id$=BizUnitPk]").val();
    //    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $(document.forms[0]).validate().resetForm();
    $("input[id$=DPT_CODE]").attr("disabled", false);
    //$("[id$=DPT_PARENT]").attr("disabled", false);
    FillState(0, 0);
    FillDepartmentCategories(0, 0);
    PageInit();
    $("select[id$=SearchType]").focus();
    //$("[id$=btnAdd]").val("Add");
    $("[id$=btnSave]").val("Save");
    $("[id$=btnReset]").val("Reset");
    $("[id$=btnCancel]").val("Cancel");
    $("input[id$=DPT_ACTIVE]").attr("checked", true);
    return false;
}

function CancelPage() {
    //<summary>Function Used to Cancel Page</summary>
    window.location = SubDepartmentMaster.SUBDEPARTMENTMASTERLISTURL;
    return false;
}

///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(SubDepartmentMaster.TEXTEMPTY);
    if (strname == SubDepartmentMaster.TEXTZERO) {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();

    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
        BindGrid();
    }
}

function SearchInit() {
    ///<summary>To handle auto complete</summary>
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", SubDepartmentMaster.SUBDEPTAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

//function CountryAuto() {
//    GrandScriptUtils.MakeAutoComplete("COUNTRY", "CommonManagement.do?Action=GetCountryDetailsAuto", "COUNTRYPK");
//}
//function StateAuto() {
//    GrandScriptUtils.MakeAutoComplete("STATE", "CommonManagement.do?Action=GetStateDetailsAuto&country=" + $("input[id$=COUNTRYPK]").val(), "STATEPK");
//}


///#endregion

///#endregion

///#region-------- Validations ----------------

function AddValidations() {

    ///<summary>function To Validations  </summary>
    $("select[id$=DPT_PARENT]").rules("add", {
        selectNone: true,
        messages: { selectNone: SubDepartmentMaster.SELECTDEPTMSG }
    });
    $("input[id$=DPT_NAME]").rules("add", {
        required: true,
        maxlength: 95,
        messages: { required: SubDepartmentMaster.PROVIDETITLEMSG }
    });
    $("input[id$=DPT_CODE]").rules("add", {
        required: true,
        maxlength: 95,
        messages: { required: SubDepartmentMaster.PROVIDECODE }
    });

    $("textarea[id$=DPT_ADDR1]").rules("add", {
        required: true,
        maxlength: 195,
        messages: { required: SubDepartmentMaster.PROVIDEADDRESS1MSG }
    });
    $("textarea[id$=DPT_ADDR2]").rules("add", {
        // required: true,
        maxlength: 195,
        messages: { required: SubDepartmentMaster.PROVIDEADDRESS2MSG }
    });
    $("input[id$=DPT_EMAIL]").rules("add", {
        required: true,
        maxlength: 95,
        email: true,
        messages: { required: SubDepartmentMaster.PROVIDEEMAILMSG }
    });
    $("input[id$=DPT_PHONE]").rules("add", {
        required: true,
        maxlength: 90,
        messages: { required: SubDepartmentMaster.PROVIDEPHONEMSG }
    });
    $("select[id$=DPT_CATEGORY]").rules("add", {
        selectMinusOne: true,
        messages: { selectMinusOne: SubDepartmentMaster.SELECTCATEGORYMSG }
    });

    $("select[id$=DPT_CNTRY]").rules("add", {
        selectNone: true,
        messages: { selectNone: SubDepartmentMaster.SELECTCOUNTRYMSG }
    });

    $("select[id$=DPT_COMPANY]").rules("add", {
        selectNone: true,
        messages: { selectNone: SubDepartmentMaster.SELECTCOMPANYMSG }
    });

    $("select[id$=DPT_STATE]").rules("add", {
        selectNone: true,
        messages: { selectNone: SubDepartmentMaster.SELECTSTATEMSG }
    });
    $("input[id$=DPT_CITY]").rules("add", {
        required: true,
        maxlength: 95,
        messages: { required: SubDepartmentMaster.PROVIDECITYMSG }
    });
    $("select[id$=DPT_CURR]").rules("add", {
        selectNone: true,
        messages: { selectNone: SubDepartmentMaster.PROVIDECURRENCYMSG }
    });


}
function ClearForm() {
    $("input[id$=DPT_PK]").val("0");
    $("select[id$=DPT_PARENT]").val(0);
    $("input[id$=DPT_NAME]").val('');
    $("input[id$=DPT_CODE]").val('');
   // $("input[id$=DPT_ADDR1]").val('');
    $("[id$=DPT_ADDR1]").val("");
   // $("input[id$=DPT_ADDR2]").val('');
    $("[id$=DPT_ADDR2]").val("");
    $("input[id$=DPT_EMAIL]").val('');
    $("input[id$=DPT_PHONE]").val('');
    $("select[id$=DPT_CNTRY]").val(0);
    $("select[id$=DPT_COMPANY]").val(0);
    $("select[id$=DPT_STATE]").val(0);
    $("input[id$=DPT_CITY]").val('');
    $("input[id$=DPT_ZIP]").val('');
    $("input[id$=DPT_MOBILE]").val('');
    $("input[id$=DPT_GST_NO]").val('');
    $("select[id$=DPT_CURR]").val(0);
}


function RemoveValidation() {
    ///<summary>function To REMOVE  Validations </summary>

    $("select[id$=DPT_PARENT]").rules("remove");
    $("input[id$=DPT_NAME]").rules("remove");
    $("input[id$=DPT_CODE]").rules("remove");


    $("textarea[id$=DPT_ADDR1]").rules("remove");
    $("textarea[id$=DPT_ADDR2]").rules("remove");
    //$("input[id$=DPT_ADDR2]").rules("remove");
    $("input[id$=DPT_EMAIL]").rules("remove");
    $("input[id$=DPT_PHONE]").rules("remove");


    $("select[id$=DPT_CNTRY]").rules("remove");
    $("select[id$=DPT_COMPANY]").rules("remove");
    $("select[id$=DPT_STATE]").rules("remove");
    $("input[id$=DPT_CITY]").rules("remove");
    $("select[id$=DPT_CURR]").rules("remove");

}
///#endregion