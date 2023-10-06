using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NimapInfotechMachineTest.SqlDbConnection
{
    public class Connection
    {
        SqlCommand sqlcmd;
        SqlDataAdapter sqlda;
        SqlConnection sqlcon = null;
        public static string constring = @"Data Source=DESKTOP-KG1N2KU; Initial Catalog=DB_Sharda_NimapInfotech; User Id=sa;Password=Game@123";

        public SqlConnection Connect()
        {
            try
            {

                sqlcon = new SqlConnection(constring);
                sqlcon.Close();
                if (sqlcon.State == ConnectionState.Open)
                    sqlcon.Close();
                    sqlcon.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sqlcon;
        }
        public DataTable FillComboBox(string query)
        {
            DataTable dt = new DataTable();
            sqlcon = Connect();
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcon;
            sqlda = new SqlDataAdapter(query, sqlcon);
            sqlda.Fill(dt);
            return dt;
        }
    }
}