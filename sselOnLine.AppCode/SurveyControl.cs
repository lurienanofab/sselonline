using LNF.Cache;
using LNF.CommonTools;
using LNF.Data;
using LNF.Models.Data;
using LNF.Repository;
using System;
using System.Data;
using System.Web.UI;

namespace sselOnLine.AppCode
{
    public abstract class SurveyControl : UserControl
    {
        public static readonly string DONE = "DONE";

        public int CutoffClientID { get; set; }

        public int Privs { get; set; }

        public string SurveyType { get; set; }

        public abstract string GetData();

        public SurveyControl()
        {
            Privs = 0;
            CutoffClientID = 0;
        }

        public bool IsUserAlreadyCompleted()
        {
            bool result;

            if (CutoffClientID == 0)
                result = false;
            else
                result = CacheManager.Current.CurrentUser.ClientID > CutoffClientID;

            if (!result)
            {
                if (Privs == 0)
                    result = false;
                else
                    result = !CacheManager.Current.CurrentUser.HasPriv(PrivUtility.CalculatePriv(Privs));
            }

            if (result) return true;

            string sql = "SELECT * FROM Survey WHERE ClientID = @ClientID AND SurveyType = @SurveyType";

            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            using (IDataReader reader = dba.CommandTypeText().ApplyParameters(new { ClientID = GetClientID(), SurveyType = SurveyType }).ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    string status = reader["Status"].ToString();
                    if (status.Trim().Equals(SurveyControl.DONE))
                    {
                        reader.Close();
                        return true;
                    }
                }
            }

            return false;
        }

        public int NewUserCompleted()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                string sql = "INSERT INTO Survey(ClientID, Status, Time, Data, SurveyType) VALUES(@ClientID, @Status, @Time, @Data, @SurveyType)";
                return dba.CommandTypeText()
                    .AddParameter("@ClientID", GetClientID())
                    .AddParameter("@Status", SurveyControl.DONE)
                    .AddParameter("@Time", DateTime.Now)
                    .AddParameter("@Data", GetData())
                    .AddParameter("@SurveyType", SurveyType)
                    .ExecuteNonQuery(sql);
            }
        }

        public int GetClientID()
        {
            return Convert.ToInt32(Session["ClientID"]);
        }
    }
}
