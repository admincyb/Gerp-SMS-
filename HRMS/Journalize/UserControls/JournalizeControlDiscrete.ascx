<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalizeControlDiscrete.ascx.cs" Inherits="ERPSMS_v01.Journalize.UserControls.JournalizeControlDiscrete" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<script type="text/javascript" language="javascript">
    var NumberDigits = 0;
    var CurrencyDigits = 0;
    var ExchRateDigits = 0;
    var pageURL1 = window.document.URL;
    var virtualPath1 = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url1 = pageURL1.replace(location.pathname, virtualPath1 == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath1 + "Handlers/AutoComplete.ashx");

    //Redirect after alert box
    function changePage(whereToGo, messageText, messageType) {
        ShowErrorMessage(messageText, messageType);
        window.location = whereToGo;
    }

    function VoucherUserControlInitComponents() {
        GrandScriptUtils.DatePickerCommon("txtRefDate");
        if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
            $('[id$=btnJournalSubmit]').hide();
    }
    $(document).ready(function () {
        NumberDigits = parseInt($("[id$=hdfNumberDigitsVoucher]").val());
        CurrencyDigits = parseInt($("[id$=hdfCurrencyDigitsVoucher]").val());
        ExchRateDigits = parseInt($("[id$=hdfExchRateDigitsVoucher]").val());
    });

    function ShowUserControlListing(flag) {
        if (flag) {
            $("[id$=pnlUserControlListing]").show();
            $("[id$=pnlUserControlEntry]").hide();
        }
        else {

            $("[id$=pnlUserControlListing]").hide();
            $("[id$=pnlUserControlEntry]").show();
        }
        return false;
    }
    function UserControlViewMode(mode) {
        //Mode = 1 Indicates its on View Mode
        //Mode = 2 Indicates its on New Mode
        if (mode == 1) {
            $("[id$=btnJournalSave]").hide();
            $("[id$=btnDelete]").hide();
            $("[id$=btnAddCredit]").hide();
            $("[id$=btnAddDebit]").hide();
        }
        else if (mode == 2) {
            $("[id$=btnDelete]").hide();
            $("[id$=btnJournalPrint]").hide();
        }
    }

    //To excecute after auto complete selection
    function AfterJournalControlAutoCompleteSelect(targetControlID) {
        if (targetControlID.substring(5, 13) == "02set102") {
            var ctrlId = targetControlID;
            var btnCtrlId = ctrlId.replace(ctrlId.substring(5, 13), "04set104");
            $("[id$=" + btnCtrlId + "]").click();
        }
    }

    function AfterJournalControlAutoCompleteInvalidSelect(targetControlID) {
        if (targetControlID.substring(5, 13) == "02set102") {
            var ctrlId = targetControlID;
            var btnCtrlId = ctrlId.replace(ctrlId.substring(5, 13), "04set104");
            var hdfCtrlId = ctrlId.replace(ctrlId.substring(5, 13), "03set103");
            $("[id$=" + hdfCtrlId + "]").val("");
            $("[id$=" + btnCtrlId + "]").click();
        }
    }


    $(document).ready(function () {
        calculateTotal();
    });


    function CalculateBCAmt(sender) {
        var val1 = parseFloat($(sender).val());
        var exrate = 0;
        if (!isNaN(parseFloat($("#[id$=hdfExchangeRateJV]").val()))) {
            exrate = parseFloat($("#[id$=hdfExchangeRateJV]").val());
        }

        if (!isNaN(val1)) {
            exrate = 0;
            var txtid = $(sender).attr('id');
            var txtExchangeRateId = txtid.replace("06set106", "07set107");
            if (!isNaN(parseFloat($("#[id$=" + txtExchangeRateId + "]").val()))) {
                exrate = parseFloat($("#[id$=" + txtExchangeRateId + "]").val());
            }
            var lblid = txtid.replace("06set106", "08set108");
            var amounttc = parseFloat((val1 * exrate).toFixed(8));
            $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));


        } else {
            var txtid = $(sender).attr('id');
            var lblid = txtid.replace("06set106", "08set108");
            var amounttc = 0;
            $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
        }

        calculateTotal(sender);
    }

    //    function CalculateTCAmt(sender) {
    //        var val1 = parseFloat($(sender).val());
    //        var exrate = 0;
    //        if (!isNaN(parseFloat($("#[id$=hdfExchangeRateJV]").val()))) {
    //            exrate = parseFloat($("#[id$=hdfExchangeRateJV]").val());
    //        }

    //        if (!isNaN(val1)) {
    //            exrate = 0;
    //            var txtid = $(sender).attr('id');
    //            var txtExchangeRateId = txtid.replace("08set108", "07set107");
    //            if (!isNaN(parseFloat($("#[id$=" + txtExchangeRateId + "]").val()))) {
    //                exrate = parseFloat($("#[id$=" + txtExchangeRateId + "]").val());
    //            }
    //            var lblid = txtid.replace("08set108", "06set106");
    //            var amounttc = 0;
    //            if (val1 > 0 && exrate > 0) {
    //                amounttc = parseFloat((val1 / exrate).toFixed(8));
    //            }
    //            //Juno 04-06-2014
    //             $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
    //            //$("[id$=" + lblid + "]").val(DecimalPartFormat( $("[id$=" + lblid + "]").val(), CurrencyDigits));


    //        } else {
    //            var txtid = $(sender).attr('id');
    //            var lblid = txtid.replace("08set108", "06set106");
    //            var amounttc = 0;
    //            $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
    //        }

    //        calculateTotal(sender);
    //    }

    //Juno 04-0602014
    function CalculateTCAmt(sender, e) {
        var val1 = parseFloat($(sender).val());
        var exrate = 0;
        var keyCode = e.keyCode ? e.keyCode : e.which;
        if (!isNaN(parseFloat($("#[id$=hdfExchangeRateJV]").val()))) {
            exrate = parseFloat($("#[id$=hdfExchangeRateJV]").val());
        }

        if (!isNaN(val1)) {
            exrate = 0;
            var txtid = $(sender).attr('id');
            var txtExchangeRateId = txtid.replace("08set108", "07set107");
            if (!isNaN(parseFloat($("#[id$=" + txtExchangeRateId + "]").val()))) {
                exrate = parseFloat($("#[id$=" + txtExchangeRateId + "]").val());
            }
            var lblid = txtid.replace("08set108", "06set106");
            var amounttc = 0;
            if (val1 > 0 && exrate > 0) {
                amounttc = parseFloat((val1 / exrate).toFixed(8));
            }
            if (keyCode != 9)
                $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));

        } else {
            var txtid = $(sender).attr('id');
            var lblid = txtid.replace("08set108", "06set106");
            var amounttc = 0;
            $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
        }

        calculateTotal(sender);
    }


    function CalculateAllTCAmt(sender) {
        var hdrExrate = 0;
        if (!isNaN(parseFloat($("#[id$=txtJournalExchangeRate]").val()))) {
            hdrExrate = parseFloat($("#[id$=txtJournalExchangeRate]").val());
            $("#[id$=hdfExchangeRateJV]").val(hdrExrate);
        }

        var totaldrBC = 0;
        $('.amounttcdr').each(function (i, obj) {
            if (!isNaN(parseFloat($("#" + this.id).val()))) {
                var exrate = hdrExrate;
                var txtid = this.id;
                var txtExchangeRateId = txtid.replace("06set106", "07set107");
                var hdfEntryModeId = txtExchangeRateId + "EntryMode";
                var lblid = txtid.replace("06set106", "08set108");
                if ($("[id$=" + hdfEntryModeId + "]").val() == "2") {

                    if (!isNaN(parseFloat($("[id$=" + lblid + "]").val()))) {
                        var amountbc = parseFloat($("[id$=" + lblid + "]").val());
                        totaldrBC = parseFloat((totaldrBC + amountbc).toFixed(8));
                    }
                }
                else {
                    if ($("[id$=" + hdfEntryModeId + "]").val() == "0" && $("[id$=hdfExchangeRateChange]").val() == "0")
                        $("[id$=" + txtExchangeRateId + "]").val(DecimalPartFormat(exrate, ExchRateDigits));
                    if (!isNaN(parseFloat($("#" + txtExchangeRateId).val()))) {
                        exrate = parseFloat($("#" + txtExchangeRateId).val());
                    }
                    var value = 0;
                    value = parseFloat($("#" + this.id).val());

                    var amounttc = parseFloat((value * exrate).toFixed(8));
                    $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
                    totaldrBC = parseFloat((totaldrBC + amounttc).toFixed(8));
                }
            }
        });
        $("[id$=txtDrTotalBC]").val(DecimalPartFormat(totaldrBC, CurrencyDigits));

        var totalcrBC = 0;
        $('.amounttccr').each(function (i, obj) {
            if (!isNaN(parseFloat($("#" + this.id).val()))) {
                var exrate = hdrExrate;
                var txtid = this.id;
                var txtExchangeRateId = txtid.replace("06set106", "07set107");
                var hdfEntryModeId = txtExchangeRateId + "EntryMode";
                var lblid = txtid.replace("06set106", "08set108");
                if ($("[id$=" + hdfEntryModeId + "]").val() == "2") {
                    if (!isNaN(parseFloat($("[id$=" + lblid + "]").val()))) {
                        var amountbc = parseFloat($("[id$=" + lblid + "]").val());
                        totalcrBC = parseFloat((totalcrBC + amountbc).toFixed(8));
                    }
                } else {
                    if ($("[id$=" + hdfEntryModeId + "]").val() == "0" && $("[id$=hdfExchangeRateChange]").val() == "0")
                        $("[id$=" + txtExchangeRateId + "]").val(DecimalPartFormat(exrate, ExchRateDigits));
                    if (!isNaN(parseFloat($("#" + txtExchangeRateId).val()))) {
                        exrate = parseFloat($("#" + txtExchangeRateId).val());
                    }
                    var value = 0;
                    value = parseFloat($("#" + this.id).val());
                    var amounttc = parseFloat((value * exrate).toFixed(8));
                    $("[id$=" + lblid + "]").val(DecimalPartFormat(amounttc, CurrencyDigits));
                    totalcrBC = parseFloat((totalcrBC + amounttc).toFixed(8));
                }
            }
        });
        $("[id$=txtCrTotalBC]").val(DecimalPartFormat(totalcrBC, CurrencyDigits));
        $("[id$=hdfExchangeRateChange]").val("0");
    }


    function calculateTotal(sender) {
        if (sender) {
            var id = $(sender).attr('id');
            if (id.substring(17) == "dr") {
                var totaldr = 0;
                var totaldrBC = 0;
                $('.amounttcdr').each(function (i, obj) {

                    var hdfEntryModeId = this.id.replace(this.id.substring(5, 13), "07set107") + "EntryMode";
                    if ($("[id$=" + hdfEntryModeId + "]").val() != "2") {
                        if (!isNaN(parseFloat($("#" + this.id).val()))) {
                            var value = parseFloat($("#" + this.id).val());
                            totaldr = parseFloat((totaldr + value).toFixed(8));
                        }
                    }

                    var drBCId = this.id.replace(this.id.substring(5, 13), "08set108");
                    if (!isNaN(parseFloat($("#" + drBCId).val()))) {
                        var value = parseFloat($("#" + drBCId).val());
                        totaldrBC = parseFloat((totaldrBC + value).toFixed(8));
                    }

                });
                $("[id$=txtDrTotal]").val(DecimalPartFormat(totaldr, CurrencyDigits));
                $("[id$=txtDrTotalBC]").val(DecimalPartFormat(totaldrBC, CurrencyDigits));
            }
            else if (id.substring(17) == "cr") {
                var totalcr = 0;
                var totalcrBC = 0;
                $('.amounttccr').each(function (i, obj) {

                    var hdfEntryModeId = this.id.replace(this.id.substring(5, 13), "07set107") + "EntryMode";
                    if ($("[id$=" + hdfEntryModeId + "]").val() != "2") {
                        if (!isNaN(parseFloat($("#" + this.id).val()))) {
                            var value = parseFloat($("#" + this.id).val());
                            totalcr = parseFloat((totalcr + value).toFixed(8));
                        }
                    }
                    var crBCId = this.id.replace(this.id.substring(5, 13), "08set108");
                    if (!isNaN(parseFloat($("#" + crBCId).val()))) {
                        var value = parseFloat($("#" + crBCId).val());
                        totalcrBC = parseFloat((totalcrBC + value).toFixed(8));
                    }
                });
                $("[id$=txtCrTotal]").val(DecimalPartFormat(totalcr, CurrencyDigits));
                $("[id$=txtCrTotalBC]").val(DecimalPartFormat(totalcrBC, CurrencyDigits));
            }
        } else {
            var totaldr = 0;
            var totaldrBC = 0;
            $('.amounttcdr').each(function (i, obj) {

                var hdfEntryModeId = this.id.replace(this.id.substring(5, 13), "07set107") + "EntryMode";
                if ($("[id$=" + hdfEntryModeId + "]").val() != "2") {
                    if (!isNaN(parseFloat($("#" + this.id).val()))) {
                        var value = parseFloat($("#" + this.id).val());
                        totaldr = parseFloat((totaldr + value).toFixed(8));
                    }
                }
                var drBCId = this.id.replace(this.id.substring(5, 13), "08set108");
                if (!isNaN(parseFloat($("#" + drBCId).val()))) {
                    var value = parseFloat($("#" + drBCId).val());
                    totaldrBC = parseFloat((totaldrBC + value).toFixed(8));

                }
            });

            $("[id$=txtDrTotal]").val(DecimalPartFormat(totaldr, CurrencyDigits));
            $("[id$=txtDrTotalBC]").val(DecimalPartFormat(totaldrBC, CurrencyDigits));

            var totalcr = 0;
            var totalcrBC = 0;
            $('.amounttccr').each(function (i, obj) {

                var hdfEntryModeId = this.id.replace(this.id.substring(5, 13), "07set107") + "EntryMode";
                if ($("[id$=" + hdfEntryModeId + "]").val() != "2") {
                    if (!isNaN(parseFloat($("#" + this.id).val()))) {
                        var value = parseFloat($("#" + this.id).val());
                        totalcr = parseFloat((totalcr + value).toFixed(8));
                    }
                }
                var crBCId = this.id.replace(this.id.substring(5, 13), "08set108");
                if (!isNaN(parseFloat($("#" + crBCId).val()))) {
                    var value = parseFloat($("#" + crBCId).val());
                    totalcrBC = parseFloat((totalcrBC + value).toFixed(8));
                }
            });
            $("[id$=txtCrTotal]").val(DecimalPartFormat(totalcr, CurrencyDigits));
            $("[id$=txtCrTotalBC]").val(DecimalPartFormat(totalcrBC, CurrencyDigits));
        }
    }




    function CalculateBCWithER(sender) {
        var exrate = 0;
        var amountTc = 0;
        var exrate = parseFloat($(sender).val());
        if (!isNaN(exrate)) {
            var txtid = $(sender).attr('id');
            var txtAmountTC = txtid.replace("07set107", "06set106");
            var amountTc = parseFloat($("[id$=" + txtAmountTC + "]").val());
            if (!isNaN(amountTc)) {
                var amountBc = 0;
                amountBc = parseFloat((exrate * amountTc).toFixed(8));
                var txtAmountBC = txtid.replace("07set107", "08set108");
                $("[id$=" + txtAmountBC + "]").val(DecimalPartFormat(amountBc, CurrencyDigits));
            }
        }
        calculateTotal(sender);
    }

    function AfterCloseWkfInJournal() {
        $("[id$=hdfIsSaveSubmit]").val("0");
        if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
            $('[id$=btnJournalSubmit]').hide();
    }


    function ValidateVoucher(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#divErrorVoucher").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
    function toFixed(num, precision) {
        return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
    }
    function DecimalPartFormat(val, power) {
        var retVal;
        var result;
        if (!isNaN(parseFloat(val))) {
            //Juno 04-06-2014
            if ($("#[id$=hdfEnableRound]").val() == "0") {
                var v = val.toString().split('.');
                if (v.length == 1) {
                    // retVal = parseFloat(v[0]).toFixed(power);
                    retVal = toFixed(result, power);
                } else {
                    if (v[1].length >= power) {
                        // retVal = v[0] + "." + v[1].substring(0, power);
                        result = v[0] + "." + v[1];
                        retVal = toFixed(result, power);
                    }
                    else {
                        //retVal = (parseFloat(v[0]) + parseFloat(0 + "." + v[1])).toFixed(power);
                        result = (parseFloat(v[0]) + parseFloat(0 + "." + v[1]));
                        retVal = toFixed(result, power);
                    }
                }
            }
            else {
                //  retVal = parseFloat(val).toFixed(power);
                retVal = toFixed(val, power);

            }
            return retVal;
        }
    }

    //    function DecimalPartFormat(val, power) {
    //        var retVal;
    //        var result;
    //        if (!isNaN(parseFloat(val))) {
    //            //Juno 04-06-2014
    //            if ($("#[id$=hdfEnableRound]").val() == "0") {
    //                var v = val.toString().split('.');
    //                if (v.length == 1) {
    //                    // retVal = parseFloat(v[0]).toFixed(power);
    //                    retVal = (Math.round(parseFloat(v[0]) * 100) / 100).toFixed(power);
    //                } else {
    //                    if (v[1].length >= power) {
    //                        // retVal = v[0] + "." + v[1].substring(0, power);
    //                        result = v[0] + "." + v[1];
    //                        retVal = (Math.round(parseFloat(result) * 100) / 100).toFixed(power);
    //                    }
    //                    else {
    //                        //retVal = (parseFloat(v[0]) + parseFloat(0 + "." + v[1])).toFixed(power);
    //                        result = (parseFloat(v[0]) + parseFloat(0 + "." + v[1]));
    //                        retVal = (Math.round(parseFloat(result) * 100) / 100).toFixed(power);
    //                    }
    //                }
    //            }
    //            else {
    //                //  retVal = parseFloat(val).toFixed(power);
    //                retVal = (Math.round(val * 100) / 100).toFixed(power);
    //            }
    //            return retVal;
    //        }
    //    }

</script>
<asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
    <ContentTemplate>
        <div class="Button-container-popup">
            <div class="buttoncontainer-fields floatLeft">
                <asp:DropDownList ID="ddlVoucherCompany" runat="server">
                </asp:DropDownList>
            </div>
            <asp:Button runat="server" ID="btnSaveAsTemplate" CommandName="SAVEASTEMPLATE" TabIndex="500"
                Text="<%$resources:ErpRes,SaveAsTemplate %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,SaveAsTemplate %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-submit" />
            <asp:Button runat="server" ID="btnJournalSubmit" CommandName="SUBMIT" TabIndex="500"
                Text="<%$resources:ErpRes,Submit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Submit %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-submit" OnClientClick="javascript:ValidateVoucher('voucher')" ValidationGroup="voucher" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="17"
                Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-submit" />
            <asp:Button runat="server" ID="btnJournalSaveSubmit" CommandName="SAVESUBMIT" TabIndex="501"
                Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,SaveSubmit %>" ValidationGroup="voucher"
                CommandArgument="ucrJournalize" SkinID="btnInner-submit" OnClientClick="javascript:ValidateVoucher('voucher')" />
            <asp:Button runat="server" ID="btnAddDebit" CommandName="ADDDEBIT" TabIndex="502"
                Text="<%$resources:ErpRes,AddDebit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,AddDebit %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-New" />
            <asp:Button runat="server" ID="btnAddCredit" CommandName="ADDCREDIT" TabIndex="503"
                Text="<%$resources:ErpRes,AddCredit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,AddCredit %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-New" />
            <asp:Button runat="server" ID="btnJournalSave" CommandName="SAVE" TabIndex="504"
                Text="<%$resources:Controls,Save %>" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>"
                CommandArgument="ucrJournalize" SkinID="btnInner-Save" OnClientClick="javascript:ValidateVoucher('voucher')" ValidationGroup="voucher"/>
            <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                OnClick="ActionHandler" TabIndex="505" CommandArgument="ucrJournalize" SkinID="btnInner-Delete"
                ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
            <asp:Button runat="server" ID="btnJournalPrint" Text="<%$resources:Controls,Print %>"
                OnClick="ActionHandler" CommandName="PRINTJORNALPOPUP" TabIndex="506" CommandArgument="ucrJournalize"
                SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
            <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                CssClass="popupclose" OnClick="ActionHandler" CommandName="CANCEL" TabIndex="507"
                CommandArgument="ucrJournalize" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
        </div>
        <div class="content-wrapper">
            <asp:HiddenField ID="hdfNumberDigitsVoucher" runat="server" Value="3" />
            <asp:HiddenField ID="hdfCurrencyDigitsVoucher" runat="server" Value="3" />
            <asp:HiddenField ID="hdfExchRateDigitsVoucher" runat="server" Value="3" />
            <asp:HiddenField ID="hdfDecimalFormatVoucher" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatVoucher" runat="server" />
            <asp:HiddenField ID="hdfRateFormatVoucher" runat="server" />
            <asp:HiddenField ID="hdfExchRateFormatVoucher" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateChange" runat="server" Value="0" />
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                <asp:TableRow ID="Header" runat="server">
                    <asp:TableCell>
                        <div class="fields-group input-margin2">
                            <div class="detail-co3" runat="server" id="divUser">
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lblPVNo" Text="<%$ resources:PVNo%>" AssociatedControlID="txtDispPVNo"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDispPVNo"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vrfDispPVNo" runat="server" ControlToValidate="txtDispPVNo"
                                        Text="*" CssClass="star" ValidationGroup="voucher" Display="Dynamic" EnableClientScript="true"
                                        SetFocusOnError="true" ErrorMessage="<%$ resources:Err_PVNo %>"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblRefNo1" Text="<%$ resources:RefNo%>" AssociatedControlID="txtRefNo"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRefNo"></asp:TextBox>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lblPVDatelb" Text="<%$ resources:PVDate%>" AssociatedControlID="txtPVDate"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPVDate" TabIndex="90" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                    <asp:HiddenField ID="hdfPVDateat" runat="server" Value="" />
                                    <asp:RequiredFieldValidator ID="vrfPVDate" runat="server" ControlToValidate="txtPVDate"
                                        Text="*" CssClass="star" ValidationGroup="voucher" Display="Dynamic" EnableClientScript="true"
                                        SetFocusOnError="true" ErrorMessage="<%$ resources:Err_PVDate %>"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblRefDate2" Text="<%$ resources:RefDate%>" AssociatedControlID="txtRefDate"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRefDate" CssClass="date-picker" onkeydown="return CheckKey(event)"
                                        onpaste="return false;"></asp:TextBox>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtJournalCurrency"></asp:Label>
                                    <asp:TextBox ID="txtJournalCurrency" runat="server" MaxLength="50" TabIndex="91"
                                        CssClass="input-w50"> </asp:TextBox>
                                    <asp:HiddenField ID="hdfJournalCurr" runat="server" Value="" />
                                    <asp:Button ID="btnCurrencyJV" runat="server" Text="<%$ resources:Controls,Search %>"
                                        OnClick="ActionHandler" TabIndex="3" CommandName="CALCURRENCY" SkinID="btnInner-search"
                                        EnableTheming="false" Style="display: none" />
                                    <asp:RequiredFieldValidator ID="vrfJournalCurrency" runat="server" ControlToValidate="txtJournalCurrency"
                                        Text="*" CssClass="star" ValidationGroup="voucher" Display="Dynamic" EnableClientScript="true"
                                        InitialValue="<%$ resources:Messages, AutoDefaultValue %>" SetFocusOnError="true"
                                        ErrorMessage="<%$ resources:Err_Currency %>"></asp:RequiredFieldValidator>
                                    <asp:ImageButton ID="imbGainLoss" runat="server" SkinID="g&l" CommandName="GAINLOSS"
                                        OnClick="ActionHandler" CommandArgument="ucrJournalize" />
                                    <div class="clear">
                                    </div>
                                    <div style="display: none">
                                        <asp:Label runat="server" ID="lblExchangeRate" Text="<%$ resources:ExchangeRate%>"
                                            AssociatedControlID="txtJournalExchangeRate"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtJournalExchangeRate" Text="" CssClass="numeric input-w50 input-normalb"
                                            TabIndex="92" MaxLength="8" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                        <cc1:ExchangeRateValidation ID="vreExchangeRate" runat="server" ControlToValidate="txtJournalExchangeRate"
                                            ErrorMessage="<%$ resources:Err_ExchangeRate %>" NumberDigits="5" Display="Dynamic"
                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="voucher"
                                            NonZero="true"></cc1:ExchangeRateValidation>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="JournalizeUserControl" runat="server">
                    <asp:TableCell>
                        <asp:Literal ID="litJournalCurrencyDr" runat="server"></asp:Literal>
                        <asp:Literal ID="litJournalCurrencyCr" runat="server"></asp:Literal>
                        <asp:Panel ID="pnlControls" runat="server">
                            <div id="divGroupHeadDr" runat="server" class="fields-grpwrap color-grey grp-after">
                                <h1>
                                    Debit Details</h1>
                                <div class="clear">
                                </div>
                                <div id="divGroupDrHdr" runat="server" class="fields-group">
                                </div>
                                <div id="divGroupDr" runat="server" class="fields-group">
                                </div>
                                <div id="divTotalDr" runat="server" class="divcolmiddle-total">
                                    <asp:Label runat="server" ID="lbl1" Text="<%$ resources:Total_Amount_Dr %>" AssociatedControlID="txtDrTotal"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDrTotal" CssClass="numeric input-w101" onkeydown="return EnableArrowKey(event);"
                                        onpaste="return false;" TabIndex="298"></asp:TextBox><asp:TextBox runat="server"
                                            ID="txtDrTotalBC" CssClass="numeric input-w101" onkeydown="return EnableArrowKey(event);"
                                            onpaste="return false;" TabIndex="299"></asp:TextBox>
                                </div>
                            </div>
                            <div id="divGroupHeadCr" runat="server" class="fields-grpwrap color-grey grp-after">
                                <h1>
                                    Credit Details</h1>
                                <div class="clear">
                                </div>
                                <div id="divGroupCrHdr" runat="server" class="fields-group">
                                </div>
                                <div id="divGroupCr" runat="server" class="fields-group">
                                </div>
                                <div id="divTotalCr" runat="server" class="divcolmiddle-total">
                                    <asp:Label runat="server" ID="lbl2" Text="<%$ resources:Total_Amount_Cr %>" AssociatedControlID="txtCrTotal"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtCrTotal" CssClass="numeric input-w101" TabIndex="497"
                                        onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox><asp:TextBox
                                            runat="server" ID="txtCrTotalBC" CssClass="numeric input-w101" onkeydown="return EnableArrowKey(event);"
                                            onpaste="return false;" TabIndex="498"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <div class="fields-group">
                            <table>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label runat="server" ID="lblNarration" Text="<%$ resources:Narration %>" AssociatedControlID="txtNarration"
                                                Width="11.9%"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtNarration" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" TabIndex="499" onkeyup="limitText(this,400);"
                                                Width="72.3%"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"
                                                Width="11.9%"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,400);" TabIndex="499" onkeyup="limitText(this,400);"
                                                Width="72.3%"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="divErrorVoucher" style="display: none">
                <asp:ValidationSummary ID="vsVoucherTemplate" ValidationGroup="vouchertemplate" runat="server" />
                <asp:ValidationSummary ID="vsVoucher" ValidationGroup="voucher" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </div>
        <div>
            <asp:HiddenField ID="hdfJournalBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateJV" runat="server" />
            <asp:HiddenField ID="hdfEnableRound" runat="server" Value="1" />
            <asp:HiddenField ID="hdfSubTypePk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCoaPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfVoucherNo" runat="server" />
            <asp:HiddenField ID="hdfVoucherStatus" Value="0" runat="server" />
        </div>
        <div id="divTemplate" style="display: none">
            <div class="Button-container-popup">
                <asp:Button ID="btnTemplateOK" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:ErpRes,Ok %>"
                    OnClick="ActionHandler" TabIndex="54" CommandName="TEMPLATEOK" CommandArgument="ucrJournalize"
                    ToolTip="<%$resources:ErpRes,Ok %>" OnClientClick="javascript:ValidateVoucher('vouchertemplate')"  ValidationGroup="vouchertemplate"/>
                <asp:Button ID="btnTemplateCancel" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:ErpRes,Cancel %>"
                    ToolTip="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler" TabIndex="54"
                    CommandName="TEMPLATECANCEL" CommandArgument="ucrJournalize" />
            </div>
            <div class="content-wrapper">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-P">
                                <asp:Label runat="server" ID="lblTemplateCategory" AssociatedControlID="ddlTemplateCategory"
                                    Text="<%$ resources:Category %>"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlTemplateCategory" Width="61.4%">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vrfTemplateCategory" runat="server" ControlToValidate="ddlTemplateCategory"
                                    Text="*" CssClass="star" InitialValue="-1" ValidationGroup="vouchertemplate"
                                    Display="Dynamic" EnableClientScript="true" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_TemplateCategory %>"></asp:RequiredFieldValidator>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-P">
                                <asp:Label runat="server" ID="lblTemplateName" AssociatedControlID="txtTemplateName"
                                    Text="<%$ resources:TemplateName %>"></asp:Label>
                                <asp:TextBox runat="server" ID="txtTemplateName" Width="59%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vrfTemplateName" runat="server" ControlToValidate="txtTemplateName"
                                    Text="*" CssClass="star" ValidationGroup="vouchertemplate" EnableClientScript="true"
                                    Display="Dynamic" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_TemplateName %>"></asp:RequiredFieldValidator>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
