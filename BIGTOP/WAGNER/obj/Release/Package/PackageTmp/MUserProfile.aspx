<%@ Page Language="C#" MasterPageFile="~/MicroSite.Master" AutoEventWireup="true"
    Inherits="MUserProfile" Title="Untitled Page" Culture="en-US"
    UICulture="en-US" CodeBehind="MUserProfile.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Contentbreadcrumb" runat="server">
<div class="grid12 hidden-phone">
        <div class="mar5 breadcrumb">          
          <a href="<%=micrositeurl %>" style="color: #089fd6 !important;"> Home </a>  &gt; <a href="/MUserProfile.aspx" style="color: #089fd6 !important;"> User Profile </a>
        </div>
      </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="maincontent" runat="Server">
     <div class="grid9">   
<%--            <asp:Panel ID="pnlProfile" runat="server" DefaultButton="btnUpdate">--%>

        <h1 class="left-titleborder bggray blue border-rad5 bdr-none-btm contmgn" >View User Profile</h1>

        <div class="box5 mar5 mgntop minheight formpdng">
            <div class="mar5 usrprofile">
            <div class="grid12">
                <div class="grid3 title">Company Name
                </div>
                <div class="grid5">
                       <asp:TextBox autocomplete="off" ReadOnly="false" ID="txtcompname"  CssClass="txtfield"
                                                            runat="server" MaxLength="50" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                </div>
            </div>

               <div class="grid12">
                <div class="grid3 title">First Name*</div>
                <div class="grid5">
                       <asp:TextBox autocomplete="off" ID="txtFname" ReadOnly="false" runat="server" MaxLength="40"
                                                             CssClass="txtfield" ></asp:TextBox>
                </div>
                 <div class="grid3 errorright">
                           <asp:RequiredFieldValidator ID="rfvFname" runat="server" ControlToValidate="txtFname" ErrorMessage="Enter first name"
                                                        Display="Dynamic"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                 </div>
            </div>

                <div class="grid12">
                <div class="grid3 title">Last Name*</div>
                <div class="grid5">
                          <asp:TextBox autocomplete="off" ID="txtLname" ReadOnly="false" runat="server" MaxLength="40"
                                                            CssClass="txtfield" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                           <asp:RequiredFieldValidator ID="rfvLname" runat="server" ControlToValidate="txtLname"
                                                        Display="Dynamic" ErrorMessage="Enter last name"  ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
            </div>


                <div class="grid12">
                <div class="grid3 title">Address1*</div>
                <div class="grid5">
                             <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd1" CssClass="txtfield"
                                                            runat="server" MaxLength="30" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                           <asp:RequiredFieldValidator ID="rfvAdd1" ErrorMessage="Enter address" runat="server"
                                                        ControlToValidate="txtAdd1" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
            </div>    
                <div class="grid12">
                <div class="grid3 title">Address2 </div>
                <div class="grid5">
                           <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd2" CssClass="txtfield"
                                                            runat="server" MaxLength="30" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">                         
                </div>
            </div>   
            <div class="grid12">
                <div class="grid3 title">Address3 </div>
                <div class="grid5">
                            <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtAdd3" CssClass="txtfield"
                                                            runat="server" MaxLength="30" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">                         
                </div>
            </div>   

              <div class="grid12">
                <div class="grid3 title">City*</div>
                <div class="grid5">
                               <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtCity" CssClass="txtfield"
                                                            runat="server" MaxLength="30" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                          <asp:RequiredFieldValidator ID="rfvCity" ErrorMessage="Enter city" runat="server"
                                                        ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                </div>
            </div>    
              <div class="grid12">
                <div class="grid3 title">State*</div>
                <div class="grid5">
                                 <asp:TextBox ReadOnly="false" Visible="false" autocomplete="off" ID="drpState" CssClass="txtfield"
                                                            runat="server" MaxLength="20" ></asp:TextBox>
                                                        <asp:DropDownList ID="ddlstate" runat="server"  Class="txtfield"
                                                            Enabled="true">
                                                        </asp:DropDownList>
                </div>
                <div class="grid3 errorright">
                          <asp:RequiredFieldValidator ID="rfvtxtstate" ErrorMessage="Enter state"
                                                        runat="server" ControlToValidate="drpState" Display="Dynamic" ValidationGroup="Mandatory"
                                                        Class="vldRequiredSkin" Enabled="false"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvddlstate" runat="server" 
                                                Text="Select State" ErrorMessage="Required" 
                                                ControlToValidate="ddlstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                </div>
            </div>   

             <div class="grid12">
                <div class="grid3 title">Post Code / Zip*</div>
                <div class="grid5">
                                <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtZip" CssClass="txtfield"
                                                            runat="server" MaxLength="10" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                            <asp:RequiredFieldValidator ID="rfvZip" ErrorMessage="Enter zip" runat="server"
                                                        ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                        ValidChars="1234567890" TargetControlID="txtZip" />
                </div>
            </div>    

            <div class="grid12">
                <div class="grid3 title">Country</div>
                <div class="grid5">
                                   <asp:DropDownList ID="drpCountry" runat="server"  Class="txtfield"
                                                            Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                </div>
                <div class="grid3 errorright">
                           
                </div>
            </div>    

            <div class="grid12">
                <div class="grid3 title">Email *</div>
                <div class="grid5">
                                  <asp:TextBox ReadOnly="true" autocomplete="off" ID="txtAltEmail" CssClass="txtfield"
                                                            runat="server" MaxLength="55" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="rfvemail" runat="server" Class="vldRequiredSkin"
                                                        ErrorMessage="Required" ValidationGroup="Mandatory" Display="Dynamic" Text="Enter Email"
                                                        ControlToValidate="txtAltEmail">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valRegEx" runat="server" ControlToValidate="txtAltEmail"
                                                        ErrorMessage="Required" Text="Enter Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        Class="vldRegExSkin" Display="Dynamic" ValidationGroup="Mandatory"></asp:RegularExpressionValidator>
                </div>
            </div>    

                <div class="grid12">
                <div class="grid3 title">Subscribe mail </div>
                <div class="grid5">
                                  <asp:CheckBox ID="chkissubscribe" runat="server" Style="width: 10px;" TextAlign="Right" />
                </div>
                <div class="grid3 errorright">
                          
                </div>
            </div>    

                        <div class="grid12">
                <div class="grid3 title">Phone *</div>
                <div class="grid5">
                                   <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtPhone" CssClass="txtfield"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                              <asp:RequiredFieldValidator ID="rfvPhone" ErrorMessage="Enter phone" runat="server"
                                        ControlToValidate="txtPhone" Display="Dynamic" ValidationGroup="Mandatory" Class="vldRequiredSkin"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtPhone" />
                </div>
            </div>    

                <div class="grid12">
                <div class="grid3 title">Mobile</div>
                <div class="grid5">
                                   <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtMobile" CssClass="txtfield"
                                            runat="server" MaxLength="16" ></asp:TextBox>
                </div>
                <div class="grid3 errorright">
                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtMobile" />
                </div>
            </div>   
            
                <div class="grid12">
                <div class="grid3 title">Fax</div>
                <div class="grid5">
                                   <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtFax" CssClass="txtfield"
                                            runat="server" MaxLength="16" ></asp:TextBox></span>
                </div>
                <div class="grid3 errorright">
                           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterMode="ValidChars"
                                        ValidChars="1234567890" TargetControlID="txtFax" />
                </div>
            </div>  


            </div>

                  <div class="mar5 usrprofile">
                       <h5>Billing information</h5>
                       <div class="grid12">
                            <div class="grid8"> <asp:CheckBox ID="ChkBillingAdd" runat="server" Visible="true" AutoPostBack="true"
                                    Class="CheckBoxSkin" Text=" Using same communication address"  Checked="false" OnCheckedChanged="ChkBillingAdd_CheckedChanged1" /></div>
                            </div>
                        <br><br>

                                   <div class="grid12">
                <div class="grid3 title">Address1*</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd1" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox>
                                         </div>
                 <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvbAdd1" runat="server" ControlToValidate="txtbillAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address"  Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                           </div>
            </div>
                        <div class="grid12">
                <div class="grid3 title">Address2</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd2" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox>
                              </div>
                 <div class="grid3 errorright">
                          
                 </div>
            </div>
                       <div class="grid12">
                <div class="grid3 title">Address3</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbilladd3" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox>  </div>
                 <div class="grid3 errorright">
                          
                 </div>
            </div>
                      <div class="grid12">
                <div class="grid3 title">City *</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillcity" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox> </div>
                 <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvbCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                             </div>
            </div>
                        <div class="grid12">
                <div class="grid3 title">State  *</div>
                <div class="grid5">
                                    <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpBillState"
                                        runat="server" CssClass="txtfield" MaxLength="20" ></asp:TextBox>
                                    <asp:DropDownList ID="ddlbillstate" runat="server" Enabled="true"  Class="txtfield">
                                    </asp:DropDownList>
                                 </div>
                 <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="RVtxtBillstate" runat="server" ControlToValidate="drpBillState"
                                    Display="Dynamic" ErrorMessage="Enter State" Class="vldRequiredSkin" ValidationGroup="Mandatory" Enabled="false">
                                    </asp:RequiredFieldValidator>
                           <asp:RequiredFieldValidator ID="RVddlBillstate" runat="server" 
                                                Text="Select State" ErrorMessage="Required" 
                                                ControlToValidate="ddlbillstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                          </div>
            </div>
                      <div class="grid12">
                <div class="grid3 title">Post Code / Zip  *</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillzip" runat="server" CssClass="txtfield"
                                        MaxLength="10" ></asp:TextBox> </div>
                 <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvbZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtbillzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator><asp:FilteredTextBoxExtender
                                        ID="FilteredTextBoxExtender5" runat="server" FilterMode="ValidChars" ValidChars="1234567890"
                                        TargetControlID="txtbillzip" />
                           </div>
            </div>
                       <div class="grid12">
                <div class="grid3 title">Country</div>
                <div class="grid5">
                                    <asp:DropDownList ID="drpBillCountry" runat="server" Enabled="true" 
                                        Class="txtfield" AutoPostBack="True" OnSelectedIndexChanged="drpBillCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                 </div>
                 <div class="grid3 errorright">
                               </div>
            </div>
                       <div class="grid12">
                <div class="grid3 title">Phone *</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtbillphone" runat="server"
                                        CssClass="txtfield" MaxLength="16" ></asp:TextBox>
                            </div>
                 <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvbPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtbillphone" ErrorMessage="Enter phone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtbillphone" />
                             </div>
            </div>

                  </div>

              <div class="mar5 usrprofile">
                <h5>Shipping Information</h5>
                <div class="grid12">
                    <div class="grid8">  <asp:CheckBox ID="ChkShippingAdd" runat="server" Visible="true" AutoPostBack="true"
                                    Enabled="true" Class="CheckBoxSkin" Checked="false" Text=" Using same billing address"
                                    OnCheckedChanged="ChkShippingAdd_CheckedChanged" /></div>
                </div>
                </br></br>

                      <div class="grid12">
                <div class="grid3 title">Address1*</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd1" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox> </div>
                <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvsAdd1" runat="server" ControlToValidate="txtshipAdd1"
                                    Display="Dynamic" ErrorMessage="Enter address" Class="vldRequiredSkin" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                               </div>
            </div>    
                      <div class="grid12">
                <div class="grid3 title">Address2</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd2" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox> </div>
                <div class="grid3 errorright">
                           </div>
            </div>    
                        <div class="grid12">
                <div class="grid3 title">Address3</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipadd3" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox></span>
                             </div>
                <div class="grid3 errorright">   </div>
            </div>    

                        <div class="grid12">
                <div class="grid3 title">City*</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipcity" runat="server"
                                        CssClass="txtfield" MaxLength="30" ></asp:TextBox>   </div>
                <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvsCity" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipcity" ErrorMessage="Enter city" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                             </div>
            </div>   
                        <div class="grid12">
                <div class="grid3 title">State *</div>
                <div class="grid5">
                                    <asp:TextBox Visible="false" ReadOnly="false" autocomplete="off" ID="drpShipState"
                                        runat="server" CssClass="txtfield" MaxLength="20" ></asp:TextBox>
                                    <asp:DropDownList ID="ddlshipstate" runat="server" Enabled="true"  Class="txtfield">
                                    </asp:DropDownList>
                               </div>
                <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="RVshiptxtstate" runat="server" ControlToValidate="drpShipState"
                                    Display="Dynamic" ErrorMessage="Enter State" Class="vldRequiredSkin" 
                                    ValidationGroup="Mandatory" Enabled="false"></asp:RequiredFieldValidator>
                              <asp:RequiredFieldValidator ID="RVshipddlstate" runat="server" 
                               Text="Select State" ErrorMessage="Required" 
                                ControlToValidate="ddlshipstate" ValidationGroup="Mandatory" InitialValue=""
                                                ></asp:RequiredFieldValidator>
                            </div>
            </div>   
                        <div class="grid12">
                <div class="grid3 title">Post Code / Zip*</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipzip" runat="server" CssClass="txtfield"
                                        MaxLength="10" ></asp:TextBox>    </div>
                <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvsZip" Class="vldRequiredSkin" runat="server" ControlToValidate="txtshipzip"
                                    ErrorMessage="Enter zip" Display="Dynamic" ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtshipzip" />
                            </div>
            </div>    
                      <div class="grid12">
                <div class="grid3 title">Country</div>
                <div class="grid5">
                                    <asp:DropDownList ID="drpShipCountry" Enabled="true" runat="server" 
                                        Class="txtfield" AutoPostBack="True" OnSelectedIndexChanged="drpShipCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                <div class="grid3 errorright"> </div>
            </div>    
                        <div class="grid12">
                <div class="grid3 title">Phone *</div>
                <div class="grid5">
                                    <asp:TextBox ReadOnly="false" autocomplete="off" ID="txtshipphone" runat="server"
                                        CssClass="txtfield" MaxLength="16" ></asp:TextBox>
                            </div>
                <div class="grid3 errorright">
                                <asp:RequiredFieldValidator ID="rfvsPhone" Class="vldRequiredSkin" runat="server"
                                    ControlToValidate="txtshipphone" ErrorMessage="Enter phone" Display="Dynamic"
                                    ValidationGroup="Mandatory"></asp:RequiredFieldValidator>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterMode="ValidChars"
                                    ValidChars="1234567890" TargetControlID="txtshipphone" />
                           </div>
                          
            </div>    
              <div class="grid12">
               <div class="grid3 title"></div>
                 <div class="grid5"><div class="updtebtn" style="text-align:center;">
                	  <asp:Button ID="btnUpdate" Visible="true" runat="server"   text="Update"
                                OnClick="btnUpdate_Click" ValidationGroup="Mandatory" class="btn-login btn txtcolor btnadj" />
                </div></div>
                  <div class="grid3 errorright"></div>
             
                </div>
                <div class="cl"></div>
              </div>

        </div>
    


   
   <div class="cl"></div>
    </div>
</asp:Content>


