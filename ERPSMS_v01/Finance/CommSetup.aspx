<%@ Page Title="<%$ Resources:Captions,Title_AgentCommision %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="CommSetup.aspx.cs" Inherits="ERPSMS_v01.Finance.CommSetup"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function ResetComm(selRowindex) {
            if (selRowindex != null) {
                var gridView = document.getElementById('<%= grdCommision.ClientID %>');
                $(gridView.rows[parseInt(selRowindex) + 1]).find('[id*=txtCommision]').val("");

                var oRows = gridView.rows;
                for (i = 1; i < oRows.length; i++) {
                    //var type = $(gridView.rows[parseInt(selRowindex) + 1]).find('[id*=ddlRateType]').val();
                    var type = gridView.rows[i].cells[3].children[0].value;
                    if (type == 3) {
                        gridView.rows[i].cells[4].children[0].disabled = true;
                        gridView.rows[i].cells[5].children[0].disabled = false;
                    }
                    else {
                        $(gridView.rows[parseInt(selRowindex) + 1]).find('[id*=ddlFormula]').val(0);
                        gridView.rows[i].cells[4].children[0].disabled = false;
                        gridView.rows[i].cells[5].children[0].disabled = true;
                    }

                    if (type != 3) {

                        //                        var ri = 7; // I suppose that you know the Index of Row Which you want to hide
                        //                        var grd = document.getElementById('<%= grdCommision.ClientID %>');
                        //                        grd.rows[ri].style.display = 'none';


                        //$(gridView.rows[parseInt(selRowindex) + 1]).find('[id*=btnPopupFormula]').disabled = false;
                        //                        var col_num = "7";
                        //                        alert(col_num)
                        //                        rows = document.getElementById("grdCommision").rows;

                        //                        for (i = 0; i < rows.length; i++) {
                        //                            rows[i].cells[col_num].style.display = "none";

                        //                        }

                    }
                }
            }
        }


        function winClose() {
            ClosePopup();
        }

        function ddlToolTipsRegion(ddlFormula) {
            if (ddlFormula.value == 0) {
                ddlFormula.title = "";
            } else {
                ddlFormula.title = ddlFormula.options[ddlFormula.selectedIndex].text;
            }
        }

        function AfterClose(containerID) {
            if (containerID == "[id$=divFormulaSettAll]") {
                $("[id$=btnFormulaclose_Action]").click();
                //                ShowContainerDiv('[id$=divPopUpDetails]', '<%= GetLocalResourceObject("Commision") %>', '950', '550');

            }
        }

        function ResetCommSetPop(mod) {
            var ConfAgentCommissionFormula = '<%= GetGlobalResourceObject("ConfigurationsRes","AgentCommissionFormula").ToString() %>';
            if (ConfAgentCommissionFormula == 1) {
                var ddlformula = $("#ddlFormulaAll").val();
            }
            var txtcommission = $("[id$=txtCommissionAll]").val();
            if (mod == 1) {
                $("#ddlFormulaAll").val(0);
            }
            else {
                $("[id$=txtCommissionAll]").val("");
            }
        }

        function DisableControls() {
            var type = $("[id$=ddlRateTypeAll]").val();
            if (type == 3) /*3 = FORMULA*/ {
                $("[id$=txtCommissionAll]").attr('disabled', 'disabled');
                $('#ddlFormulaAll').removeAttr('disabled');
            }
            else {
                $('#ddlFormulaAll').val(0);
                $("[id$=txtCommissionAll]").removeAttr('disabled');
                $('#ddlFormulaAll').attr('disabled', 'disabled');
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnNew" runat="server" SkinID="btnInner-New" Text="<%$Resources:New %>"
                                    EnableViewState="False" ToolTip="<%$resources:New %>" CommandName="NEW" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Edit %>"
                                    EnableViewState="False" ToolTip="<%$resources:Edit %>" CommandName="EDIT" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button ID="btnDelte" runat="server" SkinID="btnInner-Delete" Text="<%$Resources:Delete %>"
                                    EnableViewState="False" ToolTip="<%$resources:Delete %>" CommandName="REMOVE" OnClick="ActionHandler" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL_ACTION" SkinID="btnInner-Cancel"
                                    ToolTip="<%$resources:Controls,Cancel %>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <asp:UpdatePanel ID="aupdpnlHeader" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hfAgentPk" runat="server" />
                <asp:HiddenField ID="hfFormulaPk" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <%-- <table>
                    <tr>
                        <td>
                            <ul class="bredcrum">
                                <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                              
                            </ul>
                        </td>
                        <td>
                            <div style="text-align: right; height: 29px;">
                                <asp:Button ID="btnNew" runat="server" SkinID="btnInner-New" Text="<%$Resources:New %>"
                                    EnableViewState="False" ToolTip="<%$resources:New %>" CommandName="NEW" OnClick="ActionHandler" />
                                <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Edit %>"
                                    EnableViewState="False" ToolTip="<%$resources:Edit %>" CommandName="EDIT" OnClick="ActionHandler" />
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL_ACTION" SkinID="btnInner-Cancel"
                                    ToolTip="<%$resources:Controls,Cancel %>" />
                            </div>
                        </td>
                    </tr>
                </table>--%>
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="head-detail">
                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:Agent%>" CssClass="lblhighlight" />:
                                <%-- AssociatedControlID="txtAgentName"--%>
                                <%-- <asp:TextBox ID="txtAgentName" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>--%>
                                <asp:Label ID="lblAgentName" runat="server" CssClass="lblhighlight" />
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdCustomers" Width="100%" AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable"
                        AutoGenerateColumns="False" AutoGenerateSelectButton="False">
                        <%--OnPageIndexChanging="ActionHandler"  OnSorting="ActionHandler"  OnSelectedIndexChanging="ActionHandler"
                        OnDataBound="ActionHandler" OnRowCommand="ActionHandler"--%>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                        onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true" OnCheckedChanged="ActionHandler" />
                                    <%-- AutoPostBack="true" OnCheckedChanged="ActionHandler"--%>
                                    <asp:HiddenField ID="hfCustomerPk" runat="server" Value='<%#Eval("ACD_CUSTOMER") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="3px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Customer%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" Text='<%#Eval("ACD_CUSTOMER_TEXT") %>' runat="server" />
                                </ItemTemplate>
                                <%--<ItemStyle Width="12%" />--%>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%-- <uc1:PagerControl ID="uclPagingMyTask" runat="server" />--%>
                </div>
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdCommisionView" Width="100%" AllowSorting="True"
                        EmptyDataRowStyle-CssClass="emptytable" AutoGenerateColumns="False" AutoGenerateSelectButton="False"
                        OnDataBound="ActionHandler">
                        <%--OnPageIndexChanging="ActionHandler"  OnSorting="ActionHandler"  OnSelectedIndexChanging="ActionHandler"
                         OnRowCommand="ActionHandler"--%>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:BrandCode%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductCode" ToolTip='<%#Eval("ACD_PRODUCT_CODE") %>' runat="server"
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ACD_PRODUCT_CODE") ,18) %>' />
                                </ItemTemplate>
                                <ItemStyle Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:BrandName%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblBrandName" Text='<%#Eval("ACD_BRAND_NAME") %>' runat="server" ToolTip='<%#Eval("ACD_BRAND_NAME") %>' />
                                    <%--<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ACD_BRAND_NAME") ,35) %>--%>
                                </ItemTemplate>
                                <ItemStyle Width="71%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Cur%>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" Text='<%#Eval("VEN_CURRENCY_TEXT") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Type%>">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlRateType" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hdfRateType" Value='<%#Eval("ACD_COMMISION_TYPE") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="7%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Com%>">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCommision" Text='<%#GetFormattedRate(Eval("ACD_COMMISION")) %>'
                                        ToolTip='<%#GetFormattedRate(Eval("ACD_COMMISION")) %>' runat="server" CssClass="small"
                                        Enabled="false" Style="text-align: right;" />
                                    <%--CssClass="small-a1"--%>
                                </ItemTemplate>
                                <ItemStyle Width="5%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Formula%>" Visible="false">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlFormula" Enabled="false" onmouseover="ddlToolTipsRegion(this);">
                                    </asp:DropDownList>
                                    <asp:HiddenField runat="server" ID="hdfFromula" Value='<%#Eval("ACD_FORMULA") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="7%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%-- <uc1:PagerControl ID="uclPagingMyTask" runat="server" />--%>
                </div>
                <div id="divPopUpDetails" style="display: none;" class="content-wrapper">
                    <div style="text-align: right; height: 29px;">
                        <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Save %>"
                            EnableViewState="False" ToolTip="<%$resources:Save %>" CommandName="SAVE" OnClick="ActionHandler" />
                        <asp:Button ID="Button2" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Cancel %>"
                            EnableViewState="False" ToolTip="<%$resources:Cancel %>" CommandName="CANCEL"
                            OnClick="ActionHandler" />
                    </div>
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="divcol-S">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ resources:Customer%>" AssociatedControlID="ddlCustomer" />:
                                    <asp:DropDownList runat="server" ID="ddlCustomer" OnSelectedIndexChanged="ActionHandler"
                                        CommandName="CHANGESTATUS" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="content-wrapper">
                        <%-- <div class="gridwrap">--%>
                        <asp:GridView runat="server" ID="grdCommision" Width="100%" AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable"
                            AutoGenerateColumns="False" AutoGenerateSelectButton="False" OnDataBound="ActionHandler">
                            <%--OnPageIndexChanging="ActionHandler"  OnSorting="ActionHandler"  OnSelectedIndexChanging="ActionHandler"
                         OnRowCommand="ActionHandler"--%>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:BrandCode%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductCode" ToolTip='<%#Eval("ACD_PRODUCT_CODE") %>' runat="server"
                                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ACD_PRODUCT_CODE") ,18) %>' />
                                        <asp:HiddenField runat="server" ID="hdfAcd_Pk" Value='<%#Eval("ACD_PK") %>' />
                                        <asp:HiddenField runat="server" ID="hdfCust_Item" Value='<%#Eval("ACD_CUST_ITEM") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:BrandName%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBrandName" Text='<%#Eval("ACD_BRAND_NAME") %>' runat="server" ToolTip='<%#Eval("ACD_BRAND_NAME") %>' />
                                        <%--<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("ACD_BRAND_NAME") ,35) %>--%>
                                        <asp:HiddenField runat="server" ID="hdfBrandCode" Value='<%#Eval("ACD_BRAND_CODE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="71%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Cur%>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" Text='<%#Eval("VEN_CURRENCY_TEXT") %>' runat="server" />
                                        <asp:HiddenField runat="server" ID="hdfCurrency" Value='<%#Eval("VEN_CURRENCY") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Cur%>" Visible="false">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlCur">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Type%>">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlRateType" OnSelectedIndexChanged="ActionHandler"
                                            CommandName="CHANGEVALUE">
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hdfRateType" Value='<%#Eval("ACD_COMMISION_TYPE") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Com%>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCommision" Text='<%#GetFormattedRate(Eval("ACD_COMMISION")) %>'
                                            runat="server" CssClass="small" Style="text-align: right;" />
                                        <%--CssClass="small-a1"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Formula%>" Visible="false">
                                    <ItemTemplate>
                                        <asp:DropDownList runat="server" ID="ddlFormula" Enabled="false" onmouseover="ddlToolTipsRegion(this);">
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hdfFromula" Value='<%#Eval("ACD_FORMULA") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Button runat="server" ID="btnPopupFormula" CssClass="IMAGEaction-popup" TabIndex="13"
                                            CommandName="SHOWGRIDHEADERPOPUP" EnableTheming="false" OnClick="ActionHandler"
                                            ToolTip="<%$ resources:FormulaSettings%>" Style="margin-right: 0px; margin-bottom: 0px;" />
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="abc" />
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%-- <uc1:PagerControl ID="uclPagingMyTask" runat="server" />--%>
                    </div>
                </div>
                <div id="divFormulaSettAll" style="display: none;">
                    <div class="Button-container-popup">
                        <asp:Button runat="server" ID="btnFormulaApplyAll" CommandName="FROMULAPPLYALL" TabIndex="23"
                            Text="<%$resources:ErpRes,ApplyAll %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,ApplyAll %>"
                            SkinID="btnInner-ok" />
                        <asp:Button runat="server" ID="btnCancelPopup" CommandName="CANCELPOPUP" TabIndex="24"
                            Text="<%$resources:ErpRes,Cancel %>" ToolTip="<%$resources:ErpRes,Cancel %>"
                            SkinID="btnInner-Cancel" OnClick="ActionHandler" />
                    </div>
                    <div class="content-wrapper">
                        <div class="divcol-P1">
                            <asp:Label runat="server" ID="lblratetypeall" Text="<%$ resources:Type%>" AssociatedControlID="ddlRateTypeAll"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlRateTypeAll" CssClass="select-small-a" onchange="DisableControls()">
                            </asp:DropDownList>
                            <div id="pop" runat="server">
                                <asp:Label runat="server" ID="lblFormula" Text="<%$ resources:Formula%>" AssociatedControlID="ddlFormulaAll"></asp:Label>
                                <%--  <asp:DropDownList ID="ddlFormulaAll" CssClass="select-half valid" runat="server"
                                    OnSelectedIndexChanged="ActionHandler" CommandName="RESET" AutoPostBack="true"
                                    ValidationGroup="RateApply">
                                </asp:DropDownList>--%>
                                <asp:DropDownList ID="ddlFormulaAll" CssClass="select-half valid" runat="server"
                                    ClientIDMode="Static" onchange="ResetCommSetPop(0);" ValidationGroup="RateApply">
                                </asp:DropDownList>
                            </div>
                            <div id="Div1">
                                <asp:Label runat="server" ID="lblCommissionAll" Text="<%$ resources:Commision%>"
                                    AssociatedControlID="txtCommissionAll"></asp:Label>
                                <%--  <asp:TextBox ID="txtCommissionAll" runat="server" OnTextChanged="ActionHandler" CommandName="RESET"
                                    CssClass="input-w18-3per" AutoPostBack="true" Style="text-align: right;" />--%>
                                <asp:TextBox ID="txtCommissionAll" runat="server" onblur="ResetCommSetPop(1);"
                                 CssClass="input-w18-3per" Style="text-align: right;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divScriptButtons">
                    <asp:Button runat="server" ID="btnFormulaclose_Action" CommandName="CLOSEPOPUP" OnClick="ActionHandler"
                        EnableTheming="false" Style="display: none" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
