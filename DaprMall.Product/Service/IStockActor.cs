using Dapr.Actors;
using Dapr.Actors.Runtime;
using DaprMall.Product.Models;
using System.Text;

namespace DaprMall.Product.Service
{
    public interface IStockActor : IActor
    {
        Task<AjaxResult> DeductionAsync(StockDto input);
    }


    //[Actor(TypeName = "StockActor")]
    public class StockActor : Actor, IStockActor, IRemindable
    {
        private readonly ILogger<StockActor> _logger;
        private const string StockStateTimeout = "StockStateTimeout";
        public StockActor(ActorHost host, ILogger<StockActor> logger) : base(host)
        {
            _logger = logger;
        }


        public async Task<AjaxResult> DeductionAsync(StockDto input)
        {
            var product = Db.Products.FirstOrDefault(m => m.Id == input.ProductId);
            if (product is null)
                return AjaxResult.Failure("产品不存在");

            if (product.Deduction(input.Num) is false)
                return AjaxResult.Failure("产品库存不足");

            await RegisterReminderAsync(
                StockStateTimeout,
                Encoding.UTF8.GetBytes($"StockStateTimeout------input.ProductId:{input.ProductId}"),
                TimeSpan.FromSeconds(20),
                TimeSpan.FromMilliseconds(-1));

            return AjaxResult.Success();
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            _logger.LogWarning("收到下单超事件,事件名称：{0},消息内容：{1}", reminderName, Encoding.UTF8.GetString(state));
            return Task.CompletedTask;
        }

    }


    public class StockDto
    {
        public int ProductId { get; set; }
        public int Num { get; set; }
    }
}
