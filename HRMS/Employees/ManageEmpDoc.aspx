<%@ Page Title="<%$ Resources:Captions,Title_HR_ManageDocs %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ManageEmpDoc.aspx.cs" Inherits="HRMS.Employees.ManageEmpDoc"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControls/CheckInControl.ascx" TagName="CheckInControl" TagPrefix="ucChkIn" %>
<%@ Register Src="UserControls/CheckOutControl.ascx" TagName="CheckOutControl" TagPrefix="ucChkOut" %>
<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitPage() {
            GrandScriptUtils.DatePickerCommon("txtExpBefore");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAutoNationality ", url, "hdfAutoNationality", true, true, "NATIONALITY");

            showHideCheckInCheckOut('1');

            $('[id$=ddlFilterFor]').change(function () {
                var selectedFilter = $('[id$=ddlFilterFor] option:selected').val();
                //showHideCheckInCheckOut(selectedFilter);
            });
            //ShowHideAdvancedSearch();


            $("[id*=chkHeader]").live("click", function () {
                var chkHeader = $(this);
                var grid = $(this).closest("table");
                $("input[type=checkbox]", grid).each(function () {
                    if (chkHeader.is(":checked")) {
                        $(this).attr("checked", "checked");
                    } else {
                        $(this).removeAttr("checked");
                    }
                });
            });
            $("[id*=chkSelect]").live("click", function () {
                var grid = $(this).closest("table");
                var chkHeader = $("[id*=chkHeader]", grid);
                if (!$(this).is(":checked")) {
                    chkHeader.removeAttr("checked");
                } else {
                    if ($("[id*=chkSelect]", grid).length == $("[id*=chkSelect]:checked", grid).length) {
                        chkHeader.attr("checked", "checked");
                    }
                }
            });
        }

        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
                $("[id$=hdfCurrentSearchMode]").val('ADVANCED');
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
                $("[id$=hdfCurrentSearchMode]").val('BASIC');
            }
            return false;
        }

        function showHideCheckInCheckOut(selectedFilter) {
            if (selectedFilter === "1") { //CheckIn
                $('[id$=btnCheckOut]').hide();
                $('[id$=btnCheckIn]').show();
            } else if (selectedFilter === "0") { //CheckOut
                $('[id$=btnCheckOut]').show();
                $('[id$=btnCheckIn]').hide();
            }
        }
        
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlCheckIn">
                                        <asp:Button ID="btnCheckIn" Text="Check In" runat="server" ClientIDMode="Static"
                                            ToolTip="<%$resources:CheckIn %>" SkinID="btnInner-checkin" OnClick="ActionHandler"
                                            CommandName="CHECKIN" TabIndex="13" CssClass="margntop5" />
                                    </li>
                                    <li runat="server" id="pnlCheckOut">
                                        <asp:Button ID="btnCheckOut" Text="Check Out" runat="server" ClientIDMode="Static"
                                            TabIndex="14" SkinID="btnInner-checkout" OnClick="ActionHandler" CommandName="CHECKOUT"
                                            ToolTip="<%$resources:CheckOut %>" CssClass="margntop5" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <%-- <table class="table-devide" runat="server" id="pnlEmpDocDetails">
                                <tr>
                                    <td colspan="2" align="right">--%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                Advance Search</h1>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--</td>
                                </tr>--%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblNationality" AssociatedControlID="txtAutoNationality"
                                                Text="<%$resources:Nationality %>" CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAutoNationality" CssClass="select-small-a" TabIndex="1" />
                                            <asp:HiddenField ID="hdfAutoNationality" runat="server" />
                                            <asp:Label runat="server" ID="lblCompany" AssociatedControlID="ddlCompany" Text="<%$resources:Company %>"
                                                CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="select-small-g" TabIndex="2">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblEmployeeName" AssociatedControlID="txtEmployeeName"
                                                Text="<%$resources:EmpName %>" CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEmployeeName" CssClass="input-small-c" TabIndex="3" />
                                            <asp:Label runat="server" ID="lblEmpCode" AssociatedControlID="txtEmpCode" Text="<%$resources:EmployeeCode %>"
                                                CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEmpCode" CssClass="input-small" TabIndex="4" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblFilterFor" AssociatedControlID="ddlFilterFor" ClientIDMode="Static"
                                                Text="<%$resources:FilterFor %>" CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlFilterFor" CssClass="select-small-a2 margnbotm0"
                                                TabIndex="5">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblControl" Text="<%$ resources:ExpiresBefore %>" AssociatedControlID="txtExpBefore"
                                                CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExpBefore" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" TabIndex="6" CssClass="input-small margnbotm0" />
                                            <asp:Label runat="server" ID="lblExpiringIn" AssociatedControlID="txtExpiringIn"
                                                Text="<%$resources:ExpiringIn %>" CssClass="middle-lbl-xsmall-b"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtExpiringIn" CssClass="small margnbotm0" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                TabIndex="7" />
                                            <span style="background: none; border: 0; height: 2px;" class="margnbotm0" id="lblSpan">
                                                Days</span>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblDocumentType" AssociatedControlID="ddlDocumentTypeList"
                                                Text="<%$resources:EmpDocumentType %>" CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:DropDownList runat="server" CssClass="select-small-c1 margnbotm0" ID="ddlDocumentTypeList"
                                                TabIndex="8">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblDocNo" AssociatedControlID="txtDocNo" Text="Document No."
                                                CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDocNo" CssClass="input-small margnbotm0" TabIndex="9" />
                                            <%--<asp:Label runat="server" ID="lblGoBtn" Text="" AssociatedControlID="btnSearch"></asp:Label>--%>
                                            <%--<asp:Button ID="btnGo" SkinID="btnInner-Go" OnClick="ActionHandler" runat="server"
                                                Text="Go" CommandName="FILTER" TabIndex="5" />--%>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="10" CommandName="FILTER" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--</table>--%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEmpDocList" Width="100%" AutoGenerateColumns="false"
                                    OnPageIndexChanging="ActionHandler" PageSize="<%$ resources:PageSize %>" AllowPaging="true"
                                    AllowSorting="true" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkHeader" tabindex="11" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox Text="" runat="server" Checked="false" ID="chkSelect" TabIndex="12" />
                                                <asp:HiddenField runat="server" ID="hdfDocPk" Value='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EmpCode %>" SortExpression="<%$ resources:DataFieldRes,EmpCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpCode).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpCode).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>" SortExpression="<%$ resources:DataFieldRes,Employee %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployee" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Employee).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Employee).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="true" HeaderText="<%$ resources:Nationality %>" SortExpression="<%$ resources:DataFieldRes,Nationality %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNationality" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Nationality).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Nationality).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DocType %>" SortExpression="<%$ resources:DataFieldRes,DocType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocType" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocType).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocType).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuedBy %>" SortExpression="<%$ resources:DataFieldRes,EmpDocIssuedBy %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuedBy" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DocNo %>" SortExpression="<%$ resources:DataFieldRes,DocNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocNo" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocNo).ToString())%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocNo).ToString()),8)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IssuedOn %>" SortExpression="<%$ Resources:DataFieldRes,IssuedOn%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssuedOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.IssuedOn, Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.IssuedOn, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DocExpiresOn %>" SortExpression="<%$ Resources:DataFieldRes,DocExpiresOn%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpiresOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.DocExpiresOn, Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DocExpiresOn, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DocDaysLeft %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDaysLeft" runat="server" Text='<%# Eval(Resources.DataFieldRes.DocDaysLeft) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DocDaysLeft)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkLog" runat="server" ImageUrl="~/Images/Classic/Icons/log.jpg"
                                                    OnClick="ActionHandler" CommandName="SHOWLOG" TabIndex="8" ToolTip="Log" CommandArgument="PageAction_Entry" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="divCheckInControlContainer" style="display: none;">
                    <ucChkIn:CheckInControl ID="CheckInControl1" runat="server" OnAfterSave="ResetForm"
                        OnLogClosed="ActionHandler" OnError="ActionHandler" />
                </div>
                <div id="divCheckOutControlContainer" style="display: none;">
                    <ucChkOut:CheckOutControl ID="CheckOutControl1" runat="server" OnAfterSave="ResetForm"
                        OnLogClosed="ActionHandler" OnError="ActionHandler" />
                </div>
            </div>
            <asp:HiddenField ID="hdfCurrentSearchMode" runat="server" Value="BASIC" />
            <asp:HiddenField ID="hdfCheckingOrCheckOut" runat="server" Value="" />
            <div id="divCheckInCheckOutLog" style="display: none;">
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdCheckIncheckOutLog" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocSlNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocSLNo) %>'
                                        ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocSLNo)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:EmpDocCheckedOnDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocChkdOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocCheckedOnDate, Resources.Constants.HRMSDateFormatGrid) %> '
                                        ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocCheckedOnDate, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:EmpDocCheckedType %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocChkdType" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocCheckedType) %> '
                                        ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocCheckedType)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="true" HeaderText="<%$ resources:EmpDocCheckedBy %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedBy" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckedBy).ToString())%>'
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckedBy).ToString()),20)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="22%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Purpose %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuedOn" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckOutPurpose).ToString())%>'
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckOutPurpose).ToString()),20)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiresOn" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString())%>'
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString()),20)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="22%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
