<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GtiTabControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.GtiTabControl" %>
<div class="tab-container" style="padding-bottom: 0px !important;" id="divTabContainer"
    runat="server">
    <ul id="tab-menu">
        <li id="liEmpListing" runat="server"><span id="spnEmpListing" runat="server" class="tab-active">
            <asp:LinkButton runat="server" ID="lnbEmpListing" Text="<%$resources:PageNameRes,HrmsEmpList %>"
                CommandArgument="SEC_ActionPanel" TabIndex="250" CssClass="tab-inactive" OnClick="ActionHandler"
                CommandName="DEFAULT"></asp:LinkButton>
        </span></li>
        <li id="liBasicDetails" runat="server"><span id="spnBasicDetails" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbBasicDetails" Text="<%$resources:PageNameRes,HrmsBasicDetails %>"
                CommandArgument="SEC_ActionPanel" TabIndex="251" OnClick="ActionHandler" CommandName="BASICDETAILS"
                CssClass="tab-active"></asp:LinkButton>
        </span></li>
        <li id="liQualifications" runat="server"><span id="spnQualifications" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbQualifications" Text="<%$resources:PageNameRes,HrmsQualifications %>"
                CommandArgument="SEC_ActionPanel" TabIndex="252" OnClick="ActionHandler" CommandName="QUALIFICATIONS"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liExperience" runat="server"><span id="spnExperience" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbExperience" Text="<%$resources:PageNameRes,HrmsExperience %>"
                CommandArgument="SEC_ActionPanel" TabIndex="253" OnClick="ActionHandler" CommandName="EXPERIENCE"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liSkillDetails" runat="server"><span id="spnSkillDetails" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbSkillDetails" Text="<%$resources:PageNameRes,HrmsSkillDetails %>"
                CommandArgument="SEC_ActionPanel" TabIndex="254" OnClick="ActionHandler" CommandName="SKILLDETAILS"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liDocuments" runat="server"><span id="spnDocuments" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbDocuments" Text="<%$resources:PageNameRes,HrmsDocuments %>"
                CommandArgument="SEC_ActionPanel" TabIndex="255" OnClick="ActionHandler" CommandName="DOCUMENTS"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
      <%--  <li id="liPayDetails" runat="server"><span id="spnPayDetails" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbPayDetails" Text="<%$resources:PageNameRes,HrmsPayDetails %>"
                CommandArgument="SEC_ActionPanel" TabIndex="256" OnClick="ActionHandler" CommandName="PAYDETAILS"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>--%>
        <li id="liLeaveType" runat="server"><span id="spnLeaveType" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbLeaveType" Text="<%$resources:PageNameRes,EmployeeLeaveType %>"
                CommandArgument="SEC_ActionPanel" TabIndex="257" OnClick="ActionHandler" CommandName="LEAVETYPE"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liSalaryDetails" runat="server"><span id="spnSalaryDetails" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbSalaryDetails" Text="<%$resources:PageNameRes,HrmsSalaryDetails %>"
                CommandArgument="SEC_ActionPanel" TabIndex="258" OnClick="ActionHandler" CommandName="SALARYDETAILS"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liSalaryRevision" runat="server"><span id="spnSalaryRevision" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbSalaryRevision" Text="<%$resources:PageNameRes,HrmsAppraisal %>"
                CommandArgument="SEC_ActionPanel" TabIndex="259" OnClick="ActionHandler" CommandName="SALARYREVISION"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liItDeclaration" runat="server"><span id="spnItDeclaration" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbItDeclaration" Text="<%$resources:PageNameRes,HrmsItDeclaration %>"
                CommandArgument="SEC_ActionPanel" TabIndex="260" OnClick="ActionHandler" CommandName="ITDECLARATION"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
                <li id="liBehaviour" runat="server" ><span id="spnHrmsBehaviour" runat="server" class="tab-inactive" >
            <asp:LinkButton runat="server" ID="lnbHrmsBehaviour" Text="<%$resources:PageNameRes,HrmsBehaviour %>"
                CommandArgument="SEC_ActionPanel" TabIndex="260" OnClick="ActionHandler" CommandName="BEHAVIOUR"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
        <li id="liTraining" runat="server"><span id="spnHrmsTraining" runat="server" class="tab-inactive">
            <asp:LinkButton runat="server" ID="lnbHrmsTraining" Text="<%$resources:PageNameRes,HrmsTraining %>"
                CommandArgument="SEC_ActionPanel" TabIndex="260" OnClick="ActionHandler" CommandName="TRAINING"
                CssClass="tab-inactive"></asp:LinkButton>
        </span></li>
    </ul>
</div>
