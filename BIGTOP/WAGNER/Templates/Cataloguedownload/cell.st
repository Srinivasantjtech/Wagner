$if(TBT_PDF_CATALOGUE)$
<td align="center">$TBT_PDF_DESCRIPTION$</td>
</td>
<td align="center">$PDF_DATE$</td>
<td align="center">$PDF_SIZE$</td>
<td align="center">
  <a href="$PDF$" target="_blank" style="color:Blue">$PDF_FILE_NAME$</a>
  $if(EBOOK_DISPLAY)$
  <td align="center">
    $if(EBOOK_DISPLAY)$
    <a href="$PDF_EBOOK$" target="_blank" style="color:Blue;display:$EBOOK_DISPLAY$;">View eBook</a>
    $endif$
  </td>
  <td align="center">
    $if(TBT_ISCAT)$

    <a href="$TBT_URL_PATH$?&ld=0&cid=$TBT_CATEGORY_ID$&byp=$TBT_CUSTOM_NUM_FIELD3$&path=$EA_PATH$" style="color:Blue">Browse Online</a>
    $else$
    <a href="$TBT_URL_PATH$?&amp;id=0&amp;pcr=$TBT_PARENT_CATEGORY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=$TBT_ATTRIBUTE_VALUE$&amp;bname=&amp;byp=2&amp;Path=$EA_PATH$" style="color:Blue">Browse Online</a>
    $endif$
  </td>
  $endif$
  $if(TBT_PDF_NEWS)$
  <td align="center">$TBT_PDF_DESCRIPTION$</td>
</td>
<td align="center">$PDF_DATE$</td>
<td align="center">$PDF_SIZE$</td>
<td align="center">
  <a href="attachments$PDF$" target="_blank" style="color:Blue">$PDF_FILE_NAME$</a>
  <td align="center">
    $if(EBOOK_DISPLAY)$
    <a href="$PDF_EBOOK$" target="_blank" style="color:Blue;display:$EBOOK_DISPLAY$;">View eBook</a>
    $endif$
  </td>
  <td align="center">
    $if(TBT_ISCAT)$
    <a href="$TBT_URL_PATH$?&ld=0&cid=$TBT_CATEGORY_ID$&byp=$TBT_CUSTOM_NUM_FIELD3$&path=$EA_PATH$" style="color:Blue">Browse Online</a>
    $else$
    <a href="$TBT_URL_PATH$?&amp;id=0&amp;pcr=$TBT_PARENT_CATEGORY_ID$&amp;cid=$TBT_CATEGORY_ID$&amp;tsb=&amp;tsm=&amp;searchstr=&amp;type=Category&amp;value=$TBT_ATTRIBUTE_VALUE$&amp;bname=&amp;byp=2&amp;Path=$EA_PATH$" style="color:Blue">Browse Online</a>
    $endif$
  </td>
  $endif$
  $if(TBT_PDF_FORMS)$<td align="center"> $TBT_PDF_DESCRIPTION$</td>
</td>
<td align="center">$PDF_DATE$</td>
<td align="center">$PDF_SIZE$</td>
<td align="center">
  <a href="attachments$PDF$" target="_blank" style="color:Blue">$PDF_FILE_NAME$</a>
  <td align="center">
  </td>
  <td align="center">&nbsp;$endif$