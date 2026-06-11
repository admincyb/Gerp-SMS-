/// <reference path="../../jquery/jquery-1.5.min.js" />
/// <reference path="../../jquery/json2.js" />
/// <reference path="../../GrandScriptUtils.js" />
///#region ---------- Global Variables
//Initilize all variable(IDs) as Default as 0
var machineJson = new Object();
var locationID = 0;
var machineTypeID = 0;

var machineID = 0;
var MaintenanceID = 0;

var UPLOADURL = "Upload\\";
var UPLOADFOLDER = "Machine";
///#endregion

///#region ---------- Configuration
var MachineMaster = {
    RootName: "Root",
    // Machine Type
    MACHINETYPEURL: "MachineryManagement.do?Action=GetMachineType&SBU=",
    SAVEMACHINETYPEURL: "MachineryManagement.do?Action=SaveMachineType&SBU=",
    DELETEMACHINETYPEURL: "MachineryManagement.do?Action=DeleteMachineType&MachineTypeID=",
    MACHINETYPEGRIDURL: "MachineryManagement.do?Action=GetMachineTypeDetails&SBU=",
    // DropDown Fill
    RUNBYDTLSURL: "MachineryManagement.do?Action=GetRunByDtls&SBU=",
    MAINTENANCETYPEURL: "MachineryManagement.do?Action=GetMainteanceType&SBU=",
    FREQUENCYTYPEURL: "MachineryManagement.do?Action=GetFreequencyType&SBU=",
    CURRENCYTYPEURL: "CommonManagement.do?Action=GetCurrencyList&SBU=",
    GETPLANTURL: "CommonManagement.do?Action=GetPlant&SBU=",
    CONSUMPTIONUOMURL: "UOMManagement.do?Action=GetUnit&UOMTypeID=0&UOMPK=0",
    VENDORURL: "VendorRegistration.do?Action=GetVendors&SBUPk=",
    MeasuringTypeUrl: "MachineryManagement.do?Action=GetMeasuringType&SBU=",

    // TreeView Fill
    ProcessMappingTreeURL: "CommonManagement.do?Action=GetAppConfigTree&machinePK=",

    // Location
    LOCATIONURL: "MachineryManagement.do?Action=GetLocation&SBUPk=",
    SAVELOCATIONURL: "MachineryManagement.do?Action=SaveLocation",
    DELETELOCATIONURL: "MachineryManagement.do?Action=DeleteLocation&locID=",
    LOCATIONGRIDURL: "MachineryManagement.do?Action=GetLocationList&SBUPk=",
    CANNOTDELETE: "Translate(CannotDelete)",


    // Fields
    MachineTypeID: "MCT_PK",
    MachineName: "MCT_NAME",
    //MachineActive: "MCH_ACTIVE",
    MachineCodeAlreadyExistsMsg: "Translate(MachineCodeAlreadyExists)",

    SELECTCURRENCY: "Translate(SelectCurrency)",

    // GRID
    FILLMACHINEDETAILSURL: "MachineryManagement.do?Action=GetMachineDtls&machinePK=",
    SAVEMACHINEDTLSURL: "MachineryManagement.do?Action=SavePage",
    SelectOneMsg: "Translate(Pleaseselectanoption)",
    ValidTimeMsg: "Translate(PleaseenteravalidDateTime)",
    DoUWantToDelMsg: "Translate(Doyouwanttodeletethisdetails)",
    DefaultActionMsg: "Translate(DefaultActionneedstobeperformed)",
    ActionFailedMsg: "Translate(ActionFailedPleaseTryAgain)",
    SaveMachineSuccessMsg: "Translate(MachineDetailsSavedSuccessfully)",
    EnterMaintanaceDetailsMsg: "Translate(PleaseEnterMaintanaceDetails)",
    ProvideCode: "Translate(ProvideMachineCode)",
    ProvideNameMsg: "Translate(ProvideMachineName)",
    SelectTypeMsg: "Translate(PleaseselectaMachineType)",
    SelectLocMsg: "Translate(PleaseSelectLocationWorkCenter)",
    SelectPurchaseFrmMsg: "Translate(PleaseSelectPurchaseFrom)",
    SelectConditionMsg: "Translate(PleaseSelectCondition)",
    PurchasePriceMsg: "Translate(PleaseProvidePurchasePrice)",
    SelectUomType: "Translate(PleaseSelectUOMType)",
    ExpiryDateMsg: "Translate(PleaseProvideExpiryDate)",
    PurchaseDtateMsg: "Translate(PleaseProvidePurchaseDate)",
    SelectPurchaseModeMsg: "Translate(PleaseSelectPurchaseMode)",
    ThroughPutMsg: "Translate(PleaseProvideThroughput)",
    AvgConsumptionMsg: "Translate(PleaseProvideAvgConsumption)",
    UOMTYpeMsg: "Translate(PleaseSelectUOMType)",
    RunByMsg: "Translate(PleaseSelectRunBy)",
    MaxContinousUsgMsg: "Translate(PleaseProvideMaxContinousUsage)",
    UsageUOMMsg: "Translate(PleaseSelectUOMType)",

    ToTimeMsg: "Translate(PleaseProvideToTime)",
    FromTimeMsg: "Translate(PleaseProvideFromTime)",
    ToDateMsg: "Translate(PleaseProvideToDate)",
    FromDateMsg: "Translate(PleaseProvideFromDate)",
    FrequencyTypeMsg: "Translate(PleaseSelectFreequnecyType)",
    MaintenanceTypeMsg: "Translate(SelectMaintenanceType)",
    MachineTypeSaveMsg: "Translate(MachineTypeSavedSuccessfullly)",
    MachineTypeExists: "Translate(MachineTypeAlreadyExists)",

    // Error Msges
    MachineTypeAssignedMsg: "Translate(MachineTypeAlreadyAssigned)",
    MachineTypeDeleteMsg: "Translate(MachineTypeDetailsDeletedSuccessfully)",

    LocAlreadyExistsMsg: "Translate(LocWorkCenterNameAlreadyExists)",
    LocSaveSuccessmsg: "Translate(LocationSavedSuccessfullly)",
    LocDelSuccessMsg: "Translate(LocWorkCenterDeletedSuccessfully)",
    LocAssignedmsg: "Translate(LocationAlreadyAssigned)",
    // Location
    EnterLocation: "Translate(EnterLocWorkCenter)",
    // MachineType
    EnterMachineType: "Translate(EnterMachineType)",


    //Maintenance
    InvalidTimeMsg: "Translate(EnterValidTime)",
    MaintenaceDurationAlreadyAdded: "Translate(MaintenaceDurationAlreadyAdded)",

    // Title
    ConfirmationMsg: "Translate(Conformation)",
    InformationTitle: "Translate(Information)",
    StatusTitle: "Translate(Status)",
    MachinePK: "Mctp",
    LocationPk: "LOC_PK",
    LocationName: "LOC_NAME",
    VendorPK: "DndPK",
    VendorName: "DndNem",
    ContactName: "DndNemC",
    Address: "DndSrda1",
    Phone: "DndNph",
    Email: "DndELim",

    //Commands

    DeleteMachineTypeSucessesCMD: "deleteSucessMachine",
    DeleteMachineTypeCMD: "deletemsgMachineType",
    DeleteLocationCMD: "deleteLocation",
    DeleteMsgLocCMD: "deletemsgLocation",
    DeleteVendorCMD: "deleteVendor",
    DeleteMsgVendor: "deletemsgVendor",
    DeleteMsgMachine: "deletemsgMachine",
    DeleteMachineDtls: "deleteMachineDtls",
    SaveCMD: "save",
    DeleteMaintenanceCMD: "deletemsgMaintenance",
    // Grid handler Commands
    // Machine Grid
    MachineEdit: "edit",
    MachineDelete: "delete",
    // Maintenance Grid
    MaintenanceDelete: "deleteMaintenance",
    MaintenanceEdit: "editMaintenance",
    // Vendor Grid
    VendorEdit: "edit",
    VendorDelete: "delete",
    // Location Grid
    LocationEdit: "edit",
    LocationDelete: "delete",
    // MachineType Grid
    MachineTypeEdit: "editmachinetype",
    MachineTypeDelete: "deletemachinetype",

    ValueZero: "0",
    CreateMachineType: "Translate(CreateMachineType)",
    CreateLocation: "Translate(CreateLocWorkCenter)"

}
///#endregion

///#region ----------Initilization
$(document).ready(function () {
    // validate the Form
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    //For Adding rule to Select
    $.validator.addMethod("selectNone", function (value, element) {
        return ($(element).val() != MachineMaster.ValueZero);
    }, MachineMaster.SelectOneMsg);
    //For Adding rule to  Time Format - 24 Hours
    $.validator.addMethod("dateTime", function (value) {
        return /^([01]?[0-9]|2[0-3]):[0-5][0-9]?$/.test(value);
    }, MachineMaster.ValidTimeMsg);
    // Bind Maintenance Grid - With no values
    GrandGrid.Utilities.ResetGrid(true, "grdMaintenanceDetails");
    machineJson.MaintenanceList = new Array();
    GrandGrid.MakeGrid($("#grdMaintenanceDetails"), 0, machineJson.MaintenanceList);
    //Set Page Control - Show Hide Entry And Listing Section
    PageInit();



});

function PageInit() {
    //initialize Machine Object
    $("[id$=SBU]").val($("[id$=BizUnitPk]").val());
    machineJson = $.parseJSON($("[id$=MaintenanceList]").val());

    $("#divDatas").data("MachineData", machineJson);
    GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST", "Machine", true);
    // Create Tabs
    $("[id$=tabs]").tabs();
    //$("[id$=tabs]").tabs("select", 0);
    // Show / Hide FromDate And To Date Depends on the Maintenace type
    //ShowMaintenanceDuration();
    // Clear Maintenance Details
    ShowMaintenaceDuration();
    ClearMaintenanceDuration();
    $("#divMachineType").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveMachineTypeValidation();
        }
    });
    $("#LocationDiv").dialog({
        autoOpen: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        beforeClose: function (event, ui) {
            RemoveLocationValidation();
        }
    });
    FillMachineDetails();
    FillProcessTree()
}
///#endregion

function FillProcessTree() {
    var machinePk = $("input[id$=MCH_PK]").val();
    SetTreeHeaderStructure("trvProcessMap", MachineMaster.ProcessMappingTreeURL + machinePk, MachineMaster.RootName, true, false, "", "&pVal=0");    // set the tree view parameters
    MakeMultiTree();
}

function GetSelectedProcess() {
    //<summary>Function Used to get the all checked dept details </summary>
    var ProcessArray = new Array();
    var processPk = 0;
    $("#trvProcessMap").find("input[type=checkbox]:checked").each(function () {
        processPk = $(this).attr("id");
        /*if ($(this).next().next("input[type=hidden]").val() == "true") {
        processPk = processPk.substr(processPk.lastIndexOf("_") + 1, processPk.length);
        ProcessArray.push({ MPM_PROCESS: processPk });
        }*/
        processPk = processPk.substr(processPk.lastIndexOf("_") + 1, processPk.length);
        ProcessArray.push({ MPM_PROCESS: processPk });
    });
    return ProcessArray;
}

///#region ---------- Core Section
//<summary>function used to Save Machine details</summary>
function SavePage() {
    //Add Validation for Order Details by setting mode as 1
    RemoveAllValidations();
    AddValidations(1);
    var ObjFile = $("#divFileData").data("FileData");
    $("[id$=FILELIST]").val(JSON.stringify(ObjFile.FILELIST));
    var machineJson = $("#divDatas").data("MachineData");
    CheckPerformanceValidations();
    if ($("select[id$=MCH_VENDOR]").val() != 0) {
        AddValidations(6);
        $("[id$=tabs]").tabs("select", 0);
    }
    // Check Have The Order List have More than or equal to one Product Details
    //if (machineJson.MaintenanceList.length > 0) {
    if ($(document.forms[0]).valid()) {
        //Assginging the Machine details to a hidden field by converting the object to string using Json Stringify Methord
        //            $("select[id$=SBU]").attr("disabled", false);
        $("[id$=MachineDetails]").val(JSON.stringify(GetSelectedProcess()));
        $("input[id$=MCH_CODE]").attr("disabled", false);
        if ($("input[id$=MachineActive]").is(':checked') == true)
            $("[id$=MCH_ACTIVE]").val('1');
        else
            $("[id$=MCH_ACTIVE]").val('0');
        $("[id$=MaintenanceList]").val(JSON.stringify(machineJson.MaintenanceList));       
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(MachineMaster.SAVEMACHINEDTLSURL, jSonString, function (data) {
            // Check Machine Details Saved Successfully or not
            if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(MachineMaster.SaveMachineSuccessMsg, MachineMaster.InformationTitle, MachineMaster.SaveCMD);
                //RemoveAllValidations();
            }
            else {
                var msgtxt;
                if (parseInt(data) == 0)
                    msgtxt = MachineMaster.MachineCodeAlreadyExistsMsg;
                else if (parseInt(data) < 0)
                    msgtxt = MachineMaster.ActionFailedMsg;
                GrandScriptUtils.ShowModal(msgtxt, MachineMaster.StatusTitle, "failed");
            }

        });

    }

    //}
    //else {
    //    GrandScriptUtils.ShowModal(MachineMaster.EnterMaintanaceDetailsMsg, MachineMaster.InformationTitle);
    //}
    return false;
}

//<summary>function used to Reset Page details</summary>
function ResetPage() {
    RemoveAllValidations();
    window.location = "MachineryListing.aspx";
    return false;
}

///#region ---------- Save Machine Details

//<summary>function To Fill All Combobox  Details </summary>
function FillCombo() {
    FillVendor(0);
    FillMeasuringType(0);
    FillMachineType(0);
    FillRunByDtls(0);
    FillMainteanceType(0);
    FillCurrencyType(0);
    FillFreequencyType(0);
    FillConsumptionUOM(0);
    FillLocation(0);
    FillPlant(0);
}


function FillFileDetails() {
    ///<summary>To Fill File Details And dispaly as Listing With Delete Option</summary>
    if (FileJson.FILELIST.length > 0) {
        for (var index in FileJson.FILELIST) {
            var template = $("#_FileUploadTemplate").clone();
            $(template).find("span:eq(1)").text(FileJson.FILELIST[index].DOC_TITLE + "." + FileJson.FILELIST[index].DOC_TYPE); //FileName
            $(template).find("span:eq(0)").text(UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME);
            $(template).find("a:eq(0)").attr("href", "../../DwnloadFile.aspx?fPath=" + UPLOADURL + UPLOADFOLDER + "\\" + FileJson.FILELIST[index].DOC_NAME + "&Title=" + FileJson.FILELIST[index].DOC_TITLE);
            $("#fContainer_" + "fupUploader").append($(template).html());
        }
    }
}


//<summary>function To Fill Maachine Type Details </summary>
/// <param name="machineTypeID"  type="object"> 
/// </param>
function FillMachineType(machineTypeID) {
    // Get id of the MachineType DropDown
    var drpID = $("select[id$=MCH_TYPE]").attr("id");
    //Fill MachineType Details to the Machine Type DropDown, Name as Text, PK as Value
    $.get(MachineMaster.MACHINETYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, machineTypeID);
    });
}

//<summary>function To Fill Location Details </summary>
/// <param name="locationID"  type="object">  
/// </param>
function FillLocation(locationID) {
    // Get id of the Location DropDown
    var drpID = $("select[id$=MCH_LOCATION]").attr("id");
    //Fill Location Details to the Location DropDown, Name as Text, PK as Value
    $.get(MachineMaster.LOCATIONURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, locationID);

    });
}
function FillPlant(PlantID) {
    // Get id of the Plant DropDown
    var drpID = $("select[id$=MCH_PLANT]").attr("id");
    //Fill Plant Details to the Plant DropDown, Name as Text, PK as Value
    //alert(MachineMaster.GETPLANTURL + $("[id$=BizUnitPk]").val());
    $.get(MachineMaster.GETPLANTURL + $("[id$=BizUnitPk]").val(), function (data) {
    
        GrandScriptUtils.FillDropDown(drpID, data, true, true, PlantID);

    });
}

function FillVendor(vendorID) {
    ///<summary>function used to fill vendor to vendor drop down </summary>

    var drpID = $("select[id$=MCH_VENDOR]").attr("id");
    $.get(MachineMaster.VENDORURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, vendorID);

    });
}

function FillMeasuringType(typeId) {
    ///<summary>function used to fill Measuring Type drop down </summary>
    var drpID = $("select[id$=MCH_MEASURE]").attr("id");
    $.get(MachineMaster.MeasuringTypeUrl + $("[id$=BizUnitPk]").val() + "&Active=1&CfgType=MACHINE MEASUREMENT TYPE", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, typeId);
    });
}

//<summary>function To Fill RunBy Details </summary>
/// <param name="runByID"  type="Int">
/// Click OK which which methode perform based on this command
/// </param>
function FillRunByDtls(runByID) {
    var drpID = $("select[id$=MCH_FUEL_TYPE]").attr("id");
    $.get(MachineMaster.RUNBYDTLSURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, runByID);
    });
}

//<summary>function To Maintence Type Details </summary>
/// <param name="mainteanceID"  type="Object">
/// </param>
function FillMainteanceType(mainteanceID) {
    var drpID = $("select[id$=MCM_TYPE]").attr("id");
    $.get(MachineMaster.MAINTENANCETYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, mainteanceID);
    });
}

//<summary>function To Currency Type Details </summary>
/// <param name="currencyTypeID"  type="Object">
/// </param>
function FillCurrencyType(currencyTypeID) {
    var drpID = $("select[id$=MCH_PUR_CURR]").attr("id");
    $.get(MachineMaster.CURRENCYTYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, currencyTypeID);
    });
}



//<summary>function To Fill MCM_FREQUENCY  Details </summary>
/// <param name="freequncyType"  type="Object">
/// </param>
function FillFreequencyType(freequncyType) {
    var drpID = $("select[id$=MCM_FREQUENCY]").attr("id");
    $.get(MachineMaster.FREQUENCYTYPEURL + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, freequncyType);
    });
}
//<summary>function To Fill Consumption UOM  Details </summary>
/// <param name="uomType"  type="Object">
/// </param>
function FillConsumptionUOM(uomType) {
    var drpID = $("select[id$=MCH_AVGC_UOM]").attr("id");
    $.get(MachineMaster.CONSUMPTIONUOMURL + "&SBU=" + $("[id$=BizUnitPk]").val(), function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true, uomType);

    });
}
//<summary>function To Show / Hide Control Depends  Maintance </summary>
function ShowMaintenaceDuration() {
    var strname = $("select[id$=MCM_FREQUENCY]").val();
    // Check Maintance Duration Type Except  Day
    if (strname == "1" || strname == "4") {
        $("#maintenceFrom").show();
        $("#maintenceTo").show();
        $("#maintenceDay").hide();
        //GrandScriptUtils.AddDateRange("MCM_FROM_DT", "hdnMaintenanceFromDate", "MCM_TO_DT", "hdnMaintenanceToDate",false, false);

    }
    else if (strname == "3") {
        $("#maintenceFrom").show();
        $("#maintenceTo").show();
        $("#maintenceDay").hide();
        RemoveValidations(4);
        // GrandScriptUtils.AddDateRangeInSameMonth("MCM_FROM_DT", "hdnMaintenanceFromDate", "MCM_TO_DT", "hdnMaintenanceToDate", false, false);
    }

    // Check Maintance Duration Type ==Day
    else if (strname == "2") {
        $("#maintenceFrom").hide();
        $("#maintenceTo").hide();
        $("#maintenceDay").show();
        $("[id$=FromTime]").timepicker();
        $("[id$=ToTime]").timepicker();
    }
    // Not Selecting Any Maintance Duration Type
    else {
        $("#maintenceFrom").show();
        $("#maintenceTo").show();
        $("#maintenceDay").hide();

    }

    if (strname != "2") {
        GrandScriptUtils.AddDateRange("MCM_FROM_DT", "hdnMaintenanceFromDate", "MCM_TO_DT", "hdnMaintenanceToDate", false, false);
    }
}




///#endregion

///#region ---------- Edit Machine Details , Fill Details To Control

///<summary>Used to fill Machinery Details for editing</summary>
function FillMachineDetails() {
    var machinePk = $("input[id$=MCH_PK]").val();
    if (machinePk != "0") {
        $.get(MachineMaster.FILLMACHINEDETAILSURL + machinePk, function (data) {
            $("#divDatas").data("MachineData", data);
            FillMachineryDetails();
        });
    }
    else {
        FillCombo();
        DateInit();
    }
}


///<summary>Used to fill Machinery Details for editing</summary>
function FillMachineryDetails() {
    GrandScriptUtils.AddDateRange("MCH_PUR_DT", "hdnDateofPurchase", "MCH_EXPR_DT", "hdnExpiryDate", false, false, true);
    machineJson = $("#divDatas").data("MachineData");
    //    $("select[id$=SBU]").val(machineJson.SBU);
    FillVendor(machineJson.MCH_VENDOR);
    FillMeasuringType(machineJson.MCH_MEASURE);
    FillMachineType(machineJson.MCH_TYPE);
    FillRunByDtls(machineJson.MCH_FUEL_TYPE);
    FillCurrencyType(machineJson.MCH_PUR_CURR);
    FillConsumptionUOM(machineJson.MCH_AVGC_UOM);
    FillLocation(machineJson.MCH_LOCATION);
    FillPlant(machineJson.MCH_PLANT);
    FillMainteanceType(0);
    FillFreequencyType(0);
    $("input[id$=MCH_PK]").val(machineJson.MCH_PK);
    $("input[id$=MCH_CODE]").val(machineJson.MCH_CODE);
    $("input[id$=MCH_NAME]").val(machineJson.MCH_NAME);
    //$("select[id$=MCH_TYPE]").val(machineJson.MCH_TYPE);
    //$("select[id$=MCH_LOCATION]").val(machineJson.MCH_LOCATION);
    //$("select[id$=MCH_VENDOR]").val(machineJson.MCH_VENDOR);
    $("select[id$=MCH_CONDITION]").val(machineJson.MCH_CONDITION);
    $("input[id$=MCH_PUR_PRICE]").val(machineJson.MCH_PUR_PRICE);
    //$("select[id$=MCH_PUR_CURR]").val(machineJson.MCH_PUR_CURR);
    $("input[id$=hdnDateofPurchase]").val(GrandScriptUtils.ConvertDateFormat(machineJson.MCH_PUR_DT));
    $("input[id$=hdnExpiryDate]").val(GrandScriptUtils.ConvertDateFormat(machineJson.MCH_EXPR_DT));
    $("input[id$=MCH_PUR_DT]").val(machineJson.MCH_PUR_DT);
    $("input[id$=MCH_EXPR_DT]").val(machineJson.MCH_EXPR_DT);
    $("textarea[id$=MCH_PUR_REMARKS]").val(machineJson.MCH_PUR_REMARKS);
    $("input[id$=MCH_THROUGHPUT]").val(machineJson.MCH_THROUGHPUT == 0 ? "" : machineJson.MCH_THROUGHPUT);
    $("input[id$=MCH_AVG_CONS]").val(machineJson.MCH_AVG_CONS == 0 ? "" : machineJson.MCH_AVG_CONS);
    $("input[id$=MCH_MC_USAGE]").val(machineJson.MCH_MC_USAGE == 0 ? "" : machineJson.MCH_MC_USAGE);
    if (machineJson.MCH_ACTIVE == "1")
        $("input[id$=MachineActive]").attr("checked", true);
    else
        $("input[id$=MachineActive]").attr("checked", false);

    //$("select[id$=MCH_AVGC_UOM]").val(machineJson.MCH_AVGC_UOM);
    //$("select[id$=MCH_FUEL_TYPE]").val(machineJson.MCH_FUEL_TYPE);
    $("select[id$=MCH_MCU_UOM]").val(machineJson.MCH_MCU_UOM);
    if (!($.isArray(machineJson.MaintenanceList))) {
        var objArray;
        if (machineJson.MaintenanceList != undefined) {
            objArray = machineJson.MaintenanceList;
            machineJson.MaintenanceList = new Array();
            machineJson.MaintenanceList.push(objArray);
            GrandGrid.MakeGrid($("#grdMaintenanceDetails"), 0, machineJson.MaintenanceList);
        }
        else {
            objArray = machineJson.MaintenanceList;
            machineJson.MaintenanceList = new Array();
        }
    }
    else {
        //machineJson = $("#divDatas").data("MachineData");
        GrandGrid.MakeGrid($("#grdMaintenanceDetails"), 0, machineJson.MaintenanceList);
    }
    if (!($.isArray(machineJson.FILELIST))) {
        var objArray;
        if (machineJson.FILELIST != undefined) {
            objArray = machineJson.FILELIST;
            FileJson.FILELIST = new Array();
            FileJson.FILELIST.push(objArray);
        }
        else {
            objArray = machineJson.FILELIST;
            FileJson.FILELIST = new Array();
        }

    }
    else {
        FileJson.FILELIST = machineJson.FILELIST;
    }
    FillFileDetails();
    //    $("select[id$=SBU]").attr("disabled", true);
  //  $("input[id$=MCH_CODE]").attr("disabled", true);
}

///#endregion

///#region ----------  machine Type Section
//<summary>function To Save Machine Type Details </summary>
function SaveMachineType() {
    RemoveAllValidations();
    AddMachineTypeValidations();
    if ($(document.forms[0]).valid()) {
        var msgTxt;
        var jSonString = GrandScriptUtils.FormToJsonString("divMachineType");
        $.post(MachineMaster.SAVEMACHINETYPEURL + $("[id$=BizUnitPk]").val(), jSonString, function (data) {
            // Check Machine Type Saved Successfully or Not - >0 Success ,0- Name Already Exists, <0 - Fail(Exception)
            if (parseInt(data) > 0)
                msgTxt = MachineMaster.MachineTypeSaveMsg;
            // Machine Code Already Exists Or not
            else if (parseInt(data) == 0)
                msgTxt = MachineMaster.MachineTypeExists;
            else if (parseInt(data) < 0)
                msgTxt = MachineMaster.ActionFailedMsg;
            // Check The Machine Type  Saved Succesfully or Not
            if (parseInt(data) > 0) {
                FillMachineType(data);
                ClearMachineTypeDetails();
                $("[id$=divMachineType]").dialog("close");
                $("select[id$=MCH_TYPE]").focus();
                return false;
            }
            else {

                GrandScriptUtils.ShowModal(msgTxt, MachineMaster.InformationTitle, "SaveMachineType");
            }

        });
    }
    return false;

}

//<summary>function To Show Machine Type PopUp - For Entry  </summary>
function AddMachineTypeDetails() {
    ClearMachineTypeDetails();
    $(document.forms[0]).validate().resetForm();
    BindMachineTypeGrid();
    $("#divMachineType").dialog("open");
    $("#divMachineType").dialog({ width: 500, height: 350, resizable: true, title: MachineMaster.CreateMachineType });
    $("#divMachineType").css({ "min-height": "300", "margin-top": "25px" });
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
        case MachineMaster.MachineTypeDelete:
            machineTypeID = GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachineTypeID, $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MachineMaster.DoUWantToDelMsg, MachineMaster.ConfirmationMsg, MachineMaster.DeleteMachineTypeCMD, true);
            break;
        // To Edit Details                                       
        case MachineMaster.MachineTypeEdit:
            FillMachineTypeDetails(tr);
            break;
        default:
            alert(MachineMaster.DefaultActionMsg);
            break;
    }
    return false;
}

///<summary> Function to Fill  Machine Type Details When Edit Details </summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
function FillMachineTypeDetails(tr) {
    $("input[id$=MachineTypePK]").val(GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachineTypeID, $(tr).parent().parent().attr("id")));
    $("input[id$=MachineTypeName]").val(GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.MachineName, $(tr).parent().parent().attr("id")));
}

///<summary>Clear Machine Type Details </summary>
function ClearMachineTypeDetails() {
    $("input[id$=MachineTypePK]").val(MachineMaster.ValueZero);
    $("input[id$=MachineTypeName]").val("");
    //$("[id$=lblstarmachine]").hide();
}

///<summary>Delete Vendor Details </summary>
function DeleteMachineTypeDetails() {
    var msgtxt;
    $.get(MachineMaster.DELETEMACHINETYPEURL + machineTypeID, function (data) {
        // Check Delete Success
        if (parseInt(data) == 1) {
            msgtxt = MachineMaster.MachineTypeDeleteMsg;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteMachineTypeSucessesCMD);
        }
        // MachineType Already Assigned or not
        else if (parseInt(data) == 0) {
            msgtxt = MachineMaster.CANNOTDELETE;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteMachineTypeSucessesCMD);
        }
        else {
            msgtxt = MachineMaster.ActionFailedMsg;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteMachineTypeSucessesCMD);
        }
    });
    return false;
}
///#endregion

///#region ----------  Work Location Section
//<summary>function To Save Location Details </summary>
function SaveLocation() {
    RemoveAllValidations();
    AddLocationValidations();
    if ($(document.forms[0]).valid()) {
        var msgTxt;
        // TO Pass Location Name to handler
        // var jSonString = GrandScriptUtils.FormToJsonString("LocationDiv");
        var jSonString = GrandScriptUtils.FormToJsonString(false);
        $.post(MachineMaster.SAVELOCATIONURL, jSonString, function (data) {
            // Check Location Name Saved Successfully or Not - >0 Success ,0- Name Already Exists, <0 - Fail(Exception)
            if (parseInt(data) > 0)
                msgTxt = MachineMaster.LocSaveSuccessmsg;
            else if (parseInt(data) == 0)
                msgTxt = MachineMaster.LocAlreadyExistsMsg;
            else if (parseInt(data) < 0)
                msgTxt = MachineMaster.ActionFailedMsg;
            // Check The Location Name Saved Succesfully or Not
            if (parseInt(data) > 0) {
                // Remove validation for Location Details PopUp
                FillLocation(data);
                ClearLocationDetails();
                $("[id$=LocationDiv]").dialog("close");
                $("select[id$=MCH_LOCATION]").focus();
                return false;
            }
            else {

                GrandScriptUtils.ShowModal(msgTxt, MachineMaster.InformationTitle, "");
            }

        });
    }
    return false;

}
//<summary>function To Show Location PopUp  </summary>
function AddLocationDetails() {
    ClearLocationDetails();
    $(document.forms[0]).validate().resetForm();
    $("#LocationDiv").dialog("open");
    $("#LocationDiv").dialog({ width: 500, height: 350, resizable: true, title: MachineMaster.CreateLocation });
    $("#LocationDiv").css({ "min-height": "300", "margin-top": "25px" });
    BindLocationGrid();
    return false;
}
///<summary>Grid Handler - Location Type Catch all the grid events in this function </summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function LocationGridHandler(tr, command) {
    switch (command.toString().toLowerCase()) {
        // To Delete Details                               
        case MachineMaster.LocationDelete:
            locationID = GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.LocationPk, $(tr).parent().parent().attr("id"));
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MachineMaster.DoUWantToDelMsg, MachineMaster.ConfirmationMsg, MachineMaster.DeleteMsgLocCMD, true);
            break;
        // To Edit Details                                        
        case MachineMaster.LocationEdit:
            FillLocationDetails(tr);
            break;
        default:
            alert(MachineMaster.DefaultActionMsg);
            break;
    }
    return false;

}
// Fill Location Type Details
/// <param name="tr"  type="Object">
/// Specific Container and its controls
/// </param>
function FillLocationDetails(tr) {

    $("input[id$=LocationPK]").val(GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.LocationPk, $(tr).parent().parent().attr("id")));
    $("input[id$=LocationName]").val(GrandGrid.Utilities.GetColumnValue(tr, MachineMaster.LocationName, $(tr).parent().parent().attr("id")));

}
///<summary>Clear Location Details </summary>
function ClearLocationDetails() {
    $("input[id$=LocationPK]").val(MachineMaster.ValueZero);
    $("input[id$=LocationName]").val("");
    // $("[id$=lblstarloc]").hide();
}
///<summary>Delete Vendor Details </summary>
function DeleteLocationDetails() {
    var msgtxt;
    $.get(MachineMaster.DELETELOCATIONURL + locationID, function (data) {
        if (parseInt(data) == 1) {
            msgtxt = MachineMaster.LocDelSuccessMsg; // msgtxt = MachineMaster.LocDelSuccessMsg;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteLocationCMD);
        }
        else if (parseInt(data) == 0) {
            msgtxt = MachineMaster.CANNOTDELETE;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteLocationCMD);
        }
        else {
            msgtxt = MachineMaster.ActionFailedMsg;
            GrandScriptUtils.ShowModal(msgtxt, MachineMaster.InformationTitle, MachineMaster.DeleteLocationCMD);
        }

    });
    return false;


}
///#endregion

///#region ---------- Machine Maintenance Section




///#region ---------- Machine Maintenance Details Section - Add , Delete , Edit, Fill Details Operations
//<summary>function used to add Maintenance  details to Machine</summary>
function AddMaintenanceDtls() {
    RemoveAllValidations();
    AddValidations(2);
    // Check Selected Frequency
    if ($("select[id$=MCM_FREQUENCY]").val() == "2") {
        RemoveValidations(3);
        AddValidations(4);
    }
    else {
        RemoveValidations(4);
        AddValidations(3);
    }
    if ($(document.forms[0]).valid()) {
        machineJson = $("#divDatas").data("MachineData");
        // Get MaintenanceDtls PK
        var editMaintenance = $("input[id$=EditMaintenance]").val();
        var obj = new Object();
        var flag = true;
        // Check MaintenancePk ==0 , 0 Means New Entry, >0 Means For Update
        if (parseInt(editMaintenance) == 0) {
            if ($("select[id$=MCM_FREQUENCY]").val() != "2")
            // Check the Date is valid or Not
                flag = ValidDate(machineJson, 0);
            else
            // Check time is valid or Not - 
                flag = ValidDateTime(machineJson, 0);
        }
        else {
            if ($("select[id$=MCM_FREQUENCY]").val() != "2")
            // Check Date is Valid Or Not
                flag = ValidDate(machineJson, editMaintenance);
            else
                flag = ValidDateTime(machineJson, editMaintenance);
            // Get MaintenanceDetails Corresponding the MaintenancePK - using Loop
            for (var i in machineJson.MaintenanceList) {
                if (parseInt(editMaintenance) == machineJson.MaintenanceList[i].SL_NO)
                // Add Details To Object
                    obj = machineJson.MaintenanceList[i];
            }
        }
        // Check If Date / Time valid or Not
        if (flag) {
            var status = true;
            // Check Entry Details is Day Type
            if (parseInt($("select[id$=MCM_FREQUENCY]").val()) == "2") {
                // Check Time Is valid Or not - Not Between already Entered Details
                status = CheckTime();
            }
            //            else {
            //                //RemoveValidations(4);
            //                //AddValidations(3);
            //            }
            //            //check Time or Date is Valid or not 
            if (status == true) {
                // Add One By one Details To Object

                obj.MCM_TYPE = parseInt($("select[id$=MCM_TYPE]").val());
                obj.MNT_NAME = $("select[id$=MCM_TYPE] option:selected").text();
                obj.MCM_FREQUENCY = parseInt($("select[id$=MCM_FREQUENCY]").val());
                obj.FRQ_NAME = $("select[id$=MCM_FREQUENCY] option:selected").text();
                obj.MCM_FROM_DT = parseInt($("select[id$=MCM_FREQUENCY]").val()) == 2 ? $("input[id$=FromTime]").val() : $("input[id$=MCM_FROM_DT]").val();
                obj.MCM_TO_DT = parseInt($("select[id$=MCM_FREQUENCY]").val()) == 2 ? $("input[id$=ToTime]").val() : $("input[id$=MCM_TO_DT]").val();
                // Check Add Details - For New Entry
                if (parseInt(editMaintenance) == 0) {
                    // If Yes - get Length of the List and Assign Length+1 as the PK of New Entry
                    obj.SL_NO = machineJson.MaintenanceList.length + 1;
                    obj.MCM_PK = 0;
                    //Push Object to List
                    machineJson.MaintenanceList.push(obj);
                }
                // Add Details To DivDatas
                $("#divDatas").data("MachineData", machineJson);
                // Bind Maintenance Details Grid
                GrandGrid.MakeGrid($("#grdMaintenanceDetails"), 0, machineJson.MaintenanceList);

                ClearMaintenanceInfo();
                //AddValidations(3);
            }
            else {
                GrandScriptUtils.ShowModal(MachineMaster.InvalidTimeMsg, MachineMaster.InformationTitle);
            }
        }
        else {
            GrandScriptUtils.ShowModal(MachineMaster.MaintenaceDurationAlreadyAdded, MachineMaster.InformationTitle);
        }
        return false;
    }
}
//<summary>Function to Clear MaintenanceInfo Details </summary>
function ClearMaintenanceInfo() {
    ClearMaintenanceDuration();

}
///<summary>Grid Handler Catch all the grid events in this function  - For MaintenanceInfo Details</summary>
/// <param name="tr"  type="Object">
///     Specific Container and its controls
/// </param>
/// <param name="command"  type="Object">
///     Specific Edit/Delete
/// </param>
function MainteanceGridHandler(tr, command) {
    // Remove Validations
    RemoveValidations(2);
    RemoveValidations(3);
    RemoveValidations(4);
    switch (command.toString().toLowerCase()) {
        case "delete":
            MaintenanceID = GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", "grdMaintenanceDetails");
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal(MachineMaster.DoUWantToDelMsg, MachineMaster.ConfirmationMsg, MachineMaster.DeleteMaintenanceCMD, true);
            return false;
            break;
        case "edit":
            FillMaintenanceDetails(tr);
            return false;
            break;
        default:
            alert(MachineMaster.DefaultActionMsg);
            return false;
            break;
    }
}
///<summary>For delete the item in the grid - Maintenance Details List</summary>
function DeleteMaintenanceDetails() {
    var machineJson = $("#divDatas").data("MachineData");
    // Delete Maintenance Details - By MaintenanceInfoID Using Loop
    for (var i in machineJson.MaintenanceList) {
        // Check MaintenanceList[i].MCM_PK Equal to MaintenanceID
        if (machineJson.MaintenanceList[i].SL_NO == MaintenanceID) {
            // Splice Details From List, Corresponding MaintenancePK
            machineJson.MaintenanceList.splice(i, 1);
            break;
        }
    }
    // Reassign MaintenanceInfoID From 1 to Last Record , Starting ID=1
    for (var i in machineJson.MaintenanceList) {

        machineJson.MaintenanceList[i].SL_NO = parseInt(i + 1);
    }
    $("#divDatas").data("MachineData", machineJson);
    GrandGrid.MakeGrid($("#grdMaintenanceDetails"), 0, machineJson.MaintenanceList);
    ClearMaintenanceInfo();

}
///<summary>Used fill Details of Maintenance Details for editing</summary>
/// <param name="tr"  type="Object">
/// Specific Container and its controls
/// </param>
function FillMaintenanceDetails(tr) {
    ClearMaintenanceInfo();
    $("select[id$=MCM_TYPE]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_TYPE", $(tr).parent().parent().attr("id")));
    $("select[id$=MCM_FREQUENCY]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_FREQUENCY", $(tr).parent().parent().attr("id")));
    ShowMaintenaceDuration();
    $("input[id$=EditMaintenance]").val(GrandGrid.Utilities.GetColumnValue(tr, "SL_NO", $(tr).parent().parent().attr("id")));
    //$("input[id$=MCM_PK]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_PK", $(tr).parent().parent().attr("id")));
    if (GrandGrid.Utilities.GetColumnValue(tr, "MCM_FREQUENCY", $(tr).parent().parent().attr("id")) == "2") {
        $("input[id$=FromTime]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_FROM_DT", $(tr).parent().parent().attr("id")));
        $("input[id$=ToTime]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_TO_DT", $(tr).parent().parent().attr("id")));
        $("input[id$=MCM_FROM_DT]").val("");
        $("input[id$=MCM_TO_DT]").val("");
    }
    else {

        $("input[id$=MCM_FROM_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_FROM_DT", $(tr).parent().parent().attr("id")));
        $("input[id$=MCM_TO_DT]").val(GrandGrid.Utilities.GetColumnValue(tr, "MCM_TO_DT", $(tr).parent().parent().attr("id")));
        $("input[id$=FromTime]").val("");
        $("input[id$=ToTime]").val("");
    }

}

//<summary>function To Clear Maintenace Duration  Details </summary>
function ClearMaintenanceDuration() {
    $("#maintenceFrom").show();
    $("#maintenceTo").show();
    $("#maintenceDay").hide();
    $("select[id$=MCM_FREQUENCY]").val(MachineMaster.ValueZero);
    $("select[id$=MCM_TYPE]").val(MachineMaster.ValueZero);
    $("input[id$=EditMaintenance]").val(MachineMaster.ValueZero);
    $("input[id$=MCM_TO_DT]").val("");
    $("input[id$=MCM_FROM_DT]").val("");
    $("input[id$=FromTime]").val("");
    $("input[id$=ToTime]").val("");
    $("input[id$=hdnMaintenanceFromDate]").val("");
    $("input[id$=hdnMaintenanceToDate]").val("");
    GrandScriptUtils.AddDateRange("MCM_FROM_DT", "hdnMaintenanceFromDate", "MCM_TO_DT", "hdnMaintenanceToDate", false, false);
}
///#endregion

///#endregion

///#region ---------- Fill Grids 
//<summary>function To Bind LocationType Details </summary>
function BindLocationGrid() {
    var ajaxUrl = MachineMaster.LOCATIONGRIDURL + $("[id$=BizUnitPk]").val();
    $("#grdLocationDtls").removeAttr("ajaxurl")
    $("#grdLocationDtls").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdLocationDtls");
    GrandGrid.MakeGrid($("#grdLocationDtls"));
}
//<summary>function To Bind MachineType Details </summary>
function BindMachineTypeGrid() {
    var ajaxUrl = MachineMaster.MACHINETYPEGRIDURL + $("[id$=BizUnitPk]").val();
    $("#grdMachineTypeDtls").removeAttr("ajaxurl")
    $("#grdMachineTypeDtls").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdMachineTypeDtls");
    GrandGrid.MakeGrid($("#grdMachineTypeDtls"));
}
///#endregion

///#region ---------- ModalPopUp OK Click 
///<summary>Function invoke after Model popup ok Click</summary>
/// <param name="command" optional="true" type="String">
/// Click OK which which methode perform based on this command
/// </param>
function ModalOk(command) {
    switch (command) {
        case MachineMaster.DeleteMachineTypeCMD:
            DeleteMachineTypeDetails();
            ClearMachineTypeDetails();
            break;
        case MachineMaster.DeleteMachineTypeSucessesCMD:
            BindMachineTypeGrid();
            ClearMachineTypeDetails();
            BindMachineTypeGrid();
            FillMachineType(0);
            break;
        case MachineMaster.DeleteLocationCMD:
            BindLocationGrid();
            ClearLocationDetails();
            FillLocation(0);
            break;
        case MachineMaster.DeleteMsgLocCMD:
            DeleteLocationDetails();
            ClearLocationDetails();
            break;
        case MachineMaster.SaveCMD:
            ResetPage();
            break;
        case MachineMaster.DeleteMaintenanceCMD:
            DeleteMaintenanceDetails();
            break;
    }
    return false;
}
///#endregion


function CheckPerformanceValidations() {
    if ($("input[id$=MCH_THROUGHPUT]").val().length > 0 || $("input[id$=MCH_AVG_CONS]").val().length > 0 || $("input[id$=MCH_MC_USAGE]").val().length > 0) {
        AddValidations(5);
        $("[id$=tabs]").tabs("select", 2);
        return false;
    }
    else {
        RemoveValidations(5);
    }

}

///#endregion

///#region ---------- Validation
//<summary>Function to Add validation</summary>
/// <param name="mode"  type="Object">
/// </param>
function AddValidations(mode) {
    //Machinery Header Details
    if (mode == "1") {
        $("input[id$=MCH_CODE]").rules("add", {
            required: true,
            maxlength: 95,
            messages: { required: MachineMaster.ProvideCode }
        });
        $("input[id$=MCH_NAME]").rules("add", {
            required: true,
            maxlength: 190,
            messages: { required: MachineMaster.ProvideNameMsg }
        });
        $("select[id$=MCH_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.SelectTypeMsg }
        });
        $("select[id$=MCH_LOCATION]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.SelectLocMsg }
        });
        //Purchase Info
        //Hide For  Checking if  Purchased drop down have value
        /* $("select[id$=MCH_VENDOR]").rules("add", {
        date: true,
        selectNone: true,
        messages: { selectNone: MachineMaster.SelectPurchaseFrmMsg }
        });
        $("select[id$=MCH_CONDITION]").rules("add", {
        selectNone: true,
        messages: { selectNone: MachineMaster.SelectConditionMsg }
        });
        $("input[id$=MCH_PUR_PRICE]").rules("add", {
        required: true,
        maxlength: 12,
        ThreeDecimal: true,
        messages: { required: MachineMaster.PurchasePriceMsg }
        });
        $("select[id$=MCH_PUR_CURR]").rules("add", {
        selectNone: true,
        messages: { selectNone: MachineMaster.SELECTCURRENCY }
        });
        $("input[id$=MCH_EXPR_DT]").rules("add", {
        date: true,
        required: true,
        messages: { required: MachineMaster.ExpiryDateMsg }
        });
        $("input[id$=MCH_PUR_DT]").rules("add", {
        required: true,
        date: true,
        messages: { required: MachineMaster.PurchaseDtateMsg }
        });*/
        // Perfomance Info
        //        $("input[id$=MCH_THROUGHPUT]").rules("add", {
        //            required: true,
        //            maxlength: 12,
        //            ThreeDecimal: true,
        //            messages: { required: MachineMaster.ThroughPutMsg }
        //        });
        //        $("input[id$=MCH_AVG_CONS]").rules("add", {
        //            required: true,
        //            maxlength: 12,
        //            ThreeDecimal: true,
        //            messages: { required: MachineMaster.AvgConsumptionMsg }
        //        });
        //        $("select[id$=MCH_AVGC_UOM]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: MachineMaster.UOMTYpeMsg }
        //        });
        //        $("select[id$=MCH_FUEL_TYPE]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: MachineMaster.RunByMsg }
        //        });
        //        $("input[id$=MCH_MC_USAGE]").rules("add", {
        //            required: true,
        //            maxlength: 11,
        //            TwoDecimal: true,
        //            messages: { required: MachineMaster.MaxContinousUsgMsg }
        //        });
        //        $("select[id$=MCH_MCU_UOM]").rules("add", {
        //            selectNone: true,
        //            messages: { selectNone: MachineMaster.UsageUOMMsg }
        //        });
    }
    // Maintenanace Info
    else if (mode == "2") {
        $("select[id$=MCM_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.MaintenanceTypeMsg }
        });
        $("select[id$=MCM_FREQUENCY]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.FrequencyTypeMsg }
        });
    }
    // Maintenanace Info - Except Day type
    else if (mode == "3") {
        $("input[id$=MCM_FROM_DT]").rules("add", {
            required: true,
            date: true,
            messages: { required: MachineMaster.FromDateMsg }
        });
        $("input[id$=MCM_TO_DT]").rules("add", {
            required: true,
            date: true,
            messages: { required: MachineMaster.ToDateMsg }
        });
    }
    //Maintenanace Info With Day Type
    else if (mode == "4") {
        $("input[id$=FromTime]").rules("add", {
            dateTime: true,
            required: true,
            messages: { required: MachineMaster.FromTimeMsg }
        });
        $("input[id$=ToTime]").rules("add", {
            dateTime: true,
            required: true,
            messages: { required: MachineMaster.ToTimeMsg }
        });
    }
    // Performance Details
    else if (mode == "5") {
        $("input[id$=MCH_THROUGHPUT]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal: true,
            messages: { required: MachineMaster.ThroughPutMsg }
        });
        $("input[id$=MCH_AVG_CONS]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal: true,
            messages: { required: MachineMaster.AvgConsumptionMsg }
        });
        $("select[id$=MCH_AVGC_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.UOMTYpeMsg }
        });
        $("select[id$=MCH_FUEL_TYPE]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.RunByMsg }
        });
        $("input[id$=MCH_MC_USAGE]").rules("add", {
            required: true,
            maxlength: 11,
            TwoDecimal: true,
            messages: { required: MachineMaster.MaxContinousUsgMsg }
        });
        $("select[id$=MCH_MCU_UOM]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.UsageUOMMsg }
        });
    } //Purchase Info
    else if (mode == "6") {
        $("select[id$=MCH_VENDOR]").rules("add", {
            date: true,
            selectNone: true,
            messages: { selectNone: MachineMaster.SelectPurchaseFrmMsg }
        });
        $("select[id$=MCH_CONDITION]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.SelectConditionMsg }
        });
        $("input[id$=MCH_PUR_PRICE]").rules("add", {
            required: true,
            maxlength: 12,
            ThreeDecimal: true,
            messages: { required: MachineMaster.PurchasePriceMsg }
        });
        $("select[id$=MCH_PUR_CURR]").rules("add", {
            selectNone: true,
            messages: { selectNone: MachineMaster.SELECTCURRENCY }
        });
        $("input[id$=MCH_EXPR_DT]").rules("add", {
            date: true,
            required: true,
            messages: { required: MachineMaster.ExpiryDateMsg }
        });
        $("input[id$=MCH_PUR_DT]").rules("add", {
            required: true,
            date: true,
            messages: { required: MachineMaster.PurchaseDtateMsg }
        });
    }

}
//<summary>Function to Remove validation</summary>
/// <param name="mode"  type="Object">
/// </param>
function RemoveValidations(mode) {
    // Machinery Details
    if (mode == "1") {
        $("input[id$=MCH_CODE]").rules("remove");
        $("input[id$=MCH_NAME]").rules("remove");
        $("select[id$=MCH_TYPE]").rules("remove");
        $("select[id$=MCH_LOCATION]").rules("remove");
        // Purchase
        $("select[id$=MCH_VENDOR]").rules("remove");
        $("select[id$=MCH_CONDITION]").rules("remove");
        $("input[id$=MCH_PUR_PRICE]").rules("remove");
        $("select[id$=MCH_PUR_CURR]").rules("remove");
        $("input[id$=MCH_EXPR_DT]").rules("remove");
        $("input[id$=MCH_PUR_DT]").rules("remove");
        // Performance
        //        $("select[id$=MCH_MCU_UOM]").rules("remove");
        //        $("input[id$=MCH_MC_USAGE]").rules("remove");
        //        $("select[id$=MCH_FUEL_TYPE]").rules("remove");
        //        $("select[id$=MCH_AVGC_UOM]").rules("remove");
        //        $("input[id$=MCH_AVG_CONS]").rules("remove");
        //        $("input[id$=MCH_THROUGHPUT]").rules("remove");
    }
    // Maintenace Except Day
    if (mode == "2") {
        $("select[id$=MCM_FREQUENCY]").rules("remove");
        $("select[id$=MCM_TYPE]").rules("remove");
    }
    // Maintenace Date
    if (mode == "3") {
        $("input[id$=MCM_TO_DT]").rules("remove");
        $("input[id$=MCM_FROM_DT]").rules("remove");
    }
    // Maintenace Time
    if (mode == "4") {
        $("input[id$=FromTime]").rules("remove");
        $("input[id$=ToTime]").rules("remove");
    }

    if (mode == "5") {
        $("select[id$=MCH_MCU_UOM]").rules("remove");
        $("input[id$=MCH_MC_USAGE]").rules("remove");
        $("select[id$=MCH_FUEL_TYPE]").rules("remove");
        $("select[id$=MCH_AVGC_UOM]").rules("remove");
        $("input[id$=MCH_AVG_CONS]").rules("remove");
        $("input[id$=MCH_THROUGHPUT]").rules("remove");
    }
    if (mode == "6") {
        $("select[id$=MCH_VENDOR]").rules("remove");
        $("select[id$=MCH_CONDITION]").rules("remove");
        $("input[id$=MCH_PUR_PRICE]").rules("remove");
        $("select[id$=MCH_PUR_CURR]").rules("remove");
        $("input[id$=MCH_EXPR_DT]").rules("remove");
        $("input[id$=MCH_PUR_DT]").rules("remove");
    }
}
//<summary>function To Validate Machine PopUp Controls </summary>
function AddMachineTypeValidations() {
    $("input[id$=MachineTypeName]").rules("add", {
        required: true,
        maxlength: 190,
        messages: { required: MachineMaster.EnterMachineType }
    });
}
//<summary>function To Remove Validate Machine PopUp Controls </summary>
function RemoveMachineTypeValidation() {
    $("input[id$=MachineTypeName]").rules("remove");
}
//<summary>function To Add Validate Location PopUp Controls </summary>
function AddLocationValidations() {
    $("input[id$=LocationName]").rules("add", {
        required: true,
        maxlength: 190,
        messages: { required: MachineMaster.EnterLocation }
    });
}
//<summary>function To Remove Validate Location PopUp Controls </summary>
function RemoveLocationValidation() {
    $("input[id$=LocationName]").rules("remove");
}
//<summary>function To Remove All Validation Controls </summary>
function RemoveAllValidations() {
    RemoveValidations(1);
    RemoveValidations(2);
    RemoveValidations(3);
    RemoveValidations(4);
    RemoveValidations(5);
    RemoveValidations(6);
}

///#endregion

///#region ---------- utility Functions
///#region ---------- Check Time valid Or Not
///<summary>Function to Check Valid Time Or Not</summary>
/// True- FromDate Less Than To Date, 
function CheckTime() {

    // Check To Time Greater Than From Time
    var start = $("input[id$=FromTime]").val();
    var end = $("input[id$=ToTime]").val();
    // Add From Time HH:MM to Array
    startArr = start.split(':');
    // Add To Time HH:MM to Array
    endArr = end.split(':');
    // Calaculate Minutes Differenc
    min = endArr[1] - startArr[1];
    hour_carry = 0;
    // Check Min <60
    if (min < 0) {
        // Yes-  Add 60 Minutes With Minutes  
        min += 60;
        // Add 1 Hour With Hour_Carry
        hour_carry += 1;
    }
    // Calculate Hour = FromTime (Hour) - ToTimeHour(Hour) - HourCarry
    hour = endArr[0] - startArr[0] - hour_carry;
    // Check Hour > 0 For Check Valid or not
    if (hour > 0)
    // Retur True- Time is valid 
        return true;
    else {
        // Check Hour ==0, then check Min > 0
        if (hour == 0) {

            if (min > 0)
            // if Min > 0 it a Valid Time
                return true;
            else
                return false;
        }
        // Check Hour < 0 For Check Time Valid or not
        else if (hour < 0) {
            // Return False - Invalid Time
            return false;
        }

    }
}
///#endregion

///#region ---------- Convert Date Fromat
// Convert 10-Feb-20111 Format to 02/10/2011 
function ConvertDateFormat(date) {

    // Split Date Format 10-Feb-2011 By '-' and Assign to Array
    var conDate = date.split('-');
    // Cretae Month name Array
    var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    // Assign Month Name's Index+1 as Name of Month - and Assign to conDate[1] Using loop
    for (var i = 0; i < mmm.length; i++) {
        // check Month Equal to Monthsa in a Array
        if (mmm[i] == conDate[1]) {
            // If True - Get Index of the Array +1 and Assign to conDate[1]
            conDate[1] = i + 1;
            break;
        }
    }
    // return as  10-Feb-20111 Format to 02/10/2011
    return conDate[1] + "/" + conDate[0] + "/" + conDate[2];
}
///#endregion

///#region ---------- Check Date is Valid or Not, FromDate And ToDate  Already  Enter Or Not
//<summary>Function to Check New Entry( From Date and ToDate )Already Exists </summary>
function ValidDate(machineJson, mainPK) {
    var fromDate = new Date(ConvertDateFormat($("input[id$=MCM_FROM_DT]").val()));
    var toDate = new Date(ConvertDateFormat($("input[id$=MCM_TO_DT]").val()));
    for (var i in machineJson.MaintenanceList) {
        if ((machineJson.MaintenanceList[i].MCM_FREQUENCY != "3") && (mainPK != machineJson.MaintenanceList[i].SL_NO)) {
            if (machineJson.MaintenanceList[i].MCM_TYPE == $("select[id$=MCM_TYPE]").val() && machineJson.MaintenanceList[i].MCM_FREQUENCY == $("select[id$=MCM_FREQUENCY]").val()) {
                var grdFromDate = new Date(ConvertDateFormat(machineJson.MaintenanceList[i].MCM_FROM_DT));
                var grdToDate = new Date(ConvertDateFormat(machineJson.MaintenanceList[i].MCM_TO_DT));
                if (CheckDateTimeExists(grdFromDate, grdToDate, fromDate, toDate)) {

                }
                else {
                    return false;
                }
            }
        }
    }
    return true;
}
///#endregion

///#region ---------- Check Time is Valid or Not, FromTime And ToTime  Already  Enter Or Not
//<summary>Function to Check New Entry (From Time and ToTime) Already Exists </summary>
function ValidDateTime(machineJson, mainPK) {
    var fromTime = parseFloat($("input[id$=FromTime]").val().replace(':', '.'));
    var toTime = parseFloat($("input[id$=ToTime]").val().replace(':', '.'));
    var grdFrmTime;
    var grdToTime;
    for (var i in machineJson.MaintenanceList) {
        if ((machineJson.MaintenanceList[i].MCM_FREQUENCY == $("select[id$=MCM_FREQUENCY]").val()) && (mainPK != machineJson.MaintenanceList[i].SL_NO)) {
            if (machineJson.MaintenanceList[i].MCM_TYPE == $("select[id$=MCM_TYPE]").val()) {
                grdFrmTime = parseFloat(machineJson.MaintenanceList[i].MCM_FROM_DT.replace(':', '.'));
                grdToTime = parseFloat(machineJson.MaintenanceList[i].MCM_TO_DT.replace(':', '.'));
                if (CheckDateTimeExists(grdFrmTime, grdToTime, fromTime, toTime)) {

                }
                else {
                    return false;
                }
            }
        }
    }
    return true;
}
///#endregion

//<summary>function To Check Date Or Time Laready Exists </summary>
function CheckDateTimeExists(checkstart, checkend, fromDate, toDate) {
    if (fromDate <= checkend && toDate >= checkstart)
        return false;
    else
        return true;
}

///<summary>Set Date Format And Set Today Date as Default</summary>
function DateInit() {
    GrandScriptUtils.AddDateRange("MCH_PUR_DT", "hdnDateofPurchase", "MCH_EXPR_DT", "hdnExpiryDate", false, false);

}

///#endregion



