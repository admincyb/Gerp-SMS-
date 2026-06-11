<%@ Page Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ERPSMS_v01.General.Default" ValidateRequest="false"
    Theme="ClassicExt" Title="<%$ Resources:Captions,Title_ResourceMapping %>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupCategoryDefectMapping" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell  OnClick="ActionHandler"  --%>
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
            <div id="divUserLogin" runat="server">
                <table class="table-devide" id="Table2">
                    <tr>
                        <td>
                            <div class="div2col-S padgtop7 margn-btm0">
                                <asp:Label ID="lblPassword" runat="server" Text="Enter Password" AssociatedControlID="txtPassword"></asp:Label>
                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CommandName="SUBMIT" OnClick="ActionHandler" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="content-wrapper">
                <div id="divManageResource" runat="server" visible="false">
                    <div id="grdTable-wrap">
                        <table class="table-devide" id="Group" runat="server">
                            <tr>
                                <td>
                                    <div class="txt-rgt rdoSelection margnbotm10" style="width: 45%; margin-bottom: 0px">
                                        <asp:RadioButton runat="server" ID="rbnGlobalResource" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnCheckedChanged="ActionHandler" TabIndex="2" CssClass="radiobtn-list margnbotm0"
                                            Text="<%$ resources:Globalresource %>" Value="1" Selected="True" GroupName="SelectOne">
                                        </asp:RadioButton>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div class="div2col-S">
                                        <asp:Label ID="lblModuleServer" runat="server" AssociatedControlID="ddlModuleServer"
                                            Text="<%$ resources:ModuleServer %>" />
                                        <asp:DropDownList runat="server" ID="ddlModuleServer" CssClass="select-half-a" OnSelectedIndexChanged="ActionHandler" EnableViewState="true" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div id="searchwrap" class="div2col-S">
                                        <asp:Label ID="lblResource" runat="server" AssociatedControlID="ddlResources" Text="<%$ resources:SubFolder %>" />
                                        <asp:DropDownList runat="server" ID="ddlResources" CssClass="select-half-a" OnSelectedIndexChanged="ActionHandler"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqResource" CssClass="star" SetFocusOnError="true"
                                            InitialValue="-1" ValidationGroup="Search" EnableClientScript="true" runat="server"
                                            ControlToValidate="ddlResources" Display="Static" Text="*" ErrorMessage="<%$ resources:Err_SubFolder %>">
                                        </asp:RequiredFieldValidator>
                                        <%-- <div class="clear">
                                        </div>
                                        <asp:Label ID="lblimgSearch" runat="server" CssClass="lbl-24-3perc style-none" />--%>
                                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" Text="<%$ resources:Controls,Go %>"
                                            ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                            TabIndex="3" ValidationGroup="Search" CssClass="margntop2 margnlft-minus6 margnbotm0" />
                                        <asp:HiddenField ID="hdfUserType" runat="server" Value="0" />
                                    </div>
                                </td>
                                <td>
                                    <div class="txt-rgt rdoSelection margnbotm10" style="width: 44%; margin-bottom: 0px">
                                        <asp:RadioButton runat="server" ID="rbnLocalResource" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnCheckedChanged="ActionHandler" TabIndex="2" CssClass="radiobtn-list margnbotm0"
                                            Text="<%$ resources:Localresource %>" Value="2" GroupName="SelectOne"></asp:RadioButton>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div class="div2col-S">
                                        <asp:Label ID="lbllocalType" runat="server" AssociatedControlID="lbllocalType" Text="<%$ resources:LocalType %>" />
                                        <asp:DropDownList runat="server" ID="ddlFolder" OnSelectedIndexChanged="ActionHandler"
                                            CssClass="select-half-a" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="gridwrap">
                        <asp:GridView ID="grdResource" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                            EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" Width="100%">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <%-- <asp:TemplateField>
                         <HeaderTemplate>
                        <asp:CheckBox ID="ChkSelectAll" runat="server" Checked="true"  TabIndex="5"/>
                         </HeaderTemplate>
                        <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" Checked="true" TabIndex="5" />                                               
                       </ItemTemplate>
                     <ItemStyle Width="2%" HorizontalAlign="Center" />
                 </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="<%$ resources:ResourceKey %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResName" runat="server" Text='<%# Eval("Name").ToString() %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:ResourceValue %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtResValue" runat="server" Text='<%# Eval("Value").ToString() %>' Width="100%" />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="DateCheck" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:ValidationSummary ID="vsSearch" ValidationGroup="Search" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
