
/// <reference path="../../GrandScriptUtils.js" />


function InitComponents() {
    GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", "dd-M-yy", false, false,true);
    GrandScriptUtils.AddDateRange("txtFilterFrom", "hdfFilterFrom", "txtFilterTo", "hdfFilterTo", "dd-M-yy", false, true);
    SetGridViewState();
    CalculateGrouplistTotal();
    ShowHideAdvancedSearch($("[id$=hdfShowHideFilter]").val());
    SetSearchBy();
    SetTime();
    CalculateScenarioListTotal();
    ShowHidePlanSplit();
    ShowHideScenarioSplit();
}

$(document).ready(function () {
    //ShowHideFilter(1)
});

function EndRequestHandlerPage() {
    //ShowHideFilter(1);
}

function ValidatePageNow(validationGroup) {
    //<summary>validating form</summary>

    if (typeof (Page_ClientValidate) == 'function') {
        CheckValidationDuplicate(validationGroup);
        Page_ClientValidate(validationGroup);
    }
    if (!Page_IsValid) {
        $("[id$=litErrorMsg]").hide();
        ShowErrorMessage($("#diverror").html());
        return false;  //Page is invalid -- stop right here
    }
    else {
        return true; //everythings ok --- Call your function & do your stuff
    }
}


function isDate(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 45 || charCode > 57)) {
        return false;
    }

    return true;
}

var validationArrayGroup;

function CheckValidationDuplicate(valGroup) {

    validationArrayGroup = new Array();

    //Traversing from bottom through all the validation controls in the page

    for (var i = Page_Validators.length - 1; i >= 0; i--) {

        if (typeof (Page_Validators[i].validationGroup) == "string") {

            if (valGroup == Page_Validators[i].validationGroup) {

                //checks if the control is already in the validation array

                if (!CheckValidationExists(Page_Validators[i].id)) {

                    //insert new conrol to the Array of present validations

                    validationArrayGroup.push(Page_Validators[i].id);

                }

                //remove if control is already in Array of present validations

                else {

                    Page_Validators.splice(i, 1);

                }

            }

            //remove control if not in group

            else {

                Page_Validators.splice(i, 1);

            }

        }

    }

}

//For checking if validation control in Array of present validations

function CheckValidationExists(id) {

    for (var i in validationArrayGroup) {

        if (validationArrayGroup[i] == id) {

            return true;

        }

    }

    return false;

}
function SetSearchBy() {
    $("[id$=hdfFilterPlanPK]").val(0);
    if ($("select[id$=ddlFilterBy]").val() == 1) {
        $("[id$=txtFilterPlanName]").hide();
        $("[id$=txtFilterPlanName]").next("a").remove();
        $("[id$=txtFilterPlanCode]").show();
        $("[id$=txtFilterPlanCode]").addClass("input-w32per");
        GrandScriptUtils.MakeAutoCompleteDDL("txtFilterPlanCode", url + "?SearchBy=PNH_CODE&StatusVal=" + $("select[id$=ddlFilterStatus]").val(), "hdfFilterPlanPK", true, true, "GETORDERPLANTEXT");
    }
    else {

        $("[id$=txtFilterPlanCode]").hide();
        $("[id$=txtFilterPlanCode]").next("a").remove();
        $("[id$=txtFilterPlanName]").show();
        $("[id$=txtFilterPlanName]").addClass("input-w58-5per");
        GrandScriptUtils.MakeAutoCompleteDDL("txtFilterPlanName", url + "?SearchBy=PNH_NAME&StatusVal=" + $("select[id$=ddlFilterStatus]").val(), "hdfFilterPlanPK", true, true, "GETORDERPLANTEXT");
    }

}


function ShowListing(flag) {
    //<summary>Function used to hide page section, if has flag: show listing section, otherwise show entry section</summary>
    if (flag == 1) {
        $("[id$=PageAction_Entry]").hide();
        $("[id$=PageAction_Listing]").show();
        $("[id$=pnlListing]").show();
        $("[id$=pnlEntry]").hide();
    }
    else {
        $("[id$=PageAction_Entry]").show();
        $("[id$=PageAction_Listing]").hide();
        $("[id$=pnlListing]").hide();
        $("[id$=pnlEntry]").show();
    }
    return false;
}

function ShowTabs(flag) {
    if (flag == '1') {
        $("[id$=hdnTabListNew]").val(flag);
        $("[id$=divAdvanceFilter]").show();
        $("[id$=divPackingMaterials]").hide();
        $("[id$=divPlanning]").hide();
        $("[id$=divSummary]").hide();
        $("[id$=divScenario]").hide();
        $("[id$=lnkPendingOrders]").removeClass('tab-inactive');
        $("[id$=lnkPendingOrders]").addClass('tab-active');
        $("[id$=lnkPlanning]").removeClass('tab-active');
        $("[id$=lnkPlanning]").addClass('tab-inactive');
        $("[id$=lnkSummary]").removeClass('tab-active');
        $("[id$=lnkSummary]").addClass('tab-inactive');
        $("[id$=lnkScenario]").removeClass('tab-active');
        $("[id$=lnkScenario]").addClass('tab-inactive');
        $("[id$=tab1]").removeClass('tab-inactive');
        $("[id$=tab1]").addClass('tab-active');
        $("[id$=tab2]").removeClass('tab-active');
        $("[id$=tab2]").addClass('tab-inactive');
        $("[id$=tab3]").removeClass('tab-active');
        $("[id$=tab3]").addClass('tab-inactive');
        $("[id$=tab4]").removeClass('tab-active');
        $("[id$=tab4]").addClass('tab-inactive');
        $("[id$=pnlDelete]").hide();
        $("[id$=pnlCancelSubmit]").hide();
        $("[id$=pnlRevert]").hide();
        $("[id$=btnFinalize]").hide();
        $("[id$=pnlSave]").hide();
    }
    else if (flag == '2') {
        $("[id$=hdnTabListNew]").val(flag);
        $("[id$=divAdvanceFilter]").hide();
        $("[id$=divPlanning]").show();
        $("[id$=divSummary]").hide();
        $("[id$=divScenario]").hide();
        $("[id$=lnkPendingOrders]").removeClass('tab-active');
        $("[id$=lnkDetails]").addClass('tab-inactive');
        $("[id$=lnkPlanning]").removeClass('tab-inactive');
        $("[id$=lnkPlanning]").addClass('tab-active');
        $("[id$=lnkSummary]").removeClass('tab-active');
        $("[id$=lnkSummary]").addClass('tab-inactive');
        $("[id$=lnkScenario]").removeClass('tab-active');
        $("[id$=lnkScenario]").addClass('tab-inactive');
        $("[id$=tab1]").removeClass('tab-active');
        $("[id$=tab1]").addClass('tab-inactive');
        $("[id$=tab2]").removeClass('tab-inactive');
        $("[id$=tab2]").addClass('tab-active');
        $("[id$=tab3]").removeClass('tab-active');
        $("[id$=tab3]").addClass('tab-inactive');
        $("[id$=tab4]").removeClass('tab-active');
        $("[id$=tab4]").addClass('tab-inactive');
        $("[id$=pnlSave]").show();

    }
    else if (flag == '3') {
        $("[id$=hdnTabListNew]").val(flag);
        $("[id$=divAdvanceFilter]").hide();
        $("[id$=divPlanning]").hide();
        $("[id$=divSummary]").show();
        $("[id$=divScenario]").hide();
        $("[id$=lnkPendingOrders]").removeClass('tab-active');
        $("[id$=lnkDetails]").addClass('tab-inactive');
        $("[id$=lnkPlanning]").removeClass('tab-active');
        $("[id$=lnkPlanning]").addClass('tab-inactive');
        $("[id$=lnkSummary]").removeClass('tab-inactive');
        $("[id$=lnkSummary]").addClass('tab-active');
        $("[id$=lnkScenario]").removeClass('tab-active');
        $("[id$=lnkScenario]").addClass('tab-inactive');
        $("[id$=tab1]").removeClass('tab-active');
        $("[id$=tab1]").addClass('tab-inactive');
        $("[id$=tab2]").removeClass('tab-active');
        $("[id$=tab2]").addClass('tab-inactive');
        $("[id$=tab3]").removeClass('tab-inactive');
        $("[id$=tab3]").addClass('tab-active');
        $("[id$=tab4]").removeClass('tab-active');
        $("[id$=tab4]").addClass('tab-inactive');
        $("[id$=pnlSave]").hide();
    }
    else if (flag == '4') {
        $("[id$=hdnTabListNew]").val(flag);
        $("[id$=divAdvanceFilter]").hide();
        $("[id$=divPlanning]").hide();
        $("[id$=divSummary]").hide();
        $("[id$=divScenario]").show();

        $("[id$=pnlFinalize]").hide();
        $("[id$=pnlRevert]").hide();
        $("[id$=pnlPrint]").hide();

        $("[id$=lnkPendingOrders]").removeClass('tab-active');
        $("[id$=lnkDetails]").addClass('tab-inactive');
        $("[id$=lnkPlanning]").removeClass('tab-active');
        $("[id$=lnkPlanning]").addClass('tab-inactive');
        $("[id$=lnkSummary]").removeClass('tab-active');
        $("[id$=lnkSummary]").addClass('tab-inactive');
        $("[id$=lnkScenario]").removeClass('tab-inactive');
        $("[id$=lnkScenario]").addClass('tab-active');
        $("[id$=tab1]").removeClass('tab-active');
        $("[id$=tab1]").addClass('tab-inactive');
        $("[id$=tab2]").removeClass('tab-active');
        $("[id$=tab2]").addClass('tab-inactive');
        $("[id$=tab3]").removeClass('tab-active');
        $("[id$=tab3]").addClass('tab-inactive');
        $("[id$=tab4]").removeClass('tab-inactive');
        $("[id$=tab4]").addClass('tab-active');
        $("[id$=pnlSave]").hide();
    }
}

function ShowSummaryTabs(flag) {
    //    if (flag == '1') {
    //        $("[id$=hdnSummaryTab]").val(flag);
    //        $("[id$=divSummary1]").show();
    //        $("[id$=divSummary2]").hide();
    //        $("[id$=lnkSummary1]").removeClass('tab-inactive');
    //        $("[id$=lnkSummary1]").addClass('tab-active');
    //        $("[id$=lnkSummary2]").removeClass('tab-active');
    //        $("[id$=lnkSummary2]").addClass('tab-inactive');
    //    }
    //    else if (flag == '2') {
    //        $("[id$=hdnSummaryTab]").val(flag);
    //        $("[id$=divSummary1]").hide();
    //        $("[id$=divSummary2]").show();
    //        $("[id$=lnkSummary1]").removeClass('tab-active');
    //        $("[id$=lnkSummary1]").addClass('tab-inactive');
    //        $("[id$=lnkSummary2]").removeClass('tab-inactive');
    //        $("[id$=lnkSummary2]").addClass('tab-active');
    //    }
}


function ShowHideAdvancedSearch(flag) {//collapse panel for Check list section
    if (flag == 1) {
        $("[id$=tblOPadvancedSearch]").show();
        $("[id$=imbShowFilter]").hide();
        $("[id$=imbHideFilter]").show();
    }
    else {
        $("[id$=tblOPadvancedSearch]").hide();
        $("[id$=imbShowFilter]").show();
        $("[id$=imbHideFilter]").hide();
    }
    $("[id$=hdfShowHideFilter]").val(flag);
    return false;
}

function ViewMode(flag) {
    if (flag == 1) {
        $("[id$=btnSave]").hide();
        $("[id$=btnFinalize]").hide();
        $("[id$=btnRevise]").hide();
        $("[id$=pnlDelete]").hide();
        $("[id$=pnlRevert]").hide();
        $("[id$=btnPrint]").show();
        $("[id$=pnlCancelSubmit]").hide();
    }
}

function NewModeBtnvisibility() {
    $("[id$=btnPrint]").hide();
    $("[id$=pnlDelete]").hide();
    $("[id$=pnlCancelSubmit]").hide();
}

function ReviseMode(flag) {
    if (flag == 1) {
        $("[id$=pnlDelete]").hide();
        $("[id$=btnFinalize]").hide();
        $("[id$=btnSave]").hide();
        if ($("[id$=hdnTabListNew]").val() == 2) {
            $("[id$=btnRevise]").show();
            if ($("[id$=hdfshowRevert]").val() == 1)
                $("[id$=pnlRevert]").show();
            else
                $("[id$=pnlRevert]").hide();
        }
        else {
            $("[id$=btnRevise]").hide();
            $("[id$=pnlRevert]").hide();
        }
    }
    else {
        if ($("[id$=hdnTabListNew]").val() == 2) {
            $("[id$=pnlDelete]").show();
            $("[id$=btnFinalize]").show();
            $("[id$=btnSave]").show();
        }
        else {
            $("[id$=pnlDelete]").hide();
            $("[id$=btnFinalize]").hide();
            $("[id$=btnSave]").hide();
        }
        $("[id$=pnlRevert]").hide();

    }
}



function DisableHeader() {
    $("[id$=txtFromDate]").attr("disabled", "disabled");
    $("[id$=txtToDate]").attr("disabled", "disabled");
    $("[id$=txtPlanCode]").attr("disabled", "disabled");
    $("[id$=txtPlanName]").attr("disabled", "disabled");
}
function CheckPlanName(src, args) {
    if ($("[id$=txtPlanName]").val() == '') {
        args.IsValid = false;
        return;
    }
    else {
        args.IsValid = true;
        return;
    }
}

function CheckPlanCode(src, args) {
    if ($("[id$=txtPlanCode]").val() == '') {
        args.IsValid = false;
        return;
    }
    else {
        args.IsValid = true;
        return;
    }
}


function CheckFromDate(src, args) {
    if ($("[id$=txtFromDate]").val() == '') {
        args.IsValid = false;
        return;
    }
    else {
        args.IsValid = true;
        return;
    }
}

function CheckToDate(src, args) {
    if ($("[id$=txtToDate]").val() == '') {
        args.IsValid = false;
        return;
    }
    else {
        args.IsValid = true;
        return;
    }
}

function ShowHideExpand() {
    ///<summary>
    /// Used to Show/Hide Grid Expad Button
    ///</summary>
    $("[id*=hdfHasChildren]").each(function () {
        $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
    });
}

function AfterGridExpand(row) { 
    if ($("[id$=grdGrouplist]").attr('id') == $(row).parent().parent().attr('id')) {
        $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
        var hdf = $(row).find("[id*=hdfIsExpandedGroupItem]");
        if (hdf.val() == "0") {
            $(row).find("input[id*=btnGroupDetails]").click();
        }
        else
            SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
    }
    else if ($("[id$=grdSummaryPlant]").attr('id') == $(row).parent().parent().attr('id')) {
        $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
        var hdf2 = $(row).find("[id*=hdfIsExpandedPlantItem]");
        if (hdf2.val() == "0") {
            $(row).find("input[id*=btnPlantDetails]").click();
        }
        else
            SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
    }

    if ($(row).parent().parent().parent().find("[id*=grdLineSummary]").attr('id') == $(row).parent().parent().attr('id')) {
        $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
        var hdf3 = $(row).find("[id*=hdfIsExpandedLineItem]");
        if (hdf3.val() == "0") {
            $(row).find("input[id*=btnLineDetails]").click();
        }
        else
            SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
    }
    else if ($(row).parent().parent().parent().find("[id*=grdSize]").attr('id') == $(row).parent().parent().attr('id')) {
        $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
        var hdf2 = $(row).find("[id*=hdfIsExpandedSizeItem]");
        if (hdf2.val() == "0") {
            $(row).find("input[id*=btnSizeDetails]").click();
        }
        else
            SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
    }
    else if ($("[id$=grdGrouplistScenario]").attr('id') == $(row).parent().parent().attr('id')) {
        $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
        var hdf = $(row).find("[id*=hdfIsExpandedGroupItem]");
        if (hdf.val() == "0") {
            $(row).find("input[id*=btnGroupDetails]").click();
        }
        else
            SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
    }
}

function SetGridScroll(rowId) {
    if (rowId)
        rowArray = $("[id$=" + rowId + "]");
    else
        rowArray = $("[id$=_ExpandPosition]");
    rowArray.each(function () {
        if ($.trim($(this).val()) != "") {
            var containerDiv = $(this).parent("[id$=_ScrollContainer]");
            if (containerDiv != null) {
                $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
            }
        }
        $(this).val("")
    });
}

//To select/ Deselect and enable/disable controls along with checkbox checking
function CheckPlannedItems(evt) {
    //$('#updateProgress').show();
    var TotalPlanQty = 0;
    var isChecked;
    if (!evt) {
        $("[id*=ChkPlanOrder]").each(function () {
            var cblId = $(this).attr("id");
            var allNode;
            allNode = $("[id$=" + cblId + "]").find('input[value="-1"]');
            isChecked = allNode.attr("checked") == "checked";
            if (isChecked) {
                $("[id$=" + cblId + "]").find("tr:has(td)").each(function () {
                    $(this).find("td:nth-child(2) input[type=checkbox]").attr("checked", true);
                });
            }
        });
    }
    else {
        var src = window.event != window.undefined ? window.event.srcElement : evt.target;
        var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
        if (isChkBoxClick) {
            var listID;
            isChecked = src.checked;
            var listID = $(src).parents("table:first").attr("id");
            if ($(src).attr("id") == $("[id$=ChkPlanAll]").attr("id")) {
                if (isChecked) {
                    $("[id$=" + listID + "] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
                        if ($(this).parent().parent().find("[id*=lnbLine_" + index + "]").html() == 0) {
                            $("#[id*=" + $(this).parent().parent().find("[id*=ChkPlanOrder]").attr("id") + "]").attr("checked", true);
                            var ToPlanQty = $(this).parent().parent().find("[id*=lblBalancetoPlan_" + index + "]").html();
                            if (ToPlanQty != null) {
                                $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(ReplaceCommas(ToPlanQty));
                                FormatAmountwithID($(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id"));
                                $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").removeAttr("disabled");
                                $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").removeClass("input-disabled");
                                $("#[id*=" + $(this).parent().parent().find("[id*=imbAddLine]").attr("id") + "]").show();
                                EnableDisablePlanQty(index, 1, ReplaceCommas(ToPlanQty));
                                TotalPlanQty = parseFloat(TotalPlanQty) + parseFloat(ReplaceCommas(ToPlanQty));
                                $(this).closest('tr').addClass('Selection');
                                $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").addClass('txt-green bold');
                                if (ToPlanQty != $("#[id*=" + $(this).parent().parent().find("[id*=lnbLine]").attr("id") + "]").html())
                                    $("#[id*=" + $(this).parent().parent().find("[id*=lnbLine]").attr("id") + "]").addClass('txt-Orange bold');
                            }
                        }
                    });
                }
                else {
                    $("[id$=" + listID + "] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
                        $(this).closest('tr').css('background-color', 'White');
                        var ToPlanQty = $(this).parent().parent().find("[id*=lblBalancetoPlan_" + index + "]").html();
                        if ($(this).parent().parent().find("[id*=lnbLine_" + index + "]").html() == 0) {
                            $("#[id*=" + $(this).parent().parent().find("[id*=ChkPlanOrder]").attr("id") + "]").removeAttr("checked");
                            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val('0');
                            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").attr("disabled", "disabled");
                            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").addClass("input-disabled");
                            $("#[id*=" + $(this).parent().parent().find("[id*=imbAddLine]").attr("id") + "]").hide();
                            EnableDisablePlanQty(index, 0, 0);
                            TotalPlanQty = 0;
                            $(this).closest('tr').removeClass('Selection');
                            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").removeClass('txt-red txt-green bold errorInput');
                            $("#[id*=" + $(this).parent().parent().find("[id*=lnbLine]").attr("id") + "]").removeClass('txt-Orange bold');
                        }
                    });
                }

                if (parseFloat(TotalPlanQty) > 0)
                    $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(addCommas(TotalPlanQty.toFixed(0)));
                else
                    $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(TotalPlanQty);
            }
            else {
                var isAll = true;
                var rowId = $(src).attr("id").split("_");
                $("[id$=" + listID + "]").find("tr:has(td)").each(function (index) {
                    if (($(this).find("td:nth-child(2) input[type=checkbox]").attr("id") != $("[id$=ChkPlanAll]").attr("id")) && ($(this).find("td:nth-child(2) input[type=checkbox]").attr("checked") != "checked")) {
                        isAll = false;
                    }
                    if (isAll) {
                        $("[id$=" + listID + "]").find('input[id$=ChkPlanAll]').attr("checked", true);
                    }
                    else {
                        $("[id$=" + listID + "]").find('input[id$=ChkPlanAll]').removeAttr("checked");
                    }
                });

                if ($(src).attr("checked") == true)
                    EnableDisablePlanQty(rowId[rowId.length - 1], 1, ReplaceCommas($("[id$=grdGrouplist]").find("[id*=txtTotalCurrPlan_" + rowId[rowId.length - 1] + "]").val()));
                else
                    EnableDisablePlanQty(rowId[rowId.length - 1], 0, 0);
            }
        }
    }
}

//To enable/disable gridview row control
function EnableCurrPlan(Currow) {
    if (Currow.find("[id*=ChkPlanOrder]").attr("checked") == true) {
        var ToPlanQty = $(Currow).closest('tr').find("[id*=lblBalancetoPlan]").html();
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(ReplaceCommas(ToPlanQty));
        FormatAmountwithID($(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id"));
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").removeAttr("disabled");
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").removeClass("input-disabled");
        $(Currow).closest('tr').find("[id*=imbAddLine]").show();
        $(Currow).closest('tr').addClass('Selection');
    }
    else {
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").val('0');
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").attr("disabled", "disabled");
        $("#[id*=" + $(Currow).closest('tr').find("[id*=txtTotalCurrPlan]").attr("id") + "]").addClass("input-disabled");
        $(Currow).closest('tr').find("[id*=imbAddLine]").hide();
        Currow.closest('tr').removeClass('Selection');
    }
    CalculateGrouplistTotal();
}

//To maintain gridview state while postback
function SetGridViewState() {
    $("[id$=grdGrouplist] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
        if ($(this).parent().parent().find("[id*=ChkPlanOrder_" + index + "]").attr("checked") == true) {
            $(this).parent().parent().find("[id*=txtTotalCurrPlan_" + index + "]").removeAttr("disabled");
            $(this).parent().parent().find("[id*=txtTotalCurrPlan_" + index + "]").removeClass("input-disabled");
            $(this).parent().parent().find("[id*=imbAddLine_" + index + "]").show();
            var rowId = $(this).parent().parent().find("input[id*=txtTotalCurrPlan]").attr("id").split("_");
            EnableDisablePlanQty(rowId[rowId.length - 1], 1, 0);
        }
    });
}

//To enable/ disable child gridview control
function EnableDisablePlanQty(rowIndex, Enable, Balance) {
    var grdId = "grdOrderDtls_" + rowIndex;
    $("[id*=" + grdId + "] tr:has(td)").each(function (index) {
        if (Enable == 1) {
            if (parseFloat(Balance) > 0) {
                if ($(this).find("[id*=txtDtlCurrPlan_" + index + "]").length != 0) {
                    var PercVal = $(this).find("[id*=hdfProportionVal_" + index + "]").val();
                    var BalanceQty = (parseFloat(Balance) * parseFloat(PercVal)) / 100;
                    $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").val(BalanceQty.toFixed(0));
                    FormatAmountwithID($(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id"));
                }
            }
            $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").removeAttr("disabled");
            $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").removeClass("input-disabled");
        }
        else {
            $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").val('0');
            $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").attr("disabled", "disabled");
            $("#[id*=" + $(this).find("[id*=txtDtlCurrPlan_" + index + "]").attr("id") + "]").addClass("input-disabled");
        }
    });
}


function ReplaceCommas(num) {
    return num.replace(/,/g, "");
}
//To change child grid qty based on main grid qty change
function GroupQtyChanged(Currow) {
    var DtlTotal = 0;
    var PlanQty = ReplaceCommas($("#[id*=" + Currow.find("[id*=txtTotalCurrPlan]").attr("id") + "]").val());
    var rowId = Currow.find("[id*=txtTotalCurrPlan]").attr("id").split("_");
    var grdId = "grdSize_" + rowId[rowId.length - 1];
    $("[id*=" + grdId + "] tr:has(td)").each(function (index) {
        if ($(this).find("[id*=hdfProportionVal_" + index + "]").length != 0) {
            var PercVal = $(this).find("[id*=hdfProportionVal_" + index + "]").val();
            var BalanceQty = (parseFloat(PlanQty) * parseFloat(PercVal)) / 100;
            DtlTotal = parseFloat(DtlTotal) + parseFloat(Math.round(BalanceQty).toFixed(0));
            if (index == $("[id*=" + grdId + "] tr:has(td)").length - 1) {
                var diff = parseFloat(PlanQty) - parseFloat(DtlTotal);
                BalanceQty = parseFloat(BalanceQty) + parseFloat(diff);
            }
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").val(Math.round(BalanceQty).toFixed(0));
            FormatAmountwithID($(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id"));
        }
        $(this).find("[id*=hdfIsApplied_" + index + "]").val(0);
    });
    CalculateGrouplistTotal();
}
//To change child grid qty based on main grid qty change
function ScenarioGroupQtyChanged(Currow) {
    var DtlTotal = 0;
    var PlanQty = ReplaceCommas($("#[id*=" + Currow.find("[id*=txtTotalCurrPlan]").attr("id") + "]").val());
    
    var rowId = Currow.find("[id*=txtTotalCurrPlan]").attr("id").split("_");
    var grdId = "grdSizeScenario_" + rowId[rowId.length - 1]; 
    $("[id*=" + grdId + "] tr:has(td)").each(function (index) {
        if ($(this).find("[id*=hdfProportionVal_" + index + "]").length != 0) {
            var PercVal = $(this).find("[id*=hdfProportionVal_" + index + "]").val();
             
            var BalanceQty = (parseFloat(PlanQty) * parseFloat(PercVal)) / 100;
            DtlTotal = parseFloat(DtlTotal) + parseFloat(Math.round(BalanceQty).toFixed(0)); 
            if (index == $("[id*=" + grdId + "] tr:has(td)").length - 1) {
                var diff = parseFloat(PlanQty) - parseFloat(DtlTotal);
                BalanceQty = parseFloat(BalanceQty) + parseFloat(diff);
            }
            $("#[id*=" + $(this).find("[id*=txtDtlPlanNow_" + index + "]").attr("id") + "]").val(Math.round(BalanceQty).toFixed(0));
            //FormatAmountwithID($(this).find("[id*=txtDtlPlanNow_" + index + "]").attr("id")); 
        } 
    });
   // CalculateGrouplistTotal();
}
//To change main grid qty based on child grid qty change
function OrderQtyChanged(Currow) {
    var TotalPlanQty = 0;
    var ToPlanQty = 0;
    var grdTotalPlanQty = 0;
    var PlanQtyDtl = $("#[id*=" + Currow.find("[id*=txtDtlCurrPlan]").attr("id") + "]").val();
    var rowId = Currow.find("[id*=txtDtlCurrPlan]").attr("id").split("_");
    var grdId = "grdOrderDtls_" + rowId[3]; //To get the rowindex of child grid
    $("[id*=" + grdId + "]  input[type=text][id*=txtDtlCurrPlan]").each(function (index) {
        grdTotalPlanQty = parseFloat(grdTotalPlanQty) + parseFloat(ReplaceCommas($("#[id*=" + $(this).parent().parent().find("[id*=txtDtlCurrPlan]").attr("id") + "]").val()));
    });
    $("[id$=grdGrouplist] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
        if (parseInt(rowId[3]) == index) {
            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(grdTotalPlanQty.toFixed(0));
            FormatAmountwithID($("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").attr("id"));
        }
        ToPlanQty = $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val();
        TotalPlanQty = parseFloat(TotalPlanQty) + parseFloat(ReplaceCommas(ToPlanQty));
    });
    if (parseFloat(TotalPlanQty) > 0)
        $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(addCommas(TotalPlanQty.toFixed(0)));
    else
        $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(TotalPlanQty);
    CalculateGrouplistTotal();
}

//Calculates the total for group grid and also validations for pcs checking
function CalculateGrouplistTotal() {
    var TotalPlanQty = 0;
    var TotalBalQty = 0;
    var TotalPlannedQty = 0;
    var TotalLineQty = 0;
    var stat = false;
    $("#[id*=grdGrouplist] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
        var PlanQty = 0;
        var BalQty = 0;
        var PlannedQty = 0;
        var LineQty = 0;
        if ($("[id$=grdGrouplist]").attr('id') == $(this).closest('tr').parent().parent().attr('id')) {
            stat = true;
            if ($(this).closest('tr').find("#[id*=txtTotalCurrPlan]").val() != '')
                PlanQty = $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").val().replace(new RegExp(',', 'g'), '');
            else
                PlanQty = '0';
            TotalPlanQty = parseFloat(TotalPlanQty) + parseFloat(PlanQty);

            if ($(this).closest('tr').find("#[id*=lblBalancetoPlan]").html() != '')
                BalQty = $(this).closest('tr').find("#[id*=lblBalancetoPlan]").html().replace(new RegExp(',', 'g'), '');
            else
                BalQty = '0'
            TotalBalQty = parseFloat(TotalBalQty) + parseFloat(BalQty);

            if ($(this).closest('tr').find("#[id*=lblTotalPlanned]").html() != '' && $(this).closest('tr').find("#[id*=lblTotalPlanned]").html() != 'null' && $(this).closest('tr').find("#[id*=lblTotalPlanned]").html() != null)
                PlannedQty = $(this).closest('tr').find("#[id*=lblTotalPlanned]").html().replace(new RegExp(',', 'g'), '');
            else
                PlannedQty = '0';
            TotalPlannedQty = parseFloat(TotalPlannedQty) + parseFloat(PlannedQty);

            if ($(this).closest('tr').find("#[id*=lnbLine]").html() != '' && $(this).closest('tr').find("#[id*=lnbLine]").html() != 'null' && $(this).closest('tr').find("#[id*=lnbLine]").html() != null)
                LineQty = $(this).closest('tr').find("#[id*=lnbLine]").html().replace(new RegExp(',', 'g'), '');
            else
                LineQty = '0';
            TotalLineQty = parseFloat(TotalLineQty) + parseFloat(LineQty);

            if ($(this).closest('tr').find("#[id*=ChkPlanOrder]").attr("checked") == true) {
                if (parseFloat(PlanQty) == 0 || parseFloat(PlanQty) < 0)
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('errorInput');
                else
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('errorInput');

                if (parseFloat(PlanQty) >= parseFloat(BalQty)) {
                    $(this).closest('tr').addClass('Selection');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('txt-red');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('txt-green bold');
                }
                else {
                    $(this).closest('tr').addClass('Selection');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('txt-green');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('txt-red bold');
                }
                if (parseFloat(PlanQty) != parseFloat(LineQty)) {
                    $(this).closest('tr').find("[id*=lnbLine]").addClass('txt-Orange bold');
                }
                else {
                    $(this).closest('tr').find("[id*=lnbLine]").removeClass('txt-Orange bold');
                }
            }
            else {
                $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('txt-green txt-red bold errorInput');
                $(this).closest('tr').find("[id*=lnbLine]").removeClass('txt-Orange bold');
            }

            if (parseFloat(LineQty) > 0)
                $(this).closest('tr').find("#[id*=ChkPlanOrder]").attr('disabled', 'disabled');
        }
    });
    if (stat) {
        if (parseFloat(TotalPlanQty) > 0)
            $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(addCommas(TotalPlanQty.toFixed(0)));
        else
            $("#[id*=grdGrouplist] [id*=lblTotalPlanQty]").html(TotalPlanQty);

        if (parseFloat(TotalBalQty) > 0)
            $("#[id*=grdGrouplist] [id*=lblTotalBalQty]").html(addCommas(TotalBalQty.toFixed(0)));
        else
            $("#[id*=grdGrouplist] [id*=lblTotalBalQty]").html(TotalBalQty);

        if (parseFloat(TotalPlannedQty) > 0)
            $("#[id*=grdGrouplist] [id*=lblTotalPlannedQty]").html(addCommas(TotalPlannedQty.toFixed(0)));
        else
            $("#[id*=grdGrouplist] [id*=lblTotalPlannedQty]").html(TotalPlannedQty);

        if (parseFloat(TotalLineQty) > 0)
            $("#[id*=grdGrouplist] [id*=lblTotalLineQty]").html(addCommas(TotalLineQty.toFixed(0)));
        else
            $("#[id*=grdGrouplist] [id*=lblTotalLineQty]").html(TotalLineQty);
    }
}

//Calculates the total for group grid and also validations for pcs checking
function CalculateScenarioListTotal() { 
    if ($("[id$=hdnTabListNew]").val() == '4') {
        var TotalPlanQty = 0;
        var TotalBalQty = 0;
        var TotalPlannedQty = 0;
        var TotalLineQty = 0;
        var stat = false;
        $("#[id*=grdGrouplistScenario] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
            var PlanQty = 0;
            var BalQty = 0;
            var PlannedQty = 0;
            var LineQty = 0;

            if ($("[id$=grdGrouplistScenario]").attr('id') == $(this).closest('tr').parent().parent().attr('id')) {
                stat = true;
                if ($(this).closest('tr').find("#[id*=txtTotalCurrPlan]").val() != '')
                    PlanQty = $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").val().replace(new RegExp(',', 'g'), '');
                else
                    PlanQty = '0';
                TotalPlanQty = parseFloat(TotalPlanQty) + parseFloat(PlanQty);

                if ($(this).closest('tr').find("#[id*=lblBalancetoPlan]").html() != '')
                    BalQty = $(this).closest('tr').find("#[id*=lblBalancetoPlan]").html().replace(new RegExp(',', 'g'), '');
                else
                    BalQty = '0'
                TotalBalQty = parseFloat(TotalBalQty) + parseFloat(BalQty);

                if ($(this).closest('tr').find("#[id*=lblProdQty]").html() != '' && $(this).closest('tr').find("#[id*=lblProdQty]").html() != 'null' && $(this).closest('tr').find("#[id*=lblProdQty]").html() != null)
                    LineQty = $(this).closest('tr').find("#[id*=lblProdQty]").html().replace(new RegExp(',', 'g'), '');
                else
                    LineQty = '0';
                TotalLineQty = parseFloat(TotalLineQty) + parseFloat(LineQty);

                if (parseFloat(PlanQty) == 0 || parseFloat(PlanQty) < 0)
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('errorInput');
                else
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('errorInput');

                if (parseFloat(PlanQty) >= parseFloat(BalQty)) {
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('txt-red');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('txt-green bold');
                }
                else {
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").removeClass('txt-green');
                    $(this).closest('tr').find("#[id*=txtTotalCurrPlan]").addClass('txt-red bold');
                }
                if (parseFloat(PlanQty) != parseFloat(LineQty)) {
                    $(this).closest('tr').find("[id*=lblProdQty]").addClass('txt-Orange bold');
                }
                else {
                    $(this).closest('tr').find("[id*=lblProdQty]").removeClass('txt-Orange bold');
                }
            }
        });

        if (stat) {
            if (parseFloat(TotalPlanQty) > 0)
                $("#[id*=grdGrouplistScenario] [id*=lblTotalPlanQty]").html(addCommas(TotalPlanQty.toFixed(0)));
            else
                $("#[id*=grdGrouplistScenario] [id*=lblTotalPlanQty]").html(TotalPlanQty);

            if (parseFloat(TotalBalQty) > 0)
                $("#[id*=grdGrouplistScenario] [id*=lblTotalBalQty]").html(addCommas(TotalBalQty.toFixed(0)));
            else
                $("#[id*=grdGrouplistScenario] [id*=lblTotalBalQty]").html(TotalBalQty);

            if (parseFloat(TotalLineQty) > 0)
                $("#[id*=grdGrouplistScenario] [id*=lblTotalProdnQty]").html(addCommas(TotalLineQty.toFixed(0)));
            else
                $("#[id*=grdGrouplistScenario] [id*=lblTotalProdnQty]").html(TotalLineQty);
        }
    }
}

function addCommas(number) {
    var FormattedNumber = number;
    var curGroup1 = 3;
    var curGroup2 = 3;
    var NumericPart = "", LastNumericPart = "", DecimalPart = "";
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
        curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
    }
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
        curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
    }
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

//To calculate Plan qty with A grade perc qty added
function CalculateTotalwithAGrade(curRow) {
    if (curRow != 'undefined' && curRow != '' && curRow != null) {
        var NewPlanQty = 0;
        if (curRow.find("[id*=ChkPlanOrder]").attr("checked") == true) {
            var AGradePerc = $("#[id*=" + curRow.find("[id*=hdfAGradePercDtl]").attr("id") + "]").val();
            var PlanQty = $("#[id*=" + curRow.find("[id*=txtTotalCurrPlan_]").attr("id") + "]").val();
            if (parseFloat(AGradePerc) > 0 && parseFloat(AGradePerc) < 100)
                NewPlanQty = (parseFloat(ReplaceCommas(PlanQty)) / parseFloat(AGradePerc)) * 100;
            else
                NewPlanQty = ReplaceCommas(PlanQty);

            $("#[id*=" + curRow.find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(NewPlanQty);
            FormatAmountwithID(curRow.find("[id*=txtTotalCurrPlan]").attr("id"));

            var rowId = curRow.find("input[id*=txtTotalCurrPlan]").attr("id").split("_");
            AGradeCalcforOrderGrid(rowId[rowId.length - 1], 1, AGradePerc);
        }
    }
    else {
        $("[id$=grdGrouplist] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
            if ($(this).parent().parent().find("[id*=ChkPlanOrder_" + index + "]").attr("checked") == true) {
                var NewPlanQty = 0;
                var AGradePerc = $("[id$=hdfAGradePerc]").val();
                var PlanQty = $(this).parent().parent().find("[id*=txtTotalCurrPlan_" + index + "]").val();
                if (parseFloat(AGradePerc) > 0 && parseFloat(AGradePerc) < 100)
                    NewPlanQty = (parseFloat(ReplaceCommas(PlanQty)) / parseFloat(AGradePerc)) * 100;
                else
                    NewPlanQty = ReplaceCommas(PlanQty);
                $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(NewPlanQty);
                FormatAmountwithID($(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id"));

                var rowId = $(this).parent().parent().find("input[id*=txtTotalCurrPlan]").attr("id").split("_");
                AGradeCalcforOrderGrid(rowId[rowId.length - 1], 1, AGradePerc);
            }
        });
    }
    CalculateGrouplistTotal();
    return false;
}
//To calculate Plan qty with A grade perc qty added
function CalculateTotalwithAGradeScenario(curRow) {
    if (curRow != 'undefined' && curRow != '' && curRow != null) {
        var NewPlanQty = 0; 
        var AGradePerc = $("#[id*=" + curRow.find("[id*=hdfAGradePercDtl]").attr("id") + "]").val();
        var PlanQty = $("#[id*=" + curRow.find("[id*=txtTotalCurrPlan_]").attr("id") + "]").val();
        if (parseFloat(AGradePerc) > 0 && parseFloat(AGradePerc) < 100)
            NewPlanQty = (parseFloat(ReplaceCommas(PlanQty)) / parseFloat(AGradePerc)) * 100;
        else
            NewPlanQty = ReplaceCommas(PlanQty);

        $("#[id*=" + curRow.find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(NewPlanQty);
        FormatAmountwithID(curRow.find("[id*=txtTotalCurrPlan]").attr("id"));
        ScenarioGroupQtyChanged(curRow);
    }
    else {
        $("[id$=grdGrouplistScenario] input[type=text][id*=txtTotalCurrPlan]").each(function (index) {
            var NewPlanQty = 0;
            var AGradePerc = $(this).parent().parent().find("[id*=hdfAGradePercDtl_" + index + "]").val(); // $("[id$=hdfAGradePerc]").val();
            var PlanQty = $(this).parent().parent().find("[id*=txtTotalCurrPlan_" + index + "]").val();
            if (parseFloat(AGradePerc) > 0 && parseFloat(AGradePerc) < 100)
                NewPlanQty = (parseFloat(ReplaceCommas(PlanQty)) / parseFloat(AGradePerc)) * 100;
            else
                NewPlanQty = ReplaceCommas(PlanQty);

            $("#[id*=" + $(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id") + "]").val(NewPlanQty);
            ScenarioGroupQtyChanged($(this).parent().parent());
            FormatAmountwithID($(this).parent().parent().find("[id*=txtTotalCurrPlan]").attr("id"));
        });
    }
    CalculateScenarioListTotal();
    return false;
}
//To enable/ disable child gridview control
function AGradeCalcforOrderGrid(rowIndex, Enable, GradeVal) {
    var grdId = "grdSize_" + rowIndex;
    $("[id*=" + grdId + "] tr:has(td)").each(function (index) {
        if (Enable == 1) {
            if ($(this).find("[id*=txtSizePlanNow_" + index + "]").length != 0) {
                var DtlQty = $(this).find("[id*=txtSizePlanNow_" + index + "]").val();
                var BalanceQty = (parseFloat(ReplaceCommas(DtlQty)) / parseFloat(GradeVal)) * 100;
                $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").val(Math.round(BalanceQty).toFixed(0));
                FormatAmountwithID($(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id"));
            }
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").removeAttr("disabled");
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").removeClass("input-disabled");
        }
        else {
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").val('0');
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").attr("disabled", "disabled");
            $("#[id*=" + $(this).find("[id*=txtSizePlanNow_" + index + "]").attr("id") + "]").addClass("input-disabled");
        }
    });
}

function CalculateLineQtyTotal() {
    var TotalLineQty = 0;
    var TotalProdHrs = 0;
    $("#[id*=grdLines] input[type=hidden][id*=hdfLinePK]").each(function (index) {
        var LineQty = 0;
        var ProdHrs = 0;

        if ($(this).closest('tr').find("#[id*=lblLineQty]").html() != '')
            LineQty = $(this).closest('tr').find("#[id*=lblLineQty]").html().replace(new RegExp(',', 'g'), '');
        else
            LineQty = '0';
        TotalLineQty = parseFloat(TotalLineQty) + parseFloat(LineQty);

        if ($(this).closest('tr').find("#[id*=lblProdHrs]").html() != '')
            ProdHrs = $(this).closest('tr').find("#[id*=lblProdHrs]").html().replace(new RegExp(',', 'g'), '');
        else
            ProdHrs = '0';
        TotalProdHrs = parseFloat(TotalProdHrs) + parseFloat(ProdHrs);
        
    });

    if (parseFloat(TotalLineQty) > 0)
        $("#[id*=grdLines] [id*=lblTotalLineQty]").html(addCommas(TotalLineQty.toFixed(0)));
    else
        $("#[id*=grdLines] [id*=lblTotalLineQty]").html(TotalLineQty);

    if (parseFloat(TotalProdHrs) > 0)
        $("#[id*=grdLines] [id*=lblTotalProdHrs]").html(addCommas(TotalProdHrs.toFixed(0)));
    else
        $("#[id*=grdLines] [id*=lblTotalProdHrs]").html(TotalProdHrs);
}

function CalculateSCPopUpTotal() {
    var TotalSCPlannedQty = 0;
    var TotalSCBalQty = 0;
    var TotalSCPlanNowQty = 0;
    $("#[id*=grdOrderDtls] input[type=hidden][id*=hdfOrderPk]").each(function (index) {
        var PlannedQty = 0;
        var BalQty = 0;
        var PlanNowQty = 0;

        if ($(this).closest('tr').find("#[id*=lblOrderPlannedQty]").html() != '')
            PlannedQty = $(this).closest('tr').find("#[id*=lblOrderPlannedQty]").html().replace(new RegExp(',', 'g'), '');
        else
            PlannedQty = '0';
        if ($(this).closest('tr').find("#[id*=lblOrderBalQty]").html() != '')
            BalQty = $(this).closest('tr').find("#[id*=lblOrderBalQty]").html().replace(new RegExp(',', 'g'), '');
        else
            BalQty = '0';
        if ($(this).closest('tr').find("#[id*=txtDtlCurrPlan]").val() != '')
            PlanNowQty = $(this).closest('tr').find("#[id*=txtDtlCurrPlan]").val().replace(new RegExp(',', 'g'), '');
        else
            PlanNowQty = '0';

        TotalSCPlannedQty = parseFloat(TotalSCPlannedQty) + parseFloat(PlannedQty);
        TotalSCBalQty = parseFloat(TotalSCBalQty) + parseFloat(BalQty);
        TotalSCPlanNowQty = parseFloat(TotalSCPlanNowQty) + parseFloat(PlanNowQty);
    });

    if (parseFloat(TotalSCPlannedQty) > 0)
        $("#[id*=grdOrderDtls] [id*=lblSCPlannedTotal]").html(addCommas(TotalSCPlannedQty.toFixed(0)));
    else
        $("#[id*=grdOrderDtls] [id*=lblSCPlannedTotal]").html(TotalSCPlannedQty);

    if (parseFloat(TotalSCBalQty) > 0)
        $("#[id*=grdOrderDtls] [id*=lblSCBalTotal]").html(addCommas(TotalSCBalQty.toFixed(0)));
    else
        $("#[id*=grdOrderDtls] [id*=lblSCBalTotal]").html(TotalSCBalQty);

    if (parseFloat(TotalSCPlanNowQty) > 0)
        $("#[id*=grdOrderDtls] [id*=lblSCPlanNowTotal]").html(addCommas(TotalSCPlanNowQty.toFixed(0)));
    else
        $("#[id*=grdOrderDtls] [id*=lblSCPlanNowTotal]").html(TotalSCPlanNowQty);

    $("[id$=hdfSCPopUpTotal]").val(TotalSCPlanNowQty);

}

function HighlightFinalizedRow() {
    $("#[id*=grdPlanList] tr:has(td)").each(function (index) {
        if (parseFloat($(this).closest('tr').find("#[id*=lblVersion]").html()) > 0) {
            $(this).closest('tr').css('background-color', '#e1fbee');
        }
    });
}
function AfterClose(containerID) {
    if (containerID == "[id$=divSCDetails]") {
        $("[id$=hdfIsSCcontYes]").val('0');
    }
}

//<summary>Function used to set Start and End Dates</summary>
function SetTime() {
    $("[id$=txtLineFromTime]").timepicker();
    $("[id$=txtLineToTime]").timepicker();
    GrandScriptUtils.AddDateRangeCommon("txtLineFromDt", "hdfStartDate", "txtLineToDt", "hdfEndDate", false, false);
}

function CalculateLineCapacity() {
//    var Capacity = parseFloat($("[id$=hdfHolderPos]").val() * 60 * 24) / parseFloat($("[id$=ucLineSpeed_txtFormattedAmount]").val());
//    var StartTime = $("[id$=txtLineFromDt]").val() + " " + $("[id$=txtLineFromTime]").val();
//    var EndTime = $("[id$=txtLineToDt]").val() + " " + $("[id$=txtLineToTime]").val();
//    var durationTime = parseInt(CalculateDuration(StartTime, EndTime)) + 1;
//    var TotalCapacity = Capacity * durationTime;
//    $("[id$=lblCapacity]").html(addCommas(TotalCapacity.toFixed(0)));
}

function CalculateDuration(startTime, EndTime) {
    var unloadTm = EndTime;
    var loadTm = startTime;
    var loadTime = new Date();
    var unloadTime = new Date();
    var diff = 0;
    var durationTime = '0:00:00';
    if (unloadTm != "" && loadTm != "") {
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

        //durationTime = days + ':' + hours + ':' + minutes;
        durationTime = days;
    }
    return durationTime;
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
 

function CheckBoxSelection() {
    var gridName = 'grdMachines';
    var isChecked = $("[id$=" + gridName + "] tr:has(th)").find("[id$=chkAllActive]").attr("checked");

    $("[id$=" + gridName + "] tr:has(td)").each(function (index) {
        if (isChecked) {
            $(this).find("[id$=chkMachine_" + index + "]").attr("checked", true);
        }
        else {
            $(this).find("[id$=chkMachine_" + index + "]").attr("checked", false);
        }
    });
}

function ShowHideChkCombinedLines(flag) {
    if (flag) {
        $("[id$=chkCombinedLines]").hide();
        $("[id$=lblCombinedLines]").hide();
        $("[id$=grdGrouplistScenario_chkAllCombinedLines]").hide();
    }
    else {
        $("[id$=chkCombinedLines]").show();
        $("[id$=lblCombinedLines]").show();
        $("[id$=grdGrouplistScenario_chkAllCombinedLines]").show();
    }
} 

function ShowHidePlanSplit() {
    if ($("[id$=hdnTabListNew]").val() == '2') {
        if ($("[id$=chkOptimizePlan]").attr("checked")) {
            $("[id$=lblPlanSplit]").show();
            $("[id$=chkPlanSplit]").show();
        }
        else {
            $("[id$=chkPlanSplit]").attr("checked", false);
            $("[id$=lblPlanSplit]").hide();
            $("[id$=chkPlanSplit]").hide();
        }
    }
}
function ShowHideScenarioSplit() {
    if ($("[id$=hdnTabListNew]").val() == '4') {
        if ($("[id$=chkOptimizeScenario]").attr("checked")) {
            $("[id$=lblScenarioSplit]").show();
            $("[id$=chkScenarioSplit]").show();
        }
        else {
            $("[id$=chkScenarioSplit]").attr("checked", false);
            $("[id$=lblScenarioSplit]").hide();
            $("[id$=chkScenarioSplit]").hide();
        }
    }
}