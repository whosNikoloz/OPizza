namespace OPizza.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? PizzaName { get; set; }
        public decimal FinalPrice { get; set; }
        public bool TomatoSauce { get; set; }
        public bool Ham { get; set; }
        public bool Pepperoni { get; set; }
        public bool Mushrooms { get; set; }
        public bool Olives { get; set; }

        // Cheese
        public string? CheeseType { get; set; }

        // Additional toppings
        public bool Bacon { get; set; }
        public bool Onions { get; set; }
        public bool GreenPeppers { get; set; }
        public bool Pineapple { get; set; }
        public bool Jalapenos { get; set; }
        public bool Anchovies { get; set; }
        public byte[]? Data { get; set; }
        //Description

        public string? Description { get; set; }
    }
}
