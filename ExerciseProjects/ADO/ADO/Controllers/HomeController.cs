using ADO.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

using System.Data;
using System.Diagnostics;

namespace ADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Reading Connection String in Controller
        /// </summary>
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            // Reading Connection String in Controller
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            #region Reading Connection String in Controller
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            #endregion

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string sql = "select * from Inventory";
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = sql;
                connection.Open();
                //command.ExecuteNonQuery();

                #region SqliteDataReader
                using (SqliteDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        // dataReader.FieldCount ȡ�î�ǰ���xȡӛ䛵ę�λ����
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            string currentColName = dataReader.GetName(i); // ȡ�Ù�λ���Q
                            string? currentColValue = Convert.ToString(dataReader.GetValue(i)); // ȡ�Ù�λֵ
                        }
                    }
                }
                #endregion

                #region Sqlite DataAdapter
                // Microsoft.Data.Sqlite �Пo��� SqlDataAdapter
                using (SqliteCommand command1 = new SqliteCommand(sql, connection))
                {
                    using (SqliteDataReader dataReader1 = command1.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(dataReader1);
                    }
                }
                #endregion

                connection.Close();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
