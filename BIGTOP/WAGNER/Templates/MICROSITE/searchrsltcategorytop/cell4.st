﻿<!--<li>-->
  <!--<a href="$TBT_REWRITEURL$/pl/"  title="$TBW_ATTRIBUTE_NAME$"
     rel="/pl.aspx?&amp;id=0&amp;pcr=$TBW_PARENT_CATEGORY_ID$&amp;cid=$TBW_CATEGORY_ID$&amp;tsb=$TBW_BRAND$&amp;tsm=$TBW_MODEL$&amp;searchstr=$TBW_ATTRIBUTE_SEARCH$&amp;type=$TBW_ATTRIBUTE_TYPE$&amp;value=$TBW_ATTRIBUTE_VALUE$&amp;bname=$TBW_ATTRIBUTE_BRAND$&amp;byp=$TBW_CUSTOM_NUM_FIELD3$&amp;Path=$EA_PATH$">
    $TBW_ATTRIBUTE_NAME$$TBW_OPTION_CATEGORY_ID$
  </a>-->
  <!--<input type="submit"  style="width:100%" id="$TBW_ATTRIBUTE_INPUT$" value="$TBW_ATTRIBUTE_NAME$"
         name="$TBW_ATTRIBUTE_INPUT$"
         onclick="OnclickTab_drp('drp$TBW_ATTRIBUTE_INPUT$');" />
  <input type="hidden" id="drp$TBW_ATTRIBUTE_INPUT$" name="txt$TBW_ATTRIBUTE_INPUT$" value="$ORG_EA_PATH$@@$TBW_ATTRIBUTE_TYPE$@@$TBW_ATTRIBUTE_NAME_NEW$" >

  </input>-->
<!--</li>-->

<input type="submit"  style="width:100%" id="$TBW_ATTRIBUTE_INPUT$" value="$TBW_ATTRIBUTE_NAME$"
       name="$TBW_ATTRIBUTE_INPUT$" ONMOUSEOVER="this.className='over';" ONMOUSEOUT="this.className='';" ONMOUSEDOWN="this.className='down';" ONMOUSEUP="this.className='over';"
       onclick="OnclickTab('drp$TBW_ATTRIBUTE_INPUT$');" />
<input type="hidden" id="drp$TBW_ATTRIBUTE_INPUT$" name="txt$TBW_ATTRIBUTE_INPUT$" value="$ORG_EA_PATH$@@$TBW_ATTRIBUTE_TYPE$@@$TBW_ATTRIBUTE_NAME_NEW$" >

</input>