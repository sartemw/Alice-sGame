using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    public LoadingCurtain CurtainPrefab;
    private Game _game;
    private AllServices _allServices;

    [Inject]
    public void Construct(AllServices allServices)
    {
      _allServices = allServices;
      
      if (_game == null)
        CreateGame();
    }
    
    private void Awake()
    {
      if (_allServices == null)
        return;

      CreateGame();
    }

    public void CreateGame()
    {
      _game = new Game(this, Instantiate(CurtainPrefab), _allServices);
      _game.StateMachine.Enter<BootstrapState>();
      DontDestroyOnLoad(this);
    }
  }
}