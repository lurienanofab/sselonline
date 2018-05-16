using LNF.Models.Data;
using LNF.Repository;
using LNF.Repository.Data;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace sselOnLine.AppCode
{
    public class Contest
    {
        //public const string CONTEST_NAME = "image_contest_2016";

        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        private static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["MongoConnectionString"];
        }

        public static string GetContestName()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestName"];

            if (string.IsNullOrEmpty(setting))
                throw new Exception("The appSetting PictureContestName is not defined.");

            return setting;
        }

        public static string GetContestTitle()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestTitle"];

            if (string.IsNullOrEmpty(setting))
                throw new Exception("The appSetting PictureContestTitle is not defined.");

            return setting;
        }

        public static string GetContestUploadDir()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestUploadDir"];

            if (string.IsNullOrEmpty(setting))
                throw new Exception("The appSetting PictureContestUploadDir is not defined.");

            return setting;
        }

        public static bool GetContestAllowVoting()
        {
            string setting = ConfigurationManager.AppSettings["PictureContestAllowVoting"];

            if (string.IsNullOrEmpty(setting))
                throw new Exception("The appSetting PictureContestAllowVoting is not defined.");

            bool result;

            if (bool.TryParse(setting, out result))
                return result;
            else
                throw new InvalidCastException("The appSetting PictureContestAllowVoting is not a valid boolean value.");
        }

        static Contest()
        {
            _client = new MongoClient(GetConnectionString());
            _database = _client.GetDatabase("contests");
        }

        public string ContestName { get; private set; }

        private IMongoCollection<ContestImage> GetImageCollection()
        {
            return _database.GetCollection<ContestImage>(ContestName);
        }

        private IMongoCollection<ContestVoter> GetVoterCollection()
        {
            return _database.GetCollection<ContestVoter>(ContestName + "_votes");
        }

        public Contest(string contestName)
        {
            ContestName = contestName;
        }

        public IList<ContestImage> GetImages(int clientId)
        {
            var col = GetImageCollection();
            var result = col.Find(x => x.ClientID == clientId).ToList();
            return result;
        }

        public IList<ContestImage> GetAllImages()
        {
            var col = GetImageCollection();
            return col.Find(x => true).ToList();
        }

        public void SaveUserVotes(Client client, ArrayList votedImages)
        {
            //-------------------------
        }

        public void InsertImage(ContestImage item)
        {
            var col = GetImageCollection();
            col.InsertOne(item);
        }

        public void UpdateImage(ContestImage item)
        {
            var col = GetImageCollection();
            col.ReplaceOne<ContestImage>(x => x.ImageID == item.ImageID, item);
        }

        public void DeleteImage(ContestImage item)
        {
            DeleteVotes(item.ImageID);
            var col = GetImageCollection();
            col.DeleteOne<ContestImage>(x => x.ImageID == item.ImageID);
        }

        public void DeleteVotes(Guid imageId)
        {
            var col = GetVoterCollection();
            var allVoters = col.Find(x => true).ToList();

            foreach (var v in allVoters)
            {
                if (v.SelectedImages.Contains(imageId))
                {
                    List<Guid> selectedImages = v.SelectedImages.ToList();
                    selectedImages.Remove(imageId);
                    col.UpdateOne(Builders<ContestVoter>.Filter.Eq("ImageID", imageId), Builders<ContestVoter>.Update.Set("SelectedImages", selectedImages), new UpdateOptions() { IsUpsert = false });
                }
            }
        }

        public IList<ContestUser> GetUsers()
        {
            var col = GetImageCollection();
            var images = col.Find(x => true).ToList();
            var result = images.Select(x => x.ClientID).Distinct().Select(ContestUser.Create).ToList();
            return result;
        }

        public ContestImage GetImage(Guid imageId)
        {
            var col = GetImageCollection();
            return col.Find(x => x.ImageID == imageId).FirstOrDefault();
        }

        public long InsertVoter(ContestVoter item)
        {
            var col = GetVoterCollection();
            ContestVoter existing = GetVoter(item.ClientID);
            if (existing == null)
            {
                col.InsertOne(item);
                return 1;
            }
            else
            {
                var result = col.UpdateOne(
                    Builders<ContestVoter>.Filter.Eq("ClientID", existing.ClientID),
                    Builders<ContestVoter>.Update.Set("SelectedImages", item.SelectedImages).Set("VoteDateTime", item.VoteDateTime),
                    new UpdateOptions() { IsUpsert = false });

                return result.ModifiedCount;
            }
        }

        public ContestVoter GetVoter(int clientId)
        {
            var col = GetVoterCollection();
            ContestVoter result = col.Find(x => x.ClientID == clientId).FirstOrDefault();
            return result;
        }

        public int GetVoteCount(ContestImage image)
        {
            var col = GetVoterCollection();
            var allVotes = col.Find(x => true).ToList();
            int result = allVotes.Count(x => x.SelectedImages.Contains(image.ImageID));
            return result;
        }

        public IList<ContestImage> GetImages()
        {
            var col = GetImageCollection();
            return col.Find(x => true).ToList();
        }
    }

    public class ContestImage
    {
        [BsonId]
        public Guid ImageID { get; set; }

        public int ClientID { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }
    }

    public class ContestUser
    {
        protected ContestUser() { }

        public int ClientID { get; set; }

        public string FName { get; set; }

        public string MName { get; set; }

        public string LName { get; set; }

        public string DisplayName { get { return ClientItem.GetDisplayName(LName, FName); } }

        public static ContestUser Create(int clientId)
        {
            var client = DA.Current.Single<Client>(clientId);

            if (client == null)
                return null;

            return ContestUser.Create(client);
        }

        public static ContestUser Create(Client client)
        {
            return new ContestUser()
            {
                ClientID = client.ClientID,
                FName = client.FName,
                MName = client.MName,
                LName = client.LName
            };
        }
    }

    public class ContestVoter : ContestUser
    {
        public new static ContestVoter Create(Client client)
        {
            return new ContestVoter()
            {
                ClientID = client.ClientID,
                FName = client.FName,
                MName = client.MName,
                LName = client.LName
            };
        }

        [BsonId]
        public Guid VoterID { get; set; }
        public IEnumerable<Guid> SelectedImages { get; set; }
        public DateTime VoteDateTime { get; set; }
    }
}
