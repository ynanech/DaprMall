using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using DaprMall.Product.Models;
using DaprMall.Product.Service;
using Microsoft.AspNetCore.Mvc;

namespace DaprMall.Product.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly DaprClient _daprClient;
        private readonly IActorProxyFactory _actorProxyFactory;

        public ProductController(ILogger<ProductController> logger,
            DaprClient daprClient,
            IActorProxyFactory actorProxyFactory)
        {
            _logger = logger;
            _daprClient = daprClient;
            _actorProxyFactory = actorProxyFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogWarning("执行产品列表查询");
            return Ok(new AjaxResult<Models.Product[]>(Db.Products));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.LogWarning("执行产品信息查询");
            var product = Db.Products.FirstOrDefault(m => m.Id == id);
            if (product == null)
                return Ok(AjaxResult.Failure());

            return Ok(new AjaxResult<Models.Product>(product));
        }


        [HttpPost()]
        public async Task<AjaxResult> Post(StockDto input)
        {
            _logger.LogWarning("执行扣减库存");
            var actorId = new ActorId(input.ProductId.ToString());
            var stockService = _actorProxyFactory.CreateActorProxy<IStockActor>(actorId, nameof(StockActor));
            //var stockService = ActorProxy.Create<IStockActor>(actorId, nameof(StockActor));
            return await stockService.DeductionAsync(input);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            _logger.LogWarning("产品（{0}）被删除", id);
            Db.Products = Db.Products.Where(m => m.Id != id).ToArray();
            await _daprClient.PublishEventAsync("pubsub", "productdelete", new AjaxResult<int>(id));
        }


        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            var actorId = new ActorId("scoreActor1");

            var proxy = ActorProxy.Create<IScoreActor>(actorId, "ScoreActor");

            var score = await proxy.GetScoreAsync();
            return Ok(new AjaxResult<int>(score));
        }

    }
}