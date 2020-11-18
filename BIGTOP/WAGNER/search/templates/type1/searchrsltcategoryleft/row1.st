 $if(ShowFilter_ct)$
<ul class="subselection-list">
  $TBWDataList:{
    $it.TBWDataListItem$}$
</ul>
$else$
<section data-accordion="" class="open">
  <button data-control="" class="" onclick="return false;">$TBT_ATTRIBUTE_TITLE$</button>
  <div data-content="" class="">

   $TBWDataList:{
    $it.TBWDataListItem$}$

  </div>
</section >
$endif$