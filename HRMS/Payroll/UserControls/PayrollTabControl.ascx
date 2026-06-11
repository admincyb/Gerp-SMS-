<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayrollTabControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.PayrollTabControl" %>
<div class="tab-container" style="padding-bottom: 0px !important;" id="divTabContainer"
    runat="server">
    <ul id="tab-menu">
        <li id="liPreprocessData" runat="server"><span id="spnPreprocessData" runat="server" class="tab-active">
            <asp:LinkButton runat="server" ID="lnbPreprocessData" Text="<%$resources:PageNameRes,HrmsPreprocessData %>"
                CommandArgument="SEC_ActionPanel" TabIndex="250" CssClass="tab-inactive" OnClick="ActionHandler"
                CommandName="PREPROCESSDATA"></asp:LinkButton>
        </span></li>
        <li id="liSalaryProcess" runat="server"><span id="spnSalaryProcess" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbSalaryProcess" Text="<%$resources:PageNameRes,HrmsSalaryProcess %>"
                CommandArgument="SEC_ActionPanel" TabIndex="251" OnClick="ActionHandler" CommandName="SALARYPROCESS"
                CssClass="tab-active"></asp:LinkButton>
        </span></li>
    </ul>
</div>
<asp:HiddenField runat="server" ID="hdfCurrentDepartment" Value="-1" />
