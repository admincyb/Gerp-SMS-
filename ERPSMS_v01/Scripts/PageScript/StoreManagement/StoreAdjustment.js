var AuditJson = new Object();
var QtyDec, AmtDec;

//#region ---------- Configuration Section-------

var StoreAdjustment = {
    FillStoreDropdownURL: "StoreRequisitionSlip.do?Action=GetStores&SBUPk=",
    BizUnitPk: 0,
    AdjustmentList: new Array(),
    AdjObj: new Object(),
    InformationMessageBoxTitle: "Translate(Information)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)"
}

///#endregion

///#region ---------- Global Variable Declaration
var AuditJson = new Object();
var itemPK = 0;
///#endregion

///#region----------- Initialization Section ----------------

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    QtyDec = $("[id$='hdfQtyDecimal']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    PageInit();
});


function PageInit() {
    FillStore(0);
    AuditJson = $.parseJSON($("[id$=ItemList]").val());
    $("#divDatas").data("ItemData", AuditJson);
    var queryStr = window.location.search.substring(1);
    if (queryStr != "") {
        var queryStr = queryStr.split("&")
        for (var i = 0; i < queryStr.length; i++) {
            var pK = queryStr[i].split("=");
            if ((pK[1] != "" && pK[0] == "PK") || (pK[1] != "" && pK[0] == "RefID")) {
                FillStoreAuditDetails(AuditJson);
            }
            else
            {
           // FillStore(0);
            }
        }
    }
    else {
        GrandGrid.Utilities.ResetGrid(true, "grdItemList");
        //AuditJson.ItemList = new Array();
        //GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);

    }
}

///#endregion

//#region ---------- Core Section-------

    function FillStoreAuditDetails(AuditJson) {

        $("input[id$=SANO]").val(AuditJson.SANO);
        $("input[id$=DATE]").val(AuditJson.SAH_DATE);
        $("select[id$=Store]").val(AuditJson.SAH_DEPT_STORE);
         //FillStore(0);
        // Check Array Have atleast One Record-Check Is Array or Not
        if (!($.isArray(AuditJson.ItemList))) {
            var objArray = AuditJson.ItemList;
            AuditJson.ItemList = new Array();
            AuditJson.ItemList.push(objArray);
        }
        GrandGrid.MakeGrid($("#grdItemList"), 0, AuditJson.ItemList);

    }
    //BindWorkFlowComment();

    function FillStore(selectVal) {
        var drpID = $("select[id$=SAH_DEPT_STORE]").attr("id");
        $.get("StoreRequisitionSlip.do?Action=GetStores&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Flag=1", function (data) {
           // AuditJson = $("#divPOData").data("POData");
            GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        });
    }

    function getStoreMaterialDetailsAfterAdjustment() {
        ///<summary>Function used to add the needed po material list</summary>

        var grdID;
        var ledgerColIndex = 0;
        var remkColIndex = 0;
        var isValid = true;
        if ($("#grdItemList").css("visibility") == "visible") {
            $("#grdItemList tr:has(td)").each(function () {
                grdID = $(this).parents("table:first").attr("id");
                StoreAdjustment.AdjObj = new Object();
                StoreAdjustment.AdjObj.SAD_PK = GrandGrid.Utilities.GetColumnValue($(this), "SAD_PK", grdID);
                StoreAdjustment.AdjObj.SAD_STK_AUD = GrandGrid.Utilities.GetColumnValue($(this), "SAD_STK_AUD", grdID);
                StoreAdjustment.AdjObj.SAD_ITEM = GrandGrid.Utilities.GetColumnValue($(this), "SAD_ITEM", grdID);
                StoreAdjustment.AdjObj.DEPT_PK = GrandGrid.Utilities.GetColumnValue($(this), "DEPT_PK", grdID);
                StoreAdjustment.AdjObj.SL_NO = GrandGrid.Utilities.GetColumnValue($(this), "SL_NO", grdID);
                StoreAdjustment.AdjObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue($(this), "ITM_NAME", grdID);
                StoreAdjustment.AdjObj.ITM_CODE = GrandGrid.Utilities.GetColumnValue($(this), "ITM_CODE", grdID);
                StoreAdjustment.AdjObj.DEPT_NAME = GrandGrid.Utilities.GetColumnValue($(this), "DEPT_NAME", grdID);
                ledgerColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_LED_STK", grdID);
                StoreAdjustment.AdjObj.SAD_LED_STK = $(this).find("td:eq(" + ledgerColIndex + ") input[type=text]").val();
                StoreAdjustment.AdjObj.SADD_ACT_STK = GrandGrid.Utilities.GetColumnValue($(this), "SADD_ACT_STK", grdID);
                remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", grdID);
                StoreAdjustment.AdjObj.SAD_REMARKS = $(this).find("td:eq(" + remkColIndex + ") input[type=text]").val();
                StoreAdjustment.AdjustmentList.push(StoreAdjustment.AdjObj);
            });
        }
        return StoreAdjustment.AdjustmentList;
    }

    function SavePage() {
        StoreAdjustment.AdjustmentList = new Array();
        StoreAdjustment.AdjustmentList = getStoreMaterialDetailsAfterAdjustment();
       
        $("[id$=AdjustmentList]").val(JSON.stringify(StoreAdjustment.AdjustmentList));
        if (command != "Draft")
            $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
        else
            $("[id$=ActionID]").val('0'); // save only.
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        //RemoveValidations();
        //AddValidations(2);
        if ($(document.forms[0]).valid()) {

//            //To Prevent Duplicate Submission
//            if ($("[id$=SubmitFlag]").val() == "0")
//                $("[id$=SubmitFlag]").val('1')
//            else
//                return false;

            $.post("StoreAdjustment.do?Action=SavePage", jSonString, function (data) {
                if (parseInt(data) > 0) {
                    var SaveMessageWithPRNo = "";
                    SaveMessageWithPRNo = "";// StoreAdjustment.SaveMessage1 + " " + $("[id$=GRH_NO]").html() + " " + StoreAdjustment.SaveMessage2;
                    //GrandScriptUtils.ShowModal(SaveMessageWithPRNo, StoreAdjustment.InformationMessageBoxTitle, "Save");
                }
                else if (parseInt(data) == -1) {
                    //GrandScriptUtils.ShowModal(StoreAdjustment.ActionFailedMessage, StoreAdjustment.InformationMessageBoxTitle);
                }
                else if (parseInt(data) == -2) {
                    //GrandScriptUtils.ShowModal(StoreAdjustment.SaveMessage1 + " " + $("[id$=GRH_NO]").html() + " " + StoreAdjustment.EditUsedByAnotherUser, StoreAdjustment.InformationMessageBoxTitle, StoreAdjustment.SAVE);
                }
                else if (parseInt(data) == -3) {
                    //GrandScriptUtils.ShowModal(StoreAdjustment.GRNQtyValid, StoreAdjustment.InformationMessageBoxTitle);
                }
                else {
                    GrandScriptUtils.ShowModal(StoreAdjustment.ActionFailedMessage, StoreAdjustment.InformationMessageBoxTitle);
                }
            });
        }
        return false;
    
    }

//    function BindWorkFlowComment() {
//    ///<summary>To handle bind grid </summary>

//    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
//}

function ModalOk(command) {
    switch (command) {
        case "Save":
            window.location = "StoreAuditLsiting.aspx";
            break;
    }
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>

    window.location = "StoreAuditLsiting.aspx";
    return false;
}


function AfterGridBind(grdID) {

    if (grdID == "grdItemList") {
        var balColIndex = 0;
        var balance = "";
        var txtBoxBalance;
        var remkColIndex = 0;
        var remarks = "";
        var txtBoxRemarks;
        $("#grdItemList tr:has(td)").each(function (index) {
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), "SAD_REMARKS", grdID);
            remarks = GrandGrid.Utilities.GetColumnValue($(this), "SAD_REMARKS", grdID);
            if (remkColIndex != null) {
                txtBoxRemarks = document.createElement("input");
                txtBoxRemarks.id = "txtRemarks_" + index;
              
                txtBoxRemarks.onkeyup = function MakeNumeric(event) {
                    $(txtBoxRemarks).next("div.error").remove();
                }
                $(txtBoxRemarks).attr("type", "text");
                $(txtBoxRemarks).css({ "width": "85%" });
                $(this).find("td:eq(" + remkColIndex + ")").html("");
                $(this).find("td:eq(" + remkColIndex + ")").append($(txtBoxRemarks));
            }
           

        });
    }

}
///#endregion

//#region----------- Validation Section----------------

function AddValidations(mode) {
    //<summary>Function used to assign validation</summary>
        $("[id$=SAH_DEPT_STORE]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Select Store" }
        });
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>
    $(document.forms[0]).validate().resetForm();
    $("[id$=SAH_DEPT_STORE]").rules("remove");

}

//#endregion