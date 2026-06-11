<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Income_Tax_Master_01.aspx.cs"
    Title="<%$ Resources:Captions,Title_ITDeclaration_Master %>" Inherits="HRMS.Admin.Masters.Income_Tax_Master_01"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txt_ITCol3]").ForceNumericOnly();
            $("[id*=txt_ITCol4]").ForceNumericOnly();
        }
        function ImbHideTaxDetails_Click(divID, imbHide, imbShow) {
            var pnlelement = document.getElementById(divID);
            var imbHideelement = document.getElementById(imbHide);
            var imbShowelement = document.getElementById(imbShow);
            pnlelement.style.display = 'none';
            imbHideelement.style.display = 'none';
            imbShowelement.style.display = 'inline';
            return false;
        }
        function ImbShowTaxDetails_Click(divID, imbHide, imbShow) {
            var pnlelement = document.getElementById(divID);
            var imbHideelement = document.getElementById(imbHide);
            var imbShowelement = document.getElementById(imbShow);
            pnlelement.style.display = 'inline';
            imbHideelement.style.display = 'inline';
            imbShowelement.style.display = 'none';
            return false;
        }

        function ShowListing(flag) {

            if (flag) {
                $("[id$=div_tabcontainerList]").addClass('tab-container-floating');
                $("[id$='div_hrmsTab']").hide();
            }
            else {
                $("[id$=div_tabcontainerList]").addClass('tab-container-floating visible-hidden');
                $("[id$='div_hrmsTab']").show();
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
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
    <asp:UpdatePanel ID="upPayrollProcess" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="TableCell1" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')"
                                            TabIndex="10" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating" id="div_tabcontainerList">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <%--   Outer Repeater --%>
                            <asp:Repeater ID="rprIncomeTaxMaster" runat="server" OnItemDataBound="ActionHandler">
                                <HeaderTemplate>
                                    <table cellspacing="0">
                                        <tr>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="search-colapse-b">
                                                <h1 style="width: 80%">
                                                    <asp:Literal ID="ltrTaxDetails" runat="server" Text='<%# Eval("Title") %>' /></h1>
                                                <asp:ImageButton runat="server" Style="display: none" ID="imbShowTaxDetails" TabIndex="4"
                                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                                <asp:ImageButton runat="server" ID="imbHideTaxDetails" TabIndex="4" SkinID="imbArrowHide"
                                                    ToolTip="<%$ resources:Controls,HideDetails%>" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlIncomeTaxDetails" runat="server">
                                                <%--Style="display: none"--%>
                                                <div class="gridwrap" id="divIncomeTaxDetails">
                                                    <%-- Inner Repeater--%>
                                                    <table class="gridwraptable" id="tblDetailsSection">
                                                        <tr>
                                                            <th>
                                                                <asp:Label ID="lbl_HTCol1" runat="server" Text="<%$ resources:SlNo%>" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lbl_HTCol2" runat="server" Text="<%$ resources:Steps%>" />
                                                            </th>
                                                            <th class="padglft2  txt-rgt">
                                                                <asp:Label ID="lbl_HTCol3" runat="server" Text="<%$ resources:DefaultAmount%>" />
                                                            </th>
                                                            <th class="padglft2  txt-rgt">
                                                                <asp:Label ID="lbl_HTCol4" runat="server" Text="<%$ resources:MaxAmount%>" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lblReqChk" runat="server" Text="<%$ resources:DefaultCheckBox%>" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lblCol3Hide" runat="server" Text="<%$ resources:AmountHide%>" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lblCol4Hide" runat="server" Text="<%$ resources:ValueHide%>" />
                                                            </th>
                                                        </tr>
                                                        <asp:Repeater ID="rprIncomeTax" runat="server" OnItemDataBound="Inner_ActionHandler">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td width="1%" class="padglft2 padgrgt2 txt-center">
                                                                        <asp:Label ID="lbl_ITCol1" runat="server" Text='<%# Eval("Col1") %>' ToolTip='<%# Eval("Col1") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfItemDbId" Value='<%# Eval("DbId") %>' />
                                                                    </td>
                                                                    <td width="53%" class="padglft2">
                                                                        <asp:TextBox ID="txt_ITCol2" CssClass="input-w98per" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("Col2"))) %>'
                                                                             ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("Col2")))%>'  />
                                                                    </td>
                                                                    <td width="9%" class=" txt-rgt">
                                                                        <asp:TextBox ID="txt_ITCol3" runat="server" Text='<%# (Eval("DefaultAmount")) == "" ? "" :GetFormattedCurrency(Eval("DefaultAmount")) %>'
                                                                            MaxLength="12" ToolTip='<%# (Eval("DefaultAmount")) == "" ? "" :GetFormattedCurrency(Eval("DefaultAmount")) %>'
                                                                            CssClass="input-w80 numeric" />
                                                                    </td>
                                                                    <td width="9%" class="padgrgt2 txt-rgt">
                                                                        <asp:TextBox ID="txt_ITCol4" runat="server" Text='<%# (Eval("Col3_Max")) == "" ? "" :GetFormattedCurrency(Eval("Col3_Max")) %>'
                                                                            ToolTip='<%# (Eval("Col3_Max")) == "" ? "" :GetFormattedCurrency(Eval("Col3_Max")) %>'
                                                                            CssClass="input-w80 numeric" />
                                                                    </td>
                                                                    <td width="9%" class="txt-center">
                                                                        <asp:CheckBox ID="chkDefualtValue" runat="server" Checked='<%# Convert.ToInt32(Eval("ChkBoxReq")) == 1 ? true : false %>'
                                                                            Style="vertical-align: middle" />
                                                                    </td>
                                                                    <td width="8%" class="txt-center">
                                                                        <asp:CheckBox ID="chkAmountHide" runat="server" Checked='<%# Convert.ToInt32(Eval("Col3_Hide")) == 1 ? true : false %>' />
                                                                    </td>
                                                                    <td width="8%" class="txt-center">
                                                                        <asp:CheckBox ID="chkValueHide" runat="server" Checked='<%# Convert.ToInt32(Eval("Col4_Hide")) == 1 ? true : false %>' />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                            <asp:HiddenField runat="server" ID="hdfSectionDbId" Value='<%# Eval("SectionID") %>' />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:HiddenField
                    ID="hdfIscontYes" runat="server" />
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
