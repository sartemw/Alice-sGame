using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Events
{
    public class LoadLevelSignals : IEventSignal
    {
        public string Music;
    }

    public class LevelCompletedSignals  : IEventSignal 
    {
        public LevelCompletedSignals() => 
            Debug.Log($"<color=green> Level Completed {SceneManager.GetActiveScene().name}</color>");
    }
}