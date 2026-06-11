<%@ Page Title="<%$ Resources:Captions,Title_DirectStockAdmission %>" Language="C#" EnableEventValidation="false"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="DirectStockAdmission.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.DirectStockAdmission" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        [class="ui-widget-overlay"] {
            position: fixed !important;
        }

        [aria-labelledby^="ui-dialog"] {
            position: fixed !important;
        }

        .select-full-100 {
            width: 100%;
        }

        .search-wrap-custom1 {
            display: block !important;
            padding: 8px 10px 28px !important;
        }
    </style>
    <script type="text/javascript">
        var GRNCreate = {
            DirectStockTransferAutoURL: "DirectStockTransfer.do?Action=GetPendingDirectGRNSearchAuto&AUTOSEARCH=1&DeptPk=",
            DirectStockTransferWOAutoURL: "DirectStockTransfer.do?Action=GetPendingWOGRNSearchAuto&AUTOSEARCH=1&DeptPk="
        }

        var msgTitle = '<%= Resources.ErpRes.Information %>';
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function FillStoreLocationAutoComplete() {
            $("[id$=grdStockAdmissionList]").find("tr:has(td)").each(function (index) {
                GrandScriptUtils.MakeAutoCompleteText($(this).find("[id*=txtLocationAdmissionList]").attr("id"), url + "&Type=2&Category=17", $(this).find("[id*=hdfLocationAdmissionPK]").attr("id"), true, true, "GETINVLOC");
            });
        }

        function fnConfirmStockValueChange(command) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
                    msg = '<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('1');
                         $("[id$=btnSubmit]").click();
                        
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('0');
                    }
                }
            });
            return false;
        }


        function ShowListing(flag) {
            if (flag == 1) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                //$("[id$=pnlListing]").show();
                $("[id$=pnlbtnNew]").show();
                $("[id$=pnlbtnEdit]").show();
                $("[id$=pnlbtnView]").show();
                $("[id$=pnlbtnListPrint]").show();
                $("[id$=pnlEditforCancel]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=ddlCompany]").hide();

            }
            else if (flag == 2) {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                //$("[id$=pnlListing]").hide();
                $("[id$=pnlbtnNew]").hide();
                $("[id$=pnlbtnEdit]").hide();
                $("[id$=pnlbtnView]").hide();
                $("[id$=pnlbtnListPrint]").show();
                $("[id$=pnlEditforCancel]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=ddlCompany]").show();
                if ($("[id$=hdfWOStatus]").val() == "2" && $("[id$=hdfMenuType]").val() == "1") {
                    $("[id$=btnSave]").hide();
                    // $("[id$=btnEdit]").hide();
                }

            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                //$("[id$=pnlListing]").hide();
                $("[id$=pnlbtnNew]").hide();
                $("[id$=pnlbtnEdit]").hide();
                $("[id$=pnlbtnView]").hide();
                $("[id$=pnlbtnListPrint]").hide();
                $("[id$=pnlEditforCancel]").hide();
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
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=pnlDelete]").hide();
                // $("[id$=pnlSubmit]").hide();

            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function InitComponents() {

            GrandScriptUtils.DatePickerCommon("txtAdmissionDate", false, false, true);
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFrmDate", "ToDate", "hdfPOSearchToDate", false, false);
            var vendorSelectText = "type min 4 characters";
            if ($("[id$=hdfMenuType]").val() == "1")//Workorder GRN
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtVendor", url + "&IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val() + "&Role=7", "hdfVendorPKSearch", true, true, 0, "VENDOR");//Role 7 for WO Vendor
            else
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtVendor", url + "&IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendorPKSearch", true, true, 4, "VENDOR", "", "", "", "", vendorSelectText);

            var urlStockAdmission = url + "&FieldName=GRH_NO";
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtStockAdmissionNo", urlStockAdmission + "&MenuType=" + $("[id$='hdfMenuType']").val(), "hdfStockAdmissionNoPkSearch", true, true, 4, "DIRECTSTOCKADMISSIONNO");
            var urlStockAdmission2 = null;
            if ($("[id$=hdfMenuType]").val() == "1")//Workorder GRN
                urlStockAdmission2 = url + "&FieldName=GRH_WO_NO";
            else
                urlStockAdmission2 = url + "&FieldName=GRH_PO_NO";
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtPONo", urlStockAdmission2, "hdfPOPkSearch", true, true, 4, "DIRECTSTOCKADMISSIONNO");
            //            var urlDepartment = url + "?ProcessPK=" + $("[id$=hdfProcId]").val() + "&FieldName=DPT_NAME";
            var urlDepartment = url + "?ProcessPK=" + $("[id$=hdfProcessID]").val() + "&FieldName=DPT_NAME";
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtDepartment", urlDepartment, "hdfDepartmentPkSearch", true, true, 4, "STOCKADMISSION");

            //****************Multiple Plant**************************************************        
            var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
            if (parseInt(isMultiplePlant) == 1) {
                $("[id$=divPlantCode]").show();
            }
            else {
                $("[id$=divPlantCode]").hide();
            }
            //***************************************************************************************
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            if ($("[id$=hdfIsDSACancelled]").val() == "1") {
                $('[id$=pnlSave]').hide();
            }
            $("[id$=ddlAdmissionStore]").attr("disabled", "disabled");
            $(document).ready(function () {
                $("[id$=txtReceivedAdmissionList]").blur(function () {
                    var thisId = this.id;
                    var t = parseFloat($("#" + thisId).val());
                    var decimalFormat = $("[id$=hdfDecimalCount]").val();
                    var f = t.toFixed(decimalFormat);
                    $("#" + thisId).val(f);
                    var senderId = this.id;
                    senderId = senderId.replace("txtReceivedAdmissionList", "prefix");
                    //                    calculateRejectedQty(senderId);
                    calculateAcceptedQty(senderId);
                });
                $("[id$=txtRejectedAdmissionList]").blur(function () {
                    var thisId = this.id;
                    var t = parseFloat($("#" + thisId).val());
                    var decimalFormat = $("[id$=hdfDecimalCount]").val();
                    var f = t.toFixed(decimalFormat);
                    $("#" + thisId).val(f);
                    var senderId = this.id;
                    senderId = senderId.replace("txtRejectedAdmissionList", "prefix");
                    calculateAcceptedQty(senderId);
                });
                $("[id$=txtAcceptedAdmissionList]").blur(function () {
                    var thisId = this.id;
                    var t = parseFloat($("#" + thisId).val());
                    var decimalFormat = $("[id$=hdfDecimalCount]").val();
                    var f = t.toFixed(decimalFormat);
                    $("#" + thisId).val(f);
                    var senderId = this.id;


                    //                    senderId = senderId.replace("txtAcceptedAdmissionList", "prefix");
                    //                    calculateRejectedQty(senderId);
                });

                $("[id$=txtDOMAdmissionList]").filter(function () {
                    var txtDOMId = this.id;
                    var hdfDOMId = txtDOMId.replace("txtDOMAdmissionList", "hdfDOMAdmissionList");
                    var txtDOEId = txtDOMId.replace("txtDOMAdmissionList", "txtDOEAdmissionList");
                    var hdfDOEId = txtDOMId.replace("txtDOMAdmissionList", "hdfDOEAdmissionList");
                    GrandScriptUtils.AddDateRangeCommon(txtDOMId, hdfDOMId, txtDOEId, hdfDOEId, false, false);
                });

                enableDisableAddReasonButton();

                if ($("[id$=btnSaveSubmit]").length < 1 && $("[id$=btnSave]").length < 1) {
                    hideEditButtons();
                }
                SetSearchType();
                FillPuchaseOrderAutoComplete();
            });

            //Set a stamp for cancelled invoice
            if ($("[id$=hdfIsDSACancelled]").val() == "1")
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
            else
                $("[id$=tblDetailHdr]").addClass("table-devide");

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

        function recievedTextBoxValidate(sender, args) {
            //            sender.id = "ctl00_MainContent_grdStockAdmissionList_ctl02_cvalReceived"
            var senderId = sender.id;
            var recievedId = senderId.replace("cvalReceived", "txtReceivedAdmissionList");
            var pendingId = senderId.replace("cvalReceived", "hdfPendingQtyAdmissionList");
            var acceptedId = senderId.replace("cvalReceived", "txtAcceptedAdmissionList");
            var rejectedId = senderId.replace("cvalReceived", "txtRejectedAdmissionList");

            var pendingQty = $("#" + pendingId).val();
            var recievedQty = $("#" + recievedId).val();
            var acceptedQty = $("#" + acceptedId).val();

            pendingQty = parseFloat(pendingQty);
            recievedQty = parseFloat(recievedQty);
            acceptedQty = parseFloat(acceptedQty);
            if (isNaN(recievedQty) == true) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if (recievedQty <= 0) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if (recievedQty > pendingQty) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if (!isNaN(recievedQty) && !isNaN(acceptedQty)) {
                $("#" + rejectedId).text((recievedQty - acceptedQty));
            }
            return args.IsValid = true;
        }

        function acceptedTextBoxValidate(sender, args) {
            var senderId = sender.id;
            var recievedId = senderId.replace("cvalAccepted", "txtReceivedAdmissionList");
            var acceptedId = senderId.replace("cvalAccepted", "txtAcceptedAdmissionList");
            var rejectedId = senderId.replace("cvalAccepted", "txtRejectedAdmissionList");
            var acceptedQty = $("#" + acceptedId).val();
            var recievedQty = $("#" + recievedId).val();

            acceptedQty = parseFloat(acceptedQty);
            recievedQty = parseFloat(recievedQty);

            if (isNaN(acceptedQty) == true) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if (acceptedQty > recievedQty) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if (!isNaN(recievedQty) && !isNaN(acceptedQty)) {
                $("#" + rejectedId).text((recievedQty - acceptedQty));
            }
            var rejectedQty = $("#" + rejectedId).text();
            rejectedQty = parseFloat(rejectedQty);
            if (isNaN(rejectedQty) == false) {
                if ((acceptedQty + rejectedQty) > recievedQty) {
                    $("#litErrorMsg").text('Enter valid quantity');
                    return args.IsValid = false;
                }
            }

            return args.IsValid = true;
        }

        function rejectedTextBoxValidate(sender, args) {
            var senderId = sender.id;
            // var pendingId = senderId.replace("cvalReceived", "hdfPendingQtyAdmissionList");
            var recievedId = senderId.replace("cvalRejected", "txtReceivedAdmissionList");
            var acceptedId = senderId.replace("cvalRejected", "txtAcceptedAdmissionList");
            var rejectedId = senderId.replace("cvalRejected", "txtRejectedAdmissionList");
            // var pendingQty = $("#" + pendingId).val();
            var acceptedQty = $("#" + acceptedId).val();
            var recievedQty = $("#" + recievedId).val();
            var rejectedQty = $("#" + rejectedId).text(); // val();
            recievedQty = parseFloat(recievedQty);
            acceptedQty = parseFloat(acceptedQty);
            rejectedQty = parseFloat(rejectedQty);
            if (isNaN(rejectedQty) == true) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            //            if (rejectedQty == 'NaN') {
            //                return args.IsValid = false;
            //            }
            if (rejectedQty > recievedQty) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }
            if ((acceptedQty + rejectedQty) != recievedQty) {
                $("#litErrorMsg").text('Enter valid quantity');
                return args.IsValid = false;
            }

            return args.IsValid = true;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //  CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                if ($("[id$=litErrorMsg]").text() == '') {
                    $("[id$=litErrorMsg]").text('Enter valid quantity');
                }
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

        function ValidatePageNowWithDuplicate(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function CheckAllPO(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdPOs.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function showPODetails(sender) {
            var senderId = sender.id;
            var poUrl = '<%= Resources.PageURL.ReportUrl %>';
            var poIdHdf = senderId.replace("lbnPONoAdmissionList", "hdfPOIdStockAdmissionList");
            var poId = $("#" + poIdHdf).val();
            if ($("[id$=hdfMenuType]").val() == "1")
                poUrl = poUrl + "?ID=" + poId + "&APPTYPE=SCWO&APPSUBTYPE=1";//ID=" + WOPK + "&APPTYPE=" + ApplicationType.SCWO + "&APPSUBTYPE=1"
            else
                poUrl = poUrl + "?ID=" + poId + "&APPTYPE=PO&APPSUBTYPE=";
            OpenPDF(poUrl);

            return false;
        }

        function calculateRejectedQty(senderId) {
            var recievedId = senderId.replace("prefix", "txtReceivedAdmissionList");
            var acceptedId = senderId.replace("prefix", "txtAcceptedAdmissionList");
            var rejectedId = senderId.replace("prefix", "txtRejectedAdmissionList");
            var acceptedQty = $("#" + acceptedId).val();
            var recievedQty = $("#" + recievedId).val();

            acceptedQty = parseFloat(acceptedQty);
            recievedQty = parseFloat(recievedQty);

            $("#" + rejectedId).val(0);

            if (isNaN(acceptedQty) == true) {
                // $("#litErrorMsg").text('Enter valid quantity');
                //                return args.IsValid = false;
                return
            }
            if (acceptedQty > recievedQty) {
                //  $("#litErrorMsg").text('Enter valid quantity');
                //                return args.IsValid = false;
                return
            }
            if (!isNaN(recievedQty) && !isNaN(acceptedQty)) {
                var t = recievedQty - acceptedQty
                var decimalFormat = $("[id$=hdfDecimalCount]").val();
                var f = t.toFixed(decimalFormat);
                $("#" + rejectedId).val(f);
                var addId = senderId.replace("prefix", "lbnReasonAddStockAdmissionList");
                if (t == 0) {
                    $("#" + addId).hide();
                }
                else {
                    $("#" + addId).show();
                }
            }
        }

        function calculateAcceptedQty(senderId) {
            var recievedId = senderId.replace("prefix", "txtReceivedAdmissionList");
            var acceptedId = senderId.replace("prefix", "txtAcceptedAdmissionList");
            var hdfacceptedId = senderId.replace("prefix", "hdftxtAcceptedAdmissionList");
            var rejectedId = senderId.replace("prefix", "txtRejectedAdmissionList");
            var recievedQty = $("#" + recievedId).val();
            var rejectedQty = $("#" + rejectedId).val();

            recievedQty = parseFloat(recievedQty);
            rejectedQty = parseFloat(rejectedQty);

            $("#" + acceptedId).val(0);
            var decimalFormat = $("[id$=hdfDecimalCount]").val();
            var zero = 0;

            if (isNaN(recievedQty) == true) {
                $("#" + recievedId).val((zero.toFixed(decimalFormat)));
                return
            }
            if (isNaN(rejectedQty) == true) {
                $("#" + rejectedId).val((zero.toFixed(decimalFormat)));
                return
            }
            if (rejectedQty > recievedQty) {
                //  $("#litErrorMsg").text('Enter valid quantity');
                //                return args.IsValid = false;
                return
            }
            if (!isNaN(recievedQty) && !isNaN(rejectedQty)) {
                var t = recievedQty - rejectedQty
                var f = t.toFixed(decimalFormat);
                $("#" + acceptedId).val(f);
                $("#" + hdfacceptedId).val(f);
                var addId = senderId.replace("prefix", "lbnReasonAddStockAdmissionList");
                if (rejectedQty == 0) {
                    $("#" + addId).hide();
                }
                else {
                    $("#" + addId).show();
                }
                //$("[id$='hdftxtAcceptedAdmissionList']").val(f);
            }
        }

        function enableDisableAddReasonButton() {
            $(document).ready(function () {
                $("[id$=lbnReasonAddStockAdmissionList]").each(function (i, obj) {
                    var senderId = this.id;
                    senderId = senderId.replace("lbnReasonAddStockAdmissionList", "prefix");
                    var recievedId = senderId.replace("prefix", "txtReceivedAdmissionList");
                    var acceptedId = senderId.replace("prefix", "txtAcceptedAdmissionList");
                    var rejectedId = senderId.replace("prefix", "txtRejectedAdmissionList");
                    var acceptedQty = $("#" + acceptedId).val();
                    var recievedQty = $("#" + recievedId).val();

                    acceptedQty = parseFloat(acceptedQty);
                    recievedQty = parseFloat(recievedQty);

                    var addId = senderId.replace("prefix", "lbnReasonAddStockAdmissionList");
                    $("#" + addId).hide();

                    if (isNaN(acceptedQty) == true) {
                        // $("#litErrorMsg").text('Enter valid quantity');
                        //                return args.IsValid = false;
                        return
                    }
                    if (acceptedQty > recievedQty) {
                        //  $("#litErrorMsg").text('Enter valid quantity');
                        //                return args.IsValid = false;
                        return
                    }
                    if (!isNaN(recievedQty) && !isNaN(acceptedQty)) {
                        var t = recievedQty - acceptedQty
                        var decimalFormat = $("[id$=hdfDecimalCount]").val();
                        var f = t.toFixed(decimalFormat);
                        if (t != 0) {
                            $("#" + addId).show();
                        }
                    }

                });
            });
        }
        function hideEditButtons() {
            $("[id$=lnkRemoveAdmissionList]").each(function (i, obj) {
                $(this).hide();
            });
            $("[id$=imbEditRejectedDetails]").each(function (i, obj) {
                $(this).hide();
            });
            $("[id$=imbDeleteRejectedDetails]").each(function (i, obj) {
                $(this).hide();
            });
            $("[id$=btnSaveDamage]").hide();
        }

        //For checking selected date is a future date or not
        //command=>Draft,SaveandSubmit
        function ConfirmFutureDate(command) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Messages.Information %>'; // 'Information';
            //            msg = 'Do you want to continue new record in a future date?';
            //            $("#divConfirmation").html(msg).dialog({
            //                modal: true,
            //                height: 150,
            //                width: 350,
            //                title: msgTitle,
            //                resizable: false,
            //                buttons: {
            //                    Yes: function (e) {
            //                        $("[id$=hdfIsContFutureDate]").val(1);
            //                        $(this).dialog("close");
            //                        if (command == 'SAVE') {
            //                            $("[id$=btnSave]").click();
            //                        }
            //                        else {
            //                            $("[id$=btnSaveSubmit]").click();
            //                        }
            //                    },
            //                    Cancel: function (e) {
            //                        $("[id$=hdfIsContFutureDate]").val(0);
            //                        $(this).dialog("close");
            //                        // return false;
            //                    }
            //                }
            //            });
            // return false;
            var msg = '<%= Resources.Messages.Err_FutureDateTransactionNotAllowed %>'; // Translate(Err_FutureDateTransactionNotAllowed)//

            ShowErrorMessage(msg, msgTitle);
        }
        function ConfirmFutureDate(command) {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Messages.Information %>'; // 'Information';
            //            msg = 'Do you want to continue new record in a future date?';
            //            $("#divConfirmation").html(msg).dialog({
            //                modal: true,
            //                height: 150,
            //                width: 350,
            //                title: msgTitle,
            //                resizable: false,
            //                buttons: {
            //                    Yes: function (e) {
            //                        $("[id$=hdfIsContFutureDate]").val(1);
            //                        $(this).dialog("close");
            //                        if (command == 'SAVE') {
            //                            $("[id$=btnSave]").click();
            //                        }
            //                        else {
            //                            $("[id$=btnSaveSubmit]").click();
            //                        }
            //                    },
            //                    Cancel: function (e) {
            //                        $("[id$=hdfIsContFutureDate]").val(0);
            //                        $(this).dialog("close");
            //                        // return false;
            //                    }
            //                }
            //            });
            // return false;
            var msg = '<%= Resources.Messages.Err_FutureDateTransactionNotAllowed %>'; // Translate(Err_FutureDateTransactionNotAllowed)//

            ShowErrorMessage(msg, msgTitle);
        }

        function ReqQtyMorethanPOQtyConfirmation(command) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= Resources.Messages.ReqQtyMorethanPOQtyConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfPoQtyValidate]").val(0);
                        $(this).dialog("close");
                        //ClosePopup();
                        if (command == 'SAVE') {
                            $("[id$=btnSave]").click();
                        }
                        else {
                            //$("[id$=btnSaveSubmit]").click();
                            $("[id$=btnSubmit]").click();
                        }
                    },
                    No: function (e) {
                        $(this).dialog("close");
                    }
                }

            });
            return false;
        }

        function SetSearchType(isLoad) {
            ///<summary>Function To Enable/Disable Selected Option For Search </summary>
            ClearSearchDetails();
            var strname = $("select[id$=SearchType]").val();
            $("[id$=SearchValue]").val("");
            if (strname == "Date") {
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

        function FillPuchaseOrderAutoComplete() {

            //<summary> Function Used to make material category field as auto complete </summary>
            var PendingWo = 1;

            if ($("[id$=ChkPendingWO]").is(":checked") == true) {
                PendingWo = 1;
            }
            else {
                PendingWo = 0;
            }

            //alert(PendingWo);
            if ($("[id$=hdfMenuType]").val() == "1")//Workorder GRN
                GrandScriptUtils.MakeAutoComplete("SearchValue", GRNCreate.DirectStockTransferWOAutoURL + $("[id$=ddlAdmissionStore]").val() + "&IsPendingWO=" + PendingWo, false, true, false, "SearchType", false, "ddlSuppliers", "hdfGRH_PK", "ddlSelectDiv3");
            else
                GrandScriptUtils.MakeAutoComplete("SearchValue", GRNCreate.DirectStockTransferAutoURL + $("[id$=ddlAdmissionStore]").val(), false, true, false, "SearchType", false, "ddlSuppliers", "hdfGRH_PK", "ddlSelectDiv3");
        }

        function getFormattedDate(d) {
            var d1 = new Date(d);
            //            new Date("03/25/2015");

            var dt = d1.getDate() < 10 ? ("0" + d1.getDate()) : d1.getDate();
            var d2 = dt + "-" + getMonthMMM(d1.getMonth()) + "-" + d1.getFullYear();
            return d2;
        }

        function getMonthMMM(m) {
            var arM = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            return arM[m];
        }

        function ShowHideUploadDocDetails(flag) {
            //If flag then Show Upload Doc Details
            if (flag) {
                $("[id$=tblUploadDocDetails]").show();
                $("[id$=imbShowDetails]").hide();
                $("[id$=imbHideDetails]").show();
            }
            else {
                $("[id$=tblUploadDocDetails]").hide();
                $("[id$=imbShowDetails]").show();
                $("[id$=imbHideDetails]").hide();
            }
            return false;
        }

        //function ConfirmConvert() {
        //    if (ValidatePageNow('Convert')) {
        //        return ShowDeleteConfirm(this, 'Do you want to convert GRN qty to Pcs?')
        //    }
        //    else
        //        return false;
        //}

        function ConfirmConvert() {
            if (ValidatePageNowWithDuplicate('Convert')) {
                var msgTitle;
                var msg;
                msgTitle = '<%= Resources.ErpRes.Title_Information %>';
                msg = "Do you want to convert GRN qty to Pcs?";//'<%= Resources.Messages.MayAffectStockValueConfirmation %>';
                $("#divConfirmation").html(msg).dialog({
                    modal: true,
                    height: 150,
                    width: 275,
                    title: msgTitle,
                    resizable: false,
                    buttons: {
                        Yes: function (e) {
                            $(this).dialog("close");
                            $("[id$=btnConvertNow]").click();
                        },
                        Cancel: function (e) {
                            $(this).dialog("close");
                        }
                    }
                });
                return false;
            }
            else
                return false;
        }

        function SwitchTab(tab) {
            if (tab == 1) {
                $("[id$=hdfTabNo").val("1");
                $("#divMatReturn").hide();
                $("#divPendingItems").show();
                $("#divGRlist").show();
                $("[id$=spnPendingList]").attr('class', 'tab-active');
                $("[id$=spnMatReturnList]").attr('class', 'tab-inactive');
            }
            else if (tab == 2) {
                $("[id$=hdfTabNo").val("2");
                $("#divMatReturn").show();
                $("#divPendingItems").hide();
                $("#divGRlist").hide();
                $("[id$=spnPendingList]").attr('class', 'tab-inactive');
                $("[id$=spnMatReturnList]").attr('class', 'tab-active');
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

        function ShowWOConfirmationMsg(btn, message) {
            if ($("[id$=hdfIsValidationRequired]").val() == 1) {

            } else {
                return true;
            }

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= GetLocalResourceObject("BatchWOConfirmation").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        ShowContainerDiv('[id$=divPopupBatches]','<%= Resources.Messages.MsgAddBatches %>', '850', 'auto');
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        ShowContainerDiv('[id$=divPopupBatches]','<%= Resources.Messages.MsgAddBatches %>', '850', 'auto');
                        return false;
                    }
                }
            });
            return false;
        }

        function UpdateReturnQty() {
            $("[id$=btnUpdateReturn]").click();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <asp:HiddenField ID="hdfProcId" runat="server" />
            <asp:HiddenField ID="hdfGRH_PK" runat="server" />
            <asp:HiddenField ID="hdfGRH_VERSION" runat="server" Value="1" />
            <asp:HiddenField ID="hdfCurrentPODetIdAddRejectedReason" runat="server" />
            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
            <asp:HiddenField ID="hdfOrderPercentage" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDecimalCount" runat="server" Value="0" />
            <asp:HiddenField ID="PO_PK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
            <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsDSACancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" />
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
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="1"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="2" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="3" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="4" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="5"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="5" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li id="pnlbtnNew" runat="server" style="display: none">
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlbtnEdit" runat="server" style="display: none">
                                        <asp:Button runat="server" TabIndex="2" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li id="pnlbtnView" runat="server" style="display: none">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="3" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li runat="server" id="pnlbtnListPrint" style="display: none">
                                        <asp:Button runat="server" TabIndex="4" ID="btnListPrint" CommandName="PRINTLISTING"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlEditforCancel" style="display: none">
                                        <asp:Button runat="server" TabIndex="8" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelGRN %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelGRN %>" />
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
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="8" CssClass="input-small-b" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />

                                            <asp:Label ID="Label8" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                class="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="9" MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"
                                                CssClass="input-small"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />

                                            <div class="clear">
                                            </div>
                                            <%-----------   Plant ----------------%>
                                            <div id="divPlantCode" class="div2col-S">
                                                <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,Plant%>" AssociatedControlID="ddlPlantCode"></asp:Label>
                                                <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-w21-6per" TabIndex="12">
                                                </asp:DropDownList>
                                            </div>
                                            <%--------    End Plant ----------%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="Label3" Text="<%$ resources:Department%>" AssociatedControlID="txtDepartment"></asp:Label>
                                            <asp:TextBox ID="txtDepartment" runat="server" TabIndex="10" CssClass="select-small-c"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDepartmentPkSearch" runat="server" />

                                            <asp:Label runat="server" ID="Label7" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"
                                                class="middle-lbl-xsmall-a2"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a" TabIndex="11">
                                                <%-- <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:NotApproved %>" Value="1"></asp:ListItem>--%>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterApproved %>" Text="<%$ Resources:BindValues, StatusFilterApproved%>">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotApproved %>"
                                                    Text="<%$ Resources:BindValues, StatusFilterNotApproved%>" Selected="True">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterCancelled %>" Text="<%$ Resources:BindValues, StatusFilterCancelled%>"> </asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="Label2" runat="server" Text="<%$resources:GRNNo %>" AssociatedControlID="txtStockAdmissionNo"></asp:Label>
                                            <asp:TextBox ID="txtStockAdmissionNo" runat="server" TabIndex="12" CssClass="input-small-b margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfStockAdmissionNoPkSearch" runat="server" />
                                            <asp:Label ID="Label4" runat="server" Text="<%$resources:PoNo %>" AssociatedControlID="txtPONo"
                                                class="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox ID="txtPONo" runat="server" TabIndex="13" CssClass="input-small margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPOPkSearch" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="Label5" Text="<%$ resources:Vendor%>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" TabIndex="14" CssClass="select-half margnbotm0" Text="type min 4 characters"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorPKSearch" runat="server" />
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="15"
                                                CommandName="SEARCH" SkinID="search-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="16" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <%--Listing Grid--%>
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" TabIndex="18"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyMainList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="54" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" OnCheckedChanged="ActionHandler" CommandName="ITEMSELECTED" AutoPostBack="true" />
                                                <%--AutoPostBack="true" OnCheckedChanged="ActionHandler"--%>
                                                <asp:HiddenField ID="hdfStockTransferPk" runat="server" Value='<%# Eval("GRH_PK") %>' />

                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GRNNo %>" SortExpression="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockAdmissionNo" runat="server" Text='<%# (Eval("GRH_NO")).ToString()==string.Empty?"[NEW]": (Eval("GRH_NO")).ToString() %>'
                                                    ToolTip='<%# (Eval("GRH_NO")).ToString()==string.Empty?"[NEW]": (Eval("GRH_NO")).ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" Font-Bold="true" runat="server" Text='<%# Eval("CMP_DISPLAY_CODE") %>' ToolTip='<%# Eval("CMP_DISPLAY_CODE") %>' CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockAdmissionDate" runat="server" Text='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PoNo %>" SortExpression="PoNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("GRH_PO_NO").ToString()),35) %>'
                                                    ToolTip='<%# Eval("GRH_PO_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SupplierRefNo %>" SortExpression="PoNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierRefNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("GRH_VND_REF_NO").ToString()),17) %>'
                                                    ToolTip='<%# Eval("GRH_VND_REF_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("GRH_VENDOR_TEXT").ToString()),44) %>'
                                                    ToolTip='<%# Eval("GRH_VENDOR_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepartment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("DPT_NAME").ToString()),16) %>'
                                                    ToolTip='<%# Eval("DPT_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("GRH_INV_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("GRH_STATUS_TEXT").ToString()),9) %>'
                                                    ToolTip='<%# Eval("GRH_STATUS_TEXT") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfStatusList" runat="server" Value='<%# Eval("GRH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--     <asp:ImageButton ID="imgConvert" runat="server"  ImageUrl="../images/BlueExt/Icons/reback-green.png" ToolTip="Convert" CommandName="CONVERT"
                                                    Visible='<%# (Convert.ToInt32(Eval("GRH_IS_CONVERSION_REQD")) == 1 && Convert.ToInt32(Eval("GRH_STATUS")) == 2) ? true : false %>' />--%>
                                                <asp:Button ID="imgConvert" runat="server" CssClass="btnConvert" CommandName="CONVERT"
                                                    Visible='<%# (Convert.ToInt32(Eval("GRH_IS_CONVERSION_REQD")) == 1 && Convert.ToInt32(Eval("GRH_STATUS")) == 2) ? true : false %>' />
                                                <asp:HiddenField ID="hdfConversionRequired" runat="server" Value='<%# Eval("GRH_IS_CONVERSION_REQD") %>' />
                                                <asp:HiddenField ID="hdfConverted" runat="server" Value='<%# Eval("GRH_IS_CONVERTED") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                                            <asp:Label ID="lblDepreNo" runat="server" Text="<%$ resources:DirectStockAdmissionNo%>"
                                                AssociatedControlID="lblDirectStockAdmissionNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDirectStockAdmissionNo" CssClass="input-small"></asp:Label>
                                            <%-- <asp:HiddenField ID="hdfDepreciationNo" runat="server" Value="" />--%>
                                            <asp:Label runat="server" ID="lblAdmissionDate" class="middle-lbl" Text="<%$ resources:Date%>" AssociatedControlID="txtAdmissionDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAdmissionDate" CssClass="input-small" TabIndex="7"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqStockDateEntry" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="txtAdmissionDate" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_ReqStockDt %>">
                                            </asp:RequiredFieldValidator>

                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:SupplierRefNo%>" AssociatedControlID="txtSupplierRefNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSupplierRefNo" TabIndex="9" CssClass="input-small"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label12" Text="<%$ resources:RefDate%>" AssociatedControlID="txtAdmissionDate" class="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefDate" CssClass="input-small" TabIndex="10"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>

                                            <div id="divPlantCompany">
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:CompanyPlant%>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" TabIndex="12" runat="server" onmouseover="javascript:ShowTooltip('ddlCompany');"
                                                    CssClass="select-small-j">
                                                </asp:DropDownList>
                                            </div>
                                            <%--<asp:RequiredFieldValidator ID="reqFromDateEntry" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtFromDateEntry"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqFromDt %>">
                                            </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S" style="float: right;">
                                            <asp:Label runat="server" ID="lblSuppliers" Text="<%$ resources:Suppliers%>" AssociatedControlID="ddlSuppliers"></asp:Label>
                                            <asp:DropDownList ID="ddlSuppliers" TabIndex="8" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                CssClass="select-half" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqSuppliers" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlSuppliers" Display="Dynamic"
                                                Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectSupplier %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="Label11" Text="<%$ resources:AdmissionStore%>" AssociatedControlID="ddlAdmissionStore"></asp:Label>
                                            <asp:DropDownList ID="ddlAdmissionStore" TabIndex="11" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                CssClass="select-half" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqAdmissionStore" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlAdmissionStore" Display="Dynamic"
                                                Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectAdmissionStore %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--<h3 class="fontWGT-Nrml" id="ListHead" runat="server">                                
                            </h3>--%>

                            <div id="divTabContainer">
                                <ul id="tab-menu">
                                    <li>
                                        <span id="spnPendingList" runat="server" class="tab-active">
                                            <%--<a href="#" onclick="SwitchTab(1)" runat="server" id="aTab1"></a>--%>
                                            <asp:LinkButton runat="server" ID="lnkPendingList" Text=""
                                                CommandArgument="1" TabIndex="6" OnClick="ActionHandler" CommandName="DEFAULT"
                                                CssClass="tab-inactive" OnClientClick="SwitchTab(1)"></asp:LinkButton>
                                        </span>
                                    </li>
                                    <li>
                                        <span id="spnMatReturnList" runat="server" class="tab-inactive">
                                            <%--<a href="#" onclick="SwitchTab(2)" runat="server" id="aTab2"><%=GetLocalResourceObject("MaterialReturn").ToString() %></a>--%>
                                            <asp:LinkButton runat="server" ID="lnkMatReturn" Text="<%$ resources:MaterialReturn %>"
                                                CommandArgument="2" TabIndex="6" OnClick="ActionHandler" CommandName="EDITFORRETURN"
                                                CssClass="tab-inactive" OnClientClick="SwitchTab(2)"></asp:LinkButton>
                                        </span>
                                    </li>
                                </ul>
                            </div>

                            <div class="clear"></div>

                            <div id="divPendingItems" runat="server">
                                <div class="gridwrap">
                                    <div id="searchwrap" class="search-wrap-custom1">
                                        <label>
                                            <%=Resources.Controls.SearchBy%></label>
                                        <asp:DropDownList ID="SearchType" TabIndex="13" runat="server" CssClass="srchboxtextbx"
                                            onchange="javascript:SetSearchType(true);">
                                            <%-- <asp:ListItem Value="POH_NO" Text="<%$ Resources:BindValues, PONumber%>">
                                        </asp:ListItem>--%>
                                            <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Item%>">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Date" Text="Date Range">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <div id="divSearchDtls">
                                            <asp:TextBox ID="SearchValue" runat="server" TabIndex="14">
                                            </asp:TextBox>
                                        </div>
                                        <div id="divDate">
                                            <label for="FromDate">
                                                <%=Resources.Controls.FromDate%></label>
                                            <asp:TextBox ID="FromDate" runat="server" TabIndex="14" EnableViewState="true">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFrmDate" runat="server" EnableViewState="true" />
                                            <label for="ToDate">
                                                <%=Resources.Controls.ToDate%></label>
                                            <asp:TextBox ID="ToDate" runat="server" TabIndex="14" EnableViewState="true">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfPOSearchToDate" runat="server" EnableViewState="true" />
                                        </div>
                                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="14" OnClick="ActionHandler"
                                            CommandName="" />
                                        <div id="DivChkPendingWO" runat="server">
                                            <asp:CheckBox ID="ChkPendingWO" runat="server" ToolTip="Pending WO" AutoPostBack="true" Checked="true" OnCheckedChanged="ActionHandler" />
                                            <asp:Label ID="lblchkpendingWO" runat="server" Text="Pending WO"></asp:Label>
                                        </div>
                                    </div>
                                    <asp:GridView runat="server" ID="grdPOs" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" TabIndex="15"
                                        OnPreRender="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyPOList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAllPOList" runat="server" onclick="CheckAllPO(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectPOList" runat="server" Checked='<%# Eval("CheckBoxChecked") %>' />
                                                    <%--Checked="<%# Eval("AddedToStockList")==1?true:false %>"--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="1.5%" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton SkinID="arrived" runat="server" Visible='<%# Eval("AddedToStockList") %>'
                                                    OnClientClick="javascript:return false;" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="<%$ resources:PoNo %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfWIHItemType" Value='<%# Eval("WIH_ITEM_TYPE") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfWIHItemTypeText" Value='<%# Eval("WIH_ITEM_TYPE_TEXT") %>' />

                                                    <asp:HiddenField runat="server" ID="hdfPOIdPOList" Value='<%# Eval("POId") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPOIdStockAdmissionList" Value='<%# Eval("POId") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPODetIdPOList" Value='<%# Eval("PODetId") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPOListSelected" Value='<%# Eval("AddedToStockList") %>' />
                                                    <%--<asp:Label ID="lblPONoPOList" runat="server" Text='<%# Eval("PONumber") %>' ToolTip='<%# Eval("PONumber")%>'></asp:Label>--%>
                                                    <asp:LinkButton ID="lbnPONoAdmissionList" runat="server" ToolTip='<%# Eval("PONumber")%>'
                                                        CommandName="PODetails" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("PONumber").ToString()),17) %>'
                                                        Style="text-decoration: underline;" OnClientClick="return showPODetails(this);"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="hdfItemId" Value='<%# Eval("ItemId") %>' />
                                                    <asp:Label ID="lblItemPOList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ItemName").ToString()),100) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="50%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUomPOList" runat="server" Text='<%# Eval("UOM")%>' ToolTip='<%# Eval("UOM")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:POQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPoQtyPOList" runat="server" Text='<%# Eval("POQty", this.GetCurrencyFormat())%>'
                                                        ToolTip='<%# Eval("POQty", this.GetCurrencyFormat())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PreviousRecievedQty%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreRecievQtyPOList" runat="server" Text='<%# Eval("PreRecievedQty", this.GetCurrencyFormat())%>'
                                                        ToolTip='<%# Eval("PreRecievedQty", this.GetCurrencyFormat())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9.5%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PendingQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPendingQtyPOList" runat="server" Text='<%# Eval("BalanceQtyToRecieve", this.GetCurrencyFormat())%>'
                                                        ToolTip='<%# Eval("BalanceQtyToRecieve", this.GetCurrencyFormat())%>'></asp:Label>
                                                    <%--Eval(Resources.DataFieldRes.AssetPurchaseCost,this.GetCurrencyFormat())--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ReturnQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReturnQtyPOList" runat="server" Text='<%# Eval("POD_QTY_RETURNED", this.GetCurrencyFormat())%>'
                                                        ToolTip='<%# Eval("POD_QTY_RETURNED", this.GetCurrencyFormat())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                    OnPreRender="btnAction_PreRender" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div style="text-align: right;">
                                    <%-- <div class="button-wrap-right">--%>
                                    <asp:Button ID="btnAddToStockAdmissionList" runat="server" TabIndex="16" Text="<%$ Resources:Controls, AddToList%>"
                                        CommandName="ADDTOLIST" OnClick="ActionHandler" />
                                    <%-- </div>--%>
                                </div>
                            </div>

                            <div id="divMatReturn" runat="server">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdMaterialReturn" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" TabIndex="15"
                                        OnPreRender="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyPOList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:ItemType %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemType" runat="server" Text='<%# Eval("ItemType")%>'
                                                        ToolTip='<%# Eval("ItemType")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfItemTypePK" runat="server" Value='<%# Eval("ItemTypePK")%>' />
                                                    <asp:HiddenField ID="hdfIsAfterMulti" runat="server" Value='<%# Eval("GMR_AFTER_MULTI")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:WoNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWONo" runat="server" Text='<%# Eval("WONo")%>'
                                                        ToolTip='<%# Eval("WONo")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfMatWOPK" runat="server" Value='<%# Eval("WOPK")%>' />
                                                    <asp:HiddenField ID="hdfMatSlNo" runat="server" Value='<%# Eval("SlNo")%>' />
                                                    <asp:HiddenField ID="hdfScrapOrBgrade" runat="server" Value='<%# Eval("GMR_SCRAP_OR_BGRADE")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Category %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("Category")%>'
                                                        ToolTip='<%# Eval("Category")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfCategoryPK" runat="server" Value='<%# Eval("CategoryPK")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:IssueItem %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIssueItem" runat="server" Text='<%# Eval("IssueItem")%>'
                                                        ToolTip='<%# Eval("IssueItem")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfIssueItemPK" runat="server" Value='<%# Eval("IssueItemPK")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMatUOM" runat="server" Text='<%# Eval("UOM")%>'
                                                        ToolTip='<%# Eval("UOM")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdfMatUOMPK" runat="server" Value='<%# Eval("UOMPK")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BatchNo %>">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="select-full-100"
                                                        OnSelectedIndexChanged="ActionHandler" CommandName="SELECTEDINDEXCHANGED" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="grdText padglft-0 padgrgt-6">
                                                <ItemTemplate>
                                                    <div style="min-width: 15px;">
                                                        <asp:ImageButton CssClass="nomargin" ID="imbAddGridBatch" SkinID="add-batches" runat="server"
                                                            OnClick="ActionHandler" ToolTip="<%$ resources:AddBatches %>" CommandName="ADD_ACTION"
                                                            TabIndex="46"></asp:ImageButton>
                                                        <asp:ImageButton CssClass="nomargin" ID="imbClearGridBatch" SkinID="clear-batches"
                                                            runat="server" OnClick="ActionHandler" ToolTip="<%$ resources:btnclrbatches %>"
                                                            CommandName="CLEARADD" Visible="false" TabIndex="48"></asp:ImageButton>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Controls,Stock %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReturnStockQty" runat="server"></asp:Label>
                                                    <asp:HiddenField ID="hdfBatchUOMPK" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdfBatchUOM" runat="server" Value="" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:RequiredQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequiredQty" runat="server" Text='<%# GetFormattedNumber(Eval("RequiredQty"))%>'
                                                        ToolTip='<%# GetFormattedNumber(Eval("RequiredQty"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PreviousRecievedQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRecQty" runat="server" Text='<%# GetFormattedNumber(Eval("PrevReceivedQty"))%>'
                                                        ToolTip='<%# GetFormattedNumber(Eval("PrevReceivedQty"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ReturnQty %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblReturnQty" runat="server" Text='<%# (  (Eval("MaterialReturnPK")).ToString()=="0" && (Eval("IsValueChanged")).ToString()=="0" )?"0.00": GetFormattedNumber((Eval("ReturnQty"))) %>'
                                                        CssClass="small-a numeric" onblur="UpdateReturnQty();"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfBalanceQty" runat="server" Value='<%# Eval("BalanceQty")%>' />
                                                    <asp:HiddenField ID="hdfReturnQty" runat="server" Value='<%# Eval("ReturnQty")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <%-- <h3 class="fontWGT-Nrml">
                                <%= GetLocalResourceObject("MtaerialReturn").ToString()%>
                            </h3>--%>
                            <div id="divGRlist" runat="server">
                                <h3 class="fontWGT-Nrml">
                                    <%= GetLocalResourceObject("StockAdmissionList").ToString()%>
                                </h3>
                                <div class="scroll-container">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdStockAdmissionList" Width="1500px" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable" TabIndex="17" OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler"
                                            EnableViewState="true">
                                            <%--OnRowDataBound="ActionHandler"--%>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyAdmissionList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:PoNo %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hdfPkStockAdmissionList" Value='<%# Eval("Pk") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfPODetIdStockAdmissionList" Value='<%# Eval("PODetailId") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfPOIdStockAdmissionList" Value='<%# Eval("POId") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfPONumber" Value='<%# Eval("PONumber") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfPOrate" Value='<%# Eval("PORate") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfUOM" Value='<%# Eval("UOM") %>' />
                                                        <asp:LinkButton ID="lbnPONoAdmissionList" runat="server" ToolTip='<%# Eval("PONumber")%>'
                                                            CommandName="PODetails" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("PONumber").ToString()),15) %>'
                                                            Style="text-decoration: underline;" OnClientClick="return showPODetails(this);"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hdfItemIdStockAdmissionList" Value='<%# Eval("ItemId") %>' />
                                                        <asp:Label ID="lblItemAdmissionList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ItemName").ToString()),72) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString())%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:POQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPoQtyAdmissionList" runat="server" Text='<%# Eval("POQty",this.GetCurrencyFormat())%>'
                                                            ToolTip='<%# Eval("POQty",this.GetCurrencyFormat())%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PendQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPendingQtyAdmissionList" runat="server" Text='<%# Eval("PendingQty", this.GetCurrencyFormat())%>'
                                                            ToolTip='<%# Eval("PendingQty", this.GetCurrencyFormat())%>'></asp:Label>
                                                        <asp:HiddenField ID="hdfPendingQtyAdmissionList" runat="server" Value='<%# Eval("PendingQty")%>' />
                                                        <%--Eval(Resources.DataFieldRes.AssetPurchaseCost,this.GetCurrencyFormat())--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="<%$ resources:RetQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReturnQtyAdmissionList" runat="server" Text='<%# Eval("ReturnQty", this.GetCurrencyFormat())%>'
                                                        ToolTip='<%# Eval("ReturnQty", this.GetCurrencyFormat())%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="<%$ resources:Received %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtReceivedAdmissionList" runat="server" CssClass="small-a numeric"
                                                            Text='<%# GetFormattedNumber(Eval("Recieved")) %>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                                        <%-- <asp:CustomValidator ID="cvalReceived" runat="server" EnableClientScript="true" ClientValidationFunction="recievedTextBoxValidate"
                                                        ControlToValidate="txtReceivedAdmissionList" ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>"
                                                        Text="*" Display="Dynamic" ValidationGroup="Save" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>--%>
                                                        <asp:RequiredFieldValidator ID="reqReceived" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Save" runat="server" ControlToValidate="txtReceivedAdmissionList"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                                                        </asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5.5%" CssClass="amount-numeric" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Accepted %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAcceptedAdmissionList" runat="server" CssClass="small-a numeric input-disabled"
                                                            Enabled="false" Text='<%# GetFormattedNumber(Eval("Accepted")) %>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                                        <%--  <asp:CustomValidator ID="cvalAccepted" runat="server" EnableClientScript="true" ClientValidationFunction="acceptedTextBoxValidate"
                                                        ControlToValidate="txtAcceptedAdmissionList" ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>"
                                                        Text="*" Display="Dynamic" ValidationGroup="Save" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>--%>

                                                        <asp:HiddenField runat="server" ID="hdftxtAcceptedAdmissionList" Value='<%# Eval("Accepted") %>' />
                                                        <asp:RequiredFieldValidator ID="reqAccepted" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="Save" runat="server" ControlToValidate="txtAcceptedAdmissionList"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                                                        </asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5.5%" CssClass="amount-numeric" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Rejected %>">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="txtRejectedAdmissionList"  runat="server" CssClass="small-a numeric"
                                                        Text='<%# Eval("Rejected", this.GetCurrencyFormat()) %>' 
                                                        onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:Label>--%>
                                                        <asp:TextBox ID="txtRejectedAdmissionList" runat="server" CssClass="small-a numeric"
                                                            Text='<%# GetFormattedNumber(Eval("Rejected")) %>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                                        <%-- <asp:CustomValidator ID="cvalRejected" runat="server" EnableClientScript="true" ClientValidationFunction="rejectedTextBoxValidate"
                                                        ControlToValidate="txtRejectedAdmissionList" ValidateEmptyText="true" Text="*"
                                                        ErrorMessage="<%$ resources:Err_EnterValidQuantity %>" Display="Dynamic" ValidationGroup="Save"
                                                        CssClass="star" SetFocusOnError="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5.5%" CssClass="amount-numeric" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:RejectedReason %>">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnReasonAddStockAdmissionList" Text="<%$ resources:Add %>" runat="server"
                                                            CommandName="RejectedAddClick" Style="text-decoration: underline;" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:QALotNo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQaLotNoAdmissionList" runat="server" Text='<%# Eval("QALotNo") %>'
                                                            CssClass="small-a" ToolTip='<%# Eval("QALotNo") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5.5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:BatchNo %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBatchNoAdmissionList" runat="server" Text='<%# Eval("BatchNo") %>'
                                                            CssClass="medium-a" ToolTip='<%# Eval("BatchNo") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5.5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DOM %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDOMAdmissionList" runat="server" CssClass="small-a" Text='<%# Eval("DOM") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDOMAdmissionList" runat="server" Value='<%# Eval("DOM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DOE %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDOEAdmissionList" runat="server" CssClass="small-a" Text='<%# Eval("DOE") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfDOEAdmissionList" runat="server" Value='<%# Eval("DOE") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Location %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtLocationAdmissionList" runat="server" Text='<%# Eval("Location") %>'
                                                            CssClass="medium" ToolTip='<%# Eval("Location") %>'></asp:TextBox>
                                                        <asp:HiddenField ID="hdfLocationAdmissionPK" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:RealWeight %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRealQTYAdmissionList" runat="server" Text='<%# Eval("RealWeight") %>'
                                                            CssClass="small-a numeric" ToolTip='<%# Eval("RealWeight") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemoveAdmissionList" runat="server" CommandName="RemoveAdmissionItem"
                                                            SkinID="delete-icon" ToolTip="Delete" />
                                                        <%--OnClientClick="return ShowDeleteConfirm(this);" OnClick="ActionHandler" --%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%-- file upload start here--%>
                            <h1 class="search-colapse-normal">
                                <%=Resources.Controls.Attachments%>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideUploadDocDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideUploadDocDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" />
                                </div>
                            </h1>
                            <div class="clear">
                            </div>
                            <div class="fields-group">
                                <table class="table-devide" id="tblUploadDocDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblFileUpload" runat="server" Text="AttachFile" AssociatedControlID="fupUpload"></asp:Label>
                                                <asp:FileUpload ID="fupUpload" runat="server" TabIndex="26" Style="width: 15.6%;" />
                                                <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                        
                                                </asp:RequiredFieldValidator>
                                                <a id="anchorFile" runat="server" target="_blank" tabindex="11"></a>
                                                <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="26"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="upload"
                                                    Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                                <%--  <div class="btnwrap-divcol">
                                            
                                        </div>--%>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="gridwrap">
                                                <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                    AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="27" EmptyDataRowStyle-CssClass="emptytable">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                            <ItemTemplate>
                                                                <%--<%# Container.DataItemIndex + 1 %>--%>
                                                                <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("DOC_SEQ_NO") %>' ToolTip='<%# Eval("DOC_SEQ_NO") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="4%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="90%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a runat="server" id="fileView" class="download-icon nomargin" title="<%$ resources:View %>"
                                                                    target="_blank" href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD" TabIndex="27"
                                                                    SkinID="edit-icon" ToolTip="Edit" CommandArgument="PageAction_Entry" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD" TabIndex="27"
                                                                    SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
            </div>
            <%-- file upload end here--%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
            </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <%-- <asp:ValidationSummary ID="vsDetails" ValidationGroup="FormulationDetails" runat="server" />--%>
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                <asp:ValidationSummary ID="vsConvert" ValidationGroup="Convert" runat="server" />
                <asp:ValidationSummary ID="vsnewentryPopup" ValidationGroup="newentryPopup" runat="server" />
            </div>
            <div id="divConvert" style="display: none">
                <div class="Button-container button-container-1">
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="CONV_Panel" CssClass="" HorizontalAlign="Right">
                                <ul>
                                    <li>
                                        <asp:Button runat="server" ID="btnConvert" TabIndex="2" Text="<%$resources:Convert %>"
                                            OnClientClick="return ConfirmConvert();" ValidationGroup="Convert"
                                            ToolTip="<%$resources:Convert %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnDeleteConvert" CommandName="DELETECONVERSION" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="4" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <h3 class="fontWGT-Nrml"><%= GetLocalResourceObject("ItemReceived").ToString()%></h3>
                <div class="gridwrap">
                    <asp:GridView ID="grdReceived" runat="server" Width="100%" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyReceived" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText='<%$ resources:WoNo %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblWONumber" runat="server" Text='<%# Eval("WIH_NO") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfWOPK" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdfSBDPK" runat="server" Value='<%# Eval("SBD_PK") %>' />
                                    <asp:HiddenField ID="hdfGRNPK" runat="server" Value='<%# Eval("GRH_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:Item %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("ITM_NAME") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="65%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:UOM %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("ITM_UOM_CODE") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfUOMPK" runat="server" Value='<%# Eval("ITM_UOM") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:ReceivedQty %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedQty" runat="server" Text='<%# Eval("GRD_QTY_RECEIVED") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfReceivedQty" runat="server" Value="0" />
                                </ItemTemplate>
                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stock Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblStockQty" runat="server" Text='<%# Eval("BALANCE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <h3 class="fontWGT-Nrml"><%= GetLocalResourceObject("ItemToBeConverted").ToString()%></h3>
                <div class="gridwrap">
                    <asp:GridView ID="grdConvert" runat="server" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyConvert" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText='<%$ resources:WoNo %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblConvWONo" runat="server" Text='<%# Eval("WIH_NO") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:Item %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblConvItem" runat="server" Text='<%# Eval("ITM_NAME") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfConvItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="65%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:UOM %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblConvUOM" runat="server" Text='<%# Eval("ITM_UOM_CODE") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfConvUOMPK" runat="server" Value='<%# Eval("ITM_UOM") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:ConvertedQty %>'>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtConvQty" runat="server" Text='<%# Eval("BALANCE") %>' CssClass="small-a numeric"
                                        onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqReceived" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="Convert" runat="server" ControlToValidate="txtConvQty"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                                    </asp:RequiredFieldValidator>
                                </ItemTemplate>
                                <ItemStyle Width="20%" CssClass="amount-numeric" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div style="display: none">
                <asp:Button ID="btnConvertNow" runat="server" CommandName="CONVERT" OnClick="ActionHandler" />
                <asp:Button ID="btnUpdateReturn" runat="server" CommandName="UPDATERETURN" OnClick="ActionHandler" />
            </div>
            <div id="DivRejectedDtlsPopUp" style="display: none;">
                <div class="Button-container-popup" id="btnContainer">
                    <asp:Button runat="server" ID="btnDmgClear" SkinID="btnInner-Cancel" Text="Clear"
                        OnClick="ActionHandler" CommandName="CLEARITEM" />
                    <asp:Button runat="server" ID="btnSaveDamage" SkinID="btnInner-Save" Text="Save"
                        OnClick="ActionHandler" CommandName="SAVE_ACTIONPOPUP" ValidationGroup="SavePopUp" />
                </div>
                <div id="damagedetailsDiv" class="content-wrapper">
                    <div id="addDamageDetails">
                        <div class="divcol-P1">
                            <%--<div class="clear"></div>--%>
                            <label for="lblItemNamePopup">
                                <%=Resources.Controls.Item%></label><asp:Label runat="server" ID="lblItemNamePopup"
                                    Text=""></asp:Label>
                            <label for="lblQtyRejectedPopup">
                                <%=Resources.Controls.TotalRejectedQty%></label><asp:Label runat="server" ID="lblQtyRejectedPopup"
                                    Text=""></asp:Label>
                            <label for="txtDamageQtyPopup" class="floatLeft">
                                <%=Resources.Controls.Quantity%>*</label>
                            <asp:TextBox ID="txtDamageQtyPopup" runat="server" CssClass="small-a numeric" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rvtxtDamageQtyPopup" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="SavePopUp" runat="server" ControlToValidate="txtDamageQtyPopup"
                                Display="Static" Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                            </asp:RequiredFieldValidator>
                            <div class="clear">
                            </div>
                            <label for="ddlRejectionReasonPopup" class="floatLeft">
                                <%=Resources.Controls.RejectionReason%>*</label>
                            <asp:DropDownList ID="ddlRejectionReasonPopup" runat="server">
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="ddlRejectToDept">
                                <%=Resources.Controls.RejectTo%>*</label><asp:DropDownList ID="ddlRejectToDept" runat="server">
                                </asp:DropDownList>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <%--                    <asp:HiddenField ID="hdfPODetailIdDamageList" runat="server" Value="0" />--%>
                    <asp:HiddenField ID="hdfWOStatus" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfDamageSlNo" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfMenuType" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfSubContractStorePK" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfIsStockExceeded" runat="server" Value="0" />
                    <%--<asp:HiddenField ID="GRNPK" runat="server" Value="0" />
                    <asp:HiddenField ID="GRH_PK" runat="server" Value="0" />
                    <asp:HiddenField ID="ItemPk" runat="server" Value="0" />
                    <asp:HiddenField ID="GDD_GRN_DTL" runat="server" Value="0" />
                    <asp:HiddenField ID="RejQty" runat="server" Value="0" />
                    <asp:HiddenField ID="EditMode" runat="server" Value="0" />
                    <asp:HiddenField ID="EditQty" runat="server" Value="0" />
                    <asp:HiddenField ID="EditReasonType" runat="server" Value="0" />
                    <asp:HiddenField ID="EditStore" runat="server" Value="0" />--%>
                    <div class="gridwrap max-250">
                        <asp:GridView runat="server" ID="grdDamageDetails" Width="100%" AutoGenerateColumns="false"
                            EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyDamageList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hdfPkDamageList" Value='<%# Eval("Pk") %>' />
                                        <asp:HiddenField runat="server" ID="hdfSlNoDamageList" Value='<%# Eval("SlNo") %>' />
                                        <asp:HiddenField runat="server" ID="hdfItemIdDamageList" Value='<%# Eval("ItemId") %>' />
                                        <asp:Label ID="lblItemDamageList" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ItemText").ToString()),14) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemText").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="30%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Quantity %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyDamageList" runat="server" Text='<%# Eval("Quantity",this.GetCurrencyFormat())%>'
                                            ToolTip='<%# Eval("Quantity",this.GetCurrencyFormat())%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" CssClass="amount-numeric" />
                                    <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Reason %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReasonDamageList" runat="server" Text='<%# Eval("ReasonText") %>'
                                            ToolTip='<%# Eval("ReasonText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Store %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRejectedToStore" runat="server" Text='<%# Eval("RejectedToStoreText") %>'
                                            ToolTip='<%# Eval("RejectedToStoreText")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Action %>" ItemStyle-Width="6.7%">
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditRejectedDetails"
                                            SkinID="imbeditgrid" EnableViewState="false" CommandName="EDIT_ACTION" />
                                        <%--OnClick="ActionHandler" CommandArgument="OrderProductDetails"--%>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteRejectedDetails"
                                            SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION" />
                                        <%--OnClick="ActionHandler"  CommandArgument="OrderProductDetails"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <%--Batch Popup div Start--%>
            <div id="divPopupBatches" style="display: none">
                <div class="content-wrapper">
                    <asp:Panel runat="server" ID="pnlPopup" Style="max-height: 350px; overflow-y: auto;">
                        <div class="Button-container-popup">
                            <asp:Button ID="btnApplyBatches" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Apply %>"
                                OnClick="ActionHandler" CommandName="APPLY" />
                        </div>
                        <table style="font-weight: bold; background: #f1f1f1; margin-bottom: 10px;">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblItemPopup" Text="" CssClass="hgt-auto"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfItemPopup" Value="0"></asp:HiddenField>
                                    <asp:HiddenField runat="server" ID="hdfItemTypePopup" Value="0"></asp:HiddenField>
                                    <asp:HiddenField runat="server" ID="hdfSlNo" Value="0" />
                                    <asp:HiddenField runat="server" ID="hdfCurrentWOPK" Value="0" />
                                    <asp:HiddenField runat="server" ID="hdfIsValidationRequired" Value="0" />
                                </td>
                                <td style="text-align: right; min-width: 25%;">
                                    <asp:Label runat="server" ID="lblUomName" Text="<%$ Resources:TextUom %>"></asp:Label>
                                    <asp:Label runat="server" ID="lblUomNamePopup" Text="" CssClass="hgt-auto"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfUOMPopup" Value="0"></asp:HiddenField>
                                </td>
                            </tr>
                        </table>
                        <table class="gridwraptable gridwrap filter-arrow" style="margin-bottom: -4px!important;">
                            <tr>
                                <th width="24%">
                                    <%=GetGlobalResourceObject("Controls","BatchNo")%>
                                    <asp:RequiredFieldValidator ID="vrfLatexBatchesPopUp" SetFocusOnError="true" ValidationGroup="newentryPopup"
                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="ddlBatchesPopUp"
                                        Text="*" ErrorMessage="<%$ resources:Msg_SelectBatch %>" Display="Dynamic" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:DropDownList runat="server" ID="ddlBatchesPopUp" Width="210px" TabIndex="1"
                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" onchange="Page_BlockSubmit = false;"
                                        ValidationGroup="none">
                                    </asp:DropDownList>
                                </th>
                                <th style="vertical-align: top;" width="35%">
                                    <%=GetLocalResourceObject("WoNo")%>
                                    <div class="clear">
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblBatchWOPopup" Text=""></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfBatchWOPKPopup" />
                                    </div>
                                </th>
                                <th class="grd-head-rgt" style="vertical-align: top;" width="13%">
                                    <%=GetGlobalResourceObject("Controls","Stock")%>
                                    <div class="clear">
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblStockPopUp" Text=""></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfBatchPopupUOM" />
                                    </div>
                                </th>
                                <th class="grd-head-rgt" width="22%">
                                    <%=GetGlobalResourceObject("Controls","Quantity")%>
                                    <asp:RequiredFieldValidator ID="vrcLatexQtyPopUp" SetFocusOnError="true" ValidationGroup="newentryPopup"
                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="txtQtyPopUp"
                                        Text="*" ErrorMessage="<%$ resources:Msg_ActualQty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="vcfLatexQtyPopUp" runat="server" ErrorMessage="<%$ resources:QtymustbeGreaterZero %>"
                                        Operator="GreaterThan" CssClass="star" ValueToCompare="0" ValidationGroup="newentryPopup"
                                        Text="*" SetFocusOnError="true" ControlToValidate="txtQtyPopUp" Display="Dynamic"
                                        Type="Double"></asp:CompareValidator>
                                    <div class="clear">
                                    </div>
                                    <asp:TextBox runat="server" ID="txtQtyPopUp" Style="float: left;" CssClass="numeric input-w110"
                                        TabIndex="2" autocomplete="off" MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                </th>
                                <th width="6%" style="vertical-align: top;">
                                    <%=GetGlobalResourceObject("Controls","Action")%>
                                    <div class="clear">
                                    </div>
                                    <div style="text-align: center; margin-top: 2px;">
                                        <asp:ImageButton runat="server" ID="imgAddBatchPopup" SkinID="imbaddnew" OnClick="ActionHandler"
                                            TabIndex="3" CommandName="ADDBATCHES" ValidationGroup="newentryPopup"
                                            OnClientClick="javascript:return ValidatePageNowWithDuplicate('newentryPopup') && ShowWOConfirmationMsg(this);" />
                                    </div>
                                    <asp:HiddenField runat="server" ID="hdnPopupstatus" Value="0" />
                                </th>
                            </tr>
                        </table>
                        <asp:GridView ID="grdBatchDetailsPopup" runat="server" CssClass="grdTable" AutoGenerateColumns="False"
                            AllowPaging="false" HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowHeader="false" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                            OnRowDataBound="ActionHandler" OnRowCommand="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchNoGridPopup" runat="server" Text='<%# Eval("Batch") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfBatchPopup" Value='<%# Eval("BatchPK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfHasRowColor" Value='<%# Eval("HasRowcolor") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="24%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchWOGridPopup" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WONos"),26) %>'
                                            ToolTip='<%# Eval("WONos") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdfBatchWOGridPopuo" Value='<%# Eval("WOPKs") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockGridPopup" runat="server" Text='<%# GetFormattedNumber(Eval("StockQty")) %>'
                                            CssClass="numeric"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="13%" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblQtyGridPopupTotal" runat="server" Text="<%$ resources:totalbatchqty %>"
                                            CssClass="numeric" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyGridPopup" runat="server" Text='<%# GetFormattedNumber(Eval("ActualQty")) %>'
                                            CssClass="numeric"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblPopupFooterTotal" runat="server" CssClass="numeric" />
                                    </FooterTemplate>
                                    <ItemStyle Width="22%" />
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
                        <table style="font-weight: bold; background: #f1f1f1;">
                            <tr>
                                <td style="width: 45%;"></td>
                                <td style="width: 27%; text-align: right;">
                                    <asp:Label runat="server" ID="Label13" Text="<%$ resources:TotalQty %>" CssClass="numeric"></asp:Label>
                                </td>
                                <td style="width: 21%; text-align: right;">
                                    <asp:Label runat="server" ID="lblTotalQtyPopup" Text=""></asp:Label>
                                </td>
                                <td style="width: 7%;"></td>
                            </tr>
                        </table>
                        <table style="font-weight: bold;">
                            <tr>
                                <td style="width: 45%;"></td>
                                <td style="width: 27%; text-align: right;">
                                    <asp:Label runat="server" ID="Label14" Text="<%$ resources:balQty %>" CssClass="numeric"></asp:Label>
                                </td>
                                <td style="width: 21%; text-align: right;">
                                    <asp:Label runat="server" ID="lblBalanceQty" Text=""></asp:Label>
                                </td>
                                <td style="width: 7%;"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
            <%--Popup div End--%>

            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfindate" Value="0" runat="server" />
                <asp:HiddenField ID="hdfIsMultipleClick" Value="0" runat="server" />
                <asp:HiddenField ID="hdfGrnSupplierLocation" Value="0" runat="server" />
                <asp:HiddenField ID="hdfGrnExcessQty" Value="0" runat="server" />
                <asp:HiddenField ID="hdfPoQtyValidate" Value="1" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
