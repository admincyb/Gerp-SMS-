<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpBasicInfoControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.EmpBasicInfoControl" %>
<style type="text/css">
    .header
    {
        background: #EFF0F1;
        border: 1px solid #dcdcdc;
        padding: 3px;
        margin-bottom: 10px;
    }
</style>
<div id="divEmployeeHeader" runat="server" border="0" cellpadding="0" cellspacing="0"
    class="header">
    <%--class="detail-co2"--%>
    <table style="width: 100%;">
        <%--class="table-4devide" border="0" cellpadding="0" cellspacing="0"--%>
        <tr>
            <td style="width: 1%;">
            </td>
            <td style="width: 33%;">
                <asp:Label ID="lblhdrEmployeeNo" runat="server" AssociatedControlID="lblhdrEmployeeTxt"
                    Text="<%$ resources:Employee%>" class="margnbotm0"></asp:Label>
                <asp:Label ID="lblhdrEmployeeTxt" runat="server" Text="" class="margnbotm0"></asp:Label>
            </td>
            <td style="width: 18%;">
                <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="<%$ resources:DOJ:%>"
                    class="margnbotm0"></asp:Label>
                <asp:Label ID="lblhdrDOJText" runat="server" Text="" class="margnbotm0"></asp:Label>
            </td>
            <td style="width: 18%;">
                <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                    Text="<%$ resources:Designation:%>" class="margnbotm0"></asp:Label>
                <asp:Label ID="lblhdrDesignationTxt" runat="server" Text="" class="margnbotm0"></asp:Label>
            </td>
            <td style="width: 12%;">
                <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                    Text="<%$ resources:Department:%>" class="margnbotm0"></asp:Label>
                <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text="" class="margnbotm0"></asp:Label>
            </td>
            <td style="width: 12%;">
                <asp:Label ID="Label1" runat="server" AssociatedControlID="lblhdrLocationTxt" Text="<%$ resources:Location%>"
                    class="margnbotm0"></asp:Label>
                <asp:Label ID="lblhdrLocationTxt" runat="server" Text="" class="margnbotm0"></asp:Label>
            </td>
            <td style="width: 1%;">
                <asp:ImageButton ID="btnInfo" runat="server" OnClick="ActionHandler" Visible="<%$ resources:ConfigurationsRes,HrmsEmpInformationVisible%>" CommandName="DETAIL"
                    SkinID="information" ToolTip="<%$ resources:Information%>" class="margnbotm0" />
            </td>
        </tr>
    </table>
</div>
<div id="divInfoPopUp" style="display: none;">
    <div class="content-wrapper">
        <div class="header">
            <table style="width: 100%">
                <tr>
                    <td style="width: 60%;">
                        <asp:Label ID="lblEmpNameH" runat="server" AssociatedControlID="lblEmpName" Text="<%$ resources:Employee%>"
                            class="margnbotm0"></asp:Label>
                        <asp:Label ID="lblEmpName" runat="server" Text="" class="margnbotm0"></asp:Label>
                    </td>
                    <td style="width: 40%;">
                        <asp:Label ID="lblEmpLocH" runat="server" AssociatedControlID="lblEmpLoc" Text="<%$ resources:Location%>"
                            class="margnbotm0"></asp:Label>
                        <asp:Label ID="lblEmpLoc" runat="server" Text="" class="margnbotm0"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div3col-S">
                        <asp:Label runat="server" ID="Label3" Text="<%$ resources:ID%>" CssClass="margntop4 margnbotm5"
                            AssociatedControlID="lblID"></asp:Label>
                        <asp:Label ID="lblID" Text="" runat="server" />
                        <div class="clear">
                        </div>
                        <asp:Label ID="Label4" runat="server" Text="<%$resources:PassportNo %>" CssClass="margntop4 margnbotm5"
                            AssociatedControlID="lblPassportNumber"></asp:Label>
                        <asp:Label ID="lblPassportNumber" Text="" runat="server" />
                    </div>
                </td>
                <td>
                    <div class="div3col-S">
                        <asp:Label ID="Label5" runat="server" Text="<%$resources:EmploymentType %>" CssClass="margntop4 margnbotm5"
                            AssociatedControlID="lblEmployementType"></asp:Label>
                        <asp:Label ID="lblEmployementType" Text="" runat="server" />
                        <div class="clear">
                        </div>
                        <asp:Label ID="Label2" runat="server" Text='<%$ Resources:DOB %>' CssClass="margntop4 margnbotm5"
                            AssociatedControlID="lblDOB"></asp:Label>
                        <asp:Label ID="lblDOB" Text="" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:HiddenField runat="server" ID="hdfCurrentDepartment" Value="-1" />
