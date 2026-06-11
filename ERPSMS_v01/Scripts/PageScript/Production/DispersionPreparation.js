/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/UI/jquery.ui.datetimepicker.js" />

var ConversionFactor = 1;
var percAvailable = 100;
var deleteMaterialPk = 0;
var deptID = 0;
var tdset = "";
var dispersionDecimal = 0;
var Calcbtn = 0;
var calcFlag = 0;
var PopUpBatchQty = 0;
var GridItemGroup = 0;

var DispersJson = new Object();
var DispersionPreparation = {

    //sFillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryRawMaterialList&SBUPk=",
    FillMaterialCategoryDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryListWithoutSemiAndFinished&SBUPk=",
    GetItemNameURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&SearchType=",    
    GetMaterialByCategory: "MaterialManagement.do?Action=GetMaterialByCategory&SBUPk=",
    GetBatches: "CompoundPreparation.do?Action=GetBatchesForItem&ItemType=",
    //
    MaterialCannotAdded: "Translate(MaterialCannotAdded)",
    SelectAnotherUOM: "Translate(SelectAnotherMaterial)",
    ConfirmationMsg: "Translate(Confirmation)",
    QuantityExceeded: "Translate(QuantityExceeded)",
    InformationTitle: "Translate(Information)",
    Unsuccess: "Unsuccess",
    CANCELCONFIRMMSG: "Translate(DoyouwanttocancelSFGdetails)",
    DefaultActionMsg: "Translate(DefaultActionneedstobeperformed)",
    GetUOMListURL: "CompoundMaster.do?Action=GetUOMListForItemAndCompound&UOM=",
    DeleteDispersionPreparationURL: "DispersionPreparation.do?Action=DeleteDispersionPreparation&DispersionID=",
    //
    FillValueTypeDropDownURL: "CommonManagement.do?Action=GetAppConfig&CfgValue=PARAMETER VALUE TYPE",
    FillBatchNoDropDownURL: "MaterialManagement.do?Action=GetBatchNo&SBUPk=",
    FillBatchDetailGetURL: "MaterialManagement.do?Action=GetBatchDetails&SBUPk=",
    FillDispersionBatchDetailGetURL: "MaterialManagement.do?Action=GetBatchDetailsDispersion&SBUPk=",
    GetMaterialName: "CompoundMaster.do?Action=GetMaterialName&SBU=",
    GETUOMTYPENAME: "CompoundMaster.do?Action=GetMaterialUOMTypeName&MatPK=",
    GETCONVERSIONUOM: "CompoundMaster.do?Action=GetConversionUOMList&UOM=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    GetPlanURL: "CommonManagement.do?Action=GetPlans",
    GetDispersionURL: "DispersionManagement.do?Action=GetDispersionAuto&Dept=",
    GetInspectionDetailsListURL: "DispersionPreparation.do?Action=GetInspectionDetails&BatchPK=",
    GetRawMaterialInspectionDetails: "DispersionPreparation.do?Action=GetRawMaterialInspectionDetails&TrxPK=",
    GetMachineURL: "MachineryManagement.do?Action=GetMachineNameByType&MachType=",
    GetUOMURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=1",
    SaveDispersionDetailsURL: "DispersionPreparation.do?Action=SaveDispersionPreparation",
    GetDispersionDetailsURL: "DispersionPreparation.do?Action=GetDispersionDetail&DispersionID=",
    GetCheckListDetailsURL: "CommonManagement.do?Action=GetCheckList&checkListID=",
    DISPERSIONPREPARATIONLISTURL: "SemiFinishedGoodsList.aspx",
    GetMaterialsURL: "DispersionPreparation.do?Action=GetMaterialNameAuto&CATG=",
    MaterialCategroyDeptURL: "MaterialCategory.do?Action=GetMaterialCategoryListAuto",
    MaterialURL: "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore", 

    BatchAlreadyAdded: 'Translate(BOMBatchAlreadyAdded)',
    EnterQuantity: 'Translate(EnterQuantity)',

    DSD_ITEM: "DSD_ITEM",
    DSD_QUANTITY: "DSD_QUANTITY",
    DSD_QTY_PERC: "DSD_QTY_PERC",
    CONVERT_FACTOR: "CONVERT_FACTOR",
    QTY_IN_STOCK: "QTY_IN_STOCK",
    TIH_NO: "TIH_NO",
    TIH_PK: "TIH_PK",
    TID_MIN_VALUE: "TID_MIN_VALUE",
    TID_MAX_VALUE: "TID_MAX_VALUE",
    TID_VALUE: "TID_VALUE",
    TID_VARIANCE: "TID_VARIANCE",
    TID_STD_VALUE: "TID_STD_VALUE",
    DTD_ACTUAL_TSC: "DTD_ACTUAL_TSC",
    TID_MIN_MAX_VALUE: "TID_MIN_MAX_VALUE",
    DTD_MULT_BTCH_GRP: "DTD_MULT_BTCH_GRP",
    DTD_IS_MULT_BATCH: "DTD_IS_MULT_BATCH",
    ITM_TEXT: "ITM_TEXT",
    DSD_ITEM_TYPE: "DSD_ITEM_TYPE",
    DSD_ITEM: "DSD_ITEM",
    ITM_CUR_STK: "ITM_CUR_STK",


    TEXTZERO: "0",
    TEXTEMPTY: "",
    SAVE: "SAVE",
    EDIT: "edit",
    CANCELDISP: "CANCELDISP",
    ValueZero: "0",

    Confirmation: "Translate(Confirmation)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    RecordExist: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    EnterMaterialDetails: "Translate(EnterMaterialDetails)",
    SaveMessage1: "Translate(SFGPreparationSaved1)",
    SaveMessage2: "Translate(SFGPreparationSaved2)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotheruser)",
    NotDispersionStockExists: "Translate(NotSFGMaterialStockExists)",
    NotEnoughStockExists: "Translate(NotEnoughStock)",
    ConfirmationMessage: "Translate(Conformation)",
    DeleteConfirmation: "Translate(Doyouwanttodeletethisdetails)",
    SelectCategory: "Translate(SelectCategory)",
    SelectItem: "Translate(SelectItem)",
    SelectBatchNo: "Translate(SelectBatchNo)",
    Status: 'Translate(Status)',
    CONFIRMMSG: "Translate(Conformation)",
    CANCELSUCESS: "Translate(SFGPreparationCancelledSuccessfully)",
    CANCELFAILED: "Translate(SFGPreparationAlreadyUsed)",
    Failed: "failed",
    IsViewMode: false,
    DispertionList: new Array(),
    DispertionObj: new Object(),
    PopUpBatchList: new Array(),
    PopUpBatchObj: new Object(),
    //DispersionSaveList: new Array()
}

$(document).ready(function () {

    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {
        return ($(element).val() != "Translate(AutoDefaultValue)");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("dateTime", function (value) {
        return /^([01]?[0-9]|2[0-3]):[0-5][0-9]?$/.test(value);
    }, "Translate(EnterValidTime)");
    $.validator.addMethod("exceptZero", function (value, element) {
        return ($(element).val() != "0.00");
    }, "Translate(UnLoadTimeValid)");
    $("select[id$=ddlValueType]").change(onValuTypeChangeCallBack);
    dispersionDecimal = $("[id$=hdfCompoundingDecimal]").val();
    PageInit();
});

function PageInit() {
    ////<summary>function used to initial data </summary>    
    ShowHideAdvancedSearch(0);
    FillValueType();
    $("[id$=DTH_DEPT]").val($("[id$=hdfDeptID]").val());
    deptID = $("[id$=hdfDeptID]").val();
    $('#divMaterialInsert').hide();
    var queryString = window.location.search.substring(1);
    if (queryString != "") {
        var queryStr = queryString.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                DispersionPreparation.IsViewMode = true;
            }
            else if (pK[1] == 2 && pK[0] == "Status") {
                DispersionPreparation.IsViewMode = true;
                $("[id$=btnCancelSubmit]").show();
            }
        }
    }
    //Popup();
    var dispersionObj = $.parseJSON($("[id$=MaterialList]").val());
    $("[id$=hdfDispPK]").val(dispersionObj.DTH_PK);
    $("#divData").data("dispersionObj", dispersionObj);
    DateInit();
    var isEdit = false;


    if (dispersionObj.DTH_PK != undefined && dispersionObj.DTH_PK > 0) {
        isEdit = true;
        FillDetails(dispersionObj);
        /*
        //CheckList
                if (dispersionObj.CheckListDtl != undefined) {
                    if (!$.isArray(dispersionObj.CheckListDtl)) {
                        var materialObj = dispersionObj.CheckListDtl;
                        dispersionObj.CheckListDtl = new Array();
                        dispersionObj.CheckListDtl.push(materialObj);

                        DispersionPreparation.CheckListDtl = new Array();
                        DispersionPreparation.CheckListDtl.push(materialObj);
                    }

                    $("#divData").data("CheckListDtl", dispersionObj.CheckListDtl);
                    //GrandGrid.Utilities.ResetGrid(true, "grdChecklistDetails");
                    GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, dispersionObj.CheckListDtl);
                    //endchecklist
                    $("#divgrdChecklist").show();
                }
                else {
                    $("#divgrdChecklist").hide();
                }
            }
            else {
                $("#divgrdChecklist").hide();
                */
    }
    FillPlan(dispersionObj.DTH_PLAN, dispersionObj.PLN_NAME, dispersionObj.DTH_STATUS);
    FillDispersion(dispersionObj.DTH_DISPERSION, dispersionObj.DSP_NAME_TEXT, dispersionObj.DTH_STATUS);
    FillQuantityUOM(dispersionObj.DTH_QTY_UOM);
    if (!$.isArray(dispersionObj.MaterialList)) {
    DispersionPreparation.MaterialList = new Array();
        var materialObj = dispersionObj.MaterialList;
        dispersionObj.MaterialList = new Array();
        dispersionObj.MaterialList.push(materialObj);
        DispersionPreparation.MaterialList.push(materialObj);
    }
    $("#divData").data("MaterialData", dispersionObj.MaterialList);
    DispersionPreparation.MaterialList=$("#divData").data("MaterialData");
    DispersionPreparation.DispertionList=$("#divData").data("MaterialData");    

    //    $("#divSaveData").data("SaveData", DispersionPreparation.MaterialList);
    //$("#divSaveData").data("SaveData", dispersionObj.MaterialList);

    if (dispersionObj.MaterialList[0] != undefined) {
        for (var i in dispersionObj.MaterialList) {
            dispersionObj.MaterialList[i].SL_NO = i;
            dispersionObj.MaterialList[i].DTD_ITEM_TYPE = dispersionObj.MaterialList[i].DSD_ITEM_TYPE;
        }
    }
    //SetEditModeData(dispersionObj.MaterialList);
    GrandGrid.Utilities.ResetGrid(true, "grdDispersionDetails");
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, dispersionObj.MaterialList);
    highliteStock();
    //AddQtyInputs();

    DispersionPreparation.MaterialList = new Array();
    DispersionPreparation.MaterialList = AddToMatarialList();
    // Initialize/Load data to the view state
    DispersJson = $.parseJSON($("[id$=MaterialList]").val());

    // DispersJson = $.parseJSON($("[id$=ConversionList]").val());
    $("#divData").data("DispersionData", DispersJson);
    //$('#divgrdDispersionDetailsGrid').hide()
    FillCategoryDetails(0);
    //FillMeterialBatchNo();
    FillUOMs();
    $("[id$=DTH_DATE]").focus();
    // FillMaterialAuto(0, 0);
}

function FillValueType() {
    var drpID = $("select[id$=ddlValueType]").attr("id");
    $.get(DispersionPreparation.FillValueTypeDropDownURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

//Working
function onValuTypeChangeCallBack() {
    var selectedID = $("select[id$=ddlValueType]").val();
    $('#rowValueTemplate').show();
    switch (selectedID) {
        case "0":
            $('#rowValueTemplate').hide();
            break;
        case "1":
            $('#divValueInputContainer').html('');
            $('#divValueInputContainer').html("<input type='checkbox' id='txtValue' tabIndex='5' />");
            break;
        case "2":
            $('#divValueInputContainer').html('');
            $('#divValueInputContainer').html("<input type='text' class='input-w150' id='txtValue' value='' tabIndex='16' onkeypress='javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);' />");
            break;
        case "3":
            $('#divValueInputContainer').html('');
            $('#divValueInputContainer').html("<input type='text' class='input-w150' id='txtValue' value='' tabIndex='16' />");
            break;
        case "4":
            $('#divValueInputContainer').html('');
            $('#divValueInputContainer').html("<input type='text' class='input-w150' id='txtValue' value='' tabIndex='16' MaxLength='13' onkeydown='return CheckKey(event)' onpaste='return false;' />");
            GrandScriptUtils.DatePickerCommon("txtValue");
            break;
        default:
    }
}

function FillUOMs() {
     FillMaterialCategoryAutoComplete(); //FillMaterialCatagory();
}

function FillMaterialNames(categoryID, selValue) {
    ///<summary>Method to fill material names to material dropdown</summary>

    ClearOnMaterialCategoryDropChange();
    if (categoryID == 0) {
        FillMaterialAuto(0, categoryID);
        return false;
    }

    FillMaterialAuto(0, categoryID);
    //    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    //    $.get(DispersionPreparation.GetMaterialName + $("[id$=BizUnitPk]").val() + "&CATG=" + categoryID, function (data) {
    //        if (selValue == null) {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    //        }
    //        else {
    //            GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    //        }
    //    });
}

function FillMaterialAuto(selValue, catID) {
    //GrandScriptUtils.MakeAutoCompleteAdvance("DSD_ITEM_TEXT", DispersionPreparation.GetMaterialsURL + catID, "DSD_ITEM", true, false, false, true);
    var drpID = $("select[id$=DSD_ITEM]").attr("id");
    $.getJSON(DispersionPreparation.GetItemNameURL + $("select[id$=MaterialCatagory]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
    });
}

function FillMaterialCatagory(catagoryID) {
    //<summary>function To Fill Category Details </summary>
    // Get id of the Category DropDown
    var drpID = $("select[id$=MaterialCatagory]").attr("id");
    //Fill Category Details to the Category DropDown, Name as Text, PK as Value    
    $.get(DispersionPreparation.FillMaterialCategoryDropdownURL + $("[id$=BizUnitPk]").val()+"&ITCVAL=1", function (data) {
        if (catagoryID == null) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, catagoryID);
        }
    });

}
//#region----------- Validation Section----------------

function AddValidations(mode) {

    //<summary>Function used to assign validation</summary>
    //RemoveValidations();
    if (mode == 2) {
        $("[id$=MaterialCatagory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItemCategory)" }
        });
        $("[id$=DSD_ITEM]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectItem)" }
        });
        //        $("select[id$=DSD_QTY_UOM]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectUOM)" }
        //        });
        $("[id$=DSD_QUANTITY]").rules("add", {
            required: true,
            NonZero: true,
            messages: { required: "Translate(EnterQty)" }
        });       
        //        $("[id$=BatchNo]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectBatchNo)" }
        //        });

        if ($("#ITM_CUR_STK").text() != "")
            $("[id$=DSD_QUANTITY]").rules("add", {
                required: true,
                FourDecimal: true,
                max: $("#ITM_CUR_STK").text(),
                messages: { required: "Translate(EnterQty)" }
            });
        else
            $("[id$=DSD_QUANTITY]").rules("add", {
                required: true,
                FourDecimal: true,
                messages: { required: "Translate(EnterQty)" }
            });
    }
    else if (mode == 3) {
        //        $("[id$=DTH_QTY_PLANNED]").rules("add", {
        //            required: true,
        //            NonZero: true,
        //            messages: { required: "Translate(EnterQty)" }
        //        });
        $("[id$=DTH_QUANTITY]").rules("add", {
            required: true,
            NonZero: true,
            messages: { required: "Translate(EnterQty)" }
        });
    }
    else {
        $("input[id$=DTH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(EnterPreparationDate)" }
        });
        //        $("[id$=DTH_QTY_PLANNED]").rules("add", {
        //            FourDecimal: true,
        //            required: true,
        //            messages: { required: "Translate(EnterQty)" }
        //        });
        $("[id$=DTH_QUANTITY]").rules("add", {
            FourDecimal: true,
            required: true,
            messages: { required: "Translate(EnterQty)" }
        });

        $("[id$=DISPESION]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectBOM)" }
        });
        //        $("select[id$=DTH_MACHINE]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: "Translate(SelectMachine)" }
        //        });
        //        $("[id$=DTH_LOAD_DT]").rules("add", {
        //            required: true,
        //            date: true,
        //            messages: { required: "Translate(EnterLoadDate)" }
        //        });
        //        $("[id$=DTH_LOAD_TM]").rules("add", {
        //            required: true,
        //            //dateTime: true,
        //            messages: { required: "Translate(EnterLoadTime)" }
        //        });
        //        $("[id$=DTH_UNLOAD_DT]").rules("add", {
        //            required: true,
        //            date: true,
        //            messages: { required: "Translate(EnterUnLoadDate)" }
        //        });
        //        $("[id$=DTH_UNLOAD_TM]").rules("add", {
        //            required: true,
        //            //dateTime: true,
        //            messages: { required: "Translate(EnterUnLoadTime)" }
        //        });
        //        $("[id$=DTH_MILL_HRS]").rules("add", {
        //            required: true,
        //            //exceptZero: true,
        //            messages: { required: "Translate(UnLoadTimeValid)" }
        //        });
    }
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
    $('input[id$=DSD_ITEM]').rules("remove");
    $('input[id$=MaterialCatagory]').rules("remove");
    $('[id$=DSD_QUANTITY]').rules("remove");  
    //$('[id$=BatchNo]').rules("remove");
    $('[id$=DSD_QUANTITY]').rules("remove");
    //$('[id$=DSD_QTY_UOM]').rules("remove");

    $('[id$=DTH_DATE]').rules("remove");
    //$('[id$=DTH_QTY_PLANNED]').rules("remove");
    $('[id$=DTH_QUANTITY]').rules("remove");
    $('[id$=DISPESION]').rules("remove");
    //$('[id$=DTH_MACHINE]').rules("remove");
    //    $('[id$=DTH_LOAD_DT]').rules("remove");
    //    $('[id$=DTH_LOAD_TM]').rules("remove");
    //    $('[id$=DTH_UNLOAD_DT]').rules("remove");
    //    $('[id$=DTH_UNLOAD_TM]').rules("remove");
    //    $('[id$=DTH_MILL_HRS]').rules("remove");
}


function AddBatchPopupValidations() {
    RemoveValidations();
    RemovePopupValidations();
    $("[id$=txtLatexQtyPopUp]").rules("add", {
        required: true,
        NonZero: true,
        messages: { required: "Translate(EnterQty)" }
    });
    $("[id$=ddlLatexBatchesPopUp]").rules("add", {
        selectNone: true,
        messages: { selectNone: "Translate(SelectBatchNo)" }
    });
}

function RemovePopupValidations() {
    $("[id$=txtLatexQtyPopUp]").rules("remove");
    $("[id$=ddlLatexBatchesPopUp]").rules("remove");
}
//#endregion

function DateInit() {
    ////<summary>function used to fill dates </summary>

    //    $("[id$=DTH_LOAD_TM]").timepicker({
    //        onSelect: function (dateText, inst) {
    //            AfterDateSelect();
    //        }
    //    });
    //    $("[id$=DTH_UNLOAD_TM]").timepicker({
    //        onSelect: function (dateText, inst) {
    //            AfterDateSelect();
    //        }
    //    });
    //GrandScriptUtils.AddDateRangeAdvance("DTH_LOAD_DT", "hdnFromDate", "DTH_UNLOAD_DT", "hdnToDate", false, false, true);
    //GrandScriptUtils.DatePicker("DTH_DATE", false, false, true, "DTH_LOAD_DT", "DTH_UNLOAD_DT", "hdnDate");//change to as per beta req
    GrandScriptUtils.DatePickerAdvance("DTH_DATE", false, false, true, false, false, false);
    //GetCurrentDateTime();
}

//function AfterDateSelect(ControlID) {
//    ///<summary>To handle bind grid </summary>
//    if (ControlID == "DTH_DATE") {
//        var dt = $("[id$=DTH_DATE]").val();
//        $("[id$=DTH_LOAD_DT]").datepicker('setDate', dt);

//        // This line is used to fire the onSlect event of last binded datepicker control
//        // Here ".ui-datepicker-current-day" this code execute the "DTH_LOAD_DT" control's onSelect callBack
//        $('.ui-datepicker-current-day').click();

//        $("[id$=DTH_UNLOAD_DT]").datepicker('setDate', dt);
//    }

//    var unloadTm = $("[id$=DTH_UNLOAD_DT]").val() + " " + $("[id$=DTH_UNLOAD_TM]").val();
//    var loadTm = "";
//    if ($("[id$=DTH_LOAD_DT]").val() != "" && $("[id$=DTH_LOAD_TM]").val() != "") {
//        loadTm = $("[id$=DTH_LOAD_DT]").val() + " " + $("[id$=DTH_LOAD_TM]").val();
//    }
//    else {
//        loadTm = "";
//    }
//    var loadTime = new Date();
//    var unloadTime = new Date();
//    var diff = 0;
//    if (unloadTm != "" && loadTm != "") {
//        loadTm = loadTm.replace("-", "/");
//        loadTm = loadTm.replace("-", "/");
//        unloadTm = unloadTm.replace("-", "/");
//        unloadTm = unloadTm.replace("-", "/");
//        loadTime = new Date(CalculateDate(loadTm));
//        unloadTime = new Date(CalculateDate(unloadTm));
//        var sec = unloadTime.getTime() - loadTime.getTime();
//        var second = 1000, minute = 60 * second, hour = 60 * minute, day = 24 * hour;
//        var days = Math.floor(sec / day);
//        sec -= days * day;
//        var hours = Math.floor(sec / hour);
//        sec -= hours * hour;
//        var minutes = Math.floor(sec / minute);
//        sec -= minutes * minute;
//        var seconds = Math.floor(sec / second);
//        var totalHrs = ((24 * days) + hours) + "." + minutes;
//        if (parseFloat(totalHrs) >= 0) {
//            $("[id$=DTH_MILL_HRS]").val(totalHrs);
//        }
//        else {
//            $("[id$=DTH_MILL_HRS]").val("");
//        }
//    }
//}

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

function GetCurrentDateTime() {
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul",
        "Aug", "Sep", "Oct",
        "Nov", "Dec"
    ];

    var date = new Date();
    var day = ("0" + date.getDate()).slice(-2);
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var currentDate = day + '-' + monthNames[monthIndex] + '-' + year;
    //    $("[id$=DTH_UNLOAD_DT]").val(currentDate);
    //    $("[id$=DTH_UNLOAD_TM]").val('00:00');
    //    $("[id$=DTH_LOAD_TM]").val('00:00');
    //    $("[id$=DTH_MILL_HRS]").val('0.0');
}

function FillPlan(planID, planText, status) {
    ////<summary>function used to fill plan </summary>

    //  GrandScriptUtils.MakeAutoComplete("PLANNAME", DispersionPreparation.GetPlanURL, "DTH_PLAN", true, false, false, true, false, false, "ddlSelectDiv3");
    if (planID != 0) {
        $("[id$=DTH_PLAN]").val(planID);
        $("[id$=PLANNAME]").val(planText);
    }
    $("[id$=DTH_PLAN]").val("0");
    if (DispersionPreparation.IsViewMode) {
        $("[id$=PLANNAME]").autocomplete("option", "disabled", true);
    }
    else if (status == 1 || status == 7) {
        $("[id$=PLANNAME]").autocomplete("option", "disabled", true);
    }
}

function FillDispersion(dispersionID, dispersionText, status) {
    ////<summary>function used to fill plan </summary>    , false, false, "ddlSelectDiv3"
    GrandScriptUtils.MakeAutoCompleteAdvance("DISPESION", DispersionPreparation.GetDispersionURL+$("[id$=hdfDeptID]").val(), "DTH_DISPERSION", true, false, "", true, false, false, "ddlSelectDiv3");
    if (dispersionID != 0) {
        $("[id$=DTH_DISPERSION]").val(dispersionID);
        $("[id$=DISPESION]").val(dispersionText);
    }
    if (DispersionPreparation.IsViewMode) {
        $("[id$=DISPESION]").autocomplete("option", "disabled", true);
    }
    else if (status == 1 || status == 7) {
        $("[id$=DISPESION]").autocomplete("option", "disabled", true);
    }
}

function FillQuantityUOM(uomPK) {
    //<summary>fills UOMs on the  UOMQuantity in the grid</summary>

    //var drpID = $("select[id$=DTH_QTY_PLANNED_UOM]").attr("id");
    var drpID2 = $("select[id$=DTH_QTY_UOM]").attr("id");
    $.get(DispersionPreparation.GetUOMURL, function (data) {
        //GrandScriptUtils.FillDropDown(drpID, data, true, true, uomPK);
        GrandScriptUtils.FillDropDown(drpID2, data, true, true, uomPK);
    });
}

//function FillMachine(machType, machID) {
//    ///<summary>function To Fill UOM Details </summary> 
//    var drpID = $("select[id$=DTH_MACHINE]").attr("id");
//    $.get(DispersionPreparation.GetMachineURL + machType + "&ProcessID=7", function (data) {
//        GrandScriptUtils.FillDropDown(drpID, data, true, true, machID);
//    });
//}

function FillDetails(dispersionObj) {
    //<summary> Function used to fill the details </summary>
    $("[id$=DTH_PK]").val(dispersionObj.DTH_PK);
    if(dispersionObj.DTH_BATCH_NO != null && dispersionObj.DTH_BATCH_NO != '')
        $("[id$=DTH_BATCH_NO]").html(dispersionObj.DTH_BATCH_NO);
    else
        $("[id$=DTH_BATCH_NO]").html('[NEW]');
    //$("[id$=DTH_QTY_PLANNED]").val(parseFloat(dispersionObj.DTH_QTY_PLANNED).toFixed(dispersionDecimal));
    $("[id$=DTH_QUANTITY]").val(parseFloat(dispersionObj.DTH_QUANTITY).toFixed(dispersionDecimal));
    $("[id$=DTH_PREPARED_BY]").val(dispersionObj.DTH_PREPARED_BY);
    $("[id$=DTH_PREP_COST]").val(dispersionObj.DTH_PREP_COST);
    $("[id$=DTH_DATE]").val(dispersionObj.DTH_DATE);
    $("[id$=lblBomCategory]").text(dispersionObj.DSP_ITEM_TYPE_TEXT);
    //    $("[id$=DTH_UNLOAD_DT]").val(dispersionObj.DTH_UNLOAD_DT);
    //    $("[id$=DTH_LOAD_DT]").val(dispersionObj.DTH_LOAD_DT);
    //    $("[id$=DTH_UNLOAD_TM]").val(dispersionObj.DTH_UNLOAD_TM);
    //    $("[id$=DTH_LOAD_TM]").val(dispersionObj.DTH_LOAD_TM);
    //    $("[id$=DTH_MILL_HRS]").val(dispersionObj.DTH_MILL_HRS);
    $("[id$=LAST_MOD_DT]").val(dispersionObj.LAST_MOD_DT);
    //$("[id$=lblDispType]").text(dispersionObj.DSP_TYPE_TEXT);
    $("[id$=DTH_REMARKS]").val(dispersionObj.DTH_REMARKS);
    // $("[id$=hdfDeptID]").val(dispersionObj.DTH_DEPT);
    //    if (dispersionObj.DTH_TEST_TOTAL != "0") {
    //        $("#lnkViewInspection").html(dispersionObj.DTH_TEST_PASSED + " out of " + dispersionObj.DTH_TEST_TOTAL + " passed");
    //    }
    if (DispersionPreparation.IsViewMode) {
        DisableFields();
    }
    else if (dispersionObj.DTH_STATUS == 1 || dispersionObj.DTH_STATUS == 7) {
        DisableFields();
    }
    //FillMachine(dispersionObj.DSP_MACHINE_TYPE, dispersionObj.DTH_MACHINE); //No machine type in master now
    //FillMachine(0, dispersionObj.DTH_MACHINE);
    $("#divMaterialInsert").css({ "display": "block", "visibility": "visible" });
}


function DisableFields() {
    //<summary> Function used to fill the details </summary>

    $("[id$=PLANNAME]").attr("disabled", "disabled");
    $("[id$=DISPESION]").attr("disabled", "disabled");
    //$("[id$=DTH_QTY_PLANNED]").attr("disabled", "disabled");
    $("[id$=DTH_QUANTITY]").attr("disabled", "disabled");
    //$("[id$=DTH_MACHINE]").attr("disabled", "disabled");
    $("[id$=DTH_DATE]").attr("disabled", "disabled");
    //    $("[id$=DTH_LOAD_DT]").attr("disabled", "disabled");
    //    $("[id$=DTH_LOAD_TM]").attr("disabled", "disabled");
    //    $("[id$=DTH_UNLOAD_DT]").attr("disabled", "disabled");
    //    $("[id$=DTH_UNLOAD_TM]").attr("disabled", "disabled");
    $("[id$=DTH_REMARKS]").attr("disabled", "disabled");
    $("[id$=DTH_PREPARED_BY]").attr("disabled", "disabled");
    $("[id$=DTH_PREP_COST]").attr("disabled", "disabled");
    $("[id$=btnCalculate]").hide();
    $("[id$=imbCalculate]").hide();
    $("[id$=imbDelete]").hide();
    $("[id$=imbAddNew]").hide()
}

function EnableFields() {
    //<summary> Function used to fill the details </summary>

    //$("[id$=DTH_QTY_PLANNED]").removeAttr("disabled");
    $("[id$=DTH_QUANTITY]").removeAttr("disabled");
    //$("[id$=DTH_MACHINE]").removeAttr("disabled");
    $("[id$=DTH_DATE]").removeAttr("disabled");
    //    $("[id$=DTH_LOAD_DT]").removeAttr("disabled");
    //    $("[id$=DTH_LOAD_TM]").removeAttr("disabled");
    //    $("[id$=DTH_UNLOAD_DT]").removeAttr("disabled");
    //    $("[id$=DTH_UNLOAD_TM]").removeAttr("disabled");
    $("[id$=DTH_REMARKS]").removeAttr("disabled");
    $("[id$=DTH_PREPARED_BY]").removeAttr("disabled");
    $("[id$=DTH_PREP_COST]").removeAttr("disabled");
}
//var dropDownChangeVal = 0;
function FillDispersionDetails(dispersionObj) {
    if (dispersionObj.Materials.length > 0) {
        for (var i in dispersionObj.Materials) {
            dispersionObj.Materials[i].DSD_PK = 0;
            //dispersionObj.Materials[i].IMG = "";
            //            if (dispersionObj.Materials[i].DSD_ITEM_TYPE == "2") {
            //                dispersionObj.Materials[i].DTD_BATCH = dispersionObj.Materials[i].DSD_STK_DISP_BATCH;
            //                dispersionObj.Materials[i].DTD_STK_BATCH = null;
            //            }
            //            else {
            //                dispersionObj.Materials[i].DTD_BATCH = null;
            //                dispersionObj.Materials[i].DSD_STK_DISP_BATCH = null;
            //            }
        }
    }
    else {
        dispersionObj.Materials.DSD_PK = 0;
    }
    if (!$.isArray(dispersionObj.Materials)) {
        var materialObj = dispersionObj.Materials;
        dispersionObj.Materials = new Array();
        dispersionObj.Materials.push(materialObj);
    }
    //    $("[id$=DTH_QTY_PLANNED_UOM]").val(dispersionObj.DSP_QTY_UOM);
    //    $("[id$=DTH_QTY_PLANNED]").val(parseFloat(dispersionObj.DSP_QUANTITY).toFixed(dispersionDecimal));

    $("[id$=DTH_QTY_UOM]").val(dispersionObj.DSP_QTY_UOM);
    $("[id$=DTH_QUANTITY]").val(parseFloat(dispersionObj.DSP_QUANTITY).toFixed(dispersionDecimal));
    $("[id$=lblBomCategory]").text(dispersionObj.DSP_ITEM_TYPE_TEXT);
    //$("[id$=DTH_MACHINE]").val(dispersionObj.DSP_MACHINE);
    //    if (dispersionObj.DSP_TYPE_TEXT == undefined || dispersionObj.DSP_TYPE_TEXT == "null") {
    //        $("[id$=lblDispType]").text("");
    //    }
    //    else {
    //        $("[id$=lblDispType]").text(dispersionObj.DSP_TYPE_TEXT);
    //    }

    //    if (Calcbtn == 1) {
    //        if (dispersionObj.DSP_MACHINE == undefined) {//No Material type in master now, only material name
    //            FillMachine(0);
    //        }
    //        else {
    //            FillMachine(0, dispersionObj.DSP_MACHINE);
    //        }
    //    }

    $("#divData").data("MaterialData", dispersionObj.Materials);
    //$("#divSaveData").data("SaveData", dispersionObj.Materials);

    for (var i in dispersionObj.Materials) {
        dispersionObj.Materials[i].SL_NO = i;
        dispersionObj.Materials[i].DTD_ITEM_TYPE = dispersionObj.Materials[i].DSD_ITEM_TYPE;
    }

    GrandGrid.Utilities.ResetGrid(true, "grdDispersionDetails");
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, dispersionObj.Materials);
    //dropDownChangeVal = 1;
    //AddQtyInputs();
    //dropDownChangeVal = 0;

}

/*function FillCheckListDetails(dispersionObj) {
if (!$.isArray(dispersionObj.CheckListDtl)) {
var materialObj = dispersionObj.CheckListDtl;
dispersionObj.CheckListDtl = new Array();
dispersionObj.CheckListDtl.push(materialObj);
}


$("#divData").data("CheckListDtl", dispersionObj.CheckListDtl);

for (var i in dispersionObj.Materials) {
dispersionObj.CheckListDtl[i].SL_NO = i;
}
GrandGrid.Utilities.ResetGrid(true, "grdChecklistDetails");
if (dispersionObj.CheckListDtl[0] != undefined) {
ShowCheckList();
GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, dispersionObj.CheckListDtl);
} else {
// dispersionObj.CheckListDtl.splice(0, 1);
GrandGrid.MakeGrid($("#grdChecklistDetails"), 0, new Array());
HideCheckList();
}
}

function addNewCheckListItem() {

DispersionPreparation.CheckListDtl = $("#divData").data("CheckListDtl");
var dispersionObj = DispersionPreparation;
var nextSlNo = 0;

if ($.isArray(dispersionObj.CheckListDtl)) {
if (dispersionObj.CheckListDtl.length > 0) {
if (dispersionObj.CheckListDtl[0] === undefined) {
dispersionObj.CheckListDtl.splice(0, 1);
}

nextSlNo = dispersionObj.CheckListDtl.length + 1;
}
} else {
dispersionObj.CheckListDt = new Array();
}

var CDL_ACTIVE = 1;
var CDL_NAME = $("[id$=txtCheckList]").val();
var CDL_PK = 0;
var CDL_SL_NO = nextSlNo;

var CDL_CHECK_LIST_DTL = 0; // New Entry

var CDT_VALUE_TYPE = $("select[id$=ddlValueType]").val();
var CDL_VALUE = CDT_VALUE_TYPE == "1" ? $("[id$=txtValue]").attr('checked') : $("[id$=txtValue]").val();
var CDT_VALUE_TYPE_TEXT = $("[id$=ddlValueType] option:selected").text();
var CDL_DESC = $("[id$=txtRemarks]").val();

var jsonStr = '{ "CDL_ACTIVE" : "' + CDL_ACTIVE + '" , "CDL_CHECK_LIST_DTL": "' + CDL_CHECK_LIST_DTL + '" , "CDL_NAME" : "' + CDL_NAME + '" , "CDL_PK" : "' + CDL_PK + '", "CDL_SL_NO" : "' + CDL_SL_NO + '", "CDL_VALUE" : "' + CDL_VALUE + '", "CDT_VALUE_TYPE" : "' + CDT_VALUE_TYPE + '", "CDT_VALUE_TYPE_TEXT" : "' + CDT_VALUE_TYPE_TEXT + '", "CDL_DESC" :"' + CDL_DESC + '" }';
var jsonObj = JSON.parse(jsonStr);

dispersionObj.CheckListDtl.push(jsonObj);

$("#divData").data("CheckListDtl", dispersionObj.CheckListDtl);
FillCheckListDetails(dispersionObj);

clearChkTmplInsertBlock();

return false;
}

function clearChkTmplInsertBlock() {
$("[id$=txtCheckList]").val("");
$("select[id$=ddlValueType]").val("0");
$("[id$=txtRemarks]").val("");
$('#rowValueTemplate').hide();
}*/

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>

    if (targetControlID == "DISPESION") {
        $("#divgrdDispersionDetailsGrid").show();
        $("#divMaterialInsert").show();
        Calcbtn = 1;
        BindMaterailGrid();
    }
   if (targetControlID == "MaterialCatagory") {
        //FillItem();
        FillMaterialAutoComplete();
    }
    if (targetControlID == "DSD_ITEM") {
        MaterialChangeEvent();
    }
}
/*
function BindCheckListGrid(chklstId) {
///<summary>To handle bind grid </summary>  
var ajaxUrl = DispersionPreparation.GetCheckListDetailsURL + chklstId;
$.getJSON(ajaxUrl, function (data) {
if (data) {
FillCheckListDetails(data);
}
});
}

function FetchChecklistGridData() {
DispersionPreparation.CheckListDtl = $("#divData").data("CheckListDtl");
$("#grdChecklistDetails tr:has(td)").each(function (index) {
//        if (index > 0) {
var chklstItem = DispersionPreparation.CheckListDtl[index];
if (chklstItem != undefined) {
if (chklstItem.CDT_VALUE_TYPE == "1") {
chklstItem.CDL_VALUE = $("#txtCDLValue_" + index.toString()).attr("checked");
}
else if ($("#txtCDLValue_" + index.toString()).val() != "NaN") {
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
if (DispersionPreparation.CheckListDtl != undefined) {
$("#divData").data("CheckListDtl", DispersionPreparation.CheckListDtl);
}

}
*/
function BindMaterailGrid(flag) {
    ///<summary>To handle bind grid </summary>   
    if ($("[id$=DTH_DISPERSION]").val() == "0") {
        GrandScriptUtils.ShowModal("Translate(SelectBOM)", DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
        return false;
    }
    RemoveValidations();
    if (flag != undefined) {
        AddValidations(3);
    }
    if ($(document.forms[0]).valid()) {
        var quantity = 0.0;
        if (flag) {
            $("#divData").data("MaterialData", new Array());
            GrandGrid.Utilities.ResetGrid(true, "grdDispersionDetails");
            GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, new Array());
            quantity = parseFloat($("[id$=DTH_QUANTITY]").val());
        }
        var ajaxUrl = DispersionPreparation.GetDispersionDetailsURL + $("[id$=DTH_DISPERSION]").val() + "&DepartmentID=" + deptID;
        $.getJSON(ajaxUrl, function (data) {
            if (data) {
                FillDispersionDetails(data);

                //BindCheckList Start
                if (flag) {
                    calcFlag = 1;
                    CalcPercQuantity(quantity);
                }
                //                if (Calcbtn == 1) {
                //                    $("[id$=hdfDSPCHECKLISTHDR]").val("");
                //                    if (data["DSP_CHECK_LIST_HDR"] != "" && data["DSP_CHECK_LIST_HDR"] != undefined) {
                //                        $("[id$=hdfDSPCHECKLISTHDR]").val(data["DSP_CHECK_LIST_HDR"]);
                //                        $("#divgrdChecklist").show();
                //                        BindCheckListGrid(data["DSP_CHECK_LIST_HDR"]);
                //                    }
                //                    else {
                //                        $("#divgrdChecklist").hide();
                //                    }
                //                }
                //                //End
            }
            GetGridData();
            highliteStock();
            Calcbtn = 0;
        });
    }
}
function ShowWkfSubmitPopUp() {
    //    SelectBatch();
    //    if (!checkMultiBatchQty()) {
    //        ShowErrorMessage("<ul><li><span style='color:red'>Total batch quantity for each item should be same as Required quantity for the item</span></li></ul>");
    //    }
    //    else {
    //        if (!SelectBatch(true)) {
    //            GrandScriptUtils.ShowModal(DispersionPreparation.SelectBatchNo, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
    //            return false;
    //        }
    //        if (!CheckQtyZero(true)) {
    //            GrandScriptUtils.ShowModal(DispersionPreparation.EnterQuantity, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
    //            return false;
    //        }
    if (!CheckQtyZero(true)) {
        GrandScriptUtils.ShowModal(DispersionPreparation.EnterQuantity, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
        return false;
    }
    //CaptureChanges();
    //$("select[id$=DTH_QTY_PLANNED_UOM]").attr("disabled", false);
    $("select[id$=DTH_QTY_UOM]").attr("disabled", false);
    //$("[id$=DTH_MILL_HRS]").attr("disabled", false);
    RemoveValidations();
    AddValidations();
    if ($(document.forms[0]).valid()) {
        ShowContainerDivWkf('#divWkfSubmit', DispersionPreparation.MessageBoxTitle, '700');
    }
    //$("select[id$=DTH_QTY_PLANNED_UOM]").attr("disabled", true);
    $("select[id$=DTH_QTY_UOM]").attr("disabled", true);
    //$("[id$=DTH_MILL_HRS]").attr("disabled", true);

    return false;
}
function SavePage(command) {
    //    <summary> Function Used to save page </summary>
    //        if (!checkMultiBatchQty()) {
    //            ShowErrorMessage("<ul><li><span style='color:red'>Total batch quantity for each item should be same as Required quantity for the item</span></li></ul>");
    //        }
    //        else {
    //            SelectBatch();
    //            var rowscount = $("#grdDispersionDetails tbody tr").length;
    //            if (rowscount < 2) {
    //                GrandScriptUtils.ShowModal(DispersionPreparation.EnterMaterialDetails, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
    //                return false;
    //            }
    //            else {

    //                if (!SelectBatch(true)) {
    //                    GrandScriptUtils.ShowModal(DispersionPreparation.SelectBatchNo, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
    //                    return false;
    //                }

    //                if (!CheckQtyZero(true)) {
    //                    GrandScriptUtils.ShowModal(DispersionPreparation.EnterQuantity, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
    //                    return false;
    //                }
    //            }
    //CaptureChanges();
    var rowscount = $("#grdDispersionDetails tbody tr").length;
    if (rowscount < 2) {
        GrandScriptUtils.ShowModal(DispersionPreparation.EnterMaterialDetails, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
        return false;
    }
    else {
        if (!CheckQtyZero(true)) {
            GrandScriptUtils.ShowModal(DispersionPreparation.EnterQuantity, DispersionPreparation.MessageBoxTitle, DispersionPreparation.Failed);
            return false;
        }
    }

    DispersionPreparation.MaterialList = new Array();
    DispersionPreparation.MaterialList = AddToMatarialList();
    //$("select[id$=DTH_QTY_PLANNED_UOM]").attr("disabled", false);
    $("select[id$=DTH_QTY_UOM]").attr("disabled", false);
    //$("[id$=DTH_MILL_HRS]").attr("disabled", false);
    RemoveValidations();
    AddValidations();

    //        FetchChecklistGridData();

    $("[id$=MaterialList]").val(JSON.stringify(DispersionPreparation.MaterialList));
    //        if (DispersionPreparation.CheckListDtl != undefined) {
    //            $("[id$=CheckListDtl]").val(JSON.stringify(DispersionPreparation.CheckListDtl));
    //        }

    if (command != "Draft") {
        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
        if (!CheckStockExcists()) {
            GrandScriptUtils.ShowModal(DispersionPreparation.NotDispersionStockExists, DispersionPreparation.MessageBoxTitle);
            return false;
        }
    }
    else {
        $("[id$=ActionID]").val('0');  // save only.
    }
    EnableFields();
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    if ($(document.forms[0]).valid()) {

        //To Prevent Duplicate Submission
        if ($("[id$=SubmitFlag]").val() == "0")
            $("[id$=SubmitFlag]").val('1')
        else
            return false;

        $.post(DispersionPreparation.SaveDispersionDetailsURL, jSonString, function (data) {
            if (parseInt(data[0]) > 0) {
                if (command == "Draft") {
                    var msg = DispersionPreparation.SaveMessage1 + " " + data[1] + " " + DispersionPreparation.SaveMessage2;
                    GrandScriptUtils.ShowModal(msg, DispersionPreparation.MessageBoxTitle, DispersionPreparation.SAVE);
                }
                else {
                    $("[id$=AppNo]").val(data[1]);
                    $("[id$=hdfAppID]").val(data[0]);
                    SaveWorkFlow();
                }
            }
            else if (parseInt(data[0]) == -1) {
                GrandScriptUtils.ShowModal(DispersionPreparation.ActionFailedMessage, DispersionPreparation.MessageBoxTitle);
            }
            else if (parseInt(data[0]) == -2) {
                GrandScriptUtils.ShowModal(DispersionPreparation.SaveMessage1 + " " + data[1] + " " + DispersionPreparation.EditUsedByAnotherUser, DispersionPreparation.MessageBoxTitle, DispersionPreparation.SAVE);
            }
            else if (parseInt(data[0]) == -3) {
                GrandScriptUtils.ShowModal(DispersionPreparation.NotDispersionStockExists, DispersionPreparation.MessageBoxTitle);
            }
        });
    }
    //$("select[id$=DTH_QTY_PLANNED_UOM]").attr("disabled", true);
    $("select[id$=DTH_QTY_UOM]").attr("disabled", true);
    //$("[id$=DTH_MILL_HRS]").attr("disabled", true);
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>

    var msg = DispersionPreparation.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + DispersionPreparation.SaveMessage2;
    GrandScriptUtils.ShowModal(msg, DispersionPreparation.MessageBoxTitle, DispersionPreparation.SAVE);
}

function CheckStockExcists() {
    ///<summary>Function used to check whether each item have enough stock to prepare dispersion</summary>
    var stockExcists = true;
    var materialList = $("#divData").data("MaterialData");
    var quantity = 0;
    var colIndex = 0;
    for (var i in materialList) {
        $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
         if (index == (parseInt(i) + 1)) {
            quantity = $("#txtPreparationQuantity_" + (index)).val();
             if (parseFloat(quantity) > parseFloat(materialList[i].ITM_CUR_STK)) {
                stockExcists = false;
               
                    if (stockExcists == false) {
                        var selectedRowColor;
                        selectedRowColor = '#F9DEE5';
                        $(this).closest('tr').addClass('highlight');
                    }
            }
        }
        });
        /* if (materialList[i].DTD_IS_MULT_BATCH == "0") {
        $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
        var ItmgroupIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
        var Itmgroup = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
        if (Itmgroup == materialList[i].DTD_MULT_BTCH_GRP) {
        quantity = $("#txtPreparationQuantity_" + (index)).val();
        if (parseFloat(quantity) > parseFloat(materialList[i].ITM_CUR_STK)) {
        stockExcists = false;

        if (index == (parseInt(i) + 1)) {
        if (stockExcists == false) {
        var selectedRowColor;
        selectedRowColor = '#F9DEE5';
        $(this).closest('tr').addClass('highlight');
        }
        }
        }
        }
        });

        //stockExcists = false;
        }*/
    }
    return stockExcists;
}
function CheckQtyZero() {
    ///<summary>Function used to check whether each item have enough stock to prepare dispersion</summary>

    var QtyZero = true;
    var materialList = $("#divData").data("MaterialData");
    var quantity = 0;
    for (var i in materialList) {
        $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
            quantity = $("#txtPreparationQuantity_" + (index)).val();
            $("#txtPreparationQuantity_" + (index)).removeAttr("style");
            if (parseFloat(quantity) == 0) {
                $("#txtPreparationQuantity_" + (index)).css("border", "1px solid red");
                QtyZero = false;
            }
        });
        /* if (materialList[i].DTD_IS_MULT_BATCH == "0") {
        $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
        var ItmgroupIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
        var Itmgroup = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
        if (Itmgroup == materialList[i].DTD_MULT_BTCH_GRP) {
        quantity = $("#txtPreparationQuantity_" + (index)).val();
        $("#txtPreparationQuantity_" + (index)).removeAttr("style");
        if (parseFloat(quantity) == 0) {
        $("#txtPreparationQuantity_" + (index)).css("border", "1px solid red");
        QtyZero = false;
        }
        }
        });
        }*/
    }
    return QtyZero;
}


function AddToMatarialList() {
    ///<summary>Function used to add the needed po material list</summary>

    var grdID;
    var stockQty = 0;
    var slNo = 1;
    //var materialList = $("#divData").data("MaterialData");
    var materialList = $("#divData").data("MaterialData");
    if (materialList[0] != undefined) {
        for (var i in materialList) {
            materialList[i].DSD_QUANTITY = $("#txtPreparationQuantity_" + (slNo).toString()).val();
            slNo++;
        }
    }
    /*
    $("#grdDispersionDetails tr:has(td)").each(function (index) {
    var colValue = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DTD_IS_MULT_BATCH, "grdDispersionDetails");
    var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DTD_IS_MULT_BATCH, "grdDispersionDetails");
    var GroupValue = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");

    for (var i in materialList) {
    if (colValue == "0") {
    if (materialList[i].DTD_MULT_BTCH_GRP == GroupValue) {
    materialList[i].DSD_QUANTITY = $("#txtPreparationQuantity_" + (index).toString()).val();
    }
    }
    }
    });*/
    return materialList;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>

    switch (command) {
        case DispersionPreparation.SAVE:
            window.location = DispersionPreparation.DISPERSIONPREPARATIONLISTURL;
            break;
        case "DeleteDetail":
            DeleteDetails();
            break;
        case DispersionPreparation.CANCELDISP:
            CancelDispersionDetails();
            break;
    }
    return false;
}

var slNO = 0;
function DeleteDetails() {
var newSL_NO=0;
    GetGridData();
    //DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");
    DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
    var TempSaveList = $("#divData").data("MaterialData");

    //    //Removing row with same group-- JSlinq for selecting items which are not belong to the multiple batch group
    //    DispersionPreparation.DispersionSaveList = (JSLINQ(DispersionPreparation.DispersionSaveList).
    //                                                Where(function (item) { return item.DTD_MULT_BTCH_GRP != GridItemGroup })).items;
    DispersionPreparation.DispertionList = new Array();
    for (var i in TempSaveList) {
        if (TempSaveList[i].SL_NO != slNO) {
            var TempBatchDtls = new Object();
            TempBatchDtls.SL_NO = newSL_NO;//TempSaveList[i].SL_NO;
            TempBatchDtls.SLNO = TempSaveList[i].SLNO;
            TempBatchDtls.DSD_PK = TempSaveList[i].DSD_PK;
            TempBatchDtls.DSD_ITEM = TempSaveList[i].DSD_ITEM;
            TempBatchDtls.DSD_ITEM_TYPE = TempSaveList[i].DSD_ITEM_TYPE;
            TempBatchDtls.DTD_ITEM_TYPE = TempSaveList[i].DTD_ITEM_TYPE;
            TempBatchDtls.ITM_TEXT = TempSaveList[i].ITM_TEXT;
            TempBatchDtls.BATCH_NO_TEXT = TempSaveList[i].BATCH_NO_TEXT;
            TempBatchDtls.DTD_MULT_BTCH_GRP = TempSaveList[i].DTD_MULT_BTCH_GRP;
            TempBatchDtls.DTD_IS_MULT_BATCH = TempSaveList[i].DTD_IS_MULT_BATCH;
            TempBatchDtls.ITM_CUR_STK = parseFloat(TempSaveList[i].ITM_CUR_STK).toFixed(dispersionDecimal);
            TempBatchDtls.DSD_QUANTITY = parseFloat(TempSaveList[i].DSD_QUANTITY).toFixed(dispersionDecimal);
            TempBatchDtls.CONVERT_FACTOR = TempSaveList[i].CONVERT_FACTOR;
            TempBatchDtls.DSD_DISP = TempSaveList[i].DSD_DISP;
            TempBatchDtls.DSD_QTY_PERC = TempSaveList[i].DSD_QTY_PERC;
            TempBatchDtls.DSD_QTY_UOM = TempSaveList[i].DSD_QTY_UOM;
            TempBatchDtls.DSD_ITEM_TYPE_TEXT = TempSaveList[i].DSD_ITEM_TYPE_TEXT;
            TempBatchDtls.QTY_IN_STOCK = TempSaveList[i].QTY_IN_STOCK;
            TempBatchDtls.UOM_CODE = TempSaveList[i].UOM_CODE;
            TempBatchDtls.UOM_NAME = TempSaveList[i].UOM_NAME;
            //TempBatchDtls.DTD_ACTUAL_TSC = TempSaveList[i].DTD_ACTUAL_TSC;
            //            if (TempSaveList[i].DSD_ITEM_TYPE == 1) {
            //                TempBatchDtls.DSD_STK_BATCH = TempSaveList[i].DSD_STK_BATCH;
            //                TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_BATCH;
            //                TempBatchDtls.DTD_BATCH = null;
            //                TempBatchDtls.DSD_STK_DISP_BATCH = null;
            //            }
            //            else {
            //                TempBatchDtls.DSD_STK_DISP_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
            //                TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_DISP_BATCH;
            //                TempBatchDtls.DTD_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
            //                TempBatchDtls.DSD_STK_BATCH = null;
            //            }
            if (TempSaveList[i].DSD_QUANTITY_TEMP != undefined && TempSaveList[i].DSD_QUANTITY_TEMP != "null" && TempSaveList[i].DSD_QUANTITY_TEMP != "" && TempSaveList[i].DTD_IS_MULT_BATCH == "1") {
                TempBatchDtls.DSD_QUANTITY_TEMP = TempSaveList[i].DSD_QUANTITY_TEMP;
            }
            //TempBatchDtls.IMG = "";
            TempBatchDtls.ITM_PHR = TempSaveList[i].ITM_PHR;
            DispersionPreparation.DispertionList.push(TempBatchDtls);
            newSL_NO++;
        }
    }
    $("#divData").data("MaterialData", DispersionPreparation.DispertionList);



    //GetDisplayGridData();
    //DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
    $("#divData").data("MaterialData", DispersionPreparation.DispertionList);
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
    if (DispersionPreparation.DispertionList.length == 0) { //Used to Show the Material  details when the Materials in Dispersion is 0
        $(tdset).insertAfter($("#MaterialInsert").find("tr:eq(0)"));
        $("#MaterialInsert").show();
        $("#MaterialInsert").css({ "display": "block", "visibility": "visible" });

    }
    //    else {
    //        AddQtyInputs();
    //    }
    calcFlag = 1;
    //CalcTotalQty();
    ActualQty = parseFloat($("[id$=DTH_QUANTITY]").val());   
    CalcPercQuantity(ActualQty);
    highliteStock();
    
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0);
    FillMaterialCategoryAutoComplete();
    FillMaterialAutoComplete();
}


function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>

    if (grdID == "grdDispersionDetails") {
        var qtyIndex = 0;
        var quantity = "";
        var TempQuantity = 0.0;
        var txtQuantity;
        var itemPK = 0;
        var colIndex1 = 0;
        var dispersionObj = $("#divData").data("dispersionObj");
        var isViewMode = false;
        if (DispersionPreparation.IsViewMode) {
            isViewMode = true;
        }
        else if (dispersionObj != undefined) {
            if (dispersionObj.DTH_STATUS == 1 || dispersionObj.DTH_STATUS == 7) {
                isViewMode = true;
            }
        }
        // ==============================
        if (tdset == "") {
            tdset = $("#MaterialInsert").find("tr:eq(1)");
        }

        //        $("#MaterialInsert").hide();
        $("#MaterialInsert").css({ "display": "none", "visibility": "hidden" });
        $("#grdDispersionDetails").show();
        $(tdset).insertBefore($("#grdDispersionDetails").find("tr:eq(1)"));
        // ====================================

        $("#grdDispersionDetails").find("tr:has(th)").each(function (index) {
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_CUR_STK", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right ');
            }
            colIndex1 = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
            if (colIndex1 != null) {
                $(this).find("th:eq(" + colIndex1 + ")").attr('style', 'text-align: right');
            }
            $(this).closest('tr').addClass('row');
        });


        var rowIndex = -1;
        $("#grdDispersionDetails tr:has(td)").each(function () {
            rowIndex++;
            itemPK = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DSD_ITEM, grdID);
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DSD_QUANTITY, grdID);
            TempQuantity = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DSD_QUANTITY, grdID);
            quantity = parseFloat(TempQuantity).toFixed(dispersionDecimal);
            if (qtyIndex != null) {
                $(this).find("td:eq(" + qtyIndex + ")").html("");
                if (isViewMode) {
                    $(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"text\" value=\"" + quantity + "\" id=\"txtPreparationQuantity_" + rowIndex.toString() + "\" class=\"numeric input-w75 \" disabled=\"disabled\" />");
                }
                else {
                    //$(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"text\" value=\"" + quantity + "\" id=\"txtPreparationQuantity_" + rowIndex.toString() + "\" tabIndex=\"12\"" + "\" class=\"numeric input-w75\" onchange=\"javascript:MakeNumeric(this);\"/>");
                    $(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"text\" value=\"" + quantity + "\" id=\"txtPreparationQuantity_" + rowIndex.toString() + "\" tabIndex=\"12\"" + "\" class=\"numeric input-w75 bg-white \" onchange=\"javascript:MakeNumeric(this);\"/>");
                }
                $(this).find("td:eq(" + qtyIndex + ")").attr('style', 'text-align : right');
            }
            /*
            //Quantity Percentage
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_PERC", grdID);
            quantity = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QTY_PERC", grdID);            
            if (!isNaN(quantity)) {
                quantity = parseFloat(quantity).toFixed(dispersionDecimal);
            }
            $(this).find("td:eq(" + qtyIndex + ")").attr('style', 'text-align : right');
            if (quantity == "undefined" || quantity == "null" || quantity == "") {
                $(this).find("td:eq(" + qtyIndex + ")").html("");
            }
            else {
                $(this).find("td:eq(" + qtyIndex + ")").html(quantity);
            }*/

            //Current Stock
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_CUR_STK", grdID);
            quantity = GrandGrid.Utilities.GetColumnValue($(this), "ITM_CUR_STK", grdID);
            //            var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_IS_MULT_BATCH", grdID);
            //            var colValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_IS_MULT_BATCH", grdID);
            if (qtyIndex != null) {
                if (!isNaN(quantity)) {
                    quantity = parseFloat(quantity).toFixed(dispersionDecimal);
                }
                $(this).find("td:eq(" + qtyIndex + ")").attr('style', 'text-align : right');
                if (quantity == "undefined" || quantity == "null" || quantity == "") {
                    $(this).find("td:eq(" + qtyIndex + ")").html("0");
                }
                else {
                    $(this).find("td:eq(" + qtyIndex + ")").html(addCommasForNumeric(quantity));
                }
            }
            else {
                $(this).find("td:eq(" + qtyIndex + ")").html("0");
            }

            /*//  DTD_ACTUAL_TSC
            ActualTSCIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_ACTUAL_TSC", grdID);
            ActualTSC = GrandGrid.Utilities.GetColumnValue($(this), "DTD_ACTUAL_TSC", grdID);
            if (!isNaN(ActualTSC) && ActualTSC != '') {
            ActualTSC = parseFloat(ActualTSC).toFixed(dispersionDecimal);
            }
            if (ActualTSCIndex != null) {
            $(this).find("td:eq(" + ActualTSCIndex + ")").html("");
            if (isViewMode) {
            if (ActualTSC == "undefined" || ActualTSC == "null" || ActualTSC == "")
            $(this).find("td:eq(" + ActualTSCIndex + ")").append("<input type=\"text\" value=\"" + "" + "\" id=\"txtActualTSC_" + rowIndex.toString() + "\" class=\"numeric input-w75\" disabled=\"disabled\" />");
            else
            $(this).find("td:eq(" + ActualTSCIndex + ")").append("<input type=\"text\" value=\"" + ActualTSC + "\" id=\"txtActualTSC_" + rowIndex.toString() + "\" class=\"numeric input-w75\" disabled=\"disabled\" />");
            }
            else {
            if (ActualTSC == "undefined" || ActualTSC == "null" || ActualTSC == "")
            $(this).find("td:eq(" + ActualTSCIndex + ")").append("<input type=\"text\" value=\"" + "" + "\" id=\"txtActualTSC_" + rowIndex.toString() + "\" tabIndex=\"12\"" + "\" class=\"numeric input-w75\" />");
            else
            $(this).find("td:eq(" + ActualTSCIndex + ")").append("<input type=\"text\" value=\"" + ActualTSC + "\" id=\"txtActualTSC_" + rowIndex.toString() + "\" tabIndex=\"12\"" + "\" class=\"numeric input-w75\" />");
            }
            $(this).find("td:eq(" + ActualTSCIndex + ")").attr('style', 'text-align : right');
            }
            */
            var QtyPerc = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QTY_PERC", grdID);
            if (parseFloat(QtyPerc) == 0) {
                $(this).find("td:last input[id$=imbCalculate]").hide();
            }
            /*
            //Multiple Batch Add -- 'IMG'-> Temperory field for Add column
            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "IMG", grdID);
            $(this).find("td:eq(" + qtyIndex + ")").html("")
            if (isViewMode) {
            $(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"image\" id=\"imgViewBatch_" + rowIndex.toString() + "\" style=\"margin-left:22px;\" src=\"../images/Classic/Icons/viewbtn-grid.png\" title=\"View Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'multiplebatch');\" />");
            }
            else {
            $(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"image\" id=\"imgAddBatch_" + rowIndex.toString() + "\" style=\"margin-left:22px;\" src=\"../images/Classic/Icons/addbtn-grid.png\" title=\"Add Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'multiplebatch');\" />");
            $(this).find("td:eq(" + qtyIndex + ")").append("<input type=\"image\" id=\"imgClearBatch_" + rowIndex.toString() + "\" style=\"margin-left:22px;\" src=\"../images/Classic/Icons/clearbtn-grid.png\" title=\"Clear Batches\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'clearmultiplebatch');\" />");
            }
            */
        });
    }
    /*
    //    if (grdID == "grdInspectionDetails") {
    //        var rawInspIndex = 0;
    //        var rawInspNo = "";
    //        var trxPK = "";
    //        $("#grdInspectionDetails tr:has(td)").each(function () {
    //            rawInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TIH_NO, grdID);
    //            rawInspNo = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TIH_NO, grdID);
    //            trxPK = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TIH_PK, grdID);
    //            if (rawInspIndex != null) {
    //                $(this).find("td:eq(" + rawInspIndex + ")").html("<a style=\"cursor:pointer\"  onclick=\"javascript:ViewRawMaterialDetails(" + trxPK + ");\" >" + rawInspNo + "</a>");
    //            }
    //        });
    //    }

    //    if (grdID == "grdRawMaterialInspection") {
    //        CalculateVariance(grdID);
    //        var rawInspIndex = 0;
    //        var rawInspValue = "";
    //        var ObsrvdValue = "";
    //        $("#grdRawMaterialInspection tr:has(td)").each(function () {
    //            rawInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TID_MIN_MAX_VALUE, grdID);
    //            rawInspValue = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TID_MIN_MAX_VALUE, grdID);
    //            if (rawInspIndex != null) {
    //                if (rawInspValue == '< >') {
    //                    $(this).find("td:eq(" + rawInspIndex + ")").html("");
    //                }
    //                else {
    //                    $(this).find("td:eq(" + rawInspIndex + ")").html(rawInspValue);
    //                }
    //            }
    //            rawInspIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TID_VALUE, grdID);
    //            ObsrvdValue = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TID_VALUE, grdID);
    //            if (rawInspIndex != null) {
    //                if (ObsrvdValue != '' || ObsrvdValue != 'undefined' || ObsrvdValue != null) {
    //                    if (isNaN(ObsrvdValue))
    //                        $(this).find("td:eq(" + rawInspIndex + ")").html(ObsrvdValue);
    //                    else
    //                        $(this).find("td:eq(" + rawInspIndex + ")").html(parseFloat(ObsrvdValue));
    //                }
    //                else {
    //                    $(this).find("td:eq(" + rawInspIndex + ")").html("");
    //                }
    //            }
    //        });
    //    }

    //    //CheckList
    //    if (grdID == "grdChecklistDetails") {

    //        var isViewMode = false;
    //        if (DispersionPreparation.IsViewMode) {
    //            isViewMode = true;
    //        }
    //        else if (dispersionObj != undefined) {
    //            if (dispersionObj.DTH_STATUS == 1 || dispersionObj.DTH_STATUS == 7) {
    //                isViewMode = true;
    //            }
    //        }
    //        var CDLValueIndex = 0;
    //        var CDLValue = "";
    //        var CDT_VALUE_TYPE = "";
    //        var rowIndex = -1;
    //        $("#grdChecklistDetails tr:has(td)").each(function (index) {
    //            var CDL_NAME = GrandGrid.Utilities.GetColumnValue($(this), "CDL_NAME", grdID);
    //            if (CDL_NAME != undefined) {
    //                itemPK = GrandGrid.Utilities.GetColumnValue($(this), "CDL_PK", grdID);
    //                colIndexCDLValue = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_VALUE", grdID);
    //                CDLValue = GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "CDL_VALUE", grdID);
    //                CDT_VALUE_TYPE = GrandGrid.Utilities.GetColumnValue($(this), "CDT_VALUE_TYPE", grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "CDT_VALUE_TYPE", grdID);

    //                colIndexCDTDESC = GrandGrid.Utilities.GetColumnIndex($(this), "CDL_DESC", grdID);
    //                CDL_DESC = GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "CDL_DESC", grdID);

    //                if (colIndexCDLValue != null) {

    //                    $(this).find("td:eq(" + colIndexCDLValue + ")").html("");
    //                    switch (CDT_VALUE_TYPE) {
    //                        case "1": // Bool
    //                            var isBool = CDLValue == "true" ? true : false;
    //                            var hasChecked = isBool ? "checked='checked'" : "";
    //                            if (isViewMode) {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"checkbox\" id=\"txtCDLValue_" + index + "\" " + hasChecked + " value=\"" + CDLValue + "\" tabIndex=\"19\"  disabled=\"disabled\" />");
    //                            }
    //                            else {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"checkbox\" id=\"txtCDLValue_" + index + "\" " + hasChecked + " value=\"" + CDLValue + "\" tabIndex=\"19\"  />");
    //                            }
    //                            break;
    //                        case "2": //Numeric
    //                            if (isViewMode) {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\"  disabled=\"disabled\" />");
    //                            }
    //                            else {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\" onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\" />");
    //                            }
    //                            break;
    //                        case "3": // String
    //                            if (isViewMode) {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\"  disabled=\"disabled\" />");
    //                            }
    //                            else {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\"  />");
    //                            }
    //                            break;
    //                        case "4": //Date
    //                            if (isViewMode) {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\"  disabled=\"disabled\" />");
    //                            }
    //                            else {
    //                                $(this).find("td:eq(" + colIndexCDLValue + ")").html("<input type=\"text\" class=\"input-w150\" id=\"txtCDLValue_" + index + "\" value=\"" + CDLValue + "\" tabIndex=\"19\" MaxLength=\"13\" onkeydown=\"return CheckKey(event)\" onpaste=\"return false;\" />");
    //                            }
    //                            GrandScriptUtils.DatePickerCommon("txtCDLValue_" + index);
    //                            break;
    //                    }

    //                }
    //                if (colIndexCDTDESC != null) {
    //                    $(this).find("td:eq(" + colIndexCDTDESC + ")").html("");
    //                    if (isViewMode) {
    //                        $(this).find("td:eq(" + colIndexCDTDESC + ")").html("<input type=\"text\" class=\"grdRemarksLarge\"  id=\"txtCDL_DESC_" + index + "\" value=\"" + CDL_DESC + "\" tabIndex=\"19\" disabled=\"disabled\" />");
    //                    }
    //                    else {
    //                        $(this).find("td:eq(" + colIndexCDTDESC + ")").html("<input type=\"text\" class=\"grdRemarksLarge\"  id=\"txtCDL_DESC_" + index + "\" value=\"" + CDL_DESC + "\" tabIndex=\"19\"  />");
    //                    }
    //                }
    //            }
    //        });
    //    }
    */
    if (grdID == "grdBatchDetails") {
        //  $("#tbl tr td,th").filter(':nth-child(' + (0) + ')').remove();
    }
}

function MakeNumeric(txtQty) {
    GetGridData();
    highliteStock();
    //<summary>Function used to make qty textbox numeric</summary>

    //    var floatReg = /^([0-9]{1,8})((.[0-9]{1,5})?)$/;
    //    if (!floatReg.test($(txtQty).val())) {
    //        var totalQty = parseFloat($("[id$=DTH_QTY_PLANNED]").val());
    //        var qtyPerc = GrandGrid.Utilities.GetColumnValue($(txtQty).parents("tr:first"), DispersionPreparation.DSD_QTY_PERC, $(this).parents("table:first").attr("id"));
    //        var convertFact = GrandGrid.Utilities.GetColumnValue($(txtQty).parents("tr:first"), DispersionPreparation.CONVERT_FACTOR, $(this).parents("table:first").attr("id"));
    //        var qty = ((totalQty * convertFact * qtyPerc) / 100).toFixed(dispersionDecimal);
    //        $(txtQty).val(qty);
    //    }
}

function ResetPage() {
    //<summary>function Call Afer binding Grid</summary>

    window.location = DispersionPreparation.DISPERSIONPREPARATIONLISTURL;
    return false;
}

function CalcPercQuantity(varQuantity, slno) {
    //<summary>function Call Afer binding Grid</summary>
    var qtyIndex = 0;
    var quantity = 0;
    var qtyPerc = "";
    var gridID = "";
    var convertFact = 0.0;
    var totalQuantity = 0.0;
    var rowval = DispersionPreparation.DispertionList.length - 1;
    var cou = 0;
    if (varQuantity > 0)
        totalQuantity = varQuantity;
    else
        totalQty = parseFloat($("[id$=DTH_QUANTITY]").val());
    var total = 0.0;
    var totalqty = 0.0;
    $("#grdDispersionDetails tr:has(td)").each(function (Rowindex) {   
        qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DSD_QUANTITY, $(this).parents("table:first").attr("id"));
        qtyPerc = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DSD_QTY_PERC, $(this).parents("table:first").attr("id"));
        convertFact = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.CONVERT_FACTOR, $(this).parents("table:first").attr("id"));
        if (parseFloat(qtyPerc) == 0) {
            quantity = 0;// $("#txtPreparationQuantity_" + (Rowindex)).val();
        }
        else {
            if (slno != 'undefined' || slno != undefined) {
                if (parseInt(slno) + 1 == Rowindex) {
                    quantity = $("#txtPreparationQuantity_" + (Rowindex)).val();
                }
                else {
                    quantity = ((totalQuantity * convertFact * qtyPerc) / 100).toFixed(dispersionDecimal);
                }
            }
            else {
                quantity = ((totalQuantity * convertFact * qtyPerc) / 100).toFixed(dispersionDecimal);
            }
        }
        totalqty = (parseFloat(quantity)).toFixed(dispersionDecimal);
        if (cou != 0) {
       if(parseFloat(qtyPerc) != 0)
       {
            $(this).find("td:eq(" + qtyIndex + ") input[type=text]").val(totalqty);
            total += parseFloat(totalqty);
            }
        }
        cou = cou + 1;
    });
    if (calcFlag == 1) {
        $("[id$=DTH_QUANTITY]").val(parseFloat(varQuantity).toFixed(dispersionDecimal));
        calcFlag = 0;
    }
    else {
        $("[id$=DTH_QUANTITY]").val(parseFloat(total).toFixed(dispersionDecimal));
    }
}

function CalcTotalQty() {
    var qtyIndex = 0;
    var quantity = 0;
    var cou = 0;
    var total = 0.0;
    var qtyPerc = "";
    $("#grdDispersionDetails tr:has(td)").each(function (Rowindex) {
        qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.DSD_QUANTITY, $(this).parents("table:first").attr("id"));
        qtyPerc = GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.DSD_QTY_PERC, $(this).parents("table:first").attr("id"));
        if (cou != 0) {
        if(parseFloat(qtyPerc) != 0)
        {
            quantity = $("#txtPreparationQuantity_" + (Rowindex)).val();
            total += parseFloat(quantity);
            }
        }
        cou = cou + 1;
    });
    if (!isNaN(total)) {
        if (calcFlag == 1) {
            $("[id$=DTH_QUANTITY]").val(total.toFixed(dispersionDecimal));
            calcFlag = 0;
        }
        else {
            $("[id$=DTH_QUANTITY]").val(total.toFixed(dispersionDecimal));
        }
    }
    //CaptureChanges();
}
/*
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
    $("#divPopupLatexBatches").dialog({
        autoOpen: false,
        width: 600,
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

    var ajaxUrl = DispersionPreparation.GetInspectionDetailsListURL + $("[id$=DTH_PK]").val() + "&BatchType=2";
    $("#grdInspectionDetails").removeAttr("ajaxurl")
    $("#grdInspectionDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdInspectionDetails");
    GrandGrid.MakeGrid($("#grdInspectionDetails"));
}
*/
/*//BEGIN MULTIPLEBATCH SECTION
function MultipleBatchPopup(tr) {
var DispersionTemp = $("#divData").data("dispersionObj");
GetGridData();
clearPopupDtls();
Popup();
PopUpBatchQty = 0;
DispersionPreparation.PopUpBatchList = new Array();
DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");
slNO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdDispersionDetails");
var Itmgroup = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
$("[id$=hdfItmgroup]").val(Itmgroup);
var ItmName = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.ITM_TEXT, "grdDispersionDetails");
var ItmType = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DSD_ITEM_TYPE, "grdDispersionDetails");
$("[id$=hdfItmType]").val(ItmType);
var MatrlID = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DSD_ITEM, "grdDispersionDetails");
var BatchId = $("[id$=" + (parseInt(slNO)).toString() + "_BatchNo] option:selected").val();
var Batch = $("[id$=" + (parseInt(slNO)).toString() + "_BatchNo] option:selected").text();
var ItmQty = $("#txtPreparationQuantity_" + (parseInt(slNO) + 1).toString()).val();
$("[id$=hdfTempQty]").val(ItmQty);
var CurrStock = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.ITM_CUR_STK, "grdDispersionDetails");
var Ismultiple = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DTD_IS_MULT_BATCH, "grdDispersionDetails");
var TSC = $("#txtActualTSC_" + (parseInt(slNO) + 1).toString()).val();
var TempMatrialDtlObj;
var requiredQty = ReplaceCommas(ItmQty);
for (var i in DispersionPreparation.DispersionSaveList) {
if (DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP == Itmgroup) {
TempMatrialDtlObj = DispersionPreparation.DispersionSaveList[i];
if (parseInt(Ismultiple) > 0) {
TempMatrialDtlObj.BATCH_NO_TEXT = DispersionPreparation.DispersionSaveList[i].BATCH_NO_TEXT;
TempMatrialDtlObj.DSD_BATCH_NO = DispersionPreparation.DispersionSaveList[i].DSD_BATCH_NO;
TempMatrialDtlObj.DSD_QUANTITY = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY;
TempMatrialDtlObj.ITM_CUR_STK = DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK;
TempMatrialDtlObj.DTD_ACTUAL_TSC = DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC;
//                if (DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE == 1) {
//                    TempMatrialDtlObj.DSD_STK_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH;
//                    TempMatrialDtlObj.DSD_STK_DISP_BATCH = null;
//                    TempMatrialDtlObj.DTD_BATCH = null;
//                }
//                else {
//                    TempMatrialDtlObj.DSD_STK_DISP_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                    TempMatrialDtlObj.DTD_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                    TempMatrialDtlObj.DSD_STK_BATCH = null;
//                }
if (DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != undefined && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "null" && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "" && DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH == "1") {
TempMatrialDtlObj.DSD_QUANTITY_TEMP = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP;
}

CurrStock = DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK;
ItmQty = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY;
PopUpBatchQty = parseFloat(PopUpBatchQty) + parseFloat(ItmQty);
// requiredQty = parseFloat(requiredQty) + parseFloat(ItmQty);    
}
else {
TempMatrialDtlObj.BATCH_NO_TEXT = Batch;
TempMatrialDtlObj.DSD_BATCH_NO = BatchId;
TempMatrialDtlObj.ITM_CUR_STK = CurrStock;
TempMatrialDtlObj.DSD_QUANTITY = ItmQty;
TempMatrialDtlObj.DTD_ACTUAL_TSC = TSC;
//                if (ItmType == 1) {
//                    TempMatrialDtlObj.DSD_STK_BATCH = BatchId;
//                    TempMatrialDtlObj.DSD_STK_DISP_BATCH = null;
//                    TempMatrialDtlObj.DTD_BATCH = null;
//                }
//                else {
//                    TempMatrialDtlObj.DSD_STK_DISP_BATCH = BatchId;
//                    TempMatrialDtlObj.DTD_BATCH = BatchId;
//                    TempMatrialDtlObj.DSD_STK_BATCH = null;
//                }
// requiredQty = ItmQty;
if (parseFloat(ReplaceCommas(CurrStock)) < parseFloat(ItmQty)) {
TempMatrialDtlObj.DSD_QUANTITY = CurrStock;
PopUpBatchQty = CurrStock;
}
else {
PopUpBatchQty = ItmQty;
}
}
TempMatrialDtlObj.SLNO = i;
DispersionPreparation.PopUpBatchList.push(TempMatrialDtlObj);
}
}

$("#divPopupLatexBatches").data("BatchData", DispersionPreparation.PopUpBatchList);
$("[id$=lblItemNamePopup]").html(ItmName);
$("[id$=lblTotalBatchQty]").html(parseFloat(PopUpBatchQty).toFixed(dispersionDecimal));
$("[id$=lblQtyRequired]").html(parseFloat(requiredQty).toFixed(dispersionDecimal));
var balRequired = parseFloat(requiredQty).toFixed(dispersionDecimal) - parseFloat(PopUpBatchQty).toFixed(dispersionDecimal);
if (parseFloat(balRequired) < 0) {
balRequired = "-";
$("[id$=lblQtyBal]").html(balRequired);
$("[id$=txtLatexQtyPopUp]").val(balRequired);
}
else {
$("[id$=lblQtyBal]").html(balRequired.toFixed(dispersionDecimal));
$("[id$=txtLatexQtyPopUp]").val(balRequired.toFixed(dispersionDecimal));
}
FillPopUpBatchNo(ItmType, MatrlID);
//GrandGrid.Utilities.ResetGrid(true, "grdBatchDetails");
GrandGrid.MakeGrid($("#grdBatchDetails"), 0, DispersionPreparation.PopUpBatchList);
//$('.first_tr', '.grdBatchDetails:not(:first)').hide();
if (DispersionTemp.DTH_STATUS == "0" || DispersionTemp.DTH_STATUS == "6") {
$("[id$=btnApplyBatches]").show();
}
else {
$("[id$=btnApplyBatches]").hide();
}
$("#divPopupLatexBatches").dialog("open");

}
function FillPopUpBatchNo(categoryID, materialID) {
GrandScriptUtils.FillDropDown($("[id$=ddlLatexBatchesPopUp]").attr("id"), new Object(), true, true);
if (materialID == 0) {
return false;
}

if (categoryID == 1) {
$.getJSON(DispersionPreparation.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&DepartmentID=" + deptID, function (data) {
GrandScriptUtils.FillDropDown($("[id$=ddlLatexBatchesPopUp]").attr("id"), data, true, true);
});
}
if (categoryID > 1 && materialID != 0) {
FillBatchNo($("[id$=ddlLatexBatchesPopUp]").attr("id"), categoryID, materialID);
}

}

function AddPopUpBatchDtls() {

AddBatchPopupValidations();
if ($(document.forms[0]).valid()) {
var ItmExists = 0;
for (var i in DispersionPreparation.PopUpBatchList) {
if (DispersionPreparation.PopUpBatchList[i].DSD_BATCH_NO == $("[id$=ddlLatexBatchesPopUp] option:selected").val()) {
ItmExists = 1;
}
}
if (ItmExists != 0) {
ShowErrorMessage("<ul><li><span style='color:red'>Batch already added</span></li></ul>");
}
else if (parseFloat($("[id$=lblLatexStockPopUp]").html()) < parseFloat($("[id$=txtLatexQtyPopUp]").val())) {
ShowErrorMessage("<ul><li><span style='color:red'>Not enoguh Stock</span></li></ul>");
}
else if ((parseFloat(PopUpBatchQty) + parseFloat($("[id$=txtLatexQtyPopUp]").val())).toFixed(dispersionDecimal) > parseFloat($("[id$=lblQtyRequired]").html())) {
ShowErrorMessage("<ul><li><span style='color:red'>Quantity must be Less than or equal to Required Balance</span></li></ul>");
}
else {
var TempBatchDtls = new Object();
if (DispersionPreparation.PopUpBatchList != null && DispersionPreparation.PopUpBatchList.length > 0) {
TempBatchDtls.SL_NO = DispersionPreparation.PopUpBatchList[0].SL_NO;
TempBatchDtls.SLNO = DispersionPreparation.PopUpBatchList.length + 1;
TempBatchDtls.DSD_PK = 0;
TempBatchDtls.DSD_ITEM = DispersionPreparation.PopUpBatchList[0].DSD_ITEM;
TempBatchDtls.DSD_ITEM_TYPE = DispersionPreparation.PopUpBatchList[0].DSD_ITEM_TYPE;
TempBatchDtls.DTD_ITEM_TYPE = DispersionPreparation.PopUpBatchList[0].DTD_ITEM_TYPE;
TempBatchDtls.ITM_TEXT = DispersionPreparation.PopUpBatchList[0].ITM_TEXT;
TempBatchDtls.DSD_BATCH_NO = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
TempBatchDtls.BATCH_NO_TEXT = $("[id$=ddlLatexBatchesPopUp] option:selected").text();
TempBatchDtls.DTD_MULT_BTCH_GRP = DispersionPreparation.PopUpBatchList[0].DTD_MULT_BTCH_GRP;
TempBatchDtls.DTD_IS_MULT_BATCH = 1;
TempBatchDtls.ITM_CUR_STK = parseFloat($("[id$=lblLatexStockPopUp]").html()).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY = parseFloat($("[id$=txtLatexQtyPopUp]").val()).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY_TEMP = $("[id$=hdfTempQty]").val();
TempBatchDtls.CONVERT_FACTOR = ConversionFactor;
TempBatchDtls.DSD_DISP = $("[id$=DSD_DISP] option:selected").text();
TempBatchDtls.DSD_QTY_PERC = DispersionPreparation.PopUpBatchList[0].DSD_QTY_PERC;
TempBatchDtls.DSD_QTY_UOM = DispersionPreparation.PopUpBatchList[0].DSD_QTY_UOM;
TempBatchDtls.DSD_ITEM_TYPE_TEXT = DispersionPreparation.PopUpBatchList[0].DSD_ITEM_TYPE_TEXT;
//TempBatchDtls.ITM_CODE = DispersionPreparation.PopUpBatchList[0].ITM_CODE;
TempBatchDtls.QTY_IN_STOCK = $("[id$=txtLatexQtyPopUp]").val();
TempBatchDtls.UOM_CODE = DispersionPreparation.PopUpBatchList[0].UOM_CODE;
TempBatchDtls.UOM_NAME = DispersionPreparation.PopUpBatchList[0].UOM_NAME;
TempBatchDtls.DTD_ACTUAL_TSC = DispersionPreparation.PopUpBatchList[0].DTD_ACTUAL_TSC;
//TempBatchDtls.IMG = "";
TempBatchDtls.ITM_PHR = DispersionPreparation.PopUpBatchList[0].ITM_PHR;
//                if (DispersionPreparation.PopUpBatchList[0].DSD_ITEM_TYPE == 1) {
//                    TempBatchDtls.DSD_STK_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DTD_BATCH = null;
//                    TempBatchDtls.DSD_STK_DISP_BATCH = null;
//                }
//                else {
//                    TempBatchDtls.DSD_STK_DISP_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DTD_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DSD_STK_BATCH = null;
//                }
DispersionPreparation.PopUpBatchList.push(TempBatchDtls);
}
else {
var PopUpBatchDtls = new Array();
var TempMatrialDtlObj;
for (var i in DispersionPreparation.DispertionList) {
if (DispersionPreparation.DispertionList[i].DTD_MULT_BTCH_GRP == $("[id$=hdfItmgroup]").val()) {
TempMatrialDtlObj = DispersionPreparation.DispertionList[i];
TempMatrialDtlObj.SLNO = i;
if (parseFloat(DispersionPreparation.DispertionList[i].ITM_CUR_STK) < parseFloat(DispersionPreparation.DispertionList[i].DSD_QUANTITY)) {
TempMatrialDtlObj.DSD_QUANTITY = DispersionPreparation.DispertionList[i].ITM_CUR_STK;
}
//PopUpBatchQty = parseFloat(PopUpBatchQty) + parseFloat(DispersionPreparation.DispertionList[i].DSD_QUANTITY);
}
}
PopUpBatchDtls.push(TempMatrialDtlObj);
TempBatchDtls.SL_NO = PopUpBatchDtls[0].SL_NO;
TempBatchDtls.SLNO = PopUpBatchDtls.length + 1;
TempBatchDtls.DSD_PK = 0;
TempBatchDtls.DSD_ITEM = PopUpBatchDtls[0].DSD_ITEM;
TempBatchDtls.DSD_ITEM_TYPE = PopUpBatchDtls[0].DSD_ITEM_TYPE;
TempBatchDtls.DTD_ITEM_TYPE = PopUpBatchDtls[0].DTD_ITEM_TYPE;
TempBatchDtls.ITM_TEXT = PopUpBatchDtls[0].ITM_TEXT;
TempBatchDtls.DSD_BATCH_NO = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
TempBatchDtls.BATCH_NO_TEXT = $("[id$=ddlLatexBatchesPopUp] option:selected").text();
TempBatchDtls.DTD_MULT_BTCH_GRP = PopUpBatchDtls[0].DTD_MULT_BTCH_GRP;
TempBatchDtls.DTD_IS_MULT_BATCH = 1;
TempBatchDtls.ITM_CUR_STK = parseFloat($("[id$=lblLatexStockPopUp]").html()).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY = parseFloat($("[id$=txtLatexQtyPopUp]").val()).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY_TEMP = $("[id$=hdfTempQty]").val();
TempBatchDtls.CONVERT_FACTOR = ConversionFactor;
TempBatchDtls.DSD_DISP = $("[id$=DSD_DISP] option:selected").text();
TempBatchDtls.DSD_QTY_PERC = PopUpBatchDtls[0].DSD_QTY_PERC;
TempBatchDtls.DSD_QTY_UOM = PopUpBatchDtls[0].DSD_QTY_UOM;
TempBatchDtls.DSD_ITEM_TYPE_TEXT = PopUpBatchDtls[0].DSD_ITEM_TYPE_TEXT;
//TempBatchDtls.ITM_CODE = DispersionPreparation.PopUpBatchList[0].ITM_CODE;
TempBatchDtls.QTY_IN_STOCK = $("[id$=txtLatexQtyPopUp]").val();
TempBatchDtls.UOM_CODE = PopUpBatchDtls[0].UOM_CODE;
TempBatchDtls.UOM_NAME = PopUpBatchDtls[0].UOM_NAME;
TempBatchDtls.DTD_ACTUAL_TSC = PopUpBatchDtls[0].DTD_ACTUAL_TSC;
//TempBatchDtls.IMG = "";
TempBatchDtls.ITM_PHR = PopUpBatchDtls[0].ITM_PHR;
//                if (PopUpBatchDtls[0].DSD_ITEM_TYPE == 1) {
//                    TempBatchDtls.DSD_STK_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DTD_BATCH = null;
//                    TempBatchDtls.DSD_STK_DISP_BATCH = null;
//                }
//                else {
//                    TempBatchDtls.DSD_STK_DISP_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DTD_BATCH = $("[id$=ddlLatexBatchesPopUp] option:selected").val();
//                    TempBatchDtls.DSD_STK_BATCH = null;
//                }
DispersionPreparation.PopUpBatchList.push(TempBatchDtls);
$("#divPopupLatexBatches").data("BatchData", DispersionPreparation.PopUpBatchList);
}


$("#divPopupLatexBatches").data("BatchData", DispersionPreparation.PopUpBatchList);
//GrandGrid.Utilities.ResetGrid(true, "grdBatchDetails");
GrandGrid.MakeGrid($("#grdBatchDetails"), 0, DispersionPreparation.PopUpBatchList);

PopUpBatchQty = parseFloat(PopUpBatchQty) + parseFloat($("[id$=txtLatexQtyPopUp]").val());
$("[id$=lblTotalBatchQty]").html((PopUpBatchQty).toFixed(dispersionDecimal));
//$("[id$=lblQtyRequired]").html(ItmQty);
var balRequired = parseFloat($("[id$=lblQtyRequired]").html()) - parseFloat(PopUpBatchQty).toFixed(dispersionDecimal);
if (parseFloat(balRequired) < 0) {
balRequired = "-";
$("[id$=lblQtyBal]").html(balRequired);
$("[id$=txtLatexQtyPopUp]").val(balRequired);
}
else {
$("[id$=lblQtyBal]").html(balRequired.toFixed(dispersionDecimal));
$("[id$=txtLatexQtyPopUp]").val(balRequired.toFixed(dispersionDecimal));
}
$("#divPopupLatexBatches").dialog("open");
clearPopupDtls();
}
}
return false;
}

function clearPopupDtls() {
//$("[id$=txtLatexQtyPopUp]").val("");
$("[id$=lblLatexStockPopUp]").html("");
$("[id$=ddlLatexBatchesPopUp]").val("0");
ItmExists = 0;
}

function ApplyPopUpBatches() {
//GetGridData();
if (isNaN(parseFloat($("[id$=lblQtyBal]").html())) || parseFloat($("[id$=lblQtyBal]").html()) != 0) {
ShowErrorMessage("<ul><li><span style='color:red'>Total batch quantity should be same as Required quantity</span></li></ul>");
return false;
}
else {
DispersionPreparation.DispersionSaveList = new Array();
for (var i in DispersionPreparation.PopUpBatchList) {
if (DispersionPreparation.PopUpBatchList.length > 1) {
DispersionPreparation.PopUpBatchList[i].DTD_IS_MULT_BATCH = 1;
}
else {
DispersionPreparation.PopUpBatchList[i].DTD_IS_MULT_BATCH = 0;
}
}
DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");

//        //Removing row with same group-- JSlinq for selecting items which are not belong to the multiple batch group
//        DispersionPreparation.DispersionSaveList = (JSLINQ(DispersionPreparation.DispersionSaveList).
//                                                Where(function (item) { return item.DTD_MULT_BTCH_GRP != $("[id$=hdfItmgroup]").val() })).items;

var TempSaveList = $("#divSaveData").data("SaveData");
DispersionPreparation.DispersionSaveList = new Array();
for (var i in TempSaveList) {
if (TempSaveList[i].DTD_MULT_BTCH_GRP != $("[id$=hdfItmgroup]").val()) {
var TempBatchDtls = new Object();
TempBatchDtls.SL_NO = TempSaveList[i].SL_NO;
TempBatchDtls.SLNO = TempSaveList[i].SLNO;
TempBatchDtls.DSD_PK = TempSaveList[i].DSD_PK;
TempBatchDtls.DSD_ITEM = TempSaveList[i].DSD_ITEM;
TempBatchDtls.DSD_ITEM_TYPE = TempSaveList[i].DSD_ITEM_TYPE;
TempBatchDtls.DTD_ITEM_TYPE = TempSaveList[i].DTD_ITEM_TYPE;
TempBatchDtls.ITM_TEXT = TempSaveList[i].ITM_TEXT;
TempBatchDtls.BATCH_NO_TEXT = TempSaveList[i].BATCH_NO_TEXT;
TempBatchDtls.DTD_MULT_BTCH_GRP = TempSaveList[i].DTD_MULT_BTCH_GRP;
TempBatchDtls.DTD_IS_MULT_BATCH = TempSaveList[i].DTD_IS_MULT_BATCH;
TempBatchDtls.ITM_CUR_STK = parseFloat(TempSaveList[i].ITM_CUR_STK).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY = parseFloat(TempSaveList[i].DSD_QUANTITY).toFixed(dispersionDecimal);
TempBatchDtls.CONVERT_FACTOR = TempSaveList[i].CONVERT_FACTOR;
TempBatchDtls.DSD_DISP = TempSaveList[i].DSD_DISP;
TempBatchDtls.DSD_QTY_PERC = TempSaveList[i].DSD_QTY_PERC;
TempBatchDtls.DSD_QTY_UOM = TempSaveList[i].DSD_QTY_UOM;
TempBatchDtls.DSD_ITEM_TYPE_TEXT = TempSaveList[i].DSD_ITEM_TYPE_TEXT;
TempBatchDtls.QTY_IN_STOCK = TempSaveList[i].QTY_IN_STOCK;
TempBatchDtls.UOM_CODE = TempSaveList[i].UOM_CODE;
TempBatchDtls.UOM_NAME = TempSaveList[i].UOM_NAME;
TempBatchDtls.DTD_ACTUAL_TSC = TempSaveList[i].DTD_ACTUAL_TSC;
//                if (TempSaveList[i].DSD_ITEM_TYPE == 1) {
//                    TempBatchDtls.DSD_STK_BATCH = TempSaveList[i].DSD_STK_BATCH;
//                    TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_BATCH;
//                    TempBatchDtls.DSD_STK_DISP_BATCH = null;
//                    TempBatchDtls.DTD_BATCH = null;
//                }
//                else {
//                    TempBatchDtls.DSD_STK_DISP_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
//                    TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_DISP_BATCH;
//                    TempBatchDtls.DTD_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
//                    TempBatchDtls.DSD_STK_BATCH = null;
//                }
if (TempSaveList[i].DSD_QUANTITY_TEMP != undefined && TempSaveList[i].DSD_QUANTITY_TEMP != "null" && TempSaveList[i].DSD_QUANTITY_TEMP != "" && TempSaveList[i].DTD_IS_MULT_BATCH == "1") {
TempBatchDtls.DSD_QUANTITY_TEMP = TempSaveList[i].DSD_QUANTITY_TEMP;
}
//TempBatchDtls.IMG = "";
TempBatchDtls.ITM_PHR = TempSaveList[i].ITM_PHR;
DispersionPreparation.DispersionSaveList.push(TempBatchDtls);
}
}
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);


//Adding multiple batches to main list        
for (var i in DispersionPreparation.PopUpBatchList) {
DispersionPreparation.DispersionSaveList.push(DispersionPreparation.PopUpBatchList[i]);

}
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);
//Reassigning serial number
var srl = 0;
$("#grdDispersionDetails tr:has(td)").each(function (index) {
if (index != 0) {
var GroupIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
var Group = GrandGrid.Utilities.GetColumnValue($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
for (var i in DispersionPreparation.DispersionSaveList) {
if (DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP == Group) {
DispersionPreparation.DispersionSaveList[i].SL_NO = srl;
srl = srl + 1;
}
}
}
});


$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);

//GetDisplayGridData();
DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
GrandGrid.Utilities.ResetGrid(true, "grdDispersionDetails");
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
$("#divPopupLatexBatches").dialog("close");
}
highliteStock();
checkMultiBatchQty();
$("[id$=hdfTempQty]").val("0");
$("[id$=hdfItmgroup]").val("0");
$("[id$=hdfItmType]").val("0");
return false;
}

//To set Data in Edit Mode
function SetEditModeData(DispersionDisplayList) {
DispersionPreparation.DispersionSaveList = new Array();
var sln = 1;
for (var i in DispersionDisplayList) {
var TempBatchDtls = new Object();
TempBatchDtls.SL_NO = DispersionDisplayList[i].SL_NO;
TempBatchDtls.SLNO = sln;
TempBatchDtls.DSD_PK = DispersionDisplayList[i].DSD_PK;
TempBatchDtls.DSD_ITEM = DispersionDisplayList[i].DSD_ITEM;
TempBatchDtls.DSD_ITEM_TYPE = DispersionDisplayList[i].DSD_ITEM_TYPE;
TempBatchDtls.DTD_ITEM_TYPE = DispersionDisplayList[i].DTD_ITEM_TYPE;
TempBatchDtls.ITM_TEXT = DispersionDisplayList[i].ITM_TEXT;
//TempBatchDtls.BATCH_NO_TEXT = DispersionDisplayList[i].BATCH_NO_TEXT;
//TempBatchDtls.DTD_MULT_BTCH_GRP = DispersionDisplayList[i].DTD_MULT_BTCH_GRP;
//TempBatchDtls.DTD_IS_MULT_BATCH = DispersionDisplayList[i].DTD_IS_MULT_BATCH;
TempBatchDtls.ITM_CUR_STK = parseFloat(DispersionDisplayList[i].ITM_CUR_STK).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY = parseFloat(DispersionDisplayList[i].DSD_QUANTITY).toFixed(dispersionDecimal);
TempBatchDtls.CONVERT_FACTOR = DispersionDisplayList[i].CONVERT_FACTOR;
TempBatchDtls.DSD_DISP = DispersionDisplayList[i].DSD_DISP;
TempBatchDtls.DSD_QTY_PERC = DispersionDisplayList[i].DSD_QTY_PERC;
TempBatchDtls.DSD_QTY_UOM = DispersionDisplayList[i].DSD_QTY_UOM;
TempBatchDtls.DSD_ITEM_TYPE_TEXT = DispersionDisplayList[i].DSD_ITEM_TYPE_TEXT;
TempBatchDtls.QTY_IN_STOCK = DispersionDisplayList[i].QTY_IN_STOCK;
TempBatchDtls.UOM_CODE = DispersionDisplayList[i].UOM_CODE;
TempBatchDtls.UOM_NAME = DispersionDisplayList[i].UOM_NAME;
//TempBatchDtls.DTD_ACTUAL_TSC = DispersionDisplayList[i].DTD_ACTUAL_TSC;
//        if (DispersionDisplayList[i].DSD_ITEM_TYPE == 1) {
//            TempBatchDtls.DSD_STK_BATCH = DispersionDisplayList[i].DSD_STK_BATCH;
//            TempBatchDtls.DSD_BATCH_NO = DispersionDisplayList[i].DSD_STK_BATCH;
//            TempBatchDtls.DSD_STK_DISP_BATCH = null;
//            TempBatchDtls.DSD_BATCH_NO = null;
//        }
//        else {
//            TempBatchDtls.DSD_STK_DISP_BATCH = DispersionDisplayList[i].DSD_STK_DISP_BATCH;
//            TempBatchDtls.DSD_BATCH_NO = DispersionDisplayList[i].DSD_STK_DISP_BATCH;
//            TempBatchDtls.DTD_BATCH = DispersionDisplayList[i].DSD_STK_DISP_BATCH;
//            TempBatchDtls.DSD_STK_BATCH = null;
//        }
//TempBatchDtls.IMG = "";
TempBatchDtls.ITM_PHR = DispersionDisplayList[i].ITM_PHR;
DispersionPreparation.DispersionSaveList.push(TempBatchDtls);
sln++;
}
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);
//GetDisplayGridData();

DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
GrandGrid.Utilities.ResetGrid(true, "grdDispersionDetails");
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
return false;
}

//For checking if total quantity of an item with multiple batches is differ from required qty
function checkMultiBatchQty() {
var SameQty = true;
$("#grdDispersionDetails tr:has(td)").each(function (index) {
var TotalQty = 0;
if (index != 0) {
var GroupIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
var Group = GrandGrid.Utilities.GetColumnValue($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
var MultiBatchIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
var MultiBatch = GrandGrid.Utilities.GetColumnValue($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
var Qty = parseFloat($("#txtPreparationQuantity_" + index).val());
if (MultiBatch == "1") {
for (var i in DispersionPreparation.DispersionSaveList) {
if (DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP == Group) {
TotalQty = parseFloat(TotalQty) + parseFloat(DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY);
}
}
if (parseFloat(Qty).toFixed(dispersionDecimal) != parseFloat(TotalQty).toFixed(dispersionDecimal)) {
SameQty = false;
var selectedRowColor;
selectedRowColor = '#F9DEE5';
$(this).closest('tr').addClass('highlight');
}
}
}
});
return SameQty;
}

//To Disable batch ddl in grid with multple batches
function DisableBatchDDL(currRow, index) {
var count = 0;
var Group = GrandGrid.Utilities.GetColumnValue(currRow, "DTD_MULT_BTCH_GRP", "grdDispersionDetails");
for (var i in DispersionPreparation.DispersionSaveList) {
if (Group == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
count++;
}
}
if (count > 1) {
return true;
}
}

//To get display list from save list
function GetDisplayGridData() {
SortSaveList();
DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");
//Linq used for getting Group values from save list
var Group = JSLINQ(DispersionPreparation.DispersionSaveList)
//                          .Distinct(function (items) { return items.DTD_MULT_BTCH_GRP; })
.Select(function (a) { return a; })
.OrderBy(function (b) { b.SL_NO; }).ToArray();
//Sorting the Group values in asc order
for (var k = 0; k < Group.length; k++) {
for (var l = k + 1; l < Group.length; l++) {
if (Group[k] > Group[l]) {
var temp = Group[k];
Group[k] = Group[l];
Group[l] = temp;
}
}
}
//Addind items to display list based on group
DispersionPreparation.DispertionList = new Array();
for (var j in Group) {
var insertFlag = false;
var Total = 0;
for (var i in DispersionPreparation.DispersionSaveList) {
var TempDispersionDtls = new Object();
if (DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP == Group[j]) {
Total = parseFloat(Total) + parseFloat(DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY);
if (insertFlag == false) {
TempDispersionDtls.SL_NO = DispersionPreparation.DispersionSaveList[i].SL_NO;
TempDispersionDtls.SLNO = DispersionPreparation.DispersionSaveList[i].SLNO;
TempDispersionDtls.DSD_PK = DispersionPreparation.DispersionSaveList[i].DSD_PK;
TempDispersionDtls.DSD_ITEM = DispersionPreparation.DispersionSaveList[i].DSD_ITEM;
TempDispersionDtls.DSD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE;
TempDispersionDtls.DTD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[i].DTD_ITEM_TYPE;
TempDispersionDtls.ITM_TEXT = DispersionPreparation.DispersionSaveList[i].ITM_TEXT;
//TempDispersionDtls.DSD_BATCH_NO = DispersionPreparation.DispersionSaveList[i].DSD_BATCH_NO;
//TempDispersionDtls.BATCH_NO_TEXT = DispersionPreparation.DispersionSaveList[i].BATCH_NO_TEXT;
//TempDispersionDtls.DTD_MULT_BTCH_GRP = DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP;
//TempDispersionDtls.DTD_IS_MULT_BATCH = DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH;
TempDispersionDtls.ITM_CUR_STK = DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK;
TempDispersionDtls.DSD_QUANTITY = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY;
TempDispersionDtls.CONVERT_FACTOR = DispersionPreparation.DispersionSaveList[i].CONVERT_FACTOR;
TempDispersionDtls.DSD_DISP = DispersionPreparation.DispersionSaveList[i].DSD_DISP;
TempDispersionDtls.DSD_QTY_PERC = DispersionPreparation.DispersionSaveList[i].DSD_QTY_PERC;
TempDispersionDtls.DSD_QTY_UOM = DispersionPreparation.DispersionSaveList[i].DSD_QTY_UOM;
TempDispersionDtls.DSD_ITEM_TYPE_TEXT = DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE_TEXT;
TempDispersionDtls.QTY_IN_STOCK = DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK;
TempDispersionDtls.UOM_CODE = DispersionPreparation.DispersionSaveList[i].UOM_CODE;
TempDispersionDtls.UOM_NAME = DispersionPreparation.DispersionSaveList[i].UOM_NAME;
//TempDispersionDtls.DTD_ACTUAL_TSC = DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC;
TempDispersionDtls.DSD_QUANTITY = Total;
//                    if (DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE == 1) {
//                        TempDispersionDtls.DSD_STK_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH;
//                        TempDispersionDtls.DSD_STK_DISP_BATCH = null;
//                        TempDispersionDtls.DTD_BATCH = null;
//                    }
//                    else {
//                        TempDispersionDtls.DSD_STK_DISP_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                        TempDispersionDtls.DTD_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                        TempDispersionDtls.DSD_STK_BATCH = null;
//                    }
if (DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != undefined && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "null" && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "" && DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH == "1") {
TempDispersionDtls.DSD_QUANTITY_TEMP = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP;
}
//TempDispersionDtls.IMG = DispersionPreparation.DispersionSaveList[i].IMG;
TempDispersionDtls.ITM_PHR = DispersionPreparation.DispersionSaveList[i].ITM_PHR;
DispersionPreparation.DispertionList.push(TempDispersionDtls);
insertFlag = true;
}
DispersionPreparation.DispertionList[j].DSD_QUANTITY = Total;
if (DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH == "1") {
if (DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != undefined && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "null" && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "") {
if (DispersionPreparation.DispertionList[j].DSD_QUANTITY != DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP) {
DispersionPreparation.DispertionList[j].DSD_QUANTITY = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP;
}
}
}
}
}
}

var srlno = 0;
for (var i in DispersionPreparation.DispertionList) {
DispersionPreparation.DispertionList[i].SL_NO = srlno;
srlno = srlno + 1;
}
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
}

function SortSaveList() {
//Checking for highest value in list using loop to sort the list
DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");
for (var i = 0; i < DispersionPreparation.DispersionSaveList.length; i++) {
for (var j = parseInt(i + 1); j < parseInt(DispersionPreparation.DispersionSaveList.length); j++) {
if (parseInt(DispersionPreparation.DispersionSaveList[i].SL_NO) > parseInt(DispersionPreparation.DispersionSaveList[j].SL_NO)) {
var templist = new Object();
templist.SL_NO = DispersionPreparation.DispersionSaveList[i].SL_NO;
templist.SLNO = DispersionPreparation.DispersionSaveList[i].SLNO;
templist.DSD_PK = DispersionPreparation.DispersionSaveList[i].DSD_PK;
templist.DSD_ITEM = DispersionPreparation.DispersionSaveList[i].DSD_ITEM;
templist.DSD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE;
templist.DTD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[i].DTD_ITEM_TYPE;
templist.ITM_TEXT = DispersionPreparation.DispersionSaveList[i].ITM_TEXT;
//templist.BATCH_NO_TEXT = DispersionPreparation.DispersionSaveList[i].BATCH_NO_TEXT;
//templist.DTD_MULT_BTCH_GRP = DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP;
//templist.DTD_IS_MULT_BATCH = DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH;
templist.ITM_CUR_STK = DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK;
templist.DSD_QUANTITY = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY;
templist.CONVERT_FACTOR = DispersionPreparation.DispersionSaveList[i].CONVERT_FACTOR;
templist.DSD_DISP = DispersionPreparation.DispersionSaveList[i].DSD_DISP;
templist.DSD_QTY_PERC = DispersionPreparation.DispersionSaveList[i].DSD_QTY_PERC;
templist.DSD_QTY_UOM = DispersionPreparation.DispersionSaveList[i].DSD_QTY_UOM;
templist.DSD_ITEM_TYPE_TEXT = DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE_TEXT;
templist.QTY_IN_STOCK = DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK;
templist.UOM_CODE = DispersionPreparation.DispersionSaveList[i].UOM_CODE;
templist.UOM_NAME = DispersionPreparation.DispersionSaveList[i].UOM_NAME;
//templist.DTD_ACTUAL_TSC = DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC;
//templist.DSD_BATCH_NO = DispersionPreparation.DispersionSaveList[i].DSD_BATCH_NO;
//                if (DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE == 1) {
//                    templist.DSD_STK_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH;
//                    templist.DTD_BATCH = null;
//                    templist.DSD_STK_DISP_BATCH = null;
//                }
//                else {
//                    templist.DSD_STK_DISP_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                    templist.DTD_BATCH = DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH;
//                    templist.DSD_STK_BATCH = null;
//                }
if (DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != undefined && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "null" && DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP != "" && DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH == "1") {
templist.DSD_QUANTITY_TEMP = DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP;
}
//templist.IMG = DispersionPreparation.DispersionSaveList[i].IMG;
templist.ITM_PHR = DispersionPreparation.DispersionSaveList[i].ITM_PHR;



//Swaping the lowest serial number to top of the list
DispersionPreparation.DispersionSaveList[i].SL_NO = DispersionPreparation.DispersionSaveList[j].SL_NO;
DispersionPreparation.DispersionSaveList[i].SLNO = DispersionPreparation.DispersionSaveList[j].SLNO;
DispersionPreparation.DispersionSaveList[i].DSD_PK = DispersionPreparation.DispersionSaveList[j].DSD_PK;
DispersionPreparation.DispersionSaveList[i].DSD_ITEM = DispersionPreparation.DispersionSaveList[j].DSD_ITEM;
DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[j].DSD_ITEM_TYPE;
DispersionPreparation.DispersionSaveList[i].DTD_ITEM_TYPE = DispersionPreparation.DispersionSaveList[j].DTD_ITEM_TYPE;
DispersionPreparation.DispersionSaveList[i].ITM_TEXT = DispersionPreparation.DispersionSaveList[j].ITM_TEXT;
//DispersionPreparation.DispersionSaveList[i].BATCH_NO_TEXT = DispersionPreparation.DispersionSaveList[j].BATCH_NO_TEXT;
//DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP = DispersionPreparation.DispersionSaveList[j].DTD_MULT_BTCH_GRP;
//DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH = DispersionPreparation.DispersionSaveList[j].DTD_IS_MULT_BATCH;
DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = parseFloat(DispersionPreparation.DispersionSaveList[j].ITM_CUR_STK).toFixed(dispersionDecimal);
DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY = parseFloat(DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY).toFixed(dispersionDecimal);
DispersionPreparation.DispersionSaveList[i].CONVERT_FACTOR = DispersionPreparation.DispersionSaveList[j].CONVERT_FACTOR;
DispersionPreparation.DispersionSaveList[i].DSD_DISP = DispersionPreparation.DispersionSaveList[j].DSD_DISP;
DispersionPreparation.DispersionSaveList[i].DSD_QTY_PERC = DispersionPreparation.DispersionSaveList[j].DSD_QTY_PERC;
DispersionPreparation.DispersionSaveList[i].DSD_QTY_UOM = DispersionPreparation.DispersionSaveList[j].DSD_QTY_UOM;
DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE_TEXT = DispersionPreparation.DispersionSaveList[j].DSD_ITEM_TYPE_TEXT;
DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = DispersionPreparation.DispersionSaveList[j].QTY_IN_STOCK;
DispersionPreparation.DispersionSaveList[i].UOM_CODE = DispersionPreparation.DispersionSaveList[j].UOM_CODE;
DispersionPreparation.DispersionSaveList[i].UOM_NAME = DispersionPreparation.DispersionSaveList[j].UOM_NAME;
//DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC = DispersionPreparation.DispersionSaveList[j].DTD_ACTUAL_TSC;
//DispersionPreparation.DispersionSaveList[i].DSD_BATCH_NO = DispersionPreparation.DispersionSaveList[j].DSD_STK_DISP_BATCH;
//                if (DispersionPreparation.DispersionSaveList[j].DSD_ITEM_TYPE == 1) {
//                    DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = DispersionPreparation.DispersionSaveList[j].DSD_STK_BATCH;
//                    DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = null;
//                    DispersionPreparation.DispersionSaveList[i].DTD_BATCH = null;
//                }
//                else {
//                    DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = DispersionPreparation.DispersionSaveList[j].DSD_STK_DISP_BATCH;
//                    DispersionPreparation.DispersionSaveList[i].DTD_BATCH = DispersionPreparation.DispersionSaveList[j].DSD_STK_DISP_BATCH;
//                    DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = null;
//                }
if (DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY_TEMP != undefined && DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY_TEMP != "null" && DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY_TEMP != "" && DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH == "1") {
DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP = DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY_TEMP;
}
//DispersionPreparation.DispersionSaveList[i].IMG = DispersionPreparation.DispersionSaveList[j].IMG;
DispersionPreparation.DispersionSaveList[i].ITM_PHR = DispersionPreparation.DispersionSaveList[j].ITM_PHR;

//Swaping the replaced value from temp to next position
DispersionPreparation.DispersionSaveList[j].SL_NO = templist.SL_NO;
DispersionPreparation.DispersionSaveList[j].SLNO = templist.SLNO;
DispersionPreparation.DispersionSaveList[j].DSD_PK = templist.DSD_PK;
DispersionPreparation.DispersionSaveList[j].DSD_ITEM = templist.DSD_ITEM;
DispersionPreparation.DispersionSaveList[j].DSD_ITEM_TYPE = templist.DSD_ITEM_TYPE;
DispersionPreparation.DispersionSaveList[j].DTD_ITEM_TYPE = templist.DTD_ITEM_TYPE;
DispersionPreparation.DispersionSaveList[j].ITM_TEXT = templist.ITM_TEXT;
//DispersionPreparation.DispersionSaveList[j].BATCH_NO_TEXT = templist.BATCH_NO_TEXT;
//DispersionPreparation.DispersionSaveList[j].DTD_MULT_BTCH_GRP = templist.DTD_MULT_BTCH_GRP;
//DispersionPreparation.DispersionSaveList[j].DTD_IS_MULT_BATCH = templist.DTD_IS_MULT_BATCH;
DispersionPreparation.DispersionSaveList[j].ITM_CUR_STK = parseFloat(templist.ITM_CUR_STK).toFixed(dispersionDecimal);
DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY = parseFloat(templist.DSD_QUANTITY).toFixed(dispersionDecimal);
DispersionPreparation.DispersionSaveList[j].CONVERT_FACTOR = templist.CONVERT_FACTOR;
DispersionPreparation.DispersionSaveList[j].DSD_DISP = templist.DSD_DISP;
DispersionPreparation.DispersionSaveList[j].DSD_QTY_PERC = templist.DSD_QTY_PERC;
DispersionPreparation.DispersionSaveList[j].DSD_QTY_UOM = templist.DSD_QTY_UOM;
DispersionPreparation.DispersionSaveList[j].DSD_ITEM_TYPE_TEXT = templist.DSD_ITEM_TYPE_TEXT;
DispersionPreparation.DispersionSaveList[j].QTY_IN_STOCK = templist.QTY_IN_STOCK;
DispersionPreparation.DispersionSaveList[j].UOM_CODE = templist.UOM_CODE;
DispersionPreparation.DispersionSaveList[j].UOM_NAME = templist.UOM_NAME;
//DispersionPreparation.DispersionSaveList[j].DTD_ACTUAL_TSC = templist.DTD_ACTUAL_TSC;
//                if (templist.DSD_ITEM_TYPE == 1) {
//                    DispersionPreparation.DispersionSaveList[j].DSD_STK_BATCH = templist.DSD_STK_BATCH;
//                    DispersionPreparation.DispersionSaveList[j].DSD_BATCH_NO = templist.DSD_STK_BATCH;
//                    DispersionPreparation.DispersionSaveList[j].DSD_STK_DISP_BATCH = null;
//                    DispersionPreparation.DispersionSaveList[j].DTD_BATCH = null;
//                }
//                else {
//                    DispersionPreparation.DispersionSaveList[j].DSD_STK_DISP_BATCH = templist.DSD_STK_DISP_BATCH;
//                    DispersionPreparation.DispersionSaveList[j].DSD_BATCH_NO = templist.DSD_STK_DISP_BATCH;
//                    DispersionPreparation.DispersionSaveList[j].DTD_BATCH = templist.DSD_STK_DISP_BATCH;
//                    DispersionPreparation.DispersionSaveList[j].DSD_STK_BATCH = null;
//                }
if (templist.DSD_QUANTITY_TEMP != undefined && templist.DSD_QUANTITY_TEMP != "null" && templist.DSD_QUANTITY_TEMP != "" && templist.DTD_IS_MULT_BATCH == "1") {
DispersionPreparation.DispersionSaveList[j].DSD_QUANTITY_TEMP = templist.DSD_QUANTITY_TEMP;
}
//DispersionPreparation.DispersionSaveList[j].IMG = templist.IMG;
DispersionPreparation.DispersionSaveList[j].ITM_PHR = templist.ITM_PHR;
}
}
}
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);
}


function ClearMultipleBatch(tr) {
var Itmgroup = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DTD_MULT_BTCH_GRP, "grdDispersionDetails");
var category = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DSD_ITEM_TYPE, "grdDispersionDetails");
slNO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdDispersionDetails");
var qty = $("#txtPreparationQuantity_" + (parseInt(slNO) + 1).toString()).val();
var Item = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");

//Removing Items with group which needs to be removed and adding rest of the items to save list
var TempSaveList = $("#divSaveData").data("SaveData");
DispersionPreparation.DispersionSaveList = new Array();
for (var i in TempSaveList) {
if (TempSaveList[i].DTD_MULT_BTCH_GRP != Itmgroup) {
var TempBatchDtls = new Object();
TempBatchDtls.SL_NO = TempSaveList[i].SL_NO;
TempBatchDtls.SLNO = TempSaveList[i].SLNO;
TempBatchDtls.DSD_PK = TempSaveList[i].DSD_PK;
TempBatchDtls.DSD_ITEM = TempSaveList[i].DSD_ITEM;
TempBatchDtls.DSD_ITEM_TYPE = TempSaveList[i].DSD_ITEM_TYPE;
TempBatchDtls.DTD_ITEM_TYPE = TempSaveList[i].DTD_ITEM_TYPE;
TempBatchDtls.ITM_TEXT = TempSaveList[i].ITM_TEXT;
Te//mpBatchDtls.BATCH_NO_TEXT = TempSaveList[i].BATCH_NO_TEXT;
//TempBatchDtls.DTD_MULT_BTCH_GRP = TempSaveList[i].DTD_MULT_BTCH_GRP;
Temp//BatchDtls.DTD_IS_MULT_BATCH = TempSaveList[i].DTD_IS_MULT_BATCH;
TempBatchDtls.ITM_CUR_STK = parseFloat(TempSaveList[i].ITM_CUR_STK).toFixed(dispersionDecimal);
TempBatchDtls.DSD_QUANTITY = parseFloat(TempSaveList[i].DSD_QUANTITY).toFixed(dispersionDecimal);
TempBatchDtls.CONVERT_FACTOR = TempSaveList[i].CONVERT_FACTOR;
TempBatchDtls.DSD_DISP = TempSaveList[i].DSD_DISP;
TempBatchDtls.DSD_QTY_PERC = TempSaveList[i].DSD_QTY_PERC;
TempBatchDtls.DSD_QTY_UOM = TempSaveList[i].DSD_QTY_UOM;
TempBatchDtls.DSD_ITEM_TYPE_TEXT = TempSaveList[i].DSD_ITEM_TYPE_TEXT;
TempBatchDtls.QTY_IN_STOCK = TempSaveList[i].QTY_IN_STOCK;
TempBatchDtls.UOM_CODE = TempSaveList[i].UOM_CODE;
TempBatchDtls.UOM_NAME = TempSaveList[i].UOM_NAME;
//TempBatchDtls.DTD_ACTUAL_TSC = TempSaveList[i].DTD_ACTUAL_TSC;
//            if (TempSaveList[i].DSD_ITEM_TYPE == 1) {
//                TempBatchDtls.DSD_STK_BATCH = TempSaveList[i].DSD_STK_BATCH;
//                TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_BATCH;
//                TempBatchDtls.DSD_STK_DISP_BATCH = null;
//                TempBatchDtls.DTD_BATCH = null;
//            }
//            else {
//                TempBatchDtls.DSD_STK_DISP_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
//                TempBatchDtls.DSD_BATCH_NO = TempSaveList[i].DSD_STK_DISP_BATCH;
//                TempBatchDtls.DTD_BATCH = TempSaveList[i].DSD_STK_DISP_BATCH;
//                TempBatchDtls.DSD_STK_BATCH = null;
//            }
if (TempSaveList[i].DSD_QUANTITY_TEMP != undefined && TempSaveList[i].DSD_QUANTITY_TEMP != "null" && TempSaveList[i].DSD_QUANTITY_TEMP != "" && TempSaveList[i].DTD_IS_MULT_BATCH == "1") {
TempBatchDtls.DSD_QUANTITY_TEMP = TempSaveList[i].DSD_QUANTITY_TEMP;
}
//TempBatchDtls.IMG = "";
TempBatchDtls.ITM_PHR = TempSaveList[i].ITM_PHR;
DispersionPreparation.DispersionSaveList.push(TempBatchDtls);
}
}
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);

//Adding default item to the cleared row from grid after clearng the multiple batches
var templist = new Object();
templist.SL_NO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdDispersionDetails");
templist.SLNO = GrandGrid.Utilities.GetColumnValue(tr, "SLNO", "grdDispersionDetails");
templist.DSD_PK = 0; //GrandGrid.Utilities.GetColumnValue(tr, "DSD_PK, "grdDispersionDetails");
templist.DSD_ITEM = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");
templist.DSD_ITEM_TYPE = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM_TYPE", "grdDispersionDetails");
templist.DTD_ITEM_TYPE = GrandGrid.Utilities.GetColumnValue(tr, "DTD_ITEM_TYPE", "grdDispersionDetails");
templist.ITM_TEXT = GrandGrid.Utilities.GetColumnValue(tr, "ITM_TEXT", "grdDispersionDetails");
// templist.BATCH_NO_TEXT = GrandGrid.Utilities.GetColumnValue(tr, "BATCH_NO_TEXT", "grdDispersionDetails");
//templist.DTD_MULT_BTCH_GRP = GrandGrid.Utilities.GetColumnValue(tr, "DTD_MULT_BTCH_GRP", "grdDispersionDetails");
//templist.DTD_IS_MULT_BATCH = 0; //GrandGrid.Utilities.GetColumnValue(tr, "DTD_IS_MULT_BATCH, "grdDispersionDetails");
templist.ITM_CUR_STK = GrandGrid.Utilities.GetColumnValue(tr, "ITM_CUR_STK", "grdDispersionDetails");
templist.DSD_QUANTITY = qty; //GrandGrid.Utilities.GetColumnValue(tr, "DSD_QUANTITY", "grdDispersionDetails");
templist.CONVERT_FACTOR = GrandGrid.Utilities.GetColumnValue(tr, "CONVERT_FACTOR", "grdDispersionDetails");
templist.DSD_DISP = GrandGrid.Utilities.GetColumnValue(tr, "DSD_DISP", "grdDispersionDetails");
templist.DSD_QTY_PERC = GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_PERC", "grdDispersionDetails");
templist.DSD_QTY_UOM = GrandGrid.Utilities.GetColumnValue(tr, "DSD_QTY_UOM", "grdDispersionDetails");
templist.DSD_ITEM_TYPE_TEXT = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM_TYPE_TEXT", "grdDispersionDetails");
templist.QTY_IN_STOCK = GrandGrid.Utilities.GetColumnValue(tr, "QTY_IN_STOCK", "grdDispersionDetails");
templist.UOM_CODE = GrandGrid.Utilities.GetColumnValue(tr, "UOM_CODE", "grdDispersionDetails");
templist.UOM_NAME = GrandGrid.Utilities.GetColumnValue(tr, "UOM_NAME", "grdDispersionDetails");
//templist.DTD_ACTUAL_TSC = GrandGrid.Utilities.GetColumnValue(tr, "DTD_ACTUAL_TSC", "grdDispersionDetails");
//templist.DSD_BATCH_NO = GrandGrid.Utilities.GetColumnValue(tr, "DSD_BATCH_NO", "grdDispersionDetails");
//    if (GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM_TYPE", "grdDispersionDetails") == 1) {
//        templist.DSD_STK_BATCH = GrandGrid.Utilities.GetColumnValue(tr, "DSD_STK_BATCH", "grdDispersionDetails");
//        templist.DSD_BATCH_NO = GrandGrid.Utilities.GetColumnValue(tr, "DSD_STK_BATCH", "grdDispersionDetails");
//        templist.DTD_BATCH = null;
//        templist.DSD_STK_DISP_BATCH = null;
//    }
//    else {
//        templist.DSD_STK_DISP_BATCH = GrandGrid.Utilities.GetColumnValue(tr, "DSD_STK_DISP_BATCH", "grdDispersionDetails");
//        templist.DSD_BATCH_NO = GrandGrid.Utilities.GetColumnValue(tr, "DSD_STK_DISP_BATCH", "grdDispersionDetails");
//        templist.DTD_BATCH = GrandGrid.Utilities.GetColumnValue(tr, "DTD_BATCH", "grdDispersionDetails");
//        templist.DSD_STK_BATCH = null;
//    }
//templist.IMG = ""; //GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.IMG, "grdDispersionDetails");
templist.ITM_PHR = GrandGrid.Utilities.GetColumnValue(tr, "ITM_PHR", "grdDispersionDetails");
DispersionPreparation.DispersionSaveList.push(templist);
$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);

//GetDisplayGridData();
DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);

//Selecting and adding stock of the default item to save list and display list
if (category == "1") {
$.getJSON(DispersionPreparation.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + Item + "&DepartmentID=" + deptID, function (data1) {
$.getJSON(DispersionPreparation.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + data1[0].Value, function (data) {
if (data != null) {
var qtyStock = data[0].SBD_QTY_IN_STOCK;
for (var i in DispersionPreparation.DispersionSaveList) {
//                        if (Itmgroup == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
//                            DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = data1[0].Value;
DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = qtyStock;
//                        }
//                    }
//                    for (var i in DispersionPreparation.DispertionList) {
//                        if (Itmgroup == DispersionPreparation.DispertionList[i].DTD_MULT_BTCH_GRP) {
//                            DispersionPreparation.DispertionList[i].DSD_STK_BATCH = data1[0].Value;
//                            DispersionPreparation.DispertionList[i].QTY_IN_STOCK = qtyStock;
//                            DispersionPreparation.DispertionList[i].ITM_CUR_STK = qtyStock;
//                        }
}

$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
highliteStock();
}
});
});
}
else {
dtlPK = GrandGrid.Utilities.GetColumnValue(tr, "DSD_PK", "grdDispersionDetails");
if (dtlPK == "undefined") {
dtlPK = 0;
}

if ($("[id$=DTH_DISPERSION]").val() == null) {
cmpPK = DispersionPreparation.DTH_DISPERSION;
}
else {
cmpPK = $("[id$=DTH_DISPERSION]").val();
}
$.getJSON(DispersionPreparation.GetBatches + category + "&ItemID=" + Item + "&BatchPK=0&CompPK=" + cmpPK + "&CompDtlPK=" + dtlPK, function (data1) {
$.getJSON(DispersionPreparation.FillDispersionBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&active=2&BatchID=" + data1[0].Value, function (data) {
if (data != null) {
var qtyStock = data[0].DTH_QTY_BALANCE;
for (var i in DispersionPreparation.DispersionSaveList) {
//if (Itmgroup == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
//DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = data1[0].Value;
//DispersionPreparation.DispersionSaveList[i].DTD_BATCH = data1[0].Value;
DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = qtyStock;
// }
//}
//for (var i in DispersionPreparation.DispertionList) {
//if (Itmgroup == DispersionPreparation.DispertionList[i].DTD_MULT_BTCH_GRP) {
// DispersionPreparation.DispertionList[i].DSD_STK_DISP_BATCH = data1[0].Value;
//DispersionPreparation.DispertionList[i].DTD_BATCH = data1[0].Value;
//DispersionPreparation.DispertionList[i].QTY_IN_STOCK = qtyStock;
//DispersionPreparation.DispertionList[i].ITM_CUR_STK = qtyStock;
//}
}
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
highliteStock();
}
});
});
}
}



// END MULTIPLEBATCH SECTION//
*/
/*
function ViewRawMaterialDetails(trxPK) {
///<summary>Function used bind the inspection grid</summary> 

$.getJSON(DispersionPreparation.GetRawMaterialInspectionDetails + trxPK,
function (returnData) {
var rawMaterialObj = returnData;
$("[id$=lblRawDate]").html(rawMaterialObj.TIH_DATE_TEXT);
$("[id$=lblTestNo]").html(rawMaterialObj.TIH_NO);
$("[id$=lblTestReport]").html(rawMaterialObj.TIH_TEST_REPORT);
$("[id$=lblDoneBy]").html(rawMaterialObj.TIH_DONE_BY);
$("[id$=lblTestConductedAt]").html(rawMaterialObj.TIH_CONDUCTED_TEXT);
$("[id$=lblBatchType]").html(rawMaterialObj.TIH_BATCH_TYPE_TEXT);
$("[id$=lblLotBatch]").html(rawMaterialObj.TIH_LOT_BATCH);
$("[id$=lblMaterial]").html(rawMaterialObj.TIH_ITEM_TEXT);
$("[id$=lblLotSize]").html(rawMaterialObj.TIH_LOT_SIZE);
$("[id$=lblTest]").html(rawMaterialObj.TIH_TEST_TEXT);
$("[id$=lblSampleTaken]").html(rawMaterialObj.TIH_SAMPLE_TAKEN);
$("[id$=lblSampleSize]").html(rawMaterialObj.TIH_SAMPLE_SIZE + " " + rawMaterialObj.TIH_SAMPLE_SIZE_UOM_TEXT);
if (!$.isArray(rawMaterialObj.Detail)) {
rawMaterialObj.Detail = [rawMaterialObj.Detail];
}
GrandGrid.MakeGrid($("#grdRawMaterialInspection"), 0, rawMaterialObj.Detail);
});
$("#divRawMaterialInspection").dialog("open");
}
*/
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
        minValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TID_MIN_VALUE, grdID));
        maxValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TID_MAX_VALUE, grdID));
        observedValue = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), DispersionPreparation.TID_VALUE, grdID));
        stdIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TID_STD_VALUE, grdID);
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
            observedIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TID_VALUE, grdID);
            if (observedValue == "0") {
                $(this).find("td:eq(" + observedIndex + ")").html("No");
            }
            else {
                $(this).find("td:eq(" + observedIndex + ")").html("Yes");
            }
        }
        varianceIndex = GrandGrid.Utilities.GetColumnIndex($(this), DispersionPreparation.TID_VARIANCE, grdID);
        $(this).find("td:eq(" + varianceIndex + ")").html(varience);
    });
}


function FillCategoryDetails(categoryID) {
    //<summary>function To Fill Category Details and uom using categoryid </summary>
    //<Params>categoryID</Params>
    // FillUOM(categoryID, false);
    ClearOnMaterialCategoryDropChange();
    if (categoryID != "0") {
       // FillCategoryMaterials(categoryID);
    $("[id$=MaterialCategoryPK]").val(0); 
    FillMaterialCategoryAutoComplete();    
    }
}
function ClearOnMaterialCategoryDropChange() {
    //var drpItemID = $("select[id$=BatchNo]").attr("id");
    //var drpUomID = $("select[id$=DSD_QTY_UOM]").attr("id");
    //GrandScriptUtils.FillDropDown(drpItemID, null, true, true);
    //GrandScriptUtils.FillDropDown(drpUomID, null, true, true);
    $("[id$=DSD_QTY_UOM]").val(0);
    $("[id$=DSD_QTY_UOM_TEXT]").html("");
    $("[id$=MaterialName]").html("");
    $("[id$=DSD_QUANTITY]").val("");
    $("[id$=ITM_CUR_STK]").text("");
   // $("select[id$=DSD_ITEM]").val(DispersionPreparation.ValueZero);   
    $("[id$=MaterialPK]").val(0);     
    FillMaterialAutoComplete();
    //$("[id$=DTD_ACTUAL_TSC]").val("");

    //$("#tdPercentage").html("");
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
    $.getJSON(DispersionPreparation.GetMaterialByCategory + $("[id$=BizUnitPk]").val() + "&CategoryID=" + categoryID, function (data) {
        if (materialID) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, materialID);
        }
        else {
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
        }
    });
}

/*function FillMeterialBatchNo() {
///<summary>Function used Fill Batch No</summary>
/// <param name="categoryID"  type="object">
///     Specific categoryid to fill corresponding Material
/// </param>
/// <param name="materialID"  type="object">
///     Specific materialID to select the dropdown item after filling drop down
/// </param>  
var drpID = $("select[id$=BatchNo]").attr("id");
var itemID = $("[id$=DSD_ITEM]").val();
$.getJSON(DispersionPreparation.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + itemID + "&DepartmentID=" + deptID, function (data) {
GrandScriptUtils.FillDropDown(drpID, data, true, true);
});
}*/
/*
function MaterialChangeEvent() {
///<summary>Event triggered on Material change </summary>
//    if ($("[id$=Material]").val() != "0") {
//        // call
//        FillMeterialBatchNo();
//        FillMaterialUOMs($("[id$=DSD_ITEM]").val());
//    }
//    return false;

var categoryID = $("[id$=MaterialCatagory]").val();
var materialID = $("[id$=DSD_ITEM] option:selected").val();
//GrandScriptUtils.FillDropDown($("[id$=BatchNo]").attr("id"), new Object(), true, true);
if (materialID == 0) {
return false;
}

if (categoryID == 1)
FillMeterialBatchNo();
if (categoryID > 1 && materialID != 0) {
FillBatchNo($("[id$=BatchNo]").attr("id"), categoryID, materialID);
}
else if (categoryID == 1 && materialID != 0) {
//GetMaterialDetails(categoryID, materialID);
$.get(DispersionPreparation.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
if (data) {
//$("[id$=DTD_ACTUAL_TSC]").val(data[0].ITM_TSC);
$("[id$=DSD_QTY_UOM_TEXT]").html(data[0].UOM_CODE);
$("[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);
}
});
}
//FillMaterialUOMs($("[id$=DSD_ITEM]").val());

}*/

function MaterialChangeEvent() {
    ///<summary>Event triggered on Material change </summary>
    if ($("[id$=MaterialPK]").val() != "0") {
        FillMaterialDetails($("[id$=MaterialPK]").val());
    }
    return false;
}
function FillMaterialDetails(materialID, materialUOM) {
    ///<summary>Fill Material quantity UOM</summary>
    //fill UOM of Dispersion quantity
    var categoryID = $("[id$=MaterialCategoryPK]").val();

    var selectedUOM = "";
    // var materialID = $("[id$=DSD_ITEM]").val();
    var editProduct = $("input[id$=EditProduct]").val();
    var drpUOMID = $("select[id$=DSD_QTY_UOM]").attr("id");
    var drpID = $("select[id$=MRD_UOM]").attr("id");
    $.get(DispersionPreparation.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&Dept="+$("[id$=hdfDeptID]").val(), function (data) {
        if (data) {
            if (materialID != 0) {
                $("[id$=MaterialName]").html(data[0].ITM_NAME);
                $("[id$=DSD_QTY_UOM_TEXT]").html(data[0].UOM_CODE);
                $("[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);
                FillMaterialCategoryAutoComplete();
                $("[id$=MaterialCategoryPK]").val(data[0].ITM_CATEGORY);
                $("[id$=MaterialCatagory]").val(data[0].ITC_NAME);
                var stock=0;
                if(data[0].ITM_CUR_STK != null && data[0].ITM_CUR_STK != '')
                    stock=parseFloat(data[0].ITM_CUR_STK).toFixed(dispersionDecimal);
                else
                    stock=0;
                $("[id$=ITM_CUR_STK]").html(parseFloat(data[0].ITM_CUR_STK).toFixed(dispersionDecimal));
            }
            else {
                $("[id$=MaterialName]").html("");
                $("[id$=DSD_QTY_UOM]").val("0");
                $("[id$=DSD_QTY_UOM_TEXT]").html("");
                $("[id$=ITM_CUR_STK]").html('0');
            }

        }

    });
}
//function AddQtyInputs() {
//    var batchDropdown = $("#grdDispersionDetails tr:eq(1) td:eq(2)").clone();
//    var qtyTextbx = $("#grdDispersionDetails tr:eq(1) td:eq(4)").clone();
//    var catagory = 0;
//    var batchColIndex = 0;
//    var qtyIndex = 0;
//    var colIndex = 0;
//    DispersionPreparation.DispertionList = $("#divData").data("MaterialData");

//    $("#grdDispersionDetails tr:has(td)").each(function (index) {
//        if (index > 0) {
//            batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_BATCH_NO", "grdDispersionDetails");
//            catagory = GrandGrid.Utilities.GetColumnValue($(this), "DTD_ITEM_TYPE", "grdDispersionDetails");
//            qtyIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QUANTITY", "grdDispersionDetails");
//            $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
//            $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id", (index - 1) + "_BatchNo");
//            drpUOMID = $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id");
//            // GrandScriptUtils.FillDropDown(drpUOMID, CompoundJson.Materials[index - 1].ItemDetails, true, true); 
//            //var curStock = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_CUR_STK", "grdDispersionDetails");
//            //var curStock = $(this).find("td:eq(" + stockIndex + ")").html();
//            //Edit
//            if (catagory == 2 || catagory == 3) {
//                $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
//                $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id", (index - 1) + "_BatchNo");
//                drpUOMID = $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id");
//                // GrandScriptUtils.FillDropDown(drpUOMID, DispersionPreparation.DispertionList[index - 1].ItemDetails, true, true);  
//                var batch = 0;
//                if (DispersionPreparation.DispertionList.length > 0) {
//                    if (DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH == undefined) {
//                        batch = DispersionPreparation.DispertionList[index - 1].DSD_STK_BATCH;
//                    }
//                    else {
//                        batch = DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH;
//                    }

//                    FillBatchNo(drpUOMID, DispersionPreparation.DispertionList[index - 1].DTD_ITEM_TYPE, DispersionPreparation.DispertionList[index - 1].DSD_ITEM, batch, index - 1, true, $(this), DispersionPreparation.DispertionList[index - 1]);
//                    $("#" + drpUOMID).show();
//                }
//            }
//            else if (catagory == 1) {
//                $(this).find("td:eq(" + batchColIndex + ")").html(batchDropdown.html());
//                $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id", (index - 1) + "_BatchNo");
//                drpUOMID = $(this).find("td:eq(" + batchColIndex + ") [id$=BatchNo]").attr("id");
//                var batch = 0;
//                if (DispersionPreparation.DispertionList.length > 0) {
//                    if (DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH == undefined || DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH == "undefined") {
//                        batch = DispersionPreparation.DispertionList[index - 1].DSD_STK_BATCH;
//                    }
//                    else {
//                        batch = DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH;
//                    }

//                    FillBatchNo(drpUOMID, DispersionPreparation.DispertionList[index - 1].DTD_ITEM_TYPE, DispersionPreparation.DispertionList[index - 1].DSD_ITEM, batch, index - 1, true, $(this), DispersionPreparation.DispertionList[index - 1]);
//                    $("#" + drpUOMID).show();
//                }
//            }
//            else {
//                $(this).find("td:eq(" + batchColIndex + ")").html('');
//            }
//            //            if (catagory != null || catagory != " ") {
//            //                //$(this).find("td:eq(" + qtyIndex + ")").html(qtyTextbx.html());
//            //                $(this).find("[id$=DSD_QUANTITY]").live("change", function () {
//            //                    //  CaptureChanges();
//            //                });

//            //            }
//        }
//    });
//    $("#divData").data("MaterialData", DispersionPreparation.DispertionList);

//}

/*function FillBatchNo(drId, matCatId, matId, batchId, index, notIncludeSelect, currRow, materialsObj, currentBalance) {
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
ajaxUrl = DispersionPreparation.GetBatches + matCatId + "&ItemID=" + matId + "&BatchPK=" + batchID;
}
else {
var drpUOMID = drId;
if (currRow == undefined) {
dtlPK = 0;
}
else {
dtlPK = GrandGrid.Utilities.GetColumnValue(currRow, "DSD_PK", "grdDispersionDetails");
if (dtlPK == "undefined") {
dtlPK = 0;
}
}

if ($("[id$=DTH_DISPERSION]").val() == null) {
cmpPK = DispersionPreparation.DTH_DISPERSION;
}
else {
cmpPK = $("[id$=DTH_DISPERSION]").val();
}
if (matCatId > 1) {
$("#imgClearBatch_" + (parseInt(index) + 1)).hide();
// ajaxUrl = DispersionPreparation.GetBatches + matCatId + "&ItemID=" + matId + "&BatchPK=" + batchID + "&CompPK=" + cmpPK + "&CompDtlPK=" + dtlPK;
$.getJSON(DispersionPreparation.GetBatches + matCatId + "&ItemID=" + matId + "&BatchPK=" + batchID + "&CompPK=" + cmpPK + "&CompDtlPK=" + dtlPK, function (data) {
if (data != null && data.length > 0) {

if (batchId == null) {
if (notIncludeSelect && data.length > 0) {
GrandScriptUtils.FillDropDown(drpUOMID, data, true, false);
DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
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
else {
//if no data must include --select---
GrandScriptUtils.FillDropDown(drpUOMID, data, true, true);
}
//To hide MultiBatch add button without more than one ibatch in ddl
if (data != null && data.length > 1) {
$("#imgAddBatch_" + (parseInt(index) + 1)).show();
}
else {
$("#imgAddBatch_" + (parseInt(index) + 1)).hide();
}
//To disable batch ddl with multple batch selected
if (materialsObj != undefined && materialsObj.DTD_IS_MULT_BATCH != 0) {
$("#imgViewBatch_" + (parseInt(index) + 1)).show();
if (DisableBatchDDL(currRow, index)) {
$("[id$=" + parseInt(index) + "_BatchNo]").attr("disabled", "true");
$("[id$=" + parseInt(index) + "_BatchNo]").empty();
$("[id$=" + parseInt(index) + "_BatchNo]").append($("<option> </option>").val("-1").html("Multiple Batches"));
$("#imgClearBatch_" + (parseInt(index) + 1)).show();
}
else {
$("[id$=" + parseInt(index) + "_BatchNo]").attr("enabled", "true");
}
}
else {
$("#imgViewBatch_" + (parseInt(index) + 1)).hide();

}
});
}

if (matCatId == 1) {
$("#imgClearBatch_" + (parseInt(index) + 1)).hide();
$.getJSON(DispersionPreparation.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + matId + "&DepartmentID=" + $("[id$=hdfDeptID]").val() + "&BatchPK=" + batchID, function (data) {
if (data == null || data.length == 0)
GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, batchId);
else
GrandScriptUtils.FillDropDown(drpUOMID, data, true, false, batchId);
//To hide MultiBatch add button without more than one ibatch in ddl
if (data != null && data.length > 1) {
$("#imgAddBatch_" + (parseInt(index) + 1)).show();
}
else {
$("#imgAddBatch_" + (parseInt(index) + 1)).hide();
}
//To disable batch ddl with multple batch selected
if (materialsObj != undefined && materialsObj.DTD_IS_MULT_BATCH != 0) {
$("#imgViewBatch_" + (parseInt(index) + 1)).show();
if (DisableBatchDDL(currRow, index)) {
$("[id$=" + parseInt(index) + "_BatchNo]").attr("disabled", "true");
$("[id$=" + parseInt(index) + "_BatchNo]").empty();
$("[id$=" + parseInt(index) + "_BatchNo]").append($("<option> </option>").val("-1").html("Multiple Batches"));
$("#imgClearBatch_" + (parseInt(index) + 1)).show();
}
else {
$("[id$=" + parseInt(index) + "_BatchNo]").attr("enabled", "true");
}
}
else {
$("#imgViewBatch_" + (parseInt(index) + 1)).hide();

}
});
}
}

}

function SelectBatch() {
var isInOrder = true;
var objDisp = $("#divData").data("MaterialData");
$("#grdDispersionDetails tr:has(td)").each(function (index) {
if (index > 0) {
if ($(this).find("[id$=BatchNo]").val() == 0) {
isInOrder = false;
return isInOrder;
}
};
});
$("#divData").data("MaterialData", objDisp);
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
DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
$("#grdDispersionDetails tr:has(td)").each(function (index) {
if (index > 0) {
if (DispersionPreparation.DispertionList.length > 0) {
var material = DispersionPreparation.DispertionList[index - 1];
if (isNaN(parseFloat(material.ITM_CUR_STK))) {
material.ITM_CUR_STK = 0;
}
if (parseFloat(material.ITM_CUR_STK) < parseFloat($("#txtPreparationQuantity_" + index.toString()).val())) {
$("#txtPreparationQuantity_" + index.toString()).val(parseFloat(material.ITM_CUR_STK).toFixed(dispersionDecimal));
material.DSD_QUANTITY = material.ITM_CUR_STK;
}
else {
if ($("#txtPreparationQuantity_" + index.toString()).val() != "NaN") {
material.DSD_QUANTITY = $("#txtPreparationQuantity_" + index.toString()).val();
}
else {
$("#txtPreparationQuantity_" + index.toString()).val(material.DSD_QUANTITY);
}
}
total += parseFloat(material.DSD_QUANTITY);
$("[id$=DTH_QTY_PLANNED]").val(Math.round(total).toFixed(dispersionDecimal));
//if dispersion/compound capture  batch id (transaction iD)
if (material.DTD_ITEM_TYPE > 1) { //ie,Dispersion . (1 => Raw Material, 2=> Dispersion)
material.DTD_BATCH = $(this).find("[id$=BatchNo]").val();
material.DTD_STK_BATCH = undefined;
if (checkStock) {//Check Stock
$(this).find("[id$=MaterialQuantity]").css("border", "1px solid #7f9db9");
for (var i in material.BatchDetails) {
if (material.BatchDetails[i].Value == material.DTD_BATCH) {
if (parseFloat(material.BatchDetails[i].STOCK) < parseFloat(CompoundJson.Materials[index - 1].DTD_QUANTITY)) { //not enough stock
$(this).find("[id$=MaterialQuantity]").css("border", "1px solid red");
isStockInOrder = false;
}
}
}
}
}
else { //if raw material
material.DTD_STK_BATCH = $(this).find("[id$=BatchNo]").val();
material.DTD_BATCH = undefined;
if (checkStock) {
$(this).find("[id$=MaterialQuantity]").css("border", "1px solid #7f9db9");
if (CompoundJson.Materials[index - 1].DTD_DFT_ITEM) { //if  it is base quantity conversion factors mustbe checked
var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK);
var baseQty = CompoundJson.Materials[index - 1].DTD_QUANTITY;
}
else { ///if it is added materials no conversion factors
var baseStk = parseFloat(CompoundJson.Materials[index - 1].STOCK);
var baseQty = CompoundJson.Materials[index - 1].DTD_QUANTITY;
}
if (baseStk < baseQty) { //not enough stock
$(this).find("[id$=MaterialQuantity]").css("border", "1px solid red");
isStockInOrder = false;
}
}
}
}
}
});
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
return isStockInOrder;
}
*/
function FillMaterialUOMs(materialID, materialUOM) {
    ///<summary>Fill Material quantity UOM</summary>
    //fill UOM of Dispersion quantity
    var categoryID = $("[id$=MaterialCatagory]").val();

    var selectedUOM = "";
    // var materialID = $("[id$=DSD_ITEM]").val();
    var editProduct = $("input[id$=EditProduct]").val();
    var drpUOMID = $("select[id$=DSD_QTY_UOM]").attr("id");

    if (categoryID == "1") {
        $.get(DispersionPreparation.GETUOMTYPENAME + materialID, function (data) {
            if (data != " " && data != "-1") {
                //alert("Converted Factor  Value:" + data);
                if (parseInt(data) != 1) {
                    GrandScriptUtils.ShowModal(DispersionPreparation.MaterialCannotAdded, 'Translate(Information)');
                    return false;
                }
            }
        });
        $.get(DispersionPreparation.GETCONVERSIONUOM + $("[id$=DTH_QTY_UOM]").val(), function (data) {
            if (materialUOM == null) {
                GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, selectedUOM);
            }
            else {
                GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, materialUOM);
            }

        });



        var drpID = $("select[id$=MRD_UOM]").attr("id");
        $.get(DispersionPreparation.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
            if (data) {
                if (materialID != 0) {
                    $("[id$=MaterialName]").html(data[0].ITM_NAME);

                    if (editProduct > 0) {

                        GetConversionFactor($("[id$=DTH_QTY_UOM]").val(), materialUOM);

                    }
                    else {
                        selectedUOM = data[0].ITM_UOM;
                        $("select[id$=DSD_QTY_UOM]").val(data[0].ITM_UOM);
                        GetConversionFactor($("[id$=DTH_QTY_UOM]").val(), data[0].ITM_UOM);

                    }


                }
                else {
                    $("[id$=MaterialName]").html("");
                    $("select[id$=DSD_QTY_UOM]").val("0");
                }

            }

        });
    }
    else if (categoryID == "2") {
        if (editProduct > 0) {
            $.get(DispersionPreparation.GETCONVERSIONUOM + $("[id$=DTH_QTY_UOM]").val(), function (data) {
                if (materialUOM == null) {
                    GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, selectedUOM);
                }
                else {
                    GrandScriptUtils.FillDropDown(drpUOMID, data, true, true, materialUOM);
                }

            });
        }
        else {
            FillMaterialConversionUOM($("select[id$=DTH_QTY_UOM]").val());
        }
    }


    //    $("[id$=DSD_QTY_UOM").val("0");
}

function FillMaterialConversionUOM(selValue) {
    //<summary>Function used to Fill UOm Details With Selected UOM have COnversion Factor  </summary>
    var drpID = $("select[id$=DSD_QTY_UOM]").attr("id");
    $.get(DispersionPreparation.GetUOMListURL + $("select[id$=DTH_QTY_UOM]").val() + "&ItmPK=" + $("[id$=MaterialPK]").val() + "&Catg=" + $("[id$=MaterialCategoryPK]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        if (data) {
            if (data.length > 0) {
                //GetConversionFactor($("[id$=DTH_QTY_PLANNED_UOM]").val(), data[0].Value);
                GetConversionFactor($("[id$=DTH_QTY_UOM]").val(), data[0].Value);
            }
        }
    });
}

function GetConversionFactor(from, to, callBack) {
    ////<summary>method to get the conversion factor</summary>
    ////<param "from">From UOM PK </param>
    ////<param "to">To UOM PK </param>
    if (from == null) {
        from = $("[id$=DTH_QTY_UOM]").val();

    }
    if (to == null) {
        to = $("[id$=DSD_QTY_UOM]").val();
    }
    if (from == 0 || to == 0) {
        return false;
    }
    $.get(DispersionPreparation.GetConcversionFactors + from + "&UOMTo=" + to, function (data) {

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
            GrandScriptUtils.ShowModal(DispersionPreparation.SelectAnotherUOM, DispersionPreparation.ConfirmationMsg);
            // $("[id$=ImageButton1]").attr("disabled", "disabled");
            $("[id$=DSD_ITEM]").val("0");
            ConversionFactor = 1;
            return false;



        }
    });
}



function ValidateQuantity() {
    ///<summary>Validate quantity against stock</summary>
    /* var perc = CalculatePercentage()
    $('#tdPercentage').html((isNaN(perc) ? 0 : perc.toFixed(3)) + " / " + Round(percAvailable, 3)); //Round(perc,3) 
    if (Round(perc, 3) <= Round(percAvailable, 3) && perc != 0) {
    // $("[id$=ImageButton1]").attr("disabled", '');
    $("#tdPercentage").css({ "color": "#506c92", "visibility": "visible" });

    }
    else {
    //   $("[id$=ImageButton1]").attr("disabled", "disabled")
    $("#tdPercentage").css({ "color": "red", "visibility": "visible" });
    }*/
}

function CalculatePercentage() {
    ///<summary>method to calculate the material percentage
    if ($("input[id$=DTH_QUANTITY]").val() == "") {
        return 0;
    }
    if ($("input[id$=DSD_QUANTITY]").val() == "") {
        return 0;
    }
    return Round((parseFloat($("input[id$=DSD_QUANTITY]").val()) * 100) / (parseFloat($("input[id$=DTH_QUANTITY]").val()) * ConversionFactor), 5);
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
    // return x.toFixed(y);
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

    //if two values are significantly not equal and total quantity is greater than dispersion quantity
    if (!isEqual(totalQty, disprQty) && totalQty > disprQty) {

        GrandScriptUtils.ShowModal(DispersionPreparation.QuantityExceeded, DispersionPreparation.InformationTitle, DispersionPreparation.Unsuccess);
        return false;

    }
    else {//if not exceeded caluculate % based on actual quantity
        for (var i in objDisp.Materials) {
            //TotalQty += ObjDisp.Materials[i].QTY_BASE;
            objDisp.Materials[i].DSD_QTY_PERC = Round(objDisp.Materials[i].QTY_BASE == null ? 0 : objDisp.Materials[i].QTY_BASE * 100 / disprQty, 3);
        }
        //assign new quantiy to text box
        percAvailable = Round(100 - (totalQty * 100 / disprQty), 5);
    }

    //save object
    $("#divData").data("DispersionData", objDisp);
    ValidateQuantity();
    //bind grid
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, objDisp.Materials);
    //AddQtyInputs();
    return true;
}



/*
///#region data management

function FillBatchQuantity(containerRow) {
//<summary> ///Method to fill batch quantity into the quantity text box when batchindex chnages</summary>

var batchDropdown = $("#grdDispersionDetails tr:eq(1) td:eq(2)").clone();
var qtyTextbx = $("#grdDispersionDetails tr:eq(1) td:eq(4)").clone();
var catagory = 0;
var batchColIndex = 0;
var qtyIndex = 0;
var flag = true;
var stock = 0;
var quantity = 0;
var itemPK = GrandGrid.Utilities.GetColumnValue(containerRow, "DSD_ITEM", "grdDispersionDetails");  ///not working
var itemType = GrandGrid.Utilities.GetColumnValue(containerRow, "DTD_ITEM_TYPE", "grdDispersionDetails");
var slNO = GrandGrid.Utilities.GetColumnValue(containerRow, "SL_NO", "grdDispersionDetails");
var reqQuantity = GrandGrid.Utilities.GetColumnValue(containerRow, "DSD_QUANTITY", "grdDispersionDetails");


var flag = true;
if (containerRow) {
if (containerRow[0].rowIndex > 1) {
$("#grdDispersionDetails tr:has(td)").each(function (index) {
if (flag) {
if (index > 0 && index != containerRow[0].rowIndex - 1) {
batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_BATCH_NO", "grdDispersionDetails");
CatgID = GrandGrid.Utilities.GetColumnValue($(this), "DSD_ITEM_TYPE", "grdDispersionDetails");
drpUOMID = $(this).find("td:eq(" + batchColIndex + ") select").attr("id");
if (drpUOMID != undefined) {
if (containerRow.find("[id$=BatchNo]").val() == $("#" + drpUOMID).val()) {
containerRow.find("[id$=BatchNo]").val(GetOldValue(slNO, containerRow.find("[id$=BatchNo] option:first").val()));
flag = false;
if ((containerRow[0].rowIndex > 1) && flag == false) {
GrandScriptUtils.ShowModal(DispersionPreparation.BatchAlreadyAdded, DispersionPreparation.Information);
}
}
}
}
}
});
}
}

//If the request is from header
if (slNO == "") {
setBalanceReq();
}

DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
if (containerRow) {
if (containerRow[0].rowIndex > 1) {
$("#grdDispersionDetails tr:has(td)").each(function (index) {
if (flag) {
if (index == containerRow[0].rowIndex - 1) {
batchColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", "grdDispersionDetails");
CatgID = GrandGrid.Utilities.GetColumnValue($(this), "DSD_ITEM_TYPE", "grdDispersionDetails");
var batchID = containerRow.find("[id$=BatchNo]").val();
var stockIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "ITM_CUR_STK", "grdDispersionDetails");
//    var stockBatchIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "DSD_STK_BATCH", "grdDispersionDetails");
//  var stockDispBatchIndex = GrandGrid.Utilities.GetColumnIndex(containerRow, "DSD_STK_DISP_BATCH", "grdDispersionDetails");
var category = DispersionPreparation.DispertionList[index - 1].DSD_ITEM_TYPE;
if (category == "1") {
$.getJSON(DispersionPreparation.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
if (data != null) {
var qtyStock = data[0].SBD_QTY_IN_STOCK;
containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock);
// containerRow.find("td:eq(" + stockBatchIndex + ")").html(qtyStock);
DispersionPreparation.DispertionList[index - 1].DSD_STK_BATCH = batchID;
//DispersionPreparation.DispertionList[index - 1].DSD_QUANTITY = qtyStock;
DispersionPreparation.DispertionList[index - 1].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispertionList[index - 1].ITM_CUR_STK = qtyStock;
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
for (var i in DispersionPreparation.DispersionSaveList) {
if (DispersionPreparation.DispertionList[index - 1].DTD_MULT_BTCH_GRP == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = batchID;
DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = qtyStock;
}
}
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
ClearDispersionDetails();
highliteStock();
}
else {
containerRow.find("td:eq(" + stockIndex + ")").html("");
}
});
}
else {
$.getJSON(DispersionPreparation.FillDispersionBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&active=2&BatchID=" + batchID, function (data) {
if (data != null) {
var qtyStock = data[0].DTH_QTY_BALANCE;
containerRow.find("td:eq(" + stockIndex + ")").html(qtyStock.toFixed(dispersionDecimal));
DispersionPreparation.DispertionList[index - 1].DTD_BATCH = batchID;
DispersionPreparation.DispertionList[index - 1].DSD_STK_DISP_BATCH = batchID;
//DispersionPreparation.DispertionList[index - 1].DSD_QUANTITY = qtyStock;
DispersionPreparation.DispertionList[index - 1].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispertionList[index - 1].ITM_CUR_STK = qtyStock;
$("#divData").data("MaterialData", DispersionPreparation.DispertionList);
for (var i in DispersionPreparation.DispersionSaveList) {
if (DispersionPreparation.DispertionList[index - 1].DTD_MULT_BTCH_GRP == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
DispersionPreparation.DispersionSaveList[i].DTD_BATCH = batchID;
DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = batchID;
DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = qtyStock;
DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = qtyStock;
}
}
GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
//AddQtyInputs();
ClearDispersionDetails();
highliteStock();
}
else {
containerRow.find("td:eq(" + stockIndex + ")").html("");
}
});
}

}
}
});
}
}

}
*/
var OldValue = 0;

function GetOldValue(slNO, oldVal) {
    for (var i in DispersionPreparation.DispertionList) {
        if (DispersionPreparation.DispertionList[i].SL_NO == slNO) {
            if (DispersionPreparation.DispertionList[i].DSD_STK_BATCH == undefined) {
                return oldVal;
            }
            else {
                return DispersionPreparation.DispertionList[i].DSD_STK_BATCH;
            }
        }
    }
}

function setBalanceReq() {
    var balanceReq = 0;
    var batchID = $("[id$=BatchNo]").val();
    var category = $("[id$=MaterialCategoryPK]").val();
    if (category == "1") {
        $.getJSON(DispersionPreparation.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
            if (data != null) {
                var qtyStock = data[0].SBD_QTY_IN_STOCK;
                $("#ITM_CUR_STK").text(qtyStock.toFixed(dispersionDecimal));
            }
            else {
                $("#ITM_CUR_STK").text();
                //document.getElementById('<%= ITM_CUR_STK.ClientID %>').innerHTML = 0;
            }
        });
    }
    else {
        $.getJSON(DispersionPreparation.FillDispersionBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&active=2&BatchID=" + batchID, function (data) {
            if (data != null) {
                var qtyStock = data[0].DTH_QTY_BALANCE;
                $("#ITM_CUR_STK").text(qtyStock.toFixed(dispersionDecimal));
                $("[id$=DSD_QTY_UOM_TEXT]").html(data[0].DTH_QTY_UOM_TEXT);
                $("[id$=DSD_QTY_UOM]").val(data[0].DTH_QTY_UOM);
            }
            else {
                $("#ITM_CUR_STK").text();
                //document.getElementById('<%= ITM_CUR_STK.ClientID %>').innerHTML = 0;
            }
        });
    }
}


function AddDispersionMaterials(isValidated) {
    //<summary>function used to add Materials details to Dispersion</summary>
    //Add Validation for Material Details by setting mode as 2
     var curStock=0;
     var Qty=0;
    if($("[id$=ITM_CUR_STK").html() != null && $("[id$=ITM_CUR_STK").html() != '')
         curStock=parseFloat($("[id$=ITM_CUR_STK").html());
     if( $("[id$=DSD_QUANTITY]").val() != null &&  $("[id$=DSD_QUANTITY]").val() != '')
        Qty=parseFloat( $("[id$=DSD_QUANTITY]").val());
        
    if(curStock != 0 || curStock >= Qty)
    {
    var flag = true;
    /*for (var i in DispersionPreparation.DispertionList) {
    if ($("[id$=MaterialCatagory] option:selected").val() == 1) {
    if (DispersionPreparation.DispertionList[i].DSD_STK_BATCH == $("[id$=BatchNo] option:selected").val()) {
    flag = false;
    break;
    }
    }
    else {
    if (DispersionPreparation.DispertionList[i].DSD_STK_DISP_BATCH == $("[id$=BatchNo] option:selected").val()) {
    flag = false;
    break;
    }
    }
    }*/
    GetGridData();
    DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
    for (var i in DispersionPreparation.DispertionList) {
        if (DispersionPreparation.DispertionList[i].DSD_ITEM == $("[id$=MaterialPK]").val()) {
            flag = false;
            break;
        }
    }
    if (flag == true) {
        RemoveValidations();
        AddValidations(2);
        if ($(document.forms[0]).valid()) {
            var maxGroup = 0;
            //            for (var i in DispersionPreparation.DispersionSaveList) {
            //                if (maxGroup < DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP)
            //                    maxGroup = DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP;
            //            }
            //DispersionPreparation.DispertionList = $("#divData").data("MaterialData");
            //DispersionPreparation.DispersionSaveList = $("#divSaveData").data("SaveData");
            DispersionPreparation.DispertionObj = new Object();
            DispersionPreparation.DispertionObj.SL_NO = DispersionPreparation.DispertionList.length;
            DispersionPreparation.DispertionObj.DSD_PK = 0; //
            DispersionPreparation.DispertionObj.CONVERT_FACTOR = ConversionFactor; //$("[id$=CONVERT_FACTOR]").val();
            DispersionPreparation.DispertionObj.DSD_DISP = $("[id$=DSD_DISP] option:selected").text();
            DispersionPreparation.DispertionObj.DSD_ITEM = $("[id$=MaterialPK]").val();
            DispersionPreparation.DispertionObj.DSD_QTY_PERC = 0;//$("[id$=tdPercentage]").text().trim();
            DispersionPreparation.DispertionObj.DSD_QTY_UOM = $("[id$=DSD_QTY_UOM]").val(); //$("[id$=DSD_QTY_UOM] option:selected").val();
            DispersionPreparation.DispertionObj.DSD_QUANTITY = $("[id$=DSD_QUANTITY]").val();
            DispersionPreparation.DispertionObj.DSD_ITEM_TYPE_TEXT = $("[id$=MaterialCatagory]").val();
            DispersionPreparation.DispertionObj.DSD_ITEM_TYPE = $("[id$=MaterialCategoryPK]").val();
            DispersionPreparation.DispertionObj.ITM_CODE = $("[id$=DSD_ITEM]").val(); 
            DispersionPreparation.DispertionObj.ITM_CUR_STK = $("[id$=ITM_CUR_STK").html(); // $("[id$=ITM_CUR_STK").text();
            DispersionPreparation.DispertionObj.ITM_TEXT = $("[id$=DSD_ITEM]").val();
            DispersionPreparation.DispertionObj.QTY_IN_STOCK = $("[id$=DSD_QUANTITY]").val();
            DispersionPreparation.DispertionObj.UOM_CODE = $("[id$=DSD_QTY_UOM_TEXT]").html(); //$("[id$=DSD_QTY_UOM] option:selected").text();
            DispersionPreparation.DispertionObj.DSD_QUANTITY = $("[id$=DSD_QUANTITY]").val();
            //            if (DispersionPreparation.DispertionObj.DSD_ITEM_TYPE == 1)
            //                DispersionPreparation.DispertionObj.DSD_STK_BATCH = $("[id$=BatchNo] option:selected").val();
            //            else {
            //                DispersionPreparation.DispertionObj.DSD_STK_DISP_BATCH = $("[id$=BatchNo] option:selected").val();
            //                DispersionPreparation.DispertionObj.DTD_BATCH = $("[id$=BatchNo] option:selected").val();
            //            }

            DispersionPreparation.DispertionObj.DTD_ITEM_TYPE = $("[id$=MaterialCategoryPK]").val();
            //DispersionPreparation.DispertionObj.DTD_ACTUAL_TSC = $("[id$=DTD_ACTUAL_TSC]").val();
            //DispersionPreparation.DispertionObj.DTD_MULT_BTCH_GRP = parseInt(maxGroup) + 1;
            //DispersionPreparation.DispertionObj.DTD_IS_MULT_BATCH = 0;
            //DispersionPreparation.DispertionObj.BATCH_NO_TEXT = $("[id$=BatchNo] option:selected").text();
            DispersionPreparation.DispertionList.push(DispersionPreparation.DispertionObj);
            //DispersionPreparation.DispersionSaveList.push(DispersionPreparation.DispertionObj);
            //var totalQty = parseFloat($("[id$=DTH_QUANTITY]").val()) + parseFloat($("[id$=DSD_QUANTITY]").val());
            //$("[id$=DTH_QUANTITY]").val(totalQty.toFixed(dispersionDecimal));

            if (DispersionPreparation.DispertionObj.DSD_QTY_PERC == "0") {
                $(this).find("td:last input[id$=imbCalculate]").hide();
            }

            $("#divData").data("MaterialData", DispersionPreparation.DispertionList);
            //$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);
            GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, DispersionPreparation.DispertionList);
            //ShowFields();
            //AddQtyInputs();
            ClearDispersionDetails();
            highliteStock();
        }
    }
    else {
        GrandScriptUtils.ShowModal(DispersionPreparation.BatchAlreadyAdded, DispersionPreparation.MessageBoxTitle);
        }
    }
    else
     GrandScriptUtils.ShowModal(DispersionPreparation.NotEnoughStockExists, DispersionPreparation.MessageBoxTitle);     
     return false;
    }
    

    /*AddValidations(2);
    if ($(document.forms[0]).valid()) {
    dispersionObj = $("#divData").data("dispersionObj");
    //$("#divData").data("dispersionObj", dispersionObj);
    var editProduct = $("input[id$=EditProduct]").val();
    var obj = new Object(); 
    var flag = true;
    var perc = 0;

    //Loop used to check the Material already added in the order List
    if (isNaN(parseFloat($("input[id$=DSD_QUANTITY]").val())) || parseFloat($("input[id$=DSD_QUANTITY]").val()) == 0) {
    return false;
    }
    if (parseInt(editProduct) == 0) {
    for (var i in dispersionObj.MaterialList) { 
    if (dispersionObj.MaterialList[i].DSD_ITEM == $("select[id$=DSD_ITEM]").val()) { 
    flag = false;
    break;
    }
    }
    }
    else {
    for (var i in dispersionObj.MaterialList) {
    if (dispersionObj.MaterialList[i].DSD_ITEM == $("select[id$=DSD_ITEM]").val() && parseInt(editProduct) != dispersionObj.MaterialList[i].DSD_ITEM) {
    flag = false;
    break;
    }
    if (parseInt(editProduct) == dispersionObj.MaterialList[i].DSD_ITEM) {
    obj = dispersionObj.MaterialList[i];
    }
    }
    } 
    //add material to the list
    if (flag) { 
    obj.ITM_TEXT = $("[id$=DSD_ITEM] option:selected").text();
    obj.ITEM_CATAGORY = $("[id$=MaterialCatagory] option:selected").val();
    obj.CONVERT_FACTOR = 1;
    obj.DSD_PK = 0;
    obj.DSD_DISP = 0;
    obj.DSD_ITEM = $("select[id$=DSD_ITEM] option:selected").val();
    obj.QTY_IN_STOCK = 0;
    obj.DSD_QTY_UOM = 1;
    //obj.ITM_CUR_STK = $("select[id$=DSD_QTY_UOM] option:selected").text();
    // obj.DSD_QUANTITY = CalculatePercentage();
    obj.UOM_CODE = $("select[id$=DSD_QTY_UOM] option:selected").val();
    // obj.DSD_QTY_PERC = GetConverterdQuantity(); 
    if (parseInt(editProduct) == 0) {
    dispersionObj.MaterialList.push(obj); 
    }
    GrandGrid.MakeGrid($("#grdDispersionDetails"), 0, dispersionObj.MaterialList); 
    }
    else {
    GrandScriptUtils.ShowModal(DispersionPreparation.MaterialAlreadyAdded, DispersionPreparation.InformationTitle);
    }
    return false;
    }*/

function ClearMaterialDetails() {
    ///<Summary>Clear material input fields<summary>
//    FillMaterialAuto(0, 0);
//    $("select[id$=MaterialCatagory]").val('0');
    $("[id$=MaterialCategoryPK]").val(0);
    $("[id$=MaterialPK]").val(0); //$("select[id$=DSD_ITEM]").val('0');
    FillMaterialCategoryAutoComplete();
    FillMaterialAutoComplete();
   
}

function ClearDispersionDetails() {
    FillCategoryDetails(0);
    ClearMaterialDetails(); //FillMeterialBatchNo();
    // FillUOMs();
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for grdDispersionDetails Catch all the grid events in this function </summary>
    ///<param "tr">Current row jquery object</param>
    ///<param "command">command to be processed</param> 
    //RemoveValidations();
    switch (command.toString().toLowerCase()) {
        case "delete":
            //            deleteMaterialPk = GrandGrid.Utilities.GetColumnValue(tr, "DSD_ITEM", "grdDispersionDetails");
            //            GrandScriptUtils.ShowModal(DispersionPreparation.DoUWantToDelMsg, DispersionPreparation.ConfirmationMsg, DispersionPreparation.DeleteMaterial, true);
            //DeleteDetails(tr); 
            slNO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdDispersionDetails"); //27/10/11 Uncommented  
            //GridItemGroup = GrandGrid.Utilities.GetColumnValue(tr, "DTD_MULT_BTCH_GRP", "grdDispersionDetails");
            GrandScriptUtils.ShowModal(DispersionPreparation.DeleteConfirmation, DispersionPreparation.ConfirmationMessage, "DeleteDetail", true);
            return false;
            break;
        case "calculate":
            var totalqty = 0.0;
            slNO = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdDispersionDetails");
            var qty = $("#txtPreparationQuantity_" + (parseInt(slNO) + 1).toString()).val();
            if (qty != "" && qty != 'undefined' && !isNaN(qty)) {
                var qtyPerc = GrandGrid.Utilities.GetColumnValue(tr, DispersionPreparation.DSD_QTY_PERC, "grdDispersionDetails");
                if (qty != undefined) {
                    var tempQty = parseFloat((qty * 100) / qtyPerc);
                    totalqty = (Math.round(tempQty * 100) / 100).toFixed(dispersionDecimal);
                    //totalqty = Math.round(tempQty).toFixed(dispersionDecimal);
                    $("[id$=DTH_QUANTITY]").val(totalqty);
                }
                calcFlag = 1;
                CalcPercQuantity(totalqty, slNO);
                GetGridData();
                highliteStock();
            }
            return false;
            break;
        /*case "multiplebatch":
            MultipleBatchPopup(tr);
            return false;
            break;
        case "clearmultiplebatch":
            ClearMultipleBatch(tr);
            break;*/
        default:
            alert(DispersionPreparation.DefaultActionMsg);
            return false;
            break;
    }
}
/*
function PopUpGridHandler(tr, command) {
///<summary>Grid Handler for grdDispersionDetails Catch all the grid events in this function </summary>
///<param "tr">Current row jquery object</param>
///<param "command">command to be processed</param> 
//RemoveValidations();
switch (command.toString().toLowerCase()) {
case "delete":
var DelQty = 0;
slNO = GrandGrid.Utilities.GetColumnValue(tr, "SLNO", "grdBatchDetails");
$("#divPopupLatexBatches").data("BatchData", DispersionPreparation.PopUpBatchList);
for (var i in DispersionPreparation.PopUpBatchList) {
if ((DispersionPreparation.PopUpBatchList[i].SLNO == slNO)) {
DelQty = DispersionPreparation.PopUpBatchList[i].DSD_QUANTITY;
DispersionPreparation.PopUpBatchList.splice(i, 1);
break;
}
}
for (var i in DispersionPreparation.PopUpBatchList) {
DispersionPreparation.PopUpBatchList[i].SLNO = parseInt(i) + 1;
}
$("#divPopupLatexBatches").data("BatchData", DispersionPreparation.PopUpBatchList);
GrandGrid.MakeGrid($("#grdBatchDetails"), 0, DispersionPreparation.PopUpBatchList);
PopUpBatchQty = parseFloat(PopUpBatchQty) - parseFloat(DelQty);
$("[id$=lblTotalBatchQty]").html(PopUpBatchQty.toFixed(dispersionDecimal));
//$("[id$=lblQtyRequired]").html(ItmQty);
var balRequired = parseFloat($("[id$=lblQtyRequired]").html()) - parseFloat(PopUpBatchQty).toFixed(dispersionDecimal);
if (parseFloat(balRequired) < 0) {
balRequired = "-";
$("[id$=lblQtyBal]").html(balRequired);
$("[id$=txtLatexQtyPopUp]").val(balRequired);
}
else {
$("[id$=lblQtyBal]").html(balRequired.toFixed(dispersionDecimal));
$("[id$=txtLatexQtyPopUp]").val(balRequired.toFixed(dispersionDecimal));
}
return false;
break;
}
}
//function HideCheckList() {
//    //<summary>Function Used to Hide Reorder Panel </summary>

//    $("#imgChecklistHide").hide();
//    $("#imgCheckListShow").show();
//    $("#divgrdCheckListInner").hide();
//}

//function ShowCheckList() {
//    //<summary>Function Used to Show Reorder Panel </summary>

//    $("#imgChecklistHide").show();
//    $("#imgCheckListShow").hide();
//    $("#divgrdCheckListInner").show();
//}
*/
function ShowHideAdvancedSearch(flag) {
    //If flag then Show AdvancedSearch
    if (flag) {
        $("[id$=tblChklstAdd]").show();
        $("[id$=imbShowFilter]").hide();
        $("[id$=imbHideFilter]").show();
        $("[id$=hdfShowHideFilter]").val("0");
    }
    else {
        $("[id$=tblChklstAdd]").hide();
        $("[id$=imbShowFilter]").show();
        $("[id$=imbHideFilter]").hide();
        $("[id$=hdfShowHideFilter]").val("1");
    }
    //clearChkTmplInsertBlock();
    return false;
}

function GetGridData() {
    var colIndex = 0;
    var ColValue = "";
    DispersionPreparation.DispertionList = $("#divData").data("MaterialData");

    // DispersionPreparation.DispersionSaveList = new Array();

    $("#grdDispersionDetails tr:has(td)").each(function (index) {
        if (index > 0) {
            for (var i in DispersionPreparation.DispertionList) {
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SL_NO", $(this).parents("table:first").attr("id"));
                ColValue = GrandGrid.Utilities.GetColumnValue($(this), "SL_NO", $(this).parents("table:first").attr("id"));
                if (ColValue == DispersionPreparation.DispertionList[i].SL_NO) {
                    /*var Itmgroup = GrandGrid.Utilities.GetColumnValue($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
                    if (Itmgroup == DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP) {
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
                    if (ColValue == "0") {
                    //DispersionPreparation.DispertionObj = new Object();

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "CONVERT_FACTOR", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "CONVERT_FACTOR", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].CONVERT_FACTOR = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].CONVERT_FACTOR = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_DISP", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_DISP", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_DISP = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_DISP = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_ITEM_TYPE", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_ITEM_TYPE", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DTD_ITEM_TYPE = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DTD_ITEM_TYPE = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_ITEM", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_ITEM", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_ITEM_TYPE", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_ITEM_TYPE", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE = ColValue;
                    }
                    }*/

                    /*   Type = ColValue;
                    //***************************
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_STK_BATCH", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_STK_BATCH", $(this).parents("table:first").attr("id"));
                    if (Type == 1) {

                    DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = ColValue;
                    DispersionPreparation.DispersionSaveList[i].DTD_BATCH = null;
                    DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = null;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = null;
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_STK_DISP_BATCH", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_STK_DISP_BATCH", $(this).parents("table:first").attr("id"));
                    if (colIndex != null && Type == 2) {
                    DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = ColValue;
                    DispersionPreparation.DispersionSaveList[i].DSD_STK_BATCH = null;
                    DispersionPreparation.DispersionSaveList[i].DTD_BATCH = ColValue;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_STK_DISP_BATCH = null;
                    }

                    //****************************
                    */


                    /*colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_ITEM_TYPE_TEXT", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_ITEM_TYPE_TEXT", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE_TEXT = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_ITEM_TYPE_TEXT = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_PK", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_PK", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_PK = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_PK = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QTY_PERC", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_QTY_PERC = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_QTY_PERC = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QTY_UOM", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QTY_UOM", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DSD_QTY_UOM = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DSD_QTY_UOM = ColValue;
                    }
                    }
                    */
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DSD_QUANTITY", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                        DispersionPreparation.DispertionList[i].DSD_QUANTITY = $("#txtPreparationQuantity_" + (parseInt(index)).toString()).val();
                    }


                    /*colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_ACTUAL_TSC", $(this).parents("table:first").attr("id"));
                    //ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_ACTUAL_TSC", $(this).parents("table:first").attr("id"));DSD_STK_DISP_BATCH = "238"
                    if (colIndex != null) {
                    DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC = $("#txtActualTSC_" + (parseInt(index)).toString()).val();
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC = "";
                    }
               
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_CUR_STK", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "ITM_CUR_STK", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].ITM_CUR_STK = ReplaceCommas(ColValue);
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_PHR", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "ITM_PHR", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].ITM_PHR = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].ITM_PHR = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "ITM_TEXT", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "ITM_TEXT", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].ITM_TEXT = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].ITM_TEXT = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "QTY_IN_STOCK", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "QTY_IN_STOCK", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = 0;
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].QTY_IN_STOCK = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SL_NO", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "SL_NO", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].SL_NO = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].SL_NO = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SLNO", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "SLNO", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].SLNO = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].SLNO = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_CODE", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "UOM_CODE", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].UOM_CODE = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].UOM_CODE = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "UOM_NAME", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "UOM_NAME", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].UOM_NAME = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].UOM_NAME = ColValue;
                    }
                    }
                    */
                    /*colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_IS_MULT_BATCH", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DTD_IS_MULT_BATCH = ColValue;
                    }
                    }


                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
                    ColValue = GrandGrid.Utilities.GetColumnValue($(this), "DTD_MULT_BTCH_GRP", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    if (ColValue == 'undefined' || ColValue == "null" || ColValue == "") {
                    DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP = "";
                    }
                    else {
                    DispersionPreparation.DispersionSaveList[i].DTD_MULT_BTCH_GRP = ColValue;
                    }
                    }

                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "IMG", $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                    DispersionPreparation.DispersionSaveList[i].IMG = "";
                    }
                    }
                    //                    else {
                    // DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY = $("#txtPreparationQuantity_" + (parseInt(index)).toString()).val();
                    DispersionPreparation.DispersionSaveList[i].DSD_QUANTITY_TEMP = $("#txtPreparationQuantity_" + (parseInt(index)).toString()).val();
                    DispersionPreparation.DispersionSaveList[i].DTD_ACTUAL_TSC = $("#txtActualTSC_" + (parseInt(index)).toString()).val();
                    */
                }
            }
            //            }
        }
    });
    //$("#divSaveData").data("SaveData", DispersionPreparation.DispersionSaveList);
    //GetDisplayGridData();
}

function highliteStock() {
    $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
        $(this).closest('tr').removeClass('highlight');
    });
    var stockExcists = true;
    for (var i in DispersionPreparation.DispertionList) {
        //        var IsmultiBatch = DispersionPreparation.DispertionList[i].DTD_IS_MULT_BATCH;
        //        if (IsmultiBatch == "0") {
        quantity = DispersionPreparation.DispertionList[i].DSD_QUANTITY;
        if (parseFloat(quantity) > parseFloat(DispersionPreparation.DispertionList[i].ITM_CUR_STK)) {
            stockExcists = false;

            $("#grdDispersionDetails").find("tr:has(td)").each(function (index) {
                if (index == (parseInt(i) + 1)) {
                    if (stockExcists == false) {
                        var selectedRowColor;
                        selectedRowColor = '#F9DEE5';
                        $(this).closest('tr').addClass('highlight');
                    }
                }
            });
        }
    }
    //    }
}

//Comma Separation for Quantity & Amount  
function addCommasForNumeric(number) {
    var FormattedNumber = number;
    var curGroup1 = 3;
    var curGroup2 = 3;
    var NumericPart = "", LastNumericPart = "", DecimalPart = "";

    DecimalPart = number.split('.')[1];
    (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
    NumericPart = number.split('.')[0];
    if (NumericPart.length > curGroup1) {
        LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
        (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
    }
    if ((NumericPart.length - curGroup1) > 0) {
        NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
        var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
        var expression = new RegExp(pattern, "g");
        NumericPart = NumericPart.toString().replace(expression, ",");
    }
    FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
    return FormattedNumber;
}

function ReplaceCommas(num) {
    return num.replace(/,/g, "");
    //return num.replace(/[^0-9\.]+/g, "");
}

function CancelDispersion() {
    GrandScriptUtils.ShowModal(DispersionPreparation.CANCELCONFIRMMSG, DispersionPreparation.CONFIRMMSG, DispersionPreparation.CANCELDISP, true);
    return false;
}

function CancelDispersionDetails() {
    ///<summary>Delete Designaion Details </summary>

    var msgtxt;
    $.get(DispersionPreparation.DeleteDispersionPreparationURL + $("[id$=hdfDispPK]").val(), function (data) {
        if (parseInt(data) == 1)//Check  Deleted Succesfully or Not - 1-Sucess 0-Fail
            msgtxt = DispersionPreparation.CANCELSUCESS;
        else if (parseInt(data) == 0)
            msgtxt = "Assigned";
        else if (parseInt(data) == -10)
            msgtxt = DispersionPreparation.CANCELFAILED;
        else
            msgtxt = DispersionPreparation.ActionFailedMessage;
        GrandScriptUtils.ShowModal(msgtxt, DispersionPreparation.MessageBoxTitle, DispersionPreparation.SAVE);
    });
    return false;
}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("MaterialCatagory", DispersionPreparation.MaterialCategroyDeptURL + "&Type=1", "MaterialCategoryPK", true, false, "BizUnitPk", true);
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
    //    GrandScriptUtils.MakeAutoComplete("DSD_ITEM", DispersionPreparation.MaterialURL + "&SearchValue=" + SearchVal, "MaterialPK", true, false, "MaterialCategoryPK", true);
    GrandScriptUtils.MakeAutoComplete("DSD_ITEM", DispersionPreparation.MaterialURL, "MaterialPK", true, false, "MaterialCategoryPK", true);
}

/*
function FillPopUpBatchQuantity(containerRow) {
DispersionPreparation.PopUpBatchList = $("#divPopupLatexBatches").data("BatchData");
var category = $("[id$=hdfItmType]").val(); //DispersionPreparation.PopUpBatchList[0].DSD_ITEM_TYPE;
if (category == "1") {
$.getJSON(DispersionPreparation.FillBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&BatchID=" + $("[id$=ddlLatexBatchesPopUp] option:selected").val(), function (data) {
if (data != null) {
$("[id$=lblLatexStockPopUp]").html(parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(dispersionDecimal));
}
});
}
else {
$.getJSON(DispersionPreparation.FillDispersionBatchDetailGetURL + $("[id$=BizUnitPk]").val() + "&active=2&BatchID=" + $("[id$=ddlLatexBatchesPopUp] option:selected").val(), function (data) {
if (data != null) {
$("[id$=lblLatexStockPopUp]").html(parseFloat(data[0].DTH_QTY_BALANCE).toFixed(dispersionDecimal));
}
});
}
}*/