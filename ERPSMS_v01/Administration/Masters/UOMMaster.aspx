<%@ Page Title="<%$ Resources:Captions,Title_UOMMaster %>" Theme="ClassicExt" Language="C#"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="UOMMaster.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.UOMMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/UOMManagement/UOMMaster.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.UOMMaster%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="false" TabIndex="10" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                EnableViewState="false" TabIndex="7" />
            <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="false" TabIndex="8" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="9" />
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
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="true" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="7" EnableViewState="true" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="8" EnableViewState="true" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="true" OnClientClick="javascript:return CancelFun();" TabIndex="9" />--%>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="true" OnClientClick="javascript:return ResetPage();" TabIndex="9" />
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
                    <label for="SearchType">
                        <%=Resources.Controls.SearchBy%></label>
                    <asp:DropDownList ID="SearchType" runat="server" TabIndex="1" CssClass="srchboxtextbx"
                        EnableViewState="false">
                        <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>--%>
                        <asp:ListItem Value="UMT_NAME" Text="<%$ Resources:BindValues, UOMType%>"></asp:ListItem>
                        <asp:ListItem Value="UOM_Name" Text="<%$ Resources:BindValues, UOMName%>"></asp:ListItem>
                        <asp:ListItem Value="UOM_Code" Text="<%$ Resources:BindValues, UOMCode%>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" EnableViewState="false" />
                    <div class="clear">
                    </div>
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdUOMDetails" grandtype="GrandGrid" ajaxurl="UOMManagement.do?Action=GetUOMList"
                        width="100%" pagesize="20" paging="true" editfunction="GridAction" editable="true"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="UOM_PK" isvisible="false">
                                </th>
                                <th fieldmap="UOM_TYPE" isvisible="false">
                                </th>
                                <th fieldmap="UOM_DEFAULT" isvisible="false">
                                </th>
                                <th fieldmap="UMT_NAME" sortable="true" align="left" width="30%">
                                    <%=Resources.Controls.UOMType%>
                                </th>
                                <th fieldmap="UOM_NAME" sortable="true" align="left" width=" 30%">
                                    <%=Resources.Controls.UOMName%>
                                </th>
                                <th fieldmap="UOM_CODE" sortable="true" align="left" width="20%">
                                    <%=Resources.Controls.UOMCode%>
                                </th>
                                <th fieldmap="UOM_DECIMAL" sortable="true" align="right" width="15%">
                                    <%=Resources.Controls.UOMDecimal%>
                                </th>
                                <th type="Template" style="width: 5%" align="left">
                                    <div style="text-align: left">
                                        <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" EnableViewState="false"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                        <%--<asp:ImageButton runat="server" ID="imgActive" SkinID="btnactive" EnableViewState="false"
                                        OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Active')" />
                                      <asp:ImageButton runat="server" ID="imgInActive" SkinID="btninactive" EnableViewState="false"
                                        OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'InActive')" />--%>
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
            </div>
            <div id="divData">
                <asp:HiddenField ID="SBU" runat="server" Value="0" />
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="UOM_TYPE">
                                    <%=Resources.Controls.UOMType%>
                                    *</label>
                                <asp:DropDownList ID="UOM_TYPE" runat="server" TabIndex="1" EnableViewState="false"
                                    CssClass="select-small-c1">
                                </asp:DropDownList>
                                <asp:ImageButton ID="imbAddUOMType" runat="server" SkinID="imbaddnew" EnableViewState="false"
                                    ToolTip="<%$ Resources:Controls, AddUOM%>" TabIndex="2" OnClientClick="javascript:return AddUOMType();" />
                                <div class="clear">
                                </div>
                                <label for="UOM_NAME">
                                    <%=Resources.Controls.UOMName%>
                                    *</label>
                                <asp:TextBox ID="UOM_NAME" runat="server" TabIndex="5" MaxLength="100" EnableViewState="false"
                                    CssClass="input-small-c">
                                </asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="UOM_CODE">
                                    <%=Resources.Controls.UOMCode%>
                                    *</label>
                                <asp:TextBox runat="server" ID="UOM_CODE" TabIndex="3" MaxLength="20" EnableViewState="false"
                                    CssClass="input-small-c">
                                </asp:TextBox>
                                <asp:ImageButton ID="imbAddConversion" runat="server" SkinID="conversion" ToolTip="<%$ Resources:Controls, AddConversion%>"
                                    TabIndex="4" OnClientClick="javascript:return AddConversion();" EnableViewState="false" />
                                <div class="clear">
                                </div>
                                <label for="UOM_DECIMAL">
                                    <%=Resources.Controls.UOMDecimal%>
                                    *</label>
                                <asp:TextBox ID="UOM_DECIMAL" runat="server" TabIndex="6" MaxLength="2" CssClass="input-small-c numeric"
                                    Style="text-align: right"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="divAddConversion" title="<%=Resources.Controls.ConversionDetails%>">
            <div class="Button-container-popup">
                <asp:Button runat="server" ID="btnAddConv" Text="<%$ Resources:Controls, Save%>"
                    EnableViewState="false" SkinID="btnInner-Save" OnClientClick="javascript:return SaveConversionDtls();" />
            </div>
            <div class="content-wrapper">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <h3 class="head-2col">
                                    <%=Resources.Captions.UOMConversionFrom%></h3>
                                <label for="UOMTypeFm">
                                    <%=Resources.Controls.UOMType%></label>
                                <span id="UOMTypeFm" class="input-half"></span>
                                <div class="clear">
                                </div>
                                <label for="A_UMC_FROM">
                                    <%=Resources.Controls.Unit%></label>
                                <span id="A_UMC_FROM" class="input-half"></span>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <h3 class="head-2col">
                                    <%=Resources.Captions.UOMConversionTo%></h3>
                                <label for="UOMTypeTo">
                                    <%=Resources.Controls.UOMType%></label>
                                <span id="UOMTypeTo" class="input-half"></span>
                                <div class="clear">
                                </div>
                                <label for="UMC_TO">
                                    <%=Resources.Controls.Unit%>*</label>
                                <asp:DropDownList ID="UMC_TO" runat="server" TabIndex="12" EnableViewState="false" CssClass="select-half-a">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="UMC_CONV_FACT">
                                    <%=Resources.Controls.ConversionValue%>*</label>
                                <asp:TextBox runat="server" ID="UMC_CONV_FACT" TabIndex="13" MaxLength="10" CssClass="numeric input-half"
                                    EnableViewState="false" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="ConversionDiv">
                    <asp:HiddenField ID="EditConversion" runat="server" Value="0" />
                    <div class="grdTable">
                        <table rules="all" id="grdConversionDtls" grandtype="GrandGrid" pagesize="5" paging="true"
                            width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="UMC_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="UMC_FROM" isvisible="false">
                                    </th>
                                    <th fieldmap="UMC_TO" isvisible="false">
                                    </th>
                                    <th fieldmap="UMC_UOM_TYPE" isvisible="false">
                                    </th>
                                    <th fieldmap="UOMTYPEText" align="left" width="20%">
                                        <%=Resources.Controls.UOMType%>
                                    </th>
                                    <th fieldmap="FromUnitText" align="left" width="20%">
                                        <%=Resources.Controls.From%>
                                    </th>
                                    <th fieldmap="ToUnitText" align="left" width="20%">
                                        <%=Resources.Controls.To%>
                                    </th>
                                    <th fieldmap="UMC_CONV_FACT" align="right" width="20%">
                                        <%=Resources.Controls.Value%>
                                    </th>
                                    <th type="Template" width="20%">
                                        <div style="text-align: center">
                                            <asp:ImageButton runat="server" ID="ImageButton1" SkinID="imbeditgrid" EnableViewState="false"
                                                OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'EditConv')" />
                                            <asp:ImageButton runat="server" ID="ImageButton2" SkinID="imbdeletegrid" EnableViewState="false"
                                                OnClientClick="javascript:return ConversionGridHandler($(this).parents('tr:eq(0)'),'DeleteConv')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div id="divAddUomType">
            <div class="content-wrapper">
                <div class="search-wrap-c">
                    <label for="UMT_NAME">
                        <%=Resources.Controls.UOMType%>
                        *
                    </label>
                    <asp:TextBox runat="server" ID="UMT_NAME" TabIndex="15" MaxLength="50" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="BizUnit" runat="server" Value="0" />
                    <asp:Button runat="server" ID="btnAddUomType" Text="<%$ Resources:Controls, Save%>"
                        EnableViewState="false" SkinID="btnInner-Save" OnClientClick="javascript:return SaveUOMType();" />
                </div>
                <div class="clear">
                </div>
                <div class="grdTable max-250">
                    <table rules="all" id="grdUOMType" grandtype="GrandGrid" pagesize="5" paging="true"
                        editfunction="GridAction" editable="true" class="gridwraptable gridwrap" width="100%">
                        <thead>
                            <tr>
                                <th fieldmap="UMT_PK" isvisible="false">
                                </th>
                                <th fieldmap="UMT_NAME" sortable="true" align="left" width="85%">
                                    <%=Resources.Controls.UOMType%>
                                </th>
                                <th fieldmap="UMT_DEFAULT" isvisible="false">
                                </th>
                                <th type="Template" width="15%">
                                    <div style="text-align: center">
                                        <asp:ImageButton runat="server" ID="imbEditType" EnableViewState="false" SkinID="imbeditgrid"
                                            OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'EditType')" />
                                        <asp:ImageButton runat="server" ID="imbDelType" EnableViewState="false" SkinID="imbdeletegrid"
                                            OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'DeleteType')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <asp:HiddenField ID="UMT_PK" runat="server" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="UOM_PK" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="ConversionList" Value="0" />
    <div id="divDatas">
    </div>
</asp:Content>
