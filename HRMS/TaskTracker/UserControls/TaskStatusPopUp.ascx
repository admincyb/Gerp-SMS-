<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskStatusPopUp.ascx.cs"
    Inherits="HRMS.TaskTracker.UserControls.TaskStatusPopUp" %>
<script type="text/javascript">
    function TaskStatusPopUpInitComponents() {
        //GrandScriptUtils.DatePickerCommon("txtTaskDate");
        GrandScriptUtils.DatePickerCommon("txtStatusDate");
        //GrandScriptUtils.DatePickerCommon("txtMainTaskExpDate");
        GrandScriptUtils.DatePickerCommon("txtExpDate");

        //GrandScriptUtils.MakeAutoCompleteDDL("txtUser", url, "hdfUserPK", true, true, "ASSIGNTASKUSER");
        GrandScriptUtils.AddDateRangeCommon("txtStatusDate", "hdfCreateDate", "txtExpDate", "hdfExpDate", false, false);
    }

    function ValidateNow(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverrorStatus").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    function AfterClose() {
        $('[id$=btnClosePopUp]').click();
    }
</script>
<style type="text/css">
    .task-detail
    {
        display: inline-block;
        width: 100%;
    }
    .task-detail label
    {
        width: 30%;
    }
    .task-detail span, .task-detail input[type=text], .task-detail textarea, .task-detail select, .task-detail input[type=password]
    {
        width: 62%;
    }
    .task-detail span
    {
        padding: 3px 2px;
        min-height: 14px;
        line-height: normal;
        font-weight: bold;
        color: #506c92;
    }
    .task-detail input[type=checkbox]
    {
        margin: 3px 0 0 0;
    }
    .task-detail select
    {
        width: 63.4%;
    }
    .task-detail input[type="file"]
    {
        width: auto !important;
    }
    .task-detail .lbDt
    {
        width: 40%;
    }
    .task-detail .lbShwDt
    {
        width: 50%;
    }
    /*width half*/
    .task-detail input[type=text].half
    {
        width: 30%;
    }
    .task-detail span.half
    {
        width: 30%;
    }
    
    /*Column1*/
    .task-detail-col1 label
    {
        width: 15% !important;
    }
    .task-detail-col1 span, .task-detail-col1 input[type=text], .task-detail-col1 textarea, .task-detail-col1 select
    {
        width: 81%;
    }
    .task-detail-col1 span
    {
        padding: 2px;
        min-height: 20px;
        line-height: normal;
        font-weight: bold;
        color: #506c92;
    }
    .task-detail-col1 select
    {
        width: 82%;
    }
    .task-detail-col1 textarea
    {
        height: 60px;
        padding-top: 1px;
    }
    
    /*Notify*/
    .notify
    {
        background: #eff0f1;
        border: 1px solid #dcdcdc;
        padding: 3px;
        margin: 10px;
    }
    
    /*blank td*/
    .blank
    {
        height: 10px;
    }
</style>
<asp:UpdatePanel runat="server" ID="aupdpopup">
    <ContentTemplate>
        <div class="content-wrapper">
            <div class="Button-container-popup">
                <asp:Button runat="server" ID="btnStatusSave" CommandName="SAVE" OnClick="ActionHandler"
                    Text="<%$resources:Controls,Save %>" ToolTip="<%$resources:Controls,Save %>"
                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save1" ValidationGroup="SaveStatus"
                    OnClientClick="javascript:ValidateNow('SaveStatus')" />
                <asp:Button runat="server" ID="btnStatusCancel" Text="<%$resources:Controls,Cancel %>"
                    CssClass="popupclose" CommandName="CANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                    SkinID="btnInner-Cancel2" ToolTip="<%$resources:Controls,Cancel %>" />
                <asp:Button runat="server" ID="btnClosePopUp" Style="display: none;" CommandName="CANCELPOPUP"
                    OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
            </div>
            <asp:Table runat="server" ID="tblTemplate">
                <asp:TableRow ID="TaskInfo" runat="server" CssClass="notify">
                    <asp:TableCell Style="padding-left: 57px;">
                        <table class="table-3devide" style="margin: 5px 0px;">
                            <tr>
                                <td colspan="2">
                                    <div class="task-detail-col1">
                                        <asp:Label runat="server" ID="lblTaskName" Text="<%$resources:TskName %>" AssociatedControlID="lbShowTaskName"></asp:Label>
                                        <asp:Label ID="lbShowTaskName" runat="server"> </asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lblTaskDate" Text="<%$resources:TskDt %>" AssociatedControlID="lbShowTaskDate"></asp:Label>
                                        <asp:Label ID="lbShowTaskDate" runat="server" EnableTheming="false"> </asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lblTaskNo" Text="<%$resources:TskNo %>" AssociatedControlID="lbShowTaskNo"></asp:Label>
                                        <asp:Label ID="lbShowTaskNo" runat="server"> </asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lbTskExpdt" Text="<%$resources:TskExpDt %>" AssociatedControlID="lbShowTskExpdt"
                                            CssClass="lbDt"></asp:Label>
                                        <asp:Label ID="lbShowTskExpdt" runat="server" CssClass="lbShwDt"> </asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="task-detail">
                                        <asp:Label runat="server" ID="lbTskStatus" Text="<%$resources:TskStatus %>" AssociatedControlID="lbShowTskStatus"></asp:Label>
                                        <asp:Label ID="lbShowTskStatus" runat="server"> </asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass=" blank"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trStatus" runat="server">
                    <asp:TableCell>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbStatDate" Text="<%$resources:StatusDate %>" AssociatedControlID="txtStatusDate"></asp:Label>
                                        <asp:TextBox ID="txtStatusDate" runat="server" MaxLength="13" onkeydown="return CheckKey(event)"
                                            onpaste="return false;"> </asp:TextBox>
                                        <asp:HiddenField ID="hdfCreateDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="SaveStatus"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtStatusDate" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Err_date %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="lbStatus" Text="<%$resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ActionHandler"
                                            CommandName="CHANGESTATUS" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfStatus" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="SaveStatus" EnableClientScript="true" runat="server" ControlToValidate="ddlStatus"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Status %>" InitialValue="-1"></asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <div id="divExpDate" runat="server">
                                            <asp:Label runat="server" ID="lbExpDate" Text="<%$resources:StatusExpDate %>" AssociatedControlID="txtExpDate"></asp:Label>
                                            <asp:TextBox ID="txtExpDate" runat="server" MaxLength="13" onkeydown="return CheckKey(event)" CssClass="select-small-b"
                                                onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfExpDate" runat="server" Value="" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label runat="server" ID="lbRemarks" Text="<%$resources:StatusRemarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"> </asp:TextBox>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfTaskPK" runat="server" Value="0" />
            <div id="diverrorStatus" style="display: none">
                <asp:ValidationSummary ID="vsStatus" ValidationGroup="SaveStatus" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
