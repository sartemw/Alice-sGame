using System;
using UnityEngine;

namespace _Project.CodeBase.Logic.TilePlatforms
{
    public class CloudPlatform : TilePlatform
    {
        private void Start()
        {
            _paintable.StartPainting += OnPlatformPainting;
            Collider.enabled = false;
        }

        private void OnDestroy()
        {
            _paintable.StartPainting -= OnPlatformPainting;
        }

        private void OnPlatformPainting()
        {
            Collider.enabled = true;
        }
    }
}