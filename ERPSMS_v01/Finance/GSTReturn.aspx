<%@ Page Title="<%$ Resources:Captions,Title_GSTReturn %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="GSTReturn.aspx.cs" Inherits="ERPSMS_v01.Finance.GSTReturn"
    Theme="ClassicExt" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDgiits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function ScrollDown() {
            window.scroll(400, 400);
            return false;
        }
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            currencyDecimal = 2;
            if (!isNaN(parseInt($("[id$=hdfDecimalDigits]").val()))) {
                currencyDecimal = parseInt($("[id$=hdfDecimalDigits]").val());
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > currencyDecimal - 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        function toFixed(num, precision) {
            return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
        }
        function CalculateOutputTax(sender) {
            //Calculate Values
            var TaxValue1 = parseFloat($("[id$=txtTaxCodeValue1]").val().replace(new RegExp(',', 'g'), ''));
            TaxValue1 = isNaN(TaxValue1) ? 0 : TaxValue1;
            var TaxValue2 = parseFloat($("[id$=txtTaxCodeValue2]").val().replace(new RegExp(',', 'g'), ''));
            TaxValue2 = isNaN(TaxValue2) ? 0 : TaxValue2;
            var TaxValue3 = parseFloat($("[id$=txtTaxCodeValue3]").val().replace(new RegExp(',', 'g'), ''));
            TaxValue3 = isNaN(TaxValue3) ? 0 : TaxValue3;
            var TaxValue4 = parseFloat($("[id$=txtTaxCodeValue4]").val().replace(new RegExp(',', 'g'), ''));
            TaxValue4 = isNaN(TaxValue4) ? 0 : TaxValue4;
            var TaxValue5 = parseFloat($("[id$=txtTaxCodeValue5]").val().replace(new RegExp(',', 'g'), ''));
            TaxValue5 = isNaN(TaxValue5) ? 0 : TaxValue5;
            var TaxOtherValue = parseFloat($("[id$=txtTaxCodeOtherValue]").val().replace(new RegExp(',', 'g'), ''));
            TaxOtherValue = isNaN(TaxOtherValue) ? 0 : TaxOtherValue;
            var TaxTotalValue = TaxValue1 + TaxValue2 + TaxValue3 + TaxValue4 + TaxValue5 + TaxOtherValue; //Calculate Total
            TaxTotalValue = isNaN(TaxTotalValue) ? 0 : TaxTotalValue;
            //Print Values
            $("[id$=txtTaxCodeValue1]").val((TaxValue1).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue2]").val((TaxValue2).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue3]").val((TaxValue3).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue4]").val((TaxValue4).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue5]").val((TaxValue5).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeOtherValue]").val((TaxOtherValue).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeTotalValue]").val((TaxTotalValue).toFixed(CurrencyDigits)); //Print Total
            //Calculate and print percentage
            if (TaxTotalValue > 0) {
                $("[id$=txtTaxCodePer1]").val(((TaxValue1 * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer2]").val(((TaxValue2 * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer3]").val(((TaxValue3 * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer4]").val(((TaxValue4 * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer5]").val(((TaxValue5 * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodeOtherPer]").val(((TaxOtherValue * 100) / TaxTotalValue).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodeTotalPer]").val(((TaxTotalValue * 100) / TaxTotalValue).toFixed(CurrencyDigits));
            }
            else {
                $("[id$=txtTaxCodePer1]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer2]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer3]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer4]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodePer5]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodeOtherPer]").val((0).toFixed(CurrencyDigits));
                $("[id$=txtTaxCodeTotalPer]").val((0).toFixed(CurrencyDigits));
            }
        }
        function DefaultOutputTax() {
            //print values,total and percentages as 0.00
            $("[id$=txtTaxCodeValue1]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue2]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue3]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue4]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeValue5]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodePer1]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodePer2]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodePer3]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodePer4]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodePer5]").val((0).toFixed(CurrencyDigits));
            var TaxCodeOtherValue = parseFloat($("[id$=txtPartB_Quest5Sub2]").val().replace(new RegExp(',', 'g'), ''));
            TaxCodeOtherValue = isNaN(TaxCodeOtherValue) ? 0 : TaxCodeOtherValue;

            $("[id$=txtTaxCodeOtherValue]").val((TaxCodeOtherValue).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeOtherPer]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeTotalValue]").val((0).toFixed(CurrencyDigits));
            $("[id$=txtTaxCodeTotalPer]").val((0).toFixed(CurrencyDigits));
        }
        function StartDateConfig(flag1) {
            if (flag1 == 1) {
                var dt = $("[id$=hdfSavedStartDate]").val();
                GrandScriptUtils.DatePickerCommon("txtStartDate", "dd-M-yy", true,false,dt);
            }
            else if (flag1 == 2) {
                GrandScriptUtils.DatePickerCommon("txtStartDate");
            }
            return false;
        }
        function InitComponents() {
            //DefaultOutputTax();
            ShowHidePartItems("1", "tblShowOrHidePartA", "imbShowFilterPartA", "imbHideFilterPartA");
            ShowHidePartItems("1", "tblShowOrHidePartB", "imbShowFilterPartB", "imbHideFilterPartB");
            ShowHidePartItems("", "tblShowOrHidePartC", "imbShowFilterPartC", "imbHideFilterPartC");
            ShowHidePartItems("", "tblShowOrHidePartD", "imbShowFilterPartD", "imbHideFilterPartD");
            GrandScriptUtils.DatePickerCommon("txtReturnableDate");
            GrandScriptUtils.DatePickerCommon("txtDeclDate");
            GrandScriptUtils.AddDateRangeCommon("txtStartDate", "hdfStartDate", "txtEndDate", "hdfEndDate", "dd-M-yy", false, false, false);
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
        }
        function ShowHidePartItems(flagI, TblId, ShowId, HideId) {
            //If flag then Show Items
            if (flagI) {
                $("[id$=" + TblId + "]").show();
                $("[id$=" + ShowId + "]").show();
                $("[id$=" + HideId + "]").hide();
            }
            else {
                $("[id$=" + TblId + "]").hide();
                $("[id$=" + ShowId + "]").hide();
                $("[id$=" + HideId + "]").show();
            }
            return false;
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }
        function PageViewMode(mode) {
            //1 = NewMode
            if (mode == 1) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" Width="200px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ActionHandler"  onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="51"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="52" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="inv" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="53" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="54" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="55" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="56" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="56" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="59" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li id="pnlListPrint">
                                        <asp:Button runat="server" TabIndex="60" ID="btnListPrint" CommandName="PRINTLIST"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--<div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="43" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="44" OnClick="ActionHandler" CommandName="DETAILS"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdGSTReturnList" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="58" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfGSTReturnID" Value='<%# Eval("TGH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfCompany" Value='<%# Eval("TGH_COMPANY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("TGH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHTrNo %>" SortExpression="TGH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrNo" runat="server" Text='<%# Eval("TGH_NO")==""?"[NEW]":Eval("TGH_NO")%>' ToolTip='<%# Eval("TGH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHStartDate %>" SortExpression="TGH_FROM_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("TGH_FROM_DATE", Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval("TGH_FROM_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHEndDate %>" SortExpression="TGH_TO_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("TGH_TO_DATE", Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval("TGH_TO_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHDueDate %>" SortExpression="TGH_DUE_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("TGH_DUE_DATE", Resources.Constants.DateFormatGrid)%>'
                                                    ToolTip='<%# Eval("TGH_DUE_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHTaxPayable %>" SortExpression="TGH_TAX_PAYABLE_AMT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaxPayable" runat="server" Text='<%# GetFormattedCurrency(Eval("TGH_TAX_PAYABLE_AMT", "{0:c}")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("TGH_TAX_PAYABLE_AMT", "{0:c}")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GHTaxClaimable %>" SortExpression="TGH_TAX_CLAIMABLE_AMT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaxClaimable" runat="server" Text='<%# GetFormattedCurrency(Eval("TGH_TAX_CLAIMABLE_AMT", "{0:c}")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("TGH_TAX_CLAIMABLE_AMT", "{0:c}")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("TGH_CSS_CLASS")%>' ToolTip='<%# Eval("TGH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved1" Value='<%# Eval("TGH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                            <table style="height: 100px; margin-top: 0px;">
                                <tr>
                                    <td style="width: 100px">
                                        <img src="../Images/Demo/malysia-customs.png" alt="Logo" width="100px" height="100px" />
                                    </td>
                                    <td align="center" style="letter-spacing: 2px;">
                                        <div><b>JABATAN KASTAM DIRAJA MALAYSIA</b></div>
                                        <div style="margin-bottom: 15px"><b>ROYAL MALAYSIAN CUSTOMS DEPARTMENT</b></div>
                                        <div>PENYATA CUKAI BARANG DAN PERKHIDMATAN</div>
                                        <div>GOODS AND SEVRVICES TAX RETURN</div>
                                    </td>
                                    <td style="width: 150px; padding-top: 15px" align="center" valign="middle">
                                        <div style="background-color: #DEE3ED; color: #506C92; font-weight: bold; height: 25px;margin-bottom: 5px">
                                            <%= GetLocalResourceObject("GSTReturnName").ToString()%></div>
                                        <div style="text-align: center;">
                                            <asp:TextBox ID="txtGSTValueNo" runat="server" Width="150" Height="25" CssClass="txtAlign-center"
                                                onkeydown="return CheckKey(event)"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td><h1 class="search-colapse-normal"><%= GetLocalResourceObject("PartA_Title").ToString()%></h1></td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbHideFilterPartA" OnClientClick="javascript:return ShowHidePartItems('1', 'tblShowOrHidePartA', 'imbShowFilterPartA', 'imbHideFilterPartA');"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbShowFilterPartA" OnClientClick="javascript:return ShowHidePartItems('', 'tblShowOrHidePartA', 'imbShowFilterPartA', 'imbHideFilterPartA');"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="tblShowOrHidePartA">
                                <table class="gridwraptable gridwrap">
                                    <tr>
                                        <td><asp:Label ID="lblPartA_Quest1" Text="<%$resources:PartA_Quest1%>" runat="server" /></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPartA_Quest1" Width="300px" CssClass="input-disabled"
                                                onkeydown="return CheckKey(event)" TabIndex="1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartA_Quest2" Text="<%$resources:PartA_Quest2%>" runat="server" /></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPartA_Quest2" Width="590px" CssClass="input-disabled"
                                                onkeydown="return CheckKey(event)"  TabIndex="2"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td><h1 class="search-colapse-normal"><%= GetLocalResourceObject("PartB_Title").ToString()%></h1></td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbHideFilterPartB" OnClientClick="javascript:return ShowHidePartItems('1', 'tblShowOrHidePartB', 'imbShowFilterPartB', 'imbHideFilterPartB');"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="3" />
                                            <asp:ImageButton runat="server" ID="imbShowFilterPartB" OnClientClick="javascript:return ShowHidePartItems('', 'tblShowOrHidePartB', 'imbShowFilterPartB', 'imbHideFilterPartB');"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="3" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="tblShowOrHidePartB">
                                <table class="gridwraptable gridwrap" width="100%">
                                    <tr>
                                        <td width="35%"><asp:Label ID="lblPartB_Quest3" Text="<%$resources:PartB_Quest3%>" runat="server" /></td>
                                        <td width="10%">
                                            <asp:Label ID="lblPartB_StartDate" Text="<%$resources:PartB_StartDate%>" runat="server"
                                                AssociatedControlID="txtStartDate" />
                                        </td>
                                        <td width="20%">
                                            <asp:TextBox runat="server" ID="txtStartDate" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"  TabIndex="3"/>
                                            <asp:RequiredFieldValidator ID="vrftxtStartDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="selectDate" EnableClientScript="true" runat="server" ControlToValidate="txtStartDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_StartDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfStartDate" runat="server" />
                                            <asp:HiddenField ID="hdfSavedStartDate" runat="server" />
                                        </td>
                                        <td width="10%"><asp:Label ID="lblPartB_EndDate" Text="<%$resources:PartB_EndDate%>" runat="server" /></td>
                                        <td width="20%">
                                            <asp:TextBox runat="server" ID="txtEndDate" onkeydown="return CheckKey(event)" 
                                                onpaste="return false;"  TabIndex="4"/>
                                            <asp:RequiredFieldValidator ID="vrftxtEndDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="selectDate" EnableClientScript="true" runat="server" ControlToValidate="txtEndDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EndDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfEndDate" runat="server" />
                                        </td>
                                        <td width="5%">
                                            <asp:Button runat="server" ID="btnSet" CommandName="SET" Text="<%$resources:ErpRes,Go %>"
                                                TabIndex="5" ValidationGroup="selectDate" ToolTip="<%$resources:ErpRes,Go %>"
                                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" SkinID="btnInner-set" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><asp:Label ID="lblPartB_Quest4" Text="<%$resources:PartB_Quest4%>" runat="server" /></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtReturnableDate" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"  TabIndex="6"/>
                                        </td>
                                        <td colspan="3"></td>
                                    </tr>
                                </table>
                                <table class="gridwraptable gridwrap" width="100%">
                                    <tr>
                                        <th align="left" width="35%"><%= GetLocalResourceObject("PartB_Quest5").ToString()%></th>
                                        <th class="amount-numeric" width="10%"></th>
                                        <th width="20%"></th>
                                        <th width="10%"></th>
                                        <th width="20%"><%= GetLocalResourceObject("PartB_Quest5Amount").ToString()%></th>
                                        <th width="5%"></th>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest5Sub1" Text="<%$resources:PartB_Quest5Sub1%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartB_Quest5Sub1RM" Text="<%$resources:RMUnit%>" runat="server"
                                                AssociatedControlID="txtPartB_Quest5Sub1" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest5Sub1" CssClass="numeric" ClientIDMode="Static"  TabIndex="7"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest5Sub2" Text="<%$resources:PartB_Quest5Sub2%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="PartB_Quest5Sub1RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartB_Quest5Sub2" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest5Sub2" CssClass="numeric" ClientIDMode="Static"  TabIndex="8"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <th align="left" width="35%"><%= GetLocalResourceObject("PartB_Quest6").ToString()%></th>
                                        <th width="10%"></th>
                                        <th width="20%"></th>
                                        <th width="10%"></th>
                                        <th width="20%"><%= GetLocalResourceObject("PartB_Quest6Amount").ToString()%></th>
                                        <th width="5%"></th>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest6Sub1" Text="<%$resources:PartB_Quest6Sub1%>" runat="server"  /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartB_Quest6Sub1RM" Text="<%$resources:RMUnit%>" runat="server"
                                                AssociatedControlID="txtPartB_Quest6Sub1" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest6Sub1" CssClass="numeric" ClientIDMode="Static"  TabIndex="10"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest6Sub2" Text="<%$resources:PartB_Quest6Sub2%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartB_Quest6Sub2RM" Text="<%$resources:RMUnit%>" runat="server"
                                                AssociatedControlID="txtPartB_Quest6Sub2" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest6Sub2" CssClass="numeric" ClientIDMode="Static"  TabIndex="11"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest7" Text="<%$resources:PartB_Quest7%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartB_Quest7RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartB_Quest7" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest7" CssClass="numeric" ClientIDMode="Static"  TabIndex="12"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest8" Text="<%$resources:PartB_Quest8%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartB_Quest8RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartB_Quest8" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartB_Quest8" CssClass="numeric" ClientIDMode="Static"  TabIndex="13"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblPartB_Quest9" Text="<%$resources:PartB_Quest9%>" runat="server" /></td>
                                        <td></td>
                                        <td></td>
                                        <td align="right"><asp:CheckBox ID="chkPartB_Quest9" runat="server" Checked="false" TabIndex="14"/></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td><h1><%= GetLocalResourceObject("PartC_Title").ToString()%></h1></td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbHideFilterPartC" OnClientClick="javascript:return ShowHidePartItems('1', 'tblShowOrHidePartC', 'imbShowFilterPartC', 'imbHideFilterPartC');"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="14" />
                                            <asp:ImageButton runat="server" ID="imbShowFilterPartC" OnClientClick="javascript:return ShowHidePartItems('', 'tblShowOrHidePartC', 'imbShowFilterPartC', 'imbHideFilterPartC');"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="14" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="tblShowOrHidePartC">
                                <table class="gridwraptable gridwrap" width="100%">
                                    <tr>
                                        <td width="35%"><asp:Label ID="lblPartC_Quest10" Text="<%$resources:PartC_Quest10%>" runat="server" /></td>
                                        <td width="10%"></td>
                                        <td width="20%"></td>
                                        <td width="10%" align="right">
                                            <asp:Label ID="lblPartC_Quest10RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest10" />
                                        </td>
                                        <td width="20%"><asp:TextBox runat="server" ID="txtPartC_Quest10" CssClass="numeric"  TabIndex="15"/></td>
                                        <td width="5%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest11" Text="<%$resources:PartC_Quest11%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest11RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest11" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest11" CssClass="numeric"  TabIndex="16"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest12" Text="<%$resources:PartC_Quest12%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest12RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest12" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest12" CssClass="numeric"  TabIndex="17"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest13" Text="<%$resources:PartC_Quest13%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest13RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest13" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest13" CssClass="numeric"  TabIndex="18"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest14" Text="<%$resources:PartC_Quest14%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest14RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest14" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest14" CssClass="numeric" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest15" Text="<%$resources:PartC_Quest15%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest15RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest15" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest15" CssClass="numeric"  TabIndex="19"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest16" Text="<%$resources:PartC_Quest16%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest16RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest16" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest16" CssClass="numeric"  TabIndex="20"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest17" Text="<%$resources:PartC_Quest17%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest17RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest17" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest17" CssClass="numeric"  TabIndex="21"/></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:Label ID="lblPartC_Quest18" Text="<%$resources:PartC_Quest18%>" runat="server" /></td>
                                        <td align="right">
                                            <asp:Label ID="lblPartC_Quest18RM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtPartC_Quest18" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtPartC_Quest18" CssClass="numeric"  TabIndex="22"/></td>
                                        <td></td>
                                    </tr>
                                </table>
                                <h1 class="search-colapse-normal"><%= GetLocalResourceObject("PartC_Quest19").ToString()%></h1>
                                <table class="gridwraptable gridwrap">
                                    <thead>
                                        <tr>
                                            <th align="left" width="35%"><%= GetLocalResourceObject("PartC_Quest19Head1").ToString()%></th>
                                            <th width="10%"></th>
                                            <th align="left" width="20%"><%= GetLocalResourceObject("PartC_Quest19Head2").ToString()%></th>
                                            <th width="10%"></th>
                                            <th align="left"><%= GetLocalResourceObject("PartC_Quest19Head3").ToString()%></th>
                                            <th width="5%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtTaxCode1" Text="<%$resources:TaxCode1%>" Width="60px" TabIndex="23"/></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeRM1" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeValue1" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeValue1" CssClass="numeric" Width="60px"  TabIndex="24"
                                                    MaxLength="16" onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodePer1" CssClass="numeric" onkeydown="return CheckKey(event)"
                                                    Width="60px"  TabIndex="25"/>
                                                <asp:Label ID="lblTaxCodePer1" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodePer1" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtTaxCode2" Text="<%$resources:TaxCode2%>" Width="60px"  TabIndex="26"/></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeRM2" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeValue2" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeValue2" CssClass="numeric" Width="60px"  TabIndex="27"
                                                    MaxLength="16" onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodePer2" CssClass="numeric" Width="60px"  TabIndex="28" onkeydown="return CheckKey(event)" />
                                                <asp:Label ID="lblTaxCodePer2" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodePer2" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtTaxCode3" Text="<%$resources:TaxCode3%>" Width="60px"  TabIndex="29"/></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeRM3" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeValue3" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeValue3" CssClass="numeric" Width="60px"  TabIndex="30"
                                                    MaxLength="16" onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodePer3" CssClass="numeric" Width="60px"  TabIndex="31" onkeydown="return CheckKey(event)" />
                                                <asp:Label ID="lblTaxCodePer3" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodePer3" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtTaxCode4" Text="<%$resources:TaxCode4%>" Width="60px"  TabIndex="32"/></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeRM4" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeValue4" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeValue4" CssClass="numeric" Width="60px"  TabIndex="33"
                                                    MaxLength="16" onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodePer4" CssClass="numeric" Width="60px"  TabIndex="34" onkeydown="return CheckKey(event)" />
                                                <asp:Label ID="lblTaxCodePer4" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodePer4" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox runat="server" ID="txtTaxCode5" Text="<%$resources:TaxCode5%>" Width="60px"  TabIndex="35"/></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeRM5" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeValue5" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeValue5" CssClass="numeric" Width="60px"  TabIndex="36"
                                                    MaxLength="16" onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodePer5" CssClass="numeric" Width="60px"  TabIndex="37" onkeydown="return CheckKey(event)" />
                                                <asp:Label ID="lblTaxCodePer5" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodePer5" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTaxCodeOther" Text="<%$resources:TaxCodeOther%>" runat="server"
                                                    Width="60px" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeOtherRM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeOtherValue" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeOtherValue" CssClass="numeric" Width="60px"  TabIndex="38"
                                                    onkeypress="return validateFloatKeyPress(this,event);" onchange="return CalculateOutputTax(this);" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTaxCodeOtherPer" CssClass="numeric" Width="60px"  TabIndex="39"
                                                    onkeydown="return CheckKey(event)" />
                                                <asp:Label ID="lblTaxCodeOtherPer" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodeOtherPer" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="lblTaxCodeTotal" Text="<%$resources:TaxCodeTotal%>" runat="server" /></td>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTaxCodeTotalRM" Text="<%$resources:RMUnit%>" runat="server" AssociatedControlID="txtTaxCodeTotalValue" />
                                                <asp:TextBox runat="server" ID="txtTaxCodeTotalValue" CssClass="numeric" Width="60px"  TabIndex="40"
                                                    onkeydown="return CheckKey(event)" />
                                            </td>
                                            <td></td>
                                            <td align="left">
                                                <asp:TextBox runat="server" ID="txtTaxCodeTotalPer" CssClass="numeric input-disabled"  TabIndex="41"
                                                    onkeydown="return CheckKey(event)" Width="60px" />
                                                <asp:Label ID="lblTaxCodeTotalPer" Text="<%$resources:PercentageSymbol%>" runat="server"
                                                    AssociatedControlID="txtTaxCodeTotalPer" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td><h1><%= GetLocalResourceObject("PartD_Title").ToString()%></h1></td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbHideFilterPartD" OnClientClick="javascript:return ShowHidePartItems('1', 'tblShowOrHidePartD', 'imbShowFilterPartD', 'imbHideFilterPartD');"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="42" />
                                            <asp:ImageButton runat="server" ID="imbShowFilterPartD" OnClientClick="javascript:return ShowHidePartItems('', 'tblShowOrHidePartD', 'imbShowFilterPartD', 'imbHideFilterPartD');"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="42" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="tblShowOrHidePartD">
                                <p><%= GetLocalResourceObject("PartD_Sub1").ToString()%></p>
                                <table class="gridwraptable gridwrap">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeclName" Text="<%$resources:DeclName%>" runat="server" AssociatedControlID="txtDeclName" />
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox runat="server" EnableTheming="false" ID="txtDeclName" TabIndex="43" Width="455px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td colspan="4"><%= GetLocalResourceObject("DeclIdentityCardNewText").ToString()%></td>
                                        <td align="left"><%= GetLocalResourceObject("DeclIdentityCardOldText").ToString()%></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeclIdentityCard" Text="<%$resources:DeclIdentityCard%>" runat="server"
                                                AssociatedControlID="txtDeclIdentityCardNewPart1" />
                                        </td>
                                        <td style="width: 90px">
                                            <asp:TextBox runat="server" ID="txtDeclIdentityCardNewPart1" Width="80px"  TabIndex="44"/>
                                        </td>
                                        <td style="width: 50px">
                                            <asp:Label ID="lblDeclIdentityCardNewSep1" Text="<%$resources:DeclIdentityCardNewSep%>"
                                                runat="server" AssociatedControlID="txtDeclIdentityCardNewPart2" />
                                            <asp:TextBox runat="server" Width="25px" ID="txtDeclIdentityCardNewPart2"  TabIndex="45"/>
                                        </td>
                                        <td style="width: 120px">
                                            <asp:Label ID="lblDeclIdentityCardNewSep2" Text="<%$resources:DeclIdentityCardNewSep%>"
                                                runat="server" AssociatedControlID="txtDeclIdentityCardNewPart3" />
                                            <asp:TextBox runat="server" ID="txtDeclIdentityCardNewPart3" Width="80px"  TabIndex="46"/>
                                        </td>
                                        <td style="width: 25px">
                                            <asp:Label ID="lblDeclIdentityCardOR" Text="<%$resources:DeclIdentityCardOR%>" runat="server"
                                                AssociatedControlID="txtDeclIdentityCardOld" />
                                        </td>
                                        <td><asp:TextBox runat="server" ID="txtDeclIdentityCardOld"  TabIndex="47"/></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeclPassportNo" Text="<%$resources:DeclPassportNo%>" runat="server"
                                                AssociatedControlID="txtDeclPassportNo" />
                                        </td>
                                        <td colspan="5"><asp:TextBox runat="server" ID="txtDeclPassportNo" Width="255px"  TabIndex="48"/></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Label ID="lblDeclMandatoryText" Text="<%$resources:DeclMandatoryText%>" runat="server"
                                                CssClass="inputFull" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeclNationality" Text="<%$resources:DeclNationality%>" runat="server"
                                                AssociatedControlID="txtDeclNationality" />
                                        </td>
                                        <td colspan="5"><asp:TextBox runat="server" ID="txtDeclNationality" Width="255px" TabIndex="49"/></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDeclDate" Text="<%$resources:DeclDate%>" runat="server" AssociatedControlID="txtDeclDate" />
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox runat="server" ID="txtDeclDate" onkeydown="return CheckKey(event)" onpaste="return false;"  TabIndex="50"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="5">
                                            <asp:Label ID="lblDeclDateTypeText" Text="<%$resources:DeclDateTypeText%>" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table><div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="invoice" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                    <asp:ValidationSummary ID="vsDeduction" ValidationGroup="deduction" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfSaveWithoutAllocation" runat="server" />
                    <asp:HiddenField ID="hdfgroup" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfGSTValueNo" runat="server" />
                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                    <asp:HiddenField ID="AST_CODE" runat="server" />
                </div>
                <%--User Control--%>
                <div id="divWkfSubmit" style="display: none;">
                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                    <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice"></uc1:WorkflowUserComments>
                </div>
                <asp:HiddenField ID="hdfIscontYes" runat="server" />
                <asp:HiddenField ID="hdfAmtTC" runat="server" Value="0.0" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                <asp:HiddenField ID="hdfExchangeDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
                <asp:HiddenField ID="hdfLastModDate" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
