using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OPizza.Context;
using OPizza.Data;
using OPizza.Models;
using Microsoft.AspNetCore.Identity;


namespace OPizza.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderDbContext _context;
        private readonly PizzaDbContext _Context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(OrderDbContext context, PizzaDbContext context1 , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _Context = context1;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            Pizza pizza = _Context.Pizzas.Find(id);

            if (pizza == null)
            {
                // Handle the case where the pizza with the specified ID was not found
                return NotFound();
            }

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

            _context.Orders.Add(newOrder);
            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
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

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
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

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}
