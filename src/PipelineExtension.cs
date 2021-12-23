using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.DataStructures;
using TgBotFramework.Exceptions;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework.Stages
{
    public static class PipelineExtension
    {
        public static IBotPipelineBuilder<TContext> CheckStages<TContext>(this IBotPipelineBuilder<TContext> pipe, SortedDictionary<string, Type> stages)
           where TContext : UpdateContext, IStageContext 
        {
            foreach (KeyValuePair<string,Type> pair in stages)
            {
                pipe.ServiceCollection.AddScoped(pair.Value);
            }
            pipe.Components.Add(next => (context, cancellationToken) =>
            {
                var type = stages.PrefixSearch(context.UserStage.Stage);
                if (type != null)
                {
                    var realType = type;
                    if (type.IsGenericTypeDefinition)
                    {
                        realType = type.MakeGenericType(typeof(TContext));
                    }
                    if(context.Services.GetService(realType) is IUpdateHandler<TContext> handler)
                        return handler.HandleAsync(context, next, cancellationToken);
                    else
                    {
                        throw new PipelineException("Class wasn't registered: {0}", realType.FullName);
                    }
                }
                else
                {
                    return next(context, cancellationToken);
                }
            });
            return pipe;
        }
    }
}