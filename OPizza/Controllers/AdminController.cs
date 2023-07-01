using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Versioning;
using OPizza.Context;
using OPizza.Models;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OPizza.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5132");
        private readonly HttpClient _client;


        private readonly OrderDbContext _OrderDb;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(OrderDbContext orderDb, UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;

            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new PizzaViewModel();
            var Model = new List<Pizza>();

            var response = await _client.GetAsync("/api/Pizza");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var pizzas = JsonConvert.DeserializeObject<List<PizzaAPI>>(data);

                foreach (var pizza in pizzas)
                {
                    var pizaModel = new Pizza(); // Create a new instance for each pizza item

                    pizaModel.Id = pizza.Id;
                    pizaModel.Description = pizza.Description;
                    pizaModel.CheeseType = pizza.CheeseType;
                    pizaModel.Bacon = pizza.Bacon;
                    pizaModel.Onions = pizza.Onions;
                    pizaModel.GreenPeppers = pizza.GreenPeppers;
                    pizaModel.Pineapple = pizza.Pineapple;
                    pizaModel.Jalapenos = pizza.Jalapenos;
                    pizaModel.Anchovies = pizza.Anchovies;
                    pizaModel.Data = Convert.FromBase64String(pizza.Data);
                    pizaModel.FinalPrice = pizza.FinalPrice;
                    pizaModel.PizzaName = pizza.PizzaName;
                    pizaModel.TomatoSauce = pizza.TomatoSauce;
                    pizaModel.Ham = pizza.Ham;
                    pizaModel.Pepperoni = pizza.Pepperoni;
                    pizaModel.Mushrooms = pizza.Mushrooms;

                    Model.Add(pizaModel);
                }

                viewModel.Pizzas = Model;
            }

            return View(viewModel);
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

            var imageDataString = Convert.ToBase64String(imageData);

            // Create a new ImageModel instance
            var model = new PizzaAPI
            {
                Description = custom.Description,
                CheeseType = cheese,
                Bacon = custom.Bacon,
                Onions = custom.Onions,
                GreenPeppers = custom.GreenPeppers,
                Pineapple = custom.Pineapple,
                Jalapenos = custom.Jalapenos,
                Anchovies = custom.Anchovies,
                Data = imageDataString,
                FinalPrice = custom.FinalPrice,
                PizzaName = custom.PizzaName,
                TomatoSauce = custom.TomatoSauce,
                Ham = custom.Ham,
                Pepperoni = custom.Pepperoni,
                Mushrooms = custom.Mushrooms,
            };

            // Send the Pizza data to the API endpoint

            var response = await _client.PostAsJsonAsync("/api/Pizza", model);

            if (response.IsSuccessStatusCode)
            {
                // Handle successful response
                var result = await response.Content.ReadFromJsonAsync<PizzaAPI>();
                return RedirectToAction("Index", "admin");
            }
            else
            {
                // Handle error response
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var response = await _client.GetAsync($"/api/Pizza/{id}");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var pizza = JsonConvert.DeserializeObject<PizzaAPI>(data);


                var model = new Pizza
                {

                    Id = pizza.Id,
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

                    Data = Convert.FromBase64String(pizza.Data),
                };

                return View(model);
            }
            else
            {
                // Handle error response
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Pizza model)
        {
            var response = await _client.GetAsync($"/api/Pizza/{model.Id}");


            if (response.IsSuccessStatusCode)
            {

                string data = await response.Content.ReadAsStringAsync();
                var pizza = JsonConvert.DeserializeObject<PizzaAPI>(data);


                pizza.Id = model.Id;
                pizza.PizzaName = model.PizzaName;
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
                pizza.Data = Convert.ToBase64String(model.Data); ;

                var responseUpdate = await _client.PostAsJsonAsync($"/api/Pizza/{pizza.Id}", pizza);

                if (responseUpdate.IsSuccessStatusCode)
                {
                    var result = await responseUpdate.Content.ReadFromJsonAsync<PizzaAPI>();
                    return RedirectToAction("Index", "admin");
                }
                else
                {
                    // Handle error response
                    var error = await response.Content.ReadAsStringAsync();
                    return BadRequest(error);
                }


            }
            else
            {
                // Handle error response
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(Pizza model)
        {

            var response = await _client.DeleteAsync($"/api/Pizza/{model.Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }




        //Orders Seciton
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var users = _userManager.Users.ToList();

            var response = await _client.GetAsync("/api/Order");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(data);

                var viewModel = new List<OrderViewModel>()
                {
                    new OrderViewModel
                    {
                        Orders = orders,
                        Users = users
                    }
                };
                return View(viewModel);

            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }

        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var response = await _client.GetAsync($"/api/Order/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(data);

                var user = await _userManager.FindByIdAsync(order.UserId);
                var model = new OrderViewModel
                {
                    User = user,
                    Order = order,
                };
                return View(model);

            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }

        [HttpGet]
        public async Task<IActionResult>  Delete(int Id)
        {
            var response = await _client.DeleteAsync($"/api/Order/{Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Orders");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }
    }
}
