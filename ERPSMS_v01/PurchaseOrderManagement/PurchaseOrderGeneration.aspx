<%@ Page Title="<%$ Resources:Captions,Title_PurchaseOrder %>" Language="C#" MasterPageFile="~/ERPSMS.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderGeneration.aspx.cs" Theme="ERP-Blue" Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderGeneration" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseOrderManagement/PurchaseOrderGeneration.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="webwizard-wrap">
        <h1>Purchase Order</h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right; text-align: right">
         
       <%-- <asp:ImageButton ID="ImageButton1" runat="server" SkinID="btndraft" OnClientClick="javascript:return SavePage('Draft');" />--%>
            <asp:ImageButton ID="imbDraft" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');" />
           <%-- <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />--%>
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
            <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPagePrint" OnClientClick="javascript:return PrintPage();" />
        </div>
 </div>
 <div id="grdTable-wrap">
 <div id="divPOData"></div>
 <div id="divPRData"></div>
    <div id="divData">
        <div id="divFileData"></div>
        <div id="Wofkflowdiv">
                <%-- <asp:HiddenField ID="ActionID" runat="server" />--%>
                <asp:HiddenField ID="TaskID" runat="server" />
                <asp:HiddenField ID="TaskName" runat="server" /> 
                <asp:HiddenField ID="ReferenceID" runat="server" Value="0" />
                <asp:HiddenField ID="ProcessID" runat="server" Value="3" />
                <asp:HiddenField ID="ApplicationID" runat="server" />  
                <asp:HiddenField runat="server" ID ="ActionID" />                       
        </div>
        <asp:HiddenField ID="PRDetails" runat="server" />
        <asp:HiddenField ID="MaterialDetails" runat="server" />
        <asp:HiddenField ID="BizUnitPk" runat="server" />
        <asp:HiddenField ID="UserID" runat="server" />
        <asp:HiddenField ID ="POH_PK" runat="server" Value="0" />
        <asp:HiddenField ID="WrkfComment" runat="server"/>
        <%--<asp:HiddenField ID="DeptPk" runat="server" />--%>

        <div class="div2col-S">
      
                <label><%=Resources.Controls.PoNumber%> </label>
                <asp:Label ID="lblPOH_NO" runat="server" Text=""></asp:Label>
                <asp:HiddenField ID="POH_NO" runat="server" Value="0" />

                <label><%=Resources.Controls.createdBy%> </label>
                <asp:Label ID="CreatedBy" runat="server" Text=""></asp:Label>
                <asp:HiddenField  ID="POH_CRTD_BY" runat="server" />
     

        </div>
        <div class="div2col-S">
           
                    <label for="RequiredBy" ><%=Resources.Controls.RequiredBy%> *</label>
                    <asp:TextBox ID="POH_DATE" runat="server" TabIndex="1"></asp:TextBox>

                    <label for="Required For" ><%=Resources.Controls.RequiredFor%> *</label>
                    <asp:DropDownList runat = "server" ID="POH_DEPT" TabIndex="2"></asp:DropDownList>

         
        </div>
        <div class="clear"></div>


   
     
         
                <div class="div3col-Hi-Lbx">
                 
                        <h1><%=Resources.Controls.VendorDetails%></h1>
                        <label for="Vendor"><%=Resources.Controls.Vendor%> *</label>
                        <asp:DropDownList ID="POH_VENDOR" runat="server" onchange="javascript:FillVendorDetails($(this).val(),true)" TabIndex="3">
                        </asp:DropDownList>
                        <div class="clear"></div>
                        <label><%=Resources.Controls.Vendor%> </label>
                        <%--<asp:Label ID="VendorName" runat="server"></asp:Label>--%>
                        <p id="VendorName"></p>
                        <div class="clear"></div>
                        <label><%=Resources.Controls.ContactName%> </label>
                        <%--<asp:Label ID="ContactName" runat="server"></asp:Label>--%>
                        <p id="ContactName"></p>
                        <div class="clear"></div>
                        <label><%=Resources.Controls.Tin%></label>
                        <%--<asp:Label ID="TinNo" runat="server"></asp:Label>--%>
                        <p id="TinNo"></p>
                        <div class="clear"></div>
                        <label><%=Resources.Controls.Address%></label>
                        <%--<asp:Label ID="VendorAddressDtls" runat="server"></asp:Label>--%>
                        <p id="VendorAddressDtls"></p>
               
                </div>
        
                <div class="div3col-Hi-Lbx">
                    
           
                        <h1><%=Resources.Controls.ShippingDetails%></h1>
                            
                        <label for="ShippingAddress"><%=Resources.Controls.ShippingAddress%> *</label>
                        <asp:DropDownList ID="POH_SHIPPING" runat="server" onchange="javascript:FillDepartmentDetails($(this).val(),'Shipping')" TabIndex="4">          
                        </asp:DropDownList>
                        <div class="clear"></div>
                
                        <label><%=Resources.Controls.SiteName%></label>
                        <p id="ShippingSiteName"> </p>

                        <%--<asp:Label ID="ShippingSiteName" runat="server"></asp:Label>--%>
                        <div class="clear"></div>

                        <label><%=Resources.Controls.Address%></label>
                        <%--<asp:Label ID="ShippingAddressDtls" runat="server"></asp:Label>--%>
                        <p id="ShippingAddressDtls"> </p>
        
                    </div>
           
        
                <div class="div3col-Hi-Lbx">
                    <h1><%=Resources.Controls.BillingDetails%></h1>
   
                        <label for="BillingAddress"><%=Resources.Controls.BillingAddress%> *</label>
                        <asp:DropDownList ID="POH_BILLING" runat="server" onchange="javascript:FillDepartmentDetails($(this).val(),'Billing')" TabIndex="5">             
                        </asp:DropDownList>
                        <div class="clear"></div>

                        <label><%=Resources.Controls.SiteName%></label>
                        <%--<asp:Label ID="BillingSiteName" runat="server"></asp:Label>--%>
                         <p id="BillingSiteName"> </p>
                        <div class="clear"></div>

                        <label><%=Resources.Controls.Address%></label>
                        <%--<asp:Label ID="BillingAddressDtls" runat="server"></asp:Label>--%>
                         <p id="BillingAddressDtls"> </p>
        
                </div>

 
        <div class="clear"></div>

        <div class="grdTable">
         <div class="div3col-VMappingListing">
            <label for="POD_ITEM"><%=Resources.Controls.Material%></label>
            <asp:DropDownList ID="POD_ITEM"  runat="server" onchange="javascript:FillMaterialDetails($(this))"></asp:DropDownList>
            <asp:HiddenField ID="EditMaterial" runat="server" />

            <label for="POD_ITEM"><%=Resources.Controls.MaterialCode%></label>
            <asp:DropDownList ID="ITM_CODE"  runat="server" onchange="javascript:FillMaterialDetails($(this))" ></asp:DropDownList>

            <label for="POD_UOM"><%=Resources.Controls.UOM%></label>
            <asp:DropDownList ID="POD_UOM"  runat="server" onchange="javascript:GetUomConversion($(this).val())" ></asp:DropDownList>
             <asp:HiddenField runat="server" ID="POD_CONV_FACT" Value="1" />
             <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbSelect" OnClientClick="javascript:return AddPRDetails()" Width="16px" TabIndex="13" />
         </div>
            
           

            <div id="poDetails">
            <table rules="all" id="grdPODetails" grandtype="GrandGrid" paging="false" editfunction="GridMaterialAction" editable="true">
                <thead>
                    <tr>
                        <th fieldmap="POD_PK" isvisible="false"></th>
                        <th fieldmap="POMaterialID" isvisible="false"></th>
                        <th fieldmap="POD_ITEM" isvisible="false"></th>
                        <th fieldmap="POD_UOM" isvisible="false"></th>        
                        <th fieldmap="POD_CONV_FACT" isvisible="false"></th>
                                          
                        <th fieldmap="ITV_NAME" width="15%"><%=Resources.Controls.Materials%></th>
                        <th fieldmap="ITM_CODE"  width="10%"><%=Resources.Controls.MaterialCode%></th>
                        <th fieldmap="POD_RATE"  width="10%" align="right"><%=Resources.Controls.Price%></th>
                        <th fieldmap="POD_QTY_REQUESTED" width="10%" align="right"><%=Resources.Controls.Quantity%></th>
                        <th fieldmap="POD_UOM_TEXT" width="5%"><%=Resources.Controls.UOM%></th>
                        <th fieldmap="POD_DISC_AMT" width="8%" align="right"><%=Resources.Controls.Discount%></th>
                        <%--<th fieldmap="TAX_PERC" width="8%" align="right">Tax (%)</th> --%>
                       <th fieldmap="TaxPerPiece" width="8%" align="right"><%=Resources.Controls.Tax%> (%)</th>
                        <th fieldmap="POD_REMARKS" width="17%"><%=Resources.Controls.Remarks%> </th>
                        <th fieldmap="POD_AMT_VALUE" width="10%" align="right">Sub Total</th>     
                                
                        <th type="Template" width="10%">
                            <div>
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"/>
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
            </div>
            <br class="clear" />

            <div id="dummydiv">
            <table id="dummytable">
                
                <tr>
                    <td colspan="8" style="text-align: right">
                        <%=Resources.Controls.SubTotal%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="POH_SUB_TOTAL" EnableTheming="false"  style="text-align:right"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: right">
                        - <%=Resources.Controls.Discount%>
                    </td>
                    <td style="text-align: right">
                       <%-- <asp:TextBox runat="server" ID="POH_DISC_PERC"></asp:TextBox>--%>
                        <asp:TextBox runat="server" ID="POH_DISC_AMT" onblur="javascript:CalculateTotal();" EnableTheming="false" style="text-align:right" TabIndex="14"></asp:TextBox>
                    </td>
                     <td></td>
                </tr>
                 <tr>
                    <td colspan="8" style="text-align: right">
                        <%=Resources.Controls.TotalNet%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="POH_NET_TOTAL" EnableTheming="false" style="text-align:right" TabIndex="15"></asp:TextBox>
                        
                    </td>
                     <td></td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: right">
                        + <%=Resources.Controls.ShippingCost%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="POH_SHIP_CHARGE" EnableTheming="false"  style="text-align:right"></asp:TextBox>
                        
                    </td>
                     <td></td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: right">
                        + <%=Resources.Controls.SalesTaxRate%>
                    </td>
                    <td style="text-align: right">
                        
                        <%--<asp:TextBox runat="server" ID="POH_SALES_TAX_PERC"></asp:TextBox>--%>
                        <asp:TextBox runat="server" ID="POH_SALES_TAX_AMT" EnableTheming="false"  style="text-align:right"></asp:TextBox>
                        
                    </td>
                     <td></td>
                </tr>
                 <tr>
                    <td colspan="8" style="text-align: right">
                        + <%=Resources.Controls.AdditionalTaxRate%>
                    </td>
                    <td style="text-align: right">
                       <%-- <asp:TextBox runat="server" ID="POH_ADD_TAX_PERC"></asp:TextBox>--%>
                        <asp:TextBox runat="server" ID="POH_ADD_TAX_AMT" EnableTheming="false" style="text-align:right"></asp:TextBox>
                        
                    </td>
                     <td></td>
                </tr>
                 <tr>
                    <td colspan="8" style="text-align: right">
                       +/- <%=Resources.Controls.PriceAdjustment%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="POH_PRICE_ADJUST" onblur="javascript:CalculateTotal();" EnableTheming="false"  style="text-align:right"></asp:TextBox>
                    </td>
                     <td></td>
                </tr>
                <tr>
                    <td colspan="8" style="text-align: right">
                       Total
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="POH_TOTAL_VALUE" EnableTheming="false" style="text-align:right"></asp:TextBox>
                    </td>
                     <td></td>
                </tr>

            </table>
            </div>

             
     </div>
        <div class="clear"></div>

        <div>
            <h1><%=Resources.Controls.TermsAndConditions%></h1>
            <div class="clear"></div>
            <div class="div3col-S">
                <label for="VENDOR_TERMS"><%=Resources.Controls.VendorTerms%></label>
                <asp:DropDownList ID="VENDOR_TERMS" runat="server" onchange="javascript:FillVendorTermsDetails($(this).val());"  Width="120px"></asp:DropDownList>
                <asp:ImageButton runat="server" ID="ImageButton1" SkinID="btnrefresh" OnClientClick="javascript:return ClearTerms('Vendor')" Width="16px" />

                 <div class="scroll-h150" style="margin:0">
                    <asp:Label runat="server" ID="LblPOH_VENDOR_TERMS" EnableTheming="false" Width="100%"></asp:Label>
                </div>      
                <asp:HiddenField runat="server" ID="POH_VENDOR_TERMS" /> 


               <%-- <asp:TextBox ID="POH_VENDOR_TERMS" runat="server" TextMode="MultiLine" EnableTheming="false" Width="96%" Rows="6"></asp:TextBox>--%>
            </div>
            <div class="div3col-S">
                <label for="GENERAL_TERMS"><%=Resources.Controls.GeneralTerms%></label>
                <asp:DropDownList ID="GENERAL_TERMS" runat="server" onchange="javascript:FillGeneralTerms($(this).val());" Width="120px"></asp:DropDownList>
                <asp:ImageButton runat="server" ID="imgbtnclear" SkinID="btnrefresh"  OnClientClick="javascript:return ClearTerms('General')" Width="16px" />
               
                <div class="scroll-h150" style="margin:0">
                    <asp:Label runat="server" ID="LblPOH_TERMS" EnableTheming="false" Width="100%"></asp:Label>
                </div>      
                <asp:HiddenField runat="server" ID="POH_TERMS" />     
            </div>
            <div class="div3col-S">
              <label for="POH_COMMENTS"><%=Resources.Controls.Comments%></label>
              <div class="clear"></div>
                <asp:TextBox runat="server" ID="POH_COMMENTS" TextMode="MultiLine" EnableTheming="false" Width="96%" Height="170px" Rows="6"></asp:TextBox>
            </div>
            <div class="clear"></div>     
        
        </div>

         <h1><%=Resources.Controls.AttachmentsAndRemarks%></h1>
         <div class="div2col-S" style="padding-bottom:0; margin-bottom:0;">
                <div class="divcol-FileuplWrap">
                    <label for="aupDocument" style="width:100px"><%=Resources.Controls.Attachments%></label>
                    <div id="FileUploader" >
                    <asp:FileUpload ID="fupUploader" runat="server" clientidmode="Static" size="20" height="22px" TabIndex="15"/>
                    <asp:HiddenField ID="FILELIST" runat="server" />
                    </div>
                </div>
         </div>
               
        <div class="clear"></div>

    </div>


     <div class="divcol-actiowrap" id="divAction">
          <label for="WrkfComment">
                <%=Resources.Controls.Comments%></label>
            <asp:TextBox ID="PRDComments" runat="server" TextMode="MultiLine" Width="396px" Height="60px" EnableTheming="false">
            </asp:TextBox>
            <div class="clear">
            </div>    
     <label for="WRKFACT_ID" ><%=Resources.Controls.Action%></label>
                <asp:DropDownList runat="server" ID="WRKFACT_ID" width="200px">
                </asp:DropDownList>
                <input type="button" id="imbSave" class="inputbtn" value="<%=Resources.Controls.Saveandsubmit%>"  style="width:140px" onclick="javascript:return SavePage('Save');"; />
    </div>
    <div class="divcol-actiowrap">
        <h2><%=Resources.Controls.Comments%></h2>
            <div style="text-align: right;float:right">
                <img id="imgWrkfCommentHide" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="Show" title="Show"  style="cursor:pointer" onclick="javascript:GrandScriptUtils.ShowWorkFlowCommandList();"/>
                <img id="imgWrkfCommentShow" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="Hide" title="Hide"  style="display:none;cursor:pointer" onclick="javascript:GrandScriptUtils.HideWorkFlowCommandList();" />
            </div>
            <div id="divWrkfComment" class="grdTable" style="display:none">
                <table rules="all" id="grdWrkfComment" grandtype="GrandGrid" paging="false" width="100%">
                    <thead>
                        <tr>
                            <th fieldmap="ACTION_NAME" align="left" width="35%">
                                <%=Resources.Controls.Action%>
                            </th>
                            <th fieldmap="CMT_DESC" align="left" width="45%">
                                <%=Resources.Controls.Comments%>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>


    <div class="clear"></div>
</div>

<div id="divPRDetails" title="<%=Resources.Controls.PRDetails%>">
 <div class="grdTable">
        <div id="divPRData">
        <table rules="all" id="grdPRDetails" grandtype="GrandGrid" paging="false" editfunction="GridAction" editable="true">
                            <thead>
                                <tr>
                                  <%--PRH_PK,PRH_NO,PRH_DATE,PRD_QTY_APPROVED,QTY_BALANCE,PRD_UOM,UOM_NAME,QTY_ORDER--%>
                                    <th fieldmap="PRH_PK" isvisible="false"></th>
                                    <th fieldmap="PRD_UOM" isvisible="false"></th>
                                    <th fieldmap="QTY_ORDER" isvisible="false"></th>

                                    <%--<th fieldmap="QTY_ORDER" isvisible="false"></th>--%>
                                   
                                    <th fieldmap="PRH_NO" align="left" width="20%"><%=Resources.Controls.PRNo%></th> 
                                    <th fieldmap="PRH_DATE" width="20%"><%=Resources.Controls.Date%></th>
                                    <th fieldmap="PRD_QTY_APPROVED" width="15%"><%=Resources.Controls.ApprovedQty%></th>
                                    <th fieldmap="QTY_BALANCE" width="15%"><%=Resources.Controls.BalanceQty%></th>                                  
                                    <th fieldmap="UOM_NAME" width="15%"><%=Resources.Controls.UOM%></th>                                  
                                    <th fieldmap="PRD_ITEM" isvisible="false"></th>    
                                    <th fieldmap ="UOM_CONV_EXIST" isvisible="false"></th>
                                    <th type="Template" width="15%" align="right" >  
                                    <div style="text-align:right">
                                      <asp:TextBox runat="server" ID="txtQty" style="text-align:right;" width="80%" EnableTheming="false" typed="Orderqty" onBlur="javascript:CalculatePRTotal(this);"  onkeypress="javascript:MakeNumeric(event);" ></asp:TextBox>
                                    </div>  
                                    </th>
                                </tr>
                            </thead>
                    </table>
                    
        </div>
        <div class="clear"></div>
     

 <div id="divDummyPR">
            <table id="dummyPR" style="width:100%">
                <tr class='tDummy' >
                    <td colspan="4" style="text-align: right">
                       <%=Resources.Controls.AdditionalQty%>
                    </td>
                     <td style="text-align: right">
                       <label id="MaterialUOMText"></label>
                       <asp:HiddenField runat="server" ID="MaterialUOM" />
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="AdditionalQty" typed="Additionalqty" EnableTheming="false" onBlur="javascript:CalculatePRTotal();" onkeypress="javascript:MakeNumeric(event);" style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr class='tDummy'>
                    <td colspan="5" style="text-align: right">
                         <%=Resources.Controls.TotalQty%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="TotalQty" EnableTheming="false"  style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>

                <tr class='tDummy'>
                    <td colspan="5" style="text-align: right">
                        <%=Resources.Controls.UnitPrice%> 
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="MaterialPrice" EnableTheming="false" onblur="javascript:CalculatePRMaterialTotal();" onkeypress="javascript:MakeNumeric(event);" style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>
                 <tr class='tDummy'>
                    <td colspan="5" style="text-align: right">
                         <%=Resources.Controls.Discount%> 
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="MaterialDiscount" EnableTheming="false" onblur="javascript:CalculatePRMaterialTotal('Discount');" onkeypress="javascript:MakeNumeric(event);" style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>
                 <tr class='tDummy'>
                    <td colspan="5" style="text-align: right">
                        <%=Resources.Controls.Tax%> 
                    </td>
                    <td style="text-align: right">
                        <asp:HiddenField runat="server" ID="TaxPerPiece" />
                        <asp:HiddenField runat="server" ID="PRMaterial" />
                        <asp:HiddenField runat="server" ID="PRMaterialName" />    
                        <asp:HiddenField runat="server" ID="PRMaterialCode" />       
                        <asp:HiddenField runat="server" ID="PerPiecePrice" /> 
                                       
                        <asp:TextBox runat="server" ID="MaterialTax" EnableTheming="false"  style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>

                 <tr class='tDummy'>
                    <td colspan="5" style="text-align: right">
                        <%=Resources.Controls.SubTotal%>
                    </td>
                    <td style="text-align: right">
                        <asp:TextBox runat="server" ID="MaterailTotal" EnableTheming="false"  style="text-align:right;width=80%"></asp:TextBox>
                    </td>
                </tr>
                <tr class='tDummy'>
                   <td colspan="6">
                       <label for="MaterialRemarks"><%=Resources.Controls.Remarks%></label>
                       <asp:TextBox ID="MaterialRemarks" runat="server" Width="90%" EnableTheming="false" MaxLength="200"
                                TextMode="MultiLine" onkeypress="return (this.value.length<200)" onpaste="return this.value.length<200" Height="60px" >
                            </asp:TextBox>
                      <%-- <textarea id="MaterialRemarks" style="width:90%" rows="5" onkeypress="return (this.value.length<200)" onpaste="return this.value.length<200"></textarea>--%>
                    </td>
                    
                </tr>
                <tr class='tDummy'>
                    <td colspan="6" style="text-align: right">
                       <input type="button" class="inputbtn" value ="Add To PO List" onclick="javascript:AddtoPoList();"/>
                    </td>
                    
                </tr>


               
            </table>
   </div>

</div>
</div>

</asp:Content>
