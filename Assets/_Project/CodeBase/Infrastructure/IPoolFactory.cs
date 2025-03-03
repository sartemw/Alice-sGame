using System.Threading.Tasks;
using _Project.CodeBase.StaticData;
using UnityEngine;

namespace _Project.CodeBase.Infrastructure
{
    public interface IPoolFactory
    {
        Task<GameObject> Create(PoolObjectsTypeId pooler, Transform parent);
    }
}