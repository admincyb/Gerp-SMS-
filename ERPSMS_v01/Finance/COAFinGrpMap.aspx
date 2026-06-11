<%@ Page Title="<%$ Resources:Captions,Title_COAFinGrpMap %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="COAFinGrpMap.aspx.cs" Inherits="ERPSMS_v01.Finance.COAFinGrpMap"
    Theme="ClassicExt" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            ShowHideAdvancedSearch($("[id$='hdfShowFilter']").val());
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
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
                $("[id$='hdfShowFilter']").val('1');
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
                $("[id$='hdfShowFilter']").val('');
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
                            validationArrayGroup.push(Page_Validators[i].id);//insert new conrol to the Array of present validations
                        }
                        else {
                            Page_Validators.splice(i, 1);//remove if control is already in Array of present validations
                        }
                    }
                    else {
                        //Page_Validators.splice(i, 1);//remove control if not in group
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), "Information");
                return false;  //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="16" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
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
                                            <asp:HiddenField runat="server" ID="hdfShowFilter" />
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="10" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="10" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label runat="server" ID="lblFilterType" Text="<%$ resources:FilterType%>" class="margnbotm0"
                                                AssociatedControlID="ddlFilterType"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterType" runat="server" CssClass="select-small-b margnbotm0" TabIndex="11">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblFilterFinReport" CssClass="lbl-19perc margnbotm0"
                                                Text="<%$ resources:FilterFinReport%>" AssociatedControlID="ddlFilterFinReport"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterFinReport" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="11" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label runat="server" ID="lblFilterFinGroup" CssClass="margnbotm0" Text="<%$ resources:FilterFinGroup%>"
                                                AssociatedControlID="ddlFilterFinGroup"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterFinGroup" runat="server" CssClass="select-small-b margnbotm0"
                                                TabIndex="11">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblFilterMapped" CssClass="lbl-19perc margnbotm0" Text="<%$ resources:FilterMapped%>"
                                                AssociatedControlID="ddlFilterMapped"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterMapped" runat="server" CssClass="select-small-b margnbotm0" TabIndex="11">
                                                <asp:ListItem Selected="True" Value="0" Text="<%$ resources:MapStatusAll%>"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="<%$ resources:MapStatusMapped%>"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="<%$ resources:MapStatusUnMapped%>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="12" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="13" OnClick="ActionHandler" CommandName="CLEARSEARCH" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S margntop5 margnbotm0" style="vertical-align: bottom;">
                                            <asp:Label ID="lblFinReport" runat="server" Text="<%$resources:FinReport %>" AssociatedControlID="ddlFinReport"></asp:Label>
                                            <asp:DropDownList ID="ddlFinReport" runat="server" TabIndex="14" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-half margnbotm0">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S margntop5 margnbotm0">
                                            <asp:Label ID="lblFinGroup" runat="server" Text="<%$resources:FinGroup %>" AssociatedControlID="ddlFinGroup"></asp:Label>
                                            <asp:DropDownList ID="ddlFinGroup" runat="server" TabIndex="14" CssClass="select-half margnbotm0">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfddlFinGroup" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" InitialValue="-1" runat="server"
                                                ControlToValidate="ddlFinGroup" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FinGroup %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAccounts" Width="100%" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AllowPaging="false">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelect" TabIndex="15" />
                                                <asp:HiddenField runat="server" ID="hdfPk" Value='<%# Eval("COA_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,AccountCode%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.AccountCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.AccountCode),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Name%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.AccountName)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.AccountName),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Parent%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParent" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("COA_PARENT_TEXT")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("COA_PARENT_TEXT"),30) %>' />
                                                <asp:HiddenField ID="hdfParent" runat="server" Value='<%# Eval(Resources.DataFieldRes.CoaParent)%> ' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Type%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccType" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("COA_TYPE_TEXT")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("COA_TYPE_TEXT"),10) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:GrdMapped%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMapped" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("RTC_TEMPLATE_TEXT")) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(GetLocalResourceObject("RTC_TEMPLATE_TEXT").ToString()),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:GrdFinGroup%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccIsGroup" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.RTC_NAME)) %>'
                                                    Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.RTC_NAME),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                        <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                        </table>
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
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
