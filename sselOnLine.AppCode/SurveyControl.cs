using LNF.Data;
using LNF.Models.Data;
using LNF.Repository;
using LNF.Web;
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
                result = Context.CurrentUser().ClientID > CutoffClientID;

            if (!result)
            {
                if (Privs == 0)
                    result = false;
                else
                    result = !Context.CurrentUser().HasPriv(PrivUtility.CalculatePriv(Privs));
            }

            if (result) return true;

            string sql = "SELECT * FROM Survey WHERE ClientID = @ClientID AND SurveyType = @SurveyType";

            using (var reader = DA.Command(CommandType.Text).Param(new { Context.CurrentUser().ClientID, SurveyType }).ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    string status = reader["Status"].ToString();
                    if (status.Trim().Equals(DONE))
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
            string sql = "INSERT INTO Survey(ClientID, Status, Time, Data, SurveyType) VALUES(@ClientID, @Status, @Time, @Data, @SurveyType)";
            return DA.Command(CommandType.Text)
                .Param("ClientID", Context.CurrentUser().ClientID)
                .Param("Status", DONE)
                .Param("Time", DateTime.Now)
                .Param("Data", GetData())
                .Param("SurveyType", SurveyType)
                .ExecuteNonQuery(sql)
                .Value;
        }
    }
}
