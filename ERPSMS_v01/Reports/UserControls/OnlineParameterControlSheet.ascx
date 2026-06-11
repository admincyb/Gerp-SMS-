<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnlineParameterControlSheet.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.OnlineParameterControlSheet" %>
<script type="text/javascript">
function InitUserComponents() {
        UserDateInit();
    }
    function UserDateInit() {
        //<summary>function used to make datepicker</summary>
        GrandScriptUtils.DatePickerCommon("PTH_DATE");
      //GrandScriptUtils.DatePicker("PTH_DATE", false, false);
    }

    function AfterDateSelect(inputid) {
    //    alert('Hello');
        __doPostBack('__Page', 'MyCustomArgument');
    }
</script>
<asp:UpdatePanel ID="pnlCommonReport" runat="server" class="">
    <ContentTemplate>

        <div id="DET_SEC_1" class="fields-grpwrap color-grey grp-before pad-t10 color-white">
   <div class="fields-group">
      <table class="table-devide" border="0">
         <tbody>
            <tr>
               <td>
                  <div class="div2col-M0">
                      <label for="lblPTH_DATE" id="lblPTH_DATE" style="font-family:Verdana;height:12px;margin:0 3px 10px 0;">Date</label>
                     
                      <asp:TextBox ID="PTH_DATE" runat="server"    CssClass="input-small"
                              OnTextChanged="ActionHandler"    TabIndex="10" AutoPostBack="true"></asp:TextBox>
                       <asp:HiddenField ID="hdfPTH_DATE" runat="server" Value="1" />
                      
                      
                      </td>
               <td>
                  <div class="div2col-M0">
                      <label for="txtLNE_PK" id="lblLNE_PK" style="font-family:Verdana;height:12px;margin:0 3px 10px 0;">Line</label>
                      <input type="hidden" name="hdfAutoHDN_CIM_PK" id="hdfAutoHDN_CIM_PK">
                        <asp:DropDownList id="txtLNE_PK" tabindex="3" style="color:#303030;background-color:White;border-color:#EAF2F5;border-style:Solid;font-family:Verdana;font-size:11px;padding:3px 2px;margin:0 3px 10px 0;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" title="Select/Type" runat="server" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" >
                            </asp:DropDownList>
                      
                      
 
                      <input type="hidden" name="hdfPkLNE_PK" id="hdfPkLNE_PK" value="21072">
                      <input type="hidden" name="hdfLNE_PKType" id="hdfLNE_PKType" value="AutoComplete">
                      <input type="image" name="btnLNE_PK" id="btnLNE_PK" title="HDN_CIM_PK" src="" style="border-width:0px;display:none"><input type="hidden" name="hdfActionLNE_PK" id="hdfActionLNE_PK" value="LNE_PK"></div>
 
               
               </td>
            </tr>
         </tbody>
      </table>
      <table class="table-devide" border="0">
         <tbody>
            <tr>
               <td>
                  <div class="div2col-M0">
                      <label for="txtPTH_PRODUCT" id="lblPTH_PRODUCT" style="font-family:Verdana;height:12px;margin:0 3px 10px 0;">Product</label>
                  
                      
                                              <asp:DropDownList id="txtPTH_PRODUCT" tabindex="3" style="color:#303030;background-color:White;border-color:#EAF2F5;border-style:Solid;font-family:Verdana;font-size:11px;padding:3px 2px;margin:0 3px 10px 0;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" title="Select/Type" runat="server" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                            </asp:DropDownList>
    
                      <input type="hidden" name="hdfPkPTH_PRODUCT" id="hdfPkPTH_PRODUCT" value="21073"><input type="hidden" name="hdfPTH_PRODUCTType" id="hdfPTH_PRODUCTType" value="AutoComplete"></div>
               </td>
               <td>
                  <div class="div2col-M0"></div>
               </td>
            </tr>
         </tbody>
      </table>
   </div>
</div>

      
    </ContentTemplate>
</asp:UpdatePanel>