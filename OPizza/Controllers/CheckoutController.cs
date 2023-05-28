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
        private readonly PizzaDbContext _context;

        public CheckoutController(PizzaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            Pizza pizza = _context.Pizzas.Find(id);
            return View(pizza);
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
