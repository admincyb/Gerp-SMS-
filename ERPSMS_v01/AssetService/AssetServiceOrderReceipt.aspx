<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="AssetServiceOrderReceipt.aspx.cs" EnableEventValidation="false" Inherits="ERPSMS_v01.AssetService.AssetServiceOrderReceipt"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="Gti" Assembly="CustomControls" Namespace="CustomControls" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtListFromDate", "hdfListFromDate", "txtListToDate", "hdfListToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtListTrxNo", url + "?Type=RSH_NO", "hdfTrxPk", true, true, "GETASSETSERVICERECEIPTNOAUTO");
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendorName", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=3", "hdfVendor", true, true, ""); //VendorRole:3--->Service Provider        
            GrandScriptUtils.MakeAutoCompleteDDL("txtListVendorName", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=3", "hdfListVendor", true, true, ""); //VendorRole:3--->Service Provider        
            GrandScriptUtils.MakeAutoCompleteDDL("txtAssetType", url, "hdfAssetType", true, true, "GETASSETTYPEAUTO");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAsset", url + "?Type=" + $("[id$='hdfAssetType']").val(), "hdfAsset", true, true, "GETASSETSAUTO");
            GrandScriptUtils.MakeAutoCompleteDDL("txtPopupItems", url, "hdfPopupItemPk", true, true, "GETASSETITEMSAUTO", false, false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtPopupUOM", url, "hdfPopupUomPk", true, true, "UOM");
            GrandScriptUtils.DatePickerCommon("txtInvoiceDate");
            $("[id$=txtPopupAmount]").ForceNumericOnly();
            $("[id$=txtPopupQty]").ForceNumericOnly();
            $("[id$=txtPopupRate]").ForceNumericOnly();
            $("[id$=txtPopupDiscount]").ForceNumericOnly();
            $("[id$=txtPopupTax]").ForceNumericOnly();
            //Set a stamp for cancelled 
            if ($("[id$=hdfIsCancelled]").val() == "1") {
                $("[id$=tblDetailHdr]").addClass("table-devide invc-cancel");
                $("[id$='btnSave']").hide();
            }
            else {
                $("[id$=tblDetailHdr]").addClass("table-devide");
            }
            //End   
            HideRowUpDownIcon();
            ShowHideExpand();
            if ($("[id$=hdfSelRecordStatus]").val() == 0) {
                $("[id$=btnEditforCancel]").hide();
            } else {
                $("[id$=btnEditforCancel]").show();
            }
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                $("[id$=btnCurrency]").click();
            }
        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtAssetType") {
                $("[id$=txtAsset]").val("");
                $("[id$=hdfAsset]").val(0);
                GrandScriptUtils.MakeAutoCompleteDDL("txtAsset", url + "?Type=" + $("[id$='hdfAssetType']").val(), "hdfAsset", true, true, "GETASSETSAUTO");
            }
            if (targetControlID == "txtPopupItems") {
                $("[id$=btnItemSel]").click();
            }
            if (targetControlID == "txtVendorName") {
                $("[id$=btnVendorSel]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtListVendorName") {
                $("[id$=hdfListVendor]").val("0");
                $("[id$=txtListVendorName]").val("<%= Resources.Messages.AutoDefaultValue %>");
            }
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then hide AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            else {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
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
                        //Page_Validators.splice(i, 1);
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
        //Validation Summary
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
                return true;
            }
        }

        function onAfterDateChangeCallBack() {
            $("[id$=lblDateDay]").show();
            var date = GrandScriptUtils.ConvertDateFormat($("[id$=txtDate]").val());
            var d = new Date(date.split("-").reverse().join("-"));
            var weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $("[id$=lblDateDay]").html(weekdays[d.getDay()]);
        }

        function AfterClose(containerID) {
            //        if (containerID == "[id$=divJournalize]") {
            //           
            //        }
        }
        function validateRateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            rateDecimal = 4;
            if (!isNaN(parseInt($("[id$=hdfRateFormat]").val()))) {
                rateDecimal = parseInt($("[id$=hdfRateFormat]").val());
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > rateDecimal - 1)) {
                return false;
            }
            return true;
        }

        //Grid hierarchical******
        function SetGridScroll(rowId) {
            //        if (rowId)
            //            rowArray = $("[id$=" + rowId + "]");
            //        else
            //            rowArray = $("[id$=_ExpandPosition]");
            //        rowArray.each(function () {
            //            if ($.trim($(this).val()) != "") {
            //                var containerDiv = $(this).parent("[id$=_ScrollContainer]");
            //                if (containerDiv != null) {
            //                    $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
            //                }
            //            }
            //            $(this).val("")
            //        });
        }
        //Extra Grid
        function AfterGridExpand(row) {
            if ($("[id$=grdAssetDetails]").attr('id') == $(row).parent().parent().attr('id')) {
                {
                    $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
                    // IsAllExpand();
                }
            }
        }
        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>
            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }


        function SelectAllChild() {
            var isChecked = $("[id$=grdItemsGrid] tr:has(th)").find("[id$=chkAllActive]").attr("checked");
            $("[id$=grdActivityListGrid] tr:has(td)").each(function (index) {
                if (isChecked) {
                    $(this).find("[id$=chkSelect_" + index + "]").attr("checked", true);
                }
                else {
                    $(this).find("[id$=chkSelect_" + index + "]").attr("checked", false);
                }
            });
        }
        function ShowError() { //----------Error Maessages
            var msg = '<%=Resources.Messages.ReportError %>';
            var information = '<%=Resources.Messages.Information %>';
            GrandScriptUtils.ShowModal(msg, information);
        }
        //Sequence START
        function SendUp(CurrentRow, PreviousRow) {
            PreviousRow.parentNode.insertBefore(CurrentRow, PreviousRow);
        }
        function SequenceSwap(CurrentRow, PreviousRow) {
            var curId = CurrentRow.cells[1].children[0].id;
            var preId = PreviousRow.cells[1].children[0].id;
            var curSeq = $("[id$=" + curId + "]").val();
            var preSeq = $("[id$=" + preId + "]").val();
            curSeq = parseInt(curSeq);
            preSeq = parseInt(preSeq);
            curSeq = (isNaN(curSeq) || curSeq == "undefined") ? 1 : curSeq;
            preSeq = (isNaN(preSeq) || preSeq == "undefined") ? curSeq - 1 : preSeq;
            $("[id$=" + curId + "]").val(preSeq);
            $("[id$=" + preId + "]").val(curSeq);
            ShowRowUpDownIcon();
        }
        //function to select a row from Right Table to move up
        function MoveUp(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            for (i = 2; i < RightTable.rows.length; i++) {
                if (i == index) //is selected ?
                {
                    SendUp(RightTable.rows[i], RightTable.rows[i - 1]);
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i - 1]);
                }
            }
            HideRowUpDownIcon();
        }
        //function to select a row to move down
        function MoveDown(obj) {
            ShowRowUpDownIcon();
            var index = obj.parentElement.parentElement.rowIndex;
            var RightTable = obj.parentElement.parentElement.parentElement;
            var i = 0;
            var RowToMove = 0;
            var PreviousRow;
            var CurrentRow;
            for (i = 1; i < RightTable.rows.length - 1; i++) {
                if (i == index) {
                    RightTable.rows[i];
                    RowToMove = i;
                    SequenceSwap(RightTable.rows[i], RightTable.rows[i + 1]);
                    RightTable.rows[i].parentNode.appendChild(RightTable.rows[i]); //appends the selected row to the end of the Right Table
                    //this code moves the appended row up till it reaches
                    //to one position less than its original position
                    for (i = RightTable.rows.length - 1; i > RowToMove + 1; i--) {
                        CurrentRow = RightTable.rows[i];
                        PreviousRow = RightTable.rows[i - 1];
                        SendUp(CurrentRow, PreviousRow);
                    }
                }
            }
            HideRowUpDownIcon();
        }
        function ShowRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            $('[id*=grdItems] tr:nth-child(2)').find("[id*=imbRuleUp]").show(); //Show first rows Up arrow
            $('[id*=grdItems] tr:last-child').find("[id*=imbRuleDown]").show(); //Show last rows Down Arrow
        }
        function HideRowUpDownIcon() {
            //For Hide Move Up/Down arrow 
            $('[id*=grdItems] tr:nth-child(2)').find("[id*=imbRuleUp]").hide(); //Hide first rows Up arrow
            $('[id*=grdItems] tr:last-child').find("[id*=imbRuleDown]").hide(); //Hide last rows Down Arrow
        }
        //Sequence END
        function CalculateAmount() {
            //<summary>function used to calculate the sub total amount</summary>
            var tax = 0;
            var disc = 0;
            var qty = $("[id$=txtPopupQty]").val();
            var rate = $("[id$=txtPopupRate]").val();
            tax = parseFloat($("[id$=txtPopupTax]").val());
            disc = parseFloat($("[id$=txtPopupDiscount]").val());
            var amount = parseFloat(parseFloat(rate) * parseFloat(qty));
            amount = isNaN(amount) ? 0 : amount;
            $("[id$=txtPopupAmount]").val(amount.toFixed(CurrencyDigits));
            if (isNaN(tax))
            { tax = 0; }
            if (isNaN(disc)) {
                disc = 0;
            }
            if (!isNaN(tax) || !isNaN(disc)) {
                var amount = (amount + tax) - disc;
                $("[id$=txtPopupTotAmt]").val(amount.toFixed(CurrencyDigits));
            }
            else {
                $("[id$=txtPopupTotAmt]").val(parseFloat(0).toFixed(CurrencyDigits));
            }
        }
        function CalculateTotal(sender) {
            var subTotal = parseFloat($("[id$=txtHdrSubTotal]").val());
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var hdrDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
            hdrDiscount = isNaN(hdrDiscount) ? 0 : hdrDiscount;
            var hdrTax = parseFloat($("[id$=txtHdrTax]").val());
            hdrTax = isNaN(hdrTax) ? 0 : hdrTax;
            var hdrOtherCharge = parseFloat($("[id$=txtHdrOtherCharges]").val());
            hdrOtherCharge = isNaN(hdrOtherCharge) ? 0 : hdrOtherCharge;
            var hdrAdjamt = parseFloat($("[id$=txtHdrPriceAdj]").val());
            hdrAdjamt = isNaN(hdrAdjamt) ? 0 : hdrAdjamt;

            var netTotal = (subTotal - hdrDiscount) + hdrTax + hdrOtherCharge + hdrAdjamt;
            $("[id$=txtHdrGrandTotal]").val((netTotal).toFixed(CurrencyDigits));
            $("[id$=txtHdrGrandTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            if (hdrAdjamt == 0)
                $("[id$=txtHdrPriceAdj]").val((hdrAdjamt).toFixed(CurrencyDigits));
        }
        function CheckAllPR(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdPendingSOItems.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
        //Disabling Vendor,Service Type,Requesting Store
        function DisableVendorStoreServType() {
            if ($("[id$=hdfDisableVendorStoreSType]").val() == "1") {
                $("[id$=txtVendorName]").next("a").remove();
                $("[id$=txtVendorName]").attr("disabled", true);
                $("select[id$=ddlRequestingStore]").attr("disabled", true);
                $("select[id$=ddlServiceType]").attr("disabled", true);
            }
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="147"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="148"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="149" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="19" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="154" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="155" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSOR %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSOR %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="156" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="155" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrintList" CommandName="PRINT" TabIndex="19" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
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
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtListFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListFromDate" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small-b"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtListToDate"
                                                CssClass="middle-lbl-small"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListToDate" TabIndex="1" CssClass="input-small-b"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfListToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 ">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:ServiceType%>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlListServiceType"></asp:Label>
                                            <asp:DropDownList ID="ddlListServiceType" TabIndex="2" runat="server" CssClass="select-half-a">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblListTrxNo" runat="server" Text="<%$ resources:TrxNoList%>" CssClass="margnbotm0"
                                                AssociatedControlID="txtListTrxNo"></asp:Label>
                                            <asp:TextBox ID="txtListTrxNo" runat="server" CssClass="input-small-b margnbotm0"
                                                TabIndex="3"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTrxPk" runat="server" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-small margnbotm0"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-w21-8per margnbotm0"
                                                TabIndex="3">
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
                                            <asp:Label runat="server" ID="lblListVendorName" AssociatedControlID="txtListVendorName"
                                                CssClass="middle-lbl-small margnbotm0" Text="<%$ resources:Controls,Vendor%>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtListVendorName" CssClass="input-half margnbotm0"
                                                TabIndex="3">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfListVendor" runat="server" />
                                            <asp:ImageButton ID="imgbtnLstSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margnbotm0" />
                                            <asp:ImageButton ID="imgbtnLstClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" GroupName="SelectOne" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="10" />
                                                <asp:HiddenField runat="server" ID="hdfRSH_PK" Value='<%# Eval("RSH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("RSH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfDelStatus" runat="server" Value='<%# Eval("RSH_DEL_STATUS") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("RSH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ASRDate%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDate" runat="server" Text='<%#  Eval("RSH_DATE")!=""? Convert.ToDateTime( Eval("RSH_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'
                                                    ToolTip='<%# Eval("RSH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("RSH_NO")))?Resources.ErpRes.Draft:Eval("RSH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("RSH_NO")))?Resources.ErpRes.Draft:Eval("RSH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,RequestingStore%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("RSH_DEPT_STORE_TEXT")),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("RSH_DEPT_STORE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ServiceType%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSeviceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("RSH_SERVICE_TYPE_TEXT")),55) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("RSH_SERVICE_TYPE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%" />
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("RSH_VENDOR_TEXT"))),45) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("RSH_VENDOR_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                            <HeaderStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgStatus" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("RSH_CSS_CLASS") %>' ToolTip='<%# Eval("RSH_STATUS_TEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("RSH_STATUS") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="11" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:ASRDateReq%>" AssociatedControlID="txtDate"
                                                CssClass="lbl-21-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDate" TabIndex="4" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDate"
                                                CssClass="star" ValidationGroup="add" Text="*" ErrorMessage="<%$ resources:Err_EnterDate%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRequestingStore" runat="server" Text="<%$ resources:Controls,RequestingStoreReq%>"
                                                AssociatedControlID="ddlRequestingStore"></asp:Label>
                                            <asp:DropDownList ID="ddlRequestingStore" TabIndex="4" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqRequestingStore" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlRequestingStore"
                                                Display="Dynamic" Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectRequestingStore %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <%-- Invoice no & Invoice Date--%>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInvNO" Text="<%$ resources:InvoiceNo%>" AssociatedControlID="txtInvoiceNO"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceNO" CssClass="input-half" TabIndex="5"
                                                MaxLength="100"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInvDate" runat="server" Text="<%$ resources:InvoiceDate%>" AssociatedControlID="txtInvoiceDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceDate" TabIndex="5" CssClass="input-small Uidate-picker"
                                                onpaste="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <%-- End invoice no & invoice Date--%>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblVendorName" runat="server" Text="<%$ resources:Controls,VendorReq%>"
                                                AssociatedControlID="txtVendorName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendorName" CssClass="input-half" TabIndex="6">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvVendor" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="Save"
                                                runat="server" ControlToValidate="txtVendorName" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SelectVendor %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblServiceType" runat="server" Text="<%$ resources:ServiceTypeReq%>"
                                                AssociatedControlID="ddlServiceType"></asp:Label>
                                            <asp:DropDownList ID="ddlServiceType" TabIndex="6" runat="server" CssClass="select-half-a"
                                                OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvServiceType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="Save" runat="server" ControlToValidate="ddlServiceType" Display="Dynamic"
                                                Text="*" InitialValue="0" ErrorMessage="<%$ resources:Err_SelectServiceType %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblworkDesc" Text="<%$ resources:WorkDescription%>"
                                                AssociatedControlID="txtWorkDescription"></asp:Label>
                                            <asp:TextBox ID="txtWorkDescription" runat="server" TabIndex="7" MaxLength="500"
                                                TextMode="MultiLine" Height="30" CssClass="input-full" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <h3 class="fontWGT-Nrml">
                                <%= GetLocalResourceObject("PendingServiceOrderItems").ToString()%>
                            </h3>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdPendingSOItems" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler" TabIndex="8">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyPOList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllPOList" runat="server" onclick="CheckAllPR(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectSOList" runat="server" Checked='<%# Eval("CheckBoxChecked") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SO# %>">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfOSH_PK" Value='<%# Eval("OSD_OSH_HDR") %>' />
                                                <asp:HiddenField runat="server" ID="hdfOSD_PK" Value='<%# Eval("OSD_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfOSH_CURRENCY" Value='<%# Eval("OSH_CURRENCY") %>' />
                                                <asp:HiddenField runat="server" ID="hdfOSH_DEPT" Value='<%# Eval("OSH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOListSelected" Value='<%# Eval("AddedToStockList") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRSD_PK" Value='<%# Eval("RSD_PK") %>' />
                                                <asp:Label ID="lblOSD_OSH_NO" runat="server" Text='<%# Eval("OSD_OSH_NO") %>' ToolTip='<%# Eval("OSD_OSH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("OSH_VENDOR_TEXT").ToString()),56) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("OSH_VENDOR_TEXT").ToString())%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPk" Value='<%# Eval("OSH_VENDOR") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="24%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssetType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceTypeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("OSD_ASSET_TYPE_TEXT").ToString()),56) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("OSD_ASSET_TYPE_TEXT").ToString())%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfOSH_SERVICE_TYPE" Value='<%# Eval("OSH_SERVICE_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Asset %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("OSD_ASSET_TEXT").ToString()),56) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("OSD_ASSET_TEXT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrencyName" runat="server" Text='<%# Eval("OSH_CURRENCY_TEXT") %>'
                                                    ToolTip='<%# Eval("OSH_CURRENCY_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNetAmt" runat="server" Text='<%# Eval("OSD_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("OSD_AMOUNT", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="text-align: right;">
                                <%-- <div class="button-wrap-right">--%>
                                <asp:Button ID="btnAddToSOList" runat="server" TabIndex="8" Text="<%$ Resources:Controls, AddToList%>"
                                    CommandName="ADDTOLIST" OnClick="ActionHandler" />
                                <%-- </div>--%>
                            </div>
                            <div id="divSOPendingListing" runat="server" visible="false">
                                <%--   Hided :In Service Order no need to add New Assets--%>
                                <h3 class="fontWGT-Nrml" style="display: none">
                                    <%= GetLocalResourceObject("AssetDetails").ToString()%>
                                </h3>
                                <table class="table-devide" id="Table2" style="display: none">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblAssetType" runat="server" Text="<%$ resources:AssetType %>" AssociatedControlID="txtAssetType"></asp:Label>
                                                <asp:TextBox ID="txtAssetType" runat="server" TabIndex="8" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfAssetType" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvAssetType" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="add"
                                                    runat="server" ControlToValidate="txtAssetType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SelectAssetType %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblAsset" runat="server" Text="<%$ resources:Asset %>" AssociatedControlID="txtAsset"></asp:Label>
                                                <asp:TextBox ID="txtAsset" runat="server" TabIndex="8" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfAsset" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvAsset" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="add"
                                                    runat="server" ControlToValidate="txtAsset" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SelectAsset %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDescription" CssClass="margnrgt-minus1" Text="<%$ resources:Description%>"
                                                    AssociatedControlID="txtDescription"></asp:Label>
                                                <asp:TextBox ID="txtDescription" runat="server" TabIndex="9" MaxLength="200" CssClass="input-full"></asp:TextBox>
                                                <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                    ToolTip="Add" OnClientClick="javascript:ValidatePageNow('add')" ValidationGroup="add"
                                                    TabIndex="10" SkinID="plus" CssClass="margn-rgt4" />
                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear%>"
                                                    ToolTip="<%$ resources:Controls,Clear%>" TabIndex="10" OnClick="ActionHandler"
                                                    CommandName="CLEARADD" SkinID="clear-ext" CssClass="margn-rgt4" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <%--  End Hided: In Service Order no need to add New Assets--%>
                                <div class="gridwrap hierarchical-wrap">
                                    <asp:HiddenField ID="grdAssetDetails_ExpandPosition" runat="server" />
                                    <Gti:ExtGridView runat="server" ID="grdAssetDetails" AutoGenerateColumns="False"
                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                        Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnActivityDetails" OnClick="ActionHandler" CommandName="ACTIVITYLIST"
                                                        CommandArgument='<%# Eval("RSD_PK") %>' EnableTheming="false" Style="display: none" />
                                                    <asp:HiddenField runat="server" ID="hdfIsExpanded" Value="1" />
                                                    <asp:HiddenField runat="server" ID="hdfRSDPK" Value='<%# Eval("RSD_PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfRSDSlNo" Value='<%# Eval("RSD_SL_NO") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfRSD_OSD_DTL" Value='<%# Eval("RSD_OSD_DTL") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:AssetType %>">
                                                <ItemTemplate>
                                                    <div class="ellipsisText">
                                                        <asp:Label ID="lblAssetType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RSD_ASSET_TYPE_TEXT"),60) %>'
                                                            ToolTip='<%#HttpUtility.HtmlDecode(Convert.ToString(Eval("RSD_ASSET_TYPE_TEXT"))) %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfAssetTypePK" Value='<%# Eval("RSD_ASSET_TYPE") %>' />
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="col-md-9 col-sm-9 col-xs-9" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Asset %>">
                                                <ItemTemplate>
                                                    <div class="ellipsisText">
                                                        <asp:Label ID="lblAssetName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RSD_ASSET_TEXT"),60) %>'
                                                            ToolTip='<%#HttpUtility.HtmlDecode(Convert.ToString(Eval("RSD_ASSET_TEXT"))) %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfAssetPK" Value='<%# Eval("RSD_ASSET") %>' />
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="col-md-9 col-sm-9 col-xs-9" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%# GetFormattedCurrency(Eval("RSD_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedCurrency(Eval("RSD_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="col-md-2 col-sm-2 col-xs-2" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgShowpopup" runat="server" CommandName="SHOWPOPUP" SkinID="imbaddnew"
                                                        OnClick="ActionHandler" CommandArgument='<%# Eval("RSD_SL_NO") %>' TabIndex="12"
                                                        ToolTip="<%$ resources:Controls,Add  %>" />
                                                    <asp:ImageButton ID="imbAssetEdit" runat="server" CommandName="EDITASSET" SkinID="imbeditgrid"
                                                        Visible="false" OnClick="ActionHandler" CommandArgument='<%# Eval("RSD_SL_NO") %>'
                                                        TabIndex="12" ToolTip="<%$ resources:Controls,Edit  %>" />
                                                    <asp:ImageButton ID="imbAssetDelete" runat="server" CommandName="REMOVEASSET" SkinID="imbdeletegrid"
                                                        OnClick="ActionHandler" CommandArgument='<%# Eval("RSD_SL_NO") %>' TabIndex="12"
                                                        ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="col-md-1 col-sm-1 col-xs-1" HorizontalAlign="Right" Width="" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ControlStyle-Width="95%">
                                                <ItemTemplate>
                                                    <div class="hscroll">
                                                        <asp:GridView runat="server" ID="grdItems" Width="100%" PageSize="<%$ resources:PageSize%>"
                                                            OnRowDataBound="ActionHandler" AllowSorting="True" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblResourceMgrEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfActivityPK" runat="server" Value='<%# Eval("RID_PK") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                        <asp:HiddenField ID="hdfItm_SlNo" runat="server" Value='<%# Eval("RID_ITEM_SL_NO") %>' />
                                                                        <asp:HiddenField ID="hdfRID_OID_DTL" runat="server" Value='<%# Eval("RID_OID_DTL") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="1%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfSequence" runat="server" Value='<%# Eval("RID_SL_NO") %>' />
                                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleUp" SkinID="move-up" runat="server"
                                                                            OnClientClick="MoveUp(this);return false;" CommandName="MOVEUP" ToolTip="Move Up"
                                                                            CommandArgument='<%# Eval("RID_SL_NO") %>' TabIndex="1"></asp:ImageButton>
                                                                        <asp:ImageButton CssClass="nomargin" ID="imbRuleDown" SkinID="move-down" runat="server"
                                                                            OnClientClick="MoveDown(this);return false;" CommandName="MOVEDOWN" ToolTip="Move Down"
                                                                            CommandArgument='<%# Eval("RID_SL_NO") %>' TabIndex="1"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Items %>" SortExpression="">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RID_ASR_ITEM_TEXT"),35) %>'
                                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("RID_ASR_ITEM_TEXT"))) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="18%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Quantity %>" SortExpression="">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# GetFormattedNumber(Eval("RID_QTY")) %>'
                                                                            ToolTip='<%# GetFormattedNumber(Eval("RID_QTY")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("RID_UOM_TEXT") %>' ToolTip='<%# Eval("RID_UOM_TEXT") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRate" runat="server" Text='<%# GetFormattedRate(Eval("RID_RATE")) %>'
                                                                            ToolTip='<%# GetFormattedRate(Eval("RID_RATE")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="9%" CssClass="col-xs-1 col-sm-1 col-md-1" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmt" runat="server" Text='<%# GetFormattedCurrency(Eval("RID_AMOUNT")) %>'
                                                                            ToolTip='<%# GetFormattedCurrency(Eval("RID_AMOUNT")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Discount %>" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("RID_DISCOUNT", "{0:c}") %>'
                                                                            ToolTip='<%# Eval("RID_DISCOUNT", "{0:c}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="7%" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Tax %>" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTax" runat="server" Text='<%# Eval("RID_TAX", "{0:c}") %>' ToolTip='<%# Eval("RID_TAX", "{0:c}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNetAmt" runat="server" Text='<%# GetFormattedCurrency(Eval("RID_NET_AMOUNT")) %>'
                                                                            ToolTip='<%# GetFormattedCurrency(Eval("RID_NET_AMOUNT")) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="12%" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RID_REMARKS"),50) %>'
                                                                            ToolTip='<%# Eval("RID_REMARKS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                    <HeaderStyle />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imbItemEdit" runat="server" CommandName="EDITITEM" SkinID="imbeditgrid"
                                                                            CommandArgument='<%# Eval("RID_SL_NO") %>' OnClick="ActionHandler" ToolTip="<%$resources:Controls,Edit %>"
                                                                            TabIndex="13" />
                                                                        <asp:ImageButton ID="imbItemDelete" runat="server" CommandName="REMOVEITEM" SkinID="imbdeletegrid"
                                                                            OnClick="ActionHandler" CommandArgument='<%# Eval("RID_SL_NO") %>' ToolTip="<%$resources:Controls,Delete %>"
                                                                            OnClientClick="return ShowDeleteConfirm(this);" TabIndex="13" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" HorizontalAlign="Right" CssClass="actn-btn-container-2" />
                                                                    <HeaderStyle CssClass="actn-btn-container-2" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="nopadding" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                        <FooterStyle CssClass="table-firstlevela-total" />
                                    </Gti:ExtGridView>
                                </div>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="width: 85%; text-align: right">
                                                <asp:Label runat="server" ID="Label1" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtHdrSubTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:TextBox ID="txtHdrSubTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 85%; text-align: right">
                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                    TabIndex="14" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    onchange="CalculateTotal(this);" MaxLength="16" Enabled="false"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:OtherCharges%>" AssociatedControlID="txtHdrOtherCharges"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgOtherCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Shipping %>" CommandName="OTHERCHARGEHEADER" TabIndex="14" />
                                                <asp:TextBox ID="txtHdrOtherCharges" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    onchange="CalculateTotal(this);" Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                    <div class="clear">
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ValidationGroup="taxHdrDate" OnClientClick="javascript:ValidatePageNow('taxHdrDate')"
                                                    TabIndex="14" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    onchange="CalculateTotal(this);" Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                    <div class="clear">
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdjustment%>"
                                                    AssociatedControlID="txtHdrPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrPriceAdj" runat="server" CssClass="input-w80 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16" TabIndex="15"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtHdrPriceAdj"
                                                        ErrorMessage="Invalid Price Adjustment" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="invoice"></cc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:GrandTotal%>" AssociatedControlID="txtHdrGrandTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrGrandTotal" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divItemPopup" style="display: none">
                                <div class="Button-container-popup margnrgt5">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        OnClientClick="javascript:ValidatePageNow('AddPopupItem')" CommandName="ADDITEM"
                                        ValidationGroup="AddPopupItem" TabIndex="24" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S padgtop7">
                                                    <asp:HiddenField ID="hdfAssetPK" runat="server" />
                                                    <asp:Label ID="lblPopupItems" runat="server" Text="<%$ resources:Item%>" AssociatedControlID="txtPopupItems"></asp:Label>
                                                    <asp:TextBox ID="txtPopupItems" CssClass="input-half" runat="server" TabIndex="25"
                                                        EnableViewState="false" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                        InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="AddPopupItem"
                                                        runat="server" ControlToValidate="txtPopupItems" Display="Dynamic" Text="*" ErrorMessage="Select Item">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="AddPopupItem" runat="server" ControlToValidate="txtPopupItems"
                                                        Display="Dynamic" Text="*" ErrorMessage="Select Item">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:HiddenField ID="hdfPopupItemPk" runat="server" Value="0" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupAppxRate" runat="server" Text="<%$ resources:Rate%>" AssociatedControlID="txtPopupRate"></asp:Label>
                                                    <asp:TextBox ID="txtPopupRate" runat="server" TabIndex="26" CssClass="input-w80 numeric"
                                                        EnableViewState="false" onchange="javascript:CalculateAmount();" MaxLength="15"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="AddPopupItem"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtPopupRate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="vrfRateTaxDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtPopupRate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:RateValidationP2P ID="vreRateTaxDate" runat="server" ControlToValidate="txtPopupRate"
                                                            ErrorMessage="<%$ resources:Err_Rate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"
                                                            NonZero="true"></cc1:RateValidationP2P>
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Amount%>" CssClass="lbl-15-4perc"
                                                        AssociatedControlID="txtPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupAmount" runat="server" Enabled="false" TabIndex="26" CssClass="input-w80 numeric input-disabled"
                                                        EnableViewState="false" MaxLength="15"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="AddPopupItem" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lbltotamt" runat="server" AssociatedControlID="txtPopupTotAmt" Text="<%$ resources:Tot_Amt %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtPopupTotAmt" runat="server" CssClass="input-w80 input-disabled numeric medium input-small"
                                                        MaxLength="15"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S padgtop7">
                                                    <asp:Label ID="lblPopupQty" runat="server" Text="<%$ resources:Quantity%>" CssClass="lbl-15-4perc"
                                                        AssociatedControlID="txtPopupQty"></asp:Label>
                                                    <asp:TextBox ID="txtPopupQty" TabIndex="25" runat="server" CssClass="input-w80 numeric"
                                                        onchange="javascript:CalculateAmount();" EnableViewState="false" MaxLength="15"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="AddPopupItem" EnableClientScript="true" runat="server" ControlToValidate="txtPopupQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="vrfQuantityTaxDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="taxDate" EnableClientScript="true" runat="server" ControlToValidate="txtPopupQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidationP2P ID="vreQuantityTaxDate" runat="server" ControlToValidate="txtPopupQty"
                                                            NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="taxDate"
                                                            NonZero="true"></cc1:QuantityValidationP2P>
                                                    </div>
                                                    <asp:Label ID="Label8" runat="server" Text="<%$ resources:UOM%>" CssClass="lbl-20-1perc"
                                                        AssociatedControlID="txtPopupUOM"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPopupUOM" CssClass="input-w80" TabIndex="25"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfPopupUomPk" runat="server" Value="0" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            ValidationGroup="AddPopupItem" EnableClientScript="true" runat="server" ControlToValidate="txtPopupUOM"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_UOM %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="vrfUOMTaxDate" CssClass="star" SetFocusOnError="true"
                                                            InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="taxDate"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtPopupUOM" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_UOM %>">
                                                        </asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ resources:Discount%>" CssClass="lbl-15-4perc"
                                                        AssociatedControlID="txtPopupDiscount"></asp:Label>
                                                    <asp:TextBox ID="txtPopupDiscount" runat="server" TabIndex="27" CssClass="input-w80 numeric input-disabled"
                                                        EnableViewState="false" MaxLength="15"></asp:TextBox>
                                                    <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                        CssClass="margntop2" TabIndex="19" ToolTip="<%$ resources:Controls,Discounts %>"
                                                        CommandName="DISCDETAILS" ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:Tax%>" CssClass="lbl-13-3perc"
                                                        AssociatedControlID="txtPopupTax"></asp:Label>
                                                    <asp:TextBox ID="txtPopupTax" runat="server" TabIndex="27" CssClass="input-w80 numeric input-disabled"
                                                        EnableViewState="false" MaxLength="15"></asp:TextBox>
                                                    <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                        CssClass="margntop2" TabIndex="19" ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS"
                                                        ValidationGroup="taxDate" OnClientClick="javascript:ValidatePageNow('taxDate')" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div class="divcol-S">
                                                    <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:Remarks%>" AssociatedControlID="txtPopupRemarks"></asp:Label>
                                                    <asp:TextBox ID="txtPopupRemarks" runat="server" TabIndex="28" MaxLength="500" TextMode="MultiLine"
                                                        CssClass="input-w65per" onkeyup="limitText(this,500);"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--    ***************** Tax POPUP Start************************************--%>
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnTaxApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="TAXAPPLY"
                                        TabIndex="29" />
                                </div>
                                <div class="content-wrapper">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblTaxPopupItemAmount" runat="server" Text="Item Amount" AssociatedControlID="txtTaxPopupItemAmount"></asp:Label>
                                                    <asp:TextBox ID="txtTaxPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="Label7" runat="server" Text="Amount" AssociatedControlID="txtTaxPopupAmount"></asp:Label>
                                                    <asp:TextBox ID="txtTaxPopupAmount" TabIndex="25" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" MaxLength="15"></asp:TextBox><%--Enabled="false"--%>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtTaxPopupAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtTaxPopupAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblTaxPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlTaxPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlTaxPopupTaxType" TabIndex="24" runat="server" CssClass="medium"
                                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblTaxPopupOther" runat="server" Text="Name" AssociatedControlID="txtTaxPopupOther"></asp:Label>
                                                    <asp:TextBox ID="txtTaxPopupOther" runat="server" TabIndex="26" CssClass="medium"
                                                        EnableViewState="false" MaxLength="100" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtTaxPopupOther"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgTaxPopupAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        CommandArgument="PageAction_Entry" ValidationGroup="tax" CommandName="TAXADD"
                                                        TabIndex="27" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" TabIndex="22" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("TRD_PK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("TRD_TAX") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("TRD_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TRD_TAX_TEXT")) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TRD_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TRD_TAX_TEXT"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("TRD_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TRD_NAME"))) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("TRD_NAME") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("TRD_TAX_AMT")) %>'
                                                            ToolTip='<%#GetFormattedCurrency(Eval("TRD_TAX_AMT")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            TabIndex="28" CommandArgument="PageAction_Entry" SkinID="btnclose" ToolTip="Remove" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%--   ********************Tax Popup End ************************************************--%>
                            <asp:Button ID="btnItemSel" runat="server" OnClick="ActionHandler" CommandName="ITEMSELECTED"
                                Style="display: none" EnableTheming="false" />
                            <asp:Button ID="btnVendorSel" runat="server" OnClick="ActionHandler" CommandName="PENDINGORDERS"
                                Style="display: none" EnableTheming="false" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="add" runat="server" />
                <asp:ValidationSummary ID="vsAddPopupItem" ValidationGroup="AddPopupItem" runat="server" />
                <asp:ValidationSummary ID="vsTaxDate" ValidationGroup="taxDate" runat="server" />
                <asp:ValidationSummary ID="vsAddTax" ValidationGroup="tax" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="SaveAddDed">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfRateFormat" runat="server" />
            <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfAdvSearch" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
            <asp:HiddenField ID="hdfJournalizeWorkFlow" Value="0" runat="server" />
            <asp:HiddenField ID="hdfIsCancelled" Value="0" runat="server" />
            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyMode" runat="server" />
            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
            <asp:HiddenField runat="server" ID="hdfApplyTax" Value="0" />
            <asp:HiddenField runat="server" ID="hdfApplyHdrTax" Value="0" />
            <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
            <asp:HiddenField ID="OS_PK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
            <asp:HiddenField ID="hdfDisableVendorStoreSType" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSelRecordStatus" runat="server" />
        </ContentTemplate>
        <%--  <Triggers>            
            <asp:PostBackTrigger ControlID="btnDat" />           
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
