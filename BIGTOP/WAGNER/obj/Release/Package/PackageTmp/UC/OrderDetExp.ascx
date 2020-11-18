<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderDetExp.ascx.cs" Inherits="OrderDetExp_uc" %>
<%@ Import Namespace="System.Data" %>
<%--<%@ Import Namespace="TradingBell.Common" %>
<%@ Import Namespace="TradingBell.WebServices" %>--%>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>


             <%
                       HelperServices objHelperServices = new HelperServices();
                       ErrorHandler objerrorhandler = new ErrorHandler();
                        OrderServices objOrderServices = new OrderServices();
                        ProductServices objProductServices = new ProductServices();
                        //ProductFamily oProdFam = new ProductFamily();
                        DataSet dsOItem = new DataSet();

                        int OrderID = 0;
                        int Userid=0;
                        int ProductId;
                        decimal subtot = 0.00M;
                        decimal taxamt = 0.00M;
                        decimal Total = 0.00M;

                        string SelProductId = "";
                        string OrdStatus = "";

                        //int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                      UserServices objUserServices = new UserServices();
                      if (Session["OrderDetExp_orderid"] != null)
                      {
                          OrderID = objHelperServices.CI(Session["OrderDetExp_orderid"].ToString());
                          Userid = objHelperServices.CI(Session["OrderDetExp_userid"].ToString());
                      }
                        //Userid = objHelperServices.CI(Session["USER_ID"]);
                        //if (Userid <= 0)
                        //    Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());

                        //string id;
                        //if (Request.Url.Query.Contains("key=") == false)
                        //{
                        //    id = Request.Url.Query.Replace("?", "");
                        //    id = DecryptSP(id);
                        //    if ((id == null))
                        //    {
                        //        id = Request.Url.Query.Replace("?", "");

                        //        if (id.Contains("PaySP") == false)
                        //        {
                        //            id = id + "#####" + "PaySP";
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    id = HttpContext.Current.Session["Mchkout"].ToString();
                        //    id = DecryptSP(id);
                        //}
                        //if (id != null)
                        //{
                        //    string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);
                           
                        //    if (Userid != 999)
                        //    {
                        //        if (ids.Length > 0)
                        //        {
                        //            OrderID =  objHelperServices.CI(ids[0]);
                        //        }
                        //        else if (Session["ORDER_ID"] != null)
                        //        {
                        //            OrderID = objHelperServices.CI( Session["ORDER_ID"]);
                                
                        //        }
                        //        //string ChkCCoucode = objUserServices.GetUserBillCountryCode_EC(Userid);
                        //        //int tmpOrdStatus=0;
                        //        //if (ChkCCoucode.ToLower().ToString() == "australia")
                        //        //{
                        //        //    tmpOrdStatus = (int)OrderServices.OrderStatus.OPEN; ;
                        //        //    OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
                        //        //}
                        //        //else
                        //        //{
                        //        //    tmpOrdStatus = (int)OrderServices.OrderStatus.Intl_Waiting_Verification;
                        //        //    OrderID = objOrderServices.GetOrderID(Userid, tmpOrdStatus);
                        //        //    //Session["ORDER_ID"] = "0"; ;
                        //        //    //Session["USER_ID"] = "999";
                        //        //    //Session["EXPRESS_CHECKOUT"] = "True";
                                   

                        //        //    //Session.RemoveAll();
                        //        //   /// Session.Clear();
                        //        //   // Session.Abandon();
                        //        //    Session["USER_ID"] = "";
                        //        //    Session["DUMMY_FLAG"] = "0";
                        //        //    Session["ORDER_ID"] = "0";
                        //        //    Session["ExpressLevel"] = "";
                        //        //    //Session["USER_ROLE"] = "0";
                        //        //}
                        //    }
                        //    else
                        //    {
                        //        OrderID = objHelperServices.CI(ids[0]);
                        //    }
                            
                        //}
                        //else
                        //{
                        //    OrderID = 0;
                          
                        //}
                        
                        //if (!string.IsNullOrEmpty(Request["OrderID"]))
                        //{
                        //    OrderID = Convert.ToInt32(Request["OrderID"].ToString());                                                                                       rderServices.GetOrderID(Userid, OpenOrdStatusID);
                        //}
                       // objerrorhandler.CreateLog(OrderID.ToString());
                        OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                        ProductId = objHelperServices.CI(Request.QueryString["Pid"]);
                    %>
              <div class="brb">
                    	<div class="pv5 ph10" style="background:#f0f0f0;">
                    		<h4 class="heading_2 inlineblk">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                Order Sumary</h4>
                        	<%--<a href="#" class="pull-right mt5">Edit Cart</a>--%>
                        </div>
                   <%   
                                          	     
                            dsOItem = objOrderServices.GetOrderItems(OrderID);

                            OrderServices.OrderInfo oOrderInfo = new OrderServices.OrderInfo();
                            oOrderInfo = objOrderServices.GetOrder(OrderID);
                    

                            UserServices.UserInfo oOrdBillInfo1 = objUserServices.GetUserBillInfo(Userid);
                            UserServices.UserInfo oOrdShippInfo1 = objUserServices.GetUserShipInfo(Userid);


                            string CurSymbol = objHelperServices.GetOptionValues("CURRENCYFORMAT").ToString();
                            decimal ProdShippCost = 0.00M;
                            decimal TotalShipCost = 0.00M;
                            string catlogitem = "";
                            SelProductId = "";
                           
                                if (dsOItem != null)
                                {
                                    int i = 0;
                                    foreach (DataRow rItem in dsOItem.Tables[0].Rows)
                                    {
                                        decimal ProductUnitPrice;
                                        int pid;
                                        decimal Amt = 0;

                                        pid = objHelperServices.CI(rItem["PRODUCT_ID"].ToString());
                                        catlogitem = rItem["CATALOG_ITEM_NO"].ToString();
                                        //ProductUnitPrice = oHelper.CDEC(oProd.GetProductBasePrice(pid));
                                        ProductUnitPrice = objHelperServices.CDEC(rItem["PRICE_EXT_APPLIED"].ToString());
                                        ProductUnitPrice = objHelperServices.CDEC(ProductUnitPrice.ToString("N2"));
                                        Amt = objHelperServices.CDEC(objHelperServices.CI(rItem["QTY"].ToString()) * ProductUnitPrice); 
                                      
                        %>

<%--                          <tr>
                    <td>   <% Response.Write(rItem["CATALOG_ITEM_NO"].ToString()); %></td>
                    <td>            <% Response.Write(rItem["QTY"].ToString()); %> </td>
                    <td>       <% Response.Write(rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;"));%></td>
                    <td>     <% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %></td>
                    <td>  <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %></td>
                  </tr>--%>


                   <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-3">
                            	<%--<img class="img_h50" src="images/BANANA-SOCKETBIND.jpg">--%>
                                 <% GetImage(pid); %>
                                 <asp:Image ID="lblProImage" class="img_h50" Style="max-width: 60px;" runat="server" alt="Loading..."  />
                            </div>
                            <div class="col-xs-17 pr0 ">
                            	<div class="dblock pb10"><b class="f14"><% Response.Write(rItem["DESCRIPTION"].ToString()); %></b></div>
                                <div class="os_itemdetail">
                                	<div class="col-xs-6 pl0"> <span class="dblock pb5">Qty:</span> <% Response.Write(rItem["QTY"].ToString()); %> </div>
                                    <div class="col-xs-7"> <span class="dblock pb5">Item Cost:</span><% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %> Ex GST</div>
                                    <div class="col-xs-7 text-right"> <span class="dblock pb5">Sub-Total:</span> <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %> Ex GST</div>
                                </div>
                            </div>
                        </div>
                       
                        <%  
                               i = i + 1;
                                       
                                        
                                    } 
                                  
                                }  
                             
                          
                        %>
                        <%-- <tr class="border_none">
                   <td colspan="3"></td>
                    <td> <strong>Sub Total  </strong></td>
                    <td>  <strong>             <%    Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);        %>  </strong></td>
                  </tr >--%>
                  

                  <div class="clearfix br_b pb15 pt25">
                        	<div class="col-xs-16">
                            	<%--<b>Delivery</b>--%>
                        		<p class="mb0 mt10">
                                    <%--Click and Collect - Customer pick form store --%>
                                  <%  
                      if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                          Response.Write("Shipping Charge");
                      else
                          Response.Write("Delivery (Ex GST)");
                                 %>
                        		</p>
                            </div>
                            <div class="col-xs-4 text-right ital">
                            	<%--Free--%>
                                <%
                                                 if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                                                 {
                                                     //if (oOrderInfo.ShipCost > 0)
                                                     //   Response.Write(CurSymbol + " " + oOrderInfo.ShipCost); 
                                                     //else
                                                        Response.Write("To Be Advised");
                                                 }
                                                 else
                                                     Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);         
                            
                        %>
                            </div>
                        </div>
            
                        <%--<tr class="border_none">
                   <td colspan="3"></td>
                    <td> <strong> <%  
                      if (objOrderServices.IsNativeCountry(OrderID) == 0)
                          Response.Write("Shipping Charge");
                      else
                          Response.Write("Delivery (Ex GST)");
                                 %>
                                </strong>  </td>
                    <td>  <strong>           <%
                                                 if (objOrderServices.IsNativeCountry(OrderID) == 0)
                                                 {
                                                     if (oOrderInfo.ShipCost > 0)
                                                        Response.Write(CurSymbol + " " + oOrderInfo.ShipCost); 
                                                     else
                                                        Response.Write("To Be Advised");
                                                 }
                                                 else
                                                     Response.Write(CurSymbol + " " + oOrderInfo.ShipCost);         
                            
                        %></strong> </td>
                  </tr>--%>
                     
                   <div class="clearfix br_b pb15 pt25">
                       <div class="col-xs-16">
                            	Total Tax Amount (GST)

                          <p class="mb0 mt10">   <%
                        if (objOrderServices.IsNativeCountry_Express(OrderID) == 0)
                        {                                        
                            %>                                        
                              <b> Est. Total  </b>
                            <%
                        }
                        else
                        {
                                %><b> Est. Total Inc GST </b>
                       <%
                        } %></p>
                   
                         </div>
                        <div class="col-xs-4 text-right">
                            
                        <%   if (objOrderServices.IsNativeCountry_Express(OrderID) == 1)
                             {
                                 Response.Write(CurSymbol + " " + oOrderInfo.TaxAmount);
                             }      
                        %> 
                               

                             <p class="mb0 mt10">
                                 <b>
                          <%
                              
                                Response.Write(CurSymbol + " " + oOrderInfo.TotalAmount);     
                            %>
                             </b>
                            </p>
                        </div>
                    </div>

                  </div>
                         
                       
 






