<%@ Page Title="<%$ Resources:Captions,Title_AccPayables %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AccountsPayable.aspx.cs" Inherits="ERPSMS_v01.POInvoicing.AccountsPayable"
    Theme="ClassicExt" %>

<%--<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url + "?IsSBUVendor=" + $("[id$='hdfIsSBUVendor']").val(), "hdfVendor", true, true, "VENDOR");

        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtVendor") {
                $("[id$=txtVendor]").attr('title', $("[id$=txtVendor]").val());
            }
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
        function ValidateNow() {

            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {

                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), "Information");
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
                        <li><span id="spnPOListing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPO %>">
                            <asp:LinkButton runat="server" ID="lbnPOListing" Text="<%$resources:PageNameRes,PurchaseOrder %>"
                                TabIndex="1" CssClass="tab-inactive" OnClick="ActionHandler" CommandName="DEFAULT"></asp:LinkButton>
                        </span></li>
                        <%--  <li><span id="spnDirectPurchase" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnDirectPurchase" Text="<%$resources:PageNameRes,DirectPurchase %>"
                                TabIndex="2" CommandName="DIRECTPURCHASE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>--%>
                        <li><span id="spnInvoicing" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseAdvInvoice %>">
                            <asp:LinkButton runat="server" ID="lnkInvoicing" Text="<%$resources:PageNameRes,Invoice %>"
                                TabIndex="3" OnClick="ActionHandler" CommandName="INVOICE" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPOInvoice" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseInvoice %>">
                            <asp:LinkButton runat="server" ID="lbnPOInvoice" Text="<%$resources:PageNameRes,POInvoice %>"
                                TabIndex="4" OnClick="ActionHandler" CommandName="POINVOICE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnExpenses" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowExpense %>">
                            <asp:LinkButton runat="server" ID="lbnExpenses" Text="<%$resources:PageNameRes,Expenses %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="5" OnClick="ActionHandler" CommandName="EXPENSES"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPayment" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPayment %>">
                            <asp:LinkButton runat="server" ID="lnkPayment" Text="<%$resources:PageNameRes,Payment %>"
                                TabIndex="5" OnClick="ActionHandler" CommandName="PAYMENT" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnCrDrNote" runat="server" class="tab-inactive" visible="<%$ resources:ConfigurationsRes,TabShowPurchaseCRDR %>">
                            <asp:LinkButton runat="server" ID="lnbCrDrNote" Text="<%$resources:PageNameRes,CreditDebitNotes %>"
                                TabIndex="6" OnClick="ActionHandler" CommandName="CRDRNOTE" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnAcPayables" runat="server" class="tab-active" visible="<%$ resources:ConfigurationsRes,TabShowAP %>">
                            <asp:LinkButton runat="server" ID="lnbAcPayables" Text="<%$resources:PageNameRes,AccountPayables %>"
                                TabIndex="7" OnClick="ActionHandler" CommandName="ACPAYABLES" CssClass="tab-inactive"></asp:LinkButton>
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
                                              <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                                <asp:TextBox ID="txtVendor" runat="server" CssClass="input-half" MaxLength="100" TabIndex="1"
                                                    ValidationGroup="ap"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfVendor" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="ap" EnableClientScript="true" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtVendor" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Vendor %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdfVendor" runat="server" />
                                                <div class="clear">
                                                </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                          <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                         <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="input-small"
                                            MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;" ValidationGroup="ap"> </asp:TextBox>
                                           <%-- CssClass="Uidate-picker"--%>
                                        <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="ap" EnableClientScript="true" InitialValue="" runat="server"
                                            ControlToValidate="txtFromDate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />

                                        <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate" class="middle-lbl"></asp:Label>
                                        <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-small"
                                            MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;" ValidationGroup="ap"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="ap" EnableClientScript="true" InitialValue="" runat="server"
                                            ControlToValidate="txtToDate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdfToDate" runat="server" Value="" />

                                        <%--<asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                                        <asp:ImageButton ID="btnSearch1" runat="server" Text=""
                                            ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="ap" OnClientClick="javascript:ValidateNow()"
                                            OnClick="ActionHandler" TabIndex="4" CommandName="SEARCH" CommandArgument="SEC_ActionPanel"
                                            SkinID="search-ext" style="margin-bottom:0px!important; margin-top:2px;"/>
                                        <asp:ImageButton ID="btnClear1" runat="server" Text="" ToolTip="<%$ resources:Controls,Clear %>" TabIndex="5" 
                                          OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2" style="margin-bottom:0px!important;" />   
                                        </div>
                                    </td>
                                </tr>                               
                            </table>                             
                            <div class="gridwrap">
                                <asp:GridView ID="grdAccountPayables" runat="server" AutoGenerateColumns="False"
                                    Width="100%"  OnSorting="ActionHandler"
                                    EmptyDataRowStyle-CssClass="emptytable" TabIndex="6" ShowFooter="true" OnRowDataBound="ActionHandler"
                                    AllowSorting="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$resources:Date %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataTableRes.FinTrxHdr +"." +Resources.DataFieldRes.FinTrxDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
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
                                            <ItemStyle Width="33%" />                                            
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="<%$ resources:TRXAmt%>" HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTRXAmt" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfDebitTC" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxDrAmtTC)%>' />
                                                <asp:HiddenField ID="hdfCreditTC" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinTrxCrAmtTC)%>' />
                                                <asp:HiddenField ID="hdfCurrText" runat="server" Value='<%# Eval(Resources.DataTableRes.FinTrxHdr +"."+Resources.DataTableRes.CurrencyMst1 +"." +Resources.DataFieldRes.Currency) %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <ItemStyle Width="14%" HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label ID="LblCaption" Text="<%$resources:Total %>" runat="server"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Dr %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDr" runat="server" Text='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount, "{0:c}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxDrAmount) %>'></asp:Label>
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
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxCrAmount) %>'></asp:Label>
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
                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="ap" runat="server" />
                </div>
            </div>
            </div>
            <asp:HiddenField ID="hdfRunningBal" runat="server" />
            <asp:HiddenField ID="hdfOpeningBal" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
