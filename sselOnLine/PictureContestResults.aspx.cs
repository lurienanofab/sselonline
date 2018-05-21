using LNF.Data;
using LNF.Models.Data;
using LNF.Repository;
using LNF.Repository.Data;
using LNF.Web.Content;
using sselOnLine.AppCode;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace sselOnLine
{

    public partial class PictureContestResults : LNFPage
    {
        private readonly Contest _contest = new Contest(Contest.GetContestName());

        public override ClientPrivilege AuthTypes
        {
            get { return 0; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["command"] == "Delete")
                {
                    if (Guid.TryParse(Request.QueryString["id"], out Guid imageId))
                    {
                        ContestImage cimg = _contest.GetImage(imageId);
                        if (cimg != null)
                        {
                            _contest.DeleteImage(cimg);
                            Response.Redirect("~/PictureContestResults.aspx");
                        }
                    }
                }

                if (IsAdmin())
                    LoadContestImages();
                else
                    litNotAllowed.Text = "<div class=\"alert alert-danger\" role=\"alert\">You do not have permission to view this page.</div>";
            }

            litContestTitle.Text = Contest.GetContestTitle();
        }

        private void LoadContestImages()
        {

            var images = _contest.GetAllImages().Select(CreateVoteResultItem).ToList();

            // assign value if it is selected by user
            if (images.Count > 0)
            {
                rptImages.DataSource = images.OrderByDescending(x => x.VoteCount);
                rptImages.DataBind();
            }
            else
            {
                litNotAllowed.Text = "<p><em class=\"text-muted\">No images have been submitted.</em></p><hr>";
            }
        }

        private VoteResultItem CreateVoteResultItem(ContestImage image)
        {
            Client c = DA.Current.Single<Client>(image.ClientID);

            return new VoteResultItem()
            {
                ImageID = image.ImageID,
                VoteCount = GetVoteCount(image),
                Description = image.Description,
                SubmittedBy = string.Format("{0} ({1})", c.DisplayName, DA.Use<IClientManager>().PrimaryEmail(c))
            };
        }

        protected int GetVoteCount(ContestImage image)
        {
            return _contest.GetVoteCount(image);
        }

        private bool IsAdmin()
        {
            // check if admininstrator, staff, or developer
            if (CurrentUser.HasPriv(ClientPrivilege.Administrator | ClientPrivilege.Staff | ClientPrivilege.Developer))
                return true;

            // check if in user committee
            // this is not reliable at this time, there needs to be some cleanup 
            //int userCommitteeFlag = 16;
            //if ((CurrentUser.Communities & userCommitteeFlag) > 0)
            //    return true;

            if (GetUserCommitteeMembers().Contains(CurrentUser.ClientID))
                return true;

            return false;
        }

        private int[] GetUserCommitteeMembers()
        {
            string setting = ConfigurationManager.AppSettings["UserCommitteeMembers"];
            int[] result = setting.Split(',').Select(x => int.Parse(x)).ToArray();
            return result;
        }
    }

    public class VoteResultItem
    {
        public Guid ImageID { get; set; }
        public string Description { get; set; }
        public string SubmittedBy { get; set; }
        public int VoteCount { get; set; }
    }
}