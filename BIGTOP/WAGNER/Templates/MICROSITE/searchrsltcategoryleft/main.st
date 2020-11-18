

$if(TBT_PROD_FILTER)$
<div class="list-group font_14 hidden-xs">    
    <a  class="list-group-item active font_size_16"> Product Filters </a>
    <div id="MainMenu">
      <div class="list-group panel">
        $TBWDataList:{$it.TBWDataListItem$}$
      </div>
    </div>


  </div>
   <div id="popup_filter" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="filter_title">Product Filter</h3>
                </div>
                <div class="modal-body">
                    <div class="container">
    
                        <div id="MainMenu_mobile"  >
                    
                         <div class="list-group panel">
                                  $TBWDataList_mobile:{$it.TBWDataListItem$}$
                            </div>

                    
                        </div>
                        
                    </div>
                    
                </div>

            </div>
        </div>
    </div>
$endif$
$NEWPRODUCTLIST$


