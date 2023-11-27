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
<<<<<<< HEAD
    private ISaveLoadService _saveLoad;
    private AllServices _services;

    public void Construct(IGameStateMachine stateMachine, AllServices services)
=======
    private IPersistentProgressService _progressService;
    private ISaveLoadService _saveLoad;

    public void Construct(IGameStateMachine stateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
>>>>>>> 884faa757ea49c0624f6142f92af3e27e5492eb9
    {
      _services = services;
      _stateMachine = stateMachine;
<<<<<<< HEAD
      
      _saveLoad = _services.Single<ISaveLoadService>();

=======
      _progressService = progressService;
      _saveLoad = saveLoadService;
>>>>>>> 884faa757ea49c0624f6142f92af3e27e5492eb9
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(_triggered)
        return;

      if (other.CompareTag(PlayerTag))
      {
<<<<<<< HEAD
        _services.Single<IPersistentProgressService>().Progress.GameProgressData.LevelCompleted();
=======
        _progressService.Progress.GameProgressData.LevelCompleted();
>>>>>>> 884faa757ea49c0624f6142f92af3e27e5492eb9
        _saveLoad.SaveLevelCompleted();
        _stateMachine.Enter<LoadLevelState, string>(TransferTo);
        _triggered = true;
        Debug.Log("Open level " + _services.Single<IPersistentProgressService>().Progress.GameProgressData.LevelsCompleted);
      }
    }
  }
}