using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TgBotFramework.Attributes;
using TgBotFramework.UpdatePipeline;

namespace TgBotFramework.Stages
{
    public static class FrameworkBuilderExtension
    {
        public static IBotFrameworkBuilder<TContext> UseStates<TContext>(this IBotFrameworkBuilder<TContext> builder) where TContext : IStageContext
        {
            return UseStates<TContext>(builder, Assembly.GetAssembly(typeof(TContext)));
        }
        
        public static IBotFrameworkBuilder<TContext> UseStates<TContext>(this IBotFrameworkBuilder<TContext> builder, Assembly? assembly)
            where TContext : IStageContext
        {
            var types = assembly?.GetTypes().Where(x => 
                    x.GetCustomAttribute<StageAttribute>()!=null && 
                    x.BaseType?.GetGenericTypeDefinition() == typeof(BasicStage<>)
                )
                .ToList();

            SortedDictionary<string, Type> stages  = new ();
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
}