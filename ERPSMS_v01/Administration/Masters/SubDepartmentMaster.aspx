<%@ Page Title="<%$ Resources:Captions,Title_SubDepartmentMaster %>" Theme="ClassicExt"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="SubDepartmentMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.SubDepartmentMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/SubDepartmentManagement/SubDepartmentMaster.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.SubDepartmentMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew(1);"
                TabIndex="4" EnableViewState="false" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                TabIndex="18" EnableViewState="false" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                TabIndex="19" EnableViewState="false" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="19" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew(1);" TabIndex="21" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="22" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="23" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="13" />--%>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" TabIndex="24" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <label for="SearchType">
                            <%=Resources.Controls.SearchBy%></label>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                            onchange="javascript:SetSearchType();" EnableViewState="false">
                            <asp:ListItem Value="PARENT_NAME" Text="<%$ Resources:BindValues, BaseDepartment%>"> 
                            </asp:ListItem>
                            <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, Department%>">
                            </asp:ListItem>
                            <asp:ListItem Value="DPT_CODE" Text="<%$ Resources:BindValues, Code%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                        </asp:TextBox>
                        <label for="Active">
                            <%=Resources.Controls.Active%></label>
                        <asp:CheckBox ID="chkActiveFilter" runat="server" Checked="true" CssClass="style-none  margntop3" />
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="3" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="gridwrap">
                    <table rules="all" id="grdSubDepartment" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                <th fieldmap="DPT_PK" isvisible="false">
                                </th>
                                <th fieldmap="DPT_DEFAULT" isvisible="false">
                                </th>
                                <th fieldmap="BZU_NAME" sortable="true" align="left" width="20%">
                                    <%=Resources.Controls.SBU%>
                                </th>
                                <th fieldmap="DPT_PARENT_NAME" sortable="true" align="left" width="15%">
                                    Base Department
                                </th>
                                <th fieldmap="DPT_NAME" sortable="true" align="left" width="18%">
                                    <%=Resources.Controls.Department%>
                                </th>
                                <th fieldmap="DPT_CODE" sortable="true" align="left" width="14%">
                                    <%=Resources.Controls.Code%>
                                </th>
                                <th fieldmap="STT_NAME" sortable="true" align="left" width="10%">
                                    <%=Resources.Controls.State%>
                                </th>
                                <th fieldmap="CNT_NAME" sortable="true" align="left" width="10%">
                                    <%=Resources.Controls.Country%>
                                </th>
                                <th fieldmap="IS_ACTIVE" width="6%" sortable="true">
                                    <%=Resources.Controls.Active%>
                                </th>
                                <th type="Template" align="left" width="7%">
                                    <%--<div style="text-align: left">--%>
                                    <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                        EnableViewState="false" />
                                    <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                        EnableViewState="false" />
                                    <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                    <%-- </div>--%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divData">
                <div id="tabs-1">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="DPT_PARENT">
                                        <%=Resources.Controls.BaseDepartment%>*</label>
                                    <asp:DropDownList runat="server" ID="DPT_PARENT" TabIndex="5" EnableViewState="False"
                                        onChange="FillDepartmentCategoryList();" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <asp:ImageButton ID="imbViewDept" runat="server" SkinID="imbSelect" TabIndex="6"
                                        ToolTip="<% $Resources:AddDepartment %>" OnClientClick="javascript:return ShowDeaprtment();"
                                        EnableViewState="false" />
                                    <div class="clear">
                                    </div>
                                    <label for="DPT_CODE">
                                        <%=Resources.Controls.SubDepCode%></label>
                                    <asp:TextBox runat="server" ID="DPT_CODE" TabIndex="8" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_ADDR1">
                                        <%=Resources.Controls.AddressLine1%>*</label>
                                    <asp:TextBox runat="server" ID="DPT_ADDR1" TextMode="MultiLine" Height="50px" TabIndex="10" MaxLength="195" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <%--<asp:TextBox runat="server" ID="DPT_ADDR12" MaxLength="200" onkeydown="limitText(this,200);"
                                        onkeyup="limitText(this,200);" TabIndex="8" TextMode="Multiline" EnableViewState="true"
                                        CssClass="input-half multiline-2col" />--%>
                                    <label for="DPT_CNTRY">
                                        <%=Resources.Controls.Country%>
                                        *
                                    </label>
                                    <asp:DropDownList runat="server" ID="DPT_CNTRY" TabIndex="12" onChange="FillStateList();"
                                        EnableViewState="False" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <label for="DPT_CITY">
                                        <%=Resources.Controls.City%>
                                        *
                                    </label>
                                    <asp:TextBox runat="server" ID="DPT_CITY" TabIndex="14" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_PHONE">
                                        <%=Resources.Controls.Phone%>*</label>
                                    <asp:TextBox runat="server" ID="DPT_PHONE" TabIndex="16" MaxLength="90" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_EMAIL">
                                        <%=Resources.Controls.Email%>*</label>
                                    <asp:TextBox runat="server" ID="DPT_EMAIL" TabIndex="18" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_COMPANY">
                                        <%=Resources.Controls.Company%>*</label>
                                    <asp:DropDownList ID="DPT_COMPANY" runat="server" TabIndex="20" ClientIDMode="Static"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="DPT_TYPE">
                                        <%=Resources.Controls.Category%>
                                        *
                                    </label>
                                    <asp:DropDownList runat="server" ID="DPT_CATEGORY" TabIndex="7" EnableViewState="False"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <label for="DPT_NAME">
                                        <%=Resources.Controls.SubDepName%>
                                    </label>
                                    <asp:TextBox runat="server" ID="DPT_NAME" TabIndex="9" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_ADDR2">
                                        <%=Resources.Controls.AddressLine2%></label>
                                    <asp:TextBox runat="server" ID="DPT_ADDR2" TabIndex="11" TextMode="MultiLine" Height="50px" MaxLength="195" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_STATE">
                                        <%=Resources.Controls.State%>
                                        *
                                    </label>
                                    <asp:DropDownList runat="server" ID="DPT_STATE" TabIndex="13" EnableViewState="False"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <label for="DPT_ZIP">
                                        Zipcode
                                    </label>
                                    <asp:TextBox runat="server" ID="DPT_ZIP" TabIndex="15" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_MOBILE">
                                        Mobile</label>
                                    <asp:TextBox runat="server" ID="DPT_MOBILE" TabIndex="17" MaxLength="95" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_GST_NO">
                                        GST No.
                                    </label>
                                    <asp:TextBox runat="server" ID="DPT_GST_NO" TabIndex="19" MaxLength="90" EnableViewState="False"
                                        CssClass="input-half"></asp:TextBox>
                                    <label for="DPT_CURR">
                                        <%=Resources.Controls.Currency%>
                                        *
                                    </label>
                                    <asp:DropDownList runat="server" ID="DPT_CURR" TabIndex="21" EnableViewState="False"
                                        CssClass="select-small-a">
                                    </asp:DropDownList>
                                    <label for="DPT_ACTIVE" class="lbl-15-4perc">
                                        <%=Resources.Controls.Active%></label>
                                    <asp:CheckBox ID="DPT_ACTIVE" runat="server" TabIndex="21" EnableViewState="False" />
                                    <label for="DPT_IS_STOCK" class="middle-lbl">
                                        <%=Resources.Controls.Stock%></label>
                                    <asp:CheckBox ID="DPT_IS_STOCK" runat="server" TabIndex="21" EnableViewState="False" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div runat="server" id="divProjectAuto">
                        <label for="txtProject" class="middle-lbl-xsmall-c">
                            <%=Resources.Controls.Project%></label>
                        <asp:TextBox runat="server" ID="txtProject" TabIndex="11" EnableViewState="False"
                            CssClass="lbl-30perc"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="DPT_PROJECT" Value="-1" />
                    </div>
                    <asp:HiddenField ID="SBU" runat="server" Value="0" />
                    <asp:HiddenField ID="DPT_PK" runat="server" Value="0" />
                    <asp:HiddenField runat="server" ID="hdfShowProject" Value="<%$ Resources:ConfigurationsRes, ShowProject %>" />
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="divDeptPopUp" title="<%=Resources.Captions.ChooseDepartment%>" style="height: 400px">
            <div class="content-wrapper">
                <div id="divDepartment" title="<%=Resources.Captions.ChooseDepartment%>">
                    <div id="treewrap" class="edittree">
                        <div id="trvCategory" class="treeview-adj">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
