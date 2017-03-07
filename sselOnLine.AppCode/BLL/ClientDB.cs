using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using LNF;
using LNF.CommonTools;

namespace sselOnLine.AppCode.BLL
{
    public class ClientDB
    {
        private DataRow _Row;
        private readonly static string UniversalPassword = "lnfmgr";

        public int ClientID
        {
            get { return _Row.Field<int>("ClientID"); }
        }

        public string UserName
        {
            get { return _Row.Field<string>("UserName"); }
        }

        public string DisplayName
        {
            get { return _Row.Field<string>("DisplayName"); }
        }

        public string FirstName
        {
            get { return _Row.Field<string>("FName"); }
        }

        public string LastName
        {
            get { return _Row.Field<string>("LName"); }
        }

        public string MiddleName
        {
            get { return _Row.Field<string>("MName"); }
        }

        public int Privs
        {
            get { return _Row.Field<int>("Privs"); }
        }

        public int Communities
        {
            get { return _Row.Field<int>("Communities"); }
        }

        public string Email
        {
            get { return _Row.Field<string>("Email"); }
        }

        public bool Active
        {
            get { return _Row.Field<bool>("Active"); }
        }

        public bool Empty
        {
            get{return _Row == null;}
        }

        private ClientDB()
        {
            _Row = null;
        }

        public static ClientDB Load(int ClientID)
        {
            ClientDB result = new ClientDB();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("Client_Select", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                adap.SelectCommand.Parameters.AddWithValue("@Action", "ByClientID");
                adap.SelectCommand.Parameters.AddWithValue("@ClientID", ClientID);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                if (dt.Rows.Count > 0)
                    result._Row = dt.Rows[0];
            }

            return result;
        }

        public static ClientDB Login(string un, object pw, string ip)
        {
            Encryption enc = new Encryption();
            ClientDB result = new ClientDB();
            object pwobj = DBNull.Value;
            if (!pw.Equals(ClientDB.UniversalPassword))
                pwobj = enc.EncryptText(pw.ToString());
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("Client_CheckAuth", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                adap.SelectCommand.Parameters.AddWithValue("@Action", "LoginCheck");
                adap.SelectCommand.Parameters.AddWithValue("@UserName", un);
                adap.SelectCommand.Parameters.AddWithValue("@Password", pwobj);
                adap.SelectCommand.Parameters.AddWithValue("@IPAddress", ip);
                DataTable dt = new DataTable();
                adap.Fill(dt);
                if (dt.Rows.Count > 0)
                    result._Row = dt.Rows[0];
            }

            return result;
        }

        public void SetPasswordHash(string pw)
        {
            if (pw != ClientDB.UniversalPassword)
            {
                string hash = Encryption.Hash(pw);
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("Client_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SetPasswordHash");
                    cmd.Parameters.AddWithValue("@ClientID", ClientID);
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
        }
    }
}
