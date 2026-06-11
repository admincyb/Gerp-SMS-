<%@ Page Title="<%$ Resources:Captions,Title_AgentManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="SaleOrderForAgtComm.aspx.cs"
    Inherits="ERPSMS_v01.Sales.SaleOrderForAgtComm" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?SearchBy=" + "1", "hdfCustomerID", true, true, "GETAUTOCSI"); //"GETAUTOAGT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSONumber", url + "?SearchBy=" + "2", "hdfSoPK", true, true, "GETAUTOCSI"); //"GETAUTOAGT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtInvoiceNumber", url + "?SearchBy=" + "3", "hdfIVHPK", true, true, "GETAUTOCSI"); //"GETAUTOAGT");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtAgent", url, "hdfAgent", true, true, "VENDOR"); GETAGENTLIST
            GrandScriptUtils.MakeAutoCompleteDDL("txtAgent", url, "hdfAgent", true, true, "GETAGENTLIST"); 

        }
        function ShowHideAdvancedSearch(flag) {
            //            If flag then Show AdvancedSearch
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

            //For checking if validation control in Array of present validations
            function CheckValidationExists(id) {
                for (var i in validationArrayGroup) {
                    if (validationArrayGroup[i] == id) {
                        return true;
                    }
                }
                return false;
            }



        }
        function ResetSOSelection() {
            $('[id$=grdInvoiceList]').find('tr td input:radio[id$=rbtSelect]').removeAttr('checked');
        }
        //For Setting/Resetting Colour of a selected Row
        function SetSelectedRowColor() {
            var selectedIds;
            var selectedIdsArray = new Array();
            selectedIds = $("[id$=hdfSelectedItemPk]").val();
            selectedIdsArray = selectedIds.split(',');

            for (i = 0; i < selectedIdsArray.length; ++i) {

                if (selectedIdsArray[i] != 0) {
                    $("#<%= grdInvoiceList.ClientID %> input[type=hidden][id*=hdfInvPK]").each(function (index) {
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
                                    <li runat="server" id="pnlInvComm">
                                        <asp:Button runat="server" ID="btnPickForInvComm" CommandName="PICKFORAGENTCOMM"
                                            TabIndex="14" Text="<%$ resources:PickSoForAdvanceInvoicing %>" ToolTip="<%$ resources:PickSoForAdvanceInvoicing %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlResetSelection">
                                        <asp:Button runat="server" ID="btnResetSelection" CommandName="RESETBTN" TabIndex="14"
                                            Text="<%$ resources:ResetSel %>" OnClick="ActionHandler" ToolTip="<%$ resources:ResetSel %>"
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
                                CommandArgument="SEC_ActionPanel" TabIndex="12" CssClass="tab-inactive" OnClick="ActionHandler"
                                CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,AGTINVOICE %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="13" OnClick="ActionHandler" CommandName="AGTINVOICE"
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2">
                                <tr id="Tr1" runat="server">
                                    <td runat="server">
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate %>" CssClass="middle-lbl-small-d"
                                                AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <%--<asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="medium" TabIndex="5">
                                                <asp:ListItem Value="1"></asp:ListItem>
                                                <asp:ListItem Selected="True" Value="0"></asp:ListItem>
                                                <asp:ListItem Value="2"></asp:ListItem>
                                            </asp:DropDownList>--%>
                                        </div>
                                    </td>
                                    <td runat="server">
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="<%$resources:InvNo %>" AssociatedControlID="txtInvoiceNumber"></asp:Label>
                                            <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input-small-a" MaxLength="100"
                                                TabIndex="3"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfIVHPK" runat="server" Value="" />
                                            <asp:Label ID="lblAgent" CssClass="middle-lbl-a" runat="server" Text="<%$resources:Agent %>"
                                                AssociatedControlID="txtAgent"></asp:Label>
                                            <asp:TextBox ID="txtAgent" runat="server" MaxLength="100" TabIndex="1" CssClass="input-small margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfAgent" runat="server" Value="" />
                                            <%--<div class="clear">
                                            </div>
                                            <asp:Label ID="lblInType" runat="server" Text="<%$ resources:Type %>" 
                                                AssociatedControlID="ddlInvoiceType"></asp:Label>--%>
                                            <%--<asp:DropDownList ID="ddlInvoiceType" runat="server" TabIndex="6" CssClass="medium"></asp:DropDownList>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide ">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer  %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="4"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSONumber" runat="server" Text="<%$ resources:scno %>" AssociatedControlID="txtSONumber"></asp:Label>
                                            <asp:TextBox ID="txtSONumber" runat="server" CssClass="input-small-a margnbotm0"
                                                MaxLength="100" TabIndex="5"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSoPK" runat="server" />
                                            <asp:Label ID="lblSearch" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"
                                                AssociatedControlID="btnSearch"></asp:Label>
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
                            <div class="gridwrap scroll-container">
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <asp:GridView runat="server" ID="grdInvoiceList" Width="1400px" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="true" OnPageIndexChanging="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="11" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hdfInvPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.SalesInvoicePK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="0.5px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:scno %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblscno" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SISONo),13) %>'
                                                    ToolTip='<%#  Eval(Resources.DataFieldRes.SISONo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--SONo--%>
                                            <ItemStyle Width="5px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POHNO %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpohno" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.PONumber),13) %>'
                                                    ToolTip='<%#  Eval(Resources.DataFieldRes.PONumber) %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--SONo--%>
                                            <ItemStyle Width="6px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval(Resources.DataFieldRes.SICustomerCode),38) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SalesInvoiceCustomerText),250) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.InvCusPK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="14px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(( Convert.ToInt16(Eval("ICH_SO_TYPE"))==1?"Domestic":"Export") ,3,"") %>'
                                                    ToolTip='<%# Convert.ToInt16(Eval("ICH_SO_TYPE"))==1?"Domestic":"Export"  %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTypePK" Value='<%# Eval("ICH_SO_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Agent %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgent" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval( Resources.DataFieldRes.AgentComm) ,21)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.AgentComm),300) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfAgtPK" Value='<%# Eval("ICH_SO_AGENT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotQty %>">
                                            <ItemTemplate>
                                                <%-- <asp:Label ID="lblTotCtn" runat="server" Text='<%# GetFormattedNumberWithSeperator(Eval(Resources.DataFieldRes.TotalCrtn))+"            "+Eval("UOM_NAME")%>'
                                                    ToolTip='<%# GetFormattedNumberWithSeperator(Eval(Resources.DataFieldRes.TotalCrtn)) %>'></asp:Label>--%>
                                                <asp:Label ID="lblTotCtn" runat="server" Text='<%# GetFormattedNumberWithSeperator(Eval(Resources.DataFieldRes.TotalCrtn))%>'
                                                    ToolTip='<%# GetFormattedNumberWithSeperator(Eval(Resources.DataFieldRes.TotalCrtn)) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Label ID="lbluom" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("UOM_NAME"),4)%>'
                                                 ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("UOM_NAME"),300) %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8px" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cur %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCur" runat="server" Text='<%# Eval( Resources.DataFieldRes.CommCurrTex) %>'
                                                    ToolTip='<%# Eval(  Resources.DataFieldRes.CommCurrTex) %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfSOCurrency" Value='<%# Eval(Resources.DataFieldRes.CommCurr) %>' />
                                                <%-- <asp:Label ID="lblCur" runat="server" Text='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.TotalSOAmt, "{0:c}") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="4px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:scAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblscAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.SoAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SoAmt, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ACINVOICED %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblacinvoiced" runat="server" Text='<%# Eval(Resources.DataFieldRes.ACINVOICED, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.ACINVOICED, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle Width="2px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvNo %>">
                                            <ItemTemplate>
                                                <%-- <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONo) %>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SONo) %>'></asp:LinkButton>--%>
                                                <asp:Label ID="lblInvNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfInvNo" Value='<%# Eval(Resources.DataFieldRes.SalesInvoiceNo) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SalesInvoiceDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9px" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:invAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.InvAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.InvAmt, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:rvdAmt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrvdAmt" runat="server" Text='<%# Eval(Resources.DataFieldRes.RcvdAmt, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.RcvdAmt, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="<%$ resources:TotPcs %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotPcs" runat="server" Text='<%# GetFormattedNumberWithSeperator(Eval(  Resources.DataFieldRes.TotalPcs1)) %>'
                                                    ToolTip='<%# GetFormattedNumberWithSeperator(Eval( Resources.DataFieldRes.TotalPcs1)) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                        </asp:TemplateField>--%>
                                         <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server"/>
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.AcSaleOrderStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel-Unimport" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            <%--                            <div class="gridwrap hierarchical-wrap" id="divSO_ScrollContainer" grid="grdSoList">
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <gridview></gri>
                                <cc1:ExtGridView runat="server" ID="grdSoList" AutoGenerateColumns="False" Width="100%"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                      
                                    <RowStyle CssClass="table-firstlevel-Unimport" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            --%>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"
                        meta:resourcekey="litErrorMsgResource1"></asp:Label>
                </div>
                <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
            </div>
            <asp:HiddenField ID="hdfgroup" runat="server" Value="1" />
            <asp:HiddenField ID="hdfDecimalFormatWithSeperator" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
