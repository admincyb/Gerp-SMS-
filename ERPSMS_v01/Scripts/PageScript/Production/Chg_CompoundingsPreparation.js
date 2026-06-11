/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />

///#region----Global Variables
var CompoundJson = new Object();
var ConversionFactor = 1;
var CompoundDetailJson = new Object();
var tdset = "";
var materialID = 0;
var BatchList = new Object();
var compoundId = 0;
var itemStock=0;
///#endregion

///#region----Configuration section

var CompoundPrep = {

//URLS
GetCompoundForDropdown:"CompoundPreparation.do?Action=GetCompoundBatchNo",
GetPlanForDropdown:"CompoundPreparation.do?Action=GetPlanListCombo",
FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList",
GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory",
GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
GetCompoundDetail: "CompoundPreparation.do?Action=GetCompoundDetail&compoundID=",
GetBatches: "CompoundPreparation.do?Action=GetBatchesForItem&ItemType=",
GetTankDropdown: "CompoundPreparation.do?Action=GetTankListCombo",


//messages
 DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",
 ConfirmationMessage: "Translate(Conformation)",
  NotEnoughStock:"Translate(NotEnoughStock)",
  Information: "Translate(Information)",

  //constants
  TEXTEMPTY:"",
  TEXTZERO:"0"



   
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

//    $("[id$=SearchType]").change(function () {
//        SetSearchType();
//        if ($("select[id$=SearchType]").val() == "0") {
//            BindGrid();
//        }
//    });

//    $("[id$=imbSearch]").click(function () {
//        BindGrid();
//        return false;
//    });

    CompoundJson = $.parseJSON($("[id$=CompoundDetailsList]").val());
    $("#divData").data("CompoundData", CompoundJson);
//    $("#divData").hide();
    //    $("[id$=imbSave]").hide();
    $("#grdCompounding").css({ "display": "none", "visibility": "hidden" });
    PageInit();

    GrandGrid.Utilities.ResetGrid(true, "grdCompounding");
    //GrandGrid.MakeGrid($("#grdCompounding "), 0, CompoundJson.ConversionList);
   
});

function PageInit() {
    //seach functionality
   
    FillInitialSettings();
    //Fill Dropdowns
//    FillCompounds();
//    FillQuantityUOM();
//    FillPlans();
//    FillTanks();
//    FillTimeUOM();

    // FillMaterialTypes();
    
    var compoundObj = $.parseJSON($("[id$=CompoundDetail]").val());
    var isEdit = false;
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                isEdit = true;
                FillCompoundTrxDetails(compoundObj);
                CompoundJson = compoundObj;
            }
        }
    }
    if (compoundObj != null) {
        FillCompounds(compoundObj.CTH_COMPOUND);
        FillPlans(compoundObj.CTH_PLAN);
        FillQuantityUOM(compoundObj.CTH_QUANTITY_UOM);
        FillTanks(compoundObj.CTH_TANK_NO);
    }
    else {
             FillCompounds();
            FillQuantityUOM();
            FillPlans();
            FillTanks();
            FillTimeUOM();
    }

    BindWorkFlowComment();
}


///<summary>event triggered when add new button click</summary>
function AddNew() {
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
   // $("[id$=lblDate]").text = new Date();
    GrandScriptUtils.FillDate("lblDate", false, true);
    $("[id$=BtnFillCompound]").attr("disabled", "disabled");
    $("[id$=CTH_QUANTITY_UOM]").attr("disabled", "dd-mmm-yyyy disabled");
    $("[id$=CTH_START_TM]").timepicker();
    $("[id$=CTH_END_TM]").datetimepicker({ dateFormat: "dd-mmm-yyyy HH:mm" }); 
    $("#divgrdCompoundingGrid").hide();
    $("#divMaterialInsert").hide();
    $("[id$=imbAdd]").hide();

}
function CalculateEndTM() {
//    var endTm = new Date()
//    var hrs = parseInt($("[id$=CTH_TOTAL_TM]").val());
//    var fromTime=parsed
//    endTm.setHours(endTm.getHours() + hrs);
// //   $("[id$=CTH_END_TM]").val(endTm.getHours());
    
}
function FillCompounds(CompoundID) {

    //<summary>function To Fill Compound material </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=CTH_COMPOUND]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
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
    //fill UOM of Dispersion quantity
    var drpID = $("[id$=CTH_QUANTITY_UOM]").attr("id");
    $.get("UOMManagement.do?Action=GetUnit&UOMTypeID=1", function (data) {
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
    // Get id of the Category DropDown
    var drpID = $("[id$=CTH_PLAN]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(CompoundPrep.GetPlanForDropdown, function (data) {
        if (planID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, planID);
        }
    });
}
function FillTanks(tankID) {
    //<summary>function To Fill Compound material </summary>
    // Get id of the Category DropDown
    var drpID = $("[id$=CTH_TANK_NO]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(CompoundPrep.GetTankDropdown, function (data) {
        if (tankID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, tankID);
        }
    });
}
function FillTimeUOM(uomID){
}
function FillCategoryDetails(materialType) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    // FillUOM(categoryID, false);
    FillMaterials(materialType);

}
function FillMaterialTypes(materialTypeID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=MaterialType]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(CompoundPrep.FillMaterialCategoryDropdownURL , function (data) {
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
        $.get("CompoundMaster.do?Action=GetMaterialName&SBU=" + $("[id$=BizUnitPk]").val() + "&CATG=" + catgID, function (data) {
            if (selValue == null) {
                GrandScriptUtils.FillDropDown(drpID, data,true,true);
            }
            else {
                GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
            }
        });
        if ($("select[id$=MaterialType]").val() > 1) {
            $("[id$=BatchNo]").show();
        }
        else {
            $("[id$=BatchNo]:first").hide();
        }
//        $.get("CompoundMaster.do?Action=GetConversionUOMList&UOM=" + $("[id$=CTH_QUANTITY_UOM]").val(), function (data) {

//            GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);


//        });
    }
        function FillMaterialDetails() {
        ///<summary>Method to fill material details</summary>
//        var catagoryId = $("[id$=MaterialType]").val();
//        var materialID = $("[id$=Material]").val();

        var categoryID = $("[id$=MaterialType]").val();
        var materialID = $("[id$=Material]").val();
        //var drpUOMID = $("select[id$=MaterialQuantityUOM]").attr("id");

        $.get("CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=" + materialID, function (data) {
            if (data != " " && data != "-1") {
                //alert("Converted Factor  Value:" + data);
                if (parseInt(data) != 1) {
                    GrandScriptUtils.ShowModal("Material cannot be added", CompoundPrep.Information);
                    return false;
                }
            }
        });
     

        if (categoryID > 1&&materialID!=0) {

            //fillQuanity()
            FillBatchNo($("[id$=BatchNo]").attr("id"), categoryID, materialID);
        }
        else if (categoryID==1&&materialID!=0) {
           GetMaterialDetails(categoryID,materialID);
        }
        



        $.get(CompoundPrep.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=Material]").val(), function (data) {
            if (data) {
                if (materialID != 0) {

                    //$("select[id$=MaterialQuantityUOM]").val(data[0].ITM_UOM);
                   // $("[id$=MaterialQuantityUOM]").val(data[0].ITM_UOM);
                    $("#spnUOM").html(data[0].UOM_CODE);

                }
                else {
                }

            }

        });


//        $("[id$=MaterialQuantityUOM").val("0");


    }


    
    function FillBatchNo(drId, matCatId, matId, batchId, index) {
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

        var ajaxUrl = '';
        if (drId == null) {
            var drpUOMID = $("select[id$=BatchNo]").attr("id");
            ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId;
        }
        else {
            var drpUOMID = drId;
            ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId;
        }
        $.get(ajaxUrl, function (data) {
            if (data) {
                if (matCatId > 1) {
                    BatchList = data;

                    //for saving the batch details into the Compound data;
                    if (index != null) {
                        var ObjDisp = $("#divData").data("CompoundData");
                        ObjDisp.Materials[index].BatchDetails = data;
                        $("#divData").data("CompoundData", ObjDisp);


                    }

                    if (batchId == null) {

                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);
                    }
                    else {
                        GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, batchId);
                    }

                }

            }
//            else {
//                ObjDisp.Materials[index].STOCK = data[0].STOCK;
//            }

        });


    }
    function GetMaterialDetails(matCatId, matId) {
        ///<summary>Method to get the details of a raw material selected.Get stock,UOM,etc
        ///Used In:When material index changes in the grid
        ///<param name="matCatId">Material Catagory Id</param>
        ///<param name="matId">Material  Id</param>
        ajaxUrl = CompoundPrep.GetBatches + matCatId + "&ItemID=" + matId;
        $.get(ajaxUrl, function (data) {
            if (data) {
                itemStock = data[0].STOCK; //set to current item stock 
                $("[id$=MaterialQuantityUOM]").val(data[0].ITM_UOM);
                $("#spnUOM").html(data[0].UOM_CODE);
            }
        });
    }

   
    function FillBatchQuantity() {
        //<summary> ///Method to fill batch quantity into the quantity text box when batchindex chnages</summary>
    var BatchId= $("[id$=BatchNo]").val();   
    var index=-1;
    for (var i in BatchList) {
        if (BatchList[i].Value == BatchId) {

            index = i;
            break;
        }
    }
    if (index >= 0) {
        $(" #grdCompounding tr:eq(1) [id$=MaterialQuantity]").val(BatchList[index].STOCK);
    }
}
///#endregion



///#region------Calculation Section
function RecalculateQuantity() {
    ///<summary>Method to recalculate total quantity and fill it in the quantity text box
    ///used in :
    var objDisp = $("#divData").data("CompoundData");
    var totalQty = 0.0;
    var disprQty = parseFloat($("[id$=CTH_QUANTITY]").val());
    for (var i in objDisp.Materials) {
        totalQty += parseFloat(objDisp.Materials[i].CTD_QUANTITY);
    }


    //$("[id$=CTH_QUANTITY]").val(totalQty);
    //save object
    //$("#divData").data("CompoundData", objDisp);


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
    $.get("CompoundMaster.do?Action=GetConversionFactor&UOMFrm=" + $("select[id$=DSP_QTY_UOM]").val() + "&UOMTo=" + $("[id$=MaterialQuantityUOM]").val(), function (data) {

        if (data != "" && data != "-1") {

            ConversionFactor = parseFloat(data);
            //return ((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100*ConversionFactor ) / parseFloat($("input[id$=DSP_QUANTITY]").val()));
        }

        else {

            ConversionFactor = 1;

        }
    });
}


function AddQtyInputs() {
///<summary>Method to add textbox and dropdowns to the grid cells after grid bind</summary>
    var batchDropdown = $("#grdCompounding tr:eq(1) td:eq(2)").clone();
    var qtyTextbx = $("#grdCompounding tr:eq(1) td:eq(3)").clone();
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
                FillBatchNo(drpUOMID, CompoundJson.Materials[index - 1].CPD_ITEM_CATEGORY, CompoundJson.Materials[index - 1].CTD_ITEM, CompoundJson.Materials[index - 1].CTD_BATCH, index - 1);
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
}


function CaptureChanges(checkStock) {

    ///<summary> Method to capture changes in quantities into the Material objects
    //if Sufficient stock not available returns false;</summary>
    ///<param name="checkStock"> :whether orn not to check stock availability</param>
    var catagory = 0;
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
                //Check Stock
                if (checkStock) {
                    for (var i in material.BatchDetails) {
                        if (material.BatchDetails[i].Value == material.CTD_BATCH) {
                            stock = material.BatchDetails[i].STOCK;
                            break;
                        }

                    }

                    if (stock < CompoundJson.Materials[index - 1].CTD_QUANTITY) {
                        //not enough stock
                        $(this).find("[id$=MaterialQuantity]").focus();
                        GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock + " for " + CompoundJson.Materials[index - 1].MaterialName, CompoundPrep.ConfirmationMessage);
                        isStockInOrder = false;
                        return false;
                    }
                }
            }
            else {//if raw material
                if (checkStock) {

                    if (CompoundJson.Materials[index - 1].CTD_DFT_ITEM) { //if  it is base quantity conversion factors mustbe checked
                        var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK) * CompoundJson.Materials[index - 1].STOCK_CONV_FACT;
                        var baseQty = CompoundJson.Materials[index - 1].CTD_QUANTITY;
                    }
                    else {///if it is added materials no conversion factors
                        var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK);
                        var baseQty = CompoundJson.Materials[index - 1].CTD_QUANTITY;
                    }
                    if (baseStk < baseQty) {
                        //not enough stock
                        $(this).find("[id$=MaterialQuantity]").focus();
                        GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock + " for " + CompoundJson.Materials[index - 1].MaterialName, CompoundPrep.ConfirmationMessage);
                        isStockInOrder = false;
                        return false;
                    }

                }
            }
        }
    });

//    $("[id$=CTH_QUANTITY]").val(Round(total, 2));
    $("#divData").data("CompoundData", CompoundJson);
    return isStockInOrder;
}


function FillQuantities() {
    ///<summary>///Method to fill scaled quanitities to the input box in the grid cells after filling the grid</summary>
    var catagory = 0;
    var batchColIndex = 0;
    var qtyIndex = 0;
    
    CompoundJson = $("#divData").data("CompoundData");
    $("#grdCompounding tr:has(td)").each(function (index) {
        if (index > 0) {
            $(this).find("[id$=MaterialQuantity]").val(CompoundJson.Materials[index - 1].CTD_QUANTITY);
            $(this).find("[id$=MaterialQuantityUOM]").remove();
            $(this).find("[id$=MaterialQuantity]").parent().html($(this).find("[id$=MaterialQuantity]"));
            var text = document.createElement("span");
            $(text).html(CompoundJson.Materials[index - 1].UOM_CODE);
            $(text).css("float", "none");
            $(text).insertAfter($(this).find("[id$=MaterialQuantity]"));
            //newly added rows are only allowed to be edited.
            //            if (CompoundJson.Materials[index - 1].ISNEWENTRY == undefined) {
            $(this).find("td:last input[id$=imbEdit]").hide();
            //            }
            if (CompoundJson.Materials[index - 1].CTD_ITEM_TYPE > 1 || CompoundJson.Materials[index - 1].CTD_ITEM_CATEGORY > 1) { //if material type is dispersion 
                $(this).find("[id$=BatchNo]").val(CompoundJson.Materials[index - 1].CTD_BATCH);
                //$(this).find("[id$=BatchNo]").attr("disabled", "disabled");
            }
        }
    });
}

function InsertMaterialToTrxObject() {
    ///<summary>Method to adjust the weights according to the required compound weight and add to CompoundPreperation Object
    ///Used:When in FillCompoundDetails() while loading the details for edit or view<summary>
    var materials = CompoundDetailJson.Materials;
    var reqQnty = parseFloat($("[id$=CTH_QUANTITY]").val());
    CompoundJson = $("#divData").data("CompoundData");
    //adjust weigth according to required quantiy
    for (var i in materials) {
        materials[i].CPD_QUANTITY = Round(reqQnty * materials[i].CPD_COMP_PERC * materials[i].CTD_QTY_CONV_FACT / 100, 4);
        materials[i].CTD_ITEM_TYPE = materials[i].CPD_ITEM_CATEGORY
        materials[i].CTD_ITEM = materials[i].CPD_ITEM;
        materials[i].CTD_QUANTITY = materials[i].CPD_QUANTITY;
        materials[i].CTD_QTY_UOM = materials[i].CPD_QTY_UOM;
        materials[i].CTD_ACTIVE = materials[i].CPD_ACTIVE;
        if (materials[i].CTD_DFT_ITEM == null) { //add flag indicating whether materials are present inthe Compound formulation
            materials[i].CTD_DFT_ITEM = true;
        }
        CompoundJson.Materials.push(materials[i]) //insert into the compound preperation json

    }
    $("#divData").data("CompoundData", CompoundJson);
}

function AdjustMaterialWeights() {
    ///<summary>//Method to adjust Material Weights according to changes in Compound weight\n
    ///Called:Before initially filling the compound details to the grid  <summary>
    var reqQnty = parseFloat($("[id$=CTH_QUANTITY]").val());
    CompoundJson = $("#divData").data("CompoundData");
    for (var i in CompoundJson.Materials) {
        if (CompoundJson.Materials[i].CPD_COMP_PERC != null) {
            CompoundJson.Materials[i].CTD_QUANTITY = Round(reqQnty * CompoundJson.Materials[i].CPD_COMP_PERC * CompoundJson.Materials[i].CTD_QTY_CONV_FACT / 100, 4);
        }
      
    }

    $("#divData").data("CompoundData", CompoundJson);


}

/// #endregion



///#region data management save,delete
function AddCompoundMaterials() {
    //<summary>function used to add Materials details to Compound
    ///Called:When new material is added to the compound </summary>
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("CompoundData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var flag = true;
        var perc = 0;
        //Loop used to check the Material already added in the order List
        if (parseInt(editProduct) == 0) {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].CTD_ITEM == $("select[id$=Material]").val() && parseInt(editProduct) != ObjDisp.Materials[i].CTD_ITEM) {
                    flag = false;
                    break;
                }
                if (parseInt(editProduct) == ObjDisp.Materials[i].CTD_ITEM) {
                    obj = ObjDisp.Materials[i];
                }
            }
        }
        //add material to the list
        if (flag) {
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
            // obj.DSD_QTY_PERC = CalculatePercentage();
            //obj.QTY_BASE = GetConverterdQuantity();
            // obj.DSD_QUANTITY_TEXT = parseInt($("input[id$=DSD_QUANTITY]").val()) + "(" + obj.QTY_UOM_NAME + ")";

            //if material type is compound/Dispersion add batch No.

            obj.CTD_BATCH = $("[id$=BatchNo]").val();

            //Verify Stock
            if (obj.CTD_ITEM_TYPE > 1) {

                var idx = 0;
                for (var i in BatchList) {
                    if (BatchList[i].Value == obj.CTD_BATCH) {
                        idx = i;
                        break;
                    }
                }
                if (idx > 0) {
                    if (obj.CTD_QUANTITY > BatchList[idx].STOCK) {//out of stock

                        GrandScriptUtils.ShowModal('Translate(NotEnoughStock)', CompoundPrep.Information);
                        $("[id$=MaterialQuantity]").focus();
                        return false;
                    }
                    else {
                        obj.BatchDetails = BatchList;
                    }
                }
            }
            else if (obj.CTD_ITEM_TYPE == 1) {
            if (obj.CTD_QUANTITY > itemStock) {
                GrandScriptUtils.ShowModal(CompoundPrep.NotEnoughStock  , CompoundPrep.Information);
                $("[id$=MaterialQuantity]").focus();
                return false;
            }
            else {
                obj.STOCK = itemStock;
            }

            }

            if (parseInt(editProduct) == 0) {
                ObjDisp.Materials.push(obj);
            }
            $("#divData").data("CompoundData", ObjDisp);
            GrandGrid.MakeGrid($("#grdCompounding"), 0, ObjDisp.Materials);
            // RecalculatePercentage();
            ClearMaterialDetails();
            AddQtyInputs();
            FillQuantities();
            RecalculateQuantity();
        }
        else {
            GrandScriptUtils.ShowModal('Translate(MaterialAlreadyAdded)', CompoundPrep.Information);
        }
        return false;
    }
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

}

function FillCompoundTrxDetails(compondTrxObj) {
    ///<summary>Method to fill the compound transactions when open for edit/view </summary>
    ///<param name="compondTrxObj" >Compound transaction details object</param>

    $("[id$=CTH_PK]").val(compondTrxObj.CTH_PK);
    $("[id$=Batch_No]").html(compondTrxObj.CTH_BATCH_NO);
    $("[id$=CTH_COMPOUND]").val(compondTrxObj.CTH_COMPOUND);
    $("[id$=CTH_COMPOUND]").attr("disabled","disabled");
    $("[id$=CTH_BATCH_NO]").val(compondTrxObj.CTH_BATCH_NO);
    $("[id$=CTH_PLAN]").val(compondTrxObj.CTH_PLAN);
    $("[id$=CTH_START_TM]").val(compondTrxObj.CTH_START_TM);
    $("[id$=CTH_END_TM]").val(compondTrxObj.CTH_END_TM);
    $("[id$=CTH_TOTAL_TM]").val(Round(compondTrxObj.CTH_TOTAL_TM, 2));
    $("[id$=CTH_QUANTITY]").val(Round(compondTrxObj.CTH_QUANTITY, 2));
    $("[id$=CTH_QUANTITY_UOM]").val(compondTrxObj.CTH_QUANTITY_UOM);
    $("[id$=CTH_TANK_NO]").val(compondTrxObj.CTH_TANK_NO);
    // FillMachine(dispersionObj.DSP_MACHINE_TYPE, compondTrxObj.DTH_MACHINE);
    // Check DispersJson.Materials is Valid Array or Not- 
    // If the List Have Only One Record, need to Create New Array
    // Assign OrderList Details to that Array, and then push Array to DispersJson.Materials
    if (!($.isArray(compondTrxObj.Materials))) {
        var objArray = compondTrxObj.Materials;
        compondTrxObj.Materials = new Array();
        compondTrxObj.Materials.push(objArray);
    }

    $("#divData").data("CompoundData", compondTrxObj);
    GrandGrid.MakeGrid($("#grdCompounding"), 0, compondTrxObj.Materials)
    AddQtyInputs();
    FillQuantities();
    $("#divgrdCompoundingGrid").css({ "display": "block", "visibility": "visible" });
    $("#divMaterialInsert").css({ "display": "block", "visibility": "visible" });
}


function SavePage(command) {
    ///<summary>Method to save the page into the database </summary>
    ///<param name="command" >Command indicate whether to save with workflow or as a draft</summary>

    //Add Validation for Dispersion header Details by setting mode as 1

  //  $("[id$=MaterialList]").val(JSON.stringify(DispersionPreparation.MaterialList));
    if (command != "Draft")
        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
    else
        $("[id$=ActionID]").val('0'); // save only.
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("CompoundData");
        // Check if the dispersion have alteast 1 material added
        if (ObjDisp.Materials.length > 0) {
            if ($(document.forms[0]).valid()) {
                //capture changes in quantities
                if (!CaptureChanges(true)) {
                    return false;
                }
                $("[id$=CTH_COMPOUND]").attr("disabled", '');
                $("[id$=CTH_QUANTITY]").attr("disabled", '');
                $("[id$=CTH_QUANTITY_UOM]").attr("disabled", '');


                var ObjDisp = $("#divData").data("CompoundData");
                //Assigning the Material details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=CompoundDetailsList]").val(JSON.stringify(ObjDisp.Materials));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                //ajax save request
                $.ajax({
                    type: "post",
                    url: "CompoundPreparation.do?Action=SaveCompoundTrxDetails",
                    data: jSonString,
                    contentType: "application/json",
                    dataType: "text",
                    success: function (data) {

                        if (parseInt(data) > 0) {
                            GrandScriptUtils.ShowModal('Translate(CompoundTransactionDetailssavedsuccessfully)', CompoundPrep.Information, "saved");
                           // BindGrid();
                        }
                        else {
                            var msgtxt;
                            if (parseInt(data) == 0)
                                msgtxt = 'Translate(CompoundTransactionCodealreadyexists)';
                            else if (parseInt(data) < 0)
                                msgtxt = 'Translate(ActionFailed)';
                            GrandScriptUtils.ShowModal(msgtxt, 'Translate(Status)', "failed");
                        }
                    }
                });

            }

        }
        else {
            GrandScriptUtils.ShowModal('Translate(PleaseSelectMaterialDetails)', CompoundPrep.Information);
        }
    }
    return false;
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
        $.get(CompoundPrep.GetCompoundDetail + compoundID, function (data) {

            if (data != null) {
                $("[id$=BtnFillCompound]").attr("disabled", "");
                CompoundDetailJson = data;
                $("[id$=CTH_QUANTITY_UOM]").val(CompoundDetailJson.COM_QTY_UOM);
                $("[id$=CTH_QUANTITY]").val(Round(CompoundDetailJson.COM_QUANTITY, 2));
                
            }
            else {
                $("[id$=BtnFillCompound]").attr("disabled", "disabled");
            }
            // FillCompoundDetails();
            return false;
        });
    
    return false;
}


function DeleteDetails() {
    ///<summary>For delete the item in the grid - Compound Material Details</summary>
    var ObjDisp = $("#divData").data("CompoundData");
    for (var i in ObjDisp.Materials) {
        if (ObjDisp.Materials[i].CTD_ITEM == materialID) {
            //Will delete the Material details
            ObjDisp.Materials.splice(i, 1);
            break;
        }
    }
    $("#divData").data("CompoundData", ObjDisp);
    GrandGrid.MakeGrid($("#grdCompounding"), 0, ObjDisp.Materials);
    
    //Used to Show the Material  details when the Materials in Dispersion is 0
    if (ObjDisp.Materials.length == 0) {
        //Will insert the selection tr  into the  MaterialInsert table and show the MaterialInsert Table
        $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
        $("#MaterialInsert").show();
        $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });
    }
    else {
        AddQtyInputs();
        FillQuantities();
    }
    RecalculateQuantity();
//    else {
//        RecalculatePercentage();
//    }
}


function FillCompoundDetails() {
    ///<summary>Used to fill Compound Material Details for editing</summary>
    AddValidations(3);
    if ($(document.forms[0]).valid()) {
        $("#divgrdCompoundingGrid").show();
        $("#divMaterialInsert").show();

        //  RemoveValidations();

        // Check CompoundDetailJson.Materials is Valid Array or Not- 
        // If the List Have Only One Record, need to Create New Array
        // Assign OrderList Details to that Array, and then push Array to CompoundDetailJson.Materials
        if (!($.isArray(CompoundDetailJson.Materials))) {
            var objArray = CompoundDetailJson.Materials;
            CompoundDetailJson.Materials = new Array();
            CompoundDetailJson.Materials.push(objArray);
        }
        InsertMaterialToTrxObject();
        GrandGrid.MakeGrid($("#grdCompounding"), 0, CompoundJson.Materials);

        // AfterGridBind();

        AddQtyInputs();
        FillQuantities();
        $("[id$=CTH_COMPOUND]").attr("disabled", "disabled");
        $("[id$=BtnFillCompound]").attr("disabled", "disabled");
    }
    return false;


}





function FillQuantityChange() {
    ///<summary>Method to adjust weights preserving the formulation
    //Used:When Material Quantity Changes</summary>
    AdjustMaterialWeights();
    FillQuantities();
}


function FillMaterialsForEdit(tr) {
    ////<summary>fill material details for edit
    ///not used</summary>
    $("[id$=MaterialType]").val(GrandGrid.Utilities.GetColumnValue(tr, "CPD_ITEM_CATEGORY", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_NAME]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_NAME", $(tr).parent().parent().attr("id")));
    $("textarea[id$=TMD_DESC]").val(GrandGrid.Utilities.GetColumnValue(tr, "TMD_DESC", $(tr).parent().parent().attr("id")));
    $("input[id$=TMD_REQD]").attr("checked", GrandGrid.Utilities.GetColumnValue(tr, "TMD_REQD", $(tr).parent().parent().attr("id")) == 0 ? false : true);
    SLNo = GrandGrid.Utilities.GetColumnValue(tr, "SL", $(tr).parent().parent().attr("id"));
    //$("input[id$=IsEdit]").val("true");
    $("input[id$=TMD_NAME]").focus();
}
function BindWorkFlowComment() {
    ///<summary>To handle bind grid </summary>

    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
}
///#endregion

///#endregion



///#region -----gridhandler ---Modal Ok

///<summary>Grid Handler for grdCompounding Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            materialID = GrandGrid.Utilities.GetColumnValue(tr, "CTD_ITEM", "grdCompounding");
            GrandScriptUtils.ShowModal(CompoundPrep.DeleteConfirmation, CompoundPrep.ConfirmationMessage, "DeleteDetail", true)
           // DeleteDetails(tr);
            return false;
            break;
        case "edit":
            $("[id$=imbAdd]").hide();
            $("[id$=imbSave]").show();
            FillMaterialDetails(tr);
            return false;
            break;
        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            return false;
            break;
    }
}

///<summary>Grid Handler for grdDispersionList  Catch all the grid events in this function </summary>
function GridHandlerMain(tr, command) {
    switch (command.toString().toLowerCase()) {
        // To Delete Details 
        case "delete":
            // DispersionID = GrandGrid.Utilities.GetColumnValue(tr, "Rmdp", $(tr).parent().attr("id"));

            DispersID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal('Translate(Doyouwanttodeletethisdetails)', 'Translate(Conformation)', "deleteDisprsn", true)
            break;

        // To Edit Details 
        case "edit":
            AddNew();
            FillDetails(tr);
            break;

        default:
            alert('Translate(DefaultActionneedstobeperformed)');
            break;
    }
    return false;

}

//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {
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

///<summary>Method to bind the main grid</summary>
function BindGrid() {
    var ajaxUrl = "DispersionManagement.do?Action=GetDispersionList&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdDispersionList").removeAttr("ajaxurl")
    $("#grdDispersionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionList");
    GrandGrid.MakeGrid($("#grdDispersionList"));
}

//<summary>function Call Afer binding Grid</summary>
function AfterGridBind(grdID) {
    if (grdID == "grdCompounding") {

        if (tdset == "") {
            tdset = $("#MaterialInsert").find("tr:eq(1)");
        }
        $("#MaterialInsert").hide();
        $("#MaterialInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdCompounding").show();
        $(tdset).insertBefore($("#grdCompounding").find("tr:eq(1)"));
    }
}

///<summary>fill Dispersion</summary>
function FillDetails(tr) {

    //Get OrderID From tr - For Pass this as QueryString
    var DispersionID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
    var objDisp = new Object();
    $.get("DispersionManagement.do?Action=GetDispersionDetail&DispersionID=" + DispersionID, function (data) {
        $("#divData").data("CompoundData", data);
        FillDispersionDetails();

    });
}

///#endregion

///#region-------Validation Section Clear page

function RemoveValidations() {
    //<summary>function Remove Validation</summary>

    $('[id$=CTH_COMPOUND]').rules("remove");
    $('[id$=CTH_START_TM]').rules("remove");
    $('[id$=CTH_END_TM]').rules("remove");
    $('[id$=CTH_QUANTITY]').rules("remove");
    $('[id$=CTH_QUANTITY_UOM]').rules("remove");
    $('[id$=CTH_PLAN]').rules("remove");
    $('[id$=CTH_TOTAL_TM]').rules("remove");
    $('[id$=CTH_TANK_NO]').rules("remove");
    $('[id$=MaterialType]').rules("remove");
    $('[id$=Material]').rules("remove");
    $('[id$=BatchNo]').rules("remove");
    $('[id$=MaterialQuantity]').rules("remove");
    //$('[id$=MaterialQuantityUOM]').rules("remove");
   
    


}

function RemoveGridValidation() {
///<summary>Method to remove validations for controls in grid cells</summary>
    $('[id$=MaterialQuantity]:gt(0)').rules("remove");
    $('select[id$=_BatchNo]').rules("remove");
}

function AddValidations(mode) {
    ///<Summary>Add validations to controls<summary>
    RemoveValidations();
    //Mode = 1 represents the validation for Dispersion  Header Details
    if (mode == "1") {

        $('select[id$=CTH_COMPOUND]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectCompound)' }
        });
        $('select[id$=CTH_QUANTITY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectUOM)' }
        });
        $('select[id$=CTH_TANK_NO]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectTank)' }
        });
        $('select[id$=CTH_PLAN]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectPlan)' }
        });

        $('[id$=CTH_START_TM]').rules("add", {
            required: true,
            maxlength: 6,
            messages: { required: 'Translate(EnterStartTime)' }
        });
       
        $('[id$=CTH_END_TM]').rules("add", {
            required: true,
            maxlength: 20,
            messages: { required: 'Translate(EnterEndTime)' }
        });
        $('[id$=CTH_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 10,
            messages: { required: 'Translate(EnterQuantity)' }
        });
        $('[id$=CTH_TOTAL_TM]').rules("add", {
            required: true,
            number: true,
            maxlength: 6,
            messages: { required: 'Translate(EnterTotalTimeInHr)' }
        });
        //grid controlls
//        $('select[id$=_BatchNo]').each(function () {
//            $(this).rules("add", {
//                selectNone: true,
//                messages: { selectNone: 'Translate(SelectBatchNo)' }
//            });
//        });
//        $('select[id$=_BatchNo]').rules("add", {
//                selectNone: true,
//                messages: { selectNone: 'Translate(SelectBatchNo)' }
//            });
//        
//        $('[id$=MaterialQuantity]:gt(0)').each(function () {
//            $(this).rules("add", {
//                required: true,
//                number: true,
//                maxlength: 6,
//                messages: { required: 'Translate(EnterQuantity)' }
//            });
//        });
    }
    else if (mode == "2") {
        $('select[id$=MaterialType]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectCatagory)' }
        });
        $('select[id$=Material]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectMaterial)' }
        });
        $('select[id$=BatchNo]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectBatchNo)' }
        });
        $('[id$=MaterialQuantity]').rules("add", {
            required: true,
            number: true,
            maxlength: 10,
            messages: { required: 'Translate(EnterQuantiy)' }
        });
//        $('select[id$=MaterialQuantityUOM]').rules("add", {
//            selectNone: true,
//            messages: { selectNone: 'Translate(SelectUOM)' }
//        });
    }
    else if (mode == "3") {
        $('select[id$=CTH_COMPOUND]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectCompound)' }
        });
        $('[id$=CTH_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 10,
            messages: { required: 'Translate(EnterQuantity)' }
        });
        $('select[id$=CTH_QUANTITY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectUOM)' }
        });
    }
    else if (mode == "4") {
        $('select[id$=_BatchNo]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(SelectBatchNo)' }
        });
        $('[id$=MaterialQuantity :not(:first)]').rules("add", {
            required: true,
            number: true,
            maxlength: 10,
            messages: { required: 'Translate(EnterQuantity)' }
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
    //Reseting all input controls in the page
//    $(document.forms[0]).find("input").each(function () {
//        var idval = $(this).attr("id");
//        //Avoid Order ID And Set the value as 0
//        if (idval.search("OrderID") != -1) {
//            $(this).val('0');
//        }
//        if (idval.search("EditProduct") != -1) {
//            $(this).val('0');
//        }
//        //Avoid UserPk to get the value of log in user
//        else if (idval.search("UserPk") == -1) {
//            $(this).val("");
//        }

//    });

//    //Selecting the first value in all drop downs
//    $(document.forms[0]).find("select").each(function () {
//        $(this).val($(this).find("option:eq(0)").val());
//    });
//    //Reseting all text area controls in the page
//    $(document.forms[0]).find("textarea").each(function () {
//        $(this).val('');
//    });

//    $("#divData").hide();
//    $("#divListing").show();
//    $("[id$=imbAdd]").show();
//    $("[id$=imbSave]").hide();
//    $("[id$=SearchValue]").hide()
//    $("[id$=imbSearch]").hide();
//    $("[id$=SearchValue]").val("");
//    $("[id$=SearchType]").val("0");
//    $(document.forms[0]).validate().resetForm();
////    var ObjDisp = $("#divData").data("CompoundData");
////    if (ObjDisp != null) {
////        ObjDisp.Materials = new Array();

////    }
//   // GrandGrid.MakeGrid($("#grdCompounding"), 0, ObjDisp.Materials);
////    $("#divData").data("CompoundData", ObjDisp);
//    RemoveValidations();
    //BindGrid();
    window.location = "CompoundListing.aspx";
    return false;
}
///#endregion