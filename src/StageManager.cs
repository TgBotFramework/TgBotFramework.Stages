using System;
using System.Collections.Generic;
using TgBotFramework.DataStructures;

namespace TgBotFramework.Stages
{
    public class StageManager
    {
        public SortedDictionary<string, Type> StagesList { get; set; }
        
        public StageManager(SortedDictionary<string, Type> stagesList)
        {
            StagesList = stagesList;
        }

        public bool Check(string state)
        {
            if (state is null or "" or "default")
                return true;
            
            return StagesList.PrefixSearch(state) != null;
        }
    }
}