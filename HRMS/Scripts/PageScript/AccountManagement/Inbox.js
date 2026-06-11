/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../GrandGridMulti.js" />

// first set the task tab then call set the total count
$(document).ready(function () {

    PageInt();
});

function PageInt() {
    //<summary>Function used to initiallize Page</summary>

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    DateInit();
    DropDownReadable();
}


function EndRequestHandler(sender, args) {
    // Validate the input text controls to prevent XSS

    DropDownReadable();
}

function DropDownReadable() {

    if ($.browser.msie) {
        $("select").each(function () {
            var drp = $(this);
            $(drp).attr("id");
            $(drp).find("option").each(function () {
                $(this).attr("title", $(this).text());
            });
        });
    }
}

function DateInit() {
    GrandScriptUtils.AddDateRangeCommon("PeriodFrom", "hdfPrdFrm", "PeriodTo", "hdfPrdTo", false, false, true, false);
//    var format = "dd-M-yy";
//    $('input[id$=PeriodFrom]').datepicker({
//        dateFormat: format,
//        changeMonth: true,
//        changeYear: true,
//        altField: $("[id$=hdfPrdFrm]"),
//        altFormat: "mm/dd/yy"
//    });
//    $('input[id$=PeriodTo]').datepicker({
//        dateFormat: format,
//        changeMonth: true,
//        changeYear: true,
//        altField: $("[id$=hdfPrdTo]"),
//        altFormat: "mm/dd/yy"
//    });
}

