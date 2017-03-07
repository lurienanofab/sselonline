using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace sselOnLine.Controls
{
    public partial class Meter : System.Web.UI.UserControl
    {
        protected List<DataRowView> _DataItems = new List<DataRowView>();

        public string Group { get; set; }
        public string GroupTag { get; set; }
        public object DataSource { get; set; }
        public string AjaxDataSourceID { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                _DataItems.Add((DataRowView)e.Item.DataItem);
        }

        public new void DataBind()
        {
            if (DataSource != null)
            {
                repeater.DataSource = DataSource;
                repeater.DataBind();
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered(ID + "_script"))
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), ID + "_script", MeterClientScript(), true);

        }


        protected string MeterClientScript()
        {
            if (AjaxDataSourceID != string.Empty)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("var " + this.ID + "_update = " + AjaxDataSourceID + ".update; ");
                sb.AppendLine(AjaxDataSourceID + ".update = function() { ");
                sb.AppendLine("  var obj = " + this.ID + "_update(); ");
                sb.AppendLine("  var val; ");
                foreach (DataRowView drv in _DataItems)
                {
                    string tag = drv["MeterTag"].ToString();
                    sb.AppendLine("  if (obj){ ");
                    sb.AppendLine("    val = obj.find('group[tag=\"" + GroupTag + "\"] > meter[tag=\"" + tag + "\"] > value').text(); ");
                    sb.AppendLine("    $('#" + ID + " ." + tag + " .value').html(val); ");
                    sb.AppendLine("    $('#" + ID + " ." + tag + " .display .level').css('width', val); ");
                    sb.AppendLine("  } ");
                }
                sb.AppendLine("  return obj; ");
                sb.AppendLine("}; ");

                return sb.ToString();
            }
            else
                return string.Empty;
        }
    }
}