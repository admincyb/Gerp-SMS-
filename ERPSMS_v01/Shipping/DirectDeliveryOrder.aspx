<%@ Page Title="<%$ Resources:Captions,Title_DirectDeliveryOrder %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="DirectDeliveryOrder.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.DirectDeliveryOrder" Theme="ClassicExt"  %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="vc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
     
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDODate");
            GrandScriptUtils.DatePickerCommon("txtETD");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false, false, false);
            GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFrmDate", "ToDate", "hdfPOSearchToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "&IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerSearch", url, "hdfCustomerPKSearch", true, true, "CUSTOMERLIST");
            var pageURLDO = '<%= Resources.PageURL.DirectDeliveryOrderURL %>';
            GrandScriptUtils.MakeAutoCompleteDDL("txtDeliveryOrderNoSearch", url + "&Type=DPH_NO" + "&PAGE_URL=" + pageURLDO, "hdfDeliveryOrderNoPkSearch", true, true, "GETDIRECTDONOAUTO");
            SetSearchType();
            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsDOCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $('[id$=pnlSave]').hide();
            }
            else {
                $("[id$=tblDetailHdr]").addClass("table-devide");
            }

        }
        function ShowAllocationPopup(title) {
            ShowContainerDiv("[id$=divCartonDtlsPopUp]", title, '950', '450');
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                // $("[id$=ddlCompany]").hide();       
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                //$("[id$=ddlCompany]").show();

            }
            return false;
        }
        function PageViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSubmit]").hide();

            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
                $("[id$=pnlPLPrint]").hide();
            }
        }
        function showSODetails(sender) {
            var senderId = sender.id;
            var poUrl = '<%= Resources.PageURL.ReportUrl %>';
            var poIdHdf = senderId.replace("lbnSONoAdmissionList", "hdfSOIdStockAdmissionList");
            var poId = $("#" + poIdHdf).val();
            poUrl = poUrl + "?ID=" + poId + "&APPTYPE=SOD&APPSUBTYPE=";
            OpenPDF(poUrl);

            return false;
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
        function fnShowCartons() {
            var msgTitle = '<%= GetLocalResourceObject("CartonDetails").ToString() %>';
            var msg = $("[id$=hdfCartons]").val();
            ShowContainerDiv('[id$=divCartonDtlsPopUp]', 'Carton Details', '950', '450');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 400,
                width: 600,
                title: msgTitle,
                resizable: false,
                close: function (e) {
                    $("[id$=txtCartonPrefixPopUp]").focus();
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }
        function fnCartonListReloadConfirm() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_CartonListReloadMsg").ToString() %>';
            $("[id$=chkAutoMode]").attr('checked', false);
            ShowContainerDiv('[id$=divCartonDtlsPopUp]', 'Carton Details', '950', '450');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 140,
                width: 313,
                title: msgTitle,
                resizable: false,
                close: function (e) {
                    if ($("[id$=hdfCartonListReloadConfirm]").val() != '1') {
                        $("[id$=imgShowPopupDummy]").click();
                        $("[id$=chkAutoMode]").focus();
                    }
                },
                buttons: {
                    Yes: function (e) {
                        $("[id$=chkAutoMode]").attr('checked', true);
                        $("[id$=hdfCartonListReloadConfirm]").val('1');
                        $(this).dialog("close");
                        $("[id$=imgAutoModeDummy]").click();
                        $("[id$=chkAutoMode]").focus();
                    },
                    Cancel: function (e) {

                        $("[id$=hdfCartonListReloadConfirm]").val('0');
                        $("[id$=chkAutoMode]").attr('checked', false);
                        $(this).dialog("close");
                        //                        $("[id$=imgShowPopupDummy]").click();
                        //                        $("[id$=chkAutoMode]").focus();
                    }
                }
            });
            return false;
        }

        function fnConfirmSomeCartonMissing() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_FollowingCartonsMissing").ToString() %>';
            msg += $("[id$=hdfSomeCartonMissingMessage]").val();
            ShowContainerDiv('[id$=divCartonDtlsPopUp]', 'Carton Details', '950', '450');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 400,
                width: 600,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfSomeCartonMissingConfirm]").val('1');
                        $("[id$=imgAddPlusDummy]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfSomeCartonMissingConfirm]").val('0');
                    }
                }
            });
            return false;
        }

        function fnConfirmOrderPcs() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_CartonPcsValidationMsg").ToString() %>';
//            msg += $("[id$=hdfSomeCartonMissingMessage]").val();
            ShowContainerDiv('[id$=divCartonDtlsPopUp]', 'Carton Details', '950', '450');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 300,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfOrderPcsConfirm]").val('1');
                        $("[id$=btnApplyCartonDetails]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfOrderPcsConfirm]").val('0');
                    }
                }
            });
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
                ShowErrorMessage($("#diverrorAlert").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                //To Prevent Multiple Click 
                if ($("[id$=hdfIsMultipleClick]").val() == "0") {
                    $("[id$=hdfIsMultipleClick]").val("1");
                    return true;
                }
                else
                    return false

                //return true;
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

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustSelected]").click();
            }
        }
        function CheckAllSO(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdPendingSOs.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function SetSearchType(isLoad) {
            ///<summary>Function To Enable/Disable Selected Option For Search </summary>
            ClearSearchDetails();
            var strname = $("select[id$=SearchType]").val();
            $("[id$=SearchValue]").val("");
            if (strname == "Date") { //Date Range
                $("#divSearchDtls").hide();
                $("#divDate").show();
                $("[id$=imbSearch]").show();
                var frDate = $("[id$=hdfFrmDate]").val();
                var toDate = $("[id$=hdfPOSearchToDate]").val();
                //                GrandScriptUtils.AddDateRange("FromDate", "hdfFrmDate", "ToDate", "hdfPOSearchToDate", false, false);
                if (frDate != '' && toDate != '') {
                    $("[id$=FromDate]").val(getFormattedDate(frDate));
                    $("[id$=hdfFrmDate]").val(frDate);
                    $("[id$=ToDate]").val(getFormattedDate(toDate));
                    $("[id$=hdfPOSearchToDate]").val(toDate);
                }
            }
            else {

                GrandScriptUtils.MakeAutoCompleteDDL("SearchValue", url + "&Type=" + $("select[id$=SearchType]").val() + "&CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSearchValuePk", true, true, "GETDIRECTSOPENDINGAUTO");

                $("#divSearchDtls").show();
                $("#divDate").hide();
                $("[id$=imbSearch]").show();
            }
        }

        function ClearSearchDetails() {
            ///<summary>To Clear Details In Search Section</summary>
            $("[id$=SearchValue]").val("");
            var strname = $("select[id$=SearchType]").val();
            if (strname == "Date") {

            }
            else {
                $("[id$=FromDate]").val("");
                $("input[id$=hdfFrmDate]").val("");
                $("[id$=ToDate]").val("");
                $("input[id$=hdfPOSearchToDate]").val("");
            }
        }

        function AfterGridExpand(row) {
            if ($("[id$=grdDeliveryOrderSearchList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
        }

        //For   check  Desp.Now Qty is greater than ordered quantity 
        //1-Save,2-Submit
        function ShowDespatchNowQtyExceeds(val) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_DOQtyExceedsSOQtyContinue").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsContDespatchNowQty]").val(1);
                        $(this).dialog("close");
                        if (val == '1') {
                            $("[id$=btnSave]").click();
                        }
                        else if (val == '2') {
                            $("[id$=btnSaveSubmit]").click();
                        }
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIsContDespatchNowQty]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
        function CalculateTotalBatchQty() {
            var TotalQty = 0;
            var qty = 0;
            $("[id$=grdBatchDetailsPopup] tr").each(function () {
                if ($(this).find("[id*=lblQtyGridPopup]").length > 0) {
                    qty = parseFloat($(this).find("[id*=lblQtyGridPopup]").html().replace(/[^0-9\.]+/g, ""));
                    if (!isNaN(qty)) {
                        TotalQty = TotalQty + qty;
                    }
                }
            });
            $("#[id*=grdBatchDetailsPopup] [id*=lblPopupFooterTotal]").html(addCommas(TotalQty.toFixed(NumberDigits)));
        }

        function addCommas(number) {
            var FormattedNumber = number;
            var curGroup1 = 3;
            var curGroup2 = 3;
            var NumericPart = "", LastNumericPart = "", DecimalPart = "";
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup1]").val()))) {
                curGroup1 = parseFloat($("#[id*=hdfCurrencyGroup1]").val());
            }
            if (!isNaN(parseFloat($("#[id*=hdfCurrencyGroup2]").val()))) {
                curGroup2 = parseFloat($("#[id*=hdfCurrencyGroup2]").val());
            }

            DecimalPart = number.split('.')[1];
            (DecimalPart) ? DecimalPart = "." + DecimalPart : DecimalPart = "";
            NumericPart = number.split('.')[0];
            if (NumericPart.length > curGroup1) {
                LastNumericPart = NumericPart.substr(NumericPart.length - curGroup1, curGroup1);
                (LastNumericPart) ? LastNumericPart = "," + LastNumericPart : LastNumericPart = "";
            }
            if ((NumericPart.length - curGroup1) > 0) {
                NumericPart = NumericPart.substr(0, NumericPart.length - curGroup1);
                var pattern = "\\B(?=(\\d{" + curGroup2 + "})+(?!\\d))";
                var expression = new RegExp(pattern, "g");
                NumericPart = NumericPart.toString().replace(expression, ",");
                if (isNaN(NumericPart) && NumericPart.length == 1) {
                    LastNumericPart = LastNumericPart.substr(1, LastNumericPart.length);
                }
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
        }

        function DespNwChanged(sender) {
            if ($("[id$=hdfEnableNetWt]").val() == 1) {
                $("[id$=btnDespNwChanged]").click();
                $("[id$" + sender.id + "]").focus();
            }
        }

        //for after messge need to focus on control


        function ShowErrMessageFocusCtrl(message, title, RedirectURL, PageRebind, AfterCloseFocusCtrl) {
            ///<summary>
            ///function used to Show Messages
            ///</summary> 
            if (!title)
                title = errorTitle;
            $(".error").html("");
            $(".error").html(message);
            if (RedirectURL) {
                $(".error").dialog({
                    resizable: false,
                    title: title,
                    buttons: {
                        OK: function (e) {
                            window.location = RedirectURL;
                        }
                    },
                    beforeClose: function (event, ui) { window.location = RedirectURL; },
                    modal: true,
                    open: function (event, ui) {
                        $(this).parent().appendTo("#popupHolder");
                    }
                });
            }
            else {
                $(".error").dialog({
                    resizable: false,
                    title: title,
                    buttons: {
                        OK: function (e) {
                            if (typeof AfterMessageClose == "function") {
                                if (AfterCloseFocusCtrl)
                                    AfterMessageClose(AfterCloseFocusCtrl);
                                else
                                    AfterMessageClose();
                            }
                            $(".error").dialog('close');
                        }
                    },
                    modal: true,
                    open: function (event, ui) {
                        $(this).parent().appendTo("#popupHolder");
                    },
                    close: function (event) {
                        if (typeof AfterMessageClose == "function" && PageRebind == true) {
                            AfterMessageClose();
                        }
                        else
                            if (typeof AfterMessageClose == "function") {
                                if (AfterCloseFocusCtrl)
                                    AfterMessageClose(AfterCloseFocusCtrl);
                                else
                                    AfterMessageClose();
                            }
                    }
                });
            }
            return false;
        }

        function AfterMessageClose(CtrlID) {
//            if (CtrlID == 'txtPalleteBinCard')
//                $("[id$=txtPalleteBinCard]").focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <asp:HiddenField ID="hdfProcId" runat="server" />
            <asp:HiddenField ID="hdfGRH_PK" runat="server" />
            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
            <asp:HiddenField ID="SO_PK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperator" runat="server" />
            <asp:HiddenField ID="hdfRateFormat" runat="server" />
            <asp:HiddenField ID="hdfDPH_VERSION" runat="server" Value="1" />
            <asp:HiddenField ID="hdfEnableBatch" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsDOCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsContDespatchNowQty" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="hdfCurrencyGroup1" Value="3" />
            <asp:HiddenField runat="server" ID="hdfCurrencyGroup2" Value="2" />
            <div class="fixed-buttons-normal">
                <%--Top Buttons "Save", ...--%>
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="18"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="18" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="18" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="18" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="18"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="34" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlPLPrint">
                                        <asp:Button runat="server" TabIndex="34" ID="btnPLPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:PLPrint %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:PLPrint %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="18" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="3" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li runat="server" id="Li1">
                                        <asp:Button runat="server" TabIndex="4" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelDO %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelDO %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--Page Datas--%>
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="5" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="6" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Listing Page Table Row--%>
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
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="7" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="7" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="Label6" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="8" CssClass="input-small-a"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label ID="Label8" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="9" MaxLength="13" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="Label2" runat="server" Text="<%$resources:DeliveryOrderNo %>" AssociatedControlID="txtDeliveryOrderNoSearch"></asp:Label>
                                            <asp:TextBox ID="txtDeliveryOrderNoSearch" runat="server" TabIndex="10" CssClass="input-small-a margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDeliveryOrderNoPkSearch" runat="server" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-small margnbotm0"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1 margnbotm0"
                                                TabIndex="10">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="Label5" Text="<%$ resources:Customer%>" AssociatedControlID="txtCustomerSearch"></asp:Label>
                                            <asp:TextBox ID="txtCustomerSearch" runat="server" TabIndex="11" CssClass="select-half margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerPKSearch" runat="server" />
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="11"
                                                CommandName="SEARCH" SkinID="search-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="11" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdDeliveryOrderSearchList" AutoGenerateColumns="False"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="True" OnSorting="ActionHandler" Width="100%" OnRowDataBound="ActionHandler"
                                    PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="17" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGroupingHierarchy(this, 'grdDeliveryOrderSearchList');" />
                                                <asp:HiddenField runat="server" ID="hdfDespatchID" Value='<%# Eval("DPH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDepartmentID" Value='<%# Eval("DPH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField>
                                            <ItemStyle Width="1.5%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="DPH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Left" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DeliveryOrderNo %>" SortExpression="DPH_NO">
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkDoNumber" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="SHOW" Text='<%# Eval("DPH_NO") %>' ToolTip='<%# Eval("DPH_NO") %>'
                                                    CommandArgument='<%# Eval("DPH_PK") %>'></asp:LinkButton>--%>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("DPH_NO")))?Resources.ErpRes.Draft:Eval("DPH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("DPH_NO")))?Resources.ErpRes.Draft:Eval("DPH_NO")%>'></asp:Label>
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SHOWSODTL"
                                                    CommandArgument='<%# Eval("DPH_PK") %>' EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="DPH_CUSTOMER_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPH_CUSTOMER_TEXT"),50)%>'
                                                    ToolTip='<%#Eval("DPH_CUSTOMER_TEXT")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval("DPH_CUSTOMER") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="72%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdtUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:DespQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedTotQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPH_TOTAL_QTY")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("DPH_TOTAL_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvQty" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("DPH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("DPH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <asp:GridView runat="server" ID="grdSOList" AutoGenerateColumns="False" GridLines="None"
                                                        EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" OnRowDataBound="ActionHandler"
                                                        OnRowCommand="ActionHandler" Width="100%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="24" GroupName="SelectOne"
                                                                        ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGroupingHierarchy(this, 'grdDeliveryOrderList');" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                                <ItemTemplate>
                                                                    <%--<asp:CheckBox runat="server" ID="chkSCselect" Height="22px" />--%>
                                                                    <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval("SOH_PK") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfDPHNO" Value='<%# Eval("DPH_NO") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfDpdPK" Value='<%# Eval("DPD_PK") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfDphPK" Value='<%# Eval("DPH_PK") %>' />
                                                                     <asp:HiddenField runat="server" ID="hdfDpdTotalPcs" Value='<%# Eval("DPD_TOTAL_PCS") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" VerticalAlign="Middle" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                                <ItemTemplate>
                                                                    <%--<asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval("SOH_NO") %>'
                                                                        OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval("SOH_PK") %>'
                                                                        ToolTip='<%# Eval("SOH_NO") %>'></asp:LinkButton>--%>
                                                                    <asp:Label ID="lblSONOList" runat="server" Text='<%# Eval("SOH_NO") %>' ToolTip='<%# Eval("SOH_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="14%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSODate" runat="server" Text='<%# Eval("SOH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("SOH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Type %>" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("SOH_TYPE_TEXT"),3,"") %>'
                                                                        ToolTip='<%#Eval("SOH_TYPE_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("DPD_ITEM_TEXT").ToString()),40) %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("DPD_ITEM_TEXT").ToString())%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("SOD_CUST_ITEM") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="23%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SOQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSaleOrderQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("SOD_QTY_APPROVED"))%>'
                                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("SOD_QTY_APPROVED")) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfSaleOrderUOMPK" Value='<%# Eval("SOD_UOM") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSaleOrderUOM" runat="server" Text='<%# Eval("SOD_UOM_TEXT")%>'
                                                                        ToolTip='<%# Eval("SOD_UOM_TEXT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                                <HeaderStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DespQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSCDespQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_DESPATCHED"))  %>'
                                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_DESPATCHED")) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfDespUOMPK" Value='<%# Eval("DPD_UOM") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDespUOM" runat="server" Text='<%# Eval("DPD_UOM_TEXT")%>' ToolTip='<%# Eval("DPD_UOM_TEXT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                                <HeaderStyle />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSCInvQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("SOD_QTY_INVOICED")) %>'
                                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("SOD_QTY_INVOICED")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:NoOfBag %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbNoOfBag" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_DESPATCHED_CTN")) %>'
                                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_DESPATCHED_CTN")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Allocation %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAllocatedQty" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_ISSUED_CTN")) %>'
                                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("DPD_QTY_ISSUED_CTN")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="14%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="imbAllocation" SkinID="show-carton" CommandName="ALLOCATION_ACTION"
                                                                        ToolTip="<%$resources:Controls,Allocation %>" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="4%" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDepreNo" runat="server" Text="<%$ resources:DeliveryOrderNo%>"
                                                AssociatedControlID="lblDeliveryOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDeliveryOrderNo" CssClass="input-small"></asp:Label>
                                            <asp:Label runat="server" ID="lblDODate" class="middle-lbl-c" Text="<%$ resources:DateMandatoty%>"
                                                AssociatedControlID="txtDODate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDODate" CssClass="input-small" TabIndex="12" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqDODateEntry" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="txtDODate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_ReqDODt %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:CustomerMandatory %>"
                                                AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="13"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="Save"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtCustomer" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Customer %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="Label12" Text="<%$ resources:ETDMandatory%>" AssociatedControlID="txtETD"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtETD" CssClass="input-small" TabIndex="14" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="txtETD" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_ReqETD %>">
                                            </asp:RequiredFieldValidator>
                                            <div id="divSBUCompany" style="display: none">
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:Company%>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" TabIndex="14" runat="server" onmouseover="javascript:ShowTooltip('ddlCompany');"
                                                    CssClass="select-half">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label ID="lblDeptStore" runat="server" Text="<%$ resources:StoreMandatory%>"
                                                AssociatedControlID="ddlDeptStore"></asp:Label>
                                            <asp:DropDownList ID="ddlDeptStore" runat="server" TabIndex="12" CssClass="select-half-a"
                                                EnableViewState="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqAdmissionStore" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlDeptStore" Display="Dynamic"
                                                Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectStore %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblDeliveryTo" runat="server" Text="<%$ resources:DeliveryTo%>" AssociatedControlID="txtDeliveryAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryAddress" TextMode="MultiLine" CssClass="multiline-2line select-half"
                                                onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" oncontextmenu="return false;"
                                                TabIndex="13"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <h3 class="fontWGT-Nrml">
                                <%= GetLocalResourceObject("PendingSaleOrderItems").ToString()%>
                            </h3>
                            <div class="gridwrap">
                                <div id="searchwrap" class="search-wrap-custom1">
                                    <label>
                                        <%=Resources.Controls.SearchBy%></label>
                                    <asp:DropDownList ID="SearchType" TabIndex="15" runat="server" CssClass="srchboxtextbx"
                                        onchange="javascript:SetSearchType(true);">
                                        <asp:ListItem Value="SOH_NO" Text="<%$ Resources:BindValues, SONumber%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Item%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="Date" Text="Date Range">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <div id="divSearchDtls">
                                        <asp:TextBox ID="SearchValue" runat="server" TabIndex="15">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfSearchValuePk" runat="server" EnableViewState="true" />
                                    </div>
                                    <div id="divDate">
                                        <label for="FromDate">
                                            <%=Resources.Controls.FromDate%></label>
                                        <asp:TextBox ID="FromDate" runat="server" TabIndex="15" EnableViewState="true">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfFrmDate" runat="server" EnableViewState="true" />
                                        <label for="ToDate">
                                            <%=Resources.Controls.ToDate%></label>
                                        <asp:TextBox ID="ToDate" runat="server" TabIndex="15" EnableViewState="true">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdfPOSearchToDate" runat="server" EnableViewState="true" />
                                    </div>
                                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="15" OnClick="ActionHandler"
                                        CommandName="CHANGE" />
                                    <div class="clear">
                                    </div>
                                </div>
                                <asp:GridView runat="server" ID="grdPendingSOs" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" TabIndex="15">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyPOList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllSOList" runat="server" onclick="CheckAllSO(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectSOList" runat="server" Checked='<%# Eval("CheckBoxChecked") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfSOIdPendingList" Value='<%# Eval("SOId") %>' />
                                                <%--SOH_PK--%>
                                                <asp:HiddenField runat="server" ID="hdfSOIdStockAdmissionList" Value='<%# Eval("SOId") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSODetIdPendingList" Value='<%# Eval("SODetId") %>' />
                                                <%-- SOD_PK--%>
                                                <asp:HiddenField runat="server" ID="hdfSOPendingListSelected" Value='<%# Eval("AddedToStockList") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval("SOH_CURRENCY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfItmNeedBatchStk" Value='<%# Eval("ITM_NEED_BATCH_STK") %>' />
                                                <asp:LinkButton ID="lbnSONoAdmissionList" runat="server" ToolTip='<%# Eval("SONumber")%>'
                                                    CommandName="SODetails" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("SONumber").ToString()),18) %>'
                                                    Style="text-decoration: underline;" OnClientClick="return showSODetails(this);"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval("SODate", Resources.Constants.DateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("SODate", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemSOList" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductType" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("ProductTypeCode").ToString()) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ProductTypeText").ToString())%>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfProductType" Value='<%# Eval("ProductType") %>'/>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSORate" runat="server" Text='<%#GetFormattedRate(Eval("SORate")) %>'
                                                    ToolTip='<%#GetFormattedRate(Eval("SORate")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUomSOList" runat="server" Text='<%# Eval("UOM")%>' ToolTip='<%# Eval("UOM")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SOQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOQtySOList" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("SOQtyApproved")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("SOQtyApproved")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PreviousDeliveredQty%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreRecievQtyPOList" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("PreDeliveredQty")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("PreDeliveredQty")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PendingQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPendingQtyPOList" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("BalanceQtyToDeliver")) %>'
                                                    ToolTip='<%#GetFormattedNumberWithComma(Eval("BalanceQtyToDeliver")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="text-align: right;">
                                <%-- <div class="button-wrap-right">--%>
                                <asp:Button ID="btnAddToDOList" runat="server" TabIndex="16" Text="<%$ Resources:Controls, AddToList%>"
                                    ToolTip="<%$ Resources:Controls, AddToList%>" CommandName="ADDTOLIST" OnClick="ActionHandler" />
                                <%-- </div>--%>
                            </div>
                            <h3 class="fontWGT-Nrml">
                                <%= GetLocalResourceObject("DeliveryOrderList").ToString()%>
                            </h3>
                            <div class="scroll-container2">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdDeliveryOrderList" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" TabIndex="17" OnRowDataBound="ActionHandler"
                                        OnRowCommand="ActionHandler" EnableViewState="true">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyAdmissionList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfPkDOList" Value='<%# Eval("DPD_PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPODetIdDOList" Value='<%# Eval("DPD_SO_DTL") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPOIdDOList" Value='<%# Eval("DPD_SALE_ORDER") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfCategory" Value='<%# Eval("ItemCategoryId") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfIsMultiple" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hdfDPD_SL_NO" Value='<%# Eval("DPD_SL_NO") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfItmNeedBatchStkDOList" Value='<%# Eval("ITM_NEED_BATCH_STK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfSOUOMPK" Value='<%# Eval("DPD_SALE_UOM") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfProductType" Value='<%# Eval("ProductType") %>' />
                                                    <%--<asp:LinkButton ID="lbnPONoDeliveryOrderList" runat="server" ToolTip='<%# Eval("SONumber")%>'
                                                        CommandName="PODetails" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("SONumber").ToString()),15) %>'
                                                       ></asp:LinkButton>--%>
                                                    <asp:Label ID="lblDONo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("SONumber").ToString()),20) %>'
                                                        ToolTip='<%# Eval("SONumber")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="14%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSaleOrderODate" runat="server" Text='<%# Eval("SODate", Resources.Constants.DateFormatGrid)  %>'
                                                        ToolTip='<%# Eval("SODate", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                </ItemTemplate>
                                               <%-- <ItemStyle Width="8%" />--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfItemIdDOList" Value='<%# Eval("DPD_ITEM") %>' />
                                                    <asp:Label ID="lblItemDOList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ItemName").ToString()),60) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="<%$ resources:UOM %>">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlUOMGrid" runat="server" TabIndex="10" Width="80px" OnSelectedIndexChanged="ActionHandler"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField runat="server" ID="hdfDPDUOMPK" Value='<%# Eval("DPD_UOM") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BatchNo %>">
                                                <ItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlBatchNogrid" OnSelectedIndexChanged="ActionHandler"
                                                        CssClass="w145" AutoPostBack="true" onchange="Page_BlockSubmit = false;" ValidationGroup="none"
                                                        TabIndex="41">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lbTotal" runat="server" Text="<%$ resources:Total %>" CssClass="numeric" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton CssClass="nomargin" ID="imbAddGridBatch" SkinID="add-batches" runat="server"
                                                        OnClick="ActionHandler" ToolTip="<%$ resources:AddBatches %>" CommandName="ADD_ACTION"
                                                        TabIndex="41"></asp:ImageButton>
                                                    <asp:ImageButton CssClass="nomargin" ID="imbViewGridBatch" SkinID="view-batches"
                                                        runat="server" OnClick="ActionHandler" ToolTip="<%$ resources:ViewBatches %>"
                                                        CommandName="ADD_ACTION" Visible="false" TabIndex="41"></asp:ImageButton>
                                                    <asp:ImageButton CssClass="nomargin" ID="imbClearGridBatch" SkinID="clear-batches"
                                                        runat="server" OnClick="ActionHandler" ToolTip="<%$ resources:Clearbatches %>"
                                                        CommandName="CLEARADD" Visible="false" TabIndex="41"></asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="true" HeaderText="<%$ resources:Stock %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrentStock" runat="server"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfDPDCurrentStock" Value='<%# Eval("DPD_CURRENT_STOCK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:SOQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSOQtyDOList" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPD_SALE_QTY")) %>'
                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("DPD_SALE_QTY")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfSOQty" runat="server" Value='<%# Eval("DPD_SALE_QTY")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DespQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDespatchedQtyDOList" runat="server" Text='<%#GetFormattedNumberWithComma(Eval("DPD_PRE_DELIVERED_QTY")) %>'
                                                        ToolTip='<%#GetFormattedNumberWithComma(Eval("DPD_PRE_DELIVERED_QTY")) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfDespatchedQty" runat="server" Value='<%# Eval("DPD_PRE_DELIVERED_QTY")%>' />
                                                    <asp:HiddenField ID="hdfPendingQtyDeliveryOrderList" runat="server" Value='<%# Eval("DPD_BALANCE_QTY_TO_DELIVER")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:DespNow %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDespatchNowQty" runat="server" CssClass="small-a numeric" Text='<%#GetFormattedNumber(Eval("DPD_QTY_DESPATCHED")) %>'
                                                        onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onchange="DespNwChanged(this);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="reqReceived" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="Save" runat="server" ControlToValidate="txtDespatchNowQty" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                                                    </asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:NetWeight %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNetWeight" runat="server" CssClass="small-a numeric margn-rgt-0"
                                                        MaxLength="17" Enabled="true"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <ItemStyle Width="8%" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <%-- <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalNetWeightFooter"></asp:Label>
                                            </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:GrossWeight %>" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtGrossWeight" runat="server" CssClass="small-a numeric margn-rgt-0"
                                                        MaxLength="18" Enabled="true"></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="amount-numeric"  />
                                                <ItemStyle Width="10%" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                
                                                <%--  <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalGrossWeightFooter"></asp:Label>
                                            </FooterTemplate>--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkRemoveDOList" runat="server" CommandName="RemoveDOItem" SkinID="delete-icon"
                                                        ToolTip="Delete" />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
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
            <div id="divCartonDtlsPopUp" style="display: none;">
                <div class="content-wrapper">
                    <div class="Button-container-popup" id="btnContainer">
                        <asp:Button runat="server" ID="btnApplyCartonDetails" CommandArgument="SEC_ActionPanel"
                            SkinID="btnInner-add-dsd" Text="<%$ resources:Controls,Apply %>" ToolTip="<%$resources:Controls,Apply %>"
                            OnClick="ActionHandler" CommandName="ISSUEAPPLY" />
                        <asp:HiddenField ID="hdfCDR_SO_DTL" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfCDR_SL_NO_PopUp" runat="server" Value="0" />
                    </div>
                    <div class="head-info">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 35%;">
                                    <asp:Label runat="server" ID="lblScNo" Text="<%$resources:SCNO %>" AssociatedControlID="lblSCNoPopUp"
                                        CssClass="w31-5perc">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblSCNoPopUp" CssClass="bold"></asp:Label>
                                </td>
                                <td style="width: 32%;">
                                    <asp:Label runat="server" ID="lblBoNo" Text="<%$resources:DONo%>" AssociatedControlID="lblDONoPopUp"></asp:Label>
                                    <asp:Label runat="server" ID="lblDONoPopUp" CssClass="bold"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                    <asp:Label ID="lblDoQty" runat="server" AssociatedControlID="lblDOQtyPcs" Text="<%$resources:DOQtyPcs%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblDOQtyPcs" CssClass="bold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 75%;">
                                    <asp:Label ID="lblBrandName" runat="server" Text="<%$resources:BrandName1%>" AssociatedControlID="lblBrandNamePopUp"
                                        CssClass="margnbotm margn-lft30"></asp:Label>
                                    <asp:Label runat="server" ID="lblBrandNamePopUp" CssClass="bold margnbotm"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                    <asp:Label ID="lblCtnCount" runat="server" AssociatedControlID="lblCartonCount" Text="<%$resources:CartonCount1%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblCartonCount" CssClass="bold" Text="100"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divCartonDtls">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblCartonPreFix" runat="server" Text="<%$ resources:CartonPrefix%>"
                                            AssociatedControlID="txtCartonPrefixPopUp" CssClass="middle-lbl-small-e1"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtCartonPrefixPopUp" CssClass="input-small" MaxLength="11"></asp:TextBox>
                                        <asp:Label ID="Label7" runat="server" Text="<%$ resources:From%>" AssociatedControlID="txtCartonFromPopUp"
                                            class="middle-lbl-xsmall-f"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtCartonFromPopUp" CssClass="input-small numeric"
                                            onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="11"
                                            onpaste="return false;" ondrop="return false;"></asp:TextBox>
                                        <asp:CustomValidator ID="cvalCartonFrom" runat="server" EnableClientScript="true"
                                            ClientValidationFunction="cartonAddToListValidate" ControlToValidate="txtCartonFromPopUp"
                                            ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterCartonFrom %>"
                                            Text="*" Display="Static" ValidationGroup="AddToList" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>
                                        <asp:Label ID="Label9" runat="server" Text="<%$ resources:To%>" AssociatedControlID="txtCartonToPopUp"
                                            class="middle-lbl-xsmall-i"></asp:Label>
                                        <%--class="middle-lbl"--%>
                                        <asp:TextBox runat="server" ID="txtCartonToPopUp" CssClass="input-small numeric"
                                            onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="11"
                                            onpaste="return false;" ondrop="return false;"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <div class="display-inline">
                                            <asp:Label ID="lblAutoMode" runat="server" Text='<%$ Resources:AutoMode %>' AssociatedControlID="chkAutoMode"
                                                CssClass="middle-lbl-a0"></asp:Label>
                                            <asp:CheckBox ID="chkAutoMode" runat="server" OnCheckedChanged="ActionHandler" CommandName="AUTOALLOCATECARTON"
                                                CssClass="style-none padgtop1" AutoPostBack="true" />
                                            <asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                ValidationGroup="AddToList" OnClientClick="javascript:ValidatePageNow('AddToList')"
                                                SkinID="imbaddnew" Style="margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClearAlloc" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEARMAPPING"
                                                SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="div2col-S">
                                        <div style="display: none;">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" CssClass="middle-lbl-xsmall-a3"
                                                AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="input-small-c" MaxLength="100"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                        </div>
                                        <%-- <asp:CustomValidator ID="cvalSONumber" runat="server" EnableClientScript="true" ClientValidationFunction="cartonAddToListValidate"
                                                                ControlToValidate="txtSONumber" ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterSalesContract %>"
                                                                Text="*" Display="Dynamic" ValidationGroup="AddToList" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>--%>
                                        <%-- <div class="div2col-S">
                                                        
                                                                </div>--%>
                                        <div style="display: none;">
                                            <asp:Label runat="server" ID="Label11" Text="<%$ resources:Location%>" AssociatedControlID="ddlLocationPopUp"
                                                class="middle-lbl"></asp:Label>
                                            <asp:DropDownList ID="ddlLocationPopUp" runat="server" CssClass="select-half">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdCartons" Width="100%" AutoGenerateColumns="false"
                                EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler" ShowFooter="true"
                                OnRowDataBound="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <%--<asp:HiddenField runat="server" ID="hdfBcrPk" Value='<%# Eval("CRC_CARTON_MST") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Eval("CRC_SL_NO") %>' />--%>
                                            <asp:HiddenField runat="server" ID="hdfCRC_SL_NO_GRP" Value='<%# Eval("CRC_SL_NO_GRP") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:BagOrCarton %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPallete" runat="server" Text='<%# Eval("BCR_PALLET_NO") %>' ToolTip='<%# Eval("BCR_PALLET_NO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="45%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:CartonCount %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCarton" runat="server" Text='<%# Eval("CartonsCount") %>' ToolTip='<%# Eval("CartonsCount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" CssClass="amount-numeric" />
                                        <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:QtyInPcs %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCartonPcs" runat="server" Text='<%# Eval("QtyPcs") %>' ToolTip='<%# Eval("QtyPcs") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" CssClass="amount-numeric" />
                                        <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Location %>" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFile" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("BCR_LOCATION_TEXT")==null?"":Eval("BCR_LOCATION_TEXT").ToString()),25) %>'
                                                ToolTip='<%# Eval("BCR_LOCATION_TEXT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Button ID="lnkRemoveHeader" runat="server" CommandName="REMOVEITEMALL" SkinID="delete-icon"
                                                ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" Style="margin-top: 0px;" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="lnkRemove" runat="server" CommandName="REMOVEITEM" SkinID="delete-icon"
                                                ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" Style="margin-top: 0px;" />
                                            <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="show-carton" CommandName="EDIT_ACTION"
                                                ToolTip="<%$resources:Controls,Allocation %>" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfCartonListReloadConfirm" Value="0" runat="server" />
             <asp:HiddenField ID="hdfOrderPcsConfirm" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCartons" Value="" runat="server" />
            <asp:HiddenField ID="hdfSomeCartonMissingConfirm" runat="server" Value="0" />
            <asp:HiddenField ID="hdfConfirmQuantityNotMatch" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSomeCartonMissingMessage" runat="server" Value="" />
            <asp:ImageButton ID="imgAddPlusDummy" runat="server" OnClick="ActionHandler" CommandName="ADD"
                ValidationGroup="AddToList" SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgAutoModeDummy" runat="server" OnClick="ActionHandler" CommandName="AUTOALLOCATECARTON"
                SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgShowPopupDummy" runat="server" OnClick="ActionHandler" CommandName="TAXPOPUPDISPLAY"
                SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgbtnDummyRemoveItem" runat="server" OnClick="ActionHandler"
                CommandName="REMOVEITEM" Style="display: none;" />
            <asp:Button ID="btnDespNwChanged" runat="server" OnClick="ActionHandler" CommandName="QTYCHANGE"
                EnableTheming="false" Style="display: none" />
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <%-- <asp:ValidationSummary ID="vsDetails" ValidationGroup="FormulationDetails" runat="server" />--%>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                <asp:ValidationSummary ID="vsNewentry" ValidationGroup="BatchPopup" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfindate" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsMultipleClick" Value="0" runat="server" />
                <asp:HiddenField ID="hdfEnableNetWt" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="so" />
            </div>
            <%--Popup div Start--%>
            <div id="divPopupBatches" style="display: none">
                <div class="content-wrapper">
                    <asp:Panel runat="server" ID="pnlPopup" Style="max-height: 350px; overflow-y: auto;">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnApplyBatches" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Apply %>"
                                ToolTip="<%$ Resources:Apply %>" OnClick="ActionHandler" CommandName="APPLY" />
                        </div>
                        <table style="background: #f1f1f1; margin-bottom: 10px;">
                            <tr>
                                <td style="display: none;">
                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:Item %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblItemNamePopup" Text="" CssClass="bold"></asp:Label>
                                </td>
                                <td style="min-width: 15%;">
                                    <asp:Label runat="server" ID="lblUomName" Text="<%$ Resources:UOM1 %>" CssClass="bold"></asp:Label>
                                    <asp:Label runat="server" ID="lblUomNamePopup" Text="" CssClass="lbl-24perc"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfUomPKPopup" Value="0" />
                                </td>
                            </tr>
                        </table>
                        <table class="gridwraptable gridwrap filter-arrow" style="margin-bottom: -4px!important;">
                            <tr>
                                <th width="65%">
                                    <%=Resources.Controls.BatchNo%>
                                    <asp:RequiredFieldValidator ID="vrfLatexBatchesPopUp" SetFocusOnError="true" ValidationGroup="BatchPopup"
                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="ddlBatchesPopUp"
                                        Text="*" ErrorMessage="<%$ resources:Msg_SelectPopupBatch %>" Display="Dynamic"
                                        InitialValue="-1"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlBatchesPopUp" Width="210px" TabIndex="1"
                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" onchange="Page_BlockSubmit = false;">
                                    </asp:DropDownList>
                                </th>
                                <th class="grd-head-rgt" style="vertical-align: top;" width="15%">
                                    <%=Resources.Controls.Stock%>
                                    <div class="clear">
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblStockPopUp" Text=""></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfBatchPopupUOM" />
                                    </div>
                                </th>
                                <th class="grd-head-rgt" width="14%">
                                    <%=Resources.Controls.Quantity%>
                                    <asp:RequiredFieldValidator ID="vrcLatexQtyPopUp" SetFocusOnError="true" ValidationGroup="BatchPopup"
                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="txtQtyPopUp"
                                        Text="*" ErrorMessage="<%$ resources:Msg_EnterQty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="vcfLatexQtyPopUp" runat="server" ErrorMessage="<%$ resources:QtymustbeGreaterZero %>"
                                        Operator="GreaterThan" CssClass="star" ValueToCompare="0" ValidationGroup="BatchPopup"
                                        Text="*" SetFocusOnError="true" ControlToValidate="txtQtyPopUp" Display="Dynamic"
                                        Type="Double"></asp:CompareValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:TextBox runat="server" ID="txtQtyPopUp" Style="float: left;" CssClass="numeric w100"
                                        ValidationGroup="BatchPopup" TabIndex="2" autocomplete="off" MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                </th>
                                <th width="6%" style="vertical-align: top;">
                                    <%=Resources.Controls.Action%>
                                    <div class="clear">
                                    </div>
                                    <div style="text-align: center; margin-top: 2px;">
                                        <asp:ImageButton runat="server" ID="imgAddBatchPopup" SkinID="imbaddnew" OnClick="ActionHandler"
                                            TabIndex="3" CommandName="ADDBATCHES" ValidationGroup="BatchPopup" OnClientClick="return ValidatePageNow('BatchPopup');" /></div>
                                    <asp:HiddenField runat="server" ID="hdnPopupstatus" Value="0" />
                                </th>
                            </tr>
                        </table>
                        <asp:GridView ID="grdBatchDetailsPopup" runat="server" CssClass="grdTable" AutoGenerateColumns="False"
                            Width="100%" AllowPaging="false" HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowHeader="false" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                            OnRowDataBound="ActionHandler">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%-- <div style="width: 209px;">--%>
                                        <asp:Label ID="lblBatchNoGridPopup" runat="server" Text='<%# Eval("BatchName") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfSIC_PK" Value='<%# Eval("SIC_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfSIC_STK_BATCH" Value='<%# Eval("SIC_STK_BATCH") %>' />
                                        <%-- </div>--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="43%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockGridPopup" runat="server" Text='<%# GetFormattedNumber(Eval("BatchStock")) %>'
                                            CssClass="numeric"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblQtyGridPopupTotal" runat="server" Text="<%$ resources:Totalbatchqty %>"
                                            CssClass="numeric" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyGridPopup" runat="server" Text='<%# GetFormattedNumber(Eval("SIC_QTY_CONSUMED")) %>'
                                            CssClass="numeric"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblPopupFooterTotal" runat="server" CssClass="numeric" />
                                    </FooterTemplate>
                                    <ItemStyle Width="26%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" Style="margin-left: 21%;"
                                            runat="server" ID="btnDeletepopupBatches" TabIndex="4" SkinID="imbdeletegrid"
                                            OnClick="ActionHandler" CommandName="DELETE_ACTION" ToolTip="<%$ Resources: Controls,Delete %>"
                                            OnClientClick="return ShowDeleteConfirm(this);" />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--<table style="font-weight: bold; background: #f1f1f1;">
                            <tr>
                                <td style="width: 45%;">
                                </td>
                                <td style="width: 27%; text-align: right;">
                                    <asp:Label runat="server" ID="Label9" Text="<%$ resources:TotalQty %>" CssClass="numeric"></asp:Label>
                                </td>
                                <td style="width: 21%; text-align: right;">
                                    <asp:Label runat="server" ID="lblTotwetQtyPopup" Text=""></asp:Label>
                                </td>
                                <td style="width: 7%;">
                                </td>
                            </tr>
                        </table>
                        <table style="font-weight: bold;">
                            <tr>
                                <td style="width: 45%;">
                                </td>
                                <td style="width: 27%; text-align: right;">
                                    <asp:Label runat="server" ID="Label10" Text="<%$ resources:BalQty %>" CssClass="numeric"></asp:Label>
                                </td>
                                <td style="width: 21%; text-align: right;">
                                    <asp:Label runat="server" ID="lblwetQtyBal" Text=""></asp:Label>
                                </td>
                                <td style="width: 7%;">
                                </td>
                            </tr>
                        </table>--%>
                    </asp:Panel>
                </div>
            </div>
            <%--Popup div End--%>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
