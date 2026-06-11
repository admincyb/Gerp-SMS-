<%@ Page Title="<%$ Resources:Captions,Title_Port %>" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" CodeBehind="PortList.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.PortList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {

            ShowHideAdvancedSearch(1);

        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag == 1) {
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
        <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
            <table>
                <tr>
                    <td>
                        <h1>
                            <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                            ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                            TabIndex="2" />
                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                            ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                            TabIndex="2" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
        </div>
        <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7 ">
                        <asp:Label ID="Label1" runat="server" Text="<%$ resources:PortCode%>" AssociatedControlID="txtCodeFilterList"></asp:Label>
                        <asp:TextBox runat="server" ID="txtCodeFilterList" TabIndex="2" CssClass="input-small margnbotm0"></asp:TextBox>
                        <asp:Label ID="lblNameFilterList" runat="server" Text="<%$ resources:PortName%>"
                            AssociatedControlID="txtNameFilterList" CssClass="lbl-15-1perc"></asp:Label>
                        <asp:TextBox runat="server" ID="txtNameFilterList" TabIndex="2" CssClass="input-w34per margnbotm0"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                        <asp:ImageButton ID="btnSearchList" runat="server" Text="<%$ resources:Controls,Search %>"
                            ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="3"
                            CommandName="LIST" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                        <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                            ToolTip="<%$resources:Controls,Clear %>" TabIndex="3" OnClick="ActionHandler"
                            CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnlft-minus2 margnbotm0" />
                    </div>
                    <div class="clear">
                    </div>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <div class="gridwrap">
            <asp:GridView ID="grdPortList" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" Width="100%">
                <%--PageSize="<%$ resources:PageSize %>"--%>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                OnCheckedChanged="ActionHandler" />
                            <asp:HiddenField runat="server" ID="hdfCmpPk" Value='<%# Eval("PRM_PK") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:PortCode %>" SortExpression="PRM_DISPLAY_CODE">
                        <ItemTemplate>
                            <asp:Label ID="lblPortCode" runat="server" Text='<%# Eval("PRM_DISPLAY_CODE") %>'
                                ToolTip='<%# Eval("PRM_DISPLAY_CODE") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:PortName %>" SortExpression="PRM_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lblPortName" runat="server" Text='<%# Eval("PRM_NAME") %>' ToolTip='<%# Eval("PRM_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Type %>" SortExpression="PRM_TYPE_TEXT">
                        <ItemTemplate>
                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("PRM_TYPE_TEXT") %>' ToolTip='<%# Eval("PRM_TYPE_TEXT")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:State %>" SortExpression="PRM_STATE_TEXT">
                        <ItemTemplate>
                            <asp:Label ID="lblStatee" runat="server" Text='<%# Eval("PRM_STATE_TEXT") %>' ToolTip='<%# Eval("PRM_STATE_TEXT")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Country %>" SortExpression="PRM_COUNTRY_TEXT">
                        <ItemTemplate>
                            <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("PRM_COUNTRY_TEXT") %>'
                                ToolTip='<%# Eval("PRM_COUNTRY_TEXT")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:From %>">
                        <ItemTemplate>
                            <asp:Label ID="lblFromPort" runat="server" Text='<%# Eval("FROM_PORT") %>' ToolTip='<%# Eval("FROM_PORT")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:To %>">
                        <ItemTemplate>
                            <asp:Label ID="lblToPort" runat="server" Text='<%# Eval("TO_PORT") %>' ToolTip='<%# Eval("TO_PORT")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Status %>">
                        <ItemTemplate>
                            <image id="imgStatus" title='<%# (Eval("PRM_ACTIVE")).ToString()=="1"? Resources.ErpRes.Active :Resources.ErpRes.InActive %>'
                                class='<%# (Eval("PRM_ACTIVE")).ToString()=="1"?"active" :"inactive"%>' alt=""></image>
                        </ItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="<%$ resources:Address %>" SortExpression="PRM_ADDRESS">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("PRM_ADDRESS") %>' ToolTip='<%# Eval("PRM_ADDRESS")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
            <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
        </div>
        <%--   </asp:TableCell>
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
