using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OPizza.Models;

namespace MVCPizza.Controllers
{
    public class PizzaController : Controller
    {
        private readonly string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Opizza;Integrated Security=True;TrustServerCertificate=True";
        private readonly string query = "SELECT id, PizzaName, TomatoSauce, Ham, Pepperoni, Mushrooms,Data, Olives,Pineapple,Anchovies,Bacon,CheeseType,GreenPeppers,Jalapenos,Onions,Description, FinalPrice FROM Pizzas";
        public IActionResult Index()
        {
            PizzaViewModel viewModel = new PizzaViewModel();
            var pizzas = GetAllPizzas();
            viewModel.Pizzas = pizzas != null ? pizzas : new List<Pizza>();
            return View(viewModel);
        }



        private List<Pizza> GetAllPizzas()
        {
            List<Pizza> pizzas = new List<Pizza>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Pizza pizza = new Pizza();

                    pizza.Id = (int)reader["id"];
                    pizza.PizzaName = (string)reader["PizzaName"];
                    pizza.Description = (string)reader["Description"];
                    pizza.CheeseType = (string)reader["CheeseType"];
                    pizza.Pineapple = (bool)reader["Pineapple"];
                    pizza.TomatoSauce = (bool)reader["TomatoSauce"];
                    pizza.Anchovies = (bool)reader["Anchovies"];
                    pizza.Jalapenos = (bool)reader["Jalapenos"];
                    pizza.Onions = (bool)reader["Onions"];
                    pizza.GreenPeppers = (bool)reader["GreenPeppers"];
                    pizza.Bacon = (bool)reader["Bacon"];
                    pizza.Pepperoni = (bool)reader["Pepperoni"];
                    pizza.Ham = (bool)reader["Ham"];
                    pizza.Mushrooms = (bool)reader["Mushrooms"];
                    pizza.Olives = (bool)reader["Olives"];
                    pizza.FinalPrice = (decimal)reader["FinalPrice"];
                    pizza.Data = (byte[])reader["Data"];

                    pizzas.Add(pizza);
                }

                reader.Close();
            }

            return pizzas;
        }
    }

}
