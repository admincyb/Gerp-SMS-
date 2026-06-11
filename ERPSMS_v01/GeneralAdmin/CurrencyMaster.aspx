<%@ Page Title="<%$ Resources:Captions,Title_CurrencyMaster %>" Language="C#" Theme="ClassicExt"
MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="CurrencyMaster.aspx.cs"
EnableEventValidation="false" Inherits="ERPSMS_v01.GeneralAdmin.CurrencyMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script src="../Scripts/PageScript/Administration/Masters/CurrencyMaster.js.axd"
        type="text/javascript"></script>
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>

     <script type="text/javascript">
         function isFloatNumberKey(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode;
             if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                 if (charCode == 46)
                     return true;
                 return false;
             }

             return true;
         }      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.CurrencyMaster%></h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                EnableViewState="False" TabIndex="7" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="8" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="10" />
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
                                <asp:Button runat="server" ID="btnAddNew" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>" ToolTip ="<%$Resources:Controls,Add%>"
                                    EnableViewState="False" OnClientClick="javascript:return AddNew();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>" ToolTip="<%$Resources:Controls,Save%>"
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>" ToolTip ="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>" ToolTip ="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" />--%>
                                    <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>" ToolTip ="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False"  OnClientClick="javascript:return ResetPage();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="CurrencyCode">
                                    <%=Resources.Controls.CurrencyCode%>*</label>
                                <asp:TextBox runat="server" ID="CUR_CODE" TabIndex="11" EnableViewState="False" CssClass ="medium"></asp:TextBox>
                                <asp:ImageButton ID="imbAddLocation" runat="server" SkinID="conversion"
                                   ToolTip="<%$Resources:Captions,AddExchange%>"
                                    TabIndex="2" OnClientClick="javascript:return AddExchangeType();" EnableViewState="false" />
                                <div class="clear">
                                </div>
                                <label for="CUR_NAME">
                                    <%=Resources.Controls.CurrencyName%>*</label>
                                <asp:TextBox runat="server" ID="CUR_NAME" TabIndex="13" MaxLength="200"  
                                    EnableViewState="False"></asp:TextBox>
                                 <div class="clear">
                                </div>
                                <label for="CUR_DECIMAL">
                                    <%=Resources.Controls.CurrencyDecimal%>*</label>
                                <asp:TextBox runat="server" ID="CUR_DECIMAL" TabIndex="15" MaxLength="1"  CssClass="small numeric" Text="1"
                                    EnableViewState="False"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="CUR_FORMAT">
                                    <%=Resources.Controls.Displayin%>*</label>
                                <asp:DropDownList ID="CUR_FORMAT" runat="server"    TabIndex="12" CssClass ="medium">
                                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="<%$ Resources:BindValues, Million%>"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="<%$ Resources:BindValues, Lakhs%>"></asp:ListItem>
                                </asp:DropDownList>
                                 <div class="clear">
                                </div>
                                <label for="CUR_FRACTION">
                                    <%=Resources.Controls.Fraction%>*</label>
                                <asp:TextBox runat="server" ID="CUR_FRACTION" TabIndex="14" MaxLength="100" CssClass ="medium"   EnableViewState="False"></asp:TextBox>
                                 <div class="clear">
                                </div>
                                <label for="CUR_SYMBOL">
                                    <%=Resources.Controls.Symbol%></label>
                                <asp:TextBox runat="server" ID="CUR_SYMBOL" TabIndex="16" MaxLength="5" CssClass="medium"  EnableViewState="False"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
            </div>
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <span>
                            <%=Resources.Controls.SearchBy%></span>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                            onchange="" EnableViewState="false">
                            <%--<asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>--%>
                            <asp:ListItem Value="CUR_NAME" Text="<%$ Resources:BindValues, Name%>">
                            </asp:ListItem>
                            <asp:ListItem Value="CUR_CODE" Text="<%$ Resources:BindValues, Code%>">
                            </asp:ListItem>
                            <asp:ListItem Value="CUR_SYMBOL" Text="<%$ Resources:BindValues, Symbol%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                        </asp:TextBox>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="3" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdCurrency" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="CUR_PK" isvisible="false">
                                </th>
                                <th fieldmap="CUR_NAME" sortable="true" width="55%" align="left">
                                    <%=Resources.Controls.CurrencyName%>
                                </th>
                                <th fieldmap="CUR_CODE" sortable="true" width="15%" align="left">
                                    <%=Resources.Controls.CurrencyCode%>
                                </th>
                                <th fieldmap="CUR_FRACTION" sortable="true" width="15%" align="left">
                                    <%=Resources.Controls.Fraction%>
                                </th>
                                <th fieldmap="CUR_SYMBOL" sortable="true" width="10%" align="left">
                                    <%=Resources.Controls.Symbol%>
                                </th>
                                <th type="Template" width="5%" align="left">
                                    <div style="text-align: left">
                                        <asp:ImageButton runat="server" ID="imbEditMast" SkinID="imbeditgrid" EnableViewState="False" ToolTip="<%$ resources:Controls,Edit  %>"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                        <asp:ImageButton runat="server" ID="imbDeleteMast" SkinID="imbdeletegrid" EnableViewState="False" ToolTip="<%$ resources:Captions, Remove %>"
                                            OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>


            <div id="divExchange" title="<%=Resources.Captions.ExchangeRate%>">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnAddExchangeRate" Text="<%$ Resources:Controls, Save%>" ToolTip="<%$ Resources:Controls, Save%>"
                        EnableViewState="false" SkinID="btnInner-Save" TabIndex="16" OnClientClick="javascript:return SaveConversionDtls();" />
                </div>
                <div class="content-wrapper">

                <table class="table-devide">
                    <tbody>
                    <tr>
                        <td colspan="2">
                            <div class="div2col-Ss">
                                 <div class="divcolmiddle-S">
                        <label for="lblFromExchange" class="pop-dropdown-label-1">
                            <%=Resources.Controls.From%>
                            *
                        </label>
                        <asp:Label ID="lblFromExchange" CssClass="pop-dropdown-1"   runat="server" Text=""></asp:Label>



                     <div class="clear">
                        </div>
                    </div>
                            </div>
                            </td>
                            </tr>

                    <tr>
                        <td>
                            <div class="div2col-S">
                             <label for="CUC_TO" class="pop-dropdown-label">
                            <%=Resources.Controls.To%>
                            *
                        </label>
                        <asp:DropDownList ID="CUC_TO" CssClass="pop-dropdown-3" runat="server" TabIndex ="8">
                        </asp:DropDownList>
                            </div>
                            </td>
                             <td>
                            <div class="div2col-S">
                            
                        <label for="CUC_CONV_FACT" class="pop-dropdown-label">
                            <%=Resources.Controls.Rate%>
                            *
                        </label>
                        <asp:TextBox runat="server" ID="CUC_CONV_FACT"  CssClass="pop-dropdown" TabIndex="9" MaxLength="20" EnableViewState="False" onkeypress="return isFloatNumberKey(event);"></asp:TextBox>
                        
                            </div>
                            </td>
                            </tr>

                            <tr>
                        <td>
                            <div class="div2col-S">
                             <label for="CUC_FROM_DATE" class="pop-dropdown-label">
                            <%=Resources.Controls.FromDate%>
                            *
                        </label>
                        <asp:TextBox runat="server" ID="CUC_FROM_DATE"  CssClass="pop-dropdown" TabIndex="10" MaxLength="100" EnableViewState="False" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                       
                            </div>
                            </td>
                             <td>
                            <div class="div2col-S">
                              <label for="CUC_TO_DATE" class="pop-dropdown-label">
                            <%=Resources.Controls.ToDate%>
                            *
                        </label>
                        <asp:TextBox runat="server" ID="CUC_TO_DATE"  CssClass="pop-dropdown" TabIndex="11" MaxLength="100" EnableViewState="False" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        
                            </div>
                            </td>
                            </tr>

                            </table>







               
                </div>
                <div class="clear">
                </div>
                <div id="ExchangeDiv">
                    <div class="grdTable max-250">

                 

                        <table rules="all" id="grdExchangeRate" grandtype="GrandGrid" pagesize="10" paging="true"  width="100%" 
                            editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                  <th fieldmap="CUC_CONV_FACTOR" isvisible="false" width="0%">
                                    </th>
                                    
                                   <th fieldmap="CUC_FROM" isvisible="false" width="0%">
                                    </th>
                                    <th fieldmap="CUC_TO" isvisible="false" width="0%">
                                    </th>
                                    <th fieldmap="CUC_PK" isvisible="false" width="0%">
                                    </th>
                                    <th fieldmap="CUC_FROM_CODE" sortable="true" align="left" width="15%">
                                        <%=Resources.Controls.From%>
                                    </th>
                                    <th fieldmap="CUC_TO_CODE" sortable="true" align="left" width="15%">
                                        <%=Resources.Controls.To%>
                                    </th>
                                    <th fieldmap="CUC_CONV_FACT" sortable="true" align="left" width="10%">
                                        <%=Resources.Controls.Rate%>
                                    </th>
                                    <th fieldmap="CUC_FROM_DATE" sortable="true" align="left" width="20%">
                                        <%=Resources.Controls.FromDate%>
                                    </th>
                                    <th fieldmap="CUC_TO_DATE" sortable="true" align="left" width="20%">
                                        <%=Resources.Controls.ToDate%>
                                    </th>
                                    <th type="Template" width="20%">
                                        <div style="text-align: center">
                                            <asp:ImageButton runat="server" ID="imbEditExchangeRate" EnableViewState="false" ToolTip="<%$ resources:Controls,Edit  %>" TabIndex ="12"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'EditType')" />
                                            <asp:ImageButton runat="server" ID="imbDelExchangeRate" EnableViewState="false" SkinID="imbdeletegrid" ToolTip="<%$ resources:Captions, Remove %>"
                                                OnClientClick="javascript:return GridHandlerType($(this).parents('tr:eq(0)'),'DeleteType')" TabIndex ="13" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>



    <asp:HiddenField ID="SBU" runat="server" Value="0" />
    <asp:HiddenField ID="CUR_PK" runat="server" Value="0"  />
    <asp:HiddenField runat="server" ID="CUC_PK" Value="0" />
    <asp:HiddenField runat="server" ID="CUC_FROM" Value="0" />
    <asp:HiddenField runat="server" ID="CurrencyDetails" Value="0" EnableViewState="false" />
    <asp:HiddenField runat="server" ID="ConversionList" Value="0" EnableViewState="false" />
    <asp:HiddenField ID="EditConversion" runat="server" Value="0" />
</asp:Content>
