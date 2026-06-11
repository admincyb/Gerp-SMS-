<%@ Page Title="<%$ Resources:Captions,Title_EDocCreate %>" Language="C#" Theme="ClassicEdoc"
    MasterPageFile="~/DOC.Master" AutoEventWireup="true" CodeBehind="EdocCreate.aspx.cs"
    Inherits="HRMS.EDocs.EdocCreate" %>

<%@ Register Src="~/UserControls/GtiFolderExplorer.ascx" TagName="FileExplorer" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/FileUploaderNew.ascx" TagName="FileUploader" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/CheckListSearchControl.ascx" tagname="CheckListSearchControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function initComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSendTo", url, "hdfSendTo", true, true, "EDOCEMPLOYEES");

            $("[id$=txtSearch]").removeClass();
            $("[id$=txtSearch]").addClass("input-w52-2per margn-rgt0");
            $("[id$=divEdocPopUp]").removeClass();
            $("[id$=divEdocPopUp]").addClass("treelist-scroll2");
            $("[id$=cblList]").removeClass();
            $("[id$=cblList]").addClass("treelist2");
        }

        var validationArrayGroup;
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    else {
                        //Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;
            }
            else {
                return true;
            }
        }

        //To excecute after  auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtSendTo") {
                $('#cblList input[type=checkbox]').each(function (index) {
                    var chkValue = $('label[for=' + this.id + ']').html();
                    if ($("[id$=txtSendTo]").val() == chkValue) {
                         $(this).attr("disabled", "disabled");
                        //$(this).closest("tr").hide();
                        $(this).removeAttr("checked");
                    }
                    else {
                         $(this).attr("disabled", false);
                       // $(this).closest("tr").show();
                    }
                });
                return false;
            }
        }

        //To excecute after  auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtSendTo") {
                $('#cblList input[type=checkbox]').each(function (index) {
                    $(this).attr("disabled", false);
                   // $(this).closest("tr").show();
                });
                return false;
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>" CssClass=" padgtop2"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ resources:Controls,Save%>"
                                            ToolTip="<%$ resources:Controls,Save%>" OnClick="ActionHandler" ValidationGroup="Save"
                                            OnClientClick="javascript:ValidatePageNow('Save');" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="5" />
                                    </li>
                                    <li runat="server" id="pnlFinalize">
                                        <asp:Button runat="server" ID="btnFinalize" CommandName="FINALIZE" Text="<%$Resources:Controls,Finalize%>"
                                            ValidationGroup="Save" OnClientClick="javascript:ValidatePageNow('Save');" ToolTip="<%$Resources:Controls,Finalize%>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-allocation"
                                            TabIndex="5" />
                                    </li>
                                    <li runat="server" id="pnlForward">
                                        <asp:Button runat="server" ID="btnForward" CommandName="FORWARD" Text="<%$Resources:Controls,SendForward%>"
                                            ValidationGroup="Forward" OnClientClick="javascript:ValidatePageNow('Forward');"
                                            ToolTip="<%$Resources:Controls,SendForward%>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" TabIndex="5" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            OnClick="ActionHandler" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" TabIndex="5" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop10">
                                            <asp:Label ID="lblEDocNo" runat="server" Text="<%$ resources:EDocNo%>" AssociatedControlID="txtEDocNo"></asp:Label>
                                            <asp:TextBox ID="txtEDocNo" runat="server" MaxLength="100" TabIndex="1" Enabled="false"
                                                CssClass="input-small"> </asp:TextBox>
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"></asp:Label>
                                            <asp:TextBox ID="txtDate" runat="server" TabIndex="1" CssClass="input-small" MaxLength="15"
                                                onkeydown="javascript:return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Date%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvDateForward" runat="server" ControlToValidate="txtDate"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="Forward" Text="*" ErrorMessage="<%$ resources:Err_Date%>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop10">
                                            <asp:Label ID="lblLetterNo" runat="server" Text="<%$ resources:LetterNo%>" AssociatedControlID="txtLetterNo"></asp:Label>
                                            <asp:TextBox ID="txtLetterNo" runat="server" MaxLength="100" TabIndex="2" CssClass="input-half"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblProjectSite" runat="server" Text="<%$ resources:ProjectSiteReq%>"
                                                AssociatedControlID="ddlProjectSite"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlProjectSite" CssClass="select-full" TabIndex="3">
                                            </asp:DropDownList>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvProjectSite" runat="server" ControlToValidate="ddlProjectSite"
                                                    Display="Dynamic" InitialValue="-1" CssClass="star" ValidationGroup="Save" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ProjectSite%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvProjectSiteForward" runat="server" ControlToValidate="ddlProjectSite"
                                                    Display="Dynamic" InitialValue="-1" CssClass="star" ValidationGroup="Forward"
                                                    Text="*" ErrorMessage="<%$ resources:Err_ProjectSite%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvProjectSiteUpload" runat="server" ControlToValidate="ddlProjectSite"
                                                    Display="Dynamic" InitialValue="-1" CssClass="star" ValidationGroup="Upload"
                                                    Text="*" ErrorMessage="<%$ resources:Err_ProjectSite%>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDepartment" runat="server" Text="<%$ resources:Department %>" AssociatedControlID="ddlDepartment"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlDepartment" TabIndex="3" CssClass="select-half-b">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblFrom" runat="server" Text="<%$ resources:From %>" AssociatedControlID="ddlFrom"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlFrom" TabIndex="3" CssClass="select-small-c">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblTo" runat="server" Text="<%$ resources:To %>" CssClass="middlelbl-xsmall"
                                                AssociatedControlID="ddlTo"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTo" TabIndex="3" CssClass="select-small-c1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblSubject" runat="server" Text="<%$ resources:SubjectReq %>" AssociatedControlID="txtSubject"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSubject" MaxLength="500" TabIndex="3" CssClass="input-full"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" onpaste="limitText(this,500);"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvSubjectSave" runat="server" ControlToValidate="txtSubject"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Subect%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvSubjectForward" runat="server" ControlToValidate="txtSubject"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="SendTo" Text="*" ErrorMessage="<%$ resources:Err_Subect%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvSubjectSendTo" runat="server" ControlToValidate="txtSubject"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="Forward" Text="*" ErrorMessage="<%$ resources:Err_Subect%>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblTags" runat="server" Text="<%$ resources:Tags %>" AssociatedControlID="txtTags"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTags" MaxLength="500" TabIndex="3" CssClass="input-full"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" onpaste="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="folder-lft-div">
                                            Folder</div>
                                        <div class="folder-rgt-div">
                                            <uc1:FileExplorer ID="FileExplorer1" runat="server" width="500" />
                                        </div>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblTitle" runat="server" Text="<%$ resources:Title %>" AssociatedControlID="txtTitle"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTitle" MaxLength="500" TabIndex="1" CssClass="input-full"
                                                onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" onpaste="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblComments" runat="server" Text="<%$ resources:Comments %>" AssociatedControlID="txtComments"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtComments" MaxLength="500" TabIndex="3" TextMode="MultiLine"
                                                CssClass="input-full margnbotm6 " onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                onpaste="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblInPrivate" runat="server" Text="<%$ resources:InPrivate %>" AssociatedControlID="chkInPrivate"></asp:Label>
                                            <asp:CheckBox Text="" runat="server" ID="chkInPrivate" TabIndex="3" />
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <h4>
                                            <%# GetLocalResourceObject("Attachments") %></h4>
                                        <div>
                                            <uc1:FileUploader runat="server" id="ucrUploader" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <h4 id="hdrDocHistory" runat="server" visible="false" class="h4-heading">
                                            <%= GetLocalResourceObject("DocumentHistory").ToString() %></h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <asp:Repeater runat="server" ID="rptComments">
                                                <ItemTemplate>
                                                    <div>
                                                        <p class="comments-container">
                                                            <%# Eval("DCT_MODE_TEXT")%>
                                                            By <span>
                                                                <%# Eval("DCT_MOD_BY_TEXT")%></span>
                                                            <%# Eval("DCT_TO_USER_TEXT") == null ? "" : " to " + "<span >" + Eval("DCT_TO_USER_TEXT").ToString() +  "</span>" %>
                                                            on <span>
                                                                <%# Convert.ToDateTime(Eval("DCT_MOD_DT")).ToString(Resources.ErpRes.LastModifiedDatetimeFormat)%>
                                                            </span>
                                                            <br />
                                                            <span class="comments-descprtn">
                                                                <%# HttpUtility.HtmlDecode(Convert.ToString(Eval("DCT_COMMENT")))%></span> <span
                                                                    style='display: <%# Eval("DCT_DESC") == null || Eval("DCT_DESC").ToString() == "" ? "none": "inherit" %>;'>
                                                                    <%# Eval("DCT_DESC")%>
                                                                </span></span>
                                                            <asp:HiddenField ID="hdfHistoryPk" runat="server" Value='<%#Eval("DCT_PK")%>' />
                                                        </p>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="divSendToUserPopupContainer" style="display: none;">
                    <div class="contentwrapper">
                        <div class="padgtop7">
                            
                                <%-- <asp:Label ID="lblSendTo" runat="server" Text="<%$ resources:SendTo%>"/>--%>
                                <label class="w17-5perc">
                                    <%= GetLocalResourceObject("SendTo").ToString()%></label>
                                <%-- <asp:DropDownList runat="server" ID="ddlSendTo" CssClass="select-half">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtSendTo" runat="server" CssClass="input-w45per"></asp:TextBox>
                                <asp:HiddenField ID="hdfSendTo" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvSendTo" runat="server" ControlToValidate="txtSendTo"
                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" CssClass="star" ValidationGroup="SendTo"
                                    Text="*" ErrorMessage="<%$ resources:Err_SendTo%>">
                                </asp:RequiredFieldValidator>
                                <asp:Button runat="server" ID="btnSendTo" CommandName="SENDTO" TabIndex="18" Text="<%$resources:Controls,Send %>"
                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('SendTo');"
                                    ValidationGroup="SendTo" ToolTip="<%$resources:Controls,Send %>" SkinID="btnInner-submit2" />
                                <div class="clear">
                                </div>
                                <div>
                                <label class="float-lft margnrgt11 w17-5perc">
                                    <%= GetLocalResourceObject("UserCC").ToString()%></label>
                                <%--<asp:Label runat="server" ID="Label1" Text="<%$ resources:UserCC%>" AssociatedControlID="CheckListSearchControl1"></asp:Label>--%>
                                <uc1:CheckListSearchControl ID="CheckListSearchControl1" runat="server" />
                                </div>
                                <div class="clear">
                                </div>
                            
                        </div>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                    <asp:ValidationSummary ID="vsPageForward" ValidationGroup="Forward" runat="server" />
                    <asp:ValidationSummary ID="vsPopupSendTo" ValidationGroup="SendTo" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="Upload" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
