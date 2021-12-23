using System;
using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.Stages.StrategyMappers;

public class PerUserInChatMapper<TStage, TContext> : IUpdateHandler<TContext> where TContext : UpdateContext, IStageContext
    where TStage : BaseStage, new()
{
    private readonly IStageVault<TStage> _vault;

    public PerUserInChatMapper(IStageVault<TStage> vault)
    {
        _vault = vault;
    }
    public async Task HandleAsync(TContext context, UpdateDelegate<TContext> next, CancellationToken cancellationToken)
    {
        if (context.ChatId is not null && context.SenderId.HasValue)
        {
            Guid id = GuidFromIds.Get(context.SenderId.Value, context.ChatId.Identifier ?? 0);
            var stage = _vault.GetOrCreateStage(id);
            context.UserStage = stage;
        }
        await next(context, cancellationToken);
    }
}