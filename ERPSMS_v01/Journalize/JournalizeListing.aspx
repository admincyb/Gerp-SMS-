<%@ Page Title="<%$ Resources:Captions,Title_JournalizeListing %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="JournalizeListing.aspx.cs"
    Inherits="ERPSMS_v01.Journalize.JournalizeListing" Theme="ClassicExt" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/Journalize/UserControls/JournalizeControlNew.ascx" TagName="Journalize"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtPVDate");
            if ($("[id$=hdfJournalizeType]").val() == "JV") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtJournalCurrency", url, "hdfJournalCurr", true, true, "CURRENCYCODE");
            }
            //$("[id$=txtJournalExchangeRate]").ForceNumericOnly();
            if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                $('[id$=btnJournalSubmit]').hide();

            if ($('[id$=ddlStatus]').val() == "-1") {
                $('[id$=pnlEditforCancel]').hide();
            }

            if ($("[id$=hdfJournalizeType]").val() == "DPVJ") {
                if ($("[id$=hdfIsReverse]").val() == "1") {
                    $('[id$=btnReverse]').show();
                }
                else {
                    $('[id$=btnReverse]').hide();
                }
            }
            else {
                $('[id$=btnReverse]').hide();
            }


            if ($('[id$=btnReturn]').is(":visible")) {
                if ($('[id$=hdfShowChequeReturn]').val() == "0") {
                    $('[id$=btnReturn]').hide();
                }
            }

            if ($('[id$=btnEditforCancel]').is(":visible")) {
                if ($('[id$=hdfJournalStatus]').val() == "0") {
                    $('[id$=btnEditforCancel]').hide();
                }
            }
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divJournalize]") {
                $("[id$=btnJournalizeUpdate]").click();
            }
            else if (containerID == "#divWkfSubmit") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
                AfterCloseWkfInJournal();
                //$("[id$=btnEdit]").click();
            } else if (containerID == "[id$=divTemplate]") {
                //ShowContainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), '1000', '550');
                ShowCommonCotainerDiv('[id$=divJournalize]', $("[id$=hdfJournalHeader]").val(), "1%");
                if ($('[id$=btnJournalSaveSubmit]').is(":visible"))
                    $('[id$=btnJournalSubmit]').hide();
            }
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {

            if (targetControlID == "txtJournalCurrency") {
                $("[id$=btnCurrencyJV]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteSelect(targetControlID);
            }
        }

        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtJournalCurrency") {
                $("[id$=btnCurrencyJV]").click();
            }
            if (typeof AfterJournalControlAutoCompleteSelect == "function") {
                AfterJournalControlAutoCompleteInvalidSelect(targetControlID);
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

        //        function RemoveValidations() {
        //            //<summary>Function Remove Validation</summary>
        //            var settings = $(document.forms[0]).validate().settings;
        //            delete settings.rules;
        //            delete settings.messages;
        //            settings.rules = {};
        //            settings.messages = {};
        //        }

        //        function ValidateVoucher(valGroup) {
        //            RemoveValidations();
        //            if (typeof (Page_ClientValidate) == 'function') {
        //                //For finding and removing duplicate and other group validation controls
        //                CheckValidationDuplicate(valGroup);
        //                //For Script validating the Page
        //                Page_ClientValidate(valGroup);
        //            }
        //            if (!Page_IsValid) {
        //                $("[id$=litErrorMsg]").hide();
        //                ShowErrorMessage($("#divErrorVoucher").html());
        //                return false;  //Page is invalid -- stop right here
        //            }
        //            else {
        //                //everythings ok --- Call your function & do your stuff
        //                return true;
        //            }
        //        }

        function RoundNum(num, length) {
            var number = Math.round(num * Math.pow(10, length)) / Math.pow(10, length);
            return number;
        }
              function RedirectToComparisonPageFromJoural() {
            window.open("JournalLogVersionComparison.aspx?PK="+$("[id$=hdfSelRowTranPk]").val()+"&Version="+$("[id$=hdfSelRowVer]").val()+"&FromPosting=1");
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlJournalizeListing">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="17"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="14" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="16" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnNewFromTemplate" CommandName="NEWFROMTEMPLATE"
                                            OnClick="ActionHandler" Text="<%$ resources:Controls,New_From_Template %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New_From_Template %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="9" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="<%$resources:CancelVoucher %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:CancelVoucher %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="9" ID="btnReverse" CommandName="REVERSE" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Reverse %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize"
                                            ToolTip="<%$resources:Controls,Reverse %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="9" ID="btnReturn" CommandName="CHEQUERETURN"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Return %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,Return %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="9" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="10" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li style="display: none">
                                        <asp:Button runat="server" TabIndex="11" ID="btnAdvInvPrint" CommandName="ADVANCEINVOICE"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,AdvInvPrint %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,AdvanceInvoice %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="12" ID="btnOffReceiptPrint" CommandName="OFFRECPRINT"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,OffPrint %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" ToolTip="<%$resources:Controls,OfficialReceiptPrint %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="13" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small margnbotm5"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small margnbotm0" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:Status%>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a margnbotm5" TabIndex="3">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                                <%--<asp:ListItem Text="<%$ Resources:Captions,Posted %>" Value="1"></asp:ListItem>--%>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Verified %>" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPartynameMI" runat="server" Text="<%$resources:PartynameMI %>"
                                                AssociatedControlID="txtPartynameMI"></asp:Label>
                                            <asp:TextBox ID="txtPartynameMI" runat="server" TabIndex="5" CssClass="input-halfsmall-a margnbotm0"> </asp:TextBox>
                                        </div>
                                        </td>
                                        <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblJournalizeType" runat="server" Text="<%$resources:JournalizeType %>"
                                                AssociatedControlID="ddlJournalizeType"></asp:Label>
                                            <asp:DropDownList ID="ddlJournalizeType" runat="server" TabIndex="4" Enabled="false"
                                                CssClass="select-half margnbotm0">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfJournalizeType" runat="server" />
                                        </div>
            </td>
             </tr> 
             </table>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S div-separatn">
                            <asp:Label ID="lblVoucherNo" runat="server" Text="<%$resources:VoucherNo %>" AssociatedControlID="txtVoucherNo"></asp:Label>
                            <asp:TextBox ID="txtVoucherNo" runat="server" TabIndex="5" CssClass="input-small margnbotm0"> </asp:TextBox>
                            </div>
                            </td>
                            <td>
                            <div class="div2col-S div-separatn">
                            <asp:Label ID="lblRef" runat="server" Text="<%$resources:RefNo %>" AssociatedControlID="txtRefNo"></asp:Label>
                            <asp:TextBox ID="txtRefNo" runat="server" TabIndex="6" CssClass="input-small margnbotm0"> </asp:TextBox>
                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                OnClick="ActionHandler" TabIndex="7" CommandName="SEARCH" SkinID="search-ext"
                                CssClass="margntop2 margnbotm0" />
                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                TabIndex="7" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                        </div>
                    </td>
                    
                </tr>
            </table>
            <div class="gridwrap">
                <asp:GridView runat="server" ID="grdJournalizeList" Width="100%" PageSize="<%$ resources:PageSize%>"
                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true"
                    OnCheckedChanged="ActionHandler">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                    OnCheckedChanged="ActionHandler" />
                                <asp:HiddenField runat="server" ID="hdfTrxPK" Value='<%# Eval(Resources.DataFieldRes.TrxPK) %>' />
                                <asp:HiddenField runat="server" ID="hdfRefPK" Value='<%# Eval(Resources.DataFieldRes.RefPK) %>' />
                                <asp:HiddenField runat="server" ID="hdfRefType" Value='<%# Eval(Resources.DataFieldRes.JournalizeType) %>' />
                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("FTH_DEPT") %>' />
                                <asp:HiddenField runat="server" ID="hdfCompany" Value='<%# Eval("FTH_COMPANY") %>' />
                                <asp:HiddenField runat="server" ID="hdfIsDeleted" Value='<%# Eval("FTH_IS_DELETED") %>' />
                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("FTH_STATUS") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:JournalizeDate %>" SortExpression="<%$ resources:DataFieldRes,JournalizeDate %>">
                            <ItemTemplate>
                                <asp:Label ID="lblJournalizeDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.JournalizeDate, Resources.Constants.DateFormatGrid) %>'
                                    ToolTip='<%# Eval(Resources.DataFieldRes.JournalizeDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>" SortExpression="<%$ resources:DataFieldRes,VoucherNo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval(Resources.DataFieldRes.VoucherNo))) ? "[NEW]" : Eval(Resources.DataFieldRes.VoucherNo)%> '
                                    ToolTip='<%# Eval(Resources.DataFieldRes.VoucherNo)%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="true" HeaderText="<%$ resources:PartynameMI %>" SortExpression="<%$ resources:DataFieldRes,Partyname %>">
                            <ItemTemplate>
                                <div style="width: 100%; display: inline-block;">
                                    <div style="float: left; width: 93%;">
                                        <asp:Label ID="lblPartyname" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval(Resources.DataFieldRes.Partyname))) ,40) %>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Convert.ToString(Eval(Resources.DataFieldRes.Partyname))),600)%>'></asp:Label>
                                    </div>
                                    <div style="float: right; width: 7%;">
                                        <%--<asp:Button ID="btnPDCFlag" runat="server" OnClientClick="javascript:return false;"
                                                            Visible='<%# Eval(Resources.DataFieldRes.FTH_PDC) == null ? false : Convert.ToInt32(Eval(Resources.DataFieldRes.FTH_PDC).ToString()) >= 1 ? true : false %>' />--%>
                                        <asp:Button ID="btnPDCFlag" runat="server" OnClientClick="javascript:return false;"
                                            Visible='<%# (Eval("FTH_BOUNCED").ToString() == "1" ? false : (Eval(Resources.DataFieldRes.FTH_PDC) == null ? false : Convert.ToInt32(Eval(Resources.DataFieldRes.FTH_PDC).ToString()) >= 1 ? true : false)) %>' />
                                        <asp:Button ID="btnPDCReturn" runat="server" OnClientClick="javascript:return false;"
                                            CssClass='<%#(Eval("FTH_BOUNCED").ToString() != "0" ? "return-icon" : "")  %>'
                                            ToolTip='<%# (Eval("FTH_BOUNCED").ToString() != "0" ? GetLocalResourceObject("Cheque_Return").ToString() : "") %>'
                                            Visible='<%# (Eval("FTH_BOUNCED").ToString() != "0" ? true : false) %>' />
                                        <asp:HiddenField ID="hdfTrxFlag" runat="server" Value='<%# Eval(Resources.DataFieldRes.FTH_STATUS) == null ? 0 : Eval(Resources.DataFieldRes.FTH_STATUS)%>' />
                                        <asp:HiddenField ID="hfPdcStatus" runat="server" Value='<%# Eval(Resources.DataFieldRes.FTH_PDC) == null ? 0 : Eval(Resources.DataFieldRes.FTH_PDC)%>' />
                                    </div>
                                </div>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:RefNo %>" SortExpression="<%$ Resources:DataFieldRes,FinTrxNo%>">
                            <ItemTemplate>
                                <asp:Label ID="lblJournalizeType" runat="server" Text='<%# Eval(Resources.DataFieldRes.FinTrxNo) %>'
                                    ToolTip='<%# Eval(Resources.DataFieldRes.FinTrxNo)%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval(Resources.DataTableRes.CurrencyMst1 +"." + Resources.DataFieldRes.CurrencyCode)  %>'
                                    ToolTip='<%# Eval(Resources.DataTableRes.CurrencyMst1 +"." + Resources.DataFieldRes.CurrencyCode)  %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='' ToolTip=''></asp:Label>
                                <asp:HiddenField ID="hdfJCurrency" runat="server" Value='<%# Eval(Resources.DataFieldRes.FinBaseCurr) %>' />
                            </ItemTemplate>
                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="amount-numeric" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lblHdrAmountBaseCur" runat="server" Text='' ToolTip=''></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmountBaseCur" runat="server" Text='' ToolTip=''></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="21%" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="amount-numeric" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.TrxApproved) %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="imgPosted" runat="server" OnClientClick="javascript:return false;" />
                                <asp:HiddenField runat="server" ID="hdfPosted" Value='<%# Eval(Resources.DataFieldRes.TrxPosted) %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgHistory" runat="server" SkinID="imbArchive" OnClick="ActionHandler"
                                    CommandName="VERSIONHISTORY" ToolTip='<%$ resources:PrevVer %>' Visible=' <%# Convert.ToInt32(Eval("FTH_VERSION")) > 0 ? true : false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc1:PagerControl ID="uclPaging" runat="server" />
            </div>
            </asp:TableCell> </asp:TableRow> </asp:Table> </div>
            <div id="divTemplateList" style="display: none">
                <div class="Button-container-popup">
                    <asp:Button ID="btnLoadTemplate" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Select %>"
                        ToolTip="<%$resources:Controls,Select %>" OnClick="ActionHandler" TabIndex="54"
                        CommandName="LOADFROMTEMPLATE" CommandArgument="ucrJournalize" />
                </div>
                <div class="content-wrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-P">
                                    <asp:Label runat="server" ID="lblTemplateCategoryList" AssociatedControlID="ddlTemplateCategoryList"
                                        Text="<%$resources:Category %>"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlTemplateCategoryList" OnSelectedIndexChanged="ActionHandler"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-P">
                                    <asp:Label ID="lblTemplates" runat="server" Text="<%$resources:TemplateName %>" AssociatedControlID="ddlTemplates"></asp:Label>
                                    <asp:DropDownList ID="ddlTemplates" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <%--  <asp:ValidationSummary ID="vsPage" ValidationGroup="invoice" runat="server" />--%>
            </div>
            <div id="divHistory" style="display: none">
                <div class="Button-container-popup" style="display: none;">
                    <asp:Button ID="btnHistoryPrint" SkinID="btnInner-add-dsd" runat="server" Text="<%$resources:Controls,Print %>"
                        ToolTip="<%$resources:Controls,Print %>" OnClick="ActionHandler" CommandName="HISTORYPRINT" />
                </div>
                <div class="content-wrapper">
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdVersionHistory" Width="100%" PageSize="<%$ resources:PageSize%>"
                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Version %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbnHstryVersion" runat="server" Text='<%# Eval("FTH_VERSION") %>'
                                            CssClass="text-underline" ToolTip='<%# Eval("FTH_VERSION") %>' OnClick="ActionHandler"
                                            CommandName="HISTORYPRINT"></asp:LinkButton>
                                        <asp:HiddenField ID="hdfHstryCompany" runat="server" Value='<%# Eval("FTH_COMPANY")%>' />
                                        <asp:HiddenField ID="hdfHstryRefPk" runat="server" Value='<%# Eval("FTH_REF_PK")%>' />
                                        <asp:HiddenField ID="hdfHstryRefType" runat="server" Value='<%# Eval("FTH_REF_TYPE")%>' />
                                        <asp:HiddenField ID="hdfHstryVoucherPK" runat="server" Value='<%# Eval("FTH_PK")%>' />
                                        <asp:HiddenField ID="hdfHstryVersion" runat="server" Value='<%# Eval("FTH_VERSION")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="1%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbnHstryVoucherNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.VoucherNo) %>'
                                            CssClass="text-underline" ToolTip='<%# Eval(Resources.DataFieldRes.VoucherNo) %>'
                                            OnClick="ActionHandler" CommandName="HISTORYPRINT"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:JournalizeDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHstryJournalizeDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.JournalizeDate, Resources.Constants.DateFormatGrid) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.JournalizeDate, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:CreatedDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHstryCrtdDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.FTH_CRTD_DT, Resources.Constants.DateTimeFormatGrid) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.FTH_CRTD_DT, Resources.Constants.DateTimeFormatGrid)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:CreatedBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHstryCrtdBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FTH_CRTD_BY_TEXT"))%>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FTH_CRTD_BY_TEXT"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ModifiedDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHstryModDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.FTH_MOD_DT, Resources.Constants.DateTimeFormatGrid) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.FTH_MOD_DT, Resources.Constants.DateTimeFormatGrid)%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ModifiedBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHstryModBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FTH_MOD_BY_TEXT"))%>'
                                            ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("FTH_MOD_BY_TEXT"))%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrency" runat="server" />
            <asp:HiddenField ID="hdfJournalHeader" runat="server" />
            <asp:HiddenField ID="hdfIsReverse" runat="server" Value="0" />
            <asp:HiddenField ID="hdfReturnTye" runat="server" />
            <asp:HiddenField ID="hdfShowChequeReturn" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfJournalStatus" Value="0" />
            <asp:Button ID="btnJournalizeUpdate" runat="server" OnClick="ActionHandler" CommandName="JOURNALIZEUPDATE"
                EnableTheming="false" Style="display: none" />
            <%--User Control--%>
            <div id="divJournalize" style="display: none">
                <uc1:Journalize ID="ucrJournalize" runat="server" />
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server">
                </uc1:WorkflowUserComments>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
