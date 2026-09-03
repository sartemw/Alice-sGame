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
            //if (CurrentLevel > 8)
              //  CurrentLevel = 8;
            
            Changed?.Invoke();
        }

        public void RestartGame()
        {
            CurrentLevel = 1;
            CompletedLevels.Clear();
            Changed?.Invoke();
        }
    }
}