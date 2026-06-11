/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
var pohDefaultCompany = 0;
var GRNCreate = {
    GetCurrentDepartment: "CommonManagement.do?Action=GetCurrentDepartment",
    SaveGoodsReceiptNote: "GoodsReceiptNote.do?Action=SaveGoodsReceiptNote",
    GetGoodsReceiptNote: "GoodsReceiptNote.do?Action=GetGoodsReceiptNote&GRNPK=",
    PurchaseOrderBindGridURL: "GoodsReceiptNote.do?Action=GetPurchaseOrderPending&GRNID=",
    PreviousGRNBindGridURL: "GoodsReceiptNote.do?Action=GetPreviousGRN&poID=",
    PurchaseOrderAutoURL: "GoodsReceiptNote.do?Action=GetPendingSearchAuto&AUTOSEARCH=1",
    FillStoreDropdownURL: "StoreRequisitionSlip.do?Action=GetStoresByType&SBUPk=",
    FillVendorDropdownURL: "VendorRegistration.do?Action=GetPurchaseOrderVendors&SBUPk=",
    FillCompanyDropdownURL: "CommonManagement.do?Action=GetCompanyMappingDetails&BizUnit=",
    FillPODetailsView: "POGeneration.do?Action=GetPODetailsView&POPK=",
    GetPoVendor: "PurchaseOrderGenerate.do?Action=GetPOVendor&POID=",
    FillPREVGRNView: "GoodsReceiptNote.do?Action=GetPrevGRNDetailsView&POPK=",
    UomURL: "MaterialManagement.do?Action=GetUOMConvExistsByMaterial&MaterialPK=",
    GetMaterialUOMConversion: "CompoundMaster.do?Action=GetConversionFactor&UOMFrm=",
    GRNListUrl: "GRNList.aspx",
    InboxURL: "../AccountManagement/WorkflowInbox.aspx",

    INBOX: "INBOX",
    BizUnitPk: 0,
    POPK: 0,
    ItemPK: 0,
    GRNList: new Array(),
    GRNObj: new Object(),
    Confirmation: "Translate(Confirmation)",
    DeleteConfirmMsg: "Translate(Doyouwanttodeletethisdetails)",
    ItemAlreadyDeletedMsg: "Translate(Itemsalreadydeletedbyanotheruser)",
    RecordExist: "Translate(AlreadyExists)",
    ActionFailedMessage: "Translate(ActionFailedPleaseTryAgain)",
    MessageBoxTitle: "Translate(Information)",
    MsgRefAdded: "Translate(MsgRefAdded)",
    //    AddGoodsReceiptDetails: "Translate(AddGoodsReceiptDetails)",
    AddGoodsReceiptDetails: "Translate(GRNListMandatory)",
    ReceivedQtyExceed: "Translate(ReceivedQtyExceed)",
    SaveMessage1: "Translate(GoodsReceiptSaved1)",
    SaveMessage2: "Translate(GoodsReceiptSaved2)",
    SubmitMessage: "Translate(SubmittedMsg)",
    GRNSavedMessage: "Translate(GoodReceiptSavedSuccessfully)",
    GRNQtyValid: "Translate(GRNQtyValid)",
    SelectPOFromList: "Translate(SelectPOFromList)",
    MsgSameItemDiffRate: "Translate(MsgSameItemDiffRate)",
    EditUsedByAnotherUser: "Translate(EditUsedByAnotherUser)",
    DocGenerationNewValue: "Translate(DocGenerationNew)",
    GRNVendorRefDuplicate: "Translate(GRNVendorRefDuplicate)", //Vendor Ref Duplicate
    GRNQtyLessThanGINQty: "Translate(GRNQtyLessThanGINQty)",
    CannotDeleteHaveReference: "Translate(CannotDeleteHaveReference)",
    GRNQtyLessThanInvoiceQty: "Translate(GRNQtyLessThanInvoiceQty)",
    ContFutureDateMsg: "Translate(ContFutureDateMsg)",
    SessionExpired: "Translate(Msg_Dept_Session_Expired)",

    GRD_DOM: "GRD_DOM",
    POD_GRN_FLAG: "POD_GRN_FLAG",
    GRD_DOE: "GRD_DOE",
    GRD_PK: "GRD_PK",
    POH_PK: "POH_PK",
    POD_PK: "POD_PK",
    POD_ITEM: "POD_ITEM",
    POD_RATE: "POD_RATE",
    POH_NO: "POH_NO",
    ITM_NAME: "ITM_NAME",
    POD_QTY_APPROVED: "POD_QTY_APPROVED",
    POD_QTY_RECEIVED: "POD_QTY_RECEIVED",
    BALANCE_QTY: "BALANCE_QTY",
    ORG_BALANCE_QTY: "ORG_BALANCE_QTY",
    GRD_REMARKS: "GRD_REMARKS",
    GRD_VND_REF_NO: "GRD_VND_REF_NO",


    GRD_ITEM: "GRD_ITEM",
    GRD_PO: "GRD_PO",
    POD_UOM: "POD_UOM",
    GRD_UOM: "GRD_UOM",
    UOM_CODE: "UOM_CODE",
    GRD_QTY_RECEIVED: "GRD_QTY_RECEIVED",
    DELETE: "delete",
    POVIEW: "poview",
    EDIT: "edit",
    SAVE: "save",
    PreviousCommand: "",
    EmptyCommand: "",
    Continue: "Continue",
    PREVIOUSGRNVIEW: "previousgrnview",
    IsViewMode: false,
    IsModifyPR: false,
    LOGOUT: "LOGOUT",
    POPRINT: "poprint",
    POREPORTURL: "../Reports/GenerateReport.aspx"

}

var QtyDec, AmtDec, RateDec;
var hasExcedQty = false;
var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "GRN";


$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    $("[id$=WKF_PROCESS]").val($("[id$=hdfProcessID]").val());
    //Set Decimal Points For Qty and Amount
    QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
    AmtDec = $("[id$='hdfAmtDecimal']").val();
    RateDec = $("[id$='hdfRateDecimalDigitP2P']").val();

    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != "0");
    }, "Translate(Pleaseselectanoption)");
    PageInit();



});



function PageInit() {
    if ($("[id$=GRNList]").val() == "null") {
        GrandScriptUtils.ShowModal(GRNCreate.ItemAlreadyDeletedMsg, GRNCreate.InformationTtile, GRNCreate.SAVE);
    }
    else {
        ///<summary>initial page condition</summary>
        var queryString = window.location.search.substring(1);
        if (queryString != "") {
            var queryStr = queryString.split("&")
            for (var i = 0; i < queryStr.length; i++) {
                var pK = queryStr[i].split("=");
                if ((pK[1] == 1 && pK[0] == "Status") || (pK[1] == 1 && pK[0] == "Flag")) {
                    GRNCreate.IsViewMode = true;
                }
                else if (pK[1] == 1 && pK[0] == "IsModify") {
                    GRNCreate.IsModifyPR = true;
                    $("[id$=GRH_IS_EDIT]").val("1");
                }
            }
        }
        DateInit();

        Popup();
        GRNCreate.BizUnitPk = $("[id$=BizUnitPk]").val();
        var GRNObj = $.parseJSON($("[id$=GRNList]").val());
        GRNCreate.GRNList = GRNObj.GRNList;
        if (GRNObj.GRH_PK != undefined && GRNObj.GRH_PK > 0) {
            FillDetails(GRNObj);
        }
        else {
            //FileUploader Start
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GRN", true);
            //End
            FillCompany(0);
        }
        SetSearchType();
        if (GRNObj.GRH_DEPT == 0)
            FillStore($("[id$=hdfDeptID]").val());
        else
            FillStore(GRNObj.GRH_DEPT);
        if (GRNObj.GRH_VENDOR != 0) {
            FillVendors(GRNObj.GRH_VENDOR);
        }
        else {
            FillPOVendor();
        }
        FillPuchaseRequestAutoComplete();
        if (!$.isArray(GRNCreate.GRNList)) {
            GRNCreate.PurchaseRequestObj = GRNCreate.GRNList;
            GRNCreate.GRNList = new Array();
            GRNCreate.GRNList.push(GRNCreate.PurchaseRequestObj);
        }
        $("#divData").data("GRNData", GRNCreate.GRNList);
        GrandGrid.MakeGrid($("#grdPendingPOList"), 0, GRNCreate.GRNList);
        BindGrid();
        if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {
            $("[id$=GRH_COMPANY]").attr("disabled", "disabled");
        }
    }
}

function HidePendingOrder() {
    //<summary>Function Used to Hide Reorder Panel </summary>
    $("#imgPendingOrderHide").hide();
    $("#imgPendingOrderShow").show();
    $("#divPendingOrders").hide();
}

function ShowPendingOrder() {
    //<summary>Function Used to Show Reorder Panel </summary>
    $("#imgPendingOrderHide").show();
    $("#imgPendingOrderShow").hide();
    $("#divPendingOrders").show();
}

function HideGrnList() {
    //<summary>Function Used to Hide Reorder Panel </summary>
    $("#imgGrnListHide").hide();
    $("#imgGrnListShow").show();
    $("#divGrnList").hide();
}

function ShowGrnList() {
    //<summary>Function Used to Show Reorder Panel </summary>
    $("#imgGrnListHide").show();
    $("#imgGrnListShow").hide();
    $("#divGrnList").show();
}

function HideAttachment() {
    //<summary>Function Used to Hide Vendor Panel </summary>
    $("#imgHideAttachment").hide();
    $("#imgShowAttachment").show();
    $("#divAttachment").hide();
}

function ShowAttachment() {
    //<summary>Function Used to Show Purchase Request Panel </summary>
    $("#imgHideAttachment").show();
    $("#imgShowAttachment").hide();
    $("#divAttachment").show();
}

function FillFileDetails() {
    ///<summary>function used to fill the file details</summary>

    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + "." + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}



function DateInit() {
    ///<summary>initial Date condition</summary>
    GrandScriptUtils.DatePicker("GRH_DATE", false, false, false);
    GrandScriptUtils.DatePicker("GRH_VND_REF_DATE", false, false, false);
}

function Popup() {
    ///<summary>Function used for popup</summary>
    $("#divPreviousGRN").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });

    $("#divPODetails").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
}

function FillStore(selectVal) {
    ///<summary>to fill store combo</summary>
    var drpID = $("select[id$=GRH_DEPT]").attr("id");
    $.get(GRNCreate.FillStoreDropdownURL + GRNCreate.BizUnitPk + "&DeptType=3", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);
        $("[id$=GRH_DEPT]").attr("disabled", "disabled");
        BindGrid();
    });
}

function FillVendors(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=GRH_VENDOR]").attr("id");
    $.get(GRNCreate.FillVendorDropdownURL + GRNCreate.BizUnitPk + "&GRHPK=" + $("[id$=GRH_PK]").val() + "&ShipDeptID=" + $("[id$=hdfDeptID]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        BindGrid();
    });
}

function FillCompany(selectVal) {
    ///<summary>function used to fill vendor to vendor drop down </summary>
    var drpID = $("select[id$=GRH_COMPANY]").attr("id");
    var getURL = "";
    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 1) {//If Multiple plant, pass current department pk
        getURL = GRNCreate.FillCompanyDropdownURL + GRNCreate.BizUnitPk + "&Active=1&DeptPk=" + $("[id$=hdfDeptID]").val() + "&ApsPK=" + selectVal;
    }
    else {
        getURL = GRNCreate.FillCompanyDropdownURL + GRNCreate.BizUnitPk + "&Active=1";
    }
    $.get(getURL, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, false, selectVal);

        if (selectVal == undefined || selectVal == 0) {
            var selCompany = $("[id$=hdfCompany]").val();
            $("#GRH_COMPANY").val(selCompany);
        }
    });

}


///Fill PO Vendor
function FillPOVendor() {
    var popk = $("[id$=PO_PK]").val();
    if (popk != "0") {
        $.get(GRNCreate.GetPoVendor + popk, function (data) {
            FillVendors(data != null && data.length > 0 ? data[0].POH_VENDOR : 0);
        });
    }
    else
        FillVendors(0);
}
function FillUOM(catgID, selectVal) {
    ///<summary>function To Fill UOM Details </summary>
    var drpID = $("select[id$=UOM]").attr("id");
    $("[id$=UOMOrg]").val(selectVal);
    $.get(GRNCreate.UomURL + catgID, function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, selectVal);
        GetUomConversion(selectVal);
    });
}

function FillDetails(GRNObj) {
    //<summary> Function Used to Fill Details </summary>

    //Set a stamp for cancelled record
    if (GRNObj.GRH_STATUS == 4)
        $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
    else
        $("[id$=tblDetailHdr]").addClass("table-devide");

    //End
    //FileUploader Region Start
    if (GRNCreate.IsViewMode == false) {
        GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GRN", true);
    }
    else {
        if (parseInt($("[id$=GRH_IS_EDIT]").val()) == 1) {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GRN", true);
        }
        else {
            GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "GRN", false);
        }
    }
    //End FileUploader Region

    $("[id$=GRH_PK]").val(GRNObj.GRH_PK);

    if (GRNObj.GRH_NO == null || GRNObj.GRH_NO == "") {
        $("[id$=GRH_NO]").html(GRNCreate.DocGenerationNewValue);
    }
    else {
        $("[id$=GRH_NO]").html(GRNObj.GRH_NO);
    }

    $("[id$=GRH_STATUS]").val(GRNObj.GRH_STATUS);
    $("[id$=GRH_DATE]").val(GRNObj.GRH_DATE);
    $("[id$=LAST_MOD_DT]").val(GRNObj.GRH_MOD_DT);
    $("[id$=GRH_VND_REF_NO]").val(GRNObj.GRH_VND_REF_NO);
    $("[id$=GRH_VND_REF_DATE]").val(GRNObj.GRH_VND_REF_DATE);
    FillCompany(GRNObj.GRH_COMPANY);

    if (GRNCreate.IsViewMode) {
        $("[id$=GRH_VENDOR]").attr("disabled", "disabled");
        $("[id$=GRH_COMPANY]").attr("disabled", "disabled");
        $("[id$=GRH_DEPT]").attr("disabled", "disabled");
        $("[id$=GRH_DATE]").attr("disabled", "disabled");
        $("[id$=GRH_VND_REF_NO]").attr("disabled", "disabled");
        $("[id$=GRH_VND_REF_DATE]").attr("disabled", "disabled");
    }
    else if (GRNObj.GRH_STATUS == 1 || GRNObj.GRH_STATUS == 7 || GRNObj.GRH_STATUS == 11) {
        $("[id$=GRH_VENDOR]").attr("disabled", "disabled");
        $("[id$=GRH_COMPANY]").attr("disabled", "disabled");
        $("[id$=GRH_DEPT]").attr("disabled", "disabled");
        $("[id$=GRH_DATE]").attr("disabled", "disabled");
        $("[id$=GRH_VND_REF_NO]").attr("disabled", "disabled");
        $("[id$=GRH_VND_REF_DATE]").attr("disabled", "disabled");
        $("[id$=btnSave]").hide();
    }
    if (parseInt($("[id$=GRH_IS_EDIT]").val()) == 1) {
        $("[id$=GRH_DATE]").attr("disabled", false);
        $("[id$=GRH_VND_REF_NO]").attr("disabled", false);
        $("[id$=GRH_VND_REF_DATE]").attr("disabled", false);
    }
    //#region --------------- Fill File Upload Details----------------------------------
    if (!($.isArray(GRNObj.FILELIST))) {
        if (GRNObj.FILELIST != undefined) {
            objArray = GRNObj.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = GRNObj.FILELIST;
            FileJson.FILELIST = new Array();
        }
    }
    else {
        FileJson.FILELIST = GRNObj.FILELIST;
    }
    FillFileDetails();
    //#Endregion

}

function FillPuchaseRequestAutoComplete() {
    //<summary> Function Used to make material category field as auto complete </summary>
    GrandScriptUtils.MakeAutoComplete("SearchValue", GRNCreate.PurchaseOrderAutoURL, false, true, false, "SearchType", false, "GRH_VENDOR", "GRH_PK", "ddlSelectDiv3");
}

function GetUomConversion(UOMID, isChanged) {
    //<summary> Function Used to get the convertion factor </summary>
    if (UOMID != "0") {
        $.get(GRNCreate.GetMaterialUOMConversion + $("[id$=UOMOrg]").val() + "&UOMTo=" + UOMID, function (data) {
            if (data) {
                $("[id$=POD_CONV_FACT]").val(data);
                if (isChanged)
                    $("[id$=QtyRecvd]").val((parseFloat(data) * parseFloat($("[id$=QtyRecvdOrg]").val())).toFixed(QtyDec));
            }
        });
    }
    else {
        $("[id$=POD_CONV_FACT]").val("1");
    }
}

//#region----------- Validation Section----------------
function RemoveAllValidations() {
    //<summary>function used Remove validation </summary>

    var settings = $(document.forms[0]).validate().settings;
    delete settings.rules;
    delete settings.messages;
    settings.rules = {};
    settings.messages = {};
}
function AddValidations(mode) {
    //<summary>Function used to assign validation</summary>

    RemoveAllValidations();
    if (mode == 1) {
        $("[id$=UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(SelectUOM)" }
        });

        $("[id$=QtyRecvd]").rules("add", {
            required: true,
            ThreeDecimal: true,
            max: $("[id$=POD_CONV_FACT]").val() * $("[id$=QtyRecvdOrg]").val(),
            messages: { max: GRNCreate.GRNQtyValid, required: "Translate(EnterQty)" }
        });
    }
    if (mode == 2) {
        $("input[id$=GRH_DATE]").rules("add", {
            date: true,
            required: true,
            messages: { required: "Translate(EnterGRNDate)" }
        });
        $("[id$=GRH_DEPT]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(PleaseselectaStore)" }
        });
        $("[id$=GRH_VENDOR]").rules("add", {
            selectNone: true,
            messages: { selectNone: "Translate(ReqVendorDetail)" }
        });
    }
}

function RemoveValidations() {
    //<summary>Function Remove Validation</summary>
    $(document.forms[0]).validate().resetForm();
    //    $("[id$=UOM]").rules("remove");
    // $("[id$=QtyRecvd]").rules("remove");
    $("[id$=GRH_DEPT]").rules("remove");
    $("[id$=GRH_VENDOR]").rules("remove");
}
//#endregion

function BindGrid() {
    ///<summary>To handle bind grid </summary>
    $("[id$=AddToList]").css("display", "none");
    var ajaxUrl = "";
    //
    //ajaxUrl = GRNCreate.PurchaseOrderBindGridURL + $("[id$=GRH_PK]").val() + "&Vendor=" + $("[id$=GRH_VENDOR]").val() + "&BizUnit=" + GRNCreate.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val();
    if ($("[id$=GRH_VENDOR]").val() != null && $("[id$=GRH_DEPT]").val() != null) {
        ajaxUrl = GRNCreate.PurchaseOrderBindGridURL + $("[id$=GRH_PK]").val() + "&Vendor=" + $("[id$=GRH_VENDOR]").val() + "&StoreID=" + $("[id$=GRH_DEPT]").val() + "&BizUnit=" + GRNCreate.BizUnitPk + "&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&FromDate=" + $("[id$=FromDate]").val() + "&ToDate=" + $("[id$=ToDate]").val() + "&PohPK=" + $("[id$=PO_PK]").val();
        $("#grdPOList").removeAttr("ajaxurl")
        $("#grdPOList").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdPOList");
        GrandGrid.MakeGrid($("#grdPOList"));
        //Set Default company while page load from inbox
        if ($("[id$=IsPrefID]").val() == "1") {
            $.get(ajaxUrl, function (data) {
                if (data != null && data != "" && data.Table1.length > 0) {
                    if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 0) {
                        pohDefaultCompany = data.Table1[0].POH_COMPANY;
                        //Set DDL
                        FillCompany(pohDefaultCompany);
                    }
                }
            });
        }
    }
    return false;
}

function BindWorkFlowComment() {
    ///<summary>To handle bind grid </summary>
    GrandScriptUtils.BindWorkFlowCommand("grdWrkfComment");
}

function AfterAutoCompleteSelect(targetControlID) {
    //<summary> Function Used to an event fire after select category then fill material and uom </summary>
    if (targetControlID == "SearchValue") {
        BindGrid();
    }
}

function BindPOGrid() {
    ///<summary>To handle bind po grid </summary>
    var ajaxUrl = GRNCreate.FillPODetailsView + GRNCreate.POPK;
    $("#grdPODetails").removeAttr("ajaxurl")
    $("#grdPODetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPODetails");
    GrandGrid.MakeGrid($("#grdPODetails"));
    GRNCreate.POPK = 0;
}

function BindPreviousGRNGrid() {
    ///<summary>To handle bind previuos GRN grid </summary>
    var ajaxUrl = GRNCreate.FillPREVGRNView + GRNCreate.POPK;
    $("#grdPreviousGRN").removeAttr("ajaxurl")
    $("#grdPreviousGRN").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdPreviousGRN");
    GrandGrid.MakeGrid($("#grdPreviousGRN"));
    GRNCreate.POPK = 0;
}

function AddNew() {
    ///<summary>Function used to add new details</summary>
    ClearDetails();
    GRNCreate.GRNList = new Array();
    $("#divData").data("GRNData", GRNCreate.GRNList);
    GrandGrid.MakeGrid($("#grdPendingPOList"), 0, GRNCreate.GRNList);
    BindGrid();
}
function AddToList() {
    ///<summary>Function used to add the needed po material list</summary>
    var grdID;
    var pohPK = 0;
    var itemPK = 0;
    GRNCreate.GRNList = $("#divData").data("GRNData");
    if ($("#grdPOList tr input[type=checkbox]:checked").length == 0) {
        GrandScriptUtils.ShowModal(GRNCreate.SelectPOFromList, GRNCreate.MessageBoxTitle);
        return false;
    }

    $("#grdPOList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            GRNCreate.GRNObj = new Object();
            GRNCreate.GRNObj.GRD_PO = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POH_PK, grdID);
            GRNCreate.GRNObj.POH_PK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POH_PK, grdID);
            GRNCreate.GRNObj.POD_PK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_PK, grdID);
            GRNCreate.GRNObj.BALANCE_QTY = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, "");
            GRNCreate.GRNObj.POD_RATE = parseFloat(GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_RATE, grdID)).toFixed(RateDec);
            GRNCreate.GRNObj.GRD_ITEM = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_ITEM, grdID);
            if (CheckItemExists(GRNCreate.GRNObj.GRD_PO, GRNCreate.GRNObj.GRD_ITEM)) {
                if (IsSameItemDiffRateExists(GRNCreate.GRNObj.GRD_ITEM, GRNCreate.GRNObj.POD_RATE)) {//Check the selected PO's have same items with different rate
                    GrandScriptUtils.ShowModal(GRNCreate.MsgSameItemDiffRate, GRNCreate.MessageBoxTitle);
                    return false;
                }
                GRNCreate.GRNObj.GRD_PK = 0;
                GRNCreate.GRNObj.POH_NO = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POH_NO, grdID);
                GRNCreate.GRNObj.ITM_NAME = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.ITM_NAME, grdID);
                GRNCreate.GRNObj.GRD_UOM = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_UOM, grdID);
                GRNCreate.GRNObj.POD_QTY_APPROVED = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_APPROVED, grdID).replace(/[^0-9\.]+/g, "");
                GRNCreate.GRNObj.POD_QTY_RECEIVED = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_RECEIVED, grdID).replace(/[^0-9\.]+/g, "");
                GRNCreate.GRNObj.GRD_QTY_RECEIVED = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.BALANCE_QTY, grdID).replace(/[^0-9\.]+/g, "");
                GRNCreate.GRNObj.ORG_BALANCE_QTY = GRNCreate.GRNObj.GRD_QTY_RECEIVED;
                GRNCreate.GRNObj.UOM_CODE = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.UOM_CODE, grdID);
                GRNCreate.GRNObj.GRD_REMARKS = "";
                GRNCreate.GRNObj.GRD_VND_REF_NO = "";
                GRNCreate.GRNList.push(GRNCreate.GRNObj);

                if (parseInt($("[id$=hdfIsMultiplePlant]").val()) == 0) {
                    if ($("[id$=hdfCompany]").val() == $("[id$=hdfSBUCompany]").val()) {
                        $("[id$=hdfCompany]").val(GrandGrid.Utilities.GetColumnValue($(this), "POH_COMPANY", grdID));
                    }
                    $("select[id$=GRH_COMPANY]").val($("[id$=hdfCompany]").val());
                }

                colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_GRN_FLAG, $(this).parents("table:first").attr("id"));
                if (colIndex != null) {
                    $(this).find("td:eq(" + colIndex + ")").html("<img  src=\"../Images/Classic/Icons/arrived.png\"  alt=\"Translate(Added)\" title=\"Translate(Added)\" />");
                }
            }
            $(this).find("td:first").find("input[type=checkbox]").removeAttr("checked");
        }
        //        else {
        //            pohPK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POH_PK, grdID);
        //            itemPK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_ITEM, grdID);
        //            for (var i in GRNCreate.GRNList) {
        //                if (GRNCreate.GRNList[i].GRD_PO == pohPK && GRNCreate.GRNList[i].GRD_ITEM == itemPK) {
        //                    GRNCreate.GRNList.splice(i, 1);
        //                    break;
        //                }
        //            }
        //        }
    });
    $("#divData").data("GRNData", GRNCreate.GRNList);
    GrandGrid.MakeGrid($("#grdPendingPOList"), 0, GRNCreate.GRNList);
    return false;
}

function GridHandler(tr, command) {
    ///<summary>Grid Handler for Catch all the grid events in this function </summary>
    var grdID;
    switch (command.toString().toLowerCase()) {
        case GRNCreate.DELETE:
            grdID = $(tr).parents("table:first").attr("id");
            GRNCreate.POPK = GrandGrid.Utilities.GetColumnValue(tr, GRNCreate.GRD_PO, grdID);
            GRNCreate.ItemPK = GrandGrid.Utilities.GetColumnValue(tr, GRNCreate.GRD_ITEM, grdID);
            GrandScriptUtils.ShowModal(GRNCreate.DeleteConfirmMsg, GRNCreate.Confirmation, GRNCreate.DELETE, true);
            $("[id$=hdfCompany]").val($("[id$=hdfSBUCompany]").val());
            $("select[id$=GRH_COMPANY]").val($("[id$=hdfCompany]").val());
            break;
        case GRNCreate.POVIEW:
            grdID = $(tr).parents("table:first").attr("id");
            GRNCreate.POPK = GrandGrid.Utilities.GetColumnValue(tr, GRNCreate.POH_PK, grdID);
            ViewPODetails();
            break;
        case GRNCreate.PREVIOUSGRNVIEW:
            grdID = $(tr).parents("table:first").attr("id");
            GRNCreate.POPK = GrandGrid.Utilities.GetColumnValue(tr, GRNCreate.POH_PK, grdID);
            ViewPreviousGRNDetails();
            break;
        case GRNCreate.POPRINT:
            grdID = $(tr).parents("table:first").attr("id");
            GRNCreate.POPK = GrandGrid.Utilities.GetColumnValue(tr, GRNCreate.POH_PK, grdID);
            var url = GRNCreate.POREPORTURL + "?ID=" + GRNCreate.POPK + "&APPTYPE=" + $("[id$=hdfAppType]").val() + "&APPSUBTYPE=" + $("[id$=hdfAppSubType]").val();
            OpenPDF(url);
            break;
    }
    return false;
}

function ClearDetails() {
    //<summary> Function Used to clear the GRN Details </summary>
    $("[id$=POEdit]").val("");
    $("[id$=POEditPK]").val("0");
    $("[id$=POItemEdit]").val("");
    $("[id$=POItemEditPK]").val("0");
    $("[id$=QtyRecvd]").val("");
    $("[id$=QtyRecvdOrg]").val("0");
    $("[id$=UOM]").html("");
    $("[id$=POD_CONV_FACT]").val(0);
}

function ViewPODetails() {
    ///<summary>Function used for view the PO Details</summary>
    BindPOGrid();
    $("#divPODetails").dialog({ width: 750, height: 450, resizable: false, modal: true });
    $("#divPODetails").dialog("open");
}

function ViewPreviousGRNDetails() {
    ///<summary>Function used for view the PO Details</summary>
    BindPreviousGRNGrid();
    $("#divPreviousGRN").dialog({ width: 850, height: 450, resizable: false, modal: true });
    $("#divPreviousGRN").dialog("open");
}

function DeletePOMaterial() {
    ///<summary>For delete the item,po in the grid - Details</summary>
    GRNCreate.GRNList = $("#divData").data("GRNData");
    for (var i in GRNCreate.GRNList) {
        if ((GRNCreate.GRNList[i].GRD_ITEM == GRNCreate.ItemPK) && (GRNCreate.GRNList[i].GRD_PO == GRNCreate.POPK)) {
            //            $("#grdPOList tr:has(td)").each(function () {
            //                if ($(this).find("td:first").find("input[type=checkbox]").attr("checked")) {
            //                    grdID = $(this).parents("table:first").attr("id");
            //                    var podPK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_PK, grdID);
            //                    if (podPK == GRNCreate.GRNList[i].POD_PK) {
            //                        $(this).find("td:first").find("input[type=checkbox]").removeAttr("checked");
            //                    }
            //                }
            //            });
            $("#grdPOList tr:has(td)").each(function () {
                grdID = $(this).parents("table:first").attr("id");
                var podPK = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_PK, grdID);
                if (podPK == GRNCreate.GRNList[i].POD_PK) {
                    colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_GRN_FLAG, $(this).parents("table:first").attr("id"));
                    if (colIndex != null) {
                        $(this).find("td:eq(" + colIndex + ")").html("");
                    }
                }
            });
            GRNCreate.GRNList.splice(i, 1);
            break;
        }
    }
    GRNCreate.ItemPK = 0;
    GRNCreate.POPK = 0;
    $("#divData").data("GRNData", GRNCreate.GRNList);
    GrandGrid.MakeGrid($("#grdPendingPOList"), 0, GRNCreate.GRNList);
}

function GetAdjustRecievedQty(poPK, itemPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var rcvdQty = 0;
    var pendPO = 0;
    var pendItem = 0;
    $("#grdPendingPOList tr:has(td)").each(function () {
        grdID = $(this).parents("table:first").attr("id");
        pendPO = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_PO, grdID);
        pendItem = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_ITEM, grdID);
        if (pendPO == poPK && itemPK == pendItem) {
            rcvdQty = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_QTY_RECEIVED, grdID);
        }
    });
    return rcvdQty;
}

function GetCurrentRecievedQty(poPK, itemPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var pendPO = 0;
    var pendItem = 0;
    for (var i in GRNCreate.GRNList) {
        if ((GRNCreate.GRNList[i].GRD_ITEM == itemPK) && (GRNCreate.GRNList[i].GRD_PO == poPK)) {
            return GRNCreate.GRNList[i].GRD_QTY_RECEIVED;
        }
    }
}

function CheckItemExists(poPK, itemPK) {
    //<summary> Function Used to check whether this po item already added. </summary>
    GRNCreate.GRNList = $("#divData").data("GRNData");
    var flag = true;
    for (var i in GRNCreate.GRNList) {
        if ((GRNCreate.GRNList[i].GRD_ITEM == itemPK) && (GRNCreate.GRNList[i].GRD_PO == poPK)) {
            flag = false;
            break;
        }
    }
    return flag;
}

/// <summary>
/// Check the selected PO's have same items with different rate
/// </summary>
function IsSameItemDiffRateExists(itemPK, poRate) {
    //<summary> Function Used to check whether this po item already added. </summary>
    GRNCreate.GRNList = $("#divData").data("GRNData");
    var flag = false;
    for (var i in GRNCreate.GRNList) {
        if ((GRNCreate.GRNList[i].GRD_ITEM == itemPK) && (GRNCreate.GRNList[i].POD_RATE != poRate)) {
            flag = true;
            break;
        }
    }
    return flag;
}

function CheckOrgItemExists(poPK, itemPK, GRNObj) {
    //<summary> Function Used to check whether this po item already added. </summary>
    var flag = false;
    for (var i in GRNObj.GRNList) {
        if ((GRNObj.GRNList[i].GRD_ITEM == itemPK) && (GRNObj.GRNList[i].GRD_PO == poPK)) {
            return GRNObj.GRNList[i].GRD_QTY_RECEIVED;
        }
    }
    return flag;
}

function EnableFields() {
    //<summary> Function Used to Enable fields </summary>
    $("[id$=GRH_VENDOR]").removeAttr("disabled");
    $("[id$=GRH_COMPANY]").removeAttr("disabled");
    $("[id$=GRH_DEPT]").removeAttr("disabled");
    $("[id$=GRH_DATE]").removeAttr("disabled");
    $("[id$=GRH_VND_REF_NO]").removeAttr("disabled");
    $("[id$=GRH_VND_REF_DATE]").removeAttr("disabled");
}

//For checking selected date is a future date or not
//command=>Draft,Submit
function ShowFutureDate(command) {
    var msgTitle;
    var msg;
    msgTitle = GRNCreate.MessageBoxTitle;
    msg = GRNCreate.ContFutureDateMsg;
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
                    SavePage("Submit");
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

function SavePage(command) {
    //<summary> Function Used to save page </summary>

    $.get(GRNCreate.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GRNCreate.SessionExpired, GRNCreate.Confirmation, GRNCreate.LOGOUT, true);
            result = false;
        }
        else {

            $("[id$=GRH_COMPANY]").attr("disabled", false);
            GRNCreate.GRNList = new Array();
            GRNCreate.GRNList = AddToPendingPOList();
            if (!IsValidAdditionalQty()) {
                return false;
            }
            if (GRNCreate.GRNList.length == 0) {
                GrandScriptUtils.ShowModal(GRNCreate.AddGoodsReceiptDetails, GRNCreate.MessageBoxTitle);
                return false;
            }
            if (hasExcedQty == true && GRNCreate.PreviousCommand == GRNCreate.EmptyCommand) {
                GRNCreate.PreviousCommand = command;
                GrandScriptUtils.ShowModal(GRNCreate.ReceivedQtyExceed, GRNCreate.MessageBoxTitle, GRNCreate.Continue);
                return false;
            }

            GRNCreate.PreviousCommand == GRNCreate.EmptyCommand;

            if (command != "Draft") {
                $("[id$=GRH_STATUS]").val("1");
            }
            //        if (!IsValidRecieveQty()) {
            //            GrandScriptUtils.ShowModal(GRNCreate.GRNQtyValid, GRNCreate.MessageBoxTitle);
            //            return false;
            //        }
            $("[id$=GRNList]").val(JSON.stringify(GRNCreate.GRNList));


            //File Upload Start
            var ObjFile = $("#divFileData").data("FileData");
            $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
            //End FileUpload


            $("[id$=WKF_FLAG]").val("0");
            if (command != "Draft") {
                $("[id$=ActionID]").val($("[id$=WRKFACT_ID]").val()); // save and doworkflow.
                if ($("[id$=ReferenceID]").val() == "0") {
                    $("[id$=WKF_FLAG]").val("1");
                }
            }
            else
                $("[id$=ActionID]").val('0');  // save only.
            EnableFields();


            var jSonString = GrandScriptUtils.FormToJsonString(false);
            RemoveValidations();
            AddValidations(2);

            if ($(document.forms[0]).valid()) {
                //Showing validation for Future Date selection
                if ($("[id$=hdfIsContFutureDate]").val() != "1") {
                    var RetVal = CompareDate($("[id$=GRH_DATE]").val(), $("[id$=hdfCurrentDate]").val());
                    if (RetVal == 1) {
                        ShowFutureDate(command);
                        return false;
                    }
                }

                //To Prevent Muliple Click
                if ($("[id$=SubmitFlag]").val() == "0")
                    $("[id$=SubmitFlag]").val('1')
                else
                    return false;

                $.post(GRNCreate.SaveGoodsReceiptNote, jSonString, function (data) {
                    if (parseInt(data[0]) > 0) {
                        if (command == "Draft") {
                            var SaveMessageWithPRNo = GRNCreate.GRNSavedMessage;
                            if ($("[id$=AST_DOC_MODE]").val() == "1")
                                SaveMessageWithPRNo = GRNCreate.SaveMessage1 + " " + data[1] + " " + GRNCreate.SaveMessage2;
                            GrandScriptUtils.ShowModal(SaveMessageWithPRNo, GRNCreate.MessageBoxTitle, GRNCreate.SAVE);
                        }
                        else {
                            $("[id$=hdfAppID]").val(data[0]);
                            $("[id$=AppNo]").val(data[1]);
                            SaveWorkFlow();
                        }
                    }
                    else if (parseInt(data[0]) == 0) {
                        GrandScriptUtils.ShowModal(GRNCreate.MsgRefAdded, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -1) {
                        GrandScriptUtils.ShowModal(GRNCreate.ActionFailedMessage, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -2) {
                        GrandScriptUtils.ShowModal(GRNCreate.SaveMessage1 + " " + $("[id$=GRH_NO]").html() + " " + GRNCreate.EditUsedByAnotherUser, GRNCreate.MessageBoxTitle, GRNCreate.SAVE);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -3) {
                        GrandScriptUtils.ShowModal(GRNCreate.GRNQtyValid, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    //Vendor Ref No should not duplicate 
                    else if (parseInt(data[0]) == -6) {
                        GrandScriptUtils.ShowModal(GRNCreate.GRNVendorRefDuplicate, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -35) { //GRN Modify Qty less than GIN Qty
                        GrandScriptUtils.ShowModal(GRNCreate.GRNQtyLessThanGINQty, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -36) { //GRN  entry has a reference in Invoice/GIN
                        GrandScriptUtils.ShowModal(GRNCreate.CannotDeleteHaveReference, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else if (parseInt(data[0]) == -37) { // GRN qty less than that of invoiced qty
                        GrandScriptUtils.ShowModal(GRNCreate.GRNQtyLessThanInvoiceQty, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                    else {
                        GrandScriptUtils.ShowModal(GRNCreate.ActionFailedMessage, GRNCreate.MessageBoxTitle);
                        $("[id$=SubmitFlag]").val('0')
                    }
                });
            }
        }
    });

    return false;
}

function ShowWorkflowSaveMsg() {
    ///<summary>To Show Message, if Details saved and after do workflow</summary>
    var msg = "";
    if ($("[id$=hdfRefID]").val() > 0 && $("[id$=hdfIsGoToInbox]").val() == "1") {
        msg = GRNCreate.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + GRNCreate.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, GRNCreate.MessageBoxTitle, GRNCreate.INBOX);
    } else {
        msg = GRNCreate.SaveMessage1 + " " + $("[id$=AppNo]").val() + " " + GRNCreate.SubmitMessage;
        GrandScriptUtils.ShowModal(msg, GRNCreate.MessageBoxTitle, GRNCreate.SAVE);
    }
}

function IsValidRecieveQty() {
    ///<summary>Function used to check the recieved qty greater than po qty.</summary>
    var isValid = true;
    if (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6) {
        for (var i in GRNCreate.GRNList) {
            if ((parseFloat(GRNCreate.GRNList[i].GRD_QTY_RECEIVED) > parseFloat(GRNCreate.GRNList[i].ORG_BALANCE_QTY)) || (parseFloat(GRNCreate.GRNList[i].GRD_QTY_RECEIVED) <= 0)) {
                isValid = false;
                break;
            }
        }
    }
    return isValid;
}

function IsValidAdditionalQty() {
    var qty = 0;
    var strMsg = "";
    var receivedQty = 0;
    var ordQty = 0;
    var orderPer = 0;
    var actualQty = 0;
    var balToReceive = 0;
    var qtyReceived = 0;
    var actualQty = 0;
    var isValid = true;
    hasExcedQty = false;

    if (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6 || parseInt($("[id$=GRH_STATUS]").val()) == 8 || parseInt($("[id$=GRH_STATUS]").val()) == 9) {
        for (var i in GRNCreate.GRNList) {
            orderPer = parseFloat($("[id$=orderPercentage]").val());
            if (GRNCreate.GRNList[i].GRD_QTY_RECEIVED != "")
                receivedQtyNow = parseFloat(GRNCreate.GRNList[i].GRD_QTY_RECEIVED);
            else
                receivedQtyNow = 0;
            ordQty = parseFloat(GRNCreate.GRNList[i].POD_QTY_APPROVED);


            balToReceive = parseFloat(GRNCreate.GRNList[i].ORG_BALANCE_QTY);
            //            qtyReceived = (GRNCreate.GRNList[i].POD_QTY_RECEIVED == "[NEW]") ? 0 : parseFloat(GRNCreate.GRNList[i].POD_QTY_RECEIVED);
            qtyReceived = parseFloat(GRNCreate.GRNList[i].POD_QTY_RECEIVED);

            hasExcedQty = balToReceive < receivedQtyNow;
            actualQty = ordQty + (ordQty * orderPer / 100);
            //            actualQty = balToReceive + (balToReceive * orderPer / 100);
            if ((receivedQtyNow + qtyReceived) > actualQty) {
                strMsg += "Item: " + GRNCreate.GRNList[i].ITM_NAME + " Qty Exceed the allowed limit of (" + orderPer + "%) <br />";
                //strMsg += "Item: " + GRNCreate.GRNList[i].ITM_NAME + " Qty Exceed the allowed limit of (" +orderPer+ "%) <br />";Translate(
                isValid = false;
            }
            orderPer = 0;
            receivedQty = 0;
            balToReceive = 0;
            actualQty = 0;
        }
    }
    if (strMsg != "")
        GrandScriptUtils.ShowModal(strMsg, GRNCreate.MessageBoxTitle);

    return isValid;
}

function AddToPendingPOList() {
    ///<summary>Function used to add the needed po material list</summary>
    GRNCreate.GRNList = $("#divData").data("GRNData");
    for (var i in GRNCreate.GRNList) {
        if (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6 || parseInt($("[id$=GRH_STATUS]").val()) == 8 || parseInt($("[id$=GRH_STATUS]").val()) == 9 || GRNCreate.IsModifyPR == true) {
            GRNCreate.GRNList[i]
            GRNCreate.GRNList[i].GRD_QTY_RECEIVED = $("#txtrecvQty_" + i).val();

            var qtyReceivd = $("#txtrecvQty_" + i).val();

            if ($("#txtRemarks_" + i).val() == null || $("#txtRemarks_" + i).val() == undefined || $("#txtRemarks_" + i).val() == "") {
                GRNCreate.GRNList[i].GRD_REMARKS = "";
            }
            else {
                GRNCreate.GRNList[i].GRD_REMARKS = $("#txtRemarks_" + i).val();
            }
            //L0tno/Batchno
            if ($("#txtlotNoBatchno_" + i).val() == null || $("#txtlotNoBatchno_" + i).val() == undefined || $("#txtlotNoBatchno_" + i).val() == "") {
                GRNCreate.GRNList[i].GRD_VND_REF_NO = "";
            }
            else {
                GRNCreate.GRNList[i].GRD_VND_REF_NO = $("#txtlotNoBatchno_" + i).val();
            }

            //Date 
            if ($("#GRD_DOM_" + i).val() == null || $("#GRD_DOM_" + i).val() == undefined || $("#GRD_DOM_" + i).val() == "") {
                GRNCreate.GRNList[i].GRD_DOM = "";
            }
            else {
                GRNCreate.GRNList[i].GRD_DOM = $("#GRD_DOM_" + i).val();
            }

            if ($("#GRD_DOE_" + i).val() == null || $("#GRD_DOE_" + i).val() == undefined || $("#GRD_DOE_" + i).val() == "") {
                GRNCreate.GRNList[i].GRD_DOE = "";
            }
            else {
                GRNCreate.GRNList[i].GRD_DOE = $("#GRD_DOE_" + i).val();
            }
        }
    }
    return GRNCreate.GRNList;
}

//function IsExceedQty() { 
//   $("#grdPendingPOList tr:has(td)").each(function (index) {
//            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_REMARKS, grdID);
//            remarks = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID);
//            recvQtyColIndex = GrandGrid.Utilities.GetColumnIndex($(this), 'GRD_QTY_RECEIVED', grdID);
//            recvQty = GrandGrid.Utilities.GetColumnValue($(this), 'GRD_QTY_RECEIVED', grdID);
//            if (remkColIndex != null && (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6)) {
//                $(this).find("td:eq(" + remkColIndex + ")").html("");
//                $(this).find("td:eq(" + remkColIndex + ")").append("<input type=\"text\" id=\"txtRemarks_" + index + "\"  value=\"" + remarks + "\" width=\"85%\" > </input>");
//            }
//            else {
//                $(this).find("td:eq(" + remkColIndex + ")").html(remarks == null ? " " : remarks);
//            }
//            }
//            
//}

function AfterGridBind(grdID) {
    //<summary>function Call Afer binding Grid</summary>
    if (grdID == "grdPendingPOList") {
        var remkColIndex = 0;
        var remarks = "";

        //Adding New field Lotno/Batchno
        var lotNoBatchnoColIndex = 0;
        var lotNoBatchno = "";
        //end
        var ponoColIndex = 0;
        var pono = "";
        var recvdColIndex = 0;
        var recvd = "";
        var recvQtyColIndex = 0;
        var recvQty = "";
        var txtrecvQty;
        var colIndex = 0;
        var reqDate = "";
        var status = 0;
        var qty = 0;

        if (GRNCreate.IsViewMode) {
            $("#grdPendingPOList th:last").hide();
        }
        else if (parseInt($("[id$=GRH_STATUS]").val()) == 1 || parseInt($("[id$=GRH_STATUS]").val()) == 7 || parseInt($("[id$=GRH_STATUS]").val()) == 11) {
            $("#grdPendingPOList th:last").hide();
        }

        $("#grdPendingPOList tr:has(td)").each(function (index) {
            remkColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_REMARKS, grdID);
            remarks = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_REMARKS, grdID);

            //Adding New field Lotno/Batchno
            lotNoBatchnoColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_VND_REF_NO, grdID);
            lotNoBatchno = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_VND_REF_NO, grdID) == "null" ? "" : GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_VND_REF_NO, grdID);
            //End New field

            recvQtyColIndex = GrandGrid.Utilities.GetColumnIndex($(this), 'GRD_QTY_RECEIVED', grdID);
            recvQty = GrandGrid.Utilities.GetColumnValue($(this), 'GRD_QTY_RECEIVED', grdID);
            if (remkColIndex != null && (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6 || parseInt($("[id$=GRH_STATUS]").val()) == 8 || parseInt($("[id$=GRH_STATUS]").val()) == 9 || parseInt($("[id$=GRH_IS_EDIT]").val()) == 1)) {
                $(this).find("td:eq(" + remkColIndex + ")").html("");
                $(this).find("td:eq(" + remkColIndex + ")").append("<input type=\"text\" id=\"txtRemarks_" + index + "\" style=\"width:100px\" tabindex=\"11\"  value=\"" + remarks + "\" width=\"85%\" > </input>");
            }
            else {
                $(this).find("td:eq(" + remkColIndex + ")").html(remarks == null ? " " : remarks);
            }


            //Adding Newfield Lotno/batchno
            if (lotNoBatchnoColIndex != null && (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6 || parseInt($("[id$=GRH_STATUS]").val()) == 8 || parseInt($("[id$=GRH_STATUS]").val()) == 9 || parseInt($("[id$=GRH_IS_EDIT]").val()) == 1)) {
                $(this).find("td:eq(" + lotNoBatchnoColIndex + ")").html("");
                $(this).find("td:eq(" + lotNoBatchnoColIndex + ")").append("<input type=\"text\" id=\"txtlotNoBatchno_" + index + "\" style=\"width:85px\"  value=\"" + lotNoBatchno + "\" width=\"85%\" tabindex=\"11\" maxLength =\"200\" > </input>");
            }
            else {
                $(this).find("td:eq(" + lotNoBatchnoColIndex + ")").html(lotNoBatchno == null || lotNoBatchno == 'undefined' ? " " : lotNoBatchno);
            }

            //end


            if (recvQtyColIndex != null && (parseInt($("[id$=GRH_STATUS]").val()) == 0 || parseInt($("[id$=GRH_STATUS]").val()) == 6 || parseInt($("[id$=GRH_STATUS]").val()) == 8 || parseInt($("[id$=GRH_STATUS]").val()) == 9 || parseInt($("[id$=GRH_IS_EDIT]").val()) == 1)) {
                if (recvQtyColIndex != null) {
                    $(this).find("td:eq(" + recvQtyColIndex + ")").html("");
                    $(this).find("td:eq(" + recvQtyColIndex + ")").append("<input type=\"text\" style=\"width:60px\" id=\"txtrecvQty_" + index + "\"  value=\"" + parseFloat(recvQty).toFixed(QtyDec) + "\" width=\"80%\"  maxLength =\"15\" tabindex=\"11\" class=\"numeric\"    onchange=\"javascript:MakeNumric(this," + recvQty + ");\"   />");
                }
            }
            else {
                $(this).find("td:eq(" + recvQtyColIndex + ")").html(numberWithCommas(parseFloat(recvQty).toFixed(QtyDec)));
            }
            //Date Section DOM
            reqDate = "";
            var dateTypeFlag = 0;
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_DOM, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                reqDate = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_DOM, $(this).parents("table:first").attr("id"));
                if (reqDate == "undefined") reqDate = "";
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" style=\"width:70px\" tabindex=\"11\" onkeydown=\"return CheckKey(event)\" onpaste=\"return false;\" id=\"GRD_DOM_" + index + "\" value=\"" + reqDate + "\" onchange=\"ClearCheckDate(" + index + ")\" ></input><input id=\"GRD_DOM_hdf_" + index + "\" type=\"hidden\">");
                dateTypeFlag = 1;
            }
            //Date Section DOE
            reqDate = "";
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.GRD_DOE, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                reqDate = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.GRD_DOE, $(this).parents("table:first").attr("id"));
                if (reqDate == "undefined") reqDate = "";
                $(this).find("td:eq(" + colIndex + ")").html("<input type=\"text\" style=\"width:70px\" tabindex=\"11\" onkeydown=\"return CheckKey(event)\" onpaste=\"return false;\" id=\"GRD_DOE_" + index + "\" value=\"" + reqDate + "\" onchange=\"ClearCheckDate(" + index + ")\" ></input><input id=\"GRD_DOE_hdf_" + index + "\" type=\"hidden\">");
                //GrandScriptUtils.DatePicker("GRD_DOE_" + index, false, false, false);
                dateTypeFlag = dateTypeFlag == 1 ? 3 : 2;
            }
            //New Number Format start
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_QTY_APPROVED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "POD_QTY_APPROVED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "BALANCE_QTY", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "BALANCE_QTY", grdID);
            if (colIndex != null) {
                qty = qty < 0 ? 0.00 : qty;
                //                if (parseFloat(qty))
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }

            recvdColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_QTY_RECEIVED, grdID);
            recvd = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_RECEIVED, grdID);
            if (recvdColIndex != null) {
                $(this).find("td:eq(" + recvdColIndex + ")").html(numberWithCommas(parseFloat(recvd).toFixed(QtyDec)));
                if (parseFloat(recvd)) {
                    $(this).find("td:eq(" + recvdColIndex + ")").html("");
                    $(this).find("td:eq(" + recvdColIndex + ")").html("<a style=\"cursor:pointer;float:right\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'PREVIOUSGRNVIEW');\">" + numberWithCommas(parseFloat(recvd).toFixed(QtyDec)) + "</a>");
                }
            }
            switch (dateTypeFlag) {
                case 1:
                    GrandScriptUtils.DatePicker("GRD_DOM_" + index, false, false, false);
                    break;
                case 2:
                    GrandScriptUtils.DatePicker("GRD_DOE_" + index, false, false, false);
                    break;
                case 3:
                    GrandScriptUtils.AddDateRangeCommon("GRD_DOM_" + index, "GRD_DOM_hdf_" + index, "GRD_DOE_" + index, "GRD_DOE_hdf_" + index, false, false);
                    break;
            }
            if (GRNCreate.IsViewMode) {
                $(this).find("td:last").hide();
                $(this).find("td:last").hide();
            }
            else if (parseInt($("[id$=GRH_STATUS]").val()) == 1 || parseInt($("[id$=GRH_STATUS]").val()) == 7 || parseInt($("[id$=GRH_STATUS]").val()) == 11) {
                $(this).find("td:last").hide();
                $(this).find("td:last").hide();
            }
            if (GRNCreate.IsModifyPR) {
                $("#grdPendingPOList th:last").show();
                $(this).find("td:last").show();
                $(this).find("td:last").show();
            }
        });
    }

    if (grdID == $("#grdPreviousGRN").attr("id")) {
        var qtyRej = "";
        $("#grdPreviousGRN").find("tr").each(function () {
            qtyRej = "";
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_REJECTED", $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qtyRej = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", $(this).parents("table:first").attr("id"));
                if (qtyRej == "null")
                    qtyRej = "-";
                $(this).find("td:eq(" + colIndex + ")").html(qtyRej);
            }
        });
    }
    if (grdID == "grdPODetails") {
        var colIndex = 0;
        var qty = 0;
        $("#grdPODetails tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "POD_QTY_APPROVED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "POD_QTY_APPROVED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(parseFloat(qty).toFixed(QtyDec));
            }
        });
    }
    if (grdID == "grdPreviousGRN") {
        var colIndex = 0;
        var qty = 0;
        $("#grdPreviousGRN tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_RECEIVED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_RECEIVED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "GRD_QTY_REJECTED", grdID);
            qty = GrandGrid.Utilities.GetColumnValue($(this), "GRD_QTY_REJECTED", grdID);
            if (colIndex != null) {
                if (parseFloat(qty))
                    $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(parseFloat(qty).toFixed(QtyDec)));
            }
        });
    }
    if (grdID == "grdPOList") {
        var qty = 0;
        var strVal = "";
        // Code for hide checkbox grdPOList
        if (parseInt($("[id$=GRH_STATUS]").val()) == 1 || parseInt($("[id$=GRH_STATUS]").val()) == 7 || parseInt($("[id$=GRH_STATUS]").val()) == 11 || parseInt($("[id$=GRH_STATUS]").val()) == 2 || parseInt($("[id$=GRH_STATUS]").val()) == 3) {
            //Hiding the template field
            $("#grdPOList").find("tr").each(function () {
                $(this).find("td:first,th:first").hide();
            });
        }

        $("[id$=AddToList]").css("display", "block");
        if (GRNCreate.IsViewMode) {
            $("[id$=AddToList]").css("display", "none");
        }
        else if (parseInt($("[id$=GRH_STATUS]").val()) == 1 || parseInt($("[id$=GRH_STATUS]").val()) == 7 || parseInt($("[id$=GRH_STATUS]").val()) == 11) {
            $("[id$=AddToList]").css("display", "none");
        }
        //In Case of Modify ,show the Checkbox
        if (parseInt($("[id$=GRH_IS_EDIT]").val()) == 1) {
            $("#grdPOList").find("tr").each(function () {
                $(this).find("td:first,th:first").show();
            });
            $("[id$=AddToList]").css("display", "block"); //Show AddToList Button
        }

        $("#grdPOList tr:has(td)").each(function (index) {
            //Status
            var poDtl = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_PK, grdID);
            GRNCreate.GRNList = $("#divData").data("GRNData");
            var itemCount = 0;
            for (var grnIndex in GRNCreate.GRNList) {
                if (GRNCreate.GRNList[grnIndex].POD_PK == poDtl) {
                    itemCount = 1;
                    break;
                }
            }
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_GRN_FLAG, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                //                status = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_GRN_FLAG, $(this).parents("table:first").attr("id"));
                //                if (status == 1)
                //                    $(this).find("td:eq(" + colIndex + ")").html("<img  src=\"../Images/Classic/Icons/arrived.png\"  alt=\"Translate(Added)\" title=\"Translate(Added)\" />");
                //                else
                //                    $(this).find("td:eq(" + colIndex + ")").html("");
                if (itemCount > 0) {
                    $(this).find("td:eq(" + colIndex + ")").html("<img  src=\"../Images/Classic/Icons/arrived.png\"  alt=\"Translate(Added)\" title=\"Translate(Added)\" />");
                }
                else {
                    $(this).find("td:eq(" + colIndex + ")").html("");
                }
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_QTY_APPROVED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_APPROVED, $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_QTY_RECEIVED, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_RECEIVED, $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.BALANCE_QTY, $(this).parents("table:first").attr("id"));
            if (colIndex != null) {
                qty = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.BALANCE_QTY, $(this).parents("table:first").attr("id"));
                strVal = parseFloat(qty).toFixed(QtyDec)
                $(this).find("td:eq(" + colIndex + ")").html(numberWithCommas(strVal));
            }

            ponoColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POH_NO, grdID);
            pono = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POH_NO, grdID);
            if (ponoColIndex != null) {
                $(this).find("td:eq(" + ponoColIndex + ")").html("");
                if ($("[id$=hfPOPrint]").val() == "1") {
                    $(this).find("td:eq(" + ponoColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'POPRINT');\">" + pono + "</a>");
                }
                else {
                    $(this).find("td:eq(" + ponoColIndex + ")").html("<a style=\"cursor:pointer\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'POVIEW');\">" + pono + "</a>");
                }
            }
            recvdColIndex = GrandGrid.Utilities.GetColumnIndex($(this), GRNCreate.POD_QTY_RECEIVED, grdID);
            recvd = GrandGrid.Utilities.GetColumnValue($(this), GRNCreate.POD_QTY_RECEIVED, grdID).replace(/[^0-9\.]+/g, "");
            if (recvdColIndex != null) {
                if (parseInt(recvd)) {
                    $(this).find("td:eq(" + recvdColIndex + ")").html("");
                    $(this).find("td:eq(" + recvdColIndex + ")").html("<a style=\"cursor:pointer;float:right\" onclick=\"javascript:return GridHandler($(this).parents('tr:eq(0)'),'PREVIOUSGRNVIEW');\">" + numberWithCommas(parseFloat(recvd).toFixed(QtyDec)) + "</a>");
                }
            }
            //            var grnObj = JSLINQ(GRNCreate.GRNList.)
            //                          .Where(function (item) { return item.POD_PK == poDtl; })
            //                          .FirstOrDefault(null);
            //            if (grnObj != null) {
            //                $(this).find("input[type=checkbox]").attr("checked", "checked");
            //            }
        });
    }
}

function MakeNumric(txtReq, orgVal) {
    //<summary>Function used hide some fields</summary>

    var floatRegQty = new RegExp("(?!^0*$)(?!^0*\\.0*$)^\\d{1,8}(\\.\\d{1," + parseInt(QtyDec) + "})?$");
    // var floatRegQty = /(?!^0*$)(?!^0*\.0*$)^\d{1,8}(\.\d{1,QtyDec})?$/; 
    if (!floatRegQty.test($(txtReq).val())) {
        $(txtReq).val(orgVal);
    }
}

function SetSearchType(isLoad) {
    ///<summary>Function To Enable/Disable Selected Option For Search </summary>
    ClearSearchDetails();
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val("");
    if (strname == "Date") {
        $("#divSearchDtls").hide();
        $("#divDate").show();
        $("[id$=imbSearch]").show();
        GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
    }
    else {
        $("#divSearchDtls").show();
        $("#divDate").hide();
        $("[id$=imbSearch]").show();
    }
}

function ClearSearchDtls() {
    ///<summary>Function To clear search fields </summary>
    $("select[id$=SearchType]").val("POH_NO");
    SetSearchType();
    BindGrid();
    return false;
}

function ClearSearchDetails() {
    ///<summary>To Clear Details In Search Section</summary>
    $("[id$=SearchValue]").val("");
    $("[id$=FromDate]").val("");
    $("input[id$=hdfFrmDate]").val("");
    $("[id$=ToDate]").val("");
    $("input[id$=hdfToDate]").val("");
}

function ResetPage() {
    //<summary>Function Used to Reset Page</summary>
    $.get(GRNCreate.GetCurrentDepartment, function (data) { //for multi tab department checking
        if ($("[id$=hdfDeptID]").val() != data) {
            GrandScriptUtils.ShowModal(GRNCreate.SessionExpired, GRNCreate.Confirmation, GRNCreate.LOGOUT, true);
            result = false;
        }
        else {
            window.location = GRNCreate.GRNListUrl;
        }
    });


    return false;
}

function ModalOk(command) {
    //<summary>Function invoke after Model popup ok Click</summary>
    switch (command) {
        case GRNCreate.SAVE:
            window.location = GRNCreate.GRNListUrl;
            break;
        case GRNCreate.INBOX:
            window.location = GRNCreate.InboxURL;
            break;
        case GRNCreate.DELETE:
            DeletePOMaterial();
            break;
        case GRNCreate.Continue:
            SavePage(GRNCreate.PreviousCommand);
            break;
        case GRNCreate.LOGOUT:
            $("[id$=imbLogout]").click();
            break;
    }
}

function ClearCheckDate(index) {
    var dateType = $("#GRD_DOM_" + index).length > 0 && $("#GRD_DOE_" + index).length > 0 ? 3 : $("#GRD_DOM_" + index).length > 0 ? 1 : $("#GRD_DOE_" + index).length > 0 ? 2 : 0;
    if (dateType == 3 && ($("#GRD_DOM_" + index).val() == "" || $("#GRD_DOE_" + index).val() == "")) {
        if ($("#GRD_DOM_" + index).val() == "") {
            $("#GRD_DOM_hdf_" + index).val("");
        }
        if ($("#GRD_DOE_" + index).val() == "") {
            $("#GRD_DOE_hdf_" + index).val("");
        }
        GrandScriptUtils.AddDateRangeCommon("GRD_DOM_" + index, "GRD_DOM_hdf_" + index, "GRD_DOE_" + index, "GRD_DOE_hdf_" + index, false, false, false, false, true);
    }
}
//Comma Separation for Quantity & Amount 
//function numberWithCommas(x) {
//    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
//}