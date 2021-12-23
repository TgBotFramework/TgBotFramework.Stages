using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TgBotFramework.Stages;

/// <summary>
/// Thread safe. Uses ConcurrentDictionary<Guid, T>. 
/// </summary>
/// <typeparam name="T"></typeparam>
public class InMemoryVault<T> : IStageVault<T> where T : IStage, new()
{
    public readonly ConcurrentDictionary<Guid, T> InMemoryDictionary = new ConcurrentDictionary<Guid, T>();

    public T GetOrCreateStage(Guid entityId)
    {
        return InMemoryDictionary.GetOrAdd(entityId, new T { EntityId = entityId });
    }

    public void Save(T stage)
    {
        InMemoryDictionary.AddOrUpdate(stage.EntityId, guid => stage, (guid, stage1) => stage);
    }
}