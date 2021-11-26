using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace DaprMall.Product.Service
{
    public interface IScoreActor : IActor
    {
        Task<int> GetScoreAsync();
    }

    public class ScoreActor : Actor, IScoreActor
    {
        public ScoreActor(ActorHost host) : base(host)
        {
        }

        public Task<int> GetScoreAsync()
        {
           return Task.FromResult(123);
        }

        // TODO Implement interface methods.
    }
}
