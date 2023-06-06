using System;
using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Services.Repainting
{
    public class ChooseColor : MonoBehaviour
    {
        private void Start()
        {
            var objRenderer = GetComponent<Renderer>();
            var color= GetComponent<Repaintable>().ColorType;
            objRenderer.material.color = color.SwitchColor();
        }
    }
}