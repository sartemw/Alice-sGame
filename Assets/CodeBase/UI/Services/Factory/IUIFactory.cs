using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory: IService
  {
    Task CreateUIRoot();
    void CreateShop();
    void CreateLevelsProgress();
    Task CreateMainMenu();
  }
}