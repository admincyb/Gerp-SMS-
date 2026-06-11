<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrStockCardReportFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrStockCardReportFilter" %>

<%@ Register Src="~/UserControls/CheckListSearchControl.ascx" TagPrefix="uc1" TagName="CheckListSearchControl" %>
<%@ Register Src="~/UserControls/CheckListSearchControlNew.ascx" TagPrefix="uc2" TagName="CheckListSearchControlNew" %>
<script type="text/javascript">
    function usrInitComponents() {
        //usrDateInit();
        //GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, true);
        //GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, false, false);
        //GrandScriptUtils.RestrictedYearDatePicker("FromDate", false, true, true, $("[id$=FromDate]").val());
        //GrandScriptUtils.RestrictedYearDatePicker("ToDate", false, true, true, '', $("[id$=ToDate]").val());
        //GrandScriptUtils.RestrictedYearDatePicker("FromDate", false, true, true, $("[id$=FromDate]").val(), $("[id$=ToDate]").val());

        GrandScriptUtils.RestrictedYearDatePicker("FromDate", false, true, true, $("[id$=hdfFromDate]").val(), $("[id$=hdfToDate]").val());
        GrandScriptUtils.RestrictedYearDatePicker("ToDate", false, true, true, $("[id$=hdfFromDate]").val(), $("[id$=hdfToDate]").val());
        //GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, true);
    }
    function usrDateInit() {
        //<summary>function used to make datepicker</summary>
        //GrandScriptUtils.DatePicker("FromDate", false, false,);
        //GrandScriptUtils.DatePicker("ToDate", false, false);
        GrandScriptUtils.RestrictedDatePicker("FromDate", false, true, true, $("[id$=FromDate]").val(), $("[id$=ToDate]").val());
    }

</script>
<style>
    #ctl00_MainContent_userFilter_CheckListSearchControlNew_txtSearchItem + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    #spnCount {
        display: none
    }

    #ctl00_MainContent_userFilter_CheckListSearchControl1_txtSearch + span {
        background: none;
        padding: 0;
        min-height: 0;
        border: none;
        margin-top: 2px !important;
    }

    .input-w81per {
        min-width: 79% !important;
        max-width: 79% !important;
    }

    .treelist-scroll {
        height: 100px;
        overflow: auto;
        margin-bottom: 10px;
        width: 362px;
    }
</style>

<asp:UpdatePanel ID="pnlTestFilter" runat="server" class="">
    <ContentTemplate>
        <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
            <div class="fields-group">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" AssociatedControlID="ddlFinYear"></asp:Label>
                                <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvFinYear" InitialValue="-1" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" runat="server" ControlToValidate="ddlFinYear" ValidationGroup="fltr"
                                    Display="Static" Text="*" ErrorMessage="<%$ resources:MISFilterLabel,Err_SelectFinYear%>">
                                </asp:RequiredFieldValidator>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblFrom" runat="server" Text="<%$ resources:MISFilterLabel,From %>" AssociatedControlID="FromDate"></asp:Label>
                                <asp:TextBox ID="FromDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfFromDate" />
                                <%--<asp:RangeValidator ID="rgfvFromDate" runat="server" ControlToValidate="FromDate"  CssClass="star" SetFocusOnError="true" 
                                   EnableClientScript="true" ValidationGroup="fltr" ErrorMessage="<%$ resources:MISFilterLabel,Err_SelectFinYearFromDate%>" 
                                    Display="Static" Text="*"  />--%>

                                <asp:Label ID="lblTo" runat="server" Text="<%$ resources:MISFilterLabel,To %>"
                                    AssociatedControlID="ToDate" CssClass="lbl-30perc"></asp:Label>
                                <asp:TextBox ID="ToDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfToDate" />
                                <%--<asp:RangeValidator ID="rgfvToDate" runat="server" ControlToValidate="ToDate" CssClass="star" SetFocusOnError="true"
                                    EnableClientScript="true" ValidationGroup="fltr" ErrorMessage="<%$ resources:MISFilterLabel,Err_SelectFinYearToDate%>"
                                    Display="Static" Text="*" />--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblItemCategory" runat="server" Text="<%$ resources:MISFilterLabel,ItemCategory %>" AssociatedControlID="ddlItemCategory"></asp:Label>
                                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>

                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Store %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlStores" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl1" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">

                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:MISFilterLabel,Classification %>" AssociatedControlID="ddlClassification"></asp:Label>
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-half"></asp:DropDownList>


                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Items %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlItemChecklist" runat="server">
                                            <ContentTemplate>
                                                <uc2:CheckListSearchControlNew runat="server" ID="CheckListSearchControlNew" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <asp:Label ID="lblTransaction" runat="server" Text="<%$ resources:MISFilterLabel,TransactionsOnly %>"
                                    CssClass="label-11-26" AssociatedControlID="chkTransaction"></asp:Label>
                                <asp:CheckBox ID="chkTransaction" runat="server" />

                                <asp:Label ID="lblExcludeMatReturn" runat="server" Text="<%$ resources:MISFilterLabel,ExcludeMaterialReturn %>"
                                    AssociatedControlID="chkExcludeMatReturn"></asp:Label>
                                <asp:CheckBox ID="chkExcludeMatReturn" runat="server" />
                            </div>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <%--   <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
           <asp:ValidationSummary ID="vsFilterPage" ValidationGroup="fltr" runat="server" />
        </div>--%>
    </ContentTemplate>
</asp:UpdatePanel>
