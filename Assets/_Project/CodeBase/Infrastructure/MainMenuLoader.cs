using _Project.CodeBase.UI.Services.Factory;
using UnityEngine;
using Zenject;

namespace _Project.CodeBase.Infrastructure
{
    public class MainMenuLoader : MonoBehaviour
    {
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        private void Start() => 
            CreatMenu();

        private void CreatMenu() => 
            _uiFactory.CreateMainMenu();
    }
}