using System;
using _Project.CodeBase.CameraLogic;
using UnityEngine;

namespace _Project.CodeBase.Services.Parallax
{
    public class ParallaxService : MonoBehaviour, IParallaxService
    {
        public void Initialize(Transform heroTransform) =>
            FindAnyObjectByType<ParallaxBackground>()
                .Initialize(heroTransform);
    }
}