using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using NuGet.Versioning;
using OPizza.Context;
using OPizza.Models;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace OPizza.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {


        private readonly OrderDbContext _OrderDb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PizzaDbContext db;
        public AdminController(PizzaDbContext db, OrderDbContext orderDb, UserManager<IdentityUser> userManager)
        {
            this.db = db; 
            this._OrderDb = orderDb;
            this._userManager = userManager;
        }





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

                // Start a separate thread/task to read the bytes asynchronously
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





        [HttpGet]
        public IActionResult Orders()
        {
            var users = _userManager.Users.ToList();
            var orders = _OrderDb.Orders.ToList();

            var viewModel = new List<OrderViewModel>
            {
                new OrderViewModel
                {
                   Orders = orders,
                    Users = users
                }
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var order = await _OrderDb.Orders.FindAsync(id);
            if (order != null)
            {
                var user = await _userManager.FindByIdAsync(order.UserId);
                var model = new OrderViewModel
                {
                    User = user,
                    Order = order,
                };
                return View(model);

            }
            return RedirectToAction("Orders");
        }



        [HttpGet]
        public IActionResult  Delete(int Id)
        {
            var order = _OrderDb.Orders.Find(Id);
            if (order != null)
            {
                _OrderDb.Orders.Remove(order);
                _OrderDb.SaveChanges();

                return RedirectToAction("Orders");
            }
            return RedirectToAction("Orders");
        }
    }
}
