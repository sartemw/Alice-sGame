using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.CodeBase.UI.Windows.Cheats
{
    public class CheatPanel : MonoBehaviour
    {
        public Button Minus;
        public Button Plus;
        public TMP_Text Value;
        
        public event Action<float> OnValueChanged; 
        
        public int Multiplier = 1;

        private float _value;
        private void Start()
        {
            Minus.onClick.AddListener(OnMinusClicked);
            Plus.onClick.AddListener(OnPlusClicked);
        }
        
        public void SetValue(float value)
        {
            _value = value;
            Value.text = value.ToString();
        }

        private void OnPlusClicked()
        {
            _value += 1 * Multiplier;
            OnValueChanged?.Invoke(_value);
            Value.text = _value.ToString();
        }

        private void OnMinusClicked()
        {
            _value -= 1 * Multiplier;
            OnValueChanged?.Invoke(_value);
            Value.text = _value.ToString();
        }
    }
}