using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using OPizza.Context;
using OPizza.Models;
using System.Data;

namespace MVCPizza.Controllers
{
    public class PizzaController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5132");
        private readonly HttpClient _client;


        private readonly OrderDbContext _OrderDb;
        private readonly UserManager<IdentityUser> _userManager;
        public PizzaController(OrderDbContext orderDb, UserManager<IdentityUser> userManager)
        {
            this._OrderDb = orderDb;
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
        
    }

}
