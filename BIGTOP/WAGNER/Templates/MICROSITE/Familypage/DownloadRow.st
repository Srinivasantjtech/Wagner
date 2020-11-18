$if(TBT_FAMILY_HEAD)$ 
<div class="col-lg-12 clear padding_btm padding_top15">
  <h4 class=" font_14 table_bg download_heading padding_10 ">
    <strong>$TBT_FAMILY_NAME$
</div>$endif$
<table class="table table-bordered white_bg margin_bottom_none" width="100%">
<thead>
      <tr class="">
<th class="text-center">Description</th>
<th class="text-center">File
</th>
<th class="text-center">Size</th>
</tr></thead>
<tbody>$TBWDataList:{$it.TBWDataListItem$}$
</tbody></table>