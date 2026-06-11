

/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region ------- Global Variable -------
var StoreLocID = 0;
///#endregion

//#region ------- Configuration Section -------
var StoreDepartmentMaster = {
    // URL   
    GETPRODUCTIONDEPARTMENT: "StoreLocationMaster.do?Action=GetProductionStoreDepartments&BizUnit=",
    GETDEPARTMENTCATEGORIES: "CommonManagement.do?Action=GetParentDepartmentCategoriesByID&BizUnit=",
    GetDepartmentTreeURL: "CommonManagement.do?Action=GetDepartmentDetails&SBUPk=",
    STORELOCATIONBINDGRIDURL: "StoreLocationMaster.do?Action=GetStoreLocationList&Status=",

    STORELOCAUTOCOMPLETEURL: "StoreLocationMaster.do?Action=GetSearchValue&SBU=",
    SAVEURL: "StoreLocationMaster.do?Action=SavePage",
    DELETEURL: "StoreLocationMaster.do?Action=DeleteStoreLocDtls&StoreLocID=",  
    GETSTORELOCDTLSBYPKURL: "StoreLocationMaster.do?Action=GetStoreLocDtlsByID&StoreLocID=",
    INBOX: "../../AccountManagement/WorkflowInbox.aspx",
    StoreDepartmentMasterLISTURL: "StoreLocationMaster.aspx",
    PRINTLABELURL: "../../Reports/GenerateReport.aspx",

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
    // Messages
    INFORMATIONTITLE: "Translate(Information)",
    CONFIRMATIONMSGTITLE: "Translate(Conformation)",
    DELETECONFIRMMSG: "Translate(Doyouwanttodeletethisdetails)",
    SELECTONEMSG: "Translate(Pleaseselectanoption)",
    SELECTSBUMSG: "Transleate(ProvideSBU)",
    SELECTSTOREMSG: "Translate(PleaseselectaStore)",
    SELECTCATEGORYMSG: "Translate(ProvideType)",
    PROVIDETITLEMSG: "Translate(EnterLocationName)",
    PROVIDECODE: "Translate(EnterLocationCode)",

    DELETEDSUCCESS: "Translate(StoreLocdeletedsuccessfully)",
    ACTIONFAILEDMSG: "Translate(ActionFailedPleaseTryAgain)",
    SAVESUCCESS: "Translate(StoreLocSavedSuccessfully)",
    CONFIRMATIONTITLE: "Translate(Confirmation)",
    ACTIONFAILED: "Translate(ActionFailedPleaseTryAgain)",

    // Messages
    SUBDEPTCODEEXISTSMSG: "Translate(StoreLocCodeAlreadyExists)",
    SUBDEPTNAMEEXISTSMSG: "Translate(StoreLocNameAlreadyExists)",
    DEPTNAMEEXISTSMSG: "Translate(SubDepartmentNameAlreadyExists)",
    SUBDEPTASSIGNED: "Translate(CannotDeleteHaveReference)",
    DEPARTMENTDETAILS: "Translate(ChooseDepartment)",

    //Fields
    DPT_ACTIVE: "DPT_ACTIVE"

}
//#endregion

///#region-------- Initialization Section ----------------
$(document).ready(function () {
    PageInit();
    FillDepartment(0);
});
//initial page condition
function PageInit() {
    //Reseting all input controls in the page
    $("[id$=btnSave]").hide();
    $("[id$=btnAdd]").show();
    $("[id$=btnPrintLabel]").show();
    $("[id$=divData]").hide();
    $("[id$=divListing]").show();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    BindGrid();   
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;
}

///#endregion

///#region ------- Core Section -------

function FillDepartment(deptID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=DPT_PARENT]").attr("id");
    $.get(StoreDepartmentMaster.GETPRODUCTIONDEPARTMENT + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, deptID);
    });
}
function FillDepartmentCategories(deptID, deptCategoryID) {
    //<summary>Function Used to fill all Department</summary>
    var drpID = $("[id$=DPT_CATEGORY]").attr("id");
    $.get(StoreDepartmentMaster.GETDEPARTMENTCATEGORIES + $("[id$=BizUnitPk]").val() + "&ParentDepartement=" + deptID, function (data) {
        GrandScriptUtils.FillDropDownWithSelect(drpID, data, true, true, deptCategoryID);
    });
}

function FillDepartmentCategoryList() {
    //<summary>Function Used to fill all State Under Selected departement</summary>   
    FillDepartmentCategories($("select[id$=DPT_PARENT] option:selected").val(), 0);
}

function SavePage() {
    ///<summary>Function used to saving Designation Details  </summary>
    AddValidations();
    if ($(document.forms[0]).valid()) {
        $("[id$=DPT_PARENT]").attr("disabled", false);
        $("input[id$=DPT_CODE]").attr("disabled", false);
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(StoreDepartmentMaster.SAVEURL, jSonString, function (data) {///if data=0 already exist if data==1 saved successfully
            if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(StoreDepartmentMaster.SUBDEPTCODEEXISTSMSG, StoreDepartmentMaster.INFORMATIONTITLE);
            }
            else if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(StoreDepartmentMaster.SAVESUCCESS, StoreDepartmentMaster.INFORMATIONTITLE, StoreDepartmentMaster.SAVECMD);
                ClearForm();
            }
            else if (parseInt(data) == -2) {
                GrandScriptUtils.ShowModal(StoreDepartmentMaster.SUBDEPTNAMEEXISTSMSG, StoreDepartmentMaster.INFORMATIONTITLE);
            }
            else {
                GrandScriptUtils.ShowModal(StoreDepartmentMaster.ACTIONFAILED);
            }
        });
    }
    return false;
}

function FillDetails(StoreLocID) {
    ///<summary>Function To Fill saved Details againist PK </summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    $.get(StoreDepartmentMaster.GETSTORELOCDTLSBYPKURL + StoreLocID, function (data) {      
        $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
        FillDepartment(data.Table[0].DPT_PARENT);        
        $("input[id$=DPT_NAME]").val(data.Table[0].DPT_NAME);
        $("input[id$=DPT_CODE]").val(data.Table[0].DPT_CODE);
        $("input[id$=DPT_PK]").val(data.Table[0].DPT_PK);
        if (data.Table[0].DPT_ACTIVE == true) {
            $("input[id$=DPT_ACTIVE]").attr("checked", true);
        }
        else {
            $("input[id$=DPT_ACTIVE]").attr("checked", false);
        }
    });
}

function DeleteDetails() {
    ///<summary>Delete Designaion Details </summary>
    var msgtxt;
    $.get(StoreDepartmentMaster.DELETEURL + StoreLocID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)
            msgtxt = StoreDepartmentMaster.DELETEDSUCCESS;
        else if (parseInt(data) == 0)
            msgtxt = StoreDepartmentMaster.SUBDEPTASSIGNED;
        else
            msgtxt = StoreDepartmentMaster.ACTIONFAILEDMSG;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, StoreDepartmentMaster.INFORMATIONTITLE, StoreDepartmentMaster.SAVECMD);
    });
    return false;
}

function BindGrid(srchVal) {
    ///<summary>Bind Designaion Details With Search value </summary>
    /// <param name="srchVal"  type="Object">
    ///    Search Condition
    /// </param>
    var ajaxUrl = StoreDepartmentMaster.STORELOCATIONBINDGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val() + "&DPTCATEGORY=" + $("[id$=DPT_CATEGORY]").val();
    $("#grdStoreLocation").removeAttr("ajaxurl")
    $("#grdStoreLocation").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdStoreLocation");
    GrandGrid.MakeGrid($("#grdStoreLocation"));
    return false;
}

function AfterSelect() {
    ///<summary>//filling gridview after entering search value.</summary>
    BindGrid();
}

function RedirectToInbox() {
    window.location = StoreDepartmentMaster.INBOX;
    return false;
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
        case StoreDepartmentMaster.DELETECMD:
            StoreLocID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(StoreDepartmentMaster.DELETECONFIRMMSG, StoreDepartmentMaster.CONFIRMATIONTITLE, StoreDepartmentMaster.DELETE, true);
            break;
        // To Edit Details                
        case StoreDepartmentMaster.EDITCMD:
            StoreLocID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            FillDetails(StoreLocID);
            AddNew(1);
            break;
        case StoreDepartmentMaster.VIEWCMD:
            StoreLocID = GrandGrid.Utilities.GetColumnValue(tr, "DPT_PK", $(tr).parent().attr("id"));
            FillDetails(StoreLocID);
            AddNew(0);
            break;
        // Default Handler      
        default:
            GrandScriptUtils.ShowModal(StoreDepartmentMaster.DEFAULTACTION, DesignationMaster.InFormationTitle);
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
        case StoreDepartmentMaster.SAVECMD:          
            ResetPage();
            break;
        //Commend When calling     
        case StoreDepartmentMaster.DELETE:
            DeleteDetails();
            break;
    }
    return false;
}

function AfterGridBind() {
    //<summary>Function Used Hide/Show Delete Dfault type </summary>
    var colIndex = 0;
    var colData = "";
    $("#grdStoreLocation tr:has(td)").each(function () {
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
    $("[id$=btnPrintLabel]").hide();
    $("[id$=divData]").show();
    $("[id$=divListing]").hide();
    $("select[id$=DPT_PARENT]").focus();
    return false;
}

function ResetPage() {
    //<summary>function Used to Reset Page</summary>    
    ClearForm();  
    $(document.forms[0]).validate().resetForm();
    $("input[id$=DPT_CODE]").attr("disabled", false);     
    PageInit();
    $("select[id$=SearchType]").focus();  
    $("[id$=btnSave]").val("Save");   
    $("[id$=btnCancel]").val("Cancel");
    $("input[id$=DPT_ACTIVE]").attr("checked", true);
    return false;
}

function CancelPage() {
    //<summary>Function Used to Cancel Page</summary>
    window.location = StoreDepartmentMaster.StoreDepartmentMasterLISTURL;
    return false;
}

///#endregion

///#region---- Auto Complete Section ----
function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(StoreDepartmentMaster.TEXTEMPTY);
    if (strname == StoreDepartmentMaster.TEXTZERO) {
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
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", StoreDepartmentMaster.STORELOCAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

///#region-------- Validations ----------------

function AddValidations() {

    ///<summary>function To Validations  </summary>
    $("select[id$=DPT_PARENT]").rules("add", {
        selectNone: true,
        messages: { selectNone: StoreDepartmentMaster.SELECTSTOREMSG }
    });
    $("input[id$=DPT_NAME]").rules("add", {
        required: true,
        maxlength: 95,
        messages: { required: StoreDepartmentMaster.PROVIDETITLEMSG }
    });
    $("input[id$=DPT_CODE]").rules("add", {
        required: true,
        maxlength: 95,
        messages: { required: StoreDepartmentMaster.PROVIDECODE }
    });
}
function ClearForm() {
    $("input[id$=DPT_PK]").val("0");
    $("select[id$=DPT_PARENT]").val(0);
    $("input[id$=DPT_NAME]").val('');
    $("input[id$=DPT_CODE]").val('');
}

function RemoveValidation() {
    ///<summary>function To REMOVE  Validations </summary>
    $("select[id$=DPT_PARENT]").rules("remove");
    $("input[id$=DPT_NAME]").rules("remove");
    $("input[id$=DPT_CODE]").rules("remove");
}

function PrintLabel() {
    //var prID = 0;
    //var url = StoreDepartmentMaster.PRINTLABELURL + "?ID=" + prID + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val() + "&DPTCATEGORY=" + $("[id$=DPT_CATEGORY]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    var url = StoreDepartmentMaster.PRINTLABELURL  + "?Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&bizUnit=" + $("[id$=BizUnitPk]").val() + "&DPTCATEGORY=" + $("[id$=DPT_CATEGORY]").val() + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
    OpenPDF(url);
}
///#endregion