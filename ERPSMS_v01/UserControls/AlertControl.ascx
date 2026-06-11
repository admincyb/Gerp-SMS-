<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertControl.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.AlertControl" %>
<script type="text/javascript">
    function AlertUserControlInitComponents() {
        //        GrandScriptUtils.DatePickerCommon("txtTrxDate");
        //        GrandScriptUtils.DatePickerCommon("txtDueDate");
        GrandScriptUtils.AddDateRangeCommon("txtTrxDate", "hdfTrxDate", "txtDueDate", "hdfDueDate", false, false);
        $("[id$=txtNotifyBefore]").ForceNumericOnly();
        $("[id$=txtNoOfDays]").ForceNumericOnly();

        $("[id$=ddlAlertType]").change(function () {
            if ($("[id$=ddlAlertType]").val() != null && $("[id$=ddlAlertType]").val() != NaN && $("[id$=ddlAlertType]").val() != "Undefined"
                    && $("[id$=ddlAlertType]").val() != "-1") {
                if ($("[id$=txtAlertName]").val() == null || $("[id$=txtAlertName]").val() == "") {
                    var type = $("[id$=ddlAlertType] option:selected").text();
                    $("[id$=txtAlertName]").val(type);
                }
            }
        });
    }

    function ShowAlertListing(flag) {
        if (flag) {
            $("[id$=btnAlertEdit]").show();
            $("[id$=btnAlertDelete]").hide();
        }
        else {
            $("[id$=btnAlertEdit]").hide();
            $("[id$=btnAlertDelete]").show();
        }
        return false;
    }


    function ValidateAlert(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverrorAlert").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    function AfterAlertControlDateSelect(targetControlID) {
        if (targetControlID == "txtTrxDate") {
            UpdateDueDate();
        }
    }

    function UpdateDueDate() {
        var trxDate;
        var format = "dd-M-yy";
        var val;
        if (isNaN(parseInt($("[id$=txtNoOfDays]").val()))) {
            trxDate = $("[id$=txtTrxDate]").val();
            val = $.datepicker.parseDate(format, trxDate);
            $("[id$=txtDueDate]").val($.datepicker.formatDate(format, val));
        } else {
            var days = parseInt($("[id$=txtNoOfDays]").val());
            trxDate = $("[id$=txtTrxDate]").val();
            val = $.datepicker.parseDate(format, trxDate);
            var date = val;
            date.setDate(val.getDate() + days);
            //$("[id$=txtDueDate]").datepicker('setDate', date);
            $("[id$=txtDueDate]").val($.datepicker.formatDate(format, date));
        }
    }
</script>
<asp:UpdatePanel runat="server" ID="aupdpnlAlerts">
    <ContentTemplate>
        <div class="Button-container-popup">
            <asp:Button runat="server" ID="btnAlertSave" CommandName="ALERTSAVE" TabIndex="1013"
                Text="<%$resources:Controls,Save %>" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Save %>"
                CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="alert"
                OnClientClick="javascript:ValidateAlert('alert')" />
            <asp:Button runat="server" ID="btnAlertDelete" CommandName="ALERTDELETE" Text="<%$resources:Controls,Delete %>"
                OnClick="ActionHandler" TabIndex="1014" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
            <asp:Button runat="server" TabIndex="1015" ID="btnAlertEdit" CommandName="ALERTEDIT"
                OnClick="ActionHandler" Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel"
                SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>" />
            <asp:Button runat="server" ID="btnAlertReset" Text="<%$resources:Controls,Reset %>" 
                OnClick="ActionHandler" CommandName="ALERTRESET" TabIndex="1016" CommandArgument="SEC_ActionPanel"
                SkinID="btnInner-Reset" ToolTip="<%$resources:Controls,Reset %>" />
            <asp:Button runat="server" ID="btnAlertCancel" Text="<%$resources:Controls,Cancel %>"
                CssClass="popupclose" OnClick="ActionHandler" CommandName="ALERTCANCEL" TabIndex="1017"
                CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
        </div>
        <div class="content-wrapper">
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                <asp:TableRow ID="Header" runat="server">
                    <asp:TableCell>
                        <div class="detail-co2">
                            <div class="div2col-S">
                                <asp:Label runat="server" ID="lbl1" Text="<%$ resources:TrxRef%>" AssociatedControlID="lblTrxRef"></asp:Label>
                                <asp:Label runat="server" ID="lblTrxRef"></asp:Label></div>
                            <div class="div2col-S">
                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:TrxType%>" AssociatedControlID="lblTrxType"></asp:Label>
                                <asp:Label runat="server" ID="lblTrxType"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                    <asp:HiddenField ID="hdfAlertReturnURL" runat="server" Value="" />

                                        <asp:Label runat="server" ID="lblAlertType" Text="<%$ resources:AlertType%>" AssociatedControlID="ddlAlertType"></asp:Label>
                                        <asp:DropDownList runat="server" TabIndex="1001" ID="ddlAlertType">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfAlertType" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" InitialValue="-1" runat="server"
                                            ControlToValidate="ddlAlertType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AlertType %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="Label3" Text="No. of Days" AssociatedControlID="txtNoOfDays"></asp:Label>
                                        <asp:TextBox runat="server" TabIndex="1003" MaxLength="3" ID="txtNoOfDays" CssClass="small"
                                            onkeyup="UpdateDueDate();"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblAlertName" Text="<%$ resources:AlertName%>" AssociatedControlID="txtAlertName"></asp:Label>
                                        <asp:TextBox runat="server" TabIndex="1005" MaxLength="150" ID="txtAlertName"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfAlertName" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" runat="server" ControlToValidate="txtAlertName"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AlertName %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label runat="server" ID="Label4" Text="<%$ resources:TrxDate%>" AssociatedControlID="txtTrxDate"></asp:Label>
                                        <asp:TextBox runat="server" TabIndex="1002" ID="txtTrxDate" onkeydown="return CheckKey(event);"
                                            onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                        <asp:HiddenField ID="hdfTrxDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" runat="server" ControlToValidate="txtTrxDate"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TrxDate %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblDueDate" Text="<%$ resources:DueDate%>" AssociatedControlID="txtDueDate"></asp:Label>
                                        <asp:TextBox runat="server" TabIndex="1004" ID="txtDueDate" onkeydown="return CheckKey(event);"
                                            onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                        <asp:HiddenField ID="hdfDueDate" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="vrfDueDate" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" runat="server" ControlToValidate="txtDueDate"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DueDate %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblNotifyBefore" Text="<%$ resources:NotifyBefore%>"
                                            AssociatedControlID="txtNotifyBefore"></asp:Label>
                                        <asp:TextBox runat="server" TabIndex="1006" MaxLength="3" ID="txtNotifyBefore" CssClass="small"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="vrfNotifyBefore" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" runat="server" ControlToValidate="txtNotifyBefore"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_NotifyBefore %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:DropDownList runat="server" TabIndex="1007" ID="ddlNotifyBefore" CssClass="medium">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfNotifyBeforeUOM" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="alert" EnableClientScript="true" InitialValue="-1" runat="server"
                                            ControlToValidate="ddlNotifyBefore" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_NotifyBeforeUOM %>">
                                        </asp:RequiredFieldValidator>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-P">
                                        <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtRemarks" TabIndex="1008" MaxLength="400" onkeydown="limitText(this,400);"
                                            onkeyup="limitText(this,400);"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-P">
                                        <asp:Label ID="lblNotifyAll" runat="server" Text="<%$ resources:NotifyAll %>" AssociatedControlID="chkNotifyAll"></asp:Label>
                                        <asp:CheckBox ID="chkNotifyAll" TabIndex="1009" runat="server" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="lblCompleted" runat="server" Text="<%$ resources:Completed %>" AssociatedControlID="chkCompleted"></asp:Label>
                                        <asp:CheckBox ID="chkCompleted" TabIndex="1011" runat="server" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="chk-label-left">
                                        <asp:Label runat="server" ID="Label1" Text="<%$ resources:NotifyThrough%>" AssociatedControlID="chkNotifyThrough"></asp:Label>
                                        <asp:CheckBoxList runat="server" TabIndex="1010" ID="chkNotifyThrough" TextAlign="Right"
                                            CssClass="chk-box">
                                        </asp:CheckBoxList>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="gridwrap">
                            <asp:GridView runat="server" ID="grdAlert" Width="100%" PageSize="25" AutoGenerateColumns="false"
                                EmptyDataRowStyle-CssClass="emptytable">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton CssClass="rdoSelection" TabIndex="1012" runat="server" GroupName="SelectOne"
                                                AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                                ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            <asp:HiddenField runat="server" ID="hdfAlertPK" Value='<%# Eval("ATH_PK") %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:DueDate %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ATH_DUE_DATE", Resources.Constants.DateFormatGrid).ToString()%>'
                                                ToolTip='<%# Eval("ATH_DUE_DATE", Resources.Constants.DateFormatGrid).ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:AlertName%>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("ATH_NAME")%>' ToolTip='<%# Eval("ATH_NAME")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblThickness" runat="server" Text='<%# Eval("ATH_ALERT_TYPE_TEXT")%>'
                                                ToolTip='<%# Eval("ATH_ALERT_TYPE_TEXT")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:AlertOn %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("ATH_ALERT_ON", Resources.Constants.DateFormatGrid).ToString()%>'
                                                ToolTip='<%#Eval("ATH_ALERT_ON", Resources.Constants.DateFormatGrid).ToString()%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:User %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSurface" runat="server" Text='<%# Eval("ATH_NOTIFY_USER_TEXT")%>'
                                                ToolTip='<%# Eval("ATH_NOTIFY_USER_TEXT")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShade" runat="server" Text='<%# Eval("ATH_STATUS_TEXT")%>' ToolTip='<%# Eval("ATH_STATUS_TEXT")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsAlert" ValidationGroup="alert" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
