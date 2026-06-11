function InitComponents() {
//    var pageURL = window.document.URL;
//    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
//    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
//   // GrandScriptUtils.MakeAutoComplete("txtEmployee", url, "hdfEmployee", true, true, "NONUSEREMPLOYEE");
//    if ($("[id$=txtEmployee]").attr("disabled") == "disabled")
//        DisableAuto($("[id$=txtEmployee]"));
}
function DisableAuto(extender) {
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
    $(extender).autocomplete("option", "disabled", true);
    $(extender).attr("disabled", true);
}
function EnableAuto(extender) {
    $(extender).removeAttr("disabled");
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
    $(extender).autocomplete("option", "disabled", false);
}
//calls parent page alert message 
function AlertMessage(msg, title, command) {
    //<summary>method Show Alert Message</summary>
    Util.ShowModalDialog(msg, title, command);
}
function ShowListing(flag) {
    ///<summary>
    /// Used to handle the Listing And Enrty Section in Page
    ///</summary>
    /// <param name="flag" optional="true" type="String">
    /// flag Determines the Mode if flag then in Listing else in Edit Mode
    /// </param>           
    if (flag) {
        $("[id$=PageAction_List]").show();
        $("[id$=PageAction_Entry]").hide();
        $("[id$=pnlListing]").show();
        $("[id$=pnlEntry]").hide();

    }
    else {
        $("[id$=PageAction_List]").hide();
        $("[id$=PageAction_Entry]").show();
        $("[id$=pnlListing]").hide();
        $("[id$=pnlEntry]").show();
    }
    return false;
}

function ViewMode(mode) {
    ///<summary>
    /// Used to handle the view Mode
    ///</summary>
    /// <param name="mode" optional="true" type="String">
    /// Mode = 1 Determins ites on View Mode
    /// Mode = 2 Indicates its on New Mode
    /// </param> 
    //Mode = 1 Indicates its on View Mode
    //Mode = 2 Indicates its on New Mode
    if (mode == 1) {
        $("[id$=pnlSave]").hide();
        $("[id$=pnlDelete]").hide();
    }
    else if (mode == 2) {
        $("[id$=pnlDelete]").hide();
    }
}
function ValidateNow() {
    if (typeof (Page_ClientValidate) == 'function') {
        Page_ClientValidate();
    }
    if (!Page_IsValid) {
        $("[id$=litErrorMsg]").hide();
        ShowErrorMessage($("#diverror").html());
        return false;  //Page is invalid -- stop right here
    }
    else {
        //everythings ok --- Call your function & do your stuff
        return true;
    }
}
function InitADUserAuto() {
    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    GrandScriptUtils.MakeAutoComplete("txtADUsers", url, "hdfADUsers", true, true, "ADUSERS");
    if ($("[id$=txtADUsers]").attr("disabled") == "disabled")
        DisableAuto($("[id$=txtADUsers]"));
}
function AfterAutoCompleteSelect(targetControlID) {
    if (targetControlID == "txtADUsers") {
        $("[id$=btnADUSerChange]").click();
    }
}
//redirct to command argument
function ModalOk(command) {
    //<summary>method do action on Modal Ok Button Event</summary>
    if (command != null) {
        location.href = command;
    }
}

var Util = {
    ShowModalDialog: function (content, title, command, showCancel) {
        //<summary>method to show alert message</summary>
        if (showCancel) {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                        if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                            ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                        }

                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }

                }
            });
        }
        else {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                beforeClose: function () {
                    if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                        ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                    }
                },
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }

                }
            });
        }

    }
}