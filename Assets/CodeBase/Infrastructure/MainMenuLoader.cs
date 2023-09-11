using System;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
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