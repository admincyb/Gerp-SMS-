<%@ Page Title="<%$ Resources:Captions,Title_PayrollTypeMaster %>" Language="C#"
    AutoEventWireup="true" CodeBehind="PayrollTypeMaster.aspx.cs" Inherits="HRMS.Admin.Masters.PayrollTypeMaster"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .search-colapse-b
        {
            margin-bottom: 8px;
            height: 20px;
            background: #f5f5f5;
        }
        .width-14per
        {
            min-width: 14%;
            max-width: 14%;
        }
        .width-40per
        {
            min-width: 40%;
            max-width: 40%;
        }
    </style>
    <script type="text/javascript">
        function InitComponents() {
            ShowHideAdvancedSearch(1);
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
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
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            debugger;
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
                        //Page_Validators.splice(i, 1);
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

        //Treeview

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
        //Treeview

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <%--Top Buttons "Save", ...--%>
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            TabIndex="35" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            TabIndex="36" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                            TabIndex="37" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" TabIndex="13" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" TabIndex="14" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--Page Datas--%>
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="0" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="0" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblProcessModeSrch" runat="server" Text="<%$ resources:ProcessMode %>"
                                                AssociatedControlID="ddlProcessModeSrch" CssClass="middle-lbl"></asp:Label>
                                            <asp:DropDownList ID="ddlProcessModeSrch" runat="server" TabIndex="4" CssClass="select-small-a margnbotm0">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblTypeCodeSrch" runat="server" Text="<%$ resources:PayrollTypeCode%>"
                                                AssociatedControlID="txtTypeCodeSrch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTypeCodeSrch" TabIndex="2" CssClass="input-small-a margnbotm0"
                                                MaxLength="100"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblTypeNameSrch" runat="server" Text="<%$ resources:PayrollTypeName%>"
                                                AssociatedControlID="txtTypeNameSrch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTypeNameSrch" TabIndex="3" CssClass="input-half margnbotm0"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="5"
                                                CommandName="FILTER" SkinID="search-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="6" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--<table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" CssClass="grdTable" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="11" />
                                                <asp:HiddenField runat="server" ID="hdfPayrollTypePkListPage" Value='<%# Eval("PTM_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListPTM_MOD_DT" Value='<%# Eval("PTM_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PayrollTypeCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PTM_CODE")),10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PTM_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PayrollTypeName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PTM_NAME"), 30) %>'
                                                    ToolTip='<%#System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PTM_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PTM_DESC")),65) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PTM_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:ProcessMode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProcessMode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PTM_PRC_MODE_TEXT")),10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PTM_PRC_MODE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("PTM_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="12" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("PTM_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="12" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPayrollTypeCode" runat="server" Text="<%$ resources:PayrollTypeCodeStar%>"
                                                AssociatedControlID="txtPayrollTypeCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPayrollTypeCode" TabIndex="30" CssClass="input-half"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPayrollTypeCode" runat="server" ControlToValidate="txtPayrollTypeCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterPayrollTypeCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPayrollTypeName" Text="<%$ resources:PayrollTypeNameStar%>"
                                                AssociatedControlID="txtPayrollTypeName" class=""></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPayrollTypeName" TabIndex="31" CssClass="input-half"
                                                MaxLength="200"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPayrollTypeName" runat="server" ControlToValidate="txtPayrollTypeName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterPayrollTypeName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtPayrollTypeDesc"></asp:Label>
                                            <asp:TextBox ID="txtPayrollTypeDesc" runat="server" TabIndex="32" MaxLength="500"
                                                TextMode="MultiLine" Height="40" CssClass="input-full" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <div class="div2col-S">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProcessMode" runat="server" Text="<%$ resources:ProcessModeStar %>"
                                                    AssociatedControlID="ddlProcessMode"></asp:Label>
                                                <asp:DropDownList ID="ddlProcessMode" TabIndex="33" runat="server" CssClass="select-small-e">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPayrollStart" runat="server" Text="<%$ resources:PayrollStart %>"
                                                    AssociatedControlID="ddlPayrollStart" CssClass="middle-lbl-small-c"></asp:Label>
                                                <asp:DropDownList ID="ddlPayrollStart" TabIndex="33" runat="server" CssClass="select-small">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblOffset" runat="server" Text="<%$ resources:Offset %>" AssociatedControlID="ddlOffset"></asp:Label>
                                                <asp:DropDownList ID="ddlOffset" TabIndex="33" runat="server" CssClass="select-small-a">
                                                    <asp:ListItem Text="<%$ resources:Offset_Previous %>" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ resources:Offset_Current %>" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"
                                                    CssClass="middle-lbl"></asp:Label>
                                                <asp:CheckBox ID="chkPayrollTypeActive" runat="server" Checked="true" TabIndex="34" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="div2col-S">
                                                <asp:HiddenField ID="hdfSerialNo" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                            </table>
                            <div class="w57-7perc float-left">
                                <div class="clear">
                                </div>
                                <div class="treeview-center">
                                    <div class="treeview max-182">
                                        <%-- <asp:Label runat="server" ID="lblUser" Text="<%$ resources:User %>" Font-Bold="true"></asp:Label>--%>
                                        <asp:TreeView ID="trvUser" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static"
                                            OnTreeNodeCheckChanged="ActionHandler" ShowCheckBoxes="All" onclick="OnCheckBoxCheckChanged(event);">
                                            <NodeStyle Font-Bold="false" />
                                            <RootNodeStyle Font-Bold="True" />
                                            <ParentNodeStyle Font-Bold="True" />
                                        </asp:TreeView>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
