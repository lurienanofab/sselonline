using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace sselOnLine.AppCode.BLL
{
    public static class StaffScheduleDB
    {
        public static DataTable StaffHours()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Select", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                adap.SelectCommand.Parameters.AddWithValue("@Action", "SelectAll");
                adap.Fill(dt);
            }

            return dt;
        }

        public static DataTable StaffHours(int clientId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Select", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                adap.SelectCommand.Parameters.AddWithValue("@Action", "SelectByClientID");
                adap.SelectCommand.Parameters.AddWithValue("@ClientID", clientId);
                adap.Fill(dt);
            }

            return dt;
        }

        public static DataTable MissingStaff()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Select", conn))
            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                adap.SelectCommand.Parameters.AddWithValue("@Action", "SelectMissingStaff");
                adap.Fill(dt);
            }

            return dt;
        }

        public static void UpdateStaffHours(int staffHoursId, string hoursText)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "UpdateStaffHours");
                cmd.Parameters.AddWithValue("@StaffHoursID", staffHoursId);
                cmd.Parameters.AddWithValue("@HoursText", hoursText);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteStaffHours(int StaffHoursID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DeleteStaffHours");
                cmd.Parameters.AddWithValue("@StaffHoursID", StaffHoursID);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void AddStaffHours(int clientId, string hoursText)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cnSselData"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("StaffSchedule_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertStaffHours");
                cmd.Parameters.AddWithValue("@ClientID", clientId);
                cmd.Parameters.AddWithValue("@HoursText", hoursText);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
