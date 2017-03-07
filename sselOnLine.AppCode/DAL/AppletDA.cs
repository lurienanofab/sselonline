using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace sselOnLine.AppCode.DAL
{
    public static class AppletDA
    {
        public static DataTable SelectProperties()
        {
            DataTable dt = null;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM AppletProperty", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                dt = new DataTable();
                adap.Fill(dt);
            }

            return dt;
        }

        public static void UpdateProperty(string propname, string propval)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("IF EXISTS(SELECT * FROM AppletProperty WHERE Property = @Property) UPDATE AppletProperty SET Value = @Value WHERE Property = @Property ELSE INSERT AppletProperty (Property, Value) VALUES (@Property, @Value)", conn))
            {
                cmd.Parameters.AddWithValue("@Property", propname);
                cmd.Parameters.AddWithValue("@Value", propval);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
    }
}
