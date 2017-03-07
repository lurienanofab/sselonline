using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sselOnLine.Controls
{
    public partial class ScrollPositionTool : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string script;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("track_scroll_pos"))
            {
                script = @"<script type=""text/javascript"">";
                script += "$(window).scroll(function(e){";
                script += "$('.scroll_position').find('input:first').val($(window).scrollTop());";
                script += "});";
                script += "</script>";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "track_scroll_pos", script);
            }

            int scrollTop;
            if (!int.TryParse(hidCurrentScrollPosition.Value, out scrollTop))
                scrollTop = 0;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("set_scroll_pos"))
            {
                script = @"<script type=""text/javascript"">";
                script += "$(document).ready(function(){";
                script += "$(window).scrollTop(" + scrollTop.ToString() + ");";
                script += "});";
                script += "</script>";
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "set_scroll_pos", script);
            }
        }
    }
}