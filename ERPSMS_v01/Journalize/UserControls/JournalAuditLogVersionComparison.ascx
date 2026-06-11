<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalAuditLogVersionComparison.ascx.cs"
    Inherits="ERPSMS_v01.Journalize.UserControls.JournalAuditLogVersionComparison" %>
<style type="text/css">
    .no-border {
        border: none !important;
        background: #ecedee;
        /*height: 19px !important;*/
    }

    .highlight-version {
        font-weight: bold;
    }

    .hgt-21 {
        height: 21px !important;
    }
</style>
<%--<asp:UpdatePanel runat="server" ID="aupdpnlJournalAuditLogVersionComparison">
    <ContentTemplate>--%>
<div class="content-wrapper">
    <asp:Table runat="server" ID="Table2" CssClass="tablelayout asptbllinks">
        <asp:TableRow ID="PageAction_Entry" runat="server">
            <%--EntryPage Table Row--%>
            <asp:TableCell>
                <table class="table-devide" style="border: solid 1px;">
                    <tr>
                        <td style="padding-top: 1%;">
                            <div class="div2col-S">
                                <asp:Label runat="server" ID="lblVer1" Text="Version:" AssociatedControlID="lblVersion"
                                    CssClass="input-w24-9per highlight-version"></asp:Label>
                                <asp:Label runat="server" ID="lblVersion" CssClass="no-border highlight-version"></asp:Label>
                                <asp:Label runat="server" ID="lblAct1" Text="Activity:" AssociatedControlID="lblActivity"
                                    CssClass="min-wdth-37-3"></asp:Label>
                                <asp:Label runat="server" ID="lblActivity" CssClass="no-border"></asp:Label>
                            </div>
                        </td>
                        <td style="padding-top: 1%;">
                            <asp:Label runat="server" ID="Label1" Text="Username:" AssociatedControlID="lblUsername"
                                CssClass="input-w24-9per"></asp:Label>
                            <asp:Label runat="server" ID="lblUsername" CssClass="no-border hgt-21"></asp:Label>
                            <asp:Label runat="server" ID="Label2" Text="Date & Time:" AssociatedControlID="lblDateTime"
                                CssClass="min-wdth-34-3"></asp:Label>
                            <asp:Label runat="server" ID="lblDateTime" CssClass="no-border hgt-21"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="tblTemplate" runat="server" CssClass="tablelayout asptbllinks">
        <asp:TableRow>
            <asp:TableCell>
                <table class="table-devide">
                    <tr>
                        <td style="padding-top: 1.5%">
                            <div class="div2col-S">
                                <asp:Label ID="lblVoucherNoHead" runat="server" Text="Voucher No:"
                                    AssociatedControlID="lblVoucherNo" />
                                <asp:Label ID="lblVoucherNo" runat="server" MaxLength="100" Enabled="false" CssClass="medium input-disabled input-small no-border"
                                    TabIndex="1" />
                                <asp:Label ID="lblVoucherDateHead" runat="server" Text="Voucher Date:"
                                    CssClass="middle-lbl-a" AssociatedControlID="lblVoucherDate" />
                                <asp:Label ID="lblVoucherDate" runat="server" MaxLength="200" CssClass="input-small no-border"
                                    TabIndex="2" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                <div class="clear">
                                </div>
                                <asp:Label ID="lblCurrencyHead" runat="server" Text="Currency:"
                                    AssociatedControlID="lblCurrency" />
                                <asp:Label ID="lblCurrency" runat="server" MaxLength="100" CssClass="input-small no-border"
                                    TabIndex="5" />
                                <asp:Label ID="lblExchangeRateHead" runat="server" Text="Exchange Rate:"
                                    CssClass="middle-lbl-a" AssociatedControlID="lblExchangeRate" />
                                <asp:Label ID="lblExchangeRate" runat="server" Enabled="false" CssClass="medium input-disabled input-small no-border"
                                    TabIndex="6" />
                            </div>
                        </td>
                        <td style="padding-top: 1.5%">
                            <div class="div2col-S">
                                <asp:Label ID="lblRefNoHead" runat="server" Text="Ref No:" AssociatedControlID="lblRefNo" />
                                <asp:Label ID="lblRefNo" runat="server" CssClass="input-small no-border" TabIndex="3" MaxLength="100"
                                    onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" />

                                <asp:Label ID="lblRefDateHead" runat="server" Text="Ref Date:"
                                    CssClass="middle-lbl-small-d" AssociatedControlID="lblRefDate" />
                                <asp:Label ID="lblRefDate" runat="server" MaxLength="100" CssClass="input-small no-border"
                                    TabIndex="4" onkeydown="return CheckKey(event)" onpaste="return false;" />

                                <div id="divCashAcount">
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblCashBalance" runat="server" Visible="false" class="middle-lbl-small-d"
                                        Text="Balance" AssociatedControlID="lblBalance" />
                                    <asp:Label ID="lblBalance" runat="server" Visible="false" class="medium input-disabled input-small"
                                        Text=""></asp:Label>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr id="trTo">
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="lblToHead" runat="server" Text="To:" AssociatedControlID="lblTo" />
                                <asp:Label ID="lblTo" runat="server" MaxLength="100" TabIndex="7" CssClass="no-border"> </asp:Label>
                            </div>
                        </td>
                    </tr>

                </table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="PageAction_List" runat="server">
            <asp:TableCell>


                <div class="gridwrap grid-group">
                    <h3>Debit Details
                    </h3>
                    <div class="clear">
                    </div>
                    <div class="grid-group-table">
                        <asp:GridView runat="server" ID="grdDebitDtls" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">

                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHead" runat="server" ToolTip='<%# Eval("FTR_TYPE") %>'
                                            Text='<%# Eval("FTR_TYPE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                    <FooterStyle Width="6%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dr. Accounts">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>'
                                            Text='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_ACCOUNT_ISMOD" Value='<%# Eval("FTR_ACCOUNT_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_PAYMENT_MODE_ISMOD" Value='<%# Eval("FTR_PAYMENT_MODE_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_NARRATION_ISMOD" Value='<%# Eval("FTR_NARRATION_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_CR_AMT_TC_ISMOD" Value='<%# Eval("FTR_CR_AMT_TC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_DR_AMT_BC_ISMOD" Value='<%# Eval("FTR_DR_AMT_BC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_TYPE_PK_ISMOD" Value='<%# Eval("FTR_TYPE_PK_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_COST_BIT" Value='<%# Eval("FTR_COST_BIT") %>' />

                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ resources:Controls,SubAccount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAccount" runat="server" ToolTip='<%# Eval("FTR_TYPE_TEXT") %>'
                                            Text='<%# Eval("FTR_TYPE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                    <FooterStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval("FTR_NARRATION") %>'
                                            Text='<%# Eval("FTR_NARRATION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotal" Text="Total Amount(Dr)" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblDrTotal" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="10%" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCostcenter" TabIndex="24" runat="server" SkinID="costcenter-icon" Visible="false"
                                            ToolTip="Cost Center Allocation" OnClick="ActionHandler" CommandName="COSTCENTER" />
                                        <asp:HiddenField runat="server" ID="hdfType" Value="1" />
                                        <asp:HiddenField runat="server" ID="hdfFTR_AUDIT_VERSION" Value='<%# Eval("FTR_AUDIT_VERSION") %>' />

                                    </ItemTemplate>
                                    <FooterStyle Width="2%" />
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="gridwrap grid-group">
                    <h3>Credit Details
                    </h3>
                    <div class="clear">
                    </div>
                    <div class="grid-group-table">
                        <asp:GridView runat="server" ID="grdCreditDtls" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHead" runat="server" ToolTip='<%# Eval("FTR_TYPE") %>'
                                            Text='<%# Eval("FTR_TYPE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cr. Accounts">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>'
                                            Text='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_ACCOUNT_ISMOD" Value='<%# Eval("FTR_ACCOUNT_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_PAYMENT_MODE_ISMOD" Value='<%# Eval("FTR_PAYMENT_MODE_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_NARRATION_ISMOD" Value='<%# Eval("FTR_NARRATION_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_CR_AMT_TC_ISMOD" Value='<%# Eval("FTR_CR_AMT_TC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_DR_AMT_BC_ISMOD" Value='<%# Eval("FTR_DR_AMT_BC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_TYPE_PK_ISMOD" Value='<%# Eval("FTR_TYPE_PK_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_COST_BIT" Value='<%# Eval("FTR_COST_BIT") %>' />

                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ resources:Controls,SubAccount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAccount" runat="server" ToolTip='<%# Eval("FTR_TYPE_TEXT") %>'
                                            Text='<%# Eval("FTR_TYPE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval("FTR_NARRATION") %>'
                                            Text='<%# Eval("FTR_NARRATION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotal" Text="Total Amount(Cr)" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblCrTotal" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="10%" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCostcenter" TabIndex="24" runat="server" SkinID="costcenter-icon"
                                            ToolTip="Cost Center Allocation" OnClick="ActionHandler" CommandName="COSTCENTER" Visible="false" />
                                        <asp:HiddenField runat="server" ID="hdfType" Value="2" />
                                        <asp:HiddenField runat="server" ID="hdfFTR_AUDIT_VERSION" Value='<%# Eval("FTR_AUDIT_VERSION") %>' />
                                    </ItemTemplate>
                                    <FooterStyle Width="2%" />
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell>
                <table class="table-devide">
                    <tr>
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="lblRemarksHead" runat="server" Text="Remarks:"
                                    AssociatedControlID="lblRemarks" />
                                <asp:Label ID="lblRemarks" runat="server" MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
                    <tr id="TrNarrattion" runat="server" visible="false">
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="lnlNarrationHead" runat="server" Text="Narration:"
                                    AssociatedControlID="lblNarration" />
                                <asp:Label ID="lblNarration" runat="server" MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
            <asp:TableCell>
                <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <div class="row" runat="server" id="DivPreviousHead" visible="false">
        <h3 style="text-align: left; padding-top: 1%;">
            <asp:Literal runat="server" Text="Previous Version"></asp:Literal></h3>
    </div>
    <asp:Table runat="server" ID="tblprevoiusHead" CssClass="tablelayout asptbllinks" Visible="false">
        <asp:TableRow ID="TableRow1" runat="server">
            <%--EntryPage Table Row--%>
            <asp:TableCell>
                <table class="table-devide" style="border: solid 1px;">
                    <tr>
                        <td style="padding-top: 1%;">
                            <div class="div2col-S">
                                <asp:Label runat="server" ID="Label3" Text="Version:" AssociatedControlID="lblVersionPrev"
                                    CssClass="input-w24-9per highlight-version"></asp:Label>
                                <asp:Label runat="server" ID="lblVersionPrev" CssClass="no-border highlight-version"></asp:Label>
                                <asp:Label runat="server" ID="Label5" Text="Activity:" AssociatedControlID="lblActivityPrev"
                                    CssClass="min-wdth-37-3"></asp:Label>
                                <asp:Label runat="server" ID="lblActivityPrev" CssClass="no-border"></asp:Label>
                            </div>
                        </td>
                        <td style="padding-top: 1%;">
                            <asp:Label runat="server" ID="Label7" Text="Username:" AssociatedControlID="lblUsernamePrev"
                                CssClass="input-w24-9per"></asp:Label>
                            <asp:Label runat="server" ID="lblUsernamePrev" CssClass="no-border hgt-21"></asp:Label>
                            <asp:Label runat="server" ID="Label9" Text="Date & Time:" AssociatedControlID="lblDateTimePrev"
                                CssClass="min-wdth-34-3"></asp:Label>
                            <asp:Label runat="server" ID="lblDateTimePrev" CssClass="no-border hgt-21"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="tbltemplatePrevious" runat="server" CssClass="tablelayout asptbllinks" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <table class="table-devide">
                    <tr>
                        <td style="padding-top: 1.5%">
                            <div class="div2col-S">
                                <asp:Label ID="Label11" runat="server" Text="Voucher No:"
                                    AssociatedControlID="lblVoucherNoPre" />
                                <asp:Label ID="lblVoucherNoPre" runat="server" MaxLength="100" Enabled="false" CssClass="medium input-disabled input-small no-border"
                                    TabIndex="1" />
                                <asp:Label ID="Label13" runat="server" Text="Voucher Date:"
                                    CssClass="middle-lbl-a" AssociatedControlID="lblVoucherDatePrev" />
                                <asp:Label ID="lblVoucherDatePrev" runat="server" MaxLength="200" CssClass="input-small no-border"
                                    TabIndex="2" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                <div class="clear">
                                </div>
                                <asp:Label ID="Label15" runat="server" Text="Currency:"
                                    AssociatedControlID="lblCurrencyPrev" />
                                <asp:Label ID="lblCurrencyPrev" runat="server" MaxLength="100" CssClass="input-small no-border"
                                    TabIndex="5" />
                                <asp:Label ID="Label17" runat="server" Text="Exchange Rate:"
                                    CssClass="middle-lbl-a" AssociatedControlID="lblExchangeRatePrev" />
                                <asp:Label ID="lblExchangeRatePrev" runat="server" Enabled="false" CssClass="medium input-disabled input-small no-border"
                                    TabIndex="6" />
                            </div>
                        </td>
                        <td style="padding-top: 1.5%">
                            <div class="div2col-S">
                                <asp:Label ID="Label19" runat="server" Text="Ref No:" AssociatedControlID="lblRefNoPrev" />
                                <asp:Label ID="lblRefNoPrev" runat="server" CssClass="input-small no-border" TabIndex="3" MaxLength="100"
                                    onkeydown="limitText(this,100);" onkeyup="limitText(this,100);" />

                                <asp:Label ID="Label21" runat="server" Text="Ref Date:"
                                    CssClass="middle-lbl-small-d" AssociatedControlID="lblRefDatePrev" />
                                <asp:Label ID="lblRefDatePrev" runat="server" MaxLength="100" CssClass="input-small no-border"
                                    TabIndex="4" onkeydown="return CheckKey(event)" onpaste="return false;" />

                                <div id="divCashAcountPrev">
                                    <div class="clear">
                                    </div>

                                    <asp:Label ID="Label23" runat="server" Visible="false" class="middle-lbl-small-d"
                                        Text="Balance" AssociatedControlID="lblBalancePrev" />
                                    <asp:Label ID="lblBalancePrev" runat="server" Visible="false" class="medium input-disabled input-small"
                                        Text=""></asp:Label>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr id="trTo">
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="Label25" runat="server" Text="To:" AssociatedControlID="lblToPrev" />
                                <asp:Label ID="lblToPrev" runat="server" MaxLength="100" TabIndex="7" CssClass="no-border"> </asp:Label>
                            </div>
                        </td>
                    </tr>

                </table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server">
            <asp:TableCell>


                <div class="gridwrap grid-group">
                    <h3>Debit Details
                    </h3>
                    <div class="clear">
                    </div>
                    <div class="grid-group-table">
                        <asp:GridView runat="server" ID="grdDebitDtlsPrev" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHead" runat="server" ToolTip='<%# Eval("FTR_TYPE") %>'
                                            Text='<%# Eval("FTR_TYPE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                    <FooterStyle Width="6%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Dr. Accounts">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>'
                                            Text='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_ACCOUNT_ISMOD" Value='<%# Eval("FTR_ACCOUNT_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_PAYMENT_MODE_ISMOD" Value='<%# Eval("FTR_PAYMENT_MODE_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_NARRATION_ISMOD" Value='<%# Eval("FTR_NARRATION_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_CR_AMT_TC_ISMOD" Value='<%# Eval("FTR_CR_AMT_TC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_DR_AMT_BC_ISMOD" Value='<%# Eval("FTR_DR_AMT_BC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_TYPE_PK_ISMOD" Value='<%# Eval("FTR_TYPE_PK_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_COST_BIT" Value='<%# Eval("FTR_COST_BIT") %>' />

                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ resources:Controls,SubAccount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAccount" runat="server" ToolTip='<%# Eval("FTR_TYPE_TEXT") %>'
                                            Text='<%# Eval("FTR_TYPE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                    <FooterStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval("FTR_NARRATION") %>'
                                            Text='<%# Eval("FTR_NARRATION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotal" Text="Total Amount(Dr)" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblDrTotal" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="10%" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCostcenter" TabIndex="24" runat="server" SkinID="costcenter-icon" Visible="false"
                                            ToolTip="Cost Center Allocation" OnClick="ActionHandler" CommandName="COSTCENTER" />
                                        <asp:HiddenField runat="server" ID="hdfType" Value="3" />
                                        <asp:HiddenField runat="server" ID="hdfFTR_AUDIT_VERSION" Value='<%# Eval("FTR_AUDIT_VERSION") %>' />

                                    </ItemTemplate>
                                    <FooterStyle Width="2%" />
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="gridwrap grid-group">
                    <h3>Credit Details
                    </h3>
                    <div class="clear">
                    </div>
                    <div class="grid-group-table">
                        <asp:GridView runat="server" ID="grdCreditDtlsPrev" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHead" runat="server" ToolTip='<%# Eval("FTR_TYPE") %>'
                                            Text='<%# Eval("FTR_TYPE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cr. Accounts">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>'
                                            Text='<%# Eval("FTR_ACCOUNT_CODE") + " -  " + Eval("FTR_ACCOUNT_TEXT")%>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_ACCOUNT_ISMOD" Value='<%# Eval("FTR_ACCOUNT_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_PAYMENT_MODE_ISMOD" Value='<%# Eval("FTR_PAYMENT_MODE_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_NARRATION_ISMOD" Value='<%# Eval("FTR_NARRATION_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_CR_AMT_TC_ISMOD" Value='<%# Eval("FTR_CR_AMT_TC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_DR_AMT_BC_ISMOD" Value='<%# Eval("FTR_DR_AMT_BC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_TYPE_PK_ISMOD" Value='<%# Eval("FTR_TYPE_PK_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_COST_BIT" Value='<%# Eval("FTR_COST_BIT") %>' />

                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="<%$ resources:Controls,SubAccount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAccount" runat="server" ToolTip='<%# Eval("FTR_TYPE_TEXT") %>'
                                            Text='<%# Eval("FTR_TYPE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval("FTR_NARRATION") %>'
                                            Text='<%# Eval("FTR_NARRATION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotal" Text="Total Amount(Cr)" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblCrTotal" runat="server" />
                                        </div>
                                    </FooterTemplate>
                                    <FooterStyle Width="10%" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnCostcenter" TabIndex="24" runat="server" SkinID="costcenter-icon"
                                            ToolTip="Cost Center Allocation" OnClick="ActionHandler" CommandName="COSTCENTER" Visible="false" />
                                        <asp:HiddenField runat="server" ID="hdfType" Value="4" />
                                        <asp:HiddenField runat="server" ID="hdfFTR_AUDIT_VERSION" Value='<%# Eval("FTR_AUDIT_VERSION") %>' />
                                    </ItemTemplate>
                                    <FooterStyle Width="2%" />
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell>
                <table class="table-devide">
                    <tr>
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="Label27" runat="server" Text="Remarks:"
                                    AssociatedControlID="lblRemarksPrev" />
                                <asp:Label ID="lblRemarksPrev" runat="server" MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server">
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="Label29" runat="server" Text="Narration:"
                                    AssociatedControlID="lblNarrationPrev" />
                                <asp:Label ID="lblNarrationPrev" runat="server" MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</div>
<div id="divDebitCostCenter" style="display: none" runat="server">
    <div class="grid-group-table">
        <asp:GridView runat="server" ID="grdDebitCostCenter" Width="100%" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">

            <EmptyDataTemplate>
                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
            </EmptyDataTemplate>
            <Columns>

                <asp:TemplateField HeaderText="Cost Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCostCenter" runat="server" ToolTip='<%# Eval("FTR_CNM_CODE") %>'
                            Text='<%# Eval("FTR_CNM_CODE") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_FTD_AMT_BC_ISMOD" Value='<%# Eval("FTR_FTD_AMT_BC_ISMOD") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_CNM_CODE_ISMOD" Value='<%# Eval("FTR_CNM_CODE_ISMOD") %>' />

                    </ItemTemplate>
                    <ItemStyle Width="30%" />
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <asp:Label ID="lblTotalHead" Text="Total" runat="server" />
                        </div>
                    </FooterTemplate>
                    <ItemStyle Width="30%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountDt" runat="server" ToolTip='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>'
                            Text='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>' CssClass="input-w70 numeric" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                    </FooterTemplate>
                    <FooterStyle Width="10%" />
                    <HeaderStyle CssClass="amount-numeric" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<div id="divCreditCostCenter" style="display: none" runat="server">
    <div class="grid-group-table">
        <asp:GridView runat="server" ID="grdCreditCostCenter" Width="100%" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">

            <EmptyDataTemplate>
                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
            </EmptyDataTemplate>
            <Columns>

                <asp:TemplateField HeaderText="Cost Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCostCenter" runat="server" ToolTip='<%# Eval("FTR_CNM_CODE") %>'
                            Text='<%# Eval("FTR_CNM_CODE") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_FTD_AMT_BC_ISMOD" Value='<%# Eval("FTR_FTD_AMT_BC_ISMOD") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_CNM_CODE_ISMOD" Value='<%# Eval("FTR_CNM_CODE_ISMOD") %>' />

                    </ItemTemplate>
                    <ItemStyle Width="30%" />
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <asp:Label ID="lblTotalHead" Text="Total" runat="server" />
                        </div>
                    </FooterTemplate>
                    <ItemStyle Width="30%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountCr" runat="server" ToolTip='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>'
                            Text='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>' CssClass="input-w70 numeric" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                    </FooterTemplate>
                    <FooterStyle Width="10%" />
                    <HeaderStyle CssClass="amount-numeric" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<div id="divDebitCostCenter1" style="display: none">
    <div class="grid-group-table">
        <asp:GridView runat="server" ID="grdDebitCostCenterPrev" Width="100%" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">

            <EmptyDataTemplate>
                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
            </EmptyDataTemplate>
            <Columns>

                <asp:TemplateField HeaderText="Cost Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCostCenter" runat="server" ToolTip='<%# Eval("FTR_CNM_CODE") %>'
                            Text='<%# Eval("FTR_CNM_CODE") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_FTD_AMT_BC_ISMOD" Value='<%# Eval("FTR_FTD_AMT_BC_ISMOD") %>' />

                    </ItemTemplate>
                    <ItemStyle Width="30%" />
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <asp:Label ID="lblTotalHead" Text="Total" runat="server" />
                        </div>
                    </FooterTemplate>
                    <ItemStyle Width="30%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountDt" runat="server" ToolTip='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>'
                            Text='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>' CssClass="input-w70 numeric" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                    </FooterTemplate>
                    <FooterStyle Width="10%" />
                    <HeaderStyle CssClass="amount-numeric" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<div id="divCreditCostCenter1" style="display: none">
    <div class="grid-group-table">
        <asp:GridView runat="server" ID="grdCreditCostCenterPrev" Width="100%" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">

            <EmptyDataTemplate>
                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
            </EmptyDataTemplate>
            <Columns>

                <asp:TemplateField HeaderText="Cost Center">
                    <ItemTemplate>
                        <asp:Label ID="lblCostCenter" runat="server" ToolTip='<%# Eval("FTR_CNM_CODE") %>'
                            Text='<%# Eval("FTR_CNM_CODE") %>' />
                        <asp:HiddenField runat="server" ID="hdfFTR_FTD_AMT_BC_ISMOD" Value='<%# Eval("FTR_FTD_AMT_BC_ISMOD") %>' />

                    </ItemTemplate>
                    <ItemStyle Width="30%" />
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <asp:Label ID="lblTotalHead" Text="Total" runat="server" />
                        </div>
                    </FooterTemplate>
                    <ItemStyle Width="30%" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountCr" runat="server" ToolTip='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>'
                            Text='<%# Eval("FTR_FTD_AMT_BC","{0:N2}") %>' CssClass="input-w70 numeric" />
                    </ItemTemplate>
                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal" runat="server" />
                        </div>
                    </FooterTemplate>
                    <FooterStyle Width="10%" />
                    <HeaderStyle CssClass="amount-numeric" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<asp:Button ID="btnCloseVoucherPopup" runat="server"
    OnClick="ActionHandler" TabIndex="3" CommandName="COSTCENTERCANCEL" SkinID="btnInner-search"
    EnableTheming="false" Style="display: none" />
<%--  </ContentTemplate>
</asp:UpdatePanel>--%>
