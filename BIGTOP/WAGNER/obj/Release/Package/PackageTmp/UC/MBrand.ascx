<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MBrand.ascx.cs" Inherits="WES.UC.MBrand" %>
<div class="container">
	<div class="row">
    	<div class="col-sm-20">
        	<ul class="breadcrumb" id="mainbredcrumb">
            	<li><a href="#">Home</a></li>
                <li class="active">Our Brands</li>
            </ul>
        </div>
    </div>
    

<%
    Response.Write(ST_BrandAndModel());
       %>
    </div>

<script>
    $(document).ready(function () {
        var $nav = $('#categorynav > ul > li');
        $nav.hover(
            function () {
                $(this).children('a').addClass('hovered')
                $('ul', this).stop(true, true).slideDown('fast');
            },
            function () {
                $(this).children('a').removeClass('hovered')
                $('ul', this).slideUp('fast');
            }
        );
    });
</script>

<script>
    $('#main_menu li a').focus(function () {
        $('#main_menu li a').removeClass('focused');
        $(this).addClass('focused');
    });
</script>

<script type="text/javascript" src="js/jquery.accordion.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#only-one [data-accordion]').accordion();

            $('#multiple [data-accordion]').accordion({
                singleOpen: false
            });

            $('#single[data-accordion]').accordion({
                transitionEasing: 'cubic-bezier(0.455, 0.030, 0.515, 0.955)',
                transitionSpeed: 200
            });

        });

</script>
<script src="js/thegoods.js" type="text/javascript"></script>
<script src="js/jquery.customSelect.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.sortselect').customSelect();
    });
</script>
<script>
    $('#pricesorter').hover(function () {
        $('.dropdown-menu', this).show();
    }, function () {
        $('.dropdown-menu', this).hide();
    });
</script>

<script type="text/javascript">
    $(".filterhover").click(
     function () {
         $('#filter_collapse').collapse('show');
     }, function () {
         $('#filter_collapse').collapse('hide');
     }
    );
    $(".selectionhover").click(
     function () {
         $('#selected_category').collapse('show');
     }, function () {
         $('#selected_category').collapse('hide');
     }
    );
</script>
<script type="text/javascript">
    $(document).ready(function () {

        var width = $(window).width();
        $(window).resize(function () {
            if ($(this).width() < 992) {
                $('.mobile_filter').removeClass('in');
            } else {
                $('.mobile_filter').addClass('in');
            }
        });

        if ($(window).width() < 992) {

            $('.mobile_filter').removeClass('in');
        }

    });


</script> 