<%@ Control Language="C#" AutoEventWireup="true" Inherits="UC_categorylist" Codebehind="categorylist.ascx.cs" %>
<%@ Register Src="newproducts.ascx" TagName="newproducts" TagPrefix="uc1" %>
<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CDN"].ToString()%>scripts/jquery-migrate-1.0.0.min.js" ></script>

<%--<link href='http://fonts.googleapis.com/css?family=Lato:400,700' rel='stylesheet' type='text/css'>--%>

<input id="HidItemPage" type="hidden" runat="server" />
<div class="container">
	<div class="row">
         <% Response.Write(Bread_Crumbs()); %>
    </div>


   <div class="row">
    	<div class="col-md-4 col-lg-4 leftsidebar pl0" id="sidebar">
        	
           
                
           <%  Response.Write(ST_Categories());  %>
            	
          
       	</div>
            
        <div class="col-md-16 rightgrid pr0" id="content">
        	<div class="row">

         <%--       <div class="main-banner">
                	<img class="img-responsive" src="/images/security.jpg"/>
                </div>--%>
            	<div class="col-md-20 col-sm-20 clearfix">
               
                       <% Response.Write(ST_CategoryList()); %>
                
                    <% 
    if (Request.Url.ToString().ToLower().Contains("ct.aspx") == true && Request.QueryString["tsb"] == null )
    {
        %>

          <%   Response.Write(ST_CategoryProductList());  %>
          <%    }
  
    %>

<div class="clearfix"></div>		
          
          
       
                </div>
                
            </div>
        </div>
    </div>

    </div>
  <% 
    if (Request.Url.ToString().ToLower().Contains("ct.aspx") == true && Request.QueryString["tsb"] == null )
    {
        %>
<div class="fixed_bottom visible-xs clearfix" id="mobpopfil" runat="server">
    <div class="fixed_filter">
        <a href="#popup_filter" class="" data-toggle="modal" data-backdrop="">FILTER</a>
    </div>
    <div class="fixed_addtocart">
        <a href="#popup_sorter" class="" data-toggle="modal" >SORT BY</a>
    </div>
</div>
<div id="popup_sorter" class="modal fade in" style="display: none;z-index:9999 !important;" aria-hidden="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 class="filter_title">Product Sorter</h3>
                </div>
                <div class="modal-body">
                    <div class="container">
                        <div class="mob_sort">
                        	<li>
                                 <a onclick="SetSortOrder('latest');" style="cursor:pointer;">Latest</a>

                        	</li>
                            <li>
                                <a onclick="SetSortOrder('ltoh');" style="cursor:pointer;">Price Low to High</a>

                            </li>
                            <li>
                                <a onclick="SetSortOrder('htol');" style="cursor:pointer;">Price High to Low</a>

                            </li>
                            <li>
                               <a onclick="SetSortOrder('popularity');" style="cursor:pointer;">Popular</a>
                            </li>
                        </div>
                    </div>
                    
                </div>

            </div>
        </div>
    </div>
 <%    }
  
    %>

<script type="text/javascript">
    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }   
 </script>

<script type="text/javascript">
    function getCookie(name) {
        var re = new RegExp(name + "=([^;]+)");
        var value = re.exec(document.cookie);
        return (value != null) ? unescape(value[1]) : null;
    }
    $(window).bind("pageshow", function () {
        // update hidden input field

        $("#" + '<%= HiddenField2.ClientID %>').val("0");
        $("#" + '<%= HiddenField1.ClientID %>').val("0");
        $("#" + '<%= hfcheckload.ClientID %>').val("0");
        var hfback = $("#" + '<%= hfback.ClientID %>').val();

        if ($.browser.msie) {
            if ($.browser.version != "11.0") {

                $("#" + '<%= HFcnt.ClientID %>').val("1");
                hfback = "0";
            }
        }
        var url = window.location.href;
      var x=  url.split("/")
     
        var cookieValue = getCookie('GLVIEWMODE');
        if (hfback == 1) {

            var hfbackdata = $("#" + '<%= hfbackdata.ClientID %>').val();



            if (cookieValue == "LV" && x==6) {
                $('#home-product .product-grid').addClass('list-wrapper');
                $('#home-product .product-grid #divdesc').removeClass('dpynone');
                $('#home-product .product-grid #divdesc').addClass('pro_discrip');
                $('#progrid').removeClass('blue_text');
                $('#prolist').addClass('blue_text');
                hfbackdata = hfbackdata.replace("product-grid", "product-grid list-wrapper");
                hfbackdata = hfbackdata.replace("dpynone", "pro_discrip");
            }
            //            else {

            //                $('#home-product .home-grid').removeClass('list-wrapper');
            //                $('#home-product .home-grid #divdesc').addClass('dpynone');
            //                $('#home-product .home-grid #divdesc').removeClass('pro_discrip');
            //                hfbackdata = hfbackdata.replace("home-grid list-wrapper", "home-grid");
            //                hfbackdata = hfbackdata.replace("pro_discrip", "dpynone");
            //            }
            $('.divLoadData:last').before(hfbackdata);



            jQuery(document).ready(function () {
                $("img.lazy").lazyload();
            });
            var scrollpos = $("#" + '<%= hfscrollpos.ClientID %>').val();
            $(window).scrollTop(scrollpos);
        }
        else {

            $("#" + '<%= HFcnt.ClientID %>').val("1");
            var pgno = $("#" + '<%= HFcnt.ClientID %>').val();
            var storedData = sessionStorage.getItem("S_VIEWMODE");

            if (storedData == "LV" && x==6) {
                $('#home-product .product-grid').addClass('list-wrapper');
                $('#home-product .product-grid #divdesc').removeClass('dpynone');
                $('#home-product .product-grid #divdesc').addClass('pro_discrip');
                $('#progrid').removeClass('blue_text');
                $('#prolist').addClass('blue_text');
            }
            else {

                $('#home-product .product-grid').removeClass('list-wrapper');
                $('#home-product .product-grid #divdesc').addClass('dpynone');
                $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
                $('#progrid').addClass('blue_text');
                $('#prolist').removeClass('blue_text');
            }
        }

    });
    $(document).ready(function () {
        function lastPostFunc() {





            $("#" + '<%= hfcheckload.ClientID %>').val("1");
            $('#tblload').toggle();
            $('#tblload').show();
            var eapath = $("#ctl00_maincontent_Categorylist1_htmleapath").val();
            var BCEAPath = $("#ctl00_maincontent_Categorylist1_htmlbceapath").val();
            var iTotalPages = $("#ctl00_maincontent_Categorylist1_htmltotalpages").val();
            var ViewMode = $("#ctl00_maincontent_Categorylist1_htmlviewmode").val();
            var irecords = $("#ctl00_maincontent_Categorylist1_htmlirecords").val();
            var balrecords = '';
            balrecords = $("#ctl00_maincontent_Categorylist1_balrecords_cnt").val();
            //alert(balrecords);
            var iTpages = parseInt(iTotalPages);
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "/ct.aspx/DynamicPag",
                data: "{'strvalue':'" + hforgurl + "','ipageno':" + hfpageno + ",'iTotalPages':" + iTpages + ",'eapath':'" + eapath + "','BCEAPath':'" + BCEAPath + "','ViewMode':'" + ViewMode + "','irecords':'" + irecords + "','balrecords':'" + balrecords + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {
                        // alert(data.d);


                        $('.divLoadData:last').before(data.d);
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $("#" + '<%= balrecords_cnt.ClientID %>').val("0");
                     
                        $("#" + '<%= hfback.ClientID %>').val("1");
                        var hfpageno1 = $("#" + '<%= HFcnt.ClientID %>').val();
                        var data1 = "";
                        balrecords = "";
                        if (hfpageno1 > 2) {
                            var data1 = $("#" + '<%= hfbackdata.ClientID %>').val();
                            data1 = data1 + data.d;


                        }
                        else {
                            data1 = data.d;
                        }

                        $("#" + '<%= hfbackdata.ClientID %>').val(data1);

                        var className = $("#home-product .product-grid").attr('class');
                        if (className.indexOf("list-wrapper") != -1) {


                            $('#home-product .product-grid').addClass('list-wrapper');
                            $('#home-product .product-grid #divdesc').removeClass('dpynone');
                            $('#home-product .product-grid #divdesc').addClass('pro_discrip');
                        }
                        else {

                        
                            $('#products .item').removeClass('list-wrapper');
                            $('#home-product .product-grid #divdesc').addClass('dpynone');
                            $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
                        }

                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });

                    }
                    else {

                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#databottom').toggle();
                        $('#databottom').show();
                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");

                }
            })
        };




        $(window).scroll(function () {

            var className = $("#home-product .product-grid").attr('class');
            if (className.indexOf("list-wrapper") != -1) {

                $('#home-product .product-grid').addClass('list-wrapper');
            }
            else {

                $('#products .item').removeClass('list-wrapper');
            }

            var x = $("#" + '<%= HiddenField1.ClientID %>').val();
            var y = $(window).scrollTop();
            var z = $(document).height() - $(window).height() - 300;
            var checkload = $("#" + '<%= hfcheckload.ClientID %>').val();
            z = z - (z / 2);
            if ($(window).scrollTop() >= z) {
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        if (checkload == "0") {
                            lastPostFunc();
                        }
                    }
                }
            }
            else if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                var hf = $("#" + '<%= HiddenField2.ClientID %>').val();
                if (hf != z) {
                    $("#" + '<%= HiddenField2.ClientID %>').val(z);
                    if (x == "0") {
                        if (checkload == "0") {
                            lastPostFunc();
                        }
                    }
                }

            }

        });
    });   
</script>
 <script type="text/javascript" language="javascript">


     function SetSortOrder(orderVal) {
         var url = window.location.href;
         $.ajax({
             type: "POST",
             url: "/GblWebMethods.aspx/SetSortOrder",
             data: "{'orderVal':'" + orderVal + "','url':'" + url + "'}",
             contentType: "application/json;charset=utf-8",
             dataType: "json",
             success: function (result) {
                 if (result.d == "1") {
                     window.location = url;
                 }
             },
             error: function (result) {
                 //rtn = false;
             }


         });
         return false;
         //OnCaptchaSuccess,
     }
    
</script>
<script language="javascript" type="text/javascript">


    function OnclickTab(Attribute) {
      
        window.location.href=Attribute;
    }
    function productbuy(buyvalue, pid) {
        //var qtyval = document.forms[0].elements[buyvalue].value.trim();
        var qtyval = "1";
      
//        var qtyval = document.getElementById(buyvalue).value.trim();
        // var qtyavail = document.forms[0].elements[buyvalue].name;
        var qtyavail = buyvalue;
        qtyavail = qtyavail.toString().split('_')[1];
        //var minordqty = document.forms[0].elements[buyvalue].name;
        var minordqty = buyvalue;
        minordqty = minordqty.toString().split('_')[2];
        //var fid = document.forms[0].elements[buyvalue].name;
        var fid = buyvalue;
        fid = fid.toString().split('_')[3];


        var orgurl = "<%=HttpContext.Current.Request.Url.Scheme.ToString()  %>" + "://" + "<%=HttpContext.Current.Request.Url.Authority.ToString()  %>" + "/";

        if (isNaN(qtyval) || qtyval == "" || qtyval <= 0 || qtyval.indexOf(".") != -1) {
            alert('Invalid Quantity!');
            window.document.forms[0].elements[buyvalue].style.borderColor = "red";
            document.forms[0].elements[buyvalue].focus();
            return false;
        }
        else {
            //window.document.location = '/OrderDetails.aspx?&amp;bulkorder=1&amp;Pid=' + pid + '&amp;Qty=' + qtyval;
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
<script type="text/javascript">
    function changeLview() {
        $('#home-product .product-grid').addClass('list-wrapper');
        $('#progrid').removeClass('blue_text');
        $('#prolist').addClass('blue_text');
        $('#home-product .product-grid #divdesc').removeClass('dpynone');
        $('#home-product .product-grid #divdesc').addClass('pro_discrip');
        window.document.cookie = "GLVIEWMODE" + "=" + "LV";
        sessionStorage.setItem("S_VIEWMODE", "LV");
        sessionStorage.setItem("ViewMode", "LV");
   
    }
    function changeGview() {
        $('#home-product .product-grid').removeClass('list-wrapper');
        $('#prolist').removeClass('blue_text');
        $('#progrid').addClass('blue_text');
        $('#home-product .product-grid #divdesc').removeClass('pro_discrip');
        $('#home-product .product-grid #divdesc').addClass('dpynone');
        window.document.cookie = "GLVIEWMODE" + "=" + "GV";
        sessionStorage.setItem("S_VIEWMODE", "GV");
        sessionStorage.setItem("ViewMode", "GV");
     
       
      
    }

 

</script>
  <asp:HiddenField ID="HiddenField2" runat="server" /><asp:HiddenField ID="HiddenField1" runat="server" /><asp:HiddenField ID="HFcnt" runat="server" />
       <asp:HiddenField ID="hfcheckload" runat="server" />
       <asp:HiddenField ID="hfnewurl" runat="server" />
       <asp:HiddenField ID="hforgurl" runat="server" />
        <asp:HiddenField ID="HFCompPrProd" runat="server" />
        <asp:HiddenField ID="hfback" runat="server" />
         <asp:HiddenField ID="hfbackdata" runat="server" />
           <asp:HiddenField ID="hfcheckex" runat="server" />
           <asp:HiddenField ID="hfscrollpos" runat="server" />
           <asp:HiddenField ID="balrecords_cnt" runat="server" />
           <asp:HiddenField ID="currpage" runat="server" />
               <input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>
                     <input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
                      <input type="text" name="bceapath" id="htmlbceapath" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpages" runat="server" style="display:none;"/>







   
