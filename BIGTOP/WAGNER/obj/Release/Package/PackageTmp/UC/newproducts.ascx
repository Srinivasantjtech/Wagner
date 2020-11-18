<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_newpeoducts" Codebehind="newproducts.ascx.cs" %>

<%--<script type="text/javascript">

    $(window).bind("pageshow", function () {
        // update hidden input field

        if (document.body.innerHTML.toString().indexOf('LoadNewProducts') > -1) {
        
        }
        else {

          $("#" + '<%= hfcheckload.ClientID %>').val("0");
        }
      
    });
    $(window).bind('beforeunload', function () {
        $("#" + '<%= hfback.ClientID %>').val(1);
     });
    $(document).ready(function () {


       

        function lastPostFunc() {
           
            $('#tblload').toggle();
            $('#tblload').show();
           

            $.ajax({
                type: "POST",
                url: "/home.aspx/ST_Newproduct",
                data: "{'newprod':'newproduct'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                     
                        $('.divLoadData:last').before(data.d);
                     
                        $('#tblload').hide();
                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });
                    }
                    else {

                  
                        $('#data').toggle();
                        $('#data').show();
                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err);
                }
            })
        };
        $(window).scroll(function () {
         
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();

            var checkback = $("#" + '<%= hfback.ClientID %>').val();
            if ((checkload == "0") || (checkback=="1")) {
            if ($(window).scrollTop() >=400) {
           
                 
                $("#" + '<%= hfback.ClientID %>').val(0);
                            lastPostFunc();
                            $("#" + '<%= hfcheckload.ClientID %>').val(1);


                        }
                    }
              
            
        });
    });
</script>--%>

<script language="javascript" type="text/javascript">
    function productbuy(buyvalue, pid) {
        var qtyval = 1
        var fid = buyvalue;
        fid = fid.toString().split('_')[3];

        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";

       
        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 ) {
            alert('Invalid Quantity!');
            
            return false;
        }
        else {
            // window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval;
            CallProductPopup(orgurl, buyvalue, pid, qtyval, 0, fid);
        }
    }
    function keyct(e) {        
        var keyCode = (e.keyCode ? e.keyCode : e.which);
        if (keyCode == 8 || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105)) {

        }
        else {
            e.preventDefault();
        }
    }
</script>
<div class="container">
   <%=ST_Newproduct()%>
  
          <div class="clearfix"></div>



     <%=ST_PopularProduct()%>


      <%--<div class="divLoadData"></div>
  <div class="text-center" id="tblload" style="display:none">
    <img src="/images/MicroSiteimages/bigLoader.gif" class="margin_bottom_15" alt=""/>
    <br>Loading New Products.. Please Wait..</br>
  </div>--%>
<%--<%=ST_RecentProduct()%>--%>
    </div>
 <asp:HiddenField ID="hfcheckload" runat="server"  value="0"/>
   <asp:HiddenField ID="hfback" runat="server" value="1"/>