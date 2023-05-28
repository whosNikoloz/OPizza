using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPizza.Context;
using OPizza.Models;
using System.Data.SqlClient;

namespace OPizza.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Opizza;Integrated Security=True;TrustServerCertificate=True";
        private readonly string query = "SELECT id, PizzaName, TomatoSauce, Ham, Pepperoni,Data, Mushrooms, Olives,Pineapple,Anchovies,Bacon,CheeseType,GreenPeppers,Jalapenos,Onions,Description, FinalPrice FROM Pizzas";
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



        private readonly PizzaDbContext db;
        public AdminController(PizzaDbContext db)
        {
            this.db = db;
        }
        public IActionResult AddPizza()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPizza(IFormFile image, Pizza custom, string cheese)
        {
            // Read the image data from the request
            using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            var imageData = stream.ToArray();

            // Create a new ImageModel instance
            var model = new Pizza
            {
                Description = custom.Description,
                CheeseType = cheese,
                Bacon = custom.Bacon,
                Onions = custom.Onions,
                GreenPeppers = custom.GreenPeppers,
                Pineapple = custom.Pineapple,
                Jalapenos = custom.Jalapenos,
                Anchovies = custom.Anchovies,
                Data = imageData,
                FinalPrice = custom.FinalPrice,
                PizzaName = custom.PizzaName,
                TomatoSauce = custom.TomatoSauce,
                Ham = custom.Ham,
                Pepperoni = custom.Pepperoni,
                Mushrooms = custom.Mushrooms,

            };

            // Save the image data to SQL Server
            db.Pizzas.Add(model);
            await db.SaveChangesAsync();

            // Save the image to the wwwroot/images folder
            //var filePath = Path.Combine("wwwroot", "images","Pizzas", image.FileName);
            //await System.IO.File.WriteAllBytesAsync(filePath, imageData);

            return RedirectToAction("Index", "admin");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Pizza pizza = db.Pizzas.Find(id);

            var model = new Pizza
            {
                PizzaName = pizza.PizzaName,
                Description = pizza.Description,
                Bacon = pizza.Bacon,
                Onions = pizza.Onions,
                GreenPeppers = pizza.GreenPeppers,
                Pineapple = pizza.Pineapple,
                Jalapenos = pizza.Jalapenos,
                Anchovies = pizza.Anchovies,
                CheeseType = pizza.CheeseType,
                FinalPrice = pizza.FinalPrice,
                TomatoSauce = pizza.TomatoSauce,
                Ham = pizza.Ham,
                Pepperoni = pizza.Pepperoni,
                Mushrooms = pizza.Mushrooms,

                Data = pizza.Data
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pizza model)
        {

            var pizza = await db.Pizzas.FindAsync(model.Id);
            if (pizza != null)
            {
                pizza.Description = model.Description;
                pizza.Bacon = model.Bacon;
                pizza.Onions = model.Onions;
                pizza.GreenPeppers = model.GreenPeppers;
                pizza.Pineapple = model.Pineapple;
                pizza.Jalapenos = model.Jalapenos;
                pizza.Anchovies = model.Anchovies;
                if (model.CheeseType != null)
                {
                    pizza.CheeseType = model.CheeseType;
                }
                else
                {
                    pizza.CheeseType = "none";
                }
                pizza.FinalPrice = model.FinalPrice;
                pizza.TomatoSauce = model.TomatoSauce;
                pizza.Ham = model.Ham;
                pizza.Pepperoni = model.Pepperoni;
                pizza.Mushrooms = model.Mushrooms;


                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }



        [HttpPost]
        public async Task<IActionResult> Delete(Pizza model)
        {
            var pizza = await db.Pizzas.FindAsync(model.Id);
            if (pizza != null)
            {
                // delete the image file from the server

                db.Pizzas.Remove(pizza);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
