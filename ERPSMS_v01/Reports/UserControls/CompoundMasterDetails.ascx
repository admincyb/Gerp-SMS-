<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompoundMasterDetails.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.CompoundMasterDetails" %>
<script type="text/javascript">
    function InitComponentsCmp() {
        GrandScriptUtils.MakeAutoCompleteDDL("txtMasterCode", url + "&StatusVal=" + $("[id$=ddlStatus]").val() + "&CompanyPK=" + $("[id$=ddlPlant]").val(), "hdfMasterCode", true, true, "COMPMSTFILTER");
    }
</script>
<div id="DET_SEC_1" class="fields-grpwrap color-grey grp-before pad-t10 color-white">

    <div class="fields-group">
        <table class="table-devide" border="0">
            <tbody>
                <tr>
                    <td>
                        <div class="div2col-M0">
                            <label for="ddlPlant" id="lblPlant" style="font-family: Verdana; height: 12px; margin: 0 3px 10px 0;">Plant</label>
                            <input type="hidden" name="hdfAutoHDN_CIM_PK" id="hdfAutoHDN_CIM_PK">
                            <asp:DropDownList ID="ddlPlant" TabIndex="3" Style="color: #303030; background-color: White; border-color: #EAF2F5; border-style: Solid; font-family: Verdana; font-size: 11px; padding: 3px 2px; margin: 0 3px 10px 0;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" title="Select/Type" runat="server" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                            </asp:DropDownList>



                            <input type="hidden" name="hdfPkLNE_PK" id="hdfPkLNE_PK" value="21072">
                            <input type="hidden" name="hdfLNE_PKType" id="hdfLNE_PKType" value="AutoComplete">
                            <input type="image" name="btnLNE_PK" id="btnLNE_PK" title="HDN_CIM_PK" src="" style="border-width: 0px; display: none"><input type="hidden" name="hdfActionLNE_PK" id="hdfActionLNE_PK" value="LNE_PK">
                        </div>


                    </td>

                    <td>
                        <div class="div2col-M0">
                            <label for="ddlStatus" id="lblStatus" style="font-family: Verdana; height: 12px; margin: 0 3px 10px 0;">Status</label>


                            <asp:DropDownList ID="ddlStatus" TabIndex="3" Style="color: #303030; background-color: White; border-color: #EAF2F5; border-style: Solid; font-family: Verdana; font-size: 11px; padding: 3px 2px; margin: 0 3px 10px 0;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" title="Select/Type" runat="server" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                            </asp:DropDownList>

                            <input type="hidden" name="hdfPkPTH_PRODUCT" id="hdfPkPTH_PRODUCT" value="21073"><input type="hidden" name="hdfPTH_PRODUCTType" id="hdfPTH_PRODUCTType" value="AutoComplete">
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="table-devide" border="0">
            <tbody>
                <tr>

                    <td>
                        <div class="div2col-M0">
                            <label for="txtMasterCode" id="lblMasterCode" style="font-family: Verdana; height: 12px; margin: 0 3px 10px 0;">Master Code</label>
                            <asp:TextBox ID="txtMasterCode" runat="server" MaxLength="100" CssClass="select-halfsmall" />
                            <asp:HiddenField ID="hdfMasterCode" runat="server" />
                        </div>

                    </td>
                    <td>
                        <div class="div2col-M0"></div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
