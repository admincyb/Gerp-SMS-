<%@ Page Title="<%$ Resources:Captions,Title_SalaryReportTemplate %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="SalaryReportTemplate.aspx.cs"
    Inherits="HRMS.Admin.Masters.SalaryReportTemplate" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtSalRptValue]").ForceNumericOnly();
            $("[id*=txtGroupSequence]").ForceNumericOnly();

            HideRowUpDownIcon();
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
        //Validation Summary
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
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

        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
        }

        //Extra Grid
        function AfterGridExpand(row) {
            if ($("[id$=grdGroup]").attr('id') == $(row).parent().parent().attr('id')) {
                {
                    $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
                }
            }
        }

        function AfterGridExpand(row) {

            if ($("[id$=grdSalaryRptTemplate]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpanded]");
                if (hdf.val() == "0") {
                    // $(row).find("input[id*=btnGetSalaryRptDetails]").click();
                }
            }
        }

        //Sequence START
        function SendUp(CurrentRow, PreviousRow) {
            PreviousRow.parentNode.insertBefore(CurrentRow, PreviousRow);
        }
        function SequenceSwap(CurrentRow, PreviousRow) {
            var curId = CurrentRow.cells[1].children[0].id;
            var preId = PreviousRow.cells[1].children[0].id;
            var curSeq = $("[id$=" + curId + "]").val();
            var preSeq = $("[id$=" + preId + "]").val();
            curSeq = parseInt(curSeq);
            preSeq = parseInt(preSeq);
            curSeq = (isNaN(curSeq) || curSeq == "undefined") ? 1 : curSeq;
            preSeq = (isNaN(preSeq) || preSeq == "undefined") ? curSeq - 1 : preSeq;
            $("[id$=" + curId + "]").val(preSeq);
            $("[id$=" + preId + "]").val(curSeq);
            ShowRowUpDownIcon();
        }
        //function to select a row from Right Table to move up
        function MoveUp(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            for (i = 2; i < RightTable.rows.length; i++) {
                if (i == index) //is selected ?
                {
                    SendUp(RightTable.rows[i], RightTable.rows[i - 1]);
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i - 1]);
                }
            }
            HideRowUpDownIcon();
        }
        //function to select a row to move down
        function MoveDown(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            var RowToMove = 0;
            var PreviousRow;
            var CurrentRow;
            for (i = 1; i < RightTable.rows.length - 1; i++) {
                if (i == index) {
                    RightTable.rows[i];
                    RowToMove = i;
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i + 1]);
                    RightTable.rows[i].parentNode.appendChild(RightTable.rows[i]); //appends the selected row to the end of the Right Table
                    //this code moves the appended row up till it reaches
                    //to one position less than its original position
                    for (i = RightTable.rows.length - 1; i > RowToMove + 1; i--) {
                        CurrentRow = RightTable.rows[i];
                        PreviousRow = RightTable.rows[i - 1];
                        SendUp(CurrentRow, PreviousRow);
                    }
                }
            }
            HideRowUpDownIcon();
        }
        function ShowRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            //var totalRows = $('[id*=grdActivityList] tr').length - 0;   //Total Row Count
            $('[id*=grdPayElt] tr:nth-child(2)').find("[id*=imbRuleUpPop]").show(); //Show first rows Up arrow
            $('[id*=grdPayElt] tr:last-child').find("[id*=imbRuleDownPop]").show(); //Show last rows Down Arrow
            //$('[id*=grdActivityList] tr:nth-child(' + totalRows + ')').find("[id*=imbRuleDown]").show(); //Show last rows Down Arrow
        }
        function HideRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            //var totalRows = $('[id*=grdActivityList] tr').length - 0;   //Total Row Count
            $('[id*=grdPayElt] tr:nth-child(2)').find("[id*=imbRuleUpPop]").hide(); //Hide first rows Up arrow
            $('[id*=grdPayElt] tr:last-child').find("[id*=imbRuleDownPop]").hide(); //Hide last rows Down Arrow
            //$('[id*=grdActivityList] tr:nth-child(' + totalRows + ')').find("[id*=imbRuleDown]").hide(); //Hide last rows Down Arrow
        }
        //Sequence END 
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="15" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
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
                                        <asp:Button runat="server" TabIndex="1" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="2" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="2" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:Controls,ShowFilter%>"
                                                TabIndex="3" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:Controls,HideFilter%>"
                                                TabIndex="3" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblSalRptTempList" AssociatedControlID="txtTemplateSrchList"
                                                Text="<%$resources:SalaryTemplateName %>" CssClass="lbl-13perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateSrchList" CssClass="lbl-20perc margnbotm0"
                                                TabIndex="4">
                                            </asp:TextBox>
                                            <asp:ImageButton ID="imgFilterList" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="5"
                                                CommandName="TEMPLATEFILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="imgClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="5" OnClick="ActionHandler"
                                                CommandName="TEMPLATECLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    CssClass="grdTable" EmptyDataRowStyle-CssClass="emptytable" TabIndex="6">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="7" GroupName="SelectOne" />
                                                <asp:HiddenField runat="server" ID="hdfTemplatePkListPage" Value='<%# Eval("SRT_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateName %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTemplateName" Text='<%# Eval("SRT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTemplateRemark" Text='<%# Eval("SRT_DESC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
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
                        <%--new mode--%>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSalRptName" runat="server" Text="<%$ resources:SalaryTemplateName%>"
                                                AssociatedControlID="txtSalRptName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSalRptName" TabIndex="11" CssClass="input-half"
                                                MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvSalRptName" runat="server" ControlToValidate="txtSalRptName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterSalTemplName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label runat="server" ID="lblSalRptValue" Text="<%$ resources:SalaryTemplateValue%>"
                                                AssociatedControlID="txtSalRptValue" class="middle-lbl-small-b1"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSalRptValue" TabIndex="12" CssClass="input-w64per"
                                                MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBonusTypeName" runat="server" ControlToValidate="txtSalRptValue"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterSalTemplValue%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox ID="txtRemarks" runat="server" TabIndex="13" TextMode="MultiLine" onkeypress="return this.value.length<490"
                                                onpaste="return this.value.length<490" Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="button-wrap-right">
                                <asp:Button runat="server" ID="btnAddGroup" CommandName="SHOWPOPUP" TabIndex="13"
                                    ValidationGroup="Save" CssClass="margntop-1 margn-rgt0" Style="margin-top: -2.5px;"
                                    SkinID="btnInner-add" Text="<%$resources:NewGroup %>" OnClick="ActionHandler"
                                    ToolTip="<%$resources:NewGroup %>" />
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divDetailsGrid">
                                <div class="gridwrap hierarchical-wrap max-380">
                                    <cc1:ExtGridView runat="server" ID="grdSalaryRptTemplate" AutoGenerateColumns="False"
                                        Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                        ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfGroupPK" Value='<%# Eval("GroupPk") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfSequence" Value='<%# Eval("GroupSequence") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfGroupHdrPK" Value='<%# Eval("GroupHdrPK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfGroupActive" Value='<%# Eval("GroupActive") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--NAME--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrpName" runat="server" Text='<%# Eval("GroupName") %>' Font-Bold="true"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfIsExpanded" Value="0" />
                                                    <%--<asp:HiddenField runat="server" ID="hdfGroupPK" Value='<%# Eval("GroupPK") %>'></asp:HiddenField>--%>
                                                    <asp:Button runat="server" ID="btnGetSalaryRptDetails" OnClick="ActionHandler" CommandName="SALARYRPTDETAILS"
                                                        EnableTheming="false" Style="display: none" />
                                                </ItemTemplate>
                                                <ItemStyle Width="90%" HorizontalAlign="Left" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <%--ACTIONS--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgItemAdd" runat="server" CommandName="ADDPAYELTITEM" SkinID="imbaddnew"
                                                        OnClick="ActionHandler" TabIndex="14" ToolTip="<%$resources:Controls,Add %>" />
                                                    <asp:ImageButton ID="imbGroupEdit" runat="server" CommandName="EDITGROUP" SkinID="imbeditgrid"
                                                        OnClick="ActionHandler" ToolTip="<%$resources:Controls,Edit %>" TabIndex="14" />
                                                    <asp:ImageButton ID="btnGroupRemove" runat="server" OnClick="ActionHandler" CommandName="DELETEGROUP"
                                                        CommandArgument='<%# Eval("GroupPK") %>' OnClientClick="return ShowDeleteConfirm(this);"
                                                        SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls, Delete%>" TabIndex="14" />
                                                </ItemTemplate>
                                                <ItemStyle Width="8.4%" />
                                            </asp:TemplateField>
                                            <%--GRIDVIEW--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:GridView runat="server" ID="grdPayElt" PageSize="<%$ resources:PageSize%>" AllowSorting="True"
                                                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblPayEltEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <%--Hidden Fields--%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hdfPayElementPK" Value='<%# Eval("PayElementPK") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfPayElementDispName" Value='<%# Eval("PayElementDispName") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfPayElementName" Value='<%# Eval("PayElementName") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfItemActive" Value='<%# Eval("TDL_ACTIVE") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfTdlPK" Value='<%# Eval("TDL_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                            </asp:TemplateField>
                                                            <%--Sequence Buttons--%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hdfItemSequence" Value='<%# Eval("TDL_SEQUENCE") %>' />
                                                                    <asp:ImageButton CssClass="nomargin" ID="imbRuleUpPop" SkinID="move-up" runat="server"
                                                                        OnClientClick="MoveUp(this);return false;" ToolTip="<%$ resources:MoveUp %>"
                                                                        CommandArgument='<%# Eval("TDL_SEQUENCE") %>' TabIndex="15"></asp:ImageButton>
                                                                    <asp:ImageButton CssClass="nomargin" ID="imbRuleDownPop" SkinID="move-down" runat="server"
                                                                        OnClientClick="MoveDown(this);return false;" ToolTip="<%$ resources:MoveDown %>"
                                                                        CommandArgument='<%# Eval("TDL_SEQUENCE") %>' TabIndex="15"></asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <%--Name--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:PayEltName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblPayElmtName" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementName")),30) %>'
                                                                        ToolTip='<%#Eval("PayElementName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <%--Display Name--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:DisplayName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblPayElmtDispName" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementDispName")),80) %>'
                                                                        ToolTip='<%#Eval("PayElementDispName")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="25%" />
                                                            </asp:TemplateField>
                                                            <%--Actions--%>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imbItemEdit" runat="server" CommandName="EDITPAYELTITEM" SkinID="imbeditgrid"
                                                                        OnClick="ActionHandler" ToolTip="<%$resources:Controls,Edit %>" TabIndex="23" />
                                                                    <asp:ImageButton ID="imbItemRemove" runat="server" OnClick="ActionHandler" CommandName="DELETEPAYELTITEM"
                                                                        CommandArgument='<%# Eval("TDL_PK") %>' OnClientClick="return ShowDeleteConfirm(this);"
                                                                        SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls, Delete%>" TabIndex="23" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3.5%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                        <FooterStyle CssClass="table-firstlevela-total" />
                                    </cc1:ExtGridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="divNewGroup_PopUp" style="display: none">
                <div class="content-wrapper">
                    <div id="divGroupDetails" class="div2col-S">
                        <table class="table-devide">
                            <tr>
                                <asp:Label ID="lbnGroupName" runat="server" Text="<%$resources:GroupNameStar %>"
                                    AssociatedControlID="txtGroupName" CssClass="lbl-34perc margn-rgt-0"></asp:Label>
                                <asp:TextBox ID="txtGroupName" runat="server" MaxLength="200" TabIndex="25" onkeyup="limitText(this,200);"
                                    CssClass="margn-rgt-0 input-small-e" onkeydown="limitText(this,200);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfGroupName" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="SalGrp" EnableClientScript="true" runat="server" ControlToValidate="txtGroupName"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_GroupName %>"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <asp:Label ID="lbnGroupSequence" runat="server" Text="<%$resources:SequenceStar %>"
                                    AssociatedControlID="txtGroupSequence" CssClass="lbl-34perc margn-rgt-0"></asp:Label>
                                <asp:TextBox ID="txtGroupSequence" runat="server" MaxLength="5" TabIndex="26" onkeyup="limitText(this,5);"
                                    CssClass="margn-rgt-0 margnlft5-7per input-small-e" onkeydown="limitText(this,5);"
                                    onkeypress="return validateFloatKeyPress(this,event,0);" onDrag="return false;"
                                    onDrop="return false;" onPaste="return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrftxtGroupSequence" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="SalGrp" EnableClientScript="true" runat="server" ControlToValidate="txtGroupSequence"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Sequence %>"></asp:RequiredFieldValidator>
                            </tr>
                        </table>
                    </div>
                    <div id="divAddButton">
                        <asp:Label ID="lbldummy" runat="server" AssociatedControlID="btnSaveGroup" CssClass="lbl-34perc margn-rgt0"></asp:Label>
                        <asp:Button ID="btnSaveGroup" runat="server" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('SalGrp')"
                            ValidationGroup="SalGrp" CommandName="SAVEGROUP" Text="<%$resources:Controls,Save %>"
                            CssClass="margnlft68-5per" ToolTip="<%$resources:Controls,Save %>" TabIndex="27" />
                    </div>
                </div>
            </div>
            <div id="divPayElement_PopUp" style="display: none">
                <div class="content-wrapper">
                    <div id="div2">
                        <table class="table-devide">
                            <tr>
                                <asp:Label ID="lblPayEltName" runat="server" Text="<%$resources:PayEltName %>" AssociatedControlID="txtGroupName"
                                    CssClass="lbl-28perc margn-rgt-0"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlPayElement" OnSelectedIndexChanged="ActionHandler"
                                    AutoPostBack="true" EnableViewState="true" CssClass="select-half">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vrfPayElement" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="SalItemGrp" EnableClientScript="true" runat="server" ControlToValidate="ddlPayElement"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayElement %>"></asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <asp:Label ID="lblPayEltMdfyName" runat="server" Text="<%$resources:PayElement %>"
                                    AssociatedControlID="txtPayEltMdfyName" CssClass="lbl-28perc margn-rgt-0"></asp:Label>
                                <asp:TextBox ID="txtPayEltMdfyName" runat="server" TabIndex="26" CssClass="margn-rgt-0 margnlft5-7per input-w58-4per"></asp:TextBox>
                            </tr>
                        </table>
                    </div>
                    <div id="div3">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="btnAddGrid" CssClass="lbl-28perc margn-rgt0"></asp:Label>
                        <asp:Button ID="btnAddGrid" runat="server" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('boqGroup')"
                            ValidationGroup="SalItemGrp" CommandName="ADDITEM" Text="<%$resources:Controls,Save %>"
                            CssClass="margnlft68-5per" ToolTip="<%$resources:Controls,Save %>" TabIndex="27" />
                    </div>
                </div>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsSalRptGroup" ValidationGroup="SalGrp" runat="server" />
                <asp:ValidationSummary ID="vsSalRptItemGroup" ValidationGroup="SalItemGrp" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
