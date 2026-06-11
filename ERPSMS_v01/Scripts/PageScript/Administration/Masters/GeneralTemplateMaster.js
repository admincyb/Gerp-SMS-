/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

///#region Global variable Declaration

var TemplateJson = new Object();
var tdset = "";
var TemplateID = 0;
var SLNo = 0; //for capturing current row for editing in details grid
var SLNoDelete = 0;
var TemplateGrpPK = 0;
var TemplateGrpPKSaved = 0;

var GeneraltemplateMaster = {
    //urls
    DeleteTemplateURL: "GeneralTemplateMaster.do?Action=DeleteTemplate&TemplateID=",
    GetTemplateDetailURL: "GeneralTemplateMaster.do?Action=GetTemplateDetail&TemplateID=",
    GetSearchValueURL: "GeneralTemplateMaster.do?Action=GetSearchValue&BizUnitPk=",
    SavePageURL: "GeneralTemplateMaster.do?Action=SavePage",
    SaveTemplateGroupURL: "GeneralTemplateMaster.do?Action=SaveTemplateGroup",
    GetTemplateGroupCombo: "GeneralTemplateMaster.do?Action=GetTemplateGroupCombo&BizUnitPk=",
    GetTemplateGroupGridURL: "GeneralTemplateMaster.do?Action=GetTemplateGroupGrid&BizUnitPk=",
    DeleteTemplateGroupURL: "GeneralTemplateMaster.do?Action=DeleteTemplateGroup&TemplateGrpPK=",
    GetTemplateListURL: "GeneralTemplateMaster.do?Action=GetTemplateList&Status=",
    //Constants
    deleteTemplateGroup: "deleteTemplateGroup",
    Save: "Save",
    DeleteTemplate: "DeleteTemplate",
    saved: "savesd",
    deleteTemplateGroupOK: "deleteTemplateGroupOK",
    DeleteOK: "DeleteOK",
    failed: "failed",
    editTemplateGroup: "editTemplateGroup",
    deleteTemplateGroup: "deleteTemplateGroup",
    SaveTemplateGroupOK: "SaveTemplateGroup",
    SaveTemplateGroupNotOK: "SaveTemplateGroupNotOK",
    DeleteDetail: "DeleteDetail",
    //fields
    SearchValue: "SearchValue",
    SearchType: "SearchType",



    //Messages
    DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",

    Information: "Translate(Information)",
    MessageBoxTitle: "Translate(VendorTermsMaster)",
    ConfirmationMessage: "Translate(Conformation)",
    TermsSaveMessage: "Translate(TermSaveSuccess)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    Alreadyasigned: "Translate(CannotDeleteHaveReference)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    ActionFailed: "Translate(ActionFailedPleaseTryAgain)",
    DeletedMessage: "Translate(DeletedSuccessfully)",
    DeletedMessageTemplateGroup: "Translate(TemplateGroupDeletedSuccessfully)",
    DeletedMessageTemplate: "Translate(GeneralTemplateDeletedSuccessfully)",
    CannotDelete: "Translate(CannotDelete)",
    Savedsuccessfully: 'Translate(TemplateDetailsSavedsuccessfully)',
    TemplateGrpSaveSuccess: 'Translate(TemplateGroupSavedsuccessfully)',
    TermAlreadyExists: "Translate(TermsAdded)",
    //validation
    Pleaseselectanoption: 'Translate(Pleaseselectanoption)',
    PleaseProvideTemplateName: 'Translate(PleaseProvideTemplateName)',
    PleaseselectaTemplateGroup: 'Translate(PleaseselectaTemplateGroup)',
    PleaseProvideTitle: 'Translate(PleaseProvideTitle)',
    PleaseProvideDescription: 'Translate(PleaseProvideDescription)',
    PleaseProvideTemplateGrpName: 'Translate(PleaseProvideTemplateGrpName)',
    TemplateCodealreadyexists: 'Translate(TemplateCodealreadyexists)',
    AddTermDetails: 'Translate(PleaseAddTermDetails)',
    DefaultCannotDelete: "Translate(DefaultCannotDelete)",
    EnterName: "Translate(PleaseEnterName)",
    AlreadyExists: 'Translate(AlreadyExists)',
    //Captions
    GeneralTemplateMaster: "General Template Master"


};

///#endregion

///#region Initialization Section

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');
///<summary>Document read</summary>
$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $("#dialog-Template").dialog({ autoOpen: false });
    $("#dialog:ui-dialog").dialog("destroy");

    PageInit();


    $("[id$=SearchType]").change(function () {
        SetSearchType();
        if ($("select[id$=SearchType]").val() == "0") {
            BindGrid();
        }
    });
    $("[id$=imbSearch]").click(function () {
        BindGrid();
        return false;
    });
});
///<summary>Used for initial settings</summary>
function PageInit() {

    $('[id$=btnSave]').hide();
    $('[id$=btnAddNew]').show();
    $('[id$=divData]').hide();
    $('[id$=divListing]').show();
    //    $("[id$=BizUnitPk]").val($("[id$=SBU]").val());
    $("[id$=BizUnitPk]").val($("[id$=BizUnitPk]").val());
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    SearchInit();
    SetSearchType();

    // BindGrid(0);
    BindGrid();
    TemplateJson = $.parseJSON($("[id$=TemplateDetails]").val());
    $("#divData").data("TemplateData", TemplateJson);
    FillTemplateGroups();
    ResetPage();


}
///<summary>Clear all controls and change the visibility</summary>
function ResetPage() {
    //<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input:not(input[type=submit],input[type=button])").each(function () {
    //        var idval = $(this).attr("id");
    //        if (idval.search("TermPK") != -1)
    //            $(this).val('0');
    //        Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1)
    //            $(this).val("");
    //    });

    //    Selecting the first value in all drop downs
    //    $(document.forms[0]).find("select").each(function () {
    //        $(this).val($(this).find("option:eq(0)").val());
    //    });

    $("[id$=SearchType]").val("0");
    $("[id$=SearchValue]").val('');
    $("[id$=SearchValue]").hide();
    $("[id$=imbSearch]").hide();
    $("select[id$=TMH_TERM_GROUP]").val("0");
    $("input[id$=TMH_NAME]").val('');
    $("input[id$=TMH_PK]").val("0");
    ClearTemplateDetails();
    var ObjDisp = $("#divData").data("TemplateData");
    ObjDisp.TemplateDetails = new Array();
    $("#divData").data("TemplateData", ObjDisp);

    // HideAdvSearch();
    $(document.forms[0]).validate().resetForm();
    $('[id$=btnSave]').hide();
    $('[id$=btnAddNew]').show();
    $('[id$=divData]').hide();
    $('[id$=divListing]').show();

    //    $("input[id$=BizUnitPk]").val("1");
    //    $("input[id$=UserPk]").val("1");
    //    $("input[id$=DeptPk]").val("1");
    $("input[id$=TemplateGrpPK]").val("0");
    BindGrid();
    return false;
}

///#endregion
function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}
///#region Grid handler & Mopdal OK

///<summary>Event handler for Model ok click</summary>
function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        //comment req 

        case GeneraltemplateMaster.deleteTemplateGroup:
            DeleteTemplateGroupDetails();
            break;
        case GeneraltemplateMaster.Save:
            BindGrid();
            break;
        //Commend When calling    
        case GeneraltemplateMaster.DeleteTemplate:
            DeleteDetailsMain();
            break;
        case GeneraltemplateMaster.saved:
            PageInit();
            break;
        case GeneraltemplateMaster.deleteTemplateGroupOK:
            FillTemplateGroups();
            BindTemplateGroupGrid();
            break;
        case GeneraltemplateMaster.DeleteOK:
            BindGrid();
        case GeneraltemplateMaster.SaveTemplateGroupOK:
            ClearTemplateGroupDetails();
            FillTemplateGroups();
            BindTemplateGroupGrid();
            break;
        case GeneraltemplateMaster.DeleteDetail:
            DeleteDetails();
            break;




    }
    return false;
}
//<summary>Grid Handler for grdTermsDetails Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            SLNoDelete = GrandGrid.Utilities.GetColumnValue(tr, "SL", "grdTermsDetails");
            GrandScriptUtils.ShowModal(GeneraltemplateMaster.DeleteConfirmation, GeneraltemplateMaster.ConfirmationMessage, GeneraltemplateMaster.DeleteDetail, true)

            return false;
            break;
        case "edit":
            $("[id$=btnAddNew]").hide();
            $("[id$=btnSave]").show();
            FillTemplateDetailsTerm(tr);
            return false;
            break;
        default:
            alert(GeneraltemplateMaster.DefaultAction);
            return false;
            break;
    }
}
///<summary>Grid Handler - Template group Catch all the grid events in this function </summary>
function GridHandlerTemplateGroup(tr, command) {
    switch (command.toString()) {
        // To Delete Details          
        case GeneraltemplateMaster.deleteTemplateGroup:
            TemplateGrpPK = GrandGrid.Utilities.GetColumnValue(tr, "TMG_PK", "grdTemplateGroup");
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(GeneraltemplateMaster.DeleteConfirmation, GeneraltemplateMaster.Information, GeneraltemplateMaster.deleteTemplateGroup, true);
            break;
        // To Edit Details                   
        case GeneraltemplateMaster.editTemplateGroup:
            FillTemplateGroupDetails(tr);
            break;
        default:
            alert(GeneraltemplateMaster.DefaultAction);
            break;
    }
    return false;

}
///<summary>Grid Handler for grdTermsList  Catch all the grid events in this function </summary>
function GridHandlerMain(tr, command) {
    switch (command.toString().toLowerCase()) {
        // To Delete Details   
        case "delete":

            TemplateID = GrandGrid.Utilities.GetColumnValue(tr, "TMH_PK", $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(GeneraltemplateMaster.DeleteConfirmation, GeneraltemplateMaster.Information, GeneraltemplateMaster.DeleteTemplate, true)
            break;

        // To Edit Details   
        case "edit":
            AddNew();
            FillDetails(tr);
            break;

        default:
            alert(GeneraltemplateMaster.DefaultAction);
            break;
    }
    return false;

}

///#endregion

///#region AutoComplete Search

///<summary>Function To Enable/Disable Selected Option For Search </summary>
function SetSearchType() {
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("[id$=SearchValue]").hide();
        $("[id$=imbSearch]").hide();
        BindGrid();
    } else {
        $("[id$=SearchValue]").show();
        $("[id$=imbSearch]").show();
    }
}
///After selecting autocomplete
function AfterSelect() {
    BindGrid();
    return false;
}
///<summary>Invoke advanced search</summary>
function AdvanceSearchInvoke(srchVal) {
    BindGrid(srchVal);
}
///<summary>To handle auto complete</summary>
function SearchInit() {
    GrandScriptUtils.MakeAutoCompleteSearch(GeneraltemplateMaster.SearchValue, GeneraltemplateMaster.GetSearchValueURL + $("[id$=BizUnitPk]").val(), GeneraltemplateMaster.SearchType);
}

///#endregion

///#region Validation

///<Summary>Add validations to controls<summary>
function AddValidations(mode) {
    RemoveValidations();
    //Mode = 1 represents the validation for  General Template  Header Details
    if (mode == "1") {
        $('input[id$=TMH_NAME]').rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: GeneraltemplateMaster.PleaseProvideTemplateName }
        });


        $('select[id$=TMH_TERM_GROUP]').rules("add", {
            selectNone: true,
            messages: { selectNone: GeneraltemplateMaster.PleaseselectaTemplateGroup }
        });


    }
    //Mode =  2 represents the validation for Dispersion Material Details
    else if (mode == "2") {

        $('input[id$=TMD_NAME]').rules("add", {
            required: true,
            maxlength: 50,
            messages: { required: GeneraltemplateMaster.PleaseProvideTitle }
        });
        $('textarea[id$=TMD_DESC]').rules("add", {
            required: true,
            maxlength: 1000,
            messages: { required: GeneraltemplateMaster.PleaseProvideDescription }
        });
        $("[id$=TMD_MAX_POINT]").rules("add", {
            //            required: true,
            ThreeDecimal: true,
            maxlength: 3,
            messages: { required: "Translate(EnterValidMaxPoint)" }
        });


    }
    //Mode =  2 represents the validation for Dispersion Material Details
    else if (mode == "3") {

        $('input[id$=TermName]').rules("add", {
            required: true,
            maxlength: 50,
            messages: { required: "Enter Name" }
        });

    }

}
///<summary>Remove all validations</summary>
function RemoveValidations() {
    $('input[id$=TMH_NAME]').rules("remove");
    $('select[id$=TMH_TERM_GROUP]').rules("remove");
    $('input[id$=TMD_NAME]').rules("remove");
    $('textarea[id$=TMD_DESC]').rules("remove");
    //$('input[id$=TermName]').rules("remove");

}

///#endregion

///#region Data Management -Template Details

///<summary>Delete Terms Detail </summary>
function DeleteDetailsMain(tr) {
    ///<summary>Function To Get delete and Delete Terms Detail, And Finally, Fill Remaining Data</summary>
    /// <param name="tr"  type="object">
    ///      deleted row
    /// </param>
    var msgtxt;
    $.get(GeneraltemplateMaster.DeleteTemplateURL + TemplateID, function (data) {
        //Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) == 1)//Delete success
            msgtxt = GeneraltemplateMaster.DeletedMessageTemplate;
        else if (parseInt(data) == 0)//Unable to delete dueu to dependencey
            msgtxt = GeneraltemplateMaster.Alreadyasigned;
        else if (parseInt(data) == -2)//Default cannot be deleted
            msgtxt = GeneraltemplateMaster.DefaultCannotDelete;
        else //Delete failure
            msgtxt = GeneraltemplateMaster.ActionFailed;
        // Show MeesageBox For  Delete Status
        GrandScriptUtils.ShowModal(msgtxt, GeneraltemplateMaster.GeneralTemplateMaster, GeneraltemplateMaster.DeleteOK);

    });
    return false;
}
//<summary>fill Template</summary>
function FillDetails(tr) {

    //Get OrderID From tr - For Pass this as QueryString
    var TemplatePK = GrandGrid.Utilities.GetColumnValue(tr, "TMH_PK", $(tr).parent().parent().attr("id"));
    var objDisp = new Object();
    $.get(GeneraltemplateMaster.GetTemplateDetailURL + TemplatePK, function (data) {
        $("#divData").data("TemplateData", data);
        AddSLNo();
        FillTemplateDetails();

    });
}
///<summary>Used to fill General Template Details for editing</summary>
function FillTemplateDetails() {

    // RemoveValidations();
    TemplateJson = $("#divData").data("TemplateData");

    //Fill inputs
    $("select[id$=TMH_TERM_GROUP]").val(TemplateJson.TMH_TERM_GROUP);
    $("input[id$=TMH_NAME]").val(TemplateJson.TMH_NAME);

    $("input[id$=TMH_PK]").val(TemplateJson.TMH_PK);


    // Check TemplateJson.TemplateDetails is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign TemplateDetails Details to that Array, and then push Array to TemplateJson.TemplateDetails
    if (!($.isArray(TemplateJson.TemplateDetails))) {
        var objArray = TemplateJson.TemplateDetails;
        TemplateJson.TemplateDetails = new Array();
        TemplateJson.TemplateDetails.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdTermsDetails"), 0, TemplateJson.TemplateDetails);

    //    if (tdset == "") {
    //        tdset = $("#TermsInsert").find("tr:eq(1)");
    //    }
    //    $("#TermsInsert").hide();
    //    $("#grdTermsDetails").show();
    //    $(tdset).insertBefore($("#grdTermsDetails").find("tr:eq(1)"));
    //AfterGridBind();
}
///Add Serial No to  the object
function AddSLNo() {
    ObjDisp = $("#divData").data("TemplateData");
    //Reset SL

    if (!($.isArray(ObjDisp.TemplateDetails))) {
        var objArray = ObjDisp.TemplateDetails;
        ObjDisp.TemplateDetails = new Array();
        ObjDisp.TemplateDetails.push(objArray);
    }
    for (var i in ObjDisp.TemplateDetails) {
        ObjDisp.TemplateDetails[i].SL = parseInt(i) + 1;
        //Add Yes/No text to bind grid
        ObjDisp.TemplateDetails[i].TMD_REQD_TEXT = ObjDisp.TemplateDetails[i].TMD_REQD == "1" ? "Yes" : "No";

    }
    for (var i in ObjDisp.TemplateDetails) {
        ObjDisp.TemplateDetails[i].SL = parseInt(i) + 1;
        //Add Yes/No text to bind grid
        ObjDisp.TemplateDetails[i].TMD_ACTIVE_TEXT = ObjDisp.TemplateDetails[i].TMD_ACTIVE == "1" ? "Active" : "Inactive";

    }

    //save to data
    $("#divData").data("TemplateData", ObjDisp);
}
//<summary>function used to add Terms details to  General Template</summary>
function AddTemplateDetails() {
    //Add Validation for Terms details Details by setting mode as 2

    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("TemplateData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var flag = true;
        var TermsID = $("input[id$=TermsID]").val();

        obj.TMD_PK = $("input[id$=TMD_PK]").val();
        obj.TMD_NAME = $.trim($("input[id$=TMD_NAME]").val());
        obj.TMD_DESC = $("textarea[id$=TMD_DESC]").val();
        obj.TMD_REQD = $("input[id$=TMD_REQD]").is(':checked') == true ? 1 : 0;
        obj.TMD_REQD_TEXT = obj.TMD_REQD == 1 ? "Yes" : "No";
        obj.TMD_ACTIVE = $("input[id$=TMD_ACTIVE]").is(':checked') == true ? 1 : 0;
        obj.TMD_ACTIVE_TEXT = obj.TMD_ACTIVE == 1 ? "Active" : "Inactive";
        obj.TMD_MAX_POINT = $.trim($("input[id$=TMD_MAX_POINT]").val());

        //check if editing existing row.
        if (SLNo == "0") { //new row
            obj.SL = ObjDisp.TemplateDetails.length + 1
            //check for duplicate term name
            for (var i in ObjDisp.TemplateDetails) {
                if (ObjDisp.TemplateDetails[i].TMD_NAME == $.trim($("input[id$=TMD_NAME]").val())) {
                    GrandScriptUtils.ShowModal(GeneraltemplateMaster.TermAlreadyExists, GeneraltemplateMaster.Information, false);
                    return false;
                }
            }

            ObjDisp.TemplateDetails.push(obj);
        }
        else { //editing an existing row

            //check for duplication

            for (var i in ObjDisp.TemplateDetails) {
                if (i != SLNo - 1)//not current editing row
                {
                    if (ObjDisp.TemplateDetails[i].TMD_NAME == $.trim($("input[id$=TMD_NAME]").val())) {
                        GrandScriptUtils.ShowModal(GeneraltemplateMaster.TermAlreadyExists, GeneraltemplateMaster.Information, false);
                        return false;
                    }
                }
            }
            obj.SL = SLNo;
            ObjDisp.TemplateDetails[SLNo - 1] = obj;
            SLNo = 0;

        }
        $("#divData").data("TemplateData", ObjDisp);
        GrandGrid.MakeGrid($("#grdTermsDetails"), 0, ObjDisp.TemplateDetails);

        //                $("#grdTermsDetails tr td:eq(4)").each(function () {
        //                    this.text == "0" ? "Yes" : "No";
        //                });

        ClearTemplateDetails();
    }

    return false;
}
///<Summary>Clear Term input fields<summary> 
function ClearTemplateDetails() {
    $("input[id$=TMD_NAME]").val('');
    $("input[id$=TMD_MAX_POINT]").val('');
    $("textarea[id$=TMD_DESC]").val('');
    $("input[id$=TMD_REQD]").attr('checked', false);
    $("input[id$=TMD_ACTIVE]").attr('checked', false);
    $("input[id$=TMD_PK]").val(0);

}
///<summary>Initialize the data entry screen</summary>
function AddNew() {
    $("#divData").show();
    $("#divListing").hide();
    $("[id$=btnAddNew]").hide();
    $("[id$=btnSave]").show();
    var dummyObj = new Object();
    //initialize grid
    GrandGrid.MakeGrid($("#grdTermsDetails"), 0, dummyObj);
    $(tdset).insertAfter($("#TermsInsert").find("tr:eq(0)"));
    $("#TermsInsert").show();
    $("#TermsInsert").css({ "display": "block", "visibility": "visible" });
    $("#grdTermsDetails").hide();
    BindTemplateGroupGrid();
    return false;
}
//<summary>function Call Afer binding Grid</summary>
function AfterGridBind(grdID) {
    if (grdID == "grdTermsDetails") {

        if (tdset == "") {
            tdset = $("#TermsInsert").find("tr:eq(1)");
        }
        $("#TermsInsert").hide();
        $("#TermsInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdTermsDetails").show();
        $(tdset).insertBefore($("#grdTermsDetails").find("tr:eq(1)"));
        $("#grdTermsDetails tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "TMD_PK", grdID);
            colIndexActive = GrandGrid.Utilities.GetColumnIndex($(this), "TMD_ACTIVE_TEXT", grdID);
            if (colIndexActive != null) {
                var itemActiveText = GrandGrid.Utilities.GetColumnValue($(this), "TMD_ACTIVE_TEXT", grdID);
                if (itemActiveText == "Active") {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"active\" title=\"Translate(Active)\"  alt=\"\" />");
                }
                else {
                    $(this).find("td:eq(" + colIndexActive + ")").html("<img id=\"IMG_ITEM_" + index + "\"  class=\"inactive\" title=\"Translate(Inactive)\"  alt=\"\" />");
                }
            }
        });

    }
}
////<summary>save  General Template to the database</summary>
function SavePage() {
    //Add Validation for  General Template header Details by setting mode as 1
    AddValidations(1);
    //  $("input[ID$=BizUnitPk]").val($("select[ID$=SBUName]").val());
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("TemplateData");
        // Check if the  General Template have alteast 1 Term details added
        if (ObjDisp.TemplateDetails.length > 0) {
            if ($(document.forms[0]).valid()) {
                var ObjDisp = $("#divData").data("TemplateData");
                //Assigning the Term details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=TemplateDetails]").val(JSON.stringify(ObjDisp.TemplateDetails));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                //ajax save request
                $.ajax({
                    type: "post",
                    url: GeneraltemplateMaster.SavePageURL,
                    data: jSonString,
                    contentType: "application/json",
                    dataType: "text",
                    success: function (data) {

                        if (parseInt(data) > 0) {
                            GrandScriptUtils.ShowModal(GeneraltemplateMaster.Savedsuccessfully, GeneraltemplateMaster.Information, GeneraltemplateMaster.saved);
                            BindGrid();
                        }
                        else {
                            var msgtxt;
                            if (parseInt(data) == 0)
                                msgtxt = GeneraltemplateMaster.TemplateCodealreadyexists;
                            else if (parseInt(data) < 0)
                                msgtxt = GeneraltemplateMaster.ActionFailed;
                            GrandScriptUtils.ShowModal(msgtxt, 'Translate(Status)', GeneraltemplateMaster.failed);
                        }
                    }
                });

            }

        }
        else {
            GrandScriptUtils.ShowModal(GeneraltemplateMaster.AddTermDetails, GeneraltemplateMaster.Information);
        }
    }
    return false;
}
////<summary>fill Terms details for edit</summary>
function FillTemplateDetailsTerm(tr) {

    $("input[id$=TMD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_PK", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_NAME", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_MAX_POINT]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_MAX_POINT", $(tr).parent().parent().attr("id")));
    $("textarea[id$=TMD_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_DESC", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_REQD]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, "TMD_REQD", $(tr).parent().parent().attr("id")) == 0 ? false : true);
    $("input[id$=TMD_ACTIVE]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, "TMD_ACTIVE", $(tr).parent().parent().attr("id")) == 0 ? false : true);
    SLNo = GrandGrid.Utilities.GetColumnValue(tr, "SL", $(tr).parent().parent().attr("id"));
    //$("input[id$=IsEdit]").val("true");
    $("input[id$=TMD_NAME]").focus();
}
///<summary>For delete the item in the grid -  General Template Details</summary>
function DeleteDetails(tr) {
    // var SL = GrandGrid.Utilities.GetColumnValue(tr, "SL", "grdTermsDetails");
    var SL = SLNoDelete;
    var ObjDisp = $("#divData").data("TemplateData");

    //Delete Row
    for (var i in ObjDisp.TemplateDetails) {
        if (ObjDisp.TemplateDetails[i].SL == SL) {
            //Will delete the Term details
            ObjDisp.TemplateDetails.splice(i, 1);
            break;
        }
    }

    //Reset SL
    for (var i in ObjDisp.TemplateDetails) {
        ObjDisp.TemplateDetails[i].SL = parseInt(i) + 1;

    }

    $("#divData").data("TemplateData", ObjDisp);
    GrandGrid.MakeGrid($("#grdTermsDetails"), 0, ObjDisp.TemplateDetails);
    //Used to Show the Term  details when the Materials in Dispersion is 0
    if (ObjDisp.TemplateDetails.length == 0) {
        //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
        $(tdset).insertAfter($("#TermsInsert").find("tr:eq(0)"));
        $("#TermsInsert").show();
        $("#TermsInsert").css({ "display": "block", "visibility": "visible" });
    }
}
///<summary>Method to bind the Template grid</summary>
function BindGrid() {
    var ajaxUrl = GeneraltemplateMaster.GetTemplateListURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&BizUnitPk=" + $("[id$=BizUnitPk]").val();
    $("#grdTermsList").removeAttr("ajaxurl")
    $("#grdTermsList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTermsList");
    GrandGrid.MakeGrid($("#grdTermsList"));
}

///#endregion

///#region Data Management Template Group

///<summary>To show the template group details grid</summary>
function ShowTemplateGroup() {
    $("#dialog-Template").dialog({ width: 750, height: 300, buttons: {} });
    $("#dialog-Template").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    return false;
}
//<summary>function To Save template group Details </summary>
function SaveTemplateGroup() {

    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        var msgTxt;

        var jSonString = "{" + "'TemplateGrpPK':'" + $("input[id$=TemplateGrpPK]").val() + "'," + "'TemplateGrpName':'" + $("input[id$=TermName]").val() + "'," + "'BizUnitPk':'" + $("input[id$=BizUnitPk]").val() + "'," + "'DeptPk ':'" + $("input[id$=DeptPk]").val() + "'," + "'UserPK':'" + $("input[id$=UserPk]").val() + "'}";
        $.ajax({
            type: "post",
            url: GeneraltemplateMaster.SaveTemplateGroupURL,
            data: jSonString,
            contentType: "application/json",
            dataType: "text",
            success: function (data) {
                // Check template group Saved Successfully or Not - >0 Success ,0-  Already Exists, <0 - Fail(Exception)
                if (parseInt(data) > 0)
                    msgTxt = GeneraltemplateMaster.TemplateGrpSaveSuccess;
                else if (parseInt(data) == 0)
                    msgTxt = GeneraltemplateMaster.AlreadyExists;
                else if (parseInt(data) < 0)
                    msgTxt = GeneraltemplateMaster.ActionFailed;

                if (parseInt(data) > 0) {
                    TemplateGrpPKSaved = data;
                    GrandScriptUtils.ShowModal(msgTxt, GeneraltemplateMaster.Information, GeneraltemplateMaster.SaveTemplateGroupOK);
                    return false;
                }
                else {

                    GrandScriptUtils.ShowModal(msgTxt, GeneraltemplateMaster.Information, GeneraltemplateMaster.SaveTemplateGroup);
                    return false;
                }
            }
        });
        $('input[id$=TermName]').rules("remove");
        return false;
    }
}
///<summary>fill Template group in the dropdown</summary>
function FillTemplateGroups() {

    var drpID = $("select[id$=TMH_TERM_GROUP]").attr("id");
    //Fill Template group Details to the Template group  DropDown, Name as Text, PK as Value
    $.get(GeneraltemplateMaster.GetTemplateGroupCombo + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, 0);
        //select newly added template group
        if (parseInt(TemplateGrpPKSaved) > 0) {
            $("[id$=TMH_TERM_GROUP]").val(TemplateGrpPKSaved);
            TemplateGrpPKSaved = "0"; //reset  after
        }
    });
}
//<summary>function To Bind Template group Details </summary>
function BindTemplateGroupGrid() {
    var ajaxUrl = GeneraltemplateMaster.GetTemplateGroupGridURL + $("[id$=BizUnitPk]").val();
    $("#grdTemplateGroup").removeAttr("ajaxurl")
    $("#grdTemplateGroup").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdTemplateGroup");
    GrandGrid.MakeGrid($("#grdTemplateGroup"));
}
///<summary>Fill Template group details </summary>
function FillTemplateGroupDetails(tr) {

    $("input[id$=TemplateGrpPK]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMG_PK", "grdTemplateGroup"));
    $("input[id$=TermName]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMG_NAME", "grdTemplateGroup"));
    $("input[id$=TermName]").focus();

}
///<summary>Clear Template group Details </summary>
function ClearTemplateGroupDetails() {
    $("input[id$=TemplateGrpPK]").val("0");
    $("input[id$=TermName]").val('');

}
///<summary>Delete Template group Details </summary>
function DeleteTemplateGroupDetails() {
    var msgtxt;
    $.ajax
        ({
            type: "post",
            url: GeneraltemplateMaster.DeleteTemplateGroupURL + TemplateGrpPK,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (data) {
                //Check Deleted Succesfully or Not - 1-Sucess 0-Already assigned
                if (parseInt(data) == 1)
                    msgtxt = GeneraltemplateMaster.DeletedMessageTemplateGroup;
                else if (parseInt(data) == 0)
                    msgtxt = GeneraltemplateMaster.Alreadyasigned;
                else if (parseInt(data) == -2)
                    msgtxt = GeneraltemplateMaster.DefaultCannotDelete;
                else
                    msgtxt = GeneraltemplateMaster.ActionFailed;

                GrandScriptUtils.ShowModal(msgtxt, GeneraltemplateMaster.Information, GeneraltemplateMaster.deleteTemplateGroupOK);
            }
        });
    return false;
}

///#endregion

function CancelPage() {
    PageInit();
    return false;
}

  
