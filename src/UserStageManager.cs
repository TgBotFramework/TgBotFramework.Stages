using System;

namespace TgBotFramework.Stages
{
    public class UserStageManager : IUserState
    {
        private readonly StageManager _stageManager;
        private string _stage;
        private long _step;
        private string _languageCode;

        public UserStageManager(StageManager stageManager)
        {
            _stageManager = stageManager;
        }

        public string Stage
        {
            get => _stage;
            set
            {
                if (!_stageManager.Check(value))
                    throw new ArgumentOutOfRangeException("There is no handler for stage "+value);
                _stage = value;
            }
        }

        public long Step
        {
            get => _step;
            set => _step = value;
        }

        public string LanguageCode
        {
            get => _languageCode;
            set => _languageCode = value;
        }
    }
}