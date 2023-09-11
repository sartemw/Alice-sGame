using System;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;

    public WindowService(IUIFactory uiFactory)
    {
      _uiFactory = uiFactory;
    }

    public void Open(WindowId windowId)
    {
      switch (windowId)
      {
        case WindowId.None:
          break;
        case WindowId.Shop:
          _uiFactory.CreateShop();
          break;
        case WindowId.SelectLevels:
          _uiFactory.CreateLevelsProgress();
          break;
        case WindowId.MainMenu:
          _uiFactory.CreateMainMenu();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
      }
    }
  }
}