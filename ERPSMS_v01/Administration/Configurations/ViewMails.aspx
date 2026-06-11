<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="ViewMails.aspx.cs" Inherits="ERPSMS_v01.Administration.Configurations.ViewMails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">


        //Pename the Page_Entry ID with the corresponding pages Entry section ID        
        function ShowHide(flag) {
            if (flag == "ADD_DETAILS") {
                $("[id$=SEC_ActionPanel]").hide();
                $("[id$=Page_Entry]").hide();
            }
            else {
                $("[id$=Page_Entry]").show();
                $("[id$=SEC_ActionPanel]").show();
            }
            return false;
        }

        function ValidateNow() {
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function CheckOtherIsCheckedByGVID(spanChk) {
            var IsChecked = spanChk.checked;
            var CurrentRdbID = spanChk.id;
            $("[id$=grdPage]").find("tr:has(td)").each(function () {
                //type = $(this).find("td:first input").attr("type")
                var id = $(this).find("td:first input").attr("id");
                if (id != CurrentRdbID) {
                    $(this).find("td:first input").attr("checked", false);
                }
            });
        }
        function ShowPopUp() {
            var msgTitle;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            $("#divPopUp").dialog({
                width: "900px",
                resizable: false,
                title: msgTitle,
                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");
                }
            });
            return false;
        }
        function ShowErrorMessage(message, title) {
            //if (!title) // we are not dealing with passed titles anymore ! change - 16-Jun-2011
            title = '<%= Resources.ErpRes.Title_Information %>';
            $(".error").html("");
            $(".error").html(message);
            $(".error").dialog({
                resizable: false,
                title: title,
                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");
                }
            });
            return false;
        }
        function ClosePopUp() {
            $('#divPopUp').dialog('close');
            return false;
        }
    </script>
    <asp:UpdatePanel ID="auplDetailList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="datawrap">
                <div class="contentwrapper">
                    <table>
                        <%--<tr>
         <td colspan="2">
         <div style="float:right;width:100%">
            <a href="TestMail.aspx" style="float:left"> Send Test Mail</a>
            </div>
            <div class="clear" style="height:30px" ></div>
         </td>
         </tr>--%>
                        <tr>
                            <td>
                                <asp:Label ID="lblLocation" runat="server" Text="<%$Resources:ErpRes, Location %>"
                                    CssClass="small"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlLocation" CssClass="medium" OnSelectedIndexChanged="ActionHandler"
                                    AutoPostBack="true" TabIndex="1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vrfLocation" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlLocation" Display="Dynamic"
                                    InitialValue="-1" ValidationGroup="viewMails" Text="*" ErrorMessage="<%$ resources:Err_Location %>">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCostCenter" Text="<%$Resources:ErpRes, CostCenter %>"
                                    CssClass="medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlCostCenter" InitialValue="-1" TabIndex="2"
                                    CssClass="xlarge" ValidationGroup="viewMails">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vrfCostCenter" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlCostCenter" Display="Dynamic"
                                    InitialValue="-1" ValidationGroup="viewMails" Text="*" ErrorMessage="<%$ resources:Err_CostCenter %>">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 50px">
                                <asp:Button runat="server" ID="btnShow" TabIndex="3" CssClass="aspbtn-adj-valgn"
                                    CommandName="SHOW" ValidationGroup="viewMails" OnClientClick="javascript:ValidateNow()"
                                    Text="<%$ Resources:Controls, Show %>" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="blockr">
                </div>
                <div class="griddataviewer-buttonwrap">
                    <ul>
                        <li runat="server" id="pnlView" visible="false">
                            <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="3" Text="<%$ Resources:Controls, View %>"
                                OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" />
                        </li>
                    </ul>
                </div>
                <div class="gridwrap">
                    <asp:GridView ID="grdPage" runat="server" AutoGenerateColumns="False" Width="100%"
                        AllowPaging="True" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                        OnRowDataBound="ActionHandler" DataKeyNames="MLQ_PK">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="javascript:CheckOtherIsCheckedByGVID(this);" />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:To %>" SortExpression="F_TO">
                                <ItemTemplate>
                                    <%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_TO)%></ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Name %>" SortExpression="F_NAME">
                                <ItemTemplate>
                                    <%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_NAME)%></ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Subject %>" SortExpression="F_SUBJECT">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" Visible="false" runat="server" Text="Label"></asp:Label>
                                    <asp:Label ID="lblSubject" AssociatedControlID="Label1" runat="server" Text="<%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_SUBJECT)%>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="ali" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Attempt %>" SortExpression="F_ATTEMPT">
                                <ItemTemplate>
                                    <%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_ATTEMPT)%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:LastAttempt %>" SortExpression="F_ATTEMPTON">
                                <ItemTemplate>
                                    <%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_ATTEMPTON)%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:ErpRes,Status %>" SortExpression="F_STATUS">
                                <ItemTemplate>
                                    <%# (Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_STATUS))%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblContent" runat="server" Text="<%#Eval(ERP.Utilities.Constants.DA.Administration.ViewMail.F_CONTENT)%>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <%--Use this label to bind the server errors--%>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                    ID="vvsPage" ValidationGroup="BudgetInt" runat="server" />
            </div>
            <div id="divPopUp" style="display: none">
                <div class="contentwrapper" style="margin-left: 20px">
                    <%--All the controls will be placed here --%>
                    <asp:Label runat="server" Width="70px" Font-Bold="true" ID="lblTo" Text="<%$ resources:To %>"></asp:Label>
                    <asp:Label runat="server" ID="lblToText" Text="" CssClass="labelwrap"></asp:Label>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" Width="70px" Font-Bold="true" ID="lblSubject" Text="<%$ resources:Subject %>"></asp:Label>
                    <asp:Label runat="server" ID="lblSubjectText" Text="" CssClass="labelwrap"></asp:Label>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" Width="70px" Font-Bold="true" ID="lblContent" Text="<%$ resources:Content %>"></asp:Label>
                </div>
                <asp:Literal ID="ltContent" runat="server"></asp:Literal>
                <asp:Table ID="Table2" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ID="TableCell1" CssClass="SEC_ACTION" align="center">
                            <ul>
                                <li runat="server" id="pnlSave">
                                    <asp:Button runat="server" ID="btnResend" CommandName="RESEND" Text="<%$ resources:Resend %>"
                                        ValidationGroup="BudgetInt" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow()"
                                        CommandArgument="<%$ resources:Section2 %>" />
                                    <asp:Button runat="server" ID="btnSendCancel" CommandName="CANCEL" Text="<%$ Resources:Controls, Cancel %>"
                                        OnClientClick="javascript:ClosePopUp()" CommandArgument="<%$ resources:Section2 %>" />
                                </li>
                            </ul>
                            <div class="blockr">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
