<%@ Page Title="<%$ Resources:Captions,Title_DeliveryOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" AutoEventWireup="true" CodeBehind="DeliveryOrder.aspx.cs"
    Inherits="ERPSMS_v01.Sales.DeliveryOrder" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", false, false);
            GrandScriptUtils.DatePickerCommon("txtCYDate");
            GrandScriptUtils.DatePickerCommon("txtRtnDate");
            GrandScriptUtils.DatePickerCommon("txtDateOfShipment");
            GrandScriptUtils.DatePickerCommon("txtBookingDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtDespatchNumber", url, "hdfDPHPK", true, true, "DELIVERYORDERNUMBER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "TRANSPORTER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCompany", url, "hdfCompany", true, true, "TRANSPORTER");

            $("[id*=txtDespNow]").ForceNumericOnly();
            ShowHideExpand();
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
        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
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
        }
        $(document).ready(function () {
            $("[id$=ddlPrint]").live('change', function (e) {
                var url = '<%=Resources.PageURL.ReportUrl %>' + "?ID=" + $("[id$=hdfCurPk]").val() + "&APPTYPE=" + $("[id$=hdfType]").val() + "&APPSUBTYPE=" + $(this).val();
                //var event = "window.open('" + url + "');return false;"
                var event = GetPDFUrl(url);
                $("[id$=btnPrintlist]").attr("onClick", event);

            });

        });

        function AfterGridExpand(row) {
            if ($("[id$=grdDeliveryOrderList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
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
            $("[id*=grdSoList] tr td[cellIndex=0]").hide();
            $("[id*=grdSoList] tr th[cellIndex=0]").hide();

        }
        function ResetSOSelection() {
            $('[id$=grdSoList]').find('tr td input:checkbox[id$=chkSCselect]').removeAttr('checked');
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
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none" visible="false">
                                    <li runat="server" id="pnlSubmit" visible="false" style="display: none">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow()" ValidationGroup="inv"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave" visible="false" style="display: none">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="50" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow()" ValidationGroup="delivery"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete" visible="false" style="display: none">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="51" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li runat="server" id="pnlPrint" visible="false" style="display: none">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="53" Text="Print"
                                            OnClick="ActionHandler" ToolTip="Print" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>
                                    <li visible="false" style="display: none">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="52" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <%--<li>
                                        <asp:Button runat="server" ID="btnInvoice" CommandName="PRINTINVOICE" TabIndex="53"
                                            Text="Invoice" OnClick="ActionHandler" ToolTip="Invoice" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" />
                                    </li>--%>
                                    <%--<li>
                                        <asp:Button runat="server" ID="btnPackingList" CommandName="PRINTPACKINGLIST" TabIndex="54"
                                            Text="Packing List" OnClick="ActionHandler" ToolTip="Packing List" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnShippingInstr" CommandName="PRINTSHIPPINGINSTRUCTION"
                                            TabIndex="55" Text="Shipping.Instr" OnClick="ActionHandler" ToolTip="Shipping.Instr"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCertificationOfOrigin" CommandName="PRINTCERTIFICATIONOFORIGIN"
                                            TabIndex="56" Text="Certification" OnClick="ActionHandler" ToolTip="Certification of Origin"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPostShipmentAdvice" CommandName="PRINTPOSTSHIPMENTADVICE"
                                            TabIndex="57" Text="Post Shipment" OnClick="ActionHandler" ToolTip="Post Shipment Advice"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" />
                                    </li>--%>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server">
                                        <asp:Button runat="server" ID="btnInspection" CommandName="INSPECTION" TabIndex="53"
                                            Text="<%$resources:Inspection %>" OnClick="ActionHandler" ToolTip="<%$resources:Inspection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li style="display: none">
                                        <asp:Button runat="server" TabIndex="54" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li style="display: none">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="55" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li runat="server" id="pnlInv">
                                        <asp:Button runat="server" ID="btnPickForInvoice" CommandName="PICKFORINVOICING"
                                            TabIndex="56" Text="<%$resources:PickSoForInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickSoForInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="57"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetSOSelection()" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSC %>">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="58" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowDO %>">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="59" OnClick="ActionHandler" CommandName="DELIVERYORDER"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="60" OnClick="ActionHandler" CommandName="SALESINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="61" OnClick="ActionHandler" CommandName="INVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Spnmiscellaneous" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowMiscInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbMiscellaneous" Text="<%$resources:PageNameRes,miscellaneous %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="35" OnClick="ActionHandler" CommandName="MISC"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowReceipt %>">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="62" OnClick="ActionHandler" CommandName="SALESRECEIPT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="63" OnClick="ActionHandler" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAR %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="64" OnClick="ActionHandler" CommandName="ACRECEIVABLE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating" style="display: none">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                TabIndex="65" OnClick="ActionHandler" CommandName="DELIVERYLIST" CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="66" OnClick="ActionHandler" CommandName="DELIVERYDETAIL" CssClass="tab-inactive"></asp:LinkButton>
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="medium" MaxLength="11"
                                                onkeydown="return CheckKey(event);" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-a-07-12"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event);" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$ resources:TransportCo %>" AssociatedControlID="txtVendor" CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="3"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half  margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$ resources:GoodsOutwardNo %>"
                                                AssociatedControlID="txtDespatchNumber" CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox ID="txtDespatchNumber" runat="server" CssClass="select-small-b  margnbotm0"
                                                MaxLength="70" TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDPHPK" runat="server" Value="" />
                                            <%--<asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="6" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="7" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdDeliveryOrderList" AutoGenerateColumns="False"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="True" OnSorting="ActionHandler" Width="100%" OnRowDataBound="ActionHandler"
                                    PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="17" runat="server" GroupName="SelectOne"
                                                    Visible="false" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGroupingHierarchy(this, 'grdDeliveryOrderList');" />
                                            </ItemTemplate>
                                            <%-- <ItemStyle Width="3%" />--%>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField>
                                            <ItemStyle Width="1.5%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:GODate %>" SortExpression="DPH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("DPH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Left" Wrap="false" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GONo %>" SortExpression="DPH_NO">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDoNumber" CssClass="text-underline" runat="server" OnClick="ActionHandler"
                                                    CommandName="SHOW" Text='<%# Eval("DPH_NO") %>' ToolTip='<%# Eval("DPH_NO") %>'
                                                    CommandArgument='<%# Eval("DPH_PK") %>'></asp:LinkButton>
                                                <%--<asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("DPH_NO") ==""?"[NEW]":Eval("DPH_NO")%>'
                                                    ToolTip='<%# Eval("DPH_NO")%>'></asp:Label>--%>
                                                <asp:HiddenField runat="server" ID="hdfDespatchID" Value='<%# Eval("DPH_PK") %>' />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="GONDETAILS"
                                                    CommandArgument='<%# Eval("DPH_PK") %>' EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="CRM_CUSTOMER_MST.CUS_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(  Eval("CRM_CUSTOMER_MST"+"."+"CUS_CODE"),20)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(  Eval("CRM_CUSTOMER_MST"+"."+"CUS_NAME"),250)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.DOCustomerPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DestPort %>" SortExpression="DPH_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# Eval("DPH_TO_PORT")%>' ToolTip='<%# Eval("DPH_TO_PORT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="16%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Carrier %>" SortExpression="CON_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCarrier" runat="server" Text='<%# Eval("ADM_CONST_MST1"+"."+"CON_CODE")%>'
                                                    ToolTip='<%# Eval("ADM_CONST_MST1"+"."+"CON_CODE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ContainerNo %>" SortExpression="DPH_NO"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainer" runat="server" Text='<%# Eval("DPH_CONTAINER_NO")%>'
                                                    ToolTip='<%# Eval("DPH_CONTAINER_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <%--  <ItemStyle Width="11%" HorizontalAlign="Left" />--%>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrdtUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespQty" runat="server"></asp:Label>
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
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransportCo %>" SortExpression="VEN_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransportCo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PUR_VENDOR_MST"+"."+"VEN_NAME"),28)%>'
                                                    ToolTip='<%# Eval("PUR_VENDOR_MST"+"."+"VEN_NAME")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="return false" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("DPH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <%--  <asp:TemplateField HeaderText="Enclosure" SortExpression="SalEnclosure">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnclosure" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPH_ENCLOS_TERM_TEXT"),8)%>'
                                                    ToolTip='<%# Eval("DPH_ENCLOS_TERM_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                        OnRowDataBound="ActionHandler" Width="100%">
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
                                                                    <asp:CheckBox runat="server" ID="chkSCselect" Height="22px" />
                                                                    <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" VerticalAlign="Middle" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                                                    <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                                        OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="14%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>



                                                             <asp:TemplateField>
                                                                <ItemTemplate>
                                                                     <asp:Label ID="lblPlant" runat="server" Text='<%# Eval(Resources.DataTableRes.CompanyMst + "." +Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.CompanyMst + "." +Resources.DataFieldRes.CMP_DISPLAY_CODE) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SOType),3,"") %>'
                                                                        ToolTip='<%#Eval(Resources.DataFieldRes.SOType) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval( Resources.DataFieldRes.SohToPort),21) %>'
                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SohToPort),300) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="25%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShipBy" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),11) %>'
                                                                        ToolTip='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataTableRes.ConstMst1 + "." +Resources.DataFieldRes.ConstName),200) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DespQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSCDespQty" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSCInvQty" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
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
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap">
                                <asp:GridView ID="grdDeliveryList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="12">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSONo" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfSONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IGPLCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfBrandCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="32%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfUOM" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespNow %>" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtPayNow" runat="server" onkeyup="CalculateTotal(this);" CssClass="small-a numeric"
                                                    MaxLength="10"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtDespNow" runat="server" CssClass="small-a numeric" MaxLength="10"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--Remove--%>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" SkinID="delete-icon"
                                                    CommandName="REMOVE" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                </h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblDelivery" Text="<%$ resources:DeliveryTo %>" AssociatedControlID="lblDeliveryTo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblDeliveryTo" Text="" TabIndex="13" onkeydown="return EnableArrowKey(event)"
                                                        onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblCustomerName" Text="" AssociatedControlID="txtDeliveryAddress"></asp:Label>
                                                    <%--<asp:TextBox runat="server" ID="txtDeliveryAddress" Text="" TabIndex="15"></asp:TextBox>--%>
                                                    <asp:TextBox runat="server" ID="txtDeliveryAddress" TextMode="MultiLine" CssClass="multiline-2line"
                                                        onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" TabIndex="15"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblDestination" Text="<%$ resources:DestinationPort %>"
                                                        AssociatedControlID="lblDestinationPort"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblDestinationPort" Text="" TabIndex="17" onkeydown="return EnableArrowKey(event)"
                                                        onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblPortLoading" Text="<%$ resources:PortofLoading %>"
                                                        AssociatedControlID="lblPortOfLoading"></asp:Label>
                                                    <asp:HiddenField ID="hdfPortOfLoading" runat="server" />
                                                    <asp:Label runat="server" ID="lblPortOfLoading" Text="" TabIndex="19"></asp:Label>
                                                    <asp:Label runat="server" ID="lblFinal" Text="<%$ resources:FinalDestination %>"
                                                        AssociatedControlID="lblFinalDestination"></asp:Label>
                                                    <asp:TextBox runat="server" ID="lblFinalDestination" Text="" TabIndex="21" onkeydown="return EnableArrowKey(event)"
                                                        onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <%--<asp:TextBox runat="server" ID="txtFinalDestination" Text="" TabIndex="21"></asp:TextBox>--%>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblOrderNo" Text="<%$ resources:DeliveryOrderNo %>"
                                                        AssociatedControlID="lblDeliveryOrderNo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDeliveryOrderNo" Text="" TabIndex="14"></asp:Label>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:CYDate %>" AssociatedControlID="txtCYDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtCYDate" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="16"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="vrfCYDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="delivery" EnableClientScript="true" runat="server" ControlToValidate="txtCYDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="Enter CY Date">
                                                    </asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:RTNDate %>" AssociatedControlID="txtRtnDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRtnDate" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="18"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:ETD %>" AssociatedControlID="txtETD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETD" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="20"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETD" runat="server" Value="" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label4" Text="<%$ resources:ETA %>" AssociatedControlID="txtETA"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETA" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="22"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETA" runat="server" Value="" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Liner_Details").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label5" Text="<%$ resources:Carrier %>" AssociatedControlID="ddlCarrier"></asp:Label>
                                                    <asp:DropDownList ID="ddlCarrier" runat="server" TabIndex="23">
                                                    </asp:DropDownList>
                                                    <asp:Label runat="server" ID="Label7" Text="<%$ resources:SealNo %>" AssociatedControlID="txtSealNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSealNo" Text="" MaxLength="150" TabIndex="25"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label15" Text="<%$ resources:Container %>" AssociatedControlID="txtContainer"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtContainer" Text="" MaxLength="70" TabIndex="24"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Transportation_Details").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:Company %>" AssociatedControlID="txtCompany"></asp:Label>
                                                    <asp:TextBox ID="txtCompany" runat="server" MaxLength="100" TabIndex="26"> </asp:TextBox>
                                                    <asp:HiddenField ID="hdfCompany" runat="server" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label8" Text="<%$ resources:LorryNo %>" AssociatedControlID="txtLorryNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtLorryNo" Text="" MaxLength="150" TabIndex="28"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label9" Text="<%$ resources:Driver %>" AssociatedControlID="txtDriver"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDriver" Text="" MaxLength="150" TabIndex="27"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%-- group end here--%>
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
                                                    <asp:TextBox runat="server" ID="txtFeederVessel" Text="" MaxLength="150" TabIndex="29"></asp:TextBox>
                                                    <asp:Label runat="server" ID="Label11" Text="<%$ resources:DateofShipment %>" AssociatedControlID="txtDateOfShipment"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtDateOfShipment" Text="" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="31"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label14" Text="<%$ resources:BookingDate %>" AssociatedControlID="txtBookingDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBookingDate" Text="" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="33"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="Label12" Text="<%$ resources:MotherVessel %>" AssociatedControlID="txtMotherVessel"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtMotherVessel" Text="" MaxLength="150" TabIndex="30"></asp:TextBox>
                                                    <asp:Label runat="server" ID="Label13" Text="<%$ resources:BookingNo %>" AssociatedControlID="txtBookingNo"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtBookingNo" Text="" MaxLength="70" TabIndex="32"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:ShippingMark %>" AssociatedControlID="txtShippingMark"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShippingMark" TextMode="MultiLine" CssClass="multiline-2line"
                                                        onkeydown="limitText(this,400);" onkeyup="limitText(this,400);" TabIndex="34"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <h1>
                                    <%= GetLocalResourceObject("Other_Details").ToString()%></h1>
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label runat="server" ID="lblConsignee" Text="<%$ resources:Consignee %>" AssociatedControlID="txtConsignee"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtConsignee" Text="" TabIndex="30" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfConsignee" />
                                                    <asp:Label runat="server" ID="Label16" Text="" AssociatedControlID="txtConsignee"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtConsigneeDetails" Text="" TabIndex="31" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-L">
                                                    <asp:Label runat="server" ID="lblNotifyParty" Text="<%$ resources:NotifyParty %>"
                                                        AssociatedControlID="txtNotifyParty"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtNotifyParty" Text="" TabIndex="32" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfNotifyParty" />
                                                    <asp:Label runat="server" ID="Label17" Text="" AssociatedControlID="txtNotifyParty"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtNotifyPartyDetails" Text="" TabIndex="33" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" TextMode="MultiLine"
                                                        CssClass="input-disabled"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPaymentTerms" Text="<%$ resources:PaymentTerms %>"
                                                        AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtPaymentTerms" Text="" TabIndex="36" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hdfPaymentTerms" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblEnclosure" Text="<%$ resources:Enclosure %>" AssociatedControlID="txtEnclosure"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtEnclosure" Text="" TabIndex="37" TextMode="MultiLine"
                                                        CssClass="multiline-2line" onkeydown="limitText(this,400);" onkeyup="limitText(this,400);"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblSupplimentary" Text="<%$ resources:Supplimentary %>"
                                                        AssociatedControlID="txtSupplimentary"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtSupplimentary" Text="" TabIndex="35" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblModeofTransport" Text="<%$ resources:ModeofTransport %>"
                                                        AssociatedControlID="txtModeofTransport"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfModeofTransport" />
                                                    <asp:TextBox runat="server" ID="txtModeofTransport" Text="" TabIndex="34" Enabled="true"
                                                        onkeydown="return EnableArrowKey(event)" onpaste="return false;" CssClass="input-disabled"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <%--Print popup window --%>
                            <div id="divPrint" style="display: none" class="content-wrapper">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            &nbsp;<td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="div2col-S" align="center">
                                                <asp:DropDownList ID="ddlPrint" runat="server" TabIndex="101">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="div2col-S" align="right" style="width: 94%">
                                                <asp:Button runat="server" TabIndex="102" ID="btnPrintlist" Text="<%$resources:Controls,Print %>"
                                                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                                <asp:Button runat="server" ID="btnCancellist" Text="<%$resources:Controls,Cancel %>"
                                                    TabIndex="103" OnClientClick="javascript:ClosePopup();" SkinID="btnInner-Cancel"
                                                    ToolTip="<%$resources:Controls,Cancel %>" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
                </div>
                <asp:HiddenField ID="hdfDespatchNo" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="inv">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfType" runat="server" Value="" />
            <asp:HiddenField ID="hdfCurPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
