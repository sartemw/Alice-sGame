using CodeBase.Services;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
  public class SaveTrigger : MonoBehaviour
  {
    private ISaveLoadService _saveLoadService;
    private AllServices _services;
    
    [Inject]
    public void Construct(AllServices services)
    {
      _services = services;
    }
    
    private void Awake()
    {
      _saveLoadService = _services.Single<ISaveLoadService>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      _saveLoadService.SaveProgress();
      Debug.Log("Progress saved!");
      gameObject.SetActive(false);
    }

  }
}