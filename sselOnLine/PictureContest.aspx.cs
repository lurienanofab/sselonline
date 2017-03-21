using LNF.Models.Data;
using LNF.Repository;
using LNF.Repository.Data;
using LNF.Web.Content;
using Newtonsoft.Json;
using sselOnLine.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace sselOnLine
{
    public enum UsersOption
    {
        AllUsers = 1,
        UsersWithImageUploads = 2
    }

    public partial class PictureContest : LNFPage
    {
        private readonly Contest _contest = new Contest(Contest.GetContestName());

        private IList<ContestImage> _images = null;

        private int GetSelectedClientID()
        {
            if (IsAdmin())
            {
                int selectedValue = int.Parse(ddlUser.SelectedValue);

                if (selectedValue == 0) // this is the "-- Select --" option, default to current user's ClientID
                    return CurrentUser.ClientID;
                else
                    return selectedValue;
            }
            else
                return CurrentUser.ClientID;
        }

        private string[] GetAllowedFileTypes()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestAllowedFileTypes"];
            string[] result = setting.Split(',');
            return result;
        }

        private int[] GetUserCommitteeMembers()
        {
            string setting = ConfigurationManager.AppSettings["UserCommitteeMembers"];
            int[] result = setting.Split(',').Select(x => int.Parse(x)).ToArray();
            return result;
        }

        private int GetMaxUploads()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestMaxUploads"];
            int result = int.Parse(setting);
            return result;
        }

        private int GetMaxDescriptionLength()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestMaxDescriptionLength"];
            int result = int.Parse(setting);
            return result;
        }

        public override ClientPrivilege AuthTypes
        {
            get { return 0; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["view"]))
            {
                DisplayImage(Guid.Parse(Request.QueryString["view"]));
                return;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["list"]))
            {
                ListImages(Request.QueryString["list"]);
                return;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["update"]))
            {
                if (Request.QueryString["update"] == "extensions")
                {
                    string search = Request.QueryString["search"];
                    string replace = Request.QueryString["replace"];
                    UpdateImageExtensions(search, replace);
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                LoadContestRules();
                LoadContestUsers();
                LoadContestImages();
            }

            litContestTitle.Text = Contest.GetContestTitle();
        }

        private void LoadContestRules()
        {
            int maxUploads = GetMaxUploads();
            int maxDescLen = GetMaxDescriptionLength();
            litMaxUploads.Text = string.Format("{0} image{1}", maxUploads, maxUploads == 1 ? string.Empty : "s");
            litAllowedFileTypes.Text = string.Join(", ", GetAllowedFileTypes());
            litMaxDescriptionLength.Text = maxDescLen.ToString();
        }

        private void LoadContestUsers()
        {
            panAdmin.Visible = IsAdmin();

            if (panAdmin.Visible)
            {
                IList<ContestUser> users = null;

                if (GetUsersOption() == UsersOption.AllUsers)
                {
                    users = DA.Current.Query<Client>()
                        .Where(x => x.Active).ToList()
                        .Where(x => x.HasPriv(ClientPrivilege.LabUser | ClientPrivilege.Staff))
                        .Select(ContestUser.Create)
                        .ToList();
                }
                else
                {
                    users = _contest.GetUsers().ToList();
                }

                if (!users.Any(x => x.ClientID == CurrentUser.ClientID))
                    users.Add(ContestUser.Create(CurrentUser.ClientID));

                ddlUser.DataSource = users.OrderBy(x => x.DisplayName).ToList();
                ddlUser.DataBind();

                ddlUser.SelectedValue = CurrentUser.ClientID.ToString();
            }
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

        private UsersOption GetUsersOption()
        {
            int val = int.Parse(rblUsersOption.SelectedValue);
            return (UsersOption)val;
        }

        private void ListImages(string query)
        {
            Response.Clear();
            var allImages = _contest.GetImages();
            var search = allImages.Where(x => Path.GetExtension(x.FileName) == "." + query).ToList();

            Response.ContentType = "application/json";

            Response.Write(JsonConvert.SerializeObject(search));

            Response.End();
        }

        private void UpdateImageExtensions(string search, string replace)
        {
            Response.Clear();

            var allImages = _contest.GetImages();

            int count = 0;

            foreach (var item in allImages)
            {
                if (item.FileName.EndsWith("." + search))
                {
                    item.FileName = item.FileName.Replace("." + search, "." + replace);
                    _contest.UpdateImage(item);
                    count++;
                }
            }

            Response.ContentType = "application/json";

            Response.Write(JsonConvert.SerializeObject(new { count }));

            Response.End();
        }

        private int UpdateExtenstions(string searchExt, string replaceExt)
        {
            var allImages = _contest.GetImages();
            var search = allImages.Where(x => Path.GetExtension(x.FileName) == "." + searchExt).ToList();

            int result = 0;

            foreach (var item in search)
            {
                item.FileName = item.FileName.Replace("." + searchExt, "." + replaceExt);
                _contest.UpdateImage(item);
                result++;
            }

            return result;
        }

        private void DisplayImage(Guid imageId)
        {
            Response.Clear();

            var cimg = _contest.GetImage(imageId);

            if (cimg != null)
            {
                var mime = string.Format("image/" + Path.GetExtension(cimg.FileName).Replace(".", string.Empty));
                Response.ContentType = mime;

                var imgPath = GetImagePath(cimg);
                if (File.Exists(imgPath))
                {
                    Response.WriteFile(imgPath);
                    Response.End();
                    return;
                }
            }

            throw new HttpException(404, "Not found");
        }

        private IList<ContestImage> GetImages(int clientId)
        {
            if (_images == null)
                _images = _contest.GetImages(clientId);

            return _images;
        }

        private void LoadContestImages()
        {
            _images = null;

            var client = DA.Current.Single<Client>(GetSelectedClientID());

            if (client == null)
                litDisplayName.Text = "[unknown]";
            else
                litDisplayName.Text = string.Format("{0} {1}", client.FName, client.LName);

            var images = GetImages(client.ClientID);

            if (images.Count > 0)
            {
                rptImages.DataSource = images;
                rptImages.DataBind();
                btnSaveDesc.Visible = true;
                rptImages.Visible = true;
                panNoData.Visible = false;
            }
            else
            {
                rptImages.DataSource = null;
                rptImages.DataBind();
                btnSaveDesc.Visible = false;
                rptImages.Visible = false;
                panNoData.Visible = true;
            }
        }

        private string CurrentUserDirectoryPath(int clientId)
        {
            string result = Path.Combine(Contest.GetContestUploadDir(), Contest.GetContestName(), clientId.ToString());

            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);

            return result;
        }

        private string GetImagePath(ContestImage cimg)
        {
            return Path.Combine(CurrentUserDirectoryPath(cimg.ClientID), cimg.FileName);
        }

        private void ClearAllMessages()
        {
            litSaveMessageText.Text = string.Empty;
            panSaveMessage.Visible = false;

            litUploadErrorText.Text = string.Empty;
            panUploadError.Visible = false;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ClearAllMessages();

            // Make sure at least one file was uploaded
            if (!fileImages.HasFiles)
            {
                litUploadErrorText.Text = "Please choose a file to upload.";
                panUploadError.Visible = true;
                return;
            }

            // Limit the number of uploads to the set maximum
            // ----------------------------------------------------------------

            // 1. Get the current uploads
            int clientId = GetSelectedClientID();

            var images = GetImages(clientId);

            // 2. Check to see if this upload will go over the max
            int maxUploads = GetMaxUploads();
            if (Request.Files.Count + images.Count > maxUploads)
            {
                litUploadErrorText.Text = string.Format("You may only upload {0} image{1}.", maxUploads, maxUploads == 1 ? string.Empty : "s");
                panUploadError.Visible = true;
                return;
            }

            // Make sure a descripion was entered
            if (string.IsNullOrEmpty(txtNewImageDescription.Text))
            {
                litUploadErrorText.Text = "Please enter a brief description.";
                panUploadError.Visible = true;
                return;
            }

            // Handle uploaded files
            // ----------------------------------------------------------------
            int skipped = 0;
            foreach (HttpPostedFile userPostedFile in fileImages.PostedFiles)
            {
                // skip empty files
                if (userPostedFile.ContentLength == 0)
                    continue;

                // make sure file type is allowed
                string ext = Path.GetExtension(userPostedFile.FileName).ToLower();

                if (!GetAllowedFileTypes().Contains(ext))
                {
                    skipped++;
                    continue;
                }

                if (userPostedFile.ContentLength > 30000000)
                {
                    skipped++;
                    continue;
                }

                try
                {
                    ContestImage cimg = new ContestImage();
                    cimg.ClientID = clientId;
                    cimg.Description = string.Join(string.Empty, txtNewImageDescription.Text.Take(GetMaxDescriptionLength()));
                    cimg.FileName = userPostedFile.FileName;

                    userPostedFile.SaveAs(GetImagePath(cimg));

                    _contest.InsertImage(cimg);
                }
                catch (Exception ex)
                {
                    litUploadErrorText.Text = ex.Message;
                    panUploadError.Visible = true;
                }
            }

            if (skipped > 0)
            {
                litUploadErrorText.Text = string.Format("Skipped <strong>{0}</strong> invalid file{1}.", skipped, (skipped == 1) ? string.Empty : "s");
                panUploadError.Visible = true;
            }
            else
            {
                // everything went ok so clear the new image description textbox
                txtNewImageDescription.Text = string.Empty;
            }

            LoadContestImages();
        }

        protected void btnDelete_Command(object sender, CommandEventArgs e)
        {
            ClearAllMessages();

            Guid imageId = Guid.Parse(e.CommandArgument.ToString());

            var cimg = _contest.GetImage(imageId);

            if (cimg != null)
            {
                File.Delete(GetImagePath(cimg));
                _contest.DeleteImage(cimg);
            }

            LoadContestUsers();
            LoadContestImages();
        }

        protected void btnSaveDesc_Click(object sender, EventArgs e)
        {
            ClearAllMessages();

            var images = GetImages(GetSelectedClientID());

            int skipped = 0;

            foreach (RepeaterItem item in rptImages.Items)
            {
                TextBox txtDescription = (TextBox)item.FindControl("txtDescription");

                if (txtDescription != null)
                {
                    if (!string.IsNullOrEmpty(txtDescription.Text))
                    {
                        var cimg = images[item.ItemIndex];
                        cimg.Description = string.Join(string.Empty, txtDescription.Text.Take(GetMaxDescriptionLength()));
                        _contest.UpdateImage(cimg);
                    }
                    else
                        skipped++;
                }
            }

            litSaveMessageText.Text = "Image descriptions have been saved successfully.";

            if (skipped > 0)
                litSaveMessageText.Text += string.Format(" (skipped <strong>{0}</strong> empty description{1})", skipped, skipped == 1 ? string.Empty : "s");

            panSaveMessage.Visible = true;

            LoadContestImages();
        }

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContestImages();
        }

        protected void rblUsersOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedClientId = GetSelectedClientID();

            LoadContestUsers();

            // select the previously selected user if possible
            if (ddlUser.Items.FindByValue(selectedClientId.ToString()) != null)
                ddlUser.SelectedValue = selectedClientId.ToString();

            LoadContestImages();
        }
    }
}