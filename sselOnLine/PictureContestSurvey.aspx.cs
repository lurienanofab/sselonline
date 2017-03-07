using LNF.Cache;
using LNF.Models.Data;
using LNF.Web.Content;
using sselOnLine.AppCode;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace sselOnLine
{

    public partial class PictureContestSurvey : LNFPage
    {
        private readonly Contest _contest = new Contest(Contest.GetContestName());

        public override ClientPrivilege AuthTypes
        {
            get { return 0; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Contest.GetContestAllowVoting())
            {
                phVoting.Visible = true;
                phNoVoting.Visible = false;

                if (!Page.IsPostBack)
                {
                    LoadContestImages();
                }
            }
            else
            {
                phVoting.Visible = false;
                phNoVoting.Visible = true;
            }

            litContestTitle.Text = Contest.GetContestTitle();
        }

        private void LoadContestImages()
        {

            var images = _contest.GetAllImages();
            // assign value if it is selected by user
            if (images.Count > 0)
            {
                rptImages.DataSource = images;
                rptImages.DataBind();
            }
        }
        protected void btnSaveVoting_Click(object sender, EventArgs e)
        {


            List<Guid> userVotedImages = new List<Guid>();

            foreach (RepeaterItem i in rptImages.Items)
            {
                //Retrieve the state of the CheckBox
                HtmlInputCheckBox cb = (HtmlInputCheckBox)i.FindControl("chkVote");
                if (cb.Checked)
                {
                    HiddenField hiddenImageId = (HiddenField)i.FindControl("hiddenImageID");
                    userVotedImages.Add(Guid.Parse(hiddenImageId.Value));
                }
            }

            var c = CacheManager.Current.CurrentUser;

            ContestVoter voter = new ContestVoter()
            {
                ClientID = c.ClientID,
                FName = c.FName,
                LName = c.LName,
                MName = c.MName,
                VoteDateTime = DateTime.Now,
                SelectedImages = userVotedImages
            };

            if (_contest.InsertVoter(voter) > 0)
                litSaveMessage.Text = "<div class=\"alert alert-success\" role=\"alert\" style=\"margin-top: 10px;\">Your votes have been recorded. Thank you!</div>";
            else
                litSaveMessage.Text = "<div class=\"alert alert-danger\" role=\"alert\" style=\"margin-top: 10px;\">There was a problem saving your votes. Please contact LNF Staff.</div>";
        }

    }
}