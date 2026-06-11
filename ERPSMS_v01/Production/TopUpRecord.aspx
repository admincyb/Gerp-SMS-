<%@ Page Title="<%$ Resources:Captions,Title_TopUpRecord %>" Language="C#" Theme="ERP-Blue"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS.Master" AutoEventWireup="true"
    CodeBehind="TopUpRecord.aspx.cs" Inherits="ERPSMS_v01.Production.TopUpRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/TopUpRecordEntry.js.axd" type="text/javascript"></script>
    <script src="../Scripts/jquery/UI/timepicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.TopUpRecord%></h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" TabIndex="13" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" TabIndex="14" OnClientClick="javascript:return CancelPage();"  />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" TabIndex="15" OnClientClick="javascript:return CancelPage();"  />
        </div>
    </div>
    <div id="grdTable-wrap">
        <div id="divData">
            <div class="div2col-S">
                <label for="">
                    <%=Resources.Controls.Shift%>
                    *</label>
                <asp:TextBox ID="TUH_SHIFT_NAME" runat="server" Width="50%" TabIndex="1"></asp:TextBox>
                <asp:HiddenField ID="TUH_SHIFT" runat="server" Value="0"></asp:HiddenField>
                <label for="">
                    <%=Resources.Controls.Plan%>
                    *</label>
                <asp:TextBox ID="TUH_PLAN_NAME" runat="server" Width="50%" TabIndex="3"></asp:TextBox>
                <asp:HiddenField ID="TUH_PLAN" runat="server" Value="0"></asp:HiddenField>
            </div>
            <div class="div2col-S" style="float: right; margin-right: 0px">
                <label for="">
                    <%=Resources.Controls.Date%>
                    *</label>
                <asp:TextBox ID="TUH_DATE" runat="server" TabIndex="2"></asp:TextBox>
                <label for="">
                    <%=Resources.Controls.Time%>
                    *</label>
                <asp:TextBox ID="TUH_TIME" runat="server" TabIndex="4"></asp:TextBox>
            </div>
            <div class="clear">
            </div>
            <div class="grdTable">
                <table id="MaterialControls" cellpadding="0" cellspacing="0" border="1" style="border-collapse: collapse;
                    width: 100%; border: solid 1px #d0d0d0;">
                    <tr>
                        <th width="15%">
                            <%=Resources.Controls.TankType%>
                            *
                        </th>
                        <th width="15%">
                            <%=Resources.Controls.TankNames%>
                            *
                        </th>
                        <th width="15%">
                            <%=Resources.Controls.Type%>
                            *
                        </th>
                        <th width="15%">
                            <%=Resources.Controls.Item%>
                            *
                        </th>
                        <th width="15%">
                            <%=Resources.Controls.BatchNo%>
                            *
                        </th>
                        <th width="20%">
                            <%=Resources.Controls.Quantity%>
                            *
                        </th>
                        <th width="5%">
                            <%=Resources.Controls.Add%>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="TUD_TANK_TYPE" runat="server" CssClass="right-M" Width="90%"
                                TabIndex="5" onchange="javascript:FillTank();">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="TUD_TANK" runat="server" CssClass="right-M" Width="90%" TabIndex="6"
                                onchange="javascript:FillMaterialDetailsByTank();">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="TUD_ITEM_TYPE" runat="server" CssClass="right-M" Width="90%"
                                TabIndex="7" onchange="javascript:ShowItemsControls($(this).val());">
                                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, RawMaterial%>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, Compound%>" Value="3"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:BindValues, Dispersion%>" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="TUD_ITEM" runat="server" CssClass="right-M" Width="90%" TabIndex="8"
                                onchange="javascript:FillBatchNo();">
                            </asp:DropDownList>
                            <asp:HiddenField ID="ITEM_UOM" runat="server" Value="0" />
                        </td>
                        <td>
                            <asp:DropDownList ID="TUD_BATCH" runat="server" CssClass="right-M" Width="90%" TabIndex="9">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TUD_QUANTITY" runat="server"  Width="45%" TabIndex="10" CssClass="numeric"
                                onkeypress="javascript:MakeNumeric(event);">
                            </asp:TextBox>
                            <asp:DropDownList ID="TUD_QTY_UOM" runat="server" CssClass="right-M" Width="45%"
                                TabIndex="11">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="imbAdd1" SkinID="imbaddnew" TabIndex="12" Width="16px"
                                Height="16px" OnClientClick="javascript:return AddMaterialDetails();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
            <div class="grdTable">
                <table rules="all" id="grdTopUpMaterial" grandtype="GrandGrid" pagesize="5" paging="true"
                    width="100%" editfunction="GridAction" editable="true">
                    <thead>
                        <tr>
                            <th fieldmap="TUD_PK" isvisible="false">
                            </th>
                            <th fieldmap="SL_NO" isvisible="false">
                            </th>
                            <th fieldmap="TUD_TANK_TYPE" isvisible="false">
                            </th>
                            <th fieldmap="TUD_TANK" isvisible="false">
                            </th>
                            <th fieldmap="TUD_ITEM_TYPE" isvisible="false">
                            </th>
                            <th fieldmap="TUD_ITEM" isvisible="false">
                            </th>
                            <th fieldmap="TUD_BATCH" isvisible="false">
                            </th>
                            <th fieldmap="TUD_QUANTITY" isvisible="false">
                            </th>
                            <th fieldmap="TUD_QTY_UOM" isvisible="false">
                            </th>
                            <th fieldmap="TUD_TANK_TYPE_NAME" align="left" width="15%">
                                <%=Resources.Controls.TankType%>
                                *
                            </th>
                            <th fieldmap="TUD_TANK_NAME" align="left" width="15%">
                                <%=Resources.Controls.TankNames%>
                                *
                            </th>
                            <th fieldmap="TUD_ITEM_TYPE_NAME" align="left" width="15%">
                                <%=Resources.Controls.Type%>
                                *
                            </th>
                            <th fieldmap="TUD_ITEM_NAME" align="left" width="15%">
                                <%=Resources.Controls.Item%>
                                *
                            </th>
                            <th fieldmap="TUD_BATCH_NAME" align="left" width="15%">
                                <%=Resources.Controls.BatchNo%>
                                *
                            </th>
                            <th fieldmap="QUANTITY" align="right" width="20%">
                                <%=Resources.Controls.Quantity%>
                                *
                            </th>
                            <th type="Template" width="5%">
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
    <asp:HiddenField ID="TUH_PK" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="TUD_PK" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="SL_NO" runat="server" Value="0"></asp:HiddenField>
    <asp:HiddenField ID="ItemList" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdfTUD_BATCH" runat="server" Value="0"></asp:HiddenField>
    <div id="divDatas">
    </div>
</asp:Content>
