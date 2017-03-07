using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using LNF.CommonTools;
using sselOnLine.AppCode.DAL;

namespace sselOnLine.AppCode
{
    public abstract class Applet : System.Web.UI.UserControl
    {
        protected DataTable _Properties;

        public DataTable Properties
        {
            get
            {
                if (_Properties == null)
                    _Properties = AppletDA.SelectProperties();
                return _Properties;
            }
        }

        public string Title
        {
            get
            {
                return GetControl<Literal>("litAppletTitle").Text = string.Empty;
            }
            set
            {
                GetControl<Literal>("litAppletTitle").Text = value;
                GetControl<Literal>("litAppletAdminTitle").Text = value;
            }
        }

        public bool Enabled
        {
            get
            {
                bool result = false;

                DataRow[] rows = Properties.Select(string.Format("Property = '{0}_ENABLED'", AppletPrefix));
                if (rows.Length > 0)
                {
                    if (rows[0]["Value"].ToString() == "Yes")
                        result = true;
                }

                return result;
            }
            set
            {
                AppletDA.UpdateProperty(AppletPrefix + "_ENABLED", (value) ? "Yes" : "No");
                _Properties = AppletDA.SelectProperties();
            }
        }

        public abstract string AppletPrefix { get; }

        protected virtual void SetState()
        {
            if (IsVisible())
            {
                GetControl<Panel>("panAppletEnabled").Visible = true;
                GetControl<Panel>("panApplet").Visible = true;
                GetControl<Panel>("panAdministration").Visible = false;
                GetControl<Panel>("panAppletDisabled").Visible = false;
                GetControl<Panel>("panAdminLink").Visible = ClientIsAdmin();
            }
            else
            {
                GetControl<Panel>("panAppletEnabled").Visible = false;
                GetControl<Panel>("panApplet").Visible = false;
                GetControl<Panel>("panAdministration").Visible = false;
                GetControl<Panel>("panAppletDisabled").Visible = true;
                GetControl<LinkButton>("btnDisabledAdmin").Visible = ClientIsAdmin();
            }

            GetControl<CheckBox>("chkEnabled").Checked = Enabled;
        }

        protected virtual bool IsVisible()
        {
            return Enabled;
        }

        protected virtual int GetClientID()
        {
            return Convert.ToInt32(Session["ClientID"]);
        }

        protected virtual bool ClientIsAdmin()
        {
            bool result = false;

            if (Session["ClientID"] != null)
            {
                DataRow[] rows = Properties.Select(string.Format("Property = '{0}_ADMIN_CLIENT_ID'", AppletPrefix));
                if (rows.Length > 0)
                {
                    string list = rows[0]["Value"].ToString();
                    string[] splitter = list.Split('|');
                    foreach (string s in splitter)
                    {
                        if (s == Session["ClientID"].ToString())
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        protected virtual void btnAdmin_Click(object sender, EventArgs e)
        {
            GetControl<Panel>("panApplet").Visible = false;
            GetControl<Panel>("panAdministration").Visible = true;
        }

        protected virtual void btnAdminReturn_Click(object sender, EventArgs e)
        {
            SetState();
        }

        protected virtual void admin_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "save_enabled":
                    Enabled = GetControl<CheckBox>("chkEnabled").Checked;
                    break;
            }
        }

        protected virtual void btnDisabledAdmin_Click(object sender, EventArgs e)
        {
            GetControl<Panel>("panAppletDisabled").Visible = false;
            GetControl<Panel>("panAppletEnabled").Visible = true;
            GetControl<Panel>("panApplet").Visible = false;
            GetControl<Panel>("panAdministration").Visible = true;
        }

        private T GetControl<T>(string id) where T:System.Web.UI.Control
        {
            return (T)FindControl(id);
        }
    }
}
