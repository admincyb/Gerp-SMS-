<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="EmpMst.aspx.cs" Inherits="ERPSMS_v01.Employee.EmpMst" Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" language="javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");


        function InitDate() {
            GrandScriptUtils.AddDateRange("txtEmployeeDOB", "hdfDob", "txtEmployeeDOJ", "hdfDoJ", "dd-M-yy", false, true, true);
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

                function InitComponents() {

            //            GrandScriptUtils.MakeAutoComplete("txtDesignation", url, "hdfDesignation", true, true, "DESIGNATION");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCountry", url, "hdfCountry", true, true, "COUNTRY");
            //  $("[id$=__VIEWSTATE").val("JUNO"); 


        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlEmployee">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                 <asp:Table ID="tblButton" runat="server">
                         <asp:TableRow>
                           <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                               <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                               <ul runat="server" id="pnlEntry" style="display: none">
                               
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="13" Text="<%$resources:Controls,Save %>"
                                         OnClick="ActionHandler" ValidationGroup="Employee" OnClientClick="javascript:ValidatePageNow('Employee')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                          OnClick="ActionHandler"  TabIndex="14" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" 
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="15" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                 <ul runat="server" id="pnlListing" style="display: none">
              
                                    <li>
                                        <asp:Button runat="server" TabIndex="16" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="17" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" ToolTip="<%$resources:Controls,View %>"
                                            TabIndex="18" Text="<%$resources:Controls,View %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-View" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="19" Text="<%$resources:Controls,Print %>"
                                             CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" Visible="false" />
                                    </li>
                                </ul>
                            </asp:TableCell> 
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div id="divTabContainer" class="tab-container" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnEmployeeList" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnEmployee" Text="<%$resources:PageNameRes,Employee %>"
                             OnClick="ActionHandler" CommandName="ADDEMPLOYEE"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="searchwrap" class="search-wrap-custom1">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:ErpRes,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy"></asp:Label>
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="20">
                                    <asp:ListItem Text="<%$ resources:EmployeeCode %>" Value="<%$ resources:DataFieldRes,EmployeeCode %>"></asp:ListItem>
                                    <asp:ListItem Text="<%$ resources:EmployeeName %>" Value="<%$ resources:DataFieldRes,EmployeeName %>"></asp:ListItem>
                                   
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" TabIndex="21"></asp:TextBox>
                                <asp:Button ID="btnSearch" SkinID="btnInner-Go" runat="server" OnClick="ActionHandler"
                                    ToolTip="<%$ resources:ErpRes,Go %>" Text="<%$ resources:ErpRes,Go %>" CommandName="SEARCH"
                                    TabIndex="22" />
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmployee" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" TabIndex="23" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="24" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeCode %>" SortExpression="<%$ resources:DataFieldRes,EmployeeCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeCode" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EmployeeCode) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.EmployeeCode),16) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeName %>" SortExpression="<%$ resources:DataFieldRes,EmployeeName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EmployeeName)%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.EmployeeName),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmpCategory %>" SortExpression="<%$ resources:DataFieldRes,EmpTypeData %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCategory" runat="server" ToolTip='<%# (Eval(Resources.DataFieldRes.EmployeeCategory)).ToString()=="1"? Resources.ErpRes.Employee :Resources.ErpRes.Customer %>'
                                                    Text='<%# (Eval(Resources.DataFieldRes.EmployeeCategory)).ToString()=="0"? Resources.ErpRes.Employee :Resources.ErpRes.Customer %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                 
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeDOJ %>" SortExpression="<%$ resources:DataFieldRes,EmployeeDOJ %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeDOJ" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmployeeDOJ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="<%$ resources:EmployeeCity %>" SortExpression="<%$ resources:DataFieldRes,EmployeeCity %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeCity" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.EmployeeCity1)%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.EmployeeCity1),17) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ErpRes,Status  %>" SortExpression="<%$ resources:DataFieldRes, EmployeeStatus %>">
                                            <ItemTemplate>
                                                <image id="imgStatus" title='<%# (Eval(Resources.DataFieldRes.EmployeeStatus)).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                                    class='<%# (Eval(Resources.DataFieldRes.EmployeeStatus)).ToString()=="1"?"active" :"inactive"%>'
                                                    alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell> 
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmpCode" runat="server" AssociatedControlID="txtEmployeeCode" Text="<%$ resources:EmployeeCode %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeCode" MaxLength="100" TabIndex="1" CssClass="input-small-c"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfEmpCode" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtEmployeeCode"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EmployeeCode %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmpName" runat="server" AssociatedControlID="txtEmployeeName" Text="<%$ resources:EmployeeName %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeName" MaxLength="200" TabIndex="2" CssClass="input-half"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrftxtEmployeeName" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" runat="server" ControlToValidate="txtEmployeeName"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EmployeeName %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblEmpAddress" runat="server" AssociatedControlID="txtEmployeeAddress"
                                                    Text="<%$ resources:EmployeeAddress %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeAddress" TabIndex="3" EnableTheming="false"
                                                    CssClass="multiline-3col" TextMode="MultiLine" onkeydown="limitText(this,200);"
                                                    onkeyup="limitText(this,200);">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator runat="server" ID="revDescription" CssClass="star"
                                                    ValidationGroup="Employee" SetFocusOnError="true" ControlToValidate="txtEmployeeAddress"
                                                    Display="Dynamic" ValidationExpression="^(.|\n){1,200}$" Text="*" ErrorMessage="<%$ resources:ErpRes, Msg_Exceed_MaxLen %>"
                                                    EnableClientScript="true" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmpCity" runat="server" AssociatedControlID="txtEmployeeCity" Text="<%$ resources:EmployeeCity %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeCity" MaxLength="100" TabIndex="4"></asp:TextBox>
                                                <asp:Label ID="lblEmpCountry" runat="server" CssClass="middle-lbl" AssociatedControlID="txtCountry" Text="<%$ resources:EmployeeCountry %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCountry" TabIndex="5"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCountry" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:HiddenField ID="hdfDob" runat="server" />
                                                <asp:Label ID="lblEmpDOB" runat="server" AssociatedControlID="txtEmployeeDOB" Text="<%$ resources:EmployeeDOB %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeDOB" TabIndex="6" class="date-picker"
                                                    CssClass="medium" onpaste="return false;"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vrfEmployeeDOB" CssClass="star" ValidationGroup="Employee"
                                                    runat="server" ControlToValidate="txtEmployeeDOB" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date %>"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfDoJ" runat="server" />
                                                <asp:Label ID="lblEmpDOJ" runat="server" CssClass="lbl-19-3perc" AssociatedControlID="txtEmployeeDOJ" Text="<%$ resources:EmployeeDOJ %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtEmployeeDOJ" TabIndex="7" CssClass="medium" onpaste="return false;"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vrfEmployeeDOJ" CssClass="star" ValidationGroup="Employee"
                                                    runat="server" ControlToValidate="txtEmployeeDOJ" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date %>"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                              
                                                <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="<%$ resources:ErpRes, Status %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlStatus" TabIndex="12" CssClass="medium">
                                                    <asp:ListItem Text="<%$ resources:ErpRes, Select %>" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ resources:ErpRes, Activate %>" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="<%$ resources:ErpRes, InActivate %>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfStatus" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" InitialValue="-1" runat="server"
                                                    ControlToValidate="ddlStatus" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Status %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <%-- <asp:Label ID="lblEmpType" runat="server" AssociatedControlID="ddlEmpType" Text="<%$ resources:EmpType %>"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlEmpType" TabIndex="11" CssClass="medium">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfEmpType" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Employee" EnableClientScript="true" InitialValue="-1" runat="server"
                                                    ControlToValidate="ddlEmpType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Msg_EmpType %>">
                                                </asp:RequiredFieldValidator>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                        </asp:Table>
                        <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Employee" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>