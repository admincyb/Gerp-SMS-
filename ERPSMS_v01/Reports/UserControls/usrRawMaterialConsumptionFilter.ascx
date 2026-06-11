<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usrRawMaterialConsumptionFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.usrRawMaterialConsumptionFilter" %>

<%@ Register Src="~/UserControls/CheckListSearchControl.ascx" TagPrefix="uc1" TagName="CheckListSearchControl" %>
<%@ Register Src="~/UserControls/CheckListSearchControlNew.ascx" TagPrefix="uc2" TagName="CheckListSearchControlNew" %>
<script type="text/javascript">
    function usrInitComponents() {
        //usrDateInit();
        GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFromDate", "ToDate", "hdfToDate", false, false);
    }
    function usrDateInit() {
        //<summary>function used to make datepicker</summary>
        GrandScriptUtils.DatePicker("FromDate", false, false);
        GrandScriptUtils.DatePicker("ToDate", false, false);
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

    #ctl00_MainContent_userFilter_CheckListSearchControl_txtSearch + span {
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
                               <%-- <asp:Label ID="Label2" runat="server" Text="<%$ resources:MISFilterLabel,Finyear %>" AssociatedControlID="ddlFinYear"></asp:Label>
                                <asp:DropDownList ID="ddlFinYear" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>--%>
                                <asp:Label ID="lblFrom" runat="server" Text="<%$ resources:MISFilterLabel,From %>" AssociatedControlID="FromDate"></asp:Label>
                                <asp:TextBox ID="FromDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfFromDate" />
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">                            
                                

                                <asp:Label ID="lblTo" runat="server" Text="<%$ resources:MISFilterLabel,To %>" 
                                    AssociatedControlID="ToDate" CssClass="middle-lbl-d"></asp:Label>
                                <asp:TextBox ID="ToDate" runat="server" CssClass="Uidate-picker"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdfToDate" />
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

                                <%--<div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Store %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlStores" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl1" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">

<%--                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:MISFilterLabel,Classification %>" AssociatedControlID="ddlClassification"></asp:Label>
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-half"></asp:DropDownList>--%>

                                    
                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Materials %></label>
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
                                <asp:Label ID="lblTransMode" runat="server" Text="<%$ resources:MISFilterLabel,TransactionMode %>" AssociatedControlID="ddlTransMode"></asp:Label>
                                <asp:DropDownList ID="ddlTransMode" runat="server" CssClass="select-half"
                                    OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                </asp:DropDownList>

                                <%--<div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.Store %></label>
                                    <div>
                                        <asp:UpdatePanel ID="pnlStores" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl1" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>--%>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">

<%--                                <asp:Label ID="Label1" runat="server" Text="<%$ resources:MISFilterLabel,Classification %>" AssociatedControlID="ddlClassification"></asp:Label>
                                <asp:DropDownList ID="ddlClassification" runat="server" CssClass="select-half"></asp:DropDownList>--%>

                                    
                                <div class="tree-label2M w600">
                                    <label class="lbl-24perc float-left"><%=Resources.MISFilterLabel.TransactionNo %></label>
                                    <div>
                                         <asp:UpdatePanel ID="pnlTransNo" runat="server">
                                            <ContentTemplate>
                                                <uc1:CheckListSearchControl runat="server" ID="CheckListSearchControl" />
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
                                <asp:Label ID="lblPlant" runat="server" Text="<%$ resources:MISFilterLabel,Plant %>" AssociatedControlID="ddlPlant"></asp:Label>
                                <asp:DropDownList ID="ddlPlant" runat="server" CssClass="select-half"
                                    >
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
