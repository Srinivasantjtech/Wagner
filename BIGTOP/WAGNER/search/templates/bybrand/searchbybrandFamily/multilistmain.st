
<script language="javascript" type="text/javascript">
  function urlredirectB() {
  var SelAttrStr = '';
  var field=1;
  var url=window.document.location.href;  
  var Qarry=url.split('&');  
	var param = 'pl.aspx?&id=0&' + Qarry[2];
	var param1='';	
	
	while(field <=3)
		{		
		for (var j = 0; j < document.getElementById(field).options.length; j++)
		{
      if (document.getElementById(field).options[j].selected) 
			{      
		  if(document.getElementById(field).options[j].value!='Select Brand' && document.getElementById(field).options[j].value!='List all models' && document.getElementById(field).options[j].value!='List all products')
      {

      if(document.getElementById(field).options[j].value!='List all models' && document.getElementById(field).options[j].value!='List all products')
      {
        SelAttrStr = document.getElementById(field).options[j].value + '^' + field;
              if (field == 1)
              {
              param1 = '&sb=' + document.getElementById(field).options[j].value;
              break;
              }
              else if (field == 2)
              {
              param1 = param1 + '&sm=' + escape(document.getElementById(field).options[j].value);
              break;
              }
            
              }
              else
              {
              break;
              }
      
      }else
      {
      break;
      }
      }
      }field++;
      }
      if(param1.length>1){
      param=param+param1;    
      window.document.location = param+'&byp=2';
			}
			}
		</script>

<table width="563" cellspacing="0" cellpadding="0"><tr><td width="20px">&nbsp;</td><td align="left" colspan="1" height="20" class="tx_7A">$TOSUITE_DISPLAY$</td><td valign="middle" align="left" height="55px"><table cellspacing="8" valign="top" ><tr>$TBWDataList:{$it.TBWDataListItem$}$<td ></td></tr></table></td></tr></table>