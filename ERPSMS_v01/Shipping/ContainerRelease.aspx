<%@ Page Title="<%$ Resources:Captions,Title_ContainerRelease %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="ContainerRelease.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.ContainerRelease" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        // var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        var urlSO = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtReleaseDate");
            GrandScriptUtils.DatePickerCommon("txtStartLDate");
            $('[id$=txtOutTime]').timepicker();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            var date = $("[id$=txtReleaseDate]").val();
            var transDate = $("[id$=txtReleaseDate]").val();

            //            GrandScriptUtils.MakeAutoCompleteDDL("txtPalleteBinCard", url, "hdfPalleteBinCard", true, true, "PALLETEBINCARD");

            GrandScriptUtils.MakeAutoCompleteDDL("txtPalleteBinCard", url + "?SCID=" + $("[id$=hdfCurrentSCID]").val() + "&BrandID=" + $("[id$=hdfCartonBrandPk]").val() + "&FieldName=PBH_PK", "hdfPalleteBinCard", true, true, "PALLETE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", urlSO, "hdfSoPK", true, true, "SONUMBERAPPROVED");
            GrandScriptUtils.MakeAutoComplete("txtBatch", "MaterialManagement.do?Action=GetBatchNoConsumption&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + $("[id$=hdfItemPK]").val() + "&DepartmentID=" + $("select[id$=ddlIssueStore]").val() + "&Date=" + date + "&transDate=" + transDate + "&CDHPk=0", "hdfBatchPK", true, false, "BizUnitPk", true);
            //             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            $("[id*=txtQty]").ForceNumericOnly();
            if ($("[id$=hdfSelectedBatch]").val()!='') {
                $("[id$=txtBatch]").val($("[id$=hdfSelectedBatch]").val());
                $("[id$=hdfBatchPK]").val($("[id$=hdfSelectedBatchPK]").val());
            }
        }

        //         function EndRequestHandler(sender, args){
        //        if (args.get_error() != undefined){
        //            alert(args.get_error().message.substr(args.get_error().name.length + 2));
        //            args.set_errorHandled(true);
        //        }
        //    }

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
                if ($("[id$=litErrorMsg]").text() == '') {
                    $("[id$=litErrorMsg]").text('Enter valid quantity');
                }
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
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
            if (mode == 3) {
                var delstatus = parseFloat($("#[id*=hdfDelstatus]").val());
                if (delstatus == 1) {
                    $("[id$=pnlSave]").hide();
                }
            }
        }

        function cartonAddToListValidate(sender, args) {
            if ($("[id$=txtPalleteBinCard]").val() != 'Select/Type') {
                return args.IsValid = true;
            }
            //            if ($("[id$=txtSONumber]").val() != 'Select/Type') {
            //                return args.IsValid = true;
            //            }
            //            if ($("[id$=txtCartonPrefixPopUp]").val() != '' && $("[id$=txtCartonFromPopUp]").val() != '' && $("[id$=txtCartonToPopUp]").val() != '') {
            if ($("[id$=txtCartonFromPopUp]").val() != '') {
                return args.IsValid = true;
            }
            $("[id$=litErrorMsg]").text('Enter Pallete or Carton details');
            return args.IsValid = false;
        }

        function fnConfirmQuantityNotMatch() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_CartonQtyOrItemQtyMissmatch").ToString() %>';
            // 'Carton quantity or Item quantity mismatch with DO quantity';  //'<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmQuantityNotMatch]").val('1');
                        $("[id$=btnApplyCartonDetails]").click();
                    },
                    No: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmQuantityNotMatch]").val('0');
                    }
                }
            });
            return false;
        }

          function ConfirmQtyNotMatchInBin() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_CartonQtyOrItemQtyMissmatch").ToString() %>';
            // 'Carton quantity or Item quantity mismatch with DO quantity';  //'<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmQuantityNotMatch]").val('1');
                        $("[id$=btnApply]").click();
                    },
                    No: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmQuantityNotMatch]").val('0');
                    }
                }
            });
            return false;
        }

        function ConfirmAutoAllocation(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Confirmation_AutoAllocate").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmAutoAllocate]").val('1');
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmAutoAllocate]").val('0');
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
                    $("[id$=txtPalleteBinCard]").focus();
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

        // for GO with PlusBtnClick
        function PlusBtnClick(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13) {
                $("#txtPalleteBinCard").trigger("blur.autocomplete");
                $('[id$=btnPlus2]').click();
                $("[id$=txtPalleteBinCard]").autocomplete("disable");
                return false;
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
            if (CtrlID == 'txtPalleteBinCard')
                $("[id$=txtPalleteBinCard]").focus();
            if (CtrlID == 'txtIssueBinCard') {
                $("[id$=txtIssueBinCard]").val('');
                $("[id$=hdfBinCard]").val('0');
                $("[id$=txtIssueBinCard]").focus();
            }
        }
        //For Showing Confirmation Msg at the time of deleting an item.(Deleting an item also delete carton allocation againt that item)
        function ShowConfirmDeleteAllocation(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = $("[id$=hdfConfirmMessage]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmDeleteAllocation]").val('1');
                        $("[id$=" + btn + "]").click(); // $("[id$=imgbtnDummyRemoveItem]").click(); 
                        // $("[id$=ctl00_MainContent_grdDeliveryList_ctl02_btnRemoveItem]").click();                       
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=hdfConfirmDeleteAllocation]").val('0');
                        return false;
                    }
                }
            });
            return false;
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtBatch") {
                QtyDec = $("[id$='hdfQtyDecimalP2P']").val();
                var batchID = $("[id$=hdfBatchPK]").val();
                $.getJSON("MaterialManagement.do?Action=GetBatchDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&BatchID=" + batchID, function (data) {
                    if (data != null) {
                        if (batchID != 0) {
                            $("[id$=txtStock]").val(parseFloat(data[0].SBD_QTY_IN_STOCK).toFixed(QtyDec));
                            $("[id$=txtUOM]").val(data[0].SBD_UOM_TEXT);
                            $("[id$=hdfUOMPK]").val(data[0].SBD_UOM);
                        }
                        else {
                            $("[id$=txtStock]").val("");
                            $("[id$=txtUOM]").val("");
                            $("[id$=hdfUOMPK]").val("0");
                        }
                    }
                });
            }
        }

        function SetBinAutoComplete() {
            $("[id$=txtIssueBinCard]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "containerRelease.aspx/GetBinNo",
                        data: '{"SearchKey" : "' + request.term + '", "dept" : "' + $("select[id$=ddlIssueStore]").val() + '", "PageSize" : "' + $("[id$=hdfBinPageSize]").val() + '", "IsQaPassed" : "' + $("[id$=hdfIsQaPassedBinsOnly]").val() + '", "ItemPK" : "' + $("[id$=hdfItemPK]").val() + '"}',
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.Value,
                                    id: item.Key
                                }
                            }));
                        }
                    });
                },
                select: function (event, ui) {
                    $("[id$=hdfBinCard]").val(ui.item.id);
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
                            valid = false;
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        if (!valid) {
                            $(this).val("");
                            $("[id$=hdfBinCard]").val("0");
                            $(this).data("autocomplete").term = "";
                            return false;
                        }
                    }
                }
            });
               $("[id$=txtIssueBinCard]").focus();
        }

        function Search(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13) {
                $("#txtIssueBinCard").trigger("blur.autocomplete");
                $('[id$=imbAddBin]').click();
                $("[id$=txtIssueBinCard]").autocomplete("disable");
                return false;
            }
        }

        function ConfirmContinueWithoutAllocation(btn, message) {

            if ($("[id$=hdfAllocationExist]").val() == 0 && $("[id$=hdfHasAllocation]").val() == 1) {

            } else {
                return true;
            }

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetGlobalResourceObject("ErrorMessages", "ConfirmNoAllocation").ToString() %>';
            msg += $("[id$=hdfSomeCartonMissingMessage]").val();
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                    }
                }
            });
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlContainerRelease">
        <ContentTemplate>
            <asp:HiddenField ID="hdfConfirmQuantityNotMatch" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSomeCartonMissingMessage" runat="server" Value="" />
            <asp:HiddenField ID="hdfSomeCartonMissingConfirm" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrentSCID" runat="server" Value="" />
            <asp:HiddenField ID="hdfCurrentCDR_PK" runat="server" Value="" />
            <asp:HiddenField ID="hdfCartons" runat="server" Value="" />
            <asp:HiddenField ID="hdfCartonListReloadConfirm" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCartonBrandPk" runat="server" Value="0" />
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
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="18" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('invoice')"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="19"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:return ValidatePageNow('invoice') && ConfirmContinueWithoutAllocation(this);"
                                            ValidationGroup="invoice" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="19" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:return ValidatePageNow('invoice') && ConfirmContinueWithoutAllocation(this);"
                                            ValidationGroup="invoice" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="22" CommandArgument="SEC_ActionPanel"
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
                                TabIndex="5" CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                TabIndex="6" CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                TabIndex="7" CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadQADocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadQADocs" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                TabIndex="8" CommandName="UPLOADQA" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadExportDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadExportDocs" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                TabIndex="9" CommandName="UPLOADEXPORT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                TabIndex="10" CommandName="LOADINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="11" CommandName="UPLOADPHOTOGRAPHS" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="12" CommandName="GOODOUTWARD" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="13" CommandName="CONTAINERRELEASE" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="20" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="14" CommandName="PRINT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
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
                                    <asp:Label runat="server" ID="lbnCustomerHdr" Text="<%$resources:Controls,CustomerHdr%>"
                                        AssociatedControlID="lblCustomerHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblCustomerHdr" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnDestinationPortHdr" runat="server" Text="<%$resources:Controls,DestinationPortHdr%>"
                                        AssociatedControlID="lblDestinationPortHdr"></asp:Label>
                                    <asp:Label runat="server" ID="lblDestinationPortHdr" CssClass="disp-table"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnInTimeHdr" runat="server" AssociatedControlID="lblInTimeHdr" Text="<%$resources:Controls,InTimeHdr%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblInTimeHdr"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label runat="server" ID="lbnContainerTypeValueHdr" Text="<%$resources:ContainerTypeHr%>"
                                        AssociatedControlID="lblContainerTypeValueHdr">
                                    </asp:Label>
                                    <asp:Label runat="server" ID="lblContainerTypeValueHdr"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnInvoiceHdr" runat="server" Text="<%$resources:Controls,GONNoHdr%>"
                                        AssociatedControlID="lblGONHdr"></asp:Label>
                                    <asp:Label runat="server" ID="lblGONHdr"></asp:Label>
                                </div>
                                <div class="div3col-S">
                                    <asp:Label ID="lbnInvoiceDateHdr" runat="server" AssociatedControlID="lblGONDateHdr"
                                        Text="<%$resources:Controls,GONDateHdr%>"></asp:Label>
                                    <asp:Label runat="server" ID="lblGONDateHdr"></asp:Label>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblContainerNo" Text="<%$ resources:ContainerNo%>"
                                                AssociatedControlID="lblDispContainerNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispContainerNo" Text="" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSealNo" Text="<%$ resources:SealNo%>" AssociatedControlID="lblDispSealNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispSealNo" Text="" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDestinationPort" Text="<%$ resources:DestinationPort%>"
                                                AssociatedControlID="lblDispDestinationPort"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispDestinationPort" CssClass="select-half"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblInTime" Text="<%$ resources:InTime%>" AssociatedControlID="lblDispInTime"></asp:Label>
                                            <asp:Label runat="server" ID="lblDispInTime" Text="" CssClass="Uidate-picker"></asp:Label>

                                            <asp:Label runat="server" ID="lblTrailerNo" Text="<%$ resources:TrailerLicenseNo%>" CssClass="label-03-12"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtLicenseNo" Text="" MaxLength="100" TabIndex="16"
                                                        CssClass="lbl-20perc"></asp:TextBox>

                                        </div>
                                    </td>
                                  
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblReleaseDate" Text="<%$ resources:ReleaseDate%>"
                                                AssociatedControlID="txtReleaseDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReleaseDate" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="Uidate-picker"></asp:TextBox>
                                            <asp:HiddenField ID="hdfReleaseDateat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfReleaseDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtReleaseDate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReleaseDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblStartLoadingDate" Text="<%$ resources:StartLoadingDate%>"
                                                AssociatedControlID="txtStartLDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtStartLDate" TabIndex="1" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="Uidate-picker"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblOutTime" Text="<%$ resources:OutTime%>" AssociatedControlID="txtOutTime"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtOutTime" CssClass="Uidate-picker" TabIndex="2"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfOutTimeat" runat="server" Value="" />
                                            <asp:RequiredFieldValidator ID="vrfOutTime" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="invoice" EnableClientScript="true" InitialValue="" runat="server"
                                                ControlToValidate="txtOutTime" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_OutTime %>">
                                            </asp:RequiredFieldValidator>


                                            <asp:Label runat="server" ID="lblTruckNo" Text="<%$ resources:TruckLicenseNo%>" CssClass="label-03-12"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTruckLicenseNo" Text="" MaxLength="100" TabIndex="16"
                                                        CssClass="lbl-20perc"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" TabIndex="3" TextMode="MultiLine" CssClass="multiline-2line"
                                                onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- divDeliveryList: Visibility is setted by LocalResourceObject "ShowDeliveryList" --%>
                            <div id="divDeliveryList" runat="server" class="gridwrap">
                                <asp:GridView ID="grdDeliveryList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="false" OnRowCommand="ActionHandler" TabIndex="13" OnRowDataBound="ActionHandler">
                                    <%--OnRowDataBound="ActionHandler"--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval("SOD_NO") %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" ToolTip='' CommandArgument='<%# Eval("SOD_SO") %>'></asp:LinkButton>
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='<%# Eval("SOD_SO") %>' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='<%# Eval("CDR_SO_DTL") %>' runat="server" />
                                                <asp:HiddenField ID="hdfCDR_PK" Value='<%# Eval("CDR_PK") %>' runat="server" />
                                                <asp:HiddenField ID="hdfCDR_SL_NO" Value='<%# Eval("CDR_SL_NO") %>' runat="server" />
                                                <asp:HiddenField ID="hdfCDR_DO_DTL" Value='<%# Eval("CDR_DO_DTL") %>' runat="server" />
                                                <asp:HiddenField ID="hdfAPS_TOTAL_PCS" Value='<%# Eval("APS_TOTAL_PCS") %>' runat="server" />
                                                <asp:HiddenField ID="hdfItemType" Value='<%# Eval("SOD_IS_PACK_MAT") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval("SOD_DATE", Resources.Constants.DateFormatGridExpanded) %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IGPLCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblITM_CODE" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_CODE").ToString()),15) %>'
                                                    ToolTip='<%# Eval("ITM_CODE") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCDR_ITEM" Value='<%# Eval("CDR_ITEM") %>' runat="server" />
                                                <asp:HiddenField ID="hdfItemName" Value='<%# Eval("ITM_NAME") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='<%# Eval("CIM_BRAND_NAME") %>'
                                                    ToolTip='<%# Eval("CIM_BRAND_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfBrandPk" Value='<%# Eval("CDR_CUST_ITEM") %>' runat="server" />
                                                <asp:HiddenField ID="hdfSaleUnit" Value='<%# Eval("SaleUnit") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="24%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOMSales" runat="server" Text='<%# Eval("UOM_CODE") %>' ToolTip='<%# Eval("UOM_CODE") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCDR_UOM" Value='<%# Eval("CDR_UOM") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DOQtyInCartons %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%-- <asp:TextBox ID="txtDespNowSales" runat="server" onkeyup="CalculateTotalFooter(this);"
                                                    CssClass="small-a numeric margn-rgt-0" MaxLength="10" TabIndex="13"></asp:TextBox>--%>
                                                <asp:Label ID="lblQtyCartonsDO" runat="server" Text='<%# Eval("CDR_DO_CARTON") %>'
                                                    ToolTip='<%# Eval("CDR_DO_CARTON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblQtyCartonDOFooter"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfQtyCartonDOFooter" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DOQtyInPcs %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtDespNow" runat="server" onkeyup="CalculateTotalFooter(this);"
                                                    CssClass="input-disabled small-a numeric" MaxLength="17" TabIndex="13" Enabled="false"></asp:TextBox>--%>
                                                <%--   <asp:Label ID="lblDOQty" runat="server" Text='<%# Eval("CDR_DO_QTY") %>' ToolTip='<%# Eval("CDR_DO_QTY") %>'></asp:Label>--%>
                                                <asp:Label ID="lblDOQty" runat="server" Text='<%# Eval("CDR_DO_QTY_PCS") %>' ToolTip='<%# Eval("CDR_DO_QTY_PCS") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCDR_DO_QTY" runat="server" Value='<%# Eval("CDR_DO_CARTON") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalDOPcsSplit"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalDOPcsSplit" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QtyInCartons %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%-- <asp:TextBox ID="txtDespNowSales" runat="server" onkeyup="CalculateTotalFooter(this);"
                                                    CssClass="small-a numeric margn-rgt-0" MaxLength="10" TabIndex="13"></asp:TextBox>--%>
                                                <asp:Label ID="lblQtyCartons" runat="server" Text='<%# Eval("CDR_CARTON_DESPATCHED") %>'
                                                    ToolTip='<%# Eval("CDR_CARTON_DESPATCHED") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCDR_CARTON_DESPATCHED" runat="server" Value='<%# Eval("CDR_CARTON_DESPATCHED") %>' />

                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooterSplitSales"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplitSales" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QtyInPcs %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtDespNow" runat="server" onkeyup="CalculateTotalFooter(this);"
                                                    CssClass="input-disabled small-a numeric" MaxLength="17" TabIndex="13" Enabled="false"></asp:TextBox>--%>
                                                <asp:Label ID="lblDespNow" runat="server" Text='<%# Eval("CDR_QTY_DESPATCHED") %>'
                                                    ToolTip='<%# Eval("CDR_QTY_DESPATCHED") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfCDR_QTY_DESPATCHED" runat="server" Value='<%# Eval("CDR_QTY_DESPATCHED") %>' />
                                                <asp:HiddenField ID="hdfIsPackedBin" runat="server" Value='<%# Eval("CDR_IS_PACKED_BIN") %>' />
                                                <asp:HiddenField ID="hdfInvApproved" runat="server" Value='<%# Eval("DPH_IS_INVOICE_APPROVED") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooterSplit"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplit" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRemoveItem" runat="server" CommandName="REMOVEITEM"
                                                    SkinID="imbdeletegrid" ToolTip="Delete" />
                                                <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="show-carton"
                                                    CommandName="EDIT_ACTION" ToolTip="<%$resources:Controls,Allocation %>" />
                                                <%--   Visible='<%# Convert.ToInt32(Eval("DPH_IS_INVOICE_APPROVED"))==1?true:false %>'--%>
                                                <asp:ImageButton runat="server" ID="imbPackedBin" SkinID="grn-icon"
                                                    CommandName="PACKEDBIN" ToolTip="<%$resources:Controls,PackedBinAllocation %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="divCartonDtlsPopUp" style="display: none;">
                                    <div class="content-wrapper">
                                        <div class="Button-container-popup" id="btnContainer">
                                            <%-- <asp:Button runat="server" ID="btnAutoAllocate" SkinID="btnInner-allocate" Text="<%$ resources:Controls,AutoAllocate %>"
                                                ToolTip="<%$resources:Controls,AutoAllocate %>" OnClick="ActionHandler" CommandName="AUTOALLOCATECARTON"
                                                OnClientClick="return ConfirmAutoAllocation(this);" />                                            
                                             <asp:Button runat="server" ID="btnClearAllocate" SkinID="btnInner-deallocate" Text="<%$ resources:Controls,Deallocate %>"
                                                ToolTip="<%$resources:Controls,Deallocate %>" OnClick="ActionHandler" CommandName="CLEARAUTOALLOCATION" />--%>
                                            <%-- <asp:Button runat="server" ID="btnDmgClear" SkinID="btnInner-Cancel" Text="Clear"
                                                OnClick="ActionHandler" CommandName="CLEARITEM" />--%>
                                            <asp:Button runat="server" ID="btnApplyCartonDetails" SkinID="btnInner-add-dsd" Text="<%$ resources:Controls,Apply %>"
                                                ToolTip="<%$resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="APPLY" />
                                            <asp:HiddenField ID="hdfCDR_SO_DTL" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfCDR_SL_NO_PopUp" runat="server" Value="0" />
                                        </div>
                                        <div class="head-info">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 35%;">
                                                        <asp:Label runat="server" ID="Label5" Text="<%$resources:SCNO:%>" AssociatedControlID="lblSCNoPopUp"
                                                            CssClass="w31-5perc">
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblSCNoPopUp" CssClass="bold"></asp:Label>
                                                    </td>
                                                    <td style="width: 32%;">
                                                        <asp:Label runat="server" ID="Label11" Text="<%$resources:DONo:%>" AssociatedControlID="lblDONoPopUp"></asp:Label>
                                                        <asp:Label runat="server" ID="lblDONoPopUp" CssClass="bold"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <asp:Label ID="Label9" runat="server" AssociatedControlID="lblDOQtyPcs" Text="<%$resources:DOQtyPcs:%>"></asp:Label>
                                                        <asp:Label runat="server" ID="lblDOQtyPcs" CssClass="bold"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$resources:BrandName:%>" AssociatedControlID="lblBrandNamePopUp"
                                                            CssClass="margnbotm0 margn-lft30"></asp:Label>
                                                        <asp:Label runat="server" ID="lblBrandNamePopUp" CssClass="bold brandname margnbotm0"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divCartonDtls">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:CartonPrefix%>" AssociatedControlID="txtCartonPrefixPopUp"
                                                                CssClass="middle-lbl-small-e1"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCartonPrefixPopUp" CssClass="input-small" MaxLength="11"></asp:TextBox>
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:From%>" AssociatedControlID="txtCartonFromPopUp"
                                                                class="middle-lbl-xsmall-f"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCartonFromPopUp" CssClass="input-small numeric"
                                                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="11"
                                                                onpaste="return false;" ondrop="return false;"></asp:TextBox>
                                                            <asp:CustomValidator ID="cvalCartonFrom" runat="server" EnableClientScript="true"
                                                                ClientValidationFunction="cartonAddToListValidate" ControlToValidate="txtCartonFromPopUp"
                                                                ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterCartonFrom %>"
                                                                Text="*" Display="Static" ValidationGroup="AddToList" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>
                                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:To%>" AssociatedControlID="txtCartonToPopUp"
                                                                class="middle-lbl-xsmall-i"></asp:Label>
                                                            <%--class="middle-lbl"--%>
                                                            <asp:TextBox runat="server" ID="txtCartonToPopUp" CssClass="input-small numeric"
                                                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" MaxLength="11"
                                                                onpaste="return false;" ondrop="return false;"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <div id="divPalletNo" runat="server" class="display-inline">
                                                                <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PalletNo %>' AssociatedControlID="txtPalleteBinCard"
                                                                    CssClass="middle-lbl"></asp:Label>
                                                                <asp:TextBox ID="txtPalleteBinCard" runat="server" CssClass="select-small-c" onkeydown="return PlusBtnClick(event);">
                                                                </asp:TextBox>
                                                                <asp:HiddenField ID="hdfPalleteBinCard" runat="server" Value="-1" />
                                                                <asp:CustomValidator ID="cvalPalletBinCard" runat="server" EnableClientScript="true"
                                                                    ClientValidationFunction="cartonAddToListValidate" ControlToValidate="txtPalleteBinCard"
                                                                    ValidateEmptyText="true" ErrorMessage="<%$ resources:Err_EnterPallete %>" Text="*"
                                                                    Display="Static" ValidationGroup="AddToList" CssClass="star" SetFocusOnError="true"></asp:CustomValidator>
                                                            </div>
                                                            <div class="display-inline">
                                                                <asp:Label ID="lblAutoMode" runat="server" Text='<%$ Resources:AutoMode %>' AssociatedControlID="chkAutoMode"
                                                                    CssClass="middle-lbl-a0"></asp:Label>
                                                                <asp:CheckBox ID="chkAutoMode" runat="server" OnCheckedChanged="ActionHandler" CommandName="AUTOALLOCATECARTON"
                                                                    CssClass="style-none padgtop1" AutoPostBack="true" />
                                                                <asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                                    ValidationGroup="AddToList" OnClientClick="javascript:ValidatePageNow('AddToList')"
                                                                    SkinID="imbaddnew" Style="margin-top: 2px;" />
                                                                <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                                    ToolTip="<%$resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEARITEM"
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
                                                                <asp:Label runat="server" ID="Label6" Text="<%$ resources:Location%>" AssociatedControlID="ddlLocationPopUp"
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
                                                        <asp:TemplateField HeaderText="<%$ resources:PalletOrCarton %>">
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
                                                                <asp:ImageButton runat="server" ID="imbGrdEditDetails" SkinID="show-carton" CommandName="EDIT_ACTION"
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
                                <div id="divCartonDtlsPopUpDetail" style="display: none;">
                                    <div class="content-wrapper">
                                        <div runat="server" id="divMsgCartonDtlsPopUpDetail">
                                        </div>
                                    </div>
                                </div>
                                <div id="divMaterialCartonDtlsPopUp" style="display: none;">
                                    <div class="content-wrapper">
                                        <div class="Button-container-popup" id="btnContainer">
                                            <asp:Button runat="server" ID="btnApply" SkinID="btnInner-add-dsd" Text="<%$ resources:Controls,Apply %>"
                                                ToolTip="<%$resources:Controls,Apply %>" OnClick="ActionHandler" CommandName="PRODUCTAPPLY" />
                                            <asp:HiddenField ID="hdfItemTypePK" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfContainerReleaseDetailPK" runat="server" Value="0" />
                                        </div>
                                        <div class="head-info">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 30%;">
                                                        <asp:Label runat="server" ID="lblForSCNo" Text="<%$resources:SCNO:%>" AssociatedControlID="lblSCNo"
                                                            CssClass="w31-5perc">
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblSCNo" CssClass="bold"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfSCPK" Value="0" />
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <asp:Label ID="Label14" runat="server" AssociatedControlID="lblQty" Text="<%$resources:DOQtyPcs:%>"></asp:Label>
                                                        <asp:Label runat="server" ID="lblQty" CssClass="bold"></asp:Label>
                                                    </td>
                                                    <td style="width: 40%;">
                                                        <asp:Label runat="server" ID="lblForIssueStore" Text="<%$resources:IssueStore:%>" AssociatedControlID="ddlIssueStore"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlIssueStore" CssClass="input-half" Enabled="false"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblForItemName" runat="server" Text="<%$resources:ItemName:%>" AssociatedControlID="lblItemName"
                                                            CssClass="margnbotm0 margn-lft30"></asp:Label>
                                                        <asp:Label runat="server" ID="lblItemName" CssClass="bold brandname margnbotm0"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfItemPK" Value="0" />
                                                        <asp:HiddenField runat="server" ID="hdfItemSaleUnit" Value="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divItemDtls">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S" runat="server" id="divMaterial">
                                                            <asp:Label ID="lblBatch" runat="server" Text="<%$ resources:Batch%>" AssociatedControlID="txtCartonPrefixPopUp"
                                                                CssClass="middle-lbl-small-e1"></asp:Label>
                                                            <asp:TextBox ID="txtBatch" runat="server" CssClass="input-small-20-11-5"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBatchPK" runat="server" Value="0" />
                                                            <asp:RequiredFieldValidator ID="vrfBatch" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="item" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                runat="server" ControlToValidate="txtBatch" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_Batch %>"></asp:RequiredFieldValidator>

                                                            <asp:Label ID="lblForStock" runat="server" Text="<%$ resources:Stock%>" AssociatedControlID="txtStock"
                                                                class="lbl-17perc"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtStock" CssClass="input-small numeric input-disabled"
                                                                MaxLength="11" Enabled="false"></asp:TextBox>

                                                            <asp:HiddenField ID="hdfSlNo" runat="server" Value="0" />
                                                        </div>
                                                        <div class="div2col-S" runat="server" id="divProduct">
                                                            <label for="txtBincard" class="lbl-17-5perc">
                                                                <asp:Literal ID="ltBincard" runat="server" Text="<%$ Resources:Bincard %>"></asp:Literal>
                                                            </label>
                                                            <asp:TextBox runat="server" ID="txtIssueBinCard" TabIndex="22" ClientIDMode="Static"
                                                                CssClass="lbl-25-7perc" onkeydown="return Search(event);"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBincardPK" runat="server" Value="0" />
                                                            <asp:RequiredFieldValidator ID="vrfIssueBinCard" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="item" EnableClientScript="true" runat="server" ControlToValidate="txtIssueBinCard"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ Resources:Msg_SelectBinCard %>"
                                                                ToolTip="<%$ Resources:Msg_SelectBinCard %>" InitialValue="<%$Resources:Constants,AutoCompleteSelect %>"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="vrfIssueBinCard1" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="item" EnableClientScript="true" runat="server" ControlToValidate="txtIssueBinCard"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ Resources:Msg_SelectBinCard %>"
                                                                ToolTip="<%$ Resources:Msg_SelectBinCard %>"></asp:RequiredFieldValidator>
                                                            <asp:HiddenField runat="server" ID="hdfBinCard" Value="0" />
                                                            <asp:HiddenField runat="server" ID="hdfEnableAccept" Value="0" />
                                                            <asp:ImageButton ID="imbAddBin" runat="server" SkinID="imbaddnew" OnClick="ActionHandler"
                                                                OnClientClick="javascript:return ValidatePageNow('item');" CommandName="ADDBINCARD" TabIndex="23" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S" runat="server" id="divMaterial2">
                                                            <asp:Label ID="lblForQty" runat="server" Text="<%$ resources:Qty%>" AssociatedControlID="txtQty"
                                                                class="middle-lbl-xsmall-i"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtQty" CssClass="input-small numeric" MaxLength="16"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="vrfQty" runat="server" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="item" EnableClientScript="true" ControlToValidate="txtQty" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_Qty %>"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="crfQty" CssClass="star" SetFocusOnError="true"
                                                                Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="item"
                                                                EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtQty"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                            </asp:CompareValidator>
                                                            <div id="div1" runat="server" class="display-inline">
                                                                <asp:Label ID="lblForUOM" runat="server" Text='<%$ Resources:UOM %>' AssociatedControlID="txtUOM"
                                                                    CssClass="middle-lbl"></asp:Label>
                                                                <asp:TextBox ID="txtUOM" runat="server" CssClass="select-small-c" Enabled="false"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfUOMPK" runat="server" Value="0" />
                                                            </div>
                                                            <div class="display-inline">
                                                                <asp:ImageButton ID="imbAdd" runat="server" OnClick="ActionHandler" CommandName="ADDITEM"
                                                                    ValidationGroup="item" OnClientClick="javascript:ValidatePageNow('item')"
                                                                    SkinID="imbaddnew" Style="margin-top: 2px;" />
                                                                <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                                    ToolTip="<%$resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEARADD"
                                                                    SkinID="clear-ext" Style="margin-top: 2px!important; margin-bottom: 0px!important;" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="gridwrap">
                                                <asp:GridView runat="server" ID="grdItems" Width="100%" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler" ShowFooter="true"
                                                    OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                                <asp:HiddenField runat="server" ID="hdfDetailSlNo" Value='<%# Eval("SlNo") %>' />
                                                                <asp:HiddenField runat="server" ID="hdfIsPackedBin" Value='<%# Eval("CDR_IS_PACKED_BIN") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Batch %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrdBatch" runat="server" Text='<%# Eval("Batch") %>' ToolTip='<%# Eval("Batch") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfGrdBatchPK" Value='<%# Eval("BatchPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="45%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrdQty" runat="server" Text='<%#String.Format("{0:#.0000}", Eval("Quantity")) %>' ToolTip='<%# Eval("Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrdUOM" runat="server" Text='<%# Eval("UOM") %>' ToolTip='<%# Eval("UOM") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfGrdUOMPK" Value='<%# Eval("UOMPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <%--<HeaderTemplate>
                                                                <asp:Button ID="lnkGrdRemoveHeader" runat="server" CommandName="REMOVEITEMALL" SkinID="delete-icon"
                                                                    ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" Style="margin-top: 0px;" />
                                                            </HeaderTemplate>--%>
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkGrdRemove" runat="server" CommandName="DELETEITEM" SkinID="delete-icon"
                                                                    ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" Style="margin-top: 0px;" />
                                                                <asp:ImageButton runat="server" ID="imbEditDetails" SkinID="imbeditgrid" CommandName="EDITITEM"
                                                                    ToolTip="<%$resources:Controls,Edit %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView runat="server" ID="grdProduct" Width="100%" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler" ShowFooter="true"
                                                    OnRowDataBound="ActionHandler">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                                <asp:HiddenField runat="server" ID="hdfDetailSlNo" Value='<%# Eval("SlNo") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Bincard %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrdBincard" runat="server" Text='<%# Eval("Bincard") %>' ToolTip='<%# Eval("Bincard") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfGrdBincardPK" Value='<%# Eval("BincardPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="45%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Qty_Kg %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProdQty" runat="server" Text='<%# GetFormattedWeightwithComma(Eval("Quantity")) %>'
                                                                    ToolTip='<%# GetFormattedWeightwithComma(Eval("Quantity")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:TotalPcs %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProdPcs" runat="server" Text='<%# Eval("TotalPcs") %>' ToolTip='<%# Eval("TotalPcs") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkProdRemove" runat="server" CommandName="DELETEITEM" SkinID="delete-icon"
                                                                    ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" Style="margin-top: 0px;" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                <asp:ValidationSummary ID="vsPage" ValidationGroup="invoice" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsItem" ValidationGroup="item" runat="server" />
            </div>
            <asp:HiddenField ID="hdfDelstatus" runat="server" Value="0" />
            <asp:HiddenField ID="hdfConfirmAutoAllocate" runat="server" Value="0" />
            <asp:HiddenField ID="hdfConfirmDeleteAllocation" runat="server" Value="0" />
            <asp:HiddenField ID="hdfButtonID" runat="server" Value="" />
            <asp:HiddenField ID="hdfConfirmMessage" runat="server" Value="" />
            <asp:HiddenField ID="hdfBinPageSize" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsQaPassedBinsOnly" runat="server" Value="0" />

            <asp:HiddenField ID="hdfSelectedBatch" runat="server" Value="" />
            <asp:HiddenField ID="hdfSelectedBatchPK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAllocationExist" runat="server" Value="0" />
            <asp:HiddenField ID="hdfHasAllocation" runat="server" Value="0" />

            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="invoice">
                </uc1:workflowusercomments>
            </div>
            <asp:ImageButton ID="imgAddPlusDummy" runat="server" OnClick="ActionHandler" CommandName="ADD"
                ValidationGroup="AddToList" SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgAutoModeDummy" runat="server" OnClick="ActionHandler" CommandName="AUTOALLOCATECARTON"
                SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgShowPopupDummy" runat="server" OnClick="ActionHandler" CommandName="TAXPOPUPDISPLAY"
                SkinID="imbaddnew" Style="display: none;" />
            <asp:ImageButton ID="imgbtnDummyRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                Style="display: none;" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
