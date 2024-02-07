using System;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private const string PlayerTag = "Player";
    public string TransferTo;
    private IGameStateMachine _stateMachine;
    private bool _triggered;
    private ISaveLoadService _saveLoad;
    private IPersistentProgressService _progress;

    public void Construct(IGameStateMachine stateMachine, IPersistentProgressService progress, ISaveLoadService saveLoadService)
    {
      _progress = progress;
      _stateMachine = stateMachine;
      _saveLoad = saveLoadService;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(_triggered)
        return;

      if (other.CompareTag(PlayerTag))
      {
        _progress.Progress.GameProgressData.LevelCompleted();
        _saveLoad.SaveLevelCompleted();
        
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}
