<%@ Page Title="<%$ Resources:Title_SaleOrderListing %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="SaleOrderListing.aspx.cs" Theme="ClassicExt"
    Inherits="CustomerPortal.Sales.SaleOrderListing" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            $("table[id*=grdOrderDetails] tr td:first-child,table[id*=grdOrderDetails] tr th:first-child").css({ "width": "3%" });
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //         GrandScriptUtils.AddDateRange("txtActFromDate", "hdfActFromDate", "txtActToDate", "hdfActToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "&IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true,4, "CUSTOMERLIST");

            if ($("[id$=hdfCustomerPK]").val() != "") {
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSONumber", url + "&CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSoPK", true, true,4, "SONUMBERWKF");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSONumber", url, "hdfSoPK", true, true, 4,"SONUMBERWKF");
            }

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCUSPONumber", url, "hdfCusPoNumber", true, true, 4, "CUSPONUMBER");

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtInvNo", url, "hdfInvNo", true, true, 4, "INVCONVERTED");


            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            HideFilter();
            if ($("[id$=hdfCustomerPK]").val() != "") {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
            }
            SetGridScroll();

            if ($("[id$=hdfDelStatusCurrent]").val() == 1 || $("[id$=hdfWrkfStatusCurrent]").val() == 0) {
                $("[id$=btnEditforCancel]").hide();
            } else {
                $("[id$=btnEditforCancel]").show();
            }
            if ($("[id$=hdfDelStatusCurrent]").val() == 1 || ($("[id$=hdfWrkfStatusCurrent]").val() != 2)) {
                $("[id$=btnAmend]").hide();
            } else {
                $("[id$=btnAmend]").show();
            }
        }


        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#ImbHideDODetails").hide();
            $("#ImbShowDODetails").show();
            $("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#ImbHideDODetails").show();
            $("#ImbShowDODetails").hide();
            $("#divFilterDetails").show();
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
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtSONumber") {
                //                $("[id$=btnSONumber]").click();
            }

            if (targetControlID == "txtCUSPONumber") {
            }
        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtSONumber") {
                $("[id$=hdfSoPK]").val("0");
                $("[id$=hdfSoPK]").val(0);
                var a = parseInt($("[id$=hdfSoPK]").val());
            }
            if (targetControlID == "txtCUSPONumber") {
                $("[id$=hdfCusPoNumber]").val("0");
                $("[id$=hdfCusPoNumber]").val(0);
                var a = parseInt($("[id$=hdfCusPoNumber]").val());
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
        //        $(".GridExpandCollapseButton").click(function () {
        //            $("[id$=hdfExpandPosition]").val($(this).attr("id"));
        //        });
        function AfterGridExpand(row) {
            if ($("[id$=grdDOHdr]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedDOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetDoDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
            if ($("[id$=grdSoList]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
            if ($("[id$=grdPackingHdr]").attr('id') == $(row).parent().parent().attr('id')) {
                $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                var hdf = $(row).find("[id*=hdfIsExpandedPackingItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetPackingDetails]").click();
                }
                else
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
            }
        }
        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
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
                ShowConfirmShortClose()
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function DisableAuto(extender) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }
        function ShowShortClose() {
            ShowContainerDiv('[id$=divShortClose]', '<%= GetGlobalResourceObject("Controls","ShortClose").ToString() %>', '450', '200');
        }

        //For Show Confirm Short Close
        function ShowConfirmShortClose() {

            var msgTitle;
            var msg;
            msgTitle = '<%= GetGlobalResourceObject("Controls","ShortClose").ToString() %>';
            msg = '<%= GetGlobalResourceObject("Messages","ConfirmcloseSC").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        $("[id$=btnShCloseSave]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        //Show confirmation for inactive brand exist
        function ShowInactiveBrandExist() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_InactiveItmExist").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfCopyYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnCopy]").click();
                    },
                    Cancel: function (e) {
                        $("[id$=hdfCopyYes]").val(0);
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="51" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Add%>" />
                                    </li>
                                    <li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="52" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="53" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelSC %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelSC %>" />
                                    </li>
                                    <li id="pnlAmend">
                                        <asp:Button runat="server" TabIndex="54" ID="btnAmend" CommandName="AMEND" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Amend %>" />
                                    </li>
                                    <li id="pnlCopy">
                                        <asp:Button runat="server" TabIndex="55" ID="btnCopy" CommandName="COPY" OnClick="ActionHandler"
                                            SkinID="btnInner-copy" Text="<%$Resources:Controls,Copy%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Copy%>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="56" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="pnlPrintSO" runat="server">
                                        <asp:Button runat="server" TabIndex="57" ID="btnPrintSO" CommandName="PRINTSO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="list-active">
                            <asp:LinkButton runat="server" ID="lbnSOListing" ToolTip="Listing" TabIndex="1" CssClass="list-active"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="SALEORDERLISTING"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbSODetails" Text="<%$resources:SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="SALEORDER"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <%--  <div class="clear">
                            </div>--%>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7" id="divtxtCusPoNumber" runat="server">
                                            <asp:Label ID="lblCUSPONumber" runat="server" Text="<%$resources:PONumber %>" AssociatedControlID="txtCUSPONumber"></asp:Label>
                                            <asp:TextBox ID="txtCUSPONumber" runat="server" MaxLength="100" TabIndex="1" CssClass="input-small margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCusPoNumber" runat="server" Value="" />
                                            <%--                                            <asp:Label ID="lblAgent" CssClass="middle-lbl-a" runat="server" Text="<%$resources:Agent %>" AssociatedControlID="txtAgent"></asp:Label>
                                            <asp:TextBox ID="txtAgent" runat="server" MaxLength="100" TabIndex="1" CssClass="input-small margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfAgent" runat="server" Value="" />--%>
                                            <asp:Label ID="lblInvNo" runat="server" Text="<%$resources:InvoiceNumber %>" AssociatedControlID="txtInvNo" CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox ID="txtInvNo" runat="server" MaxLength="100" TabIndex="1" CssClass="input-small margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfInvNo" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7" id="divCompany" runat="server">
                                            <asp:Label ID="lblCompanyFilter" runat="server" Text="<%$resources:Controls,CompanyPlant%>"
                                                AssociatedControlID="ddlCompanyFilter"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyFilter" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlVoucherCompany');"
                                                CssClass="select-small-a1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small margnrgt1-5per"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblWorkFlowStatus" runat="server" Text="<%$ resources:TransactionStatus %>"
                                                AssociatedControlID="ddlWrokFlowStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlWrokFlowStatus" runat="server" TabIndex="3" CssClass="select-small-c">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:SCStatus%>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-xsmall-c1"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b  valid"
                                                TabIndex="4">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                                <%-- <asp:ListItem Text="<%$ Resources:Captions,Completed %>" Value="2"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:HiddenField ID="hdfCustomerPK" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" MaxLength="100" TabIndex="6" CssClass="input-small-c margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                            <asp:Button ID="btnSONumber" runat="server" OnClick="ActionHandler" CommandName="SONUM"
                                                Style="display: none" EnableTheming="false" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:Type %>" AssociatedControlID="ddlInvoiceType"
                                                CssClass="middle-lbl-xsmall-b margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlInvoiceType" runat="server" TabIndex="7" CssClass="select-small-b margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="8" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="9" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap maxh-290" id="divSO_ScrollContainer" grid="grdSoList">
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" Width="100%"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    PageSize="<%$ resources:PageSize %>" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" Checked="false" TabIndex="14"
                                                    GroupName="SelectOne" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);"
                                                    OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval(Resources.DataFieldRes.SaleOrderDept) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSOStatus" Value='<%# Eval(Resources.DataFieldRes.SOStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSODELStatus" Value='<%# Eval(Resources.DataFieldRes.SODeleteStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfCmpPK" Value='<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_PK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfIsGlowPr" Value='<%# Eval("SOH_IS_GLOVE_PR") %>' />
                                                <asp:HiddenField runat="server" ID="hdfIsPackPr" Value='<%# Eval("SOH_IS_PACK_PR") %>' />
                                                 <asp:HiddenField runat="server" ID="hdfSOH_CONVERTED" Value='<%# Eval("SOH_CONVERTED") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) == "" ? ERP.Utilities.CommonFunctions.GetShortString(GetGlobalResourceObject("Messages","DocGenerationNew").ToString() + " - " + Eval(Resources.DataFieldRes.SOH_Reference),15) : Eval(Resources.DataFieldRes.SONo)%>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%#GetLocalResourceObject("CustPONo").ToString() + Eval(Resources.DataFieldRes.SOH_Reference) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SOH_Reference),50)%>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SOH_Reference),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblPlantCode" Text="<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>"
                                                    ToolTip="<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_NAME) %>" runat="server"></asp:Label>--%>
                                                <asp:LinkButton ID="lnkPlantCode" CssClass="<%# Eval(Resources.DataFieldRes.CMP_LINE_COLOUR) %>"
                                                    runat="server" Text="<%# Eval(Resources.DataFieldRes.ADM_COMPANY_CMP_DISPLAY_CODE) %>"
                                                    OnClick="ActionHandler" CommandName="SHOWPLANTUPDATEPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%#GetLocalResourceObject("UpdatePlant").ToString() %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName),38) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName),300) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SOType),3,"")%>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SOType),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval( Resources.DataFieldRes.SohToPort),25) %>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SohToPort),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),11) %>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),200) %>'></asp:Label>
                                            </ItemTemplate> 
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipmentDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipmentDateLst" runat="server" Text='<%# Eval( Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.SOCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" Wrap="false" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoices" runat="server" Text='<%# Eval(Resources.DataFieldRes.ConvertedInvoices) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ConvertedInvoices) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfInvoices" Value='<%# Eval(Resources.DataFieldRes.ConvertedInvoices) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SaleOrderStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgShortClose" ToolTip="<%$ resources:ShortCloseSC %>" runat="server"
                                                    OnClick="ActionHandler" CommandName="SHORTCLOSE" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowPaging="false" OnRowDataBound="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemCode),20) %>'
                                                                        ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemName),300) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>" SortExpression="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrand" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ?
                                                                    ERP.Utilities.CommonFunctions.GetShortString(Eval("INV_ITEM_MST.ITM_NAME"),95)
                                                                    : ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),95) %>'
                                                                        ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ?
                                                                         ERP.Utilities.CommonFunctions.GetShortString(Eval("INV_ITEM_MST.ITM_NAME"),95)
                                                                        :ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),300) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="44%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DesiredDeliveryDate %>" SortExpression="<%$ resources:DataFieldRes,SodDesiredDeliveryDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesiredDeliveryDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" Wrap="false" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="4%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <%--Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode)--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="<%$ resources:DataFieldRes,SodSaleQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:C0}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:C0}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <%--SodQty--%>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvoicedQty %>" SortExpression="<%$ resources:DataFieldRes,SOInvoicedQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoicedQty" runat="server" Text='<%#  GetDivide(Eval(Resources.DataFieldRes.SOInvoicedQty), Eval(Resources.DataFieldRes.SodSaleUOMConvFactor))  %>'
                                                                        ToolTip='<%# GetDivide(Eval(Resources.DataFieldRes.SOInvoicedQty), Eval(Resources.DataFieldRes.SodSaleUOMConvFactor)) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ resources:InvoicedQty %>" SortExpression="<%$ resources:DataFieldRes,SOInvoicedQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoicedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SOInvoicedQty, "{0:C0}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SOInvoicedQty, "{0:C0}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:UnitCost %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitCost" runat="server" Text='<%# GetFormattedRate(Eval(Resources.DataFieldRes.SodUnitCost)) %>'
                                                                        ToolTip='<%# GetFormattedRate(Eval(Resources.DataFieldRes.SodUnitCost)) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SodAmount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>--%>
                                                            <%--<asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            <%--Secnd Division--%>
                            <div id="divFilter" class="max-100">
                                <h1 class="search-colapse-normal">
                                    <%-- <%= GetLocalResourceObject("Filter").ToString() %>--%>
                                    <%= GetGlobalResourceObject("Captions", "GoodsOutward").ToString()%>
                                    <img id="ImbShowDODetails" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowFilter();" />
                                    <img id="ImbHideDODetails" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideFilter();" />
                                </h1>
                                <div id="divFilterDetails">
                                    <div id="divDeliveryOrderDtls" runat="server" visible="false">
                                        <%--<h4>
                                    <%= GetGlobalResourceObject("Captions", "GoodsOutward").ToString()%>
                                </h4>--%>
                                        <%--Secnd Division--%>
                                        <div class="gridwrap hierarchical-wrap maxh-290" id="divDO_ScrollContainer" grid="grdDOHdr">
                                            <asp:HiddenField ID="hdfDO_ExpandPosition" runat="server" />
                                            <cc1:ExtGridView runat="server" ID="grdDOHdr" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                                PageSize="<%$ resources:PageSize %>">
                                                <%--OnRowDataBound="ActionHandler"--%>
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:GonNumber %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoNumber" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:Label>
                                                            <asp:HiddenField runat="server" ID="hdfIsExpandedDOItem" Value="0" />
                                                            <asp:HiddenField runat="server" ID="hdfDoPk" Value="<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>" />
                                                            <asp:Button runat="server" ID="btnGetDoDetails" OnClick="ActionHandler" CommandName="DODETAILS"
                                                                CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>' EnableTheming="false"
                                                                Style="display: none" />
                                                            <%-- <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />--%>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="12%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:DoDate %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderDate, Resources.Constants.DateFormatGrid) %>'
                                                                ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderDate, Resources.Constants.DateFormatGrid) %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="10%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblShipTo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.ShipTo),300) %>'
                                                                ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.ShipTo),300) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="25%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblShipBy" runat="server" Text='<%# Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName) %>'
                                                                ToolTip='<%# Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName) %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="53%" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <div class="hierarchical-gridwrap">
                                                                <cc1:ExtGridView runat="server" ID="grdDODetails" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                                    OnRowDataBound="ActionHandler">
                                                                    <EmptyDataTemplate>
                                                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                    </EmptyDataTemplate>
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("INV_ITEM_MST.ITM_CODE"),23) %>'
                                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("INV_ITEM_MST.ITM_CODE"),300) %>'>
                                                                                </asp:Label>
                                                                                <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="18%" HorizontalAlign="Left" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),62) %>'
                                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),300) %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="40%" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Size">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSize" runat="server" Text='<%# GetConstName(Eval("INV_ITEM_MST")) %>'
                                                                                    ToolTip='<%# Eval("INV_UOM_MST.UOM_NAME") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="10%" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'
                                                                                    ToolTip='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="2%" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:N}") %>'
                                                                                    ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:N}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                            <HeaderStyle CssClass="amount-numeric" />
                                                                        </asp:TemplateField>
                                                                        <%--<asp:TemplateField HeaderText="<%$ resources:DelQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDel" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED", "{0:N}") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderText="<%$ resources:DelQty %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDel" runat="server" Text='<%# Eval("DPD_QTY_DESPATCHED", "{0:N}") %>'
                                                                                    ToolTip='<%# Eval("DPD_QTY_DESPATCHED", "{0:N}") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                            <HeaderStyle CssClass="amount-numeric" />
                                                                        </asp:TemplateField>
                                                                        <%-- <asp:TemplateField HeaderText="<%$ resources:BalQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBal" runat="server" Text='<%#  (Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) > 0 ? Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) : 0).ToString("N") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="table-secondlevel" />
                                                                    <HeaderStyle CssClass="table-secondlevela" />
                                                                </cc1:ExtGridView>
                                                            </div>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="nopadding" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="table-firstlevel" />
                                                <HeaderStyle CssClass="table-firstlevela" />
                                            </cc1:ExtGridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divPackingDtls" runat="server" visible="false">
                                <h4>
                                    <%= GetGlobalResourceObject("Captions", "PackingDivCaption").ToString()%>
                                </h4>
                                <%--Secnd Division--%>
                                <div class="gridwrap hierarchical-wrap" id="divPacking_ScrollContainer" grid="grdPackingHdr">
                                    <asp:HiddenField ID="hdfPacking_ExpandPosition" runat="server" />
                                    <cc1:ExtGridView runat="server" ID="grdPackingHdr" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                        PageSize="<%$ resources:PageSize %>">
                                        <%--OnRowDataBound="ActionHandler"--%>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProdCode) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ProdCode) %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedPackingItem" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hdfSodtlPk" Value="<%# Eval(Resources.DataFieldRes.OrderDtlPK) %>" />
                                                    <asp:Button runat="server" ID="btnGetPackingDetails" OnClick="ActionHandler" CommandName="PACKINGDETAILS"
                                                        CommandArgument='<%# Eval(Resources.DataFieldRes.OrderDtlPK) %>' EnableTheming="false"
                                                        Style="display: none" />
                                                    <%--<asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:BrandLabel %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBrand" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.BrandNameCode),70) %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.BrandNameCode) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="55%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:N}") %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:N}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PackedQty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPackedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodQtyPAcked, "{0:N}") %>'
                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodQtyPAcked, "{0:N}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="hierarchical-gridwrap">
                                                        <cc1:ExtGridView runat="server" ID="grdPackingDetails" AutoGenerateColumns="False"
                                                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                            AllowPaging="false" OnRowDataBound="ActionHandler">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:BinCard %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBincard" runat="server" Text='<%# Eval(Resources.DataFieldRes.BinCardNo) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.BinCardNo) %>'>
                                                                        </asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:PackingDt %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPackingDt" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingDt, Resources.Constants.DateFormatGrid) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.PackingDt, Resources.Constants.DateFormatGrid) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:LotNo %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLotNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingLotNo) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.PackingLotNo) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:CartonNo %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCartonNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonNo) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.CartonNo) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:CartonQty %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCartonQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonQty) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.CartonQty) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:QALot %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQALot" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonQALot) %>'
                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.CartonQALot) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="15%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="table-secondlevel" />
                                                            <HeaderStyle CssClass="table-secondlevela" />
                                                        </cc1:ExtGridView>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="nopadding" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                    </cc1:ExtGridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="divShortClose" style="display: none">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S padgtop7">
                                    <asp:Label runat="server" ID="lblRemark" Text="<%$ resources:Remark%>" AssociatedControlID="txtRemarks"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRemarks" TabIndex="18" TextMode="MultiLine" onkeyup="limitText(this,250);"
                                        onpaste="return false;" CssClass="input-w65per" MaxLength="250">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label runat="server" ID="lblRefNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtRefNo"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtRefNo" CssClass="input-w65per" TabIndex="19" MaxLength="50">
                                    </asp:TextBox>
                                </div>
                            </td>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label class="margnrgt3">
                                        </label>
                                        <asp:Button runat="server" ToolTip="<%$ resources:ShortCloseSC %>" ID="btnShortCloseConfm"
                                            SkinID="btnInner-shortclose" Text="<%$Resources:Controls,ShortClose%>" class="inputbtn"
                                            OnClientClick="return ShowConfirmShortClose();" />
                                    </div>
                                </td>
                            </tr>
                        </tr>
                    </table>
                </div>
                <%-- Update Company/Plant Popup----------------------------%>
                <div id="divUpdatePlant" style="display: none">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S padgtop7">
                                    <asp:Label ID="lblCompany" runat="server" Text="<%$resources:Controls,CompanyPlant%>"
                                        AssociatedControlID="ddlUpdateCompany"></asp:Label>
                                    <asp:DropDownList ID="ddlUpdateCompany" runat="server" TabIndex="1" CssClass="select-half">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label class="margnrgt3">
                                        </label>
                                        <asp:Button ID="btnUpdatePlant" runat="server" Text="<%$resources:ErpRes,Save %>"
                                            CommandName="UPDATEPLANT" OnClick="ActionHandler" SkinID="btnInner-save" ToolTip="<%$resources:ErpRes,Save %>"
                                            class="inputbtn" CommandArgument="SEC_UpdatePlant" />
                                    </div>
                                </td>
                            </tr>
                        </tr>
                    </table>
                </div>
                <%-- End Update Company/Plant Popup----------------------------%>
                <div id="diverror" style="display: none">
                    <asp:HiddenField ID="hdfDelStatusCurrent" runat="server" />
                    <asp:HiddenField ID="hdfWrkfStatusCurrent" runat="server" />
                    <asp:HiddenField ID="hdfIsSCConverted" runat="server" />
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="SOClose" runat="server" />
                    <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="contract" runat="server" />--%>
                </div>
            </div>
            <div style="display: none">
                <asp:Button ID="btnShCloseSave" runat="server" OnClick="ActionHandler" CommandName="SHORTCLOSESAVE" />
            </div>
            <asp:HiddenField ID="hdfCopyYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
             <asp:HiddenField ID="hdfIsShowInvNo" runat="server" Value="" />
            <asp:HiddenField ID="hdfIsShowConverted" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
