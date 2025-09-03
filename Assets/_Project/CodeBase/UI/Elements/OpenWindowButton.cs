using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Analytics;
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
    private IAnalyticsService _analyticsService;

    public void Init(IWindowService windowService, IAnalyticsService analyticsService)
    {
      _windowService = windowService;
      _analyticsService = analyticsService;
    }

    private void Awake() => 
      Button.onClick.AddListener(Open);

    private void Open()
    {
      _analyticsService.SendWindow($"Open window \"{WindowId.ToString()}\"");

      EventBus.Invoke(new ChangeLevelsButtonClickSignal());
      _windowService.Open(WindowId);
    }
  }
}