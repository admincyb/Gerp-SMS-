<%@ Page Title="<%$ Resources:Captions,Title_EmpLeaveType %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeLeaveType.aspx.cs" Inherits="HRMS.Employees.EmployeeLeaveType"
    Theme="ClassicExt" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<%--Tab User control--%>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%--Ext Gridview control--%>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txtLimit]").ForceNumericOnly();

            $("[id*=chkAll]").live("click", function () {
                var chkHeader = $(this);
                var grid = $(this).closest("table");
                $("input[type=checkbox]", grid).each(function () {
                    if (chkHeader.is(":checked")) {
                        $(this).attr("checked", "checked");
                        $("td", $(this).closest("tr")).addClass("selected");
                    } else {
                        if ($(this).is(':disabled') == false) {
                            $(this).removeAttr("checked");
                            $("td", $(this).closest("tr")).removeClass("selected");
                        }
                    }
                    SelectChange(this);
                });
            });

            $("[id*=chkSelect]").live("click", function () {
                var grid = $(this).closest("table");
                var chkHeader = $("[id*=chkAll]", grid);
                if (!$(this).is(":checked")) {
                    $("td", $(this).closest("tr")).removeClass("selected");
                    chkHeader.removeAttr("checked");
                } else {
                    $("td", $(this).closest("tr")).addClass("selected");
                    if ($("[id*=chkSelect]", grid).length == $("[id*=chkSelect]:checked", grid).length) {
                        chkHeader.attr("checked", "checked");
                    }
                }
                SelectChange(this);
            });
        }

        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode == 8) {
                return true;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            currencyDecimal = 2;
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > currencyDecimal - 1)) {
                return false;
            }
            return true;
        }

        //        $(document).ready(function () {
        //            $("[id*=chkSelect]").live("click", function () {
        //                var chkSelected = $(this);              
        //                var limit = $(this).closest('tr').find("#[id*=hdfMstLimit]").val()
        //                if (chkSelected.is(":checked")) {
        //                    $(this).closest('tr').find("#[id*=txtLimit]").val(limit);                   
        //                } else {
        //                    $(this).closest('tr').find("#[id*=txtLimit]").val('0'); 
        //                }
        //            });

        //        });
        function SelectChange(sender) {
            var chkSelected = $(sender);
            var limit = $(sender).closest('tr').find("#[id*=hdfMstLimit]").val()
            if (chkSelected.is(":checked")) {
                $(sender).closest('tr').find("#[id*=txtLimit]").val(limit);
            } else {
                $(sender).closest('tr').find("#[id*=txtLimit]").val('0');
            }
            CalculateTotalFooter();
        }



        function CalculateTotalFooter(sender) {
            var TotalLimit = 0;
            var TotalEligible = 0;
            var TotalAvailed = 0;
            var TotalBalance = 0;
            $("#[id*=grdLeaveTypeList] input[type=text][id*=txtLimit]").each(function (index) {
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=txtLimit]").val()))) {
                    var Limit = $(this).closest('tr').find("#[id*=txtLimit]").val().replace(new RegExp(',', 'g'), '');
                    TotalLimit = TotalLimit + parseFloat(Limit);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lnkEligible]").text()))) {
                    var Eligible = $(this).closest('tr').find("#[id*=lnkEligible]").text().replace(new RegExp(',', 'g'), '');
                    TotalEligible = TotalEligible + parseFloat(Eligible);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblAvailed]").text()))) {
                    var Availed = $(this).closest('tr').find("#[id*=lblAvailed]").text().replace(new RegExp(',', 'g'), '');
                    TotalAvailed = TotalAvailed + parseFloat(Availed);
                }
                if (!isNaN(parseFloat($(this).closest('tr').find("#[id*=lblBalance]").text()))) {
                    var Balance = $(this).closest('tr').find("#[id*=lblBalance]").text().replace(new RegExp(',', 'g'), '');
                    TotalBalance = TotalBalance + parseFloat(Balance);
                }
            });

            $("#[id*=grdLeaveTypeList] [id*=lblTotLimit]").html(TotalLimit);
            $("#[id*=grdLeaveTypeList] [id*=lblTotEligible]").html(TotalEligible);
            $("#[id*=grdLeaveTypeList] [id*=lblTotAvailed]").html(TotalAvailed);
            $("#[id*=grdLeaveTypeList] [id*=lblTotBalance]").html(TotalBalance);
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveAndContinue]").hide();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="8" />
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" Text="<%$ resources:Breadcrumb%>" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSaveAndContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="63" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="Employee"
                                            OnClientClick="javascript:return ValidatePageNow('Employee')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" OnClick="ActionHandler" CommandName="SAVE"
                                            TabIndex="63" Text="Save" ToolTip="Save" ValidationGroup="Employee" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" OnClientClick="javascript:return ValidatePageNow('Employee')" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" OnClick="ActionHandler" Text="Cancel" ToolTip="Cancel"
                                            CommandName="CANCEL" TabIndex="64" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
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
                                <asp:GridView runat="server" ID="grdLeaveTypeList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    CssClass="grdTable" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler" ShowFooter="true"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="" SortExpression="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" ToolTip="<%$ resources:SelectAll%>" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# string.IsNullOrEmpty(Convert.ToString(Eval("ELV_PK"))) ? false : true%>'
                                                    onclick="SelectChange(this);"></asp:CheckBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LeaveType %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("ELV_LEAVE_TYPE_TEXT")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELV_LEAVE_TYPE_TEXT")))%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfEmpLeavePk" Value='<%# Eval("ELV_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfLeaveTypePk" Value='<%# Eval("ELV_LEAVE_TYPE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfActive" Value='<%# Eval("ELV_ACTIVE") %>' />
                                                <asp:HiddenField runat="server" ID="hdfMstLimit" Value='<%# Eval("LTM_LIMIT") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="47%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Code %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetDecodedString(Eval("ELV_LTM_CODE")),10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELV_LTM_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:MaxEligibility %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLimit" runat="server" MaxLength="5" CssClass='<%# ((Convert.ToInt32(Eval("LTM_CREDIT")) > 0) ? "input-small-c numeric" : "input-small-c numeric input-disabled")%>'
                                                    Text='<%# Eval("ELV_LIMIT") %>' onkeyup="CalculateTotalFooter();" Enabled='<%# ((Convert.ToInt32(Eval("LTM_CREDIT")) > 0) ? true : false) %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotLimit" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="5%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Eligible %>" SortExpression="">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblEligible" runat="server" Text='<%# Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) %>'
                                                    ToolTip='<%# Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) %>'></asp:Label>--%>
                                                <asp:LinkButton ID="lnkEligible" runat="server" Text='<%# Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) %>'
                                                    CssClass="text-underline" ToolTip='<%$  resources:CreditDeatils %>' CommandName="DETAILS"></asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotEligible" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Availed %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAvailed" runat="server" Text='<%# Convert.ToDouble(Eval("ELV_LEAVE_AVAILED")) %>'
                                                    ToolTip='<%# Convert.ToDouble(Eval("ELV_LEAVE_AVAILED")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotAvailed" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Balance %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# (Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) - Convert.ToDouble(Eval("ELV_LEAVE_AVAILED"))) > 0 ? (Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) - Convert.ToDouble(Eval("ELV_LEAVE_AVAILED"))) : 0  %>'
                                                    ToolTip='<%# (Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) - Convert.ToDouble(Eval("ELV_LEAVE_AVAILED"))) > 0 ? (Convert.ToDouble(Eval("ELV_LEAVE_ELIGIBLE")) - Convert.ToDouble(Eval("ELV_LEAVE_AVAILED"))) : 0  %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotBalance" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" SortExpression="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="1%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:Status %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#  Eval("ELV_ACTIVE").ToString() == "0" ? GetLocalResourceObject("Active").ToString() : GetLocalResourceObject("Inactive").ToString() %>'
                                                    ToolTip='<%#  Eval("ELV_ACTIVE").ToString() == "0" ? GetLocalResourceObject("Active").ToString() : GetLocalResourceObject("Inactive").ToString() %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divLeaveCreditDetails" style="display: none">
                                <asp:HiddenField ID="hdfCurLeaveType" runat="server" />
                                <div class="clear">
                                </div>
                                <div class="content-wrapper">
                                    <asp:GridView runat="server" ID="grdLeaveCreditDetails" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:Date%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDatePopGd" runat="server" Text='<%#Eval("ELH_DATE")!=""? Convert.ToDateTime(Eval("ELH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat):""%>'
                                                        ToolTip='<%#Eval("ELH_DATE")!=""? Convert.ToDateTime(Eval("ELH_DATE")).ToString(Resources.Constants.HRMSDateDisplayFormat):""%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="18%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Remarks%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarksPopGd" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("ELH_REMARKS")),45) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELH_REMARKS")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Credit%>  " SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLeaveCrPopGd" runat="server" Text='<%# Convert.ToDouble(Eval("ELH_LEAVE_BAL")) %>'
                                                        ToolTip='<%# Convert.ToDouble(Eval("ELH_LEAVE_BAL")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="clear">
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
