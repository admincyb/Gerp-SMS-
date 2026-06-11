/// <reference path="../../jquery/jquery-1.5-vsdoc.js" />

///#region------ Intitial Section ----------------

var GrandTreeHeaderStructure = new Object();

function SetTreeHeaderStructure(targetID, src, title, showCheckBox, showImg, extraParam, expandAll, pval, partialChecked) {
    ///<summary>Set the needed tree parameter  </summary>
    /// <param name="targetID" type="String">
    ///     A string containing id of the div.
    /// </param>    
    /// <param name="src" type="String">
    ///     A string containing the URL to which the request is sent.
    /// </param>
    /// <param name="title" type="String">
    ///     A string containing title of the root.
    /// </param>
    /// <param name="showCheckBox" type="Bool">
    ///     A string containing if need checkbox set as true .
    /// </param>
    /// <param name="showImg" type="Bool">
    ///     A string containing if the tree node want to edit set as true .
    /// </param>
    /// <param name="extraParam" type="String">
    ///     A string containing first param .
    /// </param>
    /// <param name="expandAll" type="Bool">
    ///     A string containing if the node expand all .
    /// </param>
    /// <param name="partialChecked" type="Bool">
    ///   partialChecked=="true" if the checkbox checked all its child will checked , if any one of child checked then thw parent checkbox should checked
    ///   partialChecked=="false" it will not checked its parent or its child
    ///   partialChecked=="undefined" if the checkbox checked all its child will checked and also if all child checked then the parent checkbox should checked
    /// </param>
    /// <param name="pval" type="String">
    ///     For aditional Parent Node
    /// </param>
    GrandTreeHeaderStructure = {
        TargetID: targetID,
        Src: src,
        Title: title,
        ShowCheckBox: showCheckBox,
        ShowImg: showImg,
        ExtraParam: extraParam,
        ExpandAll: expandAll,
        PartialChecked: partialChecked,
        jTreeHtml: "",
        pval: pval
    }
}

///#endregion

///#region------ Render Section ----------------

function MakeMultiTree() {
    ///<summary>Function used to call the ajax request </summary>
    if (GrandTreeHeaderStructure.Src != undefined) {
        $("#" + GrandTreeHeaderStructure.TargetID).html("");
        var reqURL = "";
        if (GrandTreeHeaderStructure.pval==null)
            reqURL = GrandTreeHeaderStructure.Src + GrandTreeHeaderStructure.ExtraParam;
        else
            reqURL = GrandTreeHeaderStructure.Src + GrandTreeHeaderStructure.ExtraParam + GrandTreeHeaderStructure.pval;
        $.get(reqURL, function (data) {
            BindMultiParentTree(data);
            if (data.length > 0) {
                if (data[0].TreeHasChild == "false")
                    $("#updateProgress").hide();
            }
        });
    }
    return false;
}

function BindMultiParentTree(data) {
    ///<summary>Function used render parent tree structure </summary>
    /// <param name="data" type="Object">
    ///     The data have the result.
    /// </param>
  
    GrandTreeHeaderStructure.jTreeHtml = "<ul ><li class=\"root\"><span id=\"span_0\" style=\"float:left;\" onclick=\"javascript:AddTreeSelected(this);\" >" + GrandTreeHeaderStructure.Title + "</span><div class=\"clear\"></div><ul  class=\"treeview\">";
    if (data) {
        if (data.length > 0) {
            for (var i in data) {
                GrandTreeHeaderStructure.jTreeHtml += "<li class=\"expandable\" id=\"" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeValue + "\">";
                GrandTreeHeaderStructure.jTreeHtml += (data[i].TreeHasChild == "true" ? "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_imgexp_" + data[i].TreeValue + "\" " + (GrandTreeHeaderStructure.ExpandAll == true ? "class = \"treeminus\"" : "class = \"treeplus\"") + "  onclick=\"javascript:ExpandCollapseMultiChild(this);\" />" : "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_line_" + data[i].TreeValue + "\" class=\"treeline\" />");
                GrandTreeHeaderStructure.jTreeHtml += (GrandTreeHeaderStructure.ShowCheckBox == true ? "<input id=\"" + GrandTreeHeaderStructure.TargetID + "_checkbox_" + data[i].TreeValue + "\" onclick=\"javascript:CheckUnCheckMultiChild(this)\" type=\"checkbox\" " + (data[i].TreeChecked == "true" ? "checked=\"checked\"" : "") + " />" : "");
                GrandTreeHeaderStructure.jTreeHtml += "<span id=\"" + GrandTreeHeaderStructure.TargetID + "_span_" + data[i].TreeValue + "\" style=\"cursor:pointer;\" onclick=\"javascript:AddTreeSelected(this);\" >" + data[i].TreeText + "</span><input type=\"hidden\" id=\"" + GrandTreeHeaderStructure.TargetID + "_hidden_" + data[i].TreeValue + "\" value=\"" + data[i].TreeSave + "\" >" + (data[i].TreeSave == "true" ? ("<a class=\"treeitem\" title=\"Material\" />") : "") + "";
                GrandTreeHeaderStructure.jTreeHtml += (GrandTreeHeaderStructure.ShowImg == true ? (data[i].TreeShowEdit == "false" ? "" : "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_imgEdit_" + data[i].TreeValue + "\" class=\"treeedit\" alt=\"Edit\" title=\"Edit\" onclick=\"javascript:EditTreeSelected(this);\" />") : "") + "</li>";
                if (GrandTreeHeaderStructure.ExpandAll) {
                    if (i == 0) {
                        GrandTreeHeaderStructure.jTreeHtml += "</ul></li></ul>";
                        $("#" + GrandTreeHeaderStructure.TargetID).html(GrandTreeHeaderStructure.jTreeHtml);
                    }
                    else
                        $("#" + GrandTreeHeaderStructure.TargetID).find("ul:eq(1)").append(GrandTreeHeaderStructure.jTreeHtml);
                    GrandTreeHeaderStructure.jTreeHtml = "";
                  
                    if (data[i].TreeHasChild == "true")
                        BindMultiChildTree(data[i].TreeValue);
                    
                }
            }
            if (!GrandTreeHeaderStructure.ExpandAll) {
                GrandTreeHeaderStructure.jTreeHtml += "</ul></li></ul>";
                $("#" + GrandTreeHeaderStructure.TargetID).html(GrandTreeHeaderStructure.jTreeHtml);
            }
            $("#" + GrandTreeHeaderStructure.TargetID).find("li:last").attr("class", "last");
        }
    }
}

function BindMultiChildTree(id) {
    ///<summary>Function used render child tree corresponding to the parent structure </summary>
    /// <param name="id" type="String">
    ///    The Id is the parent tree id, by using this id get its child element.
    /// </param>
  
    $.get(GrandTreeHeaderStructure.Src + id, function (data) {
        if (data) {
            if (data.length > 0) {
                GrandTreeHeaderStructure.jTreeHtml = "";
                for (var i in data) {
                    GrandTreeHeaderStructure.jTreeHtml += "<li class=\"expandable\" id=\"" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeValue + "\">";
                    GrandTreeHeaderStructure.jTreeHtml += (data[i].TreeHasChild == "true" ? "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_imgexpChild_" + data[i].TreeValue + "\" " + (GrandTreeHeaderStructure.ExpandAll == true ? "class = \"treeminus\"" : "class = \"treeplus\"") + " onclick=\"javascript:ExpandCollapseMultiChild(this);\" />" : "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_lineChild_" + data[i].TreeValue + "\" class=\"treeline\" />");
                    GrandTreeHeaderStructure.jTreeHtml += (GrandTreeHeaderStructure.ShowCheckBox == true ? "<input  id=\"" + GrandTreeHeaderStructure.TargetID + "_checkbox_" + data[i].TreeValue + "\" onclick=\"javascript:CheckUnCheckMultiChild(this)\" " + ($("#li_" + data[i].TreeParentValue).find("input[type=checkbox]:first").attr("checked") == true ? "checked=\"checked\"" : "") + " type=\"checkbox\" " + (data[i].TreeChecked == "true" ? "checked=\"checked\"" : "") + " />" : "");
                    GrandTreeHeaderStructure.jTreeHtml += "<span id=\"" + GrandTreeHeaderStructure.TargetID + "_span_" + data[i].TreeValue + "\" style=\"cursor:pointer;\" onclick=\"javascript:AddTreeSelected(this);\" >" + data[i].TreeText + "</span><input type=\"hidden\" id=\"" + GrandTreeHeaderStructure.TargetID + "_hidden_" + data[i].TreeValue + "\" value=\"" + data[i].TreeSave + "\" >" + (data[i].TreeSave == "true" ? ("<a class=\"treeitem\" title=\"Material\" />") : "") + "";
                    GrandTreeHeaderStructure.jTreeHtml += (GrandTreeHeaderStructure.ShowImg == true ? (data[i].TreeShowEdit == "false" ? "" : "<a id=\"" + GrandTreeHeaderStructure.TargetID + "_imgEdit_" + data[i].TreeValue + "\" class=\"treeedit\" alt=\"Edit\" title=\"Edit\" onclick=\"javascript:EditTreeSelected(this);\" />") : "") + "</li>";
                    if ($("#" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeParentValue).children("ul").length == 0)
                        $("#" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeParentValue).append("<ul></ul>");
                    $("#" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeParentValue).find("ul").append(GrandTreeHeaderStructure.jTreeHtml);
                    $("#" + GrandTreeHeaderStructure.TargetID + "_li_" + data[i].TreeParentValue).find("a:first").removeAttr("disabled");
                    GrandTreeHeaderStructure.jTreeHtml = "";
                  
                    if (GrandTreeHeaderStructure.ExpandAll) {
                        if (data[i].TreeHasChild == "true")
                            BindMultiChildTree(data[i].TreeValue);
                            else
                                $("#updateProgress").hide();

                    }
                    $("#updateProgress").hide();
                }
                $("#" + GrandTreeHeaderStructure.TargetID + "_li_" + data[0].TreeParentValue).find("li:last").attr("class", "last");
            }
        }
    });
}

///#endregion

///#region ----- Core Methods ------------------


function AddTreeSelected(liAdd) {
    /// <param name="liAdd" type="Object">
    ///    The liAdd is the selected li object 
    /// </param>

    ///<summary>Function used Add the treeData </summary>

    if (typeof AddSelectedTree == "function") {
        AddSelectedTree(liAdd);
    }
}

function EditTreeSelected(liEdit) {
    ///<summary>Function used Edit the treeData </summary>
    /// <param name="liEdit" type="Object">
    ///    The liEdit is the selected li object 
    /// </param>

    if (typeof EditSelectedTree == "function") {
        EditSelectedTree(liEdit);
    }
}

function CheckUnCheckMultiChild(jTreeChk) {
    ///<summary>Function used check or uncheck all its child </summary>
    /// <param name="jTreeChk" type="Object">
    ///    The jTreeChk the checked/unchecked tree object
    /// </param>

    if (GrandTreeHeaderStructure.PartialChecked != false) {
        $(jTreeChk).parent().children("ul").find("li input[type=checkbox]").each(function () { // gets the all it's checkbox child  
            var jTreeChkID = $(this).attr("id");
            jTreeChkID = jTreeChkID.substr(jTreeChkID.lastIndexOf("_") + 1, jTreeChkID.length);
            var jTreeChkName = $(this).parent().children("span").html();
            if ($(jTreeChk).attr("checked")) // if the parent checkbox checked all its child checkbox will be checked, vise versa
                $(this).attr("checked", true);
            else
                $(this).attr("checked", false);
        });
    }
    if (GrandTreeHeaderStructure.PartialChecked == true) // if the tree type is PartialChecked then work .... 
        CheckUnCheckPartialMultiParent(jTreeChk);
    else if(GrandTreeHeaderStructure.PartialChecked == undefined)
        CheckUnCheckMultiParent(jTreeChk);
}

function CheckUnCheckMultiParent(jTreeChk) {
    ///<summary>Function used check or uncheck all its parents </summary>
    /// <param name="jTreeChk" type="Object">
    ///    The jTreeChk the checked/unchecked tree object
    /// </param>

    var liObj = $(jTreeChk).parent().parent("ul").parent("li");
    if ($(liObj).attr("id") != "") {
        if ($(jTreeChk).attr("checked")) {
            var isChecked = true;
            $(jTreeChk).parent().parent("ul").children("li").each(function () { // gets all its childrens li
                if (!$(this).children("input[type=checkbox]").is(":checked")) // this used that all childs checkbox checked then the parent checkbox will be checked
                    isChecked = false;
            });
            if (isChecked)
                $(liObj).find("input[type=checkbox]").attr("checked", true);
        }
        else
            $(liObj).find("input[type=checkbox]:first").attr("checked", false);
    }
    if ($(liObj).find("input[type=checkbox]:first").parent().parent("ul").attr("class") != "treeview") // the recursion will be break when it sees treeview class div
        CheckUnCheckMultiParent($(liObj).children("input[type=checkbox]"));
    else
        if (typeof AfterTreeCheck == "function") { // if after the checkbox checked any function want  to take place please add this function
            AfterTreeCheck(jTreeChk);
        }
}

function CheckUnCheckPartialMultiParent(jTreeChk) {
    ///<summary>Function used check or uncheck all its parents </summary>
    /// <param name="jTreeChk" type="Object">
    ///    The jTreeChk the checked/unchecked tree Object
    /// </param>

    var liObj = $(jTreeChk).parent().parent("ul").parent("li");
    if ($(liObj).attr("id") != "") {
        if ($(jTreeChk).attr("checked"))
            $(liObj).find("input[type=checkbox]:first").attr("checked", true);
        else {
            var isChecked = false;
            $(jTreeChk).parent().parent("ul").children("li").each(function () { // gets all its childrens li
                if ($(this).children("input[type=checkbox]").is(":checked")) // this used that any of the childs checkbox checked then the parent checkbox will be checked
                    isChecked = true;
            });
            if (isChecked)
                $(liObj).find("input[type=checkbox]:first").attr("checked", true);
            else
                $(liObj).find("input[type=checkbox]:first").attr("checked", false);
        }
    }
    if ($(jTreeChk).parent().parent("ul").attr("class") != "treeview") // the recursion will be break when it sees treeview class div
        CheckUnCheckPartialMultiParent($(liObj).children("input[type=checkbox]"));
    else
        if (typeof AfterTreeCheck == "function") { // if after the checkbox checked any function want  to take place please add this function
            AfterTreeCheck(jTreeChk);
        }
}

function ExpandCollapseMultiChild(jTreeImg) {
    ///<summary>Function used expand or collapse child tree </summary>
    /// <param name="jTreeImg" type="Object">
    ///    The jTreeImg expand image Object
    /// </param>
  
    var treeClass = $(jTreeImg).attr("class");
    $(jTreeImg).attr("class", "treeminus");
    var liClass = $(jTreeImg).parent("li").attr("class");
    var targetLiID = $(jTreeImg).parent().attr("id");
    if ($("#" + targetLiID).find("ul").html() == null) {
        $("#" + targetLiID).find("ul").replaceWith("");
        $("#" + targetLiID).find("a:first").attr("disabled", "disabled");
        var jTreeImgID = $(jTreeImg).attr("id"); // get the selected tree id
        jTreeImgID = jTreeImgID.substr(jTreeImgID.lastIndexOf("_") + 1, jTreeImgID.length);
        BindMultiChildTree(jTreeImgID);
    }
    else {
        if (treeClass=="treeminus") {
            $(jTreeImg).attr("class", "treeplus");
            $(jTreeImg).parent().find("ul").slideToggle(10);
        }
        else {
            $(jTreeImg).attr("class", "treeminus");
            $(jTreeImg).parent().find("ul").slideToggle(10);
        }
    }
    if (liClass != "last") {
        if (liClass == "expandable")
            $(jTreeImg).parent("li").attr("class", "collapsable");
        else
            $(jTreeImg).parent("li").attr("class", "expandable");
    }
}

///#endregion