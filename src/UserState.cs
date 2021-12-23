namespace TgBotFramework.Stages
{
    public class UserState : IUserState
    {
        public string Stage { get; set; } = "";
        public long Step { get; set; } = 0;
        public string LanguageCode { get; set; } = "en";
    }

    public interface IUserState
    {
        public string Stage { get; set; } 
        public long Step { get; set; }
        public string LanguageCode { get; set; }
    }
}