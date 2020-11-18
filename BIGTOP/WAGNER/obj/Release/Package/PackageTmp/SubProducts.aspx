<%@ Page Language="C#" AutoEventWireup="true" Inherits="SubProducts" Codebehind="SubProducts.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"><head id="Head1" runat="server">   <title></title>    
</head><body> <form id="form1" runat="server"> <table  width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td >
 <h3 class="titleblue"><b>Order Clarification/Errors</b> </h3><table cellpadding="5" width="100%" cellspacing="0"> <tr><td colspan="4">
 Product Item Clarification: <font color="red"><strong><%=Request["Item"] %></strong></font></td></tr><tr><td colspan="4">Please select an item from below to update order with:</td></tr><tr><td class="tx_bohead1" width="10%">&nbsp;</td><td class="tx_bohead1" width="15%">Code</td>
 <td class="tx_bohead1" width="40%"> Description</td><td class="tx_bohead1" width="35%">Add to Cart</td> </tr><%=GetProducts() %></table></td></tr> </table> </form></body></html>
