<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="OrderDet_uc" Codebehind="OrderDet.ascx.cs" %>
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
                        int Userid;
                        int ProductId;
                        decimal subtot = 0.00M;
                        decimal taxamt = 0.00M;
                        decimal Total = 0.00M;

                        string SelProductId = "";
                        string OrdStatus = "";

                        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                        UserServices objUserServices = new UserServices();

                        Userid = objHelperServices.CI(Session["USER_ID"]);
                        if (Userid <= 0)
                            Userid = objHelperServices.CI(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString());

                        string id;
                        if (Request.Url.Query.Contains("key=") == false)
                        {
                            id = Request.Url.Query.Replace("?", "");
                            id = DecryptSP(id);
                            if ((id == null))
                            {
                                id = Request.Url.Query.Replace("?", "");

                                if (id.Contains("PaySP") == false)
                                {
                                    id = id + "#####" + "PaySP";
                                }
                            }
                        }
                        else
                        {
                            id = HttpContext.Current.Session["Mchkout"].ToString();
                            id = DecryptSP(id);
                        }
                        if (id != null)
                        {
                            string[] ids = id.Split(new string[] { "#####" }, StringSplitOptions.None);

                            OrderID = objHelperServices.CI(ids[0]);
                            
                        }
                        else
                        {
                            OrderID = 0;
                          
                        }
                        
                        //if (!string.IsNullOrEmpty(Request["OrderID"]))
                        //{
                        //    OrderID = Convert.ToInt32(Request["OrderID"].ToString());
                        //}
                        //else
                        //{
                        //    OrderID = objOrderServices.GetOrderID(Userid, OpenOrdStatusID);
                        //}
                        //objerrorhandler.CreateLog(OrderID.ToString());
                        OrdStatus = objOrderServices.GetOrderStatus(OrderID);
                        ProductId = objHelperServices.CI(Request.QueryString["Pid"]);
                    %>
                   <table class="table table-bordered">
                  <thead class="checkout_thead">
                    <tr>
                      <th>Order Code</th>
                      <th>Quantity </th>
                      <th>Description </th>
                      <th>Cost(EX.GST)</th>
                      <th>Extention Amount (EX.GST)</th>
                    </tr>
                  </thead>
                          <tbody>
                         
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

                          <tr>
                    <td>   <% Response.Write(rItem["CATALOG_ITEM_NO"].ToString()); %></td>
                    <td>            <% Response.Write(rItem["QTY"].ToString()); %> </td>
                    <td>       <% Response.Write(rItem["DESCRIPTION"].ToString().Replace("<ars>g</ars>", "&rarr;"));%></td>
                    <td>     <% Response.Write(CurSymbol + " " + ProductUnitPrice.ToString("#,#0.00")); %></td>
                    <td>  <% Response.Write(CurSymbol + " " + Amt.ToString("#,#0.00")); %></td>
                  </tr>
                       
                        <%  
                               i = i + 1;
                                       
                                        
                                    } 
                                  
                                }  
                             
                          
                        %>
                         <tr class="border_none">
                   <td colspan="3"></td>
                    <td> <strong>Sub Total  </strong></td>
                    <td>  <strong>             <%    Response.Write(CurSymbol + " " + oOrderInfo.ProdTotalPrice);        %>  </strong></td>
                  </tr >
                  
            
                        <tr class="border_none">
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
                  </tr>
                     
                    <tr class="border_none">
                     <td colspan="3"></td>
                    <td>   <strong>   Total Tax Amount (GST) </strong></td>
                    <td>  <strong>          <%
                          
                            Response.Write(CurSymbol + " " + oOrderInfo.TaxAmount);         
                        %> </strong> </td>
                  </tr>
         <tr class="border_none blue_color_text">
                    <td colspan="3"></td>
                    <td>  <strong>     <%
                        if (objOrderServices.IsNativeCountry(OrderID) == 0)
                        {                                        
                            %>                                        
                               Est. Total 
                            <%
                        }
                        else
                        {
                                %> Est. Total Inc GST
                       <%
                        } %>
                       
                       </strong></td>
                    <td>      <strong>     <%
                              
                                Response.Write(CurSymbol + " " + oOrderInfo.TotalAmount);     
                            %></strong> </td>
                  </tr>
 </tbody></table>




                 
