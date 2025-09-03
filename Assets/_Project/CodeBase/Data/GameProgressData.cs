using System;
using System.Collections.Generic;

namespace _Project.CodeBase.Data
{
    [Serializable]
    public class GameProgressData
    {
        public List<string> CompletedLevels = new List<string>(); 
        public int CurrentLevel = 1;
        
        public Action Changed;
        
        public void FirstTimeComplete()
        {
            CurrentLevel ++;
            Changed?.Invoke();
        }
    }
}