<%@ Page Title="<%$ Resources:Captions,Title_Inbox %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Inbox.aspx.cs"
    Inherits="ERPSMS_v01.AccountManagement.Inbox" Theme="Classic" %>

<%@ Register Src="../UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/AccountManagement/Inbox.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlInbox" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <ul id="tab-menu">
                        <asp:HiddenField runat="server" ID="hdfInboxType" Value="1" />
                        <li><span class="tab-active" id="spnTask" runat="server">
                            <asp:LinkButton ID="lbnTask" runat="server" OnClick="ActionHandler" Text="Tasks"></asp:LinkButton></span></li>
                        <li><span class="tab-inactive" id="spnIntimation" runat="server">
                            <asp:LinkButton ID="lbnIntimations" runat="server" OnClick="ActionHandler" Text="Intimations"> </asp:LinkButton></span></li>
                        <li><span class="tab-inactive" id="spnCompletedTask" runat="server">
                            <asp:LinkButton ID="lbnCompletedTask" runat="server" OnClick="ActionHandler" Text="Completed Tasks"></asp:LinkButton></span></li>
                        <li><span class="tab-inactive" id="spnAlerts" runat="server">
                            <asp:LinkButton ID="lbnAlerts" runat="server" OnClick="ActionHandler" Text="Alerts"></asp:LinkButton></span></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
            </div>
            <%--        <div class="inbox-corner-img">
                </div>
                <asp:HiddenField runat="server" ID="hdfInboxType" Value="1" />
                <asp:LinkButton ID="lbnTask" runat="server" CssClass="inbox-item-selected" EnableTheming="false"
                    OnClick="ActionHandler" Text="Tasks"></asp:LinkButton>
                <asp:LinkButton ID="lbnIntimations" runat="server" CssClass="inbox-item" EnableTheming="false"
                    OnClick="ActionHandler" Text="Intimations"> </asp:LinkButton>
                <asp:LinkButton ID="lbnCompletedTask" runat="server" CssClass="inbox-item" EnableTheming="false"
                    OnClick="ActionHandler" Text="Completed Tasks"></asp:LinkButton>--%>
            <div class="content-wrapper">
                <div id="searchwrap" class="search-wrap-custom">
                    <span runat="server" id="spnSBU">
                        <%=Resources.Controls.SBU %></span>
                    <asp:DropDownList runat="server" ID="ddlSbu" AutoPostBack="true" Width="80px" OnSelectedIndexChanged="ActionHandler">
                    </asp:DropDownList>
                    <span runat="server" id="spnDepartment">
                        <%=Resources.Controls.Department%></span>
                    <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                    </asp:DropDownList>
                    <span>
                        <%=Resources.Controls.DateFrom%></span>
                    <asp:TextBox ID="PeriodFrom" runat="server" TabIndex="2" Width="80px" CssClass="aligncenter">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfPrdFrm" runat="server" />
                    <span style="padding-left: 6px">
                        <%=Resources.Controls.DateTo%></span>
                    <asp:TextBox ID="PeriodTo" runat="server" TabIndex="2" Width="80px" CssClass="aligncenter">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfPrdTo" runat="server" />
                    <span runat="server" id="spnProcess">
                        <%=Resources.Controls.Process%></span>
                    <asp:DropDownList ID="ddlProcess" runat="server" Width="135px" EnableViewState="true">
                    </asp:DropDownList>
                    <span runat="server" id="spnTrxType" visible="false">
                        <%=Resources.Controls.TransactionType%></span>
                    <asp:DropDownList ID="ddlTrxType" runat="server" Width="135px" EnableViewState="true"
                        Visible="false">
                    </asp:DropDownList>
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="ActionHandler" />
                </div>
                <div class="gridwrap">
                    <asp:GridView ID="dgInbox" runat="server" AlternatingItemStyle-BackColor="Silver"
                        AutoGenerateColumns="False" AllowPaging="false" Width="100%" HeaderStyle-Font-Bold="true"
                        OnRowCommand="ActionHandler" EmptyDataRowStyle-CssClass="emptytable">
                        <PagerSettings Visible="false" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <%--<asp:BoundField DataField="ProcessName" HeaderText="<%$Resources:Controls,ProcessName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />--%>
                            <asp:BoundField DataField="TaskDate" HeaderText="<%$Resources:Controls,Date %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                            <asp:BoundField DataField="Dept" HeaderText="<%$Resources:Controls,DepartmentOrStore %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                            <asp:BoundField DataField="TaskName" HeaderText="<%$Resources:Controls,TaskName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="25%" />
                            <asp:BoundField DataField="Message" HeaderText="<%$Resources:Controls,Message %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="38%" />
                            <%--<asp:BoundField DataField="GroupName" HeaderText="<%$Resources:Controls,GroupName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />--%>
                            <asp:TemplateField HeaderText="<%$Resources:Controls,Action %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Width="5%">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="imbAction" ToolTip="<%$Resources:Controls,Action %>"
                                        SkinID="imbactiongrid" CommandName="Action" CommandArgument='<%#Eval("RedirectUrl")%>' />
                                    <asp:HiddenField ID="hdfRedirectUrl" runat="server" Value='<%#Eval("RedirectUrl")%>' />
                                    <asp:HiddenField ID="hdfProcessDept" runat="server" Value='<%#Eval("ProcessDept")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </asp:GridView>
                    <asp:GridView ID="dgAlerts" runat="server" AlternatingItemStyle-BackColor="Silver"
                        AutoGenerateColumns="False" AllowPaging="false" Width="100%" HeaderStyle-Font-Bold="true"
                        OnRowCommand="ActionHandler" EmptyDataRowStyle-CssClass="emptytable" Visible="false">
                        <PagerSettings Visible="false" />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="AlertOn" HeaderText="<%$Resources:Controls,AlertOn %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="DueDate" HeaderText="<%$Resources:Controls,DueDate %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="AlertName" HeaderText="<%$Resources:Controls,AlertName %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" />
                            <asp:BoundField DataField="Type" HeaderText="<%$Resources:Controls,TransactionType %>"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" />
                                <asp:BoundField DataField="Message" HeaderText="<%$Resources:Controls,Message %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources:Controls,Status %>" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                        </Columns>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </asp:GridView>
                    <uc1:PagerControl ID="ucrPager" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
