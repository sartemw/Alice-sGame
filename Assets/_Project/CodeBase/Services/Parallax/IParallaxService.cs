using UnityEngine;

namespace _Project.CodeBase.Services.Parallax
{
    public interface IParallaxService : IService
    {
        public void Initialize(Transform heroTransform);
    }
}