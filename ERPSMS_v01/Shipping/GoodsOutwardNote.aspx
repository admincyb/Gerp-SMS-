<%@ Page Title="<%$ Resources:Captions,Title_ShippingGON %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="GoodsOutwardNote.aspx.cs" Inherits="ERPSMS_v01.Shipping.GoodsOutwardNote"
    ValidateRequest="false" Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var NumberDgiits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", "dd-M-yy", false, false, false);
            GrandScriptUtils.DatePickerCommon("txtCYDate");
            GrandScriptUtils.DatePickerCommon("txtRtnDate");
            GrandScriptUtils.DatePickerCommon("txtDateOfShipment");
            GrandScriptUtils.DatePickerCommon("txtBookingDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDespatchNumber", url, "hdfDPHPK", true, true, "DELIVERYORDERNUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "TRANSPORTER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCompany", url, "hdfCompany", true, true, "TRANSPORTER");
            if ($("[id$=hdfSOTypeCheckEnabled]").val() == 0) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=0&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=hdfSaleOrderType]").val() + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
            }
            $("[id*=txtDespNow]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
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
        function ShowHideRelatedDetails(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tblRelatedDetails]").show();
                $("[id$=imbShowDetails]").hide();
                $("[id$=imbHideDetails]").show();
            }
            else {
                $("[id$=tblRelatedDetails]").hide();
                $("[id$=imbShowDetails]").show();
                $("[id$=imbHideDetails]").hide();
            }
            return false;
        }
        function ShowHideAdditionalDetails(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tblAddDetails]").show();
                $("[id$=imbAddShowDetails]").hide();
                $("[id$=imbAddHideDetails]").show();
            }
            else {
                $("[id$=tblAddDetails]").hide();
                $("[id$=imbAddShowDetails]").show();
                $("[id$=imbAddHideDetails]").hide();
            }
            return false;
        }
        function CalculateTotalFooter(sender) {
            var val1 = parseFloat($(sender).val());
            var Amount = 0;
            var BalancetoPay = 0;
            var DecimalDigits = 0;
            var ConvRate = 1;
            var AmountPcs = 0;
            var TotalAmountPcs = 0;
            var DespNow = 0;
            var TotDespNow = 0;
            var DespNowPcs = 0;
            var TotDespNowPcs = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            $("#[id*=grdDeliveryList] input[type=text][id*=txtDespNowSales]").each(function (index) {
                //Check if number is not empty
                if ($.trim($(this).val()) != "") {
                    //   Check if number is a valid integer
                    if (!isNaN(parseFloat($(this).val()))) {
                        debugger;
                        ConvRate = $(this).closest('tr').find("#[id$=hdfSalesUonConvRate]").val();
                        $(this).parent("td").find('input[type=hidden][id$=hdfPayNowSplit]').val($(this).val());
                        Amount = parseFloat(Amount) + parseFloat($(this).val());
                        AmountPcs = parseFloat(ConvRate) * parseFloat($(this).val());
                        $(this).closest('tr').find("#[id$=txtDespNow]").val(AmountPcs);
                        TotalAmountPcs = parseFloat(TotalAmountPcs) + AmountPcs;
                        if ($("[id$=hdfIsShowDONetGrsWt]").val() == "1" && $("[id$=hdfIsView]").val() == "0") {
                         // To enable the weight editing, disable the below code. 
                          //  SetNetGrossWT($(this).closest('tr').find("#[id$=hdfSaleOrderHdrPK]").val(), AmountPcs, $(this).closest('tr').find("#[id$=hdfSaleOrderDtlPK]").val(), $(this).closest('tr').find("#[id$=txtNetWeight]"), $(this).closest('tr').find("#[id$=txtGrossWeight]"));
                        }
                    }
                }
            });
            $("#[id*=grdDeliveryList] [id*=hdfTotalPayNowFooterSplit]").val(TotalAmountPcs);
            $("#[id*=grdDeliveryList] [id*=hdfTotalPayNowFooterSplitSales]").val(Amount);         
            $("[id$=hdfIsView]").val("0");

            $("[id$=grdDeliveryList] tr").each(function () {
             
                if ($(this).find("[id*=txtDespNowSales]").length > 0) {
                    DespNow = $(this).find("[id*=txtDespNowSales]").val();
                    DespNowPcs = $(this).find("#[id$=txtDespNow]").val();

                    TotDespNow = TotDespNow + parseFloat(DespNow);
                    TotDespNowPcs = TotDespNowPcs + parseFloat(DespNowPcs);
                }
               
            });
            $("#[id*=grdDeliveryList] [id*=lblTotalPayNowFooterSplitSales]").html(addCommas(TotDespNow.toFixed()));
            $("#[id*=grdDeliveryList] [id*=lblTotDespNowFooter]").html(TotDespNowPcs.toFixed());

            CalculateTotFooter();
        }

        function SetNetGrossWT(soPK, despatchNow, sodPK, ctrlNet, ctrlGross) {
            var DecimalDigits = 0;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            var pageURL = window.document.URL;
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/DataHandler.ashx" : "/" + virtualPath + "Handlers/DataHandler.ashx");
            $.getJSON(url + "?SearchType=SHPWEIGHTDTL&qty=" + despatchNow + "&sodPK=" + sodPK + "&soPK=" + soPK, function (data) {
                if (data) {
                    $(ctrlNet).val(parseFloat(data[0].SND_QTY_NET_WT).toFixed(2));
                    $(ctrlGross).val(parseFloat(data[0].SND_QTY_GROSS_WT).toFixed(2));
                }
            });
        }

        function CalculateTotFooter() {
            var NetAmount = 0.0;
            var TotNetAmount = 0;
            var GrossAmount = 0;
            var TotGrossAmount = 0;
            var DespNowAmount = '';
            var TotDespNowAmount = 0;
            var ReleasedQty = 0;
             var TotReleasedQty = 0;
            $("[id$=grdDeliveryList] tr").each(function () {
                if ($(this).find("[id*=txtNetWeight]").length > 0 || $(this).find("[id*=txtGrossWeight]").length > 0) {
                    var NetAmount = $(this).find("[id*=txtNetWeight]").val();
                    NetAmount = NetAmount.replace(/,/g, "");

                    ReleasedQty = $(this).find("[id*=txtReleasedQty]").val();
                    ReleasedQty = ReleasedQty.replace(/,/g, "");

                    GrossAmount = $(this).find("[id*=txtGrossWeight]").val();
                    GrossAmount = GrossAmount.replace(/,/g, "");
                    DespNowAmount = $(this).find("#[id$=txtDespNow]").val();
                    // DespNowAmount = DespNowAmount.replace(/,/g, "");
                   // alert($(this).find("[id*=txtDespNow]").val());
                    TotNetAmount = TotNetAmount + parseFloat(NetAmount);
                    TotGrossAmount = TotGrossAmount + parseFloat(GrossAmount);
                    TotDespNowAmount = TotDespNowAmount + parseFloat(DespNowAmount);
                    TotReleasedQty = TotReleasedQty + parseFloat(ReleasedQty);
                }
            });

            $("#[id*=grdDeliveryList] [id*=lblTotalNetWeightFooter]").html(addCommas(TotNetAmount.toFixed(CurrencyDigits)));
            $("#[id*=grdDeliveryList] [id*=lblTotalGrossWeightFooter]").html(addCommas(TotGrossAmount.toFixed(CurrencyDigits)));
             $("#[id*=grdDeliveryList] [id*=lblTotalReleasedQtyFooter]").html((TotReleasedQty));
           // $("#[id*=grdDeliveryList] [id$=lblTotDespNowFooter]").html(addCommas(TotDespNowAmount.toFixed())); 

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
            }
            FormattedNumber = NumericPart + LastNumericPart + DecimalPart;
            return FormattedNumber;
        }

        /*
        function ShowDetails(refPK,title,IsApproveVisible,ProcesssDpt,RedirectUrl) {
        var pageURL = window.document.URL; 
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/DataHandler.ashx" : "/" + virtualPath + "Handlers/DataHandler.ashx");

        $("[id$=hdfTitle]").val(title);
        $("[id$=hdfPopupRefPK]").val(refPK);
        $("[id$=hdfProcessDeptPopup]").val(ProcesssDpt);
        $("[id$=hdfRedirectUrlPopup]").val(RedirectUrl);
        $.getJSON(url+"?SearchType=SUMMARY&SearchBy=" + refPK, function (data) {
        if(data.Fields)
        {
        $('#DivDetailsDialog').html(""); 
        var alt=2;
        for (var i in data.Fields) {
        if((alt%2)==1)
        {
        if(data.Fields[i].Value==null){
        $('#DivDetailsDialog').append("<div class='summary-row1'><span>"+data.Fields[i].Label+"</span><label>" + " "+"</label></div>");
        }
        else{
        $('#DivDetailsDialog').append("<div class='summary-row1'><span>"+data.Fields[i].Label+"</span><label>" + data.Fields[i].Value+"</label></div>");
        }
        }
        else
        {
        if(data.Fields[i].Value==null){
        $('#DivDetailsDialog').append("<div class='summary-row2'><span>"+data.Fields[i].Label+"</span><label>" + " "+"</label></div>");
        }
        else{
        $('#DivDetailsDialog').append("<div class='summary-row2'><span>"+data.Fields[i].Label+"</span><label>" + data.Fields[i].Value+"</label></div>");
        }
        }
        alt=alt+1;
        }
        ShowContainerDiv('[id$=divInfoPopup]', title, '500', '280');
        $(".ui-draggable").addClass("lightborder"); 
        }
        else
        {
        $('#DivDetailsDialog').html(""); 
        $('#DivDetailsDialog').append("<%=GetGlobalResourceObject("Messages", "NoSummaryAvailable") %>"); 
        }
        });
        ShowContainerDiv('[id$=divInfoPopup]', title, '500', '280');
        if(IsApproveVisible=="1")
        $("[id$=divApprove]").show();
        else
        $("[id$=divApprove]").hide();
        return false;
        }
        */
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=ModifiedDatePnl]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
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
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
            }
            else if (mode == 2) {
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }
        function CheckOldQty(sender, args) {
        }
        function ResetInvoice() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Msg_InvCleare").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnSave]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function ValidateETD(sender, args) {            
            if ($("[id$=hdfShippingTermValue]").val() == "2") { // FOB - Date of ETD
                if (args.Value) {//if ($("[id$=txtETD]").val()) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = true;
        }
        function ValidateETA(sender, args) {            
            if ($("[id$=hdfShippingTermValue]").val() == "4") { // 4 - DAP - Date of ETA
                if (args.Value) {//if ($("[id$=txtETA]").val()) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }
            else
                args.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium margnbotm0" runat="server" TabIndex="1"
                                        onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="40" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="41"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="42" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="43" Text="Print"
                                            OnClick="ActionHandler" ToolTip="Print" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="44" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="Li1" runat="server">
                                        <asp:Button runat="server" ID="btnInspection" CommandName="INSPECTION" TabIndex="45"
                                            Text="<%$resources:Inspection %>" OnClick="ActionHandler" ToolTip="<%$resources:Inspection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="46" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="47" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container padgrgt0" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnShippingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkShippingPlan" Text="<%$resources:PageNameRes,ShippingPlan %>"
                                TabIndex="40" CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="41" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="42" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="43" CommandName="UPLOADQA" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="44" CommandName="UPLOADEXPORT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="45" CommandName="LOADINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="46" CommandName="UPLOADPHOTOGRAPHS" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="47" CommandName="GOODOUTWARD" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-active" OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="48" CommandName="CONTAINERRELEASE" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="49" CommandName="PRINT" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul style="display: none">
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                TabIndex="50" OnClick="ActionHandler" CommandName="DELIVERYLIST" CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="51" OnClick="ActionHandler" CommandName="DELIVERYDETAIL" CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
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
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="large" MaxLength="100" TabIndex="52"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="6" CssClass="date-picker"
                                                MaxLength="54" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$ resources:TransportCo %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" CssClass="medium" MaxLength="100" TabIndex="56"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$ resources:GoodsOutwardNo %>"
                                                AssociatedControlID="txtDespatchNumber"></asp:Label>
                                            <asp:TextBox ID="txtDespatchNumber" runat="server" CssClass="medium" MaxLength="70"
                                                TabIndex="53"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDPHPK" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="55" CssClass="date-picker" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="57" CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="58"
                                                OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdDeliveryOrderList" Width="100%" PageSize="25"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" TabIndex="11">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="59" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfDespatchID" Value='<%# Eval("DPH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GONo %>" SortExpression="DPH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("DPH_NO") ==""?"[NEW]":Eval("DPH_NO")%>'
                                                    ToolTip='<%# Eval("DPH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GODate %>" SortExpression="DPH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="DPH_CUSTOMER_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("CRM_CUSTOMER_MST"+"."+"CUS_NAME")%>'
                                                    ToolTip='<%# Eval("CRM_CUSTOMER_MST"+"."+"CUS_NAME")%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.DOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DestPort %>" SortExpression="DPH_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPH_TO_PORT"),300)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPH_TO_PORT"),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Carrier %>" SortExpression="CON_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCarrier" runat="server" Text='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME")%>'
                                                    ToolTip='<%# Eval("ADM_CONST_MST"+"."+"CON_NAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ContainerNo %>" SortExpression="DPH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPH_CONTAINER_NO"),15)%>'
                                                    ToolTip='<%# Eval("DPH_CONTAINER_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransportCo %>" SortExpression="VEN_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransportCo" runat="server" Text='<%# Eval("PUR_VENDOR_MST"+"."+"VEN_NAME")%>'
                                                    ToolTip='<%# Eval("PUR_VENDOR_MST"+"."+"VEN_NAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("DPH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbnCustomerHdr" Text="<%$resources:Controls,CustomerHdr%>"
                                        AssociatedControlID="lblCustomerHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblCustomerHdr" CssClass="disp-table"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnDestinationPortHdr" runat="server" Text="<%$resources:Controls,DestinationPortHdr%>"
                                        AssociatedControlID="lblDestinationPortHdr"></asp:Label>
                                    <asp:Label runat="server" ID="lblDestinationPortHdr" CssClass="disp-table"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnInTimeHdr" runat="server" AssociatedControlID="lblInTimeHdr" Text="<%$resources:Controls,InTimeHdr%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblInTimeHdr"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbnContainerTypeValueHdr" Text="<%$resources:ContainerTypeHr%>"
                                        AssociatedControlID="lblContainerTypeValueHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblContainerTypeValueHdr"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="Label18" Text="<%$resources:Controls,ShippingPlanNo%>"
                                        AssociatedControlID="lblShippingPlanNoHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblShippingPlanNoHdr"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="Label19" Text="<%$resources:Controls,ShippingPlanDate%>"
                                        AssociatedControlID="lblShippingPlanDateHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblShippingPlanDateHdr"></asp:Label></div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblOrderNo" Text="<%$ resources:DeliveryOrderNo %>"
                                                        AssociatedControlID="lblDeliveryOrderNo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDeliveryOrderNo" Text="" TabIndex="2"></asp:Label>
                                                    <asp:Label runat="server" ID="Label11" Text="<%$ resources:DateofShipment %>" AssociatedControlID="txtDateOfShipment"
                                                        CssClass="middle-lbl-d-04-01">
                                                    </asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDateOfShipment" Text="" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" MaxLength="15" onpaste="return false;" TabIndex="3"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:CYDate %>" AssociatedControlID="txtCYDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtCYDate" Text="" CssClass="Uidate-picker input-small margnrgt1per"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="4"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfCYDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtCYDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Enter CY Date">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:RTNDate %>" AssociatedControlID="txtRtnDate"
                                                        CssClass="middle-lbl-small-c"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRtnDate" Text="" CssClass="Uidate-picker input-small"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="5"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblDelivery" Text="<%$ resources:DeliveryTo %>" AssociatedControlID="lblDeliveryTo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblDeliveryTo" Text="" TabIndex="6" onkeydown="return EnableArrowKey(event)"
                                                        onpaste="return false;" CssClass="input-disabled select-half"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:ETD %>" AssociatedControlID="txtETD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETD" Text="" CssClass="Uidate-picker input-small margnrgt1per"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="7"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETD" runat="server" Value="" />
                                                    <asp:CustomValidator ID="cvETD" runat="server" ErrorMessage="<%$ resources:Err_ETDDate%>" 
                                                        ValidateEmptyText="true" ClientValidationFunction="ValidateETD"
                                                        Text="*" EnableClientScript="true" ControlToValidate="txtETD" CssClass="star"
                                                        Display="Dynamic" ValidationGroup="delivery"></asp:CustomValidator>

                                                    <asp:Label runat="server" ID="Label4" Text="<%$ resources:ETA %>" AssociatedControlID="txtETA"
                                                        CssClass="middle-lbl-small-c"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETA" Text="" CssClass="Uidate-picker input-small "
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="8"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETA" runat="server" Value="" />
                                                    <asp:CustomValidator ID="cvETA" runat="server" ErrorMessage="<%$ resources:Err_ETADate%>" 
                                                        ValidateEmptyText="true" ClientValidationFunction="ValidateETA"
                                                        Text="*" EnableClientScript="true" ControlToValidate="txtETA" CssClass="star"
                                                        Display="Dynamic" ValidationGroup="delivery"></asp:CustomValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblCustomerName" Text="" AssociatedControlID="txtDeliveryAddress"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDeliveryAddress" TextMode="MultiLine" CssClass="multiline-2line select-half"
                                                        onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" oncontextmenu="return false;"
                                                        TabIndex="9"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblDestination" Text="<%$ resources:DestinationPort %>"
                                                        AssociatedControlID="lblDestinationPort"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblDestinationPort" Text="" TabIndex="9" MaxLength="200"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPrintShipAddress" runat="server" AssociatedControlID="ChkPrintShipAddress"
                                                        Text="<%$ resources:PrintShipAddress %>"></asp:Label>
                                                    <asp:CheckBox ID="ChkPrintShipAddress" runat="server" TabIndex="10" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblFinal" Text="<%$ resources:FinalDestination %>"
                                                        AssociatedControlID="lblFinalDestination"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblFinalDestination" TabIndex="11" MaxLength="200"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap scroll-container scroll-container-19-11-2">
                                <asp:GridView ID="grdDeliveryList" runat="server" AutoGenerateColumns="False" Width="1200px"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="13">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='' OnClick="ActionHandler"
                                                    CommandName="SHOWPOPUP" ToolTip=''></asp:LinkButton>
                                                <asp:HiddenField ID="hdfSONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfDpdPk" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfIsPackingMaterial" Value='0' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantCode" Text='' ToolTip='' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IGPLCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfBrandCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="280px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOMSales" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" />

                                             <FooterStyle HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblFooTotal" Text="Total"></asp:Label>
                                            </FooterTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="75px" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalOrderQtyFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="75px" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalDespatchedQtyFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespNow %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDespNowSales" runat="server" onkeyup="CalculateTotalFooter(this);" onblur="CalculateTotFooter();"
                                                    CssClass="small-a numeric margn-rgt-0" MaxLength="10" TabIndex="13"></asp:TextBox>
                                                <asp:HiddenField ID="hdfOldDespNowSales" runat="server" />
                                                <asp:HiddenField ID="hdfUOM" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfUOMSales" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSalesUonConvRate" runat="server" />
                                                <div class="starwrap">
                                                    <asp:CustomValidator ID="csvDespNowUm" runat="server" Text="*" ControlToValidate="txtDespNowSales"
                                                        ClientValidationFunction="CheckOldQty" ErrorMessage="<%$ resources:Msg_Err_Qty %>"
                                                        ValidationGroup="delivery"></asp:CustomValidator></div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Width="70px" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooterSplitSales"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplitSales" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespNowPcs %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDespNow" runat="server" onkeyup="CalculateTotalFooter(this);"
                                                    CssClass="input-disabled small-a numeric" MaxLength="17" TabIndex="13" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="hdfOldDespNow" runat="server" />
                                               
                                                <div class="starwrap">
                                                    <asp:CustomValidator ID="csvDespNow" runat="server" Text="*" ControlToValidate="txtDespNow"
                                                        ClientValidationFunction="CheckOldQty" ErrorMessage="<%$ resources:Msg_Err_Qty %>"
                                                        ValidationGroup="delivery"></asp:CustomValidator></div>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="110px" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotDespNowFooter"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplit" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:NetWeight %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNetWeight" runat="server" onkeyup="CalculateTotFooter(this);"
                                                    CssClass="small-a numeric margn-rgt-0" MaxLength="17" TabIndex="14" Enabled="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="60px" />
                                            <FooterStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <ItemStyle Width="8px" HorizontalAlign="Right" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalNetWeightFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrossWeight %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGrossWeight" runat="server" onkeyup="CalculateTotFooter(this);" CssClass="small-a numeric margn-rgt-0"
                                                    MaxLength="18" TabIndex="14" Enabled="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="60px" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalGrossWeightFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Released Qty" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReleasedQty" runat="server"
                                                    CssClass="input-disabled small-a numeric" MaxLength="17" TabIndex="15" Enabled="false"></asp:TextBox>                                                                                        
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" Width="110px" />   
                                                <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalReleasedQtyFooter"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="<%$ resources:LotNo %>">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLotNo" runat="server" CssClass="input-full" MaxLength="100"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Shippment_Details").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label10" Text="<%$ resources:FeederVessel %>" AssociatedControlID="txtFeederVessel"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtFeederVessel" Text="" MaxLength="200" TabIndex="14"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label12" Text="<%$ resources:MotherVessel %>" AssociatedControlID="txtMotherVessel"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtMotherVessel" MaxLength="200" Text="" TabIndex="15"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label8" Text="<%$ resources:LorryNo %>" AssociatedControlID="txtLorryNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtLorryNo" Text="" MaxLength="150" TabIndex="16"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label9" Text="<%$ resources:Driver %>" AssociatedControlID="txtDriver"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDriver" MaxLength="150" Text="" TabIndex="17"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblEnclosure" Text="<%$ resources:Enclosure %>" AssociatedControlID="txtEnclosure"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtEnclosure" Text="" TabIndex="18" TextMode="MultiLine"
                                                        CssClass="multiline-2line select-half" onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"
                                                        oncontextmenu="return false;"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:ShippingMark %>" AssociatedControlID="txtShippingMark"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShippingMark" TextMode="MultiLine" CssClass="multiline-2line select-half"
                                                        onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" oncontextmenu="return false;"
                                                        TabIndex="18"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Related_Details").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideRelatedDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideRelatedDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" /></div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblRelatedDetails">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPortLoading" Text="<%$ resources:PortofLoading %>"
                                                        AssociatedControlID="lblPortOfLoading"></asp:Label>
                                                    <asp:HiddenField ID="hdfPortOfLoading" runat="server" />
                                                    <asp:Label runat="server" ID="lblPortOfLoading" Visible="false" Text="" TabIndex="19"></asp:Label>
                                                    <%--<asp:DropDownList ID="ddlFromPort" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                        TabIndex="20" CssClass="select-half-a">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox runat="server" ID="txtFromPort" TabIndex="20" CssClass="select-half"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfFromPortID" />
                                                    <asp:HiddenField runat="server" ID="hdfSaleOrderType" />
                                                    <asp:HiddenField runat="server" ID="hdfSOTypeCheckEnabled" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblModeofTransport" Text="<%$ resources:ModeofTransport %>"
                                                        AssociatedControlID="ddlShipBy"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfModeofTransport" />
                                                    <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="20" CssClass="select-half-a">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPortofDischarge" Text="<%$ resources:PortofDischarge %>"
                                                        AssociatedControlID="txtPortofDischarge"></asp:Label>
                                                    <asp:TextBox ID="txtPortofDischarge" runat="server" MaxLength="200" TabIndex="21"
                                                        CssClass="select-half "> </asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label14" Text="<%$ resources:BookingDate %>" AssociatedControlID="txtBookingDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBookingDate" Text="" CssClass="Uidate-picker input-small"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="22"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:Company %>" AssociatedControlID="txtCompany"></asp:Label>
                                                    <asp:TextBox ID="txtCompany" runat="server" MaxLength="100" TabIndex="23" CssClass="select-half"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfCompany" runat="server" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label13" Text="<%$ resources:BookingNo %>" AssociatedControlID="txtBookingNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBookingNo" Text="" MaxLength="200" TabIndex="24"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label5" Text="<%$ resources:Carrier %>" AssociatedControlID="ddlCarrier"></asp:Label>
                                                    <asp:DropDownList ID="ddlCarrier" runat="server" TabIndex="25" CssClass="select-half-a">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label15" Text="<%$ resources:Container %>" AssociatedControlID="txtContainer"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtContainer" Text="" MaxLength="200" TabIndex="26"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                        AssociatedControlID="txtContainerType"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtContainerType" MaxLength="70" Text="" TabIndex="27"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled select-half"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfContainerType" runat="server" />
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblSupplimentary" Text="<%$ resources:Supplimentary %>"
                                                        AssociatedControlID="txtSupplimentary"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSupplimentary" Text="" TabIndex="28" MaxLength="500"
                                                        CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label7" Text="<%$ resources:SealNo %>" AssociatedControlID="txtSealNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSealNo" Text="" MaxLength="200" TabIndex="29"
                                                        CssClass="select-half"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPaymentTerms" Text="<%$ resources:PaymentTerms %>"
                                                        AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                    <%--<asp:TextBox runat="server" ID="txtPaymentTerms" Text="" TabIndex="30" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled select-half"
                                                        MaxLength="500"></asp:TextBox>--%>
                                                    <asp:TextBox runat="server" ID="txtPaymentTerms" Text="" TabIndex="30" Enabled="true"
                                                        CssClass="select-half" MaxLength="500"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfPaymentTerms" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblConsignee" Text="<%$ resources:Consignee %>" AssociatedControlID="txtConsignee"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtConsignee" Text="" TabIndex="31" Enabled="true"
                                                        CssClass="input-disabled select-half" MaxLength="200"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfConsignee" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblNotifyParty" Text="<%$ resources:NotifyParty %>"
                                                        AssociatedControlID="txtNotifyParty"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtNotifyParty" Text="" TabIndex="32" Enabled="true"
                                                        CssClass="input-disabled select-half" MaxLength="200"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfNotifyParty" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label16" Text="" AssociatedControlID="txtConsignee"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtConsigneeDetails" Text="" TabIndex="34" Enabled="true"
                                                        TextMode="MultiLine" MaxLength="500" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                        oncontextmenu="return false;" CssClass="select-half "></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label17" Text="" AssociatedControlID="txtNotifyParty"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtNotifyPartyDetails" Text="" TabIndex="35" Enabled="true"
                                                        TextMode="MultiLine" MaxLength="500" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                        oncontextmenu="return false;" CssClass="select-half"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblRem" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="36" TextMode="MultiLine"
                                                        CssClass="multiline-2line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                        oncontextmenu="return false;"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                        ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="delivery"></asp:RegularExpressionValidator>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
                            <%-- group end here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Additional_Details").ToString()%></h1>
                                <div class="button-wrap-right ">
                                    <asp:ImageButton runat="server" ID="imbAddShowDetails" OnClientClick="javascript:return ShowHideAdditionalDetails(1);"
                                        ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                        TabIndex="65" />
                                    <asp:ImageButton runat="server" ID="imbAddHideDetails" OnClientClick="javascript:return ShowHideAdditionalDetails();"
                                        ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                        TabIndex="66" /></div>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide" id="tblAddDetails">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblHISCode" Text="<%$ resources:HSCode %>" AssociatedControlID="ddlHIS"></asp:Label>
                                                    <asp:DropDownList ID="ddlHIS" runat="server" TabIndex="37" CssClass="select-half-a">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblShippedBoard" Text="<%$ resources:ShippedOnBoard %>"
                                                        AssociatedControlID="txtShippedBoard"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShippedBoard" MaxLength="500" TabIndex="38" TextMode="MultiLine"
                                                        CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                        oncontextmenu="return false;"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:Label runat="server" ID="lblTranshipmentto" Text="<%$ resources:Transhipmentto %>"
                                                        AssociatedControlID="txtTranshipmentto"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtTranshipmentto" MaxLength="500" TabIndex="39"
                                                        TextMode="MultiLine" CssClass="multiline-1col" onkeydown="limitText(this,500);"
                                                        onkeyup="limitText(this,500);" oncontextmenu="return false;"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revTranshipmentto" runat="server" ControlToValidate="txtTranshipmentto"
                                                        ErrorMessage="<%$ Resources:Err_Transhipment to %>" ValidationExpression="^[\s\S]{0,500}$"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="delivery"></asp:RegularExpressionValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label20" Text="<%$ resources:Buyer %>" AssociatedControlID="txtBuyer"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBuyer" MaxLength="500" TabIndex="39" TextMode="MultiLine"
                                                        CssClass="multiline-1col select-half" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                        oncontextmenu="return false;"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S" style="margin-top: 10px;">
                                                    <asp:Label runat="server" ID="Label21" Text="<%$ resources:SwapBuyer %>" AssociatedControlID="cbkSwapByer"></asp:Label>
                                                    <asp:CheckBox ID="cbkSwapByer" runat="server" TabIndex="39" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
                            <%--Print popup window --%>
                            <%--Print popup window --%>
                            <pc1:printercontrol id="PrinterControl1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
            </div>
            <asp:HiddenField ID="hdfDespatchNo" runat="server" />
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="delivery">
                </uc1:workflowusercomments>
            </div>
            <asp:HiddenField ID="hdfType" runat="server" Value="" />
            <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsShowDONetGrsWt" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsView" Value="1" />
            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfShippingTermValue" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
