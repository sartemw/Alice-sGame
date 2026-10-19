using System;
using UnityEngine;

namespace _Project.CodeBase.Enemy
{
    public class BossState : MonoBehaviour
    {
        public event Action<BossStates> OnStateEntered;
        public event Action<BossStates> OnStateExited;

        public BossStates CurrentState;

        public void EnteredState(BossStates stateHash)
        {
            Debug.Log($"Entered state: {stateHash}");
            OnStateEntered?.Invoke(stateHash);
        }

        public void ExitedState(BossStates stateHash)
        {
            Debug.Log($"Exited state: {stateHash}");
            OnStateExited?.Invoke(stateHash);
        }
    }
}