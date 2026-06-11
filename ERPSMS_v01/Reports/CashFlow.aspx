<%@ Page Title="<%$ Resources:Captions,Title_CashFlow %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CashFlow.aspx.cs" Inherits="ERPSMS_v01.Reports.CashFlow" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("[id$='ChkSelect']").attr('checked', true);
            $('[id$=ChkSelectAll]').attr('checked', true);


            $("[id$='ChkSelectR']").attr('checked', true);
            $('[id$=ChkSelectAllR]').attr('checked', true);


            $("[id$='ChkSelectP']").attr('checked', true);
            $('[id$=ChkSelectAllP]').attr('checked', true);

            $("[id$='ChkSelectE']").attr('checked', true);
            $('[id$=ChkSelectAllE]').attr('checked', true);
        });


        function InitComponents() {

            calculate();
           
            GrandScriptUtils.DatePickerCommon("txtDate");


            $("[id$='ChkSelectAll']").bind('click', function () {
                var status = $(this).is(':checked');
                $("[id$='ChkSelect']").attr('checked', status);
            });


            $("[id$='ChkSelectAllR']").bind('click', function () {
                var status = $(this).is(':checked');
                $("[id$='ChkSelectR']").attr('checked', status);
            });

            $("[id$='ChkSelectAllP']").bind('click', function () {
                var status = $(this).is(':checked');
                $("[id$='ChkSelectP']").attr('checked', status);
            });

            $("[id$='ChkSelectAllE']").bind('click', function () {
                var status = $(this).is(':checked');
                $("[id$='ChkSelectE']").attr('checked', status);
            });


            $("[id$='ChkSelect']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectR']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectP']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectE']").click(function () {
                calculate();
            });


            $("[id$='ChkSelectAll']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectAllR']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectAllP']").click(function () {
                calculate();
            });
            $("[id$='ChkSelectAllE']").click(function () {
                calculate();
            });
        }

        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidCustomer']").hide();
            $("[id$='lblValidBrand']").hide();
            $("[id$='lblValidArtversion']").hide();

            if ($("[id$='lblCustomer']").text() == '' || $("[id$='lblCustomer']").text() == "Select/Type") {
                $("[id$='lblValidCustomer']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Customer") %></li></ul>';
            }

            if ($("[id$='lblBrand']").text() == '' || $("[id$='lblBrand']").text() == "Select/Type") {
                $("[id$='lblValidBrand']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Brand") %></li></ul>';
            }

            if ($("[id$='txtArtworkVersion']").val() == '') {
                $("[id$='lblValidArtversion']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_ArtVersion") %></li></ul>';
            } else {
                if ($("[id$=hdfMode]").val() == 'NEW') {
                    if ($("[id$='txtArtworkVersion']").val() == $("[id$=hdfOldVersion]").val()) {
                        $("[id$='lblValidArtversion']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_MustChangeArtVersion") %></li></ul>';
                    }
                }
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=grdbank]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=grdbank]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
        function ShowHideAdvancedSearchReciept(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=divGridreciept]").show();
                $("[id$=imbShowFilterReciept]").hide();
                $("[id$=imbHideFilterReciept]").show();
            }
            else {
                $("[id$=divGridreciept]").hide();
                $("[id$=imbShowFilterReciept]").show();
                $("[id$=imbHideFilterReciept]").hide();
            }
            return false;
        }
        function ShowHideAdvancedSearchpay(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=divgrdpay]").show();
                $("[id$=imbShowFilterPay]").hide();
                $("[id$=imbHideFilterPay]").show();
            }
            else {
                $("[id$=divgrdpay]").hide();
                $("[id$=imbShowFilterPay]").show();
                $("[id$=imbHideFilterPay]").hide();
            }
            return false;
        }
        function ShowHideAdvancedSearchExp(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=grdExpenses]").show();
                $("[id$=imbShowFilterExp]").hide();
                $("[id$=imbHideFilterExp]").show();
            }
            else {
                $("[id$=grdExpenses]").hide();
                $("[id$=imbShowFilterExp]").show();
                $("[id$=imbHideFilterExp]").hide();
            }

            return false;
        }
        function calculate() {
            var balamt = 0;
            var DecimalDigits = 0;
            var red = '<%= GetLocalResourceObject("Red") %>';
            var blue = '<%= GetLocalResourceObject("Blue") %>';

            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }

            $("#[id*=grdbank] [id*=lblBank_Bal]").each(function (index) {

                if ($(this).closest('tr').find("#[id*=ChkSelect]").is(":checked")) {
                    balamt = balamt + parseFloat($(this).text().replace(/[^0-9\.\-]+/g, ""));
                }

            });

            $("[id$=lblfundval]").html(addCommas(balamt.toFixed(DecimalDigits).toString()));
            if (balamt < 0) { $("[id$=lblfundval]").css("color", red); } else { $("[id$=lblfundval]").css("color", blue); }

            var balamtR = 0;
            $("#[id*=grdreciept] [id*=lblBalanceBC]").each(function (index) {

                if ($(this).closest('tr').find("#[id*=ChkSelectR]").is(":checked")) {
                    balamtR = balamtR + parseFloat($(this).text().replace(/[^0-9\.\-]+/g, ""));
                }

            });

            $("[id$=lblRecievedval]").html(addCommas(balamtR.toFixed(DecimalDigits).toString()));
            if (balamtR < 0) { $("[id$=lblRecievedval]").css("color", red); } else { $("[id$=lblRecievedval]").css("color", blue); }


            var balamtP = 0;
            $("#[id*=grdPayment] [id*=lblBalanceBC]").each(function (index) {

                if ($(this).closest('tr').find("#[id*=ChkSelectP]").is(":checked")) {
                    balamtP = balamtP + parseFloat($(this).text().replace(/[^0-9\.\-]+/g, ""));
                }

            });

 
            var balamtE = 0;
            $("#[id*=grdExpenses] [id*=lblBalanceBC]").each(function (index) {

                if ($(this).closest('tr').find("#[id*=ChkSelectE]").is(":checked")) {
                    balamtE = balamtE + parseFloat($(this).text().replace(/[^0-9\.\-]+/g, ""));
                }

            });
            var balamtPaidval = 0;
            var balNetval = 0;
            var lbldebTotal = 0;
            balamtPaidval = balamtE + balamtP


            $("[id$=lblPaidval]").html(addCommas(balamtPaidval.toFixed(DecimalDigits).toString()));
            if (balamtPaidval < 0) { $("[id$=balamtPaidval]").css("color", red); } else { $("[id$=balamtPaidval]").css("color", blue); }

            balNetval = (balamt + balamtR) - (balamtPaidval);
            $("[id$=lblNetval]").html(addCommas(balNetval.toFixed(DecimalDigits).toString()));
            if (balNetval < 0) { $("[id$=lblNetval]").css("color", red); } else { $("[id$=lblNetval]").css("color", blue); }

            lbldebTotal = balNetval = (balamt + balamtR)

            $("[id$=lbldebTotal]").html(addCommas(lbldebTotal.toFixed(DecimalDigits).toString()));
            if (lbldebTotal < 0) { $("[id$=lbldebTotal]").css("color", red); } else { $("[id$=lbldebTotal]").css("color", blue); }

            $("[id$=lblcreditTotal]").html(addCommas(balamtPaidval.toFixed(DecimalDigits).toString()));
            if (balamtPaidval < 0) { $("[id$=lblcreditTotal]").css("color", red); } else { $("[id$=lblcreditTotal]").css("color", blue); }

         }
        function addCommas(n) {
            var curGroup1 = 3;
            var curGroup2 = 2;
            var result = "";
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
                curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
                curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
            }

            var s = n.split('.')[1];
            (s) ? s = "." + s : s = "";
            n = n.split('.')[0];
            if (n.length > curGroup1) {
                s = "," + n.substr(n.length - curGroup1, curGroup1) + s;
                n = n.substr(0, n.length - curGroup1)
                while (n.length > curGroup2) {
                    s = "," + n.substr(n.length - curGroup2, curGroup2) + s;
                    n = n.substr(0, n.length - curGroup2)
                }
            }
            var i = s.indexOf(",");
            if (i == 0 && n == "-")
                s = s.substr(1, s.length);
            return n + s
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
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
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="Table1">
                        <asp:TableRow>
                            <asp:TableCell ID="TableCell1" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom" id="divTabContainer" runat="server">
                                <asp:Label runat="server" ID="lblDate" Text="<%$ resources:Date %>" AssociatedControlID="txtDate"></asp:Label>
                                <asp:TextBox runat="server" ID="txtDate" Text="" CssClass="Uidate-picker" OnClientClick="javascript:return InitComponents();"
                                    MaxLength="11" onpaste="return false;" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="cashflow"
                                    EnableClientScript="true" runat="server" ControlToValidate="txtDate" Display="Dynamic"
                                    Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="cashflow"
                                    runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ValidDate %>"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>

                                <asp:ImageButton ID="btnGo" SkinID="search-ext" runat="server" 
                                    ToolTip="<%$ resources:Controls,Search %>" CommandName="DEFAULT" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('cashflow')"
                                    TabIndex="2" />
                            </div>
                            <div class="gridwrap">
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%=GetLocalResourceObject("Cashflowbank")%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="3" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="3" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="grdbank">
                                <asp:GridView runat="server" ID="grdBank" Width="100%" Height="350" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAll" runat="server" Checked="true"  TabIndex="5"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelect" runat="server" Checked="true" TabIndex="5" />
                                                <asp:HiddenField ID="hfBPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.Bank_PK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bank_CODE%>" SortExpression="<%$ resources:DataFieldRes,Bank_CODE %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBank_CODE" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Bank_CODE))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Bank_CODE)),17) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bank_Name %>" SortExpression="<%$ resources:DataFieldRes,Bank_Name %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBank_Name" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Bank_Name))%>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Bank_Name),35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Bank_Bal %>" SortExpression="<%$ resources:DataFieldRes,Bank_Bal %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBank_Bal" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.Bank_Bal, "{0:c}") %>'
                                                    Text='<%#  Eval(Resources.DataFieldRes.Bank_Bal, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--  <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetLocalResourceObject("CashflowReciept")%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilterReciept" OnClientClick="javascript:return ShowHideAdvancedSearchReciept(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="6" />
                                            <asp:ImageButton runat="server" ID="imbHideFilterReciept" OnClientClick="javascript:return ShowHideAdvancedSearchReciept();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="6" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="divGridreciept">
                                <asp:GridView runat="server" ID="grdreciept" Width="100%" Height="30%" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAllR" runat="server" Checked="true" TabIndex="8"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelectR" runat="server" Checked="true" TabIndex="8" />
                                                <asp:HiddenField ID="hfRPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.INV_PK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Due_Date%>" SortExpression="<%$ resources:DataFieldRes,Due_Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBank_CODE" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.Due_Date))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.Due_Date, Resources.Constants.DateFormatGrid)),17) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Party %>" SortExpression="<%$ resources:DataFieldRes,Party %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.Party))%>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Party),70) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="57%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="<%$ resources:DataFieldRes,Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.Currency) %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Currency),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AmountTC %>" SortExpression="<%$ resources:DataFieldRes,AmountTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.AmountTC, "{0:c}") %>'
                                                    Text='<%#  Eval(Resources.DataFieldRes.AmountTC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceTC %>" SortExpression="<%$ resources:DataFieldRes,BalanceTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceBC %>" SortExpression="<%$ resources:DataFieldRes,BalanceBC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceBC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceBC, "{0:c}") %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.BalanceBC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl1" runat="server" />--%>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetLocalResourceObject("CashflowPay")%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilterPay" OnClientClick="javascript:return ShowHideAdvancedSearchpay(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="9" />
                                            <asp:ImageButton runat="server" ID="imbHideFilterPay" OnClientClick="javascript:return ShowHideAdvancedSearchpay();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="9" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="divgrdpay">
                                <asp:GridView runat="server" ID="grdPayment" Width="100%" Height="350" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAllP" runat="server" Checked="true"  TabIndex="11" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelectP" runat="server" Checked="true" TabIndex="11" />
                                                <asp:HiddenField ID="hfPPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.INV_PK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Due_Date%>" SortExpression="<%$ resources:DataFieldRes,Due_Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDue_Date" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Due_Date))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Due_Date, Resources.Constants.DateFormatGrid)),17) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Party %>" SortExpression="<%$ resources:DataFieldRes,Party %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Party))%>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Party),70) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="57%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="<%$ resources:DataFieldRes,Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.Currency) %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Currency),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AmountTC %>" SortExpression="<%$ resources:DataFieldRes,AmountTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.AmountTC, "{0:c}")%>'
                                                    Text='<%#  Eval(Resources.DataFieldRes.AmountTC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceTC %>" SortExpression="<%$ resources:DataFieldRes,BalanceTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceBC %>" SortExpression="<%$ resources:DataFieldRes,BalanceBC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceBC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceBC, "{0:c}") %>'
                                                    Text='<%#Eval(Resources.DataFieldRes.BalanceBC, "{0:c}")  %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="PagerControl2" runat="server" />--%>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%=GetLocalResourceObject("CashflowExp")%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilterExp" OnClientClick="javascript:return ShowHideAdvancedSearchExp(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="12" />
                                            <asp:ImageButton runat="server" ID="imbHideFilterExp" OnClientClick="javascript:return ShowHideAdvancedSearchExp();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="12" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="divgrdexpense">
                                <asp:GridView runat="server" ID="grdExpenses" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAllE" runat="server" Checked="true"  TabIndex="14"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelectE" runat="server" Checked="true" TabIndex="14" />
                                                <asp:HiddenField ID="hfEPK" runat="server" Value='<%#Eval(Resources.DataFieldRes.INV_PK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Due_Date%>" SortExpression="<%$ resources:DataFieldRes,Due_Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDue_Date" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Due_Date))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.Due_Date, Resources.Constants.DateFormatGrid)),17) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Party %>" SortExpression="<%$ resources:DataFieldRes,Party %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.Party))%>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Party),70) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="57%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" SortExpression="<%$ resources:DataFieldRes,Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.Currency) %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Currency),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AmountTC %>" SortExpression="<%$ resources:DataFieldRes,AmountTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmountTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.AmountTC , "{0:c}") %>'
                                                    Text='<%#Eval(Resources.DataFieldRes.AmountTC , "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceTC %>" SortExpression="<%$ resources:DataFieldRes,BalanceTC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceTC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>'
                                                    Text='<%#   Eval(Resources.DataFieldRes.BalanceTC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BalanceBC %>" SortExpression="<%$ resources:DataFieldRes,BalanceBC %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalanceBC" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.BalanceBC, "{0:c}")%>'
                                                    Text='<%#  Eval(Resources.DataFieldRes.BalanceBC, "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="PagerControl3" runat="server" />--%>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%=GetLocalResourceObject("Summary")%></h1>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <table class="gridwraptable tble-border">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblfund" Text="<%$ resources:Funds %>" AssociatedControlID="lblfundval"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lblfundval" Text="" TabIndex="15"></asp:Label>
                                        </td>
                                        <td align="right">
                                        </td>
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblRecieved" Text="<%$ resources:Recieved %>" AssociatedControlID="lblRecievedval"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lblRecievedval" Text="" TabIndex="16"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblPaid" Text="<%$ resources:Paid %>" AssociatedControlID="lblPaidval"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lblPaidval" Text="" TabIndex="17"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblNet" Text="<%$ resources:Net %>" AssociatedControlID="lblNetval"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lbldebTotal" Text="" TabIndex="18"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lblcreditTotal" Text="" TabIndex="19"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label runat="server" ID="lblNetval" Text="" TabIndex="20"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="button-wrap-right">
                                <asp:Button ID="Button1" SkinID="btnInner-Print" runat="server" Text="<%$ resources:Controls,Print %>"
                                    ToolTip="<%$ resources:Controls,Print %>" CommandName="PRINT" OnClick="ActionHandler"  OnClientClick="javascript:ValidatePageNow('cashflow')"
                                    TabIndex="21" />
                                    </div>
                            <div id="divReportViewer" class="reportviewer treescroll-x" runat="server">
                                <rsweb:ReportViewer ID="rvViewReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                                    Width="98%">
                                </rsweb:ReportViewer>
                            </div>
                            <div id="divNodata" class="nodata" runat="server" visible="false">
                                No Record Found
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                 <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="cashflow" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="2" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
