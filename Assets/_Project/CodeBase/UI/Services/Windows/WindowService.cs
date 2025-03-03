using System;
using _Project.CodeBase.UI.Services.Factory;

namespace _Project.CodeBase.UI.Services.Windows
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
        case WindowId.GameMenu:
          _uiFactory.CreateGameMenu();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
      }
    }
  }
}