using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Elements
{
    public class StarsBar : MonoBehaviour
    {
        public Image ImageCurrent;

        public void SetValue(float current, float max)
        {
            ImageCurrent.fillAmount = current / max;
        }
    }
}