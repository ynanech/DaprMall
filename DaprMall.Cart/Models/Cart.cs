namespace DaprMall.Cart.Models
{
    public class Cart
    {
        public Cart(string cartId, int num, int productId,  string name, decimal price)
        {
            CartId = cartId;
            Num = num;
            ProductId = productId;
            Name = name;
            Price = price;
        }

        public string CartId { get; set; }
        public int Num { get; set; }



        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
