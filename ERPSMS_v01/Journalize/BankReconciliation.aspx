<%@ Page Title="<%$ Resources:Captions,Title_BankReconciliation %>" Language="C#"
    Theme="ClassicExt" EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="BankReconciliation.aspx.cs" Inherits="ERPSMS_v01.Journalize.BankReconciliation" %>

<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtFromDate");
            GrandScriptUtils.DatePickerCommon("txtToDate");
            GrandScriptUtils.DatePickerCommon("txtClearingDate");
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtJournalCurrency", url, "hdfJournalCurr", true, true, "CURRENCYCODE");
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();
        }




        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                AfterCloseWkfInJournal();
                //$("[id$=btnEdit]").click();
            }
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtJournalCurrency") {
                $("[id$=btnCurrencyJV]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtJournalCurrency") {
                $("[id$=btnCurrencyJV]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function setValidationState(sender, validator) {
            ValidatorEnable(document.getElementById(validator), sender.checked);
        }

    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlBankReconcln" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                            </asp:TableCell>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul id="pnlEntry" runat="server">
                                    <li id="pnlAdd" runat="server">
                                        <asp:Button ID="btnAdd" runat="server" CommandName="JOURNALIZE" Text="<%$ resources:Controls,AddJV %>"
                                            SkinID="btnInner-add-dsd" CommandArgument="SEC_ActionPanel" ToolTip="<% $resources:Controls,AddJV %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            SkinID="btnInner-Save" OnClientClick="return ValidatePageNow('Save');" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<% $resources:Controls,Save %>" OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$ resources:Controls,Cancel %>"
                                            SkinID="btnInner-Cancel" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Cancel %>" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="PageAction_List" runat="server" CssClass="asptbllinks">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:HiddenField ID="hdfOpeningBal" runat="server" />
                            <asp:HiddenField ID="hdfAddAmount" runat="server" />
                            <asp:HiddenField ID="hdfLessAmount" runat="server" />
                            <asp:HiddenField ID="hdfRunningBal" runat="server" />
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:Controls,FromDate %>"
                                                AssociatedControlID="txtFromDate" />
                                            <asp:TextBox ID="txtFromDate" runat="server" MaxLength="11" CssClass="input-small"
                                                TabIndex="1" />
                                            <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Bank" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtFromDate" ErrorMessage="<%$ resources:Err_FromDate %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblBankAccount" runat="server" Text="<%$ resources:Controls,BankAccount %>"
                                                AssociatedControlID="ddlBankAccount" />
                                            <asp:DropDownList ID="ddlBankAccount" runat="server" TabIndex="3" CssClass="select-half" />
                                            <asp:RequiredFieldValidator ID="vrfBankAccount" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Bank" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlBankAccount" ErrorMessage="<%$ resources:Err_BankAccount %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:Controls,ToDate %>"
                                                AssociatedControlID="txtToDate" />
                                            <asp:TextBox ID="txtToDate" runat="server" MaxLength="11" CssClass="input-small"
                                                TabIndex="2" />
                                            <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Bank" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtToDate" ErrorMessage="<%$ resources:Err_ToDate %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblInstrNo" runat="server" Text="<%$ resources:Controls,InstrNo %>"
                                                AssociatedControlID="txtInstrNo" />
                                            <asp:TextBox ID="txtInstrNo" runat="server" CssClass="input-small" TabIndex="4" />
                                            <asp:Button ID="btnSearch" runat="server" SkinID="btnInner-search" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClientClick="return ValidatePageNow('Bank');"
                                                ValidationGroup="Bank" CommandName="SEARCH" OnClick="ActionHandler" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="check-2col">
                                            <asp:CheckBox ID="chbUnReconciled" runat="server" Text="Un Reconciled" Checked="true" />
                                            <asp:CheckBox ID="chbReconciled" runat="server" Text="Reconciled" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdVouchers" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,VoucherDate %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes,AccountCode %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblVoucherDate" runat="server" ToolTip='<%# Eval( Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>'
                                                    Text='<%# Eval( Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxPK" Value='<%# Eval(Resources.DataFieldRes.TrxPK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefPK" Value='<%# Eval(Resources.DataFieldRes.RefPK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefType" Value='<%# Eval(Resources.DataFieldRes.JournalizeType) %>' />
                                                <asp:HiddenField runat="server" ID="hdfCompany" Value='<%# Eval("FTH_COMPANY") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="false" />
                                            <ItemStyle Width="9%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,VoucherNo %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes,AccountName %>"--%>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnVoucherNo" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.VoucherNo) %>'
                                                    CssClass="text-underline" Text='<%# Eval(Resources.DataFieldRes.VoucherNo) %>'
                                                    OnClick="ActionHandler" CommandName="POPUPADD" /><%----%>
                                                <asp:HiddenField ID="hdfAppType" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrx) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Party %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes,AccountShortName %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" ToolTip='<%# Eval("FTH_PARTY_NAME") %>'
                                                    Text='<%# Eval("FTH_PARTY_NAME") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,InstrNo %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes, AccountType %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstrNo" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.FinTrxInstrNo)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.FinTrxInstrNo)),18) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,InstrDate %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes,AccountIsGroup %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstrDate" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxInstrDate, Resources.Constants.DateFormatGrid) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.FinTrxInstrDate, Resources.Constants.DateFormatGrid) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,DebitAmt %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes, AccountCategory %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount, "{0:c}") %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                              <FooterTemplate>
                                                <asp:Label ID="lblTotalDebit" runat="server" />
                                            </FooterTemplate>
                                            <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,CreditAmt %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes, AccountCategory %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount, "{0:c}") %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCredit" runat="server" />
                                            </FooterTemplate>
                                            <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Balance %>">
                                            <%--SortExpression="<%$ resources:DataFieldRes, AccountCategory %>"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" />
                                                <%--ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>'
                                                    Text='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>'--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblClosBal" runat="server" />
                                            </FooterTemplate>
                                            <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ClearingDate %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtClearingDate" runat="server" onkeydown="return CheckKey(event)" onpaste="return false;" CssClass="Uidate-picker" Text='<%# Eval(Resources.DataFieldRes.ftrClearDate) == null ? Eval(Resources.DataFieldRes.FinTrxInstrDate, Resources.Constants.DateFormatGrid) : Eval(Resources.DataFieldRes.ftrClearDate, Resources.Constants.DateFormatGrid) %>' />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfClearDate" CssClass="star" SetFocusOnError="true"
                                                        InitialValue="" ValidationGroup="Save" EnableClientScript="true" runat="server" Enabled="false"
                                                        Display="Dynamic" Text="*" ControlToValidate="txtClearingDate" ErrorMessage="<%$ resources:Err_ClearDate %>" />
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chbSelect" runat="server" Checked='<%# Eval(Resources.DataFieldRes.IsReconciled).ToString() == "1" ?  true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableFooterRow>
                        <asp:TableCell>
                            <div class="divcol-Totalwrap1" runat="server" id="Totalwrap1" visible="false">
                                <h4>
                                    <%= Resources.Controls.Summary %></h4>
                                <div class="divcol-Total">
                                    <asp:Label ID="lblOpBlnc" AssociatedControlID="lblOpngBlnc" runat="server" Text="<%$ resources:Controls,BalanceAsPerStatement %>" />
                                    <asp:Label ID="lblOpngBlnc" runat="server" Text="0" CssClass="numeric" />
                                    <asp:Label ID="lblAdd" AssociatedControlID="lblDeposits" runat="server" Text="<%$ resources:Controls,DepositNotCredited %>" />
                                    <asp:Label ID="lblDeposits" runat="server" Text="0" CssClass="numeric" />
                                    <asp:Label ID="lblLess" AssociatedControlID="lblUnPrsntdCheques" runat="server" Text="<%$ resources:Controls,UnpresentedCheques %>" />
                                    <asp:Label ID="lblUnPrsntdCheques" runat="server" Text="0" CssClass="numeric" />
                                    <asp:Label ID="lblClBlnc" AssociatedControlID="lblClosingBlnc" runat="server" Text="<%$ resources:Controls,BalanceAsPerBank %>" />
                                    <asp:Label ID="lblClosingBlnc" runat="server" Text="0" CssClass="numeric" />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableFooterRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Bank" runat="server" />
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
            </div>
            <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                EnableTheming="false" Style="display: none" />
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfSubTypePk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfTrxRefPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
