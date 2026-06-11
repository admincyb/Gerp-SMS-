<%@ Page Title="<%$ resources:HRMS- Employee Training%>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmpTraining.aspx.cs" Inherits="HRMS.Employees.EmpTraining"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%--Tab User control--%>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%--Ext Gridview control--%>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="13" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="<%$ resources:Cancel%>"
                                            ToolTip="<%$ resources:Cancel%>" CommandName="CANCEL"
                                            TabIndex="64" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks tablelayout">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="gridwrap hierarchical-wrap max-425">
                                <asp:GridView runat="server" ID="grdEmpTrainingList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    CssClass="grdTable" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true" AllowSorting="true">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNO%>" HeaderStyle-HorizontalAlign="Right">
                                            <%--SortExpression="EQD_QUALIFICATION_TYPE_TEXT"--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxNO" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_NO")))%>'
                                                    Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_NO")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxDate%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrxDate" runat="server" ToolTip='<%# Eval("ETA_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    Text='<%# Eval("ETA_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromDate%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate" runat="server" ToolTip='<%# Eval("ETA_FROM_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    Text='<%# Eval("ETA_FROM_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDate%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDate" runat="server" ToolTip='<%# Eval("ETA_TO_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    Text='<%# Eval("ETA_TO_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Topic%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTopic" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_TOPIC")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_TOPIC")),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Duration(Hrs)%>" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDuration" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_DURATION")))%>'
                                                    Text='<%# GetFormattedNumber(Eval("ETA_DURATION")) %>'></asp:Label> 
                                                <%--<%# GetFormattedNumber(Eval("ETA_DURATION")) %>--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Place%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlace" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_PLACE")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_PLACE")),25) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Trainer%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrainer" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_TRAINER")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_TRAINER")),20) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Justify" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " HeaderStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ETA_DESCRIPTION")))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ETA_DESCRIPTION")),20) %>'></asp:Label> 
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Justify"/>
                                        </asp:TemplateField>
                                    </Columns> 
                                </asp:GridView>
                                <uc1:pagercontrol id="uclPaging" runat="server" tabindex="4" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                            <span style="float: right !important;" id="lblLastModifiedDate" runat="server"></span>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
                <asp:HiddenField ID="hdfPageFlag" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                <asp:HiddenField ID="hdfExchangeDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
                <asp:HiddenField ID="hdfLastModDate" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
