﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace sselOnLine
{
    public partial class SignOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            string strReturnUrl = Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(strReturnUrl))
            {
                Response.Redirect(strReturnUrl);
            }

        }
    }
}