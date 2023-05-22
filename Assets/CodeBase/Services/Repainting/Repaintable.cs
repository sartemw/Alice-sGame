using System;
using CodeBase.Fish;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Repainting
{
    public class Repaintable: MonoBehaviour
    {
        public ColorType ColorType;

        public void Painting(Material material)
        {
            Debug.Log("repaint");
            //gameObject.GetComponent<Renderer>().material = material;
        }
    }
}