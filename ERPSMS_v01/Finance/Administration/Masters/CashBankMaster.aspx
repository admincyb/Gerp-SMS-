<%@ Page Title="<%$ Resources:Captions,Title_CashBank %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CashBankMaster.aspx.cs" Inherits="ERPSMS_v01.Finance.Administration.Masters.CashBankMaster"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='lblValidCode']").hide();
            $("[id$='lblValidName']").hide();
            $("[id$='lblValidBranch']").hide();
            $("[id$='lblValidEmail']").hide();
            $("[id$='lblValidStatus']").hide();

            var pageURL = window.document.URL;
            var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            if (url.indexOf("?") != -1) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "&AccType=11", "hdfAccount", true, true, "ACCOUNTMST");
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtAccount", url + "?AccType=11", "hdfAccount", true, true, "ACCOUNTMST");
                GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            }
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }

        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnCashBankListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnCashBankListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnCashBankDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnCashBankDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnCashBankListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnCashBankListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnCashBankDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnCashBankDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
        }


        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidCode']").hide();
            $("[id$='lblValidName']").hide();
            $("[id$='lblValidEmail']").hide();
            $("[id$='lblValidStatus']").hide();
            $("[id$='lblValidBranch']").hide();



            if ($("[id$='txtCode']").val() == '') {
                $("[id$='lblValidCode']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Code") %></li></ul>';
            }

            if ($("[id$='txtName']").val() == '') {
                $("[id$='lblValidName']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Name") %></li></ul>';
            }
            if ($("[id$='txtBranch']").val() == '') {
                $("[id$='lblValidBranch']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Branch") %></li></ul>';
            }

            if ($("[id$='txtEmail']").val() != '') {
                if (IsEmail($("[id$='txtEmail']").val()) == false) {
                    $("[id$='lblValidEmail']").show();
                    isValid = false;
                    msg += '<ul><li><%= GetLocalResourceObject("Err_Email") %>' + '</li></ul>';
                }
            }

            if ($("[id$='ddlStatus']").val() == '-1') {
                $("[id$='lblValidStatus']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Status") %>' + '</li></ul>';
            }


            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }

        function IsEmail(email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!regex.test(email)) {
                return false;
            } else {
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="CashBank" SkinID="btnInner-Save"
                                            TabIndex="97" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="98" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="99" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnCashBankListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnCashBankListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8" />
                        </span></li>
                        <li><span id="spnCashBankDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnCashBankDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="ACTIVATE" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1">
                                    <asp:ListItem Text="<%$ resources:Controls,BankCode %>" Value="<%$ resources:DataFieldRes,BankCode %>" />
                                    <asp:ListItem Text="<%$ resources:Controls,BankName %>" Value="<%$ resources:DataFieldRes,BankName %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" OnClick="ActionHandler" CssClass="input-medium"
                                    TabIndex="2" />
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" Text="<%$ resources:Controls,Go %>"
                                    ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="3" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdCashBankMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,BankCode%>" SortExpression="<%$ resources:DataFieldRes,BankCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPGCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.BankCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.BankCode)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,BankName%>" SortExpression="<%$ resources:DataFieldRes,BankName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPGName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.BankName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.BankName)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Branch%>" SortExpression="<%$ resources:DataFieldRes, BankBranch %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.BankBranch)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.BankBranch)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,AccountType%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGAccountType" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("ADM_CONST_MST.CON_NAME")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ADM_CONST_MST.CON_NAME")),25) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,AccountNumber%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGAccountNumber" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("CBM_ACC_NO")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("CBM_ACC_NO")),20) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Status%>" SortExpression="<%$ resources:DataFieldRes,BanKStatus %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCashBankActive" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblGroupCode" runat="server" Text="<%$ resources:Controls,BankCode%>"
                                                AssociatedControlID="txtCode" />
                                            <asp:TextBox ID="txtCode" runat="server" MaxLength="100" TabIndex="10" CssClass="input-half" />
                                            <asp:Label ID="lblValidCode" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:Controls,BankName%>" AssociatedControlID="txtName" />
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="100" TabIndex="11" CssClass="input-half" />
                                            <asp:Label ID="lblValidName" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBranch" runat="server" Text="<%$ resources:Controls,Branch%>" AssociatedControlID="txtBranch" />
                                            <asp:TextBox ID="txtBranch" runat="server" MaxLength="100" TabIndex="12" CssClass="input-half" />
                                            <asp:Label ID="lblValidBranch" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAccount" Text="<%$ resources:Controls,Account%>"
                                                AssociatedControlID="txtAccount"></asp:Label>
                                            <asp:TextBox ID="txtAccount" runat="server" MaxLength="100" TabIndex="13"  CssClass="input-half" />
                                            <asp:HiddenField ID="hdfAccount" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAddress" runat="server" Text="<%$ resources:Controls,Address%>"
                                                AssociatedControlID="txtAddress" />
                                            <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" MaxLength="500"  Width="357px"
                                                TabIndex="13" CssClass="multiline-3line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCity" runat="server" Text="<%$ resources:Controls,City%>" AssociatedControlID="txtCity" />
                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="200" TabIndex="14" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblState" runat="server" Text="<%$ resources:Controls,State%>" AssociatedControlID="txtState" />
                                            <asp:TextBox ID="txtState" runat="server" MaxLength="200" TabIndex="15" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCountry" runat="server" Text="<%$ resources:Controls,Country%>"
                                                AssociatedControlID="ddlCountry" />
                                            <asp:DropDownList ID="ddlCountry" runat="server" TabIndex="16" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblZip" runat="server" Text="<%$ resources:Controls,Zip%>" AssociatedControlID="txtzip" />
                                            <asp:TextBox ID="txtzip" runat="server" MaxLength="50" TabIndex="17" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblPhone" runat="server" Text="<%$ resources:Controls,Phone%>" AssociatedControlID="txtPhone" />
                                            <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" TabIndex="18" CssClass="input-small-c" />

                                             <asp:Label ID="lblFax" runat="server" CssClass="lbl-4-1perc"  Text="<%$ resources:Controls,Fax%>" AssociatedControlID="txtFax" />
                                            <asp:TextBox ID="txtFax" runat="server" MaxLength="50" TabIndex="19" CssClass="input-w29per" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblMobile" runat="server" Text="<%$ resources:Controls,Mobile%>" AssociatedControlID="txtMobile" />
                                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="50" TabIndex="20" CssClass="input-w23-6per" />

                                            <asp:Label ID="lblEmail" runat="server" CssClass="lbl-5-5perc"  Text="<%$ resources:Controls,Email%>" AssociatedControlID="txtEmail" />
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" TabIndex="21" CssClass="input-w29per" />
                                            <asp:Label ID="lblValidEmail" runat="server" Text="*" CssClass="star" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>                               
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAccountType" runat="server" Text="<%$ resources:Controls,AccountType%>"
                                                AssociatedControlID="ddlAccountType" />
                                            <asp:DropDownList ID="ddlAccountType" runat="server" TabIndex="22" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAccountNumber" runat="server" Text="<%$ resources:Controls,AccountNumber%>"
                                                AssociatedControlID="txtAccountNumber" />
                                            <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="100" TabIndex="23"  CssClass="input-half"/>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblIFSCcode" runat="server" Text="<%$ resources:Controls,IFSCcode%>"
                                                AssociatedControlID="txtIFSCcode" />
                                            <asp:TextBox ID="txtIFSCcode" runat="server" MaxLength="100" TabIndex="24" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSWIFTCode" runat="server" Text="<%$ resources:Controls,SWIFTCode%>"
                                                AssociatedControlID="txtSWIFTCode" />
                                            <asp:TextBox ID="txtSWIFTCode" runat="server" MaxLength="100" TabIndex="25" CssClass="input-half" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblDesc" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtDesc" />
                                            <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" MaxLength="500" TabIndex="26"
                                                CssClass="multiline-3line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">  
                                             <asp:Label runat="server" ID="lblCurrency"  Text="<%$ resources:Controls,Currency%>"
                                                AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" MaxLength="100" TabIndex="27" CssClass="input-half" />
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                           <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Status%>" AssociatedControlID="ddlStatus" />
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="28" CssClass="lbl-31-9perc">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblValidStatus" runat="server" Text="*" CssClass="star" />

                                            <asp:CheckBox ID="chkIsHoldAccount" runat="server" TabIndex="29" Text="<%$ resources:Controls,IsHoldAccount%>"
                                                TextAlign="Left" />
                                        </div>
                                    </td>
                                </tr>                               
                            </table>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="CashBank" runat="server" />
                </div>
                <asp:HiddenField ID="hdfIsSBUBank" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
