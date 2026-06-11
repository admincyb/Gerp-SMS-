
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

var VendorId = 0;
///#region----Initialization Section

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    // Create Tabs
    $("[id$=tabs]").tabs();

        $.validator.addMethod('selectNone', function (value, element) {
            return ($(element).val() != "0");
        }, 'Translate(Please select an option)');

        //Page Initial condtions
        PageInit();

        $("[id$=SearchType]").change(function () {
            SetSearchType();
        });

        $("[id$=imbSearch]").click(function () {
            BindGrid();
            return false;
        });
 
});
///<summary>Used for initial settings</summary>
function PageInit() {

    $('[id$=btnSave]').hide();
    $('[id$=imbAdd]').show();
    $('[id$=divData]').hide();
    $('[id$=divListing]').show();

    BindGrid();
    SearchInit();
    SetSearchType();
    FillCurrency();
    return false;
}

///#endregion



///#region----Set Or Reset Form

///<summary>Used to fill UOM Type</summary>
function FillCurrency() {
    // Get id of the UOM Type DropDown
    var drpID = $("select[id$=Currency]").attr("id");
    $.get("VendorManagement.do?Action=GetCurrency", function (data) {
        GrandScriptUtils.FillDropDown(drpID, data, true, true);
    });
}
///<summary>Function To Show Data Entry Form </summary>
function AddNew() {
    //RemoveValidations();
    $('[id$=btnSave]').show();
    $('[id$=imbAdd]').hide();
    $('[id$=divData]').show();
    $('[id$=divListing]').hide();
    $('[id$=divReports]').hide();
//    $("input[id$=VendorId]").val('0');
//    $("input[id$=StateId]").val('0');
//    $("input[id$=CountryId]").val('0');
    
    return false;
}
///<summary>Used to reset UOM Details</summary>
function ResetPage() {

    //Reseting all input controls in the page
    $(document.forms[0]).find("input").each(function () {
        var idval = $(this).attr("id");
        //alert(idval);
        //Avoid UserPk to get the value of log in user
        if (idval.search("UserPk") == -1)
            $(this).val("");
    });
    $("input[id$=VendorId]").val('0');
    $("input[id$=StateId]").val('0');
    $("input[id$=CountryId]").val('0');
    $("input[id$=btnUpload]").val('Upload All');

    //Selecting the first value in all drop downs
    $(document.forms[0]).find("select").each(function () {
        $(this).val($(this).find("option:eq(0)").val());
    });
    

    PageInit();
    return false;
}
///#endregion

///#region----Bind Grid

///<summary>To handle bind grid corr. to the search type and search value</summary>
function BindGrid() {
    var ajaxUrl = "VendorManagement.do?Action=GetVendorsList&Status=" + $("[id$=SearchType]").val() + "&SearchValue=" + $("[id$=SearchValue]").val();
    $("#grdVendorDetails").removeAttr("ajaxurl")
    $("#grdVendorDetails").attr("ajaxurl", ajaxUrl);
    GrandGrid.Utilities.ResetGrid(true, "grdVendorDetails");
    GrandGrid.MakeGrid($("#grdVendorDetails"));
}

///<summary>filling gridview after entering search value in search textbox</summary>
function AfterSelect() {
    BindGrid();
}

///#endregion

///#region----Add/ Remove Validation 

//<summary>function used to assign validation</summary>
function AddValidations() {

    $('select[id$=VendorType]').rules("add", {
        selectNone: true,
        messages: { selectNone: 'Translate(Please select vendor type)' }
    });

    $('input[id$=VendorName]').rules("add", {
        required: true,
        maxlength: 15,
        messages: { required: 'Translate(Please Provide vendor name)' }
    });
    $('input[id$=ContactName]').rules("add", {
        required: true,
        maxlength: 50,
        messages: { required: 'Translate(Please Provide contact name)' }
    });

    $('input[id$=Phone]').rules("add", {
        digits: true,
        maxlength: 15,
        messages: { digits: 'Translate(Please enter digits)' }
    });
    $('input[id$=Mobile]').rules("add", {
        digits: true,
        maxlength: 15,
        messages: { digits: 'Translate(Please enter digits)' }
    });
    $('input[id$=Fax]').rules("add", {
        digits: true,
        maxlength: 15,
        messages: { digits: 'Translate(Please enter digits)' }
    });
    $('input[id$=Email]').rules("add", {
        email: true,
        maxlength: 50,
        messages: { digits: 'Translate(Please enter a valid Email)' }
    });
    
}
//<summary>function Remove Validation</summary>
function RemoveValidations() {

    $('select[id$=VendorType]').rules("remove");
    $('input[id$=VendorName]').rules("remove");
    $('input[id$=Name]').rules("remove");
    $('input[id$=Phone]').rules("remove");
    $('input[id$=Mobile]').rules("remove");
    $('input[id$=Email]').rules("remove");
    //$('input[id$=Phone]').rules("remove");
}

///#endregion


///#region----Grid Handlers And Model Popup Ok Click

///<summary>Grid Handler Catches all grid events of UOM </summary>
function GridHandler(tr, command) {
    //RemoveValidations();

    switch (command.toString().toLowerCase()) {

        case "delete":
            //$('input[id$=VendorId]').val(GrandGrid.Utilities.GetColumnValue(tr, "Meup", $(tr).parent().attr("id")));
            VendorId = GrandGrid.Utilities.GetColumnValue(tr, "Dnvppadi", $(tr).parent().attr("id"));
            
            // Do Confirmation.. Before Delete Details
            GrandScriptUtils.ShowModal('Translate(Do you want to delete this details)', 'Translate(Confirmation)', "delete", true);
            return false;
            break;

        case "edit":
            FillDetails(tr);
            return false;
            break;

        default:
            alert('Translate(Default Action needs to be performed)');
            return false;
            break;

    }
    return false;
}
//<summary>Function invoke after Model popup ok Click</summary>
function ModalOk(command) {

    switch (command) {

        case "delete":
            DeleteDetails();
            break;

    }
    return false;
}

///#endregion

///#region----Data Management Section

//<summary>Function To Delete UOM Details</summary>
function DeleteDetails() {
   
    $.get("VendorManagement.do?Action=DelVendor&VendorId=" + VendorId, function (data) {
        if (parseInt(data) > 0) {
            $('input[id$=VendorId]').val('0');
            GrandScriptUtils.ShowModal('Translate(Vendor deleted successfully)', 'Translate(Information)');
        }
        else
            GrandScriptUtils.ShowModal('Translate(Action Failed, Please Try Again!!!)', 'Translate(Information)');
        BindGrid();
    });
    return false;

}

//<summary>Used to fill Vendor Details for editing</summary>
function FillDetails(tr) {

    $('select[id$=VendorType]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvEpt", $(tr).parent().attr("id")));
    $('input[id$=VendorId]').val(GrandGrid.Utilities.GetColumnValue(tr, "Dnvppadi", $(tr).parent().attr("id")));
    $('input[id$=VendorName]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvNpmc", $(tr).parent().attr("id")));
    $('input[id$=ContactName]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvNem", $(tr).parent().attr("id")));
    $('input[id$=Phone]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvNph", $(tr).parent().attr("id")));
    $('input[id$=Email]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvELim", $(tr).parent().attr("id")));
    $('input[id$=Mobile]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvLbm", $(tr).parent().attr("id")));
    $('input[id$=Fax]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvXf", $(tr).parent().attr("id")));
    $('input[id$=Tin]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvNt", $(tr).parent().attr("id")));
    $('input[id$=Address1]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvSrda1", $(tr).parent().attr("id")));
    $('input[id$=Address2]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvSrda2", $(tr).parent().attr("id")));
    $('input[id$=City]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvYtc", $(tr).parent().attr("id")));
    $('input[id$=State]').val(GrandGrid.Utilities.GetColumnValue(tr, "RttTst", $(tr).parent().attr("id")));
    $('input[id$=Country]').val(GrandGrid.Utilities.GetColumnValue(tr, "TncYntc", $(tr).parent().attr("id")));
    $('select[id$=Currency]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvCurr", $(tr).parent().attr("id")));
    $('input[id$=StateId]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvTst", $(tr).parent().attr("id")));
    $('input[id$=CountryId]').val(GrandGrid.Utilities.GetColumnValue(tr, "DnvYrtnc", $(tr).parent().attr("id")));

    $('[id$=btnSave]').show();
    $('[id$=imbAdd]').hide();
    $('[id$=divData]').show();
    $('[id$=divListing]').hide();
    $('[id$=divReports]').show();
    return false;

}

//<summary>Used Save Vendor Details</summary>
function SavePage() {

    AddValidations();
    var jSonString = GrandScriptUtils.FormToJsonString(false);

    if ($(document.forms[0]).valid()) {
        $.ajax({
            type: "post",
            url: "VendorManagement.do?Action=SaveVendorDetails",
            data: jSonString,
            contentType: "application/json",
            dataType: "text",
            success: function (data) {
                if (parseInt(data) > 0) {
                    GrandScriptUtils.ShowModal('Translate(saved successfully)', 'Translate(Information)', "save");
                    ResetPage();
                }
                else if (parseInt(data) == 0)
                    GrandScriptUtils.ShowModal('Translate(Vendor name already exists)', 'Translate(Information)');
                else
                    GrandScriptUtils.ShowModal('Translate(action failed, try agian)', 'Translate(Information)');


            }
        });
    }
    return false;

}

function SetZeroDefault(sender, decimal) {
    ////<summary>function to set default zero </summary>
    /// <param name="sender"  type="Object">
    /// Determines the Textbox
    /// </param>
    /// <param name="decimal"  type="integer">
    /// Determines the no. of decimal points
    /// </param>

    if ($(sender).val() == "") {
        if (decimal)
            $(sender).val("0");
        else {
            switch (decimal) {
                case 0:
                    $(sender).val("0");
                    break;
                case 1:
                    $(sender).val("0.0");
                    break;
                case 2:
                    $(sender).val("0.00");
                    break;
                case 3:
                    $(sender).val("0.000");
                    break;
            }
        }
    }
}

///#endregion