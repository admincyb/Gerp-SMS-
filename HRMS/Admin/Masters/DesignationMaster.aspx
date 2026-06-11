<%@ Page Title="<%$ Resources:Captions,Title_Designation %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DesignationMaster.aspx.cs" Inherits="HRMS.Admin.Masters.DesignationMaster"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">

        //        function InitComponents() {
        //            $(document).ready(function () {
        //                ShowHideEarnings(1);
        //                ShowHideDeductions(1);
        //            });
        //        }

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
                                        <asp:Button runat="server" ID="Button1" CommandName="SAVE" TabIndex="15" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="15" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="3" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="4" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <%--Listing Page Table Row--%>
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
                                                TabIndex="5" />
                                            <%--   ToolTip="<%$ resources:ShowFilter%>"--%>
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="5" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblJobCategSrch" runat="server" Text="<%$ resources:JobCategory%>"
                                                AssociatedControlID="ddlJobCategSrch"></asp:Label>
                                            <asp:DropDownList ID="ddlJobCategSrch" runat="server" TabIndex="12" CssClass="select-medium">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblJobGradeSrch" runat="server" Text="<%$ resources:Captions,JobLevel%>"
                                                AssociatedControlID="ddlJobGradeSrch"></asp:Label>
                                            <asp:DropDownList ID="ddlJobGradeSrch" runat="server" TabIndex="12" CssClass="select-small-a">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterCode" runat="server" Text="<%$ resources:DesignationCode%>"
                                                AssociatedControlID="txtFilterDesigCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDesigCode" TabIndex="6" CssClass="input-half margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="label6" runat="server" Text="<%$ resources:DesignationName%>" AssociatedControlID="txtFilterDesigName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterDesigName" TabIndex="7" CssClass="input-half margnbotm0"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="8"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="9" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfDesignationPkListPage" Value='<%# Eval("dsgPK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListdsgModOn" Value='<%# Eval("dsgModOn") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DesignationCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesignationCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("dsgCode")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgCode")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DesignationName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("dsgName"), 35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgName")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                            <HeaderStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("dsgDesc")),85) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgDesc")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="25%" />
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:JobCategory%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobCateg" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgJobCategoryText")))%>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgJobCategoryText")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="13%" />
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Captions,JobLevel%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobGrade" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgJobGradeText")))%>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("dsgJobGradeText")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("dsgActive").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("dsgActive").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="11" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="12" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDesignationCode" runat="server" Text="<%$ resources:DesignationCodeStar%>"
                                                AssociatedControlID="txtDesignationCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDesignationCode" TabIndex="11" CssClass="input-half"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDesignationCode" runat="server" ControlToValidate="txtDesignationCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDesignationCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label runat="server" ID="lblDesignationName" Text="<%$ resources:DesignationNameStar%>"
                                                AssociatedControlID="txtDesignationName" class="middle-lbl-small-b1"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDesignationName" TabIndex="12" CssClass="input-w64per"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDesignationName" runat="server" ControlToValidate="txtDesignationName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDesignationName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lbljobCategory" runat="server" Text="<%$ resources:JobCategoryStar%>"
                                                AssociatedControlID="ddljobCategory"></asp:Label>
                                            <asp:DropDownList ID="ddljobCategory" runat="server" TabIndex="12" CssClass="select-medium">
                                            </asp:DropDownList>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvjobCategory" InitialValue="-1" CssClass="star"
                                                    SetFocusOnError="true" ValidationGroup="Save" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddljobCategory" Display="Static" Text="*" ErrorMessage="<%$ resources:SelectJobCategory%>"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label ID="lblJobLevel" runat="server" Text="<%$ resources:JobGradeStar%>" AssociatedControlID="ddlJobLevel"
                                                CssClass="lbl-13-3perc"></asp:Label>
                                            <asp:DropDownList ID="ddlJobLevel" runat="server" TabIndex="12" CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvjobLevel" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="ddlJobLevel"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:SelectJobGrade%>"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="13" MaxLength="500" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="w50perc">
                                        <div class="treeview-center margnlft24-5per">
                                            <label runat="server">
                                            </label>
                                            <div class="treeview max-182 w93perc">
                                                <asp:TreeView ID="trvUser" runat="server" ShowLines="True" ExpandDepth="0" ClientIDMode="Static"
                                                    OnTreeNodeCheckChanged="ActionHandler" ShowCheckBoxes="All" onclick="OnCheckBoxCheckChanged(event);">
                                                    <NodeStyle Font-Bold="false" />
                                                    <RootNodeStyle Font-Bold="True" />
                                                    <ParentNodeStyle Font-Bold="True" />
                                                </asp:TreeView>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="w50perc">
                                        <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"
                                            CssClass="lbl-20-5perc margn-rgt4"></asp:Label>
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="14" CssClass="margntop3" />
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
