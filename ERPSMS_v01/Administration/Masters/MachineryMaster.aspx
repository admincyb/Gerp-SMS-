<%@ Page Title="<%$ Resources:Captions,Title_MachineryMaster %>" Language="C#" Theme="Classic"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="MachineryMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.MachineryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/MachineryManagement/MachineryMaster.js.axd"
        type="text/javascript"></script>
    <script src="../../Scripts/jquery/UI/timepicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MachineryMaster%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbSave" runat="server" TabIndex="28" EnableViewState="false"
                SkinID="btnsave" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" TabIndex="29" EnableViewState="false"
                SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" TabIndex="30" PostBackUrl="~/Administration/Masters/MachineryListing.aspx"/>
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
                                <asp:Button runat="server" TabIndex="18" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" ToolTip="<%$Resources:Controls,Save%>" />
                            </li>
                            <li>
                                <asp:Button runat="server" TabIndex="19" ID="btnReset" SkinID="btnInner-refresh"
                                    ToolTip="<%$Resources:Controls,Reset%>" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="SBU" runat="server" Value="0" />
        </div>
    </div>
    <div class="content-wrapper">
        <%--Sruthy--%>
        <div id="grdTable-wrap">
            <%-- <div id="divData">--%>
            <div id="divFileData">
            </div>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <%-- <div class="content">--%>
                            <div class="div2col-S">
                                <label for="MCH_CODE" style="min-width: 27.7%!important;">
                                    <%=Resources.Controls.MachineCode%>
                                    *
                                </label>
                                <asp:TextBox ID="MCH_CODE" runat="server" MaxLength="95" EnableViewState="false"
                                    TabIndex="1" CssClass="lbl-29perc">
                                </asp:TextBox>
                            </div>
                            <div class="div2col-S">
                                <label for="MCH_NAME" style="min-width: 27.7%!important;">
                                    <%=Resources.Controls.MachineName%>
                                    *
                                </label>
                                <asp:TextBox ID="MCH_NAME" runat="server" MaxLength="190" EnableViewState="false"
                                    TabIndex="4" CssClass="input-half">
                                </asp:TextBox>
                                <%-- <label for="MachineActive">
                                        <%=Resources.Controls.Active%></label>
                                    <asp:CheckBox ID="MachineActive" runat="server" TabIndex="2"  />
                                    <asp:HiddenField runat="server" ID="MCH_ACTIVE"/>--%>
                            </div>
                            <div runat="server" id="divPlant" class="div2col-S">
                                <label for="MCH_PLANT" style="min-width: 27.7%!important;">
                                    <%=Resources.Controls.Plant%>
                                </label>
                                <asp:DropDownList runat="server" ID="MCH_PLANT" Width="150px" TabIndex="7" CssClass="input-medium">
                                </asp:DropDownList>
                            </div>
                            <%-- </div>--%>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <%-- <div class="content"> sruthy--%>
                            <div class="div2col-S">
                                <label for="MCH_TYPE">
                                    <%=Resources.Controls.MachineType%>
                                    *
                                </label>
                                <asp:DropDownList ID="MCH_TYPE" runat="server" EnableViewState="false" Width="150px"
                                    TabIndex="2" CssClass="input-medium">
                                </asp:DropDownList>
                                <asp:ImageButton ID="imbAddMachineType" runat="server" EnableViewState="false" SkinID="btnview"
                                    CssClass="srchbtn margntop4" Text="Add" ToolTip="Add Machine Type" TabIndex="3"
                                    OnClientClick="javascript:return AddMachineTypeDetails();" />
                            </div>
                            <div class="div2col-S">
                                <label for="MCH_LOCATION">
                                    <%=Resources.Controls.LocationWorkCenter%>
                                    *
                                </label>
                                <asp:DropDownList ID="MCH_LOCATION" runat="server" Width="150px" TabIndex="5" EnableViewState="false"
                                    CssClass="input-medium">
                                </asp:DropDownList>
                                <asp:ImageButton ID="imbAddLocation" runat="server" EnableViewState="false" SkinID="btnview"
                                    CssClass="srchbtn margntop4" Text="Add" ToolTip="<%$Resources:Controls,AddLocation %>"
                                    TabIndex="6" OnClientClick="javascript:return AddLocationDetails();" />
                            </div>
                            <div class="div2col-S">
                                <label for="MachineActive">
                                    <%=Resources.Controls.Active%></label>
                                <asp:CheckBox ID="MachineActive" runat="server" TabIndex="7" Checked="true" />
                                <asp:HiddenField runat="server" ID="MCH_ACTIVE" />
                            </div>
                            <%-- </div> sruthy--%>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
            <div id="tabs" runat="server">
                <ul>
                    <li><a href="#Purchase">
                        <%=Resources.Controls.PurchaseInfo%>
                    </a></li>
                    <li><a href="#Maintanace">
                        <%=Resources.Controls.MaintenanceInfo%></a></li>
                    <li><a href="#Performance">
                        <%=Resources.Controls.PerformanceInfo%></a></li>
                    <li><a href="#ProcessMapping">
                        <%=Resources.Controls.ProcessMapping%></a></li>
                    <li id="liMeasuringType" runat="server"><a href="#MeasuringType">
                        <%=Resources.Controls.MeasuringType%></a></li>
                </ul>
                <div id="Purchase" class="padglft0" style="width: 99%">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <%--div begin--%>
                                    <div class="div2col-S">
                                        <label for="MCH_VENDOR">
                                            <%=Resources.Controls.PurchasedFrom%>
                                            *
                                        </label>
                                        <asp:DropDownList ID="MCH_VENDOR" runat="server" TabIndex="7" EnableViewState="false"
                                            CssClass="lbl-62perc">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="div2col-S">
                                        <label for="MCH_PUR_PRICE">
                                            <%=Resources.Controls.PurchasePrice%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="MCH_PUR_PRICE" EnableViewState="false" Width="150px"
                                            CssClass="numeric" MaxLength="12" TabIndex="11">
                                        </asp:TextBox>
                                        <asp:DropDownList runat="server" ID="MCH_PUR_CURR" EnableViewState="false" Width="100px"
                                            TabIndex="12">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <%--div end--%>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <%--div begin--%>
                                    <div class="div2col-S">
                                        <label for="MCH_CONDITION" style="min-width: 23.8%!important;">
                                            <%=Resources.Controls.Condition%>
                                            *
                                        </label>
                                        <asp:DropDownList runat="server" ID="MCH_CONDITION" EnableViewState="false" TabIndex="8"
                                            CssClass="select-small-a1">
                                            <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="<%$ Resources:BindValues, New%>"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="<%$ Resources:BindValues, Used%>"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label for="MCH_PUR_DT" style="margin-left: -1px!important;">
                                            <%=Resources.Controls.DateofPurchase%>
                                            *
                                        </label>
                                        <asp:TextBox runat="server" ID="MCH_PUR_DT" EnableViewState="false" TabIndex="9"
                                            ReadOnly="true" CssClass="input-small">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdnDateofPurchase" runat="server" />
                                    </div>
                                    <div class="div2col-S">
                                        <label for="MCH_EXPR_DT" style="min-width: 23.8%!important;">
                                            <%=Resources.Controls.Expiry%>
                                            *
                                        </label>
                                        <asp:TextBox runat="server" ID="MCH_EXPR_DT" EnableViewState="false" TabIndex="10"
                                            ReadOnly="true" CssClass="input-small">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdnExpiryDate" runat="server" />
                                    </div>
                                </div>
                                <%--div end--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <label for="MCH_PUR_REMARKS" style="margin-left: -4px!important;">
                                        <%=Resources.Controls.Remarks%>
                                    </label>
                                    <asp:TextBox runat="server" ID="MCH_PUR_REMARKS" TextMode="MultiLine" EnableTheming="false"
                                        EnableViewState="false" MaxLength="450" TabIndex="13" Style="min-width: 79.9%!important;">
                                    </asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear"></div>
                    <div class="divcol-FileuplWrap w100perc" id="divAttachment">
                        <label for="aupDocument" class="lbl-12-4perc">
                            <%=Resources.Controls.Upload%></label>
                        <div id="FileUploader" class="input-file">
                            <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="24" Height="22px"
                                TabIndex="14" />
                            <asp:HiddenField ID="FILELIST" runat="server" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div id="Maintanace">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="MCM_TYPE">
                                        <%=Resources.Controls.TypeOfMaintenance%>
                                    </label>
                                    <asp:DropDownList ID="MCM_TYPE" runat="server" TabIndex="15" EnableViewState="false"
                                        CssClass="input-medium">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="MCM_FREQUENCY">
                                        <%=Resources.Controls.Freequency%>
                                    </label>
                                    <asp:DropDownList ID="MCM_FREQUENCY" runat="server" TabIndex="16" onChange="ShowMaintenaceDuration();"
                                        EnableViewState="false" CssClass="input-medium">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <div id="maintenceFrom">
                                        <label for="MCM_FROM_DT">
                                            <%=Resources.Controls.FromDate%>
                                        </label>
                                        <asp:TextBox ID="MCM_FROM_DT" runat="server" TabIndex="17" EnableViewState="false"
                                            ReadOnly="true" CssClass="input-small">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdnMaintenanceFromDate" runat="server" />
                                    </div>
                                    <div id="maintenceTo">
                                        <label for="MCM_TO_DT">
                                            <%=Resources.Controls.ToDate%>
                                        </label>
                                        <asp:TextBox ID="MCM_TO_DT" runat="server" TabIndex="18" EnableViewState="false"
                                            ReadOnly="true" CssClass="input-small">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="hdnMaintenanceToDate" runat="server" />
                                    </div>
                                    <div id="maintenceDay">
                                        <label for="FromTime">
                                            <%=Resources.Controls.FromTime%>
                                        </label>
                                        <asp:TextBox ID="FromTime" runat="server" TabIndex="19" EnableViewState="false" CssClass="input-small">
                                        </asp:TextBox>
                                        <div class="clear"></div>
                                        <label for="ToTime">
                                            <%=Resources.Controls.ToTime%>
                                        </label>
                                        <asp:TextBox ID="ToTime" runat="server" TabIndex="20" EnableViewState="false" CssClass="input-small">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="MCM_PK" runat="server" />
                                    </div>
                                    <div class="button-wrap-right">
                                        <asp:Button runat="server" ID="btnAddMaintenance" Text="<%$ resources:Add%>" TabIndex="21"
                                            ToolTip="<%$ resources:Add%>" SkinID="btnInner-New" OnClientClick="javascript:return AddMaintenanceDtls();"
                                            EnableViewState="false" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="grdTable">
                        <table rules="all" id="grdMaintenanceDetails" grandtype="GrandGrid" paging="false"
                            editfunction="GridAction" ajaxurl="" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="SL_NO" isvisible="false"></th>
                                    <th fieldmap="MCM_PK" isvisible="false"></th>
                                    <th fieldmap="MCM_TYPE" isvisible="false"></th>
                                    <th fieldmap="MCM_FREQUENCY" isvisible="false"></th>
                                    <th fieldmap="MNT_NAME" align="left" width="25%">
                                        <%=Resources.Controls.TypeOfMaintenance%>
                                    </th>
                                    <th fieldmap="FRQ_NAME" align="left" width="25%">
                                        <%=Resources.Controls.Freequency%>
                                    </th>
                                    <th fieldmap="MCM_FROM_DT" align="left" width="20%">
                                        <%=Resources.Controls.FromDateTime%>
                                    </th>
                                    <th fieldmap="MCM_TO_DT" align="left" width="20%">
                                        <%=Resources.Controls.ToDateTime%>
                                    </th>
                                    <th type="Template" width="5%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbMaintnceEdit" SkinID="imbeditgrid" Width="16px"
                                                ToolTip="<%$ Resources:Controls, Edit%>" EnableViewState="false" Height="16px"
                                                OnClientClick="javascript:return MainteanceGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                            <asp:ImageButton runat="server" ID="imbMaintnceDelete" SkinID="imbdeletegrid" Width="16px"
                                                ToolTip="<%$ Resources:Controls, Delete%>" EnableViewState="false" Height="16px"
                                                OnClientClick="javascript:return MainteanceGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <div id="Performance">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="MCH_THROUGHPUT" class="lbl-25-5perc">
                                        <%=Resources.Controls.Throughput%>
                                    </label>
                                    <asp:TextBox ID="MCH_THROUGHPUT" runat="server" TabIndex="22" EnableViewState="false"
                                        CssClass="numeric" MaxLength="12">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="MCH_AVG_CONS" class="lbl-25-5perc">
                                        <%=Resources.Controls.AvgConsumption%>
                                    </label>
                                    <asp:TextBox ID="MCH_AVG_CONS" runat="server" Width="138px" TabIndex="24" EnableViewState="false"
                                        CssClass="numeric" MaxLength="12">
                                    </asp:TextBox>
                                    <asp:DropDownList ID="MCH_AVGC_UOM" runat="server" Width="125px" TabIndex="25" EnableViewState="false">
                                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="MCH_FUEL_TYPE" class="lbl-25-8perc">
                                        <%=Resources.Controls.RunBy%>
                                    </label>
                                    <asp:DropDownList ID="MCH_FUEL_TYPE" runat="server" TabIndex="23" EnableViewState="false" Width="180px">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="MCH_MC_USAGE" class="lbl-25-8perc">
                                        <%=Resources.Controls.MaxContinuousUsage%>
                                    </label>
                                    <asp:TextBox ID="MCH_MC_USAGE" runat="server" Width="125px" TabIndex="26" MaxLength="11"
                                        EnableViewState="false" CssClass="numeric">
                                    </asp:TextBox>
                                    <asp:DropDownList ID="MCH_MCU_UOM" runat="server" Width="125px" TabIndex="27" EnableViewState="false">
                                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                        </asp:ListItem>
                                        <asp:ListItem Value="1" Text="<%$ Resources:BindValues, Hour%>"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>
                <div id="ProcessMapping">
                    <div id="treewrap" class="edittree" style="overflow: auto">
                        <div id="trvProcessMap" class="treeview-adj tree-width">
                        </div>
                    </div>
                </div>
                <div id="MeasuringType">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="MCH_MEASURE">
                                            <%=Resources.Controls.MeasuringType%>
                                    </label>
                                    <asp:DropDownList ID="MCH_MEASURE" runat="server" CssClass="lbl-62perc"></asp:DropDownList>
                                </div>
                            </td>
                            <td><div class="div2col-S"></div></td>
                        </tr>
                    </table>
                </div>
                <div class="clear">
                </div>
            </div>
            <%--</div>--%>
            <div class="clear">
            </div>
        </div>
    </div>
    <%--Sruthy--%>
    <div id="divMachineType" style="margin-top: 25px">
        <div style="width: 120px; height: 20px; float: left">
            <label for="">
                <%=Resources.Controls.MachineType%>
                *
            </label>
        </div>
        <div style="float: left;">
            <asp:TextBox runat="server" ID="MachineTypeName" MaxLength="190" EnableViewState="false">
            </asp:TextBox><asp:HiddenField ID="MachineTypePK" runat="server" Value="0" />
        </div>
        <div style="float: left; margin-left: 2px">
            <asp:Button runat="server" ID="btnAdd" Text="<%$ Resources:Controls, Save%>" Width="50px"
                ToolTip="<%$ Resources:Controls, Save%>" EnableViewState="false" Height="20px"
                OnClientClick="Javascript:return SaveMachineType()" />
        </div>
        <div class="clear">
        </div>
        <%--Machine Type Details--%>
        <div class="grdTable">
            <table rules="all" id="grdMachineTypeDtls" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineTypeDetails"
                pagesize="5" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="MCT_PK" isvisible="false"></th>
                        <th fieldmap="MCT_NAME" sortable="true" align="left" align="left" width="75%">
                            <%=Resources.Controls.MachineType%>
                        </th>
                        <th type="Template" width="25%">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbMachineEdit" SkinID="imbeditgrid" ToolTip="<%$ Resources:Controls, Edit%>"
                                    OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'editmachinetype')" />
                                <asp:ImageButton runat="server" ID="imbMachineDelete" SkinID="imbdeletegrid" ToolTip="<%$ Resources:Controls, Delete%>"
                                    OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'deletemachinetype')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="LocationDiv" style="width: 300px; margin-top: 25px">
        <div style="width: 25%; height: 20px; float: left">
            <label for="LocationName">
                <%=Resources.Controls.LocationWorkCenter%>
                *
            </label>
        </div>
        <div style="float: left;">
            <asp:TextBox runat="server" ID="LocationName" MaxLength="190" EnableViewState="false">
            </asp:TextBox>
            <asp:HiddenField ID="LocationPK" runat="server" Value="0" />
        </div>
        <div style="float: left; margin-left: 2px">
            <asp:Button runat="server" ID="AddLocation" Text="<%$ Resources:Controls, Save%>"
                ToolTip="<%$ Resources:Controls, Save%>" EnableViewState="false" EnableTheming="false"
                CssClass="inputbtn" Width="50px" Height="20px" OnClientClick="Javascript:return  SaveLocation();" />
        </div>
        <div class="clear">
        </div>
        <%--Location Details--%>
        <div class="grdTable">
            <table rules="all" id="grdLocationDtls" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetLocationList"
                pagesize="5" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="LOC_PK" isvisible="false"></th>
                        <th fieldmap="LOC_NAME" sortable="true" align="left" width="75%">
                            <%=Resources.Controls.LocationWorkCenter%>
                        </th>
                        <th type="Template" align="center" width="25%">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbLocEdit" SkinID="imbeditgrid" ToolTip="<%$Resources:Controls,Edit %>"
                                    OnClientClick="javascript:return LocationGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                <asp:ImageButton runat="server" ID="imbLocDel" SkinID="imbdeletegrid" ToolTip="<%$Resources:Controls,Delete %>"
                                    OnClientClick="javascript:return LocationGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="MaintenanceList" />
    <asp:HiddenField runat="server" ID="EditMaintenance" Value="0" />
    <asp:HiddenField runat="server" ID="MCH_PK" Value="0" />
    <asp:HiddenField runat="server" ID="USER_PK" Value="0" />
    <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
    <div id="divDatas">
    </div>
    <div id="divResult">
        <asp:HiddenField ID="MachineDetails" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
