<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="AccountMaping.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.AccountMaping"
    ValidateRequest="false" Theme="ClassicExt" Title="<%$ Resources:Captions,Title_AccountMapping %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlAccountMaping">
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
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            CommandName="SAVE" OnClick="ActionHandler" ToolTip="Save" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            CommandName="CANCEL" OnClick="ActionHandler" ToolTip="Cancel" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div id="searchwrap" class="search-wrap-custom">
                    <asp:Label ID="lblMappingType" runat="server" AssociatedControlID="lblMappingType"
                        Text="<%$ resources:MappingType %>" />
                    <asp:DropDownList runat="server" ID="ddlMappingType" AutoPostBack="true" CssClass="large"
                        TabIndex="0" OnSelectedIndexChanged="ActionHandler">
                    </asp:DropDownList>
                </div>
                <div class="gridwrap">
                    <asp:GridView ID="grdAccoutMappingList" runat="server" AutoGenerateColumns="False"
                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                        OnRowDataBound="ActionHandler" Width="100%">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:BaseAccount %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblBaseAccount" runat="server" Text='<%# Eval("CKD_FROM_ACC_NAME") %>'
                                        ToolTip='<%# Eval("CKD_FROM_ACC_NAME")%>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfBaseAccountPk" Value='<%# Eval("CKD_TO_ACCOUNT") %>' />
                                    <asp:HiddenField runat="server" ID="hdfFROM_ACCOUNT" Value='<%# Eval("CKD_FROM_ACCOUNT") %>' />
                                    <asp:HiddenField runat="server" ID="hdfCKD_PK" Value='<%# Eval("CKD_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="65%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:MappedAccount %>">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlMappedAccount" CssClass="input-full" >
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
