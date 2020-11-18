


  <section id="only-one" class="open hidden-xs" data-accordion-group="" >
    
    <div class="filterhover" id="collapse_filter">
      <div class="yourselection">
        <a href="#filter_collapse" data-parent="#collapse_filter" data-toggle="collapse">
          <h4>Filter By Model</h4>

        </a>
      </div>
    </div>
 
    <div id="filter_collapse" class="mobile_filter collapse in">
      $TBWDataList:{$it.TBWDataListItem$}$
    </div>
  </section>

  <div class="mpopup_wrapper visible-xs">
 
    <div id="popup_filter" class="modal fade" role="dialog" style="z-index:9999 !important;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <a type="button" class="close" data-dismiss="modal" aria-hidden="true">x</a>
            <h3 class="filter_title">Filter By Model</h3>
          </div>
          <div class="modal-body">
            <div class="container categoryselectmenu">
              <section id="only-one" class="open visible-xs" data-accordion-group="">
                <div id="filter_collapse" class="mobile_filter1 collapse in">
                  $TBWDataList:{$it.TBWDataListItem$}$
                </div>
              </section>
              
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
 
