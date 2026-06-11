/// <reference path="../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../GrandScriptUtils.js" />
/// <reference path="../GrandGridMulti.js" />

var winCookie = "";
var winAllCookie = "";
//var errorTitle = "Information";
var errorTitle = "Translate(Information)";
var errorMessage = "Translate(PopupBlockerMsg)";
$(document).ready(function () {

    winCookie = GetCookie($("[id$=UserPk]").val());
    if (winCookie == null) {
        CreateCookie($("[id$=UserPk]").val(), "right-side", 365)
        WindowCollapse(true);
    }
    else {
        if (winCookie == "right-side") {
            WindowCollapse(true);
            $("[id$=imgShowDiv]").parent().hide();
        }
        else
            WindowExpand(true);
    }
    BindMenuAutoComplete();
    ValidationMessageSetting();
});


//Comma Separation for Quantity & Amount 
function numberWithCommas(x) {
    //Seperates the components of the number
    var n = x.toString().split(".");
    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return n.join(".");
    // return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function ValidationMessageSetting() {

    $("#errMsgContainer").dialog({
        autoOpen: false,
        modal: true,
        close: function (e, ui) {
            $("#errMsgContainer ul").html("");
        }
    });
}

function ShowErrorMessage(message, title, RedirectURL, PageRebind) {
    ///<summary>
    ///function used to Show Messages
    ///</summary>

    if (!title)
        title = errorTitle;
    $(".error").html("");
    $(".error").html(message);
    if (RedirectURL) {
        $(".error").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    window.location = RedirectURL;
                }
            },
            beforeClose: function (event, ui) { window.location = RedirectURL; },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            }
        });
    }
    else {
        $(".error").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    if (typeof AfterMessageClose == "function") {
                        AfterMessageClose();
                    }
                    $(".error").dialog('close');
                }
            },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            },
            close: function (event) {
                if (typeof AfterMessageClose == "function" && PageRebind == true) {
                    AfterMessageClose();
                }
            }
        });
    }
    return false;
}

function ShowMessageFixed(message, title, RedirectURL, PageRebind) {
    ///<summary>
    ///function used to Show Messages with max height of 200px
    ///</summary>

    if (!title)
        title = errorTitle;
    $(".msgbox.max-200").html("");
    $(".msgbox.max-200").html(message);
    if (RedirectURL) {
        $(".msgbox.max-200").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    window.location = RedirectURL;
                }
            },
            beforeClose: function (event, ui) { window.location = RedirectURL; },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            }
        });
    }
    else {
        $(".msgbox.max-200").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    if (typeof AfterMessageClose == "function") {
                        AfterMessageClose();
                    }
                    $(".msgbox.max-200").dialog('close');
                }
            },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            },
            close: function (event) {
                if (typeof AfterMessageClose == "function" && PageRebind == true) {
                    AfterMessageClose();
                }
            }
        });
    }
    return false;
}

function ShowErrorMessageCallBack(message, title, RedirectURL, PageRebind, afterMessageCloseCallBack) {
    ///<summary>
    ///function used to Show Messages
    ///</summary>

    if (!title)
        title = errorTitle;
    $(".error").html("");
    $(".error").html(message);
    if (RedirectURL) {
        $(".error").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    window.location = RedirectURL;
                }
            },
            beforeClose: function (event, ui) { window.location = RedirectURL; },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            }
        });
    }
    else {
        $(".error").dialog({
            resizable: false,
            title: title,
            buttons: {
                OK: function (e) {
                    $(".error").dialog('close');
                    if (afterMessageCloseCallBack != null && typeof afterMessageCloseCallBack == "function") {
                        afterMessageCloseCallBack();
                    } else {
                        if (typeof AfterMessageClose == "function") {
                            AfterMessageClose();
                        }
                    }

                }
            },
            modal: true,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
            },
            close: function (event) {
                if (afterMessageCloseCallBack != null && typeof afterMessageCloseCallBack == "function") {
                    afterMessageCloseCallBack();
                } else {
                    if (typeof AfterMessageClose == "function" && PageRebind == true) {
                        AfterMessageClose();
                    }
                }
            }
        });
    }
    return false;
}
function showPageErrors(validator, errorMap, errorList) {

    if (errorList.length > 0) {
        var errorMessages = "";
        for (var obj in errorList) {
            errorMessages += "<li>" + errorList[obj].message + "</li>";
            $(errorList[obj].element).addClass("error");
        }
        $("#errMsgContainer ul").html(errorMessages);
        $("#errMsgContainer").dialog("open");
    }
}

//For finding and removing duplicate and other group validation controls
//Array of present validations
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
function CheckKey(e) {
    ///<summary>
    ///Check Tab/BackSpace/Delete and return false for all other keys
    ///</summary>

    var keyCode = e.keyCode ? e.keyCode : e.which;
    if (keyCode == 8 || keyCode == 46 || keyCode == 110) {
        if (e.srcElement) {
            $(e.srcElement).val("");
            return false;
        }
        else if (e.currentTarget) {
            $(e.currentTarget).val("");
            return false;
        }
        else return true;
    }
    else if (keyCode == 9)
        return true;
    else
        return false;
}

function EnableArrowKey(e) {
    ///<summary>
    ///Check Tab/Home/End/ArrowKeys and return false for all other keys
    ///</summary>
    var keyCode = e.keyCode ? e.keyCode : e.which;
    if (keyCode == 9 || keyCode == 35 || keyCode == 36 || keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40)
        return true;
    else
        return false;
}

function CancelFun() {
    /// <summary> Function Used to navigate to previous page </summary>
    history.go(-1)
    return false;
}
function limitText(limitField, limitNum) {
    ///<summary>
    ///limits the multiline length - on copy too
    ///</summary>

    if (limitField.value.length > limitNum) {
        limitField.value = limitField.value.substring(0, limitNum);
    }
}
/// <summary> Window full size , set this mode for the current user when all time user login </summary>
function WindowExpand(isFirst) {
    $("[id$=imgHideDiv]").parent().hide();
    $("[id$=imgShowDiv]").parent().show();
    $("[id$=divLeftSide]").hide(300);
    $("[id$=divRightSide]").removeClass("right-side");
    $("[id$=divRightSide]").addClass("right-side-expanded");
    if (!isFirst)
        SetCookie($("[id$=UserPk]").val(), "right-side-expanded");
}
/// <summary> Window have 2 side , set this mode for the current user when all time user login </summary>
function WindowCollapse(isFirst) {
    $("[id$=imgShowDiv]").parent().hide();
    $("[id$=imgHideDiv]").parent().show();
    $("[id$=divLeftSide]").show(300);
    $("[id$=divRightSide]").removeClass("right-side-expanded");
    $("[id$=divRightSide]").addClass("right-side");
    if (!isFirst)
        SetCookie($("[id$=UserPk]").val(), "right-side");
}
/// <summary> Expand / collapse Favourites Section </summary>
function Favcol() {
    if ($("[id$=favourates]").is(":hidden")) {
        $("[id$=favourates]").slideDown("slow");
        $("[id$=imgFavColl]").attr("alt", "Collapse");
        $("[id$=imgFavColl]").attr("title", "Collapse");
        $("[id$=imgFavColl]").attr("src", "../images/ERP-Blue/grandtrustERP-button-minus.gif");
    }
    else {
        $("[id$=favourates]").slideUp("slow");
        $("[id$=imgFavColl]").attr("alt", "Expand");
        $("[id$=imgFavColl]").attr("title", "Expand");
        $("[id$=imgFavColl]").attr("src", "../images/ERP-Blue/grandtrustERP-button-plus.gif");
    }
}
/// <summary> Expand / collapse Recent Section  </summary>
function Recentcol() {
    if ($("[id$=divRecent]").is(":hidden")) {
        $("[id$=divRecent]").slideDown("slow");
        $("[id$=imgRecentColl]").attr("alt", "Collapse");
        $("[id$=imgRecentColl]").attr("title", "Collapse");
        $("[id$=imgRecentColl]").attr("src", "../images/ERP-Blue/grandtrustERP-button-minus.gif");
    }
    else {
        $("[id$=divRecent]").slideUp("slow");
        $("[id$=imgRecentColl]").attr("alt", "Expand");
        $("[id$=imgRecentColl]").attr("title", "Expand");
        $("[id$=imgRecentColl]").attr("src", "../images/ERP-Blue/grandtrustERP-button-plus.gif");
    }
}
/// <summary> Creates cookie for specified time and with specified name </summary>
function CreateCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}
/// <summary> Returns the cookie with specified name; else returns null </summary>
function GetCookie(name) {
    var userName = name + "=";
    winAllCookie = document.cookie.split(';');
    for (var i = 0; i < winAllCookie.length; i++) {
        winCookie = winAllCookie[i];
        while (winCookie.charAt(0) == ' ') winCookie = winCookie.substring(1, winCookie.length);
        if (winCookie.indexOf(userName) == 0) return winCookie.substring(userName.length, winCookie.length);
    }
    return null;
}
/// <summary> set the Cookie as corr user name and it's and it's value  </summary>
function SetCookie(name, masterClass) {
    winAllCookie = document.cookie.split(';');
    for (var i = 0; i < winAllCookie.length; i++) {
        winCookie = winAllCookie[i];
        var className = winCookie.split("=");
        if ($.trim(className[0]) == $.trim(name)) {
            document.cookie = "";
            CreateCookie(name, masterClass, 365);
        }
    }
}
/// <summary> Resets form validation binding </summary>
function ResetFormValidation() {
    $(document.forms[0]).unbind('submit');
}
/// <summary> Resets the page to original state </summary>
function ClearPage() {
    $('[id$=imbSave]').hide();
    $('[id$=imbAdd]').show();
    $('#divListing').show();
    $('#divData').hide();
    return false;
}
/// <summary> Sets the page to Add New state </summary>
function AddNew() {
    $('[id$=imbAdd]').hide();
    $('[id$=imbSave]').show();
    $('#divListing').hide();
    $('#divData').show();
    return false;
}

// Numeric oerators only control handler

// Numeric oerators only control handler
jQuery.fn.ForceNumericOnly =
        function () {
            return this.each(function () {
                $(this).attr("autocomplete", "off");
                $(this).keydown(function (e) {
                    var key = e.charCode || e.keyCode || 0;
                    var text = $(this).val();
                    // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                    // home, end, period, and numpad decimal
                    if (e.shiftKey === true) {
                        return false;
                    }
                    if (key == 190 || key == 110) {
                        if (text.indexOf(".") >= 0) {
                            return false;
                        }
                    }
                    return (
                        key == 8 ||
                    //key == 16 ||

                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
                });
            });
        };
//Allows only integer values
jQuery.fn.ForceNumersOnly =
        function () {
           
            return this.each(function () {
                $(this).attr("autocomplete", "off");
                $(this).keydown(function (e) {
                    var key = e.charCode || e.keyCode || 0;
                    var text = $(this).val();
                    // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                    // home, end, period, and numpad decimal
                    if (e.shiftKey === true) {
                        return false;
                    }
                    if (key == 190 || key == 110) {//Prevent '.'
                        return false;
                    }
                    return (
                        key == 8 ||
                    //key == 16 ||

                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
                });
            });
        };
jQuery.fn.ForceToNumeric =
        function () {
            return this.each(function () {
                $(this).attr("autocomplete", "off");
                $(this).keydown(function (e) {
                    var key = e.charCode || e.keyCode || 0;
                    //                    alert(key);
                    var text = $(this).val();
                    // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                    // home, end, period, and numpad decimal
                    if (e.shiftKey === true) {
                        return false;
                    }
                    if (key == 190 || key == 110) {
                        if (text.indexOf(".") >= 0) {
                            return false;
                        }
                    }
                    if (key == 109 || key == 189 || key == 173) {
                        if (text.indexOf("-") >= 0) {
                            return false;
                        }
                    }

                    return (
                        key == 8 ||
                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        key == 109 || // for negative       
                        key == 189 || // for negative (in IE & Chrome)     
                        key == 173 || // for negative (in Fire fox)      
                       (key >= 35 && key <= 40) ||
                       (key >= 48 && key <= 57) ||
                       (key >= 96 && key <= 105));
                });
            });
        };

function BindMenuAutoComplete() {

    $("[id$=MenuSearchList]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "CommonManagement.do?Action=GetMenuAutoComplete",
                data: {
                    SearchValue: request.term,
                    UserPK: $("[id$=UserPk]").val(),
                    BizUnit: $("[id$=BizUnitPk]").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Text, // format the the data as text 
                            Value: item.Value
                        }
                    }));
                }
            });
        },
        cache: false,
        select: function (event, ui) {
            var absolutePath = $("[id$=AbsolutePath]").val();
            window.location = absolutePath + ui.item.Value;
        }
    });
}

function ShowMenu() {

    ///<summary>
    ///function used to Show the Menu
    ///</summary>
    var innerWidth = "auto";
    //fix the width issue with ie 7 or less
    if ($.browser.msie) {
        if ($.browser.version < 8) {
            innerWidth = "970px";
        }
    }
    $("#divMenu").dialog({
        width: innerWidth,
        resizable: false,
        title: "Menu",
        modal: true,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolderMenu")
                          .removeClass("w55perc")
                                .addClass("w55perc")
                                .removeClass("left-22perc")
                                .addClass("left-22perc");
        }
    });
    return false;
}


function WkfSubmit() {
    ShowContainerDivWkf('#Wofkflowdiv', 'Translate(Submit)', '700');
    return false;
}

function ShowContainerDivWkf(containerID, title, width, height) {

    ///<summary>
    ///function used to Show the Menu
    ///</summary>
    if (!width)
        width = 970;
    if (!height)
        height = 'auto';
    if (!title)
        title = errorTitle;
    $(containerID).dialog('distroy');
    $("#popupHolder").html("");
    $(containerID).dialog({
        width: width,
        draggable: false,
        height: height,
        resizable: false,
        title: title,
        modal: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
            $('#divmodel').show();
            $("#divmodel").css({ "height": $('html').height() });
            if ($('#popupHolder div.ui-dialog').is(':visible')) {
                var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                $('html').scrollTop(0);
                $('html,body').animate({ scrollTop: 0 });
                $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
            }
            $("html").css({ "overflow": "hidden" });
        },
        close: function (event) {

            if (typeof AfterClose == "function") {
                AfterClose(containerID);
            }
            //$(this).remove();
            $('#divmodel').hide();
            $("html").css({ "overflow": "auto" });
        }
    });
    return false;
}

function ShowDivNoOverlay(containerID, title, width, height) {

    ///<summary>
    ///function used to Show the Menu
    ///</summary>
    if (!width)
        width = 970;
    if (!height)
        height = 'auto';
    if (!title)
        title = errorTitle;
    $(containerID).dialog('distroy');
    $("#popupHolder").html("");
    $(containerID).dialog({
        width: width,
        draggable: false,
        height: height,
        resizable: false,
        title: title,
        modal: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
        },
        close: function (event) {

            if (typeof AfterClose == "function") {
                AfterClose(containerID);
            }
        }
    });
    return false;
}
function ShowContainerDivNew(containerID, title, width, height) {
    ///<summary>
    ///function used to Show the Menu
    ///</summary>

    if (!width)
        width = 970;
    if (!height)
        height = 520;
    if (!title)
        title = errorTitle;
    $(containerID).dialog('distroy');
    $("#popupHolder").html("");
    //To set fit to screen
    $('html, body').animate({ scrollTop: '0px' }, 0);
    $('html, body').css('overflow', 'hidden');
    $(containerID).dialog({
        width: width,
        draggable: false,
        height: height,
        resizable: false,
        title: title,
        modal: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
            $('#divmodel').show();
            $("#divmodel").css({ "height": $('html').height() });
            if ($('#popupHolder div.ui-dialog').is(':visible')) {
                var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                $('html').scrollTop(0);
                $('html,body').animate({ scrollTop: 0 });
                $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
            }
            if (typeof AfterOpen == "function") {
                AfterOpen(containerID);
            }
        },
        close: function (event) {
            $('html').css('overflow', 'auto');
            $('body').css('overflow', 'visible');
            $('#divmodel').hide();
            if (typeof AfterClose == "function") {
                AfterClose(containerID);
            }
            //            $(this).remove();
        }
    });
    return false;
}
function ShowCommonCotainerDiv(containerID, title, top) {
    ///<summary>
    ///function used to Show common popup
    ///</summary>
    //ShowContainerDiv(containerID, title, 1050, 620);
    if (!top)
        top = "1%";
    ShowContainerDivPer(containerID, title, "89.5%", 620, top)
    return false;
}
function ShowContainerDivPer(containerID, title, width, height, top) {

    ///<summary>
    ///function used to Show the Menu
    ///</summary>
    if (!width)
        width = 970;
    if (!height)
        height = 520;
    if (!top)
        top = "10%";
    if (!title)
        title = errorTitle;
    $(containerID).dialog('distroy');
    $("#popupHolder").html("");
    //To set fit to screen
    $('html, body').animate({ scrollTop: '0px' }, 0);
    $('html, body').css('overflow', 'hidden');
    $(containerID).dialog({
        width: width,
        draggable: false,
        height: height,
        resizable: false,
        title: title,
        modal: false,
        open: function (event, ui) {
            $(this).parent().appendTo("#popupHolder");
            $('#divmodel').show();
            $("#divmodel").css({ "height": $('html').height() });
            if ($('#popupHolder div.ui-dialog').is(':visible')) {
                var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                $('html').scrollTop(0);
                $('html,body').animate({ scrollTop: 0 });
                $('#popupHolder div.ui-dialog').css('top', top);
                var widthDiff = ($('html').width() - $('#popupHolder div.ui-dialog').width()) / 2;
                if (widthDiff > 0)
                    $('#popupHolder div.ui-dialog').css('left', "5%");
            }
            $("html").css({ "overflow": "hidden" });
        },
        close: function (event) {

            if (typeof AfterClose == "function") {
                AfterClose(containerID);
            }
            //$(this).remove();
            $('#divmodel').hide();
            $('html').css('overflow', 'auto');
            $('body').css('overflow', 'visible');
        }
    });
    return false;
}

function ShowContainerDiv(containerID, title, width, height, afterCloseCallBack) {

    ///<summary>
    ///function used to Show the Menu
    ///</summary>

    if (!width)
        width = 970;
    if (!height)
        height = 520;
    if (!title)
        title = errorTitle;
    $(containerID).dialog('distroy');
    $("#popupHolder").html("");
    $(containerID).dialog({
        width: width,
        draggable: false,
        height: height,
        resizable: false,
        title: title,
        modal: false,
        open: function (event, ui) {

            $(this).parent().appendTo("#popupHolder");
            $('#divmodel').show();
            $("#divmodel").css({ "height": $('html').height() });
            if ($('#popupHolder div.ui-dialog').is(':visible')) {
                var heightDiff = ($('html').height() - $('#popupHolder div.ui-dialog').height()) / 2;
                $('#divmodel').height(heightDiff > 0 ? $('html').height() : $('#popupHolder div.ui-dialog').height());
                $('html').scrollTop(0);
                $('html,body').animate({ scrollTop: 0 });
                $('#popupHolder div.ui-dialog').css('top', heightDiff > 0 ? heightDiff : 0);
                var widthDiff = ($('html').width() - $('#popupHolder div.ui-dialog').width()) / 2;
                if (widthDiff > 0)
                    $('#popupHolder div.ui-dialog').css('left', (parseFloat(widthDiff) + 3.8));
            }
            $("html").css({ "overflow": "hidden" });
        },
        close: function (event) {

            if (afterCloseCallBack == null) {
                if (typeof AfterClose == "function") {
                    AfterClose(containerID);
                }
            } else {
                afterCloseCallBack(containerID);
            }

            if (typeof AfterJournalClose == "function") {
                AfterJournalClose(containerID);
            }
            //$(this).remove();
            $('#divmodel').hide();
            $("html").css({ "overflow": "auto" });
        }
    });
    return false;
}

function ClosePopup() {
    $('#divmodel').hide();
    $("html").css({ "overflow": "auto" });
}

function DisableAuto(extender, hfield) {
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
    $(extender).autocomplete("option", "disabled", true);
    $(extender).attr("disabled", true);
    $(extender).addClass('input-disabled');
}

function EnableAuto(extender) {
    $(extender).removeAttr("disabled");
    $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
    $(extender).autocomplete("option", "disabled", false);
    $(extender).removeClass('input-disabled');
}

function HideForView() {
    $(".ddlSelect").removeClass("ddlSelect").addClass("ddlSelect-disable");
    $("input,textarea").not("input:submit").not("input:button").not("input:image").attr("disabled", true);
}

function OpenPDF(url) {
    if (url != "") {
        var win = window.open(url, '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');
        if (!win) {
            var eMsg = "<span><ul><li>" + errorMessage + ' ' + window.location.host + "</li></ul></span>";
            GrandScriptUtils.ShowModal(eMsg, errorTitle);
            $("#MSGBox").addClass("error");
        }
        return false;
    }
}
function GetPDFUrl(url) {
    if (url != "") {
        var winurl = "window.open('" + url + "', '_blank', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=1200,height=800,left=100,top=0');return false;";
        return winurl;
    }
    else
        return "";
}

function ShowTooltip(ddl) {
    $("[id$=" + ddl + "]").attr("title", $("select[id$=" + ddl + "] :selected").text());
    //dropdown expand set title 
    $("[id$=" + ddl + "]").children('option').each(function () {
        $(this).attr("title", $(this).text());
    });
}

function OpenUploadDocs(url) {
    if (url != "") {
        var win = window.open(url, '_self', 'location=no,menubar=no,scrollbars=yes,titlebar=no,toolbar=no,width=600,height=400,left=200,top=100');
        if (!win) {
            var eMsg = "<span><ul><li>" + errorMessage + ' ' + window.location.host + "</li></ul></span>";
            GrandScriptUtils.ShowModal(eMsg, errorTitle);
            $("#MSGBox").addClass("error");
        }
        return false;
    }
}

function CompareDate(StartDate, EndDate) {
    ///<summary>
    ///check whether the StartDate is greater than the EndDate
    ///</summary>
    var RetVal = 0;
    var SDate = StartDate.split("-");
    var EDate = EndDate.split("-");
    var day1 = SDate[0];
    var month1 = GetMonth(StartDate);
    var year1 = SDate[2];

    var day2 = EDate[0];
    var month2 = GetMonth(EndDate);
    var year2 = EDate[2];
    if (year1 > year2) {
        RetVal = 1;  //Start date is greater than EndDate
    }
    else if (year1 == year2 && month1 > month2) {
        RetVal = 1;  //Start date is greater than EndDate
    }
    else if (year1 == year2 && month1 == month2 && day1 > day2) {
        RetVal = 1;  //Start date is greater than EndDate
    }
    return RetVal;
}
function GetMonth(EnteredDate) {
    ///<summary>
    ///For getting month from dd-mmm-yyyy format 
    ///</summary>
    var date = EnteredDate.split("-");
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    for (var j = 0; j < months.length; j++) {
        if (date[1] == months[j]) {
            // date[1] = months.indexOf(months[j]) + 1;
            date[1] = j + 1;
        }
    }
    if (date[1] < 10) {
        date[1] = '0' + date[1];
    }
    return date[1];
}

function FormatAmount(sender) {
    ///<summary>
    ///For formatting an amount with comma seperator(Only for Numeric user control)
    ///</summary>
    var senderid = sender.id;
    var number = $("#[id*=" + sender.id + "]").val().replace(new RegExp(',', 'g'), '');
    var decimalDigits = $("#[id*=" + senderid.replace("txtFormattedAmount", "hdfAmountDecimals") + "]").val();
    var isCommaSep = $("#[id*=" + senderid.replace("txtFormattedAmount", "hdfIsCommaSep") + "]").val();
    var isNumGrp = $("#[id*=" + senderid.replace("txtFormattedAmount", "hdfIsNumGrp") + "]").val();
    if (isNaN(parseInt(decimalDigits)))
        decimalDigits = 2;
    if (!isNaN(parseFloat(number))) {
        number = parseFloat(number).toFixed(decimalDigits);
        var FormattedNumber = number;
        if (!isNaN(parseInt(isCommaSep)) && isCommaSep == 1) {
            var curGroup1 = 3;
            var curGroup2 = 3;
            var NumericPart = "", LastNumericPart = "", DecimalPart = "";
            if (!isNaN(parseInt(isNumGrp)) && parseInt(isNumGrp) == 1) {
                if (!isNaN(parseFloat($("#[id*=hdfNumberGroup1]").val()))) {
                    curGroup1 = parseFloat($("#[id*=hdfNumberGroup1]").val());
                }
                if (!isNaN(parseFloat($("#[id*=hdfNumberGroup2]").val()))) {
                    curGroup2 = parseFloat($("#[id*=hdfNumberGroup2]").val());
                }
            }
            else {
                if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
                    curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
                }
                if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
                    curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
                }
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
                if (isNaN(NumericPart) && NumericPart.length == 1) {
                    LastNumericPart = LastNumericPart.substr(1, LastNumericPart.length);
                }
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
        }
        $("#[id*=" + sender.id + "]").val(FormattedNumber);
        return FormattedNumber;
    }
    else {
        FormattedNumber = 0;
        $("#[id*=" + sender.id + "]").val(FormattedNumber.toFixed(decimalDigits));
    }
}

function FormatNumber(number) {
    ///<summary>
    ///For formatting an amount with comma seperator
    ///</summary>

    var FormattedNumber = number.toString();
    var curGroup1 = 3;
    var curGroup2 = 3;
    var NumericPart = "", LastNumericPart = "", DecimalPart = "";
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
        curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
    }
    if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
        curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
    }
    alert(number);
    DecimalPart = number.toString().split('.')[1];
    (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
    NumericPart = number.toString().split('.')[0];
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

function parseNumber(numberwithcomma) {
    ///<summary>
    ///Method to remove commas from an amount
    ///</summary>
    var actualnum = numberwithcomma.replace(new RegExp(',', 'g'), '');
    if (!isNaN(parseFloat(actualnum)))
        return parseFloat(actualnum);
    else
        return 0;
}
