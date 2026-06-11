/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />

/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../../GrandScriptUtils.js" />


///#region Global variable Declaration
var DispersJson = new Object();
var tdset = "";
var DispersID = 0;
var UOMJson = new Object();
//var machineTypeID = 0;
var ConversionFactor = 1;
var toUnitId = 0;
var percAvailable = 100;
var tempQuantity = 0.0;
var deleteMaterialPk = 0;
var dispersionDecimal = 0;
var SignificantFigure = 0.0001; //value used to check equality .if the difference between from and to values is less than this value accept as equal ;otherwise reject
///#endregion

///#region Configuration section
var DispersionMaster = {
   // FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListDeptAuto&SBUPk=",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListWithoutSemiAndFinished&SBUPk=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
    GetMaterialUOMConversion: "MaterialManagement.do?Action=GetMaterialUOMConversion&MaterialId=",
    MachineTypeDeleteMsg: "Translate(MachineTypeDetailsDeletedSuccessfully)",
    MACHINETYPEURL: "MachineryManagement.do?Action=GetMachineType&SBU=",
    SAVEMACHINETYPEURL: "MachineryManagement.do?Action=SaveMachineType&SBU=",
    GETUOMTYPENAME: "CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
    GETCONVERSIONUOM: "CompoundMaster.do?Action=GetConversionUOMList&UOM=",
    GETWEIGHTUOMS: "UOMManagement.do?Action=GetUnit&UOMTypeName=Weight&",
    GETTIMEUOM: "UOMManagement.do?Action=GetUnit&UOMTypeName=Time&",
    GetConcversionFactors: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    MakeAutoCompleteSearch: "DispersionManagement.do?Action=GetSearchValue&SBU=",
    GetMaterialsCombo: "MaterialManagement.do?Action=GetMaterials",
    SavePage: "DispersionManagement.do?Action=SavePage",
    BindGrid: "DispersionManagement.do?Action=GetDispersionList&Status=",
    GetDispersionDetail: "DispersionManagement.do?Action=GetDispersionDetail&DispersionID=",
    DeleteDispersion: "DispersionManagement.do?Action=DeleteDispersion&DispersionID=",
    GetUOMsURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=1&UOMPK=",
    GETTEMPLATECHECKLISTURL: "CommonManagement.do?Action=GetTemplateCheckList&checkListPK=0&Active=1&processID=",
    GetCheckListDetailsURL: "CommonManagement.do?Action=GetCheckList&checkListID=",
    GetItemNameURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&SearchType=",
    DispersionTypeURL: "DispersionManagement.do?Action=GetDispersionTypes&SBU=",
    GetUOMListURL: "CompoundMaster.do?Action=GetUOMListForItemAndCompound&UOM=",
    GetMachineURL: "MachineryManagement.do?Action=GetMachineNameByType&MachType=",
    GetStores: "MaterialManagement.do?Action=GetMaterialStores&SBU=",
    GetMaterialCategoryDetailsURL: "CommonManagement.do?Action=GetCategoryValue&CategoryPK=",
    GetMaterialCategoryTreeURL: "MaterialCategory.do?Action=GetMaterialCategoryTypeWithoutSemiAndFinished&SBUPk=",
    MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListAuto",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore", 
    //Messages
    MachineTypeSaveMsg: "Translate(MachineTypeSavedSuccessfullly)",
    MachineTypeExists: "Translate(MachineTypeAlreadyExists)",
    ActionFailedMsg: "Translate(ActionFailedPleaseTryAgain)",
    InformationTitle: "Translate(Information)",
    QuantityNotReached: "Translate(QuantityNotReached)",
    MACHINETYPEGRIDURL: "MachineryManagement.do?Action=GetMachineTypeDetails&SBU=",
    MachineTypeDelete: "deletemachinetype",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    DefaultActionMsg: "Translate(DefaultActionneedstobeperformed)",
    DELETEMACHINETYPEURL: "MachineryManagement.do?Action=DeleteMachineType&MachineTypeID=",
    MachineTypeAssignedMsg: "Translate(MachineTypeAlreadyAssigned)",
    DeleteMachineTypeSucessesCMD: "deleteSucessMachine",
    QuantityExceeded: "Translate(QuantityExceeded)",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    SelectAnotherUOM: "Translate(SelectAnotherMaterial)",
    Alreadyasigned: "Translate(CannotDeleteHaveReference)",
    MaterialCannotAdded: "Translate(MaterialCannotAdded)",
    AddMachineType: "Translate(AddMachineType)",
    MaterialAlreadyAdded: 'Translate(MaterialAlreadyAdded)',
    PleaseSelectAnOption: 'Translate(Pleaseselectanoption)',
    ConversionAlreadyAdded: "Translate(ConversionAlreadyAdded)",
    ConfirmationMsg: "Translate(Confirmation)",
    AddMachineType: "Translate(AddMachineType)",
    SavedSuccess: 'Translate(BOMDetailssavedsuccessfully)',
    AlreadyExists: 'Translate(BOMCodealreadyexists)',
    DoyouWantToDelete: 'Translate(Doyouwanttodeletethisdetails)',
    DeleteSuccess: 'Translate(BOMDeletedSuccessfully)',
    DeletedRecord: "Translate(DeletedRecord)",
    DispersionPageResName: "Translate(BOMPageResName)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    StoreMappSaveMessage: "Translate(StoreMappingSavedSuccesfully)",

    //validation msgs
    EnterDispersionName: 'Translate(PleaseProvideBOMName)',
    EnterExpiryTime: 'Translate(PleaseProvideExpiryTime)',
    EnterUOM: 'Translate(PleaseSelectUOM)',
    EnterProduct: 'Translate(PleaseselectaProduct)',
    EnterCatagory: 'Translate(PleaseSelectCategory)',
    EnterConversionFactor: "Translate(EnterConversionFactor)",
    SelectUOM: 'Translate(SelectUOM)',
    EnterQuantity: 'Translate(PleaseProvideQuantity)',
    EnterPreparationTime: 'Translate(PleaseProvidePreparationTime)',
    EnterDispersionCode: 'Translate(PleaseProvideBOMCode)',
    SelectMachineType: 'Translate(PleaseselectaMachineType)',
    QuantityGreaterthanZero: 'Translate(OrderAddQtyNumeric)',
    // Fields
    MachineTypeID: "MCT_PK",
    MachineName: "MCT_NAME",
    ValueZero: "0",
    ProcessID: "1", //For Dispersion
    ValueEmpty: "",

    //commands
    MachineTypeEdit: "editmachinetype",
    DeleteMachineTypeCMD: "deleteMachinetype",
    DeleteMaterial: "deletematerial",
    SaveMachineType: "SaveMachineType",
    Unsuccess: "Unsuccess",
    Saved: "saved",
    Failed: "failed",
    DeleteDispersionCmd: "deleteDisprsn",
    Deleted: "deleted",
    DeleteConversion: "DeleteConversion"


}



///#endregion

///#region ------ Initialization Section ----------------



$(document).ready(function () {
    ///<summary>On Ready function</summary>    
    //Initialize page
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        onchange: false
    });
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Translate(AutoDefaultValue)");
    }, "Translate(Pleaseselectanoption)");
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersJson);
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
    $("[id$=DSP_QTY_UOM]").change(function () {
        ShowConversionDtls();
    });

    $("#divAddConversion").dialog({
        autoOpen: false,
        width: 550,
        height: 300,
        beforeClose: function (event, ui) {
            RemoveConversionValidation();
        }
    });
    // Initialize/Load data to the view state
    DispersJson = $.parseJSON($("[id$=DispersionDetailsList]").val());
    // DispersJson = $.parseJSON($("[id$=ConversionList]").val());
    $("#divData").data("DispersionData", DispersJson);



    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    DispersJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);

});


function PageInit() {
    //<summary>Method to initialize the page</summary>
    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
    }, DispersionMaster.PleaseSelectAnOption);
    //$("[id$=machineAdd]").attr("title", DispersionMaster.AddMachineType);
    //FillMachine(0);
   //seach functionality 
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    SearchInit();
    SetSearchType();
    //Fill the main grid on load
    BindGrid();
    //BindMachineTypeGrid();
    $("#divData").hide();
    $("[id$=btnSave]").hide();
    $("[id$=imbSave]").hide();
    $("[id$=imbCancel]").hide();
    //fill Material to the Material dropdown
    // FillMaterials();
    //Fill UOMs
    FillUOMs();
    //FillMachineType();
    //FillTemplateName();
   // FillDispersionTypeDDL();
    if ($("[id$=hdfShowPrefix]").val() == "1") {
        FillDispersionPrefixDDL();
        $("#divPrefix").show();
        $('select[id$=DSP_PREFIX]').rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Prefix" }
        });
    }
    else {
        $("#divPrefix").hide();
    }
    FillHeaderMaterialCategoryAutoComplete(); //FillCategory(0);
    FillStoreTree(0);    
    dispersionDecimal = $("[id$=hdfCompoundingDecimal]").val();
    $("[id$=tabs]").tabs();
    $("[id$=tabs]").tabs("select", 0);
    $("[id$=tabs]").tabs("disable", 1);    
}


function SearchInit() {
    ///<summary>To handle auto complete</summary>
    var SBU = parseInt($("[id$=BizUnitPk]").val());
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", DispersionMaster.MakeAutoCompleteSearch + SBU, "SearchType");
}


function SetSearchType() {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "0") {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
    }
}

///#endregion


////#region--- core section

///#region---fill Dropdowns

function MaterialChangeEvent() {
    ///<summary>Event triggered on Material change </summary>
    if ($("[id$=Material]").val() != "0") {
        FillMaterialUOMs($("[id$=MaterialPK]").val()); // FillMaterialUOMs($("[id$=DSD_ITEM]").val());
    }
    return false;
}


function FillUOMs() {
    ///<summary>Method the fill the UOM for fields</summary>
    FillQuantityUOMDropdown();
    FillQuantityUOMDropdouwnGrid();
    FillTimeUOMDropdown();
    FillMaterialCategoryAutoComplete(); // FillMaterialCatagory();
    FillMaterialAutoComplete();

}

function FillItem() {

    $("[id$=DSD_ITEM]").val("");
    $("[id$=MaterialPK]").val(0); //$("select[id$=DSD_ITEM]").val(DispersionMaster.ValueZero);
    $("input[id$=DSD_QUANTITY]").val(DispersionMaster.ValueEmpty);
    $("[id$=DSD_QTY_UOM]").val(DispersionMaster.ValueZero);
    $("[id$=DSD_QTY_UOM_TEXT]").html("");
    //FillMaterialNames(0, 0);
}

function FillMaterialNames(selValue, uomPk) {
    //<summary>Function used to Fill Material Name to DropDown  </summary>
    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    $.getJSON(DispersionMaster.GetItemNameURL + $("[id$=MaterialCategoryPK]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        // FillMaterialConversionUOM(uomPk);
    });
}

function FillMaterialUOMs(materialID, materialUOM) {
    ///<summary>Fill Material quantity UOM</summary>
    //fill UOM of Dispersion quantity
    var categoryID = $("[id$=MaterialCatagory]").val();

    var selectedUOM = "";
    // var materialID = $("[id$=DSD_ITEM]").val();
    var editProduct = $("input[id$=EditProduct]").val();
    var drpUOMID = $("select[id$=DSD_QTY_UOM]").attr("id");
    var drpID = $("select[id$=MRD_UOM]").attr("id");
    $.get(DispersionMaster.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=MaterialName]").html(data[0].ITM_NAME);
                $("[id$=DSD_QTY_UOM_TEXT]").html(data[0].UOM_CODE);
                $("[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);

                FillMaterialCategoryAutoComplete();
                $("[id$=MaterialCategoryPK]").val(data[0].ITM_CATEGORY);
                $("[id$=MaterialCategory]").val(data[0].ITC_NAME);
            }
            else {
                $("[id$=MaterialName]").html("");
                $("[id$=DSD_QTY_UOM]").val("0");
                $("[id$=DSD_QTY_UOM_TEXT]").html("");
            }

        }

    });

//    if (categoryID == "1") {

//        $.get(DispersionMaster.GETUOMTYPENAME + materialID, function (data) {
//            if (data != " " && data != "-1") {
//                //alert("Converted Factor  Value:" + data);
//                if (parseInt(data) != 1) {
//                    GrandScriptUtils.ShowModal(DispersionMaster.MaterialCannotAdded, 'Translate(Information)');
//                    return false;
//                }
//            }
//        });
//        $.get(DispersionMaster.GETCONVERSIONUOM + $("[id$=DSP_QTY_UOM]").val(), function (data) {
//            if (materialUOM == null) {
//                GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, selectedUOM);
//            }
//            else {
//                GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, materialUOM);
//            }

//        });

//        var drpID = $("select[id$=MRD_UOM]").attr("id");
//        $.get(DispersionMaster.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
//            if (data) {
//                if (materialID != 0) {
//                    $("[id$=MaterialName]").html(data[0].ITM_NAME);
//                    $("[id$=DSD_QTY_UOM_TEXT]").html(data[0].UOM_CODE);
//                    $("[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);
//                }
//                else {
//                    $("[id$=MaterialName]").html("");
//                    $("[id$=DSD_QTY_UOM]").val("0");
//                    $("[id$=DSD_QTY_UOM_TEXT]").html("");
//                }

//            }

//        });

//    }
//    else if (categoryID == "2") {
//        if (editProduct > 0) {
//            $.get(DispersionMaster.GETCONVERSIONUOM + $("[id$=DSP_QTY_UOM]").val(), function (data) {
//                if (materialUOM == null) {
//                    GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, selectedUOM);
//                }
//                else {
//                    GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, materialUOM);
//                }

//            });
//        }
//        else {
//            //FillMaterialConversionUOM($("select[id$=DSP_QTY_UOM]").val());
//            FillSelectedDispersionDetails();
//        }
//    }
}

function FillSelectedDispersionDetails() {
    $.getJSON(DispersionMaster.GetDispersionDetail + $("[id$=MaterialPK]").val(), function (data) {
        if (data != null) {
            $("[id$=DSD_QTY_UOM_TEXT]").html(data.DSP_QTY_UOM_TEXT);
            $("[id$=DSD_QTY_UOM]").val(data.DSP_QTY_UOM);
        }
        else {
            $("[id$=DSD_QTY_UOM_TEXT]").html("");
            $("[id$=DSD_QTY_UOM]").val('0');
        }
    });
}
function FillMaterialConversionUOM(selValue) {
    //<summary>Function used to Fill UOm Details With Selected UOM have COnversion Factor  </summary>
    var drpID = $("select[id$=DSD_QTY_UOM]").attr("id");
    $.get(DispersionMaster.GetUOMListURL + $("select[id$=DSP_QTY_UOM]").val() + "&ItmPK=" + $("[id$=MaterialPK]").val() + "&Catg=" + $("[id$=MaterialCategoryPK]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        if (data) {
            if (data.length > 0)
                GetConversionFactor($("[id$=DSP_QTY_UOM]").val(), data[0].Value);
        }
    });
}
function FillMaterialCatagory(catagoryID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=MaterialCatagory]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(DispersionMaster.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val()+"&ITCVAL=1", function (data) {
        if (catagoryID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catagoryID);
        }
    });

}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("MaterialCategory", DispersionMaster.MaterialCategroyDeptURL + "&Type=" + $("[id$=ITM_SET]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
}
function FillHeaderMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("txtDspItemCategory", DispersionMaster.MaterialCategroyDeptURL + "&Type=" + $("[id$=ITM_SET]").val(), "DSP_ITM_CATEGORY", true, false, "BizUnitPk", true);
}
function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "MaterialCategory") {
        FillItem();
        FillMaterialAutoComplete();
    }
    if (targetControlID == "DSD_ITEM") {
        MaterialChangeEvent();
    }
    if (targetControlID == "txtDspItemCategory") {
        SetCategoryType();
    }
}
function FillMaterialAutoComplete() {
    //<summary> Function Used to make Item field as auto complete </summary>
    var SearchVal = $("[id$=DSD_ITEM]").val();
    if (SearchVal == "Select/Type") {
        if ($("[id$=SearchValue]").val() != "") {
            SearchVal = encodeURIComponent($("[id$=SearchValue]").val()); //this is required when searchdata(QueryString) comes from PackingSpec form
        }
        else {
            SearchVal = "";
        }
    }
    else {
        SearchVal = encodeURIComponent($("[id$=DSD_ITEM]").val());
    }
    //    GrandScriptUtils.MakeAutoComplete("DSD_ITEM", DispersionMaster.MaterialURL + "&SearchValue=" + SearchVal, "MaterialPK", true, false, "MaterialCategoryPK", true);
    GrandScriptUtils.MakeAutoComplete("DSD_ITEM", DispersionMaster.MaterialURL, "MaterialPK", true, false, "MaterialCategoryPK", true);
}
function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    // FillUOM(categoryID, false);
    ClearOnMaterialCategoryDropChange();
    if (categoryID != "0") {

        FillCategoryMaterials(categoryID);
    }


}
function ClearOnMaterialCategoryDropChange() {
    $("[id$=DSD_ITEM]").val("");
    $("[id$=MaterialPK]").val(0); // var drpItemID = $("select[id$=DSD_ITEM]").attr("id");
    //var drpUomID = $("select[id$=DSD_QTY_UOM]").attr("id");
    //GrandScriptUtils.FillDropDown(drpItemID, null, true, true);
    //GrandScriptUtils.FillDropDown(drpUomID, null, true, true);
    $("[id$=DSD_QTY_UOM]").val(0);
    $("[id$=DSD_QTY_UOM_TEXT]").html("");
    $("[id$=MaterialName]").html("");
    $("[id$=DSD_QUANTITY]").val("");
    $("#tdPercentage").html("");
}
function FillCategoryMaterials(categoryID, materialID) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="materialID"  type="object">
    ///     Specific materialID to select the dropdown item after filling drop down
    /// </param>
    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    $.getJSON(DispersionMaster.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID, function (data) {
        if (materialID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });



}



function FillMaterials(materialID) {
    ///<summary>/fill materials in the dropdown</summary>
    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    $.getJSON(DispersionMaster.GetMaterialsCombo, function (data) {
        if (materialID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
    });
}



function FillQuantityUOMDropdouwnGrid() {
    /////<summary>fills UOMs on the  UOMQuantity in the grid</summary>
    //fill UOM of Material Quantity
    var drpID = $("select[id$=DSP_QTY_UOM]").attr("id");
    $.get(DispersionMaster.GETWEIGHTUOMS + "SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}


function FillQuantityUOMDropdown() {
    ///<summary>Fill quantity UOMs for header</summary>
    //fill UOM of Dispersion quantity
    var drpID = $("select[id$=DSP_QTY_UOM]").attr("id");
    $.get(DispersionMaster.GETWEIGHTUOMS + "SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}


function FillTimeUOMDropdown() {
    ///<summary>Fill Time uom </summary>
    var drpID1 = $("select[id$=DSP_EXP_TM_UOM]").attr("id");
    var drpID2 = $("select[id$=DSP_PREP_TM_UOM]").attr("id");
    $.get(DispersionMaster.GETTIMEUOM + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID1, data, true, true);
        GrandScriptUtils.FillDropDown(drpID2, data, true, true);
    });
}



//function FillMachineType(machineTypeID) {
//    //<summary>function To Fill Maachine Type Details </summary>
//    /// <param name="machineTypeID"  type="object"> 
//    /// </param>
//    // Get id of the MachineType DropDown
//    var drpID = $("select[id$=DSP_MACHINE_TYPE]").attr("id");
//    //Fill MachineType Details to the Machine Type DropDown, Name as Text, PK as Value
//    $.get(DispersionMaster.MACHINETYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, true, machineTypeID);
//    });
//}


//function FillTemplateName() {
//    var drpID = $("select[id$=DSP_CHECK_LIST_HDR]").attr("id");
//    //Fill Dispersion Details to the Template Name DropDown, Name as Text, PK as Value
//    $.get(DispersionMaster.GETTEMPLATECHECKLISTURL + DispersionMaster.ProcessID + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, true);
//    });
//}

//function AddMachineType() {
//    ///<summary>Method to show the popup to add Machine types</summary>
//    GrandScriptUtils.ShowModalID("divMachineType", DispersionMaster.AddMachineType, DispersionMaster.SaveMachineType);
//    return false;
//}

///#endregion

function FillDispersionTypeDDL() {
    //<summary>function To Fill Dispersion Type Details </summary>    
    /// </param>
    // Get id of the DispersionType DropDown
    var drpID = $("select[id$=DSP_TYPE]").attr("id");
    //Fill MachineType Details to the DispersionType DropDown, Name as Text, PK as Value
    $.get(DispersionMaster.DispersionTypeURL + $("[id$=BizUnitPk]").val() + "cfgPK=0&cfgType=DISPERSION TYPE&Active=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillDispersionPrefixDDL() {
    //<summary>function To Fill Dispersion Type Details </summary>    
    /// </param>
    // Get id of the DispersionType DropDown
    var drpID = $("select[id$=DSP_PREFIX]").attr("id");
    //Fill MachineType Details to the DispersionType DropDown, Name as Text, PK as Value
    $.get(DispersionMaster.DispersionTypeURL + $("[id$=BizUnitPk]").val() + "cfgPK=0&cfgType=DISPERSION PREFIX&Active=1", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}



//function ViewCheckListDetails() {
//    ///<summary>To handle bind grid </summary> 
//    var ajaxUrl = DispersionMaster.GetCheckListDetailsURL + $("[id$=DSP_CHECK_LIST_HDR]").val();
//    $.getJSON(ajaxUrl, function (data) {
//        if (data) {
//            FillCheckListDetails(data);
//            $("#divChecklist").dialog("open");
//            $("#divChecklist").dialog({ "width": 550 });
//        }
//    });
//    return false;
//}

//function FillCheckListDetails(CheckLstObj) {
//    GrandGrid.Utilities.ResetGrid(true, "grdChecklistDetails");
//    if (!($.isArray(CheckLstObj.CheckListDtl))) {
//        var objArray = CheckLstObj.CheckListDtl;
//        CheckLstObj.CheckListDtl = new Array();
//        CheckLstObj.CheckListDtl.push(objArray);
//    }
//    GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, CheckLstObj.CheckListDtl);
//}

////#region ---Calculation section

function Round(x, y) {
    ///<summary>Method to round decimal no. to given no. of positions</summary>
    /// <param name="x" >
    ///     Input decimal value
    /// </param>
    ///<param name="y" >
    ///     No. of decimal points to be restricted
    /// </param>
    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);
    // return x.toFixed(y);
}


function CalculateBaseQuantity() {
    ///<summary> method to caluclate the base quantities(quantity scaled to the Dispersion Quantity UOM)
    //Used In: when Editing existing Dispersion </summary>
    var objDisp = $("#divData").data("DispersionData");
    var totalQty = 0.0;
    // var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
    //  var editPrd = $("[id$=EditProduct]").val();

    for (var i in objDisp.Materials) {
        //   objDisp.Materials[i].QTY_BASE = (objDisp.Materials[i].DSD_QTY_PERC * objDisp.DSP_QUANTITY) / 100;
        objDisp.Materials[i].QTY_BASE = (objDisp.Materials[i].DSD_QUANTITY / objDisp.Materials[i].CONV_FACT);

        objDisp.Materials[i].DSD_QTY_PERC = (Round((objDisp.Materials[i].QTY_BASE * 100) / objDisp.DSP_QUANTITY, dispersionDecimal)).toFixed(dispersionDecimal);

    }
    $("#divData").data("DispersionData", objDisp);
}

//method to check whether given two numeric values are equal considering the significant error
function isEqual(from, to) {
    var diff = from - to;
    if (Math.abs(diff) > SignificantFigure) {
        return false;
    }
    else {
        return true;
    }
}

function RecalculatePercentage() {
    ///<summary>Recalculate the percentages of all materials after changing any material details</summary>
    ////used In : Add/Edit/Delete material,Load for Editing existing Dispersions
    var objDisp = $("#divData").data("DispersionData");
    var totalQty = 0.0;
    var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
    var editPrd = $("[id$=EditProduct]").val();

    for (var i in objDisp.Materials) {
        if (editPrd != objDisp.Materials[i].DSD_ITEM) {//if editing a row skipp its quantitiy
            totalQty += objDisp.Materials[i].QTY_BASE;
        }
    }
    //if (Round(totalQty,3) > Round(disprQty,3)) {
    /*
    //if two values are significantly not equal and total quantity is greater than dispersion quantity
    if (!isEqual(totalQty, disprQty) && totalQty > disprQty) {

        GrandScriptUtils.ShowModal(DispersionMaster.QuantityExceeded, DispersionMaster.InformationTitle, DispersionMaster.Unsuccess);
        return false;

    }
    else {//if not exceeded caluculate % based on actual quantity
        for (var i in objDisp.Materials) {
            //TotalQty += ObjDisp.Materials[i].QTY_BASE;
            objDisp.Materials[i].DSD_QTY_PERC = objDisp.Materials[i].QTY_BASE == null ? 0 : (Round(objDisp.Materials[i].QTY_BASE * 100 / disprQty, dispersionDecimal)).toFixed(dispersionDecimal);
        }
        //assign new quantiy to text box
        percAvailable = Round(100 - (totalQty * 100 / disprQty), dispersionDecimal);
    }*/


    for (var i in objDisp.Materials) {
        //TotalQty += ObjDisp.Materials[i].QTY_BASE;
        objDisp.Materials[i].DSD_QTY_PERC = objDisp.Materials[i].QTY_BASE == null ? 0 : (Round(objDisp.Materials[i].QTY_BASE * 100 / disprQty, dispersionDecimal)).toFixed(dispersionDecimal);
    }
    //assign new quantiy to text box
    percAvailable = Round(100 - (totalQty * 100 / disprQty), dispersionDecimal);
    //save object
    $("#divData").data("DispersionData", objDisp);
    //ValidateQuantity();
    //bind grid
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, objDisp.Materials);
    return true;
}
 
function CalculatePercentage() {
    ///<summary>method to calculate the material percentage
    if ($("input[id$=DSP_QUANTITY]").val() == "") {
        return 0;
    }
    if ($("input[id$=DSD_QUANTITY]").val() == "") {
        return 0;
    }

    return Round((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100) / (parseFloat($("input[id$=DSP_QUANTITY]").val()) * ConversionFactor), dispersionDecimal);
}

function GetConverterdQuantity() {
    ///<summary>convert to base dispersion quantity uom
    return (parseFloat($("input[id$=DSD_QUANTITY]").val()) / (ConversionFactor));
}


function GetConversionFactor(from, to, callBack) {
    ////<summary>method to get the conversion factor</summary>
    ////<param "from">From UOM PK </param>
    ////<param "to">To UOM PK </param>
    if (from == null) {
        from = $("[id$=DSP_QTY_UOM]").val();

    }
    if (to == null) {
        to = $("[id$=DSD_QTY_UOM]").val();
    }
    if (from == 0 || to == 0) {
        return false;
    }
    $.get(DispersionMaster.GetConcversionFactors + from + "&UOMTo=" + to, function (data) {

        if (data != "" && data != "-1") {

            ConversionFactor = parseFloat(data).toFixed(dispersionDecimal);
            //            $("[id$=ImageButton1]").attr("disabled", '');
            //return ((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100*ConversionFactor ) / parseFloat($("input[id$=DSP_QUANTITY]").val()));
            if (typeof callBack == "function") {//call back function to be called after data arrives
                callBack();
            }
        }

        else {

            // no conversion factor exists cannot add material
            GrandScriptUtils.ShowModal(DispersionMaster.SelectAnotherUOM, DispersionMaster.ConfirmationMsg);
            // $("[id$=ImageButton1]").attr("disabled", "disabled");
            $("[id$=DSD_ITEM]").val("0");
            ConversionFactor = 1;
            return false;



        }
    });
}

//function ValidateQuantity() {
//    ///<summary>Validate quantity against stock</summary>
//    var perc = CalculatePercentage()
//    if (perc.toString() == 'NaN' || perc.toString() == 'Infinity') {
//        perc = 0;
//    }
//    if (percAvailable.toString() == 'NaN' || percAvailable.toString() == 'Infinity') {
//        percAvailable = 0;
//    }
//    $('#tdPercentage').html(isNaN(perc) ? 0 : perc.toFixed(dispersionDecimal) + " / " + percAvailable.toFixed(dispersionDecimal))//Round(perc,3)
//    if (Round(perc, dispersionDecimal) <= Round(percAvailable, dispersionDecimal) && perc != 0) {

//        // $("[id$=ImageButton1]").attr("disabled", '');
//        $("#tdPercentage").css({ "color": "#506c92", "visibility": "visible" });

//    }
//    else {
//        //   $("[id$=ImageButton1]").attr("disabled", "disabled")
//        $("#tdPercentage").css({ "color": "red", "visibility": "visible" });
//    }

//}

///#endregion



///#region data management


function AddDispersionMaterials() {
    //<summary>function used to add Materials details to Dispersion</summary>
    //Add Validation for Material Details by setting mode as 2

    AddValidations(2);

    /*var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
    var qtyBase = GetConverterdQuantity();
    //var num = Round(qtyBase == null ? 0 : qtyBase * 100 / disprQty, dispersionDecimal)
    var num = qtyBase == null ? 0 : qtyBase * 100 / disprQty;
    if (num == 0) {
        $("#tdPercentage").css({ "color": "red", "visibility": "visible" });
        return false;
    }
    */
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("DispersionData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var flag = true;
        var perc = 0;
        //Loop used to check the Material already added in the order List
        if (isNaN(parseFloat($("input[id$=DSD_QUANTITY]").val())) || parseFloat($("input[id$=DSD_QUANTITY]").val()) == 0) {
            return false;
        }
        if (parseInt(editProduct) == 0) {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].DSD_ITEM == $("[id$=MaterialPK]").val() && ObjDisp.Materials[i].DSD_ITEM_TYPE == $("[id$=MaterialCategoryPK]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].DSD_ITEM == $("[id$=MaterialPK]").val() && parseInt(editProduct) != ObjDisp.Materials[i].DSD_ITEM && ObjDisp.Materials[i].DSD_ITEM_TYPE == $("[id$=MaterialCategoryPK]").val()) {
                    flag = false;
                    break;
                }
                if (parseInt(editProduct) == ObjDisp.Materials[i].DSD_ITEM) {
                    obj = ObjDisp.Materials[i];
                }
            }
        }
        //add material to the list
        if (flag) {
            //obj.ITEM_CATAGORY_NAME = $("[id$=MaterialCatagory] option:selected").text();
            //obj.ITEM_CATAGORY = $("[id$=MaterialCatagory] option:selected").val();
            obj.DSD_ITEM_TYPE_TEXT = $("[id$=MaterialCategory]").val();//$("[id$=MaterialCatagory] option:selected").text();
            obj.DSD_ITEM_TYPE = $("[id$=MaterialCategoryPK]").val();
            obj.DSD_ITEM = parseInt($("[id$=MaterialPK]").val()); //parseInt($("select[id$=DSD_ITEM]").val());
            obj.ITM_TEXT = $("[id$=DSD_ITEM]").val(); //$("[id$=DSD_ITEM] option:selected").text();
            //            obj.ITEM_NAME = $("[id$=MaterialName]").html();
            obj.DSD_QUANTITY = parseFloat($("input[id$=DSD_QUANTITY]").val());
            obj.DSD_QTY_UOM = $("[id$=DSD_QTY_UOM]").val(); //$("select[id$=DSD_QTY_UOM] option:selected").val();
            obj.UOM_CODE = $("[id$=DSD_QTY_UOM_TEXT]").html(); //$("select[id$=DSD_QTY_UOM] option:selected").text();
            obj.QTY_UOM_NAME = $("[id$=DSD_QTY_UOM_TEXT]").html(); //  $("select[id$=DSD_QTY_UOM] option:selected").text();
            obj.DSD_QTY_PERC = CalculatePercentage();
            obj.CONV_FACT = ConversionFactor;
            obj.QTY_BASE = GetConverterdQuantity();
            obj.DSD_QUANTITY_TEXT = parseFloat($("input[id$=DSD_QUANTITY]").val()) + "(" + obj.QTY_UOM_NAME + ")";
            if (parseInt(editProduct) == 0) {
                ObjDisp.Materials.push(obj);
            }
            $("#divData").data("DispersionData", ObjDisp);
            //reset edit pk 
            $("[id$=EditProduct]").val("0")
            if (RecalculatePercentage()) {
                GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, ObjDisp.Materials);
                //if atleast one material added dont allow to edit the base quantity
                if (ObjDisp.Materials.length > 0) {
                    $("[id$=DSP_QUANTITY]").attr("disabled", "disabled");
                    $("[id$=DSP_QTY_UOM]").attr("disabled", "disabled");
                }
                else {
                    $("[id$=DSP_QUANTITY]").attr("disabled", "false");
                    $("[id$=DSP_QTY_UOM]").attr("disabled", "false");
                }
                
                ClearMaterialDetails();
            }
            else {
                $("[id$=EditProduct]").val(editProduct);
                if (parseInt(editProduct) == 0)
                    ObjDisp.Materials.pop();
            }
            
        }
        else {
            GrandScriptUtils.ShowModal(DispersionMaster.MaterialAlreadyAdded, DispersionMaster.InformationTitle);
        }
        return false;
    }
}





function ClearMaterialDetails() {
    ///<Summary>Clear material input fields<summary>
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0); //$("select[id$=DSD_ITEM]").val('0');
    FillMaterialCategoryAutoComplete();
    FillMaterialAutoComplete();
    $("input[id$=DSD_QUANTITY]").val('');
    $("[id$=DSD_QTY_UOM_TEXT]").html("");
    $("[id$=DSD_QTY_UOM]").val('0');
    $("input[id$=EditProduct]").val('0');
    $("[id$=MaterialName]").html("");   
    //$("[id$=DSP_TYPE]").val('0');
    //$("[id$=DSP_CHECK_LIST_HDR]").val('0');
   // $("#tdPercentage").html('');
    ConversionFactor = 1;
//    var drpItemID = $("select[id$=DSD_ITEM]").attr("id");
//    GrandScriptUtils.FillDropDown(drpItemID, null, true, true);
}



function GridHandler(tr, command) {
    ///<summary>Grid Handler for grdDispersionDetails Catch all the grid events in this function </summary>
    ///<param "tr">Current row jquery object</param>
    ///<param "command">command to be processed</param> 
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            deleteMaterialPk = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");
            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, DispersionMaster.ConfirmationMsg, DispersionMaster.DeleteMaterial, true);
            // DeleteDetails(tr);
            return false;
            break;
        case "edit":
            $("[id$=btnAddNew]").hide();
            $("[id$=imbAdd]").hide();
            $("[id$=imdReset]").hide();
            $("[id$=btnSave]").show();
            $("[id$=imbSave]").show();
            $("[id$=imbCancel]").show();
            FillMaterialDetails(tr);
            return false;
            break;
        default:
            alert(DispersionMaster.DefaultActionMsg);
            return false;
            break;
    }
}


function DeleteDetails(tr) {
    ///<summary>For delete the item in the grid - Dispersion Details</summary>
    //var materialID = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");
    var materialID = deleteMaterialPk;
    var ObjDisp = $("#divData").data("DispersionData");
    for (var i in ObjDisp.Materials) {
        if (ObjDisp.Materials[i].DSD_ITEM == materialID) {
            //Will delete the Material details
            ObjDisp.Materials.splice(i, 1);
            break;
        }
    }
    $("#divData").data("DispersionData", ObjDisp);
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, ObjDisp.Materials);
    //Used to Show the Material  details when the Materials in Dispersion is 0
    if (ObjDisp.Materials.length == 0) {
        //Will insert the selection tr  into the  MaterialInsert table and show the MaterialInsert Table
        $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
        $("#Order").show();
        //$("#MaterialInsert").css({ "display": "block", "visibility": "visible" });

        $("[id$=DSP_QUANTITY]").attr("disabled", "");
        $("[id$=DSP_QTY_UOM]").attr("disabled", "");


    }

    RecalculatePercentage();
    //$("#tdPercentage").html('');
}


function FillMaterialDetails(tr) {
    ////<summary>fill material details for edit</summary>
 
    var catagoryId = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM_TYPE", $(tr).parent().parent().attr("id"));
    var materialId = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", $(tr).parent().parent().attr("id"));
    var materialUOM = GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", $(tr).parent().parent().attr("id"));
    $("[id$=EditProduct]").val(materialId);    

    $("input[id$=DSD_QUANTITY]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_QUANTITY", $(tr).parent().parent().attr("id")));
    $("[id$=DSD_QTY_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=EditProduct]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", $(tr).parent().parent().attr("id")));
    //    $("[id$=MaterialName]").text(GrandGrid.Utilities.GetColumnValue(tr, "ITEM_NAME", $(tr).parent().parent().attr("id")));
    ConversionFactor = parseFloat(GrandGrid.Utilities.GetColumnValue(tr, "CONV_FACT", $(tr).parent().parent().attr("id")));
    //$("#tdPercentage").html()
    $("#tdPercentage").html(tr.find("td:eq(9)").html());
    $("input[id$=IsEdit]").val("true");
   // $("select[id$=DSD_ITEM]").focus();
    RecalculatePercentage();
    // FillCategoryMaterials(catagoryId, materialId);
//    FillMaterialNames(GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", $(tr).parent().parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", $(tr).parent().parent().attr("id")));

    FillMaterialUOMs(materialId, materialUOM);
    FillMaterialCategoryAutoComplete();
    FillMaterialAutoComplete();
    $("[id$=MaterialCategoryPK]").val(catagoryId);
    $("[id$=MaterialCategory]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM_TYPE_TEXT", $(tr).parent().parent().attr("id")));
    $("[id$=MaterialPK]").val(materialId);
    $("[id$=DSD_ITEM]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITM_TEXT", $(tr).parent().parent().attr("id")));
}


function SavePage() {
    /////<summary>save dispersion to the database</summary>
    RemoveValidations();
    //Add Validation for Dispersion header Details by setting mode as 1
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        $("[id$=DSP_QUANTITY]").attr("disabled", '');
        $("[id$=DSP_QTY_UOM]").attr("disabled", '');
        var selectedtems = JSON.stringify(GetSelectedStores());       
        var ObjDisp = $("#divData").data("DispersionData");
        if (ObjDisp.Materials.length > 0) {

            // Check if the dispersion have alteast 1 material added

            //check component quantity sum meet original required quanity
           /* var totalQty = 0.0;
            var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
            for (var i in ObjDisp.Materials) {
                totalQty += ObjDisp.Materials[i].QTY_BASE;
            }

            if (disprQty > totalQty) {
                GrandScriptUtils.ShowModal(DispersionMaster.QuantityNotReached, 'Translate(Information)', "Unsuccess");

                return false;
            }

            //  if (Round(totalQty, 3) < Round(disprQty, 3)) {
            if (!isEqual(totalQty, disprQty) || totalQty < disprQty) {
                GrandScriptUtils.ShowModal(DispersionMaster.QuantityNotReached, 'Translate(Information)', "Unsuccess");

                return false;
            }*/
            var selectedtems = JSON.stringify(GetSelectedStores());
            ObjDisp.MaterialStores = selectedtems;
            $("#[id*=Materialstores]").val(selectedtems);
            $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
            if ($(document.forms[0]).valid()) {
                var ObjDisp = $("#divData").data("DispersionData");              
                //Assigning the Material details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=DispersionDetailsList]").val(JSON.stringify(ObjDisp.Materials));
                $("[id$=ConversionList]").val(JSON.stringify(ObjDisp.ConversionList));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                //ajax save request
                $.ajax({
                    type: "post",
                    url: DispersionMaster.SavePage,
                    data: jSonString,
                    contentType: "application/json",
                    dataType: "text",
                    success: function (data) {

                        if (parseInt(data) > 0) {
                            $("[id$=hdfDispersionID]").val(data);
                            if ($("[id$=hdfIsFinalTab]").val() == 1)//THis is the final tab
                            {
                                GrandScriptUtils.ShowModal(DispersionMaster.StoreMappSaveMessage, DispersionMaster.InformationTitle, DispersionMaster.Saved);
                            }
                            else {
                                GrandScriptUtils.ShowModal(DispersionMaster.SavedSuccess, DispersionMaster.InformationTitle, DispersionMaster.Saved);                               
                            }
                        }
                        else {
                            var msgtxt;
                            if (parseInt(data) == 0)
                                msgtxt = DispersionMaster.AlreadyExists;
                            else if (parseInt(data) == -5)
                                msgtxt = DispersionMaster.DeletedRecord;
                            else if (parseInt(data) == -2)
                                msgtxt = DispersionMaster.DispersionPageResName + DispersionMaster.EditUsedByAnotherUser;
                            else if (parseInt(data) < 0)
                                msgtxt = DispersionMaster.ActionFailedMsg;
                            GrandScriptUtils.ShowModal(msgtxt, DispersionMaster.InformationTitle, DispersionMaster.Failed);
                        }
                    }
                });

            }
        }
        else {
            GrandScriptUtils.ShowModal('Translate(PleaseSelectMaterialDetails)', 'Translate(Information)');

        }
    }
    return false;
}

///#endregion

///#endregion

///#region Validation

///<Summary>Add validations to controls<summary>
function AddValidations(mode) {
    RemoveValidations();
    //Mode = 1 represents the validation for Dispersion  Header Details
    if (mode == "1") {
        $('input[id$=DSP_NAME]').rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: DispersionMaster.EnterDispersionName }
        });
//        $('input[id$=DSP_EXP_TIME]').rules("add", {
//            required: true,
//            digits: true,
//            maxlength: 3,
//            NonZero: true,
//            messages: { required: DispersionMaster.EnterExpiryTime }
//        });
        $('input[id$=DSP_QUANTITY]').rules("add", {
            required: true,
            ThreeDecimal: true,
            //number: true,
            // maxlength: 10,
            NonZero: true,
            messages: { required: DispersionMaster.EnterQuantity }
        });
//        $('input[id$=DSP_PREP_TIME]').rules("add", {
//            required: true,
//            digits: true,
//            maxlength: 3,
//            NonZero: true,
//            messages: { required: DispersionMaster.EnterPreparationTime }
//        });
        $('input[id$=DSP_CODE]').rules("add", {
            required: true,
            maxlength: 50,
            messages: { required: DispersionMaster.EnterDispersionCode }
        });

        //        $('select[id$=DSP_MACHINE_TYPE]').rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: DispersionMaster.SelectMachineType }
        //        });
        $('select[id$=DSP_QTY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Quantity UOM" }
        });
        if ($('[id$=DSP_EXP_TIME]').val() != '' && $('[id$=DSP_EXP_TIME]').val() != 0) {
            $('select[id$=DSP_EXP_TM_UOM]').rules("add", {
                selectNone: true,
                messages: { selectNone: "Select Expiry Time format" }
            });
        }
        if ($('[id$=DSP_PREP_TIME]').val() != '' && $('[id$=DSP_PREP_TIME]').val() != 0) {
            $('select[id$=DSP_PREP_TM_UOM]').rules("add", {
                selectNone: true,
                messages: { selectNone: "Select Preparation Time format" }
            });
        }
        $("[id$=txtDspItemCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItemCategory)" }
        });


    }
    //Mode =  2 represents the validation for Dispersion Material Details
    else if (mode == "2") {
        $("[id$=MaterialCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItemCategory)" }
        });
        $("[id$=DSD_ITEM]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItem)" }
        });
        $('input[id$=DSD_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 10,
            NonZero: true,
            FourDecimal: true,
            messages: { required: DispersionMaster.EnterQuantity, NonZero: DispersionMaster.QuantityGreaterthanZero }
        });
        //        $('select[id$=DSD_QTY_UOM]').rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: DispersionMaster.EnterUOM }
        //        });
//        $('select[id$=MaterialCatagory]').rules("add", {
//            selectNone: true,
//            messages: { selectNone: DispersionMaster.EnterCatagory }
//        });
        $('input[id$=DSP_QUANTITY]').rules("add", {
            required: true,
            ThreeDecimal: true,
            //number: true,
            // maxlength: 10,
            NonZero: true,
            messages: { required: DispersionMaster.EnterQuantity }
        });
    }
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {

    $(document.forms[0]).validate().resetForm();
    $('input[id$=DSP_NAME]').rules("remove");
    $('input[id$=DSP_EXP_TIME]').rules("remove");
    $('input[id$=DSP_QUANTITY]').rules("remove");
    $('input[id$=DSP_PREP_TIME]').rules("remove");
    //$('select[id$=DSP_MACHINE_TYPE]').rules("remove");
    //$('select[id$=DSD_ITEM]').rules("remove");
    $('input[id$=DSD_QUANTITY]').rules("remove");
    $('select[id$=DSP_QTY_UOM]').rules("remove");
    $('select[id$=DSP_EXP_TM_UOM]').rules("remove");
    $('select[id$=DSP_PREP_TM_UOM]').rules("remove");
    //$('select[id$=DSD_QTY_UOM]').rules("remove");
    //$('select[id$=MaterialCatagory]').rules("remove");
    $('input[id$=DSD_ITEM]').rules("remove");
    $('input[id$=MaterialCategory]').rules("remove");
    $('input[id$=txtDspItemCategory]').rules("remove");


}
function RemoveFormValidations() {
    //<summary>function used Remove validation </summary>

    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

///#endregion


///#region----------Methods for Listing tab----------------------------------------


function BindGrid() {
    ///<summary>Method to bind the main grid</summary>   
    var ajaxUrl = DispersionMaster.BindGrid + $("[id$=SearchType]").val() + "&SearchValue=" + encodeURIComponent($("[id$=SearchValue]").val()) + "&SBUPk=" + $("[id$=BizUnitPk]").val();
    $("#grdDispersionList").removeAttr("ajaxurl")
    $("#grdDispersionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionList");
    GrandGrid.MakeGrid($("#grdDispersionList"));
}


function AddNew() {
    ///<summary>event triggered when add new button click</summary>
    $("#divData").show();
    $("#divListing").hide();
    $("[id$=btnAddNew]").hide();
    $("[id$=imbAdd]").hide();
    $("[id$=imdReset]").hide();
    $("[id$=btnSave]").show();
    $("[id$=imbSave]").show();
    $("[id$=imbCancel]").show();
    $("id$=DispersionID").val("0");
    $("[id$=DSP_CODE]").focus();
    EditFlag = false;
    $("[id$=btnSave]").show();
    $("#Order").show();

    var dummyObj = new Object();
//    GrandScriptUtils.FillDropDown($("[id$=DSD_ITEM]").attr("id"), dummyObj, true, true);
    //GrandScriptUtils.FillDropDown($("[id$=DSD_QTY_UOM]").attr("id"), dummyObj, true, true);
    // GrandScriptUtils.FillDropDown($("[id$=DSP_CHECK_LIST_HDR]").attr("id"), dummyObj, true, true);
    //initialize grid
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, dummyObj);
    $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
    //$("#MaterialInsert").show();
    //$("#MaterialInsert").css({ "display": "block", "visibility": "visible" });  
    return false;
}



function AfterSelect() {
    ///<summary>To handle events after selecting a value from auto complete</summary>
    //filling gridview after entering search value.
    BindGrid();
}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdDispersionDetails") {
        if (tdset == "") {
            tdset = $("#MaterialInsert").find("tr:eq(1)");
        }
        $("#Order").hide();
       // $("#MaterialInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdDispersionDetails").show();
        if (EditFlag != "true") {
            $(tdset).insertBefore($("#grdDispersionDetails").find("tr:eq(1)"));
        }
        var colIndex1 = 0;
        var colVal = 0;
        /*$("#grdDispersionDetails").find("tr:has(th)").each(function (index) {
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
        });*/

        $("#grdDispersionDetails tr:has(td)").each(function (index) {
            if (EditFlag == "true") {//Action To perform for the logged in user
                $(this).find("td:last input[id$=imbEdit]").hide();
                $(this).find("td:last input[id$=imbDelete]").hide();
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
                colVal = parseFloat(colVal).toFixed(dispersionDecimal);
                $(this).find("td:eq(" + colIndex + ")").html(colVal);
                $(this).find("td:eq(" + colIndex + ")").css('text-align', 'right');
            }
            else
                $(this).find("td:eq(" + colIndex + ")").html('');

           /* colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                colVal = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
                colVal = (colVal == "null") ? "" : colVal;
                $(this).find("td:eq(" + colIndex + ")").html(colVal);
                $(this).find("td:eq(" + colIndex + ")").css('text-align', 'right');
            }*/
        });
    }

//    if (grdID == "grdChecklistDetails") {
//        var ColIndex = 0;
//        var Col = 0;
//        $("#grdChecklistDetails tr:has(td)").each(function (index) {
//            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_VALUE", grdID);
//            Col = GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID);
//            if (ColIndex != null && Col != "null") {
//                $(this).find("td:eq(" + ColIndex + ")").html(Col);
//            }
//            else {
//                $(this).find("td:eq(" + ColIndex + ")").html("");
//            }
//            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_DESC", grdID);
//            Col = GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID);
//            if (ColIndex != null && Col != "null") {
//                $(this).find("td:eq(" + ColIndex + ")").html(Col);
//            }
//            else {
//                $(this).find("td:eq(" + ColIndex + ")").html("");
//            }


//        });
//    }
    if (grdID == "grdDispersionList") {
        $("#grdDispersionList tr:has(th)").each(function (index) {
            var ColIndex = 0;
            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSP_QUANTITY", grdID);
            if (ColIndex != null) {
                $(this).find("th:eq(" + ColIndex + ")").css('text-align', 'right');
            }
        });

        $("#grdDispersionList tr:has(td)").each(function (index) {
            var ColIndex = 0;
            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSP_TYPE_TEXT", grdID);
            if (ColIndex != null) {
                if ($(this).find("td:eq(" + ColIndex + ")").html() == "null" || $(this).find("td:eq(" + ColIndex + ")").html() == null || $(this).find("td:eq(" + ColIndex + ")").html() == undefined) {
                    $(this).find("td:eq(" + ColIndex + ")").html('');
                }
            }
            ColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSP_QUANTITY", grdID);
            if (ColIndex != null) {
                var colValue = GrandGrid.Utilities.GetColumnValue($(this), "DSP_QUANTITY", grdID);
                var Qty = colValue.split('(');
                Qty[0] = parseFloat(Qty[0]).toFixed(dispersionDecimal);
                $(this).find("td:eq(" + ColIndex + ")").html(Qty[0] + '(' + Qty[1]);
                $(this).find("td:eq(" + ColIndex + ")").css('text-align', 'right');
            }
            else
                $(this).find("td:eq(" + ColIndex + ")").html('');

            var AlreadyExists = GrandGrid.Utilities.GetColumnValue($(this), "DSP_FLAG", grdID);
            if (AlreadyExists == "true") {
                // $(this).find("td:last input[id$=ImageButton2]").hide();
                $(this).find("td:last input[id$=ImageButton3]").hide();
                $(this).find("td:last input[id$=imbView]").show();
            }
            else {
                //$(this).find("td:last input[id$=ImageButton2]").show();
                $(this).find("td:last input[id$=ImageButton3]").show();
                $(this).find("td:last input[id$=imbView]").hide();
            }
        });
    }
}


function GridHandlerMain(tr, command) {
    ///<summary>Grid Handler for grdDispersionList  Catch all the grid events in this function </summary>
    switch (command.toString().toLowerCase()) {
        // To Delete Details       
        case "delete":
            // DispersionID = GrandGrid.Utilities.GetColumnValue(tr, "Rmdp", $(tr).parent().attr("id"));
            DispersID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(DispersionMaster.DoyouWantToDelete, DispersionMaster.ConfirmationMsg, DispersionMaster.DeleteDispersionCmd, true)
            break;

        // To Edit Details   
        case "edit":
            AddNew();
            FillDetails(tr);
            EditFlag = false;
            $("[id$=imbSave]").show();
            break;
        case "view":
            AddNew();
            FillDetails(tr);
            break;

        default:
            alert(DispersionMaster.DeleteDispersion);
            break;
    }
    return false;

}

var EditFlag = false;

function FillDetails(tr) {
    ///<summary>fill Dispersion</summary>
    //Get OrderID From tr - For Pass this as QueryString
    var DispersionID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
    $("[id$=hdfDispersionID]").val(DispersionID);
    EditFlag = GrandGrid.Utilities.GetColumnValue(tr, "DSP_FLAG", $(tr).parent().parent().attr("id"));
    if (EditFlag == "true") {
        $("[id$=btnSave]").hide();
        $("[id$=imbSave]").hide();
        //$("[id$=imbCancel]").hide();
    }    
    var objDisp = new Object();
    $.get(DispersionMaster.GetDispersionDetail + DispersionID, function (data) {

        $("#divData").data("DispersionData", data);

        FillDispersionDetails();

    });   
}


function FillDispersionDetails() {
    ///<summary>Used to fill Dispersion Details for editing</summary>
    RemoveValidations();
    DispersJson = $("#divData").data("DispersionData");
    ///calucate missing fields
    $("input[id$=ITM_PK]").val(DispersJson.DSP_ITEM);
    //Fill inputs
    $("input[id$=DSP_NAME]").val(DispersJson.DSP_NAME);
    if (parseFloat(DispersJson.DSP_EXP_TIME) > 0)
    $("input[id$=DSP_EXP_TIME]").val(Round(DispersJson.DSP_EXP_TIME, dispersionDecimal));
    $("input[id$=DSP_QUANTITY]").val(Round(DispersJson.DSP_QUANTITY, dispersionDecimal));
    if (parseFloat(DispersJson.DSP_PREP_TIME) > 0)
        $("input[id$=DSP_PREP_TIME]").val(Round(DispersJson.DSP_PREP_TIME, dispersionDecimal));
   // $("select[id$=DSP_TYPE]").val(DispersJson.DSP_TYPE);
    $("select[id$=DSP_PREFIX]").val(DispersJson.DSP_PREFIX);
    //$("select[id$=DSP_MACHINE_TYPE]").val(DispersJson.DSP_MACHINE_TYPE);
//    $("select[id$=DSP_MACHINE]").val(DispersJson.DSP_MACHINE);
    $("select[id$=DSP_EXP_TM_UOM]").val(DispersJson.DSP_EXP_TM_UOM);
    $("select[id$=DSP_QTY_UOM]").val(DispersJson.DSP_QTY_UOM);
    $("select[id$=DSP_PREP_TM_UOM]").val(DispersJson.DSP_PREP_TM_UOM);
   // $("select[id$=DSP_CHECK_LIST_HDR]").val(DispersJson.DSP_CHECK_LIST_HDR);
    $("input[id$=DSP_PK]").val(DispersJson.DSP_PK);
    $("[id$=DSP_CODE]").val(DispersJson.DSP_CODE);
    $("select[id$=DSP_QTY_UOM]").attr("disabled", "disabled");
    $("[id$=DSP_QUANTITY]").attr("disabled", "disabled");
    percAvailable = 0.00; //set pecentage available as 0 for edit 
    $("[id$=LAST_MOD_DT]").val(DispersJson.DSP_MOD_DT);
    $("[id$=DSP_DESC]").val(DispersJson.DSP_DESC);

    $("[id$=DSP_ITM_CATEGORY]").val(DispersJson.DSP_ITM_CATEGORY); //$("select[id$=DSP_ITM_CATEGORY]").val(DispersJson.DSP_ITM_CATEGORY);
    $("[id$=txtDspItemCategory]").val(DispersJson.DSP_ITM_CATEGORY_TEXT); //$("select[id$=DSP_ITM_CATEGORY]").attr("disabled", "disabled"); 
    $("[id$=txtDspItemCategory]").next("a").remove();   
    $("[id$=txtDspItemCategory]").attr("disabled", "disabled");
    

    $("[id$=imbViewCag]").attr("disabled", "disabled"); 
    // Check DispersJson.Materials is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign OrderList Details to that Array, and then push Array to DispersJson.Materials
    if (!($.isArray(DispersJson.Materials))) {
        var objArray = DispersJson.Materials;
        DispersJson.Materials = new Array();
        DispersJson.Materials.push(objArray);
    }
    if (!($.isArray(DispersJson.ConversionList))) {
        var objArray = DispersJson.ConversionList;

        DispersJson.ConversionList = new Array();
        if (objArray != null) {
            DispersJson.ConversionList.push(objArray);
        }
    }
    $("#divData").data("DispersionData", DispersJson);
    CalculateBaseQuantity();
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersJson.Materials);
    //AfterGridBind();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
    ShowConversionDtls()//adding conversion details
    FillStoreTree($("[id$=ITM_PK]").val());
    //enabling store mapping tab in edit mode
    $("[id$=tabs]").tabs("enable", 1);
}


function DeleteDispersion() {
    ///<summary>Delete Dispersion</summary>
    var msgtxt;

    $.get(DispersionMaster.DeleteDispersion + DispersID, function (data) {

        //Check Order Deleted Succesfully or Not - 1-Sucess 0-Fail
        if (parseInt(data) > 0)
            msgtxt = DispersionMaster.DeleteSuccess;
        else if (parseInt(data) == 0)
            msgtxt = DispersionMaster.Alreadyasigned;
        else
            msgtxt = DispersionMaster.ActionFailedMsg;
        // Show MeesageBox For Order Delete Status
        GrandScriptUtils.ShowModal(msgtxt, DispersionMaster.InformationTitle, DispersionMaster.Deleted);


    });

    return false;
}


function ClearPage() {
    ///<summary>clear page<summary>
    //$('#updateProgress').show();
    ResetPage();
    //$('#updateProgress').hide();
    //    $("#divData").hide();
    //    $("#divListing").show();
    //    BindGrid(); 
    return false;
}


function ResetPage() {
    ///<summary>function Used to Reset Page</summary>
    //Reseting all input controls in the page

    //    sarath
    //    var cou = 0;
    //    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button],input[type=submit])").each(function () {
    //        var idval = $(this).attr("id");
    //        //Avoid Order ID And Set the value as 0 
    //        var idvalbizUnit = $("[id$=BizUnitPk]").attr("id");
    //        if (idval.search("OrderID") != -1) {
    //            $(this).val('0');
    //        }
    //        if (idval.search("EditProduct") != -1) {
    //            $(this).val('0');
    //        }
    //        //Avoid UserPk to get the value of log in user
    //        else if (idval.search("UserPk") == -1) {
    //            //Avoid bizUnit set null. If it null gridview not Fill
    //            if (idval != idvalbizUnit) {
    //                $(this).val("");
    //            }
    //        }
    //        });



    $(document.forms[0]).find("textarea").each(function () {
        $(this).val('');
    });

    $("select[id$=DSP_EXP_TM_UOM]").val('0');
    $("select[id$=DSP_QTY_UOM]").val('0');
    $("select[id$=DSP_PREP_TM_UOM]").val('0');
    //$("select[id$=DSP_MACHINE_TYPE]").val('0');
    $("[id$=DSP_CODE]").val('');
    $("[id$=DSP_EXP_TIME]").val('');
    $("[id$=DSP_NAME]").val('');
    $("[id$=DSP_PREP_TIME]").val('');   
    $("[id$=DSP_QUANTITY]").val('');
    $("[id$=DSP_PK]").val('0');
   // $("[id$=DSP_TYPE]").val('0');
    $("[id$=DSP_PREFIX]").val('0');
    $("[id$=DSP_DESC]").val('');
    $("[id$=DSP_ITM_CATEGORY]").val(0); // $("select[id$=DSP_ITM_CATEGORY]").val('0');
    FillHeaderMaterialCategoryAutoComplete();
    $("[id$=tabs]").tabs();
    $("[id$=tabs]").tabs("select", 0);
    $("[id$=tabs]").tabs("disable", 1);
    $("[id$=tabs]").tabs("enable", 0);
    $("#divData").hide();
    $("#divListing").show();
    $("[id$=btnAddNew]").show();
    $("[id$=imbAdd]").show();
    $("[id$=imdReset]").show();
    $("[id$=btnSave]").hide();
    $("[id$=imbSave]").hide();
    $("[id$=imbCancel]").hide();
    $("[id$=SearchValue]").hide()
    $("[id$=imbSearch]").hide();
    $("[id$=SearchValue]").val("");
    $("[id$=SearchType]").val("0");
    //$('#tdPercentage').html("");
    $("[id$=ImageButton1]").attr("disabled", '');
    $("[id$=hdfDispersionID]").val(0);
    $("input[id$=hdfIsFinalTab]").val('0');
    $("[id$=ITM_PK]").val('0')
    ClearMaterialDetails();
    percAvailable = 100.00;
    $(document.forms[0]).validate().resetForm();
    var ObjDisp = $("#divData").data("DispersionData");
    if (ObjDisp != null) {
        ObjDisp.Materials = new Array();

    }
    var ObjDisp = $("#divData").data("DispersionData");
    if (ObjDisp != null) {
        ObjDisp.ConversionList = new Array();

    }
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, ObjDisp.Materials);
    $("#divData").data("DispersionData", ObjDisp);
    $("[id$=DSP_QUANTITY]").attr("disabled", '');
    $("[id$=DSP_QTY_UOM]").attr("disabled", '');
    $("[id$=txtDspItemCategory]").attr("disabled", ''); //    $("[id$=DSP_ITM_CATEGORY]").attr("disabled", '');
    $("[id$=imbViewCag]").attr("disabled", '');
    RemoveValidations();
    BindGrid();
    $("#trvStores").find("input[type=checkbox]:checked").each(function () {
        $(this).attr('checked', false);
    });
    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>
    switch (command) {

        case "deleteMachine":
//            BindMachineTypeGrid();
//            ClearMachineTypeDetails();
//            //FillMachineType(0);
            break;
        case DispersionMaster.DeleteMaterial:
            DeleteDetails();
            break;
        case DispersionMaster.DeleteDispersionCmd:
            DeleteDispersion();
            break;
        case DispersionMaster.SaveMachineTypes:
            //SaveMachineType();
            break;
        case DispersionMaster.DeleteMachineTypeCMD:
//            DeleteMachineTypeDetails();
//            ClearMachineTypeDetails();
            break;
        case DispersionMaster.Deleted:
            BindGrid();
            break;
        case DispersionMaster.Saved:
            if ($("[id$=hdfIsFinalTab]").val() == 1)//THis is the final tab
            {
                ResetPage();
                SearchInit();
                SetSearchType();
            }
            else {
                var objDisp = new Object();
                $.get(DispersionMaster.GetDispersionDetail + $("[id$=hdfDispersionID]").val(), function (data) {

                    $("#divData").data("DispersionData", data);

                    FillDispersionDetails();

                });
                FillStoreTree($("[id$=ITM_PK]").val());
                $("[id$=tabs]").tabs("enable", 1);
                $("[id$=tabs]").tabs("select", 1);
                $("input[id$=hdfIsFinalTab]").val("1"); 
            }
            //ResetPage();
            //SearchInit();
            //SetSearchType();
            break;
        case DispersionMaster.DeleteConversion:
            DeleteConversionDetails();
            break;
        case DispersionMaster.DeleteMachineTypeCMD:
//            DeleteMachineTypeDetails();
            break;
        case DispersionMaster.DeleteMachineTypeSucessesCMD:
//            BindMachineTypeGrid();
//            ClearMachineTypeDetails();
            //FillMachineType();
            break;
    }
    return false;
}
///#endregion


///#region ----------  machine Type Section

//function SaveMachineType() {
//    //<summary>function To Save Machine Type Details </summary>
//    RemoveValidations();
//    AddMachineTypeValidations();
//    if ($(document.forms[0]).valid()) {
//        var msgTxt;
//        var jSonString = GrandScriptUtils.FormToJsonString("divMachineType");
//        $.post(DispersionMaster.SAVEMACHINETYPEURL + $("[id$=BizUnitPk]").val(), jSonString, function (data) {
//            // Check Machine Type Saved Successfully or Not - >0 Success ,0- Name Already Exists, <0 - Fail(Exception)
//            if (parseInt(data) > 0)
//                msgTxt = DispersionMaster.MachineTypeSaveMsg;
//            // Machine Code Already Exists Or not
//            else if (parseInt(data) == 0)
//                msgTxt = DispersionMaster.MachineTypeExists;
//            else if (parseInt(data) < 0)
//                msgTxt = DispersionMaster.ActionFailedMsg;
//            // Check The Machine Type  Saved Succesfully or Not
//            if (parseInt(data) > 0) {
//                //FillMachineType(data);
//                ClearMachineTypeDetails();
//                $("[id$=divMachineType]").dialog("close");
//                //$("select[id$=DSP_MACHINE_TYPE]").focus();
////                $("select[id$=DSP_MACHINE]").focus();
//                return false;
//            }
//            else {

//                GrandScriptUtils.ShowModal(msgTxt, DispersionMaster.InformationTitle, DispersionMaster.SaveMachineType);
//            }

//        });
//    }
//    return false;

//}

//function BindMachineTypeGrid() {
//    //<summary>function To Bind MachineType Details </summary>
//    var ajaxUrl = DispersionMaster.MACHINETYPEGRIDURL + $("[id$=BizUnitPk]").val();
//    $("#grdMachineTypeDtls").removeAttr("ajaxurl")
//    $("#grdMachineTypeDtls").attr("ajaxurl", ajaxUrl);
//    GrandGrid.Utilities.ResetGrid(true, "grdMachineTypeDtls");
//    GrandGrid.MakeGrid($("#grdMachineTypeDtls"));
//}


//function AddMachineTypeDetails() {
//    //<summary>function To Show Machine Type PopUp - For Entry  </summary>
//    ClearMachineTypeDetails();
//    $(document.forms[0]).validate().resetForm();
//    BindMachineTypeGrid();
//    //    $("#divMachineType").dialog("open");
//    //    $("#divMachineType").dialog({ width: 500, height: 350, resizable: true });
//    //    $("#divMachineType").css({ "min-height": "300", "margin-top": "25px" });
//    $("#divMachineType").dialog({ width: 550, height: 300, buttons: {} });
//    $("#divMachineType").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
//    return false;
//}


//function MachineTypeGridHandler(tr, command) {
//    ///<summary>Grid Handler - Machine Type Catch all the grid events in this function </summary>
//    /// <param name="tr"  type="Object">
//    ///     Specific Container and its controls
//    /// </param>
//    /// <param name="command"  type="Object">
//    ///     Specific Edit/Delete
//    /// </param>
//    switch (command.toString().toLowerCase()) {
//        // To Delete Details                                     
//        case DispersionMaster.MachineTypeDelete:
//            machineTypeID = GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineTypeID, $(tr).parent().parent().attr("id"));
//            // Do Confirmation.. Before Delete Details
//            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, DispersionMaster.ConfirmationMsg, DispersionMaster.DeleteMachineTypeCMD, true);
//            break;
//        // To Edit Details                                              
//        case DispersionMaster.MachineTypeEdit:
//            //FillMachineTypeDetails(tr);
//            break;
//        default:
//            alert(DispersionMaster.DefaultActionMsg);
//            break;
//    }
//    return false;
//}


//function FillMachineTypeDetails(tr) {
//    ///<summary> Function to Fill  Machine Type Details When Edit Details </summary>
//    /// <param name="tr"  type="Object">
//    ///     Specific Container and its controls
//    /// </param>
//    $("input[id$=MachineTypePK]").val(GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineTypeID, $(tr).parent().parent().attr("id")));
//    $("input[id$=MachineTypeName]").val(GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineName, $(tr).parent().parent().attr("id")));
//}


//function ClearMachineTypeDetails() {
//    ///<summary>Clear Machine Type Details </summary>
//    $("input[id$=MachineTypePK]").val(DispersionMaster.ValueZero);
//    $("input[id$=MachineTypeName]").val("");
//    //$("[id$=lblstarmachine]").hide();
//}


//function DeleteMachineTypeDetails() {
//    /<summary>Delete Vendor Details </summary>
//    var msgtxt;
//    $.get(DispersionMaster.DELETEMACHINETYPEURL + machineTypeID, function (data) {
//         Check Delete Success
//        if (parseInt(data) == 1)
//            msgtxt = DispersionMaster.MachineTypeDeleteMsg;
//         MachineType Already Assigned or not
//        else if (parseInt(data) == 0)
//            msgtxt = DispersionMaster.MachineTypeAssignedMsg;
//        else
//            msgtxt = DispersionMaster.ActionFailedMsg;
//        GrandScriptUtils.ShowModal(msgtxt, DispersionMaster.InformationTitle, DispersionMaster.DeleteMachineTypeSucessesCMD);

//    });
//    return false;
//}


//function AddMachineTypeValidations() {
//    //<summary>function To Validate PopUp Controls </summary>
//    /// <param name="mode"  type="object">  
//    /// </param>
//    $("input[id$=MachineTypeName]").rules("add", {
//        required: true,
//        maxlength: 100,
//        messages: { required: "Translate(EnterMachineType)" }
//    });

//}
///#endregion





///#region  Conversion Section =====================================

function AddConversion() {
    ///<summary>Add conversion details</summary>
    RemoveValidations();
    ClearConversionDtls();
    if (DispersJson.ConversionList.length > 0)
        $("#ConversionDiv").show();
    else
        $("#ConversionDiv").hide();
    $("input[id$=btnAddConv]").val("Save");
    $("#divAddConversion").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    return false;
}
function ShowConversionDtls() {
    ///<summary>Shows conversion popup</summary>
    if ($("select[id$=DSP_QTY_UOM]").val() != "0" && $("select[id$=DSP_QTY_UOM]").val() != null) {
        $("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
        $(document.forms[0]).validate().resetForm();
        ResetUOMConversion();
        ResetConversionDetails();
        FillUOMDetails();
        $("[id$=UOMTypeFm]").text(" : " + $("select[id$=DSP_QTY_UOM] option:selected").text());
        //#####$("[id$=UOMTypeTo]").text(" : " + $("select[id$=UOM_TYPE] option:selected").text());
        //#####$("[id$=UMC_FROM]").text(" : " + $("[id$=UOM_CODE]").val());
    }
    else

        $("[id$=imbAddConversion]").css({ "display": "none", "visibility": "hidden" });

}
function ResetConversionDetails() {
    //<summary>Function to Reset Conversion Grid Controls Details </summary>
    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    if (DispersJson.ConversionList == null) {
        DispersJson.ConversionList = new Array();
    }
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
}

function ShowConversion() {


}
function FillUOMDetails() {
    ///<summary> Fill UOM to ddl</summary>
    var drpIDTo = $("select[id$=UMC_TO]").attr("id");
    $.get(DispersionMaster.GetUOMsURL + $("select[id$=DSP_QTY_UOM]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpIDTo, data, true, true);
    });
}
function ResetUOMConversion() {
    ///<summary>Reset UOM Converion</summary>
    $("select[id$=UMC_TO]").val("0");
    $("input[id$=UMC_CONV_FACT]").val("");
}
function ClearConversionDtls() {
    ///<summary>clear conversions</summary>
    $("select[id$=UMC_TO]").val("0");
    $("input[id$=UMC_CONV_FACT]").val("");
    $("input[id$=EditConversion]").val("0");
    frmUnitID = 0;
    toUnitId = 0;
}

function SaveConversionDtls() {
    ///<summary> Add conversion details to the dispersion object</summary>
    //$(document.forms[0]).validate().resetForm();
    RemoveValidations();
    RemoveConversionValidation();
    AddConversionValidations();
    if ($(document.forms[0]).valid()) {
        DispersJson = $("#divData").data("DispersionData");
        var editConversion = $("input[id$=EditConversion]").val();
        var obj = new Object();
        var flag = true;
        if (parseInt(toUnitId) == 0) {
            for (var i in DispersJson.ConversionList) {

                if (DispersJson.ConversionList[i].UPC_TO_UOM == $("select[id$=UMC_TO]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            // Check Conversion Already Added - For Updation
            for (var i in DispersJson.ConversionList) {
                if (DispersJson.ConversionList[i].UMC_PK != editConversion || DispersJson.ConversionList[i].UPC_TO_UOM != toUnitId) {
                    if (DispersJson.ConversionList[i].UPC_TO_UOM == $("select[id$=UMC_TO]").val()) {
                        flag = false;
                        break;
                    }
                }
            }
            // Check Details Is new Entry Or to update . If Update Get Details From DispersJson and Assign to Obj, and Details Will Updated To Object
            for (var i in DispersJson.ConversionList) {

                if (parseInt(toUnitId) == DispersJson.ConversionList[i].UPC_TO_UOM)

                    obj = DispersJson.ConversionList[i];
            }

        }

        // Check Already Added Or Not
        if (flag) {
            // Add One By one Details To Object

            obj.UMC_UOM_TYPE = 1; // Weight
            obj.UPC_FROM_UOM = $("[id$=DSP_QTY_UOM]").val();
            obj.FROM_UOM_NAME = $("select[id$=DSP_QTY_UOM] :selected").text();
            obj.UPC_TO_UOM = parseInt($("select[id$=UMC_TO]").val());
            obj.TO_UOM_NAME = $("select[id$=UMC_TO] :selected").text();
            obj.UPC_CONV_FACT = $("input[id$=UMC_CONV_FACT]").val();
            // Check Add Details - For New Entry
            if (parseInt(toUnitId) == 0) {
                // If Yes - get Length of the List and Assign Length+1 as the PK of New Entry
                obj.UMC_PK = 0; //  DispersJson.ConversionList.length + 1;
                //Push Object to List
                DispersJson.ConversionList.push(obj);
            }
            ClearConversionDtls();
            // Add Details To DivDatas
            $("#divData").data("DispersionData", DispersJson);
            // Bind Conversion Details Grid
            GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
            // GrandScriptUtils.ShowModal(UOMMaster.SAVECONVERSION, UOMMaster.INFORMATIONTITLE);
            if (DispersJson.ConversionList.length > 0)
                $("#ConversionDiv").show();

            else
                $("#ConversionDiv").hide();

        }
        else {

            GrandScriptUtils.ShowModal(DispersionMaster.ConversionAlreadyAdded, DispersionMaster.InformationTitle);
        }
        return false;
    }


}

function ConversionGridHandler(tr, command) {

    ///<summary>Grid Handler Catch all the grid events in this function  - For Conversion Details</summary>
    /// <param name="tr"  type="Object">
    ///     Specific Container and its controls
    /// </param>
    /// <param name="command"  type="Object">
    ///     Specific Edit/Delete
    /// </param>
    // Remove Validations

    switch (command.toString().toLowerCase()) {
        case "delete":
            //conversionID = GrandGrid.Utilities.GetColumnValue(tr, "UMC_PK", "grdConversionDtls");

            toUnitId = GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", "grdConversionDtls");
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, DispersionMaster.ConfirmationMsg, "DeleteConversion");
            //DeleteConversionDetails();
            return false;
            break;
        case "edit":
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
    ClearConversionDtls();
    $("select[id$=UMC_TO]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=UMC_CONV_FACT]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_CONV_FACT", $(tr).parents("table:eq(0)").attr("id")));
    $("input[id$=EditConversion]").val(GrandGrid.Utilities.GetColumnValue(tr, "UPC_PK", $(tr).parent().parent().attr("id")));
    toUnitId = GrandGrid.Utilities.GetColumnValue(tr, "UPC_TO_UOM", $(tr).parents("table:eq(0)").attr("id"));
}


function DeleteConversionDetails() {
    ///<summary>For delete the item in the grid - Conversion Details List</summary>
    var DispersJson = $("#divData").data("DispersionData");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in DispersJson.ConversionList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (DispersJson.ConversionList[i].UPC_TO_UOM == toUnitId) {
            // Splice Details From List, Corresponding ToUnit
            DispersJson.ConversionList.splice(i, 1);
            break;
        }
    }

    $("#divData").data("DispersionData", DispersJson);
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
    if (DispersJson.ConversionList.length > 0)
        $("#ConversionDiv").show();

    else
        $("#ConversionDiv").hide();
    ClearConversionDtls();

    return false;

}


function AddConversionValidations() {
    //<summary>function To Validate PopUp Controls </summary>
    $("input[id$=UMC_CONV_FACT]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: DispersionMaster.EnterConversionFactor }
    });
    $('[id$=UMC_TO]').rules("add", {
        selectNone: true,
        messages: { selectNone: DispersionMaster.SelectUOM }
    });
}

function RemoveConversionValidation() {
    $("input[id$=UMC_CONV_FACT]").rules("remove");
    $('[id$=UMC_TO]').rules("remove");
}

function MakeNumeric(event) {
    ///<summary> Restrict only numeric</summary>
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }

}

/*function FillMachine(machID) {
    ///<summary>function To Fill UOM Details </summary> 
    var drpID = $("select[id$=DSP_MACHINE]").attr("id");
    $.get(DispersionMaster.GetMachineURL + "0&ProcessID=7", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, machID);
    });
}
*/
///#endregion

function FillStoreTree(itemPk) {
    //<summary>Function Used to Fill Menu Details to Tree View </summary>
    var sbuPK = $("[id$=BizUnitPk]").val();
    SetTreeHeaderStructure("trvStores", DispersionMaster.GetStores + sbuPK + "&ItemPK=" + itemPk, "Stores", true, false, "", true);
    MakeMultiTree();
    // call the function to bind tree view
}

function GetSelectedStores() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ObjMaterialStores = new Array();
    var obj = new Object();

    $("#trvStores").find("input[type=checkbox]:checked").each(function () {

        materialPK = $(this).attr("id");
        obj = new Object();
        materialPK = materialPK.substr(materialPK.lastIndexOf("_") + 1, materialPK.length);
        obj.DPT_PK = materialPK;
        ObjMaterialStores.push(obj);
    });
    return ObjMaterialStores;
}

function ShowTabs(flag) {
    $("[id$=hdfIsFinalTab]").val(flag);
}

function SetCategoryType() {
    var categoryID = 0;
    categoryID = $("[id$=DSP_ITM_CATEGORY]").val() == null ? 0 : $("[id$=DSP_ITM_CATEGORY]").val();
    $.get(DispersionMaster.GetMaterialCategoryDetailsURL + categoryID + "&Active=1", function (data) {
        if (data != null && data.length > 0) {
            $("[id$=ITC_VALUE]").text(data[0].ITC_VALUE_TEXT);
        }
    });
}

function ShowCategory() {
    ///<summary>Function used call the tree Data </summary>
    //    
    //  var encStr=encode_utf8($("[id$=ITM_DESC]").val().replace(/[!'()]/g, escape));
    // var decStr=decode_utf8(encStr);
    //  alert($("[id$=ITM_DESC]").val());
    //   alert(encStr);
    // alert(decStr);

    FillCategoryTree(); //call tree view function.
    GrandScriptUtils.ShowModalID("divCategory", "Choose Category", false, "700", false, false);
    return false;
}

///#region---- Fetch Data To Populate In Controls
function FillCategoryTree() {
    //<summary>function To Fill Category in tree view  </summary>
    //    SetTreeHeaderStructure("trvCategory", MaterialMaster.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + MaterialMaster.Param, "Root", false, false, "0", false); // set the tree view parameters
    SetTreeHeaderStructure("trvCategory", DispersionMaster.GetMaterialCategoryTreeURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val() +"&MatCagID=","Root", false, false, "0", false); // set the tree view parameters
    MakeMultiTree(); // call the function to bind tree view
}

//function FillCategory(catPK, uomPK) {
//    //<summary>function To Fill Category Details </summary>
//    // Get id of the Category DropDown
//    var drpID = $("select[id$=DSP_ITM_CATEGORY]").attr("id");
//    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
//    $.get(DispersionMaster.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ITM_SET]").val(), function (data) {
//        if ($("[id$=ITM_SET]").val() == 3 || $("[id$=ITM_SET]").val() == 4)
//            GrandScriptUtils.FillDropDown(drpID, data, true, false, catPK);
//        else
//            GrandScriptUtils.FillDropDown(drpID, data, true, true, catPK);
//        //FillUOM(catPK, uomPK);
//        SetCategoryType();
//    });
//}

function AddSelectedTree(liAdd) {
    ///<summary>Function used Add the tree Data </summary>
    /// <param name="liAdd"  type="object">
    ///     Specific categoryid to fill corrusponding uom
    /// </param>
    var cagID = $(liAdd).attr("id"); // get the selected tree id
    var CagName = cagID.substr(0, cagID.indexOf("_")); //For avoiding to execute below codes after click on Stores in StoreMapping Tab
    if (CagName != "trvStores") {
        cagID = cagID.substr(cagID.lastIndexOf("_") + 1, cagID.length); // fetch the exact id of category

        $("[id$=DSP_ITM_CATEGORY]").val(cagID)//$("select[id$=DSP_ITM_CATEGORY]").val(cagID);
        $("[id$=txtDspItemCategory]").val($(liAdd)[0].innerText);

        SetCategoryType();
        //setting selected value for uom after selecting category from treeview.
        //        FillUOM(cagID, false)
        //closing modalbox after selected from treeview.
        $("#divCategory").dialog("destroy");
        $("#divCategory").dialog({ autoOpen: false });
    }
}