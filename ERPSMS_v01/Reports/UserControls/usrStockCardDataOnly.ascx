<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrStockCardDataOnly.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrStockCardDataOnly" %>

<%@ Register Src="~/UserControls/CheckListSearchControl.ascx" TagPrefix="uc1" TagName="CheckListSearchControl" %>
<%@ Register Src="~/UserControls/CheckListSearchControlNew.ascx" TagPrefix="uc2" TagName="CheckListSearchControlNew" %>
<script type="text/javascript">
    function usrInitComponents() {
        //usrDateInit();
        //GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, false);
        GrandScriptUtils.RestrictedYearDatePicker("FromDate", false, true, true, $("[id$=hdfFromDate]").val(), $("[id$=hdfToDate]").val());
        GrandScriptUtils.RestrictedYearDatePicker("ToDate", false, true, true, $("[id$=hdfFromDate]").val(), $("[id$=hdfToDate]").val());
    }
    function usrDateInit() {
        //<summary>function used to make datepicker</summary>
        GrandScriptUtils.DatePicker("FromDate", false, false);
        GrandScriptUtils.DatePicker("ToDate", false, false);
    }

    function postBackByObject()
    {
         var o = window.event.srcElement;
         if (o.tagName == "INPUT" && o.type == "checkbox")
        {
           __doPostBack("","");
        } 
    }

    function SearchCheckListCategory(txtSearch, cblCtrl) {
        if ($(txtSearch).val() != "" && $(txtSearch).val().length > 3) {
            var count = 0;
            $(cblCtrl).children('tbody').children('tr').each(function () {
                var match = false;
                $(this).children('td').children('label').each(function () {
                    if ($(this).text().toUpperCase().indexOf($(txtSearch).val().toUpperCase()) > -1)
                        match = true;
                });
                if (match) {
                    $(this).show();
                    count++;
                }
                else { $(this).hide(); }
            });
        }
        else {
            $(cblCtrl).children('tbody').children('tr').each(function () {
                $(this).show();
            });
            $('#spnCount').html('');
        }
    }
    function SelectAllCategory(evt) {
        if ($(evt).is(":checked")) {
            $("[id$=cblCategoryList] input[type=checkbox]").each(function (index) {
                var isVisible = $(this).closest("td").find("label").is(':visible');
                if (isVisible == true) {
                    $(this).attr("checked", "checked");
                }
            });
        }
        else {
            $("[id$=cblCategoryList] input[type=checkbox]").each(function (index) {
                $(this).removeAttr("checked");
            });
        }
        $("[id$=btnCategory]").click();
    }

//treeview - start
        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }
//treeview - end


</script>
<style>
    #ctl00_MainContent_userFilter_CheckListSearchControlNew_txtSearchItem + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    #spnCount {
        display: none
    }

    #ctl00_MainContent_userFilter_CheckListSearchControl1_txtSearch + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    .input-w81per {
        min-width: 79% !important;
        max-width: 79% !important;
    }

    .treelist-scroll {
        height: 100px;
        overflow: auto;
        margin-bottom: 10px;
        width: 362px;
    }

    #ctl00_MainContent_userFilter_txtSearchCategory + span {
        background: transparent;
        border: none;
        padding: 0;
        margin-top: 1px !important;
    }
</style>

<asp:UpdatePanel ID="pnlTestFilter" runat="server" class="">
    <ContentTemplate>
        <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
            <div class="fields-group">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" AssociatedControlID="ddlFinYear"></asp:Label>
                                <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="rfvFinYear" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlFinYear" ValidationGroup="fltr"
                                    Display="Static" Text="*" ErrorMessage="<%$ resources:MISFilterLabel,Err_SelectFinYear%>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblFrom" runat="server" Text="<%$ resources:MISFilterLabel,From %>" AssociatedControlID="FromDate"></asp:Label>
                                <asp:TextBox ID="FromDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfFromDate" />

                                <asp:Label ID="lblTo" runat="server" Text="<%$ resources:MISFilterLabel,To %>"
                                    AssociatedControlID="ToDate" CssClass="lbl-30perc"></asp:Label>
                                <asp:TextBox ID="ToDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfToDate" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <%--<asp:Label ID="lblItemCategory" runat="server" Text="<%$ resources:MISFilterLabel,ItemCategory %>" AssociatedControlID="ddlItemCategory"></asp:Label>
                                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>--%>

                                <div class="tree-label2M w600">
                                 <%--   <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.ItemCategory %></label>--%>
                                    <div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div style="overflow: auto;">
                                                    <%--                                                    <asp:TextBox ID="txtSearchCategory" runat="server" onkeyup="SearchCheckListCategory(this,'#cblCategoryList');"
                                                        CssClass="input-w81per margn-lft3" placeholder="Search Text">
                                                    </asp:TextBox>
                                                    <asp:CheckBox ID="chkAll" ToolTip="Select All" onclick="SelectAllCategory(this);" runat="server"
                                                        CssClass="margntop4" />
                                                    <span id="spnCount"></span>
                                                    <div class="treelist-scroll margn-lft3" runat="server" id="divEdocPopUp">
                                                        <asp:CheckBoxList ID="cblCategoryList" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                                                            CssClass="treelist" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                                        </asp:CheckBoxList>
                                                    </div>--%>

                                                    <div class="treeview">
                                                        <asp:TreeView ID="trvCategoryList" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static"
                                                            OnTreeNodeCheckChanged="ActionHandler" ShowCheckBoxes="All" CssClass="middle-lbl-xsmall-c">
                                                            <NodeStyle Font-Bold="True" />
                                                            <RootNodeStyle Font-Bold="True" />
                                                            <ParentNodeStyle Font-Bold="True" />
                                                        </asp:TreeView>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Store %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlStores" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl1" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">

                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:MISFilterLabel,Classification %>" AssociatedControlID="ddlClassification"></asp:Label>
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-half"></asp:DropDownList>


                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Items %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlItemChecklist" runat="server">
                                            <ContentTemplate>
                                                <uc2:CheckListSearchControlNew runat="server" ID="CheckListSearchControlNew" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblTransaction" runat="server" Text="<%$ resources:MISFilterLabel,TransactionsOnly %>"
                                    CssClass="label-11-26" AssociatedControlID="chkTransaction"></asp:Label>
                                <asp:CheckBox ID="chkTransaction" runat="server" />

                                <asp:Label ID="lblExcludeMatReturn" runat="server" Text="<%$ resources:MISFilterLabel,ExcludeMaterialReturn %>"
                                    AssociatedControlID="chkExcludeMatReturn"></asp:Label>
                                <asp:CheckBox ID="chkExcludeMatReturn" runat="server" />

                                <asp:Label ID="lblAllItems" runat="server" Text="<%$ resources:MISFilterLabel,AllItems %>"
                                    CssClass="label-11perc" AssociatedControlID="chkAllItems"></asp:Label>
                                <asp:CheckBox ID="chkAllItems" runat="server" />
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnCategory" OnClick="ActionHandler" CommandName="ADDITEM" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
