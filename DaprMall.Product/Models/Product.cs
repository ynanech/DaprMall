namespace DaprMall.Product.Models
{
    public class Product
    {
        public Product()
        {

        }

        public Product(int id, string name, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }


        public bool Deduction(int num)
        {
            if (num > Stock)
                return false;

            Stock -= num;
            return true;
        }
    }
}
