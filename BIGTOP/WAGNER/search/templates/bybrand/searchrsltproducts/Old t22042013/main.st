<script language="javascript">

  var IE = document.all?true:false;
  var MouseX = 0;
  var MouseY = 0;

  if (!IE)
  {
  document.captureEvents(Event.MOUSEMOVE);
  document.onmousemove = getMouseXY;
  }


  function ShowPriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "visible";
  if (IE)
  {
  getMouseXY();
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = MouseY - (objDiv.clientHeight + 25) + 'px';
  }
  else
  {
  objDiv.style.left = MouseX - (objDiv.clientWidth + 5) + 'px';
  objDiv.style.top = (MouseY - (objDiv.offsetHeight + 50)) + 'px';
  }
  objDiv.style.zIndex = 1;
  objDiv.style.padding = "5px";
  }

  function getMouseXY(e)
  {
  if (IE) { // grab the x-y pos.s if browser is IE
  tempX = event.clientX + document.documentElement.scrollLeft;
  tempY = event.clientY + document.documentElement.scrollTop;
  } else {  // grab the x-y pos.s if browser is NS
  tempX = e.pageX;
  tempY = e.pageY;
  }
  MouseX = tempX;
  MouseY = tempY;
  }

  function ClosePriceTable(objectid)
  {
  var objDiv = document.getElementById(objectid);
  objDiv.style.visibility = "hidden";
  }
</script>
<table align="center" width="100%" border="0" cellpadding="0" cellspacing="0" >
  <tr>
    <td valign="bottom">
      <table height="150px" width="100%" border="0" cellpadding="0" cellspacing="0" align="left">
              <tr>
                <td rowspan="2" align="center" width="20%">
                  <img src="/ProdImages$TBT_TOSUITE_MODEL_IMAGE$" style="max-height:150px;max-width:150px;" alt=""/>
                </td>
              </tr>
              <tr>
                <td align="left">
                  <table>
                    <tr>
                      <td style="font-family: Arial, Helvetica, sans-serif; font-size: medium; font-weight: bold; color: #0066FF">
                        $TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$&nbsp;Accessories&nbsp;Product&nbsp;List
                      </td>
                    </tr>
                    <tr>
                      <td align="left">
                        See Below for Cellink's range of Accessories to suit the $TBT_TOSUITE_BRAND$&nbsp;$TBT_TOSUITE_MODEL$
                      </td>
                    </tr>
                    <tr>
                      <td>&nbsp;</td>
                    </tr>
                    <tr>
                      <td height="20px" colspan="2" >
                        <a>
                          <div style="background-color: #FFFF99;width:100%;" align="center" valign="middle" >
                            <span style="font-family: Arial, Helvetica, sans-serif;font-weight: bold;font-size:12px; vertical-align: middle">
                              <img src="images/lightbulb.png" alt="" align="middle" height="20px" style="text-align: center; vertical-align: middle" />
                              Tip! Narrow products using filter options on the left!&nbsp;&nbsp;
                            </span>
                          </div>
                        </a>
                      </td>
                    </tr>

                  </table>

                </td>
              </tr>
              <tr>
                <td width="100%" colspan="2">
                  <hr/>
                </td>
              </tr>
            </table>
    </td>
  </tr>
  <tr>
    <td height="126" align="center" class="">
      <TABLE border="0" cellSpacing="0" cellPadding="0" width="100%">
        <tr>
          <td>
            $TBWDataList1:{
            $it.TBWDataListItem1$}$
          </td>
        </tr>
      </TABLE>
    </td>
  </tr>
  <tr>
    <td colspan="3" valign="top">
      <div align="left">Displaying $TBT_COUNT$ Product(s)</div>
    </td>
  </tr>
</table>
