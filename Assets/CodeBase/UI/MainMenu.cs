using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI
{
    public class MainMenu : WindowBase
    {
        protected override void Cleanup()
        {
            base.Cleanup();
        }
        private void Exit()
        {
            Cleanup();
            Application.Quit();
        }
    }
}