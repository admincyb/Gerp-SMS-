<%@ Page Title="<%$ Resources:Captions,Title_ShippingLoadingPlan %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="LoadingPlan.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.LoadingPlan" Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtGeneratedOn");
            $("[id*=txtLoaded]").ForceNumericOnly();
            $("[id*=txtLoaded2]").ForceNumericOnly();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
            var pageURL = window.document.URL;
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfSCNoDtls]").val() + "&ServiceType=" + $("[id$=hdfShippingPlanPK]").val(), "hdfLoadPlanSodPk", true, true, "BRANDBYSC");

            if ($("[id$=txtBrand]").attr("disabled") == true) {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            $("[id*=txtNetWtBox]").ForceNumericOnly();
            $("[id*=txtNetWtCarton]").ForceNumericOnly();
            $("[id*=txtGrossWtBox]").ForceNumericOnly();
            $("[id*=txtGrossWtCarton]").ForceNumericOnly();
            if ($("[id*=hdfLotNobyBrand]").val() == "1") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtLotNO", uiUrl + "?Type=" + $("[id$=hdfLoadPlanSodPk]").val(), "hdfLotNO", true, true, "SELECTLOTNOBYBRAND", false, false, false, true);
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtLotNO", uiUrl + "?Type=" + $("[id$=hdfShippingPlanPK]").val(), "hdfLotNO", true, true, "SELECTLOTNO", false, false, false, true);
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
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtBrand") {
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfBrand]").val("0");
                $("[id$=txtNetWtBox]").val("");
                $("[id$=txtNetWtCarton]").val("");
                $("[id$=txtGrossWtBox]").val("");
                $("[id$=txtGrossWtCarton]").val("");
            }
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtBrand") {
                $("[id$=btnSelectBrand]").click();
            }
            if (targetControlID == "txtLotNO") {
                $("[id$=btnSelectProduct]").click();
            }
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
                if (valGroup == "LoadingDet") {
                    var isValid = true;
                    var msg = "";
                    if ($("[id$=txtRowFrom]").val() != "") {
                        if (isNaN(parseInt($("[id$=txtRowFrom]").val()))) {
                            msg = '<ul><li><%= GetLocalResourceObject("Err_InvalidRowFrom") %></li></ul>';
                            isValid = false;
                        }
                    }
                    if ($("[id$=txtRowTo]").val() != "") {
                        if (isNaN(parseInt($("[id$=txtRowTo]").val()))) {
                            msg = msg + '<ul><li><%= GetLocalResourceObject("Err_InvalidRowTo") %></li></ul>';
                            isValid = false;
                        }
                    }
                    if (!isValid) {
                        $("[id$=litErrorMsg]").show();
                        $("[id$=litErrorMsg]").html(msg);
                        ShowErrorMessage($("#diverror").html());
                    }
                    return isValid;
                }
                else {
                    return true;
                }
            }
        }


        //        function CheckQty() {
        //            var ctnqty = 0;
        //            var shippingCtnQty = parseInt($("[id$=hdfQty]").val());
        //            $("#[id*=grdLoadingPlanDet] span[id*=lblTotQty]").each(function (index) {
        //                ctnqty = parseInt($(this).html());
        //            });
        //            if (shippingCtnQty != ctnqty) {
        //                return false;
        //            }
        //            else {
        //                return true;
        //            }
        //        }

        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }
        //For checking Row From and To
        function CheckRow(oSrc, args) {
            var value1 = $('input:text[id$=txtRowFrom]').val();
            var value2 = $('input:text[id$=txtRowTo]').val();
            if (value1 != "" && value2 != "") {
                if (parseFloat(value1) > parseFloat(value2)) {
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }
        //For calculating Qty
        function CalculateQty() {
            var DecimalDigits = 0;
            var Qty;
            if (!isNaN(parseFloat($("#[id*=hdfDecimalDigits]").val()))) {
                DecimalDigits = parseFloat($("#[id*=hdfDecimalDigits]").val());
            }
            var QtyX = parseFloat($("[id$=txtLoaded]").val());

            QtyX = isNaN(QtyX) ? 0 : QtyX;
            var QtyY = parseFloat($("[id$=txtLoaded2]").val());
            QtyY = isNaN(QtyY) ? 0 : QtyY;
            Qty = QtyX * QtyY;
            $("[id$=txtQty]").val(Qty);

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
                $("[id$=pnlSave]").hide();
                $("[id$=btnAddToList]").hide();
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }

        }
        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
        }
        //        function CheckQty(sender, args) {
        //            var planQty = $("[id$=hdfQty]").val().trim();
        //            var ctnQty = $("[id$=txtQty]").val().trim();
        //            planQty = parseFloat(planQty);
        //            ctnQty = parseFloat(ctnQty);
        //            if (planQty < ctnQty) {
        //                args.IsValid = false;
        //            }
        //            else {
        //                args.IsValid = true;
        //            }
        //        }

        //For Alert 
        function ShowAlertConfirm(flag) {
            var msgTitle;
            msg = '<%= GetLocalResourceObject("Err_LoadingQty") %>';
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    YES: function (e) {
                        $(this).dialog("close");
                        $("[id$='hdfAlert']").val("1");
                        //Save
                        if (flag == 1) {
                            $("[id$='btnSave']").click();
                        }
                        else if (flag == 2) {
                            $("[id$='btnSaveSubmit']").click();
                        }
                        else if (flag == 3) {
                            $("[id$='hdfAlert']").val("5");
                            $("[id$='btnSubmit']").click();

                        }
                        else if (flag == 4) {
                            $("[id$='divActionComments'] input[type=submit][id*=btnSubmit]").click();
                        }
                    },
                    NO: function (e) {
                        $(this).dialog("close");
                        $("[id$='hdfAlert']").val("0");
                        $('#divmodel').hide();
                        //$("[id$='btnCancelAlert']").click();
                    }
                }
            });
        }
        function ShowLineItemAlertConfirm(flag) {
            var msgTitle;
            msg = '<%= GetLocalResourceObject("Err_LoadingItemQty") %>';
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    YES: function (e) {
                        $(this).dialog("close");
                        //Add Line Item
                        if (flag == 1) {
                            $("[id$='btnAddToList']").click();
                        }
                    },
                    NO: function (e) {
                        $(this).dialog("close");
                        $("[id$='hdfShpPlanQtyValid']").val("0");
                        $('#divmodel').hide();
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        $(window).load(function EndRequest() {
            FormatCalendar('4');
        });
        var ControlID = 'calendar1|calendar2';
        function EndRequest() { FormatCalendar('4'); }
        function FormatCalendar(type) {
            var ctrlBehaviourarray = ControlID.split('|');
            for (var i = 0; i < ctrlBehaviourarray.length; i++) {
                var calenderCtrl = $find(ctrlBehaviourarray[i]);
                if (calenderCtrl) {
                    switch (type) {
                        case "6":
                            return;
                            break;
                        case "4":
                            $(calenderCtrl).attr('CalenderType', '2');
                            modifyMontDelegates(calenderCtrl);
                            break;
                        case "1":
                            $(calenderCtrl).attr('CalenderType', '3');
                            modifyYearDelegates(calenderCtrl);
                            break;
                    }
                }
            }
        }
        function modifyMontDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode.
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days");
                            } else {
                                cal._visibleDate = target.date;
                                //this._switchMode("days");
                            }
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;
                        // case "day":                                                                                            
                        // this.set_selectedDate(target.date);                                                                                            
                        // this._switchMonth(target.date);                                                                                            
                        // this._blur.post(true);                                                                                            
                        // this.raiseDateSelectionChanged();                                                                                            
                        // break;                                                                                            
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }
        function modifyYearDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        // case "month":                                                                                         
                        // //if the mode is month, then stop switching to day mode.                                                                                         
                        // if (target.month == visibleDate.getMonth()) {                                                                                         
                        // //this._switchMode("days");                                                                                         
                        // } else {                                                                                         
                        // cal._visibleDate = target.date;                                                                                         
                        // //this._switchMode("days");                                                                                         
                        // }                                                                                         
                        // cal.set_selectedDate(target.date);                                                                                         
                        // cal._switchMonth(target.date);                                                                                         
                        // cal._blur.post(true);                                                                                         
                        // cal.raiseDateSelectionChanged();                                                                                         
                        // break;                                                                                         
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                // cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                //cal._switchMode("months");
                            }
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        // case "day":                                                                                         
                        // this.set_selectedDate(target.date);                                                                                         
                        // this._switchMonth(target.date);                                                                                         
                        // this._blur.post(true);                                                                                         
                        // this.raiseDateSelectionChanged();                                                                                         
                        // break;                                                                                         
                        case "today":
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function changeMonthCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function changeYearCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }


        function onCalendarShown(cal, args) {
            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden(sender, args) {
            //                        if (sender.get_selectedDate()) {
            //                            if (sender.get_selectedDate() && sender.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            //                                alert('The "From" Date should smaller than the "To" Date, please reselect!');
            //                                sender.show();
            //                                return;
            //                            }
            //                            //get the final date
            //                            var finalDate = new Date(sender.get_selectedDate());
            //                            var selectedMonth = finalDate.getMonth();
            //                            finalDate.setDate(1);
            //                            if (sender == cal2) {
            //                                // set the calender2's default date as the last day
            //                                finalDate.setMonth(selectedMonth + 1);
            //                                finalDate = new Date(finalDate - 1);
            //                            }
            //                            //set the date to the TextBox
            //                            sender.get_element().value = finalDate.format(sender._format);
            //                        }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlLoadingPlan">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" CssClass="medium margnbotm0"  runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="20" Text="<%$resources:Submit %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Submit %>" CommandArgument="SEC_ActionPanel"
                                            OnClientClick="javascript:ValidatePageNow('LoadingPlan')" SkinID="btnInner-submit"
                                            ValidationGroup="LoadingPlan" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="21"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('LoadingPlan')"
                                            ValidationGroup="LoadingPlan" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="22" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('LoadingPlan')"
                                            ValidationGroup="LoadingPlan" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="23" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
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
                                TabIndex="13" CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="14" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="15" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="16" CommandName="UPLOADQA" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="17" CommandName="UPLOADEXPORT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="18" CommandName="LOADINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="19" CommandName="UPLOADPHOTOGRAPHS" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="20" CommandName="GOODOUTWARD" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="21" CommandName="CONTAINERRELEASE" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="22" CommandName="PRINT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="detail-co3">
                                <div class="div3col-S">
                                    <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="lblCustomerText"
                                        Text="<%$ resources:Customer %>" ></asp:Label>
                                    <asp:Label runat="server" ID="lblCustomerText" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblSCNo" runat="server" AssociatedControlID="lblSCNoText" Text="<%$resources:Controls,ShippingPlanNo%>" ></asp:Label>
                                    <asp:Label runat="server" ID="lblSCNoText" CssClass="disp-table"></asp:Label>
                                    <asp:HiddenField ID="hdfShippingPlanPK" runat="server" />
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblSCDate" runat="server" AssociatedControlID="lblSCDateText" Text="<%$resources:Controls,ShippingPlanDate%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblSCDateText"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo%>" 
                                        AssociatedControlID="lblContainerNoValue">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblContainerNoValue"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType%>" 
                                        AssociatedControlID="lblContainerTypeValue">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblContainerTypeValue"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblPlanQty" runat="server" AssociatedControlID="lblPlanQtytext" Text="<%$ resources:CtnQty %>"></asp:Label>
                                    <asp:Label ID="lblPlanQtytext" runat="server"></asp:Label></div>
                                <%--  <div class="div3col-S">
                                    <asp:Label ID="lblDestinationPort" runat="server" Text="<%$ resources:DestinationPort%>"
                                        AssociatedControlID="lblDestinationPortValue"></asp:Label>
                                    <asp:Label runat="server" ID="lblDestinationPortValue"></asp:Label></div>
                                <div class="div3col-S">
                                    <asp:Label ID="lblInTime" runat="server" AssociatedControlID="lblInTimeValue" Text="<%$ resources:InTime %>"></asp:Label>
                                    <asp:Label runat="server" ID="lblInTimeValue"></asp:Label>
                                </div>--%>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPlanNo" runat="server" AssociatedControlID="txtPlanNo" Text="<%$ resources:PlanNo %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtPlanNo" runat="server" Enabled="false" MaxLength="100" CssClass="input-disabled input-small"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfShippingPlan" runat="server" />
                                            <div class="clear">
                                            </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblGeneratedOn" Text="<%$ resources:GeneratedOn%>"
                                                AssociatedControlID="txtGeneratedOn"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtGeneratedOn" CssClass="input-small" TabIndex="2"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfGeneratedOn" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="LoadingDet" EnableClientScript="true" runat="server" ControlToValidate="txtGeneratedOn"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_GeneratedOn %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <%--<asp:Label ID="lblSealNo" runat="server" AssociatedControlID="txtSealNo" Text="<%$ resources:SealNo %>"></asp:Label>
                                            <asp:TextBox ID="txtSealNo" runat="server" TabIndex="4" MaxLength="100" CssClass="input-disabled"></asp:TextBox>
                                            <div class="clear">
                                            </div>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="pnlSCDetails">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblSCNoDtls" runat="server" AssociatedControlID="ddlSCNoDtls" Text='<%$ resources:SCNoDtls %>'></asp:Label>
                                                <asp:DropDownList ID="ddlSCNoDtls" runat="server" TabIndex="2" AutoPostBack="true" CssClass="select-small-c"
                                                    OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfSCNoDtls" runat="server" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblSCDtlsDate" runat="server" AssociatedControlID="txtSCDtlsDate"
                                                    Text="<%$ resources:SCDtlsDate %>"></asp:Label>
                                                <asp:TextBox ID="txtSCDtlsDate" runat="server" MaxLength="12" Enabled="false" CssClass="Uidate-picker input-disabled input-small"
                                                    TabIndex="2"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S input-margin2 nomargin" style="margin-left: -5px !important;">
                                                <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:SCDtlsBrand %>"></asp:Label>
                                                <asp:TextBox ID="txtBrand" runat="server" TabIndex="2" MaxLength="300" CssClass="select-full"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="LoadingDet" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                                <asp:Button ID="btnSelectBrand" runat="server" OnClick="ActionHandler" CommandName="BRANDSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField ID="hdfSoPk" runat="server" />
                                                <asp:HiddenField ID="hdfLoadPlanSodPk" runat="server" />
                                                <asp:HiddenField ID="hdfLoadPlanSodPk1" runat="server" />
                                                <asp:HiddenField ID="hdfBrand" runat="server" />
                                                <asp:HiddenField ID="hdfShpPlanQty" runat="server" />
                                                <asp:HiddenField ID="hdfShpPlanQtyValid" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <%--  <div class="blockr"></div>--%>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S input-margin2 nomargin">
                                            <asp:Label ID="lblRowFrom" runat="server" AssociatedControlID="txtRowFrom" Text="<%$ resources:RowFrom %>"></asp:Label>
                                            <asp:TextBox ID="txtRowFrom" runat="server" TabIndex="3" MaxLength="8" CssClass="Uiinput-amount  numeric input-small"
                                                onkeypress="return isFloatNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                            <%--CssClass="input-104  numeric"--%>
                                            <asp:Label ID="lblRowTo" runat="server" AssociatedControlID="txtRowTo" Text="<%$ resources:RowTo %>"
                                                CssClass="middle-lbl-xsmall-e txt-center"></asp:Label>
                                            <asp:TextBox ID="txtRowTo" runat="server" TabIndex="4" MaxLength="8" CssClass="Uiinput-amount  numeric input-small"
                                                onkeypress="return isFloatNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfRowFrom" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="LoadingDet" EnableClientScript="true" runat="server" ControlToValidate="txtRowFrom"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_RowFrom %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="csvRow" runat="server" Display="Dynamic" CssClass="star"
                                                    SetFocusOnError="true" Text="*" ControlToValidate="txtRowTo" EnableClientScript="true"
                                                    ClientValidationFunction="CheckRow" ErrorMessage="<%$ resources:Err_ValidRow %> "
                                                    ValidationGroup="LoadingDet"></asp:CustomValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblLoaded" runat="server" AssociatedControlID="txtLoaded" Text="<%$ resources:Loaded %>"></asp:Label>
                                            <asp:TextBox ID="txtLoaded" runat="server" TabIndex="6" MaxLength="6" onchange="CalculateQty();"
                                                onpaste="return false;" CssClass="Uiinput-amount  numeric input-small" onkeypress="return isFloatNumberKey(event)"></asp:TextBox>
                                            <asp:Label ID="lblX" runat="server" AssociatedControlID="txtLoaded2" Text="<%$ resources:X %>"
                                                CssClass="middle-lbl-xsmall-e txt-center"></asp:Label>
                                            <asp:TextBox ID="txtLoaded2" runat="server" TabIndex="7" MaxLength="6" onchange="CalculateQty();"
                                                onpaste="return false;" CssClass="Uiinput-amount  numeric input-small" onkeypress="return isFloatNumberKey(event)"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfX" CssClass="star" SetFocusOnError="true" ValidationGroup="LoadingDet"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtLoaded" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_LoadedRow1 %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="vrfY" CssClass="star" SetFocusOnError="true" ValidationGroup="LoadingDet"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtLoaded2" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_LoadedRow2 %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblLotNO" runat="server" AssociatedControlID="txtLotNO" Text="<%$ resources:LotNo %>"></asp:Label>
                                            <%--<asp:DropDownList ID="ddlLotNo" runat="server" TabIndex="10">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtLotNO" runat="server" TabIndex="10" MaxLength="200" CssClass="input-small"></asp:TextBox>
                                            <asp:HiddenField ID="hdfLotNO" runat="server" />
                                            <asp:HiddenField ID="hdfLoadPlanPk" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S input-margin2 nomargin">
                                            <asp:Label ID="lblType" runat="server" AssociatedControlID="txtType" Text="<%$ resources:Type %>"></asp:Label>
                                            <asp:TextBox ID="txtType" runat="server" TabIndex="5" MaxLength="100" CssClass="select-halfsmall-a"></asp:TextBox>
                                                                                      
                                            <asp:Label ID="lblCartonFrom" runat="server" AssociatedControlID="txtRowFrom" Text="<%$ resources:CartonFrom %>"></asp:Label>
                                            <asp:TextBox ID="txtCartonFrom" runat="server" TabIndex="8" MaxLength="8" CssClass="Uiinput-amount  numeric input-small margnrgt1-4per" onpaste="return false;"></asp:TextBox>
                                            <%--onkeypress="return isFloatNumberKey(event)"--%>
                                            <asp:Label ID="CartonTo" runat="server" AssociatedControlID="txtCartonTo" Text="To"
                                                CssClass=" middle-lbl-small-c-20-11-6"></asp:Label><%--label-30--%>
                                            <asp:TextBox ID="txtCartonTo" runat="server" TabIndex="9" MaxLength="8" CssClass="Uiinput-amount  numeric input-small-19-11" onpaste="return false;"></asp:TextBox>
                                            <%--onkeypress="return isFloatNumberKey(event)"--%>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="LoadingDet" EnableClientScript="true" runat="server" ControlToValidate="txtCartonFrom"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_CartonFrom %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Qty %>"></asp:Label>
                                            <asp:TextBox ID="txtQty" runat="server" MaxLength="12" Enabled="false" CssClass="Uiinput-amount numeric input-disabled input-small"
                                                TabIndex="8" onkeypress="return isFloatNumberKey(event)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfQty" CssClass="star" SetFocusOnError="true" ValidationGroup="LoadingDet"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_LoadedQty %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfQty" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfAlert" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="pnlSCWtDetails">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblIrradiationLotNo" runat="server" AssociatedControlID="txtIrradiationLotNo"
                                                    Text="<%$ resources:IrradiationLotNo %>"></asp:Label>
                                                <asp:TextBox ID="txtIrradiationLotNo" runat="server" TabIndex="10" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblExpDate" runat="server" AssociatedControlID="txtExpDate" Text="<%$ resources:ExpDate %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtExpDate" CssClass="Uidate-picker input-small" TabIndex="10"
                                                    onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                                <%--<asp:TextBox ID="txtVatRefundDate" runat="server" MaxLength="200" CssClass="medium"
                                                    TabIndex="65" onkeydown="return CheckKey(event)" onpaste="return false;" />--%>
                                                <cc1:CalendarExtender runat="server" ID="txtExpDate_CalendarExtender" BehaviorID="calendar1"
                                                    TargetControlID="txtExpDate" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                    ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                                </cc1:CalendarExtender>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblNetWtBox" runat="server" AssociatedControlID="txtNetWtBox" Text="<%$ resources:NetWtBox %>"></asp:Label>
                                                <asp:TextBox ID="txtNetWtBox" runat="server" TabIndex="10" MaxLength="15" CssClass="Uiinput-amount numeric input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblNetWtCarton" runat="server" AssociatedControlID="txtNetWtCarton"
                                                    Text="<%$ resources:NetWtCarton %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNetWtCarton" TabIndex="10" MaxLength="15" CssClass="Uiinput-amount numeric input-small"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblGrossWtBox" runat="server" AssociatedControlID="txtGrossWtBox"
                                                    Text="<%$ resources:GrossWtBox %>"></asp:Label>
                                                <asp:TextBox ID="txtGrossWtBox" runat="server" TabIndex="10" MaxLength="15" CssClass="Uiinput-amount numeric input-small"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S input-margin2 nomargin">
                                                <asp:Label ID="lblGrossWtCarton" runat="server" AssociatedControlID="txtGrossWtCarton"
                                                    Text="<%$ resources:GrossWtCarton %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtGrossWtCarton" TabIndex="10" MaxLength="15" CssClass="Uiinput-amount numeric input-small"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S input-margin2 nomargin">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S input-margin2 nomargin">
                                            <div class="btnwrap-divcol">
                                                <asp:Button runat="server" ID="btnAddToList" CommandName="ADD_ACTION" TabIndex="10"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry"
                                                    ValidationGroup="LoadingDet" Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add"
                                                    OnClientClick="return ValidatePageNow('LoadingDet')" />
                                            </div>
                                            <div style="display: none;">
                                                <asp:Button runat="server" ID="btnCancelAlert" CommandName="CANCEL" OnClick="ActionHandler" />
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="fields-grpwrap">
                                <asp:HiddenField ID="hdfSLNo" runat="server" Value="0" />
                                <%--<div class="header">
                                    <h1>
                                        <%= GetLocalResourceObject("ItemDetails").ToString()%></h1>
                                </div>--%>
                                <div class="fields-group">
                                    <div class="gridwrap scroll-container scroll-container-19-11">
                                        <asp:GridView ID="grdLoadingPlanDet" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            OnRowDataBound="ActionHandler" Width="1800px" ShowFooter="true" TabIndex="11">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Row %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRow" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPD_ROW_FROM") + (Eval("LPD_ROW_TO").ToString()!="0"? " - "+ Eval("LPD_ROW_TO"):""),20) %>'
                                                            ToolTip='<%# Eval("LPD_ROW_FROM") + (Eval("LPD_ROW_TO").ToString()!="0"? " - "+ Eval("LPD_ROW_TO"):"")  %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfRow" runat="server" Value='<%#Eval("LPD_ROW_FROM") + " - "+ Eval("LPD_ROW_TO") %>' />
                                                        <asp:HiddenField ID="hdfCarton" runat="server" Value='<%#Eval("LPD_CARTON_FROM") + " - "+ Eval("LPD_CARTON_TO") %>' />
                                                        <asp:HiddenField ID="hdfLPDPK" runat="server" Value='<%#Eval("LPD_PK") %>' />
                                                        <asp:HiddenField ID="hdfRowNo" runat="server" Value='<%#Eval("ROW_NO") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" Wrap="false" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_SCNo%>">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval("LPH_SOH_NO") %>'
                                                            OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval("LPH_SO_HDR") %>'
                                                            ToolTip='<%# Eval("LPH_SOH_NO") %>'></asp:LinkButton>
                                                        <%--<asp:Label ID="lblSCNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPH_SOH_NO"),20) %>'
                                                            ToolTip='<%# Eval("LPH_SOH_NO").ToString() %>'></asp:Label>--%>
                                                        <asp:HiddenField ID="hdfSCNo" runat="server" Value='<%#Eval("LPH_SO_HDR") %>' />
                                                        <asp:HiddenField ID="hdfSodPk" runat="server" Value='<%#Eval("LPH_SO_DTL") %>' />
                                                        <asp:HiddenField ID="hdfSodPk1" runat="server" Value='<%#Eval("LPH_SO_DTL") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_SCDate%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSCDate" runat="server" Text='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("LPH_SC_DATE"))) ? Convert.ToDateTime(Eval("LPH_SC_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'
                                                            ToolTip='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("LPH_SC_DATE"))) ? Convert.ToDateTime(Eval("LPH_SC_DATE")).ToString(Resources.Constants.ReportDateFormat):"" %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="65px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_Brand%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBrand" runat="server" Text='<%#HttpUtility.HtmlDecode( Eval("LPH_BRAND_NAME").ToString()) %>'
                                                            ToolTip='<%#HttpUtility.HtmlDecode( Eval("LPH_BRAND_NAME").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfBrand" runat="server" Value='<%#Eval("LPH_BRAND") %>' />
                                                        <asp:HiddenField ID="hdfShpPlanQty" runat="server" Value='<%#Eval("SND_PLAN_QTY") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="380px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPD_TYPE"),13) %>'
                                                            ToolTip='<%# Eval("LPD_TYPE").ToString() %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("LPD_TYPE") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Loaded %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLoaded" runat="server" Text='<%#Eval("LPD_QTY_X") +" x " + Eval("LPD_QTY_Y") %>'
                                                            ToolTip='<%# Eval("LPD_QTY_X") +" x " + Eval("LPD_QTY_Y").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" Wrap="false" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty%>" FooterStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%#Eval("LPD_QTY") %>'
                                                            ToolTip='<%#Eval("LPD_QTY") %>'></asp:Label>--%>
                                                        <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# AddCommas(Eval("LPD_QTY")) %>'
                                                            ToolTip='<%# AddCommas(Eval("LPD_QTY")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotQty" runat="server" Text='<%#GetColumnTotal(Eval("LPD_QTY")) %>'
                                                            ToolTip='<%#GetColumnTotal(Eval("LPD_QTY")) %>'></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="25px" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="5px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:LotNo%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGLotNo" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("LPD_LOT_NO").ToString()) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("LPD_LOT_NO").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Carton%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCarton" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPD_CARTON_FROM") + (Eval("LPD_CARTON_TO").ToString()!="0"? " - "+ Eval("LPD_CARTON_TO"):""),20) %>'
                                                            ToolTip='<%# Eval("LPD_CARTON_FROM") + (Eval("LPD_CARTON_TO").ToString()!="0"? " - "+ Eval("LPD_CARTON_TO"):"")  %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_IrradiationLotNo%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIrradiationLotNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPH_IR_RADIATION_LOT_NO"),13) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("LPH_IR_RADIATION_LOT_NO").ToString())  %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_ExpDate%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpDate" runat="server" Text='<%#Eval("LPH_EXP_DATE") %>' ToolTip='<%# Eval("LPH_EXP_DATE")  %>'></asp:Label>
                                                        <%--<asp:Label ID="lblExpDate" runat="server" Text='<%#Eval("LPH_EXP_DATE", Resources.Constants.DateFormatGrid) %>'
                                                            ToolTip='<%# Eval("LPH_EXP_DATE", Resources.Constants.DateFormatGrid)  %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_NetWtBox%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNetWtBox" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("LPH_NET_WT").ToString()) ? Eval("LPH_NET_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_NET_WT")):"":"" %>'
                                                            ToolTip='<%# !string.IsNullOrEmpty(Eval("LPH_NET_WT").ToString()) ? Eval("LPH_NET_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_NET_WT")):"":"" %>'></asp:Label>
                                                        <%--<asp:Label ID="lblNetWtBox" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPH_NET_WT"),13) %>'
                                                            ToolTip='<%# Eval("LPH_NET_WT").ToString() %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_NetWtCarton%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNetWtCarton" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("LPH_CTN_NET_WT").ToString()) ? Eval("LPH_CTN_NET_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_CTN_NET_WT")):"":"" %>'
                                                            ToolTip='<%# !string.IsNullOrEmpty(Eval("LPH_CTN_NET_WT").ToString()) ? Eval("LPH_CTN_NET_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_CTN_NET_WT")):"":"" %>'></asp:Label>
                                                        <%--<asp:Label ID="lblNetWtCarton" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("LPH_CTN_NET_WT"),13) %>'
                                                            ToolTip='<%# Eval("LPH_CTN_NET_WT").ToString() %>'></asp:Label>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="110px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_GrossWtBox%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrossWtBox" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("LPH_GROSS_WT").ToString()) ? Eval("LPH_GROSS_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_GROSS_WT")):"":"" %>'
                                                            ToolTip='<%# !string.IsNullOrEmpty(Eval("LPH_GROSS_WT").ToString()) ? Eval("LPH_GROSS_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_GROSS_WT")):"":"" %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GH_GrossWtCarton%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrossWtCarton" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("LPH_CTN_GROSS_WT").ToString()) ? Eval("LPH_CTN_GROSS_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_CTN_GROSS_WT")):"":"" %>'
                                                            ToolTip='<%# !string.IsNullOrEmpty(Eval("LPH_CTN_GROSS_WT").ToString()) ? Eval("LPH_CTN_GROSS_WT").ToString()!="0"?GetFormattedWeight(Eval("LPH_CTN_GROSS_WT")):"":"" %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="85px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEdit" runat="server" OnClick="ActionHandler" CommandName="EDITGRID"
                                                            SkinID="imbeditgrid" ToolTip="Edit" TabIndex="12" CommandArgument="PageAction_Entry"
                                                            OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                        <asp:ImageButton ID="btnDelete" runat="server" OnClick="ActionHandler" CommandName="DELETEGRID"
                                                            SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="13" CommandArgument="PageAction_Entry"  OnClientClick="return ShowDeleteConfirm(this);"
                                                            OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="LoadingPlan" runat="server" />
                    <asp:ValidationSummary ID="vsLoadingDet" ValidationGroup="LoadingDet" runat="server" />
                </div>
                <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="ContainerInspection" />
            </div>
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            <asp:HiddenField ID="hdfWeightDecimalDigit" Value="0" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <asp:HiddenField ID="hdfLotNobyBrand" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
