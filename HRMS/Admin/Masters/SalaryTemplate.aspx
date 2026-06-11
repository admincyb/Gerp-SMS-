<%@ Page Title="<%$ Resources:Captions,Title_SalaryTemplate %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="SalaryTemplate.aspx.cs" Inherits="HRMS.Admin.Masters.SalaryTemplate"
    Theme="ClassicExt" %>

<%--<%@ Register Src="UserControls/FormulaMaster.ascx" TagName="PopUp" TagPrefix="uc1" %>--%>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PayItem.ascx" TagName="PayItemControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
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
            $(document).ready(function () {
                //                $("[id$=txtAmountOrFormula]").focus(function () {
                //                    if ($("[id$=rbtFormula]").is(':checked')) {
                //                        $("[id$=btnPopUp]").click();
                //                    }
                //                });
                ShowHideEarnings(1);
                ShowHideDeductions(1);
            });
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

        function ShowHideEarnings(flag) {
            if (flag == 1) {
                $("[id$=divEarnings]").show();
                $("[id$=imbShowEarnings]").hide();
                $("[id$=imbHideEarnings]").show();
            }
            else {
                $("[id$=divEarnings]").hide();
                $("[id$=imbShowEarnings]").show();
                $("[id$=imbHideEarnings]").hide();
            }
            return false;
        }

        function ShowHideDeductions(flag) {
            if (flag == 1) {
                $("[id$=divDeductions]").show();
                $("[id$=imbShowDeductions]").hide();
                $("[id$=imbHideDeductions]").show();
            }
            else {
                $("[id$=divDeductions]").hide();
                $("[id$=imbShowDeductions]").show();
                $("[id$=imbHideDeductions]").hide();
            }
            return false;
        }


        $("[id*=chkPayelementHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    if ($(this).is(':disabled') == false) {
                        $(this).removeAttr("checked");
                        $("td", $(this).closest("tr")).removeClass("selected");
                    }
                }


            });
        });

        $("[id*=chkEmpPayelement]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkPayelementHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkEmpPayelement]", grid).length == $("[id*=chkEmpPayelement]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });      
                
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
                                        <asp:Button runat="server" ID="Button1" CommandName="SAVE" TabIndex="19" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="19" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="19" SkinID="btnInner-Cancel"
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
                                CommandArgument="SEC_ActionPanel" TabIndex="5" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="6" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <%--Listing Page Table Row--%>
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="7" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="7" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide tablelayout" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterCode" runat="server" Text="<%$ resources:TemplateCode%>"
                                                AssociatedControlID="txtFilterCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterCode" TabIndex="8" CssClass="input-small-a margnbotm0"></asp:TextBox>
                                            <asp:Label ID="label6" runat="server" Text="<%$ resources:TemplateName%>" AssociatedControlID="txtTemplateNameListPage"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateNameListPage" TabIndex="8" CssClass="input-medium margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSrchPayrollType" runat="server" Text="<%$ resources:PayrollTypeH%>"
                                                AssociatedControlID="ddlSrchPayrollType" CssClass="middle-lbl"></asp:Label>
                                            <asp:DropDownList ID="ddlSrchPayrollType" runat="server" TabIndex="9" CssClass="select-w39-4per margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="9"
                                                CommandName="FILTER" SkinID="search-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="10" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="11" />
                                                <asp:HiddenField runat="server" ID="hdfTemplatePkListPage" Value='<%# Eval("STE_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfListSTE_MOD_DT" Value='<%# Eval("STE_MOD_DT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateCode%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("STE_CODE")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("STE_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TemplateName%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("STE_NAME"),45) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("STE_NAME"), 100) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                            <HeaderStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PayrollTypeH%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalPayrollType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("STE_PAYROLL_TYPE_TEXT"),20) %>'
                                                    ToolTip='<%#Eval("STE_PAYROLL_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("STE_DESC")),85) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("STE_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30%" />
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("STE_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="12" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("STE_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="12" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Status%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemActive" runat="server" Text='<%# Eval("STE_ACTIVE")!=null?Eval("STE_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString():GetLocalResourceObject("Inactive").ToString():string.Empty %>'
                                                    ToolTip='<%# Eval("STE_ACTIVE")!=null?Eval("STE_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString():GetLocalResourceObject("Inactive").ToString():string.Empty %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide tablelayout">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="label1" runat="server" Text="<%$ resources:TemplateCodeStar%>" AssociatedControlID="txtTemplateCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateCode" TabIndex="8" CssClass="input-half"
                                                MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTemplateCode" runat="server" ControlToValidate="txtTemplateCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTemplateCode%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="label2" Text="<%$ resources:TemplateNameStar%>" AssociatedControlID="txtTemplateName"
                                                class=""></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTemplateName" TabIndex="9" CssClass="input-half"
                                                MaxLength="180"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvTemplateName" runat="server" ControlToValidate="txtTemplateName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterTemplateName%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="9" TextMode="MultiLine"
                                                onkeypress="return this.value.length<490" onpaste="return this.value.length<490"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPayrollType" runat="server" Text="<%$ resources:PayrollType%>"
                                                AssociatedControlID="ddlPayrollType"></asp:Label>
                                            <asp:DropDownList ID="ddlPayrollType" runat="server" TabIndex="9" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPayrollType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlPayrollType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PayrollType %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="9" />
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
                            <uc1:PayItemControl ID="ucPayItemControl1" runat="server" />
                            <div class="gridwrap">
                                <div class="gridwrap floatLeft" style="width: 49%;">
                                    <div class="search-colapse-b">
                                        <h1>
                                            <asp:Literal ID="Literal3" runat="server" Text="<%$ resources:Earnings%>" /></h1>
                                        <asp:ImageButton runat="server" ID="imbShowEarnings" OnClientClick="javascript:return ShowHideEarnings(1);"
                                            SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="13" />
                                        <asp:ImageButton runat="server" ID="imbHideEarnings" OnClientClick="javascript:return ShowHideEarnings();"
                                            Style="display: none" SkinID="imbArrowHide" TabIndex="14" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div id="divEarnings">
                                        <asp:GridView runat="server" ID="grdEarning" Width="100%" AllowPaging="false" AllowSorting="True"
                                            AutoGenerateColumns="false" OnRowCommand="ActionHandler" EmptyDataRowStyle-HorizontalAlign="Center"
                                            EmptyDataRowStyle-CssClass="emptytable" TabIndex="15" OnDataBound="GridView_DataBound"
                                            OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleUpEarn" SkinID="move-up" runat="server"
                                                            OnClick="ActionHandler" CommandName="MOVEUP" ToolTip="Move Up" CommandArgument='<%# Eval("STS_SL_NO") %>'
                                                            TabIndex="15"></asp:ImageButton>
                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleDownEarn" SkinID="move-down" runat="server"
                                                            OnClick="ActionHandler" CommandName="MOVEDOWN" ToolTip="Move Down" CommandArgument='<%# Eval("STS_SL_NO") %>'
                                                            TabIndex="15"></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="2%" />
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PayElement%> " SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayElement" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementName")),28) %>'
                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PayElementName"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfSlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                        <asp:HiddenField ID="hdfPk" runat="server" Value='<%# Eval("Pk") %>' />
                                                        <asp:HiddenField ID="hdfPayElementPk" runat="server" Value='<%# Eval("PayElementPk") %>' />
                                                        <asp:HiddenField ID="hdfEarnCalcMode" runat="server" Value='<%#Eval("PayCalculationMode")%>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="38%" />
                                                    <ItemStyle Width="38%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%--<asp:Image CssClass="hide" ID="imgEarnMode" runat="server" AlternateText=" "></asp:Image>--%>
                                                        <div id="divEarnMode" runat="server" class="hide">
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="3%" CssClass="padgrgt3" />
                                                    <ItemStyle Width="3%" CssClass="padgrgt3" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayElementValue" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementDisplay")),40) %>'
                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PayElementDisplay")))  + ((Convert.ToDecimal(Eval("MinAmount")) > 0 || Convert.ToDecimal(Eval("MaxAmount")) > 0)? ("(" + Resources.Controls.Min.ToString() + GetFormattedCurrencyWithComma(Eval("MinAmount")) +", "+ Resources.Controls.Max.ToString() + GetFormattedCurrencyWithComma(Eval("MaxAmount"))+")") : "") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50%" />
                                                    <ItemStyle Width="50%" CssClass="wordwrap padglft0" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                            SkinID="imbeditgrid" EnableViewState="false" CommandName="EDIT_ACTION" ToolTip="<%$ resources:Controls,Edit%>"
                                                            TabIndex="16" />
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                            ToolTip="<%$ resources:Controls,Delete%>" SkinID="imbdeletegrid" EnableViewState="false"
                                                            CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="16" />
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbUpdate"
                                                            ToolTip="<%$ resources:UpdatePayElement%>" SkinID="imbUpdate" CommandName="UPDATE_ACTION"
                                                            TabIndex="16" Visible='<%# (Convert.ToInt32(Eval("Pk")) > 0 ? (Convert.ToInt32(Eval("IsEdited")) > 0 ? false : true) : false) %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="8%" />
                                                    <ItemStyle Width="8%" Wrap="false" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <%--<div style="width: 2%;"></div>--%>
                                <div class="gridwrap floatRight" style="width: 49%;">
                                    <div class="search-colapse-b">
                                        <h1>
                                            <asp:Literal ID="Literal4" runat="server" Text="<%$ resources:Deductions%>" /></h1>
                                        <asp:ImageButton runat="server" ID="imbShowDeductions" OnClientClick="javascript:return ShowHideDeductions(1);"
                                            SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" TabIndex="13" />
                                        <asp:ImageButton runat="server" ID="imbHideDeductions" OnClientClick="javascript:return ShowHideDeductions();"
                                            Style="display: none" SkinID="imbArrowHide" TabIndex="14" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div id="divDeductions">
                                        <asp:GridView runat="server" ID="grdDeduction" Width="100%" AllowPaging="false" AllowSorting="True"
                                            AutoGenerateColumns="false" OnRowCommand="ActionHandler" EmptyDataRowStyle-HorizontalAlign="Center"
                                            EmptyDataRowStyle-CssClass="emptytable" TabIndex="17" OnDataBound="GridView_DataBound"
                                            OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleUpDedu" SkinID="move-up" runat="server"
                                                            OnClick="ActionHandler" CommandName="MOVEUP" ToolTip="Move Up" CommandArgument='<%# Eval("STS_SL_NO") %>'
                                                            TabIndex="17"></asp:ImageButton>
                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleDownDedu" SkinID="move-down" runat="server"
                                                            OnClick="ActionHandler" CommandName="MOVEDOWN" ToolTip="Move Down" CommandArgument='<%# Eval("STS_SL_NO") %>'
                                                            TabIndex="17"></asp:ImageButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                    <HeaderStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PayElement%>" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayElement" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementName")),28) %>'
                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PayElementName")))%>'></asp:Label>
                                                        <asp:HiddenField ID="hdfSlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                        <asp:HiddenField ID="hdfPk" runat="server" Value='<%# Eval("Pk") %>' />
                                                        <asp:HiddenField ID="hdfPayElementPk" runat="server" Value='<%# Eval("PayElementPk") %>' />
                                                        <asp:HiddenField ID="hdfDeductCalcMode" runat="server" Value='<%#Eval("PayCalculationMode")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="38%" />
                                                    <HeaderStyle Width="38%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div id="divDeductMode" runat="server" class="hide">
                                                        </div>
                                                        <%--<asp:Image CssClass="hide" ID="imgDeductMode" runat="server" AlternateText=" "></asp:Image>--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="3%" CssClass="padgrgt3" />
                                                    <ItemStyle Width="3%" CssClass="padgrgt3" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPayElementValue" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("PayElementDisplay")),40) %>'
                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("PayElementDisplay")))  + ((Convert.ToDecimal(Eval("MinAmount")) > 0 || Convert.ToDecimal(Eval("MaxAmount")) > 0)? ("(" + Resources.Controls.Min.ToString() + GetFormattedCurrencyWithComma(Eval("MinAmount")) +", "+ Resources.Controls.Max.ToString() + GetFormattedCurrencyWithComma(Eval("MaxAmount"))+")") : "") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" CssClass="wordwrap padglft0" />
                                                    <HeaderStyle Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                            SkinID="imbeditgrid" EnableViewState="false" CommandName="EDIT_ACTION" ToolTip="<%$ resources:Controls,Edit%>"
                                                            TabIndex="18" />
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                            ToolTip="<%$ resources:Controls,Delete%>" SkinID="imbdeletegrid" EnableViewState="false"
                                                            CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);" TabIndex="18" />
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbUpdate"
                                                            ToolTip="<%$ resources:UpdatePayElement%>" SkinID="imbUpdate" CommandName="UPDATE_ACTION"
                                                            TabIndex="18" Visible='<%# (Convert.ToInt32(Eval("Pk")) > 0 ? (Convert.ToInt32(Eval("IsEdited")) > 0 ? false : true) : false) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" Wrap="false" HorizontalAlign="Right" />
                                                    <HeaderStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
                <%-------------------pay element update popup start------------------------%>
                <div id="divPopupUpdateElement" style="display: none;">
                    <div class="Button-container-popup margnbotm0">
                        <asp:Panel runat="server" ID="pnlBranchDet" CssClass="Button-container-popup">
                            <asp:Button runat="server" ID="btnApplyPopup" SkinID="btnInner-add-dsd" Text="<%$ resources:Controls,Apply %>"
                                ToolTip="<%$resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="APPLY"
                                Style="margin-right: 1%!important;" TabIndex="3" />
                            <asp:Button runat="server" ID="btnDeletePopup" SkinID="btnInner-Delete" Text="<%$ resources:Controls,Delete %>"
                                ToolTip="<%$resources:Controls,Delete %>" OnClick="ActionHandler" CommandName="DELETEEMPPAYELEMENT"
                                Style="margin-right: 1%!important;" TabIndex="3" OnClientClick="return ShowDeleteConfirm(this);" />
                        </asp:Panel>
                    </div>
                    <div class="content-wrapper">
                        <div class="detail-poi-co1">
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="lblModifiedPayElementName"
                                Font-Bold="true" CssClass="lbl-15-1perc margnbotm0" Text="<%$ resources:PayElement1 %>"></asp:Label>
                            <asp:Label ID="lblModifiedPayElementName" runat="server"></asp:Label>
                            <div class="clear">
                            </div>
                            <asp:Label ID="Label4" runat="server" AssociatedControlID="lblModifiedPayElementValue"
                                Font-Bold="true" CssClass="lbl-15-1perc margnbotm3" Text="<%$ resources:Value1 %>"></asp:Label>
                            <asp:Label ID="lblModifiedPayElementValue" runat="server"></asp:Label>
                            <asp:HiddenField ID="hdfModifiedPayElementValue" runat="server" />
                            <div class="clear">
                            </div>
                        </div>
                        <div class="gridwrap">
                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:InsertNew %>"></asp:Label>
                            <asp:CheckBox runat="server" ID="chkInsertNew" TabIndex="16" />
                            <asp:GridView runat="server" ID="grdEmpPaylement" Width="100%" AllowPaging="false"
                                AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                TabIndex="17">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkPayelementHeader" runat="server" TabIndex="15" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkEmpPayelement" TabIndex="16" />
                                            <asp:HiddenField ID="hdfEmpPayElementValue" runat="server" Value='<%# Eval("STS_CALC_VALUE")%>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Controls,PayElement %>" SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpPayElement" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("STS_CALC_VALUE_TEXT")),90) %>'
                                                ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("STS_CALC_VALUE_TEXT")))%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="98%" />
                                        <HeaderStyle Width="98%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%-------------------------------pay element update popup start---------------------------%>
            </div>
            <%-- <div id="divPopUpFormula" style="display: none">
               <%-- <uc1:PopUp ID="ucFormulaMaster" runat="server" AfterApply="ucPopUpFormula_AfterApply" />
            </div>--%>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <asp:HiddenField ID="hdfSalTempCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfSalTempCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfSalTempDecimalDigits" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
