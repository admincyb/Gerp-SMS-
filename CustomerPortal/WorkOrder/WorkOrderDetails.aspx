<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="WorkOrderDetails.aspx.cs"
    Inherits="CustomerPortal.WorkOrder.WorkOrderDetails" Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        $(document).ready(function () {
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function InitComponents() {
            DateInit();
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            ShowHideAttachments();
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubContractor", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=7", "hdfSubContractor", true, true, "");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubContr", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=7", "hdfSubContr", true, true, "");
            GrandScriptUtils.MakeAutoComplete("txtWOItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=1&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT", "hdfWOItem", true, false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtWONo", url, "hdfWONo", true, true, 4, "WONUMBER");
            FillMaterialAutoComplete(0);
            //GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSubContr", url, "hdfSubContr", true, true, 4, "");
            FillBOMMaterialCategory();
            FillBOMMaterial();

            $("[id*=txtWORate]").ForceNumericOnly();
            $("[id*=txtWOQty]").ForceNumericOnly();
            $("[id*=txtBOMQty]").ForceNumericOnly();
            $("[id*=txtPriceAdj]").ForceNumericOnly();
        }

        function InitCustomerBrands() {
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, 0, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
        }
        function InitProducts() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemProduct", url + "?Type=1&BinSubType=" + $("[id$=ddl_matcat]").val(), "hdfProductPK", true, true, "GETPRODUCTS");
        }
        function InitBrands() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            //alert( uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val());
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

        function ShowHideStoreDtl(flag) {
            //<summary>Function Used to Show Store details Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgStoreDtlHide").show();
                $("#imgStoreDtlShow").hide();
                $("#divStoreDtl").show();
            }
            else {
                $("#imgStoreDtlHide").hide();
                $("#imgStoreDtlShow").show();
                $("#divStoreDtl").hide();
            }
        }

        function ShowHideWODtl(flag) {
            //<summary>Function Used to Show WO details Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgWODtlHide").show();
                $("#imgWODtlShow").hide();
                $("#divWODtl").show();
            }
            else {
                $("#imgWODtlHide").hide();
                $("#imgWODtlShow").show();
                $("#divWODtl").hide();
            }
        }

        function ShowHideBOM(flag) {
            //<summary>Function Used to Show BOM Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgBOMHide").show();
                $("#imgBOMShow").hide();
                $("#divBOM").show();
            }
            else {
                $("#imgBOMHide").hide();
                $("#imgBOMShow").show();
                $("#divBOM").hide();
            }
        }

        function ShowHideAttachments(flag) {
            //<summary>Function Used to Show BOM Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgAttachHide").show();
                $("#imgAttachShow").hide();
                $("#divAttachments").show();
            }
            else {
                $("#imgAttachHide").hide();
                $("#imgAttachShow").show();
                $("#divAttachments").hide();
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                DisableAuto($("[id$=txtWODate]"), false);
                DisableAuto($("[id$=txtSubContractor]"), $("[id$=hdfSubContractor]"));
                DisableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
                DisableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
                DisableAuto($("[id$=txtBOMItem]"), $("[id$=hdfBOMItem]"));
            }
            //else if (mode == 2) {
            //    EnableAuto($("[id$=txtWODate]"), false);
            //    EnableAuto($("[id$=txtSubContractor]"), $("[id$=hdfSubContractor]"));
            //    EnableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
            //    EnableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
            //    EnableAuto($("[id$=txtBOMItem]"), $("[id$=hdfBOMItem]"));
            //}
            else {
                EnableAuto($("[id$=txtWODate]"), false);
                EnableAuto($("[id$=txtSubContractor]"), $("[id$=hdfSubContractor]"));
                EnableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
                EnableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
                EnableAuto($("[id$=txtBOMItem]"), $("[id$=hdfBOMItem]"));
            }
        }

        function ShowDeleteConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgDeleteConfirm %>';
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
                        return false;
                    }
                }
            });
            return false;
        }

        function DateInit() {
            //<summary>function used to make datepicker</summary>
            GrandScriptUtils.DatePicker("txtWODate", false, false);
            GrandScriptUtils.DatePicker("txtWODtlDate", false, false);
        }

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

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
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
        function InitWOItem() {
            //var selectItem = $("[id$=ddlWOItemType]").val();
            var selectItem = $("[id$=ddlItemType]").val();
            $("[id$=ddlWOItemType]").val(selectItem);
            switch (selectItem) {
                case "1":
                    FillMaterialAutoComplete(0);
                    break;
                case "2":
                    FillMaterialAutoComplete(0);
                    break;
                case "3":
                    FillMaterialAutoComplete(0);
                    break;
            }
        }
        //function InitItemType() {
        //    var selectItem = $("[id$=ddlItemType]").val();
        //    $("[id$=ddlWOItemType]").val(selectItem);
        //    switch (selectItem) {
        //        case "1":
        //            FillMaterialAutoComplete(0);
        //            break;
        //        case "2":
        //            FillMaterialAutoComplete(0);
        //            break;
        //        case "3":
        //            FillMaterialAutoComplete(0);
        //            break;
        //    }
        //}
        function FillMaterialAutoComplete(SelectVal) {
            //<summary> Function Used to make Item field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtWorkOrderItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ddlWOItemType]").val() + "&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT&CustomerPK=" + $("[id$=hdfCustomerID]").val(), "hdfWorkOrderItem", true, false, false, true);
            //if ($("[id$=hdfSelectMaterialPK]").val() != '0' && SelectVal == '1') {
            //    $("[id$=MaterialPK]").val($("[id$=hdfSelectMaterialPK]").val());
            //    $("[id$=ItemCodeMaterial]").val($("[id$=hdfSelectMaterial]").val());

            //}
        }
        function FillMaterialDetails(materialID) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&VendorPK=" + $("[id$=hdfSubContractor]").val(), function (data) {
                if (data) {
                    if (materialID != 0) {
                        $("[id$=txtUOM]").val(data[0].UOM_CODE);
                        $("[id$=hdfItemUOM]").val(data[0].ITM_UOM);
                        $("[id$=txtWORate]").val(data[0].ITV_PRICE);
                    }
                }
                else {
                    $("[id$=txtUOM]").val("");
                    $("[id$=hdfItemUOM]").val(0);
                    $("[id$=txtWORate]").val("");
                }
            });
        }

        function FillBOMItemDetails(materialID) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&VendorPK=" + $("[id$=hdfSubContractor]").val(), function (data) {
                if (data) {
                    if (materialID != 0) {
                        $("[id$=txtBOMUOM]").val(data[0].UOM_CODE);
                        $("[id$=hdfBOMUOM]").val(data[0].ITM_UOM);
                    }
                }
                else {
                    $("[id$=txtBOMUOM]").val("");
                    $("[id$=hdfBOMUOM]").val(0);
                }
            });
        }

        function AfterAutoCompleteSelect(targetControlID) {
            //<summary> Function Used to an event fire after select category then fill material and uom </summary>
            if (targetControlID == "txtWorkOrderItem") {
                FillMaterialDetails($("[id$=hdfWorkOrderItem]").val());
                $("[id$=hdfSubContractorItem]").val($("[id$=hdfWorkOrderItem]").val());
            }
            if (targetControlID == "txtSubContractor") {
                if (parseInt($("[id$=hdfSubContractorItem]").val()) > 0) {
                    FillMaterialDetails($("[id$=hdfSubContractorItem]").val());
                }
            }
            if (targetControlID == "txtBOMItem") {
                FillBOMItemDetails($("[id$=hdfBOMItem]").val());
            }
        }

        function CalculateAmount() {
            var rate = parseFloat($("[id$=txtWORate]").val());
            var qty = parseFloat($("[id$=txtWOQty]").val());
            var amt = rate * qty;
            if (!isNaN(amt)) {
                $("[id$=txtWOAmt]").val(amt.toFixed(CurrencyDigits));
            }
            else {
                amt = 0;
                $("[id$=txtWOAmt]").val(amt.toFixed(CurrencyDigits));
            }
        }

        function FillBOMMaterialCategory() {
            //<summary> Function Used to make material category field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtBOMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=1", "hdfBOMCategory", true, false, "BizUnitPk", true);
        }
        function FillBOMMaterial() {
            //<summary> Function Used to make Item field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtBOMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=1" + "&FLDNAME=ITM_TEXT", "hdfBOMItem", true, false, "MaterialCategoryPK", true);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlWorkOrder">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPnl" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="31" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('wo')" ValidationGroup="wo"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPnl" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="13" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" ValidationGroup="Employee" OnClientClick="javascript:ValidatePageNow('wo')"
                                            CommandArgument="SEC_ActionPnl" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="14" CommandArgument="SEC_ActionPnl" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="15" CommandArgument="SEC_ActionPnl" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="69" ID="btnPrintWO" CommandName="PRINTWO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPnl" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="16" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPnl" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="19" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPnl" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" Visible="false" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <%--------------colpase btn start----------%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1><%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
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
                            <%--------------colpase btn end----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="100" CssClass="input-small"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-21-1perc"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="101" CssClass="input-small"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblWOItem" runat="server" Text="<%$resources:WOItem %>" AssociatedControlID="txtWOItem"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtWOItem" runat="server" TabIndex="100" CssClass="input-small"
                                                onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfWOItem" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSubContr" runat="server" Text="<%$resources:SubContractor %>" AssociatedControlID="txtSubContr"></asp:Label>
                                            <asp:TextBox ID="txtSubContr" runat="server" TabIndex="100" CssClass="select-half-b margnbotm0"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSubContr" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblWONo" runat="server" Text="<%$resources:WONo %>" AssociatedControlID="txtWONo"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtWONo" runat="server" TabIndex="100" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfWONo" runat="server" Value="0" />

                                            <asp:Label ID="lblStatus" runat="server" Text="<%$resources:Status %>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2" TabIndex="10">
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                                    Selected="True">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                                                </asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>

                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                TabIndex="11" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                OnClick="ActionHandler" CommandName="SEARCH" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="12" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdWorkOrder" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" TabIndex="23" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:WODate %>" SortExpression="<%$ resources:DataFieldRes,EmployeeCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODate" runat="server" ToolTip='<%# Eval("WIH_DATE", Resources.Constants.DateFormatGridExpanded)%>'
                                                    Text='<%# Eval("WIH_DATE", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WONo %>" SortExpression="WIH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONumber" runat="server" ToolTip='<%# Eval("WIH_NO") %>'
                                                    Text='<%# Eval("WIH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RefNo %>" SortExpression="WIH_REF_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWORefNumber" runat="server" ToolTip='<%# Eval("WIH_REF_NO") %>'
                                                    Text='<%# Eval("WIH_REF_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SubContractor %>" SortExpression="WIH_VENDOR_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOSubContractor" runat="server" ToolTip='<%# Eval("WIH_VENDOR_TEXT") %>'
                                                    Text='<%# Eval("WIH_VENDOR_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WOItems %>" SortExpression="WIH_ITEM_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOItems" runat="server" ToolTip='<%# Eval("WIH_ITEM_TEXT") %>'
                                                    Text='<%# Eval("WIH_ITEM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="WIH_TOTAL_QTY" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" ToolTip='<%# Eval("WIH_TOTAL_QTY") %>'
                                                    Text='<%# Eval("WIH_TOTAL_QTY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="WIH_NET_AMOUNT" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" ToolTip='<%# Eval("WIH_NET_AMOUNT") %>'
                                                    Text='<%# Eval("WIH_NET_AMOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ErpRes,Status  %>" SortExpression="<%$ resources:ErpRes,Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# Eval("WIH_STATUS_TEXT") %>'
                                                    Text='<%# Eval("WIH_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Issued %>" Visible="false">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Issued_Received %>" Visible="false">
                                            <ItemTemplate>
                                                <image id="imgIssued" title=""
                                                    class='<%# (Eval("WIH_STATUS")).ToString() == "1" ? "active" : "inactive" %>'
                                                    alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <image id="imgReceived" title="" class="" alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" OnClick="ActionHandler"
                                                    TabIndex="10" SkinID="imbeditgrid" CommandName="EDITITEM" CommandArgument='<%# Eval("WIH_PK") %>' />
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" OnClick="ActionHandler"
                                                    TabIndex="10" SkinID="btnview" CommandName="VIEW" CommandArgument='<%# Eval("WIH_PK") %>' />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                                    SkinID="btnPrint" CommandName="PRINT" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfWOStatus" Value='<%# Eval("WIH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <div>
                                    <div id="tab1Content">
                                        <div id="divWorkOrder">
                                            <div>
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lbl_WorkOrderNo" runat="server" AssociatedControlID="lblWorkOrderNo" Text='<%$ Resources:WONo%>'></asp:Label>
                                                                <asp:Label ID="lblWorkOrderNo" runat="server" Text="" CssClass="input-small-a margnrgt0-8per"></asp:Label>
                                                                <asp:Label ID="lblWODate" runat="server" Text='<%$ Resources:WODate%>'
                                                                    CssClass="lbl-19-2perc" AssociatedControlID="txtWODate"></asp:Label>
                                                                <asp:TextBox ID="txtWODate" runat="server" TabIndex="10" onkeydown="return CheckKey(event)"
                                                                    onpaste="return false;" CssClass="input-small"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="wo" EnableClientScript="true" runat="server" ControlToValidate="txtWODate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                                </asp:RequiredFieldValidator>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblRefNo" runat="server" Text='<%$ Resources:Controls,RefNo%>'
                                                                    AssociatedControlID="txtRefNo"></asp:Label>
                                                                <asp:TextBox ID="txtRefNo" runat="server" MaxLength="50" TabIndex="14" CssClass="input-half"></asp:TextBox>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblSubContractor" runat="server" Text='<%$resources:SubContractor %>'
                                                                    AssociatedControlID="txtSubContractor"></asp:Label>
                                                                <asp:TextBox ID="txtSubContractor" runat="server" TabIndex="14" CssClass="select-half"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfSubContractor" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdfSubContractorItem" runat="server" Value="0" />

                                                                <asp:RequiredFieldValidator ID="vrfSubContractor" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="wo" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                    runat="server" ControlToValidate="txtSubContractor" Display="Dynamic" Text="*"
                                                                    ErrorMessage="<%$ resources:Err_SubContractor %>"></asp:RequiredFieldValidator>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblItemType" runat="server" Text="<%$ resources:ItemType %>"
                                                                    AssociatedControlID="ddlItemType"></asp:Label>
                                                                <asp:DropDownList ID="ddlItemType" runat="server" onchange="javascript:InitWOItem();" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ActionHandler" TabIndex="20" CssClass="select-half-a">
                                                                    <%--<asp:ListItem Text="<%$ resources:Material %>" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ resources:Product %>" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ resources:Brand %>" Value="3"></asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trCustomer" visible="false">
                                                        <td>
                                                            <div class="div2col-S">
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer %>"
                                                                    AssociatedControlID="txtCustomer"></asp:Label>
                                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfCustomerID" runat="server" Value="0" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab2Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.StoreDetails%>
                                            <img id="imgStoreDtlShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideStoreDtl(1);" />
                                            <img id="imgStoreDtlHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideStoreDtl();" />
                                        </h1>
                                        <div id="divStoreDtl">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblLocation" runat="server" Text='<%$Resources:Controls,Location %>'
                                                                AssociatedControlID="ddlLocation"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocation" runat="server" TabIndex="10" CssClass="select-half"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="vrfLocation" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="wo" EnableClientScript="true" InitialValue="-1"
                                                                runat="server" ControlToValidate="ddlLocation" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_Location %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblDeliveryTo" runat="server" Text='<%$Resources:Controls,DeliveryTo %>'
                                                                AssociatedControlID="ddlDeliveryTo"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeliveryTo" runat="server" CssClass="select-half"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="vrfDeliveryTo" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="wo" EnableClientScript="true" InitialValue="-1"
                                                                runat="server" ControlToValidate="ddlDeliveryTo" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_DeliveryTo %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="tab3Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.WODetails%>
                                            <img id="imgWODtlShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideWODtl(1);" />
                                            <img id="imgWODtlHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideWODtl();" />
                                        </h1>
                                        <div id="divWODtl">
                                            <table class="gridwraptable">
                                                <tr>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("Operation").ToString()%></th>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("ItemType").ToString()%></th>
                                                    <th align="left" style="width: 27%"><%=GetLocalResourceObject("WOItem").ToString()%></th>
                                                    <th align="left" style="width: 6%"><%=Resources.Controls.UOM %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%"><%=Resources.Controls.Rate %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%"><%=Resources.Controls.Qty %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%"><%=GetLocalResourceObject("Amount").ToString()%></th>
                                                    <th align="left" style="width: 2%"><%=Resources.Controls.ReqDate %></th>
                                                    <th align="left" style="width: 15%"><%=Resources.Controls.Remark %></th>
                                                    <th style="width: 2%"></th>
                                                </tr>
                                                <tr class="grd-rowhead">
                                                    <td>
                                                        <asp:DropDownList ID="ddlOperOrWork" runat="server" TabIndex="10" Width="90%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfOperOrWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlOperOrWork" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_OperWork %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlWOItemType" runat="server" TabIndex="20" Width="100%" Enabled="false">
                                                            <%--<asp:ListItem Text="<%$ resources:Material %>" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ resources:Product %>" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ resources:Brand %>" Value="3"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td runat="server" id="tdWOItem">
                                                        <asp:TextBox ID="txtWorkOrderItem" runat="server" TabIndex="20" Width="90%">
                                                        </asp:TextBox>
                                                        <asp:HiddenField ID="hdfWorkOrderItem" runat="server" Value="0" />
                                                        <asp:RequiredFieldValidator ID="vrfWorkOrderItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtWorkOrderItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_WOItem %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td runat="server" id="tdProduct" visible="false">
                                                        <asp:DropDownList runat="server" ID="ddlProduct" TabIndex="2" Width="90%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td runat="server" id="tdBrand" visible="false">
                                                        <asp:TextBox runat="server" ID="txtBrand" TabIndex="2" Width="90%">
                                                        </asp:TextBox>
                                                        <asp:HiddenField ID="hdfBrand" runat="server" Value="0"></asp:HiddenField>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUOM" runat="server" CssClass="input-disabled" Enabled="false" Width="90%"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfItemUOM" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWORate" runat="server" CssClass="numeric" Width="80%"
                                                            onkeyup="javascript:CalculateAmount();"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfWORate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" runat="server" ControlToValidate="txtWORate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWOQty" runat="server" CssClass="numeric" Width="80%"
                                                            onkeyup="javascript:CalculateAmount();"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfWOQty" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" runat="server" ControlToValidate="txtWOQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWOAmt" runat="server" CssClass="numeric input-disabled"
                                                            Enabled="false" Width="90%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODtlDate" runat="server" onkeydown="return CheckKey(event)"
                                                            onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODtlRemarks" CssClass="txt-left" runat="server"
                                                            MaxLength="30" TabIndex="22" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDWORKORDERDETAIL" TabIndex="28"
                                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('woDtl')"
                                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="woDtl"
                                                            SkinID="plus" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="gridwrap scroll-container">
                                                <asp:GridView ID="grdWODetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                                    Width="100%" ShowFooter="true">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Operation %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlOperOrWork" runat="server" Text='<%#Eval("Operation") %>'
                                                                    ToolTip='<%#Eval("Operation") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfWOdtlOperOrWorkPK" Value='<%#Eval("OperationPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WONo %>" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlNo" runat="server" Text='<%#Eval("WorkOrderNo") %>'
                                                                    ToolTip='<%#Eval("WorkOrderNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ItemType %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlItemType" runat="server" Text='<%#Eval("ItemType") %>'
                                                                    ToolTip='<%#Eval("ItemType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WOItem %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlItem" runat="server" Text='<%#Eval("Item") %>'
                                                                    ToolTip='<%#Eval("Item") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfWODtlItemPK" runat="server" Value='<%#Eval("ItemPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UOM%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlUOM" runat="server" Text='<%#Eval("UOM") %>'
                                                                    ToolTip='<%#Eval("UOM") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfWODtlUOMPK" runat="server" Value='<%#Eval("UOMPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="6%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Rate %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlRate" runat="server" CssClass="ItemQuantity" Text='<%#Eval("Rate") %>'
                                                                    ToolTip='<%#Eval("Rate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Qty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlQty" runat="server" CssClass="ItemQuantity" Text='<%#Eval("Quantity") %>'
                                                                    ToolTip='<%#Eval("Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlAmt" runat="server" Text='<%#Eval("Amount") %>'
                                                                    ToolTip='<%#Eval("Amount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,ReqDate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlReqDate" runat="server" Text='<%# Eval("RequiredDate", Resources.Constants.DateFormatGridExpanded)%>'
                                                                    ToolTip='<%# Eval("RequiredDate", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Remark %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlRemark" runat="server" Text='<%#Eval("Remarks") %>'
                                                                    ToolTip='<%#Eval("Remarks") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnWODtlRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEWORKORDERDETAIL"
                                                                    CommandArgument='<%# Eval("SlNo") %>' OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="31" OnPreRender="btnAction_PreRender" />
                                                                <asp:HiddenField ID="hdfWODtlSlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab4Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.BillOfMaterials%>
                                            <img id="imgBOMShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideBOM(1);" />
                                            <img id="imgBOMHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideBOM();" />
                                        </h1>
                                        <div id="divBOM">
                                            <table class="gridwraptable">
                                                <tr>
                                                    <th align="left" style="width: 10%"><%=GetLocalResourceObject("Operation").ToString() %></th>
                                                    <th align="left" style="width: 20%"><%=GetLocalResourceObject("WOItem").ToString() %></th>
                                                    <th align="left" style="width: 10%"><%=Resources.Controls.Type %></th>
                                                    <th align="left" style="width: 20%"><%=Resources.Controls.Category %></th>
                                                    <th align="left" style="width: 20%"><%=Resources.Controls.Item %></th>
                                                    <th align="left" style="width: 9%"><%=Resources.Controls.UOM %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%"><%=Resources.Controls.Qty %></th>
                                                    <th style="width: 2%"></th>
                                                </tr>
                                                <tr class="grd-rowhead">
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMOperWork" runat="server" TabIndex="20" Width="90%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMOperWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMOperWork" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_OperWork %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMWOItem" runat="server" TabIndex="20" Width="90%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMWOItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMWOItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_WOItem %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMType" runat="server" TabIndex="20" Width="90%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMType" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMType" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Type %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMCategory" runat="server" Width="90%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMCategory" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMCategory" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Category %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdfBOMCategory" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMItem" runat="server" Width="90%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Item %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdfBOMItem" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMUOM" runat="server" CssClass="input-disabled" Enabled="false" Width="90%"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBOMUOM" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMQty" runat="server" CssClass="numeric" Width="80%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMQty" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" runat="server" ControlToValidate="txtBOMQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="imbBOMAdd" CommandName="ADDBOM" TabIndex="28"
                                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('bom');"
                                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="scDetails"
                                                            SkinID="plus" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="gridwrap scroll-container">
                                                <asp:GridView ID="grdBOM" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                                    Width="100%" ShowFooter="true">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Operation %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMOperOrWork" runat="server" Text='<%# Eval("Operation") %>'
                                                                    ToolTip='<%# Eval("Operation") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMOperOrWork" runat="server" Value='<%# Eval("OperationPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WOItem %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlWOItem" runat="server" Text='<%# Eval("Item") %>'
                                                                    ToolTip='<%# Eval("Item") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlWOItem" runat="server" Value='<%# Eval("ItemPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="39%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ChooseType %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlType" runat="server" Text='<%# Eval("BOMType") %>'
                                                                    ToolTip='<%# Eval("BOMType") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlType" runat="server" Value='<%# Eval("BOMTypePK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Category %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlCategory" runat="server" Text='<%# Eval("Category") %>'
                                                                    ToolTip='<%# Eval("Category") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlCategory" runat="server" Value='<%# Eval("CategoryPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Item %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlItem" runat="server" Text='<%# Eval("Material") %>'
                                                                    ToolTip='<%# Eval("Material") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlItem" runat="server" Value='<%# Eval("MaterialPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UOM%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlUOM" runat="server" Text='<%# Eval("UOM") %>'
                                                                    ToolTip='<%# Eval("UOM") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlUOM" runat="server" Value='<%# Eval("UOMPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Qty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlQty" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedRate(Eval("Quantity")) %>'
                                                                    ToolTip='<%#GetFormattedRate(Eval("Quantity")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,ActualQty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="lblBOMDtlActualQty" runat="server" CssClass="numeric"
                                                                    Text='<%#GetFormattedRate(Eval("Quantity")) %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Remarks %>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBOMDtlRemarks" runat="server" Text="" Height="15px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnBOMDtlRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEBOMITEM"
                                                                    CommandArgument='<%# Eval("MaterialPK") %>' OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="31" OnPreRender="btnAction_PreRender" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div id="divCalc">
                                                <div class="gridwrap">
                                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label runat="server" ID="lblSubTotal" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtSubTotal"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:TextBox ID="txtSubTotal" runat="server" CssClass="input-w97 numeric input-disabled" onchange="CalculateTotal(this);"
                                                                    MaxLength="16" TabIndex="35" Enabled="false"></asp:TextBox>
                                                                <div class="starwrap">
                                                                    <cc1:AmountValidation ID="vamSubTotal" runat="server" ControlToValidate="txtSubTotal"
                                                                        ErrorMessage="<%$ resources:Err_Valid_SubTotal %>" NumberDigits="12" AllowNegative="true"
                                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                                </div>
                                                                <div class="clear">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 84%; text-align: right">
                                                                <div class="floatLeft">
                                                                </div>
                                                                <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right" class="btn-margin">
                                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                                    TabIndex="32" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                                <div class="clear">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <div class="floatLeft" style="display: none;">
                                                                    <asp:Label ID="lblHGrossWt" Font-Bold="true" runat="server" Text="<%$ resources:GrossWeight%>"
                                                                        AssociatedControlID="lblHGrossWt"></asp:Label>
                                                                    <asp:Label ID="lblGrossWeight" AssociatedControlID="lblGrossWeight" runat="server"></asp:Label>
                                                                </div>
                                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right" class="btn-margin">
                                                                <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                                    TabIndex="33" ToolTip="<%$ resources:ShippingCharge %>" CommandName="SHIPPINGHEADER" />
                                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w97 numeric  input-disabled"
                                                                    MaxLength="16" Enabled="false" TabIndex="25"></asp:TextBox>
                                                                <div class="starwrap">
                                                                    <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                                        ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                                </div>
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
                                                                    TabIndex="34" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w97 numeric" onchange="CalculateTotal(this);"
                                                                    MaxLength="16" TabIndex="35"></asp:TextBox>
                                                                <div class="starwrap">
                                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                                </div>
                                                                <div class="clear">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                                <div class="clear">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab5Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.Attachments%>
                                            <img id="imgAttachShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideAttachments(1);" />
                                            <img id="imgAttachHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideAttachments();" />
                                        </h1>
                                        <div id="divAttachments">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"></asp:Label>
                                                <div class="fileupload-main">
                                                    <asp:FileUpload ID="fupUpload" runat="server" TabIndex="63" CssClass="margn-rgt0 upload-area" />
                                                    <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_File_Upload%>">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <a id="anchorFile" runat="server" target="_blank" tabindex="55"></a>
                                                <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="64"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                                    CommandArgument="PageAction_Entry" ToolTip="<%$resources:ErpRes,Add%>" ValidationGroup="upload"
                                                    Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div class="gridwrap">
                                                <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$resources:UploadPageSize%>"
                                                    AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$resources:SlNo%>">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hdfPK" Value='<%#Eval("DOC_PK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$resources:File %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFile" runat="server" Text='<%#Eval("DOC_NAME") %>' ToolTip='<%#Eval("DOC_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="92%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <a runat="server" id="fileView" class="download-icon nomargin" style="margin-right: -1px!important;"
                                                                    title="<%$resources:View %>" target="_blank" href='<%#Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                                    SkinID="edit-icon" ToolTip="Edit" Style="margin-right: 3px!important;" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                                    SkinID="delete-icon" ToolTip="Delete" OnClientClick="return ShowDeleteConfirmationMsg(this);" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divData">
                            </div>

                            <div id="divWkfSubmit" style="display: none;">
                                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="wo" />
                            </div>

                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" TabIndex="59" />
                                </div>
                                <div class="content-wrapper">
                                    <%-- *********************Checkbox Region Start*******************************************************************--%>
                                    <div id="divTaxApplicableAmount" style="display: none" runat="server">
                                        <label for="chkSubTotal" style="width: 148px">
                                            <%=Resources.Controls.SubTotal%>
                                        </label>
                                        <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkDiscount" style="width: 120px">
                                            <%=Resources.Controls.Discounts%>
                                        </label>
                                        <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkOtherCharges" style="width: 125px">
                                            <%=Resources.Controls.OtherCharges%>
                                        </label>
                                        <asp:CheckBox ID="chkOtherCharges" runat="server" Checked="true" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                    </div>
                                    <%--   *******************End Check Box Region ****************************************************************--%>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:Amount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label><asp:TextBox ID="txtPopupItemAmount"
                                                            CssClass="input-w70 numeric" runat="server" EnableViewState="false" Enabled="false"
                                                            MaxLength="11"></asp:TextBox><div class="clear">
                                                            </div>
                                                    <div runat="server" id="dvPerc">
                                                        <asp:Label ID="lblPerc" runat="server" Text="<%$ resources:DiscPerc %>" AssociatedControlID="txtTaxPerc"></asp:Label><asp:TextBox
                                                            ID="txtTaxPerc" TabIndex="55" runat="server" CssClass="input-w70 numeric" EnableViewState="false"
                                                            MaxLength="15" onChange="CalcTax();"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Charge %>" AssociatedControlID="txtPopupAmount"></asp:Label><asp:TextBox
                                                        ID="txtPopupAmount" TabIndex="55" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox><div class="starwrap">
                                                            <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                            </asp:RequiredFieldValidator><cc1:AmountValidation ID="vamTaxAmt" runat="server"
                                                                ControlToValidate="txtPopupAmount" ErrorMessage="<%$ resources:Err_Amount_Valid %>"
                                                                NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                ValidationGroup="tax"></cc1:AmountValidation>
                                                        </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="54" runat="server" CssClass="medium" EnableViewState="true"
                                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label><asp:TextBox
                                                        ID="txtPopupOther" runat="server" TabIndex="56" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox><asp:RequiredFieldValidator ID="vrfPopupOther"
                                                            CssClass="star" SetFocusOnError="true" ValidationGroup="tax" EnableClientScript="true"
                                                            runat="server" ControlToValidate="txtPopupOther" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Name %>">
                                                        </asp:RequiredFieldValidator><asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew"
                                                            runat="server" OnClick="ActionHandler" CommandArgument="PageAction_Entry" ValidationGroup="tax"
                                                            CommandName="TAXADD" TabIndex="57" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("TaxName") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName"))) %>'></asp:Label><asp:HiddenField
                                                            ID="hdfTaxName" runat="server" Value='<%#Eval("TaxName") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("TaxHeaderPK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("TaxPK") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TaxName"))) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode( HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TaxName")))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscPer" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("DiscountPercentage")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("DiscountPercentage")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("TaxAmount")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("TaxAmount")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            TabIndex="58" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                            SkinID="btnclose" ToolTip="Remove" /><%--OnLoad="btnAction_Load"--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <div id="diverror" style="display: none">
                                <%--Use this label to bind the server errors--%>
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>

                                <asp:ValidationSummary ID="vsPage" ValidationGroup="wo" runat="server" />
                                <asp:ValidationSummary ID="vsDetails" ValidationGroup="woDtl" runat="server" />
                                <asp:ValidationSummary ID="vsBOM" ValidationGroup="bom" runat="server" />
                                <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                                <asp:ValidationSummary ID="vsSalesCost" ValidationGroup="scCost" runat="server" />
                                <asp:ValidationSummary ID="vsOrderQty" ValidationGroup="OrderQty" runat="server" />
                            </div>

                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                            <asp:HiddenField ID="hdfRateDecimalDigits" Value="3" runat="server" />
                            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfExchangeRate" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />

                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
