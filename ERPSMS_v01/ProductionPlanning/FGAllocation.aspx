<%@ Page Title="<%$ Resources:Captions,Title_FGAllocation %>" Language="C#"
 MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="Classic"  CodeBehind="FGAllocation.aspx.cs" Inherits="ERPSMS_v01.ProductionPlanning.FGAllocation" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
      function InitComponents() {
          GrandScriptUtils.DatePicker("txtRequiredBy", false, false);
          var allRules = true;
          $("[id$=grdSaleOrder] tr").each(function () {
              if ($(this).find("td:first input[type=checkbox]").length > 0 &&
                $(this).find("td:first input[type=checkbox]").attr("checked") != "checked")
                  allRules = false;
          });
          if (allRules)
              $('[id$=chbSelectAll]').attr("checked", true);
      }


      function selectAll(evt) {
          var src = window.event != window.undefined ? window.event.srcElement : evt.target;
          var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");

          if (isChkBoxClick && src.id.match(new RegExp('chbSelectAll$'))) {
              if (src.checked) {
                  $("[id$=grdSaleOrder]").find("input:checkbox[id*=chbSelect]").attr("checked", "checked");
              }
              else {
                  $("[id$=grdSaleOrder]").find("input:checkbox[id*=chbSelect]").removeAttr("checked");
              }
          }
          else if (isChkBoxClick) {
              if (!src.checked) {
                  $("[id$=grdSaleOrder]").find("input:checkbox[id$=chbSelectAll]").removeAttr("checked");

              }
          }

      }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:UpdatePanel ID="udpMainContent" runat="server">
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
                            <ul runat="server" id="pnlEntry">
                            <li>
                              <asp:Button ID="btnAdvancedFilter" runat="server" CausesValidation="true" Text="<%$ resources:AdvancedFilter %>" ToolTip ="<%$ resources:AdvancedFilter %>"
                                    OnClick="ActionHandler" CommandName="ADVANCED_FILTER" SkinID="btnInner-search" />
                            </li>
                                    <li runat="server" id="pnlAllocation">
                                        <asp:Button ID="btnAllocation" runat="server" CausesValidation="true" ValidationGroup="Filter"
                                    Text="<%$ resources:Allocation %>" ToolTip="<%$ resources:Allocation %>"  OnClick="ActionHandler" CommandName="ALLOCATION"
                                    SkinID="btnInner-Save" />
                              
                              
                                    </li>
                                    <li runat="server" id="pnlSave">
                                         <asp:Button ID="btnPlanning" runat="server" CausesValidation="true" ValidationGroup="Filter"
                                    Text="<%$ resources:Planning %>" OnClick="ActionHandler" CommandName="PLANNING" Visible ="false"
                                    SkinID="btn-normal" />
                                    </li>
                                    <li runat="server" id="pnlInv">
                                         <asp:Button ID="btnDispatch" runat="server" CausesValidation="true" ValidationGroup="Filter"
                                    Text="<%$ resources:Dispatch %>" OnClick="ActionHandler" CommandName="DISPATCH" Visible ="false"
                                    SkinID="btn-normal" />
                                     
                                <asp:Button ID="btnOrderRealization" runat="server" Visible="false" CausesValidation="true"
                                    Text="<%$ resources:OrderRealization %>" OnClick="ActionHandler" CommandName="ORDERREALIZATION"
                                    SkinID="btn-normal" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
             
            </div>
            <div class="content-wrapper">
                <div class="gridwrap" runat="server" id="divGid">
                    <div class="grdhead">
                        <%-- <asp:Label ID="lblHeader" runat="server" Text="<%$ resources:MainGridHed %>"></asp:Label>--%>
                    </div>
                    <asp:GridView runat="server" ID="grdSaleOrder" Width="100%" PageSize="<%$ resources:PageSize %>"
                        AllowSorting="True" OnRowDataBound="grdSaleOrder_RowDataBound" ShowFooter="true"
                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnSorting="grdSaleOrder_Sorting">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chbSelectAll" runat="server" onclick="selectAll(event);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox CssClass="checkbox" runat="server" ID="chbSelect" onclick="selectAll(event);" />
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Order %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%#Eval(Resources.DataFieldRes.Order) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.Order) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                               
                                <ItemStyle Width="11%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Product %>" >
                                <ItemTemplate>
                                    <asp:Label ID="lblProductGroup" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ProductText)),35) %>'
                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ProductText)) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" Visible ="false" >
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProductCode) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ resources:ProductName %>" Visible ="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ProductName),20) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.ProductName) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Size %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.Size)%>' ToolTip='<%# Eval(Resources.DataFieldRes.Size)%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="8%" HorizontalAlign="Left" />
                                 <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign ="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderTemplate>
                                    <asp:Label ID="lblhdrQty" runat="server" Text="Qty." ToolTip="<%$ resources:OrderQty %>"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQtyTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.OrderQtyTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Reqd %>" SortExpression="SOD_REQUIRED_BY">
                                <ItemTemplate>
                                    <asp:Label ID="lblReqd" runat="server" Text='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>' ToolTip='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Dispatched %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDispatched" runat="server" Text='<%# Eval(Resources.DataFieldRes.Dispatched, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.Dispatched, "{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalDispatched" runat="server" Text='<%# Eval(Resources.DataFieldRes.DispatchedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.DispatchedTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Allow %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.Allocated, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.Allocated, "{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.AllocatedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.AllocatedTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false" HeaderText="<%$ resources:Produced %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduced" runat="server" Text='<%# Eval(Resources.DataFieldRes.Produced, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.Produced, "{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalProduced" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProducedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.ProducedTotal)%>'>
                                 
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Planned %>">
                                <ItemTemplate>
                                   <asp:Label ID="lblTotalProduced" runat="server"  Text='<%# Eval(Resources.DataFieldRes.Planned, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.Planned, "{0:n0}")%>'>
                                    </asp:Label>
                                  <%--  <asp:LinkButton ID="lblPlanned" runat="server" OnClick="ActionHandler" CommandName="PLANNED"
                                        Text='<%# Eval(Resources.DataFieldRes.Planned, "{0:n0}")%>'></asp:LinkButton>--%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalPlanned" runat="server" Text='<%# Eval(Resources.DataFieldRes.PlannedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.PlannedTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblhdrBal" runat="server" Text="Bal.Plan" ToolTip="<%$ resources:BalanceToPlan %>"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlan, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToPlan, "{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalBalanceToPlan" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToPlanTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.BalanceToPlanTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                                 <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc1:PagerControl ID="uclPaging" runat="server" />
                </div>
                <div id="DivPlannedPopup" style="display: none">
                    <asp:GridView runat="server" ID="grdPlanned" Width="100%" PageSize="<%$ resources:PageSize %>"
                        AllowSorting="True" OnRowDataBound="grdSaleOrder_RowDataBound" ShowFooter="false"
                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Periode %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriode" runat="server" Text='<%# Eval(Resources.DataFieldRes.PeriodeFrom,"{0:dd-MMM-yy HH:mm}").ToString()+ (string.IsNullOrEmpty(Eval(Resources.DataFieldRes.PeriodeFrom).ToString()) ? "" : "  To  ") +Eval(Resources.DataFieldRes.PeriodeTo,"{0:dd-MMM-yy HH:mm}").ToString() %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.PeriodeFrom,"{0:dd-MMM-yy HH:mm}").ToString()+ (string.IsNullOrEmpty(Eval(Resources.DataFieldRes.PeriodeFrom).ToString()) ? "" : "  To  ") +Eval(Resources.DataFieldRes.PeriodeTo,"{0:dd-MMM-yy HH:mm}").ToString() %>'>
                                    </asp:Label>
                                    <%--<asp:Label ID="lblPeriodeT" Visible="false"  runat="server" Text='<%#Eval(Resources.DataFieldRes.PeriodeFrom,Resources.Constants.DateFormatGrid) %> '>
                                </asp:Label>--%>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                            </FooterTemplate>--%>
                                <ItemStyle Width="45%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Line %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval(Resources.DataFieldRes.LineName) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:LineSpeed %>" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblLineSpeed" runat="server" Text='<%# Eval(Resources.DataFieldRes.LineSpeed) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.LineSpeed) %>'></asp:Label>
                                </FooterTemplate>
                                <ItemStyle Width="25%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <%-- <asp:ValidationSummary ID="vsPage" ValidationGroup="BasicInfo" runat="server" />--%>
            </div>
            <div id="DiverrorMessages" runat="server" style="display: none">
                <asp:GridView runat="server" ID="grdError" Width="100%" AutoGenerateColumns="false"
                    EmptyDataRowStyle-CssClass="emptytable" EnableTheming="false" ShowHeader="false"
                    BorderWidth="0">
                    <RowStyle HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblErrorMsgLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.dbRetValTxt) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="button-container-popup-bottom">
                    <asp:Button ID="btnContinue" runat="server" CausesValidation="true" Text="<%$ resources:Continue %>"
                        OnClick="ActionHandler" CommandName="CONTINUE" SkinID="btn-normal" />
                    <%--<asp:Button ID="btnCancelpopup" runat="server" CausesValidation="true" Text="<%$ resources:Close %>"
                        OnClientClick="javascript:ClosePopup();" SkinID="btnInner-Cancel" />--%>
                    <asp:Button ID="btnCancelpopup" runat="server" CausesValidation="true" Text="<%$ resources:Close %>"
                        OnClientClick="javascript:$('[id$=DiverrorMessages]').dialog('close');return false;"
                        SkinID="btn-normal" />
                </div>
            </div>
            <div id="DivAdvancedFilterPopup" style="display: none">
                <div class="contentwrapper present">
                    <div class="asset-list">
                        <div class="asset-head">
                            <asp:Label ID="litOrder" Text="<%$ resources:Order %>" AssociatedControlID="chklstOrders"
                                runat="server"></asp:Label></div>
                        <div class="asset-scroll">
                            <asp:CheckBoxList ID="chklstOrders" runat="server">
                            </asp:CheckBoxList>
                            <%-- <asp:DropDownList ID="ddlOrder" runat="server">
                                    </asp:DropDownList>--%>
                        </div>
                    </div>
                    <div class="asset-list">
                        <div class="asset-head">
                            <asp:Label ID="lblPgGroup" Text="<%$ resources:ProductGroup %>" AssociatedControlID="chkProdutGroup"
                                runat="server"></asp:Label></div>
                        <div class="asset-scroll">
                            <asp:CheckBoxList ID="chkProdutGroup" runat="server">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <table class="table-devide present">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%--Product --%>
                                    <asp:Label ID="litProduct" Text="<%$ resources:Product %>" AssociatedControlID="ddlProduct"
                                        runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="medium">
                                    </asp:DropDownList>
                                </div>
                                <%--Size --%>
                                <div class="div2col-S">
                                    <asp:Label ID="litSize" Text="<%$ resources:Size %>" AssociatedControlID="ddlSize"
                                        runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlSize" runat="server" CssClass="medium">
                                    </asp:DropDownList>
                                    <%--RequiredBy --%>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="litReqBy" Text="<%$ resources:RequiredBy %>" AssociatedControlID="txtRequiredBy"
                                        runat="server"></asp:Label>
                                    <asp:TextBox ID="txtRequiredBy" runat="server" CausesValidation="true" ValidationGroup="Filter" CssClass="date-picker"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revRequiredBy" CssClass="star" ValidationGroup="Filter"
                                        runat="server" ControlToValidate="txtRequiredBy" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Valid_Date %>"
                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>

                                    <asp:Label ID="Label1" Text="<%$ resources:PlanStage %>" AssociatedControlID="ddlPlanStage"
                                        runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlPlanStage" runat="server" CssClass="medium">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                   
                                   
                                </div>
                            </td>
                        </tr>
                        <tr>
                          <td colspan="2" align ="right" >
                              <asp:Button ID="btnApplay" runat="server" CausesValidation="true" ValidationGroup="Filter"
                                        Text="<%$ resources:Apply %>" OnClick="ActionHandler" CommandName="FILTER" SkinID="btnInner-search" />
                                    <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Clear %>" SkinID="btnInner-Cancel"
                                        OnClick="ActionHandler" CommandName="CLEAR" />
                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ resources:Cancel %>" SkinID="btnInner-Cancel"
                                        OnClientClick="javascript:ClosePopup();" />
                          </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
