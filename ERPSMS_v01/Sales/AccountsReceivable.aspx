<%@ Page Title="<%$ Resources:Captions,Title_AccReceivable %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AccountsReceivable.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.AccountsReceivable"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerAcc", true, true, "CUSTOMERLIST");

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
                                <div id="divSBUCompany" class="buttoncontainer-fields floatLeft">
                                    <asp:DropDownList ID="ddlCompany" class="medium margnbotm3" runat="server" TabIndex="1" onmouseover="javascript:ShowTooltip('ddlCompany');">
                                    </asp:DropDownList>
                                </div>
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <%-- <li><span id="spnPOListing" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                TabIndex="1" CssClass="tab-inactive" OnClick="ActionHandler" CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                TabIndex="8" OnClick="ActionHandler" CommandName="SALESINVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                TabIndex="9" OnClick="ActionHandler" CommandName="SALESRECEIPT" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="4" OnClick="ActionHandler" CommandName="CRDRNOTE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                TabIndex="4" OnClick="ActionHandler" CommandName="ACRECEIVABLE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSC %>">
                            <asp:LinkButton runat="server" ID="lbnSOListing" Text="<%$resources:PageNameRes,SalesOrder %>"
                                TabIndex="51" CssClass="tab-inactive" OnClick="ActionHandler" CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDeliveryOrder" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowDO %>">
                            <asp:LinkButton runat="server" ID="lnbDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="52" OnClick="ActionHandler" CommandName="DELIVERYORDER" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbSalesInvoice" Text="<%$resources:PageNameRes,AdvanceInvoice %>"
                                TabIndex="53" OnClick="ActionHandler" CommandName="SALESINVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAdvanceInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbAdvanceInvoice" Text="<%$resources:PageNameRes,SalesInvoice %>"
                                TabIndex="54" OnClick="ActionHandler" CommandName="INVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Spnmiscellaneous" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowMiscInvoice %>">
                            <asp:LinkButton runat="server" ID="lnbMiscellaneous" Text="<%$resources:PageNameRes,miscellaneous %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="35" OnClick="ActionHandler" CommandName="MISC"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnSalesReceipt" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowReceipt %>">
                            <asp:LinkButton runat="server" ID="lbnSalesReceipt" Text="<%$resources:PageNameRes,SalesReceipt %>"
                                TabIndex="55" OnClick="ActionHandler" CommandName="SALESRECEIPT" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowSalesCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="56" OnClick="ActionHandler" CommandName="CRDRNOTE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowAR %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountReceivables %>"
                                TabIndex="57" OnClick="ActionHandler" CommandName="ACRECEIVABLE" CssClass="tab-inactive"></asp:LinkButton>
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
                                           <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half" MaxLength="100" TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerAcc" runat="server" Value="0" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                         <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" CssClass="middle-lbl" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="Uidate-picker input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                             <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" CssClass="middle-lbl-small" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="Uidate-picker input-small"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                             <asp:Label ID="lblSearch" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0" Width="5px"  AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="Search" OnClick="ActionHandler"
                                                TabIndex="4" CommandName="SEARCH" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server"  ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="5" OnClick="ActionHandler" CommandName="CLEAR" Style="margin-bottom: 0px!important;margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>                              
                            </table>                        
                            <div class="gridwrap">
                                <asp:GridView ID="grdAccountPayables" runat="server" AutoGenerateColumns="False"
                                    Width="100%" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                    TabIndex="15" ShowFooter="true" OnRowDataBound="ActionHandler" AllowSorting="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$resources:Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:No %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAcNo" Text='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxNo) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxNo) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Trx %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbltrx" Text='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrx) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrx) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                            <FooterTemplate>
                                                <asp:Label ID="LblCaption" Text="<%$resources:Total %>" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TRXAmt%>" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTRXAmt" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfDebitTC" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxDrAmtTC)%>' />
                                                <asp:HiddenField ID="hdfCreditTC" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxCrAmtTC)%>' />
                                                <asp:HiddenField ID="hdfCurrText" runat="server" Value='<%# Eval(Resources.DataTableRes.FinTrxHdr +"."+Resources.DataTableRes.CurrencyMst1 +"." +Resources.DataFieldRes.Currency) %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="12%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Dr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDr" runat="server" Text='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblDrTotal" runat="server"> </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Cr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCr" runat="server" Text='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount, "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblCrTotal" runat="server"> </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Balance %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblBalanceTotal" runat="server"> </asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="contract" runat="server" />--%>
                </div>
            </div>
            </div>
            <asp:HiddenField ID="hdfRunningBal" runat="server" />
            <asp:HiddenField ID="hdfOpeningBal" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
