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
            List<Inventory> inventories = new List<Inventory>();

            #region Reading Connection String in Controller
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            #endregion

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string sql = "select * from Inventory";
                //SqliteCommand command = connection.CreateCommand();
                //command.CommandText = sql;
                SqliteCommand command = new SqliteCommand(sql, connection);

                #region SqliteDataReader
                using (SqliteDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        #region dataReader.FieldCount ȡ�î�ǰ���xȡӛ䛵ę�λ����
                        //for (int i = 0; i < dataReader.FieldCount; i++)
                        //{
                        //    string currentColName = dataReader.GetName(i); // ȡ�Ù�λ���Q
                        //    string? currentColValue = Convert.ToString(dataReader.GetValue(i)); // ȡ�Ù�λֵ
                        //}
                        #endregion

                        inventories.Add(new Inventory
                        {
                            ID = Convert.ToInt32(dataReader["ID"]),
                            Name = Convert.ToString(dataReader["Name"]),
                            Price = Convert.ToDecimal(dataReader["Price"]),
                            Quantity = Convert.ToInt32(dataReader["Quantity"]),
                            AddedOn = Convert.ToDateTime(dataReader["AddedOn"]),
                        });
                    }
                }
                #endregion

                #region Sqlite DataAdapter
                //// Microsoft.Data.Sqlite �Пo��� SqlDataAdapter
                //using (SqliteCommand command1 = new SqliteCommand(sql, connection))
                //{
                //    using (SqliteDataReader dataReader1 = command1.ExecuteReader())
                //    {
                //        DataTable dataTable = new DataTable();
                //        dataTable.Load(dataReader1);
                //    }
                //}
                #endregion

                connection.Close();
            }

            return View(inventories);
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

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory inventory)
        {
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string sql = $"insert into Inventory (Name,Price,Quantity) values ($Name,$Price,$Quantity)";
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("$Name", inventory.Name);
                    command.Parameters.AddWithValue("$Price", inventory.Price);
                    command.Parameters.AddWithValue("$Quantity", inventory.Quantity);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    //SqliteParameter parameter = new SqliteParameter
                    //{
                    //    ParameterName = "$Name",
                    //    Value = inventory.Name,
                    //    SqliteType = SqliteType.Text,
                    //    //Size = 50,
                    //};
                    //command.Parameters.Add(parameter);
                }
            }
            ViewBag.Result = "Success";
            return View();
        }
        #endregion

        #region Update
        public IActionResult Update(int id)
        {
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            Inventory inventory = new Inventory();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string sql = $"select * from Inventory where ID=$ID";
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("$ID", id);
                    using (SqliteDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            inventory.ID = Convert.ToInt32(dataReader["ID"]);
                            inventory.Name = Convert.ToString(dataReader["Name"]);
                            inventory.Price = Convert.ToDecimal(dataReader["Price"]);
                            inventory.Quantity = Convert.ToInt32(dataReader["Quantity"]);
                            inventory.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                        }
                    }
                }
                connection.Close();
            }
            return View(inventory);
        }

        [HttpPost]
        public IActionResult Update(Inventory inventory, int id)
        {
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string sql = $"update Inventory set Name=$Name,Price=$Price,Quantity=$Quantity where ID=$ID";
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("$Name", inventory.Name);
                    command.Parameters.AddWithValue("$Price", inventory.Price);
                    command.Parameters.AddWithValue("$Quantity", inventory.Quantity);
                    command.Parameters.AddWithValue("$ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                string sql = $"delete from Inventory where ID=$ID";
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("$ID", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
