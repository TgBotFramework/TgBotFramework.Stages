namespace TgBotFramework.Stages
{
    public interface IStageContext : IUpdateContext
    {
        public IUserState UserState { get; set; }
    }
}