<%@ Page Title="<%$ Resources:Captions,Title_PurchaseOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="POListing.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.POListing"
    Theme="ClassicExt" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
             var SelectText = "Type min 4 characters";
             var vendorSelectText = "Type min 3 characters";
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtVendor", url + "?IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendorID", true, true, 3, "VENDOR", "", "", "", "", vendorSelectText);
            if ($("[id$='hdfEnableWO']").val() == "1")
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtPoNumber", url + "?EnableWO=1", "hdfPoPK", true, true, 4, "POWONUMBER", "", "", "", "", SelectText);
            else
                GrandScriptUtils.MakeAutoCompleteDDLNEW("txtPoNumber", url + "?IsSBUPO=" + $("[id$='hdfIsSBUPO']").val(), "hdfPoPK", true, true, 4, "PONUMBER", "", "", "", "", SelectText);

            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            HideFilter();
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

            if ($("[id$=grdPoList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
            else {
                var gridType = 0;
                var names = $(row).parent().parent().attr('id').split('_');

                if (names.length == 1) {
                    gridType = names[0] == "grdGRNList" ? 1 : names[0] == "grdGINList" ? 2 : 0;
                }
                else if (names.length > 1) {
                    gridType = (names[names.length - 1] == "grdGRNList" || names[names.length - 2] == "grdGRNList") ? 1
                        : (names[names.length - 1] == "grdGINList" || names[names.length - 2] == "grdGINList") ? 2 : 0;
                }
                if (gridType == 1) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGrn]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetGin]").click();
                    }
                }
                if (gridType == 2) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetProduct]").click();
                    }
                }
            }



            if ($("[id$=grdPOItems]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedPOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetGrn]").click();
                }
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
        function ResetPOSelection() {
            $('[id$=grdPoList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
            $('[id$=grdPoList]').find('tr td input:checkbox[id$=chkPOselect]').removeAttr('checked');
        }

        function ItemListSelection() {
            $("#[id*=grdPoList] input[type=hidden][id*=hdfPOID]").each(function (index) {

                if ($(this).val() == $("[id$=hdfSelectedItemPOPK]").val()) {
                    $(this).closest('tr').find('[id*=imbPOList]').removeClass("itemlist").addClass("itemlist-active");
                }
                else {
                    $(this).closest('tr').find('[id*=imbPOList]').attr("CssClass", "itemlist");
                }
            });
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
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnPickForInvoice" CommandName="PICKFORINVOICING"
                                            TabIndex="10" Text="<%$resources:PickPoForInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickPoForInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlInv">
                                        <asp:Button runat="server" ID="btnPickForAdvInv" CommandName="PICKFORADVANCEINVOICING"
                                            TabIndex="10" Text="<%$resources:PickForAdvanceInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickForAdvanceInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="10"
                                            Text="<%$resources:ResetSelection %>" OnClick="ActionHandler" ToolTip="<%$resources:ResetSelection %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClientClick="ResetPOSelection()" />
                                    </li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPOListing" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowPO %>">
                            <asp:LinkButton runat="server" ID="lbnPOListing" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CommandName="DEFAULT" Text="<%$resources:PageNameRes,PurchaseOrder %>" TabIndex="11"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <%--  <li><span id="spnDirectPurchase" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnDirectPurchase" Text="<%$resources:PageNameRes,DirectPurchase %>"
                                TabIndex="2" CommandName="DIRECTPURCHASE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                        <li><span id="spnInvoicing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnkInvoicing" Text="<%$resources:PageNameRes,Invoice %>"
                                TabIndex="12" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="INVOICE"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPOInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseInvoice %>">
                            <asp:LinkButton runat="server" ID="lbnPOInvoice" Text="<%$resources:PageNameRes,POInvoice %>"
                                TabIndex="13" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="POINVOICE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnExpenses" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowExpense %>">
                            <asp:LinkButton runat="server" ID="lbnExpenses" Text="<%$resources:PageNameRes,Expenses %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="14" OnClick="ActionHandler" CommandName="EXPENSES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPayment" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPayment %>">
                            <asp:LinkButton runat="server" ID="lnkPayment" Text="<%$resources:PageNameRes,Payment %>"
                                TabIndex="15" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="PAYMENT"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="16" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="CRDRNOTE"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowAP %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountPayables %>"
                                TabIndex="17" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" CommandName="ACPAYABLES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <%--  <li><span id="spnAcReceivablebles" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="LinkButton1" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                TabIndex="7" OnClick="ActionHandler" CommandName="ACRECEIVABLE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
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
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.jpg" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
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
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-w24per"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" CssClass="middle-lbl-small-e1-22-12" Text="<%$resources:ToDate %>"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small-b"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <%--<div class="clear">
                                            </div>--%>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblSearch" runat="server" Text="<%$ resources:Type%>" AssociatedControlID="ddlType"></asp:Label>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="select-small-d" TabIndex="3">
                                                <%--<asp:ListItem Text="<%$ Resources:Captions,All %>" Value="0"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,NonService %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Service %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:IoNo %>" CssClass="middle-lbl-xsmall-b"
                                                AssociatedControlID="txtIONo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIONo" TabIndex="4" CssClass="input-small"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblUserPODepartments" runat="server" Text="<%$ resources:Controls, Department %>"
                                                AssociatedControlID="ddlUserPODepartments"></asp:Label>
                                            <asp:DropDownList ID="ddlUserPODepartments" runat="server" CssClass="select-small-c"
                                                TabIndex="5" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$ resources:Controls, CompanyPlant %>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="select-small-d" TabIndex="5">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox ID="txtVendor" runat="server" CssClass="input-half margnbotm0" MaxLength="100"
                                                TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendorID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblPoNumber" runat="server" Text="<%$resources:PONumber %>" AssociatedControlID="txtPoNumber"></asp:Label>
                                            <asp:TextBox ID="txtPoNumber" runat="server" CssClass="select-small-c1 margnbotm0"
                                                MaxLength="100" TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfPoPK" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-xsmall-b margnbotm0"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a margnbotm0"
                                                TabIndex="7">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Completed %>" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="8" CommandName="SEARCH"
                                                SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="8" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <%--  <div class="clear">
                            </div>--%>
                            <div class="gridwrap hierarchical-wrap maxh-290">
                                <cc1:ExtGridView runat="server" ID="grdPoList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                    Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="9" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:ImageButton runat="server" ID="imbPOList" ToolTip="<%$Resources:ItemDetails %>"
                                                    CssClass="itemlist" OnClick="ActionHandler" CommandName="POITEMDETAILS" alt="" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="PODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.PurchaseOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPOID" Value='<%# Eval(Resources.DataFieldRes.PurchaseOrderPk) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOHIssueDept" Value='<%# Eval("POH_ISSUE_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOGroup" Value='<%# Eval(Resources.DataFieldRes.POGroup) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOTaxAmount" Value='<%# Eval(Resources.DataFieldRes.POTaxAmount) %>' />
                                                <asp:HiddenField runat="server" ID="hdfMenuType" Value='<%# Eval(Resources.DataFieldRes.POH_MENU_TYPE) %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOHGROUPVALUE" Value='<%# Eval("POH_GROUP") %>' />
                                                <%--   <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" ID="rbtSelect"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkPOselect" TabIndex="9" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.PODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.PODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POWONumber %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.PONumber) %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.PurchaseOrderPk) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.PONumber) %>'></asp:LinkButton>
                                                <%--
                                           
                                                <asp:Label ID="lblPoNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.PONumber) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.PONumber) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lnkCmpny" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    runat="server" Text='<%# Eval(Resources.DataFieldRes.POHCompanyText) %>' ToolTip='<%# Eval(Resources.DataFieldRes.POHCompanyText) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.Vendor),37) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.Vendor) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval(Resources.DataFieldRes.POVendorPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="36%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SCNumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSCNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SaleOrder),15) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrder) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerPONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCusPoNo" runat="server" Text='<%# Eval("SOH_REFERENCE") %>' ToolTip='<%# Eval("SOH_REFERENCE") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.POType)  ,3,"")%>'
                                                    ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfPOType" runat="server" Value='<%#Eval(Resources.DataFieldRes.POType) %>' />
                                                <asp:HiddenField ID="hdfMenuTypeText" runat="server" Value='<%# Eval("POH_MENU_TYPE_TEXT") %>' />
                                                <%--<asp:HiddenField ID="hdfHeaderTaxType" runat="server" Value='<%# Eval(Resources.DataTableRes.PurchaseOrderTaxHdr +"." + Resources.DataFieldRes.HeaderTaxType) %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShippingLocation %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipping" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DepartmentName),20) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DepartmentName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataFieldRes.CurrencyCode)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CurrencyCode)  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.POTotal, "{0:c}") %>'
                                                    ToolTip='<%#  Eval(Resources.DataFieldRes.POTotal, "{0:c}")  %>'></asp:Label>
                                                <asp:HiddenField ID="hdfPOCurrency" runat="server" Value='<%# Eval(Resources.DataFieldRes.POCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.POStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxStatus" Value='<%# Eval("TRX_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" DataKeyNames="<%$ resources:DataFieldRes,POItemPk %>"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                        OnRowDataBound="ActionHandler" Width="100%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="1%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemName),52) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="33%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Rate %>" SortExpression="<%$ resources:DataFieldRes,PodRate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetFormattedRateP2P(Eval(Resources.DataFieldRes.PodRate, "{0:c}")) %>'
                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetFormattedRateP2P(Eval(Resources.DataFieldRes.PodRate, "{0:c}")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:QuantityUom %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodQtyApproved, "{0:N}") +" " + Eval(Resources.DataFieldRes.UomCode) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodQtyApproved, "{0:N}") + " " +  Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataFieldRes.CurrencyCode)  %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CurrencyCode)  %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,PodAmount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodAmount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodAmount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Discount %>" SortExpression="<%$ resources:DataFieldRes,PodDiscount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodDiscount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodDiscount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>" SortExpression="<%$ resources:DataFieldRes,PodTax %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTax" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodTax, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodTax, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SubTotal %>" SortExpression="<%$ resources:DataFieldRes,PodSubTotal %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSubTot" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodSubTotal, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodSubTotal, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:RequiredBy %>" SortExpression="<%$ resources:DataFieldRes,PodReqDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqBy" runat="server" Text='<%# Eval(Resources.DataFieldRes.PodReqDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PodReqDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
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
                                    <%-- <%= GetLocalResourceObject("Filter").ToString() %>--%>
                              
                                        Item Details
                                    <img id="ImbShowPODetails" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowFilter();" />
                                    <img id="ImbHidePODetails" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideFilter();" />
                                </h1>
                                <div id="divFilterDetails">
                                    <%-- <div id="divPurchaseOrderDtls" runat="server" visible="false">--%>
                                    <%--  <h4>
                                    Item Details</h4>--%>
                                    <%--Secnd Division--%>
                                    <div class="gridwrap hierarchical-wrap">
                                        <cc1:ExtGridView runat="server" ID="grdPOItems" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                            ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemName),35) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedPOItem" Value="0" />
                                                        <asp:HiddenField runat="server" ID="hdfPOItem" Value="<%# Eval(Resources.DataFieldRes.POItemPk) %>" />
                                                        <asp:Button runat="server" ID="btnGetGrn" OnClick="ActionHandler" CommandName="GRNDETAILS"
                                                            CommandArgument='<%# Eval(Resources.DataFieldRes.POItemPk) %>' EnableTheming="false"
                                                            Style="display: none" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="45%" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ItemCat %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCat" runat="server" Text='<%# Eval(Resources.DataFieldRes.Category) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.Category) %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:POOrdQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOOrdQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.POOrdQty, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.POOrdQty, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="rate-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:PORcvdQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPORcvdQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PORcvdQty, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.PORcvdQty, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="rate-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:POAccQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOAccQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.POAccQty, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.POAccQty, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="rate-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:POInvQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOInvQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.POInvQty, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode) %>'
                                                            ToolTip='<%# Eval(Resources.DataFieldRes.POInvQty, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="rate-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <div class="hierarchical-gridwrap">
                                                            <cc1:ExtGridView runat="server" ID="grdGRNList" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                                                ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                                ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                                                <EmptyDataTemplate>
                                                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ resources:GrnNo %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGrnNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.GrnNo) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.GrnNo) %>'>
                                                                            </asp:Label>
                                                                            <asp:HiddenField runat="server" ID="hdfIsExpandedGrn" Value="0" />
                                                                            <asp:HiddenField runat="server" ID="hdfGrnPk" Value="<%# Eval(Resources.DataFieldRes.GrnDtlPk) %>" />
                                                                            <asp:Button runat="server" ID="btnGetGin" OnClick="ActionHandler" CommandName="GINDETAILS"
                                                                                CommandArgument='<%# Eval(Resources.DataFieldRes.GrnDtlPk) %>' EnableTheming="false"
                                                                                Style="display: none" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="21%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGrnDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.GrnDtlDate, Resources.Constants.DateFormatGrid) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.GrnDtlDate, Resources.Constants.DateFormatGrid) %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:GrnStore %>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGrnStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.DepartmentName),25) %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.DepartmentName) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="35%" />
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:GRNRcvdQty%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGRNRcvdQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.GrnQuantity, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode)  %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.GrnQuantity, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="14%" HorizontalAlign="Right" />
                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ resources:GRNAccQty%>">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGRNAccQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.GRNAccQty, "{0:N}") +" "+Eval(Resources.DataFieldRes.UomCode)  %>'
                                                                                ToolTip='<%# Eval(Resources.DataFieldRes.GRNAccQty, "{0:N}")  +" "+Eval(Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <div class="hierarchical-gridwrap">
                                                                                <cc1:ExtGridView runat="server" ID="grdGINList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                                                    ForeColor="#333333" AllowPaging="false">
                                                                                    <EmptyDataTemplate>
                                                                                        <asp:Label ID="lblGINList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                    </EmptyDataTemplate>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:GinNo %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.GinDtlNo) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GinDtlNo) %>'>
                                                                                                </asp:Label>
                                                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedGinList" Value="0" />
                                                                                                <asp:HiddenField runat="server" ID="hdfGinPk" Value="<%# Eval(Resources.DataFieldRes.GinDtlPk) %>" />
                                                                                                <asp:Button runat="server" ID="btnGetProduct" OnClick="ActionHandler" CommandName="STDETAILS"
                                                                                                    EnableTheming="false" Style="display: none" CommandArgument='<%# Eval(Resources.DataFieldRes.GinDtlPk) %>' />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="20%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.GinDtlDate, Resources.Constants.DateFormatGrid) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GinDtlDate, Resources.Constants.DateFormatGrid) %>'>
                                                                                                </asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:GinStore %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.DepartmentMst+"."+Resources.DataFieldRes.DepartmentName),23) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataTableRes.DepartmentMst+"."+Resources.DataFieldRes.DepartmentName) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="20%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:GinQtyInspected %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinQtyInspected" runat="server" Text='<%#Eval(Resources.DataFieldRes.GinQtyInspected, "{0:N}") +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GinQtyInspected, "{0:N}") +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="rate-numeric" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:GinQtyRejected %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinQtyRejected" runat="server" Text='<%#Eval(Resources.DataFieldRes.GinQtyRejected, "{0:N}") +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GinQtyRejected, "{0:N}") +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="rate-numeric" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ resources:GinQtyAccepted %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGinQtyAccepted" runat="server" Text='<%# Eval(Resources.DataFieldRes.GinQtyAccepted, "{0:N}")  +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'
                                                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GinQtyAccepted, "{0:N}") +" "+Eval( Resources.DataTableRes.InvUomMst+"."+Resources.DataFieldRes.UomCode) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                            <HeaderStyle CssClass="rate-numeric" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <div class="hierarchical-gridwrap">
                                                                                                    <cc1:ExtGridView runat="server" ID="grdStockTransfer" AutoGenerateColumns="False"
                                                                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                                                        GridLines="None" ExpandButtonText="+" DataKeyNames="<%$ resources:DataFieldRes,StHdrPK %>"
                                                                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                                                                        OnRowDataBound="ActionHandler">
                                                                                                        <EmptyDataTemplate>
                                                                                                            <asp:Label ID="lblSTEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                                        </EmptyDataTemplate>
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="<%$ resources:STNumber %>">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblStNumber" runat="server" Text='<%# Eval(Resources.DataFieldRes.STNumber) %>'
                                                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.STNumber) %>'>
                                                                                                                    </asp:Label>
                                                                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                                                    <asp:HiddenField ID="hdfStockTransferPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.StHdrPK) %>' />
                                                                                                                    <%--<asp:HiddenField ID="hdfInvestor" runat="server" Value='<%# Eval("POH_INVESTOR") %>' />--%>
                                                                                                                    </ItemTemplate>
                                                                                                                <ItemStyle Width="17.5%" HorizontalAlign="Left" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblStDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.STDate, Resources.Constants.DateFormatGrid) %>'
                                                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.STDate, Resources.Constants.DateFormatGrid) %>'>
                                                                                                                    </asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle Width="25%" />
                                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="<%$ resources:QuantityUom %>">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblStQuantityUom" runat="server" Text='' ToolTip=''></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                                                <HeaderStyle CssClass="rate-numeric" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <RowStyle CssClass="table-thirdlevel" />
                                                                                                        <HeaderStyle CssClass="table-thirdlevela" />
                                                                                                    </cc1:ExtGridView>
                                                                                                </div>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle CssClass="nopadding" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <%--Second--%>
                                                                                    <RowStyle CssClass="table-thirdlevel" />
                                                                                    <HeaderStyle CssClass="table-thirdlevela" />
                                                                                </cc1:ExtGridView>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <ItemStyle CssClass="nopadding" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <%--First--%>
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
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="contract" runat="server" />--%>
                </div>
                <asp:HiddenField ID="hdfIsMultiplePO" runat="server" />
                <asp:HiddenField ID="hdfSelectedItemPOPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfServicePORequired" runat="server" Value="1" />
                <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsSBUPO" runat="server" Value="0" />
                <asp:HiddenField ID="hdfIsShowCusPoNo" runat="server" Value="0" />
                <asp:HiddenField ID="hdfEnableWO" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
