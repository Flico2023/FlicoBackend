using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlicoProject.DataAccessLayer
{
    class MData
    {
        static void Main()
        {
            string connectionString = "server=(localdb)\\MSSQLLocalDB;Initial Catalog=FlicoDb;Integrated Security=True";

            string sqlScript = File.ReadAllText("MockData.sql");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlScript, connection))
                {
                    
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Veritabanı başarıyla dolduruldu.");
            }
        }
    }
}
