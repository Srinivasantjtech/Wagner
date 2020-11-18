<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td>
      <table border="0" width="300" class="border_7" align="Center" >
        <tr>
          $if(TBT_PDF_CATALOGUE)$
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:Blue">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:white">WES NEWS</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50"> 
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:white">FORMS / OTHERS</a>
          </td>
          $endif$
          $if(TBT_PDF_NEWS)$
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:white">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:Blue">WES NEWS</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:white">FORMS / OTHERS</a>
          </td>
          $endif$
          $if(TBT_PDF_FORMS)$
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50"> 
            <a href="CatalogueDownload.aspx?ActionResult=CATALOGUE"  style="color:white">CATALOGUE</a>
          </td>
          <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=NEWS" style="color:white">WES NEWS</a>
          </td> 
          <td align="center" class="tx_7" bgcolor="" style="color:white;font-size:10px;" width="50">
            <a href="CatalogueDownload.aspx?ActionResult=FORMS"  style="color:Blue">FORMS / OTHERS</a>
          </td> $endif$ 
        </tr> 
      </table> 
    </td>
  </tr> 
  <tr>
    <td align="center">
      <table>
        <tr>
          <td valign="top" align="center" colspan="2"  align="left">
            <table border="0" width="580" class="border_7">
              $if(TBT_PDF_CATALOGUE)$
              <tr>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">  File Description</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> PDF Modified</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> PDF Size</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">Download PDF</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">View eBook</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">Browse Online </td>
              </tr>
              $endif$
              $if(TBT_PDF_NEWS)$
              <tr>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">File Description </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">PDF Modified </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> PDF Size</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> Download PDF </td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">View eBook</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> Browse Online</td>
              </tr>
              $endif$ 
              $if(TBT_PDF_FORMS)$
              <tr>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;">File Description</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> PDF Modified</td>
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> PDF Size </td> 
                <td align="center" class="tx_7" bgcolor="#0077cc" style="color:white;font-size:10px;"> Download PDF </td>
              </tr>
              $endif$
              $TBWDataList:{$it.TBWDataListItem$}$
            </table> 
          </td>
        </tr> 
        <tr>
          <td height="5px" colspan="2">
            
          </td>
        </tr>  
      </table>
    </td>
  </tr>
  <tr> 
    <td>
      <br/>
      <br/>
    </td>
  </tr>
</table>



