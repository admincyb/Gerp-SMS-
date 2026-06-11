
///#region=============== Global variable Declaration
var SLNO = 0;
var toUnitId = 0;
var frmUnitID = 0;
var CompoundJson = new Object();
var tdset = "";
var totMatQty = 0;
var matQty = 0;
var lastVal = 0;
var compQyt = 0;
var decmlPlace = 5;
///#endregion

///#region=============== Configurations
var CompoundMaster = {

    FillCompoundDtlsURL: "CompoundMaster.do?Action=GetCompoundDetails&PK=",
    FillUomDetailsURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=1&UOMPK=",
    GetMaterialTypeUOMURL: "CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
    GetConersionFactorURL: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    //GetConversionUOMListURL: "CompoundMaster.do?Action=GetConversionUOMList&UOM=",
    GetUOMListURL: "CompoundMaster.do?Action=GetUOMListForItemAndCompound&UOM=",
    GetPolymerListURL: "CompoundMaster.do?Action=GetPolymerList&SBU=",
    FillUOMURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=",
    GetMaterialTypeURL: "CompoundMaster.do?Action=GetMaterialTypeName&SBU=",
    GetItemNameURL: "CompoundMaster.do?Action=GetMaterialName&SBU=",
    SavePage: "CompoundMaster.do?Action=SaveCompoundList",
    GETTEMPLATECHECKLISTURL: "CommonManagement.do?Action=GetTemplateCheckList&checkListPK=0&Active=1&processID=",
    GetCheckListDetailsURL: "CommonManagement.do?Action=GetCheckList&checkListID=",

    // Fields
    ValueZero: "0",
    ValueEmpty: "",
    Save: "Save",
    Delete: "delete",
    Edit: "edit",
    ProcessID: "2", //For Compound

    CannotDelete: "Translate(CannotDelete)",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    DeleteConversion: "DeleteConversion",
    DeleteMaterial: "DeleteMaterial",
    UnitChange: "UnitChange",
    ConfirmationMsgTitle: "Translate(Confirmation)",
    InformationTitle: "Translate(Information)",
    CompoundListingURL: "CompoundListing.aspx",
    SameConversionAlreadyAddedMsg: "Translate(ConversionAlreadyAdded)",
    EnterCode: "Translate(EnterCode)",
    EnterName: "Translate(EnterName)",
    SelectPolymer: "Translate(SelectPolymer)",
    SelectFormulation: "Translate(SelectFormulationType)",
    EnterMaturity: "Translate(EnterMaturityPerion)",
    SelectUOM: "Translate(SelectUOM)",
    EnterUnitQty: "Translate(EnterUnitQuantity)",
    EnterExpPeriod: "Translate(EnterExpiryPeriod)",
    SelectCatg: "Translate(SelectCategory)",
    Selectmaterial: "Translate(SelectMaterial)",
    EnterWetQty: "Translate(EnterWetQuantity)",
    EnterDryPerc: "Translate(EnterDryPerc)",
    EnterConversion: "Translate(EnterConversion)",
    ActionFailed: "Translate(ActionFailedPleaseTryAgain)",
    DefaultAction: "Translate(DefaultActionneedstobeperformed)",
    CodeExists: "Translate(CompoundCodeAlreadyExists)",
    NameExists: "Translate(CompoundNameExists)",
    CompoundSavedSuccessMsg: "Translate(CompoundDetailssavedsuccessfully)",
    EnterMaterial: "Translate(EnterMaterialDetails)",
    MaterialAlreadyAdded: "Translate(MaterialAlreadyAdded)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    QuantityExcceded: "Translate(QuantityExcceddedthanUnitQuantity)",
    UnitQtyMistmatchWithMatrComp: "Translate(UnitQtyMistmatchWithMatrComp)",
    MaturityPeriodUOM: "Translate(SelectMaturityTimeUOM)",
    ExpiryPeriodUOM: "Translate(SelectExpiryTimeUOM)",
    UnitQtyUOM: "Translate(SelectUnitQtyUOM)",
    NoSameCompound: "Translate(Msg_NoSameCompound)",
    CompoundPageResName: "Translate(CompoundPageResName)",
    MaxQtyMessageFor8Numeric3Decimal: "Translate(Max8NumericAND3decimalallowed)",
    MaxQtyMessageFor8Numeric2Decimal: "Translate(Max8NumericAND2decimalallowed)",
    DoUWantToContinueUnitChangeTime: "Items Will be Cleared by Changing Unit. Do You Want to Continue? ",
    DeletedRecord: "Translate(DeletedRecord)",
    COMPOUNDLISTINGPAGE: "CompoundListing.aspx"

};
///#endregion

///#region=============== Initialization Section

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != CompoundMaster.ValueZero);
}, 'Translate(Pleaseselectanoption)');
///<summary>Document read</summary>
$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    CompoundJson = $.parseJSON($("[id$=ConversionList]").val());
    CompoundJson = $.parseJSON($("[id$=CompoundMaterialsList]").val());
    $("#divDatas").data("CompoundData", CompoundJson);

    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    CompoundJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);


    GrandGrid.Utilities.ResetGrid(true, "grdCompoundMaterial");
    CompoundJson.CompoundMaterialsList = new Array();
    GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);


    PageInit();


});
///<summary>Used for initial settings</summary>
function PageInit() {
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("#divAddConversion").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveConversionValidation();
        }
    })
    //    $("[id$=COM_QTY_UOM]").change(function () {

    //        GrandScriptUtils.ShowModal(CompoundMaster.DoUWantToContinueUnitChangeTime, CompoundMaster.ConfirmationMsgTitle, CompoundMaster.DeleteMaterial, true) 

    //        ClearMaterialControls();
    //        // Reset material Details
    //        ResetMaterialDetails();
    //        // To Show Conversion Image
    //        ShowConversionDtls();


    //    }); 



    $('input[id$=COM_QUANTITY]').keyup(function () {
        compQyt = $('input[id$=COM_QUANTITY]').val();
        ClearMaterialControls();
        UpdateMaterialList();

    });

    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "Status")) {
                //Check the query string Name status if status as 1 thn its in view mode
                $("[id$=ViewStatus]").val('1');
                $("[id$=btnAdd]").css("display", "none");
                $("[id$=imbSave]").css("display", "none");

                $("[id$=COM_NAME]").attr("disabled", true);
                $("[id$=COM_TYPE]").attr("disabled", true);
                $("[id$=COM_QUANTITY]").attr("disabled", true);
                $("[id$=COM_EXP_PRD]").attr("disabled", true);
                $("[id$=COM_POLYMER]").attr("disabled", true);
                $("[id$=COM_MAT_PRD]").attr("disabled", true);
                $("[id$=COM_MAT_UOM]").attr("disabled", true);
                $("[id$=COM_EXP_UOM]").attr("disabled", true);

            }
        }
    }
    else {
        $("[id$=ViewStatus]").val('0');
    }
    FillCompound();

    $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
    $("[id$=COM_CODE]").focus();


}


function UnitChange(isValidated) {
    var ObjMaterial = $("#divDatas").data("CompoundData");
    $("[id$=CompoundMaterialsList]").val(JSON.stringify(ObjMaterial.CompoundMaterialsList));
    CompoundJson = $("#divDatas").data("CompoundData");
    if (CompoundJson.CompoundMaterialsList.length > 0) {
        if (!isValidated) {
            lastVal = $("select[id$=COM_QTY_UOM]").val();
            $("select[id$=COM_QTY_UOM]").val($("[id$=hdUnit]").val());
            GrandScriptUtils.ShowModal(CompoundMaster.DoUWantToContinueUnitChangeTime, CompoundMaster.ConfirmationMsgTitle, CompoundMaster.UnitChange, true)
            return false;
        }
        else {
            ClearMaterialControls();
            // Reset material Details
            ResetMaterialDetails();
            // To Show Conversion Image
            ShowConversionDtls();
            $("select[id$=COM_QTY_UOM]").val(lastVal);
            $("[id$=hdUnit]").val(lastVal);
        }
    }
    else {
        ClearMaterialControls();
        // Reset material Details
        ResetMaterialDetails();
        // To Show Conversion Image
        ShowConversionDtls();
    }
    $("[id$=hdUnit]").val($("select[id$=COM_QTY_UOM]").val());
    return false;
}

///<summary>Used for Fill Compound Details </summary>
function FillCompound() {
    var compoundPK = $("input[id$=COM_PK]").val();
    if (compoundPK != CompoundMaster.ValueZero) {
        $.get(CompoundMaster.FillCompoundDtlsURL + compoundPK, function (data) {
            $("#divDatas").data("CompoundData", data);
            FillCompoundDetails();
            ShowConversionDtls();
            // To hide Conversion Details Image Button
            //$("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
        });
    }
    else {
        ShowConversionDtls();
        FillCombo();
    }
}
///<summary>Used for Fill Items Name s to Drop Down </summary>
function FillItem() {

    $("select[id$=CPD_ITEM]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_WET_QTY]").val(CompoundMaster.ValueEmpty);
    $("select[id$=CPD_QTY_UOM]").val(CompoundMaster.ValueZero);
    $("input[id$=ConversionValue]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_DRY_PERC]").val(CompoundMaster.ValueEmpty);
    $("[id$=DRYWEIGHT]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero);
    $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero);
    $("input[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero);
    $("input[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
    $("input[id$=SL_NO]").val(CompoundMaster.ValueZero);
    $("[id$=MaterialUOMType]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_PK]").val(CompoundMaster.ValueZero);
    FillMaterialNames(0, 0);
}

function FillTemplateName(selVal) {
    var drpID = $("select[id$=COM_CHECK_LIST_HDR]").attr("id");
    //Fill Dispersion Details to the Template Name DropDown, Name as Text, PK as Value
    $.get(CompoundMaster.GETTEMPLATECHECKLISTURL + CompoundMaster.ProcessID + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selVal);
    });
}

function ViewCheckListDetails() {
    ///<summary>To handle bind grid </summary> 
    var ajaxUrl = CompoundMaster.GetCheckListDetailsURL + $("[id$=COM_CHECK_LIST_HDR]").val();
    $.getJSON(ajaxUrl, function (data) {
        if (data) {
            FillCheckListDetails(data);
            $("#divCmpdlist").dialog("open");
            $("#divCmpdlist").dialog({ "width": 550 });
        }
    });
    return false;
}

function FillCheckListDetails(CheckLstObj) {
    GrandGrid.Utilities.ResetGrid(true, "grdcompoundDetails");
    if (!($.isArray(CheckLstObj.CheckListDtl))) {
        var objArray = CheckLstObj.CheckListDtl;
        CheckLstObj.CheckListDtl = new Array();
        CheckLstObj.CheckListDtl.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdcompoundDetails"), 0, CheckLstObj.CheckListDtl);
}


///<summary>Calculate Compound Perscentage When Material UOM Change</summary>
function CalculateCompound() {

    if ($("select[id$=CPD_QTY_UOM]").val() != "0") {

        FillConversionFact();
        if ($("[id$=CPD_WET_QTY]").val() != CompoundMaster.ValueZero && $("[id$=CPD_WET_QTY]").val() != CompoundMaster.ValueEmpty) {
            CalculateDryWeight();
        }
        else {
            CalculateWetWeight();
        }
    }
    else {
        // $("input[id$=ConversionValue]").val("0");
        $("[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
        //$("[id$=DRYWEIGHT]").val(CompoundMaster.ValueZero); // label
        $("[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero); // hdfld
        $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // Label
        $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld
    }

}


function CalculateCompoundWithDryPerc() {
    ///<summary>Calculate Compound Perscentage When Material UOM Change</summary>
    if ($("select[id$=CPD_QTY_UOM]").val() != "0") {
        FillConversionFact();
        CalculateWetWeight();
    }
    else {
        // $("input[id$=ConversionValue]").val("0");
        $("[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
        $("[id$=CPD_WET_QTY]").val(CompoundMaster.ValueZero); // label
        $("[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero); // hdfld
        $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // Label
        $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld

    }

}



///#endregion

///#region===============  Conversion Section
function AddConversion() {
    ///<summary>To Opne Conversion PopUp Window</summary>
    RemoveValidations();
    ClearConversionDtls();
    FillUOMDetails();
    if (CompoundJson.ConversionList.length > 0)
        $("#ConversionDiv").show();
    else
        $("#ConversionDiv").hide();
    $("input[id$=btnAddConv]").val(CompoundMaster.Save);
    $("#divAddConversion").dialog("open");
    $("#divAddConversion").dialog({ width: 600, height: 450, resizable: true });
    $("#divAddConversion").css({ "min-width": "300", "margin-top": "25px" });
    $("[id$=UOMTypeFm]").text(" : " + $("select[id$=COM_QTY_UOM] option:selected").text());
    return false;
}

function ShowConversionDtls() {
    ///<summary> Function used To Show Conversion Details</summary>
    if ($("select[id$=COM_QTY_UOM]").val() != "0" && $("select[id$=COM_QTY_UOM]").val() != null) {
        // To hide Conversion Details Image Button
        //$("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
        ResetUOMConversion();
        ResetConversionDetails();
        // FillUOMDetails();
        $("[id$=UOMTypeFm]").text(" : " + $("select[id$=COM_QTY_UOM] option:selected").text());

    }
    else

    //$("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
        $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
    FillMaterialConversionUOM(0);

}

function ResetConversionDetails() {
    //<summary>Function to Reset Conversion Grid Controls Details </summary>
    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    CompoundJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);
}

function FillUOMDetails() {
    //<summary>Function used to Fill UOm Details  </summary>
    var drpIDTo = $("select[id$=UPC_TO_UOM]").attr("id");
    $.get(CompoundMaster.FillUomDetailsURL + $("select[id$=COM_QTY_UOM]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpIDTo, data, true, true);
    });
}

function ResetUOMConversion() {
    //<summary>Function used to Reset Conversion Details  </summary>
    $("select[id$=UPC_TO_UOM]").val(CompoundMaster.ValueZero);
    $("input[id$=UPC_CONV_FACT]").val(CompoundMaster.ValueEmpty);
}

function ClearConversionDtls() {
    //<summary>Function used to Clear Conversion  Details  </summary>
    $("select[id$=UPC_TO_UOM]").val(CompoundMaster.ValueZero);
    $("input[id$=UPC_CONV_FACT]").val(CompoundMaster.ValueEmpty);
    $("input[id$=EditConversion]").val(CompoundMaster.ValueZero);
    frmUnitID = 0;
    toUnitId = 0;
}

function SaveConversionDtls() {
    //<summary>Function used to Save Conversion Details  </summary>
    //RemoveValidations(1);
    //RemoveValidations(2);
    AddConversionValidation();
    if ($(document.forms[0]).valid()) {
        CompoundJson = $("#divDatas").data("CompoundData");
        var editConversion = $("input[id$=EditConversion]").val();
        var obj = new Object();
        var flag = true;
        if (parseInt(toUnitId) == 0) {
            for (var i in CompoundJson.ConversionList) {
                if (CompoundJson.ConversionList[i].UPC_TO_UOM == $("select[id$=UPC_TO_UOM]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            // Check Conversion Already Added - For Updation
            for (var i in CompoundJson.ConversionList) {
                if (CompoundJson.ConversionList[i].UPC_PK != editConversion || CompoundJson.ConversionList[i].UPC_TO_UOM != toUnitId) {
                    if (CompoundJson.ConversionList[i].UPC_TO_UOM == $("select[id$=UPC_TO_UOM]").val()) {
                        flag = false;
                        break;
                    }
                }
            }
            // Check Details Is new Entry Or to update . If Update Get Details From CompoundJson and Assign to Obj, and Details Will Updated To Object
            for (var i in CompoundJson.ConversionList) {

                if (parseInt(toUnitId) == CompoundJson.ConversionList[i].UPC_TO_UOM)

                    obj = CompoundJson.ConversionList[i];
            }

        }

        // Check Already Added Or Not
        if (flag) {
            // Add One By one Details To Object
            obj.UMC_UOM_TYPE = 1; // Weight
            obj.UPC_FROM_UOM = $("[id$=COM_QTY_UOM]").val();
            obj.FROM_UOM_NAME = $("select[id$=COM_QTY_UOM] :selected").text();
            obj.UPC_TO_UOM = parseInt($("select[id$=UPC_TO_UOM]").val());
            obj.TO_UOM_NAME = $("select[id$=UPC_TO_UOM] :selected").text();
            obj.UPC_CONV_FACT = $("input[id$=UPC_CONV_FACT]").val();
            // Check Add Details - For New Entry
            if (parseInt(toUnitId) == 0) {
                // If Yes - get Length of the List and Assign Length+1 as the PK of New Entry
                obj.UPC_PK = 0; //  CompoundJson.ConversionList.length + 1;
                //Push Object to List
                CompoundJson.ConversionList.push(obj);
            }
            ClearConversionDtls();
            // Add Details To DivDatas
            $("#divDatas").data("CompoundData", CompoundJson);
            // Bind Conversion Details Grid
            GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);
            if (CompoundJson.ConversionList.length > 0)
                $("#ConversionDiv").show();

            else
                $("#ConversionDiv").hide();

        }
        else {

            GrandScriptUtils.ShowModal(CompoundMaster.SameConversionAlreadyAddedMsg, CompoundMaster.InformationTitle);
        }
        return false;
    }


}

///<summary>Grid Handler Catch all the grid events in this function  - For Conversion Details</summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function ConversionGridHandler(tr, command) {
    // Remove Validations
    switch (command.toString().toLowerCase()) {
        case CompoundMaster.Delete:
            //conversionID = GrandGrid.Utilities.GetColumnValue(tr, "UPC_PK", "grdConversionDtls");
            toUnitId = GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", "grdConversionDtls");
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(CompoundMaster.DoUWantToDelMsg, CompoundMaster.ConfirmationMsgTitle, CompoundMaster.DeleteConversion, true);
            //DeleteConversionDetails();
            return false;
            break;
        case CompoundMaster.Edit:
            FillConversionDetails(tr);
            return false;
            break;
        default:
            GrandScriptUtils.ShowModal(UOMMaster.DEFAULTACTIONMSG, UOMMaster.INFORMATIONTITLE);
            return false;
            break;
    }
}

function FillConversionDetails(tr) {
    //<summary>Function used to Fill Conversion Details When Edit </summary>
    ClearConversionDtls();
    $("select[id$=UPC_TO_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=UPC_CONV_FACT]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_CONV_FACT", $(tr).parents("table:eq(0)").attr("id")));
    $("input[id$=EditConversion]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_PK", $(tr).parent().parent().attr("id")));
    toUnitId = GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", $(tr).parents("table:eq(0)").attr("id"));
}

///<summary>For delete the item in the grid - Conversion Details List</summary>
function DeleteConversionDetails() {
    var CompoundJson = $("#divDatas").data("CompoundData");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in CompoundJson.ConversionList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (CompoundJson.ConversionList[i].UPC_TO_UOM == toUnitId) {
            // Splice Details From List, Corresponding ToUnit
            CompoundJson.ConversionList.splice(i, 1);
            break;
        }
    }

    $("#divDatas").data("CompoundData", CompoundJson);
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);
    if (CompoundJson.ConversionList.length > 0)
        $("#ConversionDiv").show();

    else
        $("#ConversionDiv").hide();
    ClearConversionDtls();

    return false;

}

///#endregion

///#region=============== material Details

//<summary>Function used to Clear Controls </summary>
function ClearMaterialControls() {
    //FillMaterialType(0);
    $("select[id$=CPD_ITEM_CATEGORY]").val(CompoundMaster.ValueZero);
    $("select[id$=CPD_ITEM]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_WET_QTY]").val(CompoundMaster.ValueEmpty);
    $("select[id$=CPD_QTY_UOM]").val(CompoundMaster.ValueZero);
    $("input[id$=ConversionValue]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_DRY_PERC]").val(CompoundMaster.ValueEmpty);
    $("[id$=DRYWEIGHT]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero);
    $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero);
    $("input[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero);
    $("input[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
    $("input[id$=SL_NO]").val(CompoundMaster.ValueZero);
    $("[id$=MaterialUOMType]").val(CompoundMaster.ValueZero);
    $("input[id$=CPD_PK]").val(CompoundMaster.ValueZero);
    var drpID = $("select[id$=CPD_ITEM]").attr("id");
    GrandScriptUtils.FillDropDown(drpID, null, true, true);
    //FillMaterialNames(0);

}

function ResetMaterialDetails() {
    //<summary>Function used to Reset Material Grid  </summary>
    GrandGrid.Utilities.ResetGrid(true, "grdCompoundMaterial");
    CompoundJson.CompoundMaterialsList = new Array();
    GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);

    if (CompoundJson.CompoundMaterialsList.length == 0) {
        //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
        $(tdset).insertAfter($("#MaterialControls").find("tr:eq(0)"));
        $("#MaterialControls").show();
        $("#MaterialControls").css({ "display": "inline", "visibility": "visible" });
    }
    ClearMaterialControls();
}


function AddDetails() {
    if ($(document.forms[0]).valid()) {
        if (CheckQuantityNotExceeded()) {
            CompoundJson = $("#divDatas").data("CompoundData");
            var slNO = $("input[id$=SL_NO]").val();
            //var editConversion = "0";
            var obj = new Object();
            var flag = true;
            if ($("[id$=SL_NO]").val() == "0") {
                for (var i in CompoundJson.CompoundMaterialsList) {
                    if ((CompoundJson.CompoundMaterialsList[i].CPD_ITEM_CATEGORY == $("select[id$=CPD_ITEM_CATEGORY]").val()) && (CompoundJson.CompoundMaterialsList[i].CPD_ITEM == $("select[id$=CPD_ITEM]").val())) {
                        flag = false;
                        break;
                    }
                }
            }
            else {
                for (var i in CompoundJson.CompoundMaterialsList) {
                    if (CompoundJson.CompoundMaterialsList[i].SL_NO != slNO || CompoundJson.CompoundMaterialsList[i].CPD_PK != $("inuput[id$=CPD_PK]").val()) {
                        if ((CompoundJson.CompoundMaterialsList[i].UPC_TO_UOM == $("select[id$=CPD_ITEM_CATEGORY]").val()) && (CompoundJson.CompoundMaterialsList[i].CPD_ITEM == $("select[id$=CPD_ITEM]").val())) {
                            flag = false;
                            break;
                        }
                    }
                }
                for (var i in CompoundJson.CompoundMaterialsList) {

                    if (parseInt(slNO) == CompoundJson.CompoundMaterialsList[i].SL_NO)

                        obj = CompoundJson.CompoundMaterialsList[i];
                }

            }
            // Check Already Added Or Not
            if (flag) {
                // Add One By one Details To Object
                obj.CPD_ITEM_CATEGORY = $("select[id$=CPD_ITEM_CATEGORY]").val();
                obj.MATERIALTYPENAME = $("select[id$=CPD_ITEM_CATEGORY] :selected").text();
                obj.CPD_ITEM = $("select[id$=CPD_ITEM]").val();
                obj.MaterialName = $("select[id$=CPD_ITEM] :selected").text();
                obj.CPD_WET_QTY = $("[id$=CPD_WET_QTY]").val();
                obj.CPD_QTY_UOM = $("select[id$=CPD_QTY_UOM]").val();
                obj.MATERIALQUANTITYUOM = $("[id$=CPD_WET_QTY]").val() + " ( " + $("select[id$=CPD_QTY_UOM] :selected").text() + " )";
                obj.CPD_WET_MAT_QTY = $("[id$=CPD_WET_QTY]").val();
                obj.QTY_UOM_CODE = $("select[id$=CPD_QTY_UOM] :selected").text();
                obj.CPD_DRY_PERC = parseFloat($("[id$=CPD_DRY_PERC]").val());
                obj.DRYWEIGHT = $("[id$=DRYWEIGHT]").val();
                obj.CPD_DRY_QTY = $("[id$=DRYWEIGHT]").val();
                obj.CompoundPercentage = $("[id$=CompoundPercentage]").html()
                obj.CPD_COMP_PERC = parseFloat($("[id$=CPD_COMP_PERC]").val());
                obj.ConversionValue = $("[id$=ConversionValue]").val();
                obj.ConvertedWt = $("[id$=ConvertedWt]").val();
                obj.MaterialUOMType = $("[id$=MaterialUOMType]").val();
                // Check Add Details - For New Entry
                if (parseInt(slNO) == 0) {

                    obj.CPD_PK = 0;
                    obj.SL_NO = CompoundJson.CompoundMaterialsList.length + 1;
                    CompoundJson.CompoundMaterialsList.push(obj);
                }

                $("#divDatas").data("CompoundData", CompoundJson);
                GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
                ClearMaterialControls();
                RemoveValidations(2);
            }
            else {

                GrandScriptUtils.ShowModal(CompoundMaster.MaterialAlreadyAdded, CompoundMaster.InformationTitle);
            }

        }
        else {
            GrandScriptUtils.ShowModal(CompoundMaster.QuantityExcceded, CompoundMaster.InformationTitle);
        }
        return false;
    }
}
function AddMaterialDetails() {
    //<summary>Function used to Add Material Details  </summary>
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("=")
    }
    if ($("select[id$=CPD_ITEM]").val() != queryStr[1]) {
        AddValidations(1);
        AddValidations(2);
        $.get(CompoundMaster.GetMaterialTypeURL + $("[id$=BizUnitPk]").val(), function (data) {
            AddDetails();
        });
    }
    else {
        GrandScriptUtils.ShowModal(CompoundMaster.NoSameCompound, CompoundMaster.InformationTitle);
    }
    return false;
}

function FillMaterialDetails(tr) {
    //<summary>Function used to Fill Mateiral Details When Edit  </summary>
    ClearMaterialControls();
    $("select[id$=CPD_ITEM_CATEGORY]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    //FillMaterialType(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    //FillMaterialConversionUOM(GrandGrid.Utilities.GetColumnValue(tr, "CPD_QTY_UOM", $(tr).parent().parent().attr("id")));
    FillMaterialNames(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM", $(tr).parent().parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "CPD_QTY_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_WET_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_WET_QTY", $(tr).parent().parent().attr("id")));
    $("input[id$=ConversionValue]").val(GrandGrid.Utilities.GetColumnValue(tr, "ConversionValue", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_DRY_PERC]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_DRY_PERC", $(tr).parent().parent().attr("id")));
    $("[id$=DRYWEIGHT]").val(GrandGrid.Utilities.GetColumnValue(tr, "DRYWEIGHT", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_DRY_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_DRY_QTY", $(tr).parent().parent().attr("id")));
    $("[id$=CompoundPercentage]").html(GrandGrid.Utilities.GetColumnValue(tr, "CompoundPercentage", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_COMP_PERC]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_COMP_PERC", $(tr).parent().parent().attr("id")));
    $("input[id$=ConvertedWt]").val(GrandGrid.Utilities.GetColumnValue(tr, "ConvertedWt", $(tr).parent().parent().attr("id")));
    $("input[id$=SL_NO]").val(GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", $(tr).parent().parent().attr("id")));
    $("[id$=MaterialUOMType]").val(GrandGrid.Utilities.GetColumnValue(tr, "MaterialUOMType", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_PK", $(tr).parent().parent().attr("id")));
}

function DeleteMaterialDetails() {
    //<summary>Function used to Delete Material Details  </summary>
    var CompoundJson = $("#divDatas").data("CompoundData");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in CompoundJson.CompoundMaterialsList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (CompoundJson.CompoundMaterialsList[i].SL_NO == SLNO) {
            // Splice Details From List, Corresponding ToUnit
            CompoundJson.CompoundMaterialsList.splice(i, 1);
            break;
        }
    }

    $("#divDatas").data("CompoundData", CompoundJson);
    GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
    ClearMaterialControls();
    if (CompoundJson.CompoundMaterialsList.length == 0) {
        //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
        $(tdset).insertAfter($("#MaterialControls").find("tr:eq(0)"));
        $("#MaterialControls").show();
        $("#MaterialControls").css({ "display": "inline", "visibility": "visible" });
    }

    return false;

}

function GridHandler(tr, command) {
    //<summary>Function used to Do Action In Grid    </summary>
    switch (command.toString().toLowerCase()) {
        case CompoundMaster.Delete:
            SLNO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdCompoundMaterial");
            GrandScriptUtils.ShowModal(CompoundMaster.DoUWantToDelMsg, CompoundMaster.ConfirmationMsgTitle, CompoundMaster.DeleteMaterial, true)
            return false;
            break;
        case CompoundMaster.Edit:
            FillMaterialDetails(tr);
            return false;
            break;
        default:
            GrandScriptUtils.ShowModal("Default Action", CompoundMaster.InformationTitle);
            return false;
            break;
    }
}
///#endregion 

///#region===============  Core Section 
///#region=============== Fill Combo

function FillCombo() {
    //<summary>Function used to Fill All Combos in a Page  </summary>
    FillPolymer(0);
    FillMaterialNames(0, 0);
    // Fill Weight Type
    FillUOM(0, 1, $("select[id$=COM_QTY_UOM]").attr("id"), "Weight");
    // Fill Time Type
    FillUOM(0, 3, $("select[id$=COM_MAT_UOM]").attr("id"), "Time");
    FillUOM(0, 3, $("select[id$=COM_EXP_UOM]").attr("id"), "Time");
    FormulationType(0);
    FillTemplateName(0);
}

function FillConversionFact() {
    //<summary>Function used to Fill Conversion Factor  </summary>
    var uomPK = $("select[id$=CPD_QTY_UOM]").val();
    if (uomPK > 0 && uomPK != undefined && uomPK != null) {
        GetConversionFact();

    }
    else {
        $("input[id$=ConversionValue]").val(CompoundMaster.ValueZero);
    }

}

function GetMaterialUOMType() {
    //<summary>Function used to Fill UOm Type of a selected material </summary>
    if ($("select[id$=CPD_ITEM_CATEGORY]").val() == "1") {
        $.get(CompoundMaster.GetMaterialTypeUOMURL + $("select[id$=CPD_ITEM]").val(), function (data) {
            if (data != " " && data != "-1" && data != "0") {
                $("input[id$=MaterialUOMType]").val(data);
            }
            else {
                $("input[id$=MaterialUOMType]").val(CompoundMaster.ValueZero);
            }
            if ($("input[id$=MaterialUOMType]").val() == "1") {
                FillMaterialConversionUOM($("select[id$=COM_QTY_UOM]").val());
            }
            else {
                var drpID = $("select[id$=CPD_QTY_UOM]").attr("id");
                GrandScriptUtils.FillDropDown(drpID, null, true, true);
            }
        });
    }
    else {
        $("input[id$=MaterialUOMType]").val("1");
        FillMaterialConversionUOM($("select[id$=COM_QTY_UOM]").val());
    }


}

function GetConversionFact() {
    //<summary>Function used to Get Conversion Factor  </summary>
    $.get(CompoundMaster.GetConersionFactorURL + $("select[id$=COM_QTY_UOM]").val() + "&UOMTo=" + $("select[id$=CPD_QTY_UOM]").val(), function (data) {
        if (data != " " && data != "-1") {
            $("input[id$=ConversionValue]").val(data);
            CalculatecaonversionFact();
            GetCompoundPerc();
        }
        else {
            $("input[id$=ConversionValue]").val("0");
        }
    });

}

function FillMaterialConversionUOM(selValue) {
    //<summary>Function used to Fill UOm Details With Selected UOM have COnversion Factor  </summary>
    var drpID = $("select[id$=CPD_QTY_UOM]").attr("id");
    //    $.get(CompoundMaster. GetConversionUOMListURL + $("select[id$=COM_QTY_UOM]").val(), function (data) {
    //        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    //    });

    $.get(CompoundMaster.GetUOMListURL + $("select[id$=COM_QTY_UOM]").val() + "&ItmPK=" + $("select[id$=CPD_ITEM]").val() + "&Catg=" + $("select[id$=CPD_ITEM_CATEGORY]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });

}

function FillPolymer(selValue) {
    //<summary>Function used to Fill Polymer Type to DropDown  </summary>
    var drpID = $("select[id$=COM_POLYMER]").attr("id");
    $.get(CompoundMaster.GetPolymerListURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FillUOM(selValue, uOMType, drpID, uomTypeName) {
    //<summary>Function used to Fill UOM Type to DropDown  </summary>
    $.get(CompoundMaster.FillUOMURL + uOMType + "&SBU=" + $("[id$=BizUnitPk]").val() + "&UOMTypeName=" + uomTypeName, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FormulationType(selValue) {
    //<summary>Function used to Fill Formaulation Type to DropDown  </summary>
    var drpID = $("select[id$=COM_TYPE]").attr("id");
    $.get("CompoundMaster.do?Action=GetFormulationType&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FillMaterialType(selValue) {
    //<summary>Function used to Fill Material Type Type to DropDown  </summary>
    var drpID = $("select[id$=CPD_ITEM_CATEGORY]").attr("id");
    $.get(CompoundMaster.GetMaterialTypeURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FillMaterialNames(selValue, uomPk) {
    //<summary>Function used to Fill Material Name to DropDown  </summary>
    var drpID = $("select[id$=CPD_ITEM]").attr("id");
    $.getJSON(CompoundMaster.GetItemNameURL + $("[id$=BizUnitPk]").val() + "&CATG=" + $("select[id$=CPD_ITEM_CATEGORY]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        FillMaterialConversionUOM(uomPk);
    });
}

///#endregion

function SavePage() {
    //<summary>Function used to Save Compound Details  </summary>
    //Add Validation for Order Details by setting mode as 1
    RemoveValidations(2);
    AddValidations(1);
    var ObjMaterial = $("#divDatas").data("CompoundData");
    $("[id$=CompoundMaterialsList]").val(JSON.stringify(ObjMaterial.CompoundMaterialsList));
    CompoundJson = $("#divDatas").data("CompoundData");
    // Check Have The Order List have More than or equal to one Product Details    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    if ($(document.forms[0]).valid()) {
        if (CompoundJson.CompoundMaterialsList.length > 0) {
            enableUiElementBeforeSave();
            //            $("select[id$=SBU]").attr("disabled", false);
            if (CheckQuantityMatch()) {
                //Assginging the Machine details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=ConversionList]").val(JSON.stringify(CompoundJson.ConversionList));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                $.post(CompoundMaster.SavePage, jSonString, function (data) {
                    // Check Machine Details Saved Successfuully or not
                    if (parseInt(data) > 0) {
                        GrandScriptUtils.ShowModal(CompoundMaster.CompoundSavedSuccessMsg, CompoundMaster.InformationTitle, CompoundMaster.Save);
                    }
                    else {
                        var msgtxt;
                        if (parseInt(data) == 0)
                            msgtxt = CompoundMaster.CodeExists;
                        else if (parseInt(data) == -6)
                            msgtxt = CompoundMaster.NameExists;
                        else if (parseInt(data) == -5)
                            msgtxt = CompoundMaster.DeletedRecord;
                        else if (parseInt(data) == -2)
                            msgtxt = CompoundMaster.CompoundPageResName + CompoundMaster.EditUsedByAnotherUser;
                        else if (parseInt(data) < 0)
                            msgtxt = CompoundMaster.ActionFailed;
                        GrandScriptUtils.ShowModal(msgtxt, CompoundMaster.InformationTitle);
                    }
                });
            }

            else {
                GrandScriptUtils.ShowModal(CompoundMaster.UnitQtyMistmatchWithMatrComp, CompoundMaster.InformationTitle);
            }
        }
        else {
            GrandScriptUtils.ShowModal(CompoundMaster.EnterMaterial, CompoundMaster.InformationTitle);
        }
    }
    return false;
}

function enableUiElementBeforeSave() {
    $("[id$=COM_CODE]").attr("disabled", false);
    $("[id$=COM_NAME]").attr("disabled", false);
    $("select[id$=COM_TYPE]").attr("disabled", false);
    $("select[id$=COM_POLYMER]").attr("disabled", false);
    $("select[id$=COM_EXP_UOM]").attr("disabled", false);
    $("select[id$=COM_MAT_UOM]").attr("disabled", false);
    $("select[id$=COM_QTY_UOM]").attr("disabled", false);
}

// To Fill Details in pgae load
function FillCompoundDetails() {
    //<summary>Function used to Fill Compound Details Tp Controls  </summary>
    CompoundJson = $("#divDatas").data("CompoundData");
    //    $("select[id$=SBU]").val(CompoundJson.COM_BIZUNIT);
    //    $("[id$=BizUnitPk]").val(CompoundJson.COM_BIZUNIT);
    FillPolymer(CompoundJson.COM_POLYMER);
    FillMaterialNames(0, 0);
    FillUOM(CompoundJson.COM_QTY_UOM, 1, $("select[id$=COM_QTY_UOM]").attr("id"), "Weight");
    FillTemplateName(CompoundJson.COM_CHECK_LIST_HDR);
    // Fill Time Type                   
    FillUOM(CompoundJson.COM_MAT_UOM, 3, $("select[id$=COM_MAT_UOM]").attr("id"), "Time");
    FillUOM(CompoundJson.COM_EXP_UOM, 3, $("select[id$=COM_EXP_UOM]").attr("id"), "Time");
    FormulationType(CompoundJson.COM_TYPE);
    $("input[id$=COM_PK]").val(CompoundJson.COM_PK);
    $("input[id$=COM_NAME]").val(CompoundJson.COM_NAME);
    $("input[id$=COM_CODE]").val(CompoundJson.COM_CODE);

    $("input[id$=COM_QUANTITY]").val(CompoundJson.COM_QUANTITY);
    $("input[id$=COM_EXP_PRD]").val(CompoundJson.COM_EXP_PRD);
    $("input[id$=COM_MAT_PRD]").val(CompoundJson.COM_MAT_PRD);

    //$("Select[id$=COM_TYPE]").val(CompoundJson.COM_TYPE);
    $("[id$=LAST_MOD_DT]").val(CompoundJson.COM_MOD_DT);

    if (!($.isArray(CompoundJson.CompoundMaterialsList))) {
        var objArray;
        if (CompoundJson.CompoundMaterialsList != undefined) {
            objArray = CompoundJson.CompoundMaterialsList;
            CompoundJson.CompoundMaterialsList = new Array();
            CompoundJson.CompoundMaterialsList.push(objArray);
            GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
        }
        else {
            objArray = CompoundJson.CompoundMaterialsList;
            CompoundJson.CompoundMaterialsList = new Array();
        }
    }
    else {

        GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
    }
    if (!($.isArray(CompoundJson.ConversionList))) {
        var objArray;
        if (CompoundJson.ConversionList != undefined) {
            objArray = CompoundJson.ConversionList;
            CompoundJson.ConversionList = new Array();
            CompoundJson.ConversionList.push(objArray);
            GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);
        }
        else {
            objArray = CompoundJson.ConversionList;
            CompoundJson.ConversionList = new Array();
        }
    }
    else {

        GrandGrid.MakeGrid($("#grdConversionDtls"), 0, CompoundJson.ConversionList);
    }
    $("[id$=COM_CODE]").attr("disabled", true);
    $("select[id$=COM_QTY_UOM]").attr("disabled", true);
    //    $("select[id$=SBU]").attr("disabled", true);
}

function UpdateMaterialList() {
    //<summary>Function used to Update material list when change the Qunatity </summary>
    if (CompoundJson.CompoundMaterialsList.length > 0) {
        var totQty = $('input[id$=COM_QUANTITY]').val();
        var totQtyList = parseFloat(GetTotalQtyFromList());
        if (totQty >= totQtyList) {
            for (var i in CompoundJson.CompoundMaterialsList) {
                var qty = CompoundJson.CompoundMaterialsList[i].ConvertedWt;
                var perc = 0;
                if (qty && totQty) {
                    perc = (qty / totQty) * 100;
                }
                if (isNaN(perc) || Infinity == perc || perc == 0) {
                    CompoundJson.CompoundMaterialsList[i].CompoundPercentage = 0;
                    CompoundJson.CompoundMaterialsList[i].CPD_COMP_PERC = 0;

                }
                else {
                    perc = Round(perc, 3);
                    CompoundJson.CompoundMaterialsList[i].CompoundPercentage = perc;
                    CompoundJson.CompoundMaterialsList[i].CPD_COMP_PERC = perc;
                }
            }
            $("#divDatas").data("CompoundData", CompoundJson);
            GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
        }

        else {
            ResetGridValueVariation();

        }
    }

}

function ResetGridValueVariation() {
    //<summary>Function used to Reset value in Entered Compound Qunatity</summary>
    var totQtyList = parseFloat(GetTotalQtyFromList());
    $('input[id$=COM_QUANTITY]').val(totQtyList);
}

function Round(x, y) {
    //<summary>Function used to Calculate Round Value</summary>
    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);

}

///#endregion

///#region===============  Utilities
//<summary>Function used to Filter For Numeric values </summary>
function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}
//<summary>Function used to do Action after POPUP OK Button Event</summary>
function ModalOk(command) {
    switch (command) {
        case CompoundMaster.DeleteConversion:
            DeleteConversionDetails();
            break;
        case CompoundMaster.DeleteMaterial:
            DeleteMaterialDetails();
            break;
        case CompoundMaster.UnitChange:
            UnitChange(true);
            break;
        case CompoundMaster.Save:
            window.location = CompoundMaster.CompoundListingURL;
    }
    return false;
}
//<summary>Function used to rebind grid- </summary>
function AfterGridBind(grdID) {
    if (grdID == "grdCompoundMaterial") {
        if (tdset == "") {
            tdset = $("#MaterialControls").find("tr:eq(1)");
        }
        $("#MaterialControls").hide();
        $("#MaterialControls").css({ "display": "none", "visibility": "hidden" });
        $("#grdCompoundMaterial").show();


        if ($("[id$=ViewStatus]").val() == "1") {
            //Hiding the template field (Action section)
            $("#grdCompoundMaterial").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();
            });

        }
        else {
            $(tdset).insertBefore($("#grdCompoundMaterial").find("tr:eq(1)"));
        }
        $("#grdCompoundMaterial").find("tr:has(th)").each(function (index) {
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "CPD_WET_MAT_QTY", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "CPD_DRY_PERC", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "DRYWEIGHT", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "CompoundPercentage", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }  
        });
    }

    if (grdID == "grdcompoundDetails") {
        var ColIndex = 0;
        var Col = 0;
        $("#grdcompoundDetails tr:has(td)").each(function (index) {
            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_VALUE", grdID);
            Col = GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID);
            if (ColIndex != null && Col != "null") {
                $(this).find("td:eq(" + ColIndex + ")").html(Col);
            }
            else {
                $(this).find("td:eq(" + ColIndex + ")").html("");
            }
            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_DESC", grdID);
            Col = GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID);
            if (ColIndex != null && Col != "null") {
                $(this).find("td:eq(" + ColIndex + ")").html(Col);
            }
            else {
                $(this).find("td:eq(" + ColIndex + ")").html("");
            }
        });
    }
}
///#endregion 

///#region===============  Calculations

//<summary>Function used to Calculate Conversion Factor</summary>
function CalculatecaonversionFact() {
    // Updations Required
    if ($("input[id$=ConversionValue]").val() != CompoundMaster.ValueZero && $("input[id$=ConversionValue]").val() != CompoundMaster.ValueEmpty) {
        var qty = 0;
        var matQty = parseFloat($('input[id$=CPD_WET_QTY]').val());
        var convFact = parseFloat($("input[id$=ConversionValue]").val());
        if (matQty && convFact) {
            qty = matQty / convFact;

        }
        if (isNaN(qty) || Infinity == qty || qty == 0) {
            $("[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
        }
        else {
            qty = Round(qty, decmlPlace);
            $("[id$=ConvertedWt]").val(qty);
        }

    }

}
//<summary>Function used to  Calculate Calculate Quantity exceed or not</summary>
function CheckQuantityNotExceeded() {
    var totQtyList = parseFloat(GetTotalQtyFromList());
    var enterQty = parseFloat($("[id$=ConvertedWt]").val());
    if (parseFloat($("input[id$=COM_QUANTITY]").val()) >= totQtyList + enterQty) {
        return true;
    }
    else {
        return false;
    }
}

function CheckQuantityMatch() {
    //<summary>Function used to  Check Header Compound Quanatity == Total Qunatity of item in List</summary>
    var totQtyList = parseFloat(GetTotalQtyFromList());
    if (parseFloat($("input[id$=COM_QUANTITY]").val()) == totQtyList) {
        return true;
    }
    else
        return false;
}

function CalculateDryWeight() {
    //<summary>Function used to  Calculate Dry Weight Formulae - WetWeight * DryWeight%</summary>
    if ($('select[id$=CPD_WET_QTY]').val() != "0") {
        var matQty = $('input[id$=CPD_WET_QTY]').val();
        var dryPer = $('input[id$=CPD_DRY_PERC]').val();
        var dryQty = 0;
        if (matQty && dryPer) {
            dryQty = matQty * (dryPer / 100);
        }
        if (isNaN(dryQty) || Infinity == dryQty) {
            $("[id$=DRYWEIGHT]").val(CompoundMaster.ValueZero); // label

        }
        else {
            dryQty = Round(dryQty, 2);

            $("[id$=DRYWEIGHT]").val(dryQty);

        }
    }
    else {
        $("[id$=DRYWEIGHT]").val(CompoundMaster.ValueZero); // label

    }
}



function CalculateWetWeight() {
    //<summary>Function used to  Calculate Dry Weight Formulae - WetWeight * DryWeight%</summary>
    if ($('select[id$=CPD_WET_QTY]').val() != "0") {
        var matQty = $('input[id$=DRYWEIGHT]').val();
        var dryPer = $('input[id$=CPD_DRY_PERC]').val();
        var dryQty = 0;
        if (matQty && dryPer) {
            dryQty = (matQty / dryPer) * 100;
        }
        if (isNaN(dryQty) || Infinity == dryQty || dryQty == 0) {
            $("[id$=CPD_WET_QTY]").val(CompoundMaster.ValueZero); // label

        }
        else {
            dryQty = Round(dryQty, 2);

            $("[id$=CPD_WET_QTY]").val(dryQty);

        }
    }
    else {
        $("[id$=CPD_WET_QTY]").val(CompoundMaster.ValueZero); // label

    }
}



function GetTotalQtyFromList() {
    //<summary>Function used to  Check Total Qunatity of Entered Items from List</summary>
    var totQty = 0;
    for (var i in CompoundJson.CompoundMaterialsList) {
        if (CompoundJson.CompoundMaterialsList[i].SL_NO != $("input[id$=SL_NO]").val()) {
            totQty += parseFloat(CompoundJson.CompoundMaterialsList[i].ConvertedWt);
        }
    }
    return totQty;
}

function GetCompoundPerc() {
    //<summary>Function used to  Calculate Compound Percentage</summary>
    var qty = $("[id$=ConvertedWt]").val();
    var totQty = $("[id$=COM_QUANTITY]").val();
    var perc = 0;
    //var qtyFrmlist = GetTotalQtyFromList();
    if (qty && totQty) {
        perc = (qty / totQty) * 100;
    }
    if (isNaN(perc) || Infinity == perc || perc == 0) {
        $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // label
        $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld
    }
    else {
        perc = Round(perc, 3);
        $("[id$=CompoundPercentage]").html(perc); // label
        $("[id$=CPD_COMP_PERC]").val(perc); // hdfld
        //CalculatecaonversionFact();
    }
}
///#endregion  

///#region=============== Validation
function AddValidations(mode) {
    //<summary>Function used to  Add Validation To Compound Details 1 for Header Details 2 for Items Entry</summary>
    if (mode == 1) {
        $("input[id$=COM_CODE]").rules("add", {
            required: true,
            maxlength: 95,
            messages: { required: CompoundMaster.EnterCode }
        });
        $("input[id$=COM_NAME]").rules("add", {
            required: true,
            maxlength: 190,
            messages: { required: CompoundMaster.EnterName }
        });
        $("select[id$=COM_POLYMER]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectPolymer }
        });
        $("select[id$=COM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectFormulation }
        });
        $("input[id$=COM_MAT_PRD]").rules("add", {
            required: true,
            maxlength: 11,
            TwoDecimal: true,
            messages: { required: CompoundMaster.EnterMaturity, maxlength: CompoundMaster.MaxQtyMessageFor8Numeric2Decimal }
        });
        $("select[id$=COM_MAT_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.MaturityPeriodUOM }
        });

        $("input[id$=COM_QUANTITY]").rules("add", {
            required: true,
            maxlength: 14,
            FiveDecimal: true,
            messages: { required: CompoundMaster.EnterUnitQty, maxlength: CompoundMaster.MaxQtyMessageFor8Numeric3Decimal }
        });

        $("select[id$=COM_QTY_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.UnitQtyUOM }
        });

        $("input[id$=COM_EXP_PRD]").rules("add", {
            required: true,
            maxlength: 11,
            TwoDecimal: true,
            messages: { required: CompoundMaster.EnterExpPeriod, maxlength: CompoundMaster.MaxQtyMessageFor8Numeric2Decimal }
        });

        $("select[id$=COM_EXP_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.ExpiryPeriodUOM }
        });

    }

    else if (mode == 2) {
        $("select[id$=CPD_ITEM_CATEGORY]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectCatg }
        });
        $("select[id$=CPD_ITEM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.Selectmaterial }
        });
        $("select[id$=CPD_QTY_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectUOM }
        });
        $("input[id$=CPD_WET_QTY]").rules("add", {
            required: true,
            maxlength: 14,
            FiveDecimal: true,
            messages: { required: CompoundMaster.EnterWetQty }
        });
        $("input[id$=CPD_DRY_PERC]").rules("add", {
            required: true,
            maxlength: 10,
            Percentage: true,
            messages: { required: CompoundMaster.EnterDryPerc }
        });
        $("input[id$=DRYWEIGHT]").rules("add", {
            required: true,
            maxlength: 10,
            ThreeDecimalIncludeZero: true,
            messages: { required: "Enter Dry Weight" }
        });
    }
}

function RemoveValidations(mode) {
    //<summary>Function used to  Remove Validation To Compound Details 1 for Header Details 2 for Items Entry</summary>
    if (mode == 1) {

        $("input[id$=COM_CODE]").rules("remove");
        $("input[id$=COM_NAME]").rules("remove");
        $("select[id$=COM_POLYMER]").rules("remove");
        $("select[id$=COM_TYPE]").rules("remove");
        $("input[id$=COM_MAT_PRD]").rules("remove");
        $("select[id$=COM_MAT_UOM]").rules("remove");
        $("input[id$=COM_QUANTITY]").rules("remove");
        $("select[id$=COM_QTY_UOM]").rules("remove");
        $("input[id$=COM_EXP_PRD]").rules("remove");
        $("select[id$=COM_EXP_UOM]").rules("remove");
        //$("input[id$=MachineTypeName]").rules("remove");
        //$("input[id$=MachineTypeName]").rules("remove");
    }
    else if (mode == 2) {
        $("select[id$=CPD_ITEM_CATEGORY]").rules("remove");
        $("select[id$=CPD_ITEM]").rules("remove");
        $("select[id$=CPD_QTY_UOM]").rules("remove");
        $("input[id$=CPD_WET_QTY]").rules("remove");
        $("input[id$=CPD_DRY_PERC]").rules("remove");
        $("input[id$=DRYWEIGHT]").rules("remove");

    }

}

function AddConversionValidation() {
    //<summary>Function used to  Add Validation From Conversion PopUP</summary>
    $("select[id$=UPC_TO_UOM]").rules("add", {
        selectNone: true,
        messages: { selectNone: CompoundMaster.SelectUOM }
    });
    $("input[id$=UPC_CONV_FACT]").rules("add", {
        required: true,
        maxlength: 12,
        ThreeDecimal: true,
        messages: { required: CompoundMaster.EnterConversion }
    });
}

function RemoveConversionValidation() {
    //<summary>Function used to  Remove Validation From Conversion PopUP</summary>
    $("select[id$=UPC_TO_UOM]").rules("remove");
    $("input[id$=UPC_CONV_FACT]").rules("remove");
}

function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    RemoveValidations(1);
    RemoveValidations(2);
    window.location = "../Masters/CompoundListing.aspx";
    return false;
}

///#endregion

