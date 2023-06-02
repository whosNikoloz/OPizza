using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OPizza.Models;
using System.Data;

namespace MVCPizza.Controllers
{
    public class PizzaController : Controller
    {
        private readonly string _connectionString = "Server=tcp:opizzadbserver.database.windows.net,1433;Initial Catalog=OPizza_db;Persist Security Info=False;User ID=opizzaowner;Password=1Admin1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private readonly string query = "SELECT id, PizzaName, TomatoSauce, Ham, Pepperoni,Data, Mushrooms, Olives,Pineapple,Anchovies,Bacon,CheeseType,GreenPeppers,Jalapenos,Onions,Description, FinalPrice FROM Pizzas";
        public async Task<IActionResult> Index()
        {
            PizzaViewModel viewModel = new PizzaViewModel();
            var pizzas = await GetAllPizzasAsync();
            viewModel.Pizzas = pizzas != null ? pizzas : new List<Pizza>();
            return View(viewModel);
        }

        private async Task<byte[]> GetBytesAsync(SqlDataReader reader, int ordinal)
        {
            const int bufferSize = 4096;
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                long bytesRead;
                long fieldOffset = 0;

                
                await Task.Run(() =>
                {
                    while ((bytesRead = reader.GetBytes(ordinal, fieldOffset, buffer, 0, bufferSize)) > 0)
                    {
                        stream.Write(buffer, 0, (int)bytesRead);
                        fieldOffset += bytesRead;
                    }
                });

                return stream.ToArray();
            }
        }
        private async Task<List<Pizza>> GetAllPizzasAsync()
        {
            List<Pizza> pizzas = new List<Pizza>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandTimeout = 120;

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await reader.ReadAsync())
                    {
                        Pizza pizza = new Pizza();

                        pizza.Id = reader.GetInt32(reader.GetOrdinal("id"));
                        pizza.PizzaName = reader.GetString(reader.GetOrdinal("PizzaName"));
                        pizza.Description = reader.GetString(reader.GetOrdinal("Description"));
                        pizza.CheeseType = reader.GetString(reader.GetOrdinal("CheeseType"));
                        pizza.Pineapple = reader.GetBoolean(reader.GetOrdinal("Pineapple"));
                        pizza.TomatoSauce = reader.GetBoolean(reader.GetOrdinal("TomatoSauce"));
                        pizza.Anchovies = reader.GetBoolean(reader.GetOrdinal("Anchovies"));
                        pizza.Jalapenos = reader.GetBoolean(reader.GetOrdinal("Jalapenos"));
                        pizza.Onions = reader.GetBoolean(reader.GetOrdinal("Onions"));
                        pizza.GreenPeppers = reader.GetBoolean(reader.GetOrdinal("GreenPeppers"));
                        pizza.Bacon = reader.GetBoolean(reader.GetOrdinal("Bacon"));
                        pizza.Pepperoni = reader.GetBoolean(reader.GetOrdinal("Pepperoni"));
                        pizza.Ham = reader.GetBoolean(reader.GetOrdinal("Ham"));
                        pizza.Mushrooms = reader.GetBoolean(reader.GetOrdinal("Mushrooms"));
                        pizza.Olives = reader.GetBoolean(reader.GetOrdinal("Olives"));
                        pizza.FinalPrice = reader.GetDecimal(reader.GetOrdinal("FinalPrice"));
                        pizza.Data = await GetBytesAsync(reader, reader.GetOrdinal("Data"));

                        pizzas.Add(pizza);
                    }
                }
            }

            return pizzas;
        }
    }

}
