using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sselOnLine.AppCode;

namespace sselOnLine.Controls
{
    public partial class SurveyNameTags : SurveyControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsUserAlreadyCompleted())
                panMain.Visible = false;
            else
                panMain.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            NewUserCompleted();
            panMain.Visible = false;
        }

        public override string GetData()
        {
            string result = string.Empty;
            result = result + (chkCleanRoom.Checked ? "CR," : "");
            result = result + (chkWetChemistry.Checked ? "WC," : "");
            result = result + (chkDayTimeOnly.Checked ? "DO," : "");
            return result;
        }
    }
}