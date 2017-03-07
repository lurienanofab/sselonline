using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sselOnLine.AppCode;

namespace sselOnLine.Controls
{
    public partial class SurveyMOU : SurveyControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsUserAlreadyCompleted())
                panMain.Visible = false;
            else
                panMain.Visible = true;
        }

        protected void btnAgree_Click(object sender, EventArgs e)
        {
            NewUserCompleted();
            panMain.Visible = false;
        }

        public override string GetData()
        {
            return string.Empty;
        }
    }
}