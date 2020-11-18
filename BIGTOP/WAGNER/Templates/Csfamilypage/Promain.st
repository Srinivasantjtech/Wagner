<div class="clear"></div>
$if(IS_COMMON_VALUE)$
<div class="sptbl_wrap">
  <table class="table table-bordered">
    <thead class="mythead">
      <tr class="greyborder">
        <th colspan="5" > Common Values </th>
      </tr>
    </thead>
    <tbody>
    $PRODUCT_DETAILS_HEAD$
     </tbody>

  </table>
</div>
$endif$
$if(PRODUCT_FILTER_SHOW)$

<div class="tbl_filter_wrap hidden-xs clearfix" style="background: #f0f0f0;padding:15px">

	<div class="" style="color: #555;font-weight: 700;font-size: 15px;line-height: 35px;background: #f0f0f0;padding-left: 5px;">
		Filter products by one for more options below:
		<!--<a onclick="clearfilter();" style="float:right">Clear All</a>-->
	</div>

	$PRODUCT_FILTER$

	<!--<select id="dropdown1" class="dropdown">

		<option value="item1">ListItem 1</option>

		<option value="item2">ListItem 2</option>

		<option value="item3">ListItem 3</option>

		<option value="item4">ListItem 4</option>

		<option value="item5">ListItem 5</option>

	</select>-->

	<!--<select data-style="bg-white rounded-pill px-4 py-3 shadow-sm " class="multipleSelectFilter selectpicker w-100"  multiple="multiple" title="color" style="width:180px;">
		<option>white</option>
		<option>black</option>
		<option>red</option>
		<option>blue</option>
		<option>green</option>
	</select>-->



	<!--<select id="multipleSelect2" class="multipleSelectFilter selectpicker show-menu-arrow form-control" multiple="multiple" title="animals" placeholder="color">
		<option>white</option>
		<option>black</option>
		<option>red</option>
		<option>blue</option>
		<option>green</option>
	</select>-->



</div>

<div class="tbl_filter_wrap visible-xs clearfix" style="background: #f0f0f0;padding:15px">
  <!--<button class="tbl_filter_btn" onclick="return false;">
    Product Filters<i class="fa fa-caret-down fa-pull-right"></i>
  </button>
  <div class="tbl_filter_wrap tbl_filter_panel ">
    $PRODUCT_FILTER1$
  </div>-->
	<div class="mb5" style="color: #555;font-weight: 700;font-size: 15px;line-height: 35px;padding-left: 5px;">
		Filter products by one for more options below:
		<!--<a onclick="clearfilter();" style="float:right">Clear All</a>-->
	</div>
	<div class="col-xs-20 pl0">
		$PRODUCT_FILTER1$

	</div>
</div>

$endif$


<div id="RelateedPge_wrap" class="RelateedPge_wrap hidden-xs">
  $PRODUCT_DETAILS$
</div>

<div class="fmly_mbPro_wrap fp_mb_new visible-xs clearfix">
  <div id ="fmly_mbPro_wrap" class="masonry">
    $PRODUCT_DETAILS1$
  </div>
</div>
<div class="divLoadData"> 
</div>