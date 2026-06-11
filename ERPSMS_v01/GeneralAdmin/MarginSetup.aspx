<%@ Page Title="<%$ Resources:Captions,Title_MarginSetup %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="MarginSetup.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.MarginSetup" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtSearchFromDate", "hdfSearchFromDate", "txtSearchToDate", "hdfSearchToDate", false, false);
            ShowHideAdvancedSearch(0);
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
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }


        function ValidationCheckRate(sender, args) {
            if (parseInt($("[id$=txtRate]").val()) > 0) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlMarginSetup">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <%--=========Advance Search Region Begin=========================--%>
                <div class="search-colapse">
                    <table>
                        <tr>
                            <td>
                                <h1>
                                    <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                    TabIndex="8" />
                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                    TabIndex="9" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--------------colpase btn----------%>
                <div class="clear">
                </div>
                <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                    <tr>
                        <td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblSearchType" Text="<%$ Resources:Controls, ChooseType%>"
                                    AssociatedControlID="ddlSearchType"></asp:Label>
                                <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="input-half" TabIndex="10">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblSearchFromDate" Text="<%$ Resources:Controls, FromDate%>"
                                    CssClass="lbl-9perc" AssociatedControlID="txtSearchFromDate"></asp:Label>
                                <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="input-small" TabIndex="11">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfSearchFromDate" runat="server" Value="0"></asp:HiddenField>
                                <asp:Label ID="lblSearchToDate" runat="server" Text="<%$Resources:Controls,ToDate%>"
                                    CssClass="lbl-10-7perc" AssociatedControlID="txtSearchToDate"></asp:Label>
                                <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="input-small" TabIndex="12">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfSearchToDate" runat="server" Value="0"></asp:HiddenField>
                                <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                    TabIndex="13" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                    OnClientClick="" CommandName="DEFAULT" OnClick="ActionHandler" />
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="14" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                    ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                    OnClientClick="" CommandName="CLEAR" OnClick="ActionHandler" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="grdTable">
                    <div id="ProductInsert">
                        <table id="tblProductInsert" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th width="40%" align="left">
                                         <%= GetLocalResourceObject("MarginType")  %>
                                    </th>
                                    <th width="8%" align="left">
                                        <b>
                                            <%=Resources.Controls.FromDate%>* </b>
                                    </th>
                                    <th width="8%" align="left">
                                        <b>
                                            <%=Resources.Controls.ToDate%>* </b>
                                    </th>
                                    <th width="10%">
                                        <b>
                                            <%= GetLocalResourceObject("Type")  %>* </b>
                                    </th>
                                    <th width="10%">
                                        <b>
                                            <%= GetLocalResourceObject("Currency")  %>* </b>

                                    </th>
                                     <th width="19%">
                                        <b>
                                            <%=Resources.Controls.Rate%>* </b>
                                    </th>
                                    <th width="5%" align="left">
                                        <b>
                                            <%=Resources.Controls.Action%>
                                        </b>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="grd-rowhead">
                                    <td width="40%">
                                        <asp:DropDownList ID="ddlMarginType" runat="server" Width="90%" TabIndex="1">
                                        </asp:DropDownList>
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfType" CssClass="star" SetFocusOnError="true" ValidationGroup="mas"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlMarginType" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_MarginType %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td width="8%">
                                        <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="input-half" MaxLength="11"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfBookingDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="mas" EnableClientScript="true" runat="server" ControlToValidate="txtFromDate"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>"></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="8%">
                                        <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-half" MaxLength="11"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="mas" EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>"></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="10%">
                                        <asp:DropDownList ID="ddlType" runat="server" Width="90%" TabIndex="1">
                                        </asp:DropDownList>
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqType" CssClass="star" SetFocusOnError="true" ValidationGroup="mas"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlType" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Type %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td width="10%">
                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="90%" TabIndex="1">
                                        </asp:DropDownList>
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqCurrency" CssClass="star" SetFocusOnError="true" ValidationGroup="mas"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlCurrency" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency %>" InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td width="19%">
                                        <asp:TextBox ID="txtRate" runat="server" TabIndex="4" CssClass="input-half numeric"
                                            Width="80px">
                                        </asp:TextBox>
                                        <asp:CustomValidator ID="customvalOtherCharge" runat="server" ValidateEmptyText="true"
                                            ClientValidationFunction="ValidationCheckRate" ErrorMessage="<%$ resources:Err_Rate%>"
                                            Text="*" EnableClientScript="true" ControlToValidate="txtRate" CssClass="star"
                                            Display="Dynamic" ValidationGroup="mas"></asp:CustomValidator>
                                    </td>
                                    <td align="left" width="5%">
                                        <asp:ImageButton ID="imbAddNew" CommandName="ADDITEM" OnClick="ActionHandler" TabIndex="5"
                                            runat="server" SkinID="imbaddnew" OnClientClick="javascript:ValidateNow('mas')"
                                            Width="16px" />
                                        <asp:HiddenField ID="hdpk" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

                <div class="gridwrap scroll-container">
                    <asp:GridView ID="grdMarginSetup" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                        EmptyDataRowStyle-CssClass="emptytable" PageSize="<%$ resources:PageSize%>" AllowSorting="false"
                        ShowFooter="true" Width="100%">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.masType) %>'
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.masType),16) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="34%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:FromDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromDate" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.masFromDate, "{0:dd/MM/yyyy}")%>'
                                        Text='<%# Eval(Resources.DataFieldRes.masFromDate, "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:ToDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblToDate" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.masToDate, "{0:dd/MM/yyyy}") %>'
                                        Text='<%# Eval(Resources.DataFieldRes.masToDate, "{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblMarginType" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.MarginTypeText) %>'
                                        Text='<%# Eval(Resources.DataFieldRes.MarginTypeText) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="9%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.CurrencyText) %>'
                                        Text='<%# Eval(Resources.DataFieldRes.CurrencyText) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.masRate) %>'
                                        Text='<%# Eval(Resources.DataFieldRes.masRate) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="19%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdfLastModDate" runat="server" Value='<%# Eval(Resources.DataFieldRes.masLastModDate) %>' />
                                    <asp:ImageButton runat="server" ID="imbUpdate" SkinID="imbeditgrid" ToolTip='<%$ resources:Edit %>' TabIndex="6"
                                        OnClick="ActionHandler" CommandName="EDITGRID" CommandArgument='<%# Eval(Resources.DataFieldRes.masPK) %>' />
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip='<%$ resources:Delete %>' TabIndex="7"
                                        OnClick="ActionHandler" CommandName="DELETE" OnClientClick="return ShowDeleteConfirm(this);"
                                        CommandArgument='<%# Eval(Resources.DataFieldRes.masPK) %>' />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc1:PagerControl ID="uclPaging" runat="server" />
                </div>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="mas" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
