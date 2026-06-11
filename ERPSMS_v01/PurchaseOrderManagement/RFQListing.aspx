<%@ Page Title="<%$ Resources:Captions,Title_RFQ %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" AutoEventWireup="true" CodeBehind="RFQListing.aspx.cs" Inherits="ERPSMS_v01.PurchaseOrderManagement.RFQListing" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

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

        function InitComponents() {
            if ($("[id$=hdfRFQCurrentDepartment]").val() != "0")// In this page,CurrentDepartment of page will changed w. r. to selected record dept.During the time of, session multiple department checking,it leads to session logout (ValidatePageDept()).
                $("[id$=hdfCurrentDepartment]").val($("[id$=hdfRFQCurrentDepartment]").val());

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            var pageURLRFQ = '<%= Resources.PageURL.RequestForQuoteAuto %>';
            if (window.location.href.indexOf("Dep") > -1) {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url + "&PAGE_URL=" + pageURLRFQ + "&FieldName=DPT_NAME", "hdfDeptPk", true, true, "PURRFQAUTO");
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtDepartment", url + "?PAGE_URL=" + pageURLRFQ + "&FieldName=DPT_NAME", "hdfDeptPk", true, true, "PURRFQAUTO");
            }
        }
        function ShowListing(flag) {
            if (flag) {

                $("[id$=PageAction_List]").show();
                $("[id$=pnlListing]").show();
            }
            else {

                $("[id$=PageAction_List]").hide();
                $("[id$=pnlListing]").hide();
            }
            return false;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
            }
            else if (mode == 2) {
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlRFQListing">
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
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Add%>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="9" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="10" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnResponse" CommandName="RFQRESPONSE" TabIndex="10"
                                            Text="<%$resources:Controls,Response %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-respond" ToolTip="<%$resources:Controls,Response %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
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
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="3"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblDepartment" Text="<%$ resources:Department %>" AssociatedControlID="txtDepartment"></asp:Label>
                                            <asp:TextBox ID="txtDepartment" runat="server" EnableViewState="false" CssClass="input-half" TabIndex="4">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfDeptPk" runat="server" />
                                            <asp:ImageButton ID="imbtnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="4"
                                                CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="imbtnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="4" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <asp:HiddenField ID="hdfRFQCurrentDepartment" runat="server" Value="0" />
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdRFQList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                    AutoPostBack="true" ID="rbtSelect" OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="<%$ resources:DataFieldRes,RFQDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.RFQDate) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.RFQDate)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RFHNO %>" SortExpression="<%$ resources:DataFieldRes,RFQNO %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRefID" runat="server" Value='<%# Eval(Resources.DataFieldRes.RFQID) %>' />
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.RFQNO)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.RFQNO)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department %>" SortExpression="<%$ resources:DataFieldRes,DepartmentName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DPT_NAME"), 52) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("DPT_NAME").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Items %>" SortExpression="<%$ resources:DataFieldRes,RFQItemText %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItems" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.RFQItemText),60) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.RFQItemText).ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="<%$ resources:DataFieldRes,RFQStatusText %>">
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("RFH_STATUS_TEXT") %>' />
                                                <%--<asp:Label ID="lblStatus" runat="server" Text='<%# Eval(Resources.DataFieldRes.RFQStatusText) %>' ToolTip='<%# Eval(Resources.DataFieldRes.RFQStatusText)%>'></asp:Label>--%>
                                                <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval(Resources.DataFieldRes.RFQStatus) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval(Resources.DataFieldRes.RfqListDept) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="utilization" OnClick="ActionHandler"
                                                    CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                    CommandName="PRINT" ToolTip="<%$ resources:QtnComparison%>" />
                                                <%--Visible='<%# (Eval(Resources.DataFieldRes.RFQStatus)).ToString()=="2"?true :false%>' --%>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="payment" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
