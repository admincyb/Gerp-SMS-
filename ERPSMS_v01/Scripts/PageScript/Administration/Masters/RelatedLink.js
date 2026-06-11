
function ValidateNow() {
    //<summary>Function used to Vlidate page and show error messages</summary>
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    if (!Page_IsValid) {
        $("#litErrorMsg").hide();
        ShowErrorMessage($("#diverror").html());
        return false;  //Page is invalid -- stop right here
    }
    else {
        //everythings ok --- Call your function & do your stuff
        return true;
    }
}
function countChecked(src, args) {
    //<summary>Function used to Validate checkbox and show error messages</summary>
    if (($("[id$=ddlPage] :selected").val() != -1) && ($("[id$=ddlPage] :selected").val() != undefined)) {
        var n = $("input:checked").length;
        if (n == 0) {
            args.IsValid = false;
            return;
        }
        else {
            args.IsValid = true;
            return;
        }
    }
    else {
        args.IsValid = false;
        return;
    }
}

function ShowErrorMessage(message, title) {
    title = 'Information';
    $(".error").html("");
    $(".error").html(message);
    $(".error").dialog({
        title: title,
        modal: true,
        width: 400,
        height: 300,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        }
    });
    return false;
}