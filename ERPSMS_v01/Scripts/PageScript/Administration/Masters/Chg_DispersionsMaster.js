/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../../JSLINQ/JSLINQ-vsdoc.js" />

///#region Global variable Declaration
var DispersJson = new Object();
var tdset = "";
var DispersID = 0;
var UOMJson = new Object();
var machineTypeID = 0;
var ConversionFactor = 1;
var toUnitId = 0;
var percAvailable = 100;
var tempQuantity = 0.0;
var deleteMaterialPk = 0;
///#endregion

///#region Configuration section
var DispersionMaster={
FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryList&SBUPk=",
GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory&SBUPk=",
GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
FillMaterialUOMDropdownURL: "MaterialCategory.do?Action=GetUOMNameByCategory&SBUPk=",
GetMaterialUOMConversion: "MaterialManagement.do?Action=GetMaterialUOMConversion&MaterialId=",
MachineTypeDeleteMsg: "Translate(MachineTypeDetailsDeletedSuccessfully)",
MACHINETYPEURL: "MachineryManagement.do?Action=GetMachineType&SBU=",
SAVEMACHINETYPEURL: "MachineryManagement.do?Action=SaveMachineType&SBU=",
GETUOMTYPENAME: "CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
GETCONVERSIONUOM: "CompoundMaster.do?Action=GetConversionUOMList&UOM=",
GETWEIGHTUOMS: "UOMManagement.do?Action=GetUnit&UOMTypeID=1",
GETTIMEUOM: "UOMManagement.do?Action=GetUnit&UOMTypeID=3",
GetConcversionFactors:"CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
//Messages
MachineTypeSaveMsg: "Translate(MachineTypeSavedSuccessfullly)",
 MachineTypeExists: "Translate(MachineTypeAlreadyExists)",
 ActionFailedMsg: "Translate(ActionFailedPleaseTryAgain)",
 InformationTitle: "Translate(Information)",
 QuantityNotReached:"Translate(QuantityNotReached)",
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
  AddMachineType:"Translate(AddMachineType)",
  MaterialAlreadyAdded:'Translate(MaterialAlreadyAdded)',
    ConfirmationMsg: "Translate(Confirmation)",
    //validation msgs
    EnterDispersionName :'Translate(PleaseProvideDispersionName)',
    EnterExpiryTime:'Translate(PleaseProvideExpiryTime)',
   // Fields
   MachineTypeID: "MCT_PK",
   MachineName: "MCT_NAME",
   ValueZero: "0",

   //commands
   MachineTypeEdit: "editmachinetype",
   DeleteMachineTypeCMD: "deleteMachinetype",
   DeleteMaterial: "deletematerial",
   SaveMachineType:"SaveMachineType",
   Unsuccess:"Unsuccess"
 

}


///#endregion

///#region ------ Initialization Section ----------------
///<summary>On Ready function</summary>

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //Initialize page
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

    // Initialize/Load data to the view state
    DispersJson = $.parseJSON($("[id$=DispersionDetailsList]").val());
   // DispersJson = $.parseJSON($("[id$=ConversionList]").val());
    $("#divData").data("DispersionData", DispersJson);



    GrandGrid.Utilities.ResetGrid(true, "grdConversionDtls");
    DispersJson.ConversionList = new Array();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);

});

//<summary>Method to initialize the page</summary>
function PageInit() {

    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
     }, 'Translate(Pleaseselectanoption)');
     $("[id$=machineAdd]").attr("title", "Add Machine Type");
    //seach functionality
    SearchInit();
    SetSearchType();
    //Fill the main grid on load
    BindGrid();
    BindMachineTypeGrid();
    $("#divData").hide();
    $("[id$=imbSave]").hide();
    //fill Material to the Material dropdown
    FillMaterials();
    //Fill UOMs
    FillUOMs();
    FillMachineType();
}

///<summary>To handle auto complete</summary>
function SearchInit() {
    var SBU = parseInt($("[id$=SBU]").val());
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", "DispersionManagement.do?Action=GetSearchValue&SBU="+SBU, "SearchType");
}

///<summary>Function To Enable/Disable Selected Option For Search </summary>
function SetSearchType() {
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

///<summary>Event triggered on Material change </summary>
function MaterialChangeEvent() {
    if ($("[id$=Material]").val() != "0") {
        FillMaterialUOMs($("[id$=DSD_ITEM]").val());
    }
    return false;
}

///<summary>Method the fill the UOM for fields</summary>
function FillUOMs() {

    FillQuantityUOMDropdown();
    FillQuantityUOMDropdouwnGrid();
    FillTimeUOMDropdown();
    FillMaterialCatagory();

}

///<summary>Fill Material quantity UOM</summary>
function FillMaterialUOMs(materialID, materialUOM) {
   //fill UOM of Dispersion quantity
    var categoryID=$("[id$=MaterialCatagory]").val();
    // var materialID = $("[id$=DSD_ITEM]").val();
    var editProduct = $("input[id$=EditProduct]").val();
    var drpUOMID = $("select[id$=DSD_QTY_UOM]").attr("id");
   //$.get(DispersionMaster.FillMaterialUOMDropdownURL + $("[id$=SBU]").val() + "&MatCagID=" + categoryID, function (data) {
    //$.get(DispersionMaster.GetMaterialUOMConversion + $("[id$=DSD_ITEM]").val() + "&UOMId=" + materialUOM, function (data) {
        //  var Url = "MaterialManagement.do?Action=GetMaterialUOM&MatID=" + $("[id$=DSD_ITEM]").val() + "&status=0";
    // $.get(Url, function (data) {


    $.get(DispersionMaster.GETUOMTYPENAME+ materialID, function (data) {
        if (data != " " && data != "-1") {
            //alert("Converted Factor  Value:" + data);
            if (parseInt(data) != 1) {
                GrandScriptUtils.ShowModal(DispersionMaster.MaterialCannotAdded, 'Translate(Information)');
                return false;
            }
        }
    });
    $.get(DispersionMaster.GETCONVERSIONUOM + $("[id$=DSP_QTY_UOM]").val(), function (data) {
        if (materialUOM == null) {
            GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, materialUOM);
        }

    });
   


    var drpID = $("select[id$=MRD_UOM]").attr("id");
    $.get(DispersionMaster.GetMaterialDetails + $("[id$=SBU]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=MaterialName]").html(data[0].ITM_NAME);
              
                if (editProduct > 0) {
                   
                        GetConversionFactor($("[id$=DSP_QTY_UOM]").val(), materialUOM);
                    
                }
                else {
                    $("select[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);
                    GetConversionFactor($("[id$=DSP_QTY_UOM]").val(), data[0].ITM_UOM);
                   
                }


            }
            else {
                $("[id$=MaterialName]").html("");
                $("select[id$=DSD_QTY_UOM]").val("0");
            }

        }

    });


//    $("[id$=DSD_QTY_UOM").val("0");
}
function FillMaterialCatagory(catagoryID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=MaterialCatagory]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value
    $.get(DispersionMaster.FillMaterialCategoryDropdownURL + $("[id$=SBU]").val(), function (data) {
        if (catagoryID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catagoryID);
        }
    });

}
function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
   // FillUOM(categoryID, false);
    FillCategoryMaterials(categoryID);

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
    $.getJSON(DispersionMaster.GetMaterialByCategory + $("[id$=SBU]").val() + "&CategoryID=" + categoryID, function (data) {
        if (materialID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });



}





/////<summary>fills UOMs on the  UOMQuantity in the grid</summary>
function FillQuantityUOMDropdouwnGrid() {
    //fill UOM of Material Quantity
    var drpID = $("select[id$=DSP_QTY_UOM]").attr("id");
    $.get(DispersionMaster.GETWEIGHTUOMS, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

///<summary>Fill quantity UOMs for header</summary>
function FillQuantityUOMDropdown() {

    //fill UOM of Dispersion quantity
    var drpID = $("select[id$=DSP_QTY_UOM]").attr("id");
    //  $.get("DispersionManagement.do?Action=GetUOM&UOMType=1 ", function (data) {
    $.get(DispersionMaster.GETWEIGHTUOMS, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

///<summary>Fill Time uom </summary>
function FillTimeUOMDropdown() {
    var drpID1 = $("select[id$=DSP_EXP_TM_UOM]").attr("id");
    var drpID2 = $("select[id$=DSP_PREP_TM_UOM]").attr("id");
    $.get(DispersionMaster.GETTIMEUOM, function (data) {
        GrandScriptUtils.FillDropDown(drpID1, data, true, true);
        GrandScriptUtils.FillDropDown(drpID2, data, true, true);
    });
}


//<summary>function To Fill Maachine Type Details </summary>
/// <param name="machineTypeID"  type="object"> 
/// </param>
function FillMachineType(machineTypeID) {
    // Get id of the MachineType DropDown
    var drpID = $("select[id$=DSP_MACHINE_TYPE]").attr("id");
    //Fill MachineType Details to the Machine Type DropDown, Name as Text, PK as Value
    $.get(DispersionMaster.MACHINETYPEURL + $("[id$=SBU]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, machineTypeID);
    });
}

///<summary>Method to show the popup to add Machine types</summary>
function AddMachineType() {
    GrandScriptUtils.ShowModalID("divMachineType", DispersionMaster.AddMachineType,DispersionMaster.SaveMachineType );
    return false;
}

//<summary>function used to add Materials details to Dispersion</summary>
function AddDispersionMaterials() {
    //Add Validation for Material Details by setting mode as 2
    
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        var ObjDisp = $("#divData").data("DispersionData");
        var editProduct = $("input[id$=EditProduct]").val();
        var obj = new Object();
        var flag = true;
        var perc = 0;
        //Loop used to check the Material already added in the order List
        if (parseInt(editProduct) == 0) {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].DSD_ITEM == $("select[id$=DSD_ITEM]").val()) {
                    flag = false;
                    break;
                }
            }
        }
        else {
            for (var i in ObjDisp.Materials) {
                if (ObjDisp.Materials[i].DSD_ITEM == $("select[id$=DSD_ITEM]").val() && parseInt(editProduct) != ObjDisp.Materials[i].DSD_ITEM) {
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
            obj.ITEM_CATAGORY_NAME = $("[id$=MaterialCatagory] option:selected").text();
            obj.ITEM_CATAGORY = $("[id$=MaterialCatagory] option:selected").val();
            obj.DSD_ITEM = parseInt($("select[id$=DSD_ITEM]").val());
            obj.ITEM_CODE = $("[id$=DSD_ITEM] option:selected").text();
            obj.ITEM_NAME = $("[id$=MaterialName]").html();
            obj.DSD_QUANTITY = Round(parseFloat($("input[id$=DSD_QUANTITY]").val()), 4);
            obj.DSD_QTY_UOM = $("select[id$=DSD_QTY_UOM] option:selected").val();
            obj.QTY_UOM_NAME = $("select[id$=DSD_QTY_UOM] option:selected").text();
            obj.DSD_QTY_PERC = CalculatePercentage();
            obj.QTY_BASE = Round(GetConverterdQuantity(),4);
            obj.DSD_QUANTITY_TEXT = parseFloat($("input[id$=DSD_QUANTITY]").val()) + "(" + obj.QTY_UOM_NAME + ")";
            if (parseInt(editProduct) == 0) {
                ObjDisp.Materials.push(obj);
            }
            $("#divData").data("DispersionData", ObjDisp);
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
                ObjDisp.Materials.pop();
            }
        }
        else {
            GrandScriptUtils.ShowModal(DispersionMaster.MaterialAlreadyAdded , DispersionMaster.InformationTitle);
        }
        return false;
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

///method to caluclate the base quantities(quantity scaled to the Dispersion Quantity UOM)
//Used In: when Editing existing Dispersion 
function CalculateBaseQuantity() {
    var objDisp = $("#divData").data("DispersionData");
    var totalQty = 0.0;
   // var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
  //  var editPrd = $("[id$=EditProduct]").val();

    for (var i in objDisp.Materials) {
        objDisp.Materials[i].QTY_BASE = Round((objDisp.Materials[i].DSD_QTY_PERC * objDisp.DSP_QUANTITY) / 100, 2);
    }
    $("#divData").data("DispersionData", objDisp);
}

function RecalculatePercentage() {
    var objDisp = $("#divData").data("DispersionData");
    var totalQty = 0.0;
    var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
    var editPrd = $("[id$=EditProduct]").val();
    
    for (var i in objDisp.Materials) {
        if (editPrd != objDisp.Materials[i].DSD_ITEM) {//if editing a row skipp its quantitiy
            totalQty += objDisp.Materials[i].QTY_BASE;
        }
    }
    if (totalQty > disprQty) {
    //if total material qty added is greater than dispersion qty adjust dispersion qty and recalcualte percentages
        
//            //re calculate percentage
//            for (var i in objDisp.Materials) {
//                //TotalQty += ObjDisp.Materials[i].QTY_BASE;
//                objDisp.Materials[i].DSD_QTY_PERC = Round(objDisp.Materials[i].QTY_BASE * 100 / totalQty, 2);
//            }
//            //assign new quantiy to text box
        //            $("[id$=DSP_QUANTITY]").val(totalQty);
        GrandScriptUtils.ShowModal(DispersionMaster.QuantityExceeded, DispersionMaster.InformationTitle,DispersionMaster.Unsuccess  );
        return false;

        }
        else
        {//if not exceeded caluculate % based on actual quantity
         for (var i in objDisp.Materials) {
                //TotalQty += ObjDisp.Materials[i].QTY_BASE;
             objDisp.Materials[i].DSD_QTY_PERC = Round(objDisp.Materials[i].QTY_BASE == null ? 0 : objDisp.Materials[i].QTY_BASE * 100 / disprQty, 2);
            }
            //assign new quantiy to text box
            percAvailable = Round(100- (totalQty * 100 / disprQty),2);
        }

   

        //save object
        $("#divData").data("DispersionData", objDisp);

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

    return  Round((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100) / (parseFloat($("input[id$=DSP_QUANTITY]").val()) * ConversionFactor),2);
}
///<summary>convert to base dispersion quantity uom
function GetConverterdQuantity() {
    return (parseFloat($("input[id$=DSD_QUANTITY]").val())/(ConversionFactor));
}

////<summary>method to get the conversion factor
function GetConversionFactor(from, to) {
    if (from == null) {
       from = $("[id$=DSP_QTY_UOM]").val();
      
    }
    if (to == null) {
     to = $("[id$=DSD_QTY_UOM]").val();
    }

    $.get(DispersionMaster.GetConcversionFactors  + from + "&UOMTo=" + to, function (data) {

        if (data != "" && data != "-1") {

            ConversionFactor = parseFloat(data);
            $("[id$=ImageButton1]").attr("disabled", '');
            //return ((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100*ConversionFactor ) / parseFloat($("input[id$=DSP_QUANTITY]").val()));
        }

        else {

            // no conversion factor exists cannot add material
            GrandScriptUtils.ShowModal(DispersionMaster.SelectAnotherUOM, DispersionMaster.ConfirmationMsg);
            $("[id$=ImageButton1]").attr("disabled", "disabled");
            $("[id$=DSD_ITEM]").val("0");
            ConversionFactor = -1;
            return false;



        }
    });
}


    

///<Summary>Clear material input fields<summary> 
function ClearMaterialDetails() {
    $("select[id$=DSD_ITEM]").val('0');
    $("input[id$=DSD_QUANTITY]").val('');
    $("select[id$=DSD_QTY_UOM]").val('0');
    $("input[id$=EditProduct]").val('0');
    $("[id$=MaterialName]").html("");
    $("[id$=MaterialCatagory]").val('0');
    $("[id$=MaterialCatagory]").val('0');
    $("#tdPercentage").html('');
}

///<Summary>Add validations to controls<summary>
function AddValidations(mode) {
    RemoveValidations();
    //Mode = 1 represents the validation for Dispersion  Header Details
    if (mode == "1") {
        $('input[id$=DSP_NAME]').rules("add", {
            required: true,
            maxlength: 100,
            messages: { required: DispersionMaster.EnterDispersionName  }
        });
        $('input[id$=DSP_EXP_TIME]').rules("add", {
            required: true,
            digits: true,
            maxlength: 3,
            messages: { required: DispersionMaster.EnterExpiryTime  }
        });
        $('input[id$=DSP_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 6,
            messages: { required: 'Translate(PleaseProvideQuantity)' }
        });
        $('input[id$=DSP_PREP_TIME]').rules("add", {
            required: true,
            digits: true,
            maxlength: 3,
            messages: { required: 'Translate(PleaseProvidePreparationTime)' }
        });
        $('input[id$=DSP_CODE]').rules("add", {
            required: true,
            maxlength:50,
            messages: { required: 'Translate(PleaseProvideDispersionCode)' }
        });

        $('select[id$=DSP_MACHINE_TYPE]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaMachineType)' }
        });
        $('select[id$=DSP_QTY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseSelectUOM)' }
        });
        $('select[id$=DSP_EXP_TM_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseSelectUOM)' }
        });
        $('select[id$=DSP_PREP_TM_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseSelectUOM)' }
        });
       
       
    }
    //Mode =  2 represents the validation for Dispersion Material Details
    else if (mode == "2") {
        $('select[id$=DSD_ITEM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseselectaProduct)' }
        });
        $('input[id$=DSD_QUANTITY]').rules("add", {
            required: true,
            number: true,
            maxlength: 6,
            messages: { required: 'Translate(PleaseProvideQuantity)' }
        });
        $('select[id$=DSD_QTY_UOM]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseSelectUOM)' }
        });
        $('select[id$=MaterialCatagory]').rules("add", {
            selectNone: true,
            messages: { selectNone: 'Translate(PleaseSelectCategory)' }
        });
       
    }
}

//<summary>function Remove Validation</summary>
function RemoveValidations() {

    $('input[id$=DSP_NAME]').rules("remove");
    $('input[id$=DSP_EXP_TIME]').rules("remove");
    $('input[id$=DSP_QUANTITY]').rules("remove");
    $('input[id$=DSP_PREP_TIME]').rules("remove");
    $('select[id$=DSP_MACHINE_TYPE]').rules("remove");
    $('select[id$=DSD_ITEM]').rules("remove");
    $('input[id$=DSD_QUANTITY]').rules("remove");
    $('select[id$=DSP_QTY_UOM]').rules("remove");
    $('select[id$=DSP_EXP_TM_UOM]').rules("remove");
    $('select[id$=DSP_PREP_TM_UOM]').rules("remove");
    $('select[id$=DSD_QTY_UOM]').rules("remove");
    $('select[id$=MaterialCatagory]').rules("remove");
    $('input[id$=DSP_CODE]').rules("remove");


}

///<summary>/fill materials in the dropdown</summary>
function FillMaterials(materialID) {
    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    $.getJSON("MaterialManagement.do?Action=GetMaterials", function (data) {
        if (materialID==null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
    });
}

///<summary>Grid Handler for grdDispersionDetails Catch all the grid events in this function </summary>
function GridHandler(tr, command) {
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            deleteMaterialPk = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");
            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, DispersionMaster.ConfirmationMsg, DispersionMaster.DeleteMaterial);
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

///<summary>For delete the item in the grid - Dispersion Details</summary>
function DeleteDetails(tr) {
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
        $("#MaterialInsert").show();
        $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });
       
            $("[id$=DSP_QUANTITY]").attr("disabled", "");
            $("[id$=DSP_QTY_UOM]").attr("disabled", "");
      

    }
   
    RecalculatePercentage();
}

////<summary>fill material details for edit</summary>
function FillMaterialDetails(tr) {

    var catagoryId = GrandGrid.Utilities.GetColumnValue(tr, "ITEM_CATAGORY", $(tr).parent().parent().attr("id"));
    var materialId = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", $(tr).parent().parent().attr("id"));
    var materialUOM = GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", $(tr).parent().parent().attr("id"));
    $("[id$=EditProduct]").val(materialId);
    $("[id$=MaterialCatagory]").val(catagoryId);
    $("select[id$=DSD_ITEM]").val(materialId);
    $("input[id$=DSD_QUANTITY]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_QUANTITY", $(tr).parent().parent().attr("id")));
    $("select[id$=DSD_QTY_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=EditProduct]").val(GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", $(tr).parent().parent().attr("id")));
    $("[id$=MaterialName]").text(GrandGrid.Utilities.GetColumnValue(tr, "ITEM_NAME", $(tr).parent().parent().attr("id")));
    //$("#tdPercentage").html()
    $("#tdPercentage").html(tr.find("td:eq(9)").html());
    $("input[id$=IsEdit]").val("true");
    $("select[id$=DSD_ITEM]").focus();
    RecalculatePercentage();
    FillCategoryMaterials(catagoryId,materialId);
    FillMaterialUOMs( materialId,materialUOM);
}

/////<summary>save dispersion to the database</summary>
function SavePage() {
    //Add Validation for Dispersion header Details by setting mode as 1
    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        $("[id$=DSP_QUANTITY]").attr("disabled", '');
        $("[id$=DSP_QTY_UOM]").attr("disabled", '');
        var ObjDisp = $("#divData").data("DispersionData");
        if (ObjDisp.Materials.length > 0) {
           
            // Check if the dispersion have alteast 1 material added

            //check component quantity sum meet original required quanity
            var totalQty = 0.0;
            var disprQty = parseFloat($("[id$=DSP_QUANTITY]").val());
            for (var i in ObjDisp.Materials) {
                totalQty += ObjDisp.Materials[i].QTY_BASE;
            }
            ///-----
//            if (Round(totalQty,0) < Round(disprQty,0)) {
//                GrandScriptUtils.ShowModal(DispersionMaster.QuantityNotReached, 'Translate(Information)', "Unsuccess");

//                return false;
//            }
            if ($(document.forms[0]).valid()) {
                var ObjDisp = $("#divData").data("DispersionData");
                //Assigning the Material details to a hidden field by converting the object to string using Json Stringify Methord
                $("[id$=DispersionDetailsList]").val(JSON.stringify(ObjDisp.Materials));
                $("[id$=ConversionList]").val(JSON.stringify(ObjDisp.ConversionList));
                var jSonString = GrandScriptUtils.FormToJsonString(false);
                //ajax save request
                $.ajax({
                    type: "post",
                    url: "DispersionManagement.do?Action=SavePage",
                    data: jSonString,
                    contentType: "application/json",
                    dataType: "text",
                    success: function (data) {

                        if (parseInt(data) > 0) {
                            GrandScriptUtils.ShowModal('Translate(DispersionDetailssavedsuccessfully)', 'Translate(Information)', "saved");
                            BindGrid();
                        }
                        else {
                            var msgtxt;
                            if (parseInt(data) == 0)
                                msgtxt = 'Translate(DispersionCodealreadyexists)';
                            else if (parseInt(data) < 0)
                                msgtxt = 'Translate(ActionFailed)';
                            GrandScriptUtils.ShowModal(msgtxt, 'Translate(Status)', "failed");
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

////#endregion

///#region----------Methods for Listing tab----------------------------------------

///<summary>Method to bind the main grid</summary>
function BindGrid() {
    var ajaxUrl = "DispersionManagement.do?Action=GetDispersionList&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdDispersionList").removeAttr("ajaxurl")
    $("#grdDispersionList").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionList");
    GrandGrid.MakeGrid($("#grdDispersionList"));
}

///<summary>event triggered when add new button click</summary>
function AddNew() {
    $("#divData").show();
    $("#divListing").hide();
    $("[id$=imbAdd]").hide();
    $("[id$=imbSave]").show();
    $("id$=DispersionID").val("0");
    $("[id$=DSP_NAME]").focus();

    var dummyObj = new Object();
    //initialize grid
   GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, dummyObj);
    $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
    $("#MaterialInsert").show();
    $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });
    return false;
}

///<summary>To handle events after selecting a value from auto complete</summary>
//filling gridview after entering search value.

function AfterSelect() {
    BindGrid();
}
//<summary>function Call Afer binding Grid</summary>
function AfterGridBind(grdID) {
    if (grdID == "grdDispersionDetails") {

        if (tdset == "") {
            tdset = $("#MaterialInsert").find("tr:eq(1)");
        }
        $("#MaterialInsert").hide();
        $("#MaterialInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdDispersionDetails").show();
        $(tdset).insertBefore($("#grdDispersionDetails").find("tr:eq(1)"));
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

///<summary>fill Dispersion</summary>
function FillDetails(tr) {
   
    //Get OrderID From tr - For Pass this as QueryString
    var DispersionID = GrandGrid.Utilities.GetColumnValue(tr, "DSP_PK", $(tr).parent().parent().attr("id"));
    var objDisp = new Object();
    $.get("DispersionManagement.do?Action=GetDispersionDetail&DispersionID=" + DispersionID, function (data) {
        $("#divData").data("DispersionData", data);
        FillDispersionDetails();

    });
}

///<summary>Used to fill Dispersion Details for editing</summary>
function FillDispersionDetails() {

    RemoveValidations();
    DispersJson = $("#divData").data("DispersionData");
    ///calucate missing fields
    CalculateBaseQuantity();
    //Fill inputs
    $("input[id$=DSP_NAME]").val(DispersJson.DSP_NAME);
    $("input[id$=DSP_EXP_TIME]").val(Round(DispersJson.DSP_EXP_TIME,2));
    $("input[id$=DSP_QUANTITY]").val(Round(DispersJson.DSP_QUANTITY,2));
    $("input[id$=DSP_PREP_TIME]").val(Round(DispersJson.DSP_PREP_TIME,2));
    $("select[id$=DSP_MACHINE_TYPE]").val(DispersJson.DSP_MACHINE_TYPE);
    $("select[id$=DSP_EXP_TM_UOM]").val(DispersJson.DSP_EXP_TM_UOM);
    $("select[id$=DSP_QTY_UOM]").val(DispersJson.DSP_QTY_UOM);
    $("select[id$=DSP_PREP_TM_UOM]").val(DispersJson.DSP_PREP_TM_UOM);
    $("input[id$=DSP_PK]").val(DispersJson.DSP_PK);
    $("[id$=DSP_CODE]").val(DispersJson.DSP_CODE);
    $("select[id$=DSP_QTY_UOM]").attr("disabled", "disabled");
    $("[id$=DSP_QUANTITY]").attr("disabled", "disabled");

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
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersJson.Materials);
    //AfterGridBind();
    GrandGrid.MakeGrid($("#grdConversionDtls"), 0, DispersJson.ConversionList);
    ShowConversionDtls()//adding conversion details
}

///<summary>Delete Dispersion</summary>
function DeleteDispersion() {
    var msgtxt;
    
            $.get("DispersionManagement.do?Action=DeleteDispersion&DispersionID=" + DispersID, function (data) {
             
                //Check Order Deleted Succesfully or Not - 1-Sucess 0-Fail
                if (parseInt(data) >0)
                    msgtxt = 'Translate(DispersionDeletedSuccessfully)';
                else if(parseInt(data) == 0)
                    msgtxt = DispersionMaster.Alreadyasigned; 
                else 
                    msgtxt = 'Translate(ActionFailedPleaseTryAgain)';
                // Show MeesageBox For Order Delete Status
                GrandScriptUtils.ShowModal(msgtxt, 'Translate(Information)',"deleted");
                
            
        });

    return false;
}

///<summary>clear page<summary>
function ClearPage() {
    ResetPage();
    return false;
}

///<summary>function Used to Reset Page</summary>
function ResetPage() {
    //Reseting all input controls in the page



    $(document.forms[0]).find("input:not(input[id=__VIEWSTATE],input[type=button],input[type=submit])").each(function () {
        var idval = $(this).attr("id");
        //Avoid Order ID And Set the value as 0
        if (idval.search("OrderID") != -1) {
            $(this).val('0');
        }
        if (idval.search("EditProduct") != -1) {
            $(this).val('0');
        }
        //Avoid UserPk to get the value of log in user
        else if (idval.search("UserPk") == -1) {
            $(this).val("");
        }

    });

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    //Reseting all text area controls in the page
    $(document.forms[0]).find("textarea").each(function () {
        $(this).val('');
    });

    $("#divData").hide();
    $("#divListing").show();
    $("[id$=imbAdd]").show();
    $("[id$=imbSave]").hide();
    $("[id$=SearchValue]").hide()
    $("[id$=imbSearch]").hide();
    $("[id$=SearchValue]").val("");
    $("[id$=SearchType]").val("0");
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
    RemoveValidations();
    BindGrid();
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
        case DispersionMaster.DeleteMaterial:
            DeleteDetails();
            break;
        case "deleteDisprsn":
            DeleteDispersion();
            break;
        case DispersionMaster.SaveMachineTypes:
            //SaveMachineType();
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
            case DispersionMaster.DeleteMachineTypeCMD:
            DeleteMachineTypeDetails();
            break;
        case DispersionMaster.DeleteMachineTypeSucessesCMD:
            BindMachineTypeGrid();
            ClearMachineTypeDetails();
            FillMachineType();
            break;
    }
    return false;
}
///#endregion


///#region ----------  machine Type Section
//<summary>function To Save Machine Type Details </summary>
function SaveMachineType() {
    RemoveValidations();
    AddMachineTypeValidations();
    if ($(document.forms[0]).valid()) {
        var msgTxt;
        var jSonString = GrandScriptUtils.FormToJsonString("divMachineType");
        $.post(DispersionMaster.SAVEMACHINETYPEURL + $("[id$=SBU]").val(), jSonString, function (data) {
            // Check Machine Type Saved Successfully or Not - >0 Success ,0- Name Already Exists, <0 - Fail(Exception)
            if (parseInt(data) > 0)
                msgTxt = DispersionMaster.MachineTypeSaveMsg;
            // Machine Code Already Exists Or not
            else if (parseInt(data) == 0)
                msgTxt = DispersionMaster.MachineTypeExists;
            else if (parseInt(data) < 0)
                msgTxt = DispersionMaster.ActionFailedMsg;
            // Check The Machine Type  Saved Succesfully or Not
            if (parseInt(data) > 0) {
                FillMachineType(data);
                ClearMachineTypeDetails();
                $("[id$=divMachineType]").dialog("close");
                $("select[id$=DSP_MACHINE_TYPE]").focus();
                return false;
            }
            else {

                GrandScriptUtils.ShowModal(msgTxt, DispersionMaster.InformationTitle, "SaveMachineType");
            }

        });
    }
    return false;

}
//<summary>function To Bind MachineType Details </summary>
function BindMachineTypeGrid() {
    var ajaxUrl = DispersionMaster.MACHINETYPEGRIDURL + $("[id$=SBU]").val();
    $("#grdMachineTypeDtls").removeAttr("ajaxurl")
    $("#grdMachineTypeDtls").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMachineTypeDtls");
    GrandGrid.MakeGrid($("#grdMachineTypeDtls"));
}

//<summary>function To Show Machine Type PopUp - For Entry  </summary>
function AddMachineTypeDetails() {
    ClearMachineTypeDetails();
    $(document.forms[0]).validate().resetForm();
    BindMachineTypeGrid();
//    $("#divMachineType").dialog("open");
//    $("#divMachineType").dialog({ width: 500, height: 350, resizable: true });
    //    $("#divMachineType").css({ "min-height": "300", "margin-top": "25px" });
    $("#divMachineType").dialog({ width: 550, height: 300, buttons: {} });
    $("#divMachineType").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    return false;
}

///<summary>Grid Handler - Machine Type Catch all the grid events in this function </summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function MachineTypeGridHandler(tr, command) {
    switch (command.toString().toLowerCase()) {
        // To Delete Details                              
        case DispersionMaster.MachineTypeDelete:
            machineTypeID = GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineTypeID, $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, DispersionMaster.ConfirmationMsg, DispersionMaster.DeleteMachineTypeCMD, true);
            break;
        // To Edit Details                                       
        case DispersionMaster.MachineTypeEdit:
            FillMachineTypeDetails(tr);
            break;
        default:
            alert(DispersionMaster.DefaultActionMsg);
            break;
    }
    return false;
}

///<summary> Function to Fill  Machine Type Details When Edit Details </summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
function FillMachineTypeDetails(tr) {
    $("input[id$=MachineTypePK]").val(GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineTypeID, $(tr).parent().parent().attr("id")));
    $("input[id$=MachineTypeName]").val(GrandGrid.Utilities.GetColumnValue(tr, DispersionMaster.MachineName, $(tr).parent().parent().attr("id")));
}

///<summary>Clear Machine Type Details </summary>
function ClearMachineTypeDetails() {
    $("input[id$=MachineTypePK]").val(DispersionMaster.ValueZero);
    $("input[id$=MachineTypeName]").val("");
    //$("[id$=lblstarmachine]").hide();
}

///<summary>Delete Vendor Details </summary>
function DeleteMachineTypeDetails() {
    var msgtxt;
    $.get(DispersionMaster.DELETEMACHINETYPEURL + machineTypeID, function (data) {
        // Check Delete Success
        if (parseInt(data) == 1)
            msgtxt = DispersionMaster.MachineTypeDeleteMsg;
        // MachineType Already Assigned or not
        else if (parseInt(data) == 0)
            msgtxt = DispersionMaster.MachineTypeAssignedMsg;
        else
            msgtxt = DispersionMaster.ActionFailedMsg;
        GrandScriptUtils.ShowModal(msgtxt, DispersionMaster.InformationTitle, DispersionMaster.DeleteMachineTypeSucessesCMD);
      
    });
    return false;
}

//<summary>function To Validate PopUp Controls </summary>
/// <param name="mode"  type="object">  
/// </param>
function AddMachineTypeValidations() {
    $("input[id$=MachineTypeName]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Translate(EnterMachineType)" }
    });

}
///#endregion





///#region  Conversion Section =====================================
function AddConversion() {
    RemoveValidations();
    ClearConversionDtls();
    if (DispersJson.ConversionList.length > 0)
        $("#ConversionDiv").show();
    else
        $("#ConversionDiv").hide();
    $("input[id$=btnAddConv]").val("Save");
    $("#divAddConversion").dialog({ width: 550, height: 300, buttons: {} });
    $("#divAddConversion").dialog("open").parents("div:eq(0)").appendTo($(document.forms[0]));
    return false;
}
function ShowConversionDtls() {
    if ($("select[id$=DSP_QTY_UOM]").val() != "0" && $("select[id$=DSP_QTY_UOM]").val() != null) {
        $("[id$=imbAddConversion]").css({ "display": "block", "visibility": "visible" });
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
    var drpIDTo = $("select[id$=UMC_TO]").attr("id");
    $.get("UOMManagement.do?Action=GetUnit&UOMTypeID=1&UOMPK=" + $("select[id$=DSP_QTY_UOM]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpIDTo, data, true, true);
    });
}
function ResetUOMConversion() {

    $("select[id$=UMC_TO]").val("0");
    $("input[id$=UMC_CONV_FACT]").val("");
}
function ClearConversionDtls() {

    $("select[id$=UMC_TO]").val("0");
    $("input[id$=UMC_CONV_FACT]").val("");
    $("input[id$=EditConversion]").val("0");
    frmUnitID = 0;
    toUnitId = 0;
}

function SaveConversionDtls() {
    //$(document.forms[0]).validate().resetForm();
    RemoveValidations();
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

            GrandScriptUtils.ShowModal("Conversion Already Added", "Information");
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
            GrandScriptUtils.ShowModal(DispersionMaster.DoUWantToDelMsg, "Confirmation", "DeleteConversion");
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

///<summary>For delete the item in the grid - Conversion Details List</summary>
function DeleteConversionDetails() {
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

//<summary>function To Validate PopUp Controls </summary>
/// <param name="mode"  type="object">  
/// </param>
function AddConversionValidations() {
    $("input[id$=UMC_CONV_FACT]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Translate(EnterConversionFactor)" }
    });
    $('[id$=UMC_TO]').rules("add", {
        selectNone: true,
        messages: { selectNone: 'Translate(SelectUOM)' }
    });
}

function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
    
    }
function ValidateQuantity() {

    var perc = CalculatePercentage()
    $('#tdPercentage').html(CalculatePercentage())
    if (perc <= percAvailable && perc!=0) {
            
            $("[id$=ImageButton1]").attr("disabled", '');
            $("#tdPercentage").css({ "color": "#506c92", "visibility": "visible" });

        }
        else {
            $("[id$=ImageButton1]").attr("disabled", "disabled")
            $("#tdPercentage").css({ "color": "red", "visibility": "visible" });
        }
  
}
///#endregion