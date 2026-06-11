/// <reference path="../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../GrandScriptUtils.js" />
/// <reference path="../GrandGridMulti.js" />

var winCookie = "";
var winAllCookie = "";

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

function ValidationMessageSetting() {

    $("#errMsgContainer").dialog({
        autoOpen: false,
        modal: true,
        close: function (e, ui) {
            $("#errMsgContainer ul").html("");
        }
    });
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
            $(this).parent().appendTo("#popupHolder");
        }
    });
    return false;
}

function ShowContainerDiv(containerID, title, width, height) {
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
        },
        close: function (event) {

            if (typeof AfterClose == "function") {
                AfterClose(containerID);
            }
            //$(this).remove();
            $('#divmodel').hide();
        }
    });
    return false;
}

function ClosePopup() {
    $('#divmodel').hide();
}

function ShowTooltip(ddl) {
    $("[id$=" + ddl + "]").attr("title", $("select[id$=" + ddl + "] :selected").text());
    //dropdown expand set title 
    $("[id$=" + ddl + "]").children('option').each(function () {
        $(this).attr("title", $(this).text());
    });
}
