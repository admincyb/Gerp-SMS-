/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />
/// <reference path="../../GrandGridMulti.js" />
/// <reference path="../../GrandScriptUtils.js" />
/// <reference path="../../jquery/UI/jquery-ui.min.js" />
/// <reference path="../../jquery/Validate/jquery.validate.js" />


///#region----------- Global Variable Declaration ---------------

var advSearchList = new Array();

var advSearchObj = function (srchDtlPK, srchID, srchBy, srchByText, opr, srchValue, cond) {
    this.SLNo = advSearchList.length + 1;
    this.SearchDtlPK = srchDtlPK;
    this.SearchID = srchID;
    this.SearchBy = srchBy;
    this.SrchByText = srchByText;
    this.Operator = opr;
    this.SearchValue = srchValue;
    this.Condition = cond;
}

///#endregion

//#region----------- Configuration Section ----------------

var AdvanceSearch = {
    PageTitle: "",
    AutoCompleteURL: "",
    SaveSearchTemplate: "AdvanceSearch.do?Action=SaveAdvanceSearch",
    GetSearchTemplate: "AdvanceSearch.do?Action=GetAdvanceSearchList",
    GetSearchDetails: "AdvanceSearch.do?Action=GetAdvanceSearchDetails",
    SearchTemplateSavedMsg: "Translate(SearchTemplateNameSaved)",
    SearchTemplateUsedMsg: "Translate(SearchTemplateNameAlreadyUsed)",
    SearchTypeUsedMsg: "Translate(SearchTypeAlreadyUsed)",
    Information: "Translate(Information)",
    ActionFailed: "Translate(ActionFailedPleaseTryAgain)",
    SearchCriteriaValidation: "Translate(SearchCriteriaValidation)",
    SearchTypeValidation: "Translate(SearchTypeValidation)",
    SearchDateValueValidation: "Translate(DateValidation)",
    SearchTemplateNameValidation: "Translate(SearchTemplateNameValidation)",
    SearchPk: "SRH_PK"
}

//#endregion

///#region----------- Initialization Section----------------

$(document).ready(function () {
    $.validator.addMethod('selectNone', function (value, element) {
        return ($(element).val() != "0");
    }, 'Translate(Pleaseselectanoption)');
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false
    });
    PageSearchInit();
});

function PageSearchInit() {
    ///<summary> initial page condition </summary>

    MakeAutoComplete();
    $("[id$=AdvSrchType]").append($("[id$=SearchType]").html());
    AdvanceSearch.PageTitle = window.location.pathname.substr(window.location.pathname.lastIndexOf("/") + 1, location.pathname.lastIndexOf(".aspx"));
    AdvanceSearchReset();
    HideAdvSearch();
    $("#tabs").tabs();
    $("#tabs").tabs("select", 0);
    $("#grdAdvSrch").hide();
    $("#TemplateDiv").dialog({ autoOpen: false, width: 400 });
}

///#endregion

///#region----------- Core Section----------------

//function ShowAdvSearch() {
//    ///<summary>function To set show advance search and hide  search  </summary>

//    var searchOpt = new Array();
//    searchOpt[0] = { "Text": "Material Code", "Value": "TmrDc" };
//    searchOpt[1] = { "Text": "Material Name", "Value": "TmrNem" }; // if you want more search option please do like this otherwise it will take default search option
//    var advSearchProperty = {
//        AutoCompleteURL: "MaterialManagement.do?Action=GetSearchValue",
//        SearchOption: searchOpt
//    }
//    SetAdvanceSearch(advSearchProperty);
//    return false;
//}

function SetAdvanceSearch(searchOption) {
    ///<summary> function To set show advance search,some proprties and hide  search </summary>
    ///<param name="searchOption" type="Object">
    /// used to set the automcomplete url,any more search criteria if not provide it will take the default search types,page title
    ///</param>

    AdvanceSearch.AutoCompleteURL = searchOption.AutoCompleteURL;
    AdvanceSearch.PageTitle = window.location.pathname.substr(window.location.pathname.lastIndexOf("/") + 1, location.pathname.lastIndexOf(".aspx"));
    $("[id$=PageTitle]").val(AdvanceSearch.PageTitle);
    var drpID = $("select[id$=AdvSrchType]").attr("id");
    if (searchOption.SearchOption.length > 0) {
        GrandScriptUtils.FillDropDown(drpID, searchOption.SearchOption, true, true);
    }
    $("#searchwrap").hide();
    $("#tabs").tabs("select", 0);
    $("[id$=AdvDateSrchValue]").hide();
    $("#AdvanceSearch").show();
    AdvanceSearchReset();
}

function HideAdvSearch() {
    ///<summary>function To set show search and hide advance search  </summary>

    $("#searchwrap").show();
    $("#AdvanceSearch").hide();
    return false;
}

function SetAdvSearch() {
    ///<summary>function To set date or autocomplete  </summary>

    var strname = $("select[id$=AdvSrchType] option:selected").text();
    if (strname.toLocaleLowerCase().search("date") != -1) {
        $("[id$=AdvDateSrchValue]").show()
        $("[id$=AdvSrchValue]").hide();
        GrandScriptUtils.DatePicker("AdvDateSrchValue");
    }
    else {
        $("[id$=AdvDateSrchValue]").hide();
        $("[id$=AdvSrchValue]").show();
    }
}

function FillAdvSearch(tr) {
    ///<summary>function To bind the details based on the search id </summary>

    var srchPk = GrandGrid.Utilities.GetColumnValue(tr, AdvanceSearch.SearchPk, $(tr).parents("table:eq(0)").attr("id"));
    $.get(AdvanceSearch.GetSearchDetails + "&SearchPK=" + srchPk, function (data) {
        if (data) {
            $("[id$=SearchPK]").val(data.SearchPK);
            $("[id$=SearchName]").val(data.SearchName);
            advSearchList = data.SearchDetails;
            if (!($.isArray(data.SearchDetails))) {
                advSearchList = new Array();
                advSearchList.push(data.SearchDetails);
            }
            RenderAdvSearchList();
            $("#tabs").tabs("select", 0);
        }
    });
    return false;
}

function AddToAdvSearchList() {
    ///<summary>function To add the list of selected search criteria  </summary>

    RemoveAdvSrchValidations();
    AddAdvSrchValidations(1);
    if ($(document.forms[0]).valid()) {
        var isExists = false;
        var srchID = $("[id$=SearchID]").val();
        var srchDtlPK = $("[id$=SearchDtlPK]").val();
        var srchBy = $("[id$=AdvSrchType]").val();
        var srchByText = $("[id$=AdvSrchType] option:selected").text();
        var opr = $("[id$=AdvSrchOpr] option:selected").text();
        var srchVal = $("[id$=AdvSrchValue]").css("display") == "none" ? $("[id$=AdvDateSrchValue]").val() : $("[id$=AdvSrchValue]").val();
        for (var i in advSearchList) {
            if (advSearchList[i].SearchBy == srchBy) {
                isExists = true;
                break;
            }
        }
        if (!isExists) {
            advSearchList.push(new advSearchObj(srchDtlPK, srchID, srchBy, srchByText, opr, srchVal, "AND"));
            RenderAdvSearchList();

        }
        else
            GrandScriptUtils.ShowModal(AdvanceSearch.SearchTypeUsedMsg, AdvanceSearch.Information);
        ClearAdvSrch();
    }
    return false;
}

function DeleteAdvSearch(tr) {
    ///<summary>function To delete the selected item in the search list  </summary>

    for (var i in advSearchList) {
        if (advSearchList[i].SearchBy === $(tr).find("td:eq(0)").html()) {
            advSearchList.splice(i, 1);
            RenderAdvSearchList();
        }
    }
    return false;
}

function SaveAdvSrchTemplate() {
    ///<summary>function To save search template details </summary>

    RemoveAdvSrchValidations();
    AddAdvSrchValidations(2);
    $("[id$=SearchDetails]").val(JSON.stringify(advSearchList));
    $("[id$=UserID]").val($("[id$=UserPk]").val());
    $("[id$=SearchPK]").val($("[id$=SearchPK]").val() == "" ? "0" : $("[id$=SearchPK]").val());
    if (advSearchList.length == 0) {
        GrandScriptUtils.ShowModal(AdvanceSearch.SearchCriteriaValidation , AdvanceSearch.Information);
        return false;
    }
    if ($(document.forms[0]).valid()) {
        var jsonString = GrandScriptUtils.FormToJsonString("TemplateDiv");
        $.post(AdvanceSearch.SaveSearchTemplate, jsonString, function (data) {
            if (parseInt(data) > 0) {
                GrandScriptUtils.ShowModal(AdvanceSearch.SearchTemplateSavedMsg, AdvanceSearch.Information);
                ClearAdvSearchTemplate();
                BindAdvSrchGrid();
                AdvSearch();
            }
            else if (parseInt(data) == 0) {
                GrandScriptUtils.ShowModal(AdvanceSearch.SearchTemplateUsedMsg, AdvanceSearch.Information);
            }
            else {
                GrandScriptUtils.ShowModal(AdvanceSearch.ActionFailed, AdvanceSearch.Information);
            }
            ClearAdvSrch();
            $("#TemplateDiv").dialog('close');
        });
    }
    return false;
}

function BindAdvSrchGrid() {
    ///<summary>function To bind search template grid </summary>

    $("[id$=PageTitle]").val(AdvanceSearch.PageTitle);
    var ajaxUrl = AdvanceSearch.GetSearchTemplate + "&PageTitle=" + $("[id$=PageTitle]").val();
    $("#grdSrchTemplate").removeAttr("ajaxurl")
    $("#grdSrchTemplate").attr("ajaxurl", ajaxUrl);
    GrandGrid.MakeGrid($("#grdSrchTemplate"));
}

function AdvSearch() {
    ///<summary>function To pass the selected search criteria  </summary>

    var advSrchCond = "";
    for (var i in advSearchList) {
        advSrchCond += advSearchList[i].SearchBy + " ";
        advSrchCond += advSearchList[i].Operator + " '";
        advSrchCond += advSearchList[i].SearchValue + "' ";
        if (i != advSearchList.length - 1)
            advSrchCond += advSearchList[i].Condition + " ";
    }
    if (advSearchList.length > 0) {
        if (typeof AdvanceSearchInvoke == "function") {
            AdvanceSearchInvoke(advSrchCond);
        }
    }
    else
        GrandScriptUtils.ShowModal(AdvanceSearch.SearchCriteriaValidation, AdvanceSearch.Information);
}

function AdvSaveAndSearch() {
    ///<summary>function To open the model popup for add search template  </summary>

    if ($("[id$=SearchPK]").val() == "") {
        ClearAdvSearchTemplate();
    }
    $("#TemplateDiv").dialog('open').parent().appendTo($("form:first"));
    $("#TemplateDiv").dialog({ width: 450 , height: 170 });
}

function AdvanceSearchReset() {
    ///<summary>function To reset search template details </summary>

    ClearAdvSrch();
    ClearAdvSearchTemplate();
    advSearchList = new Array();
    RenderAdvSearchList();
}

function ClearAdvSrch() {
    ///<summary>function To clear search details </summary>

    $("[id$=AdvSrchType]").val(0);
    $("[id$=AdvSrchOpr]").val(0);
    $("[id$=AdvSrchValue]").val("");
    $("[id$=SearchID]").val(0);
    $("[id$=SearchDtlPK]").val(0);
}

function ClearAdvSearchTemplate() {
    ///<summary>function To clear search temaplate details </summary>

    $("[id$=SearchPK]").val(0);
    $("[id$=SearchName]").val("");
}

function MakeAutoComplete() {
    ///<summary> To make the autocomplete functionality </summary>

    $("[id$=AdvSrchValue]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: AdvanceSearch.AutoCompleteURL,
                data: {
                    SearchValue: request.term,
                    SearchType: $("[id$=AdvSrchType]").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Text // format the the data as text 
                        }
                    }));
                }
            });
        },
        cache: false
    });
}

///#endregion

//#region----------- Render Section ----------------

function RenderAdvSearchList() {
    ///<summary>function To render the selected search criteria  </summary>

    $("#grdAdvSrch tbody").replaceWith("");
    var advSrchHtml = "<tbody>";
    for (var i in advSearchList) {
        advSrchHtml += "<tr>";
        advSrchHtml += "<td style=\"text-align:left;display:none;visibility:hidden;\" >" + advSearchList[i].SearchBy + "</td>";
        advSrchHtml += "<td style=\"text-align:left;width:45%;\" >" + advSearchList[i].SrchByText + "</td>";
        advSrchHtml += "<td style=\"text-align:center;width:17%;\" >" + advSearchList[i].Operator + "</td>";
        advSrchHtml += "<td style=\"text-align:left;width:23%;\" >" + advSearchList[i].SearchValue + "</td>";
        advSrchHtml += "<td style=\"text-align:center;display:none;visibility:hidden;\" >" + advSearchList[i].Condition + "</td>";
        advSrchHtml += "<td style=\"text-align:center;width:15%;\" ><input type=\"image\" style=\"border-width: 0px;height:18px;width:18px;\" onclick=\"javascript:return DeleteAdvSearch($(this).parents('tr:eq(0)'));\" src=\"../Images/ERP-Blue/Buttons/erp-grid-delete.png\" title=\"Delete\" id=\"imbDelete\"></td>";
        advSrchHtml += "</tr>";
    }
    advSrchHtml += "</tbody>";
    $("#grdAdvSrch").append(advSrchHtml);
    if (advSearchList.length > 0)
        $("#grdAdvSrch").show();
    else
        $("#grdAdvSrch").hide();
}

//#endregion

///#region----------- Validations ----------------

function AddAdvSrchValidations(mode) {
    ///<summary>function To Validations </summary>

    switch (mode) {
        case 1:
            $("[id$=AdvSrchType]").rules("add", {
                selectNone: true,
                messages: { selectNone: AdvanceSearch.SearchTypeValidation }
            });
            if ($("[id$=AdvDateSrchValue]").css("display") == "none") {
                $("[id$=AdvSrchValue]").rules("add", {
                    required: true,
                    maxlength: 20,
                    messages: { required: AdvanceSearch.SearchCriteriaValidation }
                });
            }
            else {
                $("[id$=AdvDateSrchValue]").rules("add", {
                    required: true,
                    messages: { required: AdvanceSearch.SearchDateValueValidation }
                });
            }
            break;
        case 2:
            $("[id$=SearchName]").rules("add", {
                required: true,
                maxlength: 50,
                messages: { required: AdvanceSearch.SearchTemplateNameValidation }
            });
            break;
    }
}

function RemoveAdvSrchValidations() {
    //<summary>function Remove Validation</summary>

    $(document.forms[0]).validate().resetForm();
}

///#endregion
