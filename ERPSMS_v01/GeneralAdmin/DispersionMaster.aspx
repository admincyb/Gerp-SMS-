<%@ Page Title="<%$ Resources:Captions,Title_DispersionFormulation %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false" Theme="Classic"
    AutoEventWireup="true" CodeBehind="DispersionMaster.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.DispersionMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Administration/Masters/DispersionMaster.js.axd"
        type="text/javascript"></script>
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
                                <asp:Button ID="imbAdd" runat="server" SkinID="btnInner-New" ToolTip="<%$resources:Controls,Add %>"
                                    Text="<%$resources:Controls,Add %>" OnClientClick="javascript:return AddNew(); "
                                    TabIndex="18" />
                            </li>
                            <li>
                                <asp:Button ID="imbSave" runat="server" ToolTip="<%$resources:Controls,Save %>" Text="<%$Resources:Controls,Save %>"
                                    SkinID="btnInner-Save" OnClientClick="javascript:return SavePage();" TabIndex="19" />
                            </li>
                            <li>
                                <asp:Button ID="imdReset" runat="server" SkinID="btnInner-refresh" ToolTip="<%$resources:Controls,Refresh %>"
                                    Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ClearPage();"
                                    TabIndex="20" />
                            </li>
                            <li>
                                <asp:Button ID="imbCancel" runat="server" ToolTip="<%$resources:Controls,Cancel %>"
                                    Text="<%$Resources:Controls,Cancel %>" SkinID="btnInner-Cancel" OnClientClick="javascript:return ClearPage();"
                                    TabIndex="21" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div id="grdTable-wrap" class="content-wrapper">
        <div id="divData">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <div class="content">
                                <%--<span>
                        <%=Resources.Controls.DispersionName%></span>--%>
                                <label for="DSP_CODE">
                                    <%=Resources.Controls.DispersionCode%>
                                    *
                                </label>
                                <asp:TextBox ID="DSP_CODE" runat="server" MaxLength="30" TabIndex="1">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="DSP_EXP_TIME">
                                    <%=Resources.Controls.ExpiryTime%>
                                    *
                                </label>
                                <asp:TextBox ID="DSP_EXP_TIME" runat="server" Width="30%" MaxLength="3" TabIndex="3"
                                    CssClass="numeric">
                                </asp:TextBox>
                                <asp:DropDownList ID="DSP_EXP_TM_UOM" runat="server" Width="23%" TabIndex="4">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <%--<span>
                        <%=Resources.Controls.Quantity%></span>--%>
                                <label for="DSP_QUANTITY">
                                    <%=Resources.Controls.Quantity%>
                                    *
                                </label>
                                <asp:TextBox ID="DSP_QUANTITY" runat="server" CssClass="numeric" Width="30%" MaxLength="12"
                                    TabIndex="7" onblur="ClearMaterialDetails()" autocomplete="off" onChange="RecalculatePercentage()"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                                </asp:TextBox>
                                <asp:DropDownList ID="DSP_QTY_UOM" runat="server" Width="23%" onchange="ClearMaterialDetails()"
                                    TabIndex="8">
                                    <asp:ListItem Text="Ltrs" Value="1">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="DSP_TYPE">
                                    <%=Resources.Controls.DispersionType %>
                                </label>
                                <asp:DropDownList runat="server" ID="DSP_TYPE" TabIndex="11">
                                </asp:DropDownList>
                                 <div class="clear">
                                </div>
                                <div id="divPrefix">
                                <label for="DSP_PREFIX">
                                    <%=Resources.Controls.BatchNoPrefix %>
                                </label>
                                <asp:DropDownList runat="server" ID="DSP_PREFIX" TabIndex="13">
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>
        </td>
        <td>
            <div class="div2col-S" style="float: right; margin-right: 0">
                <div class="content">
                    <label for="DSP_NAME">
                        <%=Resources.Controls.DispersionName%>
                        *
                    </label>
                    <asp:TextBox runat="server" ID="DSP_NAME" MaxLength="100" TabIndex="2">
                    </asp:TextBox>
                    <div class="clear">
                    </div>
                    <label for="DSP_PREP_TIME">
                        <%=Resources.Controls.PreparationTime%>
                        *
                    </label>
                    <asp:TextBox ID="DSP_PREP_TIME" runat="server" Width="30%" MaxLength="3" TabIndex="5"
                        CssClass="numeric">
                    </asp:TextBox>
                    <asp:DropDownList ID="DSP_PREP_TM_UOM" runat="server" Width="23%" TabIndex="6">
                    </asp:DropDownList>
                    <div class="clear">
                    </div>
                    <div id="divMacType" runat="server" visible="false">
                    <label for="DSP_MACHINE_TYPE">
                        <%=Resources.Controls.MachineType%>
                        *
                    </label>
                    <asp:DropDownList runat="server" ID="DSP_MACHINE_TYPE" CssClass="drpdwnreduced" Width="51%"
                        TabIndex="9">
                        <asp:ListItem Text="<%$ Resources:BindValues, Tank%>" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:ImageButton runat="server" ID="machineAdd" SkinID="imbaddnew" CssClass="imgbtn"
                        TabIndex="10" Width="16px" Height="16px" OnClientClick="javascript:return AddMachineTypeDetails();" /><%--"javascript:return AddMachineType();"--%>
                    <div class="clear">
                    </div>
                    </div>
                      <label for="DSP_MACHINE">
                        <%=Resources.Controls.MachineName%>                        
                    </label>
                    <asp:DropDownList runat="server" ID="DSP_MACHINE" CssClass="drpdwnreduced" Width="51%"
                        TabIndex="9">                      
                    </asp:DropDownList>
                    <div class="clear">
                    </div>
                    <label for="DSP_CHECK_LIST_HDR">
                        <%=Resources.Controls.CheckListTemplate %>
                    </label>
                    <asp:DropDownList runat="server" ID="DSP_CHECK_LIST_HDR" CssClass="drpdwnreduced"
                        Width="50%" TabIndex="12">
                    </asp:DropDownList>
                    <asp:ImageButton runat="server" ID="templateAdd" SkinID="btnview" CssClass="imgbutton-wrap"
                        ToolTip="<%$resources:Controls,ShowDetails %>" TabIndex="13" OnClientClick="javascript:return ViewCheckListDetails(); return false;" />
                    <div class="clear">
                    </div>
                </div>
            </div>
        </td>
        </tr> </table>
        <%--Grid view --%>
        <div id="tabs-1">
            <div class="gridwrap">
                <div id="Order" class="gridwraptable gridwrap">
                    <table id="MaterialInsert">
                        <thead>
                            <tr>
                                <th width="20%" align="left">
                                    <%=Resources.Controls.MaterialCategory%>*
                                </th>
                                <th width="38%" align="left">
                                    <%=Resources.Controls.Item%>*
                                </th>
                                <%--<th width="20%" align="left">
                                        <%=Resources.Controls.MaterialName%>*
                                    </th>--%>
                                <th width="12%" style="text-align: right !important">
                                    <%=Resources.Controls.Quantity%>*
                                </th>
                                <th width="8%" align="left">
                                    <%=Resources.Controls.UOM%>*
                                </th>
                                <th width="17%" style="text-align: right !important">
                                    <%=Resources.Controls.Percentage%>
                                </th>
                                <th width="5%" align="left">
                                    <%=Resources.Controls.Action%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="20%" align="left">
                                    <%--<asp:DropDownList ID="MaterialCatagory" runat="server" TabIndex="13" Width="99%"
                                            onChange="javascript:FillCategoryDetails($(this).val());">
                                        </asp:DropDownList>--%>
                                    <asp:DropDownList ID="MaterialCatagory" Width="99%" runat="server" onchange="javascript:FillItem();"
                                        TabIndex="13">
                                        <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                        <%--<asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>--%>
                                        <asp:ListItem Text="<%$ Resources:BindValues, Dispersion%>" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:BindValues, RawMaterial%>" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="HiddenField2" runat="server"></asp:HiddenField>
                                </td>
                                <td width="38%" align="left">
                                    <asp:DropDownList ID="DSD_ITEM" TabIndex="14" runat="server" CssClass="right-M" Width="99%"
                                        onChange="return MaterialChangeEvent();">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                                </td>
                                <td style="display: none;">
                                    <asp:Label ID="MaterialName" runat="server" Text=""></asp:Label>
                                </td>
                                <td width="12%" style="text-align: right">
                                    <asp:TextBox ID="DSD_QUANTITY" runat="server" CssClass="numeric small-a" MaxLength="12"
                                        autocomplete="off" onkeyup="ValidateQuantity()" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="15">
                                    </asp:TextBox>
                                </td>
                                <td width="8%">
                                    <asp:DropDownList ID="DSD_QTY_UOM" runat="server" Width="80px" onChange="GetConversionFactor(null,null,ValidateQuantity)"
                                        TabIndex="16">
                                    </asp:DropDownList>
                                </td>
                                <td width="17%" style="text-align: right">
                                    <span id="tdPercentage"></span>
                                </td>
                                <td width="5%" align="left">
                                    <div style="text-align: center;">
                                        <asp:ImageButton runat="server" ID="ImageButton1" SkinID="imbaddnew" OnClientClick="javascript:return AddDispersionMaterials();"
                                            TabIndex="17" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="gridwrap">
                <table rules="all" id="grdDispersionDetails" grandtype="GrandGrid" paging="false"
                    width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="DSD_ITEM" isvisible="false" width="0%">
                            </th>
                            <th fieldmap="DSD_QTY_UOM" isvisible="false" width="0%">
                            </th>
                            <%--<th fieldmap="ITEM_CATAGORY" isvisible="false" width="0%">
                                </th>--%>
                            <th fieldmap="DSD_ITEM_TYPE" isvisible="false" width="0%">
                            </th>
                            <%--  <th fieldmap="DSD_QUANTITY" isvisible="false" width="0%">
                                </th>--%>
                            <th fieldmap="CONV_FACT" isvisible="false" width="0%">
                            </th>
                            <th fieldmap="DSD_ITEM_TYPE_TEXT" align="left" width="20%">
                                <%=Resources.Controls.MaterialCategory%>*
                            </th>
                            <th fieldmap="ITEM_CODE" align="left" isvisible="false">
                                <%=Resources.Controls.MaterialCode%>*
                            </th>
                            <th fieldmap="ITM_TEXT" align="left" width="42%">
                                <%=Resources.Controls.Item%>*
                            </th>
                            <%-- <th fieldmap="ITEM_NAME" align="left" isvisible="false">
                                    <%=Resources.Controls.MaterialName%>*
                                </th>--%>
                            <th fieldmap="DSD_QUANTITY" style="text-align: right !important" width="12%">
                                <%=Resources.Controls.Quantity%>*
                            </th>
                            <th fieldmap="UOM_CODE" width="8%">
                                <%=Resources.Controls.UOM%>*
                            </th>
                            <th fieldmap="DSD_QTY_PERC" style="text-align: right !important" width="13%">
                                <%=Resources.Controls.Percentage%>
                            </th>
                            <th type="Template" width="5%" align="left">
                                <div style="text-align: center;">
                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')"
                                        ToolTip="<%$resources:ErpRes,Edit %>" />
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')"
                                        ToolTip="<%$resources:ErpRes,Delete %>" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div id="divListing">
        <div id="searchwrap" class="search-wrap-custom1" style="padding-top: 8px !important;
            height: 25px !important;">
            <asp:Label runat="server" ID="Search_By" Text="Search By"></asp:Label>
            <asp:DropDownList ID="SearchType" runat="server">
               <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>--%>
               <asp:ListItem Value="DSP_CODE" Text="<%$ Resources:BindValues, DispersionCode%>"></asp:ListItem>
                <asp:ListItem Value="DSP_NAME" Text="<%$ Resources:BindValues, DispersionName%>"></asp:ListItem>                
                <%--<asp:ListItem Value="MCT_NAME" Text="<%$ Resources:BindValues, MachineType%>"></asp:ListItem>--%>
            </asp:DropDownList>
            <asp:TextBox ID="SearchValue" runat="server">
            </asp:TextBox>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" ToolTip="<%$ resources:ErpRes,Go %>" />
        </div>
        <div class="clear">
        </div>
       <div class="gridwrap">
            <table rules="all" id="grdDispersionList" grandtype="GrandGrid" ajaxurl="DispersionManagement.do?Action=GetDispersionList"
                pagesize="20" width="100%" paging="true" editfunction="GridAction" editable="true"
                class="gridwraptable gridwrap">
                <thead>
                    <tr>
                        <th fieldmap="DSP_PK" isvisible="false">
                        </th>
                        <th fieldmap="DSP_FLAG" isvisible="false">
                        </th>
                        <th fieldmap="DSP_CODE" sortable="true" align="left" width="15%">
                            <%--DSP_CODE--%>
                            <%=Resources.Controls.DispersionCode%>
                        </th>
                        <th fieldmap="DSP_NAME" sortable="true" align="left" width="20%">
                            <%=Resources.Controls.DispersionName%>
                        </th>
                         <th fieldmap="DSP_TYPE_TEXT" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.DispersionType%>
                        </th>
                        <th fieldmap="DSP_EXP_TIME" sortable="false" align="left" width="10%">
                            <%=Resources.Controls.ExpTime%>
                        </th>
                        <th fieldmap="DSP_QUANTITY" style="text-align: right!important;" width="13%">
                            <%=Resources.Controls.Quantity%>
                        </th>
                        <th fieldmap="DSP_PREP_TIME" sortable="false" align="left" width="7%">
                            <%=Resources.Controls.PrepTime%>
                        </th>
                        <th fieldmap="DSP_MACHINE_TEXT" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.MachineName%>
                        </th>
                        <th fieldmap="MACHINE_TYPE" sortable="true" align="left" width="15%" isvisible="false">
                            <%=Resources.Controls.MachineType%>
                        </th>
                         <th fieldmap="DSP_MACHINE" isvisible="false">                            
                        </th>
                         <th fieldmap="DSP_MACHINE_CODE" isvisible="false">                            
                        </th>
                        <th fieldmap="DSP_PREFIX" isvisible="false">                            
                        </th>
                        <th type="Template" width="10%">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="ImageButton2" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandlerMain($(this).parents('tr:eq(0)'),'Edit')"
                                    ToolTip="<%$resources:ErpRes,Edit %>" />
                                    <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandlerMain($(this).parents('tr:eq(0)'),'View')" 
                                ToolTip='<%$ Resources:Controls,View %>'/>
                                <asp:ImageButton runat="server" ID="ImageButton3" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandlerMain($(this).parents('tr:eq(0)'),'Delete')"
                                    ToolTip="<%$resources:ErpRes,Delete %>" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div class="clear">
    </div>
    <div id="divMachineType" style="margin-top: 25px; display: none" title="Create Machine Type">
        <div class="div2col-S">
            <label for="">
                <%=Resources.Controls.MachineType%>
            </label>
            <asp:TextBox runat="server" ID="MachineTypeName" MaxLength="190" Width="200" EnableViewState="false">
            </asp:TextBox><asp:HiddenField ID="MachineTypePK" runat="server" Value="0" />
            <asp:Button runat="server" ID="btnAdd" Text="<%$ Resources:Controls, Save%>" Width="50px"
                ToolTip="<%$resources:Controls,Save %>" EnableViewState="false" Height="20px"
                OnClientClick="Javascript:return SaveMachineType()" />
        </div>
        <div class="clear">
        </div>
        <%--Machine Type Details--%>
        <div class="grdTable">
            <table rules="all" id="grdMachineTypeDtls" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineTypeDetails"
                pagesize="5" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="MCT_PK" isvisible="false">
                        </th>
                        <th fieldmap="MCT_NAME" sortable="true" align="left" align="left" width="60%">
                            <%=Resources.Controls.MachineType%>
                        </th>
                        <th type="Template" width="20%">
                            <div style="text-align: center">
                                <asp:ImageButton runat="server" ID="imbMachineEdit" SkinID="imbeditgrid" OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'editmachinetype')"
                                    ToolTip="<%$resources:ErpRes,Edit %>" />
                                <asp:ImageButton runat="server" ID="imbMachineDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return MachineTypeGridHandler($(this).parents('tr:eq(0)'),'deletemachinetype')"
                                    ToolTip="<%$resources:ErpRes,Delete %>" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="divChecklist" class="gridwrap grid-w930" title="<%=Resources.Controls.CheckListDetails%>"
        style="display: none;">
        <table rules="all" id="grdChecklistDetails" grandtype="GrandGrid" paging="false"
            width="100%" class="gridwraptable gridwrap filter-arrow">
            <thead>
                <tr>
                    <th fieldmap="CDL_NAME" align="left" width="25%">
                        <%=Resources.Controls.ChecklistItem%>
                    </th>
                    <th fieldmap="CDL_VALUE" align="left" width="25%">
                        <%=Resources.Controls.CheckListValue%>
                    </th>
                    <th fieldmap="CDL_DESC" align="left" width="50%">
                        <%=Resources.Controls.CheckListRemarks%>
                    </th>
                </tr>
            </thead>
        </table>
    </div>
    <div id="divAddConversion" title="<%=Resources.Controls.ConversionDetails%>">
        <div class="divcol-M">
            <label for="UOMTypeFm" style="width: 70px">
                <%=Resources.Controls.From%></label>
            <span id="UOMTypeFm"></span>
            <div class="clear">
            </div>
            <label for="UMC_TO" style="width: 70px">
                <%=Resources.Controls.To%>
                *</label>
            <asp:DropDownList ID="UMC_TO" runat="server" Width="200px" EnableViewState="false">
                <asp:ListItem Value="0" Text="--select--"></asp:ListItem>
            </asp:DropDownList>
            <div class="clear">
            </div>
            <label for="UMC_CONV_FACT" style="width: 70px">
                <%=Resources.Controls.ConversionValue%>
                *
            </label>
            <asp:TextBox runat="server" ID="UMC_CONV_FACT" Width="200px" MaxLength="10" CssClass="numeric"
                EnableViewState="false"></asp:TextBox>
            <div class="clear">
            </div>
            <asp:Button runat="server" ID="btnAddConv" Text="<%$ Resources:Controls, Save%>"
                EnableViewState="false" class="inputbtn" Width="50px" Height="20px" OnClientClick="javascript:return SaveConversionDtls();" />
            <div class="clear">
            </div>
        </div>
        <div class="grdTable">
            <div id="ConversionDiv">
                <table rules="all" id="grdConversionDtls" grandtype="GrandGrid" pagesize="5" paging="true"
                    width="100%" editfunction="GridAction" editable="true">
                    <thead>
                        <tr>
                            <th fieldmap="UMC_PK" isvisible="false">
                            </th>
                            <th fieldmap="UPC_FROM_UOM" isvisible="false">
                            </th>
                            <th fieldmap="UPC_TO_UOM" isvisible="false">
                            </th>
                            <th fieldmap="UMC_UOM_TYPE" isvisible="false">
                            </th>
                            <th fieldmap="FROM_UOM_NAME" align="left" width="30%">
                                <%=Resources.Controls.From%>
                            </th>
                            <th fieldmap="TO_UOM_NAME" align="left" width="30%">
                                <%=Resources.Controls.To%>
                            </th>
                            <th fieldmap="UPC_CONV_FACT" align="right" width="20%">
                                <%=Resources.Controls.Value%>
                            </th>
                            <th type="Template" width="20%">
                                <div style="text-align: center">
                                    <asp:ImageButton runat="server" ID="ImageButton4" SkinID="imbeditgrid" EnableViewState="false"
                                        OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                    <asp:ImageButton runat="server" ID="ImageButton5" SkinID="imbdeletegrid" EnableViewState="false"
                                        OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    </div>
    <%--Machine Type --%>
    <asp:HiddenField runat="server" ID="DispersionDetailsList" />
    <asp:HiddenField runat="server" ID="ConversionList" />
    <asp:HiddenField runat="server" ID="EditProduct" Value="0" />
    <asp:HiddenField runat="server" ID="DSP_PK" Value="0" />
    <asp:HiddenField runat="server" ID="DSP_ACTIVE" Value="0" />
    <asp:HiddenField runat="server" ID="SBU" Value="1" />
    <asp:HiddenField runat="server" ID="hdfShowPrefix" Value="0" />
    <div id="divData1">
    </div>
</asp:Content>
