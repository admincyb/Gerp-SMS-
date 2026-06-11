/// <reference path="../../GrandScriptUtils.js" />

///#region=============== Global variable Declaration
var TopUpJson = new Object();
var tdset = "";
var slno = 0;
var itmPk = 0;

///#endregion

///#region=============== Configurations
var TopUp = {

    CHECKSTOCKAVAILABLEURL: "TopUpManagement.do?Action=CheckStock&ItemPk=",
    SAVEPAGEURL: "TopUpManagement.do?Action=SavePage",
    GETSHIFTDETAILSURL: "CommonManagement.do?Action=GetShift",
    FILLPLANURL: "CommonManagement.do?Action=GetPlans",
    FILLTANKTYPEURL: "TankManagement.do?Action=GetTankType&SBUPk=",
    FILLTOPUPDTLLSURL: "TopUpManagement.do?Action=GetTopUpDtls&PK=",
    FILLUOMLISTWITHCONVERSIONFACTOR: "TopUpManagement.do?Action=GetUOMList&TankPk=",
    FILLMATERIALNAMEURL: "TopUpManagement.do?Action=GetItemNameName&SBU=",
    FILLBATCHNUMBERURL: "CompoundPreparation.do?Action=GetBatchesForItem&ItemID=",
    FILLTANKNAMEURL: "TankManagement.do?Action=GetTankName&SBUPk=",

    FILLMATERIALUOM: "CompoundMaster.do?Action=GetMaterialUOMDtls&ItemID=",

    InformationTitle: "Translate(Information)",

    ItemAlreadyAdded: "Translate(Itemalreadyadded)",
    STOCKNOTAVAILABLE: "Translate(Stocknotavailable)",
    TOPUPLISTINGURL: "TopUpRecordListing.aspx",
    SAVE: "save",
    DELETE: "delete",
    EDIT: "edit",
    CONFIRMATION: "Translate(Confirmation)",
    DOUWANTTODELETEMSG: "Translate(Doyouwanttodeletethisdetails)",
    ACTIONFAILED: "Translate(ActionFailedPleaseTryAgain)",
    ValueZero: "0",
    ValueEmpty: "",
    TOPUPSAVEMSG: "Translate(TopUpDetailssavedSuccessfully)",
    DEFAULTACTION: "Translate(DefaultActionneedstobeperformed)",
    QUANTITYNOTAVAILABLE: "Translate(Quanatitynotavailableinstock)",
    ADDMATERIAL: "Translate(PleaseAddItemDetails)"



};
///#endregion

///#region=============== Initialization Section

//For Adding rule to Select
$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != TopUp.ValueZero);
}, 'Translate(Pleaseselectanoption)');
///<summary>Document read</summary>
$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });

    TopUpJson = $.parseJSON($("[id$=ItemList]").val());
    $("#divDatas").data("TopUpData", TopUpJson);
    GrandGrid.Utilities.ResetGrid(true, "grdTopUpMaterial");
    TopUpJson.ItemList = new Array();
    GrandGrid.MakeGrid($("#grdTopUpMaterial"), 0, TopUpJson.ItemList);
    PageInit();
    $("[id$=BatchNo]").hide();
});

function PageInit() {
    ///<summary>Used for initial settings</summary> 
    //$("[id$=TUH_SHIFT_NAME]").focus();
    FillTopUp();
    $("[id$=TUD_BATCH]").hide();
}

function DateTimeInit() {
    //<summary>Function used to Init Date and Time Extender to the control</summary>
    $("[id$=TUH_TIME]").timepicker();
    GrandScriptUtils.DatePicker("TUH_DATE", false, false);
}

///#endregion

///#region=============== Core Section
function CheckStockAvailable() {
    //<summary>Function  used to check stock available or not  </summary>
    $.get(TopUp.CHECKSTOCKAVAILABLEURL + $("select[id$=TUD_ITEM]").val() + "&CatgPk=" + $("select[id$=TUD_ITEM_TYPE]").val() + "&Qty=" + $("input[id$=TUD_QUANTITY]").val() + "&ToUom=" + $("select[id$=TUD_QTY_UOM]").val() + "&DtlsPK=" + $("input[id$=TUD_PK]").val(), function (data) {
        //alert(data);
        if (data == "1")
            return true;

        else

            return false;
    });
}


function AddMaterialDetails() {
    //<summary>Function used to Add Material Details  </summary>
    RemoveValidations(2);
    var stockStatus;
    AddValidations(2);
    if ($(document.forms[0]).valid()) {
        $.get(TopUp.CHECKSTOCKAVAILABLEURL + $("select[id$=TUD_ITEM]").val() + "&CatgPk=" + $("select[id$=TUD_ITEM_TYPE]").val() + "&Qty=" + $("input[id$=TUD_QUANTITY]").val() + "&ToUom=" + $("select[id$=TUD_QTY_UOM]").val() + "&DtlsPK=" + $("input[id$=TUD_PK]").val(), function (data) {
            stockStatus = data;
            if (stockStatus == "1") {
                TopUpJson = $("#divDatas").data("TopUpData");
                var slNO = $("input[id$=SL_NO]").val();
                var obj = new Object();
                var flag = true;
                if ($("[id$=SL_NO]").val() == "0") {
                    for (var i in TopUpJson.ItemList) {
                        if ((TopUpJson.ItemList[i].TUD_ITEM_TYPE == $("select[id$=TUD_ITEM_TYPE]").val()) && (TopUpJson.ItemList[i].TUD_ITEM == $("select[id$=TUD_ITEM]").val()) && (TopUpJson.ItemList[i].TUD_TANK == $("select[id$=TUD_TANK]").val())) {
                            flag = false;
                            break;
                        }
                    }
                }
                else {
                    for (var i in TopUpJson.ItemList) {
                        if (TopUpJson.ItemList[i].SL_NO != slNO || TopUpJson.ItemList[i].TUD_PK != $("inuput[id$=TUD_PK]").val()) {
                            if ((TopUpJson.ItemList[i].UPC_TO_UOM == $("select[id$=TUD_ITEM_TYPE]").val()) && (TopUpJson.ItemList[i].TUD_ITEM == $("select[id$=TUD_ITEM]").val()) && (TopUpJson.ItemList[i].TUD_TANK == $("select[id$=TUD_TANK]").val())) {
                                flag = false;
                                break;
                            }
                        }
                    }
                    for (var i in TopUpJson.ItemList) {

                        if (parseInt(slNO) == TopUpJson.ItemList[i].SL_NO)

                            obj = TopUpJson.ItemList[i];
                    }

                }
                // Check Already Added Or Not
                if (flag) {
                    // Add One By one Details To Object
                    obj.TUD_TANK_TYPE = $("select[id$=TUD_TANK_TYPE]").val();
                    obj.TUD_TANK_TYPE_NAME = $("select[id$=TUD_TANK_TYPE] :selected").text();

                    obj.TUD_TANK = $("select[id$=TUD_TANK]").val();
                    obj.TUD_TANK_NAME = $("select[id$=TUD_TANK] :selected").text();

                    obj.TUD_ITEM_TYPE = $("select[id$=TUD_ITEM_TYPE]").val();
                    obj.TUD_ITEM_TYPE_NAME = $("select[id$=TUD_ITEM_TYPE] :selected").text();

                    obj.TUD_ITEM = $("select[id$=TUD_ITEM]").val();
                    obj.TUD_ITEM_NAME = $("select[id$=TUD_ITEM] :selected").text();

                    obj.TUD_BATCH = $("select[id$=TUD_ITEM_TYPE]").val() != "1" ? $("select[id$=TUD_BATCH]").val() : "0";
                    obj.TUD_BATCH_NAME = $("select[id$=TUD_ITEM_TYPE]").val() != "1" ? $("select[id$=TUD_BATCH] :selected").text() : "";

                    obj.TUD_QUANTITY = $("[id$=TUD_QUANTITY]").val();
                    obj.TUD_QTY_UOM = $("select[id$=TUD_QTY_UOM]").val();

                    obj.QUANTITY = $("[id$=TUD_QUANTITY]").val() + " ( " + $("select[id$=TUD_QTY_UOM] :selected").text() + " ) ";
                    if (parseInt(slNO) == 0) {
                        obj.TUD_PK = 0;
                        obj.SL_NO = TopUpJson.ItemList.length + 1;
                        TopUpJson.ItemList.push(obj);
                    }

                    $("#divDatas").data("TopUpData", TopUpJson);
                    GrandGrid.MakeGrid($("#grdTopUpMaterial"), 0, TopUpJson.ItemList);
                    ClearMaterialControls();
                    RemoveValidations(2);

                }
                else {

                    GrandScriptUtils.ShowModal(TopUp.ItemAlreadyAdded, TopUp.InformationTitle);
                }

            }
            else {
                GrandScriptUtils.ShowModal(TopUp.STOCKNOTAVAILABLE, TopUp.InformationTitle);
            }
        });

        return false;
    }


}


function ClearMaterialControls() {
    //<summary>Function  used to Clear Item Details Controls</summary>
    RemoveValidations(2);
    $("select[id$=TUD_TANK_TYPE]").val(TopUp.ValueZero);
    $("select[id$=TUD_ITEM_TYPE]").val(TopUp.ValueZero);
    $("input[id$=ITEM_UOM]").val(TopUp.ValueZero);
    $("select[id$=TUD_BATCH]").val(TopUp.ValueZero);
    $("select[id$=TUD_BATCH]").hide();
    $("input[id$=TUD_QUANTITY]").val(TopUp.ValueEmpty);
    $("input[id$=TUD_PK]").val(TopUp.ValueZero);
    $("input[id$=SL_NO]").val(TopUp.ValueZero);
    $("input[id$=hdfTUD_BATCH]").val(TopUp.ValueZero);
    FillTankType(0,0,0);
    FillMaterialNames(0, 0, 0);
    FillUOM(0);

}


function FillMaterialDetails(tr) {
    //<summary>Function used to Fill Mateiral Details When Edit  </summary>
    $("select[id$=TUD_ITEM_TYPE]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_ITEM_TYPE", $(tr).parent().parent().attr("id")));
    $("select[id$=TUD_TANK_TYPE]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_TANK_TYPE", $(tr).parent().parent().attr("id")));
    FillTankName(GrandGrid.Utilities.GetColumnValue(tr, "TUD_TANK", $(tr).parent().parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "TUD_QTY_UOM", $(tr).parent().parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "TUD_ITEM", $(tr).parent().parent().attr("id")), GrandGrid.Utilities.GetColumnValue(tr, "TUD_BATCH", $(tr).parent().parent().attr("id")));
    $("select[id$=TUD_TANK]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_TANK", $(tr).parent().parent().attr("id")));
    $("input[id$=SL_NO]").val(GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", $(tr).parent().parent().attr("id")));
    $("input[id$=TUD_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_PK", $(tr).parent().parent().attr("id")));
    $("input[id$=TUD_QUANTITY]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_QUANTITY", $(tr).parent().parent().attr("id")));
    $("input[id$=ITEM_UOM]").val(GrandGrid.Utilities.GetColumnValue(tr, "ITEM_UOM", $(tr).parent().parent().attr("id")));
    $("input[id$=hdfTUD_BATCH]").val(GrandGrid.Utilities.GetColumnValue(tr, "TUD_BATCH", $(tr).parent().parent().attr("id")));
    if (GrandGrid.Utilities.GetColumnValue(tr, "TUD_ITEM_TYPE", $(tr).parent().parent().attr("id")) != "1") {
        $("select[id$=TUD_BATCH]").show();
    }
    else {
        $("select[id$=TUD_BATCH]").hide();
    }

}

function AfterGridBind(grdID) {
    //<summary>Function used to Fill Controls to Gird, after add details</summary>
    if (grdID == "grdTopUpMaterial") {
        if (tdset == "") {
            tdset = $("#MaterialControls").find("tr:eq(1)");
        }
        $("#MaterialControls").hide();
        $("#MaterialControls").css({ "display": "none", "visibility": "hidden" });
        $("#grdTopUpMaterial").show();
        $(tdset).insertBefore($("#grdTopUpMaterial").find("tr:eq(1)"));

        //Used to Avoid the Null for remarks when we have not enterd any thing in the remarks field
        
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "TUD_BATCH_NAME", $("#grdTopUpMaterial").attr("id"));
        $("#grdTopUpMaterial").find("tr:has(td)").each(function (index) {//loop through each td and find the remarks is null if null it will be cleared
            if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
                $(this).find("td:eq(" + ColIndexremarks + ")").html("");
            }
        });
    }
}


function DeleteMaterialDetails() {
    //<summary>Function used to Delete Material Details  </summary>
    var TopUpJson = $("#divDatas").data("TopUpData");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in TopUpJson.ItemList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (TopUpJson.ItemList[i].SL_NO == slno) {
            // Splice Details From List, Corresponding ToUnit
            TopUpJson.ItemList.splice(i, 1);
            break;
        }
    }
    $("#divDatas").data("TopUpData", TopUpJson);
    GrandGrid.MakeGrid($("#grdTopUpMaterial"), 0, TopUpJson.ItemList);
    ClearMaterialControls();
    if (TopUpJson.ItemList.length == 0) {
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
        case TopUp.DELETE:
            slno = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdTopUpMaterial");
            GrandScriptUtils.ShowModal(TopUp.DOUWANTTODELETEMSG, TopUp.CONFIRMATION, TopUp.DELETE, true)
            return false;
            break;
        case TopUp.EDIT:
            FillMaterialDetails(tr);
            return false;
            break;
        default:
            GrandScriptUtils.ShowModal("Default action need to perform", TopUp.InformationTitle);
            return false;
            break;
    }
}

function ModalOk(command) {
    //<summary>Function used to do action after POPup Ok Button Event</summary>
    switch (command) {
        case TopUp.DELETE:
            DeleteMaterialDetails();
            break;
        case TopUp.SAVE:
            window.location = TopUp.TOPUPLISTINGURL;

    }
    return false;
}

function CancelPage() {
    //<summary>Function used to redirect to listing page  </summary>
    RemoveValidations();
    window.location = TopUp.TOPUPLISTINGURL;
    return false;
}


function SavePage() {
    //<summary>Function used to Save TopUp Details  </summary>
    AddValidations(1);
    var ObjTopUp = $("#divDatas").data("TopUpData");
    $("[id$=ItemList]").val(JSON.stringify(ObjTopUp.ItemList));
    TopUpJson = $("#divDatas").data("TopUpData");
    // Check Have The Order List have More than or equal to one Product Details
    if ($(document.forms[0]).valid()) {
        if (TopUpJson.ItemList.length > 0) {
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.post(TopUp.SAVEPAGEURL, jSonString, function (data) {
                // Check Machine Details Saved Successfuully or not
                if (parseInt(data) > 0) {
                    GrandScriptUtils.ShowModal(TopUp.TOPUPSAVEMSG, TopUp.InformationTitle, TopUp.SAVE);
                }
                else {
                    if (parseInt(data) ==-3)
                        msgtxt = TopUp.STOCKNOTAVAILABLE;
                    else 
                        msgtxt = TopUp.ACTIONFAILED;
                    GrandScriptUtils.ShowModal(msgtxt, TopUp.InformationTitle);
                }

            });
        }
        else {
            GrandScriptUtils.ShowModal(TopUp.ADDMATERIAL, TopUp.InformationTitle);
        }

    }
    return false;
}


function FillTopUp() {
    ///<summary>Used for Fill TopUp Details </summary>
    var compoundPK = $("input[id$=TUH_PK]").val();
    if (compoundPK != TopUp.ValueZero) {
        $.get(TopUp.FILLTOPUPDTLLSURL + compoundPK, function (data) {
            $("#divDatas").data("TopUpData", data);
            FillTopUpDetails();

        });
    }
    else {

        FillCombo();
    }
    DateTimeInit();
}

function FillTopUpDetails() {
    ///<summary>Used for Fill TopUp Details </summary>
    FillShift(0);
    FillPlan(0);
    FillTankType(0, 0,0);
    FillMaterialNames(0, 0,0);
    TopUpJson = $("#divDatas").data("TopUpData");
    $("input[id$=TUH_PK]").val(TopUpJson.TUH_PK);
    $("input[id$=TUH_SHIFT]").val(TopUpJson.TUH_SHIFT);
    $("input[id$=TUH_SHIFT_NAME]").val(TopUpJson.TUH_SHIFT_NAME);
    $("input[id$=TUH_PLAN]").val(TopUpJson.TUH_PLAN);
    $("input[id$=TUH_PLAN_NAME]").val(TopUpJson.TUH_PLAN_NAME);
    $("input[id$=TUH_DATE]").val(TopUpJson.TUH_DATE);
    $("input[id$=TUH_TIME]").val(TopUpJson.TUH_TIME);
    if (!($.isArray(TopUpJson.ItemList))) {
        var objArray;
        if (TopUpJson.ItemList != undefined) {
            objArray = TopUpJson.ItemList;
            TopUpJson.ItemList = new Array();
            TopUpJson.ItemList.push(objArray);
            GrandGrid.MakeGrid($("#grdTopUpMaterial"), 0, TopUpJson.ItemList);
        }
        else {
            objArray = TopUpJson.ItemList;
            TopUpJson.ItemList = new Array();
        }
    }
    else {

        GrandGrid.MakeGrid($("#grdTopUpMaterial"), 0, TopUpJson.ItemList);
    }



}

function FillCombo() {
    //<summary>Function used to Fill All Combos in a Page  </summary>
    FillShift(0);
    FillPlan(0);
    FillTankType(0, 0,0);

}

function FillShift(shiftpk) {
    ////<summary>function used to fill shift </summary>
    GrandScriptUtils.MakeAutoComplete("TUH_SHIFT_NAME", TopUp.GETSHIFTDETAILSURL, "TUH_SHIFT", true, false, false, true, false);
}

function FillPlan(planID) {
    //<summary>function To Fill Plan Details </summary>
    GrandScriptUtils.MakeAutoComplete("TUH_PLAN_NAME", TopUp.FILLPLANURL, "TUH_PLAN", true, false, false, true, false);

}

function FillTankType(tankTypePK, tankPK, uomPK) {
    //<summary>function To Fill Tank Type Details </summary>
    var drpID = $("select[id$=TUD_TANK_TYPE]").attr("id");
    $.getJSON(TopUp.FILLTANKTYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, tankTypePK);
        FillTankName(tankPK, uomPK,0,0);
    });
}


function ShowItemsControls(typePk) {
    //<summary>Function used to Show BatchNumber Control, When Select Dispersion and Compound</summary>
    if (typePk == "1") {
        $("[id$=TUD_BATCH]").hide();
        //$("[id$=TUD_BATCHNo]").show();
    }
    else {
        $("[id$=TUD_BATCH]").show();
        //$("[id$=TUD_BATCHNo]").hide();
    }

    FillMaterialNames(0, 0,0);
}

function FillTank() {
    FillTankName(0,0,0,0);
}

function FillTankName(tankName, uomPK, matPK, batchNo) {
    //<summary>Function used to Fill Tank Name </summary>
    var drpID = $("select[id$=TUD_TANK]").attr("id");
    $.getJSON(TopUp.FILLTANKNAMEURL + $("[id$=BizUnitPk]").val() + "&TankTypePK=" + $("[id$=TUD_TANK_TYPE]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, tankName);
        //FillUOM(uomPK);
        FillMaterialNames(matPK, batchNo, uomPK);
    });

}

function FillBatchNo(selValue, uomPk) {
    //<summary>Function used to Fill Bach Number for Compound and Dispersion item</summary>
    var drpID = $("select[id$=TUD_BATCH]").attr("id");
    if ($("select[id$=TUD_ITEM]").val() != "0" && $("select[id$=TUD_ITEM_TYPE]").val() != "0" && $("select[id$=TUD_ITEM_TYPE]").val() != "1") {
        $.getJSON(TopUp.FILLBATCHNUMBERURL + $("select[id$=TUD_ITEM]").val() + "&ItemType=" + $("select[id$=TUD_ITEM_TYPE]").val() + "&BatchPK=" + $("[id$=hdfTUD_BATCH]").val(), function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
        });
    }
    else {
        GrandScriptUtils.FillDropDown(drpID, null, true, true, selValue);
    }
    FillUOM(uomPk)
}


function FillMaterialDetailsByTank() {
    //<summary>Function used to Fill Material Name By TankUOM  </summary>
    FillMaterialNames(0, 0,0);
    //FillUOM(0);
}

function FillMaterialNames(selValue, batchNo, uomPK) {
    //<summary>Function used to Fill Material Name to DropDown  </summary>
    var drpID = $("select[id$=TUD_ITEM]").attr("id");
    if ($("select[id$=TUD_ITEM_TYPE]").val() != "0" && $("select[id$=TUD_TANK]").val() != "0") {
        $.getJSON(TopUp.FILLMATERIALNAMEURL + $("[id$=BizUnitPk]").val() + "&CATG=" + $("select[id$=TUD_ITEM_TYPE]").val() + "&TankPk=" + $("select[id$=TUD_TANK]").val(), function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
            FillBatchNo(batchNo, uomPK);
        });
    }
    else {
        GrandScriptUtils.FillDropDown(drpID, null, true, true, selValue);
    }
}

function FillUOM(selValue) {
    //<summary>Function used to Fill UOM 's List , with have conversion value, for corresponding Item UOM</summary>
//    var drpID = $("select[id$=TUD_QTY_UOM]").attr("id");
//    if ($("select[id$=TUD_TANK]").val() != "0") {
//        $.getJSON(TopUp.FILLUOMLISTWITHCONVERSIONFACTOR + $("select[id$=TUD_TANK]").val(), function (data) {
//            GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
//        });
//    }
//    else {
//        GrandScriptUtils.FillDropDown(drpID, null, true, true, selValue);
//    }

      var drpID = $("select[id$=TUD_QTY_UOM]").attr("id");
      if ($("select[id$=TUD_ITEM]").val() != "0" && $("select[id$=TUD_ITEM_TYPE]").val() != "0") {
          $.getJSON(TopUp.FILLMATERIALUOM + $("select[id$=TUD_ITEM]").val() + "&Catg=" + $("select[id$=TUD_ITEM_TYPE]").val(), function (data) {
              GrandScriptUtils.FillDropDown(drpID, data, true, true, selValue);
          });
      }
      else {
          GrandScriptUtils.FillDropDown(drpID, null, true, true, selValue);
      }

}

//<summary>Function used to Filter For Numeric values </summary>
function MakeNumeric(event) {
    //var keyVal = event.keyCode;
    if (!(event.keyCode == 45 || event.keyCode == 46 || event.keyCode == 48 || event.keyCode == 49 || event.keyCode == 50 || event.keyCode == 51 || event.keyCode == 52 || event.keyCode == 53 || event.keyCode == 54 || event.keyCode == 55 || event.keyCode == 56 || event.keyCode == 57)) {
        event.returnValue = false;
    }
}

///#endregion

///#region=============== Validation
function AddValidations(mode) {

    if (mode == 1) {
        $("input[id$=TUH_SHIFT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectShift)" }
        });
        $("input[id$=TUH_PLAN]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectPlan)" }
        });

        $("input[id$=TUH_DATE]").rules("add", {
            required: true,
            maxlength: 95,
            messages: { required: "Translate(EnterDate)" }
        });
        $("input[id$=TUH_TIME]").rules("add", {
            required: true,
            maxlength: 190,
            messages: { required: "Translate(EnterTime)" }
        });

    }

    else if (mode == 2) {
        $("select[id$=TUD_TANK_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectTankType)" }
        });
        $("select[id$=TUD_TANK]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectTank)" }
        });
        $("select[id$=TUD_ITEM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseSelectMaterialType)" }
        });

        $("select[id$=TUD_ITEM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectItem)" }
        });
        // Check Selected item != RawMaterial
        if ($("select[id$=TUD_ITEM_TYPE]").val() != "1") {

            $("select[id$=TUD_BATCH]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Select Batch" }
            });

        }

        $("select[id$=TUD_QTY_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectUOM)" }
        });



        $("input[id$=TUD_QUANTITY]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal: true,
            messages: { required: "Translate(EnterQuantity)" }
        });

    }
}

function RemoveValidations(mode) {
    if (mode == 1) {

        $("input[id$=TUH_TIME]").rules("remove");
        $("input[id$=TUH_DATE]").rules("remove");
        $("input[id$=TUH_SHIFT]").rules("remove");
        $("input[id$=TUH_PLAN]").rules("remove");


    }
    else if (mode == 2) {
        $("input[id$=TUD_QUANTITY]").rules("remove");
        $("select[id$=TUD_QTY_UOM]").rules("remove");
        if ($("select[id$=TUD_ITEM_TYPE]").val != "1") {
            $("select[id$=TUD_BATCH]").rules("remove");
        }
        $("select[id$=TUD_ITEM]").rules("remove");
        $("select[id$=TUD_ITEM_TYPE]").rules("remove");
        $("select[id$=TUD_TANK]").rules("remove");
        $("select[id$=TUD_TANK_TYPE]").rules("remove");

    }

}




///#endregion