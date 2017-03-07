using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using LNF.CommonTools;

namespace sselOnLine.AppCode.BLL
{
    public class RequirementDB
    {
        public struct LoginRequirement
        {
            public int ID;
            public string Name;
            public int MaxLoginAttempts;
        }

        private int _ClientLoginRequirementID;
        private DataRow _Row;
        private int _ToggleID;

        public RequirementDB(int clientId, int loginRequirementId)
        {
            Dictionary<string, int> id = RequirementDB.GetID(clientId, loginRequirementId);
        }

        public RequirementDB(int clientLoginRequirementId)
        {
            _ClientLoginRequirementID = clientLoginRequirementId;
            Update();
        }

        public int ClientLoginRequirementID
        {
            get { return _ClientLoginRequirementID; }
        }

        public int ClientID
        {
            get { return _Row.Field<int>("ClientID"); }
        }

        public bool Locked
        {
            get { return _Row.Field<bool>("Locked"); }
            set { _Row.SetField("Locked", value); }
        }

        public int LoginAttempts
        {
            get { return _Row.Field<int>("LoginAttempts"); }
        }

        public bool RequiredAcknowledgement
        {
            get { return _Row.Field<bool>("RequiredAcknowledgment"); }
            set { _Row.SetField("RequiredAcknowledgment", value); }
        }

        public bool RequiredFileUploaded
        {
            get { return _Row.Field<bool>("RequiredFileUploaded"); }
            set { _Row.SetField("RequiredFileUploaded", value); }
        }

        public string RequiredMessageInput
        {
            get { return _Row.Field<string>("RequiredMessageInput"); }
            set { _Row.SetField("RequiredMessageInput", value.Clip(5000)); }
        }

        public bool Complete
        {
            get { return _Row.Field<bool>("Complete"); }
            set { _Row.SetField("Complete", value); }
        }

        public void AddResponseDataNode(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (_Row["ResponseData"] == DBNull.Value || _Row["ResponseData"].ToString() == string.Empty)
                _Row["ResponseData"] = "<response></response>";

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(_Row["ResponseData"].ToString());

            XmlNode node = xdoc.SelectSingleNode("/response/add[@key='" + key + "']");

            if (node != null)
                node.Attributes["value"].Value = value;
            else
            {
                node = xdoc.CreateElement("add");
                XmlAttribute attr;

                attr = xdoc.CreateAttribute("key");
                attr.Value = key;
                node.Attributes.Append(attr);

                attr = xdoc.CreateAttribute("value");
                attr.Value = value;
                node.Attributes.Append(attr);

                xdoc.SelectSingleNode("/response").AppendChild(node);
            }

            _Row["ResponseData"] = xdoc.InnerXml;
        }

        public void RemoveResponseDataNode(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (string.IsNullOrEmpty(_Row["ResponseData"].ToString()))
            {
                _Row["ResponseData"] = "<response></response>";
                return;
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(_Row["ResponseData"].ToString());

            XmlNode node = xdoc.SelectSingleNode("/response/add[@key='" + key + "']");

            if (node != null)
                xdoc.DocumentElement.RemoveChild(node);

            _Row["ResponseData"] = xdoc.InnerXml;
        }

        public bool Active
        {
            get { return _Row.Field<bool>("Active"); }
            set { _Row.SetField("Active", value); }
        }

        public int ToggleID
        {
            get { return _ToggleID; }
        }

        public void HandleLogin()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("ClientLoginRequirement_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "HandleLogin");
                cmd.Parameters.AddWithValue("@ClientLoginRequirementID", _ClientLoginRequirementID);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Dispose();
            }
            Update();
        }

        public void Save()
        {
            Save(false, false);
        }

        public void Save(bool toggle)
        {
            Save(toggle, false);
        }

        public void Save(bool toggle, bool toggleComplete)
        {
            if (_Row != null)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("ClientLoginRequirement_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UpdateAll");
                    cmd.Parameters.AddWithValue("@ClientLoginRequirementID", _ClientLoginRequirementID);
                    cmd.Parameters.AddWithValue("@LoginAttempts", _Row["LoginAttempts"]);
                    cmd.Parameters.AddWithValue("@Locked", _Row["Locked"]);
                    cmd.Parameters.AddWithValue("@LastLoginDateTime", _Row["LastLoginDateTime"]);
                    cmd.Parameters.AddWithValue("@RequiredMessageInput", _Row["RequiredMessageInput"]);
                    cmd.Parameters.AddWithValue("@RequiredFileUploaded", _Row["RequiredFileUploaded"]);
                    cmd.Parameters.AddWithValue("@RequiredAcknowledgment", _Row["RequiredAcknowledgment"]);
                    cmd.Parameters.AddWithValue("@Complete", _Row["Complete"]);
                    cmd.Parameters.AddWithValue("@ResponseData", _Row["ResponseData"]);
                    cmd.Parameters.AddWithValue("@Active", _Row["Active"]);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Dispose();
                }

                if (toggle) Toggle(toggleComplete);
            }

            Update();
        }

        private void Toggle(bool complete)
        {
            if (_ToggleID != -1)
            {
                string sql = string.Empty;
                if (complete)
                    sql = "UPDATE ClientLoginRequirement SET Active = ~Active, Complete = ~Complete WHERE ClientID = @ClientID AND LoginRequirementID = @ToggleID";
                else
                    sql = "UPDATE ClientLoginRequirement SET Active = ~Active WHERE ClientID = @ClientID AND LoginRequirementID = @ToggleID";

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ClientID", _Row["ClientID"]);
                    cmd.Parameters.AddWithValue("@ToggleID", _ToggleID);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Dispose();
                }
            }
        }

        private void Update()
        {
            _Row = null;
            _ToggleID = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM ClientLoginRequirement WHERE ClientLoginRequirementID = @ClientLoginRequirementID", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.Text;
                adap.SelectCommand.Parameters.AddWithValue("@ClientLoginRequirementID", _ClientLoginRequirementID);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _Row = ds.Tables[0].Rows[0];
                    _ToggleID = RequirementDB.GetToggleID(Convert.ToInt32(_Row["LoginRequirementID"]));
                }
            }
        }

        public static Dictionary<string, int> GetID(int ClientID, int LoginRequirementID)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 clr.LoginRequirementID, clr.ClientLoginRequirementID FROM ClientLoginRequirement clr INNER JOIN LoginRequirement lr ON lr.LoginRequirementID = clr.LoginRequirementID WHERE clr.ClientID = @ClientID AND clr.LoginRequirementID = @LoginRequirementID", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.Text;
                adap.SelectCommand.Parameters.AddWithValue("@ClientID", ClientID);
                adap.SelectCommand.Parameters.AddWithValue("@LoginRequirementID", LoginRequirementID);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                result.Add("LoginRequirementID", -1);
                result.Add("ClientLoginRequirementID", -1);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int lrID, clrID;
                    if (!int.TryParse(ds.Tables[0].Rows[0]["LoginRequirementID"].ToString(), out lrID)) lrID = -1;
                    if (!int.TryParse(ds.Tables[0].Rows[0]["ClientLoginRequirementID"].ToString(), out clrID)) clrID = -1;

                    result["LoginRequirementID"] = lrID;
                    result["ClientLoginRequirementID"] = clrID;
                }
            }

            return result;
        }

        public static Dictionary<string, int> GetID(int ClientID, bool IsKiosk)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 lr.LoginRequirementID, clr.ClientLoginRequirementID FROM LoginRequirement lr INNER JOIN ClientLoginRequirement clr ON clr.LoginRequirementID = lr.LoginRequirementID WHERE clr.ClientID = @ClientID AND clr.Active = 1 AND lr.Active = 1 AND lr." + (IsKiosk ? "ShowOnKiosk" : "ShowOnNonKiosk") + " = 1", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.Text;
                adap.SelectCommand.Parameters.AddWithValue("@ClientID", ClientID);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                result.Add("LoginRequirementID", -1);
                result.Add("ClientLoginRequirementID", -1);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int lrID, clrID;
                    if (!int.TryParse(ds.Tables[0].Rows[0]["LoginRequirementID"].ToString(), out lrID)) lrID = -1;
                    if (!int.TryParse(ds.Tables[0].Rows[0]["ClientLoginRequirementID"].ToString(), out clrID)) clrID = -1;

                    result["LoginRequirementID"] = lrID;
                    result["ClientLoginRequirementID"] = clrID;
                }
            }

            return result;
        }

        public static LoginRequirement GetLoginRequirement(int LoginRequirementID)
        {
            LoginRequirement result = new LoginRequirement();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM LoginRequirement lr WHERE lr.LoginRequirementID = @LoginRequirementID", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.Text;
                adap.SelectCommand.Parameters.AddWithValue("@LoginRequirementID", LoginRequirementID);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result.ID = LoginRequirementID;
                    result.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    result.MaxLoginAttempts = ds.Tables[0].Rows[0].Field<int>("MaxLoginAttempts");
                }
            }

            return result;
        }

        public static int GetToggleID(int LoginRequirementID)
        {
            int result = -1;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 lr.ToggleID FROM LoginRequirement lr WHERE lr.LoginRequirementID = @LoginRequirementID", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@LoginRequirementID", LoginRequirementID);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    result = ds.Tables[0].Rows[0].Field<int>("ToggleID");
            }

            return result;
        }
    }
}
