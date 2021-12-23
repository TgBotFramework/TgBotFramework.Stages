using System;
using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.Stages.StrategyMappers;

public class PerUserMapper<TStage, TContext> : IUpdateHandler<TContext> where TContext : UpdateContext, IStageContext
    where TStage : BaseStage, new()
{
    private readonly IStageVault<TStage> _vault;

    public PerUserMapper(IStageVault<TStage> vault)
    {
        _vault = vault;
    }

    public async Task HandleAsync(TContext context, UpdateDelegate<TContext> next, CancellationToken cancellationToken)
    {
        if (context.SenderId is not null or 0 )
        {
            Guid id = GuidFromIds.Get(context.SenderId.Value);
            var stage = _vault.GetOrCreateStage(id);
            context.UserStage = stage;
        }

        await next(context, cancellationToken);
    }
}