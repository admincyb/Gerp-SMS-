<%@ Page Title="<%$ Resources:Captions,Title_SaleOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="SaleOrderList.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.SaleOrderList" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
         //   alert("");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            //         GrandScriptUtils.AddDateRange("txtActFromDate", "hdfActFromDate", "txtActToDate", "hdfActToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");
            if ($("[id$=hdfCustomerID]").val() != "") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url + "?CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSoPK", true, true, "SONUMBERAPPROVED");
                GrandScriptUtils.MakeAutoCompleteDDL("txtPONumber", url + "?CustomerID=" + $("[id$=hdfCustomerID]").val(), "hdfSoPK", true, true, "CUSPONUMBERCUSTOMERWISE");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url, "hdfSoPK", true, true, "SONUMBERAPPROVED");
                GrandScriptUtils.MakeAutoCompleteDDL("txtPONumber", url, "hdfSoPK", true, true, "CUSPONUMBERCUSTOMERWISE");
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

        function ShippingPlanAlreadyCreated() {

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= GetLocalResourceObject("Err_ShpngAlreadyCreated").ToString() %>';
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
                        $("[id$=btnPickForDO]").click();
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


        //For Setting/Resetting Colour of a selected Row
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdSoList.ClientID %> input[type=hidden][id*=hdfSOID]").each(function (index) {
                        if ($.trim($(this).val()) == selectedIdsArray[i]) {
                            var selectedRowColor;
                            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';
                            $(this).closest('tr').css('background-color', selectedRowColor);

                        }

                    });
                }

            }
        }
        //End

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
                                        <asp:Button runat="server" ID="btnPickForDO" CommandName="PICKFORDO" TabIndex="14"
                                            Text="<%$resources:PickSoForSP %>" OnClick="ActionHandler" ToolTip="<%$resources:PickSoForSP %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <%-- <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnPickForInvoice" CommandName="PICKFORINVOICING"
                                            Visible="false" TabIndex="14" Text="<%$resources:PickSoForInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickSoForInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlInv">
                                        <asp:Button runat="server" ID="btnPickForAdvInv" CommandName="PICKFORADVANCEINVOICING"
                                            TabIndex="14" Text="<%$resources:PickSoForAdvanceInvoicing %>" OnClick="ActionHandler"
                                            ToolTip="<%$resources:PickSoForAdvanceInvoicing %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>--%>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESET" TabIndex="14"
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
                        <li><span id="spnPOListing" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbShippingPlan" Text="<%$resources:PageNameRes,ShippingPlan %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="SHIPPINGPLAN"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <%--  <li><span id="spnSalesInvoice" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                TabIndex="8" OnClick="ActionHandler" CommandName="SALESINVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                TabIndex="8" OnClick="ActionHandler" CommandName="INVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                TabIndex="9" OnClick="ActionHandler" CommandName="SALESRECEIPT" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="4" OnClick="ActionHandler" CommandName="CRDRNOTE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                TabIndex="4" OnClick="ActionHandler" CommandName="ACRECEIVABLE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--  //For SelectedItemId Keeping--%>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
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
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" CssClass="middle-lbl-small-d" Text="<%$resources:ToDate %>"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" CssClass="middle-lbl-small"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-d" TabIndex="3">
                                                <%--   <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="0"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Completed %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Pending %>" Value="9"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblCompanyFilter" runat="server" Text="<%$resources:Controls,CompanyPlant%>"
                                                CssClass="middle-lbl-xsmall-19-11" AssociatedControlID="ddlCompanyFilter"></asp:Label>
                                            <asp:DropDownList ID="ddlCompanyFilter" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlVoucherCompany');"
                                                CssClass="select-small-a1-19-11">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$resources:SONo %>" CssClass="middle-lbl-small"
                                                AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="select-small-c1 margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>
                                                
                                            <asp:Label ID="lblCustomerPo" runat="server" Text="<%$resources:CustPo %>" CssClass="middle-lbl-small"
                                                AssociatedControlID="txtPONumber"></asp:Label>
                                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="select-small-c1 margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>

                                            <asp:HiddenField ID="hdfSoPK" runat="server" Value="" />
                                            <asp:HiddenField ID="hdfPurPk" runat="server" Value="" />
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="6" CommandName="SEARCH"
                                                SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="7" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap maxh-290" id="divSO_ScrollContainer" grid="grdSoList">
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <cc1:extgridview runat="server" id="grdSoList" autogeneratecolumns="False" width="100%"
                                    expandbuttoncssclass="GridExpandCollapseButton" collapsebuttoncssclass="GridExpandCollapseButton"
                                    gridlines="None" expandbuttontext="+" collapsebuttontext="-" emptydatarowstyle-cssclass="emptytable"
                                    showfooter="true" onrowdatabound="ActionHandler" pagesize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="24" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfSOID" Value='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>' />
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQty" Value='<%# Eval(Resources.DataFieldRes.OrderQty) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQtyDispatched" Value='<%# Eval(Resources.DataFieldRes.SodQtyDispatched) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQtyReturned" Value='<%# Eval(Resources.DataFieldRes.SodQtyReturned) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSodQtyInvoiced" Value='<%# Eval(Resources.DataFieldRes.SOInvoicedQty) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>
                                                <%--<asp:Label ID="lblSONo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlantCode" Font-Bold="true" Text="<%# Eval(Resources.DataFieldRes.CompanySpecsCode) %>"
                                                    ToolTip="<%# Eval(Resources.DataFieldRes.CompanySpecs) %>" runat="server"
                                                    CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SODate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:CustPo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustPo" runat="server" Text='<%# Eval(Resources.DataFieldRes.CustPo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CustPo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="<%$ resources:ShowCustCode %>" HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CusCode),23) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CustomerName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="<%$ resources:ShowCustName %>" HeaderText="<%$ resources:CustomerName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CustomerName),47) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CustomerName) %>'></asp:Label>
                                                <%--     <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SOCustomerPK) %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval(Resources.DataFieldRes.SOType) %>'
                                                    ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval( Resources.DataFieldRes.SohToPort),25) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SohToPort),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalPcs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalPcs" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodTotalPieces) %>' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalCtn %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalCtn" runat="server" Text='<%# Eval(Resources.DataFieldRes.TotalCarton) %>' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataFieldRes.CurrencyCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.CurrencyCode) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderTotal, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrderTotal, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SaleOrderStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxSts" Value='<%# Eval(Resources.DataFieldRes.TrxSts) %>' />
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.SOCurrency) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="4%" />
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
                                                                    <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemCode),13) %>'
                                                                        ToolTip='<%# Eval(Resources.DataTableRes.ItemMst + "." + Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" />
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
                                                                <ItemStyle Width="58%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="<%$ resources:DesiredDeliveryDate %>"
                                                                SortExpression="<%$ resources:DataFieldRes,SodDesiredDeliveryDate %>">
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
                                                                <ItemStyle Width="5%" />
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
                                                            <asp:TemplateField HeaderText="<%$ resources:PlandQty %>" SortExpression="<%$ resources:DataFieldRes,Planned %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblplnQty" runat="server" Text='0' ToolTip='0'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvoicedQty %>" SortExpression="<%$ resources:DataFieldRes,SOInvoicedQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoicedQty" runat="server" Text='<%#String.Format("{0:n}", Convert.ToDouble(Eval(Resources.DataFieldRes.SOInvoicedQty))/ ((Eval(Resources.DataFieldRes.SodSaleUOMConvFactor) == DBNull.Value) ? 1 : Convert.ToDouble(Eval(Resources.DataFieldRes.SodSaleUOMConvFactor)))) %>'
                                                                        ToolTip='<%#String.Format("{0:n}", Convert.ToDouble(Eval(Resources.DataFieldRes.SOInvoicedQty))/ ((Eval(Resources.DataFieldRes.SodSaleUOMConvFactor) == DBNull.Value) ? 1 : Convert.ToDouble(Eval(Resources.DataFieldRes.SodSaleUOMConvFactor)))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UnitCost %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitCost" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodUnitCost, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodUnitCost, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="<%$ resources:DataFieldRes,SodAmount %>"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SodAmount, "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
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
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle CssClass="table-firstlevel-Unimport" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:extgridview>
                                <uc1:pagercontrol id="uclPaging" runat="server" visible="false" />
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
                                        <cc1:extgridview runat="server" id="grdDOHdr" autogeneratecolumns="False" expandbuttoncssclass="GridExpandCollapseButton"
                                            collapsebuttoncssclass="GridExpandCollapseButton" gridlines="None" expandbuttontext="+"
                                            collapsebuttontext="-" emptydatarowstyle-cssclass="emptytable" showfooter="true"
                                            pagesize="<%$ resources:PageSize %>">
                                    <%--OnRowDataBound="ActionHandler"--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:GonNumber %>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDoNumber" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'
                                                    OnClick="ActionHandler" CommandName="SHOW" CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:LinkButton>
                                                <%--<asp:Label ID="lblDoNumber" runat="server" Text='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DeliveryOrderNo) %>'></asp:Label>--%>
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
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipTo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipTo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ShipTo),50) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ShipTo),300) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# Eval(Resources.DataTableRes.ShipByMaster + "." +Resources.DataFieldRes.ConstName) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.ShipByMaster + "." +Resources.DataFieldRes.ConstName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
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
                                                                <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'
                                                                        ToolTip='<%# Eval("CRM_CUST_ITEM_MAP.CIM_BRAND_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="56%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Size">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSize" runat="server" Text='<%# GetConstName(Eval("INV_ITEM_MST")) %>'
                                                                        ToolTip='<%# GetConstName(Eval("INV_ITEM_MST")) %>'>
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
                                                                <ItemStyle Width="6%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("SAL_ORDER_DTL.SOD_QTY", "{0:c}") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:DelQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDel" runat="server" Text='<%# Eval("DPD_QTY_DESPATCHED", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("DPD_QTY_DESPATCHED", "{0:c}") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BalQty %>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBal" runat="server" Text='<%#  Convert.ToString(Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) > 0 ? Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) : 0) %>'
                                                                        ToolTip='<%#  Convert.ToString(Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) > 0 ? Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY")) - Convert.ToDouble(Eval("SAL_ORDER_DTL.SOD_QTY_DISPATCHED")) : 0) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
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
                                </cc1:extgridview>
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
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
