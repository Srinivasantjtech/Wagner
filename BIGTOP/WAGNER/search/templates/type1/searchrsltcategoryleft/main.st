﻿<div class="categoryselectmenu sidebar" id="innerpgearo">
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
$FILTER_MODEL$
    </div>
  </div>


  $if(ShowFilter)$
  <section id="only-one" class="open hidden-xs" data-accordion-group="" >
    
    <div class="filterhover" id="collapse_filter">
      <div class="yourselection">
        <a href="#filter_collapse" data-parent="#collapse_filter" data-toggle="collapse">
          <h4>Filter Products By</h4>

        </a>
      </div>
    </div>
 
    <div id="filter_collapse" class="mobile_filter collapse in">
      $TBWDataList:{
      $it.TBWDataListItem$}$
    </div>
  </section>

  <div class="mpopup_wrapper visible-xs">
    <!--<div id="popup_filter" >
      <section id="only-one" class="open visible-xs" data-accordion-group="">
        <div id="filter_collapse" class="mobile_filter1 collapse in">
          $TBWDataList:{$it.TBWDataListItem$}$
        </div>
      </section>
    </div>-->
    <div id="popup_filter" class="modal fade" role="dialog" style="z-index:9999 !important;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <a type="button" class="close" data-dismiss="modal" aria-hidden="true">x</a>
            <h3 class="filter_title">Filter Products By</h3>
          </div>
          <div class="modal-body">
            <div class="container categoryselectmenu">
              <section id="only-one" class="open visible-xs" data-accordion-group="">
                <div id="filter_collapse" class="mobile_filter1 collapse in">
                  $TBWDataList:{
                  $it.TBWDataListItem$}$
                </div>
              </section>
              
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  $endif$
<!--$if(ShowFilter_Model)$-->

  <!--$endif$-->
</div>
