using Microsoft.AspNetCore.Mvc;
using OPizza.Context;
using OPizza.Models;
using Newtonsoft.Json;

namespace OPizza.Controllers
{
    public class CustomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(OrderModel Custom,string cheesetype)
        {
            if (cheesetype != null)
            {
                Custom.CheeseType = cheesetype;
            }
            else
            {
                Custom.CheeseType = "none";
            }

            decimal price = 4; // base pizza price
            Dictionary<string, int> ingredientPrices = new Dictionary<string, int>()
            {
                { "TomatoSauce", 1 },
                { "Ham", 2 },
                { "Pepperoni", 3 },
                { "Mushrooms", 1 },
                { "Olives", 2 },
                { "Bacon", 1 },
                { "Onions", 1 },
                { "GreenPeppers", 3 },
                { "Pineapple", 2 },
                { "Jalapenos", 1 },
                { "Anchovies", 1 }
            };
            if (Custom != null)
            {
                if (Custom.TomatoSauce)
                    price += ingredientPrices["TomatoSauce"];
                if (Custom.Ham)
                    price += ingredientPrices["Ham"];
                if (Custom.Pepperoni)
                    price += ingredientPrices["Pepperoni"];
                if (Custom.Mushrooms)
                    price += ingredientPrices["Mushrooms"];
                if (Custom.Olives)
                    price += ingredientPrices["Olives"];
                if (Custom.Bacon)
                    price += ingredientPrices["Bacon"];
                if (Custom.Onions)
                    price += ingredientPrices["Onions"];
                if (Custom.GreenPeppers)
                    price += ingredientPrices["GreenPeppers"];
                if (Custom.Pineapple)
                    price += ingredientPrices["Pineapple"];
                if (Custom.Jalapenos)
                    price += ingredientPrices["Jalapenos"];
                if (Custom.Anchovies)
                    price += ingredientPrices["Anchovies"];
            } 

            Dictionary<string, int> CheeseTypes = new Dictionary<string, int>()
            {
                { "cheddar", 4 },
                { "mozzarella", 3 },
                { "swiss", 5 },
                { "gouda", 3 },
                { "blue", 6 },
                { "parmesan", 2 },
                { "provolone", 3 },
                { "burrata", 4 },
                { "sulguni", 3 },
                { "none", 0 }
               
            };

            if (Custom.CheeseType != null)
            {
                price += CheeseTypes[Custom.CheeseType.ToString()];
            }
         


            string customJson = JsonConvert.SerializeObject(Custom);
            TempData["Customs"] = customJson;
            TempData["Price"] = price.ToString();


            return RedirectToAction("Custom", "Checkout");
        }

        private readonly PizzaDbContext _context;
        public CustomController(PizzaDbContext context)
        {
            _context = context;
        }



        [HttpPost]
        public IActionResult EditPizza(int id)
        {
            Pizza pizza = _context.Pizzas.Find(id);
            return View(pizza);

        }


    }
}
