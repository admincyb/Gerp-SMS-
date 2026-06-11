/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../JSLINQ/JSLINQ.js" />
/// <reference path="../../JSLINQ/JSLINQ-vsdoc.js" />

///#region----Global Variables
var CompoundArray =
{
    CompoundrList: new Array()
}

var CompoundJson = new Object();
var ConversionFactor = 1;
var CompoundDetailJson = new Object();
var tdset = "";
var materialID = 0;
var materialTypeID = 0;
var batchID = 0;

var BatchList = new Object();
var compoundId = 0;
var itemStock = 0;
var deptId;
var TankPk = 0;
var ConversionFactorQty = 1;
var IsValidEndTime = true;
var decmlPlace = 5;
///#endregion

///#region----Configuration section

var CompoundPrep = {

    //URLS

    FillBatchNoDropDownURL: "MaterialManagement.do?Action=GetBatchNo&SBUPk=",
    FillBatchDetailGetURL: "MaterialManagement.do?Action=GetBatchDetails&SBUPk=",

    GetCompoundForDropdown: "CompoundPreparation.do?Action=GetCompoundBatchNo",
    GetPlanForDropdown: "CompoundPreparation.do?Action=GetPlanListCombo",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList",
    GetInspectionDetailsListURL: "DispersionPreparation.do?Action=GetInspectionDetails&BatchPK=",
    GetRawMaterialInspectionDetails: "DispersionPreparation.do?Action=GetRawMaterialInspectionDetails&TrxPK=",
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetCompoundDetail: "CompoundPreparation.do?Action=GetCompoundDetail&compoundID=",
    GetBatches: "CompoundPreparation.do?Action=GetBatchesForItem&ItemType=",
    GetTankDropdown: "CompoundPreparation.do?Action=GetTankListComboOnly",
    BindGrid: "DispersionManagement.do?Action=GetDispersionList&Status=",
    GetUOMURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=1",
    GetMaterialName: "CompoundMaster.do?Action=GetMaterialName&SBU=",
    GetCompoundTypeName: "CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
    GetConversionFactor: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    Savepage: "CompoundPreparation.do?Action=SaveCompoundTrxDetails",
    GetCheckListDetailsURL: "CommonManagement.do?Action=GetCheckList&checkListID=",
    GetSelectedTankDetails: "CompoundPreparation.do?Action=GetSelectedTankDetails&TnkPK=",
    FillUOMDropdownURL: "UOMManagement.do?Action=GetUnit",
    //messages
    SaveMessage1: "Translate(CompoundTransactionSaved1)",
    SaveMessage2: "Translate(CompoundTransactionSaved2)",
    CodeAlreadyExist: 'Translate(CompoundTransactionCodealreadyexists)',
    ActionFailed: 'Translate(ActionFailed)',
    Status: 'Translate(Status)',
    PleaseSelectMaterials: 'Translate(PlsaddrequiredMaterialDistribution)',
    DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",
    ConfirmationMessage: "Translate(Conformation)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    Information: "Translate(Information)",
    DefaultActionNeeded: 'Translate(DefaultActionneedstobeperformed)',
    DoyoWantDelete: 'Translate(Doyouwanttodeletethisdetails)',
    Confirmation: 'Translate(Conformation)',
    SelectCompound: 'Translate(SelectCompound)',
    SelectUOM: 'Translate(SelectUOM)',
    SelectTank: 'Translate(SelectTank)',
    SelectPlan: 'Translate(SelectPlan)',
    EnterEndDate: 'Translate(EnterEndDate)',
    EnterStartDate: 'Translate(EnterStartDate)',
    EnterEndTime: 'Translate(EnterEndTime)',
    EnterStartTime: 'Translate(EnterStartTime)',
    EnterQuantity: 'Translate(EnterQuantity)',
    EnterTotalTimeInHr: 'Translate(EnterTotalTimeInHr)',
    SelectCatagory: 'Translate(SelectCatagory)',
    SelectMaterial: 'Translate(SelectMaterial)',
    EndTimeValid: 'Translate(EndTimeValid)',
    SelectBatchNo: 'Translate(SelectBatchNo)',
    MaterialCannotAdd: "Translate(MaterialCannotAdd)",
    NotEnoughStockFor: "Translate(NotEnoughStockFor)",
    NotEnoughStock: "Translate(NotEnoughStock)",
    MaterialAlreadyAdded: 'Translate(MaterialAlreadyAdded)',
    BatchAlreadyAdded: 'Translate(BatchAlreadyAdded)',
    LockedForEdit: 'Translate(EditUsedByAnotheruser)',
    SameCompundasMeterial: 'Translate(SameCompundasMeterial)',
    DeletedRecord: "Translate(DeletedRecord)",
    InvalidCompoundQty: "Translate(InvalidCompoundQty)",

    //constants
    TEXTEMPTY: "",
    TEXTZERO: "0",
    //Commands
    Failed: "failed",
    TIH_NO: "TIH_NO",
    TIH_PK: "TIH_PK",
    TID_MIN_VALUE: "TID_MIN_VALUE",
    TID_MAX_VALUE: "TID_MAX_VALUE",
    TID_VALUE: "TID_VALUE",
    TID_VARIANCE: "TID_VARIANCE",
    TID_STD_VALUE: "TID_STD_VALUE"
}

///#endregion

///#region----Initialization section



$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
    }, 'Translate(Pleaseselectanoption)');
    $.validator.addMethod("dateTime", function (value) {
        return /^([01]?[0-9]|2[0-3]):[0-5][0-9]?$/.test(value);
    }, "Translate(EnterValidTime)");

    $.validator.addMethod("ValidateEnddateTime", function (value) {
        return IsValidEndTime;
    }, "Translate(EndTimeValid)");

    $("#divArray").data("CompoundArray", CompoundArray.CompoundList);
    CompoundJson = $.parseJSON($("[id$=CompoundDetailsList]").val());
    $("#divData").data("CompoundData", CompoundJson);
    //    $("#divData").hide();
    //    $("[id$=imbSave]").hide();
    $("#grdCompounding").css({ "display": "none", "visibility": "hidden" });
    PageInit();
    $("[id$=CTH_TOTAL_TM]").attr("disabled", true);
    $("form").find('input[type=text],textarea,select').filter(':visible:enabled:first').focus();
});

function PageInit() {
    //seach functionality
    $("#divgrdChecklist").hide();
    $("[id$=CTH_DEPT]").val($("[id$=hdfDeptID]").val());
    deptId = $("[id$=hdfDeptID]").val();
    FillInitialSettings();
    var compoundObj = $.parseJSON($("[id$=CompoundDetail]").val());
    var isEdit = false;
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                isEdit = true;
                if (compoundObj != null) {
                    FillCompoundTrxDetails(compoundObj);
                    CompoundJson = compoundObj;
                    CompoundDetailJson = CompoundJson;
                    //CheckList
                    if (compoundObj.CheckListDtl != undefined) {
                        if (!$.isArray(compoundObj.CheckListDtl)) {
                            var materialObj = compoundObj.CheckListDtl;
                            compoundObj.CheckListDtl = new Array();
                            compoundObj.CheckListDtl.push(materialObj);
                        }
                        $("#divData").data("CheckListDtl", compoundObj.CheckListDtl);
                        GrandGrid.Utilities.ResetGrid(true, "grdChecklistDetails");
                        GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, compoundObj.CheckListDtl);
                        $("#divgrdChecklist").show();
                    }
                } //endchecklist               
            }
        }
    }
    if (compoundObj != null) {
        FillCompounds(compoundObj.CTH_COMPOUND);
        FillPlans(compoundObj.CTH_PLAN);
        FillQuantityUOM(compoundObj.CTH_QUANTITY_UOM);
        FillTanks(compoundObj.CTH_TANK_NO);
        FillTanKcapacity(compoundObj.CTH_TANK_NO);
    }
    else {
        FillCompounds();
        FillQuantityUOM();
        FillPlans();
        FillTanks();
        FillTimeUOM();
    }
    Popup();
    GrandScriptUtils.FillDropDown($("[id$=Material]").attr("id"), new Object(), true, true);
    GrandScriptUtils.FillDropDown($("[id$=BatchNo]").attr("id"), new Object(), true, true);
    $("[id$=BatchNo] :first").hide();
}

function AddNew() {
    ///<summary>event triggered when add new button click</summary>

    $("#divData").show();
    $("#divListing").hide();
    $("[id$=imbAdd]").hide();
    $("[id$=imbSave]").show();
    $("id$=DispersionID").val("0");
    var dummyObj = new Object();
    //initialize grid
    GrandGrid.MakeGrid($("#grdCompounding"), 0, dummyObj);
    $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
    $("#MaterialInsert").show();
    $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });

    return false;
}

///#endregion

///#region---------------Core section

///#region ---------Fill Dropdowns

function FillInitialSettings() {
    ///<summary>Method to fill initial data

    $("[id$=BtnFillCompound]").attr("disabled", "disabled");
    $("[id$=CTH_QUANTITY_UOM]").attr("disabled", "disabled");
    $("[id$=UOMPk]").attr("disabled", "disabled");
    $("[id$=CTH_ST_TM]").timepicker({
        onSelect: function (dateText, inst) {
            AfterDateSelect();
        }
    });
    $("[id$=CTH_ED_TM]").timepicker({
        onSelect: function (dateText, inst) {
            AfterDateSelect();
        }
    });
    GrandScriptUtils.AddDateRange("CTH_START_TM", "hdnFromDate", "CTH_END_TM", "hdnToDate", false, false, false);
    //    GrandScriptUtils.DatePicker("CTH_COMP_DT", false, false, true, "CTH_START_TM", "CTH_END_TM", "hdnDate"); //change to as per beta req
    GrandScriptUtils.DatePicker("CTH_COMP_DT", false, false, true, false, false, false);
    $("#divgrdCompoundingGrid").hide();
    $("#divMaterialInsert").hide();
    $("[id$=imbAdd]").hide();

}

function myTrim(x) {
    return x.replace(/^\s+|\s+$/gm, '');
}

function AfterDateSelect() {
    ///<summary>To handle bind grid </summary> 

    if ($("[id$=CTH_END_TM]").val() != "" && $("[id$=CTH_ED_TM]").val() != "" && $("[id$=CTH_START_TM]").val() != "" && $("[id$=CTH_ST_TM]").val() != "") {
        var unloadTm = $("[id$=CTH_END_TM]").val() + " " + $("[id$=CTH_ED_TM]").val();
        var loadTm = $("[id$=CTH_START_TM]").val() + " " + $("[id$=CTH_ST_TM]").val();
        var loadTime = new Date();
        var unloadTime = new Date();
        var diff = 0;
        if (myTrim(unloadTm) != "" && myTrim(loadTm) != "") {
            loadTm = loadTm.replace("-", "/");
            loadTm = loadTm.replace("-", "/");
            unloadTm = unloadTm.replace("-", "/");
            unloadTm = unloadTm.replace("-", "/");
            loadTime = new Date(CalculateDate(loadTm));
            unloadTime = new Date(CalculateDate(unloadTm));
            var sec = unloadTime.getTime() - loadTime.getTime();
            var second = 1000, minute = 60 * second, hour = 60 * minute, day = 24 * hour;
            var days = Math.floor(sec / day);
            sec -= days * day;
            var hours = Math.floor(sec / hour);
            sec -= hours * hour;
            var minutes = Math.floor(sec / minute);
            sec -= minutes * minute;
            var seconds = Math.floor(sec / second);
            var totalHrs = ((24 * days) + hours) + "." + minutes;
            if (parseFloat(totalHrs) > 0) {
                $("[id$=CTH_TOTAL_TM]").val(totalHrs);
                IsValidEndTime =true;
            }
            else {
                $("[id$=CTH_TOTAL_TM]").val("");
                IsValidEndTime = false;
            }
        }
        else {
            $("[id$=CTH_TOTAL_TM]").val("");
        }
    }
    else {
        $("[id$=CTH_TOTAL_TM]").val("");
    }
}

function CalculateDate(convertDate) {
    ///<summary>To handle bind grid </summary>

    convertDate = convertDate.replace("Jan", "01");
    convertDate = convertDate.replace("Feb", "02");
    convertDate = convertDate.replace("Mar", "03");
    convertDate = convertDate.replace("Apr", "04");
    convertDate = convertDate.replace("May", "05");
    convertDate = convertDate.replace("Jun", "06");
    convertDate = convertDate.replace("Jul", "07");
    convertDate = convertDate.replace("Aug", "08");
    convertDate = convertDate.replace("Sep", "09");
    convertDate = convertDate.replace("Oct", "10");
    convertDate = convertDate.replace("Nov", "11");
    convertDate = convertDate.replace("Dec", "12");
    convertDate = convertDate.split("/");
    return convertDate[1] + "/" + convertDate[0] + "/" + convertDate[2];
}

function FillCompounds(CompoundID) {
    //<summary>function To Fill Compound material </summary>

    var drpID = $("select[id$=CTH_COMPOUND]").attr("id");
    $.get(CompoundPrep.GetCompoundForDropdown, function (data) {
        if (CompoundID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, CompoundID);
        }
    });
}

function FillQuantityUOM(quantityUOM) {
    ///<summary>Fill quantity UOMs for header</summary>

    var drpID = $("[id$=CTH_QUANTITY_UOM]").attr("id");
    $.get(CompoundPrep.GetUOMURL, function (data) {
        if (quantityUOM == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, quantityUOM);
        }
    });
}

function FillPlans(planID) {
    //<summary>function To Fill Compound material </summary>

    var drpID = $("[id$=CTH_PLAN]").attr("id");
    $.get(CompoundPrep.GetPlanForDropdown, function (data) {
        var IsPlan = $("[id$=isPlanRequired]").val();
        if (IsPlan == "0") {
            if (planID == null) {
                GrandScriptUtils.FillDropDown(drpID, data, true, false, "0", true);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, false, planID, true);
            }
        }
        else {
            if (planID == null) {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, "0");
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, planID);
            }


        }


    });
}

function FillTanks(tankID) {
    //<summary>function To Fill Compound material </summary>

    var drpID = $("[id$=CTH_TANK_NO]").attr("id");
    $.get(CompoundPrep.GetTankDropdown, function (data) {
        if (tankID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, tankID);
        }
    });
}
//For Displaying Tank Capacity of the selected Tank
function FillTanKcapacity(tankID) {
    if (tankID == null || tankID == 0) {
        TankPk = $("select[id$=CTH_TANK_NO]").val();
    }
    else {
        TankPk = tankID;
    }
    $.get(CompoundPrep.GetSelectedTankDetails + TankPk + "&SBU=" + $("[id$=BizUnitPk]").val() + "&Active=2", function (data) {
        if (data != null && data.length > 0) {
            $("[id$=TankCapacity]").val(data[0]["TNK_CAPACITY"]);
            FillUOM(data[0]["TNK_CAPACITY_UOM"]);          
            $("[id$=UOMPk]").attr("disabled", "disabled");
        }
        else {

        }
    });
}
function FillUOM(TnkCapacityUOM) {
    //<summary>function To Fill UOM Type Details </summary>
    // Get id of the UOM DropDown
    var drpID = $("select[id$=UOMPk]").attr("id");
    //Fill UOM Details to the Category DropDown, Name as Text, PK as Value
    var ajaxxurl = CompoundPrep.FillUOMDropdownURL + "&SBU=" + $("[id$=BizUnitPk]").val() + "&UOMTypeName=Weight";
    $.get(ajaxxurl, function (data) {
        if (TnkCapacityUOM == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, TnkCapacityUOM);
        }
    });

}

function FillTimeUOM(uomID) {
}
function FillCategoryDetails(materialType) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>

    FillMaterials(materialType);

}
function FillMaterialTypes(materialTypeID) {
    //<summary>function To Fill Category Details </summary>

    var drpID = $("select[id$=MaterialType]").attr("id");
    $.get(CompoundPrep.FillMaterialCategoryDropdownURL, function (data) {
        if (materialTypeID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialTypeID);
        }
    });
}

function FillMaterials(materialType, materialID) {

    var drpID = $("select[id$=Material]").attr("id");
    $.getJSON(CompoundPrep.GetMaterialByCategory + "&CategoryID=" + materialType, function (data) {
        if (materialID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });
}

function FillMaterialNames(catgID, selValue) {
    ///<summary>Method to fill material names to material dropdown</summary>

    var drpID = $("select[id$=Material]").attr("id");
    var drpUOMID = $("[id$=MaterialQuantityUOM]").attr("id");
    GrandScriptUtils.FillDropDown(drpID, new Object(), true, true);
    GrandScriptUtils.FillDropDown($("[id$=BatchNo]").attr("id"), new Object(), true, true);
    $("[id$=BatchNo]:first").hide();
    $("[id$=MaterialQuantity]:first").val("");
    $("#tdCurrentStock").html("");
    $("#spnUOM").html("");
    if (catgID == 0) {
        return false;
    }
    $.get(CompoundPrep.GetMaterialName + $("[id$=BizUnitPk]").val() + "&CATG=" + catgID, function (data) {
        if (selValue == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        }
    });
    if ($("select[id$=MaterialType]").val() >= 1) {
        $("[id$=BatchNo]").show();
    }
    else {
        $("[id$=BatchNo]:first").hide();
    }
}

function FillMaterialDetails() {
    ///<summary>Method to fill material details</summary>

    var categoryID = $("[id$=MaterialType]").val();
    var materialID = $("[id$=Material]").val();
    GrandScriptUtils.FillDropDown($("[id$=BatchNo]").attr("id"), new Object(), true, true);
    $("#spnUOM").html("");
    $("[id$=MaterialQuantity]:first").val("");
    if (materialID == 0) {
        return false;
    }

    if (categoryID == 1)
        FillMeterialBatchNo();
    GetMaterialDetails(categoryID, materialID);
    //setBalanceReq();
    $.get(CompoundPrep.GetCompoundTypeName + materialID, function (data) {
        if (data != " " && data != "-1") {
            //alert("Converted Factor  Value:" + data);
            if (parseInt(data) != 1) {
                GrandScriptUtils.ShowModal(CompoundPrep.MaterialCannotAdd, CompoundPrep.Information);
                return false;
            }
        }
    });
    if (categoryID > 1 && materialID != 0) {
        FillBatchNo($("[id$=BatchNo]").attr("id"), categoryID, materialID);
    }
    else if (categoryID == 1 && materialID != 0) {
        //GetMaterialDetails(categoryID, materialID);

    }
}


function FillMeterialBatchNo() {
    ///<summary>Function used Fill Batch No</summary>
    /// <param name="categoryID"  type="object">
    ///     Specific categoryid to fill corresponding Material
    /// </param>
    /// <param name="materialID"  type="object">
    ///     Specific materialID to select the dropdown item after filling drop down
    /// </param> 
    var drpID = $("select[id$=BatchNo]").attr("id");
    var itemID = $("[id$=Material]").val();
    $.getJSON(CompoundPrep.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + itemID + "&DepartmentID=" + deptId, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

function FillBatchNo(drId, matCatId, matId, batchId, index, notIncludeSelect, currRow, materialsObj, currentBalance) {
    ///<summary>Method to fill batch no. when material index changes</summary>
    /// <param name="drId" >
    ///     Dropdown Id to be filled
    /// </param>
    ///<param name="matCatId" >
    ///     Material Catagory ID 1:Raw material 2:Dispersion 3:Compound
    /// </param>
    ///<param name="matId" >
    ///     Material Id of the selected material.
    /// </param>
    ///<param name="batchId" >
    ///     Batch Id to be  selected after filling data
    /// </param>
    ///<param name="index" >
    ///     if Index is specified save the batch list into the corresponding material object
    /// </param>
    ///<param name="notIncludeSelect" type="bool" >
    ///     whether to include --select-- 
    /// Used: if the function is used for dynamically adding batch dropdowns to the grid,do not include 
    /// select
    /// </param>

    var batchID = batchId == undefined ? 0 : batchId;
    var stockIndex = 0;
    var ajaxUrl = '';
    var dtlPK = 0;
    var cmpPK = 0;
    var currentQty = 0;
    var reqQuantity = 0;
    var balanceReq = 0;
    if (drId == null) {
        var drpUOMID = $("select[id$=BatchNo]").attr("id");
        ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId + "&BatchPK=" + batchID;
    }
    else {
        var drpUOMID = drId;
        if (currRow == undefined) {
            dtlPK = 0;
        }
        else {
            dtlPK = GrandGrid.Utilities.GetColumnValue(currRow, "CTD_PK", "grdCompounding");
            if (dtlPK == "undefined") {
                dtlPK = 0;
            }
        }
        if ($("[id$=CTH_COMPOUND]").val() == null) {
            cmpPK = CompoundJson.CTH_COMPOUND;
        }
        else {
            cmpPK = $("[id$=CTH_COMPOUND]").val();
        }
        if (matCatId > 1) {
            ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId + "&BatchPK=" + batchID + "&CompPK=" + cmpPK + "&CompDtlPK=" + dtlPK;
        }

        if (matCatId == 1) {
            $.getJSON(CompoundPrep.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + matId + "&DepartmentID=" + $("[id$=hdfDeptID]").val() + "&BatchPK=" + batchID, function (data) {
                GrandScriptUtils.FillDropDown(drpUOMID, data, true, false, batchId);
            });
        }
    }


    //    var drpID = $("select[id$=BatchNo]").attr("id");
    //    var itemID = $("[id$=Material]").val();


    $.get(ajaxUrl, function (data) {
        if (data) {
            if (matCatId > 1) {
                BatchList = data;
                if (index != null) { //for saving the batch details into the Compound data;
                    var ObjDisp = $("#divData").data("CompoundData");
                    ObjDisp.Materials[index].BatchDetails = data;
                    $("#divData").data("CompoundData", ObjDisp);

                }
                if (batchId == null) {
                    if (notIncludeSelect && data.length > 0) {
                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, false);
                        stockIndex = GrandGrid.Utilities.GetColumnIndex(currRow, "STOCK", "grdCompounding");

                        currentQty = data[0].STOCK;
                        reqQuantity = GrandGrid.Utilities.GetColumnValue(currRow, "CTD_REQ_QTY", "grdCompounding");

                        if (parseFloat(currentQty) < parseFloat(reqQuantity))
                            currRow.find("[id$=MaterialQuantity]").val(data[0].STOCK);

                        currentQty = currRow.find("[id$=MaterialQuantity]").val();
                        //currentQty = GetCurrentQuantity(matCatId, matId);

                        var ObjDisp = $("#divData").data("CompoundData");
                        balanceReq = parseFloat(reqQuantity) - parseFloat(currentQty)
                        ObjDisp.Materials[index].BALANCE_REQ = balanceReq;
                        ObjDisp.Materials[index].CTD_QUANTITY = currentQty;

                        balanceReqObj = GrandGrid.Utilities.GetColumnIndex(currRow, "BALANCE_REQ", "grdCompounding");
                        currRow.find("td:eq(" + balanceReqObj + ")").html(ObjDisp.Materials[index].BALANCE_REQ);
                        $("#divData").data("CompoundData", ObjDisp);

                        currRow.find("td:eq(" + stockIndex + ")").html(data[0].STOCK);
                        materialsObj.STOCK = data[0].STOCK;
                    }
                    else {
                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);
                    }
                }
                else {
                    if (notIncludeSelect && data.length > 0) {
                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, false, batchId);
                    }
                    else {
                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, batchId);
                    }
                }
            }
        }
        else {
            //if no data must include --select---
            GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);
        }
    });


}

function GetCurrentQuantity(matCatId, matId) {

    var ObjDisp = $("#divData").data("CompoundData");
    var currentQty = 0;
    for (var i in ObjDisp.Materials) {
        if (ObjDisp.Materials[i].CTD_ITEM == matId && ObjDisp.Materials[i].CPD_ITEM_CATEGORY == matCatId) {
            currentQty = currentQty + parseFloat(ObjDisp.Materials[i].CTD_QUANTITY);

        }
    }
    return currentQty;
}
function GetMaterialDetails(matCatId, matId) {
    ///<summary>Method to get the details of a raw material selected.Get stock,UOM,etc
    ///Used In:When material index changes in the grid
    ///<param name="matCatId">Material Catagory Id</param>
    ///<param name="matId">Material  Id</param>

    ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId;
    $.get(ajaxUrl, function (data) {
        if (data.length>0) {
            itemStock = data[0].STOCK; //set to current item stock 
            $("[id$=MaterialQuantityUOM]").val(data[0].ITM_UOM);
            $("#spnUOM").html(data[0].UOM_CODE);
            //$("#tdCurrentStock").html(itemStock);
        }
    });
}

var OldValue = 0;

function GetOldValue(slNO, oldVal) {

    ObjDisp = $("#divData").data("CompoundData");
    for (var i in ObjDisp.Materials) {
        if (ObjDisp.Materials[i].SLNO == slNO) {
            if (ObjDisp.Materials[i].CTD_BATCH == undefined && ObjDisp.Materials[i].CTD_STK_BATCH == undefined) {
                return oldVal;
            }
            else {
                if (ObjDisp.Materials[i].CTD_BATCH == undefined) {
                    return ObjDisp.Materials[i].CTD_STK_BATCH;
                }
                else {
                    return ObjDisp.Materials[i].CTD_BATCH;
                }
            }
        }
    }
}

function setBalanceReq() {
    var balanceReq = 0;

    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {

            if (GrandGrid.Utilities.GetColumnValue($(this), "CTD_ITEM", "grdCompounding") == $("select[id$=Material]").val() && GrandGrid.Utilities.GetColumnValue($(this), "CPD_ITEM_CATEGORY", "grdCompounding") == $("select[id$=MaterialType]").val()) {
                balanceReq = balanceReq + parseFloat($(this).find("[id$=MaterialQuantity]").val());
            }
        }
    });


    //  get balance required in list
    var ReqQuantity = 0;
    var Stock = 0;
    var ObjDisp = $("#divData").data("CompoundData");
    for (var i in ObjDisp.Materials) {
        if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && ObjDisp.Materials[i].CPD_ITEM_CATEGORY == $("select[id$=MaterialType]").val()) {
            ReqQuantity = ObjDisp.Materials[i].CTD_REQ_QTY;
            if ($("select[id$=MaterialType]").val() == 1) {
                Stock = ObjDisp.Materials[i].STOCK;
                break;
            }
            else {
                var BatchId = $("select[id$=BatchNo]").val();
                for (var j in ObjDisp.Materials[i].BatchDetails)
                    if (ObjDisp.Materials[i].BatchDetails[j].Value == BatchId) {
                        Stock = ObjDisp.Materials[i].BatchDetails[j].STOCK;
                        break;
                    }
                break;
            }
        }
    }

    balanceReq = roundNumber(ReqQuantity - balanceReq, 3);
    if (balanceReq >= 0) {
        if (balanceReq < Stock)
            $("[id$=MaterialQuantity]:first").val(balanceReq);
        else
            $("[id$=MaterialQuantity]:first").val(Stock);
    }
    else
        $("[id$=MaterialQuantity]:first").val("0");

}

function FillBatchQuantity(containerRow) {
    //<summary> ///Method to fill batch quantity into the quantity text box when batchindex chnages</summary>

    var flag = true;
    var stock = 0;
    var quantity = 0;
    var itemPK = GrandGrid.Utilities.GetColumnValue(containerRow, "CTD_ITEM", "grdCompounding");  ///not working
    var itemType = GrandGrid.Utilities.GetColumnValue(containerRow, "CPD_ITEM_CATEGORY", "grdCompounding");
    var slNO = GrandGrid.Utilities.GetColumnValue(containerRow, "SLNO", "grdCompounding");
    var reqQuantity = GrandGrid.Utilities.GetColumnValue(containerRow, "CTD_REQ_QTY", "grdCompounding");
    var catId = GrandGrid.Utilities.GetColumnValue(containerRow, "CPD_ITEM_CATEGORY", "grdCompounding");

    //If the request is from header
    if (slNO == "") {
        setBalanceReq();
    }
    var fl = true;
    if (catId == 1) {
        var CompoundJson = $("#divData").data("CompoundData");
        if (containerRow) {
            if (containerRow[0].rowIndex > 1) {
                $("#grdCompounding tr:has(td)").each(function (index) {
                    if (flag) {
                        if (index > 0 && index != containerRow[0].rowIndex - 1) {
                            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdCompounding");
                            CatgID = GrandGrid.Utilities.GetColumnValue($(this), "CPD_ITEM_CATEGORY", "grdCompounding");
                            tempCatId = CatgID;
                            drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
                            if (drpUOMID != undefined) {
                                if (containerRow.find("[id$=BatchNo]").val() == $("#" + drpUOMID).val() && itemType == CatgID) {
                                    containerRow.find("[id$=BatchNo]").val(GetOldValue(slNO, containerRow.find("[id$=BatchNo] option:first").val()));
                                    fl = false;
                                    if ((containerRow[0].rowIndex > 1) && fl == false) {
                                        GrandScriptUtils.ShowModal(CompoundPrep.BatchAlreadyAdded, CompoundPrep.Information);
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
        $("#grdCompounding tr:has(td)").each(function (index) {
            if (fl) {
                if (slNO == index) {
                    var objDisp = $("#divData").data("CompoundData");
                    batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdCompounding");
                    BatchID = GrandGrid.Utilities.GetColumnValue($(this), "BatchNo", "grdCompounding");
                    drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
                    var stockIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "STOCK", "grdCompounding");
                    if (drpUOMID != undefined) {
                        $.getJSON(CompoundPrep.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + containerRow.find("[id$=BatchNo]").val(), function (data) {
                            if (data != null) {
                                var qtyStock = data[0].SBD_QTY_IN_STOCK;
                                itemStock = qtyStock;
                                containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock);
                                containerRow.find("[id$=MaterialQuantity]").val(reqQuantity);
                                if (itemType == 1) {
                                    objDisp.Materials[index - 1].CTD_STK_BATCH = containerRow.find("[id$=BatchNo]").val();
                                }
                                else {
                                    objDisp.Materials[index - 1].CTD_BATCH = containerRow.find("[id$=BatchNo]").val();
                                }
                                objDisp.Materials[index - 1].CTD_QUANTITY = containerRow.find("[id$=MaterialQuantity]").val();
                                objDisp.Materials[index - 1].STOCK = qtyStock;
                                fl = false;
                            }
                            else {
                                containerRow.find("td:eq(" + stockIndex + ")").html(0);
                                containerRow.find("[id$=MaterialQuantity]").val(0);
                                itemStock = 0;
                                fl = false;
                            }
                        });
                    }
                }
            }
        });
    }
    else {
        var tempCatId = 0;
        var CompoundJson = $("#divData").data("CompoundData");
        if (containerRow) {
            if (containerRow[0].rowIndex > 1) {
                $("#grdCompounding tr:has(td)").each(function (index) {
                    if (flag) {
                        if (index > 0 && index != containerRow[0].rowIndex - 1) {
                            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdCompounding");
                            CatgID = GrandGrid.Utilities.GetColumnValue($(this), "CPD_ITEM_CATEGORY", "grdCompounding");
                            tempCatId = CatgID;
                            drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
                            if (drpUOMID != undefined) {
                                if (containerRow.find("[id$=BatchNo]").val() == $("#" + drpUOMID).val() && itemType == CatgID) {
                                    containerRow.find("[id$=BatchNo]").val(GetOldValue(slNO, containerRow.find("[id$=BatchNo] option:first").val()));
                                    flag = false;
                                }
                            }
                        }
                    }
                });
            }
            if (flag == true) {

                if (!isNaN(itemPK)) {
                    var batchList = JSLINQ(CompoundJson.Materials).Where(function (item) { return item.CTD_ITEM == itemPK && item.CPD_ITEM_CATEGORY == itemType }).items[0].BatchDetails;
                }
                else {
                    batchList = BatchList;
                }
                var BatchId = containerRow.find("[id$=BatchNo]").val();
                var ObjDisp = $("#divData").data("CompoundData");
            }
            if ((containerRow[0].rowIndex > 1) && flag == false) {
                GrandScriptUtils.ShowModal(CompoundPrep.BatchAlreadyAdded, CompoundPrep.Information);
            }
        }
        else {
            var BatchId = $("[id$=BatchNo]").val();
        }
        if (flag == true) {
            var index = -1;
            for (var i in batchList) {
                if (batchList[i].Value == BatchId) {

                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                var quantity = containerRow.find("[id$=MaterialQuantity]").val();
                var stockIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "STOCK", "grdCompounding");
                if (containerRow[0].rowIndex > 1) {
                    containerRow.find("td:eq(" + stockIndex + ")").html(batchList[index].STOCK);
                }
                else {
                    $("#tdCurrentStock").html(batchList[index].STOCK);
                }

                containerRow.find("[id$=spnUOM]").html(batchList[index].UOM_CODE);
                containerRow.find("[id$=MaterialQuantityUOM]").val(batchList[index].ITM_UOM);
                if ((containerRow[0].rowIndex > 1)) {
                    for (var i in CompoundJson.Materials) {
                        if (CompoundJson.Materials[i].SLNO == slNO) {
                            CompoundJson.Materials[i].CTD_BATCH = containerRow.find("[id$=BatchNo]").val();
                            // CompoundJson.Materials[i].CTD_QUANTITY = batchList[index].STOCK.toFixed(3);
                            CompoundJson.Materials[i].CTD_QUANTITY = containerRow.find("[id$=MaterialQuantity]").val();

                            if (parseFloat(batchList[index].STOCK.toFixed(3)) > reqQuantity)
                                containerRow.find("[id$=MaterialQuantity]").val(reqQuantity);
                            else
                                containerRow.find("[id$=MaterialQuantity]").val(parseFloat(batchList[index].STOCK).toFixed(3));

                            CompoundJson.Materials[i].STOCK = batchList[index].STOCK.toFixed(3);
                            break;
                        }
                    }
                    $("#divData").data("CompoundData", CompoundJson);
                }
            }
            else {
                containerRow.find("[id$=MaterialQuantity]").val('');

            }
            if (containerRow[0].rowIndex == 1) {
                if ($("select[id$=MaterialType]").val() == "1") {
                    var batchID = $("[id$=BatchNo]").val();
                    $.getJSON(CompoundPrep.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
                        if (data != null) {
                            var qtyStock = data[0].SBD_QTY_IN_STOCK;
                            itemStock = qtyStock;
                            $("#tdCurrentStock").html(qtyStock);
                        }
                        else {
                            $("#tdCurrentStock").html(0);
                            itemStock = 0;
                        }
                    });
                }
            }
        }
    }
}

///#endregion

///#region------Calculation Section

function RecalculateQuantity() {
    ///<summary>Method to recalculate total quantity and fill it in the quantity text box

    var objDisp = $("#divData").data("CompoundData");
    var totalQty = 0.0;
    var disprQty = parseFloat($("[id$=CTH_QUANTITY]").val());
    for (var i in objDisp.Materials) {
        totalQty += parseFloat(objDisp.Materials[i].CTD_QUANTITY);
    }
}

function Round(x, y) {
    ///<summary>Method to round decimal no. to given no. of positions</summary>
    /// <param name="x" >
    ///     Input decimal value
    /// </param>
    ///<param name="y" >
    ///     No. of decimal points to be restricted
    /// </param>

    return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);
}

function GetConversionFactor() {
    ////<summary>method to get the conversion factor</summary>

    $.get(CompoundPrep.GetConversionFactor + $("select[id$=DSP_QTY_UOM]").val() + "&UOMTo=" + $("[id$=MaterialQuantityUOM]").val(), function (data) {
        if (data != "" && data != "-1") {
            ConversionFactor = parseFloat(data);
        }
        else {
            ConversionFactor = 1;
        }
    });
}

function AddQtyInputs() {
    ///<summary>Method to add textbox and dropdowns to the grid cells after grid bind</summary>

    var batchDropdown = $("#grdCompounding tr:eq(1) td:eq(2)").clone();
    var qtyTextbx = $("#grdCompounding tr:eq(1) td:eq(4)").clone();
    var catagory = 0;
    var batchColIndex = 0;
    var qtyIndex = 0;
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdCompounding");
            catagory = GrandGrid.Utilities.GetColumnValue($(this), "CPD_ITEM_CATEGORY", "grdCompounding");
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CPD_DRY_PERC", "grdCompounding");
            if (catagory == 2 || catagory == 3) {
                $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
                $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id", (index - 1) + "_BatchNo");
                drpUOMID = $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id");
                // GrandScriptUtils.FillDropDown(drpUOMID, CompoundJson.Materials[index - 1].ItemDetails, true, true);
                FillBatchNo(drpUOMID, CompoundJson.Materials[index - 1].CPD_ITEM_CATEGORY, CompoundJson.Materials[index - 1].CTD_ITEM, CompoundJson.Materials[index - 1].CTD_BATCH, index - 1, true, $(this), CompoundJson.Materials[index - 1]);
                $("#" + drpUOMID).show();
            }
            else if (catagory == 1) {
                $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
                $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id", (index - 1) + "_BatchNo");
                drpUOMID = $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id");
                var batch;
                if (CompoundJson.Materials[index - 1].CTD_STK_BATCH == undefined) {
                    batch = CompoundJson.Materials[index - 1].CTD_BATCH;
                }
                else {
                    batch = CompoundJson.Materials[index - 1].CTD_STK_BATCH;
                }
                FillBatchNo(drpUOMID, CompoundJson.Materials[index - 1].CPD_ITEM_CATEGORY, CompoundJson.Materials[index - 1].CTD_ITEM, batch, index - 1, true, $(this), CompoundJson.Materials[index - 1]);
                $("#" + drpUOMID).show();
            }
            else {
                $(this).find("td:eq(" + batchColIndex + ")").html('');
            }
            if (catagory != null || catagory != " ") {
                $(this).find("td:eq(" + qtyIndex + ")").html(qtyTextbx.html());
                $(this).find("[id$=MaterialQuantity]").live("change", function () {
                    CaptureChanges();
                });

            } //ItemDetails
        }
    });
    $("#divData").data("CompoundData", CompoundJson);

}

//round number 
function roundNumber(num, dec) {
    var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
    return result;
}

function SelectBatch() {
    var isInOrder = true;
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            var material = CompoundJson.Materials[index - 1];
            if (material.CPD_ITEM_CATEGORY >= 1) {
                if ($(this).find("[id$=BatchNo]").val() == 0) {
                    isInOrder = false;
                }
            }
        }
    });
    $("#divData").data("CompoundData", CompoundJson);
    return isInOrder;
}

function BatchQuantity() {
    var isInOrder = true;
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            var material = CompoundJson.Materials[index - 1];
            if ($(this).find("[id$=MaterialQuantity]").val() == 0) {
                isInOrder = false;
            }
        }
    });
    $("#divData").data("CompoundData", CompoundJson);
    return isInOrder;
}

function CaptureChanges(checkStock) {
    ///<summary> Method to capture changes in quantities into the Material objects
    //if Sufficient stock not available returns false;</summary>
    ///<param name="checkStock"> :whether orn not to check stock availability</param>

    var catagory = 0;
    var difference = 0;
    var batchColIndex = 0;
    var qtyIndex = 0;
    var total = 0;
    var isStockInOrder = true;
    var stock = 0.0;
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            var material = CompoundJson.Materials[index - 1];
            material.CTD_QUANTITY = $(this).find("[id$=MaterialQuantity]").val();
            total += parseFloat(material.CTD_QUANTITY);
            //if dispersion/compound capture  batch id (transaction iD)
            if (material.CTD_ITEM_TYPE > 1 || material.CPD_ITEM_CATEGORY > 1) { //if dispersion or compound
                material.CTD_BATCH = $(this).find("[id$=BatchNo]").val();
                material.CTD_STK_BATCH = undefined;
                if (checkStock) {//Check Stock
                    $(this).find("[id$=MaterialQuantity]").css("border", "1px solid #7f9db9");
                    for (var i in material.BatchDetails) {
                        if (material.BatchDetails[i].Value == material.CTD_BATCH) {
                            if (parseFloat(material.BatchDetails[i].STOCK) < parseFloat(CompoundJson.Materials[index - 1].CTD_QUANTITY)) { //not enough stock
                                $(this).find("[id$=MaterialQuantity]").css("border", "1px solid red");

                                isStockInOrder = false;
                            }
                        }
                    }
                }
            }
            else { //if raw material 
                material.CTD_STK_BATCH = $(this).find("[id$=BatchNo]").val();
                material.CTD_BATCH = undefined;
                if (checkStock) {
                    $(this).find("[id$=MaterialQuantity]").css("border", "1px solid #7f9db9");
                    if (CompoundJson.Materials[index - 1].CTD_DFT_ITEM) { //if  it is base quantity conversion factors mustbe checked
                        var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK);
                        var baseQty = CompoundJson.Materials[index - 1].CTD_QUANTITY;
                    }
                    else { ///if it is added materials no conversion factors
                        var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK);
                        var baseQty = CompoundJson.Materials[index - 1].CTD_QUANTITY;
                    }
                    if (baseStk < baseQty) { //not enough stock
                        $(this).find("[id$=MaterialQuantity]").css("border", "1px solid red");
                        isStockInOrder = false;
                    }
                }
            }
        }
    });
    $("#divData").data("CompoundData", CompoundJson);
    return isStockInOrder;
}
//check the total compound quantity against the tank capacity
function IsValidCompoundQty() {
    var IsValidQty = true;
    var CmpQty = parseFloat($("[id$=CTH_QUANTITY]").val());
    var TnkCapacity = parseFloat($("[id$=TankCapacity]").val());
    var CmpUOM = $("select[id$=CTH_QUANTITY_UOM]").val();
    var TnkCapacityUOM = $("select[id$=UOMPk]").val();
    if (CmpUOM == TnkCapacityUOM) {
        if (CmpQty > TnkCapacity) {
            IsValidQty = false;
        }
        else {
            IsValidQty = true;
        }
    }
    else {
       ConversionFactorQty= GetConversionFactorQty(CmpQty, TnkCapacity);
        if (ConversionFactorQty != undefined) {
            var CmpQtyConverted = parseFloat(CmpQty * parseFloat(ConversionFactorQty));
            if (CmpQtyConverted > TnkCapacity) {
                IsValidQty = false;
            }
            else {
                IsValidQty = true;
            }
        }
        else {
            IsValidQty = false;
        }
    }
    return IsValidQty;
}

function GetConversionFactorQty() {
    ////<summary>method to get the conversion factor</summary>

    $.get(CompoundPrep.GetConversionFactor + $("select[id$=CTH_QUANTITY_UOM]").val() + "&UOMTo=" + $("[id$=UOMPk]").val(), function (data) {
        if (data != "" && data != "-1") {
            ConversionFactorQty = parseFloat(data);
        }
        else {
            ConversionFactorQty = 1;
        }
    });

    return ConversionFactorQty;
}


function FillQuantities() {
    ///<summary>///Method to fill scaled quanitities to the input box in the grid cells after filling the grid</summary>
    var difference = 0;
    var text = new Object();
    var currentQty = 0;
    var reqQuantity = 0;
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            $(this).find("[id$=MaterialQuantity]").val(CompoundJson.Materials[index - 1].CTD_QUANTITY);
            if (CompoundJson.Materials[index - 1].CTD_ITEM_TYPE == 1) {//if material type is rawmaterial 
                if (parseFloat(CompoundJson.Materials[index - 1].CTD_QUANTITY) > parseFloat(CompoundJson.Materials[index - 1].STOCK))
                    $(this).find("[id$=MaterialQuantity]").val(CompoundJson.Materials[index - 1].STOCK);
                else
                    $(this).find("[id$=MaterialQuantity]").val(CompoundJson.Materials[index - 1].CTD_QUANTITY);

                currentQty = parseFloat($(this).find("[id$=MaterialQuantity]").val());
                reqQuantity = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), "CTD_REQ_QTY", "grdCompounding"));
                CompoundJson.Materials[index - 1].BALANCE_REQ = reqQuantity - currentQty;

                balanceReq = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_REQ", "grdCompounding");
                $(this).find("td:eq(" + balanceReq + ")").html(CompoundJson.Materials[index - 1].BALANCE_REQ);
            }


            $(this).find("[id$=MaterialQuantityUOM]").remove();
            $(this).find("[id$=MaterialQuantity]").parent().html($(this).find("[id$=MaterialQuantity]"));
            stockIndex = GrandGrid.Utilities.GetColumnIndex($(this), "STOCK", "grdCompounding");
            //Need to check
            $(this).find("td:eq(" + stockIndex + ")").html(CompoundJson.Materials[index - 1].STOCK);
            //$(this).find("td:eq(" + stockIndex + ")").html(CompoundJson.Materials[index - 1].STOCK[0]);
            text = document.createElement("span");
            $(text).html(CompoundJson.Materials[index - 1].UOM_CODE);
            $(text).css("float", "right");
            $(text).css("cssText", "width: 11px !important;");
            $(text).insertAfter($(this).find("[id$=MaterialQuantity]"));
            $(this).find("td:last input[id$=imbEdit]").hide();
            if (CompoundJson.Materials[index - 1].CTD_ITEM_TYPE > 1 || CompoundJson.Materials[index - 1].CTD_ITEM_CATEGORY > 1) { //if material type is dispersion 
                $(this).find("[id$=BatchNo]").val(CompoundJson.Materials[index - 1].CTD_BATCH);
            }
            else if (CompoundJson.Materials[index - 1].CTD_ITEM_TYPE == 1 || CompoundJson.Materials[index - 1].CTD_ITEM_CATEGORY == 1) {
                $(this).find("[id$=BatchNo]").val(CompoundJson.Materials[index - 1].CTD_BATCH);
            }
        }
    });
    $("#divData").data("CompoundData", CompoundJson);

}



function InsertMaterialToTrxObject() {
    ///<summary>Method to adjust the weights according to the required compound weight and add to CompoundPreperation Object
    ///Used:When in FillCompoundDetails() while loading the details for edit or view<summary>
    //  var compoundArray = $("#divArray").data("CompoundArray");

    var materials = CompoundDetailJson.Materials;
    var reqQnty = parseFloat($("[id$=CTH_QUANTITY]").val());
    CompoundJson = $("#divData").data("CompoundData");

    CompoundJson.Materials = new Array();
    if ($.isArray(materials)) {
        for (var i in materials) { //adjust weigth according to required quantiy
            materials[i].CPD_QUANTITY = Round(reqQnty * materials[i].CPD_COMP_PERC * materials[i].CTD_QTY_CONV_FACT / 100, decmlPlace);
            materials[i].CTD_ITEM_TYPE = materials[i].CPD_ITEM_CATEGORY
            materials[i].CTD_ITEM = materials[i].CPD_ITEM == undefined ? materials[i].CTD_ITEM : materials[i].CPD_ITEM;
            materials[i].CTD_PK = $("[id$=CTD_PK]").val()//done by sajeer detail pk is not passing.undefined
            materials[i].CTD_QUANTITY = Round(isNaN(materials[i].CPD_QUANTITY) ? materials[i].CTD_QUANTITY : materials[i].CPD_QUANTITY, decmlPlace);
            materials[i].CTD_QTY_UOM = materials[i].CPD_QTY_UOM;
            if (materials[i].CPD_ACTIVE != undefined)
                materials[i].CTD_ACTIVE = materials[i].CPD_ACTIVE;
            materials[i].BALANCE_REQ = materials[i].CTD_QUANTITY;
            materials[i].CTD_REQ_QTY = materials[i].CTD_QUANTITY;
            if (materials[i].CTD_DFT_ITEM == null) { //add flag indicating whether materials are present inthe Compound formulation
                materials[i].CTD_DFT_ITEM = true;
            }
            CompoundJson.Materials.push(materials[i]) //insert into the compound preperation json
        }
    }
    else {
        materials.CPD_QUANTITY = Round(reqQnty * materials.CPD_COMP_PERC * materials.CTD_QTY_CONV_FACT / 100, 3);
        materials.CTD_ITEM_TYPE = materials.CPD_ITEM_CATEGORY
        materials.CTD_ITEM = materials.CPD_ITEM == undefined ? materials.CTD_ITEM : materials.CPD_ITEM;
        materials.CTD_PK = $("[id$=CTD_PK]").val()//done by sajeer detail pk is not passing.undefined
        materials.CTD_QUANTITY = Round(isNaN(materials.CPD_QUANTITY) ? materials.CTD_QUANTITY : materials.CPD_QUANTITY, 3);
        materials.CTD_QTY_UOM = materials.CPD_QTY_UOM;
        materials.CTD_ACTIVE = materials.CPD_ACTIVE;
        materials.BALANCE_REQ = materials.CTD_QUANTITY;
        materials.CTD_REQ_QTY = materials.CTD_QUANTITY;
        if (materials.CTD_DFT_ITEM == null) { //add flag indicating whether materials are present inthe Compound formulation
            materials.CTD_DFT_ITEM = true;
        }
        CompoundJson.Materials.push(materials)
    }
    $("#divData").data("CompoundData", CompoundJson);
}

function AdjustMaterialWeights() {
    ///<summary>//Method to adjust Material Weights according to changes in Compound weight\n
    ///Called:Before initially filling the compound details to the grid  <summary>

    if ($("[id$=CTH_QUANTITY]").val() != "") {
        var reqQnty = parseFloat($("[id$=CTH_QUANTITY]").val());
        if (reqQnty == NaN || isNaN(reqQnty) || reqQnty == undefined || reqQnty == "undefined") {
            reqQnty = 0;
        }
    }
    else {
        var reqQnty = 0;
    }
    CompoundJson = $("#divData").data("CompoundData");
    for (var i in CompoundJson.Materials) {
        if (CompoundJson.Materials[i].CPD_COMP_PERC != null) {
            CompoundJson.Materials[i].CTD_QUANTITY = Round(reqQnty * CompoundJson.Materials[i].CPD_COMP_PERC * CompoundJson.Materials[i].CTD_QTY_CONV_FACT / 100, 3);
        }
    }
    $("#divData").data("CompoundData", CompoundJson);
}

/// #endregion

///#region data management save,delete

function AddCompoundMaterials() {
    //<summary>function used to add Materials details to Compound
    ///Called:When new material is added to the compound </summary>
    var ItemID = 0;
    var ItemCategory = 0;
    AddValidations(2);

//    if ($("[id$=MaterialType]").val() == 3) {
//        if ($("select[id$=Material]").val() == $("[id$=CTH_COMPOUND]").val()) {
//            GrandScriptUtils.ShowModal(CompoundPrep.SameCompundasMeterial, CompoundPrep.Information, CompoundPrep.Failed);
//            return false;
//        }
//    }

    if ($(document.forms[0]).valid()) {

        var ObjDisp = $("#divData").data("CompoundData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var dispIndx = 0;
        var flag = true;
        var matrialOrBatchFlag = 0;
        var currentBalance = 0;
        var perc = 0;
        if (parseInt(editProduct) == 0) {//Loop used to check the Material already added in the order List
            if ($("select[id$=MaterialType]").val() == "1") {
                for (var i in ObjDisp.Materials) {
                    var stkbatch;
                    if (ObjDisp.Materials[i].CTD_STK_BATCH == undefined) {
                        stkbatch = ObjDisp.Materials[i].CTD_BATCH;
                    }
                    else {
                        stkbatch = ObjDisp.Materials[i].CTD_STK_BATCH;
                    }
                    if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && stkbatch == $("select[id$=BatchNo]").val()) {//ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && ObjDisp.Materials[i].CPD_ITEM_CATEGORY == $("select[id$=MaterialType]").val()  &&
                        flag = false;
                        matrialOrBatchFlag = 1;
                        break;
                    }
                }
            }
            else {
                if ($("select[id$=BatchNo]").val() != 0) {
                    $("#grdCompounding tr:has(td)").each(function (index) {
                        if (flag == true) {
                            if (index > 0) {
                                batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdCompounding");
                                drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
                                CatgID = GrandGrid.Utilities.GetColumnValue($(this), "CPD_ITEM_CATEGORY", "grdCompounding");
                                if (drpUOMID != undefined) {
                                    if ($("select[id$=BatchNo]").val() == $("#" + drpUOMID).val() && $("select[id$=MaterialType]").val() == CatgID) {
                                        flag = false;
                                        matrialOrBatchFlag = 2;
                                    }
                                    else {
                                        flag = true;
                                    }
                                }

                            }
                        }
                    });
                }
            }
        }
        else {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && parseInt(editProduct) != ObjDisp.Materials[i].CTD_ITEM) {
                    flag = false;
                    matrialOrBatchFlag = 1;
                    break;
                }
                if (parseInt(editProduct) == ObjDisp.Materials[i].CTD_ITEM) {
                    obj = ObjDisp.Materials[i];
                }
            }
        }
        if (flag) {//add material to the list

            obj.SLNO = ObjDisp.Materials.length + 1;
            obj.MATERIALTYPENAME = $("[id$=MaterialType] option:selected").text();
            obj.CPD_ITEM_CATEGORY = $("[id$=MaterialType]").val();
            obj.CTD_ITEM = $("[id$=Material]").val();
            obj.ITEM_CODE = $("[id$=CTD_ITEM] option:selected").text();
            obj.MaterialName = $("[id$=Material] option:selected").text();
            obj.CTD_QUANTITY = parseFloat($("input[id$=MaterialQuantity]").val());
            obj.CPD_QUANTITY = obj.CTD_QUANTITY;
            obj.CTD_QTY_UOM = $("[id$=MaterialQuantityUOM]").val();
            obj.UOM_CODE = $("#spnUOM").html();
            obj.CTD_ACTIVE = 1;
            obj.CTD_ITEM_TYPE = obj.CPD_ITEM_CATEGORY;
            obj.CTD_DFT_ITEM = false; //flag indicating whether it is newly added material.
            if ($("[id$=MaterialType]").val() == "1") {
                obj.CTD_STK_BATCH = $("[id$=BatchNo]").val();
            }
            else {
                obj.CTD_BATCH = $("[id$=BatchNo]").val();
            }
            if (obj.CTD_ITEM_TYPE > 1) {//Verify Stock
                var idx = -1;
                for (var i in BatchList) {
                    if (BatchList[i].Value == obj.CTD_BATCH) {
                        idx = i;
                        break;
                    }
                }
                if (idx >= 0) {
                    if (parseFloat(obj.CTD_QUANTITY) > parseFloat(BatchList[idx].STOCK)) {//out of stock
                        GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock, CompoundPrep.Information);
                        $("[id$=MaterialQuantity]").focus();
                        return false;
                    }
                    else {
                        obj.BatchDetails = BatchList;
                        obj.STOCK = BatchList[idx].STOCK;
                    }
                }
            }
            else if (obj.CTD_ITEM_TYPE == 1) {
                if (parseFloat(obj.CTD_QUANTITY) > parseFloat(itemStock)) {
                    GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock, CompoundPrep.Information);
                    $("[id$=MaterialQuantity]").focus();
                    return false;
                }
                else {
                    obj.STOCK = itemStock;
                }
            }

            //Set balance required to list
            obj.CTD_REQ_QTY = "0";
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && ObjDisp.Materials[i].CPD_ITEM_CATEGORY == $("select[id$=MaterialType]").val()) {
                    var Quantity = parseFloat($("input[id$=MaterialQuantity]").val());
                    var balanceReq = parseFloat(ObjDisp.Materials[i].BALANCE_REQ);
                    ItemID = ObjDisp.Materials[i].CTD_ITEM;
                    ItemCategory = ObjDisp.Materials[i].CPD_ITEM_CATEGORY;

                    currentBalance = balanceReq - Quantity;
                    if (currentBalance > 0) {
                        ObjDisp.Materials[i].BALANCE_REQ = currentBalance;
                        obj.BALANCE_REQ = currentBalance;
                    }
                    else {
                        ObjDisp.Materials[i].BALANCE_REQ = 0;
                        obj.BALANCE_REQ = 0;
                    }
                    if (!isNaN(ObjDisp.Materials[i].CTD_REQ_QTY))
                        obj.CTD_REQ_QTY = ObjDisp.Materials[i].CTD_REQ_QTY;
                    else
                        obj.CTD_REQ_QTY = "0";
                    break;
                }
            }

            if (parseInt(editProduct) == 0) {
                ObjDisp.Materials.push(obj);
            }
            $("#divData").data("CompoundData", ObjDisp);
            GrandGrid.MakeGrid($("#grdCompounding"), 0, ObjDisp.Materials);
            ClearMaterialDetails();
            AddQtyInputs();
            FillQuantities();
            // RecalculateQuantity(); 
        }
        else {
            if (matrialOrBatchFlag == 1) {//materila alresdy added
                GrandScriptUtils.ShowModal(CompoundPrep.MaterialAlreadyAdded, CompoundPrep.Information);
            }
            else if (matrialOrBatchFlag == 2)//batch already added
            {
                GrandScriptUtils.ShowModal(CompoundPrep.BatchAlreadyAdded, CompoundPrep.Information);
            }
        }

        return false;
    }

    RemoveGridValidation();

    return false;
}

function ClearMaterialDetails() {
    ///<Summary>Clear material input fields<summary>

    $("select[id$=Material]").val('0');
    $("input[id$=MaterialQuantity]").val('');
    $("select[id$=MaterialQuantityUOM]").val('0');
    $("input[id$=EditProduct]").val('0');
    $("[id$=MaterialName]").html("");
    $("[id$=MaterialType]").val('0');
    $("#tdCurrentStock").html("");
    $("#spnUOM").html("");
    GrandScriptUtils.FillDropDown($("[id$=Material]").attr("id"), new Object(), true, true);
    GrandScriptUtils.FillDropDown($("[id$=BatchNo]").attr("id"), new Object(), true, true);
}

function FillCompoundTrxDetails(compondTrxObj) {
    ///<summary>Method to fill the compound transactions when open for edit/view </summary>
    ///<param name="compondTrxObj" >Compound transaction details object</param>    
    $("[id$=CTH_PK]").val(compondTrxObj.CTH_PK);
    $("[id$=Batch_No]").html(compondTrxObj.CTH_BATCH_NO);
    $("[id$=CTH_COMP_DT]").val(compondTrxObj.CTH_COMP_DT);
    $("[id$=CTH_COMPOUND]").val(compondTrxObj.CTH_COMPOUND);
    //$("[id$=CTH_COMPOUND]").attr("disabled", "disabled");
    $("[id$=CTH_BATCH_NO]").val(compondTrxObj.CTH_BATCH_NO);
    $("[id$=CTH_PLAN]").val(compondTrxObj.CTH_PLAN);
    $("[id$=CTH_START_TM]").val(compondTrxObj.CTH_START_DT);
    $("[id$=CTH_ST_TM]").val(compondTrxObj.CTH_START_TIME);
    $("[id$=CTH_END_TM]").val(compondTrxObj.CTH_END_DT);
    $("[id$=CTH_ED_TM]").val(compondTrxObj.CTH_END_TIME);
    $("[id$=CTH_TOTAL_TM]").val(Round(compondTrxObj.CTH_TOTAL_TM, 2));
    $("[id$=CTH_QUANTITY]").val(Round(compondTrxObj.CTH_QUANTITY, decmlPlace));
    $("[id$=CTH_QUANTITY_UOM]").val(compondTrxObj.CTH_QUANTITY_UOM);
    $("[id$=CTH_TANK_NO]").val(compondTrxObj.CTH_TANK_NO);
    $("[id$=LAST_MOD_DT]").val(compondTrxObj.LAST_MOD_DT);
    if (!($.isArray(compondTrxObj.Materials))) {// Check DispersJson.Materials is Valid Array or Not- 
        var objArray = compondTrxObj.Materials;
        compondTrxObj.Materials = new Array();
        compondTrxObj.Materials.push(objArray);
    }
    if (compondTrxObj.CTH_TEST_TOTAL != "0") {
        $("#lnkViewInspection").html(compondTrxObj.CTH_TEST_PASSED + " out of " + compondTrxObj.CTH_TEST_TOTAL + " passed");
    }
    $("#divData").data("CompoundData", compondTrxObj);
    GrandGrid.MakeGrid($("#grdCompounding"), 0, compondTrxObj.Materials)
    AddQtyInputs();
    FillQuantities();
    $("#divgrdCompoundingGrid").css({ "display": "block", "visibility": "visible" });
    $("#divMaterialInsert").css({ "display": "block", "visibility": "visible" });
}

function FillCheckListDetails(compoundObj) {

    if (!$.isArray(compoundObj.CheckListDtl)) {
        var materialObj = compoundObj.CheckListDtl;
        compoundObj.CheckListDtl = new Array();
        compoundObj.CheckListDtl.push(materialObj);
    }

    $("#divData").data("CheckListDtl", compoundObj.CheckListDtl);

    for (var i in compoundObj.Materials) {
        compoundObj.CheckListDtl[i].SL_NO = i;
    }
    GrandGrid.Utilities.ResetGrid(true, "grdChecklistDetails");
    GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, compoundObj.CheckListDtl);
}

function BindCheckListGrid(chklstId) {
    ///<summary>To handle bind grid </summary>  
    var ajaxUrl = CompoundPrep.GetCheckListDetailsURL + chklstId;
    $.getJSON(ajaxUrl, function (data) {
        if (data) {
            FillCheckListDetails(data);
        }
    });
}

function FetchChecklistGridData() {
    CompoundPrep.CheckListDtl = $("#divData").data("CheckListDtl");
    $("#grdChecklistDetails tr:has(td)").each(function (index) {
        //        if (index > 0) {
        var chklstItem = CompoundPrep.CheckListDtl[index];

        if (chklstItem != undefined) {

            if ($("#txtCDLValue_" + index.toString()).val() != "NaN") {
                chklstItem.CDL_VALUE = $("#txtCDLValue_" + index.toString()).val();
            }
            else {
                $("#txtCDLValue_" + index.toString()).val(chklstItem.CDL_VALUE);
            }

            if ($("#txtCDL_DESC_" + index.toString()).val() != "NaN") {
                chklstItem.CDL_DESC = $("#txtCDL_DESC_" + index.toString()).val();
            }
            else {
                $("#txtCDL_DESC_" + index.toString()).val(chklstItem.CDL_DESC);
            }
        }
    });
    if (CompoundPrep.CheckListDtl != undefined) {
        $("#divData").data("CheckListDtl", CompoundPrep.CheckListDtl);
    }

}


function ShowWkfSubmitPopUp() {
    if (!SelectBatch(true)) {
        GrandScriptUtils.ShowModal(CompoundPrep.SelectBatchNo, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }
    if (!BatchQuantity(true)) {
        GrandScriptUtils.ShowModal(CompoundPrep.EnterQuantity, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }
    $("[id$=CTH_TOTAL_TM]").attr("disabled", false);
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        ShowContainerDivWkf('#divWkfSubmit', CompoundPrep.ConfirmationMessage, '700');
    }
    $("[id$=CTH_TOTAL_TM]").attr("disabled", true);
    return false;
}
function SavePage(command) {
    ///<summary>Method to save the page into the database </summary>
    ///<param name="command" >Command indicate whether to save with workflow or as a draft</summary>
    //Add Validation for Dispersion header Details by setting mode as 1

    if (!SelectBatch(true)) {
        GrandScriptUtils.ShowModal(CompoundPrep.SelectBatchNo, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }
    if (!BatchQuantity(true)) {
        GrandScriptUtils.ShowModal(CompoundPrep.EnterQuantity, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }

    if (!CaptureChanges(true)) {//capture changes in quantities
        GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }
    //check the total compound quantity greater than tank capacity
    if (!IsValidCompoundQty()) {
        GrandScriptUtils.ShowModal(CompoundPrep.InvalidCompoundQty, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }


    $("[id$=CTH_TOTAL_TM]").attr("disabled", false);
    if (command != "Draft") {
        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.       
    }
    else
        $("[id$=ActionID]").val('0'); // save only.
    AddValidations(1);
    var IsPlan = $("[id$=isPlanRequired]").val();
    if (IsPlan == "0") {
        RemovePlanValidation();
    }
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("CompoundData");
        if (ObjDisp.Materials.length > 0) { // Check if the dispersion have alteast 1 material added
            if ($(document.forms[0]).valid()) {

                $("[id$=CTH_COMPOUND]").attr("disabled", '');
                $("[id$=CTH_QUANTITY_UOM]").attr("disabled", '');
                $("[id$=CTH_QUANTITY]").attr("disabled", false);
                $("[id$=CTH_START_TM]").attr("disabled", false);
                $("[id$=CTH_END_TM]").attr("disabled", false);
                $("[id$=CTH_ST_TM]").attr("disabled", false);
                $("[id$=CTH_ED_TM]").attr("disabled", false);
                $("[id$=CTH_COMP_DT]").attr("disabled", false);
                $("[id$=CTH_PLAN]").attr("disabled", false);
                $("[id$=CTH_TANK_NO]").attr("disabled", false);
                $("[id$=CTH_COMPOUND]").attr("disabled", false);
                CaptureChanges();
                var ObjDisp = $("#divData").data("CompoundData");

                $("[id$=CompoundDetail]").val('');
                //To Prevent Duplicate Submission
                if ($("[id$=SubmitFlag]").val() == "0")
                    $("[id$=SubmitFlag]").val('1')
                else
                    return false;

                FetchChecklistGridData();
                if (CompoundPrep.CheckListDtl != undefined) {
                    $("[id$=CheckListDtl]").val(JSON.stringify(CompoundPrep.CheckListDtl));
                }
                $("[id$=CompoundDetailsList]").val(JSON.stringify(ObjDisp.Materials));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                $.ajax({
                    type: "post",
                    url: CompoundPrep.Savepage,
                    data: jSonString,
                    success: function (data) {
                        if ($.isArray(data)) {
                            if (parseInt(data[0]) > 0) {
                                if (command == "Draft") {
                                    GrandScriptUtils.ShowModal(CompoundPrep.SaveMessage1 + " " + data[1] + " " + CompoundPrep.SaveMessage2, CompoundPrep.Information, "saved");
                                }
                                else {// else action - WorkFlow Save
                                    $("[id$=AppNo]").val(data[1]);
                                    $("[id$=hdfAppID]").val(data[0]);
                                    SaveWorkFlow();
                                }
                            }
                            else {
                                var msgtxt;
                                if (parseInt(data[0]) == 0)
                                    msgtxt = CompoundPrep.CodeAlreadyExist;
                                else if (parseInt(data[0]) == -2)
                                    msgtxt = CompoundPrep.SaveMessage1 + data[1] + " "+ CompoundPrep.LockedForEdit;
                                else if (parseInt(data[0]) == -3)
                                    msgtxt = CompoundPrep.NotEnoughStock;
                                else if (parseInt(data[0]) == -5)
                                    msgtxt = CompoundPrep.DeletedRecord;
                                else if (parseInt(data[0]) < 0)
                                    msgtxt = CompoundPrep.ActionFailed;
                                GrandScriptUtils.ShowModal(msgtxt, CompoundPrep.Information, CompoundPrep.Failed);
                            }
                        }
                        else
                            if (parseInt(data) = -3) {
                                msgtxt = CompoundPrep.NotEnoughStock;
                                GrandScriptUtils.ShowModal(msgtxt, CompoundPrep.Information, CompoundPrep.Failed);
                            }
                            else if (parseInt(data) < 0) {
                                msgtxt = CompoundPrep.ActionFailed;
                                GrandScriptUtils.ShowModal(msgtxt, CompoundPrep.Information, CompoundPrep.Failed);
                            }
                        $("[id$=CTH_COMPOUND]").attr("disabled", true);
                        $("[id$=CTH_QUANTITY_UOM]").attr("disabled", true);
                        var ObjCompoundData = $.parseJSON($("[id$=CompoundDetail]").val());
                        if (ObjCompoundData != undefined) {
                            if (ObjCompoundData.CTH_STATUS == 1 || ObjCompoundData.CTH_STATUS == 2 || ObjCompoundData.CTH_STATUS == 7 || ObjCompoundData.CTH_STATUS == 3) {
                                $("[id$=CTH_QUANTITY]").attr("disabled", true);
                                $("[id$=CTH_START_TM]").attr("disabled", true);
                                $("[id$=CTH_END_TM]").attr("disabled", true);
                                $("[id$=CTH_ST_TM]").attr("disabled", true);
                                $("[id$=CTH_ED_TM]").attr("disabled", true);
                                $("[id$=CTH_COMP_DT]").attr("disabled", true);
                                $("[id$=CTH_PLAN]").attr("disabled", true);
                                $("[id$=CTH_TANK_NO]").attr("disabled", true); 
                                $("[id$=CTH_COMPOUND]").attr("disabled", true);
                            }
                        }
                    }
                });
            }
        }
        else {
            GrandScriptUtils.ShowModal(CompoundPrep.PleaseSelectMaterials, CompoundPrep.Information);
        }
    }
    $("[id$=CTH_TOTAL_TM]").attr("disabled", true);
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>

    GrandScriptUtils.ShowModal(CompoundPrep.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + CompoundPrep.SaveMessage2, CompoundPrep.Information, "saved");
}

function GetCompoundDetails() {
    ///<summary>Method to get compound details selected
    //Used :When compound selection changes</summary> 
    var compoundID = $("[id$=CTH_COMPOUND]").val();
    if (compoundID == 0) {
        $("[id$=BtnFillCompound]").attr("disabled", "disabled");
        $("[id$=CTH_QUANTITY_UOM]").val('0');
        $("[id$=CTH_QUANTITY]").val('');
        return false;
    }
    $.get(CompoundPrep.GetCompoundDetail + compoundID + "&DepartmentID=" + $("[id$=hdfDeptID]").val() + "&BizUnitPk=" + $("[id$=BizUnitPk]").val(), function (data) {
        if ((data != null) && data != "") {
            $("[id$=CTH_QUANTITY_UOM]").val('0');
            // $("[id$=CTH_QUANTITY]").val('');
            $("[id$=BtnFillCompound]").attr("disabled", "");

            //BindCheckList Start
            $("[id$=hdfDSPCHECKLISTHDR]").val("");
            if (data["COM_CHECK_LIST_HDR"] != "" && data["COM_CHECK_LIST_HDR"] != undefined) {
                $("[id$=hdfDSPCHECKLISTHDR]").val(data["COM_CHECK_LIST_HDR"]);
                $("#divgrdChecklist").show();
                BindCheckListGrid(data["COM_CHECK_LIST_HDR"]);
            }
            else {
                $("#divgrdChecklist").hide();
            }
            //End


            if ((CompoundDetailJson.CTH_STATUS == "0" || CompoundDetailJson.CTH_STATUS == "6") && parseFloat(CompoundDetailJson.CTH_PK) > 0) {
                CompoundJson.Materials = data.Materials;
                $("[id$=CTH_QUANTITY_UOM]").val(CompoundJson.CTH_QUANTITY_UOM);
                //    $("[id$=CTH_QUANTITY]").val(Round(CompoundJson.CTH_QUANTITY == undefined ? "" : CompoundJson.CTH_QUANTITY, 3));
                InsertMaterialToTrxObject();
                GrandGrid.MakeGrid($("#grdCompounding"), 0, CompoundJson.Materials);
                AddQtyInputs();
                FillQuantities();
                //$("[id$=CTH_COMPOUND]").attr("disabled", "disabled");
                $("[id$=BtnFillCompound]").attr("disabled", "disabled");
            }
            else {
                CompoundDetailJson = data;
                $("[id$=CTH_QUANTITY_UOM]").val(CompoundDetailJson.COM_QTY_UOM);
                $("[id$=CTH_QUANTITY]").val(Round(CompoundDetailJson.COM_QUANTITY == undefined ? "" : CompoundDetailJson.COM_QUANTITY, decmlPlace));
                FillCompoundDetails();
            }
        }
        else {
            $("[id$=BtnFillCompound]").attr("disabled", "disabled");
        }
        return false;
    });
    return false;
}

var slNO = 0;

function DeleteDetails() {
    ///<summary>For delete the item in the grid - Compound Material Details</summary>

    var ObjDisp = $("#divData").data("CompoundData");
    for (var i in ObjDisp.Materials) {
        if ((ObjDisp.Materials[i].SLNO == slNO)) {
            ObjDisp.Materials.splice(i, 1);
            if (ObjDisp.Materials.length != CompoundDetailJson.Materials.length) {
                CompoundDetailJson.Materials.splice(i, 1);
            }
            break;
        }
    }
    $("#divData").data("CompoundData", ObjDisp);
    for (var i in ObjDisp.Materials) {
        ObjDisp.Materials[i].SLNO = i + 1;
    }
    GrandGrid.MakeGrid($("#grdCompounding"), 0, ObjDisp.Materials);
    if (ObjDisp.Materials.length == 0) { //Used to Show the Material  details when the Materials in Dispersion is 0
        $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
        $("#MaterialInsert").show();
        $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });
        $("[id$=CTH_COMPOUND]").attr("disabled", "");
        $("[id$=BtnFillCompound]").attr("disabled", "");
    }
    else {
        AddQtyInputs();
        FillQuantities();
    }
    RecalculateQuantity();
}

function FillCompoundDetails() {
    ///<summary>Used to fill Compound Material Details for editing</summary>

    AddValidations(3);
    //check the total compound quantity greater than tank capacity
    if (!IsValidCompoundQty()) {
        GrandScriptUtils.ShowModal(CompoundPrep.InvalidCompoundQty, CompoundPrep.Information, CompoundPrep.Failed);
        return false;
    }
    if ($(document.forms[0]).valid()) {
        $("#divgrdCompoundingGrid").show();
        $("#divMaterialInsert").show();
        if (!($.isArray(CompoundDetailJson.Materials))) { // Check CompoundDetailJson.Materials is Valid Array or Not- 
            var objArray = CompoundDetailJson.Materials;
            CompoundDetailJson.Materials = new Array();
            CompoundDetailJson.Materials.push(objArray);
        }
        if ((CompoundDetailJson.CTH_STATUS == "0" || CompoundDetailJson.CTH_STATUS == "6") && parseFloat(CompoundDetailJson.CTH_PK) > 0) {
            // GetCompoundDetails();
            if (CompoundDetailJson.Materials.length == 0) {
                GetCompoundDetails();
            }
            InsertMaterialToTrxObject();
            GrandGrid.MakeGrid($("#grdCompounding"), 0, CompoundJson.Materials);
            AddQtyInputs();
            FillQuantities();
        }
        else {
            if (CompoundDetailJson.Materials.length == 0) {
                GetCompoundDetails();
            }
            InsertMaterialToTrxObject();
            GrandGrid.MakeGrid($("#grdCompounding"), 0, CompoundJson.Materials);
            AddQtyInputs();
            FillQuantities();
            //$("[id$=CTH_COMPOUND]").attr("disabled", "disabled");
            $("[id$=BtnFillCompound]").attr("disabled", "disabled");
        }
    }


    return false;
}
function QtyChangebtn() {
    FillQuantities();
    return false;
}

function FillQuantityChange() {
    ///<summary>Method to adjust weights preserving the formulation
    //Used:When Material Quantity Changes</summary>


    ///<summary> Hide on 28-11-2014 
    ///<summary> Chanage Work Only in the Calculate Distribution Click
    //    AdjustMaterialWeights();
    //    FillQuantities();
}


function FillMaterialsForEdit(tr) {
    ////<summary>fill material details for edit
    ///not used</summary>

    $("[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_NAME", $(tr).parent().parent().attr("id")));
    $("textarea[id$=TMD_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_DESC", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_REQD]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, "TMD_REQD", $(tr).parent().parent().attr("id")) == 0 ? false : true);
    SLNo = GrandGrid.Utilities.GetColumnValue(tr, "SL", $(tr).parent().parent().attr("id"));
    $("input[id$=TMD_NAME]").focus();
}

///#region -----gridhandler ---Modal Ok


function GridHandler(tr, command) {
    ///<summary>Grid Handler for grdCompounding Catch all the grid events in this function </summary>

    switch (command.toString().toLowerCase()) {
        case "delete":
            slNO = GrandGrid.Utilities.GetColumnValue(tr, "SLNO", "grdCompounding"); //27/10/11 Uncommented

            GrandScriptUtils.ShowModal(CompoundPrep.DeleteConfirmation, CompoundPrep.ConfirmationMessage, "DeleteDetail", true)
            return false;
            break;
        case "edit":
            $("[id$=imbAdd]").hide();
            $("[id$=imbSave]").show();
            FillMaterialDetails(tr);
            return false;
            break;
        default:
            alert(CompoundPrep.DefaultActionNeeded);
            return false;
            break;
    }
}


function GridHandlerMain(tr, command) {
    ///<summary>Grid Handler for grdDispersionList  Catch all the grid events in this function </summary>

    switch (command.toString().toLowerCase()) {
        case "delete": // To Delete Details 
            DispersID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(CompoundPrep.DoyoWantDelete, CompoundPrep.Confirmation, "deleteDisprsn", true)
            break;
        case "edit": // To Edit Details  
            AddNew();
            FillDetails(tr);
            break;
        default:
            alert(CompoundPrep.DefaultActionNeeded);
            break;
    }
    return false;
}


function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case "deleteMachine":
            BindMachineTypeGrid();
            ClearMachineDetails();
            FillMachineType(0);
            break;
        case "deleteDisprsn":
            DeleteDispersion();
            break;
        case "SaveMachineType":
            SaveMachineType();
            break;
        case "deletemsgMachine":
            DeleteMachineTypeDetails();
            ClearMachineDetails();
            break;
        case "deleted":
            BindGrid();
            break;
        case "saved":
            ResetPage();
            break;
        case "DeleteConversion":
            DeleteConversionDetails();
            break;
        case "failed":
            break;
        case "DeleteDetail":
            DeleteDetails();
            break;
    }
    return false;
}

function BindGrid() {
    ///<summary>Method to bind the main grid</summary>

    var ajaxUrl = CompoundPrep.BindGrid + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdDispersionList").removeAttr("ajaxurl")
    $("#grdDispersionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionList");
    GrandGrid.MakeGrid($("#grdDispersionList"));
}

function removeTableColumn() {
    /*
    removeTableColumn() - Deletes the last column (all the last TDs).
    */
    $('#grdCompounding thead tr th:last').remove(); // Deletes the last title
    $('#grdCompounding tbody tr').each(function () {
        $(this).remove('td:last'); // Should delete the last td for each row, but it doesn't work
    });
}
function RemoveLastColumn() {
    var queryStr = window.location.search.substring(1);
    var queryStr = queryStr.split("&")
    for (var i = 0; i < queryStr.length; i++) {
        var pK = queryStr[i].split("=");
        if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
            $("#grdCompounding th:last").remove();
            $("#grdCompounding tr:has(td)").each(function (index) {
                // $(this).find("td:last").remove();
                $(this).find("[id$=BatchNo]").attr("disabled", true);
                $(this).find("[id$=MaterialQuantity]").attr("disabled", true);

            });
        }
    }
}

function AfterGridBind(grdID) {
    //<summary>function Call After binding Grid</summary>

    if (grdID == "grdCompounding") {

        if (tdset == "") {
            tdset = $("#MaterialInsert").find("tr:eq(1)");
        }
        $("#MaterialInsert").hide();
        $("#MaterialInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdCompounding").show();
        $(tdset).insertBefore($("#grdCompounding").find("tr:eq(1)"));
        var queryStr = window.location.search.substring(1); //for hiding action fields of detail section 23-11-11
        var ObjCompoundData = $.parseJSON($("[id$=CompoundDetail]").val());
        var isViewMode = false;
        var queryStr = window.location.search.substring(1);
        //---------------Old Code
        //        if (queryStr != "") {
        //            if (ObjCompoundData != undefined) {
        //                if (ObjCompoundData.CTH_STATUS ==0 || ObjCompoundData.CTH_STATUS==6) {

        //                    RemoveLastColumn();
        //                }
        //                }
        //                else {
        //                    RemoveLastColumn();
        // 
        //                }

        //        }
        //        if (ObjCompoundData != undefined) {

        //            if (ObjCompoundData.CTH_STATUS == 1 || ObjCompoundData.CTH_STATUS == 7 ) {
        //                $("#grdCompounding th:last").remove();
        //                $("#grdCompounding tr:has(td)").each(function (index) {
        //                    // $(this).find("td:last").remove();
        //                    $(this).find("[id$=BatchNo]").attr("disabled", true);
        //                    $(this).find("[id$=MaterialQuantity]").attr("disabled", true);
        //                });
        //            }

        //            if (ObjCompoundData.CTH_STATUS == 1 || ObjCompoundData.CTH_STATUS == 2 || ObjCompoundData.CTH_STATUS == 7 || ObjCompoundData.CTH_STATUS == 3) {
        //                $("#grdCompounding th:last").remove();
        //                $("#grdCompounding th:last").remove();
        //                $("#grdCompounding tr:has(td)").each(function (index) {
        //                    if (index > 0)
        //                        $(this).find("td:last").remove();
        //                    $(this).find("td:last").hide();
        //                    //                    $(this).find("td:last").hide();
        //                    $(this).find("[id$=BatchNo]").attr("disabled", true);
        //                    $(this).find("[id$=MaterialQuantity]").attr("disabled", true);
        //                    $("[id$=CTH_QUANTITY]").attr("disabled", true);
        //                    $("[id$=CTH_TOTAL_TM]").attr("disabled", true);
        //                    $("[id$=CTH_START_TM]").attr("disabled", true);
        //                    $("[id$=CTH_END_TM]").attr("disabled", true);
        //                    $("[id$=CTH_ST_TM]").attr("disabled", true);
        //                    $("[id$=CTH_ED_TM]").attr("disabled", true);
        //                    $("[id$=CTH_COMP_DT]").attr("disabled", true);
        //                    $("[id$=CTH_PLAN]").attr("disabled", true);
        //                    $("[id$=CTH_TANK_NO]").attr("disabled", true);
        //                    $("[id$=btnCalculate]").hide();
        //                });
        //            }
        //        }
        //-----------new------------
        if (ObjCompoundData != undefined) {
            if (ObjCompoundData.CTH_STATUS == 0 || ObjCompoundData.CTH_STATUS == 6) {
                $("#grdCompounding").show();
            }
            else {
                $("#grdCompounding th:last").remove();
                $("#grdCompounding tr:has(td)").each(function (index) {
                    // $(this).find("td:last").remove();
                    $(this).find("[id$=BatchNo]").attr("disabled", true);
                    $(this).find("[id$=MaterialQuantity]").attr("disabled", true);
                });

                $("#grdCompounding th:last").remove();
                $("#grdCompounding th:last").remove();
                $("#grdCompounding tr:has(td)").each(function (index) {
                    if (index > 0)
                        $(this).find("td:last").remove();
                    $(this).find("td:last").hide();
                    //                    $(this).find("td:last").hide();
                    $(this).find("[id$=BatchNo]").attr("disabled", true);
                    $(this).find("[id$=MaterialQuantity]").attr("disabled", true);
                    $("[id$=CTH_QUANTITY]").attr("disabled", true);
                    $("[id$=CTH_TOTAL_TM]").attr("disabled", true);
                    $("[id$=CTH_START_TM]").attr("disabled", true);
                    $("[id$=CTH_END_TM]").attr("disabled", true);
                    $("[id$=CTH_ST_TM]").attr("disabled", true);
                    $("[id$=CTH_ED_TM]").attr("disabled", true);
                    $("[id$=CTH_COMP_DT]").attr("disabled", true);
                    $("[id$=CTH_PLAN]").attr("disabled", true);
                    $("[id$=CTH_TANK_NO]").attr("disabled", true);
                    $("[id$=CTH_COMPOUND]").attr("disabled", true);
                    $("[id$=btnCalculate]").hide();
                });

            }

        }
    }
    if (grdID == "grdInspectionDetails") {
        var rawInspIndex = 0;
        var rawInspNo = "";
        var trxPK = "";
        $("#grdInspectionDetails tr:has(td)").each(function () {
            rawInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), CompoundPrep.TIH_NO, grdID);
            rawInspNo = GrandGrid.Utilities.GetColumnValue($(this), CompoundPrep.TIH_NO, grdID);
            trxPK = GrandGrid.Utilities.GetColumnValue($(this), CompoundPrep.TIH_PK, grdID);
            if (rawInspIndex != null) {
                $(this).find("td:eq(" + rawInspIndex + ")").html("<a style=\"cursor:pointer\"  onclick=\"javascript:ViewRawMaterialDetails(" + trxPK + ");\" >" + rawInspNo + "</a>");
            }
        });
    }

    if (grdID == "grdRawMaterialInspection") {
        CalculateVariance(grdID);
    }
    if (grdID == "grdCompounding") {
        $("#grdCompounding tr:has(td)").each(function () {
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "STOCK", grdID);
            quantity = GrandGrid.Utilities.GetColumnValue($(this), "STOCK", grdID);
            if (quantity == "undefined")
                $(this).find("td:eq(" + qtyIndex + ")").html("0");
        });
    }
    //CheckList
    if (grdID == "grdChecklistDetails") {
        var ObjCompoundData = $.parseJSON($("[id$=CompoundDetail]").val()); //Forr Viewmode
        var ViewMode = false;
        if (ObjCompoundData != undefined) {
            if (ObjCompoundData.CTH_STATUS == 0 || ObjCompoundData.CTH_STATUS == 6) {
                ViewMode = false;
            }
            else {
                ViewMode = true;
            }
        }
        var CDLValueIndex = 0;
        var CDLValue = "";
        var rowIndex = -1;
        $("#grdChecklistDetails tr:has(td)").each(function (index) {
            itemPK = GrandGrid.Utilities.GetColumnValue($(this), "CDL_PK", grdID);
            colIndexCDLValue = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_VALUE", grdID);
            CDLValue = GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID);

            colIndexCDTDESC = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_DESC", grdID);
            CDL_DESC = GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID);

            if (colIndexCDLValue != null) {
                $(this).find("td:eq(" + colIndexCDLValue + ")").html("");
                if (ViewMode) {
                    $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\"  id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"2\"   disabled=\"disabled\" />");
                }
                else {
                    $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\"  id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"2\"  />");
                }
            }
            if (colIndexCDTDESC != null) {
                $(this).find("td:eq(" + colIndexCDTDESC + ")").html("");
                if (ViewMode) {
                    $(this).find("td:eq(" + colIndexCDTDESC + ")").html("<input type=\"text\" class=\"grdRemarksLarge\"  id=\"txtCDL_DESC_" + index + "\" value=\"" + CDL_DESC + "\" tabIndex=\"3\" disabled=\"disabled\"  />");
                }
                else {
                    $(this).find("td:eq(" + colIndexCDTDESC + ")").html("<input type=\"text\" class=\"grdRemarksLarge\"  id=\"txtCDL_DESC_" + index + "\" value=\"" + CDL_DESC + "\" tabIndex=\"3\"  />");
                }
            }

        });
    }
}

///#endregion

///#region-------Validation Section Clear page
function RemovePlanValidation() {
    $('[id$=CTH_PLAN]').rules("remove");
}
function AddPlanValidation() {
    $('select[id$=CTH_PLAN]').rules("add", {
        selectNone: true,
        messages: { selectNone: CompoundPrep.SelectPlan }
    });
}
function RemoveValidations() {
    //<summary>function Remove Validation</summary>

    $('[id$=CTH_COMPOUND]').rules("remove");
    $('[id$=CTH_START_TM]').rules("remove");
    $('[id$=CTH_END_TM]').rules("remove");
    $("[id$=CTH_ST_TM]").rules("remove");
    $("[id$=CTH_ED_TM]").rules("remove");
    $('[id$=CTH_QUANTITY]').rules("remove");
    $('[id$=CTH_QUANTITY_UOM]').rules("remove");
    $('[id$=CTH_PLAN]').rules("remove");
    $('[id$=CTH_TOTAL_TM]').rules("remove");
    $('[id$=CTH_TANK_NO]').rules("remove");
    $('[id$=MaterialType]').rules("remove");
    $('[id$=Material]').rules("remove");
    $('[id$=BatchNo]').rules("remove");
    $('[id$=MaterialQuantity]').rules("remove");
}

function RemoveGridValidation() {
    ///<summary>Method to remove validations for controls in grid cells</summary>

    $('[id$=MaterialQuantity]:gt(0)').rules("remove");
    $('select[id$=_BatchNo]').rules("remove");
}

function AddValidations(mode) {
    ///<Summary>Add validations to controls<summary>

    RemoveValidations();
    if (mode == "1") {//Mode = 1 represents the validation for Dispersion  Header Details


        $('select[id$=CTH_COMPOUND]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectCompound }
        });
        $('select[id$=CTH_QUANTITY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectUOM }
        });
        $('select[id$=CTH_TANK_NO]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectTank }
        });
        $('select[id$=CTH_PLAN]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectPlan }
        });

        $('[id$=CTH_START_TM]').rules("add", {
            required: true,
            date: true,
            messages: { required: CompoundPrep.EnterStartDate }
        });
        $('[id$=CTH_ST_TM]').rules("add", {
            required: true,
            dateTime: true,
            messages: { required: CompoundPrep.EnterStartTime }
        });
        $('[id$=CTH_END_TM]').rules("add", {
            required: true,
            date: true,
            messages: { required: CompoundPrep.EnterEndDate }
        });
        $('[id$=CTH_ED_TM]').rules("add", {
            required: true,
            dateTime: true,
            messages: { required: CompoundPrep.EnterEndTime }
        });

        $('[id$=CTH_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 14,
            NonZero: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('[id$=CTH_COMP_DT]').rules("add", {
            required: true,
            date: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('[id$=CTH_START_TM]').rules("add", {
            required: true,
            date: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('[id$=CTH_END_TM]').rules("add", {
            required: true,
            date: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('[id$=CTH_END_TM]').rules("add", {
            ValidateEnddateTime: true,           
            messages: { required: CompoundPrep.EndTimeValid }
        });      

    }
    else if (mode == "2") {
        $('select[id$=MaterialType]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectCatagory }
        });
        $('select[id$=Material]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectMaterial }
        });
        if ($("[id$=MaterialType]").val() > 1) {
            $('select[id$=BatchNo]').rules("add", {
                selectNone: true,
                messages: { selectNone: CompoundPrep.SelectBatchNo }
            });
        }
        $('[id$=MaterialQuantity]').rules("add", {
            required: true,
            number: true,
            maxlength: 14,
            NonZero: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('select[id$=BatchNo]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectBatchNo }
        });
    }
    else if (mode == "3") {
        $('select[id$=CTH_COMPOUND]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectCompound }
        });
        $('[id$=CTH_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 14,
            messages: { required: CompoundPrep.EnterQuantity }
        });
        $('select[id$=CTH_QUANTITY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectUOM }
        });
    }
    else if (mode == "4") {
        $('select[id$=_BatchNo]').rules("add", {
            selectNone: true,
            messages: { selectNone: CompoundPrep.SelectBatchNo }
        });
        $('[id$=MaterialQuantity :not(:first)]').rules("add", {
            required: true,
            number: true,
            maxlength: 14,
            NonZero: true,
            messages: { required: CompoundPrep.EnterQuantity }
        });
    }
}

function ClearPage() {
    ///<summary>clear page<summary>

    ResetPage();
    return false;
}


function ResetPage() {
    ///<summary>function Used to Reset Page</summary>

    window.location = "CompoundListing.aspx";
    return false;
}

function Popup() {
    ///<summary>Function used for popup</summary>

    $("#divInspectionDetails").dialog({
        autoOpen: false,
        width: 600,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    $("#divRawMaterialInspection").dialog({
        autoOpen: false,
        width: 900,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}

function ViewInspctionDetails() {
    ///<summary>Function used open a popup</summary>

    BindInspectionDetails();
    $("#divInspectionDetails").dialog("open");
}

function BindInspectionDetails() {
    ///<summary>Function used bind the inspection grid</summary>

    var ajaxUrl = CompoundPrep.GetInspectionDetailsListURL + $("[id$=CTH_PK]").val() + "&BatchType=1";
    $("#grdInspectionDetails").removeAttr("ajaxurl")
    $("#grdInspectionDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdInspectionDetails");
    GrandGrid.MakeGrid($("#grdInspectionDetails"));
    return false;
}

function ViewRawMaterialDetails(trxPK) {
    ///<summary>Function used bind the inspection grid</summary>

    $.getJSON(CompoundPrep.GetRawMaterialInspectionDetails + trxPK,
    function (returnData) {
        var rawMaterialObj = returnData;
        $("[id$=lblRawDate]").html(rawMaterialObj.TIH_DATE);
        $("[id$=lblTestReport]").html(rawMaterialObj.TIH_NO);
        $("[id$=lblTestConductedAt]").html(rawMaterialObj.TIH_CONDUCTED_TEXT);
        $("[id$=lblBatchType]").html(rawMaterialObj.TIH_BATCH_TYPE_TEXT);
        $("[id$=lblLotBatch]").html(rawMaterialObj.TIH_LOT_BATCH);
        $("[id$=lblMaterial]").html(rawMaterialObj.TIH_ITEM_TEXT);
        $("[id$=lblLotSize]").html(rawMaterialObj.TIH_LOT_SIZE + " " + rawMaterialObj.TIH_ITEM_UOM_TEXT);
        $("[id$=lblTest]").html(rawMaterialObj.TIH_TEST_TEXT);
        $("[id$=lblSampleTaken]").html(rawMaterialObj.TIH_SAMPLE_TAKEN + " " + rawMaterialObj.TIH_ITEM_UOM_TEXT);
        $("[id$=lblSampleSize]").html(rawMaterialObj.TIH_SAMPLE_SIZE + " " + rawMaterialObj.TIH_SAMPLE_SIZE_UOM_TEXT);
        if (!$.isArray(rawMaterialObj.Detail)) {
            rawMaterialObj.Detail = [rawMaterialObj.Detail];
        }
        GrandGrid.MakeGrid($("#grdRawMaterialInspection"), 0, rawMaterialObj.Detail);
    });
    $("#divRawMaterialInspection").dialog("open");
}

function CalculateVariance(grdID) {
    ///<summary>Function used calculate the variance and standard value</summary>

    var minValue = 0;
    var maxValue = 0;
    var observedIndex = 0;
    var observedValue = 0;
    var varianceIndex = 0;
    var varience = 0;
    var stdIndex = 0;
    $("[id$=grdRawMaterialInspection]").find("tr:has(td)").each(function (index) {
        minValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), CompoundPrep.TID_MIN_VALUE, grdID));
        maxValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), CompoundPrep.TID_MAX_VALUE, grdID));
        observedValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), CompoundPrep.TID_VALUE, grdID));
        stdIndex = GrandGrid.Utilities.GetColumnIndex($(this), CompoundPrep.TID_STD_VALUE, grdID);
        if (observedValue < minValue) {
            varience = -1 * (minValue - observedValue);
        }
        else if (observedValue > maxValue) {
            varience = observedValue - maxValue;
        }
        else {
            varience = 0;
        }
        if (!isNaN(minValue) || !isNaN(maxValue)) {
            if (minValue == maxValue) {
                $(this).find("td:eq(" + stdIndex + ")").html(minValue);
            }
            else if ((minValue == "") || isNaN(minValue)) {
                $(this).find("td:eq(" + stdIndex + ")").html("<" + maxValue);
            }
            else if ((maxValue == "") || isNaN(maxValue)) {
                $(this).find("td:eq(" + stdIndex + ")").html(">" + minValue);
            }
            else if (maxValue != maxValue) {
                $(this).find("td:eq(" + stdIndex + ")").html(minValue + "-" + maxValue);
            }
        }
        else {
            $(this).find("td:eq(" + stdIndex + ")").html("");
            observedIndex = GrandGrid.Utilities.GetColumnIndex($(this), "TID_VALUE", grdID);
            if (observedValue == "0") {
                $(this).find("td:eq(" + observedIndex + ")").html("Translate(No)");
            }
            else {
                $(this).find("td:eq(" + observedIndex + ")").html("Translate(Yes)");
            }
        }
        varianceIndex = GrandGrid.Utilities.GetColumnIndex($(this), CompoundPrep.TID_VARIANCE, grdID);
        $(this).find("td:eq(" + varianceIndex + ")").html(varience);
    });
}
///#endregion