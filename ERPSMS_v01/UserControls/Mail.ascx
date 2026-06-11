<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mail.ascx.cs" Inherits="ERPSMS_v01.UserControl.Mail" %>
<script type="text/javascript">
    function InitControlComponents() {
        GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
    }

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

    function ValidateControlNow(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litMailErrorMsg]").hide();
            ShowErrorMessage($("#divMailError").html());
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


    function ClosePopUp() {
        if ($('#divmodel').length > 0) {
            $('#divmodel').hide();
        }
    }
    function ShowMailError(message, title, RedirectURL) {
        if (!title)
            title = errorTitle;
        $(".error").html("");
        $(".error").html(message);
        if (RedirectURL) {
            $(".error").dialog({
                resizable: false,
                title: title,
                buttons: {
                    OK: function (e) {
                        window.location = RedirectURL;
                    }
                },
                beforeClose: function (event, ui) { window.location = RedirectURL; },
                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");

                }
            });
        }
        else {

            $(".error").dialog({
                resizable: false,
                title: title,

                modal: true,
                open: function (event, ui) {
                    $(this).parent().appendTo("#popupHolder");
                }
            });
        }
        return false;
    }
    function ShowMailPopUp(containerID, title, width, height) {
        ///<summary>
        ///function used to Show the Menu
        ///</summary>
        if (!width)
            width = 970;
        if (!height)
            height = 520;
        if (!title)
            title = errorTitle;
        $(containerID).dialog('distroy');
        $("#popupHolder").html("");
        $(containerID).dialog({
            width: width,
            draggable: false,
            height: height,
            resizable: false,
            title: title,
            modal: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#popupHolder");
                $('#divmodel').show();
            },
            close: function (event) {

                if (typeof AfterClose == "function") {
                    AfterClose(containerID);
                }
                //            $(this).remove();
                $('#divmodel').hide();
            }
        });
        return false;
    }


</script>
<asp:UpdatePanel ID="aupdMenu" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="content-wrapper">
            <div class="search-wrap-b">
                <div style="display: none">
                    <asp:Label ID="lblmod" runat="server" Text="Module "></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlmod" AutoPostBack="false" TabIndex="1">
                        <%--<asp:ListItem Text="Select" Value="-1"></asp:ListItem>--%>
                        <asp:ListItem Text="gAero" Value="9"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblApp" Text="Application"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlApp" InitialValue="-1" TabIndex="2">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblsubApp" Text="Sub Application"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlsubApp" TabIndex="2">
                    </asp:DropDownList>
                    <asp:Label runat="server" ID="lblAction" Text="Action"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlAction" TabIndex="2">
                    </asp:DropDownList>
                </div>
                <asp:Label ID="lblProcess" runat="server" Text="<%$Resources:Controls,Process %>"
                    AssociatedControlID="ddlProcess"></asp:Label>
                <asp:DropDownList ID="ddlProcess" runat="server" Width="100px" EnableViewState="true">
                </asp:DropDownList>
                <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                <asp:TextBox runat="server" ID="txtFromDate" CssClass="medium" TabIndex="3" onkeydown="return CheckKey(event)"
                    onpaste="return false;"></asp:TextBox>
                <asp:HiddenField ID="hdfFromDate" runat="server" />
                <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                    ValidationGroup="mailQue" EnableClientScript="true" runat="server" ControlToValidate="txtFromDate"
                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                </asp:RequiredFieldValidator>
                <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                <asp:TextBox runat="server" ID="txtToDate" CssClass="medium" TabIndex="4" onkeydown="return CheckKey(event)"
                    onpaste="return false;"></asp:TextBox>
                <asp:HiddenField ID="hdfToDate" runat="server" />
                <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                    ValidationGroup="mailQue" EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                </asp:RequiredFieldValidator>
                <asp:Button runat="server" ID="btnShow" CommandName="SHOW" SkinID="btnInner-Go" Text="Show" OnClientClick="return ValidateControlNow('mailQue');"
                    OnClick="ActionHandler" meta:resourcekey="btnShowResource1" ValidationGroup="mailQue" />
            </div>
            <div class="Button-container-floatig">
                <asp:Table ID="tblButton" runat="server">
                    <asp:TableRow>
                        <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                        <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                            <ul>
                                <li>
                                    <%-- <asp:Button ID="btnCancel"  runat="server" SkinID="btnInner-Cancel" TabIndex="3" Text="Cancel" />--%>
                                </li>
                                <li runat="server" id="pnlView" visible="false">
                                    <asp:Button runat="server" ID="btnView" SkinID="btnInner-View" CommandName="VIEW"
                                        TabIndex="3" Text="View" OnClick="ActionHandler" CommandArgument="<%$ resources:Section2 %>" /></li>
                            </ul>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                <%--Used For Listing Id is Used for Section Privilage--%>
                <asp:TableRow ID="PageAction_List" runat="server">
                    <asp:TableCell>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
            <asp:GridView ID="grdPage" runat="server" AutoGenerateColumns="False" Width="100%"
                AllowPaging="True" OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                OnRowDataBound="ActionHandler" DataKeyNames="MLQ_PK" meta:resourcekey="grdPageResource1" PageSize="10">
                <PagerStyle BackColor="White" ForeColor="#333333" HorizontalAlign="Center" />
                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="Numeric" NextPageText="Next"
                    PreviousPageText="Previous" />
                <Columns>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:RadioButton runat="server" GroupName="SelectOne" ID="rbtSelect" onclick="javascript:CheckOtherIsCheckedByGVID(this);"
                                meta:resourcekey="rbtSelectResource1" />
                        </ItemTemplate>
                        <ItemStyle Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="To" SortExpression="F_TO" meta:resourcekey="TemplateFieldResource2">
                        <ItemTemplate>
                            <%#Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_TO)%></ItemTemplate>
                            <ItemStyle Width="25%" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="<%$ resources:Name %>" SortExpression="F_NAME" 
                                                    meta:resourcekey="TemplateFieldResource3"  >
                                                    <ItemTemplate  >
                                                        <%#Eval(Utilities.Constants.DA.Administration.ViewMail.F_NAME)%></ItemTemplate>
                                                </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="<%$ resources:Subject %>" SortExpression="F_SUBJECT"
                        meta:resourcekey="TemplateFieldResource4">
                        <ItemTemplate>
                            <asp:Label ID="Label1" Visible="False" runat="server" Text="Label" meta:resourcekey="Label1Resource1"></asp:Label>
                            <asp:Label ID="lblSubject" runat="server" Text="<%# Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_SUBJECT) %>"
                                meta:resourcekey="lblSubjectResource1"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="17%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Controls,Process %>" SortExpression="F_WKF_PROCESS_TEXT" >
                        <ItemTemplate>
                            <%#Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_WKF_PROCESS_TEXT)%></ItemTemplate>
                            <ItemStyle Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Controls,Date %>" SortExpression="F_CRTD_DT" >
                        <ItemTemplate>
                            <%# Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_CRTD_DT, Resources.Constants.DateFormatGrid)%></ItemTemplate>
                            <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Attempt %>" SortExpression="F_ATTEMPT"
                        meta:resourcekey="TemplateFieldResource5">
                        <ItemTemplate>
                            <%#Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_ATTEMPT)%></ItemTemplate>
                            <ItemStyle Width="7%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:LastAttempt %>" SortExpression="F_ATTEMPTON"
                        meta:resourcekey="TemplateFieldResource6">
                        <ItemTemplate>
                            <%#Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_ATTEMPTON, Resources.ErpRes.GridFormatDatetime)%></ItemTemplate>
                            <ItemStyle Width="18%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" meta:resourcekey="TemplateFieldResource7">
                        <ItemTemplate>
                            <%# (Eval("MLQ_STATUS_TEXT"))%></ItemTemplate>
                            <ItemStyle Width="8%" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False" meta:resourcekey="TemplateFieldResource8">
                        <ItemTemplate>
                            <asp:Label ID="lblContent" runat="server" Text="<%# Eval(ERP.Utilities.Constants.DA.Administration.MailSend.F_CONTENT) %>"
                                meta:resourcekey="lblContentResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--Use this label to bind the server errors--%>
            <div id="divMailError" style="display: none">
                <asp:Label runat="server" ID="litMailErrorMsg" ClientIDMode="Static"></asp:Label><asp:ValidationSummary
                    ID="vsMailQue" ValidationGroup="mailQue" runat="server" />
            </div>
            <div id="divPopUp" style="display: none">
                <div class="Button-container-popup">
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="TableCell1" CssClass="SEC_ACTION">
                                <ul>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnResend" CommandName="RESEND" SkinID="btnInner-ok"
                                            Text="<%$ resources:Resend %>" ValidationGroup="BudgetInt" OnClick="ActionHandler"
                                            CommandArgument="<%$ resources:Section2 %>" />
                                        <asp:Button runat="server" ID="btnSendCancel" CommandName="CANCEL" SkinID="btnInner-Cancel"
                                            Text="Cancel" OnClientClick="javascript:ClosePopUp()" CommandArgument="<%$ resources:Section2 %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="content-wrapper">
                    <div class="divcol-S">
                        <%--All the controls will be placed here --%>
                        <asp:Label runat="server" Font-Bold="true" ID="lblTo" AssociatedControlID="lblToText"
                            Text="To"></asp:Label>
                        <asp:Label runat="server" ID="lblToText" Text=""></asp:Label>
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" Font-Bold="true" ID="lblSubject" AssociatedControlID="lblSubjectText"
                            Text="<%$ resources:Subject %>"></asp:Label>
                        <asp:Label runat="server" ID="lblSubjectText" Text=""></asp:Label>
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" Font-Bold="true" ID="lblContent" AssociatedControlID="ltContent"
                            Text="<%$ resources:Content %>"></asp:Label>
                        <span>
                            <asp:Literal ID="ltContent" runat="server"></asp:Literal></span>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
