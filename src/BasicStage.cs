using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.Stages
{
    public abstract class BasicStage<TContext> : IUpdateHandler<TContext> where TContext : UpdateContext, IStageContext
    {
        public virtual async Task Enter(TContext state)
        {
            
        }
        
        public virtual async Task Exit(TContext state)
        {
            state.UserStage.Stage = "default";
            state.UserStage.Step = 0;
        }

        public abstract Task HandleAsync(TContext context, UpdateDelegate<TContext> next,
            CancellationToken cancellationToken);

    }
}