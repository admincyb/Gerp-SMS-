/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />
/// <reference path="../../GrandScriptUtils.js" />


///#region Global Variable
var evaluationID = 0;
var EvalJson = new Object();
var tdset = "";
var InitialSupplier = "0";
var InitialProduct = "0";
var groupID = "0";
var ParamID = "0";
var IsReadOnly = "0";
var QtyDec, AmtDec;
var VND_PK = $("[id$=VND_PK]").val();
var VndEval = {
    //urls
    URLGetEvalDetail: "VendorEvaluationManagement.do?Action=GetEvalDetail&EvalID=",
    //URLGetParameters: "GeneralTemplateMaster.do?Action=GetGeneralTerms&TermsID=1&BizUnitPk=",
    URLGetVendorAddress: "VendorRegistration.do?Action=GetVendorAddress&VendorId=",
    URLGetSuppliedMaterial: "VendorEvaluationManagement.do?Action=GetSuppliedMaterial&VendorId=",
    URLGetEvaluations: "VendorEvaluationManagement.do?Action=GetEvaluations&ParameterID=",
    URLDelete: "VendorEvaluationManagement.do?Action=DeleteEvaluation&EvaluationID=",
    URLGetVendors: "VendorRegistration.do?Action=GetVendors&SBUPk=",
    URLSavePage: "VendorEvaluationManagement.do?Action=SavePage&VendorID=",
    //URLGetParameters: "VendorEvaluationManagement.do?Action=GetParameters&TemplatePK=1&BizUnitPk=",//New - Change the template PK
    URLGetParameters: "VendorEvaluationManagement.do?Action=GetParameters&BizUnitPk=", //New - Change the template PK

    //New
    URLGetParameterValue: "VendorEvaluationManagement.do?Action=GetParameterValue&BizUnitPk=",

    URLGetEvalGroups: "VendorEvaluationManagement.do?Action=GetEvalGroups&BizUnitPk=",
    //New End
    IsViewMode: false,
    RedirectInbox: "../AccountManagement/WorkflowInbox.aspx",
    RedirectListingURL: "../vendormanagement/VendorEvaluationListing.aspx",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompany&SBUPk=",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",
    //Constants
    INBOX: "INBOX",
    DeleteMsg: "deletemsg",
    Delete: "delete",
    SaveOK: "SaveOK",
    DeleteDetail: "DeleteDetail",
    //Evaluation Status
    EvaluationStatus: { //workflow actions
        Drafted: 0,
        Evaluated: 4,
        Approved: 6,
        Rejected: 5

    },
    //fields
    //Messages
    DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",

    MessageBoxTitle: "Translate(VendorTermsMaster)",
    ConfirmationMessage: "Translate(Conformation)",
    TermsSaveMessage: "Translate(TermSaveSuccess)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    Alreadyasigned: "Translate(CannotdeleteAlreadyasigned)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    ActionFailed: "Translate(ActionFailedPleaseTryAgain)",
    DeletedMessage: "Translate(DeletedSuccessfully)",
    CannotDelete: "Translate(CannotDelete)",
    Savedsuccessfully: 'Translate(EvaluationDetailsSavedSuccesfully)',
    Submitedsuccessfully: 'Translate(EvaluationDetailssubmittedSuccesfully)',
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    //validation
    PleaseselectaSupplier: 'Translate(PleaseselectaSupplier)',
    PleaseselectaProduct: 'Translate(PleaseselectaProduct)',
    PleaseselectaGroup: 'Translate(PleaseselectaGroup)',
    PleaseselectaParameter: 'Translate(PleaseselectaParameter)',
    PleaseselectaPoints: 'Translate(PleaseselectaPoints)',
    PleaseProvideRemarks: 'Translate(PleaseProvideRemarks)',
    AlreadyExists: 'Already Exists',
    EvalAlreadyAdded: 'Translate(EvaluationAlreadyAdded)',
    VendoEvalDetails: 'Translate(EvaluationDetails)',
    Information: "Translate(Information)",
    Save: 'Translate(save)',
    PleaseEvaltCriteria: 'Translate(PleaseEvaluateAnyCriterea)'
}

///#endregion

///#region------ Initialization Section ----------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    //For Adding rule to Select
    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
    }, 'Translate(Pleaseselectanoption)');

    var dummyObj = new Object();
    GrandGrid.MakeGrid($("#grdEvalDetails"), 0, dummyObj);

    //initialize Vendor Object
    EvalJson = $.parseJSON($("[id$=EvalDetailsList]").val());
    $("#divEvalData").data("EvalData", EvalJson);

    PageInit();

    //dropdown changing and filling  Evaluation details wrp parameter id
    $("[id$=VED_PARAM]").change(function (event) {

        event.preventDefault();
        if ($("[id$=VED_PARAM]").val() != "0") {
            var parameterID = $("[id$=VED_PARAM]").val();

            // FillEval(parameterID,SelectVal) 
            //S  FillEvaluation(parameterID, false);

        }

    });
    QtyDec = $("[id$='hdfQtyDecimal']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    //dropdown changing and filling  vendor details and filling supplied materials
    $("[id$=VEH_VENDOR]").change(function (event) {

        event.preventDefault();
        if ($("[id$=VEH_VENDOR]").val() != "0") {
            var vendorID = $("[id$=VEH_VENDOR]").val();
            $("#SpnName").html($("[id$=VEH_VENDOR] :selected").text());


            //function for filling vendor details
            GetVendorAddress(vendorID);
            //function for filling Supplied material details
            GetSuplliedMaterial(vendorID);

        }
        else {
            $('#CodeSupplier').text('');
            $('#SupplierAddress').text('');
            $("#SpnName").html("-");
            GetSuplliedMaterial(0); //clear Dropdown
        }

    });

});
//initial page condition
function PageInit() {
    //Reseting all input controls in the page
    var queryString = window.location.search.substring(1);
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                VndEval.IsViewMode = true;
            }
        }
    }

    $('[id$=btnSave]').hide();
    $('[id$=imbDraft]').hide();
    $('[id$=imbAdd]').show();
    $('[id$=divData]').hide();
    $('[id$=divListing]').show();
    //    IsReadOnly = $("[id$=Readonly]").val();
    AddNew();
    //NewEval Start
    FillGroups();
    //New End
    //FillParameters();
    FillVendors();

    FillEvaluationDetails();
    FillCompany(0);

    return false;
}
///<summary>Mehtod to fill the evaluation details if 'EvalID' is specified in the query string</summary>
function FillEvaluationDetails() {

    var ID = $("[ID$=EvalDetailsList]").val();
    InitialSupplier = $("[ID$=VND_PK]").val();
    if (parseInt(ID) > 0) { //for editing already saved Evaluation
        $.get(VndEval.URLGetEvalDetail + parseInt(ID), function (data) {
            $("#divEvalData").data("EvalData", data);
            AddNew();
            FillEvalDetails();
        });
    }
    else { //if blank entry form is loading
        var date = new Date()
        $("#Date").html(date.toDateString());
        $("[id$=VEH_ISSUED_BY]").val($("[id$=UserPk]").val());
        //        var date = new Date()
        //        $("#Date").html(date.toDateString());
        GrandScriptUtils.FillDate("SpnDate", false, true);
    }
}
///<summary>Used to fill Evaluation  Details for editing</summary>
function FillEvalDetails() {

    RemoveValidations();
    EvalJson = $("#divEvalData").data("EvalData");

    //Fill
    $("[id$=VEH_PK]").val(EvalJson.VEH_PK);
    $("[id$=lblissuedby]").html(EvalJson.ISSUED_USER);
    EvalJson.APPROVED_USER != null ? $("[id$=lblApprovedBy]").html(EvalJson.APPROVED_USER) : $("[id$=lblApprovedBy]").html("-");
    $("[id$=VEH_ISSUED_BY]").val(EvalJson.VEH_ISSUED_BY);
    $("[id$=VEH_APPROVED_BY]").val($("[id$=UserPk]").val());
    $("select[id$=VEH_VENDOR]").val(EvalJson.VEH_VENDOR);
    $("[id$=VEH_STATUS]").val(EvalJson.VEH_STATUS);
    $("[id$=VEH_VENDOR]").attr("disabled", "disabled");
    $("[id$=VEH_ITEM]").attr("disabled", "disabled");
    $("#SpnDate").html(EvalJson.VEH_MOD_DT);
    $("#SpnName").html($("[id$=VEH_VENDOR] :selected").text());
    $("[id$=LAST_MOD_DT]").val(EvalJson.LAST_MOD_DT);
    //make approved by div visible
    $("#divApprovedBy").show();
    //$("[id$=lblApprovedBy]").html($("[id$=UserPk]"));

    //for setting selected value asynchronously;
    InitialSupplier = EvalJson.VEH_VENDOR;

    //New Code 07-01-2014
    if (VndEval.IsViewMode) {
        IsReadOnly = 1;
    }
    else if (EvalJson.VEH_STATUS == 1 || EvalJson.VEH_STATUS == 7 || EvalJson.VEH_STATUS == 11) {
        IsReadOnly = 1;
    }
    $("[id$=hdfSelCompany]").val(EvalJson.VEH_COMPANY)
    FillCompany(EvalJson.VEH_COMPANY)

    if ($("[id$=VEH_VENDOR]").val() != "0") {
        // var vendorID = $("[id$=VEH_VENDOR]").val();
        var vendorID = EvalJson.VEH_VENDOR;
        //function for filling vendor details
        GetVendorAddress(vendorID);
        //function for filling Supplied material details
        InitialProduct = EvalJson.VEH_ITEM;
        GetSuplliedMaterial(vendorID, EvalJson.VEH_ITEM);
    }
    else {
        $('#CodeSupplier').text('');
        $('#SupplierAddress').text('');
    }


    // Check EvalJson.EvalDetailsList is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign OrderList Details to that Array, and then push Array to EvalJson.EvalDetailsList
    if (!($.isArray(EvalJson.EvalDetailsList))) {
        var objArray = EvalJson.EvalDetailsList;
        EvalJson.EvalDetailsList = new Array();
        EvalJson.EvalDetailsList.push(objArray);
    }

    GrandGrid.MakeGrid($("#grdEvalDetails"), 0, EvalJson.EvalDetailsList);
    CalculateResults();
}

///#endregion

///#region---- Set Or Reset Form----

///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    $('[id$=btnSave]').show();
    $('[id$=imbDraft]').show();
    $('[id$=imbAdd]').hide();
    $('[id$=divData]').show();
    $('[id$=divListing]').hide();
    $("[id$=VEH_VENDOR]").focus();

    var dummyObj = new Object();
    //initialize grid
    GrandGrid.MakeGrid($("#grdEvalDetails"), 0, dummyObj);
    $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
    $("#ProductInsert").show();
    $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
    $("#grdEvalDetails").hide();
    $('#CodeSupplier').text('');
    $('#SupplierAddress').text('');
    RemoveValidations();
    return false;
}
//<summary>function Used to Reset Page</summary>
function ResetPage() {

    window.location = VndEval.RedirectListingURL;
    return false;
    //    var ObjDisp = $("#divEvalData").data("EvalData");
    //    ObjDisp.EvalDetailsList = new Array();

    //    $('[id$=btnSave]').hide();
    //    $('[id$=imbAdd]').show();
    //    $('[id$=divData]').hide();
    //    $('[id$=divListing]').show();

    //    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input").each(function () {
    //        var idval = $(this).attr("id");
    //        if (idval.search("SupplierMaterialID") != -1)
    //            $(this).val('0');
    //        else if (idval.search("EditEvaluation") != -1)
    //            $(this).val('0');
    //        //Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1)
    //            $(this).val("");
    //    });

    //    //Selecting the first value in all drop downs
    //    $(document.forms[0]).find("select").each(function () {
    //        $(this).val($(this).find("option:eq(0)").val());
    //    });
    //    $(document.forms[0]).validate().resetForm();
    //    return false;
}

///#endregion

///#region----Grid Handlers And Model Popup Ok Click----

///<summary>Grid Handler Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            ParamID = GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", "grdEvalDetails");
            GrandScriptUtils.ShowModal(VndEval.DeleteConfirmation, VndEval.ConfirmationMessage, "DeleteDetail", true)
            // DeleteEvaluationDetails(tr);
            return false;
            break;
        case "edit":
            FillDetails(tr);
            return false;
            break;

        default:
            alert(VndEval.DefaultAction);
            return false;
            break;
    }
}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {
    switch (command) {
        case VndEval.INBOX:
            window.location = VndEval.InboxURL;
            break;
        case VndEval.SaveOK:
            window.location = VndEval.RedirectListingURL;
            break;
        case VndEval.DeleteDetail:
            DeleteEvaluationDetails();
            break;

    }
    return false;
}

///#endregion

///#region---- Fetch Data To Populate In Controls

//<summary>function To Fill Parameters Details </summary>
function FillVendors() {
    // Get id of the Supplier DropDown
    var drpID = $("select[id$=VEH_VENDOR]").attr("id");
    $.get(VndEval.URLGetVendors + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
        if ($("[id$=VendorPk]").val() == "-1") {
            $("select[id$=VEH_VENDOR]").val(InitialSupplier);
        }
        else {
            $("select[id$=VEH_VENDOR]").val($("[id$=VendorPk]").val());
            GetSuplliedMaterial($("[id$=VendorPk]").val());
            GetVendorAddress($("[id$=VendorPk]").val());
            $("select[id$=VEH_VENDOR]").attr("disabled", "disabled");
        }
        VND_PK = InitialSupplier;
        //For approving the evaluation
        if (parseInt(VND_PK) > 0) {
            $("select[id$=VEH_VENDOR]").val(VND_PK);
            $("select[id$=VEH_VENDOR]").attr("disabled", "disabled");
            $("#SpnName").html($("select[id$=VEH_VENDOR] OPTION:selected").text())//populate the name field.
            GetSuplliedMaterial(VND_PK);
            GetVendorAddress(VND_PK);
        }
    });
}

//NewEval start
//<summary>function To Fill Parameters Details </summary>VendorId
function FillGroups() {
    // Get id of the Parameters DropDown
    var drpID = $("select[id$=VED_TERM_HDR]").attr("id");
    $.get(VndEval.URLGetEvalGroups + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}
//New End

//NewEval start
////<summary>function To Fill Parameters Details </summary>VendorId
//function FillParameters() {
//    // Get id of the Parameters DropDown
//    var drpID = $("select[id$=VED_PARAM]").attr("id");
//    $.get(VndEval.URLGetParameters + $("[id$=BizUnitPk]").val(), function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, true);
//    });

//}
//<summary>function To Fill Parameters Details </summary>VendorId
function FillParameters(groupId, selectVal) {
    // Get id of the Parameters DropDown
    var drpID = $("select[id$=VED_PARAM]").attr("id");
    $.get(VndEval.URLGetParameters + $("[id$=BizUnitPk]").val() + "&TemplatePK=" + groupId, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        //selectedParam
    });

}

function FillMaxPoint(tmdId, selectVal) {
    // Get id of the Parameters DropDown
    var drpID = $("select[id$=VED_PARAM]").attr("id");
    var TargetDropDown = $("select[id$=VED_POINT]").attr("id");
    $.get(VndEval.URLGetParameterValue + $("[id$=BizUnitPk]").val() + "&TmdPK=" + tmdId, function (data) {
        if (data && data.length != undefined) {
            $("select[id$=VED_POINT]").html("");
            $("select[id$=VED_POINT]").append("<option value='0'>Translate(Select)</option>");
            for (var i = 1; i <= parseInt(data[0].Text); i++) {
                $("select[id$=VED_POINT]").append("<option value='" + i + "'" + "title='" + i + "'" + " >" + i + "</option>");
            }
            $("select[id$=VED_POINT]").val(selectVal);
        }
    });
}

//New End

//NewEval start
function FillEvaluationParameters(groupID, selectedParam) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (groupID != "0") {
        FillParameters(groupID, selectedParam);
    }
    else {
        //ClearEvaluationDtls();
    }
}
//New End
function FillEvaluationPoint(tmdID, selectedParam) {
    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
    // Get id of the Category DropDown
    //<Params>materialID</Params>
    if (tmdID != "0") {
        FillMaxPoint(tmdID, selectedParam);
    }
    else {
        //ClearEvaluationDtls();
    }
}
//function FillEvaluationPoint(groupID, selectedParam) {
//    //<summary>function To Fill Category Details Fill category UOm And Materials</summary>
//    // Get id of the Category DropDown
//    //<Params>materialID</Params>
//    if (groupID != "0") {
//        FillPoints(groupID, selectedParam);
//    }
//    else {
//        //ClearEvaluationDtls();
//    }
//}



//<summary>function To Fill Vendor Details </summary>
function GetVendorAddress(vendorId) {
    // Get id of the Vendor DropDown
    var drpID = $("select[id$=Evaluation]").attr("id");
    $.get(VndEval.URLGetVendorAddress + vendorId,
    function (data) {
        var result = data;
        $('#CodeSupplier').text('');
        $('#SupplierAddress').text('');
        $('#CodeSupplier').text(result[0].Text);
        $('#SupplierAddress').text(result[0].Value);

    });

}
//<summary>function To Fill SuppliedMaterials Details </summary>
function GetSuplliedMaterial(vendorId, initalValue) {
    // Get id of the SuppliedMaterials DropDown
    var drpID = $("select[id$=VEH_ITEM]").attr("id");
    if (vendorId == 0) {
        GrandScriptUtils.FillDropDown(drpID, new Object(), true, true);
    }
    else {
        $.get(VndEval.URLGetSuppliedMaterial + vendorId,
    function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
        //initialize the dropdown with given Initial value
        $("select[id$=VEH_ITEM]").val(InitialProduct);
    });
    }
}
//<summary>function To Fill Evaluation Details </summary>
function FillEvaluation(parameterId, selectval) {
    // Get id of the Evaluation DropDown
    //   var drpID = $("select[id$=Evaluation]").attr("id");

    $.get(VndEval.URLGetEvaluations + parameterId, function (data) {
        if (selectval) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectval);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });
}

///#endregion

///#region---- Data Management Section----

//<summary>function Call Afer binding Grid</summary>
function AfterGridBind(grdID) {
    if (tdset == "") {
        tdset = $("#ProductInsert").find("tr:eq(1)");
    }
    $("#ProductInsert").hide();
    $("#ProductInsert").css({ "display": "none", "visibility": "hidden" });
    $(tdset).insertBefore($("#grdEvalDetails").find("tr:eq(1)"));

    MakeFieldsReadonly();

    if (grdID == $("#grdEvalDetails").attr("id")) {
        //Mode iS view
        if ($("[id$=VEH_STATUS]").val() == "1" || $("[id$=VEH_STATUS]").val() == "7" || $("[id$=VEH_STATUS]").val() == "11") {
            $("#grdEvalDetails").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();
            });
        }
    }
    $("#grdEvalDetails tr:has(td)").each(function () {
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VED_REMARKS", $(this).parents("table:first").attr("id"));
        if (colIndex != null) {
            colData = GrandGrid.Utilities.GetColumnValue($(this), "VED_REMARKS", $(this).parents("table:first").attr("id"));
            if (colData == "null")
                colData = "-";
            $(this).find("td:eq(" + colIndex + ")").html(colData);
        }

        //        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "VED_POINT", $(this).parents("table:first").attr("id"));
        //        if (colIndex != null) {
        //            colData = GrandGrid.Utilities.GetColumnValue($(this), "VED_POINT", $(this).parents("table:first").attr("id"));
        //            $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" value=" + colData + " class=\"numeric input-disabled\"  maxlength=\"11\" tabIndex=\"5\" />");

        //        }

    });

}
///Method to Make readonly
function MakeFieldsReadonly() {

    var WorkflowStatus = $("[id$=VEH_STATUS]").val();

    if (IsReadOnly == 1) {
        //if page is read only
        //NeweVAL START
        //$("[id$=imbEdit],[id$=imbDelete],[id$=imbAddNew],[id$=VED_POINT],[id$=VED_PARAM],[id$=VED_REMARKS],[id$=VEH_VENDOR],[id$=VEH_ITEM],[id$=ActionID],[id$=btnSave],[id$=imbDraft]").attr("disabled", "disabled");
        $("[id$=imbEdit],[id$=imbDelete],[id$=imbAddNew],[id$=VED_POINT],[id$=VED_TERM_HDR],[id$=VED_PARAM],[id$=VED_REMARKS],[id$=VEH_VENDOR],[id$=VEH_ITEM],[id$=ActionID],[id$=btnSave],[id$=imbDraft]").attr("disabled", "disabled");
        //new end
    }
    else if (WorkflowStatus == 1 || WorkflowStatus == 7 || WorkflowStatus == 11) {
        //if evaluation status is "Evaluated" the n next user is only allowed to change the point awarded.All other fields are kept disabled
        $("[id$=imbDelete],[id$=VED_REMARKS],[id$=VEH_VENDOR],[id$=VEH_ITEM],[id$=imbDraft],[id$=imbAddNew]").attr("disabled", true);
        $("[id$=imbDraft]").hide();
        $("[id$=imbAddNew]").hide();
    }
    else {
        $("[id$=imbDelete],[id$=VED_REMARKS],[id$=VEH_VENDOR],[id$=VEH_ITEM],[id$=imbDraft],[id$=imbAddNew]").attr("disabled", false);
        $("[id$=imbDraft]").show();
        $("[id$=imbAddNew]").show();
    }

}
//<summary>function used to add Evaluation details to Evaluation</summary>
function AddEvaluationDetails() {
    //Add Validation for Evaluation Details by setting mode as 2

    AddValidations(2);
    var controlStatus = true;
    if ($(document.forms[0]).valid()) {
        if ($("select[id$=VEH_VENDOR]").is(':disabled') == false) {
            controlStatus = false;
        }
        else {
            controlStatus = true;
        }

        var ObjEvaluation = $("#divEvalData").data("EvalData");
        var editEvaluation = $("input[id$=EditEvaluation]").val();
        var WorkfStatus = $("id$=VEH_STATUS").val();
        var obj = new Object();
        var flag = true;
        //Neweval start
        if ($("select[id$=VED_TERM_HDR]").val() == 0) {
            return false;
        }
        //new end

        if ($("select[id$=VED_PARAM]").val() == 0) {
            return false;
        }

        //Loop used to check the Evaluation already added in the order List 
        if (parseInt(editEvaluation) == 0) {
            for (var i in ObjEvaluation.EvalDetailsList) {
                if (ObjEvaluation.EvalDetailsList[i].VED_PARAM == $("select[id$=VED_PARAM]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjEvaluation.EvalDetailsList) {
                if (ObjEvaluation.EvalDetailsList[i].VED_PARAM == $("select[id$=VED_PARAM]").val() && parseInt(editEvaluation) != ObjEvaluation.EvalDetailsList[i].VED_PARAM) {
                    flag = false;
                    break;
                }
            }
            for (var i in ObjEvaluation.EvalDetailsList) {
                if (parseInt(editEvaluation) == ObjEvaluation.EvalDetailsList[i].VED_PARAM)
                    obj = ObjEvaluation.EvalDetailsList[i];
            }
        }

        if (flag) {
            // obj.EvaluationID = parseInt($("select[id$=Evaluation]").val());
            // obj.Evaluation = $("select[id$=Evaluation] option:selected").text();
            if (obj.VED_PK == null) {
                obj.VED_PK = 0;
            }
            //NewEval start
            obj.VED_TERM_HDR_TEXT = $("select[id$=VED_TERM_HDR] option:selected").text();
            obj.VED_TERM_HDR = parseInt($("select[id$=VED_TERM_HDR]").val());
            //New End
            obj.VED_PARAM = parseInt($("select[id$=VED_PARAM]").val());
            obj.VED_PARAM_NAME = $("select[id$=VED_PARAM] option:selected").text();
            obj.VED_POINT = parseInt($("select[id$=VED_POINT]").val());
            obj.VED_REMARKS = $("[id$=VED_REMARKS]").val();
            if (parseInt(editEvaluation) == 0) {
                obj.SupplierMaterialID = 0;
                ObjEvaluation.EvalDetailsList.push(obj);
            }
            $("#divEvalData").data("EvalData", ObjEvaluation);
            GrandGrid.MakeGrid($("#grdEvalDetails"), 0, ObjEvaluation.EvalDetailsList);
            CalculateResults();
            ClearProductDetails();
            RemoveValidations(2);
        }
        else {
            GrandScriptUtils.ShowModal(VndEval.EvalAlreadyAdded, VndEval.Information);
        }

        if (controlStatus == true) {
            $("[id$=VEH_VENDOR]").attr("disabled", "disabled");
        }
        return false;
    }
}
//<summary>calculate  the results and display</summary>
function CalculateResults() {
    ClearCalculation();
    var ObjEval = $("#divEvalData").data("EvalData");
    var Evaldtl = ObjEval.EvalDetailsList;
    if (Evaldtl.length == 0) {
        return;
    }
    var MaxPoint = 0;
    var TotalPoint = 0;
    var Percentage = 0;
    var EvalDescr = '';
    var Grade = '';
    MaxPoint = Evaldtl.length * 5;
    for (var i in Evaldtl) {
        TotalPoint += parseInt(Evaldtl[i].VED_POINT);
    }
    Percentage = Round((TotalPoint / MaxPoint) * 100, 3);

    $("[id$=VEH_POINT]").val(TotalPoint);
    //  $("[id$=VEH_PERC]").val(Percentage);

    //calculation of grade
    if (Percentage <= 20) {
        EvalDescr = "Bad";
        Grade = "E"
    }
    else if (Percentage <= 40) {
        EvalDescr = "Poor";
        Grade = "D";
    }
    else if (Percentage <= 60) {
        EvalDescr = "Fair";
        Grade = "C"
    }
    else if (Percentage <= 80) {
        EvalDescr = "Good";
        Grade = "B"
    }
    else if (Percentage <= 100) {
        EvalDescr = "Very Good";
        Grade = "A"
    }

    $("[id$=VEH_RAT_DESC]").val(EvalDescr);
    $("[id$=VEH_GRADE]").val(Grade);
    //Display

    $("#MaxPoint").html(MaxPoint);
    $("[id$=VEH_MAX_POINT]").val(MaxPoint);
    $("#PointAwarded").html(TotalPoint);
    $("#Percentage").html(Percentage + "%");
    $("#Rating").html(EvalDescr);
    $("#SpnGrade").html(Grade);
    $("#SpnName").html($("[id$=VEH_VENDOR] :selected").text());
    // var date=new Date()
    // $("#Date").html(date.toDateString());

}

///<summary>Method to round decimal no. to given no. of positions</summary>
/// <param name="x" >
///     Input decimal value
/// </param>
///<param name="y" >
///     No. of decimal points to be restricted
/// </param>
function Round(x, y) {

    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);
}
//Method to clear calculation
function ClearCalculation() {
    $("#MaxPoint").html("-");
    $("#PointAwarded").html("-");
    $("#Percentage").html("-");
    $("#Rating").html("-");

}
///Method to save evaluation
/// <param name="isDraft" >
///     flag indicating whether to save as draft
/// </param>



function SavePage(isDraft) {
    AddValidations(1);
    if ($(document.forms[0]).valid()) {

        EnableControls();

        var ObjDisp = $("#divEvalData").data("EvalData");
        if (ObjDisp.EvalDetailsList.length > 0) {
            ChangeWorkflowStatus(isDraft);
            $("[id$=EvalDetailsList]").val(JSON.stringify(ObjDisp.EvalDetailsList));
            if (isDraft && isDraft != 'Submit') {
                $("[id$=ActionID]").val('0');
            }
            else {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val());
            }

            //To Prevent Duplicate Submission
            if ($("[id$=SubmitFlag]").val() == "0")
                $("[id$=SubmitFlag]").val('1')
            else
                return false;


            var jSonString = GrandScriptUtils.FormToJsonString(false);

            $.ajax({
                type: "post",
                url: VndEval.URLSavePage + $("[id$=VEH_VENDOR]").val() + "&IsDraft=" + isDraft,
                data: jSonString,
                contentType: "application/json",
                dataType: "text",
                success: function (data) {///if data>1  saved successfully

                    if (parseInt(data) > 0) {
                        if (isDraft && isDraft != 'Submit') {
                            $("[id$=EvalDetailsList]").val("0");
                            GrandScriptUtils.ShowModal(VndEval.Savedsuccessfully, VndEval.Information, VndEval.SaveOK, false);
                            // PageInit();
                        }
                        else {
                            $("[id$=hdfAppID]").val(data);
                            SaveWorkFlow(true, 2); // 2 for vendor Evaluation

                        }
                    }
                    else if (parseInt(data) == -2) {
                        GrandScriptUtils.ShowModal(VndEval.VendoEvalDetails + " " + VndEval.EditUsedByAnotherUser, VndEval.MessageBoxTitle, VndEval.SaveOK);
                        $("[id$=SubmitFlag]").val('0');
                    }
                    else {
                        GrandScriptUtils.ShowModal(VndEval.ActionFailed);
                        $("[id$=SubmitFlag]").val('0');
                        //ResetPage();
                    }
                }
            });
        }
        else {
            GrandScriptUtils.ShowModal(VndEval.PleaseEvaltCriteria, VndEval.Information);
        }
    }
    return false;
}
function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    UpdateRefID(2);
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        GrandScriptUtils.ShowModal(VndEval.Submitedsuccessfully, VndEval.Information, VndEval.INBOX, false);
    } else {
        GrandScriptUtils.ShowModal(VndEval.Submitedsuccessfully, VndEval.Information, VndEval.SaveOK, false);
    }

}
function UpdateRefID(type) {

    var wrkfReq = new Object();
    wrkfReq.ApplicationID = parseInt($("[id$=hdfAppID]").val());
    wrkfReq.UserPK = parseInt($("[id$=UserPk]").val());
    wrkfReq.ReferenceID = parseInt($("[id$=ReferenceID]").val());
    var jsonString = JSON.stringify(wrkfReq);
    $.post("CommonManagement.do?Action=UpdateReferenceID&Type=" + type, jsonString, function (data) {
        if (parseInt(data) > 0) {
            // ShowWorkflowSaveMsg();
        }
    });
}
///<summary>Method to set the workflow status
///  0--Draft -Will not proceed in the workflow
/// 1--Evaluated -Evaluation done
/// 2- Approved -Evaluation approved
/// 3- Rejected - Evaluation Rejected
function ChangeWorkflowStatus(isDraft) {
    var WrkfStatus = $("[id$=VEH_STATUS]");
    var Status = $("[id$=ActionID]").val();
    if (WrkfStatus.val() != "0" && isDraft) WrkfStatus.val('');
    else if (isDraft) WrkfStatus.val(0);
    else if (Status == VndEval.EvaluationStatus.Evaluated) WrkfStatus.val(1);
    else if (Status == VndEval.EvaluationStatus.Approved) WrkfStatus.val(2);
    else if (Status == VndEval.EvaluationStatus.Rejected) WrkfStatus.val(3);

}
////<summary>Method to enable the disabled control before serializing
function EnableControls() {
    $("[id$=VEH_VENDOR]").attr("disabled", "");
    $("[id$=VEH_ITEM]").attr("disabled", "");

}
//<summary>function used to Clear Evaluation Details</summary>
function ClearProductDetails() {
    //NewEval start
    $("select[id$=VED_TERM_HDR]").val('0');
    //New End
    $("select[id$=VED_PARAM]").val('0');
    $("input[id$=EditEvaluation]").val('0');
    $("input[id$=VEH_ITEM]").val('0');
    $("select[id$=VED_POINT]").val('0');
    $("[id$=VED_REMARKS]").val('');
    //NewEval start
    //$("select[id$=VED_PARAM]").focus();
    $("select[id$=VED_TERM_HDR]").focus();
    //New End
}

///<summary>Used fill Details of Evaluation for editing</summary>
function FillDetails(tr) {
    //NewEval start
    $("select[id$=VED_TERM_HDR]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_TERM_HDR", $(tr).parent().attr("id")));
    FillEvaluationParameters($("select[id$=VED_TERM_HDR]").val(), GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", $(tr).parent().attr("id")));
    //$("select[id$=VED_PARAM]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", $(tr).parent().attr("id")));
    // $("select[id$=VED_POINT]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_POINT", $(tr).parent().attr("id")));
    //$("select[id$=VED_PARAM]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", $(tr).parent().attr("id")));
    FillEvaluationPoint(GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", $(tr).parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "VED_POINT", $(tr).parent().attr("id")));
    //new end
    $("select[id$=VED_POINT]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_POINT", $(tr).parent().attr("id")));
    $("[id$=VED_REMARKS]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_REMARKS", $(tr).parent().attr("id")));
    $("input[id$=EditEvaluation]").val(GrandGrid.Utilities.GetColumnValue(tr, "VED_PARAM", $(tr).parent().attr("id")));
    $("input[id$=IsEdit]").val("true");
    $("[id$=imbAddNew]").attr("disabled", "");
    //Neweval start
    //$("select[id$=VED_PARAM]").focus();
    $("select[id$=VED_TERM_HDR]").focus();
    //new end
}
///<summary>For delete the item in the grid - Evaluation Details</summary>
function DeleteEvaluationDetails() {

    var ObjDisp = $("#divEvalData").data("EvalData");

    //Delete Row
    for (var i in ObjDisp.EvalDetailsList) {
        if (ObjDisp.EvalDetailsList[i].VED_PARAM == ParamID) {
            //Will delete the Term details
            ObjDisp.EvalDetailsList.splice(i, 1);
            break;
        }
    }

    $("#divEvalData").data("EvalData", ObjDisp);
    CalculateResults();
    //Used to Show the Term  details when the Materials in Dispersion is 0
    if (ObjDisp.EvalDetailsList.length == 0) {
        //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
        $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
        $("#ProductInsert").show();
        $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
        $("#grdEvalDetails").hide();
        $("#grdEvalDetails").css({ "display": "none", "visibility": "hidden" });

    }
    else {
        GrandGrid.MakeGrid($("#grdEvalDetails"), 0, ObjDisp.EvalDetailsList);
    }
}

///#endregion

//#region-----------ValidationSection----------------
//<summary>function used to assign validation</summary>
function AddValidations(mode) {
    RemoveValidations();
    //Mode = 1 represents the validation for Evaluation(Master) Header Details

    //Mode =  2 represents the validation for Evaluation Details
    if (mode == "1") {


        $('select[id$=VEH_VENDOR]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaSupplier)' }
        });

        $('select[id$=VEH_ITEM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaProduct)' }
        });


    }
    if (mode == "2") {

        //newEval start
        $('select[id$=VED_TERM_HDR]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaGroup)' }
        });
        //new end
        $('select[id$=VED_PARAM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaParameter)' }
        });
        $('select[id$=VED_POINT]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaPoints)' }
        });
        //        $('[id$=VED_REMARKS]').rules("add", {
        //            required: true,
        //            messages: { required: 'Translate(PleaseProvideRemarks)' }
        //        });
    }
}
//<summary>function Remove Validation</summary>
function RemoveValidations() {

    //Neweval start
    $('select[id$=VED_TERM_HDR]').rules("remove");
    //new end
    $('select[id$=VED_PARAM]').rules("remove");
    //    $('select[id$=Evaluation]').rules("remove");
    $('select[id$=VED_POINT]').rules("remove");
    $('[id$=VED_REMARKS]').rules("remove");

    $('select[id$=VEH_ITEM]').rules("remove");
    $('select[id$=VEH_VENDOR]').rules("remove");

    $(document.forms[0]).validate().resetForm();


}

function ClearEvaluationDtls() {
    groupID = "0";
    $("#MaxPoint").html("-");
    $("#PointAwarded").html("-");
    $("#Percentage").html("-");
    $("#Rating").html("-");

    if ($("[id$=VEH_VENDOR]").val() != "0") {

        $("#SpnName").html($("[id$=VEH_VENDOR] :selected").text());
    }
    else {

        $("#SpnName").html("-");
    }


    $("#SpnGrade").html("-");
    var ObjDisp = $("#divEvalData").data("EvalData");
    if (ObjDisp.EvalDetailsList.length > 0) {
        //Delete Row
        //        for (var i in ObjDisp.EvalDetailsList.length) {
        //            ObjDisp.EvalDetailsList.splice(i, 1);
        //        }
        ObjDisp.EvalDetailsList = new Array();
        // ObjDisp = new Array();
        $("#divEvalData").data("EvalData", ObjDisp);
        //Used to Show the Term  details when the Materials in Dispersion is 0
        if (ObjDisp.EvalDetailsList.length == 0) {
            //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
            $(tdset).insertAfter($("#ProductInsert").find("tr:eq(0)"));
            $("#ProductInsert").show();
            $("#ProductInsert").css({ "display": "block", "visibility": "visible" });
            $("#grdEvalDetails").hide();
            $("#grdEvalDetails").css({ "display": "none", "visibility": "hidden" });

        }
        else {
            GrandGrid.MakeGrid($("#grdEvalDetails"), 0, ObjDisp.EvalDetailsList);
        }
    }
}

//#endregion
function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    if (selectVal == undefined || selectVal == 0) {
        var drpID = $("select[id$=VEH_COMPANY]").attr("id");
        $.get(VndEval.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
            var selCompany = $("[id$=hdfSelCompany]").val();
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
        });
    }
    else {
        var drpID = $("select[id$=VEH_COMPANY]").attr("id");
        $.get(VndEval.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        });
    }

}