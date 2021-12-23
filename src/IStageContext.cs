namespace TgBotFramework.Stages
{
    public interface IStageContext 
    {
        public IUserState UserStage { get; set; }
    }
}