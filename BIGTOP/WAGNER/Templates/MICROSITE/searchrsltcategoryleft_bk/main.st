

$if(TBT_PROD_FILTER)$
<div class="list-group font_14">    
    <a  class="list-group-item active font_size_16"> Product Filters </a>
    <div id="MainMenu">
      <div class="list-group panel">
        $TBWDataList:{$it.TBWDataListItem$}$
      </div>
    </div>


  </div>
$endif$
$NEWPRODUCTLIST$
