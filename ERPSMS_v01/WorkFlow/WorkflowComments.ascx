<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowComments.ascx.cs"
    Inherits="ERPSMS_v01.WorkFlow.WorkflowComments" %>
<script type="text/javascript">

    //     function ValidateNow() {
    //         if (typeof (Page_ClientValidate) == 'function') {
    //             Page_ClientValidate();
    //         }
    //         if (!Page_IsValid) {
    //             $("#litErrorMsg").hide();

    //             ShowErrorMessage($("#divWrkferror").html());
    //             return false;  //Page is invalid -- stop right here
    //         }
    //         else {
    //             //everythings ok --- Call your function & do your stuff
    //             return true;
    //         }
    //     }

    function ValidateNow(validationGroup) {

        if (typeof (Page_ClientValidate) == 'function') {
            Page_ClientValidate(validationGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#divWrkferror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }


    function ShowComments() {
        var currentHeight = $("[id$=wrkDiv]").height();
        GrandScriptUtils.ShowWorkFlowCommandList();
        var newHeight = $("[id$=wrkDiv]").height();
        var frameHeight = $("[id$=frmDetails]", parent.document).height();
        $("[id$=frmDetails]", parent.document).height((frameHeight - currentHeight) + newHeight);
        $("[id$=innerPage-wrap]").height((frameHeight - currentHeight) + newHeight);
    }

    function HideComments() {
        var currentHeight = $("[id$=wrkDiv]").height();
        GrandScriptUtils.HideWorkFlowCommandList();
        var newHeight = $("[id$=wrkDiv]").height();
        var frameHeight = $("[id$=frmDetails]", parent.document).height();
        $("[id$=frmDetails]", parent.document).height((frameHeight - currentHeight) + newHeight);
        $("[id$=innerPage-wrap]").height((frameHeight - currentHeight) + newHeight);
    }

    function ShowWorkFlowFailMessage() {

        var msgTitle;
        var msg;
        msgTitle = 'Workflow';
        msg = '<%=Resources.Messages.ErrMsg_WKF_Failed %>'; //'Workflow Server is busy.Please try Again.';
        $("#divConfirmation").html(msg).dialog({
            modal: true,
            height: 150,
            width: 350,
            title: msgTitle,
            resizable: false,
            buttons: {
                OK: function (e) {
                    $(this).dialog("close");
                    return false;
                },
                Cancel: function (e) {
                    $(this).dialog("close");
                    return false;
                }
            }
        });
        return false;

    }

    function SaveWorkFlow(isUpdate, type, addComment) {
        var wrkfReq = new Object();
        wrkfReq.ActionID = parseInt($("[id$=WRKFACT_ID]").val());
        wrkfReq.ProcessID = parseInt($("[id$=hdfProcessID]").val());
        wrkfReq.TaskID = parseInt($("[id$=TaskPK]").val());
        wrkfReq.ApplicationID = parseInt($("[id$=hdfAppID]").val());
        wrkfReq.UserPK = parseInt($("[id$=UserPk]").val());
        wrkfReq.ReferenceID = parseInt($("[id$=ReferenceID]").val());
        wrkfReq.WrkfComment = $("[id$=WrkfComments]").val();
        wrkfReq.wrkfAdditionalComments = addComment;
        var jsonString = JSON.stringify(wrkfReq);
        $.post("CommonManagement.do?Action=SaveWorkFlow", jsonString, function (data) {
            $('#updateProgress').hide();
            if (parseInt(data) > 0) {
                $('#popupHolder').hide();
                if (isUpdate) {
                    $("[id$=ReferenceID]").val(data);
                    // UpdateReferenceID(type);
                    ShowWorkflowSaveMsg();
                }
                else {
                    ShowWorkflowSaveMsg();
                }
            }
            else {
                ShowWorkFlowFailMessage();
            }
        });
        return false;
    }

    function UpdateReferenceID(type) {

        var wrkfReq = new Object();
        wrkfReq.ApplicationID = parseInt($("[id$=hdfAppID]").val());
        wrkfReq.UserPK = parseInt($("[id$=UserPk]").val());
        wrkfReq.ReferenceID = parseInt($("[id$=ReferenceID]").val());
        var jsonString = JSON.stringify(wrkfReq);
        $.post("CommonManagement.do?Action=UpdateReferenceID&Type=" + type, jsonString, function (data) {
            if (parseInt(data) > 0) {
                ShowWorkflowSaveMsg();
            }
        });
    }


</script>
<div class="content-wrapper">
    <div class="comments-container" id="div1">
        <div class="submitwrap commentswrap" id="divActionComments" runat="server">
            <label for="WRKFACT_ID">
                <%=Resources.Controls.Action %></label>
            <asp:DropDownList runat="server" CssClass="half" ID="WRKFACT_ID" TabIndex="101" Width="320px" onmouseover="javascript:ShowTooltip('WRKFACT_ID');">
            </asp:DropDownList>
            <asp:Button runat="server" ID="btnSubmitWrkf" ToolTip="<%$Resources:Controls,Submit %>"
                SkinID="btnInner-submit" Text="Submit" OnClientClick="javascript:return SavePage('Submit');" />
            <asp:Button runat="server" ID="btnSubmitDelegate" ToolTip="<%$Resources:Controls,Submit %>"
                CssClass="inputbtnNoFloat" Text="" EnableTheming="false" OnClick="wrkfSubmit_Click"
                Style="width: 140px; display: none; visibility: hidden" />
            <div class="clear">
            </div>
            <label for="VendorComments">
                <%=Resources.Controls.Comments %></label>
            <asp:TextBox ID="WrkfComments" runat="server" TextMode="MultiLine" Height="35px"
                TabIndex="100" EnableTheming="false"></asp:TextBox>
            <asp:HiddenField ID="ReferenceID" Value="0" runat="server" />
            <asp:HiddenField ID="TaskPK" Value="0" runat="server" />
            <div id="CommentsList">
                <asp:HiddenField ID="CommentsObject" Value="0" runat="server" />
            </div>
        </div>

        <%--Page Comments --%>
        <div id="divPageComments" runat="server" class="div-notify" style="display: none">
            <asp:Label ID="lblPageComment" Text="" runat="server"></asp:Label>
        </div>
        <%--Budget --%>
        <div id="divbudget" runat="server" class="div-notify" visible="false">
            <h4 class="w280 disp-inline">Budget Amount
            </h4>
            <div class="clear">
            </div>
            <div id="divWrkfBudget" class="gridwrap2 max-150 border-top">
                <asp:GridView runat="server" ID="grdChangeBudget" AutoGenerateColumns="false" Width="100%" OnRowDataBound="ActionHandler">
                    <Columns>
                        <asp:BoundField DataField="BGH_TOTAL_BUDGET" HeaderText="<%$ Resources:Controls, TotalBudget %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                        <asp:BoundField DataField="BGH_TOTAL_USED" HeaderText="<%$ Resources:Controls, TotalUsed %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                        <asp:BoundField DataField="BGH_EXPECTED_AMT" HeaderText="<%$ Resources:Controls, ExpectedAmt %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                        <asp:BoundField DataField="BGH_TOTAL_BALANCE" HeaderText="<%$ Resources:Controls, TotalBal %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <%-- Change History region Start- ----------%>
        <div id="divOuterWkfChangeHistory" runat="server" visible="false" class="div-notify">
            <h4 class="w280 disp-inline">
                <%=Resources.Controls.ChangeHistory%>
            </h4>
            <span id="spnVerify" runat="server" class="h18 float-right margntop5 margnbotm0 margn-rgt1">
                <asp:Label runat="server" ID="lblVerifyChanges" Text="<%$ Resources:Controls, VerifyChanges %>" AssociatedControlID="chkVerifyChanges" CssClass="margnbotm0"></asp:Label>
                <asp:CheckBox ID="chkVerifyChanges" runat="server" CssClass="margntop2 margnbotm0" EnableViewState="true" />
            </span>
            <div class="clear">
            </div>
            <div id="divWrkfChangeHistory" class="gridwrap2 max-150 border-top">
                <asp:GridView runat="server" ID="grdChangeHistory" AutoGenerateColumns="false" Width="100%" OnRowDataBound="ActionHandler">
                    <Columns>
                        <asp:BoundField DataField="ITM_CODE" HeaderText="<%$ Resources:Controls, ItemCode %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                        <asp:BoundField DataField="RATE_PREV" HeaderText="<%$ Resources:Controls, PrevRate %>"
                            HeaderStyle-CssClass="amount-numeric" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="RATE" HeaderText="<%$ Resources:Controls, NewRate %>"
                            HeaderStyle-CssClass="amount-numeric" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                        <asp:BoundField DataField="AMOUNT_PREV" HeaderText="<%$ Resources:Controls, PrevAmount %>"
                            HeaderStyle-CssClass="amount-numeric" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="AMOUNT" HeaderText="<%$ Resources:Controls, AmountNew %>"
                            HeaderStyle-CssClass="amount-numeric" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                        <asp:BoundField DataField="Reason" HeaderText="<%$ Resources:Controls, Reason %>"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="30%" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <%-- End Change History region - ----------%>
    </div>



    <h4>
        <%=Resources.Controls.Log%>
    </h4>
    <%--<div class="commentswrap">--%>
    <div class="clear">
    </div>
    <div id="divConfirmation">
    </div>
    <div id="divWrkfComment" class="gridwrap max-250" runat="server">
        <asp:GridView runat="server" ID="grdComments" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="cmtDate" HeaderText="<%$ Resources:Controls, Date %>"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="24%" />
                <asp:BoundField DataField="acnName" HeaderText="<%$ Resources:Controls, Action %>"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="23%" />
                <asp:BoundField DataField="cmtModByText" HeaderText="<%$ Resources:Controls, User %>"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="18%" />
                <asp:BoundField DataField="cmtDesc" HeaderText="<%$ Resources:Controls, Comments %>"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="35%" />
            </Columns>
        </asp:GridView>
    </div>
    <%--    </div>--%>
    <div id="divWrkferror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vvswrkfSummary" ValidationGroup="Save" runat="server" />
    </div>
    <div class="clear">
    </div>
    <asp:HiddenField ID="hdnVerificationRequired" runat="server" Value="0" />
    <asp:HiddenField ID="hdfDefaultActionPk" runat="server" Value="0" />
    <asp:HiddenField ID="hdfCurrencyFormatWithSeperator" runat="server" />
    <asp:HiddenField ID="hdfRateFormat" runat="server" />
</div>
