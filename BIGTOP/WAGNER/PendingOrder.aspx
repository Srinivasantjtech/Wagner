<%@ Page Title="" Language="C#" MasterPageFile="~/Mainpage.master" AutoEventWireup="true" Inherits="PendingOrder" Codebehind="PendingOrder.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace ="TradingBell.WebCat.Helpers" %>
<%@ Import Namespace ="TradingBell.WebCat.CatalogDB" %>
<%@ Import Namespace ="TradingBell.WebCat.CommonServices" %>
<asp:Content ID="Content4" ContentPlaceHolderID="maincontent" runat="Server">

            <script type="text/javascript">
                function DeleteConfirm(Order, Createdby, OrderDate,CustOrderNo) {
                    var agree = confirm("Are you sure want to Delete this order?\nOrder No : " + CustOrderNo + "\nOrder Date : " + OrderDate + "\nCreated By : " + Createdby);
                    //var agree = confirm("Are you sure want to Delete this order?\nOrder Date : " + OrderDate + "\nCreated By : " + Createdby);
                    if (agree) {
                        //document.getElementById('HiddenField2').value = "Deleted";
                        //location.href("PendingOrder.aspx?OrderId=" + Order + "&Act=D");
                        var myUrl = "PendingOrder.aspx?OrderId=" + Order + "&Act=D";
                        window.location.href = myUrl;
                        return true;
                    }
                    //        else {
                    //            document.getElementById('HiddenField2').value = "Cancel";
                    //            return false;
                    //        }
                }
            </script>
        
<table align="left"> <caption> <br /> <tr> <td> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td> <td align="left" class="tx_1"> <font color="#333333" face="sans-serif" size="4"> <asp:Label ID="Label1" runat="server">&nbsp;Pending Orders Waiting Approval</asp:Label> </font> </td> </tr> </caption> </table>
<br /><br /><br /><br />
            <table align="center" width="780">
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="center">
                        <% 
                            OrderServices objOrderServices = new OrderServices();
                            DataTable oDt = objOrderServices.PendingOrders();
                            if (oDt != null)
                            { %>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="border-width: 1px;
                            border-style: solid; border-color: #c6d3de">
                            <tr style="height: 20px">
                            <% if (System.Convert.ToInt16(Session["USER_ROLE"]) < 3)
                                 { %>
                                <td style="background-color: #c6d3de; width: 12%;" align="center">
                                    User
                                </td>
                                <td style="background-color: #c6d3de; width: 17%;" align="center">
                                    Order Date
                                </td>
                                <td style="background-color: #c6d3de; width: 15%;" align="center">
                                    Cust.Order No
                                </td>
                                <td width="0%" colspan="6" style="background-color: #c6d3de" align="left">
                                    Order Action
                                </td>
                                <%} %>
                                <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                 { %>
                                <td style="background-color: #c6d3de; width: 12%;" align="center">
                                    User
                                </td>
                                <td style="background-color: #c6d3de; width: 17%;" align="center">
                                    Order Date
                                </td>
                                <td style="background-color: #c6d3de; width: 15%;" align="center">
                                    Cust.Order No
                                </td>
                                <td width="0%" colspan="6" style="background-color: #c6d3de" align="left">
                                    Order Action
                                </td>
                                <%} %>
                            </tr>
                            <%  string bgcolor = "";
                                bool bodrow = true;
                                foreach (DataRow oDr in oDt.Rows)
                                {
                                    bgcolor = bodrow ? "white" : "#e7efef";
                                    bodrow = !bodrow;
                            %>
                           
                            <tr>
                            <% if (System.Convert.ToInt16(Session["USER_ROLE"]) < 3)
                             { %>
                                <td align="center" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["CONTACT"].ToString()%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 17%;">
                                    <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["CREATED_DATE"])%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 15%;">
                                    <%=oDr["Cust.Order No"]%>
                                </td>
                               
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Approve_Order.jpg" height="18px" width="17px" alt="" align="middle" /><a
                                        href="/checkout.aspx?<% = oDr["Order"] %>" style="text-decoration: none;
                                        border-bottom: 1px solid none"><font color="#3399FF">&nbsp Approve Order </font>
                                    </a>
                                </td>
                              
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/View_edit_order.jpg" height="17px" width="16px" alt="" align="middle" /><a
                                        href="/OrderDetails.aspx?ORDER_ID=<% =EncryptSP( oDr["Order"].ToString()) %>&bulkorder=1&ViewOrder=View"
                                        style="text-decoration: none; border-bottom: 1px solid none"> <font color="#3399FF">
                                            &nbsp View / Edit Order </font></a>
                                </td>
                                <td align="left" style="background-color: <%=bgcolor%>">
                                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Delete_order.jpg" height="17px" width="16px" alt="" align="middle" />
                                </td>
                                <td width="16%" align="left" style="background-color: <%=bgcolor%>">
                                    <%--     <asp:HyperLink runat="server" ID="HyperLink1" Style="color: #3399FF" NavigateUrl="~/OrderDetails.aspx?OrdId={0}">&nbsp; Delete Order</asp:HyperLink>--%>
                                    <a href="javascript:DeleteConfirm('<%=oDr["ORDER"]%>','<%=oDr["CONTACT"]%>','<%=oDr["CREATED_DATE"]%>','<%=oDr["Cust.Order No"]%>')"
                                        style="text-decoration: none; border-bottom: 1px solid none">&nbsp <font color="#3399FF">
                                            Delete Order</font></a>
                                </td>
                                 <%} %>
                                  <% if (System.Convert.ToInt16(Session["USER_ROLE"]) == 3)
                                 { %>
                                  <td align="center" style="background-color: <%=bgcolor%>; height: 20px; width: 12%;">
                                    <%=oDr["CONTACT"].ToString()%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 17%;">
                                    <%=string.Format("{0:dd/MM/yyyy hh:mm:ss}", oDr["CREATED_DATE"])%>
                                </td>
                                <td align="center" style="background-color: <%=bgcolor%>; width: 15%;">
                                    <%=oDr["Cust.Order No"]%>
                                </td>
                               
                            
                              
                                <td style="background-color: <%=bgcolor%>" align="left">
                                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/View_edit_order.jpg" height="17px" width="16px" alt="" align="middle" /><a
                                        href="/OrderDetails.aspx?ORDER_ID=<% = oDr["Order"] %>&bulkorder=1&ViewOrder=View"
                                        style="text-decoration: none; border-bottom: 1px solid none"> <font color="#3399FF">
                                            &nbsp View / Edit Order </font></a>
                                </td>
                                <td align="left" style="background-color: <%=bgcolor%>">
                                    <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>images/Delete_order.jpg" height="17px" width="16px" alt="" align="middle" />
                                </td>
                                <td width="16%" align="left" style="background-color: <%=bgcolor%>">
                                    <%--     <asp:HyperLink runat="server" ID="HyperLink1" Style="color: #3399FF" NavigateUrl="~/OrderDetails.aspx?OrdId={0}">&nbsp; Delete Order</asp:HyperLink>--%>
                                    <a href="javascript:DeleteConfirm('<%=oDr["ORDER"]%>','<%=oDr["CONTACT"]%>','<%=oDr["CREATED_DATE"]%>','<%=oDr["Cust.Order No"]%>')"
                                        style="text-decoration: none; border-bottom: 1px solid none">&nbsp <font color="#3399FF">
                                            Delete Order</font></a>
                                </td>

                                 
                                  <%} %>
                            </tr>
                            <%} %>
                        </table>
                        <%} %>
                    </td></tr></table>
           
</asp:Content>
