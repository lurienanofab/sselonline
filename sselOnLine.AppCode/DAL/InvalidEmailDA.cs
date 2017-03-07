using LNF.CommonTools;
using LNF.Repository;
using System.Data;

namespace sselOnLine.AppCode.DAL
{
    public static class InvalidEmailDA
    {
        public static DataTable GetAllInvalidEmailAddresses()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
                return dba.ApplyParameters(new { Action = "SelectAll" }).FillDataTable("InvalidEmailList_Select");
        }

        public static DataTable GetInvalidEmailListFiltering()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                DataTable dt = dba.ApplyParameters(new { Action = "SelectFiltering" }).FillDataTable("InvalidEmailList_Select");
                dt.PrimaryKey = new DataColumn[] { dt.Columns["EmailID"] };
                return dt;
            }
        }
    }
}
