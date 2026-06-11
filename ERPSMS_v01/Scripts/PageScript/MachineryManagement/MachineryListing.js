
///#region ------- Global Variable -----
var machinePK = 0;
///#endregion

///#region ---------- Configuration
var MachineMaster = {
    SelectOneMsg: "Translate(Pleaseselectanoption)",
    ValueZero: "0",
    TextEmpty: "",
    MachinePK: "MCH_PK",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    MACHINEGRIDURL: "MachineryManagement.do?Action=GetMachineList&Status=",
    MACHINEDTLSAUTOCOMPLETEURL: "MachineryManagement.do?Action=GetSearchMachineValue&SBU=",
    ConfirmationMsg: "Translate(Conformation)",
    InformationTitle: "Translate(Information)",
    ActionFailedMsg: "Translate(ActionFailedPleaseTryAgain)",
    DeleteMachineDtlsSuccessMsg: "Translate(MachineDetailsDeletedSuccessfully)",
    DELETEMACHINEDTLSURL: "MachineryManagement.do?Action=DeleteMachineDtls&MachineID=",
    DefaultActionMsg: "Translate(DefaultActionneedstobeperformed)",
    MachineEdit: "edit",
    MachineDelete: "delete",
    MachineView: "view",
    MachineDtlsEntryPage: "MachineryMaster.aspx",
    StatusTitle: "Translate(Status)",
    CANNOTDELETE: "Translate(CannotDelete)",
    Used: "Translate(CannotdeleteAlreadyasigned)",
    DeletedRecord: "Translate(DeletedRecord)",
    EditUsedByAnotherUser: "Translate(AlreadyUpdatedRecord)",
    SearchMachineName: "MCH_NAME"
}
///#endregion

///#region ---------- Initialization

$(document).ready(function () {
    // validate the Form
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //For Adding rule to Select
    PageInit();

});


function PageInit() {
    $("select[id$=ddlStatus]").val('1');
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("select[id$=SearchType]").val(MachineMaster.SearchMachineName);
    $("[id$=SearchValue]").val(MachineMaster.TextEmpty);
    SearchInit();
    SetSearchType();
    $("[id$=SearchType]").focus();
    return false;



}

///#endregion

///#region ---------- Core Section

//<summary>function used to Fill All Machine Details to Grid</summary>
function BindMachineGrid() {
    var ddlStatus = $("select[id$=ddlStatus]").val();
    var ajaxUrl = MachineMaster.MACHINEGRIDURL + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val() + "&SBU=" + $("[id$=BizUnitPk]").val() + "&StatusPK=" + ddlStatus;
    $("#grdMachineDetails").removeAttr("ajaxurl")
    $("#grdMachineDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMachineDetails");
    GrandGrid.MakeGrid($("#grdMachineDetails"));

    return false;
}

///<summary>function used to Handle Machine  Details Grid Command</summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function MachineGridHandler(tr, command) {

    switch (command.toString().toLowerCase()) {

        case MachineMaster.MachineDelete:
            machinePK = GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachinePK, $(tr).parent().parent().attr("id"));
            GrandScriptUtils.ShowModal(MachineMaster.DoUWantToDelMsg, MachineMaster.ConfirmationMsg, MachineMaster.MachineDelete, true);
            break;
        case MachineMaster.MachineEdit:
            machinePK = GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachinePK, $(tr).parent().parent().attr("id"));
            FillMachineDetails(machinePK);
            break;
        case MachineMaster.MachineView:
            machinePK = GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachinePK, $(tr).parent().parent().attr("id"));
            ViewMachineDetails(machinePK);
            break;
        default:
            alert(MachineMaster.DefaultActionMsg);
            break;
    }
    return false;
}

///<summary>Used to fill Machinery Details for editing</summary>
///<summary>Grid Handler Catch all the grid events in this function </summary>
/// <param name="machinePK"  type="Object">   
/// </param>
function FillMachineDetails(machinePK) {
    window.location = MachineMaster.MachineDtlsEntryPage + "?PK=" + machinePK;
}

///<summary>Used to fill and View Machinery Details for editing</summary>
///<summary>Grid Handler Catch all the grid events in this function </summary>
/// <param name="machinePK"  type="Object">   
/// </param>
function ViewMachineDetails(machinePK) {
    window.location = MachineMaster.MachineDtlsEntryPage + "?PK=" + machinePK + "&Status=0";
}


//<summary>function used to Delete Maintance  details </summary>
function DeleteMachineDtls() {
    var msgtxt;
    $.get(MachineMaster.DELETEMACHINEDTLSURL + machinePK, function (data) {
        // Check Machine Details Deleted Successfully or Not
        if (parseInt(data) == 1) {
            msgtxt = MachineMaster.DeleteMachineDtlsSuccessMsg;
        }

        //        else if (parseInt(data) == 0) {
        //            msgtxt = MachineMaster.CANNOTDELETE;
        //        }

        //----------------------------------------------

        else if (parseInt(data) == 0)
            msgtxt = MachineMaster.Used;
        else if (parseInt(data) == -5)
            msgtxt = MachineMaster.DeletedRecord;
        else if (parseInt(data) == -3)
            msgtxt = MachineMaster.EditUsedByAnotherUser;

        //-----------------------------------------------
        else {

            msgtxt = MachineMaster.ActionFailedMsg;
        }

        GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, "DeleteSuccess");
    });
    return false;
}


///<summary>Function invoke after Model popup ok Click</summary>
/// <param name="command" optional="true" type="String">
/// Click OK which which methode perform based on this command
/// </param>
function ModalOk(command) {
    switch (command) {
        case MachineMaster.MachineDelete:
            DeleteMachineDtls();

            break;
        case "DeleteSuccess":
            PageInit();

            break;

    }
    return false;
}


//<summary>function Used to Reset Page</summary>
function ResetPage() {
    //    //Reseting all input controls in the page
    //    $(document.forms[0]).find("input").each(function () {
    //        var idval = $(this).attr("id");
    //        if (idval.search("UserPk") == -1) {
    //             $(this).val(MachineMaster.TextEmpty);
    //        }
    //    });
    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    //PageInit();
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    $("select[id$=SearchType]").val(MachineMaster.SearchMachineName);
    $("[id$=SearchValue]").val(MachineMaster.TextEmpty);
    SearchSettings()
    return false;
}

//<summary> AutoComplete - For Search Machine Detials </summary>
function SearchInit() {
    GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", MachineMaster.MACHINEDTLSAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
    //     GrandScriptUtils.MakeAutoCompleteSearch("SearchValue", CompoundMaster.COMPOUNDDTLSAUTOCOMPLETEURL + $("[id$=BizUnitPk]").val(), "SearchType");
}

//<summary>Function to SearchType Change Function And Search Details </summary>
function SearchSettings() {
    $("[id$=SearchType]").change(function () {
        SetSearchType();
    });
    $("[id$=imbSearch]").click(function () {
        BindMachineGrid();
        return false;
    });
    BindMachineGrid();
    $("[id$=SearchValue]").hide()
    $("[id$=imbSearch]").hide();
}

///<summary>Set Search Type - Show Hide SearchValue textBox And Button </summary>
function SetSearchType() {
    var strname = $("select[id$=SearchType]").val();
    $("[id$=SearchValue]").val(MachineMaster.TextEmpty);
    if (strname == MachineMaster.ValueZero) {
        $("[id$=SearchValue]").hide()
        $("[id$=imbSearch]").hide();
        BindMachineGrid();
    }
    else {
        $("[id$=SearchValue]").show()
        $("[id$=imbSearch]").show();
        $("input[id$=SearchValue]").focus();
        BindMachineGrid();
    }
}

///<summary>filling gridview after entering search value in search textbox</summary>
function AfterSelect() {

    BindMachineGrid();
}

///#endregion

// After Grid bind

function AfterGridBind(gridID) {
    var colIndex = 0;
    var plant = "";

    if (gridID == "grdMachineDetails") {
        $("#grdMachineDetails tr:has(td)").each(function (index) {
            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MCH_PLANT_TEXT", gridID);
            plant = GrandGrid.Utilities.GetColumnValue($(this), "MCH_PLANT_TEXT", gridID);
            if (colIndex != 0) {
                $(this).find("td:eq(" + colIndex + ")").html("");
                if (plant == "null" || plant == "") {
                    plant = "";
                }
                $(this).find("td:eq(" + colIndex + ")").append(plant);
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MCT_NAME", gridID);
            if (colIndex != null) {
                var mctNmae = GrandGrid.Utilities.GetColumnValue($(this), "MCT_NAME", gridID);
                if (mctNmae == "null")
                    $(this).find("td:eq(" + colIndex + ")").html("");
            }

            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "LOC_NAME", gridID);
            if (colIndex != null) {
                var locNmae = GrandGrid.Utilities.GetColumnValue($(this), "LOC_NAME", gridID);
                if (locNmae == "null")
                    $(this).find("td:eq(" + colIndex + ")").html("");
            }

        });
    }

//    if (gridID == "grdMachineDetails") {
//        $("#grdMachineDetails tr:has(td)").each(function (index) {
//            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "MCT_NAME", gridID);
//            MachType = GrandGrid.Utilities.GetColumnValue($(this), "MCT_NAME", gridID);
//            if (colIndex != 0) {
//                $(this).find("td:eq(" + colIndex + ")").html("");
//                if (MachType == "null" || plant == "") {
//                    MachType = "";
//                }
//                $(this).find("td:eq(" + colIndex + ")").append(MachType);
//            }
//        });
//    }

//    if (gridID == "grdMachineDetails") {
//        $("#grdMachineDetails tr:has(td)").each(function (index) {
//            colIndex = GrandGrid.Utilities.GetColumnIndex($(this), "LOC_NAME", gridID);
//            wrkCenter = GrandGrid.Utilities.GetColumnValue($(this), "LOC_NAME", gridID);
//            if (colIndex != 0) {
//                $(this).find("td:eq(" + colIndex + ")").html("");
//                if (wrkCenter == "null" || plant == "") {
//                    wrkCenter = "";
//                }
//                $(this).find("td:eq(" + colIndex + ")").append(wrkCenter);
//            }
//        });
//    }

}

//






