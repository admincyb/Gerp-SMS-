<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayItem.ascx.cs" Inherits="HRMS.UserControls.PayItem" %>
<%@ Register Src="~/Admin/Masters/UserControls/FormulaMaster.ascx" TagName="PopUp"
    TagPrefix="uc1" %>
<script type="text/javascript">
    function ValidateNowPayItem(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsgPayItem]").hide();
            ShowErrorMessage($("#diverrorPayItem").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }


    function PayElementCalcMode(mode) {
        ///<summary>
        /// Used to handle the Pay Element Calculation Mode
        ///</summary>
        /// <param name="mode" optional="true" type="String">
        /// Mode = 0 Indicates Fixed Amount 
        /// Mode = 1 Indicates Formula 
        /// Mode = 2 Indicates Slab 
        /// Mode = 3 Indicates Custom 
        /// </param>       
        var valformula = document.getElementById("<%=rfvAmountOrFormula_PayItem.ClientID%>");
        var valamount = document.getElementById("<%=rfvPayItemAmount.ClientID%>");
        $("[id$=txtPayItemAmount]").ForceNumericOnly();
        if (mode == 0) {
            $("[id$='divAmount']").show();
            $("[id$='divFormula']").hide();

            $("[id$='divSlab']").hide();
            $("[id$='divCustome']").hide();

            ValidatorEnable(valformula, false);
            ValidatorEnable(valamount, true);
            $("[id$=txtPayItemAmount]").removeClass("input-disabled");
            $("[id$=txtPayItemAmount]").attr("disabled", false);
        }
        else if (mode == 1) {
            $("[id$='divAmount']").hide();
            $("[id$='divFormula']").show();

            $("[id$='divSlab']").hide();
            $("[id$='divCustome']").hide();

            ValidatorEnable(valformula, true);
            ValidatorEnable(valamount, false);
        }
        else if (mode == 2) {
            $("[id$='divAmount']").hide();
            $("[id$='divFormula']").hide();

            $("[id$='divSlab']").show();
            $("[id$='divCustome']").hide();

            ValidatorEnable(valformula, false);
            ValidatorEnable(valamount, false);
            $("[id$=txtPayItemAmount]").removeClass("input-disabled");
            $("[id$=txtPayItemAmount]").attr("disabled", false);
        }
        else if (mode == 3) {
            $("[id$='divAmount']").hide();
            $("[id$='divFormula']").hide();

            $("[id$='divSlab']").hide();
            $("[id$='divCustome']").show();


            ValidatorEnable(valformula, false);
            ValidatorEnable(valamount, false);
            $("[id$=txtPayItemAmount]").removeClass("input-disabled");
            $("[id$=txtPayItemAmount]").attr("disabled", false);
        }
        else {
            $("[id$='divAmount']").show();
            $("[id$='divFormula']").hide();
            $("[id$='divSlab']").hide();
            $("[id$='divCustome']").hide();
            ValidatorEnable(valformula, false);
            ValidatorEnable(valamount, false);
            $("[id$=txtPayItemAmount]").removeClass("input-disabled");
            $("[id$=txtPayItemAmount]").attr("disabled", false);
        }
    }
</script>
<asp:UpdatePanel runat="server" ID="aupdpopup">
    <ContentTemplate>
        <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
            <asp:TableRow ID="tblRow" runat="server">
                <%--EntryPage Table Row--%>
                <asp:TableCell>
                    <div class="search-colapse-b">
                        <h1>
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ resources: Controls,WageCalculation%>" /></h1>
                    </div>
                    <div class="clear">
                    </div>
                    <table class="table-devide tablelayout">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="label3" runat="server" Text="<%$ resources: Controls,PayClassificationStar%>"
                                        AssociatedControlID="ddlPayClassification_PayItem"></asp:Label>
                                    <asp:DropDownList ID="ddlPayClassification_PayItem" TabIndex="10" runat="server"
                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqClassification_PayItem" CssClass="star" SetFocusOnError="true"
                                        runat="server" ControlToValidate="ddlPayClassification_PayItem" Display="Dynamic"
                                        Text="*" InitialValue="-1" ValidationGroup="AddToList_PayItem" ErrorMessage="<%$ resources:ErrorMessages,Err_SelectPayClassification %>">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="label4" runat="server" Text="<%$ resources:Controls,PayElementStar%>"
                                        AssociatedControlID="ddlPayElement_PayItem" class=""></asp:Label>
                                    <asp:DropDownList ID="ddlPayElement_PayItem" TabIndex="11" runat="server" CssClass="select-half-a"
                                        AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqPayElement_PayItem" CssClass="star" SetFocusOnError="true"
                                        runat="server" ControlToValidate="ddlPayElement_PayItem" Display="Dynamic" Text="*"
                                        InitialValue="-1" ValidationGroup="AddToList_PayItem" ErrorMessage="<%$ resources:ErrorMessages,Err_SelectPayElement %>">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label ID="lblType" runat="server" Text="<%$ resources:Controls,ElementType %>"
                                        AssociatedControlID="ddlType"></asp:Label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="select-small-c" MaxLength="100"
                                        TabIndex="13" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType"
                                        InitialValue="-1" CssClass="star" ValidationGroup="AddToList_PayItem" Text="*"
                                        ErrorMessage="<%$ resources:Controls,Err_Type %>"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <div id="divFormula" style="display: inline">
                                        <asp:Label ID="label5" runat="server" Text="<%$ resources:Controls,FormulaStar %>"
                                            AssociatedControlID="txtAmountOrFormula_PayItem" class=""></asp:Label>
                                        <asp:TextBox runat="server" ID="txtAmountOrFormula_PayItem" TabIndex="13" CssClass="input-half input-disabled"
                                            Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAmountOrFormula_PayItem" runat="server" ControlToValidate="txtAmountOrFormula_PayItem"
                                            Display="Dynamic" CssClass="star" ValidationGroup="AddToList_PayItem" Text="*"
                                            ErrorMessage="<%$ resources:ErrorMessages,Err_AmountOrFormula%>"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdfFormula" runat="server" Value="" />
                                        <asp:ImageButton ID="btnPopUp" runat="server" OnClick="ActionHandler" CommandName="SHOWPOPUP"
                                            TabIndex="14" SkinID="salary-formula" ToolTip="<%$ resources:Controls,ApplyFormula %>" />
                                    </div>
                                    <div id="divAmount" style="display: inline">
                                        <asp:Label ID="lblPayItemAmount" runat="server" Text="<%$ resources:Controls,AmountStar %>"
                                            AssociatedControlID="txtPayItemAmount" class=""></asp:Label>
                                        <asp:TextBox runat="server" ID="txtPayItemAmount" TabIndex="13" CssClass="input-medium numeric"
                                            MaxLength="13" onkeyup="limitText(this,10);" onkeydown="limitText(this,10);"
                                            onPaste="return false;" onDrop="return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPayItemAmount" runat="server" ControlToValidate="txtPayItemAmount"
                                            Display="Static" CssClass="star" ValidationGroup="AddToList_PayItem" Text="*"
                                            ErrorMessage="<%$ resources:ErrorMessages,Err_Amount%>"></asp:RequiredFieldValidator>
                                    </div>
                                    <div id="divSlab" style="display: inline">
                                        <asp:Label ID="lblSalb" runat="server" Text="<%$ resources: Controls,Slab %> " AssociatedControlID="ddlSlab"
                                            class=""></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlSlab" TabIndex="13" CssClass="select-half-a">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="divCustome" style="display: inline">
                                        <asp:Label ID="lblCustome" runat="server" Text="<%$ resources: Controls,Custom %>"
                                            AssociatedControlID="ddlCustome" class=""></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlCustome" TabIndex="13" CssClass="select-half-a">
                                        </asp:DropDownList>
                                    </div>
                                    <%-- <asp:Button runat="server" ID="btnPopUp" CommandName="SHOWPOPUP" Text="" ToolTip=""
                                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" EnableTheming="false"
                                                Style="display: none !important;" />--%>
                                    <asp:ImageButton ID="btnPlus2" runat="server" OnClick="ActionHandler" CommandName="ADDTOLIST"
                                        ValidationGroup="AddToList_PayItem" OnClientClick="javascript:ValidateNowPayItem('AddToList_PayItem')"
                                        TabIndex="14" SkinID="imbaddnew" ToolTip="<%$ resources:Controls,Add_Add %>" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:TableCell></asp:TableRow>
        </asp:Table>
        <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
        <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
        <div id="divPopUpFormula" style="display: none">
            <uc1:PopUp ID="ucFormulaMaster" runat="server" AfterApply="ucPopUpFormula_AfterApply1" ShowMinMaxAmount="1" />
        </div>
        <div id="diverrorPayItem" style="display: none">
            <asp:ValidationSummary runat="server" ID="vsPayItem" ValidationGroup="AddToList_PayItem" />
            <asp:Label runat="server" ID="litErrorMsgPayItem" ClientIDMode="Static" CssClass="star"></asp:Label></div>
    </ContentTemplate>
</asp:UpdatePanel>
