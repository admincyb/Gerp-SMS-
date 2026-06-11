<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Title="<%$ Resources:Captions,Title_BonusEntry %>" CodeBehind="BonusEntry.aspx.cs"
    Inherits="HRMS.Payroll.BonusEntry" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtListFromDate", "hdfListFromDate", "txtListToDate", "hdfListToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtreligion", url, "hdfReligion", true, true, "RELIGION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubReligion", url, "hdfSubReligion", true, true, "SUBRELIGION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtAmount_PopUp]").ForceNumericOnly();
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry", url, "hdfCountry", true, true, "COUNTRYFIRST");
            GrandScriptUtils.DatePickerCommon("txtDojBefore");
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
            InitStateAuto();
            var country = parseInt($("[id$=hdfCountry]").val());
            if (country <= 0) {
                DisableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
            }
        }

        function InitStateAuto() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtState", url + "?Country=" + $("[id$=hdfCountry]").val(), "hdfState", true, true, "STATEAUTO");
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

        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                $("[id$=btnCurrency]").click();
            }
        }


        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtCountry") {
                ResetState()
                EnableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
                InitStateAuto();
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCountry") {
                ResetState()
                InitStateAuto();
                DisableAuto($("[id$=txtState]"), $("[id$=hdfState]"));
            }
        }

        function ResetState() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtState]").val(defText);
            $("[id$=hdfState]").val('-1');
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").show();
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
        function AfterClose(containerID) {
            if (containerID == "[id$=divPopUpSalaryPaymentDetails]") {
                $("[id$=btnCancelPopUp]").click();
            }
        }
        $("[id*=chkEmpHeader]").live("click", function () {
            var chkHeader = $(this);
            var grid = $(this).closest("table");
            $("input[type=checkbox]", grid).each(function () {
                if (chkHeader.is(":checked")) {
                    $(this).attr("checked", "checked");
                    $("td", $(this).closest("tr")).addClass("selected");
                } else {
                    $(this).removeAttr("checked");
                    $("td", $(this).closest("tr")).removeClass("selected");
                }
            });
        });

        $("[id*=chkEmpselect]").live("click", function () {
            var grid = $(this).closest("table");
            var chkHeader = $("[id*=chkEmpHeader]", grid);
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).removeClass("selected");
                chkHeader.removeAttr("checked");
            } else {
                $("td", $(this).closest("tr")).addClass("selected");
                if ($("[id*=chkEmpselect]", grid).length == $("[id*=chkEmpselect]:checked", grid).length) {
                    chkHeader.attr("checked", "checked");
                }
            }
        });

        function setValidationState(sender, validator) {
            ValidatorEnable(document.getElementById(validator),
sender.checked);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="154" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="156" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
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
                                            <%--Show Filter--%>
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                            <%--  Show Filter--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtListFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListFromDate" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate %>" AssociatedControlID="txtListToDate"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListToDate" TabIndex="3" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblListBonusType" AssociatedControlID="ddlListBonusType"
                                                CssClass="lbl-26perc" Text="<%$resources:BonusType %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlListBonusType" CssClass="lbl-25-5perc margnbotm0"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="ImageButton1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <%--<%$ resources:PageSize%>--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="4" />
                                                <asp:HiddenField runat="server" ID="hdfBOH_PKListPage" Value='<%# Eval("BOH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                            <HeaderStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BonusEntryNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployee" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_NO"))) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_NO"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDate" runat="server" Text='<%# Convert.ToDateTime(Eval("BOH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'
                                                    ToolTip='<%# Convert.ToDateTime(Eval("BOH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BonusType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvTypeText" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_BONUS_TYPE_TEXT"))) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_BONUS_TYPE_TEXT"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvCurrecyText" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_CURRENCY_CODE"))) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_CURRENCY_CODE"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                            <HeaderStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvAmountText" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("BOH_TOTAL_AMT"))%>'
                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("BOH_TOTAL_AMT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" CssClass="amount-numeric" />
                                            <HeaderStyle Width="4%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_REMARK"))),75) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("BOH_REMARK"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="40%" />
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
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
                                            <asp:Label ID="lblBonNoHdr" runat="server" Text="<%$ resources:BonusNo%>" AssociatedControlID="lblBonNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblBonNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateStar%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-20-7perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="5" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="5" CssClass="select-w61per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_SelectComapny %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblBonusTyp" AssociatedControlID="ddlListBonusTypeNew"
                                                CssClass="middle-lbl-d" Text="<%$resources:BonusTypeStar %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlListBonusTypeNew" CssClass="select-w60-2per"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBonusType" runat="server" ControlToValidate="ddlListBonusTypeNew"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterBonusType%>"
                                                InitialValue="-1"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblhdrCurrency" Text="<%$ resources:CurrencyReq%>"
                                                AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="5"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCurrency" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_Currency %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="CURRENCYSELECTED"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRateReq%>"
                                                AssociatedControlID="txtExchangeRate" CssClass="lbl-19-6perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExchangeRate" Text="" TabIndex="5" CssClass="input-small numeric medium"
                                                onkeypress="return validateRateFloatKeyPress(this,event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpExchangeRate" CssClass="star" SetFocusOnError="true"
                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="Save"
                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtExchangeRate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                            </asp:CompareValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="Label1" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarksHdr"></asp:Label>
                                            <asp:TextBox ID="txtRemarksHdr" runat="server" TabIndex="5" MaxLength="500" class="input-full"
                                                TextMode="MultiLine" onkeyup="limitText(this,500);" Height="40"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="col-md-2 col-sm-2 col-xs-2" style="height: 32px; float: right;">
                                <asp:Button runat="server" SkinID="btnInner-add" ID="btnShowPopUp" TabIndex="7" OnClick="ActionHandler"
                                    CommandName="SHOWPOPUP" Text="<%$resources:AddEmployee %>" ToolTip="<%$resources:AddEmployee %>"
                                    CssClass="margntop-1 margn-rgt0" Style="margin-top: -2.5px;" ValidationGroup="Save"
                                    OnClientClick="javascript:ValidatePageNow('Save')" />
                                <%-- ValidationGroup="save"--%>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    Details</h1>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divList" runat="server" class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpBonusTypeList" Width="100%" AllowPaging="false"
                                    AllowSorting="false" AutoGenerateColumns="false" OnRowDataBound="ActionHandler"
                                    ShowFooter="true" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                    <%--OnRowDataBound="ActionHandler"--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SINo%>">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdEmployee" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfRowNo" Value='<%# Eval("ROW_NO") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBodPk" Value='<%# Eval("BOD_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBodEmpPk" Value='<%# Eval("BOD_EMPLOYEE") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalBnsAmt" Text="Total"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdEmpType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPD_EMP_TYPE_TEXT"))),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EPD_EMP_TYPE_TEXT"))) %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDesigGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))),22) %>'
                                                    ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText")))%>'></asp:Label></ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdBranchLoc" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))),22) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("BOD_AMOUNT"))%>'
                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("BOD_AMOUNT"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblFooterTotalBonusAmt" Text='<%#GetFormattedCurrencyWithComma(Eval("BOH_TOTAL_AMT"))%>'
                                                    ToolTip='<%#GetFormattedCurrencyWithComma(Eval("BOH_TOTAL_AMT"))%>'></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle CssClass="amount-numeric" />
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle Width="5%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbDeleteEmployee"
                                                    TabIndex="21" SkinID="imbdeletegrid" CommandName="GRIDDELETE" ToolTip="<%$ resources:Delete%>"
                                                    OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="5%" HorizontalAlign="Right" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="divEmployeeDetails_PopUp" style="display: none" runat="server">
                    <div class="content-wrapper">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnAddMenu" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Add_Add %>"
                                CommandName="SAVE_ACTIONPOPUP" OnClick="ActionHandler" TabIndex="3" ToolTip="<%$resources:Controls,Add_Add %>"
                                ValidationGroup="abc" />
                            <asp:Button ID="btnCancelPopUp" runat="server" CommandName="CANCELPOPUP" OnClick="ActionHandler"
                                SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Close %>" TabIndex="3"
                                Text="<%$Resources:Controls,Close%>" />
                        </div>
                        <div id="divEmpFilterDetails">
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblreligion" Text="<%$ resources:Religion%>" AssociatedControlID="txtreligion"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtreligion" Visible="false" Text="" MaxLength="100"
                                                CssClass="input-small" TabIndex="69"></asp:TextBox>
                                            <asp:HiddenField ID="hdfReligion" Value="" runat="server" />
                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlReligion" TabIndex="1"
                                                CssClass="select-small-i" OnSelectedIndexChanged="ActionHandler">
                                                <%--select-small-b--%>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSubReligion" Text="<%$ resources:SubReligion%>"
                                                AssociatedControlID="txtSubReligion"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSubReligion" Visible="false" Text="" MaxLength="100"
                                                CssClass="input-small"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSubReligion" Value="" runat="server" />
                                            <asp:DropDownList runat="server" TabIndex="1" ID="ddlSubReligion" CssClass="select-small-i">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCountry" Text="<%$ resources:Country%>" AssociatedControlID="txtCountry"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCountry" Text="" TabIndex="37" MaxLength="100"
                                                CssClass="input-w55per"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCountry" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblState" AssociatedControlID="txtState" Text="<%$ resources:State%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtState" Text="" MaxLength="100" Width="190px" CssClass="input-w55per"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfState" Value="-1" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDojBefore" AssociatedControlID="txtDojBefore" Text="<%$ resources:DOJBefore%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDojBefore" Text="" MaxLength="100" Width="190px"
                                                CssClass="input-w55per" onkeydown="return CheckKey(event)" onpaste="return false;"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:ImageButton ID="btnFilterSearch" runat="server" Text="<%$ resources:Search%>"
                                                ToolTip="<%$ resources:Controls,Search%>" OnClick="ActionHandler" TabIndex="2"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0 margn-rgt4"
                                                ValidationGroup="Search" OnClientClick="javascript:ValidatePageNow('Search')" />
                                            <asp:ImageButton ID="btnFilterClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                ToolTip="<%$ resources:Controls,Clear%>" TabIndex="2" OnClick="ActionHandler"
                                                CommandName="CLEARDETAIL" SkinID="clear-ext" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="divEmpDetails" runat="server" class="gridwrap maxh-270">
                            <asp:GridView runat="server" ID="grdEmpBonus_PopUp" AllowPaging="false" AllowSorting="True"
                                AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
                                OnRowDataBound="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty_PopUp" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkEmpHeader_PopUp" runat="server" ToolTip="<%$ resources:Err_Employee%>"
                                                TabIndex="3" ValidationGroup="abc" />
                                            <%-- OnCheckedChanged="ChkEmpHdrPopup_Changed" AutoPostBack="true"--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkEmpselect_PopUp" TabIndex="3" ValidationGroup="abc" />
                                            <%--OnCheckedChanged="ChkEmpPopup_Changed"--%>
                                            <asp:HiddenField runat="server" ID="hdfEmpPk_PopUp" Value='<%# Eval("empPK") %>' />
                                            <asp:HiddenField runat="server" ID="hdfBOD_PK_PopUp" Value='0' />
                                            <asp:HiddenField runat="server" ID="hdfROW_NO_PopUp" Value='<%# Eval("ROW_NUMBER") %>' />
                                            <asp:HiddenField runat="server" ID="hdfEmpType" Value='<%# Eval("EPD_EMP_TYPE_TEXT") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="0.025%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempName_txt_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt"))),18) %>'
                                                ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empName_txt")))%>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Designation%> " SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesigPopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText"))),18) %>'
                                                ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDesignationText")))%>'></asp:Label></ItemTemplate>
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:BranchLocation%> " SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempBranchText_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText"))),18) %>'
                                                ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empBranchText")))%>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Department%> " SortExpression="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartment_PopUp" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText"))),18) %>'
                                                ToolTip='<%#  System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("empDepartmentText")))%>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Amount%> " SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtAmount_PopUp" Text='<%#GetEvaluatedFormula(Eval("BON_FORMULA_CAL"))%>'
                                                CssClass="numeric" ToolTip='<%#GetEvaluatedFormula(Eval("BON_FORMULA_CAL"))%>'
                                                Width="70px"></asp:TextBox>
                                            <%-- <asp:RequiredFieldValidator runat="server" ID="rfvAmnt" ControlToValidate="txtAmount_PopUp"
                                                ErrorMessage="*" ValidationGroup="abc" Display="Dynamic" Enabled="false"></asp:RequiredFieldValidator>--%>
                                            <%--<asp:RegularExpressionValidator ID="revAmnt" runat="server" ControlToValidate="txtAmount_PopUp"
                                                ForeColor="Red" ValidationExpression="^(0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*)$"
                                                Text="*" Display="Dynamic" ValidationGroup="abc" Enabled="false"></asp:RegularExpressionValidator>--%>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="amount-numeric" />
                                        <HeaderStyle Width="5%" CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <%--<asp:ValidationSummary ID="vspopup" ValidationGroup="abc" runat="server" />--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfLastModDate" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
