using System;
using System.Threading;
using System.Threading.Tasks;

namespace TgBotFramework.Stages.StrategyMappers;

public class PerChatMapper<TStage, TContext> : IUpdateHandler<TContext> where TContext : UpdateContext, IStageContext
    where TStage : BaseStage, new()
{
    private readonly IStageVault<TStage> _vault;

    public PerChatMapper(IStageVault<TStage> vault)
    {
        _vault = vault;
    }
    public async Task HandleAsync(TContext context, UpdateDelegate<TContext> next, CancellationToken cancellationToken)
    {
        if (context.ChatId != null)
        {
            Guid id = GuidFromIds.Get(0, context.ChatId.Identifier);
            var stage = _vault.GetOrCreateStage(id);
            context.UserStage = stage;
        }
        await next(context, cancellationToken);
    }
}