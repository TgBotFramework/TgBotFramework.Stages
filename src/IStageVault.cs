using System;

namespace TgBotFramework.Stages;

public interface IStageVault<T> where T : IStage
{
    T GetOrCreateStage(Guid entityId);
    void Save(T stage);
}