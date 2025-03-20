using _Project.CodeBase.Events;
using _Project.CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Elements
{
  public class OpenWindowButton : MonoBehaviour
  {
    public Button Button;
    public WindowId WindowId;
    private IWindowService _windowService;

    public void Init(IWindowService windowService) => 
      _windowService = windowService;

    private void Awake() => 
      Button.onClick.AddListener(Open);

    private void Open()
    {
      EventBus.Invoke(new ChangeLevelsButtonClickSignal());
      _windowService.Open(WindowId);
    }
  }
}