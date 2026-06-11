<%@ Page Title="<%$ Resources:Captions,Title_CompoundMaster %>" Theme="Classic" Language="C#"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="CompoundingMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.CompoundingMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/Compounding/CompoundMaster.js.axd"
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
                                <asp:Button ID="imbSave" runat="server" SkinID="btnInner-Save" OnClientClick="javascript:return SavePage();"
                                    ToolTip="<%$Resources:Controls, Save%>" Text="<%$Resources:Controls,Save%>" TabIndex="21" />
                            </li>
                            <li>
                                <asp:Button ID="imbCancel" runat="server" SkinID="btnInner-Cancel" OnClientClick="javascript:return CancelPage();"
                                    ToolTip="<%$Resources:Controls, Cancel%>" Text="<%$Resources:Controls,Cancel%>"
                                    TabIndex="22" EnableViewState="False" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <%--        <div id="divData">--%>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <div class="content">
                                <label for="COM_CODE">
                                    <%=Resources.Controls.CompoundCode%>
                                    *</label>
                                <asp:TextBox runat="server" ID="COM_CODE" TabIndex="1" MaxLength="95">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="COM_TYPE">
                                    <%=Resources.Controls.FormulationType%>
                                    *</label>
                                <asp:DropDownList ID="COM_TYPE" runat="server" TabIndex="3">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="COM_QUANTITY">
                                    <%=Resources.Controls.UnitQuantity%>
                                    *</label>
                                <asp:TextBox runat="server" ID="COM_QUANTITY" MaxLength="14" Width="142px" CssClass="numeric input" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                    TabIndex="5">
                                </asp:TextBox><%--onchange="javascript:CalcPercQuantity();"--%>
                                <asp:DropDownList ID="COM_QTY_UOM" runat="server" Width="100px" TabIndex="6" onchange="javascript:return UnitChange(false);">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdUnit" runat="server" Value="0" />
                                <asp:ImageButton ID="imbAddConversion" runat="server" ImageUrl="~/Images/ERP-Blue/Buttons/converter.png"
                                    CssClass="imgbtn" Width="16px" Height="16px" ToolTip="Add Conversion" TabIndex="7"
                                    OnClientClick="javascript:return AddConversion();" EnableViewState="false" />
                                <div class="clear">
                                </div>
                                <label for="COM_EXP_PRD">
                                    <%=Resources.Controls.ExpiryPeriod%>
                                    *</label>
                                <asp:TextBox runat="server" ID="COM_EXP_PRD" Width="142px" MaxLength="11" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                    TabIndex="10">
                                </asp:TextBox>
                                <asp:DropDownList ID="COM_EXP_UOM" runat="server" Width="100px" TabIndex="11">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S" style="float: right; margin-right: 0">
                            <div class="content">
                                <label for="COM_NAME">
                                    <%=Resources.Controls.CompoundName%>
                                    *</label>
                                <asp:TextBox runat="server" ID="COM_NAME" TabIndex="2" MaxLength="190">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="COM_POLYMER">
                                    <%=Resources.Controls.Polymer%>
                                    *</label>
                                <asp:DropDownList ID="COM_POLYMER" runat="server" TabIndex="4">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="COM_MAT_PRD">
                                    <%=Resources.Controls.MaturityPeriod%>
                                    *</label>
                                <asp:TextBox runat="server" ID="COM_MAT_PRD" Width="142px" MaxLength="11" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                    TabIndex="8">
                                </asp:TextBox>
                                <asp:DropDownList ID="COM_MAT_UOM" runat="server" Width="100px" TabIndex="9">
                                </asp:DropDownList>
                                <asp:HiddenField ID="ViewStatus" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                                <label for="COM_CHECK_LIST_HDR">
                                    <%=Resources.Controls.CheckListTemplate%>
                                </label>
                                <asp:DropDownList runat="server" ID="COM_CHECK_LIST_HDR" CssClass="drpdwnreduced"
                                    Width="50%" TabIndex="12">
                                </asp:DropDownList>
                                <asp:ImageButton runat="server" ID="templateAdd" SkinID="btnview" CssClass="imgbutton-wrap"
                                    TabIndex="13"  ToolTip="<%$resources:Controls,ShowDetails %>" OnClientClick="javascript:return ViewCheckListDetails(); return false;" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="grdTable">
                <table rules="all" id="MaterialControls" class="gridwraptable gridwrap filter-arrow">
                    <thead>
                        <tr>
                            <th width="15%" style="text-align: left">
                                <%=Resources.Controls.Category%>
                                *
                            </th>
                            <th width="32%" style="text-align: left">
                                <%=Resources.Controls.Item%>*
                            </th>
                            <th width="12%" style="text-align:right !important">
                                <%=Resources.Controls. WetQty %>
                                *
                            </th>
                            <th width="8%" align="left">
                                <%=Resources.Controls.UOM%>*
                            </th>
                            <th width="10%"  style="text-align:right !important">
                                <%=Resources.Controls.DryPerc%>
                                *
                            </th>
                            <th width="10%"  style="text-align:right !important">
                                <%=Resources.Controls.DryQty%>
                            </th>
                            <th width="8%"  style="text-align:right !important">
                                <%=Resources.Controls.Percentage%>
                            </th>
                            <th width="5%" style="text-align: center">
                                <%=Resources.Controls.Action%>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td  width="15%" align="left">
                            <asp:DropDownList ID="CPD_ITEM_CATEGORY" Width="99%" runat="server" onchange="javascript:FillItem();"
                                TabIndex="14">
                                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, Dispersion%>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, RawMaterial%>" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td  width="32%" align="left">
                            <asp:DropDownList ID="CPD_ITEM" Width="99%" runat="server" onchange="javascript:GetMaterialUOMType();"
                                TabIndex="15">
                            </asp:DropDownList>
                            <asp:HiddenField ID="MaterialUOMType" runat="server" Value="0" />
                        </td>
                        <td  width="12%" style="text-align: right">
                            <asp:TextBox runat="server" ID="CPD_WET_QTY" MaxLength="14"  onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                onblur="javascript:CalculateCompound(event);" TabIndex="16"  CssClass="numeric small-a">
                            </asp:TextBox>
                        </td>
                        <td width="8%" align="left">
                            <asp:DropDownList ID="CPD_QTY_UOM" Width="80px" runat="server" onchange="javascript:CalculateCompound();"
                                TabIndex="17">
                            </asp:DropDownList>
                            <asp:HiddenField ID="ConversionValue" runat="server" Value="0" />
                        </td>
                        <td width="10%"  style="text-align: right">
                            <asp:TextBox runat="server" ID="CPD_DRY_PERC" MaxLength="6" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                onblur="javascript:CalculateCompound(event);" TabIndex="18" CssClass="numeric small-a">
                            </asp:TextBox>
                        </td>
                        <td width="10%"  style="text-align:right">
                            <asp:TextBox runat="server" ID="DRYWEIGHT" Enabled="false" MaxLength="10" TabIndex="19"
                                CssClass="numeric small-a" Text="0">
                            </asp:TextBox>
                            <%--       onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                onblur="javascript:CalculateCompoundWithDryPerc(event);"--%>
                            <asp:HiddenField ID="CPD_DRY_QTY" runat="server" Value="0" />
                        </td>
                        <td width="8%"  style="text-align:right">
                            <asp:Label ID="CompoundPercentage" runat="server" CssClass="numeric"></asp:Label>
                            <asp:HiddenField ID="CPD_COMP_PERC" runat="server" Value="0" />
                            <asp:HiddenField ID="ConvertedWt" runat="server" Value="0" />
                            <asp:HiddenField ID="SL_NO" runat="server" Value="0" />
                            <asp:HiddenField ID="CPD_PK" runat="server" Value="0" />
                        </td>
                        <td width="5%" style="text-align: center">
                            <asp:ImageButton runat="server" ID="btnAdd" SkinID="imbaddnew" OnClientClick="javascript:return AddMaterialDetails();"
                                TabIndex="20" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="grdTable">
                <table rules="all" id="grdCompoundMaterial" grandtype="GrandGrid" paging="false"
                    editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                    <thead>
                        <tr>
                            <th fieldmap="SL_NO" isvisible="false">
                            </th>
                            <th fieldmap="CPD_PK" isvisible="false">
                            </th>
                            <th fieldmap="CPD_ITEM_CATEGORY" isvisible="false">
                            </th>
                            <th fieldmap="CPD_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="CPD_QTY_UOM" isvisible="false">
                            </th>
                            <th fieldmap="CPD_WET_QTY" isvisible="false">
                            </th>
                            <th fieldmap="ConvertedWt" isvisible="false">
                            </th>
                            <th fieldmap="CPD_DRY_QTY" isvisible="false">
                            </th>
                            <th fieldmap="CPD_COMP_PERC" isvisible="false">
                            </th>
                            <th fieldmap="ConversionValue" isvisible="false">
                            </th>
                            <th fieldmap="MATERIALTYPENAME" align="left" width="15%">
                                <%=Resources.Controls.Category%>*
                            </th>
                            <th fieldmap="MaterialName" align="left" width="32%">
                                <%=Resources.Controls.Item%>*
                            </th>
                            <th fieldmap="CPD_WET_MAT_QTY" align="right" width="12%">
                                <%=Resources.Controls. WetQty %>
                                *
                            </th>
                            <th fieldmap="QTY_UOM_CODE"  width="8%">
                             <%=Resources.Controls.UOM%>*
                            </th>
                            <th fieldmap="CPD_DRY_PERC" align="right" width="10%">
                                <%=Resources.Controls.DryPerc%>
                                *
                            </th>
                            <th fieldmap="DRYWEIGHT" align="right" width="10%">
                                <%=Resources.Controls.DryQty%>
                            </th>
                            <th fieldmap="CompoundPercentage" align="right" width="8%">
                               <%=Resources.Controls.Percentage%>
                            </th>
                            <th type="Template" width="5%">
                                <div style="text-align: center">
                                    <asp:ImageButton runat="server" ID="ImageButton5" SkinID="imbeditgrid" EnableViewState="false"
                                        ToolTip="<%$resources:ErpRes,Edit %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                    <asp:ImageButton runat="server" ID="ImageButton6" SkinID="imbdeletegrid" EnableViewState="false"
                                        ToolTip="<%$resources:ErpRes,Delete %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="divCmpdlist" class="gridwrap grid-w930 " title="<%=Resources.Controls.CheckListDetails%>"
                style="display: none;">
                <div class="content-wrapper">
                    <table rules="all" id="grdcompoundDetails" grandtype="GrandGrid" paging="false" width="100%"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="CDL_PK" isvisible="false">
                                </th>
                                <th fieldmap="CDL_SL_NO" isvisible="false">
                                </th>
                                <th fieldmap="CDL_ACTIVE" isvisible="false">
                                </th>
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
            </div>
        </div>
        <div id="divAddConversion" title="<%=Resources.Controls.ConversionDetails%>">
            <div class="divcol-M">
                <label for="UOMTypeFm" style="width: 70px">
                    <%=Resources.Controls.From%>
                    *
                </label>
                <asp:Label ID="UOMTypeFm" runat="server" Text=""></asp:Label>
                <div class="clear">
                </div>
                <label for="UPC_TO_UOM" style="width: 70px">
                    <%=Resources.Controls.To%>
                    *
                </label>
                <asp:DropDownList ID="UPC_TO_UOM" runat="server" Width="200px" TabIndex="12" EnableViewState="false">
                </asp:DropDownList>
                <div class="clear">
                </div>
                <label for="UPC_CONV_FACT" style="width: 70px">
                    <%=Resources.Controls.ConversionValue%>
                    *
                </label>
                <asp:TextBox runat="server" ID="UPC_CONV_FACT" Width="200px" TabIndex="13" MaxLength="10"
                    CssClass="numeric" EnableViewState="false" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                <asp:Button runat="server" ID="btnAddConv" Text="<%$ Resources:Controls, Save%>"
                    EnableViewState="false" class="inputbtn" Width="50px" Height="20px" OnClientClick="javascript:return SaveConversionDtls();" />
            </div>
            <div class="clear">
            </div>
            <div id="ConversionDiv">
                <asp:HiddenField ID="EditConversion" runat="server" Value="0" />
                <div class="grdTable">
                    <table rules="all" id="grdConversionDtls" grandtype="GrandGrid" pagesize="5" paging="true"
                        width="100%" editfunction="GridAction" editable="true">
                        <thead>
                            <tr>
                                <th fieldmap="UPC_PK" isvisible="false">
                                </th>
                                <th fieldmap="UPC_FROM_UOM" isvisible="false">
                                </th>
                                <th fieldmap="UPC_TO_UOM" isvisible="false">
                                </th>
                                <th fieldmap="UMC_UOM_TYPE" isvisible="false">
                                </th>
                                <th fieldmap="FROM_UOM_NAME" align="left" width="20%">
                                    <%=Resources.Controls.From%>
                                </th>
                                <th fieldmap="TO_UOM_NAME" align="left" width="20%">
                                    <%=Resources.Controls.To%>
                                </th>
                                <th fieldmap="UPC_CONV_FACT" align="right" width="20%">
                                    <%=Resources.Controls.Value%>
                                </th>
                                <th type="Template" width="20%">
                                    <div style="text-align: center">
                                        <asp:ImageButton runat="server" ID="ImageButton3" SkinID="imbeditgrid" EnableViewState="false"
                                            OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="ImageButton4" SkinID="imbdeletegrid" EnableViewState="false"
                                            OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
    <asp:HiddenField ID="COM_PK" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="CompoundMaterialsList" />
    <asp:HiddenField runat="server" ID="ConversionList" />
    <asp:HiddenField runat="server" ID="SBU" Value="0" />
    <div id="divDatas">
    </div>
</asp:Content>
