<%@ Page Title="<%$ Resources:Captions,Title_Voucher %>" Language="C#" Theme="Classic"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="VoucherOld.aspx.cs" Inherits="ERPSMS_v01.Journalize.VoucherOld" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtVoucherDate");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.DatePickerCommon("txtInstrDate");
            GrandScriptUtils.DatePickerCommon("txtInstrDate1");

            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?VoucherType=" + $("[id$='hdfVoucherType']").val() + "&AccType=1", "hdfAccount", true, true, "ACCOUNT");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtAccount1", url + "?VoucherType=" + $("[id$='hdfVoucherType']").val() + "&AccType=2", "hdfAccount1", true, true, "ACCOUNT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "&AccType=0", "hdfAccount", true, true, "JOURNALACCOUNT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccount1", url + "&AccType=0", "hdfAccount1", true, true, "JOURNALACCOUNT");

            $("[id$='txtAmount']").ForceNumericOnly();
            $("[id$='txtDebit1']").ForceNumericOnly();
            $("[id$='txtCredit1']").ForceNumericOnly();

            HideAllValidations();
            $("[id$='txtCurrency']").attr("disabled", "disabled");
            $("[id$='txtCurrency']").next().hide();
        }

        function HideAllValidations() {
            $("[id$='lblValidAccount']").hide();
            $("[id$='lblValidMode']").hide();
            $("[id$='lblValidAmount']").hide();
            $("[id$='lblValidInstrNo']").hide();
            $("[id$='lblValidInstrDate']").hide();
            $("[id$='lblValidFavourOf']").hide();

            $("[id$='lblValidAccount1']").hide();
            $("[id$='lblValidMode1']").hide();
            $("[id$='lblValidDebit1']").hide();
            $("[id$='lblValidCredit1']").hide();
            $("[id$='lblValidInstrNo1']").hide();
            $("[id$='lblValidInstrDate1']").hide();
            $("[id$='lblValidFavourOf1']").hide();
        }

        function ShowInstrDetails() {
            if ($("[id$='ddlMode']").val() == "201" || $("[id$='ddlMode']").val() == "-1") {
                $("[id$='txtInstrNo']").val('');
                $("[id$='txtInstrDate']").val('');
                $("[id$='txtFavourOf']").val('');

                $("[id$='trInstrDet']").hide();
            }
            else {
                $("[id$='trInstrDet']").show();
            }
        }

        function ShowInstrDetails1() {
            if ($("[id$='ddlMode1']").val() == "201" || $("[id$='ddlMode1']").val() == "-1") {
                $("[id$='txtInstrNo1']").val('');
                $("[id$='txtInstrDate1']").val('');
                $("[id$='txtFavourOf1']").val('');
                $("[id$='trInstrDet1']").hide();
            }
            else {
                $("[id$='trInstrDet1']").show();
            }
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For Amount validation
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

        function ValidateAddItems(valGroup) {
            var isValid = true;
            var msg = "";
            HideAllValidations();

            if (valGroup == "AddItem") {
                if ($("[id$='hdfAccount']").val() == '' || $("[id$='hdfAccount']").val() == '-1') {
                    $("[id$='lblValidAccount']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Account") %></ul></li>';
                }

                if ($("[id$='ddlMode']").val() == "-1") {
                    $("[id$='lblValidMode']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Mode") %></ul></li>';
                }

                if ($("[id$='txtAmount']").val() == '' || $("[id$='txtAmount']").val() == '0') {
                    $("[id$='lblValidAmount']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Amount") %></ul></li>';
                }

                if ($("[id$='ddlMode']").val() != "201" && $("[id$='ddlMode']").val() != "-1") {
                    if ($("[id$='txtInstrNo']").val() == '') {
                        $("[id$='lblValidInstrNo']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrNo") %></ul></li>';
                    }

                    if ($("[id$='txtInstrDate']").val() == '') {
                        $("[id$='lblValidInstrDate']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrDate") %></ul></li>';
                    }

                    if ($("[id$='txtFavourOf']").val() == '') {
                        $("[id$='lblValidFavourOf']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_FavourOf") %></ul></li>';
                    }
                }
            }
            else {
                if ($("[id$='hdfAccount1']").val() == '' || $("[id$='hdfAccount1']").val() == '-1') {
                    $("[id$='lblValidAccount1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Account") %></ul></li>';
                }

                if ($("[id$='ddlMode1']").val() == "-1") {
                    $("[id$='lblValidMode1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Mode") %></ul></li>';
                }

                if (($("[id$='txtDebit1']").val() == '' || $("[id$='txtDebit1']").val() == '0')
                && ($("[id$='txtCredit1']").val() == '' || $("[id$='txtCredit1']").val() == '0')) {
                    $("[id$='lblValidDebit1']").show();
                    $("[id$='lblValidCredit1']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Amount") %></ul></li>';
                }

                if ($("[id$='ddlMode1']").val() != "201" && $("[id$='ddlMode1']").val() != "-1") {
                    if ($("[id$='txtInstrNo1']").val() == '') {
                        $("[id$='lblValidInstrNo1']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrNo") %></ul></li>';
                    }

                    if ($("[id$='txtInstrDate1']").val() == '') {
                        $("[id$='lblValidInstrDate1']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_InstrDate") %></ul></li>';
                    }

                    if ($("[id$='txtFavourOf1']").val() == '') {
                        $("[id$='lblValidFavourOf1']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_FavourOf") %></ul></li>';
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

        function SetDebitCreditAmt(val) {
            //val = 1 then Debit change else Credit change
            if (val && $("[id$='txtCredit1']").val() != '') {
                $("[id$='txtCredit1']").val('0');
            }
            else if (!val && $("[id$='txtDebit1']").val() != '') {
                $("[id$='txtDebit1']").val('0');
            }
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCurrency") {
                $("[id$='btnCurrency']").click();
            }
            else if (targetControlID == "txtAccount1") {
                $("[id$='btnAccount1']").click();
            }
            else if (targetControlID == "txtAccount") {
                $("[id$='btnAccount']").click();
            }
            
        }

        function ShowHideSections(type) {
            if (type == 1) {
                $("[id$='hHead2']").html("<%= Resources.Controls.Accounts %>");
                $("[id$='divFirstPart']").hide();
                $("[id$='thDebit']").show();
                $("[id$='thDebit']").attr("width", "10%");
                $("[id$='tdDebit1']").show();
                $("[id$='tdDebit2']").show();
                $("[id$='thCredit']").show();
                $("[id$='thCredit']").attr("width", "10%");
                $("[id$='tdCredit1']").show();
                $("[id$='tdCredit2']").show();
            }
            if (type == 2) {
                $("[id$='hHead1']").html("<%= Resources.Controls.DebitAccounts %>");
                $("[id$='hHead2']").html("<%= Resources.Controls.CreditAccounts %>");
                $("[id$='thAmount']").html("<%= Resources.Controls.DebitAmt %>");
                $("[id$='divFirstPart']").show();
                $("[id$='thDebit']").hide();
                $("[id$='tdDebit1']").hide();
                $("[id$='tdDebit2']").hide();
                $("[id$='thCredit']").show();
                $("[id$='thCredit']").attr("width", "20%");
                $("[id$='tdCredit1']").show();
                $("[id$='tdCredit2']").show();
            }
            else if (type == 3) {
                $("[id$='hHead1']").html("<%= Resources.Controls.CreditAccounts %>");
                $("[id$='hHead2']").html("<%= Resources.Controls.DebitAccounts %>");
                $("[id$='thAmount']").html("<%= Resources.Controls.CreditAmt %>");
                $("[id$='divFirstPart']").show();
                $("[id$='thDebit']").show();
                $("[id$='thDebit']").attr("width", "20%");
                $("[id$='tdDebit1']").show();
                $("[id$='tdDebit2']").show();
                $("[id$='thCredit']").hide();
                $("[id$='tdCredit1']").hide();
                $("[id$='tdCredit2']").hide();
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSubmit]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlVoucher" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server">
                                    <li id="pnlSubmit" runat="server" style="display:none" >
                                        <asp:Button ID="btnSubmit" runat="server" CommandName="SUBMIT" Text="<%$ resources:ErpRes,Submit %>"
                                            OnClientClick="javascript:ValidatePageNow('Voucher')" ValidationGroup="Voucher"
                                            SkinID="btnInner-submit" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" TabIndex="29" />
                                    </li>
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidatePageNow('Voucher')" ValidationGroup="Voucher"
                                            SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" ToolTip="<% $resources:Controls,Save %>"
                                            OnClick="ActionHandler" TabIndex="27" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$ resources:Controls,Cancel %>"
                                            SkinID="btnInner-Cancel" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" TabIndex="28" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow></asp:Table></div></div><div class="content-wrapper">
                <asp:HiddenField ID="hdfVoucherType" runat="server" />
                <asp:HiddenField ID="hdfVecPk" runat="server" />
                <asp:HiddenField ID="hdfDebitTotal" runat="server" />
                <asp:HiddenField ID="hdfCreditTotal" runat="server" />
                <asp:HiddenField ID="hdfVoucherNo" runat="server" />
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVoucherNo" runat="server" Text="<%$ resources:Controls,VoucherNo %>"
                                                AssociatedControlID="txtVoucherNo" />
                                            <asp:TextBox ID="txtVoucherNo" runat="server" MaxLength="100" Enabled="false"
                                                CssClass="medium input-disabled" TabIndex="1" />
                                                <asp:HiddenField ID ="AST_DOC_MODE" runat ="server" Value ="0" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblRefNo" runat="server" Text="<%$ resources:Controls,RefNo %>" AssociatedControlID="txtRefNo" />
                                            <asp:TextBox ID="txtRefNo" runat="server" MaxLength="200" CssClass="medium" TabIndex="3" />
                                            <asp:RequiredFieldValidator ID="vrfRefNo" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtRefNo" ErrorMessage="<%$ resources:Err_RefNo %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCurrency" runat="server" Text="<%$ resources:Controls,Currency %>"
                                                AssociatedControlID="txtCurrency" />
                                            <asp:TextBox ID="txtCurrency" runat="server" MaxLength="100" CssClass="medium" TabIndex="5" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="Voucher"
                                                EnableClientScript="true" Display="Dynamic" Text="*" ControlToValidate="txtCurrency"
                                                ErrorMessage="<%$ resources:Err_Currency %>" />
                                            <asp:Button ID="btnCurrency" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ACTIVATE" />
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVoucherDate" runat="server" Text="<%$ resources:Controls,VoucherDate %>"
                                                AssociatedControlID="txtVoucherDate" />
                                            <asp:TextBox ID="txtVoucherDate" runat="server" MaxLength="200" CssClass="medium" TabIndex="2"
                                            onkeydown="return CheckKey(event)"  onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrfVoucherDate" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtVoucherDate" ErrorMessage="<%$ resources:Err_Voucherdate %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblRefDate" runat="server" Text="<%$ resources:Controls,refDate %>"
                                                AssociatedControlID="txtRefDate" />
                                            <asp:TextBox ID="txtRefDate" runat="server" MaxLength="100" CssClass="medium" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"/>
                                            <asp:RequiredFieldValidator ID="vrfRefDate" runat="server" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Voucher" EnableClientScript="true" Display="Dynamic"
                                                Text="*" ControlToValidate="txtRefDate" ErrorMessage="<%$ resources:Err_Refdate %>" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:Controls,ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" />
                                            <asp:TextBox ID="txtExchangeRate" runat="server" Enabled="false" CssClass="medium input-disabled"
                                                TabIndex="6" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Controls,Remarks %>"
                                                AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="7"
                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="divFirstPart" runat="server" class="gridwrap grid-group">
                                <h3 id="hHead1" runat="server">
                                </h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <table class="gridwraptable gridwrap">
                                        <tr>
                                            <th align="left" width="33%">
                                                <%= GetLocalResourceObject("Account").ToString()%>
                                            </th>
                                            <th align="left" width="12%">
                                                <%= Resources.Controls.Mode %>
                                            </th>
                                            <th align="left" width="25%">
                                                <%= Resources.Controls.Narration %>
                                            </th>
                                            <th id="thAmount" runat="server" align="left" width="20%">
                                                <%--<asp:Label ID="lblThAmount" runat="server" />--%>
                                            </th>
                                            <th align="left" width="10%">
                                                <%= Resources.Controls.Action %>
                                            </th>
                                        </tr>
                                        <tr class="grd-rowhead">
                                            <td>
                                                <asp:TextBox ID="txtAccount" runat="server" Width="95%" TabIndex="8"
                                                    onfocus="this.select();" onMouseUp="return false;" />
                                                    <asp:Button ID="btnAccount" runat="server" EnableTheming="false" Style="display: none"
                                                      OnClick="ActionHandler" CommandName="CHANGE" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAccount" runat="server" CssClass="star" Text="*" />
                                                </div>
                                                <asp:HiddenField ID="hdfAccount" runat="server" />
                                                  <asp:HiddenField ID="hdfSubTypePk" runat ="server" Value ="0" />
                                                <div class="clear">
                                                </div>
                                                <asp:DropDownList ID="ddlSubTypeAccount" runat ="server" Visible ="false"  Width="97%"  ></asp:DropDownList>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMode" runat="server" Width="95%" onchange="ShowInstrDetails()"
                                                    TabIndex="9" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidMode" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNarration" runat="server" Width="95%" TabIndex="10" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="input-w70 numeric" TabIndex="11" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAmount" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td rowspan="2" style="text-align: center; vertical-align: middle">
                                                <asp:ImageButton ID="imbAddItem" runat="server" SkinID="imbaddnew" CommandName="ADDLITEM" TabIndex="12"
                                                    OnClick="ActionHandler" ValidationGroup="AddItem" OnClientClick="return ValidateAddItems('AddItem');" />
                                            </td>
                                        </tr>
                                        <tr id="trInstrDet" class="grd-rowhead" style="display: none">
                                            <td>
                                                <asp:Label ID="lblInstrNo" runat="server" AssociatedControlID="txtInstrNo" Text="<%$ resources:Controls,InstrNo %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrNo" runat="server" Width="95%" TabIndex="13" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrNo" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInstrDate" runat="server" AssociatedControlID="txtInstrDate" Text="<%$ resources:Controls,Date %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrDate" runat="server" Width="90%" TabIndex="14" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrDate" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFavourOf" runat="server" AssociatedControlID="txtFavourOf" Text="<%$ resources:Controls,FavourOf %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtFavourOf" runat="server" Width="95%" TabIndex="15" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidFavourOf" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="divSecondPart" runat="server" class="gridwrap grid-group">
                                <h3 id="hHead2" runat="server">
                                </h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <table class="gridwraptable gridwrap">
                                        <tr>
                                            <th align="left" width="33%">
                                                <%= GetLocalResourceObject("Account").ToString()%>
                                            </th>
                                            <th align="left" width="12%">
                                                <%= Resources.Controls.Mode %>
                                            </th>
                                            <th align="left" width="25%">
                                                <%= Resources.Controls.Narration %>
                                            </th>
                                            <th id="thDebit" runat="server" align="left" width="10%">
                                                <%= Resources.Controls.DebitAmt %>
                                            </th>
                                            <th id="thCredit" runat="server" align="left" width="10%">
                                                <%= Resources.Controls.CreditAmt %>
                                            </th>
                                            <th align="left" width="10%">
                                                <%= Resources.Controls.Action %>
                                            </th>
                                        </tr>
                                        <tr class="grd-rowhead">
                                            <td>
                                                <asp:TextBox ID="txtAccount1" runat="server" Width="95%" TabIndex="16"
                                                    onfocus="this.select();" onMouseUp="return false;" />
                                                  <asp:Button ID="btnAccount1" runat="server" EnableTheming="false" Style="display: none"
                                                      OnClick="ActionHandler" CommandName="CHANGE" />
                                                <asp:HiddenField ID="hdfAccount1" runat="server" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidAccount1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                                <asp:HiddenField ID="hdfSubTypePk1" runat ="server" Value ="0" />
                                                <div class="clear">
                                                </div>
                                                <asp:DropDownList ID="ddlSubTypeAccount1" runat ="server" Visible ="false" Width="97%"   ></asp:DropDownList>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMode1" runat="server" Width="95%" onchange="ShowInstrDetails1()"
                                                    TabIndex="17" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidMode1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNarration1" runat="server" Width="95%" TabIndex="18" />
                                            </td>
                                            <td id="tdDebit1" runat="server">
                                                <asp:TextBox ID="txtDebit1" runat="server" CssClass="input-w70 numeric"
                                                    onchange="SetDebitCreditAmt(1)" TabIndex="19" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidDebit1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td id="tdCredit1" runat="server">
                                                <asp:TextBox ID="txtCredit1" runat="server" CssClass="input-w70 numeric"
                                                    onchange="SetDebitCreditAmt()" TabIndex="20" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidCredit1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td rowspan="2" style="text-align: center; vertical-align: middle">
                                                <asp:ImageButton ID="imbAddItem1" runat="server" SkinID="imbaddnew" CommandName="ADDLITEM" TabIndex="21"
                                                    OnClick="ActionHandler" ValidationGroup="AddItem1" OnClientClick="return ValidateAddItems('AddItem1');" />
                                            </td>
                                        </tr>
                                        <tr id="trInstrDet1" class="grd-rowhead" style="display: none">
                                            <td>
                                                <asp:Label ID="lblInstrNo1" runat="server" AssociatedControlID="txtInstrNo1" Text="<%$ resources:Controls,InstrNo %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrNo1" runat="server" Width="95%" TabIndex="22" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrNo1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInstrDate1" runat="server" AssociatedControlID="txtInstrDate1"
                                                    Text="<%$ resources:Controls,Date %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtInstrDate1" runat="server" Width="90%" TabIndex="23" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidInstrDate1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFavourOf1" runat="server" AssociatedControlID="txtFavourOf1" Text="<%$ resources:Controls,FavourOf %>" />
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox ID="txtFavourOf1" runat="server" Width="95%" TabIndex="24" />
                                                <div class="starwrap">
                                                    <asp:Label ID="lblValidFavourOf1" runat="server" CssClass="star" Text="*" />
                                                </div>
                                            </td>
                                            <td id="tdDebit2" runat="server">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap grid-group">
                                <h3>
                                    <%= Resources.Controls.SelectedAccounts %>
                                </h3>
                                <div class="clear">
                                </div>
                                <div class="grid-group-table">
                                    <asp:GridView runat="server" ID="grdVoucher" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" ShowFooter="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,AccountCode %>" SortExpression="<%$ resources:DataFieldRes,PackingCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountCode" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountCode) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountCode) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,AccountName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountName" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountName) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.AccountMst + "." + Resources.DataFieldRes.AccountName) %>' />
                                                    <asp:HiddenField ID="hdfAccntID" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxAccount) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="21%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Mode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMode" runat="server" ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>'
                                                        Text='<%# Eval(Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.ConfigName) %>' />
                                                    <asp:HiddenField ID="hdfMode" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxPaymentMode) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Narration %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNarration" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxNarration) %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxNarration) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Controls,Total %>" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,DebitAmt %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDebit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount,"{0:c}") %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount,"{0:c}") %>' CssClass="input-w70 numeric" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblDebitTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,CreditAmt %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCredit" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount,"{0:c}") %>'
                                                        Text='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount,"{0:c}") %>' CssClass="input-w70 numeric" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblCreditTotal" runat="server" />
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Action %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbEditGrid" runat="server" SkinID="imbeditgrid" CommandName="GRIDEDIT"
                                                        OnClick="ActionHandler" TabIndex="25" />
                                                    <asp:ImageButton ID="imbDeleteGrid" runat="server" SkinID="imbdeletegrid" CommandName="GRIDDELETE"
                                                        OnClick="ActionHandler" TabIndex="26" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow><asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow></asp:Table></div><div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="Voucher" runat="server" />
                <asp:ValidationSummary ID="vsAddItem" ValidationGroup="AddItem" runat="server" />
                <asp:ValidationSummary ID="vsAddItem1" ValidationGroup="AddItem1" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="inv"></uc1:WorkflowUserComments>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
