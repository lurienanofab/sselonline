using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace sselOnLine.AppCode.BLL
{
    public static class UnsubscribeDB
    {
        public struct UserInfo
        {
            public int ClientID;
            public string FirstName;
            public string LastName;
            public string DisplayName;
            public string Email;
            public string Organization;
            public bool Active;
        }

        public static UserInfo GetUserByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT c.ClientID, c.LName, c.FName, co.Email, c.FName + ' ' + c.LName AS 'FullName', o.OrgName, c.Active FROM ClientOrg co INNER JOIN Client c ON c.ClientID = co.ClientID INNER JOIN Org o ON o.OrgID = co.OrgID WHERE co.Email = @email", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.Parameters.AddWithValue("@email", email);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                UserInfo result = new UserInfo();
                result.ClientID = -1;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["ClientID"] != DBNull.Value)
                    {
                        result.ClientID = Convert.ToInt32(dr["ClientID"]);
                        result.FirstName = dr["FName"].ToString();
                        result.LastName = dr["LName"].ToString();
                        result.DisplayName = dr["FullName"].ToString();
                        result.Email = dr["Email"].ToString();
                        result.Organization = dr["OrgName"].ToString();
                        result.Active = Convert.ToBoolean(dr["Active"]);
                    }
                }

                return result;
            }
        }
    }
}
