<%@ Page Title="<%$ Resources:Captions,Title_CompoundMaster %>" Theme="Classic" EnableEventValidation="false"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="CompoundMaster.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Masters.CompoundMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/Compounding/CompoundMaster1.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.CompoundingMaster%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                TabIndex="18" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" PostBackUrl="~/Administration/Masters/CompoundListing.aspx"
                TabIndex="19" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="~/Administration/Masters/CompoundListing.aspx"
                TabIndex="20" />
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
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" TabIndex="19" ID="btnReset" SkinID="btnInner-refresh"
                                    Text="<%$Resources:Controls,Reset%>" EnableViewState="False" PostBackUrl="~/Administration/Masters/CompoundListing.aspx" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" TabIndex="20" runat="server" SkinID="btnInner-Cancel"
                                    Text="<%$Resources:Controls,Cancel%>" EnableViewState="False" PostBackUrl="~/Administration/Masters/CompoundListing.aspx" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="SBU" runat="server" Value="0" />
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="COM_NAME">
                                        <%=Resources.Controls.CompoundName%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="COM_NAME" TabIndex="1">
                                    </asp:TextBox>
                                   
                                    <label for="COM_TYPE">
                                        <%=Resources.Controls.FormulationType%>
                                        *</label>
                                    <asp:DropDownList ID="COM_TYPE" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                   
                                    <label for="COM_QUANTITY">
                                        <%=Resources.Controls.UnitQuantity%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="COM_QUANTITY" Width="60px" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="5">
                                    </asp:TextBox>
                                    <asp:DropDownList ID="COM_QTY_UOM" runat="server" Width="70px" TabIndex="6"></asp:DropDownList>
                                    <asp:ImageButton ID="imbAddConversion" runat="server" ImageUrl="~/Images/ERP-Blue/Buttons/converter.png"
                                        CssClass="imgbtn" Width="16px" Height="16px" ToolTip="<%$Resources:Controls,AddConversion%>"
                                        TabIndex="7" OnClientClick="javascript:return AddConversion();" EnableViewState="false" />
                                    <%--<div class="clear">
                                    </div>--%>
                                    <label for="COM_EXP_PRD">
                                        <%=Resources.Controls.ExpiryPeriod%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="COM_EXP_PRD" Width="60px" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="10">
                                    </asp:TextBox>
                                    <asp:DropDownList ID="COM_EXP_UOM" runat="server" Width="70px" TabIndex="11">
                                    </asp:DropDownList>
                                    
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S" >
                                <div class="content">
                                    <label for="COM_CODE">
                                        <%=Resources.Controls.CompoundCode%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="COM_CODE" TabIndex="2">
                                    </asp:TextBox>
                                    <label for="COM_POLYMER">
                                        <%=Resources.Controls.Polymer%>
                                        *</label>
                                    <asp:DropDownList ID="COM_POLYMER" runat="server" TabIndex="4">
                                    </asp:DropDownList>
                                    <label for="COM_MAT_PRD">
                                        <%=Resources.Controls.MaturityPeriod%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="COM_MAT_PRD" Width="60px" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                        TabIndex="8">
                                    </asp:TextBox>
                                    <asp:DropDownList ID="COM_MAT_UOM" runat="server" Width="70px" TabIndex="9">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="grdTable">
                    <table rules="all" id="MaterialControls" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th width="15%" style="text-align: center">
                                    <%=Resources.Controls.MaterialType%>
                                    *
                                </th>
                                <th width="25%" style="text-align: center">
                                    <%=Resources.Controls.MaterialName%>*
                                </th>
                                <th width="25%" style="text-align: center">
                                    <%=Resources.Controls. WetQty %>
                                    *
                                </th>
                                <th width="10%" style="text-align: center">
                                    <%=Resources.Controls.DryPerc%>
                                    *
                                </th>
                                <th width="10%" style="text-align: center">
                                    <%=Resources.Controls.DryQty%>
                                </th>
                                <th width="15%" style="text-align: center">
                                    <%=Resources.Controls.CompoundPerc%>
                                </th>
                                <th width="5%" style="text-align: center">
                                    <%=Resources.Controls.Add%>
                                </th>
                            </tr>
                        </thead>
                        <tr class="grd-rowhead">
                            <td>
                                <asp:DropDownList ID="CPD_ITEM_CATEGORY" Width="90%" runat="server" onchange="javascript:FillItem();"
                                    TabIndex="12">
                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues, RawMaterial%>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues, Dispersion%>" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="CPD_ITEM" Width="90%" runat="server" onchange="javascript:GetMaterialUOMType();"
                                    TabIndex="13">
                                </asp:DropDownList>
                                <asp:HiddenField ID="MaterialUOMType" runat="server" Value="0" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="CPD_WET_QTY" MaxLength="8" CssClass="small-a numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onblur="javascript:CalculateCompound(event);"
                                    TabIndex="14">
                                </asp:TextBox>
                                <asp:DropDownList ID="CPD_QTY_UOM" Width="90px" runat="server" onchange="javascript:CalculateCompound();"
                                    TabIndex="15">
                                </asp:DropDownList>
                                <asp:HiddenField ID="ConversionValue" runat="server" Value="0" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="CPD_DRY_PERC" MaxLength="5" CssClass="small-a numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" onblur="javascript:CalculateCompound(event);"
                                    TabIndex="16">
                                </asp:TextBox>
                            </td>
                            <td>
                                <%-- <asp:TextBox ID="DryWeight" runat="server"></asp:TextBox>--%>
                                <asp:Label ID="DRYWEIGHT" runat="server" Text="0"></asp:Label>
                                <asp:HiddenField ID="CPD_DRY_QTY" runat="server" Value="0" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="CPD_COMP_PERC" MaxLength="8" CssClass="small-a numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                <asp:HiddenField ID="SL_NO" runat="server" Value="0" />
                                <asp:HiddenField ID="CPD_PK" runat="server" Value="0" />
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton runat="server" ID="btnAdd" SkinID="imbaddnew" OnClientClick="javascript:return AddMaterialDetails();"
                                    TabIndex="17" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdCompoundMaterial" grandtype="GrandGrid" pagesize="5" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
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
                                    <%=Resources.Controls.MaterialType%>*
                                </th>
                                <th fieldmap="MaterialName" align="left" width="25%">
                                    <%=Resources.Controls.MaterialName%>*
                                </th>
                                <th fieldmap="MATERIALQUANTITYUOM" align="right" width="25%">
                                    <%=Resources.Controls. WetQty %>
                                    *
                                </th>
                                <th fieldmap="CPD_DRY_PERC" align="right" width="10%">
                                    <%=Resources.Controls.DryPerc%>
                                </th>
                                <th fieldmap="DRYWEIGHT" align="right" width="10%">
                                    <%=Resources.Controls.DryQty%>*
                                </th>
                                <th fieldmap="CPD_COMP_PERC" align="right" width="15%">
                                    <%=Resources.Controls.CompoundPerc%>
                                </th>
                                <th type="Template" width="15%">
                                    <div style="text-align: center">
                                        <asp:ImageButton runat="server" ID="ImageButton5" SkinID="imbeditgrid" EnableViewState="false"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="ImageButton6" SkinID="imbdeletegrid" EnableViewState="false"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="divAddConversion" title="<%=Resources.Controls.ConversionDetails%>">
            <div class="div2col-S">
                <label for="UOMTypeFm">
                    <%=Resources.Controls.From%>
                </label>
                <span id="UOMTypeFm"></span>
                <div class="clear">
                </div>
                <label for="UPC_TO_UOM">
                    <%=Resources.Controls.To%>
                    *</label>
                <asp:DropDownList ID="UPC_TO_UOM" runat="server" Width="200px" TabIndex="12" EnableViewState="false">
                </asp:DropDownList>
                <div class="clear">
                </div>
                <label for="UPC_CONV_FACT">
                    <%=Resources.Controls.ConversionValue%>
                    *
                </label>
                <asp:TextBox runat="server" ID="UPC_CONV_FACT" Width="200px" TabIndex="13" MaxLength="10"
                    CssClass="numeric" EnableViewState="false" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                
                <div class="button-wrap-right">
                <asp:Button runat="server" ID="btnAddConv" Text="<%$ Resources:Controls, Save%>"
                    EnableViewState="false" SkinID="btnInner-Save" OnClientClick="javascript:return SaveConversionDtls();" />
                
                </div>
                
            </div>
            <asp:HiddenField ID="EditConversion" runat="server" Value="0" />
            <div class="grdTable">
                <div id="ConversionDiv">
                    <table rules="all" id="grdConversionDtls" grandtype="GrandGrid" pagesize="5" paging="true"
                        width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
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
    </div>
    <asp:HiddenField ID="COM_PK" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="CompoundMaterialsList" />
    <asp:HiddenField runat="server" ID="ConversionList" />
    <div id="divDatas">
    </div>
</asp:Content>
