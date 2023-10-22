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
    private IPersistentProgressService _progressService;
    private ISaveLoadService _saveLoad;

    public void Construct(IGameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
    {
      _stateMachine = stateMachine;
      _progressService = progressService;
      _saveLoad = saveLoadService;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(_triggered)
        return;

      if (other.CompareTag(PlayerTag))
      {
        _progressService.Progress.GameProgressData.LevelCompleted();
        _saveLoad.SaveLevelCompleted();
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
      }
    }
  }
}