<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendNotificationMsg.aspx.cs" Inherits="Wiretek.SendNotificationMsg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
<script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/jquery-1.11.1.js" type="text/javascript"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/encryption/helpers.js"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/encryption/hmac.js"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/encryption/hkdf.js"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/encryption/encryption-factory.js"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/libs/snippets.js"></script>
  <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/libs/idb-keyval.js"></script>
     <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/app-controller.js"></script>
<%-- <script src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>Scripts/notification_send.js"></script>--%>
    <script language="javascript" type="text/javascript">

        function SendNotification() {
          var hfsubs = $("#" + '<%= HiddenField1.ClientID %>').val();
           // var hfsubs = '{"endpoint":"https://fcm.googleapis.com/fcm/send/edIjElzx6Gw:APA91bHbusfnZ6FMAcBFL3DDmzb…JwzzLQ1u6Zyq6MI_Qsj3ZEvRIo6Jk382kt7M67mw--w51r3XzlezyanCxH-oD0rX6NplS61kd9","keys":{"p256dh":"BDstYU2zOIEtX9HFoeScWOcaUNZCwmt7-DpxY_lbUccgE_c-R9k4Z1V04XyBeCpBxJwuSXI65sNqPUPfLzsc3Xc=","auth":"-3UY261GcWtHAb1KWoOdRQ=="}}';
             var hfendpoint = $("#" + '<%= HiddenField2.ClientID %>').val();
           // var hfendpoint = "https://fcm.googleapis.com/fcm/send/edIjElzx6Gw:APA91bHbusfnZ6FMAcBFL3DDmzbBskxzf52N1NPY0opk7HT__YAO6qZjpZ9KypGmxDJwzzLQ1u6Zyq6MI_Qsj3ZEvRIo6Jk382kt7M67mw--w51r3XzlezyanCxH-oD0rX6NplS61kd9";
            var hfmessage = $("#" + '<%= HiddenField3.ClientID %>').val();
           // var myendpoint = getcurrentsub();
          
            var myendpoint = "https://fcm.googleapis.com/fcm/send/fU3RQwAAGig:APA91bG4vvm9eML4_QJAtuW0bXWUCyLOC2fHFGqXeUavJAeOUAZQrqqtla1KOzjjXNthdPweETVWSWmn204U4gOM3Ovhpgbh6p9ML3JZqv28DJdJWIGyLMHifUDKB4LmbxWgXam_16SX";
      
           sendPushMessage(myendpoint, hfsubs,
    hfmessage, hfendpoint);
         
            return false;
     }
            </script>
</head>
<body>
    <form id="form1" runat="server" style="font-family: sans-serif">
        <asp:HiddenField ID="HiddenField1" runat="server" />
          <asp:HiddenField ID="HiddenField2" runat="server" />
         <asp:HiddenField ID="HiddenField3" runat="server" />
    <div>
        <table style="width:100%">
            <h2>Send Notification Details</h2>
            <tr>
                <td style="width:30%">
                    <asp:Label ID="Label3" runat="server" Text="Notifucation Text:"></asp:Label> 
                </td>
                <td>
                     <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td style="width:30%">
                      <asp:Label ID="Label4" runat="server" Text="Notification URL:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    
       
          
      
    </div>
    </form>
</body>
</html>
