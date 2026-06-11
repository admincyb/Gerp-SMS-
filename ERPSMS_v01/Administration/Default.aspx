<%@ Page Title="gERP – ADMIN" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ERPSMS_v01.Administration.Default"
    Theme="ERP-Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="clear">
    </div>
    <div class="contentboxwrap">
        <h1>
            Configuration</h1>
        <ul>
            <li><a href="Configurations/ProjectSite.aspx">Project Site</a></li>   <%--EKK Only --%>
            <li><a href="Configurations/UsersList.aspx">User Management</a></li>   <%--EKK Only --%>
            <li><a href="Configurations/SBUConfiguration.aspx">SBU Configuration</a></li>
            <%--<li><a href="Configurations/DepartmentConfig.aspx">Department Configuration</a></li>--%>
            <li><a href="Masters/SubDepartmentMaster.aspx">Sub Department Master</a></li>
            <li><a href="Configurations/DefaultValueConfig.aspx">Default Values</a></li>
   <%--         <li><a href="Configurations/DepartmentSettings.aspx">Department Settings</a></li>--%>
           <%--             <li><a href="Configurations/UserGroupDepartment.aspx">User Group Department Mappings</a></li>--%>
<%--            <li><a href="Configurations/MenuGroupSettings.aspx">User Group Menu Mappings</a></li>--%>
            <%--<li><a href="Masters/RelatedPageMaster.aspx">Related Links</a></li>--%>
        </ul>
    </div>
    <div class="contentboxwrap">
        <h1>
            Masters</h1>
        <ul>
            <li><a href="Masters/UOMMaster.aspx">UOM Master</a></li>
            <li><a href="Masters/MaterialCategoryMaster.aspx">Material Category Master</a></li>
            <li><a href="Masters/MaterialMaster.aspx">Material Master</a></li>
            <li><a href="Masters/TaxMaster.aspx">PO Charges</a></li>
            <%--<li><a href="Masters/DesignationMaster.aspx">Designation Master</a></li>--%>
            <li><a href="Masters/TermsTemplate.aspx">General Template</a></li> 
            <li><a href="Masters/VendorTermMaster.aspx">Vendor Template</a></li>
           <%-- <li><a href="Masters/MachineryListing.aspx">Machinery Master</a></li>--%>       <%--Production Only --%>
           <%-- <li><a href="Masters/TankMaster.aspx">Tank Master</a></li> --%>                 <%--Production Only --%>
            <%--<li><a href="Masters/CompoundListing.aspx">Compound Master</a></li>--%>         <%--Production Only --%>
            <li><a href="Masters/CurrencyMaster.aspx">Currency Master</a></li>                
            <li><a href="Masters/StoreMaterialMapping.aspx">Store Material Mapping</a></li>
            <%--<li><a href="Masters/DispersionMaster.aspx">Dispersion Master</a></li>--%>              <%--Production Only --%>
            <li><a href="Masters/TaxMaster.aspx">Tax Master</a></li>
            <%--<li><a href="Masters/TaxSettingsFinal.aspx">Tax Setting Master</a></li>--%>
            <%-- <li><a href="../Production/TopUpRecordListing.aspx">Top-Up Record</a></li>--%>
        </ul>
    </div>
    <%--<div class="contentboxwrap">
        <h1>
            Reports</h1>
        <ul>
            <li><a href="Configurations/SBUConfiguration.aspx">Stock Report</a></li>
            <li><a href="Configurations/DepartmentConfig.aspx">Stock Detail Report</a></li>
            
        </ul>
    </div>--%>
</asp:Content>
