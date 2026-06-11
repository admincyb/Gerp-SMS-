<%@ Page Title="<%$ Resources:Captions,Title_SaleOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="SaleOrderList.aspx.cs"
    Inherits="ERPSMS_v01.Sales.SaleOrderList" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            var customerSelectText = "Type min 4 characters";

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //         GrandScriptUtils.AddDateRange("txtActFromDate", "hdfActFromDate", "txtActToDate", "hdfActToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true,4, "CUSTOMERLIST", "", "", "", "", customerSelectText);
            if ($("[id$=hdfCustomerID]").val() != "") {
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSONumber", url + "?CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSoPK", true, true,4, "SONUMBERAPPROVED", "", "", "", "", customerSelectText);
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomerPo", url + "?CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfCusPoPK", true, true,4, "CUSPONUMBERCUSTOMERWISE", "", "", "", "", customerSelectText);
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSONumber", url, "hdfSoPK", true, true,4, "SONUMBERAPPROVED", "", "", "", "", customerSelectText);
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomerPo", url, "hdfCusPoPK", true, true,4, "CUSPONUMBERCUSTOMERWISE", "", "", "", "", customerSelectText);
            }

            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            HideFilter();
            //            if ($("[id$=hdfCustomerID]").val() != "") {
            //                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
            //            }
            SetGridScroll();
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



        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#ImbHidePODetails").hide();
            $("#ImbShowPODetails").show();
            $("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#ImbHidePODetails").show();
            $("#ImbShowPODetails").hide();
            $("#divFilterDetails").show();
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

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
        function ResetSOSelection() {
            $('[id$=grdSoList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
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
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlDO">
                                        <asp:Button runat="server" ID="btnPickForDO" CommandName="PICKFORDO" TabIndex="55"
                                            Text="<%$resources:PickSoForDO %>" OnClick="ActionHandler" ToolTip="<%$resources:PickSoForDO %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnPickForInvoice" CommandName="PICKFORINVOICING"
                                            TabIndex="56" Text="<%$resources:PickSoForInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickSoForInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlInv">
                                        <asp:Button runat="server" ID="btnPickForAdvInv" CommandName="PICKFORADVANCEINVOICING"
                                            TabIndex="57" Text="<%$resources:PickSoForAdvanceInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickSoForAdvanceInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="58"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSOSelection()" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowSC %>">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                TabIndex="59" CssClass="tab-inactive" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowDO %>">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="60" OnClick="ActionHandler" CommandName="DELIVERYORDER" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                TabIndex="61" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="SALESINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                TabIndex="62" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="INVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Spnmiscellaneous" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowMiscInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbMiscellaneous" Text="<%$resources:PageNameRes,miscellaneous %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="63" OnClick="ActionHandler" CommandName="MISC"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowReceipt %>">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                TabIndex="64" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="SALESRECEIPT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="65" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAR %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                TabIndex="66" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="ACRECEIVABLE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="1" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <%--  <div class="clear">
                            </div>--%>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" CssClass="txtFromDate-label-19-11" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-c-19-11"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" CssClass="middle-lbl-xsmall-e-19-11" Text="<%$ resources:Status%>"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-c1-19-11" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="<%$ Resources:Captions,Pending %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Completed %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblPlantName" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlPlantName" CssClass="lbl-11-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlPlantName" runat="server" CssClass="select-small-a-19-11" TabIndex="15">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide-5">
                                <tr>
                                    <td class="tds-1">
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half margnbotm0" MaxLength="100"
                                                TabIndex="4" Text="Type min 4 characters"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                        </div>
                                    </td>
                                    <td class="tds-2">
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCusPo" runat="server" Text="<%$resources:CustPo %>" CssClass="middle-lbl-xsmall-e"
                                                AssociatedControlID="txtCustomerPo"></asp:Label>
                                            <asp:TextBox ID="txtCustomerPo" runat="server" CssClass="input-small-c-3 margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>
                                                 <asp:HiddenField ID="hdfCusPoPK" runat="server" Value="" />
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" CssClass="middle-lbl-xsmall-e"
                                                AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="input-small-c margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                               <asp:HiddenField ID="hdfPurPk" runat="server" Value="" />
                                            <asp:Label ID="lblInType" runat="server" Text="<%$resources:Type %>" AssociatedControlID="ddlInvoiceType"
                                                CssClass="middle-lbl-xsmall-b-19-11 margnbotm0"></asp:Label>
                                            <asp:DropDownList ID="ddlInvoiceType" runat="server" TabIndex="6" CssClass="select-small-a margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="7" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="8" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap maxh-290" id="divSO_ScrollContainer" grid="grdSoList">
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" Width="100%"
                                    PageSize="<%$ resources:PageSize %>" ExpandButtonCssClass="GridExpandCollapseButton"
                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                    OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="9" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSOTax" Value='<%# Eval("SOH_TOTAL_TAX") %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQty" Value='<%# Eval(Resources.DataFieldRes.OrderQty) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQtyDispatched" Value='<%# Eval(Resources.DataFieldRes.SodQtyDispatched) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQtyInvoiced" Value='<%# Eval(Resources.DataFieldRes.SOInvoicedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>
                                                <%--<asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                              <%--  <asp:Label ID="lblPlant" runat="server" CssClass="<%# Eval(Resources.DataFieldRes.CMP_LINE_COLOUR) %>"
                                                    Text='<%# Eval(Resources.DataTableRes.CompanyMst + "." +Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.CompanyMst + "." +Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>--%>
                                                     <asp:Label ID="lblPlant" Font-Bold="true" Text="<%# Eval(Resources.DataFieldRes.CompanySpecsCode) %>"
                                                    ToolTip="<%# Eval(Resources.DataFieldRes.CompanySpecs) %>" runat="server"
                                                    CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName),45) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.CustomerMst + "." + Resources.DataFieldRes.CustomerName),250) %>'></asp:Label>--%>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CustomerName),47) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CustomerName) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="27%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:CustPo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerPo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.CustPo),45) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.CustPo),250) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SOType) ,3,"") %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval( Resources.DataFieldRes.SohToPort) ,25)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SohToPort),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SohShipBy),11) %>'
                                                    ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SohShipBy),200) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:ShipmentDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipmentDateLst" runat="server" Text='<%# Eval( Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                              <%--  <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.CurrencyMst +"." + Resources.DataFieldRes.CurrencyCode) %>'></asp:Label>--%>
                                                      <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataFieldRes.CurrencyCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CurrencyCode) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.SOCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" HorizontalAlign="Right" Wrap="false" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SaleOrderStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowPaging="false" OnRowDataBound="ActionHandler">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <%--   <asp:TemplateField>
                                                                <ItemStyle Width="0%" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemCode),15) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="13%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>" SortExpression="">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrand" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),107) %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="37%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DesiredDeliveryDate %>" SortExpression="<%$ resources:DataFieldRes,SodDesiredDeliveryDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesiredDeliveryDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodDesiredDeliveryDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ConfigMst+"."+ Resources.DataFieldRes.cfgData) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="<%$ resources:DataFieldRes,SodSaleQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:n}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvoicedQty %>" SortExpression="<%$ resources:DataFieldRes,SodSaleQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoicedQty" runat="server" Text='<%#  GetDivide(Eval(Resources.DataFieldRes.SOInvoicedQty), Eval(Resources.DataFieldRes.SodSaleUOMConvFactor))  %>'
                                                                        ToolTip='<%# GetDivide(Eval(Resources.DataFieldRes.SOInvoicedQty), Eval(Resources.DataFieldRes.SodSaleUOMConvFactor)) %>'></asp:Label>
                                                                    <%--<asp:Label ID="lblInvoicedQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodSaleQty, "{0:n}") %>'></asp:Label>--%><%--SOInvoicedQty--%>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UnitCost %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitCost" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetFormattedRateO2C(Eval(Resources.DataFieldRes.SodUnitCost)) %>'
                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetFormattedRateO2C(Eval(Resources.DataFieldRes.SodUnitCost)) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SodAmount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemStyle Width="6%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
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
                            <div id="divFilter" class="max-100">
                                <h1 class="search-colapse-normal">
                                    <%= GetGlobalResourceObject("Captions", "GoodsOutward").ToString()%>
                                    <img id="ImbShowPODetails" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowFilter();" />
                                    <img id="ImbHidePODetails" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideFilter();" />
                                </h1>
                                <div id="divFilterDetails">
                                    <%--Secnd Division--%>
                                    <%--  <h4>
                                <%= GetGlobalResourceObject("Captions", "GoodsOutward").ToString()%>
                            </h4>--%>
                                    <%--Secnd Division--%>
                                    <div class="gridwrap hierarchical-wrap" id="divCO_ScrollContainer" grid="grdDOHdr">
                                        <asp:HiddenField ID="hdfCO_ExpandPosition" runat="server" />
                                        <cc1:ExtGridView runat="server" ID="grdDOHdr" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                            CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                            PageSize="<%$ resources:PageSize %>">
                                            <%--OnRowDataBound="ActionHandler"--%>
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GonNumber %>">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDoNumber" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'
                                                            OnClick="ActionHandler" CommandName="SHOW" CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:LinkButton>
                                                        <%--<asp:Label ID="lblDoNumber" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DeliveryOrderNo),15) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:Label>--%>
                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedDOItem" Value="0" />
                                                        <asp:HiddenField runat="server" ID="hdfDoPk" Value="<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>" />
                                                        <asp:Button runat="server" ID="btnGetDoDetails" OnClick="ActionHandler" CommandName="DODETAILS"
                                                            CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>' EnableTheming="false"
                                                            Style="display: none" />
                                                        <%-- <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
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
                                                        <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ShipTo),36) %>'
                                                            ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.ShipTo),300) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblShipBy" runat="server" Text='<%# Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName) %>'
                                                            ToolTip='<%# Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="23%" />
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
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("INV_ITEM_MST.ITM_CODE"),15) %>'
                                                                                ToolTip='<%# Eval("INV_ITEM_MST.ITM_NAME") %>'>
                                                                            </asp:Label>
                                                                            <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME"),75) %>'
                                                                                ToolTip='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="46%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Size">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSize" runat="server" Text='<%# GetConstName(Eval("INV_ITEM_MST")) %>'
                                                                                ToolTip='<%# GetConstName(Eval("INV_ITEM_MST")) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUom" runat="server" Text='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'
                                                                                ToolTip='<%# Eval(Resources.DataTableRes.InvUomMst+"."+ Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:n}") %>'
                                                                                ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:n}") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                        <HeaderStyle CssClass="amount-numeric" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:DelQty %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDel" runat="server" Text='<%# Eval("DPD_QTY_DESPATCHED", "{0:n}") %>'
                                                                                ToolTip='<%# Eval("DPD_QTY_DESPATCHED", "{0:n}") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                        <HeaderStyle CssClass="amount-numeric" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:BalQty %>" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBal" runat="server" Text='<%#  Convert.ToString(Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) > 0 ? Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) : 0) %>'
                                                                                ToolTip='<%#  Convert.ToString(Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) > 0 ? Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) : 0) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                        <HeaderStyle CssClass="amount-numeric" />
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
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="contract" runat="server" />--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
