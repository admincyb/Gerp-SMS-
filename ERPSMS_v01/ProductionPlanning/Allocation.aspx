<%@ Page Title="<%$ Resources:Captions,Title_Allocation %>"  Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="Allocation.aspx.cs" Inherits="ERPSMS_v01.ProductionPlanning.Allocation" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            ShowHideFinishedGoodsSync(1);
            ShowHideSelectedOrderSync(1);
        });

        function ShowHideFinishedGoodsSync(flag) {
            //If flag then Show
            if (flag == "1") {
                $("[id$=divFinishedGoodsGid]").show();
                $("[id$=imbSelectedOrdersShow]").hide();
                $("[id$=imbSelectedOrdersHide]").show();
                $("[id$=hdfFinishedGoods]").val("1");
                if ($("[id$=hdfMachineFormer]").val() == "1") {
                    $("#SelectedOrderContainer").attr("class", "max-200");
                    $("#SelectedOrdersContainer").attr("class", "max-200");
                }
                else {
                    $("#SelectedOrdersContainer").attr("class", "max-425");
                }
            }
            else if (flag == "0") {
                $("[id$=divFinishedGoodsGid]").hide();
                $("[id$=imbSelectedOrdersShow]").show();
                $("[id$=imbSelectedOrdersHide]").hide();
                $("[id$=hdfFinishedGoods]").val("0");
                if ($("[id$=hdfMachineFormer]").val() == "1") {
                    $("#SelectedOrderContainer").attr("class", "max-425");
                }
            }
            return false;
        }

        function ShowHideSelectedOrderSync(flag) {
            //If flag then Show
            if (flag == "1") {
                $("[id$=divSelectedHeaderGrid]").show();
                $("[id$=imbMachineFormerShow]").hide();
                $("[id$=imbMachineFormerHide]").show();
                $("[id$=hdfMachineFormer]").val("1");
                if ($("[id$=hdfFinishedGoods]").val() == "1") {
                    $("#SelectedOrderContainer").attr("class", "max-200");
                    $("#SelectedOrdersContainer").attr("class", "max-200");
                }
                else {
                    $("#SelectedOrderContainer").attr("class", "max-425");
                }
            }
            else if (flag == "0") {
                $("[id$=divSelectedHeaderGrid]").hide();
                $("[id$=imbMachineFormerShow]").show();
                $("[id$=imbMachineFormerHide]").hide();
                $("[id$=hdfMachineFormer]").val("0");
                if ($("[id$=hdfFinishedGoods]").val() == "1") {
                    $("#SelectedOrdersContainer").attr("class", "max-425");
                }
            }
            return false;
        }

        function InitComponents() {

            ShowSelectedOrdersSync($("[id$=hdfFinishedGoods]").val());
            ShowHideMachineFormerSync($("[id$=hdfMachineFormer]").val());
        }


        function ShowHideFinishedGoods(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divFinishedGoodsGid").show();
                $("[id$=imbShowFinishedGoods]").hide();
                $("[id$=imbHideFinishedGoods]").show();
            }
            else {

                $("#divFinishedGoodsGid").hide();
                $("[id$=imbShowFinishedGoods]").show();
                $("[id$=imbHideFinishedGoods]").hide();
            }
            return false;
        }

        function ShowHideSelectedOrder(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divSelectedOrdersGid").show();
                $("[id$=imbSelectedOrderShow]").hide();
                $("[id$=imbSelectedOrderHide]").show();
            }
            else {

                $("#divSelectedOrdersGid").hide();
                $("[id$=imbSelectedOrderShow]").show();
                $("[id$=imbSelectedOrderHide]").hide();
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

        //        function AfterClose(contID) {
        //            if (contID == ('#' + '<%=DiverrorMessages.ClientID %>')) {
        //                window.location = '<%= Resources.PageURL.Allocation %>';
        //            }
        //            
        //        }


    </script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
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
                                    <li runat="server" id="pnlAllocation">
                                        <asp:Button ID="btnAllocate" runat="server" CausesValidation="true" ValidationGroup="Allocation"
                                    Text="<%$ resources:Allocate %>" ToolTip="<%$ resources:Allocate %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Allocation')"
                                    CommandName="ALLOCATION" SkinID="btnInner-Save" TabIndex ="2"  />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                       <asp:Button ID="btnCancel" runat="server" Text="<%$Resources:Controls,Cancel%>" ToolTip="<%$Resources:Controls,Cancel%>" OnClick="ActionHandler"
                                                   CommandName="EXIT" SkinID="btnInner-Cancel" TabIndex ="3"  />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <table>
                    <tr>
                        <td colspan="9">
                        <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                 <asp:Label ID="Label1" runat="server" Text="<%$ resources:FinishedGoogsHdr %>"></asp:Label></h1>
                                        </td>
                                        <td>
                                           <asp:ImageButton runat="server" ID="imbSelectedOrdersShow" OnClientClick="javascript:return ShowHideFinishedGoodsSync('1');"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show" />
                                            <asp:ImageButton runat="server" ID="imbSelectedOrdersHide" OnClientClick="javascript:return ShowHideFinishedGoodsSync('0');"
                                                Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                ToolTip="Hide" />
                                        </td>
                                    </tr>
                                </table>
                            </div>



                          
                        </td>
                    </tr>
                </table>
                <%--Finished Goods--%>
                <%-- <div class="gridwrap" id="divFinishedGoodsGid">--%>
                <div runat="server" id="divFinishedGoodsGid">
                    <asp:HiddenField runat="server" ID="hdfFinishedGoods" Value="1" />
                    <asp:HiddenField runat="server" ID="hdfMachineFormer" Value="1" />
                    <asp:HiddenField runat="server" ID="hdfOpenFormer" Value="0" />
                    <div id="SelectedOrdersContainer" class="max-200">
                        <div class="gridwrap ">
                            <asp:GridView runat="server" ID="grdFinishedGoods" Width="100%" ShowFooter="true"
                                AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="grdFinishedGoods_RowDataBound">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:Product %>" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.proCode) %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Description %>" Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemName),24) %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.ItemName) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="<%$ resources:Controls,Product %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSKU" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemText)),80) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemText)) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                       <%-- <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:gErpProductionRes,Total %>"
                                                ToolTip="<%$ resources:gErpProductionRes,Total %>"></asp:Label>
                                        </FooterTemplate>--%>
                                        <ItemStyle Width="66%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="<%$ resources:Size %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.Size)%>' ToolTip='<%# Eval(Resources.DataFieldRes.Size)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:PhysicalStock %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPhysicalStock" runat="server" Text='<%#Eval(Resources.DataFieldRes.PhysicalStock, "{0:n0}") %>' ToolTip='<%#Eval(Resources.DataFieldRes.PhysicalStock, "{0:n0}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalPhysicalStock" runat="server" Text='<%#Eval(Resources.DataFieldRes.PhysicalStockTotal) %>' ToolTip='<%#Eval(Resources.DataFieldRes.PhysicalStockTotal) %>'>
                                            </asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Allocated %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.PrdAllocated, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.PrdAllocated, "{0:n0}")%>'>
                                            </asp:Label>
                                           <%-- <asp:LinkButton ID="lnkAllocated" runat="server" OnClick="ActionHandler" CommandName="ALLOCATED"
                                                Text='<%# Eval(Resources.DataFieldRes.PrdAllocated, "{0:n0}")%>'></asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.PrdProducedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.PrdProducedTotal)%>'>
                                            </asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="8%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Available %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(Resources.DataFieldRes.Available, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.Available, "{0:n0}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalAvailable" runat="server" Text='<%# Eval(Resources.DataFieldRes.AvailableTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.AvailableTotal)%>'>
                                            </asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="8%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:PagerControl ID="uclPagingFinishedGoods" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <table>
                    <tr>
                        <td colspan="9">
                         <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                  <asp:Label ID="lblSelectedOrdersHdr" runat="server" Text="<%$ resources:SelectedOrdersHdr %>"></asp:Label></h1>
                                        </td>
                                        <td>
                                              <asp:ImageButton runat="server" ID="imbMachineFormerShow" OnClientClick="javascript:return ShowHideSelectedOrderSync('1');"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show" />
                                            <asp:ImageButton runat="server" ID="imbMachineFormerHide" OnClientClick="javascript:return ShowHideSelectedOrderSync('0');"
                                                Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                ToolTip="Hide" />
                                        </td>
                                    </tr>
                                </table>
                            </div>


                        
                        </td>
                    </tr>
                </table>
                <%--Selected Order Items --%>
                <%-- <div class="gridwrap" id="divSelectedOrdersGid">--%>
                <div runat="server" id="divSelectedHeaderGrid">
                    <div id="SelectedOrderContainer" class="max-200">
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdSelectedOrders" Width="100%" AllowSorting="True"
                                ShowFooter="true" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                OnRowDataBound="grdSelectedOrders_RowDataBound">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:Order %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrder" runat="server" Text='<%#Eval(Resources.DataFieldRes.Order) %>' ToolTip='<%#Eval(Resources.DataFieldRes.Order) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                       
                                        <ItemStyle Width="11%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Product%>"  Visible ="false"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval(Resources.DataFieldRes.ProductCode) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="14%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:ProductName %>" Visible ="false"  >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ProductName),20) %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.ProductName) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="<%$ resources:Controls,Product %>" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescriptionsKU" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ProductText)),30) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ProductText)) %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                      
                                        <ItemStyle Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Size %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" runat="server" Text='<%# Eval(Resources.DataFieldRes.Size)%>' ToolTip='<%# Eval(Resources.DataFieldRes.Size)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="6%"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:ReqdBy %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReqd" runat="server" Text='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>' ToolTip='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="9%" />
                                         <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:OrderQty %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.OrderQty, "{0:n0}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblOrderQtyTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQtyTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.OrderQtyTotal)%>'></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="8%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:AvailableStock %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAvailable" runat="server" Text='<%# Eval(Resources.DataFieldRes.AvailableStock, "{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.AvailableStock, "{0:n0}")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblAvailableTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.AvailableStockTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.AvailableStockTotal)%>'></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:BalanceToAllocate %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalanceToAllocate" runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduce, "{0:n0}")%>' ToolTip ='<%# Eval(Resources.DataFieldRes.BalanceToProduce, "{0:n0}")%>'></asp:Label>
                                          <%--  <asp:LinkButton ID="lblBalanceToAllocate" OnClick="ActionHandler" CommandName="PLAN"
                                                runat="server" Text='<%# Eval(Resources.DataFieldRes.BalanceToProduce, "{0:n0}")%>'></asp:LinkButton>--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblBalanceToAllocateTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.PrdProducedTotal)%>' ToolTip='<%# Eval(Resources.DataFieldRes.PrdProducedTotal)%>'></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                         <HeaderStyle CssClass="amount-numeric" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:AllocateNow %>">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAllocateNow" runat="server" TabIndex="1" CausesValidation="true"
                                                Width="75px" MaxLength="18" ValidationGroup="Allocation"  CssClass="numeric"></asp:TextBox>
                                         <%--   <asp:RegularExpressionValidator Style="float: left" ID="vreAllocateNow" runat="server"
                                                ErrorMessage="<%$ Resources:Msg_ValidNumber %>" Text="*" ControlToValidate="txtAllocateNow"
                                                CssClass="star" Display="Dynamic" ValidationGroup="Allocation" ValidationExpression="^\$?(^[-+]?[0-9]{0,12})?(\.[0-9]{0,3})?$">
                                            </asp:RegularExpressionValidator>--%>
                                              <cc1:QuantityValidation ID="vreAllocateNow" runat="server" ControlToValidate="txtAllocateNow"
                                                            NumberDigits="12" ErrorMessage="<%$ Resources:Msg_ValidNumber %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Allocation" NonZero="true"></cc1:QuantityValidation>




                                        </ItemTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc1:PagerControl ID="uclPagingSelectedOrders" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPlan" ValidationGroup="Allocation" runat="server" />
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
                                <%-- <label class="popup-error">
                                </label>--%>
                                <asp:Label ID="lblErrorMsgLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.dbRetValTxt) %>' Width ="89%"></asp:Label>
                                <%--  ((int)Eval(Resources.DataFieldRes.dbRetVal) > 0 ? "<span >" : "<span style='color:Red' >") + Eval(Resources.DataFieldRes.dbRetValTxt).ToString() + "</span>"--%>
                            </ItemTemplate>
                            <ItemStyle Width="90%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
               
            </div>
            <div id="divAllocatioDetails" style="display: none;">
                <div class="contentwrapper present">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%--Product --%>
                                    <asp:Label ID="litProduct" Text="<%$ resources:Product %>" AssociatedControlID="lblProduct"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblProduct" runat="server"> </asp:Label>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <%--Size --%>
                                    <asp:Label ID="litSize" Text="<%$ resources:Size %>" AssociatedControlID="lblSize"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblSize" runat="server"> </asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <%--Description --%>
                                    <asp:Label ID="litDescription" Text="<%$ resources:Description %>" AssociatedControlID="lblDescription"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <h3>
                        <asp:Label ID="lblAllocationHead" runat="server" Text="<%$ resources:AllocationDetails %>"></asp:Label>
                    </h3>
                    <asp:GridView runat="server" ID="grdAllocation" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Order %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%#Eval(Resources.DataFieldRes.Order) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                </FooterTemplate>
                                <ItemStyle Width="25%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%#Eval(Resources.DataFieldRes.CustomerName) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Reqd %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblReqd" runat="server" Text='<%#Eval(Resources.DataFieldRes.ReqdBy,Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:OrderQty %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQty,"{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblOrderQtyTotal" runat="server" Text='<%# Eval(Resources.DataFieldRes.OrderQtyTotal)%>'></asp:Label>
                                </FooterTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Allocated %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.AldQtyAllocated,"{0:n0}")%>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAllocated" runat="server" Text='<%# Eval(Resources.DataFieldRes.AldQtyAllocatedTotal)%>'>
                                    </asp:Label>
                                </FooterTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right"  />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc1:PagerControl ID="uclAllocation" runat="server" />
                </div>
            </div>
            <div id="DivOrderDtlPopup" style="display: none">
              <div class="contentwrapper present">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%--Product --%>
                                    <asp:Label ID="litOrderNo" Text="<%$ resources:OrderNo %>" AssociatedControlID="lblOrderNo"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblOrderNo" runat="server"> </asp:Label>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <%--RequiredBy --%>
                                    <asp:Label ID="litReqBy" Text="<%$ resources:ReqdBy %>" AssociatedControlID="lblRequiredBy"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblRequiredBy" runat="server"> </asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%--OrderProduct --%>
                                    <asp:Label ID="litOrderProduct" Text="<%$ resources:Product %>" AssociatedControlID="lblOrderProduct"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblOrderProduct" runat="server"></asp:Label></div>
                            </td>
                            <td>
                            <div class="div2col-S">
                                    <%--Size --%>
                                    <asp:Label ID="litOrderSize" Text="<%$ resources:Size %>" AssociatedControlID="lblOrderSize"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblOrderSize" runat="server"> </asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <%--Description --%>
                                    <asp:Label ID="litOrderDescription" Text="<%$ resources:Description %>" AssociatedControlID="lblOrderDescription"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblOrderDescription" runat="server" CssClass="width"></asp:Label>
                                </div>
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-H">
                                    <%--Ordered Qty --%>
                                    <asp:Label ID="litOrderedQty" Text="<%$ resources:OrderedQty %>" AssociatedControlID="lblOrderedQty"
                                        runat="server" CssClass="red"></asp:Label>
                                    <asp:Label ID="lblOrderedQty" runat="server" CssClass="red"> </asp:Label>
                                    <%--Dispatched --%>
                                    <asp:Label ID="litDispatched" Text="<%$ resources:Dispatched %>" AssociatedControlID="lblDispatched"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblDispatched" runat="server"> </asp:Label>
                                    <%--Allocated --%>
                                    <asp:Label ID="litAllocated" Text="<%$ resources:Allocated %>" AssociatedControlID="lblAllocated"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblAllocated" runat="server"> </asp:Label>
                                    <%--Produced --%>
                                    <asp:Label ID="litProduced" Visible="false"  Text="<%$ resources:Produced %>" AssociatedControlID="lblProduced"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblProduced" Visible="false"  runat="server"> </asp:Label>
                                    <%--Planned --%>
                                    <asp:Label ID="litPlanned" Text="<%$ resources:Planned %>" AssociatedControlID="lblPlanned"
                                        runat="server"></asp:Label>
                                    <asp:Label ID="lblPlanned" runat="server"> </asp:Label>
                                    <%--Balance To Plan --%>
                                    <asp:Label ID="litBalPlan" Text="<%$ resources:BalanceToAllocate%>" AssociatedControlID="lblBalanceToAllocate"
                                        runat="server" CssClass="red"></asp:Label>
                                    <asp:Label ID="lblBalanceToAllocate" runat="server" CssClass="red"> </asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAllocate" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
