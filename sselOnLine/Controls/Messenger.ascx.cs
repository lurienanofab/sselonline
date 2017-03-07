﻿using LNF.Cache;
using LNF.Data;
using LNF.Models.Data;
using LNF.Repository;
using LNF.Repository.Data;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace sselOnLine.Controls
{
    public partial class Messenger : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hidClientID.Value = CacheManager.Current.ClientID.ToString();
            panAdmin.Visible = IsMessengerAdmin();
        }

        private bool IsMessengerAdmin()
        {
            return CacheManager.Current.CurrentUser.HasPriv(ClientPrivilege.Developer);
        }

        protected void btnCompose_Click(object sender, EventArgs e)
        {
            panCompose.Visible = true;
            btnCompose.Visible = false;

            cblPrivs.DataSource = DA.Current.Query<Priv>().Select(x => new { PrivFlag = (int)x.PrivFlag, PrivType = x.PrivType }).ToArray();
            cblPrivs.DataBind();

            cblCommunity.DataSource = DA.Current.Query<Community>().ToArray();
            cblCommunity.DataBind();

            ddlManagers.DataSource = ClientOrgUtility.AllActiveManagers().Select(x => new { ClientOrgID = x.ClientOrgID, DisplayName = x.Client.DisplayName }).OrderBy(x => x.DisplayName);
            ddlManagers.DataBind();

            lbTools.DataSource = DA.Scheduler.Resource.SelectActive().OrderBy(x => x.ResourceName).ToList();
            lbTools.DataBind();

            cblAreas.DataSource = LNF.Providers.PhysicalAccess.GetAreas();
            cblAreas.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            panCompose.Visible = false;
            btnCompose.Visible = true;
        }

        protected void btnViewRecipients_Click(object sender, EventArgs e)
        {

        }
    }
}