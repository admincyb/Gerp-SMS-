<%@ Page Title="<%$ Resources:Captions,Title_YearEndVoucher %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="YearEndVoucher.aspx.cs" Inherits="ERPSMS_v01.Finance.YearEndVoucher"
    Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtPendingAsOnDate");
            $("[id*=txtExchangeRate]").ForceNumericOnly();
            GrandScriptUtils.DatePickerCommon("txtPVDate");

        }





        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                //$("[id$=ddlCompany]").hide();

            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();

            }
            return false;
        }
        function PageViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlInActive]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlInActive]").hide();

            }
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
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
        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='lnkList']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lnkDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                ;
                $("[id$='lnkList']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lnkDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }

        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                // $("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                //$("[id$=btnVendor]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
            }
        }


        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
                if ($("[id$=hdfJournalizeWorkFlow]").val() == "1") {
                    //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("YearEndVoucher") %>', '1000', '550');
                    ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("YearEndVoucher") %>', "1%");
                    AfterCloseWkfInJournal();
                    //$("[id$=btnJournalize_Action]").click();
                }
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("YearEndVoucher") %>', '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', '<%= GetLocalResourceObject("YearEndVoucher") %>', "1%");
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtPendingAsOnDate") {
                $("[id$=btnPendingAsOn]").click();
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
                        Page_Validators.splice(i, 1);
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function ResetSelection() {
            $('[id$=grdVoucherList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
        }


        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
        }


        function ShowConfirmationMessage(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= GetLocalResourceObject("YedConfirmMsg") %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        ValidatePageNow('yearend');
                        __doPostBack(btn.name, '');

                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlSOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="1" class="medium" runat="server" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <%-- <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="56" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <%--  <li id="liPrintSI">
                                        <asp:Button runat="server" TabIndex="63" ID="btnPrintSI" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnJournalize" CommandName="JOURNALIZE" TabIndex="7"
                                            Text="<%$resources:Journalize %>" OnClick="ActionHandler" ToolTip="<%$resources:Journalize %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize" ValidationGroup="search" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="8" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <%--<div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="75" OnClick="ActionHandler" CommandName="INVOICELIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="76" OnClick="ActionHandler" CommandName="INVOICEDETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkPrintDocs" Text="<%$resources:PageNameRes,PrintInvoiceDocs %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="77" OnClick="ActionHandler" CommandName="PRINTINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <%-- <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <%--------------colpase btn----------%>
                        <%--    <div class="clear">
                            </div>--%>
                            <table class="table-devide tablelayout" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPendingAsOn" runat="server" Text="<%$resources:PendingAsOn %>"
                                                AssociatedControlID="txtPendingAsOnDate"></asp:Label>
                                            <asp:TextBox ID="txtPendingAsOnDate" runat="server" TabIndex="1" CssClass="input-small"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;" OnTextChanged="ActionHandler"
                                                AutoPostBack="true"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfPendingAsOnDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="search" EnableClientScript="true" runat="server" ControlToValidate="txtPendingAsOnDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PendingAsOnDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfPendingAsOnDate" runat="server" Value="" />
                                            <asp:Button ID="btnPendingAsOn" runat="server" OnClick="ActionHandler" CommandName="SEARCH"
                                                EnableTheming="false" Style="display: none" />
                                            
                                              <asp:Label ID="lblVoucherType" runat="server" Text="<%$resources:VoucherType %>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlVoucherType"></asp:Label>
                                            <asp:DropDownList ID="ddlVoucherType" runat="server" TabIndex="1" CssClass="select-small-c" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" Width="137">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVoucherNumber" runat="server" Text="Voucher No."  CssClass="middle-lbl-small"  AssociatedControlID="ddlVoucherNo"></asp:Label>
                                            <asp:DropDownList ID="ddlVoucherNo" runat="server" TabIndex="2" OnSelectedIndexChanged="ActionHandler" CssClass="select-small-e1"
                                                AutoPostBack="true" Width="137">
                                            </asp:DropDownList>                                                                                     
                                            <%-- <asp:Label ID="lblSearch" runat="server" Width="5px" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="9"
                                                CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="10"
                                                OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" ToolTip="<%$ resources:Controls,Clear %>" />--%>
                                        </div>
                                    </td>
                                </tr>
                           <%-- </table>
                            <table class="table-devide">--%>
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                           <asp:Label ID="lblCustomer" runat="server" Text="Party" AssociatedControlID="ddlCustomer"></asp:Label>
                                            <asp:DropDownList ID="ddlCustomer" runat="server" TabIndex="4" CssClass="select-half-a margnbotm0"  OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true">
                                            </asp:DropDownList>                                            
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                         <asp:Label ID="lblTransaction" runat="server" Text="<%$resources:Transaction %>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlTransaction"></asp:Label>
                                            <asp:DropDownList ID="ddlTransaction" runat="server" TabIndex="5" CssClass="select-small-e1 margnbotm0"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                             <asp:Label ID="lblStatus" runat="server" Text="<%$resources:Status %>" CssClass="middle-lbl-xsmall-b margnbotm0" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="6"  CssClass="select-small-a margnbotm0" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" Width="137">
                                                <%--<asp:ListItem Text="<%$ Resources:Captions,All %>" Value="0"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Completed %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>                                           
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdVoucherList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" OnRowDataBound="ActionHandler"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="6" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfTrxPk" Value='<%# Eval("TRX_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxType" Value='<%# Eval("TRX_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxTypeYe" Value='<%# Eval("TRX_TYPE_YE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfCurrencyPk" Value='<%# Eval("TRX_CURR_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfFthRefType" Value='<%# Eval("FTH_REF_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfFthPk" Value='<%# Eval("FTH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfFthRefPk" Value='<%# Eval("FTH_REF_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfFthCompanyPk" Value='<%# Eval("FTH_COMPANY") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxNo" runat="server" Text='<%# Eval("TRX_NO")%>' ToolTip='<%# Eval("TRX_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.TRX_DATE, Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.TRX_DATE, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Party %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TRX_PARTY_NAME"), 42)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TRX_PARTY_NAME"), 600)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VoucherType %>" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVochrType" runat="server" Text='<%# Eval("YED_PROCESS_TEXT") %>'
                                                    ToolTip='<%# Eval("YED_PROCESS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkVoucherNo" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="PRINT" Text='<%# Eval("TRX_VOUCHER_NO") %>' ToolTip='<%# Eval("TRX_VOUCHER_NO")%>'></asp:LinkButton>
                                                <%--<asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("TRX_VOUCHER_NO") %>'
                                                    ToolTip='<%# Eval("TRX_VOUCHER_NO")%>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VoucherDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.TRX_VOUCHER_DATE, Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.TRX_VOUCHER_DATE, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxCurrency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("TRX_CURRENCY")%>' ToolTip='<%# Eval("TRX_CURRENCY")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("TRX_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("TRX_AMOUNT")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("TRX_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("TRX_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExRate %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblExchangeRate" runat="server" Text='<%# Eval("TRX_EXCHG_RATE") %>'
                                                    ToolTip='<%# Eval("TRX_EXCHG_RATE") %>'></asp:Label>--%>
                                                <asp:Label ID="lblExchangeRate" runat="server" Text='<%# GetFormattedExchangeRate(Eval("TRX_EXCHG_RATE")) %>'
                                                    ToolTip='<%# GetFormattedExchangeRate(Eval("TRX_EXCHG_RATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Allocated %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblAllocated" runat="server" Text='<%# GetFormattedCurrency(Eval(Resources.DataFieldRes.TRX_ALLOCATED))%>'
                                                    ToolTip='<%#GetFormattedCurrency(Eval(Resources.DataFieldRes.TRX_ALLOCATED))%>'></asp:Label>--%>
                                                <asp:Label ID="lblAllocated" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval(Resources.DataFieldRes.TRX_ALLOCATED))%>'
                                                    ToolTip='<%#GetFormattedCurrencyWithSeperation(Eval(Resources.DataFieldRes.TRX_ALLOCATED))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Balance %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblBalance" runat="server" Text='<%# GetFormattedCurrency(Eval("TRX_BALANCE")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("TRX_BALANCE"))%>'></asp:Label>--%>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("TRX_BALANCE")) %>'
                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("TRX_BALANCE"))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("ICH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SApproved) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.SPosted) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                <asp:HiddenField ID="hdfCurrencyFormatWithSeperation" runat="server" />
                                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                            </div>
                            <%---------Exchange rate popup start------------%>
                            <div id="divExchangeRate" style="display: none">
                                <div class="Button-container-popup">
                                </div>
                                <div class="content-wrapper">
                                    <table>
                                        <tr>
                                            <td width="70%">
                                                <div>
                                                    <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:ExchangeRate %>"
                                                        AssociatedControlID="txtExchangeRate"></asp:Label>
                                                    <asp:TextBox ID="txtExchangeRate" CssClass="input-w70 numeric" runat="server" MaxLength="15"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfExchangeRate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="yearend" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExchangeRate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:RateValidation ID="vreExchangeRate" runat="server" ControlToValidate="txtExchangeRate"
                                                        DecimalDigits="5" ErrorMessage="<%$ resources:Err_ExchangeRate %>" NumberDigits="100"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="yearend"
                                                        NonZero="true"></cc1:RateValidation>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td width="30%">
                                                <div>
                                                    <asp:Button ID="btnSave" SkinID="btnInner-add-dsd" runat="server" Text="OK" OnClick="ActionHandler"
                                                        ValidationGroup="yearend" TabIndex="101" CommandName="OK" CommandArgument="PageAction_List"
                                                        OnClientClick="return ShowConfirmationMessage(this);" /><%--OnClientClick="javascript:ValidatePageNow('yearend')"--%>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td class="popup-head">
                                                <asp:Label ID="lblVoucherNo" Text="Voucher No" runat="server"></asp:Label>
                                            </td>
                                            <td class="popup-head numeric">
                                                <asp:Label ID="lblExRate" runat="server" Text="Ex.Rate"></asp:Label>
                                            </td>
                                            <td class="popup-head numeric">
                                                <asp:Label ID="lblVoucherAmount" runat="server" Text="Amount"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="popup-list">
                                                <asp:LinkButton ID="lbtnVoucherNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                    CommandName="PRINTPOPUP"></asp:LinkButton>
                                                <asp:HiddenField ID="hdfPopAppType" runat="server" />
                                                <asp:HiddenField ID="hdfPopVoucherID" runat="server" />
                                                <asp:HiddenField ID="hdfPopPK" runat="server" />
                                            </td>
                                            <td class="popup-list numeric">
                                                <asp:Label ID="lblPopRate" runat="server"></asp:Label>
                                            </td>
                                            <td class="popup-list numeric">
                                                <asp:Label ID="lblPopAmount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="error" id="divErrorLabel" runat="server" visible="false">
                                        <ul>
                                            <li>
                                                <asp:Literal runat="server" ID="lblErrorMessage" Text="<%$resources:SameExchangeRate %>"></asp:Literal></li></ul>
                                    </div>
                                </div>
                            </div>
                            <%-----------Exchange rate popup end------------%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnJournalize_Action" CommandName="JOURNALIZE" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                    <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                        EnableTheming="false" Style="display: none" />
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsYearend" ValidationGroup="yearend" runat="server" />
                    <asp:ValidationSummary ID="vsSearch" ValidationGroup="search" runat="server" />
                </div>
            </div>
            <%--User Control--%>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="yearend">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfCurrency" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
