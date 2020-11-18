  $if(ShowFilter_ct)$
<div id="sidebar" class="subct-selection" >
  <div class="subcategory-selection clearfix hidden-sm hidden-xs ">
    <div class="breadcrumb clearfix">
      <li>Sub Category Selection</li>
    </div>
    <div class="subselectdiv">
      $TBWDataList:{$it.TBWDataListItem$}$
    </div>
  </div>
</div>
$else$
<div class="categoryselectmenu" id="innerpgearo">
  <div id="collapse_selection" class="selectionhover">
    <div class="yourselection">
      <a href="#selected_category" data-parent="#collapse_selection" data-toggle="collapse" class="selected_head" aria-expanded="true">
        <h4>Your Selection</h4>
      </a>
    </div>

    <div id="selected_category" class="mobile_filter collapse in">
      <div class="selection-details">
        $TBWDataList1:{$it.TBWDataListItem1$}$
      </div>
      <div >$BRAND_AND_MODEL_HTML$</div>
      <div >$MODEL_HTML$</div>

    </div>
  </div>


  $if(ShowFilter)$
  <section id="only-one" class="open" data-accordion-group="">
    <div class="filterhover" id="collapse_filter">
      <div class="yourselection">
        <a href="#filter_collapse" data-parent="#collapse_filter" data-toggle="collapse">
          <h4>Filter Products By</h4>

        </a>
      </div>
    </div>
    <div id="filter_collapse" class="mobile_filter collapse in">
      $TBWDataList:{$it.TBWDataListItem$}$
    </div>
  </section>
  $endif$
</div>

$endif$


