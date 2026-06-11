<%@ Page Title="<%$ Resources:Captions,Title_LoansAndAdvances %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="LoansAndAdvances.aspx.cs" Inherits="HRMS.Payroll.LoansAndAdvances"
    ValidateRequest="false" Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?EmpCategory=2&ToDate=" + $("[id$=hdfNextYearDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployeeSrch", url + "?EmpCategory=2", "hdfEmployeeSrch", true, true, "EMPLOYEEAUTOCOMPLETE");
            GrandScriptUtils.AddDateRangeCommon("txtApplyDate", "hdfApplyDate", "txtApprovedDate", "hdfApprovedDate", false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            GrandScriptUtils.DatePickerCommon("txtApprovedDate");
            GrandScriptUtils.DatePickerCommon("txtEffectiveDate");
            GrandScriptUtils.DatePickerCommon("txtInstallmentDate");
            $("[id*=txtInstallments]").ForceNumericOnly();
            $("[id*=txtRateofInterest]").ForceNumericOnly();
            $("[id*=txtInstallmentAmt]").ForceNumericOnly();
            $("[id*=txtPrincipalAmount]").ForceNumericOnly();
            $("[id*=txtInstAmt]").ForceNumericOnly();
            $("[id*=txtExchangeRate]").ForceNumericOnly();

            ShowHideInstallmentDetails(1);

            if ($("[id$=txtExchangeRate]").attr("disabled") == true) {
                $("[id$=txtExchangeRate]").addClass("input-disabled");
            }
            else {
                $("[id$=txtExchangeRate]").removeClass("input-disabled");
            }

            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            else {
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            var Empid = parseInt($("[id$=hdfEmployee]").val());
            if (Empid > 0) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            //            ShowHideInstallmentDetails($("[id$=hdfInstallmentVisible]").val());
        }

        //        function AfterDateSelect(controlID) {
        //            if (typeof AfterAlertControlDateSelect == "function") {
        //                AfterAlertControlDateSelect(controlID);
        //            }
        //        }

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

        function ValidateNow(valGroup) {
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

        function ShowHideInstallmentDetails(flag) {
            ///<summary>
            /// Used to Show/Hide HideInstallmentDetails Div
            ///</summary>

            //If flag then Show HideInstallmentDetails
            if (flag == 1) {
                $("[id$=divInstallmentDetails]").show();
                $("[id$=imbShowInstallmentDetails]").hide();
                $("[id$=imbHideInstallmentDetails]").show();
            }
            else {
                $("[id$=divInstallmentDetails]").hide();
                $("[id$=imbShowInstallmentDetails]").show();
                $("[id$=imbHideInstallmentDetails]").hide();
            }
            //            $("[id$=hdfInstallmentVisible]").val(flag);
            return false;
        }

        function CalculateEMI_Details(ctrl) {
            var TotalAmount = 0;
            var TotalBalAmount = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            $("#[id*=grdLoanSlabDetails] input[type=text][id*=txtInstAmt]").each(function (index) {
                var amount = 0;
                var BalanceAmt = 0;
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        amount = parseFloat($(this).val());
                        TotalAmount = TotalAmount + amount;

                        var row = $(this).closest("tr");
                        BalanceAmt = parseFloat($(this).val()) - parseFloat($("[id*=lblPaidAmt]", row).html());
                        $("[id*=lblBalanceAmt]", row).html(BalanceAmt.toFixed(DecimalDigits))
                    }
                }
            });
            $("#[id*=grdLoanSlabDetails] [id*=lblFooterTotalInstAmt]").html(TotalAmount.toFixed(DecimalDigits));

            $("[id*=lblBalanceAmt]").each(function () {
                var amount = 0;
                //Check if number is not empty
                if ($.trim($(this).html()) != "") {
                    //Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).html()))) {
                        amount = parseFloat($(this).html());
                        TotalBalAmount = TotalBalAmount + amount;
                    }
                }
            });
            $("#[id*=grdLoanSlabDetails] [id*=lblFooterTotalBalanceAmt]").html(TotalBalAmount.toFixed(DecimalDigits));
        }


        /// Used to disable Autocomplete
        function Disableautocomplete() {
            if ($("[id$=txtEmployee]").attr("disabled") == true) {
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            }
        }
        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
            if (targetControlID == "txtEmployee") {
                $("[id$=btn_EmpChange]").click();
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtApplyDate") {
                $("[id$=btnCurrency]").click();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            var defaultText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            if (targetControlID == "txtEmployee") {
                $("[id$=hdfCurrency]").val("0");
                $("[id$=txtCurrency]").val(defaultText);
                EnableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
        }

        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        /// Used to disable Autocomplete
        function EnableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
            $(extender).attr("disabled", false);
        }

        function CheckIsAdvance(ctrl) {
            if (parseFloat($("#[id*=hdfIsAdvances]").val()) == 1) {
                $("[id$=btnShow]").click();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upLoansAndAdvances" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
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
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);"
                                            TabIndex="151" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="152" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="154" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
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
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Employee%>" AssociatedControlID="txtEmployeeSrch"></asp:Label>
                                            <asp:TextBox ID="txtEmployeeSrch" runat="server" CssClass="select-w26-6per" MaxLength="200"
                                                onkeydown="limitText(this,200);" onkeyup="limitText(this,200);" TabIndex="3"></asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployeeSrch" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblEmploymentType" runat="server" Text="<%$ resources:EmploymentType%>"
                                                AssociatedControlID="ddlEmploymentType"></asp:Label>
                                            <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="3" CssClass="select-medium-27-5 margnbotm0">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblBranchLocation" runat="server" Text="<%$ resources:BranchLocation%>"
                                                AssociatedControlID="ddlBranchLocation" CssClass="lbl-17perc"></asp:Label>
                                            <asp:DropDownList ID="ddlBranchLocation" runat="server" TabIndex="3" CssClass="select-medium-27-5 margnbotm0">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterItem" runat="server" Text="<%$ resources:Item%>" AssociatedControlID="ddlFilterItem"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterItem" runat="server" TabIndex="3" CssClass="select-medium-27-5 margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="4" CommandName="SEARCH" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" ValidationGroup="Search" OnClientClick="javascript:ValidateNow('Search')" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--<table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            
                                        </div>
                                    </td>
                                </tr>
                            </table>--%>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpLoanList" Width="100%" AllowPaging="false"
                                    PageSize="<%$ resources:PageSize%>" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="6" />
                                                <asp:HiddenField runat="server" ID="hdfEmpLoanPK" Value='<%# Eval("ELM_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ELM_NO")))?Resources.ErpRes.Draft:Eval("ELM_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ELM_NO")))?Resources.ErpRes.Draft:Eval("ELM_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELM_EMPLOYEE_TEXT")),30) %>'
                                                    ToolTip='<%# Eval("ELM_EMPLOYEE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTypeText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELM_PAY_ELEMENT_TEXT")),22) %>'
                                                    ToolTip='<%# Eval("ELM_PAY_ELEMENT_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdEffdate" runat="server" Text='<%# Eval("ELM_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("ELM_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Installment%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstCount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ELM_INST_COUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ELM_INST_COUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="8%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrencyList" runat="server" Text='<%#(Eval("ELM_CURRENCY_CODE_TEXT")) %>'
                                                    ToolTip='<%# (Eval("ELM_CURRENCY_CODE_TEXT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmt%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("ELM_TOTAL_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ELM_TOTAL_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Paid%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdPaid" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ELM_PAID_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ELM_PAID_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Balance%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdBalance" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDecimal(Eval("ELM_TOTAL_AMT")) - Convert.ToDecimal(Eval("ELM_PAID_AMT")))%>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDecimal(Eval("ELM_TOTAL_AMT")) - Convert.ToDecimal(Eval("ELM_PAID_AMT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:ApplyDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApplydate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ELM_APPLY_DATE", Resources.Constants.HRMSDateFormatGrid),12)%>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ELM_APPLY_DATE", Resources.Constants.HRMSDateFormatGrid),12)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ApprovedDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApprovedate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ELM_APPROVED_DATE", Resources.Constants.HRMSDateFormatGrid),12)%>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ELM_APPROVED_DATE", Resources.Constants.HRMSDateFormatGrid),12)%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <%--   <asp:TemplateField HeaderText="<%$ resources:InstallmentAmt%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstAmount" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ELM_INST_AMT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ELM_INST_AMT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="14%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Interest%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblROI" runat="server" Text='<%# GetFormattedCurrencyWithComma(Eval("ELM_ROI")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithComma(Eval("ELM_ROI")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="10%" CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblEmpLoanHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <asp:TextBox ID="txtEmployee" runat="server" TabIndex="1" CssClass="input-half"
                                                MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                            <asp:RequiredFieldValidator ID="rfvEmployee" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtEmployee" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Employee %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:ImageButton SkinID="btnview" runat="server" ID="imbHistoryDetailsPopup" CommandName="DETAILS"
                                                CssClass="btnInner-View" OnClick="ActionHandler" Style="margin-top: 1px; margin-left: -12px;
                                                margin-right: 0px; cursor: default" TabIndex="1" ToolTip="<%$ resources:HistoryDeatils%>" /></div>
                                        <asp:Button ID="btn_EmpChange" runat="server" OnClick="ActionHandler" CommandName="CHANGEEMPLOYEE"
                                            Style="display: none" EnableTheming="false" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo" CssClass="lbl-20perc"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls,Company%>"
                                                AssociatedControlID="ddlCompany" CssClass="middle-lbl-a0"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="2" CssClass="select-small-c1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Static" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_Company %>">
                                            </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblItem" Text="<%$ resources:ItemStar%>" AssociatedControlID="ddlItem"></asp:Label>
                                            <asp:DropDownList ID="ddlItem" runat="server" TabIndex="3" CssClass="select-w61per" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvItem" CssClass="star" SetFocusOnError="true" ValidationGroup="Save"
                                                EnableClientScript="true" InitialValue="-1" runat="server" ControlToValidate="ddlItem"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Item %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblItemType" Text="<%$ resources:ItemTypeStar%>" CssClass="middle-lbl-small-b"
                                                AssociatedControlID="ddlItemType" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlItemType" runat="server" TabIndex="4" CssClass="lbl-65-9perc"
                                                Visible="false">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <div id="divHistoryDetails" style="display: none">
                                                <asp:HiddenField ID="hdfStatusPk" runat="server" />
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div class="content-wrapper" runat="server" id="div1">
                                                                <div class="detail-poi-co1">
                                                                    <div class="popup-headr">
                                                                        <asp:Label ID="lblHistoryEmployee" runat="server" Text="<%$ resources:Employee:%>"
                                                                            Font-Bold="True" AssociatedControlID="lblhdrEmployeeNoTxtPopup"></asp:Label>
                                                                        <asp:Label ID="lblhdrEmployeeNoTxtPopup" runat="server" Text="emptext" Font-Bold="True"></asp:Label>
                                                                    </div>
                                                                </div>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="content-wrapper">
                                                    <asp:GridView runat="server" ID="grdStatusHistory" Width="100%" AutoGenerateColumns="false"
                                                        Style="margin-top: -13px!important;" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Item%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" Text='<%# Eval("ELM_PAY_ELEMENT_TEXT") %>' runat="server"
                                                                        ToolTip='<%# Eval("ELM_PAY_ELEMENT_TEXT") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="35%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:EffectiveDate%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEffectiveDate" Text='<%# Eval("ELM_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                                        runat="server" ToolTip='<%# Eval("ELM_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:LastInstallmntDate%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLastInstallmntDate" Text='<%# Eval("ELM_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                                        runat="server" ToolTip='<%# Eval("ELM_DATE", Resources.Constants.HRMSDateFormatGrid) %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:TotalAmt%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmt" Text='<%#  GetFormattedCurrencyWithComma (Eval("TOTAL_AMOUNT"))  %>'
                                                                        runat="server" ToolTip='<%# GetFormattedCurrencyWithComma (Eval("TOTAL_AMOUNT")) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Paid%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHistoryPaidAmt" Text='<%#  GetFormattedCurrencyWithComma (Eval("ELM_PAID_AMT"))  %>'
                                                                        runat="server" ToolTip='<%# GetFormattedCurrencyWithComma (Eval("ELM_PAID_AMT")) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Balance%>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHistoryBalanceAmt" runat="server" Text='<%# GetFormattedCurrencyWithComma(Convert.ToDecimal(Eval("TOTAL_AMOUNT")) - Convert.ToDecimal(Eval("ELM_PAID_AMT")))%>'
                                                                        ToolTip='<%# GetFormattedCurrencyWithComma(Convert.ToDecimal(Eval("TOTAL_AMOUNT")) - Convert.ToDecimal(Eval("ELM_PAID_AMT")))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblApplyDate" Text="<%$ resources:ApplyDateStar%>"
                                                AssociatedControlID="txtApplyDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtApplyDate" CssClass="input-small" TabIndex="5"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfApplyDate" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="rfvApplyDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtApplyDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ApplyDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblApprovedDate" Text="<%$ resources:ApprovedDateStar%>"
                                                AssociatedControlID="txtApprovedDate" CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtApprovedDate" CssClass="input-small" TabIndex="6"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfApprovedDate" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="rfvApprovedDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtApprovedDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ApprovedDate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblInstallments" Text="<%$ resources:InstallmentsStar%>"
                                                AssociatedControlID="txtInstallments"></asp:Label>
                                            <asp:TextBox ID="txtInstallments" runat="server" CssClass="input-small numeric" TabIndex="9"
                                                MaxLength="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInstallments" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtInstallments"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_InstallmentCount %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revInstallments" runat="server" ControlToValidate="txtInstallments"
                                                ErrorMessage="<%$ resources:Err_Valid_InstCount %>" ValidationExpression="^\$?([0-9]{0,10})?$"
                                                Display="Static" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save">
                                            </asp:RegularExpressionValidator>
                                            <asp:Label runat="server" ID="lblInstallmentAmt" Text="<%$ resources:InstallmentAmt%>"
                                                AssociatedControlID="txtInstallmentAmt" CssClass="lbl-17-7perc"></asp:Label>
                                            <asp:TextBox ID="txtInstallmentAmt" runat="server" CssClass="lbl-18-3perc numeric"
                                                TabIndex="10" MaxLength="16"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator ID="rfvInstallmentAmt" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtInstallmentAmt"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InstAmount %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <cc1:AmountValidation ID="vamInstallmentAmt" runat="server" ControlToValidate="txtInstallmentAmt"
                                                ErrorMessage="<%$ resources:Err_Invalid_InstAmount %>" NumberDigits="11" Display="Dynamic"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></cc1:AmountValidation>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblhdrCurrency" Text="<%$ resources:CurrencyReq%>"
                                                AssociatedControlID="txtCurrency" CssClass="lbl-20perc"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="3"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRateReq%>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl-xsmall"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="3" CssClass="select-w24-7per numeric medium"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="Save"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblEffectiveDate" Text="<%$ resources:EffectiveDateStar%>"
                                                AssociatedControlID="txtEffectiveDate" CssClass="lbl-20perc"></asp:Label>
                                            <asp:HiddenField ID="hdfEffectiveDate" runat="server" Value="" />
                                            <asp:TextBox runat="server" ID="txtEffectiveDate" CssClass="date-picker input-small"
                                                TabIndex="7" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEffectiveDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtEffectiveDate"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_EffectiveDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblPrincipalAmount" Text="<%$ resources:PrincipalAmountStar%>"
                                                AssociatedControlID="txtPrincipalAmount" CssClass="lbl-18-3perc"   ></asp:Label>
                                            <asp:TextBox ID="txtPrincipalAmount" runat="server" CssClass="select-w24-7per numeric" onChange="CheckIsAdvance(this);"
                                                TabIndex="8" MaxLength="16"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvAmount" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtPrincipalAmount"
                                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                            </asp:RequiredFieldValidator>
                                            <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtPrincipalAmount"
                                                ErrorMessage="<%$ resources:Err_Invalid_Amount %>" NumberDigits="11" Display="Static"
                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></cc1:AmountValidation>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblRateofInterest" Text="<%$ resources:RateofInterest%>"
                                                AssociatedControlID="txtRateofInterest" CssClass="lbl-20perc"></asp:Label>
                                            <asp:TextBox ID="txtRateofInterest" runat="server" CssClass="lbl-18perc numeric"
                                                TabIndex="11" MaxLength="16" AutoPostBack="true" OnTextChanged="ActionHandler"></asp:TextBox>
                                            <%--   <asp:Label runat="server" ID="lblInterestAmount" Text="<%$ resources:InterestAmt%>"
                                                AssociatedControlID="txtInterestAmt" CssClass="middle-lbl-small-g"></asp:Label>
                                            <asp:TextBox runat="server" CssClass="input-small numeric input-disabled" TabIndex="27" ID="txtInterestAmt"
                                                MaxLength="16" Enabled="false"></asp:TextBox>--%>
                                            <asp:Label runat="server" ID="lblTotalAmt" Text="<%$ resources:TotalAmt%>" AssociatedControlID="txtTotalAmt"
                                                CssClass="lbl-20-1perc"></asp:Label>
                                            <asp:TextBox runat="server" CssClass="select-w24-7per input-disabled numeric" TabIndex="12"
                                                ID="txtTotalAmt" MaxLength="16" Enabled="false"></asp:TextBox>
                                            <%--<asp:Label ID="lblInterestAmt" runat="server" CssClass="input-small" TabIndex="27"
                                                MaxLength="16"></asp:Label>--%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDescription" Text="<%$ resources:Description%>"
                                                CssClass="lbl-12-4perc" AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TabIndex="13" MaxLength="500" TextMode="MultiLine"
                                                Height="40" CssClass="input-full"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="button-wrap-right">
                                <asp:Button runat="server" ID="btnShow" CommandName="CALCULATEINTERESTAMT" TabIndex="13"
                                    ValidationGroup="Save" CssClass="BTNenable-submit" Text="<%$resources:Controls,ShowInstallments %>"
                                    OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')" ToolTip="<%$resources:Controls,ShowInstallments %>" />
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("InstallmentDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowInstallmentDetails" OnClientClick="javascript:return ShowHideInstallmentDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:ShowInstallmentDetails %>" />
                                <asp:ImageButton runat="server" ID="imbHideInstallmentDetails" OnClientClick="javascript:return ShowHideInstallmentDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:HideInstallmentDetails %>" />
                                <%--<asp:HiddenField ID="hdfInstallmentVisible" runat="server" Value="0" />--%>
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divInstallmentDetails">
                                <div class="gridwrap">
                                    <asp:GridView ID="grdLoanSlabDetails" runat="server" AutoGenerateColumns="False"
                                        OnRowDataBound="ActionHandler" Width="100%" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                        AllowSorting="false" ShowFooter="true">
                                        <%--OnRowDataBound="ActionHandler"--%>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Installment %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%#Container.DisplayIndex+1%>' />
                                                    <asp:HiddenField ID="hdfELS_PK" runat="server" Value='<%#Eval("ELS_PK")%>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalInstAmt" Text="<%$ resources:Total %>"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtInstallmentDate" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("ELS_INST_DATE", Resources.Constants.HRMSDateFormatShortGrid),12)%>'
                                                        onkeydown="return CheckKey(event)" MaxLength="11" TabIndex="14" onpaste="return false;"></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                                <HeaderStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:InstallmentAmt %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInstAmt" runat="server" Text='<%#GetFormattedCurrency(Eval("ELS_INST_AMT")) %>'
                                                        CssClass=" numeric" onkeyup="CalculateEMI_Details(this);" TabIndex="14"></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblFooterTotalInstAmt"></asp:Label>
                                                </FooterTemplate>
                                                <HeaderStyle Width="20%" CssClass="amount-numeric" />
                                                <FooterStyle Width="20%" CssClass="amount-numeric" />
                                                <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Paid %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPaidAmt" runat="server" Text='<%#GetFormattedCurrency(Eval("ELS_PAID_AMT")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblFooterTotalPaidAmt"></asp:Label>
                                                </FooterTemplate>
                                                <HeaderStyle Width="20%" CssClass="amount-numeric" />
                                                <FooterStyle Width="20%" CssClass="amount-numeric" />
                                                <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalanceAmt" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("ELS_INST_AMT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" CssClass=" numeric" ID="lblFooterTotalBalanceAmt"></asp:Label>
                                                </FooterTemplate>
                                                <HeaderStyle Width="20%" CssClass="amount-numeric" />
                                                <FooterStyle Width="20%" CssClass="amount-numeric" />
                                                <ItemStyle Width="20%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                        ToolTip="<%$ resources:Controls,Delete %>" SkinID="imbdeletegrid" TabIndex="14"
                                                        OnClick="ActionHandler" CommandName="DELETE_ACTION" OnClientClick="return ShowDeleteConfirm(this);" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="3%" />
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsSearch" ValidationGroup="Search" runat="server" />
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfIsAdvances" runat="server" Value="0" />
            <asp:HiddenField ID="hdfNextYearDate" runat=server Value="" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
