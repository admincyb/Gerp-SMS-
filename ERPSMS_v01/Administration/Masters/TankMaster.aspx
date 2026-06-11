<%@ Page Title="<%$ Resources:Captions,Title_TankMaster %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="TankMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.TankMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/TankMaster.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <asp:Button ID="imbAdd" runat="server" SkinID="btnInner-New" OnClientClick="javascript:return AddNew(true);"
                                    TabIndex="100" EnableViewState="false" Text="<%$Resources:Controls,Add%>" ToolTip="<%$Resources:Controls,Add%>" />
                            </li>
                            <li>
                                <asp:Button ID="imbSave" runat="server" SkinID="btnInner-Save" OnClientClick="javascript:return SavePage();"
                                    TabIndex="101" EnableViewState="false" Text="<%$Resources:Controls,Save%>" ToolTip="<%$Resources:Controls,Save%>" />
                            </li>
                            <li>
                                <asp:Button ID="imdReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,reset %>"
                                    ToolTip="<%$Resources:Controls,reset %>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="102" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="imbCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel %>"
                                    ToolTip="<%$Resources:Controls,Cancel %>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="103" />
                            </li>
                        </ul>
                        <asp:HiddenField runat="server" ID="TankMasterPk" Value="0" EnableViewState="false" />
                        <asp:HiddenField runat="server" ID="TankTypePk" Value="0" EnableViewState="false" />
                        <asp:HiddenField runat="server" ID="LocationPk" Value="0" EnableViewState="false" />
                        <asp:HiddenField runat="server" ID="LAST_MOD_DT" EnableViewState="false" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divListing" class="grdTable">
                <div id="searchwrap" class="search-wrap-custom">
                    <span>
                        <%=Resources.Controls.SearchBy%></span>
                    <asp:DropDownList runat="server" ID="SearchType" TabIndex="12" CssClass="srchboxtextbx"
                        onchange="javascript:SetSearchType();">
                        <%--                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                        </asp:ListItem>--%>
                        <asp:ListItem Value="TNK_NAME" Text="<%$ Resources:BindValues, TankName%>">
                        </asp:ListItem>
                        <asp:ListItem Value="LOC_NAME" Text="<%$ Resources:BindValues, Location%>">
                        </asp:ListItem>
                        <asp:ListItem Value="TNT_NAME" Text="<%$ Resources:BindValues, TankType%>">
                        </asp:ListItem>
                        <asp:ListItem Value="TNK_CODE" Text="<%$ Resources:BindValues, TankCode%>">
                        </asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="SearchValue" TabIndex="13"></asp:TextBox>
                    <asp:Label runat="server" ID="lblStatus" Text="Status" TabIndex="13"></asp:Label>
                    <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="2">
                        <asp:ListItem Value="-1">ALL</asp:ListItem>
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField runat="server" ID="hdfStatus" />
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" EnableTheming="true"
                        OnClientClick="javascript:return BindGrid();" EnableViewState="false" TabIndex="14" />
                    <asp:ImageButton ID="btnClearList" runat="server" Text="<%$ resources:Controls,Clear %>"
                        ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return ResetPage();"
                        TabIndex="15" EnableViewState="false" SkinID="clear-ext" CssClass="margntop1 margnlft-minus2 margnbotm0" />
                </div>
                <div class="gridwrap">
                    <table rules="all" id="grdTankMaster" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="TNK_PK" isvisible="false">
                                </th>
                                <th fieldmap="TNK_CODE" align="left" sortable="true" width="10%">
                                    <%=Resources.Controls.TankCode%>
                                </th>
                                <th fieldmap="TNK_NAME" align="left" sortable="true" width="20%">
                                    <%=Resources.Controls.TankName%>
                                </th>
                                <th fieldmap="TNK_HEIGHT" align="right" sortable="true" width="12%" style="text-align: right !important">
                                    <%=Resources.Controls.TankHeight%>
                                </th>
                                <th fieldmap="TNK_CAPACITY_PER_CM" align="right" sortable="true" width="10%">
                                    <%=Resources.Controls.CapacityperCm%>
                                </th>
                                <th fieldmap="TNK_SLOPE" align="right" sortable="true" width="9%">
                                    Slope
                                </th>
                                <th fieldmap="TNK_CAPACITY" sortable="true" width="10%" align="right">
                                    <%=Resources.Controls.TankCapacity%>
                                </th>
                                <th fieldmap="TNK_STOCK" sortable="true" width="6%" align="right">
                                    <%=Resources.Controls.Stock%>
                                </th>
                                <th fieldmap="TNK_ACTIVE_TEXT" width="6%" sortable="true">
                                    <%=Resources.Controls.Active%>
                                </th>
                                <%--Hidden Values--%>
                                <th fieldmap="TNK_TYPE" isvisible="false">
                                </th>
                                <th fieldmap="TNK_TYPE_TEXT" sortable="true" width="20%" align="left" isvisible="false">
                                    <%=Resources.Controls.TankType%>
                                </th>
                                <th fieldmap="TNK_LOCATION" isvisible="false">
                                </th>
                                <th fieldmap="TNK_LOCATION_TEXT" sortable="true" width="20%" align="left" isvisible="false">
                                    <%=Resources.Controls.Location%>
                                </th>
                                <th fieldmap="TNK_CAPACITY_UOM" isvisible="false">
                                </th>
                                <th fieldmap="TNK_CAPACITY_UOM_TEXT" sortable="true" width="15%" align="left" isvisible="false">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th fieldmap="TNK_REMARKS" sortable="true" width="15%" align="left" isvisible="false">
                                    <%=Resources.Controls.Remarks%>
                                </th>
                                <th fieldmap="TNK_GEN_CODE" sortable="true" width="15%" align="left" isvisible="false">
                                    <%=Resources.Controls.CompoundGenNo%>
                                </th>
                                <th fieldmap="TNK_LINE" sortable="true" isvisible="false">
                                    <%=Resources.Controls.Line%>
                                </th>
                                <th fieldmap="TNK_PLANT" isvisible="false">
                                </th>
                                <th fieldmap="LAST_MOD_DT" isvisible="false">
                                </th>
                                <th fieldmap="TNK_ACTIVE" width="6%" isvisible="false">
                                </th>
                                <th fieldmap="TNK_HAS_STOCK" width="6%" isvisible="false">
                                </th>
                                    <th fieldmap="TNK_SEQ"  isvisible="false">
                                     <%=Resources.Controls.Sequence%>
                                </th>
                                <%--Hidden Values--%>
                                <th type="Template" width="5%" align="center">
                                    <div style="text-align: right">
                                        <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" EnableViewState="False"
                                            ToolTip="<%$Resources:Controls,Edit %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                        <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" EnableViewState="False"
                                            ToolTip="<%$Resources:Controls,Delete %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <div>
                                        <label for="TankCode">
                                            <%=Resources.Controls.TankCode%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankCode" runat="server" TabIndex="1" MaxLength="100" CssClass="margnlft-minus4 middle-lbl-small-b2">
                                        </asp:TextBox>
                                        <label for="TankActive" class="lbl-34perc">
                                            <%=Resources.Controls.Active%></label>
                                        <asp:CheckBox ID="TankActive" runat="server" TabIndex="2" EnableViewState="False" />
                                        <%--<asp:CheckBox ID="TankActive" runat="server" TabIndex="19" Checked="true" />--%>
                                        <%--<asp:CheckBox ID="DSP_ACTIVE_STATUS" runat="server" Checked="true" TabIndex="13" />--%>
                                    </div>
                                    <div>
                                        <label for="TankType">
                                            <%=Resources.Controls.TankType%>
                                            *
                                        </label>
                                        <asp:DropDownList ID="TankType" runat="server" Width="230px" TabIndex="3" CssClass="select-half-a margnlft-minus4">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imbAddTankType" runat="server" SkinID="btnview" CssClass="srchbtn margnlft-minus4 margntop2"
                                            TabIndex="3" EnableViewState="false" ToolTip="<%$Resources:Controls,AddTankType%>"
                                            OnClientClick="javascript:return AddTankType();" />
                                    </div>
                                    <div>
                                        <label for="TankHeight">
                                            <%=Resources.Controls.TankHeight%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankHeight" runat="server" MaxLength="7" TabIndex="5" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                            onblur="javascript:return  CalculateTotalCapacity();" CssClass="middle-lbl-small-b2 margnlft-minus4">
                                        </asp:TextBox>
                                        <label for="CapacityperCm" class="lbl-18-5perc">
                                            <%=Resources.Controls.CapacityperCm%>
                                            *
                                        </label>
                                        <asp:TextBox ID="CapacityperCm" runat="server" TabIndex="6" MaxLength="7" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                            ClientIDMode="Static" onblur="javascript:return  CalculateTotalCapacity();" CssClass="lbl-17-3perc margnlft-minus4">
                                        </asp:TextBox>
                                    </div>
                                    <div>
                                        <label for="CompoundGenNo">
                                            <%=Resources.Controls.CompoundGenNo%>
                                        </label>
                                        <asp:TextBox ID="CompoundGenNo" runat="server" TabIndex="9" MaxLength="20" CssClass="margnlft-minus4 middle-lbl-small-b2">
                                        </asp:TextBox>
                                        <label for="Line" class="lbl-18-5perc">
                                            <%=Resources.Controls.Line%>
                                        </label>
                                        <asp:DropDownList ID="Line" runat="server" TabIndex="10" CssClass="input-small margnlft-minus4">
                                          </asp:DropDownList>
                                        <label for="TNK_PLANT" class="lbl-25-1perc">
                                            <%=Resources.Controls.Plant%>
                                        </label>
                                        <asp:DropDownList ID="TNK_PLANT" runat="server" TabIndex="10" CssClass="lbl-22-1perc margnlft-minus4">
                                        </asp:DropDownList>
                                        <label for="HasStock" class="lbl-18-5perc">  <%--TNK_HAS_STOCK--%>
                                            <%=Resources.Controls.Stock%>
                                        </label>
                                        <asp:CheckBox runat="server" ID="HasStock" TabIndex="11" CssClass="margnlft-minus4 style-none" />
                                    </div>
                                    <div>
                                          <label for="Sequence">
                                            <%=Resources.Controls.Sequence%>
                                        </label>
                                        <asp:TextBox ID="Sequence" runat="server" TabIndex="9" MaxLength="20" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" CssClass="margnlft-minus4 middle-lbl-small-b2">
                                        </asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <div>
                                        <label for="TankName">
                                            <%=Resources.Controls.TankName%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankName" runat="server" TabIndex="2" MaxLength="100" CssClass="input-half margnlft-minus4">
                                        </asp:TextBox>
                                    </div>
                                    <div>
                                        <label for="Location">
                                            <%=Resources.Controls.Location%>
                                            *
                                        </label>
                                        <asp:DropDownList ID="Location" runat="server" Width="230px" TabIndex="4" CssClass="select-half-a margnlft-minus4">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imbAddLocation" runat="server" SkinID="btnview" CssClass="srchbtn margnlft-minus4 margntop2"
                                            TabIndex="4" EnableViewState="false" ToolTip="<%$Resources:Controls,AddLocation%>"
                                            OnClientClick="javascript:return AddLocation();" />
                                    </div>
                                    <div>
                                        <label for="TankSlope">
                                            <%=Resources.Controls.Slope%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankSlope" runat="server" TabIndex="7" MaxLength="8" onblur="javascript:return  CalculateTotalCapacity();"
                                            onkeypress="javascript:AllowOnlyNumberswithMinus(event,true);" CssClass="lbl-17perc margnlft-minus4">
                                        </asp:TextBox>
                                        <label for="TankCapacity" class="lbl-22-6perc">
                                            <%=Resources.Controls.TankCapacityKg%>
                                            *
                                        </label>
                                        <asp:TextBox ID="TankCapacity" runat="server" TabIndex="8" ClientIDMode="Static"
                                            ReadOnly="true" CssClass="input-disabled lbl-17perc margnlft-minus4">
                                        </asp:TextBox>
                                        <%--<asp:DropDownList ID="UOMPk" runat="server" Width="100px" TabIndex="10" Enabled="true">
                                        </asp:DropDownList>--%>
                                        <asp:HiddenField ID="UOMPk" runat="server" Value="0"></asp:HiddenField>
                                    </div>
                                    <div>
                                        <label for="Remarks">
                                            <%=Resources.Controls.Remarks%>
                                        </label>
                                        <asp:TextBox ID="Remarks" runat="server" TabIndex="11" EnableTheming="false" TextMode="MultiLine"
                                            MaxLength="10" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                            Height="50px" CssClass="input-half margnlft-minus4">
                                        </asp:TextBox>
                                        <label for="Remarks" class="middle-lbl-xsmall-a1">
                                        </label>
                                        <label for="Remarks" class="lbl-35-2perc">
                                            <%=Resources.Controls.Max500Char%>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="ProcessMapping">
                    <div id="treewrap" class="edittree bg-none maxh-290">
                        <div id="trvProcessMap" class="treeview-adj tree-width">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divTankType" title="<%=Resources.Controls.TankTypeDet%>">
        <div class="div2col-S">
            <label for="TankTypeName">
                <%=Resources.Controls.TankType%>
                *
            </label>
            <asp:TextBox runat="server" ID="TankTypeName" TabIndex="15" MaxLength="50" EnableViewState="false"
                Width="150px">
            </asp:TextBox>
            <asp:Button runat="server" ID="btnAddTankType" Text="<%$ Resources:Controls, Save%>"
                EnableViewState="false" CssClass="inputbtn" TabIndex="16" Width="50px" Height="20px"
                OnClientClick="javascript:return SaveTankType();" />
        </div>
        <div class="clear">
        </div>
        <div class="grdTable">
            <table rules="all" id="grdTankType" grandtype="GrandGrid" pagesize="5" paging="true"
                editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="TNT_PK" isvisible="false">
                        </th>
                        <th fieldmap="TNT_NAME" sortable="true" align="left" width="80%">
                            <%=Resources.Controls.TankName%>
                        </th>
                        <th type="Template" width="20px">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbEditTankType" EnableViewState="false" SkinID="imbeditgrid"
                                    ToolTip="<%$Resources:Controls,Edit %>" OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'EditType')" />
                                <asp:ImageButton runat="server" ID="imbDelTankType" EnableViewState="false" SkinID="imbdeletegrid"
                                    ToolTip="<%$Resources:Controls,Delete %>" OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'DeleteType')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="divAddLocation" title="<%=Resources.Controls.LocationDetails%>">
        <div class="div2col-S">
            <label for="LocationName">
                <%=Resources.Controls.Location%>*
            </label>
            <asp:TextBox runat="server" ID="LocationName" TabIndex="15" MaxLength="50" Width="150px"
                EnableViewState="false">
            </asp:TextBox>
            <asp:Button runat="server" ID="btnAddLocation" Text="<%$ Resources:Controls, Save%>"
                EnableViewState="false" CssClass="inputbtn" TabIndex="16" Width="50px" Height="20px"
                OnClientClick="javascript:return SaveLocation();" />
        </div>
        <div class="clear">
        </div>
        <div class="grdTable">
            <table rules="all" id="grdLocation" grandtype="GrandGrid" pagesize="5" paging="true"
                editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="LOC_PK" isvisible="false">
                        </th>
                        <th fieldmap="LOC_NAME" sortable="true" align="left" width="80%">
                            <%=Resources.Controls.Location%>
                        </th>
                        <th type="Template" width="20px%">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbEditLocationType" EnableViewState="false"
                                    ToolTip="<%$Resources:Controls,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandlerLocation($(this).parents('tr:eq(0)'),'EditLocation')" />
                                <asp:ImageButton runat="server" ID="imbDelLocationType" EnableViewState="false" SkinID="imbdeletegrid"
                                    ToolTip="<%$Resources:Controls,Delete %>" OnClientClick="javascript:return GridHandlerLocation($(this).parents('tr:eq(0)'),'DeleteLocation')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="SBU" runat="server" Value="0" />
    <div id="divResult">
        <asp:HiddenField ID="TankDetails" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
