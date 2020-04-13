using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            string sql = "insert into person(id,name,age) values(1,'lei',21)";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int count = cmd.ExecuteNonQuery();
            Console.WriteLine(count);
            conn.Close();
            Console.ReadKey();
        }
    }
}
