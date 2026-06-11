<%@ Page Title="<%$ Resources:Captions,Title_OutStandingDue %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/ERPSMS_2.Master"
    CodeBehind="OutStandingDue.aspx.cs" Inherits="CustomerPortal.OrderToCash.OutStandingDue" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">

        function InitComponents() {
            $('[id$=ChkSelectAll]').click(function () {
                $("[id$='ChkSelect']").attr('checked', this.checked);
            });
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerPK", true, true, "CUSTOMER");
        }

        $(window).load(function EndRequest() {
            //  FormatCalendar('4');

        });


        var ControlID = 'calendar1';
        function EndRequest() { //FormatCalendar('4'); 
        }
        //    function FormatCalendar(type) {
        //        var ctrlBehaviourarray = ControlID.split('|');
        //        for (var i = 0; i < ctrlBehaviourarray.length; i++) {
        //            var calenderCtrl = $find(ctrlBehaviourarray[i]);
        //            if (calenderCtrl) {
        //                switch (type) {
        //                    case "6":
        //                        return;
        //                        break;
        //                    case "4":
        //                        $(calenderCtrl).attr('CalenderType', '2');
        //                        modifyMontDelegates(calenderCtrl);
        //                        break;
        //                    case "1":
        //                        $(calenderCtrl).attr('CalenderType', '3');
        //                        modifyYearDelegates(calenderCtrl);
        //                        break;
        //                }
        //            }
        //        }
        //    }
        function modifyMontDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":


                            var _currentDay = new Date(new Date().getFullYear(), new Date().getMonth(), 0);
                            var _SelectedDay = new Date(target.date.getFullYear(), target.date.getMonth(), 0);

                            if (_currentDay <= _SelectedDay) {
                                //if the mode is month, then stop switching to day mode.
                                if (target.month == visibleDate.getMonth()) {
                                    //this._switchMode("days");
                                } else {
                                    cal._visibleDate = target.date;
                                    //this._switchMode("days");
                                }
                                cal.set_selectedDate(target.date);
                                cal._switchMonth(target.date);
                                cal._blur.post(true);
                                cal._visibleDate = target.date;
                                cal.raiseDateSelectionChanged();
                            }
                            else {
                                msg = '<ul><li><%= GetLocalResourceObject("Err_SelectMonth") %></li></ul>';
                                $("[id$=litErrorMsg]").show();
                                $("[id$=litErrorMsg]").html(msg);
                                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                            }
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;
                        // case "day":                                                                              
                        // this.set_selectedDate(target.date);                                                                              
                        // this._switchMonth(target.date);                                                                              
                        // this._blur.post(true);                                                                              
                        // this.raiseDateSelectionChanged();                                                                              
                        // break;                                                                              
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }
        function modifyYearDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        // case "month":                                                                 
                        // //if the mode is month, then stop switching to day mode.                                                                 
                        // if (target.month == visibleDate.getMonth()) {                                                                 
                        // //this._switchMode("days");                                                                 
                        // } else {                                                                 
                        // cal._visibleDate = target.date;                                                                 
                        // //this._switchMode("days");                                                                 
                        // }                                                                 
                        // cal.set_selectedDate(target.date);                                                                 
                        // cal._switchMonth(target.date);                                                                 
                        // cal._blur.post(true);                                                                 
                        // cal.raiseDateSelectionChanged();                                                                 
                        // break;                                                                 
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                // cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                //cal._switchMode("months");
                            }
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        // case "day":                                                                 
                        // this.set_selectedDate(target.date);                                                                 
                        // this._switchMonth(target.date);                                                                 
                        // this._blur.post(true);                                                                 
                        // this.raiseDateSelectionChanged();                                                                 
                        // break;                                                                 
                        case "today":
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function changeMonthCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function changeYearCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }

        function onCalendarShown(cal, args) {
            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden(sender, args) {
            //                        if (sender.get_selectedDate()) {
            //                            if (sender.get_selectedDate() && sender.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            //                                alert('The "From" Date should smaller than the "To" Date, please reselect!');
            //                                sender.show();
            //                                return;
            //                            }
            //                            //get the final date
            //                            var finalDate = new Date(sender.get_selectedDate());
            //                            var selectedMonth = finalDate.getMonth();
            //                            finalDate.setDate(1);
            //                            if (sender == cal2) {
            //                                // set the calender2's default date as the last day
            //                                finalDate.setMonth(selectedMonth + 1);
            //                                finalDate = new Date(finalDate - 1);
            //                            }
            //                            //set the date to the TextBox
            //                            sender.get_element().value = finalDate.format(sender._format);
            //                        }
        }

        $(document).ready(function () {
            $('[id$=ChkSelectAll]').click(function () {
                $("[id$='ChkSelect']").attr('checked', this.checked);
            });
        });

        function checkDate(sender, args) {
            //        if (sender._selectedDate < new Date()) {
            //            //alert("You cannot select a day earlier than today!");
            //            sender._selectedDate = new Date();
            //            // set the date back to the current date
            //            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            //        }
        }

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

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

    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlListing" runat="server">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="SENDMAIL" Text="<%$ resources:SendMail %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:SendMail %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <%--<li id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="6" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <%-- <div class="search-wrap" style="text-align: left;">
                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:Date %>" AssociatedControlID="txtCalender"></asp:Label>
                                <asp:Label ID="lblSStatus" runat="server" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="1">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Due</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="2" />
                            </div>--%>
                            <%--New Search Starts--%>
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
                                                TabIndex="0" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="0" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="input-half" MaxLength="100"
                                                TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:HiddenField ID="hdfCustomerPK" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:Date %>" CssClass="lbl-13perc"
                                                AssociatedControlID="txtCalender"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCalender" ClientIDMode="Static" CssClass="input-small"
                                                TabIndex="1" MaxLength="11" onkeydown="return false;" onpaste="return false;"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                OnClientDateSelectionChanged="checkDate" TargetControlID="txtCalender" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden" Format="<%$ resources:Constants,DateFormatShort %>">
                                            </cc1:CalendarExtender>
                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:InvoiceNo %>" CssClass="lbl-13perc"
                                                AssociatedControlID="txtInvoiceNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInvoiceNo" ClientIDMode="Static" CssClass="input-small-b"
                                                MaxLength="50" TabIndex="1"></asp:TextBox>
                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:Status %>" CssClass="lbl-11-3perc"
                                                AssociatedControlID="ddlStatus2"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus2" runat="server" TabIndex="1" CssClass="select-small">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Due</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch2" CssClass="middle-lbl-xsmall-d style-none"></asp:Label>
                                            <asp:ImageButton ID="btnSearch2" SkinID="search-ext" runat="server" Text="<%$ resources:Controls,Go %>"
                                                ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                                TabIndex="1" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label runat="server" ID="lblCC" AssociatedControlID="lblCCAddress" Text="<%$ resources:Cc %>"></asp:Label>
                                            <asp:Label runat="server" ID="lblCCAddress" Text="" CssClass="hauto"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblBCC" AssociatedControlID="lblBCCAddress" Text="<%$ resources:Bcc %>"></asp:Label>
                                            <asp:Label runat="server" ID="lblBCCAddress" Text="" CssClass="hauto"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%-- New Search Ends--%>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdOutStandingDueMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" TabIndex="3">
                                    <%--AllowPaging="true"--%>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkSelectAll" runat="server" TabIndex="4" Checked="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkSelect" runat="server" Checked="false" TabIndex="5" />
                                                <asp:HiddenField ID="hfCusPK" runat="server" Value='<%#Eval("ICH_CUST_PK") %>' />
                                                <asp:HiddenField ID="hfCusEmail" runat="server" Value='<%#Eval("ICH_CUST_MAIL") %>' />
                                                <asp:HiddenField ID="hdfInvoicePk" runat="server" Value='<%#Eval("ICH_PK") %>' />
                                                <asp:HiddenField ID="hdfInvoiceDate" runat="server" Value='<%#Eval("ICH_DATE") %>' />
                                                <asp:HiddenField ID="hdfCurCode" runat="server" Value='<%#Eval("CUR_CODE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="No" ItemStyle-Width="2%">
                                              <ItemTemplate>
                                                 <%#Container.DataItemIndex+1 %>
                                              </ItemTemplate>
                                              <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateField> CustomerMailID --%>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName%>" SortExpression="ICH_CUST_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerNameTxt" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval("ICH_CUST_NAME").ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ICH_CUST_NAME").ToString()),60) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="37%" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="<%$ resources:CustomerMailID%>" SortExpression="ICH_CUST_MAIL">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerMailID" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval("ICH_CUST_MAIL").ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ICH_CUST_MAIL").ToString()),60) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="17" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo%>" SortExpression="ICH_INV_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval("ICH_INV_NO").ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ICH_INV_NO")),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:OrderFreq %>" SortExpression="<%$ resources:DataFieldRes,CustomerOrdFreq %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerOrdFreq" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CustomerOrdFreq) %>'
                                                    Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.CustomerOrdFreq),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        --%>
                                        <asp:TemplateField HeaderText="<%$ resources:DueDate%>" SortExpression="ICH_INV_DUE_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text="" ToolTip=""></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DueAmount%>" SortExpression="ICH_BAL_AMOUNT">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblDueAmount" runat="server" ToolTip='<%# Eval("ICH_BAL_AMOUNT") %>'
                                                    Text='<%# Eval("ICH_BAL_AMOUNT") %>' />--%>
                                                <asp:Label ID="lblDueAmount" runat="server" ToolTip='<%# Eval("ICH_BAL_AMOUNT","{0:c}") %>'
                                                    Text='<%# Eval("ICH_BAL_AMOUNT","{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <%--HorizontalAlign="Right"--%>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:NextOrderExp %>" SortExpression="<%$ resources:DataFieldRes,CustomerNextSoDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNextOrderExp" runat="server" ToolTip='' Text='' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="<%$ resources:DataFieldRes,CustomerOrdStatus %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CustomerOrdStatus) %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.CustomerOrdStatus) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <%-- Dummy for getting space between due amount and mail sent on--%>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MailSenton %>" SortExpression="LAST_MAIL_ON">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMailSenton" runat="server" ToolTip='' Text='' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Packing" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
