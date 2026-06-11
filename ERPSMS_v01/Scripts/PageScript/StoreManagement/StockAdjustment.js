
///#region ---------- Global Variable Declaration
var AuditJson = new Object();
var itemPK = 0;
var QtyDec, AmtDec;
var StockAdjustment = {
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    SaveMessage: "Translate(StockAdjustmentSaved)",
    SubmitMessage: "Translate(StockAdjustmentSubmit)",
    Title: "Translate(StockAdjustment)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    CannotReceivePriorDateSendReceive: "Translate(CannotReceivePriorDateSendReceive)",    
    EditbyAnotherUser:"Translate(EditbyAnotherUser)",
    BizUnitPk: 0,
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompany&SBUPk="
}

///#endregion

//#region Inititalization Section
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
    PageInit();   
});

function PageInit() {
    $("[id$=ConfirmStockValueChange]").val('0');
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
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID") || (pK[1] != "" && pK[0] == "PRefID")) {
                FillStoreAuditDetails(AuditJson);
            }
        }
    }
    $("[id*=txtRemarks0]").focus();
    
}

function FillStoreAuditDetails(AuditJson) {

    $("input[id$=SAH_PK]").val(AuditJson.SAH_PK);
    if (AuditJson.SDH_PK == undefined)
        $("input[id$=SDH_PK]").val("0");
    else
        $("input[id$=SDH_PK]").val(AuditJson.SDH_PK);

    $("[id$=SAHTXT_NO]").text(AuditJson.SAH_NO);
    $("[id$=SAH_NO]").val(AuditJson.SAH_NO);
    $("[id$=SAH_DATE]").val(AuditJson.SAH_DATE);
    $("[id$=SAHTXT_DATE]").text(AuditJson.SAH_DATE);
    $("[id$=SAH_DEPTTXT_STORE]").text(AuditJson.DPT_NAME);
    $("[id$=SAH_DEPT_STORE]").val(AuditJson.SAH_DEPT_STORE)
    FillCompany(AuditJson.SAH_COMPANY);
    // Check Array Have atleast One Record-Check Is Array or Not
    if (!($.isArray(AuditJson.ItemList))) {
        var objArray = AuditJson.ItemList;
        AuditJson.ItemList = new Array();
        AuditJson.ItemList.push(objArray);
    }
    GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);

}

//#endregion

//#region --------------------------COre Functions ------------------------------

function AfterGridBind(grdID) {
    var colIndex = 0;
    var ColIndexremarks = 0;
    var CurrentStock = "";
    var ActualStock = "";
    var Adjustment = "";
    var remarks = "";
    var txtBoxReqQty;
    var txtRemarks;
    var batchIndex = 0;
    var batch = "";
    var ColIndexremarks = 0;
    $("#grdItemList").find("tr:has(th)").each(function (index) {
        $(this).find("th:eq(0)").css({ "width": "5%" });
    });
    $("#grdItemList").find("tr:has(td)").each(function (index) {
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", $("#grdItemList").attr("id"));
        if ($(this).find("td:eq(" + ColIndexremarks + ")").html() == "null") {
            $(this).find("td:eq(" + ColIndexremarks + ")").html("");
        }

        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_QTY_ADJ", $("#grdItemList").attr("id"));
        CurrentStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
        ActualStock = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
        Adjustment = parseFloat(ActualStock - CurrentStock).toFixed(3);
        ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_REMARKS", $("#grdItemList").attr("id"));
        remarks = GrandGrid.Utilities.GetColumnValue($(this), "SDD_REMARKS", $("#grdItemList").attr("id")) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), "SDD_REMARKS", $("#grdItemList").attr("id"));
        if (colIndex != null) {
            $(this).find("td:eq(" + colIndex + ")").html(Adjustment);
        }

        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
        fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK", $("#grdItemList").attr("id"));
        if (colIndex != null && fieldValue != "") {
            $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(fieldValue).toFixed(QtyDec)));
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id"));
        fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_CUR_STK_VAL", $("#grdItemList").attr("id"));
        if (colIndex != null && fieldValue != "") {
            $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(fieldValue).toFixed(QtyDec)));
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
        fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK", $("#grdItemList").attr("id"));
        if (colIndex != null && fieldValue != "") {
            $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(fieldValue).toFixed(QtyDec)));
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
        fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ACT_STK_VAL", $("#grdItemList").attr("id"));
        if (colIndex != null && fieldValue != "") {
            $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(fieldValue).toFixed(QtyDec)));
        }
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_QTY_ADJ", $("#grdItemList").attr("id"));
        fieldValue = GrandGrid.Utilities.GetColumnValue($(this), "SDD_QTY_ADJ", $("#grdItemList").attr("id"));
        if (colIndex != null && fieldValue != "") {
            $(this).find("td:eq(" + colIndex + ")").html(addCommas(parseFloat(fieldValue).toFixed(QtyDec)));
        }

        if (ColIndexremarks != null) {
            txtRemarks = document.createElement("TEXTAREA");
            txtRemarks.id = "txtRemarks" + index;
            $(txtRemarks).css({ "width": "95%", "height": "30px" });
            if (remarks == null || remarks == undefined || remarks == "" || remarks == "undefined") {
                remarks = "";
            }
            $(txtRemarks).text(remarks);
            $(this).find("td:eq(" + ColIndexremarks + ")").html("");
//            txtRemarks.TabIndex = "1";
//            txtRemarks.MaxLength = "5";
//            txtRemarks.onkeypress = "return this.value.length<5";
//            $(this).find("td:eq(" + ColIndexremarks + ")").append("<input type=\"text\" style=\"width:95%; height:30px\"  TextMode=\"MultiLine\"  onkeypress=\"return this.value.length<500\"  id=\"txtRemarks" + index + "\"  value=\"" + remarks + "\" width=\"80%\"  maxLength =\"500\" />");
//            // onchange=\"javascript:MakeNumric(this," + recvQty + ");\"
            $(this).find("td:eq(" + ColIndexremarks + ")").append($(txtRemarks));

        }
        batchIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_ITEM_BATCH_TEXT", $("#grdItemList").attr("id"));
        if (batchIndex != null) {
            batch = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ITEM_BATCH", $("#grdItemList").attr("id"));
            if (batch == "0") {
                $(this).find("td:eq(" + batchIndex + ")").html("");
            }
        }
        //Batch number shows 'undefined' 
        colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_STK_BATCH_NO", $("#grdItemList").attr("id"));
        if (colIndex != null) {
            batch = GrandGrid.Utilities.GetColumnValue($(this), "SDD_STK_BATCH_NO", $("#grdItemList").attr("id"));
            if (batch == "undefined") {
                $(this).find("td:eq(" + colIndex + ")").html("");
            }
        }
    });
  
}

function SavePage(command) {

    var colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_QTY_ADJ", $("#grdItemList").attr("id"));
    var ColIndexremarks = GrandGrid.Utilities.GetColumnIndex($(this), "SDD_REMARKS", $("#grdItemList").attr("id"));
    $("select[id$=SDH_COMPANY]").attr("disabled", false);
    var dummyJson = new Object();
    AuditJson = $("#divDatas").data("ItemList");
    dummyJson = AuditJson;
    var count = 0;
    $("#grdItemList").find("tr:has(td)").each(function (index) {
        // var cflag = $(this).find("td:eq(0) input").attr("checked");
        var VSAD_PK = GrandGrid.Utilities.GetColumnValue($(this), "SAD_PK", $("#grdItemList").attr("id"));
        for (var i in dummyJson.ItemList) {
            // Check ConversionList[i].FromUnit  Equal to Selected ToUnit
            if (dummyJson.ItemList[i].SAD_PK == VSAD_PK) {
                //dummyJson.ItemList[i].SDD_QTY_ADJ = $(this).find("td:eq(" + colIndex + ") input[type=text]").val();
                dummyJson.ItemList[i].SDD_QTY_ADJ = GrandGrid.Utilities.GetColumnValue($(this), "SDD_QTY_ADJ", $("#grdItemList").attr("id")).replace(/[^0-9\.]+/g, "");
                dummyJson.ItemList[i].SDD_REMARKS = $(this).find("td:eq(" + ColIndexremarks + ") textarea").val();
                dummyJson.ItemList[i].SAD_REMARKS = $(this).find("td:eq(" + ColIndexremarks + ") textarea").val();
               
                break;
            }
        }
    });
    if (command != "Draft")
        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
    else
        $("[id$=ActionID]").val(GINCreate.ValueZero); // save only.
    if ($(document.forms[0]).valid()) {
        $("[id$=ItemList]").val(JSON.stringify(dummyJson.ItemList));
        $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
        var jSonString = GrandScriptUtils.FormToJsonString(false);

        //To Prevent Duplicate Submission
                    if ($("[id$=SubmitFlag]").val() == "0")
                        $("[id$=SubmitFlag]").val('1')
                    else
                        return false;

        $.post("StockAdjustmentManagement.do?Action=SaveStockAdjustment", jSonString, function (data) {
            if (parseInt(data) > 0) { //if (data != "" && data != "-1" && data != "-10") {
                if (command == "Draft") {
                    GrandScriptUtils.ShowModal(StockAdjustment.SaveMessage + $("[id$=SAH_NO]").val(), StockAdjustment.MessageBoxTitle, "SAVE");
                }
                else {// If action - WorkFlow Save
                    $("[id$=hdfAppID]").val(data);
                    $("[id$=AppNo]").val($("[id$=SAH_NO]").val());
                    SaveWorkFlow(true, 1); // 1- for Store adjustment
                    UpdateReferenceID(1);
                }
            }
            else if (data == "-1") {
                GrandScriptUtils.ShowModal(StockAdjustment.ActionFailedMessage, StockAdjustment.MessageBoxTitle);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (data == "-31") {
                 GrandScriptUtils.ShowModal(StockAdjustment.CannotReceivePriorDateSendReceive, StockAdjustment.MessageBoxTitle);
                //                fnConfirmStockValueChange(command);
                $("[id$=SubmitFlag]").val('0')
            }
            else if (data == "-3") {  // if any concurrency occur
                GrandScriptUtils.ShowModal(StockAdjustment.EditbyAnotherUser);
               $("[id$=SubmitFlag]").val('0');
            }
            else {
                GrandScriptUtils.ShowModal(StockAdjustment.Title + " " + $("[id$=SANo]").val() + " " + StockAdjustment.EditUsedByAnotherUser, StockAdjustment.MessageBoxTitle, "Save");
                $("[id$=SubmitFlag]").val('0')
            }
        });
    }

    return false;
}

function ModalOk(command) {
    ///<summary>Function invoke after Model popup ok Click</summary>
    /// <param name="command"  type="object">
    ///      delete
    /// </param>
    switch (command) {
        case "SAVE":
            window.location = "StockAdjustmentListing.aspx";
            return false;
            break;
    }
    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var msg = StockAdjustment.SubmitMessage + " " + $("[id$=AppNo]").val();
    GrandScriptUtils.ShowModal(msg, StockAdjustment.MessageBoxTitle, "SAVE");
}

//#endregion

 function FillCompany(selectVal) {
     ///<summary>function used to fill vendor to vendor drop down </summary>
     var drpID = $("select[id$=SDH_COMPANY]").attr("id");
     $.get(StockAdjustment.FillCompanyDropdownURL + $("[id$=BizUnitPk]").val() + "&Active=1", function (data) {
         GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
     });
 }

 //Comma Separation for Quantity & Amount 
 function addCommas(n) {
     return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
 }
