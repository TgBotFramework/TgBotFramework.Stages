using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.Attributes;
using TgBotFramework.Stages.StrategyMappers;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework.Stages
{
    public static class FrameworkBuilderExtension
    {
        public static IBotFrameworkBuilder<TContext> UseStages<TContext>(this IBotFrameworkBuilder<TContext> builder, 
            StageStrategy strategy = StageStrategy.PerUser 
        )
            where TContext : UpdateContext, IStageContext
        {
            return builder.UseStages<InMemoryVault<BaseStage>, BaseStage, TContext>(strategy);
        }
    
        
        public static IBotFrameworkBuilder<TContext> UseStages<TStageVault, TStage,  TContext>(
            this IBotFrameworkBuilder<TContext> builder,
            StageStrategy strategy
            )
            where TContext : UpdateContext, IStageContext
            where TStageVault : class, IStageVault<TStage>
            where TStage : BaseStage, new()
        {
            builder.Services.AddSingleton<IStageVault<TStage>, TStageVault>();

            switch (strategy)
            {
                case StageStrategy.PerUser:
                    builder.Services.AddScoped<PerUserMapper<TStage, TContext>>();
                    builder.UseMiddleware<PerUserMapper<TStage, TContext>>();
                    break;
                case StageStrategy.PerChat:
                    builder.Services.AddScoped<PerChatMapper<TStage, TContext>>();
                    builder.UseMiddleware<PerChatMapper<TStage, TContext>>();
                    break;
                case StageStrategy.PerUserInChat:
                    builder.Services.AddScoped<PerUserInChatMapper<TStage, TContext>>();
                    builder.UseMiddleware<PerUserInChatMapper<TStage, TContext>>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return builder;
        }

        
        public static IBotFrameworkBuilder<TContext> DiscoverStages<TContext>(this IBotFrameworkBuilder<TContext> builder)
            where TContext : UpdateContext, IStageContext
        {
            return DiscoverStages<TContext>(builder,
                Assembly.GetAssembly(typeof(TContext))
                ?? throw new Exception($"Can`t get assembly for type {nameof(TContext)}"));
        }

        public static IBotFrameworkBuilder<TContext> DiscoverStages<TContext>(this IBotFrameworkBuilder<TContext> builder,
            Assembly assembly)
            where TContext : UpdateContext, IStageContext
        {
            var types = assembly?.GetTypes().Where(x =>
                    x.GetCustomAttribute<StageAttribute>() != null &&
                    x.BaseType?.GetGenericTypeDefinition() == typeof(BasicStage<>)
                )
                .ToList();

            SortedDictionary<string, Type> stages = new();
            if (types != null)
                foreach (var type in types)
                {
                    var attribute = type.GetCustomAttribute<StageAttribute>();
                    Debug.Assert(attribute != null, nameof(attribute) + " != null");

                    stages.Add(attribute.Stage, type);
                }

            builder.Pipeline.CheckStages(stages);

            //TODO ability to add states from commands
            builder.Services.AddSingleton<StageManager>(new StageManager(stages));

            return builder;
        }
    }


    public interface IStage : IUserState
    {
        Guid EntityId { get; set; }
    }

    public class BaseStage : IStage
    {
        public Guid EntityId { get; set; }
        public string Stage { get; set; }
        public long Step { get; set; }
        public string LanguageCode { get; set; }
    }
}