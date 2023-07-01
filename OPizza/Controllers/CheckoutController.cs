using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OPizza.Context;
using OPizza.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OPizza.Controllers
{
    public class CheckoutController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5132");
        private readonly HttpClient _client;

        public CheckoutController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpPost]
        public async Task<IActionResult> Index(int id)
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
        public IActionResult Custom()
        {
            decimal price = decimal.Parse(TempData["Price"] as string);

            var customJson = TempData["Customs"] as string;
            if (string.IsNullOrEmpty(customJson))
            {
                // Handle the case where TempData["Customs"] is null or empty
                // For example, redirect the user to a different page or show an error message
                return RedirectToAction("Index", "Home");
            }

            var custom = JsonConvert.DeserializeObject<OrderModel>(customJson);

            var model = new OrderModel
            {
                CheeseType = custom.CheeseType,
                Bacon = custom.Bacon,
                Onions = custom.Onions,
                GreenPeppers   = custom.GreenPeppers,
                Pineapple = custom.Pineapple,
                Jalapenos  = custom.Jalapenos,
                Anchovies = custom.Anchovies,
                FinalPrice = price,
                PizzaName = "Custom",
                TomatoSauce = custom.TomatoSauce,
                Ham = custom.Ham,
                Pepperoni = custom.Pepperoni,
                Mushrooms = custom.Mushrooms,
                Olives = custom.Olives,

            };

            return View(model);
        }
       
    }
}
