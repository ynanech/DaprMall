namespace DaprMall.Product.Models
{
    public class Db
    {

        public static Models.Product[] Products = new[]
        {
            new Models.Product(1,"Freezing",10.00M,10),
            new Models.Product(2,"Bracing",10.00M,0),
            new Models.Product(3,"Chilly",10.00M,20),
            new Models.Product(4,"Mild",10.00M,100),
            new Models.Product(5,"Warm",10.00M,50)
        };
    }
}
