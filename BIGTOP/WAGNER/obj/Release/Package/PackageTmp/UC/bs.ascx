<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="bs.ascx.cs" Inherits="WES.UC.bs" %>
<input id="HidItemPage" type="hidden" runat="server" />
<input type="hidden" id="hdnFamilyId" runat="server" />
<input type="hidden" id="__EVENTTARGET" name="__EVENTTARGET" value="" runat="server" />
<input type="hidden" id="__EVENTARGUMENT" name="__EVENTARGUMENT" value="" runat="server" />


<script type="text/javascript">
    function OnclickTab(Attribute) {

        var elem = document.getElementById(Attribute);
        var defValue = elem.value;

        document.getElementById("hfclickedattr").value = defValue;

        document.forms[0].submit();
        //        var ur = document.getElementById("ctl00_maincontent_Productlist1_hfnewurl");
        //        alert(ur);
        //        document.getElementById("ctl00_MainContent_Productlist1_hfattr").value=defValue;
        //        var ul = document.getElementById("ctl00_MainContent_Productlist1_hfattr").value;
        //        alert(ul);
    }
    //    function OnclickTab_drp(Attribute) {
    //        alert(Attribute);
    //        var elem = document.getElementById(Attribute);
    //        var defValue = elem.value;

    //        document.getElementById("hfclickedattr").value = defValue;
    //        $.ajax({
    //            type: "POST",
    //            url: "/mpl.aspx/Assignds",
    //            data: "{'strvalue':'" + defValue + "'}",
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (data) {
    //                if (data.d != "") {
    //                    var ur = document.getElementById("ctl00_maincontent_Productlist1_hfnewurl");
    //                    alert(ur);
    //                    window.location.href = ur;
    //                }
    //            },
    //            error: function (xhr, status, error) {

    //                var err = eval("(" + xhr.responseText + ")");
    //            }
    //        })



    //    };




    function __doPostBack1(eventTarget, eventArgument) {
        document.getElementById("<%=__EVENTTARGET.ClientID%>").value = eventTarget;
        document.getElementById("<%=__EVENTARGUMENT.ClientID%>").value = eventArgument;
        document.forms[0].submit();
    }
    window.onbeforeunload = function (e) {
        var scrollpos = $(window).scrollTop();
        $("#" + '<%= hfscrollpos.ClientID %>').val(scrollpos);
    }


</script>


<%--<script type="text/javascript" src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>scripts/jquery-1.4.1.min.js"></script>--%>
<script type="text/javascript">
    //    $(window).bind("pageshow", function () {
    //        // update hidden input field

    //        $("#" + '<%= HiddenField2.ClientID %>').val("0");
    //        $("#" + '<%= HiddenField1.ClientID %>').val("0");
    //        $("#" + '<%= hfcheckload.ClientID %>').val("0");
    //        $("#" + '<%= HFcnt.ClientID %>').val("1");

    //    });

    $(document).ready(function () {




        $('#tblload').hide();
        $(window).bind("pageshow", function () {
            // update hidden input field

            $("#" + '<%= HiddenField2.ClientID %>').val("0");
            $("#" + '<%= HiddenField1.ClientID %>').val("0");
            $("#" + '<%= hfcheckload.ClientID %>').val("0");

            // update hidden input field
            var hfback = $("#" + '<%= hfback.ClientID %>').val();
            if ($.browser.msie) {
                if ($.browser.version != "11.0") {

                    $("#" + '<%= HFcnt.ClientID %>').val("1");
                    hfback = "0";
                }
            }
            if (hfback == 1) {
                var hfbackdata = $("#" + '<%= hfbackdata.ClientID %>').val();

                $('.divLoadData:last').before(hfbackdata);



                jQuery(document).ready(function () {
                    $("img.lazy").lazyload();
                });
                jQuery(document).ready(function () {
                    $('.def-html').darkTooltip({
                        opacity: 1,
                        gravity: 'south'
                    });
                });
                var scrollpos = $("#" + '<%= hfscrollpos.ClientID %>').val();
                $(window).scrollTop(scrollpos);
            }
            else {

                $("#" + '<%= HFcnt.ClientID %>').val("1");
                var pgno = $("#" + '<%= HFcnt.ClientID %>').val();


            }

        });



        function lastPostFunc() {
            $("#" + '<%= hfcheckload.ClientID %>').val("1");

            $('#tblload').toggle();


            $('#tblload').show();

            var eapath = $("#ctl00_MainContent_Productlist1_htmleapath").val();
            var BCEAPath = $("#ctl00_MainContent_Productlist1_htmlbceapath").val();
            var iTotalPages = $("#ctl00_MainContent_Productlist1_htmltotalpages").val();
            var BCEAPath = $("#ctl00_MainContent_Productlist1_htmlbceapath").val();
            var ViewMode = $("#ctl00_MainContent_Productlist1_htmlviewmode").val();
            var irecords = $("#ctl00_MainContent_Productlist1_htmlirecords").val();
            var balrecords = '';
            balrecords = $("#ctl00_MainContent_Productlist1_balrecords_cnt").val();

            var iTpages = parseInt(iTotalPages);
            var hforgurl = $("#" + '<%= hforgurl.ClientID %>').val();
            var hfpageno = $("#" + '<%= HFcnt.ClientID %>').val();
            hfpageno = parseInt(hfpageno) + 1;
            $("#" + '<%= HFcnt.ClientID %>').val(hfpageno);
            $.ajax({
                type: "POST",
                url: "/mpl.aspx/DynamicPag",
                data: "{'strvalue':'" + hforgurl + "','ipageno':" + hfpageno + ",'iTotalPages':" + iTpages + ",'eapath':'" + eapath + "','BCEAPath':'" + BCEAPath + "','ViewMode':'" + ViewMode + "','irecords':'" + irecords + "','balrecords':'" + balrecords + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d != "") {

                        $('.divLoadData:last').before(data.d);
                        $('#tblload').hide();
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        // $("#ctl00_maincontent_Productlist1_searchrsltproductfamily1_balrecords_cnt").val("0");



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

                        jQuery(document).ready(function () {
                            $("img.lazy").lazyload();
                        });
                        jQuery(document).ready(function () {
                            $('.def-html').darkTooltip({
                                opacity: 1,
                                gravity: 'south'
                            });
                        });
                    }
                    else {

                        $("#" + '<%= HiddenField1.ClientID %>').val("1");
                        $("#" + '<%= hfcheckload.ClientID %>').val("0");
                        $('#tblload').hide();
                        $('#data').toggle();
                        $('#data').show();
                    }
                    $('#divPostsLoader').empty();
                },
                error: function (xhr, status, error) {

                    var err = eval("(" + xhr.responseText + ")");
                }
            })



        };
        //        $(window).scroll(function () {
        //            if ($(window).scrollTop() >= $(document).height() - $(window).height() - 300) {

        //                if (scrolldown == true || scrolldown == null || scrolldown == 'undefined')
        //                    alert('df');
        //            }
        //        });
        $(window).scroll(function () {


            var className = $("#products .item").attr('class');
            if (className.indexOf("list-group-item") != -1) {

                $('#products .item').addClass('list-group-item');
                $('#grid').removeClass('yellow');
                $('#list').addClass('yellow');

            }
            else {

                $('#products .item').removeClass('list-group-item');
                $('#list').removeClass('yellow');
                $('#grid').addClass('yellow');
            }
            var varloadloader = $("#ctl00_MainContent_Productlist1_loadloader").val();


            if (varloadloader == "true") {
                var x = $("#" + '<%= HiddenField1.ClientID %>').val();
                var y = $(window).scrollTop();
                //var z = $('#tblload').position().top;
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
                else if ($(window).scrollTop() > 100 && $(window).scrollTop() == $(document).height() - $(window).height()) {
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
        }
        });
    });
</script>


<%

    string st_productList = ST_ProductListjson();
    if (st_productList != null && st_productList != string.Empty)
    {
        Response.Write( st_productList);
        loadloader.Value = "true";
    }
    else
        if (Request.Url.OriginalString.ToString().ToUpper().Contains("PL.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BB.ASPX") || Request.Url.OriginalString.ToString().ToUpper().Contains("BYPRODUCT.ASPX"))
        {
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">Right now no products for sale from this category.</div>");
            loadloader.Value = "false";
        }
        else
        {
            Response.Write("<br/><div align=\"center\" class=\"tx_13\">No Products were found that match your selection.</div>");
            loadloader.Value = "false";
        }
            %>
 
<%--   <table id="tblload" style="display: none;" width="760px">
                                <tr>
                                    <td align="center">
                                        <div  style="width:300px;" align="center">
                                            <img src="<%=System.Configuration.ConfigurationManager.AppSettings["CurrentUrl"].ToString()%>images/bigLoader.gif"
                                                width="12%" height="12%" alt=""/></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label1" runat="server" Text="LOADING DATA...PLEASE WAIT" Font-Bold="True"
                                            Font-Names="Arial" Font-Size="X-Small"></asp:Label>
                                    </td>
                                </tr>
                            </table>--%>

       <table id="data" width="100%" bgcolor="#f2f2f2" border="0" cellspacing="0" cellpadding="0"
                style="display: none;" class="box2">
                <tr>
                    <td height="35" width="156" align="left">
                        <div class="listingmenu">
                        </div>
                    </td>
                    <td width="303" align="middle">
                        <div class="listingmenu" style="width: 232px;">
                        </div>
                    </td>
                    <td width="281" align="right">
                        <div class="listingmenu push_right listingnave" style="float: right;">
                            <table style="vertical-align: top; float: right;">
                                <tr>
                                    <td class="">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>




<input type="text" name="eapath" id="htmleapath" runat="server" style="display:none;"/>
<input type="text" name="bceapath" id="htmlbceapath" runat="server" style="display:none;"/>
<input type="text" name="Totalpages" id="htmltotalpages" runat="server" style="display:none;"/>
<input type="text" name="ViewMode" id="htmlviewmode" runat="server" style="display:none;"/>
<input type="text" name="irecords" id="htmlirecords" runat="server" style="display:none;"/>

  <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="HFcnt" runat="server" />
            <asp:HiddenField ID="hfcheckload" runat="server" />
            <asp:HiddenField ID="hforgurl" runat="server" />
            <asp:HiddenField ID="hfnewurl" runat="server" />
            <asp:HiddenField ID="hfscrollpos" runat="server" />
            <asp:HiddenField ID="hfback" runat="server" />
            <asp:HiddenField ID="hfbackdata" runat="server" />
                <asp:HiddenField ID="hfattr" runat="server" />
             <input type="hidden" id="hfclickedattr" name="hfclickedattr" value=""   />

            <input type="text" name="plloader" id="loadloader" value="false" runat="server" style="display:none;"/> 
<%--<asp:HiddenField ID="balrecords_cnt" runat="server" />--%>
