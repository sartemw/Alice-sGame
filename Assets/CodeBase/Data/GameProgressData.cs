using System;

namespace CodeBase.Data
{
    public class GameProgressData
    {
        public int LevelsCompleted = 1;
        
        public Action Changed;
        
        public void LevelCompleted()
        {
            LevelsCompleted ++;
            Changed?.Invoke();
        }
    }
}