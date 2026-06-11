
$(document).ready(function () {
    ///<summary>
    ///Document Ready function Aftre page initialization
    ///</summary>
    //    $(document.forms[0]).validate({
    //        onclick: false,
    //        onkeyup: false,
    //        focusInvalid: false
    //    });
    //Page Initial condtions
    PageInit();
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);



});


function EndRequestHandler(sender, args) {

    DateTimeInit();

}

function PageInit() {
    ///<summary>
    ///Used For PageInit
    ///</summary>
    DateTimeInit();
    return false;
}


function DateTimeInit() {
    //<summary>Function used to Init Date and Time Extender to the control</summary>
    GrandScriptUtils.DatePicker("StockDate", false, false);
//    if ($("[id$=hdfDateStock]").val() != "") {
//        $("input[id$=StockDate]").val($("[id$=hdfDateStock]").val());
//    }
}