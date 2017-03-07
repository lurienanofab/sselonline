using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sselOnLine.Controls
{
    public partial class AjaxDataSource : System.Web.UI.UserControl
    {
        public string URL { get; set; }
        public int RefreshInterval { get; set; }
        public bool Enabled { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var " + ID + " = { ");
            sb.AppendLine("  enabled: " + Enabled.ToString().ToLower() + ", ");
            sb.AppendLine("  url: '" + URL + "', ");
            sb.AppendLine("  timer: null, ");
            sb.AppendLine("  interval: " + RefreshInterval + ", ");
            sb.AppendLine("  started: false, ");
            sb.AppendLine("  update: function(){ ");
            sb.AppendLine("    return " + ID + ".get(); ");
            sb.AppendLine("  }, ");
            sb.AppendLine("  get: function(){ ");
            sb.AppendLine("    var result = null; ");
            sb.AppendLine("    $.ajax({ async: false, type: 'GET', url: " + ID + ".url, dataType: 'xml', ");
            sb.AppendLine("      success: function (response) { ");
            sb.AppendLine("        result = $(response); ");
            sb.AppendLine("      } ");
            sb.AppendLine("    }); ");
            sb.AppendLine("    return result; ");
            sb.AppendLine("  }, ");
            sb.AppendLine("  start: function(){ ");
            sb.AppendLine("    if (" + ID + ".enabled) { ");
            sb.AppendLine("      if (" + ID + ".started) { return; } ");
            sb.AppendLine("      " + ID + ".started = true; ");
            sb.AppendLine("      " + ID + ".update(); ");
            sb.AppendLine("      " + ID + ".timer = setInterval(" + ID + ".update, " + RefreshInterval.ToString() + "); ");
            sb.AppendLine("    } ");
            sb.AppendLine("  }, ");
            sb.AppendLine("  stop: function(){ ");
            sb.AppendLine("    clearInterval(" + ID + ".timer); ");
            sb.AppendLine("    " + ID + ".started = false; ");
            sb.AppendLine("  } ");
            sb.AppendLine("}; ");
            sb.AppendLine("");
            sb.AppendLine("$(document).ready(function(){ ");
            sb.AppendLine("  " + ID + ".start(); ");
            sb.AppendLine("}); ");
            sb.AppendLine("</script>");
            script.Text = sb.ToString();
        }
    }
}