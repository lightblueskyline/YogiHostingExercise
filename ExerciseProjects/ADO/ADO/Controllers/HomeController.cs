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
                        #region dataReader.FieldCount 取得當前所讀取記錄的欄位數量
                        //for (int i = 0; i < dataReader.FieldCount; i++)
                        //{
                        //    string currentColName = dataReader.GetName(i); // 取得欄位名稱
                        //    string? currentColValue = Convert.ToString(dataReader.GetValue(i)); // 取得欄位值
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
                //// Microsoft.Data.Sqlite 中無類似 SqlDataAdapter
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
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        ViewBag.Result = $"Operation got error: {ex.Message}";
                    }
                    connection.Close();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Database Transaction with ADO.NET SqlTransaction
        public IActionResult TransferMoney()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TransferMoney(bool throwEx)
        {
            string result = "";
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                SqliteCommand cmdRemove = new SqliteCommand("update Account set Money='0' where Name='Putin'", connection);
                SqliteCommand cmdAdd = new SqliteCommand("update Account set Money='200' where Name='Trump'", connection);
                connection.Open();
                SqliteTransaction? tx = null;
                try
                {
                    tx = connection.BeginTransaction();
                    // 加入事務
                    cmdRemove.Transaction = tx;
                    cmdAdd.Transaction = tx;
                    // 執行指令
                    cmdRemove.ExecuteNonQuery();
                    cmdAdd.ExecuteNonQuery();
                    // 模擬錯誤
                    if (throwEx)
                    {
                        throw new Exception("Sorry! Database error! Transaction failed");
                    }
                    // 提交
                    tx.Commit();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    tx?.Rollback();
                    result = ex.Message;
                }
                finally
                {
                    connection.Close();
                }
            }

            return View((object)result);
        }
        #endregion

        #region ADO.NET SqlBulkCopy
        public IActionResult TransferData() => View();

        [HttpPost]
        [ActionName("TransferData")]
        public IActionResult TransferData_Post()
        {
            string? connectionString = this.configuration["ConnectionStrings:SqliteDefaultConnection"];

            //SqliteConnection connection = new SqliteConnection(connectionString);
            //string sql = $"select * from AccountData";
            //SqliteCommand command = new SqliteCommand(sql, connection);
            //connection.Open();
            //SqliteDataReader dataReader = command.ExecuteReader();
            //// create a SqlBulkCopy object
            //SqlBulkCopy sqlBulk = new SqlBulkCopy(connection);
            ////Give your Destination table name
            //sqlBulk.DestinationTableName = "Account";
            ////Mappings
            //sqlBulk.ColumnMappings.Add("PersonName", "Name");
            //sqlBulk.ColumnMappings.Add("TotalCash", "Money");
            ////Copy rows to destination table
            //sqlBulk.WriteToServer(dataReader);

            #region 通過事務模擬
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand("select * from AccountData", connection);
                SqliteDataReader dataReader = command.ExecuteReader();
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    string insertSql = "insert into Account (Name,Money) values ($Name,$Money)";
                    using (SqliteCommand command1 = new SqliteCommand(insertSql, connection, transaction))
                    {
                        // 添加參數
                        command1.Parameters.Add("$Name", SqliteType.Text);
                        command1.Parameters.Add("$Money", SqliteType.Text);
                        while (dataReader.Read())
                        {
                            // 設置參數
                            command1.Parameters["$Name"].Value = dataReader["PersonName"];
                            command1.Parameters["$Money"].Value = dataReader["TotalCash"];
                            // 執行插入操作
                            command1.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
            #endregion

            return View();
        }
        #endregion
    }
}
