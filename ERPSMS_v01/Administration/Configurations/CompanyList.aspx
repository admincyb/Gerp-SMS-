<%@ Page Title="<%$ Resources:Captions,Title_Company %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="CompanyList.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.CompanyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

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
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnNew" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,New%>"
                                    OnClientClick="javascript:return ValidatePageNow('vgCompany')" CommandName="NEW"
                                    TabIndex="16" OnClick="ActionHandler" ToolTip="New" />
                            </li>
                            <li>
                                <asp:Button ID="btnEdit" runat="server" SkinID="btnInner-Edit" Text="<%$Resources:Controls,Edit%>"
                                    CommandName="EDIT" OnClick="ActionHandler" TabIndex="17" ToolTip="Edit" /><%--OnClientClick="javascript:return CancelFun();"--%>
                            </li>
                            <li>
                                <asp:Button ID="btnDelete" runat="server" Visible="true" SkinID="btnInner-Delete"
                                    Text="<%$Resources:Controls,Delete%>" CommandName="DELETE" OnClick="ActionHandler"
                                    TabIndex="18" ToolTip="Delete" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <%-- <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString() %></h1>
                                        </td>
                                        <td>
                                            <%--<asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-3devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        
                                       
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--<asp:Label ID="lblFromDt" runat="server" Text="<%$resources:FromMonth %>" AssociatedControlID="txtFromDt"></asp:Label>
                                            <asp:TextBox ID="txtFromDt" runat="server" TabIndex="1" CssClass="Uidate-picker"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtenderFrom" runat="server" BehaviorID="calendar1"
                                                TargetControlID="txtFromDt" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hdfFromDt" runat="server" />
                                            <asp:CustomValidator ID="csvMonthFrom" runat="server" Display="None" Text="*" ControlToValidate="txtFromDt"
                                                ClientValidationFunction="CheckMonthRange" ErrorMessage="<%$resources:Msg_Err_Month %>"
                                                ValidationGroup="DateCheck"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="vrftxtCalender" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="DateCheck" EnableClientScript="true" runat="server" ControlToValidate="txtFromDt"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:Button ID="btnGODetails" runat="server" OnClick="ActionHandler" ToolTip="<%$resources:Msg_Err_Month %>" CommandName="SHOW" SkinID="btnInner-Print" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                           <asp:Label ID="lblToDt" runat="server" Text="<%$resources:ToMonth %>" AssociatedControlID="txtToDt"></asp:Label>
                                            <asp:TextBox ID="txtToDt" runat="server" TabIndex="1" CssClass="Uidate-picker" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtenderTo" runat="server" BehaviorID="calendar2"
                                                TargetControlID="txtToDt" Format="MMM-yyyy" OnClientShown="onCalendarShown" ClientIDMode="Static"
                                                OnClientHidden="onCalendarHidden">
                                            </cc1:CalendarExtender>
                                            <asp:HiddenField ID="hdfToDt" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="Search" OnClick="ActionHandler"
                                                OnClientClick="javascript:ValidatePageNow('DateCheck')" TabIndex="14" CommandName="SEARCH"
                                                SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="15"
                                                OnClick="ActionHandler" ToolTip="<%$ resources:Controls,Clear %>" CommandName="CLEAR"
                                                SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>--%>
        <div class="gridwrap">
            <asp:GridView ID="grdCompanyList" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                Width="100%">
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                              OnCheckedChanged="ActionHandler" />
                            <asp:HiddenField runat="server" ID="hdfCmpPk" Value='<%# Eval("CMP_PK") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:CompanyCode %>" SortExpression="CMP_CODE">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyCode" runat="server" Text='<%# Eval("CMP_CODE") %>' ToolTip='<%# Eval("CMP_CODE")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="18%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:CompanyName %>" SortExpression="CMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lblComanyName" runat="server" Text='<%# Eval("CMP_NAME") %>' ToolTip='<%# Eval("CMP_NAME") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Address1 %>" SortExpression="CMP_ADDR1">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress1" runat="server" Text='<%# Eval("CMP_ADDR1") %>' ToolTip='<%# Eval("CMP_ADDR1")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="<%$ resources:Address3 %>" SortExpression="CMP_ADDR1">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress3" runat="server" Text='<%# Eval("CMP_ADDR3") %>' ToolTip='<%# Eval("CMP_ADDR3")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
        </div>
        <%--      </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>--%>
        <div id="diverror" style="display: none">
            <%--Use this label to bind the server errors--%>
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
        </div>
    </div>
</asp:Content>

