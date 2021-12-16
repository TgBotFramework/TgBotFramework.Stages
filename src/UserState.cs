namespace TgBotFramework.Stages
{
    public class UserState : IUserState
    {
        public string Stage { get; set; }
        public long Step { get; set; }
        public string LanguageCode { get; set; }
    }

    public interface IUserState
    {
        public string Stage { get; set; } 
        public long Step { get; set; }
        public string LanguageCode { get; set; }
    }
}