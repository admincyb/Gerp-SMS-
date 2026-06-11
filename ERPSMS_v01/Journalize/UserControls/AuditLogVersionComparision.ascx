<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuditLogVersionComparision.ascx.cs" Inherits="ERPSMS_v01.UserControls.AuditLogVersionComparision" %>
<style type="text/css">
    .no-border {
        border: none !important;
        background: #ecedee;
        /*height: 19px !important;*/
    }

    .highlight-version {
        font-weight: bold;
    }
    .hgt-21
    {
height: 21px !important;
    }
</style>
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
                                <%-- <asp:LinkButton ID="lnkAuditLog" runat="server" Text="<%$resources:Controls,AuditLog%>" ToolTip="<%$resources:Controls,AuditLog%>"
                                                CssClass="text-underline" Visible="false" CommandName="AUDITLOG" OnClick="ActionHandler" />--%>
                                <asp:Label ID="lblVoucherDateHead" runat="server" Text="Voucher Date:"
                                    CssClass="middle-lbl-a" AssociatedControlID="lblVoucherDate" />
                                <asp:Label ID="lblVoucherDate" runat="server" MaxLength="200" CssClass="input-small no-border"
                                    TabIndex="2" onkeydown="return CheckKey(event)" onpaste="return false;" />

                                <%--  <asp:Button runat="server" ID="btnLoad" CommandName="LOADFROMTEMPLATE" ValidationGroup="ycvLoad"
                                                OnClick="ActionHandler" Text="<%$resources:Controls,LoadFromTemplate %>" CommandArgument="PageAction_Entry"
                                                SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,LoadFromTemplate %>"
                                                OnClientClick="javascript:ValidatePageNow('ycvLoad')" />--%>
                                <div class="clear">
                                </div>
                                <asp:Label ID="lblCurrencyHead" runat="server" Text="Currency:"
                                    AssociatedControlID="lblCurrency" />
                                <asp:Label ID="lblCurrency" runat="server" MaxLength="100" CssClass="input-small no-border"
                                    TabIndex="5" />

                                <%--                                            <asp:Button ID="btnCurrency" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ACTIVATE" />--%>
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
                                    <%--  <asp:Label ID="lblCashAccount" runat="server" Text="Cash Account"
                                                    AssociatedControlID="ddlCashAccount" />
                                                <asp:DropDownList ID="ddlCashAccount" runat="server" AutoPostBack="true" CssClass="medium"
                                                    OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>--%>
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
                                <%--<asp:TextBox ID="txtTo" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="7"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="lblRemarksHead" runat="server" Text="Remarks:"
                                    AssociatedControlID="lblRemarks" />
                                <asp:Label ID="lblRemarks" runat="server"  MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
                      <tr id="TrNarrattion" runat="server" visible="false"> 
                        <td colspan="2">
                            <div class="divcol-S">
                                <asp:Label ID="lnlNarrationHead" runat="server" Text="Narration:"
                                    AssociatedControlID="lblNarration" />
                                <asp:Label ID="lblNarration" runat="server"  MaxLength="500"
                                    TabIndex="8" CssClass="no-border" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="PageAction_List" runat="server">
            <asp:TableCell>


                <div class="gridwrap grid-group">
                    <h3>
                        <%= Resources.Controls.SelectedAccounts %>
                    </h3>
                    <div class="clear">
                    </div>
                    <div class="grid-group-table">
                        <asp:GridView runat="server" ID="grdVoucher" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                            <%--OnRowDataBound="ActionHandler"ShowFooter="true"--%>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,AccountCode %>" SortExpression="<%$ resources:DataFieldRes,PackingCode %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_CODE") %>'
                                            Text='<%# Eval("FTR_ACCOUNT_CODE") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_ACCOUNT_ISMOD" Value='<%# Eval("FTR_ACCOUNT_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_PAYMENT_MODE_ISMOD" Value='<%# Eval("FTR_PAYMENT_MODE_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_NARRATION_ISMOD" Value='<%# Eval("FTR_NARRATION_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_CR_AMT_TC_ISMOD" Value='<%# Eval("FTR_CR_AMT_TC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_DR_AMT_BC_ISMOD" Value='<%# Eval("FTR_DR_AMT_BC_ISMOD") %>' />
                                        <asp:HiddenField runat="server" ID="hdfFTR_TYPE_PK_ISMOD" Value='<%# Eval("FTR_TYPE_PK_ISMOD") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="11%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,AccountName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountName" runat="server" ToolTip='<%# Eval("FTR_ACCOUNT_TEXT") %>'
                                            Text='<%# Eval("FTR_ACCOUNT_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <%--  <FooterTemplate>
                                                    <asp:Panel ID="pnlWHT" runat="server" Visible="false">
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblWHTAmount" Text="WHT Amount" AssociatedControlID="txtWHTAmount"
                                                                Width="83"></asp:Label>
                                                            <asp:TextBox ID="txtWHTAmount" runat="server" TabIndex="20" MaxLength="17" Enabled="false"
                                                                CssClass="Uiinput-amount numeric input-disabled"></asp:TextBox>
                                                      
                                                        </div>
                                                    </asp:Panel>
                                                </FooterTemplate>
                                                <FooterStyle Width="25%" Wrap="false" HorizontalAlign="Left" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Mode %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMode" runat="server" ToolTip='<%# Eval("FTR_PAYMENT_MODE_TEXT") %>'
                                            Text='<%# Eval("FTR_PAYMENT_MODE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                   <asp:TemplateField HeaderText="<%$ resources:Controls,SubAccount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubAccount" runat="server" ToolTip='<%# Eval("FTR_TYPE_TEXT") %>'
                                            Text='<%# Eval("FTR_TYPE_TEXT") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval("FTR_NARRATION") %>'
                                            Text='<%# Eval("FTR_NARRATION") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                    <%--  <FooterTemplate>
                                                    <asp:Panel ID="pnlVatBuy" runat="server" Visible="false">
                                                        <div class="div2col-S" style="float: left; width: 64%">
                                                            <asp:Label runat="server" ID="lblVatBuyAmount" Text="VAT BUY" AssociatedControlID="txtVatBuy"
                                                                Width="55"></asp:Label>
                                                            <asp:TextBox ID="txtVatBuy" runat="server" TabIndex="20" MaxLength="17" Enabled="false"
                                                                CssClass="Uiinput-amount numeric input-disabled"></asp:TextBox>
                                                          
                                                        </div>
                                                    </asp:Panel>
                                                    <div style="float: right; text-align: left; width: 20%">
                                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Controls,Total %>" Width="55" />
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" Wrap="false" HorizontalAlign="Center" />--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,DebitAmt %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_DR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                    <%--  <FooterTemplate>
                                                    <asp:Label ID="lblDebitTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />--%>
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Controls,CreditAmt %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>'
                                            Text='<%# Eval("FTR_CR_AMT_TC","{0:N2}") %>' CssClass="input-w70 numeric" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                    <%--<FooterTemplate>
                                                    <asp:Label ID="lblCreditTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />--%>
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
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
