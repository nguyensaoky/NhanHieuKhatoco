using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DotNetNuke.News
{
    public class JSProvider
    {
        public string jsGenShortName(string fullnameControl, string asciiControl)
        {
            string js = @"<script>
function gen_short_name(){
    var fullCtrl = document.getElementById('" + fullnameControl + @"');
    var asciiCtrl = document.getElementById('" + asciiControl + @"');
    asciiCtrl.value = to_ascii_string(fullCtrl.value);
};
</script>";
            return js;
        }



        public string RegisterScriptNews(string newsClientID)
        {
            string js = @"
<script language=javascript>
    function popup2 (name,width,height) { 
         var options = ""toolbar=1,location=1,directories=1,status=1,menubar=1,scrollbars=1,resizable=1, width=""+width+"",height=""+height; 
         Cal2=window.open(name,""popup"",options); 
    }

    function SetNewsID(val)
    {
        var img = document.getElementById('" + newsClientID + @"');
        if(img) img.value = val;
    }
</script>
";
            return js;
        }
    }
}