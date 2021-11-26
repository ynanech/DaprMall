using Dapr.Client;
using DaprMall.Cart.Models;
using DaprMall.Cart.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprMall.Cart.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly IProductService _productService;
        private readonly DaprClient _daprClient;

        public TestController(ILogger<CartController> logger, IProductService productService,
            DaprClient daprClient)
        {
            _logger = logger;
            _logger = logger;
            _productService = productService;
            _daprClient = daprClient;
        }


        // GET: api/<TestController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        //// GET: api/<TestController>
        //[HttpGet("{id}")]
        //public async Task<IEnumerable<Product>?> Get([FromServices] DaprClient _daprClient, int id)
        //{
        //    var products = await _daprClient.InvokeMethodAsync<IEnumerable<Product>>(HttpMethod.Get, "productservice", "Product");
        //    return products;
        //}

        // GET: api/<TestController>
        [HttpGet("cart")]
        public async Task<AjaxResult<IEnumerable<Models.Cart>>> Cart([FromServices] DaprClient _daprClient)
        {
            var carts = await _daprClient.InvokeMethodAsync<AjaxResult<IEnumerable<Models.Cart>>>(HttpMethod.Get, "cartservice", "api/v1/Cart");
            return carts;
        }


        [HttpGet("order")]
        public async Task<IActionResult> Order()
        {
            if (Db.Carts.Count == 0)
                return Ok(AjaxResult.Failure("购物车为空"));

            foreach (var item in Db.Carts)
            {
                var result = await _productService.DeductionAsync(new StockDto(item.ProductId, item.Num));
                if (result?.Code == StateCode.Success == false)
                    return Ok(result);
            }

            return Ok(AjaxResult.Success("下单成功"));
        }



        [HttpGet("state")]
        public async Task<int> State()
        {
            _logger.LogInformation("状态存储调用微服务开始");
            var val = await _daprClient.GetStateAsync<int>("statestore", "Test");
            await _daprClient.SaveStateAsync("statestore", "Test", ++val);
            _logger.LogInformation("状态存储调用微服务结束");

            return val;
        }
    }
}
