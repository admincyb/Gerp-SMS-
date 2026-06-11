/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />



//#region ---------- Configuration Section-------

var StoreAudit = {
    EditMsg: "Translate(EditbyAnotherUser)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    StoreAuditSave1: "Translate(StoreAuditSave1)",
    NoDamageDetails: "Translate(ErrDamageDetails)",
    Conformation: "Translate(Conformation)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    DeleteConformation: "Translate(Doyouwanttodeletethisdetails)",
    StoreAuditSave2: "Translate(StoreAuditSave2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    FillStoreDropdownURL: "SubDepartmentManagement.do?Action=GetInventoryStores&SBUPk=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompany&SBUPk=",
    FillMaterialCategoryExceptFGDropdownURL: "MaterialCategory.do?Action=GetMaterialCategoryAutoList&AUTOSEARCH=1&SBUPk=",
    GetMaterialDetails: "MaterialManagement.do?Action=GetMaterialDetails&SBUPk=",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",
    SaveCommand: "SAVE",
    BizUnitPk: 0,
    FillBatchNoDropDownURL: "MaterialManagement.do?Action=GetBatchNo&SBUPk=",
    MaterialBatchValidation: "Translate(SelectBatch)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)"
}

///#endregion

///#region ---------- Global Variable Declaration

var AuditJson = new Object();
var itemPK = 0;
var itemPKD = 0;
var DamageType = 0;
var currentStock = 0;
var QtyDec, AmtDec;
var typeText = "";
///#endregion

///#region----------- Initialization Section ----------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectNoneText", function (value, element) {
        return ($(element).val() != "Translate(Select)");
    }, "Translate(Pleaseselectanoption)");
    $.validator.addMethod("selectAuto", function (value, element) {      
        return ($(element).val() != "Select/Type");
    }, "Translate(Pleaseselectanoption)");
    if ($("[id$=AutoStartValue]").val() != 0) {
        typeText = "Type min. " + $("[id$=AutoStartValue]").val() + " characters";
    }
    else {
        typeText = "Translate(AutoDefaultValue)";
    }
    PageInit();

});

function PageInit() {
    //$("[id$=txtBoxReqQty]").hide();
    if ($("[id$=ItemList]").val() == "null") {
        GrandScriptUtils.ShowModal(StoreAudit.ItemAlreadyDeletedMsg, StoreAudit.InformationTtile, StoreAudit.SaveCommand);
    }
    else {       
        DateInit();
        AuditJson = $.parseJSON($("[id$=ItemList]").val());
        $("#divDatas").data("ItemList", AuditJson);
        //Initializing the array if one element convert it to array object
        if (!($.isArray(AuditJson.ItemList))) {
            if (AuditJson.ItemList != undefined) {
                objArray = AuditJson.ItemList;
                AuditJson.ItemList = new Array();
                AuditJson.ItemList.push(objArray);
            }
            else {
                objArray = AuditJson.ItemList;
                AuditJson.ItemList = new Array();
            }
        }
        if (!($.isArray(AuditJson.DamageStock))) {

            if (AuditJson.DamageStock != undefined) {
                objArray = AuditJson.DamageStock;
                AuditJson.DamageStock = new Array();
                AuditJson.DamageStock.push(objArray);
            }
            else {
                objArray = AuditJson.DamageStock;
                AuditJson.DamageStock = new Array();
            }
        }
        var queryStr = window.location.search.substring(1);
        if (queryStr != "") {
            var queryStr = queryStr.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                    FillStoreAuditDetails(AuditJson);
                }
                if ((pK[1] != "" && pK[0] == "Status") || (pK[1] != "" && pK[0] == "Flag")) {
                    //Check the query string Name status if status as 1 thn its in view mode
                    $("#searchwrap").hide();
                    $("[id$=ViewStatus]").val('1');
                    FillStoreAuditDetails(AuditJson);
                }
            }
        }
        else {
            FillStore(0);
            FillCompany(0);          
            GrandGrid.Utilities.ResetGrid(true, "grdItemList");
            AuditJson.ItemList = new Array();
            GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);
            EnableDisableDate();
        }
        $("#divDamage").dialog({
            autoOpen: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            }
        });
        $("#divBatch").hide();
        $("[id$=SAH_DEPT_STORE]").focus();
        //$("[id$=MaterialType]").val("1");

        //StkBatch Config Settngs
        if ($("[id$=hdfEnableBatch]").val() == "0") {
            $("[id$=StkBatchNo]").attr("disabled", true);
            $("#divStkBatch").hide();
            $("#divZeroQtyBatches").hide();                    
        }
        else {
            $("[id$=StkBatchNo]").attr("disabled", false);          
        }
    }
    $("[id$=SAHTXT_DATE]").focus();
    FillStkBatchNoAutoComplete();
}

function FillStoreAuditDetails(AuditJson) {
    $("input[id$=SAH_PK]").val(AuditJson.SAH_PK);
    $("[id$=SAHTXT_NO]").text(AuditJson.SAH_NO);
    $("[id$=SAH_NO]").val(AuditJson.SAH_NO);
    $("[id$=SAH_DATE]").val(AuditJson.SAH_DATE);    
    $("[id$=SAHTXT_DATE]").val(AuditJson.SAH_DATE);
    $("[id$=SAH_STATUS]").val(AuditJson.SAH_STATUS);
    if (parseInt($("[id$=SAH_STATUS]").val()) == 0 || parseInt($("[id$=SAH_STATUS]").val()) == 6) {
        if ($("[id$=ViewStatus]").val() != "1") {
            $("#searchwrap").show();
            $("#divDamageButtons").show();
        }
        else {
            $("#searchwrap").hide();
            $("#divDamageButtons").hide();
        }
    }
    else {
        $("#searchwrap").hide();
        $("#divDamageButtons").hide();
    }
    $("[id$=LAST_MOD_DT]").val(AuditJson.LAST_MOD_DT);
    FillStore(AuditJson.SAH_DEPT_STORE);
    FillCompany(AuditJson.SAH_COMPANY);   
    var appStatus = parseInt($("[id$=SAH_STATUS]").val());
    if (parseInt($("[id$=ViewStatus]").val()) == 1 || appStatus > 0) {
        $("[id$=SAH_DEPT_STORE]").attr("disabled", true);
        $("[id$=SAH_COMPANY]").attr("disabled", true);
    }
    // Check Array Have atleast One Record-Check Is Array or Not
    if (!($.isArray(AuditJson.ItemList))) {
        var objArray = AuditJson.ItemList;
        AuditJson.ItemList = new Array();
        AuditJson.ItemList.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);
    EnableDisableDate();
}

function FillStore(selectVal) {

    var drpID = $("select[id$=SAH_DEPT_STORE]").attr("id");
    storeURL = StoreAudit.FillStoreDropdownURL + $("[id$=BizUnitPk]").val() + "&UserPK=" + $("[id$=UserPk]").val();

    $.get(storeURL, function (data) {
        //   $.get("SubDepartment.do?Action=GetAllStores&SBUPk=" + $("[id$=BizUnitPk]").val() + "&UserFlag=0&DeptType=2&DeptPk=0&DeptCatg=0", function (data) {
        AuditJson = $("#divPOData").data("POData");
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        FillMaterialCategoryAutoComplete();
        FillItem();
    });
}

function FillDamageType(selectVal) {

    var drpID = $("select[id$=SDD_DMG_TYPE]").attr("id");
    $.get("StoreAuditManagement.do?Action=GetDamageTypes&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        if (selectVal)
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        else
            GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}
function FillMaterialCategoryAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    var storeID = $("select[id$=SAH_DEPT_STORE]").val();
     if (storeID != null) {
         GrandScriptUtils.MakeAutoComplete("MaterialCategory", StoreAudit.FillMaterialCategoryExceptFGDropdownURL + $("[id$=BizUnitPk]").val() + "&Store=" + $("select[id$=SAH_DEPT_STORE]").val(), "MaterialCategoryPK", true, false, "BizUnitPk", true);
     }
}
function FillItem(selectVal) {

    var storeID = $("select[id$=SAH_DEPT_STORE]").val();
    if (storeID != null) {
        $("[id$=ItemTXTCode]").attr("disabled", false);        
        GrandScriptUtils.MakeAutoCompleteLimitLen("ItemTXTCode", "MaterialManagement.do?Action=GetMaterialSearchValueByCategoryAndStore&AUTOSEARCH=1", "ItemCode", true, false, "MaterialCategoryPK", true, "SAH_DEPT_STORE", "", "", $("[id$=AutoStartValue]").val(), "", typeText, false);
    }
    else {
        $("[id$=ItemTXTCode]").attr("disabled", true);
    }

}

function DateInit() {
    //<summary>function used to make datepicker</summary>
    GrandScriptUtils.DatePicker("SAHTXT_DATE", false, false);   
}

function BindGrid() {
    GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);
    EnableDisableDate();
    return false;
}

function GridHandler(tr, command) {

    itemPKD = GrandGrid.Utilities.GetColumnValue(tr, "SAD_ITEM", $(tr).parent().attr("id"));
    switch (command) {
        case "DELETE":
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(StoreAudit.DeleteConformation, StoreAudit.Conformation, "DELETE", true);
            return false;
            break;
        case "ADDDAMAGE":
            CheckVariation(tr);
            if ($("[id$=ViewStatus]").val() == "1") {
                $("#damagedetailsDiv").hide();
            }

            $("[id$=ItemCode]").rules("remove");
            var damageList = new Object();
            FillDamageType();

            AuditJson = $("#divDatas").data("ItemList");
            for (var i in AuditJson.ItemList) {
                if (AuditJson.ItemList[i].SAD_ITEM == itemPKD) {
                    damageList = AuditJson.ItemList[i]
                }
            }
            if (!($.isArray(damageList.DamageStock))) {
                var objArray;
                if (damageList.DamageStock != undefined) {
                    objArray = damageList.DamageStock;
                    damageList.DamageStock = new Array();
                    damageList.DamageStock.push(objArray);
                    GrandGrid.MakeGrid($("#grdDamageDtls"), 0, damageList.DamageStock);
                }
                else {
                    objArray = damageList.DamageStock;
                    damageList.DamageStock = new Array();
                    GrandGrid.MakeGrid($("#grdDamageDtls"), 0, new Object());
                }
            }
            else {
                if (damageList.DamageStock.length == 0) {
                    GrandScriptUtils.ShowModal(StoreAudit.NoDamageDetails, StoreAudit.MessageBoxTitle);
                }
                GrandGrid.MakeGrid($("#grdDamageDtls"), 0, damageList.DamageStock);
            }
            if (parseInt($("[id$=SAH_STATUS]").val()) == 1 || parseInt($("[id$=SAH_STATUS]").val()) == 2 || parseInt($("[id$=SAH_STATUS]").val()) == 3 || parseInt($("[id$=SAH_STATUS]").val()) == 7) {
                $("#damagedetailsDiv").hide();
            }
            if (damageList.DamageStock.length > 0) {
                $("#divDamage").dialog("open");
                $("#divDamage").dialog({ width: 500, height: 350, resizable: true, title: "Damage Details" });
                $("#divDamage").css({ "min-height": "300", "margin-top": "25px" });
            }
            else {
                if (parseInt($("[id$=SAH_STATUS]").val()) == 1 || parseInt($("[id$=SAH_STATUS]").val()) == 2 || parseInt($("[id$=SAH_STATUS]").val()) == 3 || parseInt($("[id$=SAH_STATUS]").val()) == 7) {

                }
                else {
                    $("#divDamage").dialog("open");
                    $("#divDamage").dialog({ width: 500, height: 350, resizable: true, title: "Damage Details" });
                    $("#divDamage").css({ "min-height": "300", "margin-top": "25px" });
                }
            }
            $("select[id$=SDD_DMG_TYPE]").attr("disabled", false);
            return false;
            break;
    }
    return false;
}

function DamageGridHandler(tr, command) {

    DamageType = GrandGrid.Utilities.GetColumnValue(tr, "SDD_DMG_TYPE", "grdDamageDtls");
    switch (command) {
        case "EDIT":
            $("select[id$=SDD_DMG_TYPE]").val(GrandGrid.Utilities.GetColumnValue(tr, "SDD_DMG_TYPE", "grdDamageDtls"));
            $("input[id$=SDD_DMG_QTY]").val(GrandGrid.Utilities.GetColumnValue(tr, "SDD_DMG_QTY", "grdDamageDtls").replace(/[^0-9\.]+/g, ""));
            $("select[id$=SDD_DMG_TYPE]").attr("disabled", true);
            $("input[id$=PrvQuantity]").val(GrandGrid.Utilities.GetColumnValue(tr, "SDD_DMG_QTY", "grdDamageDtls").replace(/[^0-9\.]+/g, ""));
            break;
        case "DELETE":
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(StoreAudit.DeleteConformation, StoreAudit.Conformation, "DELETEDAMAGEDTL", true);
            return false;
            break;

    }
    return false;
}


function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>

    switch (command) {

        case "DELETE":
            DeleteItemDetails();
            break;
        case "SAVE":
            window.location = "StoreAuditListing.aspx";
            return false;
            break;
        case "DELETEDAMAGEDTL":
            DeleteDamageItemDetails();
            break;
        case "EXIST":
            ClearDamageDetails();
            break;
    }
    return false;
}


function DeleteDamageItemDetails() {
    var damageList = new Object();
    AuditJson = $("#divDatas").data("ItemList");
    for (var i in AuditJson.ItemList) {
        if (AuditJson.ItemList[i].SAD_ITEM == itemPKD) {
            damageList = AuditJson.ItemList[i]
        }
    }
    if (!($.isArray(damageList.DamageStock))) {
        var objArray;
        if (damageList.DamageStock != undefined) {
            objArray = damageList.DamageStock;
            damageList.DamageStock = new Array();
            damageList.DamageStock.push(objArray);
        }
        else {
            objArray = damageList.DamageStock;
            damageList.DamageStock = new Array();
        }
    }
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in damageList.DamageStock) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (damageList.DamageStock[i].SDD_DMG_TYPE == DamageType) {
            // Splice Details From List, Corresponding ToUnit
            damageList.DamageStock.splice(i, 1);
            break;
        }
    }
    $("#divDatas").data("ItemList", AuditJson);
    GrandGrid.MakeGrid($("#grdDamageDtls"), 0, damageList.DamageStock);
    ClearDamageDetails();
}

function DeleteItemDetails() {
    
    AuditJson = $("#divDatas").data("ItemList");
    // Delete Conversion Details - By MaintanceInfoID Using Loop
    for (var i in AuditJson.ItemList) {
        // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
        if (AuditJson.ItemList[i].SAD_ITEM == itemPKD) {
            // Splice Details From List, Corresponding ToUnit
            AuditJson.ItemList.splice(i, 1);
            break;
        }
    }
    $("#divDatas").data("ItemList", AuditJson);
    BindGrid(); 
    if (AuditJson.ItemList.length > 0)
        $("[id$=SAHTXT_DATE]").attr("disabled", "disabled");
    else
        $("[id$=SAHTXT_DATE]").removeAttr("disabled");
}

///#endregion

//#region----------- Validation Section----------------

function AddValidations(mode) {
    //<summary>Function used to assign validation</summary>

    RemoveValidations();
    if (mode == 1) {
        $("[id$=MaterialCategory]").rules("add", {
            selectAuto: true,
            messages: { selectAuto: "Translate(SelectCatagory)" }
        });
        $("[id$=ItemTXTCode]").rules("add", {
            selectNoneText: true,
            messages: { selectNoneText: "Translate(SelectItemCode)" }
        });
        if (parseInt($("[id$=MaterialCategoryPK]").val()) > 1) {
            $("[id$=BatchNo]").rules("add", {
                selectNone: true,
                messages: { selectNone: "Translate(SelectBatchNo)" }
            });

            $("input[id$=SAHTXT_DATE]").rules("add", { 
                required: true,
                messages: { required: "Translate(SelectSADate)" }
            });
        }
        //Stk Batch Validation
        if ($("[id$=hdfEnableBatch]").val() == "1") {
            $("[id$=StkBatchNo]").rules("add", {
                selectAuto: true,
                messages: { selectAuto: StoreAudit.MaterialBatchValidation }
            });
        }
    }
    if (mode == 2) {
        $("[id$=SDD_DMG_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectDamageType)" }
        });

        $("[id$=SDD_DMG_QTY]").rules("add", {
            required: true,
            maxlength: 12,
            DecimalDigits: QtyDec,
            CustomDecimal: true,
            messages: { required: "Translate(EnterDamageQuantity)", CustomDecimal: String.format("Translate(ErMsgMorethanDecimal)", QtyDec) }
        });

    }
}

function RemoveValidations() {
    //<summary>Function used to Remove validation</summary>

    $(document.forms[0]).validate().resetForm();
    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}

//#endregion

function SavePage(command) {

    RemoveValidations();
    var SaveMessageWithSANo = "";
    var PriorDateMsg = "";
    var SAD_ID = "";
    var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
    var ColIndexActVal = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
    var ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", $("#grdItemList").attr("id"));
    $("[id$=SAH_DEPT_STORE]").attr("disabled", false);
    $("select[id$=SAH_COMPANY]").attr("disabled", false);
    $("[id$=SAH_DATE]").val($("[id$=SAHTXT_DATE]").val());      //Assign TextBox date value to hiddenfiled value
    AuditJson = $("#divDatas").data("ItemList");
     var ActualValueFlag = true;
    //loop through Audit And Grid to Capure the last modifications in Grid
    for (var i in AuditJson.ItemList) {
        $("#grdItemList").find("tr:has(td)").each(function (index) {
            SAD_ID = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ID", $("#grdItemList").attr("id"));
            if (AuditJson.ItemList[i].SAD_ID == SAD_ID) {
                if (parseInt($("[id$=SAH_STATUS]").val()) != 1 && parseInt($("[id$=SAH_STATUS]").val()) != 7 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=SAH_STATUS]").val()) != 3) {
                    AuditJson.ItemList[i].SAD_ACT_STK = $(this).find("td:eq(" + colIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
                    AuditJson.ItemList[i].SAD_REMARKS = $(this).find("td:eq(" + ColIndexremarks + ") textarea").val().replace(/[^0-9\.]+/g, "");
                    AuditJson.ItemList[i].SAD_ACT_STK_VAL = $(this).find("td:eq(" + ColIndexActVal + ") input[type=text]").val();
                }
                else {
                    AuditJson.ItemList[i].SAD_ACT_STK_VAL = $(this).find("td:eq(" + ColIndexActVal + ")").html().replace(/[^0-9\.]+/g, "");
                    AuditJson.ItemList[i].SAD_ACT_STK = $(this).find("td:eq(" + colIndex + ")").html().replace(/[^0-9\.]+/g, "");
                    AuditJson.ItemList[i].SAD_REMARKS = $(this).find("td:eq(" + ColIndexremarks + ")").html();
                }
                if (parseFloat(AuditJson.ItemList[i].SAD_ACT_STK.toString()) > 0 && (!parseFloat(AuditJson.ItemList[i].SAD_ACT_STK_VAL.toString()) > 0)) {
                    ActualValueFlag = false;
                }

            }
        });

    }
    if (ActualValueFlag == false) {
        GrandScriptUtils.ShowModal("Translate(MsgActualValue)", StoreAudit.MessageBoxTitle, false, false);
        return false;
    }
    //
    if (AuditJson.ItemList.length > 0) {

        if ($(document.forms[0]).valid()) {
            //Showing validation for Future Date selection
            if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                var RetVal = CompareDate($("[id$=SAHTXT_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                if (RetVal == 1) {                   
                    ShowFutureDate(command);
                    return false;
                }
            }
            $("[id$=ItemList]").val(JSON.stringify(AuditJson.ItemList));
            $("[id$=WKF_FLAG]").val("0");
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
                if ($("[id$=ReferenceID]").val() == "0") {
                    $("[id$=WKF_FLAG]").val("1");
                }
            }
            else
                $("[id$=ActionID]").val('0');  // save only.
            var jSonString = GrandScriptUtils.FormToJsonString(false);

            //To Prevent Duplicate Submission
            if ($("[id$=SubmitFlag]").val() == "0")
                $("[id$=SubmitFlag]").val('1')
            else
                return false;


            $.post("StoreAuditManagement.do?Action=SaveStoreAuditDtls", jSonString, function (data) {
                if (parseInt(data[0]) > 0) {
                    if (command == "Draft") {
                        SaveMessageWithSANo = StoreAudit.StoreAuditSave1 + " " + data[1] + " " + StoreAudit.StoreAuditSave2;
                        GrandScriptUtils.ShowModal(SaveMessageWithSANo, StoreAudit.MessageBoxTitle, "SAVE");
                    }
                    else if (parseInt(data[0]) > 0) {// If action - WorkFlow Save
                        $("[id$=hdfAppID]").val(data[0]);
                        $("[id$=AppNo]").val(data[1]);
                        SaveWorkFlow();
                    }
                }
                else if (parseInt(data[0]) == -1) {
                    GrandScriptUtils.ShowModal(StoreAudit.ActionFailedMessage, StoreAudit.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -2) {
                    GrandScriptUtils.ShowModal(StoreAudit.StoreAuditSave1 + " " + data[1] + " " + StoreAudit.EditMsg, StoreAudit.MessageBoxTitle, "SAVE");
                    $("[id$=SubmitFlag]").val('0')
                }
                else if (parseInt(data[0]) == -10) {
                    PriorDateMsg = String.format(StoreAudit.CannotReceivePriorDateSendReceive, "("+data[2] +")")
                    GrandScriptUtils.ShowModal(PriorDateMsg, StoreAudit.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
                else {
                    GrandScriptUtils.ShowModal(StoreAudit.ActionFailedMessage, StoreAudit.MessageBoxTitle);
                    $("[id$=SubmitFlag]").val('0')
                }
            });
        }
    }
    else {
        GrandScriptUtils.ShowModal("Translate(AddItemDetails)", StoreAudit.MessageBoxTitle, false, false);
        $("[id$=SubmitFlag]").val('0')
    }
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>

    var msg = StoreAudit.StoreAuditSave1 + $("[id$=AppNo]").val() + " " + StoreAudit.SubmitMessage;
    GrandScriptUtils.ShowModal(msg, StoreAudit.MessageBoxTitle, "SAVE");
}


function ResetPage() {

    RemoveValidations();
    window.location = "../StoreManagement/StoreAuditListing.aspx";
    return false;
}


function AddMaterial() {

    AddValidations(1);
    if ($(document.forms[0]).valid()) {
        var materialID = $("[id$=ItemCode]").val();
        var categoryPK = $("[id$=MaterialCategoryPK]").val();
        var batchPK = "0"; //parseInt(categoryPK) > 1 ? $("[id$=BatchNo]").val() : "0";
        var StkbatchPK = $("[id$=StkBatchNoPK]").val();
        var obj = new Object();
        var SAD_ID = "";
        var colIndex = 0;
        var colAckStkValIndex = 0;
        var ColIndexremarks = 0;
        var materialDetails = new Object();
        AuditJson = $("#divDatas").data("ItemList");
        var flag = true;

        for (var i in AuditJson.ItemList) {
            if (AuditJson.ItemList[i].SAD_ITEM == materialID && AuditJson.ItemList[i].SAD_STK_BATCH == StkbatchPK) {
                flag = false;
                break;
            }
        }
        if (flag) {
            $.getJSON("StoreAuditManagement.do?Action=GetCategoryItemDetails&Dept=" + $("[id$=SAH_DEPT_STORE]").val() + "&Category=" + categoryPK + "&Item=" + materialID + "&Batch=" + batchPK + "&StkBatch=" + StkbatchPK, function (data) {
                materialDetails.SAD_ID = GrandScriptUtils.GenerateGuid();
                materialDetails.SAD_PK = 0;
                materialDetails.SAD_ITEM_CATEGORY = categoryPK;
                materialDetails.SAD_ITEM_CATEGORY_TEXT = $("[id$=MaterialCategory]").val();
                materialDetails.SAD_ITEM = data[0].ITM_PK;
                materialDetails.ITM_TEXT = data[0].ITM_TEXT;
                materialDetails.SAD_ITEM_BATCH = batchPK; //parseInt(categoryPK) > 1 ? batchPK : "0";
                materialDetails.SAD_ITEM_BATCH_TEXT = ""; //parseInt(categoryPK) > 1 ? $("[id$=BatchNo] :selected").text() : "";
                materialDetails.SAD_STK_BATCH = StkbatchPK; //parseInt(categoryPK) == 1 ? StkbatchPK : "0"; //StkBatchPK                
                materialDetails.SAD_STK_BATCH_NO = $("[id$=StkBatchNo]").val(); //parseInt(categoryPK) == 1 ? $("[id$=StkBatchNo]").val() : "";  //For Displaying Stock Batch no in grid    
                materialDetails.SAD_CUR_STK = data[0].STOCK;
                materialDetails.SAD_ACT_STK = data[0].ADJ_STK;
                materialDetails.ITM_UOM_TEXT = data[0].ITM_UOM_TEXT;
                //New
                materialDetails.SAD_CUR_STK_VAL = data[0].STOCK_VALUE == null ? 0 : data[0].STOCK_VALUE;
                materialDetails.SAD_ACT_STK_VAL = data[0].ADJ_VALUE == null ? 0 : data[0].ADJ_VALUE;
                materialDetails.SAD_CUR_STK_VAL = (parseFloat(materialDetails.SAD_CUR_STK_VAL).toFixed(QtyDec)); //Some Issue found in value -0.000002
                materialDetails.SAD_REMARKS = "";
                AuditJson.ItemList.push(materialDetails);
                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
                colAckStkValIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
                ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", $("#grdItemList").attr("id"));
                for (var i in AuditJson.ItemList) {
                    //obj = AuditJson.ItemList[i];
                    $("#grdItemList").find("tr:has(td)").each(function (index) {
                        SAD_ID = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ID", $("#grdItemList").attr("id"));
                        if (AuditJson.ItemList[i].SAD_ID == SAD_ID) {
                            AuditJson.ItemList[i].SAD_ACT_STK = $(this).find("td:eq(" + colIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
                            AuditJson.ItemList[i].SAD_ACT_STK_VAL = $(this).find("td:eq(" + colAckStkValIndex + ") input[type=text]").val().replace(/[^0-9\.]+/g, "");
                            AuditJson.ItemList[i].SAD_REMARKS = $(this).find("td:eq(" + ColIndexremarks + ") textarea").val();
                        }
                    });

                }
                $("#divDatas").data("ItemList", AuditJson);
                BindGrid();
                //                $("[id$=ItemCode]").val("0");
                //                $("[id$=ItemTXTCode]").val("Translate(Select)");
                //                $("[id$=MaterialType]").val("0");
                $("#divBatch").hide();
                //                $("[id$=StkBatchNoPK]").val("0");
                //                if ($("[id$=hdfEnableBatch]").val() == "1") {
                //                    $("[id$=StkBatchNo]").val("Translate(Select)");
                //                }
            });
        }
        else {
            GrandScriptUtils.ShowModal("Translate(MaterialAlreadyAdded)", StoreAudit.MessageBoxTitle, "EXIST", false);
        }
    }
    return false;
}

function MakeNumeric(event, AllowDot, value, control) {

    var Qty = 0;
    var qtyArray;
    var keyCode = event.keyCode ? event.keyCode : event.which;
    if (keyCode != 39 && keyCode != 37 && keyCode != 8 && keyCode != 9 && keyCode != 46) {       
        var regx = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
        if (!(regx.test(value))) {
            qtyArray = String(value).split('.');
            if (qtyArray.length > 1) {
                if (qtyArray[1] != "" && qtyArray[1] != "0") {
                    if (qtyArray[0].length <= 8 && qtyArray[1].length <= QtyDec) {
                    }
                    else {
                        if (qtyArray[0].length > 8) {
                            $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1) + "." + qtyArray[1]);
                        }
                        else {
                            $("input[id$=" + control.id + "]").val(qtyArray[0] + "." + qtyArray[1].slice(0, qtyArray[1].length - 1));
                        }
                    }
                }
                else {
                    if (qtyArray[0] == "" && qtyArray[1] == "") {
                        $("input[id$=" + control.id + "]").val(".");
                    }
                    else if (qtyArray[0] == "" && qtyArray[1] != "") {
                        $("input[id$=" + control.id + "]").val("." + qtyArray[1]);
                    }
                    else {
                        $("input[id$=" + control.id + "]").val(value);
                    }
                }
            }
            else {
                if (qtyArray[0].length > 8) {
                    $("input[id$=" + control.id + "]").val(qtyArray[0].slice(0, qtyArray[0].length - 1));
                }
                else {
                    $("input[id$=" + control.id + "]").val(value);
                }
            }
        }
    }
}

function AfterGridBind(grdID) {

    if (grdID == $("#grdItemList").attr("id")) {
        var colIndex = 0;
        var ColIndexremarks = 0;
        var reqQty = "";
        var remarks = "";
        var txtBoxReqQty;
        var txtRemarks;
        var damageText = "";
        var damageColindex = 0;
        var ledgerQty = 0;
        var batchIndex = 0;
        var batch = "";
        var fieldValue = 0;
        var ActualStkVal = "";
        $("#grdItemList").find("tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
            reqQty = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));

            ledgerQty = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
            ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", $("#grdItemList").attr("id"));
            remarks = GrandGrid.Utilities.GetColumnValue($(this), "SAD_REMARKS", $("#grdItemList").attr("id")) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "SAD_REMARKS", $("#grdItemList").attr("id"));
            if (colIndex != null) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (parseInt($("[id$=SAH_STATUS]").val()) != 1 && parseInt($("[id$=SAH_STATUS]").val()) != 7 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=SAH_STATUS]").val()) != 3) {
                    $(this).find("td:eq(" + colIndex + ")").append("<input type=\"text\"  id=\"txtACT_STK_" + index + "\" onblur=\"javascript:return ChcekStockWithAuditStock(" + index + "); \" value=\"" + parseFloat(reqQty).toFixed(QtyDec) + "\" class=\"numeric input-w75\"  maxlength=\"12\" TabIndex=\"5\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\"    onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"   />");
                }
                else {
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(reqQty).toFixed(QtyDec));
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
            if (colIndex != null) {
                ActualStkVal = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (parseInt($("[id$=SAH_STATUS]").val()) != 1 && parseInt($("[id$=SAH_STATUS]").val()) != 7 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=SAH_STATUS]").val()) != 3) {
                    //                    $(this).find("td:eq(" + colIndex + ")").append("<input type=\"text\"  id=\"txtACT_STK_VAL_" + index + "\" onblur=\"javascript:return ChcekStockWithAuditStock(" + index + "); \" value=\"" + ActualStkVal + "\"  class=\"numeric input-w75\" maxlength=\"12\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\"    onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"   />");
                    $(this).find("td:eq(" + colIndex + ")").append("<input type=\"text\"  id=\"txtACT_STK_VAL_" + index + "\" value=\"" + parseFloat(ActualStkVal).toFixed(QtyDec) + "\"  class=\"numeric input-w75\" maxlength=\"12\"  TabIndex=\"5\"  onkeyup=\"javascript:MakeNumeric(event,true,$(this).val(), this);\"    onkeypress=\"javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);\"   />");
                }
                else {
                    $(this).find("td:eq(" + colIndex + ")").html(ActualStkVal);
                }
            }
            //New code 17-01-2014
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id"));
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id")).replace(/[^0-9\.]+/g, "");
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }

            if (ColIndexremarks != null) {
                if (parseInt($("[id$=SAH_STATUS]").val()) != 1 && parseInt($("[id$=SAH_STATUS]").val()) != 7 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=SAH_STATUS]").val()) != 3) {
                    txtRemarks = document.createElement("TEXTAREA");
                    txtRemarks.id = "txtRemarks" + index;
                    $(txtRemarks).css({ "width": "85%", "height": "30px" });
                    $(txtRemarks).attr("tabindex", "5");
                    $(txtRemarks).text(remarks);
                    $(this).find("td:eq(" + ColIndexremarks + ")").html("");
                    $(this).find("td:eq(" + ColIndexremarks + ")").append($(txtRemarks));
                }
                else {
                    $(this).find("td:eq(" + ColIndexremarks + ")").html(remarks);
                }
            }
            damageColindex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_Damage", $("#grdItemList").attr("id"));
            if (parseFloat(ledgerQty) > parseFloat(reqQty)) {
                if (parseInt($("[id$=SAH_STATUS]").val()) != 1 && parseInt($("[id$=SAH_STATUS]").val()) != 7 && parseInt($("[id$=ViewStatus]").val()) == 0 && parseInt($("[id$=SAH_STATUS]").val()) != 3) {
                    damageText = "Add details";
                }
                else {
                    damageText = "View details";
                }
            }
            else {
                damageText = " ";
            }
            if (damageColindex != null) {
                $(this).find("td:eq(" + damageColindex + ")").html("");
                $(this).find("td:eq(" + damageColindex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'ADDDAMAGE');\">" + damageText + "</a>");
            }
            batchIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ITEM_BATCH_TEXT", $("#grdItemList").attr("id"));
            if (batchIndex != null) {
                batch = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ITEM_BATCH", $("#grdItemList").attr("id"));
                if (batch == "0") {
                    $(this).find("td:eq(" + batchIndex + ")").html("");
                }
            }
            //Avoiding Undefined in Batch No field
            batchIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_STK_BATCH_NO", $("#grdItemList").attr("id"))
            if (batchIndex != null) {
                batch = GrandGrid.Utilities.GetColumnValue($(this), "SAD_STK_BATCH_NO", $("#grdItemList").attr("id"));
                if (batch == "undefined") {
                    $(this).find("td:eq(" + batchIndex + ")").html("");
                }
            }

        });
        if ($("[id$=ViewStatus]").val() == "1") {
            if ($("#grdItemList").find("tr:has(td)").length > 0) {
                $("#grdItemList").find("tr").each(function () {
                    $(this).find("td:last,th:last").hide();
                });
            }
        }
        else {
            if (parseInt($("[id$=SAH_STATUS]").val()) == 1 || parseInt($("[id$=SAH_STATUS]").val()) == 2 || parseInt($("[id$=SAH_STATUS]").val()) == 3 || parseInt($("[id$=SAH_STATUS]").val()) == 7) {
                if ($("#grdItemList").find("tr:has(td)").length > 0) {
                    $("#grdItemList").find("tr").each(function () {
                        $(this).find("td:last,th:last").hide();
                    });
                }
            }
        }
    }
    if (grdID == $("#grdDamageDtls").attr("id")) {
        if ($("[id$=ViewStatus]").val() == "1" || parseInt($("[id$=SAH_STATUS]").val()) == 1 || parseInt($("[id$=SAH_STATUS]").val()) == 2 || parseInt($("[id$=SAH_STATUS]").val()) == 3 || parseInt($("[id$=SAH_STATUS]").val()) == 7) {
            $("#grdDamageDtls").find("tr").each(function () {
                $(this).find("td:last,th:last").hide();
            });
        }

        //
        $("#grdDamageDtls").find("tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_DMG_QTY", $("#grdDamageDtls").attr("id"));
            fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SDD_DMG_QTY", $("#grdDamageDtls").attr("id"));
            if (colIndex != null && fieldValue != "") {
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(fieldValue).toFixed(QtyDec)));
            }

        });
    }
}


function ChcekStockWithAuditStock(indx) {
    //## Sumesh
    ///<summary>Function check any variation occur between actual stock and audit stock</summary>
    var grdID;
    var actualStockIndx = 0;
    var actualStock = 0;
    var ledgerStock = 0;
    var adnlQty = 0;
    var actionIndex = 0;
    var damageIndx = 0;
    var actValIndex = 0;
    var actVal = 0;
    var ledgerVal;
    var ledgerStock;
    var colIndex = 0;
    ClearDamageDetailsList(indx);
    $("#grdItemList tr:has(td)").each(function (index) {
        grdID = $(this).parents("table:first").attr("id");
        if (indx == index) {
            actualStockIndx = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", grdID);
            actualStock = $(this).find("td:eq(" + actualStockIndx + ") input").val();
            ledgerStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", grdID).replace(/[^0-9\.]+/g, "");
            damageIndx = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_Damage", grdID);



            if (actualStock == null || actualStock == undefined || actualStock == "") {
                actualStock = 0;
            }
            if (ledgerStock == null || ledgerStock == undefined || ledgerStock == "") {
                ledgerStock = 0;
            }
            //Calculation for actual Value
            actValIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", grdID);
            //Ledger Val
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK_VAL", grdID);
            if (colIndex != null) {
                ledgerVal = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK_VAL", $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK", grdID);
            if (colIndex != null) {
                ledgerStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            }
            if (parseFloat(ledgerVal) > 0 && parseFloat(ledgerStock) > 0) {
                var result = (parseFloat(ledgerVal) / parseFloat(ledgerStock)) * parseFloat(actualStock);
                $(this).find("td:eq(" + actValIndex + ") input").val(parseFloat(result).toFixed(AmtDec));
            }
            if (parseFloat(actualStock) < parseFloat(ledgerStock)) {
                $(this).find("td:eq(" + damageIndx + ") a").html("Add Details");
                $("[id$=hdfVariations]").val(parseFloat(ledgerStock) - parseFloat(actualStock));


            }
            else {
                $(this).find("td:eq(" + damageIndx + ") a").html("");
            }
        }
    });
    return false;
}

function CheckVariation(tr) {
    var grdID;
    var actualStockIndx = 0;
    var actualStock = 0;
    var ledgerStock = 0;
    var adnlQty = 0;
    var actionIndex = 0;
    var damageIndx = 0;
    var actValIndex = 0;
    var actVal = 0;
    var ledgerVal;
    var ledgerStock;
    var colIndex = 0;
    $("#grdItemList tr:has(td)").each(function (index) {
        if (GrandGrid.Utilities.GetColumnValue($(tr), "SAD_ITEM", $("[id$=grdItemList]").attr("id")) == GrandGrid.Utilities.GetColumnValue($(this), "SAD_ITEM", grdID)) {
            grdID = $(this).parents("table:first").attr("id");
            actualStockIndx = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", grdID);
            actualStock = $(this).find("td:eq(" + actualStockIndx + ") input").val();
            ledgerStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", grdID).replace(/[^0-9\.]+/g, "");
            damageIndx = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_Damage", grdID);
            if (actualStock == null || actualStock == undefined || actualStock == "") {
                actualStock = 0;
            }
            if (ledgerStock == null || ledgerStock == undefined || ledgerStock == "") {
                ledgerStock = 0;
            }
            //Calculation for actual Value
            actValIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", grdID);
            //Ledger Val
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK_VAL", grdID);
            if (colIndex != null) {
                ledgerVal = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK_VAL", $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK", grdID);
            if (colIndex != null) {
                ledgerStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $(this).parents("table:first").attr("id")).replace(/[^0-9\.]+/g, "");
            }
            if (parseFloat(actualStock) < parseFloat(ledgerStock)) {
                $("[id$=hdfVariations]").val(parseFloat(ledgerStock) - parseFloat(actualStock));
            }
            else
                $("[id$=hdfVariations]").val("0");
        }
    });
    return false;
}

function ClearDamageDetailsList(indx) {
    AuditJson = $("#divDatas").data("ItemList");
    var damageList = new Object();
    for (var i in AuditJson.ItemList) {
        if (parseInt(i) == indx) {
            damageList = AuditJson.ItemList[i];
        }
    }
    if (damageList.DamageStock != null && damageList.DamageStock != undefined) {
        if (damageList.DamageStock.length > 0) {
            damageList.DamageStock = null;
        }
    }
}

function ClearMaterialDetails() {
    AuditJson = $("#divDatas").data("ItemList");
    AuditJson.ItemList = new Array();
    $("#divDatas").data("ItemList", AuditJson);
    BindGrid();  
    $("[id$=MaterialCategoryPK]").val("0");
    $("[id$=MaterialCategory]").val("Translate(Select)");
    FillMaterialCategoryAutoComplete();
    $("[id$=ItemCode]").val("0");
    $("[id$=ItemTXTCode]").val("Translate(Select)");
    FillItem(0);
    $("[id$=StkBatchNoPK]").val("0");
    if ($("[id$=hdfEnableBatch]").val() == "1") {
        $("[id$=StkBatchNo]").val("Translate(Select)");
    }
    $("[id$=SAHTXT_DATE]").removeAttr("disabled");
}

function CalculateDamageExceeded() {
    var damageList = new Object();
    var damageQty = 0;
    var varience = parseFloat($("[id$=hdfVariations]").val()).toFixed(QtyDec);
    AuditJson = $("#divDatas").data("ItemList");
    for (var i in AuditJson.ItemList) {
        if (AuditJson.ItemList[i].SAD_ITEM == itemPKD) {
            damageList = AuditJson.ItemList[i];
        }
    }
    if (damageList.DamageStock.length > 0) {
        for (var j in damageList.DamageStock) {
            var qty = damageList.DamageStock[j].SDD_DMG_QTY;
            if (qty == null || qty == undefined || qty == '') {
                qty = 0;
            }
            damageQty = damageQty + parseFloat(qty);
        }
    }
    if ((damageQty + parseFloat($("[id$=SDD_DMG_QTY]").val()) - parseFloat($("[id$=PrvQuantity]").val())) > varience) {
        return false;
    }
    else {
        return true;
    }

}


function AddDamageDetails() {
    AddValidations(2)
    if ($(document.forms[0]).valid()) {
        if (CalculateDamageExceeded()) {
            var damageList = new Object();
            AuditJson = $("#divDatas").data("ItemList");
            for (var i in AuditJson.ItemList) {
                if (AuditJson.ItemList[i].SAD_ITEM == itemPKD) {
                    damageList = AuditJson.ItemList[i];
                }
            }
            if (!($.isArray(damageList.DamageStock))) {
                var objArray;
                if (damageList.DamageStock != undefined) {
                    objArray = damageList.DamageStock;
                    damageList.DamageStock = new Array();
                    damageList.DamageStock.push(objArray);

                }
                else {
                    objArray = damageList.DamageStock;
                    damageList.DamageStock = new Array();
                }
            }


            var damageType = $("[id$=SDD_DMG_TYPE]").val();
            var damageDetails = new Object();
            var flag = true;
            if (DamageType == "0") {
                for (var i in damageList.DamageStock) {
                    if (damageList.DamageStock[i].SDD_DMG_TYPE == damageType) {
                        flag = false;
                        break;
                    }
                    else {

                    }
                }
            }
            if (flag) {
                if (DamageType == "0") {
                    damageDetails.SDD_PK = DamageType;
                    damageDetails.SDD_ITEM = itemPKD;
                    damageDetails.SDD_DMG_TYPE = $("[id$=SDD_DMG_TYPE]").val();
                    damageDetails.SDD_DMG_TYPE_TEXT = $("[id$=SDD_DMG_TYPE] :selected").text();
                    damageDetails.SDD_DMG_QTY = $("[id$=SDD_DMG_QTY]").val();
                    damageList.DamageStock.push(damageDetails);
                }
                else {
                    for (var i in damageList.DamageStock) {
                        if (DamageType == damageList.DamageStock[i].SDD_DMG_TYPE)
                            damageDetails = damageList.DamageStock[i];
                        damageDetails.SDD_DMG_QTY = $("[id$=SDD_DMG_QTY]").val();
                    }
                }
                GrandGrid.MakeGrid($("#grdDamageDtls"), 0, damageList.DamageStock);
                ClearDamageDetails();
            }
            else {
                GrandScriptUtils.ShowModal("Damage type already added for this item", "Information", "EXIST", false);
            }
        }
        else {
            GrandScriptUtils.ShowModal("Damage quantity exceeded than variance", "Information", "EXIST", false);
        }

    }

    return false;
}




function ClearDamageDetails() {
    DamageType = 0;
    $("select[id$=SDD_DMG_TYPE]").val("0");
    $("input[id$=SDD_DMG_QTY]").val("");
    $("select[id$=SDD_DMG_TYPE]").attr("disabled", false);
    $("[id$=PrvQuantity]").val("0");
    return false;
}

function ChangeBatchMode() {

//    if (parseInt($("[id$=MaterialCategoryPK]").val()) > 1) {

//        $("#divBatch").show();
//        var drpID = $("select[id$=BatchNo]").attr("id");
//        GrandScriptUtils.FillDropDown(drpID, new Object(), true, true);
//    }
//    else {
        $("#divBatch").hide();
        $("[id$=ItemTXTCode]").val("Translate(Select)");
        $("[id$=ItemCode]").val("0");
   // }
}

function AfterAutoCompleteSelect(targetControlID) {

    if (targetControlID == "MaterialCategory") {
        ChangeBatchMode();
        FillItem($("[id$=MaterialCategoryPK]").val());
    }
    if (targetControlID == "ItemTXTCode") {
        FillMaterialDetails($("[id$=ItemCode]").val());
        if (parseInt($("[id$=MaterialCategoryPK]").val()) > 1) {
            FillBatchNumber($("[id$=MaterialCategoryPK]").val(), $("[id$=ItemCode]").val());
        }
        //Stk Batch No Filling
        $("[id$=StkBatchNoPK]").val("0");
        FillStkBatchNoAutoComplete();

    }
}

function FillMaterialDetails(materialID) {
    ///<summary>Function Used Fill the material Details corresponding to the id </summary>  
    $.get(StoreAudit.GetMaterialDetails + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID, function (data) {
        if (data) {
            if (materialID != 0) {
                if ($("[id$=MaterialCategoryPK]").val() == 0) {
                    $("[id$=MaterialCategoryPK]").val(data[0].ITM_CATEGORY);
                    $("[id$=MaterialCategory]").val(data[0].ITC_NAME);
                }
            }           
        }
    });
}
function FillBatchNumber(catType, itemPK) {

    var drpID = $("select[id$=BatchNo]").attr("id");
    $.get("CompoundPreparation.do?Action=GetCategoryItemBatch&ItemType=" + catType + "&ItemID=" + itemPK, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}

//Fill Stk Batchno
//<summary> Function Used to make Batch No field as auto complete </summary>
function FillStkBatchNoAutoComplete() { 
    var drpID = $("[id$=StkBatchNo]").attr("id");
    var date = $("[id$=SAHTXT_DATE]").val();   
    if ($("[id$=hdfEnableBatch]").val() == "0") {
        $("[id$=StkBatchNo]").attr("disabled", true);
    }
    else {
         $("[id$=StkBatchNo]").attr("disabled", false);
         var isChecked = $("input[id$=chkbxZeroQtyBatches]").is(':checked');
         if (isChecked) {//For Showing Zero Qty Batches
             GrandScriptUtils.MakeAutoComplete("StkBatchNo", StoreAudit.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=ItemCode]").val() + "&DepartmentID=" + $("select[id$=SAH_DEPT_STORE]").val() + "&Date=" + date + "&ZeroQty=1" + "&transDate=" + $("input[id$=SAHTXT_DATE]").val(), "StkBatchNoPK", true, false, "BizUnitPk", true, afterAutoComplete);
         }
         else {
             GrandScriptUtils.MakeAutoComplete("StkBatchNo", StoreAudit.FillBatchNoDropDownURL + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=ItemCode]").val() + "&DepartmentID=" + $("select[id$=SAH_DEPT_STORE]").val() + "&Date=" + date + "&ZeroQty=0" + "&transDate=" + $("input[id$=SAHTXT_DATE]").val(), "StkBatchNoPK", true, false, "BizUnitPk", true, afterAutoComplete);
         }
    }
}
function afterAutoComplete() {
//    $("[id$=ICH_ISS_RCV_PK]").val(selectVal);
//    $("[id$=txtIssueTo]").val(selectText);
}
function AfterDateSelect(ctrl) {
    if (ctrl = 'SAHTXT_DATE')
        FillStkBatchNoAutoComplete();
}

function FillCompany(selectVal) {
    if (selectVal == undefined || selectVal == 0) {
        var drpID = $("select[id$=SAH_COMPANY]").attr("id");
        $.get(StoreAudit.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
            var selCompany = $("[id$=hdfSelCompany]").val();
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selCompany);
            // $("#ICH_COMPANY").val(selCompany);
        });
    }
    else {
        var drpID = $("select[id$=SAH_COMPANY]").attr("id");
        $.get(StoreAudit.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
            GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        });
    }
}

//Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}

//For checking selected date is a future date or not
//command=>Draft,SaveandSubmit
function ShowFutureDate(command) {

    var msgTitle;
    var msg;
    msgTitle = StoreAudit.MessageBoxTitle;
    msg = StoreAudit.ContFutureDateMsg;
    $("#divConfirmation").html(msg).dialog({
        modal: true,
        height: 150,
        width: 350,
        title: msgTitle,
        resizable: false,
        buttons: {
            Yes: function (e) {
                $("[id$=hdfIsContFutureDate]").val(1);
                $(this).dialog("close");
                if (command == "Draft") {
                    $("[id$=btnSave]").click();
                }
                else {
                    $("[id$=btnSaveandSubmit]").click();
                }
            },
            Cancel: function (e) {
                $("[id$=hdfIsContFutureDate]").val(0);
                $(this).dialog("close");
                return false;
            }
        }
    });
    return false;
}
 
function EnableDisableDate() {
    var rowscount = $("#grdItemList tbody tr").length;
    if (rowscount > 0)
        $("[id$=SAHTXT_DATE]").attr("disabled", "disabled");
    else
        $("[id$=SAHTXT_DATE]").removeAttr("disabled");
}