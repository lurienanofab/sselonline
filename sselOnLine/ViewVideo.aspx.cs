using LNF.Repository;
using LNF.Repository.Data;
using LNF.Web;
using System;
using System.Web;
using System.Web.UI;

namespace sselOnLine
{
    public partial class ViewVideo : Page
    {
        public HttpContextBase ContextBase { get; }

        public ViewVideo()
        {
            ContextBase = new HttpContextWrapper(Context);
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var client = DA.Current.Single<Client>(ContextBase.CurrentUser().ClientID);
            client.IsChecked = true;
            Response.Redirect("http://cnfx.cnf.cornell.edu/mediasite/viewer/?peid=ae77f040-3aaf-4a82-86a0-bec3398a6e26");
        }
    }
}