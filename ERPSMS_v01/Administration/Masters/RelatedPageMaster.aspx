<%@ Page Title="<%$ Resources:Captions,Title_RelatedLinkMaster %>" Language="C#" MasterPageFile="~/Administration/Masters/AdminMaster.Master"
    Theme="ERP-Admin" AutoEventWireup="true" CodeBehind="RelatedPageMaster.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Masters.RelatedPageMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/RelatedLink.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="srm" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="innerPage-wrap">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow>
                        <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                        <asp:TableCell CssClass="SEC_ACTION" ID="SEC_ActionPanel">
                            <div id="webwizard-wrap">
                                <h1>
                                    <asp:Literal ID="ltHead" runat="server" Text="<%= Resources.Controls.RelatedLinkMaster %>" />
                                </h1>
                                <div class="button-wrap">
                                    <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="9" OnClick="ActionHandler"
                                        CommandName="SAVE" ValidationGroup="vlgProductPlus" OnClientClick="javascript:return ValidateNow()" />
                                    <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="10" OnClick="ActionHandler"
                                        CommandName="CANCEL" />
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="Page_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell CssClass="hrzcentr">
                            <div id="divData">
                                <div class="div2col-S" style="margin-right: 0">
                                    <label for="ddlPage">
                                        <asp:Literal ID="ltPage" runat="server" Text="<%$ Resources:Controls,Page %> " />*
                                    </label>
                                    <asp:DropDownList ID="ddlPage" runat="server" TabIndex="5" Width="260px" OnSelectedIndexChanged="ActionHandler"
                                        onchange="countChecked" AutoPostBack="true" CausesValidation="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="vrfPage" SetFocusOnError="true" ValidationGroup="Save"
                                        EnableClientScript="true" runat="server" ControlToValidate="ddlPage" Text="*"
                                        ErrorMessage="<%$ Resources:Messages,SelectPage %>" InitialValue="-1" Display="Dynamic"
                                        CssClass="star"></asp:RequiredFieldValidator>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="datalistTbl">
                                    <asp:DataList ID="dlPages" runat="server" TabIndex="8" RepeatColumns="4" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" DataKeyField="PAG_PK" OnItemDataBound="ActionHandler">
                                        <HeaderTemplate>
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <h3>
                                                            <asp:Literal ID="ltCheck" runat="server" Text="<%$ Resources:Controls,CheckRelatedPages%>" />*
                                                        </h3>
                                                    </td>
                                                    <td align="right">
                                                        <h4>
                                                            <asp:Literal ID="ltPages" runat="server" Text="<%$ Resources:Controls,Pages%>" />
                                                        </h4>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPageChecked" runat="server" Text='<%#Eval("PAGE_FLAG")%>' Visible="false"></asp:Label>
                                            <asp:CheckBox ID="chkPage" Text='<%#Eval("PAG_TITLE")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <%-- <asp:CustomValidator ID="cvPageChecked" runat="server" ValidationGroup="vlgProductPlus"  Display="None" ClientValidationFunction="countChecked" ErrorMessage="<%$ resources:Msg_CheckRelatedPages%>"></asp:CustomValidator>--%>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vvsPage" ValidationGroup="Save" runat="server" />
    </div>
    <div class="error" style="display: none;height:350px">
    </div>
    <div id="popupHolder" style="width:400px;height:300px">
    </div>
</asp:Content>
