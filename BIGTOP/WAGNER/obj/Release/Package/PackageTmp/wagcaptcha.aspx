<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wagcaptcha.aspx.cs" Inherits="wagcaptcha" %>
<%@ Register Assembly="CustomCaptcha" Namespace="CustomCaptcha" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:CaptchaControl ID="cVerify" CaptchaChars="23456789" CaptchaLength="4" CaptchaBackgroundNoise="Low" runat="server" OnPreRender="cVerify_preRender"  style="display:none;"  />
    </div>
    </form>
</body>
</html>
