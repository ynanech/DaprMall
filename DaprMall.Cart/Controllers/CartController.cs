using Dapr;
using DaprMall.Cart.Models;
using DaprMall.Cart.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprMall.Cart.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly IProductService _productService;

        public CartController(ILogger<CartController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // GET: api/<CartController>
        [HttpGet]
        public AjaxResult<IEnumerable<Models.Cart>> Get()
        {
            return new AjaxResult<IEnumerable<Models.Cart>>(Db.Carts);
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var cart = Db.Carts.FirstOrDefault(m => m.CartId == id);
            if (cart == null)
                return Ok(AjaxResult.Failure());
            return Ok(new AjaxResult<Models.Cart>(cart));
        }

        // POST api/<CartController>
        [HttpPost]
        public async Task<AjaxResult> Post([FromBody] CartCreate cartCreate)
        {
            var result = await _productService.GetProductAsync(cartCreate.ProductId);

            if (result?.Code == StateCode.Success == false)
                return AjaxResult.Failure();

            var product = result.Data;
            Db.Carts.Add(new Models.Cart(DateTimeOffset.UtcNow.UtcTicks.ToString(),
                 cartCreate.Num, product.Id, product.Name, product.Price));

            return AjaxResult.Success();
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Db.Carts = Db.Carts.Where(m => m.CartId != id).ToList();
        }

        [Topic("pubsub", "productdelete")]
        [HttpPost("/product-notify")]
        public void Notify(AjaxResult<int> input)
        {
            _logger.LogInformation("接收到产品删除通知,产品Id：{0}", input.Data);

            Db.Carts = Db.Carts.Where(m => m.ProductId != input.Data).ToList();
        }

    }
}
