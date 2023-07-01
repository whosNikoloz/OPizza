using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPizza.Context;
using OPizza.Data;
using OPizza.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace OPizza.Controllers
{
    public class OrderController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5132");
        private readonly HttpClient _client;

        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(UserManager<IdentityUser> userManager)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            var response = await _client.GetAsync($"/api/Pizza/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var pizza = JsonConvert.DeserializeObject<PizzaAPI>(data);


                var user = await _userManager.GetUserAsync(HttpContext.User);

                string userId = user.Id;
                string userName = user.UserName;

                var newOrder = new OrderModel
                {
                    UserId = userId,
                    UserName = userName,
                    PizzaName = pizza.PizzaName,
                    FinalPrice = pizza.FinalPrice,
                    TomatoSauce = pizza.TomatoSauce,
                    Ham = pizza.Ham,
                    Pepperoni = pizza.Pepperoni,
                    Mushrooms = pizza.Mushrooms,
                    Olives = pizza.Olives,
                    CheeseType = pizza.CheeseType,
                    Bacon = pizza.Bacon,
                    Onions = pizza.Onions,
                    GreenPeppers = pizza.GreenPeppers,
                    Pineapple = pizza.Pineapple,
                    Jalapenos = pizza.Jalapenos,
                    Anchovies = pizza.Anchovies,

                    OrderDate = DateTime.Now
                };
                var responseUpload = await _client.PostAsJsonAsync($"/api/Order", newOrder);
                if (responseUpload.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var error = await responseUpload.Content.ReadAsStringAsync();
                    return BadRequest(error);
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
            
        }

       
        [HttpPost]
        public async Task<IActionResult> Custom(OrderModel model)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            string userId = user.Id;
            string userName = user.UserName;

            var newOrder = new OrderModel
            {
                UserId = userId,
                UserName = userName,
                PizzaName = model.PizzaName,
                FinalPrice = model.FinalPrice,
                TomatoSauce = model.TomatoSauce,
                Ham = model.Ham,
                Pepperoni = model.Pepperoni,
                Mushrooms = model.Mushrooms,
                Olives = model.Olives,
                CheeseType = model.CheeseType,
                Bacon = model.Bacon,
                Onions = model.Onions,
                GreenPeppers = model.GreenPeppers,
                Pineapple = model.Pineapple,
                Jalapenos =model.Jalapenos,
                Anchovies = model.Anchovies,

                OrderDate = DateTime.Now
            };

            var response = await _client.PostAsJsonAsync($"/api/Order", newOrder);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CustomEdit(OrderModel model)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            string userId = user.Id;
            string userName = user.UserName;


            var newOrder = new OrderModel
            {
                UserId = userId,
                UserName = userName,
                PizzaName = model.PizzaName,
                FinalPrice = model.FinalPrice,
                TomatoSauce = model.TomatoSauce,
                Ham = model.Ham,
                Pepperoni = model.Pepperoni,
                Mushrooms = model.Mushrooms,
                Olives = model.Olives,
                CheeseType = model.CheeseType,
                Bacon = model.Bacon,
                Onions = model.Onions,
                GreenPeppers = model.GreenPeppers,
                Pineapple = model.Pineapple,
                Jalapenos = model.Jalapenos,
                Anchovies = model.Anchovies,

                OrderDate = DateTime.Now
            };

            var response = await _client.PostAsJsonAsync($"/api/Order", newOrder);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest(error);
            }
        }

    }
}
