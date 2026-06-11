
///#region=============== Global variable Declaration
var SLNO = 0;
var toUnitId = 0;
var frmUnitID = 0;
var CompoundJson = new Object();

var tdset = "";
var totMatQty = 0;
var matQty = 0;

///#endregion

///#region=============== Configurations
var CompoundMaster = {

    FillCompoundDtlsURL: "CompoundMaster.do?Action=GetCompoundDetails&PK=",
    FillUomDetailsURL:"UOMManagement.do?Action=GetUnit&UOMTypeID=1&UOMPK=",
    GetMaterialTypeUOMURL:"CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
    GetConersionFactorURL:"CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    GetConversionUOMListURL:"CompoundMaster.do?Action=GetConversionUOMList&UOM=",
    GetPolymerListURL:"CompoundMaster.do?Action=GetPolymerList&SBU=",
    FillUOMURL:"UOMManagement.do?Action=GetUnit&UOMTypeID=",
    GetMaterialTypeURL:"CompoundMaster.do?Action=GetMaterialTypeName&SBU=",
    GetItemNameURL:"CompoundMaster.do?Action=GetMaterialName&SBU=",
    SavePage:"CompoundMaster.do?Action=SaveCompoundList",
    ValueZero:"0",
    ValueEmpty:"",
    Save:"Save",
    Delete:"delete",
    Edit:"edit",
    CannotDelete: "Translate(CannotDelete)",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    DeleteConversion:"DeleteConversion",
    DeleteMaterial:"DeleteMaterial",
    ConfirmationMsgTitle:"Translate(Confirmation)",
    InformationTitle:"Translate(Information)",
    CompoundListingURL:"CompoundListing.aspx",
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
    DefaultAction:"Translate(DefaultActionneedstobeperformed)",
    CodeExists:"Translate(CompoundCodeAlreadyExists)",
    CompoundSavedSuccessMsg:"Translate(CompoundDetailssavedsuccessfully)",
    EnterMaterial:"Translate(EnterMaterialDetails)",
    MaterialAlreadyAdded: "Translate(MaterialAlreadyAdded)",
    QuantityExcceded: "Translate(QuantityExcceddedthanUnitQuantity)"

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
    $("#divAddConversion").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveConversionValidation();
        }
    })
    $("[id$=COM_QTY_UOM]").change(function () {
        ClearMaterialControls();
        FillConversionFact();
        ResetMaterialDetails();
        ShowConversionDtls();
        //CalculateDRYWEIGHT();
    });
    $('input[id$=CPD_WET_QTY]').keyup(function () {
        FillConversionFact();
        CalculateDryWeight();
    });
    $('input[id$=CPD_DRY_PERC]').keyup(function () {
        FillConversionFact();
        CalculateDryWeight();
    });

    $('input[id$=COM_QUANTITY]').keyup(function () {
        ClearMaterialControls();
        GetCompoundPerc();
        // Update List Percentage
        UpdateMaterialList();

    });
    
    FillCompound();
}
///<summary>Used for Fill Compound Details </summary>
function FillCompound() {
    var compoundPK = $("input[id$=COM_PK]").val();
    if (compoundPK != CompoundMaster.ValueZero) {
        $.get(CompoundMaster.FillCompoundDtlsURL + compoundPK, function (data) {
            $("#divDatas").data("CompoundData", data);
            FillCompoundDetails();
            ShowConversionDtls();
        });
    }
    else {
        ShowConversionDtls();
        FillCombo();
    }
}
///<summary>Used for Fill Items Name s to Drop Down </summary>
function FillItem() {
    FillMaterialNames();
    //GetMaterialUOMType();
}
///<summary>Calculate Compound Perscentage When Material UOM Change</summary>
function CalculateCompound() {

    if ($("select[id$=CPD_QTY_UOM]").val() != "0") {
        FillConversionFact();
        CalculateDryWeight();
        CalculatecaonversionFact();
        GetCompoundPerc();
    }
    else {
        $("input[id$=ConversionValue]").val("0");
        $("[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
        $("[id$=DRYWEIGHT]").html(CompoundMaster.ValueZero); // label
        $("[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero); // hdfld
        $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // Label
        $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld

    }
   
}
///<summary>Used for Fill UOM Type when change the Items Drop Down  and  Fill to Hidden Fields</summary>
function GetUOMType() {
    
    GetMaterialUOMType();
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
    return false;
}

function ShowConversionDtls() {
    ///<summary> Function used To Show Conversion Details</summary>
    if ($("select[id$=COM_QTY_UOM]").val() != "0" && $("select[id$=COM_QTY_UOM]").val() != null) {
        $("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
        ResetUOMConversion();
        ResetConversionDetails();
       // FillUOMDetails();

        $("[id$=UOMTypeFm]").text(" : " + $("select[id$=COM_QTY_UOM] option:selected").text());

    }
    else

    //$("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });
        $("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
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
            obj.UMC_UOM_TYPE =1; // Weight
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

            GrandScriptUtils.ShowModal(CompoundMaster.SameConversionAlreadyAddedMsg,CompoundMaster. InformationTitle);
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
            GrandScriptUtils.ShowModal(CompoundMaster.DoUWantToDelMsg, CompoundMaster. ConfirmationMsgTitle, CompoundMaster.DeleteConversion, true);
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
    $("[id$=DRYWEIGHT]").html(CompoundMaster.ValueZero);
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
        $("#MaterialControls").css({ "display": "block", "visibility": "visible" });
    }
    ClearMaterialControls();
}

function AddMaterialDetails() {
    //<summary>Function used to Add Material Details  </summary>
    AddValidations(1);
    AddValidations(2);
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
                obj.MATERIALQUANTITYUOM = $("[id$=CPD_WET_QTY]").val() +" (" + $("select[id$=CPD_QTY_UOM] :selected").text()+" )";
                obj.CPD_DRY_PERC = parseFloat($("[id$=CPD_DRY_PERC]").val());
                obj.DRYWEIGHT = $("[id$=DRYWEIGHT]").html();
                obj.CPD_DRY_QTY = parseFloat($("[id$=CPD_DRY_QTY]").val());
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

                GrandScriptUtils.ShowModal(CompoundMaster. MaterialAlreadyAdded, CompoundMaster.InformationTitle);
            }

        }
        else {
            GrandScriptUtils.ShowModal( CompoundMaster.QuantityExcceded, CompoundMaster.InformationTitle);
        }
        return false;
    }


}

function FillMaterialDetails(tr) {
    //<summary>Function used to Fill Mateiral Details When Edit  </summary>
    ClearMaterialControls();
    $("select[id$=CPD_ITEM_CATEGORY]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    //FillMaterialType(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    FillMaterialConversionUOM(GrandGrid.Utilities.GetColumnValue(tr, "CPD_QTY_UOM", $(tr).parent().parent().attr("id")));
    FillMaterialNames(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_WET_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_WET_QTY", $(tr).parent().parent().attr("id")));
    $("input[id$=ConversionValue]").val(GrandGrid.Utilities.GetColumnValue(tr, "ConversionValue", $(tr).parent().parent().attr("id")));
    $("input[id$=CPD_DRY_PERC]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_DRY_PERC", $(tr).parent().parent().attr("id")));
    $("[id$=DRYWEIGHT]").html(GrandGrid.Utilities.GetColumnValue(tr, "DRYWEIGHT", $(tr).parent().parent().attr("id")));
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
        $("#MaterialControls").css({ "display": "block", "visibility": "visible" });
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
    //FormulationType(0);
    //FillMaterialType(0);
    FillMaterialNames(0);
    // Fill Weight Type
    FillUOM(0, 1, $("select[id$=COM_QTY_UOM]").attr("id"));
    // Fill Time Type
    FillUOM(0, 3, $("select[id$=COM_MAT_UOM]").attr("id"));
    FillUOM(0, 3, $("select[id$=COM_EXP_UOM]").attr("id"));
    FormulationType(0);
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
        $.get(CompoundMaster.GetMaterialTypeUOMURL+ $("select[id$=CPD_ITEM]").val(), function (data) {
            if (data != " " && data != "-1") {
                //alert("Converted Factor  Value:" + data);
                $("input[id$=MaterialUOMType]").val(data);
            }
            else {
                $("input[id$=MaterialUOMType]").val(CompoundMaster.ValueZero);
            }
            if ($("input[id$=MaterialUOMType]").val() == "1") {
                FillMaterialConversionUOM(0);
            }
            else {
                var drpID = $("select[id$=CPD_QTY_UOM]").attr("id");
                GrandScriptUtils.FillDropDown(drpID, null, true, true);
            }
        });
    }
    else {
        $("input[id$=MaterialUOMType]").val("1");
        FillMaterialConversionUOM(0);
    }


}



function GetConversionFact() {
    //<summary>Function used to Get Conversion Factor  </summary>
    $.get(CompoundMaster. GetConersionFactorURL + $("select[id$=COM_QTY_UOM]").val() + "&UOMTo=" + $("select[id$=CPD_QTY_UOM]").val(), function (data) {
        if (data != " " && data != "-1") {
            //alert("Converted Factor  Value:" + data);
            $("input[id$=ConversionValue]").val(data);
        }
        else {
            $("input[id$=ConversionValue]").val("0");
        }
    });

}

function FillMaterialConversionUOM(selValue) {
    //<summary>Function used to Fill UOm Details With Selected UOM have COnversion Factor  </summary>
    var drpID = $("select[id$=CPD_QTY_UOM]").attr("id");
    $.get(CompoundMaster. GetConversionUOMListURL + $("select[id$=COM_QTY_UOM]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });

}

function FillPolymer(selValue) {
    //<summary>Function used to Fill Polymer Type to DropDown  </summary>
    var drpID = $("select[id$=COM_POLYMER]").attr("id");
    $.get(CompoundMaster. GetPolymerListURL + $("[id$=SBU]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}




function FillUOM(selValue, uOMType, drpID) {
    //<summary>Function used to Fill UOM Type to DropDown  </summary>
    $.get(CompoundMaster. FillUOMURL + uOMType, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FormulationType(selValue) {
    //<summary>Function used to Fill Formaulation Type to DropDown  </summary>
    var drpID = $("select[id$=COM_TYPE]").attr("id");
    $.get("CompoundMaster.do?Action=GetFormulationType&SBU=" + $("[id$=SBU]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}
function FillMaterialType(selValue) {
    //<summary>Function used to Fill Material Type Type to DropDown  </summary>
    var drpID = $("select[id$=CPD_ITEM_CATEGORY]").attr("id");
    $.get(CompoundMaster. GetMaterialTypeURL + $("[id$=SBU]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}
function FillMaterialNames(selValue, catgID) {
    //<summary>Function used to Fill Material Name to DropDown  </summary>
    var drpID = $("select[id$=CPD_ITEM]").attr("id");
    $.get(CompoundMaster. GetItemNameURL + $("[id$=SBU]").val() + "&CATG=" + $("select[id$=CPD_ITEM_CATEGORY]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
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
    // Check Have The Order List have More than or equal to one Product Details
    if (CompoundJson.CompoundMaterialsList.length > 0) {
        if ($(document.forms[0]).valid()) {
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
                        else if (parseInt(data) < 0)
                            msgtxt = CompoundMaster.ActionFailed;
                        GrandScriptUtils.ShowModal(msgtxt, CompoundMaster.InformationTitle);
                    }

                });
            }
            else {
                GrandScriptUtils.ShowModal("Quantity Not Matching with Material Quantity", CompoundMaster.InformationTitle);
            }

        }

    }
    else {
        GrandScriptUtils.ShowModal(CompoundMaster.EnterMaterial, CompoundMaster.InformationTitle);
    }
    return false;


}

// To Fill Details in pgae load

function FillCompoundDetails() {
    //<summary>Function used to Fill Compound Details Tp Controls  </summary>
    CompoundJson = $("#divDatas").data("CompoundData");
    FillPolymer(CompoundJson.COM_POLYMER);
    //FillMaterialType(0);
    FillMaterialNames(0);
    // Fill Weight Type
    FillUOM(CompoundJson.COM_QTY_UOM, 1,$("select[id$=COM_QTY_UOM]").attr("id"));
    // Fill Time Type                   
    FillUOM(CompoundJson.COM_MAT_UOM, 3,$("select[id$=COM_MAT_UOM]").attr("id")); 
    FillUOM(CompoundJson.COM_EXP_UOM, 3,$("select[id$=COM_EXP_UOM]").attr("id"));
    FormulationType(CompoundJson.COM_TYPE);
    $("input[id$=COM_PK]").val(CompoundJson.COM_PK);
    $("input[id$=COM_NAME]").val(CompoundJson.COM_NAME);
    $("input[id$=COM_CODE]").val(CompoundJson.COM_CODE);

    $("input[id$=COM_QUANTITY]").val(CompoundJson.COM_QUANTITY);
    $("input[id$=COM_EXP_PRD]").val(CompoundJson.COM_EXP_PRD);
    $("input[id$=COM_MAT_PRD]").val(CompoundJson.COM_MAT_PRD);

    //$("Select[id$=COM_TYPE]").val(CompoundJson.COM_TYPE);


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

   

}

function UpdateMaterialList() {
    //<summary>Function used to Update material list when change the Qunatity </summary>
    if (CompoundJson.CompoundMaterialsList.length > 0) {
        var totQty = $('input[id$=COM_QUANTITY]').val();
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
                perc = Round(perc, 2);
                CompoundJson.CompoundMaterialsList[i].CompoundPercentage = perc;
                CompoundJson.CompoundMaterialsList[i].CPD_COMP_PERC = perc;
            }
        }
        if (CheckQuantityNotExceeded()) {

            $("#divDatas").data("CompoundData", CompoundJson);
            GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
        }
        else {
            GrandGrid.Utilities.ResetGrid(true, "grdCompoundMaterial");
            CompoundJson.CompoundMaterialsList = new Array();
            GrandGrid.MakeGrid($("#grdCompoundMaterial"), 0, CompoundJson.CompoundMaterialsList);
            if (CompoundJson.CompoundMaterialsList.length == 0) {
                //Will insert the selection tr  into the  TermsInsert table and show the TermsInsert Table
                $(tdset).insertAfter($("#MaterialControls").find("tr:eq(0)"));
                $("#MaterialControls").show();
                $("#MaterialControls").css({ "display": "block", "visibility": "visible" });
            }
           
        }

    }

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
        case CompoundMaster. DeleteConversion:
            DeleteConversionDetails();
            break;
        case CompoundMaster.DeleteMaterial:
            DeleteMaterialDetails();
            break;

        case CompoundMaster.Save:
            window.location =CompoundMaster. CompoundListingURL;

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
        $(tdset).insertBefore($("#grdCompoundMaterial").find("tr:eq(1)"));
    }
}
///#endregion 

///#region===============  Calculations

//<summary>Function used to Calculate Conversion Factor</summary>
function CalculatecaonversionFact() {
 // Updations Required
    if ($("input[id$=ConversionValue]").val() != CompoundMaster.ValueZero && $("input[id$=ConversionValue]").val() != CompoundMaster.ValueEmpty) {
        var qty = 0;
        var matQty = parseFloat($('input[id$=CPD_DRY_QTY]').val());
        var convFact = parseFloat($("input[id$=ConversionValue]").val());
        if (matQty && convFact) {
            qty = matQty / convFact;

        }
        if (isNaN(qty) || Infinity == qty || qty == 0) {
            $("[id$=ConvertedWt]").val(CompoundMaster.ValueZero);
        }
        else {
            qty = Round(qty, 2);
            $("[id$=ConvertedWt]").val(qty);
           //## alert(qty);
           
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
    var totQtyList = parseFloat(GetTotalQtyFromList());
    if (parseFloat($("input[id$=COM_QUANTITY]").val()) == totQtyList) {
        return true;
    }
    else
        return false;
}

function CalculateDryWeight() {
    if ($('select[id$=CPD_WET_QTY]').val() != "0") {
        var matQty = $('input[id$=CPD_WET_QTY]').val();
        var dryPer = $('input[id$=CPD_DRY_PERC]').val();
        var dryQty = 0;
        if (matQty && dryPer) {
            dryQty = matQty * (dryPer / 100);
        }
        if (isNaN(dryQty) || Infinity == dryQty || dryQty == 0) {
            $("[id$=DRYWEIGHT]").html(CompoundMaster.ValueZero); // label
            $("[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero); // hdfld
            $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // Label
            $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld
        }
        else {
            dryQty = Round(dryQty, 2);
            $("[id$=CPD_DRY_QTY]").val(dryQty);
            var qty = dryQty + $("select[id$=CPD_QTY_UOM] :selected").text();
            $("[id$=DRYWEIGHT]").html(qty);
            //CalculateCompoundPercetnage();
            FillConversionFact();
            CalculatecaonversionFact();
            GetCompoundPerc();
        }
    }
    else {
        $("[id$=DRYWEIGHT]").html(CompoundMaster.ValueZero); // label
        $("[id$=CPD_DRY_QTY]").val(CompoundMaster.ValueZero); // hdfld
        $("[id$=CompoundPercentage]").html(CompoundMaster.ValueZero); // Label
        $("[id$=CPD_COMP_PERC]").val(CompoundMaster.ValueZero); // hdfld
        FillConversionFact();
    }
}

function GetTotalQtyFromList() {
    var totQty = 0;
    for (var i in CompoundJson.CompoundMaterialsList) {
        if (CompoundJson.CompoundMaterialsList[i].SL_NO != $("input[id$=SL_NO]").val()) {
            totQty += parseFloat(CompoundJson.CompoundMaterialsList[i].ConvertedWt);
        }
    }
    return totQty;
}

function GetCompoundPerc() {
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
        perc = Round(perc, 2);
        $("[id$=CompoundPercentage]").html(perc); // label
        $("[id$=CPD_COMP_PERC]").val(perc); // hdfld
        //CalculatecaonversionFact();
    }
}
///#endregion  

///#region=============== Validation
function AddValidations(mode) {

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
            maxlength: 10,
            ThreeDecimal: true,
            messages: { required: CompoundMaster.EnterMaturity }
        });
        $("select[id$=COM_MAT_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectUOM }
        });

        $("input[id$=COM_QUANTITY]").rules("add", {
            required: true,
            maxlength: 10,
            ThreeDecimal: true,
            messages: { required: CompoundMaster.EnterUnitQty }
        });
        $("select[id$=COM_QTY_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectUOM }
        });

        $("input[id$=COM_EXP_PRD]").rules("add", {
            required: true,
            maxlength: 10,
            ThreeDecimal: true,
            messages: { required: CompoundMaster.EnterExpPeriod }
        });
        $("select[id$=COM_EXP_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundMaster.SelectUOM }
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
            maxlength: 10,
            ThreeDecimal: true,
            messages: { required: CompoundMaster.EnterWetQty }
        });
        $("input[id$=CPD_DRY_PERC]").rules("add", {
            required: true,
            maxlength: 10,
            Percentage: true,
            messages: { required: CompoundMaster.EnterDryPerc }
        });
    }
}

function RemoveValidations(mode) {
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
        $("input[id$=MachineTypeName]").rules("remove");
        $("input[id$=MachineTypeName]").rules("remove");
    }
    else if (mode == 2) {
        $("select[id$=CPD_ITEM_CATEGORY]").rules("remove");
        $("select[id$=CPD_ITEM]").rules("remove");
        $("select[id$=CPD_QTY_UOM]").rules("remove");
        $("input[id$=CPD_WET_QTY]").rules("remove");
        $("input[id$=CPD_DRY_PERC]").rules("remove");

    }

}

function AddConversionValidation() {
    $("select[id$=UPC_TO_UOM]").rules("add", {
        selectNone: true,
        messages: { selectNone: CompoundMaster.SelectUOM }
    });
    $("input[id$=UPC_CONV_FACT]").rules("add", {
        required: true,
        maxlength: 10,
        ThreeDecimal: true,
        messages: { required: CompoundMaster.EnterConversion }
    });
}

function RemoveConversionValidation() {
    $("select[id$=UPC_TO_UOM]").rules("remove");
    $("input[id$=UPC_CONV_FACT]").rules("remove");
}

///#endregion