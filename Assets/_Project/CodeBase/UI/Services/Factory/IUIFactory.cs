using System.Threading.Tasks;
using _Project.CodeBase.Services;

namespace _Project.CodeBase.UI.Services.Factory
{
  public interface IUIFactory: IService
  {
    Task CreateUIRoot();
    void CreateShop();
    void CreateLevelsProgress();
    Task CreateMainMenu();
    void CreateGameMenu();
    void CreateCheatsMenu();
  }
}