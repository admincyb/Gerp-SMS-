<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="EmployeeMst.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.EmployeeMst" Theme="Classic" %>

<%@ Register Src="~/UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            var pageURL = window.document.URL;
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        }
        function DisableAuto(extender) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
        function EnableAuto(extender) {
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }
        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();

            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
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
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
//        function ValidateNow() { 
//            if (typeof (Page_ClientValidate) == 'function') {
//                Page_ClientValidate();
//            }
//            if (!Page_IsValid) {
//                $("[id$=litErrorMsg]").hide();
//                ShowErrorMessage($("#diverror").html());
//                return false;  //Page is invalid -- stop right here
//            }
//            else {
//                //everythings ok --- Call your function & do your stuff
//                return true;
//            }
        //        }
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
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtADUsers") {
                $("[id$=btnADUSerChange]").click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlUser">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="9" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" OnClick="ActionHandler"  OnClientClick="javascript:ValidatePageNow('vgcode')"
                                            ValidationGroup="ChecklistType" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Report,Delete %>"
                                            ToolTip="<%$resources:Report,Delete %>" OnClick="ActionHandler" TabIndex="10"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$resources:Controls,Cancel %>" ToolTip="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="11" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="10" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            ToolTip="<%$resources:Controls,New %>" Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-New" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="12" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            ToolTip="<%$resources:Controls,Edit %>" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="13" Text="<%$resources:Controls,View %>"
                                            ToolTip="<%$resources:Controls,View %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-View" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="14" Text="<%$resources:Controls,Print %>"
                                            ToolTip="<%$resources:Controls,Print %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class=" content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Used For Listing Id is Used for Section Privilage--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Report,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy"></asp:Label>
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="15">
                                    <asp:ListItem Text="<%$ resources:name %>" Value="<%$ resources:DataFieldRes,EmployeeName %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ resources:code %>" Value="<%$ resources:DataFieldRes,EmployeeCode %>"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" TabIndex="16" onkeydown="return Search(event);"></asp:TextBox>
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" OnClick="ActionHandler"
                                    ToolTip="<%$ resources:Report,Go %>" Text="<%$ resources:Report,Go %>" CommandName="SEARCH"
                                    TabIndex="16" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdFlNo" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="<%$ resources:Report,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" GroupName="SelectOne" ID="rbtSelect"
                                                    TabIndex="19" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:code %>" SortExpression="<%$ resources:DataFieldRes,EmployeeCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblnameLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmployeeCode) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.EmployeeCode) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:name %>" SortExpression="<%$ resources:DataFieldRes,EmployeeName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcodeLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmployeeName) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.EmployeeName) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Report,Status  %>" SortExpression="<%$ resources:DataFieldRes, EmployeeActive %>">
                                            <ItemTemplate>
                                                <image id="imgStatus" title='<%# (Eval(Resources.DataFieldRes.EmployeeActive)).ToString()=="1"? Resources.Report.Active :Resources.Report.InActive %>'
                                                    class='<%# (Eval(Resources.DataFieldRes.EmployeeActive)).ToString()=="1"?"active" :"InActive"%>'></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--Used For Entry Id is Used for Section Privilage--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblcode" runat="server" AssociatedControlID="txtcode" Text="<%$ resources:code %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtcode" MaxLength="100" TabIndex="1" TextMode="SingleLine"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rvcode" CssClass="star" SetFocusOnError="true" ValidationGroup="vgcode"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtcode" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Msg_code %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblname" runat="server" AssociatedControlID="txtname" Text="<%$ resources:name %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtname" MaxLength="100" TabIndex="2" TextMode="SingleLine"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="rvname" CssClass="star" SetFocusOnError="true" ValidationGroup="vgcode"
                                                        EnableClientScript="true" runat="server" ControlToValidate="txtname" Display="Dynamic"
                                                        Text="*" ErrorMessage="<%$ resources:Msg_name %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ resources:Report, Status %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlStatus" TabIndex="3" CssClass="medium">
                                                    <asp:ListItem Text="<%$ resources:Report, Select %>" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ resources:Report, Active %>" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ resources:Report, InActive %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfStatus" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="vgcode" EnableClientScript="true" InitialValue="-1" runat="server"
                                                    ControlToValidate="ddlStatus" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Status %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="vgFlNo" runat="server" />
                 <asp:ValidationSummary ID="vgcode" ValidationGroup="vgFlNo" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
