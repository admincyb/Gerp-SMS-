<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvanceSearch.ascx.cs"
    EnableTheming="true" Inherits="ERPSMS_v01.UserControls.AdvanceSearch" %>
<div id="AdvanceSearch">
    <div id="tabs">
        <ul>
            <li><a href="#NewSearch">
                <%=Resources.Controls.NewSearch%></a></li>
            <li><a href="#SavedSearch">
                <%=Resources.Controls.SavedSearch%></a></li>
        </ul>
        <div class="clear">
        </div>
        <div id="NewSearch" class="divcol-inline" style="padding: 10px">
            <label for="srchboxtextbx">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="AdvSrchType" runat="server" onchange="javascript:SetAdvSearch();"
                Width="200px" EnableViewState="false">
            </asp:DropDownList>
            <label for="AdvSrchOpr">
                <%=Resources.Controls.Op%></label>
            <asp:DropDownList ID="AdvSrchOpr" runat="server" Width="70px" EnableViewState="false">
                <asp:ListItem Value="0" Text="=">
                </asp:ListItem>
                <asp:ListItem Value="1" Text="<>">
                </asp:ListItem>
                <asp:ListItem Value="2" Text="<">
                </asp:ListItem>
                <asp:ListItem Value="3" Text=">">
                </asp:ListItem>
                <asp:ListItem Value="4" Text="<=">
                </asp:ListItem>
                <asp:ListItem Value="5" Text=">=">
                </asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="AdvSrchValue" runat="server" EnableViewState="false">
            </asp:TextBox>
            <asp:TextBox ID="AdvDateSrchValue" runat="server" EnableViewState="false">
            </asp:TextBox>
            <img src="../Images/ERP-Blue/page-btn-listadd.gif" title="Add" onclick="javascript:return AddToAdvSearchList();" />
            <img src="../Images/ERP-Blue/page-btn-search.gif" title="Search" onclick="javascript:return AdvSearch();" />
            <img src="../Images/ERP-Blue/page-btn-saveNsearch.gif" title="Save And Search" onclick="javascript:return AdvSaveAndSearch();" />
            <img src="../Images/ERP-Blue/page-btn-reset.gif" title="Reset" onclick="javascript:return AdvanceSearchReset();" />
            <img src="../Images/ERP-Blue/page-btn-back.gif" title="Back" onclick="javascript:return HideAdvSearch();" />
            <div class="listTable" style="max-height: 120px; overflow-y: auto; overflow-x: hidden;
                margin: 0px 0px 10px 0px; padding: 5px; width: 60%;">
                <table id="grdAdvSrch" rules="all">
                </table>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="SavedSearch">
            <div class="grdTable" style="height: 142px; overflow-y: auto; overflow-x: hidden;
                margin: 0px 0px 10px 0px; /*border-top: dotted 1px #7388a4; */ padding: 5px">
                <table id="grdSrchTemplate" grandtype="GrandGrid" pagesize="20" paging="true" editfunction="GridAction"
                    editable="true">
                    <thead>
                        <tr>
                            <th fieldmap="SRH_PK" isvisible="false">
                            </th>
                            <th fieldmap="SRH_NAME" sortable="true" align="left" width="70%">
                                <%=Resources.Controls.SearchName%>
                            </th>
                            <th type="Template" width="10%">
                                <div>
                                    <asp:ImageButton runat="server" ID="imbEdit" Width="18px" Height="18px" SkinID="imbeditgrid"
                                        OnClientClick="javascript:return FillAdvSearch($(this).parents('tr:eq(0)'),'Edit')" />
                                </div>
                            </th>
                    </thead>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="SearchID" runat="server" Value="0">
        </asp:HiddenField>
        <asp:HiddenField ID="SearchDtlPK" runat="server" Value="0">
        </asp:HiddenField>
        <div class="clear">
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<div id="TemplateDiv" style="width: 500px; margin-top: 25px; margin-bottom: 20px">
    <div class="div2col-S">
        <label for="TemplateName">
            <%=Resources.Controls.SearchName%>
        </label>
        <asp:TextBox runat="server" ID="SearchName" MaxLength="100" EnableViewState="false">
        </asp:TextBox>
        <asp:ImageButton ID="imbSearchSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SaveAdvSrchTemplate();"
            TabIndex="11" EnableViewState="false" Width="18px" Height="18px" />
        <div class="clear">
        </div>
        <asp:HiddenField ID="SearchPK" runat="server" Value="0">
        </asp:HiddenField>
        <asp:HiddenField ID="SearchDetails" runat="server">
        </asp:HiddenField>
        <asp:HiddenField ID="UserID" runat="server">
        </asp:HiddenField>
        <asp:HiddenField ID="PageTitle" runat="server">
        </asp:HiddenField>
    </div>
</div>
