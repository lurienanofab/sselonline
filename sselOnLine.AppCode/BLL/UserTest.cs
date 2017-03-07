using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace sselOnLine.AppCode.BLL
{
    public class UserTest
    {
        public int UserTestID {get;set;}
        public int ClientID {get;set;}
        public string FirstName  {get;set;}
        public string LastName  {get;set;}
        public string UMID  {get;set;}
        public string Email  {get;set;}
        public string Misc  {get;set;}
        public DateTime TestDate {get;set;}
        public int TestID{get;set;}
        public string Answer{get;set;}
        public IPAddress IP{get;set;}
        public UserTest.Client ClientData{get;set;}

        public class Client
        {
            private int _ClientID;

            public Client(int id)
            {
                _ClientID = id;
            }

            public int ClientID
            {
                get{return _ClientID;}
            }

            public string DisplayName()
            {
                string result = string.Empty;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT c.LName + ', ' + c.FName FROM sselData.dbo.Client c WHERE c.ClientID = @ClientID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", ClientID);
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                        result = obj.ToString();
                    cmd.Connection.Close();
                }
                return result;
            }

            public string Email()
            {
                string result = string.Empty;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 co.Email FROM sselData.dbo.ClientOrg co WHERE co.ClientID = @ClientID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", ClientID);
                    cmd.Connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                        result = obj.ToString();
                    cmd.Connection.Close();
                }
                return result;
            }
        }
    }
}
