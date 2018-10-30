using LNF.Repository;
using System.Data;

namespace sselOnLine.AppCode.DAL
{
    public static class InvalidEmailDA
    {
        public static DataTable GetAllInvalidEmailAddresses()
        {
            return DA.Command().Param("Action", "SelectAll").FillDataTable("dbo.InvalidEmailList_Select");
        }

        public static DataTable GetInvalidEmailListFiltering()
        {
            var dt = DA.Command()
                .Param("Action", "SelectFiltering")
                .FillDataTable("dbo.InvalidEmailList_Select");

            dt.PrimaryKey = new[] { dt.Columns["EmailID"] };

            return dt;
        }
    }
}
