using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class GameRunner : MonoBehaviour
  {
    public GameBootstrapper BootstrapperPrefab;
    private AllServices _allServices;

    [Inject]
    public void Construct(AllServices allServices)
    {
      _allServices = allServices;
    }
    
    private void Awake()
    {
      var bootstrapper = FindObjectOfType<GameBootstrapper>();
      
      if(bootstrapper != null) return;

      RunGameAnyScene();
    }

    private void RunGameAnyScene()
    {
      Instantiate(BootstrapperPrefab).Construct(_allServices);
    }
  }
}